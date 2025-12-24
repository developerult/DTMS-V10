Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class PayablesObject
    Inherits System.Web.Services.WebService
    'Note: replace all instances of  ''' <c>ClassLibrary1.TraceExtension()</c> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
    'Should only be run For diagnostics Or In test systems.

    Private mstrLastError As String = ""
    <WebMethod()>
    Public Function LastError() As String
        Return mstrLastError
    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData(ByVal AuthorizationCode As String,
        ByVal Payables() As Ngl.FreightMaster.Integration.clsPayablesObject) As Integer
        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, Payables, s)
    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessDataEx(ByVal AuthorizationCode As String,
        ByVal Payables() As Ngl.FreightMaster.Integration.clsPayablesObject,
        ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim oPayables As New Ngl.FreightMaster.Integration.clsPayables
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oPayables)
            result = oPayables.ProcessObjectData(Payables, Utilities.GetConnectionString())
            ReturnMessage = oPayables.LastError
            Utilities.LogResults("PayablesObject", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("PayablesObject.ProcessDataEx Failure", result, "Cannot process Payables Object data.  ", ex, AuthorizationCode, "Process Payables Object Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    ''' <summary>
    ''' Add logic to save data using clsPayables
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="Payables"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()> _
    Public Function ProcessData70(ByVal AuthorizationCode As String, _
        ByVal Payables() As Ngl.FreightMaster.Integration.clsPayablesObject70, _
        ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If Payables Is Nothing OrElse Payables.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("PayablesObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim oPayables As New Ngl.FreightMaster.Integration.clsPayables
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oPayables)
            result = oPayables.ProcessObjectData70(Payables.ToList(), Utilities.GetConnectionString())
            ReturnMessage = oPayables.LastError
            Utilities.LogResults("PayablesObject.ProcessData70", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("PayablesObject.ProcessData70 Failure", result, "Cannot process Payables Object data.  ", ex, AuthorizationCode, "Process Payables Object Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class