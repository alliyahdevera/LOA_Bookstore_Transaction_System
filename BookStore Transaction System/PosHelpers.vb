Imports System.Drawing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Module PlaceholderHelper

    Private Declare Unicode Function SendMessage Lib "user32.dll" Alias "SendMessageW" (hWnd As IntPtr, msg As Integer, wParam As Integer, lParam As String) As IntPtr
    Private Const EM_SETCUEBANNER As Integer = &H1501

    ' remembers which search boxes were already set up (so calling this twice is harmless)
    Private ReadOnly handled As New System.Runtime.CompilerServices.ConditionalWeakTable(Of TextBox, Object)()

    Public Sub SetPlaceholder(tb As TextBox, hint As String)
        If tb.IsHandleCreated Then
            SendMessage(tb.Handle, EM_SETCUEBANNER, 0, hint)
        Else
            AddHandler tb.HandleCreated, Sub() SendMessage(tb.Handle, EM_SETCUEBANNER, 0, hint)
        End If
    End Sub
    Public Sub MoveStock(conn As MySqlConnection, tx As MySqlTransaction, variantId As Integer, qtyChange As Integer, movementType As String, refNo As String, remarks As String)
        Dim prevQty As Integer = 0
        Using cmd As New MySqlCommand("SELECT quantity_on_hand FROM tbl_product_variants WHERE variant_id = @v", conn, tx)
            cmd.Parameters.AddWithValue("@v", variantId)
            Dim res = cmd.ExecuteScalar()
            If res IsNot Nothing AndAlso Not DBNull.Value.Equals(res) Then
                prevQty = Convert.ToInt32(res)
            End If
        End Using

        Dim newQty As Integer = prevQty + qtyChange

        Using cmd As New MySqlCommand("UPDATE tbl_product_variants SET quantity_on_hand = @nq WHERE variant_id = @v", conn, tx)
            cmd.Parameters.AddWithValue("@nq", newQty)
            cmd.Parameters.AddWithValue("@v", variantId)
            cmd.ExecuteNonQuery()
        End Using

        Using cmd As New MySqlCommand("INSERT INTO tbl_stock_movements (variant_id, movement_type, quantity, previous_quantity, new_quantity, reference_no, remarks, created_by, created_at) VALUES (@v, @mt, @q, @pq, @nq, @ref, @rm, @uid, NOW())", conn, tx)
            cmd.Parameters.AddWithValue("@v", variantId)
            cmd.Parameters.AddWithValue("@mt", movementType)
            cmd.Parameters.AddWithValue("@q", Math.Abs(qtyChange))
            cmd.Parameters.AddWithValue("@pq", prevQty)
            cmd.Parameters.AddWithValue("@nq", newQty)
            cmd.Parameters.AddWithValue("@ref", refNo)
            cmd.Parameters.AddWithValue("@rm", remarks)
            cmd.Parameters.AddWithValue("@uid", currentuser.UserID)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function ScalarInt(conn As MySqlConnection, tx As MySqlTransaction, query As String, paramValue As Object) As Integer
        Using cmd As New MySqlCommand(query, conn, tx)
            cmd.Parameters.AddWithValue("@t", paramValue)
            Dim res = cmd.ExecuteScalar()
            If res IsNot Nothing AndAlso Not DBNull.Value.Equals(res) Then
                Return Convert.ToInt32(res)
            End If
        End Using
        Return 0
    End Function
    Public Sub ApplySearchPlaceholders(parent As Control)
        For Each c As Control In parent.Controls
            Dim tb As TextBox = TryCast(c, TextBox)
            If tb IsNot Nothing AndAlso Not tb.Multiline AndAlso tb.Name.IndexOf("search", StringComparison.OrdinalIgnoreCase) >= 0 Then
                SetupSearchBox(tb)
            End If
            If c.HasChildren Then ApplySearchPlaceholders(c)
        Next
    End Sub

    Private Sub SetupSearchBox(tb As TextBox)
        Dim dummy As Object = Nothing
        If handled.TryGetValue(tb, dummy) Then Return
        handled.Add(tb, New Object())

        ' find overlay labels: Labels in the same container that sit on top of the textbox
        Dim hints As New List(Of Label)()
        If tb.Parent IsNot Nothing Then
            For Each ctl As Control In tb.Parent.Controls
                Dim lb As Label = TryCast(ctl, Label)
                If lb IsNot Nothing AndAlso lb.Bounds.IntersectsWith(tb.Bounds) Then hints.Add(lb)
            Next
        End If

        ' no overlay label -> use the native placeholder (it hides itself when the box gets focus)
        If hints.Count = 0 Then
            SetPlaceholder(tb, If(tb.Name = "txtProductSearch", "Product code or name", "Search..."))
            Return
        End If

        ' overlay label -> show only when the box is empty AND not focused
        Dim refreshHints As Action =
            Sub()
                Dim show As Boolean = (tb.Text.Length = 0 AndAlso Not tb.Focused)
                For Each lb As Label In hints
                    lb.Visible = show
                Next
            End Sub

        For Each lb As Label In hints
            AddHandler lb.Click, Sub() tb.Focus()      ' clicking the label types into the box
        Next
        AddHandler tb.Enter, Sub() refreshHints()
        AddHandler tb.Leave, Sub() refreshHints()
        AddHandler tb.TextChanged, Sub() refreshHints()
        refreshHints()
    End Sub

