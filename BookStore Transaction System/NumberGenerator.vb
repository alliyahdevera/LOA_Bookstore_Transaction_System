Public Module NumberGenerator
    Public Function NewTransactionNo() As String
        Return "TXN-" & DateTime.Now.ToString("yyyyMMddHHmmssfff")
    End Function
    Public Function NewORNo() As String
        Return "OR-" & DateTime.Now.ToString("yyyyMMddHHmmssfff")
    End Function
    Public Function NewReferenceNo() As String
        Return "DR-" & DateTime.Now.ToString("yyyyMMddHHmmssfff")
    End Function
End Module