Imports System.Globalization
Imports MySql.Data.MySqlClient

Public Class frmCashDenomination

    Private Const METHOD_SALARY As String = "Employee's Salary"
    Private Const SESSION_FILTER As String = "created_by = @u AND DATE(created_at) = CURDATE() AND status <> 'Cancelled' "

    Private ReadOnly Peso As String = ChrW(8369)
    Private ReadOnly Denominations As Decimal() = New Decimal() {1000D, 500D, 200D, 100D, 50D, 20D, 10D, 5D, 1D, 0.25D}

    Private cashSales As Decimal = 0D
    Private salaryDeduction As Decimal = 0D
    Private cashCount As Integer = 0
    Private salaryCount As Integer = 0
    Private isLoading As Boolean = True
    Private Sub frmCashDenomination_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isLoading = True

        cboFrom.DropDownStyle = ComboBoxStyle.DropDownList
        cboTo.DropDownStyle = ComboBoxStyle.DropDownList

        txtCashier.Text = currentuser.FullName
        txtRemarksr.MaxLength = 255
        txtremarkrs.MaxLength = 255
        TextBox1.MaxLength = 150

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
                .Rows(idx).Tag = d
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
    Private Sub LoadSummary()
        cashSales = 0D : salaryDeduction = 0D : cashCount = 0 : salaryCount = 0
        cboFrom.Items.Clear()
        cboTo.Items.Clear()

        Dim uid As Object = currentuser.UserID

        Dim dt As DataTable = GetDataTable(
        "SELECT payment_method, COUNT(*) AS cnt, IFNULL(SUM(total_amount), 0) AS amt " &
        "FROM tbl_transactions WHERE " & SESSION_FILTER & "GROUP BY payment_method",
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
        "SELECT or_no FROM tbl_transactions WHERE " & SESSION_FILTER & "ORDER BY transaction_id",
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
        txtrefno.Text = "REM-" & DateTime.Now.ToString("yyyyMMddHHmmss")
        TextBox1.Clear()
        txtRemarksr.Clear()
        txtremarkrs.Clear()
        TextBox7.Text = Date.Today.ToString("MMMM d, yyyy")
        TextBox5.Text = DateTime.Now.ToString("h:mm tt")
        SetupDenominationGrid()
        RecalculateActual()
    End Sub

    ' ==================== GENERATE REMITTANCE ====================
    Private Sub btnSaveTransaction_Click(sender As Object, e As EventArgs) Handles btnSaveTransaction.Click
        If RolePermissions.IsViewOnly() Then
            MsgBox("This cashier has no transactions today. Nothing to reconcile.", vbExclamation, "Remittance")
            Exit Sub
        End If
        If cashCount + salaryCount = 0 Then
            MsgBox("This cashier has no transactions for the selected shift today. Nothing to process.", vbInformation, "Remittance")
            Exit Sub
        End If

        Dim actual As Decimal = ParseMoney(txtactualc.Text)
        If actual <= 0D AndAlso cashSales > 0D Then
            MsgBox("Enter the cash denomination count first.", vbExclamation, "Remittance")
            Exit Sub
        End If

        Dim diff As Decimal = actual - cashSales
        If diff <> 0D AndAlso String.IsNullOrWhiteSpace(txtRemarksr.Text) Then
            MsgBox("The cash is " & txtstatus.Text & " by " & Peso & Math.Abs(diff).ToString("N2") &
                   ". Enter the reason in Remarks.", vbExclamation, "Remittance")
            txtRemarksr.Focus()
            Exit Sub
        End If
        If cboFrom.SelectedIndex < 0 OrElse cboTo.SelectedIndex < 0 Then
            MsgBox("Select the OR/AR range.", vbExclamation, "Remittance")
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MsgBox("Enter who received the remittance (Received By).", vbExclamation, "Remittance")
            TextBox1.Focus()
            Exit Sub
        End If

        ' Check if user already processed a remittance for today
        Dim already As Integer = Convert.ToInt32(If(ExecScalar(
            "SELECT COUNT(*) FROM tbl_remittances WHERE prepared_by = @c AND DATE(remitted_at) = CURDATE()",
            New String() {"@c"}, New Object() {currentuser.UserID}), 0))
        If already > 0 Then
            MsgBox("This cashier already has a remittance record for today. See Remittance Report.", vbExclamation, "Remittance")
            Exit Sub
        End If

        If MsgBox("Save the remittance record?" & vbCrLf & vbCrLf &
                  "Expected cash: " & Peso & cashSales.ToString("N2") & vbCrLf &
                  "Actual cash: " & Peso & actual.ToString("N2") & vbCrLf &
                  "Status: " & txtstatus.Text, vbYesNo + vbQuestion, "Generate Remittance") <> MsgBoxResult.Yes Then Exit Sub

        Dim remittanceNo As String = txtrefno.Text.Trim()
        Dim statusText As String = txtstatus.Text
        Dim remitRemarks As String = txtremarkrs.Text.Trim()

        Try
            Using c As MySqlConnection = NewConnection()
                c.Open()
                Using tx As MySqlTransaction = c.BeginTransaction()
                    Try
                        Dim remittanceId As Long

                        ' Clean INSERT query into tbl_remittances (end_of_day_id removed)
                        Using q As New MySqlCommand(
                            "INSERT INTO tbl_remittances (remittance_no, prepared_by, verified_by, remittance_amount, status, " &
                            "prepared_at, remarks, or_from, or_to, received_by, remitted_at) " &
                            "VALUES (@no, @by, NULL, @amt, @st, NOW(), @rm, @f, @t, @rb, NOW())", c, tx)
                            q.Parameters.AddWithValue("@no", remittanceNo)
                            q.Parameters.AddWithValue("@by", currentuser.UserID)
                            q.Parameters.AddWithValue("@amt", actual)
                            q.Parameters.AddWithValue("@st", statusText)
                            q.Parameters.AddWithValue("@rm", If(remitRemarks = "", CType(DBNull.Value, Object), remitRemarks))
                            q.Parameters.AddWithValue("@f", cboFrom.Text)
                            q.Parameters.AddWithValue("@t", cboTo.Text)
                            q.Parameters.AddWithValue("@rb", TextBox1.Text.Trim())
                            q.ExecuteNonQuery()
                            remittanceId = q.LastInsertedId
                        End Using

                        ' Insert cash breakdown into tbl_cash_denominations using remittanceId
                        For Each row As DataGridViewRow In dgvcashbreakdown.Rows
                            Dim qty As Integer = 0
                            Integer.TryParse(Convert.ToString(row.Cells("Quantity").Value), qty)
                            If qty <= 0 Then Continue For
                            Dim denom As Decimal = CDec(row.Tag)

                            Using q As New MySqlCommand(
                                "INSERT INTO tbl_cash_denominations (end_of_day_id, denomination, quantity, amount) VALUES (@e, @d, @q, @a)", c, tx)
                                q.Parameters.AddWithValue("@e", remittanceId)
                                q.Parameters.AddWithValue("@d", denom)
                                q.Parameters.AddWithValue("@q", qty)
                                q.Parameters.AddWithValue("@a", denom * qty)
                                q.ExecuteNonQuery()
                            End Using
                        Next

                        tx.Commit()
                    Catch
                        tx.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            LogActivity("Remittance", remittanceNo,
                        "Remittance " & remittanceNo & " - Expected " & cashSales.ToString("N2") & ", Actual " & actual.ToString("N2") & " (" & statusText & ")")

            MsgBox("Remittance " & remittanceNo & " generated successfully.", vbInformation, "Remittance")

            Dim reports As frmReports = TryCast(Me.Parent?.FindForm(), frmReports)
            If reports IsNot Nothing Then reports.OpenRemittance()

        Catch ex As Exception
            MsgBox("Saving failed and was rolled back: " & ex.Message, vbCritical, "Remittance")
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