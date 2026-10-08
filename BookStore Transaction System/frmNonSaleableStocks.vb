Public Class frmNonSaleableStocks

    Private Const PAGE_SIZE As Integer = 25
    Private pg As GridPager
    Private isLoading As Boolean = True
    Private WithEvents tmrSearch As New System.Windows.Forms.Timer With {.Interval = 400}

    Private Sub frmNonSaleableStocks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)
        ApplySearchPlaceholders(Me)
        Label7.Text = "NON-SALEABLE / DAMAGED ITEMS"

        cbocategory.DropDownStyle = ComboBoxStyle.DropDownList
        cbocategory.Items.Clear()
        cbocategory.Items.Add("All Categories")
        Dim cats As DataTable = GetDataTable("SELECT category_name FROM tbl_categories ORDER BY category_name")
        If cats IsNot Nothing Then
            For Each r As DataRow In cats.Rows
                cbocategory.Items.Add(r("category_name").ToString())
            Next
        End If
        cbocategory.SelectedIndex = 0

        cbotype.DropDownStyle = ComboBoxStyle.DropDownList
        LoadTypeCombo()

        With dgvlistproducts
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        End With

        pg = New GridPager(dgvlistproducts, PAGE_SIZE)
        AddHandler pg.PageChanged, Sub() LoadItems()

        isLoading = False
        LoadItems()
    End Sub

    Private Sub LoadTypeCombo()
        cbotype.Items.Clear()
        cbotype.Items.Add("All Types")
        Dim dt As DataTable
        If cbocategory.SelectedIndex <= 0 Then
            dt = GetDataTable("SELECT DISTINCT type_name FROM tbl_category_types ORDER BY type_name")
        Else
            dt = GetDataTable("SELECT ct.type_name FROM tbl_category_types ct " &
                              "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
                              "WHERE c.category_name = @c ORDER BY ct.type_name",
                              New String() {"@c"}, New Object() {Convert.ToString(cbocategory.SelectedItem)})
        End If
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                cbotype.Items.Add(r("type_name").ToString())
            Next
        End If
        cbotype.SelectedIndex = 0
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isLoading Then Exit Sub
        isLoading = True
        LoadTypeCombo()
        isLoading = False
        pg.Reset()
        LoadItems()
    End Sub

    Private Sub cbotype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbotype.SelectedIndexChanged
        If isLoading Then Exit Sub
        pg.Reset()
        LoadItems()
    End Sub

    ' search waits 0.4s after typing
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If isLoading Then Exit Sub
        tmrSearch.Stop()
        tmrSearch.Start()
    End Sub

    Private Sub tmrSearch_Tick(sender As Object, e As EventArgs) Handles tmrSearch.Tick
        tmrSearch.Stop()
        pg.Reset()
        LoadItems()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        tmrSearch.Stop()
        pg.Reset()
        LoadItems()
    End Sub

    Private Sub LoadItems()
        If pg Is Nothing Then Exit Sub

        Dim category As String = If(cbocategory.SelectedIndex <= 0, "", cbocategory.Text)
        Dim typeName As String = If(cbotype.SelectedIndex <= 0, "", Convert.ToString(cbotype.SelectedItem))

        Dim query As String =
            "SELECT v.variant_id, v.product_code, p.product_name, p.product_description, c.category_name, ct.type_name, " &
            "v.size, p.unit_price, n.stock_condition, SUM(n.quantity) AS qty " &
            "FROM tbl_nonsaleable_stocks n " &
            "INNER JOIN tbl_product_variants v ON n.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "WHERE n.status = 'Nonsaleable' " &
            "AND (v.product_code LIKE @s OR p.product_name LIKE @s OR p.product_description LIKE @s) "

        Dim names As New List(Of String)({"@s"})
        Dim values As New List(Of Object)({"%" & txtSearch.Text.Trim() & "%"})

        If category <> "" Then
            query &= "AND c.category_name = @cat "
            names.Add("@cat")
            values.Add(category)
        End If
        If typeName <> "" Then
            query &= "AND ct.type_name = @type "
            names.Add("@type")
            values.Add(typeName)
        End If

        query &= "GROUP BY v.variant_id, v.product_code, p.product_name, p.product_description, c.category_name, " &
                 "ct.type_name, v.size, p.unit_price, n.stock_condition " &
                 "ORDER BY p.product_name, v.size"

        Dim totalUnits As Integer = Convert.ToInt32(If(ExecScalar(
            "SELECT IFNULL(SUM(qty), 0) FROM (" & query & ") AS t", names.ToArray(), values.ToArray()), 0))

        Dim dt As DataTable = pg.LoadPage(query, names.ToArray(), values.ToArray())

        dgvlistproducts.SuspendLayout()
        dgvlistproducts.Rows.Clear()
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                ' order = ProductCode, ProductName, ProductDescription, Category, TypeofProduct, Size, UnitPrice, Quantity, Condition
                Dim idx As Integer = dgvlistproducts.Rows.Add(
                    r("product_code").ToString(),
                    r("product_name").ToString(),
                    If(IsDBNull(r("product_description")), "", r("product_description").ToString()),
                    r("category_name").ToString(),
                    r("type_name").ToString(),
                    r("size").ToString(),
                    Convert.ToDecimal(r("unit_price")).ToString("N2"),
                    Convert.ToInt32(r("qty")).ToString("N0"),
                    r("stock_condition").ToString())
                dgvlistproducts.Rows(idx).Cells("Condition").Style.ForeColor = Color.Firebrick
                dgvlistproducts.Rows(idx).Cells("Quantity").Style.ForeColor = Color.Firebrick
            Next
        End If
        dgvlistproducts.ClearSelection()
        dgvlistproducts.ResumeLayout()

        Label7.Text = "NON-SALEABLE / DAMAGED ITEMS  -  " & totalUnits.ToString("N0") & " unit(s)"
    End Sub

End Class