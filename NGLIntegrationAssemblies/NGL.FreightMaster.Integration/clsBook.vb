Option Strict Off
Option Explicit On
Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports System.Xml.Serialization
Imports Ngl.Core.ChangeTracker
Imports System.ServiceModel

Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports BLL = Ngl.FM.BLL
Imports DTran = Ngl.Core.Utility.DataTransformation


<Serializable()>
Public Class clsBook : Inherits clsDownload

    Public Delegate Sub ProcessDataDelegate()

#Region "Constructors"

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Sub New(ByVal admin_email As String,
            ByVal from_email As String,
            ByVal group_email As String,
            ByVal auto_retry As Integer,
            ByVal smtp_server As String,
            ByVal db_server As String,
            ByVal database_catalog As String,
            ByVal auth_code As String,
            ByVal debug_mode As Boolean,
            Optional ByVal connection_string As String = "")

        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


    End Sub

#End Region

#Region " Class Variables and Properties "
    Private mintTotalOrders As Integer = 0
    Private mdblHashTotalOrders As Double = 0
    Private mintTotalDetails As Integer = 0
    Private mintTotalQty As Integer = 0
    Private mdblTotalWeight As Double = 0
    Private mdblHashTotalDetails As Double = 0
    Private mintErrCt As Integer = 0
    'Private objF As New FMLib.clsStandardFunctions
    Private mstrOrderNotificationEmail As String = ""
    Private mdCompanies As New Dictionary(Of String, Integer)
    Private mblnSomeItemsMissing As Boolean = False
    Private mintImportedCompControls As New List(Of Integer)
    Public mstrOrderNumbers As New List(Of String)
    Private mblnSharedServiceRunning As Boolean
    Private mintSharedServicesRunning As Integer = 0

    Private Shared mPadLock As New Object
    'Modified by RHR  v-7.0.5.100 5/31/2016 
    'New variable for allow blank company on delete
    Private mBlnIgnoreValidationOnDelete As Boolean = False

    Private WithEvents _SharedServices As NGL.FMWCFProxy.FMSharedServicesClient = NGL.FMWCFProxy.FMSharedServicesClient.GetInstance(New NGL.FMWCFProxy.NGLSynchronizationContext(System.Threading.SynchronizationContext.Current))
    Public ReadOnly Property SharedServices As NGL.FMWCFProxy.FMSharedServicesClient
        Get
            Return _SharedServices
        End Get
    End Property

    'Private WithEvents NGLSharedDataService As New Ngl.FMWCFProxy.FMSharedServicesClient(New Ngl.FMWCFProxy.NGLSynchronizationContext(System.Threading.SynchronizationContext.Current))

    Private _WCFDataProperties As NGL.FMWCFProxy.FMDataProperties
    Private Property WCFDataProperties As NGL.FMWCFProxy.FMDataProperties
        Get
            If _WCFDataProperties Is Nothing Then
                _WCFDataProperties = New NGL.FMWCFProxy.FMDataProperties
                With _WCFDataProperties
                    .Database = Me.Database
                    .DBServer = Me.DBServer
                    .UserName = Me.mstrCreateUser
                    .WCFAuthCode = Me.WCFAuthCode
                    .WCFServiceURL = Me.WCFURL
                    .WCFTCPServiceURL = Me.WCFTCPURL
                    .ConnectionString = Me.ConnectionString
                    'NOTE:  For multiple calls to the service it is faster
                    'to set the FormControl to 0 and leave the FormName blank
                    'This is because the Service checks if the user is authorized
                    'To access the form any time the form values are provided.
                    'The same logic applies to reports and procedures.
                    .FormControl = 0
                    .FormName = ""
                End With
            End If
            Return _WCFDataProperties
        End Get
        Set(value As NGL.FMWCFProxy.FMDataProperties)
            _WCFDataProperties = value
        End Set
    End Property


    Public Property OrderNotificationEmail() As String
        Get
            Return mstrOrderNotificationEmail
        End Get
        Set(ByVal value As String)
            mstrOrderNotificationEmail = value
        End Set
    End Property
    Private mblnValidateOrderUniqueness As Boolean = False
    Public Property ValidateOrderUniqueness() As Boolean
        Get
            Return mblnValidateOrderUniqueness
        End Get
        Set(ByVal value As Boolean)
            mblnValidateOrderUniqueness = value
        End Set
    End Property


    Public ReadOnly Property TotalWeight() As Double
        Get
            TotalWeight = mdblTotalWeight
        End Get
    End Property

    Public ReadOnly Property TotalQty() As Integer
        Get
            TotalQty = mintTotalQty
        End Get
    End Property

    Public ReadOnly Property HashTotalDetails() As Double
        Get
            HashTotalDetails = mdblHashTotalDetails
        End Get
    End Property

    Public ReadOnly Property TotalDetails() As Integer
        Get
            TotalDetails = mintTotalDetails
        End Get
    End Property

    Public ReadOnly Property HashTotalOrders() As Double
        Get
            HashTotalOrders = mdblHashTotalOrders
        End Get
    End Property

    Public ReadOnly Property TotalOrders() As Integer
        Get
            TotalOrders = mintTotalOrders
        End Get
    End Property

    Public Function getDataSet() As BookData
        Return New BookData
    End Function

    Private _RunSilentTenderAsync As Boolean = True
    Public Property RunSilentTenderAsync() As Boolean
        Get
            Return _RunSilentTenderAsync
        End Get
        Set(ByVal value As Boolean)
            _RunSilentTenderAsync = value
        End Set
    End Property



#End Region

#Region "Private Functions "

    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("PONumber", "POHDROrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK) '0
                .Add("PODefaultCustomer", "POHDRDefaultCustomer", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '1
                .Add("POOrderSequence", "POHDROrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK) '2
                .Add("POCustomerPO", "POHDRnumber", clsImportField.DataTypeID.gcvdtString, 20, True) '3
                .Add("POdate", " POHDRPOdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '4
                .Add("POvendor", "POHDRvendor", clsImportField.DataTypeID.gcvdtString, 160, True) '5
                .Add("POShipdate", "POHDRShipdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '6
                .Add("POBuyer", "POHDRBuyer", clsImportField.DataTypeID.gcvdtString, 10, True) '7
                .Add("POFrt", "POHDRFrt", clsImportField.DataTypeID.gcvdtTinyInt, 6, True) '8
                '.Add("CreateUser", "POHDRCreateUser", clsImportField.DataTypeID.gcvdtString, 25, False) '9
                '.Add("CreatedDate", "POHDRCreateDate", clsImportField.DataTypeID.gcvdtDate, 22, False) '10
                .Add("POTotalFrt", "POHDRTotalFrt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '11
                .Add("POTotalCost", "POHDRTotalCost", clsImportField.DataTypeID.gcvdtFloat, 20, True) '12
                .Add("POWgt", "POHDRWgt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '13
                .Add("POCube", "POHDRCube", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '14
                .Add("POQty", "POHDRQty", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '15
                .Add("POPallets", "POHDRPallets", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '16
                .Add("POLines", "POHDRLines", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '17
                .Add("POConfirm", "POHDRConfirm", clsImportField.DataTypeID.gcvdtBit, 2, True) '18
                .Add("PODefaultCarrier", "POHDRDefaultCarrier", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '19
                .Add("POReqDate", "POHDRReqDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '20
                .Add("POShipInstructions", "POHDRShipInstructions", clsImportField.DataTypeID.gcvdtString, 255, True) '21
                .Add("POCooler", "POHDRCooler", clsImportField.DataTypeID.gcvdtBit, 2, True) '22
                .Add("POFrozen", "POHDRFrozen", clsImportField.DataTypeID.gcvdtBit, 2, True) '23
                .Add("PODry", "POHDRDry", clsImportField.DataTypeID.gcvdtBit, 2, True) '24
                .Add("POTemp", "POHDRTemp", clsImportField.DataTypeID.gcvdtString, 1, True) '25
                .Add("POCarType", "POHDRCarType", clsImportField.DataTypeID.gcvdtString, 15, True) '26
                .Add("POShipVia", "POHDRShipVia", clsImportField.DataTypeID.gcvdtString, 10, True) '27
                .Add("POShipViaType", "POHDRShipViaType", clsImportField.DataTypeID.gcvdtString, 10, True) '28
                .Add("POOtherCosts", "POHDROtherCost", clsImportField.DataTypeID.gcvdtFloat, 22, True) '29
                .Add("POConsigneeNumber", "POConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 10, True, clsImportField.PKValue.gcHK) '30
                .Add("POStatusFlag", "POHDRStatusFlag", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '31
                .Add("POChepGLID", "POHDRChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '32
                .Add("POCarrierEquipmentCodes", "POHDRCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True) '33
                .Add("POCarrierTypeCode", "POHDRCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '34
                .Add("POPalletPositions", "POHDRPalletPositions", clsImportField.DataTypeID.gcvdtString, 50, True) '35
                .Add("POSchedulePUDate", "POHDRSchedulePUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '36
                .Add("POSchedulePUTime", "POHDRSchedulePUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '37
                .Add("POScheduleDelDate", "POHDRScheduleDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '38
                .Add("POSCheduleDelTime", "POHDRScheduleDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '39
                .Add("POActPUDate", "POHDRActPUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '40
                .Add("POActPUTime", "POHDRActPUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '41
                .Add("POActDelDate", "POHDRActDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
                .Add("POActDelTime", "POHDRActDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '43  


            End With
            Log("PO Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "POHDROrderNumber" Or oFields(ct).Name = "POHDRDefaultCustomer" Or oFields(ct).Name = "POHDROrderSequence" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' Build the 60 and 6044m header fields collection used for mapping and data validation
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' modified by RHR for v-6.0.4.4m on 04/20/2018
    '''     added support for POWhseAuthorizationNo
    ''' </remarks>
    Private Function buildHeaderCollection60(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("PONumber", "POHDROrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK) '0
                .Add("PODefaultCustomer", "POHDRDefaultCustomer", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '1
                .Add("POOrderSequence", "POHDROrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK) '2
                .Add("POCustomerPO", "POHDRnumber", clsImportField.DataTypeID.gcvdtString, 20, True) '3
                .Add("POdate", " POHDRPOdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '4
                .Add("POVendor", "POHDRvendor", clsImportField.DataTypeID.gcvdtString, 160, True) '5
                .Add("POShipdate", "POHDRShipdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '6
                .Add("POBuyer", "POHDRBuyer", clsImportField.DataTypeID.gcvdtString, 10, True) '7
                .Add("POFrt", "POHDRFrt", clsImportField.DataTypeID.gcvdtTinyInt, 6, True) '8
                '.Add("CreateUser", "POHDRCreateUser", clsImportField.DataTypeID.gcvdtString, 25, False) '9
                '.Add("CreatedDate", "POHDRCreateDate", clsImportField.DataTypeID.gcvdtDate, 22, False) '10
                .Add("POTotalFrt", "POHDRTotalFrt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '11
                .Add("POTotalCost", "POHDRTotalCost", clsImportField.DataTypeID.gcvdtFloat, 20, True) '12
                .Add("POWgt", "POHDRWgt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '13
                .Add("POCube", "POHDRCube", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '14
                .Add("POQty", "POHDRQty", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '15
                .Add("POPallets", "POHDRPallets", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '16
                .Add("POLines", "POHDRLines", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '17
                .Add("POConfirm", "POHDRConfirm", clsImportField.DataTypeID.gcvdtBit, 2, True) '18
                .Add("PODefaultCarrier", "POHDRDefaultCarrier", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '19
                .Add("POReqDate", "POHDRReqDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '20
                .Add("POShipInstructions", "POHDRShipInstructions", clsImportField.DataTypeID.gcvdtString, 255, True) '21
                .Add("POCooler", "POHDRCooler", clsImportField.DataTypeID.gcvdtBit, 2, True) '22
                .Add("POFrozen", "POHDRFrozen", clsImportField.DataTypeID.gcvdtBit, 2, True) '23
                .Add("PODry", "POHDRDry", clsImportField.DataTypeID.gcvdtBit, 2, True) '24
                .Add("POTemp", "POHDRTemp", clsImportField.DataTypeID.gcvdtString, 1, True) '25
                .Add("POCarType", "POHDRCarType", clsImportField.DataTypeID.gcvdtString, 15, True) '26
                .Add("POShipVia", "POHDRShipVia", clsImportField.DataTypeID.gcvdtString, 10, True) '27
                .Add("POShipViaType", "POHDRShipViaType", clsImportField.DataTypeID.gcvdtString, 10, True) '28
                .Add("POOtherCosts", "POHDROtherCost", clsImportField.DataTypeID.gcvdtFloat, 22, True) '29
                .Add("POConsigneeNumber", "POConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 10, True, clsImportField.PKValue.gcHK) '30
                .Add("POStatusFlag", "POHDRStatusFlag", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '31
                .Add("POChepGLID", "POHDRChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '32
                .Add("POCarrierEquipmentCodes", "POHDRCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True) '33
                .Add("POCarrierTypeCode", "POHDRCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '34
                .Add("POPalletPositions", "POHDRPalletPositions", clsImportField.DataTypeID.gcvdtString, 50, True) '35
                .Add("POSchedulePUDate", "POHDRSchedulePUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '36
                .Add("POSchedulePUTime", "POHDRSchedulePUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '37
                .Add("POScheduleDelDate", "POHDRScheduleDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '38
                .Add("POSCheduleDelTime", "POHDRScheduleDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '39
                .Add("POActPUDate", "POHDRActPUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '40
                .Add("POActPUTime", "POHDRActPUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '41
                .Add("POActDelDate", "POHDRActDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
                .Add("POActDelTime", "POHDRActDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '43 
                .Add("POOrigCompNumber", "POHDROrigCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '44
                .Add("POOrigName", "POHDROrigName", clsImportField.DataTypeID.gcvdtString, 40, True) '45
                .Add("POOrigAddress1", "POHDROrigAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '46
                .Add("POOrigAddress2", "POHDROrigAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '47
                .Add("POOrigAddress3", "POHDROrigAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '48
                .Add("POOrigCity", "POHDROrigCity", clsImportField.DataTypeID.gcvdtString, 25, True) '49
                .Add("POOrigState", "POHDROrigState", clsImportField.DataTypeID.gcvdtString, 8, True) '50
                .Add("POOrigCountry", "POHDROrigCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '51
                .Add("POOrigZip", "POHDROrigZip", clsImportField.DataTypeID.gcvdtString, 20, True) '52  'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("POOrigContactPhone", "POHDROrigContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '53 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("POOrigContactPhoneExt", "POHDROrigContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '54
                .Add("POOrigContactFax", "POHDROrigContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '55
                .Add("PODestCompNumber", "POHDRDestCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '56
                .Add("PODestName", "POHDRDestName", clsImportField.DataTypeID.gcvdtString, 40, True) '57
                .Add("PODestAddress1", "POHDRDestAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '58
                .Add("PODestAddress2", "POHDRDestAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '59
                .Add("PODestAddress3", "POHDRDestAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '60
                .Add("PODestCity", "POHDRDestCity", clsImportField.DataTypeID.gcvdtString, 25, True) '61
                .Add("PODestState", "POHDRDestState", clsImportField.DataTypeID.gcvdtString, 2, True) '62
                .Add("PODestCountry", "POHDRDestCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '63
                .Add("PODestZip", "POHDRDestZip", clsImportField.DataTypeID.gcvdtString, 20, True) '64  'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("PODestContactPhone", "POHDRDestContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '65 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("PODestContactPhoneExt", "POHDRDestContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '66
                .Add("PODestContactFax", "POHDRDestContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '67
                .Add("POPalletExchange", "POHDRPalletExchange", clsImportField.DataTypeID.gcvdtBit, 2, True) '68
                .Add("POPalletType", "POHDRPalletType", clsImportField.DataTypeID.gcvdtString, 50, True) '69
                .Add("POComments", "POHDRComments", clsImportField.DataTypeID.gcvdtString, 255, True) '70
                .Add("POCommentsConfidential", "POHDRCommentsConfidential", clsImportField.DataTypeID.gcvdtString, 255, True) '71
                .Add("POInbound", "POHDRInbound", clsImportField.DataTypeID.gcvdtBit, 2, True) '72
                .Add("PODefaultRouteSequence", "POHDRDefaultRouteSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '73
                .Add("PORouteGuideNumber", "POHDRRouteGuideNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '74
                .Add("POWhseAuthorizationNo", "POHDRWhseAuthorizationNo", clsImportField.DataTypeID.gcvdtString, 20, True) '75

            End With
            Log("PO Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "POHDROrderNumber" Or oFields(ct).Name = "POHDRDefaultCustomer" Or oFields(ct).Name = "POHDROrderSequence" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildHeaderCollection60, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildHeaderCollection60 Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' Builds a list of fields used in mapping for v-6.0.4.7
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.7 on 5/22/2017
    ''' added new fields to assist in automatic lane creation via EDI 204 in
    ''' POBFC
    ''' POBFCType
    ''' PORecMinUnload
    ''' PORecMinIn
    ''' PORecMinOut
    ''' POAppt
    ''' Modified by RHR for v-6.0.4.7 on 6/8/2017
    '''   added POHdrCarrBLNumber 
    ''' </remarks>
    Private Function buildHeaderCollection604(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("PONumber", "POHDROrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK) '0
                .Add("PODefaultCustomer", "POHDRDefaultCustomer", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '1
                .Add("POOrderSequence", "POHDROrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK) '2
                .Add("POCustomerPO", "POHDRnumber", clsImportField.DataTypeID.gcvdtString, 20, True) '3
                .Add("POdate", " POHDRPOdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '4
                .Add("POVendor", "POHDRvendor", clsImportField.DataTypeID.gcvdtString, 160, True) '5
                .Add("POShipdate", "POHDRShipdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '6
                .Add("POBuyer", "POHDRBuyer", clsImportField.DataTypeID.gcvdtString, 10, True) '7
                .Add("POFrt", "POHDRFrt", clsImportField.DataTypeID.gcvdtTinyInt, 6, True) '8
                '.Add("CreateUser", "POHDRCreateUser", clsImportField.DataTypeID.gcvdtString, 25, False) '9
                '.Add("CreatedDate", "POHDRCreateDate", clsImportField.DataTypeID.gcvdtDate, 22, False) '10
                .Add("POTotalFrt", "POHDRTotalFrt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '11
                .Add("POTotalCost", "POHDRTotalCost", clsImportField.DataTypeID.gcvdtFloat, 20, True) '12
                .Add("POWgt", "POHDRWgt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '13
                .Add("POCube", "POHDRCube", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '14
                .Add("POQty", "POHDRQty", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '15
                .Add("POPallets", "POHDRPallets", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '16
                .Add("POLines", "POHDRLines", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '17
                .Add("POConfirm", "POHDRConfirm", clsImportField.DataTypeID.gcvdtBit, 2, True) '18
                .Add("PODefaultCarrier", "POHDRDefaultCarrier", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '19
                .Add("POReqDate", "POHDRReqDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '20
                .Add("POShipInstructions", "POHDRShipInstructions", clsImportField.DataTypeID.gcvdtString, 255, True) '21
                .Add("POCooler", "POHDRCooler", clsImportField.DataTypeID.gcvdtBit, 2, True) '22
                .Add("POFrozen", "POHDRFrozen", clsImportField.DataTypeID.gcvdtBit, 2, True) '23
                .Add("PODry", "POHDRDry", clsImportField.DataTypeID.gcvdtBit, 2, True) '24
                .Add("POTemp", "POHDRTemp", clsImportField.DataTypeID.gcvdtString, 1, True) '25
                .Add("POCarType", "POHDRCarType", clsImportField.DataTypeID.gcvdtString, 15, True) '26
                .Add("POShipVia", "POHDRShipVia", clsImportField.DataTypeID.gcvdtString, 10, True) '27
                .Add("POShipViaType", "POHDRShipViaType", clsImportField.DataTypeID.gcvdtString, 10, True) '28
                .Add("POOtherCosts", "POHDROtherCost", clsImportField.DataTypeID.gcvdtFloat, 22, True) '29
                .Add("POConsigneeNumber", "POConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 10, True, clsImportField.PKValue.gcHK) '30
                .Add("POStatusFlag", "POHDRStatusFlag", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '31
                .Add("POChepGLID", "POHDRChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '32
                .Add("POCarrierEquipmentCodes", "POHDRCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True) '33
                .Add("POCarrierTypeCode", "POHDRCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '34
                .Add("POPalletPositions", "POHDRPalletPositions", clsImportField.DataTypeID.gcvdtString, 50, True) '35
                .Add("POSchedulePUDate", "POHDRSchedulePUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '36
                .Add("POSchedulePUTime", "POHDRSchedulePUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '37
                .Add("POScheduleDelDate", "POHDRScheduleDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '38
                .Add("POSCheduleDelTime", "POHDRScheduleDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '39
                .Add("POActPUDate", "POHDRActPUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '40
                .Add("POActPUTime", "POHDRActPUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '41
                .Add("POActDelDate", "POHDRActDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
                .Add("POActDelTime", "POHDRActDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '43 
                .Add("POOrigCompNumber", "POHDROrigCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '44
                .Add("POOrigName", "POHDROrigName", clsImportField.DataTypeID.gcvdtString, 40, True) '45
                .Add("POOrigAddress1", "POHDROrigAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '46
                .Add("POOrigAddress2", "POHDROrigAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '47
                .Add("POOrigAddress3", "POHDROrigAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '48
                .Add("POOrigCity", "POHDROrigCity", clsImportField.DataTypeID.gcvdtString, 25, True) '49
                .Add("POOrigState", "POHDROrigState", clsImportField.DataTypeID.gcvdtString, 8, True) '50
                .Add("POOrigCountry", "POHDROrigCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '51
                .Add("POOrigZip", "POHDROrigZip", clsImportField.DataTypeID.gcvdtString, 20, True) '52  'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("POOrigContactPhone", "POHDROrigContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '53 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("POOrigContactPhoneExt", "POHDROrigContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '54
                .Add("POOrigContactFax", "POHDROrigContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '55
                .Add("PODestCompNumber", "POHDRDestCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '56
                .Add("PODestName", "POHDRDestName", clsImportField.DataTypeID.gcvdtString, 40, True) '57
                .Add("PODestAddress1", "POHDRDestAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '58
                .Add("PODestAddress2", "POHDRDestAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '59
                .Add("PODestAddress3", "POHDRDestAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '60
                .Add("PODestCity", "POHDRDestCity", clsImportField.DataTypeID.gcvdtString, 25, True) '61
                .Add("PODestState", "POHDRDestState", clsImportField.DataTypeID.gcvdtString, 2, True) '62
                .Add("PODestCountry", "POHDRDestCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '63
                .Add("PODestZip", "POHDRDestZip", clsImportField.DataTypeID.gcvdtString, 20, True) '64 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("PODestContactPhone", "POHDRDestContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '65 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("PODestContactPhoneExt", "POHDRDestContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '66
                .Add("PODestContactFax", "POHDRDestContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '67
                .Add("POPalletExchange", "POHDRPalletExchange", clsImportField.DataTypeID.gcvdtBit, 2, True) '68
                .Add("POPalletType", "POHDRPalletType", clsImportField.DataTypeID.gcvdtString, 50, True) '69
                .Add("POComments", "POHDRComments", clsImportField.DataTypeID.gcvdtString, 255, True) '70
                .Add("POCommentsConfidential", "POHDRCommentsConfidential", clsImportField.DataTypeID.gcvdtString, 255, True) '71
                .Add("POInbound", "POHDRInbound", clsImportField.DataTypeID.gcvdtBit, 2, True) '72
                .Add("PODefaultRouteSequence", "POHDRDefaultRouteSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '73
                .Add("PORouteGuideNumber", "POHDRRouteGuideNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '74
                .Add("POBFC", "POBFC", clsImportField.DataTypeID.gcvdtFloat, 22, True, clsImportField.PKValue.gcHK) '75 used for lane mapping only
                .Add("POBFCType", "POBFCType", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '76 used for lane mapping only
                .Add("PORecMinUnload", "PORecMinUnload", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK) '77 used for lane mapping only
                .Add("PORecMinIn", "PORecMinIn", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK) '78 used for lane mapping only
                .Add("PORecMinOut", "PORecMinOut", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK) '79 used for lane mapping only
                .Add("POAppt", "POAppt", clsImportField.DataTypeID.gcvdtBit, 2, True, clsImportField.PKValue.gcHK) '80 used for lane mapping only
                .Add("POCarrBLNumber", "POHdrCarrBLNumber", clsImportField.DataTypeID.gcvdtString, 20, True) '81

            End With
            Log("PO Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "POHDROrderNumber" Or oFields(ct).Name = "POHDRDefaultCustomer" Or oFields(ct).Name = "POHDROrderSequence" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure:  NGL.FreightMaster.Integration.clsBook.buildHeaderCollection604, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildHeaderCollection604 Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' Builds a list of fields used in mapping for bv-7.x integration
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.4.7 on 6/8/2017
    '''     added POHdrCarrBLNumber
    ''' </remarks>
    Private Function buildHeaderCollection80(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("PONumber", "POHDROrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK) '0
                .Add("PODefaultCustomer", "POHDRDefaultCustomer", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '1
                .Add("POOrderSequence", "POHDROrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK) '2
                .Add("POCustomerPO", "POHDRnumber", clsImportField.DataTypeID.gcvdtString, 20, True) '3
                .Add("POdate", " POHDRPOdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '4
                .Add("POVendor", "POHDRvendor", clsImportField.DataTypeID.gcvdtString, 160, True) '5
                .Add("POShipdate", "POHDRShipdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '6
                .Add("POBuyer", "POHDRBuyer", clsImportField.DataTypeID.gcvdtString, 10, True) '7
                .Add("POFrt", "POHDRFrt", clsImportField.DataTypeID.gcvdtTinyInt, 6, True) '8
                '.Add("CreateUser", "POHDRCreateUser", clsImportField.DataTypeID.gcvdtString, 25, False) '9
                '.Add("CreatedDate", "POHDRCreateDate", clsImportField.DataTypeID.gcvdtDate, 22, False) '10
                .Add("POTotalFrt", "POHDRTotalFrt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '11
                .Add("POTotalCost", "POHDRTotalCost", clsImportField.DataTypeID.gcvdtFloat, 20, True) '12
                .Add("POWgt", "POHDRWgt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '13
                .Add("POCube", "POHDRCube", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '14
                .Add("POQty", "POHDRQty", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '15
                .Add("POPallets", "POHDRPallets", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '16
                .Add("POLines", "POHDRLines", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '17
                .Add("POConfirm", "POHDRConfirm", clsImportField.DataTypeID.gcvdtBit, 2, True) '18
                .Add("PODefaultCarrier", "POHDRDefaultCarrier", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '19
                .Add("POReqDate", "POHDRReqDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '20
                .Add("POShipInstructions", "POHDRShipInstructions", clsImportField.DataTypeID.gcvdtString, 255, True) '21
                .Add("POCooler", "POHDRCooler", clsImportField.DataTypeID.gcvdtBit, 2, True) '22
                .Add("POFrozen", "POHDRFrozen", clsImportField.DataTypeID.gcvdtBit, 2, True) '23
                .Add("PODry", "POHDRDry", clsImportField.DataTypeID.gcvdtBit, 2, True) '24
                .Add("POTemp", "POHDRTemp", clsImportField.DataTypeID.gcvdtString, 1, True) '25
                .Add("POCarType", "POHDRCarType", clsImportField.DataTypeID.gcvdtString, 15, True) '26
                .Add("POShipVia", "POHDRShipVia", clsImportField.DataTypeID.gcvdtString, 10, True) '27
                .Add("POShipViaType", "POHDRShipViaType", clsImportField.DataTypeID.gcvdtString, 10, True) '28
                .Add("POOtherCosts", "POHDROtherCost", clsImportField.DataTypeID.gcvdtFloat, 22, True) '29
                .Add("POConsigneeNumber", "POConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 10, True, clsImportField.PKValue.gcHK) '30
                .Add("POStatusFlag", "POHDRStatusFlag", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '31
                .Add("POChepGLID", "POHDRChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '32
                .Add("POCarrierEquipmentCodes", "POHDRCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True) '33
                .Add("POCarrierTypeCode", "POHDRCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '34
                .Add("POPalletPositions", "POHDRPalletPositions", clsImportField.DataTypeID.gcvdtString, 50, True) '35
                .Add("POSchedulePUDate", "POHDRSchedulePUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '36
                .Add("POSchedulePUTime", "POHDRSchedulePUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '37
                .Add("POScheduleDelDate", "POHDRScheduleDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '38
                .Add("POSCheduleDelTime", "POHDRScheduleDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '39
                .Add("POActPUDate", "POHDRActPUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '40
                .Add("POActPUTime", "POHDRActPUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '41
                .Add("POActDelDate", "POHDRActDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
                .Add("POActDelTime", "POHDRActDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '43 
                .Add("POOrigCompNumber", "POHDROrigCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '44
                .Add("POOrigName", "POHDROrigName", clsImportField.DataTypeID.gcvdtString, 40, True) '45
                .Add("POOrigAddress1", "POHDROrigAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '46
                .Add("POOrigAddress2", "POHDROrigAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '47
                .Add("POOrigAddress3", "POHDROrigAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '48
                .Add("POOrigCity", "POHDROrigCity", clsImportField.DataTypeID.gcvdtString, 25, True) '49
                .Add("POOrigState", "POHDROrigState", clsImportField.DataTypeID.gcvdtString, 8, True) '50
                .Add("POOrigCountry", "POHDROrigCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '51
                .Add("POOrigZip", "POHDROrigZip", clsImportField.DataTypeID.gcvdtString, 20, True) '52  'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("POOrigContactPhone", "POHDROrigContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '53 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("POOrigContactPhoneExt", "POHDROrigContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '54
                .Add("POOrigContactFax", "POHDROrigContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '55
                .Add("PODestCompNumber", "POHDRDestCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '56
                .Add("PODestName", "POHDRDestName", clsImportField.DataTypeID.gcvdtString, 40, True) '57
                .Add("PODestAddress1", "POHDRDestAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '58
                .Add("PODestAddress2", "POHDRDestAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '59
                .Add("PODestAddress3", "POHDRDestAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '60
                .Add("PODestCity", "POHDRDestCity", clsImportField.DataTypeID.gcvdtString, 25, True) '61
                .Add("PODestState", "POHDRDestState", clsImportField.DataTypeID.gcvdtString, 2, True) '62
                .Add("PODestCountry", "POHDRDestCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '63
                .Add("PODestZip", "POHDRDestZip", clsImportField.DataTypeID.gcvdtString, 20, True) '64 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("PODestContactPhone", "POHDRDestContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) '65 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("PODestContactPhoneExt", "POHDRDestContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '66
                .Add("PODestContactFax", "POHDRDestContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '67
                .Add("POPalletExchange", "POHDRPalletExchange", clsImportField.DataTypeID.gcvdtBit, 2, True) '68
                .Add("POPalletType", "POHDRPalletType", clsImportField.DataTypeID.gcvdtString, 50, True) '69
                .Add("POComments", "POHDRComments", clsImportField.DataTypeID.gcvdtString, 255, True) '70
                .Add("POCommentsConfidential", "POHDRCommentsConfidential", clsImportField.DataTypeID.gcvdtString, 255, True) '71
                .Add("POInbound", "POHDRInbound", clsImportField.DataTypeID.gcvdtBit, 2, True) '72
                .Add("PODefaultRouteSequence", "POHDRDefaultRouteSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '73
                .Add("PORouteGuideNumber", "POHDRRouteGuideNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '74
                .Add("POCompLegalEntity", "POHDRCompLegalEntity", clsImportField.DataTypeID.gcvdtString, 50, True) '75
                .Add("POCompAlphaCode", "POHDRCompAlphaCode", clsImportField.DataTypeID.gcvdtString, 50, True) '76
                .Add("POModeTypeControl", "POHDRModeTypeControl", clsImportField.DataTypeID.gcvdtLongInt, 20, True) '77
                .Add("POMustLeaveByDateTime", "POHDRMustLeaveByDateTime", clsImportField.DataTypeID.gcvdtDate, 22, True) '78
                .Add("POUser1", "POHDRUser1", clsImportField.DataTypeID.gcvdtString, 4000, True) '79
                .Add("POUser2", "POHDRUser2", clsImportField.DataTypeID.gcvdtString, 4000, True) '80
                .Add("POUser3", "POHDRUser3", clsImportField.DataTypeID.gcvdtString, 4000, True) '81
                .Add("POUser4", "POHDRUser4", clsImportField.DataTypeID.gcvdtString, 4000, True) '82
                .Add("POAPGLNumber", "POHDRAPGLNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '83
                .Add("POOrigLegalEntity", "POHDROrigLegalEntity", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '84
                .Add("POOrigCompAlphaCode", "POHDROrigCompAlphaCode", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '85
                .Add("PODestLegalEntity", "POHDRDestLegalEntity", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '86
                .Add("PODestCompAlphaCode", "POHDRDestCompAlphaCode", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '87
                .Add("ChangeNo", "ChangeNo", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '88
                .Add("POBFC", "POBFC", clsImportField.DataTypeID.gcvdtFloat, 22, True, clsImportField.PKValue.gcHK) '89 used for lane mapping only
                .Add("POBFCType", "POBFCType", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK) '90 used for lane mapping only
                .Add("PORecMinUnload", "PORecMinUnload", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK) '91 used for lane mapping only
                .Add("PORecMinIn", "PORecMinIn", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK) '92 used for lane mapping only
                .Add("PORecMinOut", "PORecMinOut", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK) '93 used for lane mapping only
                .Add("POAppt", "POAppt", clsImportField.DataTypeID.gcvdtBit, 2, True, clsImportField.PKValue.gcHK) '94 used for lane mapping only
                .Add("POCarrBLNumber", "POHdrCarrBLNumber", clsImportField.DataTypeID.gcvdtString, 20, True) '95
                .Add("POWhseAuthorizationNo", "POHDRWhseAuthorizationNo", clsImportField.DataTypeID.gcvdtString, 20, True) '96
                .Add("POOrigContactEmail", "POHDROrigContactEmail", clsImportField.DataTypeID.gcvdtString, 50, True) '97
                .Add("PODestContactEmail", "POHDRDestContactEmail", clsImportField.DataTypeID.gcvdtString, 50, True) '98
                .Add("POWhseReleaseNo", "POHDRWhseReleaseNo", clsImportField.DataTypeID.gcvdtString, 20, True) '99
            End With
            Log("PO Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "POHDROrderNumber" Or oFields(ct).Name = "POHDRDefaultCustomer" Or oFields(ct).Name = "POHDROrderSequence" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildHeaderCollection70, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildHeaderCollection70 Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildItemCollection(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("ItemPONumber", "ItemPONumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
                .Add("CustomerNumber", "CustomerNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
                .Add("POOrderSequence", "POOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("LotNumber", "LotNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("ItemNumber", "ItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("FixOffInvAllow", "FixOffInvAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("FixFrtAllow", "FixFrtAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("QtyOrdered", "QtyOrdered", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("FreightCost", "FreightCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("ItemCost", "ItemCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Weight", "Weight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Cube", "Cube", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("Pack", "Pack", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("Size", "Size", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Description", "Description", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Hazmat", "Hazmat", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("Brand", "Brand", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CostCenter", "CostCenter", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("LotExpirationDate", "LotExpirationDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("GTIN", "GTIN", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CustItemNumber", "CustItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            Log("PO Item Details Field Array Loaded.")
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    If oItems(ct).Name = "ItemPONumber" Or oItems(ct).Name = "CustomerNumber" Or oItems(ct).Name = "POOrderSequence" Or oItems(ct).Name = "LotNumber" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildItemCollection, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildItemCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' creates a new clsImportFields object for PO Items v-8.0 objects
    ''' </summary>
    ''' <param name="oItems"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 5/9/2019
    '''     add logic to process POItemOrderNumber and include this as a
    '''     possible primary key value,  the caller must check if the value is 
    '''     empty and reset the primary key if a value is not provided.
    ''' </remarks>
    Private Function buildItemCollection80(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("ItemPONumber", "ItemPONumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
                .Add("CustomerNumber", "CustomerNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
                .Add("POOrderSequence", "POOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("LotNumber", "LotNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("ItemNumber", "ItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("POItemOrderNumber", "POItemOrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK)
                .Add("FixOffInvAllow", "FixOffInvAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("FixFrtAllow", "FixFrtAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("QtyOrdered", "QtyOrdered", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("FreightCost", "FreightCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("ItemCost", "ItemCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Weight", "Weight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Cube", "Cube", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("Pack", "Pack", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("Size", "Size", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Description", "Description", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Hazmat", "Hazmat", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("Brand", "Brand", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CostCenter", "CostCenter", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("LotExpirationDate", "LotExpirationDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("GTIN", "GTIN", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CustItemNumber", "CustItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, True)
                'New fields added for v-5.2
                .Add("POItemHazmatTypeCode", "POItemHazmatTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItem49CFRCode", "POItem49CFRCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemIATACode", "POItemIATACode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemDOTCode", "POItemDOTCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemMarineCode", "POItemMarineCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemNMFCClass", "POItemNMFCClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemFAKClass", "POItemFAKClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemLimitedQtyFlag", "POItemLimitedQtyFlag", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("POItemPallets", "POItemPallets", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemTies", "POItemTies", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemHighs", "POItemHighs", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyPalletPercentage", "POItemQtyPalletPercentage", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyLength", "POItemQtyLength", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyWidth", "POItemQtyWidth", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyHeight", "POItemQtyHeight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemStackable", "POItemStackable", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("POItemLevelOfDensity", "POItemLevelOfDensity", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("POItemCompLegalEntity", "POItemCompLegalEntity", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("POItemCompAlphaCode", "POItemCompAlphaCode", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("POItemNMFCSubClass", "POItemNMFCSubClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemUser1", "POItemUser1", clsImportField.DataTypeID.gcvdtString, 4000, True)
                .Add("POItemUser2", "POItemUser2", clsImportField.DataTypeID.gcvdtString, 4000, True)
                .Add("POItemUser3", "POItemUser3", clsImportField.DataTypeID.gcvdtString, 4000, True)
                .Add("POItemUser4", "POItemUser4", clsImportField.DataTypeID.gcvdtString, 4000, True)
                .Add("ChangeNo", "ChangeNo", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK)
                .Add("BookItemCommCode", "POItemCommCode", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("POItemCustomerPO", "POItemCustomerPO", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("POItemLocationCode", "POItemLocationCode", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            Log("PO Item Details Field Array Loaded.")
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    If oItems(ct).Name = "ItemPONumber" Or oItems(ct).Name = "CustomerNumber" Or oItems(ct).Name = "POOrderSequence" Or oItems(ct).Name = "LotNumber" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: Ngl.FreightMaster.Integration.clsBook.buildItemCollection60, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildItemCollection60 Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' build the import fields collection object for v-7.x
    ''' </summary>
    ''' <param name="oItems"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.4.7 on 6/8/2017
    '''   added  POItemCustomerPO, POItemLocationCode 
    ''' </remarks>
    Private Function buildItemCollection7X(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("ItemPONumber", "ItemPONumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
                .Add("CustomerNumber", "CustomerNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
                .Add("POOrderSequence", "POOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("LotNumber", "LotNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("ItemNumber", "ItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("FixOffInvAllow", "FixOffInvAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("FixFrtAllow", "FixFrtAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("QtyOrdered", "QtyOrdered", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("FreightCost", "FreightCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("ItemCost", "ItemCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Weight", "Weight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Cube", "Cube", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("Pack", "Pack", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("Size", "Size", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Description", "Description", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Hazmat", "Hazmat", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("Brand", "Brand", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CostCenter", "CostCenter", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("LotExpirationDate", "LotExpirationDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("GTIN", "GTIN", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CustItemNumber", "CustItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, True)
                'New fields added for v-5.2
                .Add("POItemHazmatTypeCode", "POItemHazmatTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItem49CFRCode", "POItem49CFRCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemIATACode", "POItemIATACode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemDOTCode", "POItemDOTCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemMarineCode", "POItemMarineCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemNMFCClass", "POItemNMFCClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemFAKClass", "POItemFAKClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemLimitedQtyFlag", "POItemLimitedQtyFlag", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("POItemPallets", "POItemPallets", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemTies", "POItemTies", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemHighs", "POItemHighs", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyPalletPercentage", "POItemQtyPalletPercentage", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyLength", "POItemQtyLength", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyWidth", "POItemQtyWidth", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyHeight", "POItemQtyHeight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemStackable", "POItemStackable", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("POItemLevelOfDensity", "POItemLevelOfDensity", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("POItemCompLegalEntity", "POItemCompLegalEntity", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("POItemCompAlphaCode", "POItemCompAlphaCode", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("POItemNMFCSubClass", "POItemNMFCSubClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemUser1", "POItemUser1", clsImportField.DataTypeID.gcvdtString, 4000, True)
                .Add("POItemUser2", "POItemUser2", clsImportField.DataTypeID.gcvdtString, 4000, True)
                .Add("POItemUser3", "POItemUser3", clsImportField.DataTypeID.gcvdtString, 4000, True)
                .Add("POItemUser4", "POItemUser4", clsImportField.DataTypeID.gcvdtString, 4000, True)
                .Add("ChangeNo", "ChangeNo", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK)
                .Add("BookItemCommCode", "POItemCommCode", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("POItemCustomerPO", "POItemCustomerPO", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("POItemLocationCode", "POItemLocationCode", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            Log("PO Item Details Field Array Loaded.")
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    If oItems(ct).Name = "ItemPONumber" Or oItems(ct).Name = "CustomerNumber" Or oItems(ct).Name = "POOrderSequence" Or oItems(ct).Name = "LotNumber" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildItemCollection60, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildItemCollection60 Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function



    Private Function buildItemCollection60(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("ItemPONumber", "ItemPONumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
                .Add("CustomerNumber", "CustomerNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
                .Add("POOrderSequence", "POOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("LotNumber", "LotNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("ItemNumber", "ItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("FixOffInvAllow", "FixOffInvAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("FixFrtAllow", "FixFrtAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("QtyOrdered", "QtyOrdered", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("FreightCost", "FreightCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("ItemCost", "ItemCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Weight", "Weight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Cube", "Cube", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("Pack", "Pack", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("Size", "Size", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Description", "Description", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Hazmat", "Hazmat", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("Brand", "Brand", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CostCenter", "CostCenter", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("LotExpirationDate", "LotExpirationDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("GTIN", "GTIN", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CustItemNumber", "CustItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, True)
                'New fields added for v-5.2
                .Add("POItemHazmatTypeCode", "POItemHazmatTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItem49CFRCode", "POItem49CFRCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemIATACode", "POItemIATACode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemDOTCode", "POItemDOTCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemMarineCode", "POItemMarineCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemNMFCClass", "POItemNMFCClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemFAKClass", "POItemFAKClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemLimitedQtyFlag", "POItemLimitedQtyFlag", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("POItemPallets", "POItemPallets", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemTies", "POItemTies", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemHighs", "POItemHighs", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyPalletPercentage", "POItemQtyPalletPercentage", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyLength", "POItemQtyLength", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyWidth", "POItemQtyWidth", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyHeight", "POItemQtyHeight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemStackable", "POItemStackable", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("POItemLevelOfDensity", "POItemLevelOfDensity", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
            End With
            Log("PO Item Details Field Array Loaded.")
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    If oItems(ct).Name = "ItemPONumber" Or oItems(ct).Name = "CustomerNumber" Or oItems(ct).Name = "POOrderSequence" Or oItems(ct).Name = "LotNumber" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildItemCollection60, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildItemCollection60 Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' build the import fields collection object for v-6.0.4.7
    ''' </summary>
    ''' <param name="oItems"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 6/1/2017 for v-7.0.6.105
    '''    added support for new 604 items BookItemCommCode
    ''' Modified by RHR for v-6.0.4.7 on 6/8/2017
    '''   added  POItemCustomerPO, POItemLocationCode 
    ''' </remarks>
    Private Function buildItemCollection604(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("ItemPONumber", "ItemPONumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
                .Add("CustomerNumber", "CustomerNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
                .Add("POOrderSequence", "POOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("LotNumber", "LotNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("ItemNumber", "ItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
                .Add("FixOffInvAllow", "FixOffInvAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("FixFrtAllow", "FixFrtAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("QtyOrdered", "QtyOrdered", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("FreightCost", "FreightCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("ItemCost", "ItemCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Weight", "Weight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("Cube", "Cube", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("Pack", "Pack", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("Size", "Size", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Description", "Description", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("Hazmat", "Hazmat", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("Brand", "Brand", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CostCenter", "CostCenter", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("LotExpirationDate", "LotExpirationDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("GTIN", "GTIN", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CustItemNumber", "CustItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, True)
                'New fields added for v-5.2
                .Add("POItemHazmatTypeCode", "POItemHazmatTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItem49CFRCode", "POItem49CFRCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemIATACode", "POItemIATACode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemDOTCode", "POItemDOTCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemMarineCode", "POItemMarineCode", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemNMFCClass", "POItemNMFCClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemFAKClass", "POItemFAKClass", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("POItemLimitedQtyFlag", "POItemLimitedQtyFlag", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("POItemPallets", "POItemPallets", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemTies", "POItemTies", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemHighs", "POItemHighs", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyPalletPercentage", "POItemQtyPalletPercentage", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyLength", "POItemQtyLength", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyWidth", "POItemQtyWidth", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemQtyHeight", "POItemQtyHeight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("POItemStackable", "POItemStackable", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("POItemLevelOfDensity", "POItemLevelOfDensity", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("BookItemCommCode", "POItemCommCode", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("POItemCustomerPO", "POItemCustomerPO", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("POItemLocationCode", "POItemLocationCode", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            Log("PO Item Details Field Array Loaded.")
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    If oItems(ct).Name = "ItemPONumber" Or oItems(ct).Name = "CustomerNumber" Or oItems(ct).Name = "POOrderSequence" Or oItems(ct).Name = "LotNumber" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Book)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure:  NGL.FreightMaster.Integration.clsBook.buildItemCollection604, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.buildItemCollection604 Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' Legacy Import Header Record Procedure,  for backward compatibility only
    ''' </summary>
    ''' <param name="oOrders"></param>
    ''' <param name="oDetails"></param>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 - 6.0.4.7 6/2/1017
    '''   corrected bug in business logic for when to save no lane data
    ''' </remarks>
    Private Function importHeaderRecords(
                ByRef oOrders As BookData.BookHeaderDataTable,
                ByRef oDetails As BookData.BookDetailDataTable,
                ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsBook.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Dim intCompNumber As Integer = 0
            Dim intCompControl As Integer = 0
            Dim blnNoMatchingItem As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                TotalItems = 0
                Try
                    Log("Importing " & oOrders.Count & " PO Header Records.")
                    For Each oRow As BookData.BookHeaderRow In oOrders
                        'Reset the data types and values to defaults for the following fields 
                        'at the top of each loop to handle alpha vs numeric data changes
                        oFields("PODefaultCustomer").DataType = clsImportField.DataTypeID.gcvdtString
                        oFields("PODefaultCustomer").Length = 160
                        oFields("PODefaultCustomer").Null = True
                        strErrorMessage = ""
                        blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                        'Get the item details for this order (must be called before we lookup the company alpha number so numbers still match)
                        Dim oTheseDetails As BookData.BookDetailDataTable = getItemDetails(oFields, oDetails)
                        If oTheseDetails.Count < 1 Then
                            blnNoMatchingItem = True
                            'the modual level property determines if we insert all items 
                            'after the headers.  if any header is missing an item then
                            'we re-import all items 
                            mblnSomeItemsMissing = True
                        Else
                            blnNoMatchingItem = False
                        End If
                        'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
                        If blnDataValidated Then blnDataValidated = validateCompany(oFields("PODefaultCustomer"),
                                                                                            strErrorMessage,
                                                                                            oCX,
                                                                                            strSource)

                        'Create a new item list object
                        Dim oItemList As New List(Of clsImportFields)

                        If blnDataValidated Then
                            'Lookup the Company Control Number if needed; but only if there are no validation errors
                            intCompControl = oCX.getControlByNumber(oFields("PODefaultCustomer").Value)
                            If intCompControl = 0 Then
                                intCompControl = Me.lookupCompControlByNumber(oFields("PODefaultCustomer").Value)
                                'now add the comp control to the collection this save future trips to the database
                                'by sending nothing as the alpha code any existing alpha code does not get replaced
                                oCX.AddNew(Nothing, oFields("PODefaultCustomer").Value, intCompControl)
                            End If
                            'save the compcontrol number to a list so we only silent tender companies associated with this data
                            If mintImportedCompControls Is Nothing Then mintImportedCompControls = New List(Of Integer)
                            If Not mintImportedCompControls.Contains(intCompControl) Then mintImportedCompControls.Add(intCompControl)
                            'Fill the Item Detail List
                            If Not blnNoMatchingItem Then
                                If Not filItemRecordList(oTheseDetails, oItemList) Then
                                    'we cannot read the item so set the module level items missing propery to true
                                    mblnSomeItemsMissing = True
                                End If
                            End If
                        End If
                        'Process the NO Lane Logic
                        'If blnDataValidated Then
                        '    If (Not Me.IgnoreMissingLanes(intCompControl)) Then
                        '        If Not doesLaneExist(oFields) Then
                        '            'Save record to No Lanes
                        '            saveNoLaneData(oRow, oTheseDetails)
                        '            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
                        '            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
                        '            GoTo GetNextHeaderRecord
                        '        End If
                        '    End If
                        'End If
                        'Modified by RHR for v-7.0.6.105 - 6.0.4.7 corrected bug in business logic for when to save no lane data
                        'business rules
                        '1. data must be valid
                        '2. we only save no lane data (wait for lane to come in later) when the following are true
                        '   a) doesLaneExist returns false 
                        '   b) Me.IgnoreMissingLanes(intCompControl) = false
                        '3. POStatusFlag values are interpreted by doesLaneExist.  if the value is 5 or 6 
                        '   the system will attempt to create a new lane and if sucessful will return true
                        If blnDataValidated AndAlso (Me.IgnoreMissingLanes(intCompControl) = False And doesLaneExist(oFields) = False) Then
                            'Save record to No Lanes
                            saveNoLaneData(oRow, oTheseDetails)
                            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
                            TotalRecords += 1
                            GoTo GetNextHeaderRecord
                        End If
                        'test if the record already exists.
                        If blnDataValidated Then blnDataValidated = doesRecordExist(oFields,
                                                                                            strErrorMessage,
                                                                                            blnInsertRecord,
                                                                                            "Order number " & oFields("PONumber").Value,
                                                                                            "POHDR")
                        If Not blnDataValidated Then
                            addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                            RecordErrors += 1
                        Else
                            'add the order number to the list for silent tendering
                            If mstrOrderNumbers Is Nothing Then mstrOrderNumbers = New List(Of String)
                            mstrOrderNumbers.Add(stripQuotes(oFields("PONumber").Value))
                            'Save the changes to the main table
                            If saveData(oFields, blnInsertRecord, "POHDR", "POHDRCreateUser", "POHDRCreateDate") Then
                                'run the update defaults procedure
                                If Not updatePOHDRDefaults(oFields) Then GoTo GetNextHeaderRecord
                                TotalRecords += 1
                                mdblHashTotalOrders += Val(stripQuotes(oFields("PONumber").Value))
                                'Delete all existing item data
                                deleteItemData(oFields)
                                'Process Item Details but only if no missing items have been found for the entire batch
                                If Not mblnSomeItemsMissing Then
                                    Log("Importing " & oItemList.Count & " PO Item Records For Order Number" & oFields("PONumber").Value & ".")
                                    For Each oItems As clsImportFields In oItemList
                                        'We always do an insert because all previous detaisl were deleted
                                        If saveData(oItems, True, "POItem", "CreatedUser", "CreatedDate") Then
                                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
                                            'if it is true we wipe out the properties below and start over
                                            TotalItems += 1
                                            mintTotalQty += Val(oItems("QtyOrdered").Value)
                                            mdblTotalWeight += Val(oItems("Weight").Value)
                                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
                                        End If
                                    Next
                                End If
                                'If we get here we save the order history data
                                saveOrderHistory(oRow, oTheseDetails)
                            End If
                        End If
GetNextHeaderRecord:
                    Next
                    Return True
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, attempted to import PO Header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function



    ''' <summary>
    ''' Process and Import the 60 and 604m booking records from web services
    ''' </summary>
    ''' <param name="oOrders"></param>
    ''' <param name="oDetails"></param>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR for v-7.0.6.105 - 6.0.4.7 6/2/1017
    '''   corrected bug in business logic for when to save no lane data
    '''   Modified by RHR for v-6.0.4.4m on 04/20/2018
    '''   we now use the overloaded  clsBookHeaderObject604m object
    ''' </remarks>
    Private Function importHeaderRecords(
                ByRef oOrders As List(Of clsBookHeaderObject604m),
                ByRef oDetails As List(Of clsBookDetailObject60),
                ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            If oOrders Is Nothing OrElse oOrders.Count < 1 Then
                If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty")
                Return False
            End If
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsBook.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Dim intCompNumber As Integer = 0
            Dim intCompControl As Integer = 0
            Dim blnNoMatchingItem As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                TotalItems = 0
                Try
                    Log("Importing " & oOrders.Count & " PO Header Records.")
                    For Each oRow As clsBookHeaderObject60 In oOrders
                        'Reset the data types and values to defaults for the following fields 
                        'at the top of each loop to handle alpha vs numeric data changes
                        oFields("PODefaultCustomer").DataType = clsImportField.DataTypeID.gcvdtString
                        oFields("PODefaultCustomer").Length = 50
                        oFields("PODefaultCustomer").Null = True
                        strErrorMessage = ""
                        blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                        'Get the item details for this order (must be called before we lookup the company alpha number so numbers still match)
                        Dim oTheseDetails As List(Of clsBookDetailObject60) = getItemDetails(oFields, oDetails)
                        If oTheseDetails Is Nothing OrElse oTheseDetails.Count < 1 Then
                            blnNoMatchingItem = True
                            'the modual level property determines if we insert all items 
                            'after the headers.  if any header is missing an item then
                            'we re-import all items 
                            mblnSomeItemsMissing = True
                        Else
                            blnNoMatchingItem = False
                        End If
                        'Check for alpha company compatibility (note the company field type will be changed to an integer on success)
                        If blnDataValidated Then blnDataValidated = validateCompany(oFields("PODefaultCustomer"),
                                                                                            strErrorMessage,
                                                                                            oCX,
                                                                                            strSource)

                        'Create a new item list object
                        Dim oItemList As New List(Of clsImportFields)

                        If blnDataValidated Then
                            'Lookup the Company Control Number if needed; but only if there are no validation errors
                            intCompControl = oCX.getControlByNumber(oFields("PODefaultCustomer").Value)
                            If intCompControl = 0 Then
                                intCompControl = Me.lookupCompControlByNumber(oFields("PODefaultCustomer").Value)
                                'now add the comp control to the collection this save future trips to the database
                                'by sending nothing as the alpha code any existing alpha code does not get replaced
                                oCX.AddNew(Nothing, oFields("PODefaultCustomer").Value, intCompControl)
                            End If
                            'Fill the Item Detail List
                            If Not blnNoMatchingItem Then
                                If Not filItemRecordList(oTheseDetails, oItemList) Then
                                    'we cannot read the item so set the module level items missing propery to true
                                    mblnSomeItemsMissing = True
                                End If
                            End If
                        End If
                        'Process the NO Lane Logic
                        'If blnDataValidated Then
                        '    If (Not Me.IgnoreMissingLanes(intCompControl)) Then
                        '        If Not doesLaneExist(oFields) Then
                        '            'Save record to No Lanes
                        '            saveNoLaneData(oRow, oTheseDetails)
                        '            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
                        '            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
                        '            GoTo GetNextHeaderRecord
                        '        End If
                        '    End If
                        'End If
                        'Modified by RHR for v-7.0.6.105 - 6.0.4.7 corrected bug in business logic for when to save no lane data
                        'business rules
                        '1. data must be valid
                        '2. we only save no lane data (wait for lane to come in later) when the following are true
                        '   a) doesLaneExist returns false 
                        '   b) Me.IgnoreMissingLanes(intCompControl) = false
                        '3. POStatusFlag values are interpreted by doesLaneExist.  if the value is 5 or 6 
                        '   the system will attempt to create a new lane and if sucessful will return true
                        If blnDataValidated AndAlso (Me.IgnoreMissingLanes(intCompControl) = False And doesLaneExist(oFields) = False) Then
                            'Save record to No Lanes
                            saveNoLaneData(oRow, oTheseDetails)
                            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
                            TotalRecords += 1
                            GoTo GetNextHeaderRecord
                        End If
                        'test if the record already exists.
                        If blnDataValidated Then blnDataValidated = doesRecordExist(oFields,
                                                                                            strErrorMessage,
                                                                                            blnInsertRecord,
                                                                                            "Order number " & oFields("PONumber").Value,
                                                                                            "POHDR")
                        If Not blnDataValidated Then
                            addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                            RecordErrors += 1
                        Else
                            'add the order number to the list for silent tendering
                            If mstrOrderNumbers Is Nothing Then mstrOrderNumbers = New List(Of String)
                            mstrOrderNumbers.Add(stripQuotes(oFields("PONumber").Value))
                            'Save the changes to the main table
                            If saveData(oFields, blnInsertRecord, "POHDR", "POHDRCreateUser", "POHDRCreateDate") Then
                                'run the update defaults procedure
                                If Not updatePOHDRDefaults(oFields) Then GoTo GetNextHeaderRecord
                                TotalRecords += 1
                                mdblHashTotalOrders += Val(stripQuotes(oFields("PONumber").Value))
                                'Delete all existing item data
                                deleteItemData(oFields)
                                'Process Item Details but only if no missing items have been found for the entire batch
                                If Not mblnSomeItemsMissing Then
                                    Log("Importing " & oItemList.Count & " PO Item Records For Order Number" & oFields("PONumber").Value & ".")
                                    For Each oItems As clsImportFields In oItemList
                                        'We always do an insert because all previous detaisl were deleted
                                        If saveData(oItems, True, "POItem", "CreatedUser", "CreatedDate") Then
                                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
                                            'if it is true we wipe out the properties below and start over
                                            TotalItems += 1
                                            mintTotalQty += Val(oItems("QtyOrdered").Value)
                                            mdblTotalWeight += Val(oItems("Weight").Value)
                                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
                                        End If
                                    Next
                                End If
                                'If we get here we save the order history data
                                saveOrderHistory(oRow, oTheseDetails)
                            End If
                        End If
GetNextHeaderRecord:
                    Next
                    Return True
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, attempted to import PO Header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' 6.0.4.7 version of the Booking Header Import Procedure
    ''' </summary>
    ''' <param name="oOrders"></param>
    ''' <param name="oDetails"></param>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.7 on 5/22/2017
    '''   added to support EDI 204 inbound processing including automatic lane creation using POStatusFlag of 5 and 6
    '''  Modified by RHR on 6/1/2017 for v-7.0.6.105
    '''    added support for 604 items
    ''' </remarks>
    Private Function importHeaderRecords(
                ByRef oOrders As List(Of clsBookHeaderObject604),
                ByRef oDetails As List(Of clsBookDetailObject604),
                ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            If oOrders Is Nothing OrElse oOrders.Count < 1 Then
                If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty")
                Return False
            End If
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsBook.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Dim intCompNumber As Integer = 0
            Dim intCompControl As Integer = 0
            Dim blnNoMatchingItem As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                TotalItems = 0
                Try
                    Log("Importing " & oOrders.Count & " PO Header Records.")
                    For Each oRow As clsBookHeaderObject604 In oOrders
                        'Reset the data types and values to defaults for the following fields 
                        'at the top of each loop to handle alpha vs numeric data changes
                        oFields("PODefaultCustomer").DataType = clsImportField.DataTypeID.gcvdtString
                        oFields("PODefaultCustomer").Length = 50
                        oFields("PODefaultCustomer").Null = True
                        strErrorMessage = ""
                        blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                        'Get the item details for this order (must be called before we lookup the company alpha number so numbers still match)
                        Dim oTheseDetails As List(Of clsBookDetailObject604) = getItemDetails(oFields, oDetails)
                        If oTheseDetails Is Nothing OrElse oTheseDetails.Count < 1 Then
                            blnNoMatchingItem = True
                            'the modual level property determines if we insert all items 
                            'after the headers.  if any header is missing an item then
                            'we re-import all items 
                            mblnSomeItemsMissing = True
                        Else
                            blnNoMatchingItem = False
                        End If
                        'Check for alpha company compatibility (note the company field type will be changed to an integer on success)
                        If blnDataValidated Then blnDataValidated = validateCompany(oFields("PODefaultCustomer"),
                                                                                            strErrorMessage,
                                                                                            oCX,
                                                                                            strSource)

                        'Create a new item list object
                        Dim oItemList As New List(Of clsImportFields)

                        If blnDataValidated Then
                            'Lookup the Company Control Number if needed; but only if there are no validation errors
                            intCompControl = oCX.getControlByNumber(oFields("PODefaultCustomer").Value)
                            If intCompControl = 0 Then
                                intCompControl = Me.lookupCompControlByNumber(oFields("PODefaultCustomer").Value)
                                'now add the comp control to the collection this save future trips to the database
                                'by sending nothing as the alpha code any existing alpha code does not get replaced
                                oCX.AddNew(Nothing, oFields("PODefaultCustomer").Value, intCompControl)
                            End If
                            'Fill the Item Detail List
                            If Not blnNoMatchingItem Then
                                If Not filItemRecordList(oTheseDetails, oItemList) Then
                                    'we cannot read the item so set the module level items missing propery to true
                                    mblnSomeItemsMissing = True
                                End If
                            End If
                        End If
                        'Process the NO Lane Logic
                        'If blnDataValidated Then
                        '    If (Not Me.IgnoreMissingLanes(intCompControl)) Then
                        '        If Not doesLaneExist(oFields) Then
                        '            'Save record to No Lanes
                        '            saveNoLaneData(oRow, oTheseDetails)
                        '            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
                        '            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
                        '            GoTo GetNextHeaderRecord
                        '        End If
                        '    End If
                        'End If
                        'backup of original code: If blnDataValidated AndAlso (Not Me.IgnoreMissingLanes(intCompControl) OrElse Not doesLaneExist(oFields)) Then
                        'backup of code that does not work: If blnDataValidated AndAlso (oFields("POStatusFlag").ValueNoQuotes = "5" OrElse oFields("POStatusFlag").ValueNoQuotes = "6" OrElse (Not Me.IgnoreMissingLanes(intCompControl))) AndAlso (Not doesLaneExist(oFields)) Then
                        'Modified by RHR for v-7.0.6.105 - 6.0.4.7 corrected bug in business logic for when to save no lane data
                        'business rules
                        '1. data must be valid
                        '2. we only save no lane data (wait for lane to come in later) when the following are true
                        '   a) doesLaneExist returns false 
                        '   b) Me.IgnoreMissingLanes(intCompControl) = false
                        '3. POStatusFlag values are interpreted by doesLaneExist.  if the value is 5 or 6 
                        '   the system will attempt to create a new lane and if sucessful will return true
                        If blnDataValidated AndAlso (Me.IgnoreMissingLanes(intCompControl) = False And doesLaneExist(oFields) = False) Then
                            'Save record to No Lanes
                            saveNoLaneData(oRow, oTheseDetails)
                            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
                            TotalRecords += 1
                            GoTo GetNextHeaderRecord
                        End If
                        'test if the record already exists.
                        If blnDataValidated Then blnDataValidated = doesRecordExist(oFields,
                                                                                                                strErrorMessage,
                                                                                                                blnInsertRecord,
                                                                                                                "Order number " & oFields("PONumber").Value,
                                                                                                                "POHDR")
                        If Not blnDataValidated Then
                            addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                            RecordErrors += 1
                        Else
                            'add the order number to the list for silent tendering
                            If mstrOrderNumbers Is Nothing Then mstrOrderNumbers = New List(Of String)
                            mstrOrderNumbers.Add(stripQuotes(oFields("PONumber").Value))
                            'Save the changes to the main table
                            If saveData(oFields, blnInsertRecord, "POHDR", "POHDRCreateUser", "POHDRCreateDate") Then
                                'run the update defaults procedure
                                If Not updatePOHDRDefaults(oFields) Then GoTo GetNextHeaderRecord
                                TotalRecords += 1
                                mdblHashTotalOrders += Val(stripQuotes(oFields("PONumber").Value))
                                'Delete all existing item data
                                deleteItemData(oFields)
                                'Process Item Details but only if no missing items have been found for the entire batch
                                If Not mblnSomeItemsMissing Then
                                    Log("Importing " & oItemList.Count & " PO Item Records For Order Number" & oFields("PONumber").Value & ".")
                                    For Each oItems As clsImportFields In oItemList
                                        'We always do an insert because all previous detaisl were deleted
                                        If saveData(oItems, True, "POItem", "CreatedUser", "CreatedDate") Then
                                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
                                            'if it is true we wipe out the properties below and start over
                                            TotalItems += 1
                                            mintTotalQty += Val(oItems("QtyOrdered").Value)
                                            mdblTotalWeight += Val(oItems("Weight").Value)
                                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
                                        End If
                                    Next
                                End If
                                'If we get here we save the order history data
                                saveOrderHistory(oRow, oTheseDetails)
                            End If
                        End If
GetNextHeaderRecord:
                    Next
                    Return True
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, attempted to import PO Header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function
    ''' <summary>
    ''' 7.0.5 version of the Booking Header Imprort Procedure
    ''' </summary>
    ''' <param name="oOrders"></param>
    ''' <param name="oDetails"></param>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.104 on 4/3/2017 
    '''     added support for POStatusFlag of 6
    ''' </remarks>
    Private Function importHeaderRecords(
                ByRef oOrders As List(Of clsBookHeaderObject80),
                ByRef oDetails As List(Of clsBookDetailObject80),
                ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            If oOrders Is Nothing OrElse oOrders.Count < 1 Then
                If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty")
                Return False
            End If
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsBook.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Dim intCompNumber As Integer = 0
            Dim intCompControl As Integer = 0
            Dim blnNoMatchingItem As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                TotalItems = 0
                Try
                    Log("Importing " & oOrders.Count & " PO Header Records.")
                    For Each oRow As clsBookHeaderObject80 In oOrders
                        If Not oRow Is Nothing Then
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oFields("PODefaultCustomer").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("PODefaultCustomer").Length = 160
                            oFields("PODefaultCustomer").Null = True
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            'Get the item details for this order (must be called before we lookup the company alpha number so numbers still match)
                            Dim oTheseDetails As List(Of clsBookDetailObject80) = getItemDetails(oFields, oDetails)
                            If oTheseDetails Is Nothing OrElse oTheseDetails.Count < 1 Then
                                blnNoMatchingItem = True
                                'the modual level property determines if we insert all items 
                                'after the headers.  if any header is missing an item then
                                'we re-import all items 
                                mblnSomeItemsMissing = True
                            Else
                                blnNoMatchingItem = False
                            End If
                            'Check for alpha company compatibility (note the company field type will be changed to an integer on success)
                            If blnDataValidated Then blnDataValidated = validateCompany70(oFields("PODefaultCustomer"),
                                                                                                intCompControl,
                                                                                                strErrorMessage,
                                                                                                oCX,
                                                                                                strSource,
                                                                                                DTran.stripQuotes(oFields("POCompLegalEntity").Value),
                            DTran.stripQuotes(oFields("POCompAlphaCode").Value))

                            'Create a new item list object
                            Dim oItemList As New List(Of clsImportFields)
                            'Modified by RHR  v-7.0.5.100 5/31/2016 
                            'New variable for allow blank company on delete
                            If blnDataValidated = False AndAlso mBlnIgnoreValidationOnDelete = True AndAlso oFields("POStatusFlag").ValueNoQuotes = "2" Then
                                blnDataValidated = True
                                strErrorMessage = ""
                                Dim compField As clsImportField = oFields("PODefaultCustomer")
                                Dim lngInt As Long = 0
                                Int64.TryParse(compField.Value, lngInt)
                                compField.DataType = clsImportField.DataTypeID.gcvdtLongInt
                                compField.Length = 11
                                compField.Null = False
                                compField.Value = lngInt
                            Else
                                If blnDataValidated Then
                                    'Fill the Item Detail List
                                    If Not blnNoMatchingItem Then
                                        If Not filItemRecordList(oTheseDetails, oItemList) Then
                                            'we cannot read the item so set the module level items missing propery to true
                                            mblnSomeItemsMissing = True
                                        End If
                                    End If
                                End If
                                'Process the NO Lane Logic
                                'If blnDataValidated Then
                                '    If (Not Me.IgnoreMissingLanes(intCompControl)) Then
                                '        If Not doesLaneExist(oFields) Then
                                '            'Save record to No Lanes
                                '            saveNoLaneData(oRow, oTheseDetails)
                                '            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
                                '            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
                                '            GoTo GetNextHeaderRecord
                                '        End If
                                '    End If
                                'End If
                                'Modified by RHR for v-7.0.6.105 - 6.0.4.7 corrected bug in business logic for when to save no lane data
                                'business rules
                                '1. data must be valid
                                '2. we only save no lane data (wait for lane to come in later) when the following are true
                                '   a) doesLaneExist returns false 
                                '   b) Me.IgnoreMissingLanes(intCompControl) = false
                                '3. POStatusFlag values are interpreted by doesLaneExist.  if the value is 5 or 6 
                                '   the system will attempt to create a new lane and if sucessful will return true
                                If blnDataValidated AndAlso (Me.IgnoreMissingLanes(intCompControl) = False And doesLaneExist(oFields) = False) Then
                                    'Save record to No Lanes
                                    saveNoLaneData(oRow, oTheseDetails)
                                    ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
                                    Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
                                    TotalRecords += 1
                                    GoTo GetNextHeaderRecord
                                End If
                                'test if the record already exists.
                                If blnDataValidated Then blnDataValidated = doesRecordExist(oFields,
                                                                                                    strErrorMessage,
                                                                                                    blnInsertRecord,
                                                                                                    "Order number " & oFields("PONumber").Value,
                                                                                                    "POHDR")

                                If Not blnDataValidated Then
                                    addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                    RecordErrors += 1
                                Else
                                    'add the order number to the list for silent tendering
                                    If mstrOrderNumbers Is Nothing Then mstrOrderNumbers = New List(Of String)
                                    mstrOrderNumbers.Add(stripQuotes(oFields("PONumber").Value))
                                    'Save the changes to the main table
                                    If saveData(oFields, blnInsertRecord, "POHDR", "POHDRCreateUser", "POHDRCreateDate") Then
                                        'run the update defaults procedure
                                        If Not updatePOHDRDefaults(oFields) Then GoTo GetNextHeaderRecord
                                        TotalRecords += 1
                                        mdblHashTotalOrders += Val(stripQuotes(oFields("PONumber").Value))
                                        'Delete all existing item data
                                        deleteItemData(oFields)
                                        'Process Item Details but only if no missing items have been found for the entire batch
                                        If Not mblnSomeItemsMissing Then
                                            Log("Importing " & oItemList.Count & " PO Item Records For Order Number" & oFields("PONumber").Value & ".")
                                            For Each oItems As clsImportFields In oItemList
                                                'We always do an insert because all previous detaisl were deleted
                                                'Modified by RHR for v-8.3.0.003 on 02/01/2021
                                                ' We are getting a few duplicats so check and perform update if record exists
                                                Dim blnInsertItemDetails As Boolean = True
                                                'removed because this prevents any item from importing.  More testing is needed
                                                'Modified by RHR for v-8.3.0.003 on 02/12/2021
                                                'If doesRecordExist(oItems,
                                                '                    "Checking for duplicate Item details",
                                                '                    blnInsertItemDetails,
                                                '                   "POItem Record ",
                                                '                   "POItem") Then
                                                '    blnInsertItemDetails = False
                                                'End If
                                                If saveData(oItems, blnInsertItemDetails, "POItem", "CreatedUser", "CreatedDate") Then
                                                    'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
                                                    'if it is true we wipe out the properties below and start over
                                                    TotalItems += 1
                                                    mintTotalQty += Val(oItems("QtyOrdered").Value)
                                                    mdblTotalWeight += Val(oItems("Weight").Value)
                                                    mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
                                                End If
                                            Next
                                        End If
                                        'If we get here we save the order history data
                                        saveOrderHistory(oRow, oTheseDetails)
                                    End If
                                End If
                            End If
                        Else
                            AddToGroupEmailMsg("One of the order header records was null or empty and could not be processed.")
                        End If
GetNextHeaderRecord:
                    Next
                    Return True
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, attempted to import PO Header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importItemRecords(
        ByRef oDetails As BookData.BookDetailDataTable) As Boolean
        Dim Ret As Boolean = False
        Try
            Dim strSource As String = "clsBook.importItemRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Log("Importing " & oDetails.Count & " PO Detail Records.")
            Dim oItemList As New List(Of clsImportFields)
            filItemRecordList(oDetails, oItemList)
            If oItemList.Count < 1 Then
                'this should not happen so log the issue and send an email
                ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.!")
            Else
                'loop through each record and save the data
                For Each oItems As clsImportFields In oItemList
                    If Not doesRecordExist(oItems,
                                            strErrorMessage,
                                            blnInsertRecord,
                                            "PO Item number " & oItems("ItemNumber").Value,
                                            "POItem") Then
                        'log the error
                        addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                        ItemErrors += 1
                    Else
                        'save the data
                        If saveData(oItems, blnInsertRecord, "POItem", "CreatedUser", "CreatedDate") Then
                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
                            'if it is true we wipe out the properties below and start over
                            TotalItems += 1
                            mintTotalQty += Val(oItems("QtyOrdered").Value)
                            mdblTotalWeight += Val(oItems("Weight").Value)
                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
                        End If
                    End If
                Next
            End If
            Return True

        Catch ex As Exception
            Me.ItemErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importItemRecords; could not import PO Detail records.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.importItemRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importItemRecords(
        ByRef oDetails As List(Of clsBookDetailObject60)) As Boolean
        Dim Ret As Boolean = False
        Try
            Dim strSource As String = "clsBook.importItemRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Log("Importing " & oDetails.Count & " PO Detail Records.")
            Dim oItemList As New List(Of clsImportFields)
            filItemRecordList(oDetails, oItemList)
            If oItemList.Count < 1 Then
                'this should not happen so log the issue and send an email
                ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.!")
            Else
                'loop through each record and save the data
                For Each oItems As clsImportFields In oItemList
                    If Not doesRecordExist(oItems,
                                            strErrorMessage,
                                            blnInsertRecord,
                                            "PO Item number " & oItems("ItemNumber").Value,
                                            "POItem") Then
                        'log the error
                        addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                        ItemErrors += 1
                    Else
                        'save the data
                        If saveData(oItems, blnInsertRecord, "POItem", "CreatedUser", "CreatedDate") Then
                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
                            'if it is true we wipe out the properties below and start over
                            TotalItems += 1
                            mintTotalQty += Val(oItems("QtyOrdered").Value)
                            mdblTotalWeight += Val(oItems("Weight").Value)
                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
                        End If
                    End If
                Next
            End If
            Return True

        Catch ex As Exception
            Me.ItemErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importItemRecords; could not import PO Detail records.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.importItemRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    ''' <summary>
    ''' imports item details into the poitem table
    ''' </summary>
    ''' <param name="oDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR On 6/1/2017 For v-7.0.6.105
    '''    added support for 604 objects
    ''' </remarks>
    Private Function importItemRecords(
        ByRef oDetails As List(Of clsBookDetailObject604)) As Boolean
        Dim Ret As Boolean = False
        Try
            Dim strSource As String = "clsBook.importItemRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Log("Importing " & oDetails.Count & " PO Detail Records.")
            Dim oItemList As New List(Of clsImportFields)
            filItemRecordList(oDetails, oItemList)
            If oItemList.Count < 1 Then
                'this should not happen so log the issue and send an email
                ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.!")
            Else
                'loop through each record and save the data
                For Each oItems As clsImportFields In oItemList
                    If Not doesRecordExist(oItems,
                                            strErrorMessage,
                                            blnInsertRecord,
                                            "PO Item number " & oItems("ItemNumber").Value,
                                            "POItem") Then
                        'log the error
                        addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                        ItemErrors += 1
                    Else
                        'save the data
                        If saveData(oItems, blnInsertRecord, "POItem", "CreatedUser", "CreatedDate") Then
                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
                            'if it is true we wipe out the properties below and start over
                            TotalItems += 1
                            mintTotalQty += Val(oItems("QtyOrdered").Value)
                            mdblTotalWeight += Val(oItems("Weight").Value)
                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
                        End If
                    End If
                Next
            End If
            Return True

        Catch ex As Exception
            Me.ItemErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importItemRecords; could not import PO Detail records.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.importItemRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importItemRecords(
        ByRef oDetails As List(Of clsBookDetailObject80)) As Boolean
        Dim Ret As Boolean = False
        Try
            Dim strSource As String = "clsBook.importItemRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Log("Importing " & oDetails.Count & " PO Detail Records.")
            Dim oItemList As New List(Of clsImportFields)
            filItemRecordList(oDetails, oItemList)
            If oItemList.Count < 1 Then
                'this should not happen so log the issue and send an email
                ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.!")
            Else
                'loop through each record and save the data
                For Each oItems As clsImportFields In oItemList
                    If Not doesRecordExist(oItems,
                                            strErrorMessage,
                                            blnInsertRecord,
                                            "PO Item number " & oItems("ItemNumber").Value,
                                            "POItem") Then
                        'log the error
                        addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                        ItemErrors += 1
                    Else
                        'save the data
                        If saveData(oItems, blnInsertRecord, "POItem", "CreatedUser", "CreatedDate") Then
                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
                            'if it is true we wipe out the properties below and start over
                            TotalItems += 1
                            mintTotalQty += Val(oItems("QtyOrdered").Value)
                            mdblTotalWeight += Val(oItems("Weight").Value)
                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
                        End If
                    End If
                Next
            End If
            Return True

        Catch ex As Exception
            Me.ItemErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importItemRecords; could not import PO Detail records.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.importItemRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function filItemRecordList(
        ByRef oDetails As BookData.BookDetailDataTable,
        ByRef oItemList As List(Of clsImportFields)) As Boolean

        Dim strSource As String = "clsBook.filItemRecordList"
        Dim blnDataValidated As Boolean = False
        Dim strErrorMessage As String = ""
        Log("Building item list for  " & oDetails.Count & " PO Detail records.")
        For Each oRow As BookData.BookDetailRow In oDetails
            Dim oItems As New clsImportFields
            If Not buildItemCollection(oItems) Then Return False
            'Check if a customernumber is provided.  If not we change the logic to ignore the customernumber
            If IsDBNull(oRow.Item("CustomerNumber")) Then
                oItems("CustomerNumber").Null = True
                oItems("CustomerNumber").PK = clsImportField.PKValue.gcNK
            End If
            strErrorMessage = ""
            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
            If blnDataValidated AndAlso Not IsDBNull(oRow.Item("CustomerNumber")) Then blnDataValidated = validateCompany(oItems("CustomerNumber"),
                                                                                strErrorMessage,
                                                                                oCX,
                                                                                strSource)
            If Not blnDataValidated Then
                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                ItemErrors += 1
            Else
                'add the item to the collection
                oItemList.Add(oItems)
            End If
        Next
        Return True


    End Function

    Private Function filItemRecordList(
        ByRef oDetails As List(Of clsBookDetailObject60),
        ByRef oItemList As List(Of clsImportFields)) As Boolean

        Dim strSource As String = "clsBook.filItemRecordList"
        Dim blnDataValidated As Boolean = False
        Dim strErrorMessage As String = ""
        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
            Log("No item list records are available.")
            Return False
        End If
        If oItemList Is Nothing Then oItemList = New List(Of clsImportFields)
        Log("Building item list for  " & oDetails.Count & " PO Detail records.")
        For Each oRow As clsBookDetailObject60 In oDetails
            Dim oItems As New clsImportFields
            If Not buildItemCollection60(oItems) Then Return False
            'Check if a customernumber is provided.  If not we change the logic to ignore the customernumber
            If IsDBNull(oRow.Item("CustomerNumber")) Then
                oItems("CustomerNumber").Null = True
                oItems("CustomerNumber").PK = clsImportField.PKValue.gcNK
            End If
            strErrorMessage = ""
            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
            If blnDataValidated AndAlso Not IsDBNull(oRow.Item("CustomerNumber")) Then blnDataValidated = validateCompany(oItems("CustomerNumber"),
                                                                                strErrorMessage,
                                                                                oCX,
                                                                                strSource)
            If Not blnDataValidated Then
                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                ItemErrors += 1
            Else
                'add the item to the collection
                oItemList.Add(oItems)
            End If
        Next
        Return True


    End Function

    ''' <summary>
    ''' Fill import fields with 604 item data
    ''' </summary>
    ''' <param name="oDetails"></param>
    ''' <param name="oItemList"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 6/1/2017 for v-7.0.6.105
    '''    added support for 604 items
    ''' </remarks>
    Private Function filItemRecordList(
        ByRef oDetails As List(Of clsBookDetailObject604),
        ByRef oItemList As List(Of clsImportFields)) As Boolean

        Dim strSource As String = "clsBook.filItemRecordList"
        Dim blnDataValidated As Boolean = False
        Dim strErrorMessage As String = ""
        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
            Log("No item list records are available.")
            Return False
        End If
        If oItemList Is Nothing Then oItemList = New List(Of clsImportFields)
        Log("Building item list for  " & oDetails.Count & " PO Detail records.")
        For Each oRow As clsBookDetailObject604 In oDetails
            Dim oItems As New clsImportFields
            If Not buildItemCollection604(oItems) Then Return False
            'Check if a customernumber is provided.  If not we change the logic to ignore the customernumber
            If IsDBNull(oRow.Item("CustomerNumber")) Then
                oItems("CustomerNumber").Null = True
                oItems("CustomerNumber").PK = clsImportField.PKValue.gcNK
            End If
            strErrorMessage = ""
            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
            If blnDataValidated AndAlso Not IsDBNull(oRow.Item("CustomerNumber")) Then blnDataValidated = validateCompany(oItems("CustomerNumber"),
                                                                                strErrorMessage,
                                                                                oCX,
                                                                                strSource)
            If Not blnDataValidated Then
                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                ItemErrors += 1
            Else
                'add the item to the collection
                oItemList.Add(oItems)
            End If
        Next
        Return True


    End Function


    ''' <summary>
    ''' Process PO Item Details
    ''' </summary>
    ''' <param name="oDetails"></param>
    ''' <param name="oItemList"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR on 04/05/2019 for v-8.2
    '''   support new 80 object and fields
    ''' Modified by RHR for v-8.2 on 5/9/2019
    '''     add logic to process POItemOrderNumber and validate that this as a
    '''     possible primary key value.  changed the logic to test for blank or missing company 
    '''     information,  IsDBNull is no longer valid for standard book item objects     
    ''' </remarks>
    Private Function filItemRecordList(
        ByRef oDetails As List(Of clsBookDetailObject80),
        ByRef oItemList As List(Of clsImportFields)) As Boolean

        Dim strSource As String = "clsBook.filItemRecordList"
        Dim blnDataValidated As Boolean = False
        Dim strErrorMessage As String = ""
        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
            Log("No item list records are available.")
            Return False
        End If
        If oItemList Is Nothing Then oItemList = New List(Of clsImportFields)
        Log("Building item list for  " & oDetails.Count & " PO Detail records.")
        'Modified by RHR for v-7.0.6.104 on 8/12/2017
        'new code to manage error messages for company validation
        Dim sCompValidationErrors As New List(Of String)
        For Each oRow As clsBookDetailObject80 In oDetails
            Dim oItems As New clsImportFields
            Dim intCompControl As Integer = 0
            If Not buildItemCollection80(oItems) Then Return False

            'Check if a customernumber is provided.  If not we change the logic to ignore the customernumber
            If oRow.CustomerNumber Is Nothing OrElse String.IsNullOrWhiteSpace(oRow.CustomerNumber) Then
                oItems("CustomerNumber").Null = True
                oItems("CustomerNumber").PK = clsImportField.PKValue.gcNK
            End If
            'Check if the POItemOrderNumber is provided. if not we change the logic to ignore the POItemOrderNumber as a key field
            If oRow.POItemOrderNumber Is Nothing OrElse String.IsNullOrWhiteSpace(oRow.POItemOrderNumber) Then
                oItems("POItemOrderNumber").Null = True
                oItems("POItemOrderNumber").PK = clsImportField.PKValue.gcNK
            End If
            strErrorMessage = ""
            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
            'Check for alpha company compatibility (note the company field type will be changed to an integer on success)
            If blnDataValidated Then
                Dim strCompValErr As String = ""
                If validateCompany70(oItems("CustomerNumber"),
                                     intCompControl,
                                     strCompValErr,
                                     oCX,
                                     strSource,
                                     DTran.stripQuotes(oItems("POItemCompLegalEntity").Value),
                                     DTran.stripQuotes(oItems("POItemCompAlphaCode").Value)) Then
                    oItems("CustomerNumber").PK = clsImportField.PKValue.gcPK
                Else
                    'Modified by RHR for v-7.0.6.104 on 8/12/2017
                    'new code to manage error messages for company validation
                    If Not String.IsNullOrEmpty(strCompValErr) Then
                        If Not sCompValidationErrors.Contains(strCompValErr) Then
                            'add to strErrorMessage and sCompValidationErrors list
                            sCompValidationErrors.Add(strCompValErr)
                            strErrorMessage &= " " & strCompValErr
                            blnDataValidated = False
                        End If
                    End If

                End If

            End If


            If Not blnDataValidated Then
                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                ItemErrors += 1
            Else
                'add the item to the collection
                oItemList.Add(oItems)
            End If
        Next
        Return True


    End Function

    Private Sub saveNoLaneData(ByRef oHeaderRow As BookData.BookHeaderRow, ByRef oDetails As BookData.BookDetailDataTable)
        If oHeaderRow Is Nothing Then Return
        Dim intNewPOHNoLaneID As Integer
        Dim strNewPOHNoLaneID As String
        Dim strOrderNumber As String = DTran.NZ(oHeaderRow, "PONumber", "")
        Dim strSQL As String = "Exec dbo.spAddPOHNoLane " _
            & "'" & Me.AuthorizationCode & "'" _
            & DTran.buildSQLString(oHeaderRow, "POCustomerPO", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POvendor", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POdate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POShipdate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POBuyer", "", ",") _
            & "," & DTran.NZ(oHeaderRow, "POFrt", 0) _
            & ",'" & CreateUser & "'" _
            & ",'" & CreatedDate & "'" _
            & ",'" & CreateUser & "'" _
            & "," & DTran.NZ(oHeaderRow, "POTotalFrt", 0) _
            & "," & DTran.NZ(oHeaderRow, "POTotalCost", 0) _
            & "," & DTran.NZ(oHeaderRow, "POWgt", 0) _
            & "," & DTran.NZ(oHeaderRow, "POCube", 0) _
            & "," & DTran.NZ(oHeaderRow, "POQty", 0) _
            & "," & DTran.NZ(oHeaderRow, "POLines", 0) _
            & "," & DTran.NZ(oHeaderRow, "POConfirm", 0) _
            & "," & DTran.NZ(oHeaderRow, "PODefaultCustomer", 0) _
            & ",''" _
            & "," & DTran.NZ(oHeaderRow, "PODefaultCarrier", 0) _
            & DTran.buildSQLString(oHeaderRow, "POReqDate", "", ",") _
            & ",'" & strOrderNumber & "'" _
            & DTran.buildSQLString(oHeaderRow, "POShipInstructions", "", ",") _
            & "," & DTran.NZ(oHeaderRow, "POCooler", 0) _
            & "," & DTran.NZ(oHeaderRow, "POFrozen", 0) _
            & "," & DTran.NZ(oHeaderRow, "PODry", 0) _
            & DTran.buildSQLString(oHeaderRow, "POTemp", "", ",") _
            & ",'','','','','','','','',''" _
            & DTran.buildSQLString(oHeaderRow, "POCarType", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POShipVia", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POShipViaType", "", ",") _
            & "," & DTran.NZ(oHeaderRow, "POPallets", 0) _
            & "," & DTran.NZ(oHeaderRow, "POOtherCosts", 0) _
            & "," & DTran.NZ(oHeaderRow, "POStatusFlag", 0) _
            & ",0,'',0" _
            & "," & DTran.NZ(oHeaderRow, "POOrderSequence", 0) _
            & DTran.buildSQLString(oHeaderRow, "POChepGLID", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POCarrierEquipmentCodes", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POCarrierTypeCode", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POPalletPositions", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POSchedulePUDate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POSchedulePUTime", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POScheduleDelDate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POSCheduleDelTime", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POActPUDate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POActPUTime", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POActDelDate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POActDelTime", "", ",")
        strNewPOHNoLaneID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData")
        If Not Integer.TryParse(strNewPOHNoLaneID, intNewPOHNoLaneID) Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save no lane PO Header records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
                & vbCrLf & strSQL & "<hr />" & vbCrLf
            Return 'We cannot continue   
        End If
        Dim sItemHistoryCommands As New List(Of String)
        Dim sItemPONumbers As New List(Of String)
        Dim sItemNumbers As New List(Of String)
        If intNewPOHNoLaneID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Rows.Count > 0 Then
            'Loop through each item detail record for this order and build an add query            
            For Each oRow As BookData.BookDetailRow In oDetails
                strSQL = "Exec dbo.spAddPOINoLane " _
                    & intNewPOHNoLaneID _
                    & ",'" & Me.AuthorizationCode & "'" _
                    & DTran.buildSQLString(oRow, "ItemPONumber", "", ",") _
                    & "," & DTran.NZ(oRow, "FixOffInvAllow", 0) _
                    & "," & DTran.NZ(oRow, "FixFrtAllow", 0) _
                    & DTran.buildSQLString(oRow, "ItemNumber", "", ",") _
                    & "," & DTran.NZ(oRow, "QtyOrdered", 0) _
                    & "," & DTran.NZ(oRow, "FreightCost", 0) _
                    & "," & DTran.NZ(oRow, "ItemCost", 0) _
                    & "," & DTran.NZ(oRow, "Weight", 0) _
                    & "," & DTran.NZ(oRow, "Cube", 0) _
                    & "," & DTran.NZ(oRow, "Pack", 0) _
                    & DTran.buildSQLString(oRow, "Size", "", ",") _
                    & DTran.buildSQLString(oRow, "Description", "", ",") _
                    & DTran.buildSQLString(oRow, "Hazmat", "", ",") _
                    & ",'" & CreateUser & "'" _
                    & ",'" & CreatedDate & "'" _
                    & DTran.buildSQLString(oRow, "Brand", "", ",") _
                    & DTran.buildSQLString(oRow, "CostCenter", "", ",") _
                    & DTran.buildSQLString(oRow, "LotNumber", "", ",") _
                    & DTran.buildSQLString(oRow, "LotExpirationDate", "", ",") _
                    & DTran.buildSQLString(oRow, "GTIN", "", ",") _
                    & DTran.buildSQLString(oRow, "CustItemNumber", "", ",") _
                    & "," & DTran.NZ(oRow, "CustomerNumber", 0) _
                    & "," & DTran.NZ(oRow, "POOrderSequence", 0) _
                    & DTran.buildSQLString(oRow, "PalletType", "", ",")
                sItemHistoryCommands.Add(strSQL)
                If Not sItemPONumbers.Contains(DTran.NZ(oRow, "ItemPONumber", "")) Then sItemPONumbers.Add(DTran.NZ(oRow, "ItemPONumber", ""))
                If Not sItemNumbers.Contains(DTran.NZ(oRow, "ItemNumber", "")) Then sItemNumbers.Add(DTran.NZ(oRow, "ItemNumber", ""))
            Next
            'Build the delete query
            strSQL = "Delete From dbo.POINoLanes Where POIPOHNLControl = " & intNewPOHNoLaneID
            Dim blnUseOr As Boolean = False
            Dim sSpacer As String = ""
            If sItemPONumbers.Count > 0 Then
                blnUseOr = True
                strSQL &= " AND (ItemPONumber NOT IN ("
                For Each s As String In sItemPONumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                strSQL &= ") "
            End If
            If sItemNumbers.Count > 0 Then
                If blnUseOr Then
                    strSQL &= " OR ItemNumber NOT IN ("
                Else
                    strSQL &= " AND ItemNumber NOT IN ("
                End If
                sSpacer = ""
                For Each s As String In sItemNumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                If blnUseOr Then
                    strSQL &= "))"
                Else
                    strSQL &= ")"
                End If
            ElseIf blnUseOr Then
                strSQL &= ")"
            End If
            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to delete existing POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
            End If
            'Now execute each add item commands
            For Each s As String In sItemHistoryCommands
                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save new POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
                Else
                    TotalItems += 1
                End If
            Next
        End If

    End Sub

    Private Sub saveNoLaneData(ByRef oHeaderRow As clsBookHeaderObject60, ByRef oDetails As List(Of clsBookDetailObject60))
        If oHeaderRow Is Nothing Then Return
        Dim intNewPOHNoLaneID As Integer
        Dim strNewPOHNoLaneID As String
        Dim strOrderNumber As String = oHeaderRow.PONumber
        Dim strSQL As String = "Exec dbo.spAddPOHNoLane " _
            & "'" & Me.AuthorizationCode & "'" _
            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
            & "," & oHeaderRow.POFrt _
            & ",'" & CreateUser & "'" _
            & ",'" & CreatedDate & "'" _
            & ",'" & CreateUser & "'" _
            & "," & oHeaderRow.POTotalFrt _
            & "," & oHeaderRow.POTotalCost _
            & "," & oHeaderRow.POWgt _
            & "," & oHeaderRow.POCube _
            & "," & oHeaderRow.POQty _
            & "," & oHeaderRow.POLines _
            & "," & If(oHeaderRow.POConfirm, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
            & ",''" _
            & "," & oHeaderRow.PODefaultCarrier _
            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
            & ",'" & strOrderNumber & "'" _
            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
            & "," & If(oHeaderRow.POCooler, "1", "0") _
            & "," & If(oHeaderRow.POFrozen, "1", "0") _
            & "," & If(oHeaderRow.PODry, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
            & ",'','','','','','','','',''" _
            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
            & "," & oHeaderRow.POPallets _
            & "," & oHeaderRow.POOtherCosts _
            & "," & oHeaderRow.POStatusFlag _
            & ",0,'',0" _
            & "," & oHeaderRow.POOrderSequence _
            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
            & "," & If(oHeaderRow.POInbound, "1", "0") _
            & "," & oHeaderRow.PODefaultRouteSequence _
            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")


        strNewPOHNoLaneID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData")
        If Not Integer.TryParse(strNewPOHNoLaneID, intNewPOHNoLaneID) Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save no lane PO Header records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
                & vbCrLf & strSQL & "<hr />" & vbCrLf
            Return 'We cannot continue   
        End If
        Dim sItemHistoryCommands As New List(Of String)
        Dim sItemPONumbers As New List(Of String)
        Dim sItemNumbers As New List(Of String)
        If intNewPOHNoLaneID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
            'Loop through each item detail record for this order and build an add query            
            For Each oRow As clsBookDetailObject60 In oDetails
                strSQL = "Exec dbo.spAddPOINoLane " _
                    & intNewPOHNoLaneID _
                    & ",'" & Me.AuthorizationCode & "'" _
                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
                    & "," & oRow.FixOffInvAllow _
                    & "," & oRow.FixFrtAllow _
                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
                    & "," & oRow.QtyOrdered _
                    & "," & oRow.FreightCost _
                    & "," & oRow.ItemCost _
                    & "," & oRow.Weight _
                    & "," & oRow.Cube _
                    & "," & oRow.Pack _
                    & DTran.buildSQLString(oRow.Size, 255, ",") _
                    & DTran.buildSQLString(oRow.Description, 255, ",") _
                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
                    & ",'" & CreateUser & "'" _
                    & ",'" & CreatedDate & "'" _
                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
                    & "," & oRow.CustomerNumber _
                    & "," & oRow.POOrderSequence _
                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
                    & "," & oRow.POItemPallets _
                    & "," & oRow.POItemTies _
                    & "," & oRow.POItemHighs _
                    & "," & oRow.POItemQtyPalletPercentage _
                    & "," & oRow.POItemQtyLength _
                    & "," & oRow.POItemQtyWidth _
                    & "," & oRow.POItemQtyHeight _
                    & "," & If(oRow.POItemStackable, "1", "0") _
                    & "," & oRow.POItemLevelOfDensity

                sItemHistoryCommands.Add(strSQL)
                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
            Next
            'Build the delete query
            strSQL = "Delete From dbo.POINoLanes Where POIPOHNLControl = " & intNewPOHNoLaneID
            Dim blnUseOr As Boolean = False
            Dim sSpacer As String = ""
            If sItemPONumbers.Count > 0 Then
                blnUseOr = True
                strSQL &= " AND (ItemPONumber NOT IN ("
                For Each s As String In sItemPONumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                strSQL &= ") "
            End If
            If sItemNumbers.Count > 0 Then
                If blnUseOr Then
                    strSQL &= " OR ItemNumber NOT IN ("
                Else
                    strSQL &= " AND ItemNumber NOT IN ("
                End If
                sSpacer = ""
                For Each s As String In sItemNumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                If blnUseOr Then
                    strSQL &= "))"
                Else
                    strSQL &= ")"
                End If
            ElseIf blnUseOr Then
                strSQL &= ")"
            End If
            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to delete existing POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
            End If
            'Now execute each add item commands
            For Each s As String In sItemHistoryCommands
                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save new POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
                Else
                    TotalItems += 1
                End If
            Next
        End If

    End Sub



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oHeaderRow"></param>
    ''' <param name="oDetails"></param>
    ''' <remarks>
    ''' Modified by RHR On 6/1/2017 For v-7.0.6.105
    '''    added support for 604 objects  
    ''' </remarks>
    Private Sub saveNoLaneData(ByRef oHeaderRow As clsBookHeaderObject604, ByRef oDetails As List(Of clsBookDetailObject604))
        If oHeaderRow Is Nothing Then Return
        Dim intNewPOHNoLaneID As Integer
        Dim strNewPOHNoLaneID As String
        Dim strOrderNumber As String = oHeaderRow.PONumber
        Dim strSQL As String = "Exec dbo.spAddPOHNoLane " _
            & "'" & Me.AuthorizationCode & "'" _
            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
            & "," & oHeaderRow.POFrt _
            & ",'" & CreateUser & "'" _
            & ",'" & CreatedDate & "'" _
            & ",'" & CreateUser & "'" _
            & "," & oHeaderRow.POTotalFrt _
            & "," & oHeaderRow.POTotalCost _
            & "," & oHeaderRow.POWgt _
            & "," & oHeaderRow.POCube _
            & "," & oHeaderRow.POQty _
            & "," & oHeaderRow.POLines _
            & "," & If(oHeaderRow.POConfirm, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
            & ",''" _
            & "," & oHeaderRow.PODefaultCarrier _
            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
            & ",'" & strOrderNumber & "'" _
            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
            & "," & If(oHeaderRow.POCooler, "1", "0") _
            & "," & If(oHeaderRow.POFrozen, "1", "0") _
            & "," & If(oHeaderRow.PODry, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
            & ",'','','','','','','','',''" _
            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
            & "," & oHeaderRow.POPallets _
            & "," & oHeaderRow.POOtherCosts _
            & "," & oHeaderRow.POStatusFlag _
            & ",0,'',0" _
            & "," & oHeaderRow.POOrderSequence _
            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
            & "," & If(oHeaderRow.POInbound, "1", "0") _
            & "," & oHeaderRow.PODefaultRouteSequence _
            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")


        strNewPOHNoLaneID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData")
        If Not Integer.TryParse(strNewPOHNoLaneID, intNewPOHNoLaneID) Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save no lane PO Header records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
                & vbCrLf & strSQL & "<hr />" & vbCrLf
            Return 'We cannot continue   
        End If
        Dim sItemHistoryCommands As New List(Of String)
        Dim sItemPONumbers As New List(Of String)
        Dim sItemNumbers As New List(Of String)
        If intNewPOHNoLaneID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
            'Loop through each item detail record for this order and build an add query            
            For Each oRow As clsBookDetailObject60 In oDetails
                strSQL = "Exec dbo.spAddPOINoLane " _
                    & intNewPOHNoLaneID _
                    & ",'" & Me.AuthorizationCode & "'" _
                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
                    & "," & oRow.FixOffInvAllow _
                    & "," & oRow.FixFrtAllow _
                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
                    & "," & oRow.QtyOrdered _
                    & "," & oRow.FreightCost _
                    & "," & oRow.ItemCost _
                    & "," & oRow.Weight _
                    & "," & oRow.Cube _
                    & "," & oRow.Pack _
                    & DTran.buildSQLString(oRow.Size, 255, ",") _
                    & DTran.buildSQLString(oRow.Description, 255, ",") _
                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
                    & ",'" & CreateUser & "'" _
                    & ",'" & CreatedDate & "'" _
                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
                    & "," & oRow.CustomerNumber _
                    & "," & oRow.POOrderSequence _
                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
                    & "," & oRow.POItemPallets _
                    & "," & oRow.POItemTies _
                    & "," & oRow.POItemHighs _
                    & "," & oRow.POItemQtyPalletPercentage _
                    & "," & oRow.POItemQtyLength _
                    & "," & oRow.POItemQtyWidth _
                    & "," & oRow.POItemQtyHeight _
                    & "," & If(oRow.POItemStackable, "1", "0") _
                    & "," & oRow.POItemLevelOfDensity

                sItemHistoryCommands.Add(strSQL)
                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
            Next
            'Build the delete query
            strSQL = "Delete From dbo.POINoLanes Where POIPOHNLControl = " & intNewPOHNoLaneID
            Dim blnUseOr As Boolean = False
            Dim sSpacer As String = ""
            If sItemPONumbers.Count > 0 Then
                blnUseOr = True
                strSQL &= " AND (ItemPONumber NOT IN ("
                For Each s As String In sItemPONumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                strSQL &= ") "
            End If
            If sItemNumbers.Count > 0 Then
                If blnUseOr Then
                    strSQL &= " OR ItemNumber NOT IN ("
                Else
                    strSQL &= " AND ItemNumber NOT IN ("
                End If
                sSpacer = ""
                For Each s As String In sItemNumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                If blnUseOr Then
                    strSQL &= "))"
                Else
                    strSQL &= ")"
                End If
            ElseIf blnUseOr Then
                strSQL &= ")"
            End If
            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to delete existing POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
            End If
            'Now execute each add item commands
            For Each s As String In sItemHistoryCommands
                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save new POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
                Else
                    TotalItems += 1
                End If
            Next
        End If

    End Sub


    Private Sub saveNoLaneData(ByRef oHeaderRow As clsBookHeaderObject80, ByRef oDetails As List(Of clsBookDetailObject80))
        If oHeaderRow Is Nothing Then Return
        Dim intNewPOHNoLaneID As Integer
        Dim strNewPOHNoLaneID As String
        Dim strOrderNumber As String = oHeaderRow.PONumber
        Dim strSQL As String = "Exec dbo.spAddPOHNoLane " _
            & "'" & Me.AuthorizationCode & "'" _
            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
            & "," & oHeaderRow.POFrt _
            & ",'" & CreateUser & "'" _
            & ",'" & CreatedDate & "'" _
            & ",'" & CreateUser & "'" _
            & "," & oHeaderRow.POTotalFrt _
            & "," & oHeaderRow.POTotalCost _
            & "," & oHeaderRow.POWgt _
            & "," & oHeaderRow.POCube _
            & "," & oHeaderRow.POQty _
            & "," & oHeaderRow.POLines _
            & "," & If(oHeaderRow.POConfirm, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
            & ",''" _
            & "," & oHeaderRow.PODefaultCarrier _
            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
            & ",'" & strOrderNumber & "'" _
            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
            & "," & If(oHeaderRow.POCooler, "1", "0") _
            & "," & If(oHeaderRow.POFrozen, "1", "0") _
            & "," & If(oHeaderRow.PODry, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
            & ",'','','','','','','','',''" _
            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
            & "," & oHeaderRow.POPallets _
            & "," & oHeaderRow.POOtherCosts _
            & "," & oHeaderRow.POStatusFlag _
            & ",0,'',0" _
            & "," & oHeaderRow.POOrderSequence _
            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
            & "," & If(oHeaderRow.POInbound, "1", "0") _
            & "," & oHeaderRow.PODefaultRouteSequence _
            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")


        strNewPOHNoLaneID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData")
        If Not Integer.TryParse(strNewPOHNoLaneID, intNewPOHNoLaneID) Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save no lane PO Header records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
                & vbCrLf & strSQL & "<hr />" & vbCrLf
            Return 'We cannot continue   
        End If
        Dim sItemHistoryCommands As New List(Of String)
        Dim sItemPONumbers As New List(Of String)
        Dim sItemNumbers As New List(Of String)
        If intNewPOHNoLaneID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
            'Loop through each item detail record for this order and build an add query            
            For Each oRow As clsBookDetailObject80 In oDetails
                strSQL = "Exec dbo.spAddPOINoLane " _
                    & intNewPOHNoLaneID _
                    & ",'" & Me.AuthorizationCode & "'" _
                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
                    & "," & oRow.FixOffInvAllow _
                    & "," & oRow.FixFrtAllow _
                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
                    & "," & oRow.QtyOrdered _
                    & "," & oRow.FreightCost _
                    & "," & oRow.ItemCost _
                    & "," & oRow.Weight _
                    & "," & oRow.Cube _
                    & "," & oRow.Pack _
                    & DTran.buildSQLString(oRow.Size, 255, ",") _
                    & DTran.buildSQLString(oRow.Description, 255, ",") _
                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
                    & ",'" & CreateUser & "'" _
                    & ",'" & CreatedDate & "'" _
                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
                    & "," & oRow.CustomerNumber _
                    & "," & oRow.POOrderSequence _
                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
                    & "," & oRow.POItemPallets _
                    & "," & oRow.POItemTies _
                    & "," & oRow.POItemHighs _
                    & "," & oRow.POItemQtyPalletPercentage _
                    & "," & oRow.POItemQtyLength _
                    & "," & oRow.POItemQtyWidth _
                    & "," & oRow.POItemQtyHeight _
                    & "," & If(oRow.POItemStackable, "1", "0") _
                    & "," & oRow.POItemLevelOfDensity

                sItemHistoryCommands.Add(strSQL)
                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
            Next
            'Build the delete query
            strSQL = "Delete From dbo.POINoLanes Where POIPOHNLControl = " & intNewPOHNoLaneID
            Dim blnUseOr As Boolean = False
            Dim sSpacer As String = ""
            If sItemPONumbers.Count > 0 Then
                blnUseOr = True
                strSQL &= " AND (ItemPONumber NOT IN ("
                For Each s As String In sItemPONumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                strSQL &= ") "
            End If
            If sItemNumbers.Count > 0 Then
                If blnUseOr Then
                    strSQL &= " OR ItemNumber NOT IN ("
                Else
                    strSQL &= " AND ItemNumber NOT IN ("
                End If
                sSpacer = ""
                For Each s As String In sItemNumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                If blnUseOr Then
                    strSQL &= "))"
                Else
                    strSQL &= ")"
                End If
            ElseIf blnUseOr Then
                strSQL &= ")"
            End If
            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to delete existing POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
            End If
            'Now execute each add item commands
            For Each s As String In sItemHistoryCommands
                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save new POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
                Else
                    TotalItems += 1
                End If
            Next
        End If

    End Sub


    ''' <summary>
    ''' Check for existing lane and if it cannot be found determine if we can create a new lane.
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 01/09/2016 for v-7.0.5
    '''   Added logic to call createNewLaneFromPOData if the POStatusFlag is 5
    '''   and a lane does not exist
    ''' Modified by RHR for v-7.0.5.102 on 10/18/2016
    '''   We now set the POStatusFlag to zero after we process the 5 so that write new booking will use the Lane Data
    '''   for addresses.  This means that alternate shipping address is not supported when lanes are created dynamically
    ''' Modified by RHR for v-7.0.6.104 on 4/3/2017
    '''     added support for POStatusFlag of 6
    '''     Difference between 5 and 6:
    '''     5.  If POStatusFlag = 5 the system will create a new lane if it does not exist but the lane address validation rules will apply where the Lane Orig and Dest address will be used on additional order as they are transmitted to TMS
    '''     6.  If POStatusFlag = 6 the system will create a new lane if it does not exists and it will also use alternate shipping address information when additional order informaiton is different than the lane.
    '''     NOTE: Lane Nunber Genration Rules are the same for both 5 and 6.  The generated Lane Number will be used to determien when a new lane should be created,  not the address information.
    ''' Modified by RHR for v-7.0.6.105 - 6.0.4.7 we now test the POStatusFlag before creating the new lane it must be 5 or 6     
    ''' </remarks>
    Private Function doesLaneExist(ByRef oFields As clsImportFields) As Boolean
        Dim blnRet As Boolean = False
        If oFields Is Nothing Then Return False
        'Check the list in memory for previously processed lanes
        Dim intLaneControl As Integer = oEL.getControlByNumber(oFields("POvendor").Value)
        If intLaneControl = 0 Then
            Dim strSQL As String = "Select top 1 LaneControl From dbo.Lane Where LaneNumber = " & oFields("POvendor").Value
            Dim strLaneControl As String = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.doesLaneExist")
            If Integer.TryParse(strLaneControl, intLaneControl) Then
                If intLaneControl > 0 Then
                    'save the lane control to the collection in memory for future use to reduce trips to the database
                    oEL.AddNew(oFields("POvendor").Value, intLaneControl)
                    blnRet = True
                End If
            End If
        Else
            blnRet = True
        End If
        If blnRet Then
            Try
                deleteExistingNOLaneRecords(oFields)
            Catch ex As Exception
                'ignore any errors when cleaning up old records 
            End Try
        ElseIf Trim(oFields("POStatusFlag").ValueNoQuotes) = "5" Or Trim(oFields("POStatusFlag").ValueNoQuotes) = "6" Then
            'Modified by RHR for v-7.0.6.105 - 6.0.4.7 we now test the POStatusFlag before creating the new lane it must be 5 or 6
            blnRet = createNewLaneFromPOData(oFields)
        End If
        'Add Code to check oFields("POStatusFlag").ValueNoQuotes = "5" we update to "4" in case the shipping addresses change
        'Modified by RHR for v-7.0.5.102 on 10/18/2016
        ' We now set the POStatusFlag to zero after we process the 5 so that write new booking will use the Lane Data
        ' for addresses.  This means that alternate shipping address is not supported when lanes are created dynamically
        ' Modified by RHR for v-7.0.6.104 on 4/3/2017
        '  added support for POStatusFlag of 6
        'old code removed 4/3/2017
        'If blnRet And Trim(oFields("POStatusFlag").ValueNoQuotes) = "5" Then oFields("POStatusFlag").Value = "'0'"
        'begin new code 4/3/2017
        If blnRet Then
            If Trim(oFields("POStatusFlag").ValueNoQuotes) = "5" Then
                oFields("POStatusFlag").Value = "'0'"
            ElseIf Trim(oFields("POStatusFlag").ValueNoQuotes) = "6" Then
                oFields("POStatusFlag").Value = "'4'"
            End If
        End If
        'end new code 4/3/2017
        Return blnRet

    End Function

    ''' <summary>
    ''' Reads the first CompAbrev from the Comp table that matches, first by CompNumber then by CompAlphaCode.
    ''' CompNumber values of zero are ignored and only the CompAlphaCode is used.
    ''' If both value fail to return a result the optional sDefault parameter value is returned
    ''' All errors are encapsulated within the method but an optional ByRef string parameter sErrors is provided;
    ''' It will contain the details about any error messages associated with reading the data.
    ''' </summary>
    ''' <param name="compNumber"></param>
    ''' <param name="compAlphaCode"></param>
    ''' <param name="sDefault"></param>
    ''' <param name="sErrors"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 01/09/2016 for v-7.0.5
    ''' </remarks>
    Public Function GetCompAbrevByNumberOrAlpha(ByVal compNumber As Integer, ByVal compAlphaCode As String, Optional sDefault As String = "", Optional ByRef sErrors As String = "") As String
        Dim strRet As String = sDefault
        Try
            Dim oWCFParameters = WCFDataProperties.ConvertToWCFProperties(New DAL.WCFParameters())
            Dim oCompWCFData = New DAL.NGLCompData(oWCFParameters)
            strRet = oCompWCFData.GetCompAbrevByNumberOrAlpha(compNumber, compAlphaCode, sDefault)

        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            sErrors = sqlEx.Detail.ToString(sqlEx.Reason.ToString())
        Catch ex As Exception
            sErrors = ex.Message
        End Try

        Return strRet
    End Function


    ''' <summary>
    ''' Get company level parameter using multiple key values,  returns the first match found starting with company number.
    ''' </summary>
    ''' <param name="sParKey"></param>
    ''' <param name="sCompNumber"></param>
    ''' <param name="sCompAlpha"></param>
    ''' <param name="sCompLegalEntity"></param>
    ''' <param name="sErrors"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 on 09/07/2017
    '''  new parameter lookup which attempts to find the compcontrol using various filters
    '''  in the following order of precedence: compNumber, compalphacode and legal entity combined 
    ''' </remarks>
    Public Function GetParValueUsingOrderData(ByVal sParKey As String, ByVal sCompNumber As String, ByVal sCompAlpha As String, ByVal sCompLegalEntity As String, ByRef sErrors As String) As Double
        Dim dblRet As Double = 0
        Try
            Dim oWCFParameters = WCFDataProperties.ConvertToWCFProperties(New DAL.WCFParameters())
            Dim oCompData = New DAL.NGLCompParameterData(oWCFParameters)
            dblRet = oCompData.GetParValueUsingMultipleKeys(sParKey, sCompNumber, sCompAlpha, sCompLegalEntity)
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            sErrors = sqlEx.Detail.ToString(sqlEx.Reason.ToString())
        Catch ex As Exception
            sErrors = ex.Message
        End Try

        Return dblRet
    End Function

    ''' <summary>
    ''' Used to support the POStatusFlag of 5 option where a lane will be created if one 
    ''' does not exist.  Exception Handling follows the standard integration design pattern
    ''' Where messages are added to the GroupEmailMsg string or the AdminEmailMsg object
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 01/09/2016 for v-7.0.5
    ''' Modified by RHR for v-7.0.6.105 added support for POStatusFlag value of 6
    ''' Difference between 5 and 6:
    ''' If POStatusFlag = 5 the system will create a New lane If it does Not exist but the lane address validation rules will apply
    '''     where the Lane Orig And Dest address will be used On additional orders As they are transmitted To TMS
    ''' If POStatusFlag = 6 the system will create a New lane If it does Not exists And it will also use alternate shipping address information 
    '''     when additional order informaiton Is different than the lane.
    ''' NOTE: Lane Nunber Genration Rules are the same For both 5 And 6.  The generated Lane Number will be used To determine 
    '''      When a New lane should be created, not the address information.
    '''Modified by RHR for v-7.0.6.105 on 5/31/2017
    '''  fixed bug for parsing POInbound.  values of "1" or "0" do not parse to boolean
    '''  added code to test for "1"  default is "0" so no test is required
    ''' </remarks>
    Private Function createNewLaneFromPOData(ByRef oFields As clsImportFields) As Boolean
        Dim blnRet As Boolean = False
        If (oFields("POStatusFlag").ValueNoQuotes = "5" OrElse oFields("POStatusFlag").ValueNoQuotes = "6") And Not String.IsNullOrWhiteSpace(oFields("POvendor").ValueNoQuotes) Then
            'the ConvertToWCFProperties method of the NGLProxyAssembly builds a WCF Parameter object,
            'TODO: 
            'this is not the best way to manage this data.  
            '   (a) we do not need to use NGLProxyAssembly because we are referencing the NGL Data Library directly
            '   (b) the WCF Parameters should be stored locally and may be passed directly between Data objects
            Dim oWCFParameters = WCFDataProperties.ConvertToWCFProperties(New DAL.WCFParameters())
            Dim oLaneWCFData = New DAL.NGLLaneData(oWCFParameters)
            Dim resultObject = New ProcessLaneHeaderResult
            'Create a new allow update parameter object
            Dim oAllowUpdatePar As New AllowUpdateParameters() With {.WCFParameters = oWCFParameters, .ImportType = IntegrationTypes.Lane}
            'we create the lane automatically
            Dim oLaneImport As New clsLane(oConfig)

            With oLaneImport
                .AdminEmail = Me.AdminEmail
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .Retry = Me.Retry
                .SMTPServer = Me.SMTPServer
                .DBServer = Me.DBServer
                .Database = Me.Database
                .ConnectionString = Me.ConnectionString
                .AuthorizationCode = Me.AuthorizationCode
                .Debug = Me.Debug
                .WCFAuthCode = Me.WCFAuthCode
                .WCFURL = Me.WCFURL
                .WCFTCPURL = Me.WCFTCPURL
            End With
            Dim oLaneData As New clsLaneObject80()
            Dim blnInbound As Boolean = False
            If Not String.IsNullOrWhiteSpace(oFields("POInbound").ValueNoQuotes) Then
                If oFields("POInbound").ValueNoQuotes = "1" Then
                    blnInbound = True
                ElseIf Not Boolean.TryParse(oFields("POInbound").ValueNoQuotes, blnInbound) Then
                    blnInbound = False
                End If
            End If
            With oLaneData
                If blnInbound Then
                    .LaneName = Left(oFields("PODefaultCustomer").ValueNoQuotes & " " & oFields("POOrigName").ValueNoQuotes & " " & oFields("POOrigZip").ValueNoQuotes, 50)
                Else
                    .LaneName = Left(oFields("PODefaultCustomer").ValueNoQuotes & " " & oFields("PODestName").ValueNoQuotes & " " & oFields("PODestZip").ValueNoQuotes, 50)
                End If
                .LaneNumber = oFields("POvendor").ValueNoQuotes
                .LaneNumberMaster = ""
                .LaneNameMaster = ""
                .LaneCompNumber = oFields("PODefaultCustomer").ValueNoQuotes
                .LaneDefaultCarrierUse = False 'if integer.tryparse(oFields("PODefaultCarrier").ValueNoQuotes) then ...
                .LaneDefaultCarrierNumber = 0  'if integer.tryparse(oFields("PODefaultCarrier").ValueNoQuotes) then ...
                .LaneOrigCompNumber = oFields("POOrigCompNumber").ValueNoQuotes
                .LaneOrigName = oFields("POOrigName").ValueNoQuotes
                .LaneOrigAddress1 = oFields("POOrigAddress1").ValueNoQuotes
                .LaneOrigAddress2 = oFields("POOrigAddress2").ValueNoQuotes
                .LaneOrigAddress3 = oFields("POOrigAddress3").ValueNoQuotes
                .LaneOrigCity = oFields("POOrigCity").ValueNoQuotes
                .LaneOrigState = oFields("POOrigState").ValueNoQuotes
                .LaneOrigCountry = oFields("POOrigCountry").ValueNoQuotes
                .LaneOrigZip = oFields("POOrigZip").ValueNoQuotes
                .LaneOrigContactPhone = oFields("POOrigContactPhone").ValueNoQuotes
                .LaneOrigContactPhoneExt = oFields("POOrigContactPhoneExt").ValueNoQuotes
                .LaneOrigContactFax = oFields("POOrigContactFax").ValueNoQuotes
                If (oFields.containsKey("POOrigContactEmail")) Then
                    .LaneDestContactEmail = oFields("POOrigContactEmail").ValueNoQuotes
                End If
                .LaneDestCompNumber = oFields("PODestCompNumber").ValueNoQuotes
                .LaneDestName = oFields("PODestName").ValueNoQuotes
                .LaneDestAddress1 = oFields("PODestAddress1").ValueNoQuotes
                .LaneDestAddress2 = oFields("PODestAddress2").ValueNoQuotes
                .LaneDestAddress3 = oFields("PODestAddress3").ValueNoQuotes
                .LaneDestCity = oFields("PODestCity").ValueNoQuotes
                .LaneDestState = oFields("PODestState").ValueNoQuotes
                .LaneDestCountry = oFields("PODestCountry").ValueNoQuotes
                .LaneDestZip = oFields("PODestZip").ValueNoQuotes
                .LaneDestContactPhone = oFields("PODestContactPhone").ValueNoQuotes
                .LaneDestContactPhoneExt = oFields("PODestContactPhoneExt").ValueNoQuotes
                .LaneDestContactFax = oFields("PODestContactFax").ValueNoQuotes
                If (oFields.containsKey("PODestContactEmail")) Then
                    .LaneDestContactEmail = oFields("PODestContactEmail").ValueNoQuotes
                End If
                .LaneConsigneeNumber = oFields("POConsigneeNumber").ValueNoQuotes
                If oFields.containsKey("PORecMinIn") Then
                    .LaneRecMinIn = oFields("PORecMinIn").ValueNoQuotes
                End If
                If oFields.containsKey("PORecMinUnload") Then
                    .LaneRecMinUnload = oFields("PORecMinUnload").ValueNoQuotes
                End If
                If oFields.containsKey("PORecMinOut") Then
                    .LaneRecMinOut = oFields("PORecMinOut").ValueNoQuotes
                End If
                If oFields.containsKey("POAppt") Then
                    .LaneAppt = oFields("POAppt").ValueNoQuotes
                End If
                If Not Boolean.TryParse(oFields("POPalletExchange").ValueNoQuotes, .LanePalletExchange) Then .LanePalletExchange = False
                .LanePalletType = oFields("POPalletType").ValueNoQuotes
                'Modified by RHR for v-8.5.3.002 on 06/21/2022 added new Lane BFC Logic to booking import
                If oFields.containsKey("POBFC") Then
                    .LaneBFC = oFields("POBFC").ValueNoQuotes
                Else
                    .LaneBFC = 100
                End If

                If oFields.containsKey("POBFCType") Then
                    .LaneBFCType = oFields("POBFCType").ValueNoQuotes
                Else
                    .LaneBFCType = "PERC"
                End If
                If String.IsNullOrWhiteSpace(.LaneBFCType) Then
                    .LaneBFCType = "PERC"
                    .LaneBFC = 100
                End If
                .LaneComments = oFields("POComments").ValueNoQuotes
                .LaneCommentsConfidential = oFields("POCommentsConfidential").ValueNoQuotes
                Try
                    'we use defaults if we cannot read the temperature data,  errors are ignored
                    .LaneTempType = oLaneWCFData.getLaneTempTypeFromCommCode(oFields("POTemp").ValueNoQuotes)
                Catch ex As Exception
                    .LaneTempType = 3 'default = 3 for Dry
                End Try
                'set default TransType to 0 for N/A
                .LaneTransType = 0
                'if we can parse the po data we can map it to the LaneTransType
                Short.TryParse(oFields("POFrt").ValueNoQuotes, .LaneTransType)
                .LaneModeTypeControl = 3
                'if we can parse the po data we can map it to the LaneTransType
                Integer.TryParse(oFields("POModeTypeControl").ValueNoQuotes, .LaneModeTypeControl)
                .LaneOriginAddressUse = blnInbound
                .LaneCarrierEquipmentCodes = oFields("POCarrierEquipmentCodes").ValueNoQuotes
                If Not Integer.TryParse(oFields("PODefaultRouteSequence").ValueNoQuotes, .LaneDefaultRouteSequence) Then .LaneDefaultRouteSequence = 0
                .LaneRouteGuideNumber = oFields("PORouteGuideNumber").ValueNoQuotes
                If oFields.containsKey("POCompLegalEntity") Then
                    .LaneLegalEntity = oFields("POCompLegalEntity").ValueNoQuotes
                End If
                If oFields.containsKey("POCompAlphaCode") Then
                    .LaneCompAlphaCode = oFields("POCompAlphaCode").ValueNoQuotes
                End If
                If oFields.containsKey("POOrigLegalEntity") Then
                    .LaneOrigLegalEntity = oFields("POOrigLegalEntity").ValueNoQuotes
                End If
                If oFields.containsKey("POOrigCompAlphaCode") Then
                    .LaneOrigCompAlphaCode = oFields("POOrigCompAlphaCode").ValueNoQuotes
                End If
                If oFields.containsKey("PODestLegalEntity") Then
                    .LaneDestLegalEntity = oFields("PODestLegalEntity").ValueNoQuotes
                End If
                If oFields.containsKey("PODestCompAlphaCode") Then
                    .LaneDestCompAlphaCode = oFields("PODestCompAlphaCode").ValueNoQuotes
                End If
            End With
            resultObject = oLaneImport.ProcessLaneHeader80(oLaneData, oLaneWCFData, oAllowUpdatePar)
            If resultObject.successFlag Then
                oEL.AddNew(oFields("POvendor").Value, resultObject.LaneControl)
                blnRet = True
            Else
                AddToGroupEmailMsg(oLaneImport.GroupEmailMsg)
                AddToGroupEmailMsg("Could not create a new lane from the Order Information Provided; See Previous Email Log for more detail):")
            End If
        End If
        Return blnRet
    End Function

    ''' <summary>
    ''' if true do not wait for lane just import the order, if false save to no lane table and wait for the lane data later
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 6/6/2017
    ''' </remarks>
    Private Function IgnoreMissingLanes(ByVal CompControl As Integer) As Boolean
        Dim blnRet As Boolean = True
        Dim dblRetVal As Double = Me.getParValue("DataIntegrationPOWaitForLanes", CompControl)
        'the DataIntegrationPOWaitForLanes parameter tells us how long to wait.  A notification 
        'alert informs the users of what orders are outstanding.
        If dblRetVal > 0 Then blnRet = False
        Return blnRet
    End Function

    Public Sub testSilentTender(ByVal compControls As List(Of Integer))
        Try
            With SharedServices 'Me.PaneSettings.MainInterface.SharedServices
                If Not .UserConnected Then
                    .LogOn(WCFDataProperties)
                End If
            End With
            mintImportedCompControls = compControls
            silentImportProcessExecAsync()
        Catch ex As Exception
            LogException("Test Silent Tender Failure", "Could not process the requested data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.testSilentTender")
        Finally
            Try
                SharedServices.LogOff(WCFDataProperties)
            Catch ex As Exception

            End Try
        End Try
    End Sub

    ' ''' <summary>
    ' ''' this method is no longer beng used the code remains for historical reference it may be deleted after version 7.0
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function silentTenderLoads() As Boolean
    '    Dim blnRet As Boolean = False
    '    'Dim oCon As New System.Data.SqlClient.SqlConnection
    '    Dim strMSG As String = ""
    '    Dim strEmailError As String = ""
    '    Dim strBookTranCodeFilter As String = ""
    '    Dim strSource = "NGL.FreightMaster.Integration.clsBook.SilentTenderLoads"
    '    Try
    '        'get a list of loads to be processed
    '        'Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 Order By CompControl"
    '        ' Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)
    '        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
    '        Dim dblVal As Double = 0
    '        'Dim strAllowSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")
    '        Dim strAllowSilent As String = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")

    '        Double.TryParse(strAllowSilent, dblVal)
    '        If dblVal <> 1 Then
    '            'Log("DEBUG: Silent Tendering is Off!")
    '            'Silent Tendering is off so return false
    '            Return False
    '        End If
    '        Dim strSilentTenderEDIPCLoads = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderEDIPCLoads'")
    '        Dim blnSilentTenderEDIPCLoads As Boolean = True
    '        Double.TryParse(strSilentTenderEDIPCLoads, dblVal)
    '        If dblVal <> 1 Then
    '            blnSilentTenderEDIPCLoads = False
    '        End If
    '        Dim strSilentTenderPCLoads = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderPCLoads'")
    '        Double.TryParse(strSilentTenderPCLoads, dblVal)
    '        If dblVal <> 1 Then
    '            'Silent Tender of PC Loads is turned off check the silent tender of EDI PC Loads setting
    '            If blnSilentTenderEDIPCLoads Then
    '                'we only silent tender PC loads for EDI
    '                strBookTranCodeFilter = " AND ((isnull(dbo.Book.BookTranCode,'N') <> 'PC') OR Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204') and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204'))) "
    '            Else
    '                'we cannot tender any PC loads
    '                strBookTranCodeFilter = " AND (isnull(dbo.Book.BookTranCode,'N') <> 'PC') "
    '            End If
    '        ElseIf Not blnSilentTenderEDIPCLoads Then
    '            'we do not silent tender EDI PC loads other PC loads are ok to silent tender
    '            strBookTranCodeFilter = " AND ((isnull(dbo.Book.BookTranCode,'N') <> 'PC') OR ((isnull(dbo.Book.BookTranCode,'N') = 'PC') AND Not Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204') and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204')))) "
    '        End If




    '        Dim strDeleteLoadsOnSilent As String = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalDeleteLoadsOnSilentTendering'")
    '        Dim blnDeleteLoads As Boolean = False
    '        Double.TryParse(strDeleteLoadsOnSilent, dblVal)
    '        If dblVal = 1 Then

    '            'Log("DEBUG: Delete Loads Is True!")
    '            'Delete Loads is on
    '            blnDeleteLoads = True
    '        End If
    '        Dim intSilentTenderDelay As Integer = CInt(oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderingDelay'"))
    '        Dim strInClause As String = ""
    '        Dim strInSeperator As String = ""
    '        If Not mintImportedCompControls Is Nothing AndAlso mintImportedCompControls.Count > 0 Then
    '            strInClause = " AND dbo.Comp.CompControl in ("
    '            For Each c In mintImportedCompControls
    '                strInClause &= strInSeperator & c.ToString
    '                strInSeperator = ", "
    '            Next
    '            strInClause &= ")"
    '        End If
    '        Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl "
    '        'this code is used to determine if the order is using carrier EDI data.
    '        'strSQL &= "CAST(CASE WHEN Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204')and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204'))  THEN 1 ELSE 0 End AS BIT)  as UsingEDI"

    '        strSQL &= " FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber left outer join dbo.Book on dbo.pohdr.POHDRPRONumber = dbo.Book.BookProNumber "
    '        strSQL &= " Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 "
    '        strSQL &= strBookTranCodeFilter
    '        strSQL &= strInClause
    '        strSQL &= " Order By CompControl"
    '        Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
    '        If Not oQR.Exception Is Nothing Then
    '            ITEmailMsg &= "<br />Read Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of an unexpected error while reading the POHDR data table.<br />" & vbCrLf & readExceptionMessage(oQR.Exception) & "<br />" & vbCrLf
    '            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Read Silent Tender Loads Data Warning!" & readExceptionMessage(oQR.Exception))
    '            Return False
    '        End If
    '        Dim dt As System.Data.DataTable = oQR.Data
    '        Dim blnLoadsProcessedForCompany As Boolean = False
    '        Dim intLastComp As Integer = 0
    '        Dim intFinalizedForComp As Integer = 0
    '        Dim strTenderedLoads As New List(Of String)
    '        Dim intDeletedForComp As Integer = 0
    '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

    '            'Log("DEBUG: Processing " & dt.Rows.Count.ToString & " Rows!")
    '            Dim strOrderNumber As String = ""
    '            Dim strBookProNumber As String = ""
    '            Dim strModVerify As String = ""
    '            Dim strVendorNumber As String = ""
    '            Dim intOrderSequence As Integer = 0
    '            Dim intDefCompNumber As Integer = 0
    '            Dim intCompControl As Integer = 0
    '            Dim intRecord = 0
    '            Dim blnSendAsBatch As Boolean = True
    '            Dim intTotal = dt.Rows.Count
    '            For Each oRow As System.Data.DataRow In dt.Rows
    '                intRecord += 1
    '                If intRecord >= intTotal Then blnSendAsBatch = False
    '                'Get the next Comp Control Number
    '                intCompControl = 0
    '                Integer.TryParse(DTran.getDataRowString(oRow, "CompControl", "0"), intCompControl)
    '                'Check if the company has changed.
    '                If intCompControl <> intLastComp Then
    '                    If intLastComp > 0 And blnLoadsProcessedForCompany Then
    '                        'send email
    '                        Dim strEmail As String = ""
    '                        Try
    '                            strEmail = oQuery.getScalarValue(DBCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
    '                        Catch ex As Exception
    '                            ITEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
    '                            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
    '                        End Try
    '                        If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
    '                            strEmail = AdminEmail
    '                        End If
    '                        Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
    '                        strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
    '                        For Each s In strTenderedLoads
    '                            strBody &= s & vbCrLf
    '                        Next
    '                        SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
    '                        'Log("DEBUG: Email Generated: " & strBody)
    '                        'Clear the message string list
    '                        strTenderedLoads = New List(Of String)
    '                        'Reset the number of loads finalized
    '                        intFinalizedForComp = 0
    '                        'Reset the loads processed flag to false
    '                        blnLoadsProcessedForCompany = False
    '                    End If
    '                    'Reset the last comp control
    '                    intLastComp = intCompControl
    '                End If
    '                'Get the current data for this row
    '                strOrderNumber = DTran.getDataRowString(oRow, "POHDROrderNumber", "")
    '                strBookProNumber = DTran.getDataRowString(oRow, "POHDRPRONumber", "")
    '                strModVerify = DTran.getDataRowString(oRow, "POHDRModVerify", "")
    '                strVendorNumber = DTran.getDataRowString(oRow, "POHDRvendor", "")
    '                intOrderSequence = 0
    '                Integer.TryParse(DTran.getDataRowString(oRow, "POHDROrderSequence", "0"), intOrderSequence)
    '                intDefCompNumber = 0
    '                Integer.TryParse(DTran.getDataRowString(oRow, "POHDRDefaultCustomer", "0"), intDefCompNumber)
    '                'Check the Mod Verify setting and process the data as needed
    '                Dim strErrMsg As String = ""
    '                Dim blnErrTenderingLoad As Boolean = False
    '                Dim blnSkipLoad As Boolean = False
    '                Dim faultExceptionMessage As NGL.FMWCFProxy.FaultExceptionEventArgs
    '                'Log("DEBUG: Mod Verify Value = " & strModVerify & " for  Order Number " & strOrderNumber)
    '                Try
    '                    Select Case strModVerify
    '                        Case "No Pro"
    '                            'Log("DEBUG: RunWriteNewBookingWithData")
    '                            'Old code removed we now call the wcf proxy via ImportPOHdr
    '                            'blnErrTenderingLoad = Not runWriteNewBookingWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
    '                            blnErrTenderingLoad = Not ImportPOHdr(strModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, strSource, strErrMsg, blnSendAsBatch)
    '                        Case "FINALIZED"
    '                            'Log("DEBUG: RunProcessFinalizedData")
    '                            blnErrTenderingLoad = Not runProcessFinalizedData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
    '                        Case "DELETED"
    '                            'Log("DEBUG: runRemoveDeletedWithData")
    '                            blnErrTenderingLoad = Not runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
    '                            'We do not need to notify the users because this order was already deleted it only existed in the staging table.
    '                            blnSkipLoad = True
    '                        Case "DELETE-B"
    '                            If blnDeleteLoads Then
    '                                'Log("DEBUG: runDeleteOrderWithData")
    '                                blnErrTenderingLoad = Not runDeleteOrderWithData(strBookProNumber, strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
    '                            Else
    '                                blnSkipLoad = True
    '                            End If
    '                        Case "DELETE-F"
    '                            'Log("DEBUG: Skip DELETE-F")
    '                            blnSkipLoad = True
    '                        Case "NO LANE"
    '                            'Log("DEBUG: Skip NO LANE")
    '                            blnSkipLoad = True
    '                        Case "NEW TRAN-F"
    '                            'Log("DEBUG: Skip NEW TRAN-F")
    '                            blnSkipLoad = True
    '                        Case "NEW TRAN"
    '                            'Log("DEBUG: Skip NEW TRAN")
    '                            blnSkipLoad = True
    '                        Case "NEW COMP"
    '                            'Log("DEBUG: Skip NEW COMP")
    '                            blnSkipLoad = True
    '                        Case Else
    '                            'Log("DEBUG: Default runUpdatePOModificationWithDatad")
    '                            'Old code removed we now call the wcf proxy via ImportPOHdr
    '                            'blnErrTenderingLoad = Not runUpdatePOModificationWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
    '                            blnErrTenderingLoad = Not ImportPOHdr(strModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, strSource, strErrMsg, blnSendAsBatch)
    '                    End Select

    '                    If Not blnErrTenderingLoad Then
    '                        If Not blnSkipLoad Then
    '                            blnLoadsProcessedForCompany = True
    '                            strTenderedLoads.Add("<p>Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & "</p>")
    '                            System.Threading.Thread.Sleep(200)
    '                        End If
    '                    Else
    '                        'Process Error Message and continue
    '                        GroupEmailMsg &= "<br />Silent Tender Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & strErrMsg & "<br />" & vbCrLf
    '                        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & strErrMsg)
    '                    End If

    '                Catch ex As Exception
    '                    'Process Error Message and continue
    '                    GroupEmailMsg &= "<br />Silent Tender Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & ex.Message & "<br />" & vbCrLf
    '                    Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & ex.Message)

    '                End Try
    '                If intSilentTenderDelay > 0 Then
    '                    If Debug Then Log("Start Silent Tender Delay: " & Date.Now.ToString)
    '                    System.Threading.Thread.Sleep(intSilentTenderDelay)
    '                    If Debug Then Log("End Silent Tender Delay: " & Date.Now.ToString)
    '                End If
    '            Next
    '            'Finally process the last company data
    '            If intLastComp > 0 And blnLoadsProcessedForCompany Then
    '                'send email
    '                Dim strEmail As String = ""
    '                Try
    '                    strEmail = oQuery.getScalarValue(DBCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
    '                Catch ex As Exception
    '                    ITEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
    '                    Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
    '                End Try
    '                If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
    '                    strEmail = AdminEmail
    '                End If
    '                Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
    '                strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
    '                For Each s In strTenderedLoads
    '                    strBody &= s & vbCrLf
    '                Next
    '                SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
    '                'Log("DEBUG: Email Generated: " & strBody)
    '            End If
    '            blnRet = True
    '        End If
    '    Catch ex As Ngl.Core.DatabaseRetryExceededException
    '        ITEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a retry exceeded failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))
    '        Return False
    '    Catch ex As Ngl.Core.DatabaseLogInException
    '        ITEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a database login failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))
    '        Return False
    '    Catch ex As Ngl.Core.DatabaseInvalidException
    '        ITEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a database access failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))
    '        Return False
    '    Catch ex As Ngl.Core.DatabaseDataValidationException
    '        ITEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of an unexpected data validation failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))
    '        Return False
    '    Catch ex As Exception
    '        Throw
    '        Return False
    '    End Try

    '    Return blnRet
    'End Function

    'TODO: make private again after testing
    'private Sub silentTenderLoadsExecAsync
    Public Sub silentTenderLoadsExecAsyncold()

        Dim strMSG As String = ""
        Dim strEmailError As String = ""
        Dim strBookTranCodeFilter As String = ""
        Dim strSource = "NGL.FreightMaster.Integration.clsBook.SilentTenderLoads"
        Dim strEmailMsg As String = ""
        Dim oCon As System.Data.SqlClient.SqlConnection
        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
        Try
            'With SharedServices 'Me.PaneSettings.MainInterface.SharedServices
            '    If Not .UserConnected Then
            '        .LogOn(WCFDataProperties)
            '    End If
            'End With
            'get a list of loads to be processed
            'Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 Order By CompControl"
            ' Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)

            Dim dblVal As Double = 0
            'Because this method runs Async we must create a seperate DB connection and also close it in the Finally statement
            oCon = getNewConnection(False)
            'Dim strAllowSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")
            Dim strAllowSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")

            Double.TryParse(strAllowSilent, dblVal)
            If dblVal <> 1 Then
                'Log("DEBUG: Silent Tendering is Off!")
                'Silent Tendering is off so return false
                Return
            End If
            Dim strSilentTenderEDIPCLoads = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderEDIPCLoads'")
            Dim blnSilentTenderEDIPCLoads As Boolean = True
            Double.TryParse(strSilentTenderEDIPCLoads, dblVal)
            If dblVal <> 1 Then
                blnSilentTenderEDIPCLoads = False
            End If
            Dim strSilentTenderPCLoads = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderPCLoads'")
            Double.TryParse(strSilentTenderPCLoads, dblVal)
            If dblVal <> 1 Then
                'Silent Tender of PC Loads is turned off check the silent tender of EDI PC Loads setting
                If blnSilentTenderEDIPCLoads Then
                    'we only silent tender PC loads for EDI
                    strBookTranCodeFilter = " AND ((isnull(dbo.Book.BookTranCode,'N') <> 'PC') OR Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204') and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204'))) "
                Else
                    'we cannot tender any PC loads
                    strBookTranCodeFilter = " AND (isnull(dbo.Book.BookTranCode,'N') <> 'PC') "
                End If
            ElseIf Not blnSilentTenderEDIPCLoads Then
                'we do not silent tender EDI PC loads other PC loads are ok to silent tender
                strBookTranCodeFilter = " AND ((isnull(dbo.Book.BookTranCode,'N') <> 'PC') OR ((isnull(dbo.Book.BookTranCode,'N') = 'PC') AND Not Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204') and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204')))) "
            End If




            Dim strDeleteLoadsOnSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalDeleteLoadsOnSilentTendering'")
            Dim blnDeleteLoads As Boolean = False
            Double.TryParse(strDeleteLoadsOnSilent, dblVal)
            If dblVal = 1 Then

                'Log("DEBUG: Delete Loads Is True!")
                'Delete Loads is on
                blnDeleteLoads = True
            End If
            Dim intSilentTenderDelay As Integer = CInt(oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderingDelay'"))
            Dim strInClause As String = ""
            Dim strInSeperator As String = ""
            If Not mintImportedCompControls Is Nothing AndAlso mintImportedCompControls.Count > 0 Then
                strInClause = " AND dbo.Comp.CompControl in ("
                For Each c In mintImportedCompControls
                    strInClause &= strInSeperator & c.ToString
                    strInSeperator = ", "
                Next
                strInClause &= ")"
            End If
            Dim strOrderInClause As String = ""
            If Not mstrOrderNumbers Is Nothing AndAlso mstrOrderNumbers.Count > 0 Then
                strInSeperator = ""
                strOrderInClause = " AND POHDROrderNumber in ("
                For Each o In mstrOrderNumbers
                    strOrderInClause &= String.Format("{0}'{1}'", strInSeperator, o)
                    strInSeperator = ", "
                Next
                strOrderInClause &= ")"
            End If
            Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl "
            'this code is used to determine if the order is using carrier EDI data.
            'strSQL &= "CAST(CASE WHEN Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204')and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204'))  THEN 1 ELSE 0 End AS BIT)  as UsingEDI"

            strSQL &= " FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber left outer join dbo.Book on dbo.pohdr.POHDRPRONumber = dbo.Book.BookProNumber "
            strSQL &= " Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 "
            strSQL &= strBookTranCodeFilter
            strSQL &= strInClause
            strSQL &= strOrderInClause
            strSQL &= " Order By CompControl"
            Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQR.Exception Is Nothing Then
                strEmailMsg &= "<br />Read Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of an unexpected error while reading the POHDR data table.<br />" & vbCrLf & readExceptionMessage(oQR.Exception) & "<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Read Silent Tender Loads Data Warning!" & readExceptionMessage(oQR.Exception))
                Return
            End If
            Dim dt As System.Data.DataTable = oQR.Data
            Dim blnLoadsProcessedForCompany As Boolean = False
            Dim intLastComp As Integer = 0
            Dim intFinalizedForComp As Integer = 0
            Dim strTenderedLoads As New List(Of String)
            Dim intDeletedForComp As Integer = 0
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

                'Log("DEBUG: Processing " & dt.Rows.Count.ToString & " Rows!")
                Dim strOrderNumber As String = ""
                Dim strBookProNumber As String = ""
                Dim strModVerify As String = ""
                Dim strVendorNumber As String = ""
                Dim intOrderSequence As Integer = 0
                Dim intDefCompNumber As Integer = 0
                Dim intCompControl As Integer = 0
                Dim intRecord = 0
                Dim blnSendAsBatch As Boolean = True
                Dim intTotal = dt.Rows.Count
                For Each oRow As System.Data.DataRow In dt.Rows
                    intRecord += 1
                    If intRecord >= intTotal Then blnSendAsBatch = False
                    'Get the next Comp Control Number
                    intCompControl = 0
                    Integer.TryParse(DTran.getDataRowString(oRow, "CompControl", "0"), intCompControl)
                    'Check if the company has changed.
                    If intCompControl <> intLastComp Then
                        If intLastComp > 0 And blnLoadsProcessedForCompany Then
                            'send email
                            Dim strEmail As String = ""
                            Try
                                strEmail = oQuery.getScalarValue(oCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
                            Catch ex As Exception
                                strEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                                Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
                            End Try
                            If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
                                strEmail = AdminEmail
                            End If
                            Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
                            strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
                            For Each s In strTenderedLoads
                                strBody &= s & vbCrLf
                            Next
                            SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
                            'Log("DEBUG: Email Generated: " & strBody)
                            'Clear the message string list
                            strTenderedLoads = New List(Of String)
                            'Reset the number of loads finalized
                            intFinalizedForComp = 0
                            'Reset the loads processed flag to false
                            blnLoadsProcessedForCompany = False
                        End If
                        'Reset the last comp control
                        intLastComp = intCompControl
                    End If
                    'Get the current data for this row
                    strOrderNumber = DTran.getDataRowString(oRow, "POHDROrderNumber", "")
                    strBookProNumber = DTran.getDataRowString(oRow, "POHDRPRONumber", "")
                    strModVerify = DTran.getDataRowString(oRow, "POHDRModVerify", "")
                    strVendorNumber = DTran.getDataRowString(oRow, "POHDRvendor", "")
                    intOrderSequence = 0
                    Integer.TryParse(DTran.getDataRowString(oRow, "POHDROrderSequence", "0"), intOrderSequence)
                    intDefCompNumber = 0
                    Integer.TryParse(DTran.getDataRowString(oRow, "POHDRDefaultCustomer", "0"), intDefCompNumber)
                    'Check the Mod Verify setting and process the data as needed
                    Dim strErrMsg As String = ""
                    Dim blnErrTenderingLoad As Boolean = False
                    Dim blnSkipLoad As Boolean = False
#Disable Warning BC42024 ' Unused local variable: 'faultExceptionMessage'.
                    Dim faultExceptionMessage As Ngl.FMWCFProxy.FaultExceptionEventArgs
#Enable Warning BC42024 ' Unused local variable: 'faultExceptionMessage'.
                    'Log("DEBUG: Mod Verify Value = " & strModVerify & " for  Order Number " & strOrderNumber)
                    Try
                        Select Case strModVerify
                            Case "No Pro"
                                'Log("DEBUG: RunWriteNewBookingWithData")
                                'Old code removed we now call the wcf proxy via ImportPOHdr
                                'blnErrTenderingLoad = Not runWriteNewBookingWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
                                blnErrTenderingLoad = Not ImportPOHdr(strModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, strSource, strErrMsg, blnSendAsBatch)
                            Case "FINALIZED"
                                'Log("DEBUG: RunProcessFinalizedData")
                                blnErrTenderingLoad = Not runProcessFinalizedData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
                            Case "DELETED"
                                'Log("DEBUG: runRemoveDeletedWithData")
                                blnErrTenderingLoad = Not runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
                                'We do not need to notify the users because this order was already deleted it only existed in the staging table.
                                blnSkipLoad = True
                            Case "DELETE-B"
                                If blnDeleteLoads Then
                                    'Log("DEBUG: runDeleteOrderWithData")
                                    blnErrTenderingLoad = Not runDeleteOrderWithData(strBookProNumber, strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
                                Else
                                    blnSkipLoad = True
                                End If
                            Case "DELETE-F"
                                'Log("DEBUG: Skip DELETE-F")
                                blnSkipLoad = True
                            Case "NO LANE"
                                'Log("DEBUG: Skip NO LANE")
                                blnSkipLoad = True
                            Case "NEW LANE" 'Modified by RHR for v-8.3.0.001 on 09/18/2020  added logic to skip import on "NEW LANE"
                                'Log("DEBUG: Skip NEW LANE")
                                blnSkipLoad = True
                            Case "NEW TRAN-F"
                                'Log("DEBUG: Skip NEW TRAN-F")
                                blnSkipLoad = True
                            Case "NEW TRAN"
                                'Log("DEBUG: Skip NEW TRAN")
                                blnSkipLoad = True
                            Case "NEW COMP"
                                'Log("DEBUG: Skip NEW COMP")
                                blnSkipLoad = True
                            Case Else
                                'Log("DEBUG: Default runUpdatePOModificationWithDatad")
                                'Old code removed we now call the wcf proxy via ImportPOHdr
                                'blnErrTenderingLoad = Not runUpdatePOModificationWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
                                blnErrTenderingLoad = Not ImportPOHdr(strModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, strSource, strErrMsg, blnSendAsBatch)
                        End Select

                        If Not blnErrTenderingLoad Then
                            If Not blnSkipLoad Then
                                blnLoadsProcessedForCompany = True
                                strTenderedLoads.Add("<p>Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & "</p>")
                                System.Threading.Thread.Sleep(200)
                            End If
                        Else
                            'Process Error Message and continue
                            strEmailMsg &= "<br />Silent Tender Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & strErrMsg & "<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & strErrMsg)
                        End If

                    Catch ex As Exception
                        'Process Error Message and continue
                        strEmailMsg &= "<br />Silent Tender Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & ex.Message & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & ex.Message)

                    End Try
                    If intSilentTenderDelay > 0 Then
                        If Debug Then Log("Start Silent Tender Delay: " & Date.Now.ToString)
                        System.Threading.Thread.Sleep(intSilentTenderDelay)
                        If Debug Then Log("End Silent Tender Delay: " & Date.Now.ToString)
                    End If
                Next
                'Finally process the last company data
                If intLastComp > 0 And blnLoadsProcessedForCompany Then
                    'send email
                    Dim strEmail As String = ""
                    Try
                        strEmail = oQuery.getScalarValue(oCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
                    Catch ex As Exception
                        strEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
                    End Try
                    If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
                        strEmail = AdminEmail
                    End If
                    Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
                    strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
                    For Each s In strTenderedLoads
                        strBody &= s & vbCrLf
                    Next
                    SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
                    'Log("DEBUG: Email Generated: " & strBody)
                End If
            End If
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a retry exceeded failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

        Catch ex As Ngl.Core.DatabaseLogInException
            strEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a database login failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

        Catch ex As Ngl.Core.DatabaseInvalidException
            strEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a database access failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

        Catch ex As Ngl.Core.DatabaseDataValidationException
            strEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of an unexpected data validation failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

        Catch ex As Exception
            Log("Web Service Shared Services Log off Error: " & ex.Message)
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try


            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                    oCon.Close()
                    oCon = Nothing
                End If
            Catch ex As Exception

            End Try
            'Try
            '    'If mblnSharedServiceRunning Then SharedServices.LogOff(WCFDataProperties)
            'Catch ex As Exception
            '    Log("Web Service Shared Services Log off Error: " & ex.Message)
            'End Try
            'mblnSharedServiceRunning = False
            If strEmailMsg.Trim.Length > 0 Then
                LogError("Process Silent Tender Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & strEmailMsg, GroupEmail)
            End If
        End Try

    End Sub


    ''' <summary>
    ''' 
    ''' </summary
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.001 on 09/18/2020  
    '''   added logic to skip import on "NEW LANE"
    ''' </remarks>
    Public Sub silentImportProcessExecAsync()

        Dim strMSG As String = ""
        Dim strEmailError As String = ""
        Dim strBookTranCodeFilter As String = ""
        Dim strSource = "NGL.FreightMaster.Integration.clsBook.SilentImportProcess"
        Dim strEmailMsg As String = ""
        Dim oCon As System.Data.SqlClient.SqlConnection
        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
        Try
            Dim dblVal As Double = 0
            'Because this method runs Async we must create a seperate DB connection and also close it in the Finally statement
            oCon = getNewConnection(False)
            Dim dblGlobalAllowSilentTendering As Double = 0D
            Dim blnDeleteLoads As Boolean = False
            Dim dblGlobalSilentTenderingDelay As Double = 0D
            Dim blnLoadsProcessedForCompany As Boolean = False
            Dim intLastComp As Integer = 0
            Dim intFinalizedForComp As Integer = 0
            Dim strTenderedLoads As New List(Of String)
            Dim intDeletedForComp As Integer = 0
            Dim intRecord = 0
            Dim blnSendAsBatch As Boolean = True
            Dim strPOHDRDefaultCustomer As String = ""
            Dim strOrderNumber As String = ""
            Dim intCompControl As Integer = 0

            If Not mstrOrderNumbers Is Nothing AndAlso mstrOrderNumbers.Count > 0 Then
                For Each strOrderNumber In mstrOrderNumbers
                    Try
                        Dim oWCFParameters = WCFDataProperties.ConvertToWCFProperties(New DAL.WCFParameters())
                        Dim oPOHdrData As New DAL.NGLPOHdrData(oWCFParameters)
                        Dim oValidated = oPOHdrData.PreSilentTenderValidation(0, strOrderNumber)
                        If intLastComp = 0 Then
                            intLastComp = oValidated.CompControl 'last and current comp are the same the first time through
                        End If
                        intCompControl = oValidated.CompControl
                        If oValidated.ErrNumber <> 0 Then
                            Dim sSQLMsg = oValidated.RetMsg
                            strEmailMsg &= "<br />Silent Import Processing Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess could import order number " & strOrderNumber & ":<br /> " & vbCrLf & sSQLMsg & "<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Silent Import Processing Warning for order number " & strOrderNumber & ": " & sSQLMsg)
                            Continue For
                        End If
                        If oValidated.AllowSilentForOrder.HasValue AndAlso oValidated.AllowSilentForOrder.Value = False Then
                            'validation failed so log any messages and goto the next order
                            If Not String.IsNullOrEmpty(oValidated.RetMsg) Then
                                'add the message to the log
                                Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Silent Import validation failed for order number " & strOrderNumber & ": " & oValidated.RetMsg)
                            End If
                            Continue For
                        End If
                        If Not String.IsNullOrEmpty(oValidated.RetMsg) Then
                            'add the message to the log
                            Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Silent Import validation message for order number " & strOrderNumber & ": " & oValidated.RetMsg)
                        End If
                        dblGlobalAllowSilentTendering = oValidated.GlobalAllowSilentTendering
                        blnDeleteLoads = If(oValidated.GlobalDeleteLoadsOnSilentTendering = 1, True, False)
                        dblGlobalSilentTenderingDelay = oValidated.GlobalSilentTenderingDelay
                        strPOHDRDefaultCustomer = oValidated.POHDRDefaultCustomer
                        intRecord += 1
                        Dim strErrMsg As String = ""
                        Dim blnErrTenderingLoad As Boolean = False
                        Dim blnSkipLoad As Boolean = False
#Disable Warning BC42024 ' Unused local variable: 'faultExceptionMessage'.
                        Dim faultExceptionMessage As Ngl.FMWCFProxy.FaultExceptionEventArgs
#Enable Warning BC42024 ' Unused local variable: 'faultExceptionMessage'.
                        Dim intDefCompNumber As Integer = 0
                        Integer.TryParse(strPOHDRDefaultCustomer, intDefCompNumber)
                        Select Case oValidated.POHDRModVerify
                            Case "No Pro"
                                'Log("DEBUG: RunWriteNewBookingWithData")
                                'Old code removed we now call the wcf proxy via ImportPOHdr
                                'blnErrTenderingLoad = Not runWriteNewBookingWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
                                blnErrTenderingLoad = Not ImportPOHdr(oValidated.POHDRModVerify, strOrderNumber, oValidated.POHDROrderSequence, intDefCompNumber, oValidated.POHDRvendor, oValidated.POHDRPRONumber, strSource, strErrMsg, blnSendAsBatch)
                            Case "FINALIZED"
                                'Log("DEBUG: RunProcessFinalizedData")
                                blnErrTenderingLoad = Not runProcessFinalizedData(strOrderNumber, oValidated.POHDROrderSequence, intDefCompNumber, strErrMsg)
                            Case "DELETED"
                                'Log("DEBUG: runRemoveDeletedWithData")
                                blnErrTenderingLoad = Not runRemoveDeletedWithData(strOrderNumber, oValidated.POHDROrderSequence, intDefCompNumber, strErrMsg)
                                'We do not need to notify the users because this order was already deleted it only existed in the staging table.
                                blnSkipLoad = True
                            Case "DELETE-B"
                                If blnDeleteLoads Then
                                    'Log("DEBUG: runDeleteOrderWithData")
                                    blnErrTenderingLoad = Not runDeleteOrderWithData(oValidated.POHDRPRONumber, strOrderNumber, oValidated.POHDROrderSequence, intDefCompNumber, strErrMsg)
                                Else
                                    blnSkipLoad = True
                                End If
                            Case "DELETE-F"
                                'Log("DEBUG: Skip DELETE-F")
                                blnSkipLoad = True
                            Case "NO LANE"
                                'Log("DEBUG: Skip NO LANE")
                                blnSkipLoad = True
                            Case "NEW LANE" 'Modified by RHR for v-8.3.0.001 on 09/18/2020  added logic to skip import on "NEW LANE"
                                'Log("DEBUG: Skip NEW LANE")
                                'blnSkipLoad = True
                                blnErrTenderingLoad = Not ImportPOHdr(oValidated.POHDRModVerify, strOrderNumber, oValidated.POHDROrderSequence, intDefCompNumber, oValidated.POHDRvendor, oValidated.POHDRPRONumber, strSource, strErrMsg, blnSendAsBatch)
                            Case "NEW TRAN-F"
                                'Log("DEBUG: Skip NEW TRAN-F")
                                blnSkipLoad = True
                            Case "NEW TRAN"
                                'Log("DEBUG: Skip NEW TRAN")
                                'blnSkipLoad = True
                                blnErrTenderingLoad = Not ImportPOHdr(oValidated.POHDRModVerify, strOrderNumber, oValidated.POHDROrderSequence, intDefCompNumber, oValidated.POHDRvendor, oValidated.POHDRPRONumber, strSource, strErrMsg, blnSendAsBatch)
                            Case "NEW COMP"
                                'Log("DEBUG: Skip NEW COMP")
                                blnSkipLoad = True
                            Case Else
                                'Log("DEBUG: Default runUpdatePOModificationWithDatad")
                                'Old code removed we now call the wcf proxy via ImportPOHdr
                                'blnErrTenderingLoad = Not runUpdatePOModificationWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
                                blnErrTenderingLoad = Not ImportPOHdr(oValidated.POHDRModVerify, strOrderNumber, oValidated.POHDROrderSequence, intDefCompNumber, oValidated.POHDRvendor, oValidated.POHDRPRONumber, strSource, strErrMsg, blnSendAsBatch)
                        End Select
                        If Not blnErrTenderingLoad Then
                            If Not blnSkipLoad Then
                                blnLoadsProcessedForCompany = True
                                strTenderedLoads.Add("<p>Order Number: " & strOrderNumber & " Seq: " & oValidated.POHDROrderSequence.ToString() & " Type: " & oValidated.POHDRModVerify & " Company: " & strPOHDRDefaultCustomer & "</p>")
                                System.Threading.Thread.Sleep(200)
                            End If
                        Else
                            'Process Error Message and continue
                            strEmailMsg &= "<br />Silent Import Process Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess there was a problem while attempting to silent import Order Number: " & strOrderNumber & " Seq: " & oValidated.POHDROrderSequence.ToString() & " Type: " & oValidated.POHDRModVerify & " Company: " & strPOHDRDefaultCustomer & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & strErrMsg & "<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess there was a problem while attempting to silent import Order Number: " & strOrderNumber & " Seq: " & oValidated.POHDROrderSequence.ToString() & " Type: " & oValidated.POHDRModVerify & " Company: " & strPOHDRDefaultCustomer & ". The actual error is: " & strErrMsg)
                        End If
                    Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
                        Dim sSQLMsg = sqlEx.Detail.ToString(sqlEx.Reason.ToString())
                        strEmailMsg &= "<br />Silent Import Processing Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess could import order number " & strOrderNumber & ":<br /> " & vbCrLf & sSQLMsg & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Silent Import Processing Warning for order number " & strOrderNumber & ": " & sSQLMsg)
                    Catch ex As Exception
                        Dim sSQLMsg = ex.Message
                        strEmailMsg &= "<br />Silent Import Processing Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess could import order number " & strOrderNumber & ":<br /> " & vbCrLf & sSQLMsg & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Silent Import Processing Warning for order number " & strOrderNumber & ": " & sSQLMsg)
                    End Try
                    If dblGlobalSilentTenderingDelay > 0 Then
                        If Debug Then Log("Start Silent Import Delay: " & Date.Now.ToString)
                        System.Threading.Thread.Sleep(dblGlobalSilentTenderingDelay)
                        If Debug Then Log("End Silent Import Delay: " & Date.Now.ToString)
                    End If
                    'Check if the company has changed.
                    If intCompControl <> intLastComp Then
                        If intLastComp > 0 And blnLoadsProcessedForCompany Then
                            'send email
                            Dim strEmail As String = ""
                            Try
                                'Note:  the validation routine could return this email informaition
                                strEmail = oQuery.getScalarValue(oCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
                            Catch ex As Exception
                                strEmailMsg &= "<br />Email Silent Import Process Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess an unexpected error occurred while attempting to get the company notify contact email information for company number " & strPOHDRDefaultCustomer & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                                Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess an unexpected error occurred while attempting to get the company notify contact email information for company number " & strPOHDRDefaultCustomer & ".  " & readExceptionMessage(ex))
                            End Try
                            If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
                                strEmail = AdminEmail
                            End If
                            Dim strBody As String = "<h2>Silent Import Process for  Company Number " & strPOHDRDefaultCustomer & "</h2>" & vbCrLf
                            strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
                            For Each s In strTenderedLoads
                                strBody &= s & vbCrLf
                            Next
                            'Modified By LVV on 8/11/16 for v-7.0.5.102 Task #2 Turn off Silent Tender Emails
                            'SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
                            'Log("DEBUG: Email Generated: " & strBody)
                            'Clear the message string list
                            strTenderedLoads = New List(Of String)
                            'Reset the number of loads finalized
                            intFinalizedForComp = 0
                            'Reset the loads processed flag to false
                            blnLoadsProcessedForCompany = False
                        End If
                        'Reset the first and  last comp control
                        intLastComp = intCompControl
                        intCompControl = 0
                    End If
                Next
            End If
            'Finally process the last company data
            If ((intCompControl = 0 And intLastComp > 0) OrElse (intCompControl = intLastComp And intLastComp > 0)) And blnLoadsProcessedForCompany Then
                'send email
                Dim strEmail As String = ""
                Try
                    strEmail = oQuery.getScalarValue(oCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
                Catch ex As Exception
                    strEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess an unexpected error occurred while attempting to get the company notify contact email information for company number " & strPOHDRDefaultCustomer & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                    Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess an unexpected error occurred while attempting to get the company notify contact email information for company number " & strPOHDRDefaultCustomer & ".  " & readExceptionMessage(ex))
                End Try
                If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
                    strEmail = AdminEmail
                End If
                Dim strBody As String = "<h2>Silent Import Load for  Company Number " & strPOHDRDefaultCustomer & "</h2>" & vbCrLf
                strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
                For Each s In strTenderedLoads
                    strBody &= s & vbCrLf
                Next
                'Modified By LVV on 8/11/16 for v-7.0.5.102 Task #2 Turn off Silent Tender Emails
                'SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
                'Log("DEBUG: Email Generated: " & strBody)
            End If

        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strEmailMsg &= "<br />Process Silent Import Data Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess could not silent import orders because of a retry exceeded failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

        Catch ex As Ngl.Core.DatabaseLogInException
            strEmailMsg &= "<br />Process Silent Import Data Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess could not ilent import orders because of a database login failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

        Catch ex As Ngl.Core.DatabaseInvalidException
            strEmailMsg &= "<br />Process Silent Import Data Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess could not ilent import orders because of a database access failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

        Catch ex As Ngl.Core.DatabaseDataValidationException
            strEmailMsg &= "<br />Process Silent Import Data Warning: NGL.FreightMaster.Integration.clsBook.SilentImportProcess could not ilent import orders because of an unexpected data validation failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.SilentImportProcess Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

        Catch ex As Exception
            Log("Web Service Shared Services Log off Error: " & ex.Message)
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try


            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                    oCon.Close()
                    oCon = Nothing
                End If
            Catch ex As Exception

            End Try

            If Not String.IsNullOrWhiteSpace(strEmailMsg) Then
                LogError("Process Silent Import Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & strEmailMsg, GroupEmail)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ofields"></param>
    ''' <remarks>
    ''' To improve performance we could call this using a thread pool but we would need to modify the executeSQLQuery
    ''' because it reads and writes to properties on the calling thread 
    ''' System.Threading.ThreadPool.QueueUserWorkItem(AddressOf DoAsyncWork)
    ''' </remarks>
    Private Sub deleteExistingNOLaneRecords(ByRef ofields As clsImportFields)

        If ofields Is Nothing Then Return
        Dim strSQL As String = "DELETE FROM [dbo].[POHNoLanes]"
        If Me.AuthorizationCode.Trim.Length > 0 Then
            strSQL &= " WHERE " _
                & " POHNLAUTHCode = '" & Me.AuthorizationCode & "'" _
                & " AND" _
                & " POHNLOrderNumber = " & ofields("PONumber").Value _
                & " AND" _
                & " POHNLOrderSequence = " & ofields("POOrderSequence").Value _
                & " AND" _
                & " POHNLDefaultCustomer = " & ofields("PODefaultCustomer").Value _
                & " AND" _
                & " POHNLnumber = " & ofields("POCustomerPO").Value
        Else
            strSQL &= " WHERE " _
                & " POHNLOrderNumber = " & ofields("PONumber").Value _
                & " AND" _
                & " POHNLOrderSequence = " & ofields("POOrderSequence").Value _
                & " AND" _
                & " POHNLDefaultCustomer = " & ofields("PODefaultCustomer").Value _
                & " AND" _
                & " POHNLnumber = " & ofields("POCustomerPO").Value
        End If
        executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.deleteExistingNOLaneRecords")


    End Sub

    Private Function getItemDetails(ByRef oFields As clsImportFields, ByRef oDetails As BookData.BookDetailDataTable) As BookData.BookDetailDataTable
        Dim strFilter As String = "ItemPONumber = " & oFields("PONumber").Value & " AND POOrderSequence = " & oFields("POOrderSequence").Value & " AND CustomerNumber = " & oFields("PODefaultCustomer").Value

        Dim oTable As New BookData.BookDetailDataTable
        Dim oDRows As BookData.BookDetailRow() = oDetails.Select(strFilter)
        If oDRows.Count < 1 Then
            'No items were found so log the filter we only send an email if debugging is on
            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter:<br />" & vbCrLf & strFilter & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter: " & strFilter)
        Else
            For Each orow As BookData.BookDetailRow In oDRows
                Dim newRow As BookData.BookDetailRow = oTable.NewBookDetailRow
                For i As Integer = 0 To oTable.Columns.Count - 1
                    newRow.Item(i) = orow.Item(i)
                Next
                oTable.AddBookDetailRow(newRow)
                oTable.AcceptChanges()
            Next
        End If
        Return oTable
    End Function

    ''' <summary>
    ''' Returns a list of v-60 item details filtered by PONumber, POOrderSequence, and PODefaultCustomer
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="oDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 9/7/2017
    '''   added logic to auto assign lot numbers if needed sorted by item number based on company level parameter setting
    ''' </remarks>
    Private Function getItemDetails(ByVal oFields As clsImportFields, ByRef oDetails As List(Of clsBookDetailObject60)) As List(Of clsBookDetailObject60)
        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty.")
            Return New List(Of clsBookDetailObject60)
        End If
        Dim strFilter As String = "ItemPONumber = " & oFields("PONumber").Value & " AND POOrderSequence = " & oFields("POOrderSequence").Value & " AND CustomerNumber = " & oFields("PODefaultCustomer").Value
        Dim oDets As List(Of clsBookDetailObject60) = (From d In oDetails
                                                       Where
                                                       d.ItemPONumber = DTran.stripQuotes(oFields("PONumber").Value) _
                                                       And d.POOrderSequence = oFields("POOrderSequence").Value _
                                                       And d.CustomerNumber = DTran.stripQuotes(oFields("PODefaultCustomer").Value)
                                                       Select d).ToList

        If oDets Is Nothing OrElse oDets.Count < 1 Then
            'No items were found so log the filter we only send an email if debugging is on
            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter:<br />" & vbCrLf & strFilter & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter: " & strFilter)
            oDets = New List(Of clsBookDetailObject60)
        Else
            'Modified by RHR for v-7.0.6.105 on 9/7/2017
            '  added logic to auto assign lot numbers if needed sorted by item number based on company level parameter setting
            Dim sErrors As String = ""
            Dim dblVal = GetParValueUsingOrderData("AutoAssignLotNbrToDuplicateItems", oFields("PODefaultCustomer").Value, "", "", sErrors)
            If Not String.IsNullOrWhiteSpace(sErrors) Then
                Log("Read AutoAssignLotNbrToDuplicateItems Failure: " & sErrors)
            End If
            If dblVal = 1 Then
                For i As Integer = 0 To oDets.Count() - 1
                    oDets(i).LotNumber = oDets(i).LotNumber & i.ToString()
                Next
            End If
        End If

        Return oDets
    End Function

    ''' <summary>
    ''' Returns a list of v-604 item details filtered by PONumber, POOrderSequence, and PODefaultCustomer
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="oDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/6/2017
    ''' Modified by RHR for v-7.0.6.105 on 9/7/2017
    '''   added logic to auto assign lot numbers if needed sorted by item number based on company level parameter setting
    ''' </remarks>
    Private Function getItemDetails(ByVal oFields As clsImportFields, ByRef oDetails As List(Of clsBookDetailObject604)) As List(Of clsBookDetailObject604)
        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty.")
            Return New List(Of clsBookDetailObject604)
        End If
        Dim strFilter As String = "ItemPONumber = " & oFields("PONumber").Value & " AND POOrderSequence = " & oFields("POOrderSequence").Value & " AND CustomerNumber = " & oFields("PODefaultCustomer").Value
        Dim oDets As List(Of clsBookDetailObject604) = (From d In oDetails
                                                        Where
                                                       d.ItemPONumber = DTran.stripQuotes(oFields("PONumber").Value) _
                                                       And d.POOrderSequence = oFields("POOrderSequence").Value _
                                                       And d.CustomerNumber = DTran.stripQuotes(oFields("PODefaultCustomer").Value)
                                                        Select d).ToList()

        If oDets Is Nothing OrElse oDets.Count < 1 Then
            'No items were found so log the filter we only send an email if debugging is on
            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter:<br />" & vbCrLf & strFilter & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter: " & strFilter)
            oDets = New List(Of clsBookDetailObject604)
        Else
            'Modified by RHR for v-7.0.6.105 on 9/7/2017
            '  added logic to auto assign lot numbers if needed sorted by item number based on company level parameter setting
            Dim sErrors As String = ""
            Dim dblVal = GetParValueUsingOrderData("AutoAssignLotNbrToDuplicateItems", oFields("PODefaultCustomer").Value, "", "", sErrors)
            If Not String.IsNullOrWhiteSpace(sErrors) Then
                Log("Read AutoAssignLotNbrToDuplicateItems Failure: " & sErrors)
            End If
            If dblVal = 1 Then
                For i As Integer = 0 To oDets.Count() - 1
                    oDets(i).LotNumber = oDets(i).LotNumber & i.ToString()
                Next
            End If
        End If
        Return oDets
    End Function


    ''' <summary>
    ''' Returns a list of v-80 item details filtered by PONumber, POOrderSequence, and PODefaultCustomer
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <param name="oDetails"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 -- 6.0.4.7 on 6/6/2017
    ''' Modified by RHR for v-7.0.6.105 on 9/7/2017
    '''   added logic to auto assign lot numbers if needed sorted by item number based on company level parameter setting
    ''' </remarks>
    Private Function getItemDetails(ByVal oFields As clsImportFields, ByRef oDetails As List(Of clsBookDetailObject80)) As List(Of clsBookDetailObject80)
        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty.")
            Return New List(Of clsBookDetailObject80)
        End If
        Dim PONumber = DTran.stripQuotes(oFields("PONumber").Value)
        Dim POSeq = oFields("POOrderSequence").Value
        Dim CustNbr = DTran.stripQuotes(oFields("PODefaultCustomer").Value)
        Dim LegalEntity = DTran.stripQuotes(oFields("POCompLegalEntity").Value)
        Dim AlphaCode = DTran.stripQuotes(oFields("POCompAlphaCode").Value)
        Dim oDets As List(Of clsBookDetailObject80)
        Dim strFilter As String = ""
        Dim sFKNo As String = DTran.stripQuotes(oFields("ChangeNo").Value)
        If Not String.IsNullOrWhiteSpace(sFKNo) Then
            'try to use the new FK No value 
            oDets = (From d In oDetails
                     Where
                     d IsNot Nothing AndAlso
                     d.ChangeNo = sFKNo
                     Select d).ToList
        ElseIf String.IsNullOrWhiteSpace(CustNbr) OrElse Val(CustNbr) = 0 Then
            'try to use leagal entity
            strFilter = String.Format("ItemPONumber = '{0}' AND POOrderSequence = {1}  AND POItemCompLegalEntity = '{2}' AND POItemCompAlphaCode = '{3}'", PONumber, POSeq, LegalEntity, AlphaCode)
            Dim oTest1 = oDetails.Where(Function(x) x.ItemPONumber = PONumber AndAlso x.POOrderSequence = POSeq).ToList()
            Dim oTest2 = oDetails.Where(Function(x) x.POItemCompLegalEntity = LegalEntity).ToList()
            Dim oTest3 = oDetails.Where(Function(x) x.POItemCompAlphaCode = AlphaCode).ToList()
            Dim oTest4 = oDetails.Where(Function(x) x.ItemPONumber = PONumber _
                                            AndAlso x.POOrderSequence = POSeq _
                                            AndAlso x.POItemCompLegalEntity = LegalEntity _
                                            AndAlso x.POItemCompAlphaCode = AlphaCode).ToList()
            Dim oTest5 = oDetails.Where(Function(x) x IsNot Nothing _
                                            AndAlso x.ItemPONumber = PONumber _
                                            AndAlso x.POOrderSequence = POSeq _
                                            AndAlso x.POItemCompLegalEntity = LegalEntity _
                                            AndAlso x.POItemCompAlphaCode = AlphaCode).ToList()

            oDets = (From d In oDetails
                     Where
                     d IsNot Nothing AndAlso
                     d.ItemPONumber = PONumber _
                     AndAlso d.POOrderSequence = POSeq _
                     AndAlso d.POItemCompLegalEntity = LegalEntity _
                     AndAlso d.POItemCompAlphaCode = AlphaCode
                     Select d).ToList
        Else
            strFilter = String.Format("ItemPONumber = '{0}' AND POOrderSequence = {1}  AND CustomerNumber = '{2}'", PONumber, POSeq, CustNbr)
            oDets = (From d In oDetails
                     Where
                     d IsNot Nothing AndAlso
                     d.ItemPONumber = PONumber _
                     AndAlso d.POOrderSequence = POSeq _
                     AndAlso d.CustomerNumber = CustNbr
                     Select d).ToList
        End If

        If oDets Is Nothing OrElse oDets.Count < 1 Then            'No items were found so log the filter we only send an email if debugging is on
            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter:<br />" & vbCrLf & strFilter & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter: " & strFilter)
            oDets = New List(Of clsBookDetailObject80)
        Else
            'Modified by RHR for v-7.0.6.105 on 9/7/2017
            '  added logic to auto assign lot numbers if needed sorted by item number based on company level parameter setting
            Dim sErrors As String = ""
            Dim dblVal = GetParValueUsingOrderData("AutoAssignLotNbrToDuplicateItems", CustNbr, AlphaCode, LegalEntity, sErrors)
            If Not String.IsNullOrWhiteSpace(sErrors) Then
                Log("Read AutoAssignLotNbrToDuplicateItems Failure: " & sErrors)
            End If
            If dblVal = 1 Then
                For i As Integer = 0 To oDets.Count() - 1
                    oDets(i).LotNumber = oDets(i).LotNumber & i.ToString()
                Next
            End If
        End If
        Return oDets
    End Function

    Private Function updatePOHDRDefaults(ByRef oFields As clsImportFields) As Boolean
        Dim blnRetVal As Boolean = False

        If oFields Is Nothing Then Return False
        Dim oCon As System.Data.SqlClient.SqlConnection
        Dim oCmd As New SqlCommand
        Try
            Dim intRetryCt As Integer = 0

            oCon = getNewConnection(False)
            Do
                intRetryCt += 1

                Try

                    Dim lngErrNumber As Long
                    Dim strRetVal As String = ""
                    With oCmd
                        .Connection = oCon
                        .CommandTimeout = 3600
                        .Parameters.Add("@OrderNumber", SqlDbType.NVarChar, 20)
                        .Parameters("@OrderNumber").Value = If(stripQuotes(oFields("PONumber").Value), "")
                        .Parameters.Add("@OrderSequence", SqlDbType.Int)
                        .Parameters("@OrderSequence").Value = If(stripQuotes(oFields("POOrderSequence").Value), 0)
                        .Parameters.Add("@CustomerNumber", SqlDbType.BigInt)
                        .Parameters("@CustomerNumber").Value = If(oFields("PODefaultCustomer").Value, 0)
                        .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 4000)
                        .Parameters("@RetMsg").Direction = ParameterDirection.Output
                        .Parameters.Add("@ErrNumber", SqlDbType.BigInt)
                        .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                        .CommandText = "spUpdatePOHDRDefaults"
                        .CommandType = CommandType.StoredProcedure
                        .ExecuteNonQuery()
                        strRetVal = Trim(.Parameters("@RetMsg").Value.ToString)
                        If IsDBNull(.Parameters("@ErrNumber").Value) Then
                            lngErrNumber = 0
                        Else
                            lngErrNumber = .Parameters("@ErrNumber").Value
                        End If
                    End With

                    Try
                        If lngErrNumber <> 0 Then
                            If lngErrNumber < 10 Then
                                '******************************  NGL Stored Procedure Error Codes **********************************
                                '0 No error 
                                '1 Parameter variable error
                                '2 Cannot Complete Request like when Costing is locked or No Tariff available so costs are set to zero
                                '3 Invalid Required Value like Transaction Code
                                '4 Attempt to duplicate a previously executed process like when a freight bill has already been processed
                                '5 Attempt to duplicate a previously executed process some action is required like when a the over write option on the AP Mass Entry Table should be selected
                                '6 Invalid User Name
                                '7 Invalid(Password)
                                '8 NET_ADDRESS_FAILURE(Database Is Not valid)
                                '9 Invalid value assigned based on parameter settings like Actual Carrier Assigned is not valid
                                '*******************************************************************************/
                                Dim strMsg As String = "Data validation Error # " & lngErrNumber & ": " & strRetVal & ".'"
                                ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.updatePOHDRDefault.<br />" & vbCrLf & strMsg & "<br />" & vbCrLf & "; for order number " & oFields("PONumber").Value & ".<br />" & vbCrLf
                                Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Failed: " & strMsg)

                            ElseIf intRetryCt > Me.Retry Then
                                ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults: Procedure spUpdatePOHDRDefaults output failed " & intRetryCt.ToString & " times for order number " & oFields("PONumber").Value & ".<br />" & vbCrLf & "Error # " & lngErrNumber & ": " & strRetVal & "<br />" & vbCrLf
                                Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Failed!")
                            Else
                                Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Output Failure Retry = " & intRetryCt.ToString)
                            End If
                        Else
                            blnRetVal = True
                            Exit Do
                        End If
                    Catch ex As Exception

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults, attempted to run spUpdatePOHDRDefaults procedure " & intRetryCt.ToString & " times without success for order number " & oFields("PONumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Execution Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.

        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults, Procedure spUpdatePOHDRDefaults general failure for order number " & oFields("PONumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Failed!" & readExceptionMessage(ex))
        Finally
            Try
                oCmd.Cancel()
                oCmd = Nothing
            Catch ex As Exception

            End Try
            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                If Not oCon Is Nothing Then
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                    oCon.Close()
                    oCon = Nothing
                End If
            Catch ex As Exception

            End Try
        End Try
        Return blnRetVal
    End Function

    Private Sub deleteItemData(ByRef oFields As clsImportFields)
        Dim strSQL As String = "DELETE FROM POItem Where CustomerNumber = " & oFields("PODefaultCustomer").Value & " And ItemPONumber = " & oFields("PONumber").Value & " And POOrderSequence = " & oFields("POOrderSequence").Value
        executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.deleteItemData", False)
    End Sub

    Private Sub saveOrderHistory(ByRef oHeaderRow As BookData.BookHeaderRow, ByRef oDetails As BookData.BookDetailDataTable)
        If oHeaderRow Is Nothing Then Return
        Dim intNewPOHdrHistoryID As Integer
        Dim strNewPOHdrHistoryID As String
        Dim strOrderNumber As String = DTran.NZ(oHeaderRow, "PONumber", "")
        'Check that the Authorization Code is not null
        If String.IsNullOrEmpty(Me.AuthorizationCode) Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The Authorization Code was null or empty." & vbCrLf
            Return 'We cannot continue   
        End If
        Dim strSQL As String = "Exec dbo.spAddPOHdrHistory " _
            & "'" & Me.AuthorizationCode & "'" _
            & DTran.buildSQLString(oHeaderRow, "POCustomerPO", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POvendor", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POdate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POShipdate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POBuyer", "", ",") _
            & "," & DTran.NZ(oHeaderRow, "POFrt", 0) _
            & ",'" & CreateUser & "'" _
            & ",'" & CreatedDate & "'" _
            & ",'" & CreateUser & "'" _
            & "," & DTran.NZ(oHeaderRow, "POTotalFrt", 0) _
            & "," & DTran.NZ(oHeaderRow, "POTotalCost", 0) _
            & "," & DTran.NZ(oHeaderRow, "POWgt", 0) _
            & "," & DTran.NZ(oHeaderRow, "POCube", 0) _
            & "," & DTran.NZ(oHeaderRow, "POQty", 0) _
            & "," & DTran.NZ(oHeaderRow, "POLines", 0) _
            & "," & DTran.NZ(oHeaderRow, "POConfirm", 0) _
            & DTran.buildSQLString(oHeaderRow, "PODefaultCustomer", "", ",") _
            & ",''" _
            & "," & DTran.NZ(oHeaderRow, "PODefaultCarrier", 0) _
            & DTran.buildSQLString(oHeaderRow, "POReqDate", "", ",") _
            & ",'" & strOrderNumber & "'" _
            & DTran.buildSQLString(oHeaderRow, "POShipInstructions", "", ",") _
            & "," & DTran.NZ(oHeaderRow, "POCooler", 0) _
            & "," & DTran.NZ(oHeaderRow, "POFrozen", 0) _
            & "," & DTran.NZ(oHeaderRow, "PODry", 0) _
            & DTran.buildSQLString(oHeaderRow, "POTemp", "", ",") _
            & ",'','','','','','','','',''" _
            & DTran.buildSQLString(oHeaderRow, "POCarType", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POShipVia", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POShipViaType", "", ",") _
            & "," & DTran.NZ(oHeaderRow, "POPallets", 0) _
            & "," & DTran.NZ(oHeaderRow, "POOtherCosts", 0) _
            & "," & DTran.NZ(oHeaderRow, "POStatusFlag", 0) _
            & ",0,'',0,''" _
            & "," & DTran.NZ(oHeaderRow, "POOrderSequence", 0) _
            & DTran.buildSQLString(oHeaderRow, "POChepGLID", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POCarrierEquipmentCodes", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POCarrierTypeCode", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POPalletPositions", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POSchedulePUDate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POSchedulePUTime", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POScheduleDelDate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POSCheduleDelTime", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POActPUDate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POActPUTime", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POActDelDate", "", ",") _
            & DTran.buildSQLString(oHeaderRow, "POActDelTime", "", ",")

        strNewPOHdrHistoryID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory")
        If Not Integer.TryParse(strNewPOHdrHistoryID, intNewPOHdrHistoryID) OrElse intNewPOHdrHistoryID = 0 Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
                & vbCrLf & strSQL & "<hr />" & vbCrLf
            Return 'We cannot continue   
        End If
        Dim sItemHistoryCommands As New List(Of String)
        Dim sItemPONumbers As New List(Of String)
        Dim sItemNumbers As New List(Of String)
        If intNewPOHdrHistoryID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Rows.Count > 0 Then
            'Loop through each item detail record for this order and build an add query            
            For Each oRow As BookData.BookDetailRow In oDetails
                strSQL = "Exec dbo.spAddPOItemHistory " _
                    & intNewPOHdrHistoryID _
                    & ",'" & Me.AuthorizationCode & "'" _
                    & DTran.buildSQLString(oRow, "ItemPONumber", "", ",") _
                    & "," & DTran.NZ(oRow, "FixOffInvAllow", 0) _
                    & "," & DTran.NZ(oRow, "FixFrtAllow", 0) _
                    & DTran.buildSQLString(oRow, "ItemNumber", "", ",") _
                    & "," & DTran.NZ(oRow, "QtyOrdered", 0) _
                    & "," & DTran.NZ(oRow, "FreightCost", 0) _
                    & "," & DTran.NZ(oRow, "ItemCost", 0) _
                    & "," & DTran.NZ(oRow, "Weight", 0) _
                    & "," & DTran.NZ(oRow, "Cube", 0) _
                    & "," & DTran.NZ(oRow, "Pack", 0) _
                    & DTran.buildSQLString(oRow, "Size", "", ",") _
                    & DTran.buildSQLString(oRow, "Description", "", ",") _
                    & DTran.buildSQLString(oRow, "Hazmat", "", ",") _
                    & ",'" & CreateUser & "'" _
                    & ",'" & CreatedDate & "'" _
                    & DTran.buildSQLString(oRow, "Brand", "", ",") _
                    & DTran.buildSQLString(oRow, "CostCenter", "", ",") _
                    & DTran.buildSQLString(oRow, "LotNumber", "", ",") _
                    & DTran.buildSQLString(oRow, "LotExpirationDate", "", ",") _
                    & DTran.buildSQLString(oRow, "GTIN", "", ",") _
                    & DTran.buildSQLString(oRow, "CustItemNumber", "", ",") _
                    & DTran.buildSQLString(oRow, "CustomerNumber", "0", ",") _
                    & "," & DTran.NZ(oRow, "POOrderSequence", 0) _
                    & DTran.buildSQLString(oRow, "PalletType", "", ",")

                'We do not add the following new 6.0 fields to the query yet maybe in the future
                'For now we only update these fields when called by the 6.0 methods
                '& DTran.buildSQLString(oRow, "POItemHistoryHazmatTypeCode", "", ",") _
                '& DTran.buildSQLString(oRow, "POItemHistory49CFRCode", "", ",") _
                '& DTran.buildSQLString(oRow, "POItemHistoryIATACode", "", ",") _
                '& DTran.buildSQLString(oRow, "POItemHistoryDOTCode", "", ",") _
                '& DTran.buildSQLString(oRow, "POItemHistoryMarineCode", "", ",") _
                '& DTran.buildSQLString(oRow, "POItemHistoryNMFCClass", "", ",") _
                '& DTran.buildSQLString(oRow, "POItemHistoryFAKClass", "", ",") _
                '& "," & DTran.NZ(oRow, "POItemHistoryLimitedQtyFlag", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryPallets", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryTies", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryHighs", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryQtyPalletPercentage", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryQtyLength", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryQtyWidth", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryQtyHeight", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryStackable", 0) _
                '& "," & DTran.NZ(oRow, "POItemHistoryLevelOfDensity", 0)
                sItemHistoryCommands.Add(strSQL)
                If Not sItemPONumbers.Contains(DTran.NZ(oRow, "ItemPONumber", "")) Then sItemPONumbers.Add(DTran.NZ(oRow, "ItemPONumber", ""))
                If Not sItemNumbers.Contains(DTran.NZ(oRow, "ItemNumber", "")) Then sItemNumbers.Add(DTran.NZ(oRow, "ItemNumber", ""))
            Next
            'Build the delete query
            strSQL = "Delete From dbo.POItemHistory Where POItemHistoryPOHdrHistoryControl = " & intNewPOHdrHistoryID
            Dim blnUseOr As Boolean = False
            Dim sSpacer As String = ""
            If sItemPONumbers.Count > 0 Then
                blnUseOr = True
                strSQL &= " AND (ItemPONumber NOT IN ("
                For Each s As String In sItemPONumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                strSQL &= ") "
            End If
            If sItemNumbers.Count > 0 Then
                If blnUseOr Then
                    strSQL &= " OR ItemNumber NOT IN ("
                Else
                    strSQL &= " AND ItemNumber NOT IN ("
                End If
                sSpacer = ""
                For Each s As String In sItemNumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                If blnUseOr Then
                    strSQL &= "))"
                Else
                    strSQL &= ")"
                End If
            ElseIf blnUseOr Then
                strSQL &= ")"
            End If

            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to delete existing POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
            End If
            'Now execute each add item commands
            For Each s As String In sItemHistoryCommands
                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save new POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
                End If
            Next
        End If

    End Sub

    Private Sub saveOrderHistory(ByRef oHeaderRow As clsBookHeaderObject60, ByRef oDetails As List(Of clsBookDetailObject60))
        If oHeaderRow Is Nothing Then Return
        Dim intNewPOHdrHistoryID As Integer
        Dim strNewPOHdrHistoryID As String
        Dim strOrderNumber As String = oHeaderRow.PONumber
        Dim strSQL As String = "Exec dbo.spAddPOHdrHistory " _
            & "'" & Me.AuthorizationCode & "'" _
            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
            & "," & oHeaderRow.POFrt _
            & ",'" & CreateUser & "'" _
            & ",'" & CreatedDate & "'" _
            & ",'" & CreateUser & "'" _
            & "," & oHeaderRow.POTotalFrt _
            & "," & oHeaderRow.POTotalCost _
            & "," & oHeaderRow.POWgt _
            & "," & oHeaderRow.POCube _
            & "," & oHeaderRow.POQty _
            & "," & oHeaderRow.POLines _
            & "," & If(oHeaderRow.POConfirm, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
            & ",''" _
            & "," & oHeaderRow.PODefaultCarrier _
            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
            & ",'" & strOrderNumber & "'" _
            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
            & "," & If(oHeaderRow.POCooler, "1", "0") _
            & "," & If(oHeaderRow.POFrozen, "1", "0") _
            & "," & If(oHeaderRow.PODry, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
            & ",'','','','','','','','',''" _
            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
            & "," & oHeaderRow.POPallets _
            & "," & oHeaderRow.POOtherCosts _
            & "," & oHeaderRow.POStatusFlag _
            & ",0,'',0,''" _
            & "," & oHeaderRow.POOrderSequence _
            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
            & "," & If(oHeaderRow.POInbound, "1", "0") _
            & "," & oHeaderRow.PODefaultRouteSequence _
            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")

        strNewPOHdrHistoryID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory")
        If Not Integer.TryParse(strNewPOHdrHistoryID, intNewPOHdrHistoryID) Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
                & vbCrLf & strSQL & "<hr />" & vbCrLf
            Return 'We cannot continue   
        End If
        Dim sItemHistoryCommands As New List(Of String)
        Dim sItemPONumbers As New List(Of String)
        Dim sItemNumbers As New List(Of String)
        If intNewPOHdrHistoryID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
            'Loop through each item detail record for this order and build an add query            
            For Each oRow As clsBookDetailObject60 In oDetails
                strSQL = "Exec dbo.spAddPOItemHistory " _
                    & intNewPOHdrHistoryID _
                    & ",'" & Me.AuthorizationCode & "'" _
                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
                    & "," & oRow.FixOffInvAllow _
                    & "," & oRow.FixFrtAllow _
                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
                    & "," & oRow.QtyOrdered _
                    & "," & oRow.FreightCost _
                    & "," & oRow.ItemCost _
                    & "," & oRow.Weight _
                    & "," & oRow.Cube _
                    & "," & oRow.Pack _
                    & DTran.buildSQLString(oRow.Size, 255, ",") _
                    & DTran.buildSQLString(oRow.Description, 255, ",") _
                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
                    & ",'" & CreateUser & "'" _
                    & ",'" & CreatedDate & "'" _
                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
                    & "," & oRow.CustomerNumber _
                    & "," & oRow.POOrderSequence _
                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
                    & "," & oRow.POItemPallets _
                    & "," & oRow.POItemTies _
                    & "," & oRow.POItemHighs _
                    & "," & oRow.POItemQtyPalletPercentage _
                    & "," & oRow.POItemQtyLength _
                    & "," & oRow.POItemQtyWidth _
                    & "," & oRow.POItemQtyHeight _
                    & "," & If(oRow.POItemStackable, "1", "0") _
                    & "," & oRow.POItemLevelOfDensity


                sItemHistoryCommands.Add(strSQL)
                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
            Next
            'Build the delete query
            strSQL = "Delete From dbo.POItemHistory Where POItemHistoryPOHdrHistoryControl = " & intNewPOHdrHistoryID
            Dim blnUseOr As Boolean = False
            Dim sSpacer As String = ""
            If sItemPONumbers.Count > 0 Then
                blnUseOr = True
                strSQL &= " AND (ItemPONumber NOT IN ("
                For Each s As String In sItemPONumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                strSQL &= ") "
            End If
            If sItemNumbers.Count > 0 Then
                If blnUseOr Then
                    strSQL &= " OR ItemNumber NOT IN ("
                Else
                    strSQL &= " AND ItemNumber NOT IN ("
                End If
                sSpacer = ""
                For Each s As String In sItemNumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                If blnUseOr Then
                    strSQL &= "))"
                Else
                    strSQL &= ")"
                End If
            ElseIf blnUseOr Then
                strSQL &= ")"
            End If

            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to delete existing POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
            End If
            'Now execute each add item commands
            For Each s As String In sItemHistoryCommands
                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save new POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' save the order information to the history tables
    ''' </summary>
    ''' <param name="oHeaderRow"></param>
    ''' <param name="oDetails"></param>
    ''' <remarks>
    ''' Modified by RHR On 6/1/2017 For v-7.0.6.105
    '''    added support for 604 objects
    ''' </remarks>
    Private Sub saveOrderHistory(ByRef oHeaderRow As clsBookHeaderObject604, ByRef oDetails As List(Of clsBookDetailObject604))
        If oHeaderRow Is Nothing Then Return
        Dim intNewPOHdrHistoryID As Integer
        Dim strNewPOHdrHistoryID As String
        Dim strOrderNumber As String = oHeaderRow.PONumber
        Dim strSQL As String = "Exec dbo.spAddPOHdrHistory " _
            & "'" & Me.AuthorizationCode & "'" _
            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
            & "," & oHeaderRow.POFrt _
            & ",'" & CreateUser & "'" _
            & ",'" & CreatedDate & "'" _
            & ",'" & CreateUser & "'" _
            & "," & oHeaderRow.POTotalFrt _
            & "," & oHeaderRow.POTotalCost _
            & "," & oHeaderRow.POWgt _
            & "," & oHeaderRow.POCube _
            & "," & oHeaderRow.POQty _
            & "," & oHeaderRow.POLines _
            & "," & If(oHeaderRow.POConfirm, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
            & ",''" _
            & "," & oHeaderRow.PODefaultCarrier _
            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
            & ",'" & strOrderNumber & "'" _
            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
            & "," & If(oHeaderRow.POCooler, "1", "0") _
            & "," & If(oHeaderRow.POFrozen, "1", "0") _
            & "," & If(oHeaderRow.PODry, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
            & ",'','','','','','','','',''" _
            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
            & "," & oHeaderRow.POPallets _
            & "," & oHeaderRow.POOtherCosts _
            & "," & oHeaderRow.POStatusFlag _
            & ",0,'',0,''" _
            & "," & oHeaderRow.POOrderSequence _
            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
            & "," & If(oHeaderRow.POInbound, "1", "0") _
            & "," & oHeaderRow.PODefaultRouteSequence _
            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")

        strNewPOHdrHistoryID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory")
        If Not Integer.TryParse(strNewPOHdrHistoryID, intNewPOHdrHistoryID) Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
                & vbCrLf & strSQL & "<hr />" & vbCrLf
            Return 'We cannot continue   
        End If
        Dim sItemHistoryCommands As New List(Of String)
        Dim sItemPONumbers As New List(Of String)
        Dim sItemNumbers As New List(Of String)
        If intNewPOHdrHistoryID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
            'Loop through each item detail record for this order and build an add query            
            For Each oRow As clsBookDetailObject60 In oDetails
                strSQL = "Exec dbo.spAddPOItemHistory " _
                    & intNewPOHdrHistoryID _
                    & ",'" & Me.AuthorizationCode & "'" _
                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
                    & "," & oRow.FixOffInvAllow _
                    & "," & oRow.FixFrtAllow _
                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
                    & "," & oRow.QtyOrdered _
                    & "," & oRow.FreightCost _
                    & "," & oRow.ItemCost _
                    & "," & oRow.Weight _
                    & "," & oRow.Cube _
                    & "," & oRow.Pack _
                    & DTran.buildSQLString(oRow.Size, 255, ",") _
                    & DTran.buildSQLString(oRow.Description, 255, ",") _
                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
                    & ",'" & CreateUser & "'" _
                    & ",'" & CreatedDate & "'" _
                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
                    & "," & oRow.CustomerNumber _
                    & "," & oRow.POOrderSequence _
                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
                    & "," & oRow.POItemPallets _
                    & "," & oRow.POItemTies _
                    & "," & oRow.POItemHighs _
                    & "," & oRow.POItemQtyPalletPercentage _
                    & "," & oRow.POItemQtyLength _
                    & "," & oRow.POItemQtyWidth _
                    & "," & oRow.POItemQtyHeight _
                    & "," & If(oRow.POItemStackable, "1", "0") _
                    & "," & oRow.POItemLevelOfDensity


                sItemHistoryCommands.Add(strSQL)
                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
            Next
            'Build the delete query
            strSQL = "Delete From dbo.POItemHistory Where POItemHistoryPOHdrHistoryControl = " & intNewPOHdrHistoryID
            Dim blnUseOr As Boolean = False
            Dim sSpacer As String = ""
            If sItemPONumbers.Count > 0 Then
                blnUseOr = True
                strSQL &= " AND (ItemPONumber NOT IN ("
                For Each s As String In sItemPONumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                strSQL &= ") "
            End If
            If sItemNumbers.Count > 0 Then
                If blnUseOr Then
                    strSQL &= " OR ItemNumber NOT IN ("
                Else
                    strSQL &= " AND ItemNumber NOT IN ("
                End If
                sSpacer = ""
                For Each s As String In sItemNumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                If blnUseOr Then
                    strSQL &= "))"
                Else
                    strSQL &= ")"
                End If
            ElseIf blnUseOr Then
                strSQL &= ")"
            End If

            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to delete existing POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
            End If
            'Now execute each add item commands
            For Each s As String In sItemHistoryCommands
                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save new POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
                End If
            Next
        End If

    End Sub

    Private Sub saveOrderHistory(ByRef oHeaderRow As clsBookHeaderObject80, ByRef oDetails As List(Of clsBookDetailObject80))
        If oHeaderRow Is Nothing Then Return
        Dim intNewPOHdrHistoryID As Integer
        Dim strNewPOHdrHistoryID As String
        Dim strOrderNumber As String = oHeaderRow.PONumber
        Dim strSQL As String = "Exec dbo.spAddPOHdrHistory80 " _
            & "'" & Me.AuthorizationCode & "'" _
            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
            & "," & oHeaderRow.POFrt _
            & ",'" & CreateUser & "'" _
            & ",'" & CreatedDate & "'" _
            & ",'" & CreateUser & "'" _
            & "," & oHeaderRow.POTotalFrt _
            & "," & oHeaderRow.POTotalCost _
            & "," & oHeaderRow.POWgt _
            & "," & oHeaderRow.POCube _
            & "," & oHeaderRow.POQty _
            & "," & oHeaderRow.POLines _
            & "," & If(oHeaderRow.POConfirm, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
            & ",''" _
            & "," & oHeaderRow.PODefaultCarrier _
            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
            & ",'" & strOrderNumber & "'" _
            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
            & "," & If(oHeaderRow.POCooler, "1", "0") _
            & "," & If(oHeaderRow.POFrozen, "1", "0") _
            & "," & If(oHeaderRow.PODry, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
            & ",'','','','','','','','',''" _
            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
            & "," & oHeaderRow.POPallets _
            & "," & oHeaderRow.POOtherCosts _
            & "," & oHeaderRow.POStatusFlag _
            & ",0,'',0,''" _
            & "," & oHeaderRow.POOrderSequence _
            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
            & "," & If(oHeaderRow.POInbound, "1", "0") _
            & "," & oHeaderRow.PODefaultRouteSequence _
            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POAPGLNumber, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCompLegalEntity, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POCompAlphaCode, 50, ",") _
            & "," & oHeaderRow.POModeTypeControl _
            & DTran.buildSQLString(oHeaderRow.POMustLeaveByDateTime, 22, ",") _
            & DTran.buildSQLString(oHeaderRow.POUser1, 4000, ",") _
            & DTran.buildSQLString(oHeaderRow.POUser2, 4000, ",") _
            & DTran.buildSQLString(oHeaderRow.POUser3, 4000, ",") _
            & DTran.buildSQLString(oHeaderRow.POUser4, 4000, ",") _
            & DTran.buildSQLString(oHeaderRow.POCarrBLNumber, 20, ",") _
            & DTran.buildSQLString(oHeaderRow.POWhseAuthorizationNo, 20, ",") _
            & DTran.buildSQLString(oHeaderRow.POOrigContactEmail, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.PODestContactEmail, 50, ",") _
            & DTran.buildSQLString(oHeaderRow.POWhseReleaseNo, 20, ",")

        strNewPOHdrHistoryID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory")
        If Not Integer.TryParse(strNewPOHdrHistoryID, intNewPOHdrHistoryID) Then
            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
                & strOrderNumber _
                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
                & vbCrLf & strSQL & "<hr />" & vbCrLf
            Return 'We cannot continue   
        End If
        Dim sItemHistoryCommands As New List(Of String)
        Dim sItemPONumbers As New List(Of String)
        Dim sItemNumbers As New List(Of String)
        If intNewPOHdrHistoryID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
            'Loop through each item detail record for this order and build an add query            
            For Each oRow As clsBookDetailObject80 In oDetails
                strSQL = "Exec dbo.spAddPOItemHistory82 " _
                    & intNewPOHdrHistoryID _
                    & ",'" & Me.AuthorizationCode & "'" _
                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
                    & "," & oRow.FixOffInvAllow _
                    & "," & oRow.FixFrtAllow _
                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
                    & "," & oRow.QtyOrdered _
                    & "," & oRow.FreightCost _
                    & "," & oRow.ItemCost _
                    & "," & oRow.Weight _
                    & "," & oRow.Cube _
                    & "," & oRow.Pack _
                    & DTran.buildSQLString(oRow.Size, 255, ",") _
                    & DTran.buildSQLString(oRow.Description, 255, ",") _
                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
                    & ",'" & CreateUser & "'" _
                    & ",'" & CreatedDate & "'" _
                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
                    & "," & oRow.CustomerNumber _
                    & "," & oRow.POOrderSequence _
                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
                    & "," & oRow.POItemPallets _
                    & "," & oRow.POItemTies _
                    & "," & oRow.POItemHighs _
                    & "," & oRow.POItemQtyPalletPercentage _
                    & "," & oRow.POItemQtyLength _
                    & "," & oRow.POItemQtyWidth _
                    & "," & oRow.POItemQtyHeight _
                    & "," & If(oRow.POItemStackable, "1", "0") _
                    & "," & oRow.POItemLevelOfDensity _
                    & DTran.buildSQLString(oRow.POItemCompLegalEntity, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemCompAlphaCode, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemNMFCSubClass, 20, ",") _
                    & DTran.buildSQLString(oRow.POItemUser1, 4000, ",") _
                    & DTran.buildSQLString(oRow.POItemUser2, 4000, ",") _
                    & DTran.buildSQLString(oRow.POItemUser3, 4000, ",") _
                    & DTran.buildSQLString(oRow.POItemUser4, 4000, ",") _
                    & DTran.buildSQLString(oRow.BookItemCommCode, 1, ",") _
                    & DTran.buildSQLString(oRow.POItemCustomerPO, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemLocationCode, 50, ",") _
                    & DTran.buildSQLString(oRow.POItemOrderNumber, 20, ",")

                sItemHistoryCommands.Add(strSQL)
                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
            Next
            'Build the delete query
            strSQL = "Delete From dbo.POItemHistory Where POItemHistoryPOHdrHistoryControl = " & intNewPOHdrHistoryID
            Dim blnUseOr As Boolean = False
            Dim sSpacer As String = ""
            If sItemPONumbers.Count > 0 Then
                blnUseOr = True
                strSQL &= " AND (ItemPONumber NOT IN ("
                For Each s As String In sItemPONumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                strSQL &= ") "
            End If
            If sItemNumbers.Count > 0 Then
                If blnUseOr Then
                    strSQL &= " OR ItemNumber NOT IN ("
                Else
                    strSQL &= " AND ItemNumber NOT IN ("
                End If
                sSpacer = ""
                For Each s As String In sItemNumbers
                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
                    sSpacer = ","
                Next
                If blnUseOr Then
                    strSQL &= "))"
                Else
                    strSQL &= ")"
                End If
            ElseIf blnUseOr Then
                strSQL &= ")"
            End If

            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to delete existing POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
            End If
            'Now execute each add item commands
            For Each s As String In sItemHistoryCommands
                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save new POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
                End If
            Next
        End If

    End Sub


    Private Function runDeleteOrderWithData(ByVal strBookProNumber As String,
                                                    ByVal strOrderNumber As String,
                                                    ByVal intOrderSequence As Integer,
                                                    ByVal intDefCompNumber As Integer,
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        Dim blnRet As Boolean = False
        Dim oCon As New System.Data.SqlClient.SqlConnection
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
        Try
            'Check Data
            If String.IsNullOrEmpty(strBookProNumber) OrElse strBookProNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'PRO Number' value is invalid."
            End If

            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If blnDataErr Then Return False

            ' Ngl.Core.Data.Query(Me.DBServer, Me.Database)
            'Update the existing PO Data.

            oCon = getNewConnection(False)
            oCmd.Parameters.AddWithValue("@BookProNumber", strBookProNumber)
            If oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spDeleteBookingByPro", 3) Then
                blnRet = runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strMSG)
            Else
                blnRet = False
            End If
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Delete Booking Data Failure! Failed to execute dbo.spDeleteBookingByPro stored procedure without success: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Delete Booking Data Failure! Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "Delete Booking Data Failure! Database access failure : " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseDataValidationException
            strMSG = ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                oCmd.Cancel()
                oCmd = Nothing
            Catch ex As Exception

            End Try
            Try
                oCon.Close()
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return blnRet

    End Function

    Private Function runRemoveDeletedWithData(ByVal strOrderNumber As String,
                                                    ByVal intOrderSequence As Integer,
                                                    ByVal intDefCompNumber As Integer,
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        Dim blnRet As Boolean = False
        Dim oCon As New System.Data.SqlClient.SqlConnection
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)

        Try
            'Check Data
            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If blnDataErr Then Return False
            oCon = getNewConnection(False)
            'Update the existing PO Data.
            oCmd.Parameters.AddWithValue("@POHdrOrderNumber", strOrderNumber)
            oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
            oCmd.Parameters.AddWithValue("@POHDRDefaultCustomer", intDefCompNumber)
            blnRet = oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spRemoveDeletedPOByComp", 3)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Remove Deleted Booking Data Failure! Failed to execute dbo.spRemoveDeletedPOByComp stored procedure without success: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Remove Deleted Booking Data Failure! Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "Remove Deleted Booking Data Failure! Database access failure : " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseDataValidationException
            strMSG = ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                oCmd.Cancel()
                oCmd = Nothing
            Catch ex As Exception

            End Try
            Try
                oCon.Close()
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return blnRet

    End Function

    Private Function runUpdatePOModificationWithData(ByVal strOrderNumber As String,
                                                    ByVal intOrderSequence As Integer,
                                                    ByVal intDefCompNumber As Integer,
                                                    ByVal strVendorNumber As String,
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        Dim blnRet As Boolean = False

        Dim oCon As New System.Data.SqlClient.SqlConnection
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)

        Try
            'Check Data
            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If String.IsNullOrEmpty(strVendorNumber) OrElse strVendorNumber.Trim.Length < 1 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Lane Number' value is invalid."
            End If
            If blnDataErr Then Return False

            oCon = getNewConnection(False)
            oCmd.Parameters.AddWithValue("@POHdrOrderNumber", strOrderNumber)
            oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
            oCmd.Parameters.AddWithValue("@POHdrDefCustNumber", intDefCompNumber)
            oCmd.Parameters.AddWithValue("@POHDRvendor", strVendorNumber)
            blnRet = oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spUpdateBookingRecord", 3)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Update PO Modifications Data Failure! Failed to execute dbo.spUpdateBookingRecord stored procedure without success: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Update PO Modifications Data Failure! Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "Update PO Modifications Data Failure! Database access failure : " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseDataValidationException
            strMSG = ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                oCmd.Cancel()
                oCmd = Nothing
            Catch ex As Exception

            End Try
            Try
                oCon.Close()
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try


        Return blnRet

    End Function


    ''' <summary>
    ''' Returns True on Success or False on Failure
    ''' </summary>
    ''' <param name="strOrderNumber"></param>
    ''' <param name="intOrderSequence"></param>
    ''' <param name="intDefCompNumber"></param>
    ''' <param name="strMSG"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 1/8/2016 fixed bug where the procedure returned 
    ''' false on success by settin the default return value blnRet to true
    ''' </remarks>
    Private Function runProcessFinalizedData(ByVal strOrderNumber As String,
                                                    ByVal intOrderSequence As Integer,
                                                    ByVal intDefCompNumber As Integer,
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        'RHR 1/8/2016
        Dim blnRet As Boolean = True
        Try
            'Check Data
            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If blnDataErr Then Return False
            Dim oBookBLL As New BLL.NGLBookBLL(WCFDataProperties.ConvertToWCFProperties(New DAL.WCFParameters()))
            strMSG = oBookBLL.SilentTenderFinalized(strOrderNumber, intOrderSequence, intDefCompNumber)
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            strMSG = sqlEx.Detail.ToString(sqlEx.Reason.ToString())
            Return False
        Catch ex As Exception
            'For Silent Tender we log all errors
            blnDataErr = True
            strMSG = "Silent Tender Finalized Load Failure: " & ex.Message
            Return False
        End Try

        Return blnRet

    End Function

    Private Function ImportPOHdr(ByVal strPOHDRModVerify As String,
                                 ByVal strOrderNumber As String,
                                 ByVal intOrderSequence As Integer,
                                 ByVal intDefCompNumber As Integer,
                                 ByVal strVendorNumber As String,
                                 ByVal strBookProNumber As String,
                                 ByVal strSource As String,
                                 ByRef strErrMsg As String,
                                 ByVal blnSendAsBatch As Boolean) As Boolean
        'Dim strUser As String = updatePerson(oParameters)
        Dim strUser As String = WCFDataProperties.UserName
        Dim blnSuccess As Boolean = False

        Try
            'SaveAppError("Importing POHdr Records WS: " & strUser)
            Dim oParameters = New DAL.WCFParameters With {
                .CompControl = WCFDataProperties.CompControl,
                .ConnectionString = WCFDataProperties.ConnectionString,
                .Database = WCFDataProperties.Database,
                .DBServer = WCFDataProperties.DBServer,
                .FormControl = WCFDataProperties.FormControl,
                .FormName = WCFDataProperties.FormName,
                .ProcedureControl = WCFDataProperties.ProcedureControl,
                .ProcedureName = WCFDataProperties.ProcedureName,
                .ReportControl = WCFDataProperties.ReportControl,
                .ReportName = WCFDataProperties.ReportName,
                .SSOAClientSecret = WCFDataProperties.SSOAClientSecret,
                .SSOAName = WCFDataProperties.SSOAName,
                .USATToken = WCFDataProperties.USATToken,
                .USATUserID = WCFDataProperties.USATUserID,
                .UseExceptionEvents = WCFDataProperties.UseExceptionEvents,
                .UserName = WCFDataProperties.UserName,
                .UserRemotePassword = WCFDataProperties.UserRemotePassword,
                .UseToken = WCFDataProperties.UseToken,
                .ValidateAccess = WCFDataProperties.ValidateAccess,
                .WCFAuthCode = WCFDataProperties.WCFAuthCode,
                .WCFServiceURL = WCFDataProperties.WCFServiceURL
                }

            Dim oSQL As New BLL.NGLOrderImportBLL(oParameters)
            blnSuccess = oSQL.ImportPOHdrRecord(strPOHDRModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber)
        Catch sqlEx As FaultException(Of NGL.FreightMaster.Data.SqlFaultInfo)
            strErrMsg = sqlEx.Detail.getMsgForLogs(sqlEx.Reason.ToString())
            'SaveAppError(strErrMsg)
        Catch ex As Exception
            strErrMsg = "ImportPOHdrRecord Unexpected Error : " & strUser & ": " & ex.Message
            'SaveAppError(strErrMsg)
        End Try
        Return blnSuccess
    End Function


    'Private Function ImportPOHdr(ByVal strPOHDRModVerify As String, _
    '                             ByVal strOrderNumber As String, _
    '                             ByVal intOrderSequence As Integer, _
    '                             ByVal intDefCompNumber As Integer, _
    '                             ByVal strVendorNumber As String, _
    '                             ByVal strBookProNumber As String,
    '                             ByVal strSource As String,
    '                             ByRef strErrMsg As String,
    '                             ByVal blnSendAsBatch As Boolean) As Boolean


    '    Dim blnRet As Boolean = False
    '    strSource &= ".ImportPOHdr"
    '    Try
    '        SyncLock mPadLock
    '            With SharedServices 'Me.PaneSettings.MainInterface.SharedServices
    '                If Not .UserConnected Then
    '                    .LogOn(WCFDataProperties)
    '                End If

    '                'mintSharedServicesRunning += 1
    '            End With
    '        End SyncLock

    '        Dim oRetData As NGL.FMWCFProxy.NGLSharedServicesData.NGLSharedServiceBatchData = Me.SharedServices.ImportPOHdrRecordWS(strPOHDRModVerify,
    '                                                  strOrderNumber,
    '                                                  intOrderSequence,
    '                                                  intDefCompNumber,
    '                                                  strVendorNumber,
    '                                                  strBookProNumber,
    '                                                  WCFDataProperties, New NGL.FMWCFProxy.NGLSharedServicesData.NGLSharedServiceBatchData() With {.Caller = "clsBook", .CallerMethod = "ImportPOHdr", .BatchProcess = blnSendAsBatch})
    '        If oRetData.Success AndAlso String.IsNullOrEmpty(oRetData.LastError) Then
    '            blnRet = True
    '        Else
    '            strErrMsg = oRetData.LastError
    '        End If

    '        'Me.SharedServices.ImportPOHdrRecord(strPOHDRModVerify,
    '        '                                         strOrderNumber,
    '        '                                         intOrderSequence,
    '        '                                         intDefCompNumber,
    '        '                                         strVendorNumber,
    '        '                                         strBookProNumber,
    '        '                                         WCFDataProperties, New Ngl.FMWCFProxy.NGLSharedServicesData.NGLSharedServiceBatchData() With {.Caller = "clsBook", .CallerMethod = "ImportPOHdr", .BatchProcess = blnSendAsBatch})

    '        'blnRet = True
    '    Catch ex As System.ObjectDisposedException
    '        strErrMsg = strSource & " " & ex.ToString
    '    Catch ex As InvalidOperationException
    '        strErrMsg = strSource & " " & ex.ToString
    '    Catch ex As Exception
    '        Throw
    '    Finally
    '        'SyncLock mPadLock
    '        '    mintSharedServicesRunning -= 1
    '        '    If mintSharedServicesRunning < 1 Then
    '        '        If SharedServices.UserConnected Then SharedServices.LogOff(WCFDataProperties)
    '        '    End If
    '        'End SyncLock
    '    End Try


    '    Return blnRet
    'End Function

#End Region

#Region "Public Functions "

    ''' <summary>
    ''' Look up existing order in TMS by Order Number (prvided) and update the POHDR status using the delete code
    ''' </summary>
    ''' <param name="OrderNumber"></param>
    ''' <param name="sConnectionString"></param>
    ''' <returns></returns> 
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.007 on 11/15/2020 
    '''     part of the CPF requirement for backward compatibility     
    ''' </remarks>
    Public Function ProcessDeleteByOrderNumber(ByVal OrderNumber As String, ByVal sConnectionString As String) As ProcessDataReturnValues
        Dim intRet As New ProcessDataReturnValues
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsBook.ProcessDeleteByOrderNumber"
        intRet = ProcessDataReturnValues.nglDataIntegrationComplete ' set default to true/success
        Try
            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = "System Download"
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = Me.ConnectionString
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With
            Dim oBookData = New DAL.NGLBookData(oWCFParameters)
            Dim oBook As DTO.Book = oBookData.GetBookFiltered(BookCarrOrderNumber:=OrderNumber)
            If Not oBook Is Nothing AndAlso oBook.BookControl <> 0 Then
                Dim lOrderHeaders As New List(Of Ngl.FreightMaster.Integration.clsBookHeaderObject80)
                Dim sLaneNumber As String = ""
                Dim oLaneWCFData = New DAL.NGLLaneData(oWCFParameters)
                Dim oLane As DTO.Lane = oLaneWCFData.GetRecordFiltered(oBook.BookODControl)
                Dim oCompWCFData = New DAL.NGLCompData(oWCFParameters)
                Dim oComp As DTO.Comp = oCompWCFData.GetRecordFiltered(oBook.BookCustCompControl)
                If (Not oLane Is Nothing AndAlso oLane.LaneControl <> 0) AndAlso (Not oComp Is Nothing AndAlso oComp.CompControl <> 0) Then
                    Dim oHeader As New Ngl.FreightMaster.Integration.clsBookHeaderObject80
                    With oHeader
                        .PONumber = OrderNumber
                        .POdate = Date.Now()
                        .PODefaultCustomer = oComp.CompNumber
                        .POVendor = oLane.LaneNumber
                        .POFrt = 0
                        .POTotalFrt = 0
                        .POTotalCost = 0
                        .POWgt = 0
                        .POCube = 0
                        .POQty = 0
                        .POPallets = 0
                        .POLines = 0
                        .POConfirm = False
                        .PODefaultCarrier = 0
                        .POCooler = False
                        .POFrozen = False
                        .PODry = False
                        .POOtherCosts = 0
                        .POStatusFlag = 2
                        .POOrderSequence = 0
                        .POInbound = False
                        .POPalletExchange = False
                        .PODefaultRouteSequence = 0
                        .PORecMinIn = 0
                        .PORecMinUnload = 0
                        .PORecMinOut = 0
                        .POAppt = False
                        .POBFC = 0
                        .POModeTypeControl = 0
                    End With
                    lOrderHeaders.Add(oHeader)
                    Dim lOrderDetails As New List(Of Ngl.FreightMaster.Integration.clsBookDetailObject80)
                    Dim oDetail As New Ngl.FreightMaster.Integration.clsBookDetailObject80()
                    With oDetail
                        .ItemPONumber = OrderNumber
                        .CustomerNumber = oComp.CompNumber
                    End With
                    lOrderDetails.Add(oDetail)
                    intRet = ProcessObjectData(lOrderHeaders, lOrderDetails, sConnectionString)
                    If intRet = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors Then
                        intRet = ProcessDataReturnValues.nglDataIntegrationComplete ' set  to true/success even if we have a few issues with item details.
                    End If
                Else
                    strMsg = "The order number " & OrderNumber & " does not have a valid company or lane assigned.  No booking records were deleted."
                    LastError = strMsg
                    intRet = ProcessDataReturnValues.nglDataIntegrationFailure
                End If
            Else
                strMsg = "The order number " & OrderNumber & " cannot be found.  No booking records were deleted."
                LastError = strMsg
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process Delete Order By Order Number Failure", "Could not process the requested data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessDeleteByOrderNumber")

        End Try
        Return intRet
    End Function


    Public Function getHeaderTableFromRow(ByRef oRow As BookData.BookHeaderRow) As BookData.BookHeaderDataTable
        Dim oTable As New BookData.BookHeaderDataTable
        Dim newRow As BookData.BookHeaderRow = oTable.NewBookHeaderRow
        For i As Integer = 0 To oTable.Columns.Count - 1
            newRow.Item(i) = oRow.Item(i)
        Next
        oTable.AddBookHeaderRow(newRow)
        Return oTable
    End Function

    Public Function ProcessObjectData(
                    ByVal oOrders() As clsBookHeaderObject,
                    ByVal oDetails() As clsBookDetailObject,
                    ByVal strConnection As String) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim oHTable As New BookData.BookHeaderDataTable
        Dim oDTable As New BookData.BookDetailDataTable
        Dim dtVal As Date
        Try
            For Each oOrder As clsBookHeaderObject In oOrders
                If Not oOrder Is Nothing Then
                    Dim oRow As BookData.BookHeaderRow = oHTable.NewBookHeaderRow
                    With oRow
                        .PONumber = Left(oOrder.PONumber, 20)
                        .POVendor = Left(oOrder.POVendor, 50)
                        If validateDateWS(oOrder.POdate, dtVal) Then
                            .POdate = exportDateToString(dtVal.ToString)
                        End If
                        If validateDateWS(oOrder.POShipdate, dtVal) Then
                            .POShipdate = exportDateToString(dtVal.ToString)
                        End If
                        .POBuyer = Left(oOrder.POBuyer, 10)
                        .POFrt = oOrder.POFrt
                        .POTotalFrt = oOrder.POTotalFrt
                        .POTotalCost = oOrder.POTotalCost
                        .POWgt = oOrder.POWgt
                        .POCube = oOrder.POCube
                        .POQty = oOrder.POQty
                        .POPallets = oOrder.POPallets
                        .POLines = oOrder.POLines
                        .POConfirm = oOrder.POConfirm
                        .PODefaultCustomer = Left(oOrder.PODefaultCustomer, 50)
                        .PODefaultCarrier = oOrder.PODefaultCarrier
                        If validateDateWS(oOrder.POReqDate, dtVal) Then
                            .POReqDate = exportDateToString(dtVal.ToString)
                        End If
                        .POShipInstructions = Left(oOrder.POShipInstructions, 255)
                        .POCooler = oOrder.POCooler
                        .POFrozen = oOrder.POFrozen
                        .PODry = oOrder.PODry
                        .POTemp = Left(oOrder.POTemp, 1)
                        .POCarType = Left(oOrder.POCarType, 15)
                        .POShipVia = Left(oOrder.POShipVia, 10)
                        .POShipViaType = Left(oOrder.POShipViaType, 10)
                        .POConsigneeNumber = Left(oOrder.POConsigneeNumber, 50)
                        .POCustomerPO = Left(oOrder.POCustomerPO, 20)
                        .POOtherCosts = oOrder.POOtherCosts
                        .POStatusFlag = oOrder.POStatusFlag
                        .POOrderSequence = oOrder.POOrderSequence
                        .POChepGLID = Left(oOrder.POChepGLID, 50)
                        .POCarrierEquipmentCodes = Left(oOrder.POCarrierEquipmentCodes, 50)
                        .POCarrierTypeCode = Left(oOrder.POCarrierTypeCode, 50)
                        .POPalletPositions = Left(oOrder.POPalletPositions, 50)
                        If validateDateWS(oOrder.POSchedulePUDate, dtVal) Then
                            .POSchedulePUDate = exportDateToString(dtVal.ToString)
                        End If
                        If ValidateTimeWS(oOrder.POSchedulePUTime) Then
                            .POSchedulePUTime = oOrder.POSchedulePUTime
                        End If
                        If validateDateWS(oOrder.POScheduleDelDate, dtVal) Then
                            .POScheduleDelDate = exportDateToString(dtVal.ToString)
                        End If
                        If ValidateTimeWS(oOrder.POSCheduleDelTime) Then
                            .POSCHeduleDelTime = oOrder.POSCheduleDelTime
                        End If
                        If validateDateWS(oOrder.POActPUDate, dtVal) Then
                            .POActPUDate = exportDateToString(dtVal.ToString)
                        End If
                        If ValidateTimeWS(oOrder.POActPUTime) Then
                            .POActPUTime = oOrder.POActPUTime
                        End If
                        If validateDateWS(oOrder.POActDelDate, dtVal) Then
                            .POActDelDate = exportDateToString(dtVal.ToString)
                        End If
                        If ValidateTimeWS(oOrder.POActDelTime) Then
                            .POActDelTime = oOrder.POActDelTime
                        End If
                    End With
                    oHTable.AddBookHeaderRow(oRow)

                End If
            Next
            If Not oDetails Is Nothing Then
                For Each oDetail As clsBookDetailObject In oDetails
                    If Not oDetail Is Nothing Then


                        Dim oRow As BookData.BookDetailRow = oDTable.NewBookDetailRow
                        With oRow
                            .ItemPONumber = Left(oDetail.ItemPONumber, 20)
                            .FixOffInvAllow = oDetail.FixOffInvAllow
                            .FixFrtAllow = oDetail.FixFrtAllow
                            .ItemNumber = Left(oDetail.ItemNumber, 50)
                            .QtyOrdered = oDetail.QtyOrdered
                            .FreightCost = oDetail.FreightCost
                            .ItemCost = oDetail.ItemCost
                            .Weight = oDetail.Weight
                            .Cube = oDetail.Cube
                            .Pack = oDetail.Pack
                            .PalletType = Left(oDetail.PalletType, 50)
                            .Size = Left(oDetail.Size, 255)
                            .Description = Left(oDetail.Description, 255)
                            .Hazmat = Left(oDetail.Hazmat, 1)
                            .Brand = Left(oDetail.Brand, 255)
                            .CostCenter = Left(oDetail.CostCenter, 50)
                            .LotNumber = Left(oDetail.LotNumber, 50)
                            If validateDateWS(oDetail.LotExpirationDate, dtVal) Then
                                .LotExpirationDate = exportDateToString(dtVal.ToString)
                            End If
                            .GTIN = Left(oDetail.GTIN, 50)
                            .CustItemNumber = Left(oDetail.CustItemNumber, 50)
                            .CustomerNumber = Left(oDetail.CustomerNumber, 50)
                            .POOrderSequence = oDetail.POOrderSequence
                        End With
                        oDTable.AddBookDetailRow(oRow)
                    End If
                Next
            End If
            intRet = ProcessData(oHTable, oDTable, strConnection)
        Catch ex As Exception
            LogException("Process Object Data Failure", "Order import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessObjectData Failure")
        End Try
        Return intRet


    End Function


    Public Function ProcessObjectData(
                    ByVal oOrders As List(Of clsBookHeaderObject705),
                    ByVal oDetails As List(Of clsBookDetailObject705),
                    ByVal strConnection As String) As ProcessDataReturnValues
        Dim oOrders80 As New List(Of clsBookHeaderObject80)
        Dim oDetails80 As New List(Of clsBookDetailObject80)

        Dim strMsg As String = ""

        If Not oOrders Is Nothing AndAlso oOrders.Count() > 0 Then
            For Each o In oOrders
                Dim strSkip As New List(Of String) From {""}
                Dim newOrder = New clsBookHeaderObject80
                strMsg = ""
                CopyMatchingFields(newOrder, o, Nothing, strMsg)
                If Not String.IsNullOrWhiteSpace(strMsg) Then
                    If Debug Then Log(strMsg)
                    strMsg = ""
                End If
                oOrders80.Add(newOrder)
            Next
        Else
            Return ProcessDataReturnValues.nglDataIntegrationComplete
        End If

        If Not oDetails Is Nothing AndAlso oDetails.Count() > 0 Then
            For Each o In oDetails
                Dim strSkip As New List(Of String) From {""}
                Dim newDetail = New clsBookDetailObject80
                strMsg = ""
                CopyMatchingFields(newDetail, o, Nothing, strMsg)
                If Not String.IsNullOrWhiteSpace(strMsg) Then
                    If Debug Then Log(strMsg)
                    strMsg = ""
                End If
                oDetails80.Add(newDetail)
            Next
        End If

        Return ProcessObjectData(oOrders80, oDetails80, strConnection)



    End Function


    Public Function ProcessObjectData(
                    ByVal oOrders As List(Of clsBookHeaderObject80),
                    ByVal oDetails As List(Of clsBookDetailObject80),
                    ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsBook.ProcessObjectData"
        Dim strHeaderTable As String = "POHDR"
        Dim strItemTable As String = "POItem"
        Me.HeaderName = "PO Header"
        Me.ItemName = "PO Item"
        Me.ImportTypeKey = IntegrationTypes.Book
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Book Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        Try

            If Not Me.openConnection Then
                Return ProcessDataReturnValues.nglDataConnectionFailure
            End If
            'Modified by RHR  v-7.0.5.100 5/31/2016 
            'New variable for allow blank company on delete
            Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
            Dim strIgnoreValidationOnDelete = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'IgnoreValidationOnDelete'")
            Dim intVal As Integer = 0
            Integer.TryParse(strIgnoreValidationOnDelete, intVal)
            If intVal = 1 Then mBlnIgnoreValidationOnDelete = True

        Catch ex As Exception
            Return ProcessDataReturnValues.nglDataConnectionFailure
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Me.DALParameters.ConnectionString = strConnection

        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection80(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            'TODO: Add logic to test for null records and collections and generate warning message
            'if null records are found, but continue to process the data
            If oDetails.Where(Function(x) x Is Nothing).Any Then
                AddToGroupEmailMsg("One of the item detail records was null or empty and could not be processed.")
                oDetails = (From d In oDetails
                            Where
                            d IsNot Nothing
                            Select d).ToList
            End If


            'Import the Header Records
            If importHeaderRecords(oOrders, oDetails, oFields) Then
                'If any item details were missing we re-import all of them
                If mblnSomeItemsMissing AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                    'reset all counters
                    ItemErrors = 0
                    TotalItems = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    importItemRecords(oDetails)
                End If
                ProcessSilentTenders()
            End If
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Failure", "The following errors or warnings were reported some PO records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.TotalItems > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the PO Import Error Report for more information."
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the PO Item Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process PO Data Failure", "Could not process the requested PO data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessData")
        Finally
            'mblnSharedServiceRunning = False
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return intRet


    End Function


    Public Function ProcessObjectData(
                    ByVal oOrders As List(Of clsBookHeaderObject70),
                    ByVal oDetails As List(Of clsBookDetailObject70),
                    ByVal strConnection As String) As ProcessDataReturnValues

        Dim oOrders80 As New List(Of clsBookHeaderObject80)
        Dim oDetails80 As New List(Of clsBookDetailObject80)

        Dim strMsg As String = ""

        'add strmsg to email log messages
        If Not oOrders Is Nothing AndAlso oOrders.Count() > 0 Then
            For Each o In oOrders
                Dim strSkip As New List(Of String) From {""}
                Dim newOrder = New clsBookHeaderObject80
                strMsg = ""
                CopyMatchingFields(newOrder, o, Nothing, strMsg)
                If Not String.IsNullOrWhiteSpace(strMsg) Then
                    If Debug Then Log(strMsg)
                    strMsg = ""
                End If
                oOrders80.Add(newOrder)
            Next
        Else
            Return ProcessDataReturnValues.nglDataIntegrationComplete
        End If
        'add strmsg to email log messages
        If Not oDetails Is Nothing AndAlso oDetails.Count() > 0 Then
            For Each o In oDetails
                Dim strSkip As New List(Of String) From {""}
                Dim newDetail = New clsBookDetailObject80
                strMsg = ""
                CopyMatchingFields(newDetail, o, Nothing, strMsg)
                If Not String.IsNullOrWhiteSpace(strMsg) Then
                    If Debug Then Log(strMsg)
                    strMsg = ""
                End If
                oDetails80.Add(newDetail)
            Next
        End If

        Return ProcessObjectData(oOrders80, oDetails80, strConnection)



    End Function


    ''' <summary>
    ''' Process booking data with version 60 information.
    ''' </summary>
    ''' <param name="oOrders"></param>
    ''' <param name="oDetails"></param>
    ''' <param name="strConnection"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.4.4m on 04/20/2018
    '''     We now convert the 60 header to 604m header 
    '''     and call the new overloaded method.
    ''' </remarks>
    Public Function ProcessObjectData(
                   ByVal oOrders As List(Of clsBookHeaderObject60),
                   ByVal oDetails As List(Of clsBookDetailObject60),
                   ByVal strConnection As String) As ProcessDataReturnValues
        If oOrders Is Nothing OrElse oOrders.Count < 1 Then Return ProcessDataReturnValues.nglDataIntegrationComplete
        Dim oHeaders As New List(Of clsBookHeaderObject604m)
        For Each o In oOrders
            Dim oHeader As New clsBookHeaderObject604m
            With oHeader
                .POActDelDate = o.POActDelDate
                .POActDelTime = o.POActDelTime
                .POActPUDate = o.POActPUDate
                .POActPUTime = o.POActPUTime
                .POBuyer = o.POBuyer
                .POCarrierEquipmentCodes = o.POCarrierEquipmentCodes
                .POCarrierTypeCode = o.POCarrierTypeCode
                .POCarType = o.POCarType
                .POChepGLID = o.POChepGLID
                .POComments = o.POComments
                .POCommentsConfidential = o.POCommentsConfidential
                .POConfirm = o.POConfirm
                .POConsigneeNumber = o.POConsigneeNumber
                .POCooler = o.POCooler
                .POCube = o.POCube
                .POCustomerPO = o.POCustomerPO
                .POdate = o.POdate
                .PODefaultCarrier = o.PODefaultCarrier
                .PODefaultCustomer = o.PODefaultCustomer
                .PODefaultRouteSequence = o.PODefaultRouteSequence
                .PODestAddress1 = o.PODestAddress1
                .PODestAddress2 = o.PODestAddress2
                .PODestAddress3 = o.PODestAddress3
                .PODestCity = o.PODestCity
                .PODestCompNumber = o.PODestCompNumber
                .PODestContactFax = o.PODestContactFax
                .PODestContactPhone = o.PODestContactPhone
                .PODestContactPhoneExt = o.PODestContactPhoneExt
                .PODestCountry = o.PODestCountry
                .PODestName = o.PODestName
                .PODestState = o.PODestState
                .PODestZip = o.PODestZip
                .PODry = o.PODry
                .POFrozen = o.POFrozen
                .POFrt = o.POFrt
                .POInbound = o.POInbound
                .POLines = o.POLines
                .PONumber = o.PONumber
                .POOrderSequence = o.POOrderSequence
                .POOrigAddress1 = o.POOrigAddress1
                .POOrigAddress2 = o.POOrigAddress2
                .POOrigAddress3 = o.POOrigAddress3
                .POOrigCity = o.POOrigCity
                .POOrigCompNumber = o.POOrigCompNumber
                .POOrigContactFax = o.POOrigContactFax
                .POOrigContactPhone = o.POOrigContactPhone
                .POOrigContactPhoneExt = o.POOrigContactPhoneExt
                .POOrigCountry = o.POOrigCountry
                .POOrigName = o.POOrigName
                .POOrigState = o.POOrigState
                .POOrigZip = o.POOrigZip
                .POOtherCosts = o.POOtherCosts
                .POPalletExchange = o.POPalletExchange
                .POPalletPositions = o.POPalletPositions
                .POPallets = o.POPallets
                .POPalletType = o.POPalletType
                .POQty = o.POQty
                .POReqDate = o.POReqDate
                .PORouteGuideNumber = o.PORouteGuideNumber
                .POScheduleDelDate = o.POScheduleDelDate
                .POSCheduleDelTime = o.POActDelTime
                .POSchedulePUDate = o.POSchedulePUDate
                .POSchedulePUTime = o.POSchedulePUTime
                .POShipdate = o.POShipdate
                .POShipInstructions = o.POShipInstructions
                .POShipVia = o.POShipVia
                .POShipViaType = o.POShipViaType
                .POStatusFlag = o.POStatusFlag
                .POTemp = o.POTemp
                .POTotalCost = o.POTotalCost
                .POTotalFrt = o.POTotalFrt
                .POVendor = o.POVendor
                .POWgt = o.POWgt
                .POWhseAuthorizationNo = ""
            End With
            oHeaders.Add(oHeader)
        Next
        Return ProcessObjectData(oHeaders, oDetails, strConnection)
    End Function

    ''' <summary>
    ''' Process booking information using header 604m
    ''' </summary>
    ''' <param name="oOrders"></param>
    ''' <param name="oDetails"></param>
    ''' <param name="strConnection"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.4m on 04/20/2018
    '''     added POWhseAuthorizationNo for Mizkan 
    ''' </remarks>
    Public Function ProcessObjectData(
                    ByVal oOrders As List(Of clsBookHeaderObject604m),
                    ByVal oDetails As List(Of clsBookDetailObject60),
                    ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsBook.ProcessObjectData"
        Dim strHeaderTable As String = "POHDR"
        Dim strItemTable As String = "POItem"
        Me.HeaderName = "PO Header"
        Me.ItemName = "PO Item"
        Me.ImportTypeKey = IntegrationTypes.Book
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Book Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        Try

            If Not Me.openConnection Then
                Return ProcessDataReturnValues.nglDataConnectionFailure
            End If
        Catch ex As Exception
            Return ProcessDataReturnValues.nglDataConnectionFailure
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try

        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection60(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        Try

            'Import the Header Records
            If importHeaderRecords(oOrders, oDetails, oFields) Then
                'If any item details were missing we re-import all of them
                If mblnSomeItemsMissing AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                    'reset all counters
                    ItemErrors = 0
                    TotalItems = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    importItemRecords(oDetails)
                End If
                ProcessSilentTenders()
            End If
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Failure", "The following errors or warnings were reported some PO records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.TotalItems > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the PO Import Error Report for more information."
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the PO Item Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process PO Data Failure", "Could not process the requested PO data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessData")
        Finally
            'mblnSharedServiceRunning = False
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return intRet


    End Function

    ''' <summary>
    ''' v-6.0.4.7 version of the Process Object Data method used to support importing 
    ''' bookings from EDI 204 documents
    ''' </summary>
    ''' <param name="oOrders"></param>
    ''' <param name="oDetails"></param>
    ''' <param name="strConnection"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.7 on 5/22/2017
    '''   added to support EDI 204 inbound processing including automatic lane creation using POStatusFlag of 5 and 6
    ''' Modified by RHR On 6/1/2017 For v-7.0.6.105
    '''    added support for 604 objects
    ''' </remarks>
    Public Function ProcessObjectData(
                    ByVal oOrders As List(Of clsBookHeaderObject604),
                    ByVal oDetails As List(Of clsBookDetailObject604),
                    ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsBook.ProcessObjectData"
        Dim strHeaderTable As String = "POHDR"
        Dim strItemTable As String = "POItem"
        Me.HeaderName = "PO Header"
        Me.ItemName = "PO Item"
        Me.ImportTypeKey = IntegrationTypes.Book
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Book Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        Try

            If Not Me.openConnection Then
                Return ProcessDataReturnValues.nglDataConnectionFailure
            End If
        Catch ex As Exception
            Return ProcessDataReturnValues.nglDataConnectionFailure
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try

        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection604(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        Try

            'Import the Header Records
            If importHeaderRecords(oOrders, oDetails, oFields) Then
                'If any item details were missing we re-import all of them
                If mblnSomeItemsMissing AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                    'reset all counters
                    ItemErrors = 0
                    TotalItems = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    importItemRecords(oDetails)
                End If
                ProcessSilentTenders()
            End If
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Failure", "The following errors or warnings were reported some PO records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.TotalItems > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the PO Import Error Report for more information."
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the PO Item Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process PO Data Failure", "Could not process the requested PO data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessData")
        Finally
            'mblnSharedServiceRunning = False
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return intRet


    End Function

    Public Function ProcessData(
                ByVal oOrders As BookData.BookHeaderDataTable,
                ByVal oDetails As BookData.BookDetailDataTable,
                ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsBook.ProcessData"
        Dim strHeaderTable As String = "POHDR"
        Dim strItemTable As String = "POItem"
        Me.HeaderName = "PO Header"
        Me.ItemName = "PO Item"
        Me.ImportTypeKey = IntegrationTypes.Book
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Book Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        Try

            If Not Me.openConnection Then
                Return ProcessDataReturnValues.nglDataConnectionFailure
            End If
        Catch ex As Exception
            Return ProcessDataReturnValues.nglDataConnectionFailure
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try



        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            'Import the Header Records
            If importHeaderRecords(oOrders, oDetails, oFields) Then
                'If any item details were missing we re-import all of them
                If mblnSomeItemsMissing AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                    'reset all counters
                    ItemErrors = 0
                    TotalItems = 0
                    mintTotalQty = 0
                    mdblTotalWeight = 0
                    mdblHashTotalDetails = 0
                    importItemRecords(oDetails)
                End If
                ProcessSilentTenders()
            End If
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Failure", "The following errors or warnings were reported some PO records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.TotalItems > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the PO Import Error Report for more information."
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the PO Item Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process PO Data Failure", "Could not process the requested PO data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessData")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return intRet
    End Function

    Public Function ProcessAutoImportTest(ByVal OrderNumber As String,
                    ByVal strConnection As String,
                    ByRef strMsg As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete

        Dim strTitle As String = ""
        Dim strSource As String = "clsBook.ProcessObjectData"
        Dim strHeaderTable As String = "POHDR"
        Dim strItemTable As String = "POItem"
        Me.HeaderName = "PO Header"
        Me.ItemName = "PO Item"
        Me.ImportTypeKey = IntegrationTypes.Book
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Book Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        Try

            If Not Me.openConnection Then
                strMsg = "Cannot open DB Connection using: " & strConnection
                Return ProcessDataReturnValues.nglDataConnectionFailure
            End If
            'Modified by RHR  v-7.0.5.100 5/31/2016 
            'New variable for allow blank company on delete
            Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
            Dim strIgnoreValidationOnDelete = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'IgnoreValidationOnDelete'")
            Dim intVal As Integer = 0
            Integer.TryParse(strIgnoreValidationOnDelete, intVal)
            If intVal = 1 Then mBlnIgnoreValidationOnDelete = True

        Catch ex As Exception
            strMsg = "Unexpected Error: " & ex.ToString()
            Return ProcessDataReturnValues.nglDataConnectionFailure
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Me.DALParameters.ConnectionString = strConnection

        Try

            If mstrOrderNumbers Is Nothing Then mstrOrderNumbers = New List(Of String)
            mstrOrderNumbers.Add(OrderNumber)
            Me.silentImportProcessExecAsync()

        Catch ex As Exception
            strMsg = ("Process PO Data Failure: could not process the requested PO data. " & ex.ToString())
        Finally
            'mblnSharedServiceRunning = False
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return intRet


    End Function
    Public Sub ProcessSilentTenders()

        'If RunSilentTenderAsync Then

        '    Dim fetcher As New ProcessDataDelegate(AddressOf Me.silentImportProcessExecAsync)

        '    ' Launch thread
        '    fetcher.BeginInvoke(Nothing, Nothing)
        'Else
        Me.silentImportProcessExecAsync()
        'End If

    End Sub


    Public Function GetOrderPODownloadChanges(ByVal CompNumber As Integer,
                                    ByVal CompAlphaCode As String,
                                    ByVal CompLegalEntity As String,
                                    ByVal OrderNumber As String,
                                    ByVal OrderSequence As Integer,
                                    ByVal LaneNumber As String,
                                    ByVal POStatusFlag As Integer,
                                    ByVal TotalCases As Integer,
                                    ByVal TotalItems As Integer,
                                    ByVal TotalPL As Double,
                                    ByVal TotalWgt As Double,
                                    ByVal DateLoad As Date?,
                                    ByVal DateOrdered As Date?,
                                    ByVal DateRequired As Date?,
                                    ByVal TransType As String,
                                    ByVal CommCodeType As String,
                                    ByVal ModeType As Integer,
                                    Optional ByVal TestTransType As Boolean = 1,
                                    Optional ByVal TestModeType As Boolean = 1,
                                    Optional ByVal OrigAddress1 As String = "",
                                    Optional ByVal OrigCity As String = "",
                                    Optional ByVal OrigState As String = "",
                                    Optional ByVal OrigCountry As String = "",
                                    Optional ByVal OrigZip As String = "",
                                    Optional ByVal DestAddress1 As String = "",
                                    Optional ByVal DestCity As String = "",
                                    Optional ByVal DestState As String = "",
                                    Optional ByVal DestCountry As String = "",
                                    Optional ByVal DestZip As String = "",
                                    Optional ByVal User1 As String = "",
                                    Optional ByVal User2 As String = "",
                                    Optional ByVal User3 As String = "",
                                    Optional ByVal User4 As String = "") As Ngl.FreightMaster.Data.LTS.spGetOrderPODownloadChangesResult
        Dim oReturn As New Ngl.FreightMaster.Data.LTS.spGetOrderPODownloadChangesResult

        Try
            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = "System Download"
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = Me.ConnectionString
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With

            Dim oBookData = New DAL.NGLBookData(oWCFParameters)
            oReturn = oBookData.GetOrderPODownloadChanges(CompNumber, CompAlphaCode, CompLegalEntity, OrderNumber, OrderSequence, LaneNumber, POStatusFlag, TotalCases, TotalItems, TotalPL, TotalWgt, DateLoad, DateOrdered, DateRequired, TransType, CommCodeType, ModeType, TestTransType, TestModeType, OrigAddress1, OrigCity, OrigState, OrigCountry, OrigZip, DestAddress1, DestCity, DestState, DestCountry, DestZip, User1, User2, User3, User4)

        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            'add new error handlers for sql fault exceptions
            AddToGroupEmailMsg("cannot read existing POHDR data: " & ex.Detail.ToString(ex.Reason.ToString()))

        Catch ex As Exception
            'add new error handlers for sql fault exceptions
            AddToGroupEmailMsg("cannot read existing POHDR data: " & ex.Message())
        End Try

        Return oReturn

    End Function

    Public Function GetOrderChanges(ByVal CompNumber As Integer,
                                    ByVal CompAlphaCode As String,
                                    ByVal CompLegalEntity As String,
                                    ByVal OrderNumber As String,
                                    ByVal OrderSequence As Integer,
                                    ByVal LaneNumber As String,
                                    ByVal POStatusFlag As Integer,
                                    ByVal TotalCases As Integer,
                                    ByVal TotalItems As Integer,
                                    ByVal TotalPL As Double,
                                    ByVal TotalWgt As Double,
                                    ByVal DateLoad As Date?,
                                    ByVal DateOrdered As Date?,
                                    ByVal DateRequired As Date?,
                                    ByVal TransType As String,
                                    ByVal CommCodeType As String,
                                    ByVal ModeType As Integer,
                                    Optional ByVal TestTransType As Boolean = 1,
                                    Optional ByVal TestModeType As Boolean = 1,
                                    Optional ByVal OrigAddress1 As String = "",
                                    Optional ByVal OrigCity As String = "",
                                    Optional ByVal OrigState As String = "",
                                    Optional ByVal OrigCountry As String = "",
                                    Optional ByVal OrigZip As String = "",
                                    Optional ByVal DestAddress1 As String = "",
                                    Optional ByVal DestCity As String = "",
                                    Optional ByVal DestState As String = "",
                                    Optional ByVal DestCountry As String = "",
                                    Optional ByVal DestZip As String = "",
                                    Optional ByVal User1 As String = "",
                                    Optional ByVal User2 As String = "",
                                    Optional ByVal User3 As String = "",
                                    Optional ByVal User4 As String = "") As Ngl.FreightMaster.Data.LTS.spGetOrderChangesResult
        Dim oReturn As New Ngl.FreightMaster.Data.LTS.spGetOrderChangesResult

        Try
            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = "System Download"
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = Me.ConnectionString
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With

            Dim oBookData = New DAL.NGLBookData(oWCFParameters)
            oReturn = oBookData.GetOrderChanges(CompNumber, CompAlphaCode, CompLegalEntity, OrderNumber, OrderSequence, LaneNumber, POStatusFlag, TotalCases, TotalItems, TotalPL, TotalWgt, DateLoad, DateOrdered, DateRequired, TransType, CommCodeType, ModeType, TestTransType, TestModeType, OrigAddress1, OrigCity, OrigState, OrigCountry, OrigZip, DestAddress1, DestCity, DestState, DestCountry, DestZip, User1, User2, User3, User4)

        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            'add new error handlers for sql fault exceptions
            AddToGroupEmailMsg("cannot read existing order data: " & ex.Detail.ToString(ex.Reason.ToString()))

        Catch ex As Exception
            'add new error handlers for sql fault exceptions
            AddToGroupEmailMsg("cannot read existing order data: " & ex.Message())
        End Try

        Return oReturn

    End Function

    ''' <summary>
    ''' Returns true if updates or inserts records are allowed or needed
    ''' Returns false if updates or inserts are not allowed or needed
    ''' </summary>
    ''' <param name="CompNumber"></param>
    ''' <param name="CompAlphaCode"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="OrderSequence"></param>
    ''' <param name="LaneNumber"></param>
    ''' <param name="POStatusFlag"></param>
    ''' <param name="TotalCases"></param>
    ''' <param name="TotalItems"></param>
    ''' <param name="TotalPL"></param>
    ''' <param name="TotalWgt"></param>
    ''' <param name="DateLoad"></param>
    ''' <param name="DateOrdered"></param>
    ''' <param name="DateRequired"></param>
    ''' <param name="TransType"></param>
    ''' <param name="CommCodeType"></param>
    ''' <param name="ModeType"></param>
    ''' <param name="strChanges"></param>
    ''' <param name="TestTransType"></param>
    ''' <param name="TestModeType"></param>
    ''' <param name="OrigAddress1"></param>
    ''' <param name="OrigCity"></param>
    ''' <param name="OrigState"></param>
    ''' <param name="OrigCountry"></param>
    ''' <param name="OrigZip"></param>
    ''' <param name="DestAddress1"></param>
    ''' <param name="DestCity"></param>
    ''' <param name="DestState"></param>
    ''' <param name="DestCountry"></param>
    ''' <param name="DestZip"></param>
    ''' <param name="User1"></param>
    ''' <param name="User2"></param>
    ''' <param name="User3"></param>
    ''' <param name="User4"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.102 on 11/16/2016
    '''   new business rules applied
    '''   (1) if no records exist in POHDR table Modified = false and we always check Book table
    '''   (2) if a record does exist in POHDR table and it has been modified we return true 
    '''       we do not check the book table we always update the POHDR table on changes
    '''       we do not check the new GlobalGPImportNewOrdersOnly when updating the POHDR table
    '''   (3) if modified = false we check the Book table anyway and let book decide if an update is needed
    '''       we always override the POHDR table if Book returns Modified = true
    '''   The following are only run when 1,2, or 3 set Modified = False
    '''   (4) Check the book table for existing record, if no records Modified = true
    '''   (5) If Book reocrds exist but GlobalGPImportNewOrdersOnly = 1 Modified = false
    '''       updates are not allowed
    '''   (6) If GlobalGPImportNewOrdersOnly not  equal 1 we check for changes to the Book data
    '''       setting Modified to true if the data has changed
    ''' Modified by RHR for v-7.0.6.105 on 12/6/2017
    '''     New Rule (3)
    '''     We no longer perform rule 3 just because oPOChanges.OrderModified is false
    '''     now we compare the Lane Numbers.  if the Lane Numbers match then we have a matching 
    '''     record and update are not required.  Note: oPOChanges.POHdrLaneNumber will equal @@EMPTY@@
    '''     if no matching records are found in the pohdr table.
    '''     This change will prevent duplicate updates to the POHDRHistory table when order sit in the 
    '''     pohdr table for an extended period of time.
    ''' </remarks>
    Public Function HasOrderChanged(ByVal CompNumber As Integer,
                                    ByVal CompAlphaCode As String,
                                    ByVal CompLegalEntity As String,
                                    ByVal OrderNumber As String,
                                    ByVal OrderSequence As Integer,
                                    ByVal LaneNumber As String,
                                    ByVal POStatusFlag As Integer,
                                    ByVal TotalCases As Integer,
                                    ByVal TotalItems As Integer,
                                    ByVal TotalPL As Double,
                                    ByVal TotalWgt As Double,
                                    ByVal DateLoad As Date?,
                                    ByVal DateOrdered As Date?,
                                    ByVal DateRequired As Date?,
                                    ByVal TransType As String,
                                    ByVal CommCodeType As String,
                                    ByVal ModeType As Integer,
                                    ByRef strChanges As String,
                                    Optional ByVal TestTransType As Boolean = 1,
                                    Optional ByVal TestModeType As Boolean = 1,
                                    Optional ByVal OrigAddress1 As String = "",
                                    Optional ByVal OrigCity As String = "",
                                    Optional ByVal OrigState As String = "",
                                    Optional ByVal OrigCountry As String = "",
                                    Optional ByVal OrigZip As String = "",
                                    Optional ByVal DestAddress1 As String = "",
                                    Optional ByVal DestCity As String = "",
                                    Optional ByVal DestState As String = "",
                                    Optional ByVal DestCountry As String = "",
                                    Optional ByVal DestZip As String = "",
                                    Optional ByVal User1 As String = "",
                                    Optional ByVal User2 As String = "",
                                    Optional ByVal User3 As String = "",
                                    Optional ByVal User4 As String = "") As Boolean
        Dim blnRet As Boolean = False

        strChanges = ""
        'first we check for POHDR changes the main difference is that if we do not have any pohdr records  we do not mark the record as changed until we test the book table
        Dim oPOChanges As Ngl.FreightMaster.Data.LTS.spGetOrderPODownloadChangesResult = Me.GetOrderPODownloadChanges(CompNumber, CompAlphaCode, CompLegalEntity, OrderNumber, OrderSequence, LaneNumber, POStatusFlag, TotalCases, TotalItems, TotalPL, TotalWgt, DateLoad, DateOrdered, DateRequired, TransType, CommCodeType, ModeType, TestTransType, TestModeType, OrigAddress1, OrigCity, OrigState, OrigCountry, OrigZip, DestAddress1, DestCity, DestState, DestCountry, DestZip, User1, User2, User3, User4)
        If Not oPOChanges Is Nothing Then
            'Rule (1) or (3)
            If oPOChanges.OrderModified Then
                With oPOChanges
                    If .POHdrLaneNumberChanged Then strChanges &= "Lane Number Changed From " & .POHdrLaneNumber & " to " & LaneNumber & vbCrLf
                    If .POHdrTotalCasesChanged Then strChanges &= "Total Cases Changed From " & .POHdrTotalCases.ToString() & " to " & TotalCases.ToString() & vbCrLf
                    If .POHdrTotalItemsChanged Then strChanges &= "Total Items Changed From " & .POHdrTotalItems.ToString() & " to " & TotalItems.ToString() & vbCrLf
                    If .POHdrTotalPLChanged Then strChanges &= "Total Pallets Changed From " & .POHdrTotalPL.ToString() & " to " & TotalPL.ToString() & vbCrLf
                    If .POHdrTotalWgtChanged Then strChanges &= "Total Weight Changed From " & .POHdrTotalWgt.ToString() & " to " & TotalWgt.ToString() & vbCrLf
                    If .POHdrDateLoadChanged Then strChanges &= "Load/Ship Date Changed From " & If(.POHdrDateLoad.HasValue(), .POHdrDateLoad.Value.ToString("g"), " NULL ") & " to " & If(DateLoad.HasValue, DateLoad.Value.ToString("g"), " NULL ") & vbCrLf
                    If .POHdrDateOrderedChanged Then strChanges &= "Order Date Changed From " & If(.POHdrDateOrdered.HasValue(), .POHdrDateOrdered.Value.ToString("g"), " NULL ") & " to " & If(DateOrdered.HasValue, DateOrdered.Value.ToString("g"), " NULL ") & vbCrLf
                    If .POHdrDateRequiredChanged Then strChanges &= "Required Date Changed From " & If(.POHdrDateRequired.HasValue(), .POHdrDateRequired.Value.ToString("g"), " NULL ") & " to " & If(DateRequired.HasValue, DateRequired.Value.ToString("g"), " NULL ") & vbCrLf
                    If .POHdrTransTypeChanged Then strChanges &= "Trans Type/POFrt Changed From " & .POHdrTransType & " to " & TransType & vbCrLf
                    If .POHdrLoadComChanged Then strChanges &= "Temperature Type Changed From " & .POHdrLoadCom & " to " & CommCodeType & vbCrLf
                    If .POHdrModeTypeControlChanged Then strChanges &= "Mode Type Changed From " & .POHdrModeTypeControl.ToString() & " to " & ModeType.ToString() & vbCrLf
                    If .POHdrOrigAddress1Changed Then strChanges &= "Orig Addresss Changed From " & .POHdrOrigAddress1 & " to " & OrigAddress1 & vbCrLf
                    If .POHdrOrigCityChanged Then strChanges &= "Orig City Changed From " & .POHdrOrigCity & " to " & OrigCity & vbCrLf
                    If .POHdrOrigStateChanged Then strChanges &= "Orig State Changed From " & .POHdrOrigState & " to " & OrigState & vbCrLf
                    If .POHdrOrigCountryChanged Then strChanges &= "Orig Country Changed From " & .POHdrOrigCountry & " to " & OrigCountry & vbCrLf
                    If .POHdrOrigZipChanged Then strChanges &= "Orig Zip Changed From " & .POHdrOrigZip & " to " & OrigZip & vbCrLf
                    If .POHdrDestAddress1Changed Then strChanges &= "Dest Addresss Changed From " & .POHdrDestAddress1 & " to " & DestAddress1 & vbCrLf
                    If .POHdrDestCityChanged Then strChanges &= "Dest City Changed From " & .POHdrDestCity & " to " & DestCity & vbCrLf
                    If .POHdrDestStateChanged Then strChanges &= "Dest State Changed From " & .POHdrDestState & " to " & DestState & vbCrLf
                    If .POHdrDestCountryChanged Then strChanges &= "Dest Country Changed From " & .POHdrDestCountry & " to " & DestCountry & vbCrLf
                    If .POHdrDestZipChanged Then strChanges &= "Dest Zip Changed From " & .POHdrDestZip & " to " & DestZip & vbCrLf
                    If .POHdrUser1Changed Then strChanges &= "User 1 From " & .POHdrUser1 & " to " & User1 & vbCrLf
                    If .POHdrUser2Changed Then strChanges &= "User 2 From " & .POHdrUser2 & " to " & User2 & vbCrLf
                    If .POHdrUser3Changed Then strChanges &= "User 3 From " & .POHdrUser3 & " to " & User3 & vbCrLf
                    If .POHdrUser4Changed Then strChanges &= "User 4 From " & .POHdrUser4 & " to " & User4 & vbCrLf
                End With
                'Modified by RHR for v-7.0.5.102 on 11/16/2016 rule (2)
                Return True
            ElseIf oPOChanges.POHdrLaneNumber = LaneNumber Then
                'Modified by RHR for v-7.0.6.105 on 12/6/2017 new rule (3)
                'no changes updates are not required
                Return False
            End If
        End If
        'if we get here we have no POHDR records so check the book table
        'Rule (1) or (3)
        Dim oChanges As Ngl.FreightMaster.Data.LTS.spGetOrderChangesResult = Me.GetOrderChanges(CompNumber, CompAlphaCode, CompLegalEntity, OrderNumber, OrderSequence, LaneNumber, POStatusFlag, TotalCases, TotalItems, TotalPL, TotalWgt, DateLoad, DateOrdered, DateRequired, TransType, CommCodeType, ModeType, TestTransType, TestModeType, OrigAddress1, OrigCity, OrigState, OrigCountry, OrigZip, DestAddress1, DestCity, DestState, DestCountry, DestZip, User1, User2, User3, User4)

        If oChanges Is Nothing Then
            'Modified by RHR for v-7.0.5.102 on 11/16/2016
            ' test for errors but this should not happen based on Rules 4,5, anbd 6
            blnRet = True
        Else
            blnRet = If(oChanges.OrderModified, False)
        End If

        If blnRet = True Then
            With oChanges
                If .BookLaneNumberChanged Then strChanges &= "Lane Number Changed From " & .BookLaneNumber & " to " & LaneNumber & vbCrLf
                If .BookTotalCasesChanged Then strChanges &= "Total Cases Changed From " & .BookTotalCases.ToString() & " to " & TotalCases.ToString() & vbCrLf
                If .BookTotalItemsChanged Then strChanges &= "Total Items Changed From " & .BookTotalItems.ToString() & " to " & TotalItems.ToString() & vbCrLf
                If .BookTotalPLChanged Then strChanges &= "Total Pallets Changed From " & .BookTotalPL.ToString() & " to " & TotalPL.ToString() & vbCrLf
                If .BookTotalWgtChanged Then strChanges &= "Total Weight Changed From " & .BookTotalWgt.ToString() & " to " & TotalWgt.ToString() & vbCrLf
                If .BookDateLoadChanged Then strChanges &= "Load/Ship Date Changed From " & If(.BookDateLoad.HasValue(), .BookDateLoad.Value.ToString("g"), " NULL ") & " to " & If(DateLoad.HasValue, DateLoad.Value.ToString("g"), " NULL ") & vbCrLf
                If .BookDateOrderedChanged Then strChanges &= "Order Date Changed From " & If(.BookDateOrdered.HasValue(), .BookDateOrdered.Value.ToString("g"), " NULL ") & " to " & If(DateOrdered.HasValue, DateOrdered.Value.ToString("g"), " NULL ") & vbCrLf
                If .BookDateRequiredChanged Then strChanges &= "Required Date Changed From " & If(.BookDateRequired.HasValue(), .BookDateRequired.Value.ToString("g"), " NULL ") & " to " & If(DateRequired.HasValue, DateRequired.Value.ToString("g"), " NULL ") & vbCrLf
                If .BookTransTypeChanged Then strChanges &= "Trans Type/POFrt Changed From " & .BookTransType & " to " & TransType & vbCrLf
                If .BookLoadComChanged Then strChanges &= "Temperature Type Changed From " & .BookLoadCom & " to " & CommCodeType & vbCrLf
                If .BookModeTypeControlChanged Then strChanges &= "Mode Type Changed From " & .BookModeTypeControl.ToString() & " to " & ModeType.ToString() & vbCrLf
                If .BookOrigAddress1Changed Then strChanges &= "Orig Addresss Changed From " & .BookOrigAddress1 & " to " & OrigAddress1 & vbCrLf
                If .BookOrigCityChanged Then strChanges &= "Orig City Changed From " & .BookOrigCity & " to " & OrigCity & vbCrLf
                If .BookOrigStateChanged Then strChanges &= "Orig State Changed From " & .BookOrigState & " to " & OrigState & vbCrLf
                If .BookOrigCountryChanged Then strChanges &= "Orig Country Changed From " & .BookOrigCountry & " to " & OrigCountry & vbCrLf
                If .BookOrigZipChanged Then strChanges &= "Orig Zip Changed From " & .BookOrigZip & " to " & OrigZip & vbCrLf
                If .BookDestAddress1Changed Then strChanges &= "Dest Addresss Changed From " & .BookDestAddress1 & " to " & DestAddress1 & vbCrLf
                If .BookDestCityChanged Then strChanges &= "Dest City Changed From " & .BookDestCity & " to " & DestCity & vbCrLf
                If .BookDestStateChanged Then strChanges &= "Dest State Changed From " & .BookDestState & " to " & DestState & vbCrLf
                If .BookDestCountryChanged Then strChanges &= "Dest Country Changed From " & .BookDestCountry & " to " & DestCountry & vbCrLf
                If .BookDestZipChanged Then strChanges &= "Dest Zip Changed From " & .BookDestZip & " to " & DestZip & vbCrLf
                If .BookUser1Changed Then strChanges &= "User 1 From " & .BookUser1 & " to " & User1 & vbCrLf
                If .BookUser2Changed Then strChanges &= "User 2 From " & .BookUser2 & " to " & User2 & vbCrLf
                If .BookUser3Changed Then strChanges &= "User 3 From " & .BookUser3 & " to " & User3 & vbCrLf
                If .BookUser4Changed Then strChanges &= "User 4 From " & .BookUser4 & " to " & User4 & vbCrLf
            End With
        End If

        Return blnRet
    End Function

    ''' <summary>
    ''' Returns true if updates or inserts records are allowed or needed
    ''' Returns false if updates or inserts are not allowed or needed
    ''' </summary>
    ''' <param name="oHeader"></param>
    ''' <param name="TotalItems"></param>
    ''' <param name="strChanges"></param>
    ''' <param name="TestTransType"></param>
    ''' <param name="TestModeType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HasOrderChanged(ByVal oHeader As clsBookHeaderObject705,
                                  ByVal TotalItems As Integer,
                                  ByRef strChanges As String,
                                  Optional ByVal TestTransType As Boolean = 1,
                                  Optional ByVal TestModeType As Boolean = 1) As Boolean

        Dim CompNumber As Integer = 0
        Integer.TryParse(oHeader.PODefaultCustomer, CompNumber)
        Return Me.HasOrderChanged(
            CompNumber,
            oHeader.POCompAlphaCode,
            oHeader.POCompLegalEntity,
            oHeader.PONumber,
            oHeader.POOrderSequence,
            oHeader.POVendor,
            oHeader.POStatusFlag,
            oHeader.POQty,
            TotalItems,
            oHeader.POPallets,
            oHeader.POWgt,
            oHeader.POShipdate,
            oHeader.POdate,
            oHeader.POReqDate,
            oHeader.POFrt,
            oHeader.POTemp,
            oHeader.POModeTypeControl,
            strChanges,
            TestTransType,
            TestModeType,
            oHeader.POOrigAddress1,
            oHeader.POOrigCity,
            oHeader.POOrigState,
            oHeader.POOrigCountry,
            oHeader.POOrigZip,
            oHeader.PODestAddress1,
            oHeader.PODestCity,
            oHeader.PODestState,
            oHeader.PODestCountry,
            oHeader.PODestZip,
            oHeader.POUser1,
            oHeader.POUser2,
            oHeader.POUser3,
            oHeader.POUser4)

    End Function

    Public Function HasOrderChanged(ByVal oHeader As clsBookHeaderObject80,
                                  ByVal TotalItems As Integer,
                                  ByRef strChanges As String,
                                  Optional ByVal TestTransType As Boolean = 1,
                                  Optional ByVal TestModeType As Boolean = 1) As Boolean

        Dim CompNumber As Integer = 0
        Integer.TryParse(oHeader.PODefaultCustomer, CompNumber)
        Return Me.HasOrderChanged(
            CompNumber,
            oHeader.POCompAlphaCode,
            oHeader.POCompLegalEntity,
            oHeader.PONumber,
            oHeader.POOrderSequence,
            oHeader.POVendor,
            oHeader.POStatusFlag,
            oHeader.POQty,
            TotalItems,
            oHeader.POPallets,
            oHeader.POWgt,
            oHeader.POShipdate,
            oHeader.POdate,
            oHeader.POReqDate,
            oHeader.POFrt,
            oHeader.POTemp,
            oHeader.POModeTypeControl,
            strChanges,
            TestTransType,
            TestModeType,
            oHeader.POOrigAddress1,
            oHeader.POOrigCity,
            oHeader.POOrigState,
            oHeader.POOrigCountry,
            oHeader.POOrigZip,
            oHeader.PODestAddress1,
            oHeader.PODestCity,
            oHeader.PODestState,
            oHeader.PODestCountry,
            oHeader.PODestZip,
            oHeader.POUser1,
            oHeader.POUser2,
            oHeader.POUser3,
            oHeader.POUser4)

    End Function


    Public Function GetCostPerPoundForOrder(ByVal BookCarrOrderNumber As String, ByVal BookOrderSequence As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String, Optional ByVal CompNumber As Integer = 0) As Double
        Dim oReturn As Double = 0

        Try
            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = "System Download"
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = Me.ConnectionString
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With

            Dim oBookData = New DAL.NGLBookData(oWCFParameters)
            oReturn = oBookData.GetCostPerPoundForOrder(BookCarrOrderNumber, BookOrderSequence, CompAlphaCode, CompLegalEntity, CompNumber)

        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            'add new error handlers for sql fault exceptions
            AddToGroupEmailMsg("cannot get the cost per pound for order: " & ex.Detail.ToString(ex.Reason.ToString()))

        Catch ex As Exception
            'add new error handlers for sql fault exceptions
            AddToGroupEmailMsg("cannot get the cost per pound for order: " & ex.Message())
        End Try

        Return oReturn
    End Function


    ''' <summary>
    ''' Deprecated: for backward compatibility only; all new calls should use ProcessSilentTenders directly
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SilentTenderAsync()
        ProcessSilentTenders()
    End Sub

#Region "TMS365 Methods"

    ''' <summary>
    ''' Cannot run due to changes in Dispatch Model
    ''' </summary>
    ''' <param name="info"></param>
    ''' <returns></returns>
    Public Function CreateOrderFromRateShop(ByVal info As DAL.Models.Dispatch) As Boolean
        'All Code Removed due to changes in Dispatch Model by RHR on 01/16/2018
        Return False

        'Dim oBookHeaders As New List(Of clsBookHeaderObject705)
        'Dim oBookDetails As New List(Of clsBookDetailObject705)
        'Dim oBookHeader As New clsBookHeaderObject705

        'Dim oWCFParameters = WCFDataProperties.ConvertToWCFProperties(New DAL.WCFParameters())
        'Dim oCompWCFData = New DAL.NGLCompData(oWCFParameters)

        'Dim ProAbb = oCompWCFData.GetCompAbrevByUserOrLE(info.UserLEControl, info.UserControl, "")

        ''LaneValue = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, sErrors)
        ''ReturnValue = ProAbb & "-" & Trim(TblRec("CUSTNMBR").ToString) & "-" & Trim(TblRec("PRSTADCD").ToString)


        
#Disable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
''' Insert TMSHeader
        'oBookHeader.PONumber = info.OrderNumber.Trim()
        'oBookHeader.POdate = Date.Now.ToString()
        'oBookHeader.POShipdate = info.LoadDate.ToString()
        ''oBookHeader.POVendor = GPFunctions.CalculateLane(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString, ProAbb, sErrors)
        'oBookHeader.POBuyer = ""
        
#Disable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
'''oBookHeader.POFrt = HeaderRec("").ToString
        ''' Work with Rob to determine
        ''oBookHeader.POFrt = GPFunctions.GetShippingMethod(EconnectStr, 1, HeaderRec("SOPNumbe").ToString, 16384, HeaderRec("SOPNumbe").ToString)

        ''Double.TryParse(HeaderRec("FRTAMNT").ToString(), oBookHeader.POTotalFrt)
        ''Double.TryParse(HeaderRec("EXTDCOST").ToString, oBookHeader.POTotalCost)
        
#Disable Warning BC42304 ' XML documentation parse error: XML entity references are not supported. XML comment will be ignored.
#Disable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
'''If Me.Debug Then Log("Getting Order Weight For: " & HeaderRec("SOPNUMBE").ToString())
        'oBookHeader.POWgt = info.TotalWgt
        ''oBookHeader.POCube = 0
        'oBookHeader.POQty = info.TotalQty
        'oBookHeader.POPallets = info.TotalPlts
        'oBookHeader.POLines = 0
        'oBookHeader.POConfirm = False
        ''oBookHeader.PODefaultCustomer = "0"
        '' Talk to Rob on this field below
        'oBookHeader.PODefaultCarrier = 0
        'oBookHeader.POReqDate = info.RequiredDate.ToString()
        
#Disable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
''' Write funtion to get shipping instructions
        ''oBookHeader.POShipInstructions = ""
        ''oBookHeader.POCooler = False
        ''oBookHeader.POFrozen = False
        ''oBookHeader.PODry = False
        ''oBookHeader.POTemp = GPFunctions.GetTemp(EconnectStr, 1, HeaderRec("SOPNUMBE").ToString)
        'oBookHeader.POCarType = ""
        'oBookHeader.POShipVia = ""
        'oBookHeader.POShipViaType = ""
        'oBookHeader.POConsigneeNumber = ""
        ''oBookHeader.POCustomerPO = Trim(HeaderRec("CSTPONBR").ToString())
        'oBookHeader.POOtherCosts = 0
        'oBookHeader.POStatusFlag = 5
        'oBookHeader.POOrderSequence = 0
        'oBookHeader.POChepGLID = ""
        'oBookHeader.POCarrierEquipmentCodes = ""
        'oBookHeader.POCarrierTypeCode = ""
        'oBookHeader.POPalletPositions = ""
        'oBookHeader.POSchedulePUDate = ""
        'oBookHeader.POSchedulePUTime = ""
        'oBookHeader.POScheduleDelDate = ""
        'oBookHeader.POSCheduleDelTime = ""
        'oBookHeader.POActPUDate = ""
        'oBookHeader.POActPUTime = ""
        'oBookHeader.POActDelDate = ""
        'oBookHeader.POActDelTime = ""
        'oBookHeader.POOrigCompNumber = ""
        'oBookHeader.POOrigName = info.Origin.Name.Trim()
        ''    oBookHeader.POOrigCompAlphaCode = Trim(PorDAddressInfo.AddressID)
        'oBookHeader.POOrigAddress1 = info.Origin.Address1.Trim()
        'oBookHeader.POOrigAddress2 = info.Origin.Address2.Trim()
        'oBookHeader.POOrigAddress3 = info.Origin.Address3.Trim()
        'oBookHeader.POOrigCity = info.Origin.City.Trim()
        'oBookHeader.POOrigState = info.Origin.State.Trim()
        'oBookHeader.POOrigCountry = info.Origin.Country.Trim()
        'oBookHeader.POOrigZip = info.Origin.Zip.Trim()
        'oBookHeader.POOrigContactPhone = info.OrigContact.ContactPhone.Trim()
        'oBookHeader.POOrigContactPhoneExt = info.OrigContact.ContactPhoneExt.Trim()
        'oBookHeader.POOrigContactFax = info.OrigContact.ContactFax.Trim()
        'oBookHeader.PODestCompNumber = ""
        ''oBookHeader.PODestCompAlphaCode = Trim(HeaderRec("PRSTADCD").ToString())
        'oBookHeader.PODestName = info.Destination.Name.Trim()
        'oBookHeader.PODestAddress1 = info.Destination.Address1.Trim()
        'oBookHeader.PODestAddress2 = info.Destination.Address2.Trim()
        'oBookHeader.PODestAddress3 = info.Destination.Address3.Trim()
        'oBookHeader.PODestCity = info.Destination.City.Trim()
        'oBookHeader.PODestState = info.Destination.State.Trim()
        'oBookHeader.PODestCountry = info.Destination.Country.Trim()
        'oBookHeader.PODestZip = info.Destination.Zip.Trim()
        'oBookHeader.PODestContactPhone = info.DestContact.ContactPhone.Trim()
        'oBookHeader.PODestContactPhoneExt = info.DestContact.ContactPhoneExt.Trim()
        'oBookHeader.PODestContactFax = info.DestContact.ContactFax.Trim()
        ''oBookHeader.POInbound = False
        'oBookHeader.POPalletExchange = False
        ''oBookHeader.POPalletType = ""
        'oBookHeader.POComments = ""
        'oBookHeader.POCommentsConfidential = ""
        ''oBookHeader.PODefaultRouteSequence = 0
        'oBookHeader.PORouteGuideNumber = ""
        ''oBookHeader.POCompLegalEntity = Trim(TMSSetting.LegalEntity.ToString())
        ''oBookHeader.POCompAlphaCode = Trim(HeaderRec("LOCNCODE"))
        ''oBookHeader.POModeTypeControl = GPFunctions.GetTransporationMode(HeaderRec("SOPNUMBE").ToString, "SOP")
        'oBookHeader.POUser1 = ""
        'oBookHeader.POUser2 = ""
        'oBookHeader.POUser3 = ""
        'oBookHeader.POUser4 = ""
        'oBookHeader.POAPGLNumber = ""


        'Dim oBookDetail As New clsBookDetailObject705

        'oBookDetail.ItemPONumber = info.OrderNumber.Trim()
        'oBookDetail.ItemNumber = "info.Item.ItemNumber"
        ''    If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
        'oBookDetail.QtyOrdered = "info.Item.ItemQty"
        'oBookDetail.Weight = "info.Item.ItemWgt"
        ''    If (LineCols.Contains("FixOffInvAllow")) Then Double.TryParse(LineRec("FixOffInvAllow").ToString(), oBookDetail.FixOffInvAllow)
        ''    If (LineCols.Contains("FixFrtAllow")) Then Double.TryParse(LineRec("FixFrtAllow").ToString(), oBookDetail.FixFrtAllow)
        ''    If (LineCols.Contains("FreightCost")) Then Double.TryParse(LineRec("FreightCost").ToString(), oBookDetail.FreightCost)
        ''    If (LineCols.Contains("ItemCost")) Then Double.TryParse(LineRec("ItemCost").ToString(), oBookDetail.ItemCost)
        'oBookDetail.Cube = "info.Item.ItemCube"
        ''    If (LineCols.Contains("Pack")) Then GPFunctions.TryParseInt(LineRec("Pack").ToString(), oBookDetail.Pack)
        ''    If (LineCols.Contains("Size")) Then oBookDetail.Size = Trim(LineRec("Size").ToString())
        'oBookDetail.Description = "info.Item.ItemDesc"
        ''    If (LineCols.Contains("Hazmat")) Then oBookDetail.Hazmat = Trim(LineRec("Hazmat").ToString())
        ''    If (LineCols.Contains("Brand")) Then oBookDetail.Brand = Trim(LineRec("Brand").ToString())
        ''    If (LineCols.Contains("CostCenter")) Then oBookDetail.CostCenter = Trim(LineRec("CostCenter").ToString())
        ''    If (LineCols.Contains("LotNumber")) Then oBookDetail.LotNumber = Trim(LineRec("LotNumber").ToString())
        ''    If (LineCols.Contains("LotExpirationDate")) Then oBookDetail.LotExpirationDate = Trim(LineRec("LotExpirationDate").ToString())
        ''    If (LineCols.Contains("GTIN")) Then oBookDetail.GTIN = Trim(LineRec("GTIN").ToString())
        ''    If (LineCols.Contains("CustItemNumber")) Then oBookDetail.CustItemNumber = Trim(LineRec("CustItemNumber").ToString())
        ''    oBookDetail.CustomerNumber = "0" 'Modified by RHR for v-7.0.5.102 10/17/2016 we do not use the CUSTNMBR here  HeaderRec("CUSTNMBR").ToString oBookHeader.PODefaultCustomer
        'oBookDetail.POOrderSequence = 0
        ''    If (LineCols.Contains("PalletType")) Then oBookDetail.PalletType = Trim(LineRec("PalletType").ToString())
        ''    If (LineCols.Contains("POItemHazmatTypeCode")) Then oBookDetail.POItemHazmatTypeCode = Trim(LineRec("POItemHazmatTypeCode").ToString())
        ''    If (LineCols.Contains("POItem49CFRCode")) Then oBookDetail.POItem49CFRCode = Trim(LineRec("POItem49CFRCode").ToString())
        ''    If (LineCols.Contains("POItemIATACode")) Then oBookDetail.POItemIATACode = Trim(LineRec("POItemIATACode").ToString())
        ''    If (LineCols.Contains("POItemDOTCode")) Then oBookDetail.POItemDOTCode = Trim(LineRec("POItemDOTCode").ToString())
        ''    If (LineCols.Contains("POItemMarineCode")) Then oBookDetail.POItemMarineCode = Trim(LineRec("POItemMarineCode").ToString())
        ''    If (LineCols.Contains("POItemNMFCClass")) Then oBookDetail.POItemNMFCClass = Trim(LineRec("POItemNMFCClass").ToString())
        ''    If (LineCols.Contains("POItemFAKClass")) Then oBookDetail.POItemFAKClass = Trim(LineRec("POItemFAKClass").ToString())
        ''    If (LineCols.Contains("POItemLimitedQtyFlag")) Then Boolean.TryParse(LineRec("POItemLimitedQtyFlag").ToString(), oBookDetail.POItemLimitedQtyFlag)
        ''    If (LineCols.Contains("POItemPallets")) Then Double.TryParse(LineRec("POItemPallets").ToString(), oBookDetail.POItemPallets)
        ''    If (LineCols.Contains("POItemTies")) Then Double.TryParse(LineRec("POItemTies").ToString(), oBookDetail.POItemTies)
        ''    If (LineCols.Contains("POItemHighs")) Then Double.TryParse(LineRec("POItemHighs").ToString(), oBookDetail.POItemHighs)
        ''    If (LineCols.Contains("POItemQtyPalletPercentage")) Then Double.TryParse(LineRec("POItemQtyPalletPercentage").ToString(), oBookDetail.POItemQtyPalletPercentage)
        'oBookDetail.POItemQtyLength = "info.Item.ItemLength"
        'oBookDetail.POItemQtyWidth = "info.Item.ItemWidth"
        'oBookDetail.POItemQtyHeight = "info.Item.ItemHeight"
        'oBookDetail.POItemStackable = "info.Item.ItemStackable"
        ''    If (LineCols.Contains("POItemLevelOfDensity")) Then GPFunctions.TryParseInt(LineRec("POItemLevelOfDensity").ToString(), oBookDetail.POItemLevelOfDensity)
        ''    oBookDetail.POItemCompLegalEntity = Trim(TMSSetting.LegalEntity.ToString())
        ''    oBookDetail.POItemCompAlphaCode = Trim(HeaderRec("LOCNCODE"))
        ''    'oBookDetail.POItemCompAlphaCode = "WAREHOUSE"
        ''    If (LineCols.Contains("POItemNMFCSubClass")) Then oBookDetail.POItemNMFCSubClass = Trim(LineRec("POItemNMFCSubClass").ToString())
        ''    If (LineCols.Contains("POItemUser1")) Then oBookDetail.POItemUser1 = Trim(LineRec("POItemUser1").ToString())
        ''    If (LineCols.Contains("POItemUser2")) Then oBookDetail.POItemUser2 = Trim(LineRec("POItemUser2").ToString())
        ''    If (LineCols.Contains("POItemUser3")) Then oBookDetail.POItemUser3 = Trim(LineRec("POItemUser3").ToString())
        ''    If (LineCols.Contains("POItemUser4")) Then oBookDetail.POItemUser4 = Trim(LineRec("POItemUser4").ToString())
        ''    If (LineCols.Contains("POItemWeightUnitOfMeasure")) Then oBookDetail.POItemWeightUnitOfMeasure = Trim(LineRec("POItemWeightUnitOfMeasure").ToString())
        ''    If (LineCols.Contains("POItemCubeUnitOfMeasure")) Then oBookDetail.POItemCubeUnitOfMeasure = Trim(LineRec("POItemCubeUnitOfMeasure").ToString())
        ''    If (LineCols.Contains("POItemDimensionUnitOfMeasure")) Then oBookDetail.POItemDimensionUnitOfMeasure = Trim(LineRec("POItemDimensionUnitOfMeasure").ToString())

        ''    'If Me.Debug Then Log("Adding Item Detail  Item Number: " & LineRec("Itemnmbr").ToString)
        ''    oBookDetails.Add(oBookDetail)

        ''    oBookDetail = Nothing

        ''End While

        
#Disable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
''' Report something if no line recvords
        ''If (Counter = 0) Then

        ''    If Me.Verbose Then Log("No Line records to process for SOP #: " & SOPOrder.SOPOrders.ToString)

        ''End If
        'ProcessObjectData(oBookHeaders, oBookDetails, Me.ConnectionString)

        ''Dim ReturnMessage As String = oBookIntegration.LastError
        ''If Me.Debug Then Log("Results = " & oResults.ToString())
        ''Select Case oResults
        ''    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
        ''        If String.IsNullOrWhiteSpace(ReturnMessage) Then
        ''            If Me.Verbose Then Log("Data Connection Failure! could not import Order information")
        ''        Else
        ''            sIntegrationErrors.Add("Data Connection Failure! could not import Order information:  " & ReturnMessage)
        ''        End If

        ''    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ''        If String.IsNullOrWhiteSpace(ReturnMessage) Then
        ''            If Me.Verbose Then Log("Integration Failure! could not import Order information")
        ''        Else
        ''            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
        ''        End If

        ''    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
        ''        If String.IsNullOrWhiteSpace(ReturnMessage) Then
        ''            If Me.Verbose Then Log("Integration Had Errors! could not import some Order information")
        ''        Else
        ''            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
        ''        End If

        ''                        'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
        ''                        'blnRet = True
        ''                        'End If
        ''    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
        ''        If String.IsNullOrWhiteSpace(ReturnMessage) Then
        ''            If Me.Verbose Then Log("Data Validation Failure! could not import some Order information")
        ''        Else
        ''            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
        ''        End If

        ''        'If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
        ''        'blnRet = True
        ''        'End If
        ''    Case Else
        ''        'success
        ''        Dim strNumbers As String = "N/A"
        ''        If Not SOOrdersProcessed Is Nothing AndAlso SOOrdersProcessed.Count() > 0 Then
        ''            strNumbers = String.Join(", ", SOOrdersProcessed)
        ''        End If

        ''        sLogMsgs.Add("Success! the following Order Numbers were processed: " & strNumbers)
        ''        'Processed = oResults.
        ''        'TODO: add code to send confirmation back to NAV that the orders were processed
        ''        'mark process and success
        ''        'blnRet = True
        ''End Select

    End Function
#Enable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
#Enable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
#Enable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
#Enable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.
#Enable Warning BC42304 ' XML documentation parse error: XML entity references are not supported. XML comment will be ignored.
#Enable Warning BC42303 ' XML comment cannot appear within a method or a property. XML comment will be ignored.


#End Region

#End Region


#Region "NGLSharedDataService Events"

    Private Sub _SharedServices_BatchProcessingReturnEvent(sender As Object, e As FMWCFProxy.FMSharedServicesBatchProcessingReturnEventArgs) Handles _SharedServices.BatchProcessingReturnEvent
        If Not e Is Nothing AndAlso Not e.Result Is Nothing Then
            Select Case e.Result.Caller
                Case "clsBook"
                    Log(String.Format("Batch Process Returned {0} with {1} ", e.Result.Success, e.Result.LastError))
            End Select
        End If

        'Try
        '    If Not mblnSharedServiceRunning Then SharedServices.LogOff(WCFDataProperties)
        'Catch ex As Exception
        '    Log("Shared Services Log off Error: " & ex.Message)
        'End Try

    End Sub


    'Private Sub NGLSharedDataService_FaultException(sender As Object, e As FMWCFProxy.FaultExceptionEventArgs) Handles NGLSharedDataService.FaultException
    '    Dim strMessage As String = NGLSharedDataService.FormatDefaultFaultExceptionMessage(e.Source, e.Reason, e.Message, e.Detail)
    '    GroupEmailMsg &= strMessage
    '    Log(strMessage)
    'End Sub

    'Private Sub NGLSharedDataService_TimeOutException(sender As Object, e As FMWCFProxy.FaultExceptionEventArgs) Handles NGLSharedDataService.TimeOutException
    '    Dim strMessage As String = NGLSharedDataService.FormatDefaultFaultExceptionMessage(e.Source, e.Reason, e.Message, e.Detail)
    '    GroupEmailMsg &= strMessage
    '    Log(strMessage)
    'End Sub

    Private Sub _SharedServices_ExceptionEvent(sender As Object, e As FMWCFProxy.FMSharedServicesExceptionEventArgs) Handles _SharedServices.ExceptionEvent
        If Not e Is Nothing AndAlso Not e.Exceptions Is Nothing AndAlso e.Exceptions.Count > 0 Then
            Dim strMsg As String = ""
            Select Case e.Caller
                Case "clsBook"
                    For Each ex In e.Exceptions
                        strMsg &= String.Concat(SharedServices.LocalizeString(ex.Message), SharedServices.LocalizeString(ex.Details), vbCrLf, vbCrLf)
                    Next
                    GroupEmailMsg &= strMsg
                    Log(strMsg)
                Case Else
                    'other modules or classes handle these errors
            End Select
        End If
    End Sub

#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class



'<Serializable()> _
'Public Class clsBook1 : Inherits clsDownload

'    Public Delegate Sub ProcessDataDelegate()

'#Region "Constructors"

'    Sub New()
'        MyBase.New()
'    End Sub

'    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
'        MyBase.New(config)
'    End Sub

'    Sub New(ByVal admin_email As String, _
'            ByVal from_email As String, _
'            ByVal group_email As String, _
'            ByVal auto_retry As Integer, _
'            ByVal smtp_server As String, _
'            ByVal db_server As String, _
'            ByVal database_catalog As String, _
'            ByVal auth_code As String, _
'            ByVal debug_mode As Boolean,
'            Optional ByVal connection_string As String = "")

'        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


'    End Sub

'#End Region

'#Region " Class Variables and Properties "
'    Private mintTotalOrders As Integer = 0
'    Private mdblHashTotalOrders As Double = 0
'    Private mintTotalDetails As Integer = 0
'    Private mintTotalQty As Integer = 0
'    Private mdblTotalWeight As Double = 0
'    Private mdblHashTotalDetails As Double = 0
'    Private mintErrCt As Integer = 0
'    'Private objF As New FMLib.clsStandardFunctions
'    Private mstrOrderNotificationEmail As String = ""
'    Private mdCompanies As New Dictionary(Of String, Integer)
'    Private mblnSomeItemsMissing As Boolean = False
'    Private mintImportedCompControls As New List(Of Integer)
'    Private mstrOrderNumbers As New List(Of String)
'    Private mblnSharedServiceRunning As Boolean
'    Private mintSharedServicesRunning As Integer = 0

'    Private Shared mPadLock As New Object

'    Private WithEvents _SharedServices As Ngl.FMWCFProxy.FMSharedServicesClient = Ngl.FMWCFProxy.FMSharedServicesClient.GetInstance(New Ngl.FMWCFProxy.NGLSynchronizationContext(System.Threading.SynchronizationContext.Current))
'    Public ReadOnly Property SharedServices As Ngl.FMWCFProxy.FMSharedServicesClient
'        Get
'            Return _SharedServices
'        End Get
'    End Property

'    'Private WithEvents NGLSharedDataService As New Ngl.FMWCFProxy.FMSharedServicesClient(New Ngl.FMWCFProxy.NGLSynchronizationContext(System.Threading.SynchronizationContext.Current))

'    Private _WCFDataProperties As Ngl.FMWCFProxy.FMDataProperties
'    Private Property WCFDataProperties As Ngl.FMWCFProxy.FMDataProperties
'        Get
'            If _WCFDataProperties Is Nothing Then
'                _WCFDataProperties = New NGL.FMWCFProxy.FMDataProperties
'                With _WCFDataProperties
'                    .Database = Me.Database
'                    .DBServer = Me.DBServer
'                    .UserName = Me.mstrCreateUser
'                    .WCFAuthCode = Me.WCFAuthCode
'                    .WCFServiceURL = Me.WCFURL
'                    .WCFTCPServiceURL = Me.WCFTCPURL
'                    .ConnectionString = Me.ConnectionString
'                    'NOTE:  For multiple calls to the service it is faster
'                    'to set the FormControl to 0 and leave the FormName blank
'                    'This is because the Service checks if the user is authorized
'                    'To access the form any time the form values are provided.
'                    'The same logic applies to reports and procedures.
'                    .FormControl = 0
'                    .FormName = ""
'                End With
'            End If
'            Return _WCFDataProperties
'        End Get
'        Set(value As Ngl.FMWCFProxy.FMDataProperties)
'            _WCFDataProperties = value
'        End Set
'    End Property


'    Public Property OrderNotificationEmail() As String
'        Get
'            Return mstrOrderNotificationEmail
'        End Get
'        Set(ByVal value As String)
'            mstrOrderNotificationEmail = value
'        End Set
'    End Property
'    Private mblnValidateOrderUniqueness As Boolean = False
'    Public Property ValidateOrderUniqueness() As Boolean
'        Get
'            Return mblnValidateOrderUniqueness
'        End Get
'        Set(ByVal value As Boolean)
'            mblnValidateOrderUniqueness = value
'        End Set
'    End Property


'    Public ReadOnly Property TotalWeight() As Double
'        Get
'            TotalWeight = mdblTotalWeight
'        End Get
'    End Property

'    Public ReadOnly Property TotalQty() As Integer
'        Get
'            TotalQty = mintTotalQty
'        End Get
'    End Property

'    Public ReadOnly Property HashTotalDetails() As Double
'        Get
'            HashTotalDetails = mdblHashTotalDetails
'        End Get
'    End Property

'    Public ReadOnly Property TotalDetails() As Integer
'        Get
'            TotalDetails = mintTotalDetails
'        End Get
'    End Property

'    Public ReadOnly Property HashTotalOrders() As Double
'        Get
'            HashTotalOrders = mdblHashTotalOrders
'        End Get
'    End Property

'    Public ReadOnly Property TotalOrders() As Integer
'        Get
'            TotalOrders = mintTotalOrders
'        End Get
'    End Property

'    Public Function getDataSet() As BookData
'        Return New BookData
'    End Function

'#End Region

'#Region "Private Functions "

'    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            With oFields
'                .Add("PONumber", "POHDROrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK) '0
'                .Add("PODefaultCustomer", "POHDRDefaultCustomer", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '1
'                .Add("POOrderSequence", "POHDROrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK) '2
'                .Add("POCustomerPO", "POHDRnumber", clsImportField.DataTypeID.gcvdtString, 20, True) '3
'                .Add("POdate", " POHDRPOdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '4
'                .Add("POvendor", "POHDRvendor", clsImportField.DataTypeID.gcvdtString, 50, True) '5
'                .Add("POShipdate", "POHDRShipdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '6
'                .Add("POBuyer", "POHDRBuyer", clsImportField.DataTypeID.gcvdtString, 10, True) '7
'                .Add("POFrt", "POHDRFrt", clsImportField.DataTypeID.gcvdtTinyInt, 6, True) '8
'                '.Add("CreateUser", "POHDRCreateUser", clsImportField.DataTypeID.gcvdtString, 25, False) '9
'                '.Add("CreatedDate", "POHDRCreateDate", clsImportField.DataTypeID.gcvdtDate, 22, False) '10
'                .Add("POTotalFrt", "POHDRTotalFrt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '11
'                .Add("POTotalCost", "POHDRTotalCost", clsImportField.DataTypeID.gcvdtFloat, 20, True) '12
'                .Add("POWgt", "POHDRWgt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '13
'                .Add("POCube", "POHDRCube", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '14
'                .Add("POQty", "POHDRQty", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '15
'                .Add("POPallets", "POHDRPallets", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '16
'                .Add("POLines", "POHDRLines", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '17
'                .Add("POConfirm", "POHDRConfirm", clsImportField.DataTypeID.gcvdtBit, 2, True) '18
'                .Add("PODefaultCarrier", "POHDRDefaultCarrier", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '19
'                .Add("POReqDate", "POHDRReqDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '20
'                .Add("POShipInstructions", "POHDRShipInstructions", clsImportField.DataTypeID.gcvdtString, 255, True) '21
'                .Add("POCooler", "POHDRCooler", clsImportField.DataTypeID.gcvdtBit, 2, True) '22
'                .Add("POFrozen", "POHDRFrozen", clsImportField.DataTypeID.gcvdtBit, 2, True) '23
'                .Add("PODry", "POHDRDry", clsImportField.DataTypeID.gcvdtBit, 2, True) '24
'                .Add("POTemp", "POHDRTemp", clsImportField.DataTypeID.gcvdtString, 1, True) '25
'                .Add("POCarType", "POHDRCarType", clsImportField.DataTypeID.gcvdtString, 15, True) '26
'                .Add("POShipVia", "POHDRShipVia", clsImportField.DataTypeID.gcvdtString, 10, True) '27
'                .Add("POShipViaType", "POHDRShipViaType", clsImportField.DataTypeID.gcvdtString, 10, True) '28
'                .Add("POOtherCosts", "POHDROtherCost", clsImportField.DataTypeID.gcvdtFloat, 22, True) '29
'                .Add("POConsigneeNumber", "POConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 10, True, clsImportField.PKValue.gcHK) '30
'                .Add("POStatusFlag", "POHDRStatusFlag", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '31
'                .Add("POChepGLID", "POHDRChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '32
'                .Add("POCarrierEquipmentCodes", "POHDRCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True) '33
'                .Add("POCarrierTypeCode", "POHDRCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '34
'                .Add("POPalletPositions", "POHDRPalletPositions", clsImportField.DataTypeID.gcvdtString, 50, True) '35
'                .Add("POSchedulePUDate", "POHDRSchedulePUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '36
'                .Add("POSchedulePUTime", "POHDRSchedulePUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '37
'                .Add("POScheduleDelDate", "POHDRScheduleDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '38
'                .Add("POSCheduleDelTime", "POHDRScheduleDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '39
'                .Add("POActPUDate", "POHDRActPUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '40
'                .Add("POActPUTime", "POHDRActPUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '41
'                .Add("POActDelDate", "POHDRActDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
'                .Add("POActDelTime", "POHDRActDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '43  


'            End With
'            Log("PO Header Field Array Loaded.")
'            'get the import field flag values
'            For ct As Integer = 1 To oFields.Count
'                Dim blnUseField As Boolean = True
'                Try
'                    If oFields(ct).Name = "POHDROrderNumber" Or oFields(ct).Name = "POHDRDefaultCustomer" Or oFields(ct).Name = "POHDROrderSequence" Then
'                        'These are key fields and are always in use
'                        blnUseField = True
'                    Else
'                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Book)
'                    End If
'                Catch ex As Exception
'                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
'                End Try
'                oFields(ct).Use = blnUseField
'            Next
'            Ret = True
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.buildHeaderCollection Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function

'    Private Function buildHeaderCollection60(ByRef oFields As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            With oFields
'                .Add("PONumber", "POHDROrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK) '0
'                .Add("PODefaultCustomer", "POHDRDefaultCustomer", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '1
'                .Add("POOrderSequence", "POHDROrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK) '2
'                .Add("POCustomerPO", "POHDRnumber", clsImportField.DataTypeID.gcvdtString, 20, True) '3
'                .Add("POdate", " POHDRPOdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '4
'                .Add("POVendor", "POHDRvendor", clsImportField.DataTypeID.gcvdtString, 50, True) '5
'                .Add("POShipdate", "POHDRShipdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '6
'                .Add("POBuyer", "POHDRBuyer", clsImportField.DataTypeID.gcvdtString, 10, True) '7
'                .Add("POFrt", "POHDRFrt", clsImportField.DataTypeID.gcvdtTinyInt, 6, True) '8
'                '.Add("CreateUser", "POHDRCreateUser", clsImportField.DataTypeID.gcvdtString, 25, False) '9
'                '.Add("CreatedDate", "POHDRCreateDate", clsImportField.DataTypeID.gcvdtDate, 22, False) '10
'                .Add("POTotalFrt", "POHDRTotalFrt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '11
'                .Add("POTotalCost", "POHDRTotalCost", clsImportField.DataTypeID.gcvdtFloat, 20, True) '12
'                .Add("POWgt", "POHDRWgt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '13
'                .Add("POCube", "POHDRCube", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '14
'                .Add("POQty", "POHDRQty", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '15
'                .Add("POPallets", "POHDRPallets", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '16
'                .Add("POLines", "POHDRLines", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '17
'                .Add("POConfirm", "POHDRConfirm", clsImportField.DataTypeID.gcvdtBit, 2, True) '18
'                .Add("PODefaultCarrier", "POHDRDefaultCarrier", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '19
'                .Add("POReqDate", "POHDRReqDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '20
'                .Add("POShipInstructions", "POHDRShipInstructions", clsImportField.DataTypeID.gcvdtString, 255, True) '21
'                .Add("POCooler", "POHDRCooler", clsImportField.DataTypeID.gcvdtBit, 2, True) '22
'                .Add("POFrozen", "POHDRFrozen", clsImportField.DataTypeID.gcvdtBit, 2, True) '23
'                .Add("PODry", "POHDRDry", clsImportField.DataTypeID.gcvdtBit, 2, True) '24
'                .Add("POTemp", "POHDRTemp", clsImportField.DataTypeID.gcvdtString, 1, True) '25
'                .Add("POCarType", "POHDRCarType", clsImportField.DataTypeID.gcvdtString, 15, True) '26
'                .Add("POShipVia", "POHDRShipVia", clsImportField.DataTypeID.gcvdtString, 10, True) '27
'                .Add("POShipViaType", "POHDRShipViaType", clsImportField.DataTypeID.gcvdtString, 10, True) '28
'                .Add("POOtherCosts", "POHDROtherCost", clsImportField.DataTypeID.gcvdtFloat, 22, True) '29
'                .Add("POConsigneeNumber", "POConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 10, True, clsImportField.PKValue.gcHK) '30
'                .Add("POStatusFlag", "POHDRStatusFlag", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '31
'                .Add("POChepGLID", "POHDRChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '32
'                .Add("POCarrierEquipmentCodes", "POHDRCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True) '33
'                .Add("POCarrierTypeCode", "POHDRCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '34
'                .Add("POPalletPositions", "POHDRPalletPositions", clsImportField.DataTypeID.gcvdtString, 50, True) '35
'                .Add("POSchedulePUDate", "POHDRSchedulePUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '36
'                .Add("POSchedulePUTime", "POHDRSchedulePUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '37
'                .Add("POScheduleDelDate", "POHDRScheduleDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '38
'                .Add("POSCheduleDelTime", "POHDRScheduleDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '39
'                .Add("POActPUDate", "POHDRActPUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '40
'                .Add("POActPUTime", "POHDRActPUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '41
'                .Add("POActDelDate", "POHDRActDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
'                .Add("POActDelTime", "POHDRActDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '43 
'                .Add("POOrigCompNumber", "POHDROrigCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '44
'                .Add("POOrigName", "POHDROrigName", clsImportField.DataTypeID.gcvdtString, 40, True) '45
'                .Add("POOrigAddress1", "POHDROrigAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '46
'                .Add("POOrigAddress2", "POHDROrigAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '47
'                .Add("POOrigAddress3", "POHDROrigAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '48
'                .Add("POOrigCity", "POHDROrigCity", clsImportField.DataTypeID.gcvdtString, 25, True) '49
'                .Add("POOrigState", "POHDROrigState", clsImportField.DataTypeID.gcvdtString, 8, True) '50
'                .Add("POOrigCountry", "POHDROrigCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '51
'                .Add("POOrigZip", "POHDROrigZip", clsImportField.DataTypeID.gcvdtString, 10, True) '52
'                .Add("POOrigContactPhone", "POHDROrigContactPhone", clsImportField.DataTypeID.gcvdtString, 15, True) '53
'                .Add("POOrigContactPhoneExt", "POHDROrigContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '54
'                .Add("POOrigContactFax", "POHDROrigContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '55
'                .Add("PODestCompNumber", "POHDRDestCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '56
'                .Add("PODestName", "POHDRDestName", clsImportField.DataTypeID.gcvdtString, 40, True) '57
'                .Add("PODestAddress1", "POHDRDestAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '58
'                .Add("PODestAddress2", "POHDRDestAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '59
'                .Add("PODestAddress3", "POHDRDestAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '60
'                .Add("PODestCity", "POHDRDestCity", clsImportField.DataTypeID.gcvdtString, 25, True) '61
'                .Add("PODestState", "POHDRDestState", clsImportField.DataTypeID.gcvdtString, 2, True) '62
'                .Add("PODestCountry", "POHDRDestCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '63
'                .Add("PODestZip", "POHDRDestZip", clsImportField.DataTypeID.gcvdtString, 10, True) '64
'                .Add("PODestContactPhone", "POHDRDestContactPhone", clsImportField.DataTypeID.gcvdtString, 15, True) '65
'                .Add("PODestContactPhoneExt", "POHDRDestContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '66
'                .Add("PODestContactFax", "POHDRDestContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '67
'                .Add("POPalletExchange", "POHDRPalletExchange", clsImportField.DataTypeID.gcvdtBit, 2, True) '68
'                .Add("POPalletType", "POHDRPalletType", clsImportField.DataTypeID.gcvdtString, 50, True) '69
'                .Add("POComments", "POHDRComments", clsImportField.DataTypeID.gcvdtString, 255, True) '70
'                .Add("POCommentsConfidential", "POHDRCommentsConfidential", clsImportField.DataTypeID.gcvdtString, 255, True) '71
'                .Add("POInbound", "POHDRInbound", clsImportField.DataTypeID.gcvdtBit, 2, True) '72
'                .Add("PODefaultRouteSequence", "POHDRDefaultRouteSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '73
'                .Add("PORouteGuideNumber", "POHDRRouteGuideNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '74

'            End With
'            Log("PO Header Field Array Loaded.")
'            'get the import field flag values
'            For ct As Integer = 1 To oFields.Count
'                Dim blnUseField As Boolean = True
'                Try
'                    If oFields(ct).Name = "POHDROrderNumber" Or oFields(ct).Name = "POHDRDefaultCustomer" Or oFields(ct).Name = "POHDROrderSequence" Then
'                        'These are key fields and are always in use
'                        blnUseField = True
'                    Else
'                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Book)
'                    End If
'                Catch ex As Exception
'                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
'                End Try
'                oFields(ct).Use = blnUseField
'            Next
'            Ret = True
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildHeaderCollection60, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.buildHeaderCollection60 Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function

'    Private Function buildHeaderCollection70(ByRef oFields As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            With oFields
'                .Add("PONumber", "POHDROrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK) '0
'                .Add("PODefaultCustomer", "POHDRDefaultCustomer", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '1
'                .Add("POOrderSequence", "POHDROrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK) '2
'                .Add("POCustomerPO", "POHDRnumber", clsImportField.DataTypeID.gcvdtString, 20, True) '3
'                .Add("POdate", " POHDRPOdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '4
'                .Add("POVendor", "POHDRvendor", clsImportField.DataTypeID.gcvdtString, 50, True) '5
'                .Add("POShipdate", "POHDRShipdate", clsImportField.DataTypeID.gcvdtDate, 22, True) '6
'                .Add("POBuyer", "POHDRBuyer", clsImportField.DataTypeID.gcvdtString, 10, True) '7
'                .Add("POFrt", "POHDRFrt", clsImportField.DataTypeID.gcvdtTinyInt, 6, True) '8
'                '.Add("CreateUser", "POHDRCreateUser", clsImportField.DataTypeID.gcvdtString, 25, False) '9
'                '.Add("CreatedDate", "POHDRCreateDate", clsImportField.DataTypeID.gcvdtDate, 22, False) '10
'                .Add("POTotalFrt", "POHDRTotalFrt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '11
'                .Add("POTotalCost", "POHDRTotalCost", clsImportField.DataTypeID.gcvdtFloat, 20, True) '12
'                .Add("POWgt", "POHDRWgt", clsImportField.DataTypeID.gcvdtFloat, 20, True) '13
'                .Add("POCube", "POHDRCube", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '14
'                .Add("POQty", "POHDRQty", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '15
'                .Add("POPallets", "POHDRPallets", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '16
'                .Add("POLines", "POHDRLines", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '17
'                .Add("POConfirm", "POHDRConfirm", clsImportField.DataTypeID.gcvdtBit, 2, True) '18
'                .Add("PODefaultCarrier", "POHDRDefaultCarrier", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '19
'                .Add("POReqDate", "POHDRReqDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '20
'                .Add("POShipInstructions", "POHDRShipInstructions", clsImportField.DataTypeID.gcvdtString, 255, True) '21
'                .Add("POCooler", "POHDRCooler", clsImportField.DataTypeID.gcvdtBit, 2, True) '22
'                .Add("POFrozen", "POHDRFrozen", clsImportField.DataTypeID.gcvdtBit, 2, True) '23
'                .Add("PODry", "POHDRDry", clsImportField.DataTypeID.gcvdtBit, 2, True) '24
'                .Add("POTemp", "POHDRTemp", clsImportField.DataTypeID.gcvdtString, 1, True) '25
'                .Add("POCarType", "POHDRCarType", clsImportField.DataTypeID.gcvdtString, 15, True) '26
'                .Add("POShipVia", "POHDRShipVia", clsImportField.DataTypeID.gcvdtString, 10, True) '27
'                .Add("POShipViaType", "POHDRShipViaType", clsImportField.DataTypeID.gcvdtString, 10, True) '28
'                .Add("POOtherCosts", "POHDROtherCost", clsImportField.DataTypeID.gcvdtFloat, 22, True) '29
'                .Add("POConsigneeNumber", "POConsigneeNumber", clsImportField.DataTypeID.gcvdtString, 10, True, clsImportField.PKValue.gcHK) '30
'                .Add("POStatusFlag", "POHDRStatusFlag", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '31
'                .Add("POChepGLID", "POHDRChepGLID", clsImportField.DataTypeID.gcvdtString, 50, True) '32
'                .Add("POCarrierEquipmentCodes", "POHDRCarrierEquipmentCodes", clsImportField.DataTypeID.gcvdtString, 50, True) '33
'                .Add("POCarrierTypeCode", "POHDRCarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True) '34
'                .Add("POPalletPositions", "POHDRPalletPositions", clsImportField.DataTypeID.gcvdtString, 50, True) '35
'                .Add("POSchedulePUDate", "POHDRSchedulePUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '36
'                .Add("POSchedulePUTime", "POHDRSchedulePUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '37
'                .Add("POScheduleDelDate", "POHDRScheduleDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '38
'                .Add("POSCheduleDelTime", "POHDRScheduleDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '39
'                .Add("POActPUDate", "POHDRActPUDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '40
'                .Add("POActPUTime", "POHDRActPUTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '41
'                .Add("POActDelDate", "POHDRActDelDate", clsImportField.DataTypeID.gcvdtDate, 22, True) '42
'                .Add("POActDelTime", "POHDRActDelTime", clsImportField.DataTypeID.gcvdtTime, 22, True) '43 
'                .Add("POOrigCompNumber", "POHDROrigCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '44
'                .Add("POOrigName", "POHDROrigName", clsImportField.DataTypeID.gcvdtString, 40, True) '45
'                .Add("POOrigAddress1", "POHDROrigAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '46
'                .Add("POOrigAddress2", "POHDROrigAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '47
'                .Add("POOrigAddress3", "POHDROrigAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '48
'                .Add("POOrigCity", "POHDROrigCity", clsImportField.DataTypeID.gcvdtString, 25, True) '49
'                .Add("POOrigState", "POHDROrigState", clsImportField.DataTypeID.gcvdtString, 8, True) '50
'                .Add("POOrigCountry", "POHDROrigCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '51
'                .Add("POOrigZip", "POHDROrigZip", clsImportField.DataTypeID.gcvdtString, 10, True) '52
'                .Add("POOrigContactPhone", "POHDROrigContactPhone", clsImportField.DataTypeID.gcvdtString, 15, True) '53
'                .Add("POOrigContactPhoneExt", "POHDROrigContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '54
'                .Add("POOrigContactFax", "POHDROrigContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '55
'                .Add("PODestCompNumber", "POHDRDestCompNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '56
'                .Add("PODestName", "POHDRDestName", clsImportField.DataTypeID.gcvdtString, 40, True) '57
'                .Add("PODestAddress1", "POHDRDestAddress1", clsImportField.DataTypeID.gcvdtString, 40, True) '58
'                .Add("PODestAddress2", "POHDRDestAddress2", clsImportField.DataTypeID.gcvdtString, 40, True) '59
'                .Add("PODestAddress3", "POHDRDestAddress3", clsImportField.DataTypeID.gcvdtString, 40, True) '60
'                .Add("PODestCity", "POHDRDestCity", clsImportField.DataTypeID.gcvdtString, 25, True) '61
'                .Add("PODestState", "POHDRDestState", clsImportField.DataTypeID.gcvdtString, 2, True) '62
'                .Add("PODestCountry", "POHDRDestCountry", clsImportField.DataTypeID.gcvdtString, 30, True) '63
'                .Add("PODestZip", "POHDRDestZip", clsImportField.DataTypeID.gcvdtString, 10, True) '64
'                .Add("PODestContactPhone", "POHDRDestContactPhone", clsImportField.DataTypeID.gcvdtString, 15, True) '65
'                .Add("PODestContactPhoneExt", "POHDRDestContactPhoneExt", clsImportField.DataTypeID.gcvdtString, 50, True) '66
'                .Add("PODestContactFax", "POHDRDestContactFax", clsImportField.DataTypeID.gcvdtString, 15, True) '67
'                .Add("POPalletExchange", "POHDRPalletExchange", clsImportField.DataTypeID.gcvdtBit, 2, True) '68
'                .Add("POPalletType", "POHDRPalletType", clsImportField.DataTypeID.gcvdtString, 50, True) '69
'                .Add("POComments", "POHDRComments", clsImportField.DataTypeID.gcvdtString, 255, True) '70
'                .Add("POCommentsConfidential", "POHDRCommentsConfidential", clsImportField.DataTypeID.gcvdtString, 255, True) '71
'                .Add("POInbound", "POHDRInbound", clsImportField.DataTypeID.gcvdtBit, 2, True) '72
'                .Add("PODefaultRouteSequence", "POHDRDefaultRouteSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True) '73
'                .Add("PORouteGuideNumber", "POHDRRouteGuideNumber", clsImportField.DataTypeID.gcvdtString, 50, True) '74
'                .Add("POCompLegalEntity", "POHDRCompLegalEntity", clsImportField.DataTypeID.gcvdtString, 50, True) '75
'                .Add("POCompAlphaCode", "POHDRCompAlphaCode", clsImportField.DataTypeID.gcvdtString, 50, True) '76
'                .Add("POModeTypeControl", "POHDRModeTypeControl", clsImportField.DataTypeID.gcvdtLongInt, 20, True) '77
'                .Add("POMustLeaveByDateTime", "POHDRMustLeaveByDateTime", clsImportField.DataTypeID.gcvdtDate, 22, True) '78
'                .Add("POUser1", "POHDRUser1", clsImportField.DataTypeID.gcvdtString, 4000, True) '79
'                .Add("POUser2", "POHDRUser2", clsImportField.DataTypeID.gcvdtString, 4000, True) '80
'                .Add("POUser3", "POHDRUser3", clsImportField.DataTypeID.gcvdtString, 4000, True) '81
'                .Add("POUser4", "POHDRUser4", clsImportField.DataTypeID.gcvdtString, 4000, True) '82

'            End With
'            Log("PO Header Field Array Loaded.")
'            'get the import field flag values
'            For ct As Integer = 1 To oFields.Count
'                Dim blnUseField As Boolean = True
'                Try
'                    If oFields(ct).Name = "POHDROrderNumber" Or oFields(ct).Name = "POHDRDefaultCustomer" Or oFields(ct).Name = "POHDROrderSequence" Then
'                        'These are key fields and are always in use
'                        blnUseField = True
'                    Else
'                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Book)
'                    End If
'                Catch ex As Exception
'                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
'                End Try
'                oFields(ct).Use = blnUseField
'            Next
'            Ret = True
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildHeaderCollection70, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.buildHeaderCollection70 Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function


'    Private Function buildItemCollection(ByRef oItems As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            With oItems
'                .Add("ItemPONumber", "ItemPONumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
'                .Add("CustomerNumber", "CustomerNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
'                .Add("POOrderSequence", "POOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
'                .Add("LotNumber", "LotNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
'                .Add("ItemNumber", "ItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
'                .Add("FixOffInvAllow", "FixOffInvAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("FixFrtAllow", "FixFrtAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("QtyOrdered", "QtyOrdered", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
'                .Add("FreightCost", "FreightCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("ItemCost", "ItemCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("Weight", "Weight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("Cube", "Cube", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
'                .Add("Pack", "Pack", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
'                .Add("Size", "Size", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("Description", "Description", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("Hazmat", "Hazmat", clsImportField.DataTypeID.gcvdtString, 1, True)
'                .Add("Brand", "Brand", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("CostCenter", "CostCenter", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("LotExpirationDate", "LotExpirationDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
'                .Add("GTIN", "GTIN", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("CustItemNumber", "CustItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, True)
'            End With
'            Log("PO Item Details Field Array Loaded.")
'            'get the item  field flag values
'            For ct As Integer = 1 To oItems.Count
'                Dim blnUseField As Boolean = True
'                Try
'                    If oItems(ct).Name = "ItemPONumber" Or oItems(ct).Name = "CustomerNumber" Or oItems(ct).Name = "POOrderSequence" Or oItems(ct).Name = "LotNumber" Then
'                        'These are key fields and are always in use
'                        blnUseField = True
'                    Else
'                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Book)
'                    End If
'                Catch ex As Exception
'                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
'                End Try
'                oItems(ct).Use = blnUseField
'            Next
'            Ret = True
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildItemCollection, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.buildItemCollection Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function



'    Private Function buildItemCollection70(ByRef oItems As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            With oItems
'                .Add("ItemPONumber", "ItemPONumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
'                .Add("CustomerNumber", "CustomerNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
'                .Add("POOrderSequence", "POOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
'                .Add("LotNumber", "LotNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
'                .Add("ItemNumber", "ItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
'                .Add("FixOffInvAllow", "FixOffInvAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("FixFrtAllow", "FixFrtAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("QtyOrdered", "QtyOrdered", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
'                .Add("FreightCost", "FreightCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("ItemCost", "ItemCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("Weight", "Weight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("Cube", "Cube", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
'                .Add("Pack", "Pack", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
'                .Add("Size", "Size", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("Description", "Description", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("Hazmat", "Hazmat", clsImportField.DataTypeID.gcvdtString, 1, True)
'                .Add("Brand", "Brand", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("CostCenter", "CostCenter", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("LotExpirationDate", "LotExpirationDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
'                .Add("GTIN", "GTIN", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("CustItemNumber", "CustItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, True)
'                'New fields added for v-5.2
'                .Add("POItemHazmatTypeCode", "POItemHazmatTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItem49CFRCode", "POItem49CFRCode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemIATACode", "POItemIATACode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemDOTCode", "POItemDOTCode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemMarineCode", "POItemMarineCode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemNMFCClass", "POItemNMFCClass", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemFAKClass", "POItemFAKClass", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemLimitedQtyFlag", "POItemLimitedQtyFlag", clsImportField.DataTypeID.gcvdtBit, 2, True)
'                .Add("POItemPallets", "POItemPallets", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemTies", "POItemTies", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemHighs", "POItemHighs", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemQtyPalletPercentage", "POItemQtyPalletPercentage", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemQtyLength", "POItemQtyLength", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemQtyWidth", "POItemQtyWidth", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemQtyHeight", "POItemQtyHeight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemStackable", "POItemStackable", clsImportField.DataTypeID.gcvdtBit, 2, True)
'                .Add("POItemLevelOfDensity", "POItemLevelOfDensity", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
'                .Add("POItemCompLegalEntity", "POItemCompLegalEntity", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("POItemCompAlphaCode", "POItemCompAlphaCode", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("POItemNMFCSubClass", "POItemNMFCSubClass", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemUser1", "POItemUser1", clsImportField.DataTypeID.gcvdtString, 4000, True)
'                .Add("POItemUser2", "POItemUser2", clsImportField.DataTypeID.gcvdtString, 4000, True)
'                .Add("POItemUser3", "POItemUser3", clsImportField.DataTypeID.gcvdtString, 4000, True)
'                .Add("POItemUser4", "POItemUser4", clsImportField.DataTypeID.gcvdtString, 4000, True)

'            End With
'            Log("PO Item Details Field Array Loaded.")
'            'get the item  field flag values
'            For ct As Integer = 1 To oItems.Count
'                Dim blnUseField As Boolean = True
'                Try
'                    If oItems(ct).Name = "ItemPONumber" Or oItems(ct).Name = "CustomerNumber" Or oItems(ct).Name = "POOrderSequence" Or oItems(ct).Name = "LotNumber" Then
'                        'These are key fields and are always in use
'                        blnUseField = True
'                    Else
'                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Book)
'                    End If
'                Catch ex As Exception
'                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
'                End Try
'                oItems(ct).Use = blnUseField
'            Next
'            Ret = True
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildItemCollection60, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.buildItemCollection60 Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function


'    Private Function buildItemCollection60(ByRef oItems As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            With oItems
'                .Add("ItemPONumber", "ItemPONumber", clsImportField.DataTypeID.gcvdtString, 20, False, clsImportField.PKValue.gcPK)
'                .Add("CustomerNumber", "CustomerNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
'                .Add("POOrderSequence", "POOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
'                .Add("LotNumber", "LotNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
'                .Add("ItemNumber", "ItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcPK)
'                .Add("FixOffInvAllow", "FixOffInvAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("FixFrtAllow", "FixFrtAllow", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("QtyOrdered", "QtyOrdered", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
'                .Add("FreightCost", "FreightCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("ItemCost", "ItemCost", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("Weight", "Weight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("Cube", "Cube", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
'                .Add("Pack", "Pack", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
'                .Add("Size", "Size", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("Description", "Description", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("Hazmat", "Hazmat", clsImportField.DataTypeID.gcvdtString, 1, True)
'                .Add("Brand", "Brand", clsImportField.DataTypeID.gcvdtString, 255, True)
'                .Add("CostCenter", "CostCenter", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("LotExpirationDate", "LotExpirationDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
'                .Add("GTIN", "GTIN", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("CustItemNumber", "CustItemNumber", clsImportField.DataTypeID.gcvdtString, 50, True)
'                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, True)
'                'New fields added for v-5.2
'                .Add("POItemHazmatTypeCode", "POItemHazmatTypeCode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItem49CFRCode", "POItem49CFRCode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemIATACode", "POItemIATACode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemDOTCode", "POItemDOTCode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemMarineCode", "POItemMarineCode", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemNMFCClass", "POItemNMFCClass", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemFAKClass", "POItemFAKClass", clsImportField.DataTypeID.gcvdtString, 20, True)
'                .Add("POItemLimitedQtyFlag", "POItemLimitedQtyFlag", clsImportField.DataTypeID.gcvdtBit, 2, True)
'                .Add("POItemPallets", "POItemPallets", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemTies", "POItemTies", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemHighs", "POItemHighs", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemQtyPalletPercentage", "POItemQtyPalletPercentage", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemQtyLength", "POItemQtyLength", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemQtyWidth", "POItemQtyWidth", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemQtyHeight", "POItemQtyHeight", clsImportField.DataTypeID.gcvdtFloat, 22, True)
'                .Add("POItemStackable", "POItemStackable", clsImportField.DataTypeID.gcvdtBit, 2, True)
'                .Add("POItemLevelOfDensity", "POItemLevelOfDensity", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
'            End With
'            Log("PO Item Details Field Array Loaded.")
'            'get the item  field flag values
'            For ct As Integer = 1 To oItems.Count
'                Dim blnUseField As Boolean = True
'                Try
'                    If oItems(ct).Name = "ItemPONumber" Or oItems(ct).Name = "CustomerNumber" Or oItems(ct).Name = "POOrderSequence" Or oItems(ct).Name = "LotNumber" Then
'                        'These are key fields and are always in use
'                        blnUseField = True
'                    Else
'                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Book)
'                    End If
'                Catch ex As Exception
'                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
'                End Try
'                oItems(ct).Use = blnUseField
'            Next
'            Ret = True
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.buildItemCollection60, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.buildItemCollection60 Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function

'    Private Function importHeaderRecords( _
'                ByRef oOrders As BookData.BookHeaderDataTable, _
'                ByRef oDetails As BookData.BookDetailDataTable, _
'                ByRef oFields As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try

'            Dim intRetryCt As Integer = 0
'            Dim strSource As String = "clsBook.importHeaderRecords"
'            Dim blnDataValidated As Boolean = False
'            Dim strErrorMessage As String = ""
'            Dim blnInsertRecord As Boolean = True
'            Dim intCompNumber As Integer = 0
'            Dim intCompControl As Integer = 0
'            Dim blnNoMatchingItem As Boolean = True

'            Do
'                intRetryCt += 1
'                RecordErrors = 0
'                TotalRecords = 0
'                TotalItems = 0
'                Try
'                    Log("Importing " & oOrders.Count & " PO Header Records.")
'                    For Each oRow As BookData.BookHeaderRow In oOrders
'                        'Reset the data types and values to defaults for the following fields 
'                        'at the top of each loop to handle alpha vs numeric data changes
'                        oFields("PODefaultCustomer").DataType = clsImportField.DataTypeID.gcvdtString
'                        oFields("PODefaultCustomer").Length = 50
'                        oFields("PODefaultCustomer").Null = True
'                        strErrorMessage = ""
'                        blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
'                        'Get the item details for this order (must be called before we lookup the company alpha number so numbers still match)
'                        Dim oTheseDetails As BookData.BookDetailDataTable = getItemDetails(oFields, oDetails)
'                        If oTheseDetails.Count < 1 Then
'                            blnNoMatchingItem = True
'                            'the modual level property determines if we insert all items 
'                            'after the headers.  if any header is missing an item then
'                            'we re-import all items 
'                            mblnSomeItemsMissing = True
'                        Else
'                            blnNoMatchingItem = False
'                        End If
'                        'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
'                        If blnDataValidated Then blnDataValidated = validateCompany(oFields("PODefaultCustomer"), _
'                                                                                            strErrorMessage, _
'                                                                                            oCX, _
'                                                                                            strSource)

'                        'Create a new item list object
'                        Dim oItemList As New List(Of clsImportFields)

'                        If blnDataValidated Then
'                            'Lookup the Company Control Number if needed; but only if there are no validation errors
'                            intCompControl = oCX.getControlByNumber(oFields("PODefaultCustomer").Value)
'                            If intCompControl = 0 Then
'                                intCompControl = Me.lookupCompControlByNumber(oFields("PODefaultCustomer").Value)
'                                'now add the comp control to the collection this save future trips to the database
'                                'by sending nothing as the alpha code any existing alpha code does not get replaced
'                                oCX.AddNew(Nothing, oFields("PODefaultCustomer").Value, intCompControl)
'                            End If
'                            'save the compcontrol number to a list so we only silent tender companies associated with this data
'                            If mintImportedCompControls Is Nothing Then mintImportedCompControls = New List(Of Integer)
'                            If Not mintImportedCompControls.Contains(intCompControl) Then mintImportedCompControls.Add(intCompControl)
'                            'Fill the Item Detail List
'                            If Not blnNoMatchingItem Then
'                                If Not filItemRecordList(oTheseDetails, oItemList) Then
'                                    'we cannot read the item so set the module level items missing propery to true
'                                    mblnSomeItemsMissing = True
'                                End If
'                            End If
'                        End If
'                        'Process the NO Lane Logic
'                        'If blnDataValidated Then
'                        '    If (Not Me.IgnoreMissingLanes(intCompControl)) Then
'                        '        If Not doesLaneExist(oFields) Then
'                        '            'Save record to No Lanes
'                        '            saveNoLaneData(oRow, oTheseDetails)
'                        '            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
'                        '            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
'                        '            GoTo GetNextHeaderRecord
'                        '        End If
'                        '    End If
'                        'End If
'                        'If blnDataValidated AndAlso (Not Me.IgnoreMissingLanes(intCompControl) OrElse Not doesLaneExist(oFields)) Then
'                        If blnDataValidated AndAlso (Not Me.IgnoreMissingLanes(intCompControl)) AndAlso (Not doesLaneExist(oFields)) Then
'                            'Save record to No Lanes
'                            saveNoLaneData(oRow, oTheseDetails)
'                            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
'                            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
'                            TotalRecords += 1
'                            GoTo GetNextHeaderRecord
'                        End If
'                        'test if the record already exists.
'                        If blnDataValidated Then blnDataValidated = doesRecordExist(oFields, _
'                                                                                            strErrorMessage, _
'                                                                                            blnInsertRecord, _
'                                                                                            "Order number " & oFields("PONumber").Value, _
'                                                                                            "POHDR")
'                        If Not blnDataValidated Then
'                            addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
'                            RecordErrors += 1
'                        Else
'                            'add the order number to the list for silent tendering
'                            If mstrOrderNumbers Is Nothing Then mstrOrderNumbers = New List(Of String)
'                            mstrOrderNumbers.Add(stripQuotes(oFields("PONumber").Value))
'                            'Save the changes to the main table
'                            If saveData(oFields, blnInsertRecord, "POHDR", "POHDRCreateUser", "POHDRCreateDate") Then
'                                'run the update defaults procedure
'                                If Not updatePOHDRDefaults(oFields) Then GoTo GetNextHeaderRecord
'                                TotalRecords += 1
'                                mdblHashTotalOrders += Val(stripQuotes(oFields("PONumber").Value))
'                                'Delete all existing item data
'                                deleteItemData(oFields)
'                                'Process Item Details but only if no missing items have been found for the entire batch
'                                If Not mblnSomeItemsMissing Then
'                                    Log("Importing " & oItemList.Count & " PO Item Records For Order Number" & oFields("PONumber").Value & ".")
'                                    For Each oItems As clsImportFields In oItemList
'                                        'We always do an insert because all previous detaisl were deleted
'                                        If saveData(oItems, True, "POItem", "CreatedUser", "CreatedDate") Then
'                                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
'                                            'if it is true we wipe out the properties below and start over
'                                            TotalItems += 1
'                                            mintTotalQty += Val(oItems("QtyOrdered").Value)
'                                            mdblTotalWeight += Val(oItems("Weight").Value)
'                                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
'                                        End If
'                                    Next
'                                End If
'                                'If we get here we save the order history data
'                                saveOrderHistory(oRow, oTheseDetails)
'                            End If
'                        End If
'GetNextHeaderRecord:
'                    Next
'                    Return True
'                Catch ex As Exception
'                    If intRetryCt > Me.Retry Then
'                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, attempted to import PO Header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
'                    Else
'                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
'                    End If
'                End Try
'                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
'            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function

'    Private Function importHeaderRecords( _
'                ByRef oOrders As List(Of clsBookHeaderObject60), _
'                ByRef oDetails As List(Of clsBookDetailObject60), _
'                ByRef oFields As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            If oOrders Is Nothing OrElse oOrders.Count < 1 Then
'                If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty<br />" & vbCrLf
'                Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty")
'                Return False
'            End If
'            Dim intRetryCt As Integer = 0
'            Dim strSource As String = "clsBook.importHeaderRecords"
'            Dim blnDataValidated As Boolean = False
'            Dim strErrorMessage As String = ""
'            Dim blnInsertRecord As Boolean = True
'            Dim intCompNumber As Integer = 0
'            Dim intCompControl As Integer = 0
'            Dim blnNoMatchingItem As Boolean = True

'            Do
'                intRetryCt += 1
'                RecordErrors = 0
'                TotalRecords = 0
'                TotalItems = 0
'                Try
'                    Log("Importing " & oOrders.Count & " PO Header Records.")
'                    For Each oRow As clsBookHeaderObject60 In oOrders
'                        'Reset the data types and values to defaults for the following fields 
'                        'at the top of each loop to handle alpha vs numeric data changes
'                        oFields("PODefaultCustomer").DataType = clsImportField.DataTypeID.gcvdtString
'                        oFields("PODefaultCustomer").Length = 50
'                        oFields("PODefaultCustomer").Null = True
'                        strErrorMessage = ""
'                        blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
'                        'Get the item details for this order (must be called before we lookup the company alpha number so numbers still match)
'                        Dim oTheseDetails As List(Of clsBookDetailObject60) = getItemDetails(oFields, oDetails)
'                        If oTheseDetails Is Nothing OrElse oTheseDetails.Count < 1 Then
'                            blnNoMatchingItem = True
'                            'the modual level property determines if we insert all items 
'                            'after the headers.  if any header is missing an item then
'                            'we re-import all items 
'                            mblnSomeItemsMissing = True
'                        Else
'                            blnNoMatchingItem = False
'                        End If
'                        'Check for alpha company compatibility (note the company field type will be changed to an integer on success)
'                        If blnDataValidated Then blnDataValidated = validateCompany(oFields("PODefaultCustomer"), _
'                                                                                            strErrorMessage, _
'                                                                                            oCX, _
'                                                                                            strSource)

'                        'Create a new item list object
'                        Dim oItemList As New List(Of clsImportFields)

'                        If blnDataValidated Then
'                            'Lookup the Company Control Number if needed; but only if there are no validation errors
'                            intCompControl = oCX.getControlByNumber(oFields("PODefaultCustomer").Value)
'                            If intCompControl = 0 Then
'                                intCompControl = Me.lookupCompControlByNumber(oFields("PODefaultCustomer").Value)
'                                'now add the comp control to the collection this save future trips to the database
'                                'by sending nothing as the alpha code any existing alpha code does not get replaced
'                                oCX.AddNew(Nothing, oFields("PODefaultCustomer").Value, intCompControl)
'                            End If
'                            'Fill the Item Detail List
'                            If Not blnNoMatchingItem Then
'                                If Not filItemRecordList(oTheseDetails, oItemList) Then
'                                    'we cannot read the item so set the module level items missing propery to true
'                                    mblnSomeItemsMissing = True
'                                End If
'                            End If
'                        End If
'                        'Process the NO Lane Logic
'                        'If blnDataValidated Then
'                        '    If (Not Me.IgnoreMissingLanes(intCompControl)) Then
'                        '        If Not doesLaneExist(oFields) Then
'                        '            'Save record to No Lanes
'                        '            saveNoLaneData(oRow, oTheseDetails)
'                        '            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
'                        '            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
'                        '            GoTo GetNextHeaderRecord
'                        '        End If
'                        '    End If
'                        'End If
'                        'If blnDataValidated AndAlso (Not Me.IgnoreMissingLanes(intCompControl) OrElse Not doesLaneExist(oFields)) Then
'                        If blnDataValidated AndAlso (Not Me.IgnoreMissingLanes(intCompControl)) AndAlso (Not doesLaneExist(oFields)) Then
'                            'Save record to No Lanes
'                            saveNoLaneData(oRow, oTheseDetails)
'                            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
'                            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
'                            TotalRecords += 1
'                            GoTo GetNextHeaderRecord
'                        End If
'                        'test if the record already exists.
'                        If blnDataValidated Then blnDataValidated = doesRecordExist(oFields, _
'                                                                                            strErrorMessage, _
'                                                                                            blnInsertRecord, _
'                                                                                            "Order number " & oFields("PONumber").Value, _
'                                                                                            "POHDR")
'                        If Not blnDataValidated Then
'                            addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
'                            RecordErrors += 1
'                        Else
'                            'add the order number to the list for silent tendering
'                            If mstrOrderNumbers Is Nothing Then mstrOrderNumbers = New List(Of String)
'                            mstrOrderNumbers.Add(stripQuotes(oFields("PONumber").Value))
'                            'Save the changes to the main table
'                            If saveData(oFields, blnInsertRecord, "POHDR", "POHDRCreateUser", "POHDRCreateDate") Then
'                                'run the update defaults procedure
'                                If Not updatePOHDRDefaults(oFields) Then GoTo GetNextHeaderRecord
'                                TotalRecords += 1
'                                mdblHashTotalOrders += Val(stripQuotes(oFields("PONumber").Value))
'                                'Delete all existing item data
'                                deleteItemData(oFields)
'                                'Process Item Details but only if no missing items have been found for the entire batch
'                                If Not mblnSomeItemsMissing Then
'                                    Log("Importing " & oItemList.Count & " PO Item Records For Order Number" & oFields("PONumber").Value & ".")
'                                    For Each oItems As clsImportFields In oItemList
'                                        'We always do an insert because all previous detaisl were deleted
'                                        If saveData(oItems, True, "POItem", "CreatedUser", "CreatedDate") Then
'                                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
'                                            'if it is true we wipe out the properties below and start over
'                                            TotalItems += 1
'                                            mintTotalQty += Val(oItems("QtyOrdered").Value)
'                                            mdblTotalWeight += Val(oItems("Weight").Value)
'                                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
'                                        End If
'                                    Next
'                                End If
'                                'If we get here we save the order history data
'                                saveOrderHistory(oRow, oTheseDetails)
'                            End If
'                        End If
'GetNextHeaderRecord:
'                    Next
'                    Return True
'                Catch ex As Exception
'                    If intRetryCt > Me.Retry Then
'                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, attempted to import PO Header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
'                    Else
'                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
'                    End If
'                End Try
'                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
'            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function



'    Private Function importHeaderRecords( _
'                ByRef oOrders As List(Of clsBookHeaderObject70), _
'                ByRef oDetails As List(Of clsBookDetailObject70), _
'                ByRef oFields As clsImportFields) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            If oOrders Is Nothing OrElse oOrders.Count < 1 Then
'                If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty<br />" & vbCrLf
'                Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords failed to process PO Header records because the list is empty")
'                Return False
'            End If
'            Dim intRetryCt As Integer = 0
'            Dim strSource As String = "clsBook.importHeaderRecords"
'            Dim blnDataValidated As Boolean = False
'            Dim strErrorMessage As String = ""
'            Dim blnInsertRecord As Boolean = True
'            Dim intCompNumber As Integer = 0
'            Dim intCompControl As Integer = 0
'            Dim blnNoMatchingItem As Boolean = True

'            Do
'                intRetryCt += 1
'                RecordErrors = 0
'                TotalRecords = 0
'                TotalItems = 0
'                Try
'                    Log("Importing " & oOrders.Count & " PO Header Records.")
'                    For Each oRow As clsBookHeaderObject70 In oOrders
'                        'Reset the data types and values to defaults for the following fields 
'                        'at the top of each loop to handle alpha vs numeric data changes
'                        oFields("PODefaultCustomer").DataType = clsImportField.DataTypeID.gcvdtString
'                        oFields("PODefaultCustomer").Length = 50
'                        oFields("PODefaultCustomer").Null = True
'                        strErrorMessage = ""
'                        blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
'                        'Get the item details for this order (must be called before we lookup the company alpha number so numbers still match)
'                        Dim oTheseDetails As List(Of clsBookDetailObject70) = getItemDetails(oFields, oDetails)
'                        If oTheseDetails Is Nothing OrElse oTheseDetails.Count < 1 Then
'                            blnNoMatchingItem = True
'                            'the modual level property determines if we insert all items 
'                            'after the headers.  if any header is missing an item then
'                            'we re-import all items 
'                            mblnSomeItemsMissing = True
'                        Else
'                            blnNoMatchingItem = False
'                        End If
'                        'Check for alpha company compatibility (note the company field type will be changed to an integer on success)
'                        If blnDataValidated Then blnDataValidated = validateCompany70(oFields("PODefaultCustomer"), _
'                                                                                            intCompControl, _
'                                                                                            strErrorMessage, _
'                                                                                            oCX, _
'                                                                                            strSource, _
'                                                                                            DTran.stripQuotes(oFields("POCompLegalEntity").Value), _
'                                                                                            DTran.stripQuotes(oFields("POCompAlphaCode").Value))

'                        'Create a new item list object
'                        Dim oItemList As New List(Of clsImportFields)

'                        If blnDataValidated Then
'                            'Fill the Item Detail List
'                            If Not blnNoMatchingItem Then
'                                If Not filItemRecordList(oTheseDetails, oItemList) Then
'                                    'we cannot read the item so set the module level items missing propery to true
'                                    mblnSomeItemsMissing = True
'                                End If
'                            End If
'                        End If
'                        'Process the NO Lane Logic
'                        'If blnDataValidated Then
'                        '    If (Not Me.IgnoreMissingLanes(intCompControl)) Then
'                        '        If Not doesLaneExist(oFields) Then
'                        '            'Save record to No Lanes
'                        '            saveNoLaneData(oRow, oTheseDetails)
'                        '            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
'                        '            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
'                        '            GoTo GetNextHeaderRecord
'                        '        End If
'                        '    End If
'                        'End If
'                        'If blnDataValidated AndAlso (Not Me.IgnoreMissingLanes(intCompControl) OrElse Not doesLaneExist(oFields)) Then
'                        If blnDataValidated AndAlso (Not Me.IgnoreMissingLanes(intCompControl)) AndAlso (Not doesLaneExist(oFields)) Then
'                            'Save record to No Lanes
'                            saveNoLaneData(oRow, oTheseDetails)
'                            ITNoLaneEmailMsg &= "<br />No lane was found for order number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & " <br />" & vbCrLf & vbCrLf
'                            Log("NGL.FreightMaster.Integration.clsBook.ProcessData No Lane Found For Order Number " & oFields("PONumber").Value & " for company " & oFields("PODefaultCustomer").Value & " using lane number " & oFields("POvendor").Value & "!")
'                            TotalRecords += 1
'                            GoTo GetNextHeaderRecord
'                        End If
'                        'test if the record already exists.
'                        If blnDataValidated Then blnDataValidated = doesRecordExist(oFields, _
'                                                                                            strErrorMessage, _
'                                                                                            blnInsertRecord, _
'                                                                                            "Order number " & oFields("PONumber").Value, _
'                                                                                            "POHDR")
'                        If Not blnDataValidated Then
'                            addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
'                            RecordErrors += 1
'                        Else
'                            'add the order number to the list for silent tendering
'                            If mstrOrderNumbers Is Nothing Then mstrOrderNumbers = New List(Of String)
'                            mstrOrderNumbers.Add(stripQuotes(oFields("PONumber").Value))
'                            'Save the changes to the main table
'                            If saveData(oFields, blnInsertRecord, "POHDR", "POHDRCreateUser", "POHDRCreateDate") Then
'                                'run the update defaults procedure
'                                If Not updatePOHDRDefaults(oFields) Then GoTo GetNextHeaderRecord
'                                TotalRecords += 1
'                                mdblHashTotalOrders += Val(stripQuotes(oFields("PONumber").Value))
'                                'Delete all existing item data
'                                deleteItemData(oFields)
'                                'Process Item Details but only if no missing items have been found for the entire batch
'                                If Not mblnSomeItemsMissing Then
'                                    Log("Importing " & oItemList.Count & " PO Item Records For Order Number" & oFields("PONumber").Value & ".")
'                                    For Each oItems As clsImportFields In oItemList
'                                        'We always do an insert because all previous detaisl were deleted
'                                        If saveData(oItems, True, "POItem", "CreatedUser", "CreatedDate") Then
'                                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
'                                            'if it is true we wipe out the properties below and start over
'                                            TotalItems += 1
'                                            mintTotalQty += Val(oItems("QtyOrdered").Value)
'                                            mdblTotalWeight += Val(oItems("Weight").Value)
'                                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
'                                        End If
'                                    Next
'                                End If
'                                'If we get here we save the order history data
'                                saveOrderHistory(oRow, oTheseDetails)
'                            End If
'                        End If
'GetNextHeaderRecord:
'                    Next
'                    Return True
'                Catch ex As Exception
'                    If intRetryCt > Me.Retry Then
'                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, attempted to import PO Header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
'                    Else
'                        Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
'                    End If
'                End Try
'                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
'            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.importHeaderRecords Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function


'    Private Function importItemRecords( _
'        ByRef oDetails As BookData.BookDetailDataTable) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            Dim strSource As String = "clsBook.importItemRecords"
'            Dim blnDataValidated As Boolean = False
'            Dim strErrorMessage As String = ""
'            Dim blnInsertRecord As Boolean = True
'            Log("Importing " & oDetails.Count & " PO Detail Records.")
'            Dim oItemList As New List(Of clsImportFields)
'            filItemRecordList(oDetails, oItemList)
'            If oItemList.Count < 1 Then
'                'this should not happen so log the issue and send an email
'                ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.<br />" & vbCrLf
'                Log("NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.!")
'            Else
'                'loop through each record and save the data
'                For Each oItems As clsImportFields In oItemList
'                    If Not doesRecordExist(oItems, _
'                                            strErrorMessage, _
'                                            blnInsertRecord, _
'                                            "PO Item number " & oItems("ItemNumber").Value, _
'                                            "POItem") Then
'                        'log the error
'                        addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
'                        ItemErrors += 1
'                    Else
'                        'save the data
'                        If saveData(oItems, blnInsertRecord, "POItem", "CreatedUser", "CreatedDate") Then
'                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
'                            'if it is true we wipe out the properties below and start over
'                            TotalItems += 1
'                            mintTotalQty += Val(oItems("QtyOrdered").Value)
'                            mdblTotalWeight += Val(oItems("Weight").Value)
'                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
'                        End If
'                    End If
'                Next
'            End If
'            Return True

'        Catch ex As Exception
'            Me.ItemErrors += 1
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importItemRecords; could not import PO Detail records.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.importItemRecords Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function

'    Private Function importItemRecords( _
'        ByRef oDetails As List(Of clsBookDetailObject60)) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            Dim strSource As String = "clsBook.importItemRecords"
'            Dim blnDataValidated As Boolean = False
'            Dim strErrorMessage As String = ""
'            Dim blnInsertRecord As Boolean = True
'            Log("Importing " & oDetails.Count & " PO Detail Records.")
'            Dim oItemList As New List(Of clsImportFields)
'            filItemRecordList(oDetails, oItemList)
'            If oItemList.Count < 1 Then
'                'this should not happen so log the issue and send an email
'                ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.<br />" & vbCrLf
'                Log("NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.!")
'            Else
'                'loop through each record and save the data
'                For Each oItems As clsImportFields In oItemList
'                    If Not doesRecordExist(oItems, _
'                                            strErrorMessage, _
'                                            blnInsertRecord, _
'                                            "PO Item number " & oItems("ItemNumber").Value, _
'                                            "POItem") Then
'                        'log the error
'                        addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
'                        ItemErrors += 1
'                    Else
'                        'save the data
'                        If saveData(oItems, blnInsertRecord, "POItem", "CreatedUser", "CreatedDate") Then
'                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
'                            'if it is true we wipe out the properties below and start over
'                            TotalItems += 1
'                            mintTotalQty += Val(oItems("QtyOrdered").Value)
'                            mdblTotalWeight += Val(oItems("Weight").Value)
'                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
'                        End If
'                    End If
'                Next
'            End If
'            Return True

'        Catch ex As Exception
'            Me.ItemErrors += 1
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importItemRecords; could not import PO Detail records.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.importItemRecords Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function



'    Private Function importItemRecords( _
'        ByRef oDetails As List(Of clsBookDetailObject70)) As Boolean
'        Dim Ret As Boolean = False
'        Try
'            Dim strSource As String = "clsBook.importItemRecords"
'            Dim blnDataValidated As Boolean = False
'            Dim strErrorMessage As String = ""
'            Dim blnInsertRecord As Boolean = True
'            Log("Importing " & oDetails.Count & " PO Detail Records.")
'            Dim oItemList As New List(Of clsImportFields)
'            filItemRecordList(oDetails, oItemList)
'            If oItemList.Count < 1 Then
'                'this should not happen so log the issue and send an email
'                ITEmailMsg &= "<br />" & Source & " Warning: NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.<br />" & vbCrLf
'                Log("NGL.FreightMaster.Integration.clsBook.importItemRecords: could not Process any item records.!")
'            Else
'                'loop through each record and save the data
'                For Each oItems As clsImportFields In oItemList
'                    If Not doesRecordExist(oItems, _
'                                            strErrorMessage, _
'                                            blnInsertRecord, _
'                                            "PO Item number " & oItems("ItemNumber").Value, _
'                                            "POItem") Then
'                        'log the error
'                        addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
'                        ItemErrors += 1
'                    Else
'                        'save the data
'                        If saveData(oItems, blnInsertRecord, "POItem", "CreatedUser", "CreatedDate") Then
'                            'NOTE: after the header is imported we check the mblnSomeItemsMissing flag
'                            'if it is true we wipe out the properties below and start over
'                            TotalItems += 1
'                            mintTotalQty += Val(oItems("QtyOrdered").Value)
'                            mdblTotalWeight += Val(oItems("Weight").Value)
'                            mdblHashTotalDetails += Val(stripQuotes(oItems("ItemNumber").Value))
'                        End If
'                    End If
'                Next
'            End If
'            Return True

'        Catch ex As Exception
'            Me.ItemErrors += 1
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.importItemRecords; could not import PO Detail records.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.importItemRecords Failed!" & readExceptionMessage(ex))
'        End Try
'        Return Ret

'    End Function


'    Private Function filItemRecordList( _
'        ByRef oDetails As BookData.BookDetailDataTable, _
'        ByRef oItemList As List(Of clsImportFields)) As Boolean

'        Dim strSource As String = "clsBook.filItemRecordList"
'        Dim blnDataValidated As Boolean = False
'        Dim strErrorMessage As String = ""
'        Log("Building item list for  " & oDetails.Count & " PO Detail records.")
'        For Each oRow As BookData.BookDetailRow In oDetails
'            Dim oItems As New clsImportFields
'            If Not buildItemCollection(oItems) Then Return False
'            'Check if a customernumber is provided.  If not we change the logic to ignore the customernumber
'            If IsDBNull(oRow.Item("CustomerNumber")) Then
'                oItems("CustomerNumber").Null = True
'                oItems("CustomerNumber").PK = clsImportField.PKValue.gcNK
'            End If
'            strErrorMessage = ""
'            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
'            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
'            If blnDataValidated AndAlso Not IsDBNull(oRow.Item("CustomerNumber")) Then blnDataValidated = validateCompany(oItems("CustomerNumber"), _
'                                                                                strErrorMessage, _
'                                                                                oCX, _
'                                                                                strSource)
'            If Not blnDataValidated Then
'                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
'                ItemErrors += 1
'            Else
'                'add the item to the collection
'                oItemList.Add(oItems)
'            End If
'        Next
'        Return True


'    End Function

'    Private Function filItemRecordList( _
'        ByRef oDetails As List(Of clsBookDetailObject60), _
'        ByRef oItemList As List(Of clsImportFields)) As Boolean

'        Dim strSource As String = "clsBook.filItemRecordList"
'        Dim blnDataValidated As Boolean = False
'        Dim strErrorMessage As String = ""
'        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
'            Log("No item list records are available.")
'            Return False
'        End If
'        If oItemList Is Nothing Then oItemList = New List(Of clsImportFields)
'        Log("Building item list for  " & oDetails.Count & " PO Detail records.")
'        For Each oRow As clsBookDetailObject60 In oDetails
'            Dim oItems As New clsImportFields
'            If Not buildItemCollection60(oItems) Then Return False
'            'Check if a customernumber is provided.  If not we change the logic to ignore the customernumber
'            If IsDBNull(oRow.Item("CustomerNumber")) Then
'                oItems("CustomerNumber").Null = True
'                oItems("CustomerNumber").PK = clsImportField.PKValue.gcNK
'            End If
'            strErrorMessage = ""
'            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
'            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
'            If blnDataValidated AndAlso Not IsDBNull(oRow.Item("CustomerNumber")) Then blnDataValidated = validateCompany(oItems("CustomerNumber"), _
'                                                                                strErrorMessage, _
'                                                                                oCX, _
'                                                                                strSource)
'            If Not blnDataValidated Then
'                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
'                ItemErrors += 1
'            Else
'                'add the item to the collection
'                oItemList.Add(oItems)
'            End If
'        Next
'        Return True


'    End Function



'    Private Function filItemRecordList( _
'        ByRef oDetails As List(Of clsBookDetailObject70), _
'        ByRef oItemList As List(Of clsImportFields)) As Boolean

'        Dim strSource As String = "clsBook.filItemRecordList"
'        Dim blnDataValidated As Boolean = False
'        Dim strErrorMessage As String = ""
'        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
'            Log("No item list records are available.")
'            Return False
'        End If
'        If oItemList Is Nothing Then oItemList = New List(Of clsImportFields)
'        Log("Building item list for  " & oDetails.Count & " PO Detail records.")
'        For Each oRow As clsBookDetailObject70 In oDetails
'            Dim oItems As New clsImportFields
'            Dim intCompControl As Integer = 0
'            If Not buildItemCollection70(oItems) Then Return False

'            'Check if a customernumber is provided.  If not we change the logic to ignore the customernumber
'            If IsDBNull(oRow.Item("CustomerNumber")) Then
'                oItems("CustomerNumber").Null = True
'                oItems("CustomerNumber").PK = clsImportField.PKValue.gcNK
'            End If
'            strErrorMessage = ""
'            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
'            'Check for alpha company compatibility (note the company field type will be changed to an integer on success)
'            If blnDataValidated Then
'                If validateCompany70(oItems("CustomerNumber"), _
'                                     intCompControl, _
'                                     strErrorMessage, _
'                                     oCX, _
'                                     strSource, _
'                                     DTran.stripQuotes(oItems("POItemCompLegalEntity").Value), _
'                                     DTran.stripQuotes(oItems("POItemCompAlphaCode").Value)) Then
'                    oItems("CustomerNumber").PK = clsImportField.PKValue.gcPK
'                Else
'                    blnDataValidated = False
'                End If

'            End If


'            If Not blnDataValidated Then
'                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
'                ItemErrors += 1
'            Else
'                'add the item to the collection
'                oItemList.Add(oItems)
'            End If
'        Next
'        Return True


'    End Function



'    Private Sub saveNoLaneData(ByRef oHeaderRow As BookData.BookHeaderRow, ByRef oDetails As BookData.BookDetailDataTable)
'        If oHeaderRow Is Nothing Then Return
'        Dim intNewPOHNoLaneID As Integer
'        Dim strNewPOHNoLaneID As String
'        Dim strOrderNumber As String = DTran.NZ(oHeaderRow, "PONumber", "")
'        Dim strSQL As String = "Exec dbo.spAddPOHNoLane " _
'            & "'" & Me.AuthorizationCode & "'" _
'            & DTran.buildSQLString(oHeaderRow, "POCustomerPO", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POvendor", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POdate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POShipdate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POBuyer", "", ",") _
'            & "," & DTran.NZ(oHeaderRow, "POFrt", 0) _
'            & ",'" & CreateUser & "'" _
'            & ",'" & CreatedDate & "'" _
'            & ",'" & CreateUser & "'" _
'            & "," & DTran.NZ(oHeaderRow, "POTotalFrt", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POTotalCost", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POWgt", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POCube", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POQty", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POLines", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POConfirm", 0) _
'            & "," & DTran.NZ(oHeaderRow, "PODefaultCustomer", 0) _
'            & ",''" _
'            & "," & DTran.NZ(oHeaderRow, "PODefaultCarrier", 0) _
'            & DTran.buildSQLString(oHeaderRow, "POReqDate", "", ",") _
'            & ",'" & strOrderNumber & "'" _
'            & DTran.buildSQLString(oHeaderRow, "POShipInstructions", "", ",") _
'            & "," & DTran.NZ(oHeaderRow, "POCooler", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POFrozen", 0) _
'            & "," & DTran.NZ(oHeaderRow, "PODry", 0) _
'            & DTran.buildSQLString(oHeaderRow, "POTemp", "", ",") _
'            & ",'','','','','','','','',''" _
'            & DTran.buildSQLString(oHeaderRow, "POCarType", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POShipVia", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POShipViaType", "", ",") _
'            & "," & DTran.NZ(oHeaderRow, "POPallets", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POOtherCosts", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POStatusFlag", 0) _
'            & ",0,'',0" _
'            & "," & DTran.NZ(oHeaderRow, "POOrderSequence", 0) _
'            & DTran.buildSQLString(oHeaderRow, "POChepGLID", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POCarrierEquipmentCodes", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POCarrierTypeCode", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POPalletPositions", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POSchedulePUDate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POSchedulePUTime", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POScheduleDelDate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POSCheduleDelTime", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POActPUDate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POActPUTime", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POActDelDate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POActDelTime", "", ",")
'        strNewPOHNoLaneID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData")
'        If Not Integer.TryParse(strNewPOHNoLaneID, intNewPOHNoLaneID) Then
'            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save no lane PO Header records without success for order number " _
'                & strOrderNumber _
'                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
'                & vbCrLf & strSQL & "<hr />" & vbCrLf
'            Return 'We cannot continue   
'        End If
'        Dim sItemHistoryCommands As New List(Of String)
'        Dim sItemPONumbers As New List(Of String)
'        Dim sItemNumbers As New List(Of String)
'        If intNewPOHNoLaneID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Rows.Count > 0 Then
'            'Loop through each item detail record for this order and build an add query            
'            For Each oRow As BookData.BookDetailRow In oDetails
'                strSQL = "Exec dbo.spAddPOINoLane " _
'                    & intNewPOHNoLaneID _
'                    & ",'" & Me.AuthorizationCode & "'" _
'                    & DTran.buildSQLString(oRow, "ItemPONumber", "", ",") _
'                    & "," & DTran.NZ(oRow, "FixOffInvAllow", 0) _
'                    & "," & DTran.NZ(oRow, "FixFrtAllow", 0) _
'                    & DTran.buildSQLString(oRow, "ItemNumber", "", ",") _
'                    & "," & DTran.NZ(oRow, "QtyOrdered", 0) _
'                    & "," & DTran.NZ(oRow, "FreightCost", 0) _
'                    & "," & DTran.NZ(oRow, "ItemCost", 0) _
'                    & "," & DTran.NZ(oRow, "Weight", 0) _
'                    & "," & DTran.NZ(oRow, "Cube", 0) _
'                    & "," & DTran.NZ(oRow, "Pack", 0) _
'                    & DTran.buildSQLString(oRow, "Size", "", ",") _
'                    & DTran.buildSQLString(oRow, "Description", "", ",") _
'                    & DTran.buildSQLString(oRow, "Hazmat", "", ",") _
'                    & ",'" & CreateUser & "'" _
'                    & ",'" & CreatedDate & "'" _
'                    & DTran.buildSQLString(oRow, "Brand", "", ",") _
'                    & DTran.buildSQLString(oRow, "CostCenter", "", ",") _
'                    & DTran.buildSQLString(oRow, "LotNumber", "", ",") _
'                    & DTran.buildSQLString(oRow, "LotExpirationDate", "", ",") _
'                    & DTran.buildSQLString(oRow, "GTIN", "", ",") _
'                    & DTran.buildSQLString(oRow, "CustItemNumber", "", ",") _
'                    & "," & DTran.NZ(oRow, "CustomerNumber", 0) _
'                    & "," & DTran.NZ(oRow, "POOrderSequence", 0) _
'                    & DTran.buildSQLString(oRow, "PalletType", "", ",")
'                sItemHistoryCommands.Add(strSQL)
'                If Not sItemPONumbers.Contains(DTran.NZ(oRow, "ItemPONumber", "")) Then sItemPONumbers.Add(DTran.NZ(oRow, "ItemPONumber", ""))
'                If Not sItemNumbers.Contains(DTran.NZ(oRow, "ItemNumber", "")) Then sItemNumbers.Add(DTran.NZ(oRow, "ItemNumber", ""))
'            Next
'            'Build the delete query
'            strSQL = "Delete From dbo.POINoLanes Where POIPOHNLControl = " & intNewPOHNoLaneID
'            Dim blnUseOr As Boolean = False
'            Dim sSpacer As String = ""
'            If sItemPONumbers.Count > 0 Then
'                blnUseOr = True
'                strSQL &= " AND (ItemPONumber NOT IN ("
'                For Each s As String In sItemPONumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                strSQL &= ") "
'            End If
'            If sItemNumbers.Count > 0 Then
'                If blnUseOr Then
'                    strSQL &= " OR ItemNumber NOT IN ("
'                Else
'                    strSQL &= " AND ItemNumber NOT IN ("
'                End If
'                sSpacer = ""
'                For Each s As String In sItemNumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                If blnUseOr Then
'                    strSQL &= "))"
'                Else
'                    strSQL &= ")"
'                End If
'            ElseIf blnUseOr Then
'                strSQL &= ")"
'            End If
'            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
'                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to delete existing POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
'            End If
'            'Now execute each add item commands
'            For Each s As String In sItemHistoryCommands
'                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
'                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save new POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
'                Else
'                    TotalItems += 1
'                End If
'            Next
'        End If

'    End Sub

'    Private Sub saveNoLaneData(ByRef oHeaderRow As clsBookHeaderObject60, ByRef oDetails As List(Of clsBookDetailObject60))
'        If oHeaderRow Is Nothing Then Return
'        Dim intNewPOHNoLaneID As Integer
'        Dim strNewPOHNoLaneID As String
'        Dim strOrderNumber As String = oHeaderRow.PONumber
'        Dim strSQL As String = "Exec dbo.spAddPOHNoLane " _
'            & "'" & Me.AuthorizationCode & "'" _
'            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
'            & "," & oHeaderRow.POFrt _
'            & ",'" & CreateUser & "'" _
'            & ",'" & CreatedDate & "'" _
'            & ",'" & CreateUser & "'" _
'            & "," & oHeaderRow.POTotalFrt _
'            & "," & oHeaderRow.POTotalCost _
'            & "," & oHeaderRow.POWgt _
'            & "," & oHeaderRow.POCube _
'            & "," & oHeaderRow.POQty _
'            & "," & oHeaderRow.POLines _
'            & "," & If(oHeaderRow.POConfirm, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
'            & ",''" _
'            & "," & oHeaderRow.PODefaultCarrier _
'            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
'            & ",'" & strOrderNumber & "'" _
'            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
'            & "," & If(oHeaderRow.POCooler, "1", "0") _
'            & "," & If(oHeaderRow.POFrozen, "1", "0") _
'            & "," & If(oHeaderRow.PODry, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
'            & ",'','','','','','','','',''" _
'            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
'            & "," & oHeaderRow.POPallets _
'            & "," & oHeaderRow.POOtherCosts _
'            & "," & oHeaderRow.POStatusFlag _
'            & ",0,'',0" _
'            & "," & oHeaderRow.POOrderSequence _
'            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
'            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
'            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
'            & "," & If(oHeaderRow.POInbound, "1", "0") _
'            & "," & oHeaderRow.PODefaultRouteSequence _
'            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")


'        strNewPOHNoLaneID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData")
'        If Not Integer.TryParse(strNewPOHNoLaneID, intNewPOHNoLaneID) Then
'            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save no lane PO Header records without success for order number " _
'                & strOrderNumber _
'                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
'                & vbCrLf & strSQL & "<hr />" & vbCrLf
'            Return 'We cannot continue   
'        End If
'        Dim sItemHistoryCommands As New List(Of String)
'        Dim sItemPONumbers As New List(Of String)
'        Dim sItemNumbers As New List(Of String)
'        If intNewPOHNoLaneID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
'            'Loop through each item detail record for this order and build an add query            
'            For Each oRow As clsBookDetailObject60 In oDetails
'                strSQL = "Exec dbo.spAddPOINoLane " _
'                    & intNewPOHNoLaneID _
'                    & ",'" & Me.AuthorizationCode & "'" _
'                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
'                    & "," & oRow.FixOffInvAllow _
'                    & "," & oRow.FixFrtAllow _
'                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
'                    & "," & oRow.QtyOrdered _
'                    & "," & oRow.FreightCost _
'                    & "," & oRow.ItemCost _
'                    & "," & oRow.Weight _
'                    & "," & oRow.Cube _
'                    & "," & oRow.Pack _
'                    & DTran.buildSQLString(oRow.Size, 255, ",") _
'                    & DTran.buildSQLString(oRow.Description, 255, ",") _
'                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
'                    & ",'" & CreateUser & "'" _
'                    & ",'" & CreatedDate & "'" _
'                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
'                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
'                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
'                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
'                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
'                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
'                    & "," & oRow.CustomerNumber _
'                    & "," & oRow.POOrderSequence _
'                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
'                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
'                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
'                    & "," & oRow.POItemPallets _
'                    & "," & oRow.POItemTies _
'                    & "," & oRow.POItemHighs _
'                    & "," & oRow.POItemQtyPalletPercentage _
'                    & "," & oRow.POItemQtyLength _
'                    & "," & oRow.POItemQtyWidth _
'                    & "," & oRow.POItemQtyHeight _
'                    & "," & If(oRow.POItemStackable, "1", "0") _
'                    & "," & oRow.POItemLevelOfDensity

'                sItemHistoryCommands.Add(strSQL)
'                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
'                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
'            Next
'            'Build the delete query
'            strSQL = "Delete From dbo.POINoLanes Where POIPOHNLControl = " & intNewPOHNoLaneID
'            Dim blnUseOr As Boolean = False
'            Dim sSpacer As String = ""
'            If sItemPONumbers.Count > 0 Then
'                blnUseOr = True
'                strSQL &= " AND (ItemPONumber NOT IN ("
'                For Each s As String In sItemPONumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                strSQL &= ") "
'            End If
'            If sItemNumbers.Count > 0 Then
'                If blnUseOr Then
'                    strSQL &= " OR ItemNumber NOT IN ("
'                Else
'                    strSQL &= " AND ItemNumber NOT IN ("
'                End If
'                sSpacer = ""
'                For Each s As String In sItemNumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                If blnUseOr Then
'                    strSQL &= "))"
'                Else
'                    strSQL &= ")"
'                End If
'            ElseIf blnUseOr Then
'                strSQL &= ")"
'            End If
'            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
'                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to delete existing POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
'            End If
'            'Now execute each add item commands
'            For Each s As String In sItemHistoryCommands
'                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
'                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save new POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
'                Else
'                    TotalItems += 1
'                End If
'            Next
'        End If

'    End Sub



'    Private Sub saveNoLaneData(ByRef oHeaderRow As clsBookHeaderObject70, ByRef oDetails As List(Of clsBookDetailObject70))
'        If oHeaderRow Is Nothing Then Return
'        Dim intNewPOHNoLaneID As Integer
'        Dim strNewPOHNoLaneID As String
'        Dim strOrderNumber As String = oHeaderRow.PONumber
'        Dim strSQL As String = "Exec dbo.spAddPOHNoLane " _
'            & "'" & Me.AuthorizationCode & "'" _
'            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
'            & "," & oHeaderRow.POFrt _
'            & ",'" & CreateUser & "'" _
'            & ",'" & CreatedDate & "'" _
'            & ",'" & CreateUser & "'" _
'            & "," & oHeaderRow.POTotalFrt _
'            & "," & oHeaderRow.POTotalCost _
'            & "," & oHeaderRow.POWgt _
'            & "," & oHeaderRow.POCube _
'            & "," & oHeaderRow.POQty _
'            & "," & oHeaderRow.POLines _
'            & "," & If(oHeaderRow.POConfirm, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
'            & ",''" _
'            & "," & oHeaderRow.PODefaultCarrier _
'            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
'            & ",'" & strOrderNumber & "'" _
'            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
'            & "," & If(oHeaderRow.POCooler, "1", "0") _
'            & "," & If(oHeaderRow.POFrozen, "1", "0") _
'            & "," & If(oHeaderRow.PODry, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
'            & ",'','','','','','','','',''" _
'            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
'            & "," & oHeaderRow.POPallets _
'            & "," & oHeaderRow.POOtherCosts _
'            & "," & oHeaderRow.POStatusFlag _
'            & ",0,'',0" _
'            & "," & oHeaderRow.POOrderSequence _
'            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
'            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
'            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
'            & "," & If(oHeaderRow.POInbound, "1", "0") _
'            & "," & oHeaderRow.PODefaultRouteSequence _
'            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")


'        strNewPOHNoLaneID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData")
'        If Not Integer.TryParse(strNewPOHNoLaneID, intNewPOHNoLaneID) Then
'            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save no lane PO Header records without success for order number " _
'                & strOrderNumber _
'                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
'                & vbCrLf & strSQL & "<hr />" & vbCrLf
'            Return 'We cannot continue   
'        End If
'        Dim sItemHistoryCommands As New List(Of String)
'        Dim sItemPONumbers As New List(Of String)
'        Dim sItemNumbers As New List(Of String)
'        If intNewPOHNoLaneID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
'            'Loop through each item detail record for this order and build an add query            
'            For Each oRow As clsBookDetailObject60 In oDetails
'                strSQL = "Exec dbo.spAddPOINoLane " _
'                    & intNewPOHNoLaneID _
'                    & ",'" & Me.AuthorizationCode & "'" _
'                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
'                    & "," & oRow.FixOffInvAllow _
'                    & "," & oRow.FixFrtAllow _
'                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
'                    & "," & oRow.QtyOrdered _
'                    & "," & oRow.FreightCost _
'                    & "," & oRow.ItemCost _
'                    & "," & oRow.Weight _
'                    & "," & oRow.Cube _
'                    & "," & oRow.Pack _
'                    & DTran.buildSQLString(oRow.Size, 255, ",") _
'                    & DTran.buildSQLString(oRow.Description, 255, ",") _
'                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
'                    & ",'" & CreateUser & "'" _
'                    & ",'" & CreatedDate & "'" _
'                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
'                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
'                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
'                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
'                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
'                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
'                    & "," & oRow.CustomerNumber _
'                    & "," & oRow.POOrderSequence _
'                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
'                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
'                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
'                    & "," & oRow.POItemPallets _
'                    & "," & oRow.POItemTies _
'                    & "," & oRow.POItemHighs _
'                    & "," & oRow.POItemQtyPalletPercentage _
'                    & "," & oRow.POItemQtyLength _
'                    & "," & oRow.POItemQtyWidth _
'                    & "," & oRow.POItemQtyHeight _
'                    & "," & If(oRow.POItemStackable, "1", "0") _
'                    & "," & oRow.POItemLevelOfDensity

'                sItemHistoryCommands.Add(strSQL)
'                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
'                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
'            Next
'            'Build the delete query
'            strSQL = "Delete From dbo.POINoLanes Where POIPOHNLControl = " & intNewPOHNoLaneID
'            Dim blnUseOr As Boolean = False
'            Dim sSpacer As String = ""
'            If sItemPONumbers.Count > 0 Then
'                blnUseOr = True
'                strSQL &= " AND (ItemPONumber NOT IN ("
'                For Each s As String In sItemPONumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                strSQL &= ") "
'            End If
'            If sItemNumbers.Count > 0 Then
'                If blnUseOr Then
'                    strSQL &= " OR ItemNumber NOT IN ("
'                Else
'                    strSQL &= " AND ItemNumber NOT IN ("
'                End If
'                sSpacer = ""
'                For Each s As String In sItemNumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                If blnUseOr Then
'                    strSQL &= "))"
'                Else
'                    strSQL &= ")"
'                End If
'            ElseIf blnUseOr Then
'                strSQL &= ")"
'            End If
'            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
'                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to delete existing POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
'            End If
'            'Now execute each add item commands
'            For Each s As String In sItemHistoryCommands
'                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveNoLaneData", False) Then
'                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveNoLaneData, attempted to save new POINoLanes records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
'                Else
'                    TotalItems += 1
'                End If
'            Next
'        End If

'    End Sub


'    Private Function doesLaneExist(ByRef oFields As clsImportFields) As Boolean
'        Dim blnRet As Boolean = False
'        If oFields Is Nothing Then Return False
'        'Check the list in memory
'        Dim intLaneControl As Integer = oEL.getControlByNumber(oFields("POvendor").Value)
'        If intLaneControl = 0 Then
'            Dim strSQL As String = "Select top 1 LaneControl From dbo.Lane Where LaneNumber = " & oFields("POvendor").Value
'            Dim strLaneControl As String = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.doesLaneExist")
'            If Integer.TryParse(strLaneControl, intLaneControl) Then
'                If intLaneControl > 0 Then
'                    'save the lane control to the collection in memory for future use to reduce trips to the database
'                    oEL.AddNew(oFields("POvendor").Value, intLaneControl)
'                    blnRet = True
'                End If
'            End If
'        Else
'            blnRet = True
'        End If
'        If blnRet Then
'            Try
'                deleteExistingNOLaneRecords(oFields)
'            Catch ex As Exception
'                'ignore any errors when cleaning up old records 
'            End Try
'        End If
'        Return blnRet

'    End Function

'    Private Function IgnoreMissingLanes(ByVal CompControl As Integer) As Boolean
'        Dim blnRet As Boolean = True
'        Dim dblRetVal As Double = Me.getParValue("DataIntegrationPOWaitForLanes", CompControl)
'        'the DataIntegrationPOWaitForLanes parameter tells us how long to wait.  A notification 
'        'alert informs the users of what orders are outstanding.
'        If dblRetVal > 0 Then blnRet = False
'        Return blnRet
'    End Function

'    'Public Sub testSilentTender(ByVal compControls As List(Of Integer))
'    '    Try
'    '        With SharedServices 'Me.PaneSettings.MainInterface.SharedServices
'    '            If Not .UserConnected Then
'    '                .LogOn(WCFDataProperties)
'    '            End If
'    '        End With
'    '        mintImportedCompControls = compControls
'    '        silentTenderLoadsExecAsync()
'    '    Catch ex As Exception
'    '        LogException("Test Silent Tender Failure", "Could not process the requested data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.testSilentTender")
'    '    Finally
'    '        Try
'    '            SharedServices.LogOff(WCFDataProperties)
'    '        Catch ex As Exception

'    '        End Try
'    '    End Try
'    'End Sub

'    ' ''' <summary>
'    ' ''' this method is no longer beng used the code remains for historical reference it may be deleted after version 7.0
'    ' ''' </summary>
'    ' ''' <returns></returns>
'    ' ''' <remarks></remarks>
'    'Private Function silentTenderLoads() As Boolean
'    '    Dim blnRet As Boolean = False
'    '    'Dim oCon As New System.Data.SqlClient.SqlConnection
'    '    Dim strMSG As String = ""
'    '    Dim strEmailError As String = ""
'    '    Dim strBookTranCodeFilter As String = ""
'    '    Dim strSource = "NGL.FreightMaster.Integration.clsBook.SilentTenderLoads"
'    '    Try
'    '        'get a list of loads to be processed
'    '        'Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 Order By CompControl"
'    '        ' Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)
'    '        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
'    '        Dim dblVal As Double = 0
'    '        'Dim strAllowSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")
'    '        Dim strAllowSilent As String = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")

'    '        Double.TryParse(strAllowSilent, dblVal)
'    '        If dblVal <> 1 Then
'    '            'Log("DEBUG: Silent Tendering is Off!")
'    '            'Silent Tendering is off so return false
'    '            Return False
'    '        End If
'    '        Dim strSilentTenderEDIPCLoads = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderEDIPCLoads'")
'    '        Dim blnSilentTenderEDIPCLoads As Boolean = True
'    '        Double.TryParse(strSilentTenderEDIPCLoads, dblVal)
'    '        If dblVal <> 1 Then
'    '            blnSilentTenderEDIPCLoads = False
'    '        End If
'    '        Dim strSilentTenderPCLoads = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderPCLoads'")
'    '        Double.TryParse(strSilentTenderPCLoads, dblVal)
'    '        If dblVal <> 1 Then
'    '            'Silent Tender of PC Loads is turned off check the silent tender of EDI PC Loads setting
'    '            If blnSilentTenderEDIPCLoads Then
'    '                'we only silent tender PC loads for EDI
'    '                strBookTranCodeFilter = " AND ((isnull(dbo.Book.BookTranCode,'N') <> 'PC') OR Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204') and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204'))) "
'    '            Else
'    '                'we cannot tender any PC loads
'    '                strBookTranCodeFilter = " AND (isnull(dbo.Book.BookTranCode,'N') <> 'PC') "
'    '            End If
'    '        ElseIf Not blnSilentTenderEDIPCLoads Then
'    '            'we do not silent tender EDI PC loads other PC loads are ok to silent tender
'    '            strBookTranCodeFilter = " AND ((isnull(dbo.Book.BookTranCode,'N') <> 'PC') OR ((isnull(dbo.Book.BookTranCode,'N') = 'PC') AND Not Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204') and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204')))) "
'    '        End If




'    '        Dim strDeleteLoadsOnSilent As String = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalDeleteLoadsOnSilentTendering'")
'    '        Dim blnDeleteLoads As Boolean = False
'    '        Double.TryParse(strDeleteLoadsOnSilent, dblVal)
'    '        If dblVal = 1 Then

'    '            'Log("DEBUG: Delete Loads Is True!")
'    '            'Delete Loads is on
'    '            blnDeleteLoads = True
'    '        End If
'    '        Dim intSilentTenderDelay As Integer = CInt(oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderingDelay'"))
'    '        Dim strInClause As String = ""
'    '        Dim strInSeperator As String = ""
'    '        If Not mintImportedCompControls Is Nothing AndAlso mintImportedCompControls.Count > 0 Then
'    '            strInClause = " AND dbo.Comp.CompControl in ("
'    '            For Each c In mintImportedCompControls
'    '                strInClause &= strInSeperator & c.ToString
'    '                strInSeperator = ", "
'    '            Next
'    '            strInClause &= ")"
'    '        End If
'    '        Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl "
'    '        'this code is used to determine if the order is using carrier EDI data.
'    '        'strSQL &= "CAST(CASE WHEN Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204')and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204'))  THEN 1 ELSE 0 End AS BIT)  as UsingEDI"

'    '        strSQL &= " FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber left outer join dbo.Book on dbo.pohdr.POHDRPRONumber = dbo.Book.BookProNumber "
'    '        strSQL &= " Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 "
'    '        strSQL &= strBookTranCodeFilter
'    '        strSQL &= strInClause
'    '        strSQL &= " Order By CompControl"
'    '        Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
'    '        If Not oQR.Exception Is Nothing Then
'    '            ITEmailMsg &= "<br />Read Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of an unexpected error while reading the POHDR data table.<br />" & vbCrLf & readExceptionMessage(oQR.Exception) & "<br />" & vbCrLf
'    '            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Read Silent Tender Loads Data Warning!" & readExceptionMessage(oQR.Exception))
'    '            Return False
'    '        End If
'    '        Dim dt As System.Data.DataTable = oQR.Data
'    '        Dim blnLoadsProcessedForCompany As Boolean = False
'    '        Dim intLastComp As Integer = 0
'    '        Dim intFinalizedForComp As Integer = 0
'    '        Dim strTenderedLoads As New List(Of String)
'    '        Dim intDeletedForComp As Integer = 0
'    '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

'    '            'Log("DEBUG: Processing " & dt.Rows.Count.ToString & " Rows!")
'    '            Dim strOrderNumber As String = ""
'    '            Dim strBookProNumber As String = ""
'    '            Dim strModVerify As String = ""
'    '            Dim strVendorNumber As String = ""
'    '            Dim intOrderSequence As Integer = 0
'    '            Dim intDefCompNumber As Integer = 0
'    '            Dim intCompControl As Integer = 0
'    '            Dim intRecord = 0
'    '            Dim blnSendAsBatch As Boolean = True
'    '            Dim intTotal = dt.Rows.Count
'    '            For Each oRow As System.Data.DataRow In dt.Rows
'    '                intRecord += 1
'    '                If intRecord >= intTotal Then blnSendAsBatch = False
'    '                'Get the next Comp Control Number
'    '                intCompControl = 0
'    '                Integer.TryParse(DTran.getDataRowString(oRow, "CompControl", "0"), intCompControl)
'    '                'Check if the company has changed.
'    '                If intCompControl <> intLastComp Then
'    '                    If intLastComp > 0 And blnLoadsProcessedForCompany Then
'    '                        'send email
'    '                        Dim strEmail As String = ""
'    '                        Try
'    '                            strEmail = oQuery.getScalarValue(DBCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
'    '                        Catch ex As Exception
'    '                            ITEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '                            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
'    '                        End Try
'    '                        If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
'    '                            strEmail = AdminEmail
'    '                        End If
'    '                        Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
'    '                        strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
'    '                        For Each s In strTenderedLoads
'    '                            strBody &= s & vbCrLf
'    '                        Next
'    '                        SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
'    '                        'Log("DEBUG: Email Generated: " & strBody)
'    '                        'Clear the message string list
'    '                        strTenderedLoads = New List(Of String)
'    '                        'Reset the number of loads finalized
'    '                        intFinalizedForComp = 0
'    '                        'Reset the loads processed flag to false
'    '                        blnLoadsProcessedForCompany = False
'    '                    End If
'    '                    'Reset the last comp control
'    '                    intLastComp = intCompControl
'    '                End If
'    '                'Get the current data for this row
'    '                strOrderNumber = DTran.getDataRowString(oRow, "POHDROrderNumber", "")
'    '                strBookProNumber = DTran.getDataRowString(oRow, "POHDRPRONumber", "")
'    '                strModVerify = DTran.getDataRowString(oRow, "POHDRModVerify", "")
'    '                strVendorNumber = DTran.getDataRowString(oRow, "POHDRvendor", "")
'    '                intOrderSequence = 0
'    '                Integer.TryParse(DTran.getDataRowString(oRow, "POHDROrderSequence", "0"), intOrderSequence)
'    '                intDefCompNumber = 0
'    '                Integer.TryParse(DTran.getDataRowString(oRow, "POHDRDefaultCustomer", "0"), intDefCompNumber)
'    '                'Check the Mod Verify setting and process the data as needed
'    '                Dim strErrMsg As String = ""
'    '                Dim blnErrTenderingLoad As Boolean = False
'    '                Dim blnSkipLoad As Boolean = False
'    '                Dim faultExceptionMessage As NGL.FMWCFProxy.FaultExceptionEventArgs
'    '                'Log("DEBUG: Mod Verify Value = " & strModVerify & " for  Order Number " & strOrderNumber)
'    '                Try
'    '                    Select Case strModVerify
'    '                        Case "No Pro"
'    '                            'Log("DEBUG: RunWriteNewBookingWithData")
'    '                            'Old code removed we now call the wcf proxy via ImportPOHdr
'    '                            'blnErrTenderingLoad = Not runWriteNewBookingWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
'    '                            blnErrTenderingLoad = Not ImportPOHdr(strModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, strSource, strErrMsg, blnSendAsBatch)
'    '                        Case "FINALIZED"
'    '                            'Log("DEBUG: RunProcessFinalizedData")
'    '                            blnErrTenderingLoad = Not runProcessFinalizedData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
'    '                        Case "DELETED"
'    '                            'Log("DEBUG: runRemoveDeletedWithData")
'    '                            blnErrTenderingLoad = Not runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
'    '                            'We do not need to notify the users because this order was already deleted it only existed in the staging table.
'    '                            blnSkipLoad = True
'    '                        Case "DELETE-B"
'    '                            If blnDeleteLoads Then
'    '                                'Log("DEBUG: runDeleteOrderWithData")
'    '                                blnErrTenderingLoad = Not runDeleteOrderWithData(strBookProNumber, strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
'    '                            Else
'    '                                blnSkipLoad = True
'    '                            End If
'    '                        Case "DELETE-F"
'    '                            'Log("DEBUG: Skip DELETE-F")
'    '                            blnSkipLoad = True
'    '                        Case "NO LANE"
'    '                            'Log("DEBUG: Skip NO LANE")
'    '                            blnSkipLoad = True
'    '                        Case "NEW TRAN-F"
'    '                            'Log("DEBUG: Skip NEW TRAN-F")
'    '                            blnSkipLoad = True
'    '                        Case "NEW TRAN"
'    '                            'Log("DEBUG: Skip NEW TRAN")
'    '                            blnSkipLoad = True
'    '                        Case "NEW COMP"
'    '                            'Log("DEBUG: Skip NEW COMP")
'    '                            blnSkipLoad = True
'    '                        Case Else
'    '                            'Log("DEBUG: Default runUpdatePOModificationWithDatad")
'    '                            'Old code removed we now call the wcf proxy via ImportPOHdr
'    '                            'blnErrTenderingLoad = Not runUpdatePOModificationWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
'    '                            blnErrTenderingLoad = Not ImportPOHdr(strModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, strSource, strErrMsg, blnSendAsBatch)
'    '                    End Select

'    '                    If Not blnErrTenderingLoad Then
'    '                        If Not blnSkipLoad Then
'    '                            blnLoadsProcessedForCompany = True
'    '                            strTenderedLoads.Add("<p>Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & "</p>")
'    '                            System.Threading.Thread.Sleep(200)
'    '                        End If
'    '                    Else
'    '                        'Process Error Message and continue
'    '                        GroupEmailMsg &= "<br />Silent Tender Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & strErrMsg & "<br />" & vbCrLf
'    '                        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & strErrMsg)
'    '                    End If

'    '                Catch ex As Exception
'    '                    'Process Error Message and continue
'    '                    GroupEmailMsg &= "<br />Silent Tender Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & ex.Message & "<br />" & vbCrLf
'    '                    Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & ex.Message)

'    '                End Try
'    '                If intSilentTenderDelay > 0 Then
'    '                    If Debug Then Log("Start Silent Tender Delay: " & Date.Now.ToString)
'    '                    System.Threading.Thread.Sleep(intSilentTenderDelay)
'    '                    If Debug Then Log("End Silent Tender Delay: " & Date.Now.ToString)
'    '                End If
'    '            Next
'    '            'Finally process the last company data
'    '            If intLastComp > 0 And blnLoadsProcessedForCompany Then
'    '                'send email
'    '                Dim strEmail As String = ""
'    '                Try
'    '                    strEmail = oQuery.getScalarValue(DBCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
'    '                Catch ex As Exception
'    '                    ITEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '                    Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
'    '                End Try
'    '                If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
'    '                    strEmail = AdminEmail
'    '                End If
'    '                Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
'    '                strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
'    '                For Each s In strTenderedLoads
'    '                    strBody &= s & vbCrLf
'    '                Next
'    '                SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
'    '                'Log("DEBUG: Email Generated: " & strBody)
'    '            End If
'    '            blnRet = True
'    '        End If
'    '    Catch ex As Ngl.Core.DatabaseRetryExceededException
'    '        ITEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a retry exceeded failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))
'    '        Return False
'    '    Catch ex As Ngl.Core.DatabaseLogInException
'    '        ITEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a database login failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))
'    '        Return False
'    '    Catch ex As Ngl.Core.DatabaseInvalidException
'    '        ITEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a database access failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))
'    '        Return False
'    '    Catch ex As Ngl.Core.DatabaseDataValidationException
'    '        ITEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of an unexpected data validation failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))
'    '        Return False
'    '    Catch ex As Exception
'    '        Throw
'    '        Return False
'    '    End Try

'    '    Return blnRet
'    'End Function

'    ''TODO: make private again after testing
'    ''private Sub silentTenderLoadsExecAsync()
'    'Public Sub silentTenderLoadsExecAsync()

'    '    Dim strMSG As String = ""
'    '    Dim strEmailError As String = ""
'    '    Dim strBookTranCodeFilter As String = ""
'    '    Dim strSource = "NGL.FreightMaster.Integration.clsBook.SilentTenderLoads"
'    '    Dim strEmailMsg As String = ""
'    '    Dim oCon As System.Data.SqlClient.SqlConnection
'    '    Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
'    '    Try
'    '        'With SharedServices 'Me.PaneSettings.MainInterface.SharedServices
'    '        '    If Not .UserConnected Then
'    '        '        .LogOn(WCFDataProperties)
'    '        '    End If
'    '        'End With
'    '        'get a list of loads to be processed
'    '        'Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 Order By CompControl"
'    '        ' Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)

'    '        Dim dblVal As Double = 0
'    '        'Because this method runs Async we must create a seperate DB connection and also close it in the Finally statement
'    '        oCon = getNewConnection(False)
'    '        'Dim strAllowSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")
'    '        Dim strAllowSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")

'    '        Double.TryParse(strAllowSilent, dblVal)
'    '        If dblVal <> 1 Then
'    '            'Log("DEBUG: Silent Tendering is Off!")
'    '            'Silent Tendering is off so return false
'    '            Return
'    '        End If
'    '        Dim strSilentTenderEDIPCLoads = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderEDIPCLoads'")
'    '        Dim blnSilentTenderEDIPCLoads As Boolean = True
'    '        Double.TryParse(strSilentTenderEDIPCLoads, dblVal)
'    '        If dblVal <> 1 Then
'    '            blnSilentTenderEDIPCLoads = False
'    '        End If
'    '        Dim strSilentTenderPCLoads = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderPCLoads'")
'    '        Double.TryParse(strSilentTenderPCLoads, dblVal)
'    '        If dblVal <> 1 Then
'    '            'Silent Tender of PC Loads is turned off check the silent tender of EDI PC Loads setting
'    '            If blnSilentTenderEDIPCLoads Then
'    '                'we only silent tender PC loads for EDI
'    '                strBookTranCodeFilter = " AND ((isnull(dbo.Book.BookTranCode,'N') <> 'PC') OR Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204') and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204'))) "
'    '            Else
'    '                'we cannot tender any PC loads
'    '                strBookTranCodeFilter = " AND (isnull(dbo.Book.BookTranCode,'N') <> 'PC') "
'    '            End If
'    '        ElseIf Not blnSilentTenderEDIPCLoads Then
'    '            'we do not silent tender EDI PC loads other PC loads are ok to silent tender
'    '            strBookTranCodeFilter = " AND ((isnull(dbo.Book.BookTranCode,'N') <> 'PC') OR ((isnull(dbo.Book.BookTranCode,'N') = 'PC') AND Not Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204') and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204')))) "
'    '        End If




'    '        Dim strDeleteLoadsOnSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalDeleteLoadsOnSilentTendering'")
'    '        Dim blnDeleteLoads As Boolean = False
'    '        Double.TryParse(strDeleteLoadsOnSilent, dblVal)
'    '        If dblVal = 1 Then

'    '            'Log("DEBUG: Delete Loads Is True!")
'    '            'Delete Loads is on
'    '            blnDeleteLoads = True
'    '        End If
'    '        Dim intSilentTenderDelay As Integer = CInt(oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderingDelay'"))
'    '        Dim strInClause As String = ""
'    '        Dim strInSeperator As String = ""
'    '        If Not mintImportedCompControls Is Nothing AndAlso mintImportedCompControls.Count > 0 Then
'    '            strInClause = " AND dbo.Comp.CompControl in ("
'    '            For Each c In mintImportedCompControls
'    '                strInClause &= strInSeperator & c.ToString
'    '                strInSeperator = ", "
'    '            Next
'    '            strInClause &= ")"
'    '        End If
'    '        Dim strOrderInClause As String = ""
'    '        If Not mstrOrderNumbers Is Nothing AndAlso mstrOrderNumbers.Count > 0 Then
'    '            strInSeperator = ""
'    '            strOrderInClause = " AND POHDROrderNumber in ("
'    '            For Each o In mstrOrderNumbers
'    '                strOrderInClause &= String.Format("{0}'{1}'", strInSeperator, o)
'    '                strInSeperator = ", "
'    '            Next
'    '            strOrderInClause &= ")"
'    '        End If
'    '        Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl "
'    '        'this code is used to determine if the order is using carrier EDI data.
'    '        'strSQL &= "CAST(CASE WHEN Exists(Select * From dbo.CompEDI as cEdi,dbo.carrierEDI as carEdi Where (cEdi.CompEDICompControl = isnull(dbo.Comp.CompControl,0) and cEdi.CompEDIXaction = '204')and	(carEdi.carrierEDIcarrierControl = isnull(dbo.Book.BookCarrierControl,0) and carEdi.carrierEDIXaction = '204'))  THEN 1 ELSE 0 End AS BIT)  as UsingEDI"

'    '        strSQL &= " FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber left outer join dbo.Book on dbo.pohdr.POHDRPRONumber = dbo.Book.BookProNumber "
'    '        strSQL &= " Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 "
'    '        strSQL &= strBookTranCodeFilter
'    '        strSQL &= strInClause
'    '        strSQL &= strOrderInClause
'    '        strSQL &= " Order By CompControl"
'    '        Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
'    '        If Not oQR.Exception Is Nothing Then
'    '            strEmailMsg &= "<br />Read Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of an unexpected error while reading the POHDR data table.<br />" & vbCrLf & readExceptionMessage(oQR.Exception) & "<br />" & vbCrLf
'    '            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Read Silent Tender Loads Data Warning!" & readExceptionMessage(oQR.Exception))
'    '            Return
'    '        End If
'    '        Dim dt As System.Data.DataTable = oQR.Data
'    '        Dim blnLoadsProcessedForCompany As Boolean = False
'    '        Dim intLastComp As Integer = 0
'    '        Dim intFinalizedForComp As Integer = 0
'    '        Dim strTenderedLoads As New List(Of String)
'    '        Dim intDeletedForComp As Integer = 0
'    '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

'    '            'Log("DEBUG: Processing " & dt.Rows.Count.ToString & " Rows!")
'    '            Dim strOrderNumber As String = ""
'    '            Dim strBookProNumber As String = ""
'    '            Dim strModVerify As String = ""
'    '            Dim strVendorNumber As String = ""
'    '            Dim intOrderSequence As Integer = 0
'    '            Dim intDefCompNumber As Integer = 0
'    '            Dim intCompControl As Integer = 0
'    '            Dim intRecord = 0
'    '            Dim blnSendAsBatch As Boolean = True
'    '            Dim intTotal = dt.Rows.Count
'    '            For Each oRow As System.Data.DataRow In dt.Rows
'    '                intRecord += 1
'    '                If intRecord >= intTotal Then blnSendAsBatch = False
'    '                'Get the next Comp Control Number
'    '                intCompControl = 0
'    '                Integer.TryParse(DTran.getDataRowString(oRow, "CompControl", "0"), intCompControl)
'    '                'Check if the company has changed.
'    '                If intCompControl <> intLastComp Then
'    '                    If intLastComp > 0 And blnLoadsProcessedForCompany Then
'    '                        'send email
'    '                        Dim strEmail As String = ""
'    '                        Try
'    '                            strEmail = oQuery.getScalarValue(oCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
'    '                        Catch ex As Exception
'    '                            strEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '                            Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
'    '                        End Try
'    '                        If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
'    '                            strEmail = AdminEmail
'    '                        End If
'    '                        Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
'    '                        strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
'    '                        For Each s In strTenderedLoads
'    '                            strBody &= s & vbCrLf
'    '                        Next
'    '                        SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
'    '                        'Log("DEBUG: Email Generated: " & strBody)
'    '                        'Clear the message string list
'    '                        strTenderedLoads = New List(Of String)
'    '                        'Reset the number of loads finalized
'    '                        intFinalizedForComp = 0
'    '                        'Reset the loads processed flag to false
'    '                        blnLoadsProcessedForCompany = False
'    '                    End If
'    '                    'Reset the last comp control
'    '                    intLastComp = intCompControl
'    '                End If
'    '                'Get the current data for this row
'    '                strOrderNumber = DTran.getDataRowString(oRow, "POHDROrderNumber", "")
'    '                strBookProNumber = DTran.getDataRowString(oRow, "POHDRPRONumber", "")
'    '                strModVerify = DTran.getDataRowString(oRow, "POHDRModVerify", "")
'    '                strVendorNumber = DTran.getDataRowString(oRow, "POHDRvendor", "")
'    '                intOrderSequence = 0
'    '                Integer.TryParse(DTran.getDataRowString(oRow, "POHDROrderSequence", "0"), intOrderSequence)
'    '                intDefCompNumber = 0
'    '                Integer.TryParse(DTran.getDataRowString(oRow, "POHDRDefaultCustomer", "0"), intDefCompNumber)
'    '                'Check the Mod Verify setting and process the data as needed
'    '                Dim strErrMsg As String = ""
'    '                Dim blnErrTenderingLoad As Boolean = False
'    '                Dim blnSkipLoad As Boolean = False
'    '                Dim faultExceptionMessage As NGL.FMWCFProxy.FaultExceptionEventArgs
'    '                'Log("DEBUG: Mod Verify Value = " & strModVerify & " for  Order Number " & strOrderNumber)
'    '                Try
'    '                    Select Case strModVerify
'    '                        Case "No Pro"
'    '                            'Log("DEBUG: RunWriteNewBookingWithData")
'    '                            'Old code removed we now call the wcf proxy via ImportPOHdr
'    '                            'blnErrTenderingLoad = Not runWriteNewBookingWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
'    '                            blnErrTenderingLoad = Not ImportPOHdr(strModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, strSource, strErrMsg, blnSendAsBatch)
'    '                        Case "FINALIZED"
'    '                            'Log("DEBUG: RunProcessFinalizedData")
'    '                            blnErrTenderingLoad = Not runProcessFinalizedData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
'    '                        Case "DELETED"
'    '                            'Log("DEBUG: runRemoveDeletedWithData")
'    '                            blnErrTenderingLoad = Not runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
'    '                            'We do not need to notify the users because this order was already deleted it only existed in the staging table.
'    '                            blnSkipLoad = True
'    '                        Case "DELETE-B"
'    '                            If blnDeleteLoads Then
'    '                                'Log("DEBUG: runDeleteOrderWithData")
'    '                                blnErrTenderingLoad = Not runDeleteOrderWithData(strBookProNumber, strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
'    '                            Else
'    '                                blnSkipLoad = True
'    '                            End If
'    '                        Case "DELETE-F"
'    '                            'Log("DEBUG: Skip DELETE-F")
'    '                            blnSkipLoad = True
'    '                        Case "NO LANE"
'    '                            'Log("DEBUG: Skip NO LANE")
'    '                            blnSkipLoad = True
'    '                        Case "NEW TRAN-F"
'    '                            'Log("DEBUG: Skip NEW TRAN-F")
'    '                            blnSkipLoad = True
'    '                        Case "NEW TRAN"
'    '                            'Log("DEBUG: Skip NEW TRAN")
'    '                            blnSkipLoad = True
'    '                        Case "NEW COMP"
'    '                            'Log("DEBUG: Skip NEW COMP")
'    '                            blnSkipLoad = True
'    '                        Case Else
'    '                            'Log("DEBUG: Default runUpdatePOModificationWithDatad")
'    '                            'Old code removed we now call the wcf proxy via ImportPOHdr
'    '                            'blnErrTenderingLoad = Not runUpdatePOModificationWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
'    '                            blnErrTenderingLoad = Not ImportPOHdr(strModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, strSource, strErrMsg, blnSendAsBatch)
'    '                    End Select

'    '                    If Not blnErrTenderingLoad Then
'    '                        If Not blnSkipLoad Then
'    '                            blnLoadsProcessedForCompany = True
'    '                            strTenderedLoads.Add("<p>Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & "</p>")
'    '                            System.Threading.Thread.Sleep(200)
'    '                        End If
'    '                    Else
'    '                        'Process Error Message and continue
'    '                        strEmailMsg &= "<br />Silent Tender Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & strErrMsg & "<br />" & vbCrLf
'    '                        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & strErrMsg)
'    '                    End If

'    '                Catch ex As Exception
'    '                    'Process Error Message and continue
'    '                    strEmailMsg &= "<br />Silent Tender Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ".<br />" & vbCrLf & "The actual error is:<br />" & vbCrLf & ex.Message & "<br />" & vbCrLf
'    '                    Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & ex.Message)

'    '                End Try
'    '                If intSilentTenderDelay > 0 Then
'    '                    If Debug Then Log("Start Silent Tender Delay: " & Date.Now.ToString)
'    '                    System.Threading.Thread.Sleep(intSilentTenderDelay)
'    '                    If Debug Then Log("End Silent Tender Delay: " & Date.Now.ToString)
'    '                End If
'    '            Next
'    '            'Finally process the last company data
'    '            If intLastComp > 0 And blnLoadsProcessedForCompany Then
'    '                'send email
'    '                Dim strEmail As String = ""
'    '                Try
'    '                    strEmail = oQuery.getScalarValue(oCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
'    '                Catch ex As Exception
'    '                    strEmailMsg &= "<br />Email Silent Tender Results Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  Using the admin email by default.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '                    Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
'    '                End Try
'    '                If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
'    '                    strEmail = AdminEmail
'    '                End If
'    '                Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
'    '                strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
'    '                For Each s In strTenderedLoads
'    '                    strBody &= s & vbCrLf
'    '                Next
'    '                SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
'    '                'Log("DEBUG: Email Generated: " & strBody)
'    '            End If
'    '        End If
'    '    Catch ex As NGL.Core.DatabaseRetryExceededException
'    '        strEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a retry exceeded failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

'    '    Catch ex As NGL.Core.DatabaseLogInException
'    '        strEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a database login failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

'    '    Catch ex As NGL.Core.DatabaseInvalidException
'    '        strEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of a database access failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

'    '    Catch ex As NGL.Core.DatabaseDataValidationException
'    '        strEmailMsg &= "<br />Process Silent Tender Loads Data Warning: NGL.FreightMaster.Integration.clsBook.SilentTenderLoads could not silent tender loads because of an unexpected data validation failure.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'    '        Log("NGL.FreightMaster.Integration.clsBook.SilentTenderLoads Process Silent Tender Loads Data Warning!" & readExceptionMessage(ex))

'    '    Catch ex As Exception
'    '        Log("Web Service Shared Services Log off Error: " & ex.Message)
'    '    Finally
'    '        Try
'    '            oQuery = Nothing
'    '        Catch ex As Exception

'    '        End Try


'    '        Try
'    '            If Not oCon Is Nothing Then
'    '                oCon.Close()
'    '                oCon = Nothing
'    '            End If
'    '        Catch ex As Exception

'    '        End Try
'    '        'Try
'    '        '    'If mblnSharedServiceRunning Then SharedServices.LogOff(WCFDataProperties)
'    '        'Catch ex As Exception
'    '        '    Log("Web Service Shared Services Log off Error: " & ex.Message)
'    '        'End Try
'    '        'mblnSharedServiceRunning = False
'    '        If strEmailMsg.Trim.Length > 0 Then
'    '            LogError("Process Silent Tender Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & strEmailMsg, GroupEmail)
'    '        End If
'    '    End Try

'    'End Sub

'    Private Sub deleteExistingNOLaneRecords(ByRef ofields As clsImportFields)

'        If ofields Is Nothing Then Return
'        Dim strSQL As String = "DELETE FROM [dbo].[POHNoLanes]"
'        If Me.AuthorizationCode.Trim.Length > 0 Then
'            strSQL &= "WHERE" _
'                & " POHNLAUTHCode = '" & Me.AuthorizationCode & "'" _
'                & " AND" _
'                & " POHNLOrderNumber = " & ofields("PONumber").Value _
'                & " AND" _
'                & " POHNLOrderSequence = " & ofields("POOrderSequence").Value _
'                & " AND" _
'                & " POHNLDefaultCustomer = " & ofields("PODefaultCustomer").Value _
'                & " AND" _
'                & " POHNLnumber = " & ofields("POCustomerPO").Value
'        Else
'            strSQL &= "WHERE" _
'                & " POHNLOrderNumber = " & ofields("PONumber").Value _
'                & " AND" _
'                & " POHNLOrderSequence = " & ofields("POOrderSequence").Value _
'                & " AND" _
'                & " POHNLDefaultCustomer = " & ofields("PODefaultCustomer").Value _
'                & " AND" _
'                & " POHNLnumber = " & ofields("POCustomerPO").Value
'        End If
'        executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.deleteExistingNOLaneRecords")


'    End Sub

'    Private Function getItemDetails(ByRef oFields As clsImportFields, ByRef oDetails As BookData.BookDetailDataTable) As BookData.BookDetailDataTable
'        Dim strFilter As String = "ItemPONumber = " & oFields("PONumber").Value & " AND POOrderSequence = " & oFields("POOrderSequence").Value & " AND CustomerNumber = " & oFields("PODefaultCustomer").Value

'        Dim oTable As New BookData.BookDetailDataTable
'        Dim oDRows As BookData.BookDetailRow() = oDetails.Select(strFilter)
'        If oDRows.Count < 1 Then
'            'No items were found so log the filter we only send an email if debugging is on
'            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter:<br />" & vbCrLf & strFilter & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter: " & strFilter)
'        Else
'            For Each orow As BookData.BookDetailRow In oDRows
'                Dim newRow As BookData.BookDetailRow = oTable.NewBookDetailRow
'                For i As Integer = 0 To oTable.Columns.Count - 1
'                    newRow.Item(i) = orow.Item(i)
'                Next
'                oTable.AddBookDetailRow(newRow)
'                oTable.AcceptChanges()
'            Next
'        End If
'        Return oTable
'    End Function

'    Private Function getItemDetails(ByVal oFields As clsImportFields, ByRef oDetails As List(Of clsBookDetailObject60)) As List(Of clsBookDetailObject60)
'        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
'            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty.")
'            Return New List(Of clsBookDetailObject60)
'        End If
'        Dim strFilter As String = "ItemPONumber = " & oFields("PONumber").Value & " AND POOrderSequence = " & oFields("POOrderSequence").Value & " AND CustomerNumber = " & oFields("PODefaultCustomer").Value
'        Dim oDets As List(Of clsBookDetailObject60) = (From d In oDetails _
'                                                       Where _
'                                                       d.ItemPONumber = DTran.stripQuotes(oFields("PONumber").Value) _
'                                                       And d.POOrderSequence = oFields("POOrderSequence").Value _
'                                                       And d.CustomerNumber = DTran.stripQuotes(oFields("PODefaultCustomer").Value) _
'                                                       Select d).ToList

'        If oDets Is Nothing OrElse oDets.Count < 1 Then
'            'No items were found so log the filter we only send an email if debugging is on
'            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter:<br />" & vbCrLf & strFilter & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter: " & strFilter)
'            oDets = New List(Of clsBookDetailObject60)
'        End If
'        Return oDets
'    End Function

'    Private Function getItemDetails(ByVal oFields As clsImportFields, ByRef oDetails As List(Of clsBookDetailObject70)) As List(Of clsBookDetailObject70)
'        If oDetails Is Nothing OrElse oDetails.Count < 1 Then
'            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to process item detail records because the list is empty.")
'            Return New List(Of clsBookDetailObject70)
'        End If
'        Dim PONumber = DTran.stripQuotes(oFields("PONumber").Value)
'        Dim POSeq = oFields("POOrderSequence").Value
'        Dim CustNbr = DTran.stripQuotes(oFields("PODefaultCustomer").Value)
'        Dim LegalEntity = DTran.stripQuotes(oFields("POCompLegalEntity").Value)
'        Dim AlphaCode = DTran.stripQuotes(oFields("POCompAlphaCode").Value)
'        Dim oDets As List(Of clsBookDetailObject70)
'        Dim strFilter As String = ""
'        If String.IsNullOrWhiteSpace(CustNbr) OrElse Val(CustNbr) = 0 Then
'            'try to use leagal entity
'            strFilter = String.Format("ItemPONumber = '{0}' AND POOrderSequence = {1}  AND POItemCompLegalEntity = '{2}' AND POItemCompAlphaCode = '{3}'", PONumber, POSeq, LegalEntity, AlphaCode)
'            oDets = (From d In oDetails _
'                    Where _
'                    d.ItemPONumber = PONumber _
'                    And d.POOrderSequence = POSeq _
'                    And d.POItemCompLegalEntity = LegalEntity _
'                    And d.POItemCompAlphaCode = AlphaCode _
'                    Select d).ToList
'        Else
'            strFilter = String.Format("ItemPONumber = '{0}' AND POOrderSequence = {1}  AND CustomerNumber = '{2}'", PONumber, POSeq, CustNbr)
'            oDets = (From d In oDetails _
'                    Where _
'                    d.ItemPONumber = PONumber _
'                    And d.POOrderSequence = POSeq _
'                    And d.CustomerNumber = CustNbr _
'                    Select d).ToList
'        End If

'        If oDets Is Nothing OrElse oDets.Count < 1 Then            'No items were found so log the filter we only send an email if debugging is on
'            If Me.Debug Then ITEmailMsg &= "<br />" & Source & " Debug Message: NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter:<br />" & vbCrLf & strFilter & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.getItemDetails failed to find any records using the following filter: " & strFilter)
'            oDets = New List(Of clsBookDetailObject70)
'        End If
'        Return oDets
'    End Function

'    Private Function updatePOHDRDefaults(ByRef oFields As clsImportFields) As Boolean
'        Dim blnRetVal As Boolean = False

'        If oFields Is Nothing Then Return False
'        Dim oCon As System.Data.SqlClient.SqlConnection
'        Dim oCmd As New SqlCommand
'        Try
'            Dim intRetryCt As Integer = 0

'            oCon = getNewConnection(False)
'            Do
'                intRetryCt += 1

'                Try

'                    Dim lngErrNumber As Long
'                    Dim strRetVal As String = ""
'                    With oCmd
'                        .Connection = oCon
'                        .CommandTimeout = 3600
'                        .Parameters.Add("@OrderNumber", SqlDbType.NVarChar, 20)
'                        .Parameters("@OrderNumber").Value = stripQuotes(oFields("PONumber").Value)
'                        .Parameters.Add("@OrderSequence", SqlDbType.Int)
'                        .Parameters("@OrderSequence").Value = stripQuotes(oFields("POOrderSequence").Value)
'                        .Parameters.Add("@CustomerNumber", SqlDbType.BigInt)
'                        .Parameters("@CustomerNumber").Value = oFields("PODefaultCustomer").Value
'                        .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 4000)
'                        .Parameters("@RetMsg").Direction = ParameterDirection.Output
'                        .Parameters.Add("@ErrNumber", SqlDbType.BigInt)
'                        .Parameters("@ErrNumber").Direction = ParameterDirection.Output
'                        .CommandText = "spUpdatePOHDRDefaults"
'                        .CommandType = CommandType.StoredProcedure
'                        .ExecuteNonQuery()
'                        strRetVal = Trim(.Parameters("@RetMsg").Value.ToString)
'                        If IsDBNull(.Parameters("@ErrNumber").Value) Then
'                            lngErrNumber = 0
'                        Else
'                            lngErrNumber = .Parameters("@ErrNumber").Value
'                        End If
'                    End With

'                    Try
'                        If lngErrNumber <> 0 Then
'                            If intRetryCt > Me.Retry Then
'                                ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults: Procedure spUpdatePOHDRDefaults output failed " & intRetryCt.ToString & " times for order number " & oFields("PONumber").Value & ".<br />" & vbCrLf & "Error # " & lngErrNumber & ": " & strRetVal & "<br />" & vbCrLf
'                                Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Failed!")
'                            Else
'                                Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Output Failure Retry = " & intRetryCt.ToString)
'                            End If
'                        Else
'                            blnRetVal = True
'                            Exit Do
'                        End If
'                    Catch ex As Exception

'                    End Try
'                Catch ex As Exception
'                    If intRetryCt > Me.Retry Then
'                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults, attempted to run spUpdatePOHDRDefaults procedure " & intRetryCt.ToString & " times without success for order number " & oFields("PONumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'                        Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Failed!" & readExceptionMessage(ex))
'                    Else
'                        Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Execution Failure Retry = " & intRetryCt.ToString)

'                    End If
'                End Try
'                'We only get here if an exception is thrown and intRetryCt <= 3
'            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.

'        Catch ex As Exception
'            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults, Procedure spUpdatePOHDRDefaults general failure for order number " & oFields("PONumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
'            Log("NGL.FreightMaster.Integration.clsBook.updatePOHDRDefaults Failed!" & readExceptionMessage(ex))
'        Finally
'            Try
'                oCmd.Cancel()
'                oCmd = Nothing
'            Catch ex As Exception

'            End Try
'            Try
'                If Not oCon Is Nothing Then
'                    oCon.Close()
'                    oCon = Nothing
'                End If
'            Catch ex As Exception

'            End Try
'        End Try
'        Return blnRetVal
'    End Function

'    Private Sub deleteItemData(ByRef oFields As clsImportFields)
'        Dim strSQL As String = "DELETE FROM POItem Where CustomerNumber = " & oFields("PODefaultCustomer").Value & " And ItemPONumber = " & oFields("PONumber").Value & " And POOrderSequence = " & oFields("POOrderSequence").Value
'        executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.deleteItemData", False)
'    End Sub

'    Private Sub saveOrderHistory(ByRef oHeaderRow As BookData.BookHeaderRow, ByRef oDetails As BookData.BookDetailDataTable)
'        If oHeaderRow Is Nothing Then Return
'        Dim intNewPOHdrHistoryID As Integer
'        Dim strNewPOHdrHistoryID As String
'        Dim strOrderNumber As String = DTran.NZ(oHeaderRow, "PONumber", "")
'        'Check that the Authorization Code is not null
'        If String.IsNullOrEmpty(Me.AuthorizationCode) Then
'            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
'                & strOrderNumber _
'                & ".<br />See the error log table (tblLog) for more details. The Authorization Code was null or empty." & vbCrLf
'            Return 'We cannot continue   
'        End If
'        Dim strSQL As String = "Exec dbo.spAddPOHdrHistory " _
'            & "'" & Me.AuthorizationCode & "'" _
'            & DTran.buildSQLString(oHeaderRow, "POCustomerPO", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POvendor", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POdate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POShipdate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POBuyer", "", ",") _
'            & "," & DTran.NZ(oHeaderRow, "POFrt", 0) _
'            & ",'" & CreateUser & "'" _
'            & ",'" & CreatedDate & "'" _
'            & ",'" & CreateUser & "'" _
'            & "," & DTran.NZ(oHeaderRow, "POTotalFrt", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POTotalCost", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POWgt", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POCube", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POQty", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POLines", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POConfirm", 0) _
'            & DTran.buildSQLString(oHeaderRow, "PODefaultCustomer", "", ",") _
'            & ",''" _
'            & "," & DTran.NZ(oHeaderRow, "PODefaultCarrier", 0) _
'            & DTran.buildSQLString(oHeaderRow, "POReqDate", "", ",") _
'            & ",'" & strOrderNumber & "'" _
'            & DTran.buildSQLString(oHeaderRow, "POShipInstructions", "", ",") _
'            & "," & DTran.NZ(oHeaderRow, "POCooler", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POFrozen", 0) _
'            & "," & DTran.NZ(oHeaderRow, "PODry", 0) _
'            & DTran.buildSQLString(oHeaderRow, "POTemp", "", ",") _
'            & ",'','','','','','','','',''" _
'            & DTran.buildSQLString(oHeaderRow, "POCarType", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POShipVia", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POShipViaType", "", ",") _
'            & "," & DTran.NZ(oHeaderRow, "POPallets", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POOtherCosts", 0) _
'            & "," & DTran.NZ(oHeaderRow, "POStatusFlag", 0) _
'            & ",0,'',0,''" _
'            & "," & DTran.NZ(oHeaderRow, "POOrderSequence", 0) _
'            & DTran.buildSQLString(oHeaderRow, "POChepGLID", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POCarrierEquipmentCodes", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POCarrierTypeCode", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POPalletPositions", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POSchedulePUDate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POSchedulePUTime", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POScheduleDelDate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POSCheduleDelTime", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POActPUDate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POActPUTime", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POActDelDate", "", ",") _
'            & DTran.buildSQLString(oHeaderRow, "POActDelTime", "", ",")
'        strNewPOHdrHistoryID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory")
'        If Not Integer.TryParse(strNewPOHdrHistoryID, intNewPOHdrHistoryID) OrElse intNewPOHdrHistoryID = 0 Then
'            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
'                & strOrderNumber _
'                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
'                & vbCrLf & strSQL & "<hr />" & vbCrLf
'            Return 'We cannot continue   
'        End If
'        Dim sItemHistoryCommands As New List(Of String)
'        Dim sItemPONumbers As New List(Of String)
'        Dim sItemNumbers As New List(Of String)
'        If intNewPOHdrHistoryID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Rows.Count > 0 Then
'            'Loop through each item detail record for this order and build an add query            
'            For Each oRow As BookData.BookDetailRow In oDetails
'                strSQL = "Exec dbo.spAddPOItemHistory " _
'                    & intNewPOHdrHistoryID _
'                    & ",'" & Me.AuthorizationCode & "'" _
'                    & DTran.buildSQLString(oRow, "ItemPONumber", "", ",") _
'                    & "," & DTran.NZ(oRow, "FixOffInvAllow", 0) _
'                    & "," & DTran.NZ(oRow, "FixFrtAllow", 0) _
'                    & DTran.buildSQLString(oRow, "ItemNumber", "", ",") _
'                    & "," & DTran.NZ(oRow, "QtyOrdered", 0) _
'                    & "," & DTran.NZ(oRow, "FreightCost", 0) _
'                    & "," & DTran.NZ(oRow, "ItemCost", 0) _
'                    & "," & DTran.NZ(oRow, "Weight", 0) _
'                    & "," & DTran.NZ(oRow, "Cube", 0) _
'                    & "," & DTran.NZ(oRow, "Pack", 0) _
'                    & DTran.buildSQLString(oRow, "Size", "", ",") _
'                    & DTran.buildSQLString(oRow, "Description", "", ",") _
'                    & DTran.buildSQLString(oRow, "Hazmat", "", ",") _
'                    & ",'" & CreateUser & "'" _
'                    & ",'" & CreatedDate & "'" _
'                    & DTran.buildSQLString(oRow, "Brand", "", ",") _
'                    & DTran.buildSQLString(oRow, "CostCenter", "", ",") _
'                    & DTran.buildSQLString(oRow, "LotNumber", "", ",") _
'                    & DTran.buildSQLString(oRow, "LotExpirationDate", "", ",") _
'                    & DTran.buildSQLString(oRow, "GTIN", "", ",") _
'                    & DTran.buildSQLString(oRow, "CustItemNumber", "", ",") _
'                    & DTran.buildSQLString(oRow, "CustomerNumber", "0", ",") _
'                    & "," & DTran.NZ(oRow, "POOrderSequence", 0) _
'                    & DTran.buildSQLString(oRow, "PalletType", "", ",")

'                'We do not add the following new 6.0 fields to the query yet maybe in the future
'                'For now we only update these fields when called by the 6.0 methods
'                '& DTran.buildSQLString(oRow, "POItemHistoryHazmatTypeCode", "", ",") _
'                '& DTran.buildSQLString(oRow, "POItemHistory49CFRCode", "", ",") _
'                '& DTran.buildSQLString(oRow, "POItemHistoryIATACode", "", ",") _
'                '& DTran.buildSQLString(oRow, "POItemHistoryDOTCode", "", ",") _
'                '& DTran.buildSQLString(oRow, "POItemHistoryMarineCode", "", ",") _
'                '& DTran.buildSQLString(oRow, "POItemHistoryNMFCClass", "", ",") _
'                '& DTran.buildSQLString(oRow, "POItemHistoryFAKClass", "", ",") _
'                '& "," & DTran.NZ(oRow, "POItemHistoryLimitedQtyFlag", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryPallets", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryTies", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryHighs", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryQtyPalletPercentage", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryQtyLength", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryQtyWidth", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryQtyHeight", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryStackable", 0) _
'                '& "," & DTran.NZ(oRow, "POItemHistoryLevelOfDensity", 0)
'                sItemHistoryCommands.Add(strSQL)
'                If Not sItemPONumbers.Contains(DTran.NZ(oRow, "ItemPONumber", "")) Then sItemPONumbers.Add(DTran.NZ(oRow, "ItemPONumber", ""))
'                If Not sItemNumbers.Contains(DTran.NZ(oRow, "ItemNumber", "")) Then sItemNumbers.Add(DTran.NZ(oRow, "ItemNumber", ""))
'            Next
'            'Build the delete query
'            strSQL = "Delete From dbo.POItemHistory Where POItemHistoryPOHdrHistoryControl = " & intNewPOHdrHistoryID
'            Dim blnUseOr As Boolean = False
'            Dim sSpacer As String = ""
'            If sItemPONumbers.Count > 0 Then
'                blnUseOr = True
'                strSQL &= " AND (ItemPONumber NOT IN ("
'                For Each s As String In sItemPONumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                strSQL &= ") "
'            End If
'            If sItemNumbers.Count > 0 Then
'                If blnUseOr Then
'                    strSQL &= " OR ItemNumber NOT IN ("
'                Else
'                    strSQL &= " AND ItemNumber NOT IN ("
'                End If
'                sSpacer = ""
'                For Each s As String In sItemNumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                If blnUseOr Then
'                    strSQL &= "))"
'                Else
'                    strSQL &= ")"
'                End If
'            ElseIf blnUseOr Then
'                strSQL &= ")"
'            End If

'            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
'                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to delete existing POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
'            End If
'            'Now execute each add item commands
'            For Each s As String In sItemHistoryCommands
'                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
'                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save new POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
'                End If
'            Next
'        End If

'    End Sub

'    Private Sub saveOrderHistory(ByRef oHeaderRow As clsBookHeaderObject60, ByRef oDetails As List(Of clsBookDetailObject60))
'        If oHeaderRow Is Nothing Then Return
'        Dim intNewPOHdrHistoryID As Integer
'        Dim strNewPOHdrHistoryID As String
'        Dim strOrderNumber As String = oHeaderRow.PONumber
'        Dim strSQL As String = "Exec dbo.spAddPOHdrHistory " _
'            & "'" & Me.AuthorizationCode & "'" _
'            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
'            & "," & oHeaderRow.POFrt _
'            & ",'" & CreateUser & "'" _
'            & ",'" & CreatedDate & "'" _
'            & ",'" & CreateUser & "'" _
'            & "," & oHeaderRow.POTotalFrt _
'            & "," & oHeaderRow.POTotalCost _
'            & "," & oHeaderRow.POWgt _
'            & "," & oHeaderRow.POCube _
'            & "," & oHeaderRow.POQty _
'            & "," & oHeaderRow.POLines _
'            & "," & If(oHeaderRow.POConfirm, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
'            & ",''" _
'            & "," & oHeaderRow.PODefaultCarrier _
'            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
'            & ",'" & strOrderNumber & "'" _
'            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
'            & "," & If(oHeaderRow.POCooler, "1", "0") _
'            & "," & If(oHeaderRow.POFrozen, "1", "0") _
'            & "," & If(oHeaderRow.PODry, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
'            & ",'','','','','','','','',''" _
'            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
'            & "," & oHeaderRow.POPallets _
'            & "," & oHeaderRow.POOtherCosts _
'            & "," & oHeaderRow.POStatusFlag _
'            & ",0,'',0,''" _
'            & "," & oHeaderRow.POOrderSequence _
'            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
'            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
'            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
'            & "," & If(oHeaderRow.POInbound, "1", "0") _
'            & "," & oHeaderRow.PODefaultRouteSequence _
'            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")

'        strNewPOHdrHistoryID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory")
'        If Not Integer.TryParse(strNewPOHdrHistoryID, intNewPOHdrHistoryID) Then
'            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
'                & strOrderNumber _
'                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
'                & vbCrLf & strSQL & "<hr />" & vbCrLf
'            Return 'We cannot continue   
'        End If
'        Dim sItemHistoryCommands As New List(Of String)
'        Dim sItemPONumbers As New List(Of String)
'        Dim sItemNumbers As New List(Of String)
'        If intNewPOHdrHistoryID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
'            'Loop through each item detail record for this order and build an add query            
'            For Each oRow As clsBookDetailObject60 In oDetails
'                strSQL = "Exec dbo.spAddPOItemHistory " _
'                    & intNewPOHdrHistoryID _
'                    & ",'" & Me.AuthorizationCode & "'" _
'                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
'                    & "," & oRow.FixOffInvAllow _
'                    & "," & oRow.FixFrtAllow _
'                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
'                    & "," & oRow.QtyOrdered _
'                    & "," & oRow.FreightCost _
'                    & "," & oRow.ItemCost _
'                    & "," & oRow.Weight _
'                    & "," & oRow.Cube _
'                    & "," & oRow.Pack _
'                    & DTran.buildSQLString(oRow.Size, 255, ",") _
'                    & DTran.buildSQLString(oRow.Description, 255, ",") _
'                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
'                    & ",'" & CreateUser & "'" _
'                    & ",'" & CreatedDate & "'" _
'                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
'                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
'                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
'                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
'                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
'                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
'                    & "," & oRow.CustomerNumber _
'                    & "," & oRow.POOrderSequence _
'                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
'                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
'                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
'                    & "," & oRow.POItemPallets _
'                    & "," & oRow.POItemTies _
'                    & "," & oRow.POItemHighs _
'                    & "," & oRow.POItemQtyPalletPercentage _
'                    & "," & oRow.POItemQtyLength _
'                    & "," & oRow.POItemQtyWidth _
'                    & "," & oRow.POItemQtyHeight _
'                    & "," & If(oRow.POItemStackable, "1", "0") _
'                    & "," & oRow.POItemLevelOfDensity


'                sItemHistoryCommands.Add(strSQL)
'                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
'                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
'            Next
'            'Build the delete query
'            strSQL = "Delete From dbo.POItemHistory Where POItemHistoryPOHdrHistoryControl = " & intNewPOHdrHistoryID
'            Dim blnUseOr As Boolean = False
'            Dim sSpacer As String = ""
'            If sItemPONumbers.Count > 0 Then
'                blnUseOr = True
'                strSQL &= " AND (ItemPONumber NOT IN ("
'                For Each s As String In sItemPONumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                strSQL &= ") "
'            End If
'            If sItemNumbers.Count > 0 Then
'                If blnUseOr Then
'                    strSQL &= " OR ItemNumber NOT IN ("
'                Else
'                    strSQL &= " AND ItemNumber NOT IN ("
'                End If
'                sSpacer = ""
'                For Each s As String In sItemNumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                If blnUseOr Then
'                    strSQL &= "))"
'                Else
'                    strSQL &= ")"
'                End If
'            ElseIf blnUseOr Then
'                strSQL &= ")"
'            End If

'            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
'                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to delete existing POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
'            End If
'            'Now execute each add item commands
'            For Each s As String In sItemHistoryCommands
'                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
'                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save new POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
'                End If
'            Next
'        End If

'    End Sub



'    Private Sub saveOrderHistory(ByRef oHeaderRow As clsBookHeaderObject70, ByRef oDetails As List(Of clsBookDetailObject70))
'        If oHeaderRow Is Nothing Then Return
'        Dim intNewPOHdrHistoryID As Integer
'        Dim strNewPOHdrHistoryID As String
'        Dim strOrderNumber As String = oHeaderRow.PONumber
'        Dim strSQL As String = "Exec dbo.spAddPOHdrHistory " _
'            & "'" & Me.AuthorizationCode & "'" _
'            & DTran.buildSQLString(oHeaderRow.POCustomerPO, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POVendor, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POdate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipdate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POBuyer, 10, ",") _
'            & "," & oHeaderRow.POFrt _
'            & ",'" & CreateUser & "'" _
'            & ",'" & CreatedDate & "'" _
'            & ",'" & CreateUser & "'" _
'            & "," & oHeaderRow.POTotalFrt _
'            & "," & oHeaderRow.POTotalCost _
'            & "," & oHeaderRow.POWgt _
'            & "," & oHeaderRow.POCube _
'            & "," & oHeaderRow.POQty _
'            & "," & oHeaderRow.POLines _
'            & "," & If(oHeaderRow.POConfirm, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.PODefaultCustomer, 50, ",") _
'            & ",''" _
'            & "," & oHeaderRow.PODefaultCarrier _
'            & DTran.buildSQLString(oHeaderRow.POReqDate, 22, ",") _
'            & ",'" & strOrderNumber & "'" _
'            & DTran.buildSQLString(oHeaderRow.POShipInstructions, 255, ",") _
'            & "," & If(oHeaderRow.POCooler, "1", "0") _
'            & "," & If(oHeaderRow.POFrozen, "1", "0") _
'            & "," & If(oHeaderRow.PODry, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.POTemp, 1, ",") _
'            & ",'','','','','','','','',''" _
'            & DTran.buildSQLString(oHeaderRow.POCarType, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipVia, 10, ",") _
'            & DTran.buildSQLString(oHeaderRow.POShipViaType, 10, ",") _
'            & "," & oHeaderRow.POPallets _
'            & "," & oHeaderRow.POOtherCosts _
'            & "," & oHeaderRow.POStatusFlag _
'            & ",0,'',0,''" _
'            & "," & oHeaderRow.POOrderSequence _
'            & DTran.buildSQLString(oHeaderRow.POChepGLID, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCarrierEquipmentCodes, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCarrierTypeCode, 20, ",") _
'            & DTran.buildSQLString(oHeaderRow.POPalletPositions, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSchedulePUDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSchedulePUTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POScheduleDelDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POSCheduleDelTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActPUDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActPUTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActDelDate, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POActDelTime, 22, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigCompNumber, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress1, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress2, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigAddress3, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigCountry, 30, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactPhone, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactPhoneExt, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POOrigContactFax, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestCompNumber, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress1, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress2, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestAddress3, 40, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestCountry, 30, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactPhone, 15, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactPhoneExt, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.PODestContactFax, 15, ",") _
'            & "," & If(oHeaderRow.POPalletExchange, "1", "0") _
'            & DTran.buildSQLString(oHeaderRow.POPalletType, 50, ",") _
'            & DTran.buildSQLString(oHeaderRow.POComments, 255, ",") _
'            & DTran.buildSQLString(oHeaderRow.POCommentsConfidential, 255, ",") _
'            & "," & If(oHeaderRow.POInbound, "1", "0") _
'            & "," & oHeaderRow.PODefaultRouteSequence _
'            & DTran.buildSQLString(oHeaderRow.PORouteGuideNumber, 50, ",")

'        strNewPOHdrHistoryID = getScalarValue(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory")
'        If Not Integer.TryParse(strNewPOHdrHistoryID, intNewPOHdrHistoryID) Then
'            ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save existing PO Header History records without success for order number " _
'                & strOrderNumber _
'                & ".<br />See the error log table (tblLog) for more details. The following sql string was executed:<hr />" _
'                & vbCrLf & strSQL & "<hr />" & vbCrLf
'            Return 'We cannot continue   
'        End If
'        Dim sItemHistoryCommands As New List(Of String)
'        Dim sItemPONumbers As New List(Of String)
'        Dim sItemNumbers As New List(Of String)
'        If intNewPOHdrHistoryID > 0 AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
'            'Loop through each item detail record for this order and build an add query            
'            For Each oRow As clsBookDetailObject60 In oDetails
'                strSQL = "Exec dbo.spAddPOItemHistory " _
'                    & intNewPOHdrHistoryID _
'                    & ",'" & Me.AuthorizationCode & "'" _
'                    & DTran.buildSQLString(oRow.ItemPONumber, 20, ",") _
'                    & "," & oRow.FixOffInvAllow _
'                    & "," & oRow.FixFrtAllow _
'                    & DTran.buildSQLString(oRow.ItemNumber, 50, ",") _
'                    & "," & oRow.QtyOrdered _
'                    & "," & oRow.FreightCost _
'                    & "," & oRow.ItemCost _
'                    & "," & oRow.Weight _
'                    & "," & oRow.Cube _
'                    & "," & oRow.Pack _
'                    & DTran.buildSQLString(oRow.Size, 255, ",") _
'                    & DTran.buildSQLString(oRow.Description, 255, ",") _
'                    & DTran.buildSQLString(oRow.Hazmat, 1, ",") _
'                    & ",'" & CreateUser & "'" _
'                    & ",'" & CreatedDate & "'" _
'                    & DTran.buildSQLString(oRow.Brand, 255, ",") _
'                    & DTran.buildSQLString(oRow.CostCenter, 50, ",") _
'                    & DTran.buildSQLString(oRow.LotNumber, 50, ",") _
'                    & DTran.buildSQLString(oRow.LotExpirationDate, 22, ",") _
'                    & DTran.buildSQLString(oRow.GTIN, 50, ",") _
'                    & DTran.buildSQLString(oRow.CustItemNumber, 50, ",") _
'                    & "," & oRow.CustomerNumber _
'                    & "," & oRow.POOrderSequence _
'                    & DTran.buildSQLString(oRow.PalletType, 50, ",") _
'                    & DTran.buildSQLString(oRow.POItemHazmatTypeCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItem49CFRCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemIATACode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemDOTCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemMarineCode, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemNMFCClass, 20, ",") _
'                    & DTran.buildSQLString(oRow.POItemFAKClass, 20, ",") _
'                    & "," & If(oRow.POItemLimitedQtyFlag, "1", "0") _
'                    & "," & oRow.POItemPallets _
'                    & "," & oRow.POItemTies _
'                    & "," & oRow.POItemHighs _
'                    & "," & oRow.POItemQtyPalletPercentage _
'                    & "," & oRow.POItemQtyLength _
'                    & "," & oRow.POItemQtyWidth _
'                    & "," & oRow.POItemQtyHeight _
'                    & "," & If(oRow.POItemStackable, "1", "0") _
'                    & "," & oRow.POItemLevelOfDensity


'                sItemHistoryCommands.Add(strSQL)
'                If Not sItemPONumbers.Contains(oRow.ItemPONumber) Then sItemPONumbers.Add(oRow.ItemPONumber)
'                If Not sItemNumbers.Contains(oRow.ItemNumber) Then sItemNumbers.Add(oRow.ItemNumber)
'            Next
'            'Build the delete query
'            strSQL = "Delete From dbo.POItemHistory Where POItemHistoryPOHdrHistoryControl = " & intNewPOHdrHistoryID
'            Dim blnUseOr As Boolean = False
'            Dim sSpacer As String = ""
'            If sItemPONumbers.Count > 0 Then
'                blnUseOr = True
'                strSQL &= " AND (ItemPONumber NOT IN ("
'                For Each s As String In sItemPONumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                strSQL &= ") "
'            End If
'            If sItemNumbers.Count > 0 Then
'                If blnUseOr Then
'                    strSQL &= " OR ItemNumber NOT IN ("
'                Else
'                    strSQL &= " AND ItemNumber NOT IN ("
'                End If
'                sSpacer = ""
'                For Each s As String In sItemNumbers
'                    strSQL &= String.Format("{0}'{1}'", sSpacer, s)
'                    sSpacer = ","
'                Next
'                If blnUseOr Then
'                    strSQL &= "))"
'                Else
'                    strSQL &= ")"
'                End If
'            ElseIf blnUseOr Then
'                strSQL &= ")"
'            End If

'            If Not executeSQLQuery(strSQL, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
'                ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to delete existing POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf
'            End If
'            'Now execute each add item commands
'            For Each s As String In sItemHistoryCommands
'                If Not executeSQLQuery(s, "NGL.FreightMaster.Integration.clsBook.saveOrderHistory", False) Then
'                    ITEmailMsg &= "<br />" & Source & " Warning (Import Not Affected): NGL.FreightMaster.Integration.clsBook.saveOrderHistory, attempted to save new POItemHistory records without success for order number " & strOrderNumber & ".<br />  Please check the dbo.tblLog for the error information. The following SQL string was not executed:<hr /> " & vbCrLf & s & "<hr />" & vbCrLf
'                End If
'            Next
'        End If

'    End Sub


'    Private Function runDeleteOrderWithData(ByVal strBookProNumber As String, _
'                                                    ByVal strOrderNumber As String, _
'                                                    ByVal intOrderSequence As Integer, _
'                                                    ByVal intDefCompNumber As Integer, _
'                                                    ByRef strMSG As String) As Boolean


'        Dim blnDataErr As Boolean = False
'        Dim blnRet As Boolean = False
'        Dim oCon As New System.Data.SqlClient.SqlConnection
'        Dim oCmd As New System.Data.SqlClient.SqlCommand
'        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
'        Try
'            'Check Data
'            If String.IsNullOrEmpty(strBookProNumber) OrElse strBookProNumber.Trim.Length < 1 Then
'                blnDataErr = True
'                strMSG = "The 'PRO Number' value is invalid."
'            End If

'            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
'                If blnDataErr Then
'                    strMSG &= " And "
'                End If
'                blnDataErr = True
'                strMSG &= "The 'Order Number' value is invalid."
'            End If
'            If intDefCompNumber = 0 Then
'                If blnDataErr Then
'                    strMSG &= " And "
'                End If
'                blnDataErr = True
'                strMSG &= "The 'Default Company' value is invalid."
'            End If
'            If blnDataErr Then Return False

'            ' Ngl.Core.Data.Query(Me.DBServer, Me.Database)
'            'Update the existing PO Data.

'            oCon = getNewConnection(False)
'            oCmd.Parameters.AddWithValue("@BookProNumber", strBookProNumber)
'            If oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spDeleteBookingByPro", 3) Then
'                blnRet = runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strMSG)
'            Else
'                blnRet = False
'            End If
'        Catch ex As Ngl.Core.DatabaseRetryExceededException
'            strMSG = "Delete Booking Data Failure! Failed to execute dbo.spDeleteBookingByPro stored procedure without success: " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseLogInException
'            strMSG = "Delete Booking Data Failure! Database login failure: " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseInvalidException
'            strMSG = "Delete Booking Data Failure! Database access failure : " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseDataValidationException
'            strMSG = ex.Message
'            Return False
'        Catch ex As Exception
'            Throw
'            Return False
'        Finally
'            Try
'                oQuery = Nothing
'            Catch ex As Exception

'            End Try
'            Try
'                oCmd.Cancel()
'                oCmd = Nothing
'            Catch ex As Exception

'            End Try
'            Try
'                oCon.Close()
'                oCon = Nothing
'            Catch ex As Exception

'            End Try
'        End Try

'        Return blnRet

'    End Function

'    Private Function runRemoveDeletedWithData(ByVal strOrderNumber As String, _
'                                                    ByVal intOrderSequence As Integer, _
'                                                    ByVal intDefCompNumber As Integer, _
'                                                    ByRef strMSG As String) As Boolean


'        Dim blnDataErr As Boolean = False
'        Dim blnRet As Boolean = False
'        Dim oCon As New System.Data.SqlClient.SqlConnection
'        Dim oCmd As New System.Data.SqlClient.SqlCommand
'        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)

'        Try
'            'Check Data
'            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
'                blnDataErr = True
'                strMSG = "The 'Order Number' value is invalid."
'            End If
'            If intDefCompNumber = 0 Then
'                If blnDataErr Then
'                    strMSG &= " And "
'                End If
'                blnDataErr = True
'                strMSG &= "The 'Default Company' value is invalid."
'            End If
'            If blnDataErr Then Return False
'            oCon = getNewConnection(False)
'            'Update the existing PO Data.
'            oCmd.Parameters.AddWithValue("@POHdrOrderNumber", strOrderNumber)
'            oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
'            oCmd.Parameters.AddWithValue("@POHDRDefaultCustomer", intDefCompNumber)
'            blnRet = oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spRemoveDeletedPOByComp", 3)
'        Catch ex As Ngl.Core.DatabaseRetryExceededException
'            strMSG = "Remove Deleted Booking Data Failure! Failed to execute dbo.spRemoveDeletedPOByComp stored procedure without success: " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseLogInException
'            strMSG = "Remove Deleted Booking Data Failure! Database login failure: " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseInvalidException
'            strMSG = "Remove Deleted Booking Data Failure! Database access failure : " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseDataValidationException
'            strMSG = ex.Message
'            Return False
'        Catch ex As Exception
'            Throw
'            Return False
'        Finally
'            Try
'                oQuery = Nothing
'            Catch ex As Exception

'            End Try
'            Try
'                oCmd.Cancel()
'                oCmd = Nothing
'            Catch ex As Exception

'            End Try
'            Try
'                oCon.Close()
'                oCon = Nothing
'            Catch ex As Exception

'            End Try
'        End Try

'        Return blnRet

'    End Function

'    Private Function runUpdatePOModificationWithData(ByVal strOrderNumber As String, _
'                                                    ByVal intOrderSequence As Integer, _
'                                                    ByVal intDefCompNumber As Integer, _
'                                                    ByVal strVendorNumber As String, _
'                                                    ByRef strMSG As String) As Boolean


'        Dim blnDataErr As Boolean = False
'        Dim blnRet As Boolean = False

'        Dim oCon As New System.Data.SqlClient.SqlConnection
'        Dim oCmd As New System.Data.SqlClient.SqlCommand
'        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)

'        Try
'            'Check Data
'            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
'                blnDataErr = True
'                strMSG = "The 'Order Number' value is invalid."
'            End If
'            If intDefCompNumber = 0 Then
'                If blnDataErr Then
'                    strMSG &= " And "
'                End If
'                blnDataErr = True
'                strMSG &= "The 'Default Company' value is invalid."
'            End If
'            If String.IsNullOrEmpty(strVendorNumber) OrElse strVendorNumber.Trim.Length < 1 Then
'                If blnDataErr Then
'                    strMSG &= " And "
'                End If
'                blnDataErr = True
'                strMSG &= "The 'Lane Number' value is invalid."
'            End If
'            If blnDataErr Then Return False

'            oCon = getNewConnection(False)
'            oCmd.Parameters.AddWithValue("@POHdrOrderNumber", strOrderNumber)
'            oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
'            oCmd.Parameters.AddWithValue("@POHdrDefCustNumber", intDefCompNumber)
'            oCmd.Parameters.AddWithValue("@POHDRvendor", strVendorNumber)
'            blnRet = oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spUpdateBookingRecord", 3)
'        Catch ex As Ngl.Core.DatabaseRetryExceededException
'            strMSG = "Update PO Modifications Data Failure! Failed to execute dbo.spUpdateBookingRecord stored procedure without success: " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseLogInException
'            strMSG = "Update PO Modifications Data Failure! Database login failure: " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseInvalidException
'            strMSG = "Update PO Modifications Data Failure! Database access failure : " & ex.Message
'            Return False
'        Catch ex As Ngl.Core.DatabaseDataValidationException
'            strMSG = ex.Message
'            Return False
'        Catch ex As Exception
'            Throw
'            Return False
'        Finally
'            Try
'                oQuery = Nothing
'            Catch ex As Exception

'            End Try
'            Try
'                oCmd.Cancel()
'                oCmd = Nothing
'            Catch ex As Exception

'            End Try
'            Try
'                oCon.Close()
'                oCon = Nothing
'            Catch ex As Exception

'            End Try
'        End Try


'        Return blnRet

'    End Function

'    Private Function runProcessFinalizedData(ByVal strOrderNumber As String, _
'                                                    ByVal intOrderSequence As Integer, _
'                                                    ByVal intDefCompNumber As Integer, _
'                                                    ByRef strMSG As String) As Boolean


'        Dim blnDataErr As Boolean = False
'        Dim blnRet As Boolean = False
'        Try
'            'Check Data
'            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
'                blnDataErr = True
'                strMSG = "The 'Order Number' value is invalid."
'            End If
'            If intDefCompNumber = 0 Then
'                If blnDataErr Then
'                    strMSG &= " And "
'                End If
'                blnDataErr = True
'                strMSG &= "The 'Default Company' value is invalid."
'            End If
'            If blnDataErr Then Return False
'            Dim oBookBLL As New BLL.NGLBookBLL(WCFDataProperties.ConvertToWCFProperties(New DAL.WCFParameters()))
'            strMSG = oBookBLL.SilentTenderFinalized(strOrderNumber, intOrderSequence, intDefCompNumber)
'        Catch ex As Exception
'            'For Silent Tender we log all errors
'            blnDataErr = True
'            strMSG = "Silent Tender Finalized Load Failure: " & ex.Message
'            Return False
'        End Try

'        Return blnRet

'    End Function

'    'Private Function ImportPOHdr(ByVal strPOHDRModVerify As String, _
'    '                             ByVal strOrderNumber As String, _
'    '                             ByVal intOrderSequence As Integer, _
'    '                             ByVal intDefCompNumber As Integer, _
'    '                             ByVal strVendorNumber As String, _
'    '                             ByVal strBookProNumber As String,
'    '                             ByVal strSource As String,
'    '                             ByRef strErrMsg As String,
'    '                             ByVal blnSendAsBatch As Boolean) As Boolean


'    '    Dim blnRet As Boolean = False
'    '    strSource &= ".ImportPOHdr"
'    '    Try
'    '        SyncLock mPadLock
'    '            With SharedServices 'Me.PaneSettings.MainInterface.SharedServices
'    '                If Not .UserConnected Then
'    '                    .LogOn(WCFDataProperties)
'    '                End If

'    '                'mintSharedServicesRunning += 1
'    '            End With
'    '        End SyncLock

'    '        Dim oRetData As Ngl.FMWCFProxy.NGLSharedServicesData.NGLSharedServiceBatchData = Me.SharedServices.ImportPOHdrRecordWS(strPOHDRModVerify,
'    '                                                  strOrderNumber,
'    '                                                  intOrderSequence,
'    '                                                  intDefCompNumber,
'    '                                                  strVendorNumber,
'    '                                                  strBookProNumber,
'    '                                                  WCFDataProperties, New Ngl.FMWCFProxy.NGLSharedServicesData.NGLSharedServiceBatchData() With {.Caller = "clsBook", .CallerMethod = "ImportPOHdr", .BatchProcess = blnSendAsBatch})
'    '        If oRetData.Success AndAlso String.IsNullOrEmpty(oRetData.LastError) Then
'    '            blnRet = True
'    '        Else
'    '            strErrMsg = oRetData.LastError
'    '        End If

'    '        'Me.SharedServices.ImportPOHdrRecord(strPOHDRModVerify,
'    '        '                                         strOrderNumber,
'    '        '                                         intOrderSequence,
'    '        '                                         intDefCompNumber,
'    '        '                                         strVendorNumber,
'    '        '                                         strBookProNumber,
'    '        '                                         WCFDataProperties, New Ngl.FMWCFProxy.NGLSharedServicesData.NGLSharedServiceBatchData() With {.Caller = "clsBook", .CallerMethod = "ImportPOHdr", .BatchProcess = blnSendAsBatch})

'    '        'blnRet = True
'    '    Catch ex As System.ObjectDisposedException
'    '        strErrMsg = strSource & " " & ex.ToString
'    '    Catch ex As InvalidOperationException
'    '        strErrMsg = strSource & " " & ex.ToString
'    '    Catch ex As Exception
'    '        Throw
'    '    Finally
'    '        'SyncLock mPadLock
'    '        '    mintSharedServicesRunning -= 1
'    '        '    If mintSharedServicesRunning < 1 Then
'    '        '        If SharedServices.UserConnected Then SharedServices.LogOff(WCFDataProperties)
'    '        '    End If
'    '        'End SyncLock
'    '    End Try


'    '    Return blnRet
'    'End Function

'#End Region

'#Region "Public Functions "

'    Public Function getHeaderTableFromRow(ByRef oRow As BookData.BookHeaderRow) As BookData.BookHeaderDataTable
'        Dim oTable As New BookData.BookHeaderDataTable
'        Dim newRow As BookData.BookHeaderRow = oTable.NewBookHeaderRow
'        For i As Integer = 0 To oTable.Columns.Count - 1
'            newRow.Item(i) = oRow.Item(i)
'        Next
'        oTable.AddBookHeaderRow(newRow)
'        Return oTable
'    End Function

'    Public Function ProcessObjectData( _
'                    ByVal oOrders() As clsBookHeaderObject, _
'                    ByVal oDetails() As clsBookDetailObject, _
'                    ByVal strConnection As String) As ProcessDataReturnValues
'        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
'        Dim oHTable As New BookData.BookHeaderDataTable
'        Dim oDTable As New BookData.BookDetailDataTable
'        Dim dtVal As Date
'        Try
'            For Each oOrder As clsBookHeaderObject In oOrders
'                Dim oRow As BookData.BookHeaderRow = oHTable.NewBookHeaderRow
'                With oRow
'                    .PONumber = Left(oOrder.PONumber, 20)
'                    .POVendor = Left(oOrder.POVendor, 50)
'                    If validateDateWS(oOrder.POdate, dtVal) Then
'                        .POdate = exportDateToString(dtVal.ToString)
'                    End If
'                    If validateDateWS(oOrder.POShipdate, dtVal) Then
'                        .POShipdate = exportDateToString(dtVal.ToString)
'                    End If
'                    .POBuyer = Left(oOrder.POBuyer, 10)
'                    .POFrt = oOrder.POFrt
'                    .POTotalFrt = oOrder.POTotalFrt
'                    .POTotalCost = oOrder.POTotalCost
'                    .POWgt = oOrder.POWgt
'                    .POCube = oOrder.POCube
'                    .POQty = oOrder.POQty
'                    .POPallets = oOrder.POPallets
'                    .POLines = oOrder.POLines
'                    .POConfirm = oOrder.POConfirm
'                    .PODefaultCustomer = Left(oOrder.PODefaultCustomer, 50)
'                    .PODefaultCarrier = oOrder.PODefaultCarrier
'                    If validateDateWS(oOrder.POReqDate, dtVal) Then
'                        .POReqDate = exportDateToString(dtVal.ToString)
'                    End If
'                    .POShipInstructions = Left(oOrder.POShipInstructions, 255)
'                    .POCooler = oOrder.POCooler
'                    .POFrozen = oOrder.POFrozen
'                    .PODry = oOrder.PODry
'                    .POTemp = Left(oOrder.POTemp, 1)
'                    .POCarType = Left(oOrder.POCarType, 15)
'                    .POShipVia = Left(oOrder.POShipVia, 10)
'                    .POShipViaType = Left(oOrder.POShipViaType, 10)
'                    .POConsigneeNumber = Left(oOrder.POConsigneeNumber, 50)
'                    .POCustomerPO = Left(oOrder.POCustomerPO, 20)
'                    .POOtherCosts = oOrder.POOtherCosts
'                    .POStatusFlag = oOrder.POStatusFlag
'                    .POOrderSequence = oOrder.POOrderSequence
'                    .POChepGLID = Left(oOrder.POChepGLID, 50)
'                    .POCarrierEquipmentCodes = Left(oOrder.POCarrierEquipmentCodes, 50)
'                    .POCarrierTypeCode = Left(oOrder.POCarrierTypeCode, 50)
'                    .POPalletPositions = Left(oOrder.POPalletPositions, 50)
'                    If validateDateWS(oOrder.POSchedulePUDate, dtVal) Then
'                        .POSchedulePUDate = exportDateToString(dtVal.ToString)
'                    End If
'                    If ValidateTimeWS(oOrder.POSchedulePUTime) Then
'                        .POSchedulePUTime = oOrder.POSchedulePUTime
'                    End If
'                    If validateDateWS(oOrder.POScheduleDelDate, dtVal) Then
'                        .POScheduleDelDate = exportDateToString(dtVal.ToString)
'                    End If
'                    If ValidateTimeWS(oOrder.POSCheduleDelTime) Then
'                        .POSCHeduleDelTime = oOrder.POSCheduleDelTime
'                    End If
'                    If validateDateWS(oOrder.POActPUDate, dtVal) Then
'                        .POActPUDate = exportDateToString(dtVal.ToString)
'                    End If
'                    If ValidateTimeWS(oOrder.POActPUTime) Then
'                        .POActPUTime = oOrder.POActPUTime
'                    End If
'                    If validateDateWS(oOrder.POActDelDate, dtVal) Then
'                        .POActDelDate = exportDateToString(dtVal.ToString)
'                    End If
'                    If ValidateTimeWS(oOrder.POActDelTime) Then
'                        .POActDelTime = oOrder.POActDelTime
'                    End If
'                End With
'                oHTable.AddBookHeaderRow(oRow)
'            Next
'            If Not oDetails Is Nothing Then
'                For Each oDetail As clsBookDetailObject In oDetails
'                    Dim oRow As BookData.BookDetailRow = oDTable.NewBookDetailRow
'                    With oRow
'                        .ItemPONumber = Left(oDetail.ItemPONumber, 20)
'                        .FixOffInvAllow = oDetail.FixOffInvAllow
'                        .FixFrtAllow = oDetail.FixFrtAllow
'                        .ItemNumber = Left(oDetail.ItemNumber, 50)
'                        .QtyOrdered = oDetail.QtyOrdered
'                        .FreightCost = oDetail.FreightCost
'                        .ItemCost = oDetail.ItemCost
'                        .Weight = oDetail.Weight
'                        .Cube = oDetail.Cube
'                        .Pack = oDetail.Pack
'                        .PalletType = Left(oDetail.PalletType, 50)
'                        .Size = Left(oDetail.Size, 255)
'                        .Description = Left(oDetail.Description, 255)
'                        .Hazmat = Left(oDetail.Hazmat, 1)
'                        .Brand = Left(oDetail.Brand, 255)
'                        .CostCenter = Left(oDetail.CostCenter, 50)
'                        .LotNumber = Left(oDetail.LotNumber, 50)
'                        If validateDateWS(oDetail.LotExpirationDate, dtVal) Then
'                            .LotExpirationDate = exportDateToString(dtVal.ToString)
'                        End If
'                        .GTIN = Left(oDetail.GTIN, 50)
'                        .CustItemNumber = Left(oDetail.CustItemNumber, 50)
'                        .CustomerNumber = Left(oDetail.CustomerNumber, 50)
'                        .POOrderSequence = oDetail.POOrderSequence
'                    End With
'                    oDTable.AddBookDetailRow(oRow)
'                Next
'            End If
'            intRet = ProcessData(oHTable, oDTable, strConnection)
'        Catch ex As Exception
'            LogException("Process Object Data Failure", "Order import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessObjectData Failure")
'        End Try
'        Return intRet


'    End Function



'    Public Function ProcessObjectData( _
'                    ByVal oOrders As List(Of clsBookHeaderObject70), _
'                    ByVal oDetails As List(Of clsBookDetailObject70), _
'                    ByVal strConnection As String) As ProcessDataReturnValues

'        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
'        Dim strMsg As String = ""
'        Dim strTitle As String = ""
'        Dim strSource As String = "clsBook.ProcessObjectData"
'        Dim strHeaderTable As String = "POHDR"
'        Dim strItemTable As String = "POItem"
'        Me.HeaderName = "PO Header"
'        Me.ItemName = "PO Item"
'        Me.ImportTypeKey = IntegrationTypes.Book
'        Me.CreatedDate = Now.ToString
'        Me.CreateUser = "Data Integration DLL"
'        Me.Source = "Book Data Integration"

'        Me.DBConnection = strConnection
'        'try the connection
'        Try

'            If Not Me.openConnection Then
'                Return ProcessDataReturnValues.nglDataConnectionFailure
'            End If
'        Catch ex As Exception
'            Return ProcessDataReturnValues.nglDataConnectionFailure
'        Finally
'            Try
'                closeConnection()
'            Catch ex As Exception

'            End Try
'        End Try
'        Me.DALParameters.ConnectionString = strConnection

'        'set the error date time stamp and other Defaults
'        'Header Information
'        Dim oFields As New clsImportFields
'        If Not buildHeaderCollection70(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
'        Try

'            'Import the Header Records
'            If importHeaderRecords(oOrders, oDetails, oFields) Then
'                'If any item details were missing we re-import all of them
'                If mblnSomeItemsMissing AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
'                    'reset all counters
'                    ItemErrors = 0
'                    TotalItems = 0
'                    mintTotalQty = 0
'                    mdblTotalWeight = 0
'                    mdblHashTotalDetails = 0
'                    importItemRecords(oDetails)
'                End If
'                'SilentTenderAsync()
'            End If
'            strTitle = "Process Data Complete"
'            If GroupEmailMsg.Trim.Length > 0 Then
'                LogError("Process PO Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
'            End If
'            If ITEmailMsg.Trim.Length > 0 Then
'                LogError("Process PO Data Failure", "The following errors or warnings were reported some PO records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
'            End If
'            If Me.TotalRecords > 0 Then
'                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
'                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
'                If Me.TotalItems > 0 Then
'                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
'                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
'                End If
'                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
'                    strTitle = "Process Data Complete With Errors"
'                    If Me.RecordErrors > 0 Then
'                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the PO Import Error Report for more information."
'                    End If
'                    If Me.ItemErrors > 0 Then
'                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the PO Item Import Error Report for more information."
'                    End If
'                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
'                End If
'            Else
'                strMsg = "No " & Me.HeaderName & " records were imported."
'                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
'            End If
'            Log(strMsg)
'        Catch ex As Exception
'            LogException("Process PO Data Failure", "Could not process the requested PO data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessData")
'        Finally
'            'mblnSharedServiceRunning = False
'            Try
'                closeConnection()
'            Catch ex As Exception

'            End Try
'        End Try
'        Return intRet


'    End Function


'    Public Function ProcessObjectData( _
'                    ByVal oOrders As List(Of clsBookHeaderObject60), _
'                    ByVal oDetails As List(Of clsBookDetailObject60), _
'                    ByVal strConnection As String) As ProcessDataReturnValues

'        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
'        Dim strMsg As String = ""
'        Dim strTitle As String = ""
'        Dim strSource As String = "clsBook.ProcessObjectData"
'        Dim strHeaderTable As String = "POHDR"
'        Dim strItemTable As String = "POItem"
'        Me.HeaderName = "PO Header"
'        Me.ItemName = "PO Item"
'        Me.ImportTypeKey = IntegrationTypes.Book
'        Me.CreatedDate = Now.ToString
'        Me.CreateUser = "Data Integration DLL"
'        Me.Source = "Book Data Integration"

'        Me.DBConnection = strConnection
'        'try the connection
'        Try

'            If Not Me.openConnection Then
'                Return ProcessDataReturnValues.nglDataConnectionFailure
'            End If
'        Catch ex As Exception
'            Return ProcessDataReturnValues.nglDataConnectionFailure
'        Finally
'            Try
'                closeConnection()
'            Catch ex As Exception

'            End Try
'        End Try

'        'set the error date time stamp and other Defaults
'        'Header Information
'        Dim oFields As New clsImportFields
'        If Not buildHeaderCollection60(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
'        Try

'            'Import the Header Records
'            If importHeaderRecords(oOrders, oDetails, oFields) Then
'                'If any item details were missing we re-import all of them
'                If mblnSomeItemsMissing AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
'                    'reset all counters
'                    ItemErrors = 0
'                    TotalItems = 0
'                    mintTotalQty = 0
'                    mdblTotalWeight = 0
'                    mdblHashTotalDetails = 0
'                    importItemRecords(oDetails)
'                End If
'                'SilentTenderAsync()
'            End If
'            strTitle = "Process Data Complete"
'            If GroupEmailMsg.Trim.Length > 0 Then
'                LogError("Process PO Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
'            End If
'            If ITEmailMsg.Trim.Length > 0 Then
'                LogError("Process PO Data Failure", "The following errors or warnings were reported some PO records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
'            End If
'            If Me.TotalRecords > 0 Then
'                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
'                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
'                If Me.TotalItems > 0 Then
'                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
'                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
'                End If
'                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
'                    strTitle = "Process Data Complete With Errors"
'                    If Me.RecordErrors > 0 Then
'                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the PO Import Error Report for more information."
'                    End If
'                    If Me.ItemErrors > 0 Then
'                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the PO Item Import Error Report for more information."
'                    End If
'                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
'                End If
'            Else
'                strMsg = "No " & Me.HeaderName & " records were imported."
'                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
'            End If
'            Log(strMsg)
'        Catch ex As Exception
'            LogException("Process PO Data Failure", "Could not process the requested PO data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessData")
'        Finally
'            'mblnSharedServiceRunning = False
'            Try
'                closeConnection()
'            Catch ex As Exception

'            End Try
'        End Try
'        Return intRet


'    End Function

'    Public Function ProcessData( _
'                ByVal oOrders As BookData.BookHeaderDataTable, _
'                ByVal oDetails As BookData.BookDetailDataTable, _
'                ByVal strConnection As String) As ProcessDataReturnValues

'        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
'        Dim strMsg As String = ""
'        Dim strTitle As String = ""
'        Dim strSource As String = "clsBook.ProcessData"
'        Dim strHeaderTable As String = "POHDR"
'        Dim strItemTable As String = "POItem"
'        Me.HeaderName = "PO Header"
'        Me.ItemName = "PO Item"
'        Me.ImportTypeKey = IntegrationTypes.Book
'        Me.CreatedDate = Now.ToString
'        Me.CreateUser = "Data Integration DLL"
'        Me.Source = "Book Data Integration"

'        Me.DBConnection = strConnection
'        'try the connection
'        Try

'            If Not Me.openConnection Then
'                Return ProcessDataReturnValues.nglDataConnectionFailure
'            End If
'        Catch ex As Exception
'            Return ProcessDataReturnValues.nglDataConnectionFailure
'        Finally
'            Try
'                closeConnection()
'            Catch ex As Exception

'            End Try
'        End Try



'        'set the error date time stamp and other Defaults
'        'Header Information
'        Dim oFields As New clsImportFields
'        If Not buildHeaderCollection(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
'        Try
'            'Import the Header Records
'            If importHeaderRecords(oOrders, oDetails, oFields) Then
'                'If any item details were missing we re-import all of them
'                If mblnSomeItemsMissing AndAlso Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
'                    'reset all counters
'                    ItemErrors = 0
'                    TotalItems = 0
'                    mintTotalQty = 0
'                    mdblTotalWeight = 0
'                    mdblHashTotalDetails = 0
'                    importItemRecords(oDetails)
'                End If
'                'SilentTenderAsync()
'            End If
'            strTitle = "Process Data Complete"
'            If GroupEmailMsg.Trim.Length > 0 Then
'                LogError("Process PO Data Warning", "The following errors or warnings were reported some PO records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
'            End If
'            If ITEmailMsg.Trim.Length > 0 Then
'                LogError("Process PO Data Failure", "The following errors or warnings were reported some PO records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
'            End If
'            If Me.TotalRecords > 0 Then
'                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
'                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
'                If Me.TotalItems > 0 Then
'                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
'                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
'                End If
'                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
'                    strTitle = "Process Data Complete With Errors"
'                    If Me.RecordErrors > 0 Then
'                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the PO Import Error Report for more information."
'                    End If
'                    If Me.ItemErrors > 0 Then
'                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the PO Item Import Error Report for more information."
'                    End If
'                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
'                End If
'            Else
'                strMsg = "No " & Me.HeaderName & " records were imported."
'                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
'            End If
'            Log(strMsg)
'        Catch ex As Exception
'            LogException("Process PO Data Failure", "Could not process the requested PO data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsBook.ProcessData")
'        Finally
'            Try
'                closeConnection()
'            Catch ex As Exception

'            End Try
'        End Try
'        Return intRet
'    End Function

'    'Public Sub SilentTenderAsync()


'    '    Dim fetcher As New ProcessDataDelegate(AddressOf Me.silentTenderLoadsExecAsync)

'    '    ' Launch thread
'    '    fetcher.BeginInvoke(Nothing, Nothing)

'    'End Sub

'#End Region


'    '#Region "NGLSharedDataService Events"

'    '    Private Sub _SharedServices_BatchProcessingReturnEvent(sender As Object, e As FMWCFProxy.FMSharedServicesBatchProcessingReturnEventArgs) Handles _SharedServices.BatchProcessingReturnEvent
'    '        If Not e Is Nothing AndAlso Not e.Result Is Nothing Then
'    '            Select Case e.Result.Caller
'    '                Case "clsBook"
'    '                    Log(String.Format("Batch Process Returned {0} with {1} ", e.Result.Success, e.Result.LastError))
'    '            End Select
'    '        End If

'    '        'Try
'    '        '    If Not mblnSharedServiceRunning Then SharedServices.LogOff(WCFDataProperties)
'    '        'Catch ex As Exception
'    '        '    Log("Shared Services Log off Error: " & ex.Message)
'    '        'End Try

'    '    End Sub


'    '    'Private Sub NGLSharedDataService_FaultException(sender As Object, e As FMWCFProxy.FaultExceptionEventArgs) Handles NGLSharedDataService.FaultException
'    '    '    Dim strMessage As String = NGLSharedDataService.FormatDefaultFaultExceptionMessage(e.Source, e.Reason, e.Message, e.Detail)
'    '    '    GroupEmailMsg &= strMessage
'    '    '    Log(strMessage)
'    '    'End Sub

'    '    'Private Sub NGLSharedDataService_TimeOutException(sender As Object, e As FMWCFProxy.FaultExceptionEventArgs) Handles NGLSharedDataService.TimeOutException
'    '    '    Dim strMessage As String = NGLSharedDataService.FormatDefaultFaultExceptionMessage(e.Source, e.Reason, e.Message, e.Detail)
'    '    '    GroupEmailMsg &= strMessage
'    '    '    Log(strMessage)
'    '    'End Sub

'    '    Private Sub _SharedServices_ExceptionEvent(sender As Object, e As FMWCFProxy.FMSharedServicesExceptionEventArgs) Handles _SharedServices.ExceptionEvent
'    '        If Not e Is Nothing AndAlso Not e.Exceptions Is Nothing AndAlso e.Exceptions.Count > 0 Then
'    '            Dim strMsg As String = ""
'    '            Select Case e.Caller
'    '                Case "clsBook"
'    '                    For Each ex In e.Exceptions
'    '                        strMsg &= String.Concat(SharedServices.LocalizeString(ex.Message), SharedServices.LocalizeString(ex.Details), vbCrLf, vbCrLf)
'    '                    Next
'    '                    GroupEmailMsg &= strMsg
'    '                    Log(strMsg)
'    '                Case Else
'    '                    'other modules or classes handle these errors
'    '            End Select
'    '        End If
'    '    End Sub

'    '#End Region

'    Protected Overrides Sub Finalize()
'        MyBase.Finalize()
'    End Sub

'End Class



