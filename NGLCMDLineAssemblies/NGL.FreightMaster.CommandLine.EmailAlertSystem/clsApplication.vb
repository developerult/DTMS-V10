Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.FreightMaster.Data.DataTransferObjects

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    Private oEmailAlertData As NGL.FreightMaster.Data.EmailAlertsByCompany.EmailAlertsDataDataTable

    Private Function CreateaDataProperties() As Data.WCFParameters

        Dim data As FreightMaster.Data.WCFParameters = New FreightMaster.Data.WCFParameters

        data.UserName = "NGLSystem"
        data.Database = Me.Database
        data.DBServer = Me.DBServer
        data.WCFAuthCode = "NGLSystem"
        data.ConnectionString = Me.ConnectionString
        Return data

    End Function

    Private Enum enmAlertTypes As Integer
        AlertCarrierExposurePerShipment
        AlertCarrierExposureAll
        AlertOutdatedStaging
        AlertOutdatedNStatus
        AlertCarrierContractExpired
        AlertCarrierInsuranceExpired
        AlertOutdatedNoLanePO
        AlertSystemErrors
        AlertPOsWaiting
    End Enum

    Public Sub ProcessData()
        Try
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
            Console.Write("Reading Alerts: ")
            Console.CursorLeft = 17
            Console.Write("N Status Alerts")
            'Get the N Status Alerts
            getOutdatedNStatusOrdersAlerts(oQuery)
            Console.CursorLeft = 17
            Console.Write(" Outdated Staging Table Alerts")
            'Get the Outdated Staging Table Alerts
            getOutdatedStagingTableAlerts(oQuery)
            Console.CursorLeft = 17
            Console.Write(" Carrier Contract Expired Alerts")
            'Get the Carrier Contract Expired Alerts
            getCarrierContractExpiredAlerts(oQuery)
            Console.CursorLeft = 17
            Console.Write(" Carrier Insurance Expired Alerts")
            'Get the Carrier Insurance Expired Alerts
            getCarrierInsuranceExpiredAlerts(oQuery)
            Console.CursorLeft = 17
            Console.Write(" Carrier Insurance Exposure Per Shipment Alerts")
            'Get the Carrier Insurance Exposure Per Shipment Alerts
            getCarrierInsuranceExposurePerShipmentAlerts(oQuery)
            Console.CursorLeft = 17
            Console.Write(" Carrier Insurance Exposure All Shipments Alerts")
            'Get the Carrier Insurance Exposure All Shipment Alerts
            getCarrierInsuranceExposureAllShipmentsAlerts(oQuery)
            Console.CursorLeft = 17
            Console.Write(" Outdated No Lane PO Order Alerts")
            'Get the Outdated No Lane PO Order Alerts
            getOutdatedNoLanePOAlerts(oQuery)
            Console.CursorLeft = 17
            Console.Write(" POs Waiting Alerts")
            'Get the POs Waiting Alerts
            getPOsWaitingAlerts(oQuery)
            'Process the Emails for the above alerts
            Console.CursorLeft = 17
            Console.Write(" Process Emails For Alerts")
            ProcessEmails()
            Console.CursorLeft = 17
            Console.Write(" System Errors Alerts")
            'Run the system email alert procedure
            getSystemErrorAlerts(oQuery)
        Catch ex As Exception
            Throw New ApplicationException(Source & " Process Data Failure", ex)
        End Try
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

    Private Function addEmailAlert(ByRef oQuery As NGL.Core.Data.Query, _
                        ByVal CompControl As Integer, _
                        ByVal CompNumber As String, _
                        ByVal AlertType As enmAlertTypes, _
                        Optional ByVal CarrierControl As Integer = 0, _
                        Optional ByVal CarrierNumber As String = "") As Boolean
        Dim blnRet As Boolean
        Dim oRow As NGL.FreightMaster.Data.EmailAlertsByCompany.EmailAlertsDataRow
        Dim blnAddNew As Boolean = True
        Try
            If Me.isAlertOn(CompControl, AlertType) Then
                If Not oEmailAlertData Is Nothing AndAlso oEmailAlertData.Count > 0 Then
                    Dim oRows() As System.Data.DataRow = oEmailAlertData.Select("CompanyControl = " & CompControl & " AND AlertType = " & CType(AlertType, Integer))
                    If oRows.Length > 0 Then
                        blnAddNew = False
                        oRow = CType(oRows(0), NGL.FreightMaster.Data.EmailAlertsByCompany.EmailAlertsDataRow)
                        oRow.AlertCount += 1
                    End If
                Else
                    oEmailAlertData = New Data.EmailAlertsByCompany.EmailAlertsDataDataTable
                End If
                If blnAddNew Then
                    oRow = oEmailAlertData.NewEmailAlertsDataRow
                    oRow.CompanyControl = CompControl
                    oRow.CompanyNumber = Left(CompNumber, 20)
                    oRow.AlertType = CType(AlertType, Integer)
                    oRow.AlertCount = 1
                    oRow.CarrierControl = CarrierControl
                    oRow.CarrierNumber = Left(CarrierNumber, 20)
                    oEmailAlertData.AddEmailAlertsDataRow(oRow)
                    Dim strEmails As String = getCompContEmails(oQuery, CompControl)
                    oRow.Email = getFirstEmail(strEmails)
                    oRow.CCEmail = getCCEmail(strEmails)
                End If
                oEmailAlertData.AcceptChanges()
            End If
            blnRet = True

        Catch ex As Exception
            Throw New ApplicationException(Source & " Add Email Alert Failure", ex)
        End Try
        Return blnRet


    End Function

    Private Function getEmailAlertString(ByVal enmAlertType As enmAlertTypes) As String

        Select Case enmAlertType
            Case enmAlertTypes.AlertCarrierExposurePerShipment
                Return "AlertCarrierExposurePerShipmentFlag"
            Case enmAlertTypes.AlertCarrierExposureAll
                Return "AlertCarrierExposureAllFlag"
            Case enmAlertTypes.AlertOutdatedStaging
                Return "AlertOutdatedStagingFlag"
            Case enmAlertTypes.AlertOutdatedNStatus
                Return "AlertOutdatedNStatusFlag"
            Case enmAlertTypes.AlertCarrierContractExpired
                Return "AlertCarrierContractExpired"
            Case enmAlertTypes.AlertCarrierInsuranceExpired
                Return "AlertCarrierInsuranceExpired"
            Case enmAlertTypes.AlertOutdatedNoLanePO
                Return "AlertOutdatedNoLanePOFlag"
            Case enmAlertTypes.AlertPOsWaiting
                Return "AlertPOsWaitingFlag"
            Case enmAlertTypes.AlertSystemErrors
                Return "AlertSystemErrors"
            Case Else
                Return ""
        End Select
    End Function

    Private Function getEmailAlertMessage(ByVal AlertType As enmAlertTypes, Optional ByVal CompControl As Integer = 0) As String
        Dim strRet As String = ""
        Try
            Dim oParameter As New NGL.FreightMaster.Core.Model.Parameter(Me.oConfig)
            If Not oParameter.testConnection() Then
                Log(Source & " Database Connection Failure: " & oParameter.LastError)
                Return False
            End If
            strRet = oParameter.getParText(Me.getEmailAlertString(AlertType), CompControl)
        Catch ex As Exception
            Throw New ApplicationException(Source & " Validate Alert Parameter Failure", ex)
        End Try
        Return strRet

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="AlertType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function isAlertOn(ByVal CompControl As Integer, ByVal AlertType As enmAlertTypes) As Boolean
        Dim blnRet As Boolean = True
        Try
            Dim oParameter As New NGL.FreightMaster.Core.Model.Parameter(Me.oConfig)
            If Not oParameter.testConnection() Then
                Log(Source & " Database Connection Failure: " & oParameter.LastError)
                Return False
            End If
            If oParameter.getParValue(Me.getEmailAlertString(AlertType), CompControl) = 0 Then
                blnRet = False
                If Me.Debug Then Log("The " & Me.getEmailAlertMessage(AlertType, CompControl) & " is disabled for company control number " & CompControl.ToString)
            End If
        Catch ex As Exception
            Throw New ApplicationException(Source & " Validate Alert Parameter Failure", ex)
        End Try
        Return blnRet

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks></remarks>
    Private Sub getOutdatedNStatusOrdersAlerts(ByRef oQuery As NGL.Core.Data.Query)
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select BookControl,BookProNumber,BookDateLoad,QueryDate,CompControl,CompNumber From dbo.udfAlertsOutdatedNStatusOrders() Order By CompControl")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        If Not addEmailAlert(oQuery, oRow("CompControl"), oRow("CompNumber"), enmAlertTypes.AlertOutdatedNStatus) Then
                            Log(Source & " Cannot Process " & Me.getEmailAlertString(enmAlertTypes.AlertOutdatedNStatus) & " Email Alerts for Company Control " & oRow("CompControl"))
                        End If
                    Next
                    oDT = Nothing
                End If
            Else
                Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertOutdatedNStatus) & " Emails:" & oQuery.LastError)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get Outdated N Status Orders Alerts Failure", "There was an unexpected error while reading the  Outdated N Status Orders Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks></remarks>
    Private Sub getOutdatedNoLanePOAlerts(ByRef oQuery As NGL.Core.Data.Query)
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select CompControl, POHNLDefaultCustomer From dbo.udfAlertsOutdatedNoLaneAlerts()")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        If Not addEmailAlert(oQuery, oRow("CompControl"), oRow("POHNLDefaultCustomer"), enmAlertTypes.AlertOutdatedNoLanePO) Then
                            Log(Source & " Cannot Process " & Me.getEmailAlertString(enmAlertTypes.AlertOutdatedNoLanePO) & " Email Alerts for Company Control " & oRow("CompControl"))
                        End If
                    Next
                    oDT = Nothing
                End If
            Else
                Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertOutdatedNoLanePO) & " Emails:" & oQuery.LastError)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get Outdated No Lane PO Order Alerts Failure", "There was an unexpected error while reading the  Outdated No Lane PO Order Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use optional parameters
    ''' </remarks>
    Private Sub getSystemErrorAlerts(ByRef oQuery As NGL.Core.Data.Query)
        Dim oDT As System.Data.DataTable
        Dim strEmailTo As String
        Dim dblAlertHistoryMinutes As Double = 0
        If oQuery Is Nothing Then Return
        'get the global system error alert parameter settings
        Try
            Dim oParameter As New NGL.FreightMaster.Core.Model.Parameter(Me.oConfig)
            If Not oParameter.testConnection() Then
                Log(Source & " Database Connection Failure: " & oParameter.LastError)
                Exit Sub
            End If
            If oParameter.getParValue("AlertSystemErrors") <> 1 Then Exit Sub 'System Error Alerts are turned off
            strEmailTo = oParameter.getParText("AlertSystemErrors")
            If String.IsNullOrEmpty(strEmailTo) Then
                Log(Source & " System Email Alerts Warning: An email address is not provided in the AlertSystemErrors parameter text field.")
                Exit Sub 'no email address is available
            End If
            dblAlertHistoryMinutes = oParameter.getParValue("AlertSystemErrorsMinutes")
        Catch ex As Exception
            Throw New ApplicationException(Source & " Read System Alert Parameter Failure", ex)
        End Try
        Try
            Dim strMessage As String = ""
            Dim oEmail As New NGL.Core.Communication.Email
            Dim objIniFile As New NGL.Core.Communication.IniFile(APPPath() & "\FreightMaster.ini")
            'Test if we need to send the email alert based on the dblAlertHistoryMinutes
            Dim dtDefault As Date = Now.AddMinutes(((dblAlertHistoryMinutes + 1) * -1))
            Dim strLastDate = objIniFile.GetString(INIKey, "AlertSystemErrors-LastRanOn", dtDefault.ToString("g"))
            Dim dtLastRanOn As DateTime
            If Not Date.TryParse(strLastDate, dtLastRanOn) Then
                dtLastRanOn = dtDefault
            End If
            Dim dtNow As Date = Now
            If dtNow.AddMinutes(dblAlertHistoryMinutes * -1) < dtLastRanOn Then
                Log(Source & " Send System Error Alerts: Waiting on next Email Cycle, Last Ran On " & dtLastRanOn.ToString("g") & "! ")
            Else
                Dim strSQL As String = "SELECT [ErrID],[Message],[Record],[CurrentUser],[CurrentDate],[ErrorNumber],[ErrorSeverity],[ErrorState],[ErrorProcedure],[ErrorLineNbr]"
                strSQL &= "FROM [dbo].[tblSystemErrors] WHERE datediff(n,CurrentDate,getdate()) < " & dblAlertHistoryMinutes.ToString & " ORDER BY CurrentDate"
                Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
                If oQR.Exception Is Nothing Then
                    oDT = oQR.Data
                    If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                        For Each oRow As System.Data.DataRow In oDT.Rows
                            strMessage &= String.Format("<hr /><h2> The Procedure {0} Raised Error Number: {1} On {2} </h2>", getDataRowString(oRow, "ErrorProcedure", "NA"), getDataRowString(oRow, "ErrorNumber", "NA"), getDataRowString(oRow, "CurrentDate", ""))
                            strMessage &= String.Format("<p>Message:<br />{0}</p>", getDataRowString(oRow, "Message", ""))
                            strMessage &= String.Format("<p>Record:<br />{0}</p>", getDataRowString(oRow, "Record", ""))
                            strMessage &= String.Format("<p>User:<br />{0}</p>", getDataRowString(oRow, "CurrentUser", ""))
                            strMessage &= String.Format("<p>Severity:<br />{0}</p>", getDataRowString(oRow, "ErrorSeverity", ""))
                            strMessage &= String.Format("<p>State:<br />{0}</p>", getDataRowString(oRow, "ErrorState", ""))
                            strMessage &= String.Format("<p>Line Nbr:<br />{0}</p>", getDataRowString(oRow, "ErrorLineNbr", ""))
                        Next
                        oDT = Nothing
                        'Send the email message
                        'Modified By LVV 2/18/16 v-7.0.5.0
                        If Not oEmail.SendMail(Me.SMTPServer, strEmailTo, Me.FromEmail, strMessage, "FreightMaster System Error Alert Message", "", SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                            Log(Source & " Send System Errors Email Failure: " & oEmail.LastError)
                        Else
                            Try
                                Log(Source & " Email Sent to " & strEmailTo)
                                objIniFile.WriteString(INIKey, "AlertSystemErrors-LastRanOn", Now.ToString("g"))
                            Catch ex As Exception
                                'do nothing
                            End Try
                        End If
                    End If
                Else
                    Log(Source & " Cannot Process Alert System Errors Emails:" & oQuery.LastError)
                End If
                oQR = Nothing
            End If
        Catch ex As Exception
            LogError("Get System Errors Alerts Failure", "There was an unexpected error while reading the System Errors Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception
                'do nothing
            End Try
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks></remarks>
    Private Sub getOutdatedStagingTableAlerts(ByRef oQuery As NGL.Core.Data.Query)

        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select DefaultCustomer,CompControl From dbo.udfAlertsOutdatedStagingOrders() Order By CompControl")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        If Not addEmailAlert(oQuery, oRow("CompControl"), oRow("DefaultCustomer"), enmAlertTypes.AlertOutdatedStaging) Then
                            Log(Source & " Cannot Process " & Me.getEmailAlertString(enmAlertTypes.AlertOutdatedStaging) & " Email Alerts for Company Control " & oRow("CompControl"))
                        End If
                    Next
                    oDT = Nothing
                End If
            Else
                Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertOutdatedStaging) & " Emails:" & oQuery.LastError)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get Outdated Staging Table Alerts Failure", "There was an unexpected error while reading the Outdated Staging Table Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks></remarks>
    Private Sub getPOsWaitingAlerts(ByRef oQuery As NGL.Core.Data.Query)

        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select DefaultCustomer,CompControl From dbo.udfAlertsPOsWaiting() Order By CompControl")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        If Not addEmailAlert(oQuery, oRow("CompControl"), oRow("DefaultCustomer"), enmAlertTypes.AlertPOsWaiting) Then
                            Log(Source & " Cannot Process " & Me.getEmailAlertString(enmAlertTypes.AlertPOsWaiting) & " Email Alerts for Company Control " & oRow("CompControl"))
                        End If
                    Next
                    oDT = Nothing
                End If
            Else
                Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertPOsWaiting) & " Emails:" & oQuery.LastError)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get POs Waiting Alerts Failure", "There was an unexpected error while reading the POs Waiting Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks></remarks>
    Private Sub getCarrierContractExpiredAlerts(ByRef oQuery As NGL.Core.Data.Query)
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return

        Try
          

            'Get the Carrier Contract Expired Alerts
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select CarrierControl,CarrierNumber From  dbo.udfAlertsCarrierContractExpired (NULL) Order By CarrierControl")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    Dim intCarrierControl As Integer = 0
                    Dim strCarrierNumber As String = ""
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        intCarrierControl = oRow("CarrierControl")
                        strCarrierNumber = NGL.Core.Utility.DataTransformation.CleanNullableString(oRow("CarrierNumber"))
                        Dim oBookQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select distinct BookCustCompControl, CompNumber From dbo.Book inner join dbo.Comp on BookCustCompControl = CompControl Where BookCarrierControl = " & intCarrierControl & "  AND BookDateDelivered is null Order By BookCustCompControl ")
                        If oBookQR.Exception Is Nothing Then
                            Dim oBookDT As System.Data.DataTable = oBookQR.Data
                            If Not oBookDT Is Nothing AndAlso oBookDT.Rows.Count > 0 Then
                                For Each oBookRow As System.Data.DataRow In oBookDT.Rows
                                    If Not addEmailAlert(oQuery, oBookRow("BookCustCompControl"), NGL.Core.Utility.DataTransformation.CleanNullableString(oBookRow("CompNumber")), enmAlertTypes.AlertCarrierContractExpired, intCarrierControl, strCarrierNumber) Then
                                        Log(Source & " Cannot Process " & Me.getEmailAlertString(enmAlertTypes.AlertCarrierContractExpired) & " Email Alerts for Company Control " & oBookRow("BookCustCompControl"))
                                    End If
                                    
                                Next
                                oBookDT = Nothing
                            End If
                        Else
                            Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertCarrierContractExpired) & " Emails:" & oQuery.LastError)
                        End If
                        oBookQR = Nothing
                    Next
                    oDT = Nothing
                End If
            Else
                Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertCarrierContractExpired) & " Emails:" & oQuery.LastError)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get Carrier Contract Expired Alerts Failure", "There was an unexpected error while reading the Carrier Contract Expired Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks></remarks>
    Private Sub getCarrierInsuranceExpiredAlerts(ByRef oQuery As NGL.Core.Data.Query)
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            'Get the Carrier Contract Expired Alerts
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select CarrierControl,CarrierNumber From  dbo.udfAlertsCarrierInsuranceExpired(NULL) Order By CarrierControl")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    Dim intCarrierControl As Integer = 0
                    Dim strCarrierNumber As String = ""
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        intCarrierControl = oRow("CarrierControl")
                        strCarrierNumber = NGL.Core.Utility.DataTransformation.CleanNullableString(oRow("CarrierNumber"))
                        Dim oBookQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select distinct BookCustCompControl, CompNumber From dbo.Book inner join dbo.Comp on BookCustCompControl = CompControl Where BookCarrierControl = " & intCarrierControl & "  AND BookDateDelivered is null Order By BookCustCompControl ")
                        If oBookQR.Exception Is Nothing Then
                            Dim oBookDT As System.Data.DataTable = oBookQR.Data
                            If Not oBookDT Is Nothing AndAlso oBookDT.Rows.Count > 0 Then
                                For Each oBookRow As System.Data.DataRow In oBookDT.Rows
                                    If Not addEmailAlert(oQuery, oBookRow("BookCustCompControl"), NGL.Core.Utility.DataTransformation.CleanNullableString(oBookRow("CompNumber")), enmAlertTypes.AlertCarrierInsuranceExpired, intCarrierControl, strCarrierNumber) Then
                                        Log(Source & " Cannot Process " & Me.getEmailAlertString(enmAlertTypes.AlertCarrierInsuranceExpired) & " Email Alerts for Company Control " & oBookRow("BookCustCompControl"))
                                    End If
                                Next
                                oBookDT = Nothing
                            End If
                        Else
                            Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertCarrierInsuranceExpired) & " Emails:" & oQuery.LastError)
                        End If
                        oBookQR = Nothing
                    Next
                    oDT = Nothing
                End If
            Else
                Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertCarrierInsuranceExpired) & " Emails:" & oQuery.LastError)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get Carrier Insurance Expired Alerts Failure", "There was an unexpected error while reading the Carrier Insurance Expired Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks></remarks>
    Private Sub getCarrierInsuranceExposurePerShipmentAlerts(ByRef oQuery As NGL.Core.Data.Query)
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            'Get the Carrier Contract Expired Alerts
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select CarrierControl,CarrierNumber From  dbo.udfAlertsCarrierInsuranceExposurePerShipment(NULL) Order By CarrierControl")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    Dim intCarrierControl As Integer = 0
                    Dim strCarrierNumber As String = ""
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        intCarrierControl = oRow("CarrierControl")
                        strCarrierNumber = NGL.Core.Utility.DataTransformation.CleanNullableString(oRow("CarrierNumber"))
                        Dim oBookQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select distinct BookCustCompControl, CompNumber From dbo.Book inner join dbo.Comp on BookCustCompControl = CompControl Where BookCarrierControl = " & intCarrierControl & "  AND BookDateDelivered is null Order By BookCustCompControl ")
                        If oBookQR.Exception Is Nothing Then
                            Dim oBookDT As System.Data.DataTable = oBookQR.Data
                            If Not oBookDT Is Nothing AndAlso oBookDT.Rows.Count > 0 Then
                                For Each oBookRow As System.Data.DataRow In oBookDT.Rows
                                    If Not addEmailAlert(oQuery, oBookRow("BookCustCompControl"), NGL.Core.Utility.DataTransformation.CleanNullableString(oBookRow("CompNumber")), enmAlertTypes.AlertCarrierExposurePerShipment, intCarrierControl, strCarrierNumber) Then
                                        Log(Source & " Cannot Process " & Me.getEmailAlertString(enmAlertTypes.AlertCarrierExposurePerShipment) & " Email Alerts for Company Control " & oBookRow("BookCustCompControl"))
                                    End If
                                Next
                                oBookDT = Nothing
                            End If
                        Else
                            Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertCarrierExposurePerShipment) & " Emails:" & oQuery.LastError)
                        End If
                        oBookQR = Nothing
                    Next
                    oDT = Nothing
                End If
            Else
                Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertCarrierExposurePerShipment) & " Emails:" & oQuery.LastError)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get Carrier Insurance Exposure Per Shipment Alerts Failure", "There was an unexpected error while reading the Carrier Insurance Exposure Per Shipment Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuery"></param>
    ''' <remarks></remarks>
    Private Sub getCarrierInsuranceExposureAllShipmentsAlerts(ByRef oQuery As NGL.Core.Data.Query)
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return
        Try
            'update exposure
            oQuery.Execute("Exec spUpdateCarrierQualCurAllExposure NULL,NULL,NULL")
        Catch ex As Exception

        End Try
        Try
            'Get the Carrier Contract Expired Alerts
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select CarrierControl,CarrierNumber From  dbo.udfAlertsCarrierInsuranceExposureAllShipments(NULL) Order By CarrierControl")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    Dim intCarrierControl As Integer = 0
                    Dim strCarrierNumber As String = ""
                    For Each oRow As System.Data.DataRow In oDT.Rows
                        intCarrierControl = oRow("CarrierControl")
                        strCarrierNumber = NGL.Core.Utility.DataTransformation.CleanNullableString(oRow("CarrierNumber"))
                        Dim oBookQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("Select distinct BookCustCompControl, CompNumber From dbo.Book inner join dbo.Comp on BookCustCompControl = CompControl Where BookCarrierControl = " & intCarrierControl & "  AND BookDateDelivered is null Order By BookCustCompControl ")
                        If oBookQR.Exception Is Nothing Then
                            Dim oBookDT As System.Data.DataTable = oBookQR.Data
                            If Not oBookDT Is Nothing AndAlso oBookDT.Rows.Count > 0 Then
                                For Each oBookRow As System.Data.DataRow In oBookDT.Rows
                                    If Not addEmailAlert(oQuery, oBookRow("BookCustCompControl"), NGL.Core.Utility.DataTransformation.CleanNullableString(oBookRow("CompNumber")), enmAlertTypes.AlertCarrierExposureAll, intCarrierControl, strCarrierNumber) Then
                                        Log(Source & " Cannot Process " & Me.getEmailAlertString(enmAlertTypes.AlertCarrierExposureAll) & " Email Alerts for Company Control " & oBookRow("BookCustCompControl"))
                                    End If
                                Next
                                oBookDT = Nothing
                            End If
                        Else
                            Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertCarrierExposureAll) & " Emails:" & oQuery.LastError)
                        End If
                        oBookQR = Nothing
                    Next
                    oDT = Nothing
                End If
            Else
                Log(Source & " Cannot Process " & Me.getEmailAlertMessage(enmAlertTypes.AlertCarrierExposureAll) & " Emails:" & oQuery.LastError)
            End If
            oQR = Nothing
        Catch ex As Exception
            LogError("Get Carrier Insurance Exposure All Shipments Alerts Failure", "There was an unexpected error while reading the Carrier Insurance Exposure All Shipments Alerts: " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Private Function getCompContEmails(ByRef oQuery As NGL.Core.Data.Query, ByVal CompControl As Integer) As String
        Dim strRet As String = Me.AdminEmail
        Dim oDT As System.Data.DataTable
        If oQuery Is Nothing Then Return strRet
        Try
            'Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("select dbo.udfGetCompContTenderEmails(" & CarrierControl & " ) as Emails")
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill("select dbo.udfGetCompContNotifyEmails(" & CompControl & " ) as Emails")
            If oQR.Exception Is Nothing Then
                oDT = oQR.Data
                If Not oDT Is Nothing AndAlso oDT.Rows.Count > 0 Then
                    strRet = NGL.Core.Utility.DataTransformation.CleanNullableString(oDT.Rows(0).Item("Emails")) '& "; rramsey@nextgeneration.com"
                Else
                    Log(String.Format("{0} Cannot get the company contact emails using configured admin email by default {1}.  The actual error is: {2}", Source, strRet, oQuery.LastError))
                End If
                oQR = Nothing

            End If
        Catch ex As Exception
            LogError("Get company contact emails Failure", "There was an unexpected error while reading the company contact emails (using configured admin email by default" & strRet & "): " & readExceptionMessage(ex) & vbCrLf & vbCrLf & "Source: " & Source, Me.AdminEmail)
        Finally
            Try
                oDT = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return strRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use optional parameters
    ''' </remarks>
    Private Sub ProcessEmails()

        Dim oParameter As New NGL.FreightMaster.Core.Model.Parameter(Me.oConfig)
        If Not oParameter.testConnection() Then
            Log(Source & " Database Connection Failure: " & oParameter.LastError)
            Throw New ApplicationException(Source & " Read Parameter Data Failure")
        End If

        If Not oEmailAlertData Is Nothing AndAlso oEmailAlertData.Rows.Count > 0 Then

            'lets begin by creating the WCF property.
            Dim props As Data.WCFParameters = CreateaDataProperties()
            props.ConnectionString = Me.ConnectionString
            props.Database = oConfig.Database
            props.DBServer = oConfig.DBServer
            props.ValidateAccess = False

            Dim DV As DataView = oEmailAlertData.DefaultView
            DV.Sort = "CompanyNumber"
            Dim strMsg As String = ""
            Dim strSpacer As String = ""
            Dim strCompNumber As String = ""
            Dim strMessageWord As String = "message was"
            Dim oEmail As New Ngl.Core.Communication.Email
            Dim intCt As Integer = 0
            Dim strCurrentEmail As String = ""
            Dim strCurrentCCEmail As String = ""
            Dim intCompControl As Integer = 0
            Dim dtLastRanOn As DateTime
            Dim strLastDate As String = ""
            Dim dblEmailAlertMinutes As Double = 0
            Dim dblEmailAlertStart As Double = 0
            Dim dblEmailAlertEnd As Double = 24
            Dim dblEmailOnSaturday As Double = 0
            Dim dblEmailOnSunday As Double = 0
            Dim objIniFile As New Ngl.Core.Communication.IniFile(APPPath() & "\FreightMaster.ini")
            Dim dtDefault As Date = Now
            Dim intCarrierControl As Integer = 0
            Dim strCarrierNumber As String = ""
            Dim strAlertType As String = ""

            Console.WriteLine()
            For Each oRow As DataRowView In DV
                intCt += 1
                Console.WriteLine("Processing Email Alerts: {0}", intCt)
                If String.IsNullOrEmpty(strCompNumber) Then
                    strCompNumber = oRow.Item("CompanyNumber")
                    intCompControl = oRow.Item("CompanyControl")
                    strCarrierNumber = oRow.Item("CarrierNumber")
                    intCarrierControl = IIf(oRow.Item("CarrierControl") Is DBNull.Value, 0, oRow.Item("CarrierControl"))
                     
                    strAlertType = oRow.Item("AlertType").ToString()
                    strCurrentEmail = oRow.Item("Email")
                    strCurrentCCEmail = oRow.Item("CCEmail")
                    dblEmailAlertMinutes = oParameter.getParValue("EmailAlertMinutes", intCompControl)
                    dblEmailAlertStart = oParameter.getParValue("EmailAlertStartOfBusinessDay", intCompControl)
                    dblEmailAlertEnd = oParameter.getParValue("EmailAlertEndOfBusinessDay", intCompControl)
                    dblEmailOnSaturday = oParameter.getParValue("EmailAlertOnSaturday", intCompControl)
                    dblEmailOnSunday = oParameter.getParValue("EmailAlertOnSunday", intCompControl)
                    dtDefault = Now.AddMinutes(((dblEmailAlertMinutes + 1) * -1))
                    strLastDate = objIniFile.GetString(INIKey, strCompNumber & "-LastRanOn", dtDefault.ToString("g"))
                    If Not Date.TryParse(strLastDate, dtLastRanOn) Then
                        dtLastRanOn = dtDefault
                    End If
                    strSpacer = ""
                    strMsg = ""
                ElseIf strCompNumber <> oRow.Item("CompanyNumber") Then
                    If Not String.IsNullOrEmpty(strMsg) Then
                        strMsg &= strSpacer & "Please open the System Alerts screen in FreightMaster for more information." & vbCrLf & vbCrLf & Me.DBInfo
                        If Me.Debug Then
                            'add alert messages for desktop notifications
                            Dim oSQL As New NGL.FreightMaster.Data.NGLBatchProcessDataProvider(props)
                            Dim lAlertsSuccess As Boolean = oSQL.executeInsertAlertMessageNoEmail(strAlertType, intCompControl, strCompNumber, intCarrierControl, strCarrierNumber, "FreightMaster Alert " & strAlertType, strMsg, "", "", "", "", "", True)
                            If lAlertsSuccess = False Then
                                Log(Source & " Send InsertAlertMessageNoEmail Failure: ")
                            End If
                            'Modified By LVV 2/18/16 v-7.0.5.0
                            If Not oEmail.SendMail(Me.SMTPServer, Me.AdminEmail, Me.FromEmail, strMsg, "FreightMaster Alert Message", "", SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                                Log(Source & " Send Email Failure: " & oEmail.LastError)
                            Else
                                Try
                                    Log(Source & " Email Sent to " & Me.AdminEmail)
                                    objIniFile.WriteString(INIKey, strCompNumber & "-LastRanOn", Now.ToString("g"))
                                Catch ex As Exception

                                End Try
                            End If
                        Else
                            'Modified By LVV 2/18/16 v-7.0.5.0
                            If Not oEmail.SendMail(Me.SMTPServer, strCurrentEmail, Me.FromEmail, strMsg, "FreightMaster Alert Message", strCurrentCCEmail, SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                                Log(Source & " Send Email Failure: " & oEmail.LastError)
                            Else
                                Try
                                    Log(Source & " Email Sent to " & strCurrentEmail & " CC " & strCurrentCCEmail)
                                    objIniFile.WriteString(INIKey, strCompNumber & "-LastRanOn", Now.ToString("g"))
                                Catch ex As Exception

                                End Try
                            End If
                        End If

                    End If

                    strCompNumber = oRow.Item("CompanyNumber")
                    intCompControl = oRow.Item("CompanyControl")
                    strCarrierNumber = oRow.Item("CarrierNumber")
                    intCarrierControl = IIf(oRow.Item("CarrierControl") Is DBNull.Value, 0, oRow.Item("CarrierControl"))
                    strAlertType = oRow.Item("AlertType").ToString()
                    strCurrentEmail = oRow.Item("Email")
                    strCurrentCCEmail = oRow.Item("CCEmail")
                    dblEmailAlertMinutes = oParameter.getParValue("EmailAlertMinutes", intCompControl)
                    dblEmailAlertStart = oParameter.getParValue("EmailAlertStartOfBusinessDay", intCompControl)
                    dblEmailAlertEnd = oParameter.getParValue("EmailAlertEndOfBusinessDay", intCompControl)
                    dblEmailOnSaturday = oParameter.getParValue("EmailAlertOnSaturday", intCompControl)
                    dblEmailOnSunday = oParameter.getParValue("EmailAlertOnSunday", intCompControl)
                    dtDefault = Now.AddMinutes(((dblEmailAlertMinutes + 1) * -1))
                    strLastDate = objIniFile.GetString(INIKey, strCompNumber & "-LastRanOn", dtDefault.ToString("g"))
                    If Not Date.TryParse(strLastDate, dtLastRanOn) Then
                        dtLastRanOn = dtDefault
                    End If
                    strSpacer = ""
                    strMsg = ""
                End If
                Dim blnReadAlert As Boolean = True
                Dim dtNow As Date = Now
                Dim strFalseMsg As String = ""
                If dtNow.DayOfWeek = DayOfWeek.Saturday AndAlso dblEmailOnSaturday = 0 Then
                    strFalseMsg &= "No Emails on Saturday! "
                    blnReadAlert = False
                End If
                If blnReadAlert AndAlso dtNow.DayOfWeek = DayOfWeek.Sunday AndAlso dblEmailOnSunday = 0 Then
                    strFalseMsg &= "No Emails on Sunday! "
                    blnReadAlert = False

                End If
                If blnReadAlert AndAlso dtNow.Hour < dblEmailAlertStart Then
                    strFalseMsg &= "No Emails Before " & dblEmailAlertStart.ToString & " Hour! "
                    blnReadAlert = False
                End If
                If blnReadAlert AndAlso dtNow.Hour > dblEmailAlertEnd Then
                    strFalseMsg &= "No Emails After " & dblEmailAlertEnd.ToString & " Hour! "
                    blnReadAlert = False
                End If
                If blnReadAlert AndAlso dtNow.AddMinutes(dblEmailAlertMinutes * -1) < dtLastRanOn Then
                    strFalseMsg &= "Waiting on next Email Cycle, Last Ran On " & dtLastRanOn.ToString("g") & "! "
                    blnReadAlert = False
                End If
                If blnReadAlert Then
                    If oRow.Item("AlertCount") > 1 Then
                        strMessageWord = "messages were"
                    Else
                        strMessageWord = "message was"
                    End If

                    If Me.Debug Then
                        strMsg &= strSpacer & String.Format("{3} {2} {4} found for Company Number: {1}.  Messages will be sent to Email: {5} CCEmail: {6}", oRow.Item("CompanyControl"), oRow.Item("CompanyNumber"), getEmailAlertMessage(oRow.Item("AlertType"), oRow.Item("CompanyControl")), oRow.Item("AlertCount"), strMessageWord, strCurrentEmail, strCurrentCCEmail)
                    Else
                        strMsg &= strSpacer & String.Format("{3} {2} {4} found for Company Number: {1}.", oRow.Item("CompanyControl"), oRow.Item("CompanyNumber"), getEmailAlertMessage(oRow.Item("AlertType"), oRow.Item("CompanyControl")), oRow.Item("AlertCount"), strMessageWord, oRow.Item("Email"), oRow.Item("CCEmail"))
                    End If

                    strSpacer = vbCrLf & vbCrLf
                Else
                    Log(Source & " Read Alert False: " & strFalseMsg)
                End If

            Next
            Console.WriteLine()

        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strSubject"></param>
    ''' <param name="logMessage"></param>
    ''' <param name="strMailTo"></param>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use optional parameters
    ''' </remarks>
    Public Overrides Sub LogError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String)
        Dim oEmail As New NGL.Core.Communication.Email
        Me.LastError = logMessage
        Try
            'Modified By LVV 2/18/16 v-7.0.5.0
            If Not oEmail.SendMail(SMTPServer, strMailTo, FromEmail, logMessage & vbCrLf & vbCrLf & Me.DBInfo, strSubject, "", SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort) Then
                Log("Send Email Error:  Could not send message to " & strMailTo)
            End If

        Catch ex As Exception
            'do nothing 
        Finally
            'oEmail = Nothing

        End Try
        Dim strLogMsg As String = "Email Notice Sent to " & strMailTo & vbCrLf & " Subject: " & strSubject & vbCrLf & "Msg: " & logMessage
        Log(strLogMsg)

    End Sub
End Class
