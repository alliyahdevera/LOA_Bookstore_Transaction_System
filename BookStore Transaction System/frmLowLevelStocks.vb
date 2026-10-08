Imports MySql.Data.MySqlClient

Public Class frmLowLevelStocks
    Private pg As GridPager
    Private Sub frmLowLevelStocks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        dgvListOfProducts.AllowUserToAddRows = False
        pg = New GridPager(dgvListOfProducts, 20)
        AddHandler pg.PageChanged, Sub() LoadGrid(txtSearch.Text.Trim())
        LoadGrid("")
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If pg Is Nothing Then Exit Sub
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadGrid(searchText As String)
        Dim query As String =
        "SELECT v.product_code, p.product_name, p.product_description, c.category_name, ct.type_name, " &
        "v.size, p.unit_price, v.quantity_on_hand, v.reorder_level, " &
        "CASE WHEN v.quantity_on_hand = 0 THEN 'Out of Stock' ELSE 'Low Stock' END AS calc_status " &
        "FROM TBL_PRODUCT_VARIANTS v " &
        "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
        "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
        "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
        "WHERE v.quantity_on_hand <= v.reorder_level AND (v.product_code LIKE @s OR p.product_name LIKE @s) " &
        "ORDER BY v.quantity_on_hand ASC, p.product_name"

        Dim dt As DataTable = pg.LoadPage(query, New String() {"@s"}, New Object() {"%" & searchText & "%"})

        dgvListOfProducts.SuspendLayout()
        dgvListOfProducts.Rows.Clear()
        For Each r As DataRow In dt.Rows
            dgvListOfProducts.Rows.Add(
            r("product_code").ToString(), r("product_name").ToString(),
            If(IsDBNull(r("product_description")), "", r("product_description").ToString()),
            r("category_name").ToString(), r("type_name").ToString(), r("size").ToString(),
            Convert.ToDecimal(r("unit_price")).ToString("N2"), r("quantity_on_hand").ToString(),
            r("reorder_level").ToString(), r("calc_status").ToString())
        Next
        dgvListOfProducts.ClearSelection()
        dgvListOfProducts.ResumeLayout()
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        ' frmLowLevelStocks
        pg.ExportAllPages(Sub() LoadGrid(txtSearch.Text.Trim()), Sub() ExportGridToCsv(dgvListOfProducts, "LowLevelStocks"))

    End Sub
End Class