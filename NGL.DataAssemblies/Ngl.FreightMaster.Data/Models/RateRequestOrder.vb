Imports Microsoft.VisualBasic


Public Class RateRequestOrder

    Private _RRControl As Integer
    Public Property RRControl() As Integer
        Get
            Return _RRControl
        End Get
        Set
            _RRControl = Value
        End Set
    End Property

    Private _RRUserSecurityControl As Integer
    Public Property RRUserSecurityControl() As Integer
        Get
            Return _RRUserSecurityControl
        End Get
        Set
            _RRUserSecurityControl = Value
        End Set
    End Property

    Private _RRBookSHID As String
    Public Property RRBookSHID() As String
        Get
            Return left(_RRBookSHID, 50)
        End Get
        Set
            _RRBookSHID = left(Value, 50)
        End Set
    End Property


    Private _BookConsPrefix As String
    Public Property BookConsPrefix() As String
        Get
            Return left(_BookConsPrefix, 20)
        End Get
        Set
            _BookConsPrefix = left(Value, 20)
        End Set
    End Property

    Private _ShipDate As String
    Public Property ShipDate() As String
        Get
            Return _ShipDate
        End Get
        Set
            _ShipDate = Value
        End Set
    End Property

    Private _DeliveryDate As String
    Public Property DeliveryDate() As String
        Get
            Return _DeliveryDate
        End Get
        Set
            _DeliveryDate = Value
        End Set
    End Property

    Private _BookCustCompControl As Integer
    Public Property BookCustCompControl() As Integer
        Get
            Return _BookCustCompControl
        End Get
        Set
            _BookCustCompControl = Value
        End Set
    End Property

    Private _CompName As String
    Public Property CompName() As String
        Get
            Return left(_CompName, 50)
        End Get
        Set
            _CompName = left(Value, 50)
        End Set
    End Property

    Private _CompNumber As Integer
    Public Property CompNumber() As Integer
        Get
            Return _CompNumber
        End Get
        Set
            _CompNumber = Value
        End Set
    End Property


    Private _CompAlphaCode As String
    Public Property CompAlphaCode() As String
        Get
            Return left(_CompAlphaCode, 50)
        End Get
        Set
            _CompAlphaCode = left(Value, 50)
        End Set
    End Property

    Private _BookCarrierControl As Integer
    Public Property BookCarrierControl() As Integer
        Get
            Return _BookCarrierControl
        End Get
        Set
            _BookCarrierControl = Value
        End Set
    End Property

    Private _CarrierName As String
    Public Property CarrierName() As String
        Get
            Return left(_CarrierName, 50)
        End Get
        Set
            _CarrierName = left(Value, 50)
        End Set
    End Property

    Private _CarrierNumber As Integer
    Public Property CarrierNumber() As Integer
        Get
            Return _CarrierNumber
        End Get
        Set
            _CarrierNumber = Value
        End Set
    End Property

    Private _CarrierAlphaCode As String
    Public Property CarrierAlphaCode() As String
        Get
            Return left(_CarrierAlphaCode, 50)
        End Get
        Set
            _CarrierAlphaCode = left(Value, 50)
        End Set
    End Property

    Private _TotalCases As Integer
    Public Property TotalCases() As Integer
        Get
            Return _TotalCases
        End Get
        Set
            _TotalCases = Value
        End Set
    End Property

    Private _TotalWgt As Double
    Public Property TotalWgt() As Double
        Get
            Return _TotalWgt
        End Get
        Set
            _TotalWgt = Value
        End Set
    End Property

    Private _TotalPL As Double
    Public Property TotalPL() As Double
        Get
            Return _TotalPL
        End Get
        Set
            _TotalPL = Value
        End Set
    End Property

    Private _TotalCube As Integer
    Public Property TotalCube() As Integer
        Get
            Return _TotalCube
        End Get
        Set
            _TotalCube = Value
        End Set
    End Property

    Private _TotalStops As Integer
    Public Property TotalStops() As Integer
        Get
            Return _TotalStops
        End Get
        Set
            _TotalStops = Value
        End Set
    End Property

    Private _Pickups As RateRequestStop()
    Public Property Pickups() As RateRequestStop()
        Get
            Return _Pickups
        End Get
        Set
            _Pickups = Value
        End Set
    End Property

    Private _Stops As RateRequestStop()
    Public Property Stops() As RateRequestStop()
        Get
            Return _Stops
        End Get
        Set
            _Stops = Value
        End Set
    End Property

    Private _ModDate As String
    Public Property ModDate() As String
        Get
            Return _ModDate
        End Get
        Set
            _ModDate = Value
        End Set
    End Property

End Class


