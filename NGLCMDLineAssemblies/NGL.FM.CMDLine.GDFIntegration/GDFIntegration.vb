Imports System
Imports System.IO
Imports NGL.FreightMaster.Core.UserConfiguration
Imports NGL.FreightMaster.Data.ApplicationDataTableAdapters
Imports NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports TCSV = NGL.FM.TMSGRECSVIntegration

Public Class GDFIntegration
    Private Shared EvtLog As New System.Diagnostics.EventLog
    Private Shared Debug As Boolean = False

    Public Shared Sub Main()
        Dim sSource As String = "NGL.FM.CMDLine.GDFIntegration"
        'Dim oIntegration As New TCSV.clsApplication()
        'oIntegration.ProcessData()
        Dim oApp As New TCSV.clsApplication()

        EvtLog.Log = "Application"
        EvtLog.Source = sSource
        Try

            Dim strCreateUser As String = "System GDF Integration"
            Dim strCreatedDate As String = Now.ToString
            Dim intRetValue As Integer = 0
            With oApp
                .Source = sSource
                If .readCommandLineArgs("NGL.FM.CMDLine.GDFIntegration") = 0 Then
                    Exit Sub
                End If
                'The Debug Flag is based on the command line argument flag which overrides
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

End Class
