Public Class frmPayment

    Public Property GrandTotal As Decimal

    Public ReadOnly Property ResultORNo As String
        Get
            Return TextBox12.Text
        End Get
    End Property
    Public ReadOnly Property ResultDate As DateTime
        Get
            Return DateTimePicker1.Value.Date
        End Get
    End Property
    Public ReadOnly Property ResultMethod As String
        Get
            Return ComboBox4.Text
        End Get
    End Property
    Public ReadOnly Property ResultReceived As Decimal
        Get
            Return If(IsNumeric(TextBox8.Text), Convert.ToDecimal(TextBox8.Text), 0)
        End Get
    End Property
    Public ReadOnly Property ResultChange As Decimal
        Get
            Return If(IsNumeric(TextBox7.Text), Convert.ToDecimal(TextBox7.Text), 0)
        End Get
    End Property

    Private Sub frmPayment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox12.Text = NewORNo()
        TextBox12.ReadOnly = True
        DateTimePicker1.Value = Today
        TextBox9.Text = GrandTotal.ToString("N2")
        TextBox9.ReadOnly = True
        TextBox7.ReadOnly = True

        ComboBox4.Items.Clear()
        ComboBox4.Items.AddRange(New String() {"Cash", "Salary Deduction"})
        ComboBox4.SelectedIndex = 0
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        If ComboBox4.Text = "Salary Deduction" Then
            TextBox8.Text = GrandTotal.ToString("N2")   ' no cash handed over
            TextBox8.ReadOnly = True
        Else
            TextBox8.ReadOnly = False
        End If
        RecalculateChange()
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        RecalculateChange()
    End Sub

    Private Sub RecalculateChange()
        If IsNumeric(TextBox8.Text) Then
            TextBox7.Text = (Convert.ToDecimal(TextBox8.Text) - GrandTotal).ToString("N2")
        Else
            TextBox7.Text = ""
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click   ' Save
        If Not IsNumeric(TextBox8.Text) Then
            MsgBox("Enter the amount received.", vbExclamation, "Settle Payment")
            Exit Sub
        End If
        If Convert.ToDecimal(TextBox8.Text) < GrandTotal Then
            MsgBox("Amount received is less than the Grand Total.", vbExclamation, "Settle Payment")
            Exit Sub
        End If
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

End Class