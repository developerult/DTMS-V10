Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneCarr
        Inherits DTOBaseClass


#Region " Data Members"
        Private _LaneCarrControl As Integer = 0
        <DataMember()> _
        Public Property LaneCarrControl() As Integer
            Get
                Return _LaneCarrControl
            End Get
            Set(ByVal value As Integer)
                _LaneCarrControl = value
            End Set
        End Property

        Private _LaneCarrLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneCarrLaneControl() As Integer
            Get
                Return _LaneCarrLaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneCarrLaneControl = value
            End Set
        End Property

        Private _LaneCarrCarrierControl As Integer = 0
        <DataMember()> _
        Public Property LaneCarrCarrierControl() As Integer
            Get
                Return _LaneCarrCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LaneCarrCarrierControl = value
            End Set
        End Property

        Private _LaneCarrWgtFrom As Integer = 0
        <DataMember()> _
        Public Property LaneCarrWgtFrom() As Integer
            Get
                Return _LaneCarrWgtFrom
            End Get
            Set(ByVal value As Integer)
                _LaneCarrWgtFrom = value
            End Set
        End Property

        Private _LaneCarrWgtTo As Integer = 0
        <DataMember()> _
        Public Property LaneCarrWgtTo() As Integer
            Get
                Return _LaneCarrWgtTo
            End Get
            Set(ByVal value As Integer)
                _LaneCarrWgtTo = value
            End Set
        End Property

        Private _LaneCarrRateStarts As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneCarrRateStarts() As System.Nullable(Of Date)
            Get
                Return _LaneCarrRateStarts
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneCarrRateStarts = value
            End Set
        End Property

        Private _LaneCarrRateExpires As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneCarrRateExpires() As System.Nullable(Of Date)
            Get
                Return _LaneCarrRateExpires
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneCarrRateExpires = value
            End Set
        End Property

        Private _LaneCarrTL As Boolean = False
        <DataMember()> _
        Public Property LaneCarrTL() As Boolean
            Get
                Return _LaneCarrTL
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrTL = value
            End Set
        End Property


        Private _LaneCarrLTL As Boolean = False
        <DataMember()> _
        Public Property LaneCarrLTL() As Boolean
            Get
                Return _LaneCarrLTL
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrLTL = value
            End Set
        End Property


        Private _LaneCarrEquipment As String = ""
        <DataMember()> _
        Public Property LaneCarrEquipment() As String
            Get
                Return Left(_LaneCarrEquipment, 20)
            End Get
            Set(ByVal value As String)
                _LaneCarrEquipment = Left(value, 20)
            End Set
        End Property

        Private _LaneCarrMileRate As Decimal = 0
        <DataMember()> _
        Public Property LaneCarrMileRate() As Decimal
            Get
                Return _LaneCarrMileRate
            End Get
            Set(ByVal value As Decimal)
                _LaneCarrMileRate = value
            End Set
        End Property

        Private _LaneCarrCwtRate As Decimal = 0
        <DataMember()> _
        Public Property LaneCarrCwtRate() As Decimal
            Get
                Return _LaneCarrCwtRate
            End Get
            Set(ByVal value As Decimal)
                _LaneCarrCwtRate = value
            End Set
        End Property

        Private _LaneCarrCaseRate As Decimal = 0
        <DataMember()> _
        Public Property LaneCarrCaseRate() As Decimal
            Get
                Return _LaneCarrCaseRate
            End Get
            Set(ByVal value As Decimal)
                _LaneCarrCaseRate = value
            End Set
        End Property

        Private _LaneCarrFlatRate As Decimal = 0
        <DataMember()> _
        Public Property LaneCarrFlatRate() As Decimal
            Get
                Return _LaneCarrFlatRate
            End Get
            Set(ByVal value As Decimal)
                _LaneCarrFlatRate = value
            End Set
        End Property

        Private _LaneCarrPltRate As Decimal = 0
        <DataMember()> _
        Public Property LaneCarrPltRate() As Decimal
            Get
                Return _LaneCarrPltRate
            End Get
            Set(ByVal value As Decimal)
                _LaneCarrPltRate = value
            End Set
        End Property

        Private _LaneCarrCubeRate As Decimal = 0
        <DataMember()> _
        Public Property LaneCarrCubeRate() As Decimal
            Get
                Return _LaneCarrCubeRate
            End Get
            Set(ByVal value As Decimal)
                _LaneCarrCubeRate = value
            End Set
        End Property

        Private _LaneCarrTLT As Integer = 0
        <DataMember()> _
        Public Property LaneCarrTLT() As Integer
            Get
                Return _LaneCarrTLT
            End Get
            Set(ByVal value As Integer)
                _LaneCarrTLT = value
            End Set
        End Property

        Private _LaneCarrTMode As String = ""
        <DataMember()> _
        Public Property LaneCarrTMode() As String
            Get
                Return Left(_LaneCarrTMode, 3)
            End Get
            Set(ByVal value As String)
                _LaneCarrTMode = Left(value, 3)
            End Set
        End Property

        Private _LaneCarrFAK As String = ""
        <DataMember()> _
        Public Property LaneCarrFAK() As String
            Get
                Return Left(_LaneCarrFAK, 10)
            End Get
            Set(ByVal value As String)
                _LaneCarrFAK = Left(value, 10)
            End Set
        End Property

        Private _LaneCarrDisc As Single = 0
        <DataMember()> _
        Public Property LaneCarrDisc() As Single
            Get
                Return _LaneCarrDisc
            End Get
            Set(ByVal value As Single)
                _LaneCarrDisc = value
            End Set
        End Property

        Private _LaneCarrPUMon As Boolean = False
        <DataMember()> _
        Public Property LaneCarrPUMon() As Boolean
            Get
                Return _LaneCarrPUMon
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrPUMon = value
            End Set
        End Property


        Private _LaneCarrPUTue As Boolean = False
        <DataMember()> _
        Public Property LaneCarrPUTue() As Boolean
            Get
                Return _LaneCarrPUTue
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrPUTue = value
            End Set
        End Property


        Private _LaneCarrPUWed As Boolean = False
        <DataMember()> _
        Public Property LaneCarrPUWed() As Boolean
            Get
                Return _LaneCarrPUWed
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrPUWed = value
            End Set
        End Property


        Private _LaneCarrPUThu As Boolean = False
        <DataMember()> _
        Public Property LaneCarrPUThu() As Boolean
            Get
                Return _LaneCarrPUThu
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrPUThu = value
            End Set
        End Property


        Private _LaneCarrPUFri As Boolean = False
        <DataMember()> _
        Public Property LaneCarrPUFri() As Boolean
            Get
                Return _LaneCarrPUFri
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrPUFri = value
            End Set
        End Property


        Private _LaneCarrPUSat As Boolean = False
        <DataMember()> _
        Public Property LaneCarrPUSat() As Boolean
            Get
                Return _LaneCarrPUSat
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrPUSat = value
            End Set
        End Property


        Private _LaneCarrPUSun As Boolean = False
        <DataMember()> _
        Public Property LaneCarrPUSun() As Boolean
            Get
                Return _LaneCarrPUSun
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrPUSun = value
            End Set
        End Property


        Private _LaneCarrDLMon As Boolean = False
        <DataMember()> _
        Public Property LaneCarrDLMon() As Boolean
            Get
                Return _LaneCarrDLMon
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrDLMon = value
            End Set
        End Property


        Private _LaneCarrDLTue As Boolean = False
        <DataMember()> _
        Public Property LaneCarrDLTue() As Boolean
            Get
                Return _LaneCarrDLTue
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrDLTue = value
            End Set
        End Property


        Private _LaneCarrDLWed As Boolean = False
        <DataMember()> _
        Public Property LaneCarrDLWed() As Boolean
            Get
                Return _LaneCarrDLWed
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrDLWed = value
            End Set
        End Property


        Private _LaneCarrDLThu As Boolean = False
        <DataMember()> _
        Public Property LaneCarrDLThu() As Boolean
            Get
                Return _LaneCarrDLThu
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrDLThu = value
            End Set
        End Property


        Private _LaneCarrDLFri As Boolean = False
        <DataMember()> _
        Public Property LaneCarrDLFri() As Boolean
            Get
                Return _LaneCarrDLFri
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrDLFri = value
            End Set
        End Property


        Private _LaneCarrDLSat As Boolean = False
        <DataMember()> _
        Public Property LaneCarrDLSat() As Boolean
            Get
                Return _LaneCarrDLSat
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrDLSat = value
            End Set
        End Property


        Private _LaneCarrDLSun As Boolean = False
        <DataMember()> _
        Public Property LaneCarrDLSun() As Boolean
            Get
                Return _LaneCarrDLSun
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrDLSun = value
            End Set
        End Property


        Private _LaneCarrPayTolPerLo As Double = 0
        <DataMember()> _
        Public Property LaneCarrPayTolPerLo() As Double
            Get
                Return _LaneCarrPayTolPerLo
            End Get
            Set(ByVal value As Double)
                _LaneCarrPayTolPerLo = value
            End Set
        End Property

        Private _LaneCarrPayTolPerHi As Double = 0
        <DataMember()> _
        Public Property LaneCarrPayTolPerHi() As Double
            Get
                Return _LaneCarrPayTolPerHi
            End Get
            Set(ByVal value As Double)
                _LaneCarrPayTolPerHi = value
            End Set
        End Property

        Private _LaneCarrPayTolCurLo As Double = 0
        <DataMember()> _
        Public Property LaneCarrPayTolCurLo() As Double
            Get
                Return _LaneCarrPayTolCurLo
            End Get
            Set(ByVal value As Double)
                _LaneCarrPayTolCurLo = value
            End Set
        End Property

        Private _LaneCarrPayTolCurHi As Double = 0
        <DataMember()> _
        Public Property LaneCarrPayTolCurHi() As Double
            Get
                Return _LaneCarrPayTolCurHi
            End Get
            Set(ByVal value As Double)
                _LaneCarrPayTolCurHi = value
            End Set
        End Property

        Private _LaneCarrCurType As Integer = 0
        <DataMember()> _
        Public Property LaneCarrCurType() As Integer
            Get
                Return _LaneCarrCurType
            End Get
            Set(ByVal value As Integer)
                _LaneCarrCurType = value
            End Set
        End Property

        Private _LaneCarrModUser As String = ""
        <DataMember()> _
        Public Property LaneCarrModUser() As String
            Get
                Return Left(_LaneCarrModUser, 100)
            End Get
            Set(ByVal value As String)
                _LaneCarrModUser = Left(value, 100)
            End Set
        End Property

        Private _LaneCarrModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneCarrModDate() As System.Nullable(Of Date)
            Get
                Return _LaneCarrModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneCarrModDate = value
            End Set
        End Property

        Private _LaneCarrRoute As String = ""
        <DataMember()> _
        Public Property LaneCarrRoute() As String
            Get
                Return Left(_LaneCarrRoute, 10)
            End Get
            Set(ByVal value As String)
                _LaneCarrRoute = Left(value, 10)
            End Set
        End Property

        Private _LaneCarrPltsOpen As Integer = 0
        <DataMember()> _
        Public Property LaneCarrPltsOpen() As Integer
            Get
                Return _LaneCarrPltsOpen
            End Get
            Set(ByVal value As Integer)
                _LaneCarrPltsOpen = value
            End Set
        End Property

        Private _LaneCarrPltsCommitted As Integer = 0
        <DataMember()> _
        Public Property LaneCarrPltsCommitted() As Integer
            Get
                Return _LaneCarrPltsCommitted
            End Get
            Set(ByVal value As Integer)
                _LaneCarrPltsCommitted = value
            End Set
        End Property

        Private _LaneCarrPltsAvailable As Integer = 0
        <DataMember()> _
        Public Property LaneCarrPltsAvailable() As Integer
            Get
                Return _LaneCarrPltsAvailable
            End Get
            Set(ByVal value As Integer)
                _LaneCarrPltsAvailable = value
            End Set
        End Property

        Private _LaneCarrMiles As Integer = 0
        <DataMember()> _
        Public Property LaneCarrMiles() As Integer
            Get
                Return _LaneCarrMiles
            End Get
            Set(ByVal value As Integer)
                _LaneCarrMiles = value
            End Set
        End Property

        Private _LaneCarrBkhlCostPerc As Double = 0
        <DataMember()> _
        Public Property LaneCarrBkhlCostPerc() As Double
            Get
                Return _LaneCarrBkhlCostPerc
            End Get
            Set(ByVal value As Double)
                _LaneCarrBkhlCostPerc = value
            End Set
        End Property

        Private _LaneCarrPalletCostPer As Decimal = 0
        <DataMember()> _
        Public Property LaneCarrPalletCostPer() As Decimal
            Get
                Return _LaneCarrPalletCostPer
            End Get
            Set(ByVal value As Decimal)
                _LaneCarrPalletCostPer = value
            End Set
        End Property

        Private _LaneCarrFuelSurChargePerc As Double = 0
        <DataMember()> _
        Public Property LaneCarrFuelSurChargePerc() As Double
            Get
                Return _LaneCarrFuelSurChargePerc
            End Get
            Set(ByVal value As Double)
                _LaneCarrFuelSurChargePerc = value
            End Set
        End Property

        Private _LaneCarrStopCharge As Double = 0
        <DataMember()> _
        Public Property LaneCarrStopCharge() As Double
            Get
                Return _LaneCarrStopCharge
            End Get
            Set(ByVal value As Double)
                _LaneCarrStopCharge = value
            End Set
        End Property

        Private _LaneCarrDropCost As Double = 0
        <DataMember()> _
        Public Property LaneCarrDropCost() As Double
            Get
                Return _LaneCarrDropCost
            End Get
            Set(ByVal value As Double)
                _LaneCarrDropCost = value
            End Set
        End Property

        Private _LaneCarrUnloadDiff As Double = 0
        <DataMember()> _
        Public Property LaneCarrUnloadDiff() As Double
            Get
                Return _LaneCarrUnloadDiff
            End Get
            Set(ByVal value As Double)
                _LaneCarrUnloadDiff = value
            End Set
        End Property

        Private _LaneCarrCasesAvailable As Double = 0
        <DataMember()> _
        Public Property LaneCarrCasesAvailable() As Double
            Get
                Return _LaneCarrCasesAvailable
            End Get
            Set(ByVal value As Double)
                _LaneCarrCasesAvailable = value
            End Set
        End Property

        Private _LaneCarrCasesOpen As Double = 0
        <DataMember()> _
        Public Property LaneCarrCasesOpen() As Double
            Get
                Return _LaneCarrCasesOpen
            End Get
            Set(ByVal value As Double)
                _LaneCarrCasesOpen = value
            End Set
        End Property

        Private _LaneCarrCasesCommitted As Double = 0
        <DataMember()> _
        Public Property LaneCarrCasesCommitted() As Double
            Get
                Return _LaneCarrCasesCommitted
            End Get
            Set(ByVal value As Double)
                _LaneCarrCasesCommitted = value
            End Set
        End Property

        Private _LaneCarrWgtAvailable As Double = 0
        <DataMember()> _
        Public Property LaneCarrWgtAvailable() As Double
            Get
                Return _LaneCarrWgtAvailable
            End Get
            Set(ByVal value As Double)
                _LaneCarrWgtAvailable = value
            End Set
        End Property

        Private _LaneCarrWgtOpen As Double = 0
        <DataMember()> _
        Public Property LaneCarrWgtOpen() As Double
            Get
                Return _LaneCarrWgtOpen
            End Get
            Set(ByVal value As Double)
                _LaneCarrWgtOpen = value
            End Set
        End Property

        Private _LaneCarrWgtCommitted As Double = 0
        <DataMember()> _
        Public Property LaneCarrWgtCommitted() As Double
            Get
                Return _LaneCarrWgtCommitted
            End Get
            Set(ByVal value As Double)
                _LaneCarrWgtCommitted = value
            End Set
        End Property

        Private _LaneCarrCubesAvailable As Double = 0
        <DataMember()> _
        Public Property LaneCarrCubesAvailable() As Double
            Get
                Return _LaneCarrCubesAvailable
            End Get
            Set(ByVal value As Double)
                _LaneCarrCubesAvailable = value
            End Set
        End Property

        Private _LaneCarrCubesOpen As Double = 0
        <DataMember()> _
        Public Property LaneCarrCubesOpen() As Double
            Get
                Return _LaneCarrCubesOpen
            End Get
            Set(ByVal value As Double)
                _LaneCarrCubesOpen = value
            End Set
        End Property

        Private _LaneCarrCubesCommitted As Double = 0
        <DataMember()> _
        Public Property LaneCarrCubesCommitted() As Double
            Get
                Return _LaneCarrCubesCommitted
            End Get
            Set(ByVal value As Double)
                _LaneCarrCubesCommitted = value
            End Set
        End Property

        Private _LaneCarrCarrierTruckControl As Integer = 0
        <DataMember()> _
        Public Property LaneCarrCarrierTruckControl() As Integer
            Get
                Return _LaneCarrCarrierTruckControl
            End Get
            Set(ByVal value As Integer)
                _LaneCarrCarrierTruckControl = value
            End Set
        End Property

        Private _LaneCarrLockSettings As Boolean = False
        <DataMember()> _
        Public Property LaneCarrLockSettings() As Boolean
            Get
                Return _LaneCarrLockSettings
            End Get
            Set(ByVal value As Boolean)
                _LaneCarrLockSettings = value
            End Set
        End Property

        Private _LaneCarrUpdated As Byte()
        <DataMember()> _
        Public Property LaneCarrUpdated() As Byte()
            Get
                Return _LaneCarrUpdated
            End Get
            Set(ByVal value As Byte())
                _LaneCarrUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneCarr
            instance = DirectCast(MemberwiseClone(), LaneCarr)
            Return instance
        End Function

#End Region

    End Class
End Namespace