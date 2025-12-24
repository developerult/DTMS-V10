Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Destructurama.Attributed
Imports SerilogTracing

Namespace DataTransferObjects
    <DataContract(), LogAsScalar()>
    Public Class BookRevenue
        Inherits DTOBaseClass


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

        Private _BookRevNonTaxable As Decimal = 0
        <DataMember()>
        Public Property BookRevNonTaxable() As Decimal
            Get
                Return _BookRevNonTaxable
            End Get
            Set(ByVal value As Decimal)
                _BookRevNonTaxable = value
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

        Private _BookCarrierContControl As Integer?
        <DataMember()>
        Public Property BookCarrierContControl() As Integer?
            Get
                Return _BookCarrierContControl
            End Get
            Set(ByVal value As Integer?)
                _BookCarrierContControl = value
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

        Private _BookShipCarrierDetails As String = ""
        <DataMember()>
        Public Property BookShipCarrierDetails() As String
            Get
                Return Left(_BookShipCarrierDetails, 4000)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierDetails = Left(value, 4000)
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

        Private _BookMilesFrom As System.Nullable(Of Double)
        <DataMember()>
        Public Property BookMilesFrom() As System.Nullable(Of Double)
            Get
                Return _BookMilesFrom
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookMilesFrom = value
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

        Private _BookPickupStopNumber As Integer = 1
        <DataMember()>
        Public Property BookPickupStopNumber() As Integer
            Get
                Return _BookPickupStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookPickupStopNumber = value
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



        'Book Rev Specific Fields

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

        Private _CompanyName As String = ""
        <DataMember()>
        Public Property CompanyName() As String
            Get
                Return _CompanyName
            End Get
            Friend Set(ByVal value As String)
                _CompanyName = value
            End Set
        End Property

        Private _CompanyNumber As String = ""
        <DataMember()>
        Public Property CompanyNumber() As String
            Get
                Return _CompanyNumber
            End Get
            Friend Set(ByVal value As String)
                _CompanyNumber = value
            End Set
        End Property

        Private _CompFinUseImportFrtCost As Boolean = False
        <DataMember()>
        Public Property CompFinUseImportFrtCost() As Boolean
            Get
                Return _CompFinUseImportFrtCost
            End Get
            Set(ByVal value As Boolean)
                _CompFinUseImportFrtCost = value
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

        Private _BookFees As List(Of BookFee)
        <DataMember()>
        Public Property BookFees() As List(Of BookFee)
            Get
                Return _BookFees
            End Get
            Set(ByVal value As List(Of BookFee))
                _BookFees = value
            End Set
        End Property

        Private _LaneOriginAddressUse As Boolean?
        <DataMember()>
        Public Property LaneOriginAddressUse() As Boolean?
            Get
                Return _LaneOriginAddressUse
            End Get
            Set(ByVal value As Boolean?)
                _LaneOriginAddressUse = value
            End Set
        End Property

        Private _BookMustLeaveByDateTime As Date?
        <DataMember()>
        Public Property BookMustLeaveByDateTime() As Date?
            Get
                Return _BookMustLeaveByDateTime
            End Get
            Set(ByVal value As Date?)
                _BookMustLeaveByDateTime = value
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

        Private _BookSpotRateAllocationFormula As Integer
        <DataMember()>
        Public Property BookSpotRateAllocationFormula() As Integer
            Get
                Return _BookSpotRateAllocationFormula
            End Get
            Set(ByVal value As Integer)
                _BookSpotRateAllocationFormula = value
            End Set
        End Property

        Private _BookSpotRateAutoCalcBFC As Boolean = True
        <DataMember()>
        Public Property BookSpotRateAutoCalcBFC() As Boolean
            Get
                Return _BookSpotRateAutoCalcBFC
            End Get
            Set(ByVal value As Boolean)
                _BookSpotRateAutoCalcBFC = value
            End Set
        End Property

        Private _BookSpotRateUseCarrierFuelAddendum As Boolean = False
        <DataMember()>
        Public Property BookSpotRateUseCarrierFuelAddendum() As Boolean
            Get
                Return _BookSpotRateUseCarrierFuelAddendum
            End Get
            Set(ByVal value As Boolean)
                _BookSpotRateUseCarrierFuelAddendum = value
            End Set
        End Property

        Private _BookSpotRateBFCAllocationFormula As Integer
        <DataMember()>
        Public Property BookSpotRateBFCAllocationFormula() As Integer
            Get
                Return _BookSpotRateBFCAllocationFormula
            End Get
            Set(ByVal value As Integer)
                _BookSpotRateBFCAllocationFormula = value
            End Set
        End Property

        Private _BookSpotRateTotalUnallocatedBFC As Decimal
        <DataMember()>
        Public Property BookSpotRateTotalUnallocatedBFC() As Decimal
            Get
                Return _BookSpotRateTotalUnallocatedBFC
            End Get
            Set(ByVal value As Decimal)
                _BookSpotRateTotalUnallocatedBFC = value
            End Set
        End Property

        Private _BookSpotRateTotalUnallocatedLineHaul As Decimal
        <DataMember()>
        Public Property BookSpotRateTotalUnallocatedLineHaul() As Decimal
            Get
                Return _BookSpotRateTotalUnallocatedLineHaul
            End Get
            Set(ByVal value As Decimal)
                _BookSpotRateTotalUnallocatedLineHaul = value
            End Set
        End Property

        Private _BookSpotRateUseFuelAddendum As Boolean = False
        <DataMember()>
        Public Property BookSpotRateUseFuelAddendum() As Boolean
            Get
                Return _BookSpotRateUseFuelAddendum
            End Get
            Set(ByVal value As Boolean)
                _BookSpotRateUseFuelAddendum = value
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

        'Begin Modified by RHR for v-8.5.3.006 on 11/16/2022
        '  added new fields from book table to DTO Object

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
        Public Overrides Function ToString() As String
            Return $"[{BookOrigName} --> {BookDestName}] Tariff:{BookCarrTarName}:{BookCarrTarRevisionNumber} {BookShipCarrierName} SHID: {BookSHID} BookControl:{BookControl} PRO:{BookProNumber}, TransType: {BookTransType}, TranCode: {BookTranCode}, TotalWeight:{BookTotalWgt}, {CompanyName}({CompanyNumber}) "
        End Function
#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookRevenue
            Using Logger.StartActivity("BookRevenue.Clone()")

                instance = DirectCast(MemberwiseClone(), BookRevenue)
                instance.BookLoads = Nothing
                For Each item In BookLoads
                    instance.BookLoads.Add(DirectCast(item.Clone, BookLoad))
                Next
                instance.BookFees = Nothing
                For Each item In BookFees
                    instance.BookFees.Add(DirectCast(item.Clone, BookFee))
                Next
            End Using

            Return instance
        End Function

        ''' <summary>
        ''' Resets all booking, financial, carrier and tariff information to defaults for N status loads.
        ''' NOTE: caller must verify that this method is allowed typically not allowed for finalized or delivered orders
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Last synced to spResetToNStatus 5/15/16
        ''' Modified by LVV 7/1/16 for v-7.0.5.110 DAT
        '''  Reset BookRevLoadTenderTypeControl and 
        '''  BookRevLoadTenderStatusCode both to none
        ''' </remarks>
        Public Function ResetToNStatus() As Boolean

            'Reset Booking Information
            BookTranCode = "N"
            BookSHID = Nothing
            'reset Financial Information
            'When we go back to N the BookLockAllCosts should be turned off
            'If Not BookLockAllCosts Then
            BookLockAllCosts = False
            BookSpotRateAllocationFormula = 0
            BookSpotRateTotalUnallocatedLineHaul = 0
            BookSpotRateUseCarrierFuelAddendum = False
            BookSpotRateUseFuelAddendum = False
            BookRevCarrierCost = 0
            BookRevLineHaul = 0
            BookRevDiscount = 0
            BookRevNetCost = 0
            BookRevFreightTax = 0
            BookRevTotalCost = 0
            BookRevOtherCost = 0
            BookRevLoadSavings = 0
            BookRevGrossRevenue = 0
            BookRevCommCost = 0
            BookFinAPStdCost = 0
            BookFinARBookFrt = 0
            BookFinCommStd = 0
            BookFinServiceFee = 0
            'End If
            If Not BookLockBFCCost And CompFinUseImportFrtCost = False Then
                BookRevBilledBFC = 0
                BookTotalBFC = 0
                BookSpotRateAutoCalcBFC = True
                BookSpotRateBFCAllocationFormula = 0
                BookSpotRateTotalUnallocatedBFC = 0
            End If
            'reset BookShip info (assigned carrier info
            BookShipCarrierDetails = Nothing
            BookShipCarrierName = Nothing
            BookShipCarrierNumber = Nothing
            BookShipCarrierProControl = 0
            BookShipCarrierProNumber = Nothing
            BookShipCarrierProNumberRaw = Nothing
            'reset Carrier Information
            BookCarrierControl = 0
            BookCarrierContControl = 0
            BookCarrierContact = Nothing
            BookCarrierContactPhone = Nothing
            'reset Tariff Information
            BookCarrTarControl = 0
            BookCarrTarEquipControl = 0
            BookCarrTarEquipMatControl = 0
            BookCarrTarEquipMatDetControl = 0
            BookCarrTarEquipMatDetID = 0
            BookCarrTarEquipMatDetValue = 0
            BookCarrTarEquipMatName = Nothing
            BookCarrTarEquipName = Nothing
            BookCarrTarName = Nothing
            BookCarrTarRevisionNumber = 0
            BookCarrTruckControl = 0
            Me.BookMustLeaveByDateTime = Nothing
            Me.BookExpDelDateTime = Nothing

            'tariff engine fields
            BookBestDeficitCost = 0
            BookBestDeficitWeight = 0
            BookBestDeficitWeightBreak = 0
            BookRatedWeightBreak = 0
            BookWgtAdjCost = 0
            BookWgtAdjWeight = 0
            BookWgtAdjWeightBreak = 0
            BookBilledLoadWeight = 0
            BookMinAdjustedLoadWeight = 0
            BookSummedClassWeight = 0
            BookWgtRoundingVariance = 0
            BookHeaviestClass = ""
            BookAcutalHeaviestClassWeight = 0
            BookRevDiscountRate = 0
            BookRevDiscountMin = 0

            'Added by LVV 7/1/16 for v-7.0.5.110 DAT
            BookRevLoadTenderTypeControl = tblLoadTender.LoadTenderTypeEnum.None
            BookRevLoadTenderStatusCode = tblLoadTender.LoadTenderStatusCodeEnum.None

            'Added by LVV 10/25/16 for v-7.0.5.110 Add Book Interline
            BookCarrTarInterlinePoint = False

            'Clear any Accessorial Fees
            If Not Me.BookFees Is Nothing AndAlso Me.BookFees.Count > 0 Then
                Dim FeesToRemove As New List(Of BookFee)
                'bookRevenue.BookFees.RemoveAll(x => x.BookFeesAccessorialFeeTypeControl == (int)feetype); 
                Me.BookFees.RemoveAll(Function(x) x.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff)
                Me.BookFees.RemoveAll(Function(x) x.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Order)
                For Each f In Me.BookFees
                    'If f.BookFeesAccessorialFeeTypeControl = Utilities.AccessorialFeeType.Tariff Then
                    '    FeesToRemove.Add(f)
                    'Else
                    f.BookFeesValue = 0
                    'End If
                Next
                'If Not FeesToRemove Is Nothing AndAlso FeesToRemove.Count > 0 Then
                '    For Each f In FeesToRemove
                '        If Me.BookFees.Contains(f) Then Me.BookFees.Remove(f)
                '    Next
                'End If
            End If

            If Not Me.BookLoads Is Nothing AndAlso Me.BookLoads.Count > 0 Then
                For Each bload In Me.BookLoads
                    bload.BookLoadTotCost = 0
                    If Not BookLockBFCCost And CompFinUseImportFrtCost = False Then bload.BookLoadBFC = 0
                    If Not bload.BookItems Is Nothing AndAlso bload.BookItems.Count > 0 Then
                        For Each bItem In bload.BookItems
                            bItem.BookItemCarrTarEquipMatDetValue = 0
                            bItem.BookItemCarrTarEquipMatDetID = 0
                            bItem.BookItemCarrTarEquipMatName = Nothing
                            bItem.BookItemCarrTarEquipMatControl = 0
                            bItem.BookItemRatedFAKClass = Nothing
                            bItem.BookItemRatedNMFCClass = Nothing
                            bItem.BookItemRatedMarineCode = Nothing
                            bItem.BookItemRatedDOTCode = Nothing
                            bItem.BookItemRatedIATACode = Nothing
                            bItem.BookItemRated49CFRCode = Nothing
                            bItem.BookItemDeficitFAKClass = Nothing
                            bItem.BookItemDeficitNMFCClass = Nothing
                            bItem.BookItemDeficitMarineCode = Nothing
                            bItem.BookItemDeficitDOTCode = Nothing
                            bItem.BookItemDeficitIATACode = Nothing
                            bItem.BookItemDeficit49CFRCode = Nothing
                            bItem.BookItemWeightBreak = 0
                            bItem.BookItemDeficitWeightAdjustment = 0
                            bItem.BookItemDeficitCostAdjustment = 0
                            bItem.BookItemNonTaxableFees = 0
                            bItem.BookItemTaxes = 0
                            bItem.BookItemTaxableFees = 0
                            bItem.BookItemLineHaul = 0
                            bItem.BookItemDiscount = 0
                            bItem.BookItemFreightCost = 0
                            bItem.BookItemActFreightCost = 0
                            If Not BookLockBFCCost And CompFinUseImportFrtCost = False Then bItem.BookItemBFC = 0
                        Next
                    End If
                Next
            End If
        End Function

#End Region

    End Class
End Namespace