End Module

Public Module ApprovalHelper

    ' Returns the approving supervisor's full name, or Nothing if cancelled / not approved.
    Public Function RequireSupervisorApproval(owner As IWin32Window, action As String) As String
        For attempt As Integer = 1 To 3
            Dim user As String = "", pw As String = ""
            If Not PromptCredentials(owner, action, user, pw) Then Return Nothing

            Dim dt As DataTable = GetDataTable(
                "SELECT CONCAT(u.first_name, ' ', u.last_name) AS full_name " &
                "FROM tbl_users u INNER JOIN tbl_roles r ON u.role_id = r.role_id " &
                "WHERE u.username = @u AND u.password = @p AND u.status = 'Active' AND r.role_name = @role",
                New String() {"@u", "@p", "@role"}, New Object() {user, HashPassword(pw), ROLE_SUPERVISOR})

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim name As String = dt.Rows(0)("full_name").ToString()
                LogActivity("Supervisor Approval", "", action & " approved by " & name)
                Return name
            End If
            MsgBox("Invalid supervisor credentials. Attempt " & attempt & " of 3.", vbExclamation, "Supervisor Approval")
        Next
        LogActivity("Supervisor Approval Failed", "", action & ": 3 incorrect approval attempts")
        Return Nothing
    End Function

    Private Function PromptCredentials(owner As IWin32Window, action As String, ByRef user As String, ByRef pw As String) As Boolean
        Using dlg As New Form(), lbl As New Label(), lblU As New Label(), lblP As New Label(),
              txtU As New TextBox(), txtP As New TextBox(), ok As New Button(), cancel As New Button()
            dlg.Text = "Supervisor Approval"
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog
            dlg.StartPosition = FormStartPosition.CenterParent
            dlg.MinimizeBox = False
            dlg.MaximizeBox = False
            dlg.ClientSize = New Size(340, 190)

            lbl.Text = "A supervisor must approve:" & vbCrLf & action
            lbl.SetBounds(12, 10, 316, 40)
            lblU.Text = "Supervisor username"
            lblU.SetBounds(12, 56, 150, 18)
            txtU.SetBounds(12, 76, 316, 23)
            lblP.Text = "Supervisor password"
            lblP.SetBounds(12, 104, 150, 18)
            txtP.UseSystemPasswordChar = True
            txtP.SetBounds(12, 124, 316, 23)
            ok.Text = "Approve"
            ok.DialogResult = DialogResult.OK
            ok.SetBounds(166, 154, 80, 28)
            cancel.Text = "Cancel"
            cancel.DialogResult = DialogResult.Cancel
            cancel.SetBounds(252, 154, 76, 28)

            dlg.AcceptButton = ok
            dlg.CancelButton = cancel
            dlg.Controls.AddRange(New Control() {lbl, lblU, txtU, lblP, txtP, ok, cancel})

            If dlg.ShowDialog(owner) <> DialogResult.OK Then Return False
            user = txtU.Text.Trim()
            pw = txtP.Text
            Return user <> "" AndAlso pw <> ""
        End Using
    End Function

End Module

Public Module InventoryUi

    Public Function SelectedId(cbo As ComboBox) As Integer
        If cbo.SelectedValue IsNot Nothing AndAlso IsNumeric(cbo.SelectedValue) Then Return Convert.ToInt32(cbo.SelectedValue)
        Return 0
    End Function

    ' Which data a type must have to appear in the combo
    Public Enum TypeSource
        Products   ' type has at least one product that has a variant
        StockIns   ' type has at least one stock-in record
        Sales      ' type has at least one sold (non-cancelled) item
    End Enum

    Private Function TypeHasDataSql(source As TypeSource, activeOnly As Boolean) As String
        Select Case source
            Case TypeSource.StockIns
                Return "EXISTS (SELECT 1 FROM TBL_STOCK_IN_DETAILS sid " &
                       "INNER JOIN TBL_PRODUCT_VARIANTS v ON sid.variant_id = v.variant_id " &
                       "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                       "WHERE p.category_type_id = ct.category_type_id)"
            Case TypeSource.Sales
                Return "EXISTS (SELECT 1 FROM TBL_TRANSACTION_ITEMS ti " &
                       "INNER JOIN TBL_TRANSACTIONS t ON ti.transaction_id = t.transaction_id " &
                       "INNER JOIN TBL_PRODUCT_VARIANTS v ON ti.variant_id = v.variant_id " &
                       "INNER JOIN TBL_PRODUCTS p ON v.product_id = p.product_id " &
                       "WHERE p.category_type_id = ct.category_type_id AND t.status <> 'Cancelled')"
            Case Else
                Return "EXISTS (SELECT 1 FROM TBL_PRODUCTS p " &
                       "INNER JOIN TBL_PRODUCT_VARIANTS v ON v.product_id = p.product_id " &
                       "WHERE p.category_type_id = ct.category_type_id" &
                       If(activeOnly, " AND p.status = 'Active'", "") & ")"
        End Select
    End Function

    ' Returns category_type_id + type_name for types that have data (categoryId 0 = all categories)
    Public Function GetTypesWithData(categoryId As Integer, source As TypeSource, activeOnly As Boolean) As DataTable
        Dim sql As String = "SELECT ct.category_type_id, ct.type_name FROM TBL_CATEGORY_TYPES ct WHERE " &
                            TypeHasDataSql(source, activeOnly)
        If categoryId = 0 Then
            Return GetDataTable(sql & " ORDER BY ct.type_name")
        End If
        Return GetDataTable(sql & " AND ct.category_id = @c ORDER BY ct.type_name",
                            New String() {"@c"}, New Object() {categoryId})
    End Function

    ' ID-based type combo (ValueMember = category_type_id). Index 0 is "-- All Types --".
    Public Sub FillTypeCombo(cbo As ComboBox, categoryId As Integer,
                              Optional source As TypeSource = TypeSource.Products,
                              Optional activeOnly As Boolean = False)
        Dim dt As DataTable = GetTypesWithData(categoryId, source, activeOnly)
        Dim row As DataRow = dt.NewRow()
        row("category_type_id") = 0
        row("type_name") = "-- All Types --"
        dt.Rows.InsertAt(row, 0)
        FillCombo(cbo, dt, "type_name", "category_type_id")
        cbo.SelectedIndex = 0
    End Sub

    ' Name-based type combo (Items list) for forms that filter by type name. Index 0 is "All Types".
    ' categoryName "" = all categories.
    Public Sub FillTypeNameCombo(cbo As ComboBox, categoryName As String,
                                 Optional source As TypeSource = TypeSource.Products,
                                 Optional activeOnly As Boolean = False)
        Dim catId As Integer = 0
        If categoryName <> "" Then
            catId = Convert.ToInt32(If(ExecScalar("SELECT category_id FROM TBL_CATEGORIES WHERE category_name = @n",
                                                  New String() {"@n"}, New Object() {categoryName}), 0))
        End If

        cbo.Items.Clear()
        cbo.Items.Add("All Types")

        ' category chosen but not found -> only "All Types"
        If categoryName = "" OrElse catId > 0 Then
            Dim dt As DataTable = GetTypesWithData(catId, source, activeOnly)
            Dim seen As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            For Each r As DataRow In dt.Rows
                Dim n As String = r("type_name").ToString()
                If seen.Add(n) Then cbo.Items.Add(n)   ' avoids duplicate names across categories
            Next
        End If
        cbo.SelectedIndex = 0
    End Sub

