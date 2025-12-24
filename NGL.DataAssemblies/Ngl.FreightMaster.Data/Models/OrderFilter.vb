Namespace Models

    Public Class OrderFilter

    Public Property BookControl() As Integer
        Get
            Return m_BookControl
        End Get
        Set
            m_BookControl = Value
        End Set
    End Property
    Private m_BookControl As Integer
    Public Property BookCustCompControl() As Integer
        Get
            Return m_BookCustCompControl
        End Get
        Set
            m_BookCustCompControl = Value
        End Set
    End Property
    Private m_BookCustCompControl As Integer
    Public Property CompNameNumberCode() As String
        Get
            Return m_CompNameNumberCode
        End Get
        Set
            m_CompNameNumberCode = Value
        End Set
    End Property
        Private m_CompNameNumberCode As String = ""
        Public Property BookCarrierControl() As Integer
        Get
            Return m_BookCarrierControl
        End Get
        Set
            m_BookCarrierControl = Value
        End Set
    End Property
    Private m_BookCarrierControl As Integer
    Public Property CarrierNameNumberCode() As String
        Get
            Return m_CarrierNameNumberCode
        End Get
        Set
            m_CarrierNameNumberCode = Value
        End Set
    End Property
        Private m_CarrierNameNumberCode As String = ""
        Public Property LoadDateFrom() As String
        Get
            Return m_LoadDateFrom
        End Get
        Set
            m_LoadDateFrom = Value
        End Set
    End Property
    Private m_LoadDateFrom As String
    Public Property LoadDateTo() As String
        Get
            Return m_LoadDateTo
        End Get
        Set
            m_LoadDateTo = Value
        End Set
    End Property
        Private m_LoadDateTo As String = ""
        Public Property ReqDateFrom() As String
        Get
            Return m_ReqDateFrom
        End Get
        Set
            m_ReqDateFrom = Value
        End Set
    End Property
        Private m_ReqDateFrom As String = ""
        Public Property ReqDateTo() As String
        Get
            Return m_ReqDateTo
        End Get
        Set
            m_ReqDateTo = Value
        End Set
    End Property
        Private m_ReqDateTo As String = ""

        Public Property BookCarrOrderNumber() As String
        Get
            Return m_BookCarrOrderNumber
        End Get
        Set
            m_BookCarrOrderNumber = Value
        End Set
    End Property
        Private m_BookCarrOrderNumber As String = ""
        Public Property BookProNumber() As String
        Get
            Return m_BookProNumber
        End Get
        Set
            m_BookProNumber = Value
        End Set
    End Property
        Private m_BookProNumber As String = ""
        Public Property BookSHID() As String
        Get
            Return m_BookSHID
        End Get
        Set
            m_BookSHID = Value
        End Set
    End Property
        Private m_BookSHID As String = ""
        Public Property BookConsPrefix() As String
        Get
            Return m_BookConsPrefix
        End Get
        Set
            m_BookConsPrefix = Value
        End Set
    End Property
        Private m_BookConsPrefix As String = ""
        Public Property BookOrigName() As String
        Get
            Return m_BookOrigName
        End Get
        Set
            m_BookOrigName = Value
        End Set
    End Property
        Private m_BookOrigName As String = ""
        Public Property BookOrigAddress1() As String
        Get
            Return m_BookOrigAddress1
        End Get
        Set
            m_BookOrigAddress1 = Value
        End Set
    End Property
        Private m_BookOrigAddress1 As String = ""
        Public Property BookOrigCity() As String
        Get
            Return m_BookOrigCity
        End Get
        Set
            m_BookOrigCity = Value
        End Set
    End Property
        Private m_BookOrigCity As String = ""
        Public Property BookOrigState() As String
        Get
            Return m_BookOrigState
        End Get
        Set
            m_BookOrigState = Value
        End Set
    End Property
        Private m_BookOrigState As String = ""
        Public Property BookOrigCountry() As String
        Get
            Return m_BookOrigCountry
        End Get
        Set
            m_BookOrigCountry = Value
        End Set
    End Property
        Private m_BookOrigCountry As String = ""
        Public Property BookOrigZip() As String
        Get
            Return m_BookOrigZip
        End Get
        Set
            m_BookOrigZip = Value
        End Set
    End Property
        Private m_BookOrigZip As String = ""
        Public Property BookDestName() As String
        Get
            Return m_BookDestName
        End Get
        Set
            m_BookDestName = Value
        End Set
    End Property
        Private m_BookDestName As String = ""
        Public Property BookDestAddress1() As String
        Get
            Return m_BookDestAddress1
        End Get
        Set
            m_BookDestAddress1 = Value
        End Set
    End Property
        Private m_BookDestAddress1 As String = ""
        Public Property BookDestCity() As String
        Get
            Return m_BookDestCity
        End Get
        Set
            m_BookDestCity = Value
        End Set
    End Property
        Private m_BookDestCity As String = ""
        Public Property BookDestState() As String
        Get
            Return m_BookDestState
        End Get
        Set
            m_BookDestState = Value
        End Set
    End Property
        Private m_BookDestState As String = ""
        Public Property BookDestCountry() As String
        Get
            Return m_BookDestCountry
        End Get
        Set
            m_BookDestCountry = Value
        End Set
    End Property
        Private m_BookDestCountry As String = ""
        Public Property BookDestZip() As String
        Get
            Return m_BookDestZip
        End Get
        Set
            m_BookDestZip = Value
        End Set
    End Property
    Private m_BookDestZip As String
    Public Property BookTotalCases() As Integer
        Get
            Return m_BookTotalCases
        End Get
        Set
            m_BookTotalCases = Value
        End Set
    End Property
    Private m_BookTotalCases As Integer
    Public Property BookTotalWgt() As Double
        Get
            Return m_BookTotalWgt
        End Get
        Set
            m_BookTotalWgt = Value
        End Set
    End Property
    Private m_BookTotalWgt As Double
    Public Property BookTotalPL() As Double
        Get
            Return m_BookTotalPL
        End Get
        Set
            m_BookTotalPL = Value
        End Set
    End Property
    Private m_BookTotalPL As Double
    Public Property BookTotalCube() As Integer
        Get
            Return m_BookTotalCube
        End Get
        Set
            m_BookTotalCube = Value
        End Set
    End Property
    Private m_BookTotalCube As Integer
    Public Property NewOrders() As Boolean
        Get
            Return m_NewOrders
        End Get
        Set
            m_NewOrders = Value
        End Set
    End Property
        Private m_NewOrders As Boolean = True
        Public Property AssignedOrders() As Boolean
        Get
            Return m_AssignedOrders
        End Get
        Set
            m_AssignedOrders = Value
        End Set
    End Property
        Private m_AssignedOrders As Boolean = True
        Public Property TenderedOrders() As Boolean
        Get
            Return m_TenderedOrders
        End Get
        Set
            m_TenderedOrders = Value
        End Set
    End Property
        Private m_TenderedOrders As Boolean = False
        Public Property AcceptedOrders() As Boolean
        Get
            Return m_AcceptedOrders
        End Get
        Set
            m_AcceptedOrders = Value
        End Set
    End Property
        Private m_AcceptedOrders As Boolean = False
        Public Property ShippedOrders() As Boolean
        Get
            Return m_ShippedOrders
        End Get
        Set
            m_ShippedOrders = Value
        End Set
    End Property
        Private m_ShippedOrders As Boolean = False
        Public Property DeliveredOrders() As Boolean
        Get
            Return m_DeliveredOrders
        End Get
        Set
            m_DeliveredOrders = Value
        End Set
    End Property
        Private m_DeliveredOrders As Boolean = False
        Public Property LTLOnly() As Boolean
        Get
            Return m_LTLOnly
        End Get
        Set
            m_LTLOnly = Value
        End Set
    End Property
        Private m_LTLOnly As Boolean = False
        Public Property CNSOnly() As Boolean
        Get
            Return m_CNSOnly
        End Get
        Set
            m_CNSOnly = Value
        End Set
    End Property
        Private m_CNSOnly As Boolean = False


        Private _BookRouteConsFlag As Boolean
        Public Property BookRouteConsFlag() As Boolean
            Get
                Return _BookRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                _BookRouteConsFlag = value
            End Set
        End Property

    End Class


End Namespace