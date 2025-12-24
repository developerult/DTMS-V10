Imports System.Data.SqlClient
Imports System.IO
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NGLBC = NGL.FreightMaster.Integration

'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration

Public Class clsEDI210Output : Inherits EDIFileCore

    Public Overrides Function Read(Optional ByVal strErrMsg As String = "") As Boolean
        Dim blnRet As Boolean = False
        'Read from the web service 
        Dim o210 As New NGLBC.cls210O
        Dim enumResults As NGLWebReturnValues = NGLWebReturnValues.nglDataIntegrationComplete
        Dim strLastError As String = ""
        With o210
            .AdminEmail = Me.AdminEmail
            .FromEmail = Me.FromEmail
            .GroupEmail = Me.GroupEmail
            .Retry = Me.AutoRetry
            .SMTPServer = Me.SMTPServer
            .DBServer = Me.DBServer
            .Database = Me.Database
            .AuthorizationCode = Me.AuthorizationCode
            enumResults = .getEDI210OutString(FileData, ConnectionString, FileName)
            strLastError = .LastError
        End With


        If enumResults <> NGLWebReturnValues.nglDataIntegrationComplete Then
            Dim strMsg As String = strErrMsg & " read 210 invoice data failed for carrier " & CarrierNumber & " using file " & FileName
            Select Case enumResults
                Case NGLWebReturnValues.nglDataConnectionFailure
                    strMsg &= " due to a database connection failure the actual error message is: " & vbCrLf & strLastError
                Case NGLWebReturnValues.nglDataIntegrationFailure
                    strMsg &= " because none of the data could be processed the actual error message is: " & vbCrLf & strLastError
                Case NGLWebReturnValues.nglDataIntegrationHadErrors
                    strMsg &= " because some of the data could not be processed the actual error message is: " & vbCrLf & strLastError
                Case NGLWebReturnValues.nglDataValidationFailure
                    strMsg &= " due ot a data validation failure the actual error message is: " & vbCrLf & strLastError
                Case Else
                    strMsg &= " due to an unexpected failure the actual error message is: " & vbCrLf & strLastError
            End Select
            LogError(Source & " Read EDI 210 Data Failure", strMsg, Me.AdminEmail)
        Else
            blnRet = True
            'LVV 8/14/14
            'Log("The 204 truckload data for carrier " & CarrierNumber & " using file " & FileName & " has been read.")
        End If
        Return blnRet
    End Function

    Public Function inTimeWindow() As Boolean
        Dim oDateLastRun As Date = Me.LastOutboundTransmission
        Me.Log("Last Run = " & oDateLastRun.ToString)
        Dim blnRet As Boolean = False
        Dim strMsg As String = ""
        Dim oDateToRun As Date = Now
        Dim oDays() = DaysOfWeek.Split(",")
        Dim blnDayFound As Boolean = False
        Select Case oDateToRun.DayOfWeek
            Case DayOfWeek.Monday
                For Each sDay As String In oDays
                    If sDay.ToUpper = "MON" Then
                        blnDayFound = True
                        Exit For
                    End If
                Next
            Case DayOfWeek.Tuesday
                For Each sDay As String In oDays
                    If sDay.ToUpper = "TUE" Then
                        blnDayFound = True
                        Exit For
                    End If
                Next
            Case DayOfWeek.Wednesday
                For Each sDay As String In oDays
                    If sDay.ToUpper = "WED" Then
                        blnDayFound = True
                        Exit For
                    End If
                Next
            Case DayOfWeek.Thursday
                For Each sDay As String In oDays
                    If sDay.ToUpper = "THU" Then
                        blnDayFound = True
                        Exit For
                    End If
                Next
            Case DayOfWeek.Friday
                For Each sDay As String In oDays
                    If sDay.ToUpper = "FRI" Then
                        blnDayFound = True
                        Exit For
                    End If
                Next
            Case DayOfWeek.Saturday
                For Each sDay As String In oDays
                    If sDay.ToUpper = "SAT" Then
                        blnDayFound = True
                        Exit For
                    End If
                Next
            Case DayOfWeek.Sunday
                For Each sDay As String In oDays
                    If sDay.ToUpper = "SUN" Then
                        blnDayFound = True
                        Exit For
                    End If
                Next
        End Select
        If Not blnDayFound Then
            If Me.Debug Then Me.Log("Day of Week Not Scheduled")
            Return False
        End If
        'check if the start time window matches
        Dim oStartDate As Date = Now
        If Not Date.TryParse(oDateToRun.ToShortDateString & " " & StartTime, oStartDate) Then
            If Me.Debug Then Me.Log("Start Time Not Valid: " & oDateToRun.ToShortDateString & " " & StartTime)
            Return False
        End If
        'Check if the date to run is after the start date
        If Not oDateToRun > oStartDate Then
            If Me.Debug Then Me.Log("Date to Run is After start Date")
            Return False
        End If
        Dim oEndDate As Date = Now
        'check if the end time window matches
        If Not Date.TryParse(oDateToRun.ToShortDateString & " " & EndTime, oEndDate) Then
            If Me.Debug Then Me.Log("End time is not valid: " & oDateToRun.ToShortDateString & " " & EndTime)
            Return False
        End If
        'Check if the date to run is after the end date
        If oDateToRun > oEndDate Then
            If Me.Debug Then Me.Log("Date to Run is After end Date")
            Return False
        End If
        'check if the correct number of minutes has passed.
        If (oDateToRun < oStartDate.AddMinutes(SendMinutesOutbound) _
            AndAlso oDateLastRun > oStartDate _
            And oDateLastRun < oDateToRun) Or oDateLastRun.AddMinutes(SendMinutesOutbound) < oDateToRun Then
            Me.LastOutboundTransmission = oDateToRun
            Me.Log("Saving run time: " & oDateToRun.ToString)
            Me.SaveCarrierEDIConfig("888") 'Note: 888 is code for outbound 204
            Return True
        End If
        Return blnRet
    End Function
End Class
