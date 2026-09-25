Imports System.Data
Imports MySql.Data.MySqlClient

Public Module UIHelper

    ' Runs a SELECT and returns the results as a DataTable (used to fill ComboBoxes)
    Public Function GetDataTable(query As String, Optional paramNames As String() = Nothing, Optional paramValues As Object() = Nothing) As DataTable
        Dim dt As New DataTable()
        Try
            If Not connection() Then Return dt
            Using localCmd As New MySqlCommand(query, cn)
                If paramNames IsNot Nothing Then
                    For i As Integer = 0 To paramNames.Length - 1
                        localCmd.Parameters.AddWithValue(paramNames(i), paramValues(i))
                    Next
                End If
                Using da As New MySqlDataAdapter(localCmd)
                    da.Fill(dt)
                End Using
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Database error: " & ex.Message, vbCritical, "Error")
        End Try
        Return dt
    End Function

    ' Fills a ComboBox from a DataTable
    Public Sub FillCombo(cbo As ComboBox, dt As DataTable, displayMember As String, valueMember As String)
        cbo.DataSource = Nothing
        cbo.DisplayMember = displayMember
        cbo.ValueMember = valueMember
        cbo.DataSource = dt
    End Sub

    ' Runs a scalar query (COUNT, SUM, etc.)
    Public Function ExecScalar(query As String, Optional paramNames As String() = Nothing, Optional paramValues As Object() = Nothing) As Object
        Dim result As Object = Nothing
        Try
            If Not connection() Then Return Nothing
            Using localCmd As New MySqlCommand(query, cn)
                If paramNames IsNot Nothing Then
                    For i As Integer = 0 To paramNames.Length - 1
                        localCmd.Parameters.AddWithValue(paramNames(i), paramValues(i))
                    Next
                End If
                result = localCmd.ExecuteScalar()
            End Using
            cn.Close()
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Database error: " & ex.Message, vbCritical, "Error")
        End Try
        Return result
    End Function

    ' Runs an INSERT/UPDATE/DELETE. Returns True on success.
    Public Function ExecNonQuery(query As String, Optional paramNames As String() = Nothing, Optional paramValues As Object() = Nothing) As Boolean
        Try
            If Not connection() Then Return False
            Using localCmd As New MySqlCommand(query, cn)
                If paramNames IsNot Nothing Then
                    For i As Integer = 0 To paramNames.Length - 1
                        localCmd.Parameters.AddWithValue(paramNames(i), paramValues(i))
                    Next
                End If
                localCmd.ExecuteNonQuery()
            End Using
            cn.Close()
            Return True
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Database error: " & ex.Message, vbCritical, "Error")
            Return False
        End Try
    End Function

    ' Runs an INSERT and returns the new auto-increment id (0 on failure)
    Public Function ExecInsertGetId(query As String, Optional paramNames As String() = Nothing, Optional paramValues As Object() = Nothing) As Long
        Try
            If Not connection() Then Return 0
            Using localCmd As New MySqlCommand(query, cn)
                If paramNames IsNot Nothing Then
                    For i As Integer = 0 To paramNames.Length - 1
                        localCmd.Parameters.AddWithValue(paramNames(i), paramValues(i))
                    Next
                End If
                localCmd.ExecuteNonQuery()
                Dim newId As Long = localCmd.LastInsertedId
                cn.Close()
                Return newId
            End Using
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then cn.Close()
            MsgBox("Database error: " & ex.Message, vbCritical, "Error")
            Return 0
        End Try
    End Function

End Module