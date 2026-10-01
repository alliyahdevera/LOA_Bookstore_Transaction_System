Imports System.Globalization

Public Class frmPayment

    Private Const METHOD_CASH As String = "Cash"
    Private Const METHOD_SALARY As String = "Employee's Salary"

    Public Property GrandTotal As Decimal

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

    Public ReadOnly Property ResultEmployee As String
        Get
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

    Private Sub frmPayment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtORNo.Text = NewORNo()
        txtORNo.ReadOnly = True

        dtpORDate.MaxDate = Date.Today
        dtpORDate.Value = Date.Today

        txtGrandTotal.Text = GrandTotal.ToString("N2")   ' comes from frmPOS
        txtGrandTotal.ReadOnly = True
        txtChange.ReadOnly = True
        txtEmployeeName.ReadOnly = True

        cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList
        cboPaymentMethod.Items.Clear()
        cboPaymentMethod.Items.AddRange(New String() {METHOD_CASH, METHOD_SALARY})
        cboPaymentMethod.SelectedIndex = 0
    End Sub

    Private Sub cboPaymentMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPaymentMethod.SelectedIndexChanged
        Dim isSalary As Boolean = (cboPaymentMethod.Text = METHOD_SALARY)
        btnSearchEmployee.Enabled = isSalary

        If isSalary Then
            txtAmountReceived.Text = GrandTotal.ToString("N2")   ' no cash handed over
            txtAmountReceived.ReadOnly = True
        Else
            txtEmployeeName.Clear()
            If txtAmountReceived.ReadOnly Then txtAmountReceived.Clear()
            txtAmountReceived.ReadOnly = False
        End If
        RecalculateChange()
    End Sub

    ' ===== Employee search (Employee's Salary only) =====
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
            MsgBox("No employee found.", vbInformation, "Search Employee")
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
            MsgBox("There is nothing to pay.", vbExclamation, "Settle Payment")
            Exit Sub
        End If

        If cboPaymentMethod.SelectedIndex = -1 Then
            MsgBox("Select a payment method.", vbExclamation, "Settle Payment")
            Exit Sub
        End If

        If cboPaymentMethod.Text = METHOD_SALARY AndAlso String.IsNullOrWhiteSpace(txtEmployeeName.Text) Then
            MsgBox("Search and select the employee whose salary will be deducted.", vbExclamation, "Settle Payment")
            btnSearchEmployee.Focus()
            Exit Sub
        End If

        Dim received As Decimal
        If Not Decimal.TryParse(txtAmountReceived.Text, NumberStyles.Number, CultureInfo.CurrentCulture, received) Then
            MsgBox("Enter a valid amount received.", vbExclamation, "Settle Payment")
            txtAmountReceived.Focus()
            Exit Sub
        End If

        If received < GrandTotal Then
            MsgBox("Amount received is less than the Grand Total.", vbExclamation, "Settle Payment")
            txtAmountReceived.Focus()
            Exit Sub
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Function ToMoney(s As String) As Decimal
        Dim v As Decimal
        If Decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, v) Then Return v
        Return 0
    End Function

End Class