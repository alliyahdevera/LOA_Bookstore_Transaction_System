Imports MySql.Data.MySqlClient

Public Class frmUserManagement

    Private selectedUserId As Integer = 0

    ' ------------------------------------------------------------------
    ' Form Load & Initialization
    ' ------------------------------------------------------------------
    Private Sub frmUserManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblname.Text = currentuser.FullName
        lblposition.Text = currentuser.Role

        ' Restrict ComboBoxes to selection only (no free typing)
        cboRole.DropDownStyle = ComboBoxStyle.DropDownList
        cboStatus.DropDownStyle = ComboBoxStyle.DropDownList

        ' Populate Role ComboBox
        cboRole.Items.Clear()
        cboRole.Items.AddRange(New Object() {ROLE_SUPERVISOR, ROLE_CASHIER, ROLE_INVENTORY_STAFF, ROLE_MANAGEMENT})

        ' Populate Status ComboBox
        cboStatus.Items.Clear()
        cboStatus.Items.AddRange(New Object() {"Active", "Inactive"})

        LoadGrid("")
        ClearFields()
    End Sub

    ' ------------------------------------------------------------------
    ' KeyPress Validation Handlers
    ' ------------------------------------------------------------------

    ' Username: Letters, digits, and underscores only
    Private Sub txtusername_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtusername.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> "_"c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' First Name: Letters, spaces, hyphens, and dots only
    Private Sub txtfirstname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtfirstname.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' Last Name: Letters, spaces, hyphens, and dots only
    Private Sub txtlastname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtlastname.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso e.KeyChar <> " "c AndAlso e.KeyChar <> "-"c AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' ------------------------------------------------------------------
    ' Form Input Validation Check
    ' ------------------------------------------------------------------
    Private Function ValidateUserInputs(isNewUser As Boolean) As Boolean
        If String.IsNullOrWhiteSpace(txtusername.Text) Then
            MsgBox("Username is required.", vbExclamation, "Validation Error")
            txtusername.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtfirstname.Text) Then
            MsgBox("First Name is required.", vbExclamation, "Validation Error")
            txtfirstname.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtlastname.Text) Then
            MsgBox("Last Name is required.", vbExclamation, "Validation Error")
            txtlastname.Focus()
            Return False
        End If

        ' Dropdown validations
        If cboRole.SelectedIndex = -1 OrElse Not cboRole.Items.Contains(cboRole.Text) Then
            MsgBox("Please select a valid Role from the list.", vbExclamation, "Validation Error")
            cboRole.Focus()
            Return False
        End If

        If cboStatus.SelectedIndex = -1 OrElse Not cboStatus.Items.Contains(cboStatus.Text) Then
            MsgBox("Please select a valid Status from the list.", vbExclamation, "Validation Error")
            cboStatus.Focus()
            Return False
        End If

        ' Password validation logic
        If isNewUser Then
            If String.IsNullOrWhiteSpace(txtpassword.Text) Then
                MsgBox("Password is required for new users.", vbExclamation, "Validation Error")
                txtpassword.Focus()
                Return False
            End If

            If txtpassword.Text <> txtconfirmpassword.Text Then
                MsgBox("Password and Confirm Password do not match.", vbExclamation, "Validation Error")
                txtconfirmpassword.Focus()
                Return False
            End If
        Else
            ' On update, if user entered a new password, check confirmation match
            If Not String.IsNullOrWhiteSpace(txtpassword.Text) AndAlso txtpassword.Text <> txtconfirmpassword.Text Then
                MsgBox("Password and Confirm Password do not match.", vbExclamation, "Validation Error")
                txtconfirmpassword.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    ' ------------------------------------------------------------------
    ' Grid & Data Operations
    ' ------------------------------------------------------------------
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
                            localDr("username").ToString(),
                            "********",
                            localDr("role_name").ToString(),
                            localDr("first_name").ToString(),
                            localDr("last_name").ToString(),
                            localDr("status").ToString()
                        )
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
        If row.Tag Is Nothing Then Exit Sub

        selectedUserId = Convert.ToInt32(row.Tag)

        txtusername.Text = row.Cells(0).Value.ToString()
        txtpassword.Clear()
        txtconfirmpassword.Clear()
        cboRole.Text = row.Cells(2).Value.ToString()
        txtfirstname.Text = row.Cells(3).Value.ToString()
        txtlastname.Text = row.Cells(4).Value.ToString()
        cboStatus.Text = row.Cells(5).Value.ToString()
    End Sub

    ' ------------------------------------------------------------------
    ' Button Actions
    ' ------------------------------------------------------------------

    ' ADD USER
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not ValidateUserInputs(True) Then Exit Sub

        Try
            Dim roleId As Integer = Convert.ToInt32(If(ExecScalar("SELECT role_id FROM TBL_ROLES WHERE role_name = @r", New String() {"@r"}, New Object() {cboRole.Text.Trim()}), 0))

            Dim ok As Boolean = ExecNonQuery(
                "INSERT INTO TBL_USERS (username, password, first_name, last_name, role_id, status) VALUES (@u, @p, @f, @l, @r, @s)",
                New String() {"@u", "@p", "@f", "@l", "@r", "@s"},
                New Object() {txtusername.Text.Trim(), HashPassword(txtpassword.Text), txtfirstname.Text.Trim(), txtlastname.Text.Trim(), roleId, cboStatus.Text.Trim()})

            If ok Then
                MsgBox("User added successfully.", vbInformation, "User Management")
                ClearFields()
                LoadGrid(txtSearch.Text.Trim())
            Else
                MsgBox("Could not add user. The Username may already exist.", vbExclamation, "User Management")
            End If
        Catch ex As Exception
            MsgBox("Error adding user: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    ' UPDATE USER
    Private Sub btnupd_Click(sender As Object, e As EventArgs) Handles btnupd.Click
        If selectedUserId = 0 Then
            MsgBox("Select a user from the list first.", vbExclamation, "User Management")
            Exit Sub
        End If

        If Not ValidateUserInputs(False) Then Exit Sub

        Try
            Dim roleId As Integer = Convert.ToInt32(If(ExecScalar("SELECT role_id FROM TBL_ROLES WHERE role_name = @r", New String() {"@r"}, New Object() {cboRole.Text.Trim()}), 0))

            Dim ok As Boolean
            If String.IsNullOrWhiteSpace(txtpassword.Text) Then
                ok = ExecNonQuery("UPDATE TBL_USERS SET username=@u, first_name=@f, last_name=@l, role_id=@r, status=@s WHERE user_id=@id",
                    New String() {"@u", "@f", "@l", "@r", "@s", "@id"},
                    New Object() {txtusername.Text.Trim(), txtfirstname.Text.Trim(), txtlastname.Text.Trim(), roleId, cboStatus.Text.Trim(), selectedUserId})
            Else
                ok = ExecNonQuery("UPDATE TBL_USERS SET username=@u, password=@p, first_name=@f, last_name=@l, role_id=@r, status=@s WHERE user_id=@id",
                    New String() {"@u", "@p", "@f", "@l", "@r", "@s", "@id"},
                    New Object() {txtusername.Text.Trim(), HashPassword(txtpassword.Text), txtfirstname.Text.Trim(), txtlastname.Text.Trim(), roleId, cboStatus.Text.Trim(), selectedUserId})
            End If

            If ok Then
                MsgBox("User updated successfully.", vbInformation, "User Management")
                ClearFields()
                LoadGrid(txtSearch.Text.Trim())
            End If
        Catch ex As Exception
            MsgBox("Error updating user: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    ' DEACTIVATE USER
    Private Sub btnremove_Click(sender As Object, e As EventArgs) Handles btnremove.Click
        If selectedUserId = 0 Then
            MsgBox("Select a user from the list first.", vbExclamation, "User Management")
            Exit Sub
        End If

        If selectedUserId = currentuser.UserID Then
            MsgBox("You cannot deactivate your own account.", vbExclamation, "User Management")
            Exit Sub
        End If

        If MsgBox("Deactivate this user? They will no longer be able to log in.", vbYesNo + vbQuestion, "User Management") <> MsgBoxResult.Yes Then Exit Sub

        If ExecNonQuery("UPDATE TBL_USERS SET status = 'Inactive' WHERE user_id = @id", New String() {"@id"}, New Object() {selectedUserId}) Then
            MsgBox("User deactivated.", vbInformation, "User Management")
            ClearFields()
            LoadGrid(txtSearch.Text.Trim())
        End If
    End Sub

    ' CLEAR BUTTON
    Private Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        ClearFields()
    End Sub

    Private Sub ClearFields()
        selectedUserId = 0
        txtusername.Clear()
        txtpassword.Clear()
        txtconfirmpassword.Clear()
        txtfirstname.Clear()
        txtlastname.Clear()
        cboRole.SelectedIndex = -1
        cboStatus.SelectedIndex = -1
        DataGridView1.ClearSelection()
    End Sub

End Class