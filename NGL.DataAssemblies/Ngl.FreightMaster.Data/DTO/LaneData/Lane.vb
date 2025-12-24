Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Serilog

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Lane
        Inherits DTOBaseClass

        public Sub New ()
            Me.Logger = Me.Logger.ForContext(of Lane)
        End Sub
#Region " Data Members"
        Private _LaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneControl() As Integer
            Get
                Return _LaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneControl = value
            End Set
        End Property

        Private _LaneNumber As String = ""
        <DataMember()> _
        Public Property LaneNumber() As String
            Get
                Return Left(_LaneNumber, 150)
            End Get
            Set(ByVal value As String)
                _LaneNumber = Left(value, 150)
            End Set
        End Property

        Private _LaneName As String = ""
        <DataMember()> _
        Public Property LaneName() As String
            Get
                Return Left(_LaneName, 140)
            End Get
            Set(ByVal value As String)
                _LaneName = Left(value, 140)
            End Set
        End Property

        Private _LaneNumberMaster As String = ""
        <DataMember()> _
        Public Property LaneNumberMaster() As String
            Get
                Return Left(_LaneNumberMaster, 50)
            End Get
            Set(ByVal value As String)
                _LaneNumberMaster = Left(value, 50)
            End Set
        End Property

        Private _LaneNameMaster As String = ""
        <DataMember()> _
        Public Property LaneNameMaster() As String
            Get
                Return Left(_LaneNameMaster, 50)
            End Get
            Set(ByVal value As String)
                _LaneNameMaster = Left(value, 50)
            End Set
        End Property

        Private _LaneCompControl As Integer = 0
        <DataMember()> _
        Public Property LaneCompControl() As Integer
            Get
                Return _LaneCompControl
            End Get
            Set(ByVal value As Integer)
                _LaneCompControl = value
            End Set
        End Property

        Private _LaneDefaultCarrierUse As Boolean = False
        <DataMember()> _
        Public Property LaneDefaultCarrierUse() As Boolean
            Get
                Return _LaneDefaultCarrierUse
            End Get
            Set(ByVal value As Boolean)
                _LaneDefaultCarrierUse = value
            End Set
        End Property

        Private _LaneDefaultCarrierControl As Integer = 0
        <DataMember()> _
        Public Property LaneDefaultCarrierControl() As Integer
            Get
                Return _LaneDefaultCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LaneDefaultCarrierControl = value
            End Set
        End Property

        Private _LaneDefaultCarrierContact As String = ""
        <DataMember()> _
        Public Property LaneDefaultCarrierContact() As String
            Get
                Return Left(_LaneDefaultCarrierContact, 30)
            End Get
            Set(ByVal value As String)
                _LaneDefaultCarrierContact = Left(value, 30)
            End Set
        End Property

        Private _LaneDefaultCarrierPhone As String = ""
        <DataMember()> _
        Public Property LaneDefaultCarrierPhone() As String
            Get
                Return Left(_LaneDefaultCarrierPhone, 20)
            End Get
            Set(ByVal value As String)
                _LaneDefaultCarrierPhone = Left(value, 20)
            End Set
        End Property

        Private _LaneOrigCompControl As Integer = 0
        <DataMember()> _
        Public Property LaneOrigCompControl() As Integer
            Get
                Return _LaneOrigCompControl
            End Get
            Set(ByVal value As Integer)
                _LaneOrigCompControl = value
            End Set
        End Property

        Private _LaneOrigName As String = ""
        <DataMember()> _
        Public Property LaneOrigName() As String
            Get
                Return Left(_LaneOrigName, 40)
            End Get
            Set(ByVal value As String)
                _LaneOrigName = Left(value, 40)
            End Set
        End Property

        Private _LaneOrigAddress1 As String = ""
        <DataMember()> _
        Public Property LaneOrigAddress1() As String
            Get
                Return Left(_LaneOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LaneOrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _LaneOrigAddress2 As String = ""
        <DataMember()> _
        Public Property LaneOrigAddress2() As String
            Get
                Return Left(_LaneOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LaneOrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _LaneOrigAddress3 As String = ""
        <DataMember()> _
        Public Property LaneOrigAddress3() As String
            Get
                Return Left(_LaneOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _LaneOrigAddress3 = Left(value, 40)
            End Set
        End Property

        Private _LaneOrigCity As String = ""
        <DataMember()> _
        Public Property LaneOrigCity() As String
            Get
                Return Left(_LaneOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _LaneOrigCity = Left(value, 25)
            End Set
        End Property

        Private _LaneOrigState As String = ""
        <DataMember()> _
        Public Property LaneOrigState() As String
            Get
                Return Left(_LaneOrigState, 8)
            End Get
            Set(ByVal value As String)
                _LaneOrigState = Left(value, 8)
            End Set
        End Property

        Private _LaneOrigCountry As String = ""
        <DataMember()> _
        Public Property LaneOrigCountry() As String
            Get
                Return Left(_LaneOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _LaneOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _LaneOrigZip As String = ""
        <DataMember()> _
        Public Property LaneOrigZip() As String
            Get
                Return Left(_LaneOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LaneOrigContactPhone As String = ""
        <DataMember()> _
        Public Property LaneOrigContactPhone() As String
            Get
                Return Left(_LaneOrigContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneOrigContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LaneOrigContactPhoneExt As String = ""
        <DataMember()> _
        Public Property LaneOrigContactPhoneExt() As String
            Get
                Return Left(_LaneOrigContactPhoneExt, 50)
            End Get
            Set(ByVal value As String)
                _LaneOrigContactPhoneExt = Left(value, 50)
            End Set
        End Property

        Private _LaneOrigContactFax As String = ""
        <DataMember()> _
        Public Property LaneOrigContactFax() As String
            Get
                Return Left(_LaneOrigContactFax, 15)
            End Get
            Set(ByVal value As String)
                _LaneOrigContactFax = Left(value, 15)
            End Set
        End Property

        Private _LaneDestCompControl As Integer = 0
        <DataMember()> _
        Public Property LaneDestCompControl() As Integer
            Get
                Return _LaneDestCompControl
            End Get
            Set(ByVal value As Integer)
                _LaneDestCompControl = value
            End Set
        End Property

        Private _LaneDestName As String = ""
        <DataMember()> _
        Public Property LaneDestName() As String
            Get
                Return Left(_LaneDestName, 40)
            End Get
            Set(ByVal value As String)
                _LaneDestName = Left(value, 40)
            End Set
        End Property

        Private _LaneDestAddress1 As String = ""
        <DataMember()> _
        Public Property LaneDestAddress1() As String
            Get
                Return Left(_LaneDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LaneDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _LaneDestAddress2 As String = ""
        <DataMember()> _
        Public Property LaneDestAddress2() As String
            Get
                Return Left(_LaneDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LaneDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _LaneDestAddress3 As String = ""
        <DataMember()> _
        Public Property LaneDestAddress3() As String
            Get
                Return Left(_LaneDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _LaneDestAddress3 = Left(value, 40)
            End Set
        End Property

        Private _LaneDestCity As String = ""
        <DataMember()> _
        Public Property LaneDestCity() As String
            Get
                Return Left(_LaneDestCity, 25)
            End Get
            Set(ByVal value As String)
                _LaneDestCity = Left(value, 25)
            End Set
        End Property

        Private _LaneDestState As String = ""
        <DataMember()> _
        Public Property LaneDestState() As String
            Get
                Return Left(_LaneDestState, 2)
            End Get
            Set(ByVal value As String)
                _LaneDestState = Left(value, 2)
            End Set
        End Property

        Private _LaneDestCountry As String = ""
        <DataMember()> _
        Public Property LaneDestCountry() As String
            Get
                Return Left(_LaneDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _LaneDestCountry = Left(value, 30)
            End Set
        End Property

        Private _LaneDestZip As String = ""
        <DataMember()> _
        Public Property LaneDestZip() As String
            Get
                Return Left(_LaneDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LaneDestContactPhone As String = ""
        <DataMember()> _
        Public Property LaneDestContactPhone() As String
            Get
                Return Left(_LaneDestContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneDestContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LaneDestContactPhoneExt As String = ""
        <DataMember()> _
        Public Property LaneDestContactPhoneExt() As String
            Get
                Return Left(_LaneDestContactPhoneExt, 50)
            End Get
            Set(ByVal value As String)
                _LaneDestContactPhoneExt = Left(value, 50)
            End Set
        End Property

        Private _LaneDestContactFax As String = ""
        <DataMember()> _
        Public Property LaneDestContactFax() As String
            Get
                Return Left(_LaneDestContactFax, 15)
            End Get
            Set(ByVal value As String)
                _LaneDestContactFax = Left(value, 15)
            End Set
        End Property

        Private _LaneModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneModDate() As System.Nullable(Of Date)
            Get
                Return _LaneModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneModDate = value
            End Set
        End Property

        Private _LaneModUser As String = ""
        <DataMember()> _
        Public Property LaneModUser() As String
            Get
                Return Left(_LaneModUser, 100)
            End Get
            Set(ByVal value As String)
                _LaneModUser = Left(value, 100)
            End Set
        End Property

        Private _LaneConsigneeNumber As String = ""
        <DataMember()> _
        Public Property LaneConsigneeNumber() As String
            Get
                Return Left(_LaneConsigneeNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneConsigneeNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneRecMinIn As Integer = 0
        <DataMember()> _
        Public Property LaneRecMinIn() As Integer
            Get
                Return _LaneRecMinIn
            End Get
            Set(ByVal value As Integer)
                _LaneRecMinIn = value
            End Set
        End Property

        Private _LaneRecMinUnload As Integer = 0
        <DataMember()> _
        Public Property LaneRecMinUnload() As Integer
            Get
                Return _LaneRecMinUnload
            End Get
            Set(ByVal value As Integer)
                _LaneRecMinUnload = value
            End Set
        End Property

        Private _LaneRecMinOut As Integer = 0
        <DataMember()> _
        Public Property LaneRecMinOut() As Integer
            Get
                Return _LaneRecMinOut
            End Get
            Set(ByVal value As Integer)
                _LaneRecMinOut = value
            End Set
        End Property

        Private _LaneAppt As Boolean = False
        <DataMember()> _
        Public Property LaneAppt() As Boolean
            Get
                Return _LaneAppt
            End Get
            Set(ByVal value As Boolean)
                _LaneAppt = value
            End Set
        End Property

        Private _LanePalletExchange As Boolean = False
        <DataMember()> _
        Public Property LanePalletExchange() As Boolean
            Get
                Return _LanePalletExchange
            End Get
            Set(ByVal value As Boolean)
                _LanePalletExchange = value
            End Set
        End Property

        Private _LanePalletType As String = ""
        <DataMember()> _
        Public Property LanePalletType() As String
            Get
                Return Left(_LanePalletType, 50)
            End Get
            Set(ByVal value As String)
                _LanePalletType = Left(value, 50)
            End Set
        End Property

        Private _LaneBenchMiles As Double = 0
        <DataMember()> _
        Public Property LaneBenchMiles() As Double
            Get
                Return _LaneBenchMiles
            End Get
            Set(ByVal value As Double)
                _LaneBenchMiles = value
            End Set
        End Property

        Private _LaneBFC As Double = 0
        <DataMember()> _
        Public Property LaneBFC() As Double
            Get
                Return _LaneBFC
            End Get
            Set(ByVal value As Double)
                _LaneBFC = value
            End Set
        End Property

        Private _LaneBFCType As String = ""
        <DataMember()> _
        Public Property LaneBFCType() As String
            Get
                Return Left(_LaneBFCType, 50)
            End Get
            Set(ByVal value As String)
                _LaneBFCType = Left(value, 50)
            End Set
        End Property

        Private _LaneRecHourStart As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneRecHourStart() As System.Nullable(Of Date)
            Get
                Return _LaneRecHourStart
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneRecHourStart = value
            End Set
        End Property

        Private _LaneRecHourStop As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneRecHourStop() As System.Nullable(Of Date)
            Get
                Return _LaneRecHourStop
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneRecHourStop = value
            End Set
        End Property

        Private _LaneDestHourStart As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneDestHourStart() As System.Nullable(Of Date)
            Get
                Return _LaneDestHourStart
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneDestHourStart = value
            End Set
        End Property

        Private _LaneDestHourStop As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneDestHourStop() As System.Nullable(Of Date)
            Get
                Return _LaneDestHourStop
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneDestHourStop = value
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

        Private _LaneCommentsConfidential As String = ""
        <DataMember()> _
        Public Property LaneCommentsConfidential() As String
            Get
                Return Left(_LaneCommentsConfidential, 255)
            End Get
            Set(ByVal value As String)
                _LaneCommentsConfidential = Left(value, 255)
            End Set
        End Property

        Private _LaneCurType As Integer = 0
        <DataMember()> _
        Public Property LaneCurType() As Integer
            Get
                Return _LaneCurType
            End Get
            Set(ByVal value As Integer)
                _LaneCurType = value
            End Set
        End Property

        Private _LaneActive As Boolean = True
        <DataMember()> _
        Public Property LaneActive() As Boolean
            Get
                Return _LaneActive
            End Get
            Set(ByVal value As Boolean)
                _LaneActive = value
            End Set
        End Property

        Private _LaneLatitude As Double = 0
        <DataMember()> _
        Public Property LaneLatitude() As Double
            Get
                Return _LaneLatitude
            End Get
            Set(ByVal value As Double)
                _LaneLatitude = value
            End Set
        End Property

        Private _LaneLongitude As Double = 0
        <DataMember()> _
        Public Property LaneLongitude() As Double
            Get
                Return _LaneLongitude
            End Get
            Set(ByVal value As Double)
                _LaneLongitude = value
            End Set
        End Property

        Private _LaneTempType As String = ""
        <DataMember()> _
        Public Property LaneTempType() As String
            Get
                Return Left(_LaneTempType, 50)
            End Get
            Set(ByVal value As String)
                _LaneTempType = Left(value, 50)
            End Set
        End Property

        Private _LaneTransType As String = ""
        <DataMember()> _
        Public Property LaneTransType() As String
            Get
                Return Left(_LaneTransType, 50)
            End Get
            Set(ByVal value As String)
                _LaneTransType = Left(value, 50)
            End Set
        End Property

        Private _LanePrimaryBuyer As String = ""
        <DataMember()> _
        Public Property LanePrimaryBuyer() As String
            Get
                Return Left(_LanePrimaryBuyer, 50)
            End Get
            Set(ByVal value As String)
                _LanePrimaryBuyer = Left(value, 50)
            End Set
        End Property

        Private _LaneOLTBenchmark As Integer = 0
        <DataMember()> _
        Public Property LaneOLTBenchmark() As Integer
            Get
                Return _LaneOLTBenchmark
            End Get
            Set(ByVal value As Integer)
                _LaneOLTBenchmark = value
            End Set
        End Property

        Private _LaneTLTBenchmark As Integer = 0
        <DataMember()> _
        Public Property LaneTLTBenchmark() As Integer
            Get
                Return _LaneTLTBenchmark
            End Get
            Set(ByVal value As Integer)
                _LaneTLTBenchmark = value
            End Set
        End Property

        Private _LaneAptDelivery As Boolean = False
        <DataMember()> _
        Public Property LaneAptDelivery() As Boolean
            Get
                Return _LaneAptDelivery
            End Get
            Set(ByVal value As Boolean)
                _LaneAptDelivery = value
            End Set
        End Property

        Private _LaneUpdated As Byte()
        <DataMember()> _
        Public Property LaneUpdated() As Byte()
            Get
                Return _LaneUpdated
            End Get
            Set(ByVal value As Byte())
                _LaneUpdated = value
            End Set
        End Property

        Private _LaneOrderControl As Integer = 0
        <DataMember()> _
        Public Property LaneOrderControl() As Integer
            Get
                Return _LaneOrderControl
            End Get
            Set(ByVal value As Integer)
                _LaneOrderControl = value
            End Set
        End Property

        Private _LaneOrderSTDWgt As Integer = 0
        <DataMember()> _
        Public Property LaneOrderSTDWgt() As Integer
            Get
                Return _LaneOrderSTDWgt
            End Get
            Set(ByVal value As Integer)
                _LaneOrderSTDWgt = value
            End Set
        End Property

        Private _LaneOrderSTDCases As Integer = 0
        <DataMember()> _
        Public Property LaneOrderSTDCases() As Integer
            Get
                Return _LaneOrderSTDCases
            End Get
            Set(ByVal value As Integer)
                _LaneOrderSTDCases = value
            End Set
        End Property

        Private _LaneOrderSTDCubes As Integer = 0
        <DataMember()> _
        Public Property LaneOrderSTDCubes() As Integer
            Get
                Return _LaneOrderSTDCubes
            End Get
            Set(ByVal value As Integer)
                _LaneOrderSTDCubes = value
            End Set
        End Property

        Private _LaneOrderSTDPUAllow As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderSTDPUAllow() As Decimal
            Get
                Return _LaneOrderSTDPUAllow
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderSTDPUAllow = value
            End Set
        End Property

        Private _LaneOrderSTDAllowType As String = ""
        <DataMember()> _
        Public Property LaneOrderSTDAllowType() As String
            Get
                Return Left(_LaneOrderSTDAllowType, 4)
            End Get
            Set(ByVal value As String)
                _LaneOrderSTDAllowType = Left(value, 4)
            End Set
        End Property

        Private _LaneOrderSTDAllowValue As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderSTDAllowValue() As Decimal
            Get
                Return _LaneOrderSTDAllowValue
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderSTDAllowValue = value
            End Set
        End Property

        Private _LaneOrderSTDMonthlyOrder As Integer = 0
        <DataMember()> _
        Public Property LaneOrderSTDMonthlyOrder() As Integer
            Get
                Return _LaneOrderSTDMonthlyOrder
            End Get
            Set(ByVal value As Integer)
                _LaneOrderSTDMonthlyOrder = value
            End Set
        End Property

        Private _LaneOrderSTDYearlyFRT As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderSTDYearlyFRT() As Decimal
            Get
                Return _LaneOrderSTDYearlyFRT
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderSTDYearlyFRT = value
            End Set
        End Property

        Private _LaneOrderSTDCarrierControl As Integer = 0
        <DataMember()> _
        Public Property LaneOrderSTDCarrierControl() As Integer
            Get
                Return _LaneOrderSTDCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LaneOrderSTDCarrierControl = value
            End Set
        End Property

        Private _LaneOrderSTDCostMile As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderSTDCostMile() As Decimal
            Get
                Return _LaneOrderSTDCostMile
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderSTDCostMile = value
            End Set
        End Property

        Private _LaneOrderSTDCostCWT As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderSTDCostCWT() As Decimal
            Get
                Return _LaneOrderSTDCostCWT
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderSTDCostCWT = value
            End Set
        End Property

        Private _LaneOrderSTDCostFlat As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderSTDCostFlat() As Decimal
            Get
                Return _LaneOrderSTDCostFlat
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderSTDCostFlat = value
            End Set
        End Property

        Private _LaneOrderACTWgt As Integer = 0
        <DataMember()> _
        Public Property LaneOrderACTWgt() As Integer
            Get
                Return _LaneOrderACTWgt
            End Get
            Set(ByVal value As Integer)
                _LaneOrderACTWgt = value
            End Set
        End Property

        Private _LaneOrderACTCases As Integer = 0
        <DataMember()> _
        Public Property LaneOrderACTCases() As Integer
            Get
                Return _LaneOrderACTCases
            End Get
            Set(ByVal value As Integer)
                _LaneOrderACTCases = value
            End Set
        End Property

        Private _LaneOrderACTCubes As Integer = 0
        <DataMember()> _
        Public Property LaneOrderACTCubes() As Integer
            Get
                Return _LaneOrderACTCubes
            End Get
            Set(ByVal value As Integer)
                _LaneOrderACTCubes = value
            End Set
        End Property

        Private _LaneOrderACTPUAllow As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderACTPUAllow() As Decimal
            Get
                Return _LaneOrderACTPUAllow
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderACTPUAllow = value
            End Set
        End Property

        Private _LaneOrderACTAllowType As String = ""
        <DataMember()> _
        Public Property LaneOrderACTAllowType() As String
            Get
                Return Left(_LaneOrderACTAllowType, 4)
            End Get
            Set(ByVal value As String)
                _LaneOrderACTAllowType = Left(value, 4)
            End Set
        End Property

        Private _LaneOrderACTAllowValue As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderACTAllowValue() As Decimal
            Get
                Return _LaneOrderACTAllowValue
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderACTAllowValue = value
            End Set
        End Property

        Private _LaneOrderACTMonthlyOrder As Integer = 0
        <DataMember()> _
        Public Property LaneOrderACTMonthlyOrder() As Integer
            Get
                Return _LaneOrderACTMonthlyOrder
            End Get
            Set(ByVal value As Integer)
                _LaneOrderACTMonthlyOrder = value
            End Set
        End Property

        Private _LaneOrderACTYearlyFRT As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderACTYearlyFRT() As Decimal
            Get
                Return _LaneOrderACTYearlyFRT
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderACTYearlyFRT = value
            End Set
        End Property

        Private _LaneOrderACTCarrierControl As Integer = 0
        <DataMember()> _
        Public Property LaneOrderACTCarrierControl() As Integer
            Get
                Return _LaneOrderACTCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LaneOrderACTCarrierControl = value
            End Set
        End Property

        Private _LaneOrderACTCostMile As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderACTCostMile() As Decimal
            Get
                Return _LaneOrderACTCostMile
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderACTCostMile = value
            End Set
        End Property

        Private _LaneOrderACTCostFlat As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderACTCostFlat() As Decimal
            Get
                Return _LaneOrderACTCostFlat
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderACTCostFlat = value
            End Set
        End Property

        Private _LaneOrderACTCostCWT As Decimal = 0
        <DataMember()> _
        Public Property LaneOrderACTCostCWT() As Decimal
            Get
                Return _LaneOrderACTCostCWT
            End Get
            Set(ByVal value As Decimal)
                _LaneOrderACTCostCWT = value
            End Set
        End Property

        Private _LaneStops As Integer = 0
        <DataMember()> _
        Public Property LaneStops() As Integer
            Get
                Return _LaneStops
            End Get
            Set(ByVal value As Integer)
                _LaneStops = value
            End Set
        End Property

        Private _LaneFixedTime As String = ""
        <DataMember()> _
        Public Property LaneFixedTime() As String
            Get
                Return Left(_LaneFixedTime, 50)
            End Get
            Set(ByVal value As String)
                _LaneFixedTime = Left(value, 50)
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

        Private _LaneSDFUse As Boolean = False
        <DataMember()> _
        Public Property LaneSDFUse() As Boolean
            Get
                Return _LaneSDFUse
            End Get
            Set(ByVal value As Boolean)
                _LaneSDFUse = value
            End Set
        End Property

        Private _LaneSDFSRZone As Integer = 0
        <DataMember()> _
        Public Property LaneSDFSRZone() As Integer
            Get
                Return _LaneSDFSRZone
            End Get
            Set(ByVal value As Integer)
                _LaneSDFSRZone = value
            End Set
        End Property

        Private _LaneSDFMRZone As Integer = 0
        <DataMember()> _
        Public Property LaneSDFMRZone() As Integer
            Get
                Return _LaneSDFMRZone
            End Get
            Set(ByVal value As Integer)
                _LaneSDFMRZone = value
            End Set
        End Property

        Private _LaneSDFFixedTime As Integer = 0
        <DataMember()> _
        Public Property LaneSDFFixedTime() As Integer
            Get
                Return _LaneSDFFixedTime
            End Get
            Set(ByVal value As Integer)
                _LaneSDFFixedTime = value
            End Set
        End Property

        Private _LaneSDFEarlyTM1 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM1() As Integer
            Get
                Return _LaneSDFEarlyTM1
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM1 = value
            End Set
        End Property

        Private _LaneSDFLateTM1 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM1() As Integer
            Get
                Return _LaneSDFLateTM1
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM1 = value
            End Set
        End Property

        Private _LaneSDFDay1 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay1() As String
            Get
                Return Left(_LaneSDFDay1, 50)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay1 = Left(value, 50)
            End Set
        End Property

        Private _LaneSDFEarlyTM2 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM2() As Integer
            Get
                Return _LaneSDFEarlyTM2
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM2 = value
            End Set
        End Property

        Private _LaneSDFLateTM2 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM2() As Integer
            Get
                Return _LaneSDFLateTM2
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM2 = value
            End Set
        End Property

        Private _LaneSDFDay2 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay2() As String
            Get
                Return Left(_LaneSDFDay2, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay2 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFEarlyTM3 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM3() As Integer
            Get
                Return _LaneSDFEarlyTM3
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM3 = value
            End Set
        End Property

        Private _LaneSDFLateTM3 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM3() As Integer
            Get
                Return _LaneSDFLateTM3
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM3 = value
            End Set
        End Property

        Private _LaneSDFDay3 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay3() As String
            Get
                Return Left(_LaneSDFDay3, 11)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay3 = Left(value, 11)
            End Set
        End Property

        Private _LaneSDFEarlyTM4 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM4() As Integer
            Get
                Return _LaneSDFEarlyTM4
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM4 = value
            End Set
        End Property

        Private _LaneSDFLateTM4 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM4() As Integer
            Get
                Return _LaneSDFLateTM4
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM4 = value
            End Set
        End Property

        Private _LaneSDFDay4 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay4() As String
            Get
                Return Left(_LaneSDFDay4, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay4 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFEarlyTM5 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM5() As Integer
            Get
                Return _LaneSDFEarlyTM5
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM5 = value
            End Set
        End Property

        Private _LaneSDFLateTM5 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM5() As Integer
            Get
                Return _LaneSDFLateTM5
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM5 = value
            End Set
        End Property

        Private _LaneSDFDay5 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay5() As String
            Get
                Return Left(_LaneSDFDay5, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay5 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFEarlyTM6 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM6() As Integer
            Get
                Return _LaneSDFEarlyTM6
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM6 = value
            End Set
        End Property

        Private _LaneSDFLateTM6 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM6() As Integer
            Get
                Return _LaneSDFLateTM6
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM6 = value
            End Set
        End Property

        Private _LaneSDFDay6 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay6() As String
            Get
                Return Left(_LaneSDFDay6, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay6 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFEarlyTM7 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFEarlyTM7() As Integer
            Get
                Return _LaneSDFEarlyTM7
            End Get
            Set(ByVal value As Integer)
                _LaneSDFEarlyTM7 = value
            End Set
        End Property

        Private _LaneSDFLateTM7 As Integer = 0
        <DataMember()> _
        Public Property LaneSDFLateTM7() As Integer
            Get
                Return _LaneSDFLateTM7
            End Get
            Set(ByVal value As Integer)
                _LaneSDFLateTM7 = value
            End Set
        End Property

        Private _LaneSDFDay7 As String = ""
        <DataMember()> _
        Public Property LaneSDFDay7() As String
            Get
                Return Left(_LaneSDFDay7, 10)
            End Get
            Set(ByVal value As String)
                _LaneSDFDay7 = Left(value, 10)
            End Set
        End Property

        Private _LaneSDFUnldRate1 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate1() As Decimal
            Get
                Return _LaneSDFUnldRate1
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate1 = value
            End Set
        End Property

        Private _LaneSDFUnldRate2 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate2() As Decimal
            Get
                Return _LaneSDFUnldRate2
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate2 = value
            End Set
        End Property

        Private _LaneSDFUnldRate3 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate3() As Decimal
            Get
                Return _LaneSDFUnldRate3
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate3 = value
            End Set
        End Property

        Private _LaneSDFUnldRate4 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate4() As Decimal
            Get
                Return _LaneSDFUnldRate4
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate4 = value
            End Set
        End Property

        Private _LaneSDFUnldRate5 As Decimal = 0
        <DataMember()> _
        Public Property LaneSDFUnldRate5() As Decimal
            Get
                Return _LaneSDFUnldRate5
            End Get
            Set(ByVal value As Decimal)
                _LaneSDFUnldRate5 = value
            End Set
        End Property

        Private _LaneAutoTenderFlag As Boolean = False
        <DataMember()> _
        Public Property LaneAutoTenderFlag() As Boolean
            Get
                Return _LaneAutoTenderFlag
            End Get
            Set(ByVal value As Boolean)
                _LaneAutoTenderFlag = value
            End Set
        End Property


        Private _LaneCascadingDispatchingFlag As Boolean = False
        <DataMember()> _
        Public Property LaneCascadingDispatchingFlag() As Boolean
            Get
                Return _LaneCascadingDispatchingFlag
            End Get
            Set(ByVal value As Boolean)
                _LaneCascadingDispatchingFlag = value
            End Set
        End Property


        Private _LanePortofEntry As String = ""
        <DataMember()> _
        Public Property LanePortofEntry() As String
            Get
                Return Left(_LanePortofEntry, 255)
            End Get
            Set(ByVal value As String)
                _LanePortofEntry = Left(value, 255)
            End Set
        End Property

        Private _LaneDoNotInvoice As Boolean = False
        <DataMember()> _
        Public Property LaneDoNotInvoice() As Boolean
            Get
                Return _LaneDoNotInvoice
            End Get
            Set(ByVal value As Boolean)
                _LaneDoNotInvoice = value
            End Set
        End Property


        Private _LaneTLCases As Integer = 0
        <DataMember()> _
        Public Property LaneTLCases() As Integer
            Get
                Return _LaneTLCases
            End Get
            Set(ByVal value As Integer)
                _LaneTLCases = value
            End Set
        End Property

        Private _LaneTLWgt As Double = 0
        <DataMember()> _
        Public Property LaneTLWgt() As Double
            Get
                Return _LaneTLWgt
            End Get
            Set(ByVal value As Double)
                _LaneTLWgt = value
            End Set
        End Property

        Private _LaneTLCube As Integer = 0
        <DataMember()> _
        Public Property LaneTLCube() As Integer
            Get
                Return _LaneTLCube
            End Get
            Set(ByVal value As Integer)
                _LaneTLCube = value
            End Set
        End Property

        Private _LaneTLPL As Double = 0
        <DataMember()> _
        Public Property LaneTLPL() As Double
            Get
                Return _LaneTLPL
            End Get
            Set(ByVal value As Double)
                _LaneTLPL = value
            End Set
        End Property

        Private _LaneChepGLID As String = ""
        <DataMember()> _
        Public Property LaneChepGLID() As String
            Get
                Return Left(_LaneChepGLID, 50)
            End Get
            Set(ByVal value As String)
                _LaneChepGLID = Left(value, 50)
            End Set
        End Property

        Private _LaneCarrierTypeCode As String = ""
        <DataMember()> _
        Public Property LaneCarrierTypeCode() As String
            Get
                Return Left(_LaneCarrierTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _LaneCarrierTypeCode = Left(value, 20)
            End Set
        End Property

        Private _LaneCarrierEquipmentCodes As String = ""
        <DataMember()> _
        Public Property LaneCarrierEquipmentCodes() As String
            Get
                Return Left(_LaneCarrierEquipmentCodes, 50)
            End Get
            Set(ByVal value As String)
                _LaneCarrierEquipmentCodes = Left(value, 50)
            End Set
        End Property

        Private _LanePickUpMon As Boolean = False
        <DataMember()> _
        Public Property LanePickUpMon() As Boolean
            Get
                Return _LanePickUpMon
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpMon = value
            End Set
        End Property


        Private _LanePickUpTue As Boolean = False
        <DataMember()> _
        Public Property LanePickUpTue() As Boolean
            Get
                Return _LanePickUpTue
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpTue = value
            End Set
        End Property


        Private _LanePickUpWed As Boolean = False
        <DataMember()> _
        Public Property LanePickUpWed() As Boolean
            Get
                Return _LanePickUpWed
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpWed = value
            End Set
        End Property


        Private _LanePickUpThu As Boolean = False
        <DataMember()> _
        Public Property LanePickUpThu() As Boolean
            Get
                Return _LanePickUpThu
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpThu = value
            End Set
        End Property


        Private _LanePickUpFri As Boolean = False
        <DataMember()> _
        Public Property LanePickUpFri() As Boolean
            Get
                Return _LanePickUpFri
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpFri = value
            End Set
        End Property


        Private _LanePickUpSat As Boolean = False
        <DataMember()> _
        Public Property LanePickUpSat() As Boolean
            Get
                Return _LanePickUpSat
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpSat = value
            End Set
        End Property


        Private _LanePickUpSun As Boolean = False
        <DataMember()> _
        Public Property LanePickUpSun() As Boolean
            Get
                Return _LanePickUpSun
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpSun = value
            End Set
        End Property


        Private _LaneDropOffMon As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffMon() As Boolean
            Get
                Return _LaneDropOffMon
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffMon = value
            End Set
        End Property


        Private _LaneDropOffTue As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffTue() As Boolean
            Get
                Return _LaneDropOffTue
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffTue = value
            End Set
        End Property


        Private _LaneDropOffWed As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffWed() As Boolean
            Get
                Return _LaneDropOffWed
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffWed = value
            End Set
        End Property


        Private _LaneDropOffThu As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffThu() As Boolean
            Get
                Return _LaneDropOffThu
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffThu = value
            End Set
        End Property


        Private _LaneDropOffFri As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffFri() As Boolean
            Get
                Return _LaneDropOffFri
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffFri = value
            End Set
        End Property


        Private _LaneDropOffSat As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffSat() As Boolean
            Get
                Return _LaneDropOffSat
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffSat = value
            End Set
        End Property


        Private _LaneDropOffSun As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffSun() As Boolean
            Get
                Return _LaneDropOffSun
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffSun = value
            End Set
        End Property


        Private _LaneOrigStopControl As Integer = 0
        <DataMember()> _
        Public Property LaneOrigStopControl() As Integer
            Get
                Return _LaneOrigStopControl
            End Get
            Set(ByVal value As Integer)
                _LaneOrigStopControl = value
            End Set
        End Property

        Private _LaneDestStopControl As Integer = 0
        <DataMember()> _
        Public Property LaneDestStopControl() As Integer
            Get
                Return _LaneDestStopControl
            End Get
            Set(ByVal value As Integer)
                _LaneDestStopControl = value
            End Set
        End Property

        Private _LaneRouteTypeCode As Integer = 6
        <DataMember()> _
        Public Property LaneRouteTypeCode() As Integer
            Get
                If _LaneRouteTypeCode = 0 Then _LaneRouteTypeCode = 6
                Return _LaneRouteTypeCode
            End Get
            Set(ByVal value As Integer)
                If value = 0 Then value = 6
                _LaneRouteTypeCode = value
            End Set
        End Property

        Private _LaneDefaultRouteSequence As Integer = 0
        <DataMember()> _
        Public Property LaneDefaultRouteSequence() As Integer
            Get
                Return _LaneDefaultRouteSequence
            End Get
            Set(ByVal value As Integer)
                _LaneDefaultRouteSequence = value
            End Set
        End Property


        Private _LaneRouteGuideControl As Integer = 0
        <DataMember()> _
        Public Property LaneRouteGuideControl() As Integer
            Get
                Return _LaneRouteGuideControl
            End Get
            Set(ByVal value As Integer)
                _LaneRouteGuideControl = value
            End Set
        End Property

        Private _LaneRouteGuideNumber As String = ""
        <DataMember()> _
        Public Property LaneRouteGuideNumber() As String
            Get
                Return Left(_LaneRouteGuideNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneRouteGuideNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneIsCrossDockFacility As Boolean = False
        <DataMember()> _
        Public Property LaneIsCrossDockFacility() As Boolean
            Get
                Return _LaneIsCrossDockFacility
            End Get
            Set(ByVal value As Boolean)
                _LaneIsCrossDockFacility = value
            End Set
        End Property

        Private _LaneRequiredOnTimeServiceLevel As Decimal = 0
        <DataMember()> _
        Public Property LaneRequiredOnTimeServiceLevel() As Decimal
            Get
                Return _LaneRequiredOnTimeServiceLevel
            End Get
            Set(ByVal value As Decimal)
                _LaneRequiredOnTimeServiceLevel = value
            End Set
        End Property

        Private _LaneCalcOnTimeServiceLevel As Decimal = 0
        <DataMember()> _
        Public Property LaneCalcOnTimeServiceLevel() As Decimal
            Get
                Return _LaneCalcOnTimeServiceLevel
            End Get
            Set(ByVal value As Decimal)
                _LaneCalcOnTimeServiceLevel = value
            End Set
        End Property

        Private _LaneCalcOnTimeNoMonthsUsed As Decimal = 0
        <DataMember()> _
        Public Property LaneCalcOnTimeNoMonthsUsed() As Decimal
            Get
                Return _LaneCalcOnTimeNoMonthsUsed
            End Get
            Set(ByVal value As Decimal)
                _LaneCalcOnTimeNoMonthsUsed = value
            End Set
        End Property

        Private _LaneModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property LaneModeTypeControl() As Integer
            Get
                Return _LaneModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _LaneModeTypeControl = value
            End Set
        End Property

        Private _LaneUser1 As String = ""
        <DataMember()> _
        Public Property LaneUser1() As String
            Get
                Return Left(_LaneUser1, 4000)
            End Get
            Set(ByVal value As String)
                _LaneUser1 = Left(value, 4000)
            End Set
        End Property

        Private _LaneUser2 As String = ""
        <DataMember()> _
        Public Property LaneUser2() As String
            Get
                Return Left(_LaneUser2, 4000)
            End Get
            Set(ByVal value As String)
                _LaneUser2 = Left(value, 4000)
            End Set
        End Property

        Private _LaneUser3 As String = ""
        <DataMember()> _
        Public Property LaneUser3() As String
            Get
                Return Left(_LaneUser3, 4000)
            End Get
            Set(ByVal value As String)
                _LaneUser3 = Left(value, 4000)
            End Set
        End Property

        Private _LaneUser4 As String = ""
        <DataMember()> _
        Public Property LaneUser4() As String
            Get
                Return Left(_LaneUser4, 4000)
            End Get
            Set(ByVal value As String)
                _LaneUser4 = Left(value, 4000)
            End Set
        End Property

        Private _LaneIsTransLoad As Boolean = False
        <DataMember()> _
        Public Property LaneIsTransLoad() As Boolean
            Get
                Return _LaneIsTransLoad
            End Get
            Set(ByVal value As Boolean)
                _LaneIsTransLoad = value
            End Set
        End Property

        Private _LaneAllowInterline As Boolean = True
        <DataMember()> _
        Public Property LaneAllowInterline() As Boolean
            Get
                Return _LaneAllowInterline
            End Get
            Set(ByVal value As Boolean)
                _LaneAllowInterline = value
            End Set
        End Property

        Private _LaneCals As List(Of LaneCal)
        <DataMember()> _
        Public Property LaneCals() As List(Of LaneCal)
            Get
                Return _LaneCals
            End Get
            Set(ByVal value As List(Of LaneCal))
                _LaneCals = value
            End Set
        End Property

        'Private _LaneCarrs As List(Of LaneCarr)
        '<DataMember()> _
        'Public Property LaneCarrs() As List(Of LaneCarr)
        '    Get
        '        Return _LaneCarrs
        '    End Get
        '    Set(ByVal value As List(Of LaneCarr))
        '        _LaneCarrs = value
        '    End Set
        'End Property

        Private _LaneCodes As List(Of LaneCode)
        <DataMember()> _
        Public Property LaneCodes() As List(Of LaneCode)
            Get
                Return _LaneCodes
            End Get
            Set(ByVal value As List(Of LaneCode))
                _LaneCodes = value
            End Set
        End Property

        Private _LaneFees As List(Of LaneFee)
        <DataMember()> _
        Public Property LaneFees() As List(Of LaneFee)
            Get
                Return _LaneFees
            End Get
            Set(ByVal value As List(Of LaneFee))
                _LaneFees = value
            End Set
        End Property

        Private _LaneSecs As List(Of LaneSec)
        <DataMember()> _
        Public Property LaneSecs() As List(Of LaneSec)
            Get
                Return _LaneSecs
            End Get
            Set(ByVal value As List(Of LaneSec))
                _LaneSecs = value
            End Set
        End Property

        Private _LaneLegalEntity As String = ""
        <DataMember()> _
        Public Property LaneLegalEntity() As String
            Get
                Return Left(_LaneLegalEntity, 50)
            End Get
            Set(ByVal value As String)
                _LaneLegalEntity = Left(value, 50)
            End Set
        End Property

        'Added By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _LaneRestrictCarrierSelection As Boolean = False
        <DataMember()> _
        Public Property LaneRestrictCarrierSelection() As Boolean
            Get
                Return _LaneRestrictCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _LaneRestrictCarrierSelection = value
            End Set
        End Property

        'Added By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _LaneWarnOnRestrictedCarrierSelection As Boolean = False
        <DataMember()> _
        Public Property LaneWarnOnRestrictedCarrierSelection() As Boolean
            Get
                Return _LaneWarnOnRestrictedCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _LaneWarnOnRestrictedCarrierSelection = value
            End Set
        End Property

        'Added By LVV on 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _LaneRestrictedAtCompLevel As Boolean = False
        <DataMember()>
        Public Property LaneRestrictedAtCompLevel() As Boolean
            Get
                Return _LaneRestrictedAtCompLevel
            End Get
            Set(ByVal value As Boolean)
                _LaneRestrictedAtCompLevel = value
            End Set
        End Property

        'Begin Modified by RHR for v-8.1 on 03/26/2018
        Private _LaneOrigContactName As String = ""
        <DataMember()>
        Public Property LaneOrigContactName() As String
            Get
                Return Left(_LaneOrigContactName, 50)
            End Get
            Set(ByVal value As String)
                _LaneOrigContactName = Left(value, 50)
            End Set
        End Property

        Private _LaneOrigContactEmail As String = ""
        <DataMember()>
        Public Property LaneOrigContactEmail() As String
            Get
                Return Left(_LaneOrigContactEmail, 50)
            End Get
            Set(ByVal value As String)
                _LaneOrigContactEmail = Left(value, 50)
            End Set
        End Property

        Private _LaneOrigEmergencyContactPhone As String = ""
        <DataMember()>
        Public Property LaneOrigEmergencyContactPhone() As String
            Get
                Return Left(_LaneOrigEmergencyContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneOrigEmergencyContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LaneOrigEmergencyContactName As String = ""
        <DataMember()>
        Public Property LaneOrigEmergencyContactName() As String
            Get
                Return Left(_LaneOrigEmergencyContactName, 50)
            End Get
            Set(ByVal value As String)
                _LaneOrigEmergencyContactName = Left(value, 50)
            End Set
        End Property

        Private _LaneDestContactName As String = ""
        <DataMember()>
        Public Property LaneDestContactName() As String
            Get
                Return Left(_LaneDestContactName, 50)
            End Get
            Set(ByVal value As String)
                _LaneDestContactName = Left(value, 50)
            End Set
        End Property

        Private _LaneDestContactEmail As String = ""
        <DataMember()>
        Public Property LaneDestContactEmail() As String
            Get
                Return Left(_LaneDestContactEmail, 50)
            End Get
            Set(ByVal value As String)
                _LaneDestContactEmail = Left(value, 50)
            End Set
        End Property

        Private _LaneDestEmergencyContactPhone As String = ""
        <DataMember()>
        Public Property LaneDestEmergencyContactPhone() As String
            Get
                Return Left(_LaneDestEmergencyContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneDestEmergencyContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LaneDestEmergencyContactName As String = ""
        <DataMember()>
        Public Property LaneDestEmergencyContactName() As String
            Get
                Return Left(_LaneDestEmergencyContactName, 50)
            End Get
            Set(ByVal value As String)
                _LaneDestEmergencyContactName = Left(value, 50)
            End Set
        End Property

        Private _LaneWeightUnit As String = ""
        <DataMember()>
        Public Property LaneWeightUnit() As String
            Get
                Return Left(_LaneWeightUnit, 4)
            End Get
            Set(ByVal value As String)
                _LaneWeightUnit = Left(value, 4)
            End Set
        End Property


        Private _LaneLengthUnit As String = ""
        <DataMember()>
        Public Property LaneLengthUnit() As String
            Get
                Return Left(_LaneLengthUnit, 4)
            End Get
            Set(ByVal value As String)
                _LaneLengthUnit = Left(value, 4)
            End Set
        End Property

        'End Modified by RHR for v-8.1 on 03/26/2018

        'Start Modified by RHR for v-8.4 on 4/22/2021
        Private _LaneAllowCarrierBookApptByEmail As Boolean = False
        'Turn Carrier Book Appt With Token via Email On Or off
        <DataMember()>
        Public Property LaneAllowCarrierBookApptByEmail() As Boolean
            Get
                Return _LaneAllowCarrierBookApptByEmail
            End Get
            Set(ByVal value As Boolean)
                _LaneAllowCarrierBookApptByEmail = value
            End Set
        End Property

        Private _LaneRequireCarrierAuthBookApptByEmail As Boolean = False
        'Require Carrier Username And Password For Carrier Book Appt In addition To Token
        <DataMember()>
        Public Property LaneRequireCarrierAuthBookApptByEmail() As Boolean
            Get
                Return _LaneRequireCarrierAuthBookApptByEmail
            End Get
            Set(ByVal value As Boolean)
                _LaneRequireCarrierAuthBookApptByEmail = value
            End Set
        End Property

        Private _LaneUseCarrieContEmailForBookApptByEmail As Boolean = False
        'Use Default Carrier Contact Email instead Of Book Appt Email 
        <DataMember()>
        Public Property LaneUseCarrieContEmailForBookApptByEmail() As Boolean
            Get
                Return _LaneUseCarrieContEmailForBookApptByEmail
            End Get
            Set(ByVal value As Boolean)
                _LaneUseCarrieContEmailForBookApptByEmail = value
            End Set
        End Property

        Private _LaneCarrierBookApptviaTokenEmail As String = ""
        'Default Email When 	LaneAllowCarrierBookApptByEmail Is True
        <DataMember()>
        Public Property LaneCarrierBookApptviaTokenEmail() As String
            Get
                Return Left(_LaneCarrierBookApptviaTokenEmail, 255)
            End Get
            Set(ByVal value As String)
                _LaneCarrierBookApptviaTokenEmail = Left(value, 255)
            End Set
        End Property

        Private _LaneCarrierBookApptviaTokenFailEmail As String = ""
        'Contact Email If Token Fails
        <DataMember()>
        Public Property LaneCarrierBookApptviaTokenFailEmail() As String
            Get
                Return Left(_LaneCarrierBookApptviaTokenFailEmail, 255)
            End Get
            Set(ByVal value As String)
                _LaneCarrierBookApptviaTokenFailEmail = Left(value, 255)
            End Set
        End Property

        Private _LaneCarrierBookApptviaTokenFailPhone As String = ""
        'Contact Phone If Token Fails
        <DataMember()>
        Public Property LaneCarrierBookApptviaTokenFailPhone() As String
            Get
                Return Left(_LaneCarrierBookApptviaTokenFailPhone, 20)
            End Get
            Set(ByVal value As String)
                _LaneCarrierBookApptviaTokenFailPhone = Left(value, 20)
            End Set
        End Property
        'End Modified by RHR for v-8.1 on 3/14/2018 
        'Start Modified by RHR for v-8.4.0.003 on 7/19/2021 Colortech Lead Time Enhancement
        Private _LaneTransLeadTimeCalcType As System.Nullable(Of Integer) = 0
        ''' <summary>
        ''' Automated Lead Time Calculate Type -- 0 = None, 1 = Calculate  Ship Date, 2 = Calculate Required Date
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.4.0.003 on 07/21/2021 
        ''' </remarks>
        Public Property LaneTransLeadTimeCalcType() As System.Nullable(Of Integer)
            Get
                Return _LaneTransLeadTimeCalcType
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _LaneTransLeadTimeCalcType = value
            End Set
        End Property

        Private _LaneTransLeadTimeUseMasterLane As System.Nullable(Of Boolean) = False
        ''' <summary>
        ''' Use Master Lane Data Flag -- User Master Lane Data for Statistical Analysis
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.4.0.003 on 07/21/2021 
        ''' </remarks>
        Public Property LaneTransLeadTimeUseMasterLane() As System.Nullable(Of Boolean)
            Get
                Return _LaneTransLeadTimeUseMasterLane
            End Get
            Set(ByVal value As System.Nullable(Of Boolean))
                _LaneTransLeadTimeUseMasterLane = value
            End Set
        End Property

        Private _LaneTransLeadTimeLocationOption As System.Nullable(Of Integer) = 0
        ''' <summary>
        ''' Statistical Analysis Options -- 0 = None, 1 = Compare By State, 2 = Compare By City , 3 = Compare By 3 digit postal code range for Statistical Analysis
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.4.0.003 on 07/21/2021 
        ''' </remarks>
        Public Property LaneTransLeadTimeLocationOption() As System.Nullable(Of Integer)
            Get
                Return _LaneTransLeadTimeLocationOption
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _LaneTransLeadTimeLocationOption = value
            End Set
        End Property
        '   End  Modified by RHR for v-8.4.0.003 on 7/19/2021 Colortech Lead Time Enhancement

        'Start Modified by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone settings
        Private _LaneOrigTimeZone As String = ""
        <DataMember()>
        Public Property LaneOrigTimeZone() As String
            Get
                Return Left(_LaneOrigTimeZone, 100)
            End Get
            Set(ByVal value As String)
                _LaneOrigTimeZone = Left(value, 100)
            End Set
        End Property

        Private _LaneDestTimeZone As String = ""
        <DataMember()>
        Public Property LaneDestTimeZone() As String
            Get
                Return Left(_LaneDestTimeZone, 100)
            End Get
            Set(ByVal value As String)
                _LaneDestTimeZone = Left(value, 100)
            End Set
        End Property

        Private _ReferenceLaneNumber As String = ""
        <DataMember()>
        Public Property ReferenceLaneNumber() As String
            Get
                Return Left(_ReferenceLaneNumber, 150)
            End Get
            Set(ByVal value As String)
                _ReferenceLaneNumber = Left(value, 150)
            End Set
        End Property

        ' End Modified by RHR for v-8.5.4.005 on 04/04/2024 New Time Zone settings

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Lane
            instance = DirectCast(MemberwiseClone(), Lane)
            instance.LaneCals = Nothing
            For Each item In LaneCals
                instance.LaneCals.Add(DirectCast(item.Clone, LaneCal))
            Next
            'instance.LaneCarrs = Nothing
            'For Each item In LaneCarrs
            '    instance.LaneCarrs.Add(DirectCast(item.Clone, LaneCarr))
            'Next
            instance.LaneCodes = Nothing
            For Each item In LaneCodes
                instance.LaneCodes.Add(DirectCast(item.Clone, LaneCode))
            Next
            instance.LaneFees = Nothing
            For Each item In LaneFees
                instance.LaneFees.Add(DirectCast(item.Clone, LaneFee))
            Next
            Return instance
        End Function

#End Region

    End Class

    

End Namespace
