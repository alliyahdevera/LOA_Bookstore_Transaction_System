Imports MySql.Data.MySqlClient

Public Class frmProductList

    Private pg As GridPager
    Private isFilling As Boolean = False

    Private Sub frmProductList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        pg = New GridPager(dgvlistproducts, 20)
        AddHandler pg.PageChanged, Sub() LoadGrid()

        isFilling = True
        LoadCategoryCombo()
        FillTypeCombo(cbotype, 0)
        isFilling = False

        LoadCards()
        LoadGrid()
    End Sub

    Private Sub LoadCategoryCombo()
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")
        Dim row As DataRow = dt.NewRow()
        row("category_id") = 0
        row("category_name") = "-- All Categories --"
        dt.Rows.InsertAt(row, 0)
        FillCombo(cbocategory, dt, "category_name", "category_id")
        cbocategory.SelectedIndex = 0
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isFilling Then Exit Sub
        isFilling = True
        FillTypeCombo(cbotype, SelectedId(cbocategory))
        isFilling = False
        pg.Reset()
        LoadGrid()
    End Sub

    Private Sub cbotype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbotype.SelectedIndexChanged
        If isFilling Then Exit Sub
        pg.Reset()
        LoadGrid()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If pg Is Nothing Then Exit Sub
        pg.Reset()
        LoadGrid()
    End Sub

    Private Sub LoadCards()
        Dim totalProducts As Integer = Convert.ToInt32(If(ExecScalar("SELECT COUNT(*) FROM TBL_PRODUCTS"), 0))
        Dim totalQty As Integer = Convert.ToInt32(If(ExecScalar("SELECT IFNULL(SUM(quantity_on_hand),0) FROM TBL_PRODUCT_VARIANTS"), 0))
        Dim onHand As Integer = Convert.ToInt32(If(ExecScalar("SELECT COUNT(*) FROM TBL_PRODUCT_VARIANTS WHERE quantity_on_hand > 0"), 0))
        Dim critical As Integer = Convert.ToInt32(If(ExecScalar("SELECT COUNT(*) FROM TBL_PRODUCT_VARIANTS WHERE quantity_on_hand > 0 AND quantity_on_hand <= reorder_level"), 0))
        Dim outOfStock As Integer = Convert.ToInt32(If(ExecScalar("SELECT COUNT(*) FROM TBL_PRODUCT_VARIANTS WHERE quantity_on_hand = 0"), 0))

        lbltotalproducts.Text = totalProducts.ToString("N0")
        lbltotalqproducts.Text = totalQty.ToString("N0")
        lblonhand.Text = onHand.ToString("N0")
        lblcriticallvl.Text = critical.ToString("N0")
        lbloutofstocks.Text = outOfStock.ToString("N0")
    End Sub

    Private Sub LoadGrid()
        Dim query As String =
            "SELECT v.product_code, p.product_name, p.product_description, c.category_name, ct.type_name, " &
            "v.size, p.unit_price, v.quantity_on_hand, v.reorder_level, " &
            "CASE WHEN p.status = 'Inactive' THEN 'Inactive' " &
            "WHEN v.quantity_on_hand = 0 THEN 'Out of Stock' " &
            "WHEN v.quantity_on_hand <= v.reorder_level THEN 'Low Stock' " &
            "ELSE 'In Stock' END AS calc_status " &
            "FROM TBL_PRODUCT_VARIANTS v " &
            "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
            "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
            "WHERE (@cat = 0 OR c.category_id = @cat) AND (@type = 0 OR ct.category_type_id = @type) " &
            "AND (v.product_code LIKE @s OR p.product_name LIKE @s) " &
            "ORDER BY p.product_name, v.size"

        Dim dt As DataTable = pg.LoadPage(query, New String() {"@cat", "@type", "@s"},
                                          New Object() {SelectedId(cbocategory), SelectedId(cbotype), "%" & txtSearch.Text.Trim() & "%"})

        dgvlistproducts.SuspendLayout()
        dgvlistproducts.Rows.Clear()
        For Each r As DataRow In dt.Rows
            Dim status As String = r("calc_status").ToString()
            Dim idx As Integer = dgvlistproducts.Rows.Add(
                r("product_code").ToString(), r("product_name").ToString(), r("product_description").ToString(),
                r("category_name").ToString(), r("type_name").ToString(), r("size").ToString(),
                Convert.ToDecimal(r("unit_price")).ToString("N2"), r("quantity_on_hand").ToString(),
                r("reorder_level").ToString(), status)

            If status = "Low Stock" Then
                With dgvlistproducts.Rows(idx).DefaultCellStyle
                    .BackColor = Color.FromArgb(255, 220, 220)
                    .ForeColor = Color.Firebrick
                    .SelectionBackColor = Color.Firebrick
                    .SelectionForeColor = Color.White
                End With
            End If
        Next
        dgvlistproducts.ClearSelection()
        dgvlistproducts.ResumeLayout()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class