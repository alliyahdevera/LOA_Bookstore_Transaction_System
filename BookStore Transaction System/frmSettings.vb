Public Class frmSettings

    Private Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click
        Using f As New frmManageSY()
            f.StartPosition = FormStartPosition.CenterParent
            f.FormBorderStyle = FormBorderStyle.FixedDialog
            f.MaximizeBox = False
            f.MinimizeBox = False
            f.ShowInTaskbar = False
            f.ShowDialog(Me)
        End Using
    End Sub

End Class