Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports NGL.Core.Utility
Imports NGL.Core.Communication.General
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration
    Public Shared sErrMsg As String = "Unknown Error!"

    Dim bookbll As BLL.NGLBookBLL
    Dim params As NGL.FreightMaster.Data.WCFParameters

    Public Sub ProcessData()

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
        params = New NGL.FreightMaster.Data.WCFParameters
        With params
            .UserName = "nglweb"
            .Database = Me.oConfig.Database
            .DBServer = Me.oConfig.DBServer
            .WCFAuthCode = "NGLSystem"
            .ConnectionString = Me.oConfig.ConnectionString ' "Data Source=" & .DBServer & ";Initial Catalog=" & .Database & ";Integrated Security=True"
        End With


        bookbll = New BLL.NGLBookBLL(params)

        Try
            Log("Begin Process Data ")
            Me.ProcessExpirations()
            Log("Process Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected ProcessExpirations", "An unexpected error has occurred while attempting to ProcessExpirations.  The actual error is: " & ex.Message & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
        Finally

        End Try

    End Sub

    Public Sub fillConfig()
        Try
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .ConnectionString = ConnectionString
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

    ''Public Function ToSql(ByVal text As String, Optional ByVal IncludeTrailingComma As Boolean = True, Optional ByVal AllowNull As Boolean = True) As String

    ''    If text = String.Empty And AllowNull Then
    ''        text = "NULL"
    ''    Else
    ''        text = "'" & text.Replace("'", "''").Replace(";", "") & "'"
    ''    End If

    ''    If IncludeTrailingComma Then text &= ","
    ''    Return text
    ''End Function

    Public Function getWebDefaults(ByVal CompControl As Integer) As Tuple(Of Integer, String, String)
        Dim sqlStatement As String = "Select CarrierLoadAcceptanceAllowedMinutes, ExpiredLoadsTo, ExpiredLoadsCc From dbo.CompanyConfiguration Where CompanyId = " & CompControl
        Dim cn As New SqlConnection(OpenConnectionWeb())
        ' Dim cn As New SqlConnection(OpenConnection())
        Dim cm As New SqlCommand(sqlStatement, cn)
        Dim da As New SqlDataAdapter(cm)
        Dim dt As New DataTable
        Try
            cn.Open()
            da.Fill(dt)
            If dt.Rows.Count < 1 Then
                'get the data for company zero (defaults)
                sqlStatement = "Select CarrierLoadAcceptanceAllowedMinutes, ExpiredLoadsTo, ExpiredLoadsCc From dbo.CompanyConfiguration Where CompanyId = 0"
                cm.CommandText = sqlStatement
                da = New SqlDataAdapter(cm)
                dt = New DataTable
                da.Fill(dt)
            End If
        Catch ex As Exception
            sErrMsg = formatErrMsg(ex, Source)
            LogError("Load Expired Failure", "Load Expired Calling Stored Proc " & vbCrLf & sErrMsg & vbCrLf & vbCrLf & Me.NEXTRackDatabase, Me.NEXTRackDatabaseServer)
            EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
            Return New Tuple(Of Integer, String, String)(0, "", "")
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        If dt.Rows.Count < 1 Then
            Return New Tuple(Of Integer, String, String)(Me.GlobalDefaultLoadAcceptAllowedMinutes, "", "")
        Else
            Return New Tuple(Of Integer, String, String)(DataTransformation.FixNullInteger(dt.Rows(0).Item("CarrierLoadAcceptanceAllowedMinutes")), nz(dt.Rows(0).Item("ExpiredLoadsTo"), ""), nz(dt.Rows(0).Item("ExpiredLoadsCc"), ""))
        End If
    End Function

    Public Shared Function nz(ByVal source As Object, ByVal defaultvalue As Object) As Object


        If source Is Nothing Or source Is DBNull.Value Then
            Return defaultvalue
        ElseIf Len(Trim(source & " ")) < 1 Then
            Return defaultvalue
        Else
            Return source
        End If

    End Function

    ''' <summary>
    ''' New method to use new processExpiredLoads method in BLL.7/7/2014
    ''' changes - Select all active companies in FreightMaster.
    ''' Get the WebDefaults for each active company if exists.
    ''' Call processExpiredLoads in the BLL
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ProcessExpirations()
        Try
            Dim comps() As DTO.vLookupList = bookbll.NGLLookupData.GetViewLookupList("CompActive", 2)
            If comps IsNot Nothing AndAlso comps.Length > 0 Then
                Dim webDefaults As New Dictionary(Of Integer, Tuple(Of Integer, String, String))
                For Each item As DTO.vLookupList In comps
                    webDefaults.Add(item.Control, getWebDefaults(item.Control))
                Next
                Dim result As DTO.WCFResults = bookbll.ProcessExpiredLoads(webDefaults, True, True)
                If result IsNot Nothing AndAlso result.Success = False Then
                    ''TODO email messages from result get actrion result.
                    ''failed. lets send an  email.
                    'result.Actio
                    sErrMsg = result.getErrorsAsSingleStr(vbCrLf, False)
                    'sErrMsg = result.getMessageEnumFromString(vbCrLf, False)
                    LogError("Expired Loads Failure", String.Format("Unable to process expired loads.  The actual error message is: {0}", sErrMsg & vbCrLf & Me.DBInfo), Me.AdminEmail)
                    EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
                End If
            End If
        Catch ex As Exception
            sErrMsg = formatErrMsg(ex, Source)
            LogError("Load Expired Failure", "Unable to process expired loads.  The actual error message is:" & vbCrLf & vbCrLf & sErrMsg & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
        End Try
        'getWebDefaults(DataTransformation.FixNullInteger(row.Item("BookCustCompControl")))
    End Sub


    Private _EventLogger As EventLog
    Public Property EvtLog() As EventLog
        Get
            Return _EventLogger
        End Get
        Set(ByVal value As EventLog)
            _EventLogger = value
        End Set
    End Property

    Public Function OpenConnectionWeb() As String
        Return "Data Source=" & nz(Me.NEXTRackDatabaseServer, "NGLSQL01T") & ";" & "Initial Catalog=" & nz(Me.NEXTRackDatabase, "NGLWebDev") & ";" & "Integrated Security=SSPI"
    End Function
End Class
