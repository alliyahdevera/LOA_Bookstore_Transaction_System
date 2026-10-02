Public Class frmInventory

    Private _currentForm As Form
    Private _isReverting As Boolean = True

    Private Function AllowedItems() As String()
        Select Case If(currentuser.Role, "").Trim()
            Case ROLE_SUPERVISOR
                Return New String() {"Product List", "Manage Products", "Stock In", "Stock In History",
                                     "Low Level Stocks", "Inventory Count & Reconciliation"}
            Case ROLE_INVENTORY_STAFF
                Return New String() {"Product List", "Stock In", "Stock In History",
                                     "Low Level Stocks", "Inventory Count & Reconciliation"}
            Case ROLE_MANAGEMENT
                Return New String() {"Product List", "Stock In History", "Low Level Stocks"}
            Case Else
                Return New String() {}
        End Select
    End Function

    Private Sub frmInventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _isReverting = True
        Dim allowed As String() = AllowedItems()

        For i As Integer = cboInventory.Items.Count - 1 To 0 Step -1
            If Array.IndexOf(allowed, cboInventory.Items(i).ToString()) < 0 Then
                cboInventory.Items.RemoveAt(i)
            End If
        Next

        _isReverting = False
        If cboInventory.Items.Count > 0 Then cboInventory.SelectedIndex = 0
    End Sub

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

        pnlinventory.Controls.Add(frm)
        _currentForm = frm
        frm.Show()

    End Sub

    Private Sub pnlinventory_Paint(sender As Object, e As PaintEventArgs) Handles pnlinventory.Paint

    End Sub
End Class