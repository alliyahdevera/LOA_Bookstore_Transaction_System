Imports System.Data.SqlClient
Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class frmPOS

    Private foundStudentId As Integer = 0
    Private selectedStudentNo As String = ""
    Private paymentDate As Date = Date.Today
    Private paymentEmployee As String = ""

    ' Column names/indices matching SetupDataGridView & LoadProducts
    Private Const COL_CODE As String = "colCode"
    Private Const COL_NAME As String = "colName"
    Private Const COL_CATEGORY As String = "colCategory"
    Private Const COL_TYPE As String = "colType"
    Private Const COL_SIZE As String = "colSize"
    Private Const COL_PRICE As String = "colPrice"
    Private Const COL_STOCK As String = "colStock"
    Private Const COL_STATUS As String = "colStatus"

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

    ' Helper method to populate category dropdown directly from database
    Private Sub LoadCategoryComboBox()
        cbocategory.DropDownStyle = ComboBoxStyle.DropDownList
        cbocategory.Items.Clear()

        Try
            Dim dt As DataTable = GetDataTable("SELECT category_name FROM tbl_categories ORDER BY category_name")

            For Each row As DataRow In dt.Rows
                cbocategory.Items.Add(row("category_name").ToString())
            Next

            ' Add "All Items" option at the end
            cbocategory.Items.Add("All Items")
            cbocategory.SelectedItem = "All Items"
        Catch ex As Exception
            ' Fallback if database query fails during load
            cbocategory.Items.AddRange(New Object() {
                "Uniforms", "Textbooks", "Learning Modules",
                "School Supplies", "Office Supplies",
                "Other Bookstore Items", "All Items"
            })
            cbocategory.SelectedItem = "All Items"
        End Try
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

    ' ================= STUDENT SEARCH (opens frmStudentList) =================
    Private Sub btnSearchStudent_Click(sender As Object, e As EventArgs) Handles btnSearchStudent.Click
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
                txtGuestName.Clear()      ' a student was chosen -> not a guest
            End If
        End Using
    End Sub

    Private Sub txtStudentNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtStudentNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            btnSearchStudent.PerformClick()
        End If
    End Sub

    ' typing a different student no. after choosing one cancels the chosen student
    Private Sub txtStudentNo_TextChanged(sender As Object, e As EventArgs) Handles txtStudentNo.TextChanged
        If foundStudentId > 0 AndAlso txtStudentNo.Text <> selectedStudentNo Then
            ClearStudentFields(True)
        End If
    End Sub

    ' typing a guest name cancels the chosen student
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

        ' Base SQL Query using LEFT JOINs so items aren't filtered out by missing categories
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

        ' Add Category Filter if a specific category is chosen
        If category <> "" Then
            query &= "AND c.category_name = @cat "
            paramNames.Add("@cat")
            paramValues.Add(category)
        End If

        ' Add Search Keyword Filter if typed in text box
        If keyword <> "" Then
            query &= "AND (v.product_code LIKE @like OR p.product_name LIKE @like) "
            paramNames.Add("@like")
            paramValues.Add("%" & keyword & "%")
        End If

        query &= "ORDER BY p.product_name, v.size"

        ' Execute Query
        Dim dt As DataTable = GetDataTable(query, paramNames.ToArray(), paramValues.ToArray())

        ' Populate Grid
        For Each r As DataRow In dt.Rows
            Dim stock As Integer = Convert.ToInt32(r("quantity_on_hand"))
            Dim reorder As Integer = Convert.ToInt32(r("reorder_level"))
            Dim status As String = If(stock <= 0, "Out of Stock", If(stock <= reorder, "Low Stock", "In Stock"))

            ' Map to dgvlistproducts columns
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

        ClearGridSelection(dgvlistproducts)
    End Sub

    ' Clicking an item sets the quantity to 1 (user can raise it, never above stock)
    Private Sub dgvlistproducts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvlistproducts.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim stock As Integer = Convert.ToInt32(dgvlistproducts.Rows(e.RowIndex).Cells(COL_STOCK).Value)
        If stock <= 0 Then
            ResetQuantity()
            MsgBox("This item is out of stock.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        nudQuantity.Maximum = stock
        nudQuantity.Value = 1
    End Sub

    ' ================= QUANTITY: NO NEGATIVES =================
    Private Sub nudQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles nudQuantity.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub ResetQuantity()
        nudQuantity.Value = 0
        nudQuantity.Maximum = 0
    End Sub

    ' ================= ADD TO CART =================
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

        Dim existing As DataGridViewRow = Nothing
        For Each crow As DataGridViewRow In dgvCart.Rows
            If Convert.ToInt32(crow.Tag) = variantId Then
                existing = crow
                Exit For
            End If
        Next

        Dim alreadyInCart As Integer = If(existing IsNot Nothing, Convert.ToInt32(existing.Cells("Quantity").Value), 0)
        If alreadyInCart + qty > stock Then
            MsgBox("Cannot enter quantity higher than stock (" & stock & "). You already have " & alreadyInCart & " in the cart.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        If existing IsNot Nothing Then
            Dim newQty As Integer = alreadyInCart + qty
            existing.Cells("Quantity").Value = newQty
            existing.Cells("SubTotal").Value = newQty * price
        Else
            Dim idx As Integer = dgvCart.Rows.Add(
                prow.Cells(COL_NAME).Value,
                prow.Cells(COL_SIZE).Value,
                qty,
                price,
                qty * price)
            dgvCart.Rows(idx).Tag = variantId
        End If

        ResetQuantity()
        ClearGridSelection(dgvlistproducts)
        ClearGridSelection(dgvCart)
        RecalculateTotal()
        InvalidatePayment()
    End Sub

    ' ================= CART EDIT QUANTITY & VALIDATION =================
    Private Sub dgvCart_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellEndEdit
        If e.RowIndex < 0 OrElse dgvCart.Columns(e.ColumnIndex).Name <> "Quantity" Then Exit Sub

        Dim editedRow As DataGridViewRow = dgvCart.Rows(e.RowIndex)
        Dim variantId As Integer = Convert.ToInt32(editedRow.Tag)
        Dim price As Decimal = Convert.ToDecimal(editedRow.Cells("UnitPrice").Value)
        Dim newQty As Integer = 0

        ' Validate integer input
        If Not Integer.TryParse(Convert.ToString(editedRow.Cells("Quantity").Value), newQty) OrElse newQty <= 0 Then
            MsgBox("Please enter a valid quantity of at least 1.", vbExclamation, "Point of Sale")
            editedRow.Cells("Quantity").Value = 1
            newQty = 1
        End If

        ' Get available stock from dgvlistproducts or DB
        Dim availableStock As Integer = GetStockForVariant(variantId)

        If newQty > availableStock Then
            MsgBox("Quantity cannot exceed available stock of " & availableStock & ".", vbExclamation, "Point of Sale")
            editedRow.Cells("Quantity").Value = availableStock
            newQty = availableStock
        End If

        ' Update SubTotal automatically
        editedRow.Cells("SubTotal").Value = newQty * price

        RecalculateTotal()
        InvalidatePayment()
    End Sub

    Private Function GetStockForVariant(variantId As Integer) As Integer
        For Each prow As DataGridViewRow In dgvlistproducts.Rows
            If prow.Tag IsNot Nothing AndAlso Convert.ToInt32(prow.Tag) = variantId Then
                Return Convert.ToInt32(prow.Cells(COL_STOCK).Value)
            End If
        Next

        ' Fallback to DB fetch if item is not on the currently displayed page/category of dgvlistproducts
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

        ' ================= SELECTED ITEM =================
        If dgvCart.SelectedRows.Count > 0 Then
            If MsgBox("Are you sure you want to remove the selected item?", vbYesNo + vbQuestion, "Remove Item") <> MsgBoxResult.Yes Then
                Exit Sub
            End If
            dgvCart.Rows.RemoveAt(
            dgvCart.SelectedRows(0).Index)
        Else
            ' ================= NO ITEM SELECTED =================
            If MsgBox("No item is selected. Remove ALL items from the cart?", vbYesNo + vbQuestion, "Remove Item") <> MsgBoxResult.Yes Then
                Exit Sub
            End If
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

    ' ================= CLEAR (customer info only) =================
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        If MsgBox("Are you sure you want to clear the customer fields?", vbYesNo + vbQuestion, "Clear Fields") <> MsgBoxResult.Yes Then
            Exit Sub
        End If
        ResetCustomerInfo()
    End Sub

    ' ================= CANCEL TRANSACTION =================
    Private Sub btnCancelTransaction_Click(sender As Object, e As EventArgs) Handles btnCancelTransaction.Click
        If MsgBox("Cancel this transaction? All entered details and both item lists will be cleared.", vbYesNo + vbQuestion, "Point of Sale") = MsgBoxResult.Yes Then
            ResetAll()
        End If
    End Sub

    ' ================= SETTLE PAYMENT (opens frmPayment) =================
    Private Sub btnSettlePayment_Click(sender As Object, e As EventArgs) Handles btnSettlePayment.Click
        If dgvCart.Rows.Count = 0 Then
            MsgBox("Add at least one item to the cart first.", vbExclamation, "Point of Sale")
            Exit Sub
        End If

        Using frm As New frmPayment()

            frm.GrandTotal = ToMoney(txtTotalAMount.Text)

            ' Pass previous payment details back to frmPayment
            frm.PrefillORNo = txtReferenceNo.Text.Trim()
            frm.PrefillDate = paymentDate
            frm.PrefillMethod = txtPaymentMethod.Text.Trim()
            frm.PrefillEmployee = paymentEmployee
            frm.PrefillReceived = ToMoney(txtAmountReceived.Text)

            frm.StartPosition = FormStartPosition.CenterParent

            If frm.ShowDialog(Me) = DialogResult.OK Then

                txtReferenceNo.Text = frm.ResultORNo

                paymentDate = frm.ResultDate
                txttransactdate.Text =
            paymentDate.ToString("MMMM d, yyyy")

                txtPaymentMethod.Text = frm.ResultMethod

                paymentEmployee = frm.ResultEmployee

                txtAmountReceived.Text =
            frm.ResultReceived.ToString("N2")

                txtAmountChange.Text =
            frm.ResultChange.ToString("N2")

            End If

        End Using
    End Sub

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
        Dim saved As Boolean = False

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

                    Using c2 As New MySqlCommand("INSERT INTO TBL_TRANSACTION_ITEMS (transaction_id, variant_id, quantity, subtotal) VALUES (@t, @v, @q, @s)", cn, trans)
                        c2.Parameters.AddWithValue("@t", transactionId)
                        c2.Parameters.AddWithValue("@v", variantId)
                        c2.Parameters.AddWithValue("@q", qty)
                        c2.Parameters.AddWithValue("@s", subtotal)
                        c2.ExecuteNonQuery()
                    End Using

                    Using c3 As New MySqlCommand("UPDATE TBL_PRODUCT_VARIANTS SET quantity_on_hand = quantity_on_hand - @q WHERE variant_id = @v AND quantity_on_hand >= @q", cn, trans)
                        c3.Parameters.AddWithValue("@q", qty)
                        c3.Parameters.AddWithValue("@v", variantId)
                        If c3.ExecuteNonQuery() = 0 Then
                            Throw New Exception("Not enough stock left for " & row.Cells("ProductName").Value.ToString() & ".")
                        End If
                    End Using
                Next

                trans.Commit()
                saved = True
            Catch exInner As Exception
                trans.Rollback()
                MsgBox("Transaction failed and was rolled back: " & exInner.Message, vbCritical, "Point of Sale")
            End Try
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Database error: " & ex.Message, vbCritical, "Error")
        End Try

        If saved Then
            LogActivity("Sale", txnNo, "Sale to " & buyerName & " - Total: " & total.ToString("N2") & " (" & method & ", OR " & txtReferenceNo.Text.Trim() & ")")
            MsgBox("Transaction saved successfully. You can view it in Transaction History.", vbInformation, "Point of Sale")
            ResetAll()
        End If
    End Sub

    ' ================= HELPERS =================
    Private Sub RecalculateTotal()
        Dim total As Decimal = 0
        For Each row As DataGridViewRow In dgvCart.Rows
            total += ToMoney(Convert.ToString(row.Cells("SubTotal").Value))
        Next
        txtTotalAMount.Text = "₱" & total.ToString("N2")
    End Sub

    Private Function ToMoney(s As String) As Decimal

        If String.IsNullOrWhiteSpace(s) Then Return 0D

        s = s.Replace("₱", "").Trim()

        Dim v As Decimal

        If Decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, v) Then
            Return v
        End If

        Return 0D

    End Function

    Private Sub ClearGridSelection(dgv As DataGridView)
        dgv.ClearSelection()
        dgv.CurrentCell = Nothing
    End Sub

    Private Sub InvalidatePayment()
        If String.IsNullOrWhiteSpace(txtReferenceNo.Text) Then Exit Sub
        ResetPaymentInfo()
        MsgBox("The cart was changed, so the payment was cleared. Click 'Settle Payment' again.", vbInformation, "Point of Sale")
    End Sub

    Private Sub ResetCustomerInfo()
        ClearStudentFields(False)
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
        cbocategory.SelectedIndex = cbocategory.Items.Count - 1
        LoadProducts()
    End Sub

End Class