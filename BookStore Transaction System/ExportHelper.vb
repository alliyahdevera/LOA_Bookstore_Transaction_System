Public Module ExportHelper

    Public Sub ExportGridToCsv(dgv As DataGridView, suggestedName As String)
        Try
            Using sfd As New SaveFileDialog()
                sfd.Filter = "CSV File (*.csv)|*.csv"
                sfd.FileName = suggestedName & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".csv"
                If sfd.ShowDialog() = DialogResult.OK Then
                    Using sw As New IO.StreamWriter(sfd.FileName, False, Text.Encoding.UTF8)
                        Dim headers As New List(Of String)
                        For Each col As DataGridViewColumn In dgv.Columns
                            If col.Visible Then headers.Add(EscapeCsv(col.HeaderText))
                        Next
                        sw.WriteLine(String.Join(",", headers))

                        For Each row As DataGridViewRow In dgv.Rows
                            If row.IsNewRow Then Continue For
                            Dim vals As New List(Of String)
                            For Each col As DataGridViewColumn In dgv.Columns
                                If col.Visible Then vals.Add(EscapeCsv(If(row.Cells(col.Index).Value Is Nothing, "", row.Cells(col.Index).Value.ToString())))
                            Next
                            sw.WriteLine(String.Join(",", vals))
                        Next
                    End Using
                    MsgBox("Exported successfully to:" & vbCrLf & sfd.FileName, vbInformation, "Export to Excel")
                End If
            End Using
        Catch ex As Exception
            MsgBox("Export failed: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Function EscapeCsv(value As String) As String
        If value.Contains(",") OrElse value.Contains(""""c) OrElse value.Contains(vbLf) Then
            Return """" & value.Replace("""", """""") & """"
        End If
        Return value
    End Function

End Module