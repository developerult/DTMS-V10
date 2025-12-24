Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

<Serializable()> _
Public Class clsSchedule : Inherits clsDownload


#Region "Constructors"

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Sub New(ByVal admin_email As String, _
            ByVal from_email As String, _
            ByVal group_email As String, _
            ByVal auto_retry As Integer, _
            ByVal smtp_server As String, _
            ByVal db_server As String, _
            ByVal database_catalog As String, _
            ByVal auth_code As String, _
            ByVal debug_mode As Boolean,
            Optional ByVal connection_string As String = "")

        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


    End Sub

#End Region


    Public Function getDataSet() As ScheduleData
        Return New ScheduleData
    End Function

    Public Function ProcessObjectData( _
                        ByVal oSchedules() As clsScheduleObject, _
                        ByVal strConnection As String) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim oHTable As New ScheduleData.ScheduleHeaderDataTable
        Dim dtVal As Date

        Try
            For Each oItem As clsScheduleObject In oSchedules
                Dim oRow As ScheduleData.ScheduleHeaderRow = oHTable.NewScheduleHeaderRow
                With oRow
                    .BookCarrOrderNumber = Left(oItem.BookCarrOrderNumber, 20)
                    .BookCarrDockPUAssigment = Left(oItem.BookCarrDockPUAssigment, 10)
                    If validateDateWS(oItem.BookCarrScheduleDate, dtVal) Then
                        .BookCarrScheduleDate = exportDateToString(dtVal.ToString)
                    End If
                    .BookCarrScheduleTime = Left(oItem.BookCarrScheduleTime, 20)
                    If validateDateWS(oItem.BookCarrActualDate, dtVal) Then
                        .BookCarrActualDate = exportDateToString(dtVal.ToString)
                    End If
                    .BookCarrActualTime = Left(oItem.BookCarrActualTime, 20)
                    .BookCarrScheduleTime = Left(oItem.BookCarrScheduleTime, 20)
                    If validateDateWS(oItem.BookCarrActLoadCompleteDate, dtVal) Then
                        .BookCarrActLoadCompleteDate = exportDateToString(dtVal.ToString)
                    End If
                    .BookCarrActLoadCompleteTime = Left(oItem.BookCarrActLoadCompleteTime, 20)
                    .CompNumber = Left(oItem.CompNumber, 50)
                    .BookProNumber = Left(oItem.BookPRONumber, 20)
                    .BookOrderSequence = oItem.BookOrderSequence

                    If validateDateWS(oItem.BookCarrStartLoadingDate, dtVal) Then
                        .BookCarrStartLoadingDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrStartLoadingTime) Then
                        .BookCarrStartLoadingTime = oItem.BookCarrStartLoadingTime
                    End If

                    If validateDateWS(oItem.BookCarrFinishLoadingDate, dtVal) Then
                        .BookCarrFinishLoadingDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrFinishLoadingTime) Then
                        .BookCarrFinishLoadingTime = oItem.BookCarrFinishLoadingTime
                    End If


                    .BookCarrDockDelAssignment = Left(oItem.BookCarrDockDelAssignment, 10)

                    If validateDateWS(oItem.BookCarrApptDate, dtVal) Then
                        .BookCarrApptDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrApptTime) Then
                        .BookCarrApptTime = oItem.BookCarrApptTime
                    End If

                    If validateDateWS(oItem.BookCarrActDate, dtVal) Then
                        .BookCarrActDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrActTime) Then
                        .BookCarrActTime = oItem.BookCarrActTime
                    End If

                    If validateDateWS(oItem.BookCarrStartUnloadingDate, dtVal) Then
                        .BookCarrStartUnloadingDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrStartUnloadingTime) Then
                        .BookCarrStartUnloadingTime = oItem.BookCarrStartUnloadingTime
                    End If

                    If validateDateWS(oItem.BookCarrFinishUnloadingDate, dtVal) Then
                        .BookCarrFinishUnloadingDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrFinishUnloadingTime) Then
                        .BookCarrFinishUnloadingTime = oItem.BookCarrFinishUnloadingTime
                    End If

                    If validateDateWS(oItem.BookCarrActUnloadCompDate, dtVal) Then
                        .BookCarrActUnloadCompDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrActUnloadCompTime) Then
                        .BookCarrActUnloadCompTime = oItem.BookCarrActUnloadCompTime
                    End If
                    .BookShipCarrierProNumber = Left(oItem.BookShipCarrierProNumber, 20)
                    .BookShipCarrierName = Left(oItem.BookShipCarrierName, 60)
                    .BookShipCarrierNumber = Left(oItem.BookShipCarrierNumber, 80)
                End With
                oHTable.AddScheduleHeaderRow(oRow)
            Next

            intRet = ProcessData(oHTable, strConnection)
        Catch ex As Exception
            LogException("Process Object Data Failure", "Order import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsSchedule.ProcessObjectData Failure")
        End Try
        Return intRet


    End Function

    Public Function ProcessObjectData70( _
                       ByVal oSchedules() As clsScheduleObject70, _
                       ByVal strConnection As String) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim dtVal As Date
        Dim schedList As New List(Of clsScheduleObject70)

        Try
            For Each oItem As clsScheduleObject70 In oSchedules
                'If Not oItem Is Nothing Then
                'Dim oRow As New clsScheduleObject70
                With oItem
                    '.BookCarrOrderNumber = Left(oItem.BookCarrOrderNumber, 20)
                    '.BookCarrDockPUAssigment = Left(oItem.BookCarrDockPUAssigment, 10)
                    If validateDateWS(oItem.BookCarrScheduleDate, dtVal) Then
                        .BookCarrScheduleDate = exportDateToString(dtVal.ToString)
                    End If
                    '.BookCarrScheduleTime = Left(oItem.BookCarrScheduleTime, 20)
                    If validateDateWS(oItem.BookCarrActualDate, dtVal) Then
                        .BookCarrActualDate = exportDateToString(dtVal.ToString)
                    End If
                    '.BookCarrActualTime = Left(oItem.BookCarrActualTime, 20)
                    '.BookCarrScheduleTime = Left(oItem.BookCarrScheduleTime, 20)
                    If validateDateWS(oItem.BookCarrActLoadCompleteDate, dtVal) Then
                        .BookCarrActLoadCompleteDate = exportDateToString(dtVal.ToString)
                    End If
                    '.BookCarrActLoadCompleteTime = Left(oItem.BookCarrActLoadCompleteTime, 20)
                    '.CompNumber = Left(oItem.CompNumber, 50)
                    '.BookPRONumber = Left(oItem.BookPRONumber, 20)
                    '.BookOrderSequence = oItem.BookOrderSequence

                    If validateDateWS(oItem.BookCarrStartLoadingDate, dtVal) Then
                        .BookCarrStartLoadingDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrStartLoadingTime) Then
                        .BookCarrStartLoadingTime = oItem.BookCarrStartLoadingTime
                    End If

                    If validateDateWS(oItem.BookCarrFinishLoadingDate, dtVal) Then
                        .BookCarrFinishLoadingDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrFinishLoadingTime) Then
                        .BookCarrFinishLoadingTime = oItem.BookCarrFinishLoadingTime
                    End If

                    '.BookCarrDockDelAssignment = Left(oItem.BookCarrDockDelAssignment, 10)

                    If validateDateWS(oItem.BookCarrApptDate, dtVal) Then
                        .BookCarrApptDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrApptTime) Then
                        .BookCarrApptTime = oItem.BookCarrApptTime
                    End If

                    If validateDateWS(oItem.BookCarrActDate, dtVal) Then
                        .BookCarrActDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrActTime) Then
                        .BookCarrActTime = oItem.BookCarrActTime
                    End If

                    If validateDateWS(oItem.BookCarrStartUnloadingDate, dtVal) Then
                        .BookCarrStartUnloadingDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrStartUnloadingTime) Then
                        .BookCarrStartUnloadingTime = oItem.BookCarrStartUnloadingTime
                    End If

                    If validateDateWS(oItem.BookCarrFinishUnloadingDate, dtVal) Then
                        .BookCarrFinishUnloadingDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrFinishUnloadingTime) Then
                        .BookCarrFinishUnloadingTime = oItem.BookCarrFinishUnloadingTime
                    End If

                    If validateDateWS(oItem.BookCarrActUnloadCompDate, dtVal) Then
                        .BookCarrActUnloadCompDate = exportDateToString(dtVal.ToString)
                    End If
                    If ValidateTimeWS(oItem.BookCarrActUnloadCompTime) Then
                        .BookCarrActUnloadCompTime = oItem.BookCarrActUnloadCompTime
                    End If
                    '.BookShipCarrierProNumber = Left(oItem.BookShipCarrierProNumber, 20)
                    '.BookShipCarrierName = Left(oItem.BookShipCarrierName, 60)
                    '.BookShipCarrierNumber = Left(oItem.BookShipCarrierNumber, 80)
                End With
                schedList.Add(oItem)
                'Else
                '    AddToGroupEmailMsg("One of the schedule records was null or empty and could not be processed.")
                '    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                'End If
            Next

            intRet = ProcessData70(schedList, strConnection)
        Catch ex As Exception
            LogException("Process Object Data 70 Failure", "Order import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsSchedule.ProcessObjectData70 Failure")
        End Try
        Return intRet


    End Function


    Public Function ProcessData( _
                    ByVal oSchedules As ScheduleData.ScheduleHeaderDataTable, _
                    ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim shtServerID As Short = 0
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Dim strHeaderTable As String = "Book"
        Dim strItemTable As String = ""
        Me.HeaderName = "Booking Schedule"
        Me.ItemName = ""
        Me.ImportTypeKey = IntegrationTypes.Schedule
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Schedule Data Integration"
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If


        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection(oFields) Then Exit Function

        Try
            'Import the Header Records
            importHeaderRecords(oSchedules, oFields)
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Schedule Data Warning", "The following errors or warnings were reported some schedule records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Schedule Data Failure", "The following errors or warnings were reported some schedule records may not have been processed correctly." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Import Error Report for more information."
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)

        Catch ex As Exception
            LogException("Process Schedule Data Failure", "Could not process the requested schedule data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsSchedule.ProcessData")
        Finally
            closeConnection()
        End Try
        Return intRet
    End Function

    Public Function ProcessData70( _
                    ByVal oSchedules As List(Of clsScheduleObject70), _
                    ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Me.HeaderName = "Booking Schedule"
        Me.ItemName = ""
        Me.ImportTypeKey = IntegrationTypes.Schedule
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Schedule Data Integration"
        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If


        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection70(oFields) Then Exit Function

        Try
            'Import the Header Records
            importHeaderRecords70(oSchedules, oFields)
            strTitle = "ProcessData70 Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Schedule Data Warning", "The following errors or warnings were reported some schedule records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Schedule Data Failure", "The following errors or warnings were reported some schedule records may not have been processed correctly." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                If Me.RecordErrors > 0 Then
                    strTitle = "ProcessData70 Complete With Errors"
                    strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Import Error Report for more information."
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)

        Catch ex As Exception
            LogException("Process Schedule Data 70 Failure", "Could not process the requested schedule data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsSchedule.ProcessData70")
        Finally
            closeConnection()
        End Try
        Return intRet
    End Function


    Private Function importHeaderRecords( _
        ByRef oSchedules As ScheduleData.ScheduleHeaderDataTable, _
        ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            'now get the Schedule Header Records
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsSchedule.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try
                    Try
                        Dim lngMax As Long = oSchedules.Count
                        Log("Importing " & lngMax & " Schedule Header Records.")
                        For Each oRow As ScheduleData.ScheduleHeaderRow In oSchedules
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oFields("CompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("CompNumber").Length = 50
                            oFields("CompNumber").Null = True
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
                            If blnDataValidated Then blnDataValidated = validateCompany(oFields("CompNumber"), _
                                                                                                strErrorMessage, _
                                                                                                oCX, _
                                                                                                strSource)
                            'Only import records by Pro Number so the getProNumber check for an empty Pro and gets it based on Order Number and Company Number if needed.
                            If blnDataValidated AndAlso getProNumber(oFields) Then
                                'test if the record already exists.
                                If blnDataValidated Then blnDataValidated = doesRecordExist(oFields, _
                                                                                                    strErrorMessage, _
                                                                                                    blnInsertRecord, _
                                                                                                    "Book Pro Number " & oFields("BookProNumber").Value, _
                                                                                                    "Book")
                            End If

                            If Not blnDataValidated Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                            Else
                                oFields.Item(7).Name = "[BookCarrActLoadComplete Date]"
                                'Save the changes to the main table
                                If SaveData(oFields, False, "Book", "BookModUser", "BookModDate", True) Then
                                    TotalRecords += 1
                                End If
                            End If

                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsSchedule.importHeaderRecords, attempted to import schedule header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsSchedule.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("importHeaderRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsSchedule.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsSchedule.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importHeaderRecords70( _
       ByRef oSchedules As List(Of clsScheduleObject70), _
       ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            'now get the Schedule Header Records
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsSchedule.importHeaderRecords70"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try
                    Try
                        Dim lngMax As Long = oSchedules.Count
                        Log("Importing " & lngMax & " Schedule Header Records.")
                        For Each oRow As clsScheduleObject70 In oSchedules
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oFields("CompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("CompNumber").Length = 50
                            oFields("CompNumber").Null = True
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
                            If blnDataValidated Then blnDataValidated = validateCompany(oFields("CompNumber"), _
                                                                                                strErrorMessage, _
                                                                                                oCX, _
                                                                                                strSource)
                            'Only import records by Pro Number so the getProNumber check for an empty Pro and gets it based on Order Number and Company Number if needed.
                            'If blnDataValidated AndAlso getProNumber(oFields) Then
                            '    'test if the record already exists.
                            '    If blnDataValidated Then blnDataValidated = doesRecordExist(oFields, _
                            '                                                                        strErrorMessage, _
                            '                                                                        blnInsertRecord, _
                            '                                                                        "Book Pro Number " & oFields("BookProNumber").Value, _
                            '                                                                        "Book")
                            'End If

                            If Not blnDataValidated Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                            Else
                                oFields.Item(7).Name = "[BookCarrActLoadComplete Date]"
                                'Save the changes to the main table
                                If SaveData(oFields, False, "Book", "BookModUser", "BookModDate", True) Then
                                    TotalRecords += 1
                                End If
                            End If

                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsSchedule.importHeaderRecords70, attempted to import schedule header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsSchedule.importHeaderRecords70 Failed!" & readExceptionMessage(ex))
                    Else
                        Log("importHeaderRecords70 Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsSchedule.importHeaderRecords70, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsSchedule.importHeaderRecords70 Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function


    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("BookCarrOrderNumber", "BookCarrOrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcHK)
                .Add("BookCarrDockPUAssigment", "BookCarrDockPUAssigment", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("BookCarrScheduleDate", "BookCarrScheduleDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrScheduleTime", "BookCarrScheduleTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrActualDate", "BookCarrActualDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrActualTime", "BookCarrActualTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrActLoadCompleteDate", "BookCarrActLoadCompleteDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrActLoadCompleteTime", "BookCarrActLoadCompleteTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookProNumber", "BookProNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK)
                .Add("CompNumber", "CompNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK)
                .Add("BookOrderSequence", "BookOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK)
                .Add("BookCarrStartLoadingDate", "BookCarrStartLoadingDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrStartLoadingTime", "BookCarrStartLoadingTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrFinishLoadingDate", "BookCarrFinishLoadingDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrFinishLoadingTime", "BookCarrFinishLoadingTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrDockDelAssignment", "BookCarrDockDelAssignment", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("BookCarrApptDate", "BookCarrApptDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrApptTime", "BookCarrApptTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrActDate", "BookCarrActDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrActTime", "BookCarrActTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrStartUnloadingDate", "BookCarrStartUnloadingDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrStartUnloadingTime", "BookCarrStartUnloadingTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrFinishUnloadingDate", "BookCarrFinishUnloadingDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrFinishUnloadingTime", "BookCarrFinishUnloadingTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrActUnloadCompDate", "BookCarrActUnloadCompDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrActUnloadCompTime", "BookCarrActUnloadCompTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookShipCarrierName", "BookShipCarrierName", clsImportField.DataTypeID.gcvdtString, 60, True)
                .Add("BookShipCarrierProNumber", "BookShipCarrierProNumber", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("BookShipCarrierNumber", "BookShipCarrierNumber", clsImportField.DataTypeID.gcvdtString, 80, True)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try

                    If oFields(ct).Name = "BookProNumber" Or oFields(ct).Name = "CompNumber" Or oFields(ct).Name = "BookCarrOrderNumber" Or oFields(ct).Name = "BookOrderSequence" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Schedule)
                    End If

                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsSchedule.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsSchedule.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildHeaderCollection70(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("BookCarrOrderNumber", "BookCarrOrderNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcHK)
                .Add("BookCarrDockPUAssigment", "BookCarrDockPUAssigment", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("BookCarrScheduleDate", "BookCarrScheduleDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrScheduleTime", "BookCarrScheduleTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrActualDate", "BookCarrActualDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrActualTime", "BookCarrActualTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrActLoadCompleteDate", "BookCarrActLoadCompleteDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrActLoadCompleteTime", "BookCarrActLoadCompleteTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookProNumber", "BookProNumber", clsImportField.DataTypeID.gcvdtString, 20, True, clsImportField.PKValue.gcPK)
                .Add("CompNumber", "CompNumber", clsImportField.DataTypeID.gcvdtString, 50, True, clsImportField.PKValue.gcHK)
                .Add("BookOrderSequence", "BookOrderSequence", clsImportField.DataTypeID.gcvdtLongInt, 11, True, clsImportField.PKValue.gcHK)
                .Add("BookCarrStartLoadingDate", "BookCarrStartLoadingDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrStartLoadingTime", "BookCarrStartLoadingTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrFinishLoadingDate", "BookCarrFinishLoadingDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrFinishLoadingTime", "BookCarrFinishLoadingTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrDockDelAssignment", "BookCarrDockDelAssignment", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("BookCarrApptDate", "BookCarrApptDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrApptTime", "BookCarrApptTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrActDate", "BookCarrActDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrActTime", "BookCarrActTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrStartUnloadingDate", "BookCarrStartUnloadingDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrStartUnloadingTime", "BookCarrStartUnloadingTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrFinishUnloadingDate", "BookCarrFinishUnloadingDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrFinishUnloadingTime", "BookCarrFinishUnloadingTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookCarrActUnloadCompDate", "BookCarrActUnloadCompDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("BookCarrActUnloadCompTime", "BookCarrActUnloadCompTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("BookShipCarrierName", "BookShipCarrierName", clsImportField.DataTypeID.gcvdtString, 60, True)
                .Add("BookShipCarrierProNumber", "BookShipCarrierProNumber", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("BookShipCarrierNumber", "BookShipCarrierNumber", clsImportField.DataTypeID.gcvdtString, 80, True)
                .Add("CompLegalEntity", "CompLegalEntity", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompAlphaCode", "CompAlphaCode", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try

                    If oFields(ct).Name = "BookProNumber" Or oFields(ct).Name = "CompNumber" Or oFields(ct).Name = "BookCarrOrderNumber" Or oFields(ct).Name = "BookOrderSequence" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Schedule)
                    End If

                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsSchedule.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsSchedule.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Public Overloads Function SaveData(ByRef oFields As clsImportFields) As Boolean
        Dim blnRet As Boolean = False
        Dim strRetMsg As String = ""
        Dim intErrNumber As Integer = 0
        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmd As New SqlCommand
                Dim intRecCt As Integer = 0
                Try
                    'check the active db connection
                    If Me.openConnection Then
                        cmd = New SqlClient.SqlCommand()
                        With cmd
                            .Connection = Me.DBCon
                            .CommandTimeout = 600
                            .Parameters.Add("@BookProNumber", SqlDbType.NVarChar, 20)
                            .Parameters("@BookProNumber").Value = Left(oFields("BookProNumber").Value, 20)
                            .Parameters.Add("@UserName", SqlDbType.NVarChar, 25)
                            .Parameters("@UserName").Value = Left(CreateUser, 25)
                            .Parameters.Add("@BookCarrDockPUAssigment", SqlDbType.NVarChar, 10)
                            If Not oFields("BookCarrDockPUAssigment").Value.ToUpper = "NULL" AndAlso Not oFields("BookCarrDockPUAssigment").Value = "''" Then .Parameters("@BookCarrDockPUAssigment").Value = Left(oFields("BookCarrDockPUAssigment").Value, 10)
                            .Parameters.Add("@BookCarrScheduleDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrScheduleDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrScheduleDate").Value = oFields("BookCarrScheduleDate").Value

                            .Parameters.Add("@BookCarrScheduleTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrScheduleTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrScheduleTime").Value = oFields("BookCarrScheduleTime").Value
                            .Parameters.Add("@BookCarrActualDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrActualDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActualDate").Value = oFields("BookCarrActualDate").Value
                            .Parameters.Add("@BookCarrActualTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrActualTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActualTime").Value = oFields("BookCarrActualTime").Value
                            .Parameters.Add("@BookCarrActLoadComplete", SqlDbType.DateTime)
                            If Not oFields("BookCarrActLoadComplete").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActLoadComplete").Value = oFields("BookCarrActLoadComplete").Value
                            .Parameters.Add("@BookCarrActLoadCompleteTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrActLoadCompleteTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActLoadCompleteTime").Value = oFields("BookCarrActLoadCompleteTime").Value
                            .Parameters.Add("@BookCarrStartLoadingDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrStartLoadingDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrStartLoadingDate").Value = oFields("BookCarrStartLoadingDate").Value
                            .Parameters.Add("@BookCarrStartLoadingTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrStartLoadingTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrStartLoadingTime").Value = oFields("BookCarrStartLoadingTime").Value
                            .Parameters.Add("@BookCarrFinishLoadingDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrFinishLoadingDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrFinishLoadingDate").Value = oFields("BookCarrFinishLoadingDate").Value
                            .Parameters.Add("@BookCarrFinishLoadingTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrFinishLoadingTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrFinishLoadingTime").Value = oFields("BookCarrFinishLoadingTime").Value

                            .Parameters.Add("@BookCarrDockDelAssignment", SqlDbType.NVarChar, 10)
                            If Not oFields("BookCarrDockDelAssignment").Value.ToUpper = "NULL" AndAlso Not oFields("BookCarrDockDelAssignment").Value = "''" Then .Parameters("@BookCarrDockDelAssignment").Value = Left(oFields("BookCarrDockDelAssignment").Value, 10)

                            .Parameters.Add("@BookCarrApptDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrApptDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrApptDate").Value = oFields("BookCarrApptDate").Value
                            .Parameters.Add("@BookCarrApptTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrApptTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrApptTime").Value = oFields("BookCarrApptTime").Value
                            .Parameters.Add("@BookCarrActDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrActDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActDate").Value = oFields("BookCarrActDate").Value

                            .Parameters.Add("@BookCarrActTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrActTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActTime").Value = oFields("BookCarrActTime").Value
                            .Parameters.Add("@BookCarrStartUnloadingDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrStartUnloadingDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrStartUnloadingDate").Value = oFields("BookCarrStartUnloadingDate").Value
                            .Parameters.Add("@BookCarrStartUnloadingTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrStartUnloadingTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrStartUnloadingTime").Value = oFields("BookCarrStartUnloadingTime").Value
                            .Parameters.Add("@BookCarrFinishUnloadingDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrFinishUnloadingDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrFinishUnloadingDate").Value = oFields("BookCarrFinishUnloadingDate").Value
                            .Parameters.Add("@BookCarrFinishUnloadingTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrFinishUnloadingTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrFinishUnloadingTime").Value = oFields("BookCarrFinishUnloadingTime").Value
                            .Parameters.Add("@BookCarrActUnloadCompDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrActUnloadCompDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActUnloadCompDate").Value = oFields("BookCarrActUnloadCompDate").Value
                            .Parameters.Add("@BookCarrActUnloadCompTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrActUnloadCompTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActUnloadCompTime").Value = oFields("BookCarrActUnloadCompTime").Value

                            If Not oFields("BookShipCarrierName").Value.ToUpper = "NULL" AndAlso oFields("BookShipCarrierName").Value.Trim.Length > 1 Then
                                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@BookShipCarrierName", oFields("BookShipCarrierName").Value))
                            End If
                            If Not oFields("BookShipCarrierProNumber").Value.ToUpper = "NULL" AndAlso oFields("BookShipCarrierProNumber").Value.Trim.Length > 1 Then
                                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@BookShipCarrierProNumber", oFields("BookShipCarrierProNumber").Value))
                            End If
                            If Not oFields("BookShipCarrierNumber").Value.ToUpper = "NULL" AndAlso oFields("BookShipCarrierNumber").Value.Trim.Length > 1 Then
                                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@BookShipCarrierNumber", oFields("BookShipCarrierNumber").Value))
                            End If

                            .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 4000)
                            .Parameters("@RetMsg").Direction = ParameterDirection.Output
                            .Parameters.Add("@ErrNumber", SqlDbType.Int)
                            .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                            .CommandText = "dbo.spUpdateScheduleData"
                            .CommandType = CommandType.StoredProcedure
                            .ExecuteNonQuery()
                            strRetMsg = Trim(.Parameters("@RetMsg").Value.ToString)
                            If IsDBNull(.Parameters("@ErrNumber").Value) Then
                                intErrNumber = 0
                            Else
                                intErrNumber = .Parameters("@ErrNumber").Value
                            End If
                        End With
                        Try
                            If intErrNumber <> 0 Then
                                If intRetryCt > Me.Retry Then
                                    Me.RecordErrors += 1
                                    ITEmailMsg &= "<br />" & Source & " Failure: clsSchedule.saveData: Procedure spUpdateScheduleData output failure: " _
                                        & vbCrLf & "Error # " & intErrNumber & ": " & strRetMsg & ".  Could not update load status schedule data.<br />" & vbCrLf
                                    Log("saveData Failed!")
                                    Exit Do
                                Else
                                    Log("NGL.FreightMaster.Integration.clsSchedule.saveData Output Failure Retry = " & intRetryCt.ToString)
                                End If
                            Else
                                'return true to the calling procedrue
                                blnRet = True
                                Exit Do
                            End If
                        Catch ex As Exception
                            Log("NGL.FreightMaster.Integration.clsSchedule.saveData Unexpected Error " & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString)
                        End Try
                    Else
                        If intRetryCt > Me.Retry Then
                            Me.RecordErrors += 1
                            ITEmailMsg &= "<br />" & Source & " Failure: clsSchedule.saveData: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not import freight bill record.<br />" & vbCrLf
                            Log("saveData Failed!")
                            Exit Do
                        Else
                            Log("NGL.FreightMaster.Integration.clsSchedule.saveData Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If

                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsSchedule.saveData unexpected error. Could not update load status schedule data:<br />" & vbCrLf & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString & "<br />" & vbCrLf
                        Log("saveData Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsSchedule.saveData Unexpected Error " & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString)
                    End If
                Finally
                    Try
                        cmd.Cancel()
                        cmd = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsSchedule.saveData: Could not update load status schedule data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("saveData Failed!" & readExceptionMessage(ex))
        End Try
        Return blnRet

    End Function

    Public Overloads Function SaveData70(ByRef oFields As clsImportFields) As Boolean
        Dim blnRet As Boolean = False
        Dim strRetMsg As String = ""
        Dim intErrNumber As Integer = 0
        Try
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim cmd As New SqlCommand
                Dim intRecCt As Integer = 0
                Try
                    'check the active db connection
                    If Me.openConnection Then
                        cmd = New SqlClient.SqlCommand()
                        With cmd
                            .Connection = Me.DBCon
                            .CommandTimeout = 600
                            '.Parameters.Add("@BookProNumber", SqlDbType.NVarChar, 20)
                            '.Parameters("@BookProNumber").Value = Left(oFields("BookProNumber").Value, 20)

                            .Parameters.Add("@BookCarrOrderNumber", SqlDbType.NVarChar, 20)
                            .Parameters("@BookCarrOrderNumber").Value = oFields("BookCarrOrderNumber").Value
                            .Parameters.Add("@BookOrderSequence", SqlDbType.Int)
                            .Parameters("@BookOrderSequence").Value = oFields("BookOrderSequence").Value
                            .Parameters.Add("@CompNumber", SqlDbType.NVarChar, 50)
                            .Parameters("@CompNumber").Value = oFields("CompNumber").Value

                            .Parameters.Add("@UserName", SqlDbType.NVarChar, 25)
                            .Parameters("@UserName").Value = Left(CreateUser, 25)
                            .Parameters.Add("@BookCarrDockPUAssigment", SqlDbType.NVarChar, 10)
                            If Not oFields("BookCarrDockPUAssigment").Value.ToUpper = "NULL" AndAlso Not oFields("BookCarrDockPUAssigment").Value = "''" Then .Parameters("@BookCarrDockPUAssigment").Value = Left(oFields("BookCarrDockPUAssigment").Value, 10)
                            .Parameters.Add("@BookCarrScheduleDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrScheduleDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrScheduleDate").Value = oFields("BookCarrScheduleDate").Value

                            .Parameters.Add("@BookCarrScheduleTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrScheduleTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrScheduleTime").Value = oFields("BookCarrScheduleTime").Value
                            .Parameters.Add("@BookCarrActualDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrActualDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActualDate").Value = oFields("BookCarrActualDate").Value
                            .Parameters.Add("@BookCarrActualTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrActualTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActualTime").Value = oFields("BookCarrActualTime").Value
                            .Parameters.Add("@BookCarrActLoadComplete", SqlDbType.DateTime)
                            If Not oFields("BookCarrActLoadComplete").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActLoadComplete").Value = oFields("BookCarrActLoadComplete").Value
                            .Parameters.Add("@BookCarrActLoadCompleteTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrActLoadCompleteTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActLoadCompleteTime").Value = oFields("BookCarrActLoadCompleteTime").Value
                            .Parameters.Add("@BookCarrStartLoadingDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrStartLoadingDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrStartLoadingDate").Value = oFields("BookCarrStartLoadingDate").Value
                            .Parameters.Add("@BookCarrStartLoadingTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrStartLoadingTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrStartLoadingTime").Value = oFields("BookCarrStartLoadingTime").Value
                            .Parameters.Add("@BookCarrFinishLoadingDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrFinishLoadingDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrFinishLoadingDate").Value = oFields("BookCarrFinishLoadingDate").Value
                            .Parameters.Add("@BookCarrFinishLoadingTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrFinishLoadingTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrFinishLoadingTime").Value = oFields("BookCarrFinishLoadingTime").Value

                            .Parameters.Add("@BookCarrDockDelAssignment", SqlDbType.NVarChar, 10)
                            If Not oFields("BookCarrDockDelAssignment").Value.ToUpper = "NULL" AndAlso Not oFields("BookCarrDockDelAssignment").Value = "''" Then .Parameters("@BookCarrDockDelAssignment").Value = Left(oFields("BookCarrDockDelAssignment").Value, 10)

                            .Parameters.Add("@BookCarrApptDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrApptDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrApptDate").Value = oFields("BookCarrApptDate").Value
                            .Parameters.Add("@BookCarrApptTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrApptTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrApptTime").Value = oFields("BookCarrApptTime").Value
                            .Parameters.Add("@BookCarrActDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrActDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActDate").Value = oFields("BookCarrActDate").Value

                            .Parameters.Add("@BookCarrActTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrActTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActTime").Value = oFields("BookCarrActTime").Value
                            .Parameters.Add("@BookCarrStartUnloadingDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrStartUnloadingDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrStartUnloadingDate").Value = oFields("BookCarrStartUnloadingDate").Value
                            .Parameters.Add("@BookCarrStartUnloadingTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrStartUnloadingTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrStartUnloadingTime").Value = oFields("BookCarrStartUnloadingTime").Value
                            .Parameters.Add("@BookCarrFinishUnloadingDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrFinishUnloadingDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrFinishUnloadingDate").Value = oFields("BookCarrFinishUnloadingDate").Value
                            .Parameters.Add("@BookCarrFinishUnloadingTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrFinishUnloadingTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrFinishUnloadingTime").Value = oFields("BookCarrFinishUnloadingTime").Value
                            .Parameters.Add("@BookCarrActUnloadCompDate", SqlDbType.DateTime)
                            If Not oFields("BookCarrActUnloadCompDate").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActUnloadCompDate").Value = oFields("BookCarrActUnloadCompDate").Value
                            .Parameters.Add("@BookCarrActUnloadCompTime", SqlDbType.DateTime)
                            If Not oFields("BookCarrActUnloadCompTime").Value.ToUpper = "NULL" Then .Parameters("@BookCarrActUnloadCompTime").Value = oFields("BookCarrActUnloadCompTime").Value

                            If Not oFields("BookShipCarrierName").Value.ToUpper = "NULL" AndAlso oFields("BookShipCarrierName").Value.Trim.Length > 1 Then
                                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@BookShipCarrierName", oFields("BookShipCarrierName").Value))
                            End If
                            If Not oFields("BookShipCarrierProNumber").Value.ToUpper = "NULL" AndAlso oFields("BookShipCarrierProNumber").Value.Trim.Length > 1 Then
                                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@BookShipCarrierProNumber", oFields("BookShipCarrierProNumber").Value))
                            End If
                            If Not oFields("BookShipCarrierNumber").Value.ToUpper = "NULL" AndAlso oFields("BookShipCarrierNumber").Value.Trim.Length > 1 Then
                                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@BookShipCarrierNumber", oFields("BookShipCarrierNumber").Value))
                            End If

                            .Parameters.Add("@CompLegalEntity", SqlDbType.NVarChar, 50)
                            .Parameters("@CompLegalEntity").Value = oFields("CompLegalEntity").Value
                            .Parameters.Add("@CompAlphaCode", SqlDbType.NVarChar, 50)
                            .Parameters("@CompAlphaCode").Value = oFields("CompAlphaCode").Value

                            .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 4000)
                            .Parameters("@RetMsg").Direction = ParameterDirection.Output
                            .Parameters.Add("@ErrNumber", SqlDbType.Int)
                            .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                            .CommandText = "dbo.spUpdateScheduleData70"
                            .CommandType = CommandType.StoredProcedure
                            .ExecuteNonQuery()
                            strRetMsg = Trim(.Parameters("@RetMsg").Value.ToString)
                            If IsDBNull(.Parameters("@ErrNumber").Value) Then
                                intErrNumber = 0
                            Else
                                intErrNumber = .Parameters("@ErrNumber").Value
                            End If
                        End With
                        Try
                            If intErrNumber <> 0 Then
                                If intRetryCt > Me.Retry Then
                                    Me.RecordErrors += 1
                                    ITEmailMsg &= "<br />" & Source & " Failure: clsSchedule.saveData70: Procedure spUpdateScheduleData70 output failure: " _
                                        & vbCrLf & "Error # " & intErrNumber & ": " & strRetMsg & ".  Could not update load status schedule data.<br />" & vbCrLf
                                    Log("saveData70 Failed!")
                                    Exit Do
                                Else
                                    Log("NGL.FreightMaster.Integration.clsSchedule.saveData70 Output Failure Retry = " & intRetryCt.ToString)
                                End If
                            Else
                                'return true to the calling procedrue
                                blnRet = True
                                Exit Do
                            End If
                        Catch ex As Exception
                            Log("NGL.FreightMaster.Integration.clsSchedule.saveData70 Unexpected Error " & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString)
                        End Try
                    Else
                        If intRetryCt > Me.Retry Then
                            Me.RecordErrors += 1
                            ITEmailMsg &= "<br />" & Source & " Failure: clsSchedule.saveData70: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not import freight bill record.<br />" & vbCrLf
                            Log("saveData Failed!")
                            Exit Do
                        Else
                            Log("NGL.FreightMaster.Integration.clsSchedule.saveData70 Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If

                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: clsSchedule.saveData70 unexpected error. Could not update load status schedule data:<br />" & vbCrLf & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString & "<br />" & vbCrLf
                        Log("saveData70 Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsSchedule.saveData70 Unexpected Error " & readExceptionMessage(ex) & vbCrLf & " Retry = " & intRetryCt.ToString)
                    End If
                Finally
                    Try
                        cmd.Cancel()
                        cmd = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: clsSchedule.saveData70: Could not update load status schedule data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("saveData70 Failed!" & readExceptionMessage(ex))
        End Try
        Return blnRet

    End Function

End Class
