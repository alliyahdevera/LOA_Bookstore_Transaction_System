' frmLowLevelStocks.vb
Imports MySql.Data.MySqlClient

Public Class frmLowLevelStocks

    Private Sub frmLowLevelStocks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Low stock isn't date-based; hide the leftover date filter copied from another form
        LoadGrid("")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadGrid(searchText As String)
        Try
            If Not connection() Then Exit Sub
            Dim query As String = "SELECT v.product_code, p.product_name, p.product_description, c.category_name, ct.type_name, " &
                                  "v.size, p.unit_price, v.quantity_on_hand, v.reorder_level, " &
                                  "CASE WHEN v.quantity_on_hand = 0 THEN 'Out of Stock' ELSE 'Low Stock' END AS calc_status " &
                                  "FROM TBL_PRODUCT_VARIANTS v " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
                                  "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
                                  "WHERE v.quantity_on_hand <= v.reorder_level AND (v.product_code LIKE @s OR p.product_name LIKE @s) " &
                                  "ORDER BY v.quantity_on_hand ASC"
            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@s", "%" & searchText & "%")
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    DataGridView1.Rows.Clear()
                    While localDr.Read()
                        DataGridView1.Rows.Add(
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
            MsgBox("Error loading low stock items: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        ExportGridToCsv(DataGridView1, "LowLevelStocks")
    End Sub

End Class