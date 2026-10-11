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
    Friend WithEvents cbocategory As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents btnexportexcel As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label10 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Panel13 As Panel
    Friend WithEvents lblname As Label
    Friend WithEvents lbldatetime As Label
    Friend WithEvents Label29 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents lblposition As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents dgvstockinhistory As DataGridView
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents btngenerate As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents DateTimePicker2 As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents Label14 As Label
    Friend WithEvents TransactionNo As DataGridViewTextBoxColumn
    Friend WithEvents ORNo As DataGridViewTextBoxColumn
    Friend WithEvents BuyerName As DataGridViewTextBoxColumn
    Friend WithEvents IDNumber As DataGridViewTextBoxColumn
    Friend WithEvents ProductCode As DataGridViewTextBoxColumn
    Friend WithEvents ProductCategory As DataGridViewTextBoxColumn
    Friend WithEvents ProductName As DataGridViewTextBoxColumn
    Friend WithEvents Quantity As DataGridViewTextBoxColumn
    Friend WithEvents PickUpdate As DataGridViewTextBoxColumn
    Friend WithEvents InStock As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn
    Friend WithEvents ReleasedBy As DataGridViewTextBoxColumn
    Friend WithEvents ReleasedTo As DataGridViewTextBoxColumn
    Friend WithEvents Button1 As Button
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

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReleasing))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cbocategory = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnexportexcel = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.lblname = New System.Windows.Forms.Label()
        Me.lbldatetime = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.lblposition = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.dgvstockinhistory = New System.Windows.Forms.DataGridView()
        Me.TransactionNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ORNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BuyerName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IDNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProductCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProductCategory = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ProductName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Quantity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PickUpdate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.InStock = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReleasedBy = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ReleasedTo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btngenerate = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel13.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.dgvstockinhistory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel6.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbocategory
        '
        Me.cbocategory.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbocategory.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbocategory.FormattingEnabled = True
        Me.cbocategory.Location = New System.Drawing.Point(1033, 79)
        Me.cbocategory.Name = "cbocategory"
        Me.cbocategory.Size = New System.Drawing.Size(158, 25)
        Me.cbocategory.TabIndex = 146
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(963, 82)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 17)
        Me.Label9.TabIndex = 145
        Me.Label9.Text = "Category"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(104, 738)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(16, 21)
        Me.Label3.TabIndex = 144
        Me.Label3.Text = "-"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(16, 738)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 21)
        Me.Label4.TabIndex = 143
        Me.Label4.Text = "Total Sales"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label5.Location = New System.Drawing.Point(21, 45)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(464, 15)
        Me.Label5.TabIndex = 142
        Me.Label5.Text = "Paid items that were out of stock at the time of purchase, and their release to t" &
    "he buyer"
        '
        'btnexportexcel
        '
        Me.btnexportexcel.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnexportexcel.FlatAppearance.BorderSize = 0
        Me.btnexportexcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnexportexcel.ForeColor = System.Drawing.Color.White
        Me.btnexportexcel.Location = New System.Drawing.Point(920, 738)
        Me.btnexportexcel.Name = "btnexportexcel"
        Me.btnexportexcel.Size = New System.Drawing.Size(130, 31)
        Me.btnexportexcel.TabIndex = 131
        Me.btnexportexcel.Text = "Release Item/s"
        Me.btnexportexcel.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Location = New System.Drawing.Point(94, 78)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(263, 26)
        Me.Panel1.TabIndex = 141
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label10.Location = New System.Drawing.Point(3, 3)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(191, 17)
        Me.Label10.TabIndex = 131
        Me.Label10.Text = "Transaction No. or Buyer Name"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(239, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(19, 19)
        Me.PictureBox1.TabIndex = 29
        Me.PictureBox1.TabStop = False
        '
        'Panel13
        '
        Me.Panel13.BackColor = System.Drawing.Color.White
        Me.Panel13.Controls.Add(Me.lblname)
        Me.Panel13.Controls.Add(Me.lbldatetime)
        Me.Panel13.Controls.Add(Me.Label29)
        Me.Panel13.Controls.Add(Me.Label27)
        Me.Panel13.Controls.Add(Me.Label28)
        Me.Panel13.Controls.Add(Me.lblposition)
        Me.Panel13.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel13.Location = New System.Drawing.Point(0, 781)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(1220, 27)
        Me.Panel13.TabIndex = 140
        '
        'lblname
        '
        Me.lblname.AutoSize = True
        Me.lblname.BackColor = System.Drawing.Color.Transparent
        Me.lblname.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblname.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lblname.Location = New System.Drawing.Point(54, 3)
        Me.lblname.Name = "lblname"
        Me.lblname.Size = New System.Drawing.Size(53, 21)
        Me.lblname.TabIndex = 64
        Me.lblname.Text = "Name"
        '
        'lbldatetime
        '
        Me.lbldatetime.AutoSize = True
        Me.lbldatetime.BackColor = System.Drawing.Color.Transparent
        Me.lbldatetime.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbldatetime.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lbldatetime.Location = New System.Drawing.Point(576, 3)
        Me.lbldatetime.Name = "lbldatetime"
        Me.lbldatetime.Size = New System.Drawing.Size(16, 21)
        Me.lbldatetime.TabIndex = 68
        Me.lbldatetime.Text = "-"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.Transparent
        Me.Label29.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label29.Location = New System.Drawing.Point(4, 3)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(55, 21)
        Me.Label29.TabIndex = 63
        Me.Label29.Text = "Name:"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.Color.Transparent
        Me.Label27.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label27.Location = New System.Drawing.Point(504, 3)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(65, 21)
        Me.Label27.TabIndex = 67
        Me.Label27.Text = "Today is"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label28.Location = New System.Drawing.Point(249, 3)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(68, 21)
        Me.Label28.TabIndex = 65
        Me.Label28.Text = "Position:"
        '
        'lblposition
        '
        Me.lblposition.AutoSize = True
        Me.lblposition.BackColor = System.Drawing.Color.Transparent
        Me.lblposition.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblposition.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.lblposition.Location = New System.Drawing.Point(315, 3)
        Me.lblposition.Name = "lblposition"
        Me.lblposition.Size = New System.Drawing.Size(82, 21)
        Me.lblposition.TabIndex = 66
        Me.lblposition.Text = "NPosition"
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.dgvstockinhistory)
        Me.Panel5.Controls.Add(Me.Panel6)
        Me.Panel5.Location = New System.Drawing.Point(20, 116)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1171, 610)
        Me.Panel5.TabIndex = 139
        '
        'dgvstockinhistory
        '
        Me.dgvstockinhistory.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvstockinhistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvstockinhistory.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TransactionNo, Me.ORNo, Me.BuyerName, Me.IDNumber, Me.ProductCode, Me.ProductCategory, Me.ProductName, Me.Quantity, Me.PickUpdate, Me.InStock, Me.Status, Me.ReleasedBy, Me.ReleasedTo})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvstockinhistory.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvstockinhistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvstockinhistory.Location = New System.Drawing.Point(0, 35)
        Me.dgvstockinhistory.Name = "dgvstockinhistory"
        Me.dgvstockinhistory.ReadOnly = True
        Me.dgvstockinhistory.RowHeadersVisible = False
        Me.dgvstockinhistory.Size = New System.Drawing.Size(1169, 573)
        Me.dgvstockinhistory.TabIndex = 1
        '
        'TransactionNo
        '
        Me.TransactionNo.HeaderText = "Transaction No."
        Me.TransactionNo.Name = "TransactionNo"
        Me.TransactionNo.ReadOnly = True
        Me.TransactionNo.Width = 110
        '
        'ORNo
        '
        Me.ORNo.HeaderText = "OR No."
        Me.ORNo.Name = "ORNo"
        Me.ORNo.ReadOnly = True
        '
        'BuyerName
        '
        Me.BuyerName.HeaderText = "Buyer Name"
        Me.BuyerName.Name = "BuyerName"
        Me.BuyerName.ReadOnly = True
        '
        'IDNumber
        '
        Me.IDNumber.HeaderText = "ID No."
        Me.IDNumber.Name = "IDNumber"
        Me.IDNumber.ReadOnly = True
        '
        'ProductCode
        '
        Me.ProductCode.HeaderText = "Product Code"
        Me.ProductCode.Name = "ProductCode"
        Me.ProductCode.ReadOnly = True
        '
        'ProductCategory
        '
        Me.ProductCategory.HeaderText = "Product Category"
        Me.ProductCategory.Name = "ProductCategory"
        Me.ProductCategory.ReadOnly = True
        '
        'ProductName
        '
        Me.ProductName.HeaderText = "Product Name"
        Me.ProductName.Name = "ProductName"
        Me.ProductName.ReadOnly = True
        '
        'Quantity
        '
        Me.Quantity.HeaderText = "Quantity"
        Me.Quantity.Name = "Quantity"
        Me.Quantity.ReadOnly = True
        '
        'PickUpdate
        '
        Me.PickUpdate.HeaderText = "PickUp Date"
        Me.PickUpdate.Name = "PickUpdate"
        Me.PickUpdate.ReadOnly = True
        '
        'InStock
        '
        Me.InStock.HeaderText = "In Stock"
        Me.InStock.Name = "InStock"
        Me.InStock.ReadOnly = True
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        '
        'ReleasedBy
        '
        Me.ReleasedBy.HeaderText = "Released By"
        Me.ReleasedBy.Name = "ReleasedBy"
        Me.ReleasedBy.ReadOnly = True
        '
        'ReleasedTo
        '
        Me.ReleasedTo.HeaderText = "Released To"
        Me.ReleasedTo.Name = "ReleasedTo"
        Me.ReleasedTo.ReadOnly = True
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Panel6.Controls.Add(Me.Label7)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel6.Location = New System.Drawing.Point(0, 0)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1169, 35)
        Me.Panel6.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(7, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(225, 21)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "TRANSACTION FOR RELEASE"
        '
        'btngenerate
        '
        Me.btngenerate.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btngenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btngenerate.ForeColor = System.Drawing.Color.White
        Me.btngenerate.Location = New System.Drawing.Point(1081, 19)
        Me.btngenerate.Name = "btngenerate"
        Me.btngenerate.Size = New System.Drawing.Size(110, 28)
        Me.btngenerate.TabIndex = 138
        Me.btngenerate.Text = "Load Data"
        Me.btngenerate.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(18, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(257, 32)
        Me.Label6.TabIndex = 137
        Me.Label6.Text = "RELEASING OF ITEMS"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(917, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(22, 17)
        Me.Label2.TabIndex = 136
        Me.Label2.Text = "To"
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker2.Location = New System.Drawing.Point(945, 19)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(120, 27)
        Me.DateTimePicker2.TabIndex = 135
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(713, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 17)
        Me.Label1.TabIndex = 134
        Me.Label1.Text = "Date from"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(786, 19)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(120, 27)
        Me.DateTimePicker1.TabIndex = 133
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(20, 82)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(51, 17)
        Me.Label14.TabIndex = 132
        Me.Label14.Text = "Search "
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(1, Byte), Integer), CType(CType(21, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(1061, 738)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(130, 31)
        Me.Button1.TabIndex = 147
        Me.Button1.Text = "Print Slip"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'frmReleasing
        '
        Me.ClientSize = New System.Drawing.Size(1220, 808)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.cbocategory)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnexportexcel)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel13)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.btngenerate)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.Label14)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmReleasing"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel13.ResumeLayout(False)
        Me.Panel13.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        CType(Me.dgvstockinhistory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
End Class