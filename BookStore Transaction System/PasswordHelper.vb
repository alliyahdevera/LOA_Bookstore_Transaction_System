Imports System.Security.Cryptography
Imports System.Text

Public Module PasswordHelper
    Public Function HashPassword(plainText As String) As String
        If plainText Is Nothing Then plainText = ""

        Using sha As SHA256 = SHA256.Create()
            Dim hashBytes As Byte() = sha.ComputeHash(Encoding.UTF8.GetBytes(plainText))
            Dim sb As New StringBuilder(hashBytes.Length * 2)
            For Each b As Byte In hashBytes
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

End Module