Imports NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports Ngl.FreightMaster.Integration
Imports NAV = NGL.FM.NAVIntegration


#Region "Enums"
'None
#End Region

Public Class NAVIntegration
    Private Shared EvtLog As New System.Diagnostics.EventLog
    Private Shared Debug As Boolean = False


    Public Shared Sub Main()
        Dim sSource As String = "NGL.FM.CMDLine.NAVIntegration"
        Dim oApp As New NAV.clsApplication

        EvtLog.Log = "Application"
        EvtLog.Source = sSource
        Try
            Dim strCreateUser As String = "System EDI Integration"
            Dim strCreatedDate As String = Now.ToString
            Dim intRetValue As Integer = 0
            With oApp
                '.Source = sSource
                'If .readCommandLineArgs("NGL.FM.CMDLine.NAVIntegration") = 0 Then
                '    Exit Sub
                'End If
                ''The NAVIntegration Debug Flag is based on the command line argument flag which overrides
                ''The Task Parameters Value returned by getTaskParameters
                'Debug = .Debug
                'If Not .validateDatabase() Then
                '    Exit Sub
                'End If
                'If Not .getTaskParameters Then
                '    Exit Sub
                'End If
                'Try
                '    If Debug Then
                '        EvtLog.WriteEntry("Application Start", EventLogEntryType.Information)
                '    End If
                'Catch ex As Exception
                '    'ignore any errors while writing to the event log
                'End Try
                'Dim oSettings As New NAV.clsNavSettings
                'With oSettings
                '    .DTMSAPExportAutoConfirmation = My.Settings.APExportAutoConfirmation
                '    .DTMSAPExportMaxRetry = My.Settings.APExportMaxRetry
                '    .DTMSAPExportMaxRowsReturned = My.Settings.APExportMaxRowsReturned
                '    .DTMSAPExportRetryMinutes = My.Settings.APExportRetryMinutes
                '    .DTMSNAVWebServiceURL = My.Settings.NGL_FM_CMDLine_NAVIntegration_NAVService_DynamicsTMSWebServices
                '    .DTMSNAVUseDefaultCredentials = True
                '    .DTMSPicklistAutoConfirmation = My.Settings.PicklistAutoConfirmation
                '    .DTMSPicklistMaxRetry = My.Settings.PicklistMaxRetry
                '    .DTMSPicklistMaxRowsReturned = My.Settings.PicklistMaxRowsReturned
                '    .DTMSPicklistRetryMinutes = My.Settings.PicklistRetryMinutes
                '    .DTMSWCFAuthCode = My.Settings.WCFAuthCode
                '    .DTMSWCFTCPURL = My.Settings.WCFTCPURL
                '    .DTMSWCFURL = My.Settings.WCFURL
                '    .DTMSWSAuthCode = My.Settings.WSAuthCode
                'End With
                '.ProcessData(oSettings)
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
