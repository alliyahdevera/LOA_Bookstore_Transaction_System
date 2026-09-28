Public Class frmAuditLogs

    Private _currentForm As Form

    Private Sub frmAuditLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OpenTab(btnActivityHistory, GetType(frmActivityHistory))
    End Sub

    Private Sub btnActivityHistory_Click(sender As Object, e As EventArgs) Handles btnActivityHistory.Click
        OpenTab(btnActivityHistory, GetType(frmActivityHistory))
    End Sub

    Private Sub btnPriceChangeHistory_Click(sender As Object, e As EventArgs) Handles btnPriceChangeHistory.Click
        OpenTab(btnPriceChangeHistory, GetType(frmPriceHistory))
    End Sub

    Private Sub btnLoginHistory_Click(sender As Object, e As EventArgs) Handles btnLoginHistory.Click
        OpenTab(btnLoginHistory, GetType(frmLoginHistory))
    End Sub

    Private Sub OpenTab(activeBtn As Button, formType As Type)
        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _currentForm.Dispose()
            _currentForm = Nothing
        End If
        Panel1.Controls.Clear()

        Dim frm As Form = CType(Activator.CreateInstance(formType), Form)
        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill
        Panel1.Controls.Add(frm)
        _currentForm = frm
        frm.Show()

        For Each btn As Button In New Button() {btnActivityHistory, btnPriceChangeHistory, btnLoginHistory}
            btn.BackColor = Color.FromArgb(1, 21, 78)
        Next
        activeBtn.BackColor = Color.FromArgb(25, 55, 140)
    End Sub

End Class