Imports MySql.Data.MySqlClient

Public Class frmStudentList

    ' ---- values returned to the caller (frmPOS) ----
    Public Property InitialSearch As String = ""
    Public Property SelectedStudentId As Integer = 0
    Public Property SelectedStudentNo As String = ""
    Public Property SelectedStudentName As String = ""      ' "First Last"
    Public Property SelectedGradeLevel As String = ""
    Public Property SelectedProgramStrand As String = ""
    Public Property SelectedSection As String = ""

    Private isLoading As Boolean = True

    Private Sub frmStudentList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvstudents.AllowUserToAddRows = False
        dgvstudents.AllowUserToDeleteRows = False
        dgvstudents.MultiSelect = False
        dgvstudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        cboGradeLevel.DropDownStyle = ComboBoxStyle.DropDownList
        cboGradeLevel.Items.Clear()
        cboGradeLevel.Items.Add("All Grade Levels")
        cboGradeLevel.Items.AddRange(New Object() {
            "Kinder", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6",
            "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12",
            "1st Year College", "2nd Year College", "3rd Year College", "4th Year College"})
        cboGradeLevel.SelectedIndex = 0

        txtSearch.Text = InitialSearch      ' whatever was typed in the POS Student No. box
        isLoading = False
        LoadStudents()
        txtSearch.Focus()
    End Sub

    Private Sub LoadStudents()
        Dim grade As String = If(cboGradeLevel.SelectedIndex <= 0, "", cboGradeLevel.Text)
        Dim kw As String = txtSearch.Text.Trim()

        Dim dt As DataTable = GetDataTable(
            "SELECT student_id, student_no, last_name, first_name, education_level, grade_level, program_strand, section " &
            "FROM tbl_students " &
            "WHERE (student_no LIKE @s OR last_name LIKE @s OR first_name LIKE @s OR CONCAT(first_name, ' ', last_name) LIKE @s) " &
            "AND (@g = '' OR grade_level = @g) " &
            "ORDER BY last_name, first_name",
            New String() {"@s", "@g"}, New Object() {"%" & kw & "%", grade})

        dgvstudents.Rows.Clear()
        For Each r As DataRow In dt.Rows
            Dim idx As Integer = dgvstudents.Rows.Add(
                r("student_no").ToString(),
                r("last_name").ToString() & ", " & r("first_name").ToString(),
                r("education_level").ToString(),
                r("grade_level").ToString(),
                r("program_strand").ToString(),
                r("section").ToString())
            dgvstudents.Rows(idx).Tag = r
        Next
        dgvstudents.ClearSelection()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Not isLoading Then LoadStudents()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        LoadStudents()
    End Sub

    Private Sub cboGradeLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGradeLevel.SelectedIndexChanged
        If Not isLoading Then LoadStudents()
    End Sub

    ' ---- choose a student: double-click a row or press Enter ----
    Private Sub dgvstudents_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvstudents.CellDoubleClick
        If e.RowIndex >= 0 Then SelectCurrent()
    End Sub

    Private Sub dgvstudents_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvstudents.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SelectCurrent()
        End If
    End Sub

    Private Sub SelectCurrent()
        If dgvstudents.CurrentRow Is Nothing OrElse Not dgvstudents.CurrentRow.Selected Then
            MsgBox("Click a student first.", vbExclamation, "Student List")
            Exit Sub
        End If

        Dim r As DataRow = TryCast(dgvstudents.CurrentRow.Tag, DataRow)
        If r Is Nothing Then Exit Sub

        SelectedStudentId = Convert.ToInt32(r("student_id"))
        SelectedStudentNo = r("student_no").ToString()
        SelectedStudentName = r("first_name").ToString() & " " & r("last_name").ToString()
        SelectedGradeLevel = r("grade_level").ToString()
        SelectedProgramStrand = r("program_strand").ToString()
        SelectedSection = r("section").ToString()

        If Me.Modal Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

End Class