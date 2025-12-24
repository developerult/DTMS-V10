<Serializable()> _
Public Class clsAPExportObject
    
    Public APControl As Integer = 0 'The APControl number is new in v-4.8
    Public CarrierNumber As Integer = 0
    Public BookFinAPBillNumber As String = ""
    Public BookFinAPBillInvDate As String = ""
    Public BookCarrOrderNumber As String = ""
    Public LaneNumber As String = ""
    Public BookItemCostCenterNumber As String = ""
    Public BookFinAPACtCost As Double = 0
    Public BookCarrBLNumber As String = ""
    Public BookFinAPActWgt As Integer = 0
    Public BookFinAPBillNoDate As String = ""
    Public BookFinAPActTax As Double = 0
    Public BookProNumber As String = ""
    Public BookFinAPExportRetry As Integer = 0
    Public BookFinAPExportDate As String = ""
    Public PrevSentDate As String = ""
    Public CompanyNumber As String = ""
    'New fields added in v-4.8
    Public BookOrderSequence As Integer = 0
    Public CarrierEquipmentCodes As String = ""
    Public BookCarrierTypeCode As String = ""
    Public APFee1 As Double = 0
    Public APFee2 As Double = 0
    Public APFee3 As Double = 0
    Public APFee4 As Double = 0
    Public APFee5 As Double = 0
    Public APFee6 As Double = 0
    Public OtherCosts As Double = 0
    Public BookWarehouseNumber As String = ""
    Public BookMilesFrom As Double = 0
    Public CompNatNumber As Integer = 0
    Public BookReasonCode As String = ""
    Public BookTransType As Integer = 0
    Public BookShipCarrierProNumber As String = ""
    Public BookShipCarrierNumber As String = ""
    Public APTaxDetail1 As Double = 0
    Public APTaxDetail2 As Double = 0
    Public APTaxDetail3 As Double = 0
    Public APTaxDetail4 As Double = 0
    Public APTaxDetail5 As Double = 0


End Class

