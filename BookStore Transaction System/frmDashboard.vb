Imports MySql.Data.MySqlClient

Public Class frmDashboard

    ' Control map (names from the designer):
    '   Label8  = Total Products               Label9  = Total Quantity of Products
    '   Label10 = Total Sales                  Label11 = Total Students
    '   chtMostreqdoc     = MOST BOUGHT PRODUCT (bar)
    '   chtdocreqpermonth = PRODUCT SALES (pie)
    '   Chart1            = CRITICAL PRODUCTS (column)
    '   Chart2            = SALES PER MONTH (line)

    ' TBL_TRANSACTION_ITEMS has no quantity column, so quantity sold is derived (subtotal / unit price).
    ' If you add a quantity column later, change this to "ti.quantity".
    Private Const QTY_SOLD As String = "ROUND(ti.subtotal / p.unit_price)"

    Private WithEvents tmrClock As System.Windows.Forms.Timer

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Legend is only useful on the pie chart
        Chart1.Legends(0).Enabled = False
        chtMostreqdoc.Legends(0).Enabled = False
        Chart2.Legends(0).Enabled = False

        ' Footer: name, position, real-time clock
        lblname.Text = currentuser.FullName
        lblposition.Text = currentuser.Role
        tmrClock = New System.Windows.Forms.Timer()
        tmrClock.Interval = 1000
        tmrClock.Start()
        UpdateFooterDateTime()

        RefreshDashboard()
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
        Label8.Text = GetScalar("SELECT COUNT(*) FROM TBL_PRODUCTS").ToString("N0")
        Label9.Text = GetScalar("SELECT IFNULL(SUM(quantity_on_hand), 0) FROM TBL_PRODUCT_VARIANTS").ToString("N0")
        Label10.Text = ChrW(8369) & GetScalar("SELECT IFNULL(SUM(total_amount), 0) FROM TBL_TRANSACTIONS").ToString("N2")
        Label11.Text = GetScalar("SELECT COUNT(*) FROM TBL_STUDENTS").ToString("N0")

        CenterLabel(Label8)
        CenterLabel(Label9)
        CenterLabel(Label10)
        CenterLabel(Label11)
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

    ' ------------------------------------------------------------------
    ' MOST BOUGHT PRODUCT - top 5 products by quantity sold
    ' ------------------------------------------------------------------
    Private Sub LoadMostBoughtProducts()
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT p.product_name, SUM(" & QTY_SOLD & ") AS qty_sold " &
                                  "FROM TBL_TRANSACTION_ITEMS ti " &
                                  "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "GROUP BY p.product_id, p.product_name " &
                                  "ORDER BY qty_sold DESC " &
                                  "LIMIT 5"

            Using localCmd As New MySqlCommand(query, cn)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    chtMostreqdoc.Series("Series1").Points.Clear()
                    chtMostreqdoc.Series("Series1").IsValueShownAsLabel = True
                    chtMostreqdoc.ChartAreas(0).AxisX.IsReversed = True   ' best seller on top
                    chtMostreqdoc.ChartAreas(0).AxisX.Interval = 1
                    While localDr.Read()
                        chtMostreqdoc.Series("Series1").Points.AddXY(localDr("product_name").ToString(), Convert.ToInt32(localDr("qty_sold")))
                    End While
                End Using
            End Using

            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading most bought products chart: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    ' ------------------------------------------------------------------
    ' PRODUCT SALES - share of sales amount per category (pie)
    ' ------------------------------------------------------------------
    Private Sub LoadProductSales()
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT c.category_name, SUM(ti.subtotal) AS total_sales " &
                                  "FROM TBL_TRANSACTION_ITEMS ti " &
                                  "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
                                  "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
                                  "GROUP BY c.category_id, c.category_name " &
                                  "ORDER BY total_sales DESC"

            Using localCmd As New MySqlCommand(query, cn)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    With chtdocreqpermonth.Series("Series1")
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

    ' ------------------------------------------------------------------
    ' CRITICAL PRODUCTS - variants at or below their reorder level
    ' ------------------------------------------------------------------
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
                    Chart1.Series("Series1").Points.Clear()
                    Chart1.Series("Series1").IsValueShownAsLabel = True
                    Chart1.ChartAreas(0).AxisX.Interval = 1
                    While localDr.Read()
                        Dim idx As Integer = Chart1.Series("Series1").Points.AddXY(localDr("item_name").ToString(), Convert.ToInt32(localDr("quantity_on_hand")))
                        Chart1.Series("Series1").Points(idx).Color = Color.Firebrick
                        Chart1.Series("Series1").Points(idx).ToolTip = "Reorder level: " & localDr("reorder_level").ToString()
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

        Try
            If Not connection() Then Exit Sub

            Dim totals(12) As Decimal

            Dim monthSql As String = "SELECT MONTH(or_date) AS m, SUM(total_amount) AS total " &
                                     "FROM TBL_TRANSACTIONS WHERE YEAR(or_date) = @year " &
                                     "GROUP BY MONTH(or_date)"

            Using localCmd As New MySqlCommand(monthSql, cn)
                localCmd.Parameters.AddWithValue("@year", currentYear)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    While localDr.Read()
                        totals(Convert.ToInt32(localDr("m"))) = Convert.ToDecimal(localDr("total"))
                    End While
                End Using
            End Using

            With Chart2.Series("Series1")
                .Points.Clear()
                .BorderWidth = 3
                .MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle
                .MarkerSize = 7
                For m As Integer = 1 To 12
                    .Points.AddXY(MonthName(m, True), Convert.ToDouble(totals(m)))
                Next
            End With
            Chart2.ChartAreas(0).AxisX.Interval = 1
            Chart2.ChartAreas(0).AxisY.LabelStyle.Format = "N0"

            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading sales per month chart: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

End Class