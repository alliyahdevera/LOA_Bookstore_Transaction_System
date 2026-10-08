Imports System.Drawing.Drawing2D
Imports MySql.Data.MySqlClient

Public Class frmDashboard
    Private Const QTY_SOLD As String = "ti.quantity"

    Private WithEvents tmrClock As System.Windows.Forms.Timer
    Private WithEvents cboSchoolYear As ComboBox
    Private WithEvents btnManageSY As Button
    Private isLoadingSY As Boolean = False

    Private Sub BuildSchoolYearSelector()
        EnsureSchoolYearInitialized()

        Dim lbl As New Label With {.Text = "School Year", .AutoSize = True, .BackColor = Color.Transparent,
                                   .Font = New Font("Segoe UI", 9.75!, FontStyle.Bold), .Location = New Point(850, 32)}
        cboSchoolYear = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList,
                                           .Font = New Font("Segoe UI", 9.75!), .Location = New Point(940, 28), .Width = 150}
        btnManageSY = New Button With {.Text = "Manage", .Location = New Point(1098, 27), .Size = New Size(80, 28),
                                       .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand,
                                       .Visible = String.Equals(currentuser.Role, ROLE_SUPERVISOR, StringComparison.OrdinalIgnoreCase)}
        Controls.Add(lbl)
        Controls.Add(cboSchoolYear)
        Controls.Add(btnManageSY)
        lbl.BringToFront() : cboSchoolYear.BringToFront() : btnManageSY.BringToFront()
        FillSchoolYearCombo()
    End Sub

    Private Sub FillSchoolYearCombo()
        isLoadingSY = True
        Dim years As List(Of SchoolYearInfo) = GetSchoolYears()

        ' keep the same school year selected (re-read, in case its dates were edited)
        If SelectedSchoolYear IsNot Nothing Then
            Dim keepId As Integer = SelectedSchoolYear.Id
            SelectSchoolYear(years.Find(Function(y) y.Id = keepId))
        End If

        cboSchoolYear.Items.Clear()
        cboSchoolYear.Items.Add("All School Years")
        For Each sy As SchoolYearInfo In years
            cboSchoolYear.Items.Add(sy)
        Next
        cboSchoolYear.SelectedIndex = 0
        If SelectedSchoolYear IsNot Nothing Then
            For i As Integer = 1 To cboSchoolYear.Items.Count - 1
                If DirectCast(cboSchoolYear.Items(i), SchoolYearInfo).Id = SelectedSchoolYear.Id Then
                    cboSchoolYear.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
        isLoadingSY = False
    End Sub

    Private Sub cboSchoolYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSchoolYear.SelectedIndexChanged
        If isLoadingSY Then Exit Sub
        SelectSchoolYear(TryCast(cboSchoolYear.SelectedItem, SchoolYearInfo))    ' "All School Years" gives Nothing
        RefreshDashboard()
    End Sub

    Private Sub btnManageSY_Click(sender As Object, e As EventArgs) Handles btnManageSY.Click
        Using f As New frmSchoolYears()
            f.ShowDialog(Me)
        End Using
        FillSchoolYearCombo()
        RefreshDashboard()
    End Sub

    Private Sub UpdateCaptions()
        Dim tag As String = "  -  " & SchoolYearLabel()
        Label12.AutoSize = True : Label12.Text = "TOP 5 MOST PURCHASED PRODUCT" & tag
        Label13.AutoSize = True : Label13.Text = "DISTRIBUTION SALES" & tag
        Label15.AutoSize = True : Label15.Text = "SALES PER MONTH" & tag
    End Sub
    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            lblname.Text = currentuser.FullName
            lblposition.Text = currentuser.Role

            RoundPanel(pnltotprod)
            RoundPanel(pnltotqprod)
            RoundPanel(pnllowstock)
            RoundPanel(pnlsalestoday)
            RoundPanel(pnldistrsale)
            RoundPanel(pnlmostpurchasedprod)
            RoundPanel(pnlsalespmonth)
            RoundPanel(pnllowprod)

            tmrClock = New System.Windows.Forms.Timer()
            tmrClock.Interval = 1000
            tmrClock.Start()
            UpdateFooterDateTime()
            BuildSchoolYearSelector()
            RefreshDashboard()
            RefreshDashboard()
        Catch ex As Exception
            MsgBox("Error initializing Dashboard Form: " & ex.Message, vbCritical, "Init Error")
        End Try
    End Sub
    Private Sub frmDashboard_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
        If tmrClock IsNot Nothing Then
            tmrClock.Stop()
            tmrClock.Dispose()
        End If
    End Sub

    Private Sub tmrClock_Tick(sender As Object, e As EventArgs) Handles tmrClock.Tick
        UpdateFooterDateTime()
    End Sub

    Private Sub UpdateFooterDateTime()
        lbldatetime.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy h:mm:ss tt")
    End Sub

    Public Sub RefreshDashboard()
        UpdateCaptions()
        If Not connection() Then Exit Sub
        cn.Close()

        UpdateCaptions()
        LoadTotals()
        LoadMostBoughtProducts()
        LoadProductSales()
        LoadCriticalProducts()
        LoadSalesPerMonth()
    End Sub

    ' ------------------------------------------------------------------
    ' The four cards
    ' ------------------------------------------------------------------
    Private Sub LoadTotals()
        lbltotp.Text = GetScalar("SELECT COUNT(*) FROM TBL_PRODUCTS").ToString("N0")

        lbltotqprod.Text = GetScalar("SELECT IFNULL(SUM(quantity_on_hand), 0) FROM TBL_PRODUCT_VARIANTS").ToString("N0")

        lblsalest.Text = ChrW(8369) & GetScalar("SELECT IFNULL(SUM(total_amount), 0) FROM TBL_TRANSACTIONS WHERE status <> 'Cancelled'" & SchoolYearFilter("or_date")).ToString("N2")

        lbllowstock.Text = GetScalar("SELECT COUNT(*) FROM TBL_PRODUCT_VARIANTS WHERE quantity_on_hand <= reorder_level").ToString("N0")
    End Sub

    ' Keeps the big number centered in its card no matter how many digits it has
    Private Sub CenterLabel(lbl As Label)
        If lbl.Parent IsNot Nothing Then
            lbl.Left = (lbl.Parent.ClientSize.Width - lbl.Width) \ 2
        End If
    End Sub

    Private Function GetScalar(query As String) As Decimal
        Dim result As Decimal = 0
        Try
            If Not connection() Then Return 0
            Using localCmd As New MySqlCommand(query, cn)
                Dim value As Object = localCmd.ExecuteScalar()
                If value IsNot Nothing AndAlso Not IsDBNull(value) Then
                    result = Convert.ToDecimal(value)
                End If
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading dashboard totals: " & ex.Message, vbCritical, "Error")
        End Try
        Return result
    End Function

    Private Sub LoadMostBoughtProducts()
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT p.product_name, SUM(" & QTY_SOLD & ") AS qty_sold " &
                      "FROM TBL_TRANSACTION_ITEMS ti " &
                      "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
                      "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                      "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                            "WHERE t.status <> 'Cancelled' " & SchoolYearFilter("t.or_date") &
                      "GROUP BY p.product_id, p.product_name " &
                      "ORDER BY qty_sold DESC " &
                      "LIMIT 5"

            Using localCmd As New MySqlCommand(query, cn)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    chtmostpurchased.Series("Series1").Points.Clear()
                    chtmostpurchased.Series("Series1").IsValueShownAsLabel = True
                    chtmostpurchased.ChartAreas(0).AxisX.IsReversed = True   ' best seller on top
                    chtmostpurchased.ChartAreas(0).AxisX.Interval = 1
                    While localDr.Read()
                        chtmostpurchased.Series("Series1").Points.AddXY(localDr("product_name").ToString(), Convert.ToInt32(localDr("qty_sold")))
                    End While
                End Using
            End Using

            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading most bought products chart: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub LoadProductSales()
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT c.category_name, SUM(ti.subtotal) AS total_sales " &
                                  "FROM TBL_TRANSACTION_ITEMS ti " &
                                  "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
                                  "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
                                  "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
                                  "WHERE t.status <> 'Cancelled' " & SchoolYearFilter("t.or_date") &
                                  "GROUP BY c.category_id, c.category_name " &
                                  "ORDER BY total_sales DESC"

            Using localCmd As New MySqlCommand(query, cn)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    With chtdistsales.Series("Series1")
                        .Points.Clear()
                        .IsValueShownAsLabel = True
                        .Label = "#PERCENT{P0}"          ' show percentage on each slice
                        .LegendText = "#AXISLABEL"       ' legend shows the category name
                        While localDr.Read()
                            .Points.AddXY(localDr("category_name").ToString(), Convert.ToDouble(localDr("total_sales")))
                        End While
                    End With
                End Using
            End Using

            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading product sales chart: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub LoadCriticalProducts()
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT CONCAT(p.product_name, IF(v.size = 'N/A', '', CONCAT(' (', v.size, ')'))) AS item_name, " &
                                  "v.quantity_on_hand, v.reorder_level " &
                                  "FROM TBL_PRODUCT_VARIANTS v " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "WHERE v.quantity_on_hand <= v.reorder_level " &
                                  "ORDER BY v.quantity_on_hand ASC " &
                                  "LIMIT 6"

            Using localCmd As New MySqlCommand(query, cn)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    chrtlowlevlprod.Series("Series1").Points.Clear()
                    chrtlowlevlprod.Series("Series1").IsValueShownAsLabel = True
                    chrtlowlevlprod.ChartAreas(0).AxisX.Interval = 1
                    While localDr.Read()
                        Dim idx As Integer = chrtlowlevlprod.Series("Series1").Points.AddXY(localDr("item_name").ToString(), Convert.ToInt32(localDr("quantity_on_hand")))
                        chrtlowlevlprod.Series("Series1").Points(idx).Color = Color.Firebrick
                        chrtlowlevlprod.Series("Series1").Points(idx).ToolTip = "Reorder level: " & localDr("reorder_level").ToString()
                    End While
                End Using
            End Using

            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading critical products chart: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub
    Private Sub LoadSalesPerMonth()
        Try
            If Not connection() Then Exit Sub

            Dim sy As SchoolYearInfo = SelectedSchoolYear
            Dim firstMonth As Date
            Dim monthCount As Integer
            If sy IsNot Nothing Then
                firstMonth = New Date(sy.StartDate.Year, sy.StartDate.Month, 1)
                monthCount = (sy.EndDate.Year - sy.StartDate.Year) * 12 + sy.EndDate.Month - sy.StartDate.Month + 1
            Else
                firstMonth = New Date(DateTime.Today.Year, 1, 1)
                monthCount = 12
            End If

            Dim totals(monthCount - 1) As Decimal

            Dim monthSql As String =
                "SELECT YEAR(or_date) AS y, MONTH(or_date) AS m, SUM(total_amount) AS total " &
                "FROM TBL_TRANSACTIONS WHERE status <> 'Cancelled' " &
                If(sy IsNot Nothing, SchoolYearFilter("or_date"), " AND YEAR(or_date) = " & DateTime.Today.Year & " ") &
                "GROUP BY YEAR(or_date), MONTH(or_date)"

            Using localCmd As New MySqlCommand(monthSql, cn)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    While localDr.Read()
                        Dim idx As Integer = (Convert.ToInt32(localDr("y")) - firstMonth.Year) * 12 + Convert.ToInt32(localDr("m")) - firstMonth.Month
                        If idx >= 0 AndAlso idx < monthCount Then totals(idx) = Convert.ToDecimal(localDr("total"))
                    End While
                End Using
            End Using

            With chrtsalespermonth.Series("Series1")
                .Points.Clear()
                .BorderWidth = 3
                .MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle
                .MarkerSize = 7
                For i As Integer = 0 To monthCount - 1
                    Dim m As Date = firstMonth.AddMonths(i)
                    .Points.AddXY(If(sy IsNot Nothing, m.ToString("MMM yy"), m.ToString("MMM")), Convert.ToDouble(totals(i)))
                Next
            End With
            chrtsalespermonth.ChartAreas(0).AxisX.Interval = 1
            chrtsalespermonth.ChartAreas(0).AxisY.LabelStyle.Format = "N0"

            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading sales per month chart: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub
End Class