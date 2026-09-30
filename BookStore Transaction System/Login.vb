Imports MySql.Data.MySqlClient

Public Class Login
    ' ---- Disable the Login button while the typed username belongs to an inactive account ----
    Private Sub txtUsername_TextChanged(sender As Object, e As EventArgs) Handles txtUsername.TextChanged
        btnLogin.Enabled = Not IsAccountInactive(txtUsername.Text.Trim())
    End Sub

    Private Sub txtUsername_Leave(sender As Object, e As EventArgs) Handles txtUsername.Leave
        If Not btnLogin.Enabled Then
            MsgBox("This account is inactive. Please contact the Bookstore Supervisor.", vbExclamation, "Bookstore Transaction System")
        End If
    End Sub

    Private Function IsAccountInactive(username As String) As Boolean
        If String.IsNullOrWhiteSpace(username) Then Return False
        Dim st As Object = ExecScalar("SELECT status FROM TBL_USERS WHERE username = @u",
                                  New String() {"@u"}, New Object() {username})
        Return st IsNot Nothing AndAlso Not String.Equals(st.ToString(), "Active", StringComparison.OrdinalIgnoreCase)
    End Function

    ' ---- Records a failed attempt in tbl_audit_logs (shown in frmLoginHistory) ----
    Private Sub LogFailedLogin(username As String)
        Dim dt As DataTable = GetDataTable("SELECT user_id, status FROM TBL_USERS WHERE username = @u",
                                       New String() {"@u"}, New Object() {username})
        If dt.Rows.Count = 0 Then Exit Sub   ' unknown username: user_id is NOT NULL, so nothing to attach

        Dim uid As Integer = Convert.ToInt32(dt.Rows(0)("user_id"))
        Dim isActive As Boolean = String.Equals(dt.Rows(0)("status").ToString(), "Active", StringComparison.OrdinalIgnoreCase)

        RecordAuditLog(uid, "Login", "User Login",
                   If(isActive, "Failed - Incorrect Password", "Failed - Inactive Account"),
                   If(isActive, "Incorrect password entered", "Login attempt on an inactive account"))
    End Sub
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
        If IsAccountInactive(txtUsername.Text.Trim()) Then
            btnLogin.Enabled = False
            MsgBox("This account is inactive. Please contact the Bookstore Supervisor.", vbExclamation, "Bookstore Transaction System")
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

                        ' *** RECORD LOGIN AUDIT LOG ***
                        InsertLoginAuditLog(currentuser.UserID)

                        MsgBox("Welcome, " & currentuser.FullName & "!", vbInformation, "Bookstore Transaction System")

                        frmAdminDashboard.Show()
                        Me.Hide()
                    Else
                        localDr.Close()
                        cn.Close()

                        LogFailedLogin(txtUsername.Text.Trim())

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

    Public Sub RecordAuditLog(userId As Integer, logType As String, actionType As String, status As String, details As String)
        Try
            If Not connection() Then Exit Sub

            Dim sql As String = "INSERT INTO tbl_audit_logs (user_id, log_type, action_type, status, details, created_at) " &
                                "VALUES (@userId, @logType, @actionType, @status, @details, NOW())"

            Using cmd As New MySqlCommand(sql, cn)
                cmd.Parameters.AddWithValue("@userId", userId)
                cmd.Parameters.AddWithValue("@logType", logType)
                cmd.Parameters.AddWithValue("@actionType", actionType)
                cmd.Parameters.AddWithValue("@status", status)
                cmd.Parameters.AddWithValue("@details", details)
                cmd.ExecuteNonQuery()
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
    End Sub

    Private Sub InsertLoginAuditLog(userId As Integer)
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "INSERT INTO tbl_audit_logs " &
                                  "(user_id, log_type, action_type, status, details, created_at) " &
                                  "VALUES (@userId, 'Login', 'User Login', 'Success', 'User logged into system', NOW())"

            Using cmd As New MySqlCommand(query, cn)
                cmd.Parameters.AddWithValue("@userId", userId)
                cmd.ExecuteNonQuery()
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
    End Sub

End Class