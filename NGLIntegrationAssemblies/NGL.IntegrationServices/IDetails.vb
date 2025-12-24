Public Interface IDetails

    Property Errors As String

    Property Items As List(Of Object)

    Function ReadFromString(ByVal Data As String) As List(Of Object)

    Function ReadFromFile(ByVal fileName As String, Optional ByVal Details As List(Of Object) = Nothing) As List(Of Object)
     
    'Function getTempTypeText() As String

    'Function getTempType() As Integer

End Interface
