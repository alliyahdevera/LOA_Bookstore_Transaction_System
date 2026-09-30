Public Class frmCashDenomination

    Private Sub btnSaveTransaction_Click(sender As Object, e As EventArgs) Handles btnSaveTransaction.Click   ' Generate Remittance
        ' TODO: save the remittance record here

        Dim reports As frmReports = TryCast(Me.Parent?.FindForm(), frmReports)
        If reports IsNot Nothing Then reports.OpenRemittance()
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        btnClear.PerformClick()
    End Sub

End Class