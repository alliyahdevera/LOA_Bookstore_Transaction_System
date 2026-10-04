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