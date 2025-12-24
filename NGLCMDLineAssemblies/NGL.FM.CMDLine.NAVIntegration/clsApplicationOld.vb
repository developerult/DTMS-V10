Imports System.IO
Imports System.ServiceModel
Imports System.Reflection

Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports DTran = NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NData = NGL.FreightMaster.Data
Imports Ngl.Core
Imports TMS = Ngl.FreightMaster.Integration

Public Class clsApplicationOld : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    Public Enum MstrDataType
        Comp = 0
        Cust = 1
        Carrier = 2
    End Enum


    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Application Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()

        Try
            Log("Begin Process Data ")
            processCompanyData()
            processCarrierData()
            processLaneData()
            processOrderData()
            processPicklistData()
            processAPExportData()
            'processPayablesData()
            Log("Process Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        Finally
            Me.closeLog(0)
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

    Private Function processCompanyData() As Boolean
        Dim blnRet As Boolean = False
        Try
            Log("Begin Process Company Data ")
            Dim strMsg As String = ""
            Dim oCompIntegration As New TMS.clsCompany
            populateIntegrationObjectParameters(oCompIntegration)
            Dim oCompHeaders As New List(Of TMS.clsCompanyHeaderObject70)
            Dim oCompConts As New List(Of TMS.clsCompanyContactObject70)
            Dim oCompCalendars As New List(Of TMS.clsCompanyCalendarObject70)
            Dim oNavCompany = New NAVService.Company
            Dim oNavCompanies As New NAVService.DynamicsTMSCompanies
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()

            oNAVWebService.UseDefaultCredentials = True
            oNAVWebService.GetCompanies(oNavCompanies, True, True)
            For Each c In oNavCompanies.Company
                If Not c Is Nothing AndAlso Not String.IsNullOrWhiteSpace(c.CompAlphaCode) Then
                    Dim strSkip As New List(Of String)
                    Dim oHeader = New TMS.clsCompanyHeaderObject70
                    CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                        Log(strMsg)
                        strMsg = ""
                    End If
                    oCompHeaders.Add(oHeader)
                    'TODO: add code to read contact information
                End If
            Next
            If Not oCompHeaders Is Nothing AndAlso oCompHeaders.Count > 0 Then
                'save changes to database 
                Dim oResults As TMS.clsIntegrationUpdateResults = oCompIntegration.ProcessObjectData70(oCompHeaders, oCompConts, Me.ConnectionString, oCompCalendars)
                Dim sLastError As String = oCompIntegration.LastError
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        Log("Error Data Connection Failure! could not import company information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        Log("Error Integration Failure! could not import company information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        Log("Warning Integration Had Errors! could not import some company information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        Log("Error Data Validation Failure! could not import company information:  " & sLastError)
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        Log("Success! the following company control numbers were processed: " & strNumbers)
                        'TODO: add code to send confirmation back to NAV that the company data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Companies to Process")
            End If
            Log("Process Company Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        End Try

        Return blnRet
    End Function

    Private Function processLaneData() As Boolean
        Dim blnRet As Boolean = False
        Try

            Log("Begin Process Lane Data ")
            Dim strMsg As String = ""
            Dim oLaneIntegration As New TMS.clsLane
            populateIntegrationObjectParameters(oLaneIntegration)
            Dim oLaneHeaders As New List(Of TMS.clsLaneObject70)
            Dim oLaneCalendars As New List(Of TMS.clsLaneCalendarObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.UseDefaultCredentials = True
            Dim oNavLanes = New NAVService.DynamicsTMSLanes()
            oNAVWebService.GetLanes(oNavLanes, True, True)
            Dim strSkip As New List(Of String)
            For Each c In oNavLanes.Lane
                If Not c Is Nothing AndAlso Not String.IsNullOrWhiteSpace(c.LaneNumber) Then
                    Dim oHeader = New TMS.clsLaneObject70
                    CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                        Log(strMsg)
                        strMsg = ""
                    End If
                    'If String.IsNullOrWhiteSpace(oHeader.LaneCompAlphaCode) Then
                    '    'Testing Code for missing data in lane file
                    '    Select Case c.LaneNumber
                    '        Case "BLU-10000-DUDLEY"
                    '            oHeader.LaneCompAlphaCode = "BLUE"
                    '        Case "YLW-RED"
                    '            oHeader.LaneCompAlphaCode = "YELLOW"
                    '    End Select
                    'End If
                    'If String.IsNullOrWhiteSpace(oHeader.LaneLegalEntity) Then oHeader.LaneLegalEntity = LegalEntity 'use command line parameter
                    If String.IsNullOrWhiteSpace(oHeader.LaneName) Then oHeader.LaneName = oHeader.LaneNumber
                    If Not String.IsNullOrWhiteSpace(oHeader.LaneOrigCompAlphaCode) AndAlso String.IsNullOrWhiteSpace(oHeader.LaneOrigLegalEntity) Then
                        oHeader.LaneOrigLegalEntity = LegalEntity
                    End If
                    If Not String.IsNullOrWhiteSpace(oHeader.LaneDestCompAlphaCode) AndAlso String.IsNullOrWhiteSpace(oHeader.LaneDestLegalEntity) Then
                        oHeader.LaneDestLegalEntity = LegalEntity
                    End If
                    oLaneHeaders.Add(oHeader)
                    'TODO: Add code to copy NAV data into oLaneCalendars objects
                End If
            Next
            If Not oLaneHeaders Is Nothing AndAlso oLaneHeaders.Count > 0 Then
                'save changes to database 
                Dim oResults As TMS.clsIntegrationUpdateResults = oLaneIntegration.ProcessObjectData70(oLaneHeaders, Me.ConnectionString, oLaneCalendars)
                Dim sLastError As String = oLaneIntegration.LastError
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        Log("Error Data Connection Failure! could not import Lane information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        Log("Error Integration Failure! could not import Lane information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        Log("Warning Integration Had Errors! could not import some Lane information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        Log("Error Data Validation Failure! could not import Lane information:  " & sLastError)
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        Log("Success! the following Lane control numbers were processed: " & strNumbers)
                        'TODO: add code to send confirmation back to NAV that the lane data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Lanes to Process")
            End If
            Log("Process Lane Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        End Try

        Return blnRet
    End Function

    Private Function processCarrierData() As Boolean
        Dim blnRet As Boolean = False
        Try
            Log("Begin Process Carrier Data ")
            Dim strMsg As String = ""
            Dim oCarrierIntegration As New TMS.clsCarrier
            populateIntegrationObjectParameters(oCarrierIntegration)
            Dim oCarrierHeaders As New List(Of TMS.clsCarrierHeaderObject70)
            Dim oCarrierConts As New List(Of TMS.clsCarrierContactObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.UseDefaultCredentials = True
            Dim oNavCarriers = New NAVService.DynamicsTMSCarriers()
            oNAVWebService.GetCarriers(oNavCarriers, True, True)
            Dim strSkip As New List(Of String)
            For Each c In oNavCarriers.Carrier
                If Not c Is Nothing AndAlso Not String.IsNullOrWhiteSpace(c.CarrierAlphaCode) Then
                    Dim oHeader = New TMS.clsCarrierHeaderObject70
                    CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                        Log(strMsg)
                        strMsg = ""
                    End If
                    oCarrierHeaders.Add(oHeader)
                    'TODO: Add code to copy NAV data into Contact objects
                End If
            Next
            'save changes to database 
            If Not oCarrierHeaders Is Nothing AndAlso oCarrierHeaders.Count > 0 Then
                Dim oResults As TMS.clsIntegrationUpdateResults = oCarrierIntegration.ProcessObjectData70(oCarrierHeaders, oCarrierConts, Me.ConnectionString)
                Dim sLastError As String = oCarrierIntegration.LastError
                Select Case oResults.ReturnValue
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        Log("Error Data Connection Failure! could not import Carrier information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        Log("Error Integration Failure! could not import Carrier information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        Log("Warning Integration Had Errors! could not import some Carrier information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        Log("Error Data Validation Failure! could not import Carrier information:  " & sLastError)
                    Case Else
                        'success
                        Dim strNumbers = String.Join("; ", oResults.ControlNumbers.Select(Function(x) x.ToString()).ToArray())
                        Log("Success! the following Carrier control numbers were processed: " & strNumbers)
                        'TODO: add code to send confirmation back to NAV that the carrier data was processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Carriers to Process")
            End If

            Log("Process Carrier Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        End Try

        Return blnRet
    End Function

    Private Function processOrderData() As Boolean
        Dim blnRet As Boolean = False
        Try

            Log("Begin Process Order Data ")
            Dim strMsg As String = ""
            Dim oBookIntegration As New TMS.clsBook
            populateIntegrationObjectParameters(oBookIntegration)
            Dim oBookHeaders As New List(Of TMS.clsBookHeaderObject70)
            Dim oBookDetails As New List(Of TMS.clsBookDetailObject70)
            Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            oNAVWebService.UseDefaultCredentials = True
            Dim oNavOrders = New NAVService.DynamicsTMSBookings()
            oNAVWebService.GetBookings(oNavOrders, True, False)
            Dim strSkip As New List(Of String)
            Dim strItemSkip As New List(Of String)
            For Each c In oNavOrders.Booking
                If Not c Is Nothing AndAlso Not String.IsNullOrWhiteSpace(c.PONumber) Then
                    Dim CompAlphaCode As String
                    'debug code skip any deletes for now we want to import all the orders
                    'If c.POStatusFlag < 2 Then
                    'ok to process

                    'debug code for missing compalpha code

                    'Select Case c.POVendor
                    '    Case "BLU-10000-DUDLEY"
                    '        CompAlphaCode = "BLUE"
                    'End Select
                    'Dim OrderNumber As String = c.PONumber
                    Dim oHeader = New TMS.clsBookHeaderObject70
                    CopyMatchingFieldsImplicitCast(oHeader, c, strSkip, strMsg)
                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                        Log(strMsg)
                        strMsg = ""
                    End If
                    'debug code to correct bad company number
                    'oHeader.PODefaultCustomer = ""
                    'If String.IsNullOrWhiteSpace(oHeader.POCompLegalEntity) Then
                    '    oHeader.POCompLegalEntity = LegalEntity 'use command line parameter
                    'End If
                    'If String.IsNullOrWhiteSpace(oHeader.POCompAlphaCode) Then
                    '    oHeader.POCompAlphaCode = CompAlphaCode 'use debug code based on lane
                    'End If
                    If oHeader.POModeTypeControl = 0 Then
                        oHeader.POModeTypeControl = 3 'use default as Road
                    End If

                    oBookHeaders.Add(oHeader)
                    For Each item In c.Items
                        If Not item Is Nothing AndAlso Not String.IsNullOrWhiteSpace(item.ItemNumber) Then
                            Dim oItem As New TMS.clsBookDetailObject70
                            CopyMatchingFieldsImplicitCast(oItem, item, strItemSkip, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                Log(strMsg)
                                strMsg = ""
                            End If

                            ''Debug testing code to correct invalid customer number
                            'oItem.CustomerNumber = 0
                            'If String.IsNullOrWhiteSpace(oItem.ItemPONumber) Then oItem.ItemPONumber = OrderNumber
                            'If String.IsNullOrWhiteSpace(oItem.POItemCompAlphaCode) Then oItem.POItemCompAlphaCode = CompAlphaCode
                            'If String.IsNullOrWhiteSpace(oItem.POItemCompLegalEntity) Then oItem.POItemCompLegalEntity = LegalEntity

                            oBookDetails.Add(oItem)
                        End If
                    Next

                    'End If
                End If
            Next
            If Not oBookHeaders Is Nothing AndAlso oBookHeaders.Count > 0 Then
                'force the software to wait for the silent tender process to finish before exiting
                'typically used for unattended execution
                oBookIntegration.RunSilentTenderAsync = False
                'save changes to database 
                Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessObjectData(oBookHeaders, oBookDetails, Me.ConnectionString)
                Dim sLastError As String = oBookIntegration.LastError
                Select Case oResults
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        Log("Error Data Connection Failure! could not import Order information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        Log("Error Integration Failure! could not import Order information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        Log("Warning Integration Had Errors! could not import some Order information:  " & sLastError)
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        Log("Error Data Validation Failure! could not import Order information:  " & sLastError)
                    Case Else
                        'success
                        Dim strNumbers As String = "" 'To do read order numbers from NAV data files
                        Log("Success! the following Order Numbers were processed: " & strNumbers)
                        'TODO: add code to send confirmation back to NAV that the orders were processed
                        'mark process and success
                        blnRet = True
                End Select
            Else
                Log("No Orders to Process")
            End If
            Log("Process Order Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        End Try

        Return blnRet
    End Function

    Private Function processPicklistData() As Boolean
        Dim blnRet As Boolean = False
        Try

            Log("Begin Process Picklist Data ")
            'Dim pl As New clsPickListData70
            Dim picklist As New TMS.clsPickList
            populateIntegrationObjectParameters(picklist)
            Dim Headers() As TMS.clsPickListObject70
            Dim Details() As TMS.clsPickDetailObject70
            Dim Fees() As TMS.clsPickListFeeObject70
            Dim strMsg As String = ""
            'set the default value to false
            Dim RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim intMaxRetry As Integer = My.Settings.PicklistMaxRetry
            Dim intRetryMinutes As Integer = My.Settings.PicklistRetryMinutes
            picklist.MaxRowsReturned = My.Settings.PicklistMaxRowsReturned
            picklist.AutoConfirmation = My.Settings.PicklistAutoConfirmation
            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", intMaxRetry, intRetryMinutes, picklist.MaxRowsReturned, picklist.AutoConfirmation)

            'If Not String.IsNullOrEmpty(CompLegalEntity) Then strCriteria &= " CompLegalEntity = " & CompLegalEntity
            'Utilities.populateIntegrationObjectParameters(picklist)
            'picklist.MaxRowsReturned = MaxRowsReturned
            'picklist.AutoConfirmation = AutoConfirmation

            RetVal = picklist.readObjectData70(Headers, Me.ConnectionString, intMaxRetry, intRetryMinutes, LegalEntity, Fees, Details)
            LastError = picklist.LastError
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        Log("Error Data Connection Failure! could not export Picklist information:  " & LastError)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("Picklist Integration Error", "Error Integration Failure! could not export Picklist information:  " & LastError, AdminEmail)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        LogError("Picklist Integration Error", "Error Integration Had Errors! could not export some Picklist information:  " & LastError, AdminEmail)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        LogError("Picklist Integration Error", "Error Data Validation Failure! could not export Picklist information:  " & LastError, AdminEmail)
                        Return False
                End Select
            End If
            If Not Headers Is Nothing AndAlso Headers.Count > 0 Then
                Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
                oNAVWebService.UseDefaultCredentials = True
                Dim oNavPicklist = New NAVService.DynamicsTMSPicks

                Dim strSkip As New List(Of String)
                Dim strSkipLine As New List(Of String)
                Dim oPicks As New List(Of NAVService.Pick)
                For Each c In Headers
                    If Not c Is Nothing Then
                        Dim oPick = New NAVService.Pick()
                        'CopyMatchingFields(oPick, c, strSkip)
                        CopyMatchingFieldsImplicitCast(oPick, c, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            Log(strMsg)
                            strMsg = ""
                        End If
                        Dim oLines As New List(Of NAVService.Lines)
                        For Each i In Details.Where(Function(x) x.PLControl = c.PLControl)
                            Dim oLine As New NAVService.Lines()
                            CopyMatchingFieldsImplicitCast(oLine, i, strSkipLine, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                Log(strMsg)
                                strMsg = ""
                            End If
                            oLines.Add(oLine)
                        Next
                        If Not oLines Is Nothing AndAlso oLines.Count > 0 Then
                            oPick.Lines = oLines.ToArray()
                        End If
                        oPicks.Add(oPick)

                    End If
                Next
                If Not oPicks Is Nothing AndAlso oPicks.Count > 0 Then
                    oNavPicklist.Pick = oPicks.ToArray()
                    oNAVWebService.SendPicks(oNavPicklist)
                Else
                    Log("No Data Found for Picklist Picks Array")
                End If

            Else
                Log("No Pick List Status Updates to Process")
            End If
            Log("Process Picklist Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        End Try

        Return blnRet
    End Function

    Private Function processAPExportData() As Boolean
        Dim blnRet As Boolean = False
        Try

            Log("Begin Process APExport Data ")
            Dim strMsg As String = ""
            'Dim pl As New clsPickListData70
            Dim apExport As New TMS.clsAPExport
            Dim Headers() As TMS.clsAPExportObject70
            Dim Details() As TMS.clsAPExportDetailObject70
            Dim Fees() As TMS.clsAPExportFeeObject70

            'set the default value to false
            Dim RetVal = TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            Dim intMaxRetry As Integer = My.Settings.APExportMaxRetry
            Dim intRetryMinutes As Integer = My.Settings.APExportRetryMinutes
            apExport.MaxRowsReturned = My.Settings.APExportMaxRowsReturned
            apExport.AutoConfirmation = My.Settings.APExportAutoConfirmation
            Dim strCriteria As String = String.Format(" MaxRetry = {0} RetryMinutes = {1} MaxRowsReturned = {2} AutoConfirmation = {3}", intMaxRetry, intRetryMinutes, apExport.MaxRowsReturned, apExport.AutoConfirmation)

            'If Not String.IsNullOrEmpty(CompLegalEntity) Then strCriteria &= " CompLegalEntity = " & CompLegalEntity
            'Utilities.populateIntegrationObjectParameters(picklist)
            'picklist.MaxRowsReturned = MaxRowsReturned
            'picklist.AutoConfirmation = AutoConfirmation

            RetVal = apExport.readObjectData70(Headers, Me.ConnectionString, intMaxRetry, intRetryMinutes, LegalEntity, Fees, Details)
            LastError = apExport.LastError
            If RetVal <> FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete Then
                Select Case RetVal
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                        Log("Error Data Connection Failure! could not export APExport information:  " & LastError)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                        LogError("APExport Integration Error", "Error Integration Failure! could not export APExport information:  " & LastError, AdminEmail)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                        LogError("APExport Integration Error", "Error Integration Had Errors! could not export some APExport information:  " & LastError, AdminEmail)
                        Return False
                    Case FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                        LogError("APExport Integration Error", "Error Data Validation Failure! could not export APExport information:  " & LastError, AdminEmail)
                        Return False
                End Select
            End If
            If Not Headers Is Nothing AndAlso Headers.Count > 0 Then
                Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
                oNAVWebService.UseDefaultCredentials = True
                Dim oNavAPs = New NAVService.DynamicsTMSAP

                Dim strSkip As New List(Of String)
                Dim strSkipLine As New List(Of String)
                Dim lAPs As New List(Of NAVService.AP)
                For Each c In Headers
                    If Not c Is Nothing Then
                        Dim oAP = New NAVService.AP()
                        'CopyMatchingFields(oPick, c, strSkip)
                        CopyMatchingFieldsImplicitCast(oAP, c, strSkip, strMsg)
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            Log(strMsg)
                            strMsg = ""
                        End If
                        Dim oDetails As New List(Of NAVService.Details)
                        For Each i In Details.Where(Function(x) x.APControl = c.APControl)
                            Dim oDetail As New NAVService.Details()
                            CopyMatchingFieldsImplicitCast(oDetail, i, strSkipLine, strMsg)
                            If Not String.IsNullOrWhiteSpace(strMsg) Then
                                Log(strMsg)
                                strMsg = ""
                            End If
                            oDetails.Add(oDetail)
                        Next
                        If Not oDetails Is Nothing AndAlso oDetails.Count > 0 Then
                            oAP.Details = oDetails.ToArray()
                        End If
                        lAPs.Add(oAP)

                    End If
                Next
                If Not lAPs Is Nothing AndAlso lAPs.Count > 0 Then
                    oNavAPs.AP = lAPs.ToArray()
                    oNAVWebService.SendAP(oNavAPs)
                Else
                    Log("APExport NAV Payment Array is Empty")
                End If
            Else
                Log("No AP Export Updates to Process")
            End If

            Log("Process APExport Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        End Try

        Return blnRet
    End Function

    Private Function processPayablesData() As Boolean
        Dim blnRet As Boolean = False
        Try

            Log("Begin Process Payables Data ")

            Log("Process Payables Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        End Try

        Return blnRet
    End Function


    Private Function CopyMatchingFields(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String)) As Object
        If toObj Is Nothing Or fromObj Is Nothing Then
            Return Nothing
        End If

        Dim fromType As Type = fromObj.[GetType]()
        Dim toType As Type = toObj.[GetType]()

        ' Get all FieldInfo. 
        Dim fProps As PropertyInfo() = fromType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        Dim tProps As PropertyInfo() = toType.GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
        For Each fProp As PropertyInfo In fProps
            Dim propValue As Object = fProp.GetValue(fromObj)
            'Removed by RHR 10/8/14 did not update nullable fields when null
            'If propValue IsNot Nothing Then
            If Not skipObjs.Contains(fProp.Name) Then
                For Each tProp In tProps
                    If tProp.Name = fProp.Name Then
                        If tProp.PropertyType() = fProp.PropertyType() Then
                            Try
                                tProp.SetValue(toObj, propValue)
                            Catch ex As Exception
                                Dim strMsg As String = ex.Message
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



    Private Function CopyMatchingFieldsImplicitCast(toObj As [Object], fromObj As [Object], ByVal skipObjs As List(Of String), ByRef strMsg As String) As Object
        If toObj Is Nothing Or fromObj Is Nothing Then
            Return Nothing
        End If
        'primatives used for casting
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
            If Not skipObjs.Contains(fProp.Name) Then
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
                            Try
                                Select Case tProp.PropertyType.Name
                                    Case "String"
                                        tProp.SetValue(toObj, propValue.ToString())
                                    Case "Int16"
                                        If Int16.TryParse(propValue.ToString(), iVal16) Then
                                            tProp.SetValue(toObj, iVal16)
                                        End If
                                    Case "Int32"
                                        If Int32.TryParse(propValue.ToString(), iVal32) Then
                                            tProp.SetValue(toObj, iVal32)
                                        End If
                                    Case "Int64"
                                        If Int32.TryParse(propValue.ToString(), iVal64) Then
                                            tProp.SetValue(toObj, iVal64)
                                        End If
                                    Case "Date"
                                        If Date.TryParse(propValue.ToString(), dtVal) Then
                                            tProp.SetValue(toObj, dtVal)
                                        End If
                                    Case "DateTime"
                                        If Date.TryParse(propValue.ToString(), dtVal) Then
                                            tProp.SetValue(toObj, dtVal)
                                        End If
                                    Case "Decimal"
                                        If Decimal.TryParse(propValue.ToString(), decVal) Then
                                            tProp.SetValue(toObj, decVal)
                                        End If
                                    Case "Double"
                                        If Double.TryParse(propValue.ToString(), dblVal) Then
                                            tProp.SetValue(toObj, dblVal)
                                        End If
                                    Case "Boolean"
                                        If Boolean.TryParse(propValue.ToString(), blnVal) Then
                                            tProp.SetValue(toObj, blnVal)
                                        Else
                                            'try to convert to an integer and then test for 0 any non zero is true
                                            If Integer.TryParse(propValue.ToString(), intVal) Then
                                                If intVal = 0 Then
                                                    blnVal = False
                                                Else
                                                    blnVal = True
                                                End If
                                                tProp.SetValue(toObj, blnVal)
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


    Private Sub populateIntegrationObjectParameters(ByRef oImportExport As TMS.clsImportExport)

        Dim connectionString As String = Me.ConnectionString
        With oImportExport
            .AdminEmail = Me.AdminEmail
            .FromEmail = Me.FromEmail
            .GroupEmail = Me.GroupEmail
            .Retry = Me.AutoRetry
            .SMTPServer = Me.SMTPServer
            .DBServer = Me.DBServer
            .Database = Me.Database
            .ConnectionString = connectionString
            .Debug = Me.Debug
            .AuthorizationCode = My.Settings.WSAuthCode
            .WCFAuthCode = My.Settings.WCFAuthCode
            .WCFURL = My.Settings.WCFURL
            .WCFTCPURL = My.Settings.WCFTCPURL
        End With

    End Sub



End Class
