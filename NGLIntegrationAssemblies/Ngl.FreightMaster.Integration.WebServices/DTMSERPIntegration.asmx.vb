Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Ngl.FreightMaster.Integration
Imports System.Xml.Serialization
Imports Ngl.FreightMaster.Integration.Configuration
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://dtmsERPIntegration.nextgeneration.com/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class DTMSERPIntegration
    Inherits System.Web.Services.WebService
    'Note: replace all instances of   ''' <c>ClassLibrary1.TraceExtension()</c> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.    
    'Must Add Reference to ClassLibrary1 project if it is not already added
    'Should only be run For diagnostics Or In test systems.


    Private mstrLastError As String = ""
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function LastError() As String
        Return mstrLastError
    End Function

#Region "Company Data"

    ''' <summary>
    ''' Imports Company Data using the 70 interface  Contacts are Suggested but Calendar data is optional
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
    Public Function ProcessCompanyData70(ByVal AuthorizationCode As String,
        ByVal CompanyHeaders() As Ngl.FreightMaster.Integration.clsCompanyHeaderObject70,
        ByVal CompanyContacts() As Ngl.FreightMaster.Integration.clsCompanyContactObject70,
        ByVal CompanyCalendar() As Ngl.FreightMaster.Integration.clsCompanyCalendarObject70,
        ByRef ReturnMessage As String) As clsIntegrationUpdateResults
        Dim oRes As New clsIntegrationUpdateResults
        oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessCompanyData70"
        Dim sDataType As String = "Company"
        Try
            If CompanyHeaders Is Nothing OrElse CompanyHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationComplete
                Return oRes
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
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                oRes.ReturnValue = ProcessDataReturnValues.nglDataValidationFailure
                Return oRes
            End If
            Utilities.populateIntegrationObjectParameters(company)
            oRes = company.ProcessObjectData70(lCompHeaders, lCompContacts, Utilities.GetConnectionString(), lCompCals)
            ReturnMessage = company.LastError
            Utilities.LogResults(sSource, oRes.ReturnValue, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, oRes.ReturnValue, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try

        End Try
        Return oRes

    End Function

#End Region

#Region "Carrier Data"

    ''' <summary>
    ''' Imports Carrier Data using the 70 interface Contacts Suggested Calendar is optional.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="CarrierHeaders"></param>
    ''' <param name="CarrierContacts"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessCarrierData70(ByVal AuthorizationCode As String,
        ByVal CarrierHeaders() As Ngl.FreightMaster.Integration.clsCarrierHeaderObject70,
        ByVal CarrierContacts() As Ngl.FreightMaster.Integration.clsCarrierContactObject70,
        ByRef ReturnMessage As String) As clsIntegrationUpdateResults
        Dim oRes As New clsIntegrationUpdateResults
        oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessCarrierData70"
        Dim sDataType As String = "Company"
        Try
            If CarrierHeaders Is Nothing OrElse CarrierHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationComplete
                Return oRes
            End If

            Dim lCarrierHeaders As List(Of Ngl.FreightMaster.Integration.clsCarrierHeaderObject70) = CarrierHeaders.ToList()
            Dim lCarrierContacts As New List(Of Ngl.FreightMaster.Integration.clsCarrierContactObject70)
            If Not CarrierContacts Is Nothing AndAlso CarrierContacts.Count() > 0 Then
                lCarrierContacts = CarrierContacts.ToList()
            End If
            Dim carrier As New Ngl.FreightMaster.Integration.clsCarrier
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                oRes.ReturnValue = ProcessDataReturnValues.nglDataValidationFailure
                Return oRes
            End If
            Utilities.populateIntegrationObjectParameters(carrier)
            oRes = carrier.ProcessObjectData70(lCarrierHeaders, lCarrierContacts, Utilities.GetConnectionString())
            ReturnMessage = carrier.LastError
            Utilities.LogResults(sSource, oRes.ReturnValue, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, oRes.ReturnValue, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return oRes
    End Function

#End Region

#Region "Lane Data"

    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function doesLaneExist(ByVal AuthorizationCode As String,
                        ByVal sLaneNumber As String,
                        ByRef ReturnMessage As String) As Boolean

        Dim blnRet As Boolean = False
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.doesLaneExist"
        Dim sDataType As String = "Lane"
        Try

            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                ReturnMessage = "Invalie Authorization Code " & AuthorizationCode
                Return False
            End If
            Utilities.populateIntegrationObjectParameters(lane)
            blnRet = lane.doesLaneExist(sLaneNumber)
            ReturnMessage = lane.LastError
            Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, 0, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Imports Lane Data using the 70 inteface calendar data is optional
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="Lanes"></param>
    ''' <param name="Calendar"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessLaneData70(ByVal AuthorizationCode As String,
            ByVal Lanes() As Ngl.FreightMaster.Integration.clsLaneObject70,
            ByVal Calendar() As Ngl.FreightMaster.Integration.clsLaneCalendarObject70,
            ByRef ReturnMessage As String) As clsIntegrationUpdateResults
        Dim oRes As New clsIntegrationUpdateResults
        oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessLaneData70"
        Dim sDataType As String = "Lane"
        Try
            If Lanes Is Nothing OrElse Lanes.Length = 0 Then
                ReturnMessage = "No Lanes"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationComplete
                Return oRes
            End If
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                oRes.ReturnValue = ProcessDataReturnValues.nglDataValidationFailure
                Return oRes
            End If
            Utilities.populateIntegrationObjectParameters(lane)

            Dim lLanes As List(Of Ngl.FreightMaster.Integration.clsLaneObject70) = Lanes.ToList()
            Dim lLaneCals As New List(Of Ngl.FreightMaster.Integration.clsLaneCalendarObject70)
            If Not Calendar Is Nothing AndAlso Calendar.Count() > 0 Then
                lLaneCals = Calendar.ToList()
            End If
            oRes = lane.ProcessObjectData70(lLanes, Utilities.GetConnectionString(), lLaneCals)
            ReturnMessage = lane.LastError
            Utilities.LogResults(sSource, oRes.ReturnValue, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, oRes.ReturnValue, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return oRes

    End Function


    ''' <summary>
    ''' Imports Lane Data using the 80 inteface calendar data is optional
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="Lanes"></param>
    ''' <param name="Calendar"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessLaneData80(ByVal AuthorizationCode As String,
            ByVal Lanes() As Ngl.FreightMaster.Integration.clsLaneObject80,
            ByVal Calendar() As Ngl.FreightMaster.Integration.clsLaneCalendarObject80,
            ByRef ReturnMessage As String) As clsIntegrationUpdateResults
        Dim oRes As New clsIntegrationUpdateResults
        oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessLaneData80"
        Dim sDataType As String = "Lane"
        Try
            If Lanes Is Nothing OrElse Lanes.Length = 0 Then
                ReturnMessage = "No Lanes"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationComplete
                Return oRes
            End If
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                oRes.ReturnValue = ProcessDataReturnValues.nglDataValidationFailure
                Return oRes
            End If
            Utilities.populateIntegrationObjectParameters(lane)

            Dim lLanes As List(Of Ngl.FreightMaster.Integration.clsLaneObject80) = Lanes.ToList()
            Dim lLaneCals As New List(Of Ngl.FreightMaster.Integration.clsLaneCalendarObject80)
            If Not Calendar Is Nothing AndAlso Calendar.Count() > 0 Then
                lLaneCals = Calendar.ToList()
            End If
            oRes = lane.ProcessObjectData80(lLanes, Utilities.GetConnectionString(), lLaneCals)
            ReturnMessage = lane.LastError
            Utilities.LogResults(sSource, oRes.ReturnValue, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            oRes.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, oRes.ReturnValue, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return oRes

    End Function

#End Region

#Region "Pallet Type Data"

    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessPalletTypeData70(ByVal AuthorizationCode As String,
                                ByVal PalletTypes() As Ngl.FreightMaster.Integration.clsPalletTypeObject,
                                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessPalletTypeData70"
        Dim sDataType As String = "Pallet Type"
        Try
            If PalletTypes Is Nothing OrElse PalletTypes.Length = 0 Then
                ReturnMessage = "No Data"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataIntegrationComplete
                Return ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            Dim dataObject As New Ngl.FreightMaster.Integration.clsPalletType
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                Return result
            End If
            Utilities.populateIntegrationObjectParameters(dataObject)
            With dataObject
                result = .ProcessObjectData(PalletTypes.ToList, Utilities.GetConnectionString())
                ReturnMessage = .LastError
                Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)
            End With

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

#End Region

#Region "Hazmat Data"
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessHazmatData70(ByVal AuthorizationCode As String,
                                ByVal Hazmats() As Ngl.FreightMaster.Integration.clsHazmatObject,
                                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessHazmatData70"
        Dim sDataType As String = "Hazmat"
        Try
            If Hazmats Is Nothing OrElse Hazmats.Length = 0 Then
                ReturnMessage = "No Data"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataIntegrationComplete
                Return result
            End If
            Dim dataObject As New Ngl.FreightMaster.Integration.clsHazmat
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(dataObject)
            With dataObject
                result = .ProcessData(Hazmats.ToList, Utilities.GetConnectionString())
                ReturnMessage = .LastError
                Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)
            End With

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

#End Region

#Region "Orders / Book Data"

    ''' <summary>
    ''' Add functionality to save records in clsBook
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="OrderHeaders"></param>
    ''' <param name="OrderDetails"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessBookData70(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject70,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject70,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessBookData70"
        Dim sDataType As String = "Book"
        Try
            If OrderHeaders Is Nothing OrElse OrderHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataIntegrationComplete
                Return result
            End If
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            Dim lOrderHeaders As List(Of Ngl.FreightMaster.Integration.clsBookHeaderObject70) = OrderHeaders.ToList()
            Dim lOrderDetails As New List(Of Ngl.FreightMaster.Integration.clsBookDetailObject70)
            If Not OrderDetails Is Nothing AndAlso OrderDetails.Length > 0 Then
                lOrderDetails = OrderDetails.ToList()
            End If
            result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, Utilities.GetConnectionString())
            ReturnMessage = book.LastError
            Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function


    ''' <summary>
    ''' version 705 integration includes support for new ChangeNo key fields for header and footer references
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="OrderHeaders"></param>
    ''' <param name="OrderDetails"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 07/21/2016
    ''' Added ChangeNo field to  match header records with item detail records
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessBookData705(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject705,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject705,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessBookData705"
        Dim sDataType As String = "Book"
        Try
            If OrderHeaders Is Nothing OrElse OrderHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataIntegrationComplete
                Return result
            End If
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            Dim lOrderHeaders As List(Of Ngl.FreightMaster.Integration.clsBookHeaderObject705) = OrderHeaders.ToList()
            Dim lOrderDetails As New List(Of Ngl.FreightMaster.Integration.clsBookDetailObject705)
            If Not OrderDetails Is Nothing AndAlso OrderDetails.Length > 0 Then
                lOrderDetails = OrderDetails.ToList()
            End If
            result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, Utilities.GetConnectionString())
            ReturnMessage = book.LastError
            Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function


    ''' <summary>
    ''' version 80 integration includes support for new NAV fields
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="OrderHeaders"></param>
    ''' <param name="OrderDetails"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.2 09/11/2018
    '''     Supports changes implemented in NAV Integration
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessBookData80(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject80,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject80,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessBookData80"
        Dim sDataType As String = "Book"
        Try
            If OrderHeaders Is Nothing OrElse OrderHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataIntegrationComplete
                Return result
            End If
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            Dim lOrderHeaders As List(Of Ngl.FreightMaster.Integration.clsBookHeaderObject80) = OrderHeaders.ToList()
            Dim lOrderDetails As New List(Of Ngl.FreightMaster.Integration.clsBookDetailObject80)
            If Not OrderDetails Is Nothing AndAlso OrderDetails.Length > 0 Then
                lOrderDetails = OrderDetails.ToList()
            End If
            result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, Utilities.GetConnectionString())
            ReturnMessage = book.LastError
            Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    ''' <summary>
    ''' Look up existing order in TMS by Order Number (prvided) and update the POHDR status using the delete code
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1.007 on 11/15/2020 
    '''     part of the CPF requirement for backward compatibility     
    ''' </remarks>
    <WebMethod()>
    Public Function ProcessDeleteByOrderNumber(ByVal AuthorizationCode As String,
                                               ByVal OrderNumber As String,
                                               ByRef ReturnMessage As String) As Integer
        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessDeleteByOrderNumber"
        Dim sDataType As String = "Book"
        Try
            If String.IsNullOrWhiteSpace(OrderNumber) Then
                ReturnMessage = "Missing Order Number"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataConnectionFailure
                Return result
            End If
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            result = book.ProcessDeleteByOrderNumber(OrderNumber, Utilities.GetConnectionString())
            ReturnMessage = book.LastError
            Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process delete booking request by order number.  ", ex, AuthorizationCode, "Process process delete booking request by order number failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function




    ''' <summary>
    ''' Returns cost per pound
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="BookOrderSequence"></param>
    ''' <param name="CompAlphaCode"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 11/27/2017 for v-7.0.6.105
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetCostPerPoundForOrder(ByVal AuthorizationCode As String,
                                            ByVal BookCarrOrderNumber As String,
                                            ByVal BookOrderSequence As Integer,
                                            ByVal CompAlphaCode As String,
                                            ByVal CompLegalEntity As String) As Double
        Dim sSource As String = "DTMSERPIntegration.GetCostPerPoundForOrder"
        Dim result As Double = 0
        Try
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return -1
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            result = book.GetCostPerPoundForOrder(BookCarrOrderNumber, BookOrderSequence, CompAlphaCode, CompLegalEntity)
        Catch ex As Exception
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot Get Cost Per Pound For Order.  ", ex, AuthorizationCode, "Process Cost Per Pound Failure")

        End Try
        Return result

    End Function

    ''' <summary>
    ''' Returns cost per pound
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="BookCarrOrderNumber"></param>
    ''' <param name="BookOrderSequence"></param>
    ''' <param name="CompNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR on 11/27/2017 for v-7.0.6.105
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetCostPerPoundForOrderByCompNumber(ByVal AuthorizationCode As String,
                                                        ByVal BookCarrOrderNumber As String,
                                                        ByVal BookOrderSequence As Integer,
                                                        ByVal CompNumber As Integer) As Double
        Dim sSource As String = "DTMSERPIntegration.GetCostPerPoundForOrder"
        Dim result As Double = 0
        Try
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return -1
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            result = book.GetCostPerPoundForOrder(BookCarrOrderNumber, BookOrderSequence, "", "", CompNumber)
        Catch ex As Exception
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot Get Cost Per Pound For Order.  ", ex, AuthorizationCode, "Process Cost Per Pound Failure")

        End Try
        Return result

    End Function

#End Region

#Region "Pick List Data"

    ''' <summary>
    ''' Request Pick List Status Update data using the 70 interface.  AutoConfirmation is typically false.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="MaxRowsReturned"></param>
    ''' <param name="AutoConfirmation"></param>
    ''' <param name="RetVal"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetPickListData70(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompLegalEntity As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef RetVal As Integer,
                            ByRef ReturnMessage As String) As clsPickListData70

        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim pl As New clsPickListData70
        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim Headers() As Ngl.FreightMaster.Integration.clsPickListObject70
        Dim Details() As Ngl.FreightMaster.Integration.clsPickDetailObject70
        Dim Fees() As Ngl.FreightMaster.Integration.clsPickListFeeObject70

        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetPickListData70"
        Dim sDataType As String = "Pick List"
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
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = picklist.readObjectData70(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = picklist.LastError
            ReturnMessage = mstrLastError
            Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode)
            pl.Headers = Headers
            pl.Details = Details
            pl.Fees = Fees
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, RetVal, "Cannot process " & sDataType & " data using " & strCriteria & ".  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return pl
    End Function


    ''' <summary>
    ''' Request Pick List Status Update data using the 80 interface.  AutoConfirmation is typically false.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="MaxRowsReturned"></param>
    ''' <param name="AutoConfirmation"></param>
    ''' <param name="RetVal"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the data
    '''   includes support for BookItemOrderNumber 
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetPickListData80(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompLegalEntity As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef RetVal As Integer,
                            ByRef ReturnMessage As String) As clsPickListData80

        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim pl As New clsPickListData80
        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim Headers() As Ngl.FreightMaster.Integration.clsPickListObject80
        Dim Details() As Ngl.FreightMaster.Integration.clsPickDetailObject80
        Dim Fees() As Ngl.FreightMaster.Integration.clsPickListFeeObject80

        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetPickListData80"
        Dim sDataType As String = "Pick List"
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
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = picklist.readObjectData80(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = picklist.LastError
            ReturnMessage = mstrLastError
            Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode)
            pl.Headers = Headers
            pl.Details = Details
            pl.Fees = Fees
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, RetVal, "Cannot process " & sDataType & " data using " & strCriteria & ".  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return pl
    End Function



    ''' <summary>
    ''' Request Pick List Status Update data using the 80 interface.  AutoConfirmation is typically false.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="MaxRowsReturned"></param>
    ''' <param name="AutoConfirmation"></param>
    ''' <param name="RetVal"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by by RHR for v-8.5.0.002 on 12/03/2021 added Scheduler Fields
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetPickListData85(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompLegalEntity As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef RetVal As Integer,
                            ByRef ReturnMessage As String) As clsPickListData85

        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim pl As New clsPickListData85
        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim Headers() As Ngl.FreightMaster.Integration.clsPickListObject85
        Dim Details() As Ngl.FreightMaster.Integration.clsPickDetailObject85
        Dim Fees() As Ngl.FreightMaster.Integration.clsPickListFeeObject85

        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetPickListData85"
        Dim sDataType As String = "Pick List"
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
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = picklist.readObjectData85(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = picklist.LastError
            ReturnMessage = mstrLastError
            Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode)
            pl.Headers = Headers
            pl.Details = Details
            pl.Fees = Fees
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, RetVal, "Cannot process " & sDataType & " data using " & strCriteria & ".  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return pl
    End Function


    ''' <summary>
    ''' Update the Pick List record as recieved for the PLControl number provided. 
    ''' Returns true for success and false for failure; ReturnMessage may contain details about failures.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="PlControl"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ConfirmPickListExport70(ByVal AuthorizationCode As String, ByVal PlControl As Long, ByRef ReturnMessage As String) As Boolean
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ConfirmPickListExport70"
        Dim sDataType As String = "Pick List Confirmation"
        Dim result As Boolean = False

        Dim picklist As New Ngl.FreightMaster.Integration.clsPickList
        Dim strCriteria As String = ""
        strCriteria &= " Pl Control = " & PlControl.ToString
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(picklist)
            result = picklist.confirmExport(Utilities.GetConnectionString(), PlControl)
            If Not result Then ReturnMessage = picklist.LastError
            Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data using " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate Picklist Export Record is Possible")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function


#End Region

#Region "AP Export"

    ''' <summary>
    ''' Request AP Export data using the 70 interface.  AutoConfirmation is typically false.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="MaxRowsReturned"></param>
    ''' <param name="AutoConfirmation"></param>
    ''' <param name="RetVal"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetAPData70(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompLegalEntity As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef RetVal As Integer,
                            ByRef ReturnMessage As String) As clsAPExportData70

        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim ap As New clsAPExportData70
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim Headers() As Ngl.FreightMaster.Integration.clsAPExportObject70
        Dim Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject70
        Dim Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject70

        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetAPData70"
        Dim sDataType As String = "AP"
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
#Disable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Disable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            RetVal = apExport.readObjectData70(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            ReturnMessage = mstrLastError
            Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode)
            ap.Headers = Headers
            ap.Details = Details
            ap.Fees = Fees
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, RetVal, "Cannot get AP Export Object data with details using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return ap

    End Function


    ''' <summary>
    ''' Request AP Export data using the 70 interface.  AutoConfirmation is typically false.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="MaxRowsReturned"></param>
    ''' <param name="AutoConfirmation"></param>
    ''' <param name="RetVal"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Created by RHR v-8.2.0.117 7/17/2019
    '''   replaces the 70 version Of the data
    '''   includes support for BookItemOrderNumber 
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetAPData80(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompLegalEntity As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef RetVal As Integer,
                            ByRef ReturnMessage As String) As clsAPExportData80

        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim ap As New clsAPExportData80
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim Headers() As Ngl.FreightMaster.Integration.clsAPExportObject80
        Dim Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject80
        Dim Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject80

        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetAPData80"
        Dim sDataType As String = "AP"
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
            RetVal = apExport.readObjectData80(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            ReturnMessage = mstrLastError
            Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode)
            ap.Headers = Headers
            ap.Details = Details
            ap.Fees = Fees
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, RetVal, "Cannot get AP Export Object data with details using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP Data Failure")
        End Try
        Return ap

    End Function


    ''' <summary>
    ''' Request AP Export data using the  i85nterface.  AutoConfirmation is typically false.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="MaxRetry"></param>
    ''' <param name="RetryMinutes"></param>
    ''' <param name="CompLegalEntity"></param>
    ''' <param name="MaxRowsReturned"></param>
    ''' <param name="AutoConfirmation"></param>
    ''' <param name="RetVal"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks> 
    ''' Created by RHR v-8.5.1.001 03/28/2022
    '''   replaces the 80 version Of the data
    '''   includes new Item Level allocation of cost for
    '''     LineHaul, Fuel and Fees
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetAPData85(ByVal AuthorizationCode As String,
                            ByVal MaxRetry As Integer,
                            ByVal RetryMinutes As Integer,
                            ByVal CompLegalEntity As String,
                            ByVal MaxRowsReturned As Integer,
                            ByVal AutoConfirmation As Boolean,
                            ByRef RetVal As Integer,
                            ByRef ReturnMessage As String) As clsAPExportData85

        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim ap As New clsAPExportData85
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim Headers() As Ngl.FreightMaster.Integration.clsAPExportObject85
        Dim Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject85
        Dim Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject85

        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetAPData85"
        Dim sDataType As String = "AP"
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
            RetVal = apExport.readObjectData85(Headers, Utilities.GetConnectionString(), MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
#Enable Warning BC42030 ' Variable 'Fees' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Headers' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
#Enable Warning BC42030 ' Variable 'Details' is passed by reference before it has been assigned a value. A null reference exception could result at runtime.
            mstrLastError = apExport.LastError
            ReturnMessage = mstrLastError
            Utilities.LogResults(sSource, RetVal, mstrLastError, AuthorizationCode)
            ap.Headers = Headers
            ap.Details = Details
            ap.Fees = Fees
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, RetVal, "Cannot get AP Export 85 Object data with details using " & strCriteria & ".  ", ex, AuthorizationCode, "Export AP 85 Data Failure")
        End Try
        Return ap

    End Function


#Disable Warning BC42307 ' XML comment parameter 'LastError' does not match a parameter on the corresponding 'function' statement.
    ''' <summary>
    ''' Update the AP Export record as recieved for the APControl number provided.
    ''' Returns true for success and false for failure; ReturnMessage may contain details about failures.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="APControl"></param>
    ''' <param name="LastError"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ConfirmAPExport70(ByVal AuthorizationCode As String,
                                  ByVal APControl As Integer,
                                  ByRef ReturnMessage As String) As Boolean
#Enable Warning BC42307 ' XML comment parameter 'LastError' does not match a parameter on the corresponding 'function' statement.
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ConfirmAPExport70"
        Dim sDataType As String = "AP Export Confirmation"
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim result As Boolean = False
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Utilities.populateIntegrationObjectParameters(apExport)
            result = apExport.confirmExportEx(Utilities.GetConnectionString(), APControl)
            mstrLastError = apExport.LastError
            Utilities.LogResults(sSource, result, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & "  data using APControl Number " & APControl.ToString & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function


    ''' <summary>
    ''' Represents the AP Export data aggregated into a single invvoice for the entire freight bill.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/11/2016
    '''  currently used by the GP Integration Service
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetAPExportRecordsAggregated(ByVal AuthorizationCode As String,
                                                 ByVal MaxRetry As Integer,
                                                 ByVal RetryMinutes As Integer,
                                                 ByVal CompLegalEntity As String,
                                                 ByVal MaxRowsReturned As Integer,
                                                 ByRef RetVal As Integer,
                                                 ByRef ReturnMessage As String) As APExportRecordsAggregated()
        If CompLegalEntity = "" Then CompLegalEntity = Nothing

        Dim ap As New List(Of APExportRecordsAggregated)
        Dim APResults As New List(Of LTS.spGetAPExportRecordsAggregatedResult)
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetAPExportRecordsAggregated"
        Dim sDataType As String = "AP"
        Dim strCriteria As String = ""
        strCriteria &= " MaxRetry = " & MaxRetry.ToString
        strCriteria &= " RetryMinutes = " & RetryMinutes.ToString
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        If Not String.IsNullOrEmpty(CompLegalEntity) Then strCriteria &= " CompLegalEntity = " & CompLegalEntity

        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return ap.ToArray()
            Dim WCFParameters As New DAL.WCFParameters With
                {
                .ConnectionString = Utilities.GetConnectionString(),
                .DBServer = Utilities.GetServerName(Utilities.GetConnectionString()),
                .Database = Utilities.GetDatabase(Utilities.GetConnectionString()),
                .WCFAuthCode = "NGLSystem",
                .ValidateAccess = False,
                .UserName = ""
                }

            Dim oNGLAPDataLib As New DAL.NGLAPMassEntryData(WCFParameters)
            APResults = oNGLAPDataLib.GetAPExportRecordsAggregated(MaxRetry, RetryMinutes, CompLegalEntity, MaxRowsReturned)
            If Not APResults Is Nothing AndAlso APResults.Count() > 0 Then
                Dim skipObjects As New List(Of String) From {""}
                For Each apr As LTS.spGetAPExportRecordsAggregatedResult In APResults
                    Dim strMsg As String = ""
                    Dim nap As New APExportRecordsAggregated()
                    nap = Ngl.Core.Utility.DataTransformation.CopyMatchingFields(nap, apr, skipObjects, strMsg)
                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                        ReturnMessage &= strMsg
                        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Else
                        ap.Add(nap)
                    End If
                Next
                If (RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors) Then
                    Utilities.LogResults(sSource, RetVal, ReturnMessage, AuthorizationCode)
                Else
                    RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Not ap Is Nothing AndAlso ap.Count() > 0 Then
                    ReturnMessage = "Success! " & ap.Count().ToString & " AP records were selected for processing"
                Else
                    ReturnMessage = "No AP records are available for processing"
                End If
            Else
                RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
                ReturnMessage = "No AP records are available for processing"
            End If

            Utilities.LogResults(sSource, RetVal, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, RetVal, "Cannot Get AP Export Aggregated Records using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Export Records Aggregated Failure")
        End Try
        Return ap.ToArray()
    End Function

    ''' <summary>
    ''' Updates  the AP Export status flags based on thge SHID, Freight Bill Number.
    ''' Typically the value of APExport Flag is true  the APExport date is required.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="BookSHID"></param>
    ''' <param name="APBillNumber"></param>
    ''' <param name="APExportFlag"></param>
    ''' <param name="APExportDate"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/11/2016
    '''  currently used by the GP Integration Service
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function UpdateAPExportStatus(ByVal AuthorizationCode As String,
                                         ByVal BookSHID As String,
                                         ByVal APBillNumber As String,
                                         ByVal APExportFlag As Boolean,
                                         ByVal APExportDate As Date,
                                         ByRef ReturnMessage As String) As Boolean
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.UpdateAPExportStatus"
        Dim sDataType As String = "AP Export Status"
        'Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport
        Dim result As Boolean = False
        Dim strCriteria As String = ""
        strCriteria &= " BookSHID = " & BookSHID
        strCriteria &= " APBillNumber = " & APBillNumber
        strCriteria &= " APExportFlag = " & APExportFlag.ToString()
        strCriteria &= " APExportDate = " & APExportDate.ToString()
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return False
            Dim WCFParameters As New DAL.WCFParameters With
                {
                .ConnectionString = Utilities.GetConnectionString(),
                .DBServer = Utilities.GetServerName(Utilities.GetConnectionString()),
                .Database = Utilities.GetDatabase(Utilities.GetConnectionString()),
                .WCFAuthCode = "NGLSystem",
                .ValidateAccess = False,
                .UserName = ""
                }
            Dim oNGLAPDataLib As New DAL.NGLAPMassEntryData(WCFParameters)
            result = oNGLAPDataLib.UpdateStatus(BookSHID, APBillNumber, APExportFlag, APExportDate, Nothing)

            Utilities.LogResults(sSource, result, "Success!", AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException(sSource, result, "Cannot update " & sDataType & "  data using  " & strCriteria & ".  ", ex, AuthorizationCode, "A Duplicate AP Export Record is Possible")
        End Try
        Return result

    End Function

#End Region

#Region "Payables"

    ''' <summary>
    ''' Imports Payable data useing the 70 inteface.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="Payables"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function ProcessPayableData70(ByVal AuthorizationCode As String,
        ByVal Payables() As Ngl.FreightMaster.Integration.clsPayablesObject70,
        ByRef ReturnMessage As String) As Integer

        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.ProcessPayableData70"
        Dim sDataType As String = "Payable"
        Try
            If Payables Is Nothing OrElse Payables.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataIntegrationComplete
                Return ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            Dim oPayables As New Ngl.FreightMaster.Integration.clsPayables
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oPayables)
            result = oPayables.ProcessObjectData70(Payables.ToList(), Utilities.GetConnectionString())
            ReturnMessage = oPayables.LastError
            Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    ''' <summary>
    ''' Imports Payable data using the 705 inteface and Freight Bill Number as the key
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="Payables"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/11/2016
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function UpdatePayablesByFreightBill(ByVal AuthorizationCode As String,
                                                ByVal Payables() As Ngl.FreightMaster.Integration.clsPayablesObject705,
                                                 ByRef ReturnMessage As String) As Integer

        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.UpdatePayablesByFreightBill"
        Dim sDataType As String = "Payable"
        Try
            If Payables Is Nothing OrElse Payables.Length = 0 Then
                ReturnMessage = "Empty Payables Data Nothing to Do"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataIntegrationComplete
                Return result
            End If
            Dim oPayables As New Ngl.FreightMaster.Integration.clsPayables
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(oPayables)
            result = oPayables.UpdatePayablesByFreightBill(Payables.ToList(), Utilities.GetConnectionString())
            ReturnMessage = oPayables.LastError
            Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
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
    Public Function GetUnpaidFreightBills(ByVal AuthorizationCode As String,
                            ByVal LegalEntity As String,
                            ByVal MaxRowsReturned As Integer,
                            ByRef WSResult As Integer,
                            ByRef LastError As String) As Ngl.FreightMaster.Integration.APUnpaidFreightBills()


        Dim lUnpaid As New List(Of Ngl.FreightMaster.Integration.APUnpaidFreightBills)
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport()
        'set the default value to false
        WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        strCriteria &= " MaxRowsReturned = " & MaxRowsReturned.ToString
        strCriteria &= " LegalEntity = " & LegalEntity
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return lUnpaid.ToArray()
            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = MaxRowsReturned
            End With
            lUnpaid = apExport.GetUnpaidFreightBills(Utilities.GetConnectionString(), LegalEntity)
            If Not lUnpaid Is Nothing Then
                WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            End If
            mstrLastError = apExport.LastError
            LastError = mstrLastError
            Utilities.LogResults("DTMSERPIntegration.GetUnpaidFreightBills", WSResult, apExport.LastError, AuthorizationCode)
        Catch ex As Exception
            mstrLastError = ex.Message
            LastError = mstrLastError
            Utilities.LogException("DTMSERPIntegration.GetUnpaidFreightBills Failure", WSResult, "Cannot get unpaid freight bills using " & strCriteria & ".  ", ex, AuthorizationCode, "Get AP Open Payables Failure")
        End Try
        Return lUnpaid.ToArray()

    End Function

#End Region

#Region "Utilities or Lookup Services"

    ''' <summary>
    ''' Web Method to get the company abbreviation using the company number or the company alpha code.  
    ''' set the company number to zero to use the alpha code
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="compNumber"></param>
    ''' <param name="compAlphaCode"></param>
    ''' <param name="sDefault"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/11/2016
    '''  currently used by the GP Integration Service
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function GetCompAbrevByNumberOrAlpha(ByVal AuthorizationCode As String,
                                                ByVal compNumber As Integer,
                                                ByVal compAlphaCode As String,
                                                ByVal sDefault As String,
                                                ByRef ReturnMessage As String) As String
        Dim strRet As String = sDefault
        ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetCompAbrevByNumberOrAlpha"
        Dim sDataType As String = "Comp Abrev"
        Try
            If compNumber = 0 And String.IsNullOrWhiteSpace(compAlphaCode) Then
                ReturnMessage = "Missing Company Reference"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                Return strRet
            End If
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code."
                Return strRet
            End If
            Utilities.populateIntegrationObjectParameters(book)

            strRet = book.GetCompAbrevByNumberOrAlpha(compNumber, compAlphaCode, sDefault, ReturnMessage)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, ProcessDataReturnValues.nglDataIntegrationHadErrors, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return strRet

    End Function

    ''' <summary>
    ''' Web Method used to test if the order information has changed
    ''' using the Header data and the total items.  Used to determine 
    ''' if we are importing a new order or a modified order.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="oHeader"></param>
    ''' <param name="TotalItems"></param>
    ''' <param name="strChanges"></param>
    ''' <param name="TestTransType"></param>
    ''' <param name="TestModeType"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/11/2016
    '''   currently used by the GP Integration Service
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function HasOrderChanged(ByVal AuthorizationCode As String,
                                    ByVal oHeader As Ngl.FreightMaster.Integration.clsBookHeaderObject705,
                                    ByVal TotalItems As Integer,
                                    ByRef strChanges As String,
                                    ByVal TestTransType As Boolean,
                                    ByVal TestModeType As Boolean,
                                    ByRef ReturnMessage As String) As Boolean
        Dim blnRet As Boolean = False
        Dim sSource As String = "DTMSERPIntegration.HasOrderChanged"
        Dim sDataType As String = "Order"
        Try
            If oHeader Is Nothing Then
                ReturnMessage = "Order header information is missing"
                Return False
            End If

            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code."
                Return blnRet
            End If
            Utilities.populateIntegrationObjectParameters(book)

            blnRet = book.HasOrderChanged(oHeader, TotalItems, strChanges, TestTransType, TestModeType)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, ProcessDataReturnValues.nglDataIntegrationHadErrors, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' Web Method used to test if the order information has changed
    ''' using the Header data and the total items.  Used to determine 
    ''' if we are importing a new order or a modified order.
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="oHeader"></param>
    ''' <param name="TotalItems"></param>
    ''' <param name="strChanges"></param>
    ''' <param name="TestTransType"></param>
    ''' <param name="TestModeType"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.x on 05/15/2018
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c> 
    <WebMethod()>
    Public Function HasOrderChanged80(ByVal AuthorizationCode As String,
                                    ByVal oHeader As Ngl.FreightMaster.Integration.clsBookHeaderObject80,
                                    ByVal TotalItems As Integer,
                                    ByRef strChanges As String,
                                    ByVal TestTransType As Boolean,
                                    ByVal TestModeType As Boolean,
                                    ByRef ReturnMessage As String) As Boolean
        Dim blnRet As Boolean = False
        Dim sSource As String = "DTMSERPIntegration.HasOrderChanged"
        Dim sDataType As String = "Order"
        Try
            If oHeader Is Nothing Then
                ReturnMessage = "Order header information is missing"
                Return False
            End If

            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code."
                Return blnRet
            End If
            Utilities.populateIntegrationObjectParameters(book)

            blnRet = book.HasOrderChanged(oHeader, TotalItems, strChanges, TestTransType, TestModeType)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, ProcessDataReturnValues.nglDataIntegrationHadErrors, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function

#End Region

End Class