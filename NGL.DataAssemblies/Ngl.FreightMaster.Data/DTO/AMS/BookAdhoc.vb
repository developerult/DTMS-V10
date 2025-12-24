Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookAdhoc
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookAdhocControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocControl() As Integer
            Get
                Return _BookAdhocControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocControl = value
            End Set
        End Property

        Private _BookAdhocProNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocProNumber() As String
            Get
                Return Left(_BookAdhocProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocProBase As String = ""
        <DataMember()> _
        Public Property BookAdhocProBase() As String
            Get
                Return Left(_BookAdhocProBase, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocProBase = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocConsPrefix As String = ""
        <DataMember()> _
        Public Property BookAdhocConsPrefix() As String
            Get
                Return Left(_BookAdhocConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocCustCompControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocCustCompControl() As Integer
            Get
                Return _BookAdhocCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocCustCompControl = value
            End Set
        End Property

        Private _BookAdhocCommCompControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocCommCompControl() As Integer
            Get
                Return _BookAdhocCommCompControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocCommCompControl = value
            End Set
        End Property

        Private _BookAdhocODControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocODControl() As Integer
            Get
                Return _BookAdhocODControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocODControl = value
            End Set
        End Property

        Private _BookAdhocCarrierControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocCarrierControl() As Integer
            Get
                Return _BookAdhocCarrierControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocCarrierControl = value
            End Set
        End Property

        Private _BookAdhocCarrierContact As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrierContact() As String
            Get
                Return Left(_BookAdhocCarrierContact, 30)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrierContact = Left(value, 30)
            End Set
        End Property

        Private _BookAdhocCarrierContactPhone As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrierContactPhone() As String
            Get
                Return Left(_BookAdhocCarrierContactPhone, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrierContactPhone = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocOrigCompControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocOrigCompControl() As Integer
            Get
                Return _BookAdhocOrigCompControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocOrigCompControl = value
            End Set
        End Property

        Private _BookAdhocOrigName As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigName() As String
            Get
                Return Left(_BookAdhocOrigName, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigName = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocOrigAddress1 As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigAddress1() As String
            Get
                Return Left(_BookAdhocOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocOrigAddress2 As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigAddress2() As String
            Get
                Return Left(_BookAdhocOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocOrigAddress3 As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigAddress3() As String
            Get
                Return Left(_BookAdhocOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigAddress3 = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocOrigCity As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigCity() As String
            Get
                Return Left(_BookAdhocOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigCity = Left(value, 25)
            End Set
        End Property

        Private _BookAdhocOrigState As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigState() As String
            Get
                Return Left(_BookAdhocOrigState, 8)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigState = Left(value, 8)
            End Set
        End Property

        Private _BookAdhocOrigCountry As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigCountry() As String
            Get
                Return Left(_BookAdhocOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _BookAdhocOrigZip As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigZip() As String
            Get
                Return Left(_BookAdhocOrigZip, 10)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigZip = Left(value, 10)
            End Set
        End Property

        Private _BookAdhocOrigPhone As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigPhone() As String
            Get
                Return Left(_BookAdhocOrigPhone, 15)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigPhone = Left(value, 15)
            End Set
        End Property

        Private _BookAdhocOrigFax As String = ""
        <DataMember()> _
        Public Property BookAdhocOrigFax() As String
            Get
                Return Left(_BookAdhocOrigFax, 15)
            End Get
            Set(ByVal value As String)
                _BookAdhocOrigFax = Left(value, 15)
            End Set
        End Property

        Private _BookAdhocOriginStartHrs As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocOriginStartHrs() As System.Nullable(Of Date)
            Get
                Return _BookAdhocOriginStartHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocOriginStartHrs = value
            End Set
        End Property

        Private _BookAdhocOriginStopHrs As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocOriginStopHrs() As System.Nullable(Of Date)
            Get
                Return _BookAdhocOriginStopHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocOriginStopHrs = value
            End Set
        End Property

        Private _BookAdhocOriginApptReq As Boolean = False
        <DataMember()> _
        Public Property BookAdhocOriginApptReq() As Boolean
            Get
                Return _BookAdhocOriginApptReq
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocOriginApptReq = value
            End Set
        End Property

        Private _BookAdhocDestCompControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocDestCompControl() As Integer
            Get
                Return _BookAdhocDestCompControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocDestCompControl = value
            End Set
        End Property

        Private _BookAdhocDestName As String = ""
        <DataMember()> _
        Public Property BookAdhocDestName() As String
            Get
                Return Left(_BookAdhocDestName, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestName = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocDestAddress1 As String = ""
        <DataMember()> _
        Public Property BookAdhocDestAddress1() As String
            Get
                Return Left(_BookAdhocDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocDestAddress2 As String = ""
        <DataMember()> _
        Public Property BookAdhocDestAddress2() As String
            Get
                Return Left(_BookAdhocDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocDestAddress3 As String = ""
        <DataMember()> _
        Public Property BookAdhocDestAddress3() As String
            Get
                Return Left(_BookAdhocDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestAddress3 = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocDestCity As String = ""
        <DataMember()> _
        Public Property BookAdhocDestCity() As String
            Get
                Return Left(_BookAdhocDestCity, 25)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestCity = Left(value, 25)
            End Set
        End Property

        Private _BookAdhocDestState As String = ""
        <DataMember()> _
        Public Property BookAdhocDestState() As String
            Get
                Return Left(_BookAdhocDestState, 2)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestState = Left(value, 2)
            End Set
        End Property

        Private _BookAdhocDestCountry As String = ""
        <DataMember()> _
        Public Property BookAdhocDestCountry() As String
            Get
                Return Left(_BookAdhocDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestCountry = Left(value, 30)
            End Set
        End Property

        Private _BookAdhocDestZip As String = ""
        <DataMember()> _
        Public Property BookAdhocDestZip() As String
            Get
                Return Left(_BookAdhocDestZip, 10)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestZip = Left(value, 10)
            End Set
        End Property

        Private _BookAdhocDestPhone As String = ""
        <DataMember()> _
        Public Property BookAdhocDestPhone() As String
            Get
                Return Left(_BookAdhocDestPhone, 15)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestPhone = Left(value, 15)
            End Set
        End Property

        Private _BookAdhocDestFax As String = ""
        <DataMember()> _
        Public Property BookAdhocDestFax() As String
            Get
                Return Left(_BookAdhocDestFax, 15)
            End Get
            Set(ByVal value As String)
                _BookAdhocDestFax = Left(value, 15)
            End Set
        End Property

        Private _BookAdhocDestStartHrs As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocDestStartHrs() As System.Nullable(Of Date)
            Get
                Return _BookAdhocDestStartHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocDestStartHrs = value
            End Set
        End Property

        Private _BookAdhocDestStopHrs As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocDestStopHrs() As System.Nullable(Of Date)
            Get
                Return _BookAdhocDestStopHrs
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocDestStopHrs = value
            End Set
        End Property

        Private _BookAdhocDestApptReq As Boolean = False
        <DataMember()> _
        Public Property BookAdhocDestApptReq() As Boolean
            Get
                Return _BookAdhocDestApptReq
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocDestApptReq = value
            End Set
        End Property

        Private _BookAdhocDateOrdered As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocDateOrdered() As System.Nullable(Of Date)
            Get
                Return _BookAdhocDateOrdered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocDateOrdered = value
            End Set
        End Property

        Private _BookAdhocDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocDateLoad() As System.Nullable(Of Date)
            Get
                Return _BookAdhocDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocDateLoad = value
            End Set
        End Property

        Private _BookAdhocDateInvoice As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocDateInvoice() As System.Nullable(Of Date)
            Get
                Return _BookAdhocDateInvoice
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocDateInvoice = value
            End Set
        End Property

        Private _BookAdhocDateRequired As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocDateRequired() As System.Nullable(Of Date)
            Get
                Return _BookAdhocDateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocDateRequired = value
            End Set
        End Property

        Private _BookAdhocDateDelivered As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocDateDelivered() As System.Nullable(Of Date)
            Get
                Return _BookAdhocDateDelivered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocDateDelivered = value
            End Set
        End Property

        Private _BookAdhocTotalCases As Integer = 0
        <DataMember()> _
        Public Property BookAdhocTotalCases() As Integer
            Get
                Return _BookAdhocTotalCases
            End Get
            Set(ByVal value As Integer)
                _BookAdhocTotalCases = value
            End Set
        End Property

        Private _BookAdhocTotalWgt As Double = 0
        <DataMember()> _
        Public Property BookAdhocTotalWgt() As Double
            Get
                Return _BookAdhocTotalWgt
            End Get
            Set(ByVal value As Double)
                _BookAdhocTotalWgt = value
            End Set
        End Property

        Private _BookAdhocTotalPL As Double = 0
        <DataMember()> _
        Public Property BookAdhocTotalPL() As Double
            Get
                Return _BookAdhocTotalPL
            End Get
            Set(ByVal value As Double)
                _BookAdhocTotalPL = value
            End Set
        End Property

        Private _BookAdhocTotalCube As Integer = 0
        <DataMember()> _
        Public Property BookAdhocTotalCube() As Integer
            Get
                Return _BookAdhocTotalCube
            End Get
            Set(ByVal value As Integer)
                _BookAdhocTotalCube = value
            End Set
        End Property

        Private _BookAdhocTotalPX As Integer = 0
        <DataMember()> _
        Public Property BookAdhocTotalPX() As Integer
            Get
                Return _BookAdhocTotalPX
            End Get
            Set(ByVal value As Integer)
                _BookAdhocTotalPX = value
            End Set
        End Property

        Private _BookAdhocTotalBFC As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocTotalBFC() As Decimal
            Get
                Return _BookAdhocTotalBFC
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocTotalBFC = value
            End Set
        End Property

        Private _BookAdhocTranCode As String = ""
        <DataMember()> _
        Public Property BookAdhocTranCode() As String
            Get
                Return Left(_BookAdhocTranCode, 3)
            End Get
            Set(ByVal value As String)
                _BookAdhocTranCode = Left(value, 3)
            End Set
        End Property

        Private _BookAdhocPayCode As String = ""
        <DataMember()> _
        Public Property BookAdhocPayCode() As String
            Get
                Return Left(_BookAdhocPayCode, 3)
            End Get
            Set(ByVal value As String)
                _BookAdhocPayCode = Left(value, 3)
            End Set
        End Property

        Private _BookAdhocTypeCode As String = ""
        <DataMember()> _
        Public Property BookAdhocTypeCode() As String
            Get
                Return Left(_BookAdhocTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocBOLCode As Boolean = False
        <DataMember()> _
        Public Property BookAdhocBOLCode() As Boolean
            Get
                Return _BookAdhocBOLCode
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocBOLCode = value
            End Set
        End Property

        Private _BookAdhocStopNo As Short = 0
        <DataMember()> _
        Public Property BookAdhocStopNo() As Short
            Get
                Return _BookAdhocStopNo
            End Get
            Set(ByVal value As Short)
                _BookAdhocStopNo = value
            End Set
        End Property

        Private _BookAdhocModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocModDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocModDate = value
            End Set
        End Property

        Private _BookAdhocModUser As String = ""
        <DataMember()> _
        Public Property BookAdhocModUser() As String
            Get
                Return Left(_BookAdhocModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookAdhocModUser = Left(value, 100)
            End Set
        End Property

        Private _BookAdhocUpdated As Byte()
        <DataMember()> _
        Public Property BookAdhocUpdated() As Byte()
            Get
                Return _BookAdhocUpdated
            End Get
            Set(ByVal value As Byte())
                _BookAdhocUpdated = value
            End Set
        End Property

        Private _BookAdhocCarrFBNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrFBNumber() As String
            Get
                Return Left(_BookAdhocCarrFBNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrFBNumber = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocCarrOrderNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrOrderNumber() As String
            Get
                Return Left(_BookAdhocCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrOrderNumber = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocCarrBLNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrBLNumber() As String
            Get
                Return Left(_BookAdhocCarrBLNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrBLNumber = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocCarrBookAdhocDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrBookAdhocDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrBookAdhocDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrBookAdhocDate = value
            End Set
        End Property

        Private _BookAdhocCarrBookAdhocTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrBookAdhocTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrBookAdhocTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrBookAdhocTime = value
            End Set
        End Property

        Private _BookAdhocCarrBookAdhocContact As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrBookAdhocContact() As String
            Get
                Return Left(_BookAdhocCarrBookAdhocContact, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrBookAdhocContact = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCarrScheduleDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrScheduleDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrScheduleDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrScheduleDate = value
            End Set
        End Property

        Private _BookAdhocCarrScheduleTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrScheduleTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrScheduleTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrScheduleTime = value
            End Set
        End Property

        Private _BookAdhocCarrActualDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrActualDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrActualDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrActualDate = value
            End Set
        End Property

        Private _BookAdhocCarrActualTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrActualTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrActualTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrActualTime = value
            End Set
        End Property

        Private _BookAdhocCarrActLoadComplete_Date As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrActLoadComplete_Date() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrActLoadComplete_Date
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrActLoadComplete_Date = value
            End Set
        End Property

        Private _BookAdhocCarrActLoadCompleteTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrActLoadCompleteTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrActLoadCompleteTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrActLoadCompleteTime = value
            End Set
        End Property

        Private _BookAdhocCarrDockPUAssigment As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrDockPUAssigment() As String
            Get
                Return Left(_BookAdhocCarrDockPUAssigment, 10)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrDockPUAssigment = Left(value, 10)
            End Set
        End Property

        Private _BookAdhocCarrPODate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrPODate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrPODate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrPODate = value
            End Set
        End Property

        Private _BookAdhocCarrPOTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrPOTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrPOTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrPOTime = value
            End Set
        End Property

        Private _BookAdhocCarrApptDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrApptDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrApptDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrApptDate = value
            End Set
        End Property

        Private _BookAdhocCarrApptTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrApptTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrApptTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrApptTime = value
            End Set
        End Property

        Private _BookAdhocCarrActDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrActDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrActDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrActDate = value
            End Set
        End Property

        Private _BookAdhocCarrActTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrActTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrActTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrActTime = value
            End Set
        End Property

        Private _BookAdhocCarrActUnloadCompDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrActUnloadCompDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrActUnloadCompDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrActUnloadCompDate = value
            End Set
        End Property

        Private _BookAdhocCarrActUnloadCompTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrActUnloadCompTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrActUnloadCompTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrActUnloadCompTime = value
            End Set
        End Property

        Private _BookAdhocCarrDockDelAssignment As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrDockDelAssignment() As String
            Get
                Return Left(_BookAdhocCarrDockDelAssignment, 10)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrDockDelAssignment = Left(value, 10)
            End Set
        End Property

        Private _BookAdhocCarrVarDay As Integer = 0
        <DataMember()> _
        Public Property BookAdhocCarrVarDay() As Integer
            Get
                Return _BookAdhocCarrVarDay
            End Get
            Set(ByVal value As Integer)
                _BookAdhocCarrVarDay = value
            End Set
        End Property

        Private _BookAdhocCarrVarHrs As Integer = 0
        <DataMember()> _
        Public Property BookAdhocCarrVarHrs() As Integer
            Get
                Return _BookAdhocCarrVarHrs
            End Get
            Set(ByVal value As Integer)
                _BookAdhocCarrVarHrs = value
            End Set
        End Property

        Private _BookAdhocCarrTrailerNo As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrTrailerNo() As String
            Get
                Return Left(_BookAdhocCarrTrailerNo, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrTrailerNo = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCarrSealNo As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrSealNo() As String
            Get
                Return Left(_BookAdhocCarrSealNo, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrSealNo = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCarrDriverNo As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrDriverNo() As String
            Get
                Return Left(_BookAdhocCarrDriverNo, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrDriverNo = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCarrDriverName As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrDriverName() As String
            Get
                Return Left(_BookAdhocCarrDriverName, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrDriverName = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCarrRouteNo As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrRouteNo() As String
            Get
                Return Left(_BookAdhocCarrRouteNo, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrRouteNo = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCarrTripNo As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrTripNo() As String
            Get
                Return Left(_BookAdhocCarrTripNo, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrTripNo = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocFinARBookAdhocFrt As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinARBookAdhocFrt() As Decimal
            Get
                Return _BookAdhocFinARBookAdhocFrt
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinARBookAdhocFrt = value
            End Set
        End Property

        Private _BookAdhocFinARInvoiceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinARInvoiceDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinARInvoiceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinARInvoiceDate = value
            End Set
        End Property

        Private _BookAdhocFinARInvoiceAmt As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinARInvoiceAmt() As Decimal
            Get
                Return _BookAdhocFinARInvoiceAmt
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinARInvoiceAmt = value
            End Set
        End Property

        Private _BookAdhocFinARPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinARPayDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinARPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinARPayDate = value
            End Set
        End Property

        Private _BookAdhocFinARPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinARPayAmt() As Decimal
            Get
                Return _BookAdhocFinARPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinARPayAmt = value
            End Set
        End Property

        Private _BookAdhocFinARCheck As String = ""
        <DataMember()> _
        Public Property BookAdhocFinARCheck() As String
            Get
                Return Left(_BookAdhocFinARCheck, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinARCheck = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocFinARGLNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocFinARGLNumber() As String
            Get
                Return Left(_BookAdhocFinARGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinARGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocFinARBalance As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinARBalance() As Decimal
            Get
                Return _BookAdhocFinARBalance
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinARBalance = value
            End Set
        End Property

        Private _BookAdhocFinARCurType As Integer = 0
        <DataMember()> _
        Public Property BookAdhocFinARCurType() As Integer
            Get
                Return _BookAdhocFinARCurType
            End Get
            Set(ByVal value As Integer)
                _BookAdhocFinARCurType = value
            End Set
        End Property

        Private _BookAdhocFinAPBillNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocFinAPBillNumber() As String
            Get
                Return Left(_BookAdhocFinAPBillNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinAPBillNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocFinAPBillNoDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinAPBillNoDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinAPBillNoDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinAPBillNoDate = value
            End Set
        End Property

        Private _BookAdhocFinAPBillInvDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinAPBillInvDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinAPBillInvDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinAPBillInvDate = value
            End Set
        End Property

        Private _BookAdhocFinAPActWgt As Integer = 0
        <DataMember()> _
        Public Property BookAdhocFinAPActWgt() As Integer
            Get
                Return _BookAdhocFinAPActWgt
            End Get
            Set(ByVal value As Integer)
                _BookAdhocFinAPActWgt = value
            End Set
        End Property

        Private _BookAdhocFinAPStdCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinAPStdCost() As Decimal
            Get
                Return _BookAdhocFinAPStdCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinAPStdCost = value
            End Set
        End Property

        Private _BookAdhocFinAPActCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinAPActCost() As Decimal
            Get
                Return _BookAdhocFinAPActCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinAPActCost = value
            End Set
        End Property

        Private _BookAdhocFinAPPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinAPPayDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinAPPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinAPPayDate = value
            End Set
        End Property

        Private _BookAdhocFinAPPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinAPPayAmt() As Decimal
            Get
                Return _BookAdhocFinAPPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinAPPayAmt = value
            End Set
        End Property

        Private _BookAdhocFinAPCheck As String = ""
        <DataMember()> _
        Public Property BookAdhocFinAPCheck() As String
            Get
                Return Left(_BookAdhocFinAPCheck, 15)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinAPCheck = Left(value, 15)
            End Set
        End Property

        Private _BookAdhocFinAPGLNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocFinAPGLNumber() As String
            Get
                Return Left(_BookAdhocFinAPGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinAPGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocFinAPLastViewed As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinAPLastViewed() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinAPLastViewed
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinAPLastViewed = value
            End Set
        End Property

        Private _BookAdhocFinAPCurType As Integer = 0
        <DataMember()> _
        Public Property BookAdhocFinAPCurType() As Integer
            Get
                Return _BookAdhocFinAPCurType
            End Get
            Set(ByVal value As Integer)
                _BookAdhocFinAPCurType = value
            End Set
        End Property

        Private _BookAdhocFinCommStd As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinCommStd() As Decimal
            Get
                Return _BookAdhocFinCommStd
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinCommStd = value
            End Set
        End Property

        Private _BookAdhocFinCommAct As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinCommAct() As Decimal
            Get
                Return _BookAdhocFinCommAct
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinCommAct = value
            End Set
        End Property

        Private _BookAdhocFinCommPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinCommPayDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinCommPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinCommPayDate = value
            End Set
        End Property

        Private _BookAdhocFinCommPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinCommPayAmt() As Decimal
            Get
                Return _BookAdhocFinCommPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinCommPayAmt = value
            End Set
        End Property

        Private _BookAdhocFinCommtCheck As String = ""
        <DataMember()> _
        Public Property BookAdhocFinCommtCheck() As String
            Get
                Return Left(_BookAdhocFinCommtCheck, 15)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinCommtCheck = Left(value, 15)
            End Set
        End Property

        Private _BookAdhocFinCommCreditAmt As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinCommCreditAmt() As Decimal
            Get
                Return _BookAdhocFinCommCreditAmt
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinCommCreditAmt = value
            End Set
        End Property

        Private _BookAdhocFinCommCreditPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinCommCreditPayDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinCommCreditPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinCommCreditPayDate = value
            End Set
        End Property

        Private _BookAdhocFinCommLoadCount As Integer = 0
        <DataMember()> _
        Public Property BookAdhocFinCommLoadCount() As Integer
            Get
                Return _BookAdhocFinCommLoadCount
            End Get
            Set(ByVal value As Integer)
                _BookAdhocFinCommLoadCount = value
            End Set
        End Property

        Private _BookAdhocFinCommGLNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocFinCommGLNumber() As String
            Get
                Return Left(_BookAdhocFinCommGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinCommGLNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocFinCheckClearedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinCheckClearedDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinCheckClearedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinCheckClearedDate = value
            End Set
        End Property

        Private _BookAdhocFinCheckClearedNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocFinCheckClearedNumber() As String
            Get
                Return Left(_BookAdhocFinCheckClearedNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinCheckClearedNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocFinCheckClearedAmt As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinCheckClearedAmt() As Decimal
            Get
                Return _BookAdhocFinCheckClearedAmt
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinCheckClearedAmt = value
            End Set
        End Property

        Private _BookAdhocFinCheckClearedDesc As String = ""
        <DataMember()> _
        Public Property BookAdhocFinCheckClearedDesc() As String
            Get
                Return Left(_BookAdhocFinCheckClearedDesc, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinCheckClearedDesc = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocFinCheckClearedAcct As String = ""
        <DataMember()> _
        Public Property BookAdhocFinCheckClearedAcct() As String
            Get
                Return Left(_BookAdhocFinCheckClearedAcct, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocFinCheckClearedAcct = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocRevBilledBFC As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevBilledBFC() As Decimal
            Get
                Return _BookAdhocRevBilledBFC
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevBilledBFC = value
            End Set
        End Property

        Private _BookAdhocRevCarrierCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevCarrierCost() As Decimal
            Get
                Return _BookAdhocRevCarrierCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevCarrierCost = value
            End Set
        End Property

        Private _BookAdhocRevStopQty As Integer = 0
        <DataMember()> _
        Public Property BookAdhocRevStopQty() As Integer
            Get
                Return _BookAdhocRevStopQty
            End Get
            Set(ByVal value As Integer)
                _BookAdhocRevStopQty = value
            End Set
        End Property

        Private _BookAdhocRevStopCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevStopCost() As Decimal
            Get
                Return _BookAdhocRevStopCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevStopCost = value
            End Set
        End Property

        Private _BookAdhocRevOtherCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevOtherCost() As Decimal
            Get
                Return _BookAdhocRevOtherCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevOtherCost = value
            End Set
        End Property

        Private _BookAdhocRevTotalCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevTotalCost() As Decimal
            Get
                Return _BookAdhocRevTotalCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevTotalCost = value
            End Set
        End Property

        Private _BookAdhocRevLoadSavings As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevLoadSavings() As Decimal
            Get
                Return _BookAdhocRevLoadSavings
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevLoadSavings = value
            End Set
        End Property

        Private _BookAdhocRevCommPercent As Double = 0
        <DataMember()> _
        Public Property BookAdhocRevCommPercent() As Double
            Get
                Return _BookAdhocRevCommPercent
            End Get
            Set(ByVal value As Double)
                _BookAdhocRevCommPercent = value
            End Set
        End Property

        Private _BookAdhocRevCommCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevCommCost() As Decimal
            Get
                Return _BookAdhocRevCommCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevCommCost = value
            End Set
        End Property

        Private _BookAdhocRevGrossRevenue As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevGrossRevenue() As Decimal
            Get
                Return _BookAdhocRevGrossRevenue
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevGrossRevenue = value
            End Set
        End Property

        Private _BookAdhocRevNegRevenue As Integer = 0
        <DataMember()> _
        Public Property BookAdhocRevNegRevenue() As Integer
            Get
                Return _BookAdhocRevNegRevenue
            End Get
            Set(ByVal value As Integer)
                _BookAdhocRevNegRevenue = value
            End Set
        End Property

        Private _BookAdhocMilesFrom As Double = 0
        <DataMember()> _
        Public Property BookAdhocMilesFrom() As Double
            Get
                Return _BookAdhocMilesFrom
            End Get
            Set(ByVal value As Double)
                _BookAdhocMilesFrom = value
            End Set
        End Property

        Private _BookAdhocLaneCarrControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocLaneCarrControl() As Integer
            Get
                Return _BookAdhocLaneCarrControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocLaneCarrControl = value
            End Set
        End Property

        Private _BookAdhocHoldLoad As Integer = 0
        <DataMember()> _
        Public Property BookAdhocHoldLoad() As Integer
            Get
                Return _BookAdhocHoldLoad
            End Get
            Set(ByVal value As Integer)
                _BookAdhocHoldLoad = value
            End Set
        End Property

        Private _BookAdhocRouteFinalDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocRouteFinalDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocRouteFinalDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocRouteFinalDate = value
            End Set
        End Property

        Private _BookAdhocRouteFinalCode As String = ""
        <DataMember()> _
        Public Property BookAdhocRouteFinalCode() As String
            Get
                Return Left(_BookAdhocRouteFinalCode, 2)
            End Get
            Set(ByVal value As String)
                _BookAdhocRouteFinalCode = Left(value, 2)
            End Set
        End Property

        Private _BookAdhocRouteFinalFlag As Boolean = False
        <DataMember()> _
        Public Property BookAdhocRouteFinalFlag() As Boolean
            Get
                Return _BookAdhocRouteFinalFlag
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocRouteFinalFlag = value
            End Set
        End Property

        Private _BookAdhocWarehouseNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocWarehouseNumber() As String
            Get
                Return Left(_BookAdhocWarehouseNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocWarehouseNumber = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocComCode As String = ""
        <DataMember()> _
        Public Property BookAdhocComCode() As String
            Get
                Return Left(_BookAdhocComCode, 3)
            End Get
            Set(ByVal value As String)
                _BookAdhocComCode = Left(value, 3)
            End Set
        End Property

        Private _BookAdhocTransType As String = ""
        <DataMember()> _
        Public Property BookAdhocTransType() As String
            Get
                Return Left(_BookAdhocTransType, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocTransType = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocRouteConsFlag As Boolean = False
        <DataMember()> _
        Public Property BookAdhocRouteConsFlag() As Boolean
            Get
                Return _BookAdhocRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocRouteConsFlag = value
            End Set
        End Property

        Private _BookAdhocWhseAuthorizationNo As String = ""
        <DataMember()> _
        Public Property BookAdhocWhseAuthorizationNo() As String
            Get
                Return Left(_BookAdhocWhseAuthorizationNo, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocWhseAuthorizationNo = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocHotLoad As Boolean = False
        <DataMember()> _
        Public Property BookAdhocHotLoad() As Boolean
            Get
                Return _BookAdhocHotLoad
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocHotLoad = value
            End Set
        End Property

        Private _BookAdhocFinAPActTax As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinAPActTax() As Decimal
            Get
                Return _BookAdhocFinAPActTax
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinAPActTax = value
            End Set
        End Property

        Private _BookAdhocFinAPExportFlag As Boolean = False
        <DataMember()> _
        Public Property BookAdhocFinAPExportFlag() As Boolean
            Get
                Return _BookAdhocFinAPExportFlag
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocFinAPExportFlag = value
            End Set
        End Property

        Private _BookAdhocFinARFreightTax As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinARFreightTax() As Decimal
            Get
                Return _BookAdhocFinARFreightTax
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinARFreightTax = value
            End Set
        End Property

        Private _BookAdhocRevFreightTax As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevFreightTax() As Decimal
            Get
                Return _BookAdhocRevFreightTax
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevFreightTax = value
            End Set
        End Property

        Private _BookAdhocRevNetCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocRevNetCost() As Decimal
            Get
                Return _BookAdhocRevNetCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocRevNetCost = value
            End Set
        End Property

        Private _BookAdhocFinServiceFee As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocFinServiceFee() As Decimal
            Get
                Return _BookAdhocFinServiceFee
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocFinServiceFee = value
            End Set
        End Property

        Private _BookAdhocFinAPExportDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocFinAPExportDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocFinAPExportDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocFinAPExportDate = value
            End Set
        End Property

        Private _BookAdhocFinAPExportRetry As Integer = 0
        <DataMember()> _
        Public Property BookAdhocFinAPExportRetry() As Integer
            Get
                Return _BookAdhocFinAPExportRetry
            End Get
            Set(ByVal value As Integer)
                _BookAdhocFinAPExportRetry = value
            End Set
        End Property

        Private _BookAdhocCarrierContControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocCarrierContControl() As Integer
            Get
                Return _BookAdhocCarrierContControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocCarrierContControl = value
            End Set
        End Property

        Private _BookAdhocHotLoadSent As Boolean = False
        <DataMember()> _
        Public Property BookAdhocHotLoadSent() As Boolean
            Get
                Return _BookAdhocHotLoadSent
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocHotLoadSent = value
            End Set
        End Property

        Private _BookAdhocExportDocCreated As Boolean = False
        <DataMember()> _
        Public Property BookAdhocExportDocCreated() As Boolean
            Get
                Return _BookAdhocExportDocCreated
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocExportDocCreated = value
            End Set
        End Property

        Private _BookAdhocDoNotInvoice As Boolean = False
        <DataMember()> _
        Public Property BookAdhocDoNotInvoice() As Boolean
            Get
                Return _BookAdhocDoNotInvoice
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocDoNotInvoice = value
            End Set
        End Property

        Private _BookAdhocCarrStartLoadingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrStartLoadingDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrStartLoadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrStartLoadingDate = value
            End Set
        End Property

        Private _BookAdhocCarrStartLoadingTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrStartLoadingTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrStartLoadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrStartLoadingTime = value
            End Set
        End Property

        Private _BookAdhocCarrFinishLoadingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrFinishLoadingDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrFinishLoadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrFinishLoadingDate = value
            End Set
        End Property

        Private _BookAdhocCarrFinishLoadingTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrFinishLoadingTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrFinishLoadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrFinishLoadingTime = value
            End Set
        End Property

        Private _BookAdhocCarrStartUnloadingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrStartUnloadingDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrStartUnloadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrStartUnloadingDate = value
            End Set
        End Property

        Private _BookAdhocCarrStartUnloadingTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrStartUnloadingTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrStartUnloadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrStartUnloadingTime = value
            End Set
        End Property

        Private _BookAdhocCarrFinishUnloadingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrFinishUnloadingDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrFinishUnloadingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrFinishUnloadingDate = value
            End Set
        End Property

        Private _BookAdhocCarrFinishUnloadingTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocCarrFinishUnloadingTime() As System.Nullable(Of Date)
            Get
                Return _BookAdhocCarrFinishUnloadingTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocCarrFinishUnloadingTime = value
            End Set
        End Property

        Private _BookAdhocOrderSequence As Integer = 0
        <DataMember()> _
        Public Property BookAdhocOrderSequence() As Integer
            Get
                Return _BookAdhocOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookAdhocOrderSequence = value
            End Set
        End Property

        Private _BookAdhocChepGLID As String = ""
        <DataMember()> _
        Public Property BookAdhocChepGLID() As String
            Get
                Return Left(_BookAdhocChepGLID, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocChepGLID = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCarrierTypeCode As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrierTypeCode() As String
            Get
                Return Left(_BookAdhocCarrierTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrierTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocPalletPositions As String = ""
        <DataMember()> _
        Public Property BookAdhocPalletPositions() As String
            Get
                Return Left(_BookAdhocPalletPositions, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocPalletPositions = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocShipCarrierProNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocShipCarrierProNumber() As String
            Get
                Return Left(_BookAdhocShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocShipCarrierProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocShipCarrierName As String = ""
        <DataMember()> _
        Public Property BookAdhocShipCarrierName() As String
            Get
                Return Left(_BookAdhocShipCarrierName, 60)
            End Get
            Set(ByVal value As String)
                _BookAdhocShipCarrierName = Left(value, 60)
            End Set
        End Property

        Private _BookAdhocShipCarrierNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocShipCarrierNumber() As String
            Get
                Return Left(_BookAdhocShipCarrierNumber, 80)
            End Get
            Set(ByVal value As String)
                _BookAdhocShipCarrierNumber = Left(value, 80)
            End Set
        End Property

        Private _BookAdhocAPAdjReasonControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocAPAdjReasonControl() As Integer
            Get
                Return _BookAdhocAPAdjReasonControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocAPAdjReasonControl = value
            End Set
        End Property

        Private _BookAdhocDateRequested As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocDateRequested() As System.Nullable(Of Date)
            Get
                Return _BookAdhocDateRequested
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocDateRequested = value
            End Set
        End Property

        Private _BookAdhocCarrierEquipmentCodes As String = ""
        <DataMember()> _
        Public Property BookAdhocCarrierEquipmentCodes() As String
            Get
                Return Left(_BookAdhocCarrierEquipmentCodes, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocCarrierEquipmentCodes = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocLockAllCosts As Boolean = False
        <DataMember()> _
        Public Property BookAdhocLockAllCosts() As Boolean
            Get
                Return _BookAdhocLockAllCosts
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocLockAllCosts = value
            End Set
        End Property

        Private _BookAdhocLockBFCCost As Boolean = False
        <DataMember()> _
        Public Property BookAdhocLockBFCCost() As Boolean
            Get
                Return _BookAdhocLockBFCCost
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocLockBFCCost = value
            End Set
        End Property

        Private _BookAdhocDestStopNumber As Integer = 0
        <DataMember()> _
        Public Property BookAdhocDestStopNumber() As Integer
            Get
                Return _BookAdhocDestStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookAdhocDestStopNumber = value
            End Set
        End Property

        Private _BookAdhocOrigStopNumber As Integer = 0
        <DataMember()> _
        Public Property BookAdhocOrigStopNumber() As Integer
            Get
                Return _BookAdhocOrigStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookAdhocOrigStopNumber = value
            End Set
        End Property

        Private _BookAdhocOrigStopControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocOrigStopControl() As Integer
            Get
                Return _BookAdhocOrigStopControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocOrigStopControl = value
            End Set
        End Property

        Private _BookAdhocDestStopControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocDestStopControl() As Integer
            Get
                Return _BookAdhocDestStopControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocDestStopControl = value
            End Set
        End Property

        Private _BookAdhocRouteTypeCode As Integer = 0
        <DataMember()> _
        Public Property BookAdhocRouteTypeCode() As Integer
            Get
                Return _BookAdhocRouteTypeCode
            End Get
            Set(ByVal value As Integer)
                _BookAdhocRouteTypeCode = value
            End Set
        End Property

        Private _BookAdhocAlternateAddressLaneControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocAlternateAddressLaneControl() As Integer
            Get
                Return _BookAdhocAlternateAddressLaneControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocAlternateAddressLaneControl = value
            End Set
        End Property

        Private _BookAdhocAlternateAddressLaneNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocAlternateAddressLaneNumber() As String
            Get
                Return Left(_BookAdhocAlternateAddressLaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocAlternateAddressLaneNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocDefaultRouteSequence As Integer = 0
        <DataMember()> _
        Public Property BookAdhocDefaultRouteSequence() As Integer
            Get
                Return _BookAdhocDefaultRouteSequence
            End Get
            Set(ByVal value As Integer)
                _BookAdhocDefaultRouteSequence = value
            End Set
        End Property

        Private _BookAdhocRouteGuideControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocRouteGuideControl() As Integer
            Get
                Return _BookAdhocRouteGuideControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocRouteGuideControl = value
            End Set
        End Property

        Private _BookAdhocRouteGuideNumber As String = ""
        <DataMember()> _
        Public Property BookAdhocRouteGuideNumber() As String
            Get
                Return Left(_BookAdhocRouteGuideNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocRouteGuideNumber = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocCustomerApprovalTransmitted As Boolean = False
        <DataMember()> _
        Public Property BookAdhocCustomerApprovalTransmitted() As Boolean
            Get
                Return _BookAdhocCustomerApprovalTransmitted
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocCustomerApprovalTransmitted = value
            End Set
        End Property

        Private _BookAdhocCustomerApprovalRecieved As Boolean = False
        <DataMember()> _
        Public Property BookAdhocCustomerApprovalRecieved() As Boolean
            Get
                Return _BookAdhocCustomerApprovalRecieved
            End Get
            Set(ByVal value As Boolean)
                _BookAdhocCustomerApprovalRecieved = value
            End Set
        End Property

        Private _BookAdhocAMSPickupApptControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocAMSPickupApptControl As Integer
            Get
                Return _BookAdhocAMSPickupApptControl
            End Get
            Set(value As Integer)
                _BookAdhocAMSPickupApptControl = value
            End Set
        End Property

        Private _BookAdhocAMSDeliveryApptControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocAMSDeliveryApptControl As Integer
            Get
                Return _BookAdhocAMSDeliveryApptControl
            End Get
            Set(value As Integer)
                _BookAdhocAMSDeliveryApptControl = value
            End Set
        End Property

        Private _BookAdhocItemDetailDescription As String
        <DataMember()> _
        Public Property BookAdhocItemDetailDescription As String
            Get
                Return Left(_BookAdhocItemDetailDescription, 4000)
            End Get
            Set(value As String)
                _BookAdhocItemDetailDescription = Left(value, 4000)
            End Set
        End Property


        Private _BookAdhocLoads As List(Of BookAdhocLoad)
        <DataMember()> _
        Public Property BookAdhocLoads() As List(Of BookAdhocLoad)
            Get
                Return _BookAdhocLoads
            End Get
            Set(ByVal value As List(Of BookAdhocLoad))
                _BookAdhocLoads = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookAdhoc
            instance = DirectCast(MemberwiseClone(), BookAdhoc)
            instance.BookAdhocLoads = Nothing
            For Each item In BookAdhocLoads
                instance.BookAdhocLoads.Add(DirectCast(item.Clone, BookAdhocLoad))
            Next
           
            Return instance
        End Function

#End Region

    End Class
End Namespace
