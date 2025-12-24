Imports ngl.FreightMaster.FMLib.General
Imports ngl.FreightMaster.FMLib.dbUtilities
Imports ngl.FreightMaster.FMLib.PCMiles
Public Class clsImportExport

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
    Protected mstrCreatedDate As String = Date.Now.ToShortTimeString
    Protected mstrCreateUser As String = "system download"
    Protected mintImportTypeKey As Integer = 0
    Protected mioLog As System.IO.StreamWriter
    Protected mobjLog As FMLib.clsLog
    Protected mblnDebug As Boolean = False
    Protected mstrLogFile As String = ""
    Protected mblnSilent As Boolean = True
    Protected mintResults As Integer = 0
    Protected objF As New FMLib.clsStandardFunctions

#End Region

#Region "Public Properties"

    Public objFTP As New FMLib.FTP

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

    Protected mstrFromEmail As String = "info@maxximu.com"
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

    Protected mstrSource As String = ""
    Public Property Source() As String
        Get
            Return mstrSource
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

    Private mstrSMTPServer As String = "smtp.gmail.com"
    Public Property SMTPServer() As String
        Get
            SMTPServer = mstrSMTPServer

        End Get
        Set(ByVal Value As String)
            mstrSMTPServer = Value
        End Set
    End Property

    Private mstrGroupEmail As String = "info@maxximu.com"
    Public Property GroupEmail() As String
        Get
            GroupEmail = mstrGroupEmail
        End Get
        Set(ByVal Value As String)
            mstrGroupEmail = Value
        End Set
    End Property

    Private mstrAdminEmail As String = "info@maxximu.com"
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

    Public Property ImportTypeKey() As Integer
        Get
            Return mintImportTypeKey
        End Get
        Set(ByVal Value As Integer)
            mintImportTypeKey = Value
        End Set
    End Property

    Private mstrConnection As String = "Data Source=localhost;Initial Catalog=NGLMAS2002;Integrated Security=True"
    Public Property DBConnection() As String
        Get
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


    Private mstrDBServer As String = "localhost"
    Public Property DBServer() As String
        Get
            Return mstrDBServer
        End Get
        Set(ByVal value As String)
            mstrDBServer = value
        End Set
    End Property

    Private mstrDatabase As String = "NGLMAS2002"
    Public Property Database() As String
        Get
            Return mstrDatabase
        End Get
        Set(ByVal value As String)
            mstrDatabase = value
        End Set
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


#End Region

#Region "Public Methods"
    Public Function openConnection() As Boolean
        mobjCon = New System.Data.SqlClient.SqlConnection
        mstrConnection = "Data Source=" & DBServer & ";" 'Add Server Name
        mstrConnection &= "Initial Catalog=" & Database & ";" 'Add Database Name
        mstrConnection &= "Integrated Security=True"
        mobjCon.ConnectionString = mstrConnection
        Try
            mobjCon.Open()
            Return True

        Catch ex As System.Data.SqlClient.SqlException
            Dim strMsg As String = ""
            Dim i As Integer = 0
            If ex.Errors(i).Class.ToString() = "14" Or ex.Errors(i).Class.ToString() = "20" Then
                strMsg = "Login Error Number: " & ex.Errors(i).Class.ToString() & ControlChars.NewLine & ex.Errors(i).Message
            ElseIf ex.Errors(i).Class.ToString() = 10 Then
                strMsg = "Login Error Number 10: Windows NT Authentication Failure."
            Else
                strMsg = "Login Error Number " & ex.Errors(i).Class.ToString() & ControlChars.NewLine & ex.Message
            End If
            LogError(Source & " Database Login Failure", strMsg, AdminEmail)
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw New ApplicationException("clsPickDetails.openConnection Failure! ", ex)
        End Try
        Return False

    End Function


    Public Sub openLog()
        Try
            If mstrLogFile.Length > 0 Then
                mobjLog = New FMLib.clsLog
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

        Try
            If IsNothing(mobjLog) Then
                Exit Sub
            End If
            mobjLog.Write(logMessage, mioLog)
        Catch ex As Exception
            'ignore any errors while writing to the log
        End Try

    End Sub

    Public Sub LogError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String)
        Dim strDBInfo As String = "Server: " & mstrDBServer & vbCrLf & "Database: " & mstrDatabase
        Try
            SendMail(SMTPServer, strMailTo, FromEmail, logMessage & vbCrLf & strDBInfo, strSubject)
            
        Catch ex As Exception

        End Try
        Dim strLogMsg As String = "Email Notice Sent to " & strMailTo & vbCrLf & " Subject: " & strSubject & vbCrLf & "Msg: " & logMessage & vbCrLf & strDBInfo
        If Debug Then
            Console.WriteLine(strLogMsg)
        End If

        Log(strLogMsg)

    End Sub

    Public Function downloadFile() As Boolean
        Return downloadFile(False, False)
    End Function

    Public Function downloadFile(ByVal blnBatch As Boolean, ByVal blnUpload As Boolean) As Boolean
        Dim Ret As Boolean = False
        Try
            If Not objFTP Is Nothing Then
                With objFTP
                    'set the FTP Download Filter
                    .FileFilter = FileFilter
                    'download files
                    If Not objFTP.downloadFiles(blnBatch, blnUpload) Then
                        LastError = "FTP Dowload Error! Unable to download " & HeaderName & " Data Files: " & objFTP.LastError
                        If mblnDebug Then Console.WriteLine(LastError)
                        LogError(Source, LastError, AdminEmail)
                    Else
                        If objFTP.Results < 1 Then
                            Log("Failure! " & Source & " no files found using filter " & FileFilter)
                        Else
                            Log("Success! " & Source & " files downloaded using file filter " & FileFilter)
                        End If
                        Me.Results = .Results
                        Ret = True
                    End If
                End With
            Else
                Log(Source & " FTP Object Not Set")
            End If

        Catch ex As Exception
            Throw New ApplicationException("clsImportExport.downloadFile - Cannot Download Files Using " & FileFilter & " Filter! ", ex)
        End Try
        Return Ret
    End Function
#End Region

#Region "Protected Methods"
    Protected Function openFileDbConnection( _
        ByRef objImportCon As ADODB.Connection, _
        ByVal strDataPath As String) As Boolean
        Dim Ret As Boolean = False
        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Try
                    'build connection string for import file
                    Dim strConnection As String = "Driver={Microsoft Text Driver (*.txt; *.csv)}; Dbq=" & strDataPath & ";Extensions=asc,csv,tab,txt;HDR=YES;Persist Security Info=False"
                    'create a new connection
                    objImportCon = New ADODB.Connection
                    With objImportCon
                        .CursorLocation = ADODB.CursorLocationEnum.adUseClient
                        .Open(strConnection)
                    End With
                    Return True
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        LogError(Source & " openFileDbConnection Failure", "Could not create text db connection with schema file in  " & strDataPath & " folder : " & ex.ToString, AdminEmail)
                    Else
                        Log("openFileDbConnection Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            LogError(Source & " openFileDbConnection Failure", "Could not create text db connection with schema file in  " & strDataPath & " folder : " & ex.ToString, AdminEmail)
        End Try
        Return Ret
    End Function

#End Region

End Class
