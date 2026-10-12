Public Class frmManageSY

    Private ReadOnly allTime As New SchoolYearInfo With {.Id = 0, .Label = "All Time", .Display = "All Time"}

    Private Sub frmManageSY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EnsureSchoolYearInitialized()

        ListBox1.Items.Clear()
        ListBox1.Items.Add(allTime)
        For Each sy As SchoolYearInfo In GetSchoolYears()
            ListBox1.Items.Add(sy)
        Next

        ListBox1.SelectedIndex = 0
        If SelectedSchoolYear IsNot Nothing Then
            For i As Integer = 1 To ListBox1.Items.Count - 1
                If DirectCast(ListBox1.Items(i), SchoolYearInfo).Id = SelectedSchoolYear.Id Then
                    ListBox1.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
        UpdateViewingLabel()
    End Sub

    Private Sub UpdateViewingLabel()
        Label13.Text = "Currently Viewing: " & If(SelectedSchoolYear Is Nothing, "All Time", "SY " & SelectedSchoolYear.Label)
    End Sub

    Private Sub btnupd_Click(sender As Object, e As EventArgs) Handles btnupd.Click
        Dim chosen As SchoolYearInfo = TryCast(ListBox1.SelectedItem, SchoolYearInfo)
        If chosen Is Nothing Then
            MsgBox("Click a school year first.", vbExclamation, "Manage School Year")
            Exit Sub
        End If

        SelectSchoolYear(If(chosen.Id = 0, Nothing, chosen))
        UpdateViewingLabel()

        MsgBox("Now viewing: " & If(chosen.Id = 0, "All Time", "SY " & chosen.Label) & vbCrLf &
               "The Dashboard, Transaction History and Reports will show only this school year" & vbCrLf &
               "the next time you open them.", vbInformation, "Manage School Year")
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ListBox1_DoubleClick(sender As Object, e As EventArgs)
        If ListBox1.SelectedIndex >= 0 Then btnupd.PerformClick()
    End Sub

End Class