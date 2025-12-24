Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Schedule
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.ScheduleData.ScheduleHeaderDataTable
        Return New Ngl.FreightMaster.Integration.ScheduleData.ScheduleHeaderDataTable
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
    ByVal ScheduleTable As Ngl.FreightMaster.Integration.ScheduleData.ScheduleHeaderDataTable) As Integer
        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, ScheduleTable, s)
    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
            ByVal ScheduleTable As Ngl.FreightMaster.Integration.ScheduleData.ScheduleHeaderDataTable, _
            ByRef ReturnMessage As String) As Integer

        Dim result As Integer
        ReturnMessage = ""
        Try
            Dim schedule As New Ngl.FreightMaster.Integration.clsSchedule
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(schedule)
            result = schedule.ProcessData(ScheduleTable, Utilities.GetConnectionString())
            ReturnMessage = schedule.LastError
            Utilities.LogResults("Schedule", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("Schedule", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException("Schedule.ProcessDataEx Failure", result, "Cannot process Schedule data.  ", ex, AuthorizationCode, "Process Schedule Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function
End Class