<Serializable()> _
Public Class clsAPExportObject70 : Inherits clsImportDataBase

    Private _APControl As Integer = 0
    Public Property APControl() As Integer
        Get
            Return _APControl
        End Get
        Set(ByVal value As Integer)
            _APControl = value
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

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return Left(_CompLegalEntity, 50)
        End Get
        Set(value As String)
            _CompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _CompanyNumber As Integer = 0
    Public Property CompanyNumber() As Integer
        Get
            Return _CompanyNumber
        End Get
        Set(ByVal value As Integer)
            _CompanyNumber = value
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

    Private _BookProNumber As String = ""
    Public Property BookProNumber() As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookShipCarrierProNumber As String
    Public Property BookShipCarrierProNumber() As String
        Get
            Return Left(_BookShipCarrierProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookShipCarrierNumber As String
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

    Private _BookFinAPBillNumber As String = ""
    Public Property BookFinAPBillNumber() As String
        Get
            Return Left(_BookFinAPBillNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookFinAPBillNumber = Left(value, 50)
        End Set
    End Property

    Private _BookFinAPBillNoDate As String
    Public Property BookFinAPBillNoDate() As String
        Get
            Return _BookFinAPBillNoDate
        End Get
        Set(ByVal value As String)
            _BookFinAPBillNoDate = value
        End Set
    End Property

    Private _BookFinAPBillInvDate As String
    Public Property BookFinAPBillInvDate() As String
        Get
            Return _BookFinAPBillInvDate
        End Get
        Set(ByVal value As String)
            _BookFinAPBillInvDate = value
        End Set
    End Property

    Private _BookFinAPActWgt As Integer = 0
    Public Property BookFinAPActWgt() As Integer
        Get
            Return _BookFinAPActWgt
        End Get
        Set(ByVal value As Integer)
            _BookFinAPActWgt = value
        End Set
    End Property

    Private _BookFinAPStdCost As Double
    Public Property BookFinAPStdCost() As Double
        Get
            Return _BookFinAPStdCost
        End Get
        Set(ByVal value As Double)
            _BookFinAPStdCost = value
        End Set
    End Property

    Private _BookFinAPACtCost As Double = 0
    Public Property BookFinAPACtCost() As Double
        Get
            Return _BookFinAPACtCost
        End Get
        Set(ByVal value As Double)
            _BookFinAPACtCost = value
        End Set
    End Property

    Private _BookFinAPActTax As Double = 0
    Public Property BookFinAPActTax() As Double
        Get
            Return _BookFinAPActTax
        End Get
        Set(ByVal value As Double)
            _BookFinAPActTax = value
        End Set
    End Property

    Private _BookFinAPTotalTaxableFees As Double = 0
    Public Property BookFinAPTotalTaxableFees() As Double
        Get
            Return _BookFinAPTotalTaxableFees
        End Get
        Set(ByVal value As Double)
            _BookFinAPTotalTaxableFees = value
        End Set
    End Property

    Private _BookFinAPTotalTaxes As Double = 0
    Public Property BookFinAPTotalTaxes() As Double
        Get
            Return _BookFinAPTotalTaxes
        End Get
        Set(ByVal value As Double)
            _BookFinAPTotalTaxes = value
        End Set
    End Property

    Private _BookFinAPNonTaxableFees As Double = 0
    Public Property BookFinAPNonTaxableFees() As Double
        Get
            Return _BookFinAPNonTaxableFees
        End Get
        Set(ByVal value As Double)
            _BookFinAPNonTaxableFees = value
        End Set
    End Property

    Private _BookCarrBLNumber As String = ""
    Public Property BookCarrBLNumber() As String
        Get
            Return Left(_BookCarrBLNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrBLNumber = Left(value, 20)
        End Set
    End Property

    Private _BookFinAPExportRetry As Integer = 0
    Public Property BookFinAPExportRetry() As Integer
        Get
            Return _BookFinAPExportRetry
        End Get
        Set(ByVal value As Integer)
            _BookFinAPExportRetry = value
        End Set
    End Property

    Private _BookItemCostCenterNumber As String = ""
    Public Property BookItemCostCenterNumber() As String
        Get
            Return Left(_BookItemCostCenterNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookItemCostCenterNumber = Left(value, 50)
        End Set
    End Property

    Private _BookFinAPExportDate As String = ""
    Public Property BookFinAPExportDate() As String
        Get
            Return _BookFinAPExportDate
        End Get
        Set(ByVal value As String)
            _BookFinAPExportDate = value
        End Set
    End Property

    Private _PrevSentDate As String = ""
    Public Property PrevSentDate() As String
        Get
            Return _PrevSentDate
        End Get
        Set(ByVal value As String)
            _PrevSentDate = value
        End Set
    End Property

    Private _CarrierEquipmentCodes As String = ""
    Public Property CarrierEquipmentCodes() As String
        Get
            Return _CarrierEquipmentCodes
        End Get
        Set(ByVal value As String)
            _CarrierEquipmentCodes = value
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

    Private _APFee1 As Double = 0
    Public Property APFee1() As Double
        Get
            Return _APFee1
        End Get
        Set(ByVal value As Double)
            _APFee1 = value
        End Set
    End Property

    Private _APFee2 As Double = 0
    Public Property APFee2() As Double
        Get
            Return _APFee2
        End Get
        Set(ByVal value As Double)
            _APFee2 = value
        End Set
    End Property

    Private _APFee3 As Double = 0
    Public Property APFee3() As Double
        Get
            Return _APFee3
        End Get
        Set(ByVal value As Double)
            _APFee3 = value
        End Set
    End Property

    Private _APFee4 As Double = 0
    Public Property APFee4() As Double
        Get
            Return _APFee4
        End Get
        Set(ByVal value As Double)
            _APFee4 = value
        End Set
    End Property

    Private _APFee5 As Double = 0
    Public Property APFee5() As Double
        Get
            Return _APFee5
        End Get
        Set(ByVal value As Double)
            _APFee5 = value
        End Set
    End Property

    Private _APFee6 As Double = 0
    Public Property APFee6() As Double
        Get
            Return _APFee6
        End Get
        Set(ByVal value As Double)
            _APFee6 = value
        End Set
    End Property

    Private _OtherCosts As Double = 0
    Public Property OtherCosts() As Double
        Get
            Return _OtherCosts
        End Get
        Set(ByVal value As Double)
            _OtherCosts = value
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

    Private _BookMilesFrom As Double = 0
    Public Property BookMilesFrom() As Double
        Get
            Return _BookMilesFrom
        End Get
        Set(ByVal value As Double)
            _BookMilesFrom = value
        End Set
    End Property

    Private _BookReasonCode As String = ""
    Public Property BookReasonCode() As String
        Get
            Return _BookReasonCode
        End Get
        Set(ByVal value As String)
            _BookReasonCode = value
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

    Private _APTaxDetail1 As Double = 0
    Public Property APTaxDetail1() As Double
        Get
            Return _APTaxDetail1
        End Get
        Set(ByVal value As Double)
            _APTaxDetail1 = value
        End Set
    End Property

    Private _APTaxDetail2 As Double = 0
    Public Property APTaxDetail2() As Double
        Get
            Return _APTaxDetail2
        End Get
        Set(ByVal value As Double)
            _APTaxDetail2 = value
        End Set
    End Property

    Private _APTaxDetail3 As Double = 0
    Public Property APTaxDetail3() As Double
        Get
            Return _APTaxDetail3
        End Get
        Set(ByVal value As Double)
            _APTaxDetail3 = value
        End Set
    End Property

    Private _APTaxDetail4 As Double = 0
    Public Property APTaxDetail4() As Double
        Get
            Return _APTaxDetail4
        End Get
        Set(ByVal value As Double)
            _APTaxDetail4 = value
        End Set
    End Property

    Private _APTaxDetail5 As Double = 0
    Public Property APTaxDetail5() As Double
        Get
            Return _APTaxDetail5
        End Get
        Set(ByVal value As Double)
            _APTaxDetail5 = value
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
''' AP Export Header
''' </summary>
''' <remarks>
''' Created by RHR v-8.2.0.117 7/17/2019
'''     there are currently no changes to the header object only the item details
'''     for consistency we create matching object names with the 80 tag
''' Modifief by RHR for v-8.2.1.006 on 04/13/2020
'''     Added new fields for new Freight Bill Reduction logic
''' </remarks>
<Serializable()>
Public Class clsAPExportObject80 : Inherits clsAPExportObject70

    Private _APReduction As Double
    Public Property APReduction() As Double
        Get
            Return _APReduction
        End Get
        Set(ByVal value As Double)
            _APReduction = value
        End Set
    End Property

    Private _APReductionReason As Integer
    Public Property APReductionReason() As Integer
        Get
            Return _APReductionReason
        End Get
        Set(ByVal value As Integer)
            _APReductionReason = value
        End Set
    End Property

    Private _APReductionAdjustedCost As Double
    Public Property APReductionAdjustedCost() As Double
        Get
            Return _APReductionAdjustedCost
        End Get
        Set(ByVal value As Double)
            _APReductionAdjustedCost = value
        End Set
    End Property

End Class


''' <summary>
''' AP Export Header
''' </summary>
''' <remarks>
''' Created by RHR v-8.5.1.001 3/21/2022
'''     removed inherits from 70 so all fields are visible in the WSDL
''' </remarks>
<Serializable()>
Public Class clsAPExportObject85 : Inherits clsImportDataBase

    Private _APControl As Integer = 0
    Public Property APControl() As Integer
        Get
            Return _APControl
        End Get
        Set(ByVal value As Integer)
            _APControl = value
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

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return Left(_CompLegalEntity, 50)
        End Get
        Set(value As String)
            _CompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _CompanyNumber As Integer = 0
    Public Property CompanyNumber() As Integer
        Get
            Return _CompanyNumber
        End Get
        Set(ByVal value As Integer)
            _CompanyNumber = value
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

    Private _BookProNumber As String = ""
    Public Property BookProNumber() As String
        Get
            Return Left(_BookProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookShipCarrierProNumber As String
    Public Property BookShipCarrierProNumber() As String
        Get
            Return Left(_BookShipCarrierProNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookShipCarrierProNumber = Left(value, 20)
        End Set
    End Property

    Private _BookShipCarrierNumber As String
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

    Private _BookFinAPBillNumber As String = ""
    Public Property BookFinAPBillNumber() As String
        Get
            Return Left(_BookFinAPBillNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookFinAPBillNumber = Left(value, 50)
        End Set
    End Property

    Private _BookFinAPBillNoDate As String
    Public Property BookFinAPBillNoDate() As String
        Get
            Return _BookFinAPBillNoDate
        End Get
        Set(ByVal value As String)
            _BookFinAPBillNoDate = value
        End Set
    End Property

    Private _BookFinAPBillInvDate As String
    Public Property BookFinAPBillInvDate() As String
        Get
            Return _BookFinAPBillInvDate
        End Get
        Set(ByVal value As String)
            _BookFinAPBillInvDate = value
        End Set
    End Property

    Private _BookFinAPActWgt As Integer = 0
    Public Property BookFinAPActWgt() As Integer
        Get
            Return _BookFinAPActWgt
        End Get
        Set(ByVal value As Integer)
            _BookFinAPActWgt = value
        End Set
    End Property

    Private _BookFinAPStdCost As Double
    Public Property BookFinAPStdCost() As Double
        Get
            Return _BookFinAPStdCost
        End Get
        Set(ByVal value As Double)
            _BookFinAPStdCost = value
        End Set
    End Property

    Private _BookFinAPACtCost As Double = 0
    Public Property BookFinAPACtCost() As Double
        Get
            Return _BookFinAPACtCost
        End Get
        Set(ByVal value As Double)
            _BookFinAPACtCost = value
        End Set
    End Property

    Private _BookFinAPActTax As Double = 0
    Public Property BookFinAPActTax() As Double
        Get
            Return _BookFinAPActTax
        End Get
        Set(ByVal value As Double)
            _BookFinAPActTax = value
        End Set
    End Property

    Private _BookFinAPTotalTaxableFees As Double = 0
    Public Property BookFinAPTotalTaxableFees() As Double
        Get
            Return _BookFinAPTotalTaxableFees
        End Get
        Set(ByVal value As Double)
            _BookFinAPTotalTaxableFees = value
        End Set
    End Property

    Private _BookFinAPTotalTaxes As Double = 0
    Public Property BookFinAPTotalTaxes() As Double
        Get
            Return _BookFinAPTotalTaxes
        End Get
        Set(ByVal value As Double)
            _BookFinAPTotalTaxes = value
        End Set
    End Property

    Private _BookFinAPNonTaxableFees As Double = 0
    Public Property BookFinAPNonTaxableFees() As Double
        Get
            Return _BookFinAPNonTaxableFees
        End Get
        Set(ByVal value As Double)
            _BookFinAPNonTaxableFees = value
        End Set
    End Property

    Private _BookCarrBLNumber As String = ""
    Public Property BookCarrBLNumber() As String
        Get
            Return Left(_BookCarrBLNumber, 20)
        End Get
        Set(ByVal value As String)
            _BookCarrBLNumber = Left(value, 20)
        End Set
    End Property

    Private _BookFinAPExportRetry As Integer = 0
    Public Property BookFinAPExportRetry() As Integer
        Get
            Return _BookFinAPExportRetry
        End Get
        Set(ByVal value As Integer)
            _BookFinAPExportRetry = value
        End Set
    End Property

    Private _BookItemCostCenterNumber As String = ""
    Public Property BookItemCostCenterNumber() As String
        Get
            Return Left(_BookItemCostCenterNumber, 50)
        End Get
        Set(ByVal value As String)
            _BookItemCostCenterNumber = Left(value, 50)
        End Set
    End Property

    Private _BookFinAPExportDate As String = ""
    Public Property BookFinAPExportDate() As String
        Get
            Return _BookFinAPExportDate
        End Get
        Set(ByVal value As String)
            _BookFinAPExportDate = value
        End Set
    End Property

    Private _PrevSentDate As String = ""
    Public Property PrevSentDate() As String
        Get
            Return _PrevSentDate
        End Get
        Set(ByVal value As String)
            _PrevSentDate = value
        End Set
    End Property

    Private _CarrierEquipmentCodes As String = ""
    Public Property CarrierEquipmentCodes() As String
        Get
            Return _CarrierEquipmentCodes
        End Get
        Set(ByVal value As String)
            _CarrierEquipmentCodes = value
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

    Private _APFee1 As Double = 0
    Public Property APFee1() As Double
        Get
            Return _APFee1
        End Get
        Set(ByVal value As Double)
            _APFee1 = value
        End Set
    End Property

    Private _APFee2 As Double = 0
    Public Property APFee2() As Double
        Get
            Return _APFee2
        End Get
        Set(ByVal value As Double)
            _APFee2 = value
        End Set
    End Property

    Private _APFee3 As Double = 0
    Public Property APFee3() As Double
        Get
            Return _APFee3
        End Get
        Set(ByVal value As Double)
            _APFee3 = value
        End Set
    End Property

    Private _APFee4 As Double = 0
    Public Property APFee4() As Double
        Get
            Return _APFee4
        End Get
        Set(ByVal value As Double)
            _APFee4 = value
        End Set
    End Property

    Private _APFee5 As Double = 0
    Public Property APFee5() As Double
        Get
            Return _APFee5
        End Get
        Set(ByVal value As Double)
            _APFee5 = value
        End Set
    End Property

    Private _APFee6 As Double = 0
    Public Property APFee6() As Double
        Get
            Return _APFee6
        End Get
        Set(ByVal value As Double)
            _APFee6 = value
        End Set
    End Property

    Private _OtherCosts As Double = 0
    Public Property OtherCosts() As Double
        Get
            Return _OtherCosts
        End Get
        Set(ByVal value As Double)
            _OtherCosts = value
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

    Private _BookMilesFrom As Double = 0
    Public Property BookMilesFrom() As Double
        Get
            Return _BookMilesFrom
        End Get
        Set(ByVal value As Double)
            _BookMilesFrom = value
        End Set
    End Property

    Private _BookReasonCode As String = ""
    Public Property BookReasonCode() As String
        Get
            Return _BookReasonCode
        End Get
        Set(ByVal value As String)
            _BookReasonCode = value
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

    Private _APTaxDetail1 As Double = 0
    Public Property APTaxDetail1() As Double
        Get
            Return _APTaxDetail1
        End Get
        Set(ByVal value As Double)
            _APTaxDetail1 = value
        End Set
    End Property

    Private _APTaxDetail2 As Double = 0
    Public Property APTaxDetail2() As Double
        Get
            Return _APTaxDetail2
        End Get
        Set(ByVal value As Double)
            _APTaxDetail2 = value
        End Set
    End Property

    Private _APTaxDetail3 As Double = 0
    Public Property APTaxDetail3() As Double
        Get
            Return _APTaxDetail3
        End Get
        Set(ByVal value As Double)
            _APTaxDetail3 = value
        End Set
    End Property

    Private _APTaxDetail4 As Double = 0
    Public Property APTaxDetail4() As Double
        Get
            Return _APTaxDetail4
        End Get
        Set(ByVal value As Double)
            _APTaxDetail4 = value
        End Set
    End Property

    Private _APTaxDetail5 As Double = 0
    Public Property APTaxDetail5() As Double
        Get
            Return _APTaxDetail5
        End Get
        Set(ByVal value As Double)
            _APTaxDetail5 = value
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

    Private _APReduction As Double
    Public Property APReduction() As Double
        Get
            Return _APReduction
        End Get
        Set(ByVal value As Double)
            _APReduction = value
        End Set
    End Property

    Private _APReductionReason As Integer
    Public Property APReductionReason() As Integer
        Get
            Return _APReductionReason
        End Get
        Set(ByVal value As Integer)
            _APReductionReason = value
        End Set
    End Property

    Private _APReductionAdjustedCost As Double
    Public Property APReductionAdjustedCost() As Double
        Get
            Return _APReductionAdjustedCost
        End Get
        Set(ByVal value As Double)
            _APReductionAdjustedCost = value
        End Set
    End Property

End Class


<Serializable()> _
Partial Public Class APExportRecordsAggregated

    Private _BookSHID As String

    Private _Orders As System.Nullable(Of Integer)

    Private _CarrierNumber As System.Nullable(Of Integer)

    Private _CarrierAlphaCode As String

    Private _CarrierLegalEntity As String

    Private _CompLegalEntity As String

    Private _CompanyNumber As System.Nullable(Of Integer)

    Private _CompAlphaCode As String

    Private _CompNatNumber As System.Nullable(Of Integer)

    Private _APBillNumber As String

    Private _APBillNoDate As String

    Private _APBillInvDate As String

    Private _APExportRetry As System.Nullable(Of Integer)

    Private _APExportDate As String

    Private _PrevSentDate As String

    Private _APGLNumber As String

    Private _APActWgt As System.Nullable(Of Integer)

    Private _ContractedCost As System.Nullable(Of Double)

    Private _BilledCost As System.Nullable(Of Double)

    Private _APActTax As System.Nullable(Of Double)

    Private _APTotalTaxableFees As System.Nullable(Of Double)

    Private _APTotalTaxes As System.Nullable(Of Double)

    Private _APNonTaxableFees As System.Nullable(Of Double)

    Private _ErrNumber As System.Nullable(Of Integer)

    Private _RetMsg As String

    Private _BookCarrBLNumber As String

    Private _ShippingState As String

    Private _ShippingCity As String

    Public Sub New()
        MyBase.New()
    End Sub


    Public Property BookSHID() As String
        Get
            Return Me._BookSHID
        End Get
        Set(value As String)
            If (String.Equals(Me._BookSHID, value) = False) Then
                Me._BookSHID = value
            End If
        End Set
    End Property


    Public Property Orders() As System.Nullable(Of Integer)
        Get
            Return Me._Orders
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._Orders.Equals(value) = False) Then
                Me._Orders = value
            End If
        End Set
    End Property


    Public Property CarrierNumber() As System.Nullable(Of Integer)
        Get
            Return Me._CarrierNumber
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._CarrierNumber.Equals(value) = False) Then
                Me._CarrierNumber = value
            End If
        End Set
    End Property


    Public Property CarrierAlphaCode() As String
        Get
            Return Me._CarrierAlphaCode
        End Get
        Set(value As String)
            If (String.Equals(Me._CarrierAlphaCode, value) = False) Then
                Me._CarrierAlphaCode = value
            End If
        End Set
    End Property


    Public Property CarrierLegalEntity() As String
        Get
            Return Me._CarrierLegalEntity
        End Get
        Set(value As String)
            If (String.Equals(Me._CarrierLegalEntity, value) = False) Then
                Me._CarrierLegalEntity = value
            End If
        End Set
    End Property


    Public Property CompLegalEntity() As String
        Get
            Return Me._CompLegalEntity
        End Get
        Set(value As String)
            If (String.Equals(Me._CompLegalEntity, value) = False) Then
                Me._CompLegalEntity = value
            End If
        End Set
    End Property


    Public Property CompanyNumber() As System.Nullable(Of Integer)
        Get
            Return Me._CompanyNumber
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._CompanyNumber.Equals(value) = False) Then
                Me._CompanyNumber = value
            End If
        End Set
    End Property


    Public Property CompAlphaCode() As String
        Get
            Return Me._CompAlphaCode
        End Get
        Set(value As String)
            If (String.Equals(Me._CompAlphaCode, value) = False) Then
                Me._CompAlphaCode = value
            End If
        End Set
    End Property


    Public Property CompNatNumber() As System.Nullable(Of Integer)
        Get
            Return Me._CompNatNumber
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._CompNatNumber.Equals(value) = False) Then
                Me._CompNatNumber = value
            End If
        End Set
    End Property


    Public Property APBillNumber() As String
        Get
            Return Me._APBillNumber
        End Get
        Set(value As String)
            If (String.Equals(Me._APBillNumber, value) = False) Then
                Me._APBillNumber = value
            End If
        End Set
    End Property


    Public Property APBillNoDate() As String
        Get
            Return Me._APBillNoDate
        End Get
        Set(value As String)
            If (String.Equals(Me._APBillNoDate, value) = False) Then
                Me._APBillNoDate = value
            End If
        End Set
    End Property


    Public Property APBillInvDate() As String
        Get
            Return Me._APBillInvDate
        End Get
        Set(value As String)
            If (String.Equals(Me._APBillInvDate, value) = False) Then
                Me._APBillInvDate = value
            End If
        End Set
    End Property


    Public Property APExportRetry() As System.Nullable(Of Integer)
        Get
            Return Me._APExportRetry
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._APExportRetry.Equals(value) = False) Then
                Me._APExportRetry = value
            End If
        End Set
    End Property


    Public Property APExportDate() As String
        Get
            Return Me._APExportDate
        End Get
        Set(value As String)
            If (String.Equals(Me._APExportDate, value) = False) Then
                Me._APExportDate = value
            End If
        End Set
    End Property


    Public Property PrevSentDate() As String
        Get
            Return Me._PrevSentDate
        End Get
        Set(value As String)
            If (String.Equals(Me._PrevSentDate, value) = False) Then
                Me._PrevSentDate = value
            End If
        End Set
    End Property


    Public Property APGLNumber() As String
        Get
            Return Me._APGLNumber
        End Get
        Set(value As String)
            If (String.Equals(Me._APGLNumber, value) = False) Then
                Me._APGLNumber = value
            End If
        End Set
    End Property


    Public Property APActWgt() As System.Nullable(Of Integer)
        Get
            Return Me._APActWgt
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._APActWgt.Equals(value) = False) Then
                Me._APActWgt = value
            End If
        End Set
    End Property


    Public Property ContractedCost() As System.Nullable(Of Double)
        Get
            Return Me._ContractedCost
        End Get
        Set(value As System.Nullable(Of Double))
            If (Me._ContractedCost.Equals(value) = False) Then
                Me._ContractedCost = value
            End If
        End Set
    End Property


    Public Property BilledCost() As System.Nullable(Of Double)
        Get
            Return Me._BilledCost
        End Get
        Set(value As System.Nullable(Of Double))
            If (Me._BilledCost.Equals(value) = False) Then
                Me._BilledCost = value
            End If
        End Set
    End Property


    Public Property APActTax() As System.Nullable(Of Double)
        Get
            Return Me._APActTax
        End Get
        Set(value As System.Nullable(Of Double))
            If (Me._APActTax.Equals(value) = False) Then
                Me._APActTax = value
            End If
        End Set
    End Property


    Public Property APTotalTaxableFees() As System.Nullable(Of Double)
        Get
            Return Me._APTotalTaxableFees
        End Get
        Set(value As System.Nullable(Of Double))
            If (Me._APTotalTaxableFees.Equals(value) = False) Then
                Me._APTotalTaxableFees = value
            End If
        End Set
    End Property


    Public Property APTotalTaxes() As System.Nullable(Of Double)
        Get
            Return Me._APTotalTaxes
        End Get
        Set(value As System.Nullable(Of Double))
            If (Me._APTotalTaxes.Equals(value) = False) Then
                Me._APTotalTaxes = value
            End If
        End Set
    End Property


    Public Property APNonTaxableFees() As System.Nullable(Of Double)
        Get
            Return Me._APNonTaxableFees
        End Get
        Set(value As System.Nullable(Of Double))
            If (Me._APNonTaxableFees.Equals(value) = False) Then
                Me._APNonTaxableFees = value
            End If
        End Set
    End Property


    Public Property ErrNumber() As System.Nullable(Of Integer)
        Get
            Return Me._ErrNumber
        End Get
        Set(value As System.Nullable(Of Integer))
            If (Me._ErrNumber.Equals(value) = False) Then
                Me._ErrNumber = value
            End If
        End Set
    End Property


    Public Property RetMsg() As String
        Get
            Return Me._RetMsg
        End Get
        Set(value As String)
            If (String.Equals(Me._RetMsg, value) = False) Then
                Me._RetMsg = value
            End If
        End Set
    End Property

    Public Property BookCarrBLNumber() As String
        Get
            Return Me._BookCarrBLNumber
        End Get
        Set(value As String)
            If (String.Equals(Me._BookCarrBLNumber, value) = False) Then
                Me._BookCarrBLNumber = value
            End If
        End Set
    End Property

    Public Property ShippingState() As String
        Get
            Return Me._ShippingState
        End Get
        Set(value As String)
            If (String.Equals(Me._ShippingState, value) = False) Then
                Me._ShippingState = value
            End If
        End Set
    End Property

    Public Property ShippingCity() As String
        Get
            Return Me._ShippingCity
        End Get
        Set(value As String)
            If (String.Equals(Me._ShippingCity, value) = False) Then
                Me._ShippingCity = value
            End If
        End Set
    End Property
End Class

