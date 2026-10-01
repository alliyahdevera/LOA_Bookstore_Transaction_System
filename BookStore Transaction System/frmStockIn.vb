' frmStockEntry.vb
Imports MySql.Data.MySqlClient

Public Class frmStockIn

    Private currentStockInId As Integer = 0
    Private selectedVariantId As Integer = 0

    Private Sub frmStockEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label7.Text = "PRODUCTS - Click a row to select, then enter the quantity received"   ' fixes a copy-pasted header
        txtreference.Text = NewReferenceNo()
        txtreference.ReadOnly = True
        txtstockintime.Text = DateTime.Now.ToString("hh:mm tt")
        txtstockintime.ReadOnly = True
        LoadProducts()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnproductlist.Click   ' Product List (refresh)
        LoadProducts()
    End Sub

    Private Sub LoadProducts()
        Try
            If Not connection() Then Exit Sub
            Dim query As String = "SELECT v.variant_id, v.product_code, p.product_name, p.product_description, " &
                                  "c.category_name, ct.type_name, p.unit_price, v.quantity_on_hand, v.reorder_level, " &
                                  "CASE WHEN v.quantity_on_hand = 0 THEN 'Out of Stock' WHEN v.quantity_on_hand <= v.reorder_level THEN 'Low Stock' ELSE 'In Stock' END AS calc_status " &
                                  "FROM TBL_PRODUCT_VARIANTS v " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
                                  "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
                                  "ORDER BY p.product_name"
            Using localCmd As New MySqlCommand(query, cn)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    DataGridView1.Rows.Clear()
                    While localDr.Read()
                        Dim idx As Integer = DataGridView1.Rows.Add(
                            localDr("product_code").ToString(), localDr("product_name").ToString(), localDr("product_description").ToString(),
                            localDr("category_name").ToString(), localDr("type_name").ToString(),
                            Convert.ToDecimal(localDr("unit_price")).ToString("N2"), localDr("quantity_on_hand").ToString(),
                            localDr("reorder_level").ToString(), localDr("calc_status").ToString())
                        DataGridView1.Rows(idx).Tag = Convert.ToInt32(localDr("variant_id"))
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
        selectedVariantId = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Tag)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnstocks.Click   ' Add Stock(s)
        If selectedVariantId = 0 Then
            MsgBox("Click a product row first to select which item you're stocking in.", vbExclamation, "Stock Entry") : Exit Sub
        End If
        If Not IsNumeric(txtstocks.Text) OrElse Convert.ToInt32(txtstocks.Text) <= 0 Then
            MsgBox("Enter a valid quantity.", vbExclamation, "Stock Entry") : Exit Sub
        End If

        Dim qty As Integer = Convert.ToInt32(txtstocks.Text)
        Dim ok As Boolean = ExecNonQuery("INSERT INTO TBL_STOCK_IN_DETAILS (stock_in_id, variant_id, quantity) VALUES (@s, @v, @q)",
            New String() {"@s", "@v", "@q"}, New Object() {currentStockInId, selectedVariantId, qty})

        If ok Then
            LogActivity("Stock In", txtreference.Text, "Added " & qty & " pc(s), received from ")
            ExecNonQuery("UPDATE TBL_PRODUCT_VARIANTS SET quantity_on_hand = quantity_on_hand + @q WHERE variant_id = @v",
                New String() {"@q", "@v"}, New Object() {qty, selectedVariantId})

            MsgBox("Stock added. You can add more items under Reference No. " & txtreference.Text & ", or leave this screen when done.", vbInformation, "Stock Entry")
            txtstocks.Clear()
            selectedVariantId = 0
            LoadProducts()
        End If
    End Sub

End Class