Imports MySql.Data.MySqlClient

Public Class frmSalesDateRange
    Private showReleasedOnly As Boolean = False


    Private Sub Button2_Click(sender As Object, e As EventArgs)     ' Search
        LoadGrid()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)     ' Released Items toggle
        showReleasedOnly = Not showReleasedOnly
        Button3.Text = If(showReleasedOnly, "Show All Items", "Released Items")
        LoadGrid()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        LoadGrid()
    End Sub

    Private Sub LoadGrid()
        Try
            If Not connection() Then Exit Sub
            Dim query As String = "SELECT t.transaction_no, v.product_code, p.product_name, v.size, p.unit_price, " &
                                  "ROUND(ti.subtotal/p.unit_price) AS qty, t.total_amount, t.amount_paid, t.amount_change, ti.subtotal, " &
                                  "DATE(t.created_at) AS tdate, TIME(t.created_at) AS ttime, u.username " &
                                  "FROM TBL_TRANSACTION_ITEMS ti " &
                                  "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
                                  "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "INNER JOIN TBL_USERS u ON t.created_by = u.user_id " &
                                  "WHERE DATE(t.created_at) BETWEEN @d1 AND @d2 AND t.transaction_no LIKE @s "
            If showReleasedOnly Then query &= "AND t.status = 'Completed' "
            query &= "ORDER BY t.transaction_id DESC"

            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@d1", DateTimePicker1.Value.Date)
                localCmd.Parameters.AddWithValue("@d2", DateTimePicker2.Value.Date)
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    DataGridView1.Rows.Clear()
                    Dim sumSubtotal As Decimal = 0
                    While localDr.Read()
                        sumSubtotal += Convert.ToDecimal(localDr("subtotal"))
                        DataGridView1.Rows.Add(
                            localDr("transaction_no").ToString(), localDr("product_code").ToString(), localDr("product_name").ToString(),
                            localDr("size").ToString(), Convert.ToDecimal(localDr("unit_price")).ToString("N2"), localDr("qty").ToString(),
                            Convert.ToDecimal(localDr("total_amount")).ToString("N2"), Convert.ToDecimal(localDr("amount_paid")).ToString("N2"),
                            Convert.ToDecimal(localDr("amount_change")).ToString("N2"), Convert.ToDecimal(localDr("subtotal")).ToString("N2"),
                            Convert.ToDateTime(localDr("tdate")).ToString("yyyy-MM-dd"), localDr("ttime").ToString(), localDr("username").ToString())
                    End While
                    Label8.Text = sumSubtotal.ToString("N2")
                End Using
            End Using
            cn.Close()

            Dim statusFilter As String = If(showReleasedOnly, "AND status = 'Completed'", "")
            Label3.Text = ChrW(8369) & Convert.ToDecimal(If(ExecScalar(
                "SELECT IFNULL(SUM(total_amount),0) FROM TBL_TRANSACTIONS WHERE DATE(created_at) BETWEEN @d1 AND @d2 AND transaction_no LIKE @s AND status <> 'Cancelled' " & statusFilter,
                New String() {"@d1", "@d2", "@s"}, New Object() {DateTimePicker1.Value.Date, DateTimePicker2.Value.Date, "%"}), 0)).ToString("N2")
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading reports: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs)
        ExportGridToCsv(DataGridView1, "SalesReport")
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

    Private Sub frmDateReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblname.Text = currentuser.FullName
        lblposition.Text = currentuser.Role
        lbldatetime.Text = "Today is " & DateTime.Now.ToString("dddd, MMMM d, yyyy")

        DateTimePicker1.Value = New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
        DateTimePicker2.Value = DateTime.Today
        btnexportexcel.Visible = (currentuser.Role = ROLE_SUPERVISOR OrElse currentuser.Role = ROLE_MANAGEMENT)
        LoadGrid()
    End Sub
End Class