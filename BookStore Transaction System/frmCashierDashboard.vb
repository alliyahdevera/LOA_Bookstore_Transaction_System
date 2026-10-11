Public Class frmCashierDashboard
    Private Sub frmCashierDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RoundPanel(pnlcash)
        RoundPanel(pnlmostpurchasedprod)
        RoundPanel(pnlpaymeth)
        RoundPanel(pnlreadyfr)
        RoundPanel(pnlsalarydeduct)
        RoundPanel(pnlsalehr)
        RoundPanel(pnltots)
        RoundPanel(pnltottrant)
    End Sub
End Class