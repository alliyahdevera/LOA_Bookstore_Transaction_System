Imports System.Globalization

Public Class SchoolYearInfo
    Public Property Id As Integer
    Public Property Label As String
    Public Property StartDate As Date
    Public Property EndDate As Date
    Public Property IsCurrent As Boolean

    Public Overrides Function ToString() As String
        Return Label
    End Function
End Class

Public Module SchoolYearHelper

    ' The school year every screen filters by. Nothing = "All School Years".
    Public SelectedSchoolYear As SchoolYearInfo = Nothing
    Private initialized As Boolean = False

    Public Function GetSchoolYears() As List(Of SchoolYearInfo)
        Dim list As New List(Of SchoolYearInfo)
        Dim dt As DataTable = GetDataTable(
            "SELECT school_year_id, label, start_date, end_date, is_current FROM tbl_school_years ORDER BY start_date DESC")
        If dt IsNot Nothing Then
            For Each r As DataRow In dt.Rows
                list.Add(New SchoolYearInfo With {
                    .Id = Convert.ToInt32(r("school_year_id")),
                    .Label = r("label").ToString(),
                    .StartDate = Convert.ToDateTime(r("start_date")),
                    .EndDate = Convert.ToDateTime(r("end_date")),
                    .IsCurrent = Convert.ToInt32(r("is_current")) = 1})
            Next
        End If
        Return list
    End Function

    ' First call after login: pick the school year marked as current
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

    ' " AND DATE(col) BETWEEN '2026-06-01' AND '2027-05-31' "   (empty when "All School Years")
    ' Dates are generated here from Date values, never from typed text.
    Public Function SchoolYearFilter(dateColumn As String) As String
        If SelectedSchoolYear Is Nothing Then Return ""
        Return " AND DATE(" & dateColumn & ") BETWEEN '" &
               SelectedSchoolYear.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' AND '" &
               SelectedSchoolYear.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' "
    End Function

    Public Function SchoolYearLabel() As String
        Return If(SelectedSchoolYear Is Nothing, "All School Years", SelectedSchoolYear.Label)
    End Function

End Module