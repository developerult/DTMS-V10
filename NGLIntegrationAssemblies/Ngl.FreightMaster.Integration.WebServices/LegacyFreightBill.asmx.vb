Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class LegacyLegacyFreightBill
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
        Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.LegacyFreightBillData
        Return New Ngl.FreightMaster.Integration.LegacyFreightBillData
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
        ByVal oHTable As Ngl.FreightMaster.Integration.LegacyFreightBillData.LegacyFreightBillDataTable) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, oHTable, s)

    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
            ByVal oHTable As Ngl.FreightMaster.Integration.LegacyFreightBillData.LegacyFreightBillDataTable, _
            ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim oImport As New Ngl.FreightMaster.Integration.clsLegacyFreightBillImport
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oImport)
            result = oImport.ProcessData(oHTable, Utilities.GetConnectionString())
            ReturnMessage = oImport.LastError
            Utilities.LogResults("LegacyFreightBill", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            Utilities.LogException("LegacyFreightBill.ProcessDataEx Failure", result, "Cannot process Legacy FreightBill data.  ", ex, AuthorizationCode, "Process Legacy FreightBill Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class