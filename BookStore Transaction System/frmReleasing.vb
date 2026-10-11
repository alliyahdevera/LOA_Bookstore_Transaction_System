Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing

Public Class frmReleasing
    Inherits Form

    Private Class ItemInfo
        Public ItemId As Integer
        Public VariantId As Integer
        Public Qty As Integer
        Public Stock As Integer
        Public TxnNo As String = ""
        Public OrNo As String = ""
        Public IdNumber As String = ""
        Public BuyerName As String = ""
        Public ProductName As String = ""
        Public PickupDate As String = ""
        Public Released As Boolean
        Public ReleasedBy As String = ""
        Public ReceivedBy As String = ""
        Public ReleasedAt As String = ""
    End Class

    Private Const PAGE_SIZE As Integer = 20
    Private ReadOnly navy As Color = Color.FromArgb(1, 21, 78)
    Private ReadOnly dgv As New DataGridView
    Private ReadOnly txtSearch As New TextBox
    Private ReadOnly cboStatus As New ComboBox
    Private ReadOnly btnRelease As New Button
    Private ReadOnly btnSlip As New Button
    Private ReadOnly btnRefresh As New Button
    Private ReadOnly lblSummary As New Label
    Private WithEvents tmrSearch As New System.Windows.Forms.Timer With {.Interval = 400}
    Private pg As GridPager
    Private isLoading As Boolean = True

    ' slip printing
    Private slip As ItemInfo
    Private slipTitle As String = ""
    Private slipY As Single

    Public Sub New()
        MyBase.New()
        Me.Text = "Releasing"
        Me.FormBorderStyle = FormBorderStyle.None
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White
        Me.Font = New Font("Segoe UI", 9.75!)
        BuildUi()
    End Sub

    Private Sub BuildUi()
        With dgv
            .Dock = DockStyle.Fill
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False
            .BackgroundColor = Color.White
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .Columns.Add("TransactionNo", "Transaction No.")
            .Columns.Add("OrNo", "OR No.")
            .Columns.Add("BuyerName", "Buyer")
            .Columns.Add("IdNumber", "ID Number")
            .Columns.Add("ProductCode", "Code")
            .Columns.Add("ProductName", "Book / Module")
            .Columns.Add("Qty", "Qty")
            .Columns.Add("PickupDate", "Pick-up Date")
            .Columns.Add("InStock", "In Stock")
            .Columns.Add("Status", "Status")
            .Columns.Add("ReleasedBy", "Released By")
            .Columns.Add("ReleasedTo", "Released To")
            .Columns("TransactionNo").FillWeight = 110
            .Columns("OrNo").FillWeight = 100
            .Columns("BuyerName").FillWeight = 110
            .Columns("IdNumber").FillWeight = 70
            .Columns("ProductCode").FillWeight = 70
            .Columns("ProductName").FillWeight = 150
            .Columns("Qty").FillWeight = 40
            .Columns("PickupDate").FillWeight = 85
            .Columns("InStock").FillWeight = 55
            .Columns("Status").FillWeight = 150
            .Columns("ReleasedBy").FillWeight = 100
            .Columns("ReleasedTo").FillWeight = 100
        End With
        Controls.Add(dgv)

        Dim bottom As New Panel With {.Dock = DockStyle.Bottom, .Height = 54, .BackColor = Color.White}
        btnRelease.Text = "Release Selected Item"
        btnRelease.SetBounds(20, 9, 190, 36)
        btnRelease.FlatStyle = FlatStyle.Flat
        btnRelease.BackColor = navy
        btnRelease.ForeColor = Color.White
        btnRelease.Cursor = Cursors.Hand
        btnSlip.Text = "Print Slip"
        btnSlip.SetBounds(220, 9, 120, 36)
        btnSlip.FlatStyle = FlatStyle.Flat
        btnSlip.Cursor = Cursors.Hand
        lblSummary.AutoSize = True
        lblSummary.Location = New Point(360, 17)
        lblSummary.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)
        lblSummary.ForeColor = navy
        bottom.Controls.Add(btnRelease)
        bottom.Controls.Add(btnSlip)
        bottom.Controls.Add(lblSummary)
        Controls.Add(bottom)

        Dim bar As New Panel With {.Dock = DockStyle.Top, .Height = 46, .BackColor = Color.White}
        bar.Controls.Add(New Label With {.Text = "Search", .AutoSize = True, .Location = New Point(20, 13)})
        txtSearch.SetBounds(78, 9, 250, 27)
        bar.Controls.Add(txtSearch)
        bar.Controls.Add(New Label With {.Text = "Show", .AutoSize = True, .Location = New Point(345, 13)})
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList
        cboStatus.Items.AddRange(New Object() {"Claimable now (stock available)", "Waiting for stock", "Overdue",
                                               "All not yet released", "Released", "All"})
        cboStatus.SelectedIndex = 0
        cboStatus.SetBounds(390, 9, 250, 27)
        bar.Controls.Add(cboStatus)
        btnRefresh.Text = "Refresh"
        btnRefresh.SetBounds(655, 8, 90, 29)
        btnRefresh.FlatStyle = FlatStyle.Flat
        btnRefresh.Cursor = Cursors.Hand
        bar.Controls.Add(btnRefresh)
        Controls.Add(bar)

        Dim title As New Panel With {.Dock = DockStyle.Top, .Height = 70, .BackColor = Color.White}
        title.Controls.Add(New Label With {.Text = "RELEASING OF BOOKS / MODULES", .AutoSize = True, .ForeColor = navy,
                                           .Font = New Font("Segoe UI", 16.0!, FontStyle.Bold), .Location = New Point(18, 8)})
        title.Controls.Add(New Label With {.Text = "Paid items that were out of stock at the time of purchase, and their release to the buyer.",
                                           .AutoSize = True, .ForeColor = Color.DimGray, .Location = New Point(21, 42)})
        Controls.Add(title)

        AddHandler txtSearch.TextChanged, Sub(s, e)
                                              If isLoading Then Return
                                              tmrSearch.Stop()
                                              tmrSearch.Start()
                                          End Sub
        AddHandler cboStatus.SelectedIndexChanged, Sub(s, e)
                                                       If isLoading Then Return
                                                       pg.Reset()
                                                       LoadItems()
                                                   End Sub
        AddHandler btnRefresh.Click, Sub(s, e)
                                         pg.Reset()
                                         LoadItems()
                                     End Sub
        AddHandler btnRelease.Click, AddressOf BtnRelease_Click
        AddHandler btnSlip.Click, AddressOf BtnSlip_Click
    End Sub

    Private Sub frmReleasing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pg = New GridPager(dgv, PAGE_SIZE)
        AddHandler pg.PageChanged, Sub() LoadItems()
        btnRelease.Enabled = Not RolePermissions.IsViewOnly()
        isLoading = False
        LoadItems()
    End Sub

    Private Sub tmrSearch_Tick(sender As Object, e As EventArgs) Handles tmrSearch.Tick
        tmrSearch.Stop()
        pg.Reset()
        LoadItems()
    End Sub

    Private Const FROM_JOIN As String =
        "FROM tbl_transaction_items ti " &
        "INNER JOIN tbl_transactions t ON ti.transaction_id = t.transaction_id " &
        "INNER JOIN tbl_product_variants v ON ti.variant_id = v.variant_id " &
        "INNER JOIN tbl_products p ON v.product_id = p.product_id " &
        "LEFT JOIN tbl_users ru ON ti.released_by = ru.user_id "

    Private Sub LoadItems()
        If pg Is Nothing OrElse isLoading Then Exit Sub

        Dim statusFilter As String
        Select Case cboStatus.SelectedIndex
            Case 0 : statusFilter = "ti.released_at IS NULL AND v.quantity_on_hand >= ti.quantity"
            Case 1 : statusFilter = "ti.released_at IS NULL AND v.quantity_on_hand < ti.quantity"
            Case 2 : statusFilter = "ti.released_at IS NULL AND ti.pickup_date < CURDATE()"
            Case 3 : statusFilter = "ti.released_at IS NULL"
            Case 4 : statusFilter = "ti.released_at IS NOT NULL"
            Case Else : statusFilter = "1 = 1"
        End Select

        Dim query As String =
            "SELECT ti.transaction_item_id, v.variant_id, t.transaction_no, t.or_no, t.buyer_name, " &
            "IFNULL(t.id_number, 'N/A') AS id_number, v.product_code, p.product_name, v.size, " &
            "ti.quantity AS qty, ti.pickup_date, v.quantity_on_hand AS stock, ti.released_at, ti.received_by, " &
            "CONCAT(ru.first_name, ' ', ru.last_name) AS released_by_name, " &
            "DATEDIFF(CURDATE(), ti.pickup_date) AS days_late " &
            FROM_JOIN &
            "WHERE ti.is_backorder = 1 AND t.status <> 'Cancelled' AND " & statusFilter & " " &
            "AND (t.transaction_no LIKE @s OR t.or_no LIKE @s OR t.buyer_name LIKE @s OR t.id_number LIKE @s " &
            "OR v.product_code LIKE @s OR p.product_name LIKE @s) " &
            "ORDER BY ti.pickup_date, ti.transaction_item_id"

        Dim dt As DataTable = pg.LoadPage(query, New String() {"@s"}, New Object() {"%" & txtSearch.Text.Trim() & "%"})

        dgv.SuspendLayout()
        dgv.Rows.Clear()
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                Dim released As Boolean = Not IsDBNull(r("released_at"))
                Dim late As Integer = If(IsDBNull(r("days_late")), 0, Convert.ToInt32(r("days_late")))
                Dim qty As Integer = Convert.ToInt32(r("qty"))
                Dim stock As Integer = Convert.ToInt32(r("stock"))
                Dim sz As String = r("size").ToString()
                Dim pname As String = r("product_name").ToString() & If(sz <> "" AndAlso sz <> "N/A", " (" & sz & ")", "")
                Dim pickup As String = If(IsDBNull(r("pickup_date")), "", Convert.ToDateTime(r("pickup_date")).ToString("MMM d, yyyy"))

                Dim status As String
                Dim rowColor As Color = Color.Black
                If released Then
                    status = "Released " & Convert.ToDateTime(r("released_at")).ToString("MMM d, yyyy h:mm tt")
                    rowColor = Color.Gray
                ElseIf late > 0 Then
                    status = "Overdue " & late & " day(s)" & If(stock >= qty, " - can be claimed", " - waiting for stock")
                    rowColor = Color.Firebrick
                ElseIf stock >= qty Then
                    status = "Can be claimed now"
                    rowColor = Color.SeaGreen
                Else
                    status = "Waiting for stock"
                End If

                Dim idx As Integer = dgv.Rows.Add(
                    r("transaction_no").ToString(), r("or_no").ToString(), r("buyer_name").ToString(), r("id_number").ToString(),
                    r("product_code").ToString(), pname, qty, pickup, stock, status,
                    If(released, r("released_by_name").ToString(), ""),
                    If(released, r("received_by").ToString(), ""))

                dgv.Rows(idx).DefaultCellStyle.ForeColor = rowColor
                dgv.Rows(idx).Tag = New ItemInfo With {
                    .ItemId = Convert.ToInt32(r("transaction_item_id")), .VariantId = Convert.ToInt32(r("variant_id")),
                    .Qty = qty, .Stock = stock, .TxnNo = r("transaction_no").ToString(), .OrNo = r("or_no").ToString(),
                    .IdNumber = r("id_number").ToString(), .BuyerName = r("buyer_name").ToString(),
                    .ProductName = pname, .PickupDate = pickup, .Released = released,
                    .ReleasedBy = If(released, r("released_by_name").ToString(), ""),
                    .ReceivedBy = If(released, r("received_by").ToString(), ""),
                    .ReleasedAt = If(released, Convert.ToDateTime(r("released_at")).ToString("MMM d, yyyy h:mm tt"), "")}
            Next
        End If
        dgv.ClearSelection()
        dgv.ResumeLayout()
        UpdateSummary()
    End Sub

    Private Sub UpdateSummary()
        Dim dt As DataTable = GetDataTable(
            "SELECT COUNT(*) AS n, IFNULL(SUM(v.quantity_on_hand >= ti.quantity), 0) AS rd, " &
            "IFNULL(SUM(ti.pickup_date < CURDATE()), 0) AS od " & FROM_JOIN &
            "WHERE ti.is_backorder = 1 AND ti.released_at IS NULL AND t.status <> 'Cancelled'")
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim n As Integer = Convert.ToInt32(dt.Rows(0)("n"))
            Dim rd As Integer = Convert.ToInt32(dt.Rows(0)("rd"))
            lblSummary.Text = "Can be claimed now: " & rd & "     Waiting for stock: " & (n - rd) &
                              "     Overdue: " & dt.Rows(0)("od").ToString()
        End If
    End Sub

    Private Function SelectedItem() As ItemInfo
        If dgv.CurrentRow Is Nothing OrElse Not dgv.CurrentRow.Selected Then
            MsgBox("Click an item first.", vbExclamation, "Releasing")
            Return Nothing
        End If
        Return TryCast(dgv.CurrentRow.Tag, ItemInfo)
    End Function

    ' ==================== RELEASE ====================
    Private Sub BtnRelease_Click(sender As Object, e As EventArgs)
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "Releasing")
            Exit Sub
        End If
        Dim info As ItemInfo = SelectedItem()
        If info Is Nothing Then Exit Sub

        If info.Released Then
            MsgBox("This item was already released.", vbInformation, "Releasing")
            Exit Sub
        End If
        If info.Stock < info.Qty Then
            MsgBox("There is not enough stock yet. Needed: " & info.Qty & ", on hand: " & info.Stock & "." & vbCrLf &
                   "Record the delivery in Stock In first.", vbExclamation, "Releasing")
            Exit Sub
        End If

        ' who is receiving it
        Dim receiver As String = InputBox("Release " & info.Qty & " x " & info.ProductName & " to (name of the person receiving it):" & vbCrLf &
                                          "Transaction " & info.TxnNo, "Release To", info.BuyerName).Trim()
        If receiver = "" Then Exit Sub

        ' receipt check
        Dim note As String = ""
        If MsgBox("Did the buyer show the receipt (OR No. " & info.OrNo & ")?", vbYesNo + vbQuestion, "Receipt Check") <> MsgBoxResult.Yes Then
            If MsgBox("No receipt. Check a valid ID against the buyer on record:" & vbCrLf &
                      info.BuyerName & "  (ID " & info.IdNumber & ")" & vbCrLf & vbCrLf & "Is the identity verified?",
                      vbYesNo + vbExclamation, "Release Without Receipt") <> MsgBoxResult.Yes Then Exit Sub
            Dim approver As String = RequireSupervisorApproval(Me, "Release without receipt: " & info.TxnNo)
            If approver Is Nothing Then Exit Sub
            note = " (no receipt; ID verified; approved by " & approver & ")"
        End If

        If MsgBox("Release " & info.Qty & " x " & info.ProductName & " to " & receiver & "?" & vbCrLf &
                  "The stock will be deducted now.", vbYesNo + vbQuestion, "Confirm Release") <> MsgBoxResult.Yes Then Exit Sub

        Try
            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        Dim prevQty As Integer
                        Using q As New MySqlCommand("SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @v FOR UPDATE", c, tx)
                            q.Parameters.AddWithValue("@v", info.VariantId)
                            prevQty = Convert.ToInt32(q.ExecuteScalar())
                        End Using
                        If prevQty < info.Qty Then Throw New Exception("Not enough stock on hand (" & prevQty & ").")

                        Using q As New MySqlCommand(
                            "UPDATE tbl_transaction_items SET released_at = NOW(), released_by = @u, received_by = @r " &
                            "WHERE transaction_item_id = @id AND released_at IS NULL", c, tx)
                            q.Parameters.AddWithValue("@u", currentuser.UserID)
                            q.Parameters.AddWithValue("@r", receiver)
                            q.Parameters.AddWithValue("@id", info.ItemId)
                            If q.ExecuteNonQuery() = 0 Then Throw New Exception("This item was already released.")
                        End Using

                        Using q As New MySqlCommand("UPDATE tbl_product_variants SET quantity_on_hand = quantity_on_hand - @q WHERE variant_id = @v", c, tx)
                            q.Parameters.AddWithValue("@q", info.Qty)
                            q.Parameters.AddWithValue("@v", info.VariantId)
                            q.ExecuteNonQuery()
                        End Using

                        Dim remark As String = ("Pick-up released to " & receiver & note)
                        If remark.Length > 255 Then remark = remark.Substring(0, 255)
                        Using q As New MySqlCommand(
                            "INSERT INTO tbl_stock_movements (variant_id, movement_type, quantity, previous_quantity, new_quantity, reference_no, remarks, created_by, created_at) " &
                            "VALUES (@v, 'Stock Out', @q, @p, @n, @ref, @rm, @u, NOW())", c, tx)
                            q.Parameters.AddWithValue("@v", info.VariantId)
                            q.Parameters.AddWithValue("@q", info.Qty)
                            q.Parameters.AddWithValue("@p", prevQty)
                            q.Parameters.AddWithValue("@n", prevQty - info.Qty)
                            q.Parameters.AddWithValue("@ref", info.TxnNo)
                            q.Parameters.AddWithValue("@rm", remark)
                            q.Parameters.AddWithValue("@u", currentuser.UserID)
                            q.ExecuteNonQuery()
                        End Using

                        tx.Commit()
                    Catch
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            LogActivity("Release Item", info.TxnNo, "Released " & info.Qty & " x " & info.ProductName & " to " & receiver & note)

            info.Released = True
            info.ReleasedBy = currentuser.FullName
            info.ReceivedBy = receiver
            info.ReleasedAt = DateTime.Now.ToString("MMM d, yyyy h:mm tt")

            If MsgBox("Item released. Print the release slip?", vbYesNo + vbQuestion, "Releasing") = MsgBoxResult.Yes Then
                PrintSlip(info, "RELEASE SLIP - CLAIMED")
            End If
            LoadItems()

        Catch ex As Exception
            MsgBox("Release failed and was rolled back: " & ex.Message, vbCritical, "Releasing")
        End Try
    End Sub

    ' ==================== SLIPS ====================
    Private Sub BtnSlip_Click(sender As Object, e As EventArgs)
        Dim info As ItemInfo = SelectedItem()
        If info Is Nothing Then Exit Sub

        If info.Released Then
            PrintSlip(info, "RELEASE SLIP - CLAIMED")
        ElseIf info.Stock >= info.Qty Then
            PrintSlip(info, "CLAIM SLIP - READY FOR RELEASE")
        Else
            MsgBox("This item is not available yet, so there is nothing to claim.", vbInformation, "Releasing")
        End If
    End Sub

    Private Sub PrintSlip(info As ItemInfo, title As String)
        slip = info
        slipTitle = title
        Dim pd As New PrintDocument()
        pd.DefaultPageSettings.PaperSize = New PaperSize("Slip", 315, 560)
        pd.DefaultPageSettings.Margins = New Margins(10, 10, 10, 10)
        AddHandler pd.PrintPage, AddressOf SlipPage

        Using dlg As New PrintPreviewDialog()
            dlg.Document = pd
            dlg.WindowState = FormWindowState.Maximized
            dlg.ShowDialog(Me)
        End Using
    End Sub

    Private Sub SlipPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim w As Single = e.PageBounds.Width - 20

        Using fReg As New Font("Consolas", 8), fBold As New Font("Consolas", 9, FontStyle.Bold), fTitle As New Font("Consolas", 12, FontStyle.Bold)
            slipY = 10
            SlipCenter(g, "LOA BOOKSTORE", fTitle, w)
            SlipCenter(g, slipTitle, fBold, w)
            SlipLine(g, w)
            SlipField(g, "Transaction No:", slip.TxnNo, fReg, w)
            SlipField(g, "OR No:", slip.OrNo, fReg, w)
            SlipField(g, "Buyer:", slip.BuyerName, fReg, w)
            SlipField(g, "ID Number:", slip.IdNumber, fReg, w)
            SlipLine(g, w)
            SlipField(g, "Item:", slip.ProductName, fBold, w)
            SlipField(g, "Quantity:", slip.Qty.ToString(), fReg, w)
            SlipField(g, "Claim date was:", slip.PickupDate, fReg, w)

            If slip.Released Then
                SlipLine(g, w)
                SlipField(g, "Released by:", slip.ReleasedBy, fReg, w)
                SlipField(g, "Released to:", slip.ReceivedBy, fReg, w)
                SlipField(g, "Date released:", slip.ReleasedAt, fReg, w)
            Else
                SlipLine(g, w)
                SlipCenter(g, "Present this slip and a valid ID", fReg, w)
                SlipCenter(g, "at the bookstore to claim the item.", fReg, w)
            End If

            slipY += 28
            g.DrawLine(Pens.Black, 10, slipY, 10 + w * 0.9F, slipY)
            slipY += 2
            SlipCenter(g, If(slip.Released, "Received by (signature over printed name)", "Buyer / representative signature"), fReg, w)
            slipY += 28
            g.DrawLine(Pens.Black, 10, slipY, 10 + w * 0.9F, slipY)
            slipY += 2
            SlipCenter(g, "Released by (bookstore staff)", fReg, w)
            slipY += 6
            SlipCenter(g, "Printed " & DateTime.Now.ToString("MMM d, yyyy h:mm tt"), fReg, w)
        End Using
        e.HasMorePages = False
    End Sub

    Private Sub SlipCenter(g As Graphics, text As String, f As Font, w As Single)
        Dim sz As SizeF = g.MeasureString(text, f)
        g.DrawString(text, f, Brushes.Black, 10 + (w - sz.Width) / 2, slipY)
        slipY += sz.Height + 2
    End Sub

    Private Sub SlipField(g As Graphics, label As String, value As String, f As Font, w As Single)
        g.DrawString(label, f, Brushes.Black, 10, slipY)
        slipY += g.MeasureString(label, f).Height
        Dim sz As SizeF = g.MeasureString(value, f, CInt(w - 10))
        g.DrawString(value, f, Brushes.Black, New RectangleF(20, slipY, w - 10, sz.Height))
        slipY += sz.Height + 3
    End Sub

    Private Sub SlipLine(g As Graphics, w As Single)
        g.DrawLine(Pens.Black, 10, slipY + 3, 10 + w, slipY + 3)
        slipY += 9
    End Sub

End Class