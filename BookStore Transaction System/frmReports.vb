Imports MySql.Data.MySqlClient

Public Class frmReports

    Private _currentForm As Form

    Private Sub cboReportType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboReportType.SelectedIndexChanged

        Select Case cboReportType.Text

            Case "Sales By Item"
                OpenReport(GetType(frmSalesByItem))

            Case "Sales by Date Range"
                OpenReport(GetType(frmSalesDateRange))

            Case "Cash Denomination"
                OpenReport(GetType(frmCashDenomination))

            Case "Remittance Report"
                OpenReport(GetType(frmRemittance))

            Case "Inventory Discrepancy"
                OpenReport(GetType(frmInventoryCountReconciliation))

        End Select

    End Sub

    Private Function OpenReport(formType As Type) As Form

        If _currentForm IsNot Nothing Then

            _currentForm.Close()
            _currentForm.Dispose()
            _currentForm = Nothing

        End If

        Panel1.Controls.Clear()

        Dim frm As Form =
            CType(Activator.CreateInstance(formType), Form)

        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill

        Panel1.Controls.Add(frm)

        _currentForm = frm

        frm.Show()

        Return frm

    End Function

    Public Sub OpenRemittance()

        cboReportType.SelectedItem = "Remittance Report"

    End Sub

End Class