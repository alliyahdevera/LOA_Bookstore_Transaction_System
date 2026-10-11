Public Class frmAuditLogs

    Private _currentForm As Form
    Private _isLoading As Boolean = True

    Private Sub frmAuditLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _isLoading = True
        cboauditlogs.DropDownStyle = ComboBoxStyle.DropDownList
        cboauditlogs.Items.Clear()
        cboauditlogs.Items.AddRange(New Object() {"Activity History", "Price Change History", "Login History"})
        _isLoading = False
        cboauditlogs.SelectedIndex = 0          ' fires SelectedIndexChanged and opens the first tab
    End Sub

    Private Sub cboauditlogs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboauditlogs.SelectedIndexChanged
        If _isLoading Then Exit Sub

        Select Case cboauditlogs.Text.Trim()
            Case "Activity History"
                OpenTab(GetType(frmActivityHistory))
            Case "Price Change History"
                OpenTab(GetType(frmPriceHistory))
            Case "Login History"
                OpenTab(GetType(frmLoginHistory))
        End Select
    End Sub

    Private Sub OpenTab(formType As Type)
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
    End Sub

End Class