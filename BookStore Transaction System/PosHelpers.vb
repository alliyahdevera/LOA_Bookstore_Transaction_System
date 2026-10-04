Public Module PlaceholderHelper

    Private Declare Unicode Function SendMessage Lib "user32.dll" Alias "SendMessageW" (hWnd As IntPtr, msg As Integer, wParam As Integer, lParam As String) As IntPtr
    Private Const EM_SETCUEBANNER As Integer = &H1501

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
                SetPlaceholder(tb, If(tb.Name = "txtProductSearch", "Product code or name", "Search..."))
            End If
            If c.HasChildren Then ApplySearchPlaceholders(c)
        Next
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

            If dt.Rows.Count > 0 Then
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

    ' Fills a "Type" filter combo for a category (0 = all categories). Index 0 is "-- All Types --".
    Public Sub FillTypeCombo(cbo As ComboBox, categoryId As Integer)
        Dim dt As DataTable
        If categoryId = 0 Then
            dt = GetDataTable("SELECT category_type_id, type_name FROM TBL_CATEGORY_TYPES ORDER BY type_name")
        Else
            dt = GetDataTable("SELECT category_type_id, type_name FROM TBL_CATEGORY_TYPES WHERE category_id = @c ORDER BY type_name",
                              New String() {"@c"}, New Object() {categoryId})
        End If
        Dim row As DataRow = dt.NewRow()
        row("category_type_id") = 0
        row("type_name") = "-- All Types --"
        dt.Rows.InsertAt(row, 0)
        FillCombo(cbo, dt, "type_name", "category_type_id")
        cbo.SelectedIndex = 0
    End Sub

End Module
Public Interface IBuyerInfo
    ReadOnly Property BuyerType As String
    ReadOnly Property BuyerName As String
    ReadOnly Property StudentId As Integer
    Function ValidateBuyer(ByRef message As String) As Boolean
    Sub ClearBuyer()
End Interface

Public Class GridPager

    Public Event PageChanged As EventHandler

    Private ReadOnly pnl As New Panel()
    Private ReadOnly btnPrev As New Button()
    Private ReadOnly btnNext As New Button()
    Private ReadOnly lblPage As New Label()

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

        btnPrev.Text = "< Prev"
        btnNext.Text = "Next >"
        For Each b As Button In New Button() {btnPrev, btnNext}
            b.FlatStyle = FlatStyle.Flat
            b.BackColor = Color.White
            b.Cursor = Cursors.Hand
        Next
        lblPage.TextAlign = ContentAlignment.MiddleRight
        lblPage.Font = New Font("Segoe UI", 9)
        pnl.Controls.AddRange(New Control() {lblPage, btnPrev, btnNext})

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
        AddHandler btnPrev.Click, Sub() GoToPage(CurrentPage - 1)
        AddHandler btnNext.Click, Sub() GoToPage(CurrentPage + 1)
        LayoutControls()
        UpdateLabels()
    End Sub

    Private Sub LayoutControls()
        btnNext.SetBounds(pnl.Width - 74, 3, 70, 26)
        btnPrev.SetBounds(btnNext.Left - 74, 3, 70, 26)
        lblPage.SetBounds(0, 3, Math.Max(0, btnPrev.Left - 6), 26)
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
        UpdateLabels()
        Return GetDataTable(baseQuery & " LIMIT " & PageSize & " OFFSET " & ((CurrentPage - 1) * PageSize), names, values)
    End Function

    Private Sub UpdateLabels()
        lblPage.Text = "Page " & CurrentPage & " of " & TotalPages & "   (" & TotalRows & " items)"
        btnPrev.Enabled = CurrentPage > 1
        btnNext.Enabled = CurrentPage < TotalPages
    End Sub

End Class