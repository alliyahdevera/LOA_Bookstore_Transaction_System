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

    ' Printing components
    Private WithEvents printDoc As New PrintDocument()
    Private printPreview As New PrintPreviewDialog()

    Private Function DisplayOrNA(value As Object) As String
        If value Is Nothing OrElse Convert.IsDBNull(value) Then
            Return "N/A"
        End If
        Dim text As String = Convert.ToString(value).Trim()
        Return If(text = "", "N/A", text)
    End Function
    Private Sub btnFinanceApproval_Click(
    sender As Object,
    e As EventArgs
) Handles btnFinanceApproval.Click

        If txnId <= 0 Then Exit Sub

        Dim reference As String = InputBox(
        "Enter the reference from the returned finance approval form." &
        vbCrLf &
        "Proceed only after checking the approved form.",
        "Record Finance Approval").Trim()

        If reference = "" Then Exit Sub

        If MsgBox(
        "Record finance approval for " & TransactionNo & "?" &
        vbCrLf &
        "The deduction will be marked paid through salary deduction." &
        vbCrLf &
        "No cash collection will be recorded.",
        vbYesNo + vbQuestion + vbDefaultButton2,
        "Confirm Finance Approval"
    ) <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Try
            SalaryDeductions.RecordFinanceApproval(txnId, reference)
        Catch ex As Exception
            MsgBox(
            "Finance approval was not recorded: " & ex.Message,
            vbCritical,
            "Finance Approval")
            Exit Sub
        End Try

        MsgBox(
        "Finance approval recorded. The transaction is now Completed.",
        vbInformation,
        "Finance Approval")

        LoadTransaction()
    End Sub
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

    Private Sub frmTransactionDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupCartGrid()

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
        txtAvailstock.Enabled = False                              ' display only

        SetRange(numupqty, 0)

        LoadTransaction()
        LoadReplacementProducts()
        ResetActionPanel()
    End Sub

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
            "s.student_no, s.grade_level, s.program_strand, t.buyer_type, t.id_number, e.department AS emp_dept " &
            "FROM tbl_transactions t " &
            "INNER JOIN tbl_users u ON t.created_by = u.user_id " &
            "LEFT JOIN tbl_students s ON t.student_id = s.student_id " &
            "LEFT JOIN tbl_employees e ON t.buyer_type = 'Employee' AND e.employee_no = t.id_number " &
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

        txtorno.Text = DisplayOrNA(r("or_no"))

        Dim buyerType As String = Convert.ToString(r("buyer_type")).Trim()
        Dim idNumber As String = Convert.ToString(r("id_number")).Trim()

        If idNumber = "" AndAlso String.Equals(buyerType, "Student", StringComparison.OrdinalIgnoreCase) Then
            idNumber = Convert.ToString(r("student_no")).Trim()
        End If

        btnStudentno.Text = DisplayOrNA(idNumber)
        txtStudentName.Text = DisplayOrNA(r("buyer_name"))
        txtgrade.Text = DisplayOrNA(buyerType)

        Select Case buyerType.ToUpperInvariant()
            Case "STUDENT"
                txtProgramStrand.Text = DisplayOrNA(DepartmentOf(Convert.ToString(r("program_strand")), Convert.ToString(r("grade_level"))))
            Case "EMPLOYEE"
                txtProgramStrand.Text = DisplayOrNA(r("emp_dept"))
            Case Else
                txtProgramStrand.Text = "N/A"
        End Select

        txttdate.Text = Convert.ToDateTime(r("created_at")).ToString("MMMM d, yyyy h:mm tt")
        txtpaymentmethod.Text = Convert.ToString(r("payment_method"))
        txtstatus.Text = txnStatus
        txtareceived.Text = Peso & Convert.ToDecimal(r("amount_paid")).ToString("N2")
        txtachange.Text = Peso & Convert.ToDecimal(r("amount_change")).ToString("N2")

        ' receipt data assignments
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

        Dim canProcess As Boolean =
    txnStatus <> "Cancelled" AndAlso
    txnStatus <> SalaryDeductions.PENDING_STATUS AndAlso
    anyEligible
        btnreturnexc.Enabled = canProcess
        rbtnReturn.Enabled = canProcess
        rbtnexchange.Enabled = canProcess

        dgvCart.ClearSelection()
        UpdateSelectedItem()
        Dim supervisor As Boolean =
    currentuser.Role = RolePermissions.ROLE_SUPERVISOR OrElse
    currentuser.Role = "Admin"

        btnFinanceApproval.Visible = supervisor
        btnFinanceApproval.Enabled =
            supervisor AndAlso
            txtpaymentmethod.Text = SalaryDeductions.METHOD AndAlso
            txnStatus = SalaryDeductions.PENDING_STATUS
    End Sub

    Private Function DepartmentOf(program As String, grade As String) As String
        Select Case program.Trim().ToUpper()
            Case "BSIT", "BSCS" : Return "CCS"
            Case "BSCPE", "BSIE" : Return "COE"
            Case "BSA" : Return "CBA"
            Case "BSHM", "BSTM" : Return "CHTM"
            Case "BSPSYCH" : Return "CAS"
            Case "BSCRIM" : Return "CCJ"
            Case "BEED", "BSED" : Return "COED"
            Case "BSBA" : Return "BSBA"
            Case "BSCA" : Return "BSCA"
            Case "BSREM" : Return "BSREM"
            Case "JD" : Return "College of Law"
            Case "ABM", "HUMSS", "GAS", "STEM" : Return "Senior High School"
        End Select
        If grade.StartsWith("Grade") OrElse grade.StartsWith("Kinder") Then Return "Basic Education"
        Return If(program.Trim() = "", "N/A", program)
    End Function

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
        numupqty.Value = Math.Max(numupqty.Minimum, Math.Min(numupqty.Minimum, CDec(stock)))
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
            If it.IsBackorder AndAlso Convert.ToInt32(If(ExecScalar(
                    "SELECT COUNT(*) FROM tbl_transaction_items WHERE transaction_item_id = @i AND is_backorder = 1 AND released_at IS NULL",
                    New String() {"@i"}, New Object() {it.TransactionItemId}), 0)) > 0 Then
                MsgBox(nm & " was paid for but has not been handed over yet (pick-up item), so there is nothing to return." & vbCrLf &
                       "Ask a supervisor to cancel the transaction instead.", vbExclamation, "Return / Exchange") : Exit Sub
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
        Dim resellable As Boolean = Not String.Equals(cboCondition.Text, "Damaged", StringComparison.OrdinalIgnoreCase)

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

        If txtProduct.SelectedIndex >= 0 AndAlso String.Equals(typed, txtProduct.Text, StringComparison.OrdinalIgnoreCase) Then Exit Sub

        Dim foundIndex As Integer = -1
        For i As Integer = 0 To txtProduct.Items.Count - 1
            Dim drv As DataRowView = TryCast(txtProduct.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso String.Equals(drv("product_name").ToString().Trim(), typed, StringComparison.OrdinalIgnoreCase) Then
                foundIndex = i
                Exit For
            End If
        Next

        If foundIndex >= 0 Then
            txtProduct.SelectedIndex = foundIndex
        Else
            MsgBox("Please select a valid replacement product from the list.", vbExclamation, "Product Selection")
            txtProduct.Focus()
            e.Cancel = True
        End If
    End Sub

    ' ===================== PRINT RECEIPT =====================
    Private Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnprint.Click
        If txnId = 0 Then Exit Sub
        If txtpaymentmethod.Text.Trim() = SalaryDeductions.METHOD Then
            Try
                SalaryDeductions.PrintSlip(txnId, Me)
            Catch ex As Exception
                MsgBox(
            "Could not print the deduction slip: " & ex.Message,
            vbCritical,
            "Salary Deduction Slip")
            End Try

            Exit Sub
        End If
        ' Block bookstore receipt printing for module orders paid at the school cashier
        If String.Equals(
        txtpaymentmethod.Text.Trim(),
        ModuleOrders.PAYMENT_METHOD,
        StringComparison.OrdinalIgnoreCase) Then

            MsgBox(
            "Module payment receipts are issued by the school cashier." &
            vbCrLf &
            "Print the payment slip from Modules for Release.",
            vbInformation,
            "Module Order")

            Exit Sub
        End If

        printPreview.Document = printDoc
        printPreview.Width = 600
        printPreview.Height = 800
        printPreview.ShowDialog(Me)
    End Sub
    Private Sub printDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDoc.PrintPage
        Dim g As Graphics = e.Graphics
        Dim fHeader As New Font("Segoe UI", 12, FontStyle.Bold)
        Dim fReg As New Font("Segoe UI", 9, FontStyle.Regular)
        Dim fBold As New Font("Segoe UI", 9, FontStyle.Bold)
        Dim brush As Brush = Brushes.Black
        Dim x As Single = 20
        Dim y As Single = 20
        Dim w As Single = e.PageBounds.Width - 40

        ' Header
        g.DrawString("LYCEUM OF ALABANG - BOOKSTORE", fHeader, brush, x, y)
        y += 25
        g.DrawString("Transaction Receipt Details", fReg, brush, x, y)
        y += 20
        g.DrawLine(Pens.Black, x, y, x + w, y)
        y += 10

        ' Metadata fields using corrected labels
        RcL(g, "OR No:", rcOR, fReg, fBold, x, y, w) : y += 18
        RcL(g, "Date:", rcDate, fReg, fBold, x, y, w) : y += 18
        RcL(g, "Buyer Name:", rcBuyer, fReg, fBold, x, y, w) : y += 18
        RcL(g, "ID No:", rcStudentNo, fReg, fBold, x, y, w) : y += 18
        RcL(g, "Buyer Type:", txtgrade.Text, fReg, fBold, x, y, w) : y += 18
        RcL(g, "Department:", txtProgramStrand.Text, fReg, fBold, x, y, w) : y += 18
        RcL(g, "Payment Method:", rcPayment, fReg, fBold, x, y, w) : y += 18
        RcL(g, "Cashier:", rcCashier, fReg, fBold, x, y, w) : y += 18
        RcL(g, "Status:", rcStatus, fReg, fBold, x, y, w) : y += 25

        g.DrawLine(Pens.Gray, x, y, x + w, y)
        y += 10

        ' Items grid header
        g.DrawString("Items Purchased:", fBold, brush, x, y)
        y += 20

        For Each row As DataGridViewRow In dgvCart.Rows
            If row.IsNewRow Then Continue For
            Dim pName As String = Convert.ToString(row.Cells(1).Value)
            Dim qty As String = Convert.ToString(row.Cells(4).Value)
            Dim subTot As String = Convert.ToString(row.Cells(6).Value)

            g.DrawString(pName & " (x" & qty & ")", fReg, brush, x, y)
            g.DrawString("₱" & subTot, fReg, brush, x + w - 100, y)
            y += 18
        Next

        y += 10
        g.DrawLine(Pens.Gray, x, y, x + w, y)
        y += 15

        ' Totals
        RcL(g, "Total Amount:", "₱" & rcTotal.ToString("N2"), fReg, fBold, x, y, w) : y += 18
        RcL(g, "Amount Paid:", "₱" & rcPaid.ToString("N2"), fReg, fBold, x, y, w) : y += 18
        RcL(g, "Change:", "₱" & rcChange.ToString("N2"), fReg, fBold, x, y, w)
    End Sub

    Private Sub RcL(g As Graphics, label As String, val As String, fReg As Font, fBold As Font, x As Single, y As Single, w As Single)
        g.DrawString(label, fReg, Brushes.Black, x, y)
        g.DrawString(val, fBold, Brushes.Black, x + 120, y)
    End Sub
End Class