Public Class frmTransactionDetails

    Public Property TransactionNo As String

    Private Sub btnreturnexc_Click(sender As Object, e As EventArgs) Handles btnreturnexc.Click
        Using frm As New frmReturnExchange()
            frm.TransactionNo = TransactionNo
            frm.StartPosition = FormStartPosition.CenterParent
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Me.DialogResult = DialogResult.OK   ' closes this form and tells the history grid to refresh
            End If
        End Using
    End Sub

End Class