Imports MySql.Data.MySqlClient

Public Class frmInventoryAdjustment

    ' ---- set by frmInventoryCountReconciliation before ShowDialog ----
    Public Property VariantId As Integer
    Public Property DetailId As Integer
    Public Property CountNo As String = ""
    Public Property ProductCode As String
    Public Property ProductName As String
    Public Property ItemSize As String
    Public Property SystemQty As String
    Public Property PhysicalQty As String
    Public Property DamagedQty As Integer = 0

    Private Sub frmInventoryAdjustment_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        txtprodct.Text = ProductName
        txtprodct.ReadOnly = True

        txtsize.Text = If(String.IsNullOrWhiteSpace(ItemSize), "N/A", ItemSize)
        txtsize.Enabled = True
        txtsize.ReadOnly = True

        Dim physical As Integer
        Dim systemQuantity As Integer

        If Not Integer.TryParse(PhysicalQty, physical) OrElse physical < 0 OrElse
       Not Integer.TryParse(SystemQty, systemQuantity) OrElse systemQuantity < 0 Then

            MsgBox("The saved physical or system quantity is invalid." & vbCrLf &
               "Physical: '" & PhysicalQty & "'   System: '" & SystemQty & "'",
               vbCritical, "Inventory Adjustment")
            btnconfirm.Enabled = False
            Exit Sub
        End If

        Dim currentStock As Object = ExecScalar(
        "SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @variantId",
        New String() {"@variantId"}, New Object() {VariantId})

        If currentStock Is Nothing OrElse Convert.IsDBNull(currentStock) Then
            txtStatus.Text = "N/A"
            btnconfirm.Enabled = False
            MsgBox("The current stock could not be loaded.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If

        ' This textbox displays current stock, not count status.
        txtStatus.Text = Convert.ToString(currentStock)

        lblsysq.Text = systemQuantity.ToString()
        lblphyq.Text = physical.ToString()

        Dim difference As Integer = physical - systemQuantity
        lblDifference.Text = If(difference > 0, "+" & difference.ToString(), difference.ToString())
        lblDifference.ForeColor = If(difference = 0, Color.SeaGreen,
                                 If(difference < 0, Color.DarkOrange, Color.Firebrick))

        Label8.Text = "Damaged Units: " & Math.Max(0, Math.Min(DamagedQty, physical)).ToString()

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

        Dim physical As Integer = CInt(Val(PhysicalQty))
        Dim systemAtCount As Integer = CInt(Val(SystemQty))
        Dim damaged As Integer = Math.Max(0, DamagedQty)
        Dim currentQty As Integer = CInt(Val(txtStatus.Text))
        Dim reasonText As String = txtReason.Text.Trim()

        If damaged > physical Then
            MsgBox("Damaged units cannot be more than the physical quantity.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If
        If physical = systemAtCount AndAlso damaged = 0 Then
            MsgBox("The count matches the system and no damaged units were entered. Nothing to adjust.", vbInformation, "Inventory Adjustment")
            Exit Sub
        End If
        If reasonText = "" Then
            MsgBox("Enter the reason for the adjustment.", vbExclamation, "Inventory Adjustment")
            txtReason.Focus()
            Exit Sub
        End If
        If damaged = 0 AndAlso physical < currentQty AndAlso reasonText.ToLower().Contains("damag") Then
            MsgBox("The reason mentions damage, but no damaged units were recorded." & vbCrLf &
           "Enter them in the Damaged Qty column of the count grid, use Redo Save Count, then adjust again.",
           vbExclamation, "Inventory Adjustment")
            txtReason.Focus()
            Exit Sub
        End If
        Dim sellable As Integer = physical - damaged
        Dim msg As String = "Set the stock of " & ProductName & " (" & ItemSize & ") to " & sellable & " sellable"
        If damaged > 0 Then msg &= " and move " & damaged & " damaged unit(s) to Damaged Stocks"
        msg &= "?"
        If currentQty <> systemAtCount Then
            msg &= vbCrLf & vbCrLf & "Note: stock changed since the count (was " & systemAtCount & ", now " & currentQty & ")."
        End If
        If MsgBox(msg, vbYesNo + vbQuestion, "Confirm Adjustment") <> MsgBoxResult.Yes Then Exit Sub

        Dim approver As String = RequireSupervisorApproval(Me, "Inventory adjustment of " & ProductCode & " (" & ItemSize & "): " &
                                                          SystemQty & " to " & physical & If(damaged > 0, " (" & damaged & " damaged)", ""))
        If approver Is Nothing Then
            MsgBox("The adjustment was not saved because supervisor approval was cancelled or failed.",
           vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If

        Dim moveRemark As String = reasonText & " [Approved: " & approver & "]"
        If moveRemark.Length > 255 Then moveRemark = moveRemark.Substring(0, 255)
        Dim refNo As String = CountNo
        Dim refValue As Object = If(String.IsNullOrEmpty(refNo), CType(DBNull.Value, Object), refNo)

        Dim moveType As String = "Adjustment"
        Dim lowerReason As String = reasonText.ToLower()
        If physical < currentQty AndAlso (lowerReason.Contains("missing") OrElse lowerReason.Contains("lost")) Then moveType = "Missing"

        Try
            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        ' 1) lock + read current stock
                        Dim running As Integer
                        Using q As New MySqlCommand("SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @v FOR UPDATE", c, tx)
                            q.Parameters.AddWithValue("@v", VariantId)
                            running = Convert.ToInt32(q.ExecuteScalar())
                        End Using

                        ' 2) bring the system to the physical count
                        If running <> physical Then
                            SetStock(c, tx, physical)
                            AddMovement(c, tx, moveType, Math.Abs(physical - running), running, physical, refValue, moveRemark)
                            running = physical
                        End If

                        ' 3) damaged units leave sellable stock and go to damaged stocks
                        If damaged > 0 Then
                            SetStock(c, tx, running - damaged)
                            AddMovement(c, tx, "Damaged", damaged, running, running - damaged, refValue,
                                        (damaged & " damaged unit(s) moved to Damaged Stocks. " & moveRemark).Substring(0, Math.Min(255, (damaged & " damaged unit(s) moved to Damaged Stocks. " & moveRemark).Length)))
                            Using q As New MySqlCommand(
                                "INSERT INTO tbl_nonsaleable_stocks (variant_id, quantity, stock_condition, reason, reference_no, count_detail_id, reported_by) " &
                                "VALUES (@v, @q, 'Damaged', @r, @ref, @d, @u)", c, tx)
                                q.Parameters.AddWithValue("@v", VariantId)
                                q.Parameters.AddWithValue("@q", damaged)
                                q.Parameters.AddWithValue("@r", reasonText)
                                q.Parameters.AddWithValue("@ref", refValue)
                                q.Parameters.AddWithValue("@d", DetailId)
                                q.Parameters.AddWithValue("@u", currentuser.UserID)
                                q.ExecuteNonQuery()
                            End Using
                        End If

                        ' 4) mark the count line as adjusted
                        Dim countId As Integer = 0
                        Using q As New MySqlCommand("UPDATE tbl_inventory_count_details SET adjusted = 1, remarks = @r WHERE inventory_count_detail_id = @d", c, tx)
                            q.Parameters.AddWithValue("@r", reasonText)
                            q.Parameters.AddWithValue("@d", DetailId)
                            q.ExecuteNonQuery()
                        End Using
                        Using q As New MySqlCommand("SELECT inventory_count_id FROM tbl_inventory_count_details WHERE inventory_count_detail_id = @d", c, tx)
                            q.Parameters.AddWithValue("@d", DetailId)
                            countId = Convert.ToInt32(q.ExecuteScalar())
                        End Using

                        ' 5) close the whole count when no discrepancy is left
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
                        ProductCode & " (" & ItemSize & "): " & SystemQty & " -> " & physical &
                        If(damaged > 0, ", " & damaged & " moved to Damaged Stocks", "") &
                        ". Reason: " & reasonText & " Approved by " & approver)

            MsgBox("Inventory adjusted successfully." & If(damaged > 0, vbCrLf & damaged & " damaged unit(s) are now in Damaged Stocks.", ""),
                    vbInformation, "Inventory Adjustment")
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MsgBox("Adjustment failed and was rolled back: " & ex.Message, vbCritical, "Inventory Adjustment")
        End Try
    End Sub

    Private Sub SetStock(c As MySqlConnection, tx As MySqlTransaction, newQty As Integer)
        Using q As New MySqlCommand("UPDATE tbl_product_variants SET quantity_on_hand = @n WHERE variant_id = @v", c, tx)
            q.Parameters.AddWithValue("@n", newQty)
            q.Parameters.AddWithValue("@v", VariantId)
            q.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub AddMovement(c As MySqlConnection, tx As MySqlTransaction, moveType As String, qty As Integer,
                            prevQty As Integer, newQty As Integer, refValue As Object, remark As String)
        Using q As New MySqlCommand(
            "INSERT INTO tbl_stock_movements (variant_id, movement_type, quantity, previous_quantity, new_quantity, reference_no, remarks, created_by, created_at) " &
            "VALUES (@v, @mt, @q, @p, @n, @ref, @rm, @uid, NOW())", c, tx)
            q.Parameters.AddWithValue("@v", VariantId)
            q.Parameters.AddWithValue("@mt", moveType)
            q.Parameters.AddWithValue("@q", qty)
            q.Parameters.AddWithValue("@p", prevQty)
            q.Parameters.AddWithValue("@n", newQty)
            q.Parameters.AddWithValue("@ref", refValue)
            q.Parameters.AddWithValue("@rm", If(remark.Length > 255, remark.Substring(0, 255), remark))
            q.Parameters.AddWithValue("@uid", currentuser.UserID)
            q.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class