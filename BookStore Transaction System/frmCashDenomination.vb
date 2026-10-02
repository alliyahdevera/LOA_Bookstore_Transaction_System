' ======================================================================
' REPLACE THE WHOLE FILE: frmCashDenomination.vb   (End-Of-Day Reconciliation)
' Needs 01_database_updates.sql to be run first.
'
' Flow (matches the supervisor's Q7 + Follow-up Q6):
'   1) Pick the cashier (a Cashier is locked to himself/herself) and shift.
'      Today's system sales for that cashier fill in automatically.
'   2) Type the quantity of each bill/coin. Actual cash, difference and
'      status (Balanced / Short / Over) compute by themselves.
'   3) Fill in OR/AR range, Received By, and remarks (remarks required when
'      Short/Over), then click "Generate Remittance".
'   4) Saves tbl_end_of_day + tbl_cash_denominations + tbl_remittances in one
'      transaction, then opens the Remittance Report.
'
' CONTROL MAPPING I ASSUMED (from the designer positions) - please confirm:
'   txtRemarksr = reconciliation remarks      txtremarkrs = remittance remarks
'   TextBox4    = Amt Remitted to Accounting  TextBox1    = Received By
'   TextBox7    = Date Remitted               TextBox5    = Time Remitted
'   cboFrom / cboTo = OR/AR From / To         txtrefno    = Reference No.
' ======================================================================
Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class frmCashDenomination

    Private Const METHOD_SALARY As String = "Salary Deduction"

    Private ReadOnly Peso As String = ChrW(8369)
    Private ReadOnly Denominations As Decimal() = New Decimal() {1000D, 500D, 200D, 100D, 50D, 20D, 10D, 5D, 1D, 0.25D}

    Private cashSales As Decimal = 0D
    Private salaryDeduction As Decimal = 0D
    Private cashCount As Integer = 0
    Private salaryCount As Integer = 0
    Private isLoading As Boolean = True

    ' ==================== LOAD ====================
    Private Sub frmCashDenomination_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboFrom.DropDownStyle = ComboBoxStyle.DropDownList
        cboTo.DropDownStyle = ComboBoxStyle.DropDownList

        txtcashier.ReadOnly = True

        Dim cashierDt As DataTable = GetDataTable(
        "SELECT CONCAT(first_name, ' ', last_name) AS full_name " &
        "FROM tbl_users " &
        "WHERE user_id = @u",
        New String() {"@u"},
        New Object() {currentuser.UserID})

        If cashierDt.Rows.Count > 0 Then
            txtcashier.Text = cashierDt.Rows(0)("full_name").ToString()
        Else
            txtcashier.Text = currentuser.UserID.ToString()
        End If

        ' typed boxes
        txtRemarksr.ReadOnly = False : txtRemarksr.MaxLength = 255
        txtremarkrs.ReadOnly = False : txtremarkrs.MaxLength = 255
        TextBox1.ReadOnly = False : TextBox1.MaxLength = 150

        Dim dt As DataTable = GetDataTable(
            "SELECT u.user_id, CONCAT(u.first_name, ' ', u.last_name) AS full_name " &
            "FROM tbl_users u INNER JOIN tbl_roles r ON u.role_id = r.role_id " &
            "WHERE u.status = 'Active' AND r.role_name IN ('Cashier', 'Bookstore Supervisor') " &
            "ORDER BY u.last_name, u.first_name")

        isLoading = False
        ResetEntryFields()
        LoadSummary()
    End Sub

    ' ==================== DENOMINATION GRID ====================
    Private Sub SetupDenominationGrid()
        With dgvcashbreakdown
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = False
            .Rows.Clear()
            For Each d As Decimal In Denominations
                Dim idx As Integer = .Rows.Add()
                .Rows(idx).Cells("Denomination").Value = d.ToString("N2")
                .Rows(idx).Cells("Quantity").Value = 0
                .Rows(idx).Cells("Amount").Value = "0.00"
                .Rows(idx).Tag = d                       ' keep the real number
            Next
            .Columns("Denomination").ReadOnly = True
            .Columns("Quantity").ReadOnly = False
            .Columns("Amount").ReadOnly = True
            .ClearSelection()
        End With
    End Sub

    Private Sub dgvcashbreakdown_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvcashbreakdown.EditingControlShowing
        Dim tb As TextBox = TryCast(e.Control, TextBox)
        If tb Is Nothing Then Exit Sub
        RemoveHandler tb.KeyPress, AddressOf DigitsOnly_KeyPress
        AddHandler tb.KeyPress, AddressOf DigitsOnly_KeyPress
    End Sub

    Private Sub DigitsOnly_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub dgvcashbreakdown_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvcashbreakdown.CellEndEdit
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = dgvcashbreakdown.Rows(e.RowIndex)
        Dim qty As Integer
        If Not Integer.TryParse(Convert.ToString(row.Cells("Quantity").Value), qty) OrElse qty < 0 Then qty = 0
        row.Cells("Quantity").Value = qty
        row.Cells("Amount").Value = (CDec(row.Tag) * qty).ToString("N2")
        RecalculateActual()
    End Sub

    ' ==================== TODAY'S SYSTEM SUMMARY ====================
    Private Sub LoadSummary()
        cashSales = 0D : salaryDeduction = 0D : cashCount = 0 : salaryCount = 0
        cboFrom.Items.Clear()
        cboTo.Items.Clear()

        Dim uid As Integer = currentuser.UserID

        Dim dt As DataTable = GetDataTable(
                        "SELECT payment_method, COUNT(*) AS cnt, IFNULL(SUM(total_amount), 0) AS amt " &
                        "FROM tbl_transactions " &
                        "WHERE created_by = @u AND or_date = CURDATE() AND status <> 'Cancelled' " &
                        "GROUP BY payment_method",
                        New String() {"@u"}, New Object() {uid})

        For Each r As DataRow In dt.Rows
                Dim cnt As Integer = Convert.ToInt32(r("cnt"))
                Dim amt As Decimal = Convert.ToDecimal(r("amt"))
                If r("payment_method").ToString() = METHOD_SALARY Then
                    salaryCount += cnt : salaryDeduction += amt
                Else
                    cashCount += cnt : cashSales += amt
                End If
            Next

            Dim ors As DataTable = GetDataTable(
                "SELECT or_no FROM tbl_transactions " &
                "WHERE created_by = @u AND or_date = CURDATE() AND status <> 'Cancelled' ORDER BY transaction_id",
                New String() {"@u"}, New Object() {uid})

            For Each r As DataRow In ors.Rows
                cboFrom.Items.Add(r("or_no").ToString())
                cboTo.Items.Add(r("or_no").ToString())
            Next
            If cboFrom.Items.Count > 0 Then
                cboFrom.SelectedIndex = 0
                cboTo.SelectedIndex = cboTo.Items.Count - 1
            End If

        lblcashsales.Text = Peso & cashSales.ToString("N2")
        lblsaldec.Text = Peso & salaryDeduction.ToString("N2")
        lbltotsales.Text = Peso & (cashSales + salaryDeduction).ToString("N2")
        lblcashtc.Text = cashCount.ToString("N0")
        lblsaldc.Text = salaryCount.ToString("N0")
        lbltottransac.Text = (cashCount + salaryCount).ToString("N0")
        txtexpected.Text = cashSales.ToString("N2")

        RecalculateActual()
    End Sub

    ' ==================== ACTUAL vs EXPECTED ====================
    Private Sub RecalculateActual()
        Dim total As Decimal = 0D
        For Each row As DataGridViewRow In dgvcashbreakdown.Rows
            Dim qty As Integer = 0
            Integer.TryParse(Convert.ToString(row.Cells("Quantity").Value), qty)
            If row.Tag IsNot Nothing Then total += CDec(row.Tag) * qty
        Next

        txtactualc.Text = total.ToString("N2")
        lblamtcash.Text = Peso & total.ToString("N2")
        TextBox4.Text = total.ToString("N2")

        Dim diff As Decimal = total - cashSales
        txtdiff.Text = diff.ToString("N2")

        If diff = 0D Then
            txtstatus.Text = "Balanced"
            txtstatus.ForeColor = Color.SeaGreen
        ElseIf diff < 0D Then
            txtstatus.Text = "Short"
            txtstatus.ForeColor = Color.Firebrick
        Else
            txtstatus.Text = "Over"
            txtstatus.ForeColor = Color.DarkOrange
        End If
    End Sub

    Private Function ParseMoney(s As String) As Decimal
        Dim v As Decimal
        If Decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, v) Then Return v
        Return 0D
    End Function

    Private Sub ResetEntryFields()
        txtrefno.Text = "EOD-" & DateTime.Now.ToString("yyyyMMddHHmmss")
        TextBox1.Clear()
        txtRemarksr.Clear()
        txtremarkrs.Clear()
        TextBox7.Text = Date.Today.ToString("MMMM d, yyyy")
        TextBox5.Text = DateTime.Now.ToString("h:mm tt")
        SetupDenominationGrid()
        RecalculateActual()
    End Sub

    ' ==================== GENERATE REMITTANCE ====================
    Private Sub btnSaveTransaction_Click(sender As Object, e As EventArgs) Handles btnSaveTransaction.Click   ' Generate Remittance
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "End-Of-Day Reconciliation")
            Exit Sub
        End If
        If cashCount + salaryCount = 0 Then
            MsgBox("This cashier has no transactions today. Nothing to reconcile.", vbInformation, "End-Of-Day Reconciliation")
            Exit Sub
        End If

        Dim actual As Decimal = ParseMoney(txtactualc.Text)
        If actual <= 0D AndAlso cashSales > 0D Then
            MsgBox("Enter the cash denomination count first.", vbExclamation, "End-Of-Day Reconciliation")
            Exit Sub
        End If

        Dim diff As Decimal = actual - cashSales
        If diff <> 0D AndAlso String.IsNullOrWhiteSpace(txtRemarksr.Text) Then
            MsgBox("The cash is " & txtstatus.Text & " by " & Peso & Math.Abs(diff).ToString("N2") &
                   ". Enter the reason in Remarks (Reconciliation).", vbExclamation, "End-Of-Day Reconciliation")
            txtRemarksr.Focus()
            Exit Sub
        End If
        If cboFrom.SelectedIndex < 0 OrElse cboTo.SelectedIndex < 0 Then
            MsgBox("Select the OR/AR range.", vbExclamation, "End-Of-Day Reconciliation")
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MsgBox("Enter who received the remittance (Received By).", vbExclamation, "End-Of-Day Reconciliation")
            TextBox1.Focus()
            Exit Sub
        End If

        Dim already As Integer = Convert.ToInt32(If(ExecScalar(
            "SELECT COUNT(*) FROM tbl_end_of_day WHERE cashier_id = @c AND reconciliation_date = CURDATE()",
            New String() {"@c"}, New Object() {currentuser.UserID}), 0))
        If already > 0 Then
            MsgBox("This cashier already has an end-of-day record for today. See Remittance Report.", vbExclamation, "End-Of-Day Reconciliation")
            Exit Sub
        End If

        If MsgBox("Save the end-of-day record and generate the remittance?" & vbCrLf & vbCrLf &
                  "Expected cash: " & Peso & cashSales.ToString("N2") & vbCrLf &
                  "Actual cash: " & Peso & actual.ToString("N2") & vbCrLf &
                  "Status: " & txtstatus.Text, vbYesNo + vbQuestion, "Generate Remittance") <> MsgBoxResult.Yes Then Exit Sub

        Dim reconNo As String = txtrefno.Text.Trim()
        Dim remittanceNo As String = "REM-" & DateTime.Now.ToString("yyyyMMddHHmmssfff")
        Dim statusText As String = txtstatus.Text
        Dim reconRemarks As String = txtRemarksr.Text.Trim()
        Dim remitRemarks As String = txtremarkrs.Text.Trim()

        Try
            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        Dim eodId As Long

                        Using q As New MySqlCommand(
                            "INSERT INTO tbl_end_of_day (reconciliation_no, reconciliation_date, cashier_id, cash_sales, salary_deduction, " &
                            "total_sales, cash_transaction_count, salary_deduction_count, expected_cash, actual_cash, difference, status, remarks) " &
                            "VALUES (@no, CURDATE(), @cid, @cs, @sd, @ts, @cc, @sc, @exp, @act, @diff, @st, @rm)", c, tx)
                            q.Parameters.AddWithValue("@no", reconNo)
                            q.Parameters.AddWithValue("@cid", currentuser.UserID)
                            q.Parameters.AddWithValue("@cs", cashSales)
                            q.Parameters.AddWithValue("@sd", salaryDeduction)
                            q.Parameters.AddWithValue("@ts", cashSales + salaryDeduction)
                            q.Parameters.AddWithValue("@cc", cashCount)
                            q.Parameters.AddWithValue("@sc", salaryCount)
                            q.Parameters.AddWithValue("@exp", cashSales)
                            q.Parameters.AddWithValue("@act", actual)
                            q.Parameters.AddWithValue("@diff", diff)
                            q.Parameters.AddWithValue("@st", statusText)
                            q.Parameters.AddWithValue("@rm", If(reconRemarks = "", CType(DBNull.Value, Object), reconRemarks))
                            q.ExecuteNonQuery()
                            eodId = q.LastInsertedId
                        End Using

                        For Each row As DataGridViewRow In dgvcashbreakdown.Rows
                            Dim qty As Integer = 0
                            Integer.TryParse(Convert.ToString(row.Cells("Quantity").Value), qty)
                            If qty <= 0 Then Continue For
                            Dim denom As Decimal = CDec(row.Tag)

                            Using q As New MySqlCommand(
                                "INSERT INTO tbl_cash_denominations (end_of_day_id, denomination, quantity, amount) VALUES (@e, @d, @q, @a)", c, tx)
                                q.Parameters.AddWithValue("@e", eodId)
                                q.Parameters.AddWithValue("@d", denom)
                                q.Parameters.AddWithValue("@q", qty)
                                q.Parameters.AddWithValue("@a", denom * qty)
                                q.ExecuteNonQuery()
                            End Using
                        Next

                        Using q As New MySqlCommand(
                            "INSERT INTO tbl_remittances (remittance_no, end_of_day_id, prepared_by, verified_by, remittance_amount, status, " &
                            "prepared_at, remarks, or_from, or_to, received_by, remitted_at) " &
                            "VALUES (@no, @e, @by, NULL, @amt, 'Pending', NOW(), @rm, @f, @t, @rb, NOW())", c, tx)
                            q.Parameters.AddWithValue("@no", remittanceNo)
                            q.Parameters.AddWithValue("@e", eodId)
                            q.Parameters.AddWithValue("@by", currentuser.UserID)
                            q.Parameters.AddWithValue("@amt", actual)
                            q.Parameters.AddWithValue("@rm", If(remitRemarks = "", CType(DBNull.Value, Object), remitRemarks))
                            q.Parameters.AddWithValue("@f", cboFrom.Text)
                            q.Parameters.AddWithValue("@t", cboTo.Text)
                            q.Parameters.AddWithValue("@rb", TextBox1.Text.Trim())
                            q.ExecuteNonQuery()
                        End Using

                        tx.Commit()
                    Catch
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            LogActivity("End of Day", reconNo,
                        "Remittance " & remittanceNo & " - Expected " & cashSales.ToString("N2") & ", Actual " & actual.ToString("N2") & " (" & statusText & ")")

            MsgBox("Remittance " & remittanceNo & " generated.", vbInformation, "End-Of-Day Reconciliation")

            Dim reports As frmReports = TryCast(Me.Parent?.FindForm(), frmReports)
            If reports IsNot Nothing Then reports.OpenRemittance()

        Catch ex As Exception
            MsgBox("Saving failed and was rolled back: " & ex.Message, vbCritical, "End-Of-Day Reconciliation")
        End Try
    End Sub

    ' ==================== CLEAR / CANCEL ====================
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ResetEntryFields()
        LoadSummary()
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        btnClear.PerformClick()
    End Sub

End Class