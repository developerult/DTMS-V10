Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 7/3/2019
    ''' added New field "BookRevHistExpectedCost" to support tracking of Expected Costs
    ''' the default Is false.  Expected Costs Is a snapshot of the current Carrier costs, including fuel
    ''' for each order at the time a freight bill Is recieved.  the history table may be modified
    ''' after the freight bill Is recieved so we need to flag the expected cost records for tracking
    ''' And to compare the expected cost with the billed cost.  The billed cost Is stored in the
    ''' AP Mass Entry And AP Mass Entry History tables
    ''' </remarks>
    <DataContract()>
    Public Class BookRevHistory
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookRevHistControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistControl() As Integer
            Get
                Return _BookRevHistControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistControl = value
            End Set
        End Property

        Private _BookRevHistBookControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistBookControl() As Integer
            Get
                Return _BookRevHistBookControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistBookControl = value
            End Set
        End Property

        Private _BookRevHistRevision As Integer = 0
        <DataMember()>
        Public Property BookRevHistRevision() As Integer
            Get
                Return _BookRevHistRevision
            End Get
            Set(ByVal value As Integer)
                _BookRevHistRevision = value
            End Set
        End Property

        Private _BookRevHistBilledBFC As Decimal = 0
        <DataMember()>
        Public Property BookRevHistBilledBFC() As Decimal
            Get
                Return _BookRevHistBilledBFC
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistBilledBFC = value
            End Set
        End Property

        Private _BookRevHistCarrierCost As Decimal = 0
        <DataMember()>
        Public Property BookRevHistCarrierCost() As Decimal
            Get
                Return _BookRevHistCarrierCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistCarrierCost = value
            End Set
        End Property

        Private _BookRevHistStopQty As Integer = 0
        <DataMember()>
        Public Property BookRevHistStopQty() As Integer
            Get
                Return _BookRevHistStopQty
            End Get
            Set(ByVal value As Integer)
                _BookRevHistStopQty = value
            End Set
        End Property

        Private _BookRevHistStopCost As Decimal = 0
        <DataMember()>
        Public Property BookRevHistStopCost() As Decimal
            Get
                Return _BookRevHistStopCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistStopCost = value
            End Set
        End Property

        Private _BookRevHistOtherCost As Decimal = 0
        <DataMember()>
        Public Property BookRevHistOtherCost() As Decimal
            Get
                Return _BookRevHistOtherCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistOtherCost = value
            End Set
        End Property

        Private _BookRevHistTotalCost As Decimal = 0
        <DataMember()>
        Public Property BookRevHistTotalCost() As Decimal
            Get
                Return _BookRevHistTotalCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistTotalCost = value
            End Set
        End Property

        Private _BookRevHistLoadSavings As Decimal = 0
        <DataMember()>
        Public Property BookRevHistLoadSavings() As Decimal
            Get
                Return _BookRevHistLoadSavings
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistLoadSavings = value
            End Set
        End Property

        Private _BookRevHistCommPercent As Double = 0
        <DataMember()>
        Public Property BookRevHistCommPercent() As Double
            Get
                Return _BookRevHistCommPercent
            End Get
            Set(ByVal value As Double)
                _BookRevHistCommPercent = value
            End Set
        End Property

        Private _BookRevHistCommCost As Decimal = 0
        <DataMember()>
        Public Property BookRevHistCommCost() As Decimal
            Get
                Return _BookRevHistCommCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistCommCost = value
            End Set
        End Property

        Private _BookRevHistGrossRevenue As Decimal = 0
        <DataMember()>
        Public Property BookRevHistGrossRevenue() As Decimal
            Get
                Return _BookRevHistGrossRevenue
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistGrossRevenue = value
            End Set
        End Property

        Private _BookRevHistNegRevenue As Integer = 0
        <DataMember()>
        Public Property BookRevHistNegRevenue() As Integer
            Get
                Return _BookRevHistNegRevenue
            End Get
            Set(ByVal value As Integer)
                _BookRevHistNegRevenue = value
            End Set
        End Property

        Private _BookRevHistFreightTax As Decimal = 0
        <DataMember()>
        Public Property BookRevHistFreightTax() As Decimal
            Get
                Return _BookRevHistFreightTax
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistFreightTax = value
            End Set
        End Property

        Private _BookRevHistNetCost As Decimal = 0
        <DataMember()>
        Public Property BookRevHistNetCost() As Decimal
            Get
                Return _BookRevHistNetCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistNetCost = value
            End Set
        End Property

        Private _BookRevHistNonTaxable As Decimal = 0
        <DataMember()>
        Public Property BookRevHistNonTaxable() As Decimal
            Get
                Return _BookRevHistNonTaxable
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistNonTaxable = value
            End Set
        End Property

        Private _BookRevHistARBookFrt As System.Nullable(Of Decimal)
        <DataMember()>
        Public Property BookRevHistARBookFrt() As System.Nullable(Of Decimal)
            Get
                Return _BookRevHistARBookFrt
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _BookRevHistARBookFrt = value
            End Set
        End Property

        Private _BookRevHistAPPayAmt As Decimal = 0
        <DataMember()>
        Public Property BookRevHistAPPayAmt() As Decimal
            Get
                Return _BookRevHistAPPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistAPPayAmt = value
            End Set
        End Property

        Private _BookRevHistAPStdCost As Decimal = 0
        <DataMember()>
        Public Property BookRevHistAPStdCost() As Decimal
            Get
                Return _BookRevHistAPStdCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistAPStdCost = value
            End Set
        End Property

        Private _BookRevHistAPActCost As Decimal = 0
        <DataMember()>
        Public Property BookRevHistAPActCost() As Decimal
            Get
                Return _BookRevHistAPActCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistAPActCost = value
            End Set
        End Property

        Private _BookRevHistCommStd As Decimal = 0
        <DataMember()>
        Public Property BookRevHistCommStd() As Decimal
            Get
                Return _BookRevHistCommStd
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistCommStd = value
            End Set
        End Property

        Private _BookRevHistServiceFee As Decimal = 0
        <DataMember()>
        Public Property BookRevHistServiceFee() As Decimal
            Get
                Return _BookRevHistServiceFee
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistServiceFee = value
            End Set
        End Property

        Private _BookRevHistTranCode As String = ""
        <DataMember()>
        Public Property BookRevHistTranCode() As String
            Get
                Return Left(_BookRevHistTranCode, 3)
            End Get
            Set(ByVal value As String)
                _BookRevHistTranCode = Left(value, 3)
            End Set
        End Property

        Private _BookRevHistPayCode As String = ""
        <DataMember()>
        Public Property BookRevHistPayCode() As String
            Get
                Return Left(_BookRevHistPayCode, 3)
            End Get
            Set(ByVal value As String)
                _BookRevHistPayCode = Left(value, 3)
            End Set
        End Property

        Private _BookRevHistTypeCode As String = ""
        <DataMember()>
        Public Property BookRevHistTypeCode() As String
            Get
                Return Left(_BookRevHistTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _BookRevHistTypeCode = Left(value, 20)
            End Set
        End Property

        Private _BookRevHistStopNo As Short = 0
        <DataMember()>
        Public Property BookRevHistStopNo() As Short
            Get
                Return _BookRevHistStopNo
            End Get
            Set(ByVal value As Short)
                _BookRevHistStopNo = value
            End Set
        End Property

        Private _BookRevHistConsPrefix As String = ""
        <DataMember()>
        Public Property BookRevHistConsPrefix() As String
            Get
                Return Left(_BookRevHistConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookRevHistConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _BookRevHistCustCompControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistCustCompControl() As Integer
            Get
                Return _BookRevHistCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCustCompControl = value
            End Set
        End Property

        Private _BookRevHistODControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistODControl() As Integer
            Get
                Return _BookRevHistODControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistODControl = value
            End Set
        End Property

        Private _BookRevHistCarrierControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistCarrierControl() As Integer
            Get
                Return _BookRevHistCarrierControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCarrierControl = value
            End Set
        End Property

        Private _BookRevHistOrigCompControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistOrigCompControl() As Integer
            Get
                Return _BookRevHistOrigCompControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistOrigCompControl = value
            End Set
        End Property

        Private _BookRevHistOrigName As String = ""
        <DataMember()>
        Public Property BookRevHistOrigName() As String
            Get
                Return Left(_BookRevHistOrigName, 40)
            End Get
            Set(ByVal value As String)
                _BookRevHistOrigName = Left(value, 40)
            End Set
        End Property

        Private _BookRevHistOrigAddress1 As String = ""
        <DataMember()>
        Public Property BookRevHistOrigAddress1() As String
            Get
                Return Left(_BookRevHistOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookRevHistOrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookRevHistOrigAddress2 As String = ""
        <DataMember()>
        Public Property BookRevHistOrigAddress2() As String
            Get
                Return Left(_BookRevHistOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookRevHistOrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookRevHistOrigAddress3 As String = ""
        <DataMember()>
        Public Property BookRevHistOrigAddress3() As String
            Get
                Return Left(_BookRevHistOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookRevHistOrigAddress3 = Left(value, 40)
            End Set
        End Property

        Private _BookRevHistOrigCity As String = ""
        <DataMember()>
        Public Property BookRevHistOrigCity() As String
            Get
                Return Left(_BookRevHistOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _BookRevHistOrigCity = Left(value, 25)
            End Set
        End Property

        Private _BookRevHistOrigState As String = ""
        <DataMember()>
        Public Property BookRevHistOrigState() As String
            Get
                Return Left(_BookRevHistOrigState, 8)
            End Get
            Set(ByVal value As String)
                _BookRevHistOrigState = Left(value, 8)
            End Set
        End Property

        Private _BookRevHistOrigCountry As String = ""
        <DataMember()>
        Public Property BookRevHistOrigCountry() As String
            Get
                Return Left(_BookRevHistOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookRevHistOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _BookRevHistOrigZip As String = ""
        <DataMember()>
        Public Property BookRevHistOrigZip() As String
            Get
                Return Left(_BookRevHistOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookRevHistOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookRevHistDestCompControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistDestCompControl() As Integer
            Get
                Return _BookRevHistDestCompControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistDestCompControl = value
            End Set
        End Property

        Private _BookRevHistDestName As String = ""
        <DataMember()>
        Public Property BookRevHistDestName() As String
            Get
                Return Left(_BookRevHistDestName, 40)
            End Get
            Set(ByVal value As String)
                _BookRevHistDestName = Left(value, 40)
            End Set
        End Property

        Private _BookRevHistDestAddress1 As String = ""
        <DataMember()>
        Public Property BookRevHistDestAddress1() As String
            Get
                Return Left(_BookRevHistDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookRevHistDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookRevHistDestAddress2 As String = ""
        <DataMember()>
        Public Property BookRevHistDestAddress2() As String
            Get
                Return Left(_BookRevHistDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookRevHistDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _BookRevHistDestAddress3 As String = ""
        <DataMember()>
        Public Property BookRevHistDestAddress3() As String
            Get
                Return Left(_BookRevHistDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _BookRevHistDestAddress3 = Left(value, 40)
            End Set
        End Property

        Private _BookRevHistDestCity As String = ""
        <DataMember()>
        Public Property BookRevHistDestCity() As String
            Get
                Return Left(_BookRevHistDestCity, 25)
            End Get
            Set(ByVal value As String)
                _BookRevHistDestCity = Left(value, 25)
            End Set
        End Property

        Private _BookRevHistDestState As String = ""
        <DataMember()>
        Public Property BookRevHistDestState() As String
            Get
                Return Left(_BookRevHistDestState, 2)
            End Get
            Set(ByVal value As String)
                _BookRevHistDestState = Left(value, 2)
            End Set
        End Property

        Private _BookRevHistDestCountry As String = ""
        <DataMember()>
        Public Property BookRevHistDestCountry() As String
            Get
                Return Left(_BookRevHistDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _BookRevHistDestCountry = Left(value, 30)
            End Set
        End Property

        Private _BookRevHistDestZip As String = ""
        <DataMember()>
        Public Property BookRevHistDestZip() As String
            Get
                Return Left(_BookRevHistDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookRevHistDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _BookRevHistDateLoad As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookRevHistDateLoad() As System.Nullable(Of Date)
            Get
                Return _BookRevHistDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookRevHistDateLoad = value
            End Set
        End Property

        Private _BookRevHistDateRequired As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookRevHistDateRequired() As System.Nullable(Of Date)
            Get
                Return _BookRevHistDateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookRevHistDateRequired = value
            End Set
        End Property

        Private _BookRevHistTotalCases As Integer = 0
        <DataMember()>
        Public Property BookRevHistTotalCases() As Integer
            Get
                Return _BookRevHistTotalCases
            End Get
            Set(ByVal value As Integer)
                _BookRevHistTotalCases = value
            End Set
        End Property

        Private _BookRevHistTotalWgt As Double = 0
        <DataMember()>
        Public Property BookRevHistTotalWgt() As Double
            Get
                Return _BookRevHistTotalWgt
            End Get
            Set(ByVal value As Double)
                _BookRevHistTotalWgt = value
            End Set
        End Property

        Private _BookRevHistTotalPL As Double = 0
        <DataMember()>
        Public Property BookRevHistTotalPL() As Double
            Get
                Return _BookRevHistTotalPL
            End Get
            Set(ByVal value As Double)
                _BookRevHistTotalPL = value
            End Set
        End Property

        Private _BookRevHistTotalCube As Integer = 0
        <DataMember()>
        Public Property BookRevHistTotalCube() As Integer
            Get
                Return _BookRevHistTotalCube
            End Get
            Set(ByVal value As Integer)
                _BookRevHistTotalCube = value
            End Set
        End Property

        Private _BookRevHistTotalPX As Integer = 0
        <DataMember()>
        Public Property BookRevHistTotalPX() As Integer
            Get
                Return _BookRevHistTotalPX
            End Get
            Set(ByVal value As Integer)
                _BookRevHistTotalPX = value
            End Set
        End Property

        Private _BookRevHistTotalBFC As Decimal = 0
        <DataMember()>
        Public Property BookRevHistTotalBFC() As Decimal
            Get
                Return _BookRevHistTotalBFC
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistTotalBFC = value
            End Set
        End Property

        Private _BookRevHistRouteFinalCode As String = ""
        <DataMember()>
        Public Property BookRevHistRouteFinalCode() As String
            Get
                Return Left(_BookRevHistRouteFinalCode, 2)
            End Get
            Set(ByVal value As String)
                _BookRevHistRouteFinalCode = Left(value, 2)
            End Set
        End Property

        Private _BookRevHistRouteConsFlag As Boolean = False
        <DataMember()>
        Public Property BookRevHistRouteConsFlag() As Boolean
            Get
                Return _BookRevHistRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistRouteConsFlag = value
            End Set
        End Property

        Private _BookRevHistComCode As String = ""
        <DataMember()>
        Public Property BookRevHistComCode() As String
            Get
                Return Left(_BookRevHistComCode, 3)
            End Get
            Set(ByVal value As String)
                _BookRevHistComCode = Left(value, 3)
            End Set
        End Property

        Private _BookRevHistCarrOrderNumber As String = ""
        <DataMember()>
        Public Property BookRevHistCarrOrderNumber() As String
            Get
                Return Left(_BookRevHistCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookRevHistCarrOrderNumber = Left(value, 20)
            End Set
        End Property

        Private _BookRevHistOrderSequence As Integer = 0
        <DataMember()>
        Public Property BookRevHistOrderSequence() As Integer
            Get
                Return _BookRevHistOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookRevHistOrderSequence = value
            End Set
        End Property

        Private _BookRevHistLockAllCosts As Boolean = False
        <DataMember()>
        Public Property BookRevHistLockAllCosts() As Boolean
            Get
                Return _BookRevHistLockAllCosts
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistLockAllCosts = value
            End Set
        End Property

        Private _BookRevHistLockBFCCost As Boolean = False
        <DataMember()>
        Public Property BookRevHistLockBFCCost() As Boolean
            Get
                Return _BookRevHistLockBFCCost
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistLockBFCCost = value
            End Set
        End Property

        Private _BookRevHistShipCarrierProNumber As String = ""
        <DataMember()>
        Public Property BookRevHistShipCarrierProNumber() As String
            Get
                Return Left(_BookRevHistShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookRevHistShipCarrierProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookRevHistShipCarrierName As String = ""
        <DataMember()>
        Public Property BookRevHistShipCarrierName() As String
            Get
                Return Left(_BookRevHistShipCarrierName, 60)
            End Get
            Set(ByVal value As String)
                _BookRevHistShipCarrierName = Left(value, 60)
            End Set
        End Property

        Private _BookRevHistShipCarrierNumber As String = ""
        <DataMember()>
        Public Property BookRevHistShipCarrierNumber() As String
            Get
                Return Left(_BookRevHistShipCarrierNumber, 80)
            End Get
            Set(ByVal value As String)
                _BookRevHistShipCarrierNumber = Left(value, 80)
            End Set
        End Property

        Private _BookRevHistRouteTypeCode As Integer = 6
        <DataMember()>
        Public Property BookRevHistRouteTypeCode() As Integer
            Get
                If _BookRevHistRouteTypeCode = 0 Then _BookRevHistRouteTypeCode = 6
                Return _BookRevHistRouteTypeCode
            End Get
            Set(ByVal value As Integer)
                If value = 0 Then value = 6
                _BookRevHistRouteTypeCode = value
            End Set
        End Property

        Private _BookRevHistDefaultRouteSequence As Integer = 0
        <DataMember()>
        Public Property BookRevHistDefaultRouteSequence() As Integer
            Get
                Return _BookRevHistDefaultRouteSequence
            End Get
            Set(ByVal value As Integer)
                _BookRevHistDefaultRouteSequence = value
            End Set
        End Property

        Private _BookRevHistRouteGuideControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistRouteGuideControl() As Integer
            Get
                Return _BookRevHistRouteGuideControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistRouteGuideControl = value
            End Set
        End Property

        Private _BookRevHistCarrTruckControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistCarrTruckControl() As Integer
            Get
                Return _BookRevHistCarrTruckControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCarrTruckControl = value
            End Set
        End Property

        Private _BookRevHistCarrTarControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistCarrTarControl() As Integer
            Get
                Return _BookRevHistCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCarrTarControl = value
            End Set
        End Property

        Private _BookRevHistCarrTarRevisionNumber As Integer = 0
        <DataMember()>
        Public Property BookRevHistCarrTarRevisionNumber() As Integer
            Get
                Return _BookRevHistCarrTarRevisionNumber
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCarrTarRevisionNumber = value
            End Set
        End Property

        Private _BookRevHistCarrTarName As String
        Public Property BookRevHistCarrTarName As String
            Get
                Return Left(_BookRevHistCarrTarName, 50)
            End Get
            Set(value As String)
                _BookRevHistCarrTarName = Left(value, 50)
            End Set
        End Property

        Private _BookRevHistCarrTarEquipControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistCarrTarEquipControl() As Integer
            Get
                Return _BookRevHistCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCarrTarEquipControl = value
            End Set
        End Property

        Private _BookRevHistCarrTarEquipName As String
        Public Property BookRevHistCarrTarEquipName As String
            Get
                Return Left(_BookRevHistCarrTarEquipName, 50)
            End Get
            Set(value As String)
                _BookRevHistCarrTarEquipName = Left(value, 50)
            End Set
        End Property

        Private _BookRevHistCarrTarEquipMatControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistCarrTarEquipMatControl() As Integer
            Get
                Return _BookRevHistCarrTarEquipMatControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCarrTarEquipMatControl = value
            End Set
        End Property

        Private _BookRevHistCarrTarEquipMatName As String
        Public Property BookRevHistCarrTarEquipMatName As String
            Get
                Return Left(_BookRevHistCarrTarEquipMatName, 50)
            End Get
            Set(value As String)
                _BookRevHistCarrTarEquipMatName = Left(value, 50)
            End Set
        End Property

        Private _BookRevHistCarrTarEquipMatDetControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistCarrTarEquipMatDetControl() As Integer
            Get
                Return _BookRevHistCarrTarEquipMatDetControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCarrTarEquipMatDetControl = value
            End Set
        End Property

        Private _BookRevHistCarrTarEquipMatDetID As Integer = 0
        <DataMember()>
        Public Property BookRevHistCarrTarEquipMatDetID() As Integer
            Get
                Return _BookRevHistCarrTarEquipMatDetID
            End Get
            Set(ByVal value As Integer)
                _BookRevHistCarrTarEquipMatDetID = value
            End Set
        End Property

        Private _BookRevHistCarrTarEquipMatDetValue As System.Nullable(Of Decimal)
        <DataMember()>
        Public Property BookRevHistCarrTarEquipMatDetValue() As System.Nullable(Of Decimal)
            Get
                Return _BookRevHistCarrTarEquipMatDetValue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _BookRevHistCarrTarEquipMatDetValue = value
            End Set
        End Property

        Private _BookRevHistModeTypeControl As Integer = 0
        <DataMember()>
        Public Property BookRevHistModeTypeControl() As Integer
            Get
                Return _BookRevHistModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookRevHistModeTypeControl = value
            End Set
        End Property

        Private _BookRevHistAllowInterlinePoints As Boolean = True
        <DataMember()>
        Public Property BookRevHistAllowInterlinePoints() As Boolean
            Get
                Return _BookRevHistAllowInterlinePoints
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistAllowInterlinePoints = value
            End Set
        End Property

        Private _BookRevHistMilesFrom As System.Nullable(Of Double)
        <DataMember()>
        Public Property BookRevHistMilesFrom() As System.Nullable(Of Double)
            Get
                Return _BookRevHistMilesFrom
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookRevHistMilesFrom = value
            End Set
        End Property

        Private _BookRevHistTransType As String = ""
        <DataMember()>
        Public Property BookRevHistTransType() As String
            Get
                Return Left(_BookRevHistTransType, 50)
            End Get
            Set(ByVal value As String)
                _BookRevHistTransType = Left(value, 50)
            End Set
        End Property

        Private _BookRevHistLaneBenchMiles As System.Nullable(Of Double)
        <DataMember()>
        Public Property BookRevHistLaneBenchMiles() As System.Nullable(Of Double)
            Get
                Return _BookRevHistLaneBenchMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookRevHistLaneBenchMiles = value
            End Set
        End Property

        Private _BookRevHistLoadMiles As System.Nullable(Of Double)
        <DataMember()>
        Public Property BookRevHistLoadMiles() As System.Nullable(Of Double)
            Get
                Return _BookRevHistLoadMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookRevHistLoadMiles = value
            End Set
        End Property

        Private _BookRevHistPickupStopNumber As Integer = 1
        <DataMember()>
        Public Property BookRevHistPickupStopNumber() As Integer
            Get
                Return _BookRevHistPickupStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookRevHistPickupStopNumber = value
            End Set
        End Property

        Private _BookRevHistDiscount As Decimal = 0
        <DataMember()>
        Public Property BookRevHistDiscount() As Decimal
            Get
                Return _BookRevHistDiscount
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistDiscount = value
            End Set
        End Property

        Private _BookRevHistLineHaul As Decimal = 0
        <DataMember()>
        Public Property BookRevHistLineHaul() As Decimal
            Get
                Return _BookRevHistLineHaul
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistLineHaul = value
            End Set
        End Property

        Private _BookRevHistBookSHID As String
        <DataMember()>
        Public Property BookRevHistBookSHID() As String
            Get
                Return Left(_BookRevHistBookSHID, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BookRevHistBookSHID, value) = False) Then
                    Me._BookRevHistBookSHID = Left(value, 50)
                    Me.SendPropertyChanged("BookRevHistBookSHID")
                End If
            End Set
        End Property

        Private _BookRevHistMustLeaveByDateTime As Date?
        <DataMember()>
        Public Property BookRevHistMustLeaveByDateTime() As Date?
            Get
                Return _BookRevHistMustLeaveByDateTime
            End Get
            Set(ByVal value As Date?)
                _BookRevHistMustLeaveByDateTime = value
            End Set
        End Property


        Private _BookRevHistExpDelDateTime As Date?
        <DataMember()>
        Public Property BookRevHistExpDelDateTime() As Date?
            Get
                Return _BookRevHistExpDelDateTime
            End Get
            Set(ByVal value As Date?)
                _BookRevHistExpDelDateTime = value
            End Set
        End Property

        Private _BookRevHistOutOfRouteMiles As Double
        <DataMember()>
        Public Property BookRevHistOutOfRouteMiles() As Double
            Get
                Return _BookRevHistOutOfRouteMiles
            End Get
            Set(ByVal value As Double)
                _BookRevHistOutOfRouteMiles = value
            End Set
        End Property

        Private _BookRevHistModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookRevHistModDate() As System.Nullable(Of Date)
            Get
                Return _BookRevHistModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookRevHistModDate = value
            End Set
        End Property

        Private _BookRevHistModUser As String = ""
        <DataMember()>
        Public Property BookRevHistModUser() As String
            Get
                Return Left(_BookRevHistModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookRevHistModUser = Left(value, 100)
            End Set
        End Property

        Private _BookRevHistUpdated As Byte()
        <DataMember()>
        Public Property BookRevHistUpdated() As Byte()
            Get
                Return _BookRevHistUpdated
            End Get
            Set(ByVal value As Byte())
                _BookRevHistUpdated = value
            End Set
        End Property

        Private _BookRevHistBestDeficitCost As Decimal
        <DataMember()>
        Public Property BookRevHistBestDeficitCost() As Decimal
            Get
                Return _BookRevHistBestDeficitCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistBestDeficitCost = value
            End Set
        End Property

        Private _BookRevHistBestDeficitWeight As Double
        <DataMember()>
        Public Property BookRevHistBestDeficitWeight() As Double
            Get
                Return _BookRevHistBestDeficitWeight
            End Get
            Set(ByVal value As Double)
                _BookRevHistBestDeficitWeight = value
            End Set
        End Property

        Private _BookRevHistBestDeficitWeightBreak As Double
        <DataMember()>
        Public Property BookRevHistBestDeficitWeightBreak() As Double
            Get
                Return _BookRevHistBestDeficitWeightBreak
            End Get
            Set(ByVal value As Double)
                _BookRevHistBestDeficitWeightBreak = value
            End Set
        End Property


        Private _BookRevHistRatedWeightBreak As Double
        <DataMember()>
        Public Property BookRevHistRatedWeightBreak() As Double
            Get
                Return _BookRevHistRatedWeightBreak
            End Get
            Set(ByVal value As Double)
                _BookRevHistRatedWeightBreak = value
            End Set
        End Property

        Private _BookRevHistWgtAdjCost As Decimal
        <DataMember()>
        Public Property BookRevHistWgtAdjCost() As Decimal
            Get
                Return _BookRevHistWgtAdjCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistWgtAdjCost = value
            End Set
        End Property

        Private _BookRevHistWgtAdjWeight As Double
        <DataMember()>
        Public Property BookRevHistWgtAdjWeight() As Double
            Get
                Return _BookRevHistWgtAdjWeight
            End Get
            Set(ByVal value As Double)
                _BookRevHistWgtAdjWeight = value
            End Set
        End Property

        Private _BookRevHistWgtAdjWeightBreak As Double
        <DataMember()>
        Public Property BookRevHistWgtAdjWeightBreak() As Double
            Get
                Return _BookRevHistWgtAdjWeightBreak
            End Get
            Set(ByVal value As Double)
                _BookRevHistWgtAdjWeightBreak = value
            End Set
        End Property



        Private _BookRevHistBilledLoadWeight As Double
        <DataMember()>
        Public Property BookRevHistBilledLoadWeight() As Double
            Get
                Return _BookRevHistBilledLoadWeight
            End Get
            Set(ByVal value As Double)
                _BookRevHistBilledLoadWeight = value
            End Set
        End Property

        Private _BookRevHistMinAdjustedLoadWeight As Double
        <DataMember()>
        Public Property BookRevHistMinAdjustedLoadWeight() As Double
            Get
                Return _BookRevHistMinAdjustedLoadWeight
            End Get
            Set(ByVal value As Double)
                _BookRevHistMinAdjustedLoadWeight = value
            End Set
        End Property

        Private _BookRevHistSummedClassWeight As Double
        <DataMember()>
        Public Property BookRevHistSummedClassWeight() As Double
            Get
                Return _BookRevHistSummedClassWeight
            End Get
            Set(ByVal value As Double)
                _BookRevHistSummedClassWeight = value
            End Set
        End Property

        Private _BookRevHistWgtRoundingVariance As Double
        <DataMember()>
        Public Property BookRevHistWgtRoundingVariance() As Double
            Get
                Return _BookRevHistWgtRoundingVariance
            End Get
            Set(ByVal value As Double)
                _BookRevHistWgtRoundingVariance = value
            End Set
        End Property

        Private _BookRevHistHeaviestClass As String
        <DataMember()>
        Public Property BookRevHistHeaviestClass() As String
            Get
                Return _BookRevHistHeaviestClass
            End Get
            Set(ByVal value As String)
                _BookRevHistHeaviestClass = value
            End Set
        End Property

        Private _BookRevHistAcutalHeaviestClassWeight As Double
        <DataMember()>
        Public Property BookRevHistAcutalHeaviestClassWeight() As Double
            Get
                Return _BookRevHistAcutalHeaviestClassWeight
            End Get
            Set(ByVal value As Double)
                _BookRevHistAcutalHeaviestClassWeight = value
            End Set
        End Property

        Private _BookRevHistDiscountRate As Decimal = 0
        <DataMember()>
        Public Property BookRevHistDiscountRate() As Decimal
            Get
                Return _BookRevHistDiscountRate
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistDiscountRate = value
            End Set
        End Property

        Private _BookRevHistDiscountMin As Decimal = 0
        <DataMember()>
        Public Property BookRevHistDiscountMin() As Decimal
            Get
                Return _BookRevHistDiscountMin
            End Get
            Set(ByVal value As Decimal)
                _BookRevHistDiscountMin = value
            End Set
        End Property

        'Added By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _BookRevHistPreferredCarrier As Boolean = False
        <DataMember()>
        Public Property BookRevHistPreferredCarrier() As Boolean
            Get
                Return _BookRevHistPreferredCarrier
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistPreferredCarrier = value
            End Set
        End Property


        Private _BookRevHistExpectedCost As Boolean = False
        ''' <summary>
        ''' Flag to track Expected Costs the default Is false.  
        ''' Expected Cost Is a snapshot of the current carrier costs, including fuel
        ''' at the time a freight bill is received
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        '''  Modified by RHR for v-8.2 on 7/3/2019
        '''  added New field "BookRevHistExpectedCost" to support tracking of Expected Costs
        '''  the default Is false.  
        ''' </remarks>
        <DataMember()>
        Public Property BookRevHistExpectedCost() As Boolean
            Get
                Return _BookRevHistExpectedCost
            End Get
            Set(ByVal value As Boolean)
                _BookRevHistExpectedCost = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookRevHistory
            instance = DirectCast(MemberwiseClone(), BookRevHistory)
            Return instance
        End Function

#End Region

    End Class
End Namespace