Imports System.Text.RegularExpressions

Public Class frmSchoolYears
    Inherits Form

    Private ReadOnly dgv As New DataGridView
    Private ReadOnly txtLabel As New TextBox
    Private ReadOnly dtpStart As New DateTimePicker
    Private ReadOnly dtpEnd As New DateTimePicker
    Private ReadOnly btnSave As New Button
    Private ReadOnly btnNew As New Button
    Private ReadOnly btnCurrent As New Button
    Private ReadOnly btnDelete As New Button
    Private ReadOnly btnClose As New Button
    Private selectedId As Integer = 0

    Public Sub New()
        MyBase.New()
        Me.Text = "Manage School Year"
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = False
        Me.BackColor = Color.White
        Me.Font = New Font("Segoe UI", 9.75!)
        Me.ClientSize = New Size(640, 440)
        BuildUi()
        LoadList()
    End Sub

    Private Sub BuildUi()
        With dgv
            .Location = New Point(15, 15)
            .Size = New Size(610, 220)
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False
            .BackgroundColor = Color.White
            .Columns.Add("Label", "School Year")
            .Columns.Add("StartDate", "Start Date")
            .Columns.Add("EndDate", "End Date")
            .Columns.Add("IsCurrent", "Current")
            For Each c As DataGridViewColumn In .Columns
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Next
        End With
        AddHandler dgv.CellClick, AddressOf Dgv_CellClick
        Controls.Add(dgv)

        AddRow("School Year (e.g. 2026-2027)", txtLabel, 255)
        txtLabel.MaxLength = 9
        AddHandler txtLabel.Leave, AddressOf TxtLabel_Leave

        Controls.Add(New Label With {.Text = "Start Date", .AutoSize = True, .Location = New Point(15, 295)})
        dtpStart.Format = DateTimePickerFormat.Long
        dtpStart.SetBounds(250, 291, 220, 27)
        Controls.Add(dtpStart)

        Controls.Add(New Label With {.Text = "End Date", .AutoSize = True, .Location = New Point(15, 335)})
        dtpEnd.Format = DateTimePickerFormat.Long
        dtpEnd.SetBounds(250, 331, 220, 27)
        Controls.Add(dtpEnd)

        Dim navy As Color = Color.FromArgb(1, 21, 78)
        Dim defs As Object() = {
            New Object() {btnSave, "Save", 15, 100, True},
            New Object() {btnNew, "New", 125, 80, False},
            New Object() {btnCurrent, "Set as Current", 215, 130, False},
            New Object() {btnDelete, "Delete", 355, 90, False},
            New Object() {btnClose, "Close", 525, 100, False}}
        For Each d As Object() In defs
            Dim b As Button = DirectCast(d(0), Button)
            b.Text = CStr(d(1))
            b.SetBounds(CInt(d(2)), 390, CInt(d(3)), 34)
            b.FlatStyle = FlatStyle.Flat
            b.Cursor = Cursors.Hand
            b.BackColor = If(CBool(d(4)), navy, Color.White)
            b.ForeColor = If(CBool(d(4)), Color.White, Color.Black)
            Controls.Add(b)
        Next
        AddHandler btnSave.Click, AddressOf BtnSave_Click
        AddHandler btnNew.Click, Sub() ClearForm()
        AddHandler btnCurrent.Click, AddressOf BtnCurrent_Click
        AddHandler btnDelete.Click, AddressOf BtnDelete_Click
        AddHandler btnClose.Click, Sub() Me.Close()
        ClearForm()
    End Sub

    Private Sub AddRow(caption As String, tb As TextBox, y As Integer)
        Controls.Add(New Label With {.Text = caption, .AutoSize = True, .Location = New Point(15, y + 4)})
        tb.SetBounds(250, y, 150, 27)
        Controls.Add(tb)
    End Sub

    Private Sub LoadList()
        dgv.Rows.Clear()
        For Each sy As SchoolYearInfo In GetSchoolYears()
            Dim idx As Integer = dgv.Rows.Add(sy.Label, sy.StartDate.ToString("MMM d, yyyy"),
                                              sy.EndDate.ToString("MMM d, yyyy"), If(sy.IsCurrent, "Yes", ""))
            dgv.Rows(idx).Tag = sy
        Next
        dgv.ClearSelection()
    End Sub

    Private Sub ClearForm()
        selectedId = 0
        txtLabel.Clear()
        Dim y As Integer = If(Date.Today.Month >= 6, Date.Today.Year, Date.Today.Year - 1)
        dtpStart.Value = New Date(y, 6, 1)
        dtpEnd.Value = New Date(y + 1, 5, 31)
        dgv.ClearSelection()
    End Sub

    Private Sub Dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Exit Sub
        Dim sy As SchoolYearInfo = TryCast(dgv.Rows(e.RowIndex).Tag, SchoolYearInfo)
        If sy Is Nothing Then Exit Sub
        selectedId = sy.Id
        txtLabel.Text = sy.Label
        dtpStart.Value = sy.StartDate
        dtpEnd.Value = sy.EndDate
    End Sub

    ' typing "2027-2028" suggests the usual June 1 to May 31 dates for a new school year
    Private Sub TxtLabel_Leave(sender As Object, e As EventArgs)
        If selectedId <> 0 OrElse Not Regex.IsMatch(txtLabel.Text.Trim(), "^\d{4}-\d{4}$") Then Exit Sub
        Dim y As Integer = CInt(txtLabel.Text.Trim().Substring(0, 4))
        dtpStart.Value = New Date(y, 6, 1)
        dtpEnd.Value = New Date(y + 1, 5, 31)
    End Sub

    Private Function BlockedForRole() As Boolean
        If RolePermissions.IsViewOnly() Then
            MsgBox("Your role has view-only access.", vbExclamation, "School Year")
            Return True
        End If
        Return False
    End Function

    Private Sub BtnSave_Click(sender As Object, e As EventArgs)
        If BlockedForRole() Then Exit Sub

        Dim label As String = txtLabel.Text.Trim()
        If Not Regex.IsMatch(label, "^\d{4}-\d{4}$") Then
            MsgBox("Enter the school year like 2026-2027.", vbExclamation, "School Year")
            txtLabel.Focus()
            Exit Sub
        End If
        If CInt(label.Substring(5, 4)) <> CInt(label.Substring(0, 4)) + 1 Then
            MsgBox("The second year must be one more than the first (e.g. 2026-2027).", vbExclamation, "School Year")
            txtLabel.Focus()
            Exit Sub
        End If
        If dtpEnd.Value.Date <= dtpStart.Value.Date Then
            MsgBox("The end date must be after the start date.", vbExclamation, "School Year")
            Exit Sub
        End If

        Dim dup As Integer = Convert.ToInt32(If(ExecScalar(
            "SELECT COUNT(*) FROM tbl_school_years WHERE label = @l AND school_year_id <> @id",
            New String() {"@l", "@id"}, New Object() {label, selectedId}), 0))
        If dup > 0 Then
            MsgBox("That school year already exists.", vbExclamation, "School Year")
            Exit Sub
        End If

        Dim overlap As Integer = Convert.ToInt32(If(ExecScalar(
            "SELECT COUNT(*) FROM tbl_school_years WHERE school_year_id <> @id AND start_date <= @e AND end_date >= @s",
            New String() {"@id", "@e", "@s"}, New Object() {selectedId, dtpEnd.Value.Date, dtpStart.Value.Date}), 0))
        If overlap > 0 Then
            MsgBox("These dates overlap another school year.", vbExclamation, "School Year")
            Exit Sub
        End If

        Dim ok As Boolean
        If selectedId = 0 Then
            Dim total As Integer = Convert.ToInt32(If(ExecScalar("SELECT COUNT(*) FROM tbl_school_years"), 0))
            ok = ExecNonQuery("INSERT INTO tbl_school_years (label, start_date, end_date, is_current) VALUES (@l, @s, @e, @c)",
                              New String() {"@l", "@s", "@e", "@c"},
                              New Object() {label, dtpStart.Value.Date, dtpEnd.Value.Date, If(total = 0, 1, 0)})
            If ok Then LogActivity("Add School Year", label, "Added school year " & label)
        Else
            ok = ExecNonQuery("UPDATE tbl_school_years SET label = @l, start_date = @s, end_date = @e WHERE school_year_id = @id",
                              New String() {"@l", "@s", "@e", "@id"},
                              New Object() {label, dtpStart.Value.Date, dtpEnd.Value.Date, selectedId})
            If ok Then LogActivity("Update School Year", label, "Updated school year " & label)
        End If

        If ok Then
            LoadList()
            ClearForm()
        Else
            MsgBox("The school year could not be saved.", vbCritical, "School Year")
        End If
    End Sub

    Private Sub BtnCurrent_Click(sender As Object, e As EventArgs)
        If BlockedForRole() Then Exit Sub
        If selectedId = 0 Then
            MsgBox("Click a school year in the list first.", vbExclamation, "School Year")
            Exit Sub
        End If
        If ExecNonQuery("UPDATE tbl_school_years SET is_current = IF(school_year_id = @id, 1, 0)",
                        New String() {"@id"}, New Object() {selectedId}) Then
            LogActivity("Update School Year", txtLabel.Text.Trim(), "Set " & txtLabel.Text.Trim() & " as the current school year")
            LoadList()
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs)
        If BlockedForRole() Then Exit Sub
        If selectedId = 0 Then
            MsgBox("Click a school year in the list first.", vbExclamation, "School Year")
            Exit Sub
        End If
        Dim isCur As Integer = Convert.ToInt32(If(ExecScalar("SELECT is_current FROM tbl_school_years WHERE school_year_id = @id",
                                                             New String() {"@id"}, New Object() {selectedId}), 0))
        If isCur = 1 Then
            MsgBox("You cannot delete the current school year. Set another one as current first.", vbExclamation, "School Year")
            Exit Sub
        End If
        If MsgBox("Delete school year " & txtLabel.Text.Trim() & "? Transactions are not deleted.",
                  vbYesNo + vbQuestion, "School Year") <> MsgBoxResult.Yes Then Exit Sub
        If ExecNonQuery("DELETE FROM tbl_school_years WHERE school_year_id = @id",
                        New String() {"@id"}, New Object() {selectedId}) Then
            LogActivity("Delete School Year", txtLabel.Text.Trim(), "Deleted school year " & txtLabel.Text.Trim())
            LoadList()
            ClearForm()
        End If
    End Sub

End Class