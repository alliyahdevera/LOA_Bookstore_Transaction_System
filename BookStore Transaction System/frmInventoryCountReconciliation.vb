Imports MySql.Data.MySqlClient

Public Class frmInventoryCountReconciliation

    Private Class CountRow
        Public VariantId As Integer
        Public DetailId As Integer = 0        ' > 0 once the line is saved
        Public Adjusted As Boolean = False
    End Class

    ' Moved NewCountNo out of CountRow to form level
    Private Function NewCountNo() As String
        Return "CNT-" & DateTime.Now.ToString("yyyyMMddHHmmss")
    End Function

    Private currentCountId As Long = 0
    Private currentCountNo As String = ""
    Private isLoading As Boolean = True
    Private activeCard As String = ""          ' "", counted, matched, discrepancy, short, excess
    Private lastCategoryIndex As Integer = 0
    Private lastTypeIndex As Integer = 0

    ' ==================== LOAD ====================
    Private Sub frmInventoryCountReconciliation_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        txtGrandTotal.Text = Date.Today.ToString("MMMM d, yyyy")

        cbocategory.DropDownStyle = ComboBoxStyle.DropDownList
        cbocategory.Items.Clear()
        cbocategory.Items.Add("All Categories")

        Dim cats As DataTable = GetDataTable("SELECT category_name FROM tbl_categories ORDER BY category_name")
        If cats IsNot Nothing Then
            For Each r As DataRow In cats.Rows
                cbocategory.Items.Add(r("category_name").ToString())
            Next
        End If
        cbocategory.SelectedIndex = 0
        lblname.Text = If(Not String.IsNullOrEmpty(currentuser.FullName), currentuser.FullName, "N/A")
        lblposition.Text = If(Not String.IsNullOrEmpty(currentuser.Role), currentuser.Role, "N/A")
        cboType.DropDownStyle = ComboBoxStyle.DropDownList
        LoadTypeCombo()
        SetupCards()
        dgvlistproducts.Columns.Insert(0, New DataGridViewTextBoxColumn With {.Name = "CountNo", .HeaderText = "Count No.", .Width = 150})
        currentCountNo = NewCountNo()
        txtCountNo.Text = currentCountNo
        With dgvlistproducts
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = False
            For Each col As DataGridViewColumn In .Columns
                col.ReadOnly = Not (col.Name = "PhysicalQuantity" OrElse col.Name = "Remarks")
            Next
        End With

        isLoading = False
        LoadProducts()
    End Sub

    ' ==================== CATEGORY / TYPE FILTERS ====================
    Private Sub LoadTypeCombo()
        cboType.Items.Clear()
        cboType.Items.Add("All Types")

        Dim dt As DataTable
        If cbocategory.SelectedIndex <= 0 Then
            dt = GetDataTable("SELECT DISTINCT type_name FROM tbl_category_types ORDER BY type_name")
        Else
            dt = GetDataTable("SELECT ct.type_name FROM tbl_category_types ct " &
                              "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
                              "WHERE c.category_name = @c ORDER BY ct.type_name",
                              New String() {"@c"}, New Object() {Convert.ToString(cbocategory.SelectedItem)})
        End If
        For Each r As DataRow In dt.Rows
            cboType.Items.Add(r("type_name").ToString())
        Next
        cboType.SelectedIndex = 0
        lastTypeIndex = 0
    End Sub

    ' Changing a filter reloads the sheet - ask first if there are unsaved counts.
    Private Function ConfirmReload() As Boolean
        If Not HasUnsavedEntries() Then Return True
        Return MsgBox("You have counts that are not saved yet. Discard them and reload the list?",
                      vbYesNo + vbQuestion, "Inventory Count") = MsgBoxResult.Yes
    End Function

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isLoading Then Exit Sub
        If Not ConfirmReload() Then
            isLoading = True : cbocategory.SelectedIndex = lastCategoryIndex : isLoading = False
            Exit Sub
        End If
        lastCategoryIndex = cbocategory.SelectedIndex
        isLoading = True
        LoadTypeCombo()
        isLoading = False
        LoadProducts()
    End Sub

    Private Sub cboType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboType.SelectedIndexChanged
        If isLoading Then Exit Sub
        If Not ConfirmReload() Then
            isLoading = True : cboType.SelectedIndex = lastTypeIndex : isLoading = False
            Exit Sub
        End If
        lastTypeIndex = cboType.SelectedIndex
        LoadProducts()
    End Sub

    ' ==================== SUMMARY CARDS (click to filter the list) ====================
    Private Sub SetupCards()
        WireCard(Panel10, "counted")        ' Total Items Counted
        WireCard(Panel8, "matched")         ' Matched Items
        WireCard(Panel7, "discrepancy")     ' With Discrepancies
        WireCard(Panel9, "short")           ' Short / Missing
        WireCard(Panel11, "excess")         ' Excess
    End Sub

    Private Sub WireCard(card As Panel, key As String)
        card.Tag = key
        card.Cursor = Cursors.Hand
        AddHandler card.Click, AddressOf Card_Click
        For Each c As Control In card.Controls
            c.Cursor = Cursors.Hand
            c.Tag = key
            AddHandler c.Click, AddressOf Card_Click
        Next
    End Sub

    Private Sub Card_Click(sender As Object, e As EventArgs)
        Dim key As String = Convert.ToString(DirectCast(sender, Control).Tag)
        activeCard = If(activeCard = key, "", key)      ' click the same card again to show everything
        ApplyCardFilter()
    End Sub

    Private Function RowMatchesCard(row As DataGridViewRow) As Boolean
        If activeCard = "" Then Return True
        Dim d As String = Convert.ToString(row.Cells("Difference").Value)
        Dim diff As Integer
        If d = "" OrElse Not Integer.TryParse(d, diff) Then Return False     ' not counted yet
        Select Case activeCard
            Case "counted" : Return True
            Case "matched" : Return diff = 0
            Case "discrepancy" : Return diff <> 0
            Case "short" : Return diff < 0
            Case "excess" : Return diff > 0
        End Select
        Return True
    End Function

    Private Sub ApplyCardFilter()
        dgvlistproducts.EndEdit()
        dgvlistproducts.CurrentCell = Nothing       ' a visible row must not be hidden while it is current
        For Each row As DataGridViewRow In dgvlistproducts.Rows
            If row.IsNewRow Then Continue For
            row.Visible = RowMatchesCard(row)
        Next
        dgvlistproducts.ClearSelection()

        ' highlight the active card
        For Each p As Panel In New Panel() {Panel10, Panel8, Panel7, Panel9, Panel11}
            p.BorderStyle = If(Convert.ToString(p.Tag) = activeCard, BorderStyle.FixedSingle, BorderStyle.None)
        Next
    End Sub

    ' ==================== LOAD PRODUCTS ====================
    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        If HasUnsavedEntries() Then
            If MsgBox("You have counts that are not saved yet. Discard them and reload the list?",
                      vbYesNo + vbQuestion, "Inventory Count") <> MsgBoxResult.Yes Then Exit Sub
        End If
        LoadProducts()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            btngenerate.PerformClick()
        End If
    End Sub

    Private Sub LoadProducts()
        dgvlistproducts.Rows.Clear()

        Dim keyword As String = txtSearch.Text.Trim()
        Dim category As String = If(cbocategory.SelectedIndex <= 0, "", cbocategory.Text)
        Dim typeName As String = If(cboType.SelectedIndex <= 0, "", Convert.ToString(cboType.SelectedItem))

        Dim query As String =
            "SELECT v.variant_id, v.product_code, p.product_name, c.category_name, ct.type_name, v.size, " &
            "v.quantity_on_hand, d.inventory_count_detail_id, d.system_quantity, d.physical_quantity, " &
            "d.status AS count_status, d.remarks, d.adjusted " &
            "FROM tbl_product_variants v " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "LEFT JOIN tbl_inventory_count_details d ON d.variant_id = v.variant_id AND d.inventory_count_id = @cid " &
            "WHERE p.status = 'Active' AND (v.product_code LIKE @s OR p.product_name LIKE @s) "

        Dim names As New List(Of String)({"@cid", "@s"})
        Dim values As New List(Of Object)({CType(currentCountId, Object), "%" & keyword & "%"})

        If category <> "" Then
            query &= "AND c.category_name = @cat "
            names.Add("@cat")
            values.Add(category)
        End If
        If typeName <> "" Then
            query &= "AND ct.type_name = @type "
            names.Add("@type")
            values.Add(typeName)
        End If
        query &= "ORDER BY p.product_name, v.size"

        Dim dt As DataTable = GetDataTable(query, names.ToArray(), values.ToArray())

        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                Dim idx As Integer = dgvlistproducts.Rows.Add()
                Dim row As DataGridViewRow = dgvlistproducts.Rows(idx)
                Dim info As New CountRow With {.VariantId = Convert.ToInt32(r("variant_id"))}

                row.Cells("ProductCode").Value = r("product_code").ToString()
                row.Cells("CountNo").Value = currentCountNo
                row.Cells("ProductName").Value = r("product_name").ToString()
                row.Cells("Category").Value = r("category_name").ToString()
                row.Cells("TypeofProduct").Value = r("type_name").ToString()
                row.Cells("Size").Value = r("size").ToString()

                If Not IsDBNull(r("inventory_count_detail_id")) AndAlso Convert.ToInt32(r("inventory_count_detail_id")) > 0 Then
                    info.DetailId = Convert.ToInt32(r("inventory_count_detail_id"))
                    info.Adjusted = Convert.ToInt32(r("adjusted")) = 1
                    row.Cells("SystemQuantity").Value = Convert.ToInt32(r("system_quantity"))
                    row.Cells("PhysicalQuantity").Value = Convert.ToInt32(r("physical_quantity"))
                    row.Cells("Remarks").Value = If(IsDBNull(r("remarks")), "", r("remarks").ToString())
                Else
                    row.Cells("SystemQuantity").Value = If(IsDBNull(r("quantity_on_hand")), 0, Convert.ToInt32(r("quantity_on_hand")))
                    row.Cells("PhysicalQuantity").Value = ""
                    row.Cells("Remarks").Value = ""
                End If

                row.Tag = info
                RefreshRow(row)
            Next
        End If

        dgvlistproducts.ClearSelection()
        UpdateCards()
        ApplyCardFilter()
    End Sub

    ' ==================== TYPING THE PHYSICAL COUNT ====================
    Private Sub dgvlistproducts_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvlistproducts.CellBeginEdit
        If e.RowIndex < 0 Then Exit Sub
        Dim info As CountRow = TryCast(dgvlistproducts.Rows(e.RowIndex).Tag, CountRow)
        ' Lock line if already saved
        If info IsNot Nothing AndAlso info.DetailId > 0 Then e.Cancel = True
    End Sub

    Private Sub dgvlistproducts_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvlistproducts.EditingControlShowing
        Dim tb As TextBox = TryCast(e.Control, TextBox)
        If tb Is Nothing Then Exit Sub
        RemoveHandler tb.KeyPress, AddressOf DigitsOnly_KeyPress
        If dgvlistproducts.CurrentCell IsNot Nothing AndAlso dgvlistproducts.CurrentCell.OwningColumn.Name = "PhysicalQuantity" Then
            AddHandler tb.KeyPress, AddressOf DigitsOnly_KeyPress
        End If
    End Sub

    Private Sub DigitsOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub dgvlistproducts_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvlistproducts.CellEndEdit
        If e.RowIndex < 0 Then Exit Sub
        If dgvlistproducts.Columns(e.ColumnIndex).Name <> "PhysicalQuantity" Then Exit Sub
        RefreshRow(dgvlistproducts.Rows(e.RowIndex))
        UpdateCards()
    End Sub

    Private Function StatusOf(diff As Integer) As String
        If diff = 0 Then Return "Matched"
        If diff < 0 Then Return "Short"
        Return "Excess"
    End Function

    Private Sub RefreshRow(row As DataGridViewRow)
        Dim info As CountRow = TryCast(row.Tag, CountRow)
        Dim txt As String = Convert.ToString(row.Cells("PhysicalQuantity").Value).Trim()
        Dim phys As Integer

        If txt = "" OrElse Not Integer.TryParse(txt, phys) OrElse phys < 0 Then
            row.Cells("PhysicalQuantity").Value = ""
            row.Cells("Difference").Value = ""
            row.Cells("Status").Value = "Not counted"
            row.DefaultCellStyle.BackColor = Color.White
            row.DefaultCellStyle.ForeColor = Color.Black
            Exit Sub
        End If

        Dim sys As Integer = 0
        Integer.TryParse(Convert.ToString(row.Cells("SystemQuantity").Value), sys)

        Dim diff As Integer = phys - sys
        Dim adjusted As Boolean = (info IsNot Nothing AndAlso info.Adjusted)

        row.Cells("PhysicalQuantity").Value = phys
        row.Cells("Difference").Value = diff
        row.Cells("Status").Value = StatusOf(diff) & If(adjusted, " (Adjusted)", "")

        row.DefaultCellStyle.ForeColor = If(adjusted, Color.Gray, Color.Black)
        If diff = 0 Then
            row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233)
        ElseIf diff < 0 Then
            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238)
        Else
            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 225)
        End If
    End Sub

    Private Sub UpdateCards()
        Dim counted As Integer = 0, matched As Integer = 0, shortCount As Integer = 0, excess As Integer = 0

        For Each row As DataGridViewRow In dgvlistproducts.Rows
            Dim d As Object = row.Cells("Difference").Value
            If d Is Nothing OrElse Convert.ToString(d) = "" Then Continue For

            Dim diff As Integer = 0
            If Integer.TryParse(Convert.ToString(d), diff) Then
                counted += 1
                If diff = 0 Then
                    matched += 1
                ElseIf diff < 0 Then
                    shortCount += 1
                Else
                    excess += 1
                End If
            End If
        Next

        lbltotalproducts.Text = counted.ToString("N0")
        lbltotalqproducts.Text = matched.ToString("N0")
        lblonhand.Text = (shortCount + excess).ToString("N0")
        lblcriticallvl.Text = shortCount.ToString("N0")
        lbloutofstocks.Text = excess.ToString("N0")
    End Sub

    Private Function HasUnsavedEntries() As Boolean
        For Each row As DataGridViewRow In dgvlistproducts.Rows
            Dim info As CountRow = TryCast(row.Tag, CountRow)
            If info IsNot Nothing AndAlso info.DetailId = 0 AndAlso
               Convert.ToString(row.Cells("PhysicalQuantity").Value).Trim() <> "" Then Return True
        Next
        Return False
    End Function
    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "Inventory Count")
            Exit Sub
        End If

        ' Filter pending rows safely
        Dim pending As New List(Of DataGridViewRow)
        For Each row As DataGridViewRow In dgvlistproducts.Rows
            If row.IsNewRow Then Continue For
            Dim info As CountRow = TryCast(row.Tag, CountRow)
            Dim physVal As String = Convert.ToString(row.Cells("PhysicalQuantity").Value).Trim()

            If info IsNot Nothing AndAlso info.DetailId = 0 AndAlso physVal <> "" Then
                pending.Add(row)
            End If
        Next

        If pending.Count = 0 Then
            MsgBox("Type the physical quantity of at least one item first.", vbExclamation, "Inventory Count")
            Exit Sub
        End If

        Dim discrepancies As Integer = 0
        For Each row As DataGridViewRow In pending
            Dim diff As Integer = 0
            Integer.TryParse(Convert.ToString(row.Cells("Difference").Value), diff)
            If diff <> 0 Then discrepancies += 1
        Next

        If MsgBox("Save " & pending.Count & " counted item(s) (" & discrepancies & " with discrepancy)?",
              vbYesNo + vbQuestion, "Save Count") <> MsgBoxResult.Yes Then Exit Sub

        Try
            Dim countId As Long = currentCountId
            Dim countNo As String = currentCountNo

            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        ' Create header record if not created yet
                        If countId = 0 Then
                            If String.IsNullOrEmpty(countNo) Then countNo = NewCountNo()
                            Using q As New MySqlCommand(
                            "INSERT INTO tbl_inventory_counts (count_no, count_date, prepared_by, status, remarks) " &
                            "VALUES (@no, CURDATE(), @uid, 'Pending', NULL)", c, tx)
                                q.Parameters.AddWithValue("@no", countNo)
                                q.Parameters.AddWithValue("@uid", currentuser.UserID)
                                q.ExecuteNonQuery()
                                countId = q.LastInsertedId
                            End Using
                        End If

                        ' Insert details
                        For Each row As DataGridViewRow In pending
                            Dim info As CountRow = CType(row.Tag, CountRow)

                            Dim sys As Integer = 0
                            Dim phys As Integer = 0
                            Integer.TryParse(Convert.ToString(row.Cells("SystemQuantity").Value), sys)
                            Integer.TryParse(Convert.ToString(row.Cells("PhysicalQuantity").Value), phys)

                            Dim diff As Integer = phys - sys
                            Dim remarks As String = Convert.ToString(row.Cells("Remarks").Value).Trim()

                            Using q As New MySqlCommand(
                            "INSERT INTO tbl_inventory_count_details " &
                            "(inventory_count_id, variant_id, system_quantity, physical_quantity, difference, status, remarks, adjusted) " &
                            "VALUES (@c, @v, @s, @p, @d, @st, @r, 0)", c, tx)
                                q.Parameters.AddWithValue("@c", countId)
                                q.Parameters.AddWithValue("@v", info.VariantId)
                                q.Parameters.AddWithValue("@s", sys)
                                q.Parameters.AddWithValue("@p", phys)
                                q.Parameters.AddWithValue("@d", diff)
                                q.Parameters.AddWithValue("@st", StatusOf(diff))
                                q.Parameters.AddWithValue("@r", If(remarks = "", CType(DBNull.Value, Object), remarks))
                                q.ExecuteNonQuery()

                                info.DetailId = CInt(q.LastInsertedId)
                            End Using
                        Next

                        ' Update header status
                        Using q As New MySqlCommand(
                        "UPDATE tbl_inventory_counts SET status = IF((SELECT COUNT(*) FROM tbl_inventory_count_details " &
                        "WHERE inventory_count_id = @c AND status <> 'Matched' AND adjusted = 0) = 0, 'Reconciled', 'Pending') " &
                        "WHERE inventory_count_id = @c", c, tx)
                            q.Parameters.AddWithValue("@c", countId)
                            q.ExecuteNonQuery()
                        End Using

                        tx.Commit()
                    Catch ex As Exception
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            currentCountId = countId
            currentCountNo = countNo

            LogActivity("Inventory Count", countNo, "Saved " & pending.Count & " counted item(s), " & discrepancies & " discrepancy(ies)")

            MsgBox("Count " & countNo & " saved." & vbCrLf &
               If(discrepancies > 0, "To correct a Short/Excess item, select its row and click 'Adjust Inventory'.", "No discrepancies found."),
               vbInformation, "Inventory Count")

        Catch ex As Exception
            MsgBox("Saving the count failed and was rolled back: " & ex.Message, vbCritical, "Inventory Count")
        End Try
    End Sub

    ' ==================== ADJUST INVENTORY ====================
    Private Sub btnreconcile_Click(sender As Object, e As EventArgs) Handles btnreconcile.Click
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If
        If dgvlistproducts.CurrentRow Is Nothing OrElse Not dgvlistproducts.CurrentRow.Selected Then
            MsgBox("Select a product row first.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If

        Dim row As DataGridViewRow = dgvlistproducts.CurrentRow
        Dim info As CountRow = TryCast(row.Tag, CountRow)
        If info Is Nothing Then Exit Sub

        If info.DetailId = 0 Then
            MsgBox("Click 'Save Count' first, then adjust.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If
        If info.Adjusted Then
            MsgBox("This item was already adjusted.", vbInformation, "Inventory Adjustment")
            Exit Sub
        End If

        Dim sys As Integer = Convert.ToInt32(row.Cells("SystemQuantity").Value)
        Dim phys As Integer = Convert.ToInt32(row.Cells("PhysicalQuantity").Value)
        If sys = phys Then
            MsgBox("This item matches the system quantity. Nothing to adjust.", vbInformation, "Inventory Adjustment")
            Exit Sub
        End If

        Using frm As New frmInventoryAdjustment()
            frm.VariantId = info.VariantId
            frm.DetailId = info.DetailId
            frm.CountNo = currentCountNo
            frm.ProductCode = Convert.ToString(row.Cells("ProductCode").Value)
            frm.ProductName = Convert.ToString(row.Cells("ProductName").Value)
            frm.ItemSize = Convert.ToString(row.Cells("Size").Value)
            frm.SystemQty = sys.ToString()
            frm.PhysicalQty = phys.ToString()
            frm.StartPosition = FormStartPosition.CenterParent

            If frm.ShowDialog(Me) = DialogResult.OK Then
                info.Adjusted = True
                RefreshRow(row)
                UpdateCards()
            End If
        End Using
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

    ' ==================== CLEAR / CANCEL ====================
    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        If HasUnsavedEntries() Then
            If MsgBox("Discard the counts that are not saved yet and start a new count sheet?",
                      vbYesNo + vbQuestion, "Inventory Count") <> MsgBoxResult.Yes Then Exit Sub
        End If
        currentCountId = 0
        currentCountNo = NewCountNo()
        txtCountNo.Text = currentCountNo
        txtSearch.Clear()
        isLoading = True
        cbocategory.SelectedIndex = 0
        lastCategoryIndex = 0
        LoadTypeCombo()
        isLoading = False
        activeCard = ""
        LoadProducts()
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        btnclear.PerformClick()
    End Sub

End Class