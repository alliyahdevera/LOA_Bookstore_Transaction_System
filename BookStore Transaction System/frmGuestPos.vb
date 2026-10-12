Imports System.Text.RegularExpressions

Public Class frmGuestPos
    Implements IBuyerInfo

    Public ReadOnly Property BuyerType As String _
        Implements IBuyerInfo.BuyerType

        Get
            Return "Guest"
        End Get
    End Property

    Public ReadOnly Property BuyerName As String _
        Implements IBuyerInfo.BuyerName

        Get
            Return Regex.Replace(
                txtGuestName.Text.Trim(),
                "\s+",
                " ")
        End Get
    End Property

    Public ReadOnly Property StudentId As Integer _
        Implements IBuyerInfo.StudentId

        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property IdNumber As String _
        Implements IBuyerInfo.IdNumber

        Get
            Return TextBox1.Text.Trim()
        End Get
    End Property

    Private Sub frmGuestPos_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        TextBox1.MaxLength = 30
        txtGuestName.MaxLength = 100
    End Sub

    Public Function ValidateBuyer(
        ByRef message As String
    ) As Boolean Implements IBuyerInfo.ValidateBuyer

        message = ""

        Dim guestName As String = BuyerName

        If guestName = "" Then
            message = "Enter the guest name."
            Return False
        End If

        If Not Regex.IsMatch(
            guestName,
            "^\p{L}[\p{L} .,'\-]{1,99}$") Then

            message =
                "Guest name must contain 2 to 100 characters " &
                "using letters, spaces, periods, commas, " &
                "apostrophes, or hyphens."

            Return False
        End If

        ' ID is optional. Validate it only when supplied.
        If IdNumber <> "" AndAlso
            Not Regex.IsMatch(
                IdNumber,
                "^[\p{L}\p{N}\-]{1,30}$") Then

            message =
                "Guest ID No. may contain up to 30 letters, " &
                "numbers, or hyphens, or it may be left blank."

            Return False
        End If

        Return True
    End Function

    Public Sub ClearBuyer() Implements IBuyerInfo.ClearBuyer
        txtGuestName.Clear()
        TextBox1.Clear()
    End Sub

    Private Sub txtGuestName_KeyPress(
        sender As Object,
        e As KeyPressEventArgs
    ) Handles txtGuestName.KeyPress

        If Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub GuestId_KeyPress(
        sender As Object,
        e As KeyPressEventArgs
    ) Handles TextBox1.KeyPress

        If Not Char.IsControl(e.KeyChar) AndAlso
            Not Char.IsLetterOrDigit(e.KeyChar) AndAlso
            e.KeyChar <> "-"c Then

            e.Handled = True
        End If
    End Sub

End Class