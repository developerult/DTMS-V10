Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ApExportData
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataset() As Ngl.FreightMaster.Integration.APExport

        Return New Ngl.FreightMaster.Integration.APExport

    End Function

    <WebMethod()> _
    Public Function GetOpenPayablesEx(ByVal AuthorizationCode As String, _
                            ByVal CompanyNumber As String, _
                            ByVal MaxRowsReturned As Integer, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APExport.APExportDataDataTable


        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim dt As New Ngl.FreightMaster.Integration.APExport.APExportDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
            End With
            WSResult = apExport.readOpenPayables(dt, Utilities.GetConnectionString(), CompanyNumber)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExport.GetOpenPayablesEx", WSResult, apExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetOpenPayablesEx Failure", WSResult, "Cannot get AP Open Payables using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return dt

    End Function

    <WebMethod()> _
    Public Function GetOpenPayables(ByVal AuthorizationCode As String, _
                            ByVal CompanyNumber As String, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APExport.APExportDataDataTable

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim dt As New Ngl.FreightMaster.Integration.APExport.APExportDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apExport)
            WSResult = apExport.readOpenPayables(dt, Utilities.GetConnectionString(), CompanyNumber)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportData.GetOpenPayables", WSResult, apExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetOpenPayables Failure", WSResult, "Cannot get AP Open Payables using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return dt

    End Function

    <WebMethod()> _
    Public Function GetDataWDetails(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompanyNumber As String, _
                            ByVal OrderNumber As String, _
                            ByVal OrderSequence As Integer, _
                            ByVal BookProNumber As String, _
                            ByVal MaxRowsReturned As Integer, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APExport

        'Dim blnKeyFound As Boolean = False
        ''check first key Order and company
        'If Not (String.IsNullOrEmpty(OrderNumber) _
        '    OrElse String.IsNullOrEmpty(CompanyNumber)) Then
        '    blnKeyFound = True
        'End If
        ''next key field is the pro number
        'If Not blnKeyFound AndAlso Not String.IsNullOrEmpty(BookProNumber) Then blnKeyFound = True

        'If Not blnKeyFound Then
        '    LastError = "None of the key fields were provided please provide a BookProNumber or both an OrderNumber and a CompanyNumber when executing the APExportData.GetDateWDetails Web Method."
        '    WSResult = 2
        '    Return Nothing
        'End If

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim ds As New Ngl.FreightMaster.Integration.APExport
        Dim dt As New Ngl.FreightMaster.Integration.APExport.APExportDataDataTable
        Dim dtDetails As New Ngl.FreightMaster.Integration.APExport.APItemDetailsDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber
        strCriteria &= " OrderSequence = " & OrderSequence.ToString
        If Not String.IsNullOrEmpty(BookProNumber) Then strCriteria &= " BookProNumber = " & BookProNumber

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ds
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
            WSResult = apExport.readData(dt, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, OrderSequence, BookProNumber, dtDetails)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExport.GetDateWDetails", WSResult, mstrLastError, AuthorizationCode)
            dt.AcceptChanges()
            dtDetails.AcceptChanges()
            ds.Tables.Add(dt)
            ds.Tables.Add(dtDetails)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetDateWDetails Failure", WSResult, "Cannot get AP Export data with details using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return ds

    End Function

    <WebMethod()> _
    Public Function GetALLDataWDetails(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal MaxRowsReturned As Integer, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APExport

       
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim ds As New Ngl.FreightMaster.Integration.APExport
        Dim dt As New Ngl.FreightMaster.Integration.APExport.APExportDataDataTable
        Dim dtDetails As New Ngl.FreightMaster.Integration.APExport.APItemDetailsDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ds
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
            WSResult = apExport.readData(dt, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, Nothing, Nothing, Nothing, Nothing, dtDetails)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExport.GetALLDataWDetails", WSResult, mstrLastError, AuthorizationCode)
            dt.AcceptChanges()
            dtDetails.AcceptChanges()
            ds.Tables.Add(dt)
            ds.Tables.Add(dtDetails)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetALLDataWDetails Failure", WSResult, "Cannot get AP Export data with details using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return ds

    End Function

    <WebMethod()> _
    Public Function GetDataEx(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompanyNumber As String, _
                            ByVal OrderNumber As String, _
                            ByVal MaxRowsReturned As Integer, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APExport.APExportDataDataTable

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If OrderNumber = "" Then OrderNumber = Nothing


        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim dt As New Ngl.FreightMaster.Integration.APExport.APExportDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
            WSResult = apExport.readData(dt, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportData.GetDataEx", WSResult, apExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetDataEx Failure", WSResult, "Cannot get AP Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return dt

    End Function

    <WebMethod()> _
    Public Function GetDataByOrderSequence(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompanyNumber As String, _
                            ByVal OrderNumber As String, _
                            ByVal OrderSequence As String, _
                            ByVal MaxRowsReturned As Integer, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APExport.APExportDataDataTable

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If OrderNumber = "" Then OrderNumber = Nothing
        If OrderSequence = "" Then OrderSequence = Nothing

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim dt As New Ngl.FreightMaster.Integration.APExport.APExportDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber
        If Not String.IsNullOrEmpty(OrderSequence) Then strCriteria &= " OrderSequence = " & OrderSequence

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
            WSResult = apExport.readData(dt, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, OrderSequence)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportData.GetDataEx", WSResult, apExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetDataEx Failure", WSResult, "Cannot get AP Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return dt

    End Function

    <WebMethod()> _
    Public Function GetDataByProNumber(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal BookProNumber As String, _
                            ByVal MaxRowsReturned As Integer, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APExport.APExportDataDataTable

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim dt As New Ngl.FreightMaster.Integration.APExport.APExportDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(BookProNumber) Then strCriteria &= " BookProNumber = " & BookProNumber

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
            WSResult = apExport.readData(dt, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, Nothing, Nothing, Nothing, BookProNumber)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportData.GetDataEx", WSResult, apExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetDataEx Failure", WSResult, "Cannot get AP Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return dt

    End Function


    <WebMethod()> _
    Public Function GetData(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompanyNumber As String, _
                            ByVal OrderNumber As String, _
                            ByRef WSResult As Integer, _
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APExport.APExportDataDataTable

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim dt As New Ngl.FreightMaster.Integration.APExport.APExportDataDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return dt
            Utilities.populateIntegrationObjectParameters(apExport)
            WSResult = apExport.readData(dt, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportData.GetData", WSResult, apExport.LastError, AuthorizationCode)
            dt.AcceptChanges()
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetData Failure", WSResult, "Cannot get AP Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return dt

    End Function

    <WebMethod()> _
    Public Function ConfirmExport(ByVal AuthorizationCode As String, _
                                  ByVal BookCarrOrderNumber As String, _
                                  ByVal BookFinAPBillNumber As String, _
                                  ByVal BookProNumber As String) As Boolean

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(BookCarrOrderNumber) Then strCriteria &= " Order Number = " & BookCarrOrderNumber
        If Not String.IsNullOrEmpty(BookFinAPBillNumber) Then strCriteria &= " Freight Bill Number = " & BookFinAPBillNumber
        If Not String.IsNullOrEmpty(BookProNumber) Then strCriteria &= " Pro Number = " & BookProNumber
        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(apExport)
            result = apExport.confirmExport(Utilities.GetConnectionString(), BookCarrOrderNumber, BookFinAPBillNumber, BookProNumber)
            mstrLastError = apExport.LastError
            Utilities.LogResults("APExportData.ConfirmExport", result, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            Utilities.LogException("APExportData.ConfirmExport Failure", result, "Cannot Confirm AP Export using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function


    <WebMethod()> _
    Public Function ConfirmExportByOrderSequence(ByVal AuthorizationCode As String, _
                                  ByVal BookCarrOrderNumber As String, _
                                  ByVal BookOrderSequence As String, _
                                  ByVal CompNumber As String, _
                                  ByRef LastError As String) As Boolean

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(BookCarrOrderNumber) Then strCriteria &= " Order Number = " & BookCarrOrderNumber
        If Not String.IsNullOrEmpty(BookOrderSequence) Then strCriteria &= " Order Sequence = " & BookOrderSequence
        If Not String.IsNullOrEmpty(CompNumber) Then strCriteria &= " Company Number = " & CompNumber

        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(apExport)
            result = apExport.confirmExport(strConnection:=Utilities.GetConnectionString(), BookCarrOrderNumber:=BookCarrOrderNumber, BookOrderSequence:=BookOrderSequence, CompanyNumber:=CompNumber)
            mstrLastError = apExport.LastError
            Utilities.LogResults("APExportData.ConfirmExportEx", result, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.ConfirmExportEx Failure", result, "Cannot Confirm AP Export using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function

    <WebMethod()> _
    Public Function ConfirmExportByProNumber(ByVal AuthorizationCode As String, _
                                  ByVal BookProNumber As String, _
                                  ByRef LastError As String) As Boolean

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(BookProNumber) Then strCriteria = " Pro Number = " & BookProNumber
        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(apExport)
            result = apExport.confirmExport(strConnection:=Utilities.GetConnectionString(), BookProNumber:=BookProNumber)
            mstrLastError = apExport.LastError
            Utilities.LogResults("APExportData.ConfirmExportEx", result, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.ConfirmExportEx Failure", result, "Cannot Confirm AP Export using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function


    <WebMethod()> _
    Public Function ConfirmExportEx(ByVal AuthorizationCode As String, _
                                  ByVal APControl As Integer, _
                                  ByRef LastError As String) As Boolean


        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(apExport)
            result = apExport.confirmExportEx(Utilities.GetConnectionString(), APControl)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportData.ConfirmExportEx", result, mstrLastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.ConfirmExportEx Failure", result, "Cannot Confirm AP Export for APControl Number " & APControl.ToString & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function
    

End Class