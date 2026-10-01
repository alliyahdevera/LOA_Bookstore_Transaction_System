Imports System.Drawing.Printing
Imports MySql.Data.MySqlClient

Public Class frmTransactionDetails

    Public Property TransactionNo As String

    ' Info kept on each row of dgvCart (row.Tag)
    Private Class ItemInfo
        Public TransactionItemId As Integer
        Public VariantId As Integer
        Public UnitPrice As Decimal
        Public Purchased As Integer
        Public Processed As Integer   ' already returned/exchanged
        Public ReadOnly Property Available As Integer
            Get
                Return Purchased - Processed
            End Get
        End Property
    End Class

    Private txnId As Integer = 0
    Private txnStatus As String = ""
    Private isBinding As Boolean = False
    Private sizeTable As DataTable
    Private ReadOnly Peso As String = ChrW(8369)

    ' receipt data
    Private rcOR, rcDate, rcBuyer, rcStudentNo, rcPayment, rcCashier, rcStatus As String
    Private rcTotal, rcPaid, rcChange As Decimal
    Private rY As Single

    ' ===================== LOAD =====================
    Private Sub frmTransactionDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvCart.AllowUserToAddRows = False
        dgvCart.AllowUserToDeleteRows = False
        dgvCart.MultiSelect = False
        dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        ' locked fields
        txtcreatedby.Text = currentuser.FullName
        txtcreatedby.ReadOnly = True
        txtcreatedby.TabStop = False
        dtpORDate.Value = DateTime.Now
        dtpORDate.Enabled = False

        txtReason.MaxLength = 255
        txtcondition.MaxLength = 50

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
    End Sub

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
            "ti.quantity, ti.subtotal, " &
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

        dgvCart.Rows.Clear()
        Dim totalQty As Integer = 0
        Dim anyAvailable As Boolean = False

        For Each it As DataRow In items.Rows
            Dim qty As Integer = Convert.ToInt32(it("quantity"))
            Dim lineTotal As Decimal = Convert.ToDecimal(it("subtotal"))
            Dim info As New ItemInfo With {
                .TransactionItemId = Convert.ToInt32(it("transaction_item_id")),
                .VariantId = Convert.ToInt32(it("variant_id")),
                .UnitPrice = If(qty > 0, lineTotal / qty, 0D),
                .Purchased = qty,
                .Processed = Convert.ToInt32(it("processed_qty"))
            }
            Dim idx As Integer = dgvCart.Rows.Add(
                Convert.ToString(it("product_name")), Convert.ToString(it("category_name")),
                Convert.ToString(it("size")), qty,
                info.UnitPrice.ToString("N2"), lineTotal.ToString("N2"))
            dgvCart.Rows(idx).Tag = info
            If info.Available <= 0 Then dgvCart.Rows(idx).DefaultCellStyle.ForeColor = Color.Gray Else anyAvailable = True
            totalQty += qty
        Next

        lbltotitem.Text = items.Rows.Count.ToString()
        lbltotquantity.Text = totalQty.ToString()

        ' block processing for cancelled / fully processed transactions
        Dim canProcess As Boolean = (txnStatus <> "Cancelled") AndAlso anyAvailable
        btnreturnexc.Enabled = canProcess
        rbtnReturn.Enabled = canProcess
        rbtnexchange.Enabled = canProcess

        If dgvCart.Rows.Count > 0 Then
            dgvCart.ClearSelection()
            dgvCart.Rows(0).Selected = True
        End If
        UpdateSelectedItem()
    End Sub

    ' ===================== ITEM SELECTION =====================
    Private Function SelectedItem() As ItemInfo
        If dgvCart.SelectedRows.Count = 0 Then Return Nothing
        Return TryCast(dgvCart.SelectedRows(0).Tag, ItemInfo)
    End Function

    Private Sub dgvCart_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCart.SelectionChanged
        UpdateSelectedItem()
    End Sub

    Private Sub UpdateSelectedItem()
        Dim it As ItemInfo = SelectedItem()
        If it Is Nothing Then
            SetRange(nudQuantity, 0)
            lblquantity.Text = "of - purchased"
            Exit Sub
        End If
        SetRange(nudQuantity, it.Available)
        lblquantity.Text = If(it.Processed = 0, "of " & it.Purchased & " purchased", "of " & it.Available & " left to process")
    End Sub

    ' sets Min=1 / Max=maxValue (or 0/0 if none) and resets value to Min
    Private Sub SetRange(nud As NumericUpDown, maxValue As Integer)
        nud.Minimum = 0
        nud.Maximum = maxValue
        nud.Minimum = If(maxValue > 0, 1, 0)
        nud.Value = nud.Minimum
    End Sub

    Private Sub nudQuantity_ValueChanged(sender As Object, e As EventArgs) Handles nudQuantity.ValueChanged
        ' default replacement qty follows the returned qty
        If rbtnexchange.Checked AndAlso numupqty.Maximum > 0 Then
            numupqty.Value = Math.Max(numupqty.Minimum, Math.Min(nudQuantity.Value, numupqty.Maximum))
        End If
    End Sub

    ' ===================== ACTION TYPE =====================
    Private Sub rbtnAction_CheckedChanged(sender As Object, e As EventArgs) Handles rbtnReturn.CheckedChanged, rbtnexchange.CheckedChanged
        UpdateActionState()
    End Sub

    Private Sub UpdateActionState()
        Dim hasAction As Boolean = rbtnReturn.Checked OrElse rbtnexchange.Checked
        txtReason.ReadOnly = Not hasAction
        txtcondition.ReadOnly = Not hasAction

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
        txtcondition.Clear()
        UpdateActionState()
    End Sub

    ' ===================== REPLACEMENT ITEM =====================
    Private Sub LoadReplacementProducts()
        Dim dt As DataTable = GetDataTable(
            "SELECT p.product_id, p.product_name FROM tbl_products p " &
            "WHERE p.status = 'Active' AND EXISTS (SELECT 1 FROM tbl_product_variants v " &
            "WHERE v.product_id = p.product_id AND v.quantity_on_hand > 0) ORDER BY p.product_name")
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

    ' ===================== RETURN / EXCHANGE =====================
    Private Sub btnreturnexc_Click(sender As Object, e As EventArgs) Handles btnreturnexc.Click
        Dim it As ItemInfo = SelectedItem()
        If it Is Nothing Then
            MsgBox("Select the purchased item first.", vbExclamation, "Return / Exchange") : Exit Sub
        End If
        If it.Available <= 0 Then
            MsgBox("This item was already fully returned/exchanged.", vbExclamation, "Return / Exchange") : Exit Sub
        End If
        If Not rbtnReturn.Checked AndAlso Not rbtnexchange.Checked Then
            MsgBox("Choose Return or Exchange.", vbExclamation, "Return / Exchange") : Exit Sub
        End If

        Dim isExchange As Boolean = rbtnexchange.Checked
        Dim qty As Integer = CInt(nudQuantity.Value)
        If qty < 1 OrElse qty > it.Available Then
            MsgBox("Invalid quantity. Maximum is " & it.Available & ".", vbExclamation, "Return / Exchange") : Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtReason.Text) Then
            MsgBox("Please enter the reason.", vbExclamation, "Return / Exchange") : txtReason.Focus() : Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtcondition.Text) Then
            MsgBox("Please enter the item condition.", vbExclamation, "Return / Exchange") : txtcondition.Focus() : Exit Sub
        End If

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

        ' money summary
        Dim returnValue As Decimal = it.UnitPrice * qty
        Dim summary As String
        If isExchange Then
            Dim diff As Decimal = (repPrice * repQty) - returnValue
            summary = "Exchange " & qty & " item(s) for " & repQty & " x " & repName & "." & vbCrLf &
                      If(diff > 0, "Student pays the difference: " & Peso & diff.ToString("N2"),
                      If(diff < 0, "Refund the difference: " & Peso & Math.Abs(diff).ToString("N2"), "No price difference."))
        Else
            summary = "Return " & qty & " item(s)." & vbCrLf & "Refund to student: " & Peso & returnValue.ToString("N2")
        End If
        If MsgBox(summary & vbCrLf & vbCrLf & "Proceed?", vbYesNo + vbQuestion, "Confirm") <> MsgBoxResult.Yes Then Exit Sub

        Dim refNo As String = If(isExchange, "EXC-", "RET-") & DateTime.Now.ToString("yyyyMMddHHmmssfff")
        Dim actionName As String = If(isExchange, "Exchange", "Return")

        Try
            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        ' 1) header
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

                        ' 2) detail
                        Using q As New MySqlCommand(
                            "INSERT INTO tbl_return_exchange_items (return_exchange_id, transaction_item_id, quantity, item_condition, replacement_variant_id, replacement_quantity) " &
                            "VALUES (@rid, @tii, @q, @cond, @rv, @rq)", c, tx)
                            q.Parameters.AddWithValue("@rid", reId)
                            q.Parameters.AddWithValue("@tii", it.TransactionItemId)
                            q.Parameters.AddWithValue("@q", qty)
                            q.Parameters.AddWithValue("@cond", txtcondition.Text.Trim())
                            q.Parameters.AddWithValue("@rv", If(isExchange, CType(repVariantId, Object), DBNull.Value))
                            q.Parameters.AddWithValue("@rq", If(isExchange, CType(repQty, Object), DBNull.Value))
                            q.ExecuteNonQuery()
                        End Using

                        ' 3) returned item goes back to inventory
                        MoveStock(c, tx, it.VariantId, qty, "Returned", refNo, actionName & " of " & TransactionNo)

                        ' 4) replacement leaves inventory
                        If isExchange Then
                            MoveStock(c, tx, repVariantId, -repQty, "Stock Out", refNo, "Exchange replacement for " & TransactionNo)
                        End If

                        ' 5) update the original transaction status
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
                    Catch ex As Exception
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            LogActivity("Item " & actionName, refNo,
                        actionName & " of " & qty & " item(s) from " & TransactionNo & ". Reason: " & txtReason.Text.Trim())

            MsgBox(actionName & " processed successfully." & vbCrLf & "Reference No: " & refNo, vbInformation, "Return / Exchange")

            LoadTransaction()
            ResetActionPanel()

        Catch ex As Exception
            MsgBox("Return/Exchange failed and was rolled back: " & ex.Message, vbCritical, "Error")
        End Try
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
            RcLR(g, "Payment:", rcPayment, fReg, w)
            RcLine(g, w)

            For Each row As DataGridViewRow In dgvCart.Rows
                Dim nm As String = Convert.ToString(row.Cells(0).Value)
                Dim sz As String = Convert.ToString(row.Cells(2).Value)
                If sz <> "" AndAlso sz <> "N/A" Then nm &= " (" & sz & ")"
                RcText(g, nm, fReg, w)
                RcLR(g, "  " & row.Cells(3).Value & " x " & row.Cells(4).Value, Convert.ToString(row.Cells(5).Value), fReg, w)
            Next

            RcLine(g, w)
            RcLR(g, "TOTAL", "PHP " & rcTotal.ToString("N2"), fBold, w)
            RcLR(g, "Amount Paid", "PHP " & rcPaid.ToString("N2"), fReg, w)
            RcLR(g, "Change", "PHP " & rcChange.ToString("N2"), fReg, w)
            RcLine(g, w)
            RcLR(g, "Cashier:", rcCashier, fReg, w)
            RcLR(g, "Status:", rcStatus, fReg, w)
            rY += 8
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

End Class