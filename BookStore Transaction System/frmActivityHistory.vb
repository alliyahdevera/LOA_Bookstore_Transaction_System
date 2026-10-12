Imports MySql.Data.MySqlClient

Public Class frmActivityHistory

    Private WithEvents tmrClock As System.Windows.Forms.Timer
    Private isInitializing As Boolean = True
    Private pg As GridPager

    Private ReadOnly actionGroups As New Dictionary(Of String, String()) From {
        {"Add", New String() {"Add %"}},
        {"Edit", New String() {"Update %"}},
        {"Delete", New String() {"Delete %", "Remove %", "Deactivate %"}},
        {"Sale", New String() {"Sale", "Cancel Transaction", "Reprint Receipt"}},
        {"Return / Exchange", New String() {"Item %"}},
        {"Stock In", New String() {"Stock In"}},
        {"Inventory Count / Adjustment", New String() {"Inventory %"}},
        {"Supervisor Approval", New String() {"Supervisor Approval%"}},
        {"Remittance", New String() {"Remittance"}}
    }
    Private Sub FillActionFilter()
        cbofilter.DropDownStyle = ComboBoxStyle.DropDownList
        cbofilter.Items.Clear()
        cbofilter.Items.Add("All Actions")
        For Each k As String In actionGroups.Keys
            cbofilter.Items.Add(k)
        Next
        cbofilter.SelectedIndex = 0
    End Sub

    Private Sub cbofilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbofilter.SelectedIndexChanged
        If isInitializing Then Exit Sub
        pg.Reset()
        LoadActivityLogs()
    End Sub

    Private Sub frmActivityHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Add the missing Date & Time column in code
            If Not dgvActivityHistory.Columns.Contains("LogDateTime") Then
                dgvActivityHistory.Columns.Add("LogDateTime", "Date & Time")

            End If

            ' User Profile Info
            lblname.Text = If(Not String.IsNullOrEmpty(currentuser.FullName), currentuser.FullName, "N/A")
            lblposition.Text = If(Not String.IsNullOrEmpty(currentuser.Role), currentuser.Role, "N/A")

            ' Live Timer Setup
            tmrClock = New System.Windows.Forms.Timer()
            tmrClock.Interval = 1000
            tmrClock.Start()
            UpdateFooterDateTime()

            ' Date Picker Initial Values
            dtpfrom.Value = DateTime.Today
            dtpto.Value = DateTime.Today

            FillActionFilter()
            pg = New GridPager(dgvActivityHistory, 20)
            AddHandler pg.PageChanged, Sub() LoadActivityLogs()
            isInitializing = False
            LoadActivityLogs()
        Catch ex As Exception
            MsgBox("Error initializing Activity History: " & ex.Message, vbCritical, "Init Error")
        End Try
        For Each col As DataGridViewColumn In dgvActivityHistory.Columns
            If col.HeaderText = "Details" Then
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                col.Width = 420
            End If
        Next
        dgvActivityHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    End Sub

    Private Sub frmActivityHistory_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
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
            lbldatetime.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy h:mm:ss tt")
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        If dtpfrom.Value.Date > dtpto.Value.Date Then
            MsgBox("'Date from' cannot be later than 'To'.", vbExclamation, "Activity Logs")
            Exit Sub
        End If
        pg.Reset()
        LoadActivityLogs()
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        pg.ExportAllPages(Sub() LoadActivityLogs(), Sub() ExportGridToCsv(dgvActivityHistory, "ActivityLogs"))
    End Sub
    Public Sub LoadActivityLogs()
        Try
            If pg Is Nothing Then Exit Sub
            Dim selected As String = If(cbofilter Is Nothing OrElse cbofilter.SelectedIndex <= 0, "", cbofilter.Text)

            Dim names As New List(Of String)({"@dateFrom", "@dateTo"})
            Dim values As New List(Of Object)({CType(dtpfrom.Value.Date, Object), CType(dtpto.Value.Date, Object)})

            Dim query As String =
                "SELECT u.username, CONCAT(u.first_name, ' ', u.last_name) AS fullname, r.role_name, " &
                "IFNULL(a.action_type, '-') AS action_type, IFNULL(a.reference_no, '-') AS reference_no, " &
                "IFNULL(a.details, '-') AS details, " &
                "DATE_FORMAT(a.created_at, '%Y-%m-%d %h:%i:%s %p') AS log_datetime " &
                "FROM tbl_audit_logs a " &
                "INNER JOIN tbl_users u ON a.user_id = u.user_id " &
                "INNER JOIN tbl_roles r ON u.role_id = r.role_id " &
                "WHERE DATE(a.created_at) BETWEEN @dateFrom AND @dateTo "

            query &= "AND a.log_type = 'Activity' AND a.action_type NOT LIKE '%Login%' "
            If selected <> "" Then
                Dim patterns As String() = actionGroups(selected)
                Dim parts As New List(Of String)
                For i As Integer = 0 To patterns.Length - 1
                    parts.Add("a.action_type LIKE @p" & i)
                    names.Add("@p" & i)
                    values.Add(patterns(i))
                Next
                query &= "AND (" & String.Join(" OR ", parts) & ") "
            End If

            query &= "ORDER BY a.created_at DESC, a.audit_id DESC"

            Dim dt As DataTable = pg.LoadPage(query, names.ToArray(), values.ToArray())

            dgvActivityHistory.SuspendLayout()
            dgvActivityHistory.Rows.Clear()
            If dt IsNot Nothing Then
                For Each r As DataRow In dt.Rows
                    dgvActivityHistory.Rows.Add(
                        r("username").ToString(),
                        r("fullname").ToString(),
                        r("role_name").ToString(),
                        r("action_type").ToString(),
                        r("reference_no").ToString(),
                        r("details").ToString(),
                        r("log_datetime").ToString())
                Next
            End If
            dgvActivityHistory.ResumeLayout()
        Catch ex As Exception
            MsgBox("Error loading activity logs: " & ex.Message, vbCritical, "Audit Logs")
        End Try
    End Sub
End Class