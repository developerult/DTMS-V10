Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Payables
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
        Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.PayablesData
        Return New Ngl.FreightMaster.Integration.PayablesData
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
        ByVal PayablesTable As Ngl.FreightMaster.Integration.PayablesData.PayablesHeaderDataTable) As Integer
        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, PayablesTable, s)
    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
            ByVal PayablesTable As Ngl.FreightMaster.Integration.PayablesData.PayablesHeaderDataTable, _
            ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim payables As New Ngl.FreightMaster.Integration.clsPayables
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(payables)
            result = payables.ProcessData(PayablesTable, Utilities.GetConnectionString())
            ReturnMessage = payables.LastError
            Utilities.LogResults("Payables", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("Payables.ProcessDataEx Failure", result, "Cannot process Payables data.  ", ex, AuthorizationCode, "Process Payables Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function


End Class