End Module

Public Interface IBuyerInfo
    ReadOnly Property BuyerType As String
    ReadOnly Property BuyerName As String
    ReadOnly Property StudentId As Integer
    ReadOnly Property IdNumber As String
    Function ValidateBuyer(ByRef message As String) As Boolean
    Sub ClearBuyer()
End Interface
Public Class GridPager

    Public Event PageChanged As EventHandler

    Private ReadOnly navy As Color = Color.FromArgb(1, 21, 78)

    Private ReadOnly pnl As New Panel()
    Private ReadOnly lblInfo As New Label()
    Private ReadOnly lblRows As New Label()
    Private ReadOnly cboSize As New ComboBox()
    Private ReadOnly btnFirst As New Button()
    Private ReadOnly btnPrev As New Button()
    Private ReadOnly lblPage As New Label()
    Private ReadOnly btnNext As New Button()
    Private ReadOnly btnLast As New Button()
    Private syncing As Boolean = False

    Public Property PageSize As Integer = 20
    Public Property CurrentPage As Integer = 1
    Public Property TotalRows As Integer = 0

    Public ReadOnly Property TotalPages As Integer
        Get
            Return Math.Max(1, CInt(Math.Ceiling(TotalRows / PageSize)))
        End Get
    End Property

    Public Sub New(grid As DataGridView, Optional size As Integer = 20)
        PageSize = size
        Dim host As Control = grid.Parent
        If host Is Nothing Then
            Throw New InvalidOperationException("GridPager: put the grid on a form or panel before creating the pager.")
        End If

        ' paging only sorts one page at a time, so clicking a header must not sort
        For Each col As DataGridViewColumn In grid.Columns
            col.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
        AddHandler grid.ColumnAdded,
            Sub(snd As Object, args As DataGridViewColumnEventArgs)
                args.Column.SortMode = DataGridViewColumnSortMode.NotSortable
            End Sub

        StyleNav(btnFirst, "<<")
        StyleNav(btnPrev, "<")
        StyleNav(btnNext, ">")
        StyleNav(btnLast, ">>")
        AddHandler btnFirst.Click, Sub() GoToPage(1)
        AddHandler btnPrev.Click, Sub() GoToPage(CurrentPage - 1)
        AddHandler btnNext.Click, Sub() GoToPage(CurrentPage + 1)
        AddHandler btnLast.Click, Sub() GoToPage(TotalPages)

        lblInfo.AutoSize = False
        lblInfo.TextAlign = ContentAlignment.MiddleLeft
        lblInfo.Font = New Font("Segoe UI", 9)
        lblInfo.ForeColor = navy

        lblRows.AutoSize = False
        lblRows.Text = "Rows per page:"
        lblRows.TextAlign = ContentAlignment.MiddleRight
        lblRows.Font = New Font("Segoe UI", 9)
        lblRows.Height = 26
        lblRows.Width = TextRenderer.MeasureText(lblRows.Text, lblRows.Font).Width + 8

        lblPage.AutoSize = False
        lblPage.TextAlign = ContentAlignment.MiddleCenter
        lblPage.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        lblPage.ForeColor = navy
        lblPage.Height = 26

        cboSize.DropDownStyle = ComboBoxStyle.DropDownList
        cboSize.Font = New Font("Segoe UI", 9)
        cboSize.Width = 56
        cboSize.Items.AddRange(New Object() {"10", "20", "50", "100"})
        AddHandler cboSize.SelectedIndexChanged, AddressOf RowsPerPage_Changed

        pnl.Controls.Add(lblInfo)
        pnl.Controls.Add(lblRows)
        pnl.Controls.Add(cboSize)
        pnl.Controls.Add(btnFirst)
        pnl.Controls.Add(btnPrev)
        pnl.Controls.Add(lblPage)
        pnl.Controls.Add(btnNext)
        pnl.Controls.Add(btnLast)

        If grid.Dock = DockStyle.None Then
            ' a grid taller than its panel would push the page bar out of view: trim it first
            If grid.Bottom > host.ClientSize.Height Then
                grid.Height = Math.Max(120, host.ClientSize.Height - grid.Top)
            End If
            grid.Height -= 34
            pnl.SetBounds(grid.Left, grid.Bottom + 2, grid.Width, 32)
            Dim a As AnchorStyles = AnchorStyles.Left
            If grid.Anchor.HasFlag(AnchorStyles.Right) Then a = a Or AnchorStyles.Right
            a = a Or If(grid.Anchor.HasFlag(AnchorStyles.Bottom), AnchorStyles.Bottom, AnchorStyles.Top)
            pnl.Anchor = a
            host.Controls.Add(pnl)
            pnl.BringToFront()      ' <-- add this line
        Else
            pnl.Height = 32
            pnl.Dock = DockStyle.Bottom
            host.Controls.Add(pnl)
            grid.BringToFront()
        End If

        AddHandler pnl.Resize, Sub() LayoutControls()
        UpdateUi()
    End Sub

    ' runs an export with ALL rows in the grid, then returns the grid to the page the user was on
    Public Sub ExportAllPages(reload As Action, doExport As Action)
        Dim oldSize As Integer = PageSize
        Dim oldPage As Integer = CurrentPage
        Try
            PageSize = 1000000
            CurrentPage = 1
            reload()
            doExport()
        Finally
            PageSize = oldSize
            CurrentPage = oldPage
            reload()
        End Try
    End Sub

    Private Sub StyleNav(b As Button, text As String)
        b.Text = text
        b.FlatStyle = FlatStyle.Flat
        b.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        b.Cursor = Cursors.Hand
        b.TabStop = False
        b.Size = New Size(34, 26)
    End Sub

    ' enabled = navy button with white text, disabled = light grey (like the sample)
    Private Sub SetNav(b As Button, enabled As Boolean)
        b.Enabled = enabled
        b.BackColor = If(enabled, navy, Color.FromArgb(235, 235, 235))
        b.ForeColor = If(enabled, Color.White, Color.Gray)
        b.FlatAppearance.BorderColor = If(enabled, navy, Color.Silver)
    End Sub

    Private Sub UpdateUi()
        Dim firstRow As Integer = If(TotalRows = 0, 0, (CurrentPage - 1) * PageSize + 1)
        Dim lastRow As Integer = Math.Min(CurrentPage * PageSize, TotalRows)
        lblInfo.Text = If(TotalRows = 0, "Showing 0 of 0", "Showing " & firstRow & "-" & lastRow & " of " & TotalRows)
        lblPage.Text = "Page " & CurrentPage & " of " & TotalPages
        lblPage.Width = TextRenderer.MeasureText(lblPage.Text, lblPage.Font).Width + 14

        SetNav(btnFirst, CurrentPage > 1)
        SetNav(btnPrev, CurrentPage > 1)
        SetNav(btnNext, CurrentPage < TotalPages)
        SetNav(btnLast, CurrentPage < TotalPages)

        SyncRowsBox()
        LayoutControls()
    End Sub

    ' keeps the "Rows per page" box showing the real page size (and adds odd sizes like 15 or 25)
    Private Sub SyncRowsBox()
        If PageSize > 1000 Then Return          ' temporary export size: leave the box alone
        syncing = True
        Dim s As String = PageSize.ToString()
        If Not cboSize.Items.Contains(s) Then
            Dim at As Integer = 0
            While at < cboSize.Items.Count AndAlso Convert.ToInt32(cboSize.Items(at)) < PageSize
                at += 1
            End While
            cboSize.Items.Insert(at, s)
        End If
        cboSize.SelectedItem = s
        syncing = False
    End Sub

    Private Sub RowsPerPage_Changed(sender As Object, e As EventArgs)
        If syncing OrElse cboSize.SelectedIndex < 0 Then Return
        PageSize = Convert.ToInt32(cboSize.Text)
        CurrentPage = 1
        RaiseEvent PageChanged(Me, EventArgs.Empty)
    End Sub

    ' everything is packed against the right edge, the "Showing..." text takes the rest
    Private Sub LayoutControls()
        Dim x As Integer = pnl.Width - 4
        PlaceRight(btnLast, x)
        PlaceRight(btnNext, x)
        PlaceRight(lblPage, x)
        PlaceRight(btnPrev, x)
        PlaceRight(btnFirst, x)
        x -= 14
        PlaceRight(cboSize, x)
        PlaceRight(lblRows, x)
        lblInfo.SetBounds(0, 3, Math.Max(0, x), 26)
    End Sub

    Private Sub PlaceRight(c As Control, ByRef x As Integer)
        x -= c.Width
        c.SetBounds(x, 3 + (26 - c.Height) \ 2, c.Width, c.Height)
        x -= 3
    End Sub

    Private Sub GoToPage(p As Integer)
        If p < 1 OrElse p > TotalPages Then Return
        CurrentPage = p
        RaiseEvent PageChanged(Me, EventArgs.Empty)
    End Sub

    Public Sub Reset()
        CurrentPage = 1
    End Sub

    ' baseQuery: full SELECT including ORDER BY, without LIMIT. Column names must be unique.
    Public Function LoadPage(baseQuery As String, names As String(), values As Object()) As DataTable
        TotalRows = Convert.ToInt32(If(ExecScalar("SELECT COUNT(*) FROM (" & baseQuery & ") AS pg_count", names, values), 0))
        If CurrentPage > TotalPages Then CurrentPage = TotalPages
        UpdateUi()
        Return GetDataTable(baseQuery & " LIMIT " & PageSize & " OFFSET " & ((CurrentPage - 1) * PageSize), names, values)
    End Function

    ' for screens that already load the whole list into a DataTable: returns just the current page
    Public Function Slice(source As DataTable) As DataTable
        TotalRows = If(source Is Nothing, 0, source.Rows.Count)
        If CurrentPage > TotalPages Then CurrentPage = TotalPages
        UpdateUi()

        If source Is Nothing Then Return New DataTable()
        Dim result As DataTable = source.Clone()
        Dim first As Integer = (CurrentPage - 1) * PageSize
        Dim last As Integer = Math.Min(first + PageSize, source.Rows.Count) - 1
        For i As Integer = first To last
            result.ImportRow(source.Rows(i))
        Next
        Return result
    End Function

End Class