Imports MySql.Data.MySqlClient

Public Class frmInventoryDiscrepancies

    Private isLoading As Boolean = True

    Private Sub frmInventoryDiscrepancies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btngenerate.Text = "Generate"

        cbocategory.DropDownStyle = ComboBoxStyle.DropDownList
        cbocategory.Items.Clear()
        cbocategory.Items.Add("All Categories")
        Dim cats As DataTable = GetDataTable("SELECT category_name FROM tbl_categories ORDER BY category_name")
        For Each r As DataRow In cats.Rows
            cbocategory.Items.Add(r("category_name").ToString())
        Next
        cbocategory.SelectedIndex = 0

        With dgvlistproducts
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With

        isLoading = False
        LoadHistory()
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        LoadHistory()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Not isLoading Then LoadHistory()
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If Not isLoading Then LoadHistory()
    End Sub

    Private Sub LoadHistory()
        Dim keyword As String = txtSearch.Text.Trim()
        Dim category As String = If(cbocategory.SelectedIndex <= 0, "", cbocategory.Text)

        Dim dt As DataTable = GetDataTable(
            "SELECT cnt.count_no, cnt.count_date, p.product_name, v.size, d.system_quantity, d.physical_quantity, " &
            "d.difference, d.adjusted, d.status, d.remarks, CONCAT(u.first_name, ' ', u.last_name) AS prepared_by " &
            "FROM tbl_inventory_count_details d " &
            "INNER JOIN tbl_inventory_counts cnt ON d.inventory_count_id = cnt.inventory_count_id " &
            "INNER JOIN tbl_product_variants v ON d.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories cat ON ct.category_id = cat.category_id " &
            "INNER JOIN tbl_users u ON cnt.prepared_by = u.user_id " &
            "WHERE (p.product_name LIKE @s OR v.product_code LIKE @s OR cnt.count_no LIKE @s) " &
            "AND (@cat = '' OR cat.category_name = @cat) " &
            "ORDER BY cnt.count_date DESC, cnt.inventory_count_id DESC, p.product_name",
            New String() {"@s", "@cat"}, New Object() {"%" & keyword & "%", category})

        dgvlistproducts.Rows.Clear()

        Dim total As Integer = 0, matched As Integer = 0, withDisc As Integer = 0
        Dim missingUnits As Integer = 0, excessUnits As Integer = 0

        For Each r As DataRow In dt.Rows
            Dim status As String = r("status").ToString()
            Dim diff As Integer = Convert.ToInt32(r("difference"))
            Dim isAdjusted As Boolean = Convert.ToInt32(r("adjusted")) = 1

            total += 1
            If status = "Matched" Then
                matched += 1
            Else
                withDisc += 1
                If diff < 0 Then missingUnits += Math.Abs(diff) Else excessUnits += diff
            End If

            Dim idx As Integer = dgvlistproducts.Rows.Add()
            Dim row As DataGridViewRow = dgvlistproducts.Rows(idx)
            row.Cells("CountNo").Value = r("count_no").ToString()
            row.Cells("CountDate").Value = Convert.ToDateTime(r("count_date")).ToString("yyyy-MM-dd")
            row.Cells("ProductName").Value = r("product_name").ToString()
            row.Cells("Size").Value = r("size").ToString()
            row.Cells("SystemQuantity").Value = r("system_quantity").ToString()
            row.Cells("PhysicalQty").Value = r("physical_quantity").ToString()
            row.Cells("Difference").Value = If(diff > 0, "+" & diff, diff.ToString())
            row.Cells("Adjusted").Value = If(status = "Matched", "N/A", If(isAdjusted, "Yes", "No"))
            row.Cells("Status").Value = status
            row.Cells("Remarks").Value = If(IsDBNull(r("remarks")), "", r("remarks").ToString())
            row.Cells("PreparedBy").Value = r("prepared_by").ToString()

            If status <> "Matched" AndAlso Not isAdjusted Then
                row.DefaultCellStyle.ForeColor = Color.Firebrick      ' still waiting for adjustment
            End If
        Next

        dgvlistproducts.ClearSelection()

        lbltotalproducts.Text = total.ToString("N0")        ' Total Items Counted
        lbltotalqproducts.Text = matched.ToString("N0")     ' Matched Items
        lblonhand.Text = withDisc.ToString("N0")            ' With Discrepancies
        lblcriticallvl.Text = missingUnits.ToString("N0")   ' Total Missing Items (units)
        lbloutofstocks.Text = excessUnits.ToString("N0")    ' Total Excess Items (units)
    End Sub

End Class