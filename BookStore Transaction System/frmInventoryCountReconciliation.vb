Imports MySql.Data.MySqlClient

Public Class frmInventoryCountReconciliation
    Public Property InitialSearch As String = ""
    Private Class CountRow
        Public VariantId As Integer
        Public DetailId As Integer = 0        ' > 0 once the line is saved
        Public Adjusted As Boolean = False
    End Class

    ' A count typed on screen but not saved yet (survives paging / searching / filtering)
    Private Class PendingCount
        Public Physical As Integer = -1       ' -1 = nothing typed yet
        Public Remarks As String = ""
        Public System As Integer = 0
    End Class

    Private Function NewCountNo() As String
        Return "CNT-" & DateTime.Now.ToString("yyyyMMddHHmmss")
    End Function

    Private Const PAGE_SIZE As Integer = 25

    Private currentCountId As Long = 0
    Private currentCountNo As String = ""
    Private isLoading As Boolean = True
    Private activeCard As String = ""          ' "", counted, matched, discrepancy, short, excess
    Private pg As GridPager
    Private ReadOnly pending As New Dictionary(Of Integer, PendingCount)
    Private WithEvents tmrSearch As New System.Windows.Forms.Timer With {.Interval = 400}

    ' ==================== LOAD ====================
    Private Sub frmInventoryCountReconciliation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySearchPlaceholders(Me)

        txtGrandTotal.Text = Date.Today.ToString("MMMM d, yyyy")
        lblname.Text = If(Not String.IsNullOrEmpty(currentuser.FullName), currentuser.FullName, "N/A")
        lblposition.Text = If(Not String.IsNullOrEmpty(currentuser.Role), currentuser.Role, "N/A")

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
        cboType.DropDownStyle = ComboBoxStyle.DropDownList
        LoadTypeCombo()
        SetupCards()
        txtSearch.Text = InitialSearch
        ' extra columns
        dgvlistproducts.Columns.Insert(0, New DataGridViewTextBoxColumn With {.Name = "CountNo", .HeaderText = "Count No.", .Width = 150})
        dgvlistproducts.Columns.Insert(3, New DataGridViewTextBoxColumn With {.Name = "ProductDescription", .HeaderText = "Product Description", .Width = 200})

        currentCountNo = NewCountNo()
        txtCountNo.Text = currentCountNo

        With dgvlistproducts
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = False
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            For Each col As DataGridViewColumn In .Columns
                col.ReadOnly = Not (col.Name = "PhysicalQuantity" OrElse col.Name = "Remarks")
            Next
            ' the grid is taller than its panel: shrink it so the pager bar fits at the bottom
            .Height = Panel5.ClientSize.Height - .Top
        End With

        ' smoother drawing
        GetType(DataGridView).InvokeMember("DoubleBuffered",
            Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty,
            Nothing, dgvlistproducts, New Object() {True})

        pg = New GridPager(dgvlistproducts, PAGE_SIZE)
        AddHandler pg.PageChanged, Sub() LoadProducts()

        isLoading = False
        LoadProducts()
    End Sub

    ' ==================== CATEGORY / TYPE FILTERS ====================
    Private Sub LoadTypeCombo()
        FillTypeNameCombo(cboType,
            If(cbocategory.SelectedIndex <= 0, "", Convert.ToString(cbocategory.SelectedItem)),
            TypeSource.Products, True)
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isLoading Then Exit Sub
        isLoading = True
        LoadTypeCombo()
        isLoading = False
        pg.Reset()
        LoadProducts()
    End Sub

    Private Sub cboType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboType.SelectedIndexChanged
        If isLoading Then Exit Sub
        pg.Reset()
        LoadProducts()
    End Sub

    ' ==================== SEARCH (waits 0.4s after typing, then reloads page 1) ====================
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If isLoading Then Exit Sub
        tmrSearch.Stop()
        tmrSearch.Start()
    End Sub

    Private Sub tmrSearch_Tick(sender As Object, e As EventArgs) Handles tmrSearch.Tick
        tmrSearch.Stop()
        pg.Reset()
        LoadProducts()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            tmrSearch.Stop()
            pg.Reset()
            LoadProducts()
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        tmrSearch.Stop()
        pg.Reset()
        LoadProducts()
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        tmrSearch.Stop()
        pg.Reset()
        LoadProducts()          ' typed counts are kept, nothing is discarded
    End Sub

    ' ==================== SUMMARY CARDS (click = filter the rows on this page) ====================
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

        For Each p As Panel In New Panel() {Panel10, Panel8, Panel7, Panel9, Panel11}
            p.BorderStyle = If(Convert.ToString(p.Tag) = activeCard, BorderStyle.FixedSingle, BorderStyle.None)
        Next
    End Sub

    ' ==================== LOAD ONE PAGE OF PRODUCTS ====================
    Private Sub LoadProducts()
        If pg Is Nothing Then Exit Sub
        dgvlistproducts.EndEdit()

        Dim keyword As String = txtSearch.Text.Trim()
        Dim category As String = If(cbocategory.SelectedIndex <= 0, "", cbocategory.Text)
        Dim typeName As String = If(cboType.SelectedIndex <= 0, "", Convert.ToString(cboType.SelectedItem))

        Dim query As String =
            "SELECT v.variant_id, v.product_code, p.product_name, p.product_description, c.category_name, ct.type_name, v.size, " &
            "v.quantity_on_hand, d.inventory_count_detail_id, d.system_quantity, d.physical_quantity, " &
            "d.status AS count_status, d.remarks, d.adjusted " &
            "FROM tbl_product_variants v " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "LEFT JOIN tbl_inventory_count_details d ON d.variant_id = v.variant_id AND d.inventory_count_id = @cid " &
            "WHERE p.status = 'Active' AND (v.product_code LIKE @s OR p.product_name LIKE @s OR p.product_description LIKE @s " &
            "OR c.category_name LIKE @s OR ct.type_name LIKE @s OR v.size LIKE @s) "

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
        query &= "ORDER BY p.product_name, v.size, v.variant_id"

        Dim dt As DataTable = pg.LoadPage(query, names.ToArray(), values.ToArray())

        dgvlistproducts.SuspendLayout()
        dgvlistproducts.Rows.Clear()

        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                Dim idx As Integer = dgvlistproducts.Rows.Add()
                Dim row As DataGridViewRow = dgvlistproducts.Rows(idx)
                Dim variantId As Integer = Convert.ToInt32(r("variant_id"))
                Dim info As New CountRow With {.VariantId = variantId}

                row.Cells("ProductCode").Value = r("product_code").ToString()
                row.Cells("CountNo").Value = currentCountNo
                row.Cells("ProductName").Value = r("product_name").ToString()
                row.Cells("ProductDescription").Value = If(IsDBNull(r("product_description")), "", r("product_description").ToString())
                row.Cells("Category").Value = r("category_name").ToString()
                row.Cells("TypeofProduct").Value = r("type_name").ToString()
                row.Cells("Size").Value = r("size").ToString()

                If Not IsDBNull(r("inventory_count_detail_id")) AndAlso Convert.ToInt32(r("inventory_count_detail_id")) > 0 Then
                    ' already saved in this count: locked
                    info.DetailId = Convert.ToInt32(r("inventory_count_detail_id"))
                    info.Adjusted = Convert.ToInt32(r("adjusted")) = 1
                    row.Cells("SystemQuantity").Value = Convert.ToInt32(r("system_quantity"))
                    row.Cells("PhysicalQuantity").Value = Convert.ToInt32(r("physical_quantity"))
                    row.Cells("Remarks").Value = If(IsDBNull(r("remarks")), "", r("remarks").ToString())
                ElseIf pending.ContainsKey(variantId) Then
                    ' typed earlier on screen (maybe on another page): restore it
                    Dim pc As PendingCount = pending(variantId)
                    row.Cells("SystemQuantity").Value = pc.System
                    row.Cells("PhysicalQuantity").Value = If(pc.Physical >= 0, pc.Physical.ToString(), "")
                    row.Cells("Remarks").Value = pc.Remarks
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
        dgvlistproducts.ResumeLayout()
        UpdateCards()
        ApplyCardFilter()
    End Sub

    ' ==================== TYPING THE PHYSICAL COUNT ====================
    Private Sub dgvlistproducts_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvlistproducts.CellBeginEdit
        If e.RowIndex < 0 Then Exit Sub
        Dim info As CountRow = TryCast(dgvlistproducts.Rows(e.RowIndex).Tag, CountRow)
        If info IsNot Nothing AndAlso info.DetailId > 0 Then e.Cancel = True      ' saved lines are locked
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
        Dim colName As String = dgvlistproducts.Columns(e.ColumnIndex).Name
        If colName <> "PhysicalQuantity" AndAlso colName <> "Remarks" Then Exit Sub

        Dim row As DataGridViewRow = dgvlistproducts.Rows(e.RowIndex)
        RefreshRow(row)
        StorePending(row)
        UpdateCards()
    End Sub

    ' remember what was typed so it survives paging / searching
    Private Sub StorePending(row As DataGridViewRow)
        Dim info As CountRow = TryCast(row.Tag, CountRow)
        If info Is Nothing OrElse info.DetailId > 0 Then Exit Sub

        Dim txt As String = Convert.ToString(row.Cells("PhysicalQuantity").Value).Trim()
        Dim remarks As String = Convert.ToString(row.Cells("Remarks").Value).Trim()
        Dim phys As Integer = -1
        If txt = "" OrElse Not Integer.TryParse(txt, phys) OrElse phys < 0 Then phys = -1

        If phys < 0 AndAlso remarks = "" Then
            pending.Remove(info.VariantId)
            Exit Sub
        End If

        Dim sys As Integer = 0
        Integer.TryParse(Convert.ToString(row.Cells("SystemQuantity").Value), sys)
        pending(info.VariantId) = New PendingCount With {.Physical = phys, .Remarks = remarks, .System = sys}
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

    ' Cards cover the WHOLE count sheet: saved lines (database) + typed-but-unsaved lines (memory)
    Private Sub UpdateCards()
        Dim counted As Integer = 0, matched As Integer = 0, shortCount As Integer = 0, excess As Integer = 0

        If currentCountId > 0 Then
            Dim dt As DataTable = GetDataTable(
                "SELECT COUNT(*) AS n, IFNULL(SUM(status = 'Matched'), 0) AS m, " &
                "IFNULL(SUM(status = 'Short'), 0) AS s, IFNULL(SUM(status = 'Excess'), 0) AS x " &
                "FROM tbl_inventory_count_details WHERE inventory_count_id = @c",
                New String() {"@c"}, New Object() {CType(currentCountId, Object)})
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                counted = Convert.ToInt32(dt.Rows(0)("n"))
                matched = Convert.ToInt32(dt.Rows(0)("m"))
                shortCount = Convert.ToInt32(dt.Rows(0)("s"))
                excess = Convert.ToInt32(dt.Rows(0)("x"))
            End If
        End If

        For Each pc As PendingCount In pending.Values
            If pc.Physical < 0 Then Continue For
            counted += 1
            Dim diff As Integer = pc.Physical - pc.System
            If diff = 0 Then
                matched += 1
            ElseIf diff < 0 Then
                shortCount += 1
            Else
                excess += 1
            End If
        Next

        lbltotalproducts.Text = counted.ToString("N0")
        lbltotalqproducts.Text = matched.ToString("N0")
        lblonhand.Text = (shortCount + excess).ToString("N0")
        lblcriticallvl.Text = shortCount.ToString("N0")
        lbloutofstocks.Text = excess.ToString("N0")
    End Sub

    Private Function HasUnsavedEntries() As Boolean
        For Each pc As PendingCount In pending.Values
            If pc.Physical >= 0 Then Return True
        Next
        Return False
    End Function

    ' ==================== SAVE COUNT ====================
    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "Inventory Count")
            Exit Sub
        End If
        dgvlistproducts.EndEdit()

        ' every counted line from ALL pages
        Dim toSave As New List(Of KeyValuePair(Of Integer, PendingCount))
        For Each kv As KeyValuePair(Of Integer, PendingCount) In pending
            If kv.Value.Physical >= 0 Then toSave.Add(kv)
        Next

        If toSave.Count = 0 Then
            MsgBox("Type the physical quantity of at least one item first.", vbExclamation, "Inventory Count")
            Exit Sub
        End If

        Dim discrepancies As Integer = 0
        For Each kv As KeyValuePair(Of Integer, PendingCount) In toSave
            If kv.Value.Physical - kv.Value.System <> 0 Then discrepancies += 1
        Next

        If MsgBox("Save " & toSave.Count & " counted item(s) (" & discrepancies & " with discrepancy)?",
                  vbYesNo + vbQuestion, "Save Count") <> MsgBoxResult.Yes Then Exit Sub

        Try
            Dim countId As Long = currentCountId
            Dim countNo As String = currentCountNo

            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
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

                        For Each kv As KeyValuePair(Of Integer, PendingCount) In toSave
                            Dim sys As Integer = kv.Value.System
                            Dim phys As Integer = kv.Value.Physical
                            Dim diff As Integer = phys - sys
                            Dim remarks As String = kv.Value.Remarks

                            Using q As New MySqlCommand(
                                "INSERT INTO tbl_inventory_count_details " &
                                "(inventory_count_id, variant_id, system_quantity, physical_quantity, difference, status, remarks, adjusted) " &
                                "VALUES (@c, @v, @s, @p, @d, @st, @r, 0)", c, tx)
                                q.Parameters.AddWithValue("@c", countId)
                                q.Parameters.AddWithValue("@v", kv.Key)
                                q.Parameters.AddWithValue("@s", sys)
                                q.Parameters.AddWithValue("@p", phys)
                                q.Parameters.AddWithValue("@d", diff)
                                q.Parameters.AddWithValue("@st", StatusOf(diff))
                                q.Parameters.AddWithValue("@r", If(remarks = "", CType(DBNull.Value, Object), remarks))
                                q.ExecuteNonQuery()
                            End Using
                        Next

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

            currentCountId = countId
            currentCountNo = countNo

            For Each kv As KeyValuePair(Of Integer, PendingCount) In toSave
                pending.Remove(kv.Key)
            Next

            LogActivity("Inventory Count", countNo, "Saved " & toSave.Count & " counted item(s), " & discrepancies & " discrepancy(ies)")

            MsgBox("Count " & countNo & " saved." & vbCrLf &
                   If(discrepancies > 0, "To correct a Short/Excess item, select its row and click 'Adjust Inventory'.", "No discrepancies found."),
                   vbInformation, "Inventory Count")

            LoadProducts()      ' saved lines now show as locked

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

    ' ==================== CLEAR / CANCEL ====================
    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        If HasUnsavedEntries() Then
            If MsgBox("Discard the counts that are not saved yet and start a new count sheet?",
                      vbYesNo + vbQuestion, "Inventory Count") <> MsgBoxResult.Yes Then Exit Sub
        End If
        pending.Clear()
        currentCountId = 0
        currentCountNo = NewCountNo()
        txtCountNo.Text = currentCountNo
        activeCard = ""

        isLoading = True
        txtSearch.Clear()
        cbocategory.SelectedIndex = 0
        LoadTypeCombo()
        isLoading = False

        pg.Reset()
        LoadProducts()
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        btnclear.PerformClick()
    End Sub

End Class