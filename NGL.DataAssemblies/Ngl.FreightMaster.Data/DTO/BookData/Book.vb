Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Web.UI.WebControls
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()>
    Public Class Book
        Inherits DTOBaseClass

        Public Sub New()
            MyBase.New()
            Logger = Logger.ForContext(of Book)
        End Sub



#Region " Data Members"
        Private _BookControl As Integer = 0
        <DataMember()>
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _BookProNumber As String = ""
        <DataMember()>
        Public Property BookProNumber() As String
            Get
                Return Left(_BookProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookProBase As String = ""
        <DataMember()>
        Public Property BookProBase() As String
            Get
                Return Left(_BookProBase, 50)
            End Get
            Set(ByVal value As String)
                _BookProBase = Left(value, 50)
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()>
        Public Property BookConsPrefix() As String
            Get
                Return Left(_BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _BookCustCompControl As Integer = 0
        <DataMember()>
        Public Property BookCustCompControl() As Integer
            Get
                Return _BookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCustCompControl = value
            End Set
        End Property

        Private _BookCommCompControl As Integer = 0
        <DataMember()>
        Public Property BookCommCompControl() As Integer
            Get
                Return _BookCommCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCommCompControl = value
            End Set
        End Property

        Private _BookODControl As Integer = 0
        <DataMember()>
        Public Property BookODControl() As Integer
            Get
                Return _BookODControl
            End Get
            Set(ByVal value As Integer)
                _BookODControl = value
            End Set
        End Property

        Private _BookCarrierControl As Integer = 0
        <DataMember()>
        Public Property BookCarrierControl() As Integer
            Get
                Return _BookCarrierControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrierControl = value
            End Set
        End Property

        Private _BookCarrierContact As String = ""
        <DataMember()>
        Public Property BookCarrierContact() As String
            Get
                Return Left(_BookCarrierContact, 30)
            End Get
            Set(ByVal value As String)
                _BookCarrierContact = Left(value, 30)
            End Set
        End Property

        Private _BookCarrierContactPhone As String = ""
        <DataMember()>
        Public Property BookCarrierContactPhone() As String
            Get
                Return Left(_BookCarrierContactPhone, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrierContactPhone = Left(value, 20)
            End Set
        End Property

        Private _BookOrigCompControl As Integer = 0
        <DataMember()>
        Public Property BookOrigCompControl() As Integer
            Get
                Return _BookOrigCompControl
            End Get
            Set(ByVal value As Integer)
                _BookOrigCompControl = value
            End Set
        End Property

        Private _BookOrigName As String = ""
        <DataMember()>
        Public Property BookOrigName() As String
            Get
                Return Left(_BookOrigName, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigName = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress1 As String = ""
        <DataMember()>
        Public Property BookOrigAddress1() As String
            Get
                Return Left(_BookOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress2 As String = ""
        <DataMember()>
        Public Property BookOrigAddress2() As String
            Get
                Return Left(_BookOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress3 As String = ""
        <DataMember()>
        Public Property BookOrigAddress3() As String
            Get
                Return Left(_BookOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigAddress3 = Left(value, 40)
            End Set
        End Property

        Private _BookOrigCity As String = ""
        <DataMember()>
        Public Property BookOrigCity() As String
            Get
                Return Left(_BookOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _BookOrigCity = Left(value, 25)
            End Set
        End Property

        Private _BookOrigState As String = ""
        <DataMember()>
        Public Property BookOrigState() As String
            Get
                Return Left(_BookOrigState, 8)
            End Get
            Set(ByVal value As String)
                _BookOrigState = Left(value, 8)
            End Set
        End Property

        Private _BookOrigCountry As String = ""
        <DataMember()>
        Public Property BookOrigCountry() As String
            Get
                Return Left(_BookOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _BookOrigZip As String = ""
        <DataMember()>
        Public Property BookOrigZip() As String
            Get
                Return Left(_BookOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookOrigPhone As String = ""
        <DataMember()>
        Public Property BookOrigPhone() As String
            Get
                Return Left(_BookOrigPhone, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookOrigPhone = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookOrigFax As String = ""
        <DataMember()>
        Public Property BookOrigFax() As String
            Get
                Return Left(_BookOrigFax, 15)
            End Get
            Set(ByVal value As String)
                _BookOrigFax = Left(value, 15)
            End Set
        End Property

        Private _BookOriginStartHrs As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookOriginStartHrs() As System.Nullable(Of Date)
            Get
                Return _BookOriginStartHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookOriginStartHrs = value
            End Set
        End Property

        Private _BookOriginStopHrs As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookOriginStopHrs() As System.Nullable(Of Date)
            Get
                Return _BookOriginStopHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookOriginStopHrs = value
            End Set
        End Property

        Private _BookOriginApptReq As Boolean = False
        <DataMember()>
        Public Property BookOriginApptReq() As Boolean
            Get
                Return _BookOriginApptReq
            End Get
            Set(ByVal value As Boolean)
                _BookOriginApptReq = value
            End Set
        End Property

        Private _BookDestCompControl As Integer = 0
        <DataMember()>
        Public Property BookDestCompControl() As Integer
            Get
                Return _BookDestCompControl
            End Get
            Set(ByVal value As Integer)
                _BookDestCompControl = value
            End Set
        End Property

        Private _BookDestName As String = ""
        <DataMember()>
        Public Property BookDestName() As String
            Get
                Return Left(_BookDestName, 40)
            End Get
            Set(ByVal value As String)
                _BookDestName = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress1 As String = ""
        <DataMember()>
        Public Property BookDestAddress1() As String
            Get
                Return Left(_BookDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress2 As String = ""
        <DataMember()>
        Public Property BookDestAddress2() As String
            Get
                Return Left(_BookDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress3 As String = ""
        <DataMember()>
        Public Property BookDestAddress3() As String
            Get
                Return Left(_BookDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress3 = Left(value, 40)
            End Set
        End Property

        Private _BookDestCity As String = ""
        <DataMember()>
        Public Property BookDestCity() As String
            Get
                Return Left(_BookDestCity, 25)
            End Get
            Set(ByVal value As String)
                _BookDestCity = Left(value, 25)
            End Set
        End Property

        Private _BookDestState As String = ""
        <DataMember()>
        Public Property BookDestState() As String
            Get
                Return Left(_BookDestState, 2)
            End Get
            Set(ByVal value As String)
                _BookDestState = Left(value, 2)
            End Set
        End Property

        Private _BookDestCountry As String = ""
        <DataMember()>
        Public Property BookDestCountry() As String
            Get
                Return Left(_BookDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookDestCountry = Left(value, 30)
            End Set
        End Property

        Private _BookDestZip As String = ""
        <DataMember()>
        Public Property BookDestZip() As String
            Get
                Return Left(_BookDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookDestPhone As String = ""
        <DataMember()>
        Public Property BookDestPhone() As String
            Get
                Return Left(_BookDestPhone, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookDestPhone = Left(value, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookDestFax As String = ""
        <DataMember()>
        Public Property BookDestFax() As String
            Get
                Return Left(_BookDestFax, 15)
            End Get
            Set(ByVal value As String)
                _BookDestFax = Left(value, 15)
            End Set
        End Property

        Private _BookDestStartHrs As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookDestStartHrs() As System.Nullable(Of Date)
            Get
                Return _BookDestStartHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDestStartHrs = value
            End Set
        End Property

        Private _BookDestStopHrs As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookDestStopHrs() As System.Nullable(Of Date)
            Get
                Return _BookDestStopHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDestStopHrs = value
            End Set
        End Property

        Private _BookDestApptReq As Boolean = False
        <DataMember()>
        Public Property BookDestApptReq() As Boolean
            Get
                Return _BookDestApptReq
            End Get
            Set(ByVal value As Boolean)
                _BookDestApptReq = value
            End Set
        End Property

        Private _BookDateOrdered As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookDateOrdered() As System.Nullable(Of Date)
            Get
                Return _BookDateOrdered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateOrdered = value
            End Set
        End Property

        Private _BookDateLoad As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookDateLoad() As System.Nullable(Of Date)
            Get
                Return _BookDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateLoad = value
            End Set
        End Property

        Private _BookDateInvoice As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookDateInvoice() As System.Nullable(Of Date)
            Get
                Return _BookDateInvoice
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateInvoice = value
            End Set
        End Property

        Private _BookDateRequired As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookDateRequired() As System.Nullable(Of Date)
            Get
                Return _BookDateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateRequired = value
            End Set
        End Property

        Private _BookDateDelivered As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookDateDelivered() As System.Nullable(Of Date)
            Get
                Return _BookDateDelivered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateDelivered = value
            End Set
        End Property

        Private _BookTotalCases As Integer = 0
        <DataMember()>
        Public Property BookTotalCases() As Integer
            Get
                Return _BookTotalCases
            End Get
            Set(ByVal value As Integer)
                _BookTotalCases = value
            End Set
        End Property

        Private _BookTotalWgt As Double = 0
        <DataMember()>
        Public Property BookTotalWgt() As Double
            Get
                Return _BookTotalWgt
            End Get
            Set(ByVal value As Double)
                _BookTotalWgt = value
            End Set
        End Property

        Private _BookTotalPL As Double = 0
        <DataMember()>
        Public Property BookTotalPL() As Double
            Get
                Return _BookTotalPL
            End Get
            Set(ByVal value As Double)
                _BookTotalPL = value
            End Set
        End Property

        Private _BookTotalCube As Integer = 0
        <DataMember()>
        Public Property BookTotalCube() As Integer
            Get
                Return _BookTotalCube
            End Get
            Set(ByVal value As Integer)
                _BookTotalCube = value
            End Set
        End Property

        Private _BookTotalPX As Integer = 0
        <DataMember()>
        Public Property BookTotalPX() As Integer
            Get
                Return _BookTotalPX
            End Get
            Set(ByVal value As Integer)
                _BookTotalPX = value
            End Set
        End Property

        Private _BookTotalBFC As Decimal = 0
        <DataMember()>
        Public Property BookTotalBFC() As Decimal
            Get
                Return _BookTotalBFC
            End Get
            Set(ByVal value As Decimal)
                _BookTotalBFC = value
            End Set
        End Property

        Private _BookTranCode As String = ""
        <DataMember()>
        Public Property BookTranCode() As String
            Get
                Return Left(_BookTranCode, 3)
            End Get
            Set(ByVal value As String)
                _BookTranCode = Left(value, 3)
            End Set
        End Property

        Private _BookPayCode As String = ""
        <DataMember()>
        Public Property BookPayCode() As String
            Get
                Return Left(_BookPayCode, 3)
            End Get
            Set(ByVal value As String)
                _BookPayCode = Left(value, 3)
            End Set
        End Property

        Private _BookTypeCode As String = ""
        <DataMember()>
        Public Property BookTypeCode() As String
            Get
                Return Left(_BookTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookBOLCode As Boolean = False
        <DataMember()>
        Public Property BookBOLCode() As Boolean
            Get
                Return _BookBOLCode
            End Get
            Set(ByVal value As Boolean)
                _BookBOLCode = value
            End Set
        End Property

        Private _BookStopNo As Short = 0
        <DataMember()>
        Public Property BookStopNo() As Short
            Get
                Return _BookStopNo
            End Get
            Set(ByVal value As Short)
                _BookStopNo = value
            End Set
        End Property

        Private _BookModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookModDate() As System.Nullable(Of Date)
            Get
                Return _BookModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookModDate = value
            End Set
        End Property

        Private _BookModUser As String = ""
        <DataMember()>
        Public Property BookModUser() As String
            Get
                Return Left(_BookModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookModUser = Left(value, 100)
            End Set
        End Property

        Private _BookUpdated As Byte()
        <DataMember()>
        Public Property BookUpdated() As Byte()
            Get
                Return _BookUpdated
            End Get
            Set(ByVal value As Byte())
                _BookUpdated = value
            End Set
        End Property

        Private _BookCarrFBNumber As String = ""
        <DataMember()>
        Public Property BookCarrFBNumber() As String
            Get
                Return Left(_BookCarrFBNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrFBNumber = Left(value, 20)
            End Set
        End Property

        Private _BookCarrOrderNumber As String = ""
        <DataMember()>
        Public Property BookCarrOrderNumber() As String
            Get
                Return Left(_BookCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = Left(value, 20)
            End Set
        End Property

        Private _BookCarrBLNumber As String = ""
        <DataMember()>
        Public Property BookCarrBLNumber() As String
            Get
                Return Left(_BookCarrBLNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrBLNumber = Left(value, 20)
            End Set
        End Property

        Private _BookCarrBookDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrBookDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrBookDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrBookDate = value
            End Set
        End Property

        Private _BookCarrBookTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrBookTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrBookTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrBookTime = value
            End Set
        End Property

        Private _BookCarrBookContact As String = ""
        <DataMember()>
        Public Property BookCarrBookContact() As String
            Get
                Return Left(_BookCarrBookContact, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrBookContact = Left(value, 50)
            End Set
        End Property

        Private _BookCarrScheduleDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrScheduleDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleDate = value
            End Set
        End Property

        Private _BookCarrScheduleTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrScheduleTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrScheduleTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrScheduleTime = value
            End Set
        End Property

        Private _BookCarrActualDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActualDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActualDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActualDate = value
            End Set
        End Property

        Private _BookCarrActualTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActualTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActualTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActualTime = value
            End Set
        End Property

        Private _BookCarrActLoadComplete_Date As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActLoadComplete_Date() As System.Nullable(Of Date)
            Get
                Return _BookCarrActLoadComplete_Date
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActLoadComplete_Date = value
            End Set
        End Property

        Private _BookCarrActLoadCompleteTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActLoadCompleteTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActLoadCompleteTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActLoadCompleteTime = value
            End Set
        End Property

        Private _BookCarrDockPUAssigment As String = ""
        <DataMember()>
        Public Property BookCarrDockPUAssigment() As String
            Get
                Return Left(_BookCarrDockPUAssigment, 10)
            End Get
            Set(ByVal value As String)
                _BookCarrDockPUAssigment = Left(value, 10)
            End Set
        End Property

        Private _BookCarrPODate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrPODate() As System.Nullable(Of Date)
            Get
                Return _BookCarrPODate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrPODate = value
            End Set
        End Property

        Private _BookCarrPOTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrPOTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrPOTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrPOTime = value
            End Set
        End Property

        Private _BookCarrApptDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrApptDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptDate = value
            End Set
        End Property

        Private _BookCarrApptTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrApptTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrApptTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrApptTime = value
            End Set
        End Property

        Private _BookCarrActDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActDate = value
            End Set
        End Property

        Private _BookCarrActTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActTime = value
            End Set
        End Property

        Private _BookCarrActUnloadCompDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActUnloadCompDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrActUnloadCompDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActUnloadCompDate = value
            End Set
        End Property

        Private _BookCarrActUnloadCompTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrActUnloadCompTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrActUnloadCompTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrActUnloadCompTime = value
            End Set
        End Property

        Private _BookCarrDockDelAssignment As String = ""
        <DataMember()>
        Public Property BookCarrDockDelAssignment() As String
            Get
                Return Left(_BookCarrDockDelAssignment, 10)
            End Get
            Set(ByVal value As String)
                _BookCarrDockDelAssignment = Left(value, 10)
            End Set
        End Property

        Private _BookCarrVarDay As Integer = 0
        <DataMember()>
        Public Property BookCarrVarDay() As Integer
            Get
                Return _BookCarrVarDay
            End Get
            Set(ByVal value As Integer)
                _BookCarrVarDay = value
            End Set
        End Property

        Private _BookCarrVarHrs As Integer = 0
        <DataMember()>
        Public Property BookCarrVarHrs() As Integer
            Get
                Return _BookCarrVarHrs
            End Get
            Set(ByVal value As Integer)
                _BookCarrVarHrs = value
            End Set
        End Property

        Private _BookCarrTrailerNo As String = ""
        <DataMember()>
        Public Property BookCarrTrailerNo() As String
            Get
                Return Left(_BookCarrTrailerNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrTrailerNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrSealNo As String = ""
        <DataMember()>
        Public Property BookCarrSealNo() As String
            Get
                Return Left(_BookCarrSealNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrSealNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrDriverNo As String = ""
        <DataMember()>
        Public Property BookCarrDriverNo() As String
            Get
                Return Left(_BookCarrDriverNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrDriverNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrDriverName As String = ""
        <DataMember()>
        Public Property BookCarrDriverName() As String
            Get
                Return Left(_BookCarrDriverName, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrDriverName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrRouteNo As String = ""
        <DataMember()>
        Public Property BookCarrRouteNo() As String
            Get
                Return Left(_BookCarrRouteNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrRouteNo = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTripNo As String = ""
        <DataMember()>
        Public Property BookCarrTripNo() As String
            Get
                Return Left(_BookCarrTripNo, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrTripNo = Left(value, 50)
            End Set
        End Property

        Private _BookFinARBookFrt As Decimal = 0
        <DataMember()>
        Public Property BookFinARBookFrt() As Decimal
            Get
                Return _BookFinARBookFrt
            End Get
            Set(ByVal value As Decimal)
                _BookFinARBookFrt = value
            End Set
        End Property

        Private _BookFinARInvoiceDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinARInvoiceDate() As System.Nullable(Of Date)
            Get
                Return _BookFinARInvoiceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinARInvoiceDate = value
            End Set
        End Property

        Private _BookFinARInvoiceAmt As Decimal = 0
        <DataMember()>
        Public Property BookFinARInvoiceAmt() As Decimal
            Get
                Return _BookFinARInvoiceAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinARInvoiceAmt = value
            End Set
        End Property

        Private _BookFinARPayDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinARPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinARPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinARPayDate = value
            End Set
        End Property

        Private _BookFinARPayAmt As Decimal = 0
        <DataMember()>
        Public Property BookFinARPayAmt() As Decimal
            Get
                Return _BookFinARPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinARPayAmt = value
            End Set
        End Property

        Private _BookFinARCheck As String = ""
        <DataMember()>
        Public Property BookFinARCheck() As String
            Get
                Return Left(_BookFinARCheck, 50)
            End Get
            Set(ByVal value As String)
                _BookFinARCheck = Left(value, 50)
            End Set
        End Property

        Private _BookFinARGLNumber As String = ""
        <DataMember()>
        Public Property BookFinARGLNumber() As String
            Get
                Return Left(_BookFinARGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinARGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinARBalance As Decimal = 0
        <DataMember()>
        Public Property BookFinARBalance() As Decimal
            Get
                Return _BookFinARBalance
            End Get
            Set(ByVal value As Decimal)
                _BookFinARBalance = value
            End Set
        End Property

        Private _BookFinARCurType As Integer = 0
        <DataMember()>
        Public Property BookFinARCurType() As Integer
            Get
                Return _BookFinARCurType
            End Get
            Set(ByVal value As Integer)
                _BookFinARCurType = value
            End Set
        End Property

        Private _BookFinAPBillNumber As String = ""
        <DataMember()>
        Public Property BookFinAPBillNumber() As String
            Get
                Return Left(_BookFinAPBillNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinAPBillNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinAPBillNoDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinAPBillNoDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPBillNoDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPBillNoDate = value
            End Set
        End Property

        Private _BookFinAPBillInvDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinAPBillInvDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPBillInvDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPBillInvDate = value
            End Set
        End Property

        Private _BookFinAPActWgt As Integer = 0
        <DataMember()>
        Public Property BookFinAPActWgt() As Integer
            Get
                Return _BookFinAPActWgt
            End Get
            Set(ByVal value As Integer)
                _BookFinAPActWgt = value
            End Set
        End Property

        Private _BookFinAPStdCost As Decimal = 0
        <DataMember()>
        Public Property BookFinAPStdCost() As Decimal
            Get
                Return _BookFinAPStdCost
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPStdCost = value
            End Set
        End Property

        Private _BookFinAPActCost As Decimal = 0
        <DataMember()>
        Public Property BookFinAPActCost() As Decimal
            Get
                Return _BookFinAPActCost
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPActCost = value
            End Set
        End Property

        Private _BookFinAPPayDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinAPPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPPayDate = value
            End Set
        End Property

        Private _BookFinAPPayAmt As Decimal = 0
        <DataMember()>
        Public Property BookFinAPPayAmt() As Decimal
            Get
                Return _BookFinAPPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPPayAmt = value
            End Set
        End Property

        Private _BookFinAPCheck As String = ""
        <DataMember()>
        Public Property BookFinAPCheck() As String
            Get
                Return Left(_BookFinAPCheck, 15)
            End Get
            Set(ByVal value As String)
                _BookFinAPCheck = Left(value, 15)
            End Set
        End Property

        Private _BookFinAPGLNumber As String = ""
        <DataMember()>
        Public Property BookFinAPGLNumber() As String
            Get
                Return Left(_BookFinAPGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinAPGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinAPLastViewed As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinAPLastViewed() As System.Nullable(Of Date)
            Get
                Return _BookFinAPLastViewed
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPLastViewed = value
            End Set
        End Property

        Private _BookFinAPCurType As Integer = 0
        <DataMember()>
        Public Property BookFinAPCurType() As Integer
            Get
                Return _BookFinAPCurType
            End Get
            Set(ByVal value As Integer)
                _BookFinAPCurType = value
            End Set
        End Property

        Private _BookFinCommStd As Decimal = 0
        <DataMember()>
        Public Property BookFinCommStd() As Decimal
            Get
                Return _BookFinCommStd
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommStd = value
            End Set
        End Property

        Private _BookFinCommAct As Decimal = 0
        <DataMember()>
        Public Property BookFinCommAct() As Decimal
            Get
                Return _BookFinCommAct
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommAct = value
            End Set
        End Property

        Private _BookFinCommPayDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinCommPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinCommPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinCommPayDate = value
            End Set
        End Property

        Private _BookFinCommPayAmt As Decimal = 0
        <DataMember()>
        Public Property BookFinCommPayAmt() As Decimal
            Get
                Return _BookFinCommPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommPayAmt = value
            End Set
        End Property

        Private _BookFinCommtCheck As String = ""
        <DataMember()>
        Public Property BookFinCommtCheck() As String
            Get
                Return Left(_BookFinCommtCheck, 15)
            End Get
            Set(ByVal value As String)
                _BookFinCommtCheck = Left(value, 15)
            End Set
        End Property

        Private _BookFinCommCreditAmt As Decimal = 0
        <DataMember()>
        Public Property BookFinCommCreditAmt() As Decimal
            Get
                Return _BookFinCommCreditAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommCreditAmt = value
            End Set
        End Property

        Private _BookFinCommCreditPayDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinCommCreditPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinCommCreditPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinCommCreditPayDate = value
            End Set
        End Property

        Private _BookFinCommLoadCount As Integer = 0
        <DataMember()>
        Public Property BookFinCommLoadCount() As Integer
            Get
                Return _BookFinCommLoadCount
            End Get
            Set(ByVal value As Integer)
                _BookFinCommLoadCount = value
            End Set
        End Property

        Private _BookFinCommGLNumber As String = ""
        <DataMember()>
        Public Property BookFinCommGLNumber() As String
            Get
                Return Left(_BookFinCommGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCommGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinCheckClearedDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinCheckClearedDate() As System.Nullable(Of Date)
            Get
                Return _BookFinCheckClearedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinCheckClearedDate = value
            End Set
        End Property

        Private _BookFinCheckClearedNumber As String = ""
        <DataMember()>
        Public Property BookFinCheckClearedNumber() As String
            Get
                Return Left(_BookFinCheckClearedNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCheckClearedNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinCheckClearedAmt As Decimal = 0
        <DataMember()>
        Public Property BookFinCheckClearedAmt() As Decimal
            Get
                Return _BookFinCheckClearedAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinCheckClearedAmt = value
            End Set
        End Property

        Private _BookFinCheckClearedDesc As String = ""
        <DataMember()>
        Public Property BookFinCheckClearedDesc() As String
            Get
                Return Left(_BookFinCheckClearedDesc, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCheckClearedDesc = Left(value, 50)
            End Set
        End Property

        Private _BookFinCheckClearedAcct As String = ""
        <DataMember()>
        Public Property BookFinCheckClearedAcct() As String
            Get
                Return Left(_BookFinCheckClearedAcct, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCheckClearedAcct = Left(value, 50)
            End Set
        End Property

        Private _BookRevBilledBFC As Decimal = 0
        <DataMember()>
        Public Property BookRevBilledBFC() As Decimal
            Get
                Return _BookRevBilledBFC
            End Get
            Set(ByVal value As Decimal)
                _BookRevBilledBFC = value
            End Set
        End Property

        Private _BookRevCarrierCost As Decimal = 0
        <DataMember()>
        Public Property BookRevCarrierCost() As Decimal
            Get
                Return _BookRevCarrierCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevCarrierCost = value
            End Set
        End Property

        Private _BookRevStopQty As Integer = 0
        <DataMember()>
        Public Property BookRevStopQty() As Integer
            Get
                Return _BookRevStopQty
            End Get
            Set(ByVal value As Integer)
                _BookRevStopQty = value
            End Set
        End Property

        Private _BookRevStopCost As Decimal = 0
        <DataMember()>
        Public Property BookRevStopCost() As Decimal
            Get
                Return _BookRevStopCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevStopCost = value
            End Set
        End Property

        Private _BookRevOtherCost As Decimal = 0
        <DataMember()>
        Public Property BookRevOtherCost() As Decimal
            Get
                Return _BookRevOtherCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevOtherCost = value
            End Set
        End Property

        Private _BookRevTotalCost As Decimal = 0
        <DataMember()>
        Public Property BookRevTotalCost() As Decimal
            Get
                Return _BookRevTotalCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevTotalCost = value
            End Set
        End Property

        Private _BookRevLoadSavings As Decimal = 0
        <DataMember()>
        Public Property BookRevLoadSavings() As Decimal
            Get
                Return _BookRevLoadSavings
            End Get
            Set(ByVal value As Decimal)
                _BookRevLoadSavings = value
            End Set
        End Property

        Private _BookRevCommPercent As Double = 0
        <DataMember()>
        Public Property BookRevCommPercent() As Double
            Get
                Return _BookRevCommPercent
            End Get
            Set(ByVal value As Double)
                _BookRevCommPercent = value
            End Set
        End Property

        Private _BookRevCommCost As Decimal = 0
        <DataMember()>
        Public Property BookRevCommCost() As Decimal
            Get
                Return _BookRevCommCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevCommCost = value
            End Set
        End Property

        Private _BookRevGrossRevenue As Decimal = 0
        <DataMember()>
        Public Property BookRevGrossRevenue() As Decimal
            Get
                Return _BookRevGrossRevenue
            End Get
            Set(ByVal value As Decimal)
                _BookRevGrossRevenue = value
            End Set
        End Property

        Private _BookRevNegRevenue As Integer = 0
        <DataMember()>
        Public Property BookRevNegRevenue() As Integer
            Get
                Return _BookRevNegRevenue
            End Get
            Set(ByVal value As Integer)
                _BookRevNegRevenue = value
            End Set
        End Property

        Private _BookMilesFrom As Double = 0
        <DataMember()>
        Public Property BookMilesFrom() As Double
            Get
                Return _BookMilesFrom
            End Get
            Set(ByVal value As Double)
                _BookMilesFrom = value
            End Set
        End Property

        Private _BookLaneCarrControl As Integer = 0
        <DataMember()>
        Public Property BookLaneCarrControl() As Integer
            Get
                Return _BookLaneCarrControl
            End Get
            Set(ByVal value As Integer)
                _BookLaneCarrControl = value
            End Set
        End Property

        Private _BookHoldLoad As Integer = 0
        <DataMember()>
        Public Property BookHoldLoad() As Integer
            Get
                Return _BookHoldLoad
            End Get
            Set(ByVal value As Integer)
                _BookHoldLoad = value
            End Set
        End Property

        Private _BookRouteFinalDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookRouteFinalDate() As System.Nullable(Of Date)
            Get
                Return _BookRouteFinalDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookRouteFinalDate = value
            End Set
        End Property

        Private _BookRouteFinalCode As String = ""
        <DataMember()>
        Public Property BookRouteFinalCode() As String
            Get
                Return Left(_BookRouteFinalCode, 2)
            End Get
            Set(ByVal value As String)
                _BookRouteFinalCode = Left(value, 2)
            End Set
        End Property

        Private _BookRouteFinalFlag As Boolean = False
        <DataMember()>
        Public Property BookRouteFinalFlag() As Boolean
            Get
                Return _BookRouteFinalFlag
            End Get
            Set(ByVal value As Boolean)
                _BookRouteFinalFlag = value
            End Set
        End Property

        Private _BookWarehouseNumber As String = ""
        <DataMember()>
        Public Property BookWarehouseNumber() As String
            Get
                Return Left(_BookWarehouseNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookWarehouseNumber = Left(value, 20)
            End Set
        End Property

        Private _BookComCode As String = ""
        <DataMember()>
        Public Property BookComCode() As String
            Get
                Return Left(_BookComCode, 3)
            End Get
            Set(ByVal value As String)
                _BookComCode = Left(value, 3)
            End Set
        End Property

        Private _BookTransType As String = ""
        <DataMember()>
        Public Property BookTransType() As String
            Get
                Return Left(_BookTransType, 50)
            End Get
            Set(ByVal value As String)
                _BookTransType = Left(value, 50)
            End Set
        End Property

        Private _BookRouteConsFlag As Boolean = False
        <DataMember()>
        Public Property BookRouteConsFlag() As Boolean
            Get
                Return _BookRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                _BookRouteConsFlag = value
            End Set
        End Property

        Private _BookWhseAuthorizationNo As String = ""
        <DataMember()>
        Public Property BookWhseAuthorizationNo() As String
            Get
                Return Left(_BookWhseAuthorizationNo, 20)
            End Get
            Set(ByVal value As String)
                _BookWhseAuthorizationNo = Left(value, 20)
            End Set
        End Property

        Private _BookHotLoad As Boolean = False
        <DataMember()>
        Public Property BookHotLoad() As Boolean
            Get
                Return _BookHotLoad
            End Get
            Set(ByVal value As Boolean)
                _BookHotLoad = value
            End Set
        End Property

        Private _BookFinAPActTax As Decimal = 0
        <DataMember()>
        Public Property BookFinAPActTax() As Decimal
            Get
                Return _BookFinAPActTax
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPActTax = value
            End Set
        End Property

        Private _BookFinAPExportFlag As Boolean = False
        <DataMember()>
        Public Property BookFinAPExportFlag() As Boolean
            Get
                Return _BookFinAPExportFlag
            End Get
            Set(ByVal value As Boolean)
                _BookFinAPExportFlag = value
            End Set
        End Property

        Private _BookFinARFreightTax As Decimal = 0
        <DataMember()>
        Public Property BookFinARFreightTax() As Decimal
            Get
                Return _BookFinARFreightTax
            End Get
            Set(ByVal value As Decimal)
                _BookFinARFreightTax = value
            End Set
        End Property

        Private _BookRevFreightTax As Decimal = 0
        <DataMember()>
        Public Property BookRevFreightTax() As Decimal
            Get
                Return _BookRevFreightTax
            End Get
            Set(ByVal value As Decimal)
                _BookRevFreightTax = value
            End Set
        End Property

        Private _BookRevNetCost As Decimal = 0
        <DataMember()>
        Public Property BookRevNetCost() As Decimal
            Get
                Return _BookRevNetCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevNetCost = value
            End Set
        End Property

        Private _BookFinServiceFee As Decimal = 0
        <DataMember()>
        Public Property BookFinServiceFee() As Decimal
            Get
                Return _BookFinServiceFee
            End Get
            Set(ByVal value As Decimal)
                _BookFinServiceFee = value
            End Set
        End Property

        Private _BookFinAPExportDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookFinAPExportDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPExportDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPExportDate = value
            End Set
        End Property

        Private _BookFinAPExportRetry As Integer = 0
        <DataMember()>
        Public Property BookFinAPExportRetry() As Integer
            Get
                Return _BookFinAPExportRetry
            End Get
            Set(ByVal value As Integer)
                _BookFinAPExportRetry = value
            End Set
        End Property

        Private _BookCarrierContControl As Integer = 0
        <DataMember()>
        Public Property BookCarrierContControl() As Integer
            Get
                Return _BookCarrierContControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrierContControl = value
            End Set
        End Property

        Private _BookHotLoadSent As Boolean = False
        <DataMember()>
        Public Property BookHotLoadSent() As Boolean
            Get
                Return _BookHotLoadSent
            End Get
            Set(ByVal value As Boolean)
                _BookHotLoadSent = value
            End Set
        End Property

        Private _BookExportDocCreated As Boolean = False
        <DataMember()>
        Public Property BookExportDocCreated() As Boolean
            Get
                Return _BookExportDocCreated
            End Get
            Set(ByVal value As Boolean)
                _BookExportDocCreated = value
            End Set
        End Property

        Private _BookDoNotInvoice As Boolean = False
        <DataMember()>
        Public Property BookDoNotInvoice() As Boolean
            Get
                Return _BookDoNotInvoice
            End Get
            Set(ByVal value As Boolean)
                _BookDoNotInvoice = value
            End Set
        End Property

        Private _BookCarrStartLoadingDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrStartLoadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartLoadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartLoadingDate = value
            End Set
        End Property

        Private _BookCarrStartLoadingTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrStartLoadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartLoadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartLoadingTime = value
            End Set
        End Property

        Private _BookCarrFinishLoadingDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrFinishLoadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishLoadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishLoadingDate = value
            End Set
        End Property

        Private _BookCarrFinishLoadingTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrFinishLoadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishLoadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishLoadingTime = value
            End Set
        End Property

        Private _BookCarrStartUnloadingDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrStartUnloadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartUnloadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartUnloadingDate = value
            End Set
        End Property

        Private _BookCarrStartUnloadingTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrStartUnloadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrStartUnloadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrStartUnloadingTime = value
            End Set
        End Property

        Private _BookCarrFinishUnloadingDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrFinishUnloadingDate() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishUnloadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishUnloadingDate = value
            End Set
        End Property

        Private _BookCarrFinishUnloadingTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrFinishUnloadingTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrFinishUnloadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrFinishUnloadingTime = value
            End Set
        End Property

        Private _BookOrderSequence As Integer = 0
        <DataMember()>
        Public Property BookOrderSequence() As Integer
            Get
                Return _BookOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookOrderSequence = value
            End Set
        End Property

        Private _BookChepGLID As String = ""
        <DataMember()>
        Public Property BookChepGLID() As String
            Get
                Return Left(_BookChepGLID, 50)
            End Get
            Set(ByVal value As String)
                _BookChepGLID = Left(value, 50)
            End Set
        End Property

        Private _BookCarrierTypeCode As String = ""
        <DataMember()>
        Public Property BookCarrierTypeCode() As String
            Get
                Return Left(_BookCarrierTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrierTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookPalletPositions As String = ""
        <DataMember()>
        Public Property BookPalletPositions() As String
            Get
                Return Left(_BookPalletPositions, 50)
            End Get
            Set(ByVal value As String)
                _BookPalletPositions = Left(value, 50)
            End Set
        End Property

        Private _BookShipCarrierProNumber As String = ""
        <DataMember()>
        Public Property BookShipCarrierProNumber() As String
            Get
                Return Left(_BookShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookShipCarrierProNumberRaw As String = ""
        <DataMember()>
        Public Property BookShipCarrierProNumberRaw() As String
            Get
                Return Left(_BookShipCarrierProNumberRaw, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumberRaw = Left(value, 20)
            End Set
        End Property

        Private _BookShipCarrierProControl As System.Nullable(Of Integer)
        <DataMember()>
        Public Property BookShipCarrierProControl() As System.Nullable(Of Integer)
            Get
                Return _BookShipCarrierProControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookShipCarrierProControl = value
            End Set
        End Property

        Private _BookShipCarrierName As String = ""
        <DataMember()>
        Public Property BookShipCarrierName() As String
            Get
                Return Left(_BookShipCarrierName, 60)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierName = Left(value, 60)
            End Set
        End Property

        Private _BookShipCarrierNumber As String = ""
        <DataMember()>
        Public Property BookShipCarrierNumber() As String
            Get
                Return Left(_BookShipCarrierNumber, 80)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierNumber = Left(value, 80)
            End Set
        End Property

        Private _BookAPAdjReasonControl As Integer = 0
        <DataMember()>
        Public Property BookAPAdjReasonControl() As Integer
            Get
                Return _BookAPAdjReasonControl
            End Get
            Set(ByVal value As Integer)
                _BookAPAdjReasonControl = value
            End Set
        End Property

        Private _BookDateRequested As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookDateRequested() As System.Nullable(Of Date)
            Get
                Return _BookDateRequested
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateRequested = value
            End Set
        End Property

        Private _BookCarrierEquipmentCodes As String = ""
        <DataMember()>
        Public Property BookCarrierEquipmentCodes() As String
            Get
                Return Left(_BookCarrierEquipmentCodes, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrierEquipmentCodes = Left(value, 50)
            End Set
        End Property

        Private _BookLockAllCosts As Boolean = False
        <DataMember()>
        Public Property BookLockAllCosts() As Boolean
            Get
                Return _BookLockAllCosts
            End Get
            Set(ByVal value As Boolean)
                _BookLockAllCosts = value
            End Set
        End Property

        Private _BookLockBFCCost As Boolean = False
        <DataMember()>
        Public Property BookLockBFCCost() As Boolean
            Get
                Return _BookLockBFCCost
            End Get
            Set(ByVal value As Boolean)
                _BookLockBFCCost = value
            End Set
        End Property

        Private _BookDestStopNumber As Integer = 0
        <DataMember()>
        Public Property BookDestStopNumber() As Integer
            Get
                Return _BookDestStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookDestStopNumber = value
            End Set
        End Property

        Private _BookOrigStopNumber As Integer = 0
        <DataMember()>
        Public Property BookOrigStopNumber() As Integer
            Get
                Return _BookOrigStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookOrigStopNumber = value
            End Set
        End Property

        Private _BookOrigStopControl As Integer = 0
        <DataMember()>
        Public Property BookOrigStopControl() As Integer
            Get
                Return _BookOrigStopControl
            End Get
            Set(ByVal value As Integer)
                _BookOrigStopControl = value
            End Set
        End Property

        Private _BookDestStopControl As Integer = 0
        <DataMember()>
        Public Property BookDestStopControl() As Integer
            Get
                Return _BookDestStopControl
            End Get
            Set(ByVal value As Integer)
                _BookDestStopControl = value
            End Set
        End Property

        Private _BookRouteTypeCode As Integer = 6
        <DataMember()>
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
        <DataMember()>
        Public Property BookAlternateAddressLaneControl() As Integer
            Get
                Return _BookAlternateAddressLaneControl
            End Get
            Set(ByVal value As Integer)
                _BookAlternateAddressLaneControl = value
            End Set
        End Property

        Private _BookAlternateAddressLaneNumber As String = ""
        <DataMember()>
        Public Property BookAlternateAddressLaneNumber() As String
            Get
                Return Left(_BookAlternateAddressLaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAlternateAddressLaneNumber = Left(value, 50)
            End Set
        End Property

        Private _BookDefaultRouteSequence As Integer = 0
        <DataMember()>
        Public Property BookDefaultRouteSequence() As Integer
            Get
                Return _BookDefaultRouteSequence
            End Get
            Set(ByVal value As Integer)
                _BookDefaultRouteSequence = value
            End Set
        End Property

        Private _BookRouteGuideControl As Integer = 0
        <DataMember()>
        Public Property BookRouteGuideControl() As Integer
            Get
                Return _BookRouteGuideControl
            End Get
            Set(ByVal value As Integer)
                _BookRouteGuideControl = value
            End Set
        End Property

        Private _BookRouteGuideNumber As String = ""
        <DataMember()>
        Public Property BookRouteGuideNumber() As String
            Get
                Return Left(_BookRouteGuideNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookRouteGuideNumber = Left(value, 50)
            End Set
        End Property

        Private _BookCustomerApprovalTransmitted As Boolean = False
        <DataMember()>
        Public Property BookCustomerApprovalTransmitted() As Boolean
            Get
                Return _BookCustomerApprovalTransmitted
            End Get
            Set(ByVal value As Boolean)
                _BookCustomerApprovalTransmitted = value
            End Set
        End Property

        Private _BookCustomerApprovalRecieved As Boolean = False
        <DataMember()>
        Public Property BookCustomerApprovalRecieved() As Boolean
            Get
                Return _BookCustomerApprovalRecieved
            End Get
            Set(ByVal value As Boolean)
                _BookCustomerApprovalRecieved = value
            End Set
        End Property

        Private _BookAMSPickupApptControl As Integer = 0
        <DataMember()>
        Public Property BookAMSPickupApptControl As Integer
            Get
                Return _BookAMSPickupApptControl
            End Get
            Set(value As Integer)
                _BookAMSPickupApptControl = value
            End Set
        End Property

        Private _BookAMSDeliveryApptControl As Integer = 0
        <DataMember()>
        Public Property BookAMSDeliveryApptControl As Integer
            Get
                Return _BookAMSDeliveryApptControl
            End Get
            Set(value As Integer)
                _BookAMSDeliveryApptControl = value
            End Set
        End Property

        Private _BookItemDetailDescription As String
        <DataMember()>
        Public Property BookItemDetailDescription As String
            Get
                Return Left(_BookItemDetailDescription, 4000)
            End Get
            Set(value As String)
                _BookItemDetailDescription = Left(value, 4000)
            End Set
        End Property

        Private _BookCarrTruckControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTruckControl() As Integer
            Get
                Return _BookCarrTruckControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTruckControl = value
            End Set
        End Property

        Private _BookCarrTarControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTarControl() As Integer
            Get
                Return _BookCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarControl = value
            End Set
        End Property

        Private _BookCarrTarRevisionNumber As Integer = 0
        <DataMember()>
        Public Property BookCarrTarRevisionNumber() As Integer
            Get
                Return _BookCarrTarRevisionNumber
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarRevisionNumber = value
            End Set
        End Property

        Private _BookCarrTarName As String
        <DataMember()>
        Public Property BookCarrTarName As String
            Get
                Return Left(_BookCarrTarName, 50)
            End Get
            Set(value As String)
                _BookCarrTarName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTarEquipControl() As Integer
            Get
                Return _BookCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipControl = value
            End Set
        End Property

        Private _BookCarrTarEquipName As String
        <DataMember()>
        Public Property BookCarrTarEquipName As String
            Get
                Return Left(_BookCarrTarEquipName, 50)
            End Get
            Set(value As String)
                _BookCarrTarEquipName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipMatControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTarEquipMatControl() As Integer
            Get
                Return _BookCarrTarEquipMatControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatControl = value
            End Set
        End Property

        Private _BookCarrTarEquipMatName As String
        <DataMember()>
        Public Property BookCarrTarEquipMatName As String
            Get
                Return Left(_BookCarrTarEquipMatName, 50)
            End Get
            Set(value As String)
                _BookCarrTarEquipMatName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipMatDetControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTarEquipMatDetControl() As Integer
            Get
                Return _BookCarrTarEquipMatDetControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatDetControl = value
            End Set
        End Property

        Private _BookCarrTarEquipMatDetID As Integer = 0
        <DataMember()>
        Public Property BookCarrTarEquipMatDetID() As Integer
            Get
                Return _BookCarrTarEquipMatDetID
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatDetID = value
            End Set
        End Property

        Private _BookCarrTarEquipMatDetValue As System.Nullable(Of Decimal)
        <DataMember()>
        Public Property BookCarrTarEquipMatDetValue() As System.Nullable(Of Decimal)
            Get
                Return _BookCarrTarEquipMatDetValue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _BookCarrTarEquipMatDetValue = value
            End Set
        End Property

        Private _BookBookRevHistRevision As Integer = 0
        <DataMember()>
        Public Property BookBookRevHistRevision() As Integer
            Get
                Return _BookBookRevHistRevision
            End Get
            Set(ByVal value As Integer)
                _BookBookRevHistRevision = value
            End Set
        End Property

        Private _BookModeTypeControl As Integer = 0
        <DataMember()>
        Public Property BookModeTypeControl() As Integer
            Get
                Return _BookModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookModeTypeControl = value
            End Set
        End Property

        Private _BookAllowInterlinePoints As Boolean = True
        <DataMember()>
        Public Property BookAllowInterlinePoints() As Boolean
            Get
                Return _BookAllowInterlinePoints
            End Get
            Set(ByVal value As Boolean)
                _BookAllowInterlinePoints = value
            End Set
        End Property

        Private _BookRevLaneBenchMiles As System.Nullable(Of Double)
        <DataMember()>
        Public Property BookRevLaneBenchMiles() As System.Nullable(Of Double)
            Get
                Return _BookRevLaneBenchMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookRevLaneBenchMiles = value
            End Set
        End Property

        Private _BookRevLoadMiles As System.Nullable(Of Double)
        <DataMember()>
        Public Property BookRevLoadMiles() As System.Nullable(Of Double)
            Get
                Return _BookRevLoadMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookRevLoadMiles = value
            End Set
        End Property

        Private _BookUser1 As String = ""
        <DataMember()>
        Public Property BookUser1() As String
            Get
                Return Left(_BookUser1, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser1 = Left(value, 4000)
            End Set
        End Property

        Private _BookUser2 As String = ""
        <DataMember()>
        Public Property BookUser2() As String
            Get
                Return Left(_BookUser2, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser2 = Left(value, 4000)
            End Set
        End Property

        Private _BookUser3 As String = ""
        <DataMember()>
        Public Property BookUser3() As String
            Get
                Return Left(_BookUser3, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser3 = Left(value, 4000)
            End Set
        End Property

        Private _BookUser4 As String = ""
        <DataMember()>
        Public Property BookUser4() As String
            Get
                Return Left(_BookUser4, 4000)
            End Get
            Set(ByVal value As String)
                _BookUser4 = Left(value, 4000)
            End Set
        End Property

        Private _BookRevDiscount As Decimal = 0
        <DataMember()>
        Public Property BookRevDiscount() As Decimal
            Get
                Return _BookRevDiscount
            End Get
            Set(ByVal value As Decimal)
                _BookRevDiscount = value
            End Set
        End Property

        Private _BookRevLineHaul As Decimal = 0
        <DataMember()>
        Public Property BookRevLineHaul() As Decimal
            Get
                Return _BookRevLineHaul
            End Get
            Set(ByVal value As Decimal)
                _BookRevLineHaul = value
            End Set
        End Property

        Private _BookMustLeaveByDateTime As System.Nullable(Of Date)
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Logic used to calcualte the Must Leave By Date and Time was 
        '''  Modified by RHR for v-8.5.3.006 on 11/16/2022
        '''  This logic is not updated using the new Lead Time calculation factors
        '''  These factors adjust value in hours and use US time zones and day light saving time where available
        '''  For outbound loads the Lane pickup information will override the CompTimeZone if available or different
        '''     the LaneRecHourStart and LaneRecHourStop will override the workflow settings
        '''         (a) "Alert Start of Business Hour" (EmailAlertStartOfBusinessDay) -- Default when lane is not configured
        '''         (b) and "Alert End of Business Hour" (EmailAlertEndOfBusinessDay) -- Default when lane is not configured
        '''  For inbound loads the Lane delivery information will override the CompTimeZone if available or different
        '''     the LaneDestHourStart and LaneDestHourStop will override the workflow settings 
        '''         (a) "Alert Start of Business Hour" (EmailAlertStartOfBusinessDay) -- Default wn hen lane is not configured
        '''         (b) and "Alert End of Business Hour" (EmailAlertEndOfBusinessDay) -- Default when lane is not configured  
        ''' </remarks>
        <DataMember()>
        Public Property BookMustLeaveByDateTime() As System.Nullable(Of Date)
            Get
                Return _BookMustLeaveByDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookMustLeaveByDateTime = value
            End Set
        End Property

        Private _BookMultiMode As Boolean = False
        <DataMember()>
        Public Property BookMultiMode() As Boolean
            Get
                Return _BookMultiMode
            End Get
            Set(ByVal value As Boolean)
                _BookMultiMode = value
            End Set
        End Property

        Private _BookOriginalLaneControl As Integer = 0
        <DataMember()>
        Public Property BookOriginalLaneControl() As Integer
            Get
                Return _BookOriginalLaneControl
            End Get
            Set(ByVal value As Integer)
                _BookOriginalLaneControl = value
            End Set
        End Property

        Private _BookLaneTranXControl As Integer = 0
        <DataMember()>
        Public Property BookLaneTranXControl() As Integer
            Get
                Return _BookLaneTranXControl
            End Get
            Set(ByVal value As Integer)
                _BookLaneTranXControl = value
            End Set
        End Property

        Private _BookLaneTranXDetControl As Integer = 0
        <DataMember()>
        Public Property BookLaneTranXDetControl() As Integer
            Get
                Return _BookLaneTranXDetControl
            End Get
            Set(ByVal value As Integer)
                _BookLaneTranXDetControl = value
            End Set
        End Property

        Private _BookSHID As String
        <DataMember()>
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
        <DataMember()>
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

        Private _BookExpDelDateTime As Date?
        <DataMember()>
        Public Property BookExpDelDateTime() As Date?
            Get
                Return _BookExpDelDateTime
            End Get
            Set(ByVal value As Date?)
                _BookExpDelDateTime = value
            End Set
        End Property

        Private _BookOutOfRouteMiles As Double
        <DataMember()>
        Public Property BookOutOfRouteMiles() As Double
            Get
                Return _BookOutOfRouteMiles
            End Get
            Set(ByVal value As Double)
                _BookOutOfRouteMiles = value
            End Set
        End Property


        Private _BookLoads As List(Of BookLoad)
        <DataMember()>
        Public Property BookLoads() As List(Of BookLoad)
            Get
                Return _BookLoads
            End Get
            Set(ByVal value As List(Of BookLoad))
                _BookLoads = value
            End Set
        End Property

        Private _BookNotes As List(Of BookNote)
        <DataMember()>
        Public Property BookNotes() As List(Of BookNote)
            Get
                Return _BookNotes
            End Get
            Set(ByVal value As List(Of BookNote))
                _BookNotes = value
            End Set
        End Property

        Private _BookTracks As List(Of BookTrack)
        <DataMember()>
        Public Property BookTracks() As List(Of BookTrack)
            Get
                Return _BookTracks
            End Get
            Set(ByVal value As List(Of BookTrack))
                _BookTracks = value
            End Set
        End Property

        Private _BookCreditHold As Boolean = False
        <DataMember()>
        Public Property BookCreditHold() As Boolean
            Get
                Return _BookCreditHold
            End Get
            Set(ByVal value As Boolean)
                _BookCreditHold = value
            End Set
        End Property

        Private _BookBestDeficitCost As Decimal
        <DataMember()>
        Public Property BookBestDeficitCost() As Decimal
            Get
                Return _BookBestDeficitCost
            End Get
            Set(ByVal value As Decimal)
                _BookBestDeficitCost = value
            End Set
        End Property

        Private _BookBestDeficitWeight As Double
        <DataMember()>
        Public Property BookBestDeficitWeight() As Double
            Get
                Return _BookBestDeficitWeight
            End Get
            Set(ByVal value As Double)
                _BookBestDeficitWeight = value
            End Set
        End Property

        Private _BookBestDeficitWeightBreak As Double
        <DataMember()>
        Public Property BookBestDeficitWeightBreak() As Double
            Get
                Return _BookBestDeficitWeightBreak
            End Get
            Set(ByVal value As Double)
                _BookBestDeficitWeightBreak = value
            End Set
        End Property


        Private _BookRatedWeightBreak As Double
        <DataMember()>
        Public Property BookRatedWeightBreak() As Double
            Get
                Return _BookRatedWeightBreak
            End Get
            Set(ByVal value As Double)
                _BookRatedWeightBreak = value
            End Set
        End Property

        Private _BookWgtAdjCost As Decimal
        <DataMember()>
        Public Property BookWgtAdjCost() As Decimal
            Get
                Return _BookWgtAdjCost
            End Get
            Set(ByVal value As Decimal)
                _BookWgtAdjCost = value
            End Set
        End Property

        Private _BookWgtAdjWeight As Double
        <DataMember()>
        Public Property BookWgtAdjWeight() As Double
            Get
                Return _BookWgtAdjWeight
            End Get
            Set(ByVal value As Double)
                _BookWgtAdjWeight = value
            End Set
        End Property

        Private _BookWgtAdjWeightBreak As Double
        <DataMember()>
        Public Property BookWgtAdjWeightBreak() As Double
            Get
                Return _BookWgtAdjWeightBreak
            End Get
            Set(ByVal value As Double)
                _BookWgtAdjWeightBreak = value
            End Set
        End Property



        Private _BookBilledLoadWeight As Double
        <DataMember()>
        Public Property BookBilledLoadWeight() As Double
            Get
                Return _BookBilledLoadWeight
            End Get
            Set(ByVal value As Double)
                _BookBilledLoadWeight = value
            End Set
        End Property

        Private _BookMinAdjustedLoadWeight As Double
        <DataMember()>
        Public Property BookMinAdjustedLoadWeight() As Double
            Get
                Return _BookMinAdjustedLoadWeight
            End Get
            Set(ByVal value As Double)
                _BookMinAdjustedLoadWeight = value
            End Set
        End Property

        Private _BookSummedClassWeight As Double
        <DataMember()>
        Public Property BookSummedClassWeight() As Double
            Get
                Return _BookSummedClassWeight
            End Get
            Set(ByVal value As Double)
                _BookSummedClassWeight = value
            End Set
        End Property

        Private _BookWgtRoundingVariance As Double
        <DataMember()>
        Public Property BookWgtRoundingVariance() As Double
            Get
                Return _BookWgtRoundingVariance
            End Get
            Set(ByVal value As Double)
                _BookWgtRoundingVariance = value
            End Set
        End Property

        Private _BookHeaviestClass As String
        <DataMember()>
        Public Property BookHeaviestClass() As String
            Get
                Return _BookHeaviestClass
            End Get
            Set(ByVal value As String)
                _BookHeaviestClass = value
            End Set
        End Property

        Private _BookAcutalHeaviestClassWeight As Double
        <DataMember()>
        Public Property BookAcutalHeaviestClassWeight() As Double
            Get
                Return _BookAcutalHeaviestClassWeight
            End Get
            Set(ByVal value As Double)
                _BookAcutalHeaviestClassWeight = value
            End Set
        End Property

        Private _BookRevDiscountRate As Decimal = 0
        <DataMember()>
        Public Property BookRevDiscountRate() As Decimal
            Get
                Return _BookRevDiscountRate
            End Get
            Set(ByVal value As Decimal)
                _BookRevDiscountRate = value
            End Set
        End Property

        Private _BookRevDiscountMin As Decimal = 0
        <DataMember()>
        Public Property BookRevDiscountMin() As Decimal
            Get
                Return _BookRevDiscountMin
            End Get
            Set(ByVal value As Decimal)
                _BookRevDiscountMin = value
            End Set
        End Property

        'Added by LVV 5/12/16 for v-7.0.5.1 DAT
        Private _BookRevLoadTenderTypeControl As Integer = 0
        <DataMember()>
        Public Property BookRevLoadTenderTypeControl() As Integer
            Get
                Return _BookRevLoadTenderTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookRevLoadTenderTypeControl = value
            End Set
        End Property

        'Added by LVV 6/14/16 for v-7.0.5.110 DAT
        Private _BookRevLoadTenderStatusCode As Integer = 0
        <DataMember()>
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
        <DataMember()>
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
        <DataMember()>
        Public Property BookRevPreferredCarrier() As Boolean
            Get
                Return _BookRevPreferredCarrier
            End Get
            Set(ByVal value As Boolean)
                _BookRevPreferredCarrier = value
            End Set
        End Property

        'Begin Modified by RHR for v-8.1 on 03/26/2018
        Private _BookOrigContactName As String = ""
        <DataMember()>
        Public Property BookOrigContactName() As String
            Get
                Return Left(_BookOrigContactName, 50)
            End Get
            Set(ByVal value As String)
                _BookOrigContactName = Left(value, 50)
            End Set
        End Property

        Private _BookOrigContactEmail As String = ""
        <DataMember()>
        Public Property BookOrigContactEmail() As String
            Get
                Return Left(_BookOrigContactEmail, 50)
            End Get
            Set(ByVal value As String)
                _BookOrigContactEmail = Left(value, 50)
            End Set
        End Property

        Private _BookOrigEmergencyContactPhone As String = ""
        <DataMember()>
        Public Property BookOrigEmergencyContactPhone() As String
            Get
                Return Left(_BookOrigEmergencyContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookOrigEmergencyContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookOrigEmergencyContactName As String = ""
        <DataMember()>
        Public Property BookOrigEmergencyContactName() As String
            Get
                Return Left(_BookOrigEmergencyContactName, 50)
            End Get
            Set(ByVal value As String)
                _BookOrigEmergencyContactName = Left(value, 50)
            End Set
        End Property

        Private _BookDestContactName As String = ""
        <DataMember()>
        Public Property BookDestContactName() As String
            Get
                Return Left(_BookDestContactName, 50)
            End Get
            Set(ByVal value As String)
                _BookDestContactName = Left(value, 50)
            End Set
        End Property

        Private _BookDestContactEmail As String = ""
        <DataMember()>
        Public Property BookDestContactEmail() As String
            Get
                Return Left(_BookDestContactEmail, 50)
            End Get
            Set(ByVal value As String)
                _BookDestContactEmail = Left(value, 50)
            End Set
        End Property

        Private _BookDestEmergencyContactPhone As String = ""
        <DataMember()>
        Public Property BookDestEmergencyContactPhone() As String
            Get
                Return Left(_BookDestEmergencyContactPhone, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookDestEmergencyContactPhone = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookDestEmergencyContactName As String = ""
        <DataMember()>
        Public Property BookDestEmergencyContactName() As String
            Get
                Return Left(_BookDestEmergencyContactName, 50)
            End Get
            Set(ByVal value As String)
                _BookDestEmergencyContactName = Left(value, 50)
            End Set
        End Property

        Private _BookCapacityProviderBolUrl As String = ""
        <DataMember()>
        Public Property BookCapacityProviderBolUrl() As String
            Get
                Return Left(_BookCapacityProviderBolUrl, 1000)
            End Get
            Set(ByVal value As String)
                _BookCapacityProviderBolUrl = Left(value, 1000)
            End Set
        End Property

        Private _BookPackingVisualizationUrl As String = ""
        <DataMember()>
        Public Property BookPackingVisualizationUrl() As String
            Get
                Return Left(_BookPackingVisualizationUrl, 1000)
            End Get
            Set(ByVal value As String)
                _BookPackingVisualizationUrl = Left(value, 1000)
            End Set
        End Property

        Private _BookCarrBookEndTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrBookEndTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrBookEndTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrBookEndTime = value
            End Set
        End Property

        Private _BookCarrPOEndTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookCarrPOEndTime() As System.Nullable(Of Date)
            Get
                Return _BookCarrPOEndTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookCarrPOEndTime = value
            End Set
        End Property

        'End Modified by RHR for v-8.1 on 03/26/2018


        'Begin Modified by RHR for v-8.5.3.006 on 11/16/2022
        '  added new fields from book table to DTO Object

        Private _BookCBFC As System.Nullable(Of Decimal)
        ''' <summary>
        ''' ERP provided BFC value sum of BookItemBFC managed by backend logic
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        ''' </remarks>
        <DataMember()>
        Public Property BookCBFC() As System.Nullable(Of Decimal)
            Get
                Return _BookCBFC
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _BookCBFC = value
            End Set
        End Property

        Private _BookLaneMustLeaveByDateTime As System.Nullable(Of Date)
        ''' <summary>
        ''' Pickup information in Lane for Earliest Pickup Time
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     this data is not fully implemented in v-8.5.3.006 typically NULL
        ''' </remarks>
        <DataMember()>
        Public Property BookLaneMustLeaveByDateTime() As System.Nullable(Of Date)
            Get
                Return _BookLaneMustLeaveByDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookLaneMustLeaveByDateTime = value
            End Set
        End Property


        Private _BookLaneMustLeaveByEndDateTime As System.Nullable(Of Date)
        ''' <summary>
        ''' Pickup information in Lane for Latest Pickup Time
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     this data is not fully implemented in v-8.5.3.006 typically NULL
        ''' </remarks>
        <DataMember()>
        Public Property BookLaneMustLeaveByEndDateTime() As System.Nullable(Of Date)
            Get
                Return _BookLaneMustLeaveByEndDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookLaneMustLeaveByEndDateTime = value
            End Set
        End Property

        Private _BookCarrRequestedService As String
        ''' <summary>
        ''' Requested shipment level of service from ERP/Customer Service
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     This is for future where booking integration logic can be included
        '''     to allow customer service to request the type of delivery like "Next Day Air"
        '''     Plans to include this in v-9.0 have been discussed by not approved
        ''' </remarks>
        <DataMember()>
        Public Property BookCarrRequestedService() As String
            Get
                Return Left(_BookCarrRequestedService, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrRequestedService = Left(value, 50)
            End Set
        End Property

        Private _BookCarrActualService As String
        ''' <summary>
        ''' Actual shipment level of service 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     This is the actual level of service selected for the shipment like "Next Day Air"
        ''' </remarks>
        <DataMember()>
        Public Property BookCarrActualService() As String
            Get
                Return Left(_BookCarrActualService, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrActualService = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTransitTimeType As System.Nullable(Of Integer)
        ''' <summary>
        ''' System reference Default Null To New TransitTimeType lookup table, not editable by users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     this data is not fully implemented in v-8.5.3.006 typically NULL
        ''' </remarks>
        <DataMember()>
        Public Property BookCarrTransitTimeType() As System.Nullable(Of Integer)
            Get
                Return _BookCarrTransitTimeType
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookCarrTransitTimeType = value
            End Set
        End Property

        Private _BookCarrTransitTime As System.Nullable(Of Integer)
        ''' <summary>
        ''' Estimated transit time in hours based on lead time calculation factors, not editable by users 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     when a load is tendered to a carrier the transit time is calculated and stored here
        '''     this data should be used to adjust the Must Leave By date and time and to calculate 
        '''     the ship or receipt date based on lane settings.  
        ''' </remarks>
        <DataMember()>
        Public Property BookCarrTransitTime() As System.Nullable(Of Integer)
            Get
                Return _BookCarrTransitTime
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookCarrTransitTime = value
            End Set
        End Property

        Private _BookLaneMustArriveByStartDateTime As System.Nullable(Of Date)
        ''' <summary>
        ''' Earliest delivery time on the required date based on the Lane LaneDestHourStart using lead time calculation factors
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     when the transit time is updated the system will use the LaneDestHourStart with the calculated required 
        '''     date to populate the BookLaneMustArriveByStartDateTime.  This information, along with BookLaneMustArriveByEndDateTime 
        '''     (LaneDestHourStop) and hours of service, will be used to determing Must Leave By date and time
        '''     Additioal logic may be included to modify the ship date and/or required date based on Lane settings.
        '''     The goal here is to meet customer delivery expectation even on multi-stop loads
        '''     Once BookLaneMustArriveByStartDateTime has been popuated (not null) the system will use the time information
        '''     in BookLaneMustArriveByStartDateTime for future calculations and not return to the Lane data.  This way the 
        '''     Lane data can be modified without impacting live loads already booked with a carrier.
        '''     For this reason the BookLaneMustArriveByStartDateTime date time must be editable from the Load Board maintenance pages
        '''     Notes: 
        '''         (a) the Read and/or Update functionality may be delayed to phase II or even v-9.0)
        '''         (b) in v-8.5.3.006 US Postal Codes will be used to idetify the Time Zones and Day light saving times
        '''             for must leave by date and time calculations located in tblZipCode
        '''         (c) If the LaneDestHourStart is not populated the system will use the workflow setting
        '''             "Alert Start of Business Hour" (EmailAlertStartOfBusinessDay) as the default
        '''         (d) if the LaneDestHourStop is not populated the system will use the workflow setting 
        '''             "Alert End of Business Hour" (EmailAlertEndOfBusinessDay) as the default
        ''' </remarks>
        <DataMember()>
        Public Property BookLaneMustArriveByStartDateTime() As System.Nullable(Of Date)
            Get
                Return _BookLaneMustArriveByStartDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookLaneMustArriveByStartDateTime = value
            End Set
        End Property

        Private _BookLaneMustArriveByEndDateTime As System.Nullable(Of Date)
        ''' <summary>
        ''' Earliest delivery time on the required date based on the Lane LaneDestHourStart using lead time calculation factors
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     when the transit time is updated the system will use the LaneDestHourStart with the calculated required 
        '''     date to populate the BookLaneMustArriveByStartDateTime.  This information, along with BookLaneMustArriveByEndDateTime 
        '''     (LaneDestHourStop) and hours of service, will be used to determing Must Leave By date and time
        '''     Additioal logic may be included to modify the ship date and/or required date based on Lane settings.
        '''     The goal here is to meet customer delivery expectation even on multi-stop loads
        '''     Once BookLaneMustArriveByStartDateTime has been popuated (not null) the system will use the time information
        '''     in BookLaneMustArriveByStartDateTime for future calculations and not return to the Lane data.  This way the 
        '''     Lane data can be modified without impacting live loads already booked with a carrier.
        '''     For this reason the BookLaneMustArriveByStartDateTime date time must be editable from the Load Board maintenance pages
        '''     Notes: 
        '''         (a) the Read and/or Update functionality may be delayed to phase II or even v-9.0)
        '''         (b) in v-8.5.3.006 US Postal Codes will be used to idetify the Time Zones and Day light saving times
        '''             for must leave by date and time calculations located in tblZipCode
        '''         (c) if the LaneDestHourStop is not populated the system will use the workflow setting 
        '''             "Alert End of Business Hour" (EmailAlertEndOfBusinessDay) as the default
        ''' </remarks>
        <DataMember()>
        Public Property BookLaneMustArriveByEndDateTime() As System.Nullable(Of Date)
            Get
                Return _BookLaneMustArriveByEndDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookLaneMustArriveByEndDateTime = value
            End Set
        End Property

        Private _BookLeadTimeLTLMinimum As System.Nullable(Of Integer)
        ''' <summary>
        ''' Applied amount For minimum trans days For LTL, loads less than 10,000 lbs , based On last calculation 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     when a load is tendered to a carrier the transit time on loads less than 10,000 lbs is adjusted
        '''     to reflect a minimum.  this data contains the actual minimum value used for this booking 
        '''     primarily used to recalculate cost on ship confirmed or after a load has been tendered.
        '''     This way changes to the workflow parameters do not effect the existing loads
        ''' </remarks>
        <DataMember()>
        Public Property BookLeadTimeLTLMinimum() As System.Nullable(Of Integer)
            Get
                Return _BookLeadTimeLTLMinimum
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookLeadTimeLTLMinimum = value
            End Set
        End Property

        Private _BookProductionLeadTimeDays As System.Nullable(Of Integer)
        ''' <summary>
        ''' Applied amount For production or peperation Lead Time For last minute orders When the lane allows For automated ship Date calculations. 
        ''' New bookings will be adjusted If needed by number Of days In value.  
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     when a new load is recieved and the current date and time of the server, adjusted for time zonesfor each warehouse, 
        '''     falls inside the production lead time requirements the Ship Date will be modified to include additional days
        '''     needed to prepare the load for shipping.  
        '''     The workflow opion for "Alert Allow Emails on Saturday" (EmailAlertOnSaturday) and "Alert Allow Emails on Sunday" (EmailAlertOnSunday) 
        '''     will be used to determine if production lead time includes weekends.
        '''     Each lane has special data fields that are not being used today.  these LaneSDF fields 
        '''     may be used in the future to manage shipping and receiving information based on inbound vs outbound settings
        '''     This information does not exist in the Comp table so we would need to link to the scheduler/dock doors if needed.     
        ''' </remarks>
        <DataMember()>
        Public Property BookProductionLeadTimeDays() As System.Nullable(Of Integer)
            Get
                Return _BookProductionLeadTimeDays
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookProductionLeadTimeDays = value
            End Set
        End Property

        Private _BookProductionLeadTimeUpdateRequired As System.Nullable(Of Integer)
        ''' <summary>
        ''' Applied flag to update the required Date When the ship Date changes when BookProductionLeadTimeDays is used.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     when the rules for BookProductionLeadTimeDays is applied allow the system to update the requried date
        '''     this should normally be turned on (true)         
        ''' </remarks>
        <DataMember()>
        Public Property BookProductionLeadTimeUpdateRequired() As System.Nullable(Of Integer)
            Get
                Return _BookProductionLeadTimeUpdateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookProductionLeadTimeUpdateRequired = value
            End Set
        End Property

        Private _BookLeadTimeMultiStopDelayHours As System.Nullable(Of Integer)
        ''' <summary>
        ''' Applied adjustment For hours added To Each Stop used To adjust the lead time. The last Stop Is always zero.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        '''     On multi stop loads this value indicates the number of hours added to the transit time for this stop
        ''' </remarks>
        <DataMember()>
        Public Property BookLeadTimeMultiStopDelayHours() As System.Nullable(Of Integer)
            Get
                Return _BookLeadTimeMultiStopDelayHours
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookLeadTimeMultiStopDelayHours = value
            End Set
        End Property

        Private _BookLeadTimeHoursofService As System.Nullable(Of Integer)
        ''' <summary>
        ''' Applied amount For Default hours Of service per day used To estimate drive times.  
        ''' LaneLeadTimeAutomationDaysByMile parameter divided by BookLeadTimeHoursofService = miles per hour.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        ''' </remarks>
        <DataMember()>
        Public Property BookLeadTimeHoursofService() As System.Nullable(Of Integer)
            Get
                Return _BookLeadTimeHoursofService
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookLeadTimeHoursofService = value
            End Set
        End Property

        Private _BookLeadTimeAutomationDaysByMile As System.Nullable(Of Integer)
        ''' <summary>
        ''' Applied amount For the average miles per day a truck can travel
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        ''' </remarks>
        <DataMember()>
        Public Property BookLeadTimeAutomationDaysByMile() As System.Nullable(Of Integer)
            Get
                Return _BookLeadTimeAutomationDaysByMile
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookLeadTimeAutomationDaysByMile = value
            End Set
        End Property

        Private _BookProductionLeadTimeApplied As System.Nullable(Of Boolean)
        ''' <summary>
        ''' Flag to identify if the production perparation lead time has been applied to adjust the ship date on this order
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 11/16/2022
        ''' </remarks>
        <DataMember()>
        Public Property BookProductionLeadTimeApplied() As System.Nullable(Of Boolean)
            Get
                Return _BookProductionLeadTimeApplied
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                _BookProductionLeadTimeApplied = value
            End Set
        End Property

        'End Modified by RHR for v-8.5.3.006 on 11/16/2022

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Book
            instance = DirectCast(MemberwiseClone(), Book)
            instance.BookLoads = Nothing
            For Each item In BookLoads
                instance.BookLoads.Add(DirectCast(item.Clone, BookLoad))
            Next
            instance.BookNotes = Nothing
            For Each item In BookNotes
                instance.BookNotes.Add(DirectCast(item.Clone, BookNote))
            Next
            instance.BookTracks = Nothing
            For Each item In BookTracks
                instance.BookTracks.Add(DirectCast(item.Clone, BookTrack))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace
