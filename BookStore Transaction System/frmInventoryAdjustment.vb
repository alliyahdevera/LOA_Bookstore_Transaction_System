Public Class frmInventoryAdjustment

    Public Property ProductCode As String
    Public Property ProductName As String
    Public Property ItemSize As String
    Public Property SystemQty As String
    Public Property PhysicalQty As String

    Private Sub frmInventoryAdjustment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtProduct.Text = ProductName
        txtsize.Text = ItemSize
        lblsysq.Text = SystemQty
        lblphyq.Text = PhysicalQty
        lblDifference.Text = (Val(PhysicalQty) - Val(SystemQty)).ToString()
    End Sub

    Private Sub btnconfirm_Click(sender As Object, e As EventArgs) Handles btnconfirm.Click
        If String.IsNullOrWhiteSpace(txtReason.Text) Then
            MsgBox("Enter the reason for the adjustment.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If

        ' TODO: UPDATE quantity_on_hand + stock entry + audit log here

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class