Imports MySql.Data.MySqlClient

Public Class frmTransactionHistory
    Private Const SEARCH_FILTER As String =
    "(t.transaction_no LIKE @s OR t.buyer_name LIKE @s OR t.or_no LIKE @s OR t.status LIKE @s " &
    "OR t.student_id IN (SELECT student_id FROM tbl_students WHERE student_no LIKE @s) " &
    "OR t.transaction_id IN (SELECT ti2.transaction_id FROM tbl_transaction_items ti2 " &
    "INNER JOIN tbl_product_variants v2 ON ti2.variant_id = v2.variant_id " &
    "INNER JOIN tbl_products p2 ON v2.product_id = p2.product_id " &
    "WHERE v2.product_code LIKE @s OR p2.product_name LIKE @s)) "
    Private Sub frmTransactionHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        dtfrom.Value = New Date(Date.Today.Year, Date.Today.Month, 1)   ' 1st of this month
        dtto.Value = Date.Today

        Button3.Text = "Cancel Transaction"
        Button3.Visible = (currentuser.Role = ROLE_SUPERVISOR)
        LoadGrid("")
    End Sub
    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        If dtfrom.Value.Date > dtto.Value.Date Then
            MsgBox("'From' date cannot be later than 'To' date.", vbExclamation, "Transaction History")
            Exit Sub
        End If

        LoadGrid(txtSearch.Text.Trim())

        If dgvtransaction.Rows.Count = 0 Then
            MsgBox("No transactions found for the selected dates.", vbInformation, "Transaction History")
        End If
    End Sub

    Private Sub dgvtransaction_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvtransaction.CellDoubleClick
        If e.RowIndex >= 0 Then OpenDetails()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadGrid(searchText As String)
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT t.transaction_no, t.buyer_name, DATE(t.created_at) AS tdate, TIME(t.created_at) AS ttime, " &
                              "v.product_code, p.product_name, v.size, p.unit_price, ti.quantity AS qty, " &
                              "ti.subtotal, t.total_amount, t.amount_paid, t.amount_change, t.payment_method, t.status, u.username " &
                              "FROM TBL_TRANSACTION_ITEMS ti " &
                              "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
                              "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                              "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                              "INNER JOIN TBL_USERS u ON t.created_by = u.user_id " &
                              "WHERE " & SEARCH_FILTER &
      "AND DATE(t.created_at) BETWEEN @f AND @t " &
      "ORDER BY t.transaction_id DESC"

            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@s", "%" & searchText & "%")
                localCmd.Parameters.AddWithValue("@f", dtfrom.Value.Date)
                localCmd.Parameters.AddWithValue("@t", dtto.Value.Date)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    dgvtransaction.Rows.Clear()
                    While localDr.Read()
                        ' order = TransactionNo, StudentName, ProductCode, ProductName, Size, UnitPrice, SubTotal,
                        '         Quantity, TotalAmount, Date, Time, Am untPaid, AmountChange, Status, ProcessedBy
                        dgvtransaction.Rows.Add(
                        localDr("transaction_no").ToString(),
                        localDr("buyer_name").ToString(),
                        localDr("product_code").ToString(),
                        localDr("product_name").ToString(),
                        localDr("size").ToString(),
                        Convert.ToDecimal(localDr("unit_price")).ToString("N2"),
                        Convert.ToDecimal(localDr("subtotal")).ToString("N2"),
                        localDr("qty").ToString(),
                        Convert.ToDecimal(localDr("total_amount")).ToString("N2"),
                        Convert.ToDateTime(localDr("tdate")).ToString("yyyy-MM-dd"),
                        localDr("ttime").ToString(),
                        Convert.ToDecimal(localDr("amount_paid")).ToString("N2"),
                        Convert.ToDecimal(localDr("amount_change")).ToString("N2"),
                        localDr("payment_method").ToString(),
                        localDr("status").ToString(),
                        localDr("username").ToString())
                    End While
                End Using
            End Using
            cn.Close()

            Dim totalSum As Object = ExecScalar(
            "SELECT IFNULL(SUM(t.total_amount),0) FROM TBL_TRANSACTIONS t WHERE " & SEARCH_FILTER &
"AND DATE(t.created_at) BETWEEN @f AND @t AND t.status <> 'Cancelled'",
            New String() {"@s", "@f", "@t"},
            New Object() {"%" & searchText & "%", dtfrom.Value.Date, dtto.Value.Date})

            lbltotalsales.Text = ChrW(8369) & Convert.ToDecimal(If(totalSum, 0)).ToString("N2")

        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
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