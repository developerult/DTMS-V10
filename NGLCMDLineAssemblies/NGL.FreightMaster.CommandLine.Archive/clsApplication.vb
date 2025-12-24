Imports System.IO
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports NGL.FreightMaster.Integration
Imports NGL.FreightMaster.Integration.Configuration

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    Public Sub ProcessData()
        Dim blnLockFileCreated As Boolean = False
        Dim oIO As New NGL.Core.Communication.NGLFileIO
        Dim NumberOfDaysBack As Integer = 0

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
        Log("Reading Par Values From DB")
        Dim oParameter As New NGL.FreightMaster.Core.Model.Parameter(Me.oConfig)

        Dim objIniFile As New IniFile(APPPath() & "\FreightMaster.ini")
        Dim strArchiveDaysBack As String = objIniFile.GetString(Me.INIKey, "BookArchiveDaysBack", "(none)")
        Dim strArchiveLocalBackupFolder As String = objIniFile.GetString(Me.INIKey, "BookArchiveLocalBackupPath", "(none)")
        Dim backupFileName As String = objIniFile.GetString(Me.INIKey, "BookArchiveFileName", "(none)")

        If Not Len(Trim(strArchiveDaysBack)) > 0 Or _
        Not Len(Trim(strArchiveLocalBackupFolder)) > 0 Or _
         Not Len(Trim(backupFileName)) > 0 Then

            Log("No Archive Parameter Configured")
            Exit Sub
        End If

        Try
            NumberOfDaysBack = CType(strArchiveDaysBack, Integer)
            Log("Archiving records ")
         
            oConfig.ConnectionString = "Data Source=" & Me.oConfig.DBServer & ";Initial Catalog=" & Me.oConfig.Database & ";Integrated Security=True"
            Dim intRet As Configuration.ProcessDataReturnValues = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure

            Dim con As SqlClient.SqlConnection = oQuery.getNewConnection()
            If Not oQuery.openConnection(con) Then
                LogError(Source & " Database Connection Failure", "Actual error reported: " & oQuery.LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
                Exit Sub
            End If
            'Console.WriteLine("Execute BookArchive" & " " & NumberOfDaysBack & ", " & oQuery.Database & ", '" & backupFileName & "', '" & strArchiveLocalBackupFolder & "'")
            Dim sql As SqlClient.SqlCommand = New SqlClient.SqlCommand("Execute BookArchive" & " " & NumberOfDaysBack & ", [" & oQuery.Database & "], '" & backupFileName & "', '" & strArchiveLocalBackupFolder & "'")

            If oQuery.execNGLStoredProcedure(con, sql, "BookArchive") Then
                Console.WriteLine("Archive Success!")
            Else
                Console.WriteLine("Archive was not successfull")
            End If
 

        Catch ex As Exception
            LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to archive book table.  The actual error is: " & ex.Message & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)

        Finally

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
     

End Class
