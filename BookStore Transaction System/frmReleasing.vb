Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Drawing.Printing

Public Class frmReleasing

    Private Const ST_RELEASED As String = "Released"
    Private Const ST_READY As String = "Ready for release"

    Private pg As GridPager
    Private isLoading As Boolean = True
    Private WithEvents tmrSearch As New System.Windows.Forms.Timer With {.Interval = 350}
    Private WithEvents slipDoc As New PrintDocument()
    Private ReadOnly slipLines As New List(Of String)

    ' ==================== LOAD ====================
    Private Sub frmReleasing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySearchPlaceholders(Me)
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        With dgvstockinhistory
            .AutoGenerateColumns = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With
        BindCol("TransactionNo", "transaction_no")
        BindCol("ORNo", "or_no")
        BindCol("BuyerName", "buyer_name")
        BindCol("IDNumber", "id_number")
        BindCol("ProductCode", "product_code")
        BindCol("ProductCategory", "category_name")
        BindCol("ProductName", "item_name")
        BindCol("Quantity", "qty_n")
        BindCol("PickUpdate", "pickup_date")
        BindCol("InStock", "in_stock")
        BindCol("Status", "rel_status")
        BindCol("ReleasedBy", "released_by_name")
        BindCol("ReleasedTo", "received_by")
        dgvstockinhistory.Columns("PickUpdate").DefaultCellStyle.Format = "MMM d, yyyy"

        cbocategory.DropDownStyle = ComboBoxStyle.DropDownList
        cbocategory.Items.Clear()
        cbocategory.Items.Add("All Categories")
        Dim cats As DataTable = GetDataTable("SELECT category_name FROM tbl_categories ORDER BY category_name")
        If cats IsNot Nothing Then
            For Each r As DataRow In cats.Rows
                cbocategory.Items.Add(r("category_name").ToString())
            Next
        End If
        cbocategory.SelectedIndex = 0

        DateTimePicker1.Value = Date.Today.AddYears(-1)
        DateTimePicker2.Value = Date.Today
        ApplySchoolYearToPickers(DateTimePicker1, DateTimePicker2)

        btnexportexcel.Enabled = Not RolePermissions.IsViewOnly()      ' "Release Item/s"

        pg = New GridPager(dgvstockinhistory, 20)
        AddHandler pg.PageChanged, Sub() LoadRows()

        isLoading = False
        LoadRows()
    End Sub

    Private Sub frmReleasing_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        tmrSearch.Stop()
    End Sub

    Private Sub BindCol(colName As String, field As String)
        dgvstockinhistory.Columns(colName).DataPropertyName = field
    End Sub

    ' ==================== FILTERS ====================
    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        If isLoading Then Exit Sub
        tmrSearch.Stop()
        tmrSearch.Start()
    End Sub

    Private Sub tmrSearch_Tick(sender As Object, e As EventArgs) Handles tmrSearch.Tick
        tmrSearch.Stop()
        pg.Reset()
        LoadRows()
    End Sub

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        If isLoading Then Exit Sub
        pg.Reset()
        LoadRows()
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click       ' "Load Data"
        If DateTimePicker1.Value.Date > DateTimePicker2.Value.Date Then
            MsgBox("'Date from' cannot be later than 'To'.", vbExclamation, "Releasing")
            Exit Sub
        End If
        pg.Reset()
        LoadRows()
    End Sub

    ' ==================== DATA ====================
    Private Sub LoadRows()
        If isLoading OrElse pg Is Nothing Then Exit Sub

        Dim names As New List(Of String)(New String() {"@d1", "@d2", "@s"})
        Dim values As New List(Of Object)(New Object() {DateTimePicker1.Value.Date, DateTimePicker2.Value.Date,
                                                        "%" & txtsearch.Text.Trim() & "%"})
        Dim catCond As String = ""
        If cbocategory.SelectedIndex > 0 Then
            catCond = "AND c.category_name = @cat "
            names.Add("@cat")
            values.Add(cbocategory.Text)
        End If

        Dim query As String =
            "SELECT * FROM (" &
            "SELECT ti.transaction_item_id, ti.variant_id, ti.quantity AS qty_n, " &
            "t.transaction_no, t.or_no, t.buyer_name, t.id_number, v.product_code, c.category_name, " &
            "CONCAT(p.product_name, IF(v.size IS NULL OR v.size IN ('', 'N/A'), '', CONCAT(' (', v.size, ')'))) AS item_name, " &
            "ti.pickup_date, v.quantity_on_hand AS in_stock, " &
            "CASE WHEN ti.released_at IS NOT NULL THEN 'Released' " &
            "     WHEN v.quantity_on_hand >= ti.quantity THEN 'Ready for release' " &
            "     ELSE 'Waiting for stock' END AS rel_status, " &
            "CONCAT(u.first_name, ' ', u.last_name) AS released_by_name, ti.received_by, ti.released_at " &
            "FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_transactions t ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "INNER JOIN tbl_category_types ct ON p.category_type_id = ct.category_type_id " &
            "INNER JOIN tbl_categories c ON ct.category_id = c.category_id " &
            "LEFT JOIN tbl_users u ON ti.released_by = u.user_id " &
            "WHERE ti.is_backorder = 1 AND t.status <> 'Cancelled' AND t.or_date BETWEEN @d1 AND @d2 " & catCond &
            ") x " &
            "WHERE (x.transaction_no LIKE @s OR x.or_no LIKE @s OR x.buyer_name LIKE @s OR x.item_name LIKE @s " &
            "OR x.product_code LIKE @s OR x.rel_status LIKE @s) " &
            "ORDER BY (x.released_at IS NOT NULL), x.pickup_date, x.transaction_item_id"

        dgvstockinhistory.DataSource = pg.LoadPage(query, names.ToArray(), values.ToArray())
        Label3.Text = pg.TotalRows.ToString("N0")
    End Sub

    ' overdue pick-up = red, already released = grey
    Private Sub dgvstockinhistory_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvstockinhistory.DataBindingComplete
        For Each row As DataGridViewRow In dgvstockinhistory.Rows
            Dim d As DataRowView = TryCast(row.DataBoundItem, DataRowView)
            If d Is Nothing Then Continue For
            Dim st As String = d("rel_status").ToString()
            Dim overdue As Boolean = st <> ST_RELEASED AndAlso Not IsDBNull(d("pickup_date")) AndAlso
                                     Convert.ToDateTime(d("pickup_date")).Date < Date.Today
            row.DefaultCellStyle.ForeColor = If(st = ST_RELEASED, Color.Gray, If(overdue, Color.Firebrick, Color.Black))
        Next
        dgvstockinhistory.ClearSelection()
    End Sub

    Private Function SelectedItems() As List(Of DataRowView)
        Dim list As New List(Of DataRowView)
        For Each r As DataGridViewRow In dgvstockinhistory.SelectedRows
            Dim d As DataRowView = TryCast(r.DataBoundItem, DataRowView)
            If d IsNot Nothing Then list.Add(d)
        Next
        Return list
    End Function

    ' ==================== RELEASE ITEM/S ====================
    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role is view-only and cannot release items.", vbExclamation, "Releasing")
            Exit Sub
        End If

        Dim items As List(Of DataRowView) = SelectedItems()
        If items.Count = 0 Then
            MsgBox("Select the item/s you want to release first.", vbExclamation, "Releasing")
            Exit Sub
        End If

        For Each it As DataRowView In items
            If it("rel_status").ToString() <> ST_READY Then
                MsgBox("Only items marked '" & ST_READY & "' can be released." & vbCrLf &
                       "Unselect the items that are waiting for stock or already released.", vbExclamation, "Releasing")
                Exit Sub
            End If
        Next

        Dim trxNo As String = items(0)("transaction_no").ToString()
        For Each it As DataRowView In items
            If it("transaction_no").ToString() <> trxNo Then
                MsgBox("Release the items of one transaction at a time.", vbExclamation, "Releasing")
                Exit Sub
            End If
        Next

        Dim buyer As String = If(IsDBNull(items(0)("buyer_name")), "", items(0)("buyer_name").ToString())
        Dim receiver As String = InputBox("Who is receiving the item/s?", "Release Item/s", buyer).Trim()
        If receiver = "" Then Exit Sub

        Dim summary As String = ""
        For Each it As DataRowView In items
            summary &= it("qty_n").ToString() & " x " & it("item_name").ToString() & vbCrLf
        Next
        If MsgBox("Release the following to " & receiver & "?" & vbCrLf & vbCrLf & summary & vbCrLf &
                  "Stock on hand will be reduced.", vbYesNo + vbQuestion, "Confirm Release") <> MsgBoxResult.Yes Then Exit Sub

        Dim released As List(Of Integer) = ReleaseItems(items, receiver)
        If released Is Nothing Then Exit Sub

        LoadRows()
        If MsgBox("Item/s released. Print the release slip now?", vbYesNo + vbQuestion, "Releasing") = MsgBoxResult.Yes Then
            PrintSlip(released)
        End If
    End Sub

    Private Function ReleaseItems(items As List(Of DataRowView), receiver As String) As List(Of Integer)
        Dim done As New List(Of Integer)
        Using c As MySqlConnection = NewConnection()
            Try
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    For Each it As DataRowView In items
                        Dim itemId As Integer = Convert.ToInt32(it("transaction_item_id"))
                        Dim variantId As Integer = Convert.ToInt32(it("variant_id"))
                        Dim qty As Integer = Convert.ToInt32(it("qty_n"))
                        Dim trxNo As String = it("transaction_no").ToString()
                        Dim itemName As String = it("item_name").ToString()

                        Dim prev As Integer = 0
                        Using q As New MySqlCommand("SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @v FOR UPDATE", c, tx)
                            q.Parameters.AddWithValue("@v", variantId)
                            Dim o As Object = q.ExecuteScalar()
                            prev = If(o Is Nothing OrElse IsDBNull(o), 0, Convert.ToInt32(o))
                        End Using
                        If prev < qty Then
                            tx.Rollback()
                            MsgBox("Not enough stock for " & itemName & " (on hand: " & prev & ", needed: " & qty & ").", vbExclamation, "Releasing")
                            Return Nothing
                        End If

                        Using q As New MySqlCommand(
                            "UPDATE tbl_transaction_items SET released_at = NOW(), released_by = @by, received_by = @rb " &
                            "WHERE transaction_item_id = @id AND released_at IS NULL", c, tx)
                            q.Parameters.AddWithValue("@by", currentuser.UserID)
                            q.Parameters.AddWithValue("@rb", receiver)
                            q.Parameters.AddWithValue("@id", itemId)
                            If q.ExecuteNonQuery() = 0 Then
                                tx.Rollback()
                                MsgBox(itemName & " was already released.", vbExclamation, "Releasing")
                                LoadRows()
                                Return Nothing
                            End If
                        End Using

                        Using q As New MySqlCommand("UPDATE tbl_product_variants SET quantity_on_hand = quantity_on_hand - @q WHERE variant_id = @v", c, tx)
                            q.Parameters.AddWithValue("@q", qty)
                            q.Parameters.AddWithValue("@v", variantId)
                            q.ExecuteNonQuery()
                        End Using

                        Using q As New MySqlCommand(
                            "INSERT INTO tbl_stock_movements (variant_id, movement_type, quantity, previous_quantity, new_quantity, reference_no, remarks, created_by) " &
                            "VALUES (@v, 'Stock Out', @q, @prev, @new, @ref, @rem, @by)", c, tx)
                            q.Parameters.AddWithValue("@v", variantId)
                            q.Parameters.AddWithValue("@q", qty)
                            q.Parameters.AddWithValue("@prev", prev)
                            q.Parameters.AddWithValue("@new", prev - qty)
                            q.Parameters.AddWithValue("@ref", trxNo)
                            q.Parameters.AddWithValue("@rem", "Pick-up released to " & receiver)
                            q.Parameters.AddWithValue("@by", currentuser.UserID)
                            q.ExecuteNonQuery()
                        End Using

                        done.Add(itemId)
                    Next
                    tx.Commit()
                End Using
            Catch ex As Exception
                MsgBox("Release failed: " & ex.Message, vbCritical, "Releasing")
                Return Nothing
            End Try
        End Using

        LogActivity("Release Item", items(0)("transaction_no").ToString(),
                    "Released " & done.Count & " item(s) to " & receiver)
        Return done
    End Function

    ' ==================== PRINT SLIP ====================
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click        ' "Print Slip"
        Dim ids As New List(Of Integer)
        For Each it As DataRowView In SelectedItems()
            If it("rel_status").ToString() = ST_RELEASED Then ids.Add(Convert.ToInt32(it("transaction_item_id")))
        Next
        If ids.Count = 0 Then
            MsgBox("Select one or more released items to print a slip.", vbExclamation, "Print Slip")
            Exit Sub
        End If
        PrintSlip(ids)
    End Sub

    Private Sub PrintSlip(ids As List(Of Integer))
        If ids Is Nothing OrElse ids.Count = 0 Then Exit Sub

        Dim dt As DataTable = GetDataTable(
            "SELECT t.transaction_no, t.or_no, t.buyer_name, ti.quantity, " &
            "CONCAT(p.product_name, IF(v.size IS NULL OR v.size IN ('', 'N/A'), '', CONCAT(' (', v.size, ')'))) AS item_name, " &
            "ti.released_at, ti.received_by, CONCAT(u.first_name, ' ', u.last_name) AS released_by_name " &
            "FROM tbl_transaction_items ti " &
            "INNER JOIN tbl_transactions t ON ti.transaction_id = t.transaction_id " &
            "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
            "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
            "LEFT JOIN tbl_users u ON ti.released_by = u.user_id " &
            "WHERE ti.released_at IS NOT NULL AND ti.transaction_item_id IN (" & String.Join(",", ids) & ") " &
            "ORDER BY t.transaction_no, ti.transaction_item_id")
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Exit Sub

        Dim first As DataRow = dt.Rows(0)
        slipLines.Clear()
        slipLines.Add("LOA BOOKSTORE")
        slipLines.Add("RELEASE SLIP")
        slipLines.Add("")
        slipLines.Add("Transaction No.: " & first("transaction_no").ToString())
        slipLines.Add("OR No.: " & first("or_no").ToString())
        slipLines.Add("Buyer: " & first("buyer_name").ToString())
        slipLines.Add("Released on: " & Convert.ToDateTime(first("released_at")).ToString("MMM d, yyyy h:mm tt"))
        slipLines.Add("")
        slipLines.Add("Items released:")
        For Each r As DataRow In dt.Rows
            slipLines.Add("   " & r("quantity").ToString() & " x " & r("item_name").ToString())
        Next
        slipLines.Add("")
        slipLines.Add("Released by: " & first("released_by_name").ToString())
        slipLines.Add("Received by: " & first("received_by").ToString())
        slipLines.Add("")
        slipLines.Add("Signature: ______________________________")

        Using dlg As New PrintPreviewDialog()
            dlg.Document = slipDoc
            dlg.WindowState = FormWindowState.Maximized
            dlg.ShowDialog()
        End Using
    End Sub

    Private Sub slipDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles slipDoc.PrintPage
        Dim y As Single = e.MarginBounds.Top
        Using fTitle As New Font("Segoe UI", 14.0F, FontStyle.Bold), fBody As New Font("Segoe UI", 10.0F)
            For i As Integer = 0 To slipLines.Count - 1
                Dim f As Font = If(i < 2, fTitle, fBody)
                e.Graphics.DrawString(slipLines(i), f, Brushes.Black, e.MarginBounds.Left, y)
                y += f.GetHeight(e.Graphics) + 4
            Next
        End Using
        e.HasMorePages = False
    End Sub

End Class