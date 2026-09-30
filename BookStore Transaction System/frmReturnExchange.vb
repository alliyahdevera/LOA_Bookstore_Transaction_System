Public Class frmReturnExchange

    Public Property TransactionNo As String

    Private Sub btnreturnexc_Click(sender As Object, e As EventArgs) Handles btnreturnexc.Click
        ' TODO: validate and save the return/exchange here

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class