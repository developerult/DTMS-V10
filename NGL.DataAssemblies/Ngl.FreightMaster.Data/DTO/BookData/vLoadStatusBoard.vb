Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vLoadStatusBoard
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

        Private _BookRevCarrierCost As Decimal = 0
        <DataMember()> _
        Public Property BookRevCarrierCost() As Decimal
            Get
                Return _BookRevCarrierCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevCarrierCost = value
            End Set
        End Property

        Private _BookRevOtherCost As Decimal = 0
        <DataMember()> _
        Public Property BookRevOtherCost() As Decimal
            Get
                Return _BookRevOtherCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevOtherCost = value
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

        Private _BookCarrierContact As String = ""
        <DataMember()> _
        Public Property BookCarrierContact() As String
            Get
                Return Left(_BookCarrierContact, 50)
            End Get
            Set(ByVal value As String)
                _BookCarrierContact = Left(value, 50)
            End Set
        End Property

        Private _BookCarrierContactPhone As String = ""
        <DataMember()> _
        Public Property BookCarrierContactPhone() As String
            Get
                Return Left(_BookCarrierContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookCarrierContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
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


        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
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


        Private _BookTransType As String = ""
        <DataMember()> _
        Public Property BookTransType() As String
            Get
                Return Left(_BookTransType, 40)
            End Get
            Set(ByVal value As String)
                _BookTransType = Left(value, 40)
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

         
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vLoadStatusBoard
            instance = DirectCast(MemberwiseClone(), vLoadStatusBoard)
            Return instance
        End Function

#End Region
    End Class
End Namespace
