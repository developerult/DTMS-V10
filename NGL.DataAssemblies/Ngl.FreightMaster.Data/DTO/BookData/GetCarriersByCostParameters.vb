Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Serilog
Imports Destructurama
Imports Destructurama.Attributed

Namespace DataTransferObjects
    <DataContract(),LogAsScalar(True)>
    Public Class GetCarriersByCostParameters
        Inherits DTOBaseClass

#Region " Constructor"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Sub New()
            MyBase.New()
            Me.Page = 1
            Me.PageSize = 2000
            Me.Logger = Me.Logger.ForContext(Of GetCarriersByCostParameters)
        End Sub

        ''' <summary>
        ''' Use this constructor for auto carrier selection it supports all the default settings
        ''' </summary>
        ''' <param name="imodeTypeControl"></param>
        ''' <param name="itempType"></param>
        Public Sub New(ByVal imodeTypeControl As Integer, ByVal itempType As Integer)
            MyBase.New()
            Me.carrierControl = 0
            Me.prefered = True
            Me.noLateDelivery = False
            Me.validated = True
            Me.optimizeByCapacity = True
            Me.modeTypeControl = imodeTypeControl
            Me.tempType = itempType
            Me.tariffTypeControl = 0
            Me.carrTarEquipMatClass = Nothing
            Me.carrTarEquipMatClassTypeControl = 0
            Me.carrTarEquipMatTarRateTypeControl = 0
            Me.agentControl = 0
            Me.Page = 1
            Me.PageSize = 2000
            Me.Logger = Me.Logger.ForContext(Of GetCarriersByCostParameters)
        End Sub

        ''' <summary>
        ''' Standard Rate It settings with optional no late delivery flag
        ''' </summary>
        ''' <param name="bprefered"></param>
        ''' <param name="bvalidated"></param>
        ''' <param name="boptimizeByCapacity"></param>
        ''' <param name="imodeTypeControl"></param>
        ''' <param name="itempType"></param>
        ''' <param name="bnoLateDelivery"></param>
        Public Sub New(ByVal bprefered As Boolean, ByVal bvalidated As Boolean, ByVal boptimizeByCapacity As Boolean, ByVal imodeTypeControl As Integer, ByVal itempType As Integer, Optional bnoLateDelivery As Boolean = False)
            MyBase.New()
            Me.carrierControl = 0
            Me.prefered = bprefered
            Me.noLateDelivery = bnoLateDelivery
            Me.validated = bvalidated
            Me.optimizeByCapacity = boptimizeByCapacity
            Me.modeTypeControl = imodeTypeControl
            Me.tempType = itempType
            Me.tariffTypeControl = 0
            Me.carrTarEquipMatClass = Nothing
            Me.carrTarEquipMatClassTypeControl = 0
            Me.carrTarEquipMatTarRateTypeControl = 0
            Me.agentControl = 0
            Me.Page = 1
            Me.PageSize = 2000
            Me.Logger = Me.Logger.ForContext(Of GetCarriersByCostParameters)
        End Sub

        ''' <summary>
        ''' update all with optional agent control typically for recalculate
        ''' </summary>
        ''' <param name="bprefered"></param>
        ''' <param name="bnoLateDelivery"></param>
        ''' <param name="bvalidated"></param>
        ''' <param name="boptimizeByCapacity"></param>
        ''' <param name="imodeTypeControl"></param>
        ''' <param name="itempType"></param>
        ''' <param name="itariffTypeControl"></param>
        ''' <param name="icarrTarEquipMatClass"></param>
        ''' <param name="icarrTarEquipMatClassTypeControl"></param>
        ''' <param name="icarrTarEquipMatTarRateTypeControl"></param>
        ''' <param name="iagentControl"></param>
        Public Sub New(ByVal bprefered As Boolean, ByVal bnoLateDelivery As Boolean, ByVal bvalidated As Boolean, ByVal boptimizeByCapacity As Boolean, ByVal imodeTypeControl As Integer, ByVal itempType As Integer, ByVal itariffTypeControl As Integer, ByVal icarrTarEquipMatClass As String, ByVal icarrTarEquipMatClassTypeControl As Integer, ByVal icarrTarEquipMatTarRateTypeControl As Integer, Optional ByVal iagentControl As Integer = 0)
            MyBase.New()
            Me.carrierControl = 0
            Me.prefered = bprefered
            Me.noLateDelivery = bnoLateDelivery
            Me.validated = bvalidated
            Me.optimizeByCapacity = boptimizeByCapacity
            Me.modeTypeControl = imodeTypeControl
            Me.tempType = itempType
            Me.tariffTypeControl = itariffTypeControl
            Me.carrTarEquipMatClass = icarrTarEquipMatClass
            Me.carrTarEquipMatClassTypeControl = icarrTarEquipMatClassTypeControl
            Me.carrTarEquipMatTarRateTypeControl = icarrTarEquipMatTarRateTypeControl
            Me.agentControl = iagentControl
            Me.Page = 1
            Me.PageSize = 2000
            Me.Logger = Me.Logger.ForContext(Of GetCarriersByCostParameters)
        End Sub

#End Region


#Region " Data Members"
        Private _BookControl As Integer
        <DataMember()>
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _carrierControl As Integer
        <DataMember()>
        Public Property carrierControl() As Integer
            Get
                Return _carrierControl
            End Get
            Set(ByVal value As Integer)
                _carrierControl = value
            End Set
        End Property

        Private _prefered As Boolean = True
        <DataMember()>
        Public Property prefered() As Boolean
            Get
                Return _prefered
            End Get
            Set(ByVal value As Boolean)
                _prefered = value
            End Set
        End Property

        Private _noLateDelivery As Boolean = False
        <DataMember()>
        Public Property noLateDelivery() As Boolean
            Get
                Return _noLateDelivery
            End Get
            Set(ByVal value As Boolean)
                _noLateDelivery = value
            End Set
        End Property

        Private _validated As Boolean = False
        <DataMember()>
        Public Property validated() As Boolean
            Get
                Return _validated
            End Get
            Set(ByVal value As Boolean)
                _validated = value
            End Set
        End Property

        Private _optimizeByCapacity As Boolean = True
        <DataMember()>
        Public Property optimizeByCapacity() As Boolean
            Get
                Return _optimizeByCapacity
            End Get
            Set(ByVal value As Boolean)
                _optimizeByCapacity = value
            End Set
        End Property

        Private _modeTypeControl As Integer = 0
        <DataMember()>
        Public Property modeTypeControl() As Integer
            Get
                Return _modeTypeControl
            End Get
            Set(ByVal value As Integer)
                _modeTypeControl = value
            End Set
        End Property

        Private _tempType As Integer = 0
        <DataMember()>
        Public Property tempType() As Integer
            Get
                Return _tempType
            End Get
            Set(ByVal value As Integer)
                _tempType = value
            End Set
        End Property

        Private _tariffTypeControl As Integer = 0
        <DataMember()>
        Public Property tariffTypeControl() As Integer
            Get
                Return _tariffTypeControl
            End Get
            Set(ByVal value As Integer)
                _tariffTypeControl = value
            End Set
        End Property

        Private _carrTarEquipMatClass As String
        <DataMember()>
        Public Property carrTarEquipMatClass() As String
            Get
                Return Left(_carrTarEquipMatClass, 50)
            End Get
            Set(ByVal value As String)
                _carrTarEquipMatClass = Left(value, 50)
            End Set
        End Property

        Private _carrTarEquipMatClassTypeControl As Integer = 0
        <DataMember()>
        Public Property carrTarEquipMatClassTypeControl() As Integer
            Get
                Return _carrTarEquipMatClassTypeControl
            End Get
            Set(ByVal value As Integer)
                _carrTarEquipMatClassTypeControl = value
            End Set
        End Property

        Private _carrTarEquipMatTarRateTypeControl As Integer = 0
        <DataMember()>
        Public Property carrTarEquipMatTarRateTypeControl() As Integer
            Get
                Return _carrTarEquipMatTarRateTypeControl
            End Get
            Set(ByVal value As Integer)
                _carrTarEquipMatTarRateTypeControl = value
            End Set
        End Property

        Private _agentControl As Integer = 0
        <DataMember()>
        Public Property agentControl() As Integer
            Get
                Return _agentControl
            End Get
            Set(ByVal value As Integer)
                _agentControl = value
            End Set
        End Property

        'Old properties from client (may not be needed or used on server)

        Private _LaneControl As Integer
        <DataMember()>
        Public Property LaneControl() As Integer
            Get
                Return _LaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneControl = value
            End Set
        End Property

        Private _Cons As String
        <DataMember()>
        Public Property Cons() As String
            Get
                Return _Cons
            End Get
            Set(ByVal value As String)
                _Cons = value
            End Set
        End Property

        Private _CustCompControl As Integer = 0
        <DataMember()>
        Public Property CustCompControl() As Integer
            Get
                Return _CustCompControl
            End Get
            Set(ByVal value As Integer)
                _CustCompControl = value
            End Set
        End Property

        Private _OriginalCarrierControl As Integer = 0
        <DataMember()>
        Public Property OriginalCarrierControl() As Integer
            Get
                Return _OriginalCarrierControl
            End Get
            Set(ByVal value As Integer)
                _OriginalCarrierControl = value
            End Set
        End Property

        Private _AllowAsync As Boolean = True
        ''' <summary>
        ''' Allow Create NGL Tariff Bid Process to run in background
        ''' </summary>
        ''' <returns></returns>
        <DataMember()>
        Public Property AllowAsync() As Boolean
            Get
                Return _AllowAsync
            End Get
            Set(ByVal value As Boolean)
                _AllowAsync = value
            End Set
        End Property

        Private _AllowP44Async As Boolean = True
        ''' <summary>
        ''' Allow The P44 API Bid Process to run in background
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.006 on 05/24/2024 to add the AllowP44Async property
        ''' </remarks>
        <DataMember()>
        Public Property AllowP44Async() As Boolean
            Get
                Return _AllowP44Async
            End Get
            Set(ByVal value As Boolean)
                _AllowP44Async = value
            End Set
        End Property

        Private _AllowNGLAPIAsync As Boolean = False
        ''' <summary>
        ''' Allow All of the NGL API Bid Processs to run in background
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.006 on 05/24/2024 to add the AllowNGLAPIAsync property
        ''' </remarks>
        <DataMember()>
        Public Property AllowNGLAPIAsync() As Boolean
            Get
                Return _AllowNGLAPIAsync
            End Get
            Set(ByVal value As Boolean)
                _AllowNGLAPIAsync = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New GetCarriersByCostParameters
            instance = DirectCast(MemberwiseClone(), GetCarriersByCostParameters)
            Return instance
        End Function

        Public Overrides Function ToString() As String
            Return $"BookControl: {BookControl}, AllowAsync: {AllowAsync}, AllowP44Async: {AllowP44Async}, AllowNGLAPIAsync: {AllowNGLAPIAsync} CarrierControl: {carrierControl}, Prefered: {prefered},ModeTypeControl: {modeTypeControl}, TempType: {tempType}, TariffTypeControl: {tariffTypeControl}, NoLateDelivery: {noLateDelivery}, Validated: {validated}, OptimizeByCapacity: {optimizeByCapacity},  CarrTarEquipMatClass: {carrTarEquipMatClass}, CarrTarEquipMatClassTypeControl: {carrTarEquipMatClassTypeControl}, CarrTarEquipMatTarRateTypeControl: {carrTarEquipMatTarRateTypeControl}, AgentControl: {agentControl}, LaneControl: {LaneControl}, Cons: {Cons}, CustCompControl: {CustCompControl}, OriginalCarrierControl: {OriginalCarrierControl}"
        End Function

#End Region
    End Class
End Namespace
