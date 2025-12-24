Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Ngl.FreightMaster.Integration
Imports System.Xml.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://nextgeneration.com/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class PalletTypeObject
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
                                ByVal PalletTypes() As Ngl.FreightMaster.Integration.clsPalletTypeObject, _
                                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If PalletTypes Is Nothing OrElse PalletTypes.Length = 0 Then
                ReturnMessage = "No Data"
                Utilities.LogResults("PalletTypeObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim dataObject As New Ngl.FreightMaster.Integration.clsPalletType
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(dataObject)
            With dataObject
                result = .ProcessObjectData(PalletTypes.ToList, Utilities.GetConnectionString())
                ReturnMessage = .LastError
                Utilities.LogResults("PalletTypeObject", result, ReturnMessage, AuthorizationCode)
            End With

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("PalletTypeObject.ProcessData Failure", result, "Cannot process pallet type data.  ", ex, AuthorizationCode, "Process Pallet Type Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class