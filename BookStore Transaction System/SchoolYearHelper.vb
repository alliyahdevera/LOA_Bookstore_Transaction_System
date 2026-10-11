Imports System.Globalization

Public Class SchoolYearInfo
    Public Property Id As Integer
    Public Property Label As String
    Public Property StartDate As Date
    Public Property EndDate As Date
    Public Property IsCurrent As Boolean
    Public Property Display As String = ""

    Public Overrides Function ToString() As String
        Return If(Display <> "", Display, Label)
    End Function
End Class

Public Module SchoolYearHelper

    ' The school year the whole system is viewing. Nothing = "All Time".
    Public SelectedSchoolYear As SchoolYearInfo = Nothing
    Private initialized As Boolean = False

    Public Function GetSchoolYears() As List(Of SchoolYearInfo)
        Dim list As New List(Of SchoolYearInfo)
        Dim dt As DataTable = GetDataTable(
            "SELECT school_year_id, label, start_date, end_date, is_current FROM tbl_school_years ORDER BY start_date DESC")
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                Dim sy As New SchoolYearInfo With {
                    .Id = Convert.ToInt32(r("school_year_id")),
                    .Label = r("label").ToString(),
                    .StartDate = Convert.ToDateTime(r("start_date")),
                    .EndDate = Convert.ToDateTime(r("end_date")),
                    .IsCurrent = Convert.ToInt32(r("is_current")) = 1}
                sy.Display = "SY " & sy.Label & " (" & sy.StartDate.ToString("MMM d, yyyy") & " - " &
                             sy.EndDate.ToString("MMM d, yyyy") & ")" & If(sy.IsCurrent, "  [Current]", "")
                list.Add(sy)
            Next
        End If
        Return list
    End Function

    Public Sub EnsureSchoolYearInitialized()
        If initialized Then Exit Sub
        initialized = True
        For Each sy As SchoolYearInfo In GetSchoolYears()
            If sy.IsCurrent Then
                SelectedSchoolYear = sy
                Exit For
            End If
        Next
    End Sub

    Public Sub SelectSchoolYear(sy As SchoolYearInfo)
        SelectedSchoolYear = sy
        initialized = True
    End Sub

    ' call after a successful login so every user starts on the current school year
    Public Sub ResetSchoolYear()
        SelectedSchoolYear = Nothing
        initialized = False
    End Sub

    Public Function SchoolYearLabel() As String
        EnsureSchoolYearInitialized()
        Return If(SelectedSchoolYear Is Nothing, "All Time", SelectedSchoolYear.Label)
    End Function

    ' " AND DATE(col) BETWEEN '2026-06-01' AND '2027-05-31' "   (empty for All Time)
    Public Function SchoolYearFilter(dateColumn As String) As String
        EnsureSchoolYearInitialized()
        If SelectedSchoolYear Is Nothing Then Return ""
        Return " AND DATE(" & dateColumn & ") BETWEEN '" &
               SelectedSchoolYear.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' AND '" &
               SelectedSchoolYear.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' "
    End Function

    ' Keeps a From/To pair of date pickers inside the selected school year.
    Public Sub ApplySchoolYearToPickers(dtFrom As DateTimePicker, dtTo As DateTimePicker)
        EnsureSchoolYearInitialized()
        For Each dtp As DateTimePicker In New DateTimePicker() {dtFrom, dtTo}
            dtp.MinDate = DateTimePicker.MinimumDateTime
            dtp.MaxDate = DateTimePicker.MaximumDateTime
        Next
        If SelectedSchoolYear Is Nothing Then Exit Sub

        Dim syStart As Date = SelectedSchoolYear.StartDate.Date
        Dim syEnd As Date = SelectedSchoolYear.EndDate.Date

        If Date.Today >= syStart AndAlso Date.Today <= syEnd Then
            dtFrom.Value = If(dtFrom.Value.Date < syStart, syStart, If(dtFrom.Value.Date > syEnd, syEnd, dtFrom.Value.Date))
            dtTo.Value = If(dtTo.Value.Date > syEnd, syEnd, If(dtTo.Value.Date < syStart, syStart, dtTo.Value.Date))
        Else
            dtFrom.Value = syStart
            dtTo.Value = syEnd
        End If
        dtFrom.MinDate = syStart : dtFrom.MaxDate = syEnd
        dtTo.MinDate = syStart : dtTo.MaxDate = syEnd
    End Sub

End Module