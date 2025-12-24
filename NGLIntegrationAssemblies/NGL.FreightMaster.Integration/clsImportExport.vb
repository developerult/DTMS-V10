Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports System.Reflection
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data


<Serializable()>
Public Class clsImportExport

#Region "Constructors"

    Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Constructor that fills the config settings and local properties
    ''' </summary>
    ''' <param name="config"></param>
    ''' <remarks>
    ''' Modifed by RHR for v-7.0.6.105 on 6/3/2017
    '''   added WCF settings
    ''' </remarks>
    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New()
        Me.oConfig = config
        With oConfig
            Me.AdminEmail = .AdminEmail
            Me.Retry = .AutoRetry
            Me.Database = .Database
            Me.DBServer = .DBServer
            Me.Debug = .Debug
            Me.FromEmail = .FromEmail
            Me.GroupEmail = .GroupEmail
            Me.KeepLogDays = .KeepLogDays
            Me.LogFile = .LogFile
            Me.SaveOldLog = .SaveOldLog
            Me.SMTPServer = .SMTPServer
            Me.Source = .Source
            Me.mstrConnection = .ConnectionString
            Me.AuthorizationCode = .WSAuthCode
            Me.WCFAuthCode = .WCFAuthCode
            Me.WCFURL = .WCFURL
            Me.WCFTCPURL = .WCFTCPURL
        End With
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

        MyBase.New()

        Me.AdminEmail = admin_email
        Me.Retry = auto_retry
        Me.Database = database_catalog
        Me.DBServer = db_server
        Me.Debug = debug_mode
        Me.FromEmail = from_email
        Me.GroupEmail = group_email
        Me.SMTPServer = smtp_server
        Me.mstrConnection = connection_string

        fillConfig()

    End Sub

#End Region

#Region "Object Overrides"

    ' Return a copy of this object by making a simple field copy.
    Public Function Copy() As clsImportExport
        Return CType(Me.MemberwiseClone(), clsImportExport)
    End Function

    '' Return the objects value as a string.
    'Public Overrides Function ToString() As String
    '    Return ""
    'End Function

#End Region

#Region "Protected Properties"

    Protected oConfig As New Ngl.FreightMaster.Core.UserConfiguration

    Protected mstrCreatedDate As String = Date.Now.ToString
    Protected mstrCreateUser As String = "NGLSystem"
    Protected mintImportTypeKey As Integer = 0
    Protected mioLog As System.IO.StreamWriter
    Protected mobjLog As clsLog
    Protected mblnDebug As Boolean = False
    Protected mstrLogFile As String = ""
    Protected mblnSilent As Boolean = True
    Protected mintResults As Integer = 0
    Protected mblnSharedDB As Boolean = False

    Private _strITEmailMsg As String = ""
    Public Property ITEmailMsg() As String
        Get
            Return _strITEmailMsg
        End Get
        Set(ByVal value As String)
            _strITEmailMsg = value
        End Set
    End Property

    Private _strITNoLaneEmailMsg As String = ""
    Protected Property ITNoLaneEmailMsg() As String
        Get
            Return _strITNoLaneEmailMsg
        End Get
        Set(ByVal value As String)
            _strITNoLaneEmailMsg = value
        End Set
    End Property


    Private _strGroupEmailMsg As String = ""
    Public Property GroupEmailMsg() As String
        Get
            Return _strGroupEmailMsg
        End Get
        Set(ByVal value As String)
            _strGroupEmailMsg = value
        End Set
    End Property


#End Region

#Region "Public Properties"

    Public ReadOnly Property DBInfo() As String
        Get
            Return "Server: " & Me.DBServer & vbCrLf & "Database: " & Me.Database
        End Get
    End Property



    Protected mstrWCFURL As String = ""
    Public Property WCFURL() As String
        Get
            Return mstrWCFURL
        End Get
        Set(ByVal value As String)
            mstrWCFURL = value
        End Set
    End Property

    Protected mstrWCFTCPURL As String = ""
    Public Property WCFTCPURL() As String
        Get
            Return mstrWCFTCPURL
        End Get
        Set(ByVal value As String)
            mstrWCFTCPURL = value
        End Set
    End Property

    Protected mstrWCFAuthCode As String = "NGLSystem"
    Public Property WCFAuthCode() As String
        Get
            If String.IsNullOrWhiteSpace(mstrWCFAuthCode) Then mstrWCFAuthCode = "NGLSystem"
            Return mstrWCFAuthCode
        End Get
        Set(ByVal value As String)
            mstrWCFAuthCode = value
        End Set
    End Property

    Protected mstrAuthorizationCode As String = ""
    Public Property AuthorizationCode() As String
        Get
            Return mstrAuthorizationCode
        End Get
        Set(ByVal value As String)
            mstrAuthorizationCode = value
        End Set
    End Property


    Protected mblnSaveOldLog As Boolean = True
    Public Property SaveOldLog() As Boolean
        Get
            Return mblnSaveOldLog
        End Get
        Set(ByVal value As Boolean)
            mblnSaveOldLog = value
        End Set
    End Property

    Protected mintKeepLogDays As Integer = "30"
    Public Property KeepLogDays() As Integer
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

    Protected mstrFromEmail As String = ""
    Public Property FromEmail() As String
        Get
            Return mstrFromEmail
        End Get
        Set(ByVal value As String)
            mstrFromEmail = value
        End Set
    End Property

    Protected mstrHeaderName As String = ""
    Public Property HeaderName() As String
        Get
            Return mstrHeaderName
        End Get
        Set(ByVal Value As String)
            mstrHeaderName = Value
        End Set
    End Property

    Protected mstrItemName As String = ""
    Public Property ItemName() As String
        Get
            Return mstrItemName
        End Get
        Set(ByVal Value As String)
            mstrItemName = Value
        End Set
    End Property

    Protected mstrCalendarName As String = ""
    Public Property CalendarName() As String
        Get
            Return mstrCalendarName
        End Get
        Set(ByVal Value As String)
            mstrCalendarName = Value
        End Set
    End Property

    Protected mstrSource As String = ""
    Public Property Source() As String
        Get
            Dim strRet As String = mstrSource
            If Me.AuthorizationCode.Trim.Length > 0 Then
                If InStr(strRet, Me.AuthorizationCode) = 0 Then
                    strRet &= " - using - " & Me.AuthorizationCode
                End If
            End If
            Return strRet
        End Get
        Set(ByVal Value As String)
            mstrSource = Value
        End Set
    End Property

    Private _fileFilter As String
    Public Property FileFilter() As String
        Get
            Return _fileFilter
        End Get
        Set(ByVal value As String)
            _fileFilter = value
        End Set
    End Property

    Private _strLastErr As String = ""
    Public Property LastError() As String
        Get
            Return _strLastErr

        End Get
        Protected Set(ByVal Value As String)
            _strLastErr = Value
        End Set
    End Property

    Private _intRetry As Integer = 3
    Public Property Retry() As Integer
        Get
            Return _intRetry

        End Get
        Set(ByVal Value As Integer)
            _intRetry = Value
        End Set
    End Property

    Private mstrSMTPServer As String = ""
    Public Property SMTPServer() As String
        Get
            SMTPServer = mstrSMTPServer

        End Get
        Set(ByVal Value As String)
            mstrSMTPServer = Value
        End Set
    End Property

    Private mstrGroupEmail As String = ""
    Public Property GroupEmail() As String
        Get
            GroupEmail = mstrGroupEmail
        End Get
        Set(ByVal Value As String)
            mstrGroupEmail = Value
        End Set
    End Property

    Private mstrAdminEmail As String = ""
    Public Property AdminEmail() As String
        Get
            AdminEmail = mstrAdminEmail
        End Get
        Set(ByVal Value As String)
            mstrAdminEmail = Value
        End Set
    End Property

    Private mintTotalRecords As Integer = 0
    Public Property TotalRecords() As Integer
        Get
            TotalRecords = mintTotalRecords
        End Get
        Protected Set(ByVal Value As Integer)
            mintTotalRecords = Value
        End Set
    End Property

    Private mintRecordErrors As Integer = 0
    Public Property RecordErrors() As Integer
        Get
            RecordErrors = mintRecordErrors
        End Get
        Protected Set(ByVal Value As Integer)
            mintRecordErrors = Value
        End Set
    End Property

    Private mintStatusUpdateErrors As Integer = 0
    Public Property StatusUpdateErrors() As Integer
        Get
            Return mintStatusUpdateErrors
        End Get
        Protected Set(ByVal Value As Integer)
            mintStatusUpdateErrors = Value
        End Set
    End Property

    Private mintItemErrors As Integer = 0
    Public Property ItemErrors() As Integer
        Get
            ItemErrors = mintItemErrors
        End Get
        Protected Set(ByVal Value As Integer)
            mintItemErrors = Value
        End Set
    End Property

    Private mintCalendarErrors As Integer = 0
    Public Property CalendarErrors() As Integer
        Get
            CalendarErrors = mintCalendarErrors
        End Get
        Protected Set(ByVal Value As Integer)
            mintCalendarErrors = Value
        End Set
    End Property

    Public Property Results() As Integer
        Get
            Return mintResults
        End Get
        Set(ByVal Value As Integer)
            mintResults = Value
        End Set
    End Property


    Public Property Silent() As Boolean
        Get
            Return mblnSilent
        End Get
        Set(ByVal Value As Boolean)
            mblnSilent = Value
        End Set
    End Property

    Public Property LogFile() As String
        Get
            Return mstrLogFile
        End Get
        Set(ByVal Value As String)
            mstrLogFile = Value
        End Set
    End Property


    Public Property Debug() As Boolean
        Get
            Return mblnDebug
        End Get
        Set(ByVal Value As Boolean)
            mblnDebug = Value
        End Set
    End Property

    Public Property CreatedDate() As String
        Get
            Return mstrCreatedDate
        End Get
        Set(ByVal Value As String)
            mstrCreatedDate = Value
        End Set
    End Property

    Public Property CreateUser() As String
        Get
            Return mstrCreateUser
        End Get
        Set(ByVal Value As String)
            mstrCreateUser = Value
        End Set
    End Property

    Public Property ImportTypeKey() As IntegrationTypes
        Get
            Return mintImportTypeKey
        End Get
        Set(ByVal Value As IntegrationTypes)
            mintImportTypeKey = Value
        End Set
    End Property

    Private mstrConnection As String = ""
    Public Property DBConnection() As String
        Get
            If mstrConnection.Trim.Length < 1 Then
                mstrConnection = String.Format("Server={0};Database={1};Integrated Security=SSPI", Me.DBServer, Me.Database)
            End If
            Return mstrConnection
        End Get
        Set(ByVal value As String)
            mstrConnection = value
        End Set
    End Property


    Public Property ConnectionString() As String
        Get
            If mstrConnection.Trim.Length < 1 Then
                mstrConnection = String.Format("Server={0};Database={1};Integrated Security=SSPI", Me.DBServer, Me.Database)
            End If
            Return mstrConnection
        End Get
        Set(ByVal value As String)
            mstrConnection = value
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


    Private mstrDBServer As String = ""
    Public Property DBServer() As String
        Get
            Return mstrDBServer
        End Get
        Set(ByVal value As String)
            mstrDBServer = value
        End Set
    End Property

    Private mstrDatabase As String = ""
    Public Property Database() As String
        Get
            Return mstrDatabase
        End Get
        Set(ByVal value As String)
            mstrDatabase = value
        End Set
    End Property


    Private _DALParameters As DAL.WCFParameters
    Public Property DALParameters() As DAL.WCFParameters
        Get
            If _DALParameters Is Nothing Then
                _DALParameters = New DAL.WCFParameters With {
                .UserName = "System Download",
                .Database = Me.Database,
                .DBServer = Me.DBServer,
                .ConnectionString = Me.ConnectionString,
                .WCFAuthCode = "NGLSystem",
                .ValidateAccess = False
            }
            End If
            Return _DALParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _DALParameters = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' fill config object with local properties
    ''' </summary>
    ''' <remarks>
    ''' Modifed by RHR for v-7.0.6.105 on 6/3/2017
    '''   added WCF settings
    ''' </remarks>
    Public Sub fillConfig()
        Try
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.Retry
                .Database = Me.Database
                .DBServer = Me.DBServer
                .Debug = Me.Debug
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .KeepLogDays = Me.KeepLogDays
                .LogFile = Me.LogFile
                .SaveOldLog = Me.SaveOldLog
                .SMTPServer = Me.SMTPServer
                .Source = mstrSource
                .ConnectionString = Me.mstrConnection
                .WCFAuthCode = Me.WCFAuthCode
                .WCFURL = Me.WCFURL
                .WCFTCPURL = Me.WCFTCPURL
            End With

        Catch ex As Exception
            Throw New ApplicationException(Source & " Fill Configuration Failure", ex)
        End Try
    End Sub

    ''' <summary>
    ''' copy local properties from one clsImportExport object to another
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Modifed by RHR for v-7.0.6.105 on 6/3/2017
    '''   added WCF settings
    ''' </remarks>
    Public Sub shareSettings(ByRef e As clsImportExport)
        With e
            AdminEmail = .AdminEmail
            FromEmail = .FromEmail
            GroupEmail = .GroupEmail
            Retry = .Retry
            SMTPServer = .SMTPServer
            DBServer = .DBServer
            Database = .Database
            DBConnection = .DBConnection
            AuthorizationCode = .AuthorizationCode
            DBCon = .DBCon
            Debug = .Debug
            Silent = .Silent
            mblnSharedDB = True
            WCFAuthCode = .WCFAuthCode
            WCFURL = .WCFURL
            WCFTCPURL = .WCFTCPURL
        End With
    End Sub

    Public Sub closeConnection()
        If Not mblnSharedDB Then
            Try
                Me.DBCon.Close()
                'Modified By LVV on 8/11/16 for v-7.0.5.102 Task #6
                If Me.Debug Then
                    Log("DB Connection Closed")
                End If
                'Log("DB Connection Closed")
            Catch ex As Exception
                'throw away any errors while closing the database
            End Try
        End If
    End Sub

    Public Function openConnection(Optional ByVal blnNoLog As Boolean = False) As Boolean
        If mobjCon Is Nothing Then
            mobjCon = New System.Data.SqlClient.SqlConnection
        End If
        If mstrConnection.Trim.Length < 1 Then
            mstrConnection = "Data Source=" & DBServer & ";" 'Add Server Name
            mstrConnection &= "Initial Catalog=" & Database & ";" 'Add Database Name
            mstrConnection &= "Integrated Security=True"
        End If

        Try
            If mobjCon.State = ConnectionState.Open Then
                Return True
            Else
                mobjCon.ConnectionString = mstrConnection
                mobjCon.Open()
                'Modified By LVV on 8/11/16 for v-7.0.5.102 Task #6
                If Me.Debug Then
                    Log("DB Connection Open")
                End If
                'Log("DB Connection Open")
            End If
            Return True

        Catch ex As System.Data.SqlClient.SqlException
            Dim strMsg As String = ""
            Dim i As Integer = 0
            If ex.Errors(i).Class.ToString() = "14" Or ex.Errors(i).Class.ToString() = "20" Then
                strMsg = "Login Error Number: " & ex.Errors(i).Class.ToString() & ControlChars.NewLine & ex.Errors(i).Message
            ElseIf ex.Errors(i).Class.ToString() = 10 Then
                strMsg = "Login Error Number 10: Windows Authentication Failure."
            Else
                strMsg = "Login Error Number " & ex.Errors(i).Class.ToString() & ControlChars.NewLine & ex.Message
            End If
            If Not blnNoLog Then
                LogError(Source & " Database Login Failure", strMsg, AdminEmail)
            End If
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw New ApplicationException("NGL.FreightMaster.Integration.openConnection Failure! ", ex)
        End Try
        Return False

    End Function

    Public Function getNewConnection(Optional ByVal blnNoLog As Boolean = False) As System.Data.SqlClient.SqlConnection

        Dim objcon As New System.Data.SqlClient.SqlConnection
        If mstrConnection.Trim.Length < 1 Then
            mstrConnection = "Data Source=" & DBServer & ";" 'Add Server Name
            mstrConnection &= "Initial Catalog=" & Database & ";" 'Add Database Name
            mstrConnection &= "Integrated Security=True"
        End If

        Try

            objcon.ConnectionString = mstrConnection
            objcon.Open()



        Catch ex As System.Data.SqlClient.SqlException
            Dim strMsg As String = ""
            Dim i As Integer = 0
            If ex.Errors(i).Class.ToString() = "14" Or ex.Errors(i).Class.ToString() = "20" Then
                strMsg = "Login Error Number: " & ex.Errors(i).Class.ToString() & ControlChars.NewLine & ex.Errors(i).Message
            ElseIf ex.Errors(i).Class.ToString() = 10 Then
                strMsg = "Login Error Number 10: Windows Authentication Failure."
            Else
                strMsg = "Login Error Number " & ex.Errors(i).Class.ToString() & ControlChars.NewLine & ex.Message
            End If
            If Not blnNoLog Then
                LogError(Source & " Database Login Failure", strMsg, AdminEmail)
            End If
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw New ApplicationException("NGL.FreightMaster.Integration.getNewConnection Failure! ", ex)
        End Try
        Return objcon

    End Function


    Public Sub openLog()
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

    Public Sub closeLog(ByVal intReturn As Integer)
        Try
            If IsNothing(mobjLog) Then
                Exit Sub
            End If
            mobjLog.closeLog(intReturn, mioLog)
        Catch ex As Exception
            'ignore any errors when closing the log file
        End Try
    End Sub

    Public Sub Log(ByVal logMessage As String)


        If Me.Debug Then
            Console.WriteLine(logMessage)
        End If

        Dim strSQL As String = "INSERT INTO [dbo].[tblLog] " _
       & "([LogMessage]" _
       & " ,[LogTime]" _
       & " ,[LogUser]" _
       & " ,[LogSource])" _
       & " VALUES " _
       & " ('" & padSpaces(padQuotes(logMessage)) _
       & "','" & Date.Now.ToString _
       & "','" & CreateUser _
       & "','" & Source & "')"
        Dim oCon As System.Data.SqlClient.SqlConnection
        Dim cmd As New SqlCommand
        Try
            oCon = getNewConnection(True)
            cmd = New SqlCommand(strSQL, oCon)
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            'ignore any errors while writing to the log
            If Me.Debug Then
                Console.WriteLine("Save Log Error: " & ex.ToString)
            End If
        Finally
            Try
                cmd.Cancel()
                cmd = Nothing
            Catch ex As Exception

            End Try
            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon.Close()
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub

    Public Sub AddToGroupEmailMsg(ByVal msg As String)
        GroupEmailMsg = String.Concat(GroupEmailMsg, vbCrLf, vbCrLf, msg)
    End Sub

    Public Sub AddToITEmailMsg(ByVal msg As String)
        ITEmailMsg = String.Concat(ITEmailMsg, vbCrLf, vbCrLf, msg)
    End Sub

    Public Sub LogError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String)
        Me.LastError = logMessage
        If InStr(strSubject, Source) = 0 Then
            strSubject &= " " & Source
        End If
        Try

            If Not SendMail(SMTPServer, strMailTo, FromEmail, String.Format("{0}{1}{2}", logMessage, gcHTMLNEWLINE, DBInfo), strSubject) Then
                Log("Send Email Error:  Could not send message to " & strMailTo)
            Else
                Log(String.Format("Email Notice Sent to {0}{1} Subject: {2}{3}Msg: {4}{5}{6}", strMailTo, gcHTMLNEWLINE, strSubject, gcHTMLNEWLINE, logMessage, gcHTMLNEWLINE, DBInfo))
            End If

        Catch ex As Exception
            'ignore any errors here
        End Try


    End Sub

    Public Sub LogGroupEmailError(ByVal strSubject As String, ByVal logMessage As String)
        Dim strMailTo As String
        If GroupEmail.Trim.Length > 0 Then
            strMailTo = GroupEmail.Trim
        Else
            Log("Send GroupEmail Error:  GroupEmail address invalid. Using AdminEmail.")
            LogAdminEmailError(strSubject, logMessage)
            Return
        End If
        LogError(strSubject, logMessage, strMailTo)

    End Sub

    Public Sub LogAdminEmailError(ByVal strSubject As String, ByVal logMessage As String)
        Dim strMailTo As String
        If AdminEmail.Trim.Length > 0 Then
            strMailTo = AdminEmail.Trim
        Else
            Log("Send AdminEmail Error:  AdminEmail address invalid. The following message could not be transmitted: " & logMessage)
            Return
        End If
        LogError(strSubject, logMessage, strMailTo)

    End Sub

    Public Function SendMail(ByVal strServer As String, ByVal strTo As String, ByVal strFrom As String, ByVal strBody As String, ByVal strSubject As String) As Boolean
        Return CreateNGLEmailJob(strFrom, strTo, "", strSubject, strBody)

    End Function

    Public Sub LogException(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, ByVal ex As Exception, Optional ByVal strHeader As String = "")
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
    ''' Read the exception details and returns the message,  test the debug flag to determine how must information to return.
    ''' </summary>
    ''' <param name="oException"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 7/5/2017
    '''   added additional inner exception message when debug is true
    ''' </remarks>
    Public Function readExceptionMessage(ByRef oException As Exception) As String
        Dim strRet As String = ""
        Try
            If Not oException Is Nothing Then
                If Me.Debug Then
                    strRet = oException.ToString
                    If Not oException.InnerException Is Nothing Then
                        strRet &= oException.InnerException.ToString()
                    End If
                Else
                    strRet = oException.Message
                End If
            End If
        Catch ex As Exception
            'do nothing
        End Try

        Return strRet
    End Function



#End Region

#Region "Protected Methods"

    'Protected Function openFileDbConnection( _
    '    ByRef objImportCon As ADODB.Connection, _
    '    ByVal strDataPath As String) As Boolean
    '    Dim Ret As Boolean = False
    '    Try
    '        Dim intRetryCt As Integer = 0
    '        Do
    '            intRetryCt += 1
    '            Try
    '                'build connection string for import file
    '                Dim strConnection As String = "Driver={Microsoft Text Driver (*.txt; *.csv)}; Dbq=" & strDataPath & ";Extensions=asc,csv,tab,txt;HDR=YES;Persist Security Info=False"
    '                'create a new connection
    '                objImportCon = New ADODB.Connection
    '                With objImportCon
    '                    .CursorLocation = ADODB.CursorLocationEnum.adUseClient
    '                    .Open(strConnection)
    '                End With
    '                Return True
    '            Catch ex As Exception
    '                If intRetryCt > Me.Retry Then
    '                    LogError(Source & " openFileDbConnection Failure", "Could not create text db connection with schema file in  " & strDataPath & " folder : " & ex.ToString, AdminEmail)
    '                Else
    '                    Log("openFileDbConnection Failure Retry = " & intRetryCt.ToString)
    '                End If
    '            End Try
    '            'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
    '        Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
    '    Catch ex As Exception
    '        LogError(Source & " openFileDbConnection Failure", "Could not create text db connection with schema file in  " & strDataPath & " folder : " & ex.ToString, AdminEmail)
    '    End Try
    '    Return Ret
    'End Function

    Public Function getParValue(ByVal strKey As String, Optional ByVal CompControl As Integer = 0) As Double
        Dim dblRet As Double = 0
        Try
            Me.fillConfig()
            Dim oParameter As New Ngl.FreightMaster.Core.Model.Parameter(Me.oConfig)
            oParameter.ConnectionString = Me.DBConnection

            If Not oParameter.testConnection() Then
                Log(Source & " Database Connection Failure: " & oParameter.LastError)
                Return 0
            End If
            dblRet = oParameter.getParValue(strKey, CompControl)
        Catch ex As Exception
            Throw New ApplicationException(Source & " Read Parameter Value Failure", ex)
        End Try
        Return dblRet

    End Function


    Public Function getParText(ByVal strKey As String, Optional ByVal CompControl As Integer = 0) As String
        Dim strRet As String = ""
        Try
            Me.fillConfig()
            Dim oParameter As New Ngl.FreightMaster.Core.Model.Parameter(Me.oConfig)
            oParameter.ConnectionString = Me.DBConnection
            If Not oParameter.testConnection() Then
                Log(Source & " Database Connection Failure: " & oParameter.LastError)
                Return 0
            End If
            strRet = oParameter.getParText(strKey, CompControl)
        Catch ex As Exception
            Throw New ApplicationException(Source & " Read Parameter Text Failure", ex)
        End Try
        Return strRet

    End Function

    Protected Sub assignRowToRow(ByRef source As DataRow,
                                    ByVal sourceField As String,
                                    ByRef result As DataRow,
                                    ByRef resultField As String,
                                    ByVal defaultvalue As Object)
        If Not source.Item(sourceField) Is Nothing AndAlso Not source.Item(sourceField) Is DBNull.Value Then
            result.Item(resultField) = source.Item(sourceField)
        Else
            If Not defaultvalue Is Nothing AndAlso Not defaultvalue Is DBNull.Value Then
                result.Item(resultField) = defaultvalue
            Else
                result.Item(resultField) = DBNull.Value
            End If
        End If
    End Sub

    Protected Function getScalarValue(ByVal strSQL As String, Optional ByVal strCaller As String = "Data Import Export getScalarValue", Optional ByVal blnEmailErrors As Boolean = True) As String
        Dim strRet As String = ""

        Dim oCon As New System.Data.SqlClient.SqlConnection
        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
        Try
            oCon = getNewConnection(False)
            strRet = oQuery.getScalarValue(oCon, strSQL, Retry)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            If blnEmailErrors Then ITEmailMsg &= "<br />" & Source & vbCrLf & strCaller & " Failure :<hr /> " & vbCrLf & readExceptionMessage(ex) & "<hr />" & vbCrLf
            Log(strCaller & " failed to execute SQL String without success: " & readExceptionMessage(ex))
        Catch ex As Ngl.Core.DatabaseLogInException
            If blnEmailErrors Then ITEmailMsg &= "<br />" & Source & vbCrLf & strCaller & " Log In Failure : " & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log(strCaller & ": " & readExceptionMessage(ex))
        Catch ex As Ngl.Core.DatabaseInvalidException
            If blnEmailErrors Then ITEmailMsg &= "<br />" & Source & vbCrLf & strCaller & " Database Access Failure : " & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log(strCaller & ": " & readExceptionMessage(ex))
        Catch ex As Exception
            If blnEmailErrors Then ITEmailMsg &= "<br />" & Source & " Failure: " & strCaller & ", attempted to execute sql string without success:<hr /> " & vbCrLf & readExceptionMessage(ex) & "<hr />" & vbCrLf
            Log(strCaller & " failed to execute SQL String: " & strSQL & ". " & readExceptionMessage(ex))
        Finally
            Try
                oQuery = Nothing
            Catch ex As Exception

            End Try
            Try
                oCon.Close()
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try

        Return strRet

    End Function

    Public Function executeSQLQuery(ByVal strSQL As String, Optional ByVal strCaller As String = "Data Import Export executeSQLQuery", Optional ByVal blnEmailErrors As Boolean = True, Optional ByVal blnUseNewConnection As Boolean = False) As Boolean
        Dim blnRet As Boolean = False
        Dim oCon As New System.Data.SqlClient.SqlConnection
        Dim oQuery As New Ngl.Core.Data.Query(Me.DBConnection)
        Try

            oCon = getNewConnection(False)
            blnRet = oQuery.executeSQLQuery(oCon, strSQL, Retry)

        Catch ex As Ngl.Core.DatabaseRetryExceededException
            If blnEmailErrors Then ITEmailMsg &= "<br />" & Source & vbCrLf & strCaller & " Failure :<hr /> " & vbCrLf & readExceptionMessage(ex) & "<hr />" & vbCrLf
            Log(strCaller & " failed to execute SQL String without success: " & readExceptionMessage(ex))
        Catch ex As Ngl.Core.DatabaseLogInException
            If blnEmailErrors Then ITEmailMsg &= "<br />" & Source & vbCrLf & strCaller & " Log In Failure : " & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log(strCaller & ": " & readExceptionMessage(ex))
        Catch ex As Ngl.Core.DatabaseInvalidException
            If blnEmailErrors Then ITEmailMsg &= "<br />" & Source & vbCrLf & strCaller & " Database Access Failure : " & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log(strCaller & ": " & readExceptionMessage(ex))

        Catch ex As Exception
            If blnEmailErrors Then ITEmailMsg &= "<br />" & Source & " Failure: " & strCaller & ", attempted to execute sql string without success:<hr /> " & vbCrLf & strSQL & "<hr />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log(strCaller & " failed to execute SQL String: " & strSQL & ". " & readExceptionMessage(ex))
        Finally
            Try
                oQuery = Nothing
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

    Public Function CreateNGLEmailJob(ByVal MailFrom As String,
                                ByVal EmailTo As String,
                                ByVal CCEmail As String,
                                ByVal Subject As String,
                                ByVal Body As String) As Boolean
        Dim blnRet As Boolean = False
        'Modified by RHR 2/28/2013  may cause a DB connection to not close when called from an Async process
        'We now create a new connection for this method and close it in Finally
        Dim oCon As System.Data.SqlClient.SqlConnection
        Dim objCom As New SqlCommand
        Try

            Dim lngErrNumber As Long
            Dim strRetVal As String = ""
            oCon = getNewConnection(True)
            With objCom
                .Connection = oCon
                .CommandTimeout = 3600
                .Parameters.AddWithValue("@MailFrom", Left(MailFrom, 255))
                .Parameters.AddWithValue("@EmailTo", Left(EmailTo, 500))
                .Parameters.AddWithValue("@CCEmail", Left(CCEmail, 500))
                .Parameters.AddWithValue("@Subject", Left(Subject, 100))
                .Parameters.AddWithValue("@Body", Left(Body, 4000))
                .Parameters.AddWithValue("@UserName", "NGLSystem")
                .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 2500)
                .Parameters("@RetMsg").Direction = ParameterDirection.Output
                .Parameters.Add("@ErrNumber", SqlDbType.BigInt)
                .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                .CommandText = "spGenerateEmail"
                .CommandType = CommandType.StoredProcedure
                .ExecuteNonQuery()
                strRetVal = Trim(.Parameters("@RetMsg").Value.ToString)
                If IsDBNull(.Parameters("@ErrNumber").Value) Then
                    lngErrNumber = 0
                Else
                    lngErrNumber = .Parameters("@ErrNumber").Value
                End If
            End With
            If lngErrNumber <> 0 Then
                Log("NGL.FreightMaster.Integration.clsImportExport.CreateNGLEmailJob Failed!  " & strRetVal & "  ----  " & Body)
            Else
                blnRet = True
            End If

        Catch ex As Exception
            Log("NGL.FreightMaster.Integration.clsImportExport.CreateNGLEmailJob Error!  " & readExceptionMessage(ex) & "  ----  " & Body)
        Finally
            Try
                objCom.Cancel()
                objCom = Nothing
            Catch ex As Exception

            End Try
            Try
#Disable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon.Close()
#Enable Warning BC42104 ' Variable 'oCon' is used before it has been assigned a value. A null reference exception could result at runtime.
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet

    End Function


#End Region

#Region " Shared Methods"


    Protected Shared Function CopyMatchingFields(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String), Optional ByRef strMsg As String = "") As Object
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
                                        If Not Int32.TryParse(strPropValue, iVal64) Then iVal64 = 0
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
        Return toObj

    End Function

#End Region
End Class
