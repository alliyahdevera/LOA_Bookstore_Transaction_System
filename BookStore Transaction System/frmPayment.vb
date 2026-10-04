' ======================================================================
' REPLACE THE WHOLE FILE: frmPayment.vb   (Settle Payment dialog)
'
' What changed
'   * Cash              -> Employee label / box / Search button are DISABLED and
'                          cleared. Nothing to enter there.
'   * Employee's Salary -> Employee box + Search are ENABLED and an employee
'                          is REQUIRED (type the name or use Search).
'                          Amount received is locked to the grand total.
'   * Re-opening the dialog after "Add Payment" shows the SAME details again
'     (same OR No., date, method, employee, amount) - frmPOS passes them in
'     through the Prefill* properties. Nothing is wiped until the sale is
'     saved or cancelled in the POS screen.
'   * Grand Total shows the peso sign.
' ======================================================================
Imports System.Globalization

Public Class frmPayment

    Private Const METHOD_CASH As String = "Cash"
    Private Const METHOD_SALARY As String = "Salary Deduction"

    Private ReadOnly Peso As String = ChrW(8369)
    Private lastWasSalary As Boolean = False

    Public Property GrandTotal As Decimal

    ' ===== previous payment (set by frmPOS when the cashier re-opens the dialog) =====
    Public Property PrefillORNo As String = ""
    Public Property PrefillDate As Date = Date.Today
    Public Property PrefillMethod As String = ""
    Public Property PrefillEmployee As String = ""
    Public Property PrefillReceived As Decimal = 0D

    ' ===== values sent back to frmPOS =====
    Public ReadOnly Property ResultORNo As String
        Get
            Return txtORNo.Text
        End Get
    End Property

    Public ReadOnly Property ResultDate As DateTime
        Get
            Return dtpORDate.Value.Date
        End Get
    End Property

    Public ReadOnly Property ResultMethod As String
        Get
            Return cboPaymentMethod.Text
        End Get
    End Property

    ' empty for Cash
    Public ReadOnly Property ResultEmployee As String
        Get
            If cboPaymentMethod.Text <> METHOD_SALARY Then Return ""
            Return txtEmployeeName.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property ResultReceived As Decimal
        Get
            Return ToMoney(txtAmountReceived.Text)
        End Get
    End Property

    Public ReadOnly Property ResultChange As Decimal
        Get
            Return ToMoney(txtChange.Text)
        End Get
    End Property

    ' ==================== LOAD ====================
    Private Sub frmPayment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim hasPrefill As Boolean = Not String.IsNullOrWhiteSpace(PrefillORNo)

        txtORNo.Text = If(hasPrefill, PrefillORNo, NewORNo())

        dtpORDate.MaxDate = Date.Today
        dtpORDate.Value = If(hasPrefill AndAlso PrefillDate.Date <= Date.Today, PrefillDate.Date, Date.Today)

        txtGrandTotal.Text = Peso & GrandTotal.ToString("N2")
        txtEmployeeName.MaxLength = 150

        cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList
        cboPaymentMethod.Items.Clear()
        cboPaymentMethod.Items.AddRange(New String() {METHOD_CASH, METHOD_SALARY})

        Dim idx As Integer = If(hasPrefill, cboPaymentMethod.Items.IndexOf(PrefillMethod), -1)
        cboPaymentMethod.SelectedIndex = If(idx >= 0, idx, 0)
        ApplyMethodState()

        If hasPrefill Then

            If cboPaymentMethod.Text = METHOD_SALARY Then
                txtEmployeeName.Text = PrefillEmployee
                txtAmountReceived.Text = GrandTotal.ToString("0.00")
            Else
                txtAmountReceived.Text = PrefillReceived.ToString("0.00")
            End If

        End If
        RecalculateChange()
    End Sub

    ' ==================== CASH vs SALARY DEDUCTION ====================
    Private Sub cboPaymentMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPaymentMethod.SelectedIndexChanged
        ApplyMethodState()
    End Sub

    Private Sub ApplyMethodState()

        Dim isSalary As Boolean =
        (cboPaymentMethod.Text = METHOD_SALARY)

        ' ================= EMPLOYEE =================
        Label2.Enabled = isSalary
        txtEmployeeName.Enabled = isSalary
        btnSearchEmployee.Enabled = isSalary

        If isSalary Then

            ' Salary Deduction
            txtEmployeeName.ReadOnly = False

            ' No cash is physically received
            txtAmountReceived.ReadOnly = True
            txtAmountReceived.Text = GrandTotal.ToString("0.00")

        Else

            ' Cash
            txtEmployeeName.Clear()
            txtEmployeeName.ReadOnly = True

            txtAmountReceived.ReadOnly = False

            ' Only clear it when switching from Salary Deduction
            If lastWasSalary Then
                txtAmountReceived.Clear()
            End If

        End If

        lastWasSalary = isSalary

        RecalculateChange()

    End Sub

    ' employee name: letters, spaces, dot, dash, apostrophe, comma
    Private Sub txtEmployeeName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmployeeName.KeyPress
        If Char.IsControl(e.KeyChar) OrElse Char.IsLetter(e.KeyChar) Then Exit Sub
        If " .-',".IndexOf(e.KeyChar) >= 0 Then Exit Sub
        e.Handled = True
    End Sub

    ' ===== Employee search (optional shortcut, salary deduction only) =====
    Private Sub btnSearchEmployee_Click(sender As Object, e As EventArgs) Handles btnSearchEmployee.Click
        Dim kw As String = InputBox("Enter the employee's name (leave blank to list all):", "Search Employee")
        Dim picked As String = PickEmployee(kw.Trim())
        If picked <> "" Then txtEmployeeName.Text = picked
    End Sub

    Private Function PickEmployee(keyword As String) As String
        Dim dt As DataTable = GetDataTable(
            "SELECT CONCAT(first_name, ' ', last_name) AS full_name FROM tbl_users " &
            "WHERE status = 'Active' AND (first_name LIKE @s OR last_name LIKE @s OR CONCAT(first_name, ' ', last_name) LIKE @s) " &
            "ORDER BY last_name, first_name",
            New String() {"@s"}, New Object() {"%" & keyword & "%"})

        If dt.Rows.Count = 0 Then
            MsgBox("No employee found. You can also type the name directly.", vbInformation, "Search Employee")
            Return ""
        End If
        If dt.Rows.Count = 1 Then Return dt.Rows(0)("full_name").ToString()

        Using dlg As New Form(), lst As New ListBox()
            dlg.Text = "Select Employee (double-click)"
            dlg.StartPosition = FormStartPosition.CenterParent
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog
            dlg.MinimizeBox = False
            dlg.MaximizeBox = False
            dlg.ClientSize = New Size(340, 280)

            lst.Dock = DockStyle.Fill
            lst.Font = New Font("Segoe UI", 10.5F)
            lst.DataSource = dt
            lst.DisplayMember = "full_name"
            AddHandler lst.DoubleClick, Sub(s As Object, ev As EventArgs) dlg.DialogResult = DialogResult.OK
            dlg.Controls.Add(lst)

            If dlg.ShowDialog(Me) = DialogResult.OK AndAlso lst.SelectedIndex >= 0 Then
                Return CType(lst.SelectedItem, DataRowView)("full_name").ToString()
            End If
        End Using
        Return ""
    End Function

    ' ===== NO NEGATIVES: only digits and one decimal point can be typed =====
    Private Sub txtAmountReceived_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAmountReceived.KeyPress
        If Char.IsControl(e.KeyChar) Then Exit Sub
        If Char.IsDigit(e.KeyChar) Then Exit Sub
        If e.KeyChar = "."c AndAlso Not txtAmountReceived.Text.Contains(".") Then Exit Sub
        e.Handled = True
    End Sub

    Private Sub txtAmountReceived_TextChanged(sender As Object, e As EventArgs) Handles txtAmountReceived.TextChanged
        If txtAmountReceived.Text.Contains("-") Then
            Dim pos As Integer = txtAmountReceived.SelectionStart
            txtAmountReceived.Text = txtAmountReceived.Text.Replace("-", "")
            txtAmountReceived.SelectionStart = Math.Min(pos, txtAmountReceived.Text.Length)
            Exit Sub
        End If
        RecalculateChange()
    End Sub

    Private Sub RecalculateChange()
        Dim received As Decimal = ToMoney(txtAmountReceived.Text)
        If received >= GrandTotal AndAlso txtAmountReceived.Text.Trim() <> "" Then
            txtChange.Text = (received - GrandTotal).ToString("N2")
        Else
            txtChange.Text = "0.00"
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If GrandTotal <= 0 Then
            MsgBox("There is nothing to pay.",
               vbExclamation,
               "Settle Payment")
            Exit Sub
        End If

        If cboPaymentMethod.SelectedIndex = -1 Then
            MsgBox("Select a payment method.",
               vbExclamation,
               "Settle Payment")
            Exit Sub
        End If

        ' ================= SALARY DEDUCTION =================
        If cboPaymentMethod.Text = METHOD_SALARY Then

            If String.IsNullOrWhiteSpace(txtEmployeeName.Text) Then

                MsgBox("Salary Deduction requires an employee. " &
                   "Type the employee's name or click Search.",
                   vbExclamation,
                   "Settle Payment")

                txtEmployeeName.Focus()
                Exit Sub

            End If

            ' Salary deduction is exactly the grand total
            txtAmountReceived.Text = GrandTotal.ToString("0.00")
            txtChange.Text = "0.00"

        End If

        ' ================= CASH =================
        If cboPaymentMethod.Text = METHOD_CASH Then

            Dim received As Decimal

            If Not Decimal.TryParse(
            txtAmountReceived.Text,
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            received) Then

                MsgBox("Enter a valid amount received.",
                   vbExclamation,
                   "Settle Payment")

                txtAmountReceived.Focus()
                Exit Sub

            End If

            If received < GrandTotal Then

                MsgBox("Amount received is less than the Grand Total.",
                   vbExclamation,
                   "Settle Payment")

                txtAmountReceived.Focus()
                Exit Sub

            End If

        End If

        ' Return the payment details to frmPOS.
        ' DO NOT CLEAR THE FORM HERE.
        Me.DialogResult = DialogResult.OK
        Me.Close()

    End Sub

    ' accepts "1,234.50", "₱1,234.50" or "1234.5"
    Private Function ToMoney(s As String) As Decimal
        If s Is Nothing Then Return 0D
        Dim v As Decimal
        Dim clean As String = s.Replace(Peso, "").Trim()
        If Decimal.TryParse(clean, NumberStyles.Number, CultureInfo.CurrentCulture, v) Then Return v
        Return 0D
    End Function

End Class