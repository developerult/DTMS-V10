Namespace Models
    'Added By RHR for v-8.4.0.002 on 04/27/2021

    Public Class CarrierAcceptLoadWithTokenData

        Private _LECarAllowCarrierAcceptRejectByEmail As Boolean
        Public Property LECarAllowCarrierAcceptRejectByEmail() As Boolean
            Get
                Return _LECarAllowCarrierAcceptRejectByEmail
            End Get
            Set(ByVal value As Boolean)
                _LECarAllowCarrierAcceptRejectByEmail = value
            End Set
        End Property

        Private _LECarCarrierAuthCarrierAcceptRejectByEmail As Boolean
        Public Property LECarCarrierAuthCarrierAcceptRejectByEmail() As Boolean
            Get
                Return _LECarCarrierAuthCarrierAcceptRejectByEmail
            End Get
            Set(ByVal value As Boolean)
                _LECarCarrierAuthCarrierAcceptRejectByEmail = value
            End Set
        End Property

        Private _LECarCarrierAuthCarrierAcceptRejectExpMin As Integer
        Public Property LECarCarrierAuthCarrierAcceptRejectExpMin() As Integer
            Get
                Return _LECarCarrierAuthCarrierAcceptRejectExpMin
            End Get
            Set(ByVal value As Integer)
                _LECarCarrierAuthCarrierAcceptRejectExpMin = value
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


        Private _CarrierName As String
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
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

        Private _CarrierContControl As Integer
        Public Property CarrierContControl() As Integer
            Get
                Return _CarrierContControl
            End Get
            Set(ByVal value As Integer)
                _CarrierContControl = value
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

        '        String TokenSupportEmail { get; set; }
        'Public String TokenSupportPhone { Get; Set; }
        Private _TokenSupportEmail As String
        Public Property TokenSupportEmail() As String
            Get
                Return _TokenSupportEmail
            End Get
            Set(ByVal value As String)
                _TokenSupportEmail = value
            End Set
        End Property

        Private _TokenSupportPhone As String
        Public Property TokenSupportPhone() As String
            Get
                Return _TokenSupportPhone
            End Get
            Set(ByVal value As String)
                _TokenSupportPhone = value
            End Set
        End Property

        Private _OriginNameAddressCSZ As String
        Public Property OriginNameAddressCSZ() As String
            Get
                Return _OriginNameAddressCSZ
            End Get
            Set(ByVal value As String)
                _OriginNameAddressCSZ = value
            End Set
        End Property

        Private _DestNameAddressCSZ As String
        Public Property DestNameAddressCSZ() As String
            Get
                Return _DestNameAddressCSZ
            End Get
            Set(ByVal value As String)
                _DestNameAddressCSZ = value
            End Set
        End Property



    End Class


End Namespace
