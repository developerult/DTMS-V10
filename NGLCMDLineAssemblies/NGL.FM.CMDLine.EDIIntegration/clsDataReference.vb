Public Class clsDataReference
    Private _Control As Integer
    Public Property Control As Integer
        Get
            Return _Control
        End Get
        Set(ByVal value As Integer)
            _Control = value
        End Set
    End Property

    Private _Number As Integer
    Public Property Number As Integer
        Get
            Return _Number
        End Get
        Set(ByVal value As Integer)
            _Number = value
        End Set
    End Property

    Private _Name As String = "UnNamed"
    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

End Class
