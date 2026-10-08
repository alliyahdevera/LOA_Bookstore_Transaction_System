Imports System.Drawing.Printing
Imports MySql.Data.MySqlClient

Public Class frmTransactionDetails
    Public Property TransactionNo As String
    Private Class ItemInfo
        Public TransactionItemId As Integer
        Public VariantId As Integer
        Public UnitPrice As Decimal
        Public Purchased As Integer
        Public Processed As Integer
        Public PickupDate As Date?
        Public IsUniform As Boolean
        Public IsBackorder As Boolean
        Public ReadOnly Property Available As Integer
            Get
                Return Purchased - Processed
            End Get
        End Property
    End Class
    Private txnId As Integer = 0
    Private txnStatus As String = ""
    Private isBinding As Boolean = False
    Private isClamping As Boolean = False
    Private sizeTable As DataTable
    Private ReadOnly Peso As String = ChrW(8369)
    Private isLoadingGrid As Boolean = False
    Private rcOR, rcDate, rcBuyer, rcStudentNo, rcPayment, rcCashier, rcStatus As String
    Private rcTotal, rcPaid, rcChange As Decimal
    Private rY As Single
    Private Sub SetupCartGrid()
        dgvCart.ReadOnly = False
        dgvCart.AllowUserToAddRows = False
        dgvCart.AllowUserToDeleteRows = False
        dgvCart.MultiSelect = False
        dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect



        If Not dgvCart.Columns.Contains("colSelect") Then
            dgvCart.Columns.Insert(0, New DataGridViewCheckBoxColumn With {.Name = "colSelect", .HeaderText = "Select", .Width = 55})
            dgvCart.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colProcessQty", .HeaderText = "Qty to Process", .Width = 100})
            dgvCart.Columns("colProcessQty").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End If
    End Sub

    ' ===================== LOAD =====================
    Private Sub frmTransactionDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupCartGrid()
        nudQuantity.Visible = False

        ' locked fields
        txtcreatedby.Text = currentuser.FullName
        txtcreatedby.TabStop = False
        dtpORDate.Value = DateTime.Now
        dtpORDate.Enabled = False

        txtReason.MaxLength = 255

        ' Condition combo (designer control cbocondt)
        cbocondt.DropDownStyle = ComboBoxStyle.DropDownList
        cbocondt.Items.Clear()
        cbocondt.Items.AddRange(New Object() {"Good", "Fair", "Damaged"})
        cbocondt.SelectedIndex = -1
        txtProduct.DropDownStyle = ComboBoxStyle.DropDown          ' typeable (autocomplete)
        txtProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        txtProduct.AutoCompleteSource = AutoCompleteSource.ListItems
        txtsize.DropDownStyle = ComboBoxStyle.DropDownList
        txtAvailstock.DropDownStyle = ComboBoxStyle.DropDownList
        txtAvailstock.Enabled = False                                ' display only

        SetRange(nudQuantity, 0)
        SetRange(numupqty, 0)

        LoadTransaction()
        LoadReplacementProducts()
        ResetActionPanel()
    End Sub    ' all existing code keeps using cboCondition; it now points at the real designer combo
    Private ReadOnly Property cboCondition As ComboBox
        Get
            Return cbocondt
        End Get
    End Property

    ' ===================== LOAD TRANSACTION =====================
    Private Sub LoadTransaction()
        Dim h As DataTable = GetDataTable(
            "SELECT t.transaction_id, t.buyer_name, t.or_no, t.created_at, t.payment_method, " &
            "t.total_amount, t.amount_paid, t.amount_change, t.status, u.username, " &
            "s.student_no, s.grade_level, s.program_strand " &
            "FROM tbl_transactions t " &
            "INNER JOIN tbl_users u ON t.created_by = u.user_id " &
            "LEFT JOIN tbl_students s ON t.student_id = s.student_id " &
            "WHERE t.transaction_no = @t",
            New String() {"@t"}, New Object() {TransactionNo})

        If h.Rows.Count = 0 Then
            MsgBox("Transaction not found.", vbExclamation, "Transaction Details")
            Me.Close()
            Exit Sub
        End If

        Dim r As DataRow = h.Rows(0)
        txnId = Convert.ToInt32(r("transaction_id"))
        txnStatus = Convert.ToString(r("status"))

        txtorno.Text = Convert.ToString(r("or_no"))
        btnStudentno.Text = Convert.ToString(r("student_no"))
        txtStudentName.Text = Convert.ToString(r("buyer_name"))
        txtgrade.Text = Convert.ToString(r("grade_level"))
        txtProgramStrand.Text = Convert.ToString(r("program_strand"))
        txttdate.Text = Convert.ToDateTime(r("created_at")).ToString("MMMM d, yyyy h:mm tt")
        txtpaymentmethod.Text = Convert.ToString(r("payment_method"))
        txtstatus.Text = txnStatus
        txtareceived.Text = Peso & Convert.ToDecimal(r("amount_paid")).ToString("N2")
        txtachange.Text = Peso & Convert.ToDecimal(r("amount_change")).ToString("N2")

        ' receipt data
        rcOR = txtorno.Text
        rcDate = txttdate.Text
        rcBuyer = txtStudentName.Text
        rcStudentNo = btnStudentno.Text
        rcPayment = txtpaymentmethod.Text
        rcCashier = Convert.ToString(r("username"))
        rcStatus = txnStatus
        rcTotal = Convert.ToDecimal(r("total_amount"))
        rcPaid = Convert.ToDecimal(r("amount_paid"))
        rcChange = Convert.ToDecimal(r("amount_change"))

        ' items + how many of each were already returned/exchanged
        Dim items As DataTable = GetDataTable(
            "SELECT ti.transaction_item_id, ti.variant_id, p.product_name, c.category_name, v.size, " &
            "ti.quantity, ti.subtotal, ti.pickup_date, ti.is_backorder, " &
            "IFNULL((SELECT SUM(rei.quantity) FROM tbl_return_exchange_items rei " &
            "        INNER JOIN tbl_returns_exchanges re ON rei.return_exchange_id = re.return_exchange_id " &
            "        WHERE rei.transaction_item_id = ti.transaction_item_id AND re.status = 'Completed'), 0) AS processed_qty " &
            "FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "WHERE ti.transaction_id = @id",
            New String() {"@id"}, New Object() {txnId})

        isLoadingGrid = True
        dgvCart.Rows.Clear()
        Dim totalQty As Integer = 0
        Dim anyEligible As Boolean = False

        For Each it As DataRow In items.Rows
            Dim qty As Integer = Convert.ToInt32(it("quantity"))
            Dim lineTotal As Decimal = Convert.ToDecimal(it("subtotal"))
            Dim info As New ItemInfo With {
            .TransactionItemId = Convert.ToInt32(it("transaction_item_id")),
            .VariantId = Convert.ToInt32(it("variant_id")),
            .UnitPrice = If(qty > 0, lineTotal / qty, 0D),
            .Purchased = qty,
            .Processed = Convert.ToInt32(it("processed_qty")),
            .PickupDate = If(IsDBNull(it("pickup_date")), Nothing, CType(Convert.ToDateTime(it("pickup_date")), Date?)),
            .IsBackorder = (Convert.ToInt32(it("is_backorder")) = 1),
            .IsUniform = String.Equals(Convert.ToString(it("category_name")), "Uniforms", StringComparison.OrdinalIgnoreCase)
        }

            Dim idx As Integer = dgvCart.Rows.Add(False,
            Convert.ToString(it("product_name")), Convert.ToString(it("category_name")),
            Convert.ToString(it("size")), qty,
            info.UnitPrice.ToString("N2"), lineTotal.ToString("N2"), "")
            Dim row As DataGridViewRow = dgvCart.Rows(idx)
            row.Tag = info
            row.Cells("colProcessQty").ReadOnly = True

            If info.IsUniform AndAlso info.Available > 0 Then
                anyEligible = True
            Else
                row.Cells("colSelect").ReadOnly = True
                row.DefaultCellStyle.ForeColor = Color.Gray
                row.Cells("colSelect").ToolTipText = If(info.IsUniform, "Already fully returned/exchanged.", "Only uniforms can be returned or exchanged.")
            End If
            totalQty += qty
        Next
        isLoadingGrid = False

        lbltotitem.Text = items.Rows.Count.ToString()
        lbltotquantity.Text = totalQty.ToString()

        Dim canProcess As Boolean = (txnStatus <> "Cancelled") AndAlso anyEligible
        btnreturnexc.Enabled = canProcess
        rbtnReturn.Enabled = canProcess
        rbtnexchange.Enabled = canProcess

        dgvCart.ClearSelection()
        UpdateSelectedItem()
    End Sub


    Private Sub dgvCart_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCart.SelectionChanged
        UpdateSelectedItem()
    End Sub
    Private Sub UpdateSelectedItem()
        If Not btnreturnexc.Enabled Then
            lblquantity.Text = "Only uniforms with items left can be returned/exchanged"
            Exit Sub
        End If
        Dim n As Integer = CheckedRows().Count
        lblquantity.Text = If(n = 0, "Tick the uniform item(s) to process", n & " item(s) ticked - set quantities in the grid")
    End Sub
    Private Sub dgvCart_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvCart.CurrentCellDirtyStateChanged
        If dgvCart.IsCurrentCellDirty AndAlso TypeOf dgvCart.CurrentCell Is DataGridViewCheckBoxCell Then
            dgvCart.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub dgvCart_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellValueChanged
        If isLoadingGrid OrElse e.RowIndex < 0 Then Exit Sub
        If dgvCart.Columns(e.ColumnIndex).Name <> "colSelect" Then Exit Sub

        Dim row As DataGridViewRow = dgvCart.Rows(e.RowIndex)
        Dim it As ItemInfo = TryCast(row.Tag, ItemInfo)
        If it Is Nothing Then Exit Sub

        Dim ticked As Boolean = Convert.ToBoolean(If(row.Cells("colSelect").Value, False))
        If ticked AndAlso (Not it.IsUniform OrElse it.Available <= 0) Then
            row.Cells("colSelect").Value = False
            Exit Sub
        End If

        row.Cells("colProcessQty").ReadOnly = Not ticked
        row.Cells("colProcessQty").Value = If(ticked, it.Available.ToString(), "")
        UpdateSelectedItem()

        If ticked AndAlso rbtnexchange.Checked AndAlso CheckedRows().Count > 1 Then
            MsgBox("An exchange can only be done for one item at a time. Untick the other items, or choose Return.", vbInformation, "Return / Exchange")
        End If
    End Sub

    Private Sub dgvCart_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvCart.EditingControlShowing
        Dim tb As TextBox = TryCast(e.Control, TextBox)
        If tb Is Nothing Then Exit Sub
        RemoveHandler tb.KeyPress, AddressOf QtyCell_KeyPress
        If dgvCart.CurrentCell IsNot Nothing AndAlso dgvCart.Columns(dgvCart.CurrentCell.ColumnIndex).Name = "colProcessQty" Then
            AddHandler tb.KeyPress, AddressOf QtyCell_KeyPress
        End If
    End Sub

    Private Sub QtyCell_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub dgvCart_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellEndEdit
        If e.RowIndex < 0 OrElse dgvCart.Columns(e.ColumnIndex).Name <> "colProcessQty" Then Exit Sub
        Dim row As DataGridViewRow = dgvCart.Rows(e.RowIndex)
        Dim it As ItemInfo = TryCast(row.Tag, ItemInfo)
        If it Is Nothing Then Exit Sub

        Dim q As Integer
        If Not Integer.TryParse(Convert.ToString(row.Cells("colProcessQty").Value), q) OrElse q < 1 Then
            row.Cells("colProcessQty").Value = "1"
            MsgBox("Quantity must be at least 1.", vbExclamation, "Quantity")
        ElseIf q > it.Available Then
            row.Cells("colProcessQty").Value = it.Available.ToString()
            If it.Processed = 0 Then
                MsgBox("Quantity cannot exceed the purchased count (" & it.Purchased & ").", vbExclamation, "Quantity Exceeded")
            Else
                MsgBox("Only " & it.Available & " left to process (purchased " & it.Purchased & ", already returned/exchanged " & it.Processed & ").", vbExclamation, "Quantity Exceeded")
            End If
        End If
    End Sub
    Private Function CheckedRows() As List(Of DataGridViewRow)
        Dim list As New List(Of DataGridViewRow)
        For Each r As DataGridViewRow In dgvCart.Rows
            If Convert.ToBoolean(If(r.Cells("colSelect").Value, False)) Then list.Add(r)
        Next
        Return list
    End Function

    ' sets Min=1 / Max=maxValue (or 0/0 if none) and resets value to Min
    Private Sub SetRange(nud As NumericUpDown, maxValue As Integer)
        nud.Minimum = 0
        nud.Maximum = maxValue
        nud.Minimum = If(maxValue > 0, 1, 0)
        nud.Value = nud.Minimum
    End Sub

    ' ===================== ACTION TYPE =====================
    Private Sub rbtnAction_CheckedChanged(sender As Object, e As EventArgs) Handles rbtnReturn.CheckedChanged, rbtnexchange.CheckedChanged
        UpdateActionState()
    End Sub

    Private Sub UpdateActionState()
        Dim hasAction As Boolean = rbtnReturn.Checked OrElse rbtnexchange.Checked
        txtReason.ReadOnly = Not hasAction
        cboCondition.Enabled = hasAction
        If Not hasAction Then cboCondition.SelectedIndex = -1

        Dim isExchange As Boolean = rbtnexchange.Checked
        txtProduct.Enabled = isExchange
        txtsize.Enabled = isExchange
        numupqty.Enabled = isExchange
        If Not isExchange Then ClearReplacement()
    End Sub

    Private Sub ResetActionPanel()
        rbtnReturn.Checked = False
        rbtnexchange.Checked = False
        txtReason.Clear()
        cboCondition.SelectedIndex = -1
        UpdateActionState()
    End Sub

    ' ===================== REPLACEMENT ITEM =====================
    Private Sub LoadReplacementProducts()
        Dim dt As DataTable = GetDataTable(
    "SELECT p.product_id, p.product_name FROM tbl_products p " &
    "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
    "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
    "WHERE p.status = 'Active' AND c.category_name = 'Uniforms' " &
    "AND EXISTS (SELECT 1 FROM tbl_product_variants v WHERE v.product_id = p.product_id AND v.quantity_on_hand > 0) " &
    "ORDER BY p.product_name")
        isBinding = True
        FillCombo(txtProduct, dt, "product_name", "product_id")
        txtProduct.SelectedIndex = -1
        isBinding = False
    End Sub

    Private Sub ClearReplacement()
        isBinding = True
        txtProduct.SelectedIndex = -1
        txtProduct.Text = ""
        txtsize.DataSource = Nothing
        txtsize.Items.Clear()
        txtsize.Text = ""
        txtAvailstock.Items.Clear()
        txtAvailstock.Text = ""
        isBinding = False
        SetRange(numupqty, 0)
    End Sub

    Private Sub txtProduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtProduct.SelectedIndexChanged
        If isBinding Then Exit Sub

        isBinding = True
        txtsize.DataSource = Nothing
        txtsize.Items.Clear()
        txtsize.Text = ""
        txtAvailstock.Items.Clear()
        txtAvailstock.Text = ""
        isBinding = False
        SetRange(numupqty, 0)

        If txtProduct.SelectedIndex < 0 Then Exit Sub

        Dim pid As Integer = Convert.ToInt32(CType(txtProduct.SelectedItem, DataRowView)("product_id"))
        sizeTable = GetDataTable(
            "SELECT variant_id, size, quantity_on_hand FROM tbl_product_variants " &
            "WHERE product_id = @p AND quantity_on_hand > 0 " &
            "ORDER BY FIELD(size,'XS','S','M','L','XL','2XL','3XL','4XL','5XL','6XL'), size",
            New String() {"@p"}, New Object() {pid})

        isBinding = True
        FillCombo(txtsize, sizeTable, "size", "variant_id")
        txtsize.SelectedIndex = -1
        isBinding = False

        ' non-uniform items have a single "N/A" variant -> auto-pick and lock
        If sizeTable.Rows.Count = 1 AndAlso sizeTable.Rows(0)("size").ToString() = "N/A" Then
            txtsize.SelectedIndex = 0
            txtsize.Enabled = False
        Else
            txtsize.Enabled = True
        End If
    End Sub

    Private Sub txtsize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtsize.SelectedIndexChanged
        If isBinding Then Exit Sub
        txtAvailstock.Items.Clear()
        If txtsize.SelectedIndex < 0 Then
            SetRange(numupqty, 0)
            Exit Sub
        End If

        Dim stock As Integer = Convert.ToInt32(CType(txtsize.SelectedItem, DataRowView)("quantity_on_hand"))
        txtAvailstock.Items.Add(stock.ToString())
        txtAvailstock.SelectedIndex = 0

        SetRange(numupqty, stock)
        numupqty.Value = Math.Max(numupqty.Minimum, Math.Min(nudQuantity.Value, CDec(stock)))
    End Sub
    Private Sub btnreturnexc_Click(sender As Object, e As EventArgs) Handles btnreturnexc.Click
        Dim ticked As List(Of DataGridViewRow) = CheckedRows()
        If ticked.Count = 0 Then
            MsgBox("Tick at least one uniform item to process.", vbExclamation, "Return / Exchange") : Exit Sub
        End If
        If Not rbtnReturn.Checked AndAlso Not rbtnexchange.Checked Then
            MsgBox("Choose Return or Exchange.", vbExclamation, "Return / Exchange") : Exit Sub
        End If

        Dim isExchange As Boolean = rbtnexchange.Checked
        If isExchange AndAlso ticked.Count > 1 Then
            MsgBox("An exchange can only be done for one item at a time. Untick the other items, or choose Return.", vbExclamation, "Return / Exchange") : Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtReason.Text) Then
            MsgBox("Please enter the reason.", vbExclamation, "Return / Exchange") : txtReason.Focus() : Exit Sub
        End If
        If cboCondition.SelectedIndex < 0 Then
            MsgBox("Please select the item condition.", vbExclamation, "Return / Exchange") : cboCondition.Focus() : Exit Sub
        End If

        Dim lines As New List(Of KeyValuePair(Of ItemInfo, Integer))
        Dim itemTexts As New List(Of String)
        Dim returnValue As Decimal = 0D
        Dim totalReturnQty As Integer = 0

        For Each r As DataGridViewRow In ticked
            Dim it As ItemInfo = DirectCast(r.Tag, ItemInfo)
            Dim nm As String = Convert.ToString(r.Cells("ProductName").Value)
            Dim sz As String = Convert.ToString(r.Cells("Size").Value)
            Dim q As Integer

            If Not it.IsUniform Then
                MsgBox("Only uniforms can be returned or exchanged (" & nm & ").", vbExclamation, "Return / Exchange") : Exit Sub
            End If
            If Not Integer.TryParse(Convert.ToString(r.Cells("colProcessQty").Value), q) OrElse q < 1 OrElse q > it.Available Then
                MsgBox("Check the quantity for " & nm & " (maximum " & it.Available & ").", vbExclamation, "Return / Exchange") : Exit Sub
            End If

            lines.Add(New KeyValuePair(Of ItemInfo, Integer)(it, q))
            itemTexts.Add(nm & If(sz <> "" AndAlso sz <> "N/A", " (" & sz & ")", "") & " x" & q)
            returnValue += it.UnitPrice * q
            totalReturnQty += q
        Next

        Dim repVariantId As Integer = 0, repQty As Integer = 0
        Dim repPrice As Decimal = 0D, repName As String = ""

        If isExchange Then
            If txtProduct.SelectedIndex < 0 Then
                MsgBox("Select the replacement product.", vbExclamation, "Return / Exchange") : Exit Sub
            End If
            If txtsize.SelectedIndex < 0 Then
                MsgBox("Select the replacement size.", vbExclamation, "Return / Exchange") : Exit Sub
            End If
            repVariantId = Convert.ToInt32(CType(txtsize.SelectedItem, DataRowView)("variant_id"))
            repQty = CInt(numupqty.Value)
            If repQty < 1 Then
                MsgBox("Enter the replacement quantity.", vbExclamation, "Return / Exchange") : Exit Sub
            End If
            repPrice = Convert.ToDecimal(ExecScalar(
            "SELECT p.unit_price FROM tbl_product_variants v INNER JOIN tbl_products p ON v.product_id = p.product_id WHERE v.variant_id = @v",
            New String() {"@v"}, New Object() {repVariantId}))
            repName = txtProduct.Text & If(txtsize.Text <> "N/A", " (" & txtsize.Text & ")", "")
        End If

        Dim actionName As String = If(isExchange, "Exchange", "Return")

        Dim summary As String
        If isExchange Then
            Dim diff As Decimal = (repPrice * repQty) - returnValue
            summary = "Exchange " & String.Join(", ", itemTexts) & " for " & repQty & " x " & repName & "." & vbCrLf &
                  If(diff > 0, "Customer pays the difference: " & Peso & diff.ToString("N2"),
                  If(diff < 0, "Refund the difference: " & Peso & Math.Abs(diff).ToString("N2"), "No price difference."))
        Else
            summary = "Return " & String.Join(", ", itemTexts) & "." & vbCrLf & "Refund to customer: " & Peso & returnValue.ToString("N2")
        End If
        If MsgBox(summary & vbCrLf & vbCrLf & "Proceed?", vbYesNo + vbQuestion, "Confirm") <> MsgBoxResult.Yes Then Exit Sub

        Dim approver As String = RequireSupervisorApproval(Me, actionName & " of " & TransactionNo & ": " & String.Join(", ", itemTexts))
        If approver Is Nothing Then Exit Sub

        Dim refNo As String = If(isExchange, "EXC-", "RET-") & DateTime.Now.ToString("yyyyMMddHHmmssfff")
        Dim resellable As Boolean = Not String.Equals(cboCondition.Text, "Damaged", StringComparison.OrdinalIgnoreCase)   ' Good / Fair go back to stock
        Dim sellable As Boolean = (cboCondition.SelectedIndex <= 1)   ' Good / Fair go back to stock

        Try
            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        Dim reId As Long
                        Using q As New MySqlCommand(
                        "INSERT INTO tbl_returns_exchanges (reference_no, transaction_id, action_type, reason, processed_by, processed_at, status) " &
                        "VALUES (@ref, @tid, @act, @rs, @uid, NOW(), 'Completed')", c, tx)
                            q.Parameters.AddWithValue("@ref", refNo)
                            q.Parameters.AddWithValue("@tid", txnId)
                            q.Parameters.AddWithValue("@act", actionName)
                            q.Parameters.AddWithValue("@rs", txtReason.Text.Trim())
                            q.Parameters.AddWithValue("@uid", currentuser.UserID)
                            q.ExecuteNonQuery()
                            reId = q.LastInsertedId
                        End Using

                        For Each ln As KeyValuePair(Of ItemInfo, Integer) In lines
                            Dim it As ItemInfo = ln.Key
                            Dim qty As Integer = ln.Value

                            Using q As New MySqlCommand(
                            "INSERT INTO tbl_return_exchange_items (return_exchange_id, transaction_item_id, quantity, item_condition, replacement_variant_id, replacement_quantity) " &
                            "VALUES (@rid, @tii, @q, @cond, @rv, @rq)", c, tx)
                                q.Parameters.AddWithValue("@rid", reId)
                                q.Parameters.AddWithValue("@tii", it.TransactionItemId)
                                q.Parameters.AddWithValue("@q", qty)
                                q.Parameters.AddWithValue("@cond", cboCondition.Text.Trim())
                                q.Parameters.AddWithValue("@rv", If(isExchange, CType(repVariantId, Object), DBNull.Value))
                                q.Parameters.AddWithValue("@rq", If(isExchange, CType(repQty, Object), DBNull.Value))
                                q.ExecuteNonQuery()
                            End Using

                            ' pick-up items never left the shelf, so there is nothing to put back
                            If Not it.IsBackorder Then
                                If resellable Then
                                    MoveStock(c, tx, it.VariantId, qty, "Returned", refNo, actionName & " of " & TransactionNo)
                                Else
                                    Using q As New MySqlCommand(
                                    "INSERT INTO tbl_stock_movements (variant_id, movement_type, quantity, previous_quantity, new_quantity, reference_no, remarks, created_by, created_at) " &
                                    "SELECT @v, 'Damaged', @q, quantity_on_hand, quantity_on_hand, @ref, @rm, @uid, NOW() " &
                                    "FROM tbl_product_variants WHERE variant_id = @v", c, tx)
                                        q.Parameters.AddWithValue("@v", it.VariantId)
                                        q.Parameters.AddWithValue("@q", qty)
                                        q.Parameters.AddWithValue("@ref", refNo)
                                        q.Parameters.AddWithValue("@rm", actionName & " of " & TransactionNo & " (" & cboCondition.Text & ")")
                                        q.Parameters.AddWithValue("@uid", currentuser.UserID)
                                        q.ExecuteNonQuery()
                                    End Using
                                    Using q As New MySqlCommand(
"INSERT INTO tbl_nonsaleable_stocks (variant_id, quantity, stock_condition, reason, reference_no, reported_by) " &
"VALUES (@v, @q, 'Damaged', @rm, @ref, @uid)", c, tx)
                                        q.Parameters.AddWithValue("@v", it.VariantId)
                                        q.Parameters.AddWithValue("@q", qty)
                                        q.Parameters.AddWithValue("@rm", actionName & " of " & TransactionNo & " (" & cboCondition.Text & ")")
                                        q.Parameters.AddWithValue("@ref", refNo)
                                        q.Parameters.AddWithValue("@uid", currentuser.UserID)
                                        q.ExecuteNonQuery()
                                    End Using
                                End If
                            End If
                        Next

                        If isExchange Then
                            MoveStock(c, tx, repVariantId, -repQty, "Stock Out", refNo, "Exchange replacement for " & TransactionNo)
                        End If

                        Dim totalQty As Integer = ScalarInt(c, tx, "SELECT IFNULL(SUM(quantity),0) FROM tbl_transaction_items WHERE transaction_id = @t", txnId)
                        Dim doneQty As Integer = ScalarInt(c, tx,
                        "SELECT IFNULL(SUM(rei.quantity),0) FROM tbl_return_exchange_items rei " &
                        "INNER JOIN tbl_returns_exchanges re ON rei.return_exchange_id = re.return_exchange_id " &
                        "WHERE re.transaction_id = @t AND re.status = 'Completed'", txnId)
                        Dim fully As Boolean = doneQty >= totalQty
                        Dim newStatus As String = If(isExchange,
                        If(fully, "Exchanged", "Partially Exchanged"),
                        If(fully, "Returned", "Partially Returned"))

                        Using q As New MySqlCommand("UPDATE tbl_transactions SET status = @s WHERE transaction_id = @t", c, tx)
                            q.Parameters.AddWithValue("@s", newStatus)
                            q.Parameters.AddWithValue("@t", txnId)
                            q.ExecuteNonQuery()
                        End Using

                        tx.Commit()
                    Catch
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            LogActivity("Item " & actionName, refNo,
                    actionName & " of " & String.Join(", ", itemTexts) & " from " & TransactionNo &
                    ". Reason: " & txtReason.Text.Trim() & ". Approved by " & approver)

            MsgBox(actionName & " processed successfully." & vbCrLf & "Reference No: " & refNo, vbInformation, "Return / Exchange")

            LoadTransaction()
            ResetActionPanel()

        Catch ex As Exception
            MsgBox("Return/Exchange failed and was rolled back: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub
    Private Sub txtProduct_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtProduct.Validating
        If isBinding OrElse Not txtProduct.Enabled Then Exit Sub
        Dim typed As String = txtProduct.Text.Trim()
        If typed = "" Then Exit Sub

        If txtProduct.SelectedIndex >= 0 AndAlso String.Equals(typed,
        CType(txtProduct.SelectedItem, DataRowView)("product_name").ToString(), StringComparison.OrdinalIgnoreCase) Then Exit Sub

        Dim exact As Integer = txtProduct.FindStringExact(typed)
        If exact >= 0 Then
            txtProduct.SelectedIndex = exact
            Exit Sub
        End If

        Dim hit As Integer = -1, matches As Integer = 0
        For i As Integer = 0 To txtProduct.Items.Count - 1
            Dim nm As String = CType(txtProduct.Items(i), DataRowView)("product_name").ToString()
            If nm.IndexOf(typed, StringComparison.OrdinalIgnoreCase) >= 0 Then
                matches += 1
                hit = i
            End If
        Next

        If matches = 1 Then
            txtProduct.SelectedIndex = hit
        ElseIf matches = 0 Then
            MsgBox("No product with available stock matches '" & typed & "'.", vbExclamation, "Replacement Product")
            isBinding = True
            txtProduct.SelectedIndex = -1
            txtProduct.Text = ""
            isBinding = False
        Else
            MsgBox(matches & " products match '" & typed & "'. Please pick one from the list.", vbInformation, "Replacement Product")
            txtProduct.DroppedDown = True
        End If
    End Sub

    ' ---- transaction helpers (use the SAME connection + transaction) ----
    Private Function ScalarInt(c As MySqlConnection, tx As MySqlTransaction, sql As String, id As Integer) As Integer
        Using q As New MySqlCommand(sql, c, tx)
            q.Parameters.AddWithValue("@t", id)
            Dim o As Object = q.ExecuteScalar()
            Return If(o Is Nothing OrElse IsDBNull(o), 0, Convert.ToInt32(o))
        End Using
    End Function


    Private Sub MoveStock(c As MySqlConnection, tx As MySqlTransaction, variantId As Integer, delta As Integer,
                          movementType As String, refNo As String, remarks As String)
        Dim prev As Integer
        Using q As New MySqlCommand("SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @v FOR UPDATE", c, tx)
            q.Parameters.AddWithValue("@v", variantId)
            prev = Convert.ToInt32(q.ExecuteScalar())
        End Using

        Dim nw As Integer = prev + delta
        If nw < 0 Then Throw New Exception("Not enough stock for the replacement item (only " & prev & " left).")

        Using q As New MySqlCommand("UPDATE tbl_product_variants SET quantity_on_hand = @n WHERE variant_id = @v", c, tx)
            q.Parameters.AddWithValue("@n", nw)
            q.Parameters.AddWithValue("@v", variantId)
            q.ExecuteNonQuery()
        End Using

        Using q As New MySqlCommand(
            "INSERT INTO tbl_stock_movements (variant_id, movement_type, quantity, previous_quantity, new_quantity, reference_no, remarks, created_by, created_at) " &
            "VALUES (@v, @mt, @q, @p, @n, @ref, @rm, @uid, NOW())", c, tx)
            q.Parameters.AddWithValue("@v", variantId)
            q.Parameters.AddWithValue("@mt", movementType)
            q.Parameters.AddWithValue("@q", Math.Abs(delta))
            q.Parameters.AddWithValue("@p", prev)
            q.Parameters.AddWithValue("@n", nw)
            q.Parameters.AddWithValue("@ref", refNo)
            q.Parameters.AddWithValue("@rm", remarks)
            q.Parameters.AddWithValue("@uid", currentuser.UserID)
            q.ExecuteNonQuery()
        End Using
    End Sub

    ' ===================== PRINT RECEIPT =====================
    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        If txnId = 0 Then Exit Sub
        Dim pd As New PrintDocument()
        pd.DefaultPageSettings.PaperSize = New PaperSize("Receipt", 315, 420 + dgvCart.Rows.Count * 45)   ' ~80mm wide
        pd.DefaultPageSettings.Margins = New Margins(10, 10, 10, 10)
        AddHandler pd.PrintPage, AddressOf PrintReceiptPage

        Using dlg As New PrintPreviewDialog()
            dlg.Document = pd
            dlg.WindowState = FormWindowState.Maximized
            dlg.ShowDialog(Me)          ' has a Print button; use pd.Print() instead to skip the preview
        End Using
    End Sub

    Private Sub PrintReceiptPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim w As Single = e.PageBounds.Width - 20

        Using fReg As New Font("Consolas", 8), fBold As New Font("Consolas", 9, FontStyle.Bold), fTitle As New Font("Consolas", 12, FontStyle.Bold)
            rY = 10
            RcCenter(g, "LOA BOOKSTORE", fTitle, w)
            RcCenter(g, "OFFICIAL RECEIPT", fBold, w)
            RcLine(g, w)
            RcLR(g, "OR No:", rcOR, fReg, w)
            RcLR(g, "Txn No:", TransactionNo, fReg, w)
            RcLR(g, "Date:", rcDate, fReg, w)
            RcLR(g, "Student No:", rcStudentNo, fReg, w)
            RcLR(g, "Name:", rcBuyer, fReg, w)
            RcLR(g, "Grade:", txtgrade.Text, fReg, w)
            RcLR(g, "Program:", txtProgramStrand.Text, fReg, w)
            RcLR(g, "Payment:", rcPayment, fReg, w)
            RcLine(g, w)

            For Each row As DataGridViewRow In dgvCart.Rows
                Dim nm As String = Convert.ToString(row.Cells("ProductName").Value)
                Dim sz As String = Convert.ToString(row.Cells("Size").Value)
                If sz <> "" AndAlso sz <> "N/A" Then nm &= " (" & sz & ")"
                RcText(g, nm, fReg, w)
                RcLR(g, "  " & row.Cells("Quantity").Value & " x " & row.Cells("UnitPrice").Value, Convert.ToString(row.Cells("Subtotal").Value), fReg, w)
                Dim inf As ItemInfo = TryCast(row.Tag, ItemInfo)
                If inf IsNot Nothing AndAlso inf.PickupDate.HasValue Then
                    RcText(g, "   NO STOCK - CLAIM ON " & inf.PickupDate.Value.ToString("MMM d, yyyy"), fReg, w)
                End If
            Next

            RcLine(g, w)
            RcLR(g, "TOTAL", "PHP " & rcTotal.ToString("N2"), fBold, w)
            RcLR(g, "Amount Paid", "PHP " & rcPaid.ToString("N2"), fReg, w)
            RcLR(g, "Change", "PHP " & rcChange.ToString("N2"), fReg, w)
            RcLine(g, w)
            RcLR(g, "Cashier:", rcCashier, fReg, w)
            RcLR(g, "Status:", rcStatus, fReg, w)
            rY += 8
            For Each r As DataGridViewRow In dgvCart.Rows
                Dim inf2 As ItemInfo = TryCast(r.Tag, ItemInfo)
                If inf2 IsNot Nothing AndAlso inf2.PickupDate.HasValue Then
                    RcLine(g, w)
                    RcCenter(g, "Please present this receipt", fReg, w)
                    RcCenter(g, "when claiming your pick-up item(s).", fReg, w)
                    Exit For
                End If
            Next
            RcCenter(g, "Thank you!", fReg, w)
        End Using
        e.HasMorePages = False
    End Sub

    Private Sub RcCenter(g As Graphics, text As String, f As Font, w As Single)
        Dim sf As New StringFormat With {.Alignment = StringAlignment.Center}
        g.DrawString(text, f, Brushes.Black, New RectangleF(10, rY, w, f.GetHeight(g)), sf)
        rY += f.GetHeight(g)
    End Sub

    Private Sub RcText(g As Graphics, text As String, f As Font, w As Single)
        Dim sf As New StringFormat With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
        g.DrawString(text, f, Brushes.Black, New RectangleF(10, rY, w, f.GetHeight(g)), sf)
        rY += f.GetHeight(g)
    End Sub

    Private Sub RcLR(g As Graphics, l As String, r As String, f As Font, w As Single)
        Dim h As Single = f.GetHeight(g)
        g.DrawString(l, f, Brushes.Black, New RectangleF(10, rY, w, h))
        Dim sf As New StringFormat With {.Alignment = StringAlignment.Far}
        g.DrawString(r, f, Brushes.Black, New RectangleF(10, rY, w, h), sf)
        rY += h
    End Sub

    Private Sub RcLine(g As Graphics, w As Single)
        g.DrawLine(Pens.Black, 10, rY + 2, 10 + w, rY + 2)
        rY += 6
    End Sub

    ' ===================== CLOSE =====================
    Private Sub btnclose_Click(sender As Object, e As EventArgs) Handles btnclose.Click
        Me.Close()
    End Sub
    Private Sub Qty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles nudQuantity.KeyPress, numupqty.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub
    Private Sub numupqty_TextChanged(sender As Object, e As EventArgs) Handles numupqty.TextChanged
        ClampQty(numupqty, "Replacement quantity cannot exceed the available stock (" & numupqty.Maximum & ").")
    End Sub

    Private Sub ClampQty(nud As NumericUpDown, message As String)
        If isClamping OrElse String.IsNullOrWhiteSpace(nud.Text) Then Exit Sub

        Dim typed As Decimal
        ' Check if the typed text exceeds the allowed maximum
        If Decimal.TryParse(nud.Text, typed) AndAlso typed > nud.Maximum Then
            isClamping = True

            ' 1. Clamp the value to Maximum
            nud.Value = nud.Maximum

            ' 2. Highlight text and set cursor position safely
            nud.Select(0, nud.Text.Length)

            ' 3. Reset flag BEFORE showing the message box so events resume normally
            isClamping = False

            ' 4. Prompt user
            MessageBox.Show(message, "Quantity Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
End Class