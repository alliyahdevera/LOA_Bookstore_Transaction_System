Imports MySql.Data.MySqlClient

Public Class frmTransactionHistory

    Private Sub frmTransactionHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblname.Text = currentuser.FullName
        lblposition.Text = currentuser.Role
        lbldatetime.Text = "Today is " & DateTime.Now.ToString("dddd, MMMM d, yyyy")

        Button3.Text = "Cancel Transaction"
        Button3.Visible = (currentuser.Role = ROLE_SUPERVISOR)
        LoadGrid("")
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadGrid(searchText As String)
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT t.transaction_no, DATE(t.created_at) AS tdate, TIME(t.created_at) AS ttime, " &
                                  "v.product_code, p.product_name, v.size, p.unit_price, ROUND(ti.subtotal / p.unit_price) AS qty, " &
                                  "ti.subtotal, t.total_amount, t.amount_paid, t.amount_change, t.status, u.username " &
                                  "FROM TBL_TRANSACTION_ITEMS ti " &
                                  "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
                                  "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "INNER JOIN TBL_USERS u ON t.created_by = u.user_id " &
                                  "WHERE t.transaction_no LIKE @s ORDER BY t.transaction_id DESC"

            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@s", "%" & searchText & "%")
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    DataGridView1.Rows.Clear()
                    While localDr.Read()
                        DataGridView1.Rows.Add(
                            localDr("transaction_no").ToString(),
                            Convert.ToDateTime(localDr("tdate")).ToString("yyyy-MM-dd"),
                            localDr("ttime").ToString(),
                            localDr("product_code").ToString(),
                            localDr("product_name").ToString(),
                            localDr("size").ToString(),
                            Convert.ToDecimal(localDr("unit_price")).ToString("N2"),
                            localDr("qty").ToString(),
                            Convert.ToDecimal(localDr("subtotal")).ToString("N2"),
                            Convert.ToDecimal(localDr("total_amount")).ToString("N2"),
                            Convert.ToDecimal(localDr("amount_paid")).ToString("N2"),
                            Convert.ToDecimal(localDr("amount_change")).ToString("N2"),
                            localDr("status").ToString(),
                            localDr("username").ToString())
                    End While
                End Using
            End Using
            cn.Close()

            Dim totalSum As Object = ExecScalar(
                "SELECT IFNULL(SUM(total_amount),0) FROM TBL_TRANSACTIONS WHERE transaction_no LIKE @s AND status <> 'Cancelled'",
                New String() {"@s"}, New Object() {"%" & searchText & "%"})

            Label1.Text = ChrW(8369) & Convert.ToDecimal(If(totalSum, 0)).ToString("N2")

        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading transactions: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click   ' View Details
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Select a transaction row first.", vbExclamation, "Transaction")
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
        If selectedRow.Cells("TransactionNo").Value Is Nothing Then Exit Sub

        Dim txnNo As String = selectedRow.Cells("TransactionNo").Value.ToString()
        Dim dt As DataTable = GetDataTable(
            "SELECT p.product_name, v.size, ROUND(ti.subtotal/p.unit_price) AS qty, p.unit_price, ti.subtotal " &
            "FROM TBL_TRANSACTION_ITEMS ti " &
            "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
            "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
            "WHERE t.transaction_no = @t", New String() {"@t"}, New Object() {txnNo})

        Dim sb As New Text.StringBuilder()
        sb.AppendLine("Transaction: " & txnNo)
        sb.AppendLine("---------------------------------")
        For Each r As DataRow In dt.Rows
            sb.AppendLine(String.Format("{0} ({1}) x{2} @ {3:N2} = {4:N2}", r("product_name"), r("size"), r("qty"), r("unit_price"), r("subtotal")))
        Next
        MsgBox(sb.ToString(), vbInformation, "Transaction Details")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click   ' Cancel Transaction
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Select a transaction row first.", vbExclamation, "Transaction")
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
        Dim txnNo As String = selectedRow.Cells("TransactionNo").Value.ToString()
        Dim currentStatus As String = selectedRow.Cells("Status").Value.ToString()

        If currentStatus = "Cancelled" Then
            MsgBox("This transaction is already cancelled.", vbInformation, "Transaction")
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
                    "SELECT ti.variant_id, ROUND(ti.subtotal / p.unit_price) AS qty " &
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

                Using cmdCancel As New MySqlCommand("UPDATE TBL_TRANSACTIONS SET status = 'Cancelled', cancel_reason = @r WHERE transaction_id = @id", cn, trans)
                    cmdCancel.Parameters.AddWithValue("@r", reason.Trim())
                    cmdCancel.Parameters.AddWithValue("@id", transactionId)
                    cmdCancel.ExecuteNonQuery()
                End Using

                trans.Commit()
                MsgBox("Transaction cancelled successfully and inventory stock restored.", vbInformation, "Transaction")
                cn.Close()

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

    Private Sub SetControlText(parent As Control, controlName As String, textValue As String)
        For Each ctrl As Control In parent.Controls
            If String.Equals(ctrl.Name, controlName, StringComparison.OrdinalIgnoreCase) Then
                ctrl.Text = textValue
            End If
            If ctrl.HasChildren Then
                SetControlText(ctrl, controlName, textValue)
            End If
        Next
    End Sub

End Class