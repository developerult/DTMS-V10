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
    Public Class CarriersForRoute
        Inherits DTOBaseClass

        Public Sub New()
            MyBase.New()
            Logger = Logger.ForContext(of CarriersForRoute)
        End Sub

#Region " Data Members"


        Private _StaticRouteControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteControl() As Integer
            Get
                Return Me._StaticRouteControl
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteControl = value) _
                   = False) Then
                    Me._StaticRouteControl = value
                End If
            End Set
        End Property

        Private _StaticRouteNumber As String = ""
        <DataMember()> _
        Public Property StaticRouteNumber() As String
            Get
                Return Me._StaticRouteNumber
            End Get
            Set(value As String)
                If (String.Equals(Me._StaticRouteNumber, value) = False) Then
                    Me._StaticRouteNumber = value
                End If
            End Set
        End Property

        Private _StaticRouteCompControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCompControl() As Integer
            Get
                Return Me._StaticRouteCompControl
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCompControl = value) _
                   = False) Then
                    Me._StaticRouteCompControl = value
                End If
            End Set
        End Property

        Private _StaticRouteAutoTenderFlag As Boolean = False
        <DataMember()> _
        Public Property StaticRouteAutoTenderFlag() As Boolean
            Get
                Return Me._StaticRouteAutoTenderFlag
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteAutoTenderFlag = value) _
                   = False) Then
                    Me._StaticRouteAutoTenderFlag = value
                End If
            End Set
        End Property

        Private _StaticRouteUseShipDateFlag As Boolean = False
        <DataMember()> _
        Public Property StaticRouteUseShipDateFlag() As Boolean
            Get
                Return Me._StaticRouteUseShipDateFlag
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteUseShipDateFlag = value) _
                   = False) Then
                    Me._StaticRouteUseShipDateFlag = value
                End If
            End Set
        End Property

        Private _StaticRouteGuideDateSelectionDaysBefore As Integer = 0
        <DataMember()> _
        Public Property StaticRouteGuideDateSelectionDaysBefore() As Integer
            Get
                Return Me._StaticRouteGuideDateSelectionDaysBefore
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteGuideDateSelectionDaysBefore = value) _
                   = False) Then
                    Me._StaticRouteGuideDateSelectionDaysBefore = value
                End If
            End Set
        End Property

        Private _StaticRouteGuideDateSelectionDaysAfter As Integer = 0
        <DataMember()> _
        Public Property StaticRouteGuideDateSelectionDaysAfter() As Integer
            Get
                Return Me._StaticRouteGuideDateSelectionDaysAfter
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteGuideDateSelectionDaysAfter = value) _
                   = False) Then
                    Me._StaticRouteGuideDateSelectionDaysAfter = value
                End If
            End Set
        End Property

        Private _StaticRouteSplitOversizedLoads As Boolean = False
        <DataMember()> _
        Public Property StaticRouteSplitOversizedLoads() As Boolean
            Get
                Return Me._StaticRouteSplitOversizedLoads
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteSplitOversizedLoads = value) _
                   = False) Then
                    Me._StaticRouteSplitOversizedLoads = value
                End If
            End Set
        End Property

        Private _StaticRouteCapacityPreference As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCapacityPreference() As Integer
            Get
                Return Me._StaticRouteCapacityPreference
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCapacityPreference = value) _
                   = False) Then
                    Me._StaticRouteCapacityPreference = value
                End If
            End Set
        End Property

        Private _StaticRouteRequireAutoTenderApproval As Boolean = False
        <DataMember()> _
        Public Property StaticRouteRequireAutoTenderApproval() As Boolean
            Get
                Return Me._StaticRouteRequireAutoTenderApproval
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteRequireAutoTenderApproval = value) _
                   = False) Then
                    Me._StaticRouteRequireAutoTenderApproval = value
                End If
            End Set
        End Property

        Private _StaticRouteFillLargestFirst As Boolean = False
        <DataMember()> _
        Public Property StaticRouteFillLargestFirst() As Boolean
            Get
                Return Me._StaticRouteFillLargestFirst
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteFillLargestFirst = value) _
                   = False) Then
                    Me._StaticRouteFillLargestFirst = value
                End If
            End Set
        End Property

        Private _StaticRoutePlaceOnHold As Boolean = False
        <DataMember()> _
        Public Property StaticRoutePlaceOnHold() As Boolean
            Get
                Return Me._StaticRoutePlaceOnHold
            End Get
            Set(value As Boolean)
                If ((Me._StaticRoutePlaceOnHold = value) _
                   = False) Then
                    Me._StaticRoutePlaceOnHold = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrCarrierControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrCarrierControl() As Integer
            Get
                Return Me._StaticRouteCarrCarrierControl
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCarrCarrierControl = value) _
                   = False) Then
                    Me._StaticRouteCarrCarrierControl = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrCarrierName As String = ""
        <DataMember()> _
        Public Property StaticRouteCarrCarrierName() As String
            Get
                Return Me._StaticRouteCarrCarrierName
            End Get
            Set(value As String)
                If (String.Equals(Me._StaticRouteCarrCarrierName, value) = False) Then
                    Me._StaticRouteCarrCarrierName = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrName As String = ""
        <DataMember()> _
        Public Property StaticRouteCarrName() As String
            Get
                Return Me._StaticRouteCarrName
            End Get
            Set(value As String)
                If (String.Equals(Me._StaticRouteCarrName, value) = False) Then
                    Me._StaticRouteCarrName = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrRouteTypeCode As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrRouteTypeCode() As Integer
            Get
                Return Me._StaticRouteCarrRouteTypeCode
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCarrRouteTypeCode = value) _
                   = False) Then
                    Me._StaticRouteCarrRouteTypeCode = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrAutoTenderFlag As Boolean = False
        <DataMember()> _
        Public Property StaticRouteCarrAutoTenderFlag() As Boolean
            Get
                Return Me._StaticRouteCarrAutoTenderFlag
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteCarrAutoTenderFlag = value) _
                   = False) Then
                    Me._StaticRouteCarrAutoTenderFlag = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrTendLeadTime As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrTendLeadTime() As Integer
            Get
                Return Me._StaticRouteCarrTendLeadTime
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCarrTendLeadTime = value) _
                   = False) Then
                    Me._StaticRouteCarrTendLeadTime = value
                End If
            End Set
        End Property


        Private _StaticRouteCarrMaxStops As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrMaxStops() As Integer
            Get
                Return Me._StaticRouteCarrMaxStops
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCarrMaxStops = value) _
                   = False) Then
                    Me._StaticRouteCarrMaxStops = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrHazmatFlag As Boolean = False
        <DataMember()> _
        Public Property StaticRouteCarrHazmatFlag() As Boolean
            Get
                Return Me._StaticRouteCarrHazmatFlag
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteCarrHazmatFlag = value) _
                   = False) Then
                    Me._StaticRouteCarrHazmatFlag = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrTransType As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrTransType() As Integer
            Get
                Return Me._StaticRouteCarrTransType
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCarrTransType = value) _
                   = False) Then
                    Me._StaticRouteCarrTransType = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrRouteSequence As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrRouteSequence() As Integer
            Get
                Return Me._StaticRouteCarrRouteSequence
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCarrRouteSequence = value) _
                   = False) Then
                    Me._StaticRouteCarrRouteSequence = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrRequireAutoTenderApproval As Boolean = False
        <DataMember()> _
        Public Property StaticRouteCarrRequireAutoTenderApproval() As Boolean
            Get
                Return Me._StaticRouteCarrRequireAutoTenderApproval
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteCarrRequireAutoTenderApproval = value) _
                   = False) Then
                    Me._StaticRouteCarrRequireAutoTenderApproval = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrAutoAcceptLoads As Boolean = False
        <DataMember()> _
        Public Property StaticRouteCarrAutoAcceptLoads() As Boolean
            Get
                Return Me._StaticRouteCarrAutoAcceptLoads
            End Get
            Set(value As Boolean)
                If ((Me._StaticRouteCarrAutoAcceptLoads = value) _
                   = False) Then
                    Me._StaticRouteCarrAutoAcceptLoads = value
                End If
            End Set
        End Property

        Private _StaticRouteStateFilter As String = ""
        <DataMember()> _
        Public Property StaticRouteStateFilter() As String
            Get
                Return Me._StaticRouteStateFilter
            End Get
            Set(value As String)
                If (String.Equals(Me._StaticRouteStateFilter, value) = False) Then
                    Me._StaticRouteStateFilter = value
                End If
            End Set
        End Property

        Private _StaticRouteCarrControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCarrControl() As Integer
            Get
                Return Me._StaticRouteCarrControl
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteCarrControl = value) _
                   = False) Then
                    Me._StaticRouteCarrControl = value
                End If
            End Set
        End Property

        Private _StaticRouteEquipControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteEquipControl() As Integer
            Get
                Return Me._StaticRouteEquipControl
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteEquipControl = value) _
                   = False) Then
                    Me._StaticRouteEquipControl = value
                End If
            End Set
        End Property

        Private _StaticRouteEquipCarrierTruckControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteEquipCarrierTruckControl() As Integer
            Get
                Return Me._StaticRouteEquipCarrierTruckControl
            End Get
            Set(value As Integer)
                If ((Me._StaticRouteEquipCarrierTruckControl = value) _
                   = False) Then
                    Me._StaticRouteEquipCarrierTruckControl = value
                End If
            End Set
        End Property

        Private _StaticRouteEquipName As String = ""
        <DataMember()> _
        Public Property StaticRouteEquipName() As String
            Get
                Return Me._StaticRouteEquipName
            End Get
            Set(value As String)
                If (String.Equals(Me._StaticRouteEquipName, value) = False) Then
                    Me._StaticRouteEquipName = value
                End If
            End Set
        End Property

        Private _CarrierSCAC As String = ""
        <DataMember()> _
        Public Property CarrierSCAC() As String
            Get
                Return Me._CarrierSCAC
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierSCAC, value) = False) Then
                    Me._CarrierSCAC = value
                End If
            End Set
        End Property

        Private _CarrierActive As System.Nullable(Of Boolean)
        <DataMember()> _
        Public Property CarrierActive() As System.Nullable(Of Boolean)
            Get
                Return Me._CarrierActive
            End Get
            Set(value As System.Nullable(Of Boolean))
                If (Me._CarrierActive.Equals(value) = False) Then
                    Me._CarrierActive = value
                End If
            End Set
        End Property

        Private _CarrierIgnoreTariff As Boolean = False
        <DataMember()> _
        Public Property CarrierIgnoreTariff() As Boolean
            Get
                Return Me._CarrierIgnoreTariff
            End Get
            Set(value As Boolean)
                If ((Me._CarrierIgnoreTariff = value) _
                   = False) Then
                    Me._CarrierIgnoreTariff = value
                End If
            End Set
        End Property

        Private _CarrierAutoFinalize As Boolean = False
        <DataMember()> _
        Public Property CarrierAutoFinalize() As Boolean
            Get
                Return Me._CarrierAutoFinalize
            End Get
            Set(value As Boolean)
                If ((Me._CarrierAutoFinalize = value) _
                   = False) Then
                    Me._CarrierAutoFinalize = value
                End If
            End Set
        End Property

        Private _CarrierTruckEquipment As String = ""
        <DataMember()> _
        Public Property CarrierTruckEquipment() As String
            Get
                Return Me._CarrierTruckEquipment
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierTruckEquipment, value) = False) Then
                    Me._CarrierTruckEquipment = value
                End If
            End Set
        End Property

        Private _CarrierTruckFAK As String = ""
        <DataMember()> _
        Public Property CarrierTruckFAK() As String
            Get
                Return Me._CarrierTruckFAK
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierTruckFAK, value) = False) Then
                    Me._CarrierTruckFAK = value
                End If
            End Set
        End Property

        Private _CarrierTruckPUMon As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUMon() As Boolean
            Get
                Return Me._CarrierTruckPUMon
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckPUMon = value) _
                   = False) Then
                    Me._CarrierTruckPUMon = value
                End If
            End Set
        End Property

        Private _CarrierTruckPUTue As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUTue() As Boolean
            Get
                Return Me._CarrierTruckPUTue
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckPUTue = value) _
                   = False) Then
                    Me._CarrierTruckPUTue = value
                End If
            End Set
        End Property

        Private _CarrierTruckPUWed As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUWed() As Boolean
            Get
                Return Me._CarrierTruckPUWed
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckPUWed = value) _
                   = False) Then
                    Me._CarrierTruckPUWed = value
                End If
            End Set
        End Property


        Private _CarrierTruckPUThu As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUThu() As Boolean
            Get
                Return Me._CarrierTruckPUThu
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckPUThu = value) _
                   = False) Then
                    Me._CarrierTruckPUThu = value
                End If
            End Set
        End Property

        Private _CarrierTruckPUFri As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUFri() As Boolean
            Get
                Return Me._CarrierTruckPUFri
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckPUFri = value) _
                   = False) Then
                    Me._CarrierTruckPUFri = value
                End If
            End Set
        End Property

        Private _CarrierTruckPUSat As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUSat() As Boolean
            Get
                Return Me._CarrierTruckPUSat
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckPUSat = value) _
                   = False) Then
                    Me._CarrierTruckPUSat = value
                End If
            End Set
        End Property

        Private _CarrierTruckPUSun As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckPUSun() As Boolean
            Get
                Return Me._CarrierTruckPUSun
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckPUSun = value) _
                   = False) Then
                    Me._CarrierTruckPUSun = value
                End If
            End Set
        End Property

        Private _CarrierTruckDLMon As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLMon() As Boolean
            Get
                Return Me._CarrierTruckDLMon
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckDLMon = value) _
                   = False) Then
                    Me._CarrierTruckDLMon = value
                End If
            End Set
        End Property

        Private _CarrierTruckDLTue As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLTue() As Boolean
            Get
                Return Me._CarrierTruckDLTue
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckDLTue = value) _
                   = False) Then
                    Me._CarrierTruckDLTue = value
                End If
            End Set
        End Property

        Private _CarrierTruckDLWed As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLWed() As Boolean
            Get
                Return Me._CarrierTruckDLWed
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckDLWed = value) _
                   = False) Then
                    Me._CarrierTruckDLWed = value
                End If
            End Set
        End Property

        Private _CarrierTruckDLThu As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLThu() As Boolean
            Get
                Return Me._CarrierTruckDLThu
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckDLThu = value) _
                   = False) Then
                    Me._CarrierTruckDLThu = value
                End If
            End Set
        End Property

        Private _CarrierTruckDLFri As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLFri() As Boolean
            Get
                Return Me._CarrierTruckDLFri
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckDLFri = value) _
                   = False) Then
                    Me._CarrierTruckDLFri = value
                End If
            End Set
        End Property


        Private _CarrierTruckDLSat As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLSat() As Boolean
            Get
                Return Me._CarrierTruckDLSat
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckDLSat = value) _
                   = False) Then
                    Me._CarrierTruckDLSat = value
                End If
            End Set
        End Property

        Private _CarrierTruckDLSun As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckDLSun() As Boolean
            Get
                Return Me._CarrierTruckDLSun
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckDLSun = value) _
                   = False) Then
                    Me._CarrierTruckDLSun = value
                End If
            End Set
        End Property

        Private _CarrierTruckMaxCases As Double = 0
        <DataMember()> _
        Public Property CarrierTruckMaxCases() As Double
            Get
                Return Me._CarrierTruckMaxCases
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckMaxCases = value) _
                   = False) Then
                    Me._CarrierTruckMaxCases = value
                End If
            End Set
        End Property

        Private _CarrierTruckMinCases As Double = 0
        <DataMember()> _
        Public Property CarrierTruckMinCases() As Double
            Get
                Return Me._CarrierTruckMinCases
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckMinCases = value) _
                   = False) Then
                    Me._CarrierTruckMinCases = value
                End If
            End Set
        End Property

        Private _CarrierTruckSplitCases As Double = 0
        <DataMember()> _
        Public Property CarrierTruckSplitCases() As Double
            Get
                Return Me._CarrierTruckSplitCases
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckSplitCases = value) _
                   = False) Then
                    Me._CarrierTruckSplitCases = value
                End If
            End Set
        End Property

        Private _CarrierTruckMaxWgt As Double = 0
        <DataMember()> _
        Public Property CarrierTruckMaxWgt() As Double
            Get
                Return Me._CarrierTruckMaxWgt
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckMaxWgt = value) _
                   = False) Then
                    Me._CarrierTruckMaxWgt = value
                End If
            End Set
        End Property

        Private _CarrierTruckMinWgt As Double = 0
        <DataMember()> _
        Public Property CarrierTruckMinWgt() As Double
            Get
                Return Me._CarrierTruckMinWgt
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckMinWgt = value) _
                   = False) Then
                    Me._CarrierTruckMinWgt = value
                End If
            End Set
        End Property

        Private _CarrierTruckSplitWgt As Double = 0
        <DataMember()> _
        Public Property CarrierTruckSplitWgt() As Double
            Get
                Return Me._CarrierTruckSplitWgt
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckSplitWgt = value) _
                   = False) Then
                    Me._CarrierTruckSplitWgt = value
                End If
            End Set
        End Property

        Private _CarrierTruckMaxCubes As Double = 0
        <DataMember()> _
        Public Property CarrierTruckMaxCubes() As Double
            Get
                Return Me._CarrierTruckMaxCubes
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckMaxCubes = value) _
                   = False) Then
                    Me._CarrierTruckMaxCubes = value
                End If
            End Set
        End Property

        Private _CarrierTruckMinCubes As Double = 0
        <DataMember()> _
        Public Property CarrierTruckMinCubes() As Double
            Get
                Return Me._CarrierTruckMinCubes
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckMinCubes = value) _
                   = False) Then
                    Me._CarrierTruckMinCubes = value
                End If
            End Set
        End Property

        Private _CarrierTruckSplitCubes As Double = 0
        <DataMember()> _
        Public Property CarrierTruckSplitCubes() As Double
            Get
                Return Me._CarrierTruckSplitCubes
            End Get
            Set(value As Double)
                If ((Me._CarrierTruckSplitCubes = value) _
                   = False) Then
                    Me._CarrierTruckSplitCubes = value
                End If
            End Set
        End Property

        Private _CarrierTruckMaxPlts As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckMaxPlts() As Integer
            Get
                Return Me._CarrierTruckMaxPlts
            End Get
            Set(value As Integer)
                If ((Me._CarrierTruckMaxPlts = value) _
                   = False) Then
                    Me._CarrierTruckMaxPlts = value
                End If
            End Set
        End Property

        Private _CarrierTruckMinPlts As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckMinPlts() As Integer
            Get
                Return Me._CarrierTruckMinPlts
            End Get
            Set(value As Integer)
                If ((Me._CarrierTruckMinPlts = value) _
                   = False) Then
                    Me._CarrierTruckMinPlts = value
                End If
            End Set
        End Property

        Private _CarrierTruckSplitPlts As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckSplitPlts() As Integer
            Get
                Return Me._CarrierTruckSplitPlts
            End Get
            Set(value As Integer)
                If ((Me._CarrierTruckSplitPlts = value) _
                   = False) Then
                    Me._CarrierTruckSplitPlts = value
                End If
            End Set
        End Property

        Private _CarrierTruckTrucksAvailable As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckTrucksAvailable() As Integer
            Get
                Return Me._CarrierTruckTrucksAvailable
            End Get
            Set(value As Integer)
                If ((Me._CarrierTruckTrucksAvailable = value) _
                   = False) Then
                    Me._CarrierTruckTrucksAvailable = value
                End If
            End Set
        End Property

        Private _CarrierTruckMaxLoadsByWeek As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckMaxLoadsByWeek() As Integer
            Get
                Return Me._CarrierTruckMaxLoadsByWeek
            End Get
            Set(value As Integer)
                If ((Me._CarrierTruckMaxLoadsByWeek = value) _
                   = False) Then
                    Me._CarrierTruckMaxLoadsByWeek = value
                End If
            End Set
        End Property

        Private _CarrierTruckMaxLoadsByMonth As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckMaxLoadsByMonth() As Integer
            Get
                Return Me._CarrierTruckMaxLoadsByMonth
            End Get
            Set(value As Integer)
                If ((Me._CarrierTruckMaxLoadsByMonth = value) _
                   = False) Then
                    Me._CarrierTruckMaxLoadsByMonth = value
                End If
            End Set
        End Property

        Private _CarrierTruckTotalLoadsForWeek As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckTotalLoadsForWeek() As Integer
            Get
                Return Me._CarrierTruckTotalLoadsForWeek
            End Get
            Set(value As Integer)
                If ((Me._CarrierTruckTotalLoadsForWeek = value) _
                   = False) Then
                    Me._CarrierTruckTotalLoadsForWeek = value
                End If
            End Set
        End Property

        Private _CarrierTruckTotalLoadsForMonth As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckTotalLoadsForMonth() As Integer
            Get
                Return Me._CarrierTruckTotalLoadsForMonth
            End Get
            Set(value As Integer)
                If ((Me._CarrierTruckTotalLoadsForMonth = value) _
                   = False) Then
                    Me._CarrierTruckTotalLoadsForMonth = value
                End If
            End Set
        End Property

        Private _CarrierTruckTempType As String = ""
        <DataMember()> _
        Public Property CarrierTruckTempType() As String
            Get
                Return Me._CarrierTruckTempType
            End Get
            Set(value As String)
                If (String.Equals(Me._CarrierTruckTempType, value) = False) Then
                    Me._CarrierTruckTempType = value
                End If
            End Set
        End Property

        Private _CarrierTruckHazmat As Boolean = False
        <DataMember()> _
        Public Property CarrierTruckHazmat() As Boolean
            Get
                Return Me._CarrierTruckHazmat
            End Get
            Set(value As Boolean)
                If ((Me._CarrierTruckHazmat = value) _
                   = False) Then
                    Me._CarrierTruckHazmat = value
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarriersForRoute
            instance = DirectCast(MemberwiseClone(), CarriersForRoute)
            Return instance
        End Function

#End Region
    End Class


End Namespace
