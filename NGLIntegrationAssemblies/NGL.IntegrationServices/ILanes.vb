Public Interface ILanes

    Function buildLaneObjects(ByRef ErrMsg As String) As List(Of NglLaneWebService.clsLaneObject)

    Property Items As List(Of Object)

    Property Errors As String

    Function ReadFromFile(ByVal fileName As String) As List(Of Object)

End Interface
