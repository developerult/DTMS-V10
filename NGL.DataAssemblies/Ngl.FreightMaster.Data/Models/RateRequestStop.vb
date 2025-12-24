Imports Microsoft.VisualBasic



Public Class RateRequestStop
    Private _RRControl As Integer
    Public Property RRControl() As Integer
        Get
            Return _RRControl
        End Get
        Set
            _RRControl = Value
        End Set
    End Property

    Private _RRSRRControl As Integer
    Public Property RRSRRControl() As Integer
        Get
            Return _RRSRRControl
        End Get
        Set
            _RRSRRControl = Value
        End Set
    End Property

    Private _StopIndex As Integer
    Public Property StopIndex() As Integer
        Get
            Return _StopIndex
        End Get
        Set
            _StopIndex = Value
        End Set
    End Property

    Private _RRSBookControl As Integer
    Public Property RRSBookControl() As Integer
        Get
            Return _RRSBookControl
        End Get
        Set
            _RRSBookControl = Value
        End Set
    End Property

    Private _BookProNumber As String
    Public Property BookProNumber() As String
        Get
            Return left(_BookProNumber, 20)
        End Get
        Set
            _BookProNumber = left(Value, 20)
        End Set
    End Property

    Private _BookCarrOrderNumber As String
    Public Property BookCarrOrderNumber() As String
        Get
            Return left(_BookCarrOrderNumber, 20)
        End Get
        Set
            _BookCarrOrderNumber = left(Value, 20)
        End Set
    End Property


    Private _CompControl As Integer
    Public Property CompControl() As Integer
        Get
            Return _CompControl
        End Get
        Set
            _CompControl = Value
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


    Private _CompAddress1 As String
    Public Property CompAddress1() As String
        Get
            Return left(_CompAddress1, 50)
        End Get
        Set
            _CompAddress1 = left(Value, 50)
        End Set
    End Property

    Private _CompAddress2 As String
    Public Property CompAddress2() As String
        Get
            Return left(_CompAddress2, 50)
        End Get
        Set
            _CompAddress2 = left(Value, 50)
        End Set
    End Property

    Private _CompAddress3 As String
    Public Property CompAddress3() As String
        Get
            Return left(_CompAddress3, 50)
        End Get
        Set
            _CompAddress3 = left(Value, 50)
        End Set
    End Property

    Private _CompCity As String
    Public Property CompCity() As String
        Get
            Return left(_CompCity, 50)
        End Get
        Set
            _CompCity = left(Value, 50)
        End Set
    End Property

    Private _CompState As String
    Public Property CompState() As String
        Get
            Return left(_CompState, 20)
        End Get
        Set
            _CompState = left(Value, 20)
        End Set
    End Property


    Private _CompCountry As String
    Public Property CompCountry() As String
        Get
            Return left(_CompCountry, 50)
        End Get
        Set
            _CompCountry = left(Value, 50)
        End Set
    End Property


    Private _CompPostalCode As String
    Public Property CompPostalCode() As String
        Get
            Return left(_CompPostalCode, 20)
        End Get
        Set
            _CompPostalCode = left(Value, 20)
        End Set
    End Property


    Private _IsPickup As Boolean = False
    Public Property IsPickup() As Boolean
        Get
            Return _IsPickup
        End Get
        Set
            _IsPickup = Value
        End Set
    End Property


    Private _StopNumber As Integer = 1
    Public Property StopNumber() As Integer
        Get
            Return _StopNumber
        End Get
        Set
            _StopNumber = Value
        End Set
    End Property


    Private _TotalCases As Integer = 1
    Public Property TotalCases() As Integer
        Get
            Return _TotalCases
        End Get
        Set
            _TotalCases = Value
        End Set
    End Property

    Private _TotalWgt As Double = 1
    Public Property TotalWgt() As Double
        Get
            Return _TotalWgt
        End Get
        Set
            _TotalWgt = Value
        End Set
    End Property


    Private _TotalPL As Double = 1
    Public Property TotalPL() As Double
        Get
            Return _TotalPL
        End Get
        Set
            _TotalPL = Value
        End Set
    End Property


    Private _TotalCube As Integer = 1
    Public Property TotalCube() As Integer
        Get
            Return _TotalCube
        End Get
        Set
            _TotalCube = Value
        End Set
    End Property


    Private _LoadDate As String
    Public Property LoadDate() As String
        Get
            Return _LoadDate
        End Get
        Set
            _LoadDate = Value
        End Set
    End Property


    Private _RequiredDate As String
    Public Property RequiredDate() As String
        Get
            Return _RequiredDate
        End Get
        Set
            _RequiredDate = Value
        End Set
    End Property

    Private _Items As RateRequestItem()
    Public Property Items() As RateRequestItem()
        Get
            Return _Items
        End Get
        Set
            _Items = Value
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
