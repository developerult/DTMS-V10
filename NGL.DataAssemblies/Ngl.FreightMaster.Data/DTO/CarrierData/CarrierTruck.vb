Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierTruck
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrierTruckControl As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckControl() As Integer
            Get
                Return _CarrierTruckControl
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckControl = value
            End Set
        End Property

        Private _CarrierTruckCarrierControl As Integer? = 0
        <DataMember()>
        Public Property CarrierTruckCarrierControl() As Integer?
            Get
                Return _CarrierTruckCarrierControl
            End Get
            Set(ByVal value As Integer?)
                _CarrierTruckCarrierControl = value
            End Set
        End Property

        Private _CarrierTruckDescription As String = ""
        <DataMember()> _
        Public Property CarrierTruckDescription() As String
            Get
                Return Left(_CarrierTruckDescription, 255)
            End Get
            Set(ByVal value As String)
                _CarrierTruckDescription = Left(value, 255)
            End Set
        End Property

        Private _CarrierTruckWgtFrom As Integer? = 0
        <DataMember()>
        Public Property CarrierTruckWgtFrom() As Integer?
            Get
                Return _CarrierTruckWgtFrom
            End Get
            Set(ByVal value As Integer?)
                _CarrierTruckWgtFrom = value
            End Set
        End Property

        Private _CarrierTruckWgtTo As Integer? = 0
        <DataMember()>
        Public Property CarrierTruckWgtTo() As Integer?
            Get
                Return _CarrierTruckWgtTo
            End Get
            Set(ByVal value As Integer?)
                _CarrierTruckWgtTo = value
            End Set
        End Property

        Private _CarrierTruckRateStarts As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierTruckRateStarts() As System.Nullable(Of Date)
            Get
                Return _CarrierTruckRateStarts
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierTruckRateStarts = value
            End Set
        End Property

        Private _CarrierTruckRateExpires As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierTruckRateExpires() As System.Nullable(Of Date)
            Get
                Return _CarrierTruckRateExpires
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierTruckRateExpires = value
            End Set
        End Property

        Private _CarrierTruckTL As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckTL() As Boolean
            Get
                Return _CarrierTruckTL
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckTL = value
            End Set
        End Property

        Private _CarrierTruckLTL As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckLTL() As Boolean
            Get
                Return _CarrierTruckLTL
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckLTL = value
            End Set
        End Property

        Private _CarrierTruckEquipment As String = ""
        <DataMember()> _
        Public Property CarrierTruckEquipment() As String
            Get
                Return Left(_CarrierTruckEquipment, 20)
            End Get
            Set(ByVal value As String)
                _CarrierTruckEquipment = Left(value, 20)
            End Set
        End Property

        Private _CarrierTruckMileRate As Decimal? = 0
        <DataMember()>
        Public Property CarrierTruckMileRate() As Decimal?
            Get
                Return _CarrierTruckMileRate
            End Get
            Set(ByVal value As Decimal?)
                _CarrierTruckMileRate = value
            End Set
        End Property

        Private _CarrierTruckCwtRate As Decimal? = 0
        <DataMember()>
        Public Property CarrierTruckCwtRate() As Decimal?
            Get
                Return _CarrierTruckCwtRate
            End Get
            Set(ByVal value As Decimal?)
                _CarrierTruckCwtRate = value
            End Set
        End Property

        Private _CarrierTruckCaseRate As Decimal? = 0
        <DataMember()>
        Public Property CarrierTruckCaseRate() As Decimal?
            Get
                Return _CarrierTruckCaseRate
            End Get
            Set(ByVal value As Decimal?)
                _CarrierTruckCaseRate = value
            End Set
        End Property

        Private _CarrierTruckFlatRate As Decimal? = 0
        <DataMember()>
        Public Property CarrierTruckFlatRate() As Decimal?
            Get
                Return _CarrierTruckFlatRate
            End Get
            Set(ByVal value As Decimal?)
                _CarrierTruckFlatRate = value
            End Set
        End Property

        Private _CarrierTruckPltRate As Decimal? = 0
        <DataMember()>
        Public Property CarrierTruckPltRate() As Decimal?
            Get
                Return _CarrierTruckPltRate
            End Get
            Set(ByVal value As Decimal?)
                _CarrierTruckPltRate = value
            End Set
        End Property

        Private _CarrierTruckCubeRate As Decimal? = 0
        <DataMember()>
        Public Property CarrierTruckCubeRate() As Decimal?
            Get
                Return _CarrierTruckCubeRate
            End Get
            Set(ByVal value As Decimal?)
                _CarrierTruckCubeRate = value
            End Set
        End Property

        Private _CarrierTruckTLT As Integer? = 0
        <DataMember()>
        Public Property CarrierTruckTLT() As Integer?
            Get
                Return _CarrierTruckTLT
            End Get
            Set(ByVal value As Integer?)
                _CarrierTruckTLT = value
            End Set
        End Property

        Private _CarrierTruckTMode As String = ""
        <DataMember()> _
        Public Property CarrierTruckTMode() As String
            Get
                Return Left(_CarrierTruckTMode, 3)
            End Get
            Set(ByVal value As String)
                _CarrierTruckTMode = Left(value, 3)
            End Set
        End Property

        Private _CarrierTruckFAK As String = ""
        <DataMember()> _
        Public Property CarrierTruckFAK() As String
            Get
                Return Left(_CarrierTruckFAK, 10)
            End Get
            Set(ByVal value As String)
                _CarrierTruckFAK = Left(value, 10)
            End Set
        End Property

        Private _CarrierTruckDisc As Single? = 0
        <DataMember()>
        Public Property CarrierTruckDisc() As Single?
            Get
                Return _CarrierTruckDisc
            End Get
            Set(ByVal value As Single?)
                _CarrierTruckDisc = value
            End Set
        End Property

        Private _CarrierTruckPUMon As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUMon() As Boolean
            Get
                Return _CarrierTruckPUMon
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckPUMon = value
            End Set
        End Property

        Private _CarrierTruckPUTue As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUTue() As Boolean
            Get
                Return _CarrierTruckPUTue
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckPUTue = value
            End Set
        End Property

        Private _CarrierTruckPUWed As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUWed() As Boolean
            Get
                Return _CarrierTruckPUWed
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckPUWed = value
            End Set
        End Property

        Private _CarrierTruckPUThu As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUThu() As Boolean
            Get
                Return _CarrierTruckPUThu
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckPUThu = value
            End Set
        End Property

        Private _CarrierTruckPUFri As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUFri() As Boolean
            Get
                Return _CarrierTruckPUFri
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckPUFri = value
            End Set
        End Property

        Private _CarrierTruckPUSat As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUSat() As Boolean
            Get
                Return _CarrierTruckPUSat
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckPUSat = value
            End Set
        End Property

        Private _CarrierTruckPUSun As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUSun() As Boolean
            Get
                Return _CarrierTruckPUSun
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckPUSun = value
            End Set
        End Property

        Private _CarrierTruckDLMon As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLMon() As Boolean
            Get
                Return _CarrierTruckDLMon
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckDLMon = value
            End Set
        End Property

        Private _CarrierTruckDLTue As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLTue() As Boolean
            Get
                Return _CarrierTruckDLTue
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckDLTue = value
            End Set
        End Property

        Private _CarrierTruckDLWed As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLWed() As Boolean
            Get
                Return _CarrierTruckDLWed
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckDLWed = value
            End Set
        End Property

        Private _CarrierTruckDLThu As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLThu() As Boolean
            Get
                Return _CarrierTruckDLThu
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckDLThu = value
            End Set
        End Property

        Private _CarrierTruckDLFri As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLFri() As Boolean
            Get
                Return _CarrierTruckDLFri
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckDLFri = value
            End Set
        End Property

        Private _CarrierTruckDLSat As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLSat() As Boolean
            Get
                Return _CarrierTruckDLSat
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckDLSat = value
            End Set
        End Property

        Private _CarrierTruckDLSun As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLSun() As Boolean
            Get
                Return _CarrierTruckDLSun
            End Get
            Set(ByVal value As Boolean)
                _CarrierTruckDLSun = value
            End Set
        End Property

        Private _CarrierTruckPayTolPerLo As Double? = 0
        <DataMember()>
        Public Property CarrierTruckPayTolPerLo() As Double?
            Get
                Return _CarrierTruckPayTolPerLo
            End Get
            Set(ByVal value As Double?)
                _CarrierTruckPayTolPerLo = value
            End Set
        End Property

        Private _CarrierTruckPayTolPerHi As Double? = 0
        <DataMember()>
        Public Property CarrierTruckPayTolPerHi() As Double?
            Get
                Return _CarrierTruckPayTolPerHi
            End Get
            Set(ByVal value As Double?)
                _CarrierTruckPayTolPerHi = value
            End Set
        End Property

        Private _CarrierTruckPayTolCurLo As Double? = 0
        <DataMember()>
        Public Property CarrierTruckPayTolCurLo() As Double?
            Get
                Return _CarrierTruckPayTolCurLo
            End Get
            Set(ByVal value As Double?)
                _CarrierTruckPayTolCurLo = value
            End Set
        End Property

        Private _CarrierTruckPayTolCurHi As Double? = 0
        <DataMember()>
        Public Property CarrierTruckPayTolCurHi() As Double?
            Get
                Return _CarrierTruckPayTolCurHi
            End Get
            Set(ByVal value As Double?)
                _CarrierTruckPayTolCurHi = value
            End Set
        End Property

        Private _CarrierTruckCurType As Integer? = 0
        <DataMember()>
        Public Property CarrierTruckCurType() As Integer?
            Get
                Return _CarrierTruckCurType
            End Get
            Set(ByVal value As Integer?)
                _CarrierTruckCurType = value
            End Set
        End Property

        Private _CarrierTruckModUser As String = ""
        <DataMember()> _
        Public Property CarrierTruckModUser() As String
            Get
                Return Left(_CarrierTruckModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrierTruckModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrierTruckModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierTruckModDate() As System.Nullable(Of Date)
            Get
                Return _CarrierTruckModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierTruckModDate = value
            End Set
        End Property

        Private _CarrierTruckRoute As String = ""
        <DataMember()> _
        Public Property CarrierTruckRoute() As String
            Get
                Return Left(_CarrierTruckRoute, 10)
            End Get
            Set(ByVal value As String)
                _CarrierTruckRoute = Left(value, 10)
            End Set
        End Property

        Private _CarrierTruckMiles As Integer? = 0
        <DataMember()>
        Public Property CarrierTruckMiles() As Integer?
            Get
                Return _CarrierTruckMiles
            End Get
            Set(ByVal value As Integer?)
                _CarrierTruckMiles = value
            End Set
        End Property

        Private _CarrierTruckBkhlCostPerc As Double? = 0
        <DataMember()>
        Public Property CarrierTruckBkhlCostPerc() As Double?
            Get
                Return _CarrierTruckBkhlCostPerc
            End Get
            Set(ByVal value As Double?)
                _CarrierTruckBkhlCostPerc = value
            End Set
        End Property

        Private _CarrierTruckPalletCostPer As Decimal? = 0
        <DataMember()>
        Public Property CarrierTruckPalletCostPer() As Decimal?
            Get
                Return _CarrierTruckPalletCostPer
            End Get
            Set(ByVal value As Decimal?)
                _CarrierTruckPalletCostPer = value
            End Set
        End Property

        Private _CarrierTruckFuelSurChargePerc As Double? = 0
        <DataMember()>
        Public Property CarrierTruckFuelSurChargePerc() As Double?
            Get
                Return _CarrierTruckFuelSurChargePerc
            End Get
            Set(ByVal value As Double?)
                _CarrierTruckFuelSurChargePerc = value
            End Set
        End Property

        Private _CarrierTruckStopCharge As Double? = 0
        <DataMember()>
        Public Property CarrierTruckStopCharge() As Double?
            Get
                Return _CarrierTruckStopCharge
            End Get
            Set(ByVal value As Double?)
                _CarrierTruckStopCharge = value
            End Set
        End Property

        Private _CarrierTruckDropCost As Double = 0
        <DataMember()> _
        Public Property CarrierTruckDropCost() As Double
            Get
                Return _CarrierTruckDropCost
            End Get
            Set(ByVal value As Double)
                _CarrierTruckDropCost = value
            End Set
        End Property

        Private _CarrierTruckUnloadDiff As Double = 0
        <DataMember()> _
        Public Property CarrierTruckUnloadDiff() As Double
            Get
                Return _CarrierTruckUnloadDiff
            End Get
            Set(ByVal value As Double)
                _CarrierTruckUnloadDiff = value
            End Set
        End Property

        Private _CarrierTruckCasesAvailable As Double = 0
        <DataMember()> _
        Public Property CarrierTruckCasesAvailable() As Double
            Get
                Return _CarrierTruckCasesAvailable
            End Get
            Set(ByVal value As Double)
                _CarrierTruckCasesAvailable = value
            End Set
        End Property

        Private _CarrierTruckCasesOpen As Double = 0
        <DataMember()> _
        Public Property CarrierTruckCasesOpen() As Double
            Get
                Return _CarrierTruckCasesOpen
            End Get
            Set(ByVal value As Double)
                _CarrierTruckCasesOpen = value
            End Set
        End Property

        Private _CarrierTruckCasesCommitted As Double = 0
        <DataMember()> _
        Public Property CarrierTruckCasesCommitted() As Double
            Get
                Return _CarrierTruckCasesCommitted
            End Get
            Set(ByVal value As Double)
                _CarrierTruckCasesCommitted = value
            End Set
        End Property

        Private _CarrierTruckWgtAvailable As Double = 0
        <DataMember()> _
        Public Property CarrierTruckWgtAvailable() As Double
            Get
                Return _CarrierTruckWgtAvailable
            End Get
            Set(ByVal value As Double)
                _CarrierTruckWgtAvailable = value
            End Set
        End Property

        Private _CarrierTruckWgtOpen As Double = 0
        <DataMember()> _
        Public Property CarrierTruckWgtOpen() As Double
            Get
                Return _CarrierTruckWgtOpen
            End Get
            Set(ByVal value As Double)
                _CarrierTruckWgtOpen = value
            End Set
        End Property

        Private _CarrierTruckWgtCommitted As Double = 0
        <DataMember()> _
        Public Property CarrierTruckWgtCommitted() As Double
            Get
                Return _CarrierTruckWgtCommitted
            End Get
            Set(ByVal value As Double)
                _CarrierTruckWgtCommitted = value
            End Set
        End Property

        Private _CarrierTruckCubesAvailable As Double = 0
        <DataMember()> _
        Public Property CarrierTruckCubesAvailable() As Double
            Get
                Return _CarrierTruckCubesAvailable
            End Get
            Set(ByVal value As Double)
                _CarrierTruckCubesAvailable = value
            End Set
        End Property

        Private _CarrierTruckCubesOpen As Double = 0
        <DataMember()> _
        Public Property CarrierTruckCubesOpen() As Double
            Get
                Return _CarrierTruckCubesOpen
            End Get
            Set(ByVal value As Double)
                _CarrierTruckCubesOpen = value
            End Set
        End Property

        Private _CarrierTruckCubesCommitted As Double = 0
        <DataMember()> _
        Public Property CarrierTruckCubesCommitted() As Double
            Get
                Return _CarrierTruckCubesCommitted
            End Get
            Set(ByVal value As Double)
                _CarrierTruckCubesCommitted = value
            End Set
        End Property


        Private _CarrierTruckPltsAvailable As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckPltsAvailable() As Integer
            Get
                Return _CarrierTruckPltsAvailable
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckPltsAvailable = value
            End Set
        End Property

        Private _CarrierTruckPltsOpen As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckPltsOpen() As Integer
            Get
                Return _CarrierTruckPltsOpen
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckPltsOpen = value
            End Set
        End Property

        Private _CarrierTruckPltsCommitted As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckPltsCommitted() As Integer
            Get
                Return _CarrierTruckPltsCommitted
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckPltsCommitted = value
            End Set
        End Property

        Private _CarrierTruckTrucksAvailable As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckTrucksAvailable() As Integer
            Get
                Return _CarrierTruckTrucksAvailable
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckTrucksAvailable = value
            End Set
        End Property

        Private _CarrierTruckMaxLoadsByWeek As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckMaxLoadsByWeek() As Integer
            Get
                Return _CarrierTruckMaxLoadsByWeek
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckMaxLoadsByWeek = value
            End Set
        End Property

        Private _CarrierTruckMaxLoadsByMonth As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckMaxLoadsByMonth() As Integer
            Get
                Return _CarrierTruckMaxLoadsByMonth
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckMaxLoadsByMonth = value
            End Set
        End Property

        Private _CarrierTruckTotalLoadsForWeek As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckTotalLoadsForWeek() As Integer
            Get
                Return _CarrierTruckTotalLoadsForWeek
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckTotalLoadsForWeek = value
            End Set
        End Property

        Private _CarrierTruckTotalLoadsForMonth As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckTotalLoadsForMonth() As Integer
            Get
                Return _CarrierTruckTotalLoadsForMonth
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckTotalLoadsForMonth = value
            End Set
        End Property

        Private _CarrierTruckWeekDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierTruckWeekDate() As System.Nullable(Of Date)
            Get
                Return _CarrierTruckWeekDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierTruckWeekDate = value
            End Set
        End Property

        Private _CarrierTruckMonthDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierTruckMonthDate() As System.Nullable(Of Date)
            Get
                Return _CarrierTruckMonthDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierTruckMonthDate = value
            End Set
        End Property

        Private _CarrierTruckTempType As String
        <DataMember()> _
        Public Property CarrierTruckTempType As String
            Get
                Return Left(_CarrierTruckTempType, 50)
            End Get
            Set(value As String)
                _CarrierTruckTempType = Left(value, 50)
            End Set
        End Property

        Private _CarrierTruckHazmat As Boolean = True
        <DataMember()> _
        Public Property CarrierTruckHazmat As Boolean
            Get
                Return _CarrierTruckHazmat
            End Get
            Set(value As Boolean)
                _CarrierTruckHazmat = value
            End Set
        End Property

        Private _CarrierTruckUpdated As Byte()
        <DataMember()> _
        Public Property CarrierTruckUpdated() As Byte()
            Get
                Return _CarrierTruckUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierTruckUpdated = value
            End Set
        End Property

        Private _CarrierTruckCodes As String = ""
        <DataMember()>
        Public Property CarrierTruckCodes As String
            Get
                Return _CarrierTruckCodes
            End Get
            Set(ByVal value As String)
                _CarrierTruckCodes = value
            End Set

        End Property

        Private _LocalCarrierTruckCodes As String
        Friend Property LocalCarrierTruckCodes() As String
            Get
                Return _CarrierTruckCodes
            End Get
            Set(ByVal value As String)
                _CarrierTruckCodes = value
            End Set
        End Property






#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierTruck
            instance = DirectCast(MemberwiseClone(), CarrierTruck)
            Return instance
        End Function

#End Region

    End Class
End Namespace