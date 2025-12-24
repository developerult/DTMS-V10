Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class PickListObject
    Inherits System.Web.Services.WebService
    'Note: replace all instances of  ''' <c>ClassLibrary1.TraceExtension()</c> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
    'Should only be run For diagnostics Or In test systems.

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetData(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompanyNumber As String,
                            ByVal OrderNumber As String,
                            ByRef RetVal As Integer,
                            ByRef LastError As String) As clsPickListData


        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList

        Dim Headers() As Ngl.FreightMaster.Integration.clsPickListObject
        Dim Details() As Ngl.FreightMaster.Integration.clsPickDetailObject

        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber
        Dim pl As New clsPickListData
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return pl
            Utilities.populateIntegrationObjectParameters(picklist)
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = picklist.readObjectData(Headers, Details, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber)
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = picklist.LastError
            LastError = mstrLastError
            Utilities.LogResults("Picklist", RetVal, mstrLastError, AuthorizationCode)

            pl.Headers = Headers
            pl.Details = Details
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("PicklistObject.GetData Failure", RetVal, "Cannot get Picklist Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export Picklist Data Failure")
        End Try
        Return pl

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
                            ByRef RetVal As Integer,
                            ByRef LastError As String) As clsPickListData

        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList

        Dim Headers() As Ngl.FreightMaster.Integration.clsPickListObject
        Dim Details() As Ngl.FreightMaster.Integration.clsPickDetailObject
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber
        Dim pl As New clsPickListData
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return pl
            Utilities.populateIntegrationObjectParameters(picklist)
            picklist.MaxRowsReturned = MaxRowsReturned
            picklist.AutoConfirmation = AutoConfirmation
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = picklist.readObjectData(Headers, Details, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber)
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = picklist.LastError
            LastError = mstrLastError
            Utilities.LogResults("Picklist", RetVal, mstrLastError, AuthorizationCode)
            pl.Headers = Headers
            pl.Details = Details
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("PicklistObject.GetDataEx Failure", RetVal, "Cannot get Picklist Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export Picklist Data Failure")
        End Try
        Return pl

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetData60(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompanyNumber As String,
                            ByVal OrderNumber As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef RetVal As Integer,
                            ByRef LastError As String) As clsPickListData60

        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList

        Dim Headers As New List(Of Ngl.FreightMaster.Integration.clsPickListObject60)
        Dim Details As New List(Of Ngl.FreightMaster.Integration.clsPickDetailObject60)
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber
        Dim pl As New clsPickListData60
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return pl
            Utilities.populateIntegrationObjectParameters(picklist)
            picklist.MaxRowsReturned = MaxRowsReturned
            picklist.AutoConfirmation = AutoConfirmation
            RetVal = picklist.readObjectData60(Headers, Details, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber)
            mstrLastError = picklist.LastError
            LastError = mstrLastError
            Utilities.LogResults("Picklist", RetVal, mstrLastError, AuthorizationCode)
            pl.Headers = Headers.ToArray
            pl.Details = Details.ToArray
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("PicklistObject.GetDataEx Failure", RetVal, "Cannot get Picklist Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export Picklist Data Failure")
        End Try
        Return pl

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
                            ByRef RetVal As Integer,
                            ByRef LastError As String) As clsPickListData


        If CompanyNumber = "" Then CompanyNumber = Nothing
        If OrderNumber = "" Then OrderNumber = Nothing
        If OrderSequence = "" Then OrderSequence = Nothing

        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList

        Dim Headers() As Ngl.FreightMaster.Integration.clsPickListObject
        Dim Details() As Ngl.FreightMaster.Integration.clsPickDetailObject
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber
        If Not String.IsNullOrEmpty(OrderSequence) Then strCriteria &= " OrderSequence = " & OrderSequence
        Dim pl As New clsPickListData
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return pl
            Utilities.populateIntegrationObjectParameters(picklist)
            picklist.MaxRowsReturned = MaxRowsReturned
            picklist.AutoConfirmation = AutoConfirmation
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = picklist.readObjectData(Headers, Details, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, OrderSequence)
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = picklist.LastError
            LastError = mstrLastError
            Utilities.LogResults("Picklist", RetVal, mstrLastError, AuthorizationCode)
            pl.Headers = Headers
            pl.Details = Details
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("PicklistObject.GetDataByOrderSequence Failure", RetVal, "Cannot get Picklist Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export Picklist Data Failure")
        End Try
        Return pl

    End Function

    ''' <summary>
    ''' Note:  The PLControl must be a long integer representing the primary key for the pick list record to confirm.
    ''' The BookCarrOrderNumber and the CompanyNumber parameters are no longer used and have been maintained 
    ''' for backward compatibility only    ''' 
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="PlControl"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="CompanyNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ConfirmExport(ByVal AuthorizationCode As String, ByVal PlControl As String,
        ByVal BookCarrOrderNumber As String, ByVal CompanyNumber As String) As Boolean

        Dim lngPLControl As Long = 0
        If Not Long.TryParse(PlControl, lngPLControl) Then
            Utilities.LogResults("PicklistObject", 0, "The PlControl Number " & PlControl & " is not valid. expecting a long integer.", AuthorizationCode)
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
            Utilities.LogResults("PicklistObject", picklist.Results, picklist.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            Utilities.LogException("PicklistObject.ConfirmExport Failure", result, "Cannot Confirm Picklist Export using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate Picklist Export Record is Possible")
        End Try
        Return result

    End Function

    ''' <summary>
    ''' Add code to read data from clsPickList
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="MaxRowsReturned"></param>
    ''' <param name="AutoConfirmation"></param>
    ''' <param name="RetVal"></param>
    ''' <param name="LastError"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetData70(ByVal AuthorizationCode As String, _
                            ByVal MaxRetry As Integer, _
                            ByVal RetryMinutes As Integer, _
                            ByVal CompLegalEntity As String, _
                            ByVal MaxRowsReturned As Integer, _
                            ByVal AutoConfirmation As Boolean, _
                            ByRef RetVal As Integer, _
                            ByRef LastError As String) As clsPickListData70

        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim pl As New clsPickListData70
        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim Headers() As Ngl.FreightMaster.Integration.clsPickListObject70
        Dim Details() As Ngl.FreightMaster.Integration.clsPickDetailObject70
        Dim Fees() As Ngl.FreightMaster.Integration.clsPickListFeeObject70

        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompLegalEntity) Then strCriteria &= " CompLegalEntity = " & CompLegalEntity

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return pl
            Utilities.populateIntegrationObjectParameters(picklist)
            picklist.MaxRowsReturned = MaxRowsReturned
            picklist.AutoConfirmation = AutoConfirmation
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = picklist.readObjectData70(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = picklist.LastError
            LastError = mstrLastError
            Utilities.LogResults("Picklist", RetVal, mstrLastError, AuthorizationCode)
            pl.Headers = Headers
            pl.Details = Details
            pl.Fees = Fees
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("PicklistObject.GetDataEx Failure", RetVal, "Cannot get Picklist Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export Picklist Data Failure")
        End Try
        Return pl
    End Function


End Class

<Serializable()> _
Public Class clsPickListData
    Public Headers() As Ngl.FreightMaster.Integration.clsPickListObject
    Public Details() As Ngl.FreightMaster.Integration.clsPickDetailObject
End Class

<Serializable()> _
Public Class clsPickListData60
    Public Headers() As Ngl.FreightMaster.Integration.clsPickListObject60
    Public Details() As Ngl.FreightMaster.Integration.clsPickDetailObject60
End Class


<Serializable()>
Public Class clsPickListData70
    Public Headers() As Ngl.FreightMaster.Integration.clsPickListObject70
    Public Details() As Ngl.FreightMaster.Integration.clsPickDetailObject70
    Public Fees() As Ngl.FreightMaster.Integration.clsPickListFeeObject70
End Class


''' <summary>
''' Pick Worksheet return data for web services
''' </summary>
''' <remarks>
''' Created by RHR v-8.2.0.117 7/17/2019
'''   replaces the 70 version Of the data
'''   includes support for BookItemOrderNumber 
''' </remarks>
<Serializable()>
Public Class clsPickListData80
    Public Headers() As Ngl.FreightMaster.Integration.clsPickListObject80
    Public Details() As Ngl.FreightMaster.Integration.clsPickDetailObject80
    Public Fees() As Ngl.FreightMaster.Integration.clsPickListFeeObject80
End Class


''' <summary>
''' Pick Worksheet return data for web services
''' </summary>
''' <remarks>
''' Created by by RHR for v-8.5.0.002 on 12/03/2021 added Scheduler Fields
''' </remarks>
<Serializable()>
Public Class clsPickListData85
    Public Headers() As Ngl.FreightMaster.Integration.clsPickListObject85
    Public Details() As Ngl.FreightMaster.Integration.clsPickDetailObject85
    Public Fees() As Ngl.FreightMaster.Integration.clsPickListFeeObject85
End Class