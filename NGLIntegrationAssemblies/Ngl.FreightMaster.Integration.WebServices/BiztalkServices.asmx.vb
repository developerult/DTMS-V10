Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class BiztalkServices
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""
    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function ProcessOrder(ByVal AuthorizationCode As String, _
                                ByVal OrderHeaderTableXml As System.Xml.XmlDocument, _
                                ByVal OrderDetailTableXml As System.Xml.XmlDocument) As Integer
        Dim s As String = ""
        Return ProcessOrderEx(AuthorizationCode, OrderHeaderTableXml, OrderDetailTableXml, s)
    End Function

    <WebMethod()> _
        Public Function ProcessOrderEx(ByVal AuthorizationCode As String, _
                                    ByVal OrderHeaderTableXml As System.Xml.XmlDocument, _
                                    ByVal OrderDetailTableXml As System.Xml.XmlDocument, _
                                    ByRef ReturnMessage As String) As Integer

        'Dim finalResult As Integer = 0
        'Dim companyIds As New List(Of String)
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim book As New Ngl.FreightMaster.Integration.WebServices.Book
            Dim OrderHeaderTable As New Ngl.FreightMaster.Integration.BookData.BookHeaderDataTable
            Dim OrderDetailTable As New Ngl.FreightMaster.Integration.BookData.BookDetailDataTable
            CreateDataTable(OrderHeaderTableXml, OrderHeaderTable, "ns0:BookHeader")
            CreateDataTable(OrderDetailTableXml, OrderDetailTable, "ns0:BookDetail")
            'For Each oDetail As Ngl.FreightMaster.Integration.BookData.BookDetailRow In OrderDetailTable
            '    Utilities.LogMessage("Detail PO Number: ", oDetail.ItemPONumber)
            '    Utilities.LogMessage("Detail Item Number: ", oDetail.ItemNumber)
            '    Utilities.LogMessage("Detail Lot Number: ", oDetail.LotNumber)
            '    Utilities.LogMessage("Detail Description: ", oDetail.Description)
            '    Utilities.LogMessage("-----------------", "-----------------")
            'Next
            result = book.ProcessDataEx(AuthorizationCode, OrderHeaderTable, OrderDetailTable, ReturnMessage)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("BiztalkServices.ProcessOrderEx", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result
        '******************  Code Removed By RHR 11/18/2008 ************************ ***************
        'All business logic has been moved to the BLL component NGL.FreightMaster.Integration.clsBook
        'For Each orderRow As DataRow In OrderHeaderTable.Rows
        '    Dim dtHeader As New Ngl.FreightMaster.Integration.Book.BookHeaderDataTable
        '    Dim dtDetail As New Ngl.FreightMaster.Integration.Book.BookDetailDataTable
        '    Dim newHeader As Ngl.FreightMaster.Integration.Book.BookHeaderRow = dtHeader.NewBookHeaderRow
        '    For i As Integer = 0 To OrderHeaderTable.Columns.Count - 1
        '        newHeader(i) = orderRow(i)
        '    Next
        '    If companyIds.Contains(newHeader.PODefaultCustomer) = False Then companyIds.Add(newHeader.PODefaultCustomer)
        '    dtHeader.Rows.Add(newHeader)
        '    Dim detailsRows As Ngl.FreightMaster.Integration.BookData.BookDetailRow()
        '    detailsRows = OrderDetailTable.Select("ItemPONumber = '" & newHeader.PONumber & "'")
        '    For Each detailrow As DataRow In detailsRows
        '        Dim newDetail As Ngl.FreightMaster.Integration.BookData.BookDetailRow = dtDetail.NewBookDetailRow
        '        For i As Integer = 0 To dtDetail.Columns.Count - 1
        '            newDetail(i) = detailrow(i)
        '        Next
        '        dtDetail.Rows.Add(newDetail)
        '    Next
        '    Dim result As Integer = book.ProcessData(AuthorizationCode, dtHeader, dtDetail)
        '    If result <> 0 Then finalResult = result
        'Next

        'Dim mailTo As String = Utilities.GetSetting(AuthorizationCode, "OrderNotification")
        'Dim body As String = "An order import just occured which included orders for the following company numbers: "

        'For Each companyId As String In companyIds
        '    body &= companyId & ", "
        'Next

        'Utilities.SendNotification(AuthorizationCode, mailTo, "", "Orders Imported", body)

        'Return finalResult

    End Function

    <WebMethod()> _
    Public Function ProcessCarrier(ByVal AuthorizationCode As String, _
                                    ByVal CarrierHeaderTableXml As System.Xml.XmlDocument, _
                                    ByVal CarrierContactTableXml As System.Xml.XmlDocument) As Integer
        Dim s As String = ""
        Return ProcessCarrierEx(AuthorizationCode, CarrierHeaderTableXml, CarrierContactTableXml, s)
    End Function

    <WebMethod()> _
    Public Function ProcessCarrierEx(ByVal AuthorizationCode As String, _
                                    ByVal CarrierHeaderTableXml As System.Xml.XmlDocument, _
                                    ByVal CarrierContactTableXml As System.Xml.XmlDocument, _
                                    ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim carrier As New Ngl.FreightMaster.Integration.WebServices.Carrier
            Dim CarrierHeaderTable As New Ngl.FreightMaster.Integration.CarrierData.CarrierHeaderDataTable
            Dim CarrierContactTable As New Ngl.FreightMaster.Integration.CarrierData.CarrierContactDataTable
            CreateDataTable(CarrierHeaderTableXml, CarrierHeaderTable)
            CreateDataTable(CarrierContactTableXml, CarrierContactTable)
            result = carrier.ProcessDataEx(AuthorizationCode, CarrierHeaderTable, CarrierContactTable, ReturnMessage)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("BiztalkServices.ProcessCarrierEx", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    <WebMethod()> _
    Public Function ProcessCompany(ByVal AuthorizationCode As String, _
        ByVal CompanyHeaderTableXml As System.Xml.XmlDocument, _
        ByVal CompanyContactsTableXml As System.Xml.XmlDocument) As Integer
        Dim s As String = ""
        Return ProcessCompanyEx(AuthorizationCode, CompanyHeaderTableXml, CompanyContactsTableXml, s)
    End Function

    <WebMethod()> _
    Public Function ProcessCompanyEx(ByVal AuthorizationCode As String, _
        ByVal CompanyHeaderTableXml As System.Xml.XmlDocument, _
        ByVal CompanyContactsTableXml As System.Xml.XmlDocument, _
        ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim company As New Ngl.FreightMaster.Integration.WebServices.Company
            Dim CompanyHeaderTable As New Ngl.FreightMaster.Integration.CompanyData.CompanyHeaderDataTable
            Dim CompanyContactsTable As New Ngl.FreightMaster.Integration.CompanyData.CompanyContactDataTable
            CreateDataTable(CompanyHeaderTableXml, CompanyHeaderTable)
            CreateDataTable(CompanyContactsTableXml, CompanyContactsTable)
            result = company.ProcessDataEx(AuthorizationCode, CompanyHeaderTable, CompanyContactsTable, ReturnMessage)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("BiztalkServices.ProcessCompanyEx", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result


    End Function

    <WebMethod()> _
    Public Function ProcessLane(ByVal AuthorizationCode As String, _
            ByVal LaneTableXml As System.Xml.XmlDocument) As Integer
        Dim s As String = ""
        Return ProcessLaneEx(AuthorizationCode, LaneTableXml, s)
    End Function

    <WebMethod()> _
    Public Function ProcessLaneEx(ByVal AuthorizationCode As String, _
            ByVal LaneTableXml As System.Xml.XmlDocument, _
            ByRef ReturnMessage As String) As Integer


        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim lane As New Ngl.FreightMaster.Integration.WebServices.Lane
            Dim LaneTable As New Ngl.FreightMaster.Integration.LaneData.LaneHeaderDataTable
            CreateDataTable(LaneTableXml, LaneTable)
            result = lane.ProcessDataEx(AuthorizationCode, LaneTable, ReturnMessage)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("BiztalkServices.ProcessLaneEx", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

        '************************************************************************************
        'Code Removed By RHR 10/30/2008  We now pass the entire datatable to the web service.
        'Dim sw As New IO.StreamWriter("\\ngliis02p\biztalkwsroot\vgi\lane.xml")
        'sw.Write(LaneTableXml.OuterXml)
        'sw.Close()
        'Return 0
        'For Each laneRow As DataRow In LaneTable.Rows
        '    Dim dtLane As New Ngl.FreightMaster.Integration.LaneData.LaneHeaderDataTable
        '    Dim newRow As Ngl.FreightMaster.Integration.LaneData.LaneHeaderRow = dtLane.NewLaneHeaderRow
        '    For i As Integer = 0 To LaneTable.Columns.Count - 1
        '        newRow(i) = laneRow(i)
        '    Next
        '    dtLane.Rows.Add(newRow)
        '    Dim result As Integer = lane.ProcessData(AuthorizationCode, dtLane)
        '    Dim strSpacer As String = ""
        '    If result <> 0 Then
        '        finalResult = result
        '        mstrLastError = mstrLastError & strSpacer & lane.LastError
        '        strSpacer = vbCrLf
        '    End If

        'Next
        '*************************************************************************************



    End Function

    <WebMethod()> _
    Public Function ProcessPayable(ByVal AuthorizationCode As String, _
        ByVal PayablesTableXml As System.Xml.XmlDocument) As Integer
        Dim s As String = ""
        Return ProcessPayableEx(AuthorizationCode, PayablesTableXml, s)
    End Function

    <WebMethod()> _
    Public Function ProcessPayableEx(ByVal AuthorizationCode As String, _
                ByVal PayablesTableXml As System.Xml.XmlDocument, _
                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim payables As New Ngl.FreightMaster.Integration.WebServices.Payables
            Dim PayablesTable As New Ngl.FreightMaster.Integration.PayablesData.PayablesHeaderDataTable
            CreateDataTable(PayablesTableXml, PayablesTable)
            result = payables.ProcessDataEx(AuthorizationCode, PayablesTable, ReturnMessage)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("BiztalkServices.ProcessPayableEx", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function


    <WebMethod()> _
        Public Function ProcessSchedule(ByVal AuthorizationCode As String, _
        ByVal ScheduleTableXml As System.Xml.XmlDocument) As Integer
        Dim s As String = ""
        Return ProcessScheduleEx(AuthorizationCode, ScheduleTableXml, s)
    End Function


    <WebMethod()> _
        Public Function ProcessScheduleEx(ByVal AuthorizationCode As String, _
                ByVal ScheduleTableXml As System.Xml.XmlDocument, _
                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim schedule As New Ngl.FreightMaster.Integration.WebServices.Schedule
            Dim ScheduleTable As New Ngl.FreightMaster.Integration.ScheduleData.ScheduleHeaderDataTable
            CreateDataTable(ScheduleTableXml, ScheduleTable)
            result = schedule.ProcessDataEx(AuthorizationCode, ScheduleTable, ReturnMessage)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("BiztalkServices.ProcessScheduleEx", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result


    End Function

    <WebMethod()> _
        Public Function ProcessFreightBill(ByVal AuthorizationCode As String, _
        ByVal FreightBillTableXml As System.Xml.XmlDocument) As Integer
        Dim s As String = ""
        Return ProcessFreightBillEx(AuthorizationCode, FreightBillTableXml, s)
    End Function

    <WebMethod()> _
        Public Function ProcessFreightBillEx(ByVal AuthorizationCode As String, _
                ByVal FreightBillTableXml As System.Xml.XmlDocument, _
                ByRef ReturnMessage As String) As Integer

        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim FreightBill As New Ngl.FreightMaster.Integration.WebServices.FreightBill
            Dim FreightBillTable As New Ngl.FreightMaster.Integration.FreightBillData.FreightBillDataTable
            CreateDataTable(FreightBillTableXml, FreightBillTable)
            result = FreightBill.ProcessDataEx(AuthorizationCode, FreightBillTable, ReturnMessage)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("BiztalkServices.ProcessFreightBillEx", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result

    End Function

    <WebMethod()> _
        Public Function ProcessLegacyFreightBill(ByVal AuthorizationCode As String, _
        ByVal LegacyFreightBillTableXml As System.Xml.XmlDocument) As Integer
        Dim s As String = ""
        Return ProcessLegacyFreightBillEx(AuthorizationCode, LegacyFreightBillTableXml, s)
    End Function

    <WebMethod()> _
        Public Function ProcessLegacyFreightBillEx(ByVal AuthorizationCode As String, _
                ByVal LegacyFreightBillTableXml As System.Xml.XmlDocument, _
                ByRef ReturnMessage As String) As Integer
        Dim result As Integer = 3
        ReturnMessage = ""
        Try
            Dim LegacyFreightBill As New Ngl.FreightMaster.Integration.WebServices.LegacyLegacyFreightBill
            Dim LegacyFreightBillTable As New Ngl.FreightMaster.Integration.LegacyFreightBillData.LegacyFreightBillDataTable
            CreateDataTable(LegacyFreightBillTableXml, LegacyFreightBillTable)
            result = LegacyFreightBill.ProcessDataEx(AuthorizationCode, LegacyFreightBillTable, ReturnMessage)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults("BiztalkServices.ProcessLegacyFreightBillEx", 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result


    End Function

    Private Function CreateDataTable(ByVal xml As System.Xml.XmlDocument, ByRef dt As System.Data.DataTable) As DataTable

        For Each node As XmlNode In xml.DocumentElement.ChildNodes
            Dim row As DataRow = dt.NewRow
            For Each child As XmlNode In node.ChildNodes
                Dim elementName As String = child.Name
                If elementName.Contains(":") Then elementName = elementName.Substring(elementName.IndexOf(":") + 1)
                If child.InnerText <> "" Then row(elementName) = child.InnerText
            Next
            dt.Rows.Add(row)
        Next

        Return dt

    End Function

    Private Function CreateDataTable(ByVal xml As System.Xml.XmlDocument, ByRef dt As System.Data.DataTable, ByVal tagName As String) As DataTable

        For Each node As XmlNode In xml.GetElementsByTagName(tagName)
            Dim row As DataRow = dt.NewRow
            For Each child As XmlNode In node.ChildNodes
                Dim elementName As String = child.Name
                If elementName.Contains(":") Then elementName = elementName.Substring(elementName.IndexOf(":") + 1)
                If child.InnerText <> "" Then row(elementName) = child.InnerText
            Next
            dt.Rows.Add(row)
        Next

        Return dt

    End Function

    '<WebMethod()> _
    'Public Function Test() As Integer

    '    Dim xmlHeader As New XmlDocument()
    '    xmlHeader.Load("D:\Development\Steve Buttitta\Header.xml")
    '    Dim xmlDetail As New XmlDocument()
    '    xmlDetail.Load("D:\Development\Steve Buttitta\Detail.xml")
    '    Return ProcessOrder("Gonnella_6FF105EBA1304BCBB7910E56E7844742_", xmlHeader, xmlDetail)

    'End Function

    'Export
    <WebMethod()> _
    Public Function GetApExportData(ByVal AuthorizationCode As String, _
                                    ByVal MaxRetries As Integer, _
                                    ByVal RetryMinutes As Integer, _
                                    ByVal CompanyNumber As String, _
                                    ByVal OrderNumber As String, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As System.Xml.XmlDocument

        Dim apExport As New Ngl.FreightMaster.Integration.WebServices.ApExportData
        Dim xmlDoc As System.Xml.XmlDocument = GetXmlDoc(GetType(Ngl.FreightMaster.Integration.APExport.APExportDataDataTable), _
            apExport.GetData(AuthorizationCode, MaxRetries, RetryMinutes, CompanyNumber, OrderNumber, WSResult, LastError))
        mstrLastError = LastError
        Return xmlDoc

    End Function

    <WebMethod()> _
    Public Function GetApExportDataEx(ByVal AuthorizationCode As String, _
                                    ByVal MaxRetries As Integer, _
                                    ByVal RetryMinutes As Integer, _
                                    ByVal CompanyNumber As String, _
                                    ByVal OrderNumber As String, _
                                    ByVal MaxRowsReturned As Integer, _
                                    ByVal AutoConfirmation As Boolean, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As System.Xml.XmlDocument

        Dim apExport As New Ngl.FreightMaster.Integration.WebServices.ApExportData
        Dim xmlDoc As System.Xml.XmlDocument = GetXmlDoc(GetType(Ngl.FreightMaster.Integration.APExport.APExportDataDataTable), _
            apExport.GetDataEx(AuthorizationCode, MaxRetries, RetryMinutes, CompanyNumber, OrderNumber, MaxRowsReturned, AutoConfirmation, WSResult, LastError))
        mstrLastError = LastError
        Return xmlDoc

    End Function

    <WebMethod()> _
    Public Function GetAPExportOpenPayablesEx(ByVal AuthorizationCode As String, _
                                            ByVal CompanyNumber As String, _
                                            ByVal MaxRowsReturned As Integer, _
                                            ByRef WSResult As Integer, _
                                            ByRef LastError As String) As XmlDocument

        Dim apExport As New Ngl.FreightMaster.Integration.WebServices.ApExportData
        Dim xmlDoc As System.Xml.XmlDocument = GetXmlDoc(GetType(Ngl.FreightMaster.Integration.PayablesData.PayablesHeaderDataTable), _
            apExport.GetOpenPayablesEx(AuthorizationCode, CompanyNumber, MaxRowsReturned, WSResult, LastError))
        mstrLastError = LastError
        Return xmlDoc

    End Function

    <WebMethod()> _
    Public Function ConfirmApExport(ByVal AuthorizationCode As String, ByVal BookCarrOrderNumber As String, _
        ByVal BookFinAPBillNumber As String, ByVal BookProNumber As String) As Boolean

        Dim apExport As New Ngl.FreightMaster.Integration.WebServices.ApExportData
        Return apExport.ConfirmExport(AuthorizationCode, BookCarrOrderNumber, BookFinAPBillNumber, BookProNumber)

    End Function

    <WebMethod()> _
    Public Function GetApFreightBillData(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByVal FreightBillNumber As String, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As XmlDocument

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.WebServices.ApFreightBillExport
        Dim xmlDoc As System.Xml.XmlDocument = GetXmlDoc(GetType(Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable), _
            apFreightBillExport.GetData(AuthorizationCode, 3, 15, CompanyNumber, FreightBillNumber, WSResult, LastError))
        mstrLastError = LastError
        Return xmlDoc
    End Function

    <WebMethod()> _
    Public Function GetApFreightBillDataEx(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByVal FreightBillNumber As String, _
                                    ByVal MaxRowsReturned As Integer, _
                                    ByVal AutoConfirmation As Boolean, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As XmlDocument

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.WebServices.ApFreightBillExport
        Dim xmlDoc As System.Xml.XmlDocument = GetXmlDoc(GetType(Ngl.FreightMaster.Integration.APFreightBill.APFreightBillDataDataTable), _
            apFreightBillExport.GetDataEx(AuthorizationCode, 3, 15, CompanyNumber, FreightBillNumber, MaxRowsReturned, AutoConfirmation, WSResult, LastError))
        mstrLastError = LastError
        Return xmlDoc
    End Function

    <WebMethod()> _
    Public Function ConfirmApFreightBillExport(ByVal AuthorizationCode As String, ByVal BookFinAPBillNumber As String) As Boolean

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.WebServices.ApFreightBillExport
        Return apFreightBillExport.ConfirmExport(AuthorizationCode, BookFinAPBillNumber)

    End Function

    <WebMethod()> _
    Public Function GetOpenPayables(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As XmlDocument

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.WebServices.ApFreightBillExport
        Dim xmlDoc As System.Xml.XmlDocument = GetXmlDoc(GetType(Ngl.FreightMaster.Integration.PayablesData.PayablesHeaderDataTable), _
            apFreightBillExport.GetOpenPayables(AuthorizationCode, CompanyNumber, WSResult, LastError))
        mstrLastError = LastError
        Return xmlDoc
    End Function

    <WebMethod()> _
    Public Function GetAPFreightBillOpenPayablesEx(ByVal AuthorizationCode As String, _
                                    ByVal CompanyNumber As String, _
                                    ByVal MaxRowsReturned As Integer, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As XmlDocument

        Dim apFreightBillExport As New Ngl.FreightMaster.Integration.WebServices.ApFreightBillExport
        Dim xmlDoc As System.Xml.XmlDocument = GetXmlDoc(GetType(Ngl.FreightMaster.Integration.PayablesData.PayablesHeaderDataTable), _
            apFreightBillExport.GetOpenPayablesEx(AuthorizationCode, CompanyNumber, MaxRowsReturned, WSResult, LastError))
        mstrLastError = LastError
        Return xmlDoc
    End Function

    <WebMethod()> _
    Public Function GetPicklistData(ByVal AuthorizationCode As String, _
                                    ByVal MaxRetry As Integer, _
                                    ByVal RetryMinutes As Integer, _
                                    ByVal CompanyNumber As String, _
                                    ByVal OrderNumber As String, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As XmlDocument

        Dim picklist As New Ngl.FreightMaster.Integration.WebServices.Picklist
        Dim ds As Ngl.FreightMaster.Integration.PickListData = picklist.GetData(AuthorizationCode, MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, WSResult, LastError)
        mstrLastError = LastError
        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(ds.GetXml)
        Return xmlDoc
    End Function

    <WebMethod()> _
    Public Function GetPicklistDataEx(ByVal AuthorizationCode As String, _
                                    ByVal MaxRetry As Integer, _
                                    ByVal RetryMinutes As Integer, _
                                    ByVal CompanyNumber As String, _
                                    ByVal OrderNumber As String, _
                                    ByVal MaxRowsReturned As Integer, _
                                    ByVal AutoConfirmation As Boolean, _
                                    ByRef WSResult As Integer, _
                                    ByRef LastError As String) As XmlDocument
        Dim picklist As New Ngl.FreightMaster.Integration.WebServices.Picklist
        Dim ds As Ngl.FreightMaster.Integration.PickListData = picklist.GetDataEx(AuthorizationCode, MaxRetry, RetryMinutes, CompanyNumber, OrderNumber, MaxRowsReturned, AutoConfirmation, WSResult, LastError)
        mstrLastError = LastError
        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(ds.GetXml)
        Return xmlDoc
    End Function

    <WebMethod()> _
    Public Function ConfirmPicklistExport(ByVal AuthorizationCode As String, ByVal PlControl As String, _
        ByVal BookCarrOrderNumber As String, ByVal CompanyNumber As String) As Boolean

        Dim picklist As New Ngl.FreightMaster.Integration.WebServices.Picklist
        Return picklist.ConfirmExport(AuthorizationCode, PlControl, BookCarrOrderNumber, CompanyNumber)

    End Function

    Private Function GetXmlDoc(ByVal type As System.Type, ByVal instance As Object) As XmlDocument

        Dim serializer As New System.Xml.Serialization.XmlSerializer(type)
        Dim stream As New System.IO.MemoryStream()
        serializer.Serialize(stream, instance)
        stream.Position = 0

        Dim reader As New System.IO.StreamReader(stream)
        Dim xmlDoc As New System.Xml.XmlDocument
        xmlDoc.LoadXml(reader.ReadToEnd)
        reader.Close()
        Return xmlDoc

    End Function



End Class