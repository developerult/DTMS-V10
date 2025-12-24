Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class CarrierObject
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
        ByVal CarrierHeaders() As Ngl.FreightMaster.Integration.clsCarrierHeaderObject,
        ByVal CarrierContacts() As Ngl.FreightMaster.Integration.clsCarrierContactObject) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, CarrierHeaders, CarrierContacts, s)
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessDataEx(ByVal AuthorizationCode As String,
        ByVal CarrierHeaders() As Ngl.FreightMaster.Integration.clsCarrierHeaderObject,
        ByVal CarrierContacts() As Ngl.FreightMaster.Integration.clsCarrierContactObject,
        ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If CarrierHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("CarrierObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim carrier As New Ngl.FreightMaster.Integration.clsCarrier
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(carrier)
            result = carrier.ProcessObjectData(CarrierHeaders, CarrierContacts, Utilities.GetConnectionString())
            ReturnMessage = carrier.LastError
            Utilities.LogResults("CarrierObject", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("CarrierObject", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException("CarrierObject.ProcessDataEx Failure", result, "Cannot process Carrier data.  ", ex, AuthorizationCode, "Process Carrier Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Add logic to save records using clsCarrier
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="CarrierHeaders"></param>
    ''' <param name="CarrierContacts"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData70(ByVal AuthorizationCode As String, _
        ByVal CarrierHeaders() As Ngl.FreightMaster.Integration.clsCarrierHeaderObject70, _
        ByVal CarrierContacts() As Ngl.FreightMaster.Integration.clsCarrierContactObject70, _
        ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Dim oRes As New clsIntegrationUpdateResults
        Try
            If CarrierHeaders Is Nothing OrElse CarrierHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("CarrierObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim lCarrierHeaders As List(Of Ngl.FreightMaster.Integration.clsCarrierHeaderObject70) = CarrierHeaders.ToList()
            Dim lCarrierContacts As New List(Of Ngl.FreightMaster.Integration.clsCarrierContactObject70)
            If Not CarrierContacts Is Nothing AndAlso CarrierContacts.Count() > 0 Then
                lCarrierContacts = CarrierContacts.ToList()
            End If
            Dim carrier As New Ngl.FreightMaster.Integration.clsCarrier
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(carrier)
            oRes = carrier.ProcessObjectData70(lCarrierHeaders, lCarrierContacts, Utilities.GetConnectionString())
            result = oRes.ReturnValue
            ReturnMessage = carrier.LastError
            Utilities.LogResults("CarrierObject.ProcessData70", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("CarrierObject.ProcessData70", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException("CarrierObject.ProcessData70 Failure", result, "Cannot process Carrier data.  ", ex, AuthorizationCode, "Process Carrier Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result
    End Function

End Class