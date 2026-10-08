' frmStockInHistory.vb
Imports MySql.Data.MySqlClient

Public Class frmStockInHistory
    Public Property InitialSearch As String = ""
    Private pg As GridPager
    Private isFilling As Boolean = False
    Private Sub frmStockInHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySearchPlaceholders(Me)
        txtSearch.Text = InitialSearch
        SetupFooter(Me, lblname, lblposition, lbldatetime)
        Label4.Text = "Total Quantity"
        DateTimePicker1.Value = New DateTime(Today.Year, Today.Month, 1)
        DateTimePicker2.Value = Today

        pg = New GridPager(dgvstockinhistory, 20)
        AddHandler pg.PageChanged, Sub() LoadGrid()

        LoadCategoryCombo()
        isFilling = True
        FillTypeCombo(cbotype, 0, TypeSource.StockIns)
        isFilling = False
        LoadGrid()
    End Sub
    Private Sub LoadCategoryCombo()
        isFilling = True
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")
        Dim row As DataRow = dt.NewRow()
        row("category_id") = 0
        row("category_name") = "-- All Categories --"
        dt.Rows.InsertAt(row, 0)
        FillCombo(cbocategory, dt, "category_name", "category_id")
        cbocategory.SelectedIndex = 0
        isFilling = False
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isFilling Then Exit Sub
        isFilling = True
        FillTypeCombo(cbotype, SelectedId(cbocategory), TypeSource.StockIns)
        isFilling = False
        pg.Reset()
        LoadGrid()
    End Sub

    Private Sub cbotype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbotype.SelectedIndexChanged
        If isFilling OrElse pg Is Nothing Then Exit Sub
        pg.Reset()
        LoadGrid()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        pg.Reset()
        LoadGrid()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If pg Is Nothing Then Exit Sub
        pg.Reset()
        LoadGrid()
    End Sub

    Private Sub LoadGrid()
        Dim query As String =
        "SELECT si.reference_no, v.product_code, p.product_name, p.product_description, " &
        "sid.quantity, si.stock_in_date, si.stock_in_time, si.received_by " &
        "FROM TBL_STOCK_IN_DETAILS sid " &
        "INNER JOIN TBL_STOCK_INS si ON sid.stock_in_id = si.stock_in_id " &
        "INNER JOIN TBL_PRODUCT_VARIANTS v ON sid.variant_id = v.variant_id " &
        "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
        "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
        "WHERE si.stock_in_date BETWEEN @d1 AND @d2 " &
        "AND (@cat = 0 OR ct.category_id = @cat) AND (@type = 0 OR ct.category_type_id = @type) " &
        "AND (si.reference_no LIKE @s OR p.product_name LIKE @s) " &
        "ORDER BY si.stock_in_date DESC, si.stock_in_time DESC"

        Dim names As String() = {"@d1", "@d2", "@cat", "@type", "@s"}
        Dim values As Object() = {DateTimePicker1.Value.Date, DateTimePicker2.Value.Date,
                              SelectedId(cbocategory), SelectedId(cbotype), "%" & txtSearch.Text.Trim() & "%"}

        Dim dt As DataTable = pg.LoadPage(query, names, values)
        Label3.Text = Convert.ToInt32(If(ExecScalar("SELECT IFNULL(SUM(quantity),0) FROM (" & query & ") AS t", names, values), 0)).ToString("N0")

        dgvstockinhistory.SuspendLayout()
        dgvstockinhistory.Rows.Clear()
        For Each r As DataRow In dt.Rows
            dgvstockinhistory.Rows.Add(
            r("reference_no").ToString(), r("product_code").ToString(), r("product_name").ToString(),
            r("product_description").ToString(), Convert.ToInt32(r("quantity")),
            Convert.ToDateTime(r("stock_in_date")).ToString("yyyy-MM-dd"), r("stock_in_time").ToString(),
            If(IsDBNull(r("received_by")), "-", r("received_by").ToString()))
        Next
        dgvstockinhistory.ClearSelection()
        dgvstockinhistory.ResumeLayout()
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        pg.ExportAllPages(Sub() LoadGrid(), Sub() ExportGridToCsv(dgvstockinhistory, "StockInHistory"))
    End Sub

End Class