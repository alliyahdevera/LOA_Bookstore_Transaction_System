Imports MySql.Data.MySqlClient

Public Class frmSalesByItem
    Private isFilling As Boolean = False
    Private ReadOnly Peso As String = ChrW(8369)
    Private pg As GridPager
    Private allRows As DataTable

    Private Sub frmSalesByItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        dtfrom.Value = New Date(Date.Today.Year, Date.Today.Month, 1)
        dtto.Value = Date.Today

        Label9.Text = "Total Quantity"
        dgvsalesreport.Columns("AmountPaid").HeaderText = "Last Sold"
        dgvsalesreport.AllowUserToAddRows = False
        dgvsalesreport.AllowUserToDeleteRows = False

        btnexportexcel.Visible = (currentuser.Role = ROLE_SUPERVISOR OrElse currentuser.Role = ROLE_MANAGEMENT)
        isFilling = True
        Dim cats As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")
        Dim allRow As DataRow = cats.NewRow()
        allRow("category_id") = 0
        allRow("category_name") = "-- All Categories --"
        cats.Rows.InsertAt(allRow, 0)
        FillCombo(cbocategory, cats, "category_name", "category_id")
        cbocategory.SelectedIndex = 0
        isFilling = False
        pg = New GridPager(dgvsalesreport, 20)
        AddHandler pg.PageChanged, Sub() ShowPage()
        LoadGrid()
    End Sub
    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isFilling Then Exit Sub
        LoadGrid()
    End Sub
    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        If dtfrom.Value.Date > dtto.Value.Date Then
            MsgBox("'Date from' cannot be later than 'To'.", vbExclamation, "Sales by Item")
            Exit Sub
        End If
        LoadGrid()
        If dgvsalesreport.Rows.Count = 0 Then
            MsgBox("No sales found for the selected dates.", vbInformation, "Sales by Item")
        End If
    End Sub

    Private Sub LoadGrid()
        allRows = GetDataTable(
            "SELECT v.product_code, p.product_name, v.size, c.category_name, " &
            "SUM(ti.quantity) AS qty, SUM(ti.subtotal) AS amt, MAX(t.or_date) AS last_sold " &
            "FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_transactions t ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "WHERE t.or_date BETWEEN @d1 AND @d2 AND t.status <> 'Cancelled' AND (@cat = 0 OR c.category_id = @cat) " &
            "GROUP BY v.variant_id, v.product_code, p.product_name, v.size, c.category_name " &
            "ORDER BY amt DESC",
            New String() {"@d1", "@d2", "@cat"}, New Object() {dtfrom.Value.Date, dtto.Value.Date, SelectedId(cbocategory)})

        Dim totalQty As Integer = 0
        Dim totalSales As Decimal = 0D
        For Each r As DataRow In allRows.Rows
            totalQty += Convert.ToInt32(r("qty"))
            totalSales += Convert.ToDecimal(r("amt"))
        Next

        If pg IsNot Nothing Then pg.Reset()
        ShowPage()
        Label3.Text = Peso & totalSales.ToString("N2")     ' Total Sales
        Label8.Text = totalQty.ToString("N0")              ' Total Quantity
    End Sub
    Private Sub ShowPage()
        If allRows Is Nothing OrElse pg Is Nothing Then Exit Sub
        Dim dt As DataTable = pg.Slice(allRows)
        dgvsalesreport.Rows.Clear()
        For Each r As DataRow In dt.Rows
            Dim size As String = r("size").ToString()
            Dim name As String = r("product_name").ToString() & If(size <> "" AndAlso size <> "N/A", " (" & size & ")", "")

            Dim idx As Integer = dgvsalesreport.Rows.Add()
            Dim row As DataGridViewRow = dgvsalesreport.Rows(idx)
            row.Cells("ProductCode").Value = r("product_code").ToString()
            row.Cells("ProductName").Value = name
            row.Cells("Category").Value = r("category_name").ToString()
            row.Cells("Quantity").Value = Convert.ToInt32(r("qty"))
            row.Cells("TotalSales").Value = Convert.ToDecimal(r("amt")).ToString("N2")
            row.Cells("AmountPaid").Value = Convert.ToDateTime(r("last_sold")).ToString("yyyy-MM-dd")
        Next
        dgvsalesreport.ClearSelection()
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        pg.ExportAllPages(Sub() ShowPage(), Sub() ExportGridToCsv(dgvsalesreport, "SalesByItem"))
    End Sub

End Class