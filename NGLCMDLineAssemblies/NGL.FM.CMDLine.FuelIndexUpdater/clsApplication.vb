Imports System.IO
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports NGLData = NGL.FreightMaster.Data
Imports BLL = NGL.FM.BLL
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports NGL.FM.CMDLine.FuelIndexUpdater.FuelIndexUpdater
Imports NGL.FreightMaster.Data.DataTransferObjects

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration
    'Public blnUse48Mode As Boolean = False
    Public wcfParameters As New NGLData.WCFParameters

    ''' <summary>
    ''' Code Change PFM 12/12/2013 - Changed all code to use new BLL layer for new tariff calculator 
    ''' If you need the old code, you will have to go into TFS to get it.
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
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
            LogError(Source & " Database Connection Failure", "Actual error reported: " & oQuery.LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            Return
        End If

        Try
            'code Change PFM 11/16/2011
            'DOE is changing the site, we are going to use a downloadable XLS file.
            Dim intRet As Integer = 0
            Dim LastError As String = ""
            Log("Begin Process Data ")
            If My.Settings.GetFuelByXLS Then
                Dim updater As New SyncNatFuelIndexByXLS
                intRet = updater.Sync(ConnectionString)
                LastError = updater.LastError
            Else
                Dim updater As New SyncNatFuelIndex
                intRet = updater.Sync(ConnectionString)
                LastError = updater.LastError
            End If
            If intRet < 0 Then
                LogError(Source & " Read EIA Fuel Data Failure", LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            ElseIf intRet > 0 Then
                Log("New Fuel Index Found Updating Carrier and Booking Data")
                DoUpdate()
            Else
                Log("Fuel Index up-to-date No Need to Update Carrier and Booking Data")
            End If
            Log("Process Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected Read EIA Fuel Data Error", "An unexpected error has occurred while attempting to update the national fuel index.  The actual error is: " & ex.Message & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
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
        Dim oCon As New System.Data.SqlClient.SqlConnection
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        Dim strSQL As String = ""
        Dim strDate As String = Date.Now.ToShortDateString
        Dim htCompanyUpdates As New Hashtable
        Dim htCompanyErrors As New Hashtable
        Dim htCompanyResults As New Hashtable
        Dim oQuery As New NGL.Core.Data.Query(ConnectionString)
        Try
            Dim revBll As New BLL.NGLBookRevenueBLL(wcfParameters)
            Dim carrierBll As New BLL.NGLCarrierBLL(wcfParameters)
            Dim compdata As New NGLData.NGLCompData(wcfParameters)
            ' Dim bResults As BatchProcessingResults = bookBll.update
            Dim cResults As WCFResults = carrierBll.UpdateAllCarrierFuelFeesBatch()

            If cResults Is Nothing OrElse cResults.Success = False Then
                'something is terribly wrong
                Dim str As String = cResults.getLogAsSingleStr("<br />")
                If cResults.Log.Count > 0 Then
                    LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because of an unexpected error while executing UpdateAllCarrierFuelFeesBatch.", AdminEmail, str, Source & "DoUpdate Failure")
                End If
            Else
                Dim str As String = cResults.getLogAsSingleStr("<br />")
                If cResults.Log.Count > 0 Then
                    LogException("Update Fuel Index Log", Source & " Some fuel updates did not succeed.", AdminEmail, str, Source & " - Update Fuel Index Log")
                End If
                 
                
                    'ok lets log some emails
                    Dim strMessage As String = ""
                If cResults.BLLOnlyData IsNot Nothing Then
                    For Each bookRef As BookReferenceData In cResults.BLLOnlyData
                        'lets grab the comp informationm
                        Dim compvLookup As DTO.vLookupList = compdata.GetCompVLookup(bookRef.BookCustCompControl)
                        If compvLookup Is Nothing OrElse compvLookup.Control = 0 Then
                            'major problem here, skip and move on.
                            Log("Selecting comp for udpating fuel fees was not successfull for book control and bookcustcompcontrol: " & bookRef.BookControl & "; " & bookRef.BookCustCompControl)
                            Continue For
                        End If
                        If bookRef.Results.Errors.Count > 0 Then
                            addToCompanyMessages(bookRef.BookCustCompControl, htCompanyResults, bookRef.Results.concatErrors(), compvLookup.Description, True)
                        Else
                            addToCompanyMessages(bookRef.BookCustCompControl, htCompanyResults, "", compvLookup.Description, False)
                        End If
                    Next
                End If
 
            End If
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because too many execute failures were reported.", AdminEmail, ex, Source & ".DoUpdate Failed")
        Catch ex As Ngl.Core.DatabaseLogInException
            LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because of a database login failure.", AdminEmail, ex, Source & ".DoUpdate Failed")
        Catch ex As Ngl.Core.DatabaseInvalidException
            LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because a connection to the database could not be established.", AdminEmail, ex, Source & ".DoUpdate Failed")
        Catch ex As Exception
            LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because of an unexpected error.", AdminEmail, ex, Source & "DoUpdate Failure")
        Finally

            Try
                'check for any email messages
                For Each comp As DictionaryEntry In htCompanyResults
                    Dim strEmail As String = ""
                    If GlobalFuelIndexUpdateEmailNotificationValue And GlobalFuelIndexUpdateEmailNotification.Trim.Length > 10 Then
                        strEmail = GlobalFuelIndexUpdateEmailNotification
                    Else
                        'get the company contact email address
                        Try
                            strEmail = oQuery.getScalarValue(oCon, "Select dbo.udfGetCompContNotifyEmails(" & comp.Key & ") as Emails", 3)
                        Catch ex As Exception
                            LogException("Get Company Notify Email Error", "An unexpected error occurred while attempting to get the company notify contact email information for company control number " & comp.Key & ".", AdminEmail, ex, Source & ".DoUpdate Get Comp Notify Email")
                        End Try
                    End If
                    If Not String.IsNullOrEmpty(strEmail) AndAlso strEmail.Trim.Length > 0 Then
                        Dim oCompR As CompResults = comp.Value
                        Dim strMsg As String = "<h2>Fuel Index Updates for Company " & oCompR.CompNumber & "</h2>" & vbCrLf
                        Dim strResults As String = ""
                        If oCompR.UpdateCount > 0 Then
                            strMsg &= "<h3> There were " & oCompR.UpdateCount & " loads updated with new fuel costs</h3>" & vbCrLf
                            strResults = "<hr />" & vbCrLf & oCompR.UpdateMsg
                        End If
                        If oCompR.ErrCount > 0 Then
                            strMsg &= "<h3> There were " & oCompR.ErrCount & " loads that had errors and could not be updated with new fuel costs</h3>" & vbCrLf
                            strResults &= "<hr />" & vbCrLf & oCompR.ErrMsg
                        End If
                        strMsg &= strResults & "<hr />" & vbCrLf
                        LogResults("Fuel Index Update Results", strMsg, strEmail)
                    End If
                Next
            Catch ex As Exception

            End Try
        End Try
    End Sub


    Private Sub addToCompanyMessages(ByVal key As Integer, ByRef ht As Hashtable, ByVal strMsg As String, ByVal CompNumber As Integer, Optional ByVal isError As Boolean = False)
        Dim oCompR As CompResults
        Try
            If key > 0 And ht.ContainsKey(key) Then
                'get the company results object
                'Dim comp As DictionaryEntry = ht(key)
                oCompR = ht(key)
                oCompR.CompControl = key
                oCompR.CompNumber = CompNumber
                If isError Then
                    oCompR.ErrCount += 1
                    oCompR.ErrMsg &= vbCrLf & strMsg
                Else
                    oCompR.UpdateCount += 1
                    oCompR.UpdateMsg &= vbCrLf & strMsg
                End If
                ht(key) = oCompR
            Else
                oCompR = New CompResults
                oCompR.CompControl = key
                oCompR.CompNumber = CompNumber
                If isError Then
                    oCompR.ErrCount = 1
                    oCompR.ErrMsg = strMsg
                Else
                    oCompR.UpdateCount = 1
                    oCompR.UpdateMsg = vbCrLf & strMsg
                End If
                ht.Add(key, oCompR)
            End If
        Catch ex As Exception
            LogException("Update Fuel Index Create Company Email Message Warning", Source & " addToCompanyMessages could not update company email messages because of an unexpected error.", AdminEmail, ex, Source & "addToCompanyMessages Warning")
        End Try
    End Sub

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
        Catch ex As NGL.FreightMaster.Data.DatabaseReadDataException
            'cannot read the database settings so use the config data
            Try
                AdminEmail = My.Settings.AdminEmail
                GroupEmail = My.Settings.GroupEmail
                FromEmail = My.Settings.FromEmail
                SMTPServer = My.Settings.SMTPServer
                SaveOldLog = My.Settings.SaveOldLog
                KeepLogDays = My.Settings.KeepLogDays

                GlobalFuelIndexUpdateEmailNotification = My.Settings.GlobalFuelIndexUpdateEmailNotification
                GlobalFuelIndexUpdateEmailNotificationValue = My.Settings.GlobalFuelIndexUpdateEmailNotificationValue

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
                                 Optional ByVal database_catalog As String = "NGLMASPROD",
                                 Optional ByVal db_server As String = "EC2AMAZ-V2JTOEO",
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




