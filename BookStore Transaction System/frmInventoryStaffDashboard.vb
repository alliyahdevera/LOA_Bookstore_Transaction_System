Public Class frmInventoryStaffDashboard
    Private Sub frmInventoryStaffDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RoundPanel(pnldistrsale)
        RoundPanel(pnllowprod)
        RoundPanel(pnllowstock)
        RoundPanel(pnlmostpurchasedprod)
        RoundPanel(pnlsalespmonth)
        RoundPanel(pnlsalestoday)
        RoundPanel(pnltotprod)
        RoundPanel(pnltotqprod)
    End Sub
End Class