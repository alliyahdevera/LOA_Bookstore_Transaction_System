Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Transactions
Imports MySql.Data.MySqlClient

Public Class frmPOS

    Private foundStudentId As Integer = 0
    Private selectedStudentNo As String = ""
    Private paymentDate As Date = Date.Today
    Private paymentEmployee As String = ""
    Private isUpdatingNud As Boolean = False

    ' Column names/indices matching SetupDataGridView & LoadProducts
    Private Const COL_CODE As String = "colCode"
    Private Const COL_NAME As String = "colName"
    Private Const COL_CATEGORY As String = "colCategory"
    Private Const COL_TYPE As String = "colType"
    Private Const COL_SIZE As String = "colSize"
    Private Const COL_PRICE As String = "colPrice"
    Private Const COL_STOCK As String = "colStock"
    Private Const COL_STATUS As String = "colStatus"

    Private Const MAX_PICKUP_QTY As Integer = 10
    Private ReadOnly backorders As New Dictionary(Of Integer, Date)   ' variant_id -> pick-up date

    ' ================= LOAD & INITIALIZATION =================
    Private Sub frmPOS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Prevent auto-generating extra columns at runtime
        dgvlistproducts.AutoGenerateColumns = False
        dgvCart.AutoGenerateColumns = False

        ' Set up grid configurations
        SetupDataGridView()
        SetupGrid(dgvlistproducts)
        SetupCartGrid(dgvCart)

        ' Format numeric grid columns
        If dgvlistproducts.Columns.Contains(COL_PRICE) Then
            dgvlistproducts.Columns(COL_PRICE).DefaultCellStyle.Format = "N2"
        End If
        If dgvCart.Columns.Contains("UnitPrice") Then
            dgvCart.Columns("UnitPrice").DefaultCellStyle.Format = "N2"
        End If
        If dgvCart.Columns.Contains("SubTotal") Then
            dgvCart.Columns("SubTotal").DefaultCellStyle.Format = "N2"
        End If

        ' Configure numeric up/down control
        nudQuantity.DecimalPlaces = 0
        nudQuantity.Minimum = 0

        ' Load category list dynamically from database
        LoadCategoryComboBox()

        ' Reset all form fields and fetch initial product list
        ResetAll()
    End Sub

    Private Function Ask(msg As String, title As String) As Boolean
        Return MsgBox(msg, vbYesNo + vbQuestion, title) = MsgBoxResult.Yes
    End Function

    Private Sub LoadCategoryComboBox()
        cbocategory.DropDownStyle = ComboBoxStyle.DropDownList
        cbocategory.Items.Clear()
        cbocategory.Items.Add("All Items")

        Dim dt As DataTable = GetDataTable("SELECT category_name FROM tbl_categories ORDER BY category_name")
        For Each row As DataRow In dt.Rows
            cbocategory.Items.Add(row("category_name").ToString())
        Next

        cbocategory.SelectedIndex = 0
    End Sub

    Private Sub SetupDataGridView()
        dgvlistproducts.Columns.Clear()
        dgvlistproducts.AutoGenerateColumns = False

        dgvlistproducts.Columns.Add(COL_CODE, "Product Code")
        dgvlistproducts.Columns.Add(COL_NAME, "Product Name")
        dgvlistproducts.Columns.Add(COL_CATEGORY, "Category")
        dgvlistproducts.Columns.Add(COL_TYPE, "Type")
        dgvlistproducts.Columns.Add(COL_SIZE, "Size")
        dgvlistproducts.Columns.Add(COL_PRICE, "Price")
        dgvlistproducts.Columns.Add(COL_STOCK, "Stock")
        dgvlistproducts.Columns.Add(COL_STATUS, "Status")
    End Sub

    Private Sub SetupGrid(dgv As DataGridView)
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.MultiSelect = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    Private Sub SetupCartGrid(dgv As DataGridView)
        dgv.ReadOnly = False
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.MultiSelect = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        ' Set all columns to ReadOnly except Quantity
        For Each col As DataGridViewColumn In dgv.Columns
            If col.Name = "Quantity" Then
                col.ReadOnly = False
            Else
                col.ReadOnly = True
            End If
        Next
    End Sub

    ' ================= STUDENT SEARCH =================
    Private Sub btnSearchStudent_Click(sender As Object, e As EventArgs) Handles btnSearchStudent.Click
        If Not Ask("Search for a student record?", "Search Student") Then Exit Sub
        Using frm As New frmStudentList()
            frm.InitialSearch = txtStudentNo.Text.Trim()
            frm.StartPosition = FormStartPosition.CenterParent
            If frm.ShowDialog(Me) = DialogResult.OK Then
                foundStudentId = frm.SelectedStudentId
                selectedStudentNo = frm.SelectedStudentNo
                txtStudentNo.Text = frm.SelectedStudentNo
                txtStudentName.Text = frm.SelectedStudentName
                txtgrade.Text = frm.SelectedGradeLevel
                txtProgramStrand.Text = frm.SelectedProgramStrand
                txtGuestName.Clear()
            End If
        End Using
    End Sub

    Private Sub txtStudentNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtStudentNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            btnSearchStudent.PerformClick()
        End If
    End Sub

    Private Sub txtStudentNo_TextChanged(sender As Object, e As EventArgs) Handles txtStudentNo.TextChanged
        If foundStudentId > 0 AndAlso txtStudentNo.Text <> selectedStudentNo Then
            ClearStudentFields(True)
        End If
    End Sub

    Private Sub txtGuestName_TextChanged(sender As Object, e As EventArgs) Handles txtGuestName.TextChanged
        If txtGuestName.Text <> "" AndAlso foundStudentId > 0 Then
            ClearStudentFields(False)
        End If
    End Sub

    Private Sub ClearStudentFields(keepStudentNoText As Boolean)
        foundStudentId = 0
        selectedStudentNo = ""
        If Not keepStudentNoText Then txtStudentNo.Clear()
        txtStudentName.Clear()
        txtgrade.Clear()
        txtProgramStrand.Clear()
    End Sub

    ' ================= CATEGORY + PRODUCT SEARCH =================
    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        ResetQuantity()
        LoadProducts()
    End Sub

    Private Sub txtProductSearch_TextChanged(sender As Object, e As EventArgs) Handles txtProductSearch.TextChanged
        ResetQuantity()
        LoadProducts()
    End Sub

    Private Sub picSearchProduct_Click(sender As Object, e As EventArgs) Handles picSearchProduct.Click
        LoadProducts()
    End Sub

    Private Sub LoadProducts()
        dgvlistproducts.Rows.Clear()

        Dim keyword As String = txtProductSearch.Text.Trim()
        Dim category As String = If(cbocategory.SelectedIndex = -1 OrElse cbocategory.Text = "All Items", "", cbocategory.Text)

        Dim query As String =
            "SELECT v.variant_id, v.product_code, p.product_name, " &
            "COALESCE(c.category_name, 'Uncategorized') AS category_name, " &
            "COALESCE(ct.type_name, 'N/A') AS type_name, " &
            "v.size, p.unit_price, v.quantity_on_hand, v.reorder_level " &
            "FROM tbl_product_variants v " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "LEFT JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "LEFT JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "WHERE p.status = 'Active' "

        Dim paramNames As New List(Of String)()
        Dim paramValues As New List(Of Object)()

        If category <> "" Then
            query &= "AND c.category_name = @cat "
            paramNames.Add("@cat")
            paramValues.Add(category)
        End If

        If keyword <> "" Then
            query &= "AND (v.product_code LIKE @like OR p.product_name LIKE @like) "
            paramNames.Add("@like")
            paramValues.Add("%" & keyword & "%")
        End If

        query &= "ORDER BY p.product_name, v.size "

        If category = "" AndAlso keyword = "" Then
            query &= "LIMIT 100"
        End If

        Dim dt As DataTable = GetDataTable(query, paramNames.ToArray(), paramValues.ToArray())

        dgvlistproducts.SuspendLayout()
        For Each r As DataRow In dt.Rows
            Dim stock As Integer = Convert.ToInt32(r("quantity_on_hand"))
            Dim reorder As Integer = Convert.ToInt32(r("reorder_level"))
            Dim status As String = If(stock <= 0, "Out of Stock", If(stock <= reorder, "Low Stock", "In Stock"))

            Dim idx As Integer = dgvlistproducts.Rows.Add(
                r("product_code").ToString(),
                r("product_name").ToString(),
                r("category_name").ToString(),
                r("type_name").ToString(),
                r("size").ToString(),
                Convert.ToDecimal(r("unit_price")),
                stock,
                status)
            dgvlistproducts.Rows(idx).Tag = Convert.ToInt32(r("variant_id"))
        Next
        dgvlistproducts.ResumeLayout()

        ClearGridSelection(dgvlistproducts)
    End Sub

    Private Sub dgvlistproducts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvlistproducts.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim stock As Integer = Convert.ToInt32(dgvlistproducts.Rows(e.RowIndex).Cells(COL_STOCK).Value)

        isUpdatingNud = True
        nudQuantity.Maximum = If(stock > 0, stock, MAX_PICKUP_QTY)
        nudQuantity.Value = 1
        isUpdatingNud = False
    End Sub

    ' ================= QUANTITY VALIDATION & PROMPTS =================
    Private Sub nudQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles nudQuantity.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub nudQuantity_TextChanged(sender As Object, e As EventArgs) Handles nudQuantity.TextChanged
        If isUpdatingNud OrElse nudQuantity.Text = "" Then Exit Sub
        Dim typed As Decimal
        If Not Decimal.TryParse(nudQuantity.Text, typed) Then Exit Sub

        If typed > nudQuantity.Maximum Then
            Dim limit As Integer = Convert.ToInt32(nudQuantity.Maximum)
            Dim stock As Integer = 0
            If dgvlistproducts.SelectedRows.Count > 0 Then
                stock = Convert.ToInt32(dgvlistproducts.SelectedRows(0).Cells(COL_STOCK).Value)
            End If

            isUpdatingNud = True
            nudQuantity.Value = limit
            nudQuantity.Text = limit.ToString()
            isUpdatingNud = False
            nudQuantity.Select(nudQuantity.Text.Length, 0)

            If dgvlistproducts.SelectedRows.Count = 0 Then
                MsgBox("Select an item from the list first.", vbExclamation, "Quantity Exceeded")
            ElseIf stock > 0 Then
                MsgBox("Quantity cannot exceed the available stock (" & stock & ").", vbExclamation, "Quantity Exceeded")
            Else
                MsgBox("Pick-up items are limited to " & MAX_PICKUP_QTY & " per item.", vbExclamation, "Quantity Exceeded")
            End If
        End If
    End Sub

    Private Sub ResetQuantity()
        isUpdatingNud = True
        nudQuantity.Value = 0
        nudQuantity.Maximum = 0
        isUpdatingNud = False
    End Sub

    Private Sub btnAddToCart_Click(sender As Object, e As EventArgs) Handles btnAddToCart.Click
        If dgvlistproducts.SelectedRows.Count = 0 Then
            MsgBox("Please click an item in the list first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        Dim qty As Integer = Convert.ToInt32(nudQuantity.Value)
        If qty <= 0 Then
            MsgBox("Quantity must be at least 1.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        Dim prow As DataGridViewRow = dgvlistproducts.SelectedRows(0)
        Dim variantId As Integer = Convert.ToInt32(prow.Tag)
        Dim stock As Integer = Convert.ToInt32(prow.Cells(COL_STOCK).Value)
        Dim price As Decimal = Convert.ToDecimal(prow.Cells(COL_PRICE).Value)
        Dim itemName As String = Convert.ToString(prow.Cells(COL_NAME).Value)
        Dim isBack As Boolean = (stock <= 0)

        Dim existing As DataGridViewRow = Nothing
        For Each crow As DataGridViewRow In dgvCart.Rows
            If Convert.ToInt32(crow.Tag) = variantId Then
                existing = crow
                Exit For
            End If
        Next
        Dim alreadyInCart As Integer = If(existing IsNot Nothing, Convert.ToInt32(existing.Cells("Quantity").Value), 0)

        If isBack Then
            If alreadyInCart + qty > MAX_PICKUP_QTY Then
                MsgBox("Pick-up items are limited to " & MAX_PICKUP_QTY & " per item.", vbExclamation, "Point of Sale")
                Exit Sub
            End If
            If Not backorders.ContainsKey(variantId) Then
                If Not Ask("'" & itemName & "' is out of stock." & vbCrLf & vbCrLf &
                       "Record it as a pick-up item? The customer pays now and returns to claim it on the date you choose.",
                       "Out of Stock") Then Exit Sub
                Dim d As Date? = PromptPickupDate(itemName)
                If Not d.HasValue Then Exit Sub
                backorders(variantId) = d.Value
            End If
        Else
            If alreadyInCart + qty > stock Then
                MsgBox("Cannot enter quantity higher than stock (" & stock & "). You already have " & alreadyInCart & " in the cart.", vbExclamation, "Point of Sale")
                Exit Sub
            End If
            If Not Ask("Add " & qty & " x " & itemName & " to the cart?", "Add to Cart") Then Exit Sub
            backorders.Remove(variantId)
        End If

        If existing IsNot Nothing Then
            Dim newQty As Integer = alreadyInCart + qty
            existing.Cells("Quantity").Value = newQty
            existing.Cells("SubTotal").Value = newQty * price
        Else
            Dim label As String = itemName
            If isBack Then label &= " [Pick-up " & backorders(variantId).ToString("MMM d") & "]"
            Dim idx As Integer = dgvCart.Rows.Add(label, prow.Cells(COL_SIZE).Value, qty, price, qty * price)
            dgvCart.Rows(idx).Tag = variantId
            If isBack Then dgvCart.Rows(idx).DefaultCellStyle.ForeColor = Color.DarkOrange
        End If

        ResetQuantity()
        ClearGridSelection(dgvlistproducts)
        ClearGridSelection(dgvCart)
        RecalculateTotal()
        InvalidatePayment()
    End Sub

    ' ================= VERIFY PASSWORD & PROMPTS =================
    Private Function VerifyCashierPassword() As Boolean
        For attempt As Integer = 1 To 3
            Dim pw As String = PromptPassword()
            If pw Is Nothing Then Return False

            Dim n As Integer = Convert.ToInt32(If(ExecScalar(
            "SELECT COUNT(*) FROM tbl_users WHERE user_id = @u AND password = @p AND status = 'Active'",
            New String() {"@u", "@p"}, New Object() {currentuser.UserID, HashPassword(pw)}), 0))
            If n > 0 Then Return True

            MsgBox("Incorrect password. Attempt " & attempt & " of 3.", vbExclamation, "Save Transaction")
        Next
        LogActivity("Sale - Password Failed", txtTransactionNo.Text, "3 incorrect password attempts while saving a transaction")
        Return False
    End Function

    Private Function PromptPassword() As String
        Using dlg As New Form(), lbl As New Label(), txt As New TextBox(), ok As New Button(), cancel As New Button()
            dlg.Text = "Confirm Password"
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog
            dlg.StartPosition = FormStartPosition.CenterParent
            dlg.MinimizeBox = False
            dlg.MaximizeBox = False
            dlg.ClientSize = New Size(320, 120)

            lbl.Text = "Enter your password to save this transaction:"
            lbl.SetBounds(12, 12, 296, 20)
            txt.UseSystemPasswordChar = True
            txt.SetBounds(12, 38, 296, 23)
            ok.Text = "Confirm"
            ok.DialogResult = DialogResult.OK
            ok.SetBounds(142, 80, 80, 28)
            cancel.Text = "Cancel"
            cancel.DialogResult = DialogResult.Cancel
            cancel.SetBounds(228, 80, 80, 28)

            dlg.AcceptButton = ok
            dlg.CancelButton = cancel
            dlg.Controls.AddRange(New Control() {lbl, txt, ok, cancel})

            If dlg.ShowDialog(Me) = DialogResult.OK Then Return txt.Text
        End Using
        Return Nothing
    End Function

    Private Function PromptPickupDate(itemName As String) As Date?
        Using dlg As New Form(), lbl As New Label(), dtp As New DateTimePicker(), ok As New Button(), cancel As New Button()
            dlg.Text = "Pick-up Date"
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog
            dlg.StartPosition = FormStartPosition.CenterParent
            dlg.MinimizeBox = False
            dlg.MaximizeBox = False
            dlg.ClientSize = New Size(340, 120)

            lbl.Text = "When can the customer claim '" & itemName & "'?"
            lbl.SetBounds(12, 12, 316, 36)
            dtp.Format = DateTimePickerFormat.Long
            dtp.MinDate = Date.Today.AddDays(1)
            dtp.Value = Date.Today.AddDays(1)
            dtp.SetBounds(12, 50, 316, 23)
            ok.Text = "OK"
            ok.DialogResult = DialogResult.OK
            ok.SetBounds(162, 82, 80, 28)
            cancel.Text = "Cancel"
            cancel.DialogResult = DialogResult.Cancel
            cancel.SetBounds(248, 82, 80, 28)

            dlg.AcceptButton = ok
            dlg.CancelButton = cancel
            dlg.Controls.AddRange(New Control() {lbl, dtp, ok, cancel})

            If dlg.ShowDialog(Me) = DialogResult.OK Then Return dtp.Value.Date
        End Using
        Return Nothing
    End Function

    ' ================= CART GRID VALIDATION =================
    Private Sub dgvCart_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvCart.CellValidating
        If e.RowIndex < 0 OrElse dgvCart.Columns(e.ColumnIndex).Name <> "Quantity" Then Exit Sub

        Dim newValue As Integer = 0
        If Not Integer.TryParse(e.FormattedValue.ToString(), newValue) OrElse newValue <= 0 Then
            MsgBox("Please enter a valid quantity of at least 1.", vbExclamation, "Invalid Input")
            e.Cancel = True
            Exit Sub
        End If

        Dim editedRow As DataGridViewRow = dgvCart.Rows(e.RowIndex)
        Dim variantId As Integer = Convert.ToInt32(editedRow.Tag)
        Dim availableStock As Integer = GetStockForVariant(variantId)

        If backorders.ContainsKey(variantId) Then
            If newValue > MAX_PICKUP_QTY Then
                MsgBox("Pick-up items are limited to " & MAX_PICKUP_QTY & " per item.", vbExclamation, "Quantity Exceeded")
                e.Cancel = True
            End If
        ElseIf newValue > availableStock Then
            MsgBox("Quantity (" & newValue & ") cannot exceed available stock (" & availableStock & ").", vbExclamation, "Quantity Exceeded")
            e.Cancel = True
        End If
    End Sub

    Private Sub dgvCart_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellEndEdit
        If e.RowIndex < 0 OrElse dgvCart.Columns(e.ColumnIndex).Name <> "Quantity" Then Exit Sub

        Dim editedRow As DataGridViewRow = dgvCart.Rows(e.RowIndex)
        Dim price As Decimal = Convert.ToDecimal(editedRow.Cells("UnitPrice").Value)
        Dim qty As Integer = Convert.ToInt32(editedRow.Cells("Quantity").Value)

        editedRow.Cells("SubTotal").Value = qty * price
        RecalculateTotal()
        InvalidatePayment()
    End Sub

    Private Function GetStockForVariant(variantId As Integer) As Integer
        For Each prow As DataGridViewRow In dgvlistproducts.Rows
            If prow.Tag IsNot Nothing AndAlso Convert.ToInt32(prow.Tag) = variantId Then
                Return Convert.ToInt32(prow.Cells(COL_STOCK).Value)
            End If
        Next

        Try
            If connection() Then
                Dim q As String = "SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @vid"
                Dim dt As DataTable = GetDataTable(q, New String() {"@vid"}, New Object() {variantId})
                If dt.Rows.Count > 0 Then
                    Return Convert.ToInt32(dt.Rows(0)("quantity_on_hand"))
                End If
            End If
        Catch ex As Exception
        End Try

        Return 0
    End Function

    ' ================= REMOVE ITEM =================
    Private Sub btnRemoveItem_Click(sender As Object, e As EventArgs) Handles btnRemoveItem.Click
        If dgvCart.Rows.Count = 0 Then
            MsgBox("The cart is already empty.", vbInformation, "Remove Item")
            Exit Sub
        End If

        If dgvCart.SelectedRows.Count > 0 Then
            If MsgBox("Are you sure you want to remove the selected item?", vbYesNo + vbQuestion, "Remove Item") <> MsgBoxResult.Yes Then
                Exit Sub
            End If
            backorders.Remove(Convert.ToInt32(dgvCart.SelectedRows(0).Tag))
            dgvCart.Rows.RemoveAt(dgvCart.SelectedRows(0).Index)
        Else
            If MsgBox("No item is selected. Remove ALL items from the cart?", vbYesNo + vbQuestion, "Remove Item") <> MsgBoxResult.Yes Then
                Exit Sub
            End If
            backorders.Clear()
            dgvCart.Rows.Clear()
        End If

        ClearGridSelection(dgvCart)
        RecalculateTotal()
        InvalidatePayment()
    End Sub

    Private Sub dgvCart_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvCart.MouseDown
        If dgvCart.HitTest(e.X, e.Y).Type = DataGridViewHitTestType.None Then
            ClearGridSelection(dgvCart)
        End If
    End Sub

    ' ================= CLEAR / CANCEL / SETTLE =================
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        If MsgBox("Are you sure you want to clear the customer fields?", vbYesNo + vbQuestion, "Clear Fields") <> MsgBoxResult.Yes Then
            Exit Sub
        End If
        ResetCustomerInfo()
    End Sub

    Private Sub btnCancelTransaction_Click(sender As Object, e As EventArgs) Handles btnCancelTransaction.Click
        If MsgBox("Cancel this transaction? All entered details and both item lists will be cleared.", vbYesNo + vbQuestion, "Point of Sale") = MsgBoxResult.Yes Then
            backorders.Clear()
            ResetAll()
        End If
    End Sub

    Private Sub btnSettlePayment_Click(sender As Object, e As EventArgs) Handles btnSettlePayment.Click
        If dgvCart.Rows.Count = 0 Then
            MsgBox("Add at least one item to the cart first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        If Not Ask("Proceed to payment for " & txtTotalAMount.Text & "?", "Settle Payment") Then Exit Sub

        Using frm As New frmPayment()
            frm.GrandTotal = ToMoney(txtTotalAMount.Text)
            frm.PrefillORNo = txtReferenceNo.Text.Trim()
            frm.PrefillDate = paymentDate
            frm.PrefillMethod = txtPaymentMethod.Text.Trim()
            frm.PrefillEmployee = paymentEmployee
            frm.PrefillReceived = ToMoney(txtAmountReceived.Text)
            frm.StartPosition = FormStartPosition.CenterParent

            If frm.ShowDialog(Me) = DialogResult.OK Then
                txtReferenceNo.Text = frm.ResultORNo
                paymentDate = frm.ResultDate
                txttransactdate.Text = paymentDate.ToString("MMMM d, yyyy")
                txtPaymentMethod.Text = frm.ResultMethod
                paymentEmployee = frm.ResultEmployee
                txtAmountReceived.Text = frm.ResultReceived.ToString("N2")
                txtAmountChange.Text = frm.ResultChange.ToString("N2")
            End If
        End Using
    End Sub
    Private Function ToMoney(input As String) As Decimal
        If String.IsNullOrWhiteSpace(input) Then Return 0
        ' Strip currency symbols, commas, and whitespace
        Dim cleaned As String = input.Replace("₱", "").Replace("$", "").Replace(",", "").Trim()
        Dim result As Decimal
        If Decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, result) Then
            Return result
        End If
        Return 0
    End Function

    ' ================= SAVE TRANSACTION =================
    Private Sub btnSaveTransaction_Click(sender As Object, e As EventArgs) Handles btnSaveTransaction.Click
        If dgvCart.Rows.Count = 0 Then
            MsgBox("Add at least one item to the cart first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtReferenceNo.Text) OrElse String.IsNullOrWhiteSpace(txtPaymentMethod.Text) Then
            MsgBox("Payment is not settled. Click 'Settle Payment' first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        Dim buyerName As String = If(foundStudentId > 0, txtStudentName.Text.Trim(), txtGuestName.Text.Trim())
        If buyerName = "" Then
            MsgBox("Search and select a student, or type a guest name.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        If foundStudentId = 0 Then
            Dim ids As New List(Of String)
            For Each crow As DataGridViewRow In dgvCart.Rows
                ids.Add(Convert.ToInt32(crow.Tag).ToString())
            Next
            Dim uniformCount As Integer = Convert.ToInt32(If(ExecScalar(
                "SELECT COUNT(*) FROM tbl_product_variants v " &
                "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
                "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
                "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
                "WHERE c.category_name = 'Uniforms' AND v.variant_id IN (" & String.Join(",", ids) & ")"), 0))
            If uniformCount > 0 Then
                MsgBox("Guests may only buy supplies. Uniforms are for students only (a relative may buy only when with the student).", vbExclamation, "Point of Sale")
                Exit Sub
            End If
        End If

        If MsgBox("Are you sure you want to save this transaction?", vbYesNo + vbQuestion, "Save Transaction") <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        If Not VerifyCashierPassword() Then Exit Sub

        Dim total As Decimal = ToMoney(txtTotalAMount.Text)
        Dim received As Decimal = ToMoney(txtAmountReceived.Text)
        Dim change As Decimal = ToMoney(txtAmountChange.Text)
        If total <= 0 OrElse received < total OrElse change < 0 Then
            MsgBox("The payment does not cover the total. Click 'Settle Payment' again.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        Dim buyerType As String = If(foundStudentId > 0, "Student", "Walk-in")
        Dim txnNo As String = If(String.IsNullOrWhiteSpace(txtTransactionNo.Text), NewTransactionNo(), txtTransactionNo.Text.Trim())
        Dim method As String = txtPaymentMethod.Text.Trim()

        Try
            If Not connection() Then Exit Sub
            Dim trans As MySqlTransaction = cn.BeginTransaction()
            Try
                Dim transactionId As Long = 0
                Dim insTxn As String = "INSERT INTO TBL_TRANSACTIONS " &
                                        "(transaction_no, buyer_type, student_id, buyer_name, or_no, or_date, payment_method, employee_name, " &
                                        "total_amount, amount_paid, amount_change, created_by, status) " &
                                        "VALUES (@tno, @bt, @sid, @bn, @orno, @ord, @pm, @emp, @tot, @paid, @chg, @by, 'Completed')"

                Using c1 As New MySqlCommand(insTxn, cn, trans)
                    c1.Parameters.AddWithValue("@tno", txnNo)
                    c1.Parameters.AddWithValue("@bt", buyerType)
                    c1.Parameters.AddWithValue("@sid", If(foundStudentId > 0, CType(foundStudentId, Object), DBNull.Value))
                    c1.Parameters.AddWithValue("@bn", buyerName)
                    c1.Parameters.AddWithValue("@orno", txtReferenceNo.Text.Trim())
                    c1.Parameters.AddWithValue("@ord", paymentDate.Date)
                    c1.Parameters.AddWithValue("@pm", method)
                    c1.Parameters.AddWithValue("@emp", If(paymentEmployee = "", CType(DBNull.Value, Object), paymentEmployee))
                    c1.Parameters.AddWithValue("@tot", total)
                    c1.Parameters.AddWithValue("@paid", received)
                    c1.Parameters.AddWithValue("@chg", change)
                    c1.Parameters.AddWithValue("@by", currentuser.UserID)
                    c1.ExecuteNonQuery()
                    transactionId = c1.LastInsertedId
                End Using

                For Each row As DataGridViewRow In dgvCart.Rows
                    Dim variantId As Integer = Convert.ToInt32(row.Tag)
                    Dim subtotal As Decimal = Convert.ToDecimal(row.Cells("SubTotal").Value)
                    Dim qty As Integer = Convert.ToInt32(row.Cells("Quantity").Value)
                    Dim isBack As Boolean = backorders.ContainsKey(variantId)

                    Using c2 As New MySqlCommand("INSERT INTO TBL_TRANSACTION_ITEMS (transaction_id, variant_id, quantity, subtotal, is_backorder, pickup_date) VALUES (@t, @v, @q, @s, @bo, @pd)", cn, trans)
                        c2.Parameters.AddWithValue("@t", transactionId)
                        c2.Parameters.AddWithValue("@v", variantId)
                        c2.Parameters.AddWithValue("@q", qty)
                        c2.Parameters.AddWithValue("@s", subtotal)
                        c2.Parameters.AddWithValue("@bo", If(isBack, 1, 0))
                        c2.Parameters.AddWithValue("@pd", If(isBack, CType(backorders(variantId), Object), DBNull.Value))
                        c2.ExecuteNonQuery()
                    End Using

                    If Not isBack Then
                        Using c3 As New MySqlCommand("UPDATE TBL_PRODUCT_VARIANTS SET quantity_on_hand = quantity_on_hand - @q WHERE variant_id = @v AND quantity_on_hand >= @q", cn, trans)
                            c3.Parameters.AddWithValue("@q", qty)
                            c3.Parameters.AddWithValue("@v", variantId)
                            If c3.ExecuteNonQuery() = 0 Then
                                Throw New Exception("Not enough stock left for item.")
                            End If
                        End Using
                    End If
                Next

                trans.Commit()
                MsgBox("Transaction saved successfully!", vbInformation, "Point of Sale")
                backorders.Clear()
                ResetAll()

            Catch exInner As Exception
                trans.Rollback()
                MsgBox("Transaction failed and was rolled back: " & exInner.Message, vbCritical, "Point of Sale")
            End Try
        Catch ex As Exception
            MsgBox("Error connecting to database: " & ex.Message, vbCritical, "Point of Sale")
        End Try
    End Sub

    ' ================= HELPER & RESET METHODS =================
    Private Sub ResetCustomerInfo()
        foundStudentId = 0
        selectedStudentNo = ""
        txtStudentNo.Clear()
        txtStudentName.Clear()
        txtgrade.Clear()
        txtProgramStrand.Clear()
        txtGuestName.Clear()
    End Sub

    Private Sub ResetPaymentInfo()
        txtReferenceNo.Clear()
        txtAmountReceived.Clear()
        txtAmountChange.Clear()
        txtPaymentMethod.Clear()
        txttransactdate.Clear()
        paymentEmployee = ""
        paymentDate = Date.Today
    End Sub

    Private Sub ResetAll()
        ResetCustomerInfo()
        ResetPaymentInfo()
        txtProductSearch.Clear()
        ResetQuantity()
        dgvCart.Rows.Clear()
        txtTotalAMount.Text = "₱0.00"
        txtTransactionNo.Text = NewTransactionNo()
        If cbocategory.Items.Count > 0 Then cbocategory.SelectedIndex = 0
        LoadProducts()
    End Sub

    Private Sub RecalculateTotal()
        Dim sum As Decimal = 0
        For Each r As DataGridViewRow In dgvCart.Rows
            sum += Convert.ToDecimal(r.Cells("SubTotal").Value)
        Next
        txtTotalAMount.Text = "₱" & sum.ToString("N2")
    End Sub

    Private Sub InvalidatePayment()
        ResetPaymentInfo()
    End Sub

    Private Sub ClearGridSelection(dgv As DataGridView)
        dgv.ClearSelection()
    End Sub
End Class