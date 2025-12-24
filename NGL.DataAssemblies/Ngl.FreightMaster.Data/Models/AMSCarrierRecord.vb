Namespace Models

    Public Class AMSCarrierRecord

        Private _SHID As String
        Private _Warehouse As String
        Private _EquipID As String
        Private _CarrierControl As Integer
        Private _CarrierName As String
        Private _CarrierNumber As Integer
        Private _BookDateLoad As Date?
        Private _BookDateRequired As Date?
        Private _ScheduledDate As Date?
        Private _ScheduledTime As Date?

        Private _Inbound As Boolean
        Private _IsTransfer As Boolean
        Private _IsPickup As Boolean

        Private _BookControl As Integer
        Private _CompControl As Integer

        ''Private _CompNumber As Integer
        ''Private _CNS As String
        ''Private _OrderNo As String
        ''Private _Pro As String
        ''Private _Address1 As String
        ''Private _Address2 As String
        ''Private _City As String
        ''Private _State As String
        ''Private _Zip As String
        ''Private _Country As String
        ''Private _BookAMSPickupApptControl As Integer
        ''Private _BookAMSDeliveryApptControl As Integer

        Public Property SHID() As String
            Get
                Return _SHID
            End Get
            Set(ByVal value As String)
                _SHID = value
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

        Public Property EquipID() As String
            Get
                Return _EquipID
            End Get
            Set(ByVal value As String)
                _EquipID = value
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

        Public Property BookDateLoad() As Date?
            Get
                Return _BookDateLoad
            End Get
            Set(ByVal value As Date?)
                _BookDateLoad = value
            End Set
        End Property

        Public Property BookDateRequired() As Date?
            Get
                Return _BookDateRequired
            End Get
            Set(ByVal value As Date?)
                _BookDateRequired = value
            End Set
        End Property

        Public Property ScheduledDate() As Date?
            Get
                Return _ScheduledDate
            End Get
            Set(ByVal value As Date?)
                _ScheduledDate = value
            End Set
        End Property

        Public Property ScheduledTime() As Date?
            Get
                Return _ScheduledTime
            End Get
            Set(ByVal value As Date?)
                _ScheduledTime = value
            End Set
        End Property



        Public Property Inbound() As Boolean
            Get
                Return _Inbound
            End Get
            Set(ByVal value As Boolean)
                _Inbound = value
            End Set
        End Property

        Public Property IsTransfer() As Boolean
            Get
                Return _IsTransfer
            End Get
            Set(ByVal value As Boolean)
                _IsTransfer = value
            End Set
        End Property

        Public Property IsPickup() As Boolean
            Get
                Return _IsPickup
            End Get
            Set(ByVal value As Boolean)
                _IsPickup = value
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


        ''Public Property CompNumber() As Integer
        ''    Get
        ''        Return _CompNumber
        ''    End Get
        ''    Set(ByVal value As Integer)
        ''        _CompNumber = value
        ''    End Set
        ''End Property

        ''Public Property CNS() As String
        ''    Get
        ''        Return _CNS
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _CNS = value
        ''    End Set
        ''End Property

        ''Public Property OrderNo() As String
        ''    Get
        ''        Return _OrderNo
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _OrderNo = value
        ''    End Set
        ''End Property

        ''Public Property Pro() As String
        ''    Get
        ''        Return _Pro
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _Pro = value
        ''    End Set
        ''End Property

        ''Public Property Address1() As String
        ''    Get
        ''        Return _Address1
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _Address1 = value
        ''    End Set
        ''End Property

        ''Public Property Address2() As String
        ''    Get
        ''        Return _Address2
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _Address2 = value
        ''    End Set
        ''End Property

        ''Public Property City() As String
        ''    Get
        ''        Return _City
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _City = value
        ''    End Set
        ''End Property

        ''Public Property State() As String
        ''    Get
        ''        Return _State
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _State = value
        ''    End Set
        ''End Property

        ''Public Property Zip() As String
        ''    Get
        ''        Return _Zip
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _Zip = value
        ''    End Set
        ''End Property

        ''Public Property Country() As String
        ''    Get
        ''        Return _Country
        ''    End Get
        ''    Set(ByVal value As String)
        ''        _Country = value
        ''    End Set
        ''End Property

        ''Public Property BookAMSPickupApptControl() As Integer
        ''    Get
        ''        Return _BookAMSPickupApptControl
        ''    End Get
        ''    Set(ByVal value As Integer)
        ''        _BookAMSPickupApptControl = value
        ''    End Set
        ''End Property

        ''Public Property BookAMSDeliveryApptControl() As Integer
        ''    Get
        ''        Return _BookAMSDeliveryApptControl
        ''    End Get
        ''    Set(ByVal value As Integer)
        ''        _BookAMSDeliveryApptControl = value
        ''    End Set
        ''End Property


    End Class

End Namespace

