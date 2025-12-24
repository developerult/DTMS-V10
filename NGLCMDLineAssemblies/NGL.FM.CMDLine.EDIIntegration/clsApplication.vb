Imports System.IO
Imports System.ServiceModel
Imports Ngl.FreightMaster.Core
Imports Ngl.Core.Communication
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.Core.Communication.Email
Imports Ngl.Core.Communication.General
Imports NData = Ngl.FreightMaster.Data
Imports Ngl.Core

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    Private _MoveUnprocessedFilesToBackup As Boolean = False
    Public Property MoveUnprocessedFilesToBackup() As Boolean
        Get
            Return _MoveUnprocessedFilesToBackup
        End Get
        Set(ByVal value As Boolean)
            _MoveUnprocessedFilesToBackup = value
        End Set
    End Property

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
                MoveUnprocessedFilesToBackup = MoveUnprocessedFilesToBackupFolder()
                For Each carrier In oCarriers
                    ProcessInboundData(carrier, "997")
                    ProcessInboundData(carrier, "990")
                    ProcessInboundData(carrier, "214")
                    ProcessInboundData(carrier, "210")
                    Process204Data(carrier)
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
                .Log("Reading carrier 204 config data for " & carrier.Number.ToString & ": " & carrier.Name)
                .CarrierControl = carrier.Control
                .CarrierNumber = carrier.Number
                .CarrierName = carrier.Name
                .CreatedDate = Now.ToString
                .CreateUser = "Process 204 EDI Data"
                If Not .ReadCarrierEDIConfig("204") Then Return 'we cannot process this data
                'change to the EDI log file
                .closeLog(0)
                .LogFile = .EDILogFile
                .openLog()
                .Log("Begin " & .Source)
                If Not .inTimeWindow() Then
                    .Log("outside time windows 204s are not scheduled to run at this time")
                    Return '204s are not scheduled to run at this time
                End If
                strFileName = Me.SFun.timeStampFileName(Me.SFun.buildPath(.OutboundFilesFolder, .FileNameBaseOutbound & ".edi"), "", True)
                .FileName = strFileName
                If .Read("Read 204 EDI Data") Then
                    .Log("Read Complete")
                    'If .Debug Then .Log(.FileData)
                    .Save("Save 204 EDI Data")
                    .Log("EDI 204 data saved to file: " & strFileName)
                End If
            Catch ex As Exception
                Me.LogError(Me.Source & " Process 204 EDI Data Failure", "There was an unexpected error while processing the 204 EDI Data for Carrier " & carrier.Number.ToString & ": " & carrier.Name & ".  The actual error is: " & vbCrLf & readExceptionMessage(ex), Me.AdminEmail)
            Finally
                .closeLog(0)
            End Try
        End With

    End Sub


    Private Sub ProcessInboundData(ByVal carrier As clsDataReference, ByVal EDIXaction As String)
        'Process Inbound Data
        Dim strFileFilter As String = ""
        'Dim strFileName As String = ""
        Dim oInbound As New clsEDIInbound
        With oInbound
            Try
                .cloneTaskParameters(Me)
                .LogFile = Me.LogFile
                .openLog()
                .Log("Reading carrier " & EDIXaction & " config data for " & carrier.Number.ToString & ": " & carrier.Name)
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
                If .Verbose Then .Log("Checking for " & EDIXaction & " files in inbound folder: " & .InboundFilesFolder)

                strFileFilter = Me.SFun.buildPath(.InboundFilesFolder, .FileNameBaseInbound & "*")
                If .Verbose Then .Log("Selecting " & EDIXaction & " files using filter: " & strFileFilter)
                Dim strFiles As New List(Of String)
                If Directory.Exists(.InboundFilesFolder) Then
                    strFiles = Directory.GetFiles(.InboundFilesFolder, .FileNameBaseInbound & "*", SearchOption.TopDirectoryOnly).ToList()
                End If
                If Not strFiles Is Nothing AndAlso strFiles.Count > 0 Then
                    .Log("Reading " & strFiles.Count() & " Files.")
                Else
                    .Log("No Files Found.")
                    Return
                End If

                For Each strFileName In strFiles
                    .FileName = strFileName
                    Me.SFun.buildPath(.InboundFilesFolder, strFileName)
                    'Read the data
                    Try
                        .Log("Reading EDI File: " & .FileName)
                        .FileData = ""
                        Dim blnReadData As Boolean = .Read("Read " & EDIXaction & " EDI Data")
                        If blnReadData Or MoveUnprocessedFilesToBackup Then
                            Try
                                'clear the backup filename this will cause the next call to BackupFileName to 
                                'create a new backup file name using filename as the key
                                .BackupFileName = ""
                                If File.Exists(.FileName) Then
                                    'Backup original file
                                    .Log("Move file " & .FileName & " to back up file " & .BackupFileName & ".")
                                    BackupOriginalFile(.FileName, .BackupFileName)
                                    If .Verbose Then .Log("Success: backup file, " & .BackupFileName & ", has been created.")
                                Else
                                    .Log("Cannot backup file, " & .FileName & ", because it is no longer available.  It has been moved or deleted by another process.")
                                End If
                            Catch ex As Exception
                                Dim strMsg As String = "Backup original " & EDIXaction & " file failed for carrier " & carrier.Number & ": " & carrier.Name & ".  The file " & .FileName & " may not have been moved to " & .BackupFileName & ".  Please perform this operation manually before the next batch cycle runs or duplicate messages may be processed.  The actual error is: " & vbCrLf & readExceptionMessage(ex)
                                .LogError(Source & " Backup " & EDIXaction & " EDI File Failure", strMsg, Me.AdminEmail & ";" & Me.GroupEmail)
                            End Try
                        End If
                        If blnReadData Then .Save("Save " & EDIXaction & " EDI Data")

                    Catch ex As Exception
                        Dim strMsg As String = "There was an unexpected error wheil processing the " & EDIXaction & " data for carrier " & carrier.Number & ": " & carrier.Name & ".  The file " & .FileName & " was not processed correctly.  Please delete the file manually and have the carrier resend the data.  The actual error is: " & vbCrLf & readExceptionMessage(ex)
                        .LogError(Source & " Process " & EDIXaction & " EDI Data Error", strMsg, Me.AdminEmail & ";" & Me.GroupEmail)
                    End Try
                Next

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



    Public Function MoveUnprocessedFilesToBackupFolder() As Boolean
        Dim blnRet As Boolean = False
        'Create a data connection 
        Dim oQuery As New NGL.Core.Data.Query(ConnectionString)
        Try

            Dim strSQL As String = "select distinct ParValue from Parameter where ParKey = 'GlobalMoveUnprocessedEDIFilesToBackupFolder'"
            Dim oQRet As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If oQRet.Exception Is Nothing Then
                If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                    Dim oRow As System.Data.DataRow = oQRet.Data.Rows(0)
                    Dim intVal As Integer = DTran.getDataRowValue(oRow, "ParValue", 0)
                    If intVal <> 0 Then blnRet = True
                End If
            End If
        Catch ex As Exception
            'do nothing
        Finally
            oQuery = Nothing
        End Try

        Return blnRet
    End Function


End Class
