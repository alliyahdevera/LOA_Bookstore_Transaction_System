Imports System.Drawing.Drawing2D
Imports MySql.Data.MySqlClient

Public Class frmDashboard
    Private Const QTY_SOLD As String = "ti.quantity"
    Private isLoadingMonth As Boolean = True
    Private WithEvents tmrClock As System.Windows.Forms.Timer

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
            InitMonthCombo()
            RefreshDashboard()
            RefreshDashboard()
        Catch ex As Exception
            MsgBox("Error initializing Dashboard Form: " & ex.Message, vbCritical, "Init Error")
        End Try
    End Sub
    Private Sub InitMonthCombo()
        cbomonth.DropDownStyle = ComboBoxStyle.DropDownList
        cbomonth.Items.Clear()
        cbomonth.Items.Add("Whole Year")
        For m As Integer = 1 To 12
            cbomonth.Items.Add(MonthName(m))
        Next
        cbomonth.SelectedIndex = 0
        cbomonth.BringToFront()
        isLoadingMonth = False
    End Sub

    Private Sub cbomonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbomonth.SelectedIndexChanged
        If isLoadingMonth Then Exit Sub
        LoadSalesPerMonth()
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

    ''' <summary>
    ''' Reloads every card and chart in one call.
    ''' </summary>
    Public Sub RefreshDashboard()
        ' Test the connection once so we don't show one error per widget
        If Not connection() Then Exit Sub
        cn.Close()

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

        lblsalest.Text = ChrW(8369) & GetScalar("SELECT IFNULL(SUM(total_amount), 0) FROM TBL_TRANSACTIONS WHERE DATE(or_date) = CURDATE() AND status <> 'Cancelled'").ToString("N2")

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
                      "WHERE t.status <> 'Cancelled' " &
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
                                  "WHERE t.status <> 'Cancelled' " &
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
        Dim currentYear As Integer = DateTime.Today.Year
        Dim selMonth As Integer = If(cbomonth.SelectedIndex > 0, cbomonth.SelectedIndex, 0)   ' 0 = whole year

        Try
            If Not connection() Then Exit Sub

            Label15.AutoSize = True

            With chrtsalespermonth.Series("Series1")
                .Points.Clear()
                .BorderWidth = 3
                .MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle
                .MarkerSize = 7
            End With

            If selMonth = 0 Then
                ' ---- 12 months of the year ----
                Dim totals(12) As Decimal
                Dim monthSql As String = "SELECT MONTH(or_date) AS m, SUM(total_amount) AS total " &
                                         "FROM TBL_TRANSACTIONS WHERE YEAR(or_date) = @year AND status <> 'Cancelled' " &
                                         "GROUP BY MONTH(or_date)"
                Using localCmd As New MySqlCommand(monthSql, cn)
                    localCmd.Parameters.AddWithValue("@year", currentYear)
                    Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                        While localDr.Read()
                            totals(Convert.ToInt32(localDr("m"))) = Convert.ToDecimal(localDr("total"))
                        End While
                    End Using
                End Using

                For m As Integer = 1 To 12
                    chrtsalespermonth.Series("Series1").Points.AddXY(MonthName(m, True), Convert.ToDouble(totals(m)))
                Next
                chrtsalespermonth.ChartAreas(0).AxisX.Interval = 1
                Label15.Text = "SALES PER MONTH - " & currentYear
            Else
                ' ---- every day of the chosen month ----
                Dim days As Integer = DateTime.DaysInMonth(currentYear, selMonth)
                Dim daily(days) As Decimal
                Dim daySql As String = "SELECT DAY(or_date) AS d, SUM(total_amount) AS total " &
                                       "FROM TBL_TRANSACTIONS WHERE YEAR(or_date) = @year AND MONTH(or_date) = @month " &
                                       "AND status <> 'Cancelled' GROUP BY DAY(or_date)"
                Using localCmd As New MySqlCommand(daySql, cn)
                    localCmd.Parameters.AddWithValue("@year", currentYear)
                    localCmd.Parameters.AddWithValue("@month", selMonth)
                    Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                        While localDr.Read()
                            daily(Convert.ToInt32(localDr("d"))) = Convert.ToDecimal(localDr("total"))
                        End While
                    End Using
                End Using

                For d As Integer = 1 To days
                    chrtsalespermonth.Series("Series1").Points.AddXY(d.ToString(), Convert.ToDouble(daily(d)))
                Next
                chrtsalespermonth.ChartAreas(0).AxisX.Interval = 2
                Label15.Text = "DAILY SALES - " & MonthName(selMonth) & " " & currentYear
            End If

            chrtsalespermonth.ChartAreas(0).AxisY.LabelStyle.Format = "N0"
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading sales per month chart: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub
End Class