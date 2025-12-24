Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.IO.IsolatedStorage
Imports System.Windows.Forms
Imports Serilog.Core
Imports Microsoft.VisualBasic.Logging

Public Class NGLSystemDataProvider
    Inherits NGLDataProviderBaseClass

#Region " Constructors "
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal strConnection As String)
        MyBase.New()
        ConnectionString = strConnection
    End Sub

    Public Sub New(ByVal oParameters As WCFParameters)
        processParameters(oParameters)
    End Sub

#End Region

#Region " System Parameters"

    Public Function GetGlobalParameters() As DTO.GlobalParameter()

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                Dim oParQ = From par In db.Parameters Where par.ParIsGlobal = True Order By par.ParKey Select par
                Dim pars(oParQ.Count - 1) As DTO.GlobalParameter
                Dim i As Integer = 0
                For Each par In oParQ
                    Dim opar As New DTO.GlobalParameter
                    With opar
                        .ParKey = par.ParKey
                        If par.ParValue.HasValue Then
                            .ParValue = par.ParValue.Value
                        Else
                            .ParValue = 0
                        End If
                        .ParText = par.ParText
                        .ParDescription = .ParDescription
                        If Not par.ParOLE Is Nothing Then
                            .ParOLE = par.ParOLE.ToArray
                        End If
                        If Not par.ParUpdated Is Nothing Then
                            .upsize_ts = par.ParUpdated.ToArray
                        End If
                        .ParCategoryControl = .ParCategoryControl
                        .ParIsGlobal = .ParIsGlobal
                    End With
                    pars(i) = opar
                    i += 1
                Next


                'For i = 0 To oParQ.Count - 1
                '    Dim opar As New DTO.GlobalParameter
                '    With opar
                '        .ParKey = oParQ(i).ParKey
                '    End With
                '    pars(i) = opar
                'Next
                Return pars
            Catch ex As System.Data.SqlClient.SqlException
                Throw New DatabaseReadDataException("Cannot read Global Parameter Data", ex)
            Catch ex As Exception
                Throw
            End Try
            'Dim pars() As DTO.GlobalParameter = (From t In db.Parameters Where t.ParIsGlobal = True Order By t.ParKey _
            '                          Select New DTO.GlobalParameter _
            '                          With {.ParCategoryControl = t.ParCategoryControl _
            '                                , .ParDescription = t.ParDescription _
            '                                , .ParIsGlobal = t.ParIsGlobal _
            '                                , .ParKey = t.ParKey _
            '                                , .ParOLE = t.ParOLE.ToArray() _
            '                                , .ParText = t.ParText _
            '                                , .ParValue = t.ParValue _
            '                                , .upsize_ts = t.upsize_ts.ToArray() _
            '                                }).ToArray()


            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>DTO.GlobalTaskParameters</returns>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Added Cases for GlobalSMTPUseDefaultCredentials,
    ''' GlobalSMTPEnableSSL, GlobalSMTPTargetName, and GlobalSMTPPort
    ''' </remarks>
    Public Function GetGlobalTaskParameters() As DTO.GlobalTaskParameters
        Dim GTPs As New DTO.GlobalTaskParameters
        Try
            Dim pars() As DTO.GlobalParameter = GetGlobalParameters()
            For Each par In pars
                Select Case par.ParKey
                    Case "GlobalAutoRetry"
                        GTPs.GlobalAutoRetry = CInt(par.ParValue)
                    Case "GlobalAdminEmail"
                        GTPs.GlobalAdminEmail = par.ParText
                    Case "GlobalGroupEmail"
                        GTPs.GlobalGroupEmail = par.ParText
                    Case "GlobalFromEmail"
                        GTPs.GlobalFromEmail = par.ParText
                    Case "GlobalSMTPServer"
                        GTPs.GlobalSMTPServer = par.ParText
                    Case "GlobalSaveOldLogs"
                        If par.ParValue = 1 Then GTPs.GlobalSaveOldLogs = True Else GTPs.GlobalSaveOldLogs = False
                    Case "GlobalKeepLogDays"
                        GTPs.GlobalKeepLogDays = CInt(par.ParValue)
                    Case "GlobalDebugMode"
                        If par.ParValue = 1 Then GTPs.GlobalDebugMode = True Else GTPs.GlobalDebugMode = False
                    Case "GlobalFuelIndexUpdateEmailNotification"
                        If par.ParValue = 1 Then GTPs.GlobalFuelIndexUpdateEmailNotificationValue = True Else GTPs.GlobalFuelIndexUpdateEmailNotificationValue = False
                        GTPs.GlobalFuelIndexUpdateEmailNotification = par.ParText
                    Case "GlobalCarrierContractExpiredEmailNotification"
                        If par.ParValue = 1 Then GTPs.GlobalCarrierContractExpiredEmailNotificationValue = True Else GTPs.GlobalCarrierContractExpiredEmailNotificationValue = False
                        GTPs.GlobalCarrierContractExpiredEmailNotification = par.ParText
                    Case "GlobalCarrierExposureAllEmailNotification"
                        If par.ParValue = 1 Then GTPs.GlobalCarrierExposureAllEmailNotificationValue = True Else GTPs.GlobalCarrierExposureAllEmailNotificationValue = False
                        GTPs.GlobalCarrierExposureAllEmailNotification = par.ParText
                    Case "GlobalCarrierExposurePerShipmentEmailNotification"
                        If par.ParValue = 1 Then GTPs.GlobalCarrierExposurePerShipmentEmailNotificationValue = True Else GTPs.GlobalCarrierExposurePerShipmentEmailNotificationValue = False
                        GTPs.GlobalCarrierExposurePerShipmentEmailNotification = par.ParText
                    Case "GlobalCarrierInsuranceExpiredEmailNotification"
                        If par.ParValue = 1 Then GTPs.GlobalCarrierInsuranceExpiredEmailNotificationValue = True Else GTPs.GlobalCarrierInsuranceExpiredEmailNotificationValue = False
                        GTPs.GlobalCarrierInsuranceExpiredEmailNotification = par.ParText
                    Case "GlobalOutdatedNoLanePOEmailNotification"
                        If par.ParValue = 1 Then GTPs.GlobalOutdatedNoLanePOEmailNotificationValue = True Else GTPs.GlobalOutdatedNoLanePOEmailNotificationValue = False
                        GTPs.GlobalOutdatedNoLanePOEmailNotification = par.ParText
                    Case "GlobalOutdatedNStatusEmailNotification"
                        If par.ParValue = 1 Then GTPs.GlobalOutdatedNStatusEmailNotificationValue = True Else GTPs.GlobalOutdatedNStatusEmailNotificationValue = False
                        GTPs.GlobalOutdatedNStatusEmailNotification = par.ParText
                    Case "GlobalPOsWaitingEmailNotification"
                        If par.ParValue = 1 Then GTPs.GlobalPOsWaitingEmailNotificationValue = True Else GTPs.GlobalPOsWaitingEmailNotificationValue = False
                        GTPs.GlobalPOsWaitingEmailNotification = par.ParText
                    Case "NEXTStopAcctNo"
                        GTPs.NEXTStopAcctNo = par.ParText
                    Case "NEXTStopContact"
                        GTPs.NEXTStopContact = par.ParText
                    Case "NEXTStopHotLoadAccountName"
                        GTPs.NEXTStopHotLoadAccountName = par.ParText
                    Case "NEXTStopHotLoadContact"
                        GTPs.NEXTStopHotLoadContact = par.ParText
                    Case "NEXTStopHotLoadURL"
                        GTPs.NEXTStopHotLoadURL = par.ParText
                    Case "NEXTStopPhone"
                        GTPs.NEXTStopPhone = par.ParText
                    Case "NEXTStopURL"
                        GTPs.NEXTStopURL = par.ParText
                    Case "NEXTrackURL"
                        GTPs.NEXTrackURL = par.ParText
                    Case "GlobalWebDatabase"
                        GTPs.NEXTRackDatabase = par.ParText
                    Case "GlobalWebDatabaseServer"
                        GTPs.NEXTRackDatabaseServer = par.ParText
                    Case "GlobalDefaultLoadAcceptAllowedMinutes"
                        GTPs.GlobalDefaultLoadAcceptAllowedMinutes = CInt(par.ParValue)
                    Case "GlobalSMTPUser"
                        GTPs.GlobalSMTPUser = par.ParText
                    Case "GlobalSMTPPass"
                        GTPs.GlobalSMTPPass = par.ParText
                    Case "ReportServerURL"
                        GTPs.ReportServerURL = par.ParText
                    Case "ReportServerUser"
                        GTPs.ReportServerUser = par.ParText
                    Case "ReportServerPass"
                        GTPs.ReportServerPass = par.ParText
                    Case "ReportServerDomain"
                        GTPs.ReportServerDomain = par.ParText
                    Case "GlobalSMTPUseDefaultCredentials"
                        GTPs.GlobalSMTPUseDefaultCredentials = CInt(par.ParValue)
                    Case "GlobalSMTPEnableSSL"
                        GTPs.GlobalSMTPEnableSSL = CInt(par.ParValue)
                    Case "GlobalSMTPTargetName"
                        GTPs.GlobalSMTPTargetName = par.ParText
                    Case "GlobalSMTPPort"
                        GTPs.GlobalSMTPPort = CInt(par.ParValue)
                End Select
            Next
        Catch ex As System.Data.SqlClient.SqlException
            Throw New DatabaseReadDataException("Cannot read Global Task Parameter Data", ex)
        Catch ex As DatabaseReadDataException
            Throw
        Catch ex As Exception
            Throw
        End Try
        Return GTPs
    End Function

#End Region

#Region " Procedures and Functions"

    Public Shared Function getElmtFieldDataType(ByVal strconnection As String, ByVal strElmtName As String) As LTS.vElmtDataType
        Using db As New NGLMASSYSDataContext(strconnection)
            Try
                Return db.vElmtDataTypes.Where(Function(x) x.ElmtFieldName = strElmtName).FirstOrDefault()

            Catch ex As System.Data.SqlClient.SqlException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing


        End Using
    End Function

    Public Function GetDoFinalizeAlerts(ByVal BookControl As Integer) As DTO.DoFinalizeAlerts

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim Alerts As DTO.DoFinalizeAlerts = (
                    From t In db.spValidateAlertsBeforeFinalize(BookControl)
                    Select New DTO.DoFinalizeAlerts With {.CarrierControl = If(t.CarrierControl.HasValue, t.CarrierControl.Value, 0) _
                                                         , .CarrierNumber = If(t.CarrierNumber.HasValue, t.CarrierNumber.Value, 0) _
                                                         , .AllShipmentExposure = If(t.AllShipmentExposure.HasValue, t.AllShipmentExposure.Value, 0) _
                                                         , .ContractExpiresDate = t.ContractExpiresDate _
                                                         , .ContractExpiresMessage = t.ContractExpiresMessage _
                                                         , .ExposuerPerShipmentMessage = t.ExposuerPerShipmentMessage _
                                                         , .ExposureAllMessage = t.ExposureAllMessage _
                                                         , .InsuranceDate = t.InsuranceDate _
                                                         , .InsuranceMessage = t.InsuranceMessage _
                                                         , .PerShipmentExposure = If(t.PerShipmentExposure.HasValue, t.PerShipmentExposure.Value, 0) _
                                                         , .PerShipmentValue = If(t.PerShipmentValue.HasValue, t.PerShipmentValue.Value, 0) _
                                                         , .TotalExposueValue = If(t.TotalExposueValue.HasValue, t.TotalExposueValue.Value, 0)}).First
                Return Alerts
            Catch ex As System.Data.SqlClient.SqlException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' Returns a list of the Tasks to run by the Task Service
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.0 on 12/15/2016
    '''   designed to be used by the FM Task Serice so exceptions bubble up to the service
    ''' </remarks>
    Public Function GetTaskList() As LTS.tblRunTask()

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Return db.tblRunTasks().ToArray()
        End Using

    End Function

    ''' <summary>
    ''' Updates the task log table 
    ''' </summary>
    ''' <param name="strTaskName"></param>
    ''' <param name="strTaskDesc"></param>
    ''' <param name="intTaskType"></param>
    ''' <param name="strTaskCommand"></param>
    ''' <param name="dtTaskLastRanOn"></param>
    ''' <param name="dtTaskRanOn"></param>
    ''' <param name="strTaskMessage"></param>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.0 on 12/15/2016
    '''   designed to be used by the FM Task Serice so exceptions are ignored
    ''' </remarks>
    Public Function updateTaskLog(ByVal strTaskName As String,
                                ByVal strTaskDesc As String,
                                ByVal intTaskType As Integer,
                                ByVal strTaskCommand As String,
                                ByVal dtTaskLastRanOn As DateTime,
                                ByVal dtTaskRanOn As DateTime,
                                ByVal strTaskMessage As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim oTaskLog As New LTS.tblTaskLog()
                With oTaskLog
                    .TaskName = strTaskName
                    .TaskDesc = strTaskDesc
                    .TaskType = intTaskType
                    .TaskCommand = strTaskCommand
                    .TaskLastRanOn = dtTaskLastRanOn
                    .TaskRanOn = dtTaskRanOn
                    .TaskMessage = strTaskMessage
                End With
                db.tblTaskLogs.InsertOnSubmit(oTaskLog)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                'just return false on error
            End Try
            Return blnRet
        End Using

    End Function

    Public Function ValidateCarrierQualBeforeFinalize(ByVal BookControl As Integer) As LTS.spValidateCarrierQualBeforeFinalizeResult

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Return db.spValidateCarrierQualBeforeFinalize(BookControl, Me.Parameters.UserName).FirstOrDefault()
            Catch ex As System.Data.SqlClient.SqlException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using

    End Function

    Private Function DecodeAuth(ByVal PassCode As String) As Boolean

        Dim passnumber As Double = 0
        Dim passresult As Double = 0
        Dim passtext1 As String = ""
        Dim passtext2 As String = ""
        Dim passfraction As Double = 0
        Dim AuthDate As Date = Date.Now.AddDays(-31)
        Try

            If Len(PassCode) > 0 Then
                Double.TryParse(PassCode, passnumber)
                passresult = passnumber - 11111111111.0#
                passresult = passresult / 24124
                passfraction = passresult - Int(passresult)
                If passfraction > 0 Then passresult = 19000101
                passtext1 = Trim(Str(passresult))
                passtext2 = Mid$(passtext1, 5, 2) & "/" & Mid$(passtext1, 7, 2) & "/" & Left$(passtext1, 4)
                Date.TryParse(passtext2, AuthDate)
            End If


        Catch ex As Exception
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_ReadAuthenticationCodeError"))
        End Try

        If Date.Now > AuthDate Then
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_LicenseViolation"}, New FaultReason("E_AccessDenied"))
        ElseIf Date.Now.AddDays(30) > AuthDate Then
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_LicenseWarning"}, New FaultReason("E_AccessGranted"))
        Else
            Return True
        End If

        Return False



    End Function

    Public Function CheckAuthCode() As Boolean
        Return DecodeAuth(getScalarString("Select top 1 AuthNumber from dbo.Auth"))
    End Function

#End Region

#Region " AppErrors"


    Public Function GetAppErrors(ByVal MaxReturn As Integer) As DTO.AppError()

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim AppErrors() As DTO.AppError = (
                    From t In db.AppErrors
                    Order By t.ErrUser, t.ErrTime Descending
                    Select New DTO.AppError With {.Message = t.Message, .ErrUser = t.ErrUser, .ErrTime = t.ErrTime}).Take(MaxReturn).ToArray()
                Return AppErrors
            Catch ex As System.Data.SqlClient.SqlException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Function GetAppErrorsByUser(ByVal MaxReturn As Integer, ByVal UserName As String) As DTO.AppError()

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim AppErrors() As DTO.AppError = (
                    From t In db.AppErrors
                    Where t.ErrUser.Contains(UserName)
                    Order By t.ErrUser, t.ErrTime Descending
                    Select New DTO.AppError With {.Message = t.Message, .ErrUser = t.ErrUser, .ErrTime = t.ErrTime}).Take(MaxReturn).ToArray()
                Return AppErrors
            Catch ex As System.Data.SqlClient.SqlException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using

    End Function

    Public Sub CreateAppError(ByVal oData As DTO.AppError)


        Serilog.Log.Logger.Error("[{UserName}] - {Message}", oData.ErrUser, oData.Message)



    End Sub


    Public Sub CreateSystemErrorByMessage(ByVal Message As String,
                                          ByVal UserName As String,
                                          ByVal errorProcedure As String,
                                          Optional ByVal record As String = "",
                                          Optional ByVal errorNumber As Integer = 0,
                                          Optional ByVal errorSeverity As Integer = 0,
                                          Optional ByVal errorState As Integer = 0,
                                          Optional ByVal errorLineNber As Integer = 0)

        Serilog.Log.Error("[{UserName}] - {Message}\nerrorProcedure:{errorProcedure}", UserName, Message, errorProcedure)


    End Sub

    Public Sub CreateAppErrorByMessage(ByVal Message As String, ByVal UserName As String)
        Serilog.Log.Logger.Error("[{UserName}] - {Message}", UserName, Message)



    End Sub

#End Region

#Region " tblBatchProcessRunning"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="ProcessName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where InvalidOperationException throws FaultException
    '''     many of the callers are expecting the InvalidOperationException
    '''     to indicate that no records exist (old outdated logic)
    '''     we now return nothing 
    ''' </remarks>
    Public Function GettblBatchProcessRunning(ByVal UserName As String, ByVal ProcessName As String) As DTO.tblBatchProcessRunning
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                ' Modified by RHR for v-8.2 on 02/19/2021 
                If Not db.tblBatchProcessRunnings.Any(Function(x) x.BPRUserName.Contains(UserName) And x.BPRProcessName.Contains(ProcessName)) Then
                    Return Nothing
                End If
                Dim BatchProcessing As DTO.tblBatchProcessRunning = (
                    From t In db.tblBatchProcessRunnings
                    Where
                    t.BPRUserName.Contains(UserName) _
                    And
                    t.BPRProcessName.Contains(ProcessName)
                    Select New DTO.tblBatchProcessRunning With {.BPRControl = t.BPRControl,
                                                                .BPRUserName = t.BPRUserName,
                                                                .BPRProcessName = t.BPRProcessName,
                                                                .BPRStartDate = t.BPRStartDate,
                                                                .BPREndDate = t.BPREndDate,
                                                                .BPRRunning = t.BPRRunning,
                                                                .BPHHadErrors = t.BPHHadErrors,
                                                                .BPHErrMsg = t.BPHErrMsg,
                                                                .BPHErrTitle = t.BPHErrTitle,
                                                                .BPRUpdated = t.BPRUpdated.ToArray()}).FirstOrDefault()
                Return BatchProcessing
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblBatchProcessRunnings(ByVal UserName As String,
                                               Optional ByVal page As Integer = 1,
                                            Optional ByVal pagesize As Integer = 1000) As DTO.tblBatchProcessRunning()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                Dim data =
                    From t In db.tblBatchProcessRunnings
                    Where
                    t.BPRUserName.Contains(UserName) Select t


                intRecordCount = data.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim BatchProcessing() As DTO.tblBatchProcessRunning = (
                    From d In data Order By d.BPRControl Descending
                    Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return BatchProcessing
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblBatchProcessRunningsByTitleGroup(ByVal UserName As String,
                                                           ByVal Title As String,
                                               Optional ByVal page As Integer = 1,
                                            Optional ByVal pagesize As Integer = 1000) As DTO.tblBatchProcessRunning()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                Dim data =
                    From t In db.tblBatchProcessRunnings
                    Where
                    t.BPRUserName.Contains(UserName) _
                    And t.BPHErrTitle.Contains(Title) Select t


                intRecordCount = data.Count
                If intRecordCount < 1 Then Return Nothing
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                Dim BatchProcessing() As DTO.tblBatchProcessRunning = (
                    From d In data Order By d.BPRControl Descending
                    Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()

                Return BatchProcessing
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="ProcessName"></param>
    ''' <param name="throwError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where InvalidOperationException throws FaultException
    '''     we now test  If BatchProcessing IsNot Nothing AndAlso BatchProcessing.BPRControl not equal 0
    ''' </remarks>
    Public Function IsBatchProcessRunning(ByVal UserName As String, ByVal ProcessName As String, Optional ByVal throwError As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Try
            'if there are no null values to deal with we can build the array directly
            Dim BatchProcessing As DTO.tblBatchProcessRunning = GettblBatchProcessRunning(UserName, ProcessName)
            ' Modified by RHR for v-8.2 on 02/19/2021 
            If BatchProcessing IsNot Nothing AndAlso BatchProcessing.BPRControl <> 0 Then
                If BatchProcessing.BPRRunning = True Then
                    If BatchProcessing.BPHHadErrors Then
                        Dim strErrMsg As String = BatchProcessing.BPHErrMsg
                        Dim strErrTitle As String = BatchProcessing.BPHErrTitle
                        'mark the record as no longer running 
                        '(this normally will be handedle by the batch processor we had code here to clean up any unexpected problem.)
                        Try
                            BatchProcessing.BPRRunning = False
                            BatchProcessing.BPREndDate = Date.Now
                            UpdatetblBatchProcessRunning(BatchProcessing)
                        Catch ex As Exception
                            'if any errors occurr save the data and thow the error back to the caller
                            Utilities.SaveAppError("Multiple Errors Encountered: " & strErrMsg & ": " & ex.Message, Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_BatchProcessError"}, New FaultReason(strErrTitle))
                        End Try
                        'just send the batch processing error back to the caller
                        If throwError Then
                            Utilities.SaveAppError(strErrMsg, Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_BatchProcessError"}, New FaultReason(strErrTitle))
                        End If
                    Else
                        blnRet = True
                    End If
                Else
                    blnRet = False
                    'check for errors
                    If BatchProcessing.BPHHadErrors And throwError Then
                        Utilities.SaveAppError(BatchProcessing.BPHErrMsg, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_BatchProcessError"}, New FaultReason(BatchProcessing.BPHErrTitle))

                    End If
                End If
            End If
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As FaultException(Of SqlFaultInfo)
            If ex.Detail.Message = "E_NoData" Then
                'if we get here if no records are found so just return false because the process is not running
                blnRet = False
            Else
                Throw
            End If

        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="ProcessName"></param>
    ''' <param name="title"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where InvalidOperationException throws FaultException
    '''     we now test  If BatchProcessing IsNot Nothing AndAlso BatchProcessing.BPRControl not equal 0
    '''     we now CreatetblBatchProcessRunning(BatchProcessing) if nothing
    ''' </remarks>
    Public Sub StarttblBatchProcessRunning(ByVal UserName As String, ByVal ProcessName As String, Optional ByVal title As String = "")
        Try
            'check if this user has run the batch process before
            Dim BatchProcessing As DTO.tblBatchProcessRunning = GettblBatchProcessRunning(UserName, ProcessName)
            If BatchProcessing IsNot Nothing AndAlso BatchProcessing.BPRControl <> 0 Then
                With BatchProcessing
                    .BPRRunning = True
                    .BPRStartDate = Date.Now
                    .BPREndDate = Nothing
                    .BPHHadErrors = False
                    .BPHErrMsg = ""
                    .BPHErrTitle = title
                End With
                UpdatetblBatchProcessRunning(BatchProcessing)
            Else
                BatchProcessing = New DTO.tblBatchProcessRunning()
                With BatchProcessing
                    .BPRUserName = UserName
                    .BPRProcessName = ProcessName
                    BatchProcessing.BPRRunning = True
                    BatchProcessing.BPRStartDate = Date.Now
                    BatchProcessing.BPREndDate = Nothing
                End With
                CreatetblBatchProcessRunning(BatchProcessing)
            End If
        Catch ex As FaultException(Of SqlFaultInfo)
            If ex.Detail.Message = "E_NoData" Then
                Dim BatchProcessing As New DTO.tblBatchProcessRunning
                With BatchProcessing
                    .BPRUserName = UserName
                    .BPRProcessName = ProcessName
                    .BPRRunning = True
                    .BPRStartDate = Date.Now
                    .BPREndDate = Nothing
                    .BPHHadErrors = False
                    .BPHErrMsg = ""
                    .BPHErrTitle = title
                End With
                CreatetblBatchProcessRunning(BatchProcessing)
            End If
        Catch ex As System.Data.SqlClient.SqlException
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            'If we get here if no records are found so we must create one
            Dim BatchProcessing As New DTO.tblBatchProcessRunning
            With BatchProcessing
                .BPRUserName = UserName
                .BPRProcessName = ProcessName
                BatchProcessing.BPRRunning = True
                BatchProcessing.BPRStartDate = Date.Now
                BatchProcessing.BPREndDate = Nothing
            End With
            CreatetblBatchProcessRunning(BatchProcessing)
        Catch ex As Exception
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="ProcessName"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where InvalidOperationException throws FaultException
    '''     we now test  If BatchProcessing IsNot Nothing AndAlso BatchProcessing.BPRControl not equal 0
    '''     we now CreatetblBatchProcessRunning(BatchProcessing) if nothing
    ''' </remarks>
    Public Sub EndtblBatchProcessRunning(ByVal UserName As String, ByVal ProcessName As String)
        Try
            'check if this user has run the batch process before
            Dim BatchProcessing As DTO.tblBatchProcessRunning = GettblBatchProcessRunning(UserName, ProcessName)
            If BatchProcessing IsNot Nothing AndAlso BatchProcessing.BPRControl <> 0 Then
                BatchProcessing.BPRRunning = False
                BatchProcessing.BPREndDate = Date.Now
                UpdatetblBatchProcessRunning(BatchProcessing)
            Else
                BatchProcessing = New DTO.tblBatchProcessRunning()
                With BatchProcessing
                    .BPRUserName = UserName
                    .BPRProcessName = ProcessName
                    BatchProcessing.BPRRunning = False
                    'Because the start date was lost we use the current date
                    BatchProcessing.BPRStartDate = Date.Now
                    BatchProcessing.BPREndDate = Date.Now
                End With
                CreatetblBatchProcessRunning(BatchProcessing)
            End If
        Catch ex As System.Data.SqlClient.SqlException
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            'We get here if no records are found so we must create one
            Dim BatchProcessing As New DTO.tblBatchProcessRunning
            With BatchProcessing
                .BPRUserName = UserName
                .BPRProcessName = ProcessName
                BatchProcessing.BPRRunning = False
                'Because the start date was lost we use the current date
                BatchProcessing.BPRStartDate = Date.Now
                BatchProcessing.BPREndDate = Date.Now
            End With
            CreatetblBatchProcessRunning(BatchProcessing)
        Catch ex As Exception
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
    End Sub

    Protected Sub CreatetblBatchProcessRunning(ByVal oData As DTO.tblBatchProcessRunning)

        Using db As New NGLMASSYSDataContext(ConnectionString)
            'Check if the record exists
            Try

                Dim oExists As DTO.tblBatchProcessRunning = (
                    From t In db.tblBatchProcessRunnings
                    Where
                    t.BPRUserName.Contains(oData.BPRUserName) _
                    And
                    t.BPRProcessName.Contains(oData.BPRProcessName)
                    Select New DTO.tblBatchProcessRunning With {.BPRControl = t.BPRControl,
                                                                .BPRUserName = t.BPRUserName,
                                                                .BPRProcessName = t.BPRProcessName}).First

                If Not oExists Is Nothing Then
                    'the record already exists so just perform an update
                    UpdatetblBatchProcessRunning(oData)
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result (no records).
            End Try
            'Create New Record
            Dim oNewRecord As LTS.tblBatchProcessRunning = New LTS.tblBatchProcessRunning With {.BPRControl = oData.BPRControl,
                                                             .BPRUserName = oData.BPRUserName,
                                                             .BPRProcessName = oData.BPRProcessName,
                                                             .BPRStartDate = oData.BPRStartDate,
                                                             .BPREndDate = oData.BPREndDate,
                                                             .BPRRunning = oData.BPRRunning,
                                                             .BPHHadErrors = oData.BPHHadErrors,
                                                             .BPHErrMsg = oData.BPHErrMsg,
                                                             .BPHErrTitle = oData.BPHErrTitle,
                                                             .BPRUpdated = If(oData.BPRUpdated Is Nothing, New Byte() {}, oData.BPRUpdated)}

            db.tblBatchProcessRunnings.InsertOnSubmit(oNewRecord)
            Try
                db.SubmitChanges()

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using

    End Sub

    Protected Sub UpdatetblBatchProcessRunning(ByVal oData As DTO.tblBatchProcessRunning)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            ' Attach the order
            Dim oRecord As LTS.tblBatchProcessRunning = New LTS.tblBatchProcessRunning With {.BPRControl = oData.BPRControl,
                                                             .BPRUserName = oData.BPRUserName,
                                                             .BPRProcessName = oData.BPRProcessName,
                                                             .BPRStartDate = oData.BPRStartDate,
                                                             .BPREndDate = oData.BPREndDate,
                                                             .BPRRunning = oData.BPRRunning,
                                                             .BPHHadErrors = oData.BPHHadErrors,
                                                             .BPHErrMsg = oData.BPHErrMsg,
                                                             .BPHErrTitle = oData.BPHErrTitle,
                                                             .BPRUpdated = oData.BPRUpdated}
            db.tblBatchProcessRunnings.Attach(oRecord, True)

            Try
                db.SubmitChanges()
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                'In the case of the tblBatchProcessing if we try to perform an update when nothing has 
                'changed we get a change conflict exception.  We can ignore these types of exceptions               
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="ProcessName"></param>
    ''' <param name="ErrMsg"></param>
    ''' <param name="ErrTitle"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 02/19/2021 
    '''     fixed bug where InvalidOperationException throws FaultException
    '''     we now test  If BatchProcessing IsNot Nothing AndAlso BatchProcessing.BPRControl not equal 0
    '''     we now CreatetblBatchProcessRunning(BatchProcessing) if nothing
    ''' </remarks>
    Public Sub EndtblBatchProcessRunningWithError(ByVal UserName As String, ByVal ProcessName As String, ByVal ErrMsg As String, ByVal ErrTitle As String)
        Try
            'if there are no null values to deal with we can build the array directly
            Dim BatchProcessing As DTO.tblBatchProcessRunning = GettblBatchProcessRunning(UserName, ProcessName)
            If BatchProcessing Is Nothing OrElse BatchProcessing.BPRControl = 0 Then
                BatchProcessing = New DTO.tblBatchProcessRunning
                With BatchProcessing
                    .BPRUserName = UserName
                    .BPRProcessName = ProcessName
                    .BPRRunning = False
                    .BPRStartDate = Date.Now
                    .BPREndDate = Date.Now
                    .BPHHadErrors = True
                    .BPHErrMsg = ErrMsg
                    .BPHErrTitle = ErrTitle
                End With
                CreatetblBatchProcessRunning(BatchProcessing)
            Else
                With BatchProcessing
                    .BPRRunning = False
                    .BPREndDate = Date.Now
                    .BPHHadErrors = True
                    .BPHErrMsg = ErrMsg
                    .BPHErrTitle = ErrTitle
                End With
                UpdatetblBatchProcessRunning(BatchProcessing)
            End If
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            'If we get here if no records are found so we must create one
            Dim BatchProcessing As New DTO.tblBatchProcessRunning
            With BatchProcessing
                .BPRUserName = UserName
                .BPRProcessName = ProcessName
                .BPRRunning = False
                .BPRStartDate = Date.Now
                .BPREndDate = Date.Now
                .BPHHadErrors = True
                .BPHErrMsg = ErrMsg
                .BPHErrTitle = ErrTitle
            End With
            CreatetblBatchProcessRunning(BatchProcessing)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
    End Sub

    Friend Function selectDTOData(ByVal d As LTS.tblBatchProcessRunning, ByRef db As NGLMASSYSDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblBatchProcessRunning
        Return New DTO.tblBatchProcessRunning With {.BPRControl = d.BPRControl,
                                                                .BPRUserName = d.BPRUserName,
                                                                .BPRProcessName = d.BPRProcessName,
                                                                .BPRStartDate = d.BPRStartDate,
                                                                .BPREndDate = d.BPREndDate,
                                                                .BPRRunning = d.BPRRunning,
                                                                .BPHHadErrors = d.BPHHadErrors,
                                                                .BPHErrMsg = d.BPHErrMsg,
                                                                .BPHErrTitle = d.BPHErrTitle,
                                                                .BPRUpdated = d.BPRUpdated.ToArray(),
                                                   .Page = page,
                                                   .Pages = pagecount,
                                                   .RecordCount = recordcount,
                                                   .PageSize = pagesize}
    End Function

#End Region

#Region " About"

    Public Function GetvAbout() As DTO.vAbout
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim About As DTO.vAbout = (
                    From t In db.vAbouts
                    Select New DTO.vAbout With {.AuthName = t.AuthName _
                                                , .AuthKey = t.AuthKey _
                                                , .AuthAddress = t.AuthAddress _
                                               , .AuthCity = t.AuthCity _
                                               , .AuthNumber = t.AuthNumber _
                                               , .AuthState = t.AuthState _
                                               , .AuthZip = t.AuthZip _
                                               , .claimversion = t.claimversion _
                                               , .masversion = t.masversion _
                                               , .ServerLastMod = t.ServerLastMod _
                                               , .version = t.version _
                                               , .xactversion = t.xactversion}).First
                Return About
            Catch ex As System.Data.SqlClient.SqlException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Splits the string returned from the Version table into an array and returns the
    ''' first integer 
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMajorVersionRelease() As Integer
        Dim intReturn As Integer = 0
        Dim a = GetvAbout()
        'Dim vArray = a.version.Split()
        Dim vArray = a.version.ToCharArray()

        For i = 0 To vArray.Length - 1
            If Integer.TryParse(vArray(i).ToString(), intReturn) Then
                Exit For
            End If
        Next

        Return intReturn
    End Function

#End Region

#Region "ExportDoc"

    Public Function ExecuteExportDoc(ByVal bookControl As Integer) As DTO.ExportDocResult()

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim result() As DTO.ExportDocResult = Nothing
                Dim spresult As List(Of LTS.spGenerateExportDocResult) = db.spGenerateExportDoc(bookControl).ToList
                Dim tempArray As New List(Of DTO.ExportDocResult)

                For Each t In spresult
                    Dim newItem As New DTO.ExportDocResult()
                    newItem.ASSIST = t.ASSIST
                    newItem.BUYERSBN = t.BUYERSBN
                    newItem.COM_TERMS = t.COM_TERMS
                    newItem.COMMENTS1 = t.COMMENTS1
                    newItem.COMMENTS2 = t.COMMENTS2
                    newItem.COMMISSION = t.COMMISSION
                    newItem.CONTAINER = t.CONTAINER
                    newItem.CUBE = t.CUBE
                    newItem.CUBEUM = t.CUBEUM
                    newItem.CUPOSTCODE = t.CUPOSTCODE
                    newItem.CURR_CODE = t.CURR_CODE
                    newItem.CURRRATE = t.CURRRATE
                    newItem.CUST_2 = t.CUST_2
                    newItem.CUSTADDRS1 = t.CUSTADDRS1
                    newItem.CUSTADDRS2 = t.CUSTADDRS2
                    newItem.CUSTADDRS3 = t.CUSTADDRS3
                    newItem.CUSTADDRS4 = t.CUSTADDRS4
                    newItem.CUSTNAME = t.CUSTNAME
                    newItem.CUSTOMER = t.CUSTOMER
                    newItem.CUSTORDLN = t.CUSTORDLN
                    newItem.CUSTORDNUM = t.CUSTORDNUM
                    newItem.DESC = t.DESC
                    newItem.DETAILS1 = t.DETAILS1
                    newItem.DETAILS2 = t.DETAILS2
                    newItem.DOMESTIC = t.DOMESTIC
                    newItem.ENTRYDATE = t.ENTRYDATE
                    newItem.EXPADDRS1 = t.EXPADDRS1
                    newItem.EXPADDRS2 = t.EXPADDRS2
                    newItem.EXPADDRS3 = t.EXPADDRS3
                    newItem.EXPADDRS4 = t.EXPADDRS4
                    newItem.ExpContact = t.ExpContact
                    newItem.ExpFax = t.ExpFax
                    newItem.EXPORT_CAR = t.EXPORT_CAR
                    newItem.EXPORTER = t.EXPORTER
                    newItem.ExpTel = t.ExpTel
                    newItem.ExpZip = t.ExpZip
                    newItem.FACILITY = t.FACILITY
                    newItem.FOB_POINT = t.FOB_POINT
                    newItem.HSCODE = t.HSCODE
                    newItem.INCO_TERMS = t.INCO_TERMS
                    newItem.INSURANCE = t.INSURANCE
                    newItem.INV_DATE = t.INV_DATE
                    newItem.INV_NUMBER = t.INV_NUMBER
                    newItem.INVOICE = t.INVOICE
                    newItem.INVOICEAMT = t.INVOICEAMT
                    newItem.ISONUMBER = t.ISONUMBER
                    newItem.ITEM = t.ITEM
                    newItem.ITEMCOST = t.ITEMCOST
                    newItem.ITEMREV = t.ITEMREV
                    newItem.LINE_NO = t.LINE_NO
                    newItem.LOCAL_CAR = t.LOCAL_CAR
                    newItem.MISC = t.MISC
                    newItem.MODE = t.MODE
                    newItem.NO_LINES = t.NO_LINES
                    newItem.OCEAN = t.OCEAN
                    newItem.PACKING = t.PACKING
                    newItem.PORT_ENTRY = t.PORT_ENTRY
                    newItem.PRIC_DEC = t.PRIC_DEC
                    newItem.QNTY_DEC = t.QNTY_DEC
                    newItem.QNTYSHP = t.QNTYSHP
                    newItem.QUANTITY = t.QUANTITY
                    newItem.REFERENCE = t.REFERENCE
                    newItem.SHIP = t.SHIP
                    newItem.SHIPADDRS1 = t.SHIPADDRS1
                    newItem.SHIPADDRS2 = t.SHIPADDRS2
                    newItem.SHIPADDRS3 = t.SHIPADDRS3
                    newItem.SHIPADDRS4 = t.SHIPADDRS4
                    newItem.SHIPNAME = t.SHIPNAME
                    newItem.SHIPPERSBN = t.SHIPPERSBN
                    newItem.SHPOSTCODE = t.SHPOSTCODE
                    newItem.SHPUM = t.SHPUM
                    newItem.TAX1_CHRG = t.TAX1_CHRG
                    newItem.TAX2_CHRG = t.TAX2_CHRG
                    newItem.TAXTYPE1 = t.TAXTYPE1
                    newItem.TAXTYPE2 = t.TAXTYPE2
                    newItem.WEIGHT = t.WEIGHT
                    newItem.WGT_DEC = t.WGT_DEC
                    newItem.WTUM = t.WTUM
                    tempArray.Add(newItem)
                Next
                result = tempArray.ToArray()


                Return result
            Catch ex As System.Data.SqlClient.SqlException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using

    End Function

#End Region

End Class

Public Class NGLEmailData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.Mails
        Me.LinqDB = db
        Me.SourceClass = "NGLEmailData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            Me.LinqTable = db.Mails
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"
    Public Function SelectNew(ByVal d As LTS.Mail) As DTO.Mail
        Return (New DTO.Mail With {.MailControl = d.MailControl _
                                  , .Body = If(d.Body IsNot Nothing, d.Body.Replace(vbCr, "  ").Replace(vbLf, "  "), "") _
                                  , .DateAdded = d.DateAdded _
                                  , .DateSent = d.DateSent _
                                  , .MailCc = d.MailCc _
                                  , .MailFrom = d.MailFrom _
                                  , .MailTo = d.MailTo _
                                  , .ReadyToSend = d.ReadyToSend _
                                  , .Result = If(d.Result IsNot Nothing, d.Result.Replace(vbCr, "  ").Replace(vbLf, "  "), "") _
                                  , .Subject = d.Subject _
                                  , .SubjectLocalized = d.SubjectLocalized _
                                  , .BodyLocalized = d.BodyLocalized _
                                  , .SubjectKeys = d.SubjectKeys _
                                  , .BodyKeys = d.BodyKeys})

    End Function
    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetMailsFiltered()
    End Function

    Public Function GetMailFiltered(ByVal control As Integer) As DTO.Mail
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim mail As DTO.Mail = (
                From t In db.Mails
                Where t.MailControl = control
                Select SelectNew(t)).FirstOrDefault

                Return mail

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetMailsFiltered(Optional ByVal Count As Integer = 1000) As DTO.Mail()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'db.Log = New DebugTextWriter
                If Count = 0 Then Count = 1000
                'Return all the records that match the criteria sorted by parkey
                Dim mail() As DTO.Mail = (
                From t In db.Mails
                Order By t.DateAdded Descending
                Select SelectNew(t)).Take(Count).ToArray

                Return mail

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetMailsFailedFiltered(Optional ByVal Count As Integer = 1000) As DTO.Mail()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                If Count = 0 Then Count = 1000
                'Return all the records that match the criteria sorted by parkey
                Dim mail() As DTO.Mail = (
                From t In db.Mails
                Where Not t.Result = Nothing
                Order By t.DateAdded Descending
                Select SelectNew(t)).Take(Count).ToArray

                Return mail

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' GetMailsFiltered705110
    ''' </summary>
    ''' <param name="alreadySent"></param>
    ''' <param name="blnOnlyErrors"></param>
    ''' <param name="refIdType"></param>
    ''' <param name="refID"></param>
    ''' <param name="datefilterType"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/15/16 for v-7.0.5.110 Enhance Mail Table
    ''' Modified by RHR v-7.0.5.110 8/24/2016
    '''   added additional filters and paging support
    '''   added skip and take parameters to support
    '''   interface with TMS 365 pages
    ''' NOTE:  Ready to Send vs Null SentDate logic not supported
    '''        New code is needed to show mail that has not been sent
    ''' </remarks>
    Public Function GetMailsFiltered705110(ByVal alreadySent As Boolean,
                                              ByVal blnOnlyErrors As Boolean,
                                              ByVal refIdType As Integer?,
                                              ByVal refID As String,
                                              Optional ByVal datefilterType As Utilities.NGLDateFilterType = Utilities.NGLDateFilterType.None,
                                              Optional ByVal StartDate As DateTime? = Nothing,
                                              Optional ByVal EndDate As DateTime? = Nothing,
                                              Optional ByVal page As Integer = 1,
                                              Optional ByVal pagesize As Integer = 1000,
                                              Optional ByVal skip As Integer = 0,
                                              Optional ByVal take As Integer = 0) As DTO.Mail()
        '4 filters
        '   show only errors (as is now)
        'show by date range (either date sent or date added)
        'show by key fields (new fields)
        '   readytoSend checkbox
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                If datefilterType <> Utilities.NGLDateFilterType.None Then
                    If StartDate.HasValue And EndDate.HasValue Then
                        StartDate = DTran.formatStartDateFilter(StartDate.Value)
                        EndDate = DTran.formatEndDateFilter(EndDate.Value)
                    Else
                        'disable the date filter if the dates are null
                        datefilterType = Utilities.NGLDateFilterType.None
                    End If
                End If
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1

                Dim oQuery = From t In db.Mails
                             Where
                                      (
                                       (alreadySent = False And Not t.DateSent.HasValue) _
                                       Or
                                       (alreadySent = True And t.DateSent.HasValue)
                                      ) _
                                  And
                                     (
                                         (blnOnlyErrors = True And If(t.Result, "").Trim().Length() > 0) _
                                         Or
                                         blnOnlyErrors = False
                                     ) _
                                 And
                                     (
                                         datefilterType = Utilities.NGLDateFilterType.None _
                                         Or
                                         (
                                         (datefilterType = Utilities.NGLDateFilterType.DateAdded And t.DateAdded >= StartDate AndAlso t.DateAdded <= EndDate) _
                                         Or
                                         (datefilterType = Utilities.NGLDateFilterType.DateSent And t.DateSent.HasValue AndAlso t.DateSent >= StartDate AndAlso t.DateSent <= EndDate)
                                         )
                                     ) _
                                 And
                                     (
                                      (If(refIdType, 0) = 0) _
                                      Or
                                          (String.IsNullOrWhiteSpace(refID) = True) _
                                      Or
                                          (
                                              (If([refIdType], 0) = If(t.RefIDType, 0)) _
                                              And
                                              String.Equals(refID.ToUpper, t.RefID.ToUpper)
                                          )
                                     )
                             Order By t.DateAdded Descending
                             Select t

                intRecordCount = oQuery.Count()
                If intRecordCount < 1 Then Return Nothing

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If intRecordCount < 1 Then intRecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1

                Dim mail() As DTO.Mail = (From d In oQuery
                                          Order By d.DateAdded Descending
                                          Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)
                           ).Skip(skip).Take(pagesize).ToArray()


                Return mail

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMailsFiltered705110"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' 'Calls spGenerateEmail70 which uses the GlobalCheckForNGLEmailDuplicates parameter
    '''  to check the number of minutes the system should check for duplicate email messages 
    '''  before resending and email to the same people with the same subject and body. the 
    '''  Localized values and keys are optional.  The caller should catch sqlfaultinfo with
    '''  a message of E_SQLExceptionMSG and log or rethrow the error that an email could not
    '''  be generated.
    ''' </summary>
    ''' <param name="MailFrom"></param>
    ''' <param name="EmailTo"></param>
    ''' <param name="CCEmail"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="SubjectLocalized"></param>
    ''' <param name="BodyLocalized"></param>
    ''' <param name="SubjectKeys"></param>
    ''' <param name="BodyKeys"></param>
    ''' <remarks></remarks>
    Public Sub GenerateEmail(ByVal MailFrom As String,
                                    ByVal EmailTo As String,
                                    ByVal CCEmail As String,
                                    ByVal Subject As String,
                                    ByVal Body As String,
                                    ByVal SubjectLocalized As String,
                                    ByVal BodyLocalized As String,
                                    ByVal SubjectKeys As String,
                                    ByVal BodyKeys As String)

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim oret = (db.spGenerateEmail70(MailFrom, EmailTo, CCEmail, Subject, Body, SubjectLocalized, BodyLocalized, SubjectKeys, BodyKeys, Me.Parameters.UserName)).FirstOrDefault()
                If Not oret Is Nothing Then If oret.ErrNumber <> 0 Then throwSQLFaultException(oret.RetMsg)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GenerateEmail70"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Overload method which does not require the parameter MailFrom
    ''' and calls spGenerateEmailFromSystem
    ''' The only difference is that this sp looks up a GlobalFromEmail
    ''' to send from
    ''' </summary>
    ''' <param name="EmailTo"></param>
    ''' <param name="CCEmail"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <param name="SubjectLocalized"></param>
    ''' <param name="BodyLocalized"></param>
    ''' <param name="SubjectKeys"></param>
    ''' <param name="BodyKeys"></param>
    ''' <remarks></remarks>
    Public Sub GenerateEmail(ByVal EmailTo As String,
                                    ByVal CCEmail As String,
                                    ByVal Subject As String,
                                    ByVal Body As String,
                                    ByVal SubjectLocalized As String,
                                    ByVal BodyLocalized As String,
                                    ByVal SubjectKeys As String,
                                    ByVal BodyKeys As String)

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim oret = (db.spGenerateEmailFromSystem(EmailTo, CCEmail, Subject, Body, SubjectLocalized, BodyLocalized, SubjectKeys, BodyKeys, Me.Parameters.UserName)).FirstOrDefault()
                If Not oret Is Nothing Then If oret.ErrNumber <> 0 Then throwSQLFaultException(oret.RetMsg)

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GenerateEmailFromSystem"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Wrapper method for spGenerateEmailFromSystem sends to GlobalAdminEmail
    ''' </summary>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.0.001 on 10/08/2021 
    '''   added support for blank Send To Emails the procedure now uses GlobalAdminEmail parameter
    '''   Localization is ignored
    ''' </remarks>
    Public Sub GenerateAdminEmail(ByVal Subject As String, ByVal Body As String)

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim oret = (db.spGenerateEmailFromSystem("", "", Subject, Body, "", "", "", "", Me.Parameters.UserName)).FirstOrDefault()
            Catch ex As Exception
                'ignore any errors on admin email we are typically in an error state so just exit
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Resets the Mail Send Status by MailControl to either
    ''' Resend the Mail (blnMarkAsSent = False) 
    ''' or Mark the mail as Sent (if blnMarkAsSent = True)
    ''' </summary>
    ''' <param name="MailControl"></param>
    ''' <param name="blnMarkAsSent"></param>
    ''' <remarks>
    ''' Added By LVV on 8/30/16 for v-7.0.5.110 Enhance Mail Table
    ''' </remarks>
    Public Sub UpdateMailSendStatus(ByVal MailControl As Integer,
                                    ByVal blnMarkAsSent As Boolean)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim dtNow = Date.Now
                Try
                    Dim oLTS = (From d In db.Mails Where d.MailControl = MailControl).FirstOrDefault()
                    If Not oLTS Is Nothing AndAlso oLTS.MailControl > 0 Then
                        With oLTS

                            If blnMarkAsSent Then
                                .DateSent = dtNow
                            Else
                                .DateSent = Nothing
                                .ReadyToSend = 1
                            End If

                        End With

                        db.SubmitChanges()
                    End If
                Catch ex As Exception
                    'Ignore errors when updating
                End Try

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateMailSendStatus"))
            End Try

        End Using
    End Sub

    ''' <summary>
    ''' Resets the Mail Send Status by MailControl to
    ''' update the fields in the record and
    ''' Resend the Mail (blnMarkAsSent = False) 
    ''' </summary>
    ''' <param name="MailControl"></param>
    ''' <param name="SendTo"></param>
    ''' <param name="Cc"></param>
    ''' <param name="Subject"></param>
    ''' <param name="Body"></param>
    ''' <remarks>
    ''' Added By LVV on 8/31/16 for v-7.0.5.110 Enhance Mail Table
    ''' </remarks>
    Public Sub EditMail(ByVal MailControl As Integer,
                        ByVal SendTo As String,
                        ByVal Cc As String,
                        ByVal Subject As String,
                        ByVal Body As String)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Try
                    Dim oLTS = (From d In db.Mails Where d.MailControl = MailControl).FirstOrDefault()
                    If Not oLTS Is Nothing AndAlso oLTS.MailControl > 0 Then
                        With oLTS
                            .MailTo = SendTo
                            .MailCc = Cc
                            .Subject = Subject
                            .Body = Body
                            'set to resend
                            .DateSent = Nothing
                            .ReadyToSend = 1
                        End With

                        db.SubmitChanges()
                    End If
                Catch ex As Exception
                    'Ignore errors when updating
                End Try

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("EditMail"))
            End Try

        End Using
    End Sub

#Region "LTS Methods"


    ''' <summary>
    ''' Get an array of filtered mail log records
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 12/30/2021 for New Mail Manager Logic
    ''' </remarks>
    Public Function getMailLog(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.Mail()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.Mail
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.Mail)
                iQuery = db.Mails
                Dim filterWhere = ""
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "MailControl"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getMailLog"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function DeleteMail(ByVal iMailControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iMailControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'verify the service contract
                Dim oExisting = db.Mails.Where(Function(x) x.MailControl = iMailControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.MailControl = 0 Then Return True
                db.Mails.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteMail"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function UpdateMail(ByVal oData As LTS.Mail) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                If oData Is Nothing Then throwNoDataFaultException()
                Dim iMailControl As Integer = oData.MailControl
                If iMailControl = 0 Then throwNoDataFaultException()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateMail"), db)
            End Try
        End Using
        EditMail(oData.MailControl, oData.MailTo, oData.MailCc, oData.Subject, oData.Body)
        Return blnRet
    End Function


#End Region


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.Mail)
        'Create(New Record)
        Return New LTS.Mail With {.MailControl = d.MailControl _
                                 , .Body = d.Body _
                                 , .DateAdded = d.DateAdded _
                                 , .DateSent = d.DateSent _
                                 , .MailCc = d.MailCc _
                                 , .MailFrom = d.MailFrom _
                                 , .MailTo = d.MailTo _
                                 , .ReadyToSend = d.ReadyToSend _
                                 , .Result = d.Result _
                                 , .Subject = d.Subject _
                                 , .SubjectLocalized = d.SubjectLocalized _
                                 , .BodyLocalized = d.BodyLocalized _
                                 , .SubjectKeys = d.SubjectKeys _
                                 , .BodyKeys = d.BodyKeys _
                                 , .RefIDType = d.RefIDType _
                                 , .RefID = d.RefID}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetMailFiltered(control:=CType(LinqTable, LTS.Mail).MailControl)
    End Function

#End Region

#Region "Shared Methods"

    'Modified by RHR v-7.0.5.110 8/24/2016 Enhance Mail Table
    Friend Shared Function selectDTOData(ByVal d As LTS.Mail, ByVal db As NGLMASSYSDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.Mail
        Dim oDTO As New DTO.Mail
        'the original Carrier DTO object was not designed to support NULL integers
        'so we need to process those values using skipObjs
        Dim skipObjs As New List(Of String) From {"Page",
                                                  "Pages",
                                                  "RecordCount",
                                                  "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With

        Return oDTO

    End Function


#End Region


End Class

Public Class NGLParameterData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.Parameters
        Me.LinqDB = db
        Me.SourceClass = "NGLParameterData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If (_LinqTable Is Nothing) Then
                Dim db As New NGLMASSYSDataContext(ConnectionString)
                _LinqTable = db.Parameters
                _LinqDB = db
            End If
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetParametersFiltered()
    End Function

    Public Function GetPCMParameters(Optional ByVal CompControl As Integer = 0) As LTS.spGetPCMParametersResult
        Dim oRet = New LTS.spGetPCMParametersResult
        Try
            Using db As New NGLMASSYSDataContext(ConnectionString)
                oRet = db.spGetPCMParameters(CompControl).FirstOrDefault()
            End Using
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetPCMParameters"))
        End Try
        Return oRet
    End Function

    Public Function GetParameterFiltered(ByVal ParKey As String) As DTO.Parameter
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                '**** Removed by RHR 06/13/11 we use the same logic as in GetParametersFiltered *****
                'Dim oDLO As New DataLoadOptions
                'oDLO.LoadWith(Of LTS.Parameter)(Function(t As LTS.Parameter) t.CompParameterRefSystems)
                'db.LoadOptions = oDLO
                '************************************************************************************

                'Get the newest record that matches the provided criteria
                Dim Parameter As DTO.Parameter = (
                From t In db.Parameters
                Join c In db.tblParCategories On t.ParCategoryControl Equals c.ParCatControl
                Where
                    (t.ParKey = ParKey)
                Select New DTO.Parameter With {.ParCategoryControl = t.ParCategoryControl _
                                        , .ParKey = t.ParKey _
                                        , .ParValue = If(t.ParValue.HasValue, t.ParValue.Value, 0) _
                                        , .ParText = t.ParText _
                                        , .ParDescription = t.ParDescription _
                                        , .ParUpdated = t.ParUpdated.ToArray() _
                                        , .ParCatName = c.ParCatName _
                                        , .ParIsGlobal = t.ParIsGlobal _
                                        , .CompParameters = (
                                                From d In t.CompParameterRefSystems
                                                Join p In db.CompRefSystems On d.CompParCompControl Equals p.CompControl
                                                Order By p.CompNumber
                                                Select New DTO.CompParameter With {.CompParControl = d.CompParControl _
                                                    , .CompParCompControl = d.CompParCompControl _
                                                    , .CompParCategoryControl = d.CompParCategoryControl _
                                                    , .CompParKey = d.CompParKey _
                                                    , .CompParValue = If(d.CompParValue.HasValue, d.CompParValue.Value, 0) _
                                                    , .CompParText = d.CompParText _
                                                    , .CompParDescription = d.CompParDescription _
                                                    , .CompNumber = p.CompNumber _
                                                    , .CompParUpdated = d.CompParUpdated.ToArray()}).ToList()}).First
                If Parameter.ParIsGlobal Then
                    Parameter.CompParameters = Nothing
                End If
                Return Parameter

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Overloads Function GetParText(ByVal ParKey As String) As String
        Dim strRet As String = ""
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Parameter As DTO.Parameter = (
                From t In db.Parameters
                Where
                    (t.ParKey = ParKey)
                Select New DTO.Parameter With {.ParText = t.ParText}).First

                Return Parameter.ParText


            Catch ex As Exception
                'Do nothing on error just return an empty string
            End Try

            Return strRet

        End Using
    End Function

    Public Overloads Function GetParValue(ByVal ParKey As String) As Double
        Dim dblRet As Double = 0
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim Parameter As DTO.Parameter = (
                From t In db.Parameters
                Where
                    (t.ParKey = ParKey)
                Select New DTO.Parameter With {.ParValue = t.ParValue}).First

                Return Parameter.ParValue


            Catch ex As Exception
                'Do nothing on error just return an empty string
            End Try

            Return dblRet

        End Using
    End Function

    ''' <summary>
    ''' Reads a Parameter Value for the master company assigned to the provided/users Legal Entity
    ''' </summary>
    ''' <param name="ParKey"></param>
    ''' <param name="LEControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.002 on 05/07/2021 get par val by LE
    ''' </remarks>
    Public Overloads Function GetParValueByLegalEntity(ByVal ParKey As String, ByVal LEControl As Integer) As Double
        Dim dblRet As Double = 0
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim iCompControl = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEControl).Select(Function(y) y.LEAdminCompControl).FirstOrDefault()
                Dim dblVal = db.udfgetParValueByCompControl(ParKey, iCompControl)

                dblRet = If(dblVal, 0)


            Catch ex As Exception
                'Do nothing on error just return zero
            End Try

            Return dblRet

        End Using
    End Function


    ''' <summary>
    ''' Reads a Parameter Text for the master company assigned to the provided/users Legal Entity
    ''' </summary>
    ''' <param name="ParKey"></param>
    ''' <param name="LEControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.002 on 05/07/2021 get par text by LE
    ''' </remarks>
    Public Overloads Function GetParTextByLegalEntity(ByVal ParKey As String, ByVal LEControl As Integer) As String
        Dim strRet As String = ""
        Using db As New NGLMASCompDataContext(ConnectionString)
            Try
                Dim iCompControl = db.tblLegalEntityAdmins.Where(Function(x) x.LEAdminControl = LEControl).Select(Function(y) y.LEAdminCompControl).FirstOrDefault()
                strRet = db.udfgetParTextByCompControl(ParKey, iCompControl)

            Catch ex As Exception
                'Do nothing on error just return an empty string
            End Try

            Return strRet

        End Using
    End Function

    Public Function GetParametersFiltered(Optional ByVal ParCategoryControl As Integer = 0) As DTO.Parameter()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'db.Log = New DebugTextWriter

                'Return all the records that match the criteria sorted by parkey
                Dim Parameters() As DTO.Parameter = (
                From t In db.Parameters
                Join c In db.tblParCategories On t.ParCategoryControl Equals c.ParCatControl
                Where
                    (ParCategoryControl = 0 OrElse t.ParCategoryControl = ParCategoryControl)
                Order By c.ParCatName, t.ParKey
                Select New DTO.Parameter With {.ParCategoryControl = t.ParCategoryControl _
                                        , .ParKey = t.ParKey _
                                        , .ParValue = If(t.ParValue.HasValue, t.ParValue.Value, 0) _
                                        , .ParText = t.ParText _
                                        , .ParDescription = t.ParDescription _
                                        , .ParUpdated = t.ParUpdated.ToArray() _
                                        , .ParCatName = c.ParCatName _
                                        , .ParIsGlobal = t.ParIsGlobal _
                                        , .CompParameters = (
                                                From d In t.CompParameterRefSystems
                                                Join p In db.CompRefSystems On d.CompParCompControl Equals p.CompControl
                                                Order By p.CompNumber
                                                Select New DTO.CompParameter With {.CompParControl = d.CompParControl _
                                                    , .CompParCompControl = d.CompParCompControl _
                                                    , .CompParCategoryControl = d.CompParCategoryControl _
                                                    , .CompParKey = d.CompParKey _
                                                    , .CompParValue = If(d.CompParValue.HasValue, d.CompParValue.Value, 0) _
                                                    , .CompParText = d.CompParText _
                                                    , .CompParDescription = d.CompParDescription _
                                                    , .CompNumber = p.CompNumber _
                                                    , .CompParUpdated = d.CompParUpdated.ToArray()}).ToList()}).ToArray()
                For Each oPar In Parameters
                    If oPar.ParIsGlobal Then
                        oPar.CompParameters = Nothing
                    End If
                Next
                Return Parameters

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetParameter(ByVal sKey As String) As LTS.Parameter
        Dim oRet As New LTS.Parameter
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                oRet = db.Parameters.Where(Function(x) x.ParKey = sKey).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetParameter"))
            End Try
        End Using
        Return oRet
    End Function


    Public Function GetParameters(ByVal f As Models.AllFilters, ByRef RecordCount As Integer) As LTS.Parameter()
        Dim oRet As LTS.Parameter()
        Dim orderByFunc As Func(Of LTS.Parameter, [Object]) = Nothing
        'Dim filterByFunc As Func(Of LTS.Parameter, [Object]) = Nothing
        Dim sParValue As String = ""
        Dim sParText As String = ""
        Dim sParDescription As String = ""
        Dim iParCategoryControl As Integer? = Nothing
        Dim bParIsGlobal As Boolean? = Nothing


        Dim sortOrdinal = "CNS Number"  'default value
        Dim sortDirection = "Descending"    'default value
        Dim blnSortAsc As Boolean = True
        Dim datefilterfield = ""
        Dim dateFilterFrom As Date? = Nothing
        Dim dateFilterTo As Date? = Nothing
        Dim bookSHID As String = Nothing
        Dim bookShipCarrierProNumber As String = Nothing
        Dim oQuery As IQueryable(Of LTS.Parameter) = Nothing
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                orderByFunc = Function(item) item.ParKey
                sortOrdinal = "ParKey"
                Dim skip As Integer = 1
                Dim take As Integer = 1
                'add filtering if necessary
                If (Not f Is Nothing) Then
                    skip = f.skip
                    take = f.take
                    If f.sortDirection = "desc" Then blnSortAsc = False
                    Select Case (f.sortName)
                        Case "ParValue"
                            orderByFunc = Function(item) item.ParValue
                        Case "ParText"
                            orderByFunc = Function(item) item.ParText
                        Case "ParDescription"
                            orderByFunc = Function(item) item.ParDescription
                        Case "ParCategoryControl"
                            orderByFunc = Function(item) item.ParCategoryControl
                        Case "ParIsGlobal"
                            orderByFunc = Function(item) item.ParIsGlobal
                    End Select
                    Select Case (f.filterName)
                        Case "ParKey"
                            oQuery = db.Parameters.Where(Function(p) p.ParKey.Contains(f.filterValue))
                            'If blnSortAsc Then
                            '    oQuery = db.Parameters.Where(Function(p) p.ParKey.Contains(f.filterValue)).OrderBy(orderByFunc)
                            'Else
                            '    oQuery = db.Parameters.Where(Function(p) p.ParKey.Contains(f.filterValue)).OrderByDescending(orderByFunc)
                            'End If
                        Case "ParValue"
                            Dim dVal As Double = 0
                            If (Double.TryParse(f.filterValue, dVal)) Then
                                oQuery = db.Parameters.Where(Function(p) p.ParValue = dVal)
                                'If blnSortAsc Then
                                '    oQuery = db.Parameters.Where(Function(p) p.ParValue = dVal).OrderBy(orderByFunc)
                                'Else
                                '    oQuery = db.Parameters.Where(Function(p) p.ParValue = dVal).OrderByDescending(orderByFunc)
                                'End If
                            Else
                                oQuery = db.Parameters
                                'If blnSortAsc Then
                                '    oQuery = db.Parameters.OrderBy(orderByFunc)
                                'Else
                                '    oQuery = db.Parameters.OrderByDescending(orderByFunc)
                                'End If

                            End If
                        Case "ParText"
                            oQuery = db.Parameters.Where(Function(p) p.ParText.Contains(f.filterValue))
                            'If blnSortAsc Then
                            '    oQuery = db.Parameters.Where(Function(p) p.ParText.Contains(f.filterValue)).OrderBy(orderByFunc)
                            'Else
                            '    oQuery = db.Parameters.Where(Function(p) p.ParText.Contains(f.filterValue)).OrderByDescending(orderByFunc)
                            'End If
                        Case "ParDescription"
                            oQuery = db.Parameters.Where(Function(p) p.ParDescription.Contains(f.filterValue))
                            'If blnSortAsc Then
                            '    oQuery = db.Parameters.Where(Function(p) p.ParDescription.Contains(f.filterValue)).OrderBy(orderByFunc)
                            'Else
                            '    oQuery = db.Parameters.Where(Function(p) p.ParDescription.Contains(f.filterValue)).OrderByDescending(orderByFunc)
                            'End If
                        Case "ParCategoryControl"
                            Dim iVal As Integer = 0
                            If (Integer.TryParse(f.filterValue, iVal)) Then
                                oQuery = db.Parameters.Where(Function(p) p.ParCategoryControl = iVal)
                                'If blnSortAsc Then
                                '    oQuery = db.Parameters.Where(Function(p) p.ParCategoryControl = iVal).OrderBy(orderByFunc)
                                'Else
                                '    oQuery = db.Parameters.Where(Function(p) p.ParCategoryControl = iVal).OrderByDescending(orderByFunc)
                                'End If
                            Else
                                oQuery = db.Parameters
                                'If blnSortAsc Then
                                '    oQuery = db.Parameters.OrderBy(orderByFunc)
                                'Else
                                '    oQuery = db.Parameters.OrderByDescending(orderByFunc)
                                'End If
                            End If
                        Case "ParIsGlobal"
                            Dim bVal As Boolean = True
                            Boolean.TryParse(f.filterValue, bVal)
                            oQuery = db.Parameters.Where(Function(p) p.ParIsGlobal = bVal)
                            'If blnSortAsc Then
                            '    oQuery = db.Parameters.Where(Function(p) p.ParIsGlobal = bVal).OrderBy(orderByFunc)
                            'Else
                            '    oQuery = db.Parameters.Where(Function(p) p.ParIsGlobal = bVal).OrderByDescending(orderByFunc)
                            'End If
                        Case Else
                            oQuery = db.Parameters
                            'If blnSortAsc Then
                            '    oQuery = db.Parameters.OrderBy(orderByFunc)
                            'Else
                            '    oQuery = db.Parameters.OrderByDescending(orderByFunc)
                            'End If
                    End Select
                Else
                    oQuery = db.Parameters
                    'If blnSortAsc Then
                    '    oQuery = db.Parameters.OrderBy(orderByFunc)
                    'Else
                    '    oQuery = db.Parameters.OrderByDescending(orderByFunc)
                    'End If
                End If
                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing
                If take < 1 Then take = 1
                If skip < 1 Then skip = 1
                'oRet = oQuery.Select(Function(x) New LTS.Parameter With {.ParKey = x.ParKey, .msrepl_tran_version = x.msrepl_tran_version, .ParCategoryControl = x.ParCategoryControl, .ParDescription = x.ParDescription, .ParIsGlobal = x.ParIsGlobal, .ParOLE = x.ParOLE, .ParText = x.ParText, .ParUpdated = x.ParUpdated, .ParValue = x.ParValue, .rowguid = x.rowguid}).Skip(skip).Take(take).ToArray()
                If blnSortAsc Then
                    oRet = oQuery.OrderBy(orderByFunc).Skip(skip).Take(take).ToArray()
                Else
                    oRet = oQuery.OrderByDescending(orderByFunc).Skip(skip).Take(take).ToArray()
                End If
                'oRet = oQuery.Skip(skip).Take(take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetParameters"))
            End Try
        End Using



        Return oRet
    End Function


    Public Function GetParameters(ByRef RecordCount As Integer,
                                  Optional ByVal skip As Integer = 0,
                                  Optional ByVal take As Integer = 0) As LTS.Parameter()
        Dim oRet() As LTS.Parameter
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                RecordCount = db.Parameters.Count()
                If RecordCount < 1 Then Return Nothing
                If take < 1 Then take = 1
                If skip < 1 Then skip = 1
                oRet = db.Parameters.Skip(skip).Take(take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetParameters"))
            End Try
        End Using
        Return oRet
    End Function


    ''' <summary>
    ''' saves changes to the parameter record or insert a new one if the key does not exist
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 on 11/
    ''' </remarks>
    Public Function SaveParameter(ByVal oData As LTS.Parameter) As LTS.Parameter
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Dim blnInsert As Boolean = False
            Dim sKey As String = oData.ParKey
            If String.IsNullOrWhiteSpace(sKey) Then
                throwInvalidRequiredKeysException("Parameter", "Invalid parameter key, a key is required and cannot be blank")
            End If
            Try
                If db.Parameters.Any(Function(x) x.ParKey = sKey) Then
                    'update
                    'db.Parameters.Attach(oData, True)
                    db.SubmitChanges()
                Else
                    'insert so call the insert stored procedure (will create company parameter if required)
                    db.spAddNewParameter(oData.ParKey, oData.ParValue, oData.ParText, oData.ParDescription, oData.ParCategoryControl, oData.ParIsGlobal)
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveParameter"))
            End Try
        End Using

        Return oData
    End Function

    Public Function GetvWorkflowLeadTimeParameter(ByVal sParKey As String) As LTS.vWorkflowLeadTimeParameter
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery = db.vWorkflowLeadTimeParameters.Where(Function(x) x.ParKey = sParKey)
                Return iQuery.FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvWorkflowLeadTimeParameter"), db)
            End Try

            Return Nothing
        End Using
    End Function
    Public Function GetvWorkflowLeadTimeParameters() As List(Of LTS.vWorkflowLeadTimeParameter)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery = db.vWorkflowLeadTimeParameters
                Return iQuery.ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvWorkflowLeadTimeParameters"), db)
            End Try

            Return New List(Of LTS.vWorkflowLeadTimeParameter)
        End Using
    End Function

    Public Function SavevWorkflowLeadTimeParameters(ByVal lPars As List(Of LTS.vWorkflowLeadTimeParameter)) As Boolean
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                For Each iPar In lPars
                    Dim oPar = db.Parameters.Where(Function(x) x.ParKey = iPar.ParKey).FirstOrDefault()
                    If Not oPar Is Nothing Then
                        oPar.ParValue = iPar.ParValue
                    End If
                Next
                db.SubmitChanges()
                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavevWorkflowLeadTimeParameters"), db)
            End Try

            Return False
        End Using
    End Function


    Public Function SavevWorkflowLeadTimeParameter(ByVal iPar As LTS.vWorkflowLeadTimeParameter) As Boolean
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                Dim oPar = db.Parameters.Where(Function(x) x.ParKey = iPar.ParKey).FirstOrDefault()
                If Not oPar Is Nothing Then
                    oPar.ParValue = iPar.ParValue
                    db.SubmitChanges()
                End If


                Return True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavevWorkflowLeadTimeParameter"), db)
            End Try

            Return False
        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.Parameter)
        'Create(New Record)
        Return New LTS.Parameter With {.ParCategoryControl = d.ParCategoryControl _
                                        , .ParKey = d.ParKey _
                                        , .ParValue = d.ParValue _
                                        , .ParText = d.ParText _
                                        , .ParDescription = d.ParDescription _
                                        , .ParIsGlobal = d.ParIsGlobal _
                                        , .ParUpdated = If(d.ParUpdated Is Nothing, New Byte() {}, d.ParUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetParameterFiltered(ParKey:=CType(LinqTable, LTS.Parameter).ParKey)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.Parameter)
            Try
                Dim Parameter As DTO.Parameter = (
                    From t In CType(oDB, NGLMASSYSDataContext).Parameters
                    Where
                        (t.ParKey = .ParKey)
                    Select New DTO.Parameter With {.ParKey = t.ParKey}).First

                If Not Parameter Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Parameter data.  The Parameter Key, " & .ParKey & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow parameter data to be deleted via WCF
        Utilities.SaveAppError("Cannot delete Parameter data.  Parameters are required by the system and cannot be deleted!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DTO.DTOBaseClass)

        With CType(LinqTable, LTS.Parameter)
            'Add Parameter contact Records
            .CompParameterRefSystems.AddRange(
                     From d In CType(oData, DTO.Parameter).CompParameters
                     Select New LTS.CompParameterRefSystem With {.CompParControl = d.CompParControl _
                                        , .CompParCompControl = d.CompParCompControl _
                                        , .CompParCategoryControl = d.CompParCategoryControl _
                                        , .CompParKey = d.CompParKey _
                                        , .CompParValue = If(d.CompParValue.HasValue, d.CompParValue.Value, 0) _
                                        , .CompParText = d.CompParText _
                                        , .CompParDescription = d.CompParDescription _
                                        , .CompParOLE = If(d.CompParOLE Is Nothing, New Byte() {}, d.CompParOLE) _
                                        , .CompParUpdated = If(d.CompParUpdated Is Nothing, New Byte() {}, d.CompParUpdated)})

        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASSYSDataContext)
            .CompParameterRefSystems.InsertAllOnSubmit(CType(LinqTable, LTS.Parameter).CompParameterRefSystems)
        End With
    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        With CType(oDB, NGLMASSYSDataContext)
            ' Process any inserted contact records 
            .CompParameterRefSystems.InsertAllOnSubmit(GetParameterCompParameterChanges(oData, TrackingInfo.Created))
            ' Process any updated contact records
            .CompParameterRefSystems.AttachAll(GetParameterCompParameterChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted contact records
            Dim deletedCompParameters = GetParameterCompParameterChanges(oData, TrackingInfo.Deleted)
            .CompParameterRefSystems.AttachAll(deletedCompParameters, True)
            .CompParameterRefSystems.DeleteAllOnSubmit(deletedCompParameters)
        End With
    End Sub

    Protected Function GetParameterCompParameterChanges(ByVal source As DTO.Parameter, ByVal changeType As TrackingInfo) As List(Of LTS.CompParameterRefSystem)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CompParameterRefSystem) = (
          From d In source.CompParameters
          Where d.TrackingState = changeType
          Select New LTS.CompParameterRefSystem With {.CompParControl = d.CompParControl _
                                        , .CompParCompControl = d.CompParCompControl _
                                        , .CompParCategoryControl = d.CompParCategoryControl _
                                        , .CompParKey = d.CompParKey _
                                        , .CompParValue = If(d.CompParValue.HasValue, d.CompParValue.Value, 0) _
                                        , .CompParText = d.CompParText _
                                        , .CompParDescription = d.CompParDescription _
                                        , .CompParUpdated = If(d.CompParUpdated Is Nothing, New Byte() {}, d.CompParUpdated)})
        Return details.ToList()
    End Function
#End Region

End Class


Public Class NGLParCategoryData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblParCategories
        Me.LinqDB = db
        Me.SourceClass = "NGLParCategoryData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            Me.LinqTable = db.tblParCategories
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public DTO Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetParCategoryFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetParCategoriesFiltered()
    End Function

    Public Function GetParCategoryFiltered(Optional ByVal Control As Integer = 0) As DTO.ParCategory
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                ' Get the newest record that matches the provided criteria
                Dim ParCategory As DTO.ParCategory = (
                From d In db.tblParCategories
                Where
                    (d.ParCatControl = If(Control = 0, d.ParCatControl, Control))
                Select New DTO.ParCategory With {.ParCatControl = d.ParCatControl _
                                       , .ParCatName = d.ParCatName _
                                       , .ParCatDescription = d.ParCatDescription _
                                       , .ParCatUpdated = d.ParCatUpdated.ToArray()}).First

                Return ParCategory

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetParCategoriesFiltered(Optional ByVal ParCatName As String = "") As DTO.ParCategory()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim ParCategorys() As DTO.ParCategory = (
                From d In db.tblParCategories
                Where
                    (ParCatName Is Nothing OrElse String.IsNullOrEmpty(ParCatName) OrElse d.ParCatName = ParCatName)
                Order By d.ParCatName, d.ParCatControl
                Select New DTO.ParCategory With {.ParCatControl = d.ParCatControl _
                                        , .ParCatName = d.ParCatName _
                                        , .ParCatDescription = d.ParCatDescription _
                                        , .ParCatUpdated = d.ParCatUpdated.ToArray()}).ToArray()
                Return ParCategorys

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetParCategoriesWithDetails(Optional ByVal ParCatName As String = "") As DTO.ParCategory()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Return all the parameter categories and parameters that match the criteria sorted by name
                Dim ParCategorys() As DTO.ParCategory = (
                From g In db.tblParCategories
                Where
                    (ParCatName Is Nothing OrElse String.IsNullOrEmpty(ParCatName) OrElse g.ParCatName = ParCatName)
                Order By g.ParCatName, g.ParCatControl
                Select New DTO.ParCategory With {.ParCatControl = g.ParCatControl _
                                        , .ParCatName = g.ParCatName _
                                        , .ParCatDescription = g.ParCatDescription _
                                        , .ParCatUpdated = g.ParCatUpdated.ToArray() _
                                        , .Parameters = (
                                            From t In g.Parameters
                                            Order By t.ParKey
                                            Select New DTO.Parameter With {.ParCategoryControl = t.ParCategoryControl _
                                            , .ParKey = t.ParKey _
                                            , .ParValue = If(t.ParValue.HasValue, t.ParValue.Value, 0) _
                                            , .ParText = t.ParText _
                                            , .ParDescription = t.ParDescription _
                                            , .ParUpdated = t.ParUpdated.ToArray() _
                                            , .ParCatName = g.ParCatName _
                                            , .ParIsGlobal = t.ParIsGlobal _
                                            , .CompParameters = (
                                                From d In t.CompParameterRefSystems
                                                Join p In db.CompRefSystems On d.CompParCompControl Equals p.CompControl
                                                Order By p.CompNumber
                                                Select New DTO.CompParameter With {.CompParControl = d.CompParControl _
                                                    , .CompParCompControl = d.CompParCompControl _
                                                    , .CompParCategoryControl = d.CompParCategoryControl _
                                                    , .CompParKey = d.CompParKey _
                                                    , .CompParValue = If(d.CompParValue.HasValue, d.CompParValue.Value, 0) _
                                                    , .CompParText = d.CompParText _
                                                    , .CompParDescription = d.CompParDescription _
                                                    , .CompNumber = p.CompNumber _
                                                    , .CompParUpdated = d.CompParUpdated.ToArray()}).ToList()}).ToList()}).ToArray()
                Return ParCategorys

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region


#Region "Public LTS Methods"

    ''' <summary>
    ''' Get a filtered array of Par Category Data
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Category Logic
    ''' </remarks>
    Public Function GettblParCategories(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblParCategory()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblParCategory
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblParCategory)
                iQuery = db.tblParCategories
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblParCategories"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Insert or Update Par Category Data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Category  Logic
    ''' </remarks>
    Public Function SavetblParCategory(ByVal oData As LTS.tblParCategory) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iParCatControl As Integer = oData.ParCatControl
                'verify Name
                If String.IsNullOrWhiteSpace(oData.ParCatName) Then
                    Dim lDetails As New List(Of String) From {"Workflow Category Name", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                'check for insert
                If (iParCatControl = 0) Then
                    'this is an insert test if the name already exists
                    If db.tblParCategories.Any(Function(x) String.Compare(x.ParCatName, oData.ParCatName, True) <> 0) Then
                        Dim lDetails As New List(Of String) From {"Workflow Category Name", " " & oData.ParCatName & " already exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    db.tblParCategories.InsertOnSubmit(oData)
                Else
                    'This is an update so get the current record,  if it does not exist throw error
                    Dim oParCat As LTS.tblParCategory = db.tblParCategories.Where(Function(x) x.ParCatControl = iParCatControl).FirstOrDefault()
                    If oParCat Is Nothing OrElse oParCat.ParCatControl = 0 Then
                        Dim lDetails As New List(Of String) From {"Workflow Category Key", " was not found or has been deleted and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    If db.tblParCategories.Any(Function(x) String.Compare(x.ParCatName, oData.ParCatName, True) = 0 And x.ParCatControl <> oData.ParCatControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Category Name", " " & oData.ParCatName & " a duplicate exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    Dim otblParCategory = db.tblParCategories.Where(Function(x) x.ParCatControl = iParCatControl).FirstOrDefault()
                    Dim skipObjs As New List(Of String) From {"ParCatControl", "Parameters", "CompParameterRefSystems", "tblParProcesses"}
                    Dim strMsg As String
                    otblParCategory = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(otblParCategory, oData, skipObjs, strMsg)
                    'db.tblParCategories.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblParCategory"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific Par Category Record
    ''' </summary>
    ''' <param name="iParCatControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Category Logic
    ''' </remarks>
    Public Function DeletetblParCategory(ByVal iParCatControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iParCatControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'verify the record
                Dim oExisting = db.tblParCategories.Where(Function(x) x.ParCatControl = iParCatControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ParCatControl = 0 Then Return True
                ValidateDeletedRecord(db, iParCatControl, oExisting.ParCatName)
                db.tblParCategories.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblParCategory"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.ParCategory)
        'Create(New Record)
        Return New LTS.tblParCategory With {.ParCatControl = d.ParCatControl _
                                        , .ParCatName = d.ParCatName _
                                        , .ParCatDescription = d.ParCatDescription _
                                        , .ParCatUpdated = If(d.ParCatUpdated Is Nothing, New Byte() {}, d.ParCatUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetParCategoryFiltered(Control:=CType(LinqTable, LTS.tblParCategory).ParCatControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.ParCategory)
            Try
                Dim oExsting As DTO.ParCategory = (
                    From t In CType(oDB, NGLMASSYSDataContext).tblParCategories
                    Where
                        (t.ParCatName = .ParCatName)
                    Select New DTO.ParCategory With {.ParCatControl = t.ParCatControl}).First

                If Not oExsting Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Parameter Category data.  The Category name, " & .ParCatName & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.ParCategory)
            Try
                'Get the newest record that matches the provided criteria
                Dim Carrier As DTO.ParCategory = (
                From t In CType(oDB, NGLMASSYSDataContext).tblParCategories
                Where
                    (t.ParCatControl <> .ParCatControl) _
                    And
                    (t.ParCatName = .ParCatName)
                Select New DTO.ParCategory With {.ParCatControl = t.ParCatControl}).First

                If Not Carrier Is Nothing Then
                    Utilities.SaveAppError("Cannot save Parameter Category changes.  The Category name, " & .ParCatName & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    ''' <summary>
    ''' DTO Validate Delete base class Override throwCannotDeleteRecordInUseException on validation failure
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.006 on 10/20/2022 calls new LTS delete validation method
    ''' </remarks>
    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data is being used by the parameter or compparameter data 
        With CType(oData, DTO.ParCategory)
            Dim iKey = .ParCatControl
            Dim sName = .ParCatName
            ValidateDeletedRecord(CType(oDB, NGLMASSYSDataContext), iKey, sName)
        End With
    End Sub

    ''' <summary>
    ''' LTS Validate Delete base class Override throwCannotDeleteRecordInUseException on validation failure
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="iKey"></param>
    ''' <param name="sName"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 new LTS delete validation method
    ''' </remarks>
    Protected Overloads Sub ValidateDeletedRecord(ByRef oDB As NGLMASSYSDataContext, ByVal iKey As Integer, ByVal sName As String)

        If (
            oDB.Parameters.Any(Function(x) x.ParCategoryControl = iKey) _
            OrElse
            oDB.CompParameterRefSystems.Any(Function(x) x.CompParCategoryControl = iKey) _
            OrElse
            oDB.tblParProcesses.Any(Function(x) x.ParProcCategoryControl = iKey)
            ) Then
            throwCannotDeleteRecordInUseException("Workflow Category Name", sName)
        End If

    End Sub

#End Region

End Class


Public Class NGLCompParameterRefSystemData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.CompParameterRefSystems
        Me.LinqDB = db
        Me.SourceClass = "NGLCompParameterRefSystemData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            Me.LinqTable = db.CompParameterRefSystems
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCompParameterFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetCompParameterFiltered(Optional ByVal Control As Integer = 0, Optional ByVal CompControl As Integer = 0, Optional ByVal CompParKey As String = "") As DTO.CompParameter
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                ' Get the newest record that matches the provided criteria
                Dim CompParameter As DTO.CompParameter = (
                From d In db.CompParameterRefSystems
                Where
                    (d.CompParControl = If(Control = 0, d.CompParControl, Control)) _
                    And
                    (d.CompParCompControl = If(CompControl = 0, d.CompParCompControl, CompControl)) _
                    And
                    (CompParKey Is Nothing OrElse String.IsNullOrEmpty(CompParKey) OrElse d.CompParKey = CompParKey)
                Select New DTO.CompParameter With {.CompParControl = d.CompParControl _
                                        , .CompParCompControl = d.CompParCompControl _
                                        , .CompParCategoryControl = d.CompParCategoryControl _
                                        , .CompParKey = d.CompParKey _
                                        , .CompParValue = If(d.CompParValue.HasValue, d.CompParValue.Value, 0) _
                                        , .CompParText = d.CompParText _
                                        , .CompParDescription = d.CompParDescription _
                                        , .CompParUpdated = d.CompParUpdated.ToArray()}).First

                Return CompParameter

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCompParametersFiltered(ByVal CompControl As Integer, Optional ByVal CompParCategoryControl As Integer = 0) As DTO.CompParameter()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CompParameters() As DTO.CompParameter = (
                From d In db.CompParameterRefSystems
                Where
                    (d.CompParCompControl = If(CompControl = 0, d.CompParCompControl, CompControl)) _
                    And
                    (d.CompParCategoryControl = If(CompParCategoryControl = 0, d.CompParCategoryControl, CompParCategoryControl))
                Order By d.CompParKey
                Select New DTO.CompParameter With {.CompParControl = d.CompParControl _
                                        , .CompParCompControl = d.CompParCompControl _
                                        , .CompParCategoryControl = d.CompParCategoryControl _
                                        , .CompParKey = d.CompParKey _
                                        , .CompParValue = If(d.CompParValue.HasValue, d.CompParValue.Value, 0) _
                                        , .CompParText = d.CompParText _
                                        , .CompParDescription = d.CompParDescription _
                                        , .CompParUpdated = d.CompParUpdated.ToArray()}).ToArray()
                Return CompParameters

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.CompParameter)
        'Create(New Record)
        Return New LTS.CompParameterRefSystem With {.CompParControl = d.CompParControl _
                                        , .CompParCompControl = d.CompParCompControl _
                                        , .CompParCategoryControl = d.CompParCategoryControl _
                                        , .CompParKey = d.CompParKey _
                                        , .CompParValue = If(d.CompParValue.HasValue, d.CompParValue.Value, 0) _
                                        , .CompParText = d.CompParText _
                                        , .CompParDescription = d.CompParDescription _
                                        , .CompParUpdated = If(d.CompParUpdated Is Nothing, New Byte() {}, d.CompParUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCompParameterFiltered(Control:=CType(LinqTable, LTS.CompParameterRefSystem).CompParControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.CompParameter)
            Try
                Dim oExists As DTO.CompParameter = (
                    From t In CType(oDB, NGLMASSYSDataContext).CompParameterRefSystems
                    Where
                        (t.CompParCompControl = .CompParCompControl) _
                        And
                        (t.CompParKey = .CompParKey)
                    Select New DTO.CompParameter With {.CompParControl = t.CompParControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Company Parameter data.  The Parameter Key, " & .CompParKey & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.CompParameter)
            Try
                'Get the newest record that matches the provided criteria
                Dim oExists As DTO.CompParameter = (
                From t In CType(oDB, NGLMASSYSDataContext).CompParameterRefSystems
                Where
                    (t.CompParControl <> .CompParControl) _
                    And
                    (t.CompParCompControl = .CompParCompControl) _
                    And
                    (t.CompParKey = .CompParKey)
                Select New DTO.CompParameter With {.CompParControl = t.CompParControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save Company Parameter changes.  The Parameter Key, " & .CompParKey & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow parameter data to be deleted via WCF
        Utilities.SaveAppError("Cannot delete Company Parameter data.  Company Parameters are required by the system and cannot be deleted!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

#End Region

End Class

Public Class NGLtblDataEntryFieldData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblDataEntryFields
        Me.LinqDB = db
        Me.SourceClass = "NGLtblDataEntryFieldData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            Me.LinqTable = db.tblDataEntryFields
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GettblDataEntryFieldFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GettblDataEntryFieldFiltered(Optional ByVal Control As Integer = 0) As DTO.tblDataEntryField
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                ' Get the newest record that matches the provided criteria
                Dim tblDataEntryField As DTO.tblDataEntryField = (
                From d In db.tblDataEntryFields
                Where
                    (d.DEControl = Control)
                Select New DTO.tblDataEntryField With {.DEControl = d.DEControl _
                                        , .DEFieldDesc = d.DEFieldDesc _
                                        , .DEFieldMapCode = d.DEFieldMapCode _
                                        , .DEFieldName = d.DEFieldName _
                                        , .DEFileType = d.DEFileType _
                                        , .DEFlag = d.DEFlag _
                                        , .DEGLNumber = d.DEGLNumber _
                                        , .DEUpdated = d.DEUpdated.ToArray()}).First

                Return tblDataEntryField

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblDataEntryFieldsFiltered(ByVal DEFileType As Integer) As DTO.tblDataEntryField()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'Return all the field names that match that match the criteria sorted by name
                Dim tblDataEntryFields() As DTO.tblDataEntryField = (
                From d In db.tblDataEntryFields
                Where
                    (d.DEFileType = DEFileType)
                Order By d.DEFieldName
                Select New DTO.tblDataEntryField With {.DEControl = d.DEControl _
                                        , .DEFieldDesc = d.DEFieldDesc _
                                        , .DEFieldMapCode = d.DEFieldMapCode _
                                        , .DEFieldName = d.DEFieldName _
                                        , .DEFileType = d.DEFileType _
                                        , .DEFlag = d.DEFlag _
                                        , .DEGLNumber = d.DEGLNumber _
                                        , .DEUpdated = d.DEUpdated.ToArray()}).ToArray()
                Return tblDataEntryFields

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblDataEntryField)
        'Create(New Record)
        Return New LTS.tblDataEntryField With {.DEControl = d.DEControl _
                                        , .DEFieldDesc = d.DEFieldDesc _
                                        , .DEFieldMapCode = d.DEFieldMapCode _
                                        , .DEFieldName = d.DEFieldName _
                                        , .DEFileType = d.DEFileType _
                                        , .DEFlag = d.DEFlag _
                                        , .DEGLNumber = d.DEGLNumber _
                                        , .DEUpdated = If(d.DEUpdated Is Nothing, New Byte() {}, d.DEUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblDataEntryFieldFiltered(Control:=CType(LinqTable, LTS.tblDataEntryField).DEControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblDataEntryField)
            Try
                Dim oExists As DTO.tblDataEntryField = (
                    From t In CType(oDB, NGLMASSYSDataContext).tblDataEntryFields
                    Where
                        (t.DEFieldName = .DEFieldName) _
                        And
                        (t.DEFileType = .DEFileType)
                    Select New DTO.tblDataEntryField With {.DEControl = t.DEControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Data Entry Field data.  The Data Entry Field, " & .DEFieldName & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblDataEntryField)
            Try
                'Get the newest record that matches the provided criteria
                Dim oExists As DTO.tblDataEntryField = (
                From t In CType(oDB, NGLMASSYSDataContext).tblDataEntryFields
                Where
                    (t.DEControl <> .DEControl) _
                    And
                    (t.DEFieldName = .DEFieldName) _
                    And
                    (t.DEFileType = .DEFileType)
                Select New DTO.tblDataEntryField With {.DEControl = t.DEControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save Data Entry Field changes.  The Data Entry Field, " & .DEFieldName & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow parameter data to be deleted via WCF
        Utilities.SaveAppError("Cannot delete Data Entry Field data.  These values are required by the system and cannot be deleted!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

#End Region

End Class

Public Class NGLtblExportFieldData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblExportFields
        Me.LinqDB = db
        Me.SourceClass = "NGLtblExportFieldData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            Me.LinqTable = db.tblExportFields
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GettblExportFieldFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GettblExportFieldFiltered(Optional ByVal Control As Integer = 0) As DTO.tblExportField
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                ' Get the newest record that matches the provided criteria
                Dim tblExportField As DTO.tblExportField = (
                From d In db.tblExportFields
                Where
                    (d.ExportControl = Control)
                Select New DTO.tblExportField With {.ExportControl = d.ExportControl _
                                        , .ExportFieldDesc = d.ExportFieldDesc _
                                        , .ExportFieldName = d.ExportFieldName _
                                        , .ExportFileType = d.ExportFileType _
                                        , .ExportFlag = d.ExportFlag _
                                        , .ExportUpdated = d.ExportUpdated.ToArray()}).First

                Return tblExportField

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblExportFieldsFiltered(ByVal DEFileType As Integer) As DTO.tblExportField()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'Return all the field names that match that match the criteria sorted by name
                Dim tblExportFields() As DTO.tblExportField = (
                From d In db.tblExportFields
                Where
                    (d.ExportFileType = DEFileType)
                Order By d.ExportFieldDesc
                Select New DTO.tblExportField With {.ExportControl = d.ExportControl _
                                        , .ExportFieldDesc = d.ExportFieldDesc _
                                        , .ExportFieldName = d.ExportFieldName _
                                        , .ExportFileType = d.ExportFileType _
                                        , .ExportFlag = d.ExportFlag _
                                        , .ExportUpdated = d.ExportUpdated.ToArray()}).ToArray()
                Return tblExportFields

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblExportField)
        'Create(New Record)
        Return New LTS.tblExportField With {.ExportControl = d.ExportControl _
                                        , .ExportFieldDesc = d.ExportFieldDesc _
                                        , .ExportFieldName = d.ExportFieldName _
                                        , .ExportFileType = d.ExportFileType _
                                        , .ExportFlag = d.ExportFlag _
                                        , .ExportUpdated = If(d.ExportUpdated Is Nothing, New Byte() {}, d.ExportUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblExportFieldFiltered(Control:=CType(LinqTable, LTS.tblExportField).ExportControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblExportField)
            Try
                Dim oExists As DTO.tblExportField = (
                    From t In CType(oDB, NGLMASSYSDataContext).tblExportFields
                    Where
                        (t.ExportFieldName = .ExportFieldName) _
                        And
                        (t.ExportFileType = .ExportFileType)
                    Select New DTO.tblExportField With {.ExportControl = t.ExportControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Export Field data.  The Export Field, " & .ExportFieldName & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblExportField)
            Try
                'Get the newest record that matches the provided criteria
                Dim oExists As DTO.tblExportField = (
                From t In CType(oDB, NGLMASSYSDataContext).tblExportFields
                Where
                    (t.ExportControl <> .ExportControl) _
                    And
                    (t.ExportFieldName = .ExportFieldName) _
                    And
                    (t.ExportFileType = .ExportFileType)
                Select New DTO.tblExportField With {.ExportControl = t.ExportControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save Export Field changes.  The Export Field, " & .ExportFieldName & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'We do not allow parameter data to be deleted via WCF
        Utilities.SaveAppError("Cannot delete Export Field data.  These values are required by the system and cannot be deleted!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))
    End Sub

#End Region

End Class

Public Class NGLtblExportFileSettingData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblExportFileSettings
        Me.LinqDB = db
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            Me.LinqTable = db.tblExportFileSettings
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GettblExportFileSettingFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GettblExportFileSettingFiltered(Optional ByVal Control As Integer = 0) As DTO.tblExportFileSetting
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                ' Get the newest record that matches the provided criteria
                Dim tblExportFileSetting As DTO.tblExportFileSetting = (
                From d In db.tblExportFileSettings
                Where
                    (d.EFSControl = Control)
                Select New DTO.tblExportFileSetting With {.EFSControl = d.EFSControl _
                                        , .EFSDesc = d.EFSDesc _
                                        , .EFSFileName = d.EFSFileName _
                                        , .EFSFileType = d.EFSFileType _
                                        , .EFSFolder = d.EFSFolder _
                                        , .EFSUpdated = d.EFSUpdated.ToArray()}).First

                Return tblExportFileSetting

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblExportFileSettingsFiltered(ByVal DEFileType As Integer) As DTO.tblExportFileSetting()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'Return all the field names that match that match the criteria sorted by name
                Dim tblExportFileSettings() As DTO.tblExportFileSetting = (
                From d In db.tblExportFileSettings
                Where
                    (d.EFSFileType = DEFileType)
                Order By d.EFSDesc
                Select New DTO.tblExportFileSetting With {.EFSControl = d.EFSControl _
                                        , .EFSDesc = d.EFSDesc _
                                        , .EFSFileName = d.EFSFileName _
                                        , .EFSFileType = d.EFSFileType _
                                        , .EFSFolder = d.EFSFolder _
                                        , .EFSUpdated = d.EFSUpdated.ToArray()}).ToArray()
                Return tblExportFileSettings

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblExportFileSetting)
        'Create(New Record)
        Return New LTS.tblExportFileSetting With {.EFSControl = d.EFSControl _
                                        , .EFSDesc = d.EFSDesc _
                                        , .EFSFileName = d.EFSFileName _
                                        , .EFSFileType = d.EFSFileType _
                                        , .EFSFolder = d.EFSFolder _
                                        , .EFSUpdated = If(d.EFSUpdated Is Nothing, New Byte() {}, d.EFSUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblExportFileSettingFiltered(Control:=CType(LinqTable, LTS.tblExportFileSetting).EFSControl)
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblExportFileSetting)
            Try
                Dim oExists As DTO.tblExportFileSetting = (
                    From t In CType(oDB, NGLMASSYSDataContext).tblExportFileSettings
                    Where
                        (t.EFSFileName = .EFSFileName) _
                        And
                        (t.EFSFileType = .EFSFileType)
                    Select New DTO.tblExportFileSetting With {.EFSControl = t.EFSControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save new Export File Setting data.  The Export File Setting, " & .EFSFileName & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.tblExportFileSetting)
            Try
                'Get the newest record that matches the provided criteria
                Dim oExists As DTO.tblExportFileSetting = (
                From t In CType(oDB, NGLMASSYSDataContext).tblExportFileSettings
                Where
                    (t.EFSControl <> .EFSControl) _
                    And
                    (t.EFSFileName = .EFSFileName) _
                    And
                    (t.EFSFileType = .EFSFileType)
                Select New DTO.tblExportFileSetting With {.EFSControl = t.EFSControl}).First

                If Not oExists Is Nothing Then
                    Utilities.SaveAppError("Cannot save Export File Setting changes.  The Export File Setting, " & .EFSFileName & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

#End Region

End Class

Public Class NGLSystemAlertData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = Nothing
        Me.LinqDB = db
        Me.SourceClass = "NGLSystemAlertData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            Me.LinqTable = Nothing
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    Public Function GetSystemAlertNodes(Optional ByVal CarrierControl As Integer = 0) As DTO.SystemAlertNode()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                If CarrierControl Then CarrierFilter = CarrierControl

                Dim Nodes As DTO.SystemAlertNode() = (
                From d In db.spGetSystemAlertNodes(CarrierFilter, Me.Parameters.UserName)
                Select New DTO.SystemAlertNode With {.ATID = d.ATID _
                                        , .ATName = d.ATName _
                                        , .ATDescription = d.ATDescription _
                                        , .ATAlertCount = d.ATAlertCount}).ToArray

                Return Nodes

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierContractExpiredAlerts(Optional ByVal CarrierControl As Integer = 0) As DTO.CarrContractExpiredAlert()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                If CarrierControl Then CarrierFilter = CarrierControl

                Dim oList As DTO.CarrContractExpiredAlert() = (
                From d In db.udfAlertsCarrierContractExpiredCompFilter50(CarrierFilter, Me.Parameters.UserName)
                Select New DTO.CarrContractExpiredAlert With {.CarrierControl = d.CarrierControl _
                                        , .CarrierNumber = d.CarrierNumber _
                                        , .CarrierQualContractExpiresDate = d.CarrierQualContractExpiresDate _
                                        , .AlertDate = d.AlertDate}).ToArray

                Return oList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Return a list of Active Carriers with Undelivered loads that have a contract that will expire today or earlier. 
    ''' A null or empty contract date mean it does not expire?
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.0.001 on 10/16/2021  new 365 version
    '''     modified to use Linq instead of calling the SQL UDF functions
    ''' </remarks>
    Public Function GetCarrierContractExpiredAlerts365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCMAlert()
        Dim oRet() As LTS.vCMAlert
        Dim dtAlertDate As Date = Date.Now.ToShortDateString()
        Dim dtExpireDefault As Date = dtAlertDate.AddDays(1).ToShortDateString()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oUndeliveredCarriers = NGLBookObjData.getUnDeliveredCarriersbyUser()
                'Dim oExpContractCarriers As List(Of LTS.Carrier) = (From d In db.Carriers
                '                                                    Where (oUndeliveredCarriers Is Nothing OrElse oUndeliveredCarriers.Count = 0 OrElse oUndeliveredCarriers.Contains(d.CarrierControl)) AndAlso d.CarrierActive = True AndAlso (If(d.CarrierQualContractExpiresDate, dtExpireDefault) <= dtAlertDate) Select d).ToList()
                'If oExpContractCarriers Is Nothing OrElse oExpContractCarriers.Count() < 1 Then Return oRet

                Dim oExpContractCarriers = From d In db.Carriers Where (oUndeliveredCarriers Is Nothing OrElse oUndeliveredCarriers.Count = 0 OrElse oUndeliveredCarriers.Contains(d.CarrierControl)) AndAlso d.CarrierActive = True AndAlso (If(d.CarrierQualContractExpiresDate, dtExpireDefault) <= dtAlertDate) Select d

                Dim iQuery As IQueryable(Of LTS.vCMAlert)
                iQuery = (From d In oExpContractCarriers
                          Select New LTS.vCMAlert With {.CarrierControl = d.CarrierControl _
                              , .CarrierNumber = d.CarrierNumber _
                              , .CarrierName = d.CarrierName _
                              , .AlertDate1 = If(d.CarrierQualContractExpiresDate, dtAlertDate) _
                              , .AlertDate = dtAlertDate _
                              , .BookModDate = d.CarrierModDate _
                              , .BookModUser = d.CarrierModUser _
                              , .Type = "Carrier Contract Expired Alerts"})

                db.Log = New DebugTextWriter
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierContractExpiredAlerts365"), db)
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetCarrierInsExpPerShipAlerts(Optional ByVal CarrierControl As Integer = 0) As DTO.CarrInsExpPerShipAlert()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                If CarrierControl Then CarrierFilter = CarrierControl

                Dim oList As DTO.CarrInsExpPerShipAlert() = (
                From d In db.spGetCarrInsExpPerShipAlerts(CarrierFilter, Me.Parameters.UserName)
                Select New DTO.CarrInsExpPerShipAlert With {.CarrierControl = d.CarrierControl _
                                        , .CarrierNumber = d.CarrierNumber _
                                        , .BookConsPrefix = d.BookConsPrefix _
                                        , .BookProNumber = d.BookProNumber _
                                        , .Exposure = d.Exposure _
                                        , .Message = d.Message _
                                        , .ProductValue = d.ProductValue _
                                        , .AlertDate = d.AlertDate}).ToArray

                Return oList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Read current Carrier Insurance Exposure per Shipment Alerts
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.0.001 on 10/16/2021  new 365 version
    '''     modified to use All filter data and IQueryable
    '''     we store the results in tblTempCMAlert because Iqueryable
    '''     does not work with stored procedure data.
    ''' </remarks>
    Public Function GetCarrierInsExpPerShipAlerts365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCMAlert()
        Dim oRet() As LTS.vCMAlert
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                '365 we do not filter by carrier control, users can select carrier name or carrier number
                'If CarrierControl Then CarrierFilter = CarrierControl
                Dim dtModDate = Date.Now
                Dim sModUser = Parameters.UserName
                Dim iUserTimeKey As Long = 0
                Long.TryParse(((Date.Now - New Date(1970, 1, 1)).TotalMilliseconds).ToString(), iUserTimeKey) ' DateTimeOffset.ToUnixTimeMilliseconds(Date.Now())  '() '  DateTimeOffset.ToUnixTimeMilliseconds(date.now())
                Dim otblTempCMAlerts = (From d In db.spGetCarrInsExpPerShipAlerts(CarrierFilter, Me.Parameters.UserName)
                                        Select New LTS.tblTempCMAlert With {.TmpCMAlertCarrierControl = d.CarrierControl _
                              , .TmpCMAlertCarrierNumber = d.CarrierNumber _
                              , .TmpCMAlertCarrierName = d.CarrierName _
                              , .TmpCMAlertBookConsPrefix = d.BookConsPrefix _
                              , .TmpCMAlertBookProNumber = d.BookProNumber _
                              , .TmpCMAlertExposure = d.Exposure _
                              , .TmpCMAlertMessage = d.Message _
                              , .TmpCMAlertProductValue = d.ProductValue _
                              , .TmpCMAlertAlertDate = d.AlertDate _
                              , .TmpCMAlerteModDate = dtModDate _
                              , .TmpCMAlerteModUser = sModUser _
                              , .TmpCMAlertUserControl = iUserTimeKey}).ToArray()
                If (Not otblTempCMAlerts Is Nothing AndAlso otblTempCMAlerts.Count() > 0) Then
                    db.tblTempCMAlerts.InsertAllOnSubmit(otblTempCMAlerts)
                    db.SubmitChanges()

                    Dim iQuery As IQueryable(Of LTS.vCMAlert)
                    iQuery = (From d In db.tblTempCMAlerts Where d.TmpCMAlertUserControl = iUserTimeKey
                              Select New LTS.vCMAlert With {.CarrierControl = d.TmpCMAlertCarrierControl _
                              , .CarrierNumber = d.TmpCMAlertCarrierNumber _
                              , .CarrierName = d.TmpCMAlertCarrierName _
                              , .BookConsPrefix = d.TmpCMAlertBookConsPrefix _
                              , .BookProNumber = d.TmpCMAlertBookProNumber _
                              , .Exposure = d.TmpCMAlertExposure _
                              , .Message = d.TmpCMAlertMessage _
                              , .ProductValue = d.TmpCMAlertProductValue _
                              , .AlertDate = d.TmpCMAlertAlertDate})

                    'db.Log = New DebugTextWriter
                    Dim filterWhere = ""
                    ApplyAllFilters(iQuery, filters, filterWhere)
                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                    Try
                        db.tblTempCMAlerts.DeleteAllOnSubmit(otblTempCMAlerts)
                        db.SubmitChanges()
                    Catch ex As Exception
                        'do nothing system clear old logs will fix this later using old TmpCMAlerteModDate
                    End Try
                    Return oRet
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierInsExpPerShipAlerts365"), db)
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetCarrierInsExpAllShipAlerts(Optional ByVal CarrierControl As Integer = 0) As DTO.CarrInsExpAllShipAlert()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                If CarrierControl Then CarrierFilter = CarrierControl

                Dim oList As DTO.CarrInsExpAllShipAlert() = (
                From d In db.spGetCarrInsExpAllShipAlerts(CarrierFilter, Me.Parameters.UserName)
                Select New DTO.CarrInsExpAllShipAlert With {.CarrierControl = d.CarrierControl _
                                        , .CarrierNumber = d.CarrierNumber _
                                        , .Exposure = d.Exposure _
                                        , .Message = d.Message _
                                        , .ProductValue = d.ProductValue _
                                        , .AlertDate = d.AlertDate}).ToArray

                Return oList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Read current Carrier Insurance Exposure for All Shipments Summary Alerts
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.0.001 on 10/16/2021  new 365 version
    '''     modified to use All filter data and IQueryable
    '''     we store the results in tblTempCMAlert because Iqueryable
    '''     does not work with stored procedure data.
    ''' </remarks>
    Public Function GetCarrierInsExpAllShipAlerts365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCMAlert()
        Dim oRet() As LTS.vCMAlert
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                '365 we do not filter by carrier control, users can select carrier name or carrier number
                'If CarrierControl Then CarrierFilter = CarrierControl
                Dim dtModDate = Date.Now
                Dim sModUser = Parameters.UserName
                Dim iUserTimeKey As Long = 0
                Long.TryParse(((Date.Now - New Date(1970, 1, 1)).TotalMilliseconds).ToString(), iUserTimeKey) ' DateTimeOffset.ToUnixTimeMilliseconds(Date.Now())  '() '  DateTimeOffset.ToUnixTimeMilliseconds(date.now())

                Dim otblTempCMAlerts = (From d In db.spGetCarrInsExpAllShipAlerts(CarrierFilter, Me.Parameters.UserName)
                                        Select New LTS.tblTempCMAlert With {.TmpCMAlertCarrierControl = d.CarrierControl _
                                                      , .TmpCMAlertCarrierNumber = d.CarrierNumber _
                                                      , .TmpCMAlertCarrierName = d.CarrierName _
                                                      , .TmpCMAlertExposure = d.Exposure _
                                                      , .TmpCMAlertMessage = d.Message _
                                                      , .TmpCMAlertProductValue = d.ProductValue _
                                                      , .TmpCMAlertAlertDate = d.AlertDate _
                                                      , .TmpCMAlerteModDate = dtModDate _
                                                      , .TmpCMAlerteModUser = sModUser _
                                                      , .TmpCMAlertUserControl = iUserTimeKey}).ToArray()
                If (Not otblTempCMAlerts Is Nothing AndAlso otblTempCMAlerts.Count() > 0) Then
                    db.tblTempCMAlerts.InsertAllOnSubmit(otblTempCMAlerts)
                    db.SubmitChanges()
                    Dim iQuery As IQueryable(Of LTS.vCMAlert)
                    iQuery = (From d In db.tblTempCMAlerts Where d.TmpCMAlertUserControl = iUserTimeKey
                              Select New LTS.vCMAlert With {.CarrierControl = d.TmpCMAlertCarrierControl _
                              , .CarrierNumber = d.TmpCMAlertCarrierNumber _
                              , .CarrierName = d.TmpCMAlertCarrierName _
                              , .Exposure = d.TmpCMAlertExposure _
                              , .Message = d.TmpCMAlertMessage _
                              , .ProductValue = d.TmpCMAlertProductValue _
                              , .AlertDate = d.TmpCMAlertAlertDate})

                    'db.Log = New DebugTextWriter
                    Dim filterWhere = ""
                    ApplyAllFilters(iQuery, filters, filterWhere)
                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                    Try
                        db.tblTempCMAlerts.DeleteAllOnSubmit(otblTempCMAlerts)
                        db.SubmitChanges()
                    Catch ex As Exception
                        'do nothing system clear old logs will fix this later using old TmpCMAlerteModDate
                    End Try
                    Return oRet
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierInsExpAllShipAlerts365"), db)
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetOutdatedStagingOrderAlerts() As DTO.OutdatedStagingOrderAlert()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                Dim oList As DTO.OutdatedStagingOrderAlert() = (
                From d In db.udfAlertsOutdatedStagingOrders50(Me.Parameters.UserName)
                Select New DTO.OutdatedStagingOrderAlert With {.CompControl = d.CompControl _
                                        , .CreateDate = d.CreateDate _
                                        , .CreateUser = d.CreateUser _
                                        , .DefaultCarrier = d.DefaultCarrier _
                                        , .DefaultCustomer = d.DefaultCustomer _
                                        , .DefaultCustomerName = d.DefaultCustomerName _
                                        , .ImportType = d.ImportType _
                                        , .OrderNumber = d.OrderNumber _
                                        , .PODate = d.PODate _
                                        , .PONumber = d.PONumber _
                                        , .PRONumber = d.PRONumber _
                                        , .ShipDate = d.ShipDate _
                                        , .VendorNumber = d.VendorNumber _
                                        , .Warnings = d.Warnings}).ToArray

                Return oList



            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' Read current Oudated Staging Order Alerts, from Order Preview
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.0.001 on 10/16/2021  new 365 version
    '''     modified to use All filter data and IQueryable
    '''     uses AlertOutdatedStagingDays global parameter to determine how far back from create date to start tracking
    '''     uses POHDRDefaultCustomer to apply user company secirity
    '''     (Note: if POHDRDefaultCustomer is blank or zero this alert and security may not work as expected)
    ''' </remarks>
    Public Function GetOutdatedStagingOrderAlerts365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCMAlert()
        Dim oRet() As LTS.vCMAlert
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                '365 we do not filter by carrier control, users can select carrier name or carrier number
                'If CarrierControl Then CarrierFilter = CarrierControl
                Dim oECarrierInsExpPerShipAlerts = From d In db.spGetCarrInsExpAllShipAlerts(CarrierFilter, Me.Parameters.UserName) Select d
                Dim iQuery As IQueryable(Of LTS.vCMAlert)
                iQuery = (From d In db.udfAlertsOutdatedStagingOrders50(Me.Parameters.UserName)
                          Select New LTS.vCMAlert With {.CompControl = d.CompControl _
                                        , .BookModDate = d.CreateDate _
                                        , .BookModUser = d.CreateUser _
                                        , .CarrierNumber = d.DefaultCarrier _
                                        , .CompNumber = d.DefaultCustomer _
                                        , .CompName = d.DefaultCustomerName _
                                        , .Type = d.ImportType _
                                        , .BookCarrOrderNumber = d.OrderNumber _
                                        , .AlertDate1 = d.PODate _
                                        , .BookLoadPONumber = d.PONumber _
                                        , .BookProNumber = d.PRONumber _
                                        , .BookDateLoad = d.ShipDate _
                                        , .VendorNumber = d.VendorNumber _
                                        , .Message = d.Warnings})

                db.Log = New DebugTextWriter
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOutdatedStagingOrderAlerts365"), db)
            End Try

            Return oRet

        End Using
    End Function


    Public Function GetOutdatedNStatusOrderAlerts() As DTO.OutdatedNStatusOrderAlert()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                Dim oList As DTO.OutdatedNStatusOrderAlert() = (
                From d In db.udfAlertsOutdatedNStatusOrders50(Me.Parameters.UserName)
                Select New DTO.OutdatedNStatusOrderAlert With {.BookControl = d.BookControl _
                                        , .BookDateLoad = d.BookDateLoad _
                                        , .BookProNumber = d.BookProNumber _
                                        , .CompControl = d.CompControl _
                                        , .CompNumber = d.CompNumber _
                                        , .QueryDate = d.QueryDate}).ToArray

                Return oList



            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Read current Oudated N Status Order Alerts
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.0.001 on 10/16/2021  new 365 version
    '''     modified to use All filter data and IQueryable
    '''     BookTranCode = 'N'
    '''     uses AlertOutedatedNStatusDaysOut global parameter to determine how far back from BookDateLoad to start tracking
    '''     when BookDateDelivered is 
    '''     and CarrierIgnoreTariff = 0	(false)
    ''' </remarks>
    Public Function GetOutdatedNStatusOrderAlerts365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCMAlert()
        Dim oRet() As LTS.vCMAlert
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iTestValue As Integer = 0
                Dim iQuery As IQueryable(Of LTS.vCMAlert)
                iQuery = (From d In db.udfAlertsOutdatedNStatusOrders50(Me.Parameters.UserName)
                          Select New LTS.vCMAlert With {.BookControl = d.BookControl _
                                        , .BookDateLoad = d.BookDateLoad _
                                        , .BookProNumber = d.BookProNumber _
                                        , .CompControl = If(d.CompControl, 0) _
                                        , .CompNumber = If(Integer.TryParse(d.CompNumber, iTestValue), iTestValue, 0) _
                                        , .AlertDate = d.QueryDate})

                db.Log = New DebugTextWriter
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOutdatedNStatusOrderAlerts365"), db)
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetCarrInsExpiredAlerts(Optional ByVal CarrierControl As Integer = 0) As DTO.CarrInsExpiredAlert()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                If CarrierControl Then CarrierFilter = CarrierControl

                Dim oList As DTO.CarrInsExpiredAlert() = (
                From d In db.udfAlertsCarrInsExpired(CarrierFilter, Me.Parameters.UserName)
                Select New DTO.CarrInsExpiredAlert With {.AlertDate = d.AlertDate _
                                        , .CarrierControl = d.CarrierControl _
                                        , .CarrierNumber = d.CarrierNumber _
                                        , .CarrierQualInsuranceDate = d.CarrierQualInsuranceDate}).ToArray

                Return oList



            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Read current Carrier Insurance Expired Alerts
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.0.001 on 10/16/2021  new 365 version
    '''     modified to use All filter data and IQueryable
    ''' </remarks>
    Public Function GetCarrInsExpiredAlerts365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCMAlert()
        Dim oRet() As LTS.vCMAlert
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim CarrierFilter As New System.Nullable(Of Integer)
                '365 we do not filter by carrier control, users can select carrier name or carrier number
                'If CarrierControl Then CarrierFilter = CarrierControl
                Dim iQuery As IQueryable(Of LTS.vCMAlert)
                iQuery = (From d In db.udfAlertsCarrInsExpired365(CarrierFilter, Me.Parameters.UserName)
                          Select New LTS.vCMAlert With {.AlertDate = d.AlertDate _
                                        , .CarrierControl = d.CarrierControl _
                                        , .CarrierNumber = d.CarrierNumber _
                                        , .CarrierName = d.CarrierName _
                                        , .AlertDate1 = d.CarrierQualInsuranceDate})

                db.Log = New DebugTextWriter
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrInsExpiredAlerts365"), db)
            End Try

            Return oRet

        End Using
    End Function


    Public Function GetOutdatedNoLaneAlerts() As DTO.OutdatedNoLaneAlert()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                Dim oList As DTO.OutdatedNoLaneAlert() = (
                From d In db.udfAlertsOutdatedNoLanes(Me.Parameters.UserName)
                Select New DTO.OutdatedNoLaneAlert With {.CompControl = d.CompControl _
                                        , .CompName = d.CompName _
                                        , .Description = d.Description _
                                        , .ItemNumber = d.ItemNumber _
                                        , .POHNLCreateDate = d.POHNLCreateDate _
                                        , .POHNLDefaultCustomer = d.POHNLDefaultCustomer _
                                        , .POHNLnumber = d.POHNLnumber _
                                        , .POHNLOrderNumber = d.POHNLOrderNumber _
                                        , .POHNLPOdate = d.POHNLPOdate _
                                        , .POHNLReqDate = d.POHNLReqDate _
                                        , .POHNLShipdate = d.POHNLShipdate _
                                        , .POHNLvendor = d.POHNLvendor _
                                        , .QtyOrdered = d.QtyOrdered}).ToArray

                Return oList



            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetPOsWaitingAlerts() As DTO.POsWaitingAlert()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                Dim oList As DTO.POsWaitingAlert() = (
                From d In db.udfAlertsPOsWaiting365(Me.Parameters.UserName)
                Select New DTO.POsWaitingAlert With {.CompControl = d.CompControl _
                                        , .CreateDate = d.CreateDate _
                                        , .CreateUser = d.CreateUser _
                                        , .DefaultCarrier = d.DefaultCarrier _
                                        , .DefaultCustomer = d.DefaultCustomer _
                                        , .DefaultCustomerName = d.DefaultCustomerName _
                                        , .ImportType = d.ImportType _
                                        , .OrderNumber = d.OrderNumber _
                                        , .PODate = d.PODate _
                                        , .PONumber = d.PONumber _
                                        , .PRONumber = d.PRONumber _
                                        , .ShipDate = d.ShipDate _
                                        , .VendorNumber = d.VendorNumber _
                                        , .Warnings = d.Warnings}).ToArray

                Return oList



            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetPOsWaitingAlerts365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vCMAlert()
        Dim oRet() As LTS.vCMAlert
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vCMAlert)
                iQuery = (From d In db.udfAlertsPOsWaiting365(Me.Parameters.UserName)
                          Select New LTS.vCMAlert With {
                                          .BookControl = d.POHDRControl _
                                        , .CompControl = d.CompControl _
                                        , .BookModDate = d.CreateDate _
                                        , .BookModUser = d.CreateUser _
                                        , .CarrierNumber = d.DefaultCarrier _
                                        , .CompNumber = d.DefaultCustomer _
                                        , .CompName = d.CompName _
                                        , .Type = d.ImportType _
                                        , .BookCarrOrderNumber = d.OrderNumber _
                                        , .AlertDate1 = d.PODate _
                                        , .BookLoadPONumber = d.PONumber _
                                        , .BookProNumber = d.PRONumber _
                                        , .BookDateLoad = d.ShipDate _
                                        , .VendorNumber = d.VendorNumber _
                                        , .Message = d.Warnings})

                db.Log = New DebugTextWriter
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPOsWaitingAlerts365"), db)
            End Try

            Return oRet

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

#End Region

End Class

Public Class NGLSystemLogData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)

        Me.SourceClass = "NGLSystemLogData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASSYSDataContext(ConnectionString)
                _LinqTable = Nothing
                _LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function


    Public Function GetTaskLogs(Optional ByVal Count As Integer = 1000) As DTO.tblTaskLog()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                If Count = 0 Then Count = 1000
                Dim oList As DTO.tblTaskLog() = (
                From d In db.tblTaskLogs()
                Order By d.TaskControl Descending
                Select New DTO.tblTaskLog With {.TaskCommand = d.TaskCommand _
                                        , .TaskControl = d.TaskControl _
                                        , .TaskDesc = d.TaskDesc _
                                        , .TaskLastRanOn = d.TaskLastRanOn _
                                        , .TaskMessage = d.TaskMessage _
                                        , .TaskName = d.TaskName _
                                        , .TaskRanOn = d.TaskRanOn _
                                        , .TaskType = d.TaskType}).Take(Count).ToArray

                Return oList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetSystemErrorLogs(Optional ByVal Count As Integer = 1000) As DTO.tblSystemError()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                If Count = 0 Then Count = 1000
                Dim oList As DTO.tblSystemError() = (
                From d In db.tblSystemErrors()
                Order By d.ErrID Descending
                Select New DTO.tblSystemError With {.CurrentDate = d.CurrentDate _
                                        , .CurrentUser = d.CurrentUser _
                                        , .ErrID = d.ErrID _
                                        , .ErrorAlertSent = d.ErrorAlertSent _
                                        , .ErrorAlertSentDate = d.ErrorAlertSentDate _
                                        , .ErrorLineNbr = d.ErrorLineNbr _
                                        , .ErrorNumber = d.ErrorNumber _
                                        , .ErrorProcedure = d.ErrorProcedure _
                                        , .ErrorSeverity = d.ErrorSeverity _
                                        , .ErrorState = d.ErrorState _
                                        , .Message = d.Message _
                                        , .Record = d.Record}).Take(Count).ToArray

                Return oList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetApplicaitonLogs(Optional ByVal Count As Integer = 1000) As DTO.tblLog()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                If Count = 0 Then Count = 1000
                Dim oList As DTO.tblLog() = (
                From d In db.tblLogs()
                Order By d.LogControl Descending
                Select New DTO.tblLog With {.LogControl = d.LogControl _
                                        , .LogMessage = d.LogMessage _
                                        , .LogSource = d.LogSource _
                                        , .LogTime = d.LogTime _
                                        , .LogUser = d.LogUser}).Take(Count).ToArray

                Return oList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Sub AddApplicaitonLog(ByVal Log As DTO.tblLog)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim nObject = New LTS.tblLog With {.LogMessage = Log.LogMessage _
                                  , .LogSource = Log.LogSource _
                                  , .LogTime = Date.Now _
                                  , .LogUser = Me.Parameters.UserName}

                db.tblLogs.InsertOnSubmit(nObject)

                db.SubmitChanges()

            Catch ex As Exception
                'We ignore all errors while saving log data
            End Try
        End Using
    End Sub

    Public Sub AddApplicaitonLog(ByVal message As String, ByVal source As String)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim nObject = New LTS.tblLog With {.LogMessage = message _
                                  , .LogSource = Left(source, 50) _
                                  , .LogTime = Date.Now _
                                  , .LogUser = Me.Parameters.UserName}

                db.tblLogs.InsertOnSubmit(nObject)

                db.SubmitChanges()

            Catch ex As Exception
                'We ignore all errors while saving log data
            End Try
        End Using
    End Sub

    Public Function GetEDITransactionLogs(Optional ByVal Count As Integer = 1000) As DTO.EDITransaction()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                If Count = 0 Then Count = 1000
                Dim oList As DTO.EDITransaction() = (
                From d In db.vEDITransactions()
                Order By d.tblEDITransControl Descending
                Select New DTO.EDITransaction With {.BookCarrOrderNumber = d.BookCarrOrderNumber _
                                        , .BookConsPrefix = d.BookConsPrefix _
                                        , .BookDestAddress1 = d.BookDestAddress1 _
                                        , .BookDestCity = d.BookDestCity _
                                        , .BookDestCountry = d.BookDestCountry _
                                        , .BookDestName = d.BookDestName _
                                        , .BookDestState = d.BookDestState _
                                        , .BookDestZip = d.BookDestZip _
                                        , .BookOrderSequence = d.BookOrderSequence _
                                        , .BookOrigAddress1 = d.BookOrigAddress1 _
                                        , .BookOrigCity = d.BookOrigCity _
                                        , .BookOrigCountry = d.BookOrigCountry _
                                        , .BookOrigName = d.BookOrigName _
                                        , .BookOrigState = d.BookOrigState _
                                        , .BookOrigZip = d.BookOrigZip _
                                        , .BookProNumber = d.BookProNumber _
                                        , .CarrierName = d.CarrierName _
                                        , .CarrierNumber = d.CarrierNumber _
                                        , .CompName = d.CompName _
                                        , .CompNatName = d.CompNatName _
                                        , .CompNatNumber = d.CompNatNumber _
                                        , .CompNumber = d.CompNumber _
                                        , .tblEDITransCarrierSCAC = d.tblEDITransCarrierSCAC _
                                        , .tblEDITransControl = d.tblEDITransControl _
                                        , .tblEDITransDate = d.tblEDITransDate _
                                        , .tblEDITransGSSequence = d.tblEDITransGSSequence _
                                        , .tblEDITransISASequence = d.tblEDITransISASequence _
                                        , .tblEDITransLoadNumber = d.tblEDITransLoadNumber _
                                        , .tblEDITransMessage = d.tblEDITransMessage _
                                        , .tblEDITransReceiverCode = d.tblEDITransReceiverCode _
                                        , .tblEDITransSenderCode = d.tblEDITransSenderCode _
                                        , .tblEDITransXaction = d.tblEDITransXaction}).Take(Count).ToArray

                Return oList

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function


#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

#End Region

End Class

Public Class NGLtblStopData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblStops
        Me.LinqDB = db
        Me.SourceClass = "NGLtblStopData"
    End Sub

#End Region

#Region " Properties "
    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblStops
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property
#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DTO.DTOBaseClass
        Return GettblStopFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DTO.DTOBaseClass()
        Return GettblStopsFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DTO.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim tblStop As DTO.tblStop
                If LowerControl <> 0 Then
                    tblStop = (From d In db.tblStops Where d.StopControl >= LowerControl Order By d.StopControl Select selectDTOData(d, db)).FirstOrDefault()
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblStop = (From d In db.tblStops Order By d.StopControl Select selectDTOData(d, db)).FirstOrDefault()
                End If
                Return tblStop
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DTO.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'Get the first record that matches the provided criteria
                Dim tblStop As DTO.tblStop = (From d In db.tblStops Where d.StopControl < CurrentControl Order By d.StopControl Descending Select selectDTOData(d, db)).FirstOrDefault()
                Return tblStop
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DTO.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'Get the first record that matches the provided criteria
                Dim tblStop As DTO.tblStop = (From d In db.tblStops Where d.StopControl > CurrentControl Order By d.StopControl Select selectDTOData(d, db)).FirstOrDefault()
                Return tblStop
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DTO.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim tblStop As DTO.tblStop
                If UpperControl <> 0 Then
                    tblStop = (From d In db.tblStops Where d.StopControl >= UpperControl Order By d.StopControl Select selectDTOData(d, db)).FirstOrDefault()
                Else
                    'Zero indicates that we should get the hightest stopcontrol record
                    tblStop = (From d In db.tblStops Order By d.StopControl Descending Select selectDTOData(d, db)).FirstOrDefault()
                End If
                Return tblStop
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblStopFiltered(ByVal Control As Integer) As DTO.tblStop
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim tblStop As DTO.tblStop = (From d In db.tblStops Where d.StopControl = Control Select selectDTOData(d, db)).First()
                Return tblStop
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GettblStopsFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblStop()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record
                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblStop")
                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize
                'Return all the contacts that match the criteria sorted by name
                Dim tblStops() As DTO.tblStop = (From d In db.tblStops Order By d.StopControl Select selectDTOData(d, db, page, intPageCount, intRecordCount, pagesize)).Skip(intSkip).Take(pagesize).ToArray()
                Return tblStops
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function SaveStopData(ByVal aStopData As LTS.tblStop) As Boolean
        Dim blnRet As Boolean = False
        If aStopData Is Nothing Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                db.tblStops.InsertOnSubmit(aStopData)
                db.SubmitChanges()

                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveStopData"), db)
            End Try
        End Using
        Return blnRet
    End Function


    ''' <summary>
    ''' Updates the Latitude and Longitude values for the provided StopControl
    ''' </summary>
    ''' <param name="StopControl"></param>
    ''' <param name="Latitude"></param>
    ''' <param name="Longitude"></param>
    ''' <remarks>
    ''' Added By LVV on 10/4/19 Bing Maps
    ''' </remarks>
    Public Sub UpdateStopCoordinates(ByVal StopControl As Integer, ByVal Latitude As Double, ByVal Longitude As Double)
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim lts = db.tblStops.Where(Function(x) x.StopControl = StopControl).FirstOrDefault()
                If Not lts Is Nothing Then
                    lts.StopLatitude = Latitude
                    lts.StopLongitude = Longitude
                    lts.StopModDate = Date.Now
                    lts.StopModUser = Parameters.UserName
                    db.SubmitChanges()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateStopCoordinates"), db)
            End Try
        End Using
    End Sub

    Public Function GetStopsFilteredWildCard(ByVal strFilter As String) As LTS.tblStop()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim wildCard = String.Format("%{0}%", strFilter)
                Dim stops = (From d In db.tblStops
                             Where
                                 SqlClient.SqlMethods.Like(d.StopName, wildCard) _
                                 Or SqlClient.SqlMethods.Like(d.StopAddress1, wildCard) _
                                 Or SqlClient.SqlMethods.Like(d.StopCity, wildCard) _
                                 Or SqlClient.SqlMethods.Like(d.StopState, wildCard) _
                                 Or SqlClient.SqlMethods.Like(d.StopZip, wildCard) _
                                 Or SqlClient.SqlMethods.Like(d.StopCountry, wildCard) _
                                 Or SqlClient.SqlMethods.Like(d.StopLatitude, wildCard) _
                                 Or SqlClient.SqlMethods.Like(d.StopLongitude, wildCard)
                             Select d).ToArray()
                Return stops
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetStopsFilteredWildCard"), db)
            End Try
            Return Nothing
        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblStop)
        'Create New Record
        Return New LTS.tblStop With {.StopControl = d.StopControl _
                                    , .StopAddress1 = d.StopAddress1 _
                                    , .StopCity = d.StopCity _
                                    , .StopState = d.StopState _
                                    , .StopZip = d.StopZip _
                                    , .StopLatitude = d.StopLatitude _
                                    , .StopLongitude = d.StopLongitude _
                                    , .StopModDate = Date.Now _
                                    , .StopModUser = Parameters.UserName _
                                    , .StopUpdated = If(d.StopUpdated Is Nothing, New Byte() {}, d.StopUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblStopFiltered(Control:=CType(LinqTable, LTS.tblStop).StopControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim source As LTS.tblStop = TryCast(LinqTable, LTS.tblStop)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblStops
                       Where d.StopControl = source.StopControl
                       Select New DTO.QuickSaveResults With {.Control = d.StopControl _
                                                            , .ModDate = d.StopModDate _
                                                            , .ModUser = d.StopModUser _
                                                            , .Updated = d.StopUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblStop, ByVal db As NGLMASSYSDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblStop
        Return New DTO.tblStop With {.StopControl = d.StopControl _
                                   , .StopName = d.StopName _
                                   , .StopAddress1 = d.StopAddress1 _
                                   , .StopCity = d.StopCity _
                                   , .StopState = d.StopState _
                                   , .StopZip = d.StopZip _
                                   , .StopCountry = d.StopCountry _
                                   , .StopLatitude = d.StopLatitude _
                                   , .StopLongitude = d.StopLongitude _
                                   , .StopModDate = d.StopModDate _
                                   , .StopModUser = d.StopModUser _
                                   , .StopUpdated = d.StopUpdated.ToArray() _
                                   , .Page = page _
                                   , .Pages = pagecount _
                                   , .RecordCount = recordcount _
                                   , .PageSize = pagesize}
    End Function

#End Region

End Class

Public Class NGLtblStopDistanceData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblStopDistances
        Me.LinqDB = db
        Me.SourceClass = "NGLtblStopDistanceData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblStopDistances
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GettblStopDistanceFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblStopDistancesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' Enter a valid StopDistanceFromStopControl for the FKControl parameter or 
    ''' zero to ignore 
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim tblStopDistance As DTO.tblStopDistance

                If LowerControl <> 0 Then
                    tblStopDistance = (
                   From d In db.tblStopDistances
                   Where d.StopDistanceControl >= LowerControl _
                   And
                   (FKControl = 0 OrElse d.StopDistanceFromStopControl = FKControl)
                   Order By d.StopDistanceControl
                   Select New DTO.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                               , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                               , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                               , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                               , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                               , .StopDistanceModDate = d.StopDistanceModDate _
                                               , .StopDistanceModUser = d.StopDistanceModUser _
                                               , .StopDistanceUpdated = d.StopDistanceUpdated.ToArray()}).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblStopDistance = (
                   From d In db.tblStopDistances
                   Where (FKControl = 0 OrElse d.StopDistanceFromStopControl = FKControl)
                   Order By d.StopDistanceControl
                   Select New DTO.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                               , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                               , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                               , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                               , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                               , .StopDistanceModDate = d.StopDistanceModDate _
                                               , .StopDistanceModUser = d.StopDistanceModUser _
                                               , .StopDistanceUpdated = d.StopDistanceUpdated.ToArray()}).FirstOrDefault
                End If



                Return tblStopDistance

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' Enter a valid StopDistanceFromStopControl for the FKControl parameter or 
    ''' zero to ignore 
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblStopDistance As DTO.tblStopDistance = (
                From d In db.tblStopDistances
                Where d.StopDistanceControl < CurrentControl _
                And
                (FKControl = 0 OrElse d.StopDistanceFromStopControl = FKControl)
                Order By d.StopDistanceControl Descending
                Select New DTO.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                               , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                               , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                               , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                               , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                               , .StopDistanceModDate = d.StopDistanceModDate _
                                               , .StopDistanceModUser = d.StopDistanceModUser _
                                               , .StopDistanceUpdated = d.StopDistanceUpdated.ToArray()}).FirstOrDefault


                Return tblStopDistance

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' Enter a valid StopDistanceFromStopControl for the FKControl parameter or 
    ''' zero to ignore 
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblStopDistance As DTO.tblStopDistance = (
                From d In db.tblStopDistances
                Where d.StopDistanceControl > CurrentControl _
                And
                (FKControl = 0 OrElse d.StopDistanceFromStopControl = FKControl)
                Order By d.StopDistanceControl
                Select New DTO.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                               , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                               , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                               , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                               , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                               , .StopDistanceModDate = d.StopDistanceModDate _
                                               , .StopDistanceModUser = d.StopDistanceModUser _
                                               , .StopDistanceUpdated = d.StopDistanceUpdated.ToArray()}).FirstOrDefault


                Return tblStopDistance

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' Enter a valid StopDistanceFromStopControl for the FKControl parameter or 
    ''' zero to ignore 
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim tblStopDistance As DTO.tblStopDistance

                If UpperControl <> 0 Then

                    tblStopDistance = (
                    From d In db.tblStopDistances
                    Where d.StopDistanceControl >= UpperControl _
                    And
                    (FKControl = 0 OrElse d.StopDistanceFromStopControl = FKControl)
                    Order By d.StopDistanceControl
                    Select New DTO.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                                , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                                , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                                , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                                , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                                , .StopDistanceModDate = d.StopDistanceModDate _
                                                , .StopDistanceModUser = d.StopDistanceModUser _
                                                , .StopDistanceUpdated = d.StopDistanceUpdated.ToArray()}).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest StopDistancecontrol record
                    tblStopDistance = (
                    From d In db.tblStopDistances
                    Where (FKControl = 0 OrElse d.StopDistanceFromStopControl = FKControl)
                    Order By d.StopDistanceControl Descending
                    Select New DTO.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                                , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                                , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                                , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                                , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                                , .StopDistanceModDate = d.StopDistanceModDate _
                                                , .StopDistanceModUser = d.StopDistanceModUser _
                                                , .StopDistanceUpdated = d.StopDistanceUpdated.ToArray()}).FirstOrDefault

                End If


                Return tblStopDistance

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStopDistanceFiltered(ByVal Control As Integer) As DTO.tblStopDistance
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblStopDistance As DTO.tblStopDistance = (
                From d In db.tblStopDistances
                Where
                    d.StopDistanceControl = Control
                Select New DTO.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                                , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                                , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                                , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                                , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                                , .StopDistanceModDate = d.StopDistanceModDate _
                                                , .StopDistanceModUser = d.StopDistanceModUser _
                                                , .StopDistanceUpdated = d.StopDistanceUpdated.ToArray()}).First


                Return tblStopDistance

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblStopDistancesFiltered(Optional ByVal StopDistanceFromStopControl As Integer = 0, Optional ByVal StopDistanceToStopControl As Integer = 0, Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblStopDistance()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    If StopDistanceFromStopControl <> 0 And StopDistanceToStopControl <> 0 Then
                        intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblStopDistance Where StopDistanceFromStopControl = " & StopDistanceFromStopControl & " AND  StopDistanceToStopControl = " & StopDistanceToStopControl)
                    ElseIf StopDistanceFromStopControl <> 0 Then
                        intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblStopDistance Where StopDistanceFromStopControl = " & StopDistanceFromStopControl)
                    ElseIf StopDistanceToStopControl <> 0 Then
                        intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblStopDistance Where StopDistanceToStopControl = " & StopDistanceToStopControl)
                    Else
                        intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblStopDistance")
                    End If

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblStopDistances() As DTO.tblStopDistance = (
                From d In db.tblStopDistances
                Where (StopDistanceFromStopControl = 0 OrElse d.StopDistanceFromStopControl = StopDistanceFromStopControl) _
                And
                (StopDistanceToStopControl = 0 OrElse d.StopDistanceToStopControl = StopDistanceToStopControl)
                Order By d.StopDistanceControl
                Select New DTO.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                            , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                            , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                            , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                            , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                            , .StopDistanceModDate = d.StopDistanceModDate _
                                            , .StopDistanceModUser = d.StopDistanceModUser _
                                            , .Page = page _
                                            , .Pages = intPageCount _
                                            , .RecordCount = intRecordCount _
                                            , .PageSize = pagesize _
                                            , .StopDistanceUpdated = d.StopDistanceUpdated.ToArray()}).Skip(intSkip).Take(pagesize).ToArray()
                Return tblStopDistances

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblStopDistance)
        'Create New Record
        Return New LTS.tblStopDistance With {.StopDistanceControl = d.StopDistanceControl _
                                            , .StopDistanceFromStopControl = d.StopDistanceFromStopControl _
                                            , .StopDistanceToStopControl = d.StopDistanceToStopControl _
                                            , .StopDistanceRoadMiles = d.StopDistanceRoadMiles _
                                            , .StopDistanceDirectMiles = d.StopDistanceDirectMiles _
                                            , .StopDistanceModDate = Date.Now _
                                            , .StopDistanceModUser = Parameters.UserName _
                                            , .StopDistanceUpdated = If(d.StopDistanceUpdated Is Nothing, New Byte() {}, d.StopDistanceUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblStopDistanceFiltered(Control:=CType(LinqTable, LTS.tblStopDistance).StopDistanceControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim source As LTS.tblStopDistance = TryCast(LinqTable, LTS.tblStopDistance)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblStopDistances
                       Where d.StopDistanceControl = source.StopDistanceControl
                       Select New DTO.QuickSaveResults With {.Control = d.StopDistanceControl _
                                                            , .ModDate = d.StopDistanceModDate _
                                                            , .ModUser = d.StopDistanceModUser _
                                                            , .Updated = d.StopDistanceUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class

Public Class NGLtblDistanceQueueData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblDistanceQueues
        Me.LinqDB = db
        Me.SourceClass = "NGLtblDistanceQueueData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblDistanceQueues
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GettblDistanceQueueFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblDistanceQueuesFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim tblDistanceQueue As DTO.tblDistanceQueue

                If LowerControl <> 0 Then
                    tblDistanceQueue = (
                   From d In db.tblDistanceQueues
                   Where d.DistanceQueueControl >= LowerControl
                   Order By d.DistanceQueueControl
                   Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                               , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                               , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                               , .DistanceQueueComplete = d.DistanceQueueComplete _
                                               , .DistanceQueueMessage = d.DistanceQueueMessage _
                                               , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                               , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                               , .DistanceQueueModDate = d.DistanceQueueModDate _
                                               , .DistanceQueueModUser = d.DistanceQueueModUser _
                                               , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblDistanceQueue = (
                   From d In db.tblDistanceQueues
                   Order By d.DistanceQueueControl
                   Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                               , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                               , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                               , .DistanceQueueComplete = d.DistanceQueueComplete _
                                               , .DistanceQueueMessage = d.DistanceQueueMessage _
                                               , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                               , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                               , .DistanceQueueModDate = d.DistanceQueueModDate _
                                               , .DistanceQueueModUser = d.DistanceQueueModUser _
                                               , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).FirstOrDefault
                End If



                Return tblDistanceQueue

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblDistanceQueue As DTO.tblDistanceQueue = (
                From d In db.tblDistanceQueues
                Where d.DistanceQueueControl < CurrentControl
                Order By d.DistanceQueueControl Descending
                Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                                , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                                , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                                , .DistanceQueueComplete = d.DistanceQueueComplete _
                                                , .DistanceQueueMessage = d.DistanceQueueMessage _
                                                , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                                , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                                , .DistanceQueueModDate = d.DistanceQueueModDate _
                                                , .DistanceQueueModUser = d.DistanceQueueModUser _
                                                , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).FirstOrDefault


                Return tblDistanceQueue

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblDistanceQueue As DTO.tblDistanceQueue = (
                From d In db.tblDistanceQueues
                Where d.DistanceQueueControl > CurrentControl
                Order By d.DistanceQueueControl
                Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                                , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                                , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                                , .DistanceQueueComplete = d.DistanceQueueComplete _
                                                , .DistanceQueueMessage = d.DistanceQueueMessage _
                                                , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                                , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                                , .DistanceQueueModDate = d.DistanceQueueModDate _
                                                , .DistanceQueueModUser = d.DistanceQueueModUser _
                                                , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).FirstOrDefault


                Return tblDistanceQueue

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is ignored
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim tblDistanceQueue As DTO.tblDistanceQueue

                If UpperControl <> 0 Then

                    tblDistanceQueue = (
                    From d In db.tblDistanceQueues
                    Where d.DistanceQueueControl >= UpperControl
                    Order By d.DistanceQueueControl
                    Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                                , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                                , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                                , .DistanceQueueComplete = d.DistanceQueueComplete _
                                                , .DistanceQueueMessage = d.DistanceQueueMessage _
                                                , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                                , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                                , .DistanceQueueModDate = d.DistanceQueueModDate _
                                                , .DistanceQueueModUser = d.DistanceQueueModUser _
                                                , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest DistanceQueuecontrol record
                    tblDistanceQueue = (
                    From d In db.tblDistanceQueues
                    Order By d.DistanceQueueControl Descending
                    Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                                , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                                , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                                , .DistanceQueueComplete = d.DistanceQueueComplete _
                                                , .DistanceQueueMessage = d.DistanceQueueMessage _
                                                , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                                , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                                , .DistanceQueueModDate = d.DistanceQueueModDate _
                                                , .DistanceQueueModUser = d.DistanceQueueModUser _
                                                , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).FirstOrDefault

                End If


                Return tblDistanceQueue

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetQueueToRun() As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblDistanceQueue As DTO.tblDistanceQueue = (
                From d In db.tblDistanceQueues
                Where d.DistanceQueueComplete = False
                Order By d.DistanceQueueControl
                Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                                , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                                , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                                , .DistanceQueueComplete = d.DistanceQueueComplete _
                                                , .DistanceQueueMessage = d.DistanceQueueMessage _
                                                , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                                , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                                , .DistanceQueueModDate = d.DistanceQueueModDate _
                                                , .DistanceQueueModUser = d.DistanceQueueModUser _
                                                , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).First


                Return tblDistanceQueue

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblDistanceQueueFiltered(ByVal Control As Integer) As DTO.tblDistanceQueue
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblDistanceQueue As DTO.tblDistanceQueue = (
                From d In db.tblDistanceQueues
                Where
                    d.DistanceQueueControl = Control
                Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                                , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                                , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                                , .DistanceQueueComplete = d.DistanceQueueComplete _
                                                , .DistanceQueueMessage = d.DistanceQueueMessage _
                                                , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                                , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                                , .DistanceQueueModDate = d.DistanceQueueModDate _
                                                , .DistanceQueueModUser = d.DistanceQueueModUser _
                                                , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).First


                Return tblDistanceQueue

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblDistanceQueuesFiltered(Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblDistanceQueue()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblDistanceQueue")

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblDistanceQueues() As DTO.tblDistanceQueue = (
                From d In db.tblDistanceQueues
                Order By d.DistanceQueueControl
                Select New DTO.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                                , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                                , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                                , .DistanceQueueComplete = d.DistanceQueueComplete _
                                                , .DistanceQueueMessage = d.DistanceQueueMessage _
                                                , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                                , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                                , .DistanceQueueModDate = d.DistanceQueueModDate _
                                                , .DistanceQueueModUser = d.DistanceQueueModUser _
                                                , .Page = page _
                                                , .Pages = intPageCount _
                                                , .RecordCount = intRecordCount _
                                                , .PageSize = pagesize _
                                                , .DistanceQueueUpdated = d.DistanceQueueUpdated.ToArray()}).Skip(intSkip).Take(pagesize).ToArray()
                Return tblDistanceQueues

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblDistanceQueue)
        'Create New Record
        Return New LTS.tblDistanceQueue With {.DistanceQueueControl = d.DistanceQueueControl _
                                                , .DistanceQueueStartDate = d.DistanceQueueStartDate _
                                                , .DistanceQueueEndDate = d.DistanceQueueEndDate _
                                                , .DistanceQueueComplete = d.DistanceQueueComplete _
                                                , .DistanceQueueMessage = d.DistanceQueueMessage _
                                                , .DistanceQueueRunAll = d.DistanceQueueRunAll _
                                                , .DistanceQueueUseLatLong = d.DistanceQueueUseLatLong _
                                                , .DistanceQueueModDate = Date.Now _
                                                , .DistanceQueueModUser = Parameters.UserName _
                                                , .DistanceQueueUpdated = If(d.DistanceQueueUpdated Is Nothing, New Byte() {}, d.DistanceQueueUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblDistanceQueueFiltered(Control:=CType(LinqTable, LTS.tblDistanceQueue).DistanceQueueControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim source As LTS.tblDistanceQueue = TryCast(LinqTable, LTS.tblDistanceQueue)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblDistanceQueues
                       Where d.DistanceQueueControl = source.DistanceQueueControl
                       Select New DTO.QuickSaveResults With {.Control = d.DistanceQueueControl _
                                                            , .ModDate = d.DistanceQueueModDate _
                                                            , .ModUser = d.DistanceQueueModUser _
                                                            , .Updated = d.DistanceQueueUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class

Public Class NGLtblDistanceQueueDetailData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblDistanceQueueDetails
        Me.LinqDB = db
        Me.SourceClass = "NGLtblDistanceQueueDetailData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblDistanceQueueDetails
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GettblDistanceQueueDetailFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GettblDistanceQueueDetailsFiltered()
    End Function

    ''' <summary>
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' Enter a valid DistanceQueueDetailDistanceQueueControl for the FKControl parameter or 
    ''' zero to ignore 
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim tblDistanceQueueDetail As DTO.tblDistanceQueueDetail

                If LowerControl <> 0 Then
                    tblDistanceQueueDetail = (
                   From d In db.tblDistanceQueueDetails
                   Where d.DistanceQueueDetailControl >= LowerControl _
                   And
                   (FKControl = 0 OrElse d.DistanceQueueDetailDistanceQueueControl = FKControl)
                   Order By d.DistanceQueueDetailControl
                   Select New DTO.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                               , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                               , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                               , .DistanceQueueDetailModDate = d.DistanceQueueDetailModDate _
                                               , .DistanceQueueDetailModUser = d.DistanceQueueDetailModUser _
                                               , .DistanceQueueDetailUpdated = d.DistanceQueueDetailUpdated.ToArray()}).FirstOrDefault
                Else
                    'Zero indicates that we should get the record with the lowest control number even if it is below zero
                    tblDistanceQueueDetail = (
                   From d In db.tblDistanceQueueDetails
                   Where (FKControl = 0 OrElse d.DistanceQueueDetailDistanceQueueControl = FKControl)
                   Order By d.DistanceQueueDetailControl
                   Select New DTO.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                               , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                               , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                               , .DistanceQueueDetailModDate = d.DistanceQueueDetailModDate _
                                               , .DistanceQueueDetailModUser = d.DistanceQueueDetailModUser _
                                               , .DistanceQueueDetailUpdated = d.DistanceQueueDetailUpdated.ToArray()}).FirstOrDefault
                End If



                Return tblDistanceQueueDetail

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' Enter a valid DistanceQueueDetailDistanceQueueControl for the FKControl parameter or 
    ''' zero to ignore 
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblDistanceQueueDetail As DTO.tblDistanceQueueDetail = (
                From d In db.tblDistanceQueueDetails
                Where d.DistanceQueueDetailControl < CurrentControl _
                And
                (FKControl = 0 OrElse d.DistanceQueueDetailDistanceQueueControl = FKControl)
                Order By d.DistanceQueueDetailControl Descending
                Select New DTO.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                               , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                               , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                               , .DistanceQueueDetailModDate = d.DistanceQueueDetailModDate _
                                               , .DistanceQueueDetailModUser = d.DistanceQueueDetailModUser _
                                               , .DistanceQueueDetailUpdated = d.DistanceQueueDetailUpdated.ToArray()}).FirstOrDefault


                Return tblDistanceQueueDetail

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' Enter a valid DistanceQueueDetailDistanceQueueControl for the FKControl parameter or 
    ''' zero to ignore 
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the first record that matches the provided criteria
                Dim tblDistanceQueueDetail As DTO.tblDistanceQueueDetail = (
                From d In db.tblDistanceQueueDetails
                Where d.DistanceQueueDetailControl > CurrentControl _
                And
                (FKControl = 0 OrElse d.DistanceQueueDetailDistanceQueueControl = FKControl)
                Order By d.DistanceQueueDetailControl
                Select New DTO.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                               , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                               , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                               , .DistanceQueueDetailModDate = d.DistanceQueueDetailModDate _
                                               , .DistanceQueueDetailModUser = d.DistanceQueueDetailModUser _
                                               , .DistanceQueueDetailUpdated = d.DistanceQueueDetailUpdated.ToArray()}).FirstOrDefault


                Return tblDistanceQueueDetail

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' Enter a valid DistanceQueueDetailDistanceQueueControl for the FKControl parameter or 
    ''' zero to ignore 
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim tblDistanceQueueDetail As DTO.tblDistanceQueueDetail

                If UpperControl <> 0 Then

                    tblDistanceQueueDetail = (
                    From d In db.tblDistanceQueueDetails
                    Where d.DistanceQueueDetailControl >= UpperControl _
                    And
                    (FKControl = 0 OrElse d.DistanceQueueDetailDistanceQueueControl = FKControl)
                    Order By d.DistanceQueueDetailControl
                    Select New DTO.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                                , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                                , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                                , .DistanceQueueDetailModDate = d.DistanceQueueDetailModDate _
                                                , .DistanceQueueDetailModUser = d.DistanceQueueDetailModUser _
                                                , .DistanceQueueDetailUpdated = d.DistanceQueueDetailUpdated.ToArray()}).FirstOrDefault
                Else
                    'Zero indicates that we should get the hightest DistanceQueueDetailcontrol record
                    tblDistanceQueueDetail = (
                    From d In db.tblDistanceQueueDetails
                    Where (FKControl = 0 OrElse d.DistanceQueueDetailDistanceQueueControl = FKControl)
                    Order By d.DistanceQueueDetailControl Descending
                    Select New DTO.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                                , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                                , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                                , .DistanceQueueDetailModDate = d.DistanceQueueDetailModDate _
                                                , .DistanceQueueDetailModUser = d.DistanceQueueDetailModUser _
                                                , .DistanceQueueDetailUpdated = d.DistanceQueueDetailUpdated.ToArray()}).FirstOrDefault

                End If


                Return tblDistanceQueueDetail

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblDistanceQueueDetailFiltered(ByVal Control As Integer) As DTO.tblDistanceQueueDetail
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim tblDistanceQueueDetail As DTO.tblDistanceQueueDetail = (
                From d In db.tblDistanceQueueDetails
                Where
                    d.DistanceQueueDetailControl = Control
                Select New DTO.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                                , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                                , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                                , .DistanceQueueDetailModDate = d.DistanceQueueDetailModDate _
                                                , .DistanceQueueDetailModUser = d.DistanceQueueDetailModUser _
                                                , .DistanceQueueDetailUpdated = d.DistanceQueueDetailUpdated.ToArray()}).First


                Return tblDistanceQueueDetail

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GettblDistanceQueueDetailsFiltered(Optional ByVal DistanceQueueDetailDistanceQueueControl As Integer = 0, Optional ByVal DistanceQueueDetailStopControl As Integer = 0, Optional ByVal page As Integer = 1, Optional ByVal pagesize As Integer = 1000) As DTO.tblDistanceQueueDetail()
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record

                Try
                    If DistanceQueueDetailDistanceQueueControl <> 0 And DistanceQueueDetailStopControl <> 0 Then
                        intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblDistanceQueueDetail Where DistanceQueueDetailDistanceQueueControl = " & DistanceQueueDetailDistanceQueueControl & " AND  DistanceQueueDetailStopControl = " & DistanceQueueDetailStopControl)
                    ElseIf DistanceQueueDetailDistanceQueueControl <> 0 Then
                        intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblDistanceQueueDetail Where DistanceQueueDetailDistanceQueueControl = " & DistanceQueueDetailDistanceQueueControl)
                    ElseIf DistanceQueueDetailStopControl <> 0 Then
                        intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblDistanceQueueDetail Where DistanceQueueDetailStopControl = " & DistanceQueueDetailStopControl)
                    Else
                        intRecordCount = getScalarInteger("select COUNT(*) from dbo.tblDistanceQueueDetail")
                    End If

                Catch ex As Exception
                    'ignore any record count errors
                End Try
                If pagesize < 1 Then pagesize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If page < 1 Then page = 1
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1
                Dim intSkip As Integer = (page - 1) * pagesize

                'Return all the contacts that match the criteria sorted by name
                Dim tblDistanceQueueDetails() As DTO.tblDistanceQueueDetail = (
                From d In db.tblDistanceQueueDetails
                Where (DistanceQueueDetailDistanceQueueControl = 0 OrElse d.DistanceQueueDetailDistanceQueueControl = DistanceQueueDetailDistanceQueueControl) _
                And
                (DistanceQueueDetailStopControl = 0 OrElse d.DistanceQueueDetailStopControl = DistanceQueueDetailStopControl)
                Order By d.DistanceQueueDetailControl
                Select New DTO.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                            , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                            , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                            , .DistanceQueueDetailModDate = d.DistanceQueueDetailModDate _
                                            , .DistanceQueueDetailModUser = d.DistanceQueueDetailModUser _
                                            , .Page = page _
                                            , .Pages = intPageCount _
                                            , .RecordCount = intRecordCount _
                                            , .PageSize = pagesize _
                                            , .DistanceQueueDetailUpdated = d.DistanceQueueDetailUpdated.ToArray()}).Skip(intSkip).Take(pagesize).ToArray()
                Return tblDistanceQueueDetails

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tblDistanceQueueDetail)
        'Create New Record
        Return New LTS.tblDistanceQueueDetail With {.DistanceQueueDetailControl = d.DistanceQueueDetailControl _
                                            , .DistanceQueueDetailDistanceQueueControl = d.DistanceQueueDetailDistanceQueueControl _
                                            , .DistanceQueueDetailStopControl = d.DistanceQueueDetailStopControl _
                                            , .DistanceQueueDetailModDate = Date.Now _
                                            , .DistanceQueueDetailModUser = Parameters.UserName _
                                            , .DistanceQueueDetailUpdated = If(d.DistanceQueueDetailUpdated Is Nothing, New Byte() {}, d.DistanceQueueDetailUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GettblDistanceQueueDetailFiltered(Control:=CType(LinqTable, LTS.tblDistanceQueueDetail).DistanceQueueDetailControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim source As LTS.tblDistanceQueueDetail = TryCast(LinqTable, LTS.tblDistanceQueueDetail)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tblDistanceQueueDetails
                       Where d.DistanceQueueDetailControl = source.DistanceQueueDetailControl
                       Select New DTO.QuickSaveResults With {.Control = d.DistanceQueueDetailControl _
                                                            , .ModDate = d.DistanceQueueDetailModDate _
                                                            , .ModUser = d.DistanceQueueDetailModUser _
                                                            , .Updated = d.DistanceQueueDetailUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class

Public Class NGLWhatsNewData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblWhatsNews
        Me.LinqDB = db
        Me.SourceClass = "NGLWhatsNewData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblWhatsNews
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    'Called By WhatsNewController.Get()
    Public Function GetvWhatsNew(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vWhatsNew()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vWhatsNew
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vWhatsNew)
                iQuery = db.vWhatsNews
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvWhatsNew"), db)
            End Try
        End Using
        Return Nothing
    End Function

    'Called By WhatsNewController.Post()
    Public Function SaveWhatsNew(ByVal oData As LTS.tblWhatsNew) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Dim sVersion = oData.WhatsNewVersion
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(sVersion) Then
                    Dim lDetails As New List(Of String) From {"Version Record Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.WhatsNewModUser = Parameters.UserName
                oData.WhatsNewModDate = Date.Now
                If oData.WhatsNewControl = 0 Then
                    db.tblWhatsNews.InsertOnSubmit(oData)
                Else
                    db.tblWhatsNews.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveWhatsNew"), db)
            End Try
        End Using
        Return blnRet
    End Function

    'Called By WhatsNewController.DELETE()
    Public Function DeleteWhatsNew(ByVal iWhatsNewControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iWhatsNewControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'verify the record
                Dim oExisting = db.tblWhatsNews.Where(Function(x) x.WhatsNewControl = iWhatsNewControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.WhatsNewControl = 0 Then Return True
                db.tblWhatsNews.DeleteOnSubmit(oExisting)

                Dim oChildren = db.tblWhatsNews.Where(Function(x) x.WhatsNewParentID = iWhatsNewControl).ToArray()
                If oChildren?.Count() > 0 Then
                    db.tblWhatsNews.DeleteAllOnSubmit(oChildren)
                End If

                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteWhatsNew"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteLegalEntityCarrier(ByVal Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim oTable = db.tblLegalEntityCarriers
            Dim oChildTbl = db.tblLECarrierAccessorials
            Try
                Dim oRecord As LTS.tblLegalEntityCarrier = db.tblLegalEntityCarriers.Where(Function(x) x.LECarControl = Control).FirstOrDefault()

                Dim oChild As LTS.tblLECarrierAccessorial() = db.tblLECarrierAccessorials.Where(Function(x) x.LECALECarControl = Control).ToArray()

                If (oRecord Is Nothing OrElse oRecord.LECarControl = 0) Then Return False
                'oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)

                For Each r In oChild
                    If (Not r Is Nothing AndAlso r.LECAControl <> 0) Then
                        oChildTbl.DeleteOnSubmit(r)
                    End If
                Next

                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteLegalEntityCarrier"), db)
            End Try
        End Using
        Return blnRet
    End Function


    'Called By WhatsNewController.AddWhatsNew()
    Public Function InsertWhatsNew(ByVal oData As LTS.tblWhatsNew) As Integer
        Dim WNControl As Integer = 0
        If oData Is Nothing Then Return False 'nothing to do
        Dim sVersion = oData.WhatsNewVersion
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(sVersion) Then
                    Dim lDetails As New List(Of String) From {"Version Record Reference", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return WNControl
                End If
                oData.WhatsNewModUser = Parameters.UserName
                oData.WhatsNewModDate = Date.Now
                db.tblWhatsNews.InsertOnSubmit(oData)
                db.SubmitChanges()
                WNControl = oData.WhatsNewControl
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertWhatsNew"), db)
            End Try
        End Using
        Return WNControl
    End Function

    'Called By WhatsNewController.AddWhatsNew()
    Public Function InsertWhatsNew(ByVal oData As List(Of LTS.tblWhatsNew)) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                For Each d In oData
                    Try
                        d.WhatsNewModUser = Parameters.UserName
                        d.WhatsNewModDate = Date.Now
                        db.tblWhatsNews.InsertOnSubmit(d)
                    Catch ex As Exception
                        'if one fails keep processing the rest of them
                    End Try
                Next
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertWhatsNew"), db)
            End Try
        End Using
        Return blnRet
    End Function

    'Called By WhatsNewController.GetAllRecords()
    Public Function GetvWhatsNewHdrGrid(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vWhatsNew()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vWhatsNew
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vWhatsNew)
                iQuery = db.vWhatsNews.Where(Function(x) x.WhatsNewParentID = 0)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvWhatsNewHdrGrid"), db)
            End Try
        End Using
        Return Nothing
    End Function

    'Called By WhatsNewController.GetByParent()
    Public Function GetvWhatsNewDetailGrid(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vWhatsNew()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vWhatsNew
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vWhatsNew)
                iQuery = db.vWhatsNews.Where(Function(x) x.WhatsNewParentID = filters.ParentControl)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvWhatsNewDetailGrid"), db)
            End Try
        End Using
        Return Nothing
    End Function

    'Called By WhatsNewController.GetWhatsNewReportHTML()
    Public Function GetWhatsNewReportHTML(ByVal strVersion As String) As String
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                ''<div>
                ''    <div><h2>Version - VersionDescription - VersionDate</h2></div>
                ''    <h3>Issues Corrected</h3>
                ''    <div>
                ''        <div>Bug Title</div>
                ''        <ul>
                ''            <li>Bug Note</li>
                ''        </ul>
                ''    </div>
                ''    <h3>Enhancements</h3>
                ''    <div>
                ''        <div>Enhancement Title</div>
                ''        <ul>
                ''            <li>Enhancement Note</li>
                ''        </ul>
                ''    </div>
                ''    <h3>Notes</h3>
                ''    <div>
                ''        <div>General Title</div>
                ''        <ul>
                ''            <li>General Note</li>
                ''        </ul>
                ''    </div>
                ''    <h3>Known Issues</h3>
                ''    <div>
                ''        <div>Known Issues Title</div>
                ''        <ul>
                ''            <li>Known Issue Note</li>
                ''        </ul>
                ''    </div>
                ''</div>
                Dim sb As New Text.StringBuilder
                Dim distinctVersionList As New List(Of String)
                If Not String.IsNullOrWhiteSpace(strVersion) AndAlso db.tblWhatsNews.Any(Function(x) x.WhatsNewVersion = strVersion) Then
                    distinctVersionList.Add(strVersion)
                Else
                    distinctVersionList = db.vWhatsNews.Select(Function(x) x.WhatsNewVersion).Distinct().OrderByDescending(Function(y) y).ToList()
                End If
                For Each dvl In distinctVersionList
                    Dim versionRecords = db.vWhatsNews.Where(Function(x) x.WhatsNewVersion = dvl).ToArray()
                    If versionRecords?.Count > 0 Then
                        sb.Append("<div>")
                        sb.Append(String.Format("<div id='divVersion'><h2 style='margin-bottom:0px;'>{0}</h2><h3 style='margin-top:0px;'>{1} - Updated {2}</h3></div>", versionRecords(0).WhatsNewVersion, versionRecords(0).VersionDescription, versionRecords(0).VersionDate.Value.ToShortDateString()))
                        'Enhancements
                        If versionRecords.Any(Function(x) x.WhatsNewFeatureTypeControl = 3 AndAlso x.WhatsNewParentID = 0) Then
                            sb.Append("<h4>Enhancements</h4>")
                            sb.Append("<div>")
                            Dim enhancementParents = versionRecords.Where(Function(x) x.WhatsNewFeatureTypeControl = 3 AndAlso x.WhatsNewParentID = 0).OrderBy(Function(y) y.WhatsNewSeqNo).ToArray()
                            For Each e In enhancementParents
                                sb.AppendFormat("<div>{0}</div>", e.WhatsNewTitle) 'Feature Title
                                If versionRecords.Any(Function(x) x.WhatsNewParentID = e.WhatsNewControl) Then
                                    Dim enhancementChildren = versionRecords.Where(Function(x) x.WhatsNewParentID = e.WhatsNewControl).OrderBy(Function(y) y.WhatsNewSeqNo).ToArray()
                                    sb.Append("<ul>")
                                    For Each ee In enhancementChildren
                                        sb.AppendFormat("<li>{0}</li>", ee.WhatsNewNote)
                                    Next
                                    sb.Append("</ul>")
                                End If
                            Next
                            sb.Append("</div>")
                        End If
                        'Bugs
                        If versionRecords.Any(Function(x) x.WhatsNewFeatureTypeControl = 2 AndAlso x.WhatsNewParentID = 0) Then
                            sb.Append("<h4>Issues Corrected</h4>")
                            sb.Append("<div>")
                            Dim bugParents = versionRecords.Where(Function(x) x.WhatsNewFeatureTypeControl = 2 AndAlso x.WhatsNewParentID = 0).OrderBy(Function(y) y.WhatsNewSeqNo).ToArray()
                            For Each b In bugParents
                                sb.AppendFormat("<div>{0}</div>", b.WhatsNewTitle) 'Feature Title
                                If versionRecords.Any(Function(x) x.WhatsNewParentID = b.WhatsNewControl) Then
                                    Dim bugChildren = versionRecords.Where(Function(x) x.WhatsNewParentID = b.WhatsNewControl).OrderBy(Function(y) y.WhatsNewSeqNo).ToArray()
                                    sb.Append("<ul>")
                                    For Each bb In bugChildren
                                        sb.AppendFormat("<li>{0}</li>", bb.WhatsNewNote)
                                    Next
                                    sb.Append("</ul>")
                                End If
                            Next
                            sb.Append("</div>")
                        End If
                        'General
                        If versionRecords.Any(Function(x) x.WhatsNewFeatureTypeControl = 1 AndAlso x.WhatsNewParentID = 0) Then
                            sb.Append("<h4>Notes</h4>")
                            sb.Append("<div>")
                            Dim generalParents = versionRecords.Where(Function(x) x.WhatsNewFeatureTypeControl = 1 AndAlso x.WhatsNewParentID = 0).OrderBy(Function(y) y.WhatsNewSeqNo).ToArray()
                            For Each g In generalParents
                                sb.AppendFormat("<div>{0}</div>", g.WhatsNewTitle) 'Feature Title
                                If versionRecords.Any(Function(x) x.WhatsNewParentID = g.WhatsNewControl) Then
                                    Dim generalChildren = versionRecords.Where(Function(x) x.WhatsNewParentID = g.WhatsNewControl).OrderBy(Function(y) y.WhatsNewSeqNo).ToArray()
                                    sb.Append("<ul>")
                                    For Each gg In generalChildren
                                        sb.AppendFormat("<li>{0}</li>", gg.WhatsNewNote)
                                    Next
                                    sb.Append("</ul>")
                                End If
                            Next
                            sb.Append("</div>")
                        End If
                        'Known Issues
                        If versionRecords.Any(Function(x) x.WhatsNewFeatureTypeControl = 4 AndAlso x.WhatsNewParentID = 0) Then
                            sb.Append("<h4>Known Issues</h4>")
                            sb.Append("<div>")
                            Dim kiParents = versionRecords.Where(Function(x) x.WhatsNewFeatureTypeControl = 4 AndAlso x.WhatsNewParentID = 0).OrderBy(Function(y) y.WhatsNewSeqNo).ToArray()
                            For Each k In kiParents
                                sb.AppendFormat("<div>{0}</div>", k.WhatsNewTitle) 'Feature Title
                                If versionRecords.Any(Function(x) x.WhatsNewParentID = k.WhatsNewControl) Then
                                    Dim kiChildren = versionRecords.Where(Function(x) x.WhatsNewParentID = k.WhatsNewControl).OrderBy(Function(y) y.WhatsNewSeqNo).ToArray()
                                    sb.Append("<ul>")
                                    For Each kk In kiChildren
                                        sb.AppendFormat("<li>{0}</li>", kk.WhatsNewNote)
                                    Next
                                    sb.Append("</ul>")
                                End If
                            Next
                            sb.Append("</div>")
                        End If
                        sb.Append("</div>")
                        sb.Append("</br>")
                    End If
                Next
                Return sb.ToString()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetWhatsNewReportHTML"), db)
            End Try
        End Using
        Return Nothing
    End Function

#End Region

#Region "Protected Functions"


#End Region

End Class

'Added By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
Public Class NGLOptMsgData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblWhatsNews
        Me.LinqDB = db
        Me.SourceClass = "NGLOptMsgData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tmpOptMsgs
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Enums"
    Public Enum OptMsgType
        StatusMsg = 1
        WarningMsg = 2
        ErrorMsg = 3
    End Enum
#End Region

#Region "Public Methods"

    Public Function GetOptimizerMessages(ByVal USC As Integer) As Models.OptMsg
        If USC = 0 Then Return Nothing
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim oOptMsg As New Models.OptMsg
                Dim messages = db.tmpOptMsgs.Where(Function(x) x.OptMsgUSC = USC).ToArray()
                Dim statusMsgs = messages.Where(Function(x) x.OptMsgType = 1).ToArray()
                'Step 1
                Dim s1 = statusMsgs.Where(Function(x) x.OptMsgStep = 1).FirstOrDefault()
                If Not s1 Is Nothing Then oOptMsg.Step1 = New Models.OptMsgStep With {.StepNumber = 1, .StepProgress = s1.OptMsgProgress, .StepComplete = s1.OptMsgComplete, .StepMessage = s1.OptMsgText, .StepSubMessage = s1.OptMsgSubText} Else oOptMsg.Step1 = Nothing
                'Step 2
                Dim s2 = statusMsgs.Where(Function(x) x.OptMsgStep = 2).FirstOrDefault()
                If Not s2 Is Nothing Then oOptMsg.Step2 = New Models.OptMsgStep With {.StepNumber = 2, .StepProgress = s2.OptMsgProgress, .StepComplete = s2.OptMsgComplete, .StepMessage = s2.OptMsgText, .StepSubMessage = s2.OptMsgSubText} Else oOptMsg.Step2 = Nothing
                'Step 3
                Dim s3 = statusMsgs.Where(Function(x) x.OptMsgStep = 3).FirstOrDefault()
                If Not s3 Is Nothing Then oOptMsg.Step3 = New Models.OptMsgStep With {.StepNumber = 3, .StepProgress = s3.OptMsgProgress, .StepComplete = s3.OptMsgComplete, .StepMessage = s3.OptMsgText, .StepSubMessage = s3.OptMsgSubText} Else oOptMsg.Step3 = Nothing
                'Step 4
                Dim s4 = statusMsgs.Where(Function(x) x.OptMsgStep = 4).FirstOrDefault()
                If Not s4 Is Nothing Then oOptMsg.Step4 = New Models.OptMsgStep With {.StepNumber = 4, .StepProgress = s4.OptMsgProgress, .StepComplete = s4.OptMsgComplete, .StepMessage = s4.OptMsgText, .StepSubMessage = s4.OptMsgSubText} Else oOptMsg.Step4 = Nothing
                'Step 5
                Dim s5 = statusMsgs.Where(Function(x) x.OptMsgStep = 5).FirstOrDefault()
                If Not s5 Is Nothing Then oOptMsg.Step5 = New Models.OptMsgStep With {.StepNumber = 5, .StepProgress = s5.OptMsgProgress, .StepComplete = s5.OptMsgComplete, .StepMessage = s5.OptMsgText, .StepSubMessage = s5.OptMsgSubText} Else oOptMsg.Step5 = Nothing
                'Step 6
                Dim s6 = statusMsgs.Where(Function(x) x.OptMsgStep = 6).FirstOrDefault()
                If Not s6 Is Nothing Then oOptMsg.Step6 = New Models.OptMsgStep With {.StepNumber = 6, .StepProgress = s6.OptMsgProgress, .StepComplete = s6.OptMsgComplete, .StepMessage = s6.OptMsgText, .StepSubMessage = s6.OptMsgSubText} Else oOptMsg.Step6 = Nothing
                'Step 7
                Dim s7 = statusMsgs.Where(Function(x) x.OptMsgStep = 7).FirstOrDefault()
                If Not s7 Is Nothing Then oOptMsg.Step7 = New Models.OptMsgStep With {.StepNumber = 7, .StepProgress = s7.OptMsgProgress, .StepComplete = s7.OptMsgComplete, .StepMessage = s7.OptMsgText, .StepSubMessage = s7.OptMsgSubText} Else oOptMsg.Step7 = Nothing
                'Step 8
                Dim s8 = statusMsgs.Where(Function(x) x.OptMsgStep = 8).FirstOrDefault()
                If Not s8 Is Nothing Then oOptMsg.Step8 = New Models.OptMsgStep With {.StepNumber = 8, .StepProgress = s8.OptMsgProgress, .StepComplete = s8.OptMsgComplete, .StepMessage = s8.OptMsgText, .StepSubMessage = s8.OptMsgSubText} Else oOptMsg.Step8 = Nothing
                'Step 9
                Dim s9 = statusMsgs.Where(Function(x) x.OptMsgStep = 9).FirstOrDefault()
                If Not s9 Is Nothing Then oOptMsg.Step9 = New Models.OptMsgStep With {.StepNumber = 9, .StepProgress = s9.OptMsgProgress, .StepComplete = s9.OptMsgComplete, .StepMessage = s9.OptMsgText, .StepSubMessage = s9.OptMsgSubText} Else oOptMsg.Step9 = Nothing
                'Step 10
                Dim s10 = statusMsgs.Where(Function(x) x.OptMsgStep = 10).FirstOrDefault()
                If Not s10 Is Nothing Then oOptMsg.Step10 = New Models.OptMsgStep With {.StepNumber = 10, .StepProgress = s10.OptMsgProgress, .StepComplete = s10.OptMsgComplete, .StepMessage = s10.OptMsgText, .StepSubMessage = s10.OptMsgSubText} Else oOptMsg.Step10 = Nothing
                'Step 11
                Dim s11 = statusMsgs.Where(Function(x) x.OptMsgStep = 11).FirstOrDefault()
                If Not s11 Is Nothing Then oOptMsg.Step11 = New Models.OptMsgStep With {.StepNumber = 11, .StepProgress = s11.OptMsgProgress, .StepComplete = s11.OptMsgComplete, .StepMessage = s11.OptMsgText, .StepSubMessage = s11.OptMsgSubText} Else oOptMsg.Step11 = Nothing
                'Step 12
                Dim s12 = statusMsgs.Where(Function(x) x.OptMsgStep = 12).FirstOrDefault()
                If Not s12 Is Nothing Then oOptMsg.Step12 = New Models.OptMsgStep With {.StepNumber = 12, .StepProgress = s12.OptMsgProgress, .StepComplete = s12.OptMsgComplete, .StepMessage = s12.OptMsgText, .StepSubMessage = s12.OptMsgSubText} Else oOptMsg.Step12 = Nothing
                Return oOptMsg
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOptimizerMessages"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function SaveOptimizerStatusMessage(ByVal OptStep As Integer, ByVal Msg As String, ByVal SubMsg As String, ByVal StepProgress As Integer, ByVal StepComplete As Boolean, ByVal OptCompName As String, ByVal OptInbound As Boolean, Optional ByVal OptMsgType As OptMsgType = OptMsgType.StatusMsg) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim oData As New LTS.tmpOptMsg()
                If db.tmpOptMsgs.Any(Function(x) x.OptMsgUSC = Parameters.UserControl AndAlso x.OptMsgStep = OptStep AndAlso x.OptMsgCompName = OptCompName AndAlso x.OptMsgCompInbound = OptInbound) Then
                    'Update
                    oData = db.tmpOptMsgs.Where(Function(x) x.OptMsgUSC = Parameters.UserControl AndAlso x.OptMsgStep = OptStep).FirstOrDefault()
                    oData.OptMsgProgress = StepProgress
                    oData.OptMsgComplete = StepComplete
                    oData.OptMsgSubText = SubMsg
                    oData.OptMsgModUser = Parameters.UserName
                    oData.OptMsgModDate = Date.Now
                Else
                    'Insert
                    oData = New LTS.tmpOptMsg()
                    With oData
                        .OptMsgUSC = Parameters.UserControl
                        .OptMsgType = OptMsgType '1 = status message, 2 = Warning Msg, 3 = Error Msg 
                        .OptMsgStep = OptStep
                        .OptMsgText = Msg
                        .OptMsgSubText = SubMsg
                        .OptMsgProgress = StepProgress
                        .OptMsgComplete = StepComplete
                        .OptMsgCompName = OptCompName
                        .OptMsgCompInbound = OptInbound
                        .OptMsgModUser = Parameters.UserName
                        .OptMsgModDate = Date.Now
                    End With
                    db.tmpOptMsgs.InsertOnSubmit(oData)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveOptimizerStatusMessage"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function DeleteOptMsgByUser(ByVal USC As Integer) As Boolean
        Dim blnRet As Boolean = False
        If USC = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim oExisting = db.tmpOptMsgs.Where(Function(x) x.OptMsgUSC = USC).ToArray()
                If oExisting?.Count() > 0 Then
                    db.tmpOptMsgs.DeleteAllOnSubmit(oExisting)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteOptMsgByUser"), db)
            End Try
        End Using
        Return blnRet
    End Function

    Public Function GetLatestOptimizerMessage(ByVal USC As Integer) As Models.OptMsg
        If USC = 0 Then Return Nothing
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim oOptMsg As New Models.OptMsg
                Dim msg = db.tmpOptMsgs.Where(Function(x) x.OptMsgUSC = USC And x.OptMsgType = 1).OrderByDescending(Function(y) y.OptMsgControl).FirstOrDefault()
                If Not msg Is Nothing Then oOptMsg.Step1 = New Models.OptMsgStep With {.StepNumber = msg.OptMsgStep, .StepProgress = msg.OptMsgProgress, .StepComplete = msg.OptMsgComplete, .StepMessage = msg.OptMsgText, .StepSubMessage = msg.OptMsgSubText} Else oOptMsg.Step1 = Nothing
                Return oOptMsg
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLatestOptimizerMessage"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetOptMsgs(ByVal USC As Integer) As LTS.tmpOptMsg()
        If USC = 0 Then Return Nothing
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Return db.tmpOptMsgs.Where(Function(x) x.OptMsgUSC = USC AndAlso x.OptMsgType = 1).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOptMsgs"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetOptMsgs(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tmpOptMsg()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tmpOptMsg
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tmpOptMsg)
                iQuery = db.tmpOptMsgs
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetOptMsgs"), db)
            End Try
        End Using
        Return Nothing
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class


''' <summary>
''' Data Access Layer for Run Task Data 
''' </summary>
''' <remarks>
''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
''' </remarks>
Public Class NGLtblRunTaskData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblRunTasks
        Me.LinqDB = db
        Me.SourceClass = "NGLtblRunTaskData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblRunTasks
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Get a filtered array of Task Data
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function GettblRunTasks(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblRunTask()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblRunTask
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblRunTask)
                iQuery = db.tblRunTasks
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblRunTasks"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Insert or Update Task Data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function SavetblRunTask(ByVal oData As LTS.tblRunTask) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iRunTaskControl As Integer = oData.RunTaskControl
                If String.IsNullOrWhiteSpace(oData.RunTaskName) Then
                    Dim lDetails As New List(Of String) From {"Task Name", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.RunTaskModDate = Date.Now
                oData.RunTaskModUser = Me.Parameters.UserName

                If (iRunTaskControl = 0) Then
                    'this is an insert test if the name already exists
                    If db.tblRunTasks.Any(Function(x) String.Compare(x.RunTaskName, oData.RunTaskName, True) = 0) Then
                        Dim lDetails As New List(Of String) From {"Task Name", " " & oData.RunTaskName & " already exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    db.tblRunTasks.InsertOnSubmit(oData)
                Else
                    'This is an update so get the current record,  if it does not exist throw error
                    Dim oTask As LTS.tblRunTask = db.tblRunTasks.Where(Function(x) x.RunTaskControl = iRunTaskControl).FirstOrDefault()
                    If oTask Is Nothing OrElse oTask.RunTaskControl = 0 Then
                        Dim lDetails As New List(Of String) From {"Task Control", " was not found or has been deleted and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    If db.tblRunTasks.Any(Function(x) String.Compare(x.RunTaskName, oData.RunTaskName, True) = 0 And x.RunTaskControl <> oData.RunTaskControl) Then
                        Dim lDetails As New List(Of String) From {"Task Name", " " & oData.RunTaskName & " a duplicate exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    Dim otblRunTask = db.tblRunTasks.Where(Function(x) x.RunTaskControl = iRunTaskControl).FirstOrDefault()
                    Dim skipObjs As New List(Of String) From {"RunTaskControl"}
                    Dim strMsg As String
                    otblRunTask = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(otblRunTask, oData, skipObjs, strMsg)
                    'db.tblRunTasks.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblRunTask"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific Task Record
    ''' </summary>
    ''' <param name="iRunTaskControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function DeletetblRunTask(ByVal iRunTaskControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iRunTaskControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'verify the record
                Dim oExisting = db.tblRunTasks.Where(Function(x) x.RunTaskControl = iRunTaskControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.RunTaskControl = 0 Then Return True
                db.tblRunTasks.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblRunTask"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Format the Task Run Date Message using selected settngs
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function getTaskDateMessage(ByRef oData As LTS.vTblRunTask) As String

        Dim intMin As Integer = oData.RunTaskMinutes
        Dim intHour As Integer = oData.RunTaskHours
        Dim intDay As Integer = oData.RunTaskDays
        Dim intMonth As Integer = oData.RunTaskMonths
        Dim intWeekDay As Integer = oData.RunTaskWeekDays

        Dim d1 As DateTime = DateTime.Now
        Dim intToRunMinute As Integer
        Dim intLastRunMinute As Integer
        Dim intThisMinute As Integer = d1.Minute
        Dim intToRunHour As Integer
        Dim intLastRunHour As Integer
        Dim intThisHour As Integer = d1.Hour
        Dim intToRunDay As Integer
        Dim intLastRunDay As Integer
        Dim intThisDay As Integer = d1.Day
        Dim intToRunMonth As Integer
        Dim intLastRunMonth As Integer
        Dim intThisMonth As Integer = d1.Month
        Dim intThisWeekDay As Integer = d1.DayOfWeek
        Dim intToRunYear As Integer
        Dim intLastRunYear As Integer
        Dim intThisYear As Integer = d1.Year
        Dim intThisDayOfYear As Integer = d1.DayOfYear
        Dim blnDateFound As Boolean = False
        Dim arrDates As New ArrayList

        'set all defaults to NOW
        intToRunYear = intThisYear
        intToRunMonth = intThisMonth
        intToRunDay = intThisDay
        intToRunHour = intThisHour
        intToRunMinute = intThisMinute

        intLastRunYear = intThisYear
        intLastRunMonth = intThisMonth
        intLastRunDay = intThisDay
        intLastRunHour = intThisHour
        intLastRunMinute = intThisMinute

        Select Case intMin
            Case 62 'Every 15 Minutes
                blnDateFound = True
                Return "Every 15 Minutes"
            Case 61 'Every 10 Minutes
                blnDateFound = True
                Return "Every 10 Minutes"
            Case 60 'Every 5 Minutes
                blnDateFound = True
                Return "Every 5 Minutes"
            Case Else
                intToRunMinute = intMin
        End Select
        'Now we test the hour
        If Not blnDateFound Then
            Select Case intHour
                Case 28 'Every 12 hours
                    blnDateFound = True
                    Return "Every 12 Hours at " & intMin & " minutes past the hour."
                Case 27 'Every 6 hours
                    blnDateFound = True
                    Return "Every 6 Hours at " & intMin & " minutes past the hour."
                Case 26 'Every 4 hours
                    blnDateFound = True
                    Return "Every 4 Hours at " & intMin & " minutes past the hour."
                Case 25 'Every 2 hours
                    blnDateFound = True
                    Return "Every 2 Hours at " & intMin & " minutes past the hour."
                Case 24 'Every Hour
                    blnDateFound = True
                    Return "Every Hour at " & intMin & " minutes past the hour."
                Case Else
                    intToRunHour = intHour
            End Select
        End If
        'Now we test the day
        If Not blnDateFound Then
            Select Case intDay
                Case 0 'Every Day
                    'Test for Weekday
                    If intWeekDay < 7 Then
                        Select Case intWeekDay
                            Case 0
                                blnDateFound = True
                                Return "Every Sunday at " & formatTime(intHour, intMin) & "."
                            Case 1
                                blnDateFound = True
                                Return "Every Monday at " & formatTime(intHour, intMin) & "."
                            Case 2
                                blnDateFound = True
                                Return "Every Tuesday at " & formatTime(intHour, intMin) & "."
                            Case 3
                                blnDateFound = True
                                Return "Every Wednesday at " & formatTime(intHour, intMin) & "."
                            Case 4
                                blnDateFound = True
                                Return "Every Thursday at " & formatTime(intHour, intMin) & "."
                            Case 5
                                blnDateFound = True
                                Return "Every Friday at " & formatTime(intHour, intMin) & "."
                            Case Else
                                blnDateFound = True
                                Return "Every Saturday at " & formatTime(intHour, intMin) & "."
                        End Select
                    Else
                        blnDateFound = True
                        Return "Every Day at " & formatTime(intHour, intMin) & "."
                    End If
                Case Else
                    intToRunDay = intDay
            End Select
        End If
        'Now we test the month
        If Not blnDateFound Then
            If intMonth > 0 Then
                If intThisMonth <= intMonth Then
                    intToRunYear = intThisYear
                Else
                    intToRunYear = intThisYear + 1
                End If
                blnDateFound = True
                Return intMonth & "-" & intDay & "-" & intToRunYear & " at " & formatTime(intHour, intMin) & "."
            Else  'Run Every Month
                blnDateFound = True
                Return "Every Month on the " & formatDay(intDay) & " day at " & formatTime(intHour, intMin) & "."
            End If
        End If

        Return " N/A "
    End Function

    ''' <summary>
    ''' Format the Task Time string for seleted task settings
    ''' </summary>
    ''' <param name="intHour"></param>
    ''' <param name="intMin"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function formatTime(ByVal intHour As Integer, ByVal intMin As Integer) As String
        Dim strMin As String

        If intMin < 10 Then
            strMin = "0" & intMin
        Else
            strMin = intMin
        End If
        If intHour > 12 Then
            intHour -= 12
            Return intHour & ":" & strMin & " pm"
        Else
            Return intHour & ":" & strMin & " am"
        End If
    End Function

    ''' <summary>
    ''' Format the Task Day string using selected configuration
    ''' </summary>
    ''' <param name="intDay"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function formatDay(ByVal intDay As Integer) As String
        Select Case intDay
            Case 1, 21, 31
                Return intDay & "st"
            Case 2, 22
                Return intDay & "nd"
            Case 3, 23
                Return intDay & "rd"
            Case Else
                Return intDay & "th"
        End Select
    End Function

    ''' <summary>
    ''' Get an array of filtered task log records
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function getTaskLog(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblTaskLog()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblTaskLog
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblTaskLog)
                iQuery = db.tblTaskLogs
                Dim filterWhere = ""
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "TaskControl"
                    filters.sortDirection = "desc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("tblTaskLog"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' format task settings into readable message
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Sub formatTaskMessages(ByRef oData As LTS.vTblRunTask)
        oData.RunTaskTaskDateMessage = getTaskDateMessage(oData)
        'Case "TaskMinutes" 'Created by RHR for v-8.5.0.001 on 09/22/21 
        Select Case oData.RunTaskMinutes
            Case 60
                oData.RunTaskMinutesMessage = "Every 5 minutes all day"
            Case 61
                oData.RunTaskMinutesMessage = "Every 10 minutes all day"
            Case 62
                oData.RunTaskMinutesMessage = "Every 15 minutes all day"
            Case Else
                oData.RunTaskMinutesMessage = oData.RunTaskMinutes.ToString() & " Minutes past the hour"
        End Select

        'Case "TaskHours" 'Created by RHR for v-8.5.0.001 on 09/22/21 
        Select Case oData.RunTaskHours
            Case 24
                oData.RunTaskHoursMessage = "Every hour all day"
            Case 25
                oData.RunTaskHoursMessage = "Every 2 hours all day"
            Case 26
                oData.RunTaskHoursMessage = "Every 4 hours all day"
            Case 27
                oData.RunTaskHoursMessage = "Every 6 hours all day"
            Case 28
                oData.RunTaskHoursMessage = "Every 12 hours all day"
            Case Else
                If oData.RunTaskHours < 12 Then
                    oData.RunTaskHoursMessage = oData.RunTaskHours.ToString() & " am"
                Else
                    oData.RunTaskHoursMessage = (oData.RunTaskHours - 12).ToString() & " pm"
                End If
        End Select
        'Case "TaskDays" 'Created by RHR for v-8.5.0.001 on 09/22/21 
        If (oData.RunTaskDays = 0) Then
            oData.RunTaskDaysMessage = "Every day"
        Else
            oData.RunTaskDaysMessage = "On day " & oData.RunTaskDays.ToString() & " of the month"
        End If
        'Case "TaskWeekDays" 'Created by RHR for v-8.5.0.001 on 09/22/21 
        Select Case oData.RunTaskWeekDays
            Case 0
                oData.RunTaskWeekDaysMessage = "Every Sunday"
            Case 1
                oData.RunTaskWeekDaysMessage = "Every Monday"
            Case 2
                oData.RunTaskWeekDaysMessage = "Every Tuesday"
            Case 3
                oData.RunTaskWeekDaysMessage = "Every Wednesday"
            Case 4
                oData.RunTaskWeekDaysMessage = "Every Thursday"
            Case 5
                oData.RunTaskWeekDaysMessage = "Every Friday"
            Case 6
                oData.RunTaskWeekDaysMessage = "Every Saturday"
            Case Else
                oData.RunTaskWeekDaysMessage = "Use Days Only"
        End Select

        'Case "TaskMonths" 'Created by RHR for v-8.5.0.001 on 09/22/21 
        If (oData.RunTaskMonths = 0) Then
            oData.RunTaskMonthsMessage = "Every Month"
        Else
            oData.RunTaskMonthsMessage = oData.RunTaskMonths.ToString()
        End If

    End Sub

    ''' <summary>
    ''' Copy table data to view
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function selectLTSViewFromData(ByVal oData As LTS.tblRunTask) As LTS.vTblRunTask
        Dim ltsView As New LTS.vTblRunTask()
        If oData Is Nothing Then Return ltsView
        Dim skipObjs As New List(Of String) From {""}
        Dim sMsg As String = ""
        ltsView = DTran.CopyMatchingFields(ltsView, oData, skipObjs, sMsg)
        formatTaskMessages(ltsView)
        Return ltsView
    End Function

    ''' <summary>
    ''' Copy View data to  Table
    ''' </summary>
    ''' <param name="oView"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
    ''' </remarks>
    Public Function selectLTSDataFromView(ByVal oView As LTS.vTblRunTask) As LTS.tblRunTask
        Dim ltsTable As New LTS.tblRunTask()
        If oView Is Nothing Then Return ltsTable
        Dim skipObjs As New List(Of String) From {"RunTaskTaskDateMessage", "RunTaskMinutesMessage", "RunTaskHoursMessage", "RunTaskDaysMessage", "RunTaskMonthsMessage", "RunTaskWeekDaysMessage"}
        Dim sMsg As String = ""
        ltsTable = DTran.CopyMatchingFields(ltsTable, oView, skipObjs, sMsg)
        Return ltsTable
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class

''' <summary>
''' Data Access Layer for Par Process Data
''' </summary>
''' <remarks>
''' Created by RHR for v-8.5.3.006 on 10/20/2022
''' </remarks>
Public Class NGLtblParProcessData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblParProcesses
        Me.LinqDB = db
        Me.SourceClass = "NGLtblParProcessData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblParProcesses
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Get a filtered array of Par Process Data 
    ''' Caller must set filters.ParentControl to ParProcCategoryControl (FK)
    ''' or must pass in a filtervalue for filtername ParProcControl (PK)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Process Manager Logic
    ''' </remarks>
    Public Function GettblParProcesses(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblParProcess()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblParProcess
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblParProcess)
                iQuery = db.tblParProcesses
                Dim filterWhere = ""
                If Not (filters.filterName = "ParProcControl" AndAlso filters.filterValue <> "0") Then
                    'if we are not using the tables primary key as a filter we must use the parent primary key as a filter
                    Dim iParCarControl = filters.ParentControl
                    If filters.ParentControl = 0 Then
                        throwInvalidKeyParentRequiredException(New List(Of String) From {"Workflow Category Key", " was not valid and "})
                    End If
                    filterWhere = " (ParProcCategoryControl = " & filters.ParentControl.ToString() & ") "
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblParProcesses"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Insert or Update Par Process Data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Process  Logic
    ''' </remarks>
    Public Function SavetblParProcess(ByVal oData As LTS.tblParProcess) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iParProcControl As Integer = oData.ParProcControl
                'Verify that the parameterCategoy is valid.
                If Not db.tblParCategories.Any(Function(x) x.ParCatControl = oData.ParProcCategoryControl) Then
                    Dim lDetails As New List(Of String) From {"Workflow Category Key", " was not valid and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If String.IsNullOrWhiteSpace(oData.ParProcName) Then
                    Dim lDetails As New List(Of String) From {"Workflow Process Name", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.ParProcModDate = Date.Now
                oData.ParProcModUser = Me.Parameters.UserName

                If (iParProcControl = 0) Then
                    'this is an insert test if the name already exists for this parameter category
                    If db.tblParProcesses.Any(Function(x) String.Compare(x.ParProcName, oData.ParProcName, True) = 0 And x.ParProcCategoryControl = oData.ParProcCategoryControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Name", " " & oData.ParProcName & " already exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    db.tblParProcesses.InsertOnSubmit(oData)
                Else
                    'This is an update so get the current record,  if it does not exist throw error
                    Dim oParProc As LTS.tblParProcess = db.tblParProcesses.Where(Function(x) x.ParProcControl = iParProcControl).FirstOrDefault()
                    If oParProc Is Nothing OrElse oParProc.ParProcControl = 0 Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Key", " was not found or has been deleted and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    If db.tblParProcesses.Any(Function(x) String.Compare(x.ParProcName, oData.ParProcName, True) = 0 And x.ParProcControl <> oData.ParProcControl And x.ParProcCategoryControl = oData.ParProcCategoryControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Name", " " & oData.ParProcName & " a duplicate exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    Dim otblParProcess = db.tblParProcesses.Where(Function(x) x.ParProcControl = iParProcControl).FirstOrDefault()
                    Dim skipObjs As New List(Of String) From {"ParProcControl", "tblParProcessOptions", "tblParCategory"}
                    Dim strMsg As String
                    otblParProcess = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(otblParProcess, oData, skipObjs, strMsg)
                    'db.tblParProcesses.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblParProcess"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific Par Process Record
    ''' </summary>
    ''' <param name="iParProcControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Process Logic
    ''' </remarks>
    Public Function DeletetblParProcess(ByVal iParProcControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iParProcControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'verify the record
                Dim oExisting = db.tblParProcesses.Where(Function(x) x.ParProcControl = iParProcControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ParProcControl = 0 Then Return True
                db.tblParProcesses.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblParProcess"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class

''' <summary>
''' Data Access Layer for Par Process Options Data
''' </summary>
''' <remarks>
''' Created by RHR for v-8.5.3.006 on 10/20/2022
''' </remarks>
Public Class NGLtblParProcessOptionData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblParProcessOptions
        Me.LinqDB = db
        Me.SourceClass = "NGLtblParProcessOptionData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblParProcessOptions
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Get a filtered array of Par Process Options Data
    ''' Caller must set filters.ParentControl to ParProcOptParProcControl (FK)
    ''' or must pass in a filtervalue for filtername ParProcOptControl (PK)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Process Options Logic
    ''' </remarks>
    Public Function GettblParProcessOptions(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblParProcessOption()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblParProcessOption
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblParProcessOption)
                iQuery = db.tblParProcessOptions
                Dim filterWhere = ""
                If Not (filters.filterName = "ParProcOptControl" AndAlso filters.filterValue <> "0") Then
                    'if we are not using the tables primary key as a filter we must use the parent primary key as a filter
                    Dim iParCarControl = filters.ParentControl
                    If filters.ParentControl = 0 Then
                        throwInvalidKeyParentRequiredException(New List(Of String) From {"Workflow Process Key", " was not valid and "})
                    End If
                    filterWhere = " (ParProcOptParProcControl = " & filters.ParentControl.ToString() & ") "
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblParProcessOptions"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Insert or Update Par Process Options Data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Process Options Logic
    ''' </remarks>
    Public Function SavetblParProcessOption(ByVal oData As LTS.tblParProcessOption) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iParProcOptControl As Integer = oData.ParProcOptControl
                'Verify that the Parameter Process Key is valid.
                If Not db.tblParProcesses.Any(Function(x) x.ParProcControl = oData.ParProcOptParProcControl) Then
                    Dim lDetails As New List(Of String) From {"Workflow Process Key", " was not valid and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                If String.IsNullOrWhiteSpace(oData.ParProcOptName) Then
                    Dim lDetails As New List(Of String) From {"Workflow Process Option Name", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.ParProcOptModDate = Date.Now
                oData.ParProcOptModUser = Me.Parameters.UserName

                If (iParProcOptControl = 0) Then
                    'this is an insert test if the name already exists
                    If db.tblParProcessOptions.Any(Function(x) String.Compare(x.ParProcOptName, oData.ParProcOptName, True) = 0 And x.ParProcOptParProcControl = oData.ParProcOptParProcControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Option Name", " " & oData.ParProcOptName & " already exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    db.tblParProcessOptions.InsertOnSubmit(oData)
                Else
                    'This is an update so get the current record,  if it does not exist throw error
                    Dim oParProcOpt As LTS.tblParProcessOption = db.tblParProcessOptions.Where(Function(x) x.ParProcOptControl = iParProcOptControl).FirstOrDefault()
                    If oParProcOpt Is Nothing OrElse oParProcOpt.ParProcOptControl = 0 Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Option Key", " was not found or has been deleted and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    If db.tblParProcessOptions.Any(Function(x) String.Compare(x.ParProcOptName, oData.ParProcOptName, True) = 0 And x.ParProcOptControl <> oData.ParProcOptControl And x.ParProcOptParProcControl = oData.ParProcOptParProcControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Option Name", " " & oData.ParProcOptName & " a duplicate exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    Dim otblParProcessOption = db.tblParProcessOptions.Where(Function(x) x.ParProcOptControl = iParProcOptControl).FirstOrDefault()
                    Dim skipObjs As New List(Of String) From {"ParProcOptControl", "tblParProcOptTxtItems", "tblParProcOptValItems", "tblParProcess"}
                    Dim strMsg As String
                    otblParProcessOption = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(otblParProcessOption, oData, skipObjs, strMsg)
                    'db.tblParProcessOptions.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblParProcessOption"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific Par Process Options Record
    ''' </summary>
    ''' <param name="iParProcOptControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Par Process Options Logic
    ''' </remarks>
    Public Function DeletetblParProcessOption(ByVal iParProcOptControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iParProcOptControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'verify the record
                Dim oExisting = db.tblParProcessOptions.Where(Function(x) x.ParProcOptControl = iParProcOptControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ParProcOptControl = 0 Then Return True
                db.tblParProcessOptions.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblParProcessOption"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class

''' <summary>
''' Data Access Layer for Workflow Process Option Text Item Data
''' </summary>
''' <remarks>
''' Created by RHR for v-8.5.3.006 on 10/20/2022
''' </remarks>
Public Class NGLtblParProcOptTxtItemData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblParProcOptTxtItems
        Me.LinqDB = db
        Me.SourceClass = "NGLtblParProcOptTxtItemData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblParProcOptTxtItems
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Get a filtered array of Workflow Process Option Text Item Data
    ''' Caller must set filters.ParentControl to ParProcOptTIParProcOptControl (FK)
    ''' or must pass in a filtervalue for filtername ParProcOptTIControl (PK)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Workflow Process Option Text Item Logic
    ''' </remarks>
    Public Function GettblParProcOptTxtItems(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblParProcOptTxtItem()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblParProcOptTxtItem
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblParProcOptTxtItem)
                iQuery = db.tblParProcOptTxtItems
                Dim filterWhere = ""
                If Not (filters.filterName = "ParProcOptTIControl" AndAlso filters.filterValue <> "0") Then
                    'if we are not using the tables primary key as a filter we must use the parent primary key as a filter
                    Dim iParCarControl = filters.ParentControl
                    If filters.ParentControl = 0 Then
                        throwInvalidKeyParentRequiredException(New List(Of String) From {"Workflow Process Option Key", " was not valid and "})
                    End If
                    filterWhere = " (ParProcOptTIParProcOptControl = " & filters.ParentControl.ToString() & ") "
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblParProcOptTxtItems"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Insert or Update Workflow Process Option Text Item Data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Workflow Process Option Text Item Logic
    ''' </remarks>
    Public Function SavetblParProcOptTxtItem(ByVal oData As LTS.tblParProcOptTxtItem) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iParProcOptTIControl As Integer = oData.ParProcOptTIControl
                'Verify that the Parameter Option Key is valid.
                If Not db.tblParProcessOptions.Any(Function(x) x.ParProcOptControl = oData.ParProcOptTIParProcOptControl) Then
                    Dim lDetails As New List(Of String) From {"Workflow Process Option Key", " was not valid and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If 'ParProcOptTIName
                If String.IsNullOrWhiteSpace(oData.ParProcOptTIName) Then
                    Dim lDetails As New List(Of String) From {"Workflow Process Opt Text Item Name", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.ParProcOptTIModDate = Date.Now
                oData.ParProcOptTIModUser = Me.Parameters.UserName

                If (iParProcOptTIControl = 0) Then
                    'this is an insert test if the name already exists
                    If db.tblParProcOptTxtItems.Any(Function(x) String.Compare(x.ParProcOptTIName, oData.ParProcOptTIName, True) = 0 And x.ParProcOptTIParProcOptControl = oData.ParProcOptTIParProcOptControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Opt Text Item Name", " " & oData.ParProcOptTIName & " already exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    db.tblParProcOptTxtItems.InsertOnSubmit(oData)
                Else
                    'This is an update so get the current record,  if it does not exist throw error
                    Dim oParProcOptTI As LTS.tblParProcOptTxtItem = db.tblParProcOptTxtItems.Where(Function(x) x.ParProcOptTIControl = iParProcOptTIControl).FirstOrDefault()
                    If oParProcOptTI Is Nothing OrElse oParProcOptTI.ParProcOptTIControl = 0 Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Opt Text Item Key", " was not found or has been deleted and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    If db.tblParProcOptTxtItems.Any(Function(x) String.Compare(x.ParProcOptTIName, oData.ParProcOptTIName, True) = 0 And x.ParProcOptTIControl <> oData.ParProcOptTIControl And x.ParProcOptTIParProcOptControl = oData.ParProcOptTIParProcOptControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Opt Text Item Name", " " & oData.ParProcOptTIName & " a duplicate exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    Dim otblParProcOptTITxtItem = db.tblParProcOptTxtItems.Where(Function(x) x.ParProcOptTIControl = iParProcOptTIControl).FirstOrDefault()
                    Dim skipObjs As New List(Of String) From {"ParProcOptTIControl", "tblParProcessOption"}
                    Dim strMsg As String
                    otblParProcOptTITxtItem = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(otblParProcOptTITxtItem, oData, skipObjs, strMsg)
                    'db.tblParProcOptTxtItems.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblParProcOptTxtItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific Workflow Process Option Text Item Record
    ''' </summary>
    ''' <param name="iParProcOptTIControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Workflow Process Option Text Item Logic
    ''' </remarks>
    Public Function DeletetblParProcOptTxtItem(ByVal iParProcOptTIControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iParProcOptTIControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'verify the record
                Dim oExisting = db.tblParProcOptTxtItems.Where(Function(x) x.ParProcOptTIControl = iParProcOptTIControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ParProcOptTIControl = 0 Then Return True
                'No validation is requred
                db.tblParProcOptTxtItems.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblParProcOptTxtItem"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"


#End Region

End Class

''' <summary>
''' Data Access Layer for Workflow Process Option Value Item Data
''' </summary>
''' <remarks>
''' Created by RHR for v-8.5.3.006 on 10/20/2022
''' </remarks>
Public Class NGLtblParProcOptValItemData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASSYSDataContext(ConnectionString)
        Me.LinqTable = db.tblParProcOptValItems
        Me.LinqDB = db
        Me.SourceClass = "NGLtblParProcOptValItemData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASSYSDataContext(ConnectionString)
            _LinqTable = db.tblParProcOptValItems
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Get a filtered array of Workflow Process Option Value Item Data
    ''' Caller must set filters.ParentControl to ParProcOptVIParProcOptControl (FK)
    ''' or must pass in a filtervalue for filtername ParProcOptVIControl (PK)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New  Workflow Process Option Value Item Logic
    ''' </remarks>
    Public Function GettblParProcOptValItems(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblParProcOptValItem()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblParProcOptValItem
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblParProcOptValItem)
                iQuery = db.tblParProcOptValItems
                Dim filterWhere = ""
                If Not (filters.filterName = "ParProcOptVIControl" AndAlso filters.filterValue <> "0") Then
                    'if we are not using the tables primary key as a filter we must use the parent primary key as a filter
                    Dim iParCarControl = filters.ParentControl
                    If filters.ParentControl = 0 Then
                        throwInvalidKeyParentRequiredException(New List(Of String) From {"Workflow Category Key", " was not valid and "})
                    End If
                    filterWhere = " (ParProcOptVIParProcOptControl = " & filters.ParentControl.ToString() & ") "
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GettblParProcOptValItems"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Insert or Update Workflow Process Option Value Item Data
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Workflow Process Option Value Item Logic
    ''' </remarks>
    Public Function SavetblParProcOptValItem(ByVal oData As LTS.tblParProcOptValItem) As Boolean
        Dim blnRet As Boolean = False
        If oData Is Nothing Then Return False 'nothing to do

        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                Dim iParProcOptVIControl As Integer = oData.ParProcOptVIControl
                'Verify that the Parameter Option Key is valid.
                If Not db.tblParProcessOptions.Any(Function(x) x.ParProcOptControl = oData.ParProcOptVIParProcOptControl) Then
                    Dim lDetails As New List(Of String) From {"Workflow Process Option Key ", " was not valid and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If 'ParProcOptVIName
                If String.IsNullOrWhiteSpace(oData.ParProcOptVIName) Then
                    Dim lDetails As New List(Of String) From {"Workflow Process Option Value Item Name", " was not provided and "}
                    throwInvalidKeyParentRequiredException(lDetails)
                    Return False
                End If
                oData.ParProcOptVIModDate = Date.Now
                oData.ParProcOptVIModUser = Me.Parameters.UserName

                If (iParProcOptVIControl = 0) Then
                    'this is an insert test if the name already exists
                    If db.tblParProcOptValItems.Any(Function(x) String.Compare(x.ParProcOptVIName, oData.ParProcOptVIName, True) = 0 And x.ParProcOptVIParProcOptControl = oData.ParProcOptVIParProcOptControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Option Value Item Name", " " & oData.ParProcOptVIName & " already exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    db.tblParProcOptValItems.InsertOnSubmit(oData)
                Else
                    'This is an update so get the current record,  if it does not exist throw error
                    Dim oParProcOptVI As LTS.tblParProcOptValItem = db.tblParProcOptValItems.Where(Function(x) x.ParProcOptVIControl = iParProcOptVIControl).FirstOrDefault()
                    If oParProcOptVI Is Nothing OrElse oParProcOptVI.ParProcOptVIControl = 0 Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Option Value Item Key", " was not found or has been deleted and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    If db.tblParProcOptValItems.Any(Function(x) String.Compare(x.ParProcOptVIName, oData.ParProcOptVIName, True) = 0 And x.ParProcOptVIControl <> oData.ParProcOptVIControl And x.ParProcOptVIParProcOptControl = oData.ParProcOptVIParProcOptControl) Then
                        Dim lDetails As New List(Of String) From {"Workflow Process Option Value Item Name", " " & oData.ParProcOptVIName & " a duplicate exists, provide a unique value and "}
                        throwInvalidKeyParentRequiredException(lDetails)
                        Return False
                    End If
                    Dim otblParProcOptVIValItem = db.tblParProcOptValItems.Where(Function(x) x.ParProcOptVIControl = iParProcOptVIControl).FirstOrDefault()
                    Dim skipObjs As New List(Of String) From {"ParProcOptVIControl", "tblParProcessOption"}
                    Dim strMsg As String
                    otblParProcOptVIValItem = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(otblParProcOptVIValItem, oData, skipObjs, strMsg)
                    'db.tblParProcOptValItems.Attach(oData, True)
                End If
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SavetblParProcOptValItem"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Delete a specific Workflow Process Option Value Item Record
    ''' </summary>
    ''' <param name="iParProcOptVIControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.3.006 on 10/20/2022 for New Workflow Process Option Value Item Logic
    ''' </remarks>
    Public Function DeletetblParProcOptValItem(ByVal iParProcOptVIControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        If iParProcOptVIControl = 0 Then Return False 'nothing to do
        Using db As New NGLMASSYSDataContext(ConnectionString)
            Try
                'verify the record
                Dim oExisting = db.tblParProcOptValItems.Where(Function(x) x.ParProcOptVIControl = iParProcOptVIControl).FirstOrDefault()
                If oExisting Is Nothing OrElse oExisting.ParProcOptVIControl = 0 Then Return True
                'No Validation Required just delete
                db.tblParProcOptValItems.DeleteOnSubmit(oExisting)
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblParProcOptValItem"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"




#End Region

End Class

