Imports MySql.Data.MySqlClient

Public Class frmReports

    Private _currentForm As Form

    Private Sub frmReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OpenTab(Button1, GetType(frmSalesByItem))
    End Sub

    ' Sales by Item
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenTab(Button1, GetType(frmSalesByItem))
    End Sub

    ' Sales by Date Range
    Private Sub btnProductList_Click(sender As Object, e As EventArgs) Handles btnProductList.Click
        OpenTab(btnProductList, GetType(frmSalesDateRange))
    End Sub

    ' Sales Today (date range report with both dates set to today)
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim rpt As frmSalesDateRange = TryCast(OpenTab(Button2, GetType(frmSalesDateRange)), frmSalesDateRange)
        If rpt IsNot Nothing Then
            rpt.dtfrom.Value = DateTime.Today
            rpt.dtto.Value = DateTime.Today
            rpt.btngenerate.PerformClick()
        End If
    End Sub

    Private Function OpenTab(activeBtn As Button, formType As Type) As Form
        If _currentForm IsNot Nothing Then
            _currentForm.Close()
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

        For Each btn As Button In New Button() {Button1, btnProductList, Button2}
            btn.BackColor = Color.FromArgb(1, 21, 78)
        Next
        activeBtn.BackColor = Color.FromArgb(25, 55, 140)

        Return frm
    End Function

End Class