Imports MySql.Data.MySqlClient

Public Class frmInventoryDiscrepancies
    Private pg As GridPager
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
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With
        pg = New GridPager(dgvlistproducts, 20)
        AddHandler pg.PageChanged, Sub() LoadHistory()
        isLoading = False
        LoadHistory()
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        pg.Reset()
        LoadHistory()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Not isLoading Then
            pg.Reset()
            LoadHistory()
        End If
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If Not isLoading Then
            pg.Reset()
            LoadHistory()
        End If
    End Sub

    Private Sub LoadHistory()
        If pg Is Nothing Then Exit Sub
        Dim keyword As String = txtSearch.Text.Trim()
        Dim category As String = If(cbocategory.SelectedIndex <= 0, "", cbocategory.Text)

        Dim query As String =
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
        "ORDER BY cnt.count_date DESC, cnt.inventory_count_id DESC, p.product_name"
        Dim names As String() = {"@s", "@cat"}
        Dim values As Object() = {"%" & keyword & "%", category}

        Dim dt As DataTable = pg.LoadPage(query, names, values)

        dgvlistproducts.SuspendLayout()
        dgvlistproducts.Rows.Clear()
        For Each r As DataRow In dt.Rows
            Dim status As String = r("status").ToString()
            Dim diff As Integer = Convert.ToInt32(r("difference"))
            Dim isAdjusted As Boolean = Convert.ToInt32(r("adjusted")) = 1

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
            If status <> "Matched" AndAlso Not isAdjusted Then row.DefaultCellStyle.ForeColor = Color.Firebrick
        Next
        dgvlistproducts.ClearSelection()
        dgvlistproducts.ResumeLayout()

        Dim s As DataTable = GetDataTable(
        "SELECT COUNT(*) AS total, " &
        "IFNULL(SUM(status = 'Matched'), 0) AS matched, " &
        "IFNULL(SUM(status <> 'Matched'), 0) AS with_disc, " &
        "IFNULL(SUM(CASE WHEN status <> 'Matched' AND difference < 0 THEN -difference ELSE 0 END), 0) AS missing_units, " &
        "IFNULL(SUM(CASE WHEN status <> 'Matched' AND difference > 0 THEN difference ELSE 0 END), 0) AS excess_units " &
        "FROM (" & query & ") AS q", names, values)

        If s.Rows.Count > 0 Then
            lbltotalproducts.Text = Convert.ToInt32(s.Rows(0)("total")).ToString("N0")
            lbltotalqproducts.Text = Convert.ToInt32(s.Rows(0)("matched")).ToString("N0")
            lblonhand.Text = Convert.ToInt32(s.Rows(0)("with_disc")).ToString("N0")
            lblcriticallvl.Text = Convert.ToInt32(s.Rows(0)("missing_units")).ToString("N0")
            lbloutofstocks.Text = Convert.ToInt32(s.Rows(0)("excess_units")).ToString("N0")
        End If
    End Sub

End Class