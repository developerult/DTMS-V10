
Imports System.Text
Imports System.Text.RegularExpressions

Imports Ngl.Core.Communication
Imports DAL = Ngl.FreightMaster.Data
Public MustInherit Class NGLCommandLineBaseClass : Inherits Ngl.Core.NGLBaseClass

#Region "Enums"

    Public Enum IntegrationModule
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

    Public Enum ProcessDataReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum


#End Region
#Region " Class Variables and Properties "

    Public Overridable ReadOnly Property DBInfo() As String
        Get
            If Not String.IsNullOrEmpty(Database) AndAlso Not String.IsNullOrEmpty(DBServer) Then
                Return "Server: " & DBServer & " | " & "Database: " & Database
            Else
                'just return the conneciton string
                Return ConnectionString
            End If

        End Get
    End Property


    Private _strResultsFile As String = ""
    ''' <summary>
    ''' Result/Log file name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property ResultsFile() As String
        Get
            Return _strResultsFile
        End Get
        Set(ByVal value As String)
            _strResultsFile = value
        End Set
    End Property

    Private _strINIKey As String = "NGL"
    ''' <summary>
    ''' INI File Key Section For Configuration Settings; default = NGL
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property INIKey() As String
        Get
            Return _strINIKey
        End Get
        Set(ByVal value As String)
            _strINIKey = value
        End Set
    End Property

    Private _intAutoRetry As Integer = 3
    ''' <summary>
    ''' Number of auto retrys before throwing errors
    ''' Used on batch processing and unattended execution components    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property AutoRetry() As Integer
        Get
            Return _intAutoRetry
        End Get
        Set(ByVal value As Integer)
            _intAutoRetry = value
        End Set
    End Property

    Private _LegalEntity As String
    ''' <summary>
    ''' Optional Legal Entity Filter typically provided by a command line argument -l
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegalEntity() As String
        Get
            Return _LegalEntity
        End Get
        Set(ByVal value As String)
            _LegalEntity = value
        End Set
    End Property

    Private _ConnectionString As String = "" '"Data Source=NGLSQL01T;Initial Catalog=NGLMAS2002DEV;Integrated Security=True"
    ''' <summary>
    ''' Prefered database connection method replaces the Database and DBServer properties
    ''' typically this is passed as a command line argument.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ConnectionString() As String
        Get
            If String.IsNullOrEmpty(_ConnectionString) Then
                'try to build the connection string using database and server
                If Not String.IsNullOrEmpty(Database) AndAlso Not String.IsNullOrEmpty(DBServer) Then
                    _ConnectionString = String.Format("Server={0}; Database={1}; Integrated Security=SSPI", DBServer, Database)
                End If
            End If
            Return _ConnectionString
        End Get
        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property

    Private _strDatabase As String = ""
    ''' <summary>
    ''' Primary Database Name; Depreciated we now use ConnectionString
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property Database() As String
        Get
            'If the database name has not been set we try to extract it from the connection string 
            '(this is not the most accurate way and is now only used for discriptions)
            If String.IsNullOrEmpty(_strDatabase) AndAlso Not String.IsNullOrEmpty(_ConnectionString) Then
                'examples of connection strings are:
                'Server=NglSql01P; Database=NglMas2002; Integrated Security=SSPI
                'and
                'Data Source=NGLSQL01T;Initial Catalog=NGLMAS2002DEV;Integrated Security=True
                Dim strParts() As String = ConnectionString.Split(";")
                For Each strPart In strParts
                    If strPart.Contains("Catalog=") OrElse strPart.Contains("Database=") Then
                        Dim strNameParts() As String = strPart.Split("=")
                        _strDatabase = strNameParts(1)
                        Exit For
                    End If
                Next
            End If
            Return _strDatabase
        End Get
        Set(ByVal value As String)
            _strDatabase = value
        End Set
    End Property
    Private _strDBServer As String = ""
    ''' <summary>
    ''' Primary Database Server; Depreciated we now use ConnectionString
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property DBServer() As String
        Get
            'If the DBServer has not been set we try to extract it from the connection string 
            '(this is not the most accurate way and is now only used for discriptions)
            If String.IsNullOrEmpty(_strDBServer) AndAlso Not String.IsNullOrEmpty(_ConnectionString) Then
                'examples of connection strings are:
                'Server=NglSql01P; Database=NglMas2002; Integrated Security=SSPI
                'and
                'Data Source=NGLSQL01T;Initial Catalog=NGLMAS2002DEV;Integrated Security=True
                Dim strParts() As String = ConnectionString.Split(";")
                For Each strPart In strParts
                    If strPart.Contains("Server=") OrElse strPart.Contains("Source=") Then
                        Dim strNameParts() As String = strPart.Split("=")
                        _strDBServer = strNameParts(1)
                        Exit For
                    End If
                Next
            End If
            Return _strDBServer
        End Get
        Set(ByVal value As String)
            _strDBServer = value
        End Set
    End Property

    Private _strAdminEmail As String = ""
    ''' <summary>
    ''' Administrative Email Address 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property AdminEmail() As String
        Get
            Return _strAdminEmail
        End Get
        Set(ByVal value As String)
            _strAdminEmail = value
        End Set
    End Property

    Private _strGroupEmail As String = ""
    ''' <summary>
    ''' User Community Group Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GroupEmail() As String
        Get
            Return _strGroupEmail
        End Get
        Set(ByVal value As String)
            _strGroupEmail = value
        End Set
    End Property
    Private _strFromEmail As String = "system@nextgeneration.com"
    ''' <summary>
    ''' System From Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property FromEmail() As String
        Get
            Return _strFromEmail
        End Get
        Set(ByVal value As String)
            _strFromEmail = value
        End Set
    End Property

    Private _strSMTPServer As String = "sandbox.smtp.mailtrap.io" '"mail.ngl.local"
    ''' <summary>
    ''' SMTP Server Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property SMTPServer() As String
        Get
            Return _strSMTPServer
        End Get
        Set(ByVal value As String)
            _strSMTPServer = value
        End Set
    End Property

    Protected mstrLogFile As String = ""
    ''' <summary>
    ''' Log File Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property LogFile() As String
        Get
            Return mstrLogFile
        End Get
        Set(ByVal Value As String)
            mstrLogFile = Value
        End Set
    End Property

    Protected mobjLog As clsLog
    ''' <summary>
    ''' Log File Object
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property oLog() As clsLog
        Get
            Return mobjLog
        End Get
        Set(ByVal value As clsLog)
            mobjLog = value
        End Set
    End Property

    Protected mioLog As System.IO.StreamWriter
    ''' <summary>
    ''' Stream Writer Object
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property ioLog() As System.IO.StreamWriter
        Get
            Return mioLog
        End Get
        Set(ByVal value As System.IO.StreamWriter)
            mioLog = value
        End Set
    End Property

    Protected mblnSaveOldLog As Boolean = True
    ''' <summary>
    ''' Flag that determines if a back up copy of the log file is kept
    ''' when a new log file is created (the default is true)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property SaveOldLog() As Boolean
        Get
            Return mblnSaveOldLog
        End Get
        Set(ByVal value As Boolean)
            mblnSaveOldLog = value
        End Set
    End Property

    Protected mintKeepLogDays As Integer = "30"
    ''' <summary>
    ''' The number of days to keep the log file before
    ''' creating a new one (the defaulr is 30 days)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property KeepLogDays() As Integer
        Get
            If mintKeepLogDays < 1 Then
                mintKeepLogDays = 30
            End If
            Return mintKeepLogDays
        End Get
        Set(ByVal value As Integer)
            mintKeepLogDays = value
        End Set
    End Property

    Protected _GlobalDefaultLoadAcceptAllowedMinutes As Integer = "120"
    ''' <summary>
    ''' The number of minutes the system should wait before
    ''' expiring a load in NEXTRack. If longer than this value the load will be expired.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalDefaultLoadAcceptAllowedMinutes() As Integer
        Get
            If _GlobalDefaultLoadAcceptAllowedMinutes < 1 Then
                _GlobalDefaultLoadAcceptAllowedMinutes = 120
            End If
            Return _GlobalDefaultLoadAcceptAllowedMinutes
        End Get
        Set(ByVal value As Integer)
            _GlobalDefaultLoadAcceptAllowedMinutes = value
        End Set
    End Property

    Protected mstrAuthNbrFTPRoot As String = "ftp://ftp.nextgeneration.com/Authorization/"
    ''' <summary>
    ''' Used to hold the FTP root path for the location of the new Auth Code File
    ''' Each Database will pass the AuthName value as the actual file name like NGLMAS2002.txt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property AuthNbrFTPRoot() As String
        Get
            Return mstrAuthNbrFTPRoot
        End Get
        Set(ByVal value As String)
            mstrAuthNbrFTPRoot = value
        End Set
    End Property

    Protected _GlobalFuelIndexUpdateEmailNotificationValue As Boolean = False
    ''' <summary>
    ''' Do we use the Global Fuel Index Update Email address for Notification?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalFuelIndexUpdateEmailNotificationValue() As Boolean
        Get

            Return _GlobalFuelIndexUpdateEmailNotificationValue
        End Get
        Set(ByVal value As Boolean)
            _GlobalFuelIndexUpdateEmailNotificationValue = value
        End Set
    End Property

    Private _GlobalFuelIndexUpdateEmailNotification As String = "support@nextgeneration.com"
    ''' <summary>
    ''' Global Fuel Index Update Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalFuelIndexUpdateEmailNotification() As String
        Get
            Return _GlobalFuelIndexUpdateEmailNotification
        End Get
        Set(ByVal value As String)
            _GlobalFuelIndexUpdateEmailNotification = value
        End Set
    End Property

    Protected _GlobalCarrierContractExpiredEmailNotificationValue As Boolean = False
    ''' <summary>
    ''' Do we use the Global Carrier Contract Expired Email address for Notification?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalCarrierContractExpiredEmailNotificationValue() As Boolean
        Get

            Return _GlobalCarrierContractExpiredEmailNotificationValue
        End Get
        Set(ByVal value As Boolean)
            _GlobalCarrierContractExpiredEmailNotificationValue = value
        End Set
    End Property

    Private _GlobalCarrierContractExpiredEmailNotification As String = "support@nextgeneration.com"
    ''' <summary>
    ''' Global Carrier Contract Expired Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalCarrierContractExpiredEmailNotification() As String
        Get
            Return _GlobalCarrierContractExpiredEmailNotification
        End Get
        Set(ByVal value As String)
            _GlobalCarrierContractExpiredEmailNotification = value
        End Set
    End Property

    Protected _GlobalCarrierExposureAllEmailNotificationValue As Boolean = False
    ''' <summary>
    ''' Do we use the Global Carrier Exposure All Email address for Notification?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalCarrierExposureAllEmailNotificationValue() As Boolean
        Get

            Return _GlobalCarrierExposureAllEmailNotificationValue
        End Get
        Set(ByVal value As Boolean)
            _GlobalCarrierExposureAllEmailNotificationValue = value
        End Set
    End Property

    Private _GlobalCarrierExposureAllEmailNotification As String = "support@nextgeneration.com"
    ''' <summary>
    ''' Global Carrier Exposure All Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalCarrierExposureAllEmailNotification() As String
        Get
            Return _GlobalCarrierExposureAllEmailNotification
        End Get
        Set(ByVal value As String)
            _GlobalCarrierExposureAllEmailNotification = value
        End Set
    End Property

    Protected _GlobalCarrierExposurePerShipmentEmailNotificationValue As Boolean = False
    ''' <summary>
    ''' Do we use the Global Carrier Exposure Per Shipment Email address for Notification?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalCarrierExposurePerShipmentEmailNotificationValue() As Boolean
        Get

            Return _GlobalCarrierExposurePerShipmentEmailNotificationValue
        End Get
        Set(ByVal value As Boolean)
            _GlobalCarrierExposurePerShipmentEmailNotificationValue = value
        End Set
    End Property

    Private _GlobalCarrierExposurePerShipmentEmailNotification As String = "support@nextgeneration.com"
    ''' <summary>
    ''' Global Carrier Exposure Per Shipment Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalCarrierExposurePerShipmentEmailNotification() As String
        Get
            Return _GlobalCarrierExposurePerShipmentEmailNotification
        End Get
        Set(ByVal value As String)
            _GlobalCarrierExposurePerShipmentEmailNotification = value
        End Set
    End Property

    Protected _GlobalCarrierInsuranceExpiredEmailNotificationValue As Boolean = False
    ''' <summary>
    ''' Do we use the Global Carrier Insurance Expired Email address for Notification?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalCarrierInsuranceExpiredEmailNotificationValue() As Boolean
        Get

            Return _GlobalCarrierInsuranceExpiredEmailNotificationValue
        End Get
        Set(ByVal value As Boolean)
            _GlobalCarrierInsuranceExpiredEmailNotificationValue = value
        End Set
    End Property

    Protected _GlobalCarrierInsuranceExpiredEmailNotification As String = "support@nextgeneration.com"
    ''' <summary>
    ''' Global Carrier Insurance Expired Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalCarrierInsuranceExpiredEmailNotification() As String
        Get
            Return _GlobalCarrierInsuranceExpiredEmailNotification
        End Get
        Set(ByVal value As String)
            _GlobalCarrierInsuranceExpiredEmailNotification = value
        End Set
    End Property

    Protected _GlobalOutdatedNoLanePOEmailNotificationValue As Boolean = False
    ''' <summary>
    ''' Do we use the Global Outdated No Lane PO Email address for Notification?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalOutdatedNoLanePOEmailNotificationValue() As Boolean
        Get

            Return _GlobalOutdatedNoLanePOEmailNotificationValue
        End Get
        Set(ByVal value As Boolean)
            _GlobalOutdatedNoLanePOEmailNotificationValue = value
        End Set
    End Property

    Private _GlobalOutdatedNoLanePOEmailNotification As String = "support@nextgeneration.com"
    ''' <summary>
    ''' Global Outdated No Lane PO Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalOutdatedNoLanePOEmailNotification() As String
        Get
            Return _GlobalOutdatedNoLanePOEmailNotification
        End Get
        Set(ByVal value As String)
            _GlobalOutdatedNoLanePOEmailNotification = value
        End Set
    End Property

    Protected _GlobalOutdatedNStatusEmailNotificationValue As Boolean = False
    ''' <summary>
    ''' Do we use the Global Outdated N Status Email address for Notification?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalOutdatedNStatusEmailNotificationValue() As Boolean
        Get

            Return _GlobalOutdatedNStatusEmailNotificationValue
        End Get
        Set(ByVal value As Boolean)
            _GlobalOutdatedNStatusEmailNotificationValue = value
        End Set
    End Property

    Protected _GlobalOutdatedNStatusEmailNotification As String = "support@nextgeneration.com"
    ''' <summary>
    ''' Global Outdated N Status Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalOutdatedNStatusEmailNotification() As String
        Get
            Return _GlobalOutdatedNStatusEmailNotification
        End Get
        Set(ByVal value As String)
            _GlobalOutdatedNStatusEmailNotification = value
        End Set
    End Property

    Protected _GlobalPOsWaitingEmailNotificationValue As Boolean = False
    ''' <summary>
    ''' Do we use the Global POs Waiting Email address for Notification?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalPOsWaitingEmailNotificationValue() As Boolean
        Get

            Return _GlobalPOsWaitingEmailNotificationValue
        End Get
        Set(ByVal value As Boolean)
            _GlobalPOsWaitingEmailNotificationValue = value
        End Set
    End Property

    Protected _GlobalPOsWaitingEmailNotification As String = "support@nextgeneration.com"
    ''' <summary>
    ''' Global POs Waiting Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property GlobalPOsWaitingEmailNotification() As String
        Get
            Return _GlobalPOsWaitingEmailNotification
        End Get
        Set(ByVal value As String)
            _GlobalPOsWaitingEmailNotification = value
        End Set
    End Property


    Protected _NEXTStopAcctNo As String = ""

    Public Property NEXTStopAcctNo() As String
        Get
            Return _NEXTStopAcctNo
        End Get
        Set(ByVal value As String)
            _NEXTStopAcctNo = value
        End Set
    End Property

    Protected _NEXTStopContact As String = ""

    Public Property NEXTStopContact() As String
        Get
            Return _NEXTStopContact
        End Get
        Set(ByVal value As String)
            _NEXTStopContact = value
        End Set
    End Property

    Protected _NEXTStopHotLoadAccountName As String = ""

    Public Property NEXTStopHotLoadAccountName() As String
        Get
            Return _NEXTStopHotLoadAccountName
        End Get
        Set(ByVal value As String)
            _NEXTStopHotLoadAccountName = value
        End Set
    End Property

    Protected _NEXTStopHotLoadContact As String = ""

    Public Property NEXTStopHotLoadContact() As String
        Get
            Return _NEXTStopHotLoadContact
        End Get
        Set(ByVal value As String)
            _NEXTStopHotLoadContact = value
        End Set
    End Property

    Protected _NEXTStopHotLoadURL As String = ""

    Public Property NEXTStopHotLoadURL() As String
        Get
            Return _NEXTStopHotLoadURL
        End Get
        Set(ByVal value As String)
            _NEXTStopHotLoadURL = value
        End Set
    End Property

    Protected _NEXTStopPhone As String = ""

    Public Property NEXTStopPhone() As String
        Get
            Return _NEXTStopPhone
        End Get
        Set(ByVal value As String)
            _NEXTStopPhone = value
        End Set
    End Property

    Protected _NEXTStopURL As String = ""

    Public Property NEXTStopURL() As String
        Get
            Return _NEXTStopURL
        End Get
        Set(ByVal value As String)
            _NEXTStopURL = value
        End Set
    End Property

    Protected _NEXTRackDatabase As String = ""

    Public Property NEXTRackDatabase() As String
        Get
            Return _NEXTRackDatabase
        End Get
        Set(ByVal value As String)
            _NEXTRackDatabase = value
        End Set
    End Property

    Protected _NEXTRackDatabaseServer As String = ""

    Public Property NEXTRackDatabaseServer() As String
        Get
            Return _NEXTRackDatabaseServer
        End Get
        Set(ByVal value As String)
            _NEXTRackDatabaseServer = value
        End Set
    End Property

    Protected _NEXTrackURL As String = ""

    Public Property NEXTrackURL() As String
        Get
            Return _NEXTrackURL
        End Get
        Set(ByVal value As String)
            _NEXTrackURL = value
        End Set
    End Property

    Protected _GlobalSMTPUser As String = "3b9ce0d43392e5"

    Public Property GlobalSMTPUser() As String
        Get
            Return _GlobalSMTPUser
        End Get
        Set(ByVal value As String)
            _GlobalSMTPUser = value
        End Set
    End Property

    Protected _GlobalSMTPPass As String = "3ba5d49379a196"

    Public Property GlobalSMTPPass() As String
        Get
            Return _GlobalSMTPPass
        End Get
        Set(ByVal value As String)
            _GlobalSMTPPass = value
        End Set
    End Property

    Protected _ReportServerURL As String = ""

    Public Property ReportServerURL() As String
        Get
            Return _ReportServerURL
        End Get
        Set(ByVal value As String)
            _ReportServerURL = value
        End Set
    End Property

    Protected _ReportServerUser As String = ""

    Public Property ReportServerUser() As String
        Get
            Return _ReportServerUser
        End Get
        Set(ByVal value As String)
            _ReportServerUser = value
        End Set
    End Property

    Protected _ReportServerPass As String = ""

    Public Property ReportServerPass() As String
        Get
            Return _ReportServerPass
        End Get
        Set(ByVal value As String)
            _ReportServerPass = value
        End Set
    End Property

    Protected _ReportServerDomain As String = ""

    Public Property ReportServerDomain() As String
        Get
            Return _ReportServerDomain
        End Get
        Set(ByVal value As String)
            _ReportServerDomain = value
        End Set
    End Property

    'Added By LVV 2/18/16 v-7.0.5.0
    Protected _GlobalSMTPUseDefaultCredentials As Boolean = True
    Public Property GlobalSMTPUseDefaultCredentials() As Boolean
        Get
            Return _GlobalSMTPUseDefaultCredentials
        End Get
        Set(ByVal value As Boolean)
            _GlobalSMTPUseDefaultCredentials = value
        End Set
    End Property

    'Added By LVV 2/18/16 v-7.0.5.0
    Protected _GlobalSMTPEnableSSL As Boolean = False
    Public Property GlobalSMTPEnableSSL() As Boolean
        Get
            Return _GlobalSMTPEnableSSL
        End Get
        Set(ByVal value As Boolean)
            _GlobalSMTPEnableSSL = value
        End Set
    End Property

    'Added By LVV 2/18/16 v-7.0.5.0
    Protected _GlobalSMTPTargetName As String = ""
    Public Property GlobalSMTPTargetName() As String
        Get
            Return _GlobalSMTPTargetName
        End Get
        Set(ByVal value As String)
            _GlobalSMTPTargetName = value
        End Set
    End Property

    'Added By LVV 2/18/16 v-7.0.5.0
    Protected _GlobalSMTPPort As Integer = 25
    Public Property GlobalSMTPPort() As Integer
        Get
            Return _GlobalSMTPPort
        End Get
        Set(ByVal value As Integer)
            _GlobalSMTPPort = value
        End Set
    End Property





#End Region

#Region " Constructors "




#End Region

#Region " Properties"

    Private _WCFParameters As DAL.WCFParameters
    Public Property WCFParameters() As DAL.WCFParameters
        Get
            If _WCFParameters Is Nothing Then
                'Note: WCFAuthCode = "NGLSystem" does not validate user when ValidateAccess = False 
                _WCFParameters = New DAL.WCFParameters With {.UserName = "", _
                                                             .Database = Me.Database, _
                                                             .DBServer = Me.DBServer, _
                                                             .ConnectionString = Me.ConnectionString, _
                                                             .WCFAuthCode = "NGLSystem", _
                                                             .ValidateAccess = False}
            End If
            Return _WCFParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _WCFParameters = value
        End Set
    End Property

#End Region

#Region " Functions "

    Public Overridable Sub openLog()
        Try
            If mstrLogFile.Length > 0 Then
                mobjLog = New clsLog
                mobjLog.Debug = mblnDebug
                mioLog = mobjLog.Open(mstrLogFile, KeepLogDays, SaveOldLog)
            End If

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Public Overridable Sub closeLog(ByVal intReturn As Integer)
        Try
            If IsNothing(mobjLog) Then
                Exit Sub
            End If
            mobjLog.closeLog(intReturn, mioLog)
        Catch ex As Exception
            'ignore any errors when closing the log file
        End Try
    End Sub

    Public Overridable Sub Log(ByVal logMessage As String)

        Try


            If IsNothing(mobjLog) Then
                If Me.Debug Then
                    Console.WriteLine(logMessage)
                End If
                Exit Sub
            End If
            mobjLog.Write(logMessage, mioLog)
        Catch ex As Exception
            'ignore any errors while writing to the log
            If Me.Debug Then
                Console.WriteLine("Save Log Error: " & ex.ToString)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Updates the LastError property and add the message to the Log file.  Does not send email.
    ''' </summary>
    ''' <param name="strMsg"></param>
    ''' <remarks></remarks>
    Public Overridable Sub LogError(ByVal strMsg As String)
        LastError = strMsg
        Log(strMsg)
    End Sub

    ''' <summary>
    ''' updates the LastError property, emails the logMessage, and adds the logMessage to the log file.
    ''' Errors are added to the log file as needed
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="logMessage"></param>
    ''' <param name="strMailTo"></param>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use optional parameters
    ''' </remarks>
    Public Overridable Sub LogError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String)
        Dim oEmail As New Email
        Me.LastError = logMessage
        Try
            'Modified By LVV 2/18/16 v-7.0.5.0
            If Not oEmail.SendMail(SMTPServer, strMailTo, FromEmail, logMessage & vbCrLf & vbCrLf & Me.DBInfo, strSubject, "", SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                Log("Send Email Error:  Could not send message to " & strMailTo)
            End If

        Catch ex As Exception
            'do nothing 
        Finally
            oEmail = Nothing

        End Try
        Dim strLogMsg As String = "Email Notice Sent to " & strMailTo & vbCrLf & " Subject: " & strSubject & vbCrLf & "Msg: " & logMessage
        Log(strLogMsg)

    End Sub

    ''' <summary>
    ''' Formats the exception information based on the debug settings, updates the LastError property, emails the message, and adds the message to the log file.
    ''' Errors are added to the log file as needed
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="logMessage">appended to the beginning of the exception information</param>
    ''' <param name="strMailTo"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use optional parameters
    ''' </remarks>
    Public Overridable Sub LogError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, ByVal e As Exception)
        Dim oEmail As New Email
        Me.LastError = logMessage
        If Not e Is Nothing Then
            logMessage &= "  The actual error is:" & vbCrLf & readExceptionMessage(e) & vbCrLf & "Please check the log files for more information."
        End If
        Try
            'Modified By LVV 2/18/16 v-7.0.5.0
            If Not oEmail.SendMail(SMTPServer, strMailTo, FromEmail, logMessage & vbCrLf & vbCrLf & Me.DBInfo, strSubject, "", SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                Log("Send Email Error:  Could not send message to " & strMailTo)
            End If

        Catch ex As Exception
            'do nothing 
        Finally
            oEmail = Nothing

        End Try
        Dim strLogMsg As String = "Email Notice Sent to " & strMailTo & vbCrLf & " Subject: " & strSubject & vbCrLf & "Msg: " & logMessage
        Log(strLogMsg)

    End Sub

    ''' <summary>
    ''' Updates LastError property and sends email to strMailTo but does not add the message to the Log file. 
    ''' All errors are ignored
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="logMessage"></param>
    ''' <param name="strMailTo"></param>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use optional parameters
    ''' </remarks>
    Public Overridable Sub EmailError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String)
        Dim oEmail As New Email
        Me.LastError = logMessage
        Try
            'Modified By LVV 2/18/16 v-7.0.5.0
            oEmail.SendMail(SMTPServer, strMailTo, FromEmail, logMessage & vbCrLf & vbCrLf & Me.DBInfo, strSubject, "", SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort)


        Catch ex As Exception
            'do nothing 
        Finally
            oEmail = Nothing

        End Try

    End Sub

    ''' <summary>
    ''' Updates LastError property and sends email to strMailTo but does not add the message to the Log file. 
    ''' All errors are ignored
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="logMessage"></param>
    ''' <param name="strMailTo"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use optional parameters
    ''' </remarks>
    Public Overridable Sub EmailError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, ByVal e As Exception)
        Dim oEmail As New Email
        Me.LastError = logMessage
        If Not e Is Nothing Then
            logMessage &= "  The actual error is:" & vbCrLf & readExceptionMessage(e) & vbCrLf & "Please check the log files for more information."
        End If
        Try
            'Modified By LVV 2/18/16 v-7.0.5.0
            oEmail.SendMail(SMTPServer, strMailTo, FromEmail, logMessage & vbCrLf & vbCrLf & Me.DBInfo, strSubject, "", SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort)

        Catch ex As Exception
            'do nothing 
        Finally
            oEmail = Nothing

        End Try

    End Sub

    ''' <summary>
    ''' Generates an email with optional CC address Log messages formated to explain that Email Results were sent to the specified addresses.
    ''' Errors are added to the log as needed.  Does not update the LastError Property.
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="logMessage"></param>
    ''' <param name="strMailTo"></param>
    ''' <param name="strMailCC"></param>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Added optional parameters to SendMail call
    ''' </remarks>
    Public Overridable Sub LogResults(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, Optional ByVal strMailCC As String = "")
        Dim oEmail As New Email

        Try
            'Modified By LVV 2/18/16 v-7.0.5.0
            If Not oEmail.SendMail(SMTPServer, strMailTo, FromEmail, logMessage & vbCrLf & vbCrLf & Me.DBInfo, strSubject, strMailCC, SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                LogError("Send Email Error", "Unable to email results. " & vbCrLf & "Subject: " & strSubject & vbCrLf & "Message: " & logMessage & vbCrLf & "Mail to: " & strMailTo & vbCrLf & "CC To: " & strMailCC, AdminEmail)
            End If

            Dim strLogMsg As String = "Email Results Sent to " & vbCrLf & "Subject: " & strSubject & vbCrLf & "Message: " & logMessage & vbCrLf & "Mail to: " & strMailTo & vbCrLf & "CC To: " & strMailCC
            Log(strLogMsg)
        Catch ex As Exception
            Me.LastError = ex.Message
            LogException("Send Email Exception", "Unable to email results. " & vbCrLf & "Subject: " & strSubject & vbCrLf & "Message: " & logMessage & vbCrLf & "Mail to: " & strMailTo & vbCrLf & "CC To: " & strMailCC, AdminEmail, ex, Source & ".LogResults Failure")
        Finally
            oEmail = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' Adds formatted exception information to the log file using debug settings (Plain Text). Does not update the LastError property.
    ''' </summary>
    ''' <param name="logMessage">appended to the begining of the formtted exception information</param>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Public Overridable Sub LogException(ByVal logMessage As String, ByVal ex As Exception)
        Log(logMessage & ": " & readExceptionMessage(ex))
    End Sub

    ''' <summary>
    ''' Formats the exception information using debug settings in HTML, updates the LastError property sends an email and adds the message to the log file.
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="logMessage"></param>
    ''' <param name="strMailTo"></param>
    ''' <param name="ex"></param>
    ''' <param name="strHeader"></param>
    ''' <remarks></remarks>
    Public Overridable Sub LogException(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, ByVal ex As Exception, Optional ByVal strHeader As String = "")
        Dim strMsg As String = "<p>" & logMessage & "</p>" & vbCrLf
        If strHeader.Trim.Length > 0 Then
            strMsg = "<h2>" & strHeader & vbCrLf & "</h2>" & strMsg
        End If
        strMsg &= "<hr />" & vbCrLf
        If Me.Debug Then
            strMsg &= ex.ToString & vbCrLf
        Else
            strMsg &= ex.Message & vbCrLf
        End If
        strMsg &= "<hr />" & vbCrLf
        LogError(strSubject, strMsg, strMailTo)
    End Sub

    ''' <summary>
    ''' Formats the message in HTML sends an email, adds the message in the log file, and updates the LastError Property
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="logMessage"></param>
    ''' <param name="strMailTo"></param>
    ''' <param name="exString"></param>
    ''' <param name="strHeader"></param>
    ''' <remarks></remarks>
    Public Overridable Sub LogException(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, ByVal exString As String, Optional ByVal strHeader As String = "")
        Dim strMsg As String = "<p>" & logMessage & "</p>" & vbCrLf
        If strHeader.Trim.Length > 0 Then
            strMsg = "<h2>" & strHeader & vbCrLf & "</h2>" & strMsg
        End If
        strMsg &= "<hr />" & vbCrLf
        If Me.Debug Then
            strMsg &= exString & vbCrLf
        Else
            strMsg &= exString & vbCrLf
        End If
        strMsg &= "<hr />" & vbCrLf
        LogError(strSubject, strMsg, strMailTo)
    End Sub

    ''' <summary>
    ''' The readParameters has been depreciated and is left for backward compatibility only
    ''' the new method is readCommandLineArgs
    ''' </summary>
    ''' <param name="strExe"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function readParameters(ByVal strExe As String) As Integer
        'Note:  Return values
        '1 Success use parameters provided
        '0 Only show help message

        Dim strParameters() As String = System.Environment.GetCommandLineArgs
        Dim strVal As String
        Dim strHelpMsg As String
        Try
            strHelpMsg = vbCrLf _
                & "Usage:" & vbCrLf _
                & strExe & " [/?] " _
                & " [-k:inikey] [-d] "
            strHelpMsg &= vbCrLf & vbCrLf _
                & "Options:" & vbCrLf _
                & "    /?" & vbTab & vbTab & vbTab & "Show this help screen." & vbCrLf _
                & "    -k:inikey" & vbTab & vbTab & "INI File Key (default = NGL)." _
                & "    -d" & vbTab & vbTab & "Debug Flag." _
                & vbCrLf & vbCrLf & "Note:  Spaces are required between parameters but not allowed inside of them."
            For Each strVal In strParameters
                Dim strSwitch As String = Left(strVal, 2)
                Select Case strSwitch
                    Case "/?"
                        Console.WriteLine(strHelpMsg)
                        Console.WriteLine("Press Enter To Continue")
                        Console.ReadLine()
                        Return 0
                    Case "-k"
                        INIKey = strVal.Substring(3)
                    Case "-d"
                        Debug = True
                End Select

            Next
            ' Read the INI File
            If Debug Then
                Console.WriteLine("Ini File = " & APPPath() & "\FreightMaster.ini")
            End If
            Dim objIniFile As New IniFile(APPPath() & "\FreightMaster.ini")
            LogFile = APPPath & "\" & Source & "." & objIniFile.GetString(INIKey, "LogFile", "log")
            AutoRetry = objIniFile.GetInteger(INIKey, "Auto Retry", 3)
            Database = objIniFile.GetString(INIKey, "Database", "")
            DBServer = objIniFile.GetString(INIKey, "DBServer", "")
            AdminEmail = objIniFile.GetString(INIKey, "AdminEmail", "support@nextgeneration.com")
            GroupEmail = objIniFile.GetString(INIKey, "GroupEmail", "support@nextgeneration.com")
            FromEmail = objIniFile.GetString(INIKey, "FromEmail", FromEmail)
            SMTPServer = objIniFile.GetString(INIKey, "SMTPServer", "mail.ngl.local")
            SaveOldLog = objIniFile.GetString(INIKey, "SaveOldLog", SaveOldLog.ToString).ToLower
            KeepLogDays = objIniFile.GetInteger(INIKey, "KeepLogDays", KeepLogDays)
            AuthNbrFTPRoot = objIniFile.GetString(INIKey, "AuthNbrFTPRoot", AuthNbrFTPRoot)
            Return 1

        Catch ex As Exception
            Throw New ApplicationException(Source & " Read Parameters Failure! ", ex)
        End Try


    End Function
    ''' <summary>
    ''' Replaces readParameters; the default logic now only checks for a connection string
    ''' all other parameters are stored in the datbase as Global Parameters
    ''' </summary>
    ''' <param name="strExe"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function readCommandLineArgs(ByVal strExe As String) As Integer
        'Note:  Return values
        '1 Success use parameters provided
        '0 Only show help message

        Dim args() As String = System.Environment.GetCommandLineArgs
        Dim strHelpMsg As String
        Try

            strHelpMsg = vbCrLf _
                & "Usage:" & vbCrLf _
                & strExe & " [/?] " _
                & " [-c " & Chr(34) & "Server=DatabaseServer; Database=FreightMasterDatabase; Integrated Security=SSPI" & Chr(34) & "] [-d] [-v] [-l] "
            strHelpMsg &= vbCrLf & vbCrLf _
                & "Options:" & vbCrLf _
                & "    /?" & vbTab & vbTab & "Show this help screen." & vbCrLf _
                & "    -c " & vbTab & vbTab & "Database Connection String (Required)." & vbCrLf _
                & "    -d" & vbTab & vbTab & "Debug Flag (Optional)." & vbCrLf _
                & "    -v" & vbTab & vbTab & "Verbose Flag (Optional Check with NGL Support)." _
                & "    -l" & vbTab & vbTab & "Legal Entity (Optional Filter to restricted some data by Legal Entity)" _
                & vbCrLf & vbCrLf & "NOTE:  Spaces are required between parameters."
            For i As Integer = 0 To args.Length - 1
                Dim strArg As String = args(i).Replace("/", "-").ToLower
                Select Case strArg
                    Case "-c" 'Connection String Example: "Server=NglSql01P; Database=NglMas2002; Integrated Security=SSPI"
                        If i + 1 < args.Length Then _ConnectionString = args(i + 1)
                    Case "-d"
                        Debug = True
                    Case "-v"
                        Verbose = True
                    Case "-l"
                        Dim sVal As String = ""
                        If i + 1 < args.Length Then sVal = args(i + 1)
                        If Not String.IsNullOrWhiteSpace(sVal) Then LegalEntity = sVal
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
    ''' Method to make adding new Global Task Parameters
    ''' to the getTaskParameters() methods and overrides easier
    ''' </summary>
    ''' <param name="oGTPs"></param>
    ''' <remarks>
    ''' Created By LVV 2/18/16 v-7.0.5.0
    ''' </remarks>
    Public Sub processNewTaskParameters(ByRef oGTPs As DAL.DataTransferObjects.GlobalTaskParameters)
            With oGTPs
                GlobalSMTPUseDefaultCredentials = .GlobalSMTPUseDefaultCredentials
                GlobalSMTPEnableSSL = .GlobalSMTPEnableSSL
                GlobalSMTPTargetName = .GlobalSMTPTargetName
                GlobalSMTPPort = .GlobalSMTPPort
            End With
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 'Modified By LVV 2/18/16 v-7.0.5.0
    '''  Added call to getNewTaskParameters()
    ''' </remarks>
    Public Overridable Function getTaskParameters() As Boolean
        Dim blnRet As Boolean = False
        Try
            'get the parameter settings from the database.
            Dim oSysData As New DAL.NGLSystemDataProvider(ConnectionString)
            Dim oGTPs As DAL.DataTransferObjects.GlobalTaskParameters = oSysData.GetGlobalTaskParameters
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
            End If
            blnRet = True
        Catch ex As Ngl.FreightMaster.Data.DatabaseReadDataException
            LogException("Cannot get task parameters; the database table is not valid or is not available", ex)
        Catch ex As Exception
            Log("Read Command Line Task Parameter Failure: " & readExceptionMessage(ex))
        End Try

        Return blnRet
    End Function

    Public Overridable Sub cloneTaskParameters(ByRef source As NGLCommandLineBaseClass)
        With source
            AdminEmail = .AdminEmail
            AutoRetry = .AutoRetry
            ConnectionString = .ConnectionString
            Database = .Database
            DBServer = .DBServer
            Debug = .Debug
            Verbose = .Verbose
            FromEmail = .FromEmail
            GroupEmail = .GroupEmail
            INIKey = .INIKey
            KeepLogDays = .KeepLogDays
            ResultsFile = .ResultsFile
            SaveOldLog = .SaveOldLog
            SMTPServer = .SMTPServer
            Me.Source = .Source
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
            NEXTRackDatabase = .NEXTRackDatabase
            NEXTRackDatabaseServer = .NEXTRackDatabaseServer
        End With
    End Sub

    ''' <summary>
    ''' Validates that C:\Data\TMSLogs directory exists if not it creates it.
    ''' Opens the log object which creates a log file in the format
    ''' C:\Data\TMSLogs\Source.Database.log  it then checks if a database connection
    ''' can be established.  Finally it checks if the database name has changed and 
    ''' updates the log file information as needed (can change when both a dbname and 
    ''' connection string are provided). 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR7/23/2015 v-7.0.4 moved log file to  C:\Data\TMSLogs
    ''' </remarks>
    Public Overridable Function validateDatabase() As Boolean
        Dim blnRet As Boolean = False

        Try
            'set the default value for the log file

            If Not System.IO.Directory.Exists("C:\Data") Then
                System.IO.Directory.CreateDirectory("C:\Data")
            End If
            If Not System.IO.Directory.Exists("C:\Data\TMSLogs") Then
                System.IO.Directory.CreateDirectory("C:\Data\TMSLogs")
            End If
            LogFile = "C:\Data\TMSLogs\" & Source & "." & Database & ".log"
            openLog()
            If Me.Verbose Then Log("Validating connection string")
            Dim cn As New System.Data.SqlClient.SqlConnection(ConnectionString)
            cn.Open()
            Me.Database = cn.Database
            Me.DBServer = cn.DataSource
            cn.Close()
            blnRet = True
            If LogFile <> "C:\Data\TMSLogs\" & Source & "." & Database & ".log" Then
                'reset the value for the log file if the database name has changed
                LogFile = "C:\Data\TMSLogs\" & Source & "." & Database & ".log"
                closeLog(0)
                openLog()
            End If
            If Me.Debug Then Log("Connection string valid")
        Catch ex As Exception
            Log("The connection string, " & ConnectionString & ", is not valid.  The actual error is: " & vbCrLf & Me.readExceptionMessage(ex))
        End Try
        Return blnRet

    End Function

    Protected Overridable Sub displayParameterData()
        If Debug Then
            Console.WriteLine("Log File: " & LogFile)
            Console.WriteLine("INI Key: " & INIKey)
            Console.WriteLine("Auto Retry: " & AutoRetry)
            Console.WriteLine("Connection String: " & ConnectionString)
            Console.WriteLine("Database: " & Database)
            Console.WriteLine("DB Server: " & DBServer)
            Console.WriteLine("Admin Email: " & AdminEmail)
            Console.WriteLine("Group Email: " & GroupEmail)
            Console.WriteLine("SMTP Server: " & SMTPServer)
            Console.WriteLine("Debug Mode: " & Debug)
            Console.WriteLine("Save Old Log: " & SaveOldLog)
            Console.WriteLine("Keep Log Days: " & KeepLogDays)
            Console.WriteLine("Auth Nbr FTP Root: " & AuthNbrFTPRoot)
            Console.WriteLine("Verbose Mode: " & Verbose)
        End If

    End Sub

    Public Sub generateDataIntegrationFailureAlert(ByVal sERP As String, _
                                                    ByVal enmRetVal As ProcessDataReturnValues, _
                                                    enmIntegration As IntegrationModule, _
                                                    ByVal LastError As String, _
                                                    Optional ByVal blnNotifyAdmin As Boolean = True)
        Try
            Dim strIntegrationType As String = "Unknown"
            Select Case enmIntegration
                Case IntegrationModule.Company
                Case IntegrationModule.Carrier
                    strIntegrationType = "Carrier"
                Case IntegrationModule.Lane
                    strIntegrationType = "Lane"
                Case IntegrationModule.Hazmat
                    strIntegrationType = "Hazmat"
                Case IntegrationModule.PalletType
                    strIntegrationType = "Pallet Type"
                Case IntegrationModule.Order
                    strIntegrationType = "Order"
                Case IntegrationModule.PickList
                    strIntegrationType = "Picklist"
                Case IntegrationModule.APExport
                    strIntegrationType = "Ap Export"
                Case IntegrationModule.Payables
                    strIntegrationType = "Payables"
            End Select
            Dim subject As String = ""
            Dim msg As String = ""
            Select Case enmRetVal
                Case ProcessDataReturnValues.nglDataIntegrationFailure
                    subject = String.Format("Process {0} {1} Integration Error", sERP, strIntegrationType)
                    msg = String.Format("Error Integration Failure! could not process {0} information: {1}", strIntegrationType, LastError)
                Case ProcessDataReturnValues.nglDataIntegrationHadErrors
                    subject = String.Format("Process {0} {1} Integration Error", sERP, strIntegrationType)
                    msg = String.Format("Error Integration Had Errors! could not process some {0} information: {1}", strIntegrationType, LastError)
                Case ProcessDataReturnValues.nglDataValidationFailure
                    subject = String.Format("Process {0} {1} Integration Error", sERP, strIntegrationType)
                    msg = String.Format("Error Data Validation Failure! could not process {0} information: {1}", strIntegrationType, LastError)
            End Select
            If Not String.IsNullOrWhiteSpace(subject) Then
                If blnNotifyAdmin Then
                    LogError(subject, msg, AdminEmail)
                Else
                    LogError(msg)
                End If
                createERPInegrationFailureSubscriptionAlert(subject, enmIntegration, msg)
            End If


        Catch ex As Exception
            'ignore any errors
        End Try
    End Sub

    Public Function createERPInegrationFailureSubscriptionAlert(ByVal Subject As String, _
                                       ByVal enmIntegration As IntegrationModule, _
                                       ByVal CompControl As Integer, _
                                       ByVal CompNumber As Integer, _
                                       Optional CompAlphaCode As String = "", _
                                       Optional ByVal keyString As String = "", _
                                       Optional ByVal keyControl As Long = 0, _
                                       Optional ByVal OrderNumber As String = "", _
                                       Optional ByVal OrderSequence As String = "0", _
                                       Optional ByVal CarrierControl As Integer = 0, _
                                       Optional ByVal CarrierNumber As Integer = 0, _
                                       Optional ByVal CarrierAlphaCode As String = "", _
                                       Optional ByVal Errors As String = "", _
                                       Optional ByVal Warnings As String = "", _
                                       Optional ByVal Messages As String = "") As Boolean
        Dim strBodyMsg As String = ""
        Dim Note1 As String = ""
        Dim Note2 As String = If(String.IsNullOrWhiteSpace(Errors) = False, " Errors: " & Errors, "")
        Dim Note3 As String = If(String.IsNullOrWhiteSpace(Warnings) = False, " Warnings: " & Warnings, "")
        Dim Note4 As String = If(String.IsNullOrWhiteSpace(Messages) = False, " Messages: " & Messages, "")

        If CompNumber <> 0 Then
            Note1 &= " Company Number: " & CompNumber.ToString()
        End If
        If Not String.IsNullOrWhiteSpace(CompAlphaCode) Then
            Note1 &= " Company Alpha Code: " & CompAlphaCode
        End If
        If CarrierNumber <> 0 Then
            Note1 &= " Carrier Number: " & CarrierNumber.ToString()
        End If
        If Not String.IsNullOrWhiteSpace(CarrierAlphaCode) Then
            Note1 &= " Carrier Alpha Code: " & CarrierAlphaCode
        End If
        Select Case enmIntegration
            Case IntegrationModule.Company
                strBodyMsg = "Alert - Could not process a company integration record"
            Case IntegrationModule.Carrier
                strBodyMsg = "Alert - Could not process a carrier integration record"
            Case IntegrationModule.Lane
                strBodyMsg = "Alert - Could not process a Lane integration record"
                Note1 = " Lane Number: " & keyString & Note1
            Case IntegrationModule.Order
                strBodyMsg = "Alert - Could not process an order integration record"
                Note1 = String.Format(" Order Number - Sequence: {0} - {1}", OrderNumber, OrderSequence) & Note1
            Case IntegrationModule.Hazmat
                strBodyMsg = "Alert - Could not process a hazmat integration record"
                If keyControl <> 0 Then
                    Note1 = " Hazmat Number: " & keyControl.ToString() & Note1
                End If
                If Not String.IsNullOrWhiteSpace(keyString) Then
                    Note1 = " Hazmat Code: " & keyString & Note1
                End If
            Case IntegrationModule.PalletType
                strBodyMsg = "Alert - Could not process a pallet type integration record"
                If keyControl <> 0 Then
                    Note1 = " Pallet Type Number: " & keyControl.ToString() & Note1
                End If
                If Not String.IsNullOrWhiteSpace(keyString) Then
                    Note1 = " Pallet Type: " & keyString & Note1
                End If
            Case IntegrationModule.PickList
                strBodyMsg = "Alert - Could not process a pick list integration record"
                If keyControl <> 0 Then
                    Note1 = " Pick List Control Number: " & keyControl.ToString() & Note1
                End If
                Note1 &= String.Format(" Order Number - Sequence: {0} - {1} ", OrderNumber, OrderSequence)
            Case IntegrationModule.APExport
                strBodyMsg = "Alert - Could not process an AP Export integration record"
                If keyControl <> 0 Then
                    Note1 = " AP Control Number: " & keyControl.ToString() & Note1
                End If
                If Not String.IsNullOrWhiteSpace(keyString) Then
                    Note1 = " Freight Bill Number: " & keyString & Note1
                End If
                If Not String.IsNullOrEmpty(OrderNumber) Then Note1 &= String.Format(" Order Number - Sequence: {0} - {1} ", OrderNumber, OrderSequence)
            Case IntegrationModule.Payables
                strBodyMsg = "Alert - Could not process a payables integration record"
                If keyControl <> 0 Then
                    Note1 = " Payable Number: " & keyControl.ToString() & Note1
                End If
                If Not String.IsNullOrWhiteSpace(keyString) Then
                    Note1 = " Freight Bill Number: " & keyString & Note1
                End If
                If Not String.IsNullOrEmpty(OrderNumber) Then Note1 &= String.Format(" Order Number - Sequence: {0} - {1} ", OrderNumber, OrderSequence)
        End Select

        Dim Body As String = String.Concat(strBodyMsg, " using: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        Dim NGLtblAlertMessageData As New Ngl.FreightMaster.Data.NGLtblAlertMessageData(Me.WCFParameters)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertERPInegrationFailure", "Alert ERP Integration Failure", Subject, Body, CompControl, CompNumber, CarrierControl, CarrierNumber, Note1, Note2, Note3, Note4, "")
    End Function

    '''<summary>
    ''' Add an ERP Integration Failure Subscription Alert Message into tblAlertMessage
    ''' </summary>
    ''' <param name="Subject"></param>
    ''' <param name="enmIntegration"></param>
    ''' <param name="Errors"></param>
    ''' <param name="Warnings"></param>
    ''' <param name="Messages"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.104 on 04/18/2017
    '''   added truncation logic to Subject and Notes 1 through 5 to deal with 255 character limit
    ''' </remarks>
    Public Function createERPInegrationFailureSubscriptionAlert(ByVal Subject As String, _
                                       ByVal enmIntegration As IntegrationModule, _
                                       Optional ByVal Errors As String = "", _
                                       Optional ByVal Warnings As String = "", _
                                       Optional ByVal Messages As String = "") As Boolean
        Dim strBodyMsg As String = ""
        Dim Note1 As String = " Could not process the integration request."
        'begin changes  by RHR for v-7.0.6.104 on 04/18/2017
        Dim Note2 As String = If(String.IsNullOrWhiteSpace(Errors) = False, " Errors: " & Left(Errors, 235), "")
        Dim Note3 As String = If(String.IsNullOrWhiteSpace(Warnings) = False, " Warnings: " & Left(Warnings, 235), "")
        Dim Note4 As String = If(String.IsNullOrWhiteSpace(Messages) = False, " Messages: " & Left(Messages, 235), "")
        'end changes  by RHR for v-7.0.6.104 on 04/18/2017
        Select Case enmIntegration
            Case IntegrationModule.Company
                strBodyMsg = "Alert - Could not process a company integration record"
            Case IntegrationModule.Carrier
                strBodyMsg = "Alert - Could not process a carrier integration record"
            Case IntegrationModule.Lane
                strBodyMsg = "Alert - Could not process a Lane integration record"
            Case IntegrationModule.Order
                strBodyMsg = "Alert - Could not process an order integration record"
            Case IntegrationModule.Hazmat
                strBodyMsg = "Alert - Could not process a hazmat integration record"
            Case IntegrationModule.PalletType
                strBodyMsg = "Alert - Could not process a pallet type integration record"
            Case IntegrationModule.PickList
                strBodyMsg = "Alert - Could not process a pick list integration record"
            Case IntegrationModule.APExport
                strBodyMsg = "Alert - Could not process an AP Export integration record"
            Case IntegrationModule.Payables
                strBodyMsg = "Alert - Could not process a payables integration record"
        End Select
        'begin changes  by RHR for v-7.0.6.104 on 04/18/2017
        ' we use original strings before truncation in body
        Dim Body As String = String.Concat(strBodyMsg, " using: ", vbCrLf, Note1, vbCrLf, vbCrLf, Errors, vbCrLf, vbCrLf, Warnings, vbCrLf, vbCrLf, Messages)
        'end changes  by RHR for v-7.0.6.104 on 04/18/2017
        Dim NGLtblAlertMessageData As New Ngl.FreightMaster.Data.NGLtblAlertMessageData(Me.WCFParameters)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertERPInegrationFailure", "Alert ERP Integration Failure", Subject, Body, 0, 0, 0, 0, Note1, Note2, Note3, Note4, "")
    End Function


#End Region

End Class
