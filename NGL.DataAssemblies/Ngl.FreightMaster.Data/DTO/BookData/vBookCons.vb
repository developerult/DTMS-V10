Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vBookCons
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

        Private _BookStopNo As Short? = 0
        <DataMember()> _
        Public Property BookStopNo() As Short?
            Get
                Return _BookStopNo
            End Get
            Set(ByVal value As Short?)
                _BookStopNo = value
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

        Private _BookOrigCompControl As Integer? = 0
        <DataMember()> _
        Public Property BookOrigCompControl() As Integer?
            Get
                Return _BookOrigCompControl
            End Get
            Set(ByVal value As Integer?)
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

        Private _LaneNumber As String = ""
        <DataMember()> _
        Public Property LaneNumber() As String
            Get
                Return Left(_LaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneName As String = ""
        <DataMember()> _
        Public Property LaneName() As String
            Get
                Return Left(_LaneName, 50)
            End Get
            Set(ByVal value As String)
                _LaneName = Left(value, 50)
            End Set
        End Property

        Private _LaneBenchMiles As Double? = 0
        <DataMember()> _
        Public Property LaneBenchMiles() As Double?
            Get
                Return _LaneBenchMiles
            End Get
            Set(ByVal value As Double?)
                _LaneBenchMiles = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 50)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 50)
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

       

        Private _BookDestCompControl As Integer? = 0
        <DataMember()> _
        Public Property BookDestCompControl() As Integer?
            Get
                Return _BookDestCompControl
            End Get
            Set(ByVal value As Integer?)
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

        Private _BookTotalCases As Integer? = 0
        <DataMember()> _
        Public Property BookTotalCases() As Integer?
            Get
                Return _BookTotalCases
            End Get
            Set(ByVal value As Integer?)
                _BookTotalCases = value
            End Set
        End Property

        Private _BookTotalWgt As Double? = 0
        <DataMember()> _
        Public Property BookTotalWgt() As Double?
            Get
                Return _BookTotalWgt
            End Get
            Set(ByVal value As Double?)
                _BookTotalWgt = value
            End Set
        End Property

        Private _BookTotalPL As Double? = 0
        <DataMember()> _
        Public Property BookTotalPL() As Double?
            Get
                Return _BookTotalPL
            End Get
            Set(ByVal value As Double?)
                _BookTotalPL = value
            End Set
        End Property

        Private _BookTotalCube As Integer? = 0
        <DataMember()> _
        Public Property BookTotalCube() As Integer?
            Get
                Return _BookTotalCube
            End Get
            Set(ByVal value As Integer?)
                _BookTotalCube = value
            End Set
        End Property

        Private _BookTotalPX As Integer? = 0
        <DataMember()> _
        Public Property BookTotalPX() As Integer?
            Get
                Return _BookTotalPX
            End Get
            Set(ByVal value As Integer?)
                _BookTotalPX = value
            End Set
        End Property

        Private _BookTotalBFC As Decimal? = 0
        <DataMember()> _
        Public Property BookTotalBFC() As Decimal?
            Get
                Return _BookTotalBFC
            End Get
            Set(ByVal value As Decimal?)
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

        Private _BookUpdated As Byte()
        <DataMember()> _
        Public Property BookUpdated() As Byte()
            Get
                Return _BookUpdated
            End Get
            Set(ByVal value As Byte())
                _BookUpdated = value
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

       

        Private _BookRevBilledBFC As Decimal? = 0
        <DataMember()> _
        Public Property BookRevBilledBFC() As Decimal?
            Get
                Return _BookRevBilledBFC
            End Get
            Set(ByVal value As Decimal?)
                _BookRevBilledBFC = value
            End Set
        End Property

        Private _BookRevCarrierCost As Decimal? = 0
        <DataMember()> _
        Public Property BookRevCarrierCost() As Decimal?
            Get
                Return _BookRevCarrierCost
            End Get
            Set(ByVal value As Decimal?)
                _BookRevCarrierCost = value
            End Set
        End Property

        Private _BookRevStopQty As Integer? = 0
        <DataMember()> _
        Public Property BookRevStopQty() As Integer?
            Get
                Return _BookRevStopQty
            End Get
            Set(ByVal value As Integer?)
                _BookRevStopQty = value
            End Set
        End Property

        Private _BookRevStopCost As Decimal? = 0
        <DataMember()> _
        Public Property BookRevStopCost() As Decimal?
            Get
                Return _BookRevStopCost
            End Get
            Set(ByVal value As Decimal?)
                _BookRevStopCost = value
            End Set
        End Property

        Private _BookRevOtherCost As Decimal? = 0
        <DataMember()> _
        Public Property BookRevOtherCost() As Decimal?
            Get
                Return _BookRevOtherCost
            End Get
            Set(ByVal value As Decimal?)
                _BookRevOtherCost = value
            End Set
        End Property

        Private _BookRevTotalCost As Decimal? = 0
        <DataMember()> _
        Public Property BookRevTotalCost() As Decimal?
            Get
                Return _BookRevTotalCost
            End Get
            Set(ByVal value As Decimal?)
                _BookRevTotalCost = value
            End Set
        End Property

        Private _BookRevLoadSavings As Decimal? = 0
        <DataMember()> _
        Public Property BookRevLoadSavings() As Decimal?
            Get
                Return _BookRevLoadSavings
            End Get
            Set(ByVal value As Decimal?)
                _BookRevLoadSavings = value
            End Set
        End Property

        Private _BookRevCommPercent As Double? = 0
        <DataMember()> _
        Public Property BookRevCommPercent() As Double?
            Get
                Return _BookRevCommPercent
            End Get
            Set(ByVal value As Double?)
                _BookRevCommPercent = value
            End Set
        End Property

        Private _BookRevCommCost As Decimal? = 0
        <DataMember()> _
        Public Property BookRevCommCost() As Decimal?
            Get
                Return _BookRevCommCost
            End Get
            Set(ByVal value As Decimal?)
                _BookRevCommCost = value
            End Set
        End Property

        Private _BookRevGrossRevenue As Decimal? = 0
        <DataMember()> _
        Public Property BookRevGrossRevenue() As Decimal?
            Get
                Return _BookRevGrossRevenue
            End Get
            Set(ByVal value As Decimal?)
                _BookRevGrossRevenue = value
            End Set
        End Property


        Private _BookMilesFrom As Double? = 0
        <DataMember()> _
        Public Property BookMilesFrom() As Double?
            Get
                Return _BookMilesFrom
            End Get
            Set(ByVal value As Double?)
                _BookMilesFrom = value
            End Set
        End Property

        Private _BookLaneCarrControl As Integer? = 0
        <DataMember()> _
        Public Property BookLaneCarrControl() As Integer?
            Get
                Return _BookLaneCarrControl
            End Get
            Set(ByVal value As Integer?)
                _BookLaneCarrControl = value
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

        Private _BookPickupStopNumber As Integer
        <DataMember()> _
        Public Property BookPickupStopNumber() As Integer
            Get
                Return Me._BookPickupStopNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookPickupStopNumber = value) _
                   = False) Then
                    Me._BookPickupStopNumber = value
                    Me.SendPropertyChanged("BookPickupStopNumber")
                End If
            End Set
        End Property

        Private _BookOrigStopNumber As Integer
        <DataMember()> _
        Public Property BookOrigStopNumber() As Integer
            Get
                Return Me._BookOrigStopNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookOrigStopNumber = value) _
                   = False) Then
                    Me._BookOrigStopNumber = value
                    Me.SendPropertyChanged("BookOrigStopNumber")
                End If
            End Set
        End Property

        Private _BookDestStopNumber As Integer
        <DataMember()> _
        Public Property BookDestStopNumber() As Integer
            Get
                Return Me._BookDestStopNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookDestStopNumber = value) _
                   = False) Then
                    Me._BookDestStopNumber = value
                    Me.SendPropertyChanged("BookDestStopNumber")
                End If
            End Set
        End Property

        Private _BookOrigMiles As Double
        <DataMember()> _
        Public Property BookOrigMiles() As Double
            Get
                Return Me._BookOrigMiles
            End Get
            Set(ByVal value As Double)
                If ((Me._BookOrigMiles = value) _
                   = False) Then
                    Me._BookOrigMiles = value
                    Me.SendPropertyChanged("BookOrigMiles")
                End If
            End Set
        End Property

        Private _BookDestMiles As Double
        <DataMember()> _
        Public Property BookDestMiles() As Double
            Get
                Return Me._BookDestMiles
            End Get
            Set(ByVal value As Double)
                If ((Me._BookDestMiles = value) _
                   = False) Then
                    Me._BookDestMiles = value
                    Me.SendPropertyChanged("BookDestMiles")
                End If
            End Set
        End Property

        Private _BookPickNumber As Integer
        <DataMember()> _
        Public Property BookPickNumber() As Integer
            Get
                Return Me._BookPickNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._BookPickNumber = value) _
                   = False) Then
                    Me._BookPickNumber = value
                    Me.SendPropertyChanged("BookPickNumber")
                End If
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

        Private _BookSHID As String
        <DataMember()> _
        Public Property BookSHID() As String
            Get
                Return Left(_BookSHID, 50)
            End Get
            Set(ByVal value As String)
                _BookSHID = Left(value, 50)
            End Set
        End Property

        Private _BookExpDelDateTime As Date?
        <DataMember()> _
        Public Property BookExpDelDateTime() As Date?
            Get
                Return _BookExpDelDateTime
            End Get
            Set(ByVal value As Date?)
                _BookExpDelDateTime = value
            End Set
        End Property

        Private _BookMustLeaveByDateTime As Date?
        <DataMember()> _
        Public Property BookMustLeaveByDateTime() As Date?
            Get
                Return _BookMustLeaveByDateTime
            End Get
            Set(ByVal value As Date?)
                _BookMustLeaveByDateTime = value
            End Set
        End Property

        Private _BookOutOfRouteMiles As Double
        <DataMember()> _
        Public Property BookOutOfRouteMiles() As Double
            Get
                Return _BookOutOfRouteMiles
            End Get
            Set(ByVal value As Double)
                _BookOutOfRouteMiles = value
            End Set
        End Property

        Private _BookSpotRateAllocationFormula As Integer
        <DataMember()> _
        Public Property BookSpotRateAllocationFormula() As Integer
            Get
                Return _BookSpotRateAllocationFormula
            End Get
            Set(ByVal value As Integer)
                _BookSpotRateAllocationFormula = value
            End Set
        End Property

        Private _BookSpotRateAutoCalcBFC As Boolean = True
        <DataMember()> _
        Public Property BookSpotRateAutoCalcBFC() As Boolean
            Get
                Return _BookSpotRateAutoCalcBFC
            End Get
            Set(ByVal value As Boolean)
                _BookSpotRateAutoCalcBFC = value
            End Set
        End Property

        Private _BookSpotRateUseCarrierFuelAddendum As Boolean = False
        <DataMember()> _
        Public Property BookSpotRateUseCarrierFuelAddendum() As Boolean
            Get
                Return _BookSpotRateUseCarrierFuelAddendum
            End Get
            Set(ByVal value As Boolean)
                _BookSpotRateUseCarrierFuelAddendum = value
            End Set
        End Property

        Private _BookSpotRateBFCAllocationFormula As Integer
        <DataMember()> _
        Public Property BookSpotRateBFCAllocationFormula() As Integer
            Get
                Return _BookSpotRateBFCAllocationFormula
            End Get
            Set(ByVal value As Integer)
                _BookSpotRateBFCAllocationFormula = value
            End Set
        End Property

        Private _BookSpotRateTotalUnallocatedBFC As Decimal
        <DataMember()> _
        Public Property BookSpotRateTotalUnallocatedBFC() As Decimal
            Get
                Return _BookSpotRateTotalUnallocatedBFC
            End Get
            Set(ByVal value As Decimal)
                _BookSpotRateTotalUnallocatedBFC = value
            End Set
        End Property

        Private _BookSpotRateTotalUnallocatedLineHaul As Decimal
        <DataMember()> _
        Public Property BookSpotRateTotalUnallocatedLineHaul() As Decimal
            Get
                Return _BookSpotRateTotalUnallocatedLineHaul
            End Get
            Set(ByVal value As Decimal)
                _BookSpotRateTotalUnallocatedLineHaul = value
            End Set
        End Property

        Private _BookSpotRateUseFuelAddendum As Boolean = False
        <DataMember()> _
        Public Property BookSpotRateUseFuelAddendum() As Boolean
            Get
                Return _BookSpotRateUseFuelAddendum
            End Get
            Set(ByVal value As Boolean)
                _BookSpotRateUseFuelAddendum = value
            End Set
        End Property

        Private _BookRevLaneBenchMiles As Double? = 0
        <DataMember()> _
        Public Property BookRevLaneBenchMiles() As Double?
            Get
                Return _BookRevLaneBenchMiles
            End Get
            Set(ByVal value As Double?)
                _BookRevLaneBenchMiles = value
            End Set
        End Property

        Private _BookRevLoadMiles As Double? = 0
        <DataMember()> _
        Public Property BookRevLoadMiles() As Double?
            Get
                Return _BookRevLoadMiles
            End Get
            Set(ByVal value As Double?)
                _BookRevLoadMiles = value
            End Set
        End Property

        Private _BookCarrTarControl As Integer
        <DataMember()> _
        Public Property BookCarrTarControl() As Integer
            Get
                Return _BookCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarControl = value
            End Set
        End Property

        Private _BookCarrTarName As String
        <DataMember()> _
        Public Property BookCarrTarName() As String
            Get
                Return Left(_BookCarrTarName, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrTarName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipControl As Integer
        <DataMember()> _
        Public Property BookCarrTarEquipControl() As Integer
            Get
                Return _BookCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipControl = value
            End Set
        End Property

        Private _BookShipCarrierProControl As Integer?
        <DataMember()> _
        Public Property BookShipCarrierProControl() As Integer?
            Get
                Return _BookShipCarrierProControl
            End Get
            Set(ByVal value As Integer?)
                _BookShipCarrierProControl = value
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

        
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vBookCons
            instance = DirectCast(MemberwiseClone(), vBookCons)
            Return instance
        End Function

#End Region

    End Class
End Namespace