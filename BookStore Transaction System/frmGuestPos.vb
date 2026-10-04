Imports System.Text.RegularExpressions

Public Class frmGuestPos
    Implements IBuyerInfo

    Public ReadOnly Property BuyerType As String Implements IBuyerInfo.BuyerType
        Get
            Return "Guest"
        End Get
    End Property

    Public ReadOnly Property BuyerName As String Implements IBuyerInfo.BuyerName
        Get
            Return Regex.Replace(txtGuestName.Text.Trim(), "\s+", " ")
        End Get
    End Property

    Public ReadOnly Property StudentId As Integer Implements IBuyerInfo.StudentId
        Get
            Return 0
        End Get
    End Property

    Public Function ValidateBuyer(ByRef message As String) As Boolean Implements IBuyerInfo.ValidateBuyer
        Dim n As String = BuyerName
        If n = "" Then
            message = "Enter the guest name."
            Return False
        End If
        If Not Regex.IsMatch(n, "^\p{L}[\p{L} .,'\-]{1,99}$") Then
            message = "Guest name must be at least 2 characters and may only contain letters, spaces and . , ' -"
            Return False
        End If
        Return True
    End Function

    Public Sub ClearBuyer() Implements IBuyerInfo.ClearBuyer
        txtGuestName.Clear()
    End Sub

    Private Sub txtGuestName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtGuestName.KeyPress
        If Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

End Class