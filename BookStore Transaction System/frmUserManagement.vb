Imports MySql.Data.MySqlClient

Public Class frmUserManagement

    Private selectedUserId As Integer = 0
    Private ReadOnly validRoles As String() = {ROLE_SUPERVISOR, ROLE_CASHIER, ROLE_INVENTORY, ROLE_MANAGEMENT}

    Private Sub frmUserManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblname.Text = currentuser.FullName
        lblposition.Text = currentuser.Role
        LoadGrid("")
        ClearFields()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub LoadGrid(searchText As String)
        Try
            If Not connection() Then Exit Sub
            Dim query As String = "SELECT u.user_id, u.username, u.first_name, u.last_name, r.role_name, u.status " &
                                  "FROM TBL_USERS u INNER JOIN TBL_ROLES r ON u.role_id = r.role_id " &
                                  "WHERE u.username LIKE @s OR u.last_name LIKE @s ORDER BY u.last_name, u.first_name"
            Using localCmd As New MySqlCommand(query, cn)
                localCmd.Parameters.AddWithValue("@s", "%" & searchText & "%")
                Using localDr As MySqlDataReader = localCmd.ExecuteReader()
                    DataGridView1.Rows.Clear()
                    While localDr.Read()
                        Dim idx As Integer = DataGridView1.Rows.Add(
                            localDr("username").ToString(), "********", localDr("role_name").ToString(),
                            localDr("first_name").ToString(), localDr("last_name").ToString(), localDr("status").ToString())
                        DataGridView1.Rows(idx).Tag = Convert.ToInt32(localDr("user_id"))
                    End While
                End Using
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Error loading users: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        selectedUserId = Convert.ToInt32(row.Tag)
        txtusername.Text = row.Cells("Username").Value.ToString()
        TextBox4.Clear() : TextBox7.Clear()   ' never show a password back — leave blank = unchanged
        TextBox3.Text = row.Cells("FirstName").Value.ToString()
        TextBox6.Text = row.Cells("LastName").Value.ToString()
        TextBox1.Text = row.Cells("Column5").Value.ToString()   ' Role
        TextBox5.Text = row.Cells("Column6").Value.ToString()   ' Status
    End Sub

    Private Function ValidateRole() As Boolean
        For Each r As String In validRoles
            If String.Equals(r, TextBox1.Text.Trim(), StringComparison.OrdinalIgnoreCase) Then Return True
        Next
        MsgBox("Role must be one of: " & String.Join(", ", validRoles), vbExclamation, "User Management")
        Return False
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click   ' Add
        If String.IsNullOrWhiteSpace(txtusername.Text) OrElse String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse String.IsNullOrWhiteSpace(TextBox6.Text) Then
            MsgBox("Username, Password, First Name, and Last Name are required.", vbExclamation, "User Management") : Exit Sub
        End If
        If TextBox4.Text <> TextBox7.Text Then
            MsgBox("Password and Confirm Password do not match.", vbExclamation, "User Management") : Exit Sub
        End If
        If Not ValidateRole() Then Exit Sub

        Dim roleId As Integer = Convert.ToInt32(If(ExecScalar("SELECT role_id FROM TBL_ROLES WHERE role_name = @r", New String() {"@r"}, New Object() {TextBox1.Text.Trim()}), 0))
        Dim status As String = If(String.IsNullOrWhiteSpace(TextBox5.Text), "Active", TextBox5.Text.Trim())

        Dim ok As Boolean = ExecNonQuery(
            "INSERT INTO TBL_USERS (username, password, first_name, last_name, role_id, status) VALUES (@u, @p, @f, @l, @r, @s)",
            New String() {"@u", "@p", "@f", "@l", "@r", "@s"},
            New Object() {txtusername.Text.Trim(), HashPassword(TextBox4.Text), TextBox3.Text.Trim(), TextBox6.Text.Trim(), roleId, status})

        If ok Then
            MsgBox("User added.", vbInformation, "User Management")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        Else
            MsgBox("Could not add user. The Username may already be taken.", vbExclamation, "User Management")
        End If
    End Sub

    Private Sub btnupd_Click(sender As Object, e As EventArgs) Handles btnupd.Click
        If selectedUserId = 0 Then
            MsgBox("Select a user from the list first.", vbExclamation, "User Management") : Exit Sub
        End If
        If Not ValidateRole() Then Exit Sub
        If Not String.IsNullOrWhiteSpace(TextBox4.Text) AndAlso TextBox4.Text <> TextBox7.Text Then
            MsgBox("Password and Confirm Password do not match.", vbExclamation, "User Management") : Exit Sub
        End If

        Dim roleId As Integer = Convert.ToInt32(If(ExecScalar("SELECT role_id FROM TBL_ROLES WHERE role_name = @r", New String() {"@r"}, New Object() {TextBox1.Text.Trim()}), 0))
        Dim status As String = If(String.IsNullOrWhiteSpace(TextBox5.Text), "Active", TextBox5.Text.Trim())

        Dim ok As Boolean
        If String.IsNullOrWhiteSpace(TextBox4.Text) Then
            ok = ExecNonQuery("UPDATE TBL_USERS SET username=@u, first_name=@f, last_name=@l, role_id=@r, status=@s WHERE user_id=@id",
                New String() {"@u", "@f", "@l", "@r", "@s", "@id"},
                New Object() {txtusername.Text.Trim(), TextBox3.Text.Trim(), TextBox6.Text.Trim(), roleId, status, selectedUserId})
        Else
            ok = ExecNonQuery("UPDATE TBL_USERS SET username=@u, password=@p, first_name=@f, last_name=@l, role_id=@r, status=@s WHERE user_id=@id",
                New String() {"@u", "@p", "@f", "@l", "@r", "@s", "@id"},
                New Object() {txtusername.Text.Trim(), HashPassword(TextBox4.Text), TextBox3.Text.Trim(), TextBox6.Text.Trim(), roleId, status, selectedUserId})
        End If

        If ok Then
            MsgBox("User updated.", vbInformation, "User Management")
            LoadGrid(txtSearch.Text.Trim())
        End If
    End Sub

    Private Sub btnremove_Click(sender As Object, e As EventArgs) Handles btnremove.Click   ' Remove (soft delete)
        If selectedUserId = 0 Then
            MsgBox("Select a user from the list first.", vbExclamation, "User Management") : Exit Sub
        End If
        If selectedUserId = currentuser.UserID Then
            MsgBox("You cannot deactivate your own account.", vbExclamation, "User Management") : Exit Sub
        End If
        If MsgBox("Deactivate this user? They will no longer be able to log in.", vbYesNo + vbQuestion, "User Management") <> MsgBoxResult.Yes Then Exit Sub

        If ExecNonQuery("UPDATE TBL_USERS SET status = 'Inactive' WHERE user_id = @id", New String() {"@id"}, New Object() {selectedUserId}) Then
            MsgBox("User deactivated.", vbInformation, "User Management")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        End If
    End Sub

    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedUserId = 0
        txtusername.Clear() : TextBox4.Clear() : TextBox7.Clear()
        TextBox3.Clear() : TextBox6.Clear() : TextBox1.SelectedIndex = -1 : TextBox5.SelectedIndex = -1
        DataGridView1.ClearSelection()
    End Sub

End Class