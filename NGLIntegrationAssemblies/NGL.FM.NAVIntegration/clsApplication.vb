
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports System.IO
Imports System.Reflection
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports System.Xml
Imports System.Xml.Serialization
'NGL Imports
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports DTran = NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NGL.Core
Imports TMS = NGL.FreightMaster.Integration
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL
Imports LTS = NGL.FreightMaster.Data.LTS
Imports BCIntegration = NGL.DTMS.CMDLine.BCIntegration
Imports Comp = NGL.DTMS.CMDLine.BCIntegration.Company
Imports Carr = NGL.DTMS.CMDLine.BCIntegration.Carrier
Imports Lane = NGL.DTMS.CMDLine.BCIntegration.Lane
Imports Haz = NGL.DTMS.CMDLine.BCIntegration.Haz
Imports Book = NGL.DTMS.CMDLine.BCIntegration.Book
Imports Pallet = NGL.DTMS.CMDLine.BCIntegration.Plt
Imports Payable = NGL.DTMS.CMDLine.BCIntegration.Pay
Imports Pick = NGL.DTMS.CMDLine.BCIntegration.Pick
Imports AP = NGL.DTMS.CMDLine.BCIntegration.AP
Imports NGL.DTMS.CMDLine.BCIntegration
Imports NGL.DTMS.CMDLine


Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass

#Region " Enums"

    Public Enum MstrDataType
        Comp = 0
        Cust = 1
        Carrier = 2
    End Enum

#Disable Warning BC40004 ' enum 'IntegrationModule' conflicts with enum 'IntegrationModule' in the base class 'NGLCommandLineBaseClass' and should be declared 'Shadows'.
    Public Enum IntegrationModule
#Enable Warning BC40004 ' enum 'IntegrationModule' conflicts with enum 'IntegrationModule' in the base class 'NGLCommandLineBaseClass' and should be declared 'Shadows'.
        Company
        Carrier
        Lane
        Order
        Hazmat
        PalletType
        PickList
        APExport
        Payables
    End Enum
#End Region
#Region "Base Class Overloads "

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Modified By LVV 2/19/16 v-7.0.5.0
    ''' Added call to getNewTaskParameters()
    ''' </remarks>
    Public Overrides Function getTaskParameters() As Boolean
        Dim blnRet As Boolean = False
        Try
            If Not GlobalTaskParameters Is Nothing Then
                With GlobalTaskParameters
                    AutoRetry = .GlobalAutoRetry
                    AdminEmail = .GlobalAdminEmail
                    GroupEmail = .GlobalGroupEmail
                    FromEmail = .GlobalFromEmail
                    SMTPServer = .GlobalSMTPServer
                    SaveOldLog = .GlobalSaveOldLogs
                    KeepLogDays = .GlobalKeepLogDays
                    'the command line parameter overrides the global debug mode but only if it is true
                    If Not Debug Then
                        Debug = .GlobalDebugMode
                    End If
                    GlobalFuelIndexUpdateEmailNotification = .GlobalFuelIndexUpdateEmailNotification
                    GlobalFuelIndexUpdateEmailNotificationValue = .GlobalFuelIndexUpdateEmailNotificationValue
                    GlobalCarrierContractExpiredEmailNotification = .GlobalCarrierContractExpiredEmailNotification
                    GlobalCarrierContractExpiredEmailNotificationValue = .GlobalCarrierContractExpiredEmailNotificationValue
                    GlobalCarrierExposureAllEmailNotification = .GlobalCarrierExposureAllEmailNotification
                    GlobalCarrierExposureAllEmailNotificationValue = .GlobalCarrierExposureAllEmailNotificationValue
                    GlobalCarrierExposurePerShipmentEmailNotification = .GlobalCarrierExposurePerShipmentEmailNotification
                    GlobalCarrierExposurePerShipmentEmailNotificationValue = .GlobalCarrierExposurePerShipmentEmailNotificationValue
                    GlobalCarrierInsuranceExpiredEmailNotification = .GlobalCarrierInsuranceExpiredEmailNotification
                    GlobalCarrierInsuranceExpiredEmailNotificationValue = .GlobalCarrierInsuranceExpiredEmailNotificationValue
                    GlobalOutdatedNoLanePOEmailNotification = .GlobalOutdatedNoLanePOEmailNotification
                    GlobalOutdatedNoLanePOEmailNotificationValue = .GlobalOutdatedNoLanePOEmailNotificationValue
                    GlobalOutdatedNStatusEmailNotification = .GlobalOutdatedNStatusEmailNotification
                    GlobalOutdatedNStatusEmailNotificationValue = .GlobalOutdatedNStatusEmailNotificationValue
                    GlobalPOsWaitingEmailNotification = .GlobalPOsWaitingEmailNotification
                    GlobalPOsWaitingEmailNotificationValue = .GlobalPOsWaitingEmailNotificationValue
                    GlobalDefaultLoadAcceptAllowedMinutes = .GlobalDefaultLoadAcceptAllowedMinutes
                    NEXTStopAcctNo = .NEXTStopAcctNo
                    NEXTStopContact = .NEXTStopContact
                    NEXTStopHotLoadAccountName = .NEXTStopHotLoadAccountName
                    NEXTStopHotLoadContact = .NEXTStopHotLoadContact
                    NEXTStopHotLoadURL = .NEXTStopHotLoadURL
                    NEXTStopPhone = .NEXTStopPhone
                    NEXTStopURL = .NEXTStopURL
                    NEXTrackURL = .NEXTrackURL
                    NEXTRackDatabase = .NEXTRackDatabase
                    NEXTRackDatabaseServer = .NEXTRackDatabaseServer
                    GlobalSMTPUser = .GlobalSMTPUser
                    GlobalSMTPPass = .GlobalSMTPPass
                    ReportServerURL = .ReportServerURL
                    ReportServerUser = .ReportServerUser
                    ReportServerPass = .ReportServerPass
                    ReportServerDomain = .ReportServerDomain
                End With

                'Added By LVV 2/19/16 v-7.0.5.0
                processNewTaskParameters(GlobalTaskParameters)

            End If
            blnRet = True
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            LogError(Source & " Warning:  Read Global Task Parameters Failed", ex.Detail.ToString(ex.Reason.ToString()), Me.AdminEmail)
        Catch ex As Exception
            LogError(Source & " Warning:  Read Global Task Parameters Failed", "Read Global Task Parameters Failed", Me.AdminEmail, ex)
        End Try

        Return blnRet
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/14/2016 
    '''   orverrides the default behavior of connecting to the db to validate the connection string 
    '''   and create the log file.  we now use web services for all connection to the db.  this override
    '''   creates the log file only.
    ''' </remarks>
    Public Overrides Function validateDatabase() As Boolean
        Dim blnRet As Boolean = True

        'set the default value for the log file

        If Not System.IO.Directory.Exists("C:\Data") Then
            System.IO.Directory.CreateDirectory("C:\Data")
        End If
        If Not System.IO.Directory.Exists("C:\Data\TMSLogs") Then
            System.IO.Directory.CreateDirectory("C:\Data\TMSLogs")
        End If
        LogFile = "C:\Data\TMSLogs\" & Source & "." & Database & ".log"
        openLog()

        If LogFile <> "C:\Data\TMSLogs\" & Source & "." & Database & ".log" Then
            'reset the value for the log file if the database name has changed
            LogFile = "C:\Data\TMSLogs\" & Source & "." & Database & ".log"
            closeLog(0)
            openLog()
        End If
        If Me.Debug Then Log("Ready!")

        Return blnRet

    End Function

    Public Overrides Sub LogError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, ByVal e As Exception)
        Try
            If Me.Debug Then
                logMessage = logMessage & e.ToString()
            Else
                logMessage = logMessage & e.Message
            End If
        Catch ex As Exception
            'do nothing
        End Try
        LogError(strSubject, logMessage, strMailTo)
    End Sub

    Public Overrides Sub LogError(strSubject As String, logMessage As String, strMailTo As String)
        Try

            If dictErrorsToLog Is Nothing Then dictErrorsToLog = New Dictionary(Of String, List(Of clsErrorMessages))
            Dim oMsg As New clsErrorMessages(strSubject, logMessage)
            Dim lMessages As New List(Of clsErrorMessages)
            If dictErrorsToLog.ContainsKey(strMailTo) Then
                lMessages = dictErrorsToLog(strMailTo)
                If lMessages Is Nothing Then lMessages = New List(Of clsErrorMessages)
                lMessages.Add(oMsg)
                dictErrorsToLog(strMailTo) = lMessages
            Else
                lMessages.Add(oMsg)
                dictErrorsToLog.Add(strMailTo, lMessages)
            End If
        Catch ex As Exception
            'do nothing when logging errors
        End Try
    End Sub

    ''' <summary>
    ''' this method call the base calss LogError method for each email address in dictErrorsToLog 
    ''' returns had error flag true or false
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public Function LogAllErrors() As Boolean
        Dim blnHadErrors As Boolean = False
        Try
            If dictErrorsToLog Is Nothing OrElse dictErrorsToLog.Count < 1 Then Return blnHadErrors 'return false
            Dim strSubject = "Multiple NAV Integration Errors"
            ' each dictErrorsToLog record is unique for mail to in Key. dictErrorsToLog groups all messages for each send to as one email
            For Each oErr In dictErrorsToLog
                blnHadErrors = True
                Dim lMessages As List(Of clsErrorMessages) = oErr.Value
                Dim sToEmail = oErr.Key
                Dim sbMsg As New StringBuilder()
                Dim sSubj As String = strSubject
                'if we only have one message in the list we use this as the subject
                If lMessages.Count = 1 Then
                    sSubj = lMessages(0).Subject
                End If
                For Each m In lMessages
                    sbMsg.Append(String.Format("Subject: {0} {1} Body: {2} {1}", m.Subject, vbCrLf, m.Message))
                Next
                MyBase.LogError(sSubj, sbMsg.ToString(), sToEmail)
            Next
        Catch ex As Exception
            'do nothing when we log errors
        End Try
        Return blnHadErrors

    End Function

#End Region

#Region "Properties "


    Private _dictErrorsToLog As New Dictionary(Of String, List(Of clsErrorMessages))
    ''' <summary>
    ''' dictionary of error messages to process
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' This dictionary stores messages associated with an email address,  the key is the email address and the message is a list of clsErrorMessages
    ''' </remarks>
    Public Property dictErrorsToLog() As Dictionary(Of String, List(Of clsErrorMessages))
        Get
            Return _dictErrorsToLog
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of clsErrorMessages)))
            _dictErrorsToLog = value
        End Set
    End Property

    Private _WCFParameters As DAL.WCFParameters
    Public Overloads Property WCFParameters() As DAL.WCFParameters
        Get
            If _WCFParameters Is Nothing Then
                'Note: WCFAuthCode = "NGLSystem" does not validate user when ValidateAccess = False 
                _WCFParameters = New DAL.WCFParameters With {.UserName = "",
                                                             .Database = Me.Database,
                                                             .DBServer = Me.DBServer,
                                                             .ConnectionString = Me.ConnectionString,
                                                             .WCFAuthCode = "NGLSystem",
                                                             .ValidateAccess = False}
            End If
            Return _WCFParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _WCFParameters = value
        End Set
    End Property




    Private _NGLDynamicsTMSSettingData As DAL.NGLDynamicsTMSSettingData
    ''' <summary>
    ''' Local instance of the DAL.NGLDynamicsTMSSettingData class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLDynamicsTMSSettingData() As DAL.NGLDynamicsTMSSettingData
        Get
            If _NGLDynamicsTMSSettingData Is Nothing Then _NGLDynamicsTMSSettingData = New DAL.NGLDynamicsTMSSettingData(WCFParameters)
            Return _NGLDynamicsTMSSettingData
        End Get
        Set(value As DAL.NGLDynamicsTMSSettingData)
            _NGLDynamicsTMSSettingData = value
        End Set
    End Property

    Private _NGLBatchProcessDataProvider As DAL.NGLBatchProcessDataProvider
    ''' <summary>
    ''' Local instance of the DAL.NGLBatchProcessDataProvider class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLBatchProcessDataProvider() As DAL.NGLBatchProcessDataProvider
        Get
            If _NGLBatchProcessDataProvider Is Nothing Then _NGLBatchProcessDataProvider = New DAL.NGLBatchProcessDataProvider(WCFParameters)
            Return _NGLBatchProcessDataProvider
        End Get
        Set(ByVal value As DAL.NGLBatchProcessDataProvider)
            _NGLBatchProcessDataProvider = value
        End Set
    End Property

    Private _NGLSystemDataProvider As DAL.NGLSystemDataProvider
    ''' <summary>
    ''' Local instance of the DAL.NGLSystemDataProvider class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLSystemDataProvider() As DAL.NGLSystemDataProvider
        Get
            If _NGLSystemDataProvider Is Nothing Then _NGLSystemDataProvider = New DAL.NGLSystemDataProvider(WCFParameters)
            Return _NGLSystemDataProvider
        End Get
        Set(value As DAL.NGLSystemDataProvider)
            _NGLSystemDataProvider = value
        End Set
    End Property

    Private _NGLBookRevenueBLL As BLL.NGLBookRevenueBLL
    ''' <summary>
    ''' Local instance of the BLL.NGLBookRevenueBLL class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLBookRevenueBLL() As BLL.NGLBookRevenueBLL
        Get
            If _NGLBookRevenueBLL Is Nothing Then _NGLBookRevenueBLL = New BLL.NGLBookRevenueBLL(WCFParameters)
            Return _NGLBookRevenueBLL
        End Get
        Set(ByVal value As BLL.NGLBookRevenueBLL)
            _NGLBookRevenueBLL = value
        End Set
    End Property

    Private _NGLCarrierData As DAL.NGLCarrierData
    ''' <summary>
    ''' Local instance of the DAL.NGLCarrierData class
    ''' If WCFParameters change set to nothing then get new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NGLCarrierData() As DAL.NGLCarrierData
        Get
            If _NGLCarrierData Is Nothing Then _NGLCarrierData = New DAL.NGLCarrierData(WCFParameters)
            Return _NGLCarrierData
        End Get
        Set(ByVal value As DAL.NGLCarrierData)
            _NGLCarrierData = value
        End Set
    End Property




    Private _GlobalTaskParameters As DTO.GlobalTaskParameters
    ''' <summary>
    ''' Local instance of DTO.GlobalTaskParameters class 
    ''' If WCFParameters change set to nothing then get a new instance
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GlobalTaskParameters() As DTO.GlobalTaskParameters
        Get
            If _GlobalTaskParameters Is Nothing Then
                _GlobalTaskParameters = getSystemParameters()
            End If
            Return _GlobalTaskParameters
        End Get
        Set(ByVal value As DTO.GlobalTaskParameters)
            _GlobalTaskParameters = value
        End Set
    End Property

    Private _ERPTestingOn As Boolean = False
    Public Property ERPTestingOn() As Boolean
        Get
            Return _ERPTestingOn
        End Get
        Set(ByVal value As Boolean)
            _ERPTestingOn = value
        End Set
    End Property


    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    ' Begin Modified by RHR for v-8.4.0.003 on 10/19/2021
    Private _iTMSAPRetryMilliSeconds As Integer = 0
    Public Property iTMSAPRetryMilliSeconds() As Integer
        Get
            Return _iTMSAPRetryMilliSeconds
        End Get
        Set(value As Integer)
            _iTMSAPRetryMilliSeconds = value
        End Set
    End Property

    Private _iTMSAPRetryAttempts As Integer = 0
    Public Property iTMSAPRetryAttempts() As Integer
        Get
            Return _iTMSAPRetryAttempts
        End Get
        Set(value As Integer)
            _iTMSAPRetryAttempts = value
        End Set
    End Property

    Private _iTMSPickRetryMilliSeconds As Integer = 0
    Public Property iTMSPickRetryMilliSeconds() As Integer
        Get
            Return _iTMSPickRetryMilliSeconds
        End Get
        Set(value As Integer)
            _iTMSPickRetryMilliSeconds = value
        End Set
    End Property

    Private _iTMSPickRetryAttempts As Integer = 0
    Public Property iTMSPickRetryAttempts() As Integer
        Get
            Return _iTMSPickRetryAttempts
        End Get
        Set(value As Integer)
            _iTMSPickRetryAttempts = value
        End Set
    End Property
    ' End Modified by RHR for v-8.4.0.003 on 10/19/2021

#End Region

#Region "DAL Methods "

    Protected Function getDynamicsTMSSettings(ByVal LegalEntity As String) As List(Of DTO.DynamicsTMSSetting)

        Dim oDTMSSettings As New List(Of DTO.DynamicsTMSSetting)
        Dim oDTMSData As DTO.DynamicsTMSSetting
        Try
            If String.IsNullOrWhiteSpace(LegalEntity) Then
                oDTMSSettings = NGLDynamicsTMSSettingData.GetDynamicsTMSSettings().ToList()
            Else
                oDTMSData = NGLDynamicsTMSSettingData.GetDynamicsTMSSettingFiltered(LegalEntity)
                If oDTMSData Is Nothing Then oDTMSSettings.Add(oDTMSData)
            End If
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", ex.Detail.ToString(ex.Reason.ToString()), Me.AdminEmail)
        Catch ex As Exception
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", "Read Dynamics TMS Integration Settings Failed", Me.AdminEmail, ex)
        End Try
        Return oDTMSSettings
    End Function

    Protected Function CreateNAVTMSSettings(ByVal sDTMSLegalEntity As String,
                                            ByVal sDTMSNAVWebServiceURL As String,
                                            ByVal sDTMSNAVUserName As String,
                                            ByVal sDTMSNAVPassword As String,
                                            Optional ByVal iDTMSPicklistMaxRetry As Integer = 1,
                                            Optional ByVal iDTMSPicklistRetryMinutes As Integer = 30,
                                            Optional ByVal iDTMSPicklistMaxRowsReturned As Integer = 10,
                                            Optional ByVal bDTMSPicklistAutoConfirmation As Boolean = False,
                                            Optional ByVal iDTMSAPExportMaxRetry As Integer = 5,
                                            Optional ByVal iDTMSAPExportRetryMinutes As Integer = 30,
                                            Optional ByVal iDTMSAPExportMaxRowsReturned As Integer = 10,
                                            Optional ByVal bDTMSAPExportAutoConfirmation As Boolean = False,
                                            Optional ByVal bDTMSNAVUseDefaultCredentials As Boolean = True,
                                            Optional ByVal sDTMSWSAuthCode As String = "NGLWSDEV",
                                            Optional ByVal sDTMSWSURL As String = "http://nglwsdev704.nextgeneration.com",
                                            Optional ByVal sDTMSWCFAuthCode As String = "NGLWCFDEV",
                                            Optional ByVal sDTMSWCFURL As String = "http://nglwcfdev704.nextgeneration.com",
                                            Optional ByVal sDTMSWCFTCPURL As String = "net.tcp://nglwcfdev704.nextgeneration.com:908") As DTO.DynamicsTMSSetting


        Try
            Dim oDTMSData As New DTO.DynamicsTMSSetting With {.DTMSLegalEntity = sDTMSLegalEntity _
                                                             , .DTMSPicklistMaxRetry = iDTMSPicklistMaxRetry _
                                                             , .DTMSPicklistRetryMinutes = iDTMSPicklistRetryMinutes _
                                                             , .DTMSPicklistMaxRowsReturned = iDTMSPicklistMaxRowsReturned _
                                                             , .DTMSPicklistAutoConfirmation = bDTMSPicklistAutoConfirmation _
                                                             , .DTMSAPExportMaxRetry = iDTMSAPExportMaxRetry _
                                                             , .DTMSAPExportRetryMinutes = iDTMSAPExportRetryMinutes _
                                                             , .DTMSAPExportMaxRowsReturned = iDTMSAPExportMaxRowsReturned _
                                                             , .DTMSAPExportAutoConfirmation = bDTMSAPExportAutoConfirmation _
                                                             , .DTMSNAVWebServiceURL = sDTMSNAVWebServiceURL _
                                                             , .DTMSNAVUserName = sDTMSNAVUserName _
                                                             , .DTMSNAVPassword = sDTMSNAVPassword _
                                                             , .DTMSNAVUseDefaultCredentials = bDTMSNAVUseDefaultCredentials _
                                                             , .DTMSWSAuthCode = sDTMSWSAuthCode _
                                                             , .DTMSWSURL = sDTMSWSURL _
                                                             , .DTMSWCFAuthCode = sDTMSWCFAuthCode _
                                                             , .DTMSWCFURL = sDTMSWCFURL _
                                                             , .DTMSWCFTCPURL = sDTMSWCFTCPURL}

            Return NGLDynamicsTMSSettingData.CreateRecord(oDTMSData)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", ex.Detail.ToString(ex.Reason.ToString()), Me.AdminEmail)
        Catch ex As Exception
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", "Read Dynamics TMS Integration Settings Failed", Me.AdminEmail, ex)
        End Try
        Return Nothing
    End Function

    Protected Function getSystemParameters() As DTO.GlobalTaskParameters
        Dim oGTPs As DTO.GlobalTaskParameters
        Try
            'get the parameter settings from the database.
            oGTPs = NGLSystemDataProvider.GetGlobalTaskParameters()
        Catch ex As Exception
            Return Nothing
        End Try
        Return oGTPs
    End Function

#End Region


    Private Function getAuthsSettngs(ByVal tSettings As TMSIntegrationSettings.vERPIntegrationSetting) As BCIntegration.oAuth2Settings
        Dim sClientId As String = If(String.IsNullOrWhiteSpace(tSettings.ERPAuthUser), tSettings.ERPUser, tSettings.ERPAuthUser)
        Dim sSecret As String = If(String.IsNullOrWhiteSpace(tSettings.ERPAuthPassword), tSettings.ERPPassword, tSettings.ERPAuthPassword)
        Dim sLegal As String = tSettings.LegalEntity
        Dim sScopeUrl = tSettings.ERPCertificate
        Dim sDataUrl = tSettings.ERPURI
        Dim sActionUrl = tSettings.ERPActionURI
        Dim sAuthUrl = tSettings.ERPAuthURI

        Return New oAuth2Settings(sClientId, sSecret, sLegal, sScopeUrl, sDataUrl, sActionUrl, sAuthUrl)

    End Function



    Private Function ConfigureInstance(ByVal sSource As String, ByVal c As clsDefaultIntegrationConfiguration) As Boolean
        Return ConfigureInstance(sSource, c.TMSDBName, c.TMSDBServer, "", c.TMSDBUser, c.TMSDBPass, c.TMSRunLegalEntity, c.Debug, c.Verbos)
    End Function

    Private Function ConfigureInstance(ByVal sSource As String,
                            ByVal DBName As String,
                            ByVal DBServer As String,
                            ByVal ConnectionSting As String,
                            ByVal DBUser As String,
                            ByVal DBPass As String,
                            ByVal LegalEntity As String,
                            ByVal Debug As Boolean,
                            ByVal Verbos As Boolean) As Boolean


        Me.Database = DBName
        Me.DBServer = DBServer
        Me.Source = sSource
        Me.LegalEntity = LegalEntity
        Me.Debug = Debug
        Me.Verbose = Verbos
        Me.SaveOldLog = False
        Me.KeepLogDays = 1

        If String.IsNullOrWhiteSpace(ConnectionSting) Then
            'build the connection string
            If String.IsNullOrWhiteSpace(DBUser) Or String.IsNullOrWhiteSpace(DBPass) Then
                ConnectionSting = String.Format("Data Source={0};Initial Catalog={1}; Integrated Security=SSPI;", DBServer, DBName)
            Else
                ConnectionSting = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", DBServer, DBName, DBUser, DBPass)
            End If
        End If
        Me.ConnectionString = ConnectionSting
        'Me.SaveOldLog = False
        'Me.KeepLogDays = 1
        'open log and validate database
        If Not validateDatabase() Then Return False
        If Me.Verbose Then Me.Log(Source & " Running")

        If Not getTaskParameters() Then Return False
        fillConfig()
        Return True
    End Function

    Public Sub CleanUpTestData(ByVal LegalEntity As String)
        NGLBatchProcessDataProvider.UtilityRemoveAllTestDataByLegalEntity(LegalEntity)
    End Sub

    Public Function ProcessDataUnitTest(ByVal UnitTestKeys As clsUnitTestKeys,
                                        Optional ByVal DeleteOnFinally As Boolean = True,
                                        Optional ByVal Finalize As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        If UnitTestKeys Is Nothing OrElse String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
            LogError("Invalid Unit Test Keys")
            Return False
        End If

        Dim intCompControl As Integer
        Dim intCarrierControl As Integer
        Dim intLaneControl As Integer
        Dim dblSampleFreightCost As Integer = 1000.0
        If Not ConfigureInstance(UnitTestKeys.Source, UnitTestKeys.DBName, UnitTestKeys.DBServer, UnitTestKeys.ConnectionSting, UnitTestKeys.DBUser, UnitTestKeys.DBPass, UnitTestKeys.LegalEntity, UnitTestKeys.Debug, UnitTestKeys.Verbos) Then
            LogError("Invalid Instance Configuration")
            Return False
        End If
        Try
            Log("Begin Unit Test ")
            Dim Processed As New List(Of Integer)
            Dim Orders As New List(Of String)
            If Not processCompanyUnitTest(Processed, UnitTestKeys) Then Return False
            If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
                intCompControl = Processed(0)
            End If
            Processed = New List(Of Integer)
            If Not processCarrierUnitTest(Processed, UnitTestKeys) Then Return False
            If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
                intCarrierControl = Processed(0)
            End If
            If intCarrierControl = 0 Then
                LogError("Unit Test Get Carrier Failed", "Unit Test Failed to get a carrier control number back.", Me.AdminEmail)
                Throw New ApplicationException(Source & " Unit Test Get Carrier Failed")
            End If
            Log("Unit Test Configure Silent Tender Settings")
            NGLBatchProcessDataProvider.UtilityAllowSilentTender(UnitTestKeys.LegalEntity)
            Processed = New List(Of Integer)
            If Not processPalletTypeUnitTest(UnitTestKeys) Then Return False
            If Not processHazmatUnitTest(UnitTestKeys) Then Return False
            If Not processLaneUnitTest(Processed, UnitTestKeys) Then Return False
            If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
                intLaneControl = Processed(0)
            End If
            Processed = New List(Of Integer)
            If Not processOrderUnitTest(Orders, UnitTestKeys) Then Return False
            Log("Unit Test Do Spot Rate for Order Number: " & UnitTestKeys.OrderNumber)
            If Not NGLBookRevenueBLL.DoAutoSpotRateWithSave(UnitTestKeys.OrderNumber, 0, intCarrierControl, UnitTestKeys.FreightCost, Finalize) Then
                LogError("Unit Test Spot Rate Failed", "Unit Test Failed Cannot Apply Spot Rate ", Me.AdminEmail)
                Throw New ApplicationException(Source & " Unit Test Failed Cannot Apply Spot Rate")
            End If
            If Not processPicklistUnitTest(UnitTestKeys) Then Return False
            If Finalize Then
                Log("Unit Test Mass Update ALL Freight Bills")
                Dim oFreightBills = NGLBatchProcessDataProvider.UtilityMassUpdateAllTestFreightBills(UnitTestKeys.LegalEntity)
                If Not processAPExportUnitTest(UnitTestKeys) Then Return False
                If Not oFreightBills Is Nothing AndAlso oFreightBills.Count() > 0 Then
                    For Each fb In oFreightBills
                        Log("Unit Test Processing Payable for Freight Bill: " & fb.BookFinAPBillNumber)
                        UnitTestKeys.FreightBillNumber = fb.BookFinAPBillNumber
                        If Not processPayablesUnitTest(UnitTestKeys) Then Return False
                    Next
                ElseIf DeleteOnFinally = False Then
                    Log("Unit Test No Freight Bills available for payable processing because DeleteOnFinally is off so previous tests may have already processed freight bills")
                Else
                    LogError("Unit Test No Freight Bills available for payable processing.")
                    Return False
                End If
            Else
                Log("Finalize is off No Freight Bill tests were run.")
            End If
            Log("Unit Test Complete")
            blnRet = True
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected NAV Process Data Error", Source & " Could not process any integration requests; the actual error is:  ", AdminEmail, ex)
            Throw
        Finally
            Dim blnHadErrors As Boolean = LogAllErrors()
            Try
                If DeleteOnFinally Then CleanUpTestData(UnitTestKeys.LegalEntity)
            Catch ex As Exception
                System.Diagnostics.Debug.Assert(False, ex.Message)
            End Try
            Me.closeLog(0)
            If blnHadErrors Then
                Throw New System.ApplicationException("Unit Test Errors were found,  please check the error logs for details.")
            End If
        End Try
        Return blnRet
    End Function

    Public Function ProcessDataTest(ByVal sSource As String, ByVal c As clsDefaultIntegrationConfiguration) As Boolean

        Dim blnRet As Boolean = False
        If Not ConfigureInstance(sSource, c) Then
            LogError(sSource & " Data Test Invalid Instance Configuration")
            Return False
        End If
        Dim intCompControl As Integer
        Dim intCarrierControl As Integer
        Dim intLaneControl As Integer
        Dim dblSampleFreightCost As Integer = c.ERPTestFreightCost
        Try
            Log("Begin Process Data TEST")
            Dim Processed As New List(Of Integer)
            Dim Orders As New List(Of String)
            Dim oSettings As New TMSIntegrationSettings.vERPIntegrationSetting
            Me.LegalEntity = c.ERPTestLegalEntity
            With oSettings
                .ERPAuthCode = c.ERPTestAuthCode
                .ERPAuthPassword = c.ERPTestAuthPassword
                .ERPAuthUser = c.ERPTestAuthUser
                .ERPExportAutoConfirmation = c.ERPTestExportAutoConfirmation
                .ERPExportMaxRetry = c.ERPTestExportMaxRetry
                .ERPExportMaxRowsReturned = c.ERPTestExportMaxRowsReturned
                .ERPExportRetryMinutes = c.ERPTestExportRetryMinutes
                .ERPURI = c.ERPTestURI
                .LegalEntity = c.ERPTestLegalEntity
                .TMSAuthCode = c.TMSTestServiceAuthCode
                .TMSAuthPassword = c.TMSTestServiceAuthPassword
                .TMSAuthUser = c.TMSTestServiceAuthUser
                .TMSURI = c.TMSTestServiceURI
            End With
            'update the ERP Testing Status Flag using web services if available
            'updateERPTestingStatus(oSettings)
            If Not processCompanyData(oSettings, Processed) Then Return False
            If ERPTestingOn Then
                Log("The NAV test flag is on; all data will be processed then deleted after payable settlement data is received.")
            End If
            If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
                intCompControl = Processed(0)
            End If
            Processed = New List(Of Integer)
            If Not processCarrierData(oSettings, Processed) Then Return False
            If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
                intCarrierControl = Processed(0)
            End If
            If intCarrierControl = 0 Then
                LogError(sSource & " Data Test Get Carrier Failed", sSource & " Data Test Failed to get a carrier control number back.", Me.AdminEmail)
                Throw New ApplicationException(Source & " Data Test Get Carrier Failed")
            End If
            Log(sSource & " Data Test Configure Silent Tender Settings")
            NGLBatchProcessDataProvider.UtilityAllowSilentTender(LegalEntity)
            Processed = New List(Of Integer)
            If Not processPalletTypeData(oSettings) Then Return False
            If Not processHazmatData(oSettings) Then Return False
            If Not processLaneData(oSettings, Processed) Then Return False
            If Not Processed Is Nothing AndAlso Processed.Count() > 0 Then
                intLaneControl = Processed(0)
            End If
            Processed = New List(Of Integer)

            If Not processOrderData(oSettings, oSettings, Orders) Then Return False
            If Not Orders Is Nothing AndAlso Orders.Count() > 0 And ERPTestingOn Then
                If intCarrierControl = 0 Then
                    'we need to get the latest carrier for this legal entity
                    Log(sSource & " Data Test Get Latest Carrier By Legal Entity")
                    Dim oCarrier As DTO.Carrier = NGLCarrierData.GetLatestCarrierFilteredByLegalEntity(LegalEntity)
                    If oCarrier Is Nothing OrElse oCarrier.CarrierControl = 0 Then
                        LogError(sSource & " Data Test Get Carrier Failed", sSource & " Data Test Failed to get a carrier for legal entity:  " & LegalEntity, Me.AdminEmail)
                        Throw New ApplicationException(sSource & " Data Test Get Carrier Failed")
                    End If
                    intCarrierControl = oCarrier.CarrierControl
                End If
                For Each sOrderNumber In Orders
                    Log("NAV Data Test Do Spot Rate for Order Number: " & sOrderNumber)
                    If Not NGLBookRevenueBLL.DoAutoSpotRateWithSave(sOrderNumber, 0, intCarrierControl, dblSampleFreightCost, True) Then
                        LogError("NAV Data Test Spot Rate Failed", "NAV Data Test Failed Cannot Apply Spot Rate for order nunmber:  " & sOrderNumber & vbCrLf & "Check the address information.", Me.AdminEmail)
                        'Throw New ApplicationException(sSource & " Data Test Failed Cannot Apply Spot Rate")
                    End If
                Next

            End If
            If Not processPicklistData(oSettings) Then Return False
            If ERPTestingOn Then
                Log(sSource & " Data Test Mass Update ALL Freight Bills")
                Dim oFreightBills = NGLBatchProcessDataProvider.UtilityMassUpdateAllTestFreightBills(LegalEntity)
            End If
            If Not processAPExportData(oSettings) Then Return False

            If processPayablesData(oSettings) And ERPTestingOn Then
                Try
                    CleanUpTestData(LegalEntity)
                Catch ex As Exception

                End Try
            End If
            Log("Process Data TEST Complete")
            blnRet = True

        Catch ex As Exception
            LogError(Source & " Error!  Unexpected NAV Process Data Error", Source & " Could not process any integration requests; the actual error is:  ", AdminEmail, ex)
            Throw
        Finally
            Dim blnHadErrors = LogAllErrors()
            Me.closeLog(0)
            If blnHadErrors Then
                Throw New System.ApplicationException("Process Data Test Errors were found,  please check the error logs for details.")
            End If
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' The clsDefaultIntegrationConfiguration must have the following:
    ''' DBName
    ''' DBServer
    ''' TMSSettingsURI
    ''' TMSSettingsAuthCode
    ''' Typically db connections are managed by the web service but some features like
    ''' email and logging connect to the database directly in this case
    ''' if DBUser and DBPass are empty the system will use Integrated Security=SSPI 
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="c"></param>
    ''' <remarks>
    ''' FUTURE:  we may want to add logic to log exceptions to the DTMS log table and allow the process to 
    ''' skip partial failures and continue.
    ''' </remarks>
    Public Sub ProcessData(ByVal sSource As String, ByVal c As clsDefaultIntegrationConfiguration)
        If Not ConfigureInstance(sSource, c) Then Return
        Try
            LogFile = "C:\Data\TMSLogs\" & Source & ".log"

            If Me.Verbose Then Log("Begin Process Data ")
            'get a list of nav settings
            Dim oSettings As TMSIntegrationSettings.vERPIntegrationSetting()
            ' Begin Modified by RHR for v-8.4.0.003 on 10/19/2021
            Integer.TryParse(c.TMSAPRetryAttempts, Me.iTMSAPRetryAttempts)
            Integer.TryParse(c.TMSAPRetryMilliSeconds, Me.iTMSAPRetryMilliSeconds)
            Integer.TryParse(c.TMSPickRetryAttempts, Me.iTMSPickRetryAttempts)
            Integer.TryParse(c.TMSPickRetryMilliSeconds, Me.iTMSPickRetryMilliSeconds)
            ' End Modified by RHR for v-8.4.0.003 on 10/19/2021
#Disable Warning BC42030 ' Variable 'oSettings' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If Not getTMSIntegrationSettings(c, oSettings) Then Return
#Enable Warning BC42030 ' Variable 'oSettings' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If Not oSettings Is Nothing AndAlso oSettings.Count > 0 Then
                'group the settings by Legal Entity
                Dim sLegals As List(Of String) = oSettings.Select(Function(x) x.LegalEntity).Distinct().ToList()
                If Not sLegals Is Nothing AndAlso sLegals.Count() > 0 Then
                    For Each legal In sLegals
                        Dim lLegalSettings As TMSIntegrationSettings.vERPIntegrationSetting() = oSettings.Where(Function(x) x.LegalEntity = legal).ToArray()
                        If Not lLegalSettings Is Nothing AndAlso lLegalSettings.Count() > 0 Then
                            Me.LegalEntity = legal
                            Dim Processed As New List(Of Integer)
                            Dim Orders As New List(Of String)
                            Dim StandardSetting = getSpecificTMSSetting("Standard", lLegalSettings, Nothing)
                            'update the ERP Testing Status Flag using web services if available
                            'updateERPTestingStatus(getSpecificTMSSetting("ERPTestingStatus", lLegalSettings, StandardSetting))
                            'updateERPTestingStatusKing(getSpecificTMSSetting("ERPTestingStatus", lLegalSettings, StandardSetting))
                            processCompanyData(getSpecificTMSSetting("Company", lLegalSettings, StandardSetting), Processed)
                            'add code if needed to do something with Processed list
                            Processed = New List(Of Integer)
                            processCarrierData(getSpecificTMSSetting("Carrier", lLegalSettings, StandardSetting), Processed)
                            'add code if needed to do something with Processed list
                            Processed = New List(Of Integer)
                            processPalletTypeData(getSpecificTMSSetting("PalletType", lLegalSettings, StandardSetting))
                            processHazmatData(getSpecificTMSSetting("Hazmat", lLegalSettings, StandardSetting))
                            processLaneData(getSpecificTMSSetting("Lane", lLegalSettings, StandardSetting), Processed)
                            Processed = New List(Of Integer)
                            processOrderData(getSpecificTMSSetting("Order", lLegalSettings, StandardSetting), getSpecificTMSSetting("Lane", lLegalSettings, StandardSetting), Orders)
                            Orders = New List(Of String)
                            processPicklistData(getSpecificTMSSetting("PickList", lLegalSettings, StandardSetting))
                            processAPExportData(getSpecificTMSSetting("APExport", lLegalSettings, StandardSetting))
                            processPayablesData(getSpecificTMSSetting("Payables", lLegalSettings, StandardSetting))

                        End If
                    Next
                End If
            End If
            If Me.Verbose Then Log("Process Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex1 As System.ApplicationException
            LogError(Source & " Error!  Application Error in NAV Process Data", Source & " : " & ex1.Message, AdminEmail)
            Throw
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected NAV Process Data Error", Source & " Could not process any integration requests; the actual error is:  ", AdminEmail, ex)
            Throw
        Finally
            Dim blnHadErrors As Boolean = LogAllErrors()
            Me.closeLog(0)
            If blnHadErrors Then
                Throw New System.ApplicationException("Process Data Errors were found,  please check the error logs for details.")
            End If
        End Try
    End Sub

    Public Sub DebugOrderData(ByVal sSource As String, ByVal c As clsDefaultIntegrationConfiguration)
        If Not ConfigureInstance(sSource, c) Then Return
        Try
            Log("Begin Process Data ")
            'get a list of nav settings
            Dim oSettings As TMSIntegrationSettings.vERPIntegrationSetting()
#Disable Warning BC42030 ' Variable 'oSettings' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If Not getTMSIntegrationSettings(c, oSettings) Then Return
#Enable Warning BC42030 ' Variable 'oSettings' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If Not oSettings Is Nothing AndAlso oSettings.Count > 0 Then
                'group the settings by Legal Entity
                Dim sLegals As List(Of String) = oSettings.Select(Function(x) x.LegalEntity).Distinct().ToList()
                If Not sLegals Is Nothing AndAlso sLegals.Count() > 0 Then
                    For Each legal In sLegals
                        Dim lLegalSettings As TMSIntegrationSettings.vERPIntegrationSetting() = oSettings.Where(Function(x) x.LegalEntity = legal).ToArray()
                        If Not lLegalSettings Is Nothing AndAlso lLegalSettings.Count() > 0 Then
                            Me.LegalEntity = legal
                            Dim Processed As New List(Of Integer)
                            Dim Orders As New List(Of String)
                            Dim StandardSetting = getSpecificTMSSetting("Standard", lLegalSettings, Nothing)
                            'update the ERP Testing Status Flag using web services if available
                            'updateERPTestingStatus(getSpecificTMSSetting("ERPTestingStatus", lLegalSettings, StandardSetting))
                            processOrderDataDLLDirect(getSpecificTMSSetting("Order", lLegalSettings, StandardSetting), getSpecificTMSSetting("Lane", lLegalSettings, StandardSetting), Orders)
                            'processOrderData(getSpecificTMSSetting("Order", lLegalSettings, StandardSetting), getSpecificTMSSetting("Lane", lLegalSettings, StandardSetting), Orders)
                            Orders = New List(Of String)

                        End If
                    Next
                End If
            End If
            Log("Process Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        Finally
            Me.closeLog(0)
        End Try
    End Sub

    Public Sub fillConfig()
        Try

            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .ConnectionString = ConnectionString
                .Database = Me.Database
                .DBServer = Me.DBServer
                .Debug = Me.Debug
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .INIKey = Me.INIKey
                .KeepLogDays = Me.KeepLogDays
                .ResultsFile = Me.ResultsFile
                .LogFile = Me.LogFile
                .SaveOldLog = Me.SaveOldLog
                .SMTPServer = Me.SMTPServer
                .Source = Me.Source
            End With

        Catch ex As Exception
            Throw New ApplicationException(Source & " Fill Configuration Failure", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Returns the specific vERPIntegrationSetting in "a" filterd by IntegrationTypeName "n" or the Standard setting "s"
    ''' </summary>
    ''' <param name="n">IntegrationTypeName</param>
    ''' <param name="a">vERPIntegrationSetting array</param>
    ''' <param name="s">Standard vERPIntegrationSetting "Default"</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getSpecificTMSSetting(ByVal n As String, ByRef a As TMSIntegrationSettings.vERPIntegrationSetting(), ByRef s As TMSIntegrationSettings.vERPIntegrationSetting) As TMSIntegrationSettings.vERPIntegrationSetting
        If a Is Nothing OrElse a.Count() < 1 Then Return s
        If a.Any(Function(x) x.IntegrationTypeName = n) Then
            Return a.Where(Function(x) x.IntegrationTypeName = n).FirstOrDefault()
        Else
            Return s
        End If
    End Function

    Private Function getTMSIntegrationSettings(ByVal c As clsDefaultIntegrationConfiguration, ByRef oSettings As TMSIntegrationSettings.vERPIntegrationSetting()) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim oSettingObject As New TMSIntegrationSettings.DTMSIntegration()
            oSettingObject.Url = c.TMSSettingsURI
            Dim ReturnMessage As String
            Dim ERPTypeName As String = "NAV"
            If Not String.IsNullOrWhiteSpace(c.ERPTypeName) Then ERPTypeName = c.ERPTypeName
            Dim RetVal As Integer
#Disable Warning BC42030 ' Variable 'ReturnMessage' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            oSettings = oSettingObject.getvERPIntegrationSettingsByName(c.TMSSettingsAuthCode, c.TMSRunLegalEntity, ERPTypeName, RetVal, ReturnMessage)
#Enable Warning BC42030 ' Variable 'ReturnMessage' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            If RetVal <> TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not read Integration Settings information:  " & ReturnMessage)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        LogError("Read Integration Settings Error", "Error Data Validation Failure! could notread Integration Settings information:  " & ReturnMessage, AdminEmail)
                        Return False
                    Case Else
                        LogError("Read Integration Settings Error", "Error Integration Failure! could not read Integration Settings information:  " & ReturnMessage, AdminEmail)
                        Return False
                End Select
            Else
                blnRet = True
            End If

        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Read Integration Settings Error", Source & " Unexpected Read Integration Settings Error! Could not process any integration requests; the actual error is:  ", AdminEmail, ex)
            Throw
        End Try
        Return blnRet

    End Function

    'king.dynamics.businesscentral.api
    Private Sub updateERPTestingStatusKing(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting)
        'set the defalt to false
        ERPTestingOn = False
        If TMSSetting Is Nothing Then Return
        Try
            If Me.Verbose Then Log("Begin Read ERP Testing Flag ")
            Dim oNAVWebService = New king.dynamics.businesscentral.api.DynamicsTMSWebServices() ' NAVService.DynamicsTMSWebServices  NAVService.DynamicsTMSWebServices()
            'oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            ERPTestingOn = oNAVWebService.GetTestingStatus()
            If Me.Debug Then Log("Read ERP Testing Flag Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Read ERP Testing Flag Error", Source & " Unexpected Integration Error! Could not Read ERP Testing Flag information:  ", AdminEmail, ex)
            Throw
        End Try
    End Sub

    Private Sub updateERPTestingStatus(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting)
        'set the defalt to false
        ERPTestingOn = False
        If TMSSetting Is Nothing Then Return
        Try
            If Me.Verbose Then Log("Begin Read ERP Testing Flag ")
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)  '8wYWJmU6vBcsybjAiYfIL2Jo5ArKbZ3y+MsntMgMp08=
                'oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, "8wYWJmU6vBcsybjAiYfIL2Jo5ArKbZ3y+MsntMgMp08=")  '8wYWJmU6vBcsybjAiYfIL2Jo5ArKbZ3y+MsntMgMp08=
            End If

            ERPTestingOn = oNAVWebService.GetTestingStatus()
            If Me.Debug Then Log("Read ERP Testing Flag Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Read ERP Testing Flag Error", Source & " Unexpected Integration Error! Could not Read ERP Testing Flag information:  ", AdminEmail, ex)
            Throw
        End Try
    End Sub


    Private Function processCompanyDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Company; nothing to do returning false")
            Return False
        End If

        Try
            If Me.Debug Then Log("Begin Process Company Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCompIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCompIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCompIntegration.UseDefaultCredentials = True
            Else
                oCompIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCompHeaders As New List(Of TMSIntegrationServices.clsCompanyHeaderObject70)
            Dim oCompConts As New List(Of TMSIntegrationServices.clsCompanyContactObject70)
            Dim oCompCalendars As New List(Of TMSIntegrationServices.clsCompanyCalendarObject70)
            System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oBCIntegration = New BCIntegration.clsApplication()
            Dim oBCCompanies As Comp.Envelope = oBCIntegration.getCompanies(getAuthsSettngs(TMSSetting), ERPTestingOn, Not ERPTestingOn)

            If oBCCompanies Is Nothing OrElse oBCCompanies.Body Is Nothing OrElse oBCCompanies.Body.GetCompanies_Result Is Nothing OrElse oBCCompanies.Body.GetCompanies_Result.dynamicsTMSCompanies Is Nothing OrElse oBCCompanies.Body.GetCompanies_Result.dynamicsTMSCompanies.Length < 1 Then
                If Me.Debug Or Me.Verbose Then Log("Waiting on company Data")
                'Return True 'not ready yet so just return true
            End If
            For Each c In oBCCompanies.Body.GetCompanies_Result.dynamicsTMSCompanies
                If Not c Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(c.CompAlphaCode) Then
                        Dim strSkip As New List(Of String)
                        Dim oHeader = New TMSIntegrationServices.clsCompanyHeaderObject70
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oCompHeaders.Add(oHeader)
                        'Future: add code to read contact information if available
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Company record could not be processed because the record had an invalid Company Alpha Code value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oCompHeaders Is Nothing AndAlso oCompHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                Dim aCompHeaders As TMSIntegrationServices.clsCompanyHeaderObject70() = oCompHeaders.ToArray()
                Dim aCompConts As TMSIntegrationServices.clsCompanyContactObject70()
                If Not oCompConts Is Nothing AndAlso oCompConts.Count() > 0 Then aCompConts = oCompConts.ToArray()
                Dim aCompCalendars As TMSIntegrationServices.clsCompanyCalendarObject70()
                If Not oCompCalendars Is Nothing AndAlso oCompCalendars.Count() > 0 Then aCompCalendars = oCompCalendars.ToArray()
#Disable Warning BC42104 ' Variable 'aCompConts' is used before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42104 ' Variable 'aCompCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults = oCompIntegration.ProcessCompanyData70(TMSSetting.TMSAuthCode, aCompHeaders, aCompConts, aCompCalendars, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aCompCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42104 ' Variable 'aCompConts' is used before it has been assigned a value. A null reference exception could result at runtime.

                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import company information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if ERPTestingOn Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if ERPTestingOn Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following company control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'Future: add code to send confirmation back to NAV that the company data was processed  today we auto confirm above except when testing.
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Companies to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Company Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Company Integration Error", Source & " Unexpected Integration Error! Could not import Company information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function


    Private Function processCompanyData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Company; nothing to do returning false")
            Return False
        End If
        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processCompanyData2016(TMSSetting, Processed)
        End If

        If TMSSetting.ERPTypeControl = 4 Then
            Return processCompanyDataBC20(TMSSetting, Processed)
        End If

        Try
            If Me.Debug Then Log("Begin Process Company Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCompIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCompIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCompIntegration.UseDefaultCredentials = True
            Else
                oCompIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCompHeaders As New List(Of TMSIntegrationServices.clsCompanyHeaderObject70)
            Dim oCompConts As New List(Of TMSIntegrationServices.clsCompanyContactObject70)
            Dim oCompCalendars As New List(Of TMSIntegrationServices.clsCompanyCalendarObject70)
            Dim oNavCompany = New NAVService.Company
            Dim oNavCompanies As New NAVService.DynamicsTMSCompanies
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'Dim oNAVWebService = New DTMSBCWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            'oNAVWebService.sToken = "42048209843209"
            'oNAVWebService.UseDefaultCredentials = True
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            oNAVWebService.GetCompanies(oNavCompanies, ERPTestingOn, Not ERPTestingOn)
            If oNavCompanies Is Nothing OrElse oNavCompanies.Company Is Nothing OrElse oNavCompanies.Company.Count() < 1 Then
                If Me.Debug Or Me.Verbose Then Log("Waiting on company Data")
                'Return True 'not ready yet so just return true
            End If
            For Each c In oNavCompanies.Company
                If Not c Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(c.CompAlphaCode) Then
                        Dim strSkip As New List(Of String)
                        Dim oHeader = New TMSIntegrationServices.clsCompanyHeaderObject70
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oCompHeaders.Add(oHeader)
                        'Future: add code to read contact information if available
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Company record could not be processed because the record had an invalid Company Alpha Code value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oCompHeaders Is Nothing AndAlso oCompHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                Dim aCompHeaders As TMSIntegrationServices.clsCompanyHeaderObject70() = oCompHeaders.ToArray()
                Dim aCompConts As TMSIntegrationServices.clsCompanyContactObject70()
                If Not oCompConts Is Nothing AndAlso oCompConts.Count() > 0 Then aCompConts = oCompConts.ToArray()
                Dim aCompCalendars As TMSIntegrationServices.clsCompanyCalendarObject70()
                If Not oCompCalendars Is Nothing AndAlso oCompCalendars.Count() > 0 Then aCompCalendars = oCompCalendars.ToArray()
#Disable Warning BC42104 ' Variable 'aCompConts' is used before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42104 ' Variable 'aCompCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults = oCompIntegration.ProcessCompanyData70(TMSSetting.TMSAuthCode, aCompHeaders, aCompConts, aCompCalendars, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aCompCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42104 ' Variable 'aCompConts' is used before it has been assigned a value. A null reference exception could result at runtime.

                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import company information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if ERPTestingOn Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if ERPTestingOn Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following company control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'Future: add code to send confirmation back to NAV that the company data was processed  today we auto confirm above except when testing.
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Companies to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Company Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Company Integration Error", Source & " Unexpected Integration Error! Could not import Company information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processCompanyData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Company; nothing to do returning false")
            Return False
        End If
        Try
            If Me.Debug Then Log("Begin Process Company Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCompIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCompIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCompIntegration.UseDefaultCredentials = True
            Else
                oCompIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCompHeaders As New List(Of TMSIntegrationServices.clsCompanyHeaderObject70)
            Dim oCompConts As New List(Of TMSIntegrationServices.clsCompanyContactObject70)
            Dim oCompCalendars As New List(Of TMSIntegrationServices.clsCompanyCalendarObject70)
            Dim oNavCompany = New NAV2016Services.Company
            Dim oNavCompanies As New NAV2016Services.DynamicsTMSCompanies
            Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            oNAVWebService.GetCompanies(oNavCompanies, ERPTestingOn, Not ERPTestingOn)
            If oNavCompanies Is Nothing OrElse oNavCompanies.Company Is Nothing OrElse oNavCompanies.Company.Count() < 1 Then
                If Me.Debug Or Me.Verbose Then Log("Waiting on company Data")
                'Return True 'not ready yet so just return true
            End If
            For Each c In oNavCompanies.Company
                If Not c Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(c.CompAlphaCode) Then
                        Dim strSkip As New List(Of String)
                        Dim oHeader = New TMSIntegrationServices.clsCompanyHeaderObject70
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oCompHeaders.Add(oHeader)
                        'Future: add code to read contact information if available
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Company record could not be processed because the record had an invalid Company Alpha Code value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oCompHeaders Is Nothing AndAlso oCompHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                Dim aCompHeaders As TMSIntegrationServices.clsCompanyHeaderObject70() = oCompHeaders.ToArray()
                Dim aCompConts As TMSIntegrationServices.clsCompanyContactObject70()
                If Not oCompConts Is Nothing AndAlso oCompConts.Count() > 0 Then aCompConts = oCompConts.ToArray()
                Dim aCompCalendars As TMSIntegrationServices.clsCompanyCalendarObject70()
                If Not oCompCalendars Is Nothing AndAlso oCompCalendars.Count() > 0 Then aCompCalendars = oCompCalendars.ToArray()
#Disable Warning BC42104 ' Variable 'aCompConts' is used before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42104 ' Variable 'aCompCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults = oCompIntegration.ProcessCompanyData70(TMSSetting.TMSAuthCode, aCompHeaders, aCompConts, aCompCalendars, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aCompCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42104 ' Variable 'aCompConts' is used before it has been assigned a value. A null reference exception could result at runtime.

                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import company information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if ERPTestingOn Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Company, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if ERPTestingOn Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following company control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'Future: add code to send confirmation back to NAV that the company data was processed  today we auto confirm above except when testing.
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Companies to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Company Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Company Integration Error", Source & " Unexpected Integration Error! Could not import Company information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processCompanyUnitTest(ByRef Processed As List(Of Integer), ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        Try
            Log("Begin Process Company Data ")
            Dim strMsg As String = ""
            Dim oCompIntegration As New TMS.clsCompany
            populateIntegrationObjectParameters(oCompIntegration, UnitTestKeys)
            Dim oCompHeaders As New List(Of TMS.clsCompanyHeaderObject70)
            Dim oCompConts As New List(Of TMS.clsCompanyContactObject70)
            Dim oCompCalendars As New List(Of TMS.clsCompanyCalendarObject70)
            Dim oNavCompany = New NAVService.Company
            Dim oNavCompanies As New NAVService.DynamicsTMSCompanies
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                Log("Running unit test with sample data")
                oCompHeaders.Add(TMS.clsCompanyHeaderObject70.GenerateSampleObject(UnitTestKeys.CompName,
                                                                                   UnitTestKeys.CompNumber,
                                                                                   UnitTestKeys.CompAlphaCode,
                                                                                   UnitTestKeys.LegalEntity,
                                                                                   UnitTestKeys.CompAbrev))
                oCompConts.Add(TMS.clsCompanyContactObject70.GenerateSampleObject(UnitTestKeys.CompNumber, UnitTestKeys.CompAlphaCode, UnitTestKeys.LegalEntity))
            End If
            If Not oCompHeaders Is Nothing AndAlso oCompHeaders.Count > 0 Then
                'save changes to database 
                Dim oResults As TMS.clsIntegrationUpdateResults = oCompIntegration.ProcessObjectData70(oCompHeaders, oCompConts, Me.ConnectionString, oCompCalendars)
                Dim sLastError As String = oCompIntegration.LastError
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure

                        LogError("Error Data Connection Failure! could not import company information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Error Integration Failure! could not import company information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Company Integration Had Errors", Source & " Warning!  Could not import some Company information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Company information:  " & sLastError)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Company Integration Had Errors", Source & " Error Data Validation Failure! could not import Company information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some company information:  " & sLastError)
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        Log("Success! the following company control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers
                        'TODO: add code to send confirmation back to NAV that the company data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Companies to Process")
                blnRet = True
            End If
            Log("Process Company Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Company Integration Error", Source & " Unexpected Integration Error! Could not import Company information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function


    Private Function processLaneDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Lane; nothing to do returning false")
            Return False
        End If

        Try
            If Debug Then Log("Begin Process Lane Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oLaneIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oLaneIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oLaneIntegration.UseDefaultCredentials = True
            Else
                oLaneIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oLaneHeaders As New List(Of TMSIntegrationServices.clsLaneObject80)
            Dim oLaneCalendars As New List(Of TMSIntegrationServices.clsLaneCalendarObject80)
            System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oBCIntegration = New BCIntegration.clsApplication()
            Dim oLanes As Lane.Envelope = oBCIntegration.getLanes(getAuthsSettngs(TMSSetting), ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            strSkip.Add("LaneTransType")
            If (oLanes Is Nothing OrElse oLanes.Body Is Nothing OrElse oLanes.Body.GetLanes_Result Is Nothing OrElse oLanes.Body.GetLanes_Result.dynamicsTMSLanes Is Nothing OrElse oLanes.Body.GetLanes_Result.dynamicsTMSLanes.Length < 1) Then
                If Verbose Then Log("Waiting on Lane Data")
                Return True 'not ready yet so just return true
            End If

            For Each c In oLanes.Body.GetLanes_Result.dynamicsTMSLanes
                If Not c Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(c.LaneNumber) Then
                        'Modified by RHR v-8.2 09/18/2018
                        Dim oHeader = New TMSIntegrationServices.clsLaneObject80
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        ' Modified by RHR for v-7.0.6.105/8.0.1 on 06/28/2018
                        '  NAV web service uses integer for LaneTransType TMS web services uses Short for LaneTransType
                        '  may Not map with CopyMatchingFieldsImplicitCast
                        If c.LaneTransType <> 0 Then oHeader.LaneTransType = c.LaneTransType
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        If String.IsNullOrWhiteSpace(oHeader.LaneName) Then oHeader.LaneName = oHeader.LaneNumber
                        If Not String.IsNullOrWhiteSpace(oHeader.LaneOrigCompAlphaCode) AndAlso String.IsNullOrWhiteSpace(oHeader.LaneOrigLegalEntity) Then
                            oHeader.LaneOrigLegalEntity = LegalEntity
                        End If
                        If Not String.IsNullOrWhiteSpace(oHeader.LaneDestCompAlphaCode) AndAlso String.IsNullOrWhiteSpace(oHeader.LaneDestLegalEntity) Then
                            oHeader.LaneDestLegalEntity = LegalEntity
                        End If
                        oLaneHeaders.Add(oHeader)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Lane record could not be processed because the record had an iinvalid Lane Number value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oLaneHeaders Is Nothing AndAlso oLaneHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                'Modified by RHR v-8.2 09/18/2018
                Dim aLaneHeaders As TMSIntegrationServices.clsLaneObject80() = oLaneHeaders.ToArray()
                Dim aLaneCalendars As TMSIntegrationServices.clsLaneCalendarObject80()
                If Not oLaneCalendars Is Nothing AndAlso oLaneCalendars.Count() > 0 Then aLaneCalendars = oLaneCalendars.ToArray()
#Disable Warning BC42104 ' Variable 'aLaneCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults As TMSIntegrationServices.clsIntegrationUpdateResults = oLaneIntegration.ProcessLaneData80(TMSSetting.TMSAuthCode, aLaneHeaders, aLaneCalendars, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aLaneCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Lane information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following Lane control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'TODO: add code to send confirmation back to NAV that the lane data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Lanes to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Lane Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Lane Integration Error", Source & " Unexpected Integration Error! Could not import Lane information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="Processed"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105/8.0.1 on 06/28/2018
    '''   added logic for LaneTransType processing
    ''' Modified by RHR v-8.2 09/18/2018
    '''   added new field for NAV integration
    ''' </remarks>
    Private Function processLaneData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Lane; nothing to do returning false")
            Return False
        End If
        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processLaneData2016(TMSSetting, Processed)
        End If

        If TMSSetting.ERPTypeControl = 4 Then
            Return processLaneDataBC20(TMSSetting, Processed)
        End If

        Try
            If Debug Then Log("Begin Process Lane Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oLaneIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oLaneIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oLaneIntegration.UseDefaultCredentials = True
            Else
                oLaneIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            'Modified by RHR v-8.2 09/18/2018
            Dim oLaneHeaders As New List(Of TMSIntegrationServices.clsLaneObject80)
            Dim oLaneCalendars As New List(Of TMSIntegrationServices.clsLaneCalendarObject80)
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavLanes = New NAVService.DynamicsTMSLanes()
            oNAVWebService.GetLanes(oNavLanes, ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            ' Modified by RHR for v-7.0.6.105/8.0.1 on 06/28/2018
            '  NAV web service uses integer TMS web services uses Short may not map with CopyMatchingFieldsImplicitCast
            strSkip.Add("LaneTransType")
            If oNavLanes Is Nothing OrElse oNavLanes.Lane Is Nothing OrElse oNavLanes.Lane.Count() < 1 Then
                If Verbose Then Log("Waiting on Lane Data")
                Return True 'not ready yet so just return true
            End If
            For Each c In oNavLanes.Lane
                If Not c Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(c.LaneNumber) Then
                        'Modified by RHR v-8.2 09/18/2018
                        Dim oHeader = New TMSIntegrationServices.clsLaneObject80
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        ' Modified by RHR for v-7.0.6.105/8.0.1 on 06/28/2018
                        '  NAV web service uses integer for LaneTransType TMS web services uses Short for LaneTransType
                        '  may Not map with CopyMatchingFieldsImplicitCast
                        If c.LaneTransType <> 0 Then oHeader.LaneTransType = c.LaneTransType
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        If String.IsNullOrWhiteSpace(oHeader.LaneName) Then oHeader.LaneName = oHeader.LaneNumber
                        If Not String.IsNullOrWhiteSpace(oHeader.LaneOrigCompAlphaCode) AndAlso String.IsNullOrWhiteSpace(oHeader.LaneOrigLegalEntity) Then
                            oHeader.LaneOrigLegalEntity = LegalEntity
                        End If
                        If Not String.IsNullOrWhiteSpace(oHeader.LaneDestCompAlphaCode) AndAlso String.IsNullOrWhiteSpace(oHeader.LaneDestLegalEntity) Then
                            oHeader.LaneDestLegalEntity = LegalEntity
                        End If
                        oLaneHeaders.Add(oHeader)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Lane record could not be processed because the record had an iinvalid Lane Number value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oLaneHeaders Is Nothing AndAlso oLaneHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                'Modified by RHR v-8.2 09/18/2018
                Dim aLaneHeaders As TMSIntegrationServices.clsLaneObject80() = oLaneHeaders.ToArray()
                Dim aLaneCalendars As TMSIntegrationServices.clsLaneCalendarObject80()
                If Not oLaneCalendars Is Nothing AndAlso oLaneCalendars.Count() > 0 Then aLaneCalendars = oLaneCalendars.ToArray()
#Disable Warning BC42104 ' Variable 'aLaneCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults As TMSIntegrationServices.clsIntegrationUpdateResults = oLaneIntegration.ProcessLaneData80(TMSSetting.TMSAuthCode, aLaneHeaders, aLaneCalendars, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aLaneCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Lane information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following Lane control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'TODO: add code to send confirmation back to NAV that the lane data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Lanes to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Lane Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Lane Integration Error", Source & " Unexpected Integration Error! Could not import Lane information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processLaneData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Lane; nothing to do returning false")
            Return False
        End If
        Try
            If Debug Then Log("Begin Process Lane Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oLaneIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oLaneIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oLaneIntegration.UseDefaultCredentials = True
            Else
                oLaneIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            'Modified by RHR v-8.2 09/18/2018
            Dim oLaneHeaders As New List(Of TMSIntegrationServices.clsLaneObject80)
            Dim oLaneCalendars As New List(Of TMSIntegrationServices.clsLaneCalendarObject80)
            Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavLanes = New NAV2016Services.DynamicsTMSLanes()
            oNAVWebService.GetLanes(oNavLanes, ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            ' Modified by RHR for v-7.0.6.105/8.0.1 on 06/28/2018
            '  NAV web service uses integer TMS web services uses Short may not map with CopyMatchingFieldsImplicitCast
            strSkip.Add("LaneTransType")
            If oNavLanes Is Nothing OrElse oNavLanes.Lane Is Nothing OrElse oNavLanes.Lane.Count() < 1 Then
                If Verbose Then Log("Waiting on Lane Data")
                Return True 'not ready yet so just return true
            End If
            For Each c In oNavLanes.Lane
                If Not c Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(c.LaneNumber) Then
                        'Modified by RHR v-8.2 09/18/2018
                        Dim oHeader = New TMSIntegrationServices.clsLaneObject80
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        ' Modified by RHR for v-7.0.6.105/8.0.1 on 06/28/2018
                        '  NAV web service uses integer for LaneTransType TMS web services uses Short for LaneTransType
                        '  may Not map with CopyMatchingFieldsImplicitCast
                        If c.LaneTransType <> 0 Then oHeader.LaneTransType = c.LaneTransType
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        If String.IsNullOrWhiteSpace(oHeader.LaneName) Then oHeader.LaneName = oHeader.LaneNumber
                        If Not String.IsNullOrWhiteSpace(oHeader.LaneOrigCompAlphaCode) AndAlso String.IsNullOrWhiteSpace(oHeader.LaneOrigLegalEntity) Then
                            oHeader.LaneOrigLegalEntity = LegalEntity
                        End If
                        If Not String.IsNullOrWhiteSpace(oHeader.LaneDestCompAlphaCode) AndAlso String.IsNullOrWhiteSpace(oHeader.LaneDestLegalEntity) Then
                            oHeader.LaneDestLegalEntity = LegalEntity
                        End If
                        oLaneHeaders.Add(oHeader)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Lane record could not be processed because the record had an iinvalid Lane Number value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oLaneHeaders Is Nothing AndAlso oLaneHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                'Modified by RHR v-8.2 09/18/2018
                Dim aLaneHeaders As TMSIntegrationServices.clsLaneObject80() = oLaneHeaders.ToArray()
                Dim aLaneCalendars As TMSIntegrationServices.clsLaneCalendarObject80()
                If Not oLaneCalendars Is Nothing AndAlso oLaneCalendars.Count() > 0 Then aLaneCalendars = oLaneCalendars.ToArray()
#Disable Warning BC42104 ' Variable 'aLaneCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults As TMSIntegrationServices.clsIntegrationUpdateResults = oLaneIntegration.ProcessLaneData80(TMSSetting.TMSAuthCode, aLaneHeaders, aLaneCalendars, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aLaneCalendars' is used before it has been assigned a value. A null reference exception could result at runtime.
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Lane information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Lane, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following Lane control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'TODO: add code to send confirmation back to NAV that the lane data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Lanes to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Lane Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Lane Integration Error", Source & " Unexpected Integration Error! Could not import Lane information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processLaneUnitTest(ByRef Processed As List(Of Integer), ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        Try

            Log("Begin Process Lane Data ")
            Dim strMsg As String = ""
            Dim oLaneIntegration As New TMS.clsLane
            populateIntegrationObjectParameters(oLaneIntegration, UnitTestKeys)
            Dim oLaneHeaders As New List(Of TMS.clsLaneObject70)
            Dim oLaneCalendars As New List(Of TMS.clsLaneCalendarObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                Log("Running unit test with sample data")
                oLaneHeaders.Add(TMS.clsLaneObject70.GenerateSampleObject(UnitTestKeys.LaneName, UnitTestKeys.LaneNumber, UnitTestKeys.CompNumber, UnitTestKeys.CompAlphaCode, UnitTestKeys.LegalEntity))
            End If

            If Not oLaneHeaders Is Nothing AndAlso oLaneHeaders.Count > 0 Then
                'save changes to database 
                Dim oResults As TMS.clsIntegrationUpdateResults = oLaneIntegration.ProcessObjectData70(oLaneHeaders, Me.ConnectionString, oLaneCalendars)
                Dim sLastError As String = oLaneIntegration.LastError
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Lane information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Error Integration Failure! could not import Lane information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Lane Integration Had Errors", Source & " Warning!  Could not import some Lane information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Lane information:  " & sLastError)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Lane Integration Had Errors", Source & " Error Data Validation Failure! could not import Lane information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Lane information:  " & sLastError)
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        Log("Success! the following Lane control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers
                        'TODO: add code to send confirmation back to NAV that the lane data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Lanes to Process")
                blnRet = True
            End If
            Log("Process Lane Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Lane Integration Error", Source & " Unexpected Integration Error! Could not import Lane information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processCarrierDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Carrier; nothing to do returning false")
            Return False
        End If
        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processCarrierData2016(TMSSetting, Processed)
        End If
        Try
            If Debug Then Log("Begin Process Carrier Data 8.2 ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCarrierIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCarrierIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCarrierIntegration.UseDefaultCredentials = True
            Else
                oCarrierIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCarrierHeaders As New List(Of TMSIntegrationServices.clsCarrierHeaderObject70)
            Dim oCarrierConts As New List(Of TMSIntegrationServices.clsCarrierContactObject70)
            System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oBCIntegration = New BCIntegration.clsApplication()
            Dim oBCCarriers As Carr.Envelope = oBCIntegration.getCarriers(getAuthsSettngs(TMSSetting), ERPTestingOn, Not ERPTestingOn)
            If (oBCCarriers Is Nothing OrElse oBCCarriers.Body Is Nothing OrElse oBCCarriers.Body.GetCarriers_Result Is Nothing OrElse oBCCarriers.Body.GetCarriers_Result.dynamicsTMSCarriers Is Nothing OrElse oBCCarriers.Body.GetCarriers_Result.dynamicsTMSCarriers.Length < 1) Then
                If Verbose Then Log("Waiting on Carrier Data")
                Return True 'not ready yet so just return true
            End If

            For Each c In oBCCarriers.Body.GetCarriers_Result.dynamicsTMSCarriers
                If Not c Is Nothing Then
                    If Debug Then
                        Dim strData As String = String.Format(" CarrierAlphaCode: {0}, CarrierCurrencyType: {1}, CarrierEmail: {2}, CarrierLegalEntity: {3}, CarrierMailAddress1: {4}, CarrierMailAddress2: {5}, CarrierMailAddress3: {6}, CarrierMailCity: {7}, CarrierMailCountry: {8}, CarrierMailState: {9}, CarrierMailZip: {10}, CarrierName: {11}, CarrierNumber: {12}, CarrierQualAuthority: {13}, CarrierQualContract: {14}, CarrierQualContractExpiresDate: {15}, CarrierQualInsturanceDate: {16}, CarrierQualQualified: {17}, CarrierQualSignedDate: {18}, CarrierSCAC: {19}, CarrierStreetAddress1: {20}, CarrierStreetAddress2: {21}, CarrierStreetAddress3: {22}, CarrierStreetCity: {23}, CarrierStreetCountry: {24}, CarrierStreetState: {25}, CarrierStreetZip: {26}, CarrierTypeCode: {27}, CarrierWebsite", c.CarrierAlphaCode, c.CarrierCurrencyType, c.CarrierEmail, c.CarrierLegalEntity, c.CarrierMailAddress1, c.CarrierMailAddress2, c.CarrierMailAddress3, c.CarrierMailCity, c.CarrierMailCountry, c.CarrierMailState, c.CarrierMailZip, c.CarrierName, c.CarrierNumber, c.CarrierQualAuthority, c.CarrierQualContract, c.CarrierQualContractExpiresDate, c.CarrierQualInsturanceDate, c.CarrierQualQualified, c.CarrierQualSignedDate, c.CarrierSCAC, c.CarrierStreetAddress1, c.CarrierStreetAddress2, c.CarrierStreetAddress3, c.CarrierStreetCity, c.CarrierStreetCountry, c.CarrierStreetState, c.CarrierStreetZip, c.CarrierTypeCode, c.CarrierWebsite)
                        Log(strData)
                    End If
                    If Not String.IsNullOrWhiteSpace(c.CarrierAlphaCode) Then

                        Dim oHeader = New TMSIntegrationServices.clsCarrierHeaderObject70
                        Dim strSkip As New List(Of String)
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        If Debug Then
                            Dim h = oHeader
                            Dim strData As String = String.Format(" CarrierAlphaCode: {0}, CarrierCurrencyType: {1}, CarrierEmail: {2}, CarrierLegalEntity: {3}, CarrierMailAddress1: {4}, CarrierMailAddress2: {5}, CarrierMailAddress3: {6}, CarrierMailCity: {7}, CarrierMailCountry: {8}, CarrierMailState: {9}, CarrierMailZip: {10}, CarrierName: {11}, CarrierNumber: {12}, CarrierQualAuthority: {13}, CarrierQualContract: {14}, CarrierQualContractExpiresDate: {15}, CarrierQualInsturanceDate: {16}, CarrierQualQualified: {17}, CarrierQualSignedDate: {18}, CarrierSCAC: {19}, CarrierStreetAddress1: {20}, CarrierStreetAddress2: {21}, CarrierStreetAddress3: {22}, CarrierStreetCity: {23}, CarrierStreetCountry: {24}, CarrierStreetState: {25}, CarrierStreetZip: {26}, CarrierTypeCode: {27}, CarrierWebsite", h.CarrierAlphaCode, h.CarrierCurrencyType, h.CarrierEmail, h.CarrierLegalEntity, h.CarrierMailAddress1, h.CarrierMailAddress2, h.CarrierMailAddress3, h.CarrierMailCity, h.CarrierMailCountry, h.CarrierMailState, h.CarrierMailZip, h.CarrierName, h.CarrierNumber, h.CarrierQualAuthority, h.CarrierQualContract, h.CarrierQualContractExpiresDate, h.CarrierQualInsuranceDate, h.CarrierQualQualified, h.CarrierQualSignedDate, h.CarrierSCAC, h.CarrierStreetAddress1, h.CarrierStreetAddress2, h.CarrierStreetAddress3, h.CarrierStreetCity, h.CarrierStreetCountry, h.CarrierStreetState, h.CarrierStreetZip, h.CarrierTypeCode, h.CarrierWebSite)
                            Log(strData)
                        End If
                        oCarrierHeaders.Add(oHeader)
                        'Future: Add code to copy NAV data into Contact objects
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Carrier record could not be processed because the record had an invalid Carrier Alpha Code value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oCarrierHeaders Is Nothing AndAlso oCarrierHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                Dim aCarrierHeaders As TMSIntegrationServices.clsCarrierHeaderObject70() = oCarrierHeaders.ToArray()
                Dim aCarrierConts As TMSIntegrationServices.clsCarrierContactObject70()
                If Not oCarrierConts Is Nothing AndAlso oCarrierConts.Count() > 0 Then aCarrierConts = oCarrierConts.ToArray()
#Disable Warning BC42104 ' Variable 'aCarrierConts' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults As TMSIntegrationServices.clsIntegrationUpdateResults = oCarrierIntegration.ProcessCarrierData70(TMSSetting.TMSAuthCode, aCarrierHeaders, aCarrierConts, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aCarrierConts' is used before it has been assigned a value. A null reference exception could result at runtime.

                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Carrier information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Carriers to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Carrier Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Carrier Integration Error", Source & " Unexpected Integration Error! Could not import Carrier information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processCarrierData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Carrier; nothing to do returning false")
            Return False
        End If
        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processCarrierData2016(TMSSetting, Processed)
        End If

        If TMSSetting.ERPTypeControl = 4 Then
            Return processCarrierDataBC20(TMSSetting, Processed)
        End If

        Try
            If Debug Then Log("Begin Process Carrier Data 8.2 ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCarrierIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCarrierIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCarrierIntegration.UseDefaultCredentials = True
            Else
                oCarrierIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCarrierHeaders As New List(Of TMSIntegrationServices.clsCarrierHeaderObject70)
            Dim oCarrierConts As New List(Of TMSIntegrationServices.clsCarrierContactObject70)
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavCarriers = New NAVService.DynamicsTMSCarriers()
            oNAVWebService.GetCarriers(oNavCarriers, ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            If oNavCarriers Is Nothing OrElse oNavCarriers.Carrier Is Nothing OrElse oNavCarriers.Carrier.Count() < 1 Then
                If Verbose Then Log("Waiting on Carrier Data")
                Return True 'not ready yet so just return true
            End If
            For Each c In oNavCarriers.Carrier
                If Not c Is Nothing Then
                    If Debug Then
                        Dim strData As String = String.Format(" CarrierAlphaCode: {0}, CarrierCurrencyType: {1}, CarrierEmail: {2}, CarrierLegalEntity: {3}, CarrierMailAddress1: {4}, CarrierMailAddress2: {5}, CarrierMailAddress3: {6}, CarrierMailCity: {7}, CarrierMailCountry: {8}, CarrierMailState: {9}, CarrierMailZip: {10}, CarrierName: {11}, CarrierNumber: {12}, CarrierQualAuthority: {13}, CarrierQualContract: {14}, CarrierQualContractExpiresDate: {15}, CarrierQualInsturanceDate: {16}, CarrierQualQualified: {17}, CarrierQualSignedDate: {18}, CarrierSCAC: {19}, CarrierStreetAddress1: {20}, CarrierStreetAddress2: {21}, CarrierStreetAddress3: {22}, CarrierStreetCity: {23}, CarrierStreetCountry: {24}, CarrierStreetState: {25}, CarrierStreetZip: {26}, CarrierTypeCode: {27}, CarrierWebsite", c.CarrierAlphaCode, c.CarrierCurrencyType, c.CarrierEmail, c.CarrierLegalEntity, c.CarrierMailAddress1, c.CarrierMailAddress2, c.CarrierMailAddress3, c.CarrierMailCity, c.CarrierMailCountry, c.CarrierMailState, c.CarrierMailZip, c.CarrierName, c.CarrierNumber, c.CarrierQualAuthority, c.CarrierQualContract, c.CarrierQualContractExpiresDate, c.CarrierQualInsturanceDate, c.CarrierQualQualified, c.CarrierQualSignedDate, c.CarrierSCAC, c.CarrierStreetAddress1, c.CarrierStreetAddress2, c.CarrierStreetAddress3, c.CarrierStreetCity, c.CarrierStreetCountry, c.CarrierStreetState, c.CarrierStreetZip, c.CarrierTypeCode, c.CarrierWebsite)
                        Log(strData)
                    End If
                    If Not String.IsNullOrWhiteSpace(c.CarrierAlphaCode) Then


                        Dim oHeader = New TMSIntegrationServices.clsCarrierHeaderObject70
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        If Debug Then
                            Dim h = oHeader
                            Dim strData As String = String.Format(" CarrierAlphaCode: {0}, CarrierCurrencyType: {1}, CarrierEmail: {2}, CarrierLegalEntity: {3}, CarrierMailAddress1: {4}, CarrierMailAddress2: {5}, CarrierMailAddress3: {6}, CarrierMailCity: {7}, CarrierMailCountry: {8}, CarrierMailState: {9}, CarrierMailZip: {10}, CarrierName: {11}, CarrierNumber: {12}, CarrierQualAuthority: {13}, CarrierQualContract: {14}, CarrierQualContractExpiresDate: {15}, CarrierQualInsturanceDate: {16}, CarrierQualQualified: {17}, CarrierQualSignedDate: {18}, CarrierSCAC: {19}, CarrierStreetAddress1: {20}, CarrierStreetAddress2: {21}, CarrierStreetAddress3: {22}, CarrierStreetCity: {23}, CarrierStreetCountry: {24}, CarrierStreetState: {25}, CarrierStreetZip: {26}, CarrierTypeCode: {27}, CarrierWebsite", h.CarrierAlphaCode, h.CarrierCurrencyType, h.CarrierEmail, h.CarrierLegalEntity, h.CarrierMailAddress1, h.CarrierMailAddress2, h.CarrierMailAddress3, h.CarrierMailCity, h.CarrierMailCountry, h.CarrierMailState, h.CarrierMailZip, h.CarrierName, h.CarrierNumber, h.CarrierQualAuthority, h.CarrierQualContract, h.CarrierQualContractExpiresDate, h.CarrierQualInsuranceDate, h.CarrierQualQualified, h.CarrierQualSignedDate, h.CarrierSCAC, h.CarrierStreetAddress1, h.CarrierStreetAddress2, h.CarrierStreetAddress3, h.CarrierStreetCity, h.CarrierStreetCountry, h.CarrierStreetState, h.CarrierStreetZip, h.CarrierTypeCode, h.CarrierWebSite)
                            Log(strData)
                        End If
                        oCarrierHeaders.Add(oHeader)
                        'Future: Add code to copy NAV data into Contact objects
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Carrier record could not be processed because the record had an invalid Carrier Alpha Code value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oCarrierHeaders Is Nothing AndAlso oCarrierHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                Dim aCarrierHeaders As TMSIntegrationServices.clsCarrierHeaderObject70() = oCarrierHeaders.ToArray()
                Dim aCarrierConts As TMSIntegrationServices.clsCarrierContactObject70()
                If Not oCarrierConts Is Nothing AndAlso oCarrierConts.Count() > 0 Then aCarrierConts = oCarrierConts.ToArray()
#Disable Warning BC42104 ' Variable 'aCarrierConts' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults As TMSIntegrationServices.clsIntegrationUpdateResults = oCarrierIntegration.ProcessCarrierData70(TMSSetting.TMSAuthCode, aCarrierHeaders, aCarrierConts, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aCarrierConts' is used before it has been assigned a value. A null reference exception could result at runtime.

                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Carrier information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Carriers to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Carrier Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Carrier Integration Error", Source & " Unexpected Integration Error! Could not import Carrier information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processCarrierData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Processed As List(Of Integer) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Carrier; nothing to do returning false")
            Return False
        End If
        Try
            If Debug Then Log("Begin Process Carrier Data 8.2 ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oCarrierIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oCarrierIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oCarrierIntegration.UseDefaultCredentials = True
            Else
                oCarrierIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oCarrierHeaders As New List(Of TMSIntegrationServices.clsCarrierHeaderObject70)
            Dim oCarrierConts As New List(Of TMSIntegrationServices.clsCarrierContactObject70)
            Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavCarriers = New NAV2016Services.DynamicsTMSCarriers()
            oNAVWebService.GetCarriers(oNavCarriers, ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            If oNavCarriers Is Nothing OrElse oNavCarriers.Carrier Is Nothing OrElse oNavCarriers.Carrier.Count() < 1 Then
                If Verbose Then Log("Waiting on Carrier Data")
                Return True 'not ready yet so just return true
            End If
            For Each c In oNavCarriers.Carrier
                If Not c Is Nothing Then
                    If Debug Then
                        Dim strData As String = String.Format(" CarrierAlphaCode: {0}, CarrierCurrencyType: {1}, CarrierEmail: {2}, CarrierLegalEntity: {3}, CarrierMailAddress1: {4}, CarrierMailAddress2: {5}, CarrierMailAddress3: {6}, CarrierMailCity: {7}, CarrierMailCountry: {8}, CarrierMailState: {9}, CarrierMailZip: {10}, CarrierName: {11}, CarrierNumber: {12}, CarrierQualAuthority: {13}, CarrierQualContract: {14}, CarrierQualContractExpiresDate: {15}, CarrierQualInsturanceDate: {16}, CarrierQualQualified: {17}, CarrierQualSignedDate: {18}, CarrierSCAC: {19}, CarrierStreetAddress1: {20}, CarrierStreetAddress2: {21}, CarrierStreetAddress3: {22}, CarrierStreetCity: {23}, CarrierStreetCountry: {24}, CarrierStreetState: {25}, CarrierStreetZip: {26}, CarrierTypeCode: {27}, CarrierWebsite", c.CarrierAlphaCode, c.CarrierCurrencyType, c.CarrierEmail, c.CarrierLegalEntity, c.CarrierMailAddress1, c.CarrierMailAddress2, c.CarrierMailAddress3, c.CarrierMailCity, c.CarrierMailCountry, c.CarrierMailState, c.CarrierMailZip, c.CarrierName, c.CarrierNumber, c.CarrierQualAuthority, c.CarrierQualContract, c.CarrierQualContractExpiresDate, c.CarrierQualInsturanceDate, c.CarrierQualQualified, c.CarrierQualSignedDate, c.CarrierSCAC, c.CarrierStreetAddress1, c.CarrierStreetAddress2, c.CarrierStreetAddress3, c.CarrierStreetCity, c.CarrierStreetCountry, c.CarrierStreetState, c.CarrierStreetZip, c.CarrierTypeCode, c.CarrierWebsite)
                        Log(strData)
                    End If
                    If Not String.IsNullOrWhiteSpace(c.CarrierAlphaCode) Then


                        Dim oHeader = New TMSIntegrationServices.clsCarrierHeaderObject70
                        CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        If Debug Then
                            Dim h = oHeader
                            Dim strData As String = String.Format(" CarrierAlphaCode: {0}, CarrierCurrencyType: {1}, CarrierEmail: {2}, CarrierLegalEntity: {3}, CarrierMailAddress1: {4}, CarrierMailAddress2: {5}, CarrierMailAddress3: {6}, CarrierMailCity: {7}, CarrierMailCountry: {8}, CarrierMailState: {9}, CarrierMailZip: {10}, CarrierName: {11}, CarrierNumber: {12}, CarrierQualAuthority: {13}, CarrierQualContract: {14}, CarrierQualContractExpiresDate: {15}, CarrierQualInsturanceDate: {16}, CarrierQualQualified: {17}, CarrierQualSignedDate: {18}, CarrierSCAC: {19}, CarrierStreetAddress1: {20}, CarrierStreetAddress2: {21}, CarrierStreetAddress3: {22}, CarrierStreetCity: {23}, CarrierStreetCountry: {24}, CarrierStreetState: {25}, CarrierStreetZip: {26}, CarrierTypeCode: {27}, CarrierWebsite", h.CarrierAlphaCode, h.CarrierCurrencyType, h.CarrierEmail, h.CarrierLegalEntity, h.CarrierMailAddress1, h.CarrierMailAddress2, h.CarrierMailAddress3, h.CarrierMailCity, h.CarrierMailCountry, h.CarrierMailState, h.CarrierMailZip, h.CarrierName, h.CarrierNumber, h.CarrierQualAuthority, h.CarrierQualContract, h.CarrierQualContractExpiresDate, h.CarrierQualInsuranceDate, h.CarrierQualQualified, h.CarrierQualSignedDate, h.CarrierSCAC, h.CarrierStreetAddress1, h.CarrierStreetAddress2, h.CarrierStreetAddress3, h.CarrierStreetCity, h.CarrierStreetCountry, h.CarrierStreetState, h.CarrierStreetZip, h.CarrierTypeCode, h.CarrierWebSite)
                            Log(strData)
                        End If
                        oCarrierHeaders.Add(oHeader)
                        'Future: Add code to copy NAV data into Contact objects
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Carrier record could not be processed because the record had an invalid Carrier Alpha Code value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            If Not oCarrierHeaders Is Nothing AndAlso oCarrierHeaders.Count > 0 Then
                'convert lists to arrays and save changes to database using web service 
                Dim aCarrierHeaders As TMSIntegrationServices.clsCarrierHeaderObject70() = oCarrierHeaders.ToArray()
                Dim aCarrierConts As TMSIntegrationServices.clsCarrierContactObject70()
                If Not oCarrierConts Is Nothing AndAlso oCarrierConts.Count() > 0 Then aCarrierConts = oCarrierConts.ToArray()
#Disable Warning BC42104 ' Variable 'aCarrierConts' is used before it has been assigned a value. A null reference exception could result at runtime.
                Dim oResults As TMSIntegrationServices.clsIntegrationUpdateResults = oCarrierIntegration.ProcessCarrierData70(TMSSetting.TMSAuthCode, aCarrierHeaders, aCarrierConts, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aCarrierConts' is used before it has been assigned a value. A null reference exception could result at runtime.

                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Carrier information:  " & ReturnMessage)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults.ReturnValue, IntegrationModule.Carrier, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        If Me.Verbose Then Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers.ToList()
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Carriers to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Carrier Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Carrier Integration Error", Source & " Unexpected Integration Error! Could not import Carrier information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processCarrierUnitTest(ByRef Processed As List(Of Integer), ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = False
        If Processed Is Nothing Then Processed = New List(Of Integer)
        Try
            Log("Begin Process Carrier Data ")
            Dim strMsg As String = ""
            Dim oCarrierIntegration As New TMS.clsCarrier
            populateIntegrationObjectParameters(oCarrierIntegration, UnitTestKeys)
            Dim oCarrierHeaders As New List(Of TMS.clsCarrierHeaderObject70)
            Dim oCarrierConts As New List(Of TMS.clsCarrierContactObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                Log("Running unit test with sample data")
                oCarrierHeaders.Add(TMS.clsCarrierHeaderObject70.GenerateSampleObject(UnitTestKeys.CarrierName, UnitTestKeys.CarrierNumber, UnitTestKeys.CarrierAlphaCode, UnitTestKeys.LegalEntity))
                oCarrierConts.Add(TMS.clsCarrierContactObject70.GenerateSampleObject(UnitTestKeys.CarrierNumber, UnitTestKeys.CarrierAlphaCode, UnitTestKeys.LegalEntity))
            End If

            'save changes to database 
            If Not oCarrierHeaders Is Nothing AndAlso oCarrierHeaders.Count > 0 Then
                Dim oResults As TMS.clsIntegrationUpdateResults = oCarrierIntegration.ProcessObjectData70(oCarrierHeaders, oCarrierConts, Me.ConnectionString)
                Dim sLastError As String = oCarrierIntegration.LastError
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Carrier information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Error Integration Failure! could not import Carrier information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Carrier Integration Had Errors", Source & " Warning!  Could not import some Carrier information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Carrier information:  " & sLastError)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Carrier Integration Had Errors", Source & " Error Data Validation Failure! could not import Carrier information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Carrier information:  " & sLastError)
                        End If
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        Processed = oResults.ControlNumbers
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Carriers to Process")
                blnRet = True
            End If

            Log("Process Carrier Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Carrier Integration Error", Source & " Unexpected Integration Error! Could not import Carrier information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processOrderDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, ByVal TMSLaneSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Orders As List(Of String) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Orders Is Nothing Then Orders = New List(Of String)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Order; nothing to do returning false")
            Return False
        End If

        Try
            If Debug Then Log("Begin Process Order Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oBookIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oBookIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oBookIntegration.UseDefaultCredentials = True
            Else
                oBookIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oBookHeaders As New List(Of TMSIntegrationServices.clsBookHeaderObject80)
            Dim oBookDetails As New List(Of TMSIntegrationServices.clsBookDetailObject80)
            System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oBCIntegration = New BCIntegration.clsApplication()
            Dim oBookings As Book.Envelope = oBCIntegration.getBooks(getAuthsSettngs(TMSSetting), ERPTestingOn, Not ERPTestingOn)

            Dim blnSecondLoop As Boolean = False
            Dim sIntegrationErrors As New List(Of String)
            Dim sAdminErrors As New List(Of String)
            Dim sOperationErrors As New List(Of String)
            Dim sLogMsgs As New List(Of String)
            'we limit the reads to 100 in case of an error
            'we are not using the read loop at this time
            'in the future we need to close and reopen the web service object.
            'For i As Integer = 0 To 20
            Dim strSkip As New List(Of String)
            Dim strItemSkip As New List(Of String)
            strItemSkip.Add("ItemPONumber")
            If (oBookings Is Nothing OrElse oBookings.Body Is Nothing OrElse oBookings.Body.GetBookings_Result Is Nothing OrElse oBookings.Body.GetBookings_Result.dynamicsTMSBookings Is Nothing OrElse oBookings.Body.GetBookings_Result.dynamicsTMSBookings.Length < 1) Then
                If Verbose Then
                    sLogMsgs.Add("Waiting on Order Data")
                End If
            Else
                For Each c In oBookings.Body.GetBookings_Result.dynamicsTMSBookings
                    If Not c Is Nothing Then
                        If Not String.IsNullOrWhiteSpace(c.PONumber) Then
                            Orders.Add(c.PONumber)
                            Dim oHeader = New TMSIntegrationServices.clsBookHeaderObject80
                            CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            If oHeader.POModeTypeControl = 0 Then
                                oHeader.POModeTypeControl = 3 'use default as Road
                            End If
                            'Modified by RHR v-7.0.5.100 07/21/2016
                            Dim sFK As String = c.ChangeNo

                            oBookHeaders.Add(oHeader)
                            For Each item In c.Items
                                If Not item Is Nothing Then
                                    If Not String.IsNullOrWhiteSpace(item.ItemNumber) Then
                                        Dim oItem As New TMSIntegrationServices.clsBookDetailObject80
                                        CopyMatchingFieldsImplicitCast(oItem, item, strItemSkip, strMsg)
                                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                                            If Debug Then Log(strMsg)
                                            strMsg = ""
                                        End If
                                        'Modified by RHR v-7.0.5.100 07/21/2016
                                        oItem.ChangeNo = sFK
                                        If (Not item.ItemPONumber Is Nothing AndAlso Not item.ItemPONumber(0) Is Nothing) Then
                                            oItem.ItemPONumber = item.ItemPONumber(0).Data
                                        Else
                                            oItem.ItemPONumber = item.POItemOrderNumber
                                        End If
                                        oBookDetails.Add(oItem)
                                        ElseIf oHeader.POStatusFlag <> 2 Then
                                            'Modified by RHR v-7.0.5.102 09/22/2016
                                            If Me.Debug Or Me.Verbose Then
                                            sLogMsgs.Add("A Booking Order Detail record could not be processed because the record had an invalid Item Number for Order Number " & oHeader.PONumber & " and a POStatus Flag of " & oHeader.POStatusFlag.ToString() & ".")
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            If Me.Debug Or Me.Verbose Then
                                sLogMsgs.Add("A Booking Order record could not be processed because the record had an invalid Order Number value.  This typically indicates that an empty record was being transmitted.")
                            End If
                        End If
                    End If
                Next

                If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count > 0 Then
                    'convert lists to arrays and save changes to database using web service 
                    'Modified by RHR v-7.0.5.100 07/21/2016
                    'Modified by RHR v-8.2 09/18/2018
                    Dim aBookHeaders As TMSIntegrationServices.clsBookHeaderObject80() = oBookHeaders.ToArray()
                    Dim aBookDetails As TMSIntegrationServices.clsBookDetailObject80()
                    If Not oBookDetails Is Nothing AndAlso oBookDetails.Count() > 0 Then aBookDetails = oBookDetails.ToArray()
                    Try
                        Dim sMissingLanes As New List(Of String)
                        For Each h In aBookHeaders
                            'test if the lane exists twice,  if not call processlanedata again
                            Dim blnLaneFound As Boolean = False
                            For t As Integer = 0 To 2
                                Dim sReturnMsg As String = ""
                                Dim blnLaneExists As Boolean = oBookIntegration.doesLaneExist(TMSSetting.TMSAuthCode, h.POVendor, sReturnMsg)
                                If blnLaneExists Then
                                    blnLaneFound = True
                                    Exit For
                                End If
                                'slow down just a bit
                                System.Threading.Thread.Sleep(200)
                                Me.processLaneData(TMSLaneSetting)
                            Next
                            If Not blnLaneFound Then sMissingLanes.Add(h.POVendor)
                        Next
                        If Not sMissingLanes Is Nothing AndAlso sMissingLanes.Count() > 0 Then
                            'Modified by RHR for v-8.4.0.003 on 10/18/2021 added more details to the email message
                            sOperationErrors.Add(" Missing Lanes: the following lanes were not found, unsynchronize in ERP and resend then import orders in the Order Preview screen manually; " & String.Join(", ", sMissingLanes.ToArray()))
                        End If
                    Catch ex As Exception
                        If Me.Debug Then
                            sAdminErrors.Add("(Debuging On) Failed to process missing lanes for orders.  The actual error is: " & ex.ToString)
                        ElseIf Verbose Then
                            sLogMsgs.Add("Failed to process missing lanes for orders.  the actual error is: " & ex.Message)
                        End If
                    End Try
                    'now we can import the orders
#Disable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                    Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessBookData80(TMSSetting.TMSAuthCode, aBookHeaders, aBookDetails, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.


                    Select Case oResults
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                            sIntegrationErrors.Add("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                            If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                blnRet = True
                            End If
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                            If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                blnRet = True
                            End If
                        Case Else
                            'success
                            Dim strNumbers As String = String.Join(", ", Orders)
                            sLogMsgs.Add("Success! the following Order Numbers were processed: " & strNumbers)
                            'Processed = oResults.
                            'TODO: add code to send confirmation back to NAV that the orders were processed
                            'mark process and success
                            blnRet = True
                    End Select
                Else
                    If Verbose Then sLogMsgs.Add("No Orders to Process")
                    blnRet = True
                    'Exit For
                End If
            End If
            'Next
            If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
            End If
            If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
            End If
            If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
            End If
            If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
                Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
            End If
            If Debug Then Log("Process Order Data Complete")
        Catch ex As Exception
            'Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support record locks in ERP
            Dim sEXMessage = ex.Message
            If sEXMessage.Contains("deadlocked") Or sEXMessage.Contains("locked by another user") Then
                Log(Source & " Error!  Booking Order Integration record locked: " & vbCrLf & ex.ToString())
            Else
                LogError(Source & " Error!  Unexpected Booking Order Integration Failure", " Could not import Booking Order information:  ", AdminEmail, ex)
            End If
        End Try

        Return blnRet
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="Orders"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 10/6/2015 v-7.0.4.0
    '''   Added logic in anticipation of NAV code change for maximum number or records to export 
    '''      and new confirmation web method (not ready)
    '''   Added logic to check for existing lane and to call processlanes if the lane does not exist
    '''      an attempt to deal with timing issues on new lanes
    '''      maximum of two tries then an email goes out that a lane was not available.
    '''      requires a new web method to check if the lane exists.
    ''' Modified by RHR v-7.0.5.100 07/21/2016
    '''   Added reference to 705 book objects to support ChangeNo key fields
    ''' Modified by RHR v-8.2 09/18/2018
    '''   added new field for updated NAV integration
    '''   Modified by RHR for v-8.4.0.003 on 10/18/2021 added more details to email 
    '''   Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support record locks in ERP
    ''' </remarks>
    Private Function processOrderData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, ByVal TMSLaneSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Orders As List(Of String) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Orders Is Nothing Then Orders = New List(Of String)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Order; nothing to do returning false")
            Return False
        End If
        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processOrderData2016(TMSSetting, TMSLaneSetting, Orders)
        End If

        If TMSSetting.ERPTypeControl = 4 Then
            Return processOrderDataBC20(TMSSetting, TMSLaneSetting, Orders)
        End If

        Try
            If Debug Then Log("Begin Process Order Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oBookIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oBookIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oBookIntegration.UseDefaultCredentials = True
            Else
                oBookIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            'Modified by RHR v-7.0.5.100 07/21/2016
            'Modified by RHR v-8.2 09/18/2018
            Dim oBookHeaders As New List(Of TMSIntegrationServices.clsBookHeaderObject80)
            Dim oBookDetails As New List(Of TMSIntegrationServices.clsBookDetailObject80)
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavOrders = New NAVService.DynamicsTMSBookings()
            Dim blnSecondLoop As Boolean = False
            Dim sIntegrationErrors As New List(Of String)
            Dim sAdminErrors As New List(Of String)
            Dim sOperationErrors As New List(Of String)
            Dim sLogMsgs As New List(Of String)
            'we limit the reads to 100 in case of an error
            'we are not using the read loop at this time
            'in the future we need to close and reopen the web service object.
            'For i As Integer = 0 To 20

            oNAVWebService.GetBookings(oNavOrders, ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            Dim strItemSkip As New List(Of String)
            If oNavOrders Is Nothing OrElse oNavOrders.Booking Is Nothing OrElse oNavOrders.Booking.Count() < 1 Then
                If Verbose Then
                    sLogMsgs.Add("Waiting on Order Data")
                End If
            Else
                For Each c In oNavOrders.Booking
                    If Not c Is Nothing Then
                        If Not String.IsNullOrWhiteSpace(c.PONumber) Then
                            Orders.Add(c.PONumber)
                            Dim oHeader = New TMSIntegrationServices.clsBookHeaderObject80
                            CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            If oHeader.POModeTypeControl = 0 Then
                                oHeader.POModeTypeControl = 3 'use default as Road
                            End If
                            'Modified by RHR v-7.0.5.100 07/21/2016
                            Dim sFK As String = c.ChangeNo

                            oBookHeaders.Add(oHeader)
                            For Each item In c.Items
                                If Not item Is Nothing Then
                                    If Not String.IsNullOrWhiteSpace(item.ItemNumber) Then
                                        Dim oItem As New TMSIntegrationServices.clsBookDetailObject80
                                        CopyMatchingFieldsImplicitCast(oItem, item, strItemSkip, strMsg)
                                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                                            If Debug Then Log(strMsg)
                                            strMsg = ""
                                        End If
                                        'Modified by RHR v-7.0.5.100 07/21/2016
                                        oItem.ChangeNo = sFK
                                        oBookDetails.Add(oItem)
                                    ElseIf oHeader.POStatusFlag <> 2 Then
                                        'Modified by RHR v-7.0.5.102 09/22/2016
                                        If Me.Debug Or Me.Verbose Then
                                            sLogMsgs.Add("A Booking Order Detail record could not be processed because the record had an invalid Item Number for Order Number " & oHeader.PONumber & " and a POStatus Flag of " & oHeader.POStatusFlag.ToString() & ".")
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            If Me.Debug Or Me.Verbose Then
                                sLogMsgs.Add("A Booking Order record could not be processed because the record had an invalid Order Number value.  This typically indicates that an empty record was being transmitted.")
                            End If
                        End If
                    End If
                Next

                If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count > 0 Then
                    'convert lists to arrays and save changes to database using web service 
                    'Modified by RHR v-7.0.5.100 07/21/2016
                    'Modified by RHR v-8.2 09/18/2018
                    Dim aBookHeaders As TMSIntegrationServices.clsBookHeaderObject80() = oBookHeaders.ToArray()
                    Dim aBookDetails As TMSIntegrationServices.clsBookDetailObject80()
                    If Not oBookDetails Is Nothing AndAlso oBookDetails.Count() > 0 Then aBookDetails = oBookDetails.ToArray()
                    Try
                        Dim sMissingLanes As New List(Of String)
                        For Each h In aBookHeaders
                            'test if the lane exists twice,  if not call processlanedata again
                            Dim blnLaneFound As Boolean = False
                            For t As Integer = 0 To 2
                                Dim sReturnMsg As String = ""
                                Dim blnLaneExists As Boolean = oBookIntegration.doesLaneExist(TMSSetting.TMSAuthCode, h.POVendor, sReturnMsg)
                                If blnLaneExists Then
                                    blnLaneFound = True
                                    Exit For
                                End If
                                'slow down just a bit
                                System.Threading.Thread.Sleep(200)
                                Me.processLaneData(TMSLaneSetting)
                            Next
                            If Not blnLaneFound Then sMissingLanes.Add(h.POVendor)
                        Next
                        If Not sMissingLanes Is Nothing AndAlso sMissingLanes.Count() > 0 Then
                            'Modified by RHR for v-8.4.0.003 on 10/18/2021 added more details to the email message
                            sOperationErrors.Add(" Missing Lanes: the following lanes were not found, unsynchronize in ERP and resend then import orders in the Order Preview screen manually; " & String.Join(", ", sMissingLanes.ToArray()))
                        End If
                    Catch ex As Exception
                        If Me.Debug Then
                            sAdminErrors.Add("(Debuging On) Failed to process missing lanes for orders.  The actual error is: " & ex.ToString)
                        ElseIf Verbose Then
                            sLogMsgs.Add("Failed to process missing lanes for orders.  the actual error is: " & ex.Message)
                        End If
                    End Try
                    'now we can import the orders
#Disable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                    Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessBookData80(TMSSetting.TMSAuthCode, aBookHeaders, aBookDetails, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.


                    Select Case oResults
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                            sIntegrationErrors.Add("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                            If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                blnRet = True
                            End If
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                            If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                blnRet = True
                            End If
                        Case Else
                            'success
                            Dim strNumbers As String = String.Join(", ", Orders)
                            sLogMsgs.Add("Success! the following Order Numbers were processed: " & strNumbers)
                            'Processed = oResults.
                            'TODO: add code to send confirmation back to NAV that the orders were processed
                            'mark process and success
                            blnRet = True
                    End Select
                Else
                    If Verbose Then sLogMsgs.Add("No Orders to Process")
                    blnRet = True
                    'Exit For
                End If
            End If
            'Next
            If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
            End If
            If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
            End If
            If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
            End If
            If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
                Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
            End If
            If Debug Then Log("Process Order Data Complete")
        Catch ex As Exception
            'Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support record locks in ERP
            Dim sEXMessage = ex.Message
            If sEXMessage.Contains("deadlocked") Or sEXMessage.Contains("locked by another user") Then
                Log(Source & " Error!  Booking Order Integration record locked: " & vbCrLf & ex.ToString())
            Else
                LogError(Source & " Error!  Unexpected Booking Order Integration Failure", " Could not import Booking Order information:  ", AdminEmail, ex)
            End If
        End Try

        Return blnRet
    End Function

    Private Function processOrderData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, ByVal TMSLaneSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Orders As List(Of String) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Orders Is Nothing Then Orders = New List(Of String)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Order; nothing to do returning false")
            Return False
        End If
        Try
            If Debug Then Log("Begin Process Order Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oBookIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oBookIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oBookIntegration.UseDefaultCredentials = True
            Else
                oBookIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            'Modified by RHR v-7.0.5.100 07/21/2016
            'Modified by RHR v-8.2 09/18/2018
            Dim oBookHeaders As New List(Of TMSIntegrationServices.clsBookHeaderObject80)
            Dim oBookDetails As New List(Of TMSIntegrationServices.clsBookDetailObject80)

            Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavOrders = New NAV2016Services.DynamicsTMSBookings()
            Dim blnSecondLoop As Boolean = False
            Dim sIntegrationErrors As New List(Of String)
            Dim sAdminErrors As New List(Of String)
            Dim sOperationErrors As New List(Of String)
            Dim sLogMsgs As New List(Of String)
            'we limit the reads to 100 in case of an error
            'we are not using the read loop at this time
            'in the future we need to close and reopen the web service object.
            'For i As Integer = 0 To 20

            oNAVWebService.GetBookings(oNavOrders, ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            Dim strItemSkip As New List(Of String)
            If oNavOrders Is Nothing OrElse oNavOrders.Booking Is Nothing OrElse oNavOrders.Booking.Count() < 1 Then
                If Verbose Then
                    sLogMsgs.Add("Waiting on Order Data")
                End If
            Else
                For Each c In oNavOrders.Booking
                    If Not c Is Nothing Then
                        If Not String.IsNullOrWhiteSpace(c.PONumber) Then
                            Orders.Add(c.PONumber)
                            Dim oHeader = New TMSIntegrationServices.clsBookHeaderObject80
                            CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            If oHeader.POModeTypeControl = 0 Then
                                oHeader.POModeTypeControl = 3 'use default as Road
                            End If
                            'Modified by RHR v-7.0.5.100 07/21/2016
                            Dim sFK As String = c.ChangeNo

                            oBookHeaders.Add(oHeader)
                            For Each item In c.Items
                                If Not item Is Nothing Then
                                    If Not String.IsNullOrWhiteSpace(item.ItemNumber) Then
                                        Dim oItem As New TMSIntegrationServices.clsBookDetailObject80
                                        CopyMatchingFieldsImplicitCast(oItem, item, strItemSkip, strMsg)
                                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                                            If Debug Then Log(strMsg)
                                            strMsg = ""
                                        End If
                                        'Modified by RHR v-7.0.5.100 07/21/2016
                                        oItem.ChangeNo = sFK
                                        oBookDetails.Add(oItem)
                                    ElseIf oHeader.POStatusFlag <> 2 Then
                                        'Modified by RHR v-7.0.5.102 09/22/2016
                                        If Me.Debug Or Me.Verbose Then
                                            sLogMsgs.Add("A Booking Order Detail record could not be processed because the record had an invalid Item Number for Order Number " & oHeader.PONumber & " and a POStatus Flag of " & oHeader.POStatusFlag.ToString() & ".")
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            If Me.Debug Or Me.Verbose Then
                                sLogMsgs.Add("A Booking Order record could not be processed because the record had an invalid Order Number value.  This typically indicates that an empty record was being transmitted.")
                            End If
                        End If
                    End If
                Next

                If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count > 0 Then
                    'convert lists to arrays and save changes to database using web service 
                    'Modified by RHR v-7.0.5.100 07/21/2016
                    'Modified by RHR v-8.2 09/18/2018
                    Dim aBookHeaders As TMSIntegrationServices.clsBookHeaderObject80() = oBookHeaders.ToArray()
                    Dim aBookDetails As TMSIntegrationServices.clsBookDetailObject80()
                    If Not oBookDetails Is Nothing AndAlso oBookDetails.Count() > 0 Then aBookDetails = oBookDetails.ToArray()
                    Try
                        Dim sMissingLanes As New List(Of String)
                        For Each h In aBookHeaders
                            'test if the lane exists twice,  if not call processlanedata again
                            Dim blnLaneFound As Boolean = False
                            For t As Integer = 0 To 2
                                Dim sReturnMsg As String = ""
                                Dim blnLaneExists As Boolean = oBookIntegration.doesLaneExist(TMSSetting.TMSAuthCode, h.POVendor, sReturnMsg)
                                If blnLaneExists Then
                                    blnLaneFound = True
                                    Exit For
                                End If
                                'slow down just a bit
                                System.Threading.Thread.Sleep(200)
                                Me.processLaneData(TMSLaneSetting)
                            Next
                            If Not blnLaneFound Then sMissingLanes.Add(h.POVendor)
                        Next
                        If Not sMissingLanes Is Nothing AndAlso sMissingLanes.Count() > 0 Then
                            sOperationErrors.Add(" Missing Lanes: the following lanes were not found, check orders in the Order Preview screen; " & String.Join(", ", sMissingLanes.ToArray()))
                        End If
                    Catch ex As Exception
                        If Me.Debug Then
                            sAdminErrors.Add("(Debuging On) Failed to process missing lanes for orders.  The actual error is: " & ex.ToString)
                        ElseIf Verbose Then
                            sLogMsgs.Add("Failed to process missing lanes for orders.  the actual error is: " & ex.Message)
                        End If
                    End Try
                    'now we can import the orders
#Disable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.
                    Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessBookData80(TMSSetting.TMSAuthCode, aBookHeaders, aBookDetails, ReturnMessage)
#Enable Warning BC42104 ' Variable 'aBookDetails' is used before it has been assigned a value. A null reference exception could result at runtime.


                    Select Case oResults
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                            sIntegrationErrors.Add("Data Connection Failure! could not import Order information:  " & ReturnMessage)
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                            If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                blnRet = True
                            End If
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                            generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Order, ReturnMessage, False)
                            If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                blnRet = True
                            End If
                        Case Else
                            'success
                            Dim strNumbers As String = String.Join(", ", Orders)
                            sLogMsgs.Add("Success! the following Order Numbers were processed: " & strNumbers)
                            'Processed = oResults.
                            'TODO: add code to send confirmation back to NAV that the orders were processed
                            'mark process and success
                            blnRet = True
                    End Select
                Else
                    If Verbose Then sLogMsgs.Add("No Orders to Process")
                    blnRet = True
                    'Exit For
                End If
            End If
            'Next
            If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
            End If
            If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
            End If
            If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
            End If
            If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
                Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
            End If
            If Debug Then Log("Process Order Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Booking Order Integration Error", Source & " Unexpected Integration Error! Could not import Booking Order information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' Debug on NGLRDP06D Bypassing the Web service 
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <param name="TMSLaneSetting"></param>
    ''' <param name="Orders"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.100 07/21/2016
    '''   Added reference to 705 book objects to support ChangeNo key fields
    ''' </remarks>
    Private Function processOrderDataDLLDirect(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting, ByVal TMSLaneSetting As TMSIntegrationSettings.vERPIntegrationSetting, Optional ByRef Orders As List(Of String) = Nothing) As Boolean
        Dim blnRet As Boolean = False
        If Orders Is Nothing Then Orders = New List(Of String)
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Order; nothing to do returning false")
            Return False
        End If
        Try
            If Debug Then Log("Begin Process Order Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oBookIntegration As New TMS.clsBook()

            With oBookIntegration
                .AdminEmail = Me.AdminEmail
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .Retry = Me.AutoRetry
                .SMTPServer = Me.SMTPServer
                .DBServer = Me.DBServer
                .Database = Me.Database
                .ConnectionString = Me.ConnectionString
                .Debug = Me.Debug
                .AuthorizationCode = TMSSetting.TMSAuthCode
                .WCFAuthCode = "WCFDEV"
                '.WCFURL = "http://TMSBCWCF.NEXTGENERATION.COM"
                '.WCFTCPURL = "net.tcp://TMSBCWCF.NEXTGENERATION.COM:908"
                .WCFURL = "http://NAV2018WCFDev.nextgeneration.com"
                .WCFTCPURL = "net.tcp://NAV2018WCFDev.NEXTGENERATION.COM:908"
            End With
            'Modified by RHR v-8.2 09/18/2018
            Dim oBookHeaders As New List(Of TMS.clsBookHeaderObject80)
            Dim oBookDetails As New List(Of TMS.clsBookDetailObject80)
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavOrders = New NAVService.DynamicsTMSBookings()
            Dim blnSecondLoop As Boolean = False
            Dim sIntegrationErrors As New List(Of String)
            Dim sAdminErrors As New List(Of String)
            Dim sOperationErrors As New List(Of String)
            Dim sLogMsgs As New List(Of String)
            'Note this code will not mark the records as synchronized in NAV until the loop is finished
            'so it will repeat the import 20 times.  this should only be used for testing and debugging
            For i As Integer = 0 To 20
                oNAVWebService.GetBookings(oNavOrders, ERPTestingOn, Not ERPTestingOn)
                Dim strSkip As New List(Of String)
                Dim strItemSkip As New List(Of String)
                If oNavOrders Is Nothing OrElse oNavOrders.Booking Is Nothing OrElse oNavOrders.Booking.Count() < 1 Then
                    If Verbose Then
                        sLogMsgs.Add("Waiting on Order Data")
                    End If
                    Exit For
                End If
                For Each c In oNavOrders.Booking
                    If Not c Is Nothing Then
                        If Not String.IsNullOrWhiteSpace(c.PONumber) Then
                            Orders.Add(c.PONumber)
                            Dim oHeader = New TMS.clsBookHeaderObject80
                            CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            If oHeader.POModeTypeControl = 0 Then
                                oHeader.POModeTypeControl = 3 'use default as Road
                            End If
                            'Modified by RHR v-7.0.5.100 07/21/2016
                            Dim sFK As String = c.ChangeNo

                            oBookHeaders.Add(oHeader)
                            For Each item In c.Items
                                If Not item Is Nothing Then
                                    If Not String.IsNullOrWhiteSpace(item.ItemNumber) Then
                                        Dim oItem As New TMS.clsBookDetailObject80
                                        CopyMatchingFieldsImplicitCast(oItem, item, strItemSkip, strMsg)
                                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                                            If Debug Then Log(strMsg)
                                            strMsg = ""
                                        End If
                                        'Modified by RHR v-7.0.5.100 07/21/2016
                                        oItem.ChangeNo = sFK
                                        oBookDetails.Add(oItem)
                                    Else
                                        sAdminErrors.Add("A Booking Order Detail record could not be processed because the record had an invalid Item Number value for Order Number " & oHeader.PONumber)
                                    End If
                                End If
                            Next
                        Else
                            If Me.Debug Or Me.Verbose Then
                                sLogMsgs.Add("A Booking Order record could not be processed because the record had an invalid Order Number value.  This typically indicates that an empty record was being transmitted.")
                            End If
                        End If
                    End If
                Next

                If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count > 0 Then
                    'force the software to wait for the silent tender process to finish before exiting
                    'typically used for unattended execution
                    oBookIntegration.RunSilentTenderAsync = False
                    'now we can import the orders
                    Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessObjectData(oBookHeaders, oBookDetails, Me.ConnectionString)
                    Dim sLastError As String = oBookIntegration.LastError
                    Select Case oResults
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                            LogError("Error Data Connection Failure! could not import Order information:  " & sLastError)
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                            LogError("Error Integration Failure! could not import Order information:  " & sLastError)
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                            If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                LogError(Source & " Warning!  Order Integration Had Errors", Source & " Warning!  Could not import some Order information:  " & sLastError, AdminEmail)
                                blnRet = True
                            Else
                                LogError(Source & " Warning Integration Had Errors! could not import some Order information:  " & sLastError)
                            End If
                        Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                            If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                                LogError(Source & " Warning!  Order Integration Had Errors", Source & " Error Data Validation Failure! could not import Order information:  " & sLastError, AdminEmail)
                                blnRet = True
                            Else
                                LogError(Source & " Warning Integration Had Errors! could not import some Order information:  " & sLastError)
                            End If
                        Case Else
                            'success
                            Dim strNumbers As String = String.Join(", ", Orders)
                            If Me.Verbose Then Log("Success! the following Order Numbers were processed: " & strNumbers)
                            'Processed = oResults.
                            'TODO: add code to send confirmation back to NAV that the orders were processed
                            'mark process and success
                            blnRet = True
                    End Select
                Else
                    If Verbose Then sLogMsgs.Add("No Orders to Process")
                    blnRet = True
                    Exit For
                End If
            Next
            If Not sIntegrationErrors Is Nothing AndAlso sIntegrationErrors.Count() > 0 Then
                LogError("Warning!  " & Source & " Had Errors: " & vbCrLf & String.Join(vbCrLf, sIntegrationErrors.ToArray()))
            End If
            If Not sAdminErrors Is Nothing AndAlso sAdminErrors.Count() > 0 Then
                LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sAdminErrors.ToArray()), Me.AdminEmail)
            End If
            If Not sOperationErrors Is Nothing AndAlso sOperationErrors.Count() > 0 Then
                LogError("Process Order Import Errors", Source & " reported the following errors: " & vbCrLf & String.Join(vbCrLf, sOperationErrors.ToArray()), Me.GroupEmail)
            End If
            If Not sLogMsgs Is Nothing AndAlso sLogMsgs.Count() > 0 Then
                Log(Source & ": " & vbCrLf & String.Join(vbCrLf, sLogMsgs.ToArray()))
            End If
            If Debug Then Log("Process Order Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Booking Order Integration Error", Source & " Unexpected Integration Error! Could not import Booking Order information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processOrderUnitTest(ByRef Orders As List(Of String), ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = False
        If Orders Is Nothing Then Orders = New List(Of String)
        Try
            Log("Begin Process Order Data ")
            Dim strMsg As String = ""
            Dim oBookIntegration As New TMS.clsBook
            populateIntegrationObjectParameters(oBookIntegration, UnitTestKeys)
            Dim oBookHeaders As New List(Of TMS.clsBookHeaderObject70)
            Dim oBookDetails As New List(Of TMS.clsBookDetailObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                Log("Running unit test with sample data")
                Orders.Add(UnitTestKeys.OrderNumber)
                oBookHeaders.Add(TMS.clsBookHeaderObject70.GenerateSampleObject(UnitTestKeys.OrderNumber, UnitTestKeys.LaneNumber, UnitTestKeys.CompNumber, UnitTestKeys.CompAlphaCode, UnitTestKeys.LegalEntity))
                oBookDetails.Add(TMS.clsBookDetailObject70.GenerateSampleObject(UnitTestKeys.OrderNumber, UnitTestKeys.CompNumber, UnitTestKeys.CompAlphaCode, UnitTestKeys.LegalEntity))
            End If

            If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count > 0 Then
                'force the software to wait for the silent tender process to finish before exiting
                'typically used for unattended execution
                oBookIntegration.RunSilentTenderAsync = False
                'save changes to database 
                Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessObjectData(oBookHeaders, oBookDetails, Me.ConnectionString)
                Dim sLastError As String = oBookIntegration.LastError
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Order information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Error Integration Failure! could not import Order information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Order Integration Had Errors", Source & " Warning!  Could not import some Order information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Order information:  " & sLastError)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Order Integration Had Errors", Source & " Error Data Validation Failure! could not import Order information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Order information:  " & sLastError)
                        End If
                    Case Else
                        'success
                        Dim strNumbers As String = String.Join(", ", Orders)
                        Log("Success! the following Order Numbers were processed: " & strNumbers)
                        'Processed = oResults.
                        'TODO: add code to send confirmation back to NAV that the orders were processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Orders to Process")
                blnRet = True
            End If
            Log("Process Order Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Booking Order Integration Error", Source & " Unexpected Integration Error! Could not import Booking Order information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processPicklistDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Picklist; nothing to do returning false")
            Return False
        End If

        Try
            If Debug Then Log("Begin Process Picklist Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim picklist As New TMSIntegrationServices.DTMSERPIntegration()
            picklist.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                picklist.UseDefaultCredentials = True
            Else
                picklist.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If


            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation)

            Dim oPickListData As TMSIntegrationServices.clsPickListData80 = picklist.GetPickListData80(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation, RetVal, ReturnMessage)
            LastError = ReturnMessage
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export Picklist information:  " & LastError)
                        Return False
                    Case Else
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, RetVal, IntegrationModule.PickList, LastError)
                        Return False
                End Select
            End If
            If oPickListData Is Nothing Then Return True

            If Not oPickListData.Headers Is Nothing AndAlso oPickListData.Headers.Count() > 0 Then
                If Me.Verbose Then Log("Processing " & oPickListData.Headers.Count().ToString() & " Pick List Header Records.")
                System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
                Dim oBCIntegration = New BCIntegration.clsApplication()
                Dim oSendPicks = New Pick.SendPicks()
                Dim strSkip As New List(Of String)
                Dim strSkipLine As New List(Of String)
                '  Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 3. Only send one record at a time.  Email and log any errors we count the number or reords processed and the number of errors
                Dim iProcessed As Integer = oPickListData.Headers.Count()
                Dim iErrors As Integer = 0


                For Each c In oPickListData.Headers
                    ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time. 
                    '  we moved the definition of lists inside the For Loop so each transaction is for one Pick list record
                    Dim oPicks As New List(Of Pick.Pick)
                    If Not c Is Nothing AndAlso c.PLControl <> 0 Then
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 2. If an item hase "Comment" as the item number do Not send
                        Dim d As TMSIntegrationServices.clsPickDetailObject80() = oPickListData.Details.Where(Function(x) x.PLControl = c.PLControl).Where(Function(y) Not String.IsNullOrWhiteSpace(y.ItemNumber) AndAlso y.ItemNumber.Trim.ToLower <> "comment").ToArray()
                        If Not d Is Nothing AndAlso d.Count() > 0 Then
                            Dim oPick = New Pick.Pick()
                            Dim dtTmp As Date = DateTime.MinValue
                            If Date.TryParse(c.BookDateLoad, dtTmp) Then c.BookDateLoad = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Load: " & c.BookDateLoad)
                            If Date.TryParse(c.BookDateRequired, dtTmp) Then c.BookDateRequired = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Required: " & c.BookDateRequired)
                            If Date.TryParse(c.BookRouteFinalDate, dtTmp) Then c.BookRouteFinalDate = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Route Final Date: " & c.BookRouteFinalDate)
                            If Date.TryParse(c.BookDateOrdered, dtTmp) Then c.BookDateOrdered = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Ordered: " & c.BookDateOrdered)
                            CopyMatchingFieldsImplicitCast(oPick, c, strSkip, strMsg)
                            'Log("pick load Date: " & oPick.BookDateLoad)
                            'Log("pick req Date: " & oPick.BookDateRequired)
                            'Log("pick final Date: " & oPick.BookRouteFinalDate)
                            'Log("pick order Date: " & oPick.BookDateOrdered)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            Dim oLines As New List(Of Pick.PickLines)
                            For Each i In d
                                Dim oLine As New Pick.PickLines()
                                CopyMatchingFieldsImplicitCast(oLine, i, strSkipLine, strMsg)
                                If Not String.IsNullOrWhiteSpace(strMsg) Then
                                    If Debug Then Log(strMsg)
                                    strMsg = ""
                                End If
                                oLines.Add(oLine)
                            Next
                            If Not oLines Is Nothing AndAlso oLines.Count > 0 Then
                                oPick.Lines = oLines.ToArray()
                            End If
                            oPicks.Add(oPick)
                        Else
                            ' Rule 1. If no items do not send, already existed prior to v-8.4.0.002
                            iErrors += 1
                            Dim sSubject = "Process " & TMSSetting.ERPTypeName & " Picklist Integration Failure Manual Processing Required"
                            createERPInegrationFailureSubscriptionAlert(Subject:=sSubject,
                                                                        enmIntegration:=IntegrationModule.PickList,
                                                                        CompControl:=0,
                                                                        CompNumber:=c.CompNumber,
                                                                        CompAlphaCode:=c.CompAlphaCode,
                                                                        CarrierNumber:=c.CarrierNumber,
                                                                        CarrierAlphaCode:=c.CarrierAlphaCode,
                                                                        OrderNumber:=c.BookCarrOrderNumber,
                                                                        OrderSequence:=c.BookOrderSequence.ToString(),
                                                                        keyControl:=c.PLControl,
                                                                        Warnings:="Item Details are required. At least one Item, with an NAV Item Number, is required to Process Picklist Data using the NAV Integration module.  You must correct the items, unfinalize, then refinalize this order!")
                            'we must mark the record as exported because it will keep trying to resend
                            picklist.ConfirmPickListExport70(TMSSetting.TMSAuthCode, c.PLControl, ReturnMessage)
                            Continue For
                        End If
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                        '  we moved the code below that sends to NAV inside the loop so each transaction is for one AP Control record
                        Dim sOrderInfo As String = "Unavailable"
                        Dim iPLControl As Long
                        'Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support GL record locks in ERP
                        If Not oPicks Is Nothing AndAlso oPicks.Count > 0 Then
                            Dim iRetryCt As Integer = 0
                            Dim blnRetryNoGL As Boolean = True 'default is true must be set to false on success
                            Dim blnConfirmPick As Boolean = False
                            Dim blnReadyToSend As Boolean = False
                            Dim iFailedAttempts As Integer = 0
                            Dim eFailed As Exception = Nothing
                            iPLControl = oPicks(0).PLControl
                            sOrderInfo = "Order No.: " & oPicks(0).BookCarrOrderNumber & ", Ship ID: " & If(String.IsNullOrWhiteSpace(oPicks(0).BookSHID), oPicks(0).BookConsPrefix, oPicks(0).BookSHID) & ", using Pick List Control Number: " & iPLControl.ToString()

                            Try
                                'read the pick data 
                                oSendPicks.dynamicsTMSPicks = oPicks.ToArray()
                                blnReadyToSend = True
                            Catch ex As Exception
                                iErrors += 1
                                LogError(Me.Source & " Picklist ", " Unable to generate the NAV Pick Data.  There is a problem with the TMS data for " & sOrderInfo & ".  Please check the record and contact NGL Support.", Me.AdminEmail, ex)
                            End Try
                            Dim sMsg As String
                            Dim blnSuccess As Boolean = False
                            If blnReadyToSend Then
                                Do
                                    Try
                                        blnSuccess = oBCIntegration.sendPicks(getAuthsSettngs(TMSSetting), oSendPicks, sMsg)
                                        If blnSuccess Then
                                            blnConfirmPick = True
                                            'success do not retry
                                            iRetryCt = iTMSPickRetryAttempts + 1
                                            blnRetryNoGL = False 'default is true must be set to false on success
                                        End If

                                    Catch ex As Exception
                                        iFailedAttempts += 1
                                        eFailed = ex
                                        Log(String.Format("{0} Send Picklist Status Update failed on retry attempt # {1}, {2} Unable to export the Picklist Status Update for {3}. {2} Due to the following error: {2} {4}", Me.Source, iFailedAttempts, vbCrLf, sOrderInfo, ex.ToString()))
                                        System.Threading.Thread.Sleep(iTMSPickRetryMilliSeconds)

                                    End Try
                                    iRetryCt += 1
                                Loop While iRetryCt <= iTMSPickRetryAttempts
                                If Not blnSuccess Then
                                    Log(String.Format("{0} Send Picklist Status Update failed on retry attempt # {1}, {2} Unable to export the Picklist Status Update for {3}. {2} Due to the following server error: {2} {4}", Me.Source, iFailedAttempts, vbCrLf, sOrderInfo, sMsg))
                                End If
                                If blnRetryNoGL Then
                                    Try
                                        blnSuccess = oBCIntegration.sendPicksNoPost(getAuthsSettngs(TMSSetting), oSendPicks, sMsg)
                                        If blnSuccess Then
                                            blnConfirmPick = True
                                        Else
                                            eFailed = New System.ApplicationException("TMS cannot update the accrual G/L. A record may be locked by an external batch process. Server Message: " & sMsg)
                                        End If
                                        If eFailed Is Nothing Then
                                            eFailed = New System.ApplicationException("TMS cannot update the accrual G/L. A record may be locked by an external batch process")
                                        End If
                                        LogError(Me.Source & "Update Picklist Accrual Failure", "The system failed to update the accrual G/L " & iFailedAttempts.ToString() & " times without success.  Please open the Pick Worksheet and add the accruals manually for " & sOrderInfo, Me.AdminEmail, eFailed)

                                    Catch ex As Exception
                                        iErrors += 1
                                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                                        LogError(Me.Source & " Post to Pick Worksheet Failure", " TMS was unable to export the Pick worksheet data for " & sOrderInfo & ".  Please follow the manual re-send procedure.", Me.AdminEmail, ex)
                                    End Try
                                End If
                                If blnConfirmPick Then
                                    Try
                                        ReturnMessage = ""
                                        picklist.ConfirmPickListExport70(TMSSetting.TMSAuthCode, oPicks(0).PLControl, ReturnMessage)
                                    Catch ex As Exception
                                        iErrors += 1
                                        LogError(Me.Source & " Update Picklist Confirmation Error", "TMS was unable to update the TMS Picklist export confirmation flag for " & sOrderInfo & ". A duplicate post may be created if Retry is enabled.  Please check the data manually. ", Me.AdminEmail, ex)
                                    End Try
                                    blnRet = True
                                End If
                            End If
                        Else
                            If Verbose Then Log("No Data Found for Picklist Picks Array")
                            blnRet = True
                        End If

                    End If
                Next
                Log("Picklist Status Update processed " & iProcessed.ToString() & " records and found " & iErrors.ToString() & " errors or warning.")

            Else
                If Verbose Then Log("No Pick List Status Updates to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Picklist Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Picklist Integration Error", Source & " Unexpected Integration Error! Could not import Picklist information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function


    ''' <summary>
    ''' Read TMS Pick Worksheet Data and send to NAV Pick Worksheet web service
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the TMS Web Service with 80 version
    '''   includes support for BookItemOrderNumber 
    ''' Modified by RHR for v-8.4.0.002 on 03/30/2021
    '''     Added three new rules.
    '''     1. if no items do not send
    '''     2. If an item has "Comment" as the item number do not send
    '''     3. Only send one record at a time.  Email and log any errors  
    ''' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
    ''' Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support GL record locks in ERP
    ''' </remarks>
    Private Function processPicklistData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Picklist; nothing to do returning false")
            Return False
        End If
        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processPicklistData2016(TMSSetting)
        End If

        If TMSSetting.ERPTypeControl = 4 Then
            Return processPicklistDataBC20(TMSSetting)
        End If

        Try
            If Debug Then Log("Begin Process Picklist Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim picklist As New TMSIntegrationServices.DTMSERPIntegration()
            picklist.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                picklist.UseDefaultCredentials = True
            Else
                picklist.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If


            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation)

            Dim oPickListData As TMSIntegrationServices.clsPickListData80 = picklist.GetPickListData80(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation, RetVal, ReturnMessage)
            LastError = ReturnMessage
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export Picklist information:  " & LastError)
                        Return False
                    Case Else
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, RetVal, IntegrationModule.PickList, LastError)
                        Return False
                End Select
            End If
            If oPickListData Is Nothing Then Return True

            If Not oPickListData.Headers Is Nothing AndAlso oPickListData.Headers.Count() > 0 Then
                'We only process the dynamics tms web services when UnitTestKeys are not provided
                ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
                If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
                Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
                oNAVWebService.Url = TMSSetting.ERPURI
                If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                    oNAVWebService.UseDefaultCredentials = True
                Else
                    oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
                End If
                Dim oNavPicklist = New NAVService.DynamicsTMSPicks

                Dim strSkip As New List(Of String)
                Dim strSkipLine As New List(Of String)
                '  Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 3. Only send one record at a time.  Email and log any errors we count the number or reords processed and the number of errors
                Dim iProcessed As Integer = oPickListData.Headers.Count()
                Dim iErrors As Integer = 0


                For Each c In oPickListData.Headers
                    ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time. 
                    '  we moved the definition of lists inside the For Loop so each transaction is for one Pick list record
                    Dim oPicks As New List(Of NAVService.Pick)
                    If Not c Is Nothing AndAlso c.PLControl <> 0 Then
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 2. If an item hase "Comment" as the item number do Not send
                        Dim d As TMSIntegrationServices.clsPickDetailObject80() = oPickListData.Details.Where(Function(x) x.PLControl = c.PLControl).Where(Function(y) Not String.IsNullOrWhiteSpace(y.ItemNumber) AndAlso y.ItemNumber.Trim.ToLower <> "comment").ToArray()
                        If Not d Is Nothing AndAlso d.Count() > 0 Then
                            Dim oPick = New NAVService.Pick()
                            Dim dtTmp As Date = DateTime.MinValue
                            If Date.TryParse(c.BookDateLoad, dtTmp) Then c.BookDateLoad = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Load: " & c.BookDateLoad)
                            If Date.TryParse(c.BookDateRequired, dtTmp) Then c.BookDateRequired = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Required: " & c.BookDateRequired)
                            If Date.TryParse(c.BookRouteFinalDate, dtTmp) Then c.BookRouteFinalDate = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Route Final Date: " & c.BookRouteFinalDate)
                            If Date.TryParse(c.BookDateOrdered, dtTmp) Then c.BookDateOrdered = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Ordered: " & c.BookDateOrdered)
                            CopyMatchingFieldsImplicitCast(oPick, c, strSkip, strMsg)
                            'Log("pick load Date: " & oPick.BookDateLoad)
                            'Log("pick req Date: " & oPick.BookDateRequired)
                            'Log("pick final Date: " & oPick.BookRouteFinalDate)
                            'Log("pick order Date: " & oPick.BookDateOrdered)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            Dim oLines As New List(Of NAVService.Lines)
                            For Each i In d
                                Dim oLine As New NAVService.Lines()
                                CopyMatchingFieldsImplicitCast(oLine, i, strSkipLine, strMsg)
                                If Not String.IsNullOrWhiteSpace(strMsg) Then
                                    If Debug Then Log(strMsg)
                                    strMsg = ""
                                End If
                                oLines.Add(oLine)
                            Next
                            If Not oLines Is Nothing AndAlso oLines.Count > 0 Then
                                oPick.Lines = oLines.ToArray()
                            End If
                            oPicks.Add(oPick)
                        Else
                            ' Rule 1. If no items do not send, already existed prior to v-8.4.0.002
                            iErrors += 1
                            Dim sSubject = "Process " & TMSSetting.ERPTypeName & " Picklist Integration Failure Manual Processing Required"
                            createERPInegrationFailureSubscriptionAlert(Subject:=sSubject,
                                                                        enmIntegration:=IntegrationModule.PickList,
                                                                        CompControl:=0,
                                                                        CompNumber:=c.CompNumber,
                                                                        CompAlphaCode:=c.CompAlphaCode,
                                                                        CarrierNumber:=c.CarrierNumber,
                                                                        CarrierAlphaCode:=c.CarrierAlphaCode,
                                                                        OrderNumber:=c.BookCarrOrderNumber,
                                                                        OrderSequence:=c.BookOrderSequence.ToString(),
                                                                        keyControl:=c.PLControl,
                                                                        Warnings:="Item Details are required. At least one Item, with an NAV Item Number, is required to Process Picklist Data using the NAV Integration module.  You must correct the items, unfinalize, then refinalize this order!")
                            'we must mark the record as exported because it will keep trying to resend
                            picklist.ConfirmPickListExport70(TMSSetting.TMSAuthCode, c.PLControl, ReturnMessage)
                            Continue For
                        End If
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                        '  we moved the code below that sends to NAV inside the loop so each transaction is for one AP Control record
                        Dim sOrderInfo As String = "Unavailable"
                        Dim iPLControl As Long
                        'Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support GL record locks in ERP
                        If Not oPicks Is Nothing AndAlso oPicks.Count > 0 Then
                            Dim iRetryCt As Integer = 0
                            Dim blnRetryNoGL As Boolean = True 'default is true must be set to false on success
                            Dim blnConfirmPick As Boolean = False
                            Dim blnReadyToSend As Boolean = False
                            Dim iFailedAttempts As Integer = 0
                            Dim eFailed As Exception = Nothing
                            iPLControl = oPicks(0).PLControl
                            sOrderInfo = "Order No.: " & oPicks(0).BookCarrOrderNumber & ", Ship ID: " & If(String.IsNullOrWhiteSpace(oPicks(0).BookSHID), oPicks(0).BookConsPrefix, oPicks(0).BookSHID) & ", using Pick List Control Number: " & iPLControl.ToString()

                            Try
                                'read the pick data 
                                oNavPicklist.Pick = oPicks.ToArray()
                                blnReadyToSend = True
                            Catch ex As Exception
                                iErrors += 1
                                LogError(Me.Source & " Picklist ", " Unable to generate the NAV Pick Data.  There is a problem with the TMS data for " & sOrderInfo & ".  Please check the record and contact NGL Support.", Me.AdminEmail, ex)
                            End Try
                            If blnReadyToSend Then
                                Do
                                    Try
                                        oNAVWebService.SendPicks(oNavPicklist)
                                        blnConfirmPick = True
                                        'success do not retry
                                        iRetryCt = iTMSPickRetryAttempts + 1
                                        blnRetryNoGL = False 'default is true must be set to false on success
                                    Catch ex As Exception
                                        iFailedAttempts += 1
                                        eFailed = ex
                                        Log(String.Format("{0} Send Picklist Status Update failed on retry attempt # {1}, {2} Unable to export the Picklist Status Update for {3}. {2} Due to the following error: {2} {4}", Me.Source, iFailedAttempts, vbCrLf, sOrderInfo, ex.ToString()))
                                        System.Threading.Thread.Sleep(iTMSPickRetryMilliSeconds)

                                    End Try
                                    iRetryCt += 1
                                Loop While iRetryCt <= iTMSPickRetryAttempts
                                If blnRetryNoGL Then
                                    Try

                                        oNAVWebService.SendPicksNoPost(oNavPicklist)
                                        blnConfirmPick = True
                                        If eFailed Is Nothing Then
                                            eFailed = New System.ApplicationException("TMS cannot update the accrual G/L. A record may be locked by an external batch process")
                                        End If
                                        LogError(Me.Source & "Update Picklist Accrual Failure", "The system failed to update the accrual G/L " & iFailedAttempts.ToString() & " times without success.  Please open the Pick Worksheet and add the accruals manually for " & sOrderInfo, Me.AdminEmail, eFailed)

                                    Catch ex As Exception
                                        iErrors += 1
                                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                                        LogError(Me.Source & " Post to Pick Worksheet Failure", " TMS was unable to export the Pick worksheet data for " & sOrderInfo & ".  Please follow the manual re-send procedure.", Me.AdminEmail, ex)
                                    End Try
                                End If
                                If blnConfirmPick Then
                                    Try
                                        ReturnMessage = ""
                                        picklist.ConfirmPickListExport70(TMSSetting.TMSAuthCode, oPicks(0).PLControl, ReturnMessage)
                                    Catch ex As Exception
                                        iErrors += 1
                                        LogError(Me.Source & " Update Picklist Confirmation Error", "TMS was unable to update the TMS Picklist export confirmation flag for " & sOrderInfo & ". A duplicate post may be created if Retry is enabled.  Please check the data manually. ", Me.AdminEmail, ex)
                                    End Try
                                    blnRet = True
                                End If
                            End If
                        Else
                            If Verbose Then Log("No Data Found for Picklist Picks Array")
                            blnRet = True
                        End If

                    End If
                Next
                Log("Picklist Status Update processed " & iProcessed.ToString() & " records and found " & iErrors.ToString() & " errors or warning.")

            Else
                If Verbose Then Log("No Pick List Status Updates to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Picklist Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Picklist Integration Error", Source & " Unexpected Integration Error! Could not import Picklist information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processPicklistData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Picklist; nothing to do returning false")
            Return False
        End If
        Try
            If Debug Then Log("Begin Process Picklist Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim picklist As New TMSIntegrationServices.DTMSERPIntegration()
            picklist.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                picklist.UseDefaultCredentials = True
            Else
                picklist.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If


            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation)

            Dim oPickListData As TMSIntegrationServices.clsPickListData80 = picklist.GetPickListData80(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation, RetVal, ReturnMessage)
            LastError = ReturnMessage
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export Picklist information:  " & LastError)
                        Return False
                    Case Else
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, RetVal, IntegrationModule.PickList, LastError)
                        Return False
                End Select
            End If
            If oPickListData Is Nothing Then Return True

            If Not oPickListData.Headers Is Nothing AndAlso oPickListData.Headers.Count() > 0 Then
                'We only process the dynamics tms web services when UnitTestKeys are not provided
                Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
                oNAVWebService.Url = TMSSetting.ERPURI
                If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                    oNAVWebService.UseDefaultCredentials = True
                Else
                    oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
                End If
                Dim oNavPicklist = New NAV2016Services.DynamicsTMSPicks

                Dim strSkip As New List(Of String)

                Dim strSkipLine As New List(Of String)

                strSkip.Add("BookItemOrderNumber")
                strSkipLine.Add("BookItemOrderNumber")

                Dim oPicks As New List(Of NAV2016Services.Pick)
                For Each c In oPickListData.Headers
                    If Not c Is Nothing AndAlso c.PLControl <> 0 Then
                        Dim d As TMSIntegrationServices.clsPickDetailObject80() = oPickListData.Details.Where(Function(x) x.PLControl = c.PLControl).Where(Function(y) Not String.IsNullOrWhiteSpace(y.ItemNumber)).ToArray()
                        If Not d Is Nothing AndAlso d.Count() > 0 Then
                            Dim oPick = New NAV2016Services.Pick()
                            Dim dtTmp As Date = DateTime.MinValue
                            If Date.TryParse(c.BookDateLoad, dtTmp) Then c.BookDateLoad = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Load: " & c.BookDateLoad)
                            If Date.TryParse(c.BookDateRequired, dtTmp) Then c.BookDateRequired = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Required: " & c.BookDateRequired)
                            If Date.TryParse(c.BookRouteFinalDate, dtTmp) Then c.BookRouteFinalDate = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Route Final Date: " & c.BookRouteFinalDate)
                            If Date.TryParse(c.BookDateOrdered, dtTmp) Then c.BookDateOrdered = dtTmp.ToString("MM/dd/yyyy HH:mm:ss")
                            'Log("Book Date Ordered: " & c.BookDateOrdered)
                            CopyMatchingFieldsImplicitCast(oPick, c, strSkip, strMsg)
                            'Log("pick load Date: " & oPick.BookDateLoad)
                            'Log("pick req Date: " & oPick.BookDateRequired)
                            'Log("pick final Date: " & oPick.BookRouteFinalDate)
                            'Log("pick order Date: " & oPick.BookDateOrdered)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            Dim oLines As New List(Of NAV2016Services.Lines)
                            For Each i In d
                                Dim oLine As New NAV2016Services.Lines()
                                CopyMatchingFieldsImplicitCast(oLine, i, strSkipLine, strMsg)
                                If Not String.IsNullOrWhiteSpace(strMsg) Then
                                    If Debug Then Log(strMsg)
                                    strMsg = ""
                                End If
                                oLines.Add(oLine)
                            Next
                            If Not oLines Is Nothing AndAlso oLines.Count > 0 Then
                                oPick.Lines = oLines.ToArray()
                            End If
                            oPicks.Add(oPick)
                        Else
                            Dim sSubject = "Process " & TMSSetting.ERPTypeName & " Picklist Integration Failure Manual Processing Required"
                            createERPInegrationFailureSubscriptionAlert(Subject:=sSubject,
                                                                        enmIntegration:=IntegrationModule.PickList,
                                                                        CompControl:=0,
                                                                        CompNumber:=c.CompNumber,
                                                                        CompAlphaCode:=c.CompAlphaCode,
                                                                        CarrierNumber:=c.CarrierNumber,
                                                                        CarrierAlphaCode:=c.CarrierAlphaCode,
                                                                        OrderNumber:=c.BookCarrOrderNumber,
                                                                        OrderSequence:=c.BookOrderSequence.ToString(),
                                                                        keyControl:=c.PLControl,
                                                                        Warnings:="Item Details are required. At least one Item, with an NAV Item Number, is required to Process Picklist Data using the NAV Integration module.  You must correct the items, unfinalize, then refinalize this order!")
                            'we must mark the record as exported because it will keep trying to resend
                            picklist.ConfirmPickListExport70(TMSSetting.TMSAuthCode, c.PLControl, ReturnMessage)
                        End If
                    End If
                Next
                If Not oPicks Is Nothing AndAlso oPicks.Count > 0 Then
                    oNavPicklist.Pick = oPicks.ToArray()
                    oNAVWebService.SendPicks(oNavPicklist)
                    For Each p In oPicks
                        Try
                            ReturnMessage = ""
                            picklist.ConfirmPickListExport70(TMSSetting.TMSAuthCode, p.PLControl, ReturnMessage)
                        Catch ex As Exception
                            LogError(Me.Source & " Update Picklist Confirmation Error", Me.Source & " Update picklist confirmation failed for Order Number: " & p.BookCarrOrderNumber & " using PL Control Number: " & p.PLControl, Me.AdminEmail, ex)
                        End Try
                    Next
                    blnRet = True
                Else
                    If Verbose Then Log("No Data Found for Picklist Picks Array")
                    blnRet = True
                End If

            Else
                If Verbose Then Log("No Pick List Status Updates to Process")
                blnRet = True
            End If
            If Debug Then Log("Process Picklist Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Picklist Integration Error", Source & " Unexpected Integration Error! Could not import Picklist information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' Unit Test for TMS Pick Worksheet Data 
    ''' </summary>
    ''' <param name="UnitTestKeys"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the TMS interface with 80 version
    '''   includes support for BookItemOrderNumber 
    ''' </remarks>
    Public Function processPicklistUnitTest(ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = False
        Try

            Log("Begin Process Picklist Data ")
            Dim picklist As New TMS.clsPickList
            populateIntegrationObjectParameters(picklist, UnitTestKeys)
            Dim Headers() As TMS.clsPickListObject80
            Dim Details() As TMS.clsPickDetailObject80
            Dim Fees() As TMS.clsPickListFeeObject80
            Dim strMsg As String = ""
            'set the default value to false
            Dim RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim intMaxRetry As Integer = 0
            Dim intRetryMinutes As Integer = 30
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                intMaxRetry = UnitTestKeys.PicklistMaxRetry
                intRetryMinutes = UnitTestKeys.PicklistRetryMinutes
                picklist.MaxRowsReturned = UnitTestKeys.PicklistMaxRowsReturned
                picklist.AutoConfirmation = UnitTestKeys.PicklistAutoConfirmation
            End If
            If String.IsNullOrWhiteSpace(Me.ConnectionString) Then
                If Not ConfigureInstance(UnitTestKeys.Source, UnitTestKeys.DBName, UnitTestKeys.DBServer, UnitTestKeys.ConnectionSting, UnitTestKeys.DBUser, UnitTestKeys.DBPass, UnitTestKeys.LegalEntity, UnitTestKeys.Debug, UnitTestKeys.Verbos) Then
                    LogError("Invalid Instance Configuration")
                    Return False
                End If
            End If

            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", intMaxRetry, intRetryMinutes, picklist.MaxRowsReturned, picklist.AutoConfirmation)

#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = picklist.readObjectData80(Headers, Me.ConnectionString, intMaxRetry, intRetryMinutes, LegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            LastError = picklist.LastError
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export Picklist information:  " & LastError)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Picklist Integration Error", "Error Integration Failure! could not export Picklist information:  " & LastError, AdminEmail)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        LogError("Picklist Integration Error", "Error Integration Had Errors! could not export some Picklist information:  " & LastError, AdminEmail)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        LogError("Picklist Integration Error", "Error Data Validation Failure! could not export Picklist information:  " & LastError, AdminEmail)
                        Return False
                End Select
            End If
            If Not Headers Is Nothing AndAlso Headers.Count() > 0 Then
                For Each p In Headers
                    Try
                        picklist.confirmExport(Me.ConnectionString, p.PLControl)
                    Catch ex As Exception
                        LogError(Me.Source & " Update Picklist Confirmation Error", Me.Source & " Update picklist confirmation failed for Order Number: " & p.BookCarrOrderNumber & " using PL Control Number: " & p.PLControl, Me.AdminEmail)
                    End Try
                Next
            Else
                Log("No Pick List Status Updates to Process")
            End If
            blnRet = True
            Log("Process Picklist Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Picklist Integration Error", Source & " Unexpected Integration Error! Could not import Picklist information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processAPExportDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for APExport; nothing to do returning false")
            Return False
        End If

        Try
            If Debug Then Log("Begin Process APExport Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim apExport As New TMSIntegrationServices.DTMSERPIntegration()
            apExport.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                apExport.UseDefaultCredentials = True
            Else
                apExport.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If

            Dim iMaxRows As Integer = 1
            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, iMaxRows, TMSSetting.ERPExportAutoConfirmation)
            Dim oAPExportData As TMSIntegrationServices.clsAPExportData80 = apExport.GetAPData80(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, iMaxRows, TMSSetting.ERPExportAutoConfirmation, RetVal, ReturnMessage)
            LastError = ReturnMessage
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export APExport information:  " & LastError)
                        Return False
                    Case Else
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, RetVal, IntegrationModule.APExport, LastError)
                        Return False
                End Select
            End If
            If oAPExportData Is Nothing Then Return True

            If Not oAPExportData.Headers Is Nothing AndAlso oAPExportData.Headers.Count() > 0 Then
                If Me.Verbose Then Log("Processing " & oAPExportData.Headers.Count().ToString() & " AP Header Records.")
                System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
                Dim oBCIntegration = New BCIntegration.clsApplication()
                Dim oSendAP = New AP.SendAP()
                Dim strSkip As New List(Of String)
                Dim strSkipLine As New List(Of String)
                '  Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 3. Only send one record at a time.  Email and log any errors we count the number or reords processed and the number of errors
                Dim iProcessed As Integer = oAPExportData.Headers.Count()
                Dim iErrors As Integer = 0
                Dim sErrorMsg As String = ""
                For Each c In oAPExportData.Headers
                    ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time. 
                    '  we moved the definition of lists inside the For Loop so each transaction is for one AP Control record
                    Dim lAPs As New List(Of AP.AP)
                    If Not c Is Nothing AndAlso c.APControl <> 0 Then
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 2. If an item has "Comment" as the item number do Not send
                        Dim d As TMSIntegrationServices.clsAPExportDetailObject80() = oAPExportData.Details.Where(Function(x) x.APControl = c.APControl).Where(Function(y) Not String.IsNullOrWhiteSpace(y.ItemNumber) AndAlso y.ItemNumber.Trim.ToLower <> "comment").ToArray()
                        If Not d Is Nothing AndAlso d.Count() > 0 Then
                            ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 4. Check for items with zero freight cost, if exists skip and wait for next round 
                            If (d.Any(Function(x) x.FreightCost <= 0)) Then
                                iErrors += 1
                                sErrorMsg = "Skip AP Export Record,  No freight Cost for one or more items for Order No.: " & c.BookCarrOrderNumber & ", Ship ID: " & If(String.IsNullOrWhiteSpace(c.BookSHID), c.BookConsPrefix, c.BookSHID)
                                LogError(Me.Source & " Export AP Information Error", Me.Source & sErrorMsg, Me.AdminEmail)
                                Continue For 'skip the code below and go back to the top with the next each of c
                            End If
                            Dim oAP = New AP.AP()
                            CopyMatchingFieldsImplicitCast(oAP, c, strSkip, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            Dim oDetails As New List(Of AP.APDetails)
                            For Each i In d
                                Dim oDetail As New AP.APDetails()
                                CopyMatchingFieldsImplicitCast(oDetail, i, strSkipLine, strMsg)
                                If Not String.IsNullOrWhiteSpace(strMsg) Then
                                    If Debug Then Log(strMsg)
                                    strMsg = ""
                                End If
                                oDetails.Add(oDetail)
                            Next
                            If Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                                oAP.Details = oDetails.ToArray()
                            End If
                            lAPs.Add(oAP)
                        Else
                            ' Rule 1. If no items do not send, already existed prior to v-8.4.0.002
                            iErrors += 1
                            Dim sSubject = "Process " & TMSSetting.ERPTypeName & " AP Export Failure Manual Processing Required"
                            Dim sWarning As String = "Item Details are required. At least one Item, with a NAV Item Number, is required to Process AP Data using the NAV Integration module.  You must correct the issue and resend or enter this freight bill manually into NAV!"
                            Dim sBody As String = String.Format("Warning: {1} {0}Details: {0}Comp No: {2}{0}
                                                                 Location Code: {3}{0}
                                                                 Carrier No: {4}{0},
                                                                 Carrier Vendor: {5}{0},
                                                                 OrderNumber: {6}{0},
                                                                 Freight Bill No: {7}{0}",
                                                                vbCrLf,
                                                                sWarning,
                                                                c.CompanyNumber,
                                                                c.CompAlphaCode,
                                                                c.CarrierNumber,
                                                                c.CarrierAlphaCode,
                                                                c.BookCarrOrderNumber,
                                                                c.BookFinAPBillNumber)

                            createERPInegrationFailureSubscriptionAlert(Subject:=sSubject,
                                                                        enmIntegration:=IntegrationModule.APExport,
                                                                        CompControl:=0,
                                                                        CompNumber:=c.CompanyNumber,
                                                                        CompAlphaCode:=c.CompAlphaCode,
                                                                        CarrierNumber:=c.CarrierNumber,
                                                                        CarrierAlphaCode:=c.CarrierAlphaCode,
                                                                        OrderNumber:=c.BookCarrOrderNumber,
                                                                        OrderSequence:=c.BookOrderSequence.ToString(),
                                                                        keyControl:=c.APControl,
                                                                        keyString:=c.BookFinAPBillNumber,
                                                                        Warnings:="Item Details are required. At least one Item, with an NAV Item Number, is required to Process AP Data using the NAV Integration module.  You must enter this freight bill manually into NAV!")
                            LogError(Me.Source & sSubject, sBody, Me.AdminEmail)

                            'we must mark the record as exported because it will keep trying to resend
                            apExport.ConfirmAPExport70(TMSSetting.TMSAuthCode, c.APControl, ReturnMessage)
                            Continue For
                        End If
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                        '  we moved the code below that sends to NAV inside the loop so each transaction is for one AP Control record
                        Dim sOrderInfo As String = "Unavailable"
                        Dim iAPControl As Long
                        'Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support GL record locks in ERP
                        'Modified by RHR for v-8.4.0.004 on 02/05/2022 removed support multiple retry to support GL record locks in ERP because this creats duplicate records.
                        '  Users must resend manually on failure
                        If Not lAPs Is Nothing AndAlso lAPs.Count > 0 Then
                            Dim iRetryCt As Integer = 0
                            Dim blnRetryNoGL As Boolean = True 'default is true must be set to false on success
                            Dim blnConfirmAP As Boolean = False
                            Dim blnReadyToSend As Boolean = False
                            Dim iFailedAttempts As Integer = 0
                            Dim eFailed As Exception = Nothing
                            iAPControl = lAPs(0).APControl
                            sOrderInfo = "Order No.: " & lAPs(0).BookCarrOrderNumber & ", Ship ID: " & If(String.IsNullOrWhiteSpace(lAPs(0).BookSHID), lAPs(0).BookConsPrefix, lAPs(0).BookSHID) & ", using AP Control Number: " & iAPControl.ToString()

                            Try
                                'read the AP data 
                                oSendAP.dynamicsTMSAPs = lAPs.ToArray()
                                blnReadyToSend = True
                            Catch ex As Exception
                                iErrors += 1
                                LogError(Me.Source & " AP Export Error ", " Unable to generate the NAV Purchase Worksheet data. There is a problem with the TMS data for " & sOrderInfo & ".  Please check the record and contact NGL Support.", Me.AdminEmail, ex)
                            End Try
                            Dim sMsg As String
                            Dim blnSuccess As Boolean = False
                            If blnReadyToSend Then
                                'Do ' Modified by RHR for v-8.4.0.004 on 02/10/2022 removed Do loop
                                Try
                                    blnSuccess = oBCIntegration.sendAPs(getAuthsSettngs(TMSSetting), oSendAP, sMsg)
                                    If blnSuccess Then
                                        blnConfirmAP = True
                                        'success do not retry
                                        iRetryCt = iTMSAPRetryAttempts + 1
                                        blnRetryNoGL = False 'default is true must be set to false on success
                                    Else
                                        Log(String.Format("{0} Send Purchase Invoice Update failed on retry attempt # {1}, {2} Unable to export the Purchase Invoice data for {3}. {2} Due to the following server error: {2} {4}", Me.Source, iFailedAttempts, vbCrLf, sOrderInfo, sMsg))
                                    End If

                                Catch ex As Exception
                                    iFailedAttempts += 1
                                    eFailed = ex
                                    Log(String.Format("{0} Send Purchase Invoice Update failed on retry attempt # {1}, {2} Unable to export the Purchase Invoice data for {3}. {2} Due to the following error: {2} {4}", Me.Source, iFailedAttempts, vbCrLf, sOrderInfo, ex.ToString()))
                                    System.Threading.Thread.Sleep(iTMSAPRetryMilliSeconds)
                                    ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                                    ' we wait until the retry to log errors
                                    'LogError(Me.Source & " Picklist Status Update Error", Me.Source & " Unable to export the Picklist Status Update Data for Order Number: " & sOrderNumber, Me.AdminEmail, ex)
                                End Try
                                iRetryCt += 1
                                ' Loop While iRetryCt <= iTMSAPRetryAttempts 'Modified by RHR for v-8.4.0.004 on 02/05/2022
                                'If blnRetryNoGL Then 'Modified by RHR for v-8.4.0.004 on 02/05/2022
                                '    Try
                                '        oNAVWebService.SendAPNoPost(oNavAPs)
                                '        blnConfirmAP = True
                                '        If eFailed Is Nothing Then
                                '            eFailed = New System.ApplicationException("TMS cannot update the G/L. A record may be locked by an external batch process")
                                '        End If
                                '        LogError(Me.Source & "Update Purchase Invoice Failure", "The system failed to update the G/L " & iFailedAttempts.ToString() & " times without success.  Please open the Purchase Worksheet and generate the Purchase Invoice manually for " & sOrderInfo, Me.AdminEmail, eFailed)

                                '    Catch ex As Exception
                                '        iErrors += 1
                                '        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                                '        LogError(Me.Source & " Post to Purchase Invoice Worksheet Failure", " TMS was unable to export the Purchase Invoice data for " & sOrderInfo & ".  Please follow the manual re-send procedure.", Me.AdminEmail, ex)
                                '    End Try
                                'End If
                                If blnConfirmAP Then
                                    Try
                                        ReturnMessage = ""
                                        apExport.ConfirmAPExport70(TMSSetting.TMSAuthCode, iAPControl, ReturnMessage)
                                    Catch ex As Exception
                                        iErrors += 1
                                        LogError(Me.Source & " Update Ap Export Confirmation Error", "TMS was unable to update the TMS AP export confirmation flag for " & sOrderInfo & ". A duplicate Purchse Invoice Worksheet post may be created if Retry is enabled.  Please check the data manually. ", Me.AdminEmail, ex)
                                    End Try
                                    blnRet = True
                                End If
                            End If
                        Else
                            If Verbose Then Log("No Data Found for Ap Export Array")
                            blnRet = True
                        End If

                    End If
                Next
                Log("AP Export processed " & iProcessed.ToString() & " records and found " & iErrors.ToString() & " errors or warning.")
            Else
                If Verbose Then Log("No AP Export Updates to Process")
                blnRet = True
            End If

            If Debug Then Log("Process APExport Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected APExport Integration Error", Source & " Unexpected Integration Error! Could not import APExport information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function


    ''' <summary>
    ''' Read TMS AP Data and send to NAV Purchase Invoice Worksheet web service
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the TMS Web Service with 80 version
    '''   includes support for BookItemOrderNumber 
    ''' Modified by RHR for v-8.4.0.002 on 03/30/2021
    '''     Added four new rules.
    '''     1. if no items do not send
    '''     2. If an item has "Comment" as the item number do not send
    '''     3. Only send one record at a time.  Email and log any errors
    '''     4. Check for items with zero freight cost, if exists skip and wait for next round 
    '''   Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support record locks in ERP
    '''   Modified by RHR for v-8.4.0.004 on 02/10/2022 removed support for  multiple retry did not fix record locks in ERP
    '''     now we send one freight bill at a time.  log any errors and notify users to fix it manually.  On error we mark the record as 
    '''     confirmed but do not continue to the next record.  The next AP export record will run on the next cycle.
    ''' </remarks>
    Private Function processAPExportData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for APExport; nothing to do returning false")
            Return False
        End If
        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processAPExportData2016(TMSSetting)
        End If

        If TMSSetting.ERPTypeControl = 4 Then
            Return processAPExportDataBC20(TMSSetting)
        End If

        Try

            If Debug Then Log("Begin Process APExport Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim apExport As New TMSIntegrationServices.DTMSERPIntegration()
            apExport.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                apExport.UseDefaultCredentials = True
            Else
                apExport.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            'Modified by RHR for v-8.4.0.004 on 02/10/2022 MaxRowsReturned for AP is now 1 we do not use TMSSetting.ERPExportMaxRowsReturned
            Dim iMaxRows As Integer = 1
            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, iMaxRows, TMSSetting.ERPExportAutoConfirmation)
            Dim oAPExportData As TMSIntegrationServices.clsAPExportData80 = apExport.GetAPData80(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, iMaxRows, TMSSetting.ERPExportAutoConfirmation, RetVal, ReturnMessage)
            LastError = ReturnMessage
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export APExport information:  " & LastError)
                        Return False
                    Case Else
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, RetVal, IntegrationModule.APExport, LastError)
                        Return False
                End Select
            End If
            If oAPExportData Is Nothing Then Return True

            If Not oAPExportData.Headers Is Nothing AndAlso oAPExportData.Headers.Count() > 0 Then
                If Me.Verbose Then Log("Processing " & oAPExportData.Headers.Count().ToString() & " AP Header Records.")
                'We only process the dynamics tms web services when UnitTestKeys are not provided
                ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
                If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
                Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
                oNAVWebService.Url = TMSSetting.ERPURI
                If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                    oNAVWebService.UseDefaultCredentials = True
                Else
                    oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
                End If
                Dim oNavAPs = New NAVService.DynamicsTMSAP

                Dim strSkip As New List(Of String)
                Dim strSkipLine As New List(Of String)
                '  Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 3. Only send one record at a time.  Email and log any errors we count the number or reords processed and the number of errors
                Dim iProcessed As Integer = oAPExportData.Headers.Count()
                Dim iErrors As Integer = 0
                Dim sErrorMsg As String = ""
                For Each c In oAPExportData.Headers
                    ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time. 
                    '  we moved the definition of lists inside the For Loop so each transaction is for one AP Control record
                    Dim lAPs As New List(Of NAVService.AP)
                    If Not c Is Nothing AndAlso c.APControl <> 0 Then
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 2. If an item has "Comment" as the item number do Not send
                        Dim d As TMSIntegrationServices.clsAPExportDetailObject80() = oAPExportData.Details.Where(Function(x) x.APControl = c.APControl).Where(Function(y) Not String.IsNullOrWhiteSpace(y.ItemNumber) AndAlso y.ItemNumber.Trim.ToLower <> "comment").ToArray()
                        If Not d Is Nothing AndAlso d.Count() > 0 Then
                            ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 4. Check for items with zero freight cost, if exists skip and wait for next round 
                            If (d.Any(Function(x) x.FreightCost <= 0)) Then
                                iErrors += 1
                                sErrorMsg = "Skip AP Export Record,  No freight Cost for one or more items for Order No.: " & c.BookCarrOrderNumber & ", Ship ID: " & If(String.IsNullOrWhiteSpace(c.BookSHID), c.BookConsPrefix, c.BookSHID)
                                LogError(Me.Source & " Export AP Information Error", Me.Source & sErrorMsg, Me.AdminEmail)
                                Continue For 'skip the code below and go back to the top with the next each of c
                            End If
                            Dim oAP = New NAVService.AP()
                            CopyMatchingFieldsImplicitCast(oAP, c, strSkip, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            Dim oDetails As New List(Of NAVService.Details)
                            For Each i In d
                                Dim oDetail As New NAVService.Details()
                                CopyMatchingFieldsImplicitCast(oDetail, i, strSkipLine, strMsg)
                                If Not String.IsNullOrWhiteSpace(strMsg) Then
                                    If Debug Then Log(strMsg)
                                    strMsg = ""
                                End If
                                oDetails.Add(oDetail)
                            Next
                            If Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                                oAP.Details = oDetails.ToArray()
                            End If
                            lAPs.Add(oAP)
                        Else
                            ' Rule 1. If no items do not send, already existed prior to v-8.4.0.002
                            iErrors += 1
                            Dim sSubject = "Process " & TMSSetting.ERPTypeName & " AP Export Failure Manual Processing Required"
                            Dim sWarning As String = "Item Details are required. At least one Item, with a NAV Item Number, is required to Process AP Data using the NAV Integration module.  You must correct the issue and resend or enter this freight bill manually into NAV!"
                            Dim sBody As String = String.Format("Warning: {1} {0}Details: {0}Comp No: {2}{0}
                                                                 Location Code: {3}{0}
                                                                 Carrier No: {4}{0},
                                                                 Carrier Vendor: {5}{0},
                                                                 OrderNumber: {6}{0},
                                                                 Freight Bill No: {7}{0}",
                                                                vbCrLf,
                                                                sWarning,
                                                                c.CompanyNumber,
                                                                c.CompAlphaCode,
                                                                c.CarrierNumber,
                                                                c.CarrierAlphaCode,
                                                                c.BookCarrOrderNumber,
                                                                c.BookFinAPBillNumber)

                            createERPInegrationFailureSubscriptionAlert(Subject:=sSubject,
                                                                        enmIntegration:=IntegrationModule.APExport,
                                                                        CompControl:=0,
                                                                        CompNumber:=c.CompanyNumber,
                                                                        CompAlphaCode:=c.CompAlphaCode,
                                                                        CarrierNumber:=c.CarrierNumber,
                                                                        CarrierAlphaCode:=c.CarrierAlphaCode,
                                                                        OrderNumber:=c.BookCarrOrderNumber,
                                                                        OrderSequence:=c.BookOrderSequence.ToString(),
                                                                        keyControl:=c.APControl,
                                                                        keyString:=c.BookFinAPBillNumber,
                                                                        Warnings:="Item Details are required. At least one Item, with an NAV Item Number, is required to Process AP Data using the NAV Integration module.  You must enter this freight bill manually into NAV!")
                            LogError(Me.Source & sSubject, sBody, Me.AdminEmail)

                            'we must mark the record as exported because it will keep trying to resend
                            apExport.ConfirmAPExport70(TMSSetting.TMSAuthCode, c.APControl, ReturnMessage)
                            Continue For
                        End If
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                        '  we moved the code below that sends to NAV inside the loop so each transaction is for one AP Control record
                        Dim sOrderInfo As String = "Unavailable"
                        Dim iAPControl As Long
                        'Modified by RHR for v-8.4.0.003 on 10/19/2021 added support multiple retry to support GL record locks in ERP
                        'Modified by RHR for v-8.4.0.004 on 02/05/2022 removed support multiple retry to support GL record locks in ERP because this creats duplicate records.
                        '  Users must resend manually on failure
                        If Not lAPs Is Nothing AndAlso lAPs.Count > 0 Then
                            Dim iRetryCt As Integer = 0
                            Dim blnRetryNoGL As Boolean = True 'default is true must be set to false on success
                            Dim blnConfirmAP As Boolean = False
                            Dim blnReadyToSend As Boolean = False
                            Dim iFailedAttempts As Integer = 0
                            Dim eFailed As Exception = Nothing
                            iAPControl = lAPs(0).APControl
                            sOrderInfo = "Order No.: " & lAPs(0).BookCarrOrderNumber & ", Ship ID: " & If(String.IsNullOrWhiteSpace(lAPs(0).BookSHID), lAPs(0).BookConsPrefix, lAPs(0).BookSHID) & ", using AP Control Number: " & iAPControl.ToString()

                            Try
                                'read the AP data 
                                oNavAPs.AP = lAPs.ToArray()
                                blnReadyToSend = True
                            Catch ex As Exception
                                iErrors += 1
                                LogError(Me.Source & " AP Export Error ", " Unable to generate the NAV Purchase Worksheet data. There is a problem with the TMS data for " & sOrderInfo & ".  Please check the record and contact NGL Support.", Me.AdminEmail, ex)
                            End Try
                            If blnReadyToSend Then
                                'Do ' Modified by RHR for v-8.4.0.004 on 02/10/2022 removed Do loop
                                Try
                                    oNAVWebService.SendAP(oNavAPs)
                                    blnConfirmAP = True
                                    'success do not retry
                                    iRetryCt = iTMSAPRetryAttempts + 1
                                    blnRetryNoGL = False 'default is true must be set to false on success
                                Catch ex As Exception
                                    iFailedAttempts += 1
                                    eFailed = ex
                                    Log(String.Format("{0} Send Purchase Invoice Update failed on retry attempt # {1}, {2} Unable to export the Purchase Invoice data for {3}. {2} Due to the following error: {2} {4}", Me.Source, iFailedAttempts, vbCrLf, sOrderInfo, ex.ToString()))
                                    System.Threading.Thread.Sleep(iTMSAPRetryMilliSeconds)
                                    ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                                    ' we wait until the retry to log errors
                                    'LogError(Me.Source & " Picklist Status Update Error", Me.Source & " Unable to export the Picklist Status Update Data for Order Number: " & sOrderNumber, Me.AdminEmail, ex)
                                End Try
                                iRetryCt += 1
                                ' Loop While iRetryCt <= iTMSAPRetryAttempts 'Modified by RHR for v-8.4.0.004 on 02/05/2022
                                'If blnRetryNoGL Then 'Modified by RHR for v-8.4.0.004 on 02/05/2022
                                '    Try
                                '        oNAVWebService.SendAPNoPost(oNavAPs)
                                '        blnConfirmAP = True
                                '        If eFailed Is Nothing Then
                                '            eFailed = New System.ApplicationException("TMS cannot update the G/L. A record may be locked by an external batch process")
                                '        End If
                                '        LogError(Me.Source & "Update Purchase Invoice Failure", "The system failed to update the G/L " & iFailedAttempts.ToString() & " times without success.  Please open the Purchase Worksheet and generate the Purchase Invoice manually for " & sOrderInfo, Me.AdminEmail, eFailed)

                                '    Catch ex As Exception
                                '        iErrors += 1
                                '        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                                '        LogError(Me.Source & " Post to Purchase Invoice Worksheet Failure", " TMS was unable to export the Purchase Invoice data for " & sOrderInfo & ".  Please follow the manual re-send procedure.", Me.AdminEmail, ex)
                                '    End Try
                                'End If
                                If blnConfirmAP Then
                                    Try
                                        ReturnMessage = ""
                                        apExport.ConfirmAPExport70(TMSSetting.TMSAuthCode, iAPControl, ReturnMessage)
                                    Catch ex As Exception
                                        iErrors += 1
                                        LogError(Me.Source & " Update Ap Export Confirmation Error", "TMS was unable to update the TMS AP export confirmation flag for " & sOrderInfo & ". A duplicate Purchse Invoice Worksheet post may be created if Retry is enabled.  Please check the data manually. ", Me.AdminEmail, ex)
                                    End Try
                                    blnRet = True
                                End If
                            End If
                        Else
                            If Verbose Then Log("No Data Found for Ap Export Array")
                            blnRet = True
                        End If

                    End If
                Next
                Log("AP Export processed " & iProcessed.ToString() & " records and found " & iErrors.ToString() & " errors or warning.")
            Else
                If Verbose Then Log("No AP Export Updates to Process")
                blnRet = True
            End If

            If Debug Then Log("Process APExport Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected APExport Integration Error", Source & " Unexpected Integration Error! Could not import APExport information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Public Function processAPExportDataDebug() As Boolean



        Dim strMsg As String = ""
        Dim ReturnMessage As String = ""
        Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure

        Dim ap As New clsAPExportData80()
        Dim apExport As New NGL.FreightMaster.Integration.clsAPExport
        Dim Headers() As NGL.FreightMaster.Integration.clsAPExportObject80
        Dim Details() As NGL.FreightMaster.Integration.clsAPExportDetailObject80
        Dim Fees() As NGL.FreightMaster.Integration.clsAPExportFeeObject80

        Dim sSource As String = "DTMSERPIntegration.GetAPData80"
        Dim sDataType As String = "AP"
        Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", 1, 1, 100, False)




        With apExport
            .AdminEmail = "rramsey@ngl.net"
            .FromEmail = "rramsey@ngl.net"
            .GroupEmail = "rramsey@ngl.net"
            .Retry = 10
            .SMTPServer = "SmtpMailServer"
            .DBServer = "DESKTOP-0R0EJUB"
            .Database = "NGLMASDEV"
            .ConnectionString = "Data Source=DESKTOP-0R0EJUB;Initial Catalog=NGLMASDEV;User ID=nglweb;Password=5529;"
            .AuthorizationCode = "NGLWCFDEV"
            .Debug = True
            .WCFAuthCode = "NGLWCFDEV"
            .WCFURL = ""
            .WCFTCPURL = ""
        End With
        With apExport
            .MaxRowsReturned = 10
            .AutoConfirmation = False
        End With
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
        RetVal = apExport.readObjectData80(Headers, "Data Source=DESKTOP-0R0EJUB;Initial Catalog=NGLMASDEV;User ID=nglweb;Password=5529;", 10, 1, "Morristown", Fees, Details)
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
        If Not Headers Is Nothing AndAlso Headers.Count() > 0 Then


            Dim strSkip As New List(Of String)
            Dim strSkipLine As New List(Of String)
            '  Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 3. Only send one record at a time.  Email and log any errors we count the number or reords processed and the number of errors
            Dim iProcessed As Integer = Headers.Count()
            Dim iErrors As Integer = 0

            For Each c In Headers
                ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time. 
                '  we moved the definition of lists inside the For Loop so each transaction is for one AP Control record

                If Not c Is Nothing AndAlso c.APControl <> 0 Then
                    ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 2. If an item hase "Comment" as the item number do Not send
                    Dim d = Details.Where(Function(x) x.APControl = c.APControl).Where(Function(y) Not String.IsNullOrWhiteSpace(y.ItemNumber) AndAlso y.ItemNumber.Trim.ToLower <> "comment").ToArray()
                    If Not d Is Nothing AndAlso d.Count() > 0 Then
                        ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Rule 4. Check for items with zero freight cost, if exists skip and wait for next round 
                        If (d.Any(Function(x) x.FreightCost <= 0)) Then
                            iErrors += 1
                            If Me.Debug Then Log("Skip AP Export,  No freight Cost for one or more items for Order No.: " & c.BookCarrOrderNumber)
                            Continue For 'skip the code below and go back to the top with the next each of c
                        End If

                    Else
                        ' Rule 1. If no items do not send, already existed prior to v-8.4.0.002
                        iErrors += 1
                        Dim sSubject = "Process  AP Export Failure Manual Processing Required"
                        Dim sWarning As String = "Item Details are required. At least one Item, with a NAV Item Number, is required to Process AP Data using the NAV Integration module.  You must correct the issue and resend or enter this freight bill manually into NAV!"
                        Dim sBody As String = String.Format("Warning: {1} {0}Details: {0}Comp No: {2}{0}
                                                                 Location Code: {3}{0}
                                                                 Carrier No: {4}{0},
                                                                 Carrier Vendor: {5}{0},
                                                                 OrderNumber: {6}{0},
                                                                 Freight Bill No: {7}{0}",
                                                                vbCrLf,
                                                                sWarning,
                                                                c.CompanyNumber,
                                                                c.CompAlphaCode,
                                                                c.CarrierNumber,
                                                                c.CarrierAlphaCode,
                                                                c.BookCarrOrderNumber,
                                                                c.BookFinAPBillNumber)

                        LogError(Me.Source & sSubject, sBody, Me.AdminEmail)

                        Continue For
                    End If
                    ' Modified by RHR for v-8.4.0.002 on 03/30/2021 Only send one record at a time.  Email and log any errors
                End If
            Next

        End If


        Return True
    End Function

    Private Function processAPExportData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for APExport; nothing to do returning false")
            Return False
        End If
        Try

            If Debug Then Log("Begin Process APExport Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim apExport As New TMSIntegrationServices.DTMSERPIntegration()
            apExport.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                apExport.UseDefaultCredentials = True
            Else
                apExport.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation)
            Dim oAPExportData As TMSIntegrationServices.clsAPExportData80 = apExport.GetAPData80(TMSSetting.TMSAuthCode, TMSSetting.ERPExportMaxRetry, TMSSetting.ERPExportRetryMinutes, TMSSetting.LegalEntity, TMSSetting.ERPExportMaxRowsReturned, TMSSetting.ERPExportAutoConfirmation, RetVal, ReturnMessage)
            LastError = ReturnMessage
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export APExport information:  " & LastError)
                        Return False
                    Case Else
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, RetVal, IntegrationModule.APExport, LastError)
                        Return False
                End Select
            End If
            If oAPExportData Is Nothing Then Return True

            If Not oAPExportData.Headers Is Nothing AndAlso oAPExportData.Headers.Count() > 0 Then
                'We only process the dynamics tms web services when UnitTestKeys are not provided
                Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
                oNAVWebService.Url = TMSSetting.ERPURI
                If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                    oNAVWebService.UseDefaultCredentials = True
                Else
                    oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
                End If
                Dim oNavAPs = New NAV2016Services.DynamicsTMSAP

                Dim strSkip As New List(Of String)
                Dim strSkipLine As New List(Of String)
                If (TMSSetting.ERPSettingVersion < "8.2") Then
                    strSkip.Add("BookItemOrderNumber")
                    strSkipLine.Add("BookItemOrderNumber")
                End If
                Dim lAPs As New List(Of NAV2016Services.AP)
                For Each c In oAPExportData.Headers
                    If Not c Is Nothing AndAlso c.APControl <> 0 Then
                        Dim d As TMSIntegrationServices.clsAPExportDetailObject80() = oAPExportData.Details.Where(Function(x) x.APControl = c.APControl).Where(Function(y) Not String.IsNullOrWhiteSpace(y.ItemNumber)).ToArray()
                        If Not d Is Nothing AndAlso d.Count() > 0 Then
                            Dim oAP = New NAV2016Services.AP()
                            CopyMatchingFieldsImplicitCast(oAP, c, strSkip, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                If Debug Then Log(strMsg)
                                strMsg = ""
                            End If
                            Dim oDetails As New List(Of NAV2016Services.Details)
                            For Each i In d
                                Dim oDetail As New NAV2016Services.Details()
                                CopyMatchingFieldsImplicitCast(oDetail, i, strSkipLine, strMsg)
                                If Not String.IsNullOrWhiteSpace(strMsg) Then
                                    If Debug Then Log(strMsg)
                                    strMsg = ""
                                End If
                                oDetails.Add(oDetail)
                            Next
                            If Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                                oAP.Details = oDetails.ToArray()
                            End If
                            lAPs.Add(oAP)
                        Else
                            Dim sSubject = "Process " & TMSSetting.ERPTypeName & " AP Export Failure Manual Processing Required"
                            createERPInegrationFailureSubscriptionAlert(Subject:=sSubject,
                                                                        enmIntegration:=IntegrationModule.APExport,
                                                                        CompControl:=0,
                                                                        CompNumber:=c.CompanyNumber,
                                                                        CompAlphaCode:=c.CompAlphaCode,
                                                                        CarrierNumber:=c.CarrierNumber,
                                                                        CarrierAlphaCode:=c.CarrierAlphaCode,
                                                                        OrderNumber:=c.BookCarrOrderNumber,
                                                                        OrderSequence:=c.BookOrderSequence.ToString(),
                                                                        keyControl:=c.APControl,
                                                                        keyString:=c.BookFinAPBillNumber,
                                                                        Warnings:="Item Details are required. At least one Item, with an NAV Item Number, is required to Process AP Data using the NAV Integration module.  You must enter this freight bill manually into NAV!")
                            'we must mark the record as exported because it will keep trying to resend
                            apExport.ConfirmAPExport70(TMSSetting.TMSAuthCode, c.APControl, ReturnMessage)
                        End If
                    End If
                Next
                If Not lAPs Is Nothing AndAlso lAPs.Count > 0 Then
                    oNavAPs.AP = lAPs.ToArray()
                    oNAVWebService.SendAP(oNavAPs)
                    For Each a In lAPs
                        Try
                            apExport.ConfirmAPExport70(TMSSetting.TMSAuthCode, a.APControl, ReturnMessage)
                        Catch ex As Exception
                            LogError(Me.Source & " Update AP Export Confirmation Error", Me.Source & " Update AP Export confirmation failed for Order Number: " & a.BookCarrOrderNumber, Me.AdminEmail, ex)
                        End Try
                    Next
                    blnRet = True
                Else
                    If Verbose Then Log("APExport NAV Payment Array is Empty")
                    blnRet = True
                End If
            Else
                If Verbose Then Log("No AP Export Updates to Process")
                blnRet = True
            End If

            If Debug Then Log("Process APExport Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected APExport Integration Error", Source & " Unexpected Integration Error! Could not import APExport information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' Unit Test for TMS AP Data 
    ''' </summary>
    ''' <param name="UnitTestKeys"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the TMS interface with 80 version
    '''   includes support for BookItemOrderNumber 
    ''' </remarks>
    Private Function processAPExportUnitTest(ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = False
        Try

            Log("Begin Process APExport Data ")
            Dim strMsg As String = ""
            Dim apExport As New TMS.clsAPExport
            populateIntegrationObjectParameters(apExport, UnitTestKeys)
            Dim Headers() As TMS.clsAPExportObject80
            Dim Details() As TMS.clsAPExportDetailObject80
            Dim Fees() As TMS.clsAPExportFeeObject80

            'set the default value to false
            Dim RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim intMaxRetry As Integer = 0
            Dim intRetryMinutes As Integer = 30
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                intMaxRetry = UnitTestKeys.PicklistMaxRetry
                intRetryMinutes = UnitTestKeys.PicklistRetryMinutes
                apExport.MaxRowsReturned = UnitTestKeys.PicklistMaxRowsReturned
                apExport.AutoConfirmation = UnitTestKeys.PicklistAutoConfirmation
            End If

            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", intMaxRetry, intRetryMinutes, apExport.MaxRowsReturned, apExport.AutoConfirmation)

#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = apExport.readObjectData80(Headers, Me.ConnectionString, intMaxRetry, intRetryMinutes, LegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            LastError = apExport.LastError
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not export APExport information:  " & LastError)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("APExport Integration Error", "Error Integration Failure! could not export APExport information:  " & LastError, AdminEmail)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        LogError("APExport Integration Error", "Error Integration Had Errors! could not export some APExport information:  " & LastError, AdminEmail)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        LogError("APExport Integration Error", "Error Data Validation Failure! could not export APExport information:  " & LastError, AdminEmail)
                        Return False
                End Select
            End If
            If Not Headers Is Nothing AndAlso Headers.Count > 0 Then
                For Each a In Headers
                    Try
                        apExport.confirmExportEx(Me.ConnectionString, a.APControl)
                    Catch ex As Exception
                        LogError(Me.Source & " Update AP Export Confirmation Error", Me.Source & " Update AP Export confirmation failed for Order Number: " & a.BookCarrOrderNumber, Me.AdminEmail, ex)
                    End Try
                Next
            Else
                Log("No AP Export Updates to Process")
            End If
            blnRet = True
            Log("Process APExport Data Complete")
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected APExport Integration Error", Source & " Unexpected Integration Error! Could not import APExport information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processPayablesUnitTest(ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = True
        Try
            Log("Begin Process Payables Data ")
            Dim strMsg As String = ""
            Dim oPayablesIntegration As New TMS.clsPayables
            populateIntegrationObjectParameters(oPayablesIntegration, UnitTestKeys)
            Dim oHeaders As New List(Of TMS.clsPayablesObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                Log("Running unit test with sample data")
                oHeaders.Add(TMS.clsPayablesObject70.GenerateSampleObject(UnitTestKeys.FreightCost, UnitTestKeys.FreightBillNumber, UnitTestKeys.OrderNumber, UnitTestKeys.CompNumber, UnitTestKeys.CompAlphaCode, UnitTestKeys.LegalEntity))
            End If

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim oResults As TMS.Configuration.ProcessDataReturnValues = oPayablesIntegration.ProcessObjectData70(oHeaders, Me.ConnectionString)
                Dim sLastError As String = oPayablesIntegration.LastError
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Payable information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Error Integration Failure! could not import Payable information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Payable Integration Had Errors", Source & " Warning!  Could not import some Payable information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Payable information:  " & sLastError)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Payable Integration Had Errors", Source & " Error Data Validation Failure! could not import Payable information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Payable information:  " & sLastError)
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        Log("Success! The payable information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Payables to Process")
                blnRet = False
            End If

            Log("Process Payables Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Payables Integration Error", Source & " Unexpected Integration Error! Could not import Payables information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function


    Private Function processPayablesDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = True
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Payables; nothing to do returning false")
            Return False
        End If

        Try
            If Debug Then Log("Begin Process Payables Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oPayablesIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oPayablesIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oPayablesIntegration.UseDefaultCredentials = True
            Else
                oPayablesIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsPayablesObject70)
            System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            Dim oBCIntegration = New BCIntegration.clsApplication()
            Dim oPayables As Payable.Envelope = oBCIntegration.getPayables(getAuthsSettngs(TMSSetting), ERPTestingOn, Not ERPTestingOn)

            Dim strSkip As New List(Of String)
            If (oPayables Is Nothing OrElse oPayables.Body Is Nothing OrElse oPayables.Body.GetPayables_Result Is Nothing OrElse oPayables.Body.GetPayables_Result.dynamicsTMSPayables Is Nothing OrElse oPayables.Body.GetPayables_Result.dynamicsTMSPayables.Length < 1) Then
                If Debug Then Log("Waiting on Payables settlement Data")
                Return True 'payables are not ready yet so just return true
            End If
            For Each p In oPayables.Body.GetPayables_Result.dynamicsTMSPayables
                If Not p Is Nothing Then
                    ''***************************************************************
                    ''code added to correct bug in NGL for September Release
                    ''must be removed after bug is fixed.
                    'Try
                    '    If String.IsNullOrWhiteSpace(p.BookFinAPBillInvDate) Then
                    '        p.BookFinAPBillInvDate = p.BookFinAPBillInvDate1
                    '    End If

                    'Catch ex As Exception
                    '    'do nothing just ignore any error here
                    'End Try
                    ''****************************************************************
                    If Not String.IsNullOrWhiteSpace(p.BookCarrOrderNumber) Then
                        Dim oTMSPayable = New TMSIntegrationServices.clsPayablesObject70
                        CopyMatchingFieldsImplicitCast(oTMSPayable, p, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oHeaders.Add(oTMSPayable)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Payable record could not be processed because the record had an invalid Order Number value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aPayableHeaders As TMSIntegrationServices.clsPayablesObject70() = oHeaders.ToArray()
                Dim oResults = oPayablesIntegration.ProcessPayableData70(TMSSetting.TMSAuthCode, aPayableHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Payable information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The payable information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Payables to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Payables Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Payables Integration Error", Source & " Unexpected Integration Error! Could not import Payables information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processPayablesData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = True
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Payables; nothing to do returning false")
            Return False
        End If

        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processPayablesData2016(TMSSetting)
        End If

        If TMSSetting.ERPTypeControl = 4 Then
            Return processPayablesDataBC20(TMSSetting)
        End If
        Try
            If Debug Then Log("Begin Process Payables Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oPayablesIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oPayablesIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oPayablesIntegration.UseDefaultCredentials = True
            Else
                oPayablesIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsPayablesObject70)
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavPayables = New NAVService.DynamicsTMSPayments()
            oNAVWebService.GetPayables(oNavPayables, ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            If oNavPayables Is Nothing OrElse oNavPayables.Payment Is Nothing OrElse oNavPayables.Payment.Count() < 1 Then
                If Debug Then Log("Waiting on Payables settlement Data")
                Return True 'payables are not ready yet so just return true
            End If
            For Each p In oNavPayables.Payment
                If Not p Is Nothing Then
                    ''***************************************************************
                    ''code added to correct bug in NGL for September Release
                    ''must be removed after bug is fixed.
                    'Try
                    '    If String.IsNullOrWhiteSpace(p.BookFinAPBillInvDate) Then
                    '        p.BookFinAPBillInvDate = p.BookFinAPBillInvDate1
                    '    End If

                    'Catch ex As Exception
                    '    'do nothing just ignore any error here
                    'End Try
                    ''****************************************************************
                    If Not String.IsNullOrWhiteSpace(p.BookCarrOrderNumber) Then
                        Dim oTMSPayable = New TMSIntegrationServices.clsPayablesObject70
                        CopyMatchingFieldsImplicitCast(oTMSPayable, p, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oHeaders.Add(oTMSPayable)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Payable record could not be processed because the record had an invalid Order Number value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aPayableHeaders As TMSIntegrationServices.clsPayablesObject70() = oHeaders.ToArray()
                Dim oResults = oPayablesIntegration.ProcessPayableData70(TMSSetting.TMSAuthCode, aPayableHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Payable information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The payable information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Payables to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Payables Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Payables Integration Error", Source & " Unexpected Integration Error! Could not import Payables information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processPayablesData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = True
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Payables; nothing to do returning false")
            Return False
        End If
        Try
            If Debug Then Log("Begin Process Payables Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oPayablesIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oPayablesIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oPayablesIntegration.UseDefaultCredentials = True
            Else
                oPayablesIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsPayablesObject70)
            Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavPayables = New NAV2016Services.DynamicsTMSPayments()
            oNAVWebService.GetPayables(oNavPayables, ERPTestingOn, Not ERPTestingOn)
            Dim strSkip As New List(Of String)
            If oNavPayables Is Nothing OrElse oNavPayables.Payment Is Nothing OrElse oNavPayables.Payment.Count() < 1 Then
                If Debug Then Log("Waiting on Payables settlement Data")
                Return True 'payables are not ready yet so just return true
            End If
            For Each p In oNavPayables.Payment
                If Not p Is Nothing Then
                    ''***************************************************************
                    ''code added to correct bug in NGL for September Release
                    ''must be removed after bug is fixed.
                    'Try
                    '    If String.IsNullOrWhiteSpace(p.BookFinAPBillInvDate) Then
                    '        p.BookFinAPBillInvDate = p.BookFinAPBillInvDate1
                    '    End If

                    'Catch ex As Exception
                    '    'do nothing just ignore any error here
                    'End Try
                    ''****************************************************************
                    If Not String.IsNullOrWhiteSpace(p.BookCarrOrderNumber) Then
                        Dim oTMSPayable = New TMSIntegrationServices.clsPayablesObject70
                        CopyMatchingFieldsImplicitCast(oTMSPayable, p, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oHeaders.Add(oTMSPayable)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Payable record could not be processed because the record had an invalid Order Number value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aPayableHeaders As TMSIntegrationServices.clsPayablesObject70() = oHeaders.ToArray()
                Dim oResults = oPayablesIntegration.ProcessPayableData70(TMSSetting.TMSAuthCode, aPayableHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Payable information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Payables, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The payable information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Payables to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Payables Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Payables Integration Error", Source & " Unexpected Integration Error! Could not import Payables information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processPalletTypeUnitTest(ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = False
        Try
            Log("Begin Process Pallet Type Data ")
            Dim strMsg As String = ""
            Dim oPalletTypeIntegration As New TMS.clsPalletType
            populateIntegrationObjectParameters(oPalletTypeIntegration, UnitTestKeys)
            Dim oHeaders As New List(Of TMS.clsPalletTypeObject)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                Log("Running unit test with sample data")
                oHeaders.Add(TMS.clsPalletTypeObject.GenerateSampleObject())
            End If

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim oResults As TMS.Configuration.ProcessDataReturnValues = oPalletTypeIntegration.ProcessObjectData(oHeaders, Me.ConnectionString)
                Dim sLastError As String = oPalletTypeIntegration.LastError
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Pallet Type information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Error Integration Failure! could not import Pallet Type information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Pallet Type Integration Had Errors", Source & " Warning!  Could not import some Pallet Type information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Pallet Type information:  " & sLastError)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Pallet Type Integration Had Errors", Source & " Error Data Validation Failure! could not import Pallet Type information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Pallet Type information:  " & sLastError)
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        Log("Success! The Pallet Type information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Pallet Types to Process")
                blnRet = True
            End If

            Log("Process Pallet Type Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Pallet Type Integration Error", Source & " Unexpected Integration Error! Could not import Pallet Type information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processPalletTypeDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Pallet Type; nothing to do returning false")
            Return False
        End If

        Try
            If Debug Then Log("Begin Process Pallet Type Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oPalletTypeIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oPalletTypeIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oPalletTypeIntegration.UseDefaultCredentials = True
            Else
                oPalletTypeIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsPalletTypeObject)
            System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oBCIntegration = New BCIntegration.clsApplication()
            Dim oPalletTypes As Pallet.Envelope = oBCIntegration.getPalletTypes(getAuthsSettngs(TMSSetting))

            Dim strSkip As New List(Of String)
            If (oPalletTypes Is Nothing OrElse oPalletTypes.Body Is Nothing OrElse oPalletTypes.Body.GetPalletType_Result Is Nothing OrElse oPalletTypes.Body.GetPalletType_Result.dynamicsTMSPalletTypes Is Nothing OrElse oPalletTypes.Body.GetPalletType_Result.dynamicsTMSPalletTypes.Length < 1) Then
                If Debug Then Log("Waiting on Pallet Type Data")
                Return True 'not ready yet so just return true
            End If
            For Each p In oPalletTypes.Body.GetPalletType_Result.dynamicsTMSPalletTypes
                If Not p Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(p.PalletType1) Then
                        Dim oTMSPalletType = New TMSIntegrationServices.clsPalletTypeObject
                        CopyMatchingFieldsImplicitCast(oTMSPalletType, p, strSkip, strMsg)
                        oTMSPalletType.PalletType = p.PalletType1
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        'Modified by RHR for v-8.3.0.001 on 9/3/2020 added defaults for weight, lenght, width and depth
                        If oTMSPalletType.PalletTypeHeight <= 0 Then oTMSPalletType.PalletTypeHeight = 48
                        If oTMSPalletType.PalletTypeWidth <= 0 Then oTMSPalletType.PalletTypeWidth = 42
                        If oTMSPalletType.PalletTypeDepth <= 0 Then oTMSPalletType.PalletTypeDepth = 42
                        If oTMSPalletType.PalletTypeWeight <= 0 Then oTMSPalletType.PalletTypeWeight = 10
                        oHeaders.Add(oTMSPalletType)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Pallet Type record could not be processed because the record had an invalid Pallet Type value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aPalletTypeHeaders As TMSIntegrationServices.clsPalletTypeObject() = oHeaders.ToArray()
                Dim oResults = oPalletTypeIntegration.ProcessPalletTypeData70(TMSSetting.TMSAuthCode, aPalletTypeHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Pallet Type information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The Pallet Type information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Pallet Types to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Pallet Type Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Pallet Type Integration Error", Source & " Unexpected Integration Error! Could not import Pallet Type information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function


    ''' <summary>
    ''' Import Pallet Type data
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.001 on 9/3/2020 
    '''     added defaults for weight, lenght, width and depth
    ''' </remarks>
    Private Function processPalletTypeData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Pallet Type; nothing to do returning false")
            Return False
        End If

        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processPalletTypeData2016(TMSSetting)
        End If


        If TMSSetting.ERPTypeControl = 4 Then
            Return processPalletTypeDataBC20(TMSSetting)
        End If

        Try
            If Debug Then Log("Begin Process Pallet Type Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oPalletTypeIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oPalletTypeIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oPalletTypeIntegration.UseDefaultCredentials = True
            Else
                oPalletTypeIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsPalletTypeObject)
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavPalletTypes = New NAVService.DynamicsTMSPalletTypes()
            oNAVWebService.GetPalletType(oNavPalletTypes)
            Dim strSkip As New List(Of String)
            If oNavPalletTypes Is Nothing OrElse oNavPalletTypes.PalletType Is Nothing OrElse oNavPalletTypes.PalletType.Count() < 1 Then
                If Debug Then Log("Waiting on Pallet Type Data")
                Return True 'not ready yet so just return true
            End If
            For Each p In oNavPalletTypes.PalletType
                If Not p Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(p.PalletType1) Then
                        Dim oTMSPalletType = New TMSIntegrationServices.clsPalletTypeObject
                        CopyMatchingFieldsImplicitCast(oTMSPalletType, p, strSkip, strMsg)
                        oTMSPalletType.PalletType = p.PalletType1
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        'Modified by RHR for v-8.3.0.001 on 9/3/2020 added defaults for weight, lenght, width and depth
                        If oTMSPalletType.PalletTypeHeight <= 0 Then oTMSPalletType.PalletTypeHeight = 48
                        If oTMSPalletType.PalletTypeWidth <= 0 Then oTMSPalletType.PalletTypeWidth = 42
                        If oTMSPalletType.PalletTypeDepth <= 0 Then oTMSPalletType.PalletTypeDepth = 42
                        If oTMSPalletType.PalletTypeWeight <= 0 Then oTMSPalletType.PalletTypeWeight = 10
                        oHeaders.Add(oTMSPalletType)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Pallet Type record could not be processed because the record had an invalid Pallet Type value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aPalletTypeHeaders As TMSIntegrationServices.clsPalletTypeObject() = oHeaders.ToArray()
                Dim oResults = oPalletTypeIntegration.ProcessPalletTypeData70(TMSSetting.TMSAuthCode, aPalletTypeHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Pallet Type information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The Pallet Type information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Pallet Types to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Pallet Type Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Pallet Type Integration Error", Source & " Unexpected Integration Error! Could not import Pallet Type information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' pallet type logic for older versions of NAV Integration before 8.2.x
    ''' </summary>
    ''' <param name="TMSSetting"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Modified by RHR for v-8.3.0.001 on 9/3/2020 
    '''     added defaults for weight, lenght, width and depth
    ''' </remarks>
    Private Function processPalletTypeData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = False
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Pallet Type; nothing to do returning false")
            Return False
        End If
        Try
            If Debug Then Log("Begin Process Pallet Type Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oPalletTypeIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oPalletTypeIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oPalletTypeIntegration.UseDefaultCredentials = True
            Else
                oPalletTypeIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsPalletTypeObject)
            Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavPalletTypes = New NAV2016Services.DynamicsTMSPalletTypes()
            oNAVWebService.GetPalletType(oNavPalletTypes)
            Dim strSkip As New List(Of String)
            If oNavPalletTypes Is Nothing OrElse oNavPalletTypes.PalletType Is Nothing OrElse oNavPalletTypes.PalletType.Count() < 1 Then
                If Debug Then Log("Waiting on Pallet Type Data")
                Return True 'not ready yet so just return true
            End If
            For Each p In oNavPalletTypes.PalletType
                If Not p Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(p.PalletType1) Then
                        Dim oTMSPalletType = New TMSIntegrationServices.clsPalletTypeObject
                        CopyMatchingFieldsImplicitCast(oTMSPalletType, p, strSkip, strMsg)
                        oTMSPalletType.PalletType = p.PalletType1
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        'Modified by RHR for v-8.3.0.001 on 9/3/2020 added defaults for weight, lenght, width and depth
                        If oTMSPalletType.PalletTypeHeight <= 0 Then oTMSPalletType.PalletTypeHeight = 48
                        If oTMSPalletType.PalletTypeWidth <= 0 Then oTMSPalletType.PalletTypeWidth = 42
                        If oTMSPalletType.PalletTypeDepth <= 0 Then oTMSPalletType.PalletTypeDepth = 42
                        If oTMSPalletType.PalletTypeWeight <= 0 Then oTMSPalletType.PalletTypeWeight = 10
                        oHeaders.Add(oTMSPalletType)
                    Else
                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Pallet Type record could not be processed because the record had an invalid Pallet Type value.  This typically indicates that an empty record was being transmitted.")
                        End If
                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aPalletTypeHeaders As TMSIntegrationServices.clsPalletTypeObject() = oHeaders.ToArray()
                Dim oResults = oPalletTypeIntegration.ProcessPalletTypeData70(TMSSetting.TMSAuthCode, aPalletTypeHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Pallet Type information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.PalletType, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The Pallet Type information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Pallet Types to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Pallet Type Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Pallet Type Integration Error", Source & " Unexpected Integration Error! Could not import Pallet Type information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processHazmatUnitTest(ByVal UnitTestKeys As clsUnitTestKeys) As Boolean
        Dim blnRet As Boolean = False
        Try
            Log("Begin Process Hazmat Data ")
            Dim strMsg As String = ""
            Dim oHazmatIntegration As New TMS.clsHazmat
            populateIntegrationObjectParameters(oHazmatIntegration, UnitTestKeys)
            Dim oHeaders As New List(Of TMS.clsHazmatObject)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'If Unit Test Keys are provided and we have a Legal Entity then we are running a unit test
            If Not UnitTestKeys Is Nothing AndAlso Not String.IsNullOrWhiteSpace(UnitTestKeys.LegalEntity) Then
                Log("Running unit test with sample data")
                oHeaders.Add(TMS.clsHazmatObject.GenerateSampleObject())

            End If
            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim oResults As TMS.Configuration.ProcessDataReturnValues = oHazmatIntegration.ProcessData(oHeaders, Me.ConnectionString)
                Dim sLastError As String = oHazmatIntegration.LastError
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Hazmat information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Error Integration Failure! could not import Hazmat information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Hazmat Integration Had Errors", Source & " Warning!  Could not import some Hazmat information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Hazmat information:  " & sLastError)
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            LogError(Source & " Warning!  Hazmat Integration Had Errors", Source & " Error Data Validation Failure! could not import Hazmat information:  " & sLastError, AdminEmail)
                            blnRet = True
                        Else
                            LogError(Source & " Warning Integration Had Errors! could not import some Hazmat information:  " & sLastError)
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        Log("Success! The Hazmat information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Hazmat data to Process")
                blnRet = True
            End If

            Log("Process Hazmat Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Hazmat Integration Error", Source & " Unexpected Integration Error! Could not import Hazmat information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processHazmatDataBC20(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = True
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Hazmat Data; nothing to do returning false")
            Return False
        End If

        Try
            If Debug Then Log("Begin Process Hazmat Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oHazmatIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oHazmatIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oHazmatIntegration.UseDefaultCredentials = True
            Else
                oHazmatIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsHazmatObject)
            System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oBCIntegration = New BCIntegration.clsApplication()
            Dim oHazmats As Haz.Envelope = oBCIntegration.getHazmats(getAuthsSettngs(TMSSetting))
            Dim strSkip As New List(Of String)
            If (oHazmats Is Nothing OrElse oHazmats.Body Is Nothing OrElse oHazmats.Body.GetHazmat_Result Is Nothing OrElse oHazmats.Body.GetHazmat_Result.dynamicsTMSHazmats Is Nothing OrElse oHazmats.Body.GetHazmat_Result.dynamicsTMSHazmats.Length < 1) Then
                If Debug Then Log("Waiting on Hazmat Data")
                Return True 'not ready yet so just return true
            End If
            For Each p In oHazmats.Body.GetHazmat_Result.dynamicsTMSHazmats
                If Not p Is Nothing Then
                    If Not String.IsNullOrEmpty(p.HazRegulation) AndAlso Not String.IsNullOrEmpty(p.HazItem) AndAlso Not String.IsNullOrEmpty(p.HazID) Then
                        Dim oTMSHazmat = New TMSIntegrationServices.clsHazmatObject
                        CopyMatchingFieldsImplicitCast(oTMSHazmat, p, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oHeaders.Add(oTMSHazmat)
                    Else

                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Hazmat record could not be processed because the record had an invalid Regulation, Item, or ID value.  This typically indicates that an empty record was being transmitted.")
                        End If

                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aHazmatHeaders As TMSIntegrationServices.clsHazmatObject() = oHeaders.ToArray()
                Dim oResults = oHazmatIntegration.ProcessHazmatData70(TMSSetting.TMSAuthCode, aHazmatHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Hazmat information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The Hazmat information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Hazmat data to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Hazmat Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Hazmat Integration Error", Source & " Unexpected Integration Error! Could not import Hazmat information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processHazmatData(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = True
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Hazmat Data; nothing to do returning false")
            Return False
        End If
        If (TMSSetting.ERPSettingVersion < "8.2") Then
            Return processHazmatData2016(TMSSetting)
        End If

        If TMSSetting.ERPTypeControl = 4 Then
            Return processHazmatDataBC20(TMSSetting)
        End If

        Try
            If Debug Then Log("Begin Process Hazmat Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oHazmatIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oHazmatIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oHazmatIntegration.UseDefaultCredentials = True
            Else
                oHazmatIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsHazmatObject)
            ' Modified by RHR for v-8.4.0.003 on 07/13/2021 added support for TLs1.2
            If TMSSetting.ERPTypeControl = 3 Then System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavHazmat = New NAVService.DynamicsTMSHazmat()
            oNAVWebService.GetHazmat(oNavHazmat)
            Dim strSkip As New List(Of String)
            If oNavHazmat Is Nothing OrElse oNavHazmat.Hazmat Is Nothing OrElse oNavHazmat.Hazmat.Count() < 1 Then
                If Debug Then Log("Waiting on Hazmat Data")
                Return True 'not ready yet so just return true
            End If
            For Each p In oNavHazmat.Hazmat
                If Not p Is Nothing Then
                    If Not String.IsNullOrEmpty(p.HazRegulation) AndAlso Not String.IsNullOrEmpty(p.HazItem) AndAlso Not String.IsNullOrEmpty(p.HazID) Then
                        Dim oTMSHazmat = New TMSIntegrationServices.clsHazmatObject
                        CopyMatchingFieldsImplicitCast(oTMSHazmat, p, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oHeaders.Add(oTMSHazmat)
                    Else

                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Hazmat record could not be processed because the record had an invalid Regulation, Item, or ID value.  This typically indicates that an empty record was being transmitted.")
                        End If

                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aHazmatHeaders As TMSIntegrationServices.clsHazmatObject() = oHeaders.ToArray()
                Dim oResults = oHazmatIntegration.ProcessHazmatData70(TMSSetting.TMSAuthCode, aHazmatHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Hazmat information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The Hazmat information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Hazmat data to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Hazmat Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Hazmat Integration Error", Source & " Unexpected Integration Error! Could not import Hazmat information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function processHazmatData2016(ByVal TMSSetting As TMSIntegrationSettings.vERPIntegrationSetting) As Boolean
        Dim blnRet As Boolean = True
        If TMSSetting Is Nothing OrElse String.IsNullOrWhiteSpace(TMSSetting.TMSURI) OrElse String.IsNullOrWhiteSpace(TMSSetting.ERPURI) Then
            LogError("Missing TMS Integration settings for Hazmat Data; nothing to do returning false")
            Return False
        End If
        Try
            If Debug Then Log("Begin Process Hazmat Data ")
            Dim strMsg As String = ""
            Dim ReturnMessage As String = ""
            Dim RetVal As Integer = 0
            Dim oHazmatIntegration As New TMSIntegrationServices.DTMSERPIntegration()
            oHazmatIntegration.Url = TMSSetting.TMSURI
            If (String.IsNullOrWhiteSpace(TMSSetting.TMSAuthUser)) Then
                oHazmatIntegration.UseDefaultCredentials = True
            Else
                oHazmatIntegration.Credentials() = New System.Net.NetworkCredential(TMSSetting.TMSAuthUser, TMSSetting.TMSAuthPassword)
            End If
            Dim oHeaders As New List(Of TMSIntegrationServices.clsHazmatObject)
            Dim oNAVWebService = New NAV2016Services.DynamicsTMSWebServices()
            oNAVWebService.Url = TMSSetting.ERPURI
            If (String.IsNullOrWhiteSpace(TMSSetting.ERPAuthUser)) Then
                oNAVWebService.UseDefaultCredentials = True
            Else
                oNAVWebService.Credentials() = New System.Net.NetworkCredential(TMSSetting.ERPAuthUser, TMSSetting.ERPAuthPassword)
            End If
            Dim oNavHazmat = New NAV2016Services.DynamicsTMSHazmat()
            oNAVWebService.GetHazmat(oNavHazmat)
            Dim strSkip As New List(Of String)
            If oNavHazmat Is Nothing OrElse oNavHazmat.Hazmat Is Nothing OrElse oNavHazmat.Hazmat.Count() < 1 Then
                If Debug Then Log("Waiting on Hazmat Data")
                Return True 'not ready yet so just return true
            End If
            For Each p In oNavHazmat.Hazmat
                If Not p Is Nothing Then
                    If Not String.IsNullOrEmpty(p.HazRegulation) AndAlso Not String.IsNullOrEmpty(p.HazItem) AndAlso Not String.IsNullOrEmpty(p.HazID) Then
                        Dim oTMSHazmat = New TMSIntegrationServices.clsHazmatObject
                        CopyMatchingFieldsImplicitCast(oTMSHazmat, p, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            If Debug Then Log(strMsg)
                            strMsg = ""
                        End If
                        oHeaders.Add(oTMSHazmat)
                    Else

                        If Me.Debug Or Me.Verbose Then
                            Log(Source & ": A Hazmat record could not be processed because the record had an invalid Regulation, Item, or ID value.  This typically indicates that an empty record was being transmitted.")
                        End If

                    End If
                End If
            Next

            'save changes to database 
            If Not oHeaders Is Nothing AndAlso oHeaders.Count > 0 Then
                Dim aHazmatHeaders As TMSIntegrationServices.clsHazmatObject() = oHeaders.ToArray()
                Dim oResults = oHazmatIntegration.ProcessHazmatData70(TMSSetting.TMSAuthCode, aHazmatHeaders, ReturnMessage)
                Dim sLastError As String = ReturnMessage
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        LogError("Error Data Connection Failure! could not import Hazmat information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        generateDataIntegrationFailureAlert(TMSSetting.ERPTypeName, oResults, IntegrationModule.Hazmat, ReturnMessage, False)
                        If ERPTestingOn Then 'we return true so testing can continue other integration points if NAVTesting Flag is on
                            blnRet = True
                        End If
                    Case Else
                        'success
                        'Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        'Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        If Me.Verbose Then Log("Success! The Hazmat information was processed.")
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                If Verbose Then Log("No Hazmat data to Process")
                blnRet = True
            End If

            If Debug Then Log("Process Hazmat Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            LogError(Source & " Error!  Unexpected Hazmat Integration Error", Source & " Unexpected Integration Error! Could not import Hazmat information:  ", AdminEmail, ex)
        End Try

        Return blnRet
    End Function

    Private Function CopyMatchingFields(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String)) As Object
        If toObj Is Nothing Or fromObj Is Nothing Then
            Return Nothing
        End If

        Dim fromType As Type = fromObj.[GetType]()
        Dim toType As Type = toObj.[GetType]()

        ' Get all FieldInfo. 
        Dim fProps As PropertyInfo() = fromType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        Dim tProps As PropertyInfo() = toType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        For Each fProp As PropertyInfo In fProps
            Dim propValue As Object = fProp.GetValue(fromObj)
            'Removed by RHR 10/8/14 did not update nullable fields when null
            'If propValue IsNot Nothing Then
            If Not skipObjs.Contains(fProp.Name) Then
                For Each tProp In tProps
                    If tProp.Name = fProp.Name Then
                        If tProp.PropertyType() = fProp.PropertyType() Then
                            Try
                                tProp.SetValue(toObj, propValue)
                            Catch ex As Exception
                                Dim strMsg As String = ex.Message
                                Throw
                            End Try
                        End If
                        Exit For
                    End If
                Next
            End If
            'End If
        Next
        Return toObj

    End Function

    Private Function CopyMatchingFieldsImplicitCast(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String), ByRef strMsg As String) As Object
        If toObj Is Nothing Or fromObj Is Nothing Then
            Return Nothing
        End If
        'primatives used for casting
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
        If Me.Debug Then
            System.Diagnostics.Debug.WriteLine("")
            System.Diagnostics.Debug.WriteLine("*******************************************************")
            System.Diagnostics.Debug.WriteLine("")
        End If
        For Each fProp As PropertyInfo In fProps
            Dim propValue As Object = fProp.GetValue(fromObj)
            'Removed by RHR 10/8/14 did not update nullable fields when null
            'If propValue IsNot Nothing Then
            If Me.Debug Then System.Diagnostics.Debug.WriteLine(fProp.Name & ": " & propValue.ToString())
            If Not skipObjs.Contains(fProp.Name) Then
                For Each tProp In tProps
                    'If fProp.Name.ToUpper() = "POITEMORDERNUMBER" Then
                    '    System.Diagnostics.Debug.WriteLine(fProp.Name & ": " & propValue.ToString())
                    'End If
                    If tProp.Name.ToUpper() = fProp.Name.ToUpper() Then
                        If tProp.PropertyType() = fProp.PropertyType() Then
                            Try
                                tProp.SetValue(toObj, propValue)
                            Catch ex As Exception
                                strMsg &= ex.Message
                                Throw
                            End Try
                        Else
                            Try
                                Select Case tProp.PropertyType.Name
                                    Case "String"
                                        tProp.SetValue(toObj, propValue.ToString())
                                    Case "Int16"
                                        If Int16.TryParse(propValue.ToString(), iVal16) Then
                                            tProp.SetValue(toObj, iVal16)
                                        End If
                                    Case "Int32"
                                        If Int32.TryParse(propValue.ToString(), iVal32) Then
                                            tProp.SetValue(toObj, iVal32)
                                        End If
                                    Case "Int64"
                                        If Int32.TryParse(propValue.ToString(), iVal64) Then
                                            tProp.SetValue(toObj, iVal64)
                                        End If
                                    Case "Date"
                                        If Date.TryParse(propValue.ToString(), dtVal) Then
                                            tProp.SetValue(toObj, dtVal)
                                        End If
                                    Case "DateTime"
                                        If Date.TryParse(propValue.ToString(), dtVal) Then
                                            tProp.SetValue(toObj, dtVal)
                                        End If
                                    Case "Decimal"
                                        If Decimal.TryParse(propValue.ToString(), decVal) Then
                                            tProp.SetValue(toObj, decVal)
                                        End If
                                    Case "Double"
                                        If Double.TryParse(propValue.ToString(), dblVal) Then
                                            tProp.SetValue(toObj, dblVal)
                                        End If
                                    Case "Boolean"
                                        If Boolean.TryParse(propValue.ToString(), blnVal) Then
                                            tProp.SetValue(toObj, blnVal)
                                        Else
                                            'try to convert to an integer and then test for 0 any non zero is true
                                            If Integer.TryParse(propValue.ToString(), intVal) Then
                                                If intVal = 0 Then
                                                    blnVal = False
                                                Else
                                                    blnVal = True
                                                End If
                                                tProp.SetValue(toObj, blnVal)
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
        If Me.Debug Then
            System.Diagnostics.Debug.WriteLine("")
            System.Diagnostics.Debug.WriteLine("*******************************************************")
            System.Diagnostics.Debug.WriteLine("")
        End If
        Return toObj

    End Function

    Private Sub populateIntegrationObjectParameters(ByRef oImportExport As TMS.clsImportExport, ByVal NavSettings As DTO.DynamicsTMSSetting, Optional ByVal UnitTestKeys As clsUnitTestKeys = Nothing)

        Dim connectionString As String = Me.ConnectionString
        With oImportExport
            .AdminEmail = Me.AdminEmail
            .FromEmail = Me.FromEmail
            .GroupEmail = Me.GroupEmail
            .Retry = Me.AutoRetry
            .SMTPServer = Me.SMTPServer
            .DBServer = Me.DBServer
            .Database = Me.Database
            .ConnectionString = connectionString
            .Debug = Me.Debug
            If UnitTestKeys Is Nothing Then
                If Not NavSettings Is Nothing Then
                    .AuthorizationCode = NavSettings.DTMSWSAuthCode
                    .WCFAuthCode = NavSettings.DTMSWCFAuthCode
                    .WCFURL = NavSettings.DTMSWCFURL
                    .WCFTCPURL = NavSettings.DTMSWCFTCPURL
                End If
            Else
                .AuthorizationCode = UnitTestKeys.WSAuthCode
                .WCFAuthCode = UnitTestKeys.WCFAuthCode
                .WCFURL = UnitTestKeys.WCFURL
                .WCFTCPURL = UnitTestKeys.WCFTCPURL
            End If
        End With

    End Sub


    Private Sub populateIntegrationObjectParameters(ByRef oImportExport As TMS.clsImportExport, ByVal UnitTestKeys As clsUnitTestKeys)

        Dim connectionString As String = Me.ConnectionString
        With oImportExport
            .AdminEmail = Me.AdminEmail
            .FromEmail = Me.FromEmail
            .GroupEmail = Me.GroupEmail
            .Retry = Me.AutoRetry
            .SMTPServer = Me.SMTPServer
            .DBServer = Me.DBServer
            .Database = Me.Database
            .ConnectionString = connectionString
            .Debug = Me.Debug
            .AuthorizationCode = UnitTestKeys.WSAuthCode
            .WCFAuthCode = UnitTestKeys.WCFAuthCode
            .WCFURL = UnitTestKeys.WCFURL
            .WCFTCPURL = UnitTestKeys.WCFTCPURL
        End With
    End Sub


End Class

Public Class clsAPExportData80
    Public Headers() As NGL.FreightMaster.Integration.clsAPExportObject80
    Public Details() As NGL.FreightMaster.Integration.clsAPExportDetailObject80
    Public Fees() As NGL.FreightMaster.Integration.clsAPExportFeeObject80
End Class
