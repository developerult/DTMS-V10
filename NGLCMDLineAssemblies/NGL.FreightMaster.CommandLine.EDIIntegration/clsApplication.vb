Imports System.IO
Imports System.ServiceModel
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports DTran = NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NData = NGL.FreightMaster.Data
Imports NGL.Core

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _MasterAlertMsgList As New Dictionary(Of Integer, String)
    Public Property MasterAlertMsgList() As Dictionary(Of Integer, String)
        Get
            Return _MasterAlertMsgList
        End Get
        Set(ByVal value As Dictionary(Of Integer, String))
            _MasterAlertMsgList = value
        End Set
    End Property


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' Added calls to Process210Data() and ProcessInboundData("820")
    ''' </remarks>
    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()

        Try
            Log("Begin Process Data ")
            Dim oCarriers As List(Of clsDataReference) = Me.GetCarrierList()
            If Not oCarriers Is Nothing AndAlso oCarriers.Count > 0 Then
                For Each carrier In oCarriers
                    ProcessInboundData(carrier, "997")
                    ProcessInboundData(carrier, "990")
                    ProcessInboundData(carrier, "214")
                    ProcessInboundData(carrier, "210")
                    ProcessInboundData(carrier, "820")
                    ProcessInboundData(carrier, "204") 'EDI 204 Inbound
                    Process204Data(carrier)
                    Process210Data(carrier)
                Next
                Log("EDI Carriers Processed: " & oCarriers.Count.ToString)
            End If
            Log("Process Data Complete")
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

    Private Sub Process204Data(ByVal carrier As clsDataReference)

        Dim strFileName As String = ""
        Dim outbound As New clsEDI204Output

        With outbound
            Try
                .cloneTaskParameters(Me)
                .LogFile = Me.LogFile
                .openLog()
                .Log("Reading carrier config data for " & carrier.Number.ToString & ": " & carrier.Name)
                .CarrierControl = carrier.Control
                .CarrierNumber = carrier.Number
                .CarrierName = carrier.Name
                .CreatedDate = Now.ToString
                .CreateUser = "Process 204 EDI Data"
                If Not .ReadCarrierEDIConfig("204") Then Return 'we cannot process this data
                'The logic below allows to work with local file folders
                'If System.Diagnostics.Debugger.IsAttached Then
                '    outbound.LogFile = "C:\Data\EDI\Log\log.txt"
                '    outbound.OutboundFilesFolder = "C:\Data\EDI\Log"
                '    outbound.BackupFolder = "C:\Data\EDI\Log"
                '    outbound.InboundFilesFolder = "C:\Data\EDI\Log"
                '    outbound.EDILogFile = "C:\Data\EDI\Log\EDIlog.txt"
                'End If
                'change to the EDI log file
                .closeLog(0)
                .LogFile = .EDILogFile
                .openLog()
                .Log("Begin " & .Source)
                If Not .inTimeWindow() AndAlso Not System.Diagnostics.Debugger.IsAttached Then
                    .Log("outside time windows 204s are not scheduled to run at this time")
                    Return '204s are not scheduled to run at this time
                End If
                strFileName = Me.SFun.timeStampFileName(Me.SFun.buildPath(.OutboundFilesFolder, .FileNameBaseOutbound & ".edi"), "", True)
                .FileName = strFileName
                If .Read("Read 204 EDI Data") Then
                    .Log("Read Complete")
                    'If .Debug Then .Log(.FileData)
                    If .Save("Save 204 EDI Data") Then
                        If Not String.IsNullOrEmpty(.FileData) AndAlso .FileData.Trim.Length > 50 Then
                            Log("The 204 truckload data for carrier " & .CarrierNumber & " using file " & .FileName & " has been created.")
                            .Log("EDI 204 data saved to file: " & strFileName)
                        End If
                    End If
                End If
            Catch ex As Exception
                Me.LogError(Me.Source & " Process 204 EDI Data Failure", "There was an unexpected error while processing the 204 EDI Data for Carrier " & carrier.Number.ToString & ": " & carrier.Name & ".  The actual error is: " & vbCrLf & readExceptionMessage(ex), Me.AdminEmail)
            Finally
                .closeLog(0)
            End Try
        End With

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="carrier"></param>
    ''' <remarks>
    ''' Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' </remarks>
    Private Sub Process210Data(ByVal carrier As clsDataReference)

        Dim strFileName As String = ""
        Dim outbound As New clsEDI210Output
        With outbound
            Try
                .cloneTaskParameters(Me)
                .LogFile = Me.LogFile
                .openLog()
                .Log("Reading carrier 210 config data for " & carrier.Number.ToString & ": " & carrier.Name)
                .CarrierControl = carrier.Control
                .CarrierNumber = carrier.Number
                .CarrierName = carrier.Name
                .CreatedDate = Now.ToString
                .CreateUser = "Process 210 EDI Data"
                If Not .ReadCarrierEDIConfig("888") Then Return 'we cannot process this data  'Note: 888 is code for outbound 204
                'change to the EDI log file
                .closeLog(0)
                .LogFile = .EDILogFile
                .openLog()
                .Log("Begin " & .Source)
                If Not .inTimeWindow() Then
                    .Log("outside time windows 210s are not scheduled to run at this time")
                    Return '210s are not scheduled to run at this time
                End If
                strFileName = Me.SFun.timeStampFileName(Me.SFun.buildPath(.OutboundFilesFolder, .FileNameBaseOutbound & ".edi"), "", True)
                .FileName = strFileName
                If .Read("Read 210 EDI Data") Then
                    .Log("Read Complete")
                    'If .Debug Then .Log(.FileData)
                    If .Save("Save 210 EDI Data") Then
                        If Not String.IsNullOrEmpty(.FileData) AndAlso .FileData.Trim.Length > 50 Then
                            Log("The 210 invoice data for carrier " & .CarrierNumber & " using file " & .FileName & " has been created.")
                            .Log("EDI 210 data saved to file: " & strFileName)
                        End If
                    End If
                End If
            Catch ex As Exception
                Me.LogError(Me.Source & " Process 210 EDI Data Failure", "There was an unexpected error while processing the 210 EDI Data for Carrier " & carrier.Number.ToString & ": " & carrier.Name & ".  The actual error is: " & vbCrLf & readExceptionMessage(ex), Me.AdminEmail)
            Finally
                .closeLog(0)
            End Try
        End With

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="carrier"></param>
    ''' <param name="EDIXaction"></param>
    ''' <remarks>
    ''' Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' </remarks>
    Private Sub ProcessInboundData(ByVal carrier As clsDataReference, ByVal EDIXaction As String)
        'Process Inbound Data
        Dim strFileFilter As String = ""
        Dim strFileName As String = ""
        Dim oInbound As New clsEDIInbound
        With oInbound
            Try
                .cloneTaskParameters(Me)
                .LogFile = Me.LogFile
                .openLog()
                .Log("Reading carrier config data for " & carrier.Number.ToString & ": " & carrier.Name)
                .CarrierControl = carrier.Control
                .CarrierNumber = carrier.Number
                .CarrierName = carrier.Name
                .CreatedDate = Now.ToString
                .CreateUser = "Process " & EDIXaction & " EDI Data"
                If Not .ReadCarrierEDIConfig(EDIXaction) Then Return 'we cannot process this data
                'change to the EDI log file
                .closeLog(0)
                .LogFile = .EDILogFile
                .openLog()
                strFileFilter = Me.SFun.buildPath(.InboundFilesFolder, .FileNameBaseInbound & "*")
                strFileName = Dir(strFileFilter)

                .dateprocessed = Date.Now 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration

                Do While Not String.IsNullOrEmpty(strFileName)
                    .FileName = Me.SFun.buildPath(.InboundFilesFolder, strFileName)
                    'a file was found so loop through all the 214 files 
                    'confirm that the file exists
                    If System.IO.File.Exists(.FileName) Then
                        'Read the data
                        .FileData = ""
                        Try
                            Log("Reading EDI File: " & .FileName)
                            If .Read("Read " & EDIXaction & " EDI Data") Then
                                '@TODO pass Carrier.Control to the Save method
                                .Save(carrier.Control, "Save " & EDIXaction & " EDI Data")
                                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                'Add to MasterAlertMsgList if needed 
                                If (Not oInbound.AlertMsgList Is Nothing) AndAlso (oInbound.AlertMsgList.Count > 0) Then
                                    AddToMasterAlertMsgList(oInbound.AlertMsgList)
                                End If
                            End If
                            'clear the backup filename 
                            .BackupFileName = ""
                            'Backup original file
                            .Log("Move file " & .FileName & " to back up file " & .BackupFileName & ".")
                            Try
                                'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                                BackupOriginalFileRef(.FileName, .BackupFileName)
                            Catch ex As Exception
                                Dim strMsg As String = "Backup original " & EDIXaction & " file failed for carrier " & carrier.Number & ": " & carrier.Name & ".  The file " & .FileName & " may not have been moved to " & .BackupFileName & ".  Please perform this operation manually before the next batch cycle runs or duplicate messages may be processed.  The actual error is: " & vbCrLf & readExceptionMessage(ex)
                                .LogError(Source & " Backup " & EDIXaction & " EDI File Failure", strMsg, Me.AdminEmail & ";" & Me.GroupEmail)
                            End Try
                        Catch ex As Exception
                            Dim strMsg As String = "There was an unexpected error wheil processing the " & EDIXaction & " data for carrier " & carrier.Number & ": " & carrier.Name & ".  The file " & .FileName & " was not processed correctly.  Please delete the file manually and have the carrier resend the data.  The actual error is: " & vbCrLf & readExceptionMessage(ex)
                            .LogError(Source & " Process " & EDIXaction & " EDI Data Error", strMsg, Me.AdminEmail & ";" & Me.GroupEmail)
                        End Try
                    End If
                    'get the next file
                    strFileName = Dir()
                Loop

                'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
                'Send alerts if needed
                If (Not MasterAlertMsgList Is Nothing) AndAlso (MasterAlertMsgList.Count > 0) Then
                    ProcessMasterAlertMsgList(MasterAlertMsgList)
                End If

            Catch ex As Exception
                Me.LogError(Me.Source & " Process " & EDIXaction & " EDI Data Failure", "There was an unexpected error while processing the " & EDIXaction & " EDI Data for Carrier " & carrier.Number.ToString & ": " & carrier.Name & ".  The actual error is: " & vbCrLf & readExceptionMessage(ex), Me.AdminEmail)
            Finally
                .closeLog(0)
            End Try
        End With
    End Sub

    Public Function GetCarrierList() As List(Of clsDataReference)
        'select distinct dbo.Carrier.CarrierControl, dbo.Carrier.CarrierNumber from dbo.Carrier inner join dbo.CarrierEDI on dbo.carrier.CarrierControl = dbo.carrieredi.CarrierEDICarrierControl where isnull(dbo.CarrierEDI.CarrierEDIXaction,'') in ('204','997','210','214','990') order by CarrierNumber desc
        'Create a data connection 
        Dim oQuery As New NGL.Core.Data.Query(ConnectionString)
        Dim oList As New List(Of clsDataReference)

        Try

            Dim strSQL As String = "select distinct dbo.Carrier.CarrierControl as Control, dbo.Carrier.CarrierNumber as Number, dbo.Carrier.CarrierName as Name from dbo.Carrier inner join dbo.CarrierEDI on dbo.carrier.CarrierControl = dbo.carrieredi.CarrierEDICarrierControl where isnull(dbo.CarrierEDI.CarrierEDIXaction,'') in ('204','997','210','214','990') order by CarrierNumber desc"
            Dim oQRet As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                LogError(Source & " Cannot Get Carrier List", "There was a problem while reading the available EDI Carriers.  The actual error is:" & vbCrLf & readExceptionMessage(oQRet.Exception), Me.AdminEmail)
                Return Nothing
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oItem As New clsDataReference
                    With oItem
                        .Control = DTran.getDataRowValue(oRow, "Control", 0)
                        .Number = DTran.getDataRowValue(oRow, "Number", 0)
                        .Name = DTran.getDataRowValue(oRow, "Name", "UnNamed Carrier")
                    End With
                    oList.Add(oItem)
                Next
            End If
        Catch ex As FaultException
            LogError(Source & " Cannot Get Carrier List", "There was a problem while reading the available EDI Carriers.  The actual error is:" & vbCrLf & readExceptionMessage(ex), Me.AdminEmail)
        Catch ex As System.Data.SqlClient.SqlException
            LogError(Source & " Cannot Get Carrier List", "There was a problem while reading the available EDI Carriers.  The actual error is:" & vbCrLf & readExceptionMessage(ex), Me.AdminEmail)

        Catch ex As InvalidOperationException
            LogError(Source & " Cannot Get Carrier List", "There was a problem while reading the available EDI Carriers.  The actual error is:" & vbCrLf & readExceptionMessage(ex), Me.AdminEmail)
        Catch ex As Exception
            Throw
        Finally
            oQuery = Nothing
        End Try

        Return oList
    End Function

    ''' <summary>
    ''' Adds the alert message and key from local alertMsgList to the MasterAlertMsgList
    ''' if the key does not yet exist. Else, it appends the message to the associated key message pair.
    ''' </summary>
    ''' <param name="alertMsgList"></param>
    ''' <remarks>
    ''' Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' </remarks>
    Public Sub AddToMasterAlertMsgList(ByVal alertMsgList As Dictionary(Of Integer, String))

        For Each key As Integer In alertMsgList.Keys
            If MasterAlertMsgList.ContainsKey(key) Then
                MasterAlertMsgList(key) += alertMsgList(key) + vbCrLf
            Else
                MasterAlertMsgList.Add(key, alertMsgList(key))
            End If
        Next

    End Sub

    ''' <summary>
    ''' Process all alerts in MasterAlertMsgList
    ''' Alerts are grouped by key
    ''' </summary>
    ''' <param name="MasterAlertMsgList"></param>
    ''' <remarks>
    ''' 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    ''' </remarks>
    Public Sub ProcessMasterAlertMsgList(ByVal MasterAlertMsgList As Dictionary(Of Integer, String))

        Dim oWCFPar = New Ngl.FreightMaster.Data.WCFParameters() With {.Database = Me.Database,
                                                              .DBServer = Me.DBServer,
        .WCFAuthCode = "NGLSystem"}

        Dim oBatchData As New Ngl.FreightMaster.Data.NGLBatchProcessDataProvider(oWCFPar)

        For Each key As Integer In MasterAlertMsgList.Keys
            Select Case key
                Case 1
                    oBatchData.executeInsertAlertMessage("Alert210RejectedBy997", 0, 0, 0, 0, "EDI 210 Invoice Rejected Via 997", MasterAlertMsgList(key), "", "", "", "", "")
                    'oSecData.InsertAlertMessageWithEmail("Alert210RejectedBy997", 0, 0, 0, 0, "EDI 210 Invoice Rejected Via 997", MasterAlertMsgList(key), "", "", "", "", "")
                Case 2
                    oBatchData.executeInsertAlertMessage("Alert997CannotBeProcessed", 0, 0, 0, 0, "EDI 997 Cannot be Processed", MasterAlertMsgList(key), "", "", "", "", "")
                    'oSecData.InsertAlertMessageWithEmail("Alert997CannotBeProcessed", 0, 0, 0, 0, "EDI 997 Cannot be Processed", MasterAlertMsgList(key), "", "", "", "", "")
                Case 3
                    oBatchData.executeInsertAlertMessage("Alert210InvoiceStatusUpdateFailure", 0, 0, 0, 0, "EDI 210 Invoice Update Failure", MasterAlertMsgList(key), "", "", "", "", "")
                    'oSecData.InsertAlertMessageWithEmail("Alert210InvoiceStatusUpdateFailure", 0, 0, 0, 0, "EDI 210 Invoice Update Failure", MasterAlertMsgList(key), "", "", "", "", "")
                Case 4
                    LogError("Process 997 Warnings", MasterAlertMsgList(key), Me.AdminEmail)
            End Select
        Next

    End Sub

End Class
