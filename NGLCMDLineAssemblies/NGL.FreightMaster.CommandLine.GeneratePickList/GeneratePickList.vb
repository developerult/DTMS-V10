

Imports NGL.FreightMaster.Core.UserConfiguration
Imports NGL.FreightMaster.Data.ApplicationDataTableAdapters
Imports Ngl.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General

Public Class GeneratePickList
    Private Shared EvtLog As New System.Diagnostics.EventLog
    Private Shared Debug As Boolean = False


    Public Shared Sub Main()
        Dim sSource As String = "NGL.FreightMaster.CommandLine.GeneratePickList"
        Dim oApp As New clsApplication

        EvtLog.Log = "Application"
        EvtLog.Source = sSource
        Try
            Dim strCreateUser As String = "System Generate Pick List"
            Dim strCreatedDate As String = Now.ToString
            Dim intRetValue As Integer = 0
            With oApp
                .Source = sSource
                If .readParameters("NGL.FreightMaster.CommandLine.GeneratePickList") = 0 Then
                    Exit Sub
                End If
                Try
                    If .Debug Then
                        EvtLog.WriteEntry("Application Start", EventLogEntryType.Information)
                    End If
                Catch ex As Exception
                    'ignore any errors while writing to the event log
                End Try
                Debug = .Debug
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

End Class
