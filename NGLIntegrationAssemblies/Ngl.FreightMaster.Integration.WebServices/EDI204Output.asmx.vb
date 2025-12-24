Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://nextgeneration.com/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class EDI204Output
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
        Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function About() As String
        Return "<h1>Nextgeneration Logistics EDI 204 Output Web Services</h1><p>Access Restricted Authorization Required</p>"
    End Function

    <WebMethod()> _
   Public Function getTruckLoadEDI204String(ByVal AuthorizationCode As String, _
                            ByVal CarrierNumber As String, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As String

        Dim strEDI204 As String = ""
        If CarrierNumber = "" Then CarrierNumber = Nothing
        Try
            Dim oEDI204 As New Ngl.FreightMaster.Integration.clsEDI204
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ""
            Utilities.populateIntegrationObjectParameters(oEDI204)
            With oEDI204
                .AutoConfirmation = AutoConfirmation
                WSResult = .getTruckLoadEDI204String(strEDI204, Utilities.GetConnectionString(), CarrierNumber, AutoConfirmation)
                mstrLastError = .LastError
            End With

            LastError = mstrLastError
            Utilities.LogResults("getTruckLoadEDI204String", WSResult, mstrLastError, AuthorizationCode)
        Catch ex As Exception
            LastError = ex.Message
            Utilities.LogException("EDI204Output.getTruckLoadEDI204String Failure", 0, "Cannot get EDI 204 truck load data.  ", ex, AuthorizationCode, "Process EDI 204 truck load Data Failure")
        End Try

        Return strEDI204

    End Function



End Class