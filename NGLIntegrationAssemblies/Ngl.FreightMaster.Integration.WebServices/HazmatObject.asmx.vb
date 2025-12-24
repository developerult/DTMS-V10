Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Ngl.FreightMaster.Integration
Imports System.Xml.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://nextgeneration.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class HazmatObject
    Inherits System.Web.Services.WebService


    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
                                ByVal Hazmats() As Ngl.FreightMaster.Integration.clsHazmatObject, _
                                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If Hazmats Is Nothing OrElse Hazmats.Length = 0 Then
                ReturnMessage = "No Data"
                Utilities.LogResults("HazmatObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim dataObject As New Ngl.FreightMaster.Integration.clsHazmat
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(dataObject)
            With dataObject
                result = .ProcessData(Hazmats.ToList, Utilities.GetConnectionString())
                ReturnMessage = .LastError
                Utilities.LogResults("HazmatObject", result, ReturnMessage, AuthorizationCode)
            End With

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("HazmatObject.ProcessData Failure", result, "Cannot process hazmat data.  ", ex, AuthorizationCode, "Process Hazmat Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class