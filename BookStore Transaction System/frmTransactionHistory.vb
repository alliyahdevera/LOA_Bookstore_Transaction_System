Imports MySql.Data.MySqlClient

Public Class frmTransactionHistory
    Public Property InitialSearch As String = ""
    Private Const SEARCH_FILTER As String =
    "(t.transaction_no LIKE @s OR t.buyer_name LIKE @s OR t.or_no LIKE @s OR t.status LIKE @s " &
    "OR t.id_number LIKE @s OR t.student_id IN (SELECT student_id FROM tbl_students WHERE student_no LIKE @s) " &
    "OR t.transaction_id IN (SELECT ti2.transaction_id FROM tbl_transaction_items ti2 " &
    "INNER JOIN tbl_product_variants v2 ON ti2.variant_id = v2.variant_id " &
    "INNER JOIN tbl_products p2 ON v2.product_id = p2.product_id " &
    "WHERE v2.product_code LIKE @s OR p2.product_name LIKE @s)) "
    Private isLoadingFilters As Boolean = True
    Private pg As GridPager

    Private Sub frmTransactionHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySearchPlaceholders(Me)
        SetupFooter(Me, lblname, lblposition, lbldatetime)
        LoadCategoryCombo()
        If Not dgvtransaction.Columns.Contains("CustomerID") Then
            dgvtransaction.Columns.Insert(2, New DataGridViewTextBoxColumn With {.Name = "CustomerID", .HeaderText = "Customer ID"})
            dgvtransaction.Columns.Insert(3, New DataGridViewTextBoxColumn With {.Name = "BuyerType", .HeaderText = "Buyer Type"})
        End If
        dtfrom.Value = New Date(Date.Today.Year, Date.Today.Month, 1)   ' 1st of this month
        dtto.Value = Date.Today
        txtSearch.Text = InitialSearch
        Button3.Text = "Cancel Transaction"
        Button3.Visible = (currentuser.Role = ROLE_SUPERVISOR)
        isLoadingFilters = False
        pg = New GridPager(dgvtransaction, 20)
        AddHandler pg.PageChanged, Sub() LoadGrid(txtSearch.Text.Trim())
        LoadGrid("")
    End Sub

    ' ---------- Category / Type filters ----------
    Private Sub LoadCategoryCombo()
        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList
        cboCategory.Items.Clear()
        cboCategory.Items.Add("All Categories")
        For Each r As DataRow In GetDataTable("SELECT category_name FROM tbl_categories ORDER BY category_name").Rows
            cboCategory.Items.Add(r("category_name").ToString())
        Next
        cboCategory.SelectedIndex = 0
        LoadTypeCombo()
    End Sub

    Private Sub LoadTypeCombo()
        cboType.DropDownStyle = ComboBoxStyle.DropDownList
        FillTypeNameCombo(cboType,
            If(cboCategory.SelectedIndex <= 0, "", Convert.ToString(cboCategory.SelectedItem)),
            TypeSource.Sales)
    End Sub

    Private Function SelectedCategory() As String
        Return If(cboCategory.SelectedIndex <= 0, "", Convert.ToString(cboCategory.SelectedItem))
    End Function

    Private Function SelectedType() As String
        Return If(cboType.SelectedIndex <= 0, "", Convert.ToString(cboType.SelectedItem))
    End Function
    Private Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedIndexChanged
        If isLoadingFilters Then Exit Sub
        isLoadingFilters = True
        LoadTypeCombo()
        isLoadingFilters = False
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub cboType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboType.SelectedIndexChanged
        If isLoadingFilters Then Exit Sub
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If pg Is Nothing Then Exit Sub
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        If dtfrom.Value.Date > dtto.Value.Date Then
            MsgBox("'From' date cannot be later than 'To' date.", vbExclamation, "Transaction History")
            Exit Sub
        End If
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim())

        If dgvtransaction.Rows.Count = 0 Then
            MsgBox("No transactions found for the selected dates.", vbInformation, "Transaction History")
        End If
    End Sub

    Private Sub dgvtransaction_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvtransaction.CellDoubleClick
        If e.RowIndex >= 0 Then OpenDetails()
    End Sub

    Private Sub LoadGrid(searchText As String)
        If pg Is Nothing Then Exit Sub
        Try
            Dim query As String = "SELECT t.transaction_no, t.buyer_name, t.buyer_type, t.id_number, DATE(t.created_at) AS tdate, TIME(t.created_at) AS ttime, " &
                              "v.product_code, p.product_name, v.size, p.unit_price, ti.quantity AS qty, " &
                              "ti.subtotal, t.total_amount, t.amount_paid, t.amount_change, t.payment_method, t.status, u.username " &
                              "FROM TBL_TRANSACTION_ITEMS ti " &
                              "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
                              "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                              "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                              "LEFT JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
                              "LEFT JOIN tbl_categories c ON ct.category_id = c.category_id " &
                              "INNER JOIN TBL_USERS u ON t.created_by = u.user_id " &
                              "WHERE " & SEARCH_FILTER &
                              "AND DATE(t.created_at) BETWEEN @f AND @t " &
                              "AND (@cat = '' OR c.category_name = @cat) AND (@typ = '' OR ct.type_name = @typ) " &
                              "ORDER BY t.transaction_id DESC, ti.transaction_item_id"

            Dim names As String() = {"@s", "@f", "@t", "@cat", "@typ"}
            Dim values As Object() = {"%" & searchText & "%", dtfrom.Value.Date, dtto.Value.Date, SelectedCategory(), SelectedType()}

            Dim dt As DataTable = pg.LoadPage(query, names, values)

            dgvtransaction.SuspendLayout()
            dgvtransaction.Rows.Clear()
            For Each r As DataRow In dt.Rows
                ' same column order as before
                dgvtransaction.Rows.Add(
                    r("transaction_no").ToString(),
                    r("buyer_name").ToString(),
                    r("buyer_name").ToString(),
                    If(IsDBNull(r("id_number")) OrElse r("id_number").ToString() = "", "N/A", r("id_number").ToString()),
                    r("buyer_type").ToString(),
                    r("product_code").ToString(),
                    r("product_code").ToString(),
                    r("product_name").ToString(),
                    r("size").ToString(),
                    Convert.ToDecimal(r("unit_price")).ToString("N2"),
                    Convert.ToDecimal(r("subtotal")).ToString("N2"),
                    r("qty").ToString(),
                    Convert.ToDecimal(r("total_amount")).ToString("N2"),
                    Convert.ToDateTime(r("tdate")).ToString("yyyy-MM-dd"),
                    r("ttime").ToString(),
                    Convert.ToDecimal(r("amount_paid")).ToString("N2"),
                    Convert.ToDecimal(r("amount_change")).ToString("N2"),
                    r("payment_method").ToString(),
                    r("status").ToString(),
                    r("username").ToString())
            Next
            dgvtransaction.ResumeLayout()

            ' total sales covers ALL pages, not just the one on screen
            Dim totalSum As Object
            If SelectedCategory() = "" AndAlso SelectedType() = "" Then
                totalSum = ExecScalar(
                "SELECT IFNULL(SUM(t.total_amount),0) FROM TBL_TRANSACTIONS t WHERE " & SEARCH_FILTER &
                "AND DATE(t.created_at) BETWEEN @f AND @t AND t.status <> 'Cancelled'",
                New String() {"@s", "@f", "@t"},
                New Object() {"%" & searchText & "%", dtfrom.Value.Date, dtto.Value.Date})
            Else
                ' category/type chosen: add up only the matching items
                totalSum = ExecScalar(
                "SELECT IFNULL(SUM(ti.subtotal),0) FROM TBL_TRANSACTION_ITEMS ti " &
                "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
                "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                "LEFT JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
                "LEFT JOIN tbl_categories c ON ct.category_id = c.category_id " &
                "WHERE " & SEARCH_FILTER &
                "AND DATE(t.created_at) BETWEEN @f AND @t AND t.status <> 'Cancelled' " &
                "AND (@cat = '' OR c.category_name = @cat) AND (@typ = '' OR ct.type_name = @typ)",
                New String() {"@s", "@f", "@t", "@cat", "@typ"},
                New Object() {"%" & searchText & "%", dtfrom.Value.Date, dtto.Value.Date, SelectedCategory(), SelectedType()})
            End If

            lbltotalsales.Text = ChrW(8369) & Convert.ToDecimal(If(totalSum, 0)).ToString("N2")

        Catch ex As Exception
            MsgBox("Error loading transactions: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub btnviewdetails_Click(sender As Object, e As EventArgs) Handles btnviewdetails.Click
        OpenDetails()
    End Sub

    Private Sub OpenDetails()
        If dgvtransaction.SelectedRows.Count = 0 Then
            MsgBox("Select a transaction row first.", vbExclamation, "Transaction")
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = dgvtransaction.SelectedRows(0)
        If selectedRow.Cells("TransactionNo").Value Is Nothing Then Exit Sub

        Using frm As New frmTransactionDetails()
            frm.TransactionNo = selectedRow.Cells("TransactionNo").Value.ToString()   ' history -> details
            frm.StartPosition = FormStartPosition.CenterParent
            frm.ShowDialog(Me)
        End Using

        LoadGrid(txtSearch.Text.Trim())   ' refresh: a return/exchange may have changed the status
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click   ' Cancel Transaction
        If dgvtransaction.SelectedRows.Count = 0 Then
            MsgBox("Select a transaction row first.", vbExclamation, "Transaction")
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = dgvtransaction.SelectedRows(0)
        Dim txnNo As String = selectedRow.Cells("TransactionNo").Value.ToString()
        Dim currentStatus As String = selectedRow.Cells("Status").Value.ToString()

        If currentStatus = "Cancelled" Then
            MsgBox("This transaction is already cancelled.", vbInformation, "Transaction")
            Exit Sub
        End If
        If currentStatus <> "Completed" Then
            MsgBox("Only 'Completed' transactions can be cancelled. This one is '" & currentStatus & "'.", vbInformation, "Transaction")
            Exit Sub
        End If
        Dim reason As String = InputBox("Reason for cancelling transaction " & txnNo & ":", "Cancel Transaction")
        If String.IsNullOrWhiteSpace(reason) Then Exit Sub

        If MsgBox("Cancel " & txnNo & "? Stock quantities will be restored.", vbYesNo + vbQuestion, "Cancel Transaction") <> MsgBoxResult.Yes Then Exit Sub

        Try
            If Not connection() Then Exit Sub
            Dim trans As MySqlTransaction = cn.BeginTransaction()

            Try
                Dim transactionId As Integer = Convert.ToInt32(If(ExecScalar("SELECT transaction_id FROM TBL_TRANSACTIONS WHERE transaction_no = @t", New String() {"@t"}, New Object() {txnNo}), 0))

                Dim items As DataTable = GetDataTable(
                    "SELECT ti.variant_id, ti.quantity AS qty " &
                    "FROM TBL_TRANSACTION_ITEMS ti " &
                    "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                    "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id WHERE ti.transaction_id = @id",
                    New String() {"@id"}, New Object() {transactionId})

                For Each r As DataRow In items.Rows
                    Using cmdStock As New MySqlCommand("UPDATE TBL_PRODUCT_VARIANTS SET quantity_on_hand = quantity_on_hand + @q WHERE variant_id = @v", cn, trans)
                        cmdStock.Parameters.AddWithValue("@q", Convert.ToInt32(r("qty")))
                        cmdStock.Parameters.AddWithValue("@v", Convert.ToInt32(r("variant_id")))
                        cmdStock.ExecuteNonQuery()
                    End Using
                Next

                Using cmdCancel As New MySqlCommand("UPDATE TBL_TRANSACTIONS SET status = 'Cancelled', cancel_reason = @r, cancelled_by = @by, cancelled_at = NOW() WHERE transaction_id = @id", cn, trans)
                    cmdCancel.Parameters.AddWithValue("@r", reason.Trim())
                    cmdCancel.Parameters.AddWithValue("@by", currentuser.UserID)
                    cmdCancel.Parameters.AddWithValue("@id", transactionId)
                    cmdCancel.ExecuteNonQuery()
                End Using

                trans.Commit()
                MsgBox("Transaction cancelled successfully and inventory stock restored.", vbInformation, "Transaction")
                cn.Close()
                LogActivity("Cancel Transaction", txnNo, "Cancelled " & txnNo & ". Items: " & TransactionItemsSummary(transactionId) & ". Reason: " & reason.Trim())

                LoadGrid(txtSearch.Text.Trim())

            Catch exTransaction As Exception
                trans.Rollback()
                cn.Close()
                MsgBox("Cancellation failed and was rolled back: " & exTransaction.Message, vbCritical, "Error")
            End Try

        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error cancelling transaction: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub
End Class