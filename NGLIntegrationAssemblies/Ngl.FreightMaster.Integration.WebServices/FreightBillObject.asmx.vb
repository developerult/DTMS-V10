Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class FreightBillObject
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
        Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
    ByVal oFreightBill() As Ngl.FreightMaster.Integration.clsFreightBillObject) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, oFreightBill, s)

    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
                ByVal oFreightBill() As Ngl.FreightMaster.Integration.clsFreightBillObject, _
                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim oImport As New Ngl.FreightMaster.Integration.clsFreightBillImport
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oImport)
            result = oImport.ProcessObjectData(oFreightBill, Utilities.GetConnectionString())
            ReturnMessage = oImport.LastError
            Utilities.LogResults("FreightBillObject", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("FreightBillObject", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException("FreightBillObject.ProcessDataEx Failure", result, "Cannot process FreightBill data.  ", ex, AuthorizationCode, "Process FreightBill Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class