Imports MySql.Data.MySqlClient

Public Class frmManageProducts
    Private pg As GridPager
    Private selectedProductId As Integer = 0
    Private selectedVariantId As Integer = 0
    Private isFilling As Boolean = False

    Private Sub frmManageProducts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList
        cboTypeOfProduct.DropDownStyle = ComboBoxStyle.DropDownList
        pg = New GridPager(dgvListOfProducts, 20)
        AddHandler pg.PageChanged, Sub() LoadGrid(txtSearch.Text.Trim(), GetSelectedCategoryId())
        LoadCategoryCombo()
        LoadGrid("", GetSelectedCategoryId())
        ClearFields()
    End Sub

    ' Keeps the live clock running if you have a Timer control on the form
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lbldatetime.Text = "Today is " & DateTime.Now.ToString("dddd, MMMM d, yyyy - hh:mm:ss tt")
    End Sub
    Private Sub LoadCategoryCombo()
        isFilling = True
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")
        Dim row As DataRow = dt.NewRow()
        row("category_id") = 0
        row("category_name") = "-- All Categories --"
        dt.Rows.InsertAt(row, 0)
        FillCombo(cboCategory, dt, "category_name", "category_id")
        cboCategory.SelectedIndex = 0
        isFilling = False
    End Sub

    Private Function GetSelectedCategoryId() As Integer
        If cboCategory.SelectedValue IsNot Nothing AndAlso IsNumeric(cboCategory.SelectedValue) Then
            Return Convert.ToInt32(cboCategory.SelectedValue)
        End If
        Return 0
    End Function
    Private Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedIndexChanged
        If isFilling Then Exit Sub
        Dim catId As Integer = GetSelectedCategoryId()
        LoadTypes(catId)
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim(), catId)
    End Sub
    Private Sub cboTypeOfProduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTypeOfProduct.SelectedIndexChanged
        If isFilling OrElse pg Is Nothing Then Exit Sub
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim(), GetSelectedCategoryId())
    End Sub
    Private Function GetSelectedTypeId() As Integer
        If cboTypeOfProduct.SelectedValue IsNot Nothing AndAlso IsNumeric(cboTypeOfProduct.SelectedValue) Then
            Return Convert.ToInt32(cboTypeOfProduct.SelectedValue)
        End If
        Return 0
    End Function
    Private Sub LoadTypes(catId As Integer)
        Dim wasFilling As Boolean = isFilling
        isFilling = True
        If catId = 0 Then
            cboTypeOfProduct.DataSource = Nothing
        Else
            Dim dt As DataTable = GetDataTable("SELECT category_type_id, type_name FROM TBL_CATEGORY_TYPES WHERE category_id = @c ORDER BY type_name",
                                           New String() {"@c"}, New Object() {catId})
            FillCombo(cboTypeOfProduct, dt, "type_name", "category_type_id")
            cboTypeOfProduct.SelectedIndex = -1
        End If
        isFilling = wasFilling
    End Sub


    ' Product Code: Alphanumeric and hyphens only
    Private Sub txtProductCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtProductCode.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> "-"c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' Unit Price: Numbers and single decimal point only
    Private Sub txtUnitPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUnitPrice.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
        If e.KeyChar = "."c AndAlso txtUnitPrice.Text.Contains(".") Then
            e.Handled = True
        End If
    End Sub

    ' Reorder Level / Quantity: Digits only
    Private Sub txtQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQuantity.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' Pre-Save Validation Check
    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtProductCode.Text) Then
            MsgBox("Product Code is required.", vbExclamation, "Manage Products")
            txtProductCode.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtProductName.Text) Then
            MsgBox("Product Name is required.", vbExclamation, "Manage Products")
            txtProductName.Focus()
            Return False
        End If

        If cboCategory.SelectedValue Is Nothing OrElse GetSelectedCategoryId() = 0 Then
            MsgBox("Select a valid Category.", vbExclamation, "Manage Products")
            cboCategory.Focus()
            Return False
        End If

        If cboTypeOfProduct.SelectedValue Is Nothing OrElse cboTypeOfProduct.SelectedIndex = -1 Then
            MsgBox("Select a valid Type of Product.", vbExclamation, "Manage Products")
            cboTypeOfProduct.Focus()
            Return False
        End If

        If Not IsNumeric(txtUnitPrice.Text) OrElse Convert.ToDecimal(txtUnitPrice.Text) < 0 Then
            MsgBox("Unit Price must be a valid positive number.", vbExclamation, "Manage Products")
            txtUnitPrice.Focus()
            Return False
        End If

        If Not IsNumeric(txtQuantity.Text) OrElse Convert.ToInt32(txtQuantity.Text) < 0 Then
            MsgBox("Reorder Level / Quantity must be a valid non-negative number.", vbExclamation, "Manage Products")
            txtQuantity.Focus()
            Return False
        End If

        Return True
    End Function
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If pg Is Nothing Then Exit Sub
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim(), GetSelectedCategoryId())
    End Sub

    Private Sub LoadGrid(searchText As String, categoryId As Integer)
        Dim query As String =
        "SELECT v.variant_id, p.product_id, v.product_code, p.product_name, p.product_description, " &
        "c.category_name, ct.type_name, v.size, p.unit_price, v.quantity_on_hand, v.reorder_level, p.status " &
        "FROM TBL_PRODUCT_VARIANTS v " &
        "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
        "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
        "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
        "WHERE (@cat = 0 OR c.category_id = @cat) AND (@type = 0 OR ct.category_type_id = @type) " &
        "AND (v.product_code LIKE @s OR p.product_name LIKE @s) " &
        "ORDER BY p.product_name, v.size"

        Dim dt As DataTable = pg.LoadPage(query, New String() {"@cat", "@type", "@s"},
                                      New Object() {categoryId, GetSelectedTypeId(), "%" & searchText & "%"})

        dgvListOfProducts.SuspendLayout()
        dgvListOfProducts.Rows.Clear()
        For Each r As DataRow In dt.Rows
            Dim idx As Integer = dgvListOfProducts.Rows.Add(
            r("product_code").ToString(), r("product_name").ToString(), r("product_description").ToString(),
            r("category_name").ToString(), r("type_name").ToString(), r("size").ToString(),
            Convert.ToDecimal(r("unit_price")).ToString("N2"), r("quantity_on_hand").ToString(),
            r("reorder_level").ToString(), r("status").ToString())
            dgvListOfProducts.Rows(idx).Tag = New Integer() {Convert.ToInt32(r("product_id")), Convert.ToInt32(r("variant_id"))}
        Next
        dgvListOfProducts.ClearSelection()
        dgvListOfProducts.ResumeLayout()
    End Sub

    Private Sub dgvListOfProducts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvListOfProducts.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = dgvListOfProducts.Rows(e.RowIndex)
        If row.Tag Is Nothing Then Exit Sub

        Dim ids As Integer() = CType(row.Tag, Integer())
        selectedProductId = ids(0)
        selectedVariantId = ids(1)

        txtProductCode.Text = row.Cells(0).Value.ToString()
        txtProductName.Text = row.Cells(1).Value.ToString()
        txtProductDescription.Text = row.Cells(2).Value.ToString()
        Dim catId As Integer = GetCategoryIdByName(row.Cells(3).Value.ToString())
        isFilling = True
        cboCategory.SelectedValue = catId
        LoadTypes(catId)
        cboTypeOfProduct.SelectedValue = GetTypeIdByName(row.Cells(4).Value.ToString())
        isFilling = False
        txtSize.Text = row.Cells(5).Value.ToString()
        txtUnitPrice.Text = row.Cells(6).Value.ToString()
        txtQuantity.Text = row.Cells(8).Value.ToString() ' Displays Reorder Level
        txtStatus.Text = row.Cells(9).Value.ToString()
    End Sub

    Private Function GetCategoryIdByName(name As String) As Integer
        Return Convert.ToInt32(If(ExecScalar("SELECT category_id FROM TBL_CATEGORIES WHERE category_name = @n", New String() {"@n"}, New Object() {name}), 0))
    End Function

    Private Function GetTypeIdByName(name As String) As Integer
        Return Convert.ToInt32(If(ExecScalar("SELECT category_type_id FROM TBL_CATEGORY_TYPES WHERE type_name = @n", New String() {"@n"}, New Object() {name}), 0))
    End Function

    ' ADD BUTTON
    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can add products.", vbExclamation, "Access Denied")
            Exit Sub
        End If

        If Not ValidateInputs() Then Exit Sub

        Dim isNewProduct As Boolean = (selectedProductId = 0)
        Dim oldPrice As Decimal = 0D
        If Not isNewProduct Then
            oldPrice = Convert.ToDecimal(If(ExecScalar("SELECT unit_price FROM TBL_PRODUCTS WHERE product_id = @id", New String() {"@id"}, New Object() {selectedProductId}), 0))
        End If

        If selectedProductId = 0 Then
            selectedProductId = CInt(ExecInsertGetId(
                "INSERT INTO TBL_PRODUCTS (product_name, product_description, category_type_id, unit_price, status) VALUES (@n, @d, @t, @p, @st)",
                New String() {"@n", "@d", "@t", "@p", "@st"},
                New Object() {txtProductName.Text.Trim(), txtProductDescription.Text.Trim(), cboTypeOfProduct.SelectedValue, Convert.ToDecimal(txtUnitPrice.Text),
                               If(String.IsNullOrWhiteSpace(txtStatus.Text), "Active", txtStatus.Text.Trim())}))
            If selectedProductId = 0 Then Exit Sub
        Else
            ExecNonQuery("UPDATE TBL_PRODUCTS SET unit_price = @p, product_description = @d, status = @st WHERE product_id = @id",
                New String() {"@p", "@d", "@st", "@id"},
                New Object() {Convert.ToDecimal(txtUnitPrice.Text), txtProductDescription.Text.Trim(),
                               If(String.IsNullOrWhiteSpace(txtStatus.Text), "Active", txtStatus.Text.Trim()), selectedProductId})
        End If

        Dim ok As Boolean = ExecNonQuery(
            "INSERT INTO TBL_PRODUCT_VARIANTS (product_id, product_code, size, quantity_on_hand, reorder_level) VALUES (@pid, @code, @size, 0, @reorder)",
            New String() {"@pid", "@code", "@size", "@reorder"},
            New Object() {selectedProductId, txtProductCode.Text.Trim(), If(String.IsNullOrWhiteSpace(txtSize.Text), "N/A", txtSize.Text.Trim()), Convert.ToInt32(txtQuantity.Text)})

        If ok Then
            Dim newPrice As Decimal = Convert.ToDecimal(txtUnitPrice.Text)
            LogActivity("Add Product", txtProductCode.Text.Trim(), "Added product '" & txtProductName.Text.Trim() & "' (Price: " & newPrice.ToString("N2") & ")")
            If isNewProduct Then
                LogPriceChange(txtProductCode.Text.Trim(), txtProductName.Text.Trim(), 0D, newPrice, "Initial price")
            ElseIf oldPrice <> newPrice Then
                LogPriceChange(txtProductCode.Text.Trim(), txtProductName.Text.Trim(), oldPrice, newPrice, "Price Update")
            End If
            MsgBox("Product added. Use Stock Entry to add its initial quantity.", vbInformation, "Manage Products")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim(), GetSelectedCategoryId())
        Else
            MsgBox("Could not add product. The Product Code may already be in use.", vbExclamation, "Manage Products")
        End If
    End Sub

    ' UPDATE BUTTON
    Private Sub btnupd_Click(sender As Object, e As EventArgs) Handles btnupd.Click
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can update products.", vbExclamation, "Access Denied")
            Exit Sub
        End If

        If selectedVariantId = 0 Then
            MsgBox("Select a product from the list first.", vbExclamation, "Manage Products")
            Exit Sub
        End If

        Dim oldUnitPrice As Decimal = Convert.ToDecimal(If(ExecScalar("SELECT unit_price FROM TBL_PRODUCTS WHERE product_id = @id", New String() {"@id"}, New Object() {selectedProductId}), 0))

        If Not ValidateInputs() Then Exit Sub

        ExecNonQuery("UPDATE TBL_PRODUCTS SET product_name=@n, product_description=@d, category_type_id=@t, unit_price=@p, status=@st WHERE product_id=@id",
            New String() {"@n", "@d", "@t", "@p", "@st", "@id"},
            New Object() {txtProductName.Text.Trim(), txtProductDescription.Text.Trim(), cboTypeOfProduct.SelectedValue, Convert.ToDecimal(txtUnitPrice.Text),
                           If(String.IsNullOrWhiteSpace(txtStatus.Text), "Active", txtStatus.Text.Trim()), selectedProductId})

        Dim ok As Boolean = ExecNonQuery("UPDATE TBL_PRODUCT_VARIANTS SET product_code=@code, size=@size, reorder_level=@reorder WHERE variant_id=@vid",
            New String() {"@code", "@size", "@reorder", "@vid"},
            New Object() {txtProductCode.Text.Trim(), If(String.IsNullOrWhiteSpace(txtSize.Text), "N/A", txtSize.Text.Trim()), Convert.ToInt32(txtQuantity.Text), selectedVariantId})

        If ok Then
            Dim newUnitPrice As Decimal = Convert.ToDecimal(txtUnitPrice.Text)
            LogActivity("Update Product", txtProductCode.Text.Trim(), "Updated product '" & txtProductName.Text.Trim() & "'")
            If oldUnitPrice <> newUnitPrice Then
                LogPriceChange(txtProductCode.Text.Trim(), txtProductName.Text.Trim(), oldUnitPrice, newUnitPrice, "Price Update")
            End If
            MsgBox("Product updated.", vbInformation, "Manage Products")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim(), GetSelectedCategoryId())
        End If
    End Sub

    ' REMOVE BUTTON
    Private Sub btnremove_Click(sender As Object, e As EventArgs) Handles btnremove.Click
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can remove products.", vbExclamation, "Access Denied")
            Exit Sub
        End If

        If selectedVariantId = 0 Then
            MsgBox("Select a product from the list first.", vbExclamation, "Manage Products")
            Exit Sub
        End If

        If MsgBox("Remove this product variant? This cannot be undone.", vbYesNo + vbQuestion, "Manage Products") <> MsgBoxResult.Yes Then Exit Sub

        Dim ok As Boolean = ExecNonQuery("DELETE FROM TBL_PRODUCT_VARIANTS WHERE variant_id = @vid", New String() {"@vid"}, New Object() {selectedVariantId})
        If ok Then
            LogActivity("Remove Product", txtProductCode.Text.Trim(), "Removed product '" & txtProductName.Text.Trim() & "'")
            MsgBox("Product removed.", vbInformation, "Manage Products")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim(), GetSelectedCategoryId())
        Else
            MsgBox("Cannot remove: this product already has transaction or stock-in history. Set its Status to Inactive instead.", vbExclamation, "Manage Products")
        End If
    End Sub
    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        ClearFields()
        pg.Reset()
        LoadGrid("", 0)
    End Sub

    Private Sub ClearFields()
        selectedProductId = 0
        selectedVariantId = 0
        txtProductCode.Clear()
        txtProductName.Clear()
        txtProductDescription.Clear()
        txtUnitPrice.Clear()
        txtQuantity.Clear()
        txtSize.Clear()
        txtStatus.Clear()
        txtSearch.Clear()
        isFilling = True
        cboCategory.SelectedIndex = 0
        isFilling = False
        cboTypeOfProduct.DataSource = Nothing
        dgvListOfProducts.ClearSelection()
    End Sub

End Class