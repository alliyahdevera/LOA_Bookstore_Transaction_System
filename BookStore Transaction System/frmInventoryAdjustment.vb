
Imports MySql.Data.MySqlClient

Public Class frmInventoryAdjustment

    ' ---- set by frmInventoryCountReconciliation before ShowDialog ----
    Public Property VariantId As Integer
    Public Property DetailId As Integer          ' tbl_inventory_count_details.inventory_count_detail_id
    Public Property CountNo As String = ""
    Public Property ProductCode As String
    Public Property ProductName As String
    Public Property ItemSize As String
    Public Property SystemQty As String
    Public Property PhysicalQty As String

    Private Sub frmInventoryAdjustment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' display-only product/size (works whatever DropDownStyle the designer used)
        txtProductname.Items.Clear()
        txtProductname.Items.Add(ProductName)
        txtProductname.SelectedIndex = 0
        txtProductname.Enabled = False

        txtsize.Items.Clear()
        txtsize.Items.Add(ItemSize)
        txtsize.SelectedIndex = 0
        txtsize.Enabled = False

        ' "Available Stock" = what the system has right now
        Dim cur As Object = ExecScalar("SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @v",
                                       New String() {"@v"}, New Object() {VariantId})
        txtStatus.ReadOnly = True
        txtStatus.Text = If(cur Is Nothing, "-", Convert.ToString(cur))

        lblsysq.Text = SystemQty
        lblphyq.Text = PhysicalQty
        Dim diff As Integer = CInt(Val(PhysicalQty)) - CInt(Val(SystemQty))
        lblDifference.Text = If(diff > 0, "+" & diff, diff.ToString())
        lblDifference.ForeColor = If(diff < 0, Color.Firebrick, If(diff > 0, Color.DarkOrange, Color.SeaGreen))

        txtReason.MaxLength = 255
    End Sub

    Private Sub btnconfirm_Click(sender As Object, e As EventArgs) Handles btnconfirm.Click
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If
        If DetailId <= 0 OrElse VariantId <= 0 Then
            MsgBox("Save the count first, then adjust.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtReason.Text) Then
            MsgBox("Enter the reason for the adjustment.", vbExclamation, "Inventory Adjustment")
            txtReason.Focus()
            Exit Sub
        End If

        Dim physical As Integer = CInt(Val(PhysicalQty))
        Dim systemAtCount As Integer = CInt(Val(SystemQty))
        Dim reason As String = txtReason.Text.Trim()

        ' warn if sales/stock-ins happened after the count was taken
        Dim currentQty As Integer = CInt(Val(txtStatus.Text))
        Dim msg As String = "Set the stock of " & ProductName & " (" & ItemSize & ") to " & physical & "?"
        If currentQty <> systemAtCount Then
            msg &= vbCrLf & vbCrLf & "Note: stock changed since the count (was " & systemAtCount & ", now " & currentQty & ")."
        End If
        If MsgBox(msg, vbYesNo + vbQuestion, "Confirm Adjustment") <> MsgBoxResult.Yes Then Exit Sub
        Dim approver As String = RequireSupervisorApproval(Me, "Inventory adjustment of " & ProductCode & " (" & ItemSize & "): " & SystemQty & " to " & physical)
        If approver Is Nothing Then Exit Sub

        Dim moveRemark As String = reason & " [Approved: " & approver & "]"
        If moveRemark.Length > 255 Then moveRemark = moveRemark.Substring(0, 255)
        Dim refNo As String = CountNo
        Dim moveType As String = "Adjustment"
        Dim lowerReason As String = reason.ToLower()
        If physical < currentQty Then
            If lowerReason.Contains("damag") Then
                moveType = "Damaged"
            ElseIf lowerReason.Contains("missing") OrElse lowerReason.Contains("lost") Then
                moveType = "Missing"
            End If
        End If

        Try
            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        ' 1) lock + read current stock
                        Dim prev As Integer
                        Using q As New MySqlCommand("SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @v FOR UPDATE", c, tx)
                            q.Parameters.AddWithValue("@v", VariantId)
                            prev = Convert.ToInt32(q.ExecuteScalar())
                        End Using

                        ' 2) set stock to the physical count + movement history
                        If prev <> physical Then
                            Using q As New MySqlCommand("UPDATE tbl_product_variants SET quantity_on_hand = @n WHERE variant_id = @v", c, tx)
                                q.Parameters.AddWithValue("@n", physical)
                                q.Parameters.AddWithValue("@v", VariantId)
                                q.ExecuteNonQuery()
                            End Using

                            Using q As New MySqlCommand(
                                "INSERT INTO tbl_stock_movements (variant_id, movement_type, quantity, previous_quantity, new_quantity, reference_no, remarks, created_by, created_at) " &
                                "VALUES (@v, @mt, @q, @p, @n, @ref, @rm, @uid, NOW())", c, tx)
                                q.Parameters.AddWithValue("@v", VariantId)
                                q.Parameters.AddWithValue("@mt", moveType)
                                q.Parameters.AddWithValue("@q", Math.Abs(physical - prev))
                                q.Parameters.AddWithValue("@p", prev)
                                q.Parameters.AddWithValue("@n", physical)
                                q.Parameters.AddWithValue("@ref", If(String.IsNullOrEmpty(refNo), CType(DBNull.Value, Object), refNo))
                                q.Parameters.AddWithValue("@rm", moveRemark)
                                q.Parameters.AddWithValue("@uid", currentuser.UserID)
                                q.ExecuteNonQuery()
                            End Using
                        End If

                        ' 3) mark the count line as adjusted
                        Dim countId As Integer = 0
                        Using q As New MySqlCommand("UPDATE tbl_inventory_count_details SET adjusted = 1, remarks = @r WHERE inventory_count_detail_id = @d", c, tx)
                            q.Parameters.AddWithValue("@r", reason)
                            q.Parameters.AddWithValue("@d", DetailId)
                            q.ExecuteNonQuery()
                        End Using
                        Using q As New MySqlCommand("SELECT inventory_count_id FROM tbl_inventory_count_details WHERE inventory_count_detail_id = @d", c, tx)
                            q.Parameters.AddWithValue("@d", DetailId)
                            countId = Convert.ToInt32(q.ExecuteScalar())
                        End Using

                        ' 4) close the whole count when no discrepancy is left
                        Using q As New MySqlCommand(
                            "UPDATE tbl_inventory_counts SET status = IF((SELECT COUNT(*) FROM tbl_inventory_count_details " &
                            "WHERE inventory_count_id = @c AND status <> 'Matched' AND adjusted = 0) = 0, 'Reconciled', 'Pending') " &
                            "WHERE inventory_count_id = @c", c, tx)
                            q.Parameters.AddWithValue("@c", countId)
                            q.ExecuteNonQuery()
                        End Using

                        tx.Commit()
                    Catch
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            LogActivity("Inventory Adjustment", refNo,
                        ProductCode & " (" & ItemSize & "): " & SystemQty & " -> " & physical & ". Reason: " & reason & " Approved by " & approver)

            MsgBox("Inventory adjusted successfully.", vbInformation, "Inventory Adjustment")
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MsgBox("Adjustment failed and was rolled back: " & ex.Message, vbCritical, "Inventory Adjustment")
        End Try
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class