Namespace Models
    'Created by LVV on 6/11/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues
    Public Class SecurityTypeConfig

        Private _Control As Integer
        Private _Name As String
        Private _Desc As String
        Private _Everyone As Boolean
        Private _CarrierDispatch As Boolean
        Private _CarrierAccounting As Boolean
        Private _Warehouse As Boolean
        Private _NEXTrack As Boolean
        Private _NEXTStop As Boolean
        Private _LEOperations As Boolean
        Private _LEAccounting As Boolean
        Private _LEAdmin As Boolean
        Private _Super As Boolean
        Private _Inactive As Boolean

        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                _Control = value
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

        Public Property Everyone() As Boolean
            Get
                Return _Everyone
            End Get
            Set(ByVal value As Boolean)
                _Everyone = value
            End Set
        End Property

        Public Property CarrierDispatch() As Boolean
            Get
                Return _CarrierDispatch
            End Get
            Set(ByVal value As Boolean)
                _CarrierDispatch = value
            End Set
        End Property

        Public Property CarrierAccounting() As Boolean
            Get
                Return _CarrierAccounting
            End Get
            Set(ByVal value As Boolean)
                _CarrierAccounting = value
            End Set
        End Property

        Public Property Warehouse() As Boolean
            Get
                Return _Warehouse
            End Get
            Set(ByVal value As Boolean)
                _Warehouse = value
            End Set
        End Property

        Public Property NEXTrack() As Boolean
            Get
                Return _NEXTrack
            End Get
            Set(ByVal value As Boolean)
                _NEXTrack = value
            End Set
        End Property

        Public Property NEXTStop() As Boolean
            Get
                Return _NEXTStop
            End Get
            Set(ByVal value As Boolean)
                _NEXTStop = value
            End Set
        End Property

        Public Property LEOperations() As Boolean
            Get
                Return _LEOperations
            End Get
            Set(ByVal value As Boolean)
                _LEOperations = value
            End Set
        End Property

        Public Property LEAccounting() As Boolean
            Get
                Return _LEAccounting
            End Get
            Set(ByVal value As Boolean)
                _LEAccounting = value
            End Set
        End Property

        Public Property LEAdmin() As Boolean
            Get
                Return _LEAdmin
            End Get
            Set(ByVal value As Boolean)
                _LEAdmin = value
            End Set
        End Property

        Public Property Super() As Boolean
            Get
                Return _Super
            End Get
            Set(ByVal value As Boolean)
                _Super = value
            End Set
        End Property

        Public Property Inactive() As Boolean
            Get
                Return _Inactive
            End Get
            Set(ByVal value As Boolean)
                _Inactive = value
            End Set
        End Property

    End Class

End Namespace