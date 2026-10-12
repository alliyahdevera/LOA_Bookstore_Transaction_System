Public Class frmSupervisorApproval

    ' set by the caller before ShowDialog
    Public Property ActionText As String = ""

    Public ReadOnly Property SupervisorUsername As String
        Get
            Return txtUsername.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property SupervisorPassword As String
        Get
            Return txtPassword.Text
        End Get
    End Property

    Private Sub frmSupervisorApproval_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Supervisor Approval"
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowInTaskbar = False
        Me.AcceptButton = btnApprove
        Me.CancelButton = btnCancel

        txtPassword.UseSystemPasswordChar = True

        Dim shown As String = If(ActionText, "").Trim()
        If shown.Length > 110 Then shown = shown.Substring(0, 107) & "..."
        Label4.Text = "A supervisor must approve:" & vbCrLf & shown

        txtUsername.Focus()
    End Sub

    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        If txtUsername.Text.Trim() = "" Then
            MsgBox("Enter the supervisor username.", vbExclamation, "Supervisor Approval")
            txtUsername.Focus()
            Exit Sub
        End If
        If txtPassword.Text = "" Then
            MsgBox("Enter the supervisor password.", vbExclamation, "Supervisor Approval")
            txtPassword.Focus()
            Exit Sub
        End If
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmSupervisorApproval_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        txtPassword.Clear()
    End Sub

End Class