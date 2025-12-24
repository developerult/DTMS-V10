
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Runtime.Serialization
Imports System.Runtime.CompilerServices
Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Text.RegularExpressions

''' <summary>
''' 
''' </summary>
''' <remarks>
''' Created by RHR v-7.0.5.102 8/23/2016
'''  Public Module use to provide Extensions to Linq Queries
''' </remarks>
Module LinqExtensions


    ''' <summary>
    ''' TMS specific Order by extension uses the prvided orderByProperty and desc boolean flag to sort the linq query
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="source"></param>
    ''' <param name="orderByProperty"></param>
    ''' <param name="desc"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.102 8/23/2016
    ''' </remarks>
    <System.Runtime.CompilerServices.Extension> _
    Public Function OrderBy(Of TEntity)(source As IQueryable(Of TEntity), orderByProperty As String, desc As Boolean) As IQueryable(Of TEntity)
        Try
            Dim command As String = If(desc, "OrderByDescending", "OrderBy")
            Dim type = GetType(TEntity)
            Dim [property] = type.GetProperty(orderByProperty)
            Dim parameter = Expression.Parameter(type, "p")
            Dim propertyAccess = Expression.MakeMemberAccess(parameter, [property])
            Dim orderByExpression = Expression.Lambda(propertyAccess, parameter)
            Dim resultExpression = Expression.[Call](GetType(Queryable), command, New Type() {type, [property].PropertyType}, source.Expression, Expression.Quote(orderByExpression))
            Return source.Provider.CreateQuery(Of TEntity)(resultExpression)

        Catch ex As Exception
            'merely ordering should not cause an exception and crash the app.
            'if the field is not found or the query is invalid the original query should be returned
            Return source
        End Try
    End Function

    ''' <summary>
    ''' uses a comma seperated list in sortExpression to dynamically sort a linq query.  data must consist of a field name -- space --  followed by a sort ored like ASC or Desc.  No spaces are allowed int he field name.
    ''' 
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="source"></param>
    ''' <param name="sortExpression"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.102 8/23/2016
    ''' </remarks>
    <System.Runtime.CompilerServices.Extension> _
    Public Function OrderBy(Of T)(source As IQueryable(Of T), sortExpression As String) As IQueryable(Of T)
        If source Is Nothing Then
            Throw New ArgumentNullException("source", "source is null.")
        End If

        If String.IsNullOrEmpty(sortExpression) Then
            Throw New ArgumentException("sortExpression is null or empty.", "sortExpression")
        End If
        Dim filters = sortExpression.Split(",")
        If Not filters Is Nothing AndAlso filters.Count > 0 Then

            For Each f In filters
                Dim parts = f.Split(" ")
                Dim isDescending = False
                Dim propertyName = ""
                Dim tType = GetType(T)

                If parts.Length > 0 AndAlso parts(0) <> "" Then
                    propertyName = parts(0)

                    If parts.Length > 1 Then
                        isDescending = parts(1).ToLower().Contains("esc")
                    End If

                    Dim prop As PropertyInfo = tType.GetProperty(propertyName)

                    If prop Is Nothing Then
                        Throw New ArgumentException(String.Format("No property '{0}' on type '{1}'", propertyName, tType.Name))
                    End If

                    Dim funcType = GetType(Func(Of ,)).MakeGenericType(tType, prop.PropertyType)

                    Dim lambdaBuilder = GetType(Expression).GetMethods().First(Function(x) x.Name = "Lambda" AndAlso x.ContainsGenericParameters AndAlso x.GetParameters().Length = 2).MakeGenericMethod(funcType)

                    Dim parameter = Expression.Parameter(tType)
                    Dim propExpress = Expression.[Property](parameter, prop)

                    Dim sortLambda = lambdaBuilder.Invoke(Nothing, New Object() {propExpress, New ParameterExpression() {parameter}})

                    Dim sorter = GetType(Queryable).GetMethods().FirstOrDefault(Function(x) x.Name = (If(isDescending, "OrderByDescending", "OrderBy")) AndAlso x.GetParameters().Length = 2).MakeGenericMethod({tType, prop.PropertyType})

                    Return DirectCast(sorter.Invoke(Nothing, New Object() {source, sortLambda}), IQueryable(Of T))
                End If
            Next
        End If


        Return source
    End Function


End Module

Public Class Utilities


#Region "   Changes Made in v-8.2.1.004 By RHR on 12/23/2019 to support new AP Procssing Rules (move code from BLL to DAL where possible)"


#Region "Enum"

    Public Enum ResultProcedures
        None
        freightbill
    End Enum

    Public Enum ResultTitles
        None
        TitleSaveHistLogFailure
        TitleSaveExpectedCost
        TitleDataValidationError
        TitlePendingFeeApprovalWarning
        TitlePendingFeeApprovalError
        TitleAuditFreightBillWarning
    End Enum

    Public Enum ResultPrefix
        None
        MsgDetails
        MsgCostComparisonNotAvailable
        MsgUnexpectedFeeValidationIssue
        MsgRecalculateCostForFeeFailed
    End Enum

    Public Enum ResultSuffix
        None
        MsgDoesNotEffectProcess
        MsgCheckAppErrorLogs
        MsgUpdatedTotalCostManually
    End Enum

#End Region

#Region "New Localized Strings by Key dictionary"


    Private Shared _dicCMLocalizedByKey As New Dictionary(Of String, LTS.cmLocalizeKeyValuePair)
    Public Shared Property dicCMLocalizedByKey() As Dictionary(Of String, LTS.cmLocalizeKeyValuePair)
        Get
            If _dicCMLocalizedByKey Is Nothing Then _dicCMLocalizedByKey = New Dictionary(Of String, LTS.cmLocalizeKeyValuePair)
            Return _dicCMLocalizedByKey
        End Get
        Set(ByVal value As Dictionary(Of String, LTS.cmLocalizeKeyValuePair))
            _dicCMLocalizedByKey = value
        End Set
    End Property

#End Region


    Private Shared _dictLoadStatusControls As New Dictionary(Of Integer, Integer)
    ''' <summary>
    ''' this dictionary can be static and used across multiple sessions because the LoadStatsCodes are 
    ''' never deleted only added.  improves performance by reducing database lookups on tables that 
    ''' do not change or only allow inserts
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property dictLoadStatusControls() As Dictionary(Of Integer, Integer)
        Get
            Return _dictLoadStatusControls
        End Get
        Set(ByVal value As Dictionary(Of Integer, Integer))
            _dictLoadStatusControls = value
        End Set
    End Property

    Public Shared Function tryGetLoadStatusControl(ByVal LoadStatusCode As Integer, ByRef LoadStatusControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Try
            If dictLoadStatusControls Is Nothing OrElse dictLoadStatusControls.Count < 1 Then Return False
            If dictLoadStatusControls.ContainsKey(LoadStatusCode) Then
                LoadStatusControl = dictLoadStatusControls(LoadStatusCode)
                blnRet = True
            End If

        Catch ex As Exception
            'ignore errors just return false
        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' adds or replaces the LoadStatusControl to the dictLoadStatusControls object using the key LoadStatusCode returns the LoadStatusControl
    ''' </summary>
    ''' <param name="LoadStatusCode"></param>
    ''' <param name="LoadStatusControl"></param>
    ''' <remarks></remarks>
    Public Shared Function storeLoadStatusControl(ByVal LoadStatusCode As Integer, ByVal LoadStatusControl As Integer) As Integer
        Try
            If dictLoadStatusControls Is Nothing Then dictLoadStatusControls = New Dictionary(Of Integer, Integer)

            If Not dictLoadStatusControls.ContainsKey(LoadStatusCode) Then
                dictLoadStatusControls.Add(LoadStatusCode, LoadStatusControl)
            ElseIf dictLoadStatusControls(LoadStatusCode) <> LoadStatusControl Then
                dictLoadStatusControls(LoadStatusCode) = LoadStatusControl
            End If

        Catch ex As Exception
            'ignore errors just return false
        End Try
        Return LoadStatusControl
    End Function


#End Region


    ''' <summary>
    ''' Suported Single Sign On Accounts Enum
    ''' </summary>
    ''' <remarks>
    ''' Modified  By RHR for v-8.5.1.001 on 01/25/2022 added new API SSOA
    ''' changes made here must also be made in spUtilityPopulateLookupLists70
    ''' </remarks>
    <DataContract()>
    Public Enum SSOAAccount
        <EnumMember(Value:="0")> None = 0
        <EnumMember(Value:="1")> NGL = 1
        <EnumMember(Value:="2")> DAT = 2
        <EnumMember(Value:="3")> NextStop = 3
        <EnumMember(Value:="4")> P44 = 4
        <EnumMember(Value:="5")> Microsoft = 5
        <EnumMember(Value:="6")> NGLR = 6 'NGL Service Account
        <EnumMember(Value:="7")> BingMaps = 7 'Added By LVV on 9/24/19 Bing Maps
        <EnumMember(Value:="8")> Trimble = 8 'Added By RHR on 12/29/2020 Trimble API
        <EnumMember(Value:="9")> Samsara = 9 'Added By RHR on 07/24/2021 
        <EnumMember(Value:="10")> CHRAPI = 10 'Added By RHR for v-8.5.1.001 on 01/25/2022
        <EnumMember(Value:="11")> UPSAPI = 11 'Added By RHR for v-8.5.1.001 on 01/25/2022
        <EnumMember(Value:="12")> YRCAPI = 12 'Added By RHR for v-8.5.1.001 on 01/25/2022
        <EnumMember(Value:="13")> JTSAPI = 13 'Added By RHR for v-8.5.1.001 on 01/25/2022
        <EnumMember(Value:="14")> FedXAPI = 14 'Added By RHR for v-8.5.1.001 on 01/25/2022
        <EnumMember(Value:="15")> EngageLaneAPI = 15 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="16")> HMBayAPI = 16 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="17")> FFEAPI = 17 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="18")> EVANSTSAPI = 18 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="19")> FROZENLOGAPI = 19 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="20")> HUDSONAPI = 20 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="21")> JBPARTNERSAPI = 21 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="22")> LANTERAPI = 22 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="23")> TQLAPI = 23 'Added By RHR for v-8.5.4.003 on 10/10/2023
        <EnumMember(Value:="24")> GTZAPI = 24 'Global Tranz API Added By RHR for v-8.5.4.005 on 03/16/2024
    End Enum


    ''' <summary>
    '''SSOA ENUM verion of tblSSOAType data
    ''' </summary>
    ''' <remarks>
    ''' added by RHR for v-8.5.4.002 note: changes to database need to be updated in DAL Utilities SSOAAccount Enum
    ''' changes made here must also be made in spUtilityPopulateLookupLists70
    ''' Modified by RHR for v-8.5.4.005  on 03/21/2024 added new API options 
    ''' </remarks>
    <DataContract()>
    Public Enum SSOAType
        <EnumMember(Value:="0")> None = 0
        <EnumMember(Value:="1")> RateRequestOut = 1 'Send Outbound Rate Request to Carrier
        <EnumMember(Value:="2")> RateRequestIn = 2 'Receive Inbound Rate Request from Partner
        <EnumMember(Value:="3")> LoadTenderOut = 3 'Send Outbound Load Tender to Carrier; like EDI 204 Out
        <EnumMember(Value:="4")> LoadTenderIn = 4 'Receive Inbound Load Tender from partner like EDI 204 In 
        <EnumMember(Value:="5")> LoadDetailOut = 5 'Send Outbound Load Detail to Partner like EDI 220 
        <EnumMember(Value:="6")> LoadDetailIn = 6 'Receive Inbound Load Detail From Carrier ; like EDI 220 
        <EnumMember(Value:="7")> AcceptRejectOut = 7 'Send Outbound Accept Reject to Partner like EDI 990 
        <EnumMember(Value:="8")> AcceptRejectIn = 8 'Receive Inbound Accept Reject From Carrier like EDI 990 
        <EnumMember(Value:="9")> LoadStatusOut = 9 'Send Outbound Load Status to Partner like EDI 214 
        <EnumMember(Value:="10")> LoadStatusIn = 10 'Receive Inbound Load Status from Carrier like EDI 214 
        <EnumMember(Value:="11")> FreightInvoiceOut = 11 'Send Outbound Freight Invoice to Partner like EDI 210 
        <EnumMember(Value:="12")> FreightInvoiceIn = 12 'Receive Inbound Freight Invoice from Carrier like EDI 210  
        <EnumMember(Value:="13")> LoadStatusRequestOut = 13 'Send Request to carrier for Load Status like EDI 214  
        <EnumMember(Value:="14")> LoadStatusRequestIn = 14 'Receive Request from Partner for Load Status like EDI 214   
        <EnumMember(Value:="15")> FreightInvoiceRequestOut = 15 'Send Request to carrier for Freight Invoice like EDI 210   
        <EnumMember(Value:="16")> FreightInvoiceRequestIn = 16 'Receive Request from Partner for Freight Invoice like EDI 210   
    End Enum

    Public Enum NGLStoredProcError
        NoError = 0
        ParameterVariable = 1 ' one or more of the parameters is not valid for the operation
        DataValidation = 2 'Cannot Complete Request like when Costing is locked or No Tariff available or other data validaton errors
        MissingValue = 3 'Invalid Required Value like Transaction Code
        PreviouslyRun = 4 'Attempt to duplicate a previously executed process like when a freight bill has already been processed
        ActionRequired = 5 'Attempt to duplicate a previously executed process some action is required like when a the over write option on the AP Mass Entry Table should be selected
        InvalidUser = 6 'Invalid User Name
        InvalidPassword = 7 'Invalid(Password)
        NotAuthorized = 8 ' like a NET_ADDRESS_FAILURE(Database Is Not valid)
        InvalidKey = 9 'Invalid key value assigned based on parameter settings or keys: like Actual Carrier Assigned is not valid

    End Enum



    ''' <summary>
    ''' Look up Single Sign On Account for user Authentication
    ''' </summary>
    ''' <param name="parameters"></param>
    ''' <returns>
    ''' returns the SSOAControl or uses the enum to return the value
    ''' </returns>
    ''' <remarks>
    ''' Modified  By RHR for v-8.5.1.001 on 01/25/2022 added new API SSOA
    ''' </remarks>
    Public Shared Function getSSOAccountEnum(ByVal parameters As WCFParameters) As SSOAAccount
        If System.Enum.IsDefined(GetType(SSOAAccount), parameters.SSOAControl) Then
            Return parameters.SSOAControl
        End If
        If parameters.SSOAName.ToUpper() = "NGLR" Then
            Return SSOAAccount.NGLR
        ElseIf parameters.SSOAName.ToUpper().Contains("NGL") Then
            Return SSOAAccount.NGL
        ElseIf parameters.SSOAName.ToUpper().Contains("DAT") Then
            Return SSOAAccount.DAT
        ElseIf parameters.SSOAName.ToUpper().Contains("P44") Then
            Return SSOAAccount.P44
        ElseIf parameters.SSOAName.ToUpper().Contains("MICRO") Then
            Return SSOAAccount.Microsoft
        ElseIf parameters.SSOAName.ToUpper().Contains("BingMaps") Then
            Return SSOAAccount.BingMaps
        ElseIf parameters.SSOAName.ToUpper().Contains("Trimble") Then
            Return SSOAAccount.Trimble
        ElseIf parameters.SSOAName.ToUpper().Contains("Samsara") Then
            Return SSOAAccount.Samsara
        ElseIf parameters.SSOAName.ToUpper().Contains("CHR") Then
            Return SSOAAccount.CHRAPI
        ElseIf parameters.SSOAName.ToUpper().Contains("UPS") Then
            Return SSOAAccount.UPSAPI
        ElseIf parameters.SSOAName.ToUpper().Contains("YRC") Then
            Return SSOAAccount.YRCAPI
        ElseIf parameters.SSOAName.ToUpper().Contains("JTS") Then
            Return SSOAAccount.JTSAPI
        ElseIf parameters.SSOAName.ToUpper().Contains("FedX") Then
            Return SSOAAccount.FedXAPI
        Else Return SSOAAccount.None
        End If
    End Function

    Public Shared Function getSSOAccountEnum(ByVal SSOAName As String) As SSOAAccount

        If SSOAName.ToUpper() = "NGLR" Then
            Return SSOAAccount.NGLR
        ElseIf SSOAName.ToUpper().Contains("NGL") Then
            Return SSOAAccount.NGL
        ElseIf SSOAName.ToUpper().Contains("DAT") Then
            Return SSOAAccount.DAT
        ElseIf SSOAName.ToUpper().Contains("P44") Then
            Return SSOAAccount.P44
        ElseIf SSOAName.ToUpper().Contains("MICRO") Then
            Return SSOAAccount.Microsoft
        ElseIf SSOAName.ToUpper().Contains("NEXTS") Then
            Return SSOAAccount.NextStop
        Else Return SSOAAccount.None
        End If
    End Function

    <DataContract()>
    Public Enum NGLSPErrorCodes
        No_Error = 0
        Parameter_Variable_Error = 1
        Cannot_Complete_Request = 2 ' like when Costing is locked or No Tariff available or other data validaton errors
        Invalid_Required_Value = 3  'like Transaction Code
        Attempt_To_Duplicate_Process = 4 'like when a previously executed process eg freight bill has already been processed
        Attempt_To_Duplicate_Process_NeeAction = 5 'Like When a previously executed process requires some action eg. a the overwrite Option On the AP Mass Entry Table should be selected
        Invalid_User_Name = 6
        Invalid_Password = 7
        NET_ADDRESS_FAILURE = 8 '(Database Is Not valid)
        Invalid_Value = 9 ' invalid value assigned based on parameter settings like Actual Carrier Assigned is not valid
    End Enum

    ''' <summary>
    ''' The NGLDateFilterType is used to determine which field is filtered using a Start and End Date Filter value.
    ''' Not all filters are supported for each inteface so the list must be managed on the UI side to limit access
    ''' to supported filters.
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 8/25/2016
    ''' </remarks>
    <DataContract()>
    Public Enum NGLDateFilterType
        <EnumMember(Value:="0")> None = 0
        <EnumMember(Value:="1")> DateAdded = 1
        <EnumMember(Value:="2")> DateSent = 2
        <EnumMember(Value:="3")> DateOrdered = 3
        <EnumMember(Value:="4")> DateLoad = 4
        <EnumMember(Value:="5")> DateRequired = 5
        <EnumMember(Value:="6")> DateRequested = 6
        <EnumMember(Value:="7")> DateModified = 7
    End Enum

    ''' <summary>
    ''' The NGLMessageKeyRef can be stored in the database as tblNGLMessageType.NMTControl
    ''' so besure to synchornize these values with the tblNGLMessageType table data
    ''' </summary>
    ''' <remarks></remarks>
    <DataContract()>
    Public Enum NGLMessageKeyRef
        <EnumMember(Value:="0")> NoneZero = 0
        <EnumMember(Value:="1")> None = 1
        <EnumMember(Value:="2")> CarrierTariff = 2
        <EnumMember(Value:="3")> Book = 3
        <EnumMember(Value:="4")> LoadPlanningTruck = 4 'Uses CompControl and TruckKey as fk filters
        <EnumMember(Value:="5")> LoadPlanningDetail = 5
        <EnumMember(Value:="6")> tblSolutionTruck = 6
        <EnumMember(Value:="7")> tblSolutionDetail = 7
        <EnumMember(Value:="8")> NGLAPIInfoMessages = 8
        <EnumMember(Value:="9")> NGLAPIWarnings = 9
        <EnumMember(Value:="10")> NGLAPIErrors = 10
    End Enum

    ''' <summary>
    ''' Enumerator to represent the tblLoadTenderTypes;  Must update source code if new types are added to database
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 02/06/2018
    ''' Modified by RHR for v-8.2 on 11/27/2018
    '''   added new LoadTenderType Spot Rate
    ''' </remarks>
    <DataContract()>
    Public Enum NGLLoadTenderTypes
        <EnumMember(Value:="0")> None = 0
        <EnumMember(Value:="1")> Manual = 1
        <EnumMember(Value:="2")> EDI204 = 2
        <EnumMember(Value:="3")> Web = 3
        <EnumMember(Value:="4")> CPU = 4
        <EnumMember(Value:="5")> DAT = 5
        <EnumMember(Value:="6")> NextStop = 6
        <EnumMember(Value:="7")> P44 = 7
        <EnumMember(Value:="8")> RateQuote = 8
        <EnumMember(Value:="9")> SpotRate = 9
    End Enum


    Public Enum ParmaterKeys
        UsePCMiler
        PCMilerDistanceType
        PCMilerRouteType
        AutoCorrectBadLaneZipCodes
    End Enum


    'Constants
    Public Const gcHTMLNEWLINE As String = vbCrLf & " < br /> "

    'Report Control Conts.
    'Standard < 1000
    'Custom > 1000
    'BI 3000 
    Public Const ONETHOUSAND = 1000
    Public Const TWOTHOUSAND = 2000
    Public Const THREETHOUSAND = 3000

    'Import/Export/Data Entry Types (Source)
    Public Const gcImportCarrier As Short = 0
    Public Const gcImportLane As Short = 1
    Public Const gcImportComp As Short = 2
    Public Const gcImportPayables As Short = 3
    Public Const gcImportSchedule As Short = 4
    Public Const gcimportFrtBill As Short = 5
    Public Const gcimportBook As Short = 6
    Public Const gcDataEntryFrtBill As Short = 0
    Public Const gcDataEntryFrtBillAudit As Short = 1
    Public Const gcExportAP As Short = 0
    Public Const gcIgnore As String = "4"
    Public Const gcHK As String = "3"
    Public Const gcFK As String = "2"
    Public Const gcPK As String = "1"
    Public Const gcNK As String = "0"

    'Error Constants
    Public Const gclngErrorNumber1 As Short = 20001
    Public Const gcstrErrorDesc1 As String = "Cannot locate database server."
    Public Const gclngErrorNumber2 As Short = 20002
    Public Const gcstrErrorDesc2 As String = "Null value not allowed."
    Public Const gclngErrorNumber3 As Short = 20003
    Public Const gcstrErrorDesc3 As String = "Invalid data type."
    Public Const gclngErrorNumber4 As Short = 20004
    Public Const gcstrErrorDesc4 As String = "Text data is too long."
    Public Const gclngErrorNumber5 As Short = 20005
    Public Const gcstrErrorDesc5 As String = "Unexpected error."

    Public Enum ValidateDataType As Integer
        vdtDate
        vdtLongInt
        vdtSmallInt
        vdtString
        vdtFloat
        vdtTinyInt
        vdtBit
        vdtMoney
        vdtTime
    End Enum

    Public Enum OptimizerTypes As Integer
        optTruckStops
        optPCMiler
        optFaxServer
        optany
    End Enum

    Public Enum ProcessDataReturnValues As Integer
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum

    Public Enum IntegrationTypes As Integer
        Carrier = 0
        Lane
        Company
        Payables
        Schedule
        LegacyFreightBillImport
        FreightBillImport
        Book
        PickList
        APExport
        FreightBillExport
        OpenPayables
        EDI214
        EDI204
        EDI990
    End Enum

    ''' <summary>
    ''' the different bracket and/or allocation types supported by the tariff rating engine
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
    ''' </remarks>
    Public Enum BracketType As Integer
        None = 0 'Used for Distance, Flat
        Pallets = 1 'Break or Allocate by number of pallets
        Volume = 2 'Break or Allocate by number of Cubes
        Quantity = 3 'Break or Allocate by number of Cases
        Lbs = 4 'Break or Allocate by number of Lbs
        Cwt = 5 'Break or Allocate by number of Cwt
        Distance = 6 'Break or Allocate by Distance like Per Kilometer
        Even = 7 'Break or Allocate evenly by number of orders
        FlatPallet = 8 'Allocate cost by single unit
    End Enum

    ''' <summary>
    ''' The different rating classes supported by the tariff matrix rating engine
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TariffClassType As Integer
        None = 0    '(used for distance, flat ect..)
        class49CFR = 1 'Class 49CFR
        classIATA = 2 'Class IATA
        classDOT = 3 'Class DOT
        classMarine = 4 'Class Marine
        classNMFC = 5 'Class NMFC
        classFAK = 6 'Class FAK
    End Enum

    ''' <summary>
    ''' The different modes of delivery supported by the tariff contract
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TariffModeType As Integer
        Air = 1 'Freight moved in the air
        Rail = 2 'Freight moved by rail 
        Road = 3 'Freight moved over the road
        Sea = 4 'Freight moved over water
        Service = 5 'Service Provider
    End Enum

    ''' <summary>
    ''' The different point types supported by the tariff engine
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum PointType As Integer
        Interline = 1 'Interline Point
        Direct = 2 'Direct Delivery Point
        Any = 3 'Any Point
    End Enum

    ''' <summary>
    ''' The different route types supported by the routing guide
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum RouteType As Integer
        FullLoad = 1 'This represents an order or load that is typically shipped independently of other orders
        MultiPick = 2 'This represents an order or load that is typically part of a multi-pick route
        Consolidated = 3 'This represents an order or load that is typically combined with other orders to create a consolidation on the same equipment
        LTLPool = 4 'This represents an order or load that is shipped as LTL but grouped with a common Load or consolidation number and shipped on the same equipment
        SingleLTL = 5 'This represents an order that is shipped as LTL with a unique route or consolidation number for each destination
        Unassigned = 6 'The selected record has not been assigned a route type
        Hold = 7 'The selected record is on hold and should not be routed at this time
    End Enum

    ''' <summary>
    ''' The different tariff types supported by the tariff contract
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TariffType As Integer
        PublicTariff = 1 'Public Tariff
        PrivateTariff = 2 'Private Tariff
    End Enum

    ''' <summary>
    ''' The different rate types supported by the tariff matrix rating engine
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TariffRateType As Integer
        DistanceM = 1 'Rates per Mile
        DistanceK = 2 'Rates per Kilometer
        ClassRate = 3 'Class Rates based on Class Type like FAK
        FlatRate = 4 'Flat Rates
        UnitOfMeasure = 5 'Rates based on units like pallets or cubes
        CzarLite = 6 'Rates based on CzarLite Data
    End Enum

    ''' <summary>
    ''' The different temperature types supported by the tariff contracts
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TariffTempType As Integer
        Any = 0
        Dry = 1
        Frozen = 2
        Refrigerated = 3
    End Enum

    ''' <summary>
    ''' Determines how fees are allocated based on BracketType
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FeeAllocationType As Integer
        None = 1 'Not Allocated
        Origin = 2 'Allocate By Origin
        Destination = 3 'Allocate By Destination
        Load = 4 'Allocate By Load
    End Enum

    ''' <summary>
    ''' Determines how fees are calculated the default priority is Order, Lane then Carrier
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FeeCalcType As Integer
        All = 1 'Combine All Fees
        Unique = 2 'Override Duplicate Fees At Order Level
        CarrierOverLane = 3 'Carrier Fees Override Lane and Combine With Order Fees
        LaneOverCarrier = 4 'Lane Fees Override Carrier and Combine With Order Fees
        CarrierOverALL = 5 'Carrier Fees Override All Fees
        LaneOverALL = 6 'Lane Fees Override All Fees
    End Enum

    Public Enum LookUpListEnum
        UOM
        TempType
        LaneTran
        State
        CarrierEquipCode
        Seasonality
        CreditCardType
        PaymentForm
        Currency
        LoadType
        Lane
        Carrier
        Comp
        PalletType
        tblFormList
        tblFormMenu
        tblProcedureList
        tblReportList
        tblReportPar
        tblReportMenu
        tblReportParType
        tblUserSecurity
        NatAcctNumber
        CarrierActive
        APAdjReason
        ComCodes
        PayCodes
        TranCodes
        ChartOfAcounts
        LoadStatusCodes
        NegativeRevenueReason
        ARCompany
        APCarrier
        APActiveCarrier
        APCompany
        APCarrierPaid
        APCarrierAmtPaid
        tblBracketType
        AccessorialVariableCodes
        LaneNonRestrictedCarriers
        LaneRestrictedCarriers
        CarrAdHoc
        LaneActive
        CompActive
        tblParCategories
        TariffTempType
        TariffType
        ImportFileType
        tblRouteTypes
        tblStaticRoutes
        CarrierEquipment
        CapacityPreference
        ActionType
        tblAttribute
        tblAttributeType
        tblAction
        LaneCrossDock
        ColorCodeType
        ApptStatusColorCodeKey
        ApptTypeColorCodeKey
        CompNEXTrack
        TariffShipper
        tblTarBracketType
        tblTarRateType
        tblTarAgent
        tblTariffType
        tblModeType
        tblPointType
        tblClassType
        AccessorialFeeCalcType
        AccessorialFeeAllocationType
        AccessorialFeeType
        LaneByWarehouse
        LaneActiveByWarehouse
    End Enum

    ''' <summary>
    ''' Determines how fees are calculated 
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FeeVariableCode As Integer
        LoadWeight = 1 'Multiply Variable Value By Total Weight Based on Allocation Rules
        NumberPallets = 2 'Multiply Variable Value By Number of Pallets Based on Allocation Rules
        CarrierCost = 3 'Multiply Variable Value By Carrier Cost (normally used as a percentage)  Based on Allocation Rules
        PerMile = 4 'Multiply Variable Value By Number of Miles  Based on Allocation Rules
        ByVolume = 5 'Multiply Variable Value By Total Volume or Cubes Based on Allocation Rules
        ByQuantity = 6 'Multiply Variable Value By Total Quantity or Cases Based on Allocation Rules
        Stop1Flat = 7 'Fee Add Minimum Value To Fees If Load Is Stop 1 or Zero
        Stop2Flat = 8 'Fee Add Minimum Value To Fees If Load Is Stop 2
        Stop3Flat = 9 'Fee Add Minimum Value To Fees If Load Is Stop 3
        Stop4Flat = 10 'Fee Add Minimum Value To Fees If Load Is Stop 4
        Stop5Flat = 11 'Fee Add Minimum Value To Fees If Load Is Stop 5
        Stop6Flat = 12 'Fee	Add Minimum Value To Fees If Load Is Stop 6
        Stop7Flat = 13 'Fee	Add Minimum Value To Fees If Load Is Stop 7
        Stop8Flat = 14 'Fee	Add Minimum Value To Fees If Load Is Stop 8
        Stop9Flat = 15 'Fee	Add Minimum Value To Fees If Load Is Stop 9
        Stop10Flat = 16 'Fee Add Minimum Value To Fees If Load Is Stop 10
        Pick1Flat = 17 'Fee	Add Minimum Value To Fees If Load Is Pick 1 or Zero
        Pick2Flat = 18 'Fee	Add Minimum Value To Fees If Load Is Pick 2
        Pick3Flat = 19 'Fee	Add Minimum Value To Fees If Load Is Pick 3
        Pick4Flat = 20 'Fee	Add Minimum Value To Fees If Load Is Pick 4
        Pick5Flat = 21 'Fee	Add Minimum Value To Fees If Load Is Pick 5
        Pick6Flat = 22 'Fee	Add Minimum Value To Fees If Load Is Pick 6
        Pick7Flat = 23 'Fee Add Minimum Value To Fees If Load Is Pick 7
        Pick8Flat = 24 'Fee Add Minimum Value To Fees If Load Is Pick 8
        Pick9Flat = 25 'Fee Add Minimum Value To Fees If Load Is Pick 9
        Pick10Flat = 26 'Fee Add Minimum Value To Fees If Load Is Pick 10
    End Enum

    ''' <summary>
    ''' Determines the priority of fees for each booking record 
    ''' and how they were originally assigned to the load
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AccessorialFeeType As Integer
        Tariff = 1 'Carrier Tariff Specific Fee
        Lane = 2 'Lane Specific Fee
        Order = 3 'Order Specific Fee
    End Enum

    Public Enum AssignCarrierCalculationType As Integer
        Normal 'Select lowest cost tariff and execute assigned carrier logic BookTranCode of 'N','P', & 'PC'
        UpdateAssignedCarrier 'execute assigned carrier logic to select the correct tariff BookTranCode of 'N','P','PC', & 'PB' Ok
        UpdateCarrier 'Use Existing Carrier Tariff and recalculate all costs including fees and discounts BookTranCode of 'N','P', & 'PC'
        Recalculate 'Use current carrier cost (no pivot needed) to update fees and BFC only BookTranCode of IC is restricted
        RecalcuateNoBFC 'Use current carrier cost (no pivot needed) to update fees no BFC only BookTranCode of IC is restricted
        RecalcuateSpotRate
        RateShopOnly
    End Enum

    ''' <summary>
    ''' Flags the Fee with an override code used to provide information
    ''' about why the fee was overridden
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AccessorialFeeOverRideReasonCode As Integer
        None = 1
        MissingRouteDependency
        MissingLaneDependency
        MissingOrderDependency
        UserOverRidden
        SystemOverRidden
        DependentUponLane
        DependentUponRoute
        DependentUponOrder
        UserAdded
        SystemAdded
        SystemReversed
    End Enum

    ''' <summary>
    ''' links a fee to a dependency key field used to determine
    ''' if the fee is valid.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AccessorialFeeDependencyType As Integer
        None = 1
        Route
        Lane
        Order
    End Enum

    Private Shared _ConfigDataRows As DataRow()
    Friend Shared Property ConfigDataRows As DataRow()
        Get
            Return _ConfigDataRows
        End Get
        Set(value As DataRow())
            _ConfigDataRows = value
        End Set
    End Property

    Friend Shared AdminEmail As String
    Friend Shared GroupEmail As String
    Friend Shared FromEmail As String
    Friend Shared SMTPServer As String

    Public Shared Sub GetEmailParameters(ByVal ConnectionString As String)

        Dim oSysData As New NGLSystemDataProvider(ConnectionString)
        Dim oGTPs As DataTransferObjects.GlobalTaskParameters = oSysData.GetGlobalTaskParameters
        If Not oGTPs Is Nothing Then
            With oGTPs
                AdminEmail = .GlobalAdminEmail
                GroupEmail = .GlobalGroupEmail
                FromEmail = .GlobalFromEmail
                SMTPServer = .GlobalSMTPServer
            End With
        End If
    End Sub

    Public Shared Sub SaveAppError(ByVal Message As String, ByVal parameters As WCFParameters)
        Dim oSys As New NGLSystemDataProvider(parameters)
        Try
            oSys.CreateAppErrorByMessage(Message, parameters.UserName)
        Catch ex As Exception
            'we ignore all errors while saving application error data
            'Console.WriteLine("Unexpected Exception: " & ex.Message)
        End Try

    End Sub


    Public Shared Sub SaveAppError(ByRef ex As Exception, ByVal parameters As WCFParameters)
        Dim oSys As New NGLSystemDataProvider(parameters)
        Try
            oSys.CreateAppErrorByMessage(ex.ToString, parameters.UserName)
        Catch e As Exception
            'we ignore all errors while saving application error data
            'Console.WriteLine("Unexpected Exception: " & ex.Message)
        End Try

    End Sub

    Public Shared Sub SaveSysError(ByVal pars As sysErrorParameters, ByVal parameters As WCFParameters)
        Dim oSys As New NGLSystemDataProvider(parameters)
        Try
            With pars
                oSys.CreateSystemErrorByMessage(.Message, parameters.UserName, .Procedure, .Record, .Number, .Severity, .ErrState, .LineNber)

            End With
        Catch ex As Exception
            'we ignore all errors while saving application error data
            'Console.WriteLine("Unexpected Exception: " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="parameters"></param>
    ''' <param name="SendToGroup"></param>
    ''' <param name="CopyAdmin"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' TODO: update logic to use email localization properties if available
    ''' </remarks>
    Public Shared Function SendToNGLEmailService(ByVal Subject As String,
                                ByVal Body As String,
                                ByVal parameters As WCFParameters,
                                Optional SendToGroup As Boolean = True,
                                Optional ByVal CopyAdmin As Boolean = True) As Boolean

        Try
            Dim strSQLAuthUser As String = readConfigSettings("SQLAuthUser").Trim
            Dim strSQLAuthPass As String = readConfigSettings("SQLAuthPass").Trim
            Dim connectionstring As String = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", parameters.DBServer.Trim, parameters.Database.Trim, strSQLAuthUser, strSQLAuthPass)
            If String.IsNullOrEmpty(AdminEmail) Then GetEmailParameters(connectionstring)
            If Not SendToGroup Then
                Return SendToNGLEmailService(FromEmail, AdminEmail, "", Subject, Body, New NGLBatchProcessDataProvider(parameters))
            ElseIf CopyAdmin Then
                Return SendToNGLEmailService(FromEmail, GroupEmail, AdminEmail, Subject, Body, New NGLBatchProcessDataProvider(parameters))
            Else
                Return SendToNGLEmailService(FromEmail, GroupEmail, "", Subject, Body, New NGLBatchProcessDataProvider(parameters))
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="MailFrom"></param>
    ''' <param name="EmailTo"></param>
    ''' <param name="CCEmail"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="parameters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' TODO: update logic to use email localization properties if available
    ''' </remarks>
    Public Shared Function SendToNGLEmailService(ByVal MailFrom As String,
                                ByVal EmailTo As String,
                                ByVal CCEmail As String,
                                ByVal Subject As String,
                                ByVal Body As String,
                                ByVal parameters As WCFParameters) As Boolean

        Return SendToNGLEmailService(MailFrom, EmailTo, CCEmail, Subject, Body, New NGLBatchProcessDataProvider(parameters))

    End Function



    Public Shared Function SendToNGLEmailService(ByVal MailFrom As String,
                                ByVal EmailTo As String,
                                ByVal CCEmail As String,
                                ByVal Subject As String,
                                ByVal Body As String,
                                ByRef oBatchProcessing As NGLBatchProcessDataProvider) As Boolean
        Dim blnRet As Boolean = False

        Try
            blnRet = oBatchProcessing.executeGenerateEmail2Way(MailFrom, EmailTo, CCEmail, Subject, Body)

        Catch ex As Exception
            'ignore any errors while generating the email for async batch processing; there is no where to go from here.
        End Try
        Return blnRet
    End Function


    Public Shared Function getImportFileName(ByVal enmVal As IntegrationTypes) As String
        Select Case enmVal
            Case IntegrationTypes.APExport
                Return "APExport"
            Case IntegrationTypes.Book
                Return "Book"
            Case IntegrationTypes.Carrier
                Return "Carrier"
            Case IntegrationTypes.Company
                Return "Company"
            Case IntegrationTypes.FreightBillExport
                Return "FreightBillExport"
            Case IntegrationTypes.FreightBillImport
                Return "FreightBillImport"
            Case IntegrationTypes.Lane
                Return "Lane"
            Case IntegrationTypes.LegacyFreightBillImport
                Return "LegacyFreightBillImport"
            Case IntegrationTypes.OpenPayables
                Return "OpenPayables"
            Case IntegrationTypes.Payables
                Return "Payables"
            Case IntegrationTypes.PickList
                Return "PickList"
            Case IntegrationTypes.Schedule
                Return "Schedule"
            Case IntegrationTypes.EDI214
                Return "EDI214"
            Case IntegrationTypes.EDI204
                Return "EDI204"
            Case IntegrationTypes.EDI990
                Return "EDI990"
            Case Else
                Return ""
        End Select
    End Function

    Public Shared Function validateSQLValue(ByRef oField As Object,
                                            ByVal enumDataType As ValidateDataType,
                                            Optional ByVal strSource As String = "NGL.FreightMaster.Integration",
                                            Optional ByVal strErrMsg As String = "",
                                            Optional ByVal blnAllowNull As Boolean = False,
                                            Optional ByVal intLength As Short = 1) As String

        Dim lngErrorNumber As Integer = 0
        Dim strErrdesc As String = ""
        Dim lngval As Integer = 0
        Dim intVal As Short = 0
        Dim blnVal As Boolean = False
        Dim dtVal As Date
        Dim dblVal As Double = 0
        Dim curVal As Decimal = 0
        Dim stryear As String = ""
        Dim strmonth As String
        Dim strday As String
        Dim strhour As String
        Dim strminute As String
        Dim strReturn As String = "''"


        Try
            If enumDataType = ValidateDataType.vdtDate Then
                'test for date value
                If IsDBNull(oField) Or Val(DTran.NZ(oField, "0")) = 0 Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf IsDate(oField) Then
                    Return "'" & oField.ToString & "'"
                Else
                    If InStr(1, oField.ToString, "-") Or InStr(1, oField.Value, "\") Then
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                    Else
                        stryear = Right(oField.ToString, 4)
                        strReturn = Left(oField.ToString, Len(oField.Value) - 4)
                        strday = Right(strReturn, 2)
                        strmonth = Left(strReturn, Len(strReturn) - 2)
                        If Not validateDate(strmonth & "/" & strday & "/" & stryear, "M/d/yyyy", dtVal) Then
                            Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                        End If
                        Return "'" & dtVal & "'"
                    End If
                End If
            ElseIf enumDataType = ValidateDataType.vdtTime Then
                'test for Time value
                If IsDBNull(oField) Or Val(DTran.NZ(oField, "0")) = 0 Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf IsDate(oField) Then
                    Return "'" & oField.ToString & "'"
                Else
                    If InStr(1, oField.ToString, "-") Or InStr(1, oField.ToString, "\") Or InStr(1, oField.ToString, ":") Then
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                    Else
                        strminute = Right(oField.ToString, 2)
                        strhour = Left(oField.ToString, 2)
                        If Not DateTime.TryParse("1/1/2000 " & strhour & ":" & strminute, dtVal) Then
                            Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                        End If
                        Return "'" & dtVal.ToShortTimeString & "'"
                    End If
                End If
            ElseIf enumDataType = ValidateDataType.vdtFloat Then
                'test for float value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Double.TryParse(oField.ToString, dblVal) Then
                    Return dblVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtMoney Then
                'test for currency value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Double.TryParse(oField.ToString, dblVal) Then
                    curVal = dblVal
                    Return curVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtLongInt Then
                'test for long integer value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Integer.TryParse(oField.ToString, lngval) Then
                    Return lngval.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtSmallInt Then
                'test for small integer value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Short.TryParse(oField.ToString, intVal) Then
                    Return intVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtTinyInt Then
                'test for tiny integer value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Short.TryParse(oField.ToString, intVal) Then
                    If intVal < 0 Or intVal > 255 Then
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                    End If
                    Return intVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtBit Then
                'test for Bit integer value

                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Boolean.TryParse(oField.ToString, blnVal) Then
                    Select Case blnVal
                        Case True
                            intVal = 1
                        Case False
                            intVal = 0
                        Case Else
                            Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                    End Select
                    Return intVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            Else
                'we test for a string (text) value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Len(oField.ToString) <= intLength Then
                    Return "'" & DTran.padQuotes(oField.ToString) & "'"
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            End If
        Catch ex As System.ApplicationException
            Throw
        Catch ex As System.InvalidCastException
            Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & ex.Message & " " & strErrMsg)
        Catch ex As Exception
            Throw New System.ApplicationException("Error # " & gclngErrorNumber5.ToString & ". " & gcstrErrorDesc5 & " " & ex.Message & " " & strErrMsg)
        End Try
        Return strReturn

    End Function


    ''' <summary>
    ''' Test postalCode using sCountry any modified format is returned in sZipOut returns true if valid false if no match
    ''' </summary>
    ''' <param name="postalCode"></param>
    ''' <param name="sCountry"></param>
    ''' <param name="sZipOut"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Created by RHR for v-8.4.0.002 on 04/14/2021
    '''     new function to test the format of postal codes for P44 API
    '''     if sCountry is null or Empty US is used
    ''' </remarks>
    Public Shared Function validatePostalCodeForAPI(ByVal postalCode As String, ByVal sCountry As String, ByRef sZipOut As String) As Boolean
        Dim blnRet As Boolean = False
        If String.IsNullOrWhiteSpace(postalCode) Then
            sZipOut = Nothing
            Return False
        End If

        postalCode = postalCode.Trim()
        If String.IsNullOrWhiteSpace(sCountry) Then sCountry = "US"
        If Len(sCountry.Trim()) > 2 Then sCountry = Left(sCountry.Trim(), 2)

        If sCountry.ToUpper() = "CA" Then
            blnRet = Regex.IsMatch(postalCode, "/^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ]( )?\d[ABCEGHJKLMNPRSTVWXYZ]\d$/i")
            sZipOut = postalCode
        Else
            blnRet = Regex.IsMatch(postalCode, "^\\d{5}(-{0,1}\\d{4})?$")
            sZipOut = Left(postalCode, 5)
        End If

        Return blnRet
    End Function

    Public Shared Function validateDate(ByVal strDate As String, ByVal strFormat As String, ByRef dtVal As Date) As Boolean
        Dim blnRet As Boolean = False
        Try
            dtVal = DateTime.ParseExact(strDate, strFormat, Nothing).ToString
            Return True
        Catch ex As System.ArgumentNullException
            Return False
        Catch ex As System.FormatException
            Return False
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Shared Function validateDateWS(ByVal strDate As String, ByRef dtVal As Date) As Boolean

        Dim blnRet As Boolean = False
        Try

            'test for date value
            If IsDBNull(strDate) Or Val(DTran.NZ(strDate, "0")) = 0 Then
                Return False
            ElseIf IsDate(strDate) Then
                dtVal = CDate(strDate)
                Return True
            Else
                strDate = Trim(strDate)
                If InStr(1, strDate, "-") Or InStr(1, strDate, "\") Then
                    Return False
                Else
                    If Len(strDate) <> 8 Then
                        Return False
                    Else
                        Dim strYear As String = Right(strDate, 4)
                        Dim strReturn As String = Left(strDate, Len(strDate) - 4)
                        Dim strDay As String = Right(strReturn, 2)
                        Dim strMonth = Left(strReturn, Len(strReturn) - 2)
                        strReturn = strMonth & "/" & strDay & "/" & strYear
                        If validateDate(strReturn, "M/d/yyyy", dtVal) Then  'default is short date
                            Return True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
        Return blnRet

    End Function

    Public Shared Function ValidateTimeWS(ByRef strVal As String, Optional ByVal strDefault As String = "") As Boolean
        Dim blnRet As Boolean = False
        Dim strhour As String
        Dim strminute As String
        Dim strseconds As String
        Dim dtVal As Date

        'test for Time value
        If IsDBNull(strVal) Or Val(DTran.NZ(strVal, "0")) = 0 Then
            If Not String.IsNullOrEmpty(strDefault.Trim) AndAlso IsDate(strDefault) Then
                strVal = strDefault
                blnRet = True
            End If
        ElseIf IsDate(strVal) Then
            blnRet = True
        Else
            If InStr(1, strVal, "-") Or InStr(1, strVal, "\") Or InStr(1, strVal, ":") Then
                'the IsDate function should have worked on a valid time string
                blnRet = False
            ElseIf strVal.Length = 4 Then
                'convert the time format HHmm to a string containing the name of the day of the week, 
                'the name of the month, the numeric day of the hours, minutes equivalent 
                'to the time value of this instance using the date Jan 1st 2000 
                strminute = Right(strVal.ToString, 2)
                strhour = Left(strVal.ToString, 2)
                If DateTime.TryParse("1/1/2000 " & strhour & ":" & strminute, dtVal) Then
                    strVal = dtVal.ToShortTimeString
                    blnRet = True
                End If
            ElseIf strVal.Length = 8 Then
                'convert the time format HHmmss to a string containing the name of the day of the week, 
                'the name of the month, the numeric day of the hours, minutes, and seconds equivalent 
                'to the time value of this instance using the date Jan 1st 2000
                strseconds = Right(strVal.ToString, 2)
                strminute = strVal.Substring(2, 2)
                strhour = Left(strVal.ToString, 2)
                If DateTime.TryParse("1/1/2000 " & strhour & ":" & strminute & ":" & strseconds, dtVal) Then
                    strVal = dtVal.ToShortTimeString
                    blnRet = True
                End If
            Else
                blnRet = False
            End If
        End If
        Return blnRet
    End Function

    Friend Shared Function readConfigSettings(ByVal Setting As String) As String
        Dim strRet As String = ""
        If Not ConfigDataRows Is Nothing AndAlso ConfigDataRows.Length > 0 Then
            strRet = ConfigDataRows(0)(Setting).ToString
        Else
            Dim dsXML As New System.Data.DataSet ' Data.DataSet
            Dim strPath As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString()
            If Right(strPath, 1) <> "\" Then strPath &= "\"
            strPath &= "bin\config.xml"
            If Not System.IO.File.Exists(strPath) Then
                'try the current path typically used when accessing the DLL directly (not via WCF)
                strPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString()
                If Right(strPath, 1) <> "\" Then strPath &= "\"
                strPath &= "config.xml"
                If Not System.IO.File.Exists(strPath) Then Throw New ApplicationException("Cannot read configuration settings in application folder or application bin folder.")
            End If
            dsXML.ReadXmlSchema(strPath)
            dsXML.ReadXml(strPath)

            Dim oTable As DataTable = dsXML.Tables(0)
            'Dim oDRows As DataRow() = oTable.Select()
            ConfigDataRows = oTable.Select()
            If ConfigDataRows.Length > 0 Then
                strRet = ConfigDataRows(0)(Setting).ToString
            Else
                Throw New ApplicationException("Cannot read configuration settings.")
            End If
        End If
        Return strRet
    End Function

    ''' <summary>
    ''' checks if the str can be converted to a date, returns dtDefault if not,  if dtDefault is not provided uses Date.now as default
    ''' </summary>
    ''' <param name="strDate"></param>
    ''' <param name="dtDefault"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR vor v-8.1 on 02/08/2018
    ''' Modified by RHR for v-8.2.0.119 on 09/26/2019
    '''   added logic to test for sql min and sql max dates
    ''' </remarks>
    Public Shared Function returnDateFromString(ByVal strDate As String, Optional ByVal dtDefault As Date? = Nothing) As Date

        If Not dtDefault.HasValue Then dtDefault = Date.Now
        Dim dtTest As Date = Date.Now
        Dim dtSQLMin As Date = "01/01/1754"
        Dim dtSQLMax As Date = "12/31/9998"
        If Not String.IsNullOrWhiteSpace(strDate) Then
            If Date.TryParse(strDate, dtTest) Then
                dtDefault = dtTest
            End If
        End If
        If dtDefault.Value < dtSQLMin Or dtDefault.Value > dtSQLMax Then dtDefault = Date.Now

        Return dtDefault

    End Function

    Public Shared Function UpperFirst(ByVal s As String) As String
        Dim strRet As String = ""
        Dim myDeligate As System.Text.RegularExpressions.MatchEvaluator = New System.Text.RegularExpressions.MatchEvaluator(AddressOf DoMatch)
        strRet = System.Text.RegularExpressions.Regex.Replace(s, "\b[a-z]\w+", myDeligate)
        Return strRet
    End Function

    Public Shared Function DoMatch(ByVal m As System.Text.RegularExpressions.Match) As String
        Dim v As String = m.ToString
        Return Char.ToUpper(v(0)) + v.Substring(1)
    End Function

#Region "  Email Enum Processing"

    Public Enum EmailLocalizationTypesEnum
        LoadAccepted
        LoadManuallyAccepted
        LoadChangesAccepted
        LoadRejected
        LoadManuallyRejected
        LoadChangesRejected
        LoadExpired
        LoadUnfinalized
        LoadAutoTendered
        LoadManuallyTendered
        LoadDropped
        LoadManuallyDropped
        LoadUnassigned
        LoadModifyUnaccepted
    End Enum

    Public Enum EmailBodyLocalizedEnum
        EBody_LoadAccepted ' "A load with SHID Number {0} and Consolidation Number {1} was accepted by Carrier {2} -- {3} <br><br>Carrier Comments:<br>{4}"
        EBody_LoadManuallyAccepted ' "A load with SHID Number {0} and Consolidation Number {1} was manually accepted for Carrier {2} -- {3} via Fax, Email, or phone."
        EBody_LoadChangesAccepted '"Changes to a load with SHID Number {0} and Consolidation Number {1} were accepted by Carrier {2} -- {3} <br><br>Carrier Comments:<br>{4}"
        EBody_LoadRejected '"A load with SHID Number {0} and Consolidation Number {1} was rejected by Carrier {2} -- {3} <br><br>Carrier Comments:<br>{4}"
        EBody_LoadManuallyRejected '"A load with SHID Number {0} and Consolidation Number {1} was manually rejected for Carrier {2} -- {3} via Fax, Email, or phone."
        EBody_LoadChangesRejected '"Changes to a load with SHID Number {0} and Consolidation Number {1} were rejected by Carrier {2} -- {3} <br><br>Carrier Comments:<br>{4}"
        EBody_LoadExpired '"A load with SHID Number {0} and Consolidation Number {1} expired without response from Carrier {2} -- {3} <br><br>Comments:<br>{4}"
        EBody_LoadUnfinalized '"A load with SHID Number {0} and Consolidation Number {1} assigned to Carrier {2} -- {3} has been unfinalized and is being modified.  You should expect an update to be transmitted."
        EBody_LoadAutoTendered '"A load with SHID Number {0} and Consolidation Number {1} has been automatically tendered to Carrier {2} -- {3}.  Please see the attached PDF file for details.  To accept or reject the load, please respond with an EDI 990; visit the NEXTrack portal or contact Operations per your current accept/reject procedures.  This load may have an expiration."        
        EBody_LoadManuallyTendered '"A load with SHID Number {0} and Consolidation Number {1} has been manually tendered to Carrier {2} -- {3}.  Please see the attached PDF file for details.  To accept or reject the load, please respond with an EDI 990; visit the NEXTrack portal or contact Operations per your current accept/reject procedures.  This load may have an expiration."        
        EBody_LoadDropped '"A load with SHID Number {0} and Consolidation Number {1} was dropped by Carrier {2} -- {3} <br><br>Comments:<br>{4}"
        EBody_LoadManuallyDropped '"A load with SHID Number {0} and Consolidation Number {1} was manually dropped for Carrier {2} -- {3} via Fax, Email, or phone."
        EBody_LoadManuallyUnassigned '"A load with SHID Number {0} and Consolidation Number {1} was manually unassigned for Carrier {2} -- {3} via Fax, Email, or phone."
        EBody_LoadModifyUnaccepted '"A load is being modified after it was tendered but before it was accepted by Carrier {2} -- {3}.  For SHID Number {0} and Consolidation Number {1}. You should expect an update to be transmitted. "
    End Enum

    Public Enum EmailSubjectLocalizedEnum
        ESubject_LoadAccepted '"Load Accepted SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadChangesAccepted '"Changes to Load Accepted SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadRejected '"Load Rejected SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadChangesRejected '"Changes to Load Rejected SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadExpired '"Load Expired SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadUnfinalized '"Load Unfinalized SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadTendered '"Load Tendered SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadDropped '"Load Dropped SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadUnassigned '"Load Unassigned SHID Number {0}; CNS {1}; Carrier {2}"
        ESubject_LoadModifyUnaccepted '"Load is Being Modified SHID Number {0}; CNS {1}; Carrier {2}"
    End Enum

    Public Shared Function getEmailBodyNotLocalizedString(ByVal item As EmailBodyLocalizedEnum, Optional ByVal sdefault As String = "N/A") As String
        Dim strReturn = sdefault
        Try
            Select Case item
                Case EmailBodyLocalizedEnum.EBody_LoadAccepted
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} was accepted by Carrier {2} -- {3} <br><br>Carrier Comments:<br>{4}"
                Case EmailBodyLocalizedEnum.EBody_LoadManuallyAccepted
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} was manually accepted for Carrier {2} -- {3} via Fax, Email, or phone."
                Case EmailBodyLocalizedEnum.EBody_LoadChangesAccepted
                    strReturn = "Changes to a load with SHID Number {0} and Consolidation Number {1} were accepted by Carrier {2} -- {3} <br><br>Carrier Comments:<br>{4}"
                Case EmailBodyLocalizedEnum.EBody_LoadRejected
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} was rejected by Carrier {2} -- {3} <br><br>Carrier Comments:<br>{4}"
                Case EmailBodyLocalizedEnum.EBody_LoadManuallyRejected
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} was manually rejected for Carrier {2} -- {3} via Fax, Email, or phone."
                Case EmailBodyLocalizedEnum.EBody_LoadChangesRejected
                    strReturn = "Changes to a load with SHID Number {0} and Consolidation Number {1} were rejected by Carrier {2} -- {3} <br><br>Carrier Comments:<br>{4}"
                Case EmailBodyLocalizedEnum.EBody_LoadExpired
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} expired without response from Carrier {2} -- {3} <br><br>Comments:<br>{4}"
                Case EmailBodyLocalizedEnum.EBody_LoadUnfinalized
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} assigned to Carrier {2} -- {3} has been unfinalized and is being modified.  You should expect an update to be transmitted."
                Case EmailBodyLocalizedEnum.EBody_LoadAutoTendered
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} has been automatically tendered to Carrier {2} -- {3}.  Please see the attached PDF file for details.  To accept or reject the load, please respond with an EDI 990; visit the NEXTrack portal or contact Operations per your current accept/reject procedures.  This load may have an expiration."
                Case EmailBodyLocalizedEnum.EBody_LoadManuallyTendered
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} has been manually tendered to Carrier {2} -- {3}.  Please see the attached PDF file for details.  To accept or reject the load, please respond with an EDI 990; visit the NEXTrack portal or contact Operations per your current accept/reject procedures.  This load may have an expiration."
                Case EmailBodyLocalizedEnum.EBody_LoadDropped
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} was dropped by Carrier {2} -- {3} <br><br>Comments:<br>{4}"
                Case EmailBodyLocalizedEnum.EBody_LoadManuallyDropped
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} was manually dropped for Carrier {2} -- {3} via Fax, Email, or phone."
                Case EmailBodyLocalizedEnum.EBody_LoadManuallyUnassigned
                    strReturn = "A load with SHID Number {0} and Consolidation Number {1} was manually unassigned for Carrier {2} -- {3} via Fax, Email, or phone."
                Case EmailBodyLocalizedEnum.EBody_LoadModifyUnaccepted
                    strReturn = "A load is being modified after it was tendered but before it was accepted by Carrier {2} -- {3}.  For SHID Number {0} and Consolidation Number {1}. You should expect an update to be transmitted. "
            End Select
            Return strReturn
        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Throw
        End Try

        Return strReturn
    End Function

    Public Shared Function getEmailSubjectNotLocalizedString(ByVal item As EmailSubjectLocalizedEnum, Optional ByVal sdefault As String = "NGL System Message") As String
        Dim strReturn = sdefault
        Try
            Select Case item
                Case EmailSubjectLocalizedEnum.ESubject_LoadAccepted
                    strReturn = "Load Accepted SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadChangesAccepted
                    strReturn = "Changes to Load Accepted SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadRejected
                    strReturn = "Load Rejected SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadChangesRejected
                    strReturn = "Changes to Load Rejected SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadExpired
                    strReturn = "Load Expired SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadUnfinalized
                    strReturn = "Load Unfinalized SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadTendered
                    strReturn = "Load Tendered SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadDropped
                    strReturn = "Load Dropped SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadUnassigned
                    strReturn = "Load Unassigned SHID Number {0}; CNS {1}; Carrier {2}"
                Case EmailSubjectLocalizedEnum.ESubject_LoadModifyUnaccepted
                    strReturn = "Load is Being Modified SHID Number {0}; CNS {1}; Carrier {2}"
            End Select
            Return strReturn
        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Throw
        End Try

        Return strReturn
    End Function

    Public Shared Function getEmailBodyLocalizedString(ByVal item As EmailBodyLocalizedEnum, Optional ByVal sdefault As String = "N/A") As String
        Dim strReturn = sdefault
        Try
            Dim Enumerator As Type = GetType(EmailBodyLocalizedEnum)
            strReturn = [Enum].GetName(Enumerator, item)
        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Throw
        End Try

        Return strReturn
    End Function

    Public Shared Function getEmailSubjectLocalizedString(ByVal item As EmailSubjectLocalizedEnum, Optional ByVal sdefault As String = "N/A") As String
        Dim strReturn = sdefault
        Try
            Dim Enumerator As Type = GetType(EmailSubjectLocalizedEnum)
            strReturn = [Enum].GetName(Enumerator, item)
        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Throw
        End Try

        Return strReturn
    End Function


#End Region


#Region "Tariff Import/Export Processing"

    Public Enum TariffLocalizationTypesEnum
        ImportWorkBookRejected 'The workbook is rejected, there are too many problems..
        UnableImportRateCloned 'Unable to Import Rates, the tariff selected has already been cloned at least once.
        ImportTariffCouldNotParseKeyData 'The worksheet with name: {0} was not able to be imported, could not parse key data fields..
        ImportTariffCouldNotKeyDataInvalid 'The worksheet with name: {0} was not able to be imported, the key data fields are invalid..
        ImportTariffCouldNotParseBreakPoints ' The worksheet with name: {0} was not able to be imported, could not parse break points..
        ImportTariffCouldNotInvalidRates 'The worksheet with name: {0} was not able to be imported, some rates where invalid..
        tariffSqlFaultProblem 'There was a SqlFaultInfo problem.  {0}
        tariffUnknownProblem 'There was a major problem. {0}
        ETariffWorkSheetContractDataNotFound ' The worksheet with name: {0} was not able to be imported, the contract data was not found in the worksheet.
        ETariffContractClonedPrevously 'The worksheet with name: {0} was not able to be imported. The contract has been cloned previously.
        ETariffContractClonedUnknown ' The worksheet with name: {0} was not able to be imported, Unable to detemine if the contract has been cloned previously.
        ETariffWorkSheetEquipDataNotFound ' The worksheet with name: {0} was not able to be imported, the equipment data was not found in the worksheet.
        ETariffWorkSheetContractNotFound ' The worksheet with name: {0} was not able to be imported, the contract in the Excel sheet was not found in the database.
        ETariffWorkSheetContractRateNotFound ' The worksheet with name: {0} was not able to be imported, the rate name(CarrTarEquipMatName) in the Excel sheet was not found in the database.
        EImportTariffRateTypeNotvalid '{0} {1} RateTypeControl is not valid.
        EImportTariffTariffControlNotvalid '{0} {1} TariffControl is not valid.
        EImportTariffTTempTypeNotvalid ' {0} {1} CarrTarTempType is not valid.
        EImportTariffTCarrierControlNotvalid ' {0} {1} Carrrier Control is not valid.
        EImportTariffTItemNotvalid ' {0} {1} {2} is not valid.
        EImportTariffComFailure
        EImportTariffExtractRatesUnknown ' Please correct the problem if possible and try again. {0}
        EImportCloneFailedEmptyResult ' 
        EImportCloneFailedUnknown
        EImportCloneFailedNewestEquipControl
        EImportCloneFailedUnknownException
        EImportTariffRateFromRowCellNotValid
        EImportTariffRateFromRowLaneNotFound
        EImportTariffRateSqlProblem
        EImportTariffUnknownProblem
        EImportTariffUnableClone
        EImportCloneFailedNewestBPControl
        EImportCSVCarrierRatesFailed ' Failed to import csv carrier rates to temp table with file path {0} - {1}
        EUnableToReadTMPCarRates ' Failed to read carrier rates from temp table {0}
        EImportRateRejectedRateNull ' The rates have been rejected do to null record.
        EImportTariffTarIDNotvalid '{0}  - Contract ID is not valid.
        EImportTariffEquipNameNotvalid '{0}  - Equipment Name is not valid.
        EImportTariffBracketTypeNotvalid '{0}  - BracketType is not valid. 
        EImportCSVTariffCouldNotParseKeyData 'The rate row: {0} was not able to be imported, could not parse key data fields row index: {1} ..
        EImportTariffClassTypeeNotvalid '{0}  - Class Type is not valid.
        EImportTariffEffectiveFromNotvalid '{0}  - Effective From date is not valid.
        EImportTariffEffectiveToNotvalid '{0}  - Effective To date is not valid.
        EImportTariffMinvalueNotvalid '{0}  - Min value is not valid - {1}
        EImportTariffMaxDaysNotvalid '{0}  - Max days is not valid - {1}
        EImportCSVCarTarInterlineFailed ' Failed to import csv interline points to temp table with file path {0} - {1}
        EImportCSVCarTarNonServFailed ' Failed to import csv non service points to temp table with file path {0} - {1}
        ECopyContractFailedEmptyResult ' Failed to Copy, unknown failure in procedure.
        ECopyConFailedUnknown
        ECopyConFailedUnknownException
        ECopyConUnableCopy
        ECopyConFailedNoCompany 'Select a To Company to copy
        ECopyConFailedNoTariffFound 'No contract found to copy
        ECopyConFailedContractExists 'Contract already exists for company
        ECopyConFailedNoNameProvided 'No name provided
    End Enum

#End Region

#Region "Print Label Enums"
    Public Enum PrintLabelType
        PrintPalletLabels
        PrintBillLabels
        PrintBothPalletAndBills
        PrintLabelNotSelected
    End Enum
#End Region



End Class

Public Class sysErrorParameters

    Public Enum sysErrorSeverity As Integer
        Information = 0
        Warning
        Unexpected
        Critical
    End Enum

    Public Enum sysErrorState As Integer
        UserLevelFault = 0
        ServerLevelFault
        SystemLevelFault
    End Enum

    Private _Message As String
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property

    Private _Procedure As String
    Public Property Procedure() As String
        Get
            Return _Procedure
        End Get
        Set(ByVal value As String)
            _Procedure = value
        End Set
    End Property

    Private _Record As String
    Public Property Record() As String
        Get
            Return _Record
        End Get
        Set(ByVal value As String)
            _Record = value
        End Set
    End Property

    Private _Number As Integer
    Public Property Number() As Integer
        Get
            Return _Number
        End Get
        Set(ByVal value As Integer)
            _Number = value
        End Set
    End Property

    Private _Severity As Integer
    Public Property Severity() As sysErrorParameters.sysErrorSeverity
        Get
            Return _Severity
        End Get
        Set(ByVal value As sysErrorParameters.sysErrorSeverity)
            _Severity = value
        End Set
    End Property

    Private _ErrState As Integer
    Public Property ErrState() As sysErrorParameters.sysErrorState
        Get
            Return _ErrState
        End Get
        Set(ByVal value As sysErrorParameters.sysErrorState)
            _ErrState = value
        End Set
    End Property

    Private _LineNber As Integer
    Public Property LineNber() As Integer
        Get
            Return _LineNber
        End Get
        Set(ByVal value As Integer)
            _LineNber = value
        End Set
    End Property

End Class


