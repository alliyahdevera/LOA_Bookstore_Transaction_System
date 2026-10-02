Imports MySql.Data.MySqlClient

Public Class frmSalesByItem

    Private ReadOnly Peso As String = ChrW(8369)

    Private Sub frmSalesByItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        dtfrom.Value = New Date(Date.Today.Year, Date.Today.Month, 1)
        dtto.Value = Date.Today

        Label9.Text = "Total Quantity"
        dgvsalesreport.Columns("AmountPaid").HeaderText = "Last Sold"
        dgvsalesreport.AllowUserToAddRows = False
        dgvsalesreport.AllowUserToDeleteRows = False
        dgvsalesreport.ReadOnly = True

        btnexportexcel.Visible = (currentuser.Role = ROLE_SUPERVISOR OrElse currentuser.Role = ROLE_MANAGEMENT)

        LoadGrid()
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        If dtfrom.Value.Date > dtto.Value.Date Then
            MsgBox("'Date from' cannot be later than 'To'.", vbExclamation, "Sales by Item")
            Exit Sub
        End If
        LoadGrid()
        If dgvsalesreport.Rows.Count = 0 Then
            MsgBox("No sales found for the selected dates.", vbInformation, "Sales by Item")
        End If
    End Sub

    Private Sub LoadGrid()
        Dim dt As DataTable = GetDataTable(
            "SELECT v.product_code, p.product_name, v.size, c.category_name, " &
            "SUM(ti.quantity) AS qty, SUM(ti.subtotal) AS amt, MAX(t.or_date) AS last_sold " &
            "FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_transactions t ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "WHERE t.or_date BETWEEN @d1 AND @d2 AND t.status <> 'Cancelled' " &
            "GROUP BY v.variant_id, v.product_code, p.product_name, v.size, c.category_name " &
            "ORDER BY amt DESC",
            New String() {"@d1", "@d2"}, New Object() {dtfrom.Value.Date, dtto.Value.Date})

        dgvsalesreport.Rows.Clear()

        Dim totalQty As Integer = 0
        Dim totalSales As Decimal = 0D

        For Each r As DataRow In dt.Rows
            Dim qty As Integer = Convert.ToInt32(r("qty"))
            Dim amt As Decimal = Convert.ToDecimal(r("amt"))
            totalQty += qty
            totalSales += amt

            Dim size As String = r("size").ToString()
            Dim name As String = r("product_name").ToString() & If(size <> "" AndAlso size <> "N/A", " (" & size & ")", "")

            Dim idx As Integer = dgvsalesreport.Rows.Add()
            Dim row As DataGridViewRow = dgvsalesreport.Rows(idx)
            row.Cells("ProductCode").Value = r("product_code").ToString()
            row.Cells("ProductName").Value = name
            row.Cells("Category").Value = r("category_name").ToString()
            row.Cells("Quantity").Value = qty
            row.Cells("TotalSales").Value = amt.ToString("N2")
            row.Cells("AmountPaid").Value = Convert.ToDateTime(r("last_sold")).ToString("yyyy-MM-dd")
        Next

        dgvsalesreport.ClearSelection()
        Label3.Text = Peso & totalSales.ToString("N2")     ' Total Sales
        Label8.Text = totalQty.ToString("N0")              ' Total Quantity
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        ExportGridToCsv(dgvsalesreport, "SalesByItem")
    End Sub

End Class