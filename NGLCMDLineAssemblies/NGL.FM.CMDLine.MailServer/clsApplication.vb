Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.ServiceModel
Imports System.Text.RegularExpressions
Imports NGL.Core
Imports NGL.Core.Communication
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NGL.FreightMaster.Core
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports DTran = NGL.Core.Utility.DataTransformation
Imports LTS = NGL.FreightMaster.Data.LTS
Imports NData = NGL.FreightMaster.Data

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration
    Protected DBCon As New System.Data.SqlClient.SqlConnection
    Protected rs As New SqlReportingServices.ReportExecutionService
    Protected mail As DataTable
    Private EvtLog As New System.Diagnostics.EventLog

    'Instead of sending a error mail message after each error. We save it off until the end of the loop then loop through the saved errors and send one error email.
    Private SavedSendMailMessagesExceptions As New List(Of LogExceptionError)
    Private SavedGetAttachmentsExceptions As New List(Of LogExceptionError)
    Private SavedGetReportssExceptions As New List(Of LogExceptionError)


    Public blnEmailAddressInvalid As Boolean = False

    Public Sub New()
        MyBase.new()
        EvtLog.Log = "Application"
        EvtLog.Source = "NGL.FM.CMDLine.MailServer.clsApplication"
    End Sub

    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()

        Dim strMSG As String = ""
        Dim strEmailError As String = ""

        Dim oQuery As New NGL.Core.Data.Query
        oQuery.Database = Me.oConfig.Database
        oQuery.Server = Me.oConfig.DBServer
        If Not oQuery.testConnection() Then
            LogError(Source & " Database Connection Failure", "Actual error reported: " & oQuery.LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            Return
        End If

        Try
            Log("Begin Process Data ")
            If Not GetMail(oQuery) Then Return
            SendMailMessages(oQuery)
            LogSavedExceptions()
            Log("Process Data Complete")
        Catch ex As NGL.Core.DatabaseRetryExceededException
            LogError(Me.Source & " Process Data Warning", "NGL.FM.CMDLine.MailServer could not process email data because of a retry exceeded failure.", Me.AdminEmail, ex)
        Catch ex As NGL.Core.DatabaseLogInException
            LogError(Me.Source & " Process Data Warning", "NGL.FM.CMDLine.MailServer could not process email data because of a database login failure.", Me.AdminEmail, ex)
        Catch ex As NGL.Core.DatabaseInvalidException
            LogError(Me.Source & " Process Data Warning", "NGL.FM.CMDLine.MailServer could not process email data because of a database access failure.", Me.AdminEmail, ex)
        Catch ex As Exception
            LogError(Me.Source & " Process Email Data Failure", "An unexpected error has occurred while processing email messages.", Me.AdminEmail, ex)
        Finally
            Me.closeLog(0)
            Try
                DBCon.Close()
            Catch ex As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Read all the Mail Messages (limited by the GlobalSMTPLimitMailMessagesToSend parameter)
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.006 on 04/15/2020
    '''   changed logic to read GlobalSMTPLimitMailMessagesToSend and limit the number of records
    '''   returned by the query.  Modified the SQL query string
    ''' </remarks>
    Private Function GetMail(ByRef oQuery As NGL.Core.Data.Query) As Boolean
        Dim blnRet As Boolean = False

        Log("reading pending mail data")
        ' Modified by RHR for v-8.2.1.006 on 04/15/2020
        Dim intMaxMessages As Integer = 25
        Dim strSQL As String = "Select ParValue From Parameter Where ParKey = 'GlobalSMTPLimitMailMessagesToSend'"
        Dim oCon As System.Data.SqlClient.SqlConnection
        Try
            'oCon is passed before it has been assigned a value because the funtion will create a connection if it does not exist
            Integer.TryParse(oQuery.getScalarValue(oCon, strSQL, 1), intMaxMessages)
        Catch ex As Exception
            Try
                If Not oCon Is Nothing Then
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    End If
                End If
                oCon = Nothing
            Catch e As Exception
                'do nothing
            End Try
        End Try
        ' Modified by RHR for v-8.2.1.006 on 04/15/2020
        If intMaxMessages < 1 Then intMaxMessages = 25
        Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("SELECT Top " & intMaxMessages.ToString() & "  * FROM Mail WHERE DateSent IS NULL AND ReadyToSend = 1 Order By MailControl")

        If Not oQR.Exception Is Nothing Then
            LogException("Read Mail Data Failure", Source & " GetMail could not read the Mail table data because of an unexpected error.", AdminEmail, oQR.Exception, Source & ".GetMail Failure")
        End If
        mail = oQR.Data
        If Not mail Is Nothing AndAlso mail.Rows.Count > 0 Then
            blnRet = True
            Log(mail.Rows.Count & " pending messages found")
        Else
            Log("No Mail messages found")
        End If

        Return blnRet
    End Function

    ''' <summary>
    ''' Loop through all the select Mail Messages (limited by the GlobalSMTPLimitMailMessagesToSend parameter)
    ''' and call SendMailMessage
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.006 on 04/15/2020
    '''   changed logic in for each  to call new return value from SendMailMessage and stop sending on error.
    ''' </remarks>
    Private Sub SendMailMessages(ByRef oQuery As NGL.Core.Data.Query)

        For Each row As DataRow In mail.Rows
            Log("sending mail message " & mail.Rows.IndexOf(row) + 1 & " of " & mail.Rows.Count)
            If Not SendMailMessage(row, oQuery) Then
                ' Modified by RHR for v-8.2.1.006 on 04/15/2020
                Log("Sending Mail Failed Cannot Process Next Mail Record After Mail Control " & row("MailControl").ToString())
                Exit For
            End If
        Next

    End Sub


    ''' <summary>
    ''' Process one Mail Message Record and send it to the SMTP Server
    ''' </summary>
    ''' <param name="row"></param>
    ''' <param name="oQuery"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.006 on 04/15/2020
    '''   changed sub to function.  If we get an error we return false
    ''' </remarks>
    Private Function SendMailMessage(ByVal row As DataRow, ByRef oQuery As NGL.Core.Data.Query) As Boolean
        Dim strTo As String = ""
        Dim strCC As String = ""
        Dim blnRet As Boolean = True
        Try
            Dim mailTos() As String = row("MailTo").ToString().Split(";")
            strTo = row("MailTo").ToString()
            Dim message As New Net.Mail.MailMessage()
            message.From = New MailAddress(row("MailFrom"))
            Dim blnMailToOk As Boolean = False
            'Modified By LVV on 8/9/16 for v-7.0.5.110 Ticket #2290
            Dim blnIsExHanlded As Boolean = False
            For Each mailTo As String In mailTos
                'trim the string and strip any single quotes off the ends
                mailTo = NGL.Core.Utility.DataTransformation.stripQuotes(mailTo.Trim)
                If mailTo.Trim <> "" Then
                    If IsValidEmail(mailTo) Then
                        message.To.Add(mailTo)
                        blnMailToOk = True
                    Else
                        Dim ex As New ApplicationException("Invalid Send To Email Address")
                        'Modified By LVV on 8/9/16 for v-7.0.5.110 Ticket #2290
                        'Dim logError As New LogExceptionError("Send EMail Warning", Source & " SendMailMessage the following TO email address is not valid and will not be included in the email transmission: " & mailTo & " Where MailControl = " & row("MailControl") & ".</br>", AdminEmail, ex, Source & ".SendMailMessage Warning")
                        Dim strESub = row("Subject")
                        Dim strEBody = row("Body")
                        Dim logError As New LogExceptionError("Send EMail Warning", Source & "</br>" & " SendMailMessage the following TO email address is not valid and will not be included in the email transmission: " & mailTo & "</br></br>" & " Where MailControl = " & row("MailControl") & ".</br></br>Subject: " & strESub & "</br></br>Body: " & strEBody & "</br>", AdminEmail, ex, Source & ".SendMailMessage Warning")
                        blnIsExHanlded = True
                        Me.SavedSendMailMessagesExceptions.Add(logError)
                    End If
                End If
            Next
            'Modified By LVV on 8/9/16 for v-7.0.5.110 Ticket #2290
            If Not blnMailToOk Then
                If blnIsExHanlded Then
                    oQuery.Execute("UPDATE Mail SET DateSent = GETDATE(), Result = 'Invalid Send To Email Address' WHERE MailControl = " & row("MailControl"))
                    Return blnRet
                Else
                    'we do not have a valid TO email address so throw an Exception                
                    Throw New System.ApplicationException("Missing Send To Email Address")
                End If

            End If
            message.Subject = row("Subject")
            message.Body = row("Body")
            If row("mailCc").ToString <> "" Then
                Dim mailccs As String() = row("mailCc").ToString().Split(";")
                strCC = row("mailCc").ToString()
                For Each mailcc As String In mailccs
                    'trim the string and strip any single quotes off the ends
                    mailcc = NGL.Core.Utility.DataTransformation.stripQuotes(mailcc.Trim)
                    If mailcc.Trim <> "" Then
                        If IsValidEmail(mailcc) Then
                            message.CC.Add(mailcc)
                        Else
                            Dim ex As New ApplicationException("Invalid CC Email Address")
                            'Modified By LVV on 8/9/16 for v-7.0.5.110 Ticket #2290
                            'Dim logError As New LogExceptionError("Send EMail Warning", Source & " SendMailMessage the following CC email address is not valid and will not be included in the email transmission: " & mailcc & " Where MailControl = " & row("MailControl") & ".</br>", AdminEmail, ex, Source & ".SendMailMessage Warning")
                            Dim strESub = row("Subject")
                            Dim strEBody = row("Body")
                            Dim logError As New LogExceptionError("Send EMail Warning", Source & "</br>" & "SendMailMessage the following CC email address is not valid and will not be included in the email transmission: " & mailcc & "</br></br>" & " Where MailControl = " & row("MailControl") & ".</br></br>Subject: " & strESub & "</br></br>Body: " & strEBody & "</br>", AdminEmail, ex, Source & ".SendMailMessage Warning")

                            Me.SavedSendMailMessagesExceptions.Add(logError)
                        End If
                    End If
                Next
            End If
            message.IsBodyHtml = True
            GetAttachments(message, row("MailControl"), oQuery)
            GetReports(message, row("MailControl"), oQuery)
            'When running this in NGL Environment, set port to 24 which 
            'by passes spam filter.
            'Dim server As New Net.Mail.SmtpClient(mailServer, 24)
            Dim server As New Net.Mail.SmtpClient(SMTPServer)

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls13
            ServicePointManager.ServerCertificateValidationCallback = Function(ByVal s As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) True

            'TODO Add code to support office 365 similar to the core email server logic
            'Modified By LVV 2/17/16 v-7.0.5.0
            '*************************
            server.UseDefaultCredentials = GlobalSMTPUseDefaultCredentials
            If Not GlobalSMTPUseDefaultCredentials Then
                server.Credentials = New System.Net.NetworkCredential(GlobalSMTPUser, GlobalSMTPPass)
            End If
            server.Host = SMTPServer
            If GlobalSMTPEnableSSL Then
                server.EnableSsl = GlobalSMTPEnableSSL
            End If
            server.TargetName = GlobalSMTPTargetName
            server.Port = GlobalSMTPPort
            '*************************

            server.Send(message)
            Log("message sent")
            oQuery.Execute("UPDATE Mail SET DateSent = GETDATE() WHERE MailControl = " & row("MailControl"))
        Catch ex As Exception
            blnRet = False
            'Modified By LVV on 8/9/16 for v-7.0.5.110 Ticket #2290
            'Dim strErrMsg = FormatErrMsgForSQL(ex, "To: " & strTo & vbCrLf & "CC: " & strCC & vbCrLf & "MailControl: " & row("MailControl"), Source & ".SendMailMessage", 1000, Me.Debug)
            Dim strErrMsg = FormatErrMsgForSQL(ex, "</br></br>" & "To: " & strTo & "</br>" & "CC: " & strCC & "</br>" & "MailControl: " & row("MailControl"), Source & ".SendMailMessage", 1000, Me.Debug)
            EvtLog.WriteEntry(strErrMsg, EventLogEntryType.Error)

            'Instead of sending a error mail message after each error. We save it off until the end of the loop then loop through the saved errors and send one error email.
            'Modified By LVV on 8/9/16 for v-7.0.5.110 Ticket #2290
            'Dim logError As New LogExceptionError("Send EMail Failure", Source & " SendMailMessage could not process the email request because of an unexpected error.</br>" & vbCrLf & strErrMsg, AdminEmail, ex, Source & ".SendMailMessage Error")
            Dim strESub = row("Subject")
            Dim strEBody = row("Body")
            Dim logError As New LogExceptionError("Send EMail Failure", Source & "</br>" & " SendMailMessage could not process the email request because of an unexpected error.</br>" & "</br>" & strErrMsg & "</br></br>Subject: " & strESub & "</br></br>Body: " & strEBody & "</br>", AdminEmail, ex, Source & ".SendMailMessage Error")

            Me.SavedSendMailMessagesExceptions.Add(logError)
            'LogException("Send EMail Failure", Source & " SendMailMessage could not process the email request because of an unexpected error.", AdminEmail, ex, Source & ".SendMailMessage Error")

            oQuery.Execute("UPDATE Mail SET DateSent = GETDATE(), Result = '" & NGL.Core.Utility.DataTransformation.replaceQuotes(strErrMsg) & "' WHERE MailControl = " & row("MailControl"))
        End Try
        Return blnRet

    End Function

    Private Sub GetAttachments(ByRef message As Net.Mail.MailMessage, ByVal mailControl As Integer, ByRef oQuery As NGL.Core.Data.Query)

        Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("SELECT * FROM MailAttachments WHERE MailControl = " & mailControl)
        If Not oQR.Exception Is Nothing Then
            Dim logErr As New LogExceptionError("Get Mail Attachments Data Failure", Source & " GetAttachments could not read the MailAttachments table data because of an unexpected error.", AdminEmail, oQR.Exception, Source & ".GetAttachments Failure")
            Me.SavedGetAttachmentsExceptions.Add(logErr)
            'LogException("Get Mail Attachments Data Failure", Source & " GetAttachments could not read the MailAttachments table data because of an unexpected error.", AdminEmail, oQR.Exception, Source & ".GetAttachments Failure")
        End If
        Dim attachments = oQR.Data
        For Each row As DataRow In attachments.Rows
            Try
                Log("Adding attachment " & attachments.Rows.IndexOf(row) + 1 & " of " & attachments.Rows.Count)
                message.Attachments.Add(New Net.Mail.Attachment(row("FilePath")))

                oQuery.Execute("UPDATE MailAttachments SET Result = 'sent' WHERE MailAttachmentControl = " & row("MailAttachmentControl"))
                If Not oQR.Exception Is Nothing Then
                    Dim logErr As New LogExceptionError("Update Mail Attachments Data Failure", Source & " GetAttachments could not update the status for MailAttachmentControl = " & row("MailAttachmentControl") & " because of an unexpected error.", AdminEmail, oQR.Exception, Source & ".GetAttachments Error")
                    Me.SavedGetAttachmentsExceptions.Add(logErr)
                    'LogException("Update Mail Attachments Data Failure", Source & " GetAttachments could not update the status for MailAttachmentControl = " & row("MailAttachmentControl") & " because of an unexpected error.", AdminEmail, oQR.Exception, Source & ".GetAttachments Error")
                    oQuery.Execute("UPDATE MailAttachments SET Result = '" & FormatErrMsgForSQL(oQR.Exception, Source & ".GetAttachments", 1000, Me.Debug) & "' WHERE MailAttachmentControl = " & row("MailAttachmentControl"))
                End If
            Catch ex As Exception
                Dim strErrMsg = FormatErrMsgForSQL(ex, Source & ".GetAttachments", 1000, Me.Debug)
                EvtLog.WriteEntry(strErrMsg, EventLogEntryType.Error)
                Dim logErr As New LogExceptionError("Update Mail Attachments Data Failure", Source & " GetAttachments could not update the status for MailAttachmentControl = " & row("MailAttachmentControl") & " because of an unexpected error.", AdminEmail, ex, Source & ".GetAttachments Error")
                Me.SavedGetAttachmentsExceptions.Add(logErr)
                'LogException("Update Mail Attachments Data Failure", Source & " GetAttachments could not update the status for MailAttachmentControl = " & row("MailAttachmentControl") & " because of an unexpected error.", AdminEmail, ex, Source & ".GetAttachments Error")
                oQuery.Execute("UPDATE MailAttachments SET Result = '" & strErrMsg & "' WHERE MailAttachmentControl = " & row("MailAttachmentControl"))
            End Try
        Next

    End Sub

    Private Sub GetReports(ByRef message As MailMessage, ByVal mailControl As Integer, ByRef oQuery As NGL.Core.Data.Query)
        ReportServerURL = ReportServerURL.Trim
        If Not Right(ReportServerURL, 1) = "/" Then ReportServerURL += "/"
        rs.Url = ReportServerURL & "ReportExecution2005.asmx"
        rs.Credentials = Net.CredentialCache.DefaultCredentials
        If ReportServerUser <> "" Then
            rs.Credentials = New Net.NetworkCredential(ReportServerUser, ReportServerPass, ReportServerDomain)
        End If
        Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("SELECT * FROM MailReports WHERE MailControl = " & mailControl)
        If Not oQR.Exception Is Nothing Then
            Dim logErr As New LogExceptionError("Get Mail Reports Data Failure", Source & " GetReports could not read the MailReports table data because of an unexpected error.", AdminEmail, oQR.Exception, Source & ".GetReports Failure")
            Me.SavedGetReportssExceptions.Add(logErr)
            'LogException("Get Mail Reports Data Failure", Source & " GetReports could not read the MailReports table data because of an unexpected error.", AdminEmail, oQR.Exception, Source & ".GetReports Failure")
        End If
        Dim reports = oQR.Data
        'modified by RHR for v-8.4.0.003 on 10/07/2021  added parameter data to error messges
        Dim strParameterData As String = ""
        For Each row As DataRow In reports.Rows
            Try
                Log("Adding report " & reports.Rows.IndexOf(row) + 1 & " of " & reports.Rows.Count)
                Dim result As Byte() = Nothing
                Dim historyId As String = Nothing
                Dim encoding As String
                Dim mimeType As String
                Dim extension As String
                Dim warnings As SqlReportingServices.Warning() = Nothing
                Dim streamIds As String() = Nothing
                Dim parameterValues() As SqlReportingServices.ParameterValue = Nothing

                Dim execInfo As New SqlReportingServices.ExecutionInfo
                Dim execHeader As New SqlReportingServices.ExecutionHeader

                rs.ExecutionHeaderValue = execHeader
                execInfo = rs.LoadReport(row("path"), historyId)

                If row("parameters").ToString <> "" Then
                    Dim parameters() As String = row("parameters").ToString.Split("|")
                    For Each parameter As String In parameters
                        Dim parameterStrings() As String = parameter.Split("^")
                        If parameterStrings.Length = 2 Then
                            If parameterValues Is Nothing Then
                                ReDim parameterValues(0)
                            Else
                                ReDim Preserve parameterValues(parameterValues.Length)
                            End If
                            parameterValues(parameterValues.GetUpperBound(0)) = New SqlReportingServices.ParameterValue
                            parameterValues(parameterValues.GetUpperBound(0)).Name = parameterStrings(0)
                            parameterValues(parameterValues.GetUpperBound(0)).Value = parameterStrings(1)
                            strParameterData = strParameterData & " " & parameterStrings(0) & " = " & parameterStrings(1)
                        End If
                    Next
                End If

                If parameterValues IsNot Nothing Then rs.SetExecutionParameters(parameterValues, "en-us")

                Dim sessionId As String = rs.ExecutionHeaderValue.ExecutionID
                result = rs.Render(row("Format"), Nothing, extension, mimeType, encoding, warnings, streamIds)

                Dim stream As New System.IO.MemoryStream()
                stream.Write(result, 0, result.Length)
                stream.Position = 0
                message.Attachments.Add(New Attachment(stream, row("AttachAsFileName").ToString))
                oQuery.Execute("UPDATE MailReports Set Result = 'sent' WHERE MailReportControl = " & row("MailReportControl"))
                If Not oQR.Exception Is Nothing Then
                    'modified by RHR for v-8.4.0.003 on 10/07/2021  the email error message was not helpfull added more information
                    Dim logErr As New LogExceptionError("Update Mail Reports Status Failure", Source & " The system could not update the status for an email attachment. The attahchment may not have been sent correcly,  check the order information and verify that the attachment was received, or resend the email.  Data:  " & strParameterData & " Table Key MailReportControl = " & row("MailReportControl") & " .", AdminEmail, oQR.Exception, Source & ".GetReports Error")
                    Me.SavedGetReportssExceptions.Add(logErr)
                    'LogException("Update Mail Reports Data Failure", Source & " GetReports could not update the status for MailReportControl = " & row("MailReportControl") & " because of an unexpected error.", AdminEmail, oQR.Exception, Source & ".GetReports Error")
                    oQuery.Execute("UPDATE MailReports SET Result = '" & FormatErrMsgForSQL(oQR.Exception, Source & ".GetReports", 1000, Me.Debug) & "' WHERE MailReportControl = " & row("MailReportControl"))
                End If
            Catch ex As Exception
                Dim strErrMsg = FormatErrMsgForSQL(ex, Source & ".GetReports", 1000, Me.Debug)
                EvtLog.WriteEntry(strErrMsg, EventLogEntryType.Error)
                'modified by RHR for v-8.4.0.003 on 10/07/2021  the email error message was not helpfull added more information
                Dim logErr As New LogExceptionError("Update Mail Reports Status Failure", Source & " The system could not update the status for an email attachment. The attahchment may not have been sent correctly,  check the order information and verify that the attachment was received, or resend the email.  Data:  " & strParameterData & " Table Key MailReportControl = " & row("MailReportControl") & " .", AdminEmail, ex, Source & ".GetReports Error")
                Me.SavedGetReportssExceptions.Add(logErr)
                'LogException("Update Mail Reports Data Failure", Source & " GetReports could not update the status for MailReportControl = " & row("MailReportControl") & " because of an unexpected error.", AdminEmail, ex, Source & ".GetReports Error")
                oQuery.Execute("UPDATE MailReports SET Result = '" & strErrMsg & "' WHERE MailReportControl = " & row("MailReportControl"))
            End Try
        Next

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

    Private Sub LogSavedExceptions()
        'This method sends only one error notification email for all of the logged exceptions.
        'This way the sytem is not flooded with emails.

        Dim strMsg As String = ""

        For Each item As LogExceptionError In Me.SavedSendMailMessagesExceptions

            strMsg = formatSaveLogErrorMessage(strMsg, item.LogMessage, item.EXception, item.Subject, item.Header)

        Next

        For Each item As LogExceptionError In Me.SavedGetAttachmentsExceptions

            strMsg = formatSaveLogErrorMessage(strMsg, item.LogMessage, item.EXception, item.Subject, item.Header)

        Next

        For Each item As LogExceptionError In Me.SavedGetReportssExceptions

            strMsg = formatSaveLogErrorMessage(strMsg, item.LogMessage, item.EXception, item.Subject, item.Header)

        Next

        Dim count As Integer = Me.SavedSendMailMessagesExceptions.Count + Me.SavedGetAttachmentsExceptions.Count + Me.SavedGetReportssExceptions.Count

        If count > 0 Then
            'now the the huge email body is created. lets log it.
            LogError("NGL.FM.CMDLine.MailServer Logged " & count & " Errors", strMsg, Me.AdminEmail)
        End If

    End Sub
    Private Function formatSaveLogErrorMessage(ByVal returnString As String, ByVal logMessage As String, ByVal ex As Exception, ByVal subject As String, Optional ByVal header As String = "") As String

        If header.Trim.Length > 0 Then
            returnString &= "<h2>" & header & vbCrLf & "</h2>"
        End If

        returnString &= "<p>" & logMessage & "</p>" & vbCrLf

        returnString &= subject & vbCrLf

        returnString &= "<hr />" & vbCrLf

        If Me.Debug Then
            returnString &= ex.ToString & vbCrLf
        Else
            returnString &= ex.Message & vbCrLf
        End If
        returnString &= "<hr />" & vbCrLf

        returnString &= vbCrLf & vbCrLf & vbCrLf
        Return returnString
    End Function


    Public Function IsValidEmail(strIn As String) As Boolean
        blnEmailAddressInvalid = False
        If String.IsNullOrEmpty(strIn) Then Return False

        ' Use IdnMapping class to convert Unicode domain names.
        strIn = Regex.Replace(strIn, "(@)(.+)$", AddressOf Me.DomainMapper)
        If blnEmailAddressInvalid Then Return False

        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn, _
               "^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + _
               "(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
               RegexOptions.IgnoreCase)
    End Function

    Private Function DomainMapper(match As Match) As String
        ' IdnMapping class with default property values. 
        Dim idn As New IdnMapping()

        Dim domainName As String = match.Groups(2).Value
        Try
            domainName = idn.GetAscii(domainName)
        Catch e As ArgumentException
            blnEmailAddressInvalid = True
        End Try
        Return match.Groups(1).Value + domainName
    End Function

End Class

Public Class LogExceptionError

    Sub New(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, ByVal ex As Exception, Optional ByVal strHeader As String = "")
        Me.Subject = strSubject
        Me.LogMessage = logMessage
        Me.MailTo = strMailTo
        Me.EXception = ex
        Me.Header = strHeader
    End Sub

    Private _Subject As String = ""
    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property

    Private _LogMessage As String = ""
    Public Property LogMessage() As String
        Get
            Return _LogMessage
        End Get
        Set(ByVal value As String)
            _LogMessage = value
        End Set
    End Property


    Private _MailTo As String = ""
    Public Property MailTo() As String
        Get
            Return _MailTo
        End Get
        Set(ByVal value As String)
            _MailTo = value
        End Set
    End Property


    Private _EXception As New Exception
    Public Property EXception() As Exception
        Get
            Return _EXception
        End Get
        Set(ByVal value As Exception)
            _EXception = value
        End Set
    End Property


    Private _Header As String = ""
    Public Property Header() As String
        Get
            Return _Header
        End Get
        Set(ByVal value As String)
            _Header = value
        End Set
    End Property

End Class