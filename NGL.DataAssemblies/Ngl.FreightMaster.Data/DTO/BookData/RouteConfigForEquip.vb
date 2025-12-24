Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class RouteConfigForEquip
        Inherits DTOBaseClass

#Region " Data Members"

        Private _StaticRouteControl As Integer
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

        Private _StaticRouteCarrControl As Integer
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

        Private _StaticRouteEquipControl As Integer
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

        Private _StaticRouteAutoTenderFlag As Boolean
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

        Private _StaticRouteUseShipDateFlag As Boolean
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

        Private _StaticRouteGuideDateSelectionDaysBefore As Integer
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

        Private _StaticRouteGuideDateSelectionDaysAfter As Integer
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

        Private _StaticRouteSplitOversizedLoads As Boolean
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

        Private _StaticRouteCapacityPreference As Integer
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

        Private _StaticRouteRequireAutoTenderApproval As Boolean
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

        Private _StaticRouteFillLargestFirst As Boolean
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

        Private _StaticRoutePlaceOnHold As Boolean
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

        Private _StaticRouteCarrCarrierControl As Integer
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

        Private _StaticRouteCarrRouteTypeCode As Integer
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

        Private _StaticRouteCarrAutoTenderFlag As Boolean
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

        Private _StaticRouteCarrTendLeadTime As Integer
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

        Private _StaticRouteCarrMaxStops As Integer
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

        Private _StaticRouteCarrHazmatFlag As Boolean
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

        Private _StaticRouteCarrTransType As Integer
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

        Private _StaticRouteCarrRouteSequence As Integer
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

        Private _StaticRouteCarrRequireAutoTenderApproval As Boolean
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

        Private _StaticRouteCarrAutoAcceptLoads As Boolean
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

        Private _StaticRouteStateFilter As String
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


#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New RouteConfigForEquip
            instance = DirectCast(MemberwiseClone(), RouteConfigForEquip)
            Return instance
        End Function

#End Region

    End Class
End Namespace

