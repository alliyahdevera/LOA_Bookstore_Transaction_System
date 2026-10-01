Imports MySql.Data.MySqlClient

Public Class frmReports

    Private _currentForm As Form

    Private Sub cboReportType_SelectedIndexChanged(sender As Object, e As EventArgs) _
            Handles cboReportType.SelectedIndexChanged

        Select Case cboReportType.Text.Trim()

            Case "Sales By Item"
                OpenReport(GetType(frmSalesByItem))

            Case "Sales by Date Range"
                OpenReport(GetType(frmSalesDateRange))

            Case "Cash Denomination"
                OpenReport(GetType(frmCashDenomination))

            Case "Remittance Report"
                OpenReport(GetType(frmRemittance))

            Case "Inventory Discrepancy"
                OpenReport(GetType(frmInventoryDiscrepancies))   ' <-- was frmInventoryCountReconciliation

            Case Else
                MsgBox("No form is linked to report type: " & cboReportType.Text,
                       vbExclamation, "Reports")
        End Select

    End Sub

    Private Sub OpenReport(formType As Type)

        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _currentForm.Dispose()
            _currentForm = Nothing
        End If

        pnlreports.Controls.Clear()

        Dim frm As Form = CType(Activator.CreateInstance(formType), Form)
        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill

        pnlreports.Controls.Add(frm)
        _currentForm = frm
        frm.Show()

    End Sub

    ' Called by frmCashDenomination after saving
    Public Sub OpenRemittance()
        ' Deferred so the calling form isn't disposed inside its own click handler
        Me.BeginInvoke(Sub() cboReportType.SelectedItem = "Remittance Report")
    End Sub

    Private Sub frmReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboReportType.SelectedIndex = 0
    End Sub

End Class