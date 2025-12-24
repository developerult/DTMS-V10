Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Picklist
    Inherits System.Web.Services.WebService
    'Note: replace all instances of  ''' <c>ClassLibrary1.TraceExtension()</c> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
    'Should only be run For diagnostics Or In test systems.

    Private mstrLastError As String = ""
    <WebMethod()>
    Public Function LastError() As String
        Return mstrLastError
    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetDataset() As Ngl.FreightMaster.Integration.PickListData

        Return New Ngl.FreightMaster.Integration.PickListData

    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetData(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompanyNumber As String,
                            ByVal OrderNumber As String,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.PickListData

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If OrderNumber = "" Then OrderNumber = Nothing

        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim ds As New Ngl.FreightMaster.Integration.PickListData
        ds.Tables.Clear()
        Dim dt As New Ngl.FreightMaster.Integration.PickListData.PickListDataTable
        Dim dtDetails As New Ngl.FreightMaster.Integration.PickListData.PickDetailDataTable
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ds
            Utilities.populateIntegrationObjectParameters(picklist)
            WSResult = picklist.readData(dt, dtDetails, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber)
            mstrLastError = picklist.LastError
            LastError = mstrLastError
            Utilities.LogResults("Picklist", WSResult, mstrLastError, AuthorizationCode)
            ds.Tables.Add(dt)
            ds.Tables.Add(dtDetails)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("Picklist.GetData Failure", WSResult, "Cannot get Picklist data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export Picklist Data Failure")
        End Try
        Return ds

    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetDataEx(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompanyNumber As String,
                            ByVal OrderNumber As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.PickListData

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If OrderNumber = "" Then OrderNumber = Nothing

        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim ds As New Ngl.FreightMaster.Integration.PickListData
        ds.Tables.Clear()
        Dim dt As New Ngl.FreightMaster.Integration.PickListData.PickListDataTable
        Dim dtDetails As New Ngl.FreightMaster.Integration.PickListData.PickDetailDataTable
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
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ds
            Utilities.populateIntegrationObjectParameters(picklist)
            picklist.MaxRowsReturned = MaxRowsReturned
            picklist.AutoConfirmation = AutoConfirmation
            WSResult = picklist.readData(dt, dtDetails, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber)
            mstrLastError = picklist.LastError
            LastError = mstrLastError
            Utilities.LogResults("Picklist", WSResult, mstrLastError, AuthorizationCode)
            ds.Tables.Add(dt)
            ds.Tables.Add(dtDetails)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("Picklist.GetDataEx Failure", WSResult, "Cannot get Picklist data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export Picklist Data Failure")
        End Try
        Return ds

    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetDataByOrderSequence(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompanyNumber As String,
                            ByVal OrderNumber As String,
                            ByVal OrderSequence As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.PickListData

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If OrderNumber = "" Then OrderNumber = Nothing
        If OrderSequence = "" Then OrderSequence = Nothing


        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim ds As New Ngl.FreightMaster.Integration.PickListData
        ds.Tables.Clear()
        Dim dt As New Ngl.FreightMaster.Integration.PickListData.PickListDataTable
        Dim dtDetails As New Ngl.FreightMaster.Integration.PickListData.PickDetailDataTable
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
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ds
            Utilities.populateIntegrationObjectParameters(picklist)
            picklist.MaxRowsReturned = MaxRowsReturned
            picklist.AutoConfirmation = AutoConfirmation
            WSResult = picklist.readData(dt, dtDetails, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, OrderSequence)
            mstrLastError = picklist.LastError
            LastError = mstrLastError
            Utilities.LogResults("Picklist", WSResult, mstrLastError, AuthorizationCode)
            ds.Tables.Add(dt)
            ds.Tables.Add(dtDetails)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("Picklist.GetDataByOrderSequence Failure", WSResult, "Cannot get Picklist Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export Picklist Data Failure")
        End Try
        Return ds

    End Function
    ''' <summary>
    ''' Note:  The PLControl must be a long integer representing the primary key for the pick list record to confirm.
    ''' The BookCarrOrderNumber and the CompanyNumber parameters are no longer used and have been maintained 
    ''' for backward compatibility only
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="PlControl"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="CompanyNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()> _
    Public Function ConfirmExport(ByVal AuthorizationCode As String, ByVal PlControl As String, _
        ByVal BookCarrOrderNumber As String, ByVal CompanyNumber As String) As Boolean
        Dim lngPLControl As Long = 0

        If Not Long.TryParse(PlControl, lngPLControl) Then
            Utilities.LogResults("Picklist", 0, "The PlControl Number " & PlControl & " is not valid. expecting a long integer.", AuthorizationCode)
            Return False
        End If
        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim strCriteria As String = ""
        strCriteria &= " Pl Control = " & PlControl.ToString
        If Not String.IsNullOrEmpty(BookCarrOrderNumber) Then strCriteria &= " Order Number = " & BookCarrOrderNumber
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " Company Number = " & CompanyNumber
        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(picklist)
            result = picklist.confirmExport(Utilities.GetConnectionString(), lngPLControl)
            mstrLastError = picklist.LastError
            Utilities.LogResults("Picklist", result, mstrLastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            Utilities.LogException("Picklist.ConfirmExport Failure", result, "Cannot Confirm Picklist Export using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate Picklist Export Record is Possible")
        End Try
        Return result

    End Function

End Class