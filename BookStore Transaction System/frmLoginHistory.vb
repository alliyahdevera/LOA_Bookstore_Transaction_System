Imports MySql.Data.MySqlClient

Public Class frmLoginHistory

    Private WithEvents tmrClock As System.Windows.Forms.Timer

    Private Sub frmLoginHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' User Profile Info
            lblname.Text = If(Not String.IsNullOrEmpty(currentuser.FullName), currentuser.FullName, "N/A")
            lblposition.Text = If(Not String.IsNullOrEmpty(currentuser.Role), currentuser.Role, "N/A")

            ' Live Timer Setup
            tmrClock = New System.Windows.Forms.Timer()
            tmrClock.Interval = 1000
            tmrClock.Start()
            UpdateFooterDateTime()

            ' Date Picker Initial Values
            dtpFrom.Value = System.DateTime.Today
            dtpTo.Value = System.DateTime.Today

            LoadLoginLogs()
        Catch ex As Exception
            MsgBox("Error initializing Login History: " & ex.Message, vbCritical, "Init Error")
        End Try
    End Sub

    Private Sub frmLoginHistory_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
        If tmrClock IsNot Nothing Then
            tmrClock.Stop()
            tmrClock.Dispose()
        End If
    End Sub

    Private Sub tmrClock_Tick(sender As Object, e As EventArgs) Handles tmrClock.Tick
        UpdateFooterDateTime()
    End Sub

    Private Sub UpdateFooterDateTime()
        If lbldatetime IsNot Nothing Then
            lbldatetime.Text = System.DateTime.Now.ToString("dddd, MMMM d, yyyy h:mm:ss tt")
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        LoadLoginLogs()
    End Sub

    Public Sub LoadLoginLogs()
        Try
            If Not connection() Then Exit Sub

            Dim query As String = "SELECT u.username, " &
                                  "CONCAT(u.first_name, ' ', u.last_name) AS fullname, " &
                                  "r.role_name, " &
                                  "IFNULL(a.status, 'Success') AS status, " &
                                  "DATE_FORMAT(a.created_at, '%Y-%m-%d %h:%i:%s %p') AS log_datetime " &
                                  "FROM tbl_audit_logs a " &
                                  "INNER JOIN tbl_users u ON a.user_id = u.user_id " &
                                  "INNER JOIN tbl_roles r ON u.role_id = r.role_id " &
                                  "WHERE a.log_type = 'Login' " &
                                  "AND DATE(a.created_at) BETWEEN @dateFrom AND @dateTo " &
                                  "ORDER BY a.created_at DESC"

            Using cmd As New MySqlCommand(query, cn)
                cmd.Parameters.AddWithValue("@dateFrom", dtpFrom.Value.ToString("yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@dateTo", dtpTo.Value.ToString("yyyy-MM-dd"))

                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    dgvLoginHistory.Rows.Clear()
                    While dr.Read()
                        Dim idx As Integer = dgvLoginHistory.Rows.Add(
        dr("username").ToString(),
        dr("fullname").ToString(),
        dr("role_name").ToString(),
        dr("status").ToString(),
        dr("log_datetime").ToString()
    )
                        If dr("status").ToString().StartsWith("Failed") Then
                            dgvLoginHistory.Rows(idx).DefaultCellStyle.ForeColor = Color.Firebrick
                        End If
                    End While
                End Using
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading login logs: " & ex.Message, vbCritical, "Audit Logs")
        End Try
    End Sub

End Class