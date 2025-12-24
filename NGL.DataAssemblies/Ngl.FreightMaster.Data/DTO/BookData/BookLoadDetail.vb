Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookLoadDetail
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookControl As Integer = 0
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _BookProNumber As String = ""
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return Left(_BookProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookProBase As String = ""
        <DataMember()> _
        Public Property BookProBase() As String
            Get
                Return Left(_BookProBase, 50)
            End Get
            Set(ByVal value As String)
                _BookProBase = Left(value, 50)
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Left(_BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _BookCustCompControl As Integer = 0
        <DataMember()> _
        Public Property BookCustCompControl() As Integer
            Get
                Return _BookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCustCompControl = value
            End Set
        End Property

        Private _BookCommCompControl As Integer = 0
        <DataMember()> _
        Public Property BookCommCompControl() As Integer
            Get
                Return _BookCommCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCommCompControl = value
            End Set
        End Property

        Private _BookODControl As Integer = 0
        <DataMember()> _
        Public Property BookODControl() As Integer
            Get
                Return _BookODControl
            End Get
            Set(ByVal value As Integer)
                _BookODControl = value
            End Set
        End Property

        Private _BookCarrierControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrierControl() As Integer
            Get
                Return _BookCarrierControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrierControl = value
            End Set
        End Property

        Private _BookCarrierContact As String = ""
        <DataMember()> _
        Public Property BookCarrierContact() As String
            Get
                Return Left(_BookCarrierContact, 30)
            End Get
            Set(ByVal value As String)
                _BookCarrierContact = Left(value, 30)
            End Set
        End Property

        Private _BookCarrierContactPhone As String = ""
        <DataMember()> _
        Public Property BookCarrierContactPhone() As String
            Get
                Return Left(_BookCarrierContactPhone, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrierContactPhone = Left(value, 20)
            End Set
        End Property

        Private _BookOrigCompControl As Integer = 0
        <DataMember()> _
        Public Property BookOrigCompControl() As Integer
            Get
                Return _BookOrigCompControl
            End Get
            Set(ByVal value As Integer)
                _BookOrigCompControl = value
            End Set
        End Property

        Private _BookOrigName As String = ""
        <DataMember()> _
        Public Property BookOrigName() As String
            Get
                Return Left(_BookOrigName, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigName = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress1 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress1() As String
            Get
                Return Left(_BookOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress2 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress2() As String
            Get
                Return Left(_BookOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress3 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress3() As String
            Get
                Return Left(_BookOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress3 = Left(value, 40)
            End Set
        End Property

        Private _BookOrigCity As String = ""
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return Left(_BookOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _BookOrigCity = Left(value, 25)
            End Set
        End Property

        Private _BookOrigState As String = ""
        <DataMember()> _
        Public Property BookOrigState() As String
            Get
                Return Left(_BookOrigState, 8)
            End Get
            Set(ByVal value As String)
                _BookOrigState = Left(value, 8)
            End Set
        End Property

        Private _BookOrigCountry As String = ""
        <DataMember()> _
        Public Property BookOrigCountry() As String
            Get
                Return Left(_BookOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _BookOrigZip As String = ""
        <DataMember()> _
        Public Property BookOrigZip() As String
            Get
                Return Left(_BookOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookOrigPhone As String = ""
        <DataMember()> _
        Public Property BookOrigPhone() As String
            Get
                Return Left(_BookOrigPhone, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookOrigPhone = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookOrigFax As String = ""
        <DataMember()> _
        Public Property BookOrigFax() As String
            Get
                Return Left(_BookOrigFax, 15)
            End Get
            Set(ByVal value As String)
                _BookOrigFax = Left(value, 15)
            End Set
        End Property

        Private _BookOriginStartHrs As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookOriginStartHrs() As System.Nullable(Of Date)
            Get
                Return _BookOriginStartHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookOriginStartHrs = value
            End Set
        End Property

        Private _BookOriginStopHrs As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookOriginStopHrs() As System.Nullable(Of Date)
            Get
                Return _BookOriginStopHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookOriginStopHrs = value
            End Set
        End Property

        Private _BookOriginApptReq As Boolean = False
        <DataMember()> _
        Public Property BookOriginApptReq() As Boolean
            Get
                Return _BookOriginApptReq
            End Get
            Set(ByVal value As Boolean)
                _BookOriginApptReq = value
            End Set
        End Property

        Private _BookDestCompControl As Integer = 0
        <DataMember()> _
        Public Property BookDestCompControl() As Integer
            Get
                Return _BookDestCompControl
            End Get
            Set(ByVal value As Integer)
                _BookDestCompControl = value
            End Set
        End Property

        Private _BookDestName As String = ""
        <DataMember()> _
        Public Property BookDestName() As String
            Get
                Return Left(_BookDestName, 40)
            End Get
            Set(ByVal value As String)
                _BookDestName = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress1 As String = ""
        <DataMember()> _
        Public Property BookDestAddress1() As String
            Get
                Return Left(_BookDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress2 As String = ""
        <DataMember()> _
        Public Property BookDestAddress2() As String
            Get
                Return Left(_BookDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress3 As String = ""
        <DataMember()> _
        Public Property BookDestAddress3() As String
            Get
                Return Left(_BookDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress3 = Left(value, 40)
            End Set
        End Property

        Private _BookDestCity As String = ""
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return Left(_BookDestCity, 25)
            End Get
            Set(ByVal value As String)
                _BookDestCity = Left(value, 25)
            End Set
        End Property

        Private _BookDestState As String = ""
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return Left(_BookDestState, 2)
            End Get
            Set(ByVal value As String)
                _BookDestState = Left(value, 2)
            End Set
        End Property

        Private _BookDestCountry As String = ""
        <DataMember()> _
        Public Property BookDestCountry() As String
            Get
                Return Left(_BookDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookDestCountry = Left(value, 30)
            End Set
        End Property

        Private _BookDestZip As String = ""
        <DataMember()> _
        Public Property BookDestZip() As String
            Get
                Return Left(_BookDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookDestPhone As String = ""
        <DataMember()> _
        Public Property BookDestPhone() As String
            Get
                Return Left(_BookDestPhone, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookDestPhone = Left(value, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookDestFax As String = ""
        <DataMember()> _
        Public Property BookDestFax() As String
            Get
                Return Left(_BookDestFax, 15)
            End Get
            Set(ByVal value As String)
                _BookDestFax = Left(value, 15)
            End Set
        End Property

        Private _BookDestStartHrs As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDestStartHrs() As System.Nullable(Of Date)
            Get
                Return _BookDestStartHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDestStartHrs = value
            End Set
        End Property

        Private _BookDestStopHrs As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDestStopHrs() As System.Nullable(Of Date)
            Get
                Return _BookDestStopHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDestStopHrs = value
            End Set
        End Property

        Private _BookDestApptReq As Boolean = False
        <DataMember()> _
        Public Property BookDestApptReq() As Boolean
            Get
                Return _BookDestApptReq
            End Get
            Set(ByVal value As Boolean)
                _BookDestApptReq = value
            End Set
        End Property

        Private _BookDateOrdered As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateOrdered() As System.Nullable(Of Date)
            Get
                Return _BookDateOrdered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateOrdered = value
            End Set
        End Property

        Private _BookDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateLoad() As System.Nullable(Of Date)
            Get
                Return _BookDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateLoad = value
            End Set
        End Property

        Private _BookDateInvoice As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateInvoice() As System.Nullable(Of Date)
            Get
                Return _BookDateInvoice
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateInvoice = value
            End Set
        End Property

        Private _BookFinARInvoiceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinARInvoiceDate() As System.Nullable(Of Date)
            Get
                Return _BookFinARInvoiceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinARInvoiceDate = value
            End Set
        End Property

        Private _BookDateRequired As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateRequired() As System.Nullable(Of Date)
            Get
                Return _BookDateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateRequired = value
            End Set
        End Property

        Private _BookDateDelivered As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateDelivered() As System.Nullable(Of Date)
            Get
                Return _BookDateDelivered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateDelivered = value
            End Set
        End Property

        Private _BookTotalCases As Integer = 0
        <DataMember()> _
        Public Property BookTotalCases() As Integer
            Get
                Return _BookTotalCases
            End Get
            Set(ByVal value As Integer)
                _BookTotalCases = value
            End Set
        End Property

        Private _BookTotalWgt As Double = 0
        <DataMember()> _
        Public Property BookTotalWgt() As Double
            Get
                Return _BookTotalWgt
            End Get
            Set(ByVal value As Double)
                _BookTotalWgt = value
            End Set
        End Property

        Private _BookTotalPL As Double = 0
        <DataMember()> _
        Public Property BookTotalPL() As Double
            Get
                Return _BookTotalPL
            End Get
            Set(ByVal value As Double)
                _BookTotalPL = value
            End Set
        End Property

        Private _BookTotalCube As Integer = 0
        <DataMember()> _
        Public Property BookTotalCube() As Integer
            Get
                Return _BookTotalCube
            End Get
            Set(ByVal value As Integer)
                _BookTotalCube = value
            End Set
        End Property

        Private _BookTotalPX As Integer = 0
        <DataMember()> _
        Public Property BookTotalPX() As Integer
            Get
                Return _BookTotalPX
            End Get
            Set(ByVal value As Integer)
                _BookTotalPX = value
            End Set
        End Property

        Private _BookTotalBFC As Decimal = 0
        <DataMember()> _
        Public Property BookTotalBFC() As Decimal
            Get
                Return _BookTotalBFC
            End Get
            Set(ByVal value As Decimal)
                _BookTotalBFC = value
            End Set
        End Property

        Private _BookTranCode As String = ""
        <DataMember()> _
        Public Property BookTranCode() As String
            Get
                Return Left(_BookTranCode, 3)
            End Get
            Set(ByVal value As String)
                _BookTranCode = Left(value, 3)
            End Set
        End Property

        Private _BookPayCode As String = ""
        <DataMember()> _
        Public Property BookPayCode() As String
            Get
                Return Left(_BookPayCode, 3)
            End Get
            Set(ByVal value As String)
                _BookPayCode = Left(value, 3)
            End Set
        End Property

        Private _BookTypeCode As String = ""
        <DataMember()> _
        Public Property BookTypeCode() As String
            Get
                Return Left(_BookTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookBOLCode As Boolean = False
        <DataMember()> _
        Public Property BookBOLCode() As Boolean
            Get
                Return _BookBOLCode
            End Get
            Set(ByVal value As Boolean)
                _BookBOLCode = value
            End Set
        End Property

        Private _BookStopNo As Short = 0
        <DataMember()> _
        Public Property BookStopNo() As Short
            Get
                Return _BookStopNo
            End Get
            Set(ByVal value As Short)
                _BookStopNo = value
            End Set
        End Property

        Private _BookModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookModDate() As System.Nullable(Of Date)
            Get
                Return _BookModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookModDate = value
            End Set
        End Property

        Private _BookModUser As String = ""
        <DataMember()> _
        Public Property BookModUser() As String
            Get
                Return Left(_BookModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookModUser = Left(value, 100)
            End Set
        End Property

        Private _BookCarrBLNumber As String = ""
        <DataMember()> _
        Public Property BookCarrBLNumber() As String
            Get
                Return Left(_BookCarrBLNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrBLNumber = Left(value, 20)
            End Set
        End Property

        Private _BookCarrOrderNumber As String = ""
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return Left(_BookCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = Left(value, 20)
            End Set
        End Property

        Private _BookFinAPBillNumber As String = ""
        <DataMember()> _
        Public Property BookFinAPBillNumber() As String
            Get
                Return Left(_BookFinAPBillNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinAPBillNumber = Left(value, 50)
            End Set
        End Property

        Private _BookRevBilledBFC As Decimal = 0
        <DataMember()> _
        Public Property BookRevBilledBFC() As Decimal
            Get
                Return _BookRevBilledBFC
            End Get
            Set(ByVal value As Decimal)
                _BookRevBilledBFC = value
            End Set
        End Property

        Private _BookRevTotalCost As Decimal = 0
        <DataMember()> _
        Public Property BookRevTotalCost() As Decimal
            Get
                Return _BookRevTotalCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevTotalCost = value
            End Set
        End Property

        Private _BookRouteFinalDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookRouteFinalDate() As System.Nullable(Of Date)
            Get
                Return _BookRouteFinalDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookRouteFinalDate = value
            End Set
        End Property

        Private _BookRouteFinalCode As String = ""
        <DataMember()> _
        Public Property BookRouteFinalCode() As String
            Get
                Return Left(_BookRouteFinalCode, 2)
            End Get
            Set(ByVal value As String)
                _BookRouteFinalCode = Left(value, 2)
            End Set
        End Property

        Private _BookRouteFinalFlag As Boolean = False
        <DataMember()> _
        Public Property BookRouteFinalFlag() As Boolean
            Get
                Return _BookRouteFinalFlag
            End Get
            Set(ByVal value As Boolean)
                _BookRouteFinalFlag = value
            End Set
        End Property

        Private _BookWarehouseNumber As String = ""
        <DataMember()> _
        Public Property BookWarehouseNumber() As String
            Get
                Return Left(_BookWarehouseNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookWarehouseNumber = Left(value, 20)
            End Set
        End Property

        Private _BookComCode As String = ""
        <DataMember()> _
        Public Property BookComCode() As String
            Get
                Return Left(_BookComCode, 3)
            End Get
            Set(ByVal value As String)
                _BookComCode = Left(value, 3)
            End Set
        End Property

        Private _BookTransType As String = ""
        <DataMember()> _
        Public Property BookTransType() As String
            Get
                Return Left(_BookTransType, 50)
            End Get
            Set(ByVal value As String)
                _BookTransType = Left(value, 50)
            End Set
        End Property

        Private _BookCarrActDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActDate = value
            End Set
        End Property

        Private _BookCarrActTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrActTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActTime = value
            End Set
        End Property

        Private _BookRouteConsFlag As Boolean = False
        <DataMember()> _
        Public Property BookRouteConsFlag() As Boolean
            Get
                Return _BookRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                _BookRouteConsFlag = value
            End Set
        End Property

        Private _BookHotLoad As Boolean = False
        <DataMember()> _
        Public Property BookHotLoad() As Boolean
            Get
                Return _BookHotLoad
            End Get
            Set(ByVal value As Boolean)
                _BookHotLoad = value
            End Set
        End Property

        Private _BookMilesFrom As Double = 0
        <DataMember()> _
        Public Property BookMilesFrom() As Double
            Get
                Return _BookMilesFrom
            End Get
            Set(ByVal value As Double)
                _BookMilesFrom = value
            End Set
        End Property

        Private _BookCarrierContControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrierContControl() As Integer
            Get
                Return _BookCarrierContControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrierContControl = value
            End Set
        End Property

        Private _BookExportDocCreated As Boolean = False
        <DataMember()> _
        Public Property BookExportDocCreated() As Boolean
            Get
                Return _BookExportDocCreated
            End Get
            Set(ByVal value As Boolean)
                _BookExportDocCreated = value
            End Set
        End Property

        Private _BookDoNotInvoice As Boolean = False
        <DataMember()> _
        Public Property BookDoNotInvoice() As Boolean
            Get
                Return _BookDoNotInvoice
            End Get
            Set(ByVal value As Boolean)
                _BookDoNotInvoice = value
            End Set
        End Property

        Private _BookOrderSequence As Integer = 0
        <DataMember()> _
        Public Property BookOrderSequence() As Integer
            Get
                Return _BookOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookOrderSequence = value
            End Set
        End Property

        Private _BookCarrierEquipmentCodes As String = ""
        <DataMember()> _
        Public Property BookCarrierEquipmentCodes() As String
            Get
                Return Left(_BookCarrierEquipmentCodes, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrierEquipmentCodes = Left(value, 50)
            End Set
        End Property

        Private _BookDateRequested As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateRequested() As System.Nullable(Of Date)
            Get
                Return _BookDateRequested
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateRequested = value
            End Set
        End Property

        Private _BookShipCarrierProNumber As String = ""
        <DataMember()> _
        Public Property BookShipCarrierProNumber() As String
            Get
                Return Left(_BookShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookShipCarrierProNumberRaw As String = ""
        <DataMember()> _
        Public Property BookShipCarrierProNumberRaw() As String
            Get
                Return Left(_BookShipCarrierProNumberRaw, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumberRaw = Left(value, 20)
            End Set
        End Property

        Private _BookShipCarrierProControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookShipCarrierProControl() As System.Nullable(Of Integer)
            Get
                Return _BookShipCarrierProControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookShipCarrierProControl = value
            End Set
        End Property

        Private _BookShipCarrierName As String = ""
        <DataMember()> _
        Public Property BookShipCarrierName() As String
            Get
                Return Left(_BookShipCarrierName, 60)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierName = Left(value, 60)
            End Set
        End Property

        Private _BookShipCarrierNumber As String = ""
        <DataMember()> _
        Public Property BookShipCarrierNumber() As String
            Get
                Return Left(_BookShipCarrierNumber, 80)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierNumber = Left(value, 80)
            End Set
        End Property

        Private _BookAPAdjReasonControl As Integer = 0
        <DataMember()> _
        Public Property BookAPAdjReasonControl() As Integer
            Get
                Return _BookAPAdjReasonControl
            End Get
            Set(ByVal value As Integer)
                _BookAPAdjReasonControl = value
            End Set
        End Property


        Private _CompanyName As String = ""
        <DataMember()> _
        Public Property CompanyName() As String
            Get
                Return _CompanyName
            End Get
            Friend Set(ByVal value As String)
                _CompanyName = value
            End Set
        End Property

        Private _CompanyNumber As String = ""
        <DataMember()> _
        Public Property CompanyNumber() As String
            Get
                Return _CompanyNumber
            End Get
            Friend Set(ByVal value As String)
                _CompanyNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Friend Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property

        Private _CarrierNumber As String = ""
        <DataMember()> _
        Public Property CarrierNumber() As String
            Get
                Return _CarrierNumber
            End Get
            Friend Set(ByVal value As String)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierText As String = ""
        <DataMember()> _
        Public Property CarrierText() As String
            Get
                If _CarrierText.Trim.Length < 1 Then _CarrierText = _CarrierNumber & " " & _CarrierName
                Return _CarrierText
            End Get
            Friend Set(ByVal value As String)
                _CarrierText = value
            End Set
        End Property

        Private _CommissionsName As String = ""
        <DataMember()> _
        Public Property CommissionsName() As String
            Get
                Return _CommissionsName
            End Get
            Friend Set(ByVal value As String)
                _CommissionsName = value
            End Set
        End Property

        Private _LaneName As String = ""
        <DataMember()> _
        Public Property LaneName() As String
            Get
                Return _LaneName
            End Get
            Friend Set(ByVal value As String)
                _LaneName = value
            End Set
        End Property

        Private _LaneOriginAddressUse As Boolean = False
        <DataMember()> _
        Public Property LaneOriginAddressUse() As Boolean
            Get
                Return _LaneOriginAddressUse
            End Get
            Set(ByVal value As Boolean)
                _LaneOriginAddressUse = value
            End Set
        End Property

        Private _BookLockAllCosts As Boolean = False
        <DataMember()> _
        Public Property BookLockAllCosts() As Boolean
            Get
                Return _BookLockAllCosts
            End Get
            Set(ByVal value As Boolean)
                _BookLockAllCosts = value
            End Set
        End Property

        Private _BookLockBFCCost As Boolean = False
        <DataMember()> _
        Public Property BookLockBFCCost() As Boolean
            Get
                Return _BookLockBFCCost
            End Get
            Set(ByVal value As Boolean)
                _BookLockBFCCost = value
            End Set
        End Property

        Private _CompFinUseImportFrtCost As Boolean = False
        <DataMember()> _
        Public Property CompFinUseImportFrtCost() As Boolean
            Get
                Return _CompFinUseImportFrtCost
            End Get
            Set(ByVal value As Boolean)
                _CompFinUseImportFrtCost = value
            End Set
        End Property

        Private _BookDestStopNumber As Integer = 0
        <DataMember()> _
        Public Property BookDestStopNumber() As Integer
            Get
                Return _BookDestStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookDestStopNumber = value
            End Set
        End Property

        Private _BookOrigStopNumber As Integer = 0
        <DataMember()> _
        Public Property BookOrigStopNumber() As Integer
            Get
                Return _BookOrigStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookOrigStopNumber = value
            End Set
        End Property

        Private _BookOrigStopControl As Integer = 0
        <DataMember()> _
        Public Property BookOrigStopControl() As Integer
            Get
                Return _BookOrigStopControl
            End Get
            Set(ByVal value As Integer)
                _BookOrigStopControl = value
            End Set
        End Property

        Private _BookDestStopControl As Integer = 0
        <DataMember()> _
        Public Property BookDestStopControl() As Integer
            Get
                Return _BookDestStopControl
            End Get
            Set(ByVal value As Integer)
                _BookDestStopControl = value
            End Set
        End Property

        Private _BookRouteTypeCode As Integer = 6
        <DataMember()> _
        Public Property BookRouteTypeCode() As Integer
            Get
                If _BookRouteTypeCode = 0 Then _BookRouteTypeCode = 6
                Return _BookRouteTypeCode
            End Get
            Set(ByVal value As Integer)
                If value = 0 Then value = 6
                _BookRouteTypeCode = value
            End Set
        End Property

        Private _BookAlternateAddressLaneControl As Integer = 0
        <DataMember()> _
        Public Property BookAlternateAddressLaneControl() As Integer
            Get
                Return _BookAlternateAddressLaneControl
            End Get
            Set(ByVal value As Integer)
                _BookAlternateAddressLaneControl = value
            End Set
        End Property

        Private _BookAlternateAddressLaneNumber As String = ""
        <DataMember()> _
        Public Property BookAlternateAddressLaneNumber() As String
            Get
                Return Left(_BookAlternateAddressLaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAlternateAddressLaneNumber = Left(value, 50)
            End Set
        End Property

        Private _BookDefaultRouteSequence As Integer = 0
        <DataMember()> _
        Public Property BookDefaultRouteSequence() As Integer
            Get
                Return _BookDefaultRouteSequence
            End Get
            Set(ByVal value As Integer)
                _BookDefaultRouteSequence = value
            End Set
        End Property

        Private _BookRouteGuideControl As Integer = 0
        <DataMember()> _
        Public Property BookRouteGuideControl() As Integer
            Get
                Return _BookRouteGuideControl
            End Get
            Set(ByVal value As Integer)
                _BookRouteGuideControl = value
            End Set
        End Property

        Private _BookRouteGuideNumber As String = ""
        <DataMember()> _
        Public Property BookRouteGuideNumber() As String
            Get
                Return Left(_BookRouteGuideNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookRouteGuideNumber = Left(value, 50)
            End Set
        End Property

        Private _BookCustomerApprovalTransmitted As Boolean = False
        <DataMember()> _
        Public Property BookCustomerApprovalTransmitted() As Boolean
            Get
                Return _BookCustomerApprovalTransmitted
            End Get
            Set(ByVal value As Boolean)
                _BookCustomerApprovalTransmitted = value
            End Set
        End Property

        Private _BookCustomerApprovalRecieved As Boolean = False
        <DataMember()> _
        Public Property BookCustomerApprovalRecieved() As Boolean
            Get
                Return _BookCustomerApprovalRecieved
            End Get
            Set(ByVal value As Boolean)
                _BookCustomerApprovalRecieved = value
            End Set
        End Property

        Private _BookCarrTruckControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrTruckControl() As Integer
            Get
                Return _BookCarrTruckControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTruckControl = value
            End Set
        End Property

        Private _BookCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrTarControl() As Integer
            Get
                Return _BookCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarControl = value
            End Set
        End Property

        Private _BookCarrTarRevisionNumber As Integer = 0
        <DataMember()> _
        Public Property BookCarrTarRevisionNumber() As Integer
            Get
                Return _BookCarrTarRevisionNumber
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarRevisionNumber = value
            End Set
        End Property

        Private _BookCarrTarName As String
        <DataMember()> _
        Public Property BookCarrTarName As String
            Get
                Return Left(_BookCarrTarName, 50)
            End Get
            Set(value As String)
                _BookCarrTarName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrTarEquipControl() As Integer
            Get
                Return _BookCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipControl = value
            End Set
        End Property

        Private _BookCarrTarEquipName As String
        <DataMember()> _
        Public Property BookCarrTarEquipName As String
            Get
                Return Left(_BookCarrTarEquipName, 50)
            End Get
            Set(value As String)
                _BookCarrTarEquipName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipMatName As String
        <DataMember()> _
        Public Property BookCarrTarEquipMatName As String
            Get
                Return Left(_BookCarrTarEquipMatName, 50)
            End Get
            Set(value As String)
                _BookCarrTarEquipMatName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipMatControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrTarEquipMatControl() As Integer
            Get
                Return _BookCarrTarEquipMatControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatControl = value
            End Set
        End Property

        Private _BookCarrTarEquipMatDetControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrTarEquipMatDetControl() As Integer
            Get
                Return _BookCarrTarEquipMatDetControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatDetControl = value
            End Set
        End Property

        Private _BookCarrTarEquipMatDetID As Integer = 0
        <DataMember()> _
        Public Property BookCarrTarEquipMatDetID() As Integer
            Get
                Return _BookCarrTarEquipMatDetID
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatDetID = value
            End Set
        End Property

        Private _BookCarrTarEquipMatDetValue As System.Nullable(Of Decimal)
        <DataMember()> _
        Public Property BookCarrTarEquipMatDetValue() As System.Nullable(Of Decimal)
            Get
                Return _BookCarrTarEquipMatDetValue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _BookCarrTarEquipMatDetValue = value
            End Set
        End Property

        Private _BookBookRevHistRevision As Integer = 0
        <DataMember()> _
        Public Property BookBookRevHistRevision() As Integer
            Get
                Return _BookBookRevHistRevision
            End Get
            Set(ByVal value As Integer)
                _BookBookRevHistRevision = value
            End Set
        End Property

        Private _BookRevLaneBenchMiles As System.Nullable(Of Double)
        <DataMember()> _
        Public Property BookRevLaneBenchMiles() As System.Nullable(Of Double)
            Get
                Return _BookRevLaneBenchMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookRevLaneBenchMiles = value
            End Set
        End Property

        Private _BookRevLoadMiles As System.Nullable(Of Double)
        <DataMember()> _
        Public Property BookRevLoadMiles() As System.Nullable(Of Double)
            Get
                Return _BookRevLoadMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookRevLoadMiles = value
            End Set
        End Property

        Private _BookModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property BookModeTypeControl() As Integer
            Get
                Return _BookModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookModeTypeControl = value
            End Set
        End Property

        Private _BookAllowInterlinePoints As Boolean = True
        <DataMember()> _
        Public Property BookAllowInterlinePoints() As Boolean
            Get
                Return _BookAllowInterlinePoints
            End Get
            Set(ByVal value As Boolean)
                _BookAllowInterlinePoints = value
            End Set
        End Property

        Private _BookUser1 As String = ""
        <DataMember()> _
        Public Property BookUser1() As String
            Get
                Return Left(_BookUser1, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser1 = Left(value, 4000)
            End Set
        End Property

        Private _BookUser2 As String = ""
        <DataMember()> _
        Public Property BookUser2() As String
            Get
                Return Left(_BookUser2, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser2 = Left(value, 4000)
            End Set
        End Property

        Private _BookUser3 As String = ""
        <DataMember()> _
        Public Property BookUser3() As String
            Get
                Return Left(_BookUser3, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser3 = Left(value, 4000)
            End Set
        End Property

        Private _BookUser4 As String = ""
        <DataMember()> _
        Public Property BookUser4() As String
            Get
                Return Left(_BookUser4, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser4 = Left(value, 4000)
            End Set
        End Property

        Private _BookExpDelDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookExpDelDateTime() As System.Nullable(Of Date)
            Get
                Return _BookExpDelDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookExpDelDateTime = value
            End Set
        End Property

        Private _BookMustLeaveByDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookMustLeaveByDateTime() As System.Nullable(Of Date)
            Get
                Return _BookMustLeaveByDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookMustLeaveByDateTime = value
            End Set
        End Property

        Private _BookMultiMode As Boolean = False
        <DataMember()> _
        Public Property BookMultiMode() As Boolean
            Get
                Return _BookMultiMode
            End Get
            Set(ByVal value As Boolean)
                _BookMultiMode = value
            End Set
        End Property

        Private _BookOriginalLaneControl As Integer = 0
        <DataMember()> _
        Public Property BookOriginalLaneControl() As Integer
            Get
                Return _BookOriginalLaneControl
            End Get
            Set(ByVal value As Integer)
                _BookOriginalLaneControl = value
            End Set
        End Property

        Private _BookLaneTranXControl As Integer = 0
        <DataMember()> _
        Public Property BookLaneTranXControl() As Integer
            Get
                Return _BookLaneTranXControl
            End Get
            Set(ByVal value As Integer)
                _BookLaneTranXControl = value
            End Set
        End Property

        Private _BookLaneTranXDetControl As Integer = 0
        <DataMember()> _
        Public Property BookLaneTranXDetControl() As Integer
            Get
                Return _BookLaneTranXDetControl
            End Get
            Set(ByVal value As Integer)
                _BookLaneTranXDetControl = value
            End Set
        End Property

        Private _BookSHID As String
        <DataMember()> _
        Public Property BookSHID() As String
            Get
                Return Left(_BookSHID, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookSHID, value) = False) Then
                    Me._BookSHID = Left(value, 50)
                    Me.SendPropertyChanged("BookSHID")
                End If
            End Set
        End Property

        Private _BookShipCarrierDetails As String
        <DataMember()> _
        Public Property BookShipCarrierDetails() As String
            Get
                Return Left(_BookShipCarrierDetails, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookShipCarrierDetails, value) = False) Then
                    Me._BookShipCarrierDetails = Left(value, 4000)
                    Me.SendPropertyChanged("BookShipCarrierDetails")
                End If
            End Set
        End Property

        Private _BookCreditHold As Boolean = False
        <DataMember()> _
        Public Property BookCreditHold() As Boolean
            Get
                Return _BookCreditHold
            End Get
            Set(ByVal value As Boolean)
                _BookCreditHold = value
            End Set
        End Property

        'Added by LVV 6/22/16 for v-7.0.5.110 DAT
        Private _BookRevLoadTenderTypeControl As Integer = 0
        <DataMember()> _
        Public Property BookRevLoadTenderTypeControl() As Integer
            Get
                Return _BookRevLoadTenderTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookRevLoadTenderTypeControl = value
            End Set
        End Property

        'Added by LVV 6/22/16 for v-7.0.5.110 DAT
        Private _BookRevLoadTenderStatusCode As Integer = 0
        <DataMember()> _
        Public Property BookRevLoadTenderStatusCode() As Integer
            Get
                Return _BookRevLoadTenderStatusCode
            End Get
            Set(ByVal value As Integer)
                _BookRevLoadTenderStatusCode = value
            End Set
        End Property

        'Added by LVV 10/25/16 for v-7.0.5.110 Add Book Interline
        Private _BookCarrTarInterlinePoint As Boolean = False
        <DataMember()> _
        Public Property BookCarrTarInterlinePoint() As Boolean
            Get
                Return _BookCarrTarInterlinePoint
            End Get
            Set(ByVal value As Boolean)
                _BookCarrTarInterlinePoint = value
            End Set
        End Property

        'Added By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _BookRevPreferredCarrier As Boolean = False
        <DataMember()> _
        Public Property BookRevPreferredCarrier() As Boolean
            Get
                Return _BookRevPreferredCarrier
            End Get
            Set(ByVal value As Boolean)
                _BookRevPreferredCarrier = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookLoadDetail
            instance = DirectCast(MemberwiseClone(), BookLoadDetail)
            Return instance
        End Function

#End Region

    End Class
End Namespace
