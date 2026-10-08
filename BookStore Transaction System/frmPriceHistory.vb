Imports MySql.Data.MySqlClient

Public Class frmPriceHistory
    Private pg As GridPager
    Private WithEvents tmrClock As System.Windows.Forms.Timer

    Private Sub frmPriceHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
            dtpfrom.Value = DateTime.Today
            dtpto.Value = DateTime.Today
            pg = New GridPager(dgvPriceHistory, 20)
            AddHandler pg.PageChanged, Sub() LoadPriceHistoryLogs()
            LoadPriceHistoryLogs()
        Catch ex As Exception
            MsgBox("Error initializing Price History: " & ex.Message, vbCritical, "Init Error")
        End Try
    End Sub

    Private Sub frmPriceHistory_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
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
            MsgBox("'Date from' cannot be later than 'To'.", vbExclamation, "Price Change History")
            Exit Sub
        End If
        pg.Reset()
        LoadPriceHistoryLogs()
    End Sub

    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        pg.ExportAllPages(Sub() LoadPriceHistoryLogs(), Sub() ExportGridToCsv(dgvPriceHistory, "PriceChangeHistory"))
    End Sub

    Public Sub LoadPriceHistoryLogs()
        If pg Is Nothing Then Exit Sub
        Try
            Dim query As String = "SELECT IFNULL(a.product_code, '-') AS product_code, " &
                                  "IFNULL(a.product_name, '-') AS product_name, " &
                                  "IFNULL(a.old_price, 0) AS old_price, " &
                                  "IFNULL(a.new_price, 0) AS new_price, " &
                                  "CONCAT(u.first_name, ' ', u.last_name) AS changed_by, " &
                                  "DATE_FORMAT(a.created_at, '%Y-%m-%d %h:%i:%s %p') AS date_changed, " &
                                  "IFNULL(a.reason, 'Price Update') AS reason " &
                                  "FROM tbl_audit_logs a " &
                                  "INNER JOIN tbl_users u ON a.user_id = u.user_id " &
                                  "WHERE a.log_type = 'Price Change' " &
                                  "AND DATE(a.created_at) BETWEEN @dateFrom AND @dateTo " &
                                  "ORDER BY a.created_at DESC, a.audit_id DESC"

            Dim dt As DataTable = pg.LoadPage(query,
                New String() {"@dateFrom", "@dateTo"},
                New Object() {dtpfrom.Value.ToString("yyyy-MM-dd"), dtpto.Value.ToString("yyyy-MM-dd")})

            dgvPriceHistory.Rows.Clear()
            For Each r As DataRow In dt.Rows
                dgvPriceHistory.Rows.Add(
                    r("product_code").ToString(),
                    r("product_name").ToString(),
                    Convert.ToDecimal(r("old_price")).ToString("N2"),
                    Convert.ToDecimal(r("new_price")).ToString("N2"),
                    r("changed_by").ToString(),
                    r("date_changed").ToString(),
                    r("reason").ToString())
            Next
        Catch ex As Exception
            MsgBox("Error loading price history logs: " & ex.Message, vbCritical, "Price History")
        End Try
    End Sub
End Class