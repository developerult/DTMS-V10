Imports System.IO
Imports Ngl.Core
Imports Ngl.FreightMaster.Core
Imports Ngl.Core.Communication
Imports NGLData = Ngl.FreightMaster.Data
Imports BLL = Ngl.FM.BLL
Imports CarUtil = NGL.FM.CarTar.Util
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports Ngl.FM.CMDLine.ImportCarrierRates.ImportCarrierRates
Imports Ngl.FreightMaster.Data.DataTransferObjects
Imports System.Threading

Public Class clsApplication : Inherits Ngl.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New Ngl.FreightMaster.Core.UserConfiguration
    'Public blnUse48Mode As Boolean = False
    Public wcfParameters As New NGLData.WCFParameters

    Public Overrides Function readCommandLineArgs(ByVal strExe As String) As Integer
        'Note:  Return values
        '1 Success use parameters provided
        '0 Only show help message

        Dim args() As String = System.Environment.GetCommandLineArgs
        Dim strHelpMsg As String
        Try

            strHelpMsg = vbCrLf _
                & "Usage:" & vbCrLf _
                & strExe & " [/?] " _
                & " [-c " & Chr(34) & "Server=DatabaseServer; Database=FreightMasterDatabase; Integrated Security=SSPI" & Chr(34) & "] [-d] [-v] "
            strHelpMsg &= vbCrLf & vbCrLf _
                & "Options:" & vbCrLf _
                & "    /?" & vbTab & vbTab & "Show this help screen." & vbCrLf _
                & "    -c " & vbTab & vbTab & "Database Connection String (Required)." & vbCrLf _
                & "    -d" & vbTab & vbTab & "Debug Flag (Optional)." & vbCrLf _
                & "    -v" & vbTab & vbTab & "Verbose Flag (Optional Check with NGL Support)." _
                & vbCrLf & vbCrLf & "NOTE:  Spaces are required between parameters."
            For i As Integer = 0 To args.Length - 1
                Dim strArg As String = args(i).Replace("/", "-").ToLower
                Select Case strArg
                    Case "-c" 'Connection String Example: "Server=NglSql01P; Database=NglMas2002; Integrated Security=SSPI"
                        If i + 1 < args.Length Then ConnectionString = args(i + 1)
                    Case "-d"
                        Debug = True
                    Case "-v"
                        Verbose = True
                    Case "-type"
                        If i + 1 < args.Length Then ImportType = args(i + 1)
                    Case "-fn"
                        If i + 1 < args.Length Then FileName = args(i + 1)
                    Case "-?"
                        Console.WriteLine(strHelpMsg)
                        Console.WriteLine("Press Enter To Continue")
                        Console.ReadLine()
                        Return 0
                End Select
            Next
            'set the default value for the log file


            Return 1
        Catch ex As Exception
            Throw New ApplicationException(Source & " Read Command Line Args Failure! ", ex)
        End Try

    End Function

    ''' <summary>
    ''' must be in c:data/csvimports
    ''' </summary>
    ''' <remarks></remarks>
    Private _fileName As String
    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property
     
    ''' <summary>
    ''' Types:
    ''' ImportFromCSVRates 
    ''' ImportFromCSVInterline
    ''' ImportFromCSVNonService
    ''' </summary>
    ''' <remarks></remarks>
    Private _Type As String
    Public Property ImportType() As String
        Get
            Return _Type
        End Get
        Set(ByVal value As String)
            _Type = value
        End Set
    End Property


    Public Sub ProcessData()

        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()

        Dim oQuery As New Ngl.Core.Data.Query
        oQuery.Database = Me.oConfig.Database
        oQuery.Server = Me.oConfig.DBServer
        If Not oQuery.testConnection() Then
            LogError(Source & " Database Connection Failure", "Actual error reported: " & oQuery.LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            Return
        End If

        Try
            Dim args() As String = System.Environment.GetCommandLineArgs

            Dim intRet As Integer = 0
            Dim LastError As String = ""
            Log("Begin Process Data ")
            DoUpdate()

            Log("Process Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected Import Carrier Rates error", " The actual error is: " & ex.Message & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
        Finally

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

    Private Sub DoUpdate()
        Try
            Select Case ImportType
                Case CarUtil.ImportExportTypes.ImportFromCSVRates.ToString
                    importcarrierRates()
                    Exit Select
                Case CarUtil.ImportExportTypes.ImportFromCSVInterline.ToString
                    importcarrierRatesInterline()
                    Exit Select
                Case CarUtil.ImportExportTypes.ImportFromCSVNonService.ToString
                    importcarrierRatesNonService()
                    Exit Select
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Function importcarrierRates() As List(Of Dictionary(Of String, ArrayList))
        'must be in this folder
        ''  this.LocalDirectory = "C:\\Data\\CSVImport\\";
        Dim processName As Guid = Guid.NewGuid()
        Dim bll As New BLL.NGLCarrTarBLL(wcfParameters)
        Dim list As List(Of Dictionary(Of String, ArrayList)) = bll.ImportCarrTarRatesFromCSV(FileName, False, wcfParameters.UserName, processName.ToString(), "CarrTarEquipMatName")
        Dim continueWhile As Boolean = True
        Do While continueWhile
            Thread.Sleep(5000)
            Dim list2 As List(Of Dictionary(Of String, ArrayList)) = bll.CheckImportTariffBatches(wcfParameters.UserName, processName.ToString())
            If list2 IsNot Nothing AndAlso list2.Count > 0 Then
                Console.WriteLine("*****Messages******")
                For Each item In list2
                    For Each innerItem In item
                        Console.WriteLine(innerItem.Key)
                    Next
                Next
                Console.WriteLine("*****End of Messages******")
                continueWhile = False
                Exit Do
            End If
        Loop
        Return list
    End Function

    Private Function importcarrierRatesInterline() As List(Of Dictionary(Of String, ArrayList))
        'must be in this folder
        ''  this.LocalDirectory = "C:\\Data\\CSVImport\\";
        Dim processName As Guid = Guid.NewGuid()
        Dim bll As New BLL.NGLCarrTarBLL(wcfParameters)
        Dim list As List(Of Dictionary(Of String, ArrayList)) = bll.ImportCarrTarInterlinePointsFromCSV(FileName, False, wcfParameters.UserName, processName.ToString())
        Dim continueWhile As Boolean = True
        Do While continueWhile
            Thread.Sleep(5000)
            Dim list2 As List(Of Dictionary(Of String, ArrayList)) = bll.CheckImportTariffBatches(wcfParameters.UserName, processName.ToString())
            If list2 IsNot Nothing AndAlso list2.Count > 0 Then
                Console.WriteLine("*****Messages******")
                For Each item In list2
                    For Each innerItem In item
                        Console.WriteLine(innerItem.Key)
                    Next
                Next
                Console.WriteLine("*****End of Messages******")
                continueWhile = False
                Exit Do
            End If
        Loop
        Return list
    End Function

    Private Function importcarrierRatesNonService() As List(Of Dictionary(Of String, ArrayList))
        'must be in this folder
        ''  this.LocalDirectory = "C:\\Data\\CSVImport\\";
        Dim processName As Guid = Guid.NewGuid()
        Dim bll As New BLL.NGLCarrTarBLL(wcfParameters)
        Dim list As List(Of Dictionary(Of String, ArrayList)) = bll.ImportCarrTarNonServicePointsFromCSV(FileName, False, wcfParameters.UserName, processName.ToString())
        Dim continueWhile As Boolean = True
        Do While continueWhile
            Thread.Sleep(5000)
            Dim list2 As List(Of Dictionary(Of String, ArrayList)) = bll.CheckImportTariffBatches(wcfParameters.UserName, processName.ToString())
            If list2 IsNot Nothing AndAlso list2.Count > 0 Then
                Console.WriteLine("*****Messages******")
                For Each item In list2
                    For Each innerItem In item
                        Console.WriteLine(innerItem.Key)
                    Next
                Next
                Console.WriteLine("*****End of Messages******")
                continueWhile = False
                Exit Do
            End If
        Loop
        Return list
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Added call to processNewTaskParameters()
    ''' </remarks>
    Public Overrides Function getTaskParameters() As Boolean
        Dim blnRet As Boolean = False
        Try
            'blnUse48Mode = My.Settings.Use48Mode
            'get the parameter settings from the database.
            Dim oSysData As New NGLData.NGLSystemDataProvider(ConnectionString)
            Dim oGTPs As NGLData.DataTransferObjects.GlobalTaskParameters = oSysData.GetGlobalTaskParameters
            If Not oGTPs Is Nothing Then
                With oGTPs
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

                'Added By LVV 2/18/16 v-7.0.5.0
                processNewTaskParameters(oGTPs)

                If wcfParameters Is Nothing Then wcfParameters = New NGLData.WCFParameters
                With wcfParameters
                    .UserName = "nglweb"
                    .Database = Database
                    .DBServer = DBServer
                    .WCFAuthCode = "NGLSystem"
                    .ConnectionString = "Data Source=" & .DBServer & ";Initial Catalog=" & .Database & ";Integrated Security=True"
                End With
            End If
            blnRet = True
        Catch ex As Ngl.FreightMaster.Data.DatabaseReadDataException
            'cannot read the database settings so use the config data
            Try


                blnRet = True
            Catch e As Exception
                LogException("Cannot get task parameters", e)
            End Try
        Catch ex As Exception
            Log("Read Command Line Task Parameter Failure: " & readExceptionMessage(ex))
        End Try

        Return blnRet
    End Function




    Public Sub assignParameters(Optional ByVal user_name As String = "NGL\rramsey",
                                 Optional ByVal database_catalog As String = "NGLMAS2013DEV",
                                 Optional ByVal db_server As String = "NGLRDP06D",
                                 Optional ByVal wcf_auth_code As String = "NGLWCFDEV",
                                 Optional ByVal admin_email As String = "info@maxximu.com",
                                 Optional ByVal from_email As String = "info@maxximu.com",
                                 Optional ByVal group_email As String = "info@maxximu.com",
                                 Optional ByVal smtp_server As String = "smtp.gmail.com",
                                 Optional ByVal web_auth_code As String = "NGLWSTest",
                                 Optional ByVal comp_control As Integer = 0,
                                 Optional ByVal comp_number As Integer = 0,
                                 Optional ByVal lane_control As Integer = 0,
                                 Optional ByVal lane_number As String = "",
                                 Optional ByVal carrier_control As Integer = 0,
                                 Optional ByVal carrier_number As Integer = 0,
                                 Optional ByVal order_number As String = "",
                                 Optional ByVal pro_number As String = "",
                                 Optional ByVal cns_number As String = "")

        If wcfParameters Is Nothing Then wcfParameters = New NGLData.WCFParameters
        With wcfParameters
            .UserName = user_name
            .Database = database_catalog
            .DBServer = db_server
            .WCFAuthCode = wcf_auth_code
            '.ConnectionString = 
        End With
        AdminEmail = admin_email
        GroupEmail = group_email
        FromEmail = from_email
        SMTPServer = smtp_server
        DBServer = db_server
        Database = database_catalog
        'WebAuthCode = web_auth_code
        'CompControl = comp_control
        'CompNumber = comp_number
        'LaneControl = lane_control
        'LaneNumber = lane_number
        'CarrierControl = carrier_control
        'CarrierNumber = carrier_number
        'OrderNumber = order_number
        'ProNumber = pro_number
        'CNSNumber = cns_number
        'testConnectionString = "Data Source=" & DBServer & ";Initial Catalog=" & Database & ";Integrated Security=True"

    End Sub



End Class




