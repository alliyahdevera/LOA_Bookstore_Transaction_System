Public Class frmReports

    Private _currentForm As Form
    Private _loading As Boolean = True

    Private Function AllowedReports() As String()
        Select Case If(currentuser.Role, "").Trim()
            Case ROLE_SUPERVISOR
                Return New String() {"Sales By Item", "Sales by Date Range", "Cash Denomination", "Remittance Report", "Inventory Discrepancy"}
            Case ROLE_MANAGEMENT
                Return New String() {"Sales By Item", "Sales by Date Range", "Remittance Report", "Inventory Discrepancy"}
            Case ROLE_CASHIER
                Return New String() {"Cash Denomination", "Remittance Report"}
            Case Else
                Return New String() {}
        End Select
    End Function

    Private Sub frmReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _loading = True
        Dim allowed As String() = AllowedReports()

        For i As Integer = cboReportType.Items.Count - 1 To 0 Step -1
            If Array.IndexOf(allowed, cboReportType.Items(i).ToString()) < 0 Then
                cboReportType.Items.RemoveAt(i)
            End If
        Next

        _loading = False
        If cboReportType.Items.Count > 0 Then cboReportType.SelectedIndex = 0
    End Sub

    Private Sub cboReportType_SelectedIndexChanged(sender As Object, e As EventArgs) _
            Handles cboReportType.SelectedIndexChanged

        If _loading Then Exit Sub

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
                OpenReport(GetType(frmInventoryDiscrepancies))

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

        pnlreports.Controls.Add(frm)
        _currentForm = frm
        frm.Show()

    End Sub

    Public Sub OpenRemittance()
        Me.BeginInvoke(Sub() cboReportType.SelectedItem = "Remittance Report")
    End Sub

End Class