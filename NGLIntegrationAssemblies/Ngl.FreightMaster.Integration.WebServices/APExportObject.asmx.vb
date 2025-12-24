Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class APExportObject
    Inherits System.Web.Services.WebService
    'Note: replace all instances of  <ClassLibrary1.TraceExtension()> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
    'Should only be run For diagnostics Or In test systems.
    Private mstrLastError As String = ""
    ''''<ClassLibrary1.TraceExtension()>
    <WebMethod()>
    Public Function LastError() As String
        Return mstrLastError
    End Function

    ''' <summary>
    ''' Need to implement Read Data Functionality in clsAPExport
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="MaxRowsReturned"></param>
    ''' <param name="AutoConfirmation"></param>
    ''' <param name="WSResult"></param>
    ''' <param name="LastError"></param>
    ''' <returns></returns>
    ''' <remarks></remarks> 
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetData70(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompLegalEntity As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As clsAPExportData70

        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim ap As New clsAPExportData70
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim Headers() As Ngl.FreightMaster.Integration.clsAPExportObject70
        Dim Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject70
        Dim Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject70

        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(CompLegalEntity) Then strCriteria &= " CompLegalEntity = " & CompLegalEntity


        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ap
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            WSResult = apExport.readObjectData70(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetData70", WSResult, mstrLastError, AuthorizationCode)
            ap.Headers = Headers
            ap.Details = Details
            ap.Fees = Fees
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.GetData70 Failure", WSResult, "Cannot get AP Export Object data with details using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return ap

    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetDataWDetails(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompanyNumber As String,
                            ByVal OrderNumber As String,
                            ByVal OrderSequence As String,
                            ByVal BookProNumber As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As clsAPExportData

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If OrderNumber = "" Then OrderNumber = Nothing
        If OrderSequence = "" Then OrderSequence = Nothing

        Dim ap As New clsAPExportData
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim Headers() As Ngl.FreightMaster.Integration.clsAPExportObject
        Dim Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject

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
        If Not String.IsNullOrEmpty(BookProNumber) Then strCriteria &= " BookProNumber = " & BookProNumber


        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ap
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            WSResult = apExport.readObjectData(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, OrderSequence, BookProNumber, Details)
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetDataWDetails", WSResult, mstrLastError, AuthorizationCode)
            ap.Headers = Headers
            ap.Details = Details
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.GetDataWDetails Failure", WSResult, "Cannot get AP Export Object data with details using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return ap

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetAllDataWDetails(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As clsAPExportData

        Dim ap As New clsAPExportData
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim Headers() As Ngl.FreightMaster.Integration.clsAPExportObject
        Dim Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject

        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString


        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ap
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            WSResult = apExport.readObjectData(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, Nothing, Nothing, Nothing, Nothing, Details)
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetAllDataWDetails", WSResult, mstrLastError, AuthorizationCode)
            ap.Headers = Headers
            ap.Details = Details
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.GetAllDataWDetails Failure", WSResult, "Cannot get AP Export Object data with details using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return ap

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
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPExportObject()

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPExportObject
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
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return Nothing
            Utilities.populateIntegrationObjectParameters(apExport)
            Dim connectionString As String = Utilities.GetConnectionString()
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
#Disable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            WSResult = apExport.readObjectData(oData, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, Nothing, Nothing, Nothing)
#Enable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetDataEx", WSResult, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetDataEx Failure", WSResult, "Cannot get AP Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return oData

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
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPExportObject()

        If CompanyNumber = "" Then CompanyNumber = Nothing
        If OrderNumber = "" Then OrderNumber = Nothing
        If OrderSequence = "" Then OrderSequence = Nothing

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPExportObject
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
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return Nothing
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
#Disable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            WSResult = apExport.readObjectData(oData, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, OrderSequence, Nothing, Nothing)
#Enable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetDataEx", WSResult, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetDataEx Failure", WSResult, "Cannot get AP Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return oData

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetDataByProNumber(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal BookProNumber As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPExportObject()

        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPExportObject
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " AutoConfirmation = " & AutoConfirmation.ToString
        If Not String.IsNullOrEmpty(BookProNumber) Then strCriteria &= " BookProNumber = " & BookProNumber

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return Nothing
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
#Disable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            WSResult = apExport.readObjectData(oData, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, Nothing, Nothing, Nothing, BookProNumber, Nothing)
#Enable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetDataEx", WSResult, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportData.GetDataEx Failure", WSResult, "Cannot get AP Export data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return oData

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetData(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompanyNumber As String,
                            ByVal OrderNumber As String,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPExportObject()


        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPExportObject
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        If Not String.IsNullOrEmpty(OrderNumber) Then strCriteria &= " OrderNumber = " & OrderNumber

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return Nothing
            Utilities.populateIntegrationObjectParameters(apExport)
#Disable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            WSResult = apExport.readObjectData(oData, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompanyNumber, OrderNumber)
#Enable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetData", WSResult, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.GetData Failure", WSResult, "Cannot get AP Export Object data using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return oData

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ConfirmExport(ByVal AuthorizationCode As String,
                                  ByVal BookCarrOrderNumber As String,
                                  ByVal BookFinAPBillNumber As String,
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
            Utilities.LogResults("APExportObject.ConfirmExport", result, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            Utilities.LogException("APExportObject.ConfirmExport Failure", result, "Cannot Confirm AP Export Object using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ConfirmExportByOrderSequence(ByVal AuthorizationCode As String,
                                  ByVal BookCarrOrderNumber As String,
                                  ByVal BookOrderSequence As String,
                                  ByVal CompNumber As String,
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
            Utilities.LogResults("APExportObject.ConfirmExportEx", result, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.ConfirmExportEx Failure", result, "Cannot Confirm AP Export object using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ConfirmExportByPro(ByVal AuthorizationCode As String,
                                  ByVal BookProNumber As String,
                                  ByRef LastError As String) As Boolean
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(BookProNumber) Then strCriteria &= " Pro Number = " & BookProNumber
        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(apExport)
            result = apExport.confirmExport(strConnection:=Utilities.GetConnectionString(), BookProNumber:=BookProNumber)
            mstrLastError = apExport.LastError
            Utilities.LogResults("APExportObject.ConfirmExportEx", result, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.ConfirmExportEx Failure", result, "Cannot Confirm AP Export object using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ConfirmExportEx(ByVal AuthorizationCode As String,
                                  ByVal APControl As Integer,
                                  ByRef LastError As String) As Boolean
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(apExport)
            result = apExport.confirmExportEx(Utilities.GetConnectionString(), APControl)
            mstrLastError = apExport.LastError
            Utilities.LogResults("APExportObject.ConfirmExportEx", result, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.ConfirmExportEx Failure", result, "Cannot Confirm AP Export object for APControl Number " & APControl.ToString & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetOpenPayables(ByVal AuthorizationCode As String,
                                    ByVal CompanyNumber As String,
                                    ByRef WSResult As Integer,
                                    ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPExportObject()


        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPExportObject
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return Nothing
            Utilities.populateIntegrationObjectParameters(apExport)
#Disable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            WSResult = apExport.readObjectOpenPayables(oData, Utilities.GetConnectionString(), CompanyNumber)
#Enable Warning BC42030 ' Variable 'oData' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetOpenPayables", WSResult, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.GetOpenPayables Failure", WSResult, "Cannot get AP Open Payables object using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return oData

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()> _
    Public Function GetOpenPayablesEx(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByVal MaxRowsReturned As Integer, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As Ngl.FreightMaster.Integration.clsAPExportObject()


        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim oData() As Ngl.FreightMaster.Integration.clsAPExportObject
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        If Not String.IsNullOrEmpty(CompanyNumber) Then strCriteria &= " CompanyNumber = " & CompanyNumber
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(apExport)
            apExport.MaxRowsReturned = MaxRowsReturned
            WSResult = apExport.readObjectOpenPayables(oData, Utilities.GetConnectionString(), CompanyNumber)
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("APExportObject.GetOpenPayablesEx", WSResult, mstrLastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("APExportObject.GetOpenPayablesEx Failure", WSResult, "Cannot get AP Open Payables Object using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return oData

    End Function

End Class


<Serializable()> _
Public Class clsAPExportData
    Public Headers() As Ngl.FreightMaster.Integration.clsAPExportObject
    Public Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject
End Class


<Serializable()>
Public Class clsAPExportData70
    Public Headers() As Ngl.FreightMaster.Integration.clsAPExportObject70
    Public Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject70
    Public Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject70
End Class


<Serializable()>
Public Class clsAPExportData80
    Public Headers() As Ngl.FreightMaster.Integration.clsAPExportObject80
    Public Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject80
    Public Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject80
End Class


<Serializable()>
Public Class clsAPExportData85
    Public Headers() As Ngl.FreightMaster.Integration.clsAPExportObject85
    Public Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject85
    Public Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject85
End Class