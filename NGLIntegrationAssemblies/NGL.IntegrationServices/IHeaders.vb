Public Interface IHeaders

    Function buildBookObjectData(vHeaders As IHeaders, ByRef ErrMsg As String, ByRef headerList As List(Of NGLBookWebService.clsBookHeaderObject), ByRef detailList As List(Of NGLBookWebService.clsBookDetailObject)) As Boolean

    Property Items As List(Of Object)

    Property Errors As String

    Function ReadFromFile(ByVal fileName As String, Optional ByVal Details As List(Of Object) = Nothing) As List(Of Object)
 
End Interface
