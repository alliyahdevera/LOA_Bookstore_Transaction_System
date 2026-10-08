Public Class frmEmployeeList

    ' ---- values returned to the caller (frmEmployeepos) ----
    Public Property InitialSearch As String = ""
    Public Property SelectedEmployeeId As Integer = 0
    Public Property SelectedEmployeeNo As String = ""
    Public Property SelectedEmployeeName As String = ""      ' "First Last"
    Public Property SelectedDepartment As String = ""
    Public Property SelectedPosition As String = ""

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
        LoadEmployees()
        txtSearch.Focus()
    End Sub

    Private Sub LoadEmployees()
        Dim dept As String = If(cboDepartment.SelectedIndex <= 0, "", cboDepartment.Text)
        Dim kw As String = txtSearch.Text.Trim()

        Dim dt As DataTable = GetDataTable(
            "SELECT employee_id, employee_no, last_name, first_name, department, job_position, status " &
            "FROM tbl_employees " &
            "WHERE (employee_no LIKE @s OR last_name LIKE @s OR first_name LIKE @s OR CONCAT(first_name, ' ', last_name) LIKE @s) " &
            "AND (@d = '' OR department = @d) " &
            "ORDER BY last_name, first_name",
            New String() {"@s", "@d"}, New Object() {"%" & kw & "%", dept})

        dgvstudents.Rows.Clear()
        For Each r As DataRow In dt.Rows
            Dim idx As Integer = dgvstudents.Rows.Add(
                r("employee_no").ToString(),
                r("last_name").ToString() & ", " & r("first_name").ToString(),
                r("department").ToString(),
                r("job_position").ToString(),
                r("status").ToString())
            dgvstudents.Rows(idx).Tag = r
        Next
        dgvstudents.ClearSelection()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Not isLoading Then LoadEmployees()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        LoadEmployees()
    End Sub

    Private Sub cboDepartment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDepartment.SelectedIndexChanged
        If Not isLoading Then LoadEmployees()
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

End Class
