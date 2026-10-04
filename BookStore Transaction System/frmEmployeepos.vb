Imports System.Text.RegularExpressions

Public Class frmEmployeepos
    Implements IBuyerInfo

    Private foundId As Integer = 0
    Private foundNo As String = ""

    Public ReadOnly Property BuyerType As String Implements IBuyerInfo.BuyerType
        Get
            Return "Employee"
        End Get
    End Property

    Public ReadOnly Property BuyerName As String Implements IBuyerInfo.BuyerName
        Get
            Return txtStudentName.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property StudentId As Integer Implements IBuyerInfo.StudentId
        Get
            Return 0
        End Get
    End Property

    Public Function ValidateBuyer(ByRef message As String) As Boolean Implements IBuyerInfo.ValidateBuyer
        Dim no As String = txtStudentNo.Text.Trim()
        If no = "" Then
            message = "Enter the employee number and click Search."
            Return False
        End If
        If Not Regex.IsMatch(no, "^[A-Za-z0-9-]{3,20}$") Then
            message = "Employee number may only contain letters, numbers and dashes."
            Return False
        End If
        If foundId = 0 OrElse Not String.Equals(no, foundNo, StringComparison.OrdinalIgnoreCase) Then
            message = "Click Search to verify the employee number first."
            Return False
        End If
        Return True
    End Function

    Public Sub ClearBuyer() Implements IBuyerInfo.ClearBuyer
        txtStudentNo.Clear()
        ResetFound()
    End Sub

    Private Sub ResetFound()
        foundId = 0
        foundNo = ""
        txtStudentName.Clear()
        txtgrade.Clear()
        txtProgramStrand.Clear()
        txtStudentName.ForeColor = Color.Black
    End Sub

    Private Sub ShowInline(text As String)
        ResetFound()
        txtStudentName.Text = text
        txtStudentName.ForeColor = Color.Firebrick
    End Sub

    Private Sub txtStudentNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtStudentNo.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> "-"c AndAlso Not Char.IsControl(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub txtStudentNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtStudentNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SearchEmployee()
        End If
    End Sub

    Private Sub txtStudentNo_TextChanged(sender As Object, e As EventArgs) Handles txtStudentNo.TextChanged
        If foundId <> 0 Then
            If Not String.Equals(txtStudentNo.Text.Trim(), foundNo, StringComparison.OrdinalIgnoreCase) Then ResetFound()
        ElseIf txtStudentName.ForeColor = Color.Firebrick Then
            ResetFound()
        End If
    End Sub

    Private Sub btnSearchStudent_Click(sender As Object, e As EventArgs) Handles btnSearchStudent.Click
        SearchEmployee()
    End Sub

    Private Sub SearchEmployee()
        Dim no As String = txtStudentNo.Text.Trim()
        If Not Regex.IsMatch(no, "^[A-Za-z0-9-]{3,20}$") Then
            ShowInline("Invalid employee number")
            Exit Sub
        End If

        Dim dt As DataTable = GetDataTable(
            "SELECT employee_id, employee_no, last_name, first_name, department, job_position, status FROM tbl_employees WHERE employee_no = @n",
            New String() {"@n"}, New Object() {no})

        If dt.Rows.Count = 0 Then
            ShowInline("Employee not found")
            Exit Sub
        End If

        Dim r As DataRow = dt.Rows(0)
        If r("status").ToString() <> "Active" Then
            ShowInline("Employee is inactive")
            Exit Sub
        End If

        foundId = Convert.ToInt32(r("employee_id"))
        foundNo = r("employee_no").ToString()
        txtStudentName.ForeColor = Color.Black
        txtStudentName.Text = r("first_name").ToString() & " " & r("last_name").ToString()
        txtgrade.Text = If(IsDBNull(r("department")), "", r("department").ToString())
        txtProgramStrand.Text = If(IsDBNull(r("job_position")), "", r("job_position").ToString())
    End Sub

End Class