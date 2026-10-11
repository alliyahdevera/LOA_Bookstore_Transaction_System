Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmCashierDashboard

    Private ReadOnly Peso As String = ChrW(&H20B1)
    Private WithEvents tmrRefresh As New System.Windows.Forms.Timer With {.Interval = 60000}
    Private ReadOnly tip As New ToolTip()

    Private Sub frmCashierDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        RoundPanel(pnlcash)
        RoundPanel(pnlmostpurchasedprod)
        RoundPanel(pnlpaymeth)
        RoundPanel(pnlreadyfr)
        RoundPanel(pnlsalarydeduct)
        RoundPanel(pnlsalehr)
        RoundPanel(pnltots)
        RoundPanel(pnltottrant)

        Label11.Text = "Active School Year: " & ActiveSchoolYearLabel()

        ' one-series charts do not need the "Series1" legend
        chrtsaleshr.Legends(0).Enabled = False
        chtmostpurchased.Legends(0).Enabled = False

        tip.SetToolTip(pnltots, "Total of the transactions you processed today (cancelled ones are not counted).")
        tip.SetToolTip(pnltottrant, "Number of transactions you processed today.")
        tip.SetToolTip(pnlcash, "Cash you collected today. This is what the Remittance and the denomination count must match.")
        tip.SetToolTip(pnlsalarydeduct, "Employee salary-deduction sales today: the count is in the title, the amount is the value.")

        With dgvreadyreleased
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With

        RefreshDashboard()
        tmrRefresh.Start()
    End Sub

    Private Sub frmCashierDashboard_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        tmrRefresh.Stop()
    End Sub

    Private Sub tmrRefresh_Tick(sender As Object, e As EventArgs) Handles tmrRefresh.Tick
        RefreshDashboard()
    End Sub

    Private Sub RefreshDashboard()
        LoadCards()
        LoadPaymentMethods()
        LoadSalesByHour()
        LoadTopItems()
        LoadReadyForRelease()
    End Sub

    ' value in a card; long amounts get a smaller font so they stay inside
    Private Sub SetValue(lbl As Label, text As String)
        lbl.Text = text
        Dim size As Single = If(text.Length > 11, 15.0F, If(text.Length > 9, 18.0F, 24.0F))
        If lbl.Font.Size <> size Then lbl.Font = New Font(lbl.Font.FontFamily, size, FontStyle.Bold)
    End Sub

    ' ==================== CARDS ====================
    Private Sub LoadCards()
        Dim dt As DataTable = GetDataTable(
            "SELECT COUNT(*) AS n, IFNULL(SUM(total_amount), 0) AS total, " &
            "IFNULL(SUM(CASE WHEN payment_method = 'Cash' THEN total_amount ELSE 0 END), 0) AS cash, " &
            "IFNULL(SUM(CASE WHEN payment_method = 'Salary Deduction' THEN 1 ELSE 0 END), 0) AS sal_n, " &
            "IFNULL(SUM(CASE WHEN payment_method = 'Salary Deduction' THEN total_amount ELSE 0 END), 0) AS sal_amt " &
            "FROM tbl_transactions " &
            "WHERE created_by = @u AND DATE(created_at) = CURDATE() AND status <> 'Cancelled'",
            New String() {"@u"}, New Object() {currentuser.UserID})
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        Dim r As DataRow = dt.Rows(0)
        SetValue(lbltotp, Peso & Convert.ToDecimal(r("total")).ToString("N2"))            ' Total Sales (mine, today)
        SetValue(lbltotqprod, Convert.ToInt32(r("n")).ToString("N0"))                      ' Total Transactions Today
        SetValue(lblsalest, Peso & Convert.ToDecimal(r("cash")).ToString("N2"))           ' Cash On Hand Today
        SetValue(lbllowstock, Peso & Convert.ToDecimal(r("sal_amt")).ToString("N2"))      ' Salary Deduction Today (amount)
        Label6.Text = "Salary Deduction Today (" & Convert.ToInt32(r("sal_n")) & ")"       '   ... and the count
    End Sub

    ' ==================== SALES BY PAYMENT METHOD ====================
    Private Sub LoadPaymentMethods()
        Dim dt As DataTable = GetDataTable(
            "SELECT payment_method, SUM(total_amount) AS total FROM tbl_transactions " &
            "WHERE created_by = @u AND DATE(created_at) = CURDATE() AND status <> 'Cancelled' " &
            "GROUP BY payment_method ORDER BY total DESC",
            New String() {"@u"}, New Object() {currentuser.UserID})

        Dim s As Series = chtsalepymeth.Series("Series1")
        s.Points.Clear()
        s.IsValueShownAsLabel = True
        s.Label = Peso & "#VAL{N0}\n(#PERCENT{P0})"
        s.ToolTip = "#AXISLABEL: " & Peso & "#VAL{N2} (#PERCENT{P1})"
        s.SmartLabelStyle.Enabled = True
        If dt Is Nothing Then Exit Sub

        Dim grand As Double = 0
        For Each r As DataRow In dt.Rows
            grand += Convert.ToDouble(r("total"))
        Next
        For Each r As DataRow In dt.Rows
            Dim amt As Double = Convert.ToDouble(r("total"))
            Dim idx As Integer = s.Points.AddXY(r("payment_method").ToString(), amt)
            s.Points(idx).LegendText = r("payment_method").ToString() & " - " & Peso & amt.ToString("N2") &
                                       " (" & If(grand > 0, amt / grand, 0).ToString("P0") & ")"
        Next
    End Sub

    ' ==================== SALES BY HOUR ====================
    Private Sub LoadSalesByHour()
        Dim totals(23) As Decimal
        Dim firstH As Integer = 7, lastH As Integer = 18               ' usual store hours, widened if a sale falls outside

        Dim dt As DataTable = GetDataTable(
            "SELECT HOUR(created_at) AS h, SUM(total_amount) AS total FROM tbl_transactions " &
            "WHERE created_by = @u AND DATE(created_at) = CURDATE() AND status <> 'Cancelled' GROUP BY HOUR(created_at)",
            New String() {"@u"}, New Object() {currentuser.UserID})
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                Dim h As Integer = Convert.ToInt32(r("h"))
                totals(h) = Convert.ToDecimal(r("total"))
                firstH = Math.Min(firstH, h)
                lastH = Math.Max(lastH, h)
            Next
        End If

        Dim s As Series = chrtsaleshr.Series("Series1")
        s.Points.Clear()
        s.BorderWidth = 3
        s.MarkerStyle = MarkerStyle.Circle
        s.MarkerSize = 7
        s.ToolTip = "#AXISLABEL: " & Peso & "#VAL{N2}"
        For h As Integer = firstH To lastH
            s.Points.AddXY(New DateTime(2000, 1, 1, h, 0, 0).ToString("h tt"), Convert.ToDouble(totals(h)))
        Next
        chrtsaleshr.ChartAreas(0).AxisX.Interval = 1
        chrtsaleshr.ChartAreas(0).AxisY.LabelStyle.Format = Peso & "#,##0"
    End Sub

    ' ==================== TOP 5 ITEMS TODAY ====================
    Private Sub LoadTopItems()
        Dim dt As DataTable = GetDataTable(
            "SELECT p.product_name, SUM(ti.quantity) AS qty " &
            "FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_transactions t ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "WHERE t.created_by = @u AND DATE(t.created_at) = CURDATE() AND t.status <> 'Cancelled' " &
            "GROUP BY p.product_id, p.product_name ORDER BY qty DESC LIMIT 5",
            New String() {"@u"}, New Object() {currentuser.UserID})

        Dim s As Series = chtmostpurchased.Series("Series1")
        s.Points.Clear()
        s.IsValueShownAsLabel = True
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                s.Points.AddXY(r("product_name").ToString(), Convert.ToDouble(r("qty")))
            Next
        End If
        chtmostpurchased.ChartAreas(0).AxisX.IsReversed = True            ' biggest on top
        chtmostpurchased.ChartAreas(0).AxisX.Interval = 1
    End Sub

    ' ==================== READY FOR RELEASE ====================
    ' paid pick-up items whose stock has arrived and are waiting to be handed over
    Private Sub LoadReadyForRelease()
        Dim dt As DataTable = GetDataTable(
            "SELECT t.transaction_no, t.or_no, t.buyer_name, p.product_name, v.size, ti.quantity AS qty, ti.pickup_date " &
            "FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_transactions t ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "WHERE ti.is_backorder = 1 AND ti.released_at IS NULL AND t.status <> 'Cancelled' " &
            "AND v.quantity_on_hand >= ti.quantity " &
            "ORDER BY ti.pickup_date, ti.transaction_item_id")

        dgvreadyreleased.SuspendLayout()
        dgvreadyreleased.Rows.Clear()
        Dim overdue As Integer = 0
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                Dim sz As String = r("size").ToString()
                Dim idx As Integer = dgvreadyreleased.Rows.Add(
                    r("transaction_no").ToString(), r("or_no").ToString(), r("buyer_name").ToString(),
                    r("product_name").ToString() & If(sz <> "" AndAlso sz <> "N/A", " (" & sz & ")", ""),
                    r("qty").ToString())
                If Not IsDBNull(r("pickup_date")) AndAlso Convert.ToDateTime(r("pickup_date")).Date < Date.Today Then
                    dgvreadyreleased.Rows(idx).DefaultCellStyle.ForeColor = Color.Firebrick        ' waiting past the pick-up date
                    overdue += 1
                End If
            Next
        End If
        dgvreadyreleased.ClearSelection()
        dgvreadyreleased.ResumeLayout()

        Dim n As Integer = If(dt Is Nothing, 0, dt.Rows.Count)
        Label15.Text = "READY FOR RELEASE ITEMS (" & n & If(overdue > 0, ", " & overdue & " overdue", "") & ")"
    End Sub

End Class