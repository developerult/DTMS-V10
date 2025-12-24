Imports System.Data
Imports System.Data.SqlClient
Imports NGL.Core
Imports NGL.Core.Communication
Imports NGL.Core.Utility
Imports NGL.Core.Communication.General

'Imports NGL.FreightMaster.FMLib
'Imports NGL.FreightMaster.FMLib.General
'Imports NGL.FreightMaster.FMLib.dbUtilities

Public Class LoadExpired

    Private Shared EvtLog As New System.Diagnostics.EventLog
    Private Shared Debug As Boolean = False

    'Dim _connectionString As String
    'Public Shared mintAllowedMinutes As Integer = "0"
    'Dim _reportPath As String
    'Dim _notificationTo As String
    'Dim _notificationCC As String
    'Public Shared mblnDebug As Boolean = False
    'Public Shared mstrUserName As String = ""
    'Public Shared mstrPassword As String = ""
    'Public Shared mstrServer As String = ""
    'Public Shared mstrLocalFolder As String = ""
    'Public Shared mstrRemoteFolder As String = ""
    'Public Shared mstrLocalBackupFolder As String = ""
    'Public Shared mstrRemoteBackupFolder As String = ""
    'Public Shared mstrPOFilter As String = "PO*.*"
    'Public Shared mstrLaneFilter As String = "Lane*.*"
    'Public Shared mstrCompFilter As String = "Comp*.*"
    'Public Shared mstrCompContFilter As String = "CompanyContact*.*"
    'Public Shared mstrCarrContFilter As String = "CarrierContact*.*"
    'Public Shared mstrCarrFilter As String = "Carr*.*"
    'Public Shared mstrSchedFilter As String = "Sched*.*"
    'Public Shared mstrPayFilter As String = "Pay*.*"
    'Public Shared mstrPOHeaderFilter As String = "POHeader*.*"
    'Public Shared mstrPODetailFilter As String = "PODetail*.*"
    'Public Shared mstrExternalProcessingFile As String = ""
    'Public Shared mstrInternalProcessingFile As String = ""
    'Public Shared mstrResultsFile As String = ""
    'Public Shared mstrINIKey As String = "NGL"
    'Public Shared mstrTransferType As String = "d"
    'Public Shared mintAutoRetry As Integer = 3
    'Public Shared mstrDatabase As String = ""
    'Public Shared mstrDBServer As String = ""
    'Public Shared mstrAdminEmail As String = ""
    'Public Shared mstrGroupEmail As String = ""
    'Public Shared mstrFromEmail As String = "system@nextgeneration.com"
    'Public Shared mstrBatchFormat As String = ""
    'Public Shared mstrSMTPServer As String = "mail.ngl.local"
    'Public Shared mblnGenerateHASH As Boolean = True
    'Public Shared mstrHashFileName As String = "HashPOTotals"
    'Public Shared mintKeepLogDays As Integer = 30
    'Public Shared mblnSaveOldLog As Boolean = True
    'Public Shared mstrWebDBServer As String = ""
    'Public Shared mstrWebDatabase As String = ""
    'Public Shared sSource As String = "Load Expiration "
    'Public Shared sErrMsg As String = "Unknown Error!"
    'Private Shared objF As New clsStandardFunctions
    'Protected Shared objFTP As New NGL.FreightMaster.FMLib.FTP

    'Public Shared consolidatedLoads As New Generic.List(Of String)
    'Public Shared strCNSNumber As String = ""

    'Public Shared ExpiredLoadsTo As String = ""
    'Public Shared ExpiredLoadsCc As String = ""
    'Public Shared CarrierLoadAcceptanceAllowedMinutes As Integer = 0

    'Public Sub New(ByVal ConnectionString As String, ByVal AllowedMinutes As Integer, _
    'ByVal ReportPath As String, ByVal NotificationTo As String, ByVal NotificationCC As String)
    '    _connectionString = ConnectionString
    '    _allowedMinutes = AllowedMinutes
    '    _reportPath = ReportPath
    '    _notificationTo = NotificationTo
    '    _notificationCC = NotificationCC
    'End Sub
    'Public Shared ReadOnly Property DBInfo() As String
    '    Get
    '        Return "Server: " & mstrDBServer & " | " & "Database: " & mstrDatabase

    '    End Get
    'End Property
    'Public Shared Function appPath() As String
    '    Return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
    'End Function

    'Update 3/21/2011 PFM, requirement to be Inherit the Ngl.FreightMaster.Core.NGLCommandLineBaseClass  
    Public Shared Sub Main()
        Dim sSource As String = "NGL.FM.CMDLine.ProcessExpiredLoads"
        Dim oApp As New clsApplication

        EvtLog.Log = "Application"
        EvtLog.Source = sSource

        Try
            Dim strCreateUser As String = "System Process Expired Loads"
            Dim strCreatedDate As String = Now.ToString
            Dim intRetValue As Integer = 0
            With oApp
                .Source = sSource
                If .readCommandLineArgs("NGL.FM.CMDLine.ProcessExpiredLoads") = 0 Then
                    Exit Sub
                End If
                'The Debug Flag is based on the command line argument flag which overrides
                'The Task Parameters Value returned by getTaskParameters
                Debug = .Debug

                If Not .validateDatabase() Then
                    Exit Sub
                End If
                If Not .getTaskParameters Then
                    Exit Sub
                End If
                Try
                    If Debug Then
                        EvtLog.WriteEntry("Application Start", EventLogEntryType.Information)
                    End If
                Catch ex As Exception
                    'ignore any errors while writing to the event log
                End Try
                .EvtLog = EvtLog
                .ProcessData()

            End With


        Catch ex As ApplicationException
            If Debug Then
                Console.WriteLine("Error! " & ex.ToString)
            End If
            EvtLog.WriteEntry(FormatErrMsg(ex, sSource, Debug), EventLogEntryType.Error)

        Catch ex As Exception
            If Debug Then
                Console.WriteLine("Error! " & ex.ToString)
            End If
            EvtLog.WriteEntry(FormatErrMsg(ex, sSource, Debug), EventLogEntryType.Error)
        Finally
            If Debug Then
                Console.WriteLine("Press Enter To Continue")
                Console.ReadLine()
            End If
        End Try
    End Sub

    'Public Shared Function readParameters() As Integer
    '    Dim strParameters() As String = System.Environment.GetCommandLineArgs
    '    Dim strVal As String
    '    Dim strHelpMsg As String
    '    Try
    '        strHelpMsg = vbCrLf _
    '            & "Usage:" & vbCrLf _
    '            & " NGL.FM.CMDLine.ProcessExpiredLoads.exe [/?] " _
    '            & " [-k:inikey] -d "
    '        strHelpMsg &= vbCrLf & vbCrLf _
    '            & "Options:" & vbCrLf _
    '            & "    /?" & vbTab & vbTab & vbTab & "Show this help screen." & vbCrLf _
    '            & "    -k:inikey" & vbTab & vbTab & "INI File Key (default = NGL)." _
    '            & "    -d" & vbTab & vbTab & "Debug Flag." _
    '            & vbCrLf & vbCrLf & "Note:  Spaces are required between parameters but not allowed inside of them."
    '        For Each strVal In strParameters
    '            Dim strSwitch As String = Left(strVal, 2)
    '            Select Case strSwitch
    '                Case "/?"
    '                    Console.WriteLine(strHelpMsg)
    '                    Console.WriteLine("Press Enter To Continue")
    '                    Console.ReadLine()
    '                    Return 0
    '                Case "-k"
    '                    mstrINIKey = strVal.Substring(3)
    '                Case "-d"
    '                    mblnDebug = True
    '            End Select

    '        Next
    '        ' Read the INI File
    '        '  If mblnDebug Then
    '        Console.WriteLine("Ini File = " & appPath() & "\FreightMaster.ini")
    '        'End If
    '        Dim objIniFile As New IniFile(appPath() & "\FreightMaster.ini")
    '        mstrLocalFolder = objIniFile.GetString(mstrINIKey, "Download Local Folder", "(none)")
    '        mstrRemoteFolder = objIniFile.GetString(mstrINIKey, "Download Remote Folder", "(none)")
    '        mstrLocalBackupFolder = objIniFile.GetString(mstrINIKey, "Download Local Backup Folder", "(none)")
    '        mstrRemoteBackupFolder = objIniFile.GetString(mstrINIKey, "Download Remote Backup Folder", "(none)")
    '        mstrInternalProcessingFile = objIniFile.GetString(mstrINIKey, "Internal Processing File", "(none)")
    '        mstrExternalProcessingFile = objIniFile.GetString(mstrINIKey, "External Processing File", "(none)")
    '        mstrResultsFile = objIniFile.GetString(mstrINIKey, "Download Results File", "(none)")
    '        mintAutoRetry = objIniFile.GetInteger(mstrINIKey, "Auto Retry", 3)
    '        mstrPOFilter = objIniFile.GetString(mstrINIKey, "PO Filter", mstrPOFilter)
    '        mstrLaneFilter = objIniFile.GetString(mstrINIKey, "Lane Filter", mstrLaneFilter)
    '        mstrCompFilter = objIniFile.GetString(mstrINIKey, "Company Filter", mstrCompFilter)
    '        mstrCompContFilter = objIniFile.GetString(mstrINIKey, "Company Contact Filter", mstrCompContFilter)
    '        mstrCarrFilter = objIniFile.GetString(mstrINIKey, "Carrier Filter", mstrCarrFilter)
    '        mstrCarrContFilter = objIniFile.GetString(mstrINIKey, "Carrier Contact Filter", mstrCarrContFilter)
    '        mstrPayFilter = objIniFile.GetString(mstrINIKey, "Payables Filter", mstrPayFilter)
    '        mstrSchedFilter = objIniFile.GetString(mstrINIKey, "Schedule Filter", mstrSchedFilter)
    '        mstrServer = objIniFile.GetString(mstrINIKey, "FTP Server IP", "")
    '        mstrDatabase = objIniFile.GetString(mstrINIKey, "Database", "")
    '        mstrDBServer = objIniFile.GetString(mstrINIKey, "DBServer", "")
    '        mstrWebDatabase = objIniFile.GetString(mstrINIKey, "WebDatabase", "")
    '        mstrWebDBServer = objIniFile.GetString(mstrINIKey, "WebDBServer", "")
    '        mstrAdminEmail = objIniFile.GetString(mstrINIKey, "AdminEmail", "support@nextgeneration.com")
    '        mstrGroupEmail = objIniFile.GetString(mstrINIKey, "GroupEmail", "support@nextgeneration.com")
    '        mstrFromEmail = objIniFile.GetString(mstrINIKey, "FromEmail", mstrFromEmail)
    '        mstrBatchFormat = objIniFile.GetString(mstrINIKey, "BatchFormat", "yyMMddhhmm")
    '        mstrPOHeaderFilter = objIniFile.GetString(mstrINIKey, "POHeaderFilter", "POHeader*.*")
    '        mstrPODetailFilter = objIniFile.GetString(mstrINIKey, "PODetailFilter", "PODetail*.*")
    '        mstrSMTPServer = objIniFile.GetString(mstrINIKey, "SMTPServer", "mail.ngl.local")
    '        mblnGenerateHASH = objIniFile.GetString(mstrINIKey, "GenerateHASH", mblnGenerateHASH.ToString).ToLower
    '        mstrHashFileName = objIniFile.GetString(mstrINIKey, "downloadHashFile", "HashPOTotals")
    '        mblnSaveOldLog = objIniFile.GetString(mstrINIKey, "SaveOldLog", mblnSaveOldLog.ToString).ToLower
    '        mintKeepLogDays = objIniFile.GetInteger(mstrINIKey, "KeepLogDays", mintKeepLogDays)
    '        mintAllowedMinutes = objIniFile.GetInteger(mstrINIKey, "AllowedMinutes", mintAllowedMinutes)
    '        Return 1
    '    Catch ex As Exception
    '        Throw New ApplicationException("FTPUpload.readParameters Failure! ", ex)
    '    End Try


    'End Function

    'Public Shared Sub ProcessExpirations()
    '    EvtLog.Log = "Application"
    '    EvtLog.Source = sSource

    '    If mblnDebug Then
    '        EvtLog.WriteEntry("Application Start", EventLogEntryType.Information)
    '    End If
    '    'gstrDatabase = mstrDatabase
    '    'gstrServer = mstrDBServer
    '    If readParameters() = 0 Then
    '        Exit Sub
    '    End If

    '    Dim sqlStatement As String = ""
    '    sqlStatement = "[spBookRecordsPendingAcceptance] "
    '    Dim cn As New SqlConnection(OpenConnection())
    '    ' Dim cn As New SqlConnection(OpenConnection())
    '    Dim cm As New SqlCommand(sqlStatement, cn)
    '    Dim da As New SqlDataAdapter(cm)
    '    Dim dt As New DataTable
    '    Try
    '        cn.Open()
    '        da.Fill(dt)
    '    Catch ex As Exception
    '        sErrMsg = FormatErrMsg(ex, sSource)
    '        LogError("Load Expired Failure", "Load Expired Calling Stored Proc " & vbCrLf & sErrMsg, mstrAdminEmail)
    '        EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
    '    Finally
    '        If cn.State = ConnectionState.Open Then cn.Close()
    '    End Try


    '    For Each row As DataRow In dt.Rows
    '        getWebDefaults(DataTransformation.FixNullInteger(row.Item("BookCustCompControl")))

    '        If HasExpired(row("BookTrackDate")) Then
    '            ExpireLoad(row)
    '        End If
    '    Next

    'End Sub

    'Public Shared Sub getWebDefaults(ByVal CompControl As Integer)

    '    Dim sqlStatement As String = "Select CarrierLoadAcceptanceAllowedMinutes, ExpiredLoadsTo, ExpiredLoadsCc From dbo.CompanyConfiguration Where CompanyId = " & CompControl
    '    Dim cn As New SqlConnection(OpenConnectionWeb())
    '    ' Dim cn As New SqlConnection(OpenConnection())
    '    Dim cm As New SqlCommand(sqlStatement, cn)
    '    Dim da As New SqlDataAdapter(cm)
    '    Dim dt As New DataTable
    '    Try
    '        cn.Open()
    '        da.Fill(dt)
    '        If dt.Rows.Count < 1 Then
    '            'get the data for company zero (defaults)
    '            sqlStatement = "Select CarrierLoadAcceptanceAllowedMinutes, ExpiredLoadsTo, ExpiredLoadsCc From dbo.CompanyConfiguration Where CompanyId = 0"
    '            cm.CommandText = sqlStatement
    '            da = New SqlDataAdapter(cm)
    '            dt = New DataTable
    '            da.Fill(dt)
    '        End If
    '    Catch ex As Exception
    '        sErrMsg = FormatErrMsg(ex, sSource)
    '        LogError("Load Expired Failure", "Load Expired Calling Stored Proc " & vbCrLf & sErrMsg, mstrAdminEmail)
    '        EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
    '    Finally
    '        If cn.State = ConnectionState.Open Then cn.Close()
    '    End Try
    '    If dt.Rows.Count < 1 Then
    '        CarrierLoadAcceptanceAllowedMinutes = mintAllowedMinutes
    '        ExpiredLoadsTo = ""
    '        ExpiredLoadsCc = ""
    '    Else
    '        CarrierLoadAcceptanceAllowedMinutes = DataTransformation.FixNullInteger(dt.Rows(0).Item("CarrierLoadAcceptanceAllowedMinutes"))
    '        ExpiredLoadsTo = nz(dt.Rows(0).Item("ExpiredLoadsTo"), "")
    '        ExpiredLoadsCc = nz(dt.Rows(0).Item("ExpiredLoadsCc"), "")
    '    End If
    'End Sub

    'Private Shared Function HasExpired(ByVal loadAssigned As Object) As Boolean

    '    If IsDate(loadAssigned) = False Then Return False

    '    Dim minutesSinceOffered As Integer = _
    '        DateDiff(DateInterval.Minute, CDate(loadAssigned), Now)
    '    If mintAllowedMinutes = 0 Then
    '        Return False
    '    ElseIf minutesSinceOffered >= CarrierLoadAcceptanceAllowedMinutes Then
    '        Return True
    '    End If

    '    Return False

    'End Function

    'Private Shared Sub ExpireLoad(ByVal row As DataRow)

    '    Console.WriteLine("Expiring Load " & row("BookControl"))
    '    Dim proNumber As String = row("BookProNumber").ToString
    '    Dim bookControl As Integer = row("BookControl")
    '    Dim carrierNumber As Integer = row("CarrierNumber")
    '    Dim CarrierControl As Integer = row("CarrierControl")
    '    Dim BookCarrierContControl As Integer = row("BookCarrierContControl")
    '    Dim carrEmail As String = nz(row("CarrierEMail"), "")
    '    strCNSNumber = nz(row("BookConsPrefix"), "").ToString
    '    Dim blnSendEmail As Boolean = True
    '    If strCNSNumber <> "" AndAlso consolidatedLoads.Contains(strCNSNumber) Then blnSendEmail = False

    '    Try
    '        'NOTE:  We pass 0 as the carrier and carrier contact to allow for cascading dispatching
    '        ProcessCarrierAcceptOrRejectData(bookControl _
    '                                    , CarrierControl _
    '                                    , BookCarrierContControl _
    '                                    , 2 _
    '                                    , blnSendEmail _
    '                                    , "Web Tendered Load Has Expired" _
    '                                    , 0 _
    '                                    , "ProcessExpiredLoads" _
    '                                    , ExpiredLoadsTo _
    '                                    , ExpiredLoadsCc)
    '    Catch ex As Exception
    '        sErrMsg = FormatErrMsg(ex, sSource)
    '        LogError("Load Expired Failure", String.Format("Unable to process expired load for Pro number {0} assigned to carrier number {1}.  The actual error message is:", proNumber, carrierNumber) & vbCrLf & vbCrLf & sErrMsg, mstrAdminEmail)
    '        EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
    '    End Try
    'End Sub

    ''Public Shared Function SendMail(ByVal strServer As String, _
    '        ByVal strFrom As String, _
    '        ByVal strBody As String, _
    '        ByVal strTo As String, _
    '        ByVal strCC As String, _
    '        ByVal strSubject As String) As Boolean

    'Dim email As New System.Net.Mail.MailMessage(strFrom, strTo)
    'Dim client As New System.Net.Mail.SmtpClient(strServer)
    '    Try

    '        email.Body = strBody
    '        email.Subject = strSubject
    '        If strCC <> "" Then
    'Dim cc As New System.Net.Mail.MailAddress(strCC)
    '            email.CC.Add(cc)
    '        End If

    '' email.Bcc = strCC
    '        client.UseDefaultCredentials = True
    '        client.Send(email)
    '        Return True

    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function
    'Public Shared Function ToSql(ByVal text As String, Optional ByVal IncludeTrailingComma As Boolean = True, Optional ByVal AllowNull As Boolean = True) As String

    '    If text = String.Empty And AllowNull Then
    '        text = "NULL"
    '    Else
    '        text = "'" & text.Replace("'", "''").Replace(";", "") & "'"
    '    End If

    '    If IncludeTrailingComma Then text &= ","
    '    Return text
    'End Function
    'Public Shared Function OpenConnection() As String

    '    Return "Data Source=" & nz(mstrDBServer, "NGLSQL01T") & ";" & "Initial Catalog=" & nz(mstrDatabase, "NGLMAS2002DEV") & ";" & "Integrated Security=SSPI"

    'End Function
    'Public Shared Function OpenConnectionWeb() As String
    '    Return "Data Source=" & nz(mstrWebDBServer, "NGLSQL01T") & ";" & "Initial Catalog=" & nz(mstrWebDatabase, "NGLWebDev") & ";" & "Integrated Security=SSPI"
    'End Function
    'Public Shared Sub LogError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String)
    '    Try
    '        SendMail(mstrSMTPServer, mstrFromEmail, logMessage & vbCrLf & DBInfo, strMailTo, "", strSubject)

    '    Catch ex As Exception

    '    End Try

    'End Sub
    'Public Shared Function FormatErrMsg(ByVal oException As Exception, ByVal strSource As String) As String
    '    Dim strMsg As String = ""
    '    Try
    '        strMsg = "Error:"
    '    Catch ex As Exception

    '    End Try


    '    Dim outerDetail As String = ""
    '    Try
    '        outerDetail = oException.ToString
    '    Catch ex As Exception

    '    End Try

    '    Dim inner As Exception = Nothing
    '    Try
    '        inner = oException.InnerException
    '    Catch ex As Exception

    '    End Try

    '    Dim innerDetail As String = ""
    '    If Not inner Is Nothing Then
    '        Try
    '            innerDetail = inner.ToString
    '        Catch ex As Exception

    '        End Try
    '    End If

    '    Try
    '        If outerDetail.Trim.Length > 0 Then
    '            strMsg &= vbCrLf & outerDetail & vbCrLf
    '        End If
    '        If innerDetail.Trim.Length > 0 Then
    '            strMsg &= vbCrLf & innerDetail & vbCrLf
    '        End If
    '    Catch ex As Exception

    '    End Try
    '    strMsg &= vbCrLf & "Source: " & strSource & vbCrLf

    '    Return strMsg
    'End Function
    'Public Shared Function nz(ByVal strVal As String, ByVal strDefault As String) As String

    '    If Len(Trim(strVal & " ")) < 1 Then
    '        nz = strDefault
    '    Else

    '        nz = strVal & ""
    '    End If
    'End Function


End Class
