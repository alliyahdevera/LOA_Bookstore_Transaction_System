Public Class frmManageSY

    Private ReadOnly allTime As New SchoolYearInfo With {.Id = 0, .Label = "All Time", .Display = "All Time"}

    Private Sub frmManageSY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EnsureSchoolYearInitialized()

        dgvsy.Rows.Clear()

        ' Add "All Time" as the first row option
        Dim allTimeRowIdx As Integer = dgvsy.Rows.Add("All Time", "", "")
        dgvsy.Rows(allTimeRowIdx).Tag = allTime

        ' Add school years to the DataGridView and store the object in the row's Tag
        For Each sy As SchoolYearInfo In GetSchoolYears()
            Dim rowIndex As Integer = dgvsy.Rows.Add(sy.Label, sy.StartDate.ToShortDateString(), sy.EndDate.ToShortDateString())
            dgvsy.Rows(rowIndex).Tag = sy
        Next

        ' Select the appropriate row by default
        dgvsy.ClearSelection()
        Dim targetIndex As Integer = 0

        If SelectedSchoolYear IsNot Nothing Then
            For Each row As DataGridViewRow In dgvsy.Rows
                Dim syInfo As SchoolYearInfo = TryCast(row.Tag, SchoolYearInfo)
                If syInfo IsNot Nothing AndAlso syInfo.Id = SelectedSchoolYear.Id Then
                    targetIndex = row.Index
                    Exit For
                End If
            Next
        End If

        If dgvsy.Rows.Count > targetIndex Then
            dgvsy.Rows(targetIndex).Selected = True
            dgvsy.CurrentCell = dgvsy.Rows(targetIndex).Cells(0)
        End If

        UpdateViewingLabel()
    End Sub

    Private Sub UpdateViewingLabel()
        Label13.Text = "Currently Viewing: " & If(SelectedSchoolYear Is Nothing, "All Time", "SY " & SelectedSchoolYear.Label)
    End Sub

    Private Sub btnupd_Click(sender As Object, e As EventArgs) Handles btnupd.Click
        If dgvsy.SelectedRows.Count = 0 AndAlso dgvsy.CurrentRow Is Nothing Then
            MsgBox("Click a school year first.", vbExclamation, "Manage School Year")
            Exit Sub
        End If

        Dim currentRow As DataGridViewRow = If(dgvsy.SelectedRows.Count > 0, dgvsy.SelectedRows(0), dgvsy.CurrentRow)
        Dim chosen As SchoolYearInfo = TryCast(currentRow.Tag, SchoolYearInfo)

        If chosen Is Nothing Then
            MsgBox("Please select a valid school year.", vbExclamation, "Manage School Year")
            Exit Sub
        End If

        SelectSchoolYear(If(chosen.Id = 0, Nothing, chosen))
        UpdateViewingLabel()

        MsgBox("Now viewing: " & If(chosen.Id = 0, "All Time", "SY " & chosen.Label) & vbCrLf &
               "The Dashboard, Transaction History and Reports will show only this school year" & vbCrLf &
               "the next time you open them.", vbInformation, "Manage School Year")
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub dgvsy_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvsy.CellDoubleClick
        If e.RowIndex >= 0 Then btnupd.PerformClick()
    End Sub

End Class