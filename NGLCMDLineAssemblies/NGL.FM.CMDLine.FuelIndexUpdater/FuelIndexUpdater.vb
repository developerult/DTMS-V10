Imports Ngl.Core.Utility.DataTransformation
Imports Ngl.Core.Communication.Email
Imports Ngl.Core.Communication.General

Public Class FuelIndexUpdater
    Private Shared EvtLog As New System.Diagnostics.EventLog
    Private Shared Debug As Boolean = False


    Public Shared Sub Main()
        Dim sSource As String = "NGL.FreightMaster.CommandLine.FuelIndexUpdater"
        Dim oApp As New clsApplication

        EvtLog.Log = "Application"
        EvtLog.Source = sSource
        Try
            Dim strCreateUser As String = "System Fuel Index Updater"
            Dim strCreatedDate As String = Now.ToString
            Dim intRetValue As Integer = 0
            With oApp
                .Source = sSource
                If .readCommandLineArgs("NGL.FreightMaster.CommandLine.FuelIndexUpdater") = 0 Then
                    Exit Sub
                End If
                'The FuelIndexUpdater Debug Flag is based on the command line argument flag which overrides
                'The Task Parameters Value returned by getTaskParameters
                Debug = .Debug
                If Not .validateDatabase() Then
                    Exit Sub
                End If
                If Not .getTaskParameters Then
                    Exit Sub
                End If
                Try
                    If Debug Then
                        EvtLog.WriteEntry("Application Start", EventLogEntryType.Information)
                    End If
                Catch ex As Exception
                    'ignore any errors while writing to the event log
                End Try
                .ProcessData()
            End With


        Catch ex As ApplicationException
            If Debug Then
                Console.WriteLine("Error! " & ex.ToString)
            End If
            EvtLog.WriteEntry(formatErrMsg(ex, sSource, Debug), EventLogEntryType.Error)

        Catch ex As Exception
            If Debug Then
                Console.WriteLine("Error! " & ex.ToString)
            End If
            EvtLog.WriteEntry(formatErrMsg(ex, sSource, Debug), EventLogEntryType.Error)
        Finally
            If Debug Then
                Console.WriteLine("Press Enter To Continue")
                Console.ReadLine()
            End If
        End Try
    End Sub

    Public Class CompResults
        Public CompControl As Integer = 0
        Public CompNumber As Integer = 0
        Public ErrMsg As String = ""
        Public UpdateMsg As String = ""
        Public UpdateCount As Integer = 0
        Public ErrCount As Integer = 0
    End Class
End Class
