Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ScheduleObject
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
    ByVal Schedules() As Ngl.FreightMaster.Integration.clsScheduleObject) As Integer
        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, Schedules, s)
    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
            ByVal Schedules() As Ngl.FreightMaster.Integration.clsScheduleObject, _
            ByRef ReturnMessage As String) As Integer

        Dim result As Integer
        ReturnMessage = ""
        Try
            Dim schedule As New Ngl.FreightMaster.Integration.clsSchedule
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(schedule)
            result = schedule.ProcessObjectData(Schedules, Utilities.GetConnectionString())
            ReturnMessage = schedule.LastError
            Utilities.LogResults("ScheduleObject", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("ScheduleObject.ProcessDataEx Failure", result, "Cannot process Schedule Object data.  ", ex, AuthorizationCode, "Process Schedule Object Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    <WebMethod()> _
    Public Function ProcessData70(ByVal AuthorizationCode As String, _
                                  ByVal Schedule() As Ngl.FreightMaster.Integration.clsScheduleObject70, _
                                  ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If Schedule Is Nothing OrElse Schedule.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("ScheduleObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim oSchedule As New Ngl.FreightMaster.Integration.clsSchedule
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oSchedule)
            result = oSchedule.ProcessObjectData70(Schedule, Utilities.GetConnectionString())
            ReturnMessage = oSchedule.LastError
            Utilities.LogResults("ScheduleObject.ProcessData70", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("ScheduleObject.ProcessData70 Failure", result, "Cannot process Schedule Object data.  ", ex, AuthorizationCode, "Process Schedule Object Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class