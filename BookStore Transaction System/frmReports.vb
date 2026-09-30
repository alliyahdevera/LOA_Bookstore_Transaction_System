Imports MySql.Data.MySqlClient

Public Class frmReports

    Private _currentForm As Form

    Private Sub frmReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OpenTab(btnsalesitem, GetType(frmSalesByItem))
    End Sub

    Private Sub btnsalesitem_Click(sender As Object, e As EventArgs) Handles btnsalesitem.Click
        OpenTab(btnsalesitem, GetType(frmSalesByItem))
    End Sub

    Private Sub btnsalesrange_Click(sender As Object, e As EventArgs) Handles btnsalesrange.Click
        OpenTab(btnsalesrange, GetType(frmSalesDateRange))
    End Sub

    ' "Cash Denomination" button (End-Of-Day Reconciliation)
    Private Sub btnendofday_Click(sender As Object, e As EventArgs) Handles btnendofday.Click
        OpenTab(btnendofday, GetType(frmCashDenomination))
    End Sub

    ' "Inventory Discrepancy" button
    Private Sub btninvdiscrepancy_Click(sender As Object, e As EventArgs) Handles btninvdiscrepancy.Click
        OpenTab(btninvdiscrepancy, GetType(frmInventoryCountReconciliation))
    End Sub

    ' "Remittance Report" button
    Private Sub btnremittance_Click(sender As Object, e As EventArgs) Handles btnremittance.Click
        OpenRemittance()
    End Sub

    ' Public so frmCashDenomination can jump here after generating a remittance
    Public Sub OpenRemittance()
        OpenTab(btnremittance, GetType(frmRemittance))
    End Sub

    Private Function OpenTab(activeBtn As Button, formType As Type) As Form
        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _currentForm.Dispose()
            _currentForm = Nothing
        End If
        Panel1.Controls.Clear()

        Dim frm As Form = CType(Activator.CreateInstance(formType), Form)
        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill
        Panel1.Controls.Add(frm)
        _currentForm = frm
        frm.Show()

        For Each btn As Button In New Button() {btnsalesitem, btnsalesrange, btnendofday, btninvdiscrepancy, btnremittance}
            btn.BackColor = Color.FromArgb(1, 21, 78)
        Next
        activeBtn.BackColor = Color.FromArgb(25, 55, 140)

        Return frm
    End Function

End Class