Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class FreightBill
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.FreightBillData
        Return New Ngl.FreightMaster.Integration.FreightBillData
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
    ByVal oHTable As Ngl.FreightMaster.Integration.FreightBillData.FreightBillDataTable) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, oHTable, s)

    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
                ByVal oHTable As Ngl.FreightMaster.Integration.FreightBillData.FreightBillDataTable, _
                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim oImport As New Ngl.FreightMaster.Integration.clsFreightBillImport
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oImport)
            result = oImport.ProcessData(oHTable, Utilities.GetConnectionString())
            ReturnMessage = oImport.LastError
            Utilities.LogResults("FreightBill", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("FreightBill.ProcessDataEx Failure", result, "Cannot process FreightBill data.  ", ex, AuthorizationCode, "Process FreightBill Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function



End Class