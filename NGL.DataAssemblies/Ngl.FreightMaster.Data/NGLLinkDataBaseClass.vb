Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Reflection
Imports System.Linq.Dynamic
Imports Newtonsoft.Json
Imports System.Runtime.Serialization
Imports SerilogTracing
Imports Serilog.Events

Public MustInherit Class NGLLinkDataBaseClass

#Region " Constructors "
    Public Sub New()
        MyBase.New()

    End Sub

#End Region

#Region " Base Class Factory Properties"
    Public Logger As Serilog.ILogger = Serilog.Log.Logger.ForContext(Of NGLLinkDataBaseClass)
    Private _NGLUserAdminObjData As NGLUserAdminData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLUserAdminObjData() As NGLUserAdminData
        Get
            If _NGLUserAdminObjData Is Nothing Then
                _NGLUserAdminObjData = New NGLUserAdminData(Parameters)

            End If
            Return _NGLUserAdminObjData
        End Get
        Set(value As NGLUserAdminData)
            _NGLUserAdminObjData = value
        End Set
    End Property

    Private _NGLAPMassEntryHistoriesData As NGLAPMassEntryHistories
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLAPMassEntryHistoriesData() As NGLAPMassEntryHistories
        Get
            If _NGLAPMassEntryHistoriesData Is Nothing Then
                _NGLAPMassEntryHistoriesData = New NGLAPMassEntryHistories(Parameters)

            End If
            Return _NGLAPMassEntryHistoriesData
        End Get
        Set(value As NGLAPMassEntryHistories)
            _NGLAPMassEntryHistoriesData = value
        End Set
    End Property

    Private _NGLAPMassEntryHistoryFeesData As NGLAPMassEntryHistoryFees
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLAPMassEntryHistoryFeesData() As NGLAPMassEntryHistoryFees
        Get
            If _NGLAPMassEntryHistoryFeesData Is Nothing Then
                _NGLAPMassEntryHistoryFeesData = New NGLAPMassEntryHistoryFees(Parameters)

            End If
            Return _NGLAPMassEntryHistoryFeesData
        End Get
        Set(value As NGLAPMassEntryHistoryFees)
            _NGLAPMassEntryHistoryFeesData = value
        End Set
    End Property

    Private _NGLAPMassEntryMsgData As NGLAPMassEntryMsg
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLAPMassEntryMsgData() As NGLAPMassEntryMsg
        Get
            If _NGLAPMassEntryMsgData Is Nothing Then
                _NGLAPMassEntryMsgData = New NGLAPMassEntryMsg(Parameters)

            End If
            Return _NGLAPMassEntryMsgData
        End Get
        Set(value As NGLAPMassEntryMsg)
            _NGLAPMassEntryMsgData = value
        End Set
    End Property

    Private _NGLLoadStatusCodeObjData As NGLLoadStatusCodeData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLLoadStatusCodeObjData() As NGLLoadStatusCodeData
        Get
            If _NGLLoadStatusCodeObjData Is Nothing Then
                _NGLLoadStatusCodeObjData = New NGLLoadStatusCodeData(Parameters)

            End If
            Return _NGLLoadStatusCodeObjData
        End Get
        Set(value As NGLLoadStatusCodeData)
            _NGLLoadStatusCodeObjData = value
        End Set
    End Property

    Private _NGLLoadTenderObjData As NGLLoadTenderData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLLoadTenderObjData() As NGLLoadTenderData
        Get
            If _NGLLoadTenderObjData Is Nothing Then
                _NGLLoadTenderObjData = New NGLLoadTenderData(Parameters)

            End If
            Return _NGLLoadTenderObjData
        End Get
        Set(value As NGLLoadTenderData)
            _NGLLoadTenderObjData = value
        End Set
    End Property

    Private _NGLAPMassEntryFeesData As NGLAPMassEntryFees
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLAPMassEntryFeesData() As NGLAPMassEntryFees
        Get
            If _NGLAPMassEntryFeesData Is Nothing Then
                _NGLAPMassEntryFeesData = New NGLAPMassEntryFees(Parameters)

            End If
            Return _NGLAPMassEntryFeesData
        End Get
        Set(value As NGLAPMassEntryFees)
            _NGLAPMassEntryFeesData = value
        End Set
    End Property

    Private _NGLAPMassEntryObjData As NGLAPMassEntryData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLAPMassEntryObjData() As NGLAPMassEntryData
        Get
            If _NGLAPMassEntryObjData Is Nothing Then
                _NGLAPMassEntryObjData = New NGLAPMassEntryData(Parameters)

            End If
            Return _NGLAPMassEntryObjData
        End Get
        Set(value As NGLAPMassEntryData)
            _NGLAPMassEntryObjData = value
        End Set
    End Property

    Private _NGLBatchProcessObjData As NGLBatchProcessDataProvider
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLBatchProcessObjData() As NGLBatchProcessDataProvider
        Get
            If _NGLBatchProcessObjData Is Nothing Then
                _NGLBatchProcessObjData = New NGLBatchProcessDataProvider(Parameters)

            End If
            Return _NGLBatchProcessObjData
        End Get
        Set(value As NGLBatchProcessDataProvider)
            _NGLBatchProcessObjData = value
        End Set
    End Property

    Private _NGLBookFeePendingObjData As NGLBookFeePendingData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLBookFeePendingObjData() As NGLBookFeePendingData
        Get
            If _NGLBookFeePendingObjData Is Nothing Then
                _NGLBookFeePendingObjData = New NGLBookFeePendingData(Parameters)

            End If
            Return _NGLBookFeePendingObjData
        End Get
        Set(value As NGLBookFeePendingData)
            _NGLBookFeePendingObjData = value
        End Set
    End Property

    Private _NGLtblAccessorialObjData As NGLtblAccessorialData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLtblAccessorialObjData() As NGLtblAccessorialData
        Get
            If _NGLtblAccessorialObjData Is Nothing Then
                _NGLtblAccessorialObjData = New NGLtblAccessorialData(Parameters)

            End If
            Return _NGLtblAccessorialObjData
        End Get
        Set(value As NGLtblAccessorialData)
            _NGLtblAccessorialObjData = value
        End Set
    End Property

    Private _NGLBookRevObjData As NGLBookRevenueData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLBookRevObjData() As NGLBookRevenueData
        Get
            If _NGLBookRevObjData Is Nothing Then
                _NGLBookRevObjData = New NGLBookRevenueData(Parameters)

            End If
            Return _NGLBookRevObjData
        End Get
        Set(value As NGLBookRevenueData)
            _NGLBookRevObjData = value
        End Set
    End Property


    Private _NGLBookObjData As NGLBookData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLBookObjData() As NGLBookData
        Get
            If _NGLBookObjData Is Nothing Then
                _NGLBookObjData = New NGLBookData(Parameters)

            End If
            Return _NGLBookObjData
        End Get
        Set(value As NGLBookData)
            _NGLBookObjData = value
        End Set
    End Property

    Private _NGLCompDockDoorObjData As NGLCompDockDoorData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLCompDockDoorObjData() As NGLCompDockDoorData
        Get
            If _NGLCompDockDoorObjData Is Nothing Then
                _NGLCompDockDoorObjData = New NGLCompDockDoorData(Parameters)

            End If
            Return _NGLCompDockDoorObjData
        End Get
        Set(value As NGLCompDockDoorData)
            _NGLCompDockDoorObjData = value
        End Set
    End Property
    'NGLDockSettingData

    Private _NGLDockSettingObjData As NGLDockSettingData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLDockSettingObjData() As NGLDockSettingData
        Get
            If _NGLDockSettingObjData Is Nothing Then
                _NGLDockSettingObjData = New NGLDockSettingData(Parameters)

            End If
            Return _NGLDockSettingObjData
        End Get
        Set(value As NGLDockSettingData)
            _NGLDockSettingObjData = value
        End Set
    End Property

    Private _NGLCompObjData As NGLCompData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLCompObjData() As NGLCompData
        Get
            If _NGLCompObjData Is Nothing Then
                _NGLCompObjData = New NGLCompData(Parameters)

            End If
            Return _NGLCompObjData
        End Get
        Set(value As NGLCompData)
            _NGLCompObjData = value
        End Set
    End Property

    Private _NGLCarrierObjData As NGLCarrierData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLCarrierObjData() As NGLCarrierData
        Get
            If _NGLCarrierObjData Is Nothing Then
                _NGLCarrierObjData = New NGLCarrierData(Parameters)

            End If
            Return _NGLCarrierObjData
        End Get
        Set(value As NGLCarrierData)
            _NGLCarrierObjData = value
        End Set
    End Property

    Private _NGLCarrierFuelAddendumObjData As NGLCarrierFuelAddendumData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLCarrierFuelAddendumObjData() As NGLCarrierFuelAddendumData
        Get
            If _NGLCarrierFuelAddendumObjData Is Nothing Then
                _NGLCarrierFuelAddendumObjData = New NGLCarrierFuelAddendumData(Parameters)

            End If
            Return _NGLCarrierFuelAddendumObjData
        End Get
        Set(value As NGLCarrierFuelAddendumData)
            _NGLCarrierFuelAddendumObjData = value
        End Set
    End Property

    'NGLLegalEntityCarrierData

    Private _NGLLegalEntityCarrierObjData As NGLLegalEntityCarrierData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLLegalEntityCarrierObjData() As NGLLegalEntityCarrierData
        Get
            If _NGLLegalEntityCarrierObjData Is Nothing Then
                _NGLLegalEntityCarrierObjData = New NGLLegalEntityCarrierData(Parameters)

            End If
            Return _NGLLegalEntityCarrierObjData
        End Get
        Set(value As NGLLegalEntityCarrierData)
            _NGLLegalEntityCarrierObjData = value
        End Set
    End Property

    Private _NGLSystemData As NGLSystemDataProvider
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLSystemData As NGLSystemDataProvider
        Get
            If _NGLSystemData Is Nothing Then
                If Not Parameters Is Nothing Then
                    Dim blnValidateAccess = Parameters.ValidateAccess
                    Parameters.ValidateAccess = False
                    _NGLSystemData = New NGLSystemDataProvider(Parameters)
                    Parameters.ValidateAccess = blnValidateAccess
                End If
            End If
            Return _NGLSystemData
        End Get
        Set(value As NGLSystemDataProvider)
            _NGLSystemData = value
        End Set
    End Property

    Private _NGLcmLocalizeKeyValuePairObjData As NGLcmLocalizeKeyValuePairData
    <JsonIgnore(), IgnoreDataMember()>
    Public Property NGLcmLocalizeKeyValuePairObjData() As NGLcmLocalizeKeyValuePairData
        Get
            If _NGLcmLocalizeKeyValuePairObjData Is Nothing Then
                _NGLcmLocalizeKeyValuePairObjData = New NGLcmLocalizeKeyValuePairData(Parameters)

            End If
            Return _NGLcmLocalizeKeyValuePairObjData
        End Get
        Set(value As NGLcmLocalizeKeyValuePairData)
            _NGLcmLocalizeKeyValuePairObjData = value
        End Set
    End Property


#End Region

#Region " Properties"

    Private _dictLinkBaseClasses As New Dictionary(Of String, NGLLinkDataBaseClass)
    ''' <summary>
    ''' Creates and Shares Instances of NGLLinkDataBaseClass object using the Class type.
    ''' Only one instance per type will be created unless the blnAlwasyCreateNew 
    ''' flag is true: this is the default behavior because each object
    ''' with a shared instance must override teh LinqTable property
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="blnAlwaysCreateNew"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 2/18/14 v.6.4 to use dictionary instad of list for more efficient look up of source object reference
    ''' Modified by RHR for v-8.2 on 10/06/2018
    '''   NGLLinkDataBaseClass is not the super class to NDPBaseClass
    ''' </remarks>
    Public Function LinkBaseClassFactory(ByVal source As String, Optional ByVal blnAlwaysCreateNew As Boolean = True) As NGLLinkDataBaseClass
        Try

            Dim t As Type = Type.[GetType]("Ngl.FreightMaster.Data." & source)

            Logger.Information("LinkBaseClassFactory: {source} - blnAlwaysCreateNew = {blnAlwaysCreateNew}", source, blnAlwaysCreateNew)
            If Not blnAlwaysCreateNew Then
                'every data object that uses this factory must overrides the LinqTable property of the base class
                If Not _dictLinkBaseClasses Is Nothing AndAlso _dictLinkBaseClasses.Count > 0 Then
                    Logger.Information("LinkBaseClassFactory: {source} - {count}", source, _dictLinkBaseClasses.Count)
                    If _dictLinkBaseClasses.ContainsKey(source) Then
                        Logger.Verbose("LinkBaseClassFactory: {source} - {count} - Found", source, _dictLinkBaseClasses.Count)
                        Return _dictLinkBaseClasses(source)
                    End If
                End If
            End If
            Logger.Verbose("Attempting to create new instance of {@t} with Parameters {@Parameters}", t, Me.Parameters)
            Dim newC As NGLLinkDataBaseClass = TryCast(Activator.CreateInstance(t, New Object() {Me.Parameters}), NGLLinkDataBaseClass)
            If newC Is Nothing Then
                Logger.Error("LinkBaseClassFactory: {source} - Could not create new instance", source)
                'Throw New System.InvalidCastException("The Class " & source & " Is Not a valid LinkBaseClass")
            End If
            If Not blnAlwaysCreateNew Then
                'if we are not always creating a new instance then add the instance to the collection
                If _dictLinkBaseClasses Is Nothing Then
                    Logger.Verbose("LinkBaseClassFactory: {source} - Creating new dictionary", source)
                    _dictLinkBaseClasses = New Dictionary(Of String, NGLLinkDataBaseClass)
                End If
                If Not _dictLinkBaseClasses.ContainsKey(source) Then
                    Logger.Verbose("LinkBaseClassFactory: {source} - Adding to dictionary", source)
                    _dictLinkBaseClasses.Add(source, newC)
                End If
            End If
            Return newC
        Catch ex As FaultException
            Logger.Error(ex, "LinkBaseClassFactory: {source}", source)
            'Throw
        Catch ex As System.NullReferenceException
            Logger.Error(ex, "LinkBaseClassFactory: {source}", source)
            'Throw New System.InvalidCastException("The Class " & source & " Is Not a valid NDPBaseClass")
        Catch ex As System.ArgumentNullException
            'Throw New System.InvalidCastException("The Class " & source & " cannot be found Or Is Not a valid NDPBaseClass")
            Logger.Error(ex, "LinkBaseClassFactory: {source}", source)
        Catch ex As System.MissingMethodException
            Logger.Error(ex, "LinkBaseClassFactory: {source}", source)
            'Throw New System.InvalidCastException("The Class " & source & " does Not support the required constructor.  It may Not be a valid NDPBaseClass")
        Catch ex As Exception
            Logger.Error(ex, "LinkBaseClassFactory: {source}", source)
            'Throw
        End Try
        Return Nothing

    End Function


    Protected _LinqDB As System.Data.Linq.DataContext
    <JsonIgnore(), IgnoreDataMember()>
    Protected Overridable Property LinqDB() As System.Data.Linq.DataContext
        Get
            Return _LinqDB
        End Get
        Set(ByVal value As System.Data.Linq.DataContext)
            _LinqDB = value
        End Set
    End Property

    Protected _LinqTable As Object
    <JsonIgnore(), IgnoreDataMember()>
    Protected Overridable Property LinqTable() As Object
        Get
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

    Public Function getLinqTable() As Object
        Return LinqTable
    End Function

    Private _ConnectionString As String = ""  ' is not available via class libraries My.Settings.NGLMAS 'the default value uses the configuration setting NGLMAS
    Public Property ConnectionString() As String
        Get
            If Len(Trim(_ConnectionString)) < 5 Then
                If Len(Trim(_Parameters.ConnectionString)) < 5 Then
                    _ConnectionString = String.Format("Server={0}; Database={1}; Integrated Security=SSPI", _Parameters.DBServer.Trim, _Parameters.Database.Trim)
                Else
                    _ConnectionString = _Parameters.ConnectionString
                End If
            End If
            Return _ConnectionString
        End Get
        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property

    Private _IsAuthorized As Boolean = True
    Public Property IsAuthorized() As Boolean
        Get
            Return _IsAuthorized
        End Get
        Set(ByVal value As Boolean)
            _IsAuthorized = value
        End Set
    End Property

    Private _Parameters As WCFParameters
    <JsonIgnore(), IgnoreDataMember()>
    Public Property Parameters() As WCFParameters
        Get
            Return _Parameters
        End Get
        Set(ByVal value As WCFParameters)
            _Parameters = value
        End Set
    End Property
    <JsonIgnore(), IgnoreDataMember()>
    Public ReadOnly Property DBInfo() As String
        Get
            Return "Server: " & Me.DBServer & vbCrLf & "Database: " & Me.Database
        End Get
    End Property

    Protected mblnSilent As Boolean = True
    Public Property Silent() As Boolean
        Get
            Return mblnSilent
        End Get
        Set(ByVal Value As Boolean)
            mblnSilent = Value
        End Set
    End Property

    Private mobjCon As New System.Data.SqlClient.SqlConnection
    Public Property DBCon() As System.Data.SqlClient.SqlConnection
        Get
            Return mobjCon
        End Get
        Set(ByVal value As System.Data.SqlClient.SqlConnection)
            mobjCon = value
        End Set
    End Property

    Public ReadOnly Property DBServer() As String
        Get
            Return Me.Parameters.DBServer
        End Get
    End Property
    <JsonIgnore(), IgnoreDataMember()>
    Public ReadOnly Property Database() As String
        Get
            Return Me.Parameters.Database
        End Get
    End Property

    Protected mblnSharedDB As Boolean = False
    Public Property SharedDB() As Boolean
        Get
            Return mblnSharedDB
        End Get
        Set(ByVal value As Boolean)
            mblnSharedDB = value
        End Set
    End Property


    Private _SourceClass As String = "NGLLinkDataBaseClass"
    Public Overridable Property SourceClass() As String
        Get
            Return _SourceClass
        End Get
        Set(ByVal value As String)
            _SourceClass = value
        End Set
    End Property


#End Region

#Region " Methods"

    Public Sub closeConnection()
        If Not mblnSharedDB Then
            Try
                If Not DBCon Is Nothing Then
                    If DBCon.State = ConnectionState.Open Then
                        DBCon.Close()
                    End If
                End If
                DBCon = Nothing
            Catch ex As Exception
                'throw away any errors while closing the database
            End Try
        End If
    End Sub

    Public Sub closeADBConnection(ByRef oCon As System.Data.SqlClient.SqlConnection)

        Try
            If Not oCon Is Nothing Then
                If oCon.State = ConnectionState.Open Then
                    oCon.Close()
                End If
            End If
            oCon = Nothing
        Catch ex As Exception
            'throw away any errors while closing the database
        End Try

    End Sub

    Public Sub openConnection()
        If mobjCon Is Nothing Then
            mobjCon = New System.Data.SqlClient.SqlConnection
        End If
        If ConnectionString.Trim.Length < 1 Then
            ConnectionString = "Data Source=" & DBServer & ";" _
                & "Initial Catalog=" & Database & ";" _
                & "Integrated Security=True"
        End If

        Try
            If mobjCon.State = ConnectionState.Open Then
                Return
            Else
                mobjCon.ConnectionString = ConnectionString
                mobjCon.Open()
            End If
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Dim strMsg As String = ex.Message
            If ex.Errors.Count > 0 Then
                strMsg = "Login Error Number: " & ex.Errors(0).Class.ToString() & ControlChars.NewLine & strMsg
            End If
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMsg}, New FaultReason("E_DBLoginFailure"))
        Catch ex As Exception
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try

    End Sub

    Public Function getNewConnection(Optional ByVal conString As String = "") As System.Data.SqlClient.SqlConnection

        Dim objcon As New System.Data.SqlClient.SqlConnection

        If conString.Trim.Length < 1 Then


            If ConnectionString.Trim.Length < 1 Then
                ConnectionString = "Data Source=" & DBServer & ";" _
                    & "Initial Catalog=" & Database & ";" _
                    & "Integrated Security=True;TrustServerCertificate=True;"
            End If
            conString = ConnectionString

        End If

        Try

            objcon.ConnectionString = conString
            objcon.Open()

        Catch ex As FaultException
            Logger.Error(ex, "getNewConnection: {conString}", conString)
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Dim strMsg As String = ex.Message
            If ex.Errors.Count > 0 Then
                strMsg = "Login Error Number: " & ex.Errors(0).Class.ToString() & ControlChars.NewLine & strMsg
            End If
            Logger.Error(ex, "getNewConnection: {conString}", conString)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMsg}, New FaultReason("E_DBLoginFailure"))
        Catch ex As Exception
            Logger.Error(ex, "getNewConnection: {conString}", conString)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return objcon

    End Function

    Private Function DecodeAuth(ByVal PassCode As String) As Boolean

        Dim passnumber As Double = 0
        Dim passresult As Double = 0
        Dim passtext1 As String = ""
        Dim passtext2 As String = ""
        Dim passfraction As Double = 0
        Dim AuthDate As Date = Date.Now.AddDays(-31)
        Try

            If Len(PassCode) > 0 Then
                Double.TryParse(PassCode, passnumber)
                passresult = passnumber - 11111111111.0#
                passresult = passresult / 24124
                passfraction = passresult - Int(passresult)
                If passfraction > 0 Then passresult = 19000101
                passtext1 = Trim(Str(passresult))
                passtext2 = Mid$(passtext1, 5, 2) & "/" & Mid$(passtext1, 7, 2) & "/" & Left$(passtext1, 4)
                Date.TryParse(passtext2, AuthDate)
            End If


        Catch ex As Exception
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_ReadAuthenticationCodeError"))
        End Try

        If Date.Now > AuthDate Then
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_LicenseViolation"}, New FaultReason("E_AccessDenied"))
        ElseIf Date.Now.AddDays(30) > AuthDate Then
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_LicenseWarning"}, New FaultReason("E_AccessGranted"))
        Else
            Return True
        End If

        Return False



    End Function

    Public Function CheckAuthCode() As Boolean
        Return DecodeAuth(getScalarString("Select top 1 AuthNumber from dbo.Auth"))
    End Function

    Public Function Login() As WCFParameters
        CheckAuthCode()
        Return Me.Parameters
    End Function



    Public Overridable Sub Log(ByVal message As String, Optional ByVal source As String = "NGL.FreightMaster.Data")
        Try
            Logger.Information("{source} - {message}", source, message)
            'CType(Me.LinkBaseClassFactory("NGLSystemLogData"), NGLSystemLogData).AddApplicaitonLog(message, source)

        Catch ex As Exception
            'we ignore errors when adding data to the log
        End Try
    End Sub

    Dim processParameterCallCount = 0
    Protected Sub processParameters(ByVal oParameters As WCFParameters)
        Try
            processParameterCallCount += 1
            'Logger.Verbose("NGL.FreightMaster.Data.processParameters - CallCount: {processParameterCallCount}\n {@oParameters}", processParameterCallCount, oParameters)
            If Not oParameters Is Nothing Then
                'clear the connection string to be sure we use the parameter values provided
                Me.ConnectionString = oParameters.ConnectionString
                'Save the parameters object
                _Parameters = oParameters

                'validate the Auth Code
                'For NGLSystem we use the default connectionstring (provided connectionstring via WCFParameters) or integrated windows security and the WCFAuthCode is not required
                If oParameters.WCFAuthCode <> "NGLSystem" Then
                    Dim strWCFAuthCode As String = readConfigSettings("WCFAuthCode").Trim
                    Dim strSQLAuthUser As String = readConfigSettings("SQLAuthUser").Trim
                    Dim strSQLAuthPass As String = readConfigSettings("SQLAuthPass").Trim
                    If Not oParameters.WCFAuthCode = strWCFAuthCode Then
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotAuthService"}, New FaultReason("E_AuthCodeFail"))
                    End If
                    'Check if we need to build a new connection string
                    If Me.ConnectionString = "" Then


                        If (Not String.IsNullOrEmpty(oParameters.DBServer) AndAlso oParameters.DBServer.Trim.Length > 0) AndAlso (Not String.IsNullOrEmpty(oParameters.Database) AndAlso oParameters.Database.Trim.Length > 0) Then
                            ConnectionString = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};TrustServerCertificate=True;", oParameters.DBServer.Trim, oParameters.Database.Trim, strSQLAuthUser, strSQLAuthPass)
                        End If
                    End If

                End If
                Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
                Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
                Dim oCmd As New System.Data.SqlClient.SqlCommand
                'Perform Authentication Procedures
                Try
                    Dim strRet As String = "0"
                    Dim intRet As Integer = 0
                    'Check if we need to authenticate the user
                    If oParameters.ValidateAccess Then
                        Dim blnLogonRequired As Boolean = True
                        Dim oUserSecurityAccessToken As NGLtblUserSecurityAccessTokenData
                        If Me.SourceClass = "NGLtblUserSecurityAccessTokenData" Then
                            'TODO: add Security Access Token Logic
                            ''oUserSecurityAccessToken = Me
                        Else
                            oUserSecurityAccessToken = New NGLtblUserSecurityAccessTokenData()
                            oUserSecurityAccessToken.loadParameterSettings(oParameters)
                        End If
                        If oParameters.UseToken Then
                            blnLogonRequired = Not oUserSecurityAccessToken.validateUserToken(oParameters)
                        End If
                        If blnLogonRequired Then
                            Dim strRetMsg As String = ""
                            Dim intErrNbr As Integer
                            'TODO LVV: Note: Add if statement here to check for usercontrol in oParameters if not 0 call
                            'the new sp to validate the user -return username and populate oParams
                            'else execute the existing code below
                            If oParameters.UserControl <> 0 Then
                                'linq call here
                                Dim sysWCF = oParameters.CloneParameters()
                                sysWCF.ValidateAccess = False
                                sysWCF.WCFAuthCode = "NGLSystem"
                                Dim oSec = New NGLSecurityDataProvider(sysWCF)
                                Dim res = oSec.NetCheckUserSecurityByControl(oParameters.UserControl, DTran.Encrypt(oParameters.UserRemotePassword, "NGL"))
                                'Dim res = NetCheckUserSecurityByControl(oParameters.UserControl, DTran.Encrypt(oParameters.UserRemotePassword, "NGL"))
                                If Not res Is Nothing Then
                                    oParameters.UserName = res.UserName
                                    If res.ErrNumber <> 0 Then
                                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = res.RetMsg}, New FaultReason("E_AccessDenied"))
                                    End If
                                End If
                            Else
                                oCmd.Parameters.AddWithValue("@UserName", oParameters.UserName)
                                'We only provide a password if not using NGL Integrated sucurity, for integrated security the password in passed as NULL
                                If oParameters.UserRemotePassword <> "@NGL_Integrated_Security_2011!@" Then
                                    oCmd.Parameters.AddWithValue("@UserRemotePassword", DTran.Encrypt(oParameters.UserRemotePassword, "NGL"))
                                End If
                                If Not oQuery.execNGLStoredProcedure(oCon, oCmd, "dbo.spNetCheckUserSecurity", 1, True, strRetMsg, intErrNbr) Then
                                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strRetMsg}, New FaultReason("E_AccessDenied"))
                                End If
                            End If
                            'Update the token information
                            oUserSecurityAccessToken.updateNGLToken(oParameters)
                        End If
                    End If
                    'Check if we need to validate Form level access
                    If oParameters.FormControl > 0 Or (Not String.IsNullOrEmpty(oParameters.FormName) AndAlso oParameters.FormName.Trim.Length > 2) Then
                        'If oCon is passed before it has been opened the funtion will create a new connection.
                        strRet = oQuery.getScalarValue(oCon, "Exec dbo.spNetCheckFormSecurityWCF " & oParameters.FormControl & ",'" & oParameters.FormName & "','" & oParameters.UserName & "'", 1)
                        If strRet.ToUpper <> "TRUE" Then
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotAuthScreen"}, New FaultReason("E_AccessDenied"))
                        End If
                    End If
                    'Check if we need to validata Procedure level access
                    If oParameters.ProcedureControl > 0 Or (Not String.IsNullOrEmpty(oParameters.ProcedureName) AndAlso oParameters.ProcedureName.Trim.Length > 2) Then
                        'If oCon is passed before it has been opened the funtion will create a new connection.
                        strRet = oQuery.getScalarValue(oCon, "Exec dbo.spNetCheckProcedureSecurityWCF " & oParameters.ProcedureControl & ",'" & oParameters.ProcedureName & "','" & oParameters.UserName & "'", 1)
                        If strRet.ToUpper <> "TRUE" Then
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotAuthProcedure"}, New FaultReason("E_AccessDenied"))
                        End If
                    End If
                    'Check if we need to validata Report level access
                    If oParameters.ReportControl > 0 Or (Not String.IsNullOrEmpty(oParameters.ReportName) AndAlso oParameters.ReportName.Trim.Length > 2) Then
                        'If oCon is passed before it has been opened the funtion will create a new connection.
                        strRet = oQuery.getScalarValue(oCon, "Exec dbo.spNetCheckReportSecurityWCF " & oParameters.ReportControl & ",'" & oParameters.ReportName & "','" & oParameters.UserName & "'", 1)
                        If strRet.ToUpper <> "TRUE" Then
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NotAuthReport"}, New FaultReason("E_AccessDenied"))
                        End If
                    End If
                Catch ex As FaultException
                    Logger.Error(ex, "NGL.FreightMaster.Data.processParameters: {@oParameters}", oParameters)
                    '  Throw
                Catch ex As Exception
                    Logger.Error(ex, "NGL.FreightMaster.Data.processParameters: {@oParameters}", oParameters)
                    ' Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
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
                        If Not oCon Is Nothing Then
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try

                End Try
            End If
        Catch ex As FaultException
            Logger.Error(ex, "NGL.FreightMaster.Data.processParameters: {@oParameters}", oParameters)
            ' Throw
        Catch ex As Exception
            Logger.Error(ex, "NGL.FreightMaster.Data.processParameters: {@oParameters}", oParameters)
            ' Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally

        End Try
    End Sub

    Public Function executeSQL(ByVal strSQL As String) As Boolean
        Dim blnRet As Boolean = False
        'Create a data connection 
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)

        Try

            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                blnRet = True
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' use the sql string to get an integer from the database
    ''' </summary>
    ''' <param name="strSQL"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.006 on 04/18/2024 added new exception logic to show time out failures to user and set default to zero on cast exception
    ''' </remarks>
    Public Function getScalarInteger(ByVal strSQL As String) As Integer

        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim intRet As Integer = 0
        Try
            'If oCon is passed before it has been opened the funtion will create a new connection.
            Integer.TryParse(oQuery.getScalarValue(oCon, strSQL, 1), intRet)
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException

            If Not ex.InnerException Is Nothing Then
                If ex.InnerException.GetType = GetType(System.InvalidCastException) Then
                    Return 0
                Else
                    Utilities.SaveAppError(ex.InnerException.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.InnerException.Message}, New FaultReason("E_DataAccessError"))
                End If
            Else
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_DataAccessError"))
            End If
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try

        End Try
        Return intRet
    End Function

    Public Function getScalarString(ByVal strSQL As String) As String

        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim strRet As String = ""
        Try
            'If oCon is passed before it has been opened the funtion will create a new connection.
            strRet = oQuery.getScalarValue(oCon, strSQL, 1)
        Catch ex As FaultException
            Throw
        Catch ex As Core.DatabaseRetryExceededException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseLogInException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBLoginFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DBConnectionFailure"}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try

        End Try
        Return strRet
    End Function

    Protected Sub runNGLStoredProcedure(ByRef oCmd As System.Data.SqlClient.SqlCommand,
                                             ByVal strProcName As String,
                                             Optional ByVal intMaxRetry As Integer = 3)
        Using Logger.StartActivity("runNGLStoredProcedure: {SQLProcedureName}", strProcName)
            runNGLStoredProcedure(New clsNGLSPConfig(oCmd, strProcName, intMaxRetry))
        End Using


    End Sub

    Protected Sub runNGLStoredProcedure(ByVal spConfig As clsNGLSPConfig)
        Logger.Information("runNGLStoredProcedure: {SQLProcedureName}", spConfig.strProcName)
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
        Dim strMessage As String = ""
        Dim strDetails As String = ""
        Try
            'If oCon is passed before it has been opened the funtion will create a new connection.
            With spConfig
                oQuery.execNGLStoredProcedure(oCon, .oCmd, .strProcName, .intMaxRetry)
            End With
        Catch ex As FaultException
            Logger.Error(ex, "Error executing {SQLProcedureName}", spConfig.strProcName)
            Throw
        Catch ex As Ngl.Core.DatabaseDataValidationException
            strMessage = "E_DataValidationFailure"
            strDetails = ex.Message
            If Not ex.InnerException Is Nothing Then
                strDetails &= "  " & ex.InnerException.Message
            End If
            Utilities.SaveAppError(strDetails, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMessage, .Details = strDetails}, New FaultReason("E_ProcessProcedureFailure"))
        Catch ex As Core.DatabaseRetryExceededException
            strMessage = "E_FailedToExecute"
            strDetails = ex.Message
            If Not ex.InnerException Is Nothing Then
                strDetails &= "  " & ex.InnerException.Message
            End If
            Utilities.SaveAppError(strDetails, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMessage, .Details = strDetails}, New FaultReason("E_ProcessProcedureFailure"))
        Catch ex As Ngl.Core.DatabaseLogInException
            strMessage = "E_DBLoginFailure"
            strDetails = ex.Message
            If Not ex.InnerException Is Nothing Then
                strDetails &= "  " & ex.InnerException.Message
            End If
            Utilities.SaveAppError(strDetails, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMessage, .Details = strDetails}, New FaultReason("E_DataAccessError"))
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMessage = "E_DBConnectionFailure"
            strDetails = ex.Message
            If Not ex.InnerException Is Nothing Then
                strDetails &= "  " & ex.InnerException.Message
            End If
            Utilities.SaveAppError(strDetails, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMessage, .Details = strDetails}, New FaultReason("E_DataAccessError"))
        Catch ex As System.Data.SqlClient.SqlException
            Logger.Error(ex, "Error executing {SQLProcedureName}", spConfig.strProcName)
            strMessage = "E_SQLExceptionMSG"
            strDetails = ex.Message
            If Not ex.InnerException Is Nothing Then
                strDetails &= "  " & ex.InnerException.Message
            End If
            Utilities.SaveAppError(strDetails, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMessage, .Details = strDetails}, New FaultReason("E_SQLException"))
        Catch ex As Exception
            Logger.Error(ex, "Error executing {SQLProcedureName}", spConfig.strProcName)
            strMessage = "E_UnExpectedMSG"
            strDetails = ex.Message
            If Not ex.InnerException Is Nothing Then
                strDetails &= "  " & ex.InnerException.Message
            End If
            Utilities.SaveAppError(strDetails, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = strMessage, .Details = strDetails}, New FaultReason("E_UnExpected"))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                spConfig.oCmd.Cancel()
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch ex As Exception

            End Try

        End Try

    End Sub

    Protected Sub RunBatchStoredProcedure(ByVal strProcName As String, ByVal strBatchName As String, ByRef oCmd As System.Data.SqlClient.SqlCommand, ByRef oSystem As NGLSystemDataProvider)

        RunBatchStoredProcedure(New clsNGLSPConfig(strBatchName, strProcName, False, oCmd), oSystem)

    End Sub

    Protected Sub RunBatchStoredProcedure(ByVal spConfig As clsNGLSPConfig, Optional ByRef oSystem As NGLSystemDataProvider = Nothing)
        Using Logger.StartActivity("RunBatchStoredProcedure: {SQLProcedureName}", spConfig.strProcName)
            If oSystem Is Nothing Then oSystem = New NGLSystemDataProvider(Me.Parameters)
            Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
            Dim oCon As System.Data.SqlClient.SqlConnection = getNewConnection(ConnectionString)
            With spConfig
                Try
                    'Update the batch processing tracking table
                    oSystem.StarttblBatchProcessRunning(Me.Parameters.UserName, .strBatchName)
                    'If oCon is passed before it has been opened the funtion will create a new connection.
                    oQuery.execNGLStoredProcedure(oCon, .oCmd, .strProcName, .intMaxRetry)
                    oSystem.EndtblBatchProcessRunning(Me.Parameters.UserName, .strBatchName)
                Catch ex As FaultException(Of SqlFaultInfo)
                    Try
                        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, ex.Detail.Message, ex.Reason.ToString)
                    Catch e As Exception
                        'just log the error and continue
                        Utilities.SaveAppError(e.Message, Me.Parameters)
                    End Try
                Catch ex As Ngl.Core.DatabaseDataValidationException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, ex.Message, "E_DataValidationFailure")
                Catch ex As Core.DatabaseRetryExceededException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Dim inner As System.Exception = ex.InnerException
                    If Not inner Is Nothing Then
                        Utilities.SaveAppError(inner.Message, Me.Parameters)
                        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, inner.Message, "E_FailedToExecute")
                    Else
                        oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, ex.Message, "E_FailedToExecute")
                    End If
                Catch ex As Ngl.Core.DatabaseLogInException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, ex.Message, "E_DBLoginFailure")
                Catch ex As Ngl.Core.DatabaseInvalidException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, ex.Message, "E_DBConnectionFailure")
                Catch ex As System.Data.SqlClient.SqlException
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, ex.Message, "E_SQLException")
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, ex.Message, "E_UnExpected")
                Finally
                    Try
                        oQuery = Nothing
                    Catch ex As Exception

                    End Try

                    Try
                        spConfig.oCmd.Cancel()
                    Catch ex As Exception

                    End Try

                    Try
                        If Not oCon Is Nothing Then
                            If oCon.State = ConnectionState.Open Then
                                oCon.Close()
                            End If
                        End If
                        oCon = Nothing
                    Catch ex As Exception

                    End Try

                End Try
            End With
        End Using

    End Sub

    Protected Function processNGLStoredProcedure(ByVal spConfig As clsNGLSPConfig, Optional ByRef oSystem As NGLSystemDataProvider = Nothing) As Boolean
        
        Using operation = Logger.StartActivity("processNGLStoredProcedure: {SQLProcedureName}", spConfig.strProcName)
            Dim blnRet As Boolean = False

            If oSystem Is Nothing Then oSystem = New NGLSystemDataProvider(Me.Parameters)
            With spConfig
                Try

                    If .blnTwoWay Then
                        runNGLStoredProcedure(spConfig)
                    Else
                        RunBatchStoredProcedure(spConfig, oSystem)

                    End If
                    operation.Complete()
                    blnRet = True
                Catch ex As Exception
                    operation.Complete()
                    Logger.Error(ex, "Error processing {SQLProcedureName}", spConfig.strProcName)
                    If .blnTwoWay Then
                        Throw
                    Else
                        Try

                            oSystem.EndtblBatchProcessRunningWithError(Me.Parameters.UserName, .strBatchName, ex.Message, "E_UnExpected")
                        Catch e As Exception
                            Logger.Error(e, "Error processing {SQLProcedureName}", spConfig.strProcName)
                        End Try
                    End If
                End Try
            End With
            Return blnRet
        End Using
        
    End Function

    Friend Function readConfigSettings(ByVal Setting As String) As String
        Return Utilities.readConfigSettings(Setting)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="member"></param>
    ''' <param name="current"></param>
    ''' <param name="original"></param>
    ''' <param name="ConflictData"></param>
    ''' <param name="blnConflictFound"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.006 on 12/22/2022 added logic to allow null/nothing equal to empty string
    ''' support of D365 web components
    ''' </remarks>
    Friend Sub addToConflicts(ByVal member As String, ByVal current As Object, ByVal original As Object, ByRef ConflictData As List(Of KeyValuePair(Of String, String)), ByRef blnConflictFound As Boolean)
        'Modified 3/13/2011 by RHR added logic to test for Nullable value
        Try
            Dim blnNewConflictFound As Boolean = False
            Dim strCurrent As String = "NULL"
            Dim strOriginal As String = "NULL"
            If current Is Nothing Then
                If Not original Is Nothing Then
                    blnNewConflictFound = True
                    blnConflictFound = True
                    strOriginal = original.ToString
                End If
            ElseIf original Is Nothing Then
                ' Modified by RHR for v-8.5.2.006
                Dim strVal As String = current.ToString()
                If Not String.IsNullOrWhiteSpace(strVal) Then
                    blnNewConflictFound = True
                    blnConflictFound = True
                    strCurrent = current.ToString
                Else
                    Return
                End If

            ElseIf current <> original Then
                blnNewConflictFound = True
                blnConflictFound = True
                strOriginal = original.ToString
                strCurrent = current.ToString
            End If
            If blnNewConflictFound Then
                ConflictData.Add(New KeyValuePair(Of String, String)(member, "current value: " & strCurrent & "; database value: " & strOriginal & vbCrLf))
            End If
            ''Old Code Removed by RHR 3/13/2011 did not handle Nullable objects correclty
            'If current <> original Then
            '    blnConflictFound = True
            '    If Not current Is Nothing Then strCurrent = current.ToString
            '    If Not original Is Nothing Then strOriginal = original.ToString

            '    ConflictData.Add(New KeyValuePair(Of String, String)(member, "current value: " & strCurrent & "; database value: " & strOriginal & vbCrLf))
            'End If
        Catch ex As Exception
            'add exception as conflict
            blnConflictFound = True
            ConflictData.Add(New KeyValuePair(Of String, String)(member, ex.Message))
        End Try
    End Sub
    ''' <summary>
    ''' Selects the desired parameter value from the main parameter table
    ''' Company level parameters are not identified
    ''' </summary>
    ''' <param name="ParKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetSystemParameterValue(ByVal ParKey As String) As Double
        Dim dblRet As Double = 0
        Try
            Dim oPars As New NGLParameterData(Me.Parameters)
            dblRet = oPars.GetParValue(ParKey)
        Catch ex As Exception
            'do nothing 
        End Try
        Return dblRet
    End Function

    Public Function GetParValueByLegalEntity(ByVal ParKey As String, ByVal LEControl As Integer) As Double
        Dim dblRet As Double = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim iCompControl = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEControl).Select(Function(y) y.LEAdminCompControl).FirstOrDefault()
                Dim dblVal = db.udfgetParValueByCompControl(ParKey, iCompControl)

                dblRet = If(dblVal, 0)


            Catch ex As Exception
                'Do nothing on error just return zero
            End Try

            Return dblRet

        End Using
    End Function



    ''' <summary>
    ''' Selects the desired parameter text from the main parameter table
    ''' Company level parameters are not identified
    ''' </summary>
    ''' <param name="ParKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetSystemParameterText(ByVal ParKey As String) As String
        Dim strRet As String = ""
        Try
            Dim oPars As New NGLParameterData(Me.Parameters)
            strRet = oPars.GetParText(ParKey)
        Catch ex As Exception
            'do nothing 
        End Try
        Return strRet
    End Function

    Protected Sub ManageLinqDataExceptions(ByRef ex As Exception, ByVal procedure As String, Optional ByRef db As System.Data.Linq.DataContext = Nothing)
        Select Case ex.GetType
            Case GetType(FaultException(Of SqlFaultInfo))
                Throw ex
            Case GetType(System.Data.SqlClient.SqlException)
                throwSQLFaultException(ex.Message)
            Case GetType(InvalidOperationException)
                throwNoDataFaultException(ex.Message)
            Case GetType(FaultException(Of Ngl.FreightMaster.Data.ConflictFaultInfo))
                Throw ex
            Case GetType(ChangeConflictException)
                throwConflictException(ex, procedure, db)
            Case Else
                throwUnExpectedFaultException(ex, procedure)
        End Select
    End Sub

    ''' <summary>
    ''' Throws a simple FaultException(Of ConflictFaultInfo)
    ''' </summary>
    ''' <param name="conflictEx"></param>
    ''' <param name="procedure"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwConflictException(ByRef conflictEx As ChangeConflictException, ByVal procedure As String, Optional ByRef db As System.Data.Linq.DataContext = Nothing)
        Try
            If db Is Nothing Then
                db = LinqDB
            End If
            If db Is Nothing Then
                Return
            End If
            Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(db))
            conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
            Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
        Catch ex As System.ObjectDisposedException
            'cannot read conflict info because the data object LinqDB has been close or disposed so just raise an invalid request message
            'using: E_CannotSaveRecordInUseDetails = Cannot save data the {0} value {1} is being used and cannot be modified.
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, SqlFaultInfo.FaultDetailsKey.E_CannotSaveRecordInUseDetails, New List(Of String) From {"selected", "it has been modified or"}, SqlFaultInfo.FaultReasons.E_DataValidationFailure)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, procedure)
        End Try
    End Sub

    ''' <summary>
    ''' Throws a Fault Exception and optionally
    ''' Saves an Application Error Log and/or
    ''' A System Error Log
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="Details"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="Reason"></param>
    ''' <param name="sendAppErr"></param>
    ''' <param name="sendSystemError"></param>
    ''' <param name="pars"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwFaultException(Message As String, Details As String, DetailsList As List(Of String), Reason As String, Optional sendAppErr As Boolean = True, Optional sendSystemError As Boolean = False, Optional pars As sysErrorParameters = Nothing)
        ' Try
        Logger.Error("Throw Fault Exception: {0}, Details: {1}, DetailsList: {@2}, Reason: {3}", Message, Details, String.Join(",", DetailsList), Reason)
        '   If sendAppErr Then Utilities.SaveAppError(Message & " " & Details & " " & String.Join(",", DetailsList), Me.Parameters)
        '  If sendSystemError Then
        ' If Not pars Is Nothing Then
        'Utilities.SaveSysError(pars, Me.Parameters)
        'Else
        'Utilities.SaveSysError(New sysErrorParameters() With {.Message = Message, .Record = Details & ": " & String.Join(",", DetailsList)}, Me.Parameters)
        'End If
        'End If
        'Catch ex As Exception
        'do nothing when saving log records
        'End Try

        '    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = Message, .Details = Details, .DetailsList = DetailsList}, New FaultReason(Reason))

    End Sub

    ''' <summary>
    ''' Throws a Fault Exception and optionally creates an Application Error Log
    ''' </summary>
    ''' <param name="enmMsg"></param>
    ''' <param name="enmDetails"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="enmReason"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwFaultException(enmMsg As SqlFaultInfo.FaultInfoMsgs,
                                               enmDetails As SqlFaultInfo.FaultDetailsKey,
                                               DetailsList As List(Of String),
                                               enmReason As SqlFaultInfo.FaultReasons,
                                               Optional sendAppErr As Boolean = True)
        Try
            Dim Message = SqlFaultInfo.getFaultMessage(enmMsg)
            Dim Reason = SqlFaultInfo.getFaultReason(enmReason)
            Dim Details = SqlFaultInfo.getFaultDetailsKey(enmDetails)
            throwFaultException(Message, Details, DetailsList, Reason, sendAppErr)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Throws a Fault Exception
    ''' </summary>
    ''' <param name="enmMsg"></param>
    ''' <param name="enmDetails"></param>
    ''' <param name="DetailsMsg"></param>
    ''' <param name="enmReason"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwFaultException(enmMsg As SqlFaultInfo.FaultInfoMsgs,
                                              enmDetails As SqlFaultInfo.FaultDetailsKey,
                                              DetailsMsg As String,
                                              enmReason As SqlFaultInfo.FaultReasons,
                                              Optional sendAppErr As Boolean = True)

        Logger.Error("Throw Fault Exception: {enmMsg}, Details: {enmDetails}, DetailsList: {@DetailsMsg}, Reason: {enmReason}", enmMsg, enmDetails, DetailsMsg, enmReason)


    End Sub

    ''' <summary>
    ''' Throws a Fault Exception and creates a system Error Message Log
    ''' </summary>
    ''' <param name="enmMsg"></param>
    ''' <param name="enmDetails"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="enmReason"></param>
    ''' <param name="sysMessage"></param>
    ''' <param name="Procedure"></param>
    ''' <param name="Record"></param>
    ''' <param name="enmSeverity"></param>
    ''' <param name="enmErrState"></param>
    ''' <param name="LineNber"></param>
    ''' <param name="Number"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwFaultException(enmMsg As SqlFaultInfo.FaultInfoMsgs,
                                               enmDetails As SqlFaultInfo.FaultDetailsKey,
                                               DetailsList As List(Of String),
                                                  enmReason As SqlFaultInfo.FaultReasons,
                                                  sysMessage As String,
                                                  Procedure As String,
                                                  Record As String,
                                                  enmSeverity As sysErrorParameters.sysErrorSeverity,
                                                  enmErrState As sysErrorParameters.sysErrorState,
                                                  Optional LineNber As Integer = 0,
                                                  Optional Number As Integer = 0)

        Logger.Error("Throw Fault Exception: {@FaultInfoMsgs}, Details: {@enmDetails}, DetailsList: {@DetailsList}, Reason: {@Reason}, SysMessage: {SysMessage}, Procedure: {Procedure}, Record: {Record}", enmMsg, enmDetails, String.Join(",", DetailsList), enmReason, sysMessage, Procedure, Record)



    End Sub

    ''' <summary>
    ''' Throws an E_FieldRequired Fault Exception.  The actual field name must be included in the FieldName parameter
    ''' </summary>
    ''' <param name="FieldName"></param>
    ''' <param name="sendAppErr"></param>
    ''' <param name="sendSystemError"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwFieldRequiredException(ByVal FieldName As String,
                                               Optional sendAppErr As Boolean = True,
                                               Optional sendSystemError As Boolean = False)



        Dim DetailsList As New List(Of String) From {FieldName}
        Dim Message = SqlFaultInfo.getFaultMessage(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_InvalidRecordKeyField)
        Dim Reason = SqlFaultInfo.getFaultReason(FreightMaster.Data.SqlFaultInfo.FaultReasons.E_DataValidationFailure)
        Dim Details = SqlFaultInfo.getFaultDetailsKey(FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_FieldRequired)

        Logger.Error("Throw Fault Exception: {0}, Details: {1}, DetailsList: {@2}, Reason: {3}", Message, Details, String.Join(",", DetailsList), Reason)



    End Sub

    'If oData.POHDRHoldLoad Then throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_AccessDenied, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_RecordOnHold, Nothing, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_InvalidOperationException)

    Public Overridable Sub throwDataValidationException(ByVal sMessage As String,
                                               detailsKey As FreightMaster.Data.SqlFaultInfo.FaultDetailsKey,
                                               Optional sendAppErr As Boolean = True,
                                               Optional sendSystemError As Boolean = False)
        Logger.Error("Throw Fault Exception: {0}, Details: {1}, DetailsList: {@2}, Reason: {3}", FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyField, detailsKey, sMessage, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_DataValidationFailure)
    End Sub


    Public Overridable Sub throwRecordOnHoldException(ByVal sMessage As String,
                                               Optional sendAppErr As Boolean = True,
                                               Optional sendSystemError As Boolean = False)

        Logger.Error("Throw Fault Exception: {0}, Details: {1}, DetailsList: {@2}, Reason: {3}", FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_RecordOnHold, sMessage, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_DataValidationFailure)


    End Sub

    ''' <summary>
    ''' Throws an "One of more of the key fields already exists in the database." Fault Exception
    ''' using a custom FaultDetailsKey
    ''' and optionally creates an application error log
    ''' </summary>
    ''' <param name="enmDetails"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidKeyFaultException(ByVal enmDetails As SqlFaultInfo.FaultDetailsKey,
                                                       DetailsList As List(Of String),
                                                       Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyField, enmDetails, DetailsList, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)
    End Sub

    ''' <summary>
    ''' Throws an "One of more of the key fields already exists in the database." exception 
    ''' using "'Cannot save changes to {0}.  The unique key {1} value {2} already exists." Details 
    ''' and optionally creates an application error log
    ''' E_CannotSaveKeyFieldsRequired 'Cannot save changes to {0}.  The following key fields are required: {1}. 
    ''' </summary>
    ''' <param name="Container"></param>
    ''' <param name="Key"></param>
    ''' <param name="Val"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidKeyAlreadyExistsException(ByVal Container As String,
                                                               ByVal Key As String,
                                                               ByVal Val As String,
                                                               Optional sendAppErr As Boolean = True)

        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyField, SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyAlreadyExists, New List(Of String) From {Container, Key, Val}, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)
    End Sub

    ''' <summary>
    ''' Throws a "One of more of the key fields already exists in the database." exception
    ''' using "Cannot save changes to {0}. A record with one or more of the following key values already exist: Keys {1} Values {2}." Details
    ''' and optionally creates an applicaiton error log.  NOTE: Container,Keys and Vals should not contain commas
    ''' E_CannotSaveKeyFieldsRequired 'Cannot save changes to {0}.  The following key fields are required: {1}. 
    ''' </summary>
    ''' <param name="Container"></param>
    ''' <param name="Keys"></param>
    ''' <param name="Vals"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidKeysAlreadyExistsException(ByVal Container As String,
                                                               ByVal Keys As String,
                                                               ByVal Vals As String,
                                                               Optional sendAppErr As Boolean = True)

        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyField, SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyValuesAlreadyExist, New List(Of String) From {Container, Keys, Vals}, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)
    End Sub

    ''' <summary>
    ''' Throws a "One of more of the key fields already exists in the database." exception
    ''' using "Cannot save changes to {0}.  The following key fields are required: {1}." Details
    ''' and optionally creates an applicaiton error log.  NOTE: Container, and ValidationMsg should not contain commas
    ''' </summary>
    ''' <param name="Container"></param>
    ''' <param name="ValidationMsg"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidRequiredKeysException(ByVal Container As String,
                                                               ByVal ValidationMsg As String,
                                                               Optional sendAppErr As Boolean = True)

        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyField, SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyFieldsRequired, New List(Of String) From {Container, ValidationMsg}, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)
    End Sub
    ''' <summary>
    ''' Throws an "One or more key record values are invalid or cannot be found." Fault Exception 
    ''' using a custom FaultDetailsKey
    ''' and optionally creates an application error log
    ''' </summary>
    ''' <param name="enmDetails"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidKeyFilterMetaDataException(ByVal enmDetails As SqlFaultInfo.FaultDetailsKey, ByVal DetailsList As List(Of String), Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, enmDetails, DetailsList, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)
    End Sub

    ''' <summary>
    ''' Throws a fault exception like: 
    ''' Reason: Cannot Create New Record, 
    ''' Message: The automatic creation of dependent records failed. Please complete the process manually., 
    ''' Details: The system could not generate a new {sDetails}
    ''' </summary>
    ''' <param name="sDetails"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 04/05/2018
    '''     new exception used for messages like when then
    '''     booking order cannot be created after a load is dispatched.
    ''' </remarks>
    Public Overloads Sub throwCreateNewDependentRecordFailed(ByVal sDetails As String, Optional sendAppErr As Boolean = True)
        Dim DetailsList = New List(Of String) From {sDetails}
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_CreateNewDependentRecordFailed, SqlFaultInfo.FaultDetailsKey.E_SystemFaliedToGeneratedKeyField, DetailsList, SqlFaultInfo.FaultReasons.E_CreateRecordFailure, sendAppErr)
    End Sub

    ''' <summary>
    ''' Throws an "One or more key record values are invalid or cannot be found." Fault Exception 
    ''' using "Cannot save your changes because the {0} value {1} is required"
    ''' and optionally creates an application error log
    ''' </summary>
    ''' <param name="DetailsList"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidKeyParentRequiredException(ByVal DetailsList As List(Of String), Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_CannotSaveParentKeyRequired, DetailsList, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)
    End Sub

    ''' <summary>
    ''' Throws an E_InvalidRequest when a delete is requested for a protected record
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <param name="Val"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidDeleteRequestException(ByVal Key As String,
                                                               ByVal Val As String,
                                                               Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, SqlFaultInfo.FaultDetailsKey.E_CannotDeleteProtectedDataDetails, New List(Of String) From {Key, Val}, SqlFaultInfo.FaultReasons.E_DataValidationFailure)
    End Sub

    ''' <summary>
    ''' Throws an E_InvalidRequest when a delete is requested for a record that is still being used by a dependent record
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <param name="Val"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwCannotDeleteRecordInUseException(ByVal Key As String,
                                                               ByVal Val As String,
                                                               Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, SqlFaultInfo.FaultDetailsKey.E_CannotDeleteRecordInUseDetails, New List(Of String) From {Key, Val}, SqlFaultInfo.FaultReasons.E_DataValidationFailure)
    End Sub

    ''' <summary>
    ''' Throws an "Invalid Request" when an update is requested for a protected record
    ''' using "Cannot save changes the {0} value {1} is protected and cannot be modified."
    ''' and optionally creates an application error log    ''' 
    ''' </summary>
    ''' <param name="Key">{0}</param>
    ''' <param name="Val">{1}</param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidSaveRequestException(ByVal Key As String,
                                                               ByVal Val As String,
                                                               Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, SqlFaultInfo.FaultDetailsKey.E_CannotSaveProtectedDataDetails, New List(Of String) From {Key, Val}, SqlFaultInfo.FaultReasons.E_DataValidationFailure)
    End Sub

    Public Overloads Sub throwCannotSaveInUseException(ByVal Key As String,
                                                               ByVal Val As String,
                                                               Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidRequest, SqlFaultInfo.FaultDetailsKey.E_CannotSaveRecordInUseDetails, New List(Of String) From {Key, Val}, SqlFaultInfo.FaultReasons.E_DataValidationFailure)
    End Sub

    ''' <summary>
    ''' Throws an E_CannotUpdateTariffApproved Fault Exception and optionally creates an application error log
    ''' </summary>
    ''' <param name="enmDetails"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwCannotUpdateTariffApprovedDataException(ByVal enmDetails As SqlFaultInfo.FaultDetailsKey, ByVal DetailsList As List(Of String), Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_CannotUpdateTariffApproved, enmDetails, DetailsList, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)
    End Sub

    ''' <summary>
    ''' Throws an E_AssignCarrierFailed Fault Exception using E_NoTariffAvailable message for the load
    ''' </summary>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwNoTariffAvailableException(ByVal BookConsPrefix As String, Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_AssignCarrierFailed, SqlFaultInfo.FaultDetailsKey.E_NoTariffAvailable, New List(Of String) From {BookConsPrefix}, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)

    End Sub

    ''' <summary>
    ''' Throws an E_AssignCarrierFailed Fault Exception using E_InvalidTranCode message for the load 
    ''' </summary>
    ''' <param name="BookTransCode"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwInvalidTranCodeException(ByVal BookTransCode As String, Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_AssignCarrierFailed, SqlFaultInfo.FaultDetailsKey.E_InvalidTranCode, New List(Of String) From {BookTransCode}, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)

    End Sub

    ''' <summary>
    ''' Throws an E_AssignCarrierFailed Fault Exception using E_CostsAreLocked message for the load 
    ''' </summary>
    ''' <param name="BookProNumber"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwCostLockedException(ByVal BookProNumber As String, Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_AssignCarrierFailed, SqlFaultInfo.FaultDetailsKey.E_CostsAreLocked, New List(Of String) From {BookProNumber}, SqlFaultInfo.FaultReasons.E_DataValidationFailure, sendAppErr)

    End Sub

    ''' <summary>
    ''' Throws an E_SQLExceptionMSG Fault Exception and optionally creates an application error log
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwSQLFaultException(ByVal message As String, Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, New List(Of String) From {message}, SqlFaultInfo.FaultReasons.E_SQLException, sendAppErr)

    End Sub

    ''' <summary>
    ''' Throws an E_NoData Fault Exception with an optional details message and optionally creates an application error log
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwNoDataFaultException(Optional message As String = "", Optional sendAppErr As Boolean = True)
        If message.Trim.Length < 1 Then 'E_NoDetails
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_NoData, SqlFaultInfo.FaultDetailsKey.E_NoDetails, New List(Of String), SqlFaultInfo.FaultReasons.E_InvalidOperationException, sendAppErr)
        Else
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_NoData, SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, New List(Of String) From {message}, SqlFaultInfo.FaultReasons.E_InvalidOperationException, sendAppErr)
        End If
    End Sub

    Public Overloads Sub throwNoDataFaultMessage(ByVal Details As String, Optional sendAppErr As Boolean = True)

        Dim Message = SqlFaultInfo.getFaultMessage(SqlFaultInfo.FaultInfoMsgs.E_NoData)
        Dim Reason = SqlFaultInfo.getFaultReason(SqlFaultInfo.FaultReasons.E_InvalidOperationException)
        throwFaultException(Message, Details, New List(Of String), Reason, sendAppErr)

    End Sub



    ''' <summary>
    ''' Throws an E_MethodOrProperyDepreciated Fault Exception with an optional details message typically the procedure name
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwDepreciatedException(Optional message As String = "")
        If message.Trim.Length < 1 Then 'E_NoDetails
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_MethodOrProperyDepreciated, SqlFaultInfo.FaultDetailsKey.E_NoDetails, New List(Of String), SqlFaultInfo.FaultReasons.E_InvalidOperationException, False)
        Else
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_MethodOrProperyDepreciated, SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, New List(Of String) From {message}, SqlFaultInfo.FaultReasons.E_InvalidOperationException, False)
        End If
    End Sub

    ''' <summary>
    ''' Throws an E_RecordDeleted Fault Exception with an optional details message and optionally creates an application error log
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwRecordDeletedFaultException(Optional message As String = "", Optional sendAppErr As Boolean = True)
        If message.Trim.Length < 1 Then 'E_NoDetails
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_RecordDeleted, SqlFaultInfo.FaultDetailsKey.E_NoDetails, New List(Of String), SqlFaultInfo.FaultReasons.E_InvalidOperationException, sendAppErr)
        Else
            throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_RecordDeleted, SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, New List(Of String) From {message}, SqlFaultInfo.FaultReasons.E_InvalidOperationException, sendAppErr)
        End If
    End Sub

    ''' <summary>
    ''' Throws an E_UnExpected Fault Exception with an optional details message and optionally creates an application error log
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="sendAppErr"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwUnExpectedFaultException(ByVal message As String, Optional sendAppErr As Boolean = True)
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_UnExpectedMSG, SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails, New List(Of String) From {message}, SqlFaultInfo.FaultReasons.E_UnExpected, sendAppErr)
    End Sub

    ''' <summary>
    ''' Throws an E_UnExpected Fault Exception and creates a system Error Message Log
    ''' </summary>
    ''' <param name="execp"></param>
    ''' <param name="Procedure"></param>
    ''' <param name="enmErrState"></param>
    ''' <param name="enmSeverity"></param>
    ''' <remarks></remarks>
    Public Overloads Sub throwUnExpectedFaultException(ByVal execp As Exception,
                                                          Procedure As String,
                                                          Optional enmErrState As sysErrorParameters.sysErrorState = sysErrorParameters.sysErrorState.UserLevelFault,
                                                          Optional enmSeverity As sysErrorParameters.sysErrorSeverity = sysErrorParameters.sysErrorSeverity.Unexpected)

        Logger.Error("Throw Fault Exception: {0}, Details: {1}, DetailsList: {@2}, Reason: {3}", SqlFaultInfo.FaultInfoMsgs.E_UnExpectedMSG, SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails, execp.Message, SqlFaultInfo.FaultReasons.E_UnExpected)
    End Sub

    Public Overloads Sub throwUnExpectedFaultException(ByVal execp As Exception,
                                                          Procedure As String,
                                                  Record As String,
                                                  enmSeverity As sysErrorParameters.sysErrorSeverity,
                                                  enmErrState As sysErrorParameters.sysErrorState,
                                                  Optional LineNber As Integer = 0,
                                                  Optional Number As Integer = 0)
        Logger.Error("Throw Fault Exception: {0}, Details: {1}, DetailsList: {@2}, Reason: {3}", SqlFaultInfo.FaultInfoMsgs.E_UnExpectedMSG, SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails, execp.Message, SqlFaultInfo.FaultReasons.E_UnExpected)

    End Sub

    ''' <summary>
    ''' Throws a system error for InvalidOperation with a server message
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="sendAppErr"></param>
    ''' <param name="sendSystemError"></param>
    ''' <remarks></remarks>
    Public Overridable Sub throwInvalidOperatonException(ByVal Message As String, Optional sendAppErr As Boolean = True, Optional sendSystemError As Boolean = True)
        Dim DetailsList As New List(Of String) From {Message}
        throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidOperationException, SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, DetailsList, SqlFaultInfo.FaultReasons.E_DataAccessFailure, sendAppErr, sendSystemError)
    End Sub

    Protected Overridable Function getSourceCaller(ByVal caller As String) As String
        Return SourceClass & "." & caller
    End Function

    Protected Overridable Function buildProcedureName(ByVal caller As String) As String
        Return SourceClass & "." & caller
    End Function

    Protected Function CheckForDataConflicts(newData As [Object], existingData As [Object], ByVal skipObjs As List(Of String), ByRef ConflictData As List(Of KeyValuePair(Of String, String))) As Boolean
        If newData Is Nothing Or existingData Is Nothing Then
            Return False
        End If
        If ConflictData Is Nothing Then ConflictData = New List(Of KeyValuePair(Of String, String))
        Dim blnConflictFound As Boolean = False
        Dim existingType As Type = existingData.[GetType]()
        Dim newType As Type = newData.[GetType]()

        ' Get all the property data 
        Dim eProps As PropertyInfo() = existingType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        Dim nProps As PropertyInfo() = newType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        For Each eProp As PropertyInfo In eProps
            If Not skipObjs.Contains(eProp.Name) Then
                For Each nProp In nProps
                    If nProp.Name = eProp.Name Then
                        If nProp.PropertyType() = eProp.PropertyType() Then
                            Try
                                addToConflicts(eProp.Name, nProp.GetValue(newData), eProp.GetValue(existingData), ConflictData, blnConflictFound)
                            Catch ex As Exception
                                Dim strMsg As String = ex.Message
                                Throw
                            End Try
                        End If
                        Exit For
                    End If
                Next
            End If
        Next
        Return blnConflictFound

    End Function

    ''' <summary>
    ''' Checks if a company level parameter exists
    ''' if found return company level parameter
    ''' if not found return the default global parameter
    ''' </summary>
    ''' <param name="parKey"></param>
    ''' <param name="compControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetParValue(ByVal parKey As String, ByVal compControl As Integer) As Double
        Using operation = Logger.StartActivity("GetParValue(ParKey: {ParKey}, CompControl: {CompControl})", parKey, compControl)
            Using db As New NGLMASCompDataContext(ConnectionString)
                Try
                    Dim dblVal = db.udfgetParValueByCompControl(parKey, compControl)
                    operation.Complete()

                    Return If(dblVal.HasValue, dblVal.Value, 0)

                Catch ex As System.Data.SqlClient.SqlException
                    operation.Complete(LogEventLevel.Error, ex)
                Catch ex As InvalidOperationException
                    operation.Complete(LogEventLevel.Error, ex)

                Catch ex As Exception
                    operation.Complete(LogEventLevel.Error, ex)

                End Try

                Return 0
            End Using

        End Using

    End Function

    ''' <summary>
    ''' Checks if a company level parameter exists
    ''' if found return company level parameter
    ''' if not found return the default global parameter
    ''' </summary>
    ''' <param name="parKey"></param>
    ''' <param name="compControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetParText(ByVal parKey As String, ByVal compControl As Integer) As String

        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Return db.udfgetParTextByCompControl(parKey, compControl)
            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex,
                                    getSourceCaller("GetParText"),
                                    "ParKey: " & parKey & " CompControl: " & compControl.ToString,
                                    sysErrorParameters.sysErrorSeverity.Unexpected,
                                    sysErrorParameters.sysErrorState.UserLevelFault)
            End Try

            Return ""

        End Using

    End Function



    ''' <summary>
    ''' Apply paging and sorting rules provided in AllFilters Model object; caller must construct IQueryable and handle all exceptions
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="iQuery"></param>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 12/01/2017
    '''     used to normalize paging and sorting by single column when using AllFilters Model
    ''' Modified by RHR for v-8.2.0.002 on 12/23/2020
    '''     Moved Groups below Sort so groups are applied first.
    '''         Linq loads in reverse order
    ''' Modified by RHR for v-8.5.3.007 reset RecordCount before return
    '''     because all non-filtered records are showing 500 available
    ''' Modified by RHR for v-8.5.4.003 on 11/06/2023 changed logic to 
    '''  update iTake using the iQuery.Count() when iTake is less than  1
    ''' </remarks>
    Public Overridable Sub PrepareQuery(Of TEntity)(ByRef iQuery As IQueryable(Of TEntity), ByRef filters As Models.AllFilters, ByRef RecordCount As Integer)
        Using operation = Logger.StartActivity("PrepareQuery<{TEntity}>: RecordCount: {RecordCount}, Filters: {@Filters}", GetType(TEntity), RecordCount, filters)

            Dim iTake As Integer = 0
            RecordCount = iQuery.Count()
            If filters.FilterValues?.Count() < 1 AndAlso RecordCount = 0 Then
                iTake = 500
            End If
            If RecordCount < 1 Then RecordCount = 5
            If iTake < 1 Then
                'Modified by RHR for v-8.5.4.003 on 11/06/2023
                iTake = RecordCount
                If iTake < 5 Then iTake = 5
            End If
            If (iTake > 500) Then iTake = 500

            If filters.take < 1 Then filters.take = iTake 'If(RecordCount > 500, 500, RecordCount)
            'adjust for last page if skip beyound last page
            If filters.skip >= RecordCount Then filters.skip = (CInt(((iTake - 1) / filters.take)) * filters.take)  '(CInt(((RecordCount - 1) / filters.take)) * filters.take)
            'adjust for first page if skip beyound or below first page
            If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0
            If Not String.IsNullOrWhiteSpace(filters.sortName) Then
                If Left(filters.sortDirection.ToLower(), 3) = "des" Then
                    iQuery = iQuery.OrderBy(filters.sortName, True)
                Else
                    iQuery = iQuery.OrderBy(filters.sortName, False)
                End If
            End If
            ' Modified by RHR for v-8.2.0.002 on 12/23/2020
            '  Moved Groups below Sort so groups are applied first.
            '  Linq loads in reverse order
            If filters.Groups IsNot Nothing AndAlso filters.Groups.Count > 0 Then
                For i As Integer = (filters.Groups.Count - 1) To 0 Step -1
                    Dim s As String = filters.Groups(i)
                    iQuery = iQuery.OrderBy(s, False)
                Next
            End If
        End Using


        ' Modified by RHR for v-8.5.3.007 reset RecordCount before return
        'RecordCount = iOrigRecordCount


    End Sub

    ''' <summary>
    ''' Apply filers to iQuery using predefined or blank filterWhere string and the AllFilters Model data
    ''' </summary>
    ''' <typeparam name="TEntity"></typeparam>
    ''' <param name="iQuery"></param>
    ''' <param name="filters"></param>
    ''' <param name="filterWhere"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 03/02/2018
    '''     used to normalize filter processing when using AllFilters Model
    '''     requires referece to DLinqUtil
    '''     additional predefined filterWhere values may be passed in like
    '''     (CarrierControl = 10) AND (CompContol = 5)
    ''' Modified by RHR for v-8.1 on 3/15/2018
    '''     added logic to look up data types based on field name using  getElmtFieldDataType
    ''' Modified by RHR for v-8.2 on 06/26/2018
    '''     added logic for multiple filters
    ''' Modified by RHR for v-8.2 on 10/06/2018
    '''    added logic to support Or when filter name is the same
    ''' </remarks>
    Public Overridable Sub ApplyAllFilters(Of TEntity)(ByRef iQuery As IQueryable(Of TEntity), ByRef filters As Models.AllFilters, ByRef filterWhere As String)
        Using Logger.StartActivity("ApplyAllFilters<{TEntity}>: FilterWhere: {@FilterWhere}, Filters: {@Filters}", GetType(TEntity), filterWhere, filters)
            Dim sSep = ""
            If Not String.IsNullOrWhiteSpace(filterWhere) Then sSep = " AND "
            Dim sPreviousFilter As String = ""
            Dim sFilterToApply As String = ""
            For Each f In filters.FilterValues.OrderBy(Function(x) x.filterName)
                If (Not String.IsNullOrWhiteSpace(f.filterName)) Then
                    Dim oElmtDataType = NGLSystemDataProvider.getElmtFieldDataType(ConnectionString, f.filterName)
                    If (Not String.IsNullOrWhiteSpace(f.filterValueFrom)) Then
                        Dim blnBetween As Boolean = False
                        If (Not String.IsNullOrWhiteSpace(f.filterValueTo)) Then
                            blnBetween = True
                        End If
                        Dim blnUseOr As Boolean = False
                        Dim sOrSep As String = ""
                        If f.filterName = sPreviousFilter Then
                            blnUseOr = True
                            sOrSep = " Or "
                        ElseIf Not String.IsNullOrWhiteSpace(sFilterToApply) Then
                            filterWhere &= String.Concat(sSep, "( ", sFilterToApply, " )")
                            sFilterToApply = ""
                            sSep = " AND "
                        End If
                        If Not oElmtDataType Is Nothing Then
                            Dim dblValFrom = 0
                            Dim dblValTo = 0
                            If oElmtDataType.NBR.HasValue AndAlso oElmtDataType.NBR = True Then
                                If f.filterValueFrom.ToLower() = "false" Then
                                    sFilterToApply &= String.Concat(sOrSep, "(", f.filterName, " = false", ")")
                                ElseIf f.filterValueFrom.ToLower() = "true" Then
                                    sFilterToApply &= String.Concat(sOrSep, "(", f.filterName, " = true", ")")
                                ElseIf f.filterValueFrom.ToLower() = "both" Then
                                    sFilterToApply &= String.Concat(sOrSep, "(", f.filterName, " = true or ", f.filterName, " = false", ")")
                                ElseIf Double.TryParse(f.filterValueFrom, dblValFrom) Then
                                    If blnBetween And Double.TryParse(f.filterValueTo, dblValTo) Then
                                        sFilterToApply &= String.Concat(sOrSep, "(", f.filterName, " >= ", dblValFrom.ToString(), ") AND (", f.filterName, " <= ", dblValTo.ToString(), ")")
                                    Else
                                        sFilterToApply &= String.Concat(sOrSep, "(", f.filterName, " = ", dblValFrom.ToString(), ")")
                                    End If
                                End If
                            ElseIf oElmtDataType.TXT.HasValue AndAlso oElmtDataType.TXT = True Then
                                If blnBetween Then
                                    sFilterToApply &= String.Concat(sOrSep, "(", f.filterName, " >= """, f.filterValueFrom.Trim(), """) AND (", f.filterName, " <= """, f.filterValueTo.Trim(), """)")
                                Else
                                    sFilterToApply &= String.Concat(sOrSep, "(", f.filterName, ".Contains(""", f.filterValueFrom.Trim(), """))")
                                End If
                            ElseIf oElmtDataType.DAT.HasValue AndAlso oElmtDataType.DAT = True Then
                                If f.filterFrom.HasValue Then
                                    Dim StartDate = DTran.formatStartDateFilter(f.filterFrom)
                                    'this is  date so check for between dates
                                    If f.filterTo.HasValue Then
                                        Dim EndDate = DTran.formatEndDateFilter(f.filterTo)
                                        sFilterToApply &= String.Concat(sOrSep, "((", f.filterName, " = NULL) OR (", f.filterName, " >= DateTime.Parse(""", StartDate, """) AND ", f.filterName, " <= DateTime.Parse(""", EndDate, """)))")
                                    Else
                                        Dim EndDate = DTran.formatEndDateFilter(f.filterFrom)
                                        sFilterToApply &= String.Concat(sOrSep, "((", f.filterName, " = NULL) OR (", f.filterName, " >= DateTime.Parse(""", StartDate, """) AND ", f.filterName, " <= DateTime.Parse(""", EndDate, """)))")
                                    End If
                                Else
                                    sFilterToApply &= String.Concat(sOrSep, "(", f.filterName, " = """, f.filterValueFrom.Trim(), """)")
                                End If
                            Else  'here we assume a valid date even if the mapping is not set as a date
                                If f.filterFrom.HasValue Then
                                    Dim StartDate = DTran.formatStartDateFilter(f.filterFrom)
                                    'this is a date so check for between dates
                                    If f.filterTo.HasValue Then
                                        Dim EndDate = DTran.formatEndDateFilter(f.filterTo)
                                        sFilterToApply &= String.Concat(sOrSep, "((", f.filterName, " = NULL) OR (", f.filterName, " >= DateTime.Parse(""", StartDate, """) AND ", f.filterName, " <= DateTime.Parse(""", EndDate, """)))")
                                    Else
                                        Dim EndDate = DTran.formatEndDateFilter(f.filterFrom)
                                        sFilterToApply &= String.Concat(sOrSep, "((", f.filterName, " = NULL) OR (", f.filterName, " >= DateTime.Parse(""", StartDate, """) AND ", f.filterName, " <= DateTime.Parse(""", EndDate, """)))")
                                    End If
                                End If
                            End If
                        End If
                    ElseIf f.filterFrom.HasValue Then
                        Dim StartDate = DTran.formatStartDateFilter(f.filterFrom)
                        'this is a date so check for between dates
                        If f.filterTo.HasValue Then
                            Dim EndDate = DTran.formatEndDateFilter(f.filterTo)
                            filterWhere &= String.Concat(sSep, "((", f.filterName, " = NULL) OR (", f.filterName, " >= DateTime.Parse(""", StartDate, """) AND ", f.filterName, " <= DateTime.Parse(""", EndDate, """)))")
                        Else
                            Dim EndDate = DTran.formatEndDateFilter(f.filterFrom)
                            filterWhere &= String.Concat(sSep, "((", f.filterName, " = NULL) OR (", f.filterName, " >= DateTime.Parse(""", StartDate, """) AND ", f.filterName, " <= DateTime.Parse(""", EndDate, """)))")
                        End If
                        sSep = " AND "

                    End If
                End If
                sPreviousFilter = f.filterName
            Next
            If Not String.IsNullOrWhiteSpace(sFilterToApply) Then filterWhere &= String.Concat(sSep, " ( ", sFilterToApply, " ) ")

            Logger.Information("ApplyAllFilters: ComputedFilterWhere: {ComputedFilterWhere}", filterWhere)

            If Not String.IsNullOrWhiteSpace(filterWhere) Then
                iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Used to perform a clone copy of data using primatives  use skipObjs to copy more complex data individually
    ''' </summary>
    ''' <param name="toObj"></param>
    ''' <param name="fromObj"></param>
    ''' <param name="skipObjs"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Changed by RHR for v-8.5.3.007 on 04/06/2023 from Protected to Public so the function can be used by the new API Assembly
    ''' </remarks>
    Public Shared Function CopyMatchingFields(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String), Optional ByRef strMsg As String = "") As Object
        SyncLock toObj
            If toObj Is Nothing Or fromObj Is Nothing Then
                Return Nothing
            End If
            'primitives used for casting
            Dim iVal16 As Int16 = 0
            Dim iVal32 As Int32 = 0
            Dim iVal64 As Int64 = 0
            Dim dblVal As Double = 0
            Dim decVal As Decimal = 0
            Dim dtVal As Date = Date.Now()
            Dim blnVal As Boolean = False
            Dim intVal As Integer = 0

            Dim fromType As Type = fromObj.[GetType]()
            Dim toType As Type = toObj.[GetType]()

            ' Get all FieldInfo. 
            Dim fProps As PropertyInfo() = fromType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
            Dim tProps As PropertyInfo() = toType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
            For Each fProp As PropertyInfo In fProps
                Dim propValue As Object = fProp.GetValue(fromObj)
                'Removed by RHR 10/8/14 did not update nullable fields when null
                'If propValue IsNot Nothing Then
                If skipObjs Is Nothing OrElse Not skipObjs.Contains(fProp.Name) Then
                    For Each tProp In tProps
                        If tProp.Name = fProp.Name Then
                            If tProp.PropertyType() = fProp.PropertyType() Then
                                Try
                                    tProp.SetValue(toObj, propValue)
                                Catch ex As Exception
                                    strMsg &= ex.Message
                                    Throw
                                End Try
                            Else
                                Dim sfPropName = fProp.PropertyType.Name
                                Dim strPropValue As String = ""
                                If Not propValue Is Nothing Then strPropValue = propValue.ToString()
                                Dim stPropName = tProp.PropertyType.Name
                                If stPropName.Substring(0, 4).ToUpper = "NULL" Then
                                    'this is a nullable data type check which type
                                    If tProp.PropertyType.FullName.Contains("Int16") Then
                                        stPropName = "Int16"
                                    ElseIf tProp.PropertyType.FullName.Contains("Int32") Then
                                        stPropName = "Int32"
                                    ElseIf tProp.PropertyType.FullName.Contains("Int64") Then
                                        stPropName = "Int64"
                                    ElseIf tProp.PropertyType.FullName.Contains("Date") Then
                                        stPropName = "Date"
                                    ElseIf tProp.PropertyType.FullName.Contains("Decimal") Then
                                        stPropName = "Decimal"
                                    ElseIf tProp.PropertyType.FullName.Contains("Double") Then
                                        stPropName = "Double"
                                    ElseIf tProp.PropertyType.FullName.Contains("Boolean") Then
                                        stPropName = "Boolean"
                                    End If
                                End If
                                Try
                                    Select Case stPropName
                                        Case "String"
                                            tProp.SetValue(toObj, strPropValue)
                                        Case "Int16"
                                            If Not Int16.TryParse(strPropValue, iVal16) Then iVal16 = 0
                                            tProp.SetValue(toObj, iVal16)
                                        Case "Int32"
                                            If Not Int32.TryParse(strPropValue, iVal32) Then iVal32 = 0
                                            tProp.SetValue(toObj, iVal32)
                                        Case "Int64"
                                            If Not Int64.TryParse(strPropValue, iVal64) Then iVal64 = 0
                                            tProp.SetValue(toObj, iVal64)
                                        Case "Date"
                                            If Not Date.TryParse(strPropValue, dtVal) Then dtVal = Date.MinValue
                                            tProp.SetValue(toObj, dtVal)
                                        Case "DateTime"
                                            If Not Date.TryParse(strPropValue, dtVal) Then dtVal = Date.MinValue
                                            tProp.SetValue(toObj, dtVal)
                                        Case "Decimal"
                                            If Not Decimal.TryParse(strPropValue, decVal) Then decVal = 0
                                            tProp.SetValue(toObj, decVal)
                                        Case "Double"
                                            If Not Double.TryParse(strPropValue, dblVal) Then dblVal = 0
                                            tProp.SetValue(toObj, dblVal)
                                        Case "Boolean"
                                            If Boolean.TryParse(strPropValue, blnVal) Then
                                                tProp.SetValue(toObj, blnVal)
                                            Else
                                                'try to convert to an integer and then test for 0 any non zero is true
                                                If Integer.TryParse(strPropValue, intVal) Then
                                                    If intVal = 0 Then
                                                        blnVal = False
                                                    Else
                                                        blnVal = True
                                                    End If
                                                    tProp.SetValue(toObj, blnVal)
                                                Else
                                                    tProp.SetValue(toObj, False)
                                                End If
                                            End If
                                        Case Else
                                            'cannot parse
                                            Dim s As String = ""
                                            If propValue IsNot Nothing Then s = propValue.ToString
                                            strMsg &= " Cannot Copy " & fProp.Name & " invalid type " & s
                                    End Select
                                Catch ex As Exception
                                    strMsg &= ex.Message
                                    Throw
                                End Try
                            End If
                            Exit For
                        End If
                    Next
                End If
                'End If
            Next
        End SyncLock
        Return toObj

    End Function

    Friend Function convertCarrierNumbersToControl(ByRef db As NGLMASCarrierDataContext, ByVal sFilterName As String, ByRef filters As Models.AllFilters, ByRef oSecureCarrier As List(Of Integer)) As List(Of Integer)
        Dim intCarrierNumberFrom As Integer = 0
        Dim intCarrierNumberTo As Integer = 0
        Dim dicCarrierNumber As New Dictionary(Of Integer, Integer)
        Dim lCarrierControl As New List(Of Integer)
        If filters Is Nothing Then Return lCarrierControl
        Try
            If filters.FilterValues.Any(Function(x) x.filterName = sFilterName) Then
                Dim oCarrierFilters = filters.FilterValues.Where(Function(X) X.filterName = sFilterName).ToList()
                If Not oCarrierFilters Is Nothing AndAlso oCarrierFilters.Count > 0 Then
                    For Each c In oCarrierFilters
                        If Not String.IsNullOrWhiteSpace(c.filterValueFrom) Then
                            If Integer.TryParse(c.filterValueFrom, intCarrierNumberFrom) Then
                                If Not String.IsNullOrWhiteSpace(c.filterValueTo) Then
                                    If Not Integer.TryParse(c.filterValueTo, intCarrierNumberTo) Then
                                        intCarrierNumberTo = intCarrierNumberFrom
                                    End If
                                End If
                                dicCarrierNumber.Add(intCarrierNumberFrom, intCarrierNumberTo)
                            End If
                        End If
                    Next
                End If
            End If
            Dim blnTestUserCarrierRestrictions As Boolean = False
            If Not oSecureCarrier Is Nothing AndAlso oSecureCarrier.Count > 0 Then blnTestUserCarrierRestrictions = True
            If Not dicCarrierNumber Is Nothing AndAlso dicCarrierNumber.Count > 0 Then
                For Each d In dicCarrierNumber
                    Dim intCarrierFromControl As Integer? = db.Carriers.Where(Function(x) x.CarrierNumber = d.Key).Select(Function(x) x.CarrierControl).FirstOrDefault()
                    If intCarrierFromControl.HasValue AndAlso (blnTestUserCarrierRestrictions = False OrElse oSecureCarrier.Contains(intCarrierFromControl)) Then
                        'this is a valid Carrier control so test the to control number and add each carrier control number to the list
                        If d.Value > d.Key Then
                            For c As Integer = d.Key + 1 To d.Value
                                Dim intCarrierToNumber = c
                                Dim intCarrierToControl As Integer? = db.Carriers.Where(Function(x) x.CarrierNumber = intCarrierToNumber).Select(Function(x) x.CarrierControl).FirstOrDefault()
                                If intCarrierToControl.HasValue AndAlso (blnTestUserCarrierRestrictions = False OrElse oSecureCarrier.Contains(intCarrierToControl)) Then
                                    'to carrier is valid so use add it to the list
                                    lCarrierControl.Add(intCarrierToControl)
                                End If
                            Next
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("convertCarrierNumbersToControl"), db)
        End Try
        Return lCarrierControl
    End Function
    Friend Function convertCompNumbersToControl(ByRef db As NGLMASCarrierDataContext, ByVal sFilterName As String, ByRef filters As Models.AllFilters, ByRef oSecureComp As List(Of Integer)) As List(Of Integer)
        Dim intCompNumberFrom As Integer = 0
        Dim intCompNumberTo As Integer = 0
        Dim dicCompNumber As New Dictionary(Of Integer, Integer)
        Dim lCompControl As New List(Of Integer)
        If filters Is Nothing Then Return lCompControl
        Try
            If filters.FilterValues.Any(Function(x) x.filterName = sFilterName) Then
                Dim oCompFilters = filters.FilterValues.Where(Function(X) X.filterName = sFilterName).ToList()
                If Not oCompFilters Is Nothing AndAlso oCompFilters.Count > 0 Then
                    For Each c In oCompFilters
                        If Not String.IsNullOrWhiteSpace(c.filterValueFrom) Then
                            If Integer.TryParse(c.filterValueFrom, intCompNumberFrom) Then
                                If Not String.IsNullOrWhiteSpace(c.filterValueTo) Then
                                    If Not Integer.TryParse(c.filterValueTo, intCompNumberTo) Then
                                        intCompNumberTo = intCompNumberFrom
                                    End If
                                End If
                                dicCompNumber.Add(intCompNumberFrom, intCompNumberTo)
                            End If
                        End If
                    Next
                End If
            End If
            Dim blnTestUserCompRestrictions As Boolean = False
            If Not oSecureComp Is Nothing AndAlso oSecureComp.Count > 0 Then blnTestUserCompRestrictions = True
            If Not dicCompNumber Is Nothing AndAlso dicCompNumber.Count > 0 Then
                For Each d In dicCompNumber
                    Dim intCompFromControl As Integer? = db.CompRefCarriers.Where(Function(x) x.CompNumber = d.Key).Select(Function(x) x.CompControl).FirstOrDefault()
                    If intCompFromControl.HasValue AndAlso (blnTestUserCompRestrictions = False OrElse oSecureComp.Contains(intCompFromControl)) Then
                        'this is a valid company control so test the to control number and add each company control number to the list
                        If d.Value > d.Key Then
                            For c As Integer = d.Key + 1 To d.Value
                                Dim intCompToNumber = c
                                Dim intCompToControl As Integer? = db.CompRefCarriers.Where(Function(x) x.CompNumber = intCompToNumber).Select(Function(x) x.CompControl).FirstOrDefault()
                                If intCompToControl.HasValue AndAlso (blnTestUserCompRestrictions = False OrElse oSecureComp.Contains(intCompFromControl)) Then
                                    'to company is valid so add it to the list
                                    lCompControl.Add(intCompToControl)
                                End If
                            Next
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("convertCompNumbersToControl"), db)
        End Try
        Return lCompControl
    End Function

    Friend Function getLEAdminControlByLegalEntityName(ByVal sLegalEntityName As String) As Integer
        Dim iRet As Integer = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try

                Dim oLegalEntity = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminLegalEntity.Equals(sLegalEntityName)).FirstOrDefault()
                If Not oLegalEntity Is Nothing Then
                    iRet = oLegalEntity.LEAdminControl
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getLEAdminControlByLegalEntityName"), db)
            End Try
        End Using
        Return iRet
    End Function

    ''' <summary>
    ''' Uses sKey to lookup the localized value from the database, then uses args to format the localized string.
    ''' If a record does not exists for sKey save strDefault to cmLocalizeKeyValuePair as English.
    ''' Does not thow any errors as this could break other error handlers and throw the wrong message, instead just return strDefault
    ''' </summary>
    ''' <param name="sKey">Maps to field cmLocalKey in table cmLocalizeKeyValuePair</param>
    ''' <param name="strDefault">Maps to field cmLocalValue in table cmLocalizeKeyValuePair</param>
    ''' <param name="args">Array of objects to pass to the String.Format() method</param>
    ''' <returns>String</returns>
    ''' <remarks>Created By LVV on 10/23/19</remarks>
    Protected Function formatLocalizedString(ByVal sKey As String, ByVal strDefault As String, ByVal args() As Object) As String
        Dim strRet As String = strDefault
        Try
            Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Me.Parameters)
            Dim strLocal As String = ""
            Dim strFormat As String = ""
            strLocal = oLocalize.GetLocalizedValueByKey(sKey, strDefault)
            If Not String.IsNullOrWhiteSpace(strLocal) Then strFormat = String.Format(strLocal, args)
            If Not String.IsNullOrWhiteSpace(strFormat) Then strRet = strFormat
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            'ignore any errors just return default
        End Try
        Return strRet
    End Function

    ''' <summary>
    ''' Uses <paramref name="sKey"/> to lookup the localized value from the database.
    ''' If a record does not exists for <paramref name="sKey"/> save <paramref name="strDefault"/> to cmLocalizeKeyValuePair as English.
    ''' Does not thow any errors as this could break other error handlers and throw the wrong message, instead just return <paramref name="strDefault"/>
    ''' </summary>
    ''' <param name="sKey">Maps to field cmLocalKey in table cmLocalizeKeyValuePair</param>
    ''' <param name="strDefault">Maps to field cmLocalValue in table cmLocalizeKeyValuePair</param>
    ''' <returns>String</returns>
    ''' <remarks>Created By LVV on 10/23/19</remarks>
    Public Function getLocalizedString(ByVal sKey As String, ByVal strDefault As String) As String
        Dim strRet As String = strDefault
        Try
            Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Me.Parameters)
            Return oLocalize.GetLocalizedValueByKey(sKey, strDefault)
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            'ignore any errors just return default
        End Try
        Return strRet
    End Function


    Public Overridable Sub logSystemError(ByVal pars As sysErrorParameters, ByVal Message As String, ByVal Record As String)
        If pars Is Nothing Then
            pars = New sysErrorParameters() With {.Message = Message, .Record = Record}
        End If
        With pars
            NGLSystemData.CreateSystemErrorByMessage(.Message, Parameters.UserName, .Procedure, .Record, .Number, .Severity, .ErrState, .LineNber)
        End With
    End Sub
    Public Overridable Sub logSystemError(ByVal Message As String)

        Dim pars = New sysErrorParameters() With {.Message = Message, .Record = "No Record"}
        With pars
            NGLSystemData.CreateSystemErrorByMessage(.Message, Parameters.UserName, .Procedure, .Record, .Number, .Severity, .ErrState, .LineNber)
        End With
    End Sub

    Public Overridable Sub logSystemError(ByVal execp As Exception,
                                          Procedure As String,
                                          Optional ByVal Record As String = "",
                                          Optional enmErrState As sysErrorParameters.sysErrorState = sysErrorParameters.sysErrorState.UserLevelFault,
                                          Optional enmSeverity As sysErrorParameters.sysErrorSeverity = sysErrorParameters.sysErrorSeverity.Unexpected)


        Dim pars As New sysErrorParameters With {.ErrState = enmErrState,
                                                     .Message = execp.ToString,
                                                     .Procedure = Procedure,
                                                     .Record = Record,
                                                     .Severity = enmSeverity}
        logSystemError(pars, "", "")

    End Sub


    Protected Overridable Function calculateBookDestStopNumber(ByVal BookStopNoOld As Integer,
                                                               ByVal BookStopNoNew As Integer,
                                                               ByVal BookDestStopNumberOld As Integer) As Integer
        Dim intRet As Integer = 2
        Using operation = Logger.StartActivity("calculateBookDestStopNumber(BookStopNoOld: {BookStopNoOld}, BookStopNoNew: {BookStopNoNew}, BookDestStopNumberOld: {BookDestStopNumberOld} ")
            Try
                'calculate the BookDestStopNumber if the BookStopNo is changed by the consumer of the service.
                If BookStopNoOld < 1 Then BookStopNoOld = 1
                If BookStopNoNew < 1 Then BookStopNoNew = 1
                If BookDestStopNumberOld < 2 Then BookDestStopNumberOld = 2
                If BookStopNoOld <> BookStopNoNew Then
                    Dim difference = BookStopNoOld - BookStopNoNew
                    BookDestStopNumberOld = BookDestStopNumberOld - difference
                End If
                intRet = BookDestStopNumberOld
                If intRet < 2 Then intRet = 2
            Catch ex As Exception
                Logger.Error(ex, "calculateBookDestStopNumber")
                'ignore any errors and just return the default
                operation.Complete()
            End Try
        End Using
        Return intRet
    End Function

    Protected Overloads Function incrementID(ByRef seed As Integer) As Integer
        seed += 1
        Return seed
    End Function






#End Region


#Region "Static Methods"

    ''' <summary>
    ''' returns true if the uses group category is configure via  the parameter value dblCarrierCostUpchargeLimitVisibility to show the carrier cost
    ''' Sales =  0 ' 11  SalesRep
    ''' Customer = 1 ' 11  SalesRep and 10  Customers
    ''' NEXTrack = 2 ' 11  SalesRep and 10  Customers and 9 NEXTrackUsers
    '''All others can see the raw carrier cost
    ''' </summary>
    ''' <param name="dblCarrierCostUpchargeLimitVisibility"></param>
    ''' <param name="iUserGroupCategory"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' created by RHR for v-8.5.4.001 
    ''' </remarks>
    Public Shared Function CanUserSeeCarrierCost(ByVal dblCarrierCostUpchargeLimitVisibility As Double, ByVal iUserGroupCategory As Integer) As Boolean
        Dim blnCanSeeCost As Boolean = True
        Dim iVal As Integer = CInt(dblCarrierCostUpchargeLimitVisibility)
        Select Case iVal
            Case NGLLookupDataProvider.UpchargeLimitVisibility.Sales
                If (iUserGroupCategory = 11) Then blnCanSeeCost = False
            Case NGLLookupDataProvider.UpchargeLimitVisibility.Customer
                If (iUserGroupCategory = 11 Or iUserGroupCategory = 10) Then blnCanSeeCost = False
            Case NGLLookupDataProvider.UpchargeLimitVisibility.NEXTrack
                If (iUserGroupCategory = 11 Or iUserGroupCategory = 10 Or iUserGroupCategory = 9) Then blnCanSeeCost = False
            Case Else
                blnCanSeeCost = True
        End Select
        Return blnCanSeeCost
    End Function
#End Region
End Class
