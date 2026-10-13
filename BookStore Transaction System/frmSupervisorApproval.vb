Public Class frmSupervisorApproval

    ' set by the caller before ShowDialog
    Public Property ActionText As String = ""

    ' saved when Approve is clicked, because the textboxes are cleared when the form closes
    Private _username As String = ""
    Private _password As String = ""

    Public ReadOnly Property SupervisorUsername As String
        Get
            Return _username
        End Get
    End Property

    Public ReadOnly Property SupervisorPassword As String
        Get
            Return _password
        End Get
    End Property

    ' Window settings go in the constructor, not in Load
    Public Sub New()
        InitializeComponent()

        Me.Text = "Supervisor Approval"
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowInTaskbar = False
        Me.AcceptButton = btnApprove
        Me.CancelButton = btnCancel

        txtPassword.UseSystemPasswordChar = True

        Label4.AutoSize = False
        Label4.Size = New Size(371, 44)          ' room for the action text
    End Sub

    Private Sub frmSupervisorApproval_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        _username = txtUsername.Text.Trim()
        _password = txtPassword.Text

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        _username = ""
        _password = ""
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmSupervisorApproval_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        txtPassword.Clear()          ' the saved copy is read by the caller, this only wipes the screen
    End Sub

End Class