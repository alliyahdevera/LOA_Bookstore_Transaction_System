' frmManageProducts.vb
Imports MySql.Data.MySqlClient

Public Class frmManageProducts

    Private selectedProductId As Integer = 0
    Private selectedVariantId As Integer = 0

    Private Sub frmManageProducts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' The designer label reads "Quantity" — this field actually sets the Reorder Level.
        ' Real on-hand quantity is only ever changed through Stock Entry.
        Label2.Text = "Reorder Level"
        LoadCategoryCombo()
        LoadGrid("")
    End Sub

    Private Sub LoadCategoryCombo()
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")
        FillCombo(ComboBox2, dt, "category_name", "category_id")
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedValue Is Nothing OrElse Not IsNumeric(ComboBox2.SelectedValue) Then Exit Sub
        Dim dt As DataTable = GetDataTable("SELECT category_type_id, type_name FROM TBL_CATEGORY_TYPES WHERE category_id = @c ORDER BY type_name",
                                            New String() {"@c"}, New Object() {ComboBox2.SelectedValue})
        FillCombo(ComboBox1, dt, "type_name", "category_type_id")
    End Sub

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
                    DataGridView1.Rows.Clear()
                    While localDr.Read()
                        Dim idx As Integer = DataGridView1.Rows.Add(
                            localDr("product_code").ToString(), localDr("product_name").ToString(), localDr("product_description").ToString(),
                            localDr("category_name").ToString(), localDr("type_name").ToString(), localDr("size").ToString(),
                            Convert.ToDecimal(localDr("unit_price")).ToString("N2"), localDr("quantity_on_hand").ToString(),
                            localDr("reorder_level").ToString(), localDr("status").ToString())
                        DataGridView1.Rows(idx).Tag = New Integer() {Convert.ToInt32(localDr("product_id")), Convert.ToInt32(localDr("variant_id"))}
                    End While
                End Using
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading products: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        If row.Tag Is Nothing Then Exit Sub
        Dim ids As Integer() = CType(row.Tag, Integer())
        selectedProductId = ids(0)
        selectedVariantId = ids(1)

        TextBox3.Text = row.Cells("ProductCode").Value.ToString()
        TextBox2.Text = row.Cells("ProductName").Value.ToString()
        TextBox7.Text = row.Cells("ProductDescription").Value.ToString()
        ComboBox2.SelectedValue = GetCategoryIdByName(row.Cells("Category").Value.ToString())
        ComboBox1.SelectedValue = GetTypeIdByName(row.Cells("TypeofProduct").Value.ToString())
        TextBox5.Text = row.Cells("Size").Value.ToString()
        TextBox4.Text = row.Cells("UnitPrice").Value.ToString()
        TextBox1.Text = row.Cells("ReorderLevel").Value.ToString()
        TextBox8.Text = row.Cells("Status").Value.ToString()
    End Sub

    Private Function GetCategoryIdByName(name As String) As Integer
        Return Convert.ToInt32(If(ExecScalar("SELECT category_id FROM TBL_CATEGORIES WHERE category_name = @n", New String() {"@n"}, New Object() {name}), 0))
    End Function

    Private Function GetTypeIdByName(name As String) As Integer
        Return Convert.ToInt32(If(ExecScalar("SELECT category_type_id FROM TBL_CATEGORY_TYPES WHERE type_name = @n", New String() {"@n"}, New Object() {name}), 0))
    End Function

    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(TextBox3.Text) OrElse String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MsgBox("Product Code and Product Name are required.", vbExclamation, "Manage Products") : Return False
        End If
        If ComboBox2.SelectedValue Is Nothing OrElse ComboBox1.SelectedValue Is Nothing Then
            MsgBox("Select a Category and Type of Product.", vbExclamation, "Manage Products") : Return False
        End If
        If Not IsNumeric(TextBox4.Text) Then
            MsgBox("Unit Price must be numeric.", vbExclamation, "Manage Products") : Return False
        End If
        If Not IsNumeric(TextBox1.Text) Then
            MsgBox("Reorder Level must be numeric.", vbExclamation, "Manage Products") : Return False
        End If
        Return True
    End Function

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can add products.", vbExclamation, "Access Denied") : Exit Sub
        End If
        If Not ValidateInputs() Then Exit Sub

        Dim productId As Integer = Convert.ToInt32(If(ExecScalar(
            "SELECT product_id FROM TBL_PRODUCTS WHERE product_name = @n AND category_type_id = @t",
            New String() {"@n", "@t"}, New Object() {TextBox2.Text.Trim(), ComboBox1.SelectedValue}), 0))

        If productId = 0 Then
            productId = CInt(ExecInsertGetId(
                "INSERT INTO TBL_PRODUCTS (product_name, product_description, category_type_id, unit_price, status) VALUES (@n, @d, @t, @p, @st)",
                New String() {"@n", "@d", "@t", "@p", "@st"},
                New Object() {TextBox2.Text.Trim(), TextBox7.Text.Trim(), ComboBox1.SelectedValue, Convert.ToDecimal(TextBox4.Text),
                               If(String.IsNullOrWhiteSpace(TextBox8.Text), "Active", TextBox8.Text.Trim())}))
            If productId = 0 Then Exit Sub
        Else
            ExecNonQuery("UPDATE TBL_PRODUCTS SET unit_price = @p, product_description = @d, status = @st WHERE product_id = @id",
                New String() {"@p", "@d", "@st", "@id"},
                New Object() {Convert.ToDecimal(TextBox4.Text), TextBox7.Text.Trim(),
                               If(String.IsNullOrWhiteSpace(TextBox8.Text), "Active", TextBox8.Text.Trim()), productId})
        End If

        Dim ok As Boolean = ExecNonQuery(
            "INSERT INTO TBL_PRODUCT_VARIANTS (product_id, product_code, size, quantity_on_hand, reorder_level) VALUES (@pid, @code, @size, 0, @reorder)",
            New String() {"@pid", "@code", "@size", "@reorder"},
            New Object() {productId, TextBox3.Text.Trim(), If(String.IsNullOrWhiteSpace(TextBox5.Text), "N/A", TextBox5.Text.Trim()), Convert.ToInt32(TextBox1.Text)})

        If ok Then
            MsgBox("Product added. Use Stock Entry to add its initial quantity.", vbInformation, "Manage Products")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        Else
            MsgBox("Could not add product. The Product Code may already be in use.", vbExclamation, "Manage Products")
        End If
    End Sub

    Private Sub btnupd_Click(sender As Object, e As EventArgs) Handles btnupd.Click
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can update products.", vbExclamation, "Access Denied") : Exit Sub
        End If
        If selectedVariantId = 0 Then
            MsgBox("Select a product from the list first.", vbExclamation, "Manage Products") : Exit Sub
        End If
        If Not ValidateInputs() Then Exit Sub

        ExecNonQuery("UPDATE TBL_PRODUCTS SET product_name=@n, product_description=@d, category_type_id=@t, unit_price=@p, status=@st WHERE product_id=@id",
            New String() {"@n", "@d", "@t", "@p", "@st", "@id"},
            New Object() {TextBox2.Text.Trim(), TextBox7.Text.Trim(), ComboBox1.SelectedValue, Convert.ToDecimal(TextBox4.Text),
                           If(String.IsNullOrWhiteSpace(TextBox8.Text), "Active", TextBox8.Text.Trim()), selectedProductId})

        Dim ok As Boolean = ExecNonQuery("UPDATE TBL_PRODUCT_VARIANTS SET product_code=@code, size=@size, reorder_level=@reorder WHERE variant_id=@vid",
            New String() {"@code", "@size", "@reorder", "@vid"},
            New Object() {TextBox3.Text.Trim(), If(String.IsNullOrWhiteSpace(TextBox5.Text), "N/A", TextBox5.Text.Trim()), Convert.ToInt32(TextBox1.Text), selectedVariantId})

        If ok Then
            MsgBox("Product updated.", vbInformation, "Manage Products")
            LoadGrid(txtSearch.Text.Trim())
        End If
    End Sub

    Private Sub btnremove_Click(sender As Object, e As EventArgs) Handles btnremove.Click
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can remove products.", vbExclamation, "Access Denied") : Exit Sub
        End If
        If selectedVariantId = 0 Then
            MsgBox("Select a product from the list first.", vbExclamation, "Manage Products") : Exit Sub
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

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedProductId = 0 : selectedVariantId = 0
        TextBox3.Clear() : TextBox2.Clear() : TextBox7.Clear()
        TextBox5.Clear() : TextBox4.Clear() : TextBox1.Clear() : TextBox8.Clear()
        DataGridView1.ClearSelection()
    End Sub

End Class