' frmStockInHistory.vb
Imports MySql.Data.MySqlClient

Public Class frmStockInHistory

    Private Sub frmStockInHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Text = "Total Quantity"   ' fixes a copy-pasted "Total Sales" label
        DateTimePicker1.Value = New DateTime(Today.Year, Today.Month, 1)
        DateTimePicker2.Value = Today
        LoadGrid()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click   ' Generate
        LoadGrid()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid()
    End Sub

    Private Sub LoadGrid()
        Try
            If Not connection() Then Exit Sub
            Dim searchText As String = txtSearch.Text.Trim()
            Dim query As String = "SELECT si.reference_no, v.product_code, p.product_name, p.product_description, " &
                                  "sid.quantity, si.stock_in_date, si.stock_in_time, si.received_by " &
                                  "FROM TBL_STOCK_IN_DETAILS sid " &
                                  "INNER JOIN TBL_STOCK_INS si ON sid.stock_in_id = si.stock_in_id " &
                                  "INNER JOIN TBL_PRODUCT_VARIANTS v ON sid.variant_id = v.variant_id " &
                                  "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                                  "WHERE si.stock_in_date BETWEEN @d1 AND @d2 AND (si.reference_no LIKE @s OR p.product_name LIKE @s) " &
                                  "ORDER BY si.stock_in_date DESC, si.stock_in_time DESC"
            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@d1", DateTimePicker1.Value.Date)
                localCmd.Parameters.AddWithValue("@d2", DateTimePicker2.Value.Date)
                localCmd.Parameters.AddWithValue("@s", "%" & searchText & "%")
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    DataGridView1.Rows.Clear()
                    Dim totalQty As Integer = 0
                    While localDr.Read()
                        Dim qty As Integer = Convert.ToInt32(localDr("quantity"))
                        totalQty += qty
                        DataGridView1.Rows.Add(
                            localDr("reference_no").ToString(), localDr("product_code").ToString(), localDr("product_name").ToString(),
                            localDr("product_description").ToString(), qty,
                            Convert.ToDateTime(localDr("stock_in_date")).ToString("yyyy-MM-dd"), localDr("stock_in_time").ToString(),
                            If(IsDBNull(localDr("received_by")), "-", localDr("received_by").ToString()))
                    End While
                    Label3.Text = totalQty.ToString("N0")
                End Using
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading stock-in history: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        ExportGridToCsv(DataGridView1, "StockInHistory")
    End Sub

End Class