Imports System.Text.RegularExpressions

Public Class frmEmployeeManagement

    Private pg As GridPager
    Private selectedEmployeeId As Integer = 0
    Public Property InitialSearch As String = ""
    Private Function NextEmployeeNo() As String
        Dim v As Object = ExecScalar(
        "SELECT MAX(CAST(SUBSTRING(employee_no, 5) AS UNSIGNED)) FROM tbl_employees WHERE employee_no REGEXP '^EMP-[0-9]+$'")
        Dim n As Integer = 0
        If v IsNot Nothing AndAlso Not IsDBNull(v) Then n = Convert.ToInt32(v)
        Return "EMP-" & (n + 1).ToString("0000")
    End Function
    Private Sub frmEmployeeManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySearchPlaceholders(Me)
        SetupFooter(Me, lblname, lblposition, lbldatetime)
        Relabel(Me, New Dictionary(Of String, String) From {
            {"Grade Level", "Department"},
            {"Program/Strand", "Position"},
            {"Section", "Status"},
            {"View and Manage All Students", "View and Manage All Employees"},
            {"List of Students", "List of Employees"},
            {"Student Information", "Employee Information"}
        })

        AddHandler dgvemployee.CellClick, AddressOf dgvstudents_CellClick
        BindColumn("EmployeeNo", "employee_no", "Employee ID No.")
        BindColumn("LastName", "last_name", "Last Name")
        BindColumn("Firstname", "first_name", "First Name")
        BindColumn("Department", "department", "Department")
        BindColumn("Position", "job_position", "Position")
        BindColumn("Status", "status", "Status")
        If Not dgvemployee.Columns.Contains("colId") Then
            dgvemployee.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colId", .DataPropertyName = "employee_id", .Visible = False})
        End If

        cbodepartment.DropDownStyle = ComboBoxStyle.DropDown
        cbodepartment.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cbodepartment.AutoCompleteSource = AutoCompleteSource.ListItems
        cboposition.DropDownStyle = ComboBoxStyle.DropDown
        cboposition.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboposition.AutoCompleteSource = AutoCompleteSource.ListItems
        cbostatus.DropDownStyle = ComboBoxStyle.DropDownList
        cbostatus.Items.Clear()
        cbostatus.Items.AddRange(New Object() {"Active", "Inactive"})
        pg = New GridPager(dgvemployee, 20)
        AddHandler pg.PageChanged, Sub() LoadGrid(txtSearch.Text.Trim())
        txtSearch.Text = InitialSearch
        LoadLists()
        LoadGrid("")
        txtemployeeno.ReadOnly = True
        ClearFields()
    End Sub

    Private Sub BindColumn(colName As String, field As String, header As String)
        dgvemployee.Columns(colName).DataPropertyName = field
        dgvemployee.Columns(colName).HeaderText = header
    End Sub

    Private Sub Relabel(parent As Control, map As Dictionary(Of String, String))
        For Each c As Control In parent.Controls
            If TypeOf c Is Label AndAlso map.ContainsKey(c.Text.Trim()) Then c.Text = map(c.Text.Trim())
            If c.HasChildren Then Relabel(c, map)
        Next
    End Sub

    Private ReadOnly DepartmentList As String() = {
    "CCS", "COE", "CBA", "CAS", "COED", "CTHM", "COA", "CCJ", "CCA",
    "Accounting", "Registrar", "Library", "Guidance", "HR", "Bookstore"
}

    Private Sub LoadLists()
        cbodepartment.Items.Clear()
        cbodepartment.Items.AddRange(DepartmentList)

        ' keep any department already saved in the database that is not in the list above
        Dim d As DataTable = GetDataTable("SELECT DISTINCT department FROM tbl_employees WHERE department IS NOT NULL AND department <> '' ORDER BY department")
        For Each r As DataRow In d.Rows
            Dim dept As String = r("department").ToString()
            If Not cbodepartment.Items.Contains(dept) Then cbodepartment.Items.Add(dept)
        Next

        cboposition.Items.Clear()
        Dim p As DataTable = GetDataTable("SELECT DISTINCT job_position FROM tbl_employees WHERE job_position IS NOT NULL AND job_position <> '' ORDER BY job_position")
        For Each r As DataRow In p.Rows : cboposition.Items.Add(r("job_position").ToString()) : Next
    End Sub

    Private Sub txtstudentno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtemployeeno.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> "-"c AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub NameBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtlastname.KeyPress, txtfirstname.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Function ValidateInputs() As Boolean
        If Not Regex.IsMatch(txtemployeeno.Text.Trim(), "^[A-Za-z0-9-]{3,20}$") Then
            MsgBox("Employee ID No. is required (3-20 letters, numbers or dashes).", vbExclamation, "Validation Error")
            txtemployeeno.Focus() : Return False
        End If
        If String.IsNullOrWhiteSpace(txtlastname.Text) Then
            MsgBox("Last Name is required.", vbExclamation, "Validation Error")
            txtlastname.Focus() : Return False
        End If
        If String.IsNullOrWhiteSpace(txtfirstname.Text) Then
            MsgBox("First Name is required.", vbExclamation, "Validation Error")
            txtfirstname.Focus() : Return False
        End If
        If String.IsNullOrWhiteSpace(cbodepartment.Text) Then
            MsgBox("Department is required.", vbExclamation, "Validation Error")
            cbodepartment.Focus() : Return False
        End If
        If String.IsNullOrWhiteSpace(cboposition.Text) Then
            MsgBox("Position is required.", vbExclamation, "Validation Error")
            cboposition.Focus() : Return False
        End If
        If cbostatus.SelectedIndex < 0 Then
            MsgBox("Please select the Status.", vbExclamation, "Validation Error")
            cbostatus.Focus() : Return False
        End If
        Return True
    End Function

    Private Sub LoadGrid(searchText As String)
        If pg Is Nothing Then Exit Sub
        Dim dt As DataTable = pg.LoadPage(
            "SELECT employee_id, employee_no, last_name, first_name, department, job_position, status FROM tbl_employees " &
            "WHERE employee_no LIKE @s OR last_name LIKE @s OR first_name LIKE @s OR department LIKE @s OR job_position LIKE @s " &
            "ORDER BY last_name, first_name, employee_id",
            New String() {"@s"}, New Object() {"%" & searchText & "%"})
        dgvemployee.DataSource = dt
        dgvemployee.ClearSelection()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If pg Is Nothing Then Exit Sub
        pg.Reset()
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub dgvstudents_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = dgvemployee.Rows(e.RowIndex)
        If row.Cells("colId").Value Is Nothing OrElse IsDBNull(row.Cells("colId").Value) Then Exit Sub

        selectedEmployeeId = Convert.ToInt32(row.Cells("colId").Value)
        txtemployeeno.Text = Convert.ToString(row.Cells("EmployeeNo").Value)
        txtlastname.Text = Convert.ToString(row.Cells("LastName").Value)
        txtfirstname.Text = Convert.ToString(row.Cells("Firstname").Value)
        cbodepartment.Text = Convert.ToString(row.Cells("Department").Value)
        cboposition.Text = Convert.ToString(row.Cells("Position").Value)
        cbostatus.SelectedIndex = cbostatus.FindStringExact(Convert.ToString(row.Cells("Status").Value))
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If Not ValidateInputs() Then Exit Sub

        Dim count As Integer = Convert.ToInt32(ExecScalar("SELECT COUNT(*) FROM tbl_employees WHERE employee_no = @n",
                                                        New String() {"@n"}, New Object() {txtemployeeno.Text.Trim()}))
        If count > 0 Then
            MsgBox("An employee with this Employee ID No. already exists.", vbExclamation, "Duplicate Entry")
            Exit Sub
        End If

        If ExecNonQuery(
            "INSERT INTO tbl_employees (employee_no, last_name, first_name, department, job_position, status) VALUES (@n, @ln, @fn, @d, @p, @s)",
            New String() {"@n", "@ln", "@fn", "@d", "@p", "@s"},
            New Object() {txtemployeeno.Text.Trim(), txtlastname.Text.Trim(), txtfirstname.Text.Trim(),
                          cbodepartment.Text.Trim(), cboposition.Text.Trim(), cbostatus.Text}) Then
            LogActivity("Add Employee", txtemployeeno.Text.Trim(), "Added " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
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
                                                        New String() {"@n", "@id"}, New Object() {txtemployeeno.Text.Trim(), selectedEmployeeId}))
        If count > 0 Then
            MsgBox("Another employee is already using this Employee ID No.", vbExclamation, "Duplicate Entry")
            Exit Sub
        End If

        If MsgBox("Save the changes to " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim() & "?", vbYesNo + vbQuestion, "Confirm Update") <> MsgBoxResult.Yes Then Exit Sub

        If ExecNonQuery(
            "UPDATE tbl_employees SET employee_no=@n, last_name=@ln, first_name=@fn, department=@d, job_position=@p, status=@s WHERE employee_id=@id",
            New String() {"@n", "@ln", "@fn", "@d", "@p", "@s", "@id"},
            New Object() {txtemployeeno.Text.Trim(), txtlastname.Text.Trim(), txtfirstname.Text.Trim(),
                          cbodepartment.Text.Trim(), cboposition.Text.Trim(), cbostatus.Text, selectedEmployeeId}) Then
            LogActivity("Update Employee", txtemployeeno.Text.Trim(), "Updated " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
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
            LogActivity("Delete Employee", txtemployeeno.Text.Trim(), "Deleted " & txtfirstname.Text.Trim() & " " & txtlastname.Text.Trim())
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
        txtemployeeno.Text = NextEmployeeNo()
        txtlastname.Clear()
        txtfirstname.Clear()
        cbodepartment.SelectedIndex = -1
        cbodepartment.Text = ""
        cboposition.SelectedIndex = -1
        cboposition.Text = ""
        cbostatus.SelectedIndex = 0
        txtSearch.Clear()
        dgvemployee.ClearSelection()
    End Sub
End Class