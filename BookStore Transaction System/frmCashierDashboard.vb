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
            "WHERE created_by = @u AND DATE(t.created_at) = CURDATE() AND t.status <> 'Cancelled' AND t.payment_method <> 'School Cashier'",
            New String() {"@u"}, New Object() {currentuser.UserID})
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        Dim r As DataRow = dt.Rows(0)
        SetValue(lbltotp, Peso & Convert.ToDecimal(r("total")).ToString("N2"))            ' Total Sales (mine, today)
        SetValue(lbltotqprod, Convert.ToInt32(r("n")).ToString("N0"))                      ' Total Transactions Today
        SetValue(lblsalest, Peso & Convert.ToDecimal(r("cash")).ToString("N2"))           ' Cash On Hand Today
        SetValue(lbllowstock, Peso & Convert.ToDecimal(r("sal_amt")).ToString("N2"))      ' Salary Deduction Today (amount)
        Label6.Text = "Salary Deduction Today (" & Convert.ToInt32(r("sal_n")) & ")"       '   ... and the count
    End Sub
    Private Sub LoadPaymentMethods()
        Dim totals As New Dictionary(Of String, Decimal)(
        StringComparer.OrdinalIgnoreCase) From {
        {"Cash", 0D},
        {"Salary Deduction", 0D}
    }

        Dim methodOrder As New List(Of String) From {
        "Cash",
        "Salary Deduction"
    }

        Dim result As DataTable = GetDataTable(
        "SELECT payment_method, " &
        "IFNULL(SUM(total_amount), 0) AS total " &
        "FROM tbl_transactions " &
        "WHERE created_by = @userId " &
        "AND DATE(t.created_at) = CURDATE() AND t.status <> 'Cancelled' AND t.payment_method <> 'School Cashier'" &
        "GROUP BY payment_method " &
        "ORDER BY payment_method",
        New String() {"@userId"},
        New Object() {currentuser.UserID})

        If result Is Nothing Then
            SetDashboardChartMessage(
            chtsalepymeth,
            "Payment data could not be loaded")
            Exit Sub
        End If

        For Each row As DataRow In result.Rows
            Dim methodName As String =
            row("payment_method").ToString().Trim()

            If methodName = "" Then methodName = "Unspecified"

            If Not totals.ContainsKey(methodName) Then
                totals.Add(methodName, 0D)
                methodOrder.Add(methodName)
            End If

            totals(methodName) += Convert.ToDecimal(row("total"))
        Next

        Dim payments As Series = chtsalepymeth.Series("Series1")

        payments.Points.Clear()

        ' Bars keep zero-value payment methods visible.
        payments.ChartType = SeriesChartType.Column
        payments.IsValueShownAsLabel = True
        payments.Label = Peso & "#VAL{N2}"
        payments.ToolTip = "#AXISLABEL: " & Peso & "#VAL{N2}"
        payments.SmartLabelStyle.Enabled = True

        Dim hasAmounts As Boolean = False

        For Each methodName As String In methodOrder
            Dim amount As Decimal = totals(methodName)

            Dim pointIndex As Integer =
            payments.Points.AddXY(
                methodName,
                Convert.ToDouble(amount))

            payments.Points(pointIndex).Color =
            If(methodName = "Cash",
               Color.SeaGreen,
               Color.SteelBlue)

            If amount <> 0D Then hasAmounts = True
        Next

        For Each legend As Legend In chtsalepymeth.Legends
            legend.Enabled = False
        Next

        With chtsalepymeth.ChartAreas(0)
            .AxisX.Enabled = AxisEnabled.True
            .AxisY.Enabled = AxisEnabled.True
            .AxisX.Interval = 1
            .AxisX.LabelStyle.Angle = 0
            .AxisY.Minimum = 0
            .AxisY.Maximum = If(hasAmounts, Double.NaN, 1.0)
            .AxisY.LabelStyle.Format = Peso & "#,##0"
        End With

        SetDashboardChartMessage(
        chtsalepymeth,
        If(hasAmounts, "", "No transactions recorded today"))
    End Sub
    Private Sub LoadSalesByHour()
        Dim totals(23) As Decimal

        Dim result As DataTable = GetDataTable(
        "SELECT HOUR(created_at) AS sale_hour, " &
        "IFNULL(SUM(total_amount), 0) AS total " &
        "FROM tbl_transactions " &
        "WHERE created_by = @userId " &
        "AND DATE(t.created_at) = CURDATE() AND t.status <> 'Cancelled' AND t.payment_method <> 'School Cashier' " &
        "AND HOUR(created_at) BETWEEN 8 AND 17 " &
        "GROUP BY HOUR(created_at)",
        New String() {"@userId"},
        New Object() {currentuser.UserID})

        If result Is Nothing Then
            SetDashboardChartMessage(
            chrtsaleshr,
            "Sales data could not be loaded")
            Exit Sub
        End If

        For Each row As DataRow In result.Rows
            Dim saleHour As Integer = Convert.ToInt32(row("sale_hour"))
            totals(saleHour) = Convert.ToDecimal(row("total"))
        Next

        Dim sales As Series = chrtsaleshr.Series("Series1")

        sales.Points.Clear()
        sales.ChartType = SeriesChartType.Line
        sales.BorderWidth = 3
        sales.MarkerStyle = MarkerStyle.Circle
        sales.MarkerSize = 7
        sales.ToolTip = "#AXISLABEL: " & Peso & "#VAL{N2}"

        Dim hasSales As Boolean = False

        For saleHour As Integer = 8 To 17
            Dim hourLabel As String =
            New DateTime(2000, 1, 1, saleHour, 0, 0).
                ToString("h tt")

            sales.Points.AddXY(
            hourLabel,
            Convert.ToDouble(totals(saleHour)))

            If totals(saleHour) <> 0D Then hasSales = True
        Next

        With chrtsaleshr.ChartAreas(0)
            .AxisX.Interval = 1
            .AxisY.Minimum = 0
            .AxisY.Maximum = If(hasSales, Double.NaN, 1.0)
            .AxisY.LabelStyle.Format = Peso & "#,##0"
        End With

        SetDashboardChartMessage(
        chrtsaleshr,
        If(hasSales, "", "No sales recorded for these hours"))
    End Sub
    Private Sub SetDashboardChartMessage(
    target As Chart,
    message As String)

        Const messageTitleName As String = "DashboardMessage"

        Dim messageTitle As Title =
        target.Titles.FindByName(messageTitleName)

        If message = "" Then
            If messageTitle IsNot Nothing Then
                target.Titles.Remove(messageTitle)
            End If

            Exit Sub
        End If

        If messageTitle Is Nothing Then
            messageTitle = New Title()
            messageTitle.Name = messageTitleName
            messageTitle.Docking = Docking.Top
            messageTitle.ForeColor = Color.DimGray

            target.Titles.Add(messageTitle)
        End If

        messageTitle.Text = message
    End Sub
    Private Sub LoadTopItems()
        Dim result As DataTable = GetDataTable(
        "SELECT p.product_name, SUM(ti.quantity) AS qty " &
        "FROM tbl_transaction_items ti " &
        "INNER JOIN tbl_transactions t " &
        "ON ti.transaction_id = t.transaction_id " &
        "INNER JOIN tbl_product_variants v " &
        "ON ti.variant_id = v.variant_id " &
        "INNER JOIN tbl_products p " &
        "ON v.product_id = p.product_id " &
        "WHERE t.created_by = @userId " &
        "AND DATE(t.created_at) = CURDATE() AND t.status <> 'Cancelled' AND t.payment_method <> 'School Cashier' " &
        "GROUP BY p.product_id, p.product_name " &
        "ORDER BY qty DESC, p.product_name LIMIT 5",
        New String() {"@userId"},
        New Object() {currentuser.UserID})

        If result Is Nothing Then
            SetDashboardChartMessage(
            chtmostpurchased,
            "Product data could not be loaded")
            Exit Sub
        End If

        Dim products As Series = chtmostpurchased.Series("Series1")

        products.Points.Clear()
        products.ChartType = SeriesChartType.Bar
        products.IsValueShownAsLabel = True
        products.Label = "#VAL{N0}"
        products.ToolTip = "#AXISLABEL: #VAL{N0} item(s)"

        If result.Rows.Count = 0 Then
            products.Points.AddXY("No items sold", 0)

            SetDashboardChartMessage(
            chtmostpurchased,
            "No items sold today")
        Else
            For Each row As DataRow In result.Rows
                products.Points.AddXY(
                row("product_name").ToString(),
                Convert.ToDouble(row("qty")))
            Next

            SetDashboardChartMessage(chtmostpurchased, "")
        End If

        With chtmostpurchased.ChartAreas(0)
            .AxisX.IsReversed = True
            .AxisX.Interval = 1
            .AxisY.Minimum = 0
            .AxisY.Maximum =
            If(result.Rows.Count = 0, 1.0, Double.NaN)
            .AxisY.LabelStyle.Format = "#,##0"
        End With
    End Sub
    Private Sub LoadReadyForRelease()
        Dim result As DataTable = GetDataTable(
            "SELECT t.transaction_no, t.or_no, t.buyer_name, " &
            "p.product_name, v.size, ti.quantity AS qty " &
            "FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_transactions t " &
            "ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v " &
            "ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p " &
            "ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct " &
            "ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c " &
            "ON ct.category_id = c.category_id " &
            "WHERE c.category_name = 'Modules' " &
            "AND ti.released_at IS NULL " &
            "AND t.status NOT IN ('Cancelled', 'Returned', 'Exchanged') " &
            "AND NULLIF(TRIM(t.or_no), '') IS NOT NULL " &
            "AND v.quantity_on_hand >= ti.quantity " &
            "ORDER BY t.created_at, ti.transaction_item_id")

        If result Is Nothing Then
            dgvreadyreleased.Rows.Clear()
            Label15.Text = "MODULE RELEASE DATA UNAVAILABLE"
            Exit Sub
        End If

        dgvreadyreleased.SuspendLayout()

        Try
            dgvreadyreleased.Rows.Clear()

            For Each row As DataRow In result.Rows
                Dim itemName As String =
                    row("product_name").ToString()

                Dim itemSize As String =
                    row("size").ToString().Trim()

                If itemSize <> "" AndAlso
                    Not String.Equals(
                        itemSize,
                        "N/A",
                        StringComparison.OrdinalIgnoreCase) Then

                    itemName &= " (" & itemSize & ")"
                End If

                dgvreadyreleased.Rows.Add(
                    row("transaction_no").ToString(),
                    row("or_no").ToString(),
                    row("buyer_name").ToString(),
                    itemName,
                    row("qty").ToString())
            Next

            dgvreadyreleased.ClearSelection()
        Finally
            dgvreadyreleased.ResumeLayout()
        End Try

        Label15.Text =
            "MODULES READY FOR RELEASE (" &
            result.Rows.Count.ToString() & ")"
    End Sub
End Class