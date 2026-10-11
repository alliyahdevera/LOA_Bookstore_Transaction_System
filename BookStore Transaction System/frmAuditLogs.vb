Public Class frmAuditLogs

    Private _currentForm As Form


    Private Sub OpenTab(activeBtn As Button, formType As Type)
        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _currentForm.Dispose()
            _currentForm = Nothing
        End If
        Panel1.Controls.Clear()

        Dim frm As Form = CType(Activator.CreateInstance(formType), Form)
        frm.TopLevel = False
        frm.Dock = DockStyle.Fill
        Panel1.Controls.Add(frm)
        _currentForm = frm
        frm.Show()
    End Sub

End Class