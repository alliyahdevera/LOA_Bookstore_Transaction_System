Imports System.Drawing
Imports System.Windows.Forms

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

    Private Const MAX_SLOTS As Integer = 7                      ' renamed constant to avoid name collision
    Private ReadOnly navy As Color = Color.FromArgb(1, 21, 78)

    Private ReadOnly pnl As New Panel()
    Private ReadOnly lblInfo As New Label()
    Private ReadOnly btnPrev As New Button()
    Private ReadOnly btnNext As New Button()
    Private ReadOnly slots As New List(Of Button)()

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

        StyleButton(btnPrev, "<")
        StyleButton(btnNext, ">")
        btnPrev.Visible = True
        btnNext.Visible = True
        AddHandler btnPrev.Click, Sub() GoToPage(CurrentPage - 1)
        AddHandler btnNext.Click, Sub() GoToPage(CurrentPage + 1)

        lblInfo.AutoSize = False
        lblInfo.TextAlign = ContentAlignment.MiddleLeft
        lblInfo.Font = New Font("Segoe UI", 9)

        pnl.Controls.Add(lblInfo)
        pnl.Controls.Add(btnPrev)
        pnl.Controls.Add(btnNext)
        For i As Integer = 1 To MAX_SLOTS
            Dim b As New Button()
            StyleButton(b, "")
            AddHandler b.Click, AddressOf PageButton_Click
            slots.Add(b)
            pnl.Controls.Add(b)
        Next

        If grid.Dock = DockStyle.None Then
            grid.Height -= 34
            pnl.SetBounds(grid.Left, grid.Bottom + 2, grid.Width, 32)
            Dim a As AnchorStyles = AnchorStyles.Left
            If grid.Anchor.HasFlag(AnchorStyles.Right) Then a = a Or AnchorStyles.Right
            a = a Or If(grid.Anchor.HasFlag(AnchorStyles.Bottom), AnchorStyles.Bottom, AnchorStyles.Top)
            pnl.Anchor = a
            host.Controls.Add(pnl)
        Else
            pnl.Height = 32
            pnl.Dock = DockStyle.Bottom
            host.Controls.Add(pnl)
            grid.BringToFront()
        End If

        AddHandler pnl.Resize, Sub() LayoutControls()
        UpdateUi()
    End Sub

    Private Sub StyleButton(b As Button, text As String)
        b.Text = text
        b.FlatStyle = FlatStyle.Flat
        b.FlatAppearance.BorderColor = Color.Silver
        b.BackColor = Color.White
        b.ForeColor = Color.Black
        b.Font = New Font("Segoe UI", 9)
        b.Cursor = Cursors.Hand
        b.TabStop = False
        b.Visible = False
    End Sub

    ' ---------- which page numbers to show (0 = "...") ----------
    '  <  1 2 3 4 ... 10  >      <  1 ... 4 5 6 ... 10  >      <  1 ... 7 8 9 10  >
    Private Function BuildPageList() As List(Of Integer)
        Dim items As New List(Of Integer)()
        Dim total As Integer = TotalPages

        If total <= MAX_SLOTS Then
            For i As Integer = 1 To total
                items.Add(i)
            Next
            Return items
        End If

        Dim s As Integer = Math.Max(2, CurrentPage - 1)
        Dim e As Integer = Math.Min(total - 1, CurrentPage + 1)
        If CurrentPage <= 3 Then
            s = 2
            e = 4
        ElseIf CurrentPage >= total - 2 Then
            s = total - 3
            e = total - 1
        End If

        items.Add(1)
        If s > 2 Then items.Add(0)
        For i As Integer = s To e
            items.Add(i)
        Next
        If e < total - 1 Then items.Add(0)
        items.Add(total)
        Return items
    End Function

    Private Sub UpdateUi()
        lblInfo.Text = "Page " & CurrentPage & " of " & TotalPages & "   (" & TotalRows & " items)"
        btnPrev.Enabled = CurrentPage > 1
        btnNext.Enabled = CurrentPage < TotalPages

        Dim items As List(Of Integer) = BuildPageList()
        For i As Integer = 0 To slots.Count - 1
            Dim b As Button = slots(i)
            If i >= items.Count Then
                b.Visible = False
                Continue For
            End If

            Dim pageNo As Integer = items(i)
            b.Tag = pageNo
            b.Visible = True

            If pageNo = 0 Then                          ' "..."
                b.Text = "..."
                b.Cursor = Cursors.Default
                b.FlatAppearance.BorderSize = 0
                b.BackColor = Color.White
                b.ForeColor = Color.Gray
            ElseIf pageNo = CurrentPage Then            ' current page highlighted
                b.Text = pageNo.ToString()
                b.Cursor = Cursors.Default
                b.FlatAppearance.BorderSize = 1
                b.BackColor = navy
                b.ForeColor = Color.White
            Else
                b.Text = pageNo.ToString()
                b.Cursor = Cursors.Hand
                b.FlatAppearance.BorderSize = 1
                b.BackColor = Color.White
                b.ForeColor = Color.Black
            End If
        Next
        LayoutControls()
    End Sub

    ' buttons are packed against the right edge, the info text takes the rest
    Private Sub LayoutControls()
        Dim x As Integer = pnl.Width - 4

        PlaceRight(btnNext, x)
        For i As Integer = slots.Count - 1 To 0 Step -1
            If slots(i).Visible Then PlaceRight(slots(i), x)
        Next
        PlaceRight(btnPrev, x)

        lblInfo.SetBounds(0, 3, Math.Max(0, x), 26)
    End Sub

    Private Sub PlaceRight(b As Button, ByRef x As Integer)
        Dim w As Integer = Math.Max(28, TextRenderer.MeasureText(b.Text, b.Font).Width + 16)
        x -= w
        b.SetBounds(x, 3, w, 26)
        x -= 3
    End Sub

    Private Sub PageButton_Click(sender As Object, e As EventArgs)
        Dim p As Integer = Convert.ToInt32(DirectCast(sender, Button).Tag)
        If p > 0 AndAlso p <> CurrentPage Then GoToPage(p)
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