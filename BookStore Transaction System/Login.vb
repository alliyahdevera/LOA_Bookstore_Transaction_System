Imports MySql.Data.MySqlClient

Public Class Login

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtPassword.UseSystemPasswordChar = True
        txtPassword.PasswordChar = ControlChars.NullChar
        Me.AcceptButton = btnLogin
    End Sub

    Private Sub Login_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible Then
            txtUsername.Clear()
            txtPassword.Clear()
            cboShowPW.Checked = False
            txtUsername.Focus()
        End If
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            MsgBox("Fill in Username", vbExclamation, "Bookstore Transaction System")
            txtUsername.Focus()
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MsgBox("Fill in Password", vbExclamation, "Bookstore Transaction System")
            txtPassword.Focus()
            Exit Sub
        End If

        Try
            If Not connection() Then Exit Sub

            sql = "SELECT u.user_id, u.first_name, u.last_name, r.role_name " &
      "FROM TBL_USERS u " &
      "INNER JOIN TBL_ROLES r ON u.role_id = r.role_id " &
      "WHERE u.username = @u AND u.password = @p AND u.status = 'Active'"
            cmd = New MySqlCommand(sql, cn)
            cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim())
            cmd.Parameters.AddWithValue("@p", HashPassword(txtPassword.Text))
            dr = cmd.ExecuteReader()

            If dr.Read() Then
                currentuser.UserID = Convert.ToInt32(dr("user_id"))
                currentuser.FullName = dr("first_name").ToString() & " " & dr("last_name").ToString()
                currentuser.Role = dr("role_name").ToString()
                dr.Close()
                cn.Close()

                ' Role has no allowed modules -> do not let the user in
                If GetDefaultForm() = "" Then
                    currentuser.UserID = 0
                    currentuser.FullName = ""
                    currentuser.Role = ""
                    MsgBox("Your account has no access to this system. Please contact the Bookstore Supervisor.", vbExclamation, "Bookstore Transaction System")
                    Exit Sub
                End If

                MsgBox("Welcome, " & currentuser.FullName & "!", vbInformation, "Bookstore Transaction System")
                frmAdminDashboard.Show()
                Me.Hide()
            Else
                dr.Close()
                cn.Close()
                MsgBox("Invalid Username or Password", vbExclamation, "Bookstore Transaction System")
                txtPassword.Clear()
                txtPassword.Focus()
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)

        Finally
            If cn IsNot Nothing AndAlso cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub cboShowPW_CheckedChanged(sender As Object, e As EventArgs) Handles cboShowPW.CheckedChanged
        txtPassword.UseSystemPasswordChar = Not cboShowPW.Checked
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        If MsgBox("Are you sure you want to exit system?", vbQuestion + vbYesNo, "Bookstore Transaction System") = vbYes Then
            End
        End If
    End Sub

End Class