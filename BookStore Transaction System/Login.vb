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

            Dim sqlQuery As String = "SELECT u.user_id, u.first_name, u.last_name, r.role_name " &
                                     "FROM TBL_USERS u " &
                                     "INNER JOIN TBL_ROLES r ON u.role_id = r.role_id " &
                                     "WHERE u.username = @u AND u.password = @p AND u.status = 'Active'"

            Using localCmd As New MySqlCommand(sqlQuery, cn)
                localCmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim())
                localCmd.Parameters.AddWithValue("@p", HashPassword(txtPassword.Text))

                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    If localDr.Read() Then
                        currentuser.UserID = Convert.ToInt32(localDr("user_id"))
                        currentuser.FullName = localDr("first_name").ToString() & " " & localDr("last_name").ToString()
                        currentuser.Role = localDr("role_name").ToString()

                        localDr.Close()
                        cn.Close()

                        ' Verify that the user has a valid role assigned
                        If String.IsNullOrWhiteSpace(currentuser.Role) Then
                            currentuser.UserID = 0
                            currentuser.FullName = ""
                            currentuser.Role = ""
                            MsgBox("Your account has no assigned role in the system. Please contact the Bookstore Supervisor.", vbExclamation, "Bookstore Transaction System")
                            Exit Sub
                        End If

                        MsgBox("Welcome, " & currentuser.FullName & "!", vbInformation, "Bookstore Transaction System")

                        frmAdminDashboard.Show()
                        Me.Hide()
                    Else
                        localDr.Close()
                        cn.Close()
                        MsgBox("Invalid Username or Password", vbExclamation, "Bookstore Transaction System")
                        txtPassword.Clear()
                        txtPassword.Focus()
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            Application.Exit()
        End If
    End Sub

End Class