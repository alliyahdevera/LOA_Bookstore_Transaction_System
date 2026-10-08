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
        txtGuestId.Clear()
    End Sub

    Private Sub txtGuestName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtGuestName.KeyPress
        If Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub
    Private txtGuestId As TextBox

    Public ReadOnly Property IdNumber As String Implements IBuyerInfo.IdNumber
        Get
            Return txtGuestId.Text.Trim()
        End Get
    End Property

    Private Sub frmGuestPos_LoadId(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim lbl As New Label With {.AutoSize = True, .Font = Label13.Font, .Text = "ID Number",
                                   .Location = New Point(Label13.Left, 52)}
        txtGuestId = New TextBox With {.BorderStyle = BorderStyle.FixedSingle, .Font = txtGuestName.Font,
                                       .Location = New Point(txtGuestName.Left, 50), .Size = txtGuestName.Size,
                                       .MaxLength = 30}
        AddHandler txtGuestId.KeyPress, Sub(s, ev)
                                            If Not Char.IsLetterOrDigit(ev.KeyChar) AndAlso ev.KeyChar <> "-"c AndAlso Not Char.IsControl(ev.KeyChar) Then ev.Handled = True
                                        End Sub
        Controls.Add(lbl)
        Controls.Add(txtGuestId)
        Panel1.Top = 90
        Panel1.Height = 70
    End Sub

End Class