Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class frmLoginHistory

    Private pg As GridPager
    Private WithEvents tmrClock As System.Windows.Forms.Timer
    Private isInitializing As Boolean = True

    Private Sub frmLoginHistory_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        isInitializing = True

        Try
            lblname.Text =
                If(String.IsNullOrWhiteSpace(currentuser.FullName),
                   "N/A",
                   currentuser.FullName)

            lblposition.Text =
                If(String.IsNullOrWhiteSpace(currentuser.Role),
                   "N/A",
                   currentuser.Role)

            tmrClock = New System.Windows.Forms.Timer()
            tmrClock.Interval = 1000
            tmrClock.Start()

            UpdateFooterDateTime()

            dtpFrom.Value = System.DateTime.Today
            dtpTo.Value = System.DateTime.Today

            FillUserFilter()

            pg = New GridPager(dgvLoginHistory, 20)
            AddHandler pg.PageChanged, Sub() LoadLoginLogs()

            isInitializing = False
            LoadLoginLogs()

        Catch ex As Exception
            MsgBox(
                "Error initializing Login History: " & ex.Message,
                vbCritical,
                "Login History")
        End Try
    End Sub

    Private Sub FillUserFilter()
        Dim users As New DataTable()

        users.Columns.Add("user_id", GetType(Integer))
        users.Columns.Add("display_name", GetType(String))
        users.Rows.Add(0, "All Users")

        Using conn As MySqlConnection = NewConnection()
            conn.Open()

            Dim query As String =
                "SELECT user_id, username " &
                "FROM tbl_users " &
                "ORDER BY username, user_id"

            Using command As New MySqlCommand(query, conn)
                Using reader As MySqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        users.Rows.Add(
                            Convert.ToInt32(reader("user_id")),
                            reader("username").ToString())
                    End While
                End Using
            End Using
        End Using

        cbotype.DataSource = Nothing
        cbotype.Items.Clear()
        cbotype.DropDownStyle = ComboBoxStyle.DropDownList
        cbotype.DisplayMember = "display_name"
        cbotype.ValueMember = "user_id"
        cbotype.DataSource = users
        cbotype.SelectedIndex = 0
    End Sub

    Private Sub cbotype_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    ) Handles cbotype.SelectedIndexChanged

        If isInitializing OrElse pg Is Nothing Then Exit Sub

        pg.Reset()
        LoadLoginLogs()
    End Sub

    Private Sub btnGenerate_Click(
        sender As Object,
        e As EventArgs
    ) Handles btngenerate.Click

        If pg Is Nothing Then Exit Sub

        If dtpFrom.Value.Date > dtpTo.Value.Date Then
            MsgBox(
                "'Date from' cannot be later than 'To'.",
                vbExclamation,
                "Login History")
            Exit Sub
        End If

        pg.Reset()
        LoadLoginLogs()
    End Sub

    Public Sub LoadLoginLogs()
        If pg Is Nothing Then Exit Sub

        If dtpFrom.Value.Date > dtpTo.Value.Date Then
            dgvLoginHistory.Rows.Clear()
            Exit Sub
        End If

        Try
            Dim selectedUserId As Integer = 0
            Dim selectedUser As DataRowView =
                TryCast(cbotype.SelectedItem, DataRowView)

            If selectedUser IsNot Nothing Then
                selectedUserId =
                    Convert.ToInt32(selectedUser("user_id"))
            End If

            Dim names As New List(Of String) From {
                "@dateFrom",
                "@dateTo"
            }

            Dim values As New List(Of Object) From {
                dtpFrom.Value.Date,
                dtpTo.Value.Date.AddDays(1)
            }

            Dim query As String =
                "SELECT u.username, " &
                "CONCAT(u.first_name, ' ', u.last_name) AS fullname, " &
                "r.role_name, " &
                "IFNULL(a.status, 'Success') AS status, " &
                "DATE_FORMAT(a.created_at, '%Y-%m-%d %h:%i:%s %p') AS log_datetime " &
                "FROM tbl_audit_logs a " &
                "INNER JOIN tbl_users u ON a.user_id = u.user_id " &
                "INNER JOIN tbl_roles r ON u.role_id = r.role_id " &
                "WHERE a.log_type = 'Login' " &
                "AND a.created_at >= @dateFrom " &
                "AND a.created_at < @dateTo "

            If selectedUserId <> 0 Then
                query &= "AND a.user_id = @userId "

                names.Add("@userId")
                values.Add(selectedUserId)
            End If

            query &= "ORDER BY a.created_at DESC, a.audit_id DESC"

            Dim result As DataTable =
                pg.LoadPage(query, names.ToArray(), values.ToArray())

            dgvLoginHistory.SuspendLayout()

            Try
                dgvLoginHistory.Rows.Clear()

                If result IsNot Nothing Then
                    For Each row As DataRow In result.Rows
                        Dim statusText As String =
                            row("status").ToString()

                        Dim index As Integer =
                            dgvLoginHistory.Rows.Add(
                                row("username").ToString(),
                                row("fullname").ToString(),
                                row("role_name").ToString(),
                                statusText,
                                row("log_datetime").ToString())

                        If statusText.StartsWith(
                            "Failed",
                            StringComparison.OrdinalIgnoreCase
                        ) Then
                            dgvLoginHistory.Rows(index).
                                DefaultCellStyle.ForeColor =
                                Color.Firebrick
                        End If
                    Next
                End If
            Finally
                dgvLoginHistory.ResumeLayout()
            End Try

        Catch ex As Exception
            MsgBox(
                "Error loading login logs: " & ex.Message,
                vbCritical,
                "Login History")
        End Try
    End Sub

    Private Sub btnexportexcel_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnexportexcel.Click

        If pg Is Nothing Then Exit Sub

        If dtpFrom.Value.Date > dtpTo.Value.Date Then
            MsgBox(
                "'Date from' cannot be later than 'To'.",
                vbExclamation,
                "Login History")
            Exit Sub
        End If

        pg.ExportAllPages(
            Sub() LoadLoginLogs(),
            Sub() ExportGridToCsv(dgvLoginHistory, "LoginHistory"))
    End Sub

    Private Sub tmrClock_Tick(
        sender As Object,
        e As EventArgs
    ) Handles tmrClock.Tick

        UpdateFooterDateTime()
    End Sub

    Private Sub UpdateFooterDateTime()
        lbldatetime.Text =
            System.DateTime.Now.ToString(
                "dddd, MMMM d, yyyy h:mm:ss tt")
    End Sub

    Private Sub frmLoginHistory_Disposed(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Disposed

        If tmrClock IsNot Nothing Then
            tmrClock.Stop()
            tmrClock.Dispose()
        End If
    End Sub

End Class