Imports NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NGL.FreightMaster.Integration


#Region "Enums"

Public Enum DataValidationType
    DateField
    LongIntField
    SmallIntField
    TinyIntField
    StringField
    FloatField
    BitField
    MoneyField
    TimeField
End Enum

Public Enum DataIntegrationResults
    Complete
    ConnectionFailure
    ValidationFailure
    IntegrationFailure
    HadErrors
    NoFile
    NoRecords
End Enum


Public Enum RowValidationError
    UnexpectedError
    SaveRecordFailure
    InvalidNullValue
    InvalidDataType
    RowShort
    RowInFooter
    InvalidRow
End Enum

Public Enum NGLWebReturnValues
    nglDataIntegrationComplete
    nglDataConnectionFailure
    nglDataValidationFailure
    nglDataIntegrationFailure
    nglDataIntegrationHadErrors
End Enum


#End Region

Public Class SilentTenderLoadTest
    Private Shared EvtLog As New System.Diagnostics.EventLog
    Private Shared Debug As Boolean = False


    Public Shared Sub Main()
        Dim sSource As String = "NGL.FM.CMDLine.SilentTenderLoadTest"
        Dim oApp As New clsApplication

        EvtLog.Log = "Application"
        EvtLog.Source = sSource
        Try
            Dim strCreateUser As String = "Load Test"
            Dim strCreatedDate As String = Now.ToString
            Dim intRetValue As Integer = 0
            With oApp
                .Source = sSource
                If .readCommandLineArgs("NGL.FM.CMDLine.SilentTenderLoadTest") = 0 Then
                    Exit Sub
                End If
                Debug = True
                If Not .validateDatabase() Then
                    Exit Sub
                End If
                If Not .getTaskParameters Then
                    Exit Sub
                End If
               
                .StartApp()
                .AdminEmail = My.Settings.Email
                .GroupEmail = My.Settings.Email
                If .ReadData Then
                    For i = 1 To My.Settings.LoadCount
                        .ProcessDataAsync(i)
                    Next
                    .StartTest = True
                End If
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
            Console.WriteLine("Press Enter To Continue")
            Console.ReadLine()
            oApp.EndApp()

        End Try
    End Sub

End Class
