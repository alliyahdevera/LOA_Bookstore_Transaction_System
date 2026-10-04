Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions

Public Class frmStudentManagement

    Private selectedStudentId As Integer = 0
    Private isFilling As Boolean = False

    Private ReadOnly GradeLevels As String() = {
        "Kinder", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6",
        "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12",
        "1st Year College", "2nd Year College", "3rd Year College", "4th Year College"}
    Private ReadOnly ShsStrands As String() = {"STEM", "ABM", "HUMSS", "GAS", "ICT", "HE", "IA"}
    Private ReadOnly CollegePrograms As String() = {
        "BSPsych", "BSA", "BSCA", "BSBA", "BSCS", "BSIT", "BSCrim", "BTVTED",
        "BSCpE", "BSIE", "BSREM", "BSTM", "BSHM", "JD"}

    Private Sub frmStudentManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        BindGridColumns()

        cboGradeLevel.DropDownStyle = ComboBoxStyle.DropDownList
        cboProgram.DropDownStyle = ComboBoxStyle.DropDownList
        cboSection.DropDownStyle = ComboBoxStyle.DropDown
        cboSection.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboSection.AutoCompleteSource = AutoCompleteSource.ListItems

        cboGradeLevel.Items.Clear()
        For Each g As String In GradeLevels
            cboGradeLevel.Items.Add(g)
        Next

        LoadGrid("")
        ClearFields()
    End Sub

    ' Grid columns map to database fields by name, so values can never shift columns.
    Private Sub BindGridColumns()
        dgvstudents.AutoGenerateColumns = False
        dgvstudents.Columns("StudentNo").DataPropertyName = "student_no"
        dgvstudents.Columns("LastName").DataPropertyName = "last_name"
        dgvstudents.Columns("FirstName").DataPropertyName = "first_name"
        dgvstudents.Columns("EducationalLevel").DataPropertyName = "education_level"
        dgvstudents.Columns("GradeLevel").DataPropertyName = "grade_level"
        dgvstudents.Columns("ProgramStrand").DataPropertyName = "program_strand"
        dgvstudents.Columns("Section").DataPropertyName = "section"

        If Not dgvstudents.Columns.Contains("colId") Then
            dgvstudents.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colId", .DataPropertyName = "student_id", .Visible = False})
        End If
    End Sub

    Private Function LevelOf(grade As String) As String
        Dim g As String = If(grade, "").Trim()
        If g = "" Then Return ""
        If g.IndexOf("Year", StringComparison.OrdinalIgnoreCase) >= 0 Then Return "College"
        If g.Equals("Kinder", StringComparison.OrdinalIgnoreCase) Then Return "Grade School"
        Dim n As Integer
        If g.StartsWith("Grade ", StringComparison.OrdinalIgnoreCase) AndAlso Integer.TryParse(g.Substring(6), n) Then
            If n <= 6 Then Return "Grade School"
            If n <= 10 Then Return "Junior High School"
            Return "Senior High School"
        End If
        Return ""
    End Function

    ' ---------- year level drives Program/Strand and Section ----------
    Private Sub LoadProgramOptions()
        Dim level As String = LevelOf(cboGradeLevel.Text)
        cboProgram.Items.Clear()

        Select Case level
            Case "Senior High School"
                For Each s As String In ShsStrands : cboProgram.Items.Add(s) : Next
                cboProgram.Enabled = True
            Case "College"
                For Each s As String In CollegePrograms : cboProgram.Items.Add(s) : Next
                cboProgram.Enabled = True
            Case Else
                cboProgram.Items.Add("N/A")
                cboProgram.SelectedIndex = 0
                cboProgram.Enabled = False
        End Select
    End Sub

    Private Sub LoadSectionOptions()
        cboSection.Items.Clear()
        If cboGradeLevel.SelectedIndex < 0 Then Exit Sub

        Dim prog As String = If(cboProgram.SelectedIndex >= 0 AndAlso cboProgram.Text <> "N/A", cboProgram.Text, "")
        Dim dt As DataTable = GetDataTable(
            "SELECT DISTINCT section FROM tbl_students " &
            "WHERE grade_level = @g AND section IS NOT NULL AND section <> '' AND (@p = '' OR program_strand = @p) ORDER BY section",
            New String() {"@g", "@p"}, New Object() {cboGradeLevel.Text, prog})
        For Each r As DataRow In dt.Rows
            cboSection.Items.Add(r("section").ToString())
        Next
    End Sub

    Private Sub cboGradeLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGradeLevel.SelectedIndexChanged
        If isFilling Then Exit Sub
        LoadProgramOptions()
        cboSection.Text = ""
        LoadSectionOptions()
    End Sub

    Private Sub cboProgram_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProgram.SelectedIndexChanged
        If isFilling Then Exit Sub
        cboSection.Text = ""
        LoadSectionOptions()
    End Sub

    ' ---------- input restrictions ----------
    Private Sub txtstudentno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtstudentno.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "-"c AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub NameBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtlastname.KeyPress, txtfirstname.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Function ValidateInputs() As Boolean
        If Not Regex.IsMatch(txtstudentno.Text.Trim(), "^\d{2,6}-\d{2}$") Then
            MsgBox("Student Number is required and must look like 0000-00.", vbExclamation, "Validation Error")
            txtstudentno.Focus() : Return False
        End If
        If String.IsNullOrWhiteSpace(txtlastname.Text) Then
            MsgBox("Last Name is required.", vbExclamation, "Validation Error")
            txtlastname.Focus() : Return False
        End If
        If String.IsNullOrWhiteSpace(txtfirstname.Text) Then
            MsgBox("First Name is required.", vbExclamation, "Validation Error")
            txtfirstname.Focus() : Return False
        End If
        If cboGradeLevel.SelectedIndex < 0 Then
            MsgBox("Please select a Grade Level.", vbExclamation, "Validation Error")
            cboGradeLevel.Focus() : Return False
        End If
        Dim level As String = LevelOf(cboGradeLevel.Text)
        If (level = "Senior High School" OrElse level = "College") AndAlso cboProgram.SelectedIndex < 0 Then
            MsgBox("Please select the Program/Strand for this year level.", vbExclamation, "Validation Error")
            cboProgram.Focus() : Return False
        End If
        If String.IsNullOrWhiteSpace(cboSection.Text) Then
            MsgBox("Section is required.", vbExclamation, "Validation Error")
            cboSection.Focus() : Return False
        End If
        If cboSection.Text.Trim().Length > 50 Then
            MsgBox("Section must be 50 characters or less.", vbExclamation, "Validation Error")
            cboSection.Focus() : Return False
        End If
        Return True
    End Function

    Private Function ProgramValue() As Object
        If cboProgram.SelectedIndex < 0 OrElse cboProgram.Text = "N/A" Then Return DBNull.Value
        Return cboProgram.Text.Trim()
    End Function

    ' ---------- grid ----------
    Public Sub LoadGrid(searchText As String)
        Dim dt As DataTable = GetDataTable(
            "SELECT student_id, student_no, last_name, first_name, education_level, grade_level, program_strand, section " &
            "FROM tbl_students WHERE student_no LIKE @s OR last_name LIKE @s OR first_name LIKE @s ORDER BY last_name, first_name",
            New String() {"@s"}, New Object() {"%" & searchText & "%"})
        dgvstudents.DataSource = dt
        dgvstudents.ClearSelection()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub dgvstudents_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvstudents.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = dgvstudents.Rows(e.RowIndex)
        If row.Cells("colId").Value Is Nothing OrElse IsDBNull(row.Cells("colId").Value) Then Exit Sub

        selectedStudentId = Convert.ToInt32(row.Cells("colId").Value)
        isFilling = True
        txtstudentno.Text = Convert.ToString(row.Cells("StudentNo").Value)
        txtlastname.Text = Convert.ToString(row.Cells("LastName").Value)
        txtfirstname.Text = Convert.ToString(row.Cells("FirstName").Value)

        cboGradeLevel.SelectedIndex = cboGradeLevel.FindStringExact(Convert.ToString(row.Cells("GradeLevel").Value))
        LoadProgramOptions()
        Dim prog As String = Convert.ToString(row.Cells("ProgramStrand").Value)
        If cboProgram.Enabled Then cboProgram.SelectedIndex = cboProgram.FindStringExact(prog)
        LoadSectionOptions()
        cboSection.Text = Convert.ToString(row.Cells("Section").Value)
        isFilling = False
    End Sub

    ' ---------- buttons ----------
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If Not ValidateInputs() Then Exit Sub

        Dim count As Integer = Convert.ToInt32(ExecScalar("SELECT COUNT(*) FROM tbl_students WHERE student_no = @sn",
                                                           New String() {"@sn"}, New Object() {txtstudentno.Text.Trim()}))
        If count > 0 Then
            MsgBox("A student with this Student No. already exists.", vbExclamation, "Duplicate Entry")
            Exit Sub
        End If

        Dim ok As Boolean = ExecNonQuery(
            "INSERT INTO tbl_students (student_no, last_name, first_name, education_level, grade_level, program_strand, section) " &
            "VALUES (@sn, @ln, @fn, @el, @gl, @ps, @sec)",
            New String() {"@sn", "@ln", "@fn", "@el", "@gl", "@ps", "@sec"},
            New Object() {txtstudentno.Text.Trim(), txtlastname.Text.Trim(), txtfirstname.Text.Trim(),
                          LevelOf(cboGradeLevel.Text), cboGradeLevel.Text, ProgramValue(), cboSection.Text.Trim()})

        If ok Then
            LogActivity("Add Student", txtstudentno.Text.Trim(), "Added " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
            MsgBox("Student added successfully.", vbInformation, "Success")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        Else
            MsgBox("Failed to add student.", vbExclamation, "Database Error")
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        If selectedStudentId = 0 Then
            MsgBox("Please select a student from the list to update.", vbExclamation, "No Selection")
            Exit Sub
        End If
        If Not ValidateInputs() Then Exit Sub

        Dim count As Integer = Convert.ToInt32(ExecScalar("SELECT COUNT(*) FROM tbl_students WHERE student_no = @sn AND student_id <> @id",
                                                           New String() {"@sn", "@id"}, New Object() {txtstudentno.Text.Trim(), selectedStudentId}))
        If count > 0 Then
            MsgBox("Another student is already using this Student No.", vbExclamation, "Duplicate Entry")
            Exit Sub
        End If

        If MsgBox("Save the changes to " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim() & "?", vbYesNo + vbQuestion, "Confirm Update") <> MsgBoxResult.Yes Then Exit Sub

        Dim ok As Boolean = ExecNonQuery(
            "UPDATE tbl_students SET student_no = @sn, last_name = @ln, first_name = @fn, education_level = @el, " &
            "grade_level = @gl, program_strand = @ps, section = @sec WHERE student_id = @id",
            New String() {"@sn", "@ln", "@fn", "@el", "@gl", "@ps", "@sec", "@id"},
            New Object() {txtstudentno.Text.Trim(), txtlastname.Text.Trim(), txtfirstname.Text.Trim(),
                          LevelOf(cboGradeLevel.Text), cboGradeLevel.Text, ProgramValue(), cboSection.Text.Trim(), selectedStudentId})

        If ok Then
            LogActivity("Update Student", txtstudentno.Text.Trim(), "Updated " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
            MsgBox("Student updated successfully.", vbInformation, "Success")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        Else
            MsgBox("Failed to update student.", vbExclamation, "Database Error")
        End If
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnremove.Click
        If selectedStudentId = 0 Then
            MsgBox("Please select a student from the list to remove.", vbExclamation, "No Selection")
            Exit Sub
        End If
        If MsgBox("Are you sure you want to delete this student record?", vbYesNo + vbQuestion, "Confirm Delete") <> MsgBoxResult.Yes Then Exit Sub

        If ExecNonQuery("DELETE FROM tbl_students WHERE student_id = @id", New String() {"@id"}, New Object() {selectedStudentId}) Then
            LogActivity("Delete Student", txtstudentno.Text.Trim(), "Deleted " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
            MsgBox("Student record deleted.", vbInformation, "Success")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        Else
            MsgBox("Could not delete record. It may be referenced in existing transactions.", vbExclamation, "Delete Blocked")
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        If MsgBox("Clear all the fields?", vbYesNo + vbQuestion, "Clear Fields") <> MsgBoxResult.Yes Then Exit Sub
        ClearFields()
    End Sub

    Private Sub ClearFields()
        isFilling = True
        selectedStudentId = 0
        txtstudentno.Clear()
        txtlastname.Clear()
        txtfirstname.Clear()
        cboGradeLevel.SelectedIndex = -1
        cboProgram.Items.Clear()
        cboProgram.Enabled = True
        cboSection.Items.Clear()
        cboSection.Text = ""
        txtSearch.Clear()
        dgvstudents.ClearSelection()
        isFilling = False
    End Sub

End Class