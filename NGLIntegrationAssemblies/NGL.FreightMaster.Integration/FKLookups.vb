Public Class FKLookups : Inherits List(Of FKLookup)

    Public Sub AddNew(ByVal strKeyFieldName As String, _
                      ByVal strKeyValue As String, _
                      ByVal strKeyTableName As String, _
                      ByVal strKeyParentField As String, _
                      ByVal strKeyFKValue As String)
        Dim oK As New FKLookup
        With oK
            .KeyFieldName = strKeyFieldName
            .KeyValue = strKeyValue
            .KeyTableName = strKeyTableName
            .KeyParentField = strKeyParentField
            .KeyFKValue = strKeyFKValue
        End With
        Me.Add(oK)
    End Sub

    Public Function getFKValue(ByVal strKeyFieldName As String, _
                               ByVal strKeyFKValue As String, _
                               ByVal strKeyTableName As String, _
                               ByVal strParentfield As String) As String
        Dim strRet As String = ""
        Dim query = From fk In Me Where fk.KeyFieldName = strKeyFieldName _
                                    And fk.KeyFKValue = strKeyFKValue _
                                    And fk.KeyTableName = strKeyTableName _
                                    And fk.KeyParentField = strParentfield
        If query.Count > 0 Then
            Dim fkl As FKLookup = query(0)
            strRet = fkl.KeyValue
        End If
        Return strRet
    End Function

End Class

Public Class FKLookup
    Public KeyFieldName As String = ""
    Public KeyValue As String = ""
    Public KeyTableName As String = ""
    Public KeyParentField As String = ""
    Public KeyFKValue As String = ""

End Class
