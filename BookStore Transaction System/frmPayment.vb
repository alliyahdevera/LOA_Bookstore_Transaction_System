Imports System.Globalization

Public Class frmPayment

    Private Const METHOD_CASH As String = "Cash"
    Private Const METHOD_SALARY As String = "Salary Deduction"

    Private ReadOnly Peso As String = ChrW(8369)
    Private isInitializing As Boolean = True

    Public Property GrandTotal As Decimal

    Public Property AllowSalaryDeduction As Boolean = False
    Public Property SalaryEmployeeName As String = ""

    Public Property PrefillORNo As String = ""
    Public Property PrefillDate As Date = Date.Today
    Public Property PrefillMethod As String = ""
    Public Property PrefillEmployee As String = ""
    Public Property PrefillReceived As Decimal = 0D

    Public ReadOnly Property ResultORNo As String
        Get
            If IsSalary() Then Return ""
            Return txtORNo.Text.Trim()
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

    Public ReadOnly Property ResultEmployee As String
        Get
            Return If(IsSalary(), SalaryEmployeeName, "")
        End Get
    End Property

    Public ReadOnly Property ResultReceived As Decimal
        Get
            If IsSalary() Then Return 0D
            Return ToMoney(txtAmountReceived.Text)
        End Get
    End Property

    Public ReadOnly Property ResultChange As Decimal
        Get
            If IsSalary() Then Return 0D
            Return ToMoney(txtChange.Text)
        End Get
    End Property

    Private Function IsSalary() As Boolean
        Return cboPaymentMethod.Text = METHOD_SALARY
    End Function

    Private Sub frmPayment_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        isInitializing = True

        txtGrandTotal.Text = Peso & GrandTotal.ToString("N2")
        txtEmployeeName.ReadOnly = True
        txtEmployeeName.MaxLength = 150
        txtORNo.MaxLength = 30

        dtpORDate.MaxDate = Date.Today
        dtpORDate.Value =
            If(PrefillDate.Date >= dtpORDate.MinDate.Date AndAlso
               PrefillDate.Date <= Date.Today,
               PrefillDate.Date,
               Date.Today)

        cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList
        cboPaymentMethod.Items.Clear()
        cboPaymentMethod.Items.Add(METHOD_CASH)

        If AllowSalaryDeduction AndAlso
            Not String.IsNullOrWhiteSpace(SalaryEmployeeName) Then

            cboPaymentMethod.Items.Add(METHOD_SALARY)
        End If

        Dim selected As Integer =
            cboPaymentMethod.Items.IndexOf(PrefillMethod)

        cboPaymentMethod.SelectedIndex =
            If(selected >= 0, selected, 0)

        txtORNo.Text =
            If(String.IsNullOrWhiteSpace(PrefillORNo),
               NewORNo(),
               PrefillORNo.Trim())

        txtAmountReceived.Text =
            If(PrefillMethod = METHOD_CASH,
               PrefillReceived.ToString("N2"),
               "")

        isInitializing = False
        ApplyMethodState()
    End Sub

    Private Sub cboPaymentMethod_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    ) Handles cboPaymentMethod.SelectedIndexChanged

        If isInitializing Then Exit Sub
        ApplyMethodState()
    End Sub

    Private Sub ApplyMethodState()
        Dim salary As Boolean = IsSalary()

        txtEmployeeName.Text =
            If(salary, SalaryEmployeeName, "")

        Label2.Enabled = salary
        txtEmployeeName.Enabled = salary
        txtEmployeeName.ReadOnly = True

        ' Employee selection is done in the main POS buyer panel.
        btnSearchEmployee.Enabled = False

        txtORNo.Enabled = Not salary
        dtpORDate.Enabled = Not salary
        txtAmountReceived.ReadOnly = salary
        txtChange.ReadOnly = True

        If salary Then
            txtORNo.Clear()
            txtAmountReceived.Text = "0.00"
            txtChange.Text = "0.00"
            btnSave.Text = "Save Deduction Request"
        Else
            If String.IsNullOrWhiteSpace(txtORNo.Text) Then
                txtORNo.Text = NewORNo()
            End If

            btnSave.Text = "Save Payment"
            RecalculateChange()
        End If
    End Sub

    Private Sub txtAmountReceived_KeyPress(
        sender As Object,
        e As KeyPressEventArgs
    ) Handles txtAmountReceived.KeyPress

        If Char.IsControl(e.KeyChar) OrElse
            Char.IsDigit(e.KeyChar) Then
            Exit Sub
        End If

        If e.KeyChar = "."c AndAlso
            Not txtAmountReceived.Text.Contains(".") Then
            Exit Sub
        End If

        e.Handled = True
    End Sub

    Private Sub txtAmountReceived_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtAmountReceived.TextChanged

        If isInitializing Then Exit Sub
        RecalculateChange()
    End Sub

    Private Sub RecalculateChange()
        If IsSalary() Then
            txtChange.Text = "0.00"
            Exit Sub
        End If

        Dim received As Decimal = ToMoney(txtAmountReceived.Text)

        txtChange.Text =
            Math.Max(0D, received - GrandTotal).ToString("N2")
    End Sub

    Private Sub btnSave_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnSave.Click

        If GrandTotal <= 0D Then
            MsgBox(
                "There is nothing to pay.",
                vbExclamation,
                "Payment")
            Exit Sub
        End If

        If IsSalary() Then
            If Not AllowSalaryDeduction OrElse
                String.IsNullOrWhiteSpace(SalaryEmployeeName) Then

                MsgBox(
                    "Salary deduction requires an employee buyer " &
                    "and a cart containing only books or uniforms.",
                    vbExclamation,
                    "Salary Deduction")
                Exit Sub
            End If

            txtAmountReceived.Text = "0.00"
            txtChange.Text = "0.00"

            MsgBox(
                "The deduction request is ready." &
                vbCrLf &
                "Save the transaction in POS to print the finance slip." &
                vbCrLf &
                "It will remain unpaid until finance approval " &
                "is recorded by the supervisor.",
                vbInformation,
                "Salary Deduction")
        Else
            Dim received As Decimal

            If Not Decimal.TryParse(
                txtAmountReceived.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                received) OrElse received < GrandTotal Then

                MsgBox(
                    "Enter an amount received that covers the total.",
                    vbExclamation,
                    "Payment")
                Exit Sub
            End If

            If String.IsNullOrWhiteSpace(txtORNo.Text) Then
                MsgBox(
                    "Enter the receipt number.",
                    vbExclamation,
                    "Payment")
                Exit Sub
            End If

            RecalculateChange()
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Function ToMoney(value As String) As Decimal
        Dim amount As Decimal

        If Decimal.TryParse(
            If(value, "").Replace(Peso, "").Trim(),
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            amount) Then

            Return amount
        End If

        Return 0D
    End Function

End Class