Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://nextgeneration.com/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class EDIInput
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
        Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function About() As String
        Return "<h1>Nextgeneration Logistics EDI Input Web Services</h1><p>Access Restricted Authorization Required</p>"
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
                                ByVal EDIString As String, _
                                ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim oEDIInput As New Ngl.FreightMaster.Integration.clsEDIInput
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oEDIInput)
            Dim connectionString As String = Utilities.GetConnectionString()
            With oEDIInput
                result = .ProcessData(EDIString, Utilities.GetConnectionString())
                ReturnMessage = .LastError
            End With
            Utilities.LogResults("EDI Input", result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("EDI Input", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException("EDIInput.ProcessData Failure", result, "Cannot process EDI input data.  ", ex, AuthorizationCode, "Process EDI input Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
    End Function

End Class