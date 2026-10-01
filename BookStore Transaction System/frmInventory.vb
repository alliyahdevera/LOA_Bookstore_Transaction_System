Public Class frmInventory

    Private _currentForm As Form

    Private Sub cboInventory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboInventory.SelectedIndexChanged

        Select Case cboInventory.Text

            Case "Product List"
                OpenInventoryForm(GetType(frmProductList))

            Case "Manage Products"

                If currentuser.Role <> ROLE_SUPERVISOR Then
                    MsgBox(
                        "Only the Bookstore Supervisor can manage product information.",
                        vbExclamation,
                        "Access Denied"
                    )

                    cboInventory.SelectedItem = "Product List"
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

        End Select

    End Sub


    Private Sub OpenInventoryForm(formType As Type)

        ' Close current form
        If _currentForm IsNot Nothing Then
            _currentForm.Close()
            _currentForm.Dispose()
            _currentForm = Nothing
        End If

        pnlinventory.Controls.Clear()

        ' Create selected form
        Dim frm As Form = CType(Activator.CreateInstance(formType), Form)

        frm.TopLevel = False

        pnlinventory.Controls.Add(frm)

        _currentForm = frm

        frm.Show()

    End Sub

    Private Sub frmInventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboInventory.SelectedIndex = 0
    End Sub
End Class