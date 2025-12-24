Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblStaticRoute
        Inherits DTOBaseClass


#Region " Data Members"
        Private _StaticRouteControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteControl() As Integer
            Get
                Return _StaticRouteControl
            End Get
            Set(ByVal value As Integer)
                _StaticRouteControl = value
            End Set
        End Property

        Private _StaticRouteNumber As String = ""
        <DataMember()> _
        Public Property StaticRouteNumber() As String
            Get
                Return Left(_StaticRouteNumber, 50)
            End Get
            Set(ByVal value As String)
                _StaticRouteNumber = Left(value, 50)
            End Set
        End Property


        Private _StaticRouteName As String = ""
        <DataMember()> _
        Public Property StaticRouteName() As String
            Get
                Return Left(_StaticRouteName, 50)
            End Get
            Set(ByVal value As String)
                _StaticRouteName = Left(value, 50)
            End Set
        End Property

        Private _StaticRouteDescription As String = ""
        <DataMember()> _
        Public Property StaticRouteDescription() As String
            Get
                Return Left(_StaticRouteDescription, 255)
            End Get
            Set(ByVal value As String)
                _StaticRouteDescription = Left(value, 255)
            End Set
        End Property

        Private _StaticRouteCompControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCompControl() As Integer
            Get
                Return _StaticRouteCompControl
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCompControl = value
            End Set
        End Property

        Private _StaticRouteNatNumber As Integer = 0
        <DataMember()> _
        Public Property StaticRouteNatNumber() As Integer
            Get
                Return _StaticRouteNatNumber
            End Get
            Set(ByVal value As Integer)
                _StaticRouteNatNumber = value
            End Set
        End Property

        Private _StaticRouteNatName As String = ""
        <DataMember()> _
        Public Property StaticRouteNatName() As String
            Get
                Return Left(_StaticRouteNatName, 40)
            End Get
            Set(ByVal value As String)
                _StaticRouteNatName = Left(value, 40)
            End Set
        End Property

        Private _StaticRouteCompName As String = ""
        <DataMember()> _
        Public Property StaticRouteCompName() As String
            Get
                Return Left(_StaticRouteCompName, 40)
            End Get
            Set(ByVal value As String)
                _StaticRouteCompName = Left(value, 40)
            End Set
        End Property

        Private _StaticRouteCompNumber As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCompNumber() As Integer
            Get
                Return _StaticRouteCompNumber
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCompNumber = value
            End Set
        End Property


        Private _StaticRouteAutoTenderFlag As Boolean = False
        <DataMember()> _
        Public Property StaticRouteAutoTenderFlag() As Boolean
            Get
                Return _StaticRouteAutoTenderFlag
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteAutoTenderFlag = value
            End Set
        End Property

        Private _StaticRouteUseShipDateFlag As Boolean = False
        <DataMember()> _
        Public Property StaticRouteUseShipDateFlag() As Boolean
            Get
                Return _StaticRouteUseShipDateFlag
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteUseShipDateFlag = value
            End Set
        End Property

        Private _StaticRouteGuideDateSelectionDaysBefore As Integer = 0
        <DataMember()> _
        Public Property StaticRouteGuideDateSelectionDaysBefore() As Integer
            Get
                Return _StaticRouteGuideDateSelectionDaysBefore
            End Get
            Set(ByVal value As Integer)
                _StaticRouteGuideDateSelectionDaysBefore = value
            End Set
        End Property

        Private _StaticRouteGuideDateSelectionDaysAfter As Integer = 0
        <DataMember()> _
        Public Property StaticRouteGuideDateSelectionDaysAfter() As Integer
            Get
                Return _StaticRouteGuideDateSelectionDaysAfter
            End Get
            Set(ByVal value As Integer)
                _StaticRouteGuideDateSelectionDaysAfter = value
            End Set
        End Property

        Private _StaticRouteSplitOversizedLoads As Boolean = False
        <DataMember()> _
        Public Property StaticRouteSplitOversizedLoads() As Boolean
            Get
                Return _StaticRouteSplitOversizedLoads
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteSplitOversizedLoads = value
            End Set
        End Property

        Private _StaticRouteCapacityPreference As Integer = 0
        <DataMember()> _
        Public Property StaticRouteCapacityPreference() As Integer
            Get
                Return _StaticRouteCapacityPreference
            End Get
            Set(ByVal value As Integer)
                _StaticRouteCapacityPreference = value
            End Set
        End Property

        Private _StaticRouteRequireAutoTenderApproval As Boolean = False
        <DataMember()> _
        Public Property StaticRouteRequireAutoTenderApproval() As Boolean
            Get
                Return _StaticRouteRequireAutoTenderApproval
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteRequireAutoTenderApproval = value
            End Set
        End Property

        Private _StaticRouteFillLargestFirst As Boolean = True
        <DataMember()> _
        Public Property StaticRouteFillLargestFirst() As Boolean
            Get
                Return _StaticRouteFillLargestFirst
            End Get
            Set(ByVal value As Boolean)
                _StaticRouteFillLargestFirst = value
            End Set
        End Property

        Private _StaticRoutePlaceOnHold As Boolean = False
        <DataMember()> _
        Public Property StaticRoutePlaceOnHold() As Boolean
            Get
                Return _StaticRoutePlaceOnHold
            End Get
            Set(ByVal value As Boolean)
                _StaticRoutePlaceOnHold = value
            End Set
        End Property


        Private _StaticRouteModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property StaticRouteModDate() As System.Nullable(Of Date)
            Get
                Return _StaticRouteModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _StaticRouteModDate = value
            End Set
        End Property

        Private _StaticRouteModUser As String = ""
        <DataMember()> _
        Public Property StaticRouteModUser() As String
            Get
                Return Left(_StaticRouteModUser, 100)
            End Get
            Set(ByVal value As String)
                _StaticRouteModUser = Left(value, 100)
            End Set
        End Property

        Private _StaticRouteURI As String = ""
        <DataMember()> _
        Public Property StaticRouteURI() As String
            Get
                Return Left(_StaticRouteURI, 500)
            End Get
            Set(ByVal value As String)
                _StaticRouteURI = Left(value, 500)
            End Set
        End Property

        Private _StaticRouteUpdated As Byte()
        <DataMember()> _
        Public Property StaticRouteUpdated() As Byte()
            Get
                Return _StaticRouteUpdated
            End Get
            Set(ByVal value As Byte())
                _StaticRouteUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblStaticRoute
            instance = DirectCast(MemberwiseClone(), tblStaticRoute)
            Return instance
        End Function

#End Region

    End Class
End Namespace