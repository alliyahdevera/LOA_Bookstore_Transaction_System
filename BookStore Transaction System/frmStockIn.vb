Imports MySql.Data.MySqlClient

Public Class frmStockIn

    Private currentStockInId As Long = 0       ' 0 until the first item is added
    Private selectedVariantId As Integer = 0
    Private selectedItemLabel As String = ""

    Private Sub frmStockIn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label7.Text = "PRODUCTS - Click a row to select, then enter the quantity received"
        txtreference.Text = NewReferenceNo()
        txtreference.ReadOnly = True
        txtstockintime.Text = DateTime.Now.ToString("hh:mm tt")
        txtstockintime.ReadOnly = True

        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.MultiSelect = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        LoadProducts()
    End Sub

    Private Sub btnproductlist_Click(sender As Object, e As EventArgs) 
        LoadProducts()
    End Sub

    ' digits only in the quantity box
    Private Sub txtstocks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtstocks.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
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
                                  "WHERE p.status = 'Active' " &
                                  "ORDER BY p.product_name, v.size"
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

        selectedVariantId = 0
        selectedItemLabel = ""
        DataGridView1.ClearSelection()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        If row.Tag Is Nothing Then Exit Sub
        selectedVariantId = Convert.ToInt32(row.Tag)
        selectedItemLabel = Convert.ToString(row.Cells(0).Value) & " - " & Convert.ToString(row.Cells(1).Value)
    End Sub

    ' ==================== ADD STOCK ====================
    Private Sub btnstocks_Click(sender As Object, e As EventArgs) Handles btnstocks.Click
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "Stock Entry")
            Exit Sub
        End If
        If selectedVariantId = 0 Then
            MsgBox("Click a product row first to select which item you're stocking in.", vbExclamation, "Stock Entry")
            Exit Sub
        End If

        Dim qty As Integer
        If Not Integer.TryParse(txtstocks.Text.Trim(), qty) OrElse qty <= 0 Then
            MsgBox("Enter a valid quantity.", vbExclamation, "Stock Entry")
            txtstocks.Focus()
            Exit Sub
        End If

        Dim headerId As Long = currentStockInId

        Try
            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        ' 1) header - only once per Reference No.
                        If headerId = 0 Then
                            Using q As New MySqlCommand(
                                "INSERT INTO tbl_stock_ins (reference_no, received_by, stock_in_date, stock_in_time, created_by) " &
                                "VALUES (@ref, @rb, CURDATE(), CURTIME(), @uid)", c, tx)
                                q.Parameters.AddWithValue("@ref", txtreference.Text.Trim())
                                q.Parameters.AddWithValue("@rb", currentuser.FullName)
                                q.Parameters.AddWithValue("@uid", currentuser.UserID)
                                q.ExecuteNonQuery()
                                headerId = q.LastInsertedId
                            End Using
                        End If

                        ' 2) detail line
                        Using q As New MySqlCommand(
                            "INSERT INTO tbl_stock_in_details (stock_in_id, variant_id, quantity) VALUES (@s, @v, @q)", c, tx)
                            q.Parameters.AddWithValue("@s", headerId)
                            q.Parameters.AddWithValue("@v", selectedVariantId)
                            q.Parameters.AddWithValue("@q", qty)
                            q.ExecuteNonQuery()
                        End Using

                        ' 3) add to quantity on hand (row locked while we update)
                        Dim prev As Integer
                        Using q As New MySqlCommand("SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @v FOR UPDATE", c, tx)
                            q.Parameters.AddWithValue("@v", selectedVariantId)
                            prev = Convert.ToInt32(q.ExecuteScalar())
                        End Using
                        Dim nw As Integer = prev + qty

                        Using q As New MySqlCommand("UPDATE tbl_product_variants SET quantity_on_hand = @n WHERE variant_id = @v", c, tx)
                            q.Parameters.AddWithValue("@n", nw)
                            q.Parameters.AddWithValue("@v", selectedVariantId)
                            q.ExecuteNonQuery()
                        End Using

                        ' 4) stock movement history
                        Using q As New MySqlCommand(
                            "INSERT INTO tbl_stock_movements (variant_id, movement_type, quantity, previous_quantity, new_quantity, reference_no, remarks, created_by, created_at) " &
                            "VALUES (@v, 'Stock In', @q, @p, @n, @ref, @rm, @uid, NOW())", c, tx)
                            q.Parameters.AddWithValue("@v", selectedVariantId)
                            q.Parameters.AddWithValue("@q", qty)
                            q.Parameters.AddWithValue("@p", prev)
                            q.Parameters.AddWithValue("@n", nw)
                            q.Parameters.AddWithValue("@ref", txtreference.Text.Trim())
                            q.Parameters.AddWithValue("@rm", "Stock received")
                            q.Parameters.AddWithValue("@uid", currentuser.UserID)
                            q.ExecuteNonQuery()
                        End Using

                        tx.Commit()
                    Catch
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            currentStockInId = headerId    ' remember it so the next item shares this Reference No.

            LogActivity("Stock In", txtreference.Text.Trim(), "Received " & qty & " pc(s) of " & selectedItemLabel)

            MsgBox("Stock added. You can add more items under Reference No. " & txtreference.Text &
                   ", or leave this screen when done.", vbInformation, "Stock Entry")
            txtstocks.Clear()
            LoadProducts()

        Catch ex As Exception
            MsgBox("Stock In failed and was rolled back: " & ex.Message, vbCritical, "Stock Entry")
        End Try
    End Sub

End Class