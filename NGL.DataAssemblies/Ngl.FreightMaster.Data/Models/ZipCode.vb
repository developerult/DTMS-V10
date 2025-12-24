Namespace Models
    'Added By LVV On 5/11/17 For v-8.0 Rate Shop Free Trial
    Public Class ZipCode

        Private _ZipCodeControl As Integer
        Private _ZipCode As String
        Private _State As String
        Private _City As String
        Private _Country As String

        Public Property ZipCodeControl() As Integer
            Get
                Return _ZipCodeControl
            End Get
            Set
                _ZipCodeControl = Value
            End Set
        End Property

        Public Property ZipCode() As String
            Get
                Return _ZipCode
            End Get
            Set(ByVal value As String)
                _ZipCode = value
            End Set
        End Property

        Public Property State() As String
            Get
                Return _State
            End Get
            Set(ByVal value As String)
                _State = value
            End Set
        End Property

        Public Property City() As String
            Get
                Return _City
            End Get
            Set(ByVal value As String)
                _City = value
            End Set
        End Property

        Public Property Country() As String
            Get
                Return _Country
            End Get
            Set(ByVal value As String)
                _Country = value
            End Set
        End Property

    End Class


End Namespace

