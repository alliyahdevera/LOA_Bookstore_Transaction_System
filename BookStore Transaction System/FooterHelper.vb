Public Module FooterHelper

    ' Same footer everywhere: Name | Position | live date & time
    Public Sub SetupFooter(frm As Form, lblName As Label, lblPosition As Label, lblDateTime As Label)
        ApplySearchPlaceholders(frm)
        lblName.Text = If(Not String.IsNullOrEmpty(currentuser.FullName), currentuser.FullName, "N/A")
        lblPosition.Text = If(Not String.IsNullOrEmpty(currentuser.Role), currentuser.Role, "N/A")
        lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy h:mm:ss tt")

        Dim tmr As New System.Windows.Forms.Timer()
        tmr.Interval = 1000
        AddHandler tmr.Tick,
            Sub(s As Object, ev As EventArgs)
                If Not lblDateTime.IsDisposed Then
                    lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy h:mm:ss tt")
                End If
            End Sub
        AddHandler frm.Disposed,
            Sub(s As Object, ev As EventArgs)
                tmr.Stop()
                tmr.Dispose()
            End Sub
        tmr.Start()
    End Sub

End Module