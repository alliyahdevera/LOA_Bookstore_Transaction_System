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
    Private Sub cboPaymentMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPaymentMethod.SelectedIndexChanged
        ApplyMethodState()
        Me.BeginInvoke(New Action(AddressOf OpenListIfNeeded))
    End Sub

    Private Sub ApplyMethodState()

        Dim isSalary As Boolean =
        (cboPaymentMethod.Text = METHOD_SALARY)

        ' ================= EMPLOYEE =================
        Label2.Enabled = isSalary
        txtEmployeeName.Enabled = isSalary
        btnSearchEmployee.Enabled = isSalary

        If isSalary Then
            txtEmployeeName.ReadOnly = True          ' pick from the Employee List, no typing

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
    ' Salary Deduction: the employee must come from the Employee List
    Private Sub btnSearchEmployee_Click(sender As Object, e As EventArgs) Handles btnSearchEmployee.Click
        PickEmployeeFromList()
    End Sub

    Private Sub PickEmployeeFromList()
        Using frm As New frmEmployeeList()
            frm.InitialSearch = txtEmployeeName.Text.Trim()
            frm.StartPosition = FormStartPosition.CenterParent
            If frm.ShowDialog(Me) = DialogResult.OK AndAlso frm.SelectedEmployeeId > 0 Then
                txtEmployeeName.Text = frm.SelectedEmployeeName
            End If
        End Using
    End Sub

    ' opens the list right after Salary Deduction is chosen
    Private Sub OpenListIfNeeded()
        If cboPaymentMethod.Text = METHOD_SALARY AndAlso txtEmployeeName.Text.Trim() = "" Then PickEmployeeFromList()
    End Sub
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