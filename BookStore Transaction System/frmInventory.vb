' frmInventory.vb
Public Class frmInventory

    Private _currentForm As Form

    Private Sub frmInventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Q39: only the Bookstore Supervisor can add/edit/remove product master data
        btnManageProducts.Visible = (currentuser.Role = ROLE_SUPERVISOR)
        OpenTab(btnProductList, GetType(frmProductList))
    End Sub

    Private Sub btnProductList_Click(sender As Object, e As EventArgs) Handles btnProductList.Click
        OpenTab(btnProductList, GetType(frmProductList))
    End Sub

    Private Sub btnManageProducts_Click(sender As Object, e As EventArgs) Handles btnManageProducts.Click   ' Manage Products
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can manage product information.", vbExclamation, "Access Denied")
            Exit Sub
        End If
        OpenTab(btnManageProducts, GetType(frmManageProducts))
    End Sub

    Private Sub btnStockEntry_Click(sender As Object, e As EventArgs) Handles btnStockEntry.Click   ' Stock Entry
        OpenTab(btnStockEntry, GetType(frmStockEntry))
    End Sub

    Private Sub btnStockInHistory_Click(sender As Object, e As EventArgs) Handles btnStockInHistory.Click   ' Stock In History
        OpenTab(btnStockInHistory, GetType(frmStockInHistory))
    End Sub

    Private Sub btnLowLevelStocks_Click(sender As Object, e As EventArgs) Handles btnLowLevelStocks.Click   ' Low Level Stocks
        OpenTab(btnLowLevelStocks, GetType(frmLowLevelStocks))
    End Sub

    Private Sub OpenTab(activeBtn As Button, formType As Type)
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

        For Each btn As Button In New Button() {btnProductList, btnManageProducts, btnStockEntry, btnStockInHistory, btnLowLevelStocks}
            btn.BackColor = Color.FromArgb(1, 21, 78)
        Next
        activeBtn.BackColor = Color.FromArgb(25, 55, 140)
    End Sub

End Class