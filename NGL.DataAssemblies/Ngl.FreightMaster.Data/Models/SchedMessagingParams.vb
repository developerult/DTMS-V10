Namespace Models

    Public Class SchedMessagingParams

        Private _ApptControl As Integer
        Private _BookControl As Integer
        Private _CompControl As Integer
        Private _CarrierControl As Integer
        Private _Warehouse As String
        Private _CompNumber As Integer
        Private _CarrierName As String
        Private _CarrierNumber As Integer
        Private _SHID As String
        Private _EquipID As String
        Private _Orders As String
        Private _BookDateLoad As Date
        Private _BookDateRequired As Date
        Private _ogDockID As String
        Private _ogDockName As String
        Private _ogApptStart As Date
        Private _ogApptEnd As Date
        Private _ogDockEmail As String
        Private _modDockID As String
        Private _modDockName As String
        Private _modApptStart As Date
        Private _modApptEnd As Date
        Private _modDockEmail As String
        Private _spDets As LTS.spAMSGetDetailsForMessagingResult
        Private _dt As Date


        Public Property ApptControl() As Integer
            Get
                Return _ApptControl
            End Get
            Set(ByVal value As Integer)
                _ApptControl = value
            End Set
        End Property

        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
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

        Public Property Warehouse() As String
            Get
                Return _Warehouse
            End Get
            Set(ByVal value As String)
                _Warehouse = value
            End Set
        End Property

        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
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

        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Public Property SHID() As String
            Get
                Return _SHID
            End Get
            Set(ByVal value As String)
                _SHID = value
            End Set
        End Property

        Public Property EquipID() As String
            Get
                Return _EquipID
            End Get
            Set(ByVal value As String)
                _EquipID = value
            End Set
        End Property

        Public Property Orders() As String
            Get
                Return _Orders
            End Get
            Set(ByVal value As String)
                _Orders = value
            End Set
        End Property

        Public Property BookDateLoad() As Date
            Get
                Return _BookDateLoad
            End Get
            Set(ByVal value As Date)
                _BookDateLoad = value
            End Set
        End Property

        Public Property BookDateRequired() As Date
            Get
                Return _BookDateRequired
            End Get
            Set(ByVal value As Date)
                _BookDateRequired = value
            End Set
        End Property

        Public Property ogDockID() As String
            Get
                Return _ogDockID
            End Get
            Set(ByVal value As String)
                _ogDockID = value
            End Set
        End Property

        Public Property ogDockName() As String
            Get
                Return _ogDockName
            End Get
            Set(ByVal value As String)
                _ogDockName = value
            End Set
        End Property

        Public Property ogApptStart() As Date
            Get
                Return _ogApptStart
            End Get
            Set(ByVal value As Date)
                _ogApptStart = value
            End Set
        End Property

        Public Property ogApptEnd() As Date
            Get
                Return _ogApptEnd
            End Get
            Set(ByVal value As Date)
                _ogApptEnd = value
            End Set
        End Property

        Public Property ogDockEmail() As String
            Get
                Return _ogDockEmail
            End Get
            Set(ByVal value As String)
                _ogDockEmail = value
            End Set
        End Property

        Public Property modDockID() As String
            Get
                Return _modDockID
            End Get
            Set(ByVal value As String)
                _modDockID = value
            End Set
        End Property

        Public Property modDockName() As String
            Get
                Return _modDockName
            End Get
            Set(ByVal value As String)
                _modDockName = value
            End Set
        End Property

        Public Property modApptStart() As Date
            Get
                Return _modApptStart
            End Get
            Set(ByVal value As Date)
                _modApptStart = value
            End Set
        End Property

        Public Property modApptEnd() As Date
            Get
                Return _modApptEnd
            End Get
            Set(ByVal value As Date)
                _modApptEnd = value
            End Set
        End Property

        Public Property modDockEmail() As String
            Get
                Return _modDockEmail
            End Get
            Set(ByVal value As String)
                _modDockEmail = value
            End Set
        End Property

        Public Property spDets() As LTS.spAMSGetDetailsForMessagingResult
            Get
                Return _spDets
            End Get
            Set(ByVal value As LTS.spAMSGetDetailsForMessagingResult)
                _spDets = value
            End Set
        End Property

        Public Property dt() As Date
            Get
                Return _dt
            End Get
            Set(ByVal value As Date)
                _dt = value
            End Set
        End Property


    End Class


End Namespace

