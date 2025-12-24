Imports Microsoft.VisualBasic



Public Class RateRequestItem
    Private _RRIControl As Integer
    Public Property RRIControl() As Integer
        Get
            Return _RRIControl
        End Get
        Set
            _RRIControl = Value
        End Set
    End Property

    Private _RRIRRSControl As Integer
    Public Property RRIRRSControl() As Integer
        Get
            Return _RRIRRSControl
        End Get
        Set
            _RRIRRSControl = Value
        End Set
    End Property

    Private _ItemIndex As Integer
    Public Property ItemIndex() As Integer
        Get
            Return _ItemIndex
        End Get
        Set
            _ItemIndex = Value
        End Set
    End Property

    Private _ItemStopIndex As Integer
    Public Property ItemStopIndex() As Integer
        Get
            Return _ItemStopIndex
        End Get
        Set
            _ItemStopIndex = Value
        End Set
    End Property


    Private _ItemControl As Integer
    Public Property ItemControl() As Integer
        Get
            Return _ItemControl
        End Get
        Set
            _ItemControl = Value
        End Set
    End Property

    Private _ItemNumber As String
    Public Property ItemNumber() As String
        Get
            Return left(_ItemNumber, 50)
        End Get
        Set
            _ItemNumber = left(Value, 50)
        End Set
    End Property

    Private _Weight As Double = 0
    Public Property Weight() As Double
        Get
            Return _Weight
        End Get
        Set
            _Weight = Value
        End Set
    End Property


    Private _WeightUnit As String = "lbs"
    Public Property WeightUnit() As String
        Get
            Return left(_WeightUnit, 20)
        End Get
        Set
            _WeightUnit = left(Value, 20)
        End Set
    End Property


    Private _FreightClass As String = "50"
    Public Property FreightClass() As String
        Get
            Return left(_FreightClass, 20)
        End Get
        Set
            _FreightClass = left(Value, 20)
        End Set
    End Property


    Private _PalletCount As Double = 1
    Public Property PalletCount() As Double
        Get
            Return _PalletCount
        End Get
        Set
            _PalletCount = Value
        End Set
    End Property

    Private _numPieces As Integer = 1
    Public Property NumPieces() As Integer
        Get
            Integer.TryParse(Me.Quantity, _numPieces)
            Return _numPieces
        End Get

        Set
            _numPieces = Value
        End Set
    End Property

    Private _Description As String
    Public Property Description() As String
        Get
            Return left(_Description, 255)
        End Get
        Set
            _Description = left(Value, 255)
        End Set
    End Property


    Private _Quantity As Integer
    Public Property Quantity() As Integer
        Get
            Return _Quantity
        End Get
        Set
            _Quantity = Value
        End Set
    End Property


    Private _HazmatId As String
    Public Property HazmatId() As String
        Get
            Return left(_HazmatId, 20)
        End Get
        Set
            _HazmatId = left(Value, 20)
        End Set
    End Property


    Private _Code As String
    Public Property Code() As String
        Get
            Return left(_Code, 20)
        End Get
        Set
            _Code = left(Value, 20)
        End Set
    End Property


    Private _HazmatClass As String
    Public Property HazmatClass() As String
        Get
            Return left(_HazmatClass, 20)
        End Get
        Set
            _HazmatClass = left(Value, 20)
        End Set
    End Property


    Private _IsHazmat As Boolean = 0
    Public Property IsHazmat() As Boolean
        Get
            Return _IsHazmat
        End Get
        Set
            _IsHazmat = Value
        End Set
    End Property

    Private _Pieces As String
    Public Property Pieces() As String
        Get
            Return left(_Pieces, 20)
        End Get
        Set
            _Pieces = left(Value, 20)
        End Set
    End Property


    Private _PackageType As String
    Public Property PackageType() As String
        Get
            Return left(_PackageType, 50)
        End Get
        Set
            _PackageType = left(Value, 50)
        End Set
    End Property


    Private _Length As Double
    Public Property Length() As Double
        Get
            Return _Length
        End Get
        Set
            _Length = Value
        End Set
    End Property

    Private _Width As Double
    Public Property Width() As Double
        Get
            Return _Width
        End Get
        Set
            _Width = Value
        End Set
    End Property


    Private _Height As Double
    Public Property Height() As Double
        Get
            Return _Height
        End Get
        Set
            _Height = Value
        End Set
    End Property


    Private _Density As String
    Public Property Density() As String
        Get
            Return left(_Density, 50)
        End Get
        Set
            _Density = left(Value, 50)
        End Set
    End Property


    Private _NMFCItem As String
    Public Property NMFCItem() As String
        Get
            Return left(_NMFCItem, 20)
        End Get
        Set
            _NMFCItem = left(Value, 20)
        End Set
    End Property


    Private _NMFCSub As String
    Public Property NMFCSub() As String
        Get
            Return left(_NMFCSub, 20)
        End Get
        Set
            _NMFCSub = left(Value, 20)
        End Set
    End Property


    Private _Stackable As Boolean = False
    Public Property Stackable() As Boolean
        Get
            Return _Stackable
        End Get
        Set
            _Stackable = Value
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

