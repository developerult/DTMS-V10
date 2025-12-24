'Added By LVV on 9/27/19 Bing Maps
Namespace Models

    Public Class MapRouteWayPoints

        Private _MapItWayPoints As New List(Of MapWaypoint)
        Private _TrackItWayPoints As New List(Of MapWaypoint)
        Private _ErrMsg As String

        Public Property MapItWayPoints() As List(Of MapWaypoint)
            Get
                Return _MapItWayPoints
            End Get
            Set(ByVal value As List(Of MapWaypoint))
                _MapItWayPoints = value
            End Set
        End Property

        Public Property TrackItWayPoints() As List(Of MapWaypoint)
            Get
                Return _TrackItWayPoints
            End Get
            Set(ByVal value As List(Of MapWaypoint))
                _TrackItWayPoints = value
            End Set
        End Property

        Public Property ErrMsg() As String
            Get
                Return _ErrMsg
            End Get
            Set(ByVal value As String)
                _ErrMsg = value
            End Set
        End Property

    End Class

    Public Class MapWaypoint

        Public Enum StopType
            Pickup
            Delivery
            PickAndDel
            Track
        End Enum

        Private _StopNumber As Integer
        Private _Name As String
        Private _Address1 As String
        Private _Address2 As String
        Private _Address3 As String
        Private _City As String
        Private _State As String
        Private _Zip As String
        Private _Country As String
        Private _AddressString As String
        Private _Lattitude As Double
        Private _Longitude As Double
        Private _StopCategory As StopType '0 = Pickup, 1 = Delivery, 2 = Pickup And Delivery (so I can Do the colors)
        Private _PickupOrders As String 'comma separated string listing OrderNumbers being picked up from address
        Private _DropoffOrders As String 'comma separated string listing OrderNumbers being delivered to address
        Private _DateDelivered As Date?
        Private _Comment As String
        Private _StopCompleted As Boolean
        Private _ArrivedDate As Date?
        Private _DepartDate As Date?

        Public Property StopNumber() As Integer
            Get
                Return _StopNumber
            End Get
            Set(ByVal value As Integer)
                _StopNumber = value
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

        Public Property Address1() As String
            Get
                Return _Address1
            End Get
            Set(ByVal value As String)
                _Address1 = value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return _Address2
            End Get
            Set(ByVal value As String)
                _Address2 = value
            End Set
        End Property

        Public Property Address3() As String
            Get
                Return _Address3
            End Get
            Set(ByVal value As String)
                _Address3 = value
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

        Public Property State() As String
            Get
                Return _State
            End Get
            Set(ByVal value As String)
                _State = value
            End Set
        End Property

        Public Property Zip() As String
            Get
                Return _Zip
            End Get
            Set(ByVal value As String)
                _Zip = value
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

        Public Property AddressString() As String
            Get
                Return _AddressString
            End Get
            Set(ByVal value As String)
                _AddressString = value
            End Set
        End Property

        Public Property Lattitude() As Double
            Get
                Return _Lattitude
            End Get
            Set(ByVal value As Double)
                _Lattitude = value
            End Set
        End Property

        Public Property Longitude() As Double
            Get
                Return _Longitude
            End Get
            Set(ByVal value As Double)
                _Longitude = value
            End Set
        End Property

        Public Property StopCategory() As StopType
            Get
                Return _StopCategory
            End Get
            Set(ByVal value As StopType)
                _StopCategory = value
            End Set
        End Property

        Public Property PickupOrders() As String
            Get
                Return _PickupOrders
            End Get
            Set(ByVal value As String)
                _PickupOrders = value
            End Set
        End Property

        Public Property DropoffOrders() As String
            Get
                Return _DropoffOrders
            End Get
            Set(ByVal value As String)
                _DropoffOrders = value
            End Set
        End Property

        Public Property DateDelivered() As Date?
            Get
                Return _DateDelivered
            End Get
            Set(ByVal value As Date?)
                _DateDelivered = value
            End Set
        End Property

        Public Property Comment() As String
            Get
                Return _Comment
            End Get
            Set(ByVal value As String)
                _Comment = value
            End Set
        End Property

        Public Property StopCompleted() As Boolean
            Get
                Return _StopCompleted
            End Get
            Set(ByVal value As Boolean)
                _StopCompleted = value
            End Set
        End Property

        Public Property ArrivedDate() As Date?
            Get
                Return _ArrivedDate
            End Get
            Set(ByVal value As Date?)
                _ArrivedDate = value
            End Set
        End Property


        Public Property DepartDate() As Date?
            Get
                Return _DepartDate
            End Get
            Set(ByVal value As Date?)
                _DepartDate = value
            End Set
        End Property

    End Class

End Namespace

