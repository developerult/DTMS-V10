Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarriersByCostByLane
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Friend Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Friend Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Friend Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property

        Private _CarrierMileRate As Decimal = 0
        <DataMember()> _
        Public Property CarrierMileRate() As Decimal
            Get
                Return _CarrierMileRate
            End Get
            Friend Set(ByVal value As Decimal)
                _CarrierMileRate = value
            End Set
        End Property

        Private _CarrierLbsRate As Decimal = 0
        <DataMember()> _
        Public Property CarrierLbsRate() As Decimal
            Get
                Return _CarrierLbsRate
            End Get
            Friend Set(ByVal value As Decimal)
                _CarrierLbsRate = value
            End Set
        End Property

        Private _CarrierCubeRate As Decimal = 0
        <DataMember()> _
        Public Property CarrierCubeRate() As Decimal
            Get
                Return _CarrierCubeRate
            End Get
            Friend Set(ByVal value As Decimal)
                _CarrierCubeRate = value
            End Set
        End Property

        Private _CarrierCaseRate As Decimal = 0
        <DataMember()> _
        Public Property CarrierCaseRate() As Decimal
            Get
                Return _CarrierCaseRate
            End Get
            Friend Set(ByVal value As Decimal)
                _CarrierCaseRate = value
            End Set
        End Property

        Private _CarrierPltRate As Decimal = 0
        <DataMember()> _
        Public Property CarrierPltRate() As Decimal
            Get
                Return _CarrierPltRate
            End Get
            Friend Set(ByVal value As Decimal)
                _CarrierPltRate = value
            End Set
        End Property

        Private _CarrierTLCost As Decimal = 0
        <DataMember()> _
        Public Property CarrierTLCost() As Decimal
            Get
                Return _CarrierTLCost
            End Get
            Friend Set(ByVal value As Decimal)
                _CarrierTLCost = value
            End Set
        End Property

        Private _CarrierEquipment As String = ""
        <DataMember()> _
        Public Property CarrierEquipment() As String
            Get
                Return _CarrierEquipment
            End Get
            Friend Set(ByVal value As String)
                _CarrierEquipment = value
            End Set
        End Property

        Private _CarrierCost As Decimal = 0
        <DataMember()> _
        Public Property CarrierCost() As Decimal
            Get
                Return _CarrierCost
            End Get
            Friend Set(ByVal value As Decimal)
                _CarrierCost = value
            End Set
        End Property

        Private _CarrierMinCost As Decimal = 0
        <DataMember()> _
        Public Property CarrierMinCost() As Decimal
            Get
                Return _CarrierMinCost
            End Get
            Friend Set(ByVal value As Decimal)
                _CarrierMinCost = value
            End Set
        End Property

        Private _CarrierRateExpires As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierRateExpires() As System.Nullable(Of Date)
            Get
                Return _CarrierRateExpires
            End Get
            Friend Set(ByVal value As System.Nullable(Of Date))
                _CarrierRateExpires = value
            End Set
        End Property

        Private _ErrMsg As String = ""
        <DataMember()> _
        Public Property ErrMsg() As String
            Get
                Return _ErrMsg
            End Get
            Friend Set(ByVal value As String)
                _ErrMsg = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarriersByCostByLane
            instance = DirectCast(MemberwiseClone(), CarriersByCostByLane)
            Return instance
        End Function

#End Region
    End Class


End Namespace