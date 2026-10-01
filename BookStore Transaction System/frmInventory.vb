Public Class frmInventory

    Private _currentForm As Form
    Private _isReverting As Boolean = False

    Private Sub cboInventory_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles cboInventory.SelectedIndexChanged

        If _isReverting Then Exit Sub

        Select Case cboInventory.Text.Trim()

            Case "Product List"
                OpenInventoryForm(GetType(frmProductList))

            Case "Manage Products"
                If currentuser.Role <> ROLE_SUPERVISOR Then
                    MsgBox("Only the Bookstore Supervisor can manage product information.",
                       vbExclamation, "Access Denied")

                    _isReverting = True
                    cboInventory.SelectedItem = "Product List"
                    _isReverting = False

                    OpenInventoryForm(GetType(frmProductList))
                    Exit Sub
                End If
                OpenInventoryForm(GetType(frmManageProducts))

            Case "Stock In"
                OpenInventoryForm(GetType(frmStockIn))

            Case "Stock In History"
                OpenInventoryForm(GetType(frmStockInHistory))

            Case "Low Level Stocks"
                OpenInventoryForm(GetType(frmLowLevelStocks))

            Case "Inventory Count & Reconciliation"
                OpenInventoryForm(GetType(frmInventoryCountReconciliation))

            Case Else
                MsgBox("No form is linked to: " & cboInventory.Text, vbExclamation, "Inventory")
        End Select

    End Sub

    Private Sub OpenInventoryForm(formType As Type)

        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _currentForm.Dispose()
            _currentForm = Nothing
        End If

        pnlinventory.Controls.Clear()

        Dim frm As Form = CType(Activator.CreateInstance(formType), Form)
        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill

        pnlinventory.Controls.Add(frm)
        _currentForm = frm
        frm.Show()

    End Sub

    Private Sub frmInventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboInventory.SelectedIndex = 0
    End Sub
End Class