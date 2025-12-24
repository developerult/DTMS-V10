Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class APFreightBillExportObject
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetData(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompanyNumber As String, _
                            ByVal FreightBillNumber As String, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPFreightBillObject()

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If FreightBillNumber = "" Then FreightBillNumber = Nothing

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPFreightBillObject
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(FreightBillNumber) Then strCriteria &= " Freight Bill Number = " & FreightBillNumber

        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            WSResult = apFreightBillExport.readObjectData(oData, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, FreightBillNumber)
            mstrLastError = apFreightBillExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APFreightBillExportObject", WSResult, apFreightBillExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APFreightBillExportObject.GetData Failure", WSResult, "Cannot get AP FreightBill data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export FreightBill Data Failure")
        End Try
        Return oData

    End Function

    <WebMethod()> _
    Public Function GetDataEX(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompanyNumber As String, _
                            ByVal FreightBillNumber As String, _
                            ByVal MaxRowsReturned As Integer, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPFreightBillObject()

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If FreightBillNumber = "" Then FreightBillNumber = Nothing

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPFreightBillObject
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(FreightBillNumber) Then strCriteria &= " Freight Bill Number = " & FreightBillNumber

        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            apFreightBillExport.MaxRowsReturned = MaxRowsReturned
            apFreightBillExport.AutoConfirmation = AutoConfirmation
            WSResult = apFreightBillExport.readObjectData(oData, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, FreightBillNumber)
            mstrLastError = apFreightBillExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APFreightBillExportObject", WSResult, apFreightBillExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APFreightBillExportObject.GetDataEx Failure", WSResult, "Cannot get FreightBill data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export FreightBill Data Failure")
        End Try
        Return oData

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
            Utilities.LogResults("APFreightBillExportObject", result, apFreightBillExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            Utilities.LogException("APFreightBillExportObject.ConfirmExport Failure", result, "Cannot Confirm FreightBill using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate FreightBill Record is Possible")
        End Try
        Return result

    End Function

    <WebMethod()> _
    Public Function GetOpenPayables(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPFreightBillObject()

        If CompanyNumber = "" Then CompanyNumber = Nothing

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPFreightBillObject
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            WSResult = apFreightBillExport.readObjectOpenPayables(oData, Utilities.GetConnectionString(), CompanyNumber)
            mstrLastError = apFreightBillExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APFreightBillExportObject", WSResult, apFreightBillExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APFreightBillExportObject.GetOpenPayables Failure", WSResult, "Cannot get AP Open Payables using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return oData

    End Function

    <WebMethod()> _
    Public Function GetOpenPayablesEx(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByVal MaxRowsReturned As Integer, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPFreightBillObject()

        If CompanyNumber = "" Then CompanyNumber = Nothing

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.clsAPFreightBillExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPFreightBillObject
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(apFreightBillExport)
            apFreightBillExport.MaxRowsReturned = MaxRowsReturned
            WSResult = apFreightBillExport.readObjectOpenPayables(oData, Utilities.GetConnectionString(), CompanyNumber)
            mstrLastError = apFreightBillExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APFreightBillExportObject", WSResult, apFreightBillExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APFreightBillExport.GetOpenPayablesEx Failure", WSResult, "Cannot get AP Open Payables using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return oData

    End Function
End Class