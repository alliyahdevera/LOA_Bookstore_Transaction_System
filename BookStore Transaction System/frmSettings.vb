Public Class frmSettings

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Change Password shortcut, same look as the Manage School Year button
        Dim btnPass As New Button With {
            .Name = "btnchangepassword",
            .Text = "Change Password",
            .Size = btngenerate.Size,
            .Location = New Point(btngenerate.Right + 12, btngenerate.Top),
            .Font = btngenerate.Font,
            .BackColor = btngenerate.BackColor,
            .ForeColor = btngenerate.ForeColor,
            .FlatStyle = btngenerate.FlatStyle,
            .Cursor = Cursors.Hand
        }
        btnPass.FlatAppearance.BorderSize = btngenerate.FlatAppearance.BorderSize
        AddHandler btnPass.Click, AddressOf btnchangepassword_Click
        Controls.Add(btnPass)
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        OpenDialog(New frmManageSY())
    End Sub

    Private Sub btnchangepassword_Click(sender As Object, e As EventArgs)
        OpenDialog(New frmChangePassword())
    End Sub

    ' opens a form as a centered pop-up
    Private Sub OpenDialog(f As Form)
        Using f
            f.StartPosition = FormStartPosition.CenterParent
            f.FormBorderStyle = FormBorderStyle.FixedDialog
            f.MaximizeBox = False
            f.MinimizeBox = False
            f.ShowInTaskbar = False
            f.ShowDialog(Me)
        End Using
    End Sub

End Class