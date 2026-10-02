Imports System.Drawing.Printing
Imports MySql.Data.MySqlClient

Public Class frmRemittance

    Private ReadOnly Peso As String = ChrW(8369)
    Private isLoading As Boolean = True
    Private printRow As DataGridViewRow

    ' ==================== LOAD ====================
    Private Sub frmRemittance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isLoading = True

        SetupFooter(Me, lblname, lblposition, lbldatetime)

        dtfrom.Value = New Date(Date.Today.Year, Date.Today.Month, 1)
        dtto.Value = Date.Today

        cbocashier.DropDownStyle = ComboBoxStyle.DropDownList

        dgvsalesreport.Columns("Difference").HeaderText = "Salary Deduction"
        dgvsalesreport.AllowUserToAddRows = False
        dgvsalesreport.AllowUserToDeleteRows = False
        dgvsalesreport.ReadOnly = True
        dgvsalesreport.MultiSelect = False
        dgvsalesreport.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        ' Populate Cashiers ComboBox
        Dim dt As DataTable = GetDataTable(
            "SELECT u.user_id, CONCAT(u.first_name, ' ', u.last_name) AS full_name " &
            "FROM tbl_users u INNER JOIN tbl_roles r ON u.role_id = r.role_id " &
            "WHERE r.role_name IN ('Cashier', 'Bookstore Supervisor') ORDER BY u.last_name, u.first_name")

        Dim allRow As DataRow = dt.NewRow()
        allRow("user_id") = 0
        allRow("full_name") = "All Cashiers"
        dt.Rows.InsertAt(allRow, 0)

        cbocashier.DataSource = dt
        cbocashier.DisplayMember = "full_name"
        cbocashier.ValueMember = "user_id"
        cbocashier.SelectedIndex = 0

        isLoading = False
        LoadGrid()
    End Sub

    Private Sub cbocashier_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocashier.SelectedIndexChanged
        If isLoading Then Exit Sub
        LoadGrid()
    End Sub

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        If dtfrom.Value.Date > dtto.Value.Date Then
            MsgBox("'Date from' cannot be later than 'To'.", vbExclamation, "Remittance Report")
            Exit Sub
        End If
        LoadGrid()
        If dgvsalesreport.Rows.Count = 0 Then
            MsgBox("No remittance records found for the selected filters.", vbInformation, "Remittance Report")
        End If
    End Sub

    ' ==================== GRID ====================
    Private Function Txt(r As DataRow, col As String) As String
        Return If(IsDBNull(r(col)), "", r(col).ToString())
    End Function

    Private Sub LoadGrid()
        Dim cashierId As Integer = 0
        If cbocashier.SelectedValue IsNot Nothing Then
            Integer.TryParse(cbocashier.SelectedValue.ToString(), cashierId)
        End If

        Dim dt As DataTable = GetDataTable(
            "SELECT r.remittance_no, e.reconciliation_date, CONCAT(u.first_name, ' ', u.last_name) AS cashier, " &
            "e.total_sales, e.cash_sales, e.actual_cash, e.salary_deduction, r.remittance_amount, r.or_from, r.or_to, " &
            "r.remitted_at, r.received_by, r.remarks AS rem_remarks, e.remarks AS eod_remarks, e.difference, e.status " &
            "FROM tbl_remittances r " &
            "INNER JOIN tbl_end_of_day e ON r.end_of_day_id = e.end_of_day_id " &
            "INNER JOIN tbl_users u ON e.cashier_id = u.user_id " &
            "WHERE e.reconciliation_date BETWEEN @d1 AND @d2 " &
            "AND (@c = 0 OR e.cashier_id = @c) " &
            "ORDER BY r.remitted_at DESC, r.remittance_id DESC",
            New String() {"@d1", "@d2", "@c"},
            New Object() {dtfrom.Value.Date, dtto.Value.Date, cashierId})

        dgvsalesreport.Rows.Clear()

        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                Dim idx As Integer = dgvsalesreport.Rows.Add()
                Dim row As DataGridViewRow = dgvsalesreport.Rows(idx)

                Dim recDate As Date = Convert.ToDateTime(r("reconciliation_date"))
                Dim diff As Decimal = Convert.ToDecimal(r("difference"))

                ' Remarks column formatting
                Dim remarks As String
                If diff = 0D Then
                    remarks = "No variance"
                ElseIf diff < 0D Then
                    remarks = "Short " & Peso & Math.Abs(diff).ToString("N2")
                Else
                    remarks = "Over " & Peso & diff.ToString("N2")
                End If
                Dim typed As String = (Txt(r, "eod_remarks") & " " & Txt(r, "rem_remarks")).Trim()
                If typed <> "" Then remarks &= " - " & typed

                row.Cells("RemittanceNo").Value = r("remittance_no").ToString()
                row.Cells("nDate").Value = recDate.ToString("MMMM d, yyyy")
                row.Cells("CashierStaff").Value = r("cashier").ToString()
                row.Cells("TotalSales").Value = Peso & Convert.ToDecimal(r("total_sales")).ToString("N2")
                row.Cells("CashCollected").Value = Peso & Convert.ToDecimal(r("actual_cash")).ToString("N2")
                row.Cells("Difference").Value = Peso & Convert.ToDecimal(r("salary_deduction")).ToString("N2")
                row.Cells("TotalRemittance").Value = Peso & Convert.ToDecimal(r("cash_sales")).ToString("N2")
                row.Cells("ORARRange").Value = Txt(r, "or_from") & " - " & Txt(r, "or_to")
                row.Cells("AmtAccounting").Value = Peso & Convert.ToDecimal(r("remittance_amount")).ToString("N2")
                row.Cells("DateTimeRemitted").Value = If(IsDBNull(r("remitted_at")), "", Convert.ToDateTime(r("remitted_at")).ToString("MMMM d, yyyy - h:mm tt"))
                row.Cells("ReceivedBy").Value = Txt(r, "received_by")
                row.Cells("Remarks").Value = remarks
                row.Cells("Signature").Value = ""

                If diff <> 0D Then row.DefaultCellStyle.ForeColor = Color.Firebrick
            Next
        End If

        dgvsalesreport.ClearSelection()
    End Sub

    ' ==================== PRINT ====================
    Private Sub btnexportexcel_Click(sender As Object, e As EventArgs) Handles btnexportexcel.Click
        If dgvsalesreport.SelectedRows.Count = 0 Then
            MsgBox("Select a remittance record to print.", vbExclamation, "Remittance Report")
            Exit Sub
        End If
        printRow = dgvsalesreport.SelectedRows(0)

        Dim pd As New PrintDocument()
        AddHandler pd.PrintPage, AddressOf PrintRemittancePage

        Using dlg As New PrintPreviewDialog()
            dlg.Document = pd
            dlg.WindowState = FormWindowState.Maximized
            dlg.ShowDialog(Me)
        End Using
    End Sub

    Private Sub PrintRemittancePage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim left As Single = 60
        Dim y As Single = 60
        Dim pageW As Single = e.PageBounds.Width

        Using fTitle As New Font("Segoe UI", 15, FontStyle.Bold),
              fSub As New Font("Segoe UI", 11, FontStyle.Bold),
              fReg As New Font("Segoe UI", 10),
              fBold As New Font("Segoe UI", 10, FontStyle.Bold)

            Dim center As New StringFormat With {.Alignment = StringAlignment.Center}
            g.DrawString("LYCEUM OF ALABANG BOOKSTORE", fTitle, Brushes.Black, New RectangleF(0, y, pageW, 30), center)
            y += 32
            g.DrawString("REMITTANCE RECORD", fSub, Brushes.Black, New RectangleF(0, y, pageW, 24), center)
            y += 44

            Dim fields As New List(Of KeyValuePair(Of String, String)) From {
                New KeyValuePair(Of String, String)("Remittance No.", "RemittanceNo"),
                New KeyValuePair(Of String, String)("Date", "nDate"),
                New KeyValuePair(Of String, String)("Cashier / Staff", "CashierStaff"),
                New KeyValuePair(Of String, String)("Total Sales", "TotalSales"),
                New KeyValuePair(Of String, String)("Cash Collected", "CashCollected"),
                New KeyValuePair(Of String, String)("Salary Deduction", "Difference"),
                New KeyValuePair(Of String, String)("Total Remittance", "TotalRemittance"),
                New KeyValuePair(Of String, String)("OR / AR Range", "ORARRange"),
                New KeyValuePair(Of String, String)("Amount Remitted to Accounting", "AmtAccounting"),
                New KeyValuePair(Of String, String)("Date / Time Remitted", "DateTimeRemitted"),
                New KeyValuePair(Of String, String)("Received By", "ReceivedBy"),
                New KeyValuePair(Of String, String)("Remarks", "Remarks")
            }

            For Each f As KeyValuePair(Of String, String) In fields
                g.DrawString(f.Key & ":", fBold, Brushes.Black, left, y)
                g.DrawString(Convert.ToString(printRow.Cells(f.Value).Value), fReg, Brushes.Black, left + 260, y)
                y += 28
            Next

            y += 80
            Dim labels As String() = {"Cashier", "Bookstore Supervisor", "Accounting Staff"}
            For i As Integer = 0 To 2
                Dim x As Single = left + i * 220
                g.DrawLine(Pens.Black, x, y, x + 180, y)
                g.DrawString(labels(i), fReg, Brushes.Black, x, y + 4)
            Next
        End Using

        e.HasMorePages = False
    End Sub

End Class