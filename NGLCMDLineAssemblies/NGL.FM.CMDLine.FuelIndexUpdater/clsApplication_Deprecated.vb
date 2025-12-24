Imports System.IO
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports NGLData = NGL.FreightMaster.Data
Imports BLL = NGL.FM.BLL
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports NGL.FM.CMDLine.FuelIndexUpdater.FuelIndexUpdater

''' <summary>
''' This class has been deprecated and is no longer used. 12/12/2013 - use clsApplication
''' </summary>
''' <remarks></remarks>
Public Class clsApplication_Deprecated : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration
    Public blnUse48Mode As Boolean = False

    Public testParameters As New NGLData.WCFParameters

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

            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("SELECT CarrierControl,CarrierNumber FROM Carrier")
            If Not oQR.Exception Is Nothing Then
                LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because of an unexpected error while reading the carrier data table.", AdminEmail, oQR.Exception, Source & "DoUpdate Failure")
            End If
            Dim dt As System.Data.DataTable = oQR.Data
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In dt.Rows
                    Try
                        If Debug Then Log("Updating carriers fuel fees for carrier control number: " & oRow("CarrierControl"))
                        oCmd = New System.Data.SqlClient.SqlCommand
                        oCmd.Parameters.AddWithValue("@CarrierControl", oRow("CarrierControl"))
                        If oQuery.execNGLStoredProcedure(oCon, oCmd, "spUpdateCarrierFuelFees", 3) Then
                            'Get the Carrier Fuel Addendum Settings
                            strSQL = "SELECT TOP 1 isnull(CarrFuelAdControl,0) as CarrFuelAdControl,isnull(CarrFuelAdUseRatePerMile,0) as CarrFuelAdUseRatePerMile " _
                                & " FROM dbo.CarrierFuelAddendum WHERE CarrFuelAdCarrierControl = " & oRow("CarrierControl") _
                                & " ORDER BY CarrFuelAdControl DESC "
                            oQR = oQuery.ExecuteWithFill(strSQL)
                            If Not oQR.Exception Is Nothing Then
                                LogException("Update Fuel Index Failure (partial update failure)", Source & " DoUpdate could not update some fuel index data because of an unexpected error while reading the CarrierFuelAddendum data table for carrier number " & oRow("CarrierNumber") & ".", AdminEmail, oQR.Exception, Source & "DoUpdate Partial Failure")
                            ElseIf Not oQR.Data Is Nothing AndAlso oQR.Data.Rows.Count > 0 Then
                                Dim CarrFuelAdControl As Integer = oQR.Data(0)("CarrFuelAdControl")
                                Dim CarrFuelAdUseRatePerMile As Integer = oQR.Data(0)("CarrFuelAdUseRatePerMile")
                                Dim strMessage As String = ""
                                Dim strRecID As String = ""
                                'we now get a list of the Book Records for this Carrier
                                If blnUse48Mode Then
                                    strSQL = "Select BookControl,isNull(BookProNumber,'NONE') as BookProNumber, isNull(BookDestState,'') as BookDestState,isNull(BookConsPrefix,'NULL') as BookConsPrefix,isNull(BookRouteConsFlag,0) as BookRouteConsFlag,isNull(BookCustCompControl,0) as BookCustCompControl,isnull(CompNumber,0) as CompNumber  From dbo.Book inner join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl Where BookCarrierControl = " & oRow("CarrierControl") & " AND BookDateLoad	>= '" & strDate & "'  Order By BookConsPrefix, BookRouteConsFlag, BookCustCompControl"
                                Else
                                    strSQL = "Select BookControl,isNull(BookProNumber,'NONE') as BookProNumber, isNull(BookDestState,'') as BookDestState,isNull(BookConsPrefix,'NULL') as BookConsPrefix,isNull(BookRouteConsFlag,0) as BookRouteConsFlag,isNull(BookCustCompControl,0) as BookCustCompControl,isnull(CompNumber,0) as CompNumber  From dbo.Book inner join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl Where BookCarrierControl = " & oRow("CarrierControl") & " AND BookDateLoad	>= '" & strDate & "'  AND isnull(BookLockAllCosts,0) = 0 Order By BookConsPrefix, BookRouteConsFlag, BookCustCompControl"
                                End If

                                'Limited Test Code
                                'strSQL = "Select BookControl,isNull(BookProNumber,'NONE') as BookProNumber, isNull(BookDestState,'') as BookDestState,isNull(BookConsPrefix,'NULL') as BookConsPrefix,isNull(BookRouteConsFlag,0) as BookRouteConsFlag,isNull(BookCustCompControl,0) as BookCustCompControl,isnull(CompNumber,0) as CompNumber  From dbo.Book inner join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl Where BookCarrierControl = " & oRow("CarrierControl") & " AND BookDateLoad	>= '" & strDate & "' AND BookCustCompControl = 1 Order By BookConsPrefix, BookRouteConsFlag, BookCustCompControl"
                                oQR = oQuery.ExecuteWithFill(strSQL)
                                Dim bt As System.Data.DataTable = oQR.Data
                                If Not oQR.Exception Is Nothing Then
                                    LogException("Update Fuel Index Failure (partial update failure)", Source & " DoUpdate could not update some fuel index data because of an unexpected error while reading the Book data table for carrier number " & oRow("CarrierNumber") & ".", AdminEmail, oQR.Exception, Source & "DoUpdate Partial Failure")
                                ElseIf Not bt Is Nothing AndAlso bt.Rows.Count > 0 Then
                                    Dim htCarrFuelCost As New Hashtable

                                    If Debug Then Console.WriteLine("Carrier Number: " & oRow("CarrierNumber")) : Console.WriteLine()

                                    For Each oBR In bt.Rows
                                        Dim BookControl As Integer = oBR("BookControl")
                                        Dim BookProNumber As String = oBR("BookProNumber")
                                        Dim BookDestState As String = oBR("BookDestState")
                                        Dim BookConsPrefix As String = oBR("BookConsPrefix")
                                        Dim CompNumber As Integer = oBR("CompNumber")
                                        Dim BookRouteConsFlag As Integer = 0
                                        If oBR("BookRouteConsFlag") Then
                                            BookRouteConsFlag = 1
                                        End If
                                        Dim BookCustCompControl As Integer = oBR("BookCustCompControl")
                                        Dim blnRecalcCost As Boolean = False
                                        If Debug Then Console.CursorLeft = 1 : Console.Write("Processing Book Pro Number: " & BookProNumber)
                                        Try
                                            If BookDestState.Trim.Length > 0 Then
                                                If Not htCarrFuelCost.ContainsKey(BookDestState) Then
                                                    'we need to look up the carrier fuel cost
                                                    strSQL = "select IsNULL(dbo.udfGetFuelSurcharge(dbo.udfGetAvgFuelPrice (" & oRow("CarrierControl") & ",'" & BookDestState & "','" & strDate & "')," & CarrFuelAdControl & "," & CarrFuelAdUseRatePerMile & ",'" & BookDestState & "','" & strDate & "'),0) as FuelSurCharge"
                                                    Dim FuelSurCharge As String = oQuery.getScalarValue(oCon, strSQL, 3)
                                                    Dim dblVal As Double = 0
                                                    Double.TryParse(FuelSurCharge, dblVal)
                                                    htCarrFuelCost(BookDestState) = dblVal
                                                End If
                                                'now update the book data
                                                If Not updateBookFees(oCon, oQuery, BookControl, BookProNumber, htCarrFuelCost(BookDestState), CarrFuelAdUseRatePerMile, strMessage) Then
                                                    addToCompanyMessages(BookCustCompControl, htCompanyResults, strMessage, CompNumber, True)
                                                Else
                                                    blnRecalcCost = True
                                                    If Not calcBookCosts(oCon, oQuery, BookControl, BookProNumber, strMessage) Then
                                                        addToCompanyMessages(BookCustCompControl, htCompanyResults, strMessage, CompNumber, True)
                                                    Else
                                                        addToCompanyMessages(BookCustCompControl, htCompanyResults, "Pro Number " & BookProNumber & ", " & vbCrLf, CompNumber)
                                                    End If
                                                End If
                                            Else
                                                'add to the company errors 
                                                addToCompanyMessages(BookCustCompControl, htCompanyResults, " <br />The Pro Number " & BookProNumber & " does not have a valid Destination State.<br />" & vbCrLf, CompNumber, True)
                                            End If
                                        Catch ex As Exception
                                            Try
                                                addToCompanyMessages(BookCustCompControl, htCompanyResults, " <br /> Could not update the Fuel for Pro Number " & BookProNumber & " because of an unexpected error: " & ex.Message, CompNumber, True)
                                            Catch e As Exception
                                                'do nothing skip to next record
                                            End Try
                                        End Try
                                    Next
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        Try
                            LogException("Update Fuel Index Partial Failure for Carrier " & oRow("CarrierNumber"), Source & " DoUpdate could not update fuel index because of an unexpected error while reading the carrier data table.", AdminEmail, ex, Source & "DoUpdate Partial Failure")
                        Catch e As Exception
                            'do nothing skp to next record
                        End Try
                    End Try
                Next
            End If
        Catch ex As NGL.Core.DatabaseRetryExceededException
            LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because too many execute failures were reported.", AdminEmail, ex, Source & ".DoUpdate Failed")
        Catch ex As NGL.Core.DatabaseLogInException
            LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because of a database login failure.", AdminEmail, ex, Source & ".DoUpdate Failed")
        Catch ex As NGL.Core.DatabaseInvalidException
            LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because a connection to the database could not be established.", AdminEmail, ex, Source & ".DoUpdate Failed")
        Catch ex As Exception
            LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because of an unexpected error.", AdminEmail, ex, Source & "DoUpdate Failure")
        Finally
            Try
                oCmd.Cancel()
                oCmd = Nothing
            Catch ex As Exception

            End Try
            Try
                If Not oCon Is Nothing AndAlso oCon.State = ConnectionState.Open Then oCon.Close()
                oCon = Nothing
            Catch ex As Exception

            End Try
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



    Private Function calcBookCosts(ByRef oCon As System.Data.SqlClient.SqlConnection, _
                                   ByRef oQuery As NGL.Core.Data.Query, _
                                   ByVal BookControl As Integer, _
                                   ByVal BookProNumber As String, _
                                   ByRef Message As String) As Boolean
        Dim blnRet As Boolean = False
        Dim oBcmd As New System.Data.SqlClient.SqlCommand
        Try
            oBcmd.Parameters.AddWithValue("@BookControl", BookControl)
            blnRet = oQuery.execNGLStoredProcedure(oCon, oBcmd, "dbo.spCalcBookRev", 1)
        Catch ex As NGL.Core.DatabaseRetryExceededException
            LogException("Update Book Fuel Surcharge Calculate Costs Failure (partial update failure)", Source & " calcBookCosts could not update the booking costs for Pro Number " & BookProNumber & " because too many execute failures were reported.", AdminEmail, ex, Source & ".calcBookCosts Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
        Catch ex As NGL.Core.DatabaseLogInException
            LogException("Update Book Fuel Surcharge Calculate Costs Failure (partial update failure)", Source & " calcBookCosts could not update the booking costs for Pro Number " & BookProNumber & " because of a database login failure.", AdminEmail, ex, Source & ".calcBookCosts Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
        Catch ex As NGL.Core.DatabaseInvalidException
            LogException("Update Book Fuel Surcharge Calculate Costs Failure (partial update failure)", Source & " calcBookCosts could not update the booking costs for Pro Number " & BookProNumber & " because a connection to the database could not be established.", AdminEmail, ex, Source & ".calcBookCosts Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
        Catch ex As NGL.Core.DatabaseDataValidationException
            LogException("Update Book Fuel Surcharge Calculate Costs Failure (partial update failure)", Source & " calcBookCosts could not update the booking costs for Pro Number " & BookProNumber & " because of a data validation failure.", AdminEmail, ex, Source & ".calcBookCosts Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because " & ex.Message & ".<br />" & vbCrLf
        Catch ex As Exception
            LogException("Update Book Fuel Surcharge Calculate Costs Failure (partial update failure)", Source & " updateBookFees could not update the book fees data for Pro Number " & BookProNumber & " because of an unexpected error.", AdminEmail, ex, Source & ".UpdateBookFees Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because " & ex.Message & ".<br />" & vbCrLf
        Finally
            Try
                oBcmd.Cancel()
                oBcmd = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function

    Private Function updateBookFees(ByRef oCon As System.Data.SqlClient.SqlConnection, _
                                   ByRef oQuery As NGL.Core.Data.Query, _
                                   ByVal BookControl As Integer, _
                                   ByVal BookProNumber As String, _
                                   ByVal FuelSurCharge As Double, _
                                   ByVal CarrFuelAdUseRatePerMile As Double, _
                                   ByRef Message As String) As Boolean
        Dim blnRet As Boolean = False
        Dim oBcmd As New System.Data.SqlClient.SqlCommand
        Dim intRetryCt As Integer = 0
        Try


            Do
                intRetryCt += 1
                Try
                    oBcmd.Parameters.AddWithValue("@BookControl", BookControl)
                    oBcmd.Parameters.AddWithValue("@SurCharge", FuelSurCharge)
                    oBcmd.Parameters.AddWithValue("@UseRatePerMile", CarrFuelAdUseRatePerMile)
                    'Throw New Ngl.Core.DatabaseInvalidException("Test invalid Database Exception")
                    blnRet = oQuery.execNGLStoredProcedure(oCon, oBcmd, "dbo.spUpdateBookFuelSurChargeNoCalc", 1)
                    Exit Do
                Catch ex As NGL.Core.DatabaseRetryExceededException
                    LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because too many execute failures were reported.", AdminEmail, ex, Source & ".UpdateBookFees Partial Failure")
                    Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
                    Exit Do
                Catch ex As NGL.Core.DatabaseLogInException
                    If intRetryCt > 3 Then
                        LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because of a database login failure.", AdminEmail, ex, Source & ".UpdateBookFees Partial Failure")
                        Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
                    Else
                        Try
                            oBcmd.Cancel()
                            oBcmd = New System.Data.SqlClient.SqlCommand
                        Catch e As Exception

                        End Try
                        'goto sleep for 2 seconds and try again. this typically happens during a dead lock.
                        System.Threading.Thread.Sleep(2000)
                    End If
                Catch ex As NGL.Core.DatabaseInvalidException
                    If intRetryCt > 3 Then
                        LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because a connection to the database could not be established.", AdminEmail, ex, Source & ".UpdateBookFees Partial Failure")
                        Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
                    Else
                        Try
                            oBcmd.Cancel()
                            oBcmd = New System.Data.SqlClient.SqlCommand
                        Catch e As Exception

                        End Try
                        Try
                            If oCon.State <> ConnectionState.Closed Then oCon.Close()
                            oCon = New System.Data.SqlClient.SqlConnection
                        Catch conErr As Exception

                        End Try
                        'goto sleep for 2 seconds and try again. this typically happens during a dead lock.
                        System.Threading.Thread.Sleep(2000)
                    End If
                Catch ex As NGL.Core.DatabaseDataValidationException
                    LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " updateBookFees could not update the book fees data for Pro Number " & BookProNumber & " because of a data validation failure.", AdminEmail, ex, Source & ".UpdateBookFees Partial Failure")
                    Message = " <br />The Pro Number " & BookProNumber & " was not updated because " & ex.Message & ".<br />" & vbCrLf
                    Exit Do
                Catch ex As Exception
                    Throw
                End Try
            Loop Until intRetryCt > 3 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " updateBookFees could not update the book fees data for Pro Number " & BookProNumber & " because of an unexpected error.", AdminEmail, ex, Source & ".UpdateBookFees Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because " & ex.Message & ".<br />" & vbCrLf
        Finally
            Try
                oBcmd.Cancel()
                oBcmd = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
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
            blnUse48Mode = My.Settings.Use48Mode
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

        If testParameters Is Nothing Then testParameters = New NGLData.WCFParameters
        With testParameters
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



