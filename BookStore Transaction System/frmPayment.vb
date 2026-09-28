Imports System.Globalization

Public Class frmPayment

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

        dtpORDate.Value = Date.Today

        txtGrandTotal.Text = GrandTotal.ToString("N2")   ' comes from frmPOS
        txtGrandTotal.ReadOnly = True
        txtChange.ReadOnly = True

        cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList
        cboPaymentMethod.Items.Clear()
        cboPaymentMethod.Items.AddRange(New String() {"Cash", "Salary Deduction"})
        cboPaymentMethod.SelectedIndex = 0
    End Sub

    Private Sub cboPaymentMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPaymentMethod.SelectedIndexChanged
        If cboPaymentMethod.Text = "Salary Deduction" Then
            txtAmountReceived.Text = GrandTotal.ToString("N2")   ' no cash handed over
            txtAmountReceived.ReadOnly = True
        Else
            If txtAmountReceived.ReadOnly Then txtAmountReceived.Clear()
            txtAmountReceived.ReadOnly = False
        End If
        RecalculateChange()
    End Sub

    ' ===== NO NEGATIVES: only digits and one decimal point can be typed =====
    Private Sub txtAmountReceived_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAmountReceived.KeyPress
        If Char.IsControl(e.KeyChar) Then Exit Sub
        If Char.IsDigit(e.KeyChar) Then Exit Sub
        If e.KeyChar = "."c AndAlso Not txtAmountReceived.Text.Contains(".") Then Exit Sub
        e.Handled = True   ' blocks "-", letters, and a second "."
    End Sub

    Private Sub txtAmountReceived_TextChanged(sender As Object, e As EventArgs) Handles txtAmountReceived.TextChanged
        ' catches a negative number that was pasted in
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
            txtChange.Text = "0.00"   ' never shows a negative change
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If cboPaymentMethod.SelectedIndex = -1 Then
            MsgBox("Select a payment method.", vbExclamation, "Settle Payment")
            Exit Sub
        End If

        Dim received As Decimal
        If Not Decimal.TryParse(txtAmountReceived.Text, NumberStyles.Number, CultureInfo.CurrentCulture, received) Then
            MsgBox("Enter a valid amount received.", vbExclamation, "Settle Payment")
            txtAmountReceived.Focus()
            Exit Sub
        End If

        If received < 0 Then
            MsgBox("Amount received cannot be negative.", vbExclamation, "Settle Payment")
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