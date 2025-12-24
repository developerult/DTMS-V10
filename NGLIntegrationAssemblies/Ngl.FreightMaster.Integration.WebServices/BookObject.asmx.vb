Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Ngl.FreightMaster.Integration
Imports System.Xml.Serialization
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data
#Disable Warning BC40056 ' Namespace or type specified in the Imports 'ClassLibrary1' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.
Imports ClassLibrary1
#Enable Warning BC40056 ' Namespace or type specified in the Imports 'ClassLibrary1' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.


<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class BookObject
    Inherits System.Web.Services.WebService
    'Note: replace all instances of  ''' <c>ClassLibrary1.TraceExtension()</c> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
    'Should only be run For diagnostics Or In test systems.

#Const LogsOn = False

    Private mstrLastError As String = ""
    <WebMethod()>
    Public Function LastError() As String
        Return mstrLastError
    End Function

    ' <c>ClassLibrary1.TraceExtension()</c>
    '<WebMethod()>
    'Public Function ProcessData(ByVal AuthorizationCode As String,
    '        ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject,
    '        ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject) As Integer

    '#If LogsOn Then
    '    <ClassLibrary1.TraceExtension()>
    '    <WebMethod()>
    '    Public Function ProcessData(ByVal AuthorizationCode As String,
    '            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject,
    '            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject) As Integer
    '#Else
    '    ''' <c>ClassLibrary1.TraceExtension()</c>
    '    <WebMethod()>
    '    Public Function ProcessData(ByVal AuthorizationCode As String,
    '        ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject,
    '        ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject) As Integer
    '#End If

    '''<c> ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject) As Integer
        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, OrderHeaders, OrderDetails, s)

    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessDataEx(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If OrderHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("BookObject", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            result = book.ProcessObjectData(OrderHeaders, OrderDetails, Utilities.GetConnectionString())
            ReturnMessage = book.LastError
            Utilities.LogResults("BookObject", result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("BookObject.ProcessDataEx Failure", result, "Cannot process book data.  ", ex, AuthorizationCode, "Process Book Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function
    '    <TraceExtension()>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData60(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject60,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject60,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If OrderHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("BookObject.ProcessData60", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            result = book.ProcessObjectData(OrderHeaders.ToList, OrderDetails.ToList, Utilities.GetConnectionString())
            ReturnMessage = book.LastError
            Utilities.LogResults("BookObject.ProcessData60", result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("BookObject.ProcessData60 Failure", result, "Cannot process book data.  ", ex, AuthorizationCode, "Process Book Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    ''' <summary>
    ''' Process Book Object Data web method for v-6.0.4.6
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="OrderHeaders"></param>
    ''' <param name="OrderDetails"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-6.0.4.6 on 12/2/2016
    '''   logic added to support BookItemCommCode for the xml wsdl 
    '''   full functionality to be completed in v-7.0.6.0
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData604(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject60,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject604,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If OrderHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("BookObject.ProcessData60", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
            End If
            Dim book As New Ngl.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            Dim lOrderHeaders As List(Of Ngl.FreightMaster.Integration.clsBookHeaderObject60) = OrderHeaders.ToList()
            Dim lOrderDetails As New List(Of Ngl.FreightMaster.Integration.clsBookDetailObject60)
            If Not OrderDetails Is Nothing AndAlso OrderDetails.Length > 0 Then
                lOrderDetails = (From d In OrderDetails
                                 Select New Ngl.FreightMaster.Integration.clsBookDetailObject60 _
                                 With {.ItemPONumber = d.ItemPONumber,
                                       .FixOffInvAllow = d.FixOffInvAllow,
                                       .FixFrtAllow = d.FixFrtAllow,
                                       .ItemNumber = d.ItemNumber,
                                       .QtyOrdered = d.QtyOrdered,
                                       .FreightCost = d.FreightCost,
                                       .ItemCost = d.ItemCost,
                                       .Weight = d.Weight,
                                       .Cube = d.Cube,
                                       .Pack = d.Pack,
                                       .Size = d.Size,
                                       .Description = d.Description,
                                       .Hazmat = d.Hazmat,
                                       .Brand = d.Brand,
                                       .CostCenter = d.CostCenter,
                                       .LotNumber = d.LotNumber,
                                       .LotExpirationDate = d.LotExpirationDate,
                                       .GTIN = d.GTIN,
                                       .CustItemNumber = d.CustItemNumber,
                                       .CustomerNumber = d.CustomerNumber,
                                       .POOrderSequence = d.POOrderSequence,
                                       .PalletType = d.PalletType,
                                       .POItemHazmatTypeCode = d.POItemHazmatTypeCode,
                                       .POItem49CFRCode = d.POItem49CFRCode,
                                       .POItemIATACode = d.POItemIATACode,
                                       .POItemDOTCode = d.POItemDOTCode,
                                       .POItemMarineCode = d.POItemMarineCode,
                                       .POItemNMFCClass = d.POItemNMFCClass,
                                       .POItemFAKClass = d.POItemFAKClass,
                                       .POItemLimitedQtyFlag = d.POItemLimitedQtyFlag,
                                       .POItemPallets = d.POItemPallets,
                                       .POItemTies = d.POItemTies,
                                       .POItemHighs = d.POItemHighs,
                                       .POItemQtyPalletPercentage = d.POItemQtyPalletPercentage,
                                       .POItemQtyLength = d.POItemQtyLength,
                                       .POItemQtyWidth = d.POItemQtyWidth,
                                       .POItemQtyHeight = d.POItemQtyHeight,
                                       .POItemStackable = d.POItemStackable,
                                       .POItemLevelOfDensity = d.POItemLevelOfDensity
                                       }
                                 ).ToList()

            End If
            result = book.ProcessObjectData(lOrderHeaders, lOrderDetails, Utilities.GetConnectionString())
            ReturnMessage = book.LastError
            Utilities.LogResults("BookObject.ProcessData60", result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("BookObject.ProcessData60 Failure", result, "Cannot process book data.  ", ex, AuthorizationCode, "Process Book Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function


    ''' <summary>
    ''' Add functionality to save records in clsBook
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="OrderHeaders"></param>
    ''' <param name="OrderDetails"></param>
    ''' <param name="OrderSeals"></param>
    ''' <param name="OrderTrailer"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function ProcessData70WithSeals(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject70,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject70,
            ByVal OrderSeals() As Ngl.FreightMaster.Integration.clsBookSeal,
            ByVal OrderTrailer() As Ngl.FreightMaster.Integration.clsBookTrailer,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            'If OrderHeaders.Length = 0 Then
            '    ReturnMessage = "Empty Header"
            '    Utilities.LogResults("BookObject.ProcessData60", 0, ReturnMessage, AuthorizationCode)
            '    result = 0
            '    Return 0
            'End If
            'Dim book As New Ngl.FreightMaster.Integration.clsBook
            'If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            'Utilities.populateIntegrationObjectParameters(book)
            'book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            'book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            'result = book.ProcessObjectData(OrderHeaders.ToList, OrderDetails.ToList, Utilities.GetConnectionString())
            'ReturnMessage = book.LastError
            Utilities.LogResults("BookObject.ProcessData70", result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("BookObject.ProcessData70 Failure", result, "Cannot process book data.  ", ex, AuthorizationCode, "Process Book Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

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
    Public Function ProcessData70(ByVal AuthorizationCode As String,
            ByVal OrderHeaders() As Ngl.FreightMaster.Integration.clsBookHeaderObject70,
            ByVal OrderDetails() As Ngl.FreightMaster.Integration.clsBookDetailObject70,
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            If OrderHeaders Is Nothing OrElse OrderHeaders.Length = 0 Then
                ReturnMessage = "Empty Header"
                Utilities.LogResults("BookObject.ProcessData70", 0, ReturnMessage, AuthorizationCode)
                result = 0
                Return 0
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
            Utilities.LogResults("BookObject.ProcessData70", result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("BookObject.ProcessData70 Failure", result, "Cannot process book data.  ", ex, AuthorizationCode, "Process Book Data Failure")
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
    Public Function GetAllRequiredDateChangesByCompNumber(ByVal AuthorizationCode As String,
                                  ByVal CompNumber As Integer,
                                  ByRef RetVal As Integer,
                                  ByRef ReturnMessage As String) As LTS.spGetTMSModifiedRequiredDateResult()
        Dim sProcedureName As String = "Get All Required Date Changes"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using company # " & CompNumber.ToString & ".  "
        Dim oData As New List(Of LTS.spGetTMSModifiedRequiredDateResult)
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try

            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code."
                Return oData.ToArray()
            End If
            Dim WCFParameters As New DAL.WCFParameters With
                {
                .ConnectionString = Utilities.GetConnectionString(),
                .DBServer = Utilities.GetServerName(Utilities.GetConnectionString()),
                .Database = Utilities.GetDatabase(Utilities.GetConnectionString()),
                .WCFAuthCode = "NGLSystem",
                .ValidateAccess = False,
                .UserName = ""
                }
            Dim BookDataObject As New DAL.NGLBookData(WCFParameters)
            oData = BookDataObject.GetTMSModifiedRequiredDate(Nothing, CompNumber)
            RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            ReturnMessage = "Success!"
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("BookObject.GetAllRequiredDateChanges Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData.ToArray()
    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetAllRequiredDateChangesByCompAlphaCode(ByVal AuthorizationCode As String,
                                  ByVal CompAlphaCode As String,
                                  ByRef RetVal As Integer,
                                  ByRef ReturnMessage As String) As LTS.spGetTMSModifiedRequiredDateResult()
        Dim sProcedureName As String = "Get All Required Date Changes By Comp Alpha Code"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using " & CompAlphaCode & ".  "
        Dim oData As New List(Of LTS.spGetTMSModifiedRequiredDateResult)
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try

            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code."
                Return oData.ToArray()
            End If
            Dim WCFParameters As New DAL.WCFParameters With
                {
                .ConnectionString = Utilities.GetConnectionString(),
                .DBServer = Utilities.GetServerName(Utilities.GetConnectionString()),
                .Database = Utilities.GetDatabase(Utilities.GetConnectionString()),
                .WCFAuthCode = "NGLSystem",
                .ValidateAccess = False,
                .UserName = ""
                }
            Dim CompNumber As Integer = 0
            Dim CompDataObject As New DAL.NGLCompData(WCFParameters)
            CompNumber = CompDataObject.GetCompNumberByAlpha(CompAlphaCode)
            Dim BookDataObject As New DAL.NGLBookData(WCFParameters)
            oData = BookDataObject.GetTMSModifiedRequiredDate(Nothing, CompNumber)

            RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            ReturnMessage = "Success!"
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("BookObject.GetAllRequiredDateChangesByCompAlphaCode Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData.ToArray()
    End Function


    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetRequiredDateChangesByOrderNumber(ByVal AuthorizationCode As String,
                                  ByVal OrderNumber As String,
                                  ByRef RetVal As Integer,
                                  ByRef ReturnMessage As String) As LTS.spGetTMSModifiedRequiredDateResult()
        Dim sProcedureName As String = "Get Required Date Changes By Order Number"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using " & OrderNumber & ".  "
        Dim oData As New List(Of LTS.spGetTMSModifiedRequiredDateResult)
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try

            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code."
                Return oData.ToArray()
            End If
            Dim WCFParameters As New DAL.WCFParameters With
                {
                .ConnectionString = Utilities.GetConnectionString(),
                .DBServer = Utilities.GetServerName(Utilities.GetConnectionString()),
                .Database = Utilities.GetDatabase(Utilities.GetConnectionString()),
                .WCFAuthCode = "NGLSystem",
                .ValidateAccess = False,
                .UserName = ""
                }
            Dim BookDataObject As New DAL.NGLBookData(WCFParameters)
            oData = BookDataObject.GetTMSModifiedRequiredDate(OrderNumber, 0)

            RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            ReturnMessage = "Success!"
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("BookObject.GetRequiredDateChangesByOrderNumber Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData.ToArray()
    End Function

End Class