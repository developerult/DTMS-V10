' Modified by RHR for v-8.5.4.006 on 04/21/024 added mapping to Ngl.API.Mapping.Dispatch object
Imports Map = Ngl.API.Mapping
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

Namespace Models
    'Added By LVV On 8/11/17 For v-8.0 TMS365
    'Modified by RHR for v-8.1 on 03/27/2018
    '  modified properties to match dispatching mapping model
    'Modified by RHR for v-8.2 on 12/6/2018 
    '  added DispatchBidType which maps to [BidBidTypeControl]
    'Modified by RHR for v-8.2 on 12/19/2018 
    '  added fields for dispatching which map to the NEXTStop NSAcceptBid object
    'Modified by RHR for v-8.2 on 12/22/2018 
    '  added LoadTenderType fields to assist with dispatching 
    Public Class Dispatch

        Private _LoadTenderControl As Integer
        Public Property LoadTenderControl() As Integer
            Get
                Return _LoadTenderControl
            End Get
            Set(ByVal value As Integer)
                _LoadTenderControl = value
            End Set
        End Property

        Private _DispatchLoadTenderType As Integer
        Public Property DispatchLoadTenderType() As Integer
            Get
                Return _DispatchLoadTenderType
            End Get
            Set(ByVal value As Integer)
                _DispatchLoadTenderType = value
            End Set
        End Property

        Private _BookControl As Integer
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _ModeTypeControl As Integer
        Public Property ModeTypeControl() As Integer
            Get
                Return _ModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _ModeTypeControl = value
            End Set
        End Property


        Private _BidControl As Integer
        Public Property BidControl() As Integer
            Get
                Return _BidControl
            End Get
            Set(ByVal value As Integer)
                _BidControl = value
            End Set
        End Property

        Private _DispatchBidType As Integer
        Public Property DispatchBidType() As Integer
            Get
                Return _DispatchBidType
            End Get
            Set(ByVal value As Integer)
                _DispatchBidType = value
            End Set
        End Property

        Private _CarrierControl As Integer
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierName As String
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property

        Private _CarrTarEquipMatControl As Integer
        Public Property CarrTarEquipMatControl() As Integer
            Get
                Return _CarrTarEquipMatControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarEquipMatControl = value
            End Set
        End Property

        Private _CarrTarEquipControl As Integer
        Public Property CarrTarEquipControl() As Integer
            Get
                Return _CarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarEquipControl = value
            End Set
        End Property

        Private _Origin As AddressBook
        Public Property Origin() As AddressBook
            Get
                Return _Origin
            End Get
            Set(ByVal value As AddressBook)
                _Origin = value
            End Set
        End Property

        Private _Destination As AddressBook
        Public Property Destination() As AddressBook
            Get
                Return _Destination
            End Get
            Set(ByVal value As AddressBook)
                _Destination = value
            End Set
        End Property

        Private _Items As Item()
        Public Property Items() As Item()
            Get
                Return _Items
            End Get
            Set(ByVal value As Item())
                _Items = value
            End Set
        End Property

        Private _HazControl As Integer
        Public Property HazControl() As Integer
            Get
                Return _HazControl
            End Get
            Set(ByVal value As Integer)
                _HazControl = value
            End Set
        End Property

        Private _PickupDate As Date
        ''' <summary>
        ''' Maps to LoadDate
        ''' </summary>
        ''' <returns></returns>
        Public Property PickupDate() As Date
            Get
                Return _PickupDate
            End Get
            Set(ByVal value As Date)
                _PickupDate = value
            End Set
        End Property

        Private _PickupStartTime As String
        ''' <summary>
        ''' Beginning of pickup time window
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' When saving to the database add time to PickupDate to create a valid date field
        ''' </remarks>
        Public Property PickupStartTime() As String
            Get
                Return _PickupStartTime
            End Get
            Set(ByVal value As String)
                _PickupStartTime = value
            End Set
        End Property

        Private _PickupEndTime As String
        ''' <summary>
        ''' End of pickup time window as string
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' When saving to the database add time to PickupDate to create a valid date field
        ''' </remarks>
        Public Property PickupEndTime() As String
            Get
                Return _PickupEndTime
            End Get
            Set(ByVal value As String)
                _PickupEndTime = value
            End Set
        End Property

        Private _DeliveryDate As Date
        ''' <summary>
        ''' Maps to RequiredDate
        ''' </summary>
        ''' <returns></returns>
        Public Property DeliveryDate() As Date
            Get
                Return _DeliveryDate
            End Get
            Set(ByVal value As Date)
                _DeliveryDate = value
            End Set
        End Property

        Private _DeliveryStartTime As String
        ''' <summary>
        ''' Beginning of Delivery time window
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' When saving to the database add time to DeliveryDate to create a valid date field
        ''' </remarks>
        Public Property DeliveryStartTime() As String
            Get
                Return _DeliveryStartTime
            End Get
            Set(ByVal value As String)
                _DeliveryStartTime = value
            End Set
        End Property

        Private _DeliveryEndTime As String
        ''' <summary>
        ''' End of Delivery time window as string
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' When saving to the database add time to DeliveryDate to create a valid date field
        ''' </remarks>
        Public Property DeliveryEndTime() As String
            Get
                Return _DeliveryEndTime
            End Get
            Set(ByVal value As String)
                _DeliveryEndTime = value
            End Set
        End Property

        Private _ProviderSCAC As String
        Public Property ProviderSCAC() As String
            Get
                Return _ProviderSCAC
            End Get
            Set(ByVal value As String)
                _ProviderSCAC = value
            End Set
        End Property

        Private _VendorSCAC As String
        Public Property VendorSCAC() As String
            Get
                Return _VendorSCAC
            End Get
            Set(ByVal value As String)
                _VendorSCAC = value
            End Set
        End Property

        Private _BillOfLading As String
        Public Property BillOfLading() As String
            Get
                Return _BillOfLading
            End Get
            Set(ByVal value As String)
                _BillOfLading = value
            End Set
        End Property

        Private _PONumber As String
        Public Property PONumber() As String
            Get
                Return _PONumber
            End Get
            Set(ByVal value As String)
                _PONumber = value
            End Set
        End Property

        Private _SHID As String
        Public Property SHID() As String
            Get
                Return _SHID
            End Get
            Set(ByVal value As String)
                _SHID = value
            End Set
        End Property

        Private _OrderNumber As String
        Public Property OrderNumber() As String
            Get
                Return _OrderNumber
            End Get
            Set(ByVal value As String)
                _OrderNumber = value
            End Set
        End Property


        Private _ItemOrderNumbers As String
        Public Property ItemOrderNumbers() As String
            Get
                Return _ItemOrderNumbers
            End Get
            Set(ByVal value As String)
                _ItemOrderNumbers = value
            End Set
        End Property

        Private _PickupNumber As String
        ''' <summary>
        ''' Maps to BookCarrOrderNumber
        ''' </summary>
        ''' <returns></returns>
        Public Property PickupNumber() As String
            Get
                Return _PickupNumber
            End Get
            Set(ByVal value As String)
                _PickupNumber = value
            End Set
        End Property

        Private _CarrierProNumber As String
        Public Property CarrierProNumber() As String
            Get
                Return _CarrierProNumber
            End Get
            Set(ByVal value As String)
                _CarrierProNumber = value
            End Set
        End Property

        Private _SystemGeneratedNbr As String
        Public Property SystemGeneratedNbr() As String
            Get
                Return _SystemGeneratedNbr
            End Get
            Set(ByVal value As String)
                _SystemGeneratedNbr = value
            End Set
        End Property

        Private _EXTERNALNbr As String
        Public Property EXTERNALNbr() As String
            Get
                Return _EXTERNALNbr
            End Get
            Set(ByVal value As String)
                _EXTERNALNbr = value
            End Set
        End Property

        Private _Accessorials As String()
        Public Property Accessorials() As String()
            Get
                Return _Accessorials
            End Get
            Set(ByVal value As String())
                _Accessorials = value
            End Set
        End Property

        Private _PickupNote As String
        Public Property PickupNote() As String
            Get
                Return _PickupNote
            End Get
            Set(ByVal value As String)
                _PickupNote = value
            End Set
        End Property

        Private _DeliveryNote As String
        Public Property DeliveryNote() As String
            Get
                Return _DeliveryNote
            End Get
            Set(ByVal value As String)
                _DeliveryNote = value
            End Set
        End Property

        Private _ConfidentialNote As String
        Public Property ConfidentialNote() As String
            Get
                Return _ConfidentialNote
            End Get
            Set(ByVal value As String)
                _ConfidentialNote = value
            End Set
        End Property

        Private _QuoteNumber As String
        Public Property QuoteNumber() As String
            Get
                Return _QuoteNumber
            End Get
            Set(ByVal value As String)
                _QuoteNumber = value
            End Set
        End Property

        Private _TotalWgt As Double
        Public Property TotalWgt() As Double
            Get
                Return _TotalWgt
            End Get
            Set(ByVal value As Double)
                _TotalWgt = value
            End Set
        End Property

        Private _TotalQty As Integer
        Public Property TotalQty() As Integer
            Get
                Return _TotalQty
            End Get
            Set(ByVal value As Integer)
                _TotalQty = value
            End Set
        End Property

        Private _TotalPlts As Integer
        Public Property TotalPlts() As Integer
            Get
                Return _TotalPlts
            End Get
            Set(ByVal value As Integer)
                _TotalPlts = value
            End Set
        End Property

        Private _TotalCube As Integer
        Public Property TotalCube() As Integer
            Get
                Return _TotalCube
            End Get
            Set(ByVal value As Integer)
                _TotalCube = value
            End Set
        End Property

        Private _WeightUnit As String
        Public Property WeightUnit() As String
            Get
                Return _WeightUnit
            End Get
            Set(ByVal value As String)
                _WeightUnit = value
            End Set
        End Property

        Private _LengthUnit As String
        Public Property LengthUnit() As String
            Get
                Return _LengthUnit
            End Get
            Set(ByVal value As String)
                _LengthUnit = value
            End Set
        End Property

        Private _Requestor As AddressBook
        Public Property Requestor() As AddressBook
            Get
                Return _Requestor
            End Get
            Set(ByVal value As AddressBook)
                _Requestor = value
            End Set
        End Property


        Private _BillTo As AddressBook
        ''' <summary>
        ''' Company Bill To information for carrier dispatching
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.4.006 on 04/21/2024
        ''' </remarks>
        Public Property BillTo() As AddressBook
            Get
                Return _BillTo
            End Get
            Set(ByVal value As AddressBook)
                _BillTo = value
            End Set
        End Property

        Private _EmergencyContact As Contact
        Public Property EmergencyContact() As Contact
            Get
                Return _EmergencyContact
            End Get
            Set(ByVal value As Contact)
                _EmergencyContact = value
            End Set
        End Property

        Private _PaymentTermsOverride As Boolean = False
        Public Property PaymentTermsOverride() As Boolean
            Get
                Return _PaymentTermsOverride
            End Get
            Set(ByVal value As Boolean)
                _PaymentTermsOverride = value
            End Set
        End Property

        Private _PaymentTermsOverrideControl As Integer
        ''' <summary>
        ''' Maps to API Code for "PREPAID" "COLLECT" "THIRD_PARTY" when PaymentTermsOverride is true
        ''' </summary>
        ''' <returns></returns>
        Public Property PaymentTermsOverrideControl() As Integer
            Get
                Return _PaymentTermsOverrideControl
            End Get
            Set(ByVal value As Integer)
                _PaymentTermsOverrideControl = value
            End Set
        End Property

        Private _DirectionOverride As Boolean = False
        Public Property DirectionOverride() As Boolean
            Get
                Return _DirectionOverride
            End Get
            Set(ByVal value As Boolean)
                _DirectionOverride = value
            End Set
        End Property

        Private _DirectionOverrideControl As String
        ''' <summary>
        ''' Maps to API Code for "SHIPPER" "CONSIGNEE" "THIRD_PARTY"  when DirectionOverride is true
        ''' </summary>
        ''' <returns></returns>
        Public Property DirectionOverrideControl() As String
            Get
                Return _DirectionOverrideControl
            End Get
            Set(ByVal value As String)
                _DirectionOverrideControl = value
            End Set
        End Property

        Private _LoadTenderTransTypeControl As Integer
        ''' <summary>
        ''' Outbound = 1, Transfer = 2, Inbound = 3
        ''' </summary>
        ''' <returns></returns>
        Public Property LoadTenderTransTypeControl() As Integer
            Get
                Return _LoadTenderTransTypeControl
            End Get
            Set(ByVal value As Integer)
                _LoadTenderTransTypeControl = value
            End Set
        End Property

        Private _LinearFeet As Integer
        Public Property LinearFeet() As Integer
            Get
                Return _LinearFeet
            End Get
            Set(ByVal value As Integer)
                _LinearFeet = value
            End Set
        End Property

        ' Response Fields updated by Controller
        Private _RespCapacityProviderBolUrl As String
        Public Property RespCapacityProviderBolUrl() As String
            Get
                Return Left(_RespCapacityProviderBolUrl, 1000)
            End Get
            Set(ByVal value As String)
                _RespCapacityProviderBolUrl = Left(value, 1000)
            End Set
        End Property

        Private _RespPackingVisualizationUrl As String
        Public Property RespPackingVisualizationUrl() As String
            Get
                Return Left(_RespPackingVisualizationUrl, 1000)
            End Get
            Set(ByVal value As String)
                _RespPackingVisualizationUrl = Left(value, 1000)
            End Set
        End Property

        Private _RespPickupNote As String
        Public Property RespPickupNote() As String
            Get
                Return Left(_RespPickupNote, 4000)
            End Get
            Set(ByVal value As String)
                _RespPickupNote = Left(value, 4000)
            End Set
        End Property

        Private _RespPickupDateTime As Date?
        Public Property RespPickupDateTime() As Date?
            Get
                Return _RespPickupDateTime
            End Get
            Set(ByVal value As Date?)
                _RespPickupDateTime = value
            End Set
        End Property

        Private _InfoMessages As APIMessage()
        ''' <summary>
        ''' Used to generate Load Status Messages associated with dispatching
        ''' </summary>
        ''' <returns></returns>
        Public Property InfoMessages() As APIMessage()
            Get
                Return _InfoMessages
            End Get
            Set(ByVal value As APIMessage())
                _InfoMessages = value
            End Set
        End Property

        Private _ErrorCode As String
        ''' <summary>
        ''' Values: 
        ''' 0 of Blank  = Success (No Problems)
        ''' 1 = Connection Failure (Database Connection Problems)
        ''' 2 = Data Validation Failure (There Is a problem with the data being transmitted)
        ''' 3 = Failed (Total failure Check the Error Messages Returned)
        ''' 4 = Had Errors (Some failures were reported validate that your data is correct)
        ''' 400 =  invalid request; 
        ''' 401 = invalid or missing credentials; 
        ''' 403 = User not authorized to perform this operation
        ''' </summary>
        ''' <returns></returns>
        Public Property ErrorCode() As String
            Get
                Return Left(_ErrorCode, 20)
            End Get
            Set(ByVal value As String)
                _ErrorCode = Left(value, 20)
            End Set
        End Property

        Private _ErrorMessage As String
        Public Property ErrorMessage() As String
            Get
                Return Left(_ErrorMessage, 1000)
            End Get
            Set(ByVal value As String)
                _ErrorMessage = Left(value, 1000)
            End Set
        End Property

        Private _Errors As APIMessage()
        ''' <summary>
        ''' Used to generate Load Status Messages associated with dispatching
        ''' </summary>
        ''' <returns></returns>
        Public Property Errors() As APIMessage()
            Get
                Return _Errors
            End Get
            Set(ByVal value As APIMessage())
                _Errors = value
            End Set
        End Property

        Private _LineHaul As Decimal
        Public Property LineHaul() As Decimal
            Get
                Return _LineHaul
            End Get
            Set(ByVal value As Decimal)
                _LineHaul = value
            End Set
        End Property

        Private _OtherCost As Decimal
        Public Property OtherCost() As Decimal
            Get
                Return _OtherCost
            End Get
            Set(ByVal value As Decimal)
                _OtherCost = value
            End Set
        End Property

        Private _Fuel As Decimal
        Public Property Fuel() As Decimal
            Get
                Return _Fuel
            End Get
            Set(ByVal value As Decimal)
                _Fuel = value
            End Set
        End Property

        Private _Fees As Decimal
        Public Property Fees() As Decimal
            Get
                Return _Fees
            End Get
            Set(ByVal value As Decimal)
                _Fees = value
            End Set
        End Property

        Private _FuelVariable As Decimal
        Public Property FuelVariable() As Decimal
            Get
                Return _FuelVariable
            End Get
            Set(ByVal value As Decimal)
                _FuelVariable = value
            End Set
        End Property

        Private _FuelUOM As String
        Public Property FuelUOM() As String
            Get
                Return _FuelUOM
            End Get
            Set(ByVal value As String)
                _FuelUOM = value
            End Set
        End Property

        Private _TotalCost As String
        Public Property TotalCost() As String
            Get
                Return _TotalCost
            End Get
            Set(ByVal value As String)
                _TotalCost = value
            End Set
        End Property

        Private _BOLLegalText As String
        Public Property BOLLegalText() As String
            Get
                Return _BOLLegalText
            End Get
            Set(ByVal value As String)
                _BOLLegalText = value
            End Set
        End Property

        Private _DispatchLegalText As String
        Public Property DispatchLegalText() As String
            Get
                Return _DispatchLegalText
            End Get
            Set(ByVal value As String)
                _DispatchLegalText = value
            End Set
        End Property

        'Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
        Private _CarrierContact As Contact
        Public Property CarrierContact() As Contact
            Get
                Return _CarrierContact
            End Get
            Set(ByVal value As Contact)
                _CarrierContact = value
            End Set
        End Property

        Private _AutoAcceptOnDispatch As Boolean = False
        Public Property AutoAcceptOnDispatch() As Boolean
            Get
                Return _AutoAcceptOnDispatch
            End Get
            Set(ByVal value As Boolean)
                _AutoAcceptOnDispatch = value
            End Set
        End Property

        Private _EmailLoadTenderSheet As Boolean = True
        Public Property EmailLoadTenderSheet() As Boolean
            Get
                Return _EmailLoadTenderSheet
            End Get
            Set(ByVal value As Boolean)
                _EmailLoadTenderSheet = value
            End Set
        End Property

        Private _sShipIDs As String
        ''' <summary>
        ''' All Shipment ID returned from API
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2.0.119 on 10/01/19
        '''     used to log data in tblLoadTender.LTMessage field
        '''     used to help trouble shoot issues with confirmation number 
        ''' </remarks>
        Public Property sShipIDs() As String
            Get
                Return _sShipIDs
            End Get
            Set(ByVal value As String)
                _sShipIDs = value
            End Set
        End Property

        Private _BookFees As List(Of DTO.BookFee) = New List(Of DTO.BookFee)
        Public Property BookFees As List(Of DTO.BookFee)
            Get
                If _BookFees Is Nothing Then _BookFees = New List(Of DTO.BookFee)
                Return _BookFees
            End Get
            Set(value As List(Of DTO.BookFee))
                If value Is Nothing Then value = New List(Of DTO.BookFee)
                _BookFees = value
            End Set
        End Property

        Private _TempTypeDescription As String
        Public Property TempTypeDescription() As String
            Get
                Return _TempTypeDescription
            End Get
            Set(ByVal value As String)
                _TempTypeDescription = value
            End Set
        End Property

        Public Function MapNGLAPIDispatch() As Map.Dispatch
            Dim oRet As Map.Dispatch = New Map.Dispatch()

            Dim skipObjs As New List(Of String) From {"Origin",
                        "Destination",
                        "Items",
                        "Requestor",
                        "BillTo",
                        "InfoMessages",
                        "Errors",
                        "CarrierContact",
                        "BookFees"}
            oRet = DTran.CopyMatchingFields(oRet, Me, skipObjs)
            'add custom formatting
            With oRet
                oRet.Charges = New List(Of Map.Fees)
                For Each bFee As DTO.BookFee In Me.BookFees
                    Dim oFee As New Map.Fees With {.BookAcssControl = bFee.BookFeesControl, .BookAcssValue = bFee.BookFeesValue, .AccessorialCode = bFee.BookFeesAccessorialCode, .AccessorialName = bFee.BookFeesCaption, .AccessorialEDICode = bFee.BookFeesEDICode}
                    oRet.Charges.Add(oFee)
                Next
                oRet.Origin = Me.Origin.MapNGLAPIAddressBook()
                oRet.Destination = Me.Destination.MapNGLAPIAddressBook()
                oRet.Items = (From d In Me.Items Select d.MapNGLAPIItem()).ToArray() ' call RateRequestStop map to api function for each    
                oRet.Requestor = Me.Requestor.MapNGLAPIAddressBook()
                oRet.BillTo = Me.BillTo.MapNGLAPIAddressBook()
                oRet.CarrierContact = Me.CarrierContact.MapNGLAPIContact()
                oRet.InfoMessages = Me.InfoMessages.Select(Function(x) x.MapNGLAPIAPIMessage()).ToArray()
                oRet.Errors = Me.Errors.Select(Function(x) x.MapNGLAPIAPIMessage()).ToArray()
            End With


            Return oRet
        End Function



    End Class


End Namespace

