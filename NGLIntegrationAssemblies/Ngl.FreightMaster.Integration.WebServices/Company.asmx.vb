Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Company
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.CompanyData

        Return New Ngl.FreightMaster.Integration.CompanyData

    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
        ByVal CompanyHeaderTable As Ngl.FreightMaster.Integration.CompanyData.CompanyHeaderDataTable, _
        ByVal CompanyContactsTable As Ngl.FreightMaster.Integration.CompanyData.CompanyContactDataTable) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, CompanyHeaderTable, CompanyContactsTable, s)

    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
        ByVal CompanyHeaderTable As Ngl.FreightMaster.Integration.CompanyData.CompanyHeaderDataTable, _
        ByVal CompanyContactsTable As Ngl.FreightMaster.Integration.CompanyData.CompanyContactDataTable, _
        ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim company As New Ngl.FreightMaster.Integration.clsCompany
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(company)
            result = company.ProcessData(CompanyHeaderTable, CompanyContactsTable, Utilities.GetConnectionString())
            ReturnMessage = company.LastError
            Utilities.LogResults("Company.ProcessDataEx", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("Company.ProcessDataEx Failure", result, "Cannot process Company data.  ", ex, AuthorizationCode, "Process Company Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    <WebMethod()> _
    Public Function ProcessDataWCalendar(ByVal AuthorizationCode As String, _
                            ByVal CompanyHeaderTable As Ngl.FreightMaster.Integration.CompanyData.CompanyHeaderDataTable, _
                            ByVal CompanyContactsTable As Ngl.FreightMaster.Integration.CompanyData.CompanyContactDataTable, _
                            ByVal CompanyCalendarTable As Ngl.FreightMaster.Integration.CompanyData.CompanyCalendarDataTable, _
                            ByRef LastError As String) As Integer

        Dim result As Integer = 3
        LastError = ""
        Try
            Dim company As New Ngl.FreightMaster.Integration.clsCompany
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(company)
            result = company.ProcessData(CompanyHeaderTable, CompanyContactsTable, Utilities.GetConnectionString(), CompanyCalendarTable)
            LastError = company.LastError
            Utilities.LogResults("Company.ProcessDataWCalendar", result, LastError, AuthorizationCode)
        Catch ex As Exception
            LastError = ex.Message
            Utilities.LogException("Company.ProcessDataWCalendar Failure", result, "Cannot process Company data.  ", ex, AuthorizationCode, "Process Company Data Failure")
        Finally
            Try
                mstrLastError = LastError
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

End Class