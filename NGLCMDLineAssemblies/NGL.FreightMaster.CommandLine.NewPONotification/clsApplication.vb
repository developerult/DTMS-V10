Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports Ngl.Core.Communication

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    Protected Property LastNotifyDate() As Date
        Get
            Dim dtVal As Date = "01-01-1900"
            Try
                dtVal = My.Settings.LastNotifyDate
            Catch ex As Exception

            End Try
            If dtVal < "01/01/1900 12:00:00 AM" Then dtVal = "01/01/1900 12:00:00 AM"
            'Date.TryParse(My.Settings.LastNotifyDate, dtVal)
            'Return dtVal
            Return dtVal
        End Get
        Set(ByVal value As Date)
            My.Settings.LastNotifyDate = value
            My.Settings.Save()
        End Set
    End Property

    Private Function ProcessAlerts() As Boolean
        Dim blnProcessAlerts As Boolean = True
        Dim oParameter As New NGL.FreightMaster.Core.Model.Parameter(Me.oConfig)
        If Not oParameter.testConnection() Then
            Log(Source & " Database Connection Failure: " & oParameter.LastError)
            Throw New ApplicationException(Source & " Read Parameter Data Failure")
        End If
        Dim dblEmailAlertMinutes As Double = 0
        Dim dblEmailAlertStart As Double = 0
        Dim dblEmailAlertEnd As Double = 24
        Dim dblEmailOnSaturday As Double = 0
        Dim dblEmailOnSunday As Double = 0
        Dim dtDefault As Date = Now
        Try
            'We pass 0 to getParValue as the company control number to use Global Settings (Defaults)
            dblEmailAlertMinutes = oParameter.getParValue("EmailAlertMinutes", 0)
            dblEmailAlertStart = oParameter.getParValue("EmailAlertStartOfBusinessDay", 0)
            dblEmailAlertEnd = oParameter.getParValue("EmailAlertEndOfBusinessDay", 0)
            dblEmailOnSaturday = oParameter.getParValue("EmailAlertOnSaturday", 0)
            dblEmailOnSunday = oParameter.getParValue("EmailAlertOnSunday", 0)
            dtDefault = Now.AddMinutes(((dblEmailAlertMinutes + 1) * -1))

            Dim dtNow As Date = Now
            Dim strFalseMsg As String = ""
            If dtNow.DayOfWeek = DayOfWeek.Saturday AndAlso dblEmailOnSaturday = 0 Then
                strFalseMsg &= "No Emails on Saturday! "
                blnProcessAlerts = False
            End If
            If blnProcessAlerts AndAlso dtNow.DayOfWeek = DayOfWeek.Sunday AndAlso dblEmailOnSunday = 0 Then
                strFalseMsg &= "No Emails on Sunday! "
                blnProcessAlerts = False
            End If
            If blnProcessAlerts AndAlso dtNow.Hour < dblEmailAlertStart Then
                strFalseMsg &= "No Emails Before " & dblEmailAlertStart.ToString & " Hour! "
                blnProcessAlerts = False
            End If
            If blnProcessAlerts AndAlso dtNow.Hour > dblEmailAlertEnd Then
                strFalseMsg &= "No Emails After " & dblEmailAlertEnd.ToString & " Hour! "
                blnProcessAlerts = False
            End If
            If Not blnProcessAlerts Then Log(Source & " Process Alerts is False: " & strFalseMsg)

        Catch ex As Exception
            LogError("Check Process Alert Settings Failure", "There was an unexpected error while checking the Alert processing parameter values: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
            blnProcessAlerts = False
        End Try
        Return blnProcessAlerts
    End Function

    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Applicaiton Start")

        'use the database name as part of the source
        displayParameterData()
        fillConfig()
        Dim oQuery As New NGL.Core.Data.Query
        oQuery.Database = Me.oConfig.Database
        oQuery.Server = Me.oConfig.DBServer
        If Not oQuery.testConnection() Then
            Log(Source & " Database Connection Failure: " & oQuery.LastError)
            Return
        End If
        'Find out if we are scheduled to process alerts
        Console.WriteLine("Testing Process Alert Parameters.")
        If Not ProcessAlerts() Then Return
        Console.Write("Reading Last Notify Date: ")
        Console.CursorLeft = 26
        'get the last notify date
        Dim dtLastNotify As Date = LastNotifyDate
        Console.Write(dtLastNotify.ToString)
        'update the last notify date with the current date and time
        LastNotifyDate = Now
        Console.WriteLine()

        'Get any new PO's
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            Dim strSQL As String = "Select isnull(count(POHDRnumber),0) As NbrOfOrders ,isnull(POHDRCreateDate,getdate()) as POHDRCreateDate,isnull(POHDRDefaultCustomer,0) as POHDRDefaultCustomer ,isnull(POHDRDefaultCustomerName,'') as POHDRDefaultCustomerName From dbo.POHdr Where POHDRCreateDate > '" & dtLastNotify.ToString & "' Group By POHDRDefaultCustomer,POHDRDefaultCustomerName,POHDRCreateDate"
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    Dim intCompanyNumber As Integer = 0
                    Dim blnSendEmail As Boolean = False
                    Dim oEmailData As New EmailData
                    Dim oEmailList As New EmailDataList
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        'Test if this is a new company number
                        If intCompanyNumber <> oRow("POHDRDefaultCustomer") Then
                            'this is a new company so send any previous emails that were pending
                            If blnSendEmail Then
                                ProcessEmail(oEmailList)
                                oEmailList = New EmailDataList
                                blnSendEmail = False
                            End If
                            intCompanyNumber = oRow("POHDRDefaultCustomer")
                        End If
                        oEmailData = New EmailData
                        oEmailData.NbrOfOrders = oRow("NbrOfOrders")
                        oEmailData.CreateDate = oRow("POHDRCreateDate")
                        oEmailData.CompanyNumber = oRow("POHDRDefaultCustomer")
                        oEmailData.CompanyName = oRow("POHDRDefaultCustomerName")
                        'get the company information 
                        Try
                            getCompanyInfo(oQuery, oEmailData)
                            oEmailList.Add(oEmailData)
                            blnSendEmail = True
                        Catch ex As Exception
                            LogError("Send New PO Notification Failure.", "There was a problem while reading the company information for company number " & oEmailData.CompanyNumber.ToString & ". Notification about " & oEmailData.NbrOfOrders.ToString & " new orders could not be transmitted.  The actual error message is: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
                            'reset all variables so that the next record can be processed as a new notification
                            oEmailList = New EmailDataList
                            intCompanyNumber = 0
                            blnSendEmail = False
                        End Try
                    Next
                    'send the last email
                    If blnSendEmail Then
                        ProcessEmail(oEmailList)
                    End If
                    oDT = Nothing
                Else
                    Log("No new POs available.")
                End If
            Else
                LogError("Get New PO Notification Failure", "There was a problem reading the POHdr table information.  The database returned the following message :" & oQuery.LastError & vbCrLf & "Using the following query:" & vbCrLf & strSQL & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get New PO Notification Failure", "There was an unexpected error while reading the new PO records: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try

    End Sub


    ''' <summary>
    ''' This function reads in the compnay contact notify email address info using the 
    ''' udfGetCompContNotifyEmail business logic component
    ''' Any errors encountered are logged and the current administion email is used by default.
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getCompContEmails(ByRef oQuery As NGL.Core.Data.Query, ByVal CompControl As Integer) As String
        Dim strRet As String = Me.AdminEmail
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return strRet
        Try
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("select dbo.udfGetCompContNotifyEmails(" & CompControl & " ) as Emails")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    strRet = NGL.Core.Utility.DataTransformation.CleanNullableString(oDT.Rows(0).Item("Emails")) '& "; rramsey@nextgeneration.com"
                Else
                    LogError("Send New PO Notification Warning!", String.Format("{0} Cannot get the company contact emails using configured admin email by default {1}.  The actual error is: {2}", Source, strRet, oQuery.LastError), Me.AdminEmail)
                End If
                oQR = Nothing

            End If
        Catch ex As Exception
            LogError("Send New PO Notification Warning!", "There was an unexpected error while reading the company contact emails.  The application will use the configured admin email by default " & strRet & ".  The actual error message is: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return strRet
    End Function

    Private Function getFirstEmail(ByVal strEmails As String) As String
        Dim strRet As String = strEmails
        Try
            Dim intSplit As Integer = InStr(1, strEmails, ";")
            If intSplit > 0 Then
                strRet = Left(strEmails, intSplit - 1)
            Else
                strRet = strEmails
            End If

        Catch ex As Exception
            'Do nothing
        End Try
        Return strRet
    End Function

    Private Function getCCEmail(ByVal strEmails As String) As String
        Dim strRet As String = ""
        Try
            strEmails = strEmails.Replace(" ", "")
            strEmails = strEmails.Replace(";;", ";")
            Dim intSplit As Integer = InStr(1, strEmails, ";")
            If intSplit > 0 Then strRet = Mid(strEmails, intSplit + 1)
        Catch ex As Exception
            'Do nothing
        End Try
        Return strRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oEmailList"></param>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use optional parameters
    ''' </remarks>
    Private Sub ProcessEmail(ByRef oEmailList As EmailDataList)
        If Not oEmailList Is Nothing AndAlso oEmailList.Count > 0 Then
            'send an email notification            
            Dim strMsg As String = ""
            Dim strSubject As String = ""
            Dim oEmail As New NGL.Core.Communication.Email
            Dim strCurrentEmail As String = ""
            Dim strCurrentCCEmail As String = ""
            For Each oEmailData As EmailData In oEmailList
                If String.IsNullOrEmpty(strCurrentEmail) Then
                    'this is the first email item to send
                    strCurrentEmail = oEmailData.Email
                    strCurrentCCEmail = oEmailData.CCEmail
                    strMsg = oEmailData.NbrOfOrders.ToString & " new orders for company number " & oEmailData.CompanyNumber.ToString & " are available for processing." & vbCrLf & vbCrLf & "  These orders were downloaded on " & oEmailData.CreateDate.ToString & "."
                    strSubject = "New orders have arrived for company number " & oEmailData.CompanyNumber
                    Log(strMsg)
                ElseIf strCurrentEmail <> oEmailData.Email Then
                    'for some reason the default email address is different so we send multiple emails                    
                    If Me.Debug Then
                        Log("Actual Email Not Transmitted In Debug Mode.")
                        Log("Server: " & Me.SMTPServer)
                        Log("From Email: " & Me.FromEmail)
                        Log("To Email: " & strCurrentEmail)
                        Log("CC Email: " & strCurrentCCEmail)
                        Log("Subject: " & strSubject)
                        Log("Message: " & strMsg)
                    Else 'write the previous message to the log
                        Log(strMsg)
                        'send the previous email data
                        'Modified By LVV 2/18/16 v-7.0.5.0
                        If Not oEmail.SendMail(Me.SMTPServer, strCurrentEmail, Me.FromEmail, strMsg, strSubject, strCurrentCCEmail, SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                            LogError("Send New PO Notification Failure", "Could not send the following email message: " & vbCrLf & strMsg & vbCrLf & "The actual error is: " & vbCrLf & oEmail.LastError, Me.AdminEmail)
                        End If
                    End If
                    'start a new message
                    strMsg = oEmailData.NbrOfOrders.ToString & " new orders for company number " & oEmailData.CompanyNumber.ToString & " are available for processing." & vbCrLf & vbCrLf & "  These orders were downloaded on " & oEmailData.CreateDate.ToString & "."
                    strSubject = "New orders have arrived for company number " & oEmailData.CompanyNumber
                    'save the new email address info
                    strCurrentEmail = oEmailData.Email
                    strCurrentCCEmail = oEmailData.CCEmail
                Else
                    'We have multiple imports from the same company to the same email address so just add to the message
                    strMsg &= vbCrLf & vbCrLf & oEmailData.NbrOfOrders.ToString & " new orders for company number " & oEmailData.CompanyNumber.ToString & " are available for processing." & vbCrLf & vbCrLf & "  These orders were downloaded on " & oEmailData.CreateDate.ToString & "."
                End If
            Next
            If Me.Debug Then
                Log("Actual Email Not Transmitted In Debug Mode.")
                Log("Server: " & Me.SMTPServer)
                Log("From Email: " & Me.FromEmail)
                Log("To Email: " & strCurrentEmail)
                Log("CC Email: " & strCurrentCCEmail)
                Log("Subject: " & strSubject)
                Log("Message: " & strMsg)
            Else
                'log the message
                Log(strMsg)
                'send the email
                'Modified By LVV 2/18/16 v-7.0.5.0
                If Not oEmail.SendMail(Me.SMTPServer, strCurrentEmail, Me.FromEmail, strMsg, strSubject, strCurrentCCEmail, SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                    LogError("Send New PO Notification Failure", "Could not send the following email message: " & vbCrLf & strMsg & vbCrLf & "The actual error is: " & vbCrLf & oEmail.LastError, Me.AdminEmail)
                End If
            End If
        Else
            Console.WriteLine("No Email Data Is Available To Send.")
        End If
    End Sub


    Public Sub fillConfig()
        Try
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
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
    ''' This sub routine reads the company control number and determines what emails to use
    ''' All errors are thrown back to the calling procedure and must be handled for each company 
    ''' So that one error does not affect all notifications
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <param name="oEmailData"></param>
    ''' <remarks></remarks>
    Private Sub getCompanyInfo(ByRef oQuery As NGL.Core.Data.Query, ByRef oEmailData As EmailData)
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            Dim strSQL As String = "Select Top 1 CompControl From dbo.Comp Where CompNumber = " & oEmailData.CompanyNumber
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    oEmailData.CompanyControl = oDT.Rows(0).Item("CompControl")
                Else
                    Throw New ApplicationException("No company records exist for company number " & oEmailData.CompanyNumber & " using the following query: " & vbCrLf & strSQL)
                    Return
                End If
                'get the contact email address
                Dim strEmails As String = getCompContEmails(oQuery, oEmailData.CompanyControl)
                'strip off the first email address and use it in the to address field
                oEmailData.Email = getFirstEmail(strEmails)
                'check for any additional email addresses and use them in the cc address field
                oEmailData.CCEmail = getCCEmail(strEmails)
            Else
                Throw New ApplicationException("There was a problem reading the company information from the comp table.  The database returned the following error:" & oQuery.LastError & vbCrLf & "Using the following query: " & vbCrLf & strSQL)
            End If
            oQR = Nothing
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw New ApplicationException("There was an unexpected error while reading the company information for company number " & oEmailData.CompanyNumber & ".  The actual error message is: " & readExceptionMessage(ex), ex)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

End Class

Public Class EmailDataList : Inherits List(Of EmailData)

    Public Sub New()
        MyBase.New()

    End Sub
End Class

Public Class EmailData
    Public CompanyControl As Integer = 0
    Public CompanyNumber As Integer = 0
    Public CompanyName As String = ""
    Public NbrOfOrders As Integer = 0
    Public CreateDate As Date = Now
    Public Email As String = ""
    Public CCEmail As String = ""
End Class
