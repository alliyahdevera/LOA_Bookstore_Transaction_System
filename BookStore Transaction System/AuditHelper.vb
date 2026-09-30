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

End Module