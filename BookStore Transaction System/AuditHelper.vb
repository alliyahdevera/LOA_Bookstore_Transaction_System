Imports MySql.Data.MySqlClient

Public Module AuditHelper

    Public Sub LogActivity(actionType As String, referenceNo As String, details As String)
        Try
            If Not connection() Then Exit Sub
            Dim q As String = "INSERT INTO tbl_audit_logs (user_id, log_type, action_type, reference_no, details, status, created_at) " &
                              "VALUES (@uid, 'Activity', @act, @ref, @det, 'Success', NOW())"
            Using c As New MySqlCommand(q, cn)
                c.Parameters.AddWithValue("@uid", currentuser.UserID)
                c.Parameters.AddWithValue("@act", actionType)
                c.Parameters.AddWithValue("@ref", If(String.IsNullOrWhiteSpace(referenceNo), CType(DBNull.Value, Object), referenceNo))
                c.Parameters.AddWithValue("@det", If(String.IsNullOrWhiteSpace(details), CType(DBNull.Value, Object), details))
                c.ExecuteNonQuery()
            End Using
            cn.Close()
        Catch
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
    End Sub

    Public Sub LogPriceChange(productCode As String, productName As String, oldPrice As Decimal, newPrice As Decimal, reason As String)
        Try
            If Not connection() Then Exit Sub
            Dim q As String = "INSERT INTO tbl_audit_logs (user_id, log_type, action_type, product_code, product_name, old_price, new_price, reason, status, created_at) " &
                              "VALUES (@uid, 'Price Change', 'Price Change', @pc, @pn, @op, @np, @rs, 'Success', NOW())"
            Using c As New MySqlCommand(q, cn)
                c.Parameters.AddWithValue("@uid", currentuser.UserID)
                c.Parameters.AddWithValue("@pc", productCode)
                c.Parameters.AddWithValue("@pn", productName)
                c.Parameters.AddWithValue("@op", oldPrice)
                c.Parameters.AddWithValue("@np", newPrice)
                c.Parameters.AddWithValue("@rs", reason)
                c.ExecuteNonQuery()
            End Using
            cn.Close()
        Catch
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
    End Sub
    Public Function TransactionItemsSummary(transactionId As Integer) As String
        Dim dt As DataTable = GetDataTable(
            "SELECT p.product_name, v.size, ti.quantity FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "WHERE ti.transaction_id = @id ORDER BY ti.transaction_item_id",
            New String() {"@id"}, New Object() {transactionId})

        Dim parts As New List(Of String)
        For Each r As DataRow In dt.Rows
            Dim sz As String = r("size").ToString()
            parts.Add(r("product_name").ToString() & If(sz <> "" AndAlso sz <> "N/A", " (" & sz & ")", "") & " x" & r("quantity").ToString())
        Next
        Return String.Join(", ", parts)
    End Function
End Module