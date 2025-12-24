Imports System.IO
Imports Ngl.Core
Imports Ngl.FreightMaster.Core
Imports Ngl.Core.Communication

Public Class clsApplication : Inherits Ngl.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New Ngl.FreightMaster.Core.UserConfiguration

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
           
            Dim intRet As Integer = 0
            Dim LastError As String = ""
            Log("Begin Process Data ")
             
            Dim updater As New SyncNatFuelIndex
            intRet = updater.Sync(ConnectionString)
            LastError = updater.LastError

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
        'Debug Code
        'Dim strDate As String = "10/17/2011"
        Dim htCompanyUpdates As New Hashtable
        Dim htCompanyErrors As New Hashtable
        Dim htCompanyResults As New Hashtable
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Try

            Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill("SELECT CarrierControl,CarrierNumber FROM Carrier")
            'Debug code only uses one carriercontrol (557)
            'Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill("SELECT CarrierControl,CarrierNumber FROM Carrier WHERE CarrierControl in (557,597)")
            If Not oQR.Exception Is Nothing Then
                LogException("Update Fuel Index Failure", Source & " DoUpdate could not update fuel index because of an unexpected error while reading the carrier data table.", AdminEmail, oQR.Exception, Source & "DoUpdate Failure")
            End If
            Dim dt As System.Data.DataTable = oQR.Data
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In dt.Rows
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
                            'Dim lastBookControl As Integer = 0
                            'Dim lastBookPro As String = 0
                            'Dim lastBookRouteConsFlag As Integer = -1
                            'Dim lastBookConsPrefix As String = "NONE"
                            'Dim lastCompNumber As Integer = 0
                            'Dim lastBookCustCompControl As Integer = 0
                            'Dim lastState As String = ""
                            Dim strMessage As String = ""
                            Dim strRecID As String = ""
                            'we now get a list of the Book Records for this Carrier
                            strSQL = "Select BookControl,isNull(BookProNumber,'NONE') as BookProNumber, isNull(BookDestState,'') as BookDestState,isNull(BookConsPrefix,'NULL') as BookConsPrefix,isNull(BookRouteConsFlag,0) as BookRouteConsFlag,isNull(BookCustCompControl,0) as BookCustCompControl,isnull(CompNumber,0) as CompNumber  From dbo.Book inner join dbo.Comp on dbo.Book.BookCustCompControl = dbo.Comp.CompControl Where BookCarrierControl = " & oRow("CarrierControl") & " AND BookDateLoad	>= '" & strDate & "'  AND isnull(BookLockAllCosts,0) = 0 Order By BookConsPrefix, BookRouteConsFlag, BookCustCompControl"
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
                                        ''test if we are on a new truck and calculate costs for the previous load accordingly
                                        'If blnRecalcCost Then
                                        '    If lastBookRouteConsFlag = -1 Or lastBookConsPrefix = "NONE" Then
                                        '        'save the data for the next loop
                                        '        lastBookControl = BookControl
                                        '        lastBookConsPrefix = BookConsPrefix
                                        '        lastBookRouteConsFlag = BookRouteConsFlag
                                        '        lastState = BookDestState
                                        '        lastBookPro = BookProNumber
                                        '        lastCompNumber = CompNumber
                                        '        lastBookCustCompControl = BookCustCompControl
                                        '    ElseIf lastBookRouteConsFlag = 0 Then
                                        '        'this is a new CNS number and the previous route consolidation flag is unchecked (not grouped) 
                                        '        'so calculate costs for the previous load
                                        '        If Not calcBookCosts(oCon, oQuery, lastBookControl, lastBookPro, strMessage) Then
                                        '            addToCompanyMessages(lastBookCustCompControl, htCompanyResults, strMessage, lastCompNumber, True)
                                        '        Else
                                        '            addToCompanyMessages(lastBookCustCompControl, htCompanyResults, "Pro Number " & lastBookPro & ", " & vbCrLf, lastCompNumber)
                                        '        End If
                                        '        'save the data for the next loop
                                        '        lastBookControl = BookControl
                                        '        lastBookConsPrefix = BookConsPrefix
                                        '        lastBookRouteConsFlag = BookRouteConsFlag
                                        '        lastState = BookDestState
                                        '        lastBookPro = BookProNumber
                                        '        lastCompNumber = CompNumber
                                        '        lastBookCustCompControl = BookCustCompControl
                                        '    ElseIf lastBookRouteConsFlag = 1 AndAlso (lastBookConsPrefix <> BookConsPrefix OrElse BookConsPrefix = "NULL" OrElse BookConsPrefix.Trim.Length < 1) Then
                                        '        'this is a new consolidation number so so calculate costs for the previous load
                                        '        'no consolidation is assigned to this load so calculate costs for the previous load

                                        '        If (lastBookConsPrefix = "NULL" OrElse lastBookConsPrefix.Trim.Length < 1) Then
                                        '            strRecID = "Pro Number " & lastBookPro
                                        '        Else
                                        '            strRecID = "CNS Number " & lastBookConsPrefix
                                        '        End If
                                        '        If Not calcBookCosts(oCon, oQuery, lastBookControl, lastBookPro, strMessage) Then
                                        '            addToCompanyMessages(lastBookCustCompControl, htCompanyResults, strMessage & " <br />" & strRecID & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf, lastCompNumber, True)
                                        '        Else
                                        '            addToCompanyMessages(lastBookCustCompControl, htCompanyResults, strRecID & ", " & vbCrLf, lastCompNumber)
                                        '        End If
                                        '        'save the data for the next loop
                                        '        lastBookControl = BookControl
                                        '        lastBookConsPrefix = BookConsPrefix
                                        '        lastBookRouteConsFlag = BookRouteConsFlag
                                        '        lastState = BookDestState
                                        '        lastBookPro = BookProNumber
                                        '        lastCompNumber = CompNumber
                                        '        lastBookCustCompControl = BookCustCompControl
                                        '    Else
                                        '        'this is part of the current consolidaiton so just save the previous data for the next loop
                                        '        lastBookControl = BookControl
                                        '        lastBookConsPrefix = BookConsPrefix
                                        '        lastBookRouteConsFlag = BookRouteConsFlag
                                        '        lastState = BookDestState
                                        '        lastBookPro = BookProNumber
                                        '        lastCompNumber = CompNumber
                                        '        lastBookCustCompControl = BookCustCompControl
                                        '    End If
                                        'Else
                                        '    'there was an error so just save the previous data an move on to the next record
                                        '    lastBookControl = BookControl
                                        '    lastBookConsPrefix = BookConsPrefix
                                        '    lastBookRouteConsFlag = BookRouteConsFlag
                                        '    lastState = BookDestState
                                        '    lastBookPro = BookProNumber
                                        '    lastCompNumber = CompNumber
                                        '    lastBookCustCompControl = BookCustCompControl
                                        'End If
                                    Else
                                        'add to the company errors 
                                        addToCompanyMessages(BookCustCompControl, htCompanyResults, " <br />The Pro Number " & BookProNumber & " does not have a valid Destination State.<br />" & vbCrLf, CompNumber, True)
                                    End If
                                Next
                                'Code removed by RHR 10/20/11 we now update the cost for each booking record not once for each CNS
                                ''update the final last booking data
                                'If (lastBookConsPrefix = "NULL" OrElse lastBookConsPrefix.Trim.Length < 1) Then
                                '    strRecID = "Pro Number " & lastBookPro
                                'Else
                                '    strRecID = "CNS Number " & lastBookConsPrefix
                                'End If
                                'If Not calcBookCosts(oCon, oQuery, lastBookControl, lastBookPro, strMessage) Then
                                '    addToCompanyMessages(lastBookCustCompControl, htCompanyResults, strMessage & " <br />" & strRecID & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf, lastCompNumber, True)
                                'Else
                                '    addToCompanyMessages(lastBookCustCompControl, htCompanyResults, strRecID & ", " & vbCrLf, lastCompNumber)
                                'End If
                            End If
                        End If
                    End If
                Next
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
    End Sub



    Private Function calcBookCosts(ByRef oCon As System.Data.SqlClient.SqlConnection, _
                                   ByRef oQuery As Ngl.Core.Data.Query, _
                                   ByVal BookControl As Integer, _
                                   ByVal BookProNumber As String, _
                                   ByRef Message As String) As Boolean
        Dim blnRet As Boolean = False
        Dim oBcmd As New System.Data.SqlClient.SqlCommand
        Try
            oBcmd.Parameters.AddWithValue("@BookControl", BookControl)
            blnRet = oQuery.execNGLStoredProcedure(oCon, oBcmd, "dbo.spCalcBookRev", 3)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because too many execute failures were reported.", AdminEmail, ex, Source & ".DoUpdate Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
        Catch ex As Ngl.Core.DatabaseLogInException
            LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because of a database login failure.", AdminEmail, ex, Source & ".DoUpdate Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
        Catch ex As Ngl.Core.DatabaseInvalidException
            LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because a connection to the database could not be established.", AdminEmail, ex, Source & ".DoUpdate Partial Failure")
            Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf
        Catch ex As Exception
            Throw
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
                                   ByRef oQuery As Ngl.Core.Data.Query, _
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
                    blnRet = oQuery.execNGLStoredProcedure(oCon, oBcmd, "dbo.spUpdateBookFuelSurChargeNoCalc", 3)
                    Exit Do
                Catch ex As Ngl.Core.DatabaseRetryExceededException
                    LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because too many execute failures were reported.", AdminEmail, ex, Source & ".DoUpdate Partial Failure")
                    Message = " <br />The Pro Number " & BookProNumber & " was not updated because of a system error (contact your system administrator for more information).<br />" & vbCrLf

                Catch ex As Ngl.Core.DatabaseLogInException
                    If intRetryCt > 3 Then
                        LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because of a database login failure.", AdminEmail, ex, Source & ".DoUpdate Partial Failure")
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
                Catch ex As Ngl.Core.DatabaseInvalidException
                    If intRetryCt > 3 Then
                        LogException("Update Book Fuel Surcharge Failure (partial update failure)", Source & " DoUpdate could not update the book data for Pro Number " & BookProNumber & " because a connection to the database could not be established.", AdminEmail, ex, Source & ".DoUpdate Partial Failure")
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
                Catch ex As Exception
                    Throw
                End Try
            Loop Until intRetryCt > 3 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            Throw
        Finally
            Try
                oBcmd.Cancel()
                oBcmd = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function

End Class

Public Class CompResults
    Public CompControl As Integer = 0
    Public CompNumber As Integer = 0
    Public ErrMsg As String = ""
    Public UpdateMsg As String = ""
    Public UpdateCount As Integer = 0
    Public ErrCount As Integer = 0
End Class


