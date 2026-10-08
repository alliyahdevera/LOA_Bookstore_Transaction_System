Public Class frmSalesDateRange

    Private isFilling As Boolean = False

    Private Sub frmDateReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SetupFooter(Me, lblname, lblposition, lbldatetime)

        dtfrom.Value = New DateTime(Date.Today.Year, Date.Today.Month, 1)
        dtto.Value = Date.Today
        btnexportexcel.Visible = (currentuser.Role = ROLE_SUPERVISOR OrElse currentuser.Role = ROLE_MANAGEMENT)

        isFilling = True
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")
        Dim row As DataRow = dt.NewRow()
        row("category_id") = 0
        row("category_name") = "-- All Categories --"
        dt.Rows.InsertAt(row, 0)
        FillCombo(cbocategory, dt, "category_name", "category_id")
        cbocategory.SelectedIndex = 0
        FillTypeCombo(cbotype, 0, TypeSource.Sales)
        isFilling = False

        LoadGrid()
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isFilling Then Exit Sub
        isFilling = True
        FillTypeCombo(cbotype, SelectedId(cbocategory), TypeSource.Sales)
        isFilling = False
        LoadGrid()
    End Sub

    Private Sub cbotype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbotype.SelectedIndexChanged
        If isFilling Then Exit Sub
        LoadGrid()
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        If dtfrom.Value.Date > dtto.Value.Date Then
            MsgBox("'From' date cannot be later than 'To' date.", vbExclamation, "Sales Report")
            Exit Sub
        End If
        LoadGrid()
    End Sub

    Private Sub LoadGrid()
        Dim catId As Integer = SelectedId(cbocategory)
        Dim typeId As Integer = SelectedId(cbotype)
        Dim names As String() = {"@d1", "@d2", "@cat", "@type"}
        Dim values As Object() = {dtfrom.Value.Date, dtto.Value.Date, catId, typeId}

        Dim dt As DataTable = GetDataTable(
            "SELECT t.transaction_no, t.buyer_name, v.product_code, p.product_name, v.size, p.unit_price, " &
            "ti.quantity AS qty, t.total_amount, t.amount_paid, t.amount_change, t.payment_method, ti.subtotal, " &
            "DATE(t.created_at) AS tdate, TIME(t.created_at) AS ttime, u.username " &
            "FROM TBL_TRANSACTION_ITEMS ti " &
            "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
            "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
            "INNER JOIN TBL_CATEGORY_TYPES ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN TBL_USERS u ON t.created_by = u.user_id " &
            "WHERE DATE(t.created_at) BETWEEN @d1 AND @d2 AND t.status <> 'Cancelled' " &
            "AND (@cat = 0 OR ct.category_id = @cat) AND (@type = 0 OR ct.category_type_id = @type) " &
            "ORDER BY t.transaction_id DESC", names, values)

        Dim sumSubtotal As Decimal = 0D
        DataGridView1.SuspendLayout()
        DataGridView1.Rows.Clear()
        For Each r As DataRow In dt.Rows
            sumSubtotal += Convert.ToDecimal(r("subtotal"))
            DataGridView1.Rows.Add(
                r("transaction_no").ToString(), r("buyer_name").ToString(),
                r("product_code").ToString(), r("product_name").ToString(), r("size").ToString(),
                Convert.ToDecimal(r("unit_price")).ToString("N2"), r("qty").ToString(),
                Convert.ToDecimal(r("total_amount")).ToString("N2"), Convert.ToDecimal(r("amount_paid")).ToString("N2"),
                Convert.ToDecimal(r("amount_change")).ToString("N2"), r("payment_method").ToString(),
                Convert.ToDateTime(r("tdate")).ToString("yyyy-MM-dd"), r("ttime").ToString(), r("username").ToString())
        Next
        DataGridView1.ClearSelection()
        DataGridView1.ResumeLayout()

        Label8.Text = sumSubtotal.ToString("N2")

        If catId = 0 AndAlso typeId = 0 Then
            Label3.Text = ChrW(8369) & Convert.ToDecimal(If(ExecScalar(
                "SELECT IFNULL(SUM(total_amount),0) FROM TBL_TRANSACTIONS WHERE DATE(created_at) BETWEEN @d1 AND @d2 AND status <> 'Cancelled'",
                New String() {"@d1", "@d2"}, New Object() {dtfrom.Value.Date, dtto.Value.Date}), 0)).ToString("N2")
        Else
            Label3.Text = ChrW(8369) & sumSubtotal.ToString("N2")
        End If
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        ExportGridToCsv(DataGridView1, "SalesReport")
    End Sub

End Class