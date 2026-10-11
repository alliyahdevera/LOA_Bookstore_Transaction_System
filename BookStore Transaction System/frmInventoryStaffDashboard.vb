Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmInventoryStaffDashboard

    Private WithEvents tmrRefresh As New System.Windows.Forms.Timer With {.Interval = 60000}

    Private Sub frmInventoryStaffDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        RoundPanel(pnldistrsale)
        RoundPanel(pnllowprod)
        RoundPanel(pnllowstock)
        RoundPanel(pnlmostpurchasedprod)
        RoundPanel(pnlsalespmonth)
        RoundPanel(pnlsalestoday)
        RoundPanel(pnltotprod)
        RoundPanel(pnltotqprod)

        Label11.Text = "Active School Year: " & ActiveSchoolYearLabel()

        ' each chart has a single series, so hide the "Series1" legend
        For Each ch As Chart In New Chart() {chrtlowlevlprod, chtmostpurchased, chtdistsales}
            If ch.Legends.Count > 0 Then ch.Legends(0).Enabled = False
        Next

        With dgvstockin
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With

        RefreshDashboard()
        tmrRefresh.Start()
    End Sub

    Private Sub frmInventoryStaffDashboard_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        tmrRefresh.Stop()
    End Sub

    Private Sub tmrRefresh_Tick(sender As Object, e As EventArgs) Handles tmrRefresh.Tick
        RefreshDashboard()
    End Sub

    Private Sub RefreshDashboard()
        LoadCards()
        LoadMovementChart()
        LoadCategoryLevels()
        LoadLowStockChart()
        LoadRecentStockIns()
    End Sub

    Private Sub SetValue(lbl As Label, text As String)
        lbl.Text = text
        Dim size As Single = If(text.Length > 11, 15.0F, If(text.Length > 8, 18.0F, 24.0F))
        If lbl.Font.Size <> size Then lbl.Font = New Font(lbl.Font.FontFamily, size, FontStyle.Bold)
    End Sub

    Private Function Count(sql As String) As Integer
        Return Convert.ToInt32(If(ExecScalar(sql), 0))
    End Function

    ' ==================== CARDS (captions come from the designer) ====================
    Private Sub LoadCards()
        Const ACTIVE_VARIANTS As String =
            "FROM tbl_product_variants v INNER JOIN tbl_products p ON v.product_id = p.product_id WHERE p.status = 'Active' "

        ' Out of Stock Items
        SetValue(lbltotp, Count("SELECT COUNT(*) " & ACTIVE_VARIANTS & "AND v.quantity_on_hand = 0").ToString("N0"))

        ' Low Stock Items
        SetValue(lbltotqprod, Count("SELECT COUNT(*) " & ACTIVE_VARIANTS &
                                    "AND v.quantity_on_hand > 0 AND v.quantity_on_hand <= v.reorder_level").ToString("N0"))

        ' Damaged Items (pieces currently held as non-saleable)
        SetValue(lblsalest, Count("SELECT IFNULL(SUM(quantity), 0) FROM tbl_nonsaleable_stocks WHERE status = 'Nonsaleable'").ToString("N0"))

        ' Stocked In Today (pieces received today)
        SetValue(lbllowstock, Count(
            "SELECT IFNULL(SUM(sid.quantity), 0) FROM tbl_stock_in_details sid " &
            "INNER JOIN tbl_stock_ins si ON sid.stock_in_id = si.stock_in_id WHERE si.stock_in_date = CURDATE()").ToString("N0"))
    End Sub

    ' ==================== STOCK MOVEMENT: last 7 days (pieces in + pieces out) ====================
    Private Sub LoadMovementChart()
        Dim startDay As Date = Date.Today.AddDays(-6)
        Dim stockIn(6) As Integer
        Dim stockOut(6) As Integer

        Dim a As DataTable = GetDataTable(
            "SELECT si.stock_in_date AS d, SUM(sid.quantity) AS q " &
            "FROM tbl_stock_in_details sid INNER JOIN tbl_stock_ins si ON sid.stock_in_id = si.stock_in_id " &
            "WHERE si.stock_in_date >= @f GROUP BY si.stock_in_date",
            New String() {"@f"}, New Object() {startDay})
        If a IsNot Nothing Then
            For Each r As DataRow In a.Rows
                Dim i As Integer = CInt((Convert.ToDateTime(r("d")).Date - startDay).TotalDays)
                If i >= 0 AndAlso i <= 6 Then stockIn(i) = Convert.ToInt32(r("q"))
            Next
        End If

        ' "out" = items that left the shelf (sold at the counter or released from pick-up)
        Dim b As DataTable = GetDataTable(
            "SELECT DATE(ti.released_at) AS d, SUM(ti.quantity) AS q " &
            "FROM tbl_transaction_items ti INNER JOIN tbl_transactions t ON ti.transaction_id = t.transaction_id " &
            "WHERE ti.released_at IS NOT NULL AND DATE(ti.released_at) >= @f AND t.status <> 'Cancelled' " &
            "GROUP BY DATE(ti.released_at)",
            New String() {"@f"}, New Object() {startDay})
        If b IsNot Nothing Then
            For Each r As DataRow In b.Rows
                Dim i As Integer = CInt((Convert.ToDateTime(r("d")).Date - startDay).TotalDays)
                If i >= 0 AndAlso i <= 6 Then stockOut(i) = Convert.ToInt32(r("q"))
            Next
        End If

        Dim s As Series = chrtlowlevlprod.Series("Series1")
        s.Points.Clear()
        For i As Integer = 0 To 6
            Dim label As String = startDay.AddDays(i).ToString("MMM d")
            Dim idx As Integer = s.Points.AddXY(label, stockIn(i) + stockOut(i))
            s.Points(idx).ToolTip = label & ": " & stockIn(i) & " pc(s) in, " & stockOut(i) & " pc(s) out"
        Next
        chrtlowlevlprod.ChartAreas(0).AxisX.Interval = 1
        chrtlowlevlprod.ChartAreas(0).AxisY.LabelStyle.Format = "N0"
    End Sub

    ' ==================== STOCK LEVELS BY CATEGORY ====================
    Private Sub LoadCategoryLevels()
        Dim dt As DataTable = GetDataTable(
            "SELECT c.category_name, IFNULL(SUM(v.quantity_on_hand), 0) AS qty " &
            "FROM tbl_product_variants v " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "WHERE p.status = 'Active' GROUP BY c.category_id, c.category_name ORDER BY qty DESC")

        Dim s As Series = chtmostpurchased.Series("Series1")
        s.Points.Clear()
        s.IsValueShownAsLabel = True
        s.ToolTip = "#AXISLABEL: #VAL pc(s) on hand"
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                s.Points.AddXY(r("category_name").ToString(), Convert.ToDouble(r("qty")))
            Next
        End If
        chtmostpurchased.ChartAreas(0).AxisX.IsReversed = True
        chtmostpurchased.ChartAreas(0).AxisX.Interval = 1
    End Sub

    ' ==================== LOW STOCK ITEMS (6 most urgent, pieces on hand) ====================
    Private Sub LoadLowStockChart()
        Dim dt As DataTable = GetDataTable(
            "SELECT p.product_name, v.size, v.quantity_on_hand AS qty " &
            "FROM tbl_product_variants v INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "WHERE p.status = 'Active' AND v.quantity_on_hand <= v.reorder_level " &
            "ORDER BY (v.quantity_on_hand - v.reorder_level), p.product_name LIMIT 6")

        Dim s As Series = chtdistsales.Series("Series1")
        s.Points.Clear()
        s.IsValueShownAsLabel = True
        s.ToolTip = "#AXISLABEL: #VAL pc(s) on hand"
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                Dim sz As String = r("size").ToString()
                Dim nm As String = r("product_name").ToString() & If(sz <> "" AndAlso sz <> "N/A", " (" & sz & ")", "")
                s.Points.AddXY(nm, Convert.ToInt32(r("qty")))
            Next
        End If
        chtdistsales.ChartAreas(0).AxisX.Interval = 1
        chtdistsales.ChartAreas(0).AxisX.LabelStyle.Angle = -25
    End Sub

    ' ==================== RECENT STOCK INS ====================
    Private Sub LoadRecentStockIns()
        Dim dt As DataTable = GetDataTable(
            "SELECT si.reference_no, v.product_code, p.product_name, c.category_name, " &
            "TIMESTAMP(si.stock_in_date, si.stock_in_time) AS stock_in_at, si.received_by " &
            "FROM tbl_stock_in_details sid " &
            "INNER JOIN tbl_stock_ins si ON sid.stock_in_id = si.stock_in_id " &
            "INNER JOIN tbl_product_variants v ON sid.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "ORDER BY si.stock_in_date DESC, si.stock_in_time DESC, si.stock_in_id DESC LIMIT 10")

        dgvstockin.SuspendLayout()
        dgvstockin.Rows.Clear()
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                dgvstockin.Rows.Add(
                    r("reference_no").ToString(), r("product_code").ToString(), r("product_name").ToString(),
                    r("category_name").ToString(), Convert.ToDateTime(r("stock_in_at")).ToString("MMM d, yyyy h:mm tt"),
                    r("received_by").ToString())
            Next
        End If
        dgvstockin.ClearSelection()
        dgvstockin.ResumeLayout()
    End Sub

End Class