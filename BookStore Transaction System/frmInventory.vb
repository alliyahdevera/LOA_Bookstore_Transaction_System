' frmInventory.vb
Public Class frmInventory

    Private _currentForm As Form

    Private Sub frmInventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Q39: only the Bookstore Supervisor can add/edit/remove product master data
        Button3.Visible = (currentuser.Role = ROLE_SUPERVISOR)
        OpenTab(btnProductList, GetType(frmProductList))
    End Sub

    Private Sub btnProductList_Click(sender As Object, e As EventArgs) Handles btnProductList.Click
        OpenTab(btnProductList, GetType(frmProductList))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click   ' Manage Products
        If currentuser.Role <> ROLE_SUPERVISOR Then
            MsgBox("Only the Bookstore Supervisor can manage product information.", vbExclamation, "Access Denied")
            Exit Sub
        End If
        OpenTab(Button3, GetType(frmManageProducts))
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click   ' Stock Entry
        OpenTab(Button5, GetType(frmStockEntry))
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click   ' Stock In History
        OpenTab(Button4, GetType(frmStockInHistory))
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

        For Each btn As Button In New Button() {btnProductList, Button3, Button5, Button4, Button5}
            btn.BackColor = Color.FromArgb(1, 21, 78)
        Next
        activeBtn.BackColor = Color.FromArgb(25, 55, 140)
    End Sub
End Class