Public Class WCFResult


    Private _success As Boolean = False
    Public Property Success() As Boolean
        Get
            Return _success
        End Get
        Set(ByVal value As Boolean)
            _success = value
        End Set
    End Property


    Private _parameter As Object = Nothing
    Public Property Parameter() As Object
        Get
            Return _parameter
        End Get
        Set(ByVal value As Object)
            _parameter = value
        End Set
    End Property

    Private _description As String = "NA"
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property


End Class

