Imports MySql.Data.MySqlClient
Module DBConnect
    Public cn As New MySqlConnection
    Public cmd As MySqlCommand
    Public dr As MySqlDataReader 'retrieve
    Public sql As String 'sql command
    Public Function connection() As Boolean
        Try
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
            cn.ConnectionString = "server=localhost;userid=root;password=;database=bookstore_db;"
            cn.Open()
            Return True
        Catch ex As MySqlException
            MsgBox("Database connection failed: " & ex.Message, vbCritical, "Connection Error")
            Return False
        Catch ex As Exception
            MsgBox("An unexpected error occurred: " & ex.Message, vbCritical, "Error")
            Return False
        End Try
    End Function
End Module
