' frmStockEntry.vb
Imports MySql.Data.MySqlClient

Public Class frmStockEntry

    Private currentStockInId As Integer = 0
    Private selectedVariantId As Integer = 0

    Private Sub frmStockEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label7.Text = "PRODUCTS - Click a row to select, then enter the quantity received"   ' fixes a copy-pasted header
        TextBox2.Text = NewReferenceNo()
        TextBox2.ReadOnly = True
        DateTimePicker1.Value = Today
        TextBox4.Text = DateTime.Now.ToString("hh:mm tt")
        TextBox4.ReadOnly = True
        LoadProducts()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click   ' Product List (refresh)
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click   ' Add Stock(s)
        If selectedVariantId = 0 Then
            MsgBox("Click a product row first to select which item you're stocking in.", vbExclamation, "Stock Entry") : Exit Sub
        End If
        If Not IsNumeric(TextBox1.Text) OrElse Convert.ToInt32(TextBox1.Text) <= 0 Then
            MsgBox("Enter a valid quantity.", vbExclamation, "Stock Entry") : Exit Sub
        End If
        If String.IsNullOrWhiteSpace(TextBox6.Text) Then
            MsgBox("Enter who the stock was received from (Stock In By).", vbExclamation, "Stock Entry") : Exit Sub
        End If

        ' Create the delivery header the first time an item is added under this Reference No.
        If currentStockInId = 0 Then
            currentStockInId = CInt(ExecInsertGetId(
                "INSERT INTO TBL_STOCK_INS (reference_no, received_by, stock_in_date, stock_in_time, created_by) VALUES (@r, @rb, @d, @t, @u)",
                New String() {"@r", "@rb", "@d", "@t", "@u"},
                New Object() {TextBox2.Text, TextBox6.Text.Trim(), DateTimePicker1.Value.Date, DateTime.Now.TimeOfDay, currentuser.UserID}))
            If currentStockInId = 0 Then Exit Sub
        End If

        Dim qty As Integer = Convert.ToInt32(TextBox1.Text)
        Dim ok As Boolean = ExecNonQuery("INSERT INTO TBL_STOCK_IN_DETAILS (stock_in_id, variant_id, quantity) VALUES (@s, @v, @q)",
            New String() {"@s", "@v", "@q"}, New Object() {currentStockInId, selectedVariantId, qty})

        If ok Then
            ExecNonQuery("UPDATE TBL_PRODUCT_VARIANTS SET quantity_on_hand = quantity_on_hand + @q WHERE variant_id = @v",
                New String() {"@q", "@v"}, New Object() {qty, selectedVariantId})

            MsgBox("Stock added. You can add more items under Reference No. " & TextBox2.Text & ", or leave this screen when done.", vbInformation, "Stock Entry")
            TextBox1.Clear()
            selectedVariantId = 0
            LoadProducts()
        End If
    End Sub

End Class