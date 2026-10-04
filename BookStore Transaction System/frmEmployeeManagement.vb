Imports System.Text.RegularExpressions

Public Class frmEmployeeManagement

    Private selectedEmployeeId As Integer = 0

    Private Sub frmEmployeeManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupFooter(Me, lblname, lblposition, lbldatetime)

        Relabel(Me, New Dictionary(Of String, String) From {
            {"Student No.", "Employee No."}, {"Grade Level", "Department"}, {"Program/Strand", "Position"},
            {"Section", "Status"}, {"View and Manage All Students", "View and Manage All Employees"},
            {"List of Students", "List of Employees"}, {"Student Information", "Employee Information"}})

        dgvstudents.AutoGenerateColumns = False
        BindColumn("StudentNo", "employee_no", "Employee No.")
        BindColumn("LastName", "last_name", "Last Name")
        BindColumn("FirstName", "first_name", "First Name")
        BindColumn("GradeLevel", "department", "Department")
        BindColumn("ProgramStrand", "job_position", "Position")
        BindColumn("Section", "status", "Status")
        dgvstudents.Columns("EducationalLevel").Visible = False
        If Not dgvstudents.Columns.Contains("colId") Then
            dgvstudents.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colId", .DataPropertyName = "employee_id", .Visible = False})
        End If

        cboGradeLevel.DropDownStyle = ComboBoxStyle.DropDown
        cboGradeLevel.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboGradeLevel.AutoCompleteSource = AutoCompleteSource.ListItems
        cboProgram.DropDownStyle = ComboBoxStyle.DropDown
        cboProgram.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboProgram.AutoCompleteSource = AutoCompleteSource.ListItems
        cboSection.DropDownStyle = ComboBoxStyle.DropDownList
        cboSection.Items.Clear()
        cboSection.Items.AddRange(New Object() {"Active", "Inactive"})

        LoadLists()
        LoadGrid("")
        ClearFields()
    End Sub

    Private Sub BindColumn(colName As String, field As String, header As String)
        dgvstudents.Columns(colName).DataPropertyName = field
        dgvstudents.Columns(colName).HeaderText = header
    End Sub

    Private Sub Relabel(parent As Control, map As Dictionary(Of String, String))
        For Each c As Control In parent.Controls
            If TypeOf c Is Label AndAlso map.ContainsKey(c.Text.Trim()) Then c.Text = map(c.Text.Trim())
            If c.HasChildren Then Relabel(c, map)
        Next
    End Sub

    Private Sub LoadLists()
        cboGradeLevel.Items.Clear()
        Dim d As DataTable = GetDataTable("SELECT DISTINCT department FROM tbl_employees WHERE department IS NOT NULL AND department <> '' ORDER BY department")
        For Each r As DataRow In d.Rows : cboGradeLevel.Items.Add(r("department").ToString()) : Next

        cboProgram.Items.Clear()
        Dim p As DataTable = GetDataTable("SELECT DISTINCT job_position FROM tbl_employees WHERE job_position IS NOT NULL AND job_position <> '' ORDER BY job_position")
        For Each r As DataRow In p.Rows : cboProgram.Items.Add(r("job_position").ToString()) : Next
    End Sub

    Private Sub txtstudentno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtstudentno.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> "-"c AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub NameBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtlastname.KeyPress, txtfirstname.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Function ValidateInputs() As Boolean
        If Not Regex.IsMatch(txtstudentno.Text.Trim(), "^[A-Za-z0-9-]{3,20}$") Then
            MsgBox("Employee No. is required (3-20 letters, numbers or dashes).", vbExclamation, "Validation Error")
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
        If String.IsNullOrWhiteSpace(cboGradeLevel.Text) Then
            MsgBox("Department is required.", vbExclamation, "Validation Error")
            cboGradeLevel.Focus() : Return False
        End If
        If String.IsNullOrWhiteSpace(cboProgram.Text) Then
            MsgBox("Position is required.", vbExclamation, "Validation Error")
            cboProgram.Focus() : Return False
        End If
        If cboSection.SelectedIndex < 0 Then
            MsgBox("Please select the Status.", vbExclamation, "Validation Error")
            cboSection.Focus() : Return False
        End If
        Return True
    End Function

    Private Sub LoadGrid(searchText As String)
        Dim dt As DataTable = GetDataTable(
            "SELECT employee_id, employee_no, last_name, first_name, department, job_position, status FROM tbl_employees " &
            "WHERE employee_no LIKE @s OR last_name LIKE @s OR first_name LIKE @s OR department LIKE @s OR job_position LIKE @s " &
            "ORDER BY last_name, first_name",
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

        selectedEmployeeId = Convert.ToInt32(row.Cells("colId").Value)
        txtstudentno.Text = Convert.ToString(row.Cells("StudentNo").Value)
        txtlastname.Text = Convert.ToString(row.Cells("LastName").Value)
        txtfirstname.Text = Convert.ToString(row.Cells("FirstName").Value)
        cboGradeLevel.Text = Convert.ToString(row.Cells("GradeLevel").Value)
        cboProgram.Text = Convert.ToString(row.Cells("ProgramStrand").Value)
        cboSection.SelectedIndex = cboSection.FindStringExact(Convert.ToString(row.Cells("Section").Value))
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If Not ValidateInputs() Then Exit Sub

        Dim count As Integer = Convert.ToInt32(ExecScalar("SELECT COUNT(*) FROM tbl_employees WHERE employee_no = @n",
                                                           New String() {"@n"}, New Object() {txtstudentno.Text.Trim()}))
        If count > 0 Then
            MsgBox("An employee with this Employee No. already exists.", vbExclamation, "Duplicate Entry")
            Exit Sub
        End If

        If ExecNonQuery(
            "INSERT INTO tbl_employees (employee_no, last_name, first_name, department, job_position, status) VALUES (@n, @ln, @fn, @d, @p, @s)",
            New String() {"@n", "@ln", "@fn", "@d", "@p", "@s"},
            New Object() {txtstudentno.Text.Trim(), txtlastname.Text.Trim(), txtfirstname.Text.Trim(),
                          cboGradeLevel.Text.Trim(), cboProgram.Text.Trim(), cboSection.Text}) Then
            LogActivity("Add Employee", txtstudentno.Text.Trim(), "Added " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
            MsgBox("Employee added successfully.", vbInformation, "Success")
            ClearFields()
            LoadLists()
            LoadGrid(txtSearch.Text.Trim())
        Else
            MsgBox("Failed to add employee.", vbExclamation, "Database Error")
        End If
    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        If selectedEmployeeId = 0 Then
            MsgBox("Please select an employee from the list to update.", vbExclamation, "No Selection")
            Exit Sub
        End If
        If Not ValidateInputs() Then Exit Sub

        Dim count As Integer = Convert.ToInt32(ExecScalar("SELECT COUNT(*) FROM tbl_employees WHERE employee_no = @n AND employee_id <> @id",
                                                           New String() {"@n", "@id"}, New Object() {txtstudentno.Text.Trim(), selectedEmployeeId}))
        If count > 0 Then
            MsgBox("Another employee is already using this Employee No.", vbExclamation, "Duplicate Entry")
            Exit Sub
        End If

        If MsgBox("Save the changes to " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim() & "?", vbYesNo + vbQuestion, "Confirm Update") <> MsgBoxResult.Yes Then Exit Sub

        If ExecNonQuery(
            "UPDATE tbl_employees SET employee_no=@n, last_name=@ln, first_name=@fn, department=@d, job_position=@p, status=@s WHERE employee_id=@id",
            New String() {"@n", "@ln", "@fn", "@d", "@p", "@s", "@id"},
            New Object() {txtstudentno.Text.Trim(), txtlastname.Text.Trim(), txtfirstname.Text.Trim(),
                          cboGradeLevel.Text.Trim(), cboProgram.Text.Trim(), cboSection.Text, selectedEmployeeId}) Then
            LogActivity("Update Employee", txtstudentno.Text.Trim(), "Updated " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
            MsgBox("Employee updated successfully.", vbInformation, "Success")
            ClearFields()
            LoadLists()
            LoadGrid(txtSearch.Text.Trim())
        Else
            MsgBox("Failed to update employee.", vbExclamation, "Database Error")
        End If
    End Sub

    Private Sub btnremove_Click(sender As Object, e As EventArgs) Handles btnremove.Click
        If selectedEmployeeId = 0 Then
            MsgBox("Please select an employee from the list to remove.", vbExclamation, "No Selection")
            Exit Sub
        End If
        If MsgBox("Are you sure you want to delete this employee record?", vbYesNo + vbQuestion, "Confirm Delete") <> MsgBoxResult.Yes Then Exit Sub

        If ExecNonQuery("DELETE FROM tbl_employees WHERE employee_id = @id", New String() {"@id"}, New Object() {selectedEmployeeId}) Then
            LogActivity("Delete Employee", txtstudentno.Text.Trim(), "Deleted " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
            MsgBox("Employee record deleted.", vbInformation, "Success")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        End If
    End Sub

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        If MsgBox("Clear all the fields?", vbYesNo + vbQuestion, "Clear Fields") <> MsgBoxResult.Yes Then Exit Sub
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedEmployeeId = 0
        txtstudentno.Clear()
        txtlastname.Clear()
        txtfirstname.Clear()
        cboGradeLevel.SelectedIndex = -1
        cboGradeLevel.Text = ""
        cboProgram.SelectedIndex = -1
        cboProgram.Text = ""
        cboSection.SelectedIndex = 0
        txtSearch.Clear()
        dgvstudents.ClearSelection()
    End Sub

End Class