Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports NGL.FreightMaster.Data
Imports NGL.FreightMaster.Data.DataTransferObjects

Public Class clsApplication : Inherits Ngl.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New Ngl.FreightMaster.Core.UserConfiguration

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
        Dim oParameter As New Ngl.FreightMaster.Core.Model.Parameter(Me.oConfig)
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

    ''' <summary>
    ''' Process the subscription alert emails
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.0 on 12/12/2016
    '''   support user option to send or ignore emails on some alerts
    '''   added new logic to group all emails by user upto 3999 characters in the body independent of subject
    ''' </remarks>
    Public Sub ProcessData()
        Try
            Me.openLog()
            Me.Log(Source & " Applicaiton Start")

            'use the database name as part of the source
            displayParameterData()
            fillConfig()
            Dim oQuery As New Ngl.Core.Data.Query
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

            'lets begin by creating the WCFproperty.
            Dim props As WCFParameters = CreateaDataProperties()
            props.ConnectionString = Me.ConnectionString
            props.Database = oConfig.Database
            props.DBServer = oConfig.DBServer
            props.ValidateAccess = False

            'next, get all alerts that have not been sent as email.
            Dim oSQL As New NGLSecurityDataProvider(props)
            Dim lAlerts() As tblAlertMessage = oSQL.GetEmailAlertMessages()
            Dim failedAlertControls As New List(Of Long)
            'next group alerts by procedurel control
            Dim grpAlertPControl = From item As tblAlertMessage In lAlerts Group By item.AlertMessageProcedureControl Into Group

            'next, get list of subscribed users by procedure control
            Dim listOfProcControlWUsers As New List(Of UserAlert)
            For Each item In grpAlertPControl
                'also group comp controls by procedure control
                'next, get a list of compControls for each alert
                Dim grpAlertPControlCompControl = From item2 In item.Group Group By item2.AlertMessageCompControl Into Group
                For Each compControlGrp In grpAlertPControlCompControl
                    'Modified by LVV on 4/1/16 for v-7.0.5.0 Ticket #1355 "Order Finalized"
                    Dim alertsByComp = New List(Of tblAlertMessage)
                    alertsByComp = (From d In compControlGrp.Group Select d).ToList()
                    'finally get the users for that procedure control
                    Dim users() As tblUserSecurity = Nothing

                    Try
                        'Modified by RHR for v-7.0.6.0 on 12/12/2016
                        users = oSQL.UsersToEmailAlert(item.AlertMessageProcedureControl, compControlGrp.AlertMessageCompControl)
                    Catch iop As InvalidOperationException
                        'do something
                    Catch ex As Exception
                        'do something
                    End Try

                    'create the Anonymous object and add it to the list
                    'Dim obj = New With {.Controlinq exceptionsl = item.AlertMessageProcedureControl, .Users = users}
                    For Each user As tblUserSecurity In users.Where(Function(u) u.UserEmail.Trim.Length > 0)
                        'create the email body to add to.
                        'next, create one email body with all alerts in it.
                        'Dim alertMessages As String = ""
                        'For Each alert As tblAlertMessage In compControlGrp.Group ' item.Group
                        '    alertMessages = alertMessages & "<br/>" & alert.AlertMessageSubject & alert.AlertMessageBody & "<br/>"
                        'Next
                        'find the user, add if we dont have the user already
                        Dim userFromUSers As tblUserSecurity = user
                        Dim findUser = (From d In listOfProcControlWUsers Where d.User.UserSecurityControl = userFromUSers.UserSecurityControl Select d).FirstOrDefault
                        If findUser Is Nothing Then
                            'add it now.
                            'listOfProcControlWUsers.Add(New With {.User = userFromUSers, .EmailBody = alertMessages})
                            'Modified by LVV on 4/1/16 for v-7.0.5.0 Ticket #1355 "Order Finalized"
                            Dim oAlert = New UserAlert
                            oAlert.User = userFromUSers
                            oAlert.Messages = New List(Of tblAlertMessage)
                            oAlert.Messages.AddRange(alertsByComp)
                            listOfProcControlWUsers.Add(oAlert)
                        Else
                            'Dim oMessages As New List(Of NGL.FreightMaster.Data.DataTransferObjects.tblAlertMessage) From {findUser.AlertMessages}
                            'oMessages.AddRange(compControlGrp.Group)
                            findUser.Messages.AddRange(alertsByComp)
                            'findUser.EmailBody = findUser.EmailBody & "<br/><br/>" & alertMessages
                        End If
                    Next
                Next
            Next

            'next, for each user, send an email with the alert email body
            Dim emailProvider As New NGLBatchProcessDataProvider(props)
            Dim totalEmails As Integer = 0
            Dim totalEmailSuccess As Integer = 0
            For Each useralert In listOfProcControlWUsers
                Dim blnSendEmail As Boolean = False
                Try
                    Dim strPreviousSubj As String = ""
                    Dim strBody As String = ""
                    Dim alertCount As Integer = 0
                    For Each alert In useralert.Messages.OrderBy(Function(x) x.AlertMessageSubject)
                        blnSendEmail = True
                        Try
                            'Modified by RHR for v-7.0.6.0 on 12/12/2016

                            If strBody.Trim.Length + alert.AlertMessageBody.Trim.Length > 3999 - (Me.DBInfo.Length + 10) Then
                                'send the email 
                                blnSendEmail = emailProvider.executeGenerateEmail2Way(Me.FromEmail, useralert.User.UserEmail, "", "Subscription Alerts (" & alertCount & ")", strBody & Me.DBInfo)
                                If strPreviousSubj.Trim.Length > 0 AndAlso strPreviousSubj <> alert.AlertMessageSubject Then
                                    'the code below will add the subject back in
                                    strBody = ""
                                Else
                                    strBody = alert.AlertMessageSubject & ":<br />" & vbCrLf
                                End If
                                alertCount = 0
                            End If
                            If strPreviousSubj.Trim.Length > 0 AndAlso strPreviousSubj <> alert.AlertMessageSubject Then
                                strBody &= alert.AlertMessageSubject & ":<br />" & vbCrLf
                            End If
                        Catch ex As Exception
                            blnSendEmail = False
                        End Try
                        If Not blnSendEmail Then
                            failedAlertControls.Add(alert.AlertMessageControl)
                            strBody = ""
                            alertCount = 0
                        End If
                        strPreviousSubj = alert.AlertMessageSubject
                        alertCount += 1
                        'Modified by RHR for v-7.0.6.0 on 12/12/2016
                        strBody &= alert.AlertMessageBody & "<br />" & vbCrLf & "<br />"

                    Next
                    'Send the last email if one exists
                    If Not String.IsNullOrWhiteSpace(strBody) AndAlso Not String.IsNullOrWhiteSpace(strPreviousSubj) Then
                        If alertCount < 1 Then alertCount = 1
                        blnSendEmail = emailProvider.executeGenerateEmail2Way(Me.FromEmail, useralert.User.UserEmail, "", "Subscription Alerts (" & alertCount & ")", strBody & Me.DBInfo)
                    End If  'Dim userObj = useralert
                    'Dim emailBody As String = userObj.EmailBody
                    'Dim arrList As New ArrayList
                    ''loop until the emails body is less than 4000
                    'Do While emailBody.Length > 4000
                    '    Dim bodyString As String = emailBody.Substring(0, 3999)
                    '    arrList.Add(bodyString)
                    '    emailBody = emailBody.Substring(bodyString.Length, emailBody.Length - bodyString.Length)
                    'Loop
                    'Dim arrCount As Integer = 1
                    'For Each item In arrList
                    '    blnSendEmail = emailProvider.executeGenerateEmail2Way(Me.FromEmail, useralert.User.UserEmail, "", "FM Alerts " & arrCount, item)
                    '    arrCount = arrCount + 1
                    'Next
                Catch iop As InvalidOperationException
                    'do something
                Catch ex As Exception
                    emailProvider.executeGenerateEmail2Way(Me.FromEmail, Me.GroupEmail, Me.AdminEmail, "FM Alerts Email Error Sending", "There were problems sending Alert Emails to user: " & useralert.User.UserEmail & Me.DBServer & " " & Me.Database)
                End Try

                'If blnSendEmail Then
                '    totalEmailSuccess = totalEmailSuccess + 1
                'End If
                'totalEmails = totalEmails + 1
            Next

            ''next, once all users emails were sent succesfully, update the alertmessage to sent = true.
            'If totalEmails = totalEmailSuccess Then
            '    For Each alert As tblAlertMessage In lAlerts
            '        Try 
            '            oSQL.ConfirmEmailAlertMessageIsSent(alert.AlertMessageControl)
            '        Catch iop As InvalidOperationException
            '            LogError("Get New Alert Notification Failure", "There was an invalid operation error while updating alert record setting EMailSent = true: " & readExceptionMessage(iop) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
            '        Catch ex As Exception
            '            LogError("Get New Alert Notification Failure", "There was an unexpected error while updating alert record setting EMailSent = true: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
            '        End Try
            '    Next
            'End If

            For Each alert As tblAlertMessage In lAlerts
                Try
                    If Not (failedAlertControls Is Nothing OrElse (failedAlertControls.Count > 0 AndAlso failedAlertControls.Contains(alert.AlertMessageControl))) Then
                        oSQL.ConfirmEmailAlertMessageIsSent(alert.AlertMessageControl)
                    End If
                Catch iop As InvalidOperationException
                    LogError("Get New Alert Notification Failure", "There was an invalid operation error while updating alert record setting EMailSent = true: " & readExceptionMessage(iop) & vbCrLf & vbCrLf & "Source: " & Source & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
                Catch ex As Exception
                    LogError("Get New Alert Notification Failure", "There was an unexpected error while updating alert record setting EMailSent = true: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
                End Try
            Next

            'next, end of program.
        Catch ex As Exception
            LogError("Get New Alert Notification Failure", "There was an unexpected error while processing New Alert Email Notifications: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
        Finally

        End Try


    End Sub

    Private Function CreateaDataProperties() As WCFParameters

        Dim data As WCFParameters = New WCFParameters

        data.UserName = "NGLSystem"
        data.Database = "NGLMAS2012DEV"
        data.DBServer = "NGLRDP05D"
        data.WCFAuthCode = "NGLSystem"

        Return data

    End Function

    ''' <summary>
    ''' This function reads in the compnay contact notify email address info using the 
    ''' udfGetCompContNotifyEmail business logic component
    ''' Any errors encountered are logged and the current administion email is used by default.
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getCompContEmails(ByRef oQuery As Ngl.Core.Data.Query, ByVal CompControl As Integer) As String
        Dim strRet As String = Me.AdminEmail
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return strRet
        Try
            Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill("select dbo.udfGetCompContNotifyEmails(" & CompControl & " ) as Emails")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    strRet = Ngl.Core.Utility.DataTransformation.CleanNullableString(oDT.Rows(0).Item("Emails")) '& "; rramsey@nextgeneration.com"
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
    Private Sub getCompanyInfo(ByRef oQuery As Ngl.Core.Data.Query, ByRef oEmailData As EmailData)
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            Dim strSQL As String = "Select Top 1 CompControl From dbo.Comp Where CompNumber = " & oEmailData.CompanyNumber
            Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
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
