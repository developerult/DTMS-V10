<Serializable()> _
Public Class clsPickDetailObject
    Public PLControl As Int64 = 0
    Public BookCarrOrderNumber As String = ""
    Public ItemNumber As String = ""
    Public QtyOrdered As String = ""
    Public FreightCost As String = ""
    Public ItemCost As String = ""
    Public Weight As String = ""
    Public Cube As String = ""
    Public Pack As String = ""
    Public Size As String = ""
    Public Description As String = ""
    Public CustItemNumber As String = ""
    Public CustomerNumber As String = ""
    Public OrderSequence As Integer = 0
    Public Hazmat As String = ""
    Public Brand As String = ""
    Public CostCenter As String = ""
    Public LotNumber As String = ""
    Public LotExpirationDate As String = ""
    Public GTIN As String = ""
    Public BFC As String = ""
    Public CountryOfOrigin As String = ""
    Public CustomerPONumber As String = ""
    Public HST As String = ""
    Public BookProNumber As String = ""
    Public PalletType As String = ""
    Public CompNatNumber As Integer = 0
End Class

<Serializable()> _
Public Class clsPickDetailObject60 : Inherits clsIntegrationItemDetailObject

    Private _PLControl As Int64 = 0
    Public Property PLControl As Int64
        Get
            Return _PLControl
        End Get
        Set(value As Int64)
            _PLControl = value
        End Set
    End Property

    Private _BookCarrOrderNumber As String = ""
    Public Property BookCarrOrderNumber As String
        Get
            Return Left(_BookCarrOrderNumber, 20)
        End Get
        Set(value As String)
            _BookCarrOrderNumber = Left(value, 20)
        End Set
    End Property

    Private _CustomerNumber As String = ""
    Public Property CustomerNumber As String
        Get
            Return Left(_CustomerNumber, 50)
        End Get
        Set(value As String)
            _CustomerNumber = Left(value, 50)
        End Set
    End Property

    Private _OrderSequence As Integer = 0
    Public Property OrderSequence As Integer
        Get
            Return _OrderSequence
        End Get
        Set(value As Integer)
            _OrderSequence = value
        End Set
    End Property

    Private _BookProNumber As String = ""
    Public Property BookProNumber As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _CompNatNumber As Integer = 0
    Public Property CompNatNumber As Integer
        Get
            Return _CompNatNumber
        End Get
        Set(value As Integer)
            _CompNatNumber = value
        End Set
    End Property

End Class

<Serializable()>
Public Class clsPickDetailObject70 : Inherits clsIntegrationItemDetailObject70

    Private _PLControl As Int64 = 0
    Public Property PLControl As Int64
        Get
            Return _PLControl
        End Get
        Set(value As Int64)
            _PLControl = value
        End Set
    End Property

End Class


''' <summary>
''' Pick Worksheet Item Details85
''' </summary>
''' <remarks>
''' Created by RHR v-8.5.1.001 3/21/2022
'''   removed inherits from 70 so all fields are visible in the WSDL
'''   includes BookItemOrderNumber
''' </remarks>
<Serializable()>
Public Class clsPickDetailObject80

    Private _PLControl As Int64 = 0
    Public Property PLControl As Int64
        Get
            Return _PLControl
        End Get
        Set(value As Int64)
            _PLControl = value
        End Set
    End Property


    Private _ItemNumber As String = ""

    Public Property ItemNumber() As String
        Get
            Return Left(_ItemNumber, 50)
        End Get
        Set(ByVal value As String)
            _ItemNumber = Left(value, 50)
        End Set
    End Property

    Private _QtyOrdered As Integer = 0

    Public Property QtyOrdered() As Integer
        Get
            Return _QtyOrdered
        End Get
        Set(ByVal value As Integer)
            _QtyOrdered = value
        End Set
    End Property

    Private _FreightCost As Decimal = 0

    Public Property FreightCost() As Decimal
        Get
            Return _FreightCost
        End Get
        Set(ByVal value As Decimal)
            _FreightCost = value
        End Set
    End Property

    Private _ItemCost As Decimal = 0

    Public Property ItemCost() As Decimal
        Get
            Return _ItemCost
        End Get
        Set(ByVal value As Decimal)
            _ItemCost = value
        End Set
    End Property

    Private _Weight As Double = 0

    Public Property Weight() As Double
        Get
            Return _Weight
        End Get
        Set(ByVal value As Double)
            _Weight = value
        End Set
    End Property

    Private _Cube As Integer = 0

    Public Property Cube() As Integer
        Get
            Return _Cube
        End Get
        Set(ByVal value As Integer)
            _Cube = value
        End Set
    End Property

    Private _Pack As Short = 0

    Public Property Pack() As Short
        Get
            Return _Pack
        End Get
        Set(ByVal value As Short)
            _Pack = value
        End Set
    End Property

    Private _Size As String = ""

    Public Property Size() As String
        Get
            Return Left(_Size, 255)
        End Get
        Set(ByVal value As String)
            _Size = Left(value, 255)
        End Set
    End Property

    Private _Description As String = ""

    Public Property Description() As String
        Get
            Return Left(_Description, 255)
        End Get
        Set(ByVal value As String)
            _Description = Left(value, 255)
        End Set
    End Property

    Private _Hazmat As String

    Public Property Hazmat() As String
        Get
            Return Left(_Hazmat, 1)
        End Get
        Set(ByVal value As String)
            _Hazmat = Left(value, 1)
        End Set
    End Property

    Private _Brand As String = ""

    Public Property Brand() As String
        Get
            Return Left(_Brand, 255)
        End Get
        Set(ByVal value As String)
            _Brand = Left(value, 255)
        End Set
    End Property

    Private _CostCenter As String = ""

    Public Property CostCenter() As String
        Get
            Return Left(_CostCenter, 50)
        End Get
        Set(ByVal value As String)
            _CostCenter = Left(value, 50)
        End Set
    End Property

    Private _LotNumber As String = ""

    Public Property LotNumber() As String
        Get
            Return Left(_LotNumber, 50)
        End Get
        Set(ByVal value As String)
            _LotNumber = Left(value, 50)
        End Set
    End Property

    Private _LotExpirationDate As String

    Public Property LotExpirationDate() As String
        Get
            Return Left(_LotExpirationDate, 50)
        End Get
        Set(ByVal value As String)
            _LotExpirationDate = Left(value, 50)
        End Set
    End Property

    Private _GTIN As String = ""

    Public Property GTIN() As String
        Get
            Return Left(_GTIN, 50)
        End Get
        Set(ByVal value As String)
            _GTIN = Left(value, 50)
        End Set
    End Property

    Private _CustItemNumber As String = ""

    Public Property CustItemNumber() As String
        Get
            Return Left(_CustItemNumber, 50)
        End Get
        Set(ByVal value As String)
            _CustItemNumber = Left(value, 50)
        End Set
    End Property

    Private _BFC As Decimal = 0

    Public Property BFC() As Decimal
        Get
            Return _BFC
        End Get
        Set(ByVal value As Decimal)
            _BFC = value
        End Set
    End Property

    Private _CountryOfOrigin As String = ""

    Public Property CountryOfOrigin() As String
        Get
            Return Left(_CountryOfOrigin, 255)
        End Get
        Set(ByVal value As String)
            _CountryOfOrigin = Left(value, 255)
        End Set
    End Property

    Private _HST As String = ""

    Public Property HST() As String
        Get
            Return Left(_HST, 50)
        End Get
        Set(ByVal value As String)
            _HST = Left(value, 50)
        End Set
    End Property

    Private _PalletType As String = "NA"

    Public Property PalletType() As String
        Get
            Return Left(_PalletType, 50)
        End Get
        Set(ByVal value As String)
            _PalletType = Left(value, 50)
        End Set
    End Property

    Private _HazmatTypeCode As String = ""

    Public Property HazmatTypeCode() As String
        Get
            Return Left(_HazmatTypeCode, 20)
        End Get
        Set(ByVal value As String)
            _HazmatTypeCode = Left(value, 20)
        End Set
    End Property

    Private _Hazmat49CFRCode As String = ""

    Public Property Hazmat49CFRCode() As String
        Get
            Return Left(_Hazmat49CFRCode, 20)
        End Get
        Set(ByVal value As String)
            _Hazmat49CFRCode = Left(value, 20)
        End Set
    End Property

    Private _IATACode As String = ""

    Public Property IATACode() As String
        Get
            Return Left(_IATACode, 20)
        End Get
        Set(ByVal value As String)
            _IATACode = Left(value, 20)
        End Set
    End Property

    Private _DOTCode As String = ""

    Public Property DOTCode() As String
        Get
            Return Left(_DOTCode, 20)
        End Get
        Set(ByVal value As String)
            _DOTCode = Left(value, 20)
        End Set
    End Property

    Private _MarineCode As String = ""

    Public Property MarineCode() As String
        Get
            Return Left(_MarineCode, 20)
        End Get
        Set(ByVal value As String)
            _MarineCode = Left(value, 20)
        End Set
    End Property

    Private _NMFCClass As String = ""

    Public Property NMFCClass() As String
        Get
            Return Left(_NMFCClass, 20)
        End Get
        Set(ByVal value As String)
            _NMFCClass = Left(value, 20)
        End Set
    End Property

    Private _FAKClass As String = ""

    Public Property FAKClass() As String
        Get
            Return Left(_FAKClass, 20)
        End Get
        Set(ByVal value As String)
            _FAKClass = Left(value, 20)
        End Set
    End Property

    Private _LimitedQtyFlag As Boolean = False

    Public Property LimitedQtyFlag() As Boolean
        Get
            Return _LimitedQtyFlag
        End Get
        Set(ByVal value As Boolean)
            _LimitedQtyFlag = value
        End Set
    End Property

    Private _Pallets As Double = 0

    Public Property Pallets() As Double
        Get
            Return _Pallets
        End Get
        Set(ByVal value As Double)
            _Pallets = value
        End Set
    End Property

    Private _Ties As Double = 0

    Public Property Ties() As Double
        Get
            Return _Ties
        End Get
        Set(ByVal value As Double)
            _Ties = value
        End Set
    End Property

    Private _Highs As Double = 0

    Public Property Highs() As Double
        Get
            Return _Highs
        End Get
        Set(ByVal value As Double)
            _Highs = value
        End Set
    End Property

    Private _QtyPalletPercentage As Double = 0

    Public Property QtyPalletPercentage() As Double
        Get
            Return _QtyPalletPercentage
        End Get
        Set(ByVal value As Double)
            _QtyPalletPercentage = value
        End Set
    End Property

    Private _QtyLength As Double = 0

    Public Property QtyLength() As Double
        Get
            Return _QtyLength
        End Get
        Set(ByVal value As Double)
            _QtyLength = value
        End Set
    End Property

    Private _QtyWidth As Double = 0

    Public Property QtyWidth() As Double
        Get
            Return _QtyWidth
        End Get
        Set(ByVal value As Double)
            _QtyWidth = value
        End Set
    End Property

    Private _QtyHeight As Double = 0

    Public Property QtyHeight() As Double
        Get
            Return _QtyHeight
        End Get
        Set(ByVal value As Double)
            _QtyHeight = value
        End Set
    End Property


    Private _Stackable As Boolean = False

    Public Property Stackable() As Boolean
        Get
            Return _Stackable
        End Get
        Set(ByVal value As Boolean)
            _Stackable = value
        End Set
    End Property


    Private _LevelOfDensity As Integer = 0

    Public Property LevelOfDensity() As Integer
        Get
            Return _LevelOfDensity
        End Get
        Set(ByVal value As Integer)
            _LevelOfDensity = value
        End Set
    End Property

    Private _CustomerPONumber As String = ""
    Public Property CustomerPONumber As String
        Get
            Return (Left(_CustomerPONumber, 50))
        End Get
        Set(value As String)
            _CustomerPONumber = Left(value, 50)
        End Set
    End Property

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return Left(_CompLegalEntity, 50)
        End Get
        Set(value As String)
            _CompLegalEntity = Left(value, 50)
        End Set
    End Property

    'Private _CompNumber As Integer = 0
    'Public Property CompNumber() As Integer
    '    Get
    '        Return _CompNumber
    '    End Get
    '    Set(ByVal value As Integer)
    '        _CompNumber = value
    '    End Set
    'End Property

    Private _CompAlphaCode As String = ""
    Public Property CompAlphaCode() As String
        Get
            Return Left(_CompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _BookCarrOrderNumber As String = ""
    Public Property BookCarrOrderNumber() As String
        Get
            Return Left(_BookCarrOrderNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrOrderNumber = Left(value, 20)
        End Set
    End Property

    'Private _BookOrderSequence As Integer = 0
    'Public Property BookOrderSequence() As Integer
    '    Get
    '        Return _BookOrderSequence
    '    End Get
    '    Set(ByVal value As Integer)
    '        _BookOrderSequence = value
    '    End Set
    'End Property

    Private _BookProNumber As String = ""
    Public Property BookProNumber As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookItemDiscount As Double
    Public Property BookItemDiscount() As Double
        Get
            Return _BookItemDiscount
        End Get
        Set(ByVal value As Double)
            _BookItemDiscount = value
        End Set
    End Property

    Private _BookItemLineHaul As Double
    Public Property BookItemLineHaul() As Double
        Get
            Return _BookItemLineHaul
        End Get
        Set(ByVal value As Double)
            _BookItemLineHaul = value
        End Set
    End Property

    Private _BookItemTaxableFees As Double
    Public Property BookItemTaxableFees() As Double
        Get
            Return _BookItemTaxableFees
        End Get
        Set(ByVal value As Double)
            _BookItemTaxableFees = value
        End Set
    End Property

    Private _BookItemTaxes As Double
    Public Property BookItemTaxes() As Double
        Get
            Return _BookItemTaxes
        End Get
        Set(ByVal value As Double)
            _BookItemTaxes = value
        End Set
    End Property

    Private _BookItemNonTaxableFees As Double
    Public Property BookItemNonTaxableFees() As Double
        Get
            Return _BookItemNonTaxableFees
        End Get
        Set(ByVal value As Double)
            _BookItemNonTaxableFees = value
        End Set
    End Property

    Private _BookItemWeightBreak As Double
    Public Property BookItemWeightBreak() As Double
        Get
            Return _BookItemWeightBreak
        End Get
        Set(ByVal value As Double)
            _BookItemWeightBreak = value
        End Set
    End Property

    Private _BookItemRated49CFRCode As String
    Public Property BookItemRated49CFRCode() As String
        Get
            Return Left(_BookItemRated49CFRCode, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRated49CFRCode = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedIATACode As String
    Public Property BookItemRatedIATACode() As String
        Get
            Return Left(_BookItemRatedIATACode, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedIATACode = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedDOTCode As String
    Public Property BookItemRatedDOTCode() As String
        Get
            Return Left(_BookItemRatedDOTCode, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedDOTCode = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedMarineCode As String
    Public Property BookItemRatedMarineCode() As String
        Get
            Return Left(_BookItemRatedMarineCode, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedMarineCode = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedNMFCClass As String
    Public Property BookItemRatedNMFCClass() As String
        Get
            Return Left(_BookItemRatedNMFCClass, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedNMFCClass = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedNMFCSubClass As String
    Public Property BookItemRatedNMFCSubClass() As String
        Get
            Return Left(_BookItemRatedNMFCSubClass, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedNMFCSubClass = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedFAKClass As String
    Public Property BookItemRatedFAKClass() As String
        Get
            Return Left(_BookItemRatedFAKClass, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedFAKClass = Left(value, 20)
        End Set
    End Property

    Private _CustomerNumber As String = ""
    Public Property CustomerNumber As String
        Get
            Return Left(_CustomerNumber, 50)
        End Get
        Set(value As String)
            _CustomerNumber = Left(value, 50)
        End Set
    End Property

    Private _OrderSequence As Integer = 0
    Public Property OrderSequence As Integer
        Get
            Return _OrderSequence
        End Get
        Set(value As Integer)
            _OrderSequence = value
        End Set
    End Property

    Private _CompNatNumber As Integer = 0
    Public Property CompNatNumber As Integer
        Get
            Return _CompNatNumber
        End Get
        Set(value As Integer)
            _CompNatNumber = value
        End Set
    End Property

    Private _OrderNumber As String = ""
    Public Property OrderNumber() As String
        Get
            Return Left(_OrderNumber, 20)
        End Get
        Set(ByVal value As String)
            _OrderNumber = Left(value, 20)
        End Set
    End Property

    Private _BookItemOrderNumber As String = ""
    Public Property BookItemOrderNumber() As String
        Get
            Return Left(_BookItemOrderNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookItemOrderNumber = Left(value, 20)
        End Set
    End Property
End Class



''' <summary>
''' Pick Worksheet Item Details 85
''' </summary>
''' <remarks>
''' Created by RHR v-8.5.1.001 3/21/2022
'''     removed inherits from 70 so all fields are visible in the WSDL
'''     added new field for D365 costing
'''         LineHaulCost, FuelCost, and FeesCost
''' </remarks>
<Serializable()>
Public Class clsPickDetailObject85

    Private _PLControl As Int64 = 0
    Public Property PLControl As Int64
        Get
            Return _PLControl
        End Get
        Set(value As Int64)
            _PLControl = value
        End Set
    End Property


    Private _ItemNumber As String = ""

    Public Property ItemNumber() As String
        Get
            Return Left(_ItemNumber, 50)
        End Get
        Set(ByVal value As String)
            _ItemNumber = Left(value, 50)
        End Set
    End Property

    Private _QtyOrdered As Integer = 0

    Public Property QtyOrdered() As Integer
        Get
            Return _QtyOrdered
        End Get
        Set(ByVal value As Integer)
            _QtyOrdered = value
        End Set
    End Property

    Private _FreightCost As Decimal = 0

    Public Property FreightCost() As Decimal
        Get
            Return _FreightCost
        End Get
        Set(ByVal value As Decimal)
            _FreightCost = value
        End Set
    End Property


    Private _LineHaulCost As Decimal = 0

    Public Property LineHaulCost() As Decimal
        Get
            Return _LineHaulCost
        End Get
        Set(ByVal value As Decimal)
            _LineHaulCost = value
        End Set
    End Property

    Private _FuelCost As Decimal = 0

    Public Property FuelCost() As Decimal
        Get
            Return _FuelCost
        End Get
        Set(ByVal value As Decimal)
            _FuelCost = value
        End Set
    End Property

    Private _FeesCost As Decimal = 0

    Public Property FeesCost() As Decimal
        Get
            Return _FeesCost
        End Get
        Set(ByVal value As Decimal)
            _FeesCost = value
        End Set
    End Property

    Private _ItemCost As Decimal = 0

    Public Property ItemCost() As Decimal
        Get
            Return _ItemCost
        End Get
        Set(ByVal value As Decimal)
            _ItemCost = value
        End Set
    End Property

    Private _Weight As Double = 0

    Public Property Weight() As Double
        Get
            Return _Weight
        End Get
        Set(ByVal value As Double)
            _Weight = value
        End Set
    End Property

    Private _Cube As Integer = 0

    Public Property Cube() As Integer
        Get
            Return _Cube
        End Get
        Set(ByVal value As Integer)
            _Cube = value
        End Set
    End Property

    Private _Pack As Short = 0

    Public Property Pack() As Short
        Get
            Return _Pack
        End Get
        Set(ByVal value As Short)
            _Pack = value
        End Set
    End Property

    Private _Size As String = ""

    Public Property Size() As String
        Get
            Return Left(_Size, 255)
        End Get
        Set(ByVal value As String)
            _Size = Left(value, 255)
        End Set
    End Property

    Private _Description As String = ""

    Public Property Description() As String
        Get
            Return Left(_Description, 255)
        End Get
        Set(ByVal value As String)
            _Description = Left(value, 255)
        End Set
    End Property

    Private _Hazmat As String

    Public Property Hazmat() As String
        Get
            Return Left(_Hazmat, 1)
        End Get
        Set(ByVal value As String)
            _Hazmat = Left(value, 1)
        End Set
    End Property

    Private _Brand As String = ""

    Public Property Brand() As String
        Get
            Return Left(_Brand, 255)
        End Get
        Set(ByVal value As String)
            _Brand = Left(value, 255)
        End Set
    End Property

    Private _CostCenter As String = ""

    Public Property CostCenter() As String
        Get
            Return Left(_CostCenter, 50)
        End Get
        Set(ByVal value As String)
            _CostCenter = Left(value, 50)
        End Set
    End Property

    Private _LotNumber As String = ""

    Public Property LotNumber() As String
        Get
            Return Left(_LotNumber, 50)
        End Get
        Set(ByVal value As String)
            _LotNumber = Left(value, 50)
        End Set
    End Property

    Private _LotExpirationDate As String

    Public Property LotExpirationDate() As String
        Get
            Return Left(_LotExpirationDate, 50)
        End Get
        Set(ByVal value As String)
            _LotExpirationDate = Left(value, 50)
        End Set
    End Property

    Private _GTIN As String = ""

    Public Property GTIN() As String
        Get
            Return Left(_GTIN, 50)
        End Get
        Set(ByVal value As String)
            _GTIN = Left(value, 50)
        End Set
    End Property

    Private _CustItemNumber As String = ""

    Public Property CustItemNumber() As String
        Get
            Return Left(_CustItemNumber, 50)
        End Get
        Set(ByVal value As String)
            _CustItemNumber = Left(value, 50)
        End Set
    End Property

    Private _BFC As Decimal = 0

    Public Property BFC() As Decimal
        Get
            Return _BFC
        End Get
        Set(ByVal value As Decimal)
            _BFC = value
        End Set
    End Property

    Private _CountryOfOrigin As String = ""

    Public Property CountryOfOrigin() As String
        Get
            Return Left(_CountryOfOrigin, 255)
        End Get
        Set(ByVal value As String)
            _CountryOfOrigin = Left(value, 255)
        End Set
    End Property

    Private _HST As String = ""

    Public Property HST() As String
        Get
            Return Left(_HST, 50)
        End Get
        Set(ByVal value As String)
            _HST = Left(value, 50)
        End Set
    End Property

    Private _PalletType As String = "NA"

    Public Property PalletType() As String
        Get
            Return Left(_PalletType, 50)
        End Get
        Set(ByVal value As String)
            _PalletType = Left(value, 50)
        End Set
    End Property

    Private _HazmatTypeCode As String = ""

    Public Property HazmatTypeCode() As String
        Get
            Return Left(_HazmatTypeCode, 20)
        End Get
        Set(ByVal value As String)
            _HazmatTypeCode = Left(value, 20)
        End Set
    End Property

    Private _Hazmat49CFRCode As String = ""

    Public Property Hazmat49CFRCode() As String
        Get
            Return Left(_Hazmat49CFRCode, 20)
        End Get
        Set(ByVal value As String)
            _Hazmat49CFRCode = Left(value, 20)
        End Set
    End Property

    Private _IATACode As String = ""

    Public Property IATACode() As String
        Get
            Return Left(_IATACode, 20)
        End Get
        Set(ByVal value As String)
            _IATACode = Left(value, 20)
        End Set
    End Property

    Private _DOTCode As String = ""

    Public Property DOTCode() As String
        Get
            Return Left(_DOTCode, 20)
        End Get
        Set(ByVal value As String)
            _DOTCode = Left(value, 20)
        End Set
    End Property

    Private _MarineCode As String = ""

    Public Property MarineCode() As String
        Get
            Return Left(_MarineCode, 20)
        End Get
        Set(ByVal value As String)
            _MarineCode = Left(value, 20)
        End Set
    End Property

    Private _NMFCClass As String = ""

    Public Property NMFCClass() As String
        Get
            Return Left(_NMFCClass, 20)
        End Get
        Set(ByVal value As String)
            _NMFCClass = Left(value, 20)
        End Set
    End Property

    Private _FAKClass As String = ""

    Public Property FAKClass() As String
        Get
            Return Left(_FAKClass, 20)
        End Get
        Set(ByVal value As String)
            _FAKClass = Left(value, 20)
        End Set
    End Property

    Private _LimitedQtyFlag As Boolean = False

    Public Property LimitedQtyFlag() As Boolean
        Get
            Return _LimitedQtyFlag
        End Get
        Set(ByVal value As Boolean)
            _LimitedQtyFlag = value
        End Set
    End Property

    Private _Pallets As Double = 0

    Public Property Pallets() As Double
        Get
            Return _Pallets
        End Get
        Set(ByVal value As Double)
            _Pallets = value
        End Set
    End Property

    Private _Ties As Double = 0

    Public Property Ties() As Double
        Get
            Return _Ties
        End Get
        Set(ByVal value As Double)
            _Ties = value
        End Set
    End Property

    Private _Highs As Double = 0

    Public Property Highs() As Double
        Get
            Return _Highs
        End Get
        Set(ByVal value As Double)
            _Highs = value
        End Set
    End Property

    Private _QtyPalletPercentage As Double = 0

    Public Property QtyPalletPercentage() As Double
        Get
            Return _QtyPalletPercentage
        End Get
        Set(ByVal value As Double)
            _QtyPalletPercentage = value
        End Set
    End Property

    Private _QtyLength As Double = 0

    Public Property QtyLength() As Double
        Get
            Return _QtyLength
        End Get
        Set(ByVal value As Double)
            _QtyLength = value
        End Set
    End Property

    Private _QtyWidth As Double = 0

    Public Property QtyWidth() As Double
        Get
            Return _QtyWidth
        End Get
        Set(ByVal value As Double)
            _QtyWidth = value
        End Set
    End Property

    Private _QtyHeight As Double = 0

    Public Property QtyHeight() As Double
        Get
            Return _QtyHeight
        End Get
        Set(ByVal value As Double)
            _QtyHeight = value
        End Set
    End Property


    Private _Stackable As Boolean = False

    Public Property Stackable() As Boolean
        Get
            Return _Stackable
        End Get
        Set(ByVal value As Boolean)
            _Stackable = value
        End Set
    End Property


    Private _LevelOfDensity As Integer = 0

    Public Property LevelOfDensity() As Integer
        Get
            Return _LevelOfDensity
        End Get
        Set(ByVal value As Integer)
            _LevelOfDensity = value
        End Set
    End Property

    Private _CustomerPONumber As String = ""
    Public Property CustomerPONumber As String
        Get
            Return (Left(_CustomerPONumber, 50))
        End Get
        Set(value As String)
            _CustomerPONumber = Left(value, 50)
        End Set
    End Property

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return Left(_CompLegalEntity, 50)
        End Get
        Set(value As String)
            _CompLegalEntity = Left(value, 50)
        End Set
    End Property

    'Private _CompNumber As Integer = 0
    'Public Property CompNumber() As Integer
    '    Get
    '        Return _CompNumber
    '    End Get
    '    Set(ByVal value As Integer)
    '        _CompNumber = value
    '    End Set
    'End Property

    Private _CompAlphaCode As String = ""
    Public Property CompAlphaCode() As String
        Get
            Return Left(_CompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _BookCarrOrderNumber As String = ""
    Public Property BookCarrOrderNumber() As String
        Get
            Return Left(_BookCarrOrderNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrOrderNumber = Left(value, 20)
        End Set
    End Property

    'Private _BookOrderSequence As Integer = 0
    'Public Property BookOrderSequence() As Integer
    '    Get
    '        Return _BookOrderSequence
    '    End Get
    '    Set(ByVal value As Integer)
    '        _BookOrderSequence = value
    '    End Set
    'End Property

    Private _BookProNumber As String = ""
    Public Property BookProNumber As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookItemDiscount As Double
    Public Property BookItemDiscount() As Double
        Get
            Return _BookItemDiscount
        End Get
        Set(ByVal value As Double)
            _BookItemDiscount = value
        End Set
    End Property

    Private _BookItemLineHaul As Double
    Public Property BookItemLineHaul() As Double
        Get
            Return _BookItemLineHaul
        End Get
        Set(ByVal value As Double)
            _BookItemLineHaul = value
        End Set
    End Property

    Private _BookItemTaxableFees As Double
    Public Property BookItemTaxableFees() As Double
        Get
            Return _BookItemTaxableFees
        End Get
        Set(ByVal value As Double)
            _BookItemTaxableFees = value
        End Set
    End Property

    Private _BookItemTaxes As Double
    Public Property BookItemTaxes() As Double
        Get
            Return _BookItemTaxes
        End Get
        Set(ByVal value As Double)
            _BookItemTaxes = value
        End Set
    End Property

    Private _BookItemNonTaxableFees As Double
    Public Property BookItemNonTaxableFees() As Double
        Get
            Return _BookItemNonTaxableFees
        End Get
        Set(ByVal value As Double)
            _BookItemNonTaxableFees = value
        End Set
    End Property

    Private _BookItemWeightBreak As Double
    Public Property BookItemWeightBreak() As Double
        Get
            Return _BookItemWeightBreak
        End Get
        Set(ByVal value As Double)
            _BookItemWeightBreak = value
        End Set
    End Property

    Private _BookItemRated49CFRCode As String
    Public Property BookItemRated49CFRCode() As String
        Get
            Return Left(_BookItemRated49CFRCode, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRated49CFRCode = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedIATACode As String
    Public Property BookItemRatedIATACode() As String
        Get
            Return Left(_BookItemRatedIATACode, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedIATACode = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedDOTCode As String
    Public Property BookItemRatedDOTCode() As String
        Get
            Return Left(_BookItemRatedDOTCode, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedDOTCode = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedMarineCode As String
    Public Property BookItemRatedMarineCode() As String
        Get
            Return Left(_BookItemRatedMarineCode, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedMarineCode = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedNMFCClass As String
    Public Property BookItemRatedNMFCClass() As String
        Get
            Return Left(_BookItemRatedNMFCClass, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedNMFCClass = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedNMFCSubClass As String
    Public Property BookItemRatedNMFCSubClass() As String
        Get
            Return Left(_BookItemRatedNMFCSubClass, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedNMFCSubClass = Left(value, 20)
        End Set
    End Property

    Private _BookItemRatedFAKClass As String
    Public Property BookItemRatedFAKClass() As String
        Get
            Return Left(_BookItemRatedFAKClass, 20)
        End Get
        Set(ByVal value As String)
            _BookItemRatedFAKClass = Left(value, 20)
        End Set
    End Property

    Private _CustomerNumber As String = ""
    Public Property CustomerNumber As String
        Get
            Return Left(_CustomerNumber, 50)
        End Get
        Set(value As String)
            _CustomerNumber = Left(value, 50)
        End Set
    End Property

    Private _OrderSequence As Integer = 0
    Public Property OrderSequence As Integer
        Get
            Return _OrderSequence
        End Get
        Set(value As Integer)
            _OrderSequence = value
        End Set
    End Property

    Private _CompNatNumber As Integer = 0
    Public Property CompNatNumber As Integer
        Get
            Return _CompNatNumber
        End Get
        Set(value As Integer)
            _CompNatNumber = value
        End Set
    End Property

    Private _OrderNumber As String = ""
    Public Property OrderNumber() As String
        Get
            Return Left(_OrderNumber, 20)
        End Get
        Set(ByVal value As String)
            _OrderNumber = Left(value, 20)
        End Set
    End Property


    Private _BookItemOrderNumber As String = ""
    Public Property BookItemOrderNumber() As String
        Get
            Return Left(_BookItemOrderNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookItemOrderNumber = Left(value, 20)
        End Set
    End Property

End Class