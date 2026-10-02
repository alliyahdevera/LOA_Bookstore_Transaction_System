Imports System.Data.SqlTypes
Imports MySql.Data.MySqlClient

Public Class frmStockIn

    Private Sub frmStockIn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)
        LoadCategoryCombo()
    End Sub

    Private Sub LoadCategoryCombo()
        Dim dt As DataTable = GetDataTable("SELECT category_id, category_name FROM TBL_CATEGORIES ORDER BY category_name")

        ' Add default prompt selection
        Dim row As DataRow = dt.NewRow()
        row("category_id") = 0
        row("category_name") = "-- Select Category --"
        dt.Rows.InsertAt(row, 0)

        FillCombo(cbocategory, dt, "category_name", "category_id")
        cbocategory.SelectedIndex = 0
    End Sub

    Private Function GetSelectedCategoryId() As Integer
        If cbocategory.SelectedValue IsNot Nothing AndAlso IsNumeric(cbocategory.SelectedValue) Then
            Return Convert.ToInt32(cbocategory.SelectedValue)
        End If
        Return 0
    End Function

    Private Sub cbocategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbocategory.SelectedIndexChanged
        ' Filter product selection dropdown or grid when category changes
        LoadProductsByCategory(GetSelectedCategoryId())
    End Sub

    Private Sub LoadProductsByCategory(categoryId As Integer)
        If categoryId = 0 Then Exit Sub
        ' Load corresponding products for entry
    End Sub

End Class