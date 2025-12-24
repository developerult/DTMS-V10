<Serializable()> _
Public Class clsPickListObject
    Private _PLControl As Int64 = 0
    Public Property PLControl() As Int64
        Get
            Return _PLControl
        End Get
        Set(ByVal value As Int64)
            _PLControl = value
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

    Private _BookConsPrefix As String = ""
    Public Property BookConsPrefix() As String
        Get
            Return Left(_BookConsPrefix, 20)
        End Get
        Set(ByVal value As String)
            _BookConsPrefix = Left(value, 20)
        End Set
    End Property

    Private _CarrierNumber As String = ""
    Public Property CarrierNumber() As String
        Get
            Return Left(_CarrierNumber, 50)
        End Get
        Set(ByVal value As String)
            _CarrierNumber = Left(value, 50)
        End Set
    End Property

    Private _BookRevTotalCost As String = ""
    Public Property BookRevTotalCost() As String
        Get
            Return Left(_BookRevTotalCost, 20)
        End Get
        Set(ByVal value As String)
            _BookRevTotalCost = Left(value, 20)
        End Set
    End Property

    Private _LoadOrder As String = ""
    Public Property LoadOrder() As String
        Get
            Return Left(_LoadOrder, 6)
        End Get
        Set(ByVal value As String)
            _LoadOrder = Left(value, 6)
        End Set
    End Property

    Private _BookDateLoad As String = ""
    Public Property BookDateLoad() As String
        Get
            Return Left(_BookDateLoad, 25)
        End Get
        Set(ByVal value As String)
            _BookDateLoad = Left(value, 25)
        End Set
    End Property

    Private _BookDateRequired As String = ""
    Public Property BookDateRequired() As String
        Get
            Return Left(_BookDateRequired, 25)
        End Get
        Set(ByVal value As String)
            _BookDateRequired = Left(value, 25)
        End Set
    End Property

    Private _BookLoadCom As String = ""
    Public Property BookLoadCom() As String
        Get
            Return Left(_BookLoadCom, 1)
        End Get
        Set(ByVal value As String)
            _BookLoadCom = Left(value, 1)
        End Set
    End Property

    Private _BookProNumber As String = ""
    Public Property BookProNumber() As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookRouteFinalCode As String = ""
    Public Property BookRouteFinalCode() As String
        Get
            Return Left(_BookRouteFinalCode, 2)
        End Get
        Set(ByVal value As String)
            _BookRouteFinalCode = Left(value, 2)
        End Set
    End Property

    Private _BookRouteFinalDate As String = ""
    Public Property BookRouteFinalDate() As String
        Get
            Return Left(_BookRouteFinalDate, 25)
        End Get
        Set(ByVal value As String)
            _BookRouteFinalDate = Left(value, 25)
        End Set
    End Property

    Private _BookTotalCases As String = ""
    Public Property BookTotalCases() As String
        Get
            Return Left(_BookTotalCases, 12)
        End Get
        Set(ByVal value As String)
            _BookTotalCases = Left(value, 12)
        End Set
    End Property

    Private _BookTotalWgt As String = ""
    Public Property BookTotalWgt() As String
        Get
            Return Left(_BookTotalWgt, 22)
        End Get
        Set(ByVal value As String)
            _BookTotalWgt = Left(value, 22)
        End Set
    End Property

    Private _BookTotalPL As String = ""
    Public Property BookTotalPL() As String
        Get
            Return Left(_BookTotalPL, 22)
        End Get
        Set(ByVal value As String)
            _BookTotalPL = Left(value, 22)
        End Set
    End Property

    Private _BookTotalCube As String = ""
    Public Property BookTotalCube() As String
        Get
            Return Left(_BookTotalCube, 12)
        End Get
        Set(ByVal value As String)
            _BookTotalCube = Left(value, 12)
        End Set
    End Property

    Private _BookTotalBFC As String = ""
    Public Property BookTotalBFC() As String
        Get
            Return Left(_BookTotalBFC, 20)
        End Get
        Set(ByVal value As String)
            _BookTotalBFC = Left(value, 20)
        End Set
    End Property

    Private _BookStopNo As String = ""
    Public Property BookStopNo() As String
        Get
            Return Left(_BookStopNo, 12)
        End Get
        Set(ByVal value As String)
            _BookStopNo = Left(value, 12)
        End Set
    End Property

    Private _CompName As String = ""
    Public Property CompName() As String
        Get
            Return Left(_CompName, 40)
        End Get
        Set(ByVal value As String)
            _CompName = Left(value, 40)
        End Set
    End Property

    Private _CompNumber As String = ""
    Public Property CompNumber() As String
        Get
            Return Left(_CompNumber, 50)
        End Get
        Set(ByVal value As String)
            _CompNumber = Left(value, 50)
        End Set
    End Property

    Private _BookTypeCode As String = ""
    Public Property BookTypeCode() As String
        Get
            Return Left(_BookTypeCode, 20)
        End Get
        Set(ByVal value As String)
            _BookTypeCode = Left(value, 20)
        End Set
    End Property

    Private _BookDateOrdered As String = ""
    Public Property BookDateOrdered() As String
        Get
            Return Left(_BookDateOrdered, 25)
        End Get
        Set(ByVal value As String)
            _BookDateOrdered = Left(value, 25)
        End Set
    End Property

    Private _BookOrigName As String = ""
    Public Property BookOrigName() As String
        Get
            Return Left(_BookOrigName, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigName = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress1 As String = ""
    Public Property BookOrigAddress1() As String
        Get
            Return Left(_BookOrigAddress1, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress1 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress2 As String = ""
    Public Property BookOrigAddress2() As String
        Get
            Return Left(_BookOrigAddress2, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress2 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress3 As String = ""
    Public Property BookOrigAddress3() As String
        Get
            Return Left(_BookOrigAddress3, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress3 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigCity As String = ""
    Public Property BookOrigCity() As String
        Get
            Return Left(_BookOrigCity, 25)
        End Get
        Set(ByVal value As String)
            _BookOrigCity = Left(value, 25)
        End Set
    End Property

    Private _BookOrigState As String = ""
    Public Property BookOrigState() As String
        Get
            Return Left(_BookOrigState, 8)
        End Get
        Set(ByVal value As String)
            _BookOrigState = Left(value, 8)
        End Set
    End Property

    Private _BookOrigCountry As String = ""
    Public Property BookOrigCountry() As String
        Get
            Return Left(_BookOrigCountry, 30)
        End Get
        Set(ByVal value As String)
            _BookOrigCountry = Left(value, 30)
        End Set
    End Property

    Private _BookOrigZip As String = ""
    Public Property BookOrigZip() As String
        Get
            Return Left(_BookOrigZip, 10) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _BookOrigZip = Left(value, 10) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _BookDestName As String = ""
    Public Property BookDestName() As String
        Get
            Return Left(_BookDestName, 40)
        End Get
        Set(ByVal value As String)
            _BookDestName = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress1 As String = ""
    Public Property BookDestAddress1() As String
        Get
            Return Left(_BookDestAddress1, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress1 = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress2 As String = ""
    Public Property BookDestAddress2() As String
        Get
            Return Left(_BookDestAddress2, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress2 = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress3 As String = ""
    Public Property BookDestAddress3() As String
        Get
            Return Left(_BookDestAddress3, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress3 = Left(value, 40)
        End Set
    End Property

    Private _BookDestCity As String = ""
    Public Property BookDestCity() As String
        Get
            Return Left(_BookDestCity, 25)
        End Get
        Set(ByVal value As String)
            _BookDestCity = Left(value, 25)
        End Set
    End Property

    Private _BookDestState As String = ""
    Public Property BookDestState() As String
        Get
            Return Left(_BookDestState, 2)
        End Get
        Set(ByVal value As String)
            _BookDestState = Left(value, 2)
        End Set
    End Property

    Private _BookDestCountry As String = ""
    Public Property BookDestCountry() As String
        Get
            Return Left(_BookDestCountry, 30)
        End Get
        Set(ByVal value As String)
            _BookDestCountry = Left(value, 30)
        End Set
    End Property

    Private _BookDestZip As String = ""
    Public Property BookDestZip() As String
        Get
            Return Left(_BookDestZip, 10)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _BookDestZip = Left(value, 10) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _BookLoadPONumber As String = ""
    Public Property BookLoadPONumber() As String
        Get
            Return Left(_BookLoadPONumber, 20)
        End Get
        Set(ByVal value As String)
            _BookLoadPONumber = Left(value, 20)
        End Set
    End Property

    Private _CarrierName As String = ""
    Public Property CarrierName() As String
        Get
            Return Left(_CarrierName, 40)
        End Get
        Set(ByVal value As String)
            _CarrierName = Left(value, 40)
        End Set
    End Property

    Private _LaneNumber As String = ""
    Public Property LaneNumber() As String
        Get
            Return Left(_LaneNumber, 50)
        End Get
        Set(ByVal value As String)
            _LaneNumber = Left(value, 50)
        End Set
    End Property

    Private _CommCodeDescription As String = ""
    Public Property CommCodeDescription() As String
        Get
            Return Left(_CommCodeDescription, 40)
        End Get
        Set(ByVal value As String)
            _CommCodeDescription = Left(value, 40)
        End Set
    End Property

    Private _BookMilesFrom As String = ""
    Public Property BookMilesFrom() As String
        Get
            Return Left(_BookMilesFrom, 22)
        End Get
        Set(ByVal value As String)
            _BookMilesFrom = Left(value, 22)
        End Set
    End Property

    Private _BookCommCompControl As Integer = 0
    Public Property BookCommCompControl() As Integer
        Get
            Return _BookCommCompControl
        End Get
        Set(ByVal value As Integer)
            _BookCommCompControl = value
        End Set
    End Property

    Private _BookRevCommCost As Double = 0
    Public Property BookRevCommCost() As Double
        Get
            Return _BookRevCommCost
        End Get
        Set(ByVal value As Double)
            _BookRevCommCost = value
        End Set
    End Property

    Private _BookRevGrossRevenue As Double = 0
    Public Property BookRevGrossRevenue() As Double
        Get
            Return _BookRevGrossRevenue
        End Get
        Set(ByVal value As Double)
            _BookRevGrossRevenue = value
        End Set
    End Property

    Private _BookFinCommStd As Double = 0
    Public Property BookFinCommStd() As Double
        Get
            Return _BookFinCommStd
        End Get
        Set(ByVal value As Double)
            _BookFinCommStd = value
        End Set
    End Property

    Private _BookDoNotInvoice As Boolean = False
    Public Property BookDoNotInvoice() As Boolean
        Get
            Return _BookDoNotInvoice
        End Get
        Set(ByVal value As Boolean)
            _BookDoNotInvoice = value
        End Set
    End Property

    Private _BookOrderSequence As Integer = 0
    Public Property BookOrderSequence() As Integer
        Get
            Return _BookOrderSequence
        End Get
        Set(ByVal value As Integer)
            _BookOrderSequence = value
        End Set
    End Property

    Private _CarrierEquipmentCodes As String = ""
    Public Property CarrierEquipmentCodes() As String
        Get
            Return Left(_CarrierEquipmentCodes, 50)
        End Get
        Set(ByVal value As String)
            _CarrierEquipmentCodes = Left(value, 50)
        End Set
    End Property

    Private _BookCarrierTypeCode As String = ""
    Public Property BookCarrierTypeCode() As String
        Get
            Return Left(_BookCarrierTypeCode, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrierTypeCode = Left(value, 20)
        End Set
    End Property

    Private _BookWarehouseNumber As String = ""
    Public Property BookWarehouseNumber() As String
        Get
            Return Left(_BookWarehouseNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookWarehouseNumber = Left(value, 20)
        End Set
    End Property

    Private _CompNatNumber As Integer = 0
    Public Property CompNatNumber() As Integer
        Get
            Return _CompNatNumber
        End Get
        Set(ByVal value As Integer)
            _CompNatNumber = value
        End Set
    End Property

    Private _BookTransType As String = ""
    Public Property BookTransType() As String
        Get
            Return Left(_BookTransType, 50)
        End Get
        Set(ByVal value As String)
            _BookTransType = Left(value, 50)
        End Set
    End Property

    Private _BookShipCarrierProNumber As String = ""
    Public Property BookShipCarrierProNumber() As String
        Get
            Return Left(_BookShipCarrierProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookShipCarrierNumber As String = ""
    Public Property BookShipCarrierNumber() As String
        Get
            Return Left(_BookShipCarrierNumber, 80)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierNumber = Left(value, 80)
        End Set
    End Property

    Private _LaneComments As String = ""
    Public Property LaneComments() As String
        Get
            Return Left(_LaneComments, 255)
        End Get
        Set(ByVal value As String)
            _LaneComments = Left(value, 255)
        End Set
    End Property

    Private _FuelSurCharge As Double = 0
    Public Property FuelSurCharge() As Double
        Get
            Return _FuelSurCharge
        End Get
        Set(ByVal value As Double)
            _FuelSurCharge = value
        End Set
    End Property

    Private _BookRevCarrierCost As Double = 0
    Public Property BookRevCarrierCost() As Double
        Get
            Return _BookRevCarrierCost
        End Get
        Set(ByVal value As Double)
            _BookRevCarrierCost = value
        End Set
    End Property

    Private _BookRevOtherCost As Double = 0
    Public Property BookRevOtherCost() As Double
        Get
            Return _BookRevOtherCost
        End Get
        Set(ByVal value As Double)
            _BookRevOtherCost = value
        End Set
    End Property

    Private _BookRevNetCost As Double = 0
    Public Property BookRevNetCost() As Double
        Get
            Return _BookRevNetCost
        End Get
        Set(ByVal value As Double)
            _BookRevNetCost = value
        End Set
    End Property

    Private _BookRevFreightTax As Double = 0
    Public Property BookRevFreightTax() As Double
        Get
            Return _BookRevFreightTax
        End Get
        Set(ByVal value As Double)
            _BookRevFreightTax = value
        End Set
    End Property

    Private _BookFinServiceFee As Double = 0
    Public Property BookFinServiceFee() As Double
        Get
            Return _BookFinServiceFee
        End Get
        Set(ByVal value As Double)
            _BookFinServiceFee = value
        End Set
    End Property

    Private _BookRevLoadSavings As Double = 0
    Public Property BookRevLoadSavings() As Double
        Get
            Return _BookRevLoadSavings
        End Get
        Set(ByVal value As Double)
            _BookRevLoadSavings = value
        End Set
    End Property

    Private _TotalNonFuelFees As Double = 0
    Public Property TotalNonFuelFees() As Double
        Get
            Return _TotalNonFuelFees
        End Get
        Set(ByVal value As Double)
            _TotalNonFuelFees = value
        End Set
    End Property

    Private _BookPickNumber As Integer = 0
    Public Property BookPickNumber() As Integer
        Get
            Return _BookPickNumber
        End Get
        Set(ByVal value As Integer)
            _BookPickNumber = value
        End Set
    End Property

    Private _BookPickupStopNumber As Integer = 0
    Public Property BookPickupStopNumber() As Integer
        Get
            Return _BookPickupStopNumber
        End Get
        Set(ByVal value As Integer)
            _BookPickupStopNumber = value
        End Set
    End Property



End Class

<Serializable()> _
Public Class clsPickListObject60
    Private _PLControl As Int64 = 0
    Public Property PLControl() As Int64
        Get
            Return _PLControl
        End Get
        Set(ByVal value As Int64)
            _PLControl = value
        End Set
    End Property

    Private _PLExportRetry As System.Nullable(Of Integer)
    Friend Property PLExportRetry As System.Nullable(Of Integer)
        Get
            Return _PLExportRetry
        End Get
        Set(value As System.Nullable(Of Integer))
            _PLExportRetry = value
        End Set
    End Property


    Private _PLExportDate As System.Nullable(Of Date)
    Friend Property PLExportDate As System.Nullable(Of Date)
        Get
            Return _PLExportDate
        End Get
        Set(value As System.Nullable(Of Date))
            _PLExportDate = value
        End Set
    End Property

    Private _PLExported As System.Nullable(Of Boolean)
    Friend Property PLExported As System.Nullable(Of Boolean)
        Get
            Return _PLExported
        End Get
        Set(value As System.Nullable(Of Boolean))
            _PLExported = value
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

    Private _BookConsPrefix As String = ""
    Public Property BookConsPrefix() As String
        Get
            Return Left(_BookConsPrefix, 20)
        End Get
        Set(ByVal value As String)
            _BookConsPrefix = Left(value, 20)
        End Set
    End Property

    Private _CarrierNumber As String = ""
    Public Property CarrierNumber() As String
        Get
            Return Left(_CarrierNumber, 50)
        End Get
        Set(ByVal value As String)
            _CarrierNumber = Left(value, 50)
        End Set
    End Property

    Private _BookRevTotalCost As String = ""
    Public Property BookRevTotalCost() As String
        Get
            Return Left(_BookRevTotalCost, 20)
        End Get
        Set(ByVal value As String)
            _BookRevTotalCost = Left(value, 20)
        End Set
    End Property

    Private _LoadOrder As String = ""
    Public Property LoadOrder() As String
        Get
            Return Left(_LoadOrder, 6)
        End Get
        Set(ByVal value As String)
            _LoadOrder = Left(value, 6)
        End Set
    End Property

    Private _BookDateLoad As String = ""
    Public Property BookDateLoad() As String
        Get
            Return Left(_BookDateLoad, 20)
        End Get
        Set(ByVal value As String)
            _BookDateLoad = Left(value, 20)
        End Set
    End Property

    Private _BookDateRequired As String = ""
    Public Property BookDateRequired() As String
        Get
            Return Left(_BookDateRequired, 20)
        End Get
        Set(ByVal value As String)
            _BookDateRequired = Left(value, 20)
        End Set
    End Property

    Private _BookLoadCom As String = ""
    Public Property BookLoadCom() As String
        Get
            Return Left(_BookLoadCom, 1)
        End Get
        Set(ByVal value As String)
            _BookLoadCom = Left(value, 1)
        End Set
    End Property

    Private _BookProNumber As String = ""
    Public Property BookProNumber() As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookRouteFinalCode As String = ""
    Public Property BookRouteFinalCode() As String
        Get
            Return Left(_BookRouteFinalCode, 2)
        End Get
        Set(ByVal value As String)
            _BookRouteFinalCode = Left(value, 2)
        End Set
    End Property

    Private _BookRouteFinalDate As String = ""
    Public Property BookRouteFinalDate() As String
        Get
            Return Left(_BookRouteFinalDate, 20)
        End Get
        Set(ByVal value As String)
            _BookRouteFinalDate = Left(value, 20)
        End Set
    End Property

    Private _BookTotalCases As String = ""
    Public Property BookTotalCases() As String
        Get
            Return Left(_BookTotalCases, 12)
        End Get
        Set(ByVal value As String)
            _BookTotalCases = Left(value, 12)
        End Set
    End Property

    Private _BookTotalWgt As String = ""
    Public Property BookTotalWgt() As String
        Get
            Return Left(_BookTotalWgt, 22)
        End Get
        Set(ByVal value As String)
            _BookTotalWgt = Left(value, 22)
        End Set
    End Property

    Private _BookTotalPL As String = ""
    Public Property BookTotalPL() As String
        Get
            Return Left(_BookTotalPL, 22)
        End Get
        Set(ByVal value As String)
            _BookTotalPL = Left(value, 22)
        End Set
    End Property

    Private _BookTotalCube As String = ""
    Public Property BookTotalCube() As String
        Get
            Return Left(_BookTotalCube, 6)
        End Get
        Set(ByVal value As String)
            _BookTotalCube = Left(value, 6)
        End Set
    End Property

    Private _BookTotalBFC As String = ""
    Public Property BookTotalBFC() As String
        Get
            Return Left(_BookTotalBFC, 20)
        End Get
        Set(ByVal value As String)
            _BookTotalBFC = Left(value, 20)
        End Set
    End Property

    Private _BookStopNo As String = ""
    Public Property BookStopNo() As String
        Get
            Return Left(_BookStopNo, 6)
        End Get
        Set(ByVal value As String)
            _BookStopNo = Left(value, 6)
        End Set
    End Property

    Private _CompName As String = ""
    Public Property CompName() As String
        Get
            Return Left(_CompName, 40)
        End Get
        Set(ByVal value As String)
            _CompName = Left(value, 40)
        End Set
    End Property

    Private _CompNumber As String = ""
    Public Property CompNumber() As String
        Get
            Return Left(_CompNumber, 11)
        End Get
        Set(ByVal value As String)
            _CompNumber = Left(value, 11)
        End Set
    End Property

    Private _BookTypeCode As String = ""
    Public Property BookTypeCode() As String
        Get
            Return Left(_BookTypeCode, 20)
        End Get
        Set(ByVal value As String)
            _BookTypeCode = Left(value, 20)
        End Set
    End Property

    Private _BookDateOrdered As String = ""
    Public Property BookDateOrdered() As String
        Get
            Return Left(_BookDateOrdered, 20)
        End Get
        Set(ByVal value As String)
            _BookDateOrdered = Left(value, 20)
        End Set
    End Property

    Private _BookOrigName As String = ""
    Public Property BookOrigName() As String
        Get
            Return Left(_BookOrigName, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigName = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress1 As String = ""
    Public Property BookOrigAddress1() As String
        Get
            Return Left(_BookOrigAddress1, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress1 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress2 As String = ""
    Public Property BookOrigAddress2() As String
        Get
            Return Left(_BookOrigAddress2, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress2 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress3 As String = ""
    Public Property BookOrigAddress3() As String
        Get
            Return Left(_BookOrigAddress3, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress3 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigCity As String = ""
    Public Property BookOrigCity() As String
        Get
            Return Left(_BookOrigCity, 25)
        End Get
        Set(ByVal value As String)
            _BookOrigCity = Left(value, 25)
        End Set
    End Property

    Private _BookOrigState As String = ""
    Public Property BookOrigState() As String
        Get
            Return Left(_BookOrigState, 8)
        End Get
        Set(ByVal value As String)
            _BookOrigState = Left(value, 8)
        End Set
    End Property

    Private _BookOrigCountry As String = ""
    Public Property BookOrigCountry() As String
        Get
            Return Left(_BookOrigCountry, 30)
        End Get
        Set(ByVal value As String)
            _BookOrigCountry = Left(value, 30)
        End Set
    End Property

    Private _BookOrigZip As String = ""
    Public Property BookOrigZip() As String
        Get
            Return Left(_BookOrigZip, 10)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _BookOrigZip = Left(value, 10)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _BookDestName As String = ""
    Public Property BookDestName() As String
        Get
            Return Left(_BookDestName, 40)
        End Get
        Set(ByVal value As String)
            _BookDestName = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress1 As String = ""
    Public Property BookDestAddress1() As String
        Get
            Return Left(_BookDestAddress1, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress1 = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress2 As String = ""
    Public Property BookDestAddress2() As String
        Get
            Return Left(_BookDestAddress2, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress2 = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress3 As String = ""
    Public Property BookDestAddress3() As String
        Get
            Return Left(_BookDestAddress3, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress3 = Left(value, 40)
        End Set
    End Property

    Private _BookDestCity As String = ""
    Public Property BookDestCity() As String
        Get
            Return Left(_BookDestCity, 25)
        End Get
        Set(ByVal value As String)
            _BookDestCity = Left(value, 25)
        End Set
    End Property

    Private _BookDestState As String = ""
    Public Property BookDestState() As String
        Get
            Return Left(_BookDestState, 2)
        End Get
        Set(ByVal value As String)
            _BookDestState = Left(value, 2)
        End Set
    End Property

    Private _BookDestCountry As String = ""
    Public Property BookDestCountry() As String
        Get
            Return Left(_BookDestCountry, 30)
        End Get
        Set(ByVal value As String)
            _BookDestCountry = Left(value, 30)
        End Set
    End Property

    Private _BookDestZip As String = ""
    Public Property BookDestZip() As String
        Get
            Return Left(_BookDestZip, 10)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _BookDestZip = Left(value, 10) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _BookLoadPONumber As String = ""
    Public Property BookLoadPONumber() As String
        Get
            Return Left(_BookLoadPONumber, 20)
        End Get
        Set(ByVal value As String)
            _BookLoadPONumber = Left(value, 20)
        End Set
    End Property

    Private _CarrierName As String = ""
    Public Property CarrierName() As String
        Get
            Return Left(_CarrierName, 40)
        End Get
        Set(ByVal value As String)
            _CarrierName = Left(value, 40)
        End Set
    End Property

    Private _LaneNumber As String = ""
    Public Property LaneNumber() As String
        Get
            Return Left(_LaneNumber, 50)
        End Get
        Set(ByVal value As String)
            _LaneNumber = Left(value, 50)
        End Set
    End Property

    Private _CommCodeDescription As String = ""
    Public Property CommCodeDescription() As String
        Get
            Return Left(_CommCodeDescription, 40)
        End Get
        Set(ByVal value As String)
            _CommCodeDescription = Left(value, 40)
        End Set
    End Property

    Private _BookMilesFrom As String = ""
    Public Property BookMilesFrom() As String
        Get
            Return Left(_BookMilesFrom, 22)
        End Get
        Set(ByVal value As String)
            _BookMilesFrom = Left(value, 22)
        End Set
    End Property

    Private _BookCommCompControl As Integer = 0
    Public Property BookCommCompControl() As Integer
        Get
            Return _BookCommCompControl
        End Get
        Set(ByVal value As Integer)
            _BookCommCompControl = value
        End Set
    End Property

    Private _BookRevCommCost As Double = 0
    Public Property BookRevCommCost() As Double
        Get
            Return _BookRevCommCost
        End Get
        Set(ByVal value As Double)
            _BookRevCommCost = value
        End Set
    End Property

    Private _BookRevGrossRevenue As Double = 0
    Public Property BookRevGrossRevenue() As Double
        Get
            Return _BookRevGrossRevenue
        End Get
        Set(ByVal value As Double)
            _BookRevGrossRevenue = value
        End Set
    End Property

    Private _BookFinCommStd As Double = 0
    Public Property BookFinCommStd() As Double
        Get
            Return _BookFinCommStd
        End Get
        Set(ByVal value As Double)
            _BookFinCommStd = value
        End Set
    End Property

    Private _BookDoNotInvoice As Boolean = False
    Public Property BookDoNotInvoice() As Boolean
        Get
            Return _BookDoNotInvoice
        End Get
        Set(ByVal value As Boolean)
            _BookDoNotInvoice = value
        End Set
    End Property

    Private _BookOrderSequence As Integer = 0
    Public Property BookOrderSequence() As Integer
        Get
            Return _BookOrderSequence
        End Get
        Set(ByVal value As Integer)
            _BookOrderSequence = value
        End Set
    End Property

    Private _CarrierEquipmentCodes As String = ""
    Public Property CarrierEquipmentCodes() As String
        Get
            Return Left(_CarrierEquipmentCodes, 50)
        End Get
        Set(ByVal value As String)
            _CarrierEquipmentCodes = Left(value, 50)
        End Set
    End Property

    Private _BookCarrierTypeCode As String = ""
    Public Property BookCarrierTypeCode() As String
        Get
            Return Left(_BookCarrierTypeCode, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrierTypeCode = Left(value, 20)
        End Set
    End Property

    Private _BookWarehouseNumber As String = ""
    Public Property BookWarehouseNumber() As String
        Get
            Return Left(_BookWarehouseNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookWarehouseNumber = Left(value, 20)
        End Set
    End Property

    Private _CompNatNumber As Integer = 0
    Public Property CompNatNumber() As Integer
        Get
            Return _CompNatNumber
        End Get
        Set(ByVal value As Integer)
            _CompNatNumber = value
        End Set
    End Property

    Private _BookTransType As String = ""
    Public Property BookTransType() As String
        Get
            Return Left(_BookTransType, 50)
        End Get
        Set(ByVal value As String)
            _BookTransType = Left(value, 50)
        End Set
    End Property

    Private _BookShipCarrierProNumber As String = ""
    Public Property BookShipCarrierProNumber() As String
        Get
            Return Left(_BookShipCarrierProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookShipCarrierNumber As String = ""
    Public Property BookShipCarrierNumber() As String
        Get
            Return Left(_BookShipCarrierNumber, 80)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierNumber = Left(value, 80)
        End Set
    End Property

    Private _LaneComments As String = ""
    Public Property LaneComments() As String
        Get
            Return Left(_LaneComments, 255)
        End Get
        Set(ByVal value As String)
            _LaneComments = Left(value, 255)
        End Set
    End Property

    Private _FuelSurCharge As Double = 0
    Public Property FuelSurCharge() As Double
        Get
            Return _FuelSurCharge
        End Get
        Set(ByVal value As Double)
            _FuelSurCharge = value
        End Set
    End Property

    Private _BookRevCarrierCost As Double = 0
    Public Property BookRevCarrierCost() As Double
        Get
            Return _BookRevCarrierCost
        End Get
        Set(ByVal value As Double)
            _BookRevCarrierCost = value
        End Set
    End Property

    Private _BookRevOtherCost As Double = 0
    Public Property BookRevOtherCost() As Double
        Get
            Return _BookRevOtherCost
        End Get
        Set(ByVal value As Double)
            _BookRevOtherCost = value
        End Set
    End Property

    Private _BookRevNetCost As Double = 0
    Public Property BookRevNetCost() As Double
        Get
            Return _BookRevNetCost
        End Get
        Set(ByVal value As Double)
            _BookRevNetCost = value
        End Set
    End Property

    Private _BookRevFreightTax As Double = 0
    Public Property BookRevFreightTax() As Double
        Get
            Return _BookRevFreightTax
        End Get
        Set(ByVal value As Double)
            _BookRevFreightTax = value
        End Set
    End Property

    Private _BookFinServiceFee As Double = 0
    Public Property BookFinServiceFee() As Double
        Get
            Return _BookFinServiceFee
        End Get
        Set(ByVal value As Double)
            _BookFinServiceFee = value
        End Set
    End Property

    Private _BookRevLoadSavings As Double = 0
    Public Property BookRevLoadSavings() As Double
        Get
            Return _BookRevLoadSavings
        End Get
        Set(ByVal value As Double)
            _BookRevLoadSavings = value
        End Set
    End Property

    Private _TotalNonFuelFees As Double = 0
    Public Property TotalNonFuelFees() As Double
        Get
            Return _TotalNonFuelFees
        End Get
        Set(ByVal value As Double)
            _TotalNonFuelFees = value
        End Set
    End Property

    Private _BookPickNumber As Integer = 0
    Public Property BookPickNumber() As Integer
        Get
            Return _BookPickNumber
        End Get
        Set(ByVal value As Integer)
            _BookPickNumber = value
        End Set
    End Property

    Private _BookPickupStopNumber As Integer = 0
    Public Property BookPickupStopNumber() As Integer
        Get
            Return _BookPickupStopNumber
        End Get
        Set(ByVal value As Integer)
            _BookPickupStopNumber = value
        End Set
    End Property

    Private _BookRouteConsFlag As Boolean = 0
    Public Property BookRouteConsFlag() As Boolean
        Get
            Return _BookRouteConsFlag
        End Get
        Set(ByVal value As Boolean)
            _BookRouteConsFlag = value
        End Set
    End Property

    Private _BookAlternateAddressLaneNumber As String = ""
    Public Property BookAlternateAddressLaneNumber() As String
        Get
            Return Left(_BookAlternateAddressLaneNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookAlternateAddressLaneNumber = Left(value, 50)
        End Set
    End Property



End Class

<Serializable()> _
Public Class clsPickListObject70 : Inherits clsImportDataBase

    Private _PLControl As Int64 = 0
    Public Property PLControl() As Int64
        Get
            Return _PLControl
        End Get
        Set(ByVal value As Int64)
            _PLControl = value
        End Set
    End Property

    Private _PLExportRetry As System.Nullable(Of Integer)
    Friend Property PLExportRetry As System.Nullable(Of Integer)
        Get
            Return _PLExportRetry
        End Get
        Set(value As System.Nullable(Of Integer))
            _PLExportRetry = value
        End Set
    End Property

    Private _PLExportDate As System.Nullable(Of Date)
    Friend Property PLExportDate As System.Nullable(Of Date)
        Get
            Return _PLExportDate
        End Get
        Set(value As System.Nullable(Of Date))
            _PLExportDate = value
        End Set
    End Property

    Private _PLExported As System.Nullable(Of Boolean)
    Friend Property PLExported As System.Nullable(Of Boolean)
        Get
            Return _PLExported
        End Get
        Set(value As System.Nullable(Of Boolean))
            _PLExported = value
        End Set
    End Property

    Private _BookSHID As String = ""
    Public Property BookSHID() As String
        Get
            Return Left(_BookSHID, 50)
        End Get
        Set(ByVal value As String)
            _BookSHID = Left(value, 50)
        End Set
    End Property

    Private _CarrierNumber As Integer = 0
    Public Property CarrierNumber() As Integer
        Get
            Return _CarrierNumber
        End Get
        Set(ByVal value As Integer)
            _CarrierNumber = value
        End Set
    End Property

    Private _CarrierAlphaCode As String = ""
    Public Property CarrierAlphaCode() As String
        Get
            Return Left(_CarrierAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CarrierAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _CarrierLegalEntity As String = ""
    Public Property CarrierLegalEntity As String
        Get
            Return Left(_CarrierLegalEntity, 50)
        End Get
        Set(value As String)
            _CarrierLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _CarrierName As String = ""
    Public Property CarrierName() As String
        Get
            Return Left(_CarrierName, 40)
        End Get
        Set(ByVal value As String)
            _CarrierName = Left(value, 40)
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

    Private _CompNumber As Integer = 0
    Public Property CompNumber() As Integer
        Get
            Return _CompNumber
        End Get
        Set(ByVal value As Integer)
            _CompNumber = value
        End Set
    End Property

    Private _CompName As String = ""
    Public Property CompName() As String
        Get
            Return Left(_CompName, 40)
        End Get
        Set(ByVal value As String)
            _CompName = Left(value, 40)
        End Set
    End Property

    Private _CompAlphaCode As String = ""
    Public Property CompAlphaCode() As String
        Get
            Return Left(_CompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _CompNatNumber As Integer = 0
    Public Property CompNatNumber() As Integer
        Get
            Return _CompNatNumber
        End Get
        Set(ByVal value As Integer)
            _CompNatNumber = value
        End Set
    End Property

    Private _LaneLegalEntity As String = ""
    Public Property LaneLegalEntity As String
        Get
            Return Left(_LaneLegalEntity, 50)
        End Get
        Set(value As String)
            _LaneLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _LaneNumber As String = ""
    Public Property LaneNumber As String
        Get
            Return Left(_LaneNumber, 50)
        End Get
        Set(value As String)
            _LaneNumber = Left(value, 50)
        End Set
    End Property

    Private _BookOriginalLaneNumber As String
    Public Property BookOriginalLaneNumber() As String
        Get
            Return Left(_BookOriginalLaneNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookOriginalLaneNumber = Left(value, 50)
        End Set
    End Property

    Private _BookOriginalLaneLegalEntity As String = ""
    Public Property BookOriginalLaneLegalEntity As String
        Get
            Return Left(_BookOriginalLaneLegalEntity, 50)
        End Get
        Set(value As String)
            _BookOriginalLaneLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _BookAlternateAddressLaneNumber As String = ""
    Public Property BookAlternateAddressLaneNumber() As String
        Get
            Return Left(_BookAlternateAddressLaneNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookAlternateAddressLaneNumber = Left(value, 50)
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

    Private _BookOrderSequence As Integer = 0
    Public Property BookOrderSequence() As Integer
        Get
            Return _BookOrderSequence
        End Get
        Set(ByVal value As Integer)
            _BookOrderSequence = value
        End Set
    End Property

    Private _BookConsPrefix As String = ""
    Public Property BookConsPrefix() As String
        Get
            Return Left(_BookConsPrefix, 20)
        End Get
        Set(ByVal value As String)
            _BookConsPrefix = Left(value, 20)
        End Set
    End Property

    Private _BookRouteConsFlag As Boolean = 0
    Public Property BookRouteConsFlag() As Boolean
        Get
            Return _BookRouteConsFlag
        End Get
        Set(ByVal value As Boolean)
            _BookRouteConsFlag = value
        End Set
    End Property

    Private _LoadOrder As String = ""
    Public Property LoadOrder() As String
        Get
            Return Left(_LoadOrder, 6)
        End Get
        Set(ByVal value As String)
            _LoadOrder = Left(value, 6)
        End Set
    End Property

    Private _BookDateLoad As String = ""
    Public Property BookDateLoad() As String
        Get
            Return Left(_BookDateLoad, 20)
        End Get
        Set(ByVal value As String)
            _BookDateLoad = Left(value, 20)
        End Set
    End Property

    Private _BookDateRequired As String = ""
    Public Property BookDateRequired() As String
        Get
            Return Left(_BookDateRequired, 20)
        End Get
        Set(ByVal value As String)
            _BookDateRequired = Left(value, 20)
        End Set
    End Property

    Private _BookLoadCom As String = ""
    Public Property BookLoadCom() As String
        Get
            Return Left(_BookLoadCom, 1)
        End Get
        Set(ByVal value As String)
            _BookLoadCom = Left(value, 1)
        End Set
    End Property

    Private _BookProNumber As String = ""
    Public Property BookProNumber() As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookRouteFinalCode As String = ""
    Public Property BookRouteFinalCode() As String
        Get
            Return Left(_BookRouteFinalCode, 2)
        End Get
        Set(ByVal value As String)
            _BookRouteFinalCode = Left(value, 2)
        End Set
    End Property

    Private _BookRouteFinalDate As String = ""
    Public Property BookRouteFinalDate() As String
        Get
            Return Left(_BookRouteFinalDate, 20)
        End Get
        Set(ByVal value As String)
            _BookRouteFinalDate = Left(value, 20)
        End Set
    End Property

    Private _BookTotalCases As String = ""
    Public Property BookTotalCases() As String
        Get
            Return Left(_BookTotalCases, 12)
        End Get
        Set(ByVal value As String)
            _BookTotalCases = Left(value, 12)
        End Set
    End Property

    Private _BookTotalWgt As String = ""
    Public Property BookTotalWgt() As String
        Get
            Return Left(_BookTotalWgt, 22)
        End Get
        Set(ByVal value As String)
            _BookTotalWgt = Left(value, 22)
        End Set
    End Property

    Private _BookTotalPL As String = ""
    Public Property BookTotalPL() As String
        Get
            Return Left(_BookTotalPL, 22)
        End Get
        Set(ByVal value As String)
            _BookTotalPL = Left(value, 22)
        End Set
    End Property

    Private _BookTotalCube As String = ""
    Public Property BookTotalCube() As String
        Get
            Return Left(_BookTotalCube, 6)
        End Get
        Set(ByVal value As String)
            _BookTotalCube = Left(value, 6)
        End Set
    End Property

    Private _BookStopNo As String = ""
    Public Property BookStopNo() As String
        Get
            Return Left(_BookStopNo, 6)
        End Get
        Set(ByVal value As String)
            _BookStopNo = Left(value, 6)
        End Set
    End Property

    Private _BookTypeCode As String = ""
    Public Property BookTypeCode() As String
        Get
            Return Left(_BookTypeCode, 20)
        End Get
        Set(ByVal value As String)
            _BookTypeCode = Left(value, 20)
        End Set
    End Property

    Private _BookDateOrdered As String = ""
    Public Property BookDateOrdered() As String
        Get
            Return Left(_BookDateOrdered, 20)
        End Get
        Set(ByVal value As String)
            _BookDateOrdered = Left(value, 20)
        End Set
    End Property

    Private _BookOrigName As String = ""
    Public Property BookOrigName() As String
        Get
            Return Left(_BookOrigName, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigName = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress1 As String = ""
    Public Property BookOrigAddress1() As String
        Get
            Return Left(_BookOrigAddress1, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress1 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress2 As String = ""
    Public Property BookOrigAddress2() As String
        Get
            Return Left(_BookOrigAddress2, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress2 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigAddress3 As String = ""
    Public Property BookOrigAddress3() As String
        Get
            Return Left(_BookOrigAddress3, 40)
        End Get
        Set(ByVal value As String)
            _BookOrigAddress3 = Left(value, 40)
        End Set
    End Property

    Private _BookOrigCity As String = ""
    Public Property BookOrigCity() As String
        Get
            Return Left(_BookOrigCity, 25)
        End Get
        Set(ByVal value As String)
            _BookOrigCity = Left(value, 25)
        End Set
    End Property

    Private _BookOrigState As String = ""
    Public Property BookOrigState() As String
        Get
            Return Left(_BookOrigState, 8)
        End Get
        Set(ByVal value As String)
            _BookOrigState = Left(value, 8)
        End Set
    End Property

    Private _BookOrigCountry As String = ""
    Public Property BookOrigCountry() As String
        Get
            Return Left(_BookOrigCountry, 30)
        End Get
        Set(ByVal value As String)
            _BookOrigCountry = Left(value, 30)
        End Set
    End Property

    Private _BookOrigZip As String = ""
    Public Property BookOrigZip() As String
        Get
            Return Left(_BookOrigZip, 10)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _BookOrigZip = Left(value, 10) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _BookDestName As String = ""
    Public Property BookDestName() As String
        Get
            Return Left(_BookDestName, 40)
        End Get
        Set(ByVal value As String)
            _BookDestName = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress1 As String = ""
    Public Property BookDestAddress1() As String
        Get
            Return Left(_BookDestAddress1, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress1 = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress2 As String = ""
    Public Property BookDestAddress2() As String
        Get
            Return Left(_BookDestAddress2, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress2 = Left(value, 40)
        End Set
    End Property

    Private _BookDestAddress3 As String = ""
    Public Property BookDestAddress3() As String
        Get
            Return Left(_BookDestAddress3, 40)
        End Get
        Set(ByVal value As String)
            _BookDestAddress3 = Left(value, 40)
        End Set
    End Property

    Private _BookDestCity As String = ""
    Public Property BookDestCity() As String
        Get
            Return Left(_BookDestCity, 25)
        End Get
        Set(ByVal value As String)
            _BookDestCity = Left(value, 25)
        End Set
    End Property

    Private _BookDestState As String = ""
    Public Property BookDestState() As String
        Get
            Return Left(_BookDestState, 2)
        End Get
        Set(ByVal value As String)
            _BookDestState = Left(value, 2)
        End Set
    End Property

    Private _BookDestCountry As String = ""
    Public Property BookDestCountry() As String
        Get
            Return Left(_BookDestCountry, 30)
        End Get
        Set(ByVal value As String)
            _BookDestCountry = Left(value, 30)
        End Set
    End Property

    Private _BookDestZip As String = ""
    Public Property BookDestZip() As String
        Get
            Return Left(_BookDestZip, 10)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _BookDestZip = Left(value, 10) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _BookLoadPONumber As String = ""
    Public Property BookLoadPONumber() As String
        Get
            Return Left(_BookLoadPONumber, 20)
        End Get
        Set(ByVal value As String)
            _BookLoadPONumber = Left(value, 20)
        End Set
    End Property

    Private _CommCodeDescription As String = ""
    Public Property CommCodeDescription() As String
        Get
            Return Left(_CommCodeDescription, 40)
        End Get
        Set(ByVal value As String)
            _CommCodeDescription = Left(value, 40)
        End Set
    End Property

    Private _BookMilesFrom As String = ""
    Public Property BookMilesFrom() As String
        Get
            Return Left(_BookMilesFrom, 22)
        End Get
        Set(ByVal value As String)
            _BookMilesFrom = Left(value, 22)
        End Set
    End Property

    Private _BookCommCompControl As Integer = 0
    Public Property BookCommCompControl() As Integer
        Get
            Return _BookCommCompControl
        End Get
        Set(ByVal value As Integer)
            _BookCommCompControl = value
        End Set
    End Property

    Private _BookFinCommStd As Double = 0
    Public Property BookFinCommStd() As Double
        Get
            Return _BookFinCommStd
        End Get
        Set(ByVal value As Double)
            _BookFinCommStd = value
        End Set
    End Property

    Private _BookDoNotInvoice As Boolean = False
    Public Property BookDoNotInvoice() As Boolean
        Get
            Return _BookDoNotInvoice
        End Get
        Set(ByVal value As Boolean)
            _BookDoNotInvoice = value
        End Set
    End Property

    Private _CarrierEquipmentCodes As String = ""
    Public Property CarrierEquipmentCodes() As String
        Get
            Return Left(_CarrierEquipmentCodes, 50)
        End Get
        Set(ByVal value As String)
            _CarrierEquipmentCodes = Left(value, 50)
        End Set
    End Property

    Private _BookCarrierTypeCode As String = ""
    Public Property BookCarrierTypeCode() As String
        Get
            Return Left(_BookCarrierTypeCode, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrierTypeCode = Left(value, 20)
        End Set
    End Property

    Private _BookWarehouseNumber As String = ""
    Public Property BookWarehouseNumber() As String
        Get
            Return Left(_BookWarehouseNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookWarehouseNumber = Left(value, 20)
        End Set
    End Property

    Private _BookWhseAuthorizationNo As String = ""
    Public Property BookWhseAuthorizationNo() As String
        Get
            Return Left(_BookWhseAuthorizationNo, 20)
        End Get
        Set(ByVal value As String)
            _BookWhseAuthorizationNo = Left(value, 20)
        End Set
    End Property

    Private _BookTransType As String = ""
    Public Property BookTransType() As String
        Get
            Return Left(_BookTransType, 50)
        End Get
        Set(ByVal value As String)
            _BookTransType = Left(value, 50)
        End Set
    End Property

    Private _BookShipCarrierProNumber As String = ""
    Public Property BookShipCarrierProNumber() As String
        Get
            Return Left(_BookShipCarrierProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookShipCarrierNumber As String = ""
    Public Property BookShipCarrierNumber() As String
        Get
            Return Left(_BookShipCarrierNumber, 80)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierNumber = Left(value, 80)
        End Set
    End Property

    Private _BookShipCarrierName As String
    Public Property BookShipCarrierName() As String
        Get
            Return Left(_BookShipCarrierName, 60)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierName = Left(value, 60)
        End Set
    End Property

    Private _BookShipCarrierDetails As String
    Public Property BookShipCarrierDetails() As String
        Get
            Return Left(_BookShipCarrierDetails, 4000)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierDetails = Left(value, 4000)
        End Set
    End Property

    Private _LaneComments As String = ""
    Public Property LaneComments() As String
        Get
            Return Left(_LaneComments, 255)
        End Get
        Set(ByVal value As String)
            _LaneComments = Left(value, 255)
        End Set
    End Property

    Private _FuelSurCharge As Double = 0
    Public Property FuelSurCharge() As Double
        Get
            Return _FuelSurCharge
        End Get
        Set(ByVal value As Double)
            _FuelSurCharge = value
        End Set
    End Property



    Private _BookTotalBFC As String = ""
    Public Property BookTotalBFC() As String
        Get
            Return Left(_BookTotalBFC, 20)
        End Get
        Set(ByVal value As String)
            _BookTotalBFC = Left(value, 20)
        End Set
    End Property

    Private _BookRevTotalCost As String = ""
    Public Property BookRevTotalCost() As String
        Get
            Return Left(_BookRevTotalCost, 20)
        End Get
        Set(ByVal value As String)
            _BookRevTotalCost = Left(value, 20)
        End Set
    End Property

    Private _BookRevCommCost As Double = 0
    Public Property BookRevCommCost() As Double
        Get
            Return _BookRevCommCost
        End Get
        Set(ByVal value As Double)
            _BookRevCommCost = value
        End Set
    End Property

    Private _BookRevGrossRevenue As Double = 0
    Public Property BookRevGrossRevenue() As Double
        Get
            Return _BookRevGrossRevenue
        End Get
        Set(ByVal value As Double)
            _BookRevGrossRevenue = value
        End Set
    End Property

    Private _BookRevCarrierCost As Double = 0
    Public Property BookRevCarrierCost() As Double
        Get
            Return _BookRevCarrierCost
        End Get
        Set(ByVal value As Double)
            _BookRevCarrierCost = value
        End Set
    End Property

    Private _BookRevOtherCost As Double = 0
    Public Property BookRevOtherCost() As Double
        Get
            Return _BookRevOtherCost
        End Get
        Set(ByVal value As Double)
            _BookRevOtherCost = value
        End Set
    End Property

    Private _BookRevNetCost As Double = 0
    Public Property BookRevNetCost() As Double
        Get
            Return _BookRevNetCost
        End Get
        Set(ByVal value As Double)
            _BookRevNetCost = value
        End Set
    End Property

    Private _BookRevNonTaxable As Double = 0
    Public Property BookRevNonTaxable() As Double
        Get
            Return _BookRevNonTaxable
        End Get
        Set(ByVal value As Double)
            _BookRevNonTaxable = value
        End Set
    End Property

    Private _BookRevFreightTax As Double = 0
    Public Property BookRevFreightTax() As Double
        Get
            Return _BookRevFreightTax
        End Get
        Set(ByVal value As Double)
            _BookRevFreightTax = value
        End Set
    End Property

    Private _BookFinServiceFee As Double = 0
    Public Property BookFinServiceFee() As Double
        Get
            Return _BookFinServiceFee
        End Get
        Set(ByVal value As Double)
            _BookFinServiceFee = value
        End Set
    End Property

    Private _BookRevLoadSavings As Double = 0
    Public Property BookRevLoadSavings() As Double
        Get
            Return _BookRevLoadSavings
        End Get
        Set(ByVal value As Double)
            _BookRevLoadSavings = value
        End Set
    End Property

    Private _TotalNonFuelFees As Double = 0
    Public Property TotalNonFuelFees() As Double
        Get
            Return _TotalNonFuelFees
        End Get
        Set(ByVal value As Double)
            _TotalNonFuelFees = value
        End Set
    End Property

    Private _BookPickNumber As Integer = 0
    Public Property BookPickNumber() As Integer
        Get
            Return _BookPickNumber
        End Get
        Set(ByVal value As Integer)
            _BookPickNumber = value
        End Set
    End Property

    Private _BookPickupStopNumber As Integer = 0
    Public Property BookPickupStopNumber() As Integer
        Get
            Return _BookPickupStopNumber
        End Get
        Set(ByVal value As Integer)
            _BookPickupStopNumber = value
        End Set
    End Property

    Private _BookFinAPGLNumber As String = ""
    Public Property BookFinAPGLNumber() As String
        Get
            Return Left(_BookFinAPGLNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookFinAPGLNumber = Left(value, 50)
        End Set
    End Property



End Class

''' <summary>
''' Pick Worksheet Header Data for Web Services
''' </summary>
''' <remarks>
''' Created by RHR v-8.2.0.117 7/17/2019
'''     there are currently no changes to the header object only the item details
'''     for consistency we create matching object names with the 80 tag
''' </remarks>
<Serializable()>
Public Class clsPickListObject80 : Inherits clsPickListObject70


    Private _BookOrigZip As String = ""
    Public Overloads Property BookOrigZip() As String
        Get
            Return Left(_BookOrigZip, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _BookOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _BookDestZip As String = ""
    Public Overloads Property BookDestZip() As String
        Get
            Return Left(_BookDestZip, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _BookDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

End Class

''' <summary>
''' Pick Worksheet Header Data for Web Services
''' </summary>
''' <remarks>
''' Created by by RHR for v-8.5.0.002 on 12/03/2021 added Scheduler Fields
''' </remarks>
<Serializable()>
Public Class clsPickListObject85 : Inherits clsPickListObject80

    Private _BookCarrTrailerNo As String
    Public Property BookCarrTrailerNo() As String
        Get
            Return Left(_BookCarrTrailerNo, 50)
        End Get
        Set(ByVal value As String)
            _BookCarrTrailerNo = Left(value, 50)
        End Set
    End Property

    Private _BookCarrSealNo As String
    Public Property BookCarrSealNo() As String
        Get
            Return Left(_BookCarrSealNo, 50)
        End Get
        Set(ByVal value As String)
            _BookCarrSealNo = Left(value, 50)
        End Set
    End Property

    Private _BookCarrDriverNo As String
    Public Property BookCarrDriverNo() As String
        Get
            Return Left(_BookCarrDriverNo, 50)
        End Get
        Set(ByVal value As String)
            _BookCarrDriverNo = Left(value, 50)
        End Set
    End Property

    Private _BookCarrDriverName As String
    Public Property BookCarrDriverName() As String
        Get
            Return Left(_BookCarrDriverName, 50)
        End Get
        Set(ByVal value As String)
            _BookCarrDriverName = Left(value, 50)
        End Set
    End Property

    Private _BookCarrRouteNo As String
    Public Property BookCarrRouteNo() As String
        Get
            Return Left(_BookCarrRouteNo, 50)
        End Get
        Set(ByVal value As String)
            _BookCarrRouteNo = Left(value, 50)
        End Set
    End Property


    Private _BookCarrTripNo As String
    Public Property BookCarrTripNo() As String
        Get
            Return Left(_BookCarrTripNo, 50)
        End Get
        Set(ByVal value As String)
            _BookCarrTripNo = Left(value, 50)
        End Set
    End Property

    Private _BookCarrApptDate As System.Nullable(Of Date)

    Public Property BookCarrApptDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrApptDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrApptDate = value
        End Set
    End Property


    Private _BookCarrApptTime As System.Nullable(Of Date)

    Public Property BookCarrApptTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrApptTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrApptTime = value
        End Set
    End Property

    Private _BookCarrActDate As System.Nullable(Of Date)

    Public Property BookCarrActDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrActDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrActDate = value
        End Set
    End Property

    Private _BookCarrActTime As System.Nullable(Of Date)

    Public Property BookCarrActTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrActTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrActTime = value
        End Set
    End Property

    Private _BookCarrStartUnloadingDate As System.Nullable(Of Date)

    Public Property BookCarrStartUnloadingDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrStartUnloadingDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrStartUnloadingDate = value
        End Set
    End Property

    Private _BookCarrStartUnloadingTime As System.Nullable(Of Date)

    Public Property BookCarrStartUnloadingTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrStartUnloadingTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrStartUnloadingTime = value
        End Set
    End Property

    Private _BookCarrFinishUnloadingDate As System.Nullable(Of Date)

    Public Property BookCarrFinishUnloadingDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrFinishUnloadingDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrFinishUnloadingDate = value
        End Set
    End Property

    Private _BookCarrFinishUnloadingTime As System.Nullable(Of Date)

    Public Property BookCarrFinishUnloadingTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrFinishUnloadingTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrFinishUnloadingTime = value
        End Set
    End Property


    Private _BookCarrActUnloadCompDate As System.Nullable(Of Date)

    Public Property BookCarrActUnloadCompDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrActUnloadCompDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrActUnloadCompDate = value
        End Set
    End Property

    Private _BookCarrActUnloadCompTime As System.Nullable(Of Date)

    Public Property BookCarrActUnloadCompTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrActUnloadCompTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrActUnloadCompTime = value
        End Set
    End Property

    Private _BookCarrScheduleDate As System.Nullable(Of Date)

    Public Property BookCarrScheduleDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrScheduleDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrScheduleDate = value
        End Set
    End Property

    Private _BookCarrScheduleTime As System.Nullable(Of Date)

    Public Property BookCarrScheduleTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrScheduleTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrScheduleTime = value
        End Set
    End Property

    Private _BookCarrActualDate As System.Nullable(Of Date)

    Public Property BookCarrActualDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrActualDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrActualDate = value
        End Set
    End Property

    Private _BookCarrActualTime As System.Nullable(Of Date)

    Public Property BookCarrActualTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrActualTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrActualTime = value
        End Set
    End Property

    Private _BookCarrStartLoadingDate As System.Nullable(Of Date)

    Public Property BookCarrStartLoadingDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrStartLoadingDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrStartLoadingDate = value
        End Set
    End Property

    Private _BookCarrStartLoadingTime As System.Nullable(Of Date)

    Public Property BookCarrStartLoadingTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrStartLoadingTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrStartLoadingTime = value
        End Set
    End Property

    Private _BookCarrFinishLoadingDate As System.Nullable(Of Date)

    Public Property BookCarrFinishLoadingDate() As System.Nullable(Of Date)
        Get
            Return _BookCarrFinishLoadingDate
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrFinishLoadingDate = value
        End Set
    End Property

    Private _BookCarrFinishLoadingTime As System.Nullable(Of Date)

    Public Property BookCarrFinishLoadingTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrFinishLoadingTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrFinishLoadingTime = value
        End Set
    End Property

    Private _BookCarrActLoadComplete_Date As System.Nullable(Of Date)

    Public Property BookCarrActLoadComplete_Date() As System.Nullable(Of Date)
        Get
            Return _BookCarrActLoadComplete_Date
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrActLoadComplete_Date = value
        End Set
    End Property

    Private _BookCarrActLoadCompleteTime As System.Nullable(Of Date)

    Public Property BookCarrActLoadCompleteTime() As System.Nullable(Of Date)
        Get
            Return _BookCarrActLoadCompleteTime
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _BookCarrActLoadCompleteTime = value
        End Set
    End Property


End Class
