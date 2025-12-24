Imports System.IO
Imports System.ServiceModel
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports PCM = NGL.Service.PCMiler64
'Modified by RHR v-7.0.5.110 6/28/2016
'Imports PCMDebug = NGL.Service.PCMiler.Debug


''' <summary>
''' 
''' </summary>
''' <remarks>
''' Modified by RHR v-7.0.5.110 6/28/2016  I have removed the reference to the PC miler Debug library it is no longer being used.
''' </remarks>
Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration


    Protected _DistRefs As New List(Of clsDistanceRef)
    Protected _StopData As NGLtblStopData
    Protected _QueData As NGLtblDistanceQueueData
    Protected _StopDetData As NGLtblStopDistanceData
    Protected _QueDetData As NGLtblDistanceQueueDetailData
    'Modified by RHR v-7.0.5.110 6/28/2016
    'Protected _PCMDebug As PCMDebug.PCMiles
    Protected _PCM As PCM.PCMiles


    Protected _Parameter As New WCFParameters

    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()

        With _Parameter
            .Database = Me.Database
            .DBServer = Me.DBServer
            .ConnectionString = Me.ConnectionString
            .UserName = "NGLSystem"
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With
        Try
            'Create the data objects
            _StopData = New NGLtblStopData(_Parameter)
            _StopData.ConnectionString = Me.ConnectionString
            _QueData = New NGLtblDistanceQueueData(_Parameter)
            _QueData.ConnectionString = Me.ConnectionString
            _StopDetData = New NGLtblStopDistanceData(_Parameter)
            _StopDetData.ConnectionString = Me.ConnectionString
            _QueDetData = New NGLtblDistanceQueueDetailData(_Parameter)
            _QueDetData.ConnectionString = Me.ConnectionString

            'Step 1: Check if an existing process is already running.
            Log("Check if an existing process is already running")
            Dim oQueue As DTO.tblDistanceQueue = _QueData.GetQueueToRun()
            If oQueue Is Nothing Then
                Log("Nothing To Do; no data exists in the queue")
                Return
            End If

            If oQueue.DistanceQueueStartDate.HasValue Then
                If oQueue.DistanceQueueStartDate.Value.AddMinutes(My.Settings.WarningMinutes) < Date.Now Then
                    'The process has been running for more than 60 minutes
                    'Check if the message has a date
                    Dim dtPrevious As Date
                    If Date.TryParse(oQueue.DistanceQueueMessage, dtPrevious) Then
                        If DateDiff(DateInterval.Minute, dtPrevious, Date.Now) > My.Settings.WarningMinutes Then
                            Dim intRuningMinutes As Integer = DateDiff(DateInterval.Minute, oQueue.DistanceQueueStartDate.Value, Date.Now)
                            LogError(My.Settings.WarningSubject, String.Format(My.Settings.WarningMsg, intRuningMinutes), Me.AdminEmail)
                            Log("Updating the queue with last warning message date and time")
                            oQueue.DistanceQueueMessage = Date.Now.ToString
                            Try
                                _QueData.UpdateRecordNoReturn(oQueue)
                            Catch ex As Exception
                                'just log the exception
                                Log("Save last warning message date error: " & ex.Message)
                            End Try
                        End If
                    Else
                        Log("Updating the queue with last warning message date and time")
                        oQueue.DistanceQueueMessage = Date.Now.ToString
                        Try
                            _QueData.UpdateRecordNoReturn(oQueue)
                        Catch ex As Exception
                            'just log the exception
                            Log("Save last warning message date error: " & ex.Message)
                        End Try
                    End If
                End If
                Log("Process already running")
                Return
            End If
            'Step 2: Update the start date
            oQueue.DistanceQueueStartDate = Date.Now
            Try
                Dim oRet = _QueData.UpdateRecordQuick(oQueue)
                With oQueue
                    .DistanceQueueModDate = oRet.ModDate
                    .DistanceQueueModUser = oRet.ModUser
                    .DistanceQueueUpdated = oRet.Updated
                End With
            Catch sqlEx As FaultException(Of SqlFaultInfo)
                LogError(Source & " Save Start Date Failure: " & sqlEx.Reason.ToString, "Please check that you are using the correct information. The actual error is: " & sqlEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)
                Return
            Catch conflictEx As FaultException(Of ConflictFaultInfo)
                LogError(Source & " Save Start Date Failure (Conflict Error): " & conflictEx.Reason.ToString, conflictEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)
            Catch ex As Exception
                Throw
            End Try

            'Step 3: Check if this is a Run all
            Dim blnQueProcessed As Boolean = False
            Dim strMessage As String = "No stops were updated."
            Try 'We must undo the start date on error so the queue can be processed on the next batch
                'Modified by RHR v-7.0.5.110 6/28/2016
                'If Me.Debug Then
                '    _PCMDebug = New PCMDebug.PCMiles
                'Else
                _PCM = New PCM.PCMiles
                'End If
                If oQueue.DistanceQueueRunAll Then
                    Log("Running All")
                    Dim StopItem As DTO.tblStop = _StopData.GetFirstRecord(0, 0)
                    Do While Not StopItem Is Nothing AndAlso StopItem.StopControl <> 0
                        updateStopMiles(StopItem.StopControl)
                        StopItem = _StopData.GetNextRecord(StopItem.StopControl, 0)
                    Loop
                Else
                    Log("Processing Queue")
                    'Get the details
                    Dim intQueControl As Integer = oQueue.DistanceQueueControl

                    Dim ProcessedStops As New List(Of Integer)
                    Dim oQueDet As DTO.tblDistanceQueueDetail = _QueDetData.GetFirstRecord(0, intQueControl)
                    Do While Not oQueDet Is Nothing AndAlso oQueDet.DistanceQueueDetailControl <> 0
                        Dim StopControl As Integer = oQueDet.DistanceQueueDetailStopControl
                        If Not ProcessedStops.Contains(StopControl) Then
                            If updateStopMiles(StopControl) Then ProcessedStops.Add(StopControl)
                        End If
                        Try
                            oQueDet = _QueDetData.GetNextRecord(oQueDet.DistanceQueueDetailControl, intQueControl)
                        Catch sqlEx As FaultException(Of SqlFaultInfo)
                            If sqlEx.Detail.Message = "E_NoData" Then
                                Log("Last stop in queue")
                                oQueDet = Nothing
                                Exit Do
                            Else
                                LogError(Source & " data access failure: " & sqlEx.Reason.ToString, "Please check that you are using the correct information. The actual error is: " & sqlEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)
                            End If
                        End Try
                    Loop
                    If ProcessedStops.Count > 0 Then strMessage = ProcessedStops.Count.ToString & " stops were updated"

                    Log(strMessage)
                End If
                blnQueProcessed = True

            Catch sqlEx As FaultException(Of SqlFaultInfo)
                strMessage = sqlEx.Reason.ToString & ":  " & sqlEx.Detail.Message
                LogError(Source & " data access failure: " & sqlEx.Reason.ToString, "Please check that you are using the correct information. The actual error is: " & sqlEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)

            Catch conflictEx As FaultException(Of ConflictFaultInfo)
                strMessage = conflictEx.Reason.ToString & ":  " & conflictEx.Detail.Message
                LogError(Source & " Save Data Failure (Conflict Error): " & conflictEx.Reason.ToString, conflictEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)
            Catch ex As System.Net.WebException
                strMessage = ex.Message
                LogError(Source & " process data failure.", "Please check that you are using the correct information. The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
            Catch ex As Exception
                strMessage = ex.Message
                LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to process the distance queue information.  The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)

            Finally
                Try
                    'Step 4: Mark the Queue as complete
                    If blnQueProcessed Then
                        Log("Mark the Queue as complete")
                        oQueue.DistanceQueueEndDate = Date.Now
                        oQueue.DistanceQueueComplete = True
                        oQueue.DistanceQueueMessage = strMessage
                    Else
                        oQueue.DistanceQueueStartDate = Nothing
                        oQueue.DistanceQueueMessage = "Errors were reported: " & strMessage
                    End If
                    _QueData.UpdateRecordNoReturn(oQueue)
                Catch sqlEx As FaultException(Of SqlFaultInfo)
                    LogError(Source & " data access failure: " & sqlEx.Reason.ToString, "Please check that you are using the correct information. The actual error is: " & sqlEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)
                Catch conflictEx As FaultException(Of ConflictFaultInfo)
                    LogError(Source & " Save Data Failure (Conflict Error): " & conflictEx.Reason.ToString, conflictEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)
                Catch ex As System.Net.WebException
                    LogError(Source & " process data failure.", "Please check that you are using the correct information. The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
                Catch ex As Exception
                    LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to process the distance queue information.  The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
                End Try

            End Try

            Log("Process Data Complete")
        Catch sqlEx As FaultException(Of SqlFaultInfo)
            If sqlEx.Detail.Message = "E_NoData" Then
                Log("Nothing To Do. No data exists in the queue")
            Else
                LogError(Source & " data access failure: " & sqlEx.Reason.ToString, "Please check that you are using the correct information. The actual error is: " & sqlEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)
            End If
        Catch conflictEx As FaultException(Of ConflictFaultInfo)
            LogError(Source & " Save Data Failure (Conflict Error): " & conflictEx.Reason.ToString, conflictEx.Detail.Message & vbCrLf & vbCrLf, Me.AdminEmail)
        Catch ex As System.Net.WebException
            LogError(Source & " process data failure.", "Please check that you are using the correct information. The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
        Catch ex As Exception
            LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to process the distance queue information.  The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)

        Finally

        End Try
    End Sub

    Public Function updateStopMiles(ByVal StopControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim strErrorMsgs As New List(Of String)
        Try
            Dim oStop As DTO.tblStop = _StopData.GetRecordFiltered(StopControl)
            If oStop Is Nothing Then
                Log("Skipping stop because the stop control number, " & StopControl.ToString & ", cannot be found.")
                Return False
            End If

            Dim oOrig As New PCM.clsAddress
            With oOrig
                .strAddress = oStop.StopAddress1
                .strCity = oStop.StopCity
                .strState = oStop.StopState
                .strZip = oStop.StopZip
            End With
            Dim oStopTo As DTO.tblStop = _StopData.GetFirstRecord(0, 0)
            Dim dblBatchID As Double = CDbl(Format(Now(), "MddyyyyHHmmss"))
            Dim arrBaddAddressesI() As Ngl.Interfaces.clsPCMBadAddress
            Dim arrBaddAddresses() As PCM.clsPCMBadAddress

            Do While Not oStopTo Is Nothing
                Dim oDest As New PCM.clsAddress
                With oDest
                    .strAddress = oStopTo.StopAddress1
                    .strCity = oStopTo.StopCity
                    .strState = oStopTo.StopState
                    .strZip = oStopTo.StopZip
                End With
                Dim dblMiles As Double = 0

                'Modified by RHR v-7.0.5.110 6/28/2016
                'If Me.Debug Then
                '    Dim oOrigI As New Ngl.Interfaces.clsAddress
                '    With oOrigI
                '        .strAddress = oOrig.strAddress
                '        .strCity = oOrig.strCity
                '        .strState = oOrig.strState
                '        .strZip = oOrig.strZip
                '    End With
                '    Dim oDestI As New Ngl.Interfaces.clsAddress
                '    With oDestI
                '        .strAddress = oDest.strAddress
                '        .strCity = oDest.strCity
                '        .strState = oDest.strState
                '        .strZip = oDest.strZip
                '    End With
                'Dim oDebugRet As PCMDebug.clsGlobalStopData = _PCMDebug.getPracticalMiles(oOrigI, oDestI, 0, 0, 0, StopControl, 0, "", "", 0, dblBatchID, True, arrBaddAddressesI)
                'If String.IsNullOrEmpty(_PCMDebug.LastError) Then
                '    dblMiles = oDebugRet.TotalMiles
                'Else
                'Log("PCM Error: " & _PCMDebug.LastError)
                'End If
                'Else

                Dim oRet As PCM.clsGlobalStopData = _PCM.getPracticalMiles(oOrig, oDest, 0, 0, 0, StopControl, 0, "", "", 0, dblBatchID, True, arrBaddAddresses)
                If String.IsNullOrEmpty(_PCM.LastError) Then
                    dblMiles = oRet.TotalMiles
                Else
                    Log("PCM Error: " & _PCM.LastError)
                End If

                'Modified by RHR v-7.0.5.110 6/28/2016
                'End If
                If dblMiles > 0 Then
                    If Not haveStopsBeenUsed(StopControl, oStopTo.StopControl) Then
                        Try

                            Dim oStopDists As DTO.tblStopDistance()
                            'Update the distance for records where stopcontrol is from control
                            Try
                                oStopDists = _StopDetData.GettblStopDistancesFiltered(StopControl, oStopTo.StopControl, 1, 1)
                            Catch sqlEx As FaultException(Of SqlFaultInfo)
                                If sqlEx.Detail.Message <> "E_NoData" Then
                                    strErrorMsgs.Add("Read tblStopDistance data failure for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & sqlEx.Reason.ToString & "    " & sqlEx.Detail.Message)
                                End If
                            Catch conflictEx As FaultException(Of ConflictFaultInfo)
                                strErrorMsgs.Add("Read tblStopDistance data conflict error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & conflictEx.Reason.ToString & "    " & conflictEx.Detail.Message)
                            Catch ex As System.Net.WebException
                                strErrorMsgs.Add("Read tblStopDistance error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & ex.Message)
                            Catch ex As Exception
                                strErrorMsgs.Add("Read tblStopDistance unexpected error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & ex.Message)
                            End Try

                            If Not oStopDists Is Nothing AndAlso oStopDists.Count > 0 Then
                                With oStopDists(0)
                                    .StopDistanceRoadMiles = dblMiles
                                End With
                                _StopDetData.UpdateRecordNoReturn(oStopDists(0))
                            Else
                                Dim ostopDist As New DTO.tblStopDistance
                                With ostopDist
                                    .StopDistanceFromStopControl = StopControl
                                    .StopDistanceRoadMiles = dblMiles
                                    .StopDistanceToStopControl = oStopTo.StopControl
                                End With
                                _StopDetData.CreateRecord(ostopDist)
                            End If

                            'Update distance for stop where stop from = stopto is from control
                            Try
                                oStopDists = _StopDetData.GettblStopDistancesFiltered(oStopTo.StopControl, StopControl, 1, 1)
                            Catch sqlEx As FaultException(Of SqlFaultInfo)
                                If sqlEx.Detail.Message <> "E_NoData" Then
                                    strErrorMsgs.Add("Read tblStopDistance data failure for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & sqlEx.Reason.ToString & "    " & sqlEx.Detail.Message)
                                End If
                            Catch conflictEx As FaultException(Of ConflictFaultInfo)
                                strErrorMsgs.Add("Read tblStopDistance data conflict error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & conflictEx.Reason.ToString & "    " & conflictEx.Detail.Message)
                            Catch ex As System.Net.WebException
                                strErrorMsgs.Add("Read tblStopDistance error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & ex.Message)
                            Catch ex As Exception
                                strErrorMsgs.Add("Read tblStopDistance unexpected error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & ex.Message)
                            End Try

                            If Not oStopDists Is Nothing AndAlso oStopDists.Count > 0 Then
                                With oStopDists(0)
                                    .StopDistanceRoadMiles = dblMiles
                                End With
                                _StopDetData.UpdateRecordNoReturn(oStopDists(0))
                            Else
                                Dim ostopDist As New DTO.tblStopDistance
                                With ostopDist
                                    .StopDistanceFromStopControl = oStopTo.StopControl
                                    .StopDistanceRoadMiles = dblMiles
                                    .StopDistanceToStopControl = StopControl
                                End With
                                _StopDetData.CreateRecord(ostopDist)
                            End If

                        Catch sqlEx As FaultException(Of SqlFaultInfo)
                            If sqlEx.Detail.Message <> "E_NoData" Then
                                strErrorMsgs.Add("Update tblStopDistance data failure for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & sqlEx.Reason.ToString & "    " & sqlEx.Detail.Message)
                            End If
                        Catch conflictEx As FaultException(Of ConflictFaultInfo)
                            strErrorMsgs.Add("Update tblStopDistance data conflict error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & conflictEx.Reason.ToString & "    " & conflictEx.Detail.Message)
                        Catch ex As System.Net.WebException
                            strErrorMsgs.Add("Update tblStopDistance error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & ex.Message)
                        Catch ex As Exception
                            strErrorMsgs.Add("Update tblStopDistance unexpected error for stop from " & StopControl.ToString & " to " & oStopTo.StopControl.ToString & ": " & ex.Message)
                        End Try
                    End If
                    UpdateUsedStops(StopControl, oStopTo.StopControl)
                End If

                Try
                    oStopTo = _StopData.GetNextRecord(oStopTo.StopControl, 0)
                Catch sqlEx As FaultException(Of SqlFaultInfo)
                    If sqlEx.Detail.Message <> "E_NoData" Then
                        Log("Last Stop Record In Loop")
                    Else
                        Throw
                    End If

                Catch ex As Exception
                    Throw
                End Try
            Loop
            blnRet = True
        Catch sqlEx As FaultException(Of SqlFaultInfo)
            If sqlEx.Detail.Message = "E_NoData" Then
                Log("Nothing To Do. No stop data exists.")
            Else
                strErrorMsgs.Add("Update Stop Miles failure: " & sqlEx.Reason.ToString & "    " & sqlEx.Detail.Message)
            End If
        Catch conflictEx As FaultException(Of ConflictFaultInfo)
            strErrorMsgs.Add("Update Stop Miles failure: " & conflictEx.Reason.ToString & "    " & conflictEx.Detail.Message)
        Catch ex As System.Net.WebException
            strErrorMsgs.Add("Update Stop Miles failure: " & ex.Message)
        Catch ex As Exception
            strErrorMsgs.Add("Update Stop Miles failure: " & ex.Message)
        Finally
            If strErrorMsgs.Count > 0 Then
                Dim sMsg As String = ""
                For Each s In strErrorMsgs
                    sMsg &= s & vbCrLf & vbCrLf
                Next
                LogError(Source & ".updateStopMiles Errors", sMsg, Me.AdminEmail)
            End If
        End Try
        Return blnRet
    End Function


    Private Function haveStopsBeenUsed(ByVal Stop1 As Integer, ByVal Stop2 As Integer) As Boolean
        Dim oDistRef As clsDistanceRef = (From d In _DistRefs Where d.Stop1 = Stop1 And d.Stop2 = Stop2 Select New clsDistanceRef With {.Stop1 = d.Stop1, .Stop2 = d.Stop2}).FirstOrDefault

        If oDistRef Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub UpdateUsedStops(ByVal Stop1 As Integer, ByVal Stop2 As Integer)
        _DistRefs.Add(New clsDistanceRef(Stop1, Stop2))
        _DistRefs.Add(New clsDistanceRef(Stop2, Stop1))
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



    Public Function DecodeAuth(ByVal strAuthNbr As String) As String

        Dim passnumber As Double = 0
        Dim passresult As Double = 0
        Dim passtext1 As String = ""
        Dim passtext2 As String = "*** NONE ***"
        Dim passfraction As Double = 0


        If Len(strAuthNbr) > 0 Then
            passnumber = CDbl(Val(strAuthNbr))
            passresult = passnumber - 11111111111.0#
            passresult = passresult / 24124
            passfraction = passresult - Int(passresult)
            If passfraction > 0 Then passresult = 19000101
            passtext1 = Trim(Str(passresult))
            passtext2 = Mid$(passtext1, 5, 2) & "/" & Mid$(passtext1, 7, 2) & "/" & Left$(passtext1, 4)

        End If

        Return passtext2

    End Function


End Class
