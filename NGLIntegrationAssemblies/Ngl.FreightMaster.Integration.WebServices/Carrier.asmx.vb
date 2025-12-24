Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Carrier
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.CarrierData

        Return New Ngl.FreightMaster.Integration.CarrierData

    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
        ByVal CarrierHeaderTable As Ngl.FreightMaster.Integration.CarrierData.CarrierHeaderDataTable, _
        ByVal CarrierContactTable As Ngl.FreightMaster.Integration.CarrierData.CarrierContactDataTable) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, CarrierHeaderTable, CarrierContactTable, s)
    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
        ByVal CarrierHeaderTable As Ngl.FreightMaster.Integration.CarrierData.CarrierHeaderDataTable, _
        ByVal CarrierContactTable As Ngl.FreightMaster.Integration.CarrierData.CarrierContactDataTable, _
        ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try

            Dim carrier As New Ngl.FreightMaster.Integration.clsCarrier
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(carrier)
            result = carrier.ProcessData(CarrierHeaderTable, CarrierContactTable, Utilities.GetConnectionString())
            ReturnMessage = carrier.LastError
            Utilities.LogResults("Carrier", result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("Carrier.ProcessDataEx Failure", result, "Cannot process Carrier data.  ", ex, AuthorizationCode, "Process Carrier Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try

        End Try
        Return result
    End Function

End Class