Imports MySql.Data.MySqlClient

Public Class frmManageProducts

    Private selectedProductId As Integer = 0
    Private selectedVariantId As Integer = 0

    ' ------------------------------------------------------------------
    ' Form Load & Initialization
    ' ------------------------------------------------------------------
    Private Sub frmManageProducts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set User Info
        lblname.Text = currentuser.FullName
        lblposition.Text = currentuser.Role

        ' Format and Display Current Date & Time
        lbldatetime.Text = DateTime.Now.ToString("MMMM dd, yyyy - hh:mm tt")

        ' Lock ComboBoxes so users can only pick allowed items
        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList
        cboTypeOfProduct.DropDownStyle = ComboBoxStyle.DropDownList

        LoadCategoryCombo()
        LoadGrid("")
        ClearFields()
    End Sub

    ' Optional: Keeps the live clock running if you have a Timer control on the form
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lbldatetime.Text = DateTime.Now.ToString("MMMM dd, yyyy - hh:mm:ss tt")
    End Sub

    ' ------------------------------------------------------------------
    ' Category & Type Dropdown Loading
    ' ------------------------------------------------------------------
    Private Sub LoadCategoryCombo()
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")
        FillCombo(cboCategory, dt, "category_name", "category_id")
    End Sub

    Private Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedIndexChanged
        If cboCategory.SelectedValue Is Nothing OrElse Not IsNumeric(cboCategory.SelectedValue) Then Exit Sub

        Dim dt As DataTable = GetDataTable("SELECT category_type_id, type_name FROM TBL_CATEGORY_TYPES WHERE category_id = @c ORDER BY type_name",
                                           New String() {"@c"}, New Object() {cboCategory.SelectedValue})

        FillCombo(cboTypeOfProduct, dt, "type_name", "category_type_id")
    End Sub

    ' ------------------------------------------------------------------
    ' Field Input Validations (KeyPress Restrictions)
    ' ------------------------------------------------------------------

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

        If cboCategory.SelectedValue Is Nothing OrElse cboCategory.SelectedIndex = -1 Then
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

    ' ------------------------------------------------------------------
    ' Data Grid Operations
    ' ------------------------------------------------------------------
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadGrid(searchText As String)
        Try
            If Not connection() Then Exit Sub
            Dim query As String = "SELECT v.variant_id, p.product_id, v.product_code, p.product_name, p.product_description, " &
                                  "c.category_name, ct.type_name, v.size, p.unit_price, v.quantity_on_hand, v.reorder_level, p.status " &
                                  "FROM TBL_PRODUCT_VARIANTS v " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
                                  "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
                                  "WHERE v.product_code LIKE @s OR p.product_name LIKE @s " &
                                  "ORDER BY p.product_name, v.size"

            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@s", "%" & searchText & "%")
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    dgvListOfProducts.Rows.Clear()
                    While localDr.Read()
                        Dim idx As Integer = dgvListOfProducts.Rows.Add(
                            localDr("product_code").ToString(),
                            localDr("product_name").ToString(),
                            localDr("product_description").ToString(),
                            localDr("category_name").ToString(),
                            localDr("type_name").ToString(),
                            localDr("size").ToString(),
                            Convert.ToDecimal(localDr("unit_price")).ToString("N2"),
                            localDr("quantity_on_hand").ToString(),
                            localDr("reorder_level").ToString(),
                            localDr("status").ToString()
                        )
                        dgvListOfProducts.Rows(idx).Tag = New Integer() {Convert.ToInt32(localDr("product_id")), Convert.ToInt32(localDr("variant_id"))}
                    End While
                End Using
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading products: " & ex.Message, vbCritical, "Error")
        End Try
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
        cboCategory.SelectedValue = GetCategoryIdByName(row.Cells(3).Value.ToString())
        cboTypeOfProduct.SelectedValue = GetTypeIdByName(row.Cells(4).Value.ToString())
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

    ' ------------------------------------------------------------------
    ' Button Actions
    ' ------------------------------------------------------------------

    ' ADD BUTTON
    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can add products.", vbExclamation, "Access Denied")
            Exit Sub
        End If

        If Not ValidateInputs() Then Exit Sub

        Dim productId As Integer = Convert.ToInt32(If(ExecScalar(
            "SELECT product_id FROM TBL_PRODUCTS WHERE product_name = @n AND category_type_id = @t",
            New String() {"@n", "@t"}, New Object() {txtProductName.Text.Trim(), cboTypeOfProduct.SelectedValue}), 0))

        If productId = 0 Then
            productId = CInt(ExecInsertGetId(
                "INSERT INTO TBL_PRODUCTS (product_name, product_description, category_type_id, unit_price, status) VALUES (@n, @d, @t, @p, @st)",
                New String() {"@n", "@d", "@t", "@p", "@st"},
                New Object() {txtProductName.Text.Trim(), txtProductDescription.Text.Trim(), cboTypeOfProduct.SelectedValue, Convert.ToDecimal(txtUnitPrice.Text),
                               If(String.IsNullOrWhiteSpace(txtStatus.Text), "Active", txtStatus.Text.Trim())}))
            If productId = 0 Then Exit Sub
        Else
            ExecNonQuery("UPDATE TBL_PRODUCTS SET unit_price = @p, product_description = @d, status = @st WHERE product_id = @id",
                New String() {"@p", "@d", "@st", "@id"},
                New Object() {Convert.ToDecimal(txtUnitPrice.Text), txtProductDescription.Text.Trim(),
                               If(String.IsNullOrWhiteSpace(txtStatus.Text), "Active", txtStatus.Text.Trim()), productId})
        End If

        Dim ok As Boolean = ExecNonQuery(
            "INSERT INTO TBL_PRODUCT_VARIANTS (product_id, product_code, size, quantity_on_hand, reorder_level) VALUES (@pid, @code, @size, 0, @reorder)",
            New String() {"@pid", "@code", "@size", "@reorder"},
            New Object() {productId, txtProductCode.Text.Trim(), If(String.IsNullOrWhiteSpace(txtSize.Text), "N/A", txtSize.Text.Trim()), Convert.ToInt32(txtQuantity.Text)})

        If ok Then
            MsgBox("Product added. Use Stock Entry to add its initial quantity.", vbInformation, "Manage Products")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
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

        If Not ValidateInputs() Then Exit Sub

        ExecNonQuery("UPDATE TBL_PRODUCTS SET product_name=@n, product_description=@d, category_type_id=@t, unit_price=@p, status=@st WHERE product_id=@id",
            New String() {"@n", "@d", "@t", "@p", "@st", "@id"},
            New Object() {txtProductName.Text.Trim(), txtProductDescription.Text.Trim(), cboTypeOfProduct.SelectedValue, Convert.ToDecimal(txtUnitPrice.Text),
                           If(String.IsNullOrWhiteSpace(txtStatus.Text), "Active", txtStatus.Text.Trim()), selectedProductId})

        Dim ok As Boolean = ExecNonQuery("UPDATE TBL_PRODUCT_VARIANTS SET product_code=@code, size=@size, reorder_level=@reorder WHERE variant_id=@vid",
            New String() {"@code", "@size", "@reorder", "@vid"},
            New Object() {txtProductCode.Text.Trim(), If(String.IsNullOrWhiteSpace(txtSize.Text), "N/A", txtSize.Text.Trim()), Convert.ToInt32(txtQuantity.Text), selectedVariantId})

        If ok Then
            MsgBox("Product updated.", vbInformation, "Manage Products")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
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
            MsgBox("Product removed.", vbInformation, "Manage Products")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        Else
            MsgBox("Cannot remove: this product already has transaction or stock-in history. Set its Status to Inactive instead.", vbExclamation, "Manage Products")
        End If
    End Sub

    ' CLEAR BUTTON
    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        ClearFields()
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
        cboCategory.SelectedIndex = -1
        cboTypeOfProduct.SelectedIndex = -1
        dgvListOfProducts.ClearSelection()
    End Sub

End Class