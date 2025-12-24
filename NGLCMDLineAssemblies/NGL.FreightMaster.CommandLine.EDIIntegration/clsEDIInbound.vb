Imports System.Data.SqlClient
Imports System.IO
Imports Ngl.FreightMaster.Core
Imports Ngl.Core.Communication
Imports Ngl.Core.Utility.DataTransformation
Imports Ngl.Core.Communication.Email
Imports Ngl.Core.Communication.General
Imports NGLBC = NGL.FreightMaster.Integration

Public Class clsEDIInbound : Inherits EDIFileCore

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _AlertMsgList As Dictionary(Of Integer, String)
    Public Property AlertMsgList() As Dictionary(Of Integer, String)
        Get
            Return _AlertMsgList
        End Get
        Set(ByVal value As Dictionary(Of Integer, String))
            _AlertMsgList = value
        End Set
    End Property

    'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
    Private _DateProcessed As System.Nullable(Of Date)
    Public Property DateProcessed() As System.Nullable(Of Date)
        Get
            Return _DateProcessed
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _DateProcessed = value
        End Set
    End Property

    '
    ''' <summary>
    ''' This overload uses the integration EDI tools to save the data from the file to the database
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="strErrMsg"></param>
    ''' <returns>
    ''' Modified this method to accept parameter CarrierControl and to pass the CarrierControl on to the processData method below
    ''' Modified by RHR for v-7.0.6.105 On 6/2/2017
    '''   added logic for EDI Config File
    ''' </returns>
    Public Overloads Function Save(ByVal CarrierControl As Integer, Optional ByVal strErrMsg As String = "") As Boolean
        Dim blnRet As Boolean = False
        'write results to the database 
        Dim oEDIInput As New NGLBC.clsEDIInput
        Dim enumResults As NGLWebReturnValues = NGLWebReturnValues.nglDataIntegrationComplete
        Dim strLastError As String = ""
        Dim str997sOut As String = ""
        With oEDIInput
            .AdminEmail = Me.AdminEmail
            .FromEmail = Me.FromEmail
            .GroupEmail = Me.GroupEmail
            .Retry = Me.AutoRetry
            .SMTPServer = Me.SMTPServer
            .DBServer = Me.DBServer
            .Database = Me.Database
            .AuthorizationCode = Me.AuthorizationCode
            'Modified by LVV 3/2/16 for v-7.0.5.1 EDI Migration
            'Modified by RHR for v-7.0.6.105 On 6/2/2017
            '  added InboundConfigFileName
            enumResults = .ProcessData(FileData, Me.ConnectionString, CarrierControl, BackupFileName, DateProcessed, InboundConfigFileName, Me.CarrierEDISend997)
            'check for any 997s to go out
            str997sOut = .EDI997Response
            strLastError = .LastError
        End With

        'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
        Me.AlertMsgList = oEDIInput.AlertMsgList

        If enumResults <> NGLWebReturnValues.nglDataIntegrationComplete Then
            Dim strMsg As String = strErrMsg & " process EDI input data failed for carrier " & CarrierNumber & " using file " & FileName & ""
            Select Case enumResults
                Case NGLWebReturnValues.nglDataConnectionFailure
                    strMsg &= " due to a database connection failure the actual error message is: " & vbCrLf & strLastError
                Case NGLWebReturnValues.nglDataIntegrationFailure
                    strMsg &= " because none of the data could be processed the actual error message is: " & vbCrLf & strLastError
                Case NGLWebReturnValues.nglDataIntegrationHadErrors
                    strMsg &= " because some of the data could not be processed the actual error message is: " & vbCrLf & strLastError
                Case NGLWebReturnValues.nglDataValidationFailure
                    strMsg &= " due to a data validation failure the actual error message is: " & vbCrLf & strLastError
                Case Else
                    strMsg &= " due to an unexpected failure the actual error message is: " & vbCrLf & strLastError
            End Select
            LogError(Source & " Save EDI Inbound Data Failure", strMsg, Me.AdminEmail)
        Else
            blnRet = True
            Log("The EDI input data for carrier " & CarrierNumber & " using file " & FileName & " has been processed.")
            If Not String.IsNullOrEmpty(str997sOut) AndAlso str997sOut.Trim.Length > 100 Then
                Dim o997out As New clsEDI997Output
                o997out.Send997ResponseData(str997sOut, Me)
            End If
        End If
        Return blnRet


    End Function

End Class
