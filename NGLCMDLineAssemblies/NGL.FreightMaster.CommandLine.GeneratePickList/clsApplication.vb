Imports System.IO
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports Ngl.Core.Communication
Imports NGL.FreightMaster.Integration
Imports NGL.FreightMaster.Integration.Configuration

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    Public Sub ProcessData()
        Dim blnLockFileCreated As Boolean = False
        Dim oIO As New NGL.Core.Communication.NGLFileIO

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
        Dim strPickListFile As String = oParameter.getParText("PickListFile")
        If strPickListFile.Trim.Length < 1 Then
            Log("No Pick List File Configured")
            Exit Sub
        End If
        Dim strLockFile As String = oParameter.getParText("PICKLISTPROCESSINGFILE")
        Try
            Log("Reading Pick List Data ")
            Dim oPickList As New clsPickList
            Dim dtPickList As New PickListData.PickListDataTable
            Dim dtPickDetail As New PickListData.PickDetailDataTable
            oConfig.ConnectionString = "Data Source=" & Me.oConfig.DBServer & ";Initial Catalog=" & Me.oConfig.Database & ";Integrated Security=True"
            Dim intRet As Configuration.ProcessDataReturnValues = Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            With oPickList
                .AdminEmail = Me.oConfig.AdminEmail
                .GroupEmail = Me.oConfig.GroupEmail
                .SMTPServer = Me.oConfig.SMTPServer
                .Retry = Me.oConfig.AutoRetry
                .DBServer = Me.oConfig.DBServer
                .Database = Me.oConfig.Database
                .Debug = Me.oConfig.Debug
                .FromEmail = Me.oConfig.FromEmail
                intRet = .readData(dtPickList, dtPickDetail, oConfig.ConnectionString, 3, 10)
            End With
            If strLockFile.Trim.Length > 0 Then
                Log("Testing For Lock File " & strLockFile)
                Dim intCt As Integer = 0
                Try
                    Do While Not oIO.CreateLockFile(strLockFile)
                        If intCt > Me.oConfig.AutoRetry Then
                            LogError(Source & " The Pick List File is in use.", "The lock file " & strLockFile & " has the pick list records locked.  If this error is repeated on more than one cycle then the validity of the lock file should be confirmed." & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
                            Exit Sub
                        End If
                        Log("File Locked Waiting")
                        System.Threading.Thread.Sleep(2000)
                        intCt += 1
                        Log("Retry Attempt # " & intCt)
                    Loop
                    blnLockFileCreated = True
                Catch ex As System.ApplicationException
                    LogError(Source & " Unable to create pick list lock file.", "The lock file " & strLockFile & " could not be created please confirm that the system is working as expected.  The actual error is: " & vbCrLf & vbCrLf & ex.Message & oIO.LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
                    Exit Sub
                Catch ex As Exception
                    Throw
                End Try
            End If
            Log("Writing Pick List File")
            Dim oRow As PickListData.PickListRow
            For i As Integer = 0 To dtPickList.Rows.Count - 1
                oRow = dtPickList.Rows(i)
                'If oIO.DataRowToCSV(oRow, strPickListFile) Then
                If WritePickList(oRow, strPickListFile) Then
                    oPickList.confirmExport(oConfig.ConnectionString, oRow.PLControl)
                Else
                    LogError(Source & " Write Pick List File Failure", "Unable to write PLControl # " & oRow.PLControl.ToString & " to pick list file.  The application will retry on the next cycle." & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
                    Exit Sub
                End If
            Next
            oPickList = Nothing
            Log("Process Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected Error", "An unexpected error has occurred whil attempting to write the pick list file.  The actual error is: " & ex.Message & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)

        Finally
            If blnLockFileCreated Then
                Try
                    oIO.DeleteLockFile(strLockFile)
                Catch ex As Exception
                    LogError("GeneratePickList DeleteLockFile Failure", "Please delete the lock file " & strLockFile & " manually before the next cycle.", Me.AdminEmail)
                End Try
            End If
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
    Private Function WritePickList(ByVal oRow As DataRow, ByVal strFileName As String) As Boolean
        Dim blnRet As Boolean = False
        Dim fi As FileInfo = New FileInfo(strFileName)
        Dim w As StreamWriter
        Dim strSpacer As String = Chr(34)
        If Not File.Exists(strFileName) Then
            w = fi.CreateText
            w.Close()
        End If
        Try
            w = File.AppendText(strFileName)
            Using w

                For x As Integer = 0 To 40 ' oRow.ItemArray.Length - 1
                    Dim strVal As String = NGL.Core.Communication.General.CleanNullableString(oRow.ItemArray(x))
                    If x = 5 Or x = 6 Or x = 20 Then 'format bookdateload, bookdaterequired and BookDateOrdered to match original data
                        Dim dtVal As Date
                        If Date.TryParse(strVal, dtVal) Then
                            strVal = dtVal.ToString("MM/dd/yyyy")
                        End If
                    End If
                    w.Write(strSpacer & strVal)
                    strSpacer = ""","""
                Next
                strSpacer = Chr(34)
                w.Write(strSpacer & vbCrLf)
                w.Flush()
            End Using
            blnRet = True
        Catch ex As System.IO.IOException
            Throw New System.ApplicationException("Invalid File IO Operation for file name " & strFileName)
        Catch ex As Exception
            Throw
        Finally
            Try
                w.Close()
            Catch ex As Exception

            End Try

        End Try
        Return blnRet

    End Function

End Class
