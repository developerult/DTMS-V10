Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblStaticRouteEquip
        Inherits DTOBaseClass


#Region " Data Members"
        Private _StaticRouteEquipControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteEquipControl() As Integer
            Get
                Return _StaticRouteEquipControl
            End Get
            Set(ByVal value As Integer)
                _StaticRouteEquipControl = value
            End Set
        End Property

        Private _StaticRouteEquipStaticRouteCarrControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteEquipStaticRouteCarrControl() As Integer
            Get
                Return _StaticRouteEquipStaticRouteCarrControl
            End Get
            Set(ByVal value As Integer)
                _StaticRouteEquipStaticRouteCarrControl = value
            End Set
        End Property

        Private _StaticRouteEquipCarrierTruckControl As Integer = 0
        <DataMember()> _
        Public Property StaticRouteEquipCarrierTruckControl() As Integer
            Get
                Return _StaticRouteEquipCarrierTruckControl
            End Get
            Set(ByVal value As Integer)
                _StaticRouteEquipCarrierTruckControl = value
            End Set
        End Property

        Private _StaticRouteEquipCarrierTruckDescription As String = ""
        <DataMember()> _
        Public Property StaticRouteEquipCarrierTruckDescription() As String
            Get
                Return Left(_StaticRouteEquipCarrierTruckDescription, 255)
            End Get
            Set(ByVal value As String)
                _StaticRouteEquipCarrierTruckDescription = Left(value, 255)
            End Set
        End Property

        Private _StaticRouteEquipName As String = ""
        <DataMember()> _
        Public Property StaticRouteEquipName() As String
            Get
                Return Left(_StaticRouteEquipName, 50)
            End Get
            Set(ByVal value As String)
                _StaticRouteEquipName = Left(value, 50)
            End Set
        End Property

        Private _StaticRouteEquipDescription As String = ""
        <DataMember()> _
        Public Property StaticRouteEquipDescription() As String
            Get
                Return Left(_StaticRouteEquipDescription, 255)
            End Get
            Set(ByVal value As String)
                _StaticRouteEquipDescription = Left(value, 255)
            End Set
        End Property

        Private _StaticRouteEquipTruckDetails As CarrierTruck
        <DataMember()> _
        Public Property StaticRouteEquipTruckDetails As CarrierTruck
            Get
                Return _StaticRouteEquipTruckDetails
            End Get
            Set(value As CarrierTruck)
                _StaticRouteEquipTruckDetails = value
            End Set
        End Property

        Private _StaticRouteEquipModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property StaticRouteEquipModDate() As System.Nullable(Of Date)
            Get
                Return _StaticRouteEquipModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _StaticRouteEquipModDate = value
            End Set
        End Property

        Private _StaticRouteEquipModUser As String = ""
        <DataMember()> _
        Public Property StaticRouteEquipModUser() As String
            Get
                Return Left(_StaticRouteEquipModUser, 100)
            End Get
            Set(ByVal value As String)
                _StaticRouteEquipModUser = Left(value, 100)
            End Set
        End Property

        Private _StaticRouteEquipUpdated As Byte()
        <DataMember()> _
        Public Property StaticRouteEquipUpdated() As Byte()
            Get
                Return _StaticRouteEquipUpdated
            End Get
            Set(ByVal value As Byte())
                _StaticRouteEquipUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblStaticRouteEquip
            instance = DirectCast(MemberwiseClone(), tblStaticRouteEquip)
            Return instance
        End Function

#End Region

    End Class
End Namespace