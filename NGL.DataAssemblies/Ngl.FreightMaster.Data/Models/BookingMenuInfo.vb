Namespace Models

    Public Class BookingMenuInfo

        Private _BookControl As Integer
        Private _BookProNumber As String
        Private _CarrierControl As Integer
        Private _CarrierNumber As Integer
        Private _CarrierName As String

        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Public Property BookProNumber() As String
            Get
                Return _BookProNumber
            End Get
            Set(ByVal value As String)
                _BookProNumber = value
            End Set
        End Property

        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property

    End Class


End Namespace

