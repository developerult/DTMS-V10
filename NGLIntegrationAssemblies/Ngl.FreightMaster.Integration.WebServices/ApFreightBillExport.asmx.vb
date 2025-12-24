Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ApFreightBillExport
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.APFreightBill

        Return New Ngl.FreightMaster.Integration.APFreightBill

    End Function

    <WebMethod()> _
    Public Function GetData(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompanyNumber As String, _
                            ByVal FreightBillNumber As String, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If FreightBillNumber = "" Then FreightBillNumber = Nothing
        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim dt As New Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(FreightBillNumber) Then strCriteria &= " FreightBillNumber = " & FreightBillNumber
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            WSResult = apFreightBillExport.readData(dt, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, FreightBillNumber)
            mstrLastError = apFreightBillExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APFreightBillExport", WSResult, apFreightBillExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APFreightBillExport.GetData Failure", WSResult, "Cannot get AP FreightBill data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP FreightBill Data Failure")
        End Try
        Return dt

    End Function

    <WebMethod()> _
    Public Function GetDataEx(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompanyNumber As String, _
                            ByVal FreightBillNumber As String, _
                            ByVal MaxRowsReturned As Integer, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If FreightBillNumber = "" Then FreightBillNumber = Nothing

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim dt As New Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(FreightBillNumber) Then strCriteria &= " FreightBillNumber = " & FreightBillNumber

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            With apFreightBillExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
            WSResult = apFreightBillExport.readData(dt, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, FreightBillNumber)
            mstrLastError = apFreightBillExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APFreightBillExport", WSResult, apFreightBillExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APFreightBillExport.GetDataEx Failure", WSResult, "Cannot get AP FreightBill data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP FreightBill Data Failure")
        End Try
        Return dt

    End Function

    <WebMethod()> _
    Public Function ConfirmExport(ByVal AuthorizationCode As String, ByVal BookFinAPBillNumber As String) As Boolean

        If BookFinAPBillNumber = "" Then BookFinAPBillNumber = Nothing

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(BookFinAPBillNumber) Then strCriteria &= " Freight Bill Number = " & BookFinAPBillNumber
        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            result = apFreightBillExport.confirmExport(Utilities.GetConnectionString(), BookFinAPBillNumber)
            mstrLastError = apFreightBillExport.LastError
            Utilities.LogResults("APFreightBillExport", result, apFreightBillExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            Utilities.LogException("APFreightBillExport.ConfirmExport Failure", result, "Cannot Confirm AP FreightBill Export using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate AP FreightBill Record is Possible")
        End Try
        Return result

    End Function

    <WebMethod()> _
    Public Function GetOpenPayables(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable

        If CompanyNumber = "" Then CompanyNumber = Nothing

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim dt As New Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            WSResult = apFreightBillExport.readOpenPayables(dt, Utilities.GetConnectionString(), CompanyNumber)
            mstrLastError = apFreightBillExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APFreightBillExport", WSResult, apFreightBillExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APFreightBillExport.GetOpenPayables Failure", WSResult, "Cannot get AP Open Payables using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return dt

    End Function

    <WebMethod()> _
    Public Function GetOpenPayablesEx(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByVal MaxRowsReturned As Integer, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable

        If CompanyNumber = "" Then CompanyNumber = Nothing

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim dt As New Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            With apFreightBillExport
                .MaxRowsReturned = MaxRowsReturned
            End With
            WSResult = apFreightBillExport.readOpenPayables(dt, Utilities.GetConnectionString(), CompanyNumber)
            mstrLastError = apFreightBillExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APFreightBillExport", WSResult, apFreightBillExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APFreightBillExport.GetOpenPayablesEx Failure", WSResult, "Cannot get AP Open Payables using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return dt

    End Function

End Class