Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml
Imports NGL.FreightMaster.Integration.Configuration

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class CompanyObject
    Inherits System.Web.Services.WebService
    'Note: replace all instances of  ''' <c>ClassLibrary1.TraceExtension()</c> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
    'Should only be run For diagnostics Or In test systems.

    Private mstrLastError As String = ""

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData(ByVal AuthorizationCode As String,
        ByVal CompanyHeaders() As Ngl.FreightMaster.Integration.clsCompanyHeaderObject,
        ByVal CompanyContacts() As Ngl.FreightMaster.Integration.clsCompanyContactObject) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, CompanyHeaders, CompanyContacts, s)

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessDataEx(ByVal AuthorizationCode As String,
        ByVal CompanyHeaders() As Ngl.FreightMaster.Integration.clsCompanyHeaderObject,
        ByVal CompanyContacts() As Ngl.FreightMaster.Integration.clsCompanyContactObject,
        ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 0
        ReturnMessage = ""
        Try
            If CompanyHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("CompanyObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim company As New Ngl.FreightMaster.Integration.clsCompany
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(company)
            result = company.ProcessObjectData(CompanyHeaders, CompanyContacts, Utilities.GetConnectionString())
            ReturnMessage = company.LastError
            Utilities.LogResults("CompanyObject", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("CompanyObject.ProcessDataEx Failure", result, "Cannot process Company data.  ", ex, AuthorizationCode, "Process Company Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try

        End Try
        Return result

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessDataWCalendar(ByVal AuthorizationCode As String,
        ByVal CompanyHeaders() As Ngl.FreightMaster.Integration.clsCompanyHeaderObject,
        ByVal CompanyContacts() As Ngl.FreightMaster.Integration.clsCompanyContactObject,
        ByVal CompanyCalendar() As Ngl.FreightMaster.Integration.clsCompanyCalendarObject,
        ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 0
        ReturnMessage = ""
        Try
            If CompanyHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("CompanyObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim company As New Ngl.FreightMaster.Integration.clsCompany
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(company)
            result = company.ProcessObjectData(CompanyHeaders, CompanyContacts, Utilities.GetConnectionString(), CompanyCalendar)
            ReturnMessage = company.LastError
            Utilities.LogResults("CompanyObject.ProcessDataWCalendar", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("CompanyObject.ProcessDataWCalendar Failure", result, "Cannot process Company data.  ", ex, AuthorizationCode, "Process Company Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try

        End Try
        Return result

    End Function

    ''' <summary>
    ''' Add logic to save data using clsCompany
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="CompanyHeaders"></param>
    ''' <param name="CompanyContacts"></param>
    ''' <param name="CompanyCalendar"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData70(ByVal AuthorizationCode As String, _
        ByVal CompanyHeaders() As Ngl.FreightMaster.Integration.clsCompanyHeaderObject70, _
        ByVal CompanyContacts() As Ngl.FreightMaster.Integration.clsCompanyContactObject70, _
        ByVal CompanyCalendar() As Ngl.FreightMaster.Integration.clsCompanyCalendarObject70, _
        ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 0
        Dim oRes As New clsIntegrationUpdateResults
        ReturnMessage = ""
        Try
            If CompanyHeaders Is Nothing OrElse CompanyHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("CompanyObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim lCompHeaders As List(Of Ngl.FreightMaster.Integration.clsCompanyHeaderObject70) = CompanyHeaders.ToList()
            Dim lCompContacts As New List(Of Ngl.FreightMaster.Integration.clsCompanyContactObject70)
            If Not CompanyContacts Is Nothing AndAlso CompanyContacts.Count() > 0 Then
                lCompContacts = CompanyContacts.ToList()
            End If

            Dim lCompCals As New List(Of Ngl.FreightMaster.Integration.clsCompanyCalendarObject70)
            If Not CompanyCalendar Is Nothing AndAlso CompanyCalendar.Count() > 0 Then
                lCompCals = CompanyCalendar.ToList()
            End If

            Dim company As New Ngl.FreightMaster.Integration.clsCompany
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(company)
            oRes = company.ProcessObjectData70(lCompHeaders, lCompContacts, Utilities.GetConnectionString(), lCompCals)
            result = oRes.ReturnValue
            ReturnMessage = company.LastError
            Utilities.LogResults("CompanyObject.ProcessData70", result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            result = ProcessDataReturnValues.nglDataIntegrationFailure
            ReturnMessage = ex.Message
            Utilities.LogException("CompanyObject.ProcessData70 Failure", result, "Cannot process Company data.  ", ex, AuthorizationCode, "Process Company Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try

        End Try
        Return result

    End Function

End Class