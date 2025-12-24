Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Destructurama
Imports Destructurama.Attributed
Namespace DataTransferObjects
    <DataContract(), LogAsScalar(true)> _
    Public Class RateShop
        Inherits DTOBaseClass

        Public Sub New()
            MyBase.New()
            Logger = Logger.ForContext(of RateShop)
        End Sub


#Region " Data Members"

        Private _BookRevs As List(Of BookRevenue)
        <DataMember()> _
        Public Property BookRevs() As List(Of BookRevenue)
            Get
                Return _BookRevs
            End Get
            Set(ByVal value As List(Of BookRevenue))
                _BookRevs = value
            End Set
        End Property

        Private _BookFees As List(Of BookFee)
        <DataMember()> _
        Public Property BookFees() As List(Of BookFee)
            Get
                Return _BookFees
            End Get
            Set(ByVal value As List(Of BookFee))
                _BookFees = value
            End Set
        End Property

        Private _carrierControl As Integer
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _carrierControl
            End Get
            Set(ByVal value As Integer)
                _carrierControl = value
            End Set
        End Property

        Private _prefered As Boolean = True
        <DataMember()> _
        Public Property Prefered() As Boolean
            Get
                Return _prefered
            End Get
            Set(ByVal value As Boolean)
                _prefered = value
            End Set
        End Property

        Private _NoLateDelivery As Boolean = False
        <DataMember()> _
        Public Property NoLateDelivery() As Boolean
            Get
                Return _NoLateDelivery
            End Get
            Set(ByVal value As Boolean)
                _NoLateDelivery = value
            End Set
        End Property

        Private _Validated As Boolean = True
        <DataMember()> _
        Public Property Validated() As Boolean
            Get
                Return _Validated
            End Get
            Set(ByVal value As Boolean)
                _Validated = value
            End Set
        End Property

        Private _optimizeByCapacity As Boolean = True
        <DataMember()> _
        Public Property OptimizeByCapacity() As Boolean
            Get
                Return _optimizeByCapacity
            End Get
            Set(ByVal value As Boolean)
                _optimizeByCapacity = value
            End Set
        End Property

        Private _modeTypeControl As Integer = 0
        <DataMember()> _
        Public Property ModeTypeControl() As Integer
            Get
                Return _modeTypeControl
            End Get
            Set(ByVal value As Integer)
                _modeTypeControl = value
            End Set
        End Property

        Private _tempType As Integer = 0
        <DataMember()> _
        Public Property TempType() As Integer
            Get
                Return _tempType
            End Get
            Set(ByVal value As Integer)
                _tempType = value
            End Set
        End Property

        Private _tariffTypeControl As Integer = 0
        <DataMember()> _
        Public Property TariffTypeControl() As Integer
            Get
                Return _tariffTypeControl
            End Get
            Set(ByVal value As Integer)
                _tariffTypeControl = value
            End Set
        End Property

        Private _carrTarEquipMatClass As String
        <DataMember()> _
        Public Property CarrTarEquipMatClass() As String
            Get
                Return _carrTarEquipMatClass
            End Get
            Set(ByVal value As String)
                _carrTarEquipMatClass = value
            End Set
        End Property

        Private _carrTarEquipMatClassTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatClassTypeControl() As Integer
            Get
                Return _carrTarEquipMatClassTypeControl
            End Get
            Set(ByVal value As Integer)
                _carrTarEquipMatClassTypeControl = value
            End Set
        End Property

        Private _carrTarEquipMatTarRateTypeControl As Integer = 0
        <DataMember()> _
        Public Property CarrTarEquipMatTarRateTypeControl() As Integer
            Get
                Return _carrTarEquipMatTarRateTypeControl
            End Get
            Set(ByVal value As Integer)
                _carrTarEquipMatTarRateTypeControl = value
            End Set
        End Property

        Private _agentControl As Integer = 0
        <DataMember()> _
        Public Property AgentControl() As Integer
            Get
                Return _agentControl
            End Get
            Set(ByVal value As Integer)
                _agentControl = value
            End Set
        End Property

        Private _Outbound As Boolean = True
        <DataMember()> _
        Public Property Outbound() As Boolean
            Get
                Return _Outbound
            End Get
            Set(ByVal value As Boolean)
                _Outbound = value
            End Set
        End Property

        Private _UsePCM As Boolean = True
        <DataMember()> _
        Public Property UsePCM() As Boolean
            Get
                Return _UsePCM
            End Get
            Set(ByVal value As Boolean)
                _UsePCM = value
            End Set
        End Property

        Private _UseERE As Boolean = False
        <DataMember()> _
        Public Property UseERE() As Boolean
            Get
                Return _UseERE
            End Get
            Set(ByVal value As Boolean)
                _UseERE = value
            End Set
        End Property

#End Region

#Region " Public Methods"

        Public Overrides Function ToString() As String
            Return $"RateShop: {CarrierControl} BookRevs: {BookRevs?.Count()} BookFees: {BookFees?.Count()} CarrTarEquipMatClass: {Me.CarrTarEquipMatClass}, ModeType: {Me.ModeTypeControl}, Outbound: {Me.Outbound}, Preferred:{Me.Prefered}, TariffType: {Me.TariffTypeControl}"
                
        End Function

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New RateShop
            instance = DirectCast(MemberwiseClone(), RateShop)
            Return instance
        End Function

#End Region

    End Class
End Namespace