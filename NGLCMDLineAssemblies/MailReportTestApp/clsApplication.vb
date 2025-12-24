Imports System.Net.Mail
Imports System.IO
Imports System.ServiceModel
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports Ngl.FreightMaster.Core
Imports Ngl.Core.Communication
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.Core.Communication.Email
Imports Ngl.Core.Communication.General
Imports NData = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core

Public Class clsApplication : Inherits Ngl.FreightMaster.Core.NGLCommandLineBaseClass

    Protected oConfig As New Ngl.FreightMaster.Core.UserConfiguration
    Protected DBCon As New System.Data.SqlClient.SqlConnection
    Protected rs As New SqlReportingServices.ReportExecutionService
    Protected mail As DataTable
    Private EvtLog As New System.Diagnostics.EventLog

    'Instead of sending a error mail message after each error. We save it off until the end of the loop then loop through the saved errors and send one error email.
    Private SavedSendMailMessagesExceptions As New List(Of LogExceptionError)
    Private SavedGetAttachmentsExceptions As New List(Of LogExceptionError)
    Private SavedGetReportssExceptions As New List(Of LogExceptionError)


    Public blnEmailAddressInvalid As Boolean = False

    Public Sub New()
        MyBase.New()
        EvtLog.Log = "Mail Report Test Application"
        EvtLog.Source = "MailReportTest.clsApplication"
    End Sub


    Public Sub GetReports(ByVal mailControl As String)
        Dim oQuery As New Ngl.Core.Data.Query
        oQuery.ConnectionString = My.Settings.connectionString
        If Not oQuery.testConnection() Then
            Console.WriteLine(" Database Connection Failure", "Actual error reported: " & oQuery.LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            Console.WriteLine("Connection String: " & oQuery.ConnectionString)
            Return
        End If

        ReportServerURL = ReportServerURL.Trim
        If Not Right(ReportServerURL, 1) = "/" Then ReportServerURL += "/"
        rs.Url = ReportServerURL & "ReportExecution2005.asmx"
        Console.WriteLine("ReportServerURL: " & rs.Url)
        rs.Credentials = Net.CredentialCache.DefaultCredentials
        If ReportServerUser <> "" Then
            rs.Credentials = New Net.NetworkCredential(ReportServerUser, ReportServerPass, ReportServerDomain)
        End If
        Dim oQR As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill("SELECT * FROM MailReports WHERE MailControl = " & mailControl)
        If Not oQR.Exception Is Nothing Then
            Console.WriteLine("Get Mail Reports Data Failure. Could not read the MailReports table data because of an unexpected error: " & oQR.Exception.ToString())
            Return
        End If
        Dim reports = oQR.Data
        'modified by RHR for v-8.4.0.003 on 10/07/2021  added parameter data to error messges
        Dim strParameterData As String = ""
        For Each row As DataRow In reports.Rows
            Try
                Log("Adding report " & reports.Rows.IndexOf(row) + 1 & " of " & reports.Rows.Count)
                Dim result As Byte() = Nothing
                Dim historyId As String = Nothing
                Dim encoding As String
                Dim mimeType As String
                Dim extension As String
                Dim warnings As SqlReportingServices.Warning() = Nothing
                Dim streamIds As String() = Nothing
                Dim parameterValues() As SqlReportingServices.ParameterValue = Nothing

                Dim execInfo As New SqlReportingServices.ExecutionInfo
                Dim execHeader As New SqlReportingServices.ExecutionHeader

                rs.ExecutionHeaderValue = execHeader
                execInfo = rs.LoadReport(row("path"), historyId)

                If row("parameters").ToString <> "" Then
                    Dim parameters() As String = row("parameters").ToString.Split("|")
                    For Each parameter As String In parameters
                        Dim parameterStrings() As String = parameter.Split("^")
                        If parameterStrings.Length = 2 Then
                            If parameterValues Is Nothing Then
                                ReDim parameterValues(0)
                            Else
                                ReDim Preserve parameterValues(parameterValues.Length)
                            End If
                            parameterValues(parameterValues.GetUpperBound(0)) = New SqlReportingServices.ParameterValue
                            parameterValues(parameterValues.GetUpperBound(0)).Name = parameterStrings(0)
                            parameterValues(parameterValues.GetUpperBound(0)).Value = parameterStrings(1)
                            strParameterData = strParameterData & " " & parameterStrings(0) & " = " & parameterStrings(1)
                        End If
                    Next
                End If

                If parameterValues IsNot Nothing Then rs.SetExecutionParameters(parameterValues, "en-us")

                Dim sessionId As String = rs.ExecutionHeaderValue.ExecutionID
                result = rs.Render(row("Format"), Nothing, extension, mimeType, encoding, warnings, streamIds)
                If File.Exists("C:\Data\report.pdf") Then
                    File.Delete("C:\Data\report.pdf")
                End If
                File.WriteAllBytes("C:\Data\report.pdf", result)
                Console.WriteLine("Report Saved as: C:\Data\report.pdf")
            Catch ex As Exception
                Console.WriteLine("Unexpected Error: " & ex.ToString())
            End Try
        Next

    End Sub


End Class
