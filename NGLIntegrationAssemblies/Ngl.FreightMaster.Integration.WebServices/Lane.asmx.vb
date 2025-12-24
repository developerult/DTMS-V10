Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Lane
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.LaneData
        Return New Ngl.FreightMaster.Integration.LaneData
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
        ByVal LaneTable As Ngl.FreightMaster.Integration.LaneData.LaneHeaderDataTable) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, LaneTable, s)

    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
            ByVal LaneTable As Ngl.FreightMaster.Integration.LaneData.LaneHeaderDataTable, _
            ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(lane)
            result = lane.ProcessData(LaneTable, Utilities.GetConnectionString())
            ReturnMessage = lane.LastError
            Dim sLogMsg As String = "Processing " & LaneTable.Rows.Count & " Lane Records"
            If lane.Debug Then
                sLogMsg &= String.Format(", AdminEmail: {0}, MailServer: {1}", lane.AdminEmail, lane.SMTPServer)
            End If
            Utilities.LogResults(sLogMsg, result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("Lane.ProcessDataEx Failure", result, "Cannot process Lane data.  ", ex, AuthorizationCode, "Process Lane Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    <WebMethod()> _
    Public Function ProcessDataWCalendar(ByVal AuthorizationCode As String, _
            ByVal LaneTable As Ngl.FreightMaster.Integration.LaneData.LaneHeaderDataTable, _
            ByVal CalendarTable As Ngl.FreightMaster.Integration.LaneData.LaneCalendarDataTable, _
            ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(lane)
            result = lane.ProcessData(LaneTable, Utilities.GetConnectionString(), CalendarTable)
            ReturnMessage = lane.LastError
            Dim sLogMsg As String = "Processing " & LaneTable.Rows.Count & " Lane.ProcessDataWCalendar Records"
            If lane.Debug Then
                sLogMsg &= String.Format(", AdminEmail: {0}, MailServer: {1}", lane.AdminEmail, lane.SMTPServer)
            End If
            Utilities.LogResults(sLogMsg, result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("Lane.ProcessDataWCalendar Failure", result, "Cannot process Lane data.  ", ex, AuthorizationCode, "Process Book Lane Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class