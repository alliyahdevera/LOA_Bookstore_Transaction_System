Imports MySql.Data.MySqlClient

Public Class frmStudentManagement

    Private selectedStudentId As Integer = 0

    ' ------------------------------------------------------------------
    ' Form Load & Initialization
    ' ------------------------------------------------------------------
    Private Sub frmStudentManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblname.Text = currentuser.FullName
        lblposition.Text = currentuser.Role
        lblDateTime.Text = DateTime.Now.ToString("MMMM dd, yyyy")

        ' Lock ComboBoxes so users can only pick allowed items (prevents free typing)
        cboGradeLevel.DropDownStyle = ComboBoxStyle.DropDownList
        cboSection.DropDownStyle = ComboBoxStyle.DropDownList

        PopulateDropdowns()
        LoadGrid("")
        ClearFields()
    End Sub

    Private Sub PopulateDropdowns()
        ' Grade Level options
        cboGradeLevel.Items.Clear()
        cboGradeLevel.Items.AddRange(New Object() {
            "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6",
            "Grade 7", "Grade 8", "Grade 9", "Grade 10", "Grade 11", "Grade 12",
            "1st Year College", "2nd Year College", "3rd Year College", "4th Year College"
        })

        ' Section options (includes 11M1-11M4, 21A1-21A4, 31E1-31E4, 41E1-41E4, and database values)
        cboSection.Items.Clear()
        cboSection.Items.AddRange(New Object() {
            "11M1", "11M2", "11M3", "11M4",
            "21A1", "21A2", "21A3", "21A4",
            "31E1", "31E2", "31E3", "31E4",
            "41E1", "41E2", "41E3", "41E4",
            "Sampaguita", "Rosa", "Narra", "Molave", "Newton", "Einstein",
            "STEM-A", "ABM B", "BSIT 1A", "BSCS-2B", "31L1", "31F1", "31F3"
        })
    End Sub

    ' ------------------------------------------------------------------
    ' Field Input Validations (Keypress Restrictions)
    ' ------------------------------------------------------------------

    ' Student No: Numbers and Hyphens only
    ' ------------------------------------------------------------------
    ' Field Input Validations (Keypress Restrictions)
    ' ------------------------------------------------------------------

    ' Student No: Numbers and Hyphens only
    Private Sub txtstudentno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtstudentno.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "-"c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' Last Name: Letters, Spaces, Hyphens, and Dots only
    Private Sub txtlastname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtlastname.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' First Name: Letters, Spaces, Hyphens, and Dots only
    Private Sub txtfirstname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtfirstname.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' Strict Validation Function before Saving
    Private Function ValidateInputs() As Boolean
        ' Mandatory fields check
        If String.IsNullOrWhiteSpace(txtstudentno.Text) Then
            MsgBox("Student Number is required.", vbExclamation, "Validation Error")
            txtstudentno.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtlastname.Text) Then
            MsgBox("Last Name is required.", vbExclamation, "Validation Error")
            txtlastname.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtfirstname.Text) Then
            MsgBox("First Name is required.", vbExclamation, "Validation Error")
            txtfirstname.Focus()
            Return False
        End If

        ' Dropdown selection checks
        If cboGradeLevel.SelectedIndex = -1 OrElse Not cboGradeLevel.Items.Contains(cboGradeLevel.Text) Then
            MsgBox("Please select a valid Grade Level from the list.", vbExclamation, "Validation Error")
            cboGradeLevel.Focus()
            Return False
        End If

        If cboSection.SelectedIndex = -1 OrElse Not cboSection.Items.Contains(cboSection.Text) Then
            MsgBox("Please select a valid Section from the list.", vbExclamation, "Validation Error")
            cboSection.Focus()
            Return False
        End If

        Return True
    End Function
    Public Sub LoadGrid(searchText As String)
        Try
            If Not connection() Then Exit Sub

            ' Query restricted strictly to student_no OR last_name
            Dim query As String = "SELECT student_id, student_no, last_name, first_name, grade_level, section " &
                              "FROM tbl_students " &
                              "WHERE student_no LIKE @s OR last_name LIKE @s " &
                              "ORDER BY last_name, first_name"

            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@s", "%" & searchText & "%")
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    DataGridView1.Rows.Clear()
                    While localDr.Read()
                        Dim idx As Integer = DataGridView1.Rows.Add(
                        localDr("student_no").ToString(),
                        localDr("last_name").ToString(),
                        localDr("first_name").ToString(),
                        localDr("grade_level").ToString(),
                        localDr("section").ToString()
                    )
                        DataGridView1.Rows(idx).Tag = Convert.ToInt32(localDr("student_id"))
                    End While
                End Using
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading students: " & ex.Message, vbCritical, "Database Error")
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        If row.Tag Is Nothing Then Exit Sub

        selectedStudentId = Convert.ToInt32(row.Tag)
        txtstudentno.Text = row.Cells(0).Value.ToString()
        txtlastname.Text = row.Cells(1).Value.ToString()
        txtfirstname.Text = row.Cells(2).Value.ToString()
        cboGradeLevel.Text = row.Cells(3).Value.ToString()
        cboSection.Text = row.Cells(4).Value.ToString()
    End Sub

    ' ------------------------------------------------------------------
    ' Button Action Handlers
    ' ------------------------------------------------------------------

    ' ADD BUTTON
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If Not ValidateInputs() Then Exit Sub

        Try
            Dim checkSql As String = "SELECT COUNT(*) FROM tbl_students WHERE student_no = @sn"
            Dim count As Integer = Convert.ToInt32(ExecScalar(checkSql, New String() {"@sn"}, New Object() {txtstudentno.Text.Trim()}))
            If count > 0 Then
                MsgBox("A student with this Student No. already exists.", vbExclamation, "Duplicate Entry")
                Exit Sub
            End If

            Dim insertSql As String = "INSERT INTO tbl_students (student_no, last_name, first_name, grade_level, section) " &
                                      "VALUES (@sn, @ln, @fn, @gl, @sec)"

            Dim ok As Boolean = ExecNonQuery(
                insertSql,
                New String() {"@sn", "@ln", "@fn", "@gl", "@sec"},
                New Object() {
                    txtstudentno.Text.Trim(),
                    txtlastname.Text.Trim(),
                    txtfirstname.Text.Trim(),
                    cboGradeLevel.Text.Trim(),
                    cboSection.Text.Trim()
                })

            If ok Then
                MsgBox("Student added successfully.", vbInformation, "Success")
                ClearFields()
                LoadGrid(txtSearch.Text.Trim())
            Else
                MsgBox("Failed to add student.", vbExclamation, "Database Error")
            End If

        Catch ex As Exception
            MsgBox("Error adding student: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    ' UPDATE BUTTON
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        If selectedStudentId = 0 Then
            MsgBox("Please select a student from the list to update.", vbExclamation, "No Selection")
            Exit Sub
        End If

        If Not ValidateInputs() Then Exit Sub

        Try
            Dim checkSql As String = "SELECT COUNT(*) FROM tbl_students WHERE student_no = @sn AND student_id <> @id"
            Dim count As Integer = Convert.ToInt32(ExecScalar(checkSql, New String() {"@sn", "@id"}, New Object() {txtstudentno.Text.Trim(), selectedStudentId}))
            If count > 0 Then
                MsgBox("Another student is already using this Student No.", vbExclamation, "Duplicate Entry")
                Exit Sub
            End If

            Dim updateSql As String = "UPDATE tbl_students SET student_no = @sn, last_name = @ln, " &
                                      "first_name = @fn, grade_level = @gl, section = @sec " &
                                      "WHERE student_id = @id"

            Dim ok As Boolean = ExecNonQuery(
                updateSql,
                New String() {"@sn", "@ln", "@fn", "@gl", "@sec", "@id"},
                New Object() {
                    txtstudentno.Text.Trim(),
                    txtlastname.Text.Trim(),
                    txtfirstname.Text.Trim(),
                    cboGradeLevel.Text.Trim(),
                    cboSection.Text.Trim(),
                    selectedStudentId
                })

            If ok Then
                MsgBox("Student updated successfully.", vbInformation, "Success")
                ClearFields()
                LoadGrid(txtSearch.Text.Trim())
            Else
                MsgBox("Failed to update student.", vbExclamation, "Database Error")
            End If

        Catch ex As Exception
            MsgBox("Error updating student: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    ' REMOVE BUTTON
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnremove.Click
        If selectedStudentId = 0 Then
            MsgBox("Please select a student from the list to remove.", vbExclamation, "No Selection")
            Exit Sub
        End If

        If MsgBox("Are you sure you want to delete this student record?", vbYesNo + vbQuestion, "Confirm Delete") <> MsgBoxResult.Yes Then
            Exit Sub
        End If

        Try
            Dim deleteSql As String = "DELETE FROM tbl_students WHERE student_id = @id"
            Dim ok As Boolean = ExecNonQuery(deleteSql, New String() {"@id"}, New Object() {selectedStudentId})

            If ok Then
                MsgBox("Student record deleted.", vbInformation, "Success")
                ClearFields()
                LoadGrid(txtSearch.Text.Trim())
            Else
                MsgBox("Could not delete record. It may be referenced in existing transactions.", vbExclamation, "Delete Blocked")
            End If

        Catch ex As Exception
            MsgBox("Error deleting student: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    ' CLEAR BUTTON
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedStudentId = 0
        txtstudentno.Clear()
        txtlastname.Clear()
        txtfirstname.Clear()
        cboGradeLevel.SelectedIndex = -1
        cboSection.SelectedIndex = -1
        txtSearch.Clear()
        DataGridView1.ClearSelection()
    End Sub

End Class