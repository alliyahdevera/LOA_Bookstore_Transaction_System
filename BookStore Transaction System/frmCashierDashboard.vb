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
        chtsalepymeth.Legends(0).Enabled = False
        StyleChart(chrtsaleshr)
        StyleChart(chtmostpurchased)
        StyleChart(chtsalepymeth)

        tip.SetToolTip(pnltots, "Total cash sales of all cashiers today (cancelled sales and pending salary deductions are not counted).")
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
        Dim cash As DataTable = GetDataTable(
        "SELECT COUNT(*) AS transaction_count, " &
        "IFNULL(SUM(total_amount), 0) AS total " &
        "FROM tbl_transactions " &
        "WHERE created_by = @userId " &
        "AND DATE(created_at) = CURDATE() " &
        "AND status <> 'Cancelled' " &
        "AND payment_method = 'Cash'",
        New String() {"@userId"},
        New Object() {currentuser.UserID})

        Dim pending As DataTable = GetDataTable(
        "SELECT COUNT(*) AS transaction_count, " &
        "IFNULL(SUM(total_amount), 0) AS total " &
        "FROM tbl_transactions " &
        "WHERE created_by = @userId " &
        "AND DATE(created_at) = CURDATE() " &
        "AND payment_method = 'Salary Deduction' " &
        "AND status = 'Pending Finance'",
        New String() {"@userId"},
        New Object() {currentuser.UserID})

        Dim allSales As DataTable = GetDataTable(
        "SELECT IFNULL(SUM(total_amount), 0) AS total " &
        "FROM tbl_transactions " &
        "WHERE DATE(created_at) = CURDATE() " &
        "AND status <> 'Cancelled' " &
        "AND payment_method = 'Cash'",
        New String() {},
        New Object() {})

        If cash Is Nothing OrElse cash.Rows.Count = 0 OrElse
           pending Is Nothing OrElse pending.Rows.Count = 0 OrElse
           allSales Is Nothing OrElse allSales.Rows.Count = 0 Then
            Exit Sub
        End If

        Dim cashAmount As Decimal = Convert.ToDecimal(cash.Rows(0)("total"))

        SetValue(lbltotp, Peso & Convert.ToDecimal(allSales.Rows(0)("total")).ToString("N2"))
        SetValue(lblsalest, Peso & cashAmount.ToString("N2"))

        SetValue(lbltotqprod,
                 Convert.ToInt32(cash.Rows(0)("transaction_count")).ToString("N0"))

        SetValue(lbllowstock,
                 Peso & Convert.ToDecimal(pending.Rows(0)("total")).ToString("N2"))

        ' Label6 is the caption under the salary-deduction card
        Label6.Text = "Pending Finance (" &
                      Convert.ToInt32(pending.Rows(0)("transaction_count")).ToString() & ")"

        tip.SetToolTip(pnlsalarydeduct,
            "Unpaid salary-deduction requests awaiting finance approval. " &
            "Excluded from daily cash totals.")
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
        "FROM tbl_transactions t " &
        "WHERE created_by = @userId " &
        "AND DATE(t.created_at) = CURDATE() AND t.status <> 'Cancelled' AND t.payment_method <> 'School Cashier' " &
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
        payments.ChartType = SeriesChartType.Pie        ' use SeriesChartType.Doughnut for a doughnut
        payments.IsValueShownAsLabel = False
        payments.BorderColor = Color.White
        payments.BorderWidth = 2
        payments("PieStartAngle") = "270"
        payments("PieLabelStyle") = "Inside"

        Dim total As Decimal = 0D
        For Each methodName As String In methodOrder
            total += totals(methodName)
        Next

        Dim hasAmounts As Boolean = (total <> 0D)

        If hasAmounts Then
            For Each methodName As String In methodOrder
                Dim amount As Decimal = totals(methodName)

                Dim pointIndex As Integer =
                    payments.Points.AddXY(methodName, Convert.ToDouble(amount))

                Dim slice As DataPoint = payments.Points(pointIndex)

                Select Case methodName
                    Case "Cash" : slice.Color = Color.SeaGreen
                    Case "Salary Deduction" : slice.Color = Color.SteelBlue
                    Case Else : slice.Color = Color.Gray
                End Select

                slice.LegendText = methodName & "  " & Peso & amount.ToString("N2")
                slice.ToolTip = methodName & ": " & Peso & amount.ToString("N2") &
                                " (" & (amount / total).ToString("P0") & ")"

                ' a zero slice has no area, so no label for it; it still appears in the legend
                If amount = 0D Then
                    slice.Label = ""
                Else
                    slice.Label = (amount / total).ToString("P0")
                    slice.LabelForeColor = Color.White
                    slice.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                End If
            Next
        End If

        If chtsalepymeth.Legends.Count = 0 Then
            chtsalepymeth.Legends.Add(New Legend("Legend1"))
        End If

        For Each chartLegend As Legend In chtsalepymeth.Legends
            chartLegend.Enabled = hasAmounts
            chartLegend.Docking = Docking.Bottom
        Next

        With chtsalepymeth.ChartAreas(0)
            .AxisX.Enabled = AxisEnabled.False
            .AxisY.Enabled = AxisEnabled.False
            .AxisX.MajorGrid.Enabled = False
            .AxisY.MajorGrid.Enabled = False
            .AxisY.Minimum = Double.NaN
            .AxisY.Maximum = Double.NaN
            .Area3DStyle.Enable3D = False
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
        "FROM tbl_transactions t " &
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
    target As System.Windows.Forms.DataVisualization.Charting.Chart,
    message As String)

        Const messageTitleName As String = "DashboardMessage"

        Dim messageTitle As System.Windows.Forms.DataVisualization.Charting.Title =
        target.Titles.FindByName(messageTitleName)

        If message = "" Then
            If messageTitle IsNot Nothing Then
                target.Titles.Remove(messageTitle)
            End If

            Exit Sub
        End If

        If messageTitle Is Nothing Then
            messageTitle = New System.Windows.Forms.DataVisualization.Charting.Title()
            messageTitle.Name = messageTitleName
            messageTitle.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top
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