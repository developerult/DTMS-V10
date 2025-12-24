Namespace Models
    Public Class NGLAPIAccessorial

        Private _Control As Integer
        Private _Code As String
        Private _Name As String
        Private _Desc As String
        Private _Value As Decimal

        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set
                _Control = Value
            End Set
        End Property

        Public Property Code() As String
            Get
                Return _Code
            End Get
            Set(ByVal value As String)
                _Code = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Public Property Desc() As String
            Get
                Return _Desc
            End Get
            Set(ByVal value As String)
                _Desc = value
            End Set
        End Property

        Public Property Value() As Decimal
            Get
                Return _Value
            End Get
            Set(ByVal value As Decimal)
                _Value = value
            End Set
        End Property


    End Class


End Namespace


