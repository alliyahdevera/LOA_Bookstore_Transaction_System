Public Class frmInventoryCountReconciliation

    Private Sub btnreconcile_Click(sender As Object, e As EventArgs) Handles btnreconcile.Click
        If dgvlistproducts.CurrentRow Is Nothing Then
            MsgBox("Select a product row first.", vbExclamation, "Inventory Adjustment")
            Exit Sub
        End If

        Dim row As DataGridViewRow = dgvlistproducts.CurrentRow

        Using frm As New frmInventoryAdjustment()
            frm.ProductCode = Convert.ToString(row.Cells("ProductCode").Value)
            frm.ProductName = Convert.ToString(row.Cells("ProductName").Value)
            frm.ItemSize = Convert.ToString(row.Cells("Size").Value)
            frm.SystemQty = Convert.ToString(row.Cells("SystemQuantity").Value)
            frm.PhysicalQty = Convert.ToString(row.Cells("PhysicalQuantity").Value)
            frm.StartPosition = FormStartPosition.CenterParent

            If frm.ShowDialog(Me) = DialogResult.OK Then
                btngenerate.PerformClick()   ' refresh the list after the adjustment
            End If
        End Using
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        btnclear.PerformClick()
    End Sub

End Class