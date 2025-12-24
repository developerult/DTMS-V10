Public Class clsBadRows : Inherits List(Of clsBadRow)

    Public Function addItem(ByVal Key As String, _
                        ByVal RowNumber As Integer, _
                        ByVal Record As String, _
                        ByVal ErrorType As RowValidationError, _
                        ByVal FileName As String) As clsBadRow
        Dim oBadRow As New clsBadRow
        With oBadRow
            .Key = Key
            .RowNumber = RowNumber
            .Record = Record
            .ErrorType = ErrorType
            .FileName = FileName
        End With
        Me.Add(oBadRow)
        Return oBadRow
    End Function

End Class

Public Class clsBadRow
    Public Key As String
    Public RowNumber As Integer
    Public Record As String
    Public ErrorType As RowValidationError
    Public FileName As String
End Class
