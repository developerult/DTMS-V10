Namespace Models
    'Added By RHR for v-8.4.0.002 on 04/27/2021
    Public Class CarrierBookApptWithTokenData

        Private _LaneAllowCarrierBookApptByEmail As Boolean
        Public Property LaneAllowCarrierBookApptByEmail() As Boolean
            Get
                Return _LaneAllowCarrierBookApptByEmail
            End Get
            Set(ByVal value As Boolean)
                _LaneAllowCarrierBookApptByEmail = value
            End Set
        End Property

        Private _LaneRequireCarrierAuthBookApptByEmail As Boolean
        Public Property LaneRequireCarrierAuthBookApptByEmail() As Boolean
            Get
                Return _LaneRequireCarrierAuthBookApptByEmail
            End Get
            Set(ByVal value As Boolean)
                _LaneRequireCarrierAuthBookApptByEmail = value
            End Set
        End Property


        Private _LaneUseCarrieContEmailForBookApptByEmail As Boolean
        Public Property LaneUseCarrieContEmailForBookApptByEmail() As Boolean
            Get
                Return _LaneUseCarrieContEmailForBookApptByEmail
            End Get
            Set(ByVal value As Boolean)
                _LaneUseCarrieContEmailForBookApptByEmail = value
            End Set
        End Property

        Private _LaneCarrierBookApptviaTokenEmail As String
        Public Property LaneCarrierBookApptviaTokenEmail() As String
            Get
                Return _LaneCarrierBookApptviaTokenEmail
            End Get
            Set(ByVal value As String)
                _LaneCarrierBookApptviaTokenEmail = value
            End Set
        End Property

        Private _LaneCarrierBookApptviaTokenFailEmail As String
        Public Property LaneCarrierBookApptviaTokenFailEmail() As String
            Get
                Return _LaneCarrierBookApptviaTokenFailEmail
            End Get
            Set(ByVal value As String)
                _LaneCarrierBookApptviaTokenFailEmail = value
            End Set
        End Property

        Private _LaneCarrierBookApptviaTokenFailPhone As String
        Public Property LaneCarrierBookApptviaTokenFailPhone() As String
            Get
                Return _LaneCarrierBookApptviaTokenFailPhone
            End Get
            Set(ByVal value As String)
                _LaneCarrierBookApptviaTokenFailPhone = value
            End Set
        End Property

        Private _IsPickup As Boolean
        Public Property IsPickup() As Boolean
            Get
                Return _IsPickup
            End Get
            Set(ByVal value As Boolean)
                _IsPickup = value
            End Set
        End Property

        Private _BookSHID As String
        Public Property BookSHID() As String
            Get
                Return _BookSHID
            End Get
            Set(ByVal value As String)
                _BookSHID = value
            End Set
        End Property

        Private _BookConsPrefix As String
        Public Property BookConsPrefix() As String
            Get
                Return _BookConsPrefix
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = value
            End Set
        End Property

        Private _BookControl As Integer
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _BookDateLoad As Date
        Public Property BookDateLoad() As Date
            Get
                Return _BookDateLoad
            End Get
            Set(ByVal value As Date)
                _BookDateLoad = value
            End Set
        End Property

        Private _BookDateRequired As Date
        Public Property BookDateRequired() As Date
            Get
                Return _BookDateRequired
            End Get
            Set(ByVal value As Date)
                _BookDateRequired = value
            End Set
        End Property

        ''' <summary>
        ''' Modified by RHR for v-8.4.0.004 on 11/2/2021
        ''' </summary>
        Private _BookCarrOrderNumber As String
        Public Property BookCarrOrderNumber() As String
            Get
                Return _BookCarrOrderNumber
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = value
            End Set
        End Property


        Private _CompControl As Integer
        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property

        Private _CompName As String
        Public Property CompName() As String
            Get
                Return _CompName
            End Get
            Set(ByVal value As String)
                _CompName = value
            End Set
        End Property

        Private _CarrierControl As Integer
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _LaneControl As Integer
        Public Property LaneControl() As Integer
            Get
                Return _LaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneControl = value
            End Set
        End Property

        Private _ExpirationDate As Date
        Public Property ExpirationDate() As Date
            Get
                Return _ExpirationDate
            End Get
            Set(ByVal value As Date)
                _ExpirationDate = value
            End Set
        End Property

        Private _ExpriationMinutes As Integer
        Public Property ExpirationMinutes() As Integer
            Get
                Return _ExpriationMinutes
            End Get
            Set(ByVal value As Integer)
                _ExpriationMinutes = value
            End Set
        End Property



    End Class


End Namespace

