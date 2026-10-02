Imports MySql.Data.MySqlClient

Public Class frmProductList

    Private Sub frmProductList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)
        LoadCategoryCombo()
        LoadCards()
        LoadGrid("", GetSelectedCategoryId())
    End Sub

    ' Populate Category Dropdown with "All Categories" option
    Private Sub LoadCategoryCombo()
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")

        ' Add default "All Categories" option
        Dim row As DataRow = dt.NewRow()
        row("category_id") = 0
        row("category_name") = "-- All Categories --"
        dt.Rows.InsertAt(row, 0)

        FillCombo(cbocategory, dt, "category_name", "category_id")
        cbocategory.SelectedIndex = 0
    End Sub

    Private Function GetSelectedCategoryId() As Integer
        If cbocategory.SelectedValue IsNot Nothing AndAlso IsNumeric(cbocategory.SelectedValue) Then
            Return Convert.ToInt32(cbocategory.SelectedValue)
        End If
        Return 0
    End Function

    ' Trigger filter when Category selection changes
    Private Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        LoadGrid(txtSearch.Text.Trim(), GetSelectedCategoryId())
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid(txtSearch.Text.Trim(), GetSelectedCategoryId())
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

    ' Optimized Data Load with Category Filtering
    Private Sub LoadGrid(searchText As String, categoryId As Integer)
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT v.product_code, p.product_name, p.product_description, c.category_name, ct.type_name, " &
                                  "v.size, p.unit_price, v.quantity_on_hand, v.reorder_level, " &
                                  "CASE WHEN p.status = 'Inactive' THEN 'Inactive' " &
                                  "WHEN v.quantity_on_hand = 0 THEN 'Out of Stock' " &
                                  "WHEN v.quantity_on_hand <= v.reorder_level THEN 'Low Stock' " &
                                  "ELSE 'In Stock' END AS calc_status " &
                                  "FROM TBL_PRODUCT_VARIANTS v " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
                                  "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
                                  "WHERE (@cat = 0 OR c.category_id = @cat) " &
                                  "AND (v.product_code LIKE @s OR p.product_name LIKE @s) " &
                                  "ORDER BY p.product_name, v.size"

            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@cat", categoryId)
                localCmd.Parameters.AddWithValue("@s", "%" & searchText & "%")

                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    dgvlistproducts.Rows.Clear()
                    While localDr.Read()
                        dgvlistproducts.Rows.Add(
                            localDr("product_code").ToString(), localDr("product_name").ToString(), localDr("product_description").ToString(),
                            localDr("category_name").ToString(), localDr("type_name").ToString(), localDr("size").ToString(),
                            Convert.ToDecimal(localDr("unit_price")).ToString("N2"), localDr("quantity_on_hand").ToString(),
                            localDr("reorder_level").ToString(), localDr("calc_status").ToString())
                    End While
                End Using
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading products: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

End Class