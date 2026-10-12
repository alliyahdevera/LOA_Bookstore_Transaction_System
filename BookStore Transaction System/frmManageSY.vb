Public Class frmManageSY

    Private ReadOnly allTime As New SchoolYearInfo With {.Id = 0, .Label = "All Time", .Display = "All Time"}
    Private isFilling As Boolean = False

    Private Sub frmManageSY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EnsureSchoolYearInitialized()

        With dgvsy
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With

        txtStudentID.MaxLength = 9
        Dim tip As New ToolTip()
        tip.SetToolTip(txtStudentID, "Format: 2026-2027")

        LoadSchoolYears(ViewingId())
    End Sub

    ' ==================== helpers ====================
    Private Function ViewingId() As Integer
        Return If(SelectedSchoolYear Is Nothing, 0, SelectedSchoolYear.Id)
    End Function

    Private Function SelectedInfo() As SchoolYearInfo
        If dgvsy.SelectedRows.Count = 0 Then Return Nothing
        Return TryCast(dgvsy.SelectedRows(0).Tag, SchoolYearInfo)
    End Function

    Private Sub UpdateViewingLabel()
        Label13.Text = "Currently Viewing: " &
            If(SelectedSchoolYear Is Nothing, "All Time", "SY " & SelectedSchoolYear.Label)
    End Sub

    Private Sub LoadSchoolYears(selectId As Integer)
        isFilling = True
        dgvsy.Rows.Clear()

        Dim idx As Integer = dgvsy.Rows.Add("All Time", "", "")
        dgvsy.Rows(idx).Tag = allTime

        For Each sy As SchoolYearInfo In GetSchoolYears()
            Dim r As Integer = dgvsy.Rows.Add(
                sy.Label & If(sy.IsCurrent, " (Current)", ""),
                sy.StartDate.ToShortDateString(),
                sy.EndDate.ToShortDateString())
            dgvsy.Rows(r).Tag = sy
        Next

        dgvsy.ClearSelection()
        Dim target As Integer = 0
        For Each row As DataGridViewRow In dgvsy.Rows
            Dim info As SchoolYearInfo = TryCast(row.Tag, SchoolYearInfo)
            If info IsNot Nothing AndAlso info.Id = selectId Then
                target = row.Index
                Exit For
            End If
        Next
        dgvsy.Rows(target).Selected = True
        dgvsy.CurrentCell = dgvsy.Rows(target).Cells(0)

        isFilling = False
        FillInputsFromSelection()
        UpdateViewingLabel()
    End Sub

    Private Sub FillInputsFromSelection()
        Dim info As SchoolYearInfo = SelectedInfo()
        If info Is Nothing OrElse info.Id = 0 Then
            ResetInputs()
        Else
            txtStudentID.Text = info.Label
            DateTimePicker1.Value = info.StartDate
            DateTimePicker2.Value = info.EndDate
        End If
    End Sub

    Private Sub ResetInputs()
        txtStudentID.Clear()
        DateTimePicker1.Value = Date.Today
        DateTimePicker2.Value = Date.Today
    End Sub

    Private Function ValidateInputs(label As String, excludeId As Integer) As Boolean
        If label = "" Then
            MsgBox("Enter the school year (example: 2026-2027).", vbExclamation, "Manage School Year")
            txtStudentID.Focus()
            Return False
        End If
        If Not IsValidSchoolYearLabel(label) Then
            MsgBox("School year must look like 2026-2027 (the second year is one more than the first).",
                   vbExclamation, "Manage School Year")
            txtStudentID.Focus()
            Return False
        End If
        If DateTimePicker1.Value.Date >= DateTimePicker2.Value.Date Then
            MsgBox("The start date must be earlier than the end date.", vbExclamation, "Manage School Year")
            DateTimePicker1.Focus()
            Return False
        End If
        If SchoolYearLabelExists(label, excludeId) Then
            MsgBox("A school year named " & label & " already exists.", vbExclamation, "Manage School Year")
            txtStudentID.Focus()
            Return False
        End If
        If SchoolYearOverlaps(DateTimePicker1.Value.Date, DateTimePicker2.Value.Date, excludeId) Then
            MsgBox("These dates overlap another school year. Adjust the start or end date.",
                   vbExclamation, "Manage School Year")
            DateTimePicker1.Focus()
            Return False
        End If
        Return True
    End Function

    ' ==================== events ====================
    Private Sub dgvsy_SelectionChanged(sender As Object, e As EventArgs) Handles dgvsy.SelectionChanged
        If isFilling Then Exit Sub
        FillInputsFromSelection()
    End Sub

    Private Sub btnsavereq_Click(sender As Object, e As EventArgs) Handles btnsavereq.Click   ' ADD
        Dim label As String = txtStudentID.Text.Trim()
        If Not ValidateInputs(label, 0) Then Exit Sub

        If Not AddSchoolYear(label, DateTimePicker1.Value.Date, DateTimePicker2.Value.Date) Then
            MsgBox("The school year could not be saved.", vbCritical, "Manage School Year")
            Exit Sub
        End If

        LogActivity("School Year Added", label,
            "Added school year " & label & " (" & DateTimePicker1.Value.ToString("MMM d, yyyy") &
            " - " & DateTimePicker2.Value.ToString("MMM d, yyyy") & ")")
        MsgBox("School year " & label & " was added.", vbInformation, "Manage School Year")

        LoadSchoolYears(ViewingId())
        ResetInputs()
    End Sub

    Private Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click   ' EDIT
        Dim info As SchoolYearInfo = SelectedInfo()
        If info Is Nothing OrElse info.Id = 0 Then
            MsgBox("Click a school year in the list first. ""All Time"" cannot be edited.",
                   vbExclamation, "Manage School Year")
            Exit Sub
        End If

        Dim label As String = txtStudentID.Text.Trim()
        If Not ValidateInputs(label, info.Id) Then Exit Sub

        If MsgBox("Save the changes to school year " & info.Label & "?",
                  vbQuestion + vbYesNo + vbDefaultButton2, "Manage School Year") <> MsgBoxResult.Yes Then Exit Sub

        If Not UpdateSchoolYear(info.Id, label, DateTimePicker1.Value.Date, DateTimePicker2.Value.Date) Then
            MsgBox("The school year could not be updated.", vbCritical, "Manage School Year")
            Exit Sub
        End If

        LogActivity("School Year Edited", label,
            "Changed school year " & info.Label & " to " & label & " (" &
            DateTimePicker1.Value.ToString("MMM d, yyyy") & " - " &
            DateTimePicker2.Value.ToString("MMM d, yyyy") & ")")

        RefreshSelectedSchoolYear()
        MsgBox("School year updated.", vbInformation, "Manage School Year")
        LoadSchoolYears(info.Id)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click   ' DELETE
        Dim info As SchoolYearInfo = SelectedInfo()
        If info Is Nothing OrElse info.Id = 0 Then
            MsgBox("Click a school year in the list first. ""All Time"" cannot be deleted.",
                   vbExclamation, "Manage School Year")
            Exit Sub
        End If

        If info.IsCurrent Then
            MsgBox("The current school year cannot be deleted.", vbExclamation, "Manage School Year")
            Exit Sub
        End If

        Dim txnCount As Integer = CountTransactionsInRange(info.StartDate, info.EndDate)
        Dim warning As String = "Delete school year " & info.Label & "?"
        If txnCount > 0 Then
            warning &= vbCrLf & vbCrLf & txnCount & " transaction(s) fall inside these dates. " &
                       "They will not be deleted, but they will no longer belong to any school year filter."
        End If

        If MsgBox(warning, vbExclamation + vbYesNo + vbDefaultButton2, "Manage School Year") <> MsgBoxResult.Yes Then Exit Sub

        If Not DeleteSchoolYear(info.Id) Then
            MsgBox("The school year could not be deleted.", vbCritical, "Manage School Year")
            Exit Sub
        End If

        LogActivity("School Year Deleted", info.Label, "Deleted school year " & info.Label)

        If SelectedSchoolYear IsNot Nothing AndAlso SelectedSchoolYear.Id = info.Id Then
            ResetSchoolYear()
            EnsureSchoolYearInitialized()
        End If

        MsgBox("School year deleted.", vbInformation, "Manage School Year")
        LoadSchoolYears(ViewingId())
    End Sub

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click   ' CLEAR
        isFilling = True
        dgvsy.ClearSelection()
        isFilling = False
        ResetInputs()
        txtStudentID.Focus()
    End Sub

    Private Sub btnupd_Click(sender As Object, e As EventArgs) Handles btnupd.Click   ' CHANGE S.Y.
        Dim chosen As SchoolYearInfo = SelectedInfo()
        If chosen Is Nothing Then
            MsgBox("Click a school year first.", vbExclamation, "Manage School Year")
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