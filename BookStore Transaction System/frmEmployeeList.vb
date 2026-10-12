Public Class frmEmployeeList

    ' ---- values returned to the caller (frmEmployeepos) ----
    Public Property InitialSearch As String = ""
    Public Property SelectedEmployeeId As Integer = 0
    Public Property SelectedEmployeeNo As String = ""
    Public Property SelectedEmployeeName As String = ""      ' "First Last"
    Public Property SelectedDepartment As String = ""
    Public Property SelectedPosition As String = ""
    Private pg As GridPager
    Private isLoading As Boolean = True

    Private Sub frmEmployeeList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplySearchPlaceholders(Me)
        dgvstudents.AutoGenerateColumns = False
        dgvstudents.ReadOnly = True
        dgvstudents.AllowUserToAddRows = False
        dgvstudents.AllowUserToDeleteRows = False
        dgvstudents.MultiSelect = False
        dgvstudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        cboDepartment.DropDownStyle = ComboBoxStyle.DropDownList
        cboDepartment.Items.Clear()
        cboDepartment.Items.Add("All Departments")
        For Each r As DataRow In GetDataTable("SELECT DISTINCT department FROM tbl_employees WHERE department IS NOT NULL AND department <> '' ORDER BY department").Rows
            cboDepartment.Items.Add(r("department").ToString())
        Next
        cboDepartment.SelectedIndex = 0

        txtSearch.Text = InitialSearch
        isLoading = False
        pg = New GridPager(dgvstudents, 15)
        AddHandler pg.PageChanged, Sub() LoadEmployees()
        LoadEmployees()
        txtSearch.Focus()
    End Sub
    Private Sub LoadEmployees()
        If pg Is Nothing Then Exit Sub

        Dim departmentFilter As String =
        If(cboDepartment.SelectedIndex <= 0,
           "",
           cboDepartment.Text)

        Dim keyword As String = txtSearch.Text.Trim()

        Dim result As DataTable = pg.LoadPage(
        "SELECT employee_id, employee_no, last_name, " &
        "first_name, department, job_position, status " &
        "FROM tbl_employees " &
        "WHERE (employee_no LIKE @search " &
        "OR last_name LIKE @search " &
        "OR first_name LIKE @search " &
        "OR CONCAT(first_name, ' ', last_name) LIKE @search) " &
        "AND (@department = '' OR department = @department) " &
        "ORDER BY last_name, first_name, employee_id",
        New String() {"@search", "@department"},
        New Object() {
            "%" & keyword & "%",
            departmentFilter
        })

        dgvstudents.Rows.Clear()

        If result Is Nothing Then Exit Sub

        For Each employee As DataRow In result.Rows
            Dim index As Integer = dgvstudents.Rows.Add()
            Dim gridRow As DataGridViewRow = dgvstudents.Rows(index)

            gridRow.Cells("EmployeeNo").Value =
            employee("employee_no").ToString()

            gridRow.Cells("EmployeeName").Value =
            employee("last_name").ToString() & ", " &
            employee("first_name").ToString()

            gridRow.Cells("Department").Value =
            If(String.IsNullOrWhiteSpace(
                   employee("department").ToString()),
               "N/A",
               employee("department").ToString())

            gridRow.Cells("Position").Value =
            If(String.IsNullOrWhiteSpace(
                   employee("job_position").ToString()),
               "N/A",
               employee("job_position").ToString())

            ' Preserve status for SelectCurrent's inactive-user check.
            gridRow.Tag = employee

            If Not String.Equals(
            employee("status").ToString(),
            "Active",
            StringComparison.OrdinalIgnoreCase) Then

                gridRow.DefaultCellStyle.ForeColor = Color.Gray
            End If
        Next

        dgvstudents.ClearSelection()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        pg.Reset()
        LoadEmployees()
    End Sub

    Private Sub cboDepartment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDepartment.SelectedIndexChanged
        If isLoading Then Exit Sub
        pg.Reset()
        LoadEmployees()
    End Sub
    ' ---- choose an employee: double-click a row or press Enter ----
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
            MsgBox("Click an employee first.", vbExclamation, "Employee List")
            Exit Sub
        End If

        Dim r As DataRow = TryCast(dgvstudents.CurrentRow.Tag, DataRow)
        If r Is Nothing Then Exit Sub

        If r("status").ToString() <> "Active" Then
            MsgBox("This employee is inactive and cannot make purchases.", vbExclamation, "Employee List")
            Exit Sub
        End If

        SelectedEmployeeId = Convert.ToInt32(r("employee_id"))
        SelectedEmployeeNo = r("employee_no").ToString()
        SelectedEmployeeName = r("first_name").ToString() & " " & r("last_name").ToString()
        SelectedDepartment = If(IsDBNull(r("department")), "", r("department").ToString())
        SelectedPosition = If(IsDBNull(r("job_position")), "", r("job_position").ToString())

        If Me.Modal Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub btnselect_Click(sender As Object, e As EventArgs) Handles btnselect.Click
        SelectCurrent()          ' same as double-click / Enter: picks the highlighted student
    End Sub

End Class
