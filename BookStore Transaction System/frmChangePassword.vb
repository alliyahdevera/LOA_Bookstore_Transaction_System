Public Class frmChangePassword

    Private Sub frmChangePassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = False
        Me.KeyPreview = True
        Me.AcceptButton = btnLogin              ' Enter = Update

        txtold.PasswordChar = "*"c
        txtnew.PasswordChar = "*"c
        txtconfirm.PasswordChar = "*"c
        txtold.Clear()
        txtnew.Clear()
        txtconfirm.Clear()
        txtold.Focus()
    End Sub

    ' Esc = close without changing anything
    Private Sub frmChangePassword_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    ' ---- show / hide password buttons ----
    Private Sub TogglePassword(tb As TextBox)
        tb.PasswordChar = If(tb.PasswordChar = ControlChars.NullChar, "*"c, ControlChars.NullChar)
    End Sub

    Private Sub btnnewp_Click(sender As Object, e As EventArgs) Handles btnnewp.Click
        TogglePassword(txtnew)
    End Sub

    Private Sub btncshow_Click(sender As Object, e As EventArgs) Handles btncshow.Click
        TogglePassword(txtconfirm)
    End Sub

    ' ---- Update ----
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim oldPass As String = txtold.Text
        Dim newPass As String = txtnew.Text
        Dim confirmPass As String = txtconfirm.Text

        If oldPass = "" Then
            MsgBox("Enter your old password.", vbExclamation, "Change Password")
            txtold.Focus()
            Exit Sub
        End If

        If newPass = "" Then
            MsgBox("Enter your new password.", vbExclamation, "Change Password")
            txtnew.Focus()
            Exit Sub
        End If

        If newPass.Length < 6 Then
            MsgBox("New password must be at least 6 characters.", vbExclamation, "Change Password")
            txtnew.Focus()
            Exit Sub
        End If

        If newPass <> newPass.Trim() Then
            MsgBox("New password cannot start or end with a space.", vbExclamation, "Change Password")
            txtnew.Focus()
            Exit Sub
        End If

        If confirmPass = "" Then
            MsgBox("Confirm your new password.", vbExclamation, "Change Password")
            txtconfirm.Focus()
            Exit Sub
        End If

        If newPass <> confirmPass Then
            MsgBox("New Password and Confirm Password do not match.", vbExclamation, "Change Password")
            txtconfirm.Clear()
            txtconfirm.Focus()
            Exit Sub
        End If

        If newPass = oldPass Then
            MsgBox("New password must be different from the old password.", vbExclamation, "Change Password")
            txtnew.Focus()
            Exit Sub
        End If

        Try
            ' 1) Verify the old password for the LOGGED-IN user
            Dim ok As Boolean = Convert.ToInt32(If(ExecScalar(
                "SELECT COUNT(*) FROM tbl_users WHERE user_id = @id AND password = @p AND status = 'Active'",
                New String() {"@id", "@p"},
                New Object() {currentuser.UserID, HashPassword(oldPass)}), 0)) > 0

            If Not ok Then
                MsgBox("Old password is incorrect.", vbExclamation, "Change Password")
                txtold.Clear()
                txtold.Focus()
                Exit Sub
            End If

            ' 2) Confirm
            If MsgBox("Change your password?", vbYesNo + vbQuestion, "Confirm Password Change") <> MsgBoxResult.Yes Then Exit Sub

            ' 3) Update ONLY this user's own password (ID comes from the session, not a textbox)
            If Not ExecNonQuery("UPDATE tbl_users SET password = @new WHERE user_id = @id",
                                New String() {"@new", "@id"},
                                New Object() {HashPassword(newPass), currentuser.UserID}) Then
                MsgBox("The password could not be changed. Please try again.", vbCritical, "Error")
                Exit Sub
            End If

            LogActivity("Update Password", Nothing, currentuser.FullName & " changed their password.")

            MsgBox("Your password was changed successfully!", vbInformation, "Success")
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MsgBox("Error changing password: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

End Class