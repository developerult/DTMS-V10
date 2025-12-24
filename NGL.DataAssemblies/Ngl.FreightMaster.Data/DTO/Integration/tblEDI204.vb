Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV 2/29/16 for v-7.0.5.1 EDI Migration
Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblEDI204
        Inherits DTOBaseClass

#Region " Data Members "

        Private _EDI204Control As Integer = 0
        <DataMember()> _
        Public Property EDI204Control() As Integer
            Get
                Return _EDI204Control
            End Get
            Set(ByVal value As Integer)
                _EDI204Control = value
            End Set
        End Property

        Private _CarrierSCAC As String = ""
        <DataMember()> _
        Public Property CarrierSCAC() As String
            Get
                Return Left(_CarrierSCAC, 4)
            End Get
            Set(ByVal value As String)
                _CarrierSCAC = Left(value, 4)
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 40)
            End Set
        End Property

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

        Private _BookTransactionPurpose As String = ""
        <DataMember()> _
        Public Property BookTransactionPurpose() As String
            Get
                Return Left(_BookTransactionPurpose, 2)
            End Get
            Set(ByVal value As String)
                _BookTransactionPurpose = Left(value, 2)
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

        Private _BookCarrScheduleDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrScheduleDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleDate = value
            End Set
        End Property

        Private _BookCarrScheduleTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrScheduleTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleTime = value
            End Set
        End Property

        Private _BookCarrApptDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrApptDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptDate = value
            End Set
        End Property

        Private _BookCarrApptTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookCarrApptTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptTime = value
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

        Private _BookOrigIDENTIFICATIONCODEQUALIFIER As String = ""
        <DataMember()> _
        Public Property BookOrigIDENTIFICATIONCODEQUALIFIER() As String
            Get
                Return Left(_BookOrigIDENTIFICATIONCODEQUALIFIER, 2)
            End Get
            Set(ByVal value As String)
                _BookOrigIDENTIFICATIONCODEQUALIFIER = Left(value, 2)
            End Set
        End Property

        Private _BookOrigCompanyNumber As String = ""
        <DataMember()> _
        Public Property BookOrigCompanyNumber() As String
            Get
                Return Left(_BookOrigCompanyNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookOrigCompanyNumber = Left(value, 50)
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

        Private _BookDestIDENTIFICATIONCODEQUALIFIER As String = ""
        <DataMember()> _
        Public Property BookDestIDENTIFICATIONCODEQUALIFIER() As String
            Get
                Return Left(_BookDestIDENTIFICATIONCODEQUALIFIER, 2)
            End Get
            Set(ByVal value As String)
                _BookDestIDENTIFICATIONCODEQUALIFIER = Left(value, 2)
            End Set
        End Property

        Private _BookDestCompanyNumber As String = ""
        <DataMember()> _
        Public Property BookDestCompanyNumber() As String
            Get
                Return Left(_BookDestCompanyNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookDestCompanyNumber = Left(value, 50)
            End Set
        End Property

        Private _BookLoadPONumber As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber() As String
            Get
                Return Left(_BookLoadPONumber, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber = Left(value, 20)
            End Set
        End Property

        Private _BookLoadCom As String
        <DataMember()> _
        Public Property BookLoadCom() As String
            Get
                If Len(Trim(_BookLoadCom)) < 1 Then _BookLoadCom = "D"
                Return _BookLoadCom
            End Get
            Set(ByVal value As String)
                _BookLoadCom = value
            End Set
        End Property

        Private _CommCodeDescription As String = ""
        <DataMember()> _
        Public Property CommCodeDescription() As String
            Get
                Return Left(_CommCodeDescription, 40)
            End Get
            Set(ByVal value As String)
                _CommCodeDescription = Left(value, 40)
            End Set
        End Property

        Private _LaneComments As String = ""
        <DataMember()> _
        Public Property LaneComments() As String
            Get
                Return Left(_LaneComments, 255)
            End Get
            Set(ByVal value As String)
                _LaneComments = Left(value, 255)
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

        Private _BookTrackDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookTrackDate() As System.Nullable(Of Date)
            Get
                Return _BookTrackDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookTrackDate = value
            End Set
        End Property

        Private _CompEDISecurityQual As String = ""
        <DataMember()> _
        Public Property CompEDISecurityQual() As String
            Get
                Return Left(_CompEDISecurityQual, 2)
            End Get
            Set(ByVal value As String)
                _CompEDISecurityQual = Left(value, 2)
            End Set
        End Property

        Private _CompEDISecurityCode As String = ""
        <DataMember()> _
        Public Property CompEDISecurityCode() As String
            Get
                Return Left(_CompEDISecurityCode, 10)
            End Get
            Set(ByVal value As String)
                _CompEDISecurityCode = Left(value, 10)
            End Set
        End Property

        Private _CompEDIPartnerQual As String = ""
        <DataMember()> _
        Public Property CompEDIPartnerQual() As String
            Get
                Return Left(_CompEDIPartnerQual, 2)
            End Get
            Set(ByVal value As String)
                _CompEDIPartnerQual = Left(value, 2)
            End Set
        End Property

        Private _CompEDIPartnerCode As String = ""
        <DataMember()> _
        Public Property CompEDIPartnerCode() As String
            Get
                Return Left(_CompEDIPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _CompEDIPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _CompEDIEmailNotificationOn As Boolean = False
        <DataMember()> _
        Public Property CompEDIEmailNotificationOn() As Boolean
            Get
                Return _CompEDIEmailNotificationOn
            End Get
            Set(ByVal value As Boolean)
                _CompEDIEmailNotificationOn = value
            End Set
        End Property

        Private _CompEDIEmailAddress As String = ""
        <DataMember()> _
        Public Property CompEDIEmailAddress() As String
            Get
                Return Left(_CompEDIEmailAddress, 255)
            End Get
            Set(ByVal value As String)
                _CompEDIEmailAddress = Left(value, 255)
            End Set
        End Property

        Private _CompEDIAcknowledgementRequested As Boolean = False
        <DataMember()> _
        Public Property CompEDIAcknowledgementRequested() As Boolean
            Get
                Return _CompEDIAcknowledgementRequested
            End Get
            Set(ByVal value As Boolean)
                _CompEDIAcknowledgementRequested = value
            End Set
        End Property

        Private _CompEDIMethodOfPayment As String = ""
        <DataMember()> _
        Public Property CompEDIMethodOfPayment() As String
            Get
                Return Left(_CompEDIMethodOfPayment, 2)
            End Get
            Set(ByVal value As String)
                _CompEDIMethodOfPayment = Left(value, 2)
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

        Private _BillToCompName As String = ""
        <DataMember()> _
        Public Property BillToCompName() As String
            Get
                Return Left(_BillToCompName, 40)
            End Get
            Set(ByVal value As String)
                _BillToCompName = Left(value, 40)
            End Set
        End Property

        Private _BillToCompNumber As String = ""
        <DataMember()> _
        Public Property BillToCompNumber() As String
            Get
                Return Left(_BillToCompNumber, 50)
            End Get
            Set(ByVal value As String)
                _BillToCompNumber = Left(value, 50)
            End Set
        End Property

        Private _BillToCompAddress1 As String = ""
        <DataMember()> _
        Public Property BillToCompAddress1() As String
            Get
                Return Left(_BillToCompAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BillToCompAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BillToCompAddress2 As String = ""
        <DataMember()> _
        Public Property BillToCompAddress2() As String
            Get
                Return Left(_BillToCompAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BillToCompAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BillToCompCity As String = ""
        <DataMember()> _
        Public Property BillToCompCity() As String
            Get
                Return Left(_BillToCompCity, 25)
            End Get
            Set(ByVal value As String)
                _BillToCompCity = Left(value, 25)
            End Set
        End Property

        Private _BillToCompState As String = ""
        <DataMember()> _
        Public Property BillToCompState() As String
            Get
                Return Left(_BillToCompState, 8)
            End Get
            Set(ByVal value As String)
                _BillToCompState = Left(value, 8)
            End Set
        End Property

        Private _BillToCompZip As String = ""
        <DataMember()> _
        Public Property BillToCompZip() As String
            Get
                Return Left(_BillToCompZip, 10)
            End Get
            Set(ByVal value As String)
                _BillToCompZip = Left(value, 10)
            End Set
        End Property

        Private _BillToCompCountry As String = ""
        <DataMember()> _
        Public Property BillToCompCountry() As String
            Get
                Return Left(_BillToCompCountry, 30)
            End Get
            Set(ByVal value As String)
                _BillToCompCountry = Left(value, 30)
            End Set
        End Property

        Private _EDICombineOrdersForStops As Decimal = 0
        <DataMember()> _
        Public Property EDICombineOrdersForStops() As Decimal
            Get
                Return _EDICombineOrdersForStops
            End Get
            Set(ByVal value As Decimal)
                _EDICombineOrdersForStops = value
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

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Left(_CompName, 40)
            End Get
            Set(ByVal value As String)
                _CompName = Left(value, 40)
            End Set
        End Property

        Private _FirstDateSent As System.Nullable(Of Date)
        <DataMember()> _
        Public Property FirstDateSent() As System.Nullable(Of Date)
            Get
                Return _FirstDateSent
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _FirstDateSent = value
            End Set
        End Property

        Private _LastDateSent As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LastDateSent() As System.Nullable(Of Date)
            Get
                Return _LastDateSent
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LastDateSent = value
            End Set
        End Property

        Private _EDI204997Received As Boolean = False
        <DataMember()> _
        Public Property EDI204997Received() As Boolean
            Get
                Return _EDI204997Received
            End Get
            Set(ByVal value As Boolean)
                _EDI204997Received = value
            End Set
        End Property

        Private _EDI204997ReceivedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI204997ReceivedDate() As System.Nullable(Of Date)
            Get
                Return _EDI204997ReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI204997ReceivedDate = value
            End Set
        End Property

        Private _EDI990Received As Boolean = False
        <DataMember()> _
        Public Property EDI990Received() As Boolean
            Get
                Return _EDI990Received
            End Get
            Set(ByVal value As Boolean)
                _EDI990Received = value
            End Set
        End Property

        Private _EDI990ReceivedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI990ReceivedDate() As System.Nullable(Of Date)
            Get
                Return _EDI990ReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI990ReceivedDate = value
            End Set
        End Property

        Private _EDI204Retry As Integer = 0
        <DataMember()> _
        Public Property EDI204Retry() As Integer
            Get
                Return _EDI204Retry
            End Get
            Set(ByVal value As Integer)
                _EDI204Retry = value
            End Set
        End Property

        Private _EDI204StatusCode As Integer? = 0
        <DataMember()> _
        Public Property EDI204StatusCode() As Integer?
            Get
                Return _EDI204StatusCode
            End Get
            Set(ByVal value As Integer?)
                _EDI204StatusCode = value
            End Set
        End Property

        Private _EDI204Message As String = ""
        <DataMember()> _
        Public Property EDI204Message() As String
            Get
                Return _EDI204Message
            End Get
            Set(ByVal value As String)
                _EDI204Message = value
            End Set
        End Property

        Private _Archived As Boolean = False
        <DataMember()> _
        Public Property Archived() As Boolean
            Get
                Return _Archived
            End Get
            Set(ByVal value As Boolean)
                _Archived = value
            End Set
        End Property

        Private _EDI204ModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI204ModDate() As System.Nullable(Of Date)
            Get
                Return _EDI204ModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI204ModDate = value
                Me.SendPropertyChanged("EDI204ModDate")
            End Set
        End Property

        Private _EDI204ModUser As String = ""
        <DataMember()> _
        Public Property EDI204ModUser() As String
            Get
                Return Left(_EDI204ModUser, 100)
            End Get
            Set(ByVal value As String)
                _EDI204ModUser = Left(value, 100)
                Me.SendPropertyChanged("EDI204ModUser")
            End Set
        End Property

        Private _EDI204Updated As Byte()
        <DataMember()> _
        Public Property EDI204Updated() As Byte()
            Get
                Return _EDI204Updated
            End Get
            Set(ByVal value As Byte())
                _EDI204Updated = value
            End Set
        End Property

        Private _EDI204FileName204 As String = ""
        <DataMember()> _
        Public Property EDI204FileName204() As String
            Get
                Return Left(_EDI204FileName204, 255)
            End Get
            Set(ByVal value As String)
                _EDI204FileName204 = Left(value, 255)
                Me.SendPropertyChanged("EDI204FileName204")
            End Set
        End Property

        Private _EDI204FileName997 As String = ""
        <DataMember()> _
        Public Property EDI204FileName997() As String
            Get
                Return Left(_EDI204FileName997, 255)
            End Get
            Set(ByVal value As String)
                _EDI204FileName997 = Left(value, 255)
                Me.SendPropertyChanged("EDI204FileName997")
            End Set
        End Property

        Private _EDI204FileName990 As String = ""
        <DataMember()> _
        Public Property EDI204FileName990() As String
            Get
                Return Left(_EDI204FileName990, 255)
            End Get
            Set(ByVal value As String)
                _EDI204FileName990 = Left(value, 255)
                Me.SendPropertyChanged("EDI204FileName990")
            End Set
        End Property

        Private _EDI204GS06 As Integer = 0
        <DataMember()> _
        Public Property EDI204GS06() As Integer
            Get
                Return _EDI204GS06
            End Get
            Set(ByVal value As Integer)
                _EDI204GS06 = value
            End Set
        End Property

        Private _BookSHID As String = ""
        <DataMember()> _
        Public Property BookSHID() As String
            Get
                Return Left(_BookSHID, 50)
            End Get
            Set(ByVal value As String)
                _BookSHID = Left(value, 50)
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblEDI204
            instance = DirectCast(MemberwiseClone(), tblEDI204)
            Return instance
        End Function

#End Region

    End Class
End Namespace
