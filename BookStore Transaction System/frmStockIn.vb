Imports MySql.Data.MySqlClient

Public Class frmStockIn

    Private currentStockInId As Long = 0
    Private selectedVariantId As Integer = 0
    Private isFilling As Boolean = False

    Private Sub frmStockIn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)
        txtreference.Text = NewReferenceNo()
        txtstockintime.Text = DateTime.Now.ToString("MMMM d, yyyy  hh:mm tt")
        LoadCategoryCombo()
        LoadProducts()
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

    Private Function GetSelectedCategoryId() As Integer
        If cbocategory.SelectedValue IsNot Nothing AndAlso IsNumeric(cbocategory.SelectedValue) Then
            Return Convert.ToInt32(cbocategory.SelectedValue)
        End If
        Return 0
    End Function

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isFilling Then Exit Sub
        selectedVariantId = 0
        LoadProducts()
    End Sub

    Private Sub LoadProducts()
        Dim catId As Integer = GetSelectedCategoryId()
        Dim query As String =
            "SELECT v.variant_id, v.product_code, p.product_name, p.product_description, " &
            "c.category_name, ct.type_name, p.unit_price, v.quantity_on_hand, v.reorder_level, " &
            "CASE WHEN v.quantity_on_hand = 0 THEN 'Out of Stock' WHEN v.quantity_on_hand <= v.reorder_level THEN 'Low Stock' ELSE 'In Stock' END AS calc_status " &
            "FROM TBL_PRODUCT_VARIANTS v " &
            "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
            "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN TBL_CATEGORIES c ON ct.category_id = c.category_id " &
            "WHERE (@cat = 0 OR c.category_id = @cat) " &
            "ORDER BY p.product_name, v.size" & If(catId = 0, " LIMIT 200", "")

        Dim dt As DataTable = GetDataTable(query, New String() {"@cat"}, New Object() {catId})

        DataGridView1.SuspendLayout()
        DataGridView1.Rows.Clear()
        For Each r As DataRow In dt.Rows
            Dim idx As Integer = DataGridView1.Rows.Add(
                r("product_code").ToString(), r("product_name").ToString(), r("product_description").ToString(),
                r("category_name").ToString(), r("type_name").ToString(),
                Convert.ToDecimal(r("unit_price")).ToString("N2"), r("quantity_on_hand").ToString(),
                r("reorder_level").ToString(), r("calc_status").ToString())
            DataGridView1.Rows(idx).Tag = Convert.ToInt32(r("variant_id"))
        Next
        DataGridView1.ClearSelection()
        DataGridView1.ResumeLayout()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        selectedVariantId = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Tag)
    End Sub

    Private Sub txtstocks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtstocks.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub btnstocks_Click(sender As Object, e As EventArgs) Handles btnstocks.Click
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "Stock Entry")
            Exit Sub
        End If
        If selectedVariantId = 0 OrElse DataGridView1.CurrentRow Is Nothing Then
            MsgBox("Click a product row first to select which item you're stocking in.", vbExclamation, "Stock Entry")
            Exit Sub
        End If

        Dim qty As Integer
        If Not Integer.TryParse(txtstocks.Text.Trim(), qty) OrElse qty <= 0 Then
            MsgBox("Enter a valid quantity.", vbExclamation, "Stock Entry")
            txtstocks.Focus()
            Exit Sub
        End If

        Dim code As String = Convert.ToString(DataGridView1.CurrentRow.Cells(0).Value)
        Dim name As String = Convert.ToString(DataGridView1.CurrentRow.Cells(1).Value)
        If MsgBox("Add " & qty & " pc(s) of " & name & " (" & code & ") to stock?", vbYesNo + vbQuestion, "Stock Entry") <> MsgBoxResult.Yes Then Exit Sub

        Try
            Dim headerId As Long = currentStockInId
            Dim stamp As DateTime = DateTime.Now

            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        If headerId = 0 Then
                            Using q As New MySqlCommand(
                                "INSERT INTO tbl_stock_ins (reference_no, received_by, stock_in_date, stock_in_time, created_by) " &
                                "VALUES (@r, @by, @d, @t, @uid)", c, tx)
                                q.Parameters.AddWithValue("@r", txtreference.Text.Trim())
                                q.Parameters.AddWithValue("@by", currentuser.FullName)
                                q.Parameters.AddWithValue("@d", stamp.Date)
                                q.Parameters.AddWithValue("@t", stamp.TimeOfDay)
                                q.Parameters.AddWithValue("@uid", currentuser.UserID)
                                q.ExecuteNonQuery()
                                headerId = q.LastInsertedId
                            End Using
                        End If

                        Using q As New MySqlCommand("INSERT INTO tbl_stock_in_details (stock_in_id, variant_id, quantity) VALUES (@s, @v, @q)", c, tx)
                            q.Parameters.AddWithValue("@s", headerId)
                            q.Parameters.AddWithValue("@v", selectedVariantId)
                            q.Parameters.AddWithValue("@q", qty)
                            q.ExecuteNonQuery()
                        End Using

                        Using q As New MySqlCommand("UPDATE tbl_product_variants SET quantity_on_hand = quantity_on_hand + @q WHERE variant_id = @v", c, tx)
                            q.Parameters.AddWithValue("@q", qty)
                            q.Parameters.AddWithValue("@v", selectedVariantId)
                            q.ExecuteNonQuery()
                        End Using

                        tx.Commit()
                    Catch
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            If currentStockInId = 0 Then txtstockintime.Text = stamp.ToString("MMMM d, yyyy  hh:mm tt")
            currentStockInId = headerId

            LogActivity("Stock In", txtreference.Text, "Added " & qty & " pc(s) of " & code)
            MsgBox("Stock added. You can add more items under Reference No. " & txtreference.Text & ", or leave this screen when done.", vbInformation, "Stock Entry")

            txtstocks.Clear()
            selectedVariantId = 0
            LoadProducts()
        Catch ex As Exception
            MsgBox("Stock-in failed and was rolled back: " & ex.Message, vbCritical, "Stock Entry")
        End Try
    End Sub

End Class