Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml
Imports Ngl.FreightMaster.Integration
Imports System.Xml.Serialization

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class LaneObject
    Inherits System.Web.Services.WebService
    'Note: replace all instances of  ''' <c>ClassLibrary1.TraceExtension()</c> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
    'Should only be run For diagnostics Or In test systems.

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData(ByVal AuthorizationCode As String,
        ByVal Lanes() As Ngl.FreightMaster.Integration.clsLaneObject) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, Lanes, s)

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessDataEx(ByVal AuthorizationCode As String,
            ByVal Lanes() As Ngl.FreightMaster.Integration.clsLaneObject,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If Lanes.Length = 0 Then
                mstrLastError = "No Lanes"
                Utilities.LogResults("LaneObject", 0, mstrLastError, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(lane)
            result = lane.ProcessObjectData(Lanes, Utilities.GetConnectionString())
            ReturnMessage = lane.LastError
            Utilities.LogResults("LaneObject", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("LaneObject.ProcessDataEx Failure", result, "Cannot process Lane data.  ", ex, AuthorizationCode, "Process Lane Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessDataWCalendar(ByVal AuthorizationCode As String,
            ByVal Lanes() As Ngl.FreightMaster.Integration.clsLaneObject,
            ByVal Calendar() As Ngl.FreightMaster.Integration.clsLaneCalendarObject,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If Lanes.Length = 0 Then
                mstrLastError = "No Lanes"
                Utilities.LogResults("LaneObject.ProcessDataWCalendar", 0, mstrLastError, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(lane)
            result = lane.ProcessObjectData(Lanes, Utilities.GetConnectionString(), Calendar)
            ReturnMessage = lane.LastError
            Utilities.LogResults("LaneObject.ProcessDataWCalendar", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("LaneObject.ProcessDataWCalendar Failure", result, "Cannot process Lane data.  ", ex, AuthorizationCode, "Process Lane Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData60(ByVal AuthorizationCode As String,
            ByVal Lanes() As Ngl.FreightMaster.Integration.clsLaneObject60,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If Lanes.Length = 0 Then
                mstrLastError = "No Lanes"
                Utilities.LogResults("LaneObject.ProcessData60", 0, mstrLastError, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(lane)
            result = lane.ProcessObjectData(Lanes.ToList, Utilities.GetConnectionString())
            ReturnMessage = lane.LastError
            Utilities.LogResults("LaneObject.ProcessData60", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("LaneObject.ProcessData60 Failure", result, "Cannot process Lane data.  ", ex, AuthorizationCode, "Process Lane Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData60WCalendar(ByVal AuthorizationCode As String,
            ByVal Lanes() As Ngl.FreightMaster.Integration.clsLaneObject60,
            ByVal Calendar() As Ngl.FreightMaster.Integration.clsLaneCalendarObject60,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If Lanes.Length = 0 Then
                mstrLastError = "No Lanes"
                Utilities.LogResults("LaneObject.ProcessData60WCalendar", 0, mstrLastError, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(lane)
            result = lane.ProcessObjectData(Lanes.ToList, Utilities.GetConnectionString(), Calendar.ToList)
            ReturnMessage = lane.LastError
            Utilities.LogResults("LaneObject.ProcessData60WCalendar", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("LaneObject.ProcessData60WCalendar Failure", result, "Cannot process Lane data.  ", ex, AuthorizationCode, "Process Lane Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    ''' <summary>
    ''' Add logic to save data using clsLane
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="Lanes"></param>
    ''' <param name="Calendar"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData70(ByVal AuthorizationCode As String, _
            ByVal Lanes() As Ngl.FreightMaster.Integration.clsLaneObject70, _
            ByVal Calendar() As Ngl.FreightMaster.Integration.clsLaneCalendarObject70, _
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Dim retVal As New clsIntegrationUpdateResults
        Try
            If Lanes Is Nothing OrElse Lanes.Length = 0 Then
                mstrLastError = "No Lanes"
                Utilities.LogResults("LaneObject.ProcessData70", 0, mstrLastError, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(lane)

            Dim lLanes As List(Of Ngl.FreightMaster.Integration.clsLaneObject70) = Lanes.ToList()
            Dim lLaneCals As New List(Of Ngl.FreightMaster.Integration.clsLaneCalendarObject70)
            If Not Calendar Is Nothing AndAlso Calendar.Count() > 0 Then
                lLaneCals = Calendar.ToList()
            End If
            retVal = lane.ProcessObjectData70(lLanes, Utilities.GetConnectionString(), lLaneCals)
            result = retVal.ReturnValue
            ReturnMessage = lane.LastError
            Utilities.LogResults("LaneObject.ProcessData70", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("LaneObject.ProcessData70 Failure", result, "Cannot process Lane data.  ", ex, AuthorizationCode, "Process Lane Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class