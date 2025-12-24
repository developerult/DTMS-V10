<Serializable()>
Public Class clsBookHeaderObject
    Public PONumber As String = ""
    Public POVendor As String = ""
    Public POdate As String = ""
    Public POShipdate As String = ""
    Public POBuyer As String = ""
    Public POFrt As Short = 0
    Public POTotalFrt As Double = 0
    Public POTotalCost As Double = 0
    Public POWgt As Double = 0
    Public POCube As Integer = 0
    Public POQty As Integer = 0
    Public POPallets As Integer = 0
    Public POLines As Double = 0
    Public POConfirm As Boolean = False
    Public PODefaultCustomer As String = ""
    Public PODefaultCarrier As Integer = 0
    Public POReqDate As String = ""
    Public POShipInstructions As String = ""
    Public POCooler As Boolean = False
    Public POFrozen As Boolean = False
    Public PODry As Boolean = False
    Public POTemp As String = ""
    Public POCarType As String = ""
    Public POShipVia As String = ""
    Public POShipViaType As String = ""
    Public POConsigneeNumber As String = ""
    Public POCustomerPO As String = ""
    Public POOtherCosts As Double = 0
    Public POStatusFlag As Integer = 0
    Public POOrderSequence As Integer = 0
    Public POChepGLID As String = ""
    Public POCarrierEquipmentCodes As String = ""
    Public POCarrierTypeCode As String = ""
    Public POPalletPositions As String = ""
    Public POSchedulePUDate As String = ""
    Public POSchedulePUTime As String = ""
    Public POScheduleDelDate As String = ""
    Public POSCheduleDelTime As String = ""
    Public POActPUDate As String = ""
    Public POActPUTime As String = ""
    Public POActDelDate As String = ""
    Public POActDelTime As String = ""
End Class

''' <summary>
''' 
''' </summary>
''' <remarks>
''' Modified by RHR for v-6.x on 04/20/2018
'''     added POWhseAuthorizationNo for Mizkan 
''' </remarks>
<Serializable()>
Public Class clsBookHeaderObject60 : Inherits clsImportDataBase


    Private _PONumber As String = ""
    Public Property PONumber As String
        Get
            Return Left(_PONumber, 20)
        End Get
        Set(value As String)
            _PONumber = Left(value, 20)
        End Set
    End Property

    Private _POVendor As String = ""
    Public Property POVendor As String
        Get
            Return Left(_POVendor, 160)
        End Get
        Set(value As String)
            _POVendor = Left(value, 160)
        End Set
    End Property

    Private _POdate As String = ""
    Public Property POdate As String
        Get
            Return cleanDate(_POdate)
        End Get
        Set(value As String)
            _POdate = value
        End Set
    End Property

    Private _POShipdate As String = ""
    Public Property POShipdate As String
        Get
            Return cleanDate(_POShipdate)
        End Get
        Set(value As String)
            _POShipdate = value
        End Set
    End Property

    Private _POBuyer As String = ""
    Public Property POBuyer As String
        Get
            Return Left(_POBuyer, 10)
        End Get
        Set(value As String)
            _POBuyer = Left(value, 10)
        End Set
    End Property

    Private _POFrt As Short = 0
    Public Property POFrt As Short
        Get
            Return _POFrt
        End Get
        Set(value As Short)
            _POFrt = value
        End Set
    End Property

    Private _POTotalFrt As Double = 0
    Public Property POTotalFrt As Double
        Get
            Return _POTotalFrt
        End Get
        Set(value As Double)
            _POTotalFrt = value
        End Set
    End Property

    Private _POTotalCost As Double = 0
    Public Property POTotalCost As Double
        Get
            Return _POTotalCost
        End Get
        Set(value As Double)
            _POTotalCost = value
        End Set
    End Property

    Private _POWgt As Double = 0
    Public Property POWgt As Double
        Get
            Return _POWgt
        End Get
        Set(value As Double)
            _POWgt = value
        End Set
    End Property

    Private _POCube As Integer = 0
    Public Property POCube As Integer
        Get
            Return _POCube
        End Get
        Set(value As Integer)
            _POCube = value
        End Set
    End Property

    Private _POQty As Integer = 0
    Public Property POQty As Integer
        Get
            Return _POQty
        End Get
        Set(value As Integer)
            _POQty = value
        End Set
    End Property

    Private _POPallets As Integer = 0
    Public Property POPallets As Integer
        Get
            Return _POPallets
        End Get
        Set(value As Integer)
            _POPallets = value
        End Set
    End Property

    Private _POLines As Double = 0
    Public Property POLines As Double
        Get
            Return _POLines
        End Get
        Set(value As Double)
            _POLines = value
        End Set
    End Property

    Private _POConfirm As Boolean = False
    Public Property POConfirm As Boolean
        Get
            Return _POConfirm
        End Get
        Set(value As Boolean)
            _POConfirm = value
        End Set
    End Property

    Private _PODefaultCustomer As String = ""
    Public Property PODefaultCustomer As String
        Get
            Return Left(_PODefaultCustomer, 50)
        End Get
        Set(value As String)
            _PODefaultCustomer = Left(value, 50)
        End Set
    End Property

    Private _PODefaultCarrier As Integer = 0
    Public Property PODefaultCarrier As Integer
        Get
            Return _PODefaultCarrier
        End Get
        Set(value As Integer)
            _PODefaultCarrier = value
        End Set
    End Property

    Private _POReqDate As String = ""
    Public Property POReqDate As String
        Get
            Return cleanDate(_POReqDate)
        End Get
        Set(value As String)
            _POReqDate = value
        End Set
    End Property

    Private _POShipInstructions As String = ""
    Public Property POShipInstructions As String
        Get
            Return Left(_POShipInstructions, 255)
        End Get
        Set(value As String)
            _POShipInstructions = Left(value, 255)
        End Set
    End Property

    Private _POCooler As Boolean = False
    Public Property POCooler As Boolean
        Get
            Return _POCooler
        End Get
        Set(value As Boolean)
            _POCooler = value
        End Set
    End Property

    Private _POFrozen As Boolean = False
    Public Property POFrozen As Boolean
        Get
            Return _POFrozen
        End Get
        Set(value As Boolean)
            _POFrozen = value
        End Set
    End Property

    Private _PODry As Boolean = False
    Public Property PODry As Boolean
        Get
            Return _PODry
        End Get
        Set(value As Boolean)
            _PODry = value
        End Set
    End Property

    Private _POTemp As String = ""
    Public Property POTemp As String
        Get
            Return Left(_POTemp, 1)
        End Get
        Set(value As String)
            _POTemp = Left(value, 1)
        End Set
    End Property

    Private _POCarType As String = ""
    Public Property POCarType As String
        Get
            Return Left(_POCarType, 15)
        End Get
        Set(value As String)
            _POCarType = Left(value, 15)
        End Set
    End Property

    Private _POShipVia As String = ""
    Public Property POShipVia As String
        Get
            Return Left(_POShipVia, 10)
        End Get
        Set(value As String)
            _POShipVia = Left(value, 10)
        End Set
    End Property

    Private _POShipViaType As String = ""
    Public Property POShipViaType As String
        Get
            Return Left(_POShipViaType, 10)
        End Get
        Set(value As String)
            _POShipViaType = Left(value, 10)
        End Set
    End Property

    Private _POConsigneeNumber As String = ""
    Public Property POConsigneeNumber As String
        Get
            Return Left(_POConsigneeNumber, 50)
        End Get
        Set(value As String)
            _POConsigneeNumber = Left(value, 50)
        End Set
    End Property

    Private _POCustomerPO As String = ""
    Public Property POCustomerPO As String
        Get
            Return Left(_POCustomerPO, 20)
        End Get
        Set(value As String)
            _POCustomerPO = Left(value, 20)
        End Set
    End Property

    Private _POOtherCosts As Double = 0
    Public Property POOtherCosts As Double
        Get
            Return _POOtherCosts
        End Get
        Set(value As Double)
            _POOtherCosts = value
        End Set
    End Property

    Private _POStatusFlag As Integer = 0
    Public Property POStatusFlag As Integer
        Get
            Return _POStatusFlag
        End Get
        Set(value As Integer)
            _POStatusFlag = value
        End Set
    End Property

    Private _POOrderSequence As Integer = 0
    Public Property POOrderSequence As Integer
        Get
            Return _POOrderSequence
        End Get
        Set(value As Integer)
            _POOrderSequence = value
        End Set
    End Property

    Private _POChepGLID As String = ""
    Public Property POChepGLID As String
        Get
            Return Left(_POChepGLID, 50)
        End Get
        Set(value As String)
            _POChepGLID = Left(value, 50)
        End Set
    End Property

    Private _POCarrierEquipmentCodes As String = ""
    Public Property POCarrierEquipmentCodes As String
        Get
            Return Left(_POCarrierEquipmentCodes, 50)
        End Get
        Set(value As String)
            _POCarrierEquipmentCodes = Left(value, 50)
        End Set
    End Property

    Private _POCarrierTypeCode As String = ""
    Public Property POCarrierTypeCode As String
        Get
            Return Left(_POCarrierTypeCode, 50)
        End Get
        Set(value As String)
            _POCarrierTypeCode = Left(value, 50)
        End Set
    End Property

    Private _POPalletPositions As String = ""
    Public Property POPalletPositions As String
        Get
            Return Left(_POPalletPositions, 50)
        End Get
        Set(value As String)
            _POPalletPositions = Left(value, 50)
        End Set
    End Property

    Private _POSchedulePUDate As String = ""
    Public Property POSchedulePUDate As String
        Get
            Return cleanDate(_POSchedulePUDate)
        End Get
        Set(value As String)
            _POSchedulePUDate = value
        End Set
    End Property

    Private _POSchedulePUTime As String = ""
    Public Property POSchedulePUTime As String
        Get
            Return CleanTime(_POSchedulePUTime)
        End Get
        Set(value As String)
            _POSchedulePUTime = value
        End Set
    End Property

    Private _POScheduleDelDate As String = ""
    Public Property POScheduleDelDate As String
        Get
            Return cleanDate(_POScheduleDelDate)
        End Get
        Set(value As String)
            _POScheduleDelDate = value
        End Set
    End Property

    Private _POSCheduleDelTime As String = ""
    Public Property POSCheduleDelTime As String
        Get
            Return CleanTime(_POSCheduleDelTime)
        End Get
        Set(value As String)
            _POSCheduleDelTime = value
        End Set
    End Property

    Private _POActPUDate As String = ""
    Public Property POActPUDate As String
        Get
            Return cleanDate(_POActPUDate)
        End Get
        Set(value As String)
            _POActPUDate = value
        End Set
    End Property

    Private _POActPUTime As String = ""
    Public Property POActPUTime As String
        Get
            Return CleanTime(_POActPUTime)
        End Get
        Set(value As String)
            _POActPUTime = value
        End Set
    End Property

    Private _POActDelDate As String = ""
    Public Property POActDelDate As String
        Get
            Return cleanDate(_POActDelDate)
        End Get
        Set(value As String)
            _POActDelDate = value
        End Set
    End Property

    Private _POActDelTime As String = ""
    Public Property POActDelTime As String
        Get
            Return CleanTime(_POActDelTime)
        End Get
        Set(value As String)
            _POActDelTime = value
        End Set
    End Property

    Private _POOrigCompNumber As String = ""
    ''' <summary>
    ''' Origin company number when creating lanes or using an alternate shipping address, 
    ''' set to zero when using company alpha codes,  
    ''' If this value is empty and this is an outbound load (POInbound = false)  the
    ''' system will use  PO Default Customer value by default
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property POOrigCompNumber As String
        Get
            If String.IsNullOrWhiteSpace(_POOrigCompNumber) And Me.POInbound = False Then _POOrigCompNumber = Me.PODefaultCustomer
            Return Left(_POOrigCompNumber, 50)
        End Get
        Set(value As String)
            _POOrigCompNumber = Left(value, 50)
        End Set
    End Property

    Private _POOrigName As String = ""
    Public Property POOrigName As String
        Get
            Return Left(_POOrigName, 40)
        End Get
        Set(value As String)
            _POOrigName = Left(value, 40)
        End Set
    End Property

    Private _POOrigAddress1 As String = ""
    Public Property POOrigAddress1 As String
        Get
            Return Left(_POOrigAddress1, 40)
        End Get
        Set(value As String)
            _POOrigAddress1 = Left(value, 40)
        End Set
    End Property

    Private _POOrigAddress2 As String = ""
    Public Property POOrigAddress2 As String
        Get
            Return Left(_POOrigAddress2, 40)
        End Get
        Set(value As String)
            _POOrigAddress2 = Left(value, 40)
        End Set
    End Property

    Private _POOrigAddress3 As String = ""
    Public Property POOrigAddress3 As String
        Get
            Return Left(_POOrigAddress3, 40)
        End Get
        Set(value As String)
            _POOrigAddress3 = Left(value, 40)
        End Set
    End Property

    Private _POOrigCity As String = ""
    Public Property POOrigCity As String
        Get
            Return Left(_POOrigCity, 25)
        End Get
        Set(value As String)
            _POOrigCity = Left(value, 25)
        End Set
    End Property

    Private _POOrigState As String = ""
    Public Property POOrigState As String
        Get
            Return Left(_POOrigState, 2)
        End Get
        Set(value As String)
            _POOrigState = Left(value, 2)
        End Set
    End Property

    Private _POOrigCountry As String = ""
    Public Property POOrigCountry As String
        Get
            Return Left(_POOrigCountry, 30)
        End Get
        Set(value As String)
            _POOrigCountry = Left(value, 30)
        End Set
    End Property

    Private _POOrigZip As String = ""
    Public Property POOrigZip As String
        Get
            Return Left(_POOrigZip, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(value As String)
            _POOrigZip = Left(value, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _POOrigContactPhone As String = ""
    Public Property POOrigContactPhone As String
        Get
            Return Left(_POOrigContactPhone, 15)
        End Get
        Set(value As String)
            _POOrigContactPhone = Left(value, 15)
        End Set
    End Property

    Private _POOrigContactPhoneExt As String = ""
    Public Property POOrigContactPhoneExt As String
        Get
            Return Left(_POOrigContactPhoneExt, 50)
        End Get
        Set(value As String)
            _POOrigContactPhoneExt = Left(value, 50)
        End Set
    End Property

    Private _POOrigContactFax As String = ""
    Public Property POOrigContactFax As String
        Get
            Return Left(_POOrigContactFax, 15)
        End Get
        Set(value As String)
            _POOrigContactFax = Left(value, 15)
        End Set
    End Property

    Private _PODestCompNumber As String = ""
    ''' <summary>
    ''' Destination company number when creating lanes or using an alternate shipping address, 
    ''' set to zero when using company alpha codes,  
    ''' If this value is empty and this is an inbound load (POInbound = true)  the
    ''' system will use  PO Default Customer value by default
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PODestCompNumber As String
        Get
            If String.IsNullOrWhiteSpace(_PODestCompNumber) And Me.POInbound = True Then _PODestCompNumber = Me.PODefaultCustomer
            Return Left(_PODestCompNumber, 50)
        End Get
        Set(value As String)
            _PODestCompNumber = Left(value, 50)
        End Set
    End Property

    Private _PODestName As String = ""
    Public Property PODestName As String
        Get
            Return Left(_PODestName, 40)
        End Get
        Set(value As String)
            _PODestName = Left(value, 40)
        End Set
    End Property

    Private _PODestAddress1 As String = ""
    Public Property PODestAddress1 As String
        Get
            Return Left(_PODestAddress1, 40)
        End Get
        Set(value As String)
            _PODestAddress1 = Left(value, 40)
        End Set
    End Property

    Private _PODestAddress2 As String = ""
    Public Property PODestAddress2 As String
        Get
            Return Left(_PODestAddress2, 40)
        End Get
        Set(value As String)
            _PODestAddress2 = Left(value, 40)
        End Set
    End Property

    Private _PODestAddress3 As String = ""
    Public Property PODestAddress3 As String
        Get
            Return Left(_PODestAddress3, 40)
        End Get
        Set(value As String)
            _PODestAddress3 = Left(value, 40)
        End Set
    End Property

    Private _PODestCity As String = ""
    Public Property PODestCity As String
        Get
            Return Left(_PODestCity, 25)
        End Get
        Set(value As String)
            _PODestCity = Left(value, 25)
        End Set
    End Property

    Private _PODestState As String = ""
    Public Property PODestState As String
        Get
            Return Left(_PODestState, 2)
        End Get
        Set(value As String)
            _PODestState = Left(value, 2)
        End Set
    End Property

    Private _PODestCountry As String = ""
    Public Property PODestCountry As String
        Get
            Return Left(_PODestCountry, 30)
        End Get
        Set(value As String)
            _PODestCountry = Left(value, 30)
        End Set
    End Property

    Private _PODestZip As String = ""
    Public Property PODestZip As String
        Get
            Return Left(_PODestZip, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(value As String)
            _PODestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _PODestContactPhone As String = ""
    Public Property PODestContactPhone As String
        Get
            Return Left(_PODestContactPhone, 15)
        End Get
        Set(value As String)
            _PODestContactPhone = Left(value, 15)
        End Set
    End Property

    Private _PODestContactPhoneExt As String = ""
    Public Property PODestContactPhoneExt As String
        Get
            Return Left(_PODestContactPhoneExt, 50)
        End Get
        Set(value As String)
            _PODestContactPhoneExt = Left(value, 50)
        End Set
    End Property

    Private _PODestContactFax As String = ""
    Public Property PODestContactFax As String
        Get
            Return Left(_PODestContactFax, 15)
        End Get
        Set(value As String)
            _PODestContactFax = Left(value, 15)
        End Set
    End Property

    Private _POInbound As Boolean = False
    Public Property POInbound As Boolean
        Get
            Return _POInbound
        End Get
        Set(value As Boolean)
            _POInbound = value
        End Set
    End Property


    Private _POPalletExchange As Boolean = False
    Public Property POPalletExchange As Boolean
        Get
            Return _POPalletExchange
        End Get
        Set(value As Boolean)
            _POPalletExchange = value
        End Set
    End Property

    Private _POPalletType As String = ""
    Public Property POPalletType As String
        Get
            Return Left(_POPalletType, 50)
        End Get
        Set(value As String)
            _POPalletType = Left(value, 50)
        End Set
    End Property

    Private _POComments As String = ""
    Public Property POComments As String
        Get
            Return Left(_POComments, 255)
        End Get
        Set(value As String)
            _POComments = Left(value, 255)
        End Set
    End Property

    Private _POCommentsConfidential As String = ""
    Public Property POCommentsConfidential As String
        Get
            Return Left(_POCommentsConfidential, 255)
        End Get
        Set(value As String)
            _POCommentsConfidential = Left(value, 255)
        End Set
    End Property

    Private _PODefaultRouteSequence As Integer = 0
    Public Property PODefaultRouteSequence As Integer
        Get
            Return _PODefaultRouteSequence
        End Get
        Set(value As Integer)
            _PODefaultRouteSequence = value
        End Set
    End Property

    Private _PORouteGuideNumber As String = ""
    Public Property PORouteGuideNumber As String
        Get
            Return Left(_PORouteGuideNumber, 50)
        End Get
        Set(value As String)
            _PORouteGuideNumber = Left(value, 50)
        End Set
    End Property

End Class


''' <summary>
''' Booking Header Object with POWhseAuthorizationNo
''' </summary>
''' <remarks>
''' Created by RHR for v-6.0.4.4m on 04/20/2018
'''     added POWhseAuthorizationNo for Mizkan 
''' </remarks>
<Serializable()>
Public Class clsBookHeaderObject604m : Inherits clsBookHeaderObject60


    Private _POWhseAuthorizationNo As String
    Public Property POWhseAuthorizationNo() As String
        Get
            Return Left(_POWhseAuthorizationNo, 20)
        End Get
        Set(ByVal value As String)
            _POWhseAuthorizationNo = Left(value, 20)
        End Set
    End Property


End Class


''' <summary>
''' Order import header data for v-6.0.4.7
''' </summary>
''' <remarks>
''' Created by RHR for v-6.0.4.7 on 5/22/2017
'''  used to implement EDI 204 In processing
'''  mirrors v-7.0.6.105 logic where possible without adding new fields to the database
'''  most are used to update the Lane information on auto creation of new lanes
''' Modified by RHR for v-6.0.4.7 on 6/8/2017
'''   added POHdrCarrBLNumber
''' </remarks>
<Serializable()>
Public Class clsBookHeaderObject604 : Inherits clsBookHeaderObject60


    Private _PORecMinIn As Integer = 0
    Public Property PORecMinIn As Integer
        Get
            Return _PORecMinIn
        End Get
        Set(value As Integer)
            _PORecMinIn = value
        End Set
    End Property

    Private _PORecMinUnload As Integer = 0
    Public Property PORecMinUnload As Integer
        Get
            Return _PORecMinUnload
        End Get
        Set(value As Integer)
            _PORecMinUnload = value
        End Set
    End Property

    Private _PORecMinOut As Integer = 0
    Public Property PORecMinOut As Integer
        Get
            Return _PORecMinOut
        End Get
        Set(value As Integer)
            _PORecMinOut = value
        End Set
    End Property

    Private _POAppt As Boolean = False
    Public Property POAppt As Boolean
        Get
            Return _POAppt
        End Get
        Set(value As Boolean)
            _POAppt = value
        End Set
    End Property

    Private _POBFC As Double = 0
    Public Property POBFC As Double
        Get
            Return _POBFC
        End Get
        Set(value As Double)
            _POBFC = value
        End Set
    End Property

    Private _POBFCType As String = ""
    Public Property POBFCType As String
        Get
            Return Left(_POBFCType, 50)
        End Get
        Set(value As String)
            _POBFCType = Left(value, 50)
        End Set
    End Property

    Private _POCarrBLNumber As String
    Public Property POCarrBLNumber() As String
        Get
            Return Left(_POCarrBLNumber, 20)
        End Get
        Set(ByVal value As String)
            _POCarrBLNumber = Left(value, 20)
        End Set
    End Property

End Class

''' <summary>
''' Booing Import Header data for v-7.0
''' </summary>
''' <remarks>
''' Modified by RHR for v-7.0.6.105 on 6/8/2017
'''  changed Inherits to use clsBookHeaderObject604 as the base class
''' </remarks>
<Serializable()>
Public Class clsBookHeaderObject70 : Inherits clsBookHeaderObject604

    'New Fields added to 7.0 interface
    Private _POCompLegalEntity As String = ""
    Public Property POCompLegalEntity As String
        Get
            Return Left(_POCompLegalEntity, 50)
        End Get
        Set(value As String)
            _POCompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _POCompAlphaCode As String = ""
    Public Property POCompAlphaCode() As String
        Get
            Return Left(_POCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _POCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _POModeTypeControl As Integer = 3 'Truck
    Public Property POModeTypeControl As Integer
        Get
            Return _POModeTypeControl
        End Get
        Set(value As Integer)
            _POModeTypeControl = value
        End Set
    End Property

    Private _POMustLeaveByDateTime As String = ""
    Public Property POMustLeaveByDateTime As String
        Get
            Return cleanDate(_POMustLeaveByDateTime)
        End Get
        Set(value As String)
            _POMustLeaveByDateTime = value
        End Set
    End Property

    Private _POUser1 As String = ""
    Public Property POUser1 As String
        Get
            Return Left(_POUser1, 4000)
        End Get
        Set(value As String)
            _POUser1 = Left(value, 4000)
        End Set
    End Property

    Private _POUser2 As String = ""
    Public Property POUser2 As String
        Get
            Return Left(_POUser2, 4000)
        End Get
        Set(value As String)
            _POUser2 = Left(value, 4000)
        End Set
    End Property

    Private _POUser3 As String = ""
    Public Property POUser3 As String
        Get
            Return Left(_POUser3, 4000)
        End Get
        Set(value As String)
            _POUser3 = Left(value, 4000)
        End Set
    End Property

    Private _POUser4 As String = ""
    Public Property POUser4 As String
        Get
            Return Left(_POUser4, 4000)
        End Get
        Set(value As String)
            _POUser4 = Left(value, 4000)
        End Set
    End Property

    Private _POAPGLNumber As String = ""
    Public Property POAPGLNumber As String
        Get
            Return Left(_POAPGLNumber, 50)
        End Get
        Set(value As String)
            _POAPGLNumber = Left(value, 50)
        End Set
    End Property

    Public Shared Function GenerateSampleObject(ByVal OrderNumber As String, ByVal LaneNumber As String, ByVal CompNumber As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String) As clsBookHeaderObject70

        Return New clsBookHeaderObject70 With {
            .PONumber = OrderNumber,
           .POVendor = LaneNumber,
           .PODefaultCustomer = CompNumber.ToString(),
           .POCompAlphaCode = CompAlphaCode,
           .POCompLegalEntity = CompLegalEntity,
           .POdate = System.DateTime.Now.ToShortDateString(),
           .POShipdate = System.DateTime.Now.ToShortDateString(),
           .POWgt = 14000.0,
           .POCube = 100,
           .POQty = 1,
           .POPallets = 1,
           .POReqDate = System.DateTime.Now.ToShortDateString(),
           .POCustomerPO = "XXX",
           .POStatusFlag = 0}

    End Function

End Class

<Serializable()>
Public Class clsBookHeaderObject705 : Inherits clsBookHeaderObject70

    'New Fields added to 7.0.5 interface

    Private _POOrigLegalEntity As String = ""
    ''' <summary>
    ''' Origin company legal entity used when creating lanes or using an alternate shipping address, 
    ''' set to empty string when shipping from a non-managed facility,  
    ''' If this value is empty and this is an outbound load (POInbound = false)  the
    ''' system will use PO Company Legal Entity value by default
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property POOrigLegalEntity As String
        Get
            If String.IsNullOrWhiteSpace(_POOrigLegalEntity) And Me.POInbound = False Then _POOrigLegalEntity = Me.POCompLegalEntity
            Return Left(_POOrigLegalEntity, 50)
        End Get
        Set(value As String)
            _POOrigLegalEntity = Left(value, 50)
        End Set
    End Property

    ''' <summary>
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <remarks></remarks>
    Private _POOrigCompAlphaCode As String = ""
    ''' <summary>
    ''' Origin company alpha code when creating lanes or using an alternate shipping address, 
    ''' set to empty string when shipping from a non-managed facility,  
    ''' If this value is empty and this is an outbound load (POInbound = false)  the
    ''' system will use PO Company AlphaCode value by default
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property POOrigCompAlphaCode() As String
        Get
            If String.IsNullOrWhiteSpace(_POOrigCompAlphaCode) And Me.POInbound = False Then _POOrigCompAlphaCode = Me.POCompAlphaCode
            Return Left(_POOrigCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _POOrigCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _PODestLegalEntity As String = ""
    ''' <summary>
    ''' Destination company legal entity used when creating lanes or using an alternate shipping address, 
    ''' set to empty string when shipping to a non-managed facility,  
    ''' If this value is empty and this is an inbound load (POInbound = true)  the
    ''' system will use PO Company Legal Entity value by default
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PODestLegalEntity As String
        Get
            If String.IsNullOrWhiteSpace(_PODestLegalEntity) And Me.POInbound = True Then _PODestLegalEntity = Me.POCompLegalEntity
            Return Left(_PODestLegalEntity, 50)
        End Get
        Set(value As String)
            _PODestLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _PODestCompAlphaCode As String = ""
    ''' <summary>
    ''' Destination company alpha code when creating lanes or using an alternate shipping address, 
    ''' set to empty string when shipping to a non-managed facility,  
    ''' If this value is empty and this is an inbound load (POInbound = true)  the
    ''' system will use PO Company AlphaCode value by default
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PODestCompAlphaCode() As String
        Get
            If String.IsNullOrWhiteSpace(_PODestCompAlphaCode) And Me.POInbound = True Then _PODestCompAlphaCode = Me.POCompAlphaCode
            Return Left(_PODestCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _PODestCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _ChangeNo As String = ""
    ''' <summary>
    ''' ERP System key field for header record
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 7/21/2016
    ''' Needed a reference to the ERP systems header key to assist with 
    ''' matching item detail records when duplicate records are transmitted in the same batch
    ''' </remarks>
    Public Property ChangeNo() As String
        Get
            Return Me._ChangeNo
        End Get
        Set(value As String)
            Me._ChangeNo = value
        End Set
    End Property




End Class

''' <summary>
''' booking header object for version 8.0 web services
''' </summary>
''' <remarks>
''' Created by RHR for v-8.0 on 09/11/2018
'''   added code changes implemented in v-6.0.4.4m
'''   now Inherits uses the POWhseAuthorizationNo
''' Modified by RHR for v-8.0 on 09/11/2018
'''     added new header fields
'''     POWhseReleaseNo
'''     POOrigContactEmail
'''     PODestContactEmail
''' Modifief by RHR for v-8.5.1.001 on 01/21/2022  merged properties
'''     Parent class properties do not display in WSDL so we do not inherit from older versions any longer.
'''     we just inherit from clsImportDataBase
''' </remarks>
<Serializable()>
Public Class clsBookHeaderObject80 : Inherits clsImportDataBase


    Private _PONumber As String = ""
    Public Property PONumber As String
        Get
            Return Left(_PONumber, 20)
        End Get
        Set(value As String)
            _PONumber = Left(value, 20)
        End Set
    End Property

    Private _POVendor As String = ""
    Public Property POVendor As String
        Get
            Return Left(_POVendor, 160)
        End Get
        Set(value As String)
            _POVendor = Left(value, 160)
        End Set
    End Property

    Private _POdate As String = ""
    Public Property POdate As String
        Get
            Return cleanDate(_POdate)
        End Get
        Set(value As String)
            _POdate = value
        End Set
    End Property

    Private _POShipdate As String = ""
    Public Property POShipdate As String
        Get
            Return cleanDate(_POShipdate)
        End Get
        Set(value As String)
            _POShipdate = value
        End Set
    End Property

    Private _POBuyer As String = ""
    Public Property POBuyer As String
        Get
            Return Left(_POBuyer, 10)
        End Get
        Set(value As String)
            _POBuyer = Left(value, 10)
        End Set
    End Property

    Private _POFrt As Short = 0
    Public Property POFrt As Short
        Get
            Return _POFrt
        End Get
        Set(value As Short)
            _POFrt = value
        End Set
    End Property

    Private _POTotalFrt As Double = 0
    Public Property POTotalFrt As Double
        Get
            Return _POTotalFrt
        End Get
        Set(value As Double)
            _POTotalFrt = value
        End Set
    End Property

    Private _POTotalCost As Double = 0
    Public Property POTotalCost As Double
        Get
            Return _POTotalCost
        End Get
        Set(value As Double)
            _POTotalCost = value
        End Set
    End Property

    Private _POWgt As Double = 0
    Public Property POWgt As Double
        Get
            Return _POWgt
        End Get
        Set(value As Double)
            _POWgt = value
        End Set
    End Property

    Private _POCube As Integer = 0
    Public Property POCube As Integer
        Get
            Return _POCube
        End Get
        Set(value As Integer)
            _POCube = value
        End Set
    End Property

    Private _POQty As Integer = 0
    Public Property POQty As Integer
        Get
            Return _POQty
        End Get
        Set(value As Integer)
            _POQty = value
        End Set
    End Property

    Private _POPallets As Integer = 0
    Public Property POPallets As Integer
        Get
            Return _POPallets
        End Get
        Set(value As Integer)
            _POPallets = value
        End Set
    End Property

    Private _POLines As Double = 0
    Public Property POLines As Double
        Get
            Return _POLines
        End Get
        Set(value As Double)
            _POLines = value
        End Set
    End Property

    Private _POConfirm As Boolean = False
    Public Property POConfirm As Boolean
        Get
            Return _POConfirm
        End Get
        Set(value As Boolean)
            _POConfirm = value
        End Set
    End Property

    Private _PODefaultCustomer As String = ""
    Public Property PODefaultCustomer As String
        Get
            Return Left(_PODefaultCustomer, 50)
        End Get
        Set(value As String)
            _PODefaultCustomer = Left(value, 50)
        End Set
    End Property

    Private _PODefaultCarrier As Integer = 0
    Public Property PODefaultCarrier As Integer
        Get
            Return _PODefaultCarrier
        End Get
        Set(value As Integer)
            _PODefaultCarrier = value
        End Set
    End Property

    Private _POReqDate As String = ""
    Public Property POReqDate As String
        Get
            Return cleanDate(_POReqDate)
        End Get
        Set(value As String)
            _POReqDate = value
        End Set
    End Property

    Private _POShipInstructions As String = ""
    Public Property POShipInstructions As String
        Get
            Return Left(_POShipInstructions, 255)
        End Get
        Set(value As String)
            _POShipInstructions = Left(value, 255)
        End Set
    End Property

    Private _POCooler As Boolean = False
    Public Property POCooler As Boolean
        Get
            Return _POCooler
        End Get
        Set(value As Boolean)
            _POCooler = value
        End Set
    End Property

    Private _POFrozen As Boolean = False
    Public Property POFrozen As Boolean
        Get
            Return _POFrozen
        End Get
        Set(value As Boolean)
            _POFrozen = value
        End Set
    End Property

    Private _PODry As Boolean = False
    Public Property PODry As Boolean
        Get
            Return _PODry
        End Get
        Set(value As Boolean)
            _PODry = value
        End Set
    End Property

    Private _POTemp As String = ""
    Public Property POTemp As String
        Get
            Return Left(_POTemp, 1)
        End Get
        Set(value As String)
            _POTemp = Left(value, 1)
        End Set
    End Property

    Private _POCarType As String = ""
    Public Property POCarType As String
        Get
            Return Left(_POCarType, 15)
        End Get
        Set(value As String)
            _POCarType = Left(value, 15)
        End Set
    End Property

    Private _POShipVia As String = ""
    Public Property POShipVia As String
        Get
            Return Left(_POShipVia, 10)
        End Get
        Set(value As String)
            _POShipVia = Left(value, 10)
        End Set
    End Property

    Private _POShipViaType As String = ""
    Public Property POShipViaType As String
        Get
            Return Left(_POShipViaType, 10)
        End Get
        Set(value As String)
            _POShipViaType = Left(value, 10)
        End Set
    End Property

    Private _POConsigneeNumber As String = ""
    Public Property POConsigneeNumber As String
        Get
            Return Left(_POConsigneeNumber, 50)
        End Get
        Set(value As String)
            _POConsigneeNumber = Left(value, 50)
        End Set
    End Property

    Private _POCustomerPO As String = ""
    Public Property POCustomerPO As String
        Get
            Return Left(_POCustomerPO, 20)
        End Get
        Set(value As String)
            _POCustomerPO = Left(value, 20)
        End Set
    End Property

    Private _POOtherCosts As Double = 0
    Public Property POOtherCosts As Double
        Get
            Return _POOtherCosts
        End Get
        Set(value As Double)
            _POOtherCosts = value
        End Set
    End Property

    Private _POStatusFlag As Integer = 0
    Public Property POStatusFlag As Integer
        Get
            Return _POStatusFlag
        End Get
        Set(value As Integer)
            _POStatusFlag = value
        End Set
    End Property

    Private _POOrderSequence As Integer = 0
    Public Property POOrderSequence As Integer
        Get
            Return _POOrderSequence
        End Get
        Set(value As Integer)
            _POOrderSequence = value
        End Set
    End Property

    Private _POChepGLID As String = ""
    Public Property POChepGLID As String
        Get
            Return Left(_POChepGLID, 50)
        End Get
        Set(value As String)
            _POChepGLID = Left(value, 50)
        End Set
    End Property

    Private _POCarrierEquipmentCodes As String = ""
    Public Property POCarrierEquipmentCodes As String
        Get
            Return Left(_POCarrierEquipmentCodes, 50)
        End Get
        Set(value As String)
            _POCarrierEquipmentCodes = Left(value, 50)
        End Set
    End Property

    Private _POCarrierTypeCode As String = ""
    Public Property POCarrierTypeCode As String
        Get
            Return Left(_POCarrierTypeCode, 50)
        End Get
        Set(value As String)
            _POCarrierTypeCode = Left(value, 50)
        End Set
    End Property

    Private _POPalletPositions As String = ""
    Public Property POPalletPositions As String
        Get
            Return Left(_POPalletPositions, 50)
        End Get
        Set(value As String)
            _POPalletPositions = Left(value, 50)
        End Set
    End Property

    Private _POSchedulePUDate As String = ""
    Public Property POSchedulePUDate As String
        Get
            Return cleanDate(_POSchedulePUDate)
        End Get
        Set(value As String)
            _POSchedulePUDate = value
        End Set
    End Property

    Private _POSchedulePUTime As String = ""
    Public Property POSchedulePUTime As String
        Get
            Return CleanTime(_POSchedulePUTime)
        End Get
        Set(value As String)
            _POSchedulePUTime = value
        End Set
    End Property

    Private _POScheduleDelDate As String = ""
    Public Property POScheduleDelDate As String
        Get
            Return cleanDate(_POScheduleDelDate)
        End Get
        Set(value As String)
            _POScheduleDelDate = value
        End Set
    End Property

    Private _POSCheduleDelTime As String = ""
    Public Property POSCheduleDelTime As String
        Get
            Return CleanTime(_POSCheduleDelTime)
        End Get
        Set(value As String)
            _POSCheduleDelTime = value
        End Set
    End Property

    Private _POActPUDate As String = ""
    Public Property POActPUDate As String
        Get
            Return cleanDate(_POActPUDate)
        End Get
        Set(value As String)
            _POActPUDate = value
        End Set
    End Property

    Private _POActPUTime As String = ""
    Public Property POActPUTime As String
        Get
            Return CleanTime(_POActPUTime)
        End Get
        Set(value As String)
            _POActPUTime = value
        End Set
    End Property

    Private _POActDelDate As String = ""
    Public Property POActDelDate As String
        Get
            Return cleanDate(_POActDelDate)
        End Get
        Set(value As String)
            _POActDelDate = value
        End Set
    End Property

    Private _POActDelTime As String = ""
    Public Property POActDelTime As String
        Get
            Return CleanTime(_POActDelTime)
        End Get
        Set(value As String)
            _POActDelTime = value
        End Set
    End Property

    Private _POOrigCompNumber As String = ""
    ''' <summary>
    ''' Origin company number when creating lanes or using an alternate shipping address, 
    ''' set to zero when using company alpha codes,  
    ''' If this value is empty and this is an outbound load (POInbound = false)  the
    ''' system will use  PO Default Customer value by default
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property POOrigCompNumber As String
        Get
            If String.IsNullOrWhiteSpace(_POOrigCompNumber) And Me.POInbound = False Then _POOrigCompNumber = Me.PODefaultCustomer
            Return Left(_POOrigCompNumber, 50)
        End Get
        Set(value As String)
            _POOrigCompNumber = Left(value, 50)
        End Set
    End Property

    Private _POOrigName As String = ""
    Public Property POOrigName As String
        Get
            Return Left(_POOrigName, 40)
        End Get
        Set(value As String)
            _POOrigName = Left(value, 40)
        End Set
    End Property

    Private _POOrigAddress1 As String = ""
    Public Property POOrigAddress1 As String
        Get
            Return Left(_POOrigAddress1, 40)
        End Get
        Set(value As String)
            _POOrigAddress1 = Left(value, 40)
        End Set
    End Property

    Private _POOrigAddress2 As String = ""
    Public Property POOrigAddress2 As String
        Get
            Return Left(_POOrigAddress2, 40)
        End Get
        Set(value As String)
            _POOrigAddress2 = Left(value, 40)
        End Set
    End Property

    Private _POOrigAddress3 As String = ""
    Public Property POOrigAddress3 As String
        Get
            Return Left(_POOrigAddress3, 40)
        End Get
        Set(value As String)
            _POOrigAddress3 = Left(value, 40)
        End Set
    End Property

    Private _POOrigCity As String = ""
    Public Property POOrigCity As String
        Get
            Return Left(_POOrigCity, 25)
        End Get
        Set(value As String)
            _POOrigCity = Left(value, 25)
        End Set
    End Property

    Private _POOrigState As String = ""
    Public Property POOrigState As String
        Get
            Return Left(_POOrigState, 2)
        End Get
        Set(value As String)
            _POOrigState = Left(value, 2)
        End Set
    End Property

    Private _POOrigCountry As String = ""
    Public Property POOrigCountry As String
        Get
            Return Left(_POOrigCountry, 30)
        End Get
        Set(value As String)
            _POOrigCountry = Left(value, 30)
        End Set
    End Property

    Private _POOrigZip As String = ""
    Public Property POOrigZip As String
        Get
            Return Left(_POOrigZip, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(value As String)
            _POOrigZip = Left(value, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _POOrigContactPhone As String = ""
    Public Property POOrigContactPhone As String
        Get
            Return Left(_POOrigContactPhone, 15)
        End Get
        Set(value As String)
            _POOrigContactPhone = Left(value, 15)
        End Set
    End Property

    Private _POOrigContactPhoneExt As String = ""
    Public Property POOrigContactPhoneExt As String
        Get
            Return Left(_POOrigContactPhoneExt, 50)
        End Get
        Set(value As String)
            _POOrigContactPhoneExt = Left(value, 50)
        End Set
    End Property

    Private _POOrigContactFax As String = ""
    Public Property POOrigContactFax As String
        Get
            Return Left(_POOrigContactFax, 15)
        End Get
        Set(value As String)
            _POOrigContactFax = Left(value, 15)
        End Set
    End Property

    Private _PODestCompNumber As String = ""
    ''' <summary>
    ''' Destination company number when creating lanes or using an alternate shipping address, 
    ''' set to zero when using company alpha codes,  
    ''' If this value is empty and this is an inbound load (POInbound = true)  the
    ''' system will use  PO Default Customer value by default
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PODestCompNumber As String
        Get
            If String.IsNullOrWhiteSpace(_PODestCompNumber) And Me.POInbound = True Then _PODestCompNumber = Me.PODefaultCustomer
            Return Left(_PODestCompNumber, 50)
        End Get
        Set(value As String)
            _PODestCompNumber = Left(value, 50)
        End Set
    End Property

    Private _PODestName As String = ""
    Public Property PODestName As String
        Get
            Return Left(_PODestName, 40)
        End Get
        Set(value As String)
            _PODestName = Left(value, 40)
        End Set
    End Property

    Private _PODestAddress1 As String = ""
    Public Property PODestAddress1 As String
        Get
            Return Left(_PODestAddress1, 40)
        End Get
        Set(value As String)
            _PODestAddress1 = Left(value, 40)
        End Set
    End Property

    Private _PODestAddress2 As String = ""
    Public Property PODestAddress2 As String
        Get
            Return Left(_PODestAddress2, 40)
        End Get
        Set(value As String)
            _PODestAddress2 = Left(value, 40)
        End Set
    End Property

    Private _PODestAddress3 As String = ""
    Public Property PODestAddress3 As String
        Get
            Return Left(_PODestAddress3, 40)
        End Get
        Set(value As String)
            _PODestAddress3 = Left(value, 40)
        End Set
    End Property

    Private _PODestCity As String = ""
    Public Property PODestCity As String
        Get
            Return Left(_PODestCity, 25)
        End Get
        Set(value As String)
            _PODestCity = Left(value, 25)
        End Set
    End Property

    Private _PODestState As String = ""
    Public Property PODestState As String
        Get
            Return Left(_PODestState, 2)
        End Get
        Set(value As String)
            _PODestState = Left(value, 2)
        End Set
    End Property

    Private _PODestCountry As String = ""
    Public Property PODestCountry As String
        Get
            Return Left(_PODestCountry, 30)
        End Get
        Set(value As String)
            _PODestCountry = Left(value, 30)
        End Set
    End Property

    Private _PODestZip As String = ""
    Public Property PODestZip As String
        Get
            Return Left(_PODestZip, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(value As String)
            _PODestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _PODestContactPhone As String = ""
    Public Property PODestContactPhone As String
        Get
            Return Left(_PODestContactPhone, 15)
        End Get
        Set(value As String)
            _PODestContactPhone = Left(value, 15)
        End Set
    End Property

    Private _PODestContactPhoneExt As String = ""
    Public Property PODestContactPhoneExt As String
        Get
            Return Left(_PODestContactPhoneExt, 50)
        End Get
        Set(value As String)
            _PODestContactPhoneExt = Left(value, 50)
        End Set
    End Property

    Private _PODestContactFax As String = ""
    Public Property PODestContactFax As String
        Get
            Return Left(_PODestContactFax, 15)
        End Get
        Set(value As String)
            _PODestContactFax = Left(value, 15)
        End Set
    End Property

    Private _POInbound As Boolean = False
    Public Property POInbound As Boolean
        Get
            Return _POInbound
        End Get
        Set(value As Boolean)
            _POInbound = value
        End Set
    End Property


    Private _POPalletExchange As Boolean = False
    Public Property POPalletExchange As Boolean
        Get
            Return _POPalletExchange
        End Get
        Set(value As Boolean)
            _POPalletExchange = value
        End Set
    End Property

    Private _POPalletType As String = ""
    Public Property POPalletType As String
        Get
            Return Left(_POPalletType, 50)
        End Get
        Set(value As String)
            _POPalletType = Left(value, 50)
        End Set
    End Property

    Private _POComments As String = ""
    Public Property POComments As String
        Get
            Return Left(_POComments, 255)
        End Get
        Set(value As String)
            _POComments = Left(value, 255)
        End Set
    End Property

    Private _POCommentsConfidential As String = ""
    Public Property POCommentsConfidential As String
        Get
            Return Left(_POCommentsConfidential, 255)
        End Get
        Set(value As String)
            _POCommentsConfidential = Left(value, 255)
        End Set
    End Property

    Private _PODefaultRouteSequence As Integer = 0
    Public Property PODefaultRouteSequence As Integer
        Get
            Return _PODefaultRouteSequence
        End Get
        Set(value As Integer)
            _PODefaultRouteSequence = value
        End Set
    End Property

    Private _PORouteGuideNumber As String = ""
    Public Property PORouteGuideNumber As String
        Get
            Return Left(_PORouteGuideNumber, 50)
        End Get
        Set(value As String)
            _PORouteGuideNumber = Left(value, 50)
        End Set
    End Property


    Private _PORecMinIn As Integer = 0
    Public Property PORecMinIn As Integer
        Get
            Return _PORecMinIn
        End Get
        Set(value As Integer)
            _PORecMinIn = value
        End Set
    End Property

    Private _PORecMinUnload As Integer = 0
    Public Property PORecMinUnload As Integer
        Get
            Return _PORecMinUnload
        End Get
        Set(value As Integer)
            _PORecMinUnload = value
        End Set
    End Property

    Private _PORecMinOut As Integer = 0
    Public Property PORecMinOut As Integer
        Get
            Return _PORecMinOut
        End Get
        Set(value As Integer)
            _PORecMinOut = value
        End Set
    End Property

    Private _POAppt As Boolean = False
    Public Property POAppt As Boolean
        Get
            Return _POAppt
        End Get
        Set(value As Boolean)
            _POAppt = value
        End Set
    End Property

    Private _POBFC As Double = 0
    Public Property POBFC As Double
        Get
            Return _POBFC
        End Get
        Set(value As Double)
            _POBFC = value
        End Set
    End Property

    Private _POBFCType As String = ""
    Public Property POBFCType As String
        Get
            Return Left(_POBFCType, 50)
        End Get
        Set(value As String)
            _POBFCType = Left(value, 50)
        End Set
    End Property

    Private _POCarrBLNumber As String
    Public Property POCarrBLNumber() As String
        Get
            Return Left(_POCarrBLNumber, 20)
        End Get
        Set(ByVal value As String)
            _POCarrBLNumber = Left(value, 20)
        End Set
    End Property

    'New Fields added to 7.0 interface
    Private _POCompLegalEntity As String = ""
    Public Property POCompLegalEntity As String
        Get
            Return Left(_POCompLegalEntity, 50)
        End Get
        Set(value As String)
            _POCompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _POCompAlphaCode As String = ""
    Public Property POCompAlphaCode() As String
        Get
            Return Left(_POCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _POCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _POModeTypeControl As Integer = 3 'Truck
    Public Property POModeTypeControl As Integer
        Get
            Return _POModeTypeControl
        End Get
        Set(value As Integer)
            _POModeTypeControl = value
        End Set
    End Property

    Private _POMustLeaveByDateTime As String = ""
    Public Property POMustLeaveByDateTime As String
        Get
            Return cleanDate(_POMustLeaveByDateTime)
        End Get
        Set(value As String)
            _POMustLeaveByDateTime = value
        End Set
    End Property

    Private _POUser1 As String = ""
    Public Property POUser1 As String
        Get
            Return Left(_POUser1, 4000)
        End Get
        Set(value As String)
            _POUser1 = Left(value, 4000)
        End Set
    End Property

    Private _POUser2 As String = ""
    Public Property POUser2 As String
        Get
            Return Left(_POUser2, 4000)
        End Get
        Set(value As String)
            _POUser2 = Left(value, 4000)
        End Set
    End Property

    Private _POUser3 As String = ""
    Public Property POUser3 As String
        Get
            Return Left(_POUser3, 4000)
        End Get
        Set(value As String)
            _POUser3 = Left(value, 4000)
        End Set
    End Property

    Private _POUser4 As String = ""
    Public Property POUser4 As String
        Get
            Return Left(_POUser4, 4000)
        End Get
        Set(value As String)
            _POUser4 = Left(value, 4000)
        End Set
    End Property

    Private _POAPGLNumber As String = ""
    Public Property POAPGLNumber As String
        Get
            Return Left(_POAPGLNumber, 50)
        End Get
        Set(value As String)
            _POAPGLNumber = Left(value, 50)
        End Set
    End Property

    'New Fields added to 7.0.5 interface

    Private _POOrigLegalEntity As String = ""
    ''' <summary>
    ''' Origin company legal entity used when creating lanes or using an alternate shipping address, 
    ''' set to empty string when shipping from a non-managed facility,  
    ''' If this value is empty and this is an outbound load (POInbound = false)  the
    ''' system will use PO Company Legal Entity value by default
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property POOrigLegalEntity As String
        Get
            If String.IsNullOrWhiteSpace(_POOrigLegalEntity) And Me.POInbound = False Then _POOrigLegalEntity = Me.POCompLegalEntity
            Return Left(_POOrigLegalEntity, 50)
        End Get
        Set(value As String)
            _POOrigLegalEntity = Left(value, 50)
        End Set
    End Property

    ''' <summary>
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <remarks></remarks>
    Private _POOrigCompAlphaCode As String = ""
    ''' <summary>
    ''' Origin company alpha code when creating lanes or using an alternate shipping address, 
    ''' set to empty string when shipping from a non-managed facility,  
    ''' If this value is empty and this is an outbound load (POInbound = false)  the
    ''' system will use PO Company AlphaCode value by default
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property POOrigCompAlphaCode() As String
        Get
            If String.IsNullOrWhiteSpace(_POOrigCompAlphaCode) And Me.POInbound = False Then _POOrigCompAlphaCode = Me.POCompAlphaCode
            Return Left(_POOrigCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _POOrigCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _PODestLegalEntity As String = ""
    ''' <summary>
    ''' Destination company legal entity used when creating lanes or using an alternate shipping address, 
    ''' set to empty string when shipping to a non-managed facility,  
    ''' If this value is empty and this is an inbound load (POInbound = true)  the
    ''' system will use PO Company Legal Entity value by default
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PODestLegalEntity As String
        Get
            If String.IsNullOrWhiteSpace(_PODestLegalEntity) And Me.POInbound = True Then _PODestLegalEntity = Me.POCompLegalEntity
            Return Left(_PODestLegalEntity, 50)
        End Get
        Set(value As String)
            _PODestLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _PODestCompAlphaCode As String = ""
    ''' <summary>
    ''' Destination company alpha code when creating lanes or using an alternate shipping address, 
    ''' set to empty string when shipping to a non-managed facility,  
    ''' If this value is empty and this is an inbound load (POInbound = true)  the
    ''' system will use PO Company AlphaCode value by default
    ''' Not Stored in POHDR Table,  used for automatic lane generation only
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PODestCompAlphaCode() As String
        Get
            If String.IsNullOrWhiteSpace(_PODestCompAlphaCode) And Me.POInbound = True Then _PODestCompAlphaCode = Me.POCompAlphaCode
            Return Left(_PODestCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _PODestCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _ChangeNo As String = ""
    ''' <summary>
    ''' ERP System key field for header record
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 7/21/2016
    ''' Needed a reference to the ERP systems header key to assist with 
    ''' matching item detail records when duplicate records are transmitted in the same batch
    ''' </remarks>
    Public Property ChangeNo() As String
        Get
            Return Me._ChangeNo
        End Get
        Set(value As String)
            Me._ChangeNo = value
        End Set
    End Property


    '********************************************

    Private _POWhseAuthorizationNo As String
    Public Property POWhseAuthorizationNo() As String
        Get
            Return Left(_POWhseAuthorizationNo, 20)
        End Get
        Set(ByVal value As String)
            _POWhseAuthorizationNo = Left(value, 20)
        End Set
    End Property

    Private _POOrigContactEmail As String
    Public Property POOrigContactEmail() As String
        Get
            Return Left(_POOrigContactEmail, 50)
        End Get
        Set(ByVal value As String)
            _POOrigContactEmail = Left(value, 50)
        End Set
    End Property

    Private _PODestContactEmail As String
    Public Property PODestContactEmail() As String
        Get
            Return Left(_PODestContactEmail, 50)
        End Get
        Set(ByVal value As String)
            _PODestContactEmail = Left(value, 50)
        End Set
    End Property

    Private _POWhseReleaseNo As String
    Public Property POWhseReleaseNo() As String
        Get
            Return Left(_POWhseReleaseNo, 20)
        End Get
        Set(ByVal value As String)
            _POWhseReleaseNo = Left(value, 20)
        End Set
    End Property



End Class

