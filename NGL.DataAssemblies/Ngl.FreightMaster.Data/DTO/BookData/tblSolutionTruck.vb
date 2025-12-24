Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblSolutionTruck
        Inherits DTOBaseClass

#Region " ENUM "
        Public Enum CapacityType
            Cases
            Weight
            Pallets
            Cubes
        End Enum

        Public Enum RoutingCapacityPreference
            Sequence
            Cases
            Weight
            Pallets
            Cubes
        End Enum

        Public Enum RouteTypeCodes
            Full_Load = 1
            Multi_Pick
            Consolidated
            LTL_Pool
            Single_LTL
            Unassigned
            Hold
        End Enum
#End Region

#Region " Constructors "


        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByRef parameters As WCFParameters, ByRef truckdata As NGLLoadPlanningTruckData)
            MyBase.New()
            Me.Parameters = parameters
            Me.TruckDataObject = truckdata
            Me.CNS = getTempCNSNbr()
        End Sub

        Public Sub New(ByRef parameters As WCFParameters, ByRef truckdata As NGLLoadPlanningTruckData, ByRef batchdata As NGLBatchProcessDataProvider, ByRef itemdata As NGLBookItemData)
            MyBase.New()
            Me.Parameters = parameters
            Me.TruckDataObject = truckdata
            Me.BatchProcessingDataObject = batchdata
            Me.BookItemDataObject = itemdata
            Me.CNS = getTempCNSNbr()
        End Sub

#End Region

#Region " Server Properties "

        Private settingCarriersForRoute As CarriersForRoute

        Private _RouteConfig As New RouteConfigForEquip
        Public Property RouteConfig As RouteConfigForEquip
            Get
                Return _RouteConfig
            End Get
            Set(value As RouteConfigForEquip)
                _RouteConfig = value
            End Set
        End Property

        Private _TruckDataObject As NGLLoadPlanningTruckData
        Public Property TruckDataObject As NGLLoadPlanningTruckData
            Get
                If _TruckDataObject Is Nothing Then _TruckDataObject = New NGLLoadPlanningTruckData(Me.Parameters)
                Return _TruckDataObject
            End Get
            Set(value As NGLLoadPlanningTruckData)
                _TruckDataObject = value
            End Set
        End Property

        Private _BookItemDataObject As NGLBookItemData
        Public Property BookItemDataObject As NGLBookItemData
            Get
                If _BookItemDataObject Is Nothing Then _BookItemDataObject = New NGLBookItemData(Me.Parameters)
                Return _BookItemDataObject
            End Get
            Set(value As NGLBookItemData)
                _BookItemDataObject = value
            End Set
        End Property


        Private _BatchProcessingDataObject As NGLBatchProcessDataProvider
        Public Property BatchProcessingDataObject As NGLBatchProcessDataProvider
            Get
                If _BatchProcessingDataObject Is Nothing Then _BatchProcessingDataObject = New NGLBatchProcessDataProvider(Me.Parameters)
                Return _BatchProcessingDataObject
            End Get
            Set(value As NGLBatchProcessDataProvider)
                _BatchProcessingDataObject = value
            End Set
        End Property

        Public ReadOnly Property maxCases As Double
            Get
                Return SolutionTruckMaxCases
            End Get
        End Property

        Public ReadOnly Property maxWgt As Double
            Get
                Return SolutionTruckMaxWgt
            End Get
        End Property

        Public ReadOnly Property maxPLTs As Double
            Get
                Return SolutionTruckMaxPlts
            End Get
        End Property

        Public ReadOnly Property maxCubes As Double
            Get
                Return SolutionTruckMaxCubes
            End Get
        End Property

        Public Property CNS As String
            Get
                Return SolutionTruckConsPrefix
            End Get
            Set(value As String)
                SolutionTruckConsPrefix = value
            End Set
        End Property

        Public Property Orders As List(Of tblSolutionDetail)
            Get
                Return SolutionDetails
            End Get
            Set(value As List(Of tblSolutionDetail))
                SolutionDetails = value
            End Set
        End Property

        Public ReadOnly Property minTuckLoadCases As Double
            Get
                Return Me.SolutionTruckMinCases
            End Get
        End Property

        Public ReadOnly Property minTuckLoadWgt As Double
            Get
                Return Me.SolutionTruckMinWgt
            End Get
        End Property

        Public ReadOnly Property minTuckLoadPLTs As Double
            Get
                Return Me.SolutionTruckMinPlts
            End Get
        End Property

        Public ReadOnly Property minTuckLoadCubes As Double
            Get
                Return Me.SolutionTruckMinCubes
            End Get
        End Property



        Public ReadOnly Property minTenderLoadCases As Double
            Get
                Return If(Me.SolutionTruckSplitCases = 0, Me.SolutionTruckMaxCases, Me.SolutionTruckSplitCases)
            End Get
        End Property

        Public ReadOnly Property minTenderLoadWgt As Double
            Get
                Return If(Me.SolutionTruckSplitWgt = 0, Me.SolutionTruckMaxWgt, Me.SolutionTruckSplitWgt)

            End Get
        End Property

        Public ReadOnly Property minTenderLoadPLTs As Double
            Get
                Return If(Me.SolutionTruckSplitPlts = 0, Me.SolutionTruckMaxPlts, Me.SolutionTruckSplitPlts)
            End Get
        End Property

        Public ReadOnly Property minTenderLoadCubes As Double
            Get
                Return If(Me.SolutionTruckSplitCubes = 0, Me.SolutionTruckMaxCubes, Me.SolutionTruckSplitCubes)
            End Get
        End Property

        Public ReadOnly Property atCapacity() As Boolean
            Get
                If (maxPLTs > 0 AndAlso TotalPlts >= maxPLTs) OrElse (maxWgt > 0 AndAlso TotalWgt >= maxWgt) OrElse (maxCases > 0 AndAlso TotalCases >= maxCases) OrElse (maxCubes > 0 AndAlso TotalCubes >= maxCubes) Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property MinSequence() As Integer
            Get
                If Not Orders Is Nothing AndAlso Orders.Count > 0 Then
                    Return Orders.Min(Function(c) c.RouteSequence)
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property MaxSequence() As Integer
            Get
                If Not Orders Is Nothing AndAlso Orders.Count > 0 Then
                    Return Orders.Max(Function(c) c.RouteSequence)
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property TotalCases() As Double
            Get
                If Not Orders Is Nothing AndAlso Orders.Count > 0 Then
                    Dim total = Orders.Sum(Function(c) c.Cases)
                    If Me.SolutionTruckTotalCases <> total Then
                        Me.SolutionTruckTotalCases = total
                    End If
                Else
                    Me.SolutionTruckTotalCases = 0
                End If
                Return Me.SolutionTruckTotalCases
            End Get
        End Property

        Public ReadOnly Property TotalWgt() As Double
            Get
                If Not Orders Is Nothing AndAlso Orders.Count > 0 Then
                    Dim total = Orders.Sum(Function(c) c.Wgt)
                    If Me.SolutionTruckTotalWgt <> total Then
                        Me.SolutionTruckTotalWgt = total
                    End If
                Else
                    Me.SolutionTruckTotalWgt = 0
                End If
                Return Me.SolutionTruckTotalWgt
            End Get
        End Property

        Public ReadOnly Property TotalPlts() As Double
            Get
                If Not Orders Is Nothing AndAlso Orders.Count > 0 Then
                    Dim total = Orders.Sum(Function(c) c.Plts)
                    If Me.SolutionTruckTotalPL <> total Then
                        Me.SolutionTruckTotalPL = total
                    End If
                Else
                    Me.SolutionTruckTotalPL = 0
                End If
                Return Me.SolutionTruckTotalPL
            End Get
        End Property

        Public ReadOnly Property TotalCubes() As Double
            Get
                If Not Orders Is Nothing AndAlso Orders.Count > 0 Then
                    Dim total = Orders.Sum(Function(c) c.Cubes)
                    If Me.SolutionTruckTotalCube <> total Then
                        Me.SolutionTruckTotalCube = total
                    End If
                Else
                    Me.SolutionTruckTotalCube = 0
                End If
                Return Me.SolutionTruckTotalCube
            End Get
        End Property

#End Region

#Region " Data Members"
        Private _SolutionTruckControl As Long = 0
        <DataMember()> _
        Public Property SolutionTruckControl() As Long
            Get
                Return _SolutionTruckControl
            End Get
            Set(ByVal value As Long)
                If ((Me._SolutionTruckControl = value) = False) Then
                    Me._SolutionTruckControl = value
                    Me.SendPropertyChanged("SolutionTruckControl")
                End If
            End Set
        End Property

        Private _SolutionTruckKey As String = ""
        <DataMember()> _
        Public Property SolutionTruckKey() As String
            Get
                Return Left(Me._SolutionTruckKey, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckKey, value) = False) Then
                    Me._SolutionTruckKey = Left(value, 50)
                    Me.SendPropertyChanged("SolutionTruckKey")
                End If
            End Set
        End Property

        Private _SolutionTruckSolutionControl As Long = 0
        <DataMember()> _
        Public Property SolutionTruckSolutionControl() As Long
            Get
                Return _SolutionTruckSolutionControl
            End Get
            Set(ByVal value As Long)
                If ((Me._SolutionTruckSolutionControl = value) = False) Then
                    Me._SolutionTruckSolutionControl = value
                    Me.SendPropertyChanged("SolutionTruckSolutionControl")
                End If
            End Set
        End Property



        Private _SolutionTruckCompControl As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckCompControl() As Integer
            Get
                Return _SolutionTruckCompControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckCompControl = value) = False) Then
                    Me._SolutionTruckCompControl = value
                    Me.SendPropertyChanged("SolutionTruckCompControl")
                End If
            End Set
        End Property

        Private _SolutionTruckStaticRouteControl As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckStaticRouteControl() As Integer
            Get
                Return Me._SolutionTruckStaticRouteControl
            End Get
            Set(value As Integer)
                If ((Me._SolutionTruckStaticRouteControl = value) _
                   = False) Then
                    Me._SolutionTruckStaticRouteControl = value
                    Me.SendPropertyChanged("SolutionTruckStaticRouteControl")
                End If
            End Set
        End Property

        Private _SolutionTruckStaticRouteNumber As String = ""
        <DataMember()> _
        Public Property SolutionTruckStaticRouteNumber() As String
            Get
                Return Left(Me._SolutionTruckStaticRouteNumber, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckStaticRouteNumber, value) = False) Then
                    Me._SolutionTruckStaticRouteNumber = Left(value, 50)
                    Me.SendPropertyChanged("SolutionTruckStaticRouteNumber")
                End If
            End Set
        End Property

        Private _SolutionTruckAttributeControl As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckAttributeControl() As Integer
            Get
                Return Me._SolutionTruckAttributeControl
            End Get
            Set(value As Integer)
                If ((Me._SolutionTruckAttributeControl = value) _
                   = False) Then
                    Me._SolutionTruckAttributeControl = value
                    Me.SendPropertyChanged("SolutionTruckAttributeControl")
                End If
            End Set
        End Property


        Private _SolutionTruckAttributeTypeControl As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckAttributeTypeControl() As Integer
            Get
                Return Me._SolutionTruckAttributeTypeControl
            End Get
            Set(value As Integer)
                If ((Me._SolutionTruckAttributeTypeControl = value) _
                   = False) Then
                    Me._SolutionTruckAttributeTypeControl = value
                    Me.SendPropertyChanged("SolutionTruckAttributeTypeControl")
                End If
            End Set
        End Property

        Private _SolutionTruckCom As String = ""
        <DataMember()> _
        Public Property SolutionTruckCom() As String
            Get
                Return Left(Me._SolutionTruckCom, 1)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckCom, value) = False) Then
                    Me._SolutionTruckCom = Left(value, 1)
                    Me.SendPropertyChanged("SolutionTruckCom")
                End If
            End Set
        End Property

        Private _SolutionTruckConsPrefix As String = ""
        <DataMember()> _
        Public Property SolutionTruckConsPrefix() As String
            Get
                Return Left(Me._SolutionTruckConsPrefix, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckConsPrefix, value) = False) Then
                    Me._SolutionTruckConsPrefix = Left(value, 20)
                    Me.SendPropertyChanged("SolutionTruckConsPrefix")
                End If
            End Set
        End Property

        Private _SolutionTruckRouteConsFlag As Boolean = False
        <DataMember()> _
        Public Property SolutionTruckRouteConsFlag() As Boolean
            Get
                Return Me._SolutionTruckRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionTruckRouteConsFlag = value) = False) Then
                    Me._SolutionTruckRouteConsFlag = value
                    Me.SendPropertyChanged("SolutionTruckRouteConsFlag")
                End If
            End Set
        End Property

        Private _SolutionTruckCarrierControl As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckCarrierControl() As Integer
            Get
                Return Me._SolutionTruckCarrierControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckCarrierControl = value) = False) Then
                    Me._SolutionTruckCarrierControl = value
                    Me.SendPropertyChanged("SolutionTruckCarrierControl")
                End If
            End Set
        End Property

        Private _SolutionTruckCarrierNumber As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckCarrierNumber() As Integer
            Get
                Return Me._SolutionTruckCarrierNumber
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckCarrierNumber = value) = False) Then
                    Me._SolutionTruckCarrierNumber = value
                    Me.SendPropertyChanged("SolutionTruckCarrierNumber")
                End If
            End Set
        End Property

        Private _SolutionTruckCarrierName As String = ""
        <DataMember()> _
        Public Property SolutionTruckCarrierName() As String
            Get
                Return Left(Me._SolutionTruckCarrierName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckCarrierName, value) = False) Then
                    Me._SolutionTruckCarrierName = Left(value, 40)
                    Me.SendPropertyChanged("SolutionTruckCarrierName")
                End If
            End Set
        End Property

        Private _SolutionTruckCarrierTruckControl As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckCarrierTruckControl() As Integer
            Get
                Return Me._SolutionTruckCarrierTruckControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckCarrierTruckControl = value) = False) Then
                    Me._SolutionTruckCarrierTruckControl = value
                    Me.SendPropertyChanged("SolutionTruckCarrierTruckControl")
                End If
            End Set
        End Property

        Private _SolutionTruckCarrierTruckDescription As String = ""
        <DataMember()> _
        Public Property SolutionTruckCarrierTruckDescription() As String
            Get
                Return Left(Me._SolutionTruckCarrierTruckDescription, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckCarrierTruckDescription, value) = False) Then
                    Me._SolutionTruckCarrierTruckDescription = Left(value, 255)
                    Me.SendPropertyChanged("SolutionTruckCarrierTruckDescription")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalCases As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckTotalCases() As Integer
            Get
                Return Me._SolutionTruckTotalCases
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckTotalCases = value) = False) Then
                    Me._SolutionTruckTotalCases = value
                    Me.SendPropertyChanged("SolutionTruckTotalCases")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalWgt As Double = 0
        <DataMember()> _
        Public Property SolutionTruckTotalWgt() As Double
            Get
                Return Me._SolutionTruckTotalWgt
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTruckTotalWgt = value) = False) Then
                    Me._SolutionTruckTotalWgt = value
                    Me.SendPropertyChanged("SolutionTruckTotalWgt")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalPL As Double = 0
        <DataMember()> _
        Public Property SolutionTruckTotalPL() As Double
            Get
                Return Me._SolutionTruckTotalPL
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTruckTotalPL = value) = False) Then
                    Me._SolutionTruckTotalPL = value
                    Me.SendPropertyChanged("SolutionTruckTotalPL")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalCube As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckTotalCube() As Integer
            Get
                Return Me._SolutionTruckTotalCube
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckTotalCube = value) = False) Then
                    Me._SolutionTruckTotalCube = value
                    Me.SendPropertyChanged("SolutionTruckTotalCube")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalPX As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckTotalPX() As Integer
            Get
                Return Me._SolutionTruckTotalPX
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckTotalPX = value) = False) Then
                    Me._SolutionTruckTotalPX = value
                    Me.SendPropertyChanged("SolutionTruckTotalPX")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalBFC As Decimal = 0
        <DataMember()> _
        Public Property SolutionTruckTotalBFC() As Decimal
            Get
                Return Me._SolutionTruckTotalBFC
            End Get
            Set(ByVal value As Decimal)
                If ((Me._SolutionTruckTotalBFC = value) = False) Then
                    Me._SolutionTruckTotalBFC = value
                    Me.SendPropertyChanged("SolutionTruckTotalBFC")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalOrders As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckTotalOrders() As Integer
            Get
                Return Me._SolutionTruckTotalOrders
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckTotalOrders = value) = False) Then
                    Me._SolutionTruckTotalOrders = value
                    Me.SendPropertyChanged("SolutionTruckTotalOrders")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalCost As Double = 0
        <DataMember()> _
        Public Property SolutionTruckTotalCost() As Double
            Get
                Return Me._SolutionTruckTotalCost
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTruckTotalCost = value) = False) Then
                    Me._SolutionTruckTotalCost = value
                    Me.SendPropertyChanged("SolutionTruckTotalCost")
                End If
            End Set
        End Property

        Private _SolutionTruckTotalMiles As Double = 0
        <DataMember()> _
        Public Property SolutionTruckTotalMiles() As Double
            Get
                Return Me._SolutionTruckTotalMiles
            End Get
            Set(ByVal value As Double)
                If ((Me._SolutionTruckTotalMiles = value) = False) Then
                    Me._SolutionTruckTotalMiles = value
                    Me.SendPropertyChanged("SolutionTruckTotalMiles")
                End If
            End Set
        End Property

        Private _SolutionTruckCarrierEquipmentCodes As String = ""
        <DataMember()> _
        Public Property SolutionTruckCarrierEquipmentCodes() As String
            Get
                Return Left(Me._SolutionTruckCarrierEquipmentCodes, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckCarrierEquipmentCodes, value) = False) Then
                    Me._SolutionTruckCarrierEquipmentCodes = Left(value, 50)
                    Me.SendPropertyChanged("SolutionTruckCarrierEquipmentCodes")
                End If
            End Set
        End Property

        Private _SolutionTruckRouteTypeCode As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckRouteTypeCode() As Integer
            Get
                Return Me._SolutionTruckRouteTypeCode
            End Get
            Set(ByVal value As Integer)
                If ((Me._SolutionTruckRouteTypeCode = value) = False) Then
                    Me._SolutionTruckRouteTypeCode = value
                    Me.SendPropertyChanged("SolutionTruckRouteTypeCode")
                End If
            End Set
        End Property

        Private _SolutionTruckCommitted As Boolean = False
        <DataMember()> _
        Public Property SolutionTruckCommitted() As Boolean
            Get
                Return Me._SolutionTruckCommitted
            End Get
            Set(ByVal value As Boolean)
                If ((Me._SolutionTruckCommitted = value) = False) Then
                    Me._SolutionTruckCommitted = value
                    Me.SendPropertyChanged("SolutionTruckCommitted")
                End If
            End Set
        End Property

        Private _SolutionTruckCommittedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionTruckCommittedDate() As System.Nullable(Of Date)
            Get
                Return Me._SolutionTruckCommittedDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SolutionTruckCommittedDate.Equals(value) = False) Then
                    Me._SolutionTruckCommittedDate = value
                    Me.SendPropertyChanged("SolutionTruckCommittedDate")
                End If
            End Set
        End Property

        Private _SolutionTruckCapacityPreference As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckCapacityPreference() As Integer
            Get
                Return Me._SolutionTruckCapacityPreference
            End Get
            Set(value As Integer)
                If ((Me._SolutionTruckCapacityPreference = value) _
                   = False) Then
                    Me._SolutionTruckCapacityPreference = value
                    Me.SendPropertyChanged("SolutionTruckCapacityPreference")
                End If
            End Set
        End Property

        Private _SolutionTruckMinCases As Double = 0
        <DataMember()> _
        Public Property SolutionTruckMinCases() As Double
            Get
                Return Me._SolutionTruckMinCases
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckMinCases = value) _
                   = False) Then
                    Me._SolutionTruckMinCases = value
                    Me.SendPropertyChanged("SolutionTruckMinCases")
                End If
            End Set
        End Property

        Private _SolutionTruckSplitCases As Double = 0
        <DataMember()> _
        Public Property SolutionTruckSplitCases() As Double
            Get
                Return Me._SolutionTruckSplitCases
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckSplitCases = value) _
                   = False) Then
                    Me._SolutionTruckSplitCases = value
                    Me.SendPropertyChanged("SolutionTruckSplitCases")
                End If
            End Set
        End Property

        Private _SolutionTruckMaxCases As Double = 0
        <DataMember()> _
        Public Property SolutionTruckMaxCases() As Double
            Get
                Return Me._SolutionTruckMaxCases
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckMaxCases = value) _
                   = False) Then
                    Me._SolutionTruckMaxCases = value
                    Me.SendPropertyChanged("SolutionTruckMaxCases")
                End If
            End Set
        End Property

        Private _SolutionTruckMinWgt As Double = 0
        <DataMember()> _
        Public Property SolutionTruckMinWgt() As Double
            Get
                Return Me._SolutionTruckMinWgt
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckMinWgt = value) _
                   = False) Then
                    Me._SolutionTruckMinWgt = value
                    Me.SendPropertyChanged("SolutionTruckMinWgt")
                End If
            End Set
        End Property

        Private _SolutionTruckSplitWgt As Double = 0
        <DataMember()> _
        Public Property SolutionTruckSplitWgt() As Double
            Get
                Return Me._SolutionTruckSplitWgt
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckSplitWgt = value) _
                   = False) Then
                    Me._SolutionTruckSplitWgt = value
                    Me.SendPropertyChanged("SolutionTruckSplitWgt")
                End If
            End Set
        End Property

        Private _SolutionTruckMaxWgt As Double = 0
        <DataMember()> _
        Public Property SolutionTruckMaxWgt() As Double
            Get
                Return Me._SolutionTruckMaxWgt
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckMaxWgt = value) _
                   = False) Then
                    Me._SolutionTruckMaxWgt = value
                    Me.SendPropertyChanged("SolutionTruckMaxWgt")
                End If
            End Set
        End Property

        Private _SolutionTruckMinCubes As Double = 0
        <DataMember()> _
        Public Property SolutionTruckMinCubes() As Double
            Get
                Return Me._SolutionTruckMinCubes
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckMinCubes = value) _
                   = False) Then
                    Me._SolutionTruckMinCubes = value
                    Me.SendPropertyChanged("SolutionTruckMinCubes")
                End If
            End Set
        End Property

        Private _SolutionTruckSplitCubes As Double = 0
        <DataMember()> _
        Public Property SolutionTruckSplitCubes() As Double
            Get
                Return Me._SolutionTruckSplitCubes
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckSplitCubes = value) _
                   = False) Then
                    Me._SolutionTruckSplitCubes = value
                    Me.SendPropertyChanged("SolutionTruckSplitCubes")
                End If
            End Set
        End Property

        Private _SolutionTruckMaxCubes As Double = 0
        <DataMember()> _
        Public Property SolutionTruckMaxCubes() As Double
            Get
                Return Me._SolutionTruckMaxCubes
            End Get
            Set(value As Double)
                If ((Me._SolutionTruckMaxCubes = value) _
                   = False) Then
                    Me._SolutionTruckMaxCubes = value
                    Me.SendPropertyChanged("SolutionTruckMaxCubes")
                End If
            End Set
        End Property

        Private _SolutionTruckMinPlts As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckMinPlts() As Integer
            Get
                Return Me._SolutionTruckMinPlts
            End Get
            Set(value As Integer)
                If ((Me._SolutionTruckMinPlts = value) _
                   = False) Then
                    Me._SolutionTruckMinPlts = value
                    Me.SendPropertyChanged("SolutionTruckMinPlts")
                End If
            End Set
        End Property

        Private _SolutionTruckSplitPlts As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckSplitPlts() As Integer
            Get
                Return Me._SolutionTruckSplitPlts
            End Get
            Set(value As Integer)
                If ((Me._SolutionTruckSplitPlts = value) _
                   = False) Then
                    Me._SolutionTruckSplitPlts = value
                    Me.SendPropertyChanged("SolutionTruckSplitPlts")
                End If
            End Set
        End Property

        Private _SolutionTruckMaxPlts As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckMaxPlts() As Integer
            Get
                Return Me._SolutionTruckMaxPlts
            End Get
            Set(value As Integer)
                If ((Me._SolutionTruckMaxPlts = value) _
                   = False) Then
                    Me._SolutionTruckMaxPlts = value
                    Me.SendPropertyChanged("SolutionTruckMaxPlts")
                End If
            End Set
        End Property

        Private _SolutionTruckTrucksAvailable As Integer = 0
        <DataMember()> _
        Public Property SolutionTruckTrucksAvailable() As Integer
            Get
                Return Me._SolutionTruckTrucksAvailable
            End Get
            Set(value As Integer)
                If ((Me._SolutionTruckTrucksAvailable = value) _
                   = False) Then
                    Me._SolutionTruckTrucksAvailable = value
                    Me.SendPropertyChanged("SolutionTruckTrucksAvailable")
                End If
            End Set
        End Property


        Private _SolutionTruckIsHazmat As Boolean = False
        <DataMember()> _
        Public Property SolutionTruckIsHazmat() As Boolean
            Get
                Return Me._SolutionTruckIsHazmat
            End Get
            Set(value As Boolean)
                If ((Me._SolutionTruckIsHazmat = value) _
                   = False) Then
                    Me._SolutionTruckIsHazmat = value
                    Me.SendPropertyChanged("SolutionTruckIsHazmat")
                End If
            End Set
        End Property


        Private _SolutionTruckLaneNumbers As String = ""
        <DataMember()> _
        Public Property SolutionTruckLaneNumbers() As String
            Get
                Return Me._SolutionTruckLaneNumbers
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckLaneNumbers, value) = False) Then
                    Me._SolutionTruckLaneNumbers = value
                    Me.SendPropertyChanged("SolutionTruckLaneNumbers")
                End If
            End Set
        End Property

        Private _SolutionTruckLaneNames As String = ""
        <DataMember()> _
        Public Property SolutionTruckLaneNames() As String
            Get
                Return Me._SolutionTruckLaneNames
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckLaneNames, value) = False) Then
                    Me._SolutionTruckLaneNames = value
                    Me.SendPropertyChanged("SolutionTruckLaneNames")
                End If
            End Set
        End Property


        Private _SolutionTruckBookNotes As String = ""
        <DataMember()> _
        Public Property SolutionTruckBookNotes() As String
            Get
                Return Me._SolutionTruckBookNotes
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckBookNotes, value) = False) Then
                    Me._SolutionTruckBookNotes = value
                    Me.SendPropertyChanged("SolutionTruckBookNotes")
                End If
            End Set
        End Property

        Private _SolutionTruckModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SolutionTruckModDate() As System.Nullable(Of Date)
            Get
                Return _SolutionTruckModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _SolutionTruckModDate = value
                Me.SendPropertyChanged("SolutionTruckModDate")
            End Set
        End Property

        Private _SolutionTruckModUser As String = ""
        <DataMember()> _
        Public Property SolutionTruckModUser() As String
            Get
                Return Left(_SolutionTruckModUser, 100)
            End Get
            Set(ByVal value As String)
                _SolutionTruckModUser = Left(value, 100)
                Me.SendPropertyChanged("SolutionTruckModUser")
            End Set
        End Property

        Private _SolutionTruckUpdated As Byte()
        <DataMember()>
        Public Property SolutionTruckUpdated() As Byte()
            Get
                Return _SolutionTruckUpdated
            End Get
            Set(ByVal value As Byte())
                _SolutionTruckUpdated = value
            End Set
        End Property
        Private _SolutionTruckCarrierSCAC As String = ""
        <DataMember()>
        Public Property SolutionTruckCarrierSCAC() As String
            Get
                Return Left(Me._SolutionTruckCarrierSCAC, 8)
            End Get
            Set(value As String)
                If (String.Equals(Me._SolutionTruckCarrierSCAC, value) = False) Then
                    Me._SolutionTruckCarrierSCAC = Left(value, 8)
                    Me.SendPropertyChanged("SolutionTruckCarrierSCAC")
                End If
            End Set
        End Property

        'Private _DestStates As String = ""
        '<DataMember()> _
        'Public Property DestStates() As String
        '    Get
        '        Return _DestStates
        '    End Get
        '    Set(ByVal value As String)
        '        _DestStates = value
        '    End Set
        'End Property



        Private _SolutionDetails As New List(Of tblSolutionDetail)
        <DataMember()> _
        Public Property SolutionDetails() As List(Of tblSolutionDetail)
            Get
                If _SolutionDetails Is Nothing Then _SolutionDetails = New List(Of tblSolutionDetail)
                Return _SolutionDetails
            End Get
            Set(ByVal value As List(Of tblSolutionDetail))
                _SolutionDetails = value
            End Set
        End Property

        Private _Stops As New NEXTrackTreeNode
        <DataMember()> _
        Public Property Stops() As NEXTrackTreeNode
            Get
                Return _Stops
            End Get
            Set(ByVal value As NEXTrackTreeNode)
                _Stops = value
            End Set
        End Property

        Private _Messages As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()> _
        Public Property Messages() As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Messages
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Messages = value
            End Set
        End Property

        Private _Log As New List(Of NGLMessage)
        <DataMember()> _
        Public Property Log() As List(Of NGLMessage)
            Get
                Return _Log
            End Get
            Set(ByVal value As List(Of NGLMessage))
                _Log = value
            End Set
        End Property

       

#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblSolutionTruck
            instance = DirectCast(MemberwiseClone(), tblSolutionTruck)
            instance.SolutionDetails = Nothing
            For Each item In SolutionDetails
                instance.SolutionDetails.Add(DirectCast(item.Clone, tblSolutionDetail))
            Next
            Return instance
        End Function

        Public Sub addMessages(ByRef oMsg As List(Of LTS.tblNGLMessageRefBook))
            If Not oMsg Is Nothing AndAlso oMsg.Count() > 0 Then
                Dim sLastKey As String = ""
                Dim lMsg As New List(Of NGLMessage)
                For Each m In oMsg.OrderBy(Function(x) x.NMKeyString).ToList()
                    If Not m Is Nothing AndAlso Not String.IsNullOrWhiteSpace(m.NMKeyString) Then
                        If String.IsNullOrWhiteSpace(sLastKey) Then
                            sLastKey = m.NMKeyString
                            lMsg = New List(Of NGLMessage)
                        ElseIf m.NMKeyString <> sLastKey Then
                            If Me.Messages.ContainsKey(sLastKey) Then
                                Me.Messages(sLastKey) = lMsg
                            Else
                                Me.Messages.Add(sLastKey, lMsg)
                            End If
                            lMsg = New List(Of NGLMessage)
                            sLastKey = m.NMKeyString
                        End If
                        lMsg.Add(New NGLMessage(m.NMMessage, m.NMMTRefControl, m.NMMTRefAlphaControl, m.NMMTRefName, m.NMNMTControl, m.NMErrorReason, m.NMErrorMessage, m.NMErrorDetails))
                    End If
                Next
                If Not lMsg Is Nothing AndAlso lMsg.Count() > 0 Then
                    'add the last list of messages to the collection
                    If Me.Messages.ContainsKey(sLastKey) Then
                        Me.Messages(sLastKey) = lMsg
                    Else
                        Me.Messages.Add(sLastKey, lMsg)
                    End If
                End If
            End If
        End Sub

        Public Function readMessages() As List(Of LTS.tblNGLMessageRefBook)
            Dim lRet As New List(Of LTS.tblNGLMessageRefBook)
            If Not Me.Messages Is Nothing AndAlso Me.Messages.Count() > 0 Then
                For Each d In Me.Messages
                    If Not d.Value Is Nothing AndAlso d.Value.Count > 0 Then
                        For Each m In d.Value
                            Dim ltsMsg As New LTS.tblNGLMessageRefBook
                            With ltsMsg
                                .NMNMTControl = m.ControlReference
                                .NMMTRefControl = m.Control
                                .NMMTRefAlphaControl = m.AlphaCode
                                .NMMTRefName = m.ControlReferenceName
                                .NMKeyString = d.Key
                                .NMMessage = m.Message
                                .NMErrorReason = m.ErrorReason
                                .NMErrorDetails = m.ErrorDetails
                                .NMErrorMessage = m.ErrorMessage
                            End With
                            lRet.Add(ltsMsg)
                        Next
                    End If
                Next
            End If
            Return lRet
        End Function


        ''' <summary>
        ''' The method adds an order to the current truck then saves the order information to the db
        ''' to save the changes to the database we must call spUpdateConsolidationNumber
        ''' </summary>
        ''' <param name="o"></param>
        ''' <param name="RtCapPref"></param>
        ''' <param name="canUseLargerTruck"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function addOrder(ByRef o As tblSolutionDetail, ByVal RtCapPref As RoutingCapacityPreference, Optional ByVal canUseLargerTruck As Boolean = True) As Boolean
            Dim blnRet As Boolean = False
            If Me.TruckDataObject Is Nothing Then Return False 'cannot route load without a truckdataobject   
            If CanFit(CapacityType.Cases, o.Cases) _
                And CanFit(CapacityType.Weight, o.Wgt) _
                And CanFit(CapacityType.Pallets, o.Plts) _
                And CanFit(CapacityType.Cubes, o.Cubes) Then
                'The following fields get updated when we add an order
                'We try to set these value here but they are re=set again upon 
                'save in the case where orders are moved from truck to truck without 
                'calling the addOrder method!!!
                o.SolutionDetailCarrierControl = Me.SolutionTruckCarrierControl
                o.SolutionDetailConsPrefix = Me.CNS
                Select Case Me.SolutionTruckRouteTypeCode
                    Case RouteTypeCodes.LTL_Pool
                        o.SolutionDetailRouteConsFlag = False
                    Case RouteTypeCodes.Full_Load
                        o.SolutionDetailRouteConsFlag = True
                        If Not Me.Orders Is Nothing AndAlso Me.Orders.Count > 0 AndAlso o.LaneLocation <> Me.Orders(0).LaneLocation Then
                            Return False
                        End If
                    Case RouteTypeCodes.Single_LTL
                        o.SolutionDetailRouteConsFlag = True
                        If Not Me.Orders Is Nothing AndAlso Me.Orders.Count > 0 Then
                            Return False
                        End If
                    Case Else
                        o.SolutionDetailRouteConsFlag = True
                End Select
                o.SolutionDetailStopNo = 1 'for now we just reset the stop number to 1
                If Me.RouteConfig.StaticRoutePlaceOnHold Then o.SolutionDetailHoldLoad = True
                Me.TrackingState = TrackingInfo.Updated
                Orders.Add(o)
                blnRet = True
            ElseIf canUseLargerTruck And TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                Dim spaceNeeded As Double = 0
                Select Case RtCapPref
                    Case RoutingCapacityPreference.Cases
                        spaceNeeded = Me.TotalCases + o.Cases
                    Case RoutingCapacityPreference.Weight
                        spaceNeeded = Me.TotalWgt + o.Wgt
                    Case RoutingCapacityPreference.Cubes
                        spaceNeeded = Me.TotalCubes + o.Cubes
                    Case Else
                        spaceNeeded = Me.TotalPlts + o.Plts
                End Select
                If TruckDataObject.moveLoadToNextLargerTruck(Me, RtCapPref, spaceNeeded) Then
                    Return Me.addOrder(o, RtCapPref)
                End If
            End If
            Return blnRet
        End Function

        ''' <summary>
        ''' this method adds
        ''' </summary>
        ''' <param name="o"></param>
        ''' <param name="trucks"></param>
        ''' <param name="RtCapPref"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function addOrderWSplit(ByRef o As tblSolutionDetail, ByVal trucks As List(Of tblSolutionTruck), Optional ByVal RtCapPref As RoutingCapacityPreference = RoutingCapacityPreference.Pallets) As Boolean
            Dim blnRet As Boolean = False
            If Me.TruckDataObject Is Nothing Then Return False
            If CanFit(CapacityType.Cases, o.Cases) _
                And CanFit(CapacityType.Weight, o.Wgt) _
                And CanFit(CapacityType.Pallets, o.Plts) _
                And CanFit(CapacityType.Cubes, o.Cubes) Then
                'The following fields get updated when we add an order
                o.SolutionDetailCarrierControl = Me.SolutionTruckCarrierControl
                o.SolutionDetailConsPrefix = Me.CNS
                Select Case Me.SolutionTruckRouteTypeCode
                    Case RouteTypeCodes.LTL_Pool
                        o.SolutionDetailRouteConsFlag = False
                    Case Else
                        o.SolutionDetailRouteConsFlag = True
                End Select
                If Me.RouteConfig.StaticRoutePlaceOnHold Then o.SolutionDetailHoldLoad = True
                Me.TrackingState = TrackingInfo.Updated
                Orders.Add(o)
                blnRet = True
            ElseIf o.Cases > Me.maxCases Or o.Wgt > Me.maxWgt Or o.Plts > Me.maxPLTs Or o.Cubes > Me.maxCubes Then
                If TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                    Dim spaceNeeded As Double = 0
                    Select Case RtCapPref
                        Case RoutingCapacityPreference.Cases
                            spaceNeeded = Me.TotalCases + o.Cases
                        Case RoutingCapacityPreference.Weight
                            spaceNeeded = Me.TotalWgt + o.Wgt
                        Case RoutingCapacityPreference.Cubes
                            spaceNeeded = Me.TotalCubes + o.Cubes
                        Case Else
                            spaceNeeded = Me.TotalPlts + o.Plts
                    End Select
                    If TruckDataObject.moveLoadToNextLargerTruck(Me, RtCapPref, spaceNeeded) Then
                        If Me.addOrder(o, RtCapPref) Then Return True
                    End If

                    Dim NewAddedOrders As New List(Of tblSolutionDetail)
                    Dim NotLoadedOrder As New tblSolutionDetail
                    If trucks Is Nothing Then trucks = New List(Of tblSolutionTruck) From {Me}
                    If splitOrder(o, trucks, RtCapPref, NewAddedOrders, NotLoadedOrder) Then
                        If Me.Orders.Contains(o) Then Me.Orders.Remove(o)
                        blnRet = True
                        If Not NotLoadedOrder Is Nothing Then
                            If Not Me.masterBuild(NotLoadedOrder, trucks, RtCapPref) Then
                                blnRet = moveToNewTruck(New List(Of tblSolutionDetail) From {NotLoadedOrder}, trucks, RtCapPref)
                            End If
                        End If
                    Else
                        blnRet = False
                    End If
                Else
                    'see if there is a larger truck
                    If TruckDataObject.doesLargerTruckExist(Me, RtCapPref) Then
                        Return moveToNewTruck(New List(Of tblSolutionDetail) From {o}, trucks, RtCapPref)
                    Else
                        Dim NewAddedOrders As New List(Of tblSolutionDetail)
                        Dim NotLoadedOrder As New tblSolutionDetail
                        If trucks Is Nothing Then trucks = New List(Of tblSolutionTruck) From {Me}
                        If splitOrder(o, trucks, RtCapPref, NewAddedOrders, NotLoadedOrder) Then
                            If Me.Orders.Contains(o) Then Me.Orders.Remove(o)
                            blnRet = True
                            If Not NotLoadedOrder Is Nothing Then
                                If Not Me.masterBuild(NotLoadedOrder, trucks, RtCapPref) Then
                                    blnRet = moveToNewTruck(New List(Of tblSolutionDetail) From {NotLoadedOrder}, trucks, RtCapPref)
                                End If
                            End If
                        Else
                            blnRet = True
                        End If
                    End If
                End If
            End If

            Return blnRet
        End Function

        Public Function addOrders(ByRef orders As List(Of tblSolutionDetail), ByRef os As clsOrderSummary, ByVal RtCapPref As RoutingCapacityPreference, Optional ByVal canUseLargerTruck As Boolean = True) As Boolean
            Dim blnRet As Boolean = False
            If TruckDataObject Is Nothing Then Return False
            If Not orders Is Nothing AndAlso orders.Count > 0 AndAlso Not os Is Nothing Then
                Dim ThisLocation As String = os.Key
                If CanFit(CapacityType.Cases, os.TotalCases) _
                    And CanFit(CapacityType.Weight, os.TotalWgt) _
                    And CanFit(CapacityType.Pallets, os.TotalPLTs) _
                    And CanFit(CapacityType.Cubes, os.TotalCubes) Then
                    Me.TrackingState = TrackingInfo.Updated
                    For Each o In orders.Where(Function(x) x.LaneLocation = ThisLocation).ToList
                        'The following fields get updated when we add an order
                        o.SolutionDetailCarrierControl = Me.SolutionTruckCarrierControl
                        o.SolutionDetailConsPrefix = Me.CNS
                        Select Case Me.SolutionTruckRouteTypeCode
                            Case RouteTypeCodes.LTL_Pool
                                o.SolutionDetailRouteConsFlag = False
                            Case RouteTypeCodes.Full_Load
                                o.SolutionDetailRouteConsFlag = True
                                If Not Me.Orders Is Nothing AndAlso Me.Orders.Count > 0 AndAlso o.LaneLocation <> Me.Orders(0).LaneLocation Then
                                    Return False
                                End If
                            Case RouteTypeCodes.Single_LTL
                                o.SolutionDetailRouteConsFlag = True
                                If Not Me.Orders Is Nothing AndAlso Me.Orders.Count > 0 Then
                                    Return False
                                End If
                            Case Else
                                o.SolutionDetailRouteConsFlag = True
                        End Select
                        If Me.RouteConfig.StaticRoutePlaceOnHold Then o.SolutionDetailHoldLoad = True
                        If Not Me.addOrder(o, RtCapPref) Then Return False
                        'modified by RHR 2/19/13 by adding the order to orders we are doing no work.  
                        'it does not add the order to the truck we now call the above line  me.addOrder....
                        'orders.Add(o)
                    Next
                    blnRet = True
                ElseIf canUseLargerTruck And TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                    Dim spaceNeeded As Double = 0
                    Select Case RtCapPref
                        Case RoutingCapacityPreference.Cases
                            spaceNeeded = Me.TotalCases + os.TotalCases
                        Case RoutingCapacityPreference.Weight
                            spaceNeeded = Me.TotalWgt + os.TotalWgt
                        Case RoutingCapacityPreference.Cubes
                            spaceNeeded = Me.TotalCubes + os.TotalCubes
                        Case Else
                            spaceNeeded = Me.TotalPlts + os.TotalPLTs
                    End Select
                    If TruckDataObject.moveLoadToNextLargerTruck(Me, RtCapPref, spaceNeeded) Then
                        Return Me.addOrders(orders, os, RtCapPref)
                    End If
                End If
            End If
            Return blnRet
        End Function

        ''' <summary>
        ''' The caller is expected to determine if this truck is qualified to master build this order
        ''' </summary>
        ''' <param name="o"></param>
        ''' <param name="trucks"></param>
        ''' <param name="RtCapPref"></param>
        ''' <param name="blnMatchSeqOnly"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function masterBuild(ByRef o As tblSolutionDetail, _
                                    ByRef trucks As List(Of tblSolutionTruck), _
                                    Optional ByVal RtCapPref As RoutingCapacityPreference = RoutingCapacityPreference.Pallets, _
                                    Optional ByVal blnMatchSeqOnly As Boolean = False) As Boolean
            Dim blnRet As Boolean = False
            Dim blnTryToLoad As Boolean = False
            Dim ThisLocation As String = o.LaneLocation
            Dim spaceNeeded As Double = 0
            Dim spaceLeft As Double = 0
            If TruckDataObject Is Nothing Then Return False

            If Me.addOrder(o, RtCapPref) Then Return True
            If Not populateSpaceValues(spaceNeeded, spaceLeft, o, RtCapPref) Then Return False
            If RtCapPref = RoutingCapacityPreference.Sequence Then
                Dim blnRetry As Boolean = True
                Do While blnRetry
                    blnRetry = False
                    If Me.MaxSequence <= o.RouteSequence Then
                        'check if this is the only lane on the truck.  this indicates that a full truck load going to the same location
                        'already exists and should not be moved.
                        Dim otherlanes = Me.Orders.Where(Function(x) x.LaneLocation <> ThisLocation)
                        If Not otherlanes Is Nothing AndAlso otherlanes.Count > 0 Then
                            'other lanes exist on this truck so it is ok to consolidate orders with the current lane
                            'and move them all to the next truck
                            Return moveToNextTruck(ThisLocation, consolidateOrders(o, ThisLocation), trucks, RtCapPref, o.RouteSequence, blnMatchSeqOnly)
                        Else
                            'there is more than one truckload of orders going to the same lane so move the current order to the next truck
                            'see if we can move the combinded load to a larger truck
                            Dim oMmoving = consolidateOrders(o, ThisLocation)
                            populateSpaceValues(spaceNeeded, spaceLeft, oMmoving, RtCapPref)
                            'If doesLargerTruckExist(Me, RtCapPref) Then
                            If TruckDataObject.doesLargerTruckExist(Me, RtCapPref, spaceNeeded) Then
                                Return moveToNewTruck(oMmoving, trucks, RtCapPref)
                            Else
                                'before we move this order to a new truck lets try to clear some space from this truck; this should optimize space
                                'but only if fill largest first is selected
                                If TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                                    populateSpaceValues(spaceNeeded, spaceLeft, o, RtCapPref)
                                    'this version of can clear space does not maintain lanes it allows some orders to be moved to the next truck
                                    Do While canClearSpaceForLargerOrder(o, trucks, spaceLeft, spaceNeeded, RtCapPref, o.RouteSequence)
                                        'we were able to clear some space so try to add the orders to the selected load
                                        If addOrder(o, RtCapPref) Then
                                            Return True 'the orders fit to return
                                        End If
                                        If Not populateSpaceValues(spaceNeeded, spaceLeft, o, RtCapPref) Then Return False
                                    Loop
                                End If
                                'if we get here the order will not fit so it must be added to a the next available or a new truck
                                Return moveToNextTruck(ThisLocation, New List(Of tblSolutionDetail) From {o}, trucks, RtCapPref, o.RouteSequence, blnMatchSeqOnly)
                            End If
                        End If
                    Else
                        'there are orders on this load with a higher sequence so try to clear some space
                        Do While canClearSpace(ThisLocation, trucks, spaceLeft, spaceNeeded, RtCapPref, o.RouteSequence)
                            'we were able to clear some space so try to add the orders to the selected load
                            If addOrder(o, RtCapPref) Then
                                Return True 'the orders fit to return
                            End If
                            If Not populateSpaceValues(spaceNeeded, spaceLeft, o, RtCapPref) Then Return False
                        Loop
                        'if we get here we need to go back to the top of the loop
                        blnRetry = True
                    End If
                Loop
                Return False
            Else
                'see if we can move one of the orders that do not match to the next truck
                If canClearSpace(ThisLocation, trucks, spaceLeft, spaceNeeded, RtCapPref) Then
                    Return addOrder(o, RtCapPref)
                Else
                    'check if this is the only lane on the truck.  this indicates that a full truck load going to the same location
                    'already exists and should not be moved.
                    Dim otherlanes = Me.Orders.Where(Function(x) x.LaneLocation <> ThisLocation)
                    If Not otherlanes Is Nothing AndAlso otherlanes.Count > 0 Then
                        'other lanes exist on this truck so it is ok to consolidate orders with the current lane
                        'and move them all to the next truck but only if we have a truck large enough to handle all of the orders.
                        'so check if a truck exists
                        Dim ordersByLane As List(Of tblSolutionDetail) = consolidateOrders(o, ThisLocation)
                        Dim laneSpaceNeeded As Double = 0
                        Dim laneSpaceLeft As Double = 0
                        populateSpaceValues(laneSpaceNeeded, laneSpaceLeft, ordersByLane, RtCapPref)
                        If TruckDataObject.doesLargerTruckExist(Me, RtCapPref, laneSpaceNeeded) Then
                            Return moveToNextTruck(ThisLocation, ordersByLane, trucks, RtCapPref, o.RouteSequence)
                        Else
                            'just move this order
                            Return moveToNextTruck(ThisLocation, New List(Of tblSolutionDetail) From {o}, trucks, RtCapPref, o.RouteSequence)
                        End If
                    Else
                        'there is more than one truckload of orders going to the same lane so move the current order to the next truck
                        Return moveToNextTruck(ThisLocation, New List(Of tblSolutionDetail) From {o}, trucks, RtCapPref, o.RouteSequence)
                    End If
                End If
            End If
            Return blnRet
        End Function

        ''' <summary>
        ''' This overloaded method master builds a list of orders together.
        ''' The caller is expected to determine if this truck is qualified to master build these orders
        ''' Each list of orders must belong to the same lane.
        ''' </summary>
        ''' <param name="newOrders"></param>
        ''' <param name="trucks"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function masterBuild(ByRef newOrders As List(Of tblSolutionDetail), _
                                    ByRef trucks As List(Of tblSolutionTruck), _
                                    Optional ByVal RtCapPref As RoutingCapacityPreference = RoutingCapacityPreference.Pallets) As Boolean
            Dim blnRet As Boolean = False
            Dim ThisLocation As String = ""
            Dim spaceNeeded As Double = 0
            Dim spaceLeft As Double = 0

            If Me.Orders Is Nothing OrElse Me.Orders.Count < 1 Then
                ThisLocation = newOrders(0).LaneLocation
            Else
                ThisLocation = Me.Orders(0).LaneLocation
            End If

            'ensure we have the orders with the same lane number all others are ignored
            Dim OrderSummary As clsOrderSummary = getOrderSummary(newOrders, ThisLocation)

            If OrderSummary Is Nothing Then Return False

            'try to add the orders to the truck
            If addOrders(newOrders.Where(Function(x) x.LaneLocation = ThisLocation).ToList, OrderSummary, RtCapPref) Then Return True

            If Not populateSpaceValues(spaceNeeded, spaceLeft, OrderSummary, RtCapPref) Then Return False
            If RtCapPref = RoutingCapacityPreference.Sequence Then
                Dim blnRetry As Boolean = True
                Do While blnRetry
                    blnRetry = False
                    If Me.MaxSequence <= OrderSummary.MaxSeq Then
                        'check if this is the only lane on the truck.  this indicates that a full truck load going to the same location
                        'already exists and should not be moved.
                        Dim otherlanes = Me.Orders.Where(Function(x) x.LaneLocation <> ThisLocation)
                        If Not otherlanes Is Nothing AndAlso otherlanes.Count > 0 Then
                            'other lanes exist on this truck so it is ok to consolidate orders with the current lane
                            'and move them all to the next truck
                            Return moveToNextTruck(ThisLocation, consolidateOrders(newOrders, ThisLocation), trucks, RtCapPref, OrderSummary.MaxSeq)
                        Else
                            'there is more than one truckload of orders going to the same lane so move the new orders to the next truck
                            Return moveToNextTruck(ThisLocation, newOrders, trucks, RtCapPref, OrderSummary.MaxSeq)
                        End If
                    Else
                        'there are orders on this load with a higher sequence so try to clear some space
                        Do While canClearSpace(ThisLocation, trucks, spaceLeft, spaceNeeded, RtCapPref, OrderSummary.MaxSeq)
                            'we were able to clear some space so try to add the orders to the selected load
                            If addOrders(newOrders.Where(Function(x) x.LaneLocation = ThisLocation).ToList, OrderSummary, RtCapPref) Then
                                Return True 'the orders fit to return
                            End If
                        Loop
                        'if we get here we need to go back to the top of the loop
                        blnRetry = True
                    End If
                Loop
                Return False
            Else
                'see if we can move one of the orders that do not match to the next truck
                If canClearSpace(ThisLocation, trucks, spaceLeft, spaceNeeded, RtCapPref) Then
                    Return addOrders(newOrders.Where(Function(x) x.LaneLocation = ThisLocation).ToList, OrderSummary, RtCapPref)
                Else
                    'check if this is the only lane on the truck.  this indicates that a full truck load going to the same location
                    'already exists and should not be moved.
                    Dim otherlanes = Me.Orders.Where(Function(x) x.LaneLocation <> ThisLocation)
                    If Not otherlanes Is Nothing AndAlso otherlanes.Count > 0 Then
                        'other lanes exist on this truck so it is ok to consolidate orders with the current lane
                        'and move them all to the next truck
                        Return moveToNextTruck(ThisLocation, consolidateOrders(newOrders, ThisLocation), trucks, RtCapPref, OrderSummary.MaxSeq)
                    Else
                        'there is more than one truckload of orders going to the same lane so move the new orders to the next truck
                        Return moveToNextTruck(ThisLocation, newOrders, trucks, RtCapPref, OrderSummary.MaxSeq)
                    End If
                End If
            End If
            Return blnRet
        End Function

        Public Function populateSpaceValues(ByRef spaceNeeded As Double, ByRef spaceLeft As Double, ByRef OrderSummary As clsOrderSummary, ByVal RtCapPref As RoutingCapacityPreference) As Boolean
            Dim blnRet As Boolean = True
            Select Case RtCapPref
                Case RoutingCapacityPreference.Cases
                    spaceNeeded = OrderSummary.TotalCases
                    spaceLeft = maxCases - TotalCases
                Case RoutingCapacityPreference.Weight
                    spaceNeeded = OrderSummary.TotalWgt
                    spaceLeft = maxWgt - TotalWgt
                Case RoutingCapacityPreference.Pallets
                    spaceNeeded = OrderSummary.TotalPLTs
                    spaceLeft = maxPLTs - TotalPlts
                Case RoutingCapacityPreference.Cubes
                    spaceNeeded = OrderSummary.TotalCubes
                    spaceLeft = maxCubes - TotalCubes
                Case RoutingCapacityPreference.Sequence
                    'for sequence number routing we typically do not use space values but 
                    'when required we use the pallet count
                    spaceNeeded = OrderSummary.TotalPLTs
                    spaceLeft = maxPLTs - TotalPlts
                Case Else
                    blnRet = False
            End Select
            Return blnRet
        End Function

        Public Function populateSpaceValues(ByRef spaceNeeded As Double, ByRef spaceLeft As Double, ByRef o As tblSolutionDetail, ByVal RtCapPref As RoutingCapacityPreference) As Boolean
            Dim blnRet As Boolean = True
            Select Case RtCapPref
                Case RoutingCapacityPreference.Cases
                    spaceNeeded = o.Cases
                    spaceLeft = maxCases - TotalCases
                Case RoutingCapacityPreference.Weight
                    spaceNeeded = o.Wgt
                    spaceLeft = maxWgt - TotalWgt
                Case RoutingCapacityPreference.Pallets
                    spaceNeeded = o.Plts
                    spaceLeft = maxPLTs - TotalPlts
                Case RoutingCapacityPreference.Cubes
                    spaceNeeded = o.Cubes
                    spaceLeft = maxCubes - TotalCubes
                Case RoutingCapacityPreference.Sequence
                    'for sequence number routing we typically do not use space values but 
                    'when required we use the pallet count
                    spaceNeeded = o.Plts
                    spaceLeft = maxPLTs - TotalPlts
                Case Else
                    blnRet = False
            End Select
            Return blnRet
        End Function

        Public Function populateSpaceValues(ByRef spaceNeeded As Double, ByRef spaceLeft As Double, ByRef l As List(Of tblSolutionDetail), ByVal RtCapPref As RoutingCapacityPreference) As Boolean
            Dim blnRet As Boolean = True
            Select Case RtCapPref
                Case RoutingCapacityPreference.Cases
                    spaceNeeded = l.Sum(Function(x) x.Cases)
                    spaceLeft = maxCases - TotalCases
                Case RoutingCapacityPreference.Weight
                    spaceNeeded = l.Sum(Function(x) x.Wgt)
                    spaceLeft = maxWgt - TotalWgt
                Case RoutingCapacityPreference.Cubes
                    spaceNeeded = l.Sum(Function(x) x.Cubes)
                    spaceLeft = maxCubes - TotalCubes
                Case Else
                    'always use pallets by default
                    spaceNeeded = l.Sum(Function(x) x.Plts)
                    spaceLeft = maxPLTs - TotalPlts
            End Select
            Return blnRet
        End Function

        Public Function getOrderSummary(ByRef newOrders As List(Of tblSolutionDetail), ByVal LaneLocation As String) As clsOrderSummary
            Return (getOrderSummaryList(newOrders).Where(Function(x) x.Key = LaneLocation)).FirstOrDefault
        End Function

        Public Function getOrderSummaryList(ByRef newOrders As List(Of tblSolutionDetail)) As List(Of clsOrderSummary)
            Return (newOrders.GroupBy(Function(l) l.LaneLocation).Select(Function(lg) New clsOrderSummary With { _
                                                                                  .Count = lg.Count, _
                                                                                  .Key = lg.Key, _
                                                                                  .TotalPLTs = lg.Sum(Function(w) w.Plts), _
                                                                                  .TotalCases = lg.Sum(Function(w) w.Cases), _
                                                                                  .TotalWgt = lg.Sum(Function(w) w.Wgt), _
                                                                                  .TotalCubes = lg.Sum(Function(w) w.Cubes), _
                                                                                  .MaxSeq = lg.Max(Function(w) w.RouteSequence), _
                                                                                  .LaneLocation = lg.Key})).ToList
        End Function

        ''' <summary>
        ''' This overloaded method returns a clsOrderSummary record for each order on the truck
        ''' primarily used for splitting of loads and multiple destination truck optimization
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getOrderSummaryList(ByVal Control As Integer) As List(Of clsOrderSummary)
            Return (Me.Orders.Where(Function(w) w.TempControl <> Control).Select(Function(lg) New clsOrderSummary With { _
                                                                                  .Count = 1, _
                                                                                  .Key = lg.TempControl, _
                                                                                  .TotalPLTs = lg.Plts, _
                                                                                  .TotalCases = lg.Cases, _
                                                                                  .TotalWgt = lg.Wgt, _
                                                                                  .TotalCubes = lg.Cubes, _
                                                                                  .MaxSeq = lg.RouteSequence, _
                                                                                  .Control = lg.TempControl, _
                                                                                  .LaneLocation = lg.LaneLocation})).ToList
        End Function

        ''' <summary>
        ''' This overloaded method does not filter by lane number so it returns all 
        ''' orders where space is possible.  This is used primarily when optimizing 
        ''' new trucks for the same destination
        ''' </summary>
        ''' <param name="SpaceLeft"></param>
        ''' <param name="SpaceNeeded"></param>
        ''' <param name="RtCapPref"></param>
        ''' <param name="intSeq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getOrderSummaryListToClear(ByVal Control As Integer, _
                                                   ByVal SpaceLeft As Double, _
                                                   ByVal SpaceNeeded As Double, _
                                                   ByVal RtCapPref As RoutingCapacityPreference, _
                                                   ByVal intSeq As Integer) As List(Of clsOrderSummary)

            Select Case RtCapPref
                Case RoutingCapacityPreference.Cases
                    Return getOrderSummaryList(Control).Where(Function(w) w.TotalCases + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalCases).ToList
                Case RoutingCapacityPreference.Weight
                    Return getOrderSummaryList(Control).Where(Function(w) w.TotalWgt + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalWgt).ToList
                Case RoutingCapacityPreference.Pallets
                    Return getOrderSummaryList(Control).Where(Function(w) w.TotalPLTs + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalPLTs).ToList
                Case RoutingCapacityPreference.Cubes
                    Return getOrderSummaryList(Control).Where(Function(w) w.TotalCubes + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalCubes).ToList
                Case RoutingCapacityPreference.Sequence
                    'When routing by sequence we only get one order at a time 
                    'This may not work as expected when splitting orders accross multiple trucks for now we just use pallets
                    Return getOrderSummaryList(Control).Where(Function(w) w.TotalPLTs + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalPLTs).ToList
                    'Return New List(Of clsOrderSummary) From {getOrderSummaryList().Where(Function(f) f.MaxSeq > intSeq).OrderByDescending(Function(x) x.MaxSeq).FirstOrDefault}
                Case Else
                    Return New List(Of clsOrderSummary)
            End Select

        End Function

        ''' <summary>
        ''' Primarily used to optimze space for larger orders even when they
        ''' are being delivered to the same location 
        ''' </summary>
        ''' <param name="o"></param>
        ''' <param name="RtCapPref"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getOrderSummaryList(ByRef o As tblSolutionDetail, ByVal RtCapPref As RoutingCapacityPreference) As List(Of clsOrderSummary)

            Dim Control As Integer = o.TempControl
            Dim largerCapacity As Double = 0

            Select Case RtCapPref
                Case RoutingCapacityPreference.Cases
                    largerCapacity = o.Cases
                    Return (Me.Orders.Where(Function(w) w.TempControl <> Control And w.Cases < largerCapacity).Select(Function(lg) New clsOrderSummary With { _
                                                                                   .Count = 1, _
                                                                                   .Key = lg.TempControl, _
                                                                                   .TotalPLTs = lg.Plts, _
                                                                                   .TotalCases = lg.Cases, _
                                                                                   .TotalWgt = lg.Wgt, _
                                                                                   .TotalCubes = lg.Cubes, _
                                                                                   .MaxSeq = lg.RouteSequence, _
                                                                                   .Control = lg.TempControl, _
                                                                                   .LaneLocation = lg.LaneLocation})).ToList
                Case RoutingCapacityPreference.Weight
                    largerCapacity = o.Wgt
                    Return (Me.Orders.Where(Function(w) w.TempControl <> Control And w.Wgt < largerCapacity).Select(Function(lg) New clsOrderSummary With { _
                                                                                   .Count = 1, _
                                                                                   .Key = lg.TempControl, _
                                                                                   .TotalPLTs = lg.Plts, _
                                                                                   .TotalCases = lg.Cases, _
                                                                                   .TotalWgt = lg.Wgt, _
                                                                                   .TotalCubes = lg.Cubes, _
                                                                                   .MaxSeq = lg.RouteSequence, _
                                                                                   .Control = lg.TempControl, _
                                                                                   .LaneLocation = lg.LaneLocation})).ToList
                Case RoutingCapacityPreference.Cubes
                    largerCapacity = o.Cubes
                    Return (Me.Orders.Where(Function(w) w.TempControl <> Control And w.Cubes < largerCapacity).Select(Function(lg) New clsOrderSummary With { _
                                                                                   .Count = 1, _
                                                                                   .Key = lg.TempControl, _
                                                                                   .TotalPLTs = lg.Plts, _
                                                                                   .TotalCases = lg.Cases, _
                                                                                   .TotalWgt = lg.Wgt, _
                                                                                   .TotalCubes = lg.Cubes, _
                                                                                   .MaxSeq = lg.RouteSequence, _
                                                                                   .Control = lg.TempControl, _
                                                                                   .LaneLocation = lg.LaneLocation})).ToList
                Case Else
                    largerCapacity = o.Plts
                    Return (Me.Orders.Where(Function(w) w.TempControl <> Control And w.Plts < largerCapacity).Select(Function(lg) New clsOrderSummary With { _
                                                                                   .Count = 1, _
                                                                                   .Key = lg.TempControl, _
                                                                                   .TotalPLTs = lg.Plts, _
                                                                                   .TotalCases = lg.Cases, _
                                                                                   .TotalWgt = lg.Wgt, _
                                                                                   .TotalCubes = lg.Cubes, _
                                                                                   .MaxSeq = lg.RouteSequence, _
                                                                                   .Control = lg.TempControl, _
                                                                                   .LaneLocation = lg.LaneLocation})).ToList
            End Select

        End Function

        Public Function getOrderSummaryListToClear(ByVal LaneLocation As String, _
                                     ByVal SpaceLeft As Double, _
                                     ByVal SpaceNeeded As Double, _
                                     ByVal RtCapPref As RoutingCapacityPreference, _
                                     ByVal intSeq As Integer) As List(Of clsOrderSummary)

            Select Case RtCapPref
                Case RoutingCapacityPreference.Cases
                    Return getOrderSummaryListNotLane(LaneLocation).Where(Function(w) w.TotalCases + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalCases).ToList
                Case RoutingCapacityPreference.Weight
                    Return getOrderSummaryListNotLane(LaneLocation).Where(Function(w) w.TotalWgt + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalWgt).ToList
                Case RoutingCapacityPreference.Pallets
                    Return getOrderSummaryListNotLane(LaneLocation).Where(Function(w) w.TotalPLTs + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalPLTs).ToList
                Case RoutingCapacityPreference.Cubes
                    Return getOrderSummaryListNotLane(LaneLocation).Where(Function(w) w.TotalCubes + SpaceLeft >= SpaceNeeded).OrderBy(Function(w) w.TotalCubes).ToList
                Case RoutingCapacityPreference.Sequence
                    'When routing by sequence we only get one order at a time 
                    Return New List(Of clsOrderSummary) From {getOrderSummaryListNotLane(LaneLocation).Where(Function(f) f.MaxSeq > intSeq).OrderByDescending(Function(x) x.MaxSeq).FirstOrDefault}
                Case Else
                    Return New List(Of clsOrderSummary)
            End Select

        End Function

        Public Function getOrderSummaryListNotLane(ByVal ThisLocation As String) As List(Of clsOrderSummary)

            Return (Orders.Where(Function(w) w.LaneLocation <> ThisLocation).GroupBy(Function(l) l.LaneLocation).Select(Function(lg) New clsOrderSummary With { _
                                                                                  .Count = lg.Count, _
                                                                                  .Key = lg.Key, _
                                                                                  .TotalPLTs = lg.Sum(Function(w) w.Plts), _
                                                                                  .TotalCases = lg.Sum(Function(w) w.Cases), _
                                                                                  .TotalWgt = lg.Sum(Function(w) w.Wgt), _
                                                                                  .TotalCubes = lg.Sum(Function(w) w.Cubes), _
                                                                                  .MaxSeq = lg.Max(Function(w) w.RouteSequence)})).ToList

        End Function

        Public Function getOrderSummaryListByLane(ByVal ThisLocation As String) As List(Of clsOrderSummary)

            Return (Orders.Where(Function(w) w.LaneLocation = ThisLocation).GroupBy(Function(l) l.LaneLocation).Select(Function(lg) New clsOrderSummary With { _
                                                                                  .Count = lg.Count, _
                                                                                  .Key = lg.Key, _
                                                                                  .TotalPLTs = lg.Sum(Function(w) w.Plts), _
                                                                                  .TotalCases = lg.Sum(Function(w) w.Cases), _
                                                                                  .TotalWgt = lg.Sum(Function(w) w.Wgt), _
                                                                                  .TotalCubes = lg.Sum(Function(w) w.Cubes), _
                                                                                  .MaxSeq = lg.Max(Function(w) w.RouteSequence)})).ToList

        End Function

        Public Function consolidateOrders(ByRef newOrders As List(Of tblSolutionDetail), ByVal ThisLocation As String) As List(Of tblSolutionDetail)
            For Each o In Orders.Where(Function(x) x.LaneLocation = ThisLocation).ToList
                If Not newOrders.Contains(o) Then newOrders.Add(o)
            Next
            Return newOrders
        End Function

        Public Function consolidateOrders(ByRef order As tblSolutionDetail, ByVal ThisLocation As String) As List(Of tblSolutionDetail)
            Dim newOrders As New List(Of tblSolutionDetail) From {order}
            For Each o In Orders.Where(Function(x) x.LaneLocation = ThisLocation).ToList
                If Not newOrders.Contains(o) Then newOrders.Add(o)
            Next
            Return newOrders
        End Function

        Public Function ordersToMove(ByVal ThisLocation As String) As List(Of tblSolutionDetail)
            Return Orders.Where(Function(x) x.LaneLocation = ThisLocation).ToList
        End Function

        Public Function ordersToMove(ByVal control As Integer) As List(Of tblSolutionDetail)
            Return Orders.Where(Function(x) x.TempControl = control).ToList
        End Function

        Public Function canClearSpace(ByVal LaneLocation As String, _
                                     ByRef trucks As List(Of tblSolutionTruck), _
                                     ByVal SpaceLeft As Double, _
                                     ByVal SpaceNeeded As Double, _
                                     Optional ByVal RtCapPref As RoutingCapacityPreference = RoutingCapacityPreference.Pallets, _
                                     Optional ByVal intSeq As Integer = 0) As Boolean
            Dim blnRet As Boolean = False
            Dim OrderSummary = getOrderSummaryListToClear(LaneLocation, SpaceLeft, SpaceNeeded, RtCapPref, intSeq)
            If OrderSummary Is Nothing OrElse OrderSummary.Count < 1 Then Return False

            For Each os In OrderSummary
                If Not os Is Nothing Then
                    If moveToNextTruck(os.Key, ordersToMove(os.Key), trucks, RtCapPref, os.MaxSeq) Then
                        Return True
                    End If
                End If
            Next

            Return blnRet
        End Function

        Public Function canClearSpace(ByVal Control As Integer, _
                                      ByRef trucks As List(Of tblSolutionTruck), _
                                     ByVal SpaceLeft As Double, _
                                     ByVal SpaceNeeded As Double, _
                                     Optional ByVal RtCapPref As RoutingCapacityPreference = RoutingCapacityPreference.Pallets, _
                                     Optional ByVal intSeq As Integer = 0) As Boolean
            Dim blnRet As Boolean = False
            Dim OrderSummary = getOrderSummaryListToClear(Control, SpaceLeft, SpaceNeeded, RtCapPref, intSeq)
            If OrderSummary Is Nothing OrElse OrderSummary.Count < 1 Then Return False
            'when clearing space by control number we ignore grouping by lane. this is primarily used
            'to split large orders across multiple trucks where they all have the same lane.
            For Each os In OrderSummary
                If Not os Is Nothing Then
                    'see if we can move the first order/item to the next truck, this is used to optimize space
                    If moveToNextTruck(os.LaneLocation, ordersToMove(os.Control), trucks, RtCapPref, os.MaxSeq) Then
                        Return True
                    End If
                End If
            Next

            Return blnRet
        End Function

        ''' <summary>
        ''' This method is used to optimize space by moving 
        ''' smaller orders to other trucks even if they are going to 
        ''' the same location.
        ''' </summary>
        ''' <param name="o"></param>
        ''' <param name="trucks"></param>
        ''' <param name="SpaceLeft"></param>
        ''' <param name="SpaceNeeded"></param>
        ''' <param name="RtCapPref"></param>
        ''' <param name="intSeq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function canClearSpaceForLargerOrder(ByRef o As tblSolutionDetail, _
                                      ByRef trucks As List(Of tblSolutionTruck), _
                                     ByVal SpaceLeft As Double, _
                                     ByVal SpaceNeeded As Double, _
                                     Optional ByVal RtCapPref As RoutingCapacityPreference = RoutingCapacityPreference.Pallets, _
                                     Optional ByVal intSeq As Integer = 0) As Boolean
            Dim blnRet As Boolean = False
            Dim OrderSummary = getOrderSummaryList(o, RtCapPref)
            If OrderSummary Is Nothing OrElse OrderSummary.Count < 1 Then Return False
            'when clearing space for large orders we ignore grouping by lane. this is primarily used
            'to optimize capacity for multiple orders going to the same locations when multiple trucks are needed
            For Each os In OrderSummary
                If Not os Is Nothing Then
                    'see if we can move the first order/item to the next truck, this is used to optimize space
                    If moveToNextTruck(os.LaneLocation, ordersToMove(os.Control), trucks, RtCapPref, os.MaxSeq) Then
                        Return True
                    End If
                End If
            Next

            Return blnRet
        End Function

        Public Function moveToNextTruck(ByVal LaneLocation As String, _
                                        ByRef items As List(Of tblSolutionDetail), _
                                        ByRef trucks As List(Of tblSolutionTruck), _
                                        Optional ByVal RtCapPref As RoutingCapacityPreference = RoutingCapacityPreference.Pallets, _
                                        Optional ByVal intSeq As Integer = 0, _
                                        Optional ByVal blnMatchSeqOnly As Boolean = False) As Boolean
            Dim blnRet As Boolean = False
            Dim blnOrdersMoved As Boolean = False

            Dim AddedOrders As New List(Of tblSolutionDetail)
            Dim spaceNeeded As Double = 0
            Dim maxSpaceAvailable As Double = 0
            Select Case RtCapPref
                Case RoutingCapacityPreference.Cases
                    spaceNeeded = items.Sum(Function(x) x.Cases)
                    maxSpaceAvailable = Me.maxCases
                Case RoutingCapacityPreference.Weight
                    spaceNeeded = items.Sum(Function(x) x.Wgt)
                    maxSpaceAvailable = Me.maxWgt
                Case RoutingCapacityPreference.Pallets
                    spaceNeeded = items.Sum(Function(x) x.Plts)
                    maxSpaceAvailable = Me.maxPLTs
                Case RoutingCapacityPreference.Cubes
                    spaceNeeded = items.Sum(Function(x) x.Cubes)
                    maxSpaceAvailable = Me.maxCubes
                Case RoutingCapacityPreference.Sequence
                    'when routing by sequence number we use pallets by default
                    spaceNeeded = items.Sum(Function(x) x.Plts)
                    maxSpaceAvailable = Me.maxPLTs
                Case Else
                    Return False
            End Select
            Dim spaceLeft As Double = 0
            Dim currentCNS As String = Me.CNS

            Dim existingTrucks As New List(Of tblSolutionTruck)
            'first see if any truck already has orders with this LaneLocation.
            existingTrucks = trucks.Where(Function(p) p.CNS <> currentCNS And p.Orders.Any(Function(y) y.LaneLocation = LaneLocation)).ToList

            If Not existingTrucks Is Nothing AndAlso existingTrucks.Count > 0 Then
                'try to move the orders to one of these trucks
                'see if we can move all of the orders that do not match to the next truck
                For Each t In existingTrucks
                    'see if we can move all of the orders that do not match to the next truck
                    Select Case RtCapPref
                        Case RoutingCapacityPreference.Cases
                            spaceLeft = t.maxCases - t.TotalCases
                            spaceNeeded = items.Sum(Function(x) x.Cases) + t.TotalCases
                        Case RoutingCapacityPreference.Weight
                            spaceLeft = t.maxWgt - t.TotalWgt
                            spaceNeeded = items.Sum(Function(x) x.Wgt) + t.TotalWgt
                        Case RoutingCapacityPreference.Cubes
                            spaceLeft = t.maxCubes - t.TotalCubes
                            spaceNeeded = items.Sum(Function(x) x.Cubes) + t.TotalCubes
                        Case RoutingCapacityPreference.Sequence
                            spaceLeft = t.maxPLTs - t.TotalPlts
                        Case Else
                            spaceLeft = t.maxPLTs - t.TotalPlts
                            spaceNeeded = items.Sum(Function(x) x.Plts) + t.TotalPlts
                    End Select
                    If RtCapPref = RoutingCapacityPreference.Sequence Then
                        blnOrdersMoved = True
                        For Each o In items
                            'if routing by sequence number we must make the order fit
                            Do Until t.addOrder(o, RtCapPref)
                                If Not t.canClearSpace(LaneLocation, trucks, spaceLeft, spaceNeeded, RtCapPref, intSeq) Then
                                    blnOrdersMoved = False
                                    Exit For
                                End If
                            Loop
                            AddedOrders.Add(o)
                        Next
                        If blnOrdersMoved Then
                            For Each r In AddedOrders
                                If Me.Orders.Contains(r) Then
                                    Me.Orders.Remove(r)
                                    Me.TrackingState = TrackingInfo.Updated
                                End If
                            Next
                            Return True
                        Else
                            For Each r In AddedOrders
                                If t.Orders.Contains(r) Then
                                    t.Orders.Remove(r)
                                    t.TrackingState = TrackingInfo.Updated
                                End If
                            Next
                        End If
                    Else
                        If spaceLeft >= spaceNeeded Then
                            blnOrdersMoved = True
                            For Each o In items
                                If t.addOrder(o, RtCapPref) Then
                                    AddedOrders.Add(o)
                                Else
                                    blnOrdersMoved = False
                                    Exit For
                                End If
                            Next
                            If blnOrdersMoved Then
                                For Each r In AddedOrders
                                    If Me.Orders.Contains(r) Then
                                        Me.Orders.Remove(r)
                                        Me.TrackingState = TrackingInfo.Updated
                                    End If
                                Next
                                Return True
                            Else
                                'If all the orders do not fit remove them from the selected truck t
                                For Each r In AddedOrders
                                    If t.Orders.Contains(r) Then
                                        t.Orders.Remove(r)
                                        t.TrackingState = TrackingInfo.Updated
                                    End If
                                Next
                            End If
                        ElseIf t.canClearSpace(LaneLocation, trucks, spaceLeft, spaceNeeded, RtCapPref, intSeq) Then
                            blnOrdersMoved = True
                            For Each o In items
                                If t.addOrder(o, RtCapPref) Then
                                    AddedOrders.Add(o)
                                Else
                                    blnOrdersMoved = False
                                    Exit For
                                End If
                            Next
                            If blnOrdersMoved Then
                                For Each r In AddedOrders
                                    If Me.Orders.Contains(r) Then
                                        Me.Orders.Remove(r)
                                        Me.TrackingState = TrackingInfo.Updated
                                    End If
                                Next
                                Return True
                                Exit For
                            Else
                                For Each r In AddedOrders
                                    If t.Orders.Contains(r) Then
                                        t.Orders.Remove(r)
                                        t.TrackingState = TrackingInfo.Updated
                                    End If
                                Next
                            End If
                        Else
                            'see if we can move this to a larger truck
                            If TruckDataObject.moveLoadToNextLargerTruck(t, RtCapPref, spaceNeeded) Then
                                blnOrdersMoved = True
                                For Each o In items
                                    If t.addOrder(o, RtCapPref) Then
                                        AddedOrders.Add(o)
                                    Else
                                        blnOrdersMoved = False
                                        Exit For
                                    End If
                                Next
                                If blnOrdersMoved Then
                                    For Each r In AddedOrders
                                        If Me.Orders.Contains(r) Then
                                            Me.Orders.Remove(r)
                                            Me.TrackingState = TrackingInfo.Updated
                                        End If
                                    Next
                                    Return True
                                Else
                                    'If all the orders do not fit remove them from the selected truck t
                                    For Each r In AddedOrders
                                        If t.Orders.Contains(r) Then
                                            t.Orders.Remove(r)
                                            t.TrackingState = TrackingInfo.Updated
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
                Next
                If Not blnOrdersMoved Then
                    Return moveToNewTruck(items, trucks, RtCapPref)
                End If
            End If
            'if we get here we could not move the data to an existing truck so try the next truck
            If RtCapPref = RoutingCapacityPreference.Sequence Then
                'get the next truck sorted by capacity preference this will get the truck with the most capacity 
                'available first in the case where there are multiple trucks with the same sequence number.
                Dim nextTruck As tblSolutionTruck
                Select Case RtCapPref
                    Case RoutingCapacityPreference.Cases
                        nextTruck = (From t In trucks Where t.MinSequence >= intSeq And t.CNS <> Me.CNS Order By t.MinSequence, t.TotalCases Select t).FirstOrDefault
                    Case RoutingCapacityPreference.Weight
                        nextTruck = (From t In trucks Where t.MinSequence >= intSeq And t.CNS <> Me.CNS Order By t.MinSequence, t.TotalWgt Select t).FirstOrDefault
                    Case RoutingCapacityPreference.Cubes
                        nextTruck = (From t In trucks Where t.MinSequence >= intSeq And t.CNS <> Me.CNS Order By t.MinSequence, t.TotalCubes Select t).FirstOrDefault
                    Case Else
                        nextTruck = (From t In trucks Where t.MinSequence >= intSeq And t.CNS <> Me.CNS Order By t.MinSequence, t.TotalPlts Select t).FirstOrDefault
                End Select
                'changing the sort order like below simply move orders arround on previous builds and does not always solve the problem
                'to make this work we may need to find a way to loop through all options and see if it can fit but this is a major performance
                'problem.
                'Dim nextTruck = (From t In trucks Where t.MinSequence > intSeq Order By t.MinSequence, t.TotalPlts Select t).FirstOrDefault
                'Dim nextTrucks = (From t In trucks Where t.MinSequence > intSeq Order By t.MinSequence, t.TotalPlts Select t).ToList

                If Not nextTruck Is Nothing Then
                    'if the blnMatchOnly flag is true we only move to the next truck if it has this sequence already
                    If blnMatchSeqOnly And nextTruck.MinSequence <> intSeq Then Return False
                    spaceLeft = nextTruck.maxPLTs - nextTruck.TotalPlts
                    blnOrdersMoved = True
                    For Each o In items
                        'if routing by sequence number we must make the order fit
                        Do Until nextTruck.addOrder(o, RtCapPref)
                            If Not nextTruck.canClearSpace(LaneLocation, trucks, spaceLeft, spaceNeeded, RtCapPref, intSeq) Then
                                blnOrdersMoved = False
                                Exit For
                            End If
                        Loop
                        AddedOrders.Add(o)
                    Next
                    If blnOrdersMoved Then
                        For Each r In AddedOrders
                            If Me.Orders.Contains(r) Then
                                Me.Orders.Remove(r)
                                Me.TrackingState = TrackingInfo.Updated
                            End If
                        Next
                        Return True
                    Else
                        For Each r In AddedOrders
                            If nextTruck.Orders.Contains(r) Then
                                nextTruck.Orders.Remove(r)
                                nextTruck.TrackingState = TrackingInfo.Updated
                            End If
                        Next
                        If blnMatchSeqOnly Then Return False
                        Return moveToNewTruck(items, trucks, RtCapPref)
                    End If
                Else
                    If blnMatchSeqOnly Then Return False 'we do not move to a new truck if the match seq only flag is true
                    Return moveToNewTruck(items, trucks, RtCapPref)
                End If
            Else
                Dim availableTrucks As List(Of tblSolutionTruck) = getAvailableTrucks(trucks, RtCapPref, currentCNS, spaceNeeded)
                If Not availableTrucks Is Nothing AndAlso availableTrucks.Count > 0 Then
                    For Each t In availableTrucks
                        Dim blnCanUse As Boolean = False
                        Select Case RtCapPref
                            Case RoutingCapacityPreference.Cases
                                If (t.maxCases - t.TotalCases) >= spaceNeeded Then
                                    blnCanUse = True
                                ElseIf TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                                    'see if we can move this load to a larger piece of equipment
                                    If TruckDataObject.moveLoadToNextLargerTruck(t, RtCapPref, spaceNeeded) Then blnCanUse = True
                                End If
                            Case RoutingCapacityPreference.Weight
                                If (t.maxWgt - t.TotalWgt) >= spaceNeeded Then
                                    blnCanUse = True
                                ElseIf TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                                    'see if we can move this load to a larger piece of equipment
                                    If TruckDataObject.moveLoadToNextLargerTruck(t, RtCapPref, spaceNeeded) Then blnCanUse = True
                                End If
                            Case RoutingCapacityPreference.Cubes
                                If (t.maxCubes - t.TotalCubes) >= spaceNeeded Then
                                    blnCanUse = True
                                ElseIf TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                                    'see if we can move this load to a larger piece of equipment
                                    If TruckDataObject.moveLoadToNextLargerTruck(t, RtCapPref, spaceNeeded) Then blnCanUse = True
                                End If
                            Case Else
                                'the default is to use pallets
                                If (t.maxPLTs - t.TotalPlts) >= spaceNeeded Then
                                    blnCanUse = True
                                ElseIf TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                                    'see if we can move this load to a larger piece of equipment
                                    If TruckDataObject.moveLoadToNextLargerTruck(t, RtCapPref, spaceNeeded) Then blnCanUse = True
                                End If
                        End Select
                        If blnCanUse Then
                            blnOrdersMoved = True
                            For Each o In items
                                If t.addOrder(o, RtCapPref) Then
                                    AddedOrders.Add(o)
                                Else
                                    blnOrdersMoved = False
                                    For Each r In AddedOrders
                                        If t.Orders.Contains(r) Then
                                            t.Orders.Remove(r)
                                            t.TrackingState = TrackingInfo.Updated
                                        End If
                                    Next
                                    Exit For
                                End If
                            Next
                            If blnOrdersMoved Then
                                For Each r In AddedOrders
                                    If Me.Orders.Contains(r) Then
                                        Me.Orders.Remove(r)
                                        Me.TrackingState = TrackingInfo.Updated
                                    End If
                                Next
                                Return True
                                Exit For
                            End If
                        End If
                    Next
                    'if we get here no trucks are available so create a new truck
                    Return moveToNewTruck(items, trucks, RtCapPref)
                Else
                    Return moveToNewTruck(items, trucks, RtCapPref)
                End If
            End If

            Return blnRet
        End Function

        Private Function getAvailableTrucks(ByRef trucks As List(Of tblSolutionTruck), _
                                        ByVal RtCapPref As RoutingCapacityPreference, _
                                        ByVal currentCNS As String,
                                        ByVal spaceNeeded As Double) As List(Of tblSolutionTruck)
            If TruckDataObject Is Nothing Then Return Nothing
            Dim availableTrucks As List(Of tblSolutionTruck)
            If TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst Then
                availableTrucks = (From t In trucks Where t.CNS <> currentCNS Order By t.maxWgt Descending).ToList
            Else
                availableTrucks = (From t In trucks Where t.CNS <> currentCNS Order By t.maxWgt).ToList
            End If
            Return availableTrucks
        End Function

        Private Function moveToNewTruck(ByRef items As List(Of tblSolutionDetail), _
                                        ByRef trucks As List(Of tblSolutionTruck), _
                                        ByVal RtCapPref As RoutingCapacityPreference) As Boolean
            Dim blnRet As Boolean = False
            If TruckDataObject Is Nothing Then Return False
            Dim spaceNeeded As Double = 0
            Dim spaceLeft As Double = 0
            Dim blnOrderAdded As Boolean = False
            Dim notloadeditems As New List(Of tblSolutionDetail)
            Dim AddedOrders As New List(Of tblSolutionDetail)
            Dim OrdersToSplit As New List(Of tblSolutionDetail)
            'this version of the getNewTruck gets the smallest possible equipment that will hold the list of items (orders)
            Dim t As tblSolutionTruck = TruckDataObject.getNewTruck(RtCapPref, items, BatchProcessingDataObject, BookItemDataObject)

            'try to add the items to the truck
            For Each o In items
                Dim blnNeedToSplit As Boolean = False
                Do While blnNeedToSplit = False And (o.Cases > t.maxCases Or o.Wgt > t.maxWgt Or o.Plts > t.maxPLTs Or o.Cubes > t.maxCubes)
                    If Not TruckDataObject.moveLoadToNextLargerTruck(t, RtCapPref) Then
                        blnNeedToSplit = True
                        Exit Do
                    End If
                Loop

                If blnNeedToSplit Then
                    'this order will not fit and it must be split
                    OrdersToSplit.Add(o)
                Else
                    blnOrderAdded = False
                    t.populateSpaceValues(spaceNeeded, spaceLeft, o, RtCapPref)
                    If Not t.addOrder(o, RtCapPref) Then
                        'try to clear some space
                        If t.canClearSpace(o.TempControl, trucks, spaceLeft, spaceNeeded, RtCapPref, o.RouteSequence) Then
                            'try again
                            blnOrderAdded = t.addOrder(o, RtCapPref)
                        End If
                    Else
                        blnOrderAdded = True
                    End If
                    If Not blnOrderAdded Then
                        notloadeditems.Add(o)
                    Else
                        AddedOrders.Add(o)
                    End If
                End If
            Next
            If Not AddedOrders Is Nothing AndAlso AddedOrders.Count > 0 Then
                t.TrackingState = TrackingInfo.Updated
                trucks.Add(t)
                For Each r In AddedOrders
                    If Me.Orders.Contains(r) Then
                        Me.Orders.Remove(r)
                        Me.TrackingState = TrackingInfo.Updated
                    End If
                Next
                blnRet = True
            End If

            If Not notloadeditems Is Nothing AndAlso notloadeditems.Count > 0 Then
                If moveToNewTruck(notloadeditems, trucks, RtCapPref) Then
                    'we do not set blnRet to false if this fails
                    blnRet = True
                Else
                    For Each nl In notloadeditems
                        AddToNotLoaded(nl)
                        If Me.Orders.Contains(nl) Then
                            Me.Orders.Remove(nl)
                            Me.TrackingState = TrackingInfo.Updated
                        End If
                    Next
                End If

            End If

            If Not OrdersToSplit Is Nothing AndAlso OrdersToSplit.Count > 0 Then
                'moveToNewTruck(OrdersToSplit, trucks)
                'we need to split the orders and move them to a new truck
                For Each o In OrdersToSplit
                    Dim NewAddedOrders As New List(Of tblSolutionDetail)
                    Dim NotLoadedOrder As New tblSolutionDetail
                    If trucks Is Nothing Then trucks = New List(Of tblSolutionTruck) From {Me}
                    If splitOrder(o, trucks, RtCapPref, NewAddedOrders, NotLoadedOrder) Then
                        If Me.Orders.Contains(o) Then
                            Me.Orders.Remove(o)
                            Me.TrackingState = TrackingInfo.Updated
                        End If
                        blnRet = True
                        If Not NotLoadedOrder Is Nothing AndAlso Not String.IsNullOrEmpty(NotLoadedOrder.OrderNumber) Then
                            If Not Me.masterBuild(NotLoadedOrder, trucks, RtCapPref) Then
                                blnRet = moveToNewTruck(New List(Of tblSolutionDetail) From {NotLoadedOrder}, trucks, RtCapPref)
                            End If
                        End If
                    Else
                        blnRet = False
                    End If
                Next
            End If

            Return blnRet

        End Function

        Public Function CanFit(ByVal type As CapacityType, ByVal value As Double) As Boolean
            Dim blnRet As Boolean = False
            Dim dblMax As Double = 0
            Dim dblCurrent As Double = 0
            Select Case type
                Case CapacityType.Cases
                    dblMax = maxCases
                    dblCurrent = TotalCases()
                Case CapacityType.Cubes
                    dblMax = maxCubes
                    dblCurrent = TotalCubes()
                Case CapacityType.Pallets
                    dblMax = maxPLTs
                    dblCurrent = TotalPlts()
                Case CapacityType.Weight
                    dblMax = maxWgt
                    dblCurrent = TotalWgt()

                Case Else
                    Return False
            End Select
            blnRet = (dblMax = 0 OrElse (value + dblCurrent) <= dblMax)
            Return blnRet
        End Function

        Public Function splitOrder(ByRef largeOrder As tblSolutionDetail, _
                                        ByRef trucks As List(Of tblSolutionTruck), _
                                        ByVal RtCapPref As RoutingCapacityPreference,
                                        ByRef AddedOrders As List(Of tblSolutionDetail), _
                                        ByRef notloaded As tblSolutionDetail) As Boolean
            If Not Me.RouteConfig.StaticRouteSplitOversizedLoads Then Return False
            'In this example we drop largeOrder and create all new orders
            'in produciton we will need to re-organize the items and assign the correct
            'ones to the original largeOrder record
            Dim blnRet As Boolean = False
            Dim spaceNeeded As Double = 0
            Dim spaceLeft As Double = 0
            Dim blnOrderAdded As Boolean = False
            Dim notloadeditems As New List(Of tblSolutionDetail)
            Dim t As tblSolutionTruck
            Dim newTrucks As New List(Of tblSolutionTruck)
            ''  ' Modified by RHR 2/19/2013 we cannot use the current truck even if it is empty because this truck is'
            ''  ' already assigned to the main truck collection so it will update the truck list in trucks not in newTrucks
            ''  ' We must use newTrucks to be sure the sequence numbers are properly assigned.
            'If TruckDataObject.isLargestTruck(Me) AndAlso (Me.Orders Is Nothing OrElse Me.Orders.Count < 1) Then
            '    'this is an empty truck so use it as the first truck for loading the split order data
            '    t = Me
            'Else
            'we cannot call addNewTruck because we need the trucks list to 
            'get the cns and we actually add the new truck to the newTruck list while splitting orders
            t = TruckDataObject.getNewTruck(RtCapPref, BatchProcessingDataObject, BookItemDataObject, GetLargest:=True)
            'End If
            newTrucks.Add(t)
            Dim splitOrders As New List(Of tblSolutionDetail)
            'we use the nextOrderSequence value when calling canclearspace recursively this 
            'should produce sequential temp sequence numbers; the actual sequence numbers will be assigned upon save
            Dim nextOrderSequence As Integer = largeOrder.nextOrderSequenceNbr
            If AddedOrders Is Nothing Then AddedOrders = New List(Of tblSolutionDetail)
            If notloadeditems Is Nothing Then notloadeditems = New List(Of tblSolutionDetail)
            Dim blnMoreToSplit As Boolean = True
            'create seperate orders for each item with unique order sequence numbers
            For Each d In largeOrder.Details.OrderBy(Function(x) x.BookItemHazmat).ThenByDescending(Function(x) x.BookItemWeight)
                Dim o As tblSolutionDetail = largeOrder.CloneNoChildren()
                'assign a temporary control number
                o.TempControl = nextTempOrderControlNbr
                o.TempOrderSequence = nextOrderSequence
                'copy the selected order to the details a list 
                o.Details = New List(Of BookItem) From {d}
                blnMoreToSplit = True
                Do While blnMoreToSplit
                    If o.Cases > t.maxCases Or o.Wgt > t.maxWgt Or o.Plts > t.maxPLTs Or o.Cubes > t.maxCubes Then
                        Dim extraItems As tblSolutionDetail = largeOrder.CloneNoChildren()
                        'assign a temporary control number
                        extraItems.TempControl = nextTempOrderControlNbr
                        'If splitItem(o, trucks, RtCapPref, AddedOrders, extraItems) Then
                        If splitItem(o, newTrucks, RtCapPref, AddedOrders, extraItems) Then
                            'a full truck load of items is created and added to the truck collection
                            'any remaining items are returned in extraItems
                            o = extraItems
                        Else
                            Return False
                        End If
                    Else
                        blnMoreToSplit = False
                        blnOrderAdded = False
                        t.populateSpaceValues(spaceNeeded, spaceLeft, o, RtCapPref)
                        If Not t.addOrder(o, RtCapPref) Then
                            'try to clear some space
                            If t.canClearSpace(o.TempControl, newTrucks, spaceLeft, spaceNeeded, RtCapPref, o.RouteSequence) Then
                                'try again
                                blnOrderAdded = t.addOrder(o, RtCapPref)
                            End If
                        Else
                            blnOrderAdded = True
                        End If
                        If Not blnOrderAdded Then
                            notloadeditems.Add(o)
                        Else
                            AddedOrders.Add(o)
                        End If
                    End If
                    nextOrderSequence = o.nextOrderSequenceNbr
                Loop
            Next
            If Not AddedOrders Is Nothing AndAlso AddedOrders.Count > 0 Then
                Dim truckct As Integer = 0
                'intOrderSequenceNumber should start at zero and go up when saving to db
                'we remove the item details for each order but only update the sequence 
                'number when tempOrderSequence is > 0; this should leave the original 
                'order sequence of zero in place with an updated list of items.
                Dim intOrderSequenceNumber As Integer = largeOrder.TempOrderSequence
                For Each truck In sortTrucksByCapacity(newTrucks, RtCapPref, True)
                    If Not truck.Orders Is Nothing AndAlso truck.Orders.Count > 0 Then
                        'Dim newOrder As New tblSolutionDetail() With {.TempControl = nextTempOrderControlNbr, .OrderNumber = largeOrder.OrderNumber, .TempOrderSequence = intOrderSequenceNumber, .RouteSequence = largeOrder.RouteSequence}
                        Dim newOrder As tblSolutionDetail = largeOrder.CloneNoChildren()
                        'Modified by RHR 2/19/2012  the totals for the new order were not correct
                        'we now update the totals with the totals for the truck.  This logic assumes 
                        'that all trucks except for the last truck will be full truck loads based on 
                        'equipment limitations.
                        With newOrder
                            .TempOrderSequence = intOrderSequenceNumber
                            .HasBeenSplit = True
                            .TempControl = nextTempOrderControlNbr
                            .Details = New List(Of BookItem)
                            .SolutionDetailTotalCases = truck.TotalCases
                            .SolutionDetailTotalCube = truck.TotalCubes
                            .SolutionDetailTotalPL = truck.TotalPlts
                            .SolutionDetailTotalWgt = truck.TotalWgt
                        End With
                        truckct += 1
                        For Each o In truck.Orders
                            If Not o.Details Is Nothing Then
                                For Each d In o.Details
                                    newOrder.Details.Add(d)
                                Next
                            End If
                        Next
                        If truckct < newTrucks.Count Then
                            truck.Orders = New List(Of tblSolutionDetail) From {newOrder}
                            truck.TrackingState = TrackingInfo.Updated
                            '******** Test Code ********************
                            If RtCapPref = tblSolutionTruck.RoutingCapacityPreference.Sequence Then
                                'we add full truck loads of split orders to the tendered collection for this test
                                If trucks.Contains(truck) Then trucks.Remove(truck)
                                AddToReadyToTender(truck)
                            Else
                                If Not trucks.Contains(truck) Then
                                    truck.TrackingState = TrackingInfo.Updated
                                    trucks.Add(truck)
                                End If
                            End If
                        Else
                            'if this is the last truck in newtrucks we test if it is less than a full truck and 
                            'return the remaining items as not loaded  they can be loaded onto existing trucks
                            If (truck.minTenderLoadCases > 0 And truck.TotalCases >= truck.minTenderLoadCases) _
                                Or (truck.minTenderLoadWgt > 0 And truck.TotalWgt >= truck.minTenderLoadWgt) _
                                Or (truck.minTenderLoadPLTs And truck.TotalPlts >= truck.minTenderLoadPLTs) _
                                Or (truck.minTenderLoadCubes > 0 And truck.TotalCubes >= truck.minTenderLoadCubes) Then
                                'this qualifies as a full truck load so just add it to the collection
                                truck.Orders = New List(Of tblSolutionDetail) From {newOrder}
                                truck.TrackingState = TrackingInfo.Updated
                                '******** Test Code ********************
                                If RtCapPref = tblSolutionTruck.RoutingCapacityPreference.Sequence Then
                                    'we add full truck loads of split orders to the tendered collection for this test
                                    If trucks.Contains(truck) Then trucks.Remove(truck)
                                    AddToReadyToTender(truck)
                                Else
                                    If Not trucks.Contains(truck) Then trucks.Add(truck)

                                End If
                            Else
                                'add this order to the no loaded collection
                                notloadeditems.Add(newOrder)
                            End If
                        End If
                    End If
                    intOrderSequenceNumber += 1
                Next
                blnRet = True
            End If
            'We add all of the not loaded items together to create one order
            'in production we will need to confirm that the correct order sequence number is being applied.
            If Not notloadeditems Is Nothing AndAlso notloadeditems.Count > 0 Then
                notloaded = notloadeditems(0)
                For Each o In notloadeditems
                    If o.TempOrderSequence <> notloaded.TempOrderSequence Then
                        For Each d In o.Details
                            notloaded.Details.Add(d)
                        Next
                    End If
                Next
            End If

            Return blnRet
        End Function

        ''' <summary>
        ''' this metod expects an order with one item and splits it into two orders
        ''' each with one item where the first order fits on the truck.  The second
        ''' order is not evaluated and must be recalculated by the calling procedure 
        ''' </summary>
        ''' <param name="splitOrder"></param>
        ''' <param name="trucks"></param>
        ''' <param name="RtCapPref"></param>
        ''' <param name="AddedOrders"></param>
        ''' <param name="extraItems"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function splitItem(ByRef splitOrder As tblSolutionDetail, _
                                        ByRef trucks As List(Of tblSolutionTruck), _
                                        ByVal RtCapPref As RoutingCapacityPreference,
                                        ByRef AddedOrders As List(Of tblSolutionDetail), _
                                        ByRef extraItems As tblSolutionDetail) As Boolean
            'validate that we have item detail data
            If splitOrder.Details Is Nothing OrElse splitOrder.Details.Count < 1 OrElse splitOrder.Details(0) Is Nothing Then
                Return False
            End If
            Dim curItem As BookItem = splitOrder.Details(0)
            Dim blnRet As Boolean = False
            Dim ExtraItemDetails As New BookItem
            Dim newTruck As tblSolutionTruck = TruckDataObject.getNewTruck(RtCapPref, BatchProcessingDataObject, BookItemDataObject, GetLargest:=True)
            Dim casesToMove As Double
            Dim wgtToMove As Double
            Dim pltToMove As Double
            Dim cubesToMove As Double
            Dim wgtPerCase As Double
            Dim pltsPerCase As Double
            Dim cubesPerCase As Double
            'Dim ItemNumber As String = splitOrder.Details(0).BookItemItemNumber
            'Dim blnHazmat As Boolean = splitOrder.Details(0).BookItemHazmat
            Dim blnOrderadded As Boolean = False
            Dim nextOrderSequence As Integer = 0
            'the actual formula to split items to be defined later for now we use this formula
            'determine the weight, plts and cubes neded for each case
            With splitOrder
                wgtPerCase = If(.Wgt > 0 And .Cases > 0, .Wgt / .Cases, 0)
                pltsPerCase = If(.Plts > 0 And .Cases > 0, .Plts / .Cases, 0)
                'Note this logic requires a valid case count.  we will need to use weight, pallets or cubes in the final formula
                cubesPerCase = If(.Cubes > 0 And .Cases > 0, .Cubes / .Cases, 0)
                'in case cases (qty) is 1 or zero?  Not sure how this will work if no qty
                nextOrderSequence = .nextOrderSequenceNbr
                For casect = .Cases - 1 To 1 Step -1
                    casesToMove = .Cases - casect
                    wgtToMove = casesToMove * wgtPerCase
                    pltToMove = casesToMove * pltsPerCase
                    cubesToMove = casesToMove * cubesPerCase

                    Dim splitDetail As BookItem = curItem.Clone
                    With splitDetail
                        .BookItemQtyOrdered = .BookItemQtyOrdered - casesToMove
                        .BookItemWeight = .BookItemWeight - wgtToMove
                        .BookItemPallets = .BookItemPallets - pltToMove
                        .BookItemCube = .BookItemCube - cubesToMove
                        .TempControl = nextTempItemDetailControlNbr
                    End With

                    'NOTE:  This logic was developed in the POC.  It was intended to add as many items
                    'to the truck as would fit leaving the remainder in casestomove, wgttomove, plttomove and cubestomove.
                    'These values are loaded below in extraItems.  It is not clear if this works as expected because extraitems
                    'should only contain the qty that could not be loaded.  Additional testing is needed.
                    Dim testOrder As tblSolutionDetail = splitOrder.CloneNoChildren
                    Dim newDetails As New System.Collections.Generic.List(Of BookItem)
                    newDetails.Add(splitDetail)
                    testOrder.Details = newDetails
                    'testOrder.Details = New List(Of BookItem)(splitDetail)

                    If newTruck.addOrder(testOrder, RtCapPref) Then
                        testOrder.TempControl = nextTempOrderControlNbr
                        testOrder.TempOrderSequence = nextOrderSequence
                        nextOrderSequence = testOrder.nextOrderSequenceNbr
                        blnOrderadded = True
                        Exit For
                    End If
                Next
                If blnOrderadded Then
                    newTruck.TrackingState = TrackingInfo.Updated
                    trucks.Add(newTruck)
                    blnRet = True
                    'new code not finished
                    Dim extraItem As BookItem = curItem.Clone
                    With extraItem
                        .BookItemQtyOrdered = casesToMove
                        .BookItemWeight = wgtToMove
                        .BookItemPallets = pltToMove
                        .BookItemCube = cubesToMove
                        .TempControl = nextTempItemDetailControlNbr
                    End With
                    Dim eItems As New List(Of BookItem)
                    eItems.Add(extraItem)
                    extraItems.Details = eItems
                    'extraItems.Details = New List(Of BookItem)(extraItem)
                End If
            End With
            Return blnRet
        End Function

        Public Sub updateEquipmentInfo(ByVal e As CarriersForRoute)
            If TruckDataObject Is Nothing Then Return 'we cannot change the equipment info
            Me.TrackingState = TrackingInfo.Updated
            settingCarriersForRoute = e
            With e

                Me.SolutionTruckCarrierControl = .StaticRouteCarrCarrierControl
                Me.SolutionTruckCarrierName = .StaticRouteCarrCarrierName
                'these fields are not currently available in the equipment data so we set them to default 
                'to avoid any confusion.  
                Me.SolutionTruckCarrierNumber = 0
                Me.SolutionTruckCarrierEquipmentCodes = ""

                With Me.RouteConfig
                    .StaticRouteControl = TruckDataObject.StaticRouteData.StaticRouteControl
                    .StaticRouteCarrControl = e.StaticRouteCarrControl
                    .StaticRouteEquipControl = e.StaticRouteEquipControl
                    .StaticRouteAutoTenderFlag = TruckDataObject.StaticRouteData.StaticRouteAutoTenderFlag
                    .StaticRouteUseShipDateFlag = TruckDataObject.StaticRouteData.StaticRouteUseShipDateFlag
                    .StaticRouteGuideDateSelectionDaysBefore = TruckDataObject.StaticRouteData.StaticRouteGuideDateSelectionDaysBefore
                    .StaticRouteGuideDateSelectionDaysAfter = TruckDataObject.StaticRouteData.StaticRouteGuideDateSelectionDaysAfter
                    .StaticRouteSplitOversizedLoads = TruckDataObject.StaticRouteData.StaticRouteSplitOversizedLoads
                    .StaticRouteCapacityPreference = TruckDataObject.StaticRouteData.StaticRouteCapacityPreference
                    .StaticRouteRequireAutoTenderApproval = TruckDataObject.StaticRouteData.StaticRouteRequireAutoTenderApproval
                    .StaticRouteFillLargestFirst = TruckDataObject.StaticRouteData.StaticRouteFillLargestFirst
                    .StaticRoutePlaceOnHold = TruckDataObject.StaticRouteData.StaticRoutePlaceOnHold
                    .StaticRouteCarrCarrierControl = e.StaticRouteCarrCarrierControl
                    .StaticRouteCarrRouteTypeCode = e.StaticRouteCarrRouteTypeCode
                    .StaticRouteCarrAutoTenderFlag = e.StaticRouteCarrAutoTenderFlag
                    .StaticRouteCarrTendLeadTime = e.StaticRouteCarrTendLeadTime
                    .StaticRouteCarrMaxStops = e.StaticRouteCarrMaxStops
                    .StaticRouteCarrHazmatFlag = e.StaticRouteCarrHazmatFlag
                    .StaticRouteCarrTransType = e.StaticRouteCarrTransType
                    .StaticRouteCarrRouteSequence = e.StaticRouteCarrRouteSequence
                    .StaticRouteCarrRequireAutoTenderApproval = e.StaticRouteCarrRequireAutoTenderApproval
                    .StaticRouteCarrAutoAcceptLoads = e.StaticRouteCarrAutoAcceptLoads
                    .StaticRouteStateFilter = e.StaticRouteStateFilter
                End With


                Me.SolutionTruckRouteTypeCode = .StaticRouteCarrRouteTypeCode


                'truck settings
                Me.SolutionTruckCarrierTruckControl = .StaticRouteEquipCarrierTruckControl
                Me.SolutionTruckMaxCases = .CarrierTruckMaxCases
                Me.SolutionTruckMinCases = .CarrierTruckMinCases
                Me.SolutionTruckSplitCases = .CarrierTruckSplitCases

                Me.SolutionTruckMaxWgt = .CarrierTruckMaxWgt
                Me.SolutionTruckMinWgt = .CarrierTruckMinWgt
                Me.SolutionTruckSplitWgt = .CarrierTruckSplitWgt

                Me.SolutionTruckMaxCubes = .CarrierTruckMaxCubes
                Me.SolutionTruckMinCubes = .CarrierTruckMinCubes
                Me.SolutionTruckSplitCubes = .CarrierTruckSplitCubes

                Me.SolutionTruckMaxPlts = .CarrierTruckMaxPlts
                Me.SolutionTruckMinPlts = .CarrierTruckMinPlts
                Me.SolutionTruckSplitPlts = .CarrierTruckSplitPlts
                Me.SolutionTruckTrucksAvailable = .CarrierTruckTrucksAvailable


            End With
        End Sub

#End Region

#Region " CRUD Methods "

        ''' <summary>
        ''' Saves the changes to the order to the db exceptions are passed back to the caller
        ''' The caller must capture any fault exceptions thrown by the update stored procedures
        ''' </summary>
        ''' <param name="order"></param>
        ''' <remarks></remarks>
        Private Sub saveOrder(ByRef order As tblSolutionDetail)
            Dim BookRouteConsFlag As Boolean = True
            If Me.SolutionTruckRouteTypeCode = RouteTypeCodes.LTL_Pool Then BookRouteConsFlag = False
            Dim BookHoldLoad As Integer = 0
            If Me.RouteConfig.StaticRoutePlaceOnHold Then BookHoldLoad = 1
            'the tempcontrol numbers start at -1000000 and are assigned when an order is split
            'the same is true for each split item and the order sequence
            If order.SolutionDetailStopNo < 1 Then order.SolutionDetailStopNo = 1
            If order.TempControl > 0 AndAlso order.HasBeenSplit = False Then
                'we can save the changes to the database because this order was not split
                Me.TruckDataObject.UpdateLoadPlanningCarrier(order.SolutionDetailBookControl, _
                                                             Me.CNS, _
                                                             BookRouteConsFlag, _
                                                             order.SolutionDetailStopNo, _
                                                             Me.SolutionTruckCarrierControl, _
                                                             Me.SolutionTruckCarrierTruckControl, _
                                                             BookHoldLoad)
            Else
                'this is a split order so we must first create or update the booking record then remove and add each of the item details
                Dim NewOrderData = TruckDataObject.UpdateLoadPlanningSplitOrder(order.SolutionDetailBookControl, _
                                                                                order.TempOrderSequence, _
                                                                                 Me.CNS, _
                                                                                 BookRouteConsFlag, _
                                                                                 order.SolutionDetailStopNo, _
                                                                                 Me.SolutionTruckCarrierControl, _
                                                                                 Me.SolutionTruckCarrierTruckControl, _
                                                                                 BookHoldLoad)

                If NewOrderData Is Nothing OrElse NewOrderData.ErrorNumber <> 0 Then
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "Cannot save split order information. Please check the data carefully for order number " & order.OrderNumber, .Details = "system failure: No data was returned from the stored procedure."}, New FaultReason("Process UpdateLoadPlanningSplitOrder Procedure Failure"))
                ElseIf NewOrderData.ErrorNumber.HasValue AndAlso NewOrderData.ErrorNumber <> 0 Then
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "Cannot save split order information. Please check the data carefully for order number " & order.OrderNumber, .Details = NewOrderData.RetMessage}, New FaultReason("Process UpdateLoadPlanningSplitOrder Procedure Failure"))
                ElseIf Not NewOrderData.NewBookLoadControl.HasValue Or (NewOrderData.NewBookLoadControl.HasValue AndAlso NewOrderData.NewBookLoadControl = 0) Then
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "Cannot save split order item detail information. Please check the data carefully for order number " & order.OrderNumber, .Details = "The Load Detail data was not saved so there was not BookLoadControl number."}, New FaultReason("Process UpdateLoadPlanningSplitOrder Procedure Failure"))
                Else
                    'add the item details
                    For Each i In order.Details
                        i.BookItemBookLoadControl = NewOrderData.NewBookLoadControl
                        BookItemDataObject.CreateRecord(i)
                    Next
                End If
            End If
        End Sub


        Public Sub SaveChanges()

            If Me.Orders Is Nothing OrElse Me.Orders.Count < 1 Then Return 'nothing to do
            If Left(Me.CNS, 7) = "TempCNS" Then Me.CNS = getNextCNSNbr()
            If Me.RouteConfig.StaticRouteCapacityPreference = RoutingCapacityPreference.Sequence Then
                'we resequence the stops to match the sequence numbers
                Dim stopNo As Integer = 0
                Dim strPreviousLocation As String = ""
                For Each o In Me.Orders.OrderBy(Function(x) x.SolutionDetailDefaultRouteSequence)
                    If strPreviousLocation <> o.LaneLocation Then stopNo += 1 'we increase the stop number only when the location changes 
                    o.SolutionDetailStopNo = stopNo
                    strPreviousLocation = o.LaneLocation
                Next
            End If
            For Each o In Me.Orders
                saveOrder(o)
            Next
            Me.TrackingState = TrackingInfo.Unchanged
        End Sub

        Public Function getNextCNSNbr() As String
            Return BatchProcessingDataObject.GetNextConsNumber(Me.SolutionTruckCompControl)
        End Function

        Public Function getTempCNSNbr() As String
            If Me.TruckDataObject Is Nothing Then
                Return "TempCNS01"
            Else
                Return TruckDataObject.getTempCNSNbr()
            End If
        End Function

#End Region

#Region " Global Properties and Methods that need to be moved or changed"

        Public Function sortTrucksByCapacity(ByRef trucks As List(Of tblSolutionTruck), ByVal RtCapPref As tblSolutionTruck.RoutingCapacityPreference, Optional ByVal decending As Boolean = False) As List(Of tblSolutionTruck)
            Select Case RtCapPref
                Case tblSolutionTruck.RoutingCapacityPreference.Cases
                    If decending Then
                        Return trucks.OrderByDescending(Function(s) s.TotalCases).ToList
                    Else
                        Return trucks.OrderBy(Function(s) s.TotalCases).ToList
                    End If
                Case tblSolutionTruck.RoutingCapacityPreference.Weight
                    If decending Then
                        Return trucks.OrderByDescending(Function(s) s.TotalWgt).ToList
                    Else
                        Return trucks.OrderBy(Function(s) s.TotalWgt).ToList
                    End If

                Case tblSolutionTruck.RoutingCapacityPreference.Pallets
                    If decending Then
                        Return trucks.OrderByDescending(Function(s) s.TotalPlts).ToList
                    Else
                        Return trucks.OrderBy(Function(s) s.TotalPlts).ToList
                    End If

                Case tblSolutionTruck.RoutingCapacityPreference.Cubes
                    If decending Then
                        Return trucks.OrderByDescending(Function(s) s.TotalCubes).ToList
                    Else
                        Return trucks.OrderBy(Function(s) s.TotalCubes).ToList
                    End If

                Case tblSolutionTruck.RoutingCapacityPreference.Sequence
                    If decending Then
                        Return trucks.OrderByDescending(Function(s) s.TotalPlts).ToList
                    Else
                        Return trucks.OrderBy(Function(s) s.TotalPlts).ToList
                    End If

                Case Else
                    Return trucks
            End Select
        End Function

        Private Function moveToLargerTruck(ByVal LaneLocation As String, _
                                        ByRef items As List(Of tblSolutionDetail), _
                                        ByRef trucks As List(Of tblSolutionTruck), _
                                        Optional ByVal RtCapPref As RoutingCapacityPreference = RoutingCapacityPreference.Pallets, _
                                        Optional ByVal intSeq As Integer = 0) As Boolean
            Dim blnRet As Boolean = False
            'Dim blnOrdersMoved As Boolean = False

            'Dim AddedOrders As New List(Of tblSolutionDetail)
            'Dim spaceNeeded As Double = 0
            'Dim maxSpaceAvailable As Double = 0
            'Select Case RtCapPref
            '    Case RoutingCapacityPreference.Cases
            '        spaceNeeded = items.Sum(Function(x) x.Cases)
            '        maxSpaceAvailable = Me.maxCases
            '    Case RoutingCapacityPreference.Weight
            '        spaceNeeded = items.Sum(Function(x) x.Wgt)
            '        maxSpaceAvailable = Me.maxWgt
            '    Case RoutingCapacityPreference.Pallets
            '        spaceNeeded = items.Sum(Function(x) x.Plts)
            '        maxSpaceAvailable = Me.maxPLTs
            '    Case RoutingCapacityPreference.Cubes
            '        spaceNeeded = items.Sum(Function(x) x.Cubes)
            '        maxSpaceAvailable = Me.maxCubes
            '    Case RoutingCapacityPreference.Sequence
            '        'when routing by sequence number we use pallets by default
            '        spaceNeeded = items.Sum(Function(x) x.Plts)
            '        maxSpaceAvailable = Me.maxPLTs
            '    Case Else
            '        Return False
            'End Select

            'Dim spaceLeft As Double = 0
            'Dim currentCNS As String = Me.CNS

            'Dim existingTrucks As New List(Of tblSolutionTruck)
            ''first see if any truck already has orders with this LaneLocation 
            'existingTrucks = trucks.Where(Function(p) p.CNS <> currentCNS And p.Orders.Any(Function(y) y.LaneLocation = LaneLocation)).ToList

            'If Not existingTrucks Is Nothing AndAlso existingTrucks.Count > 0 Then
            '    'try to move the orders to one of these trucks
            '    'see if we can move all of the orders that do not match to the next truck
            '    For Each t In existingTrucks
            '        'see if we can move all of the orders that do not match to the next truck
            '        Select Case RtCapPref
            '            Case RoutingCapacityPreference.Cases
            '                spaceLeft = t.maxCases - t.TotalCases
            '            Case RoutingCapacityPreference.Weight
            '                spaceLeft = t.maxWgt - t.TotalWgt
            '            Case RoutingCapacityPreference.Pallets
            '                spaceLeft = t.maxPLTs - t.TotalPlts
            '            Case RoutingCapacityPreference.Cubes
            '                spaceLeft = t.maxCubes - t.TotalCubes
            '            Case RoutingCapacityPreference.Sequence
            '                spaceLeft = t.maxPLTs - t.TotalPlts
            '            Case Else
            '                Return False
            '        End Select
            '        If RtCapPref = RoutingCapacityPreference.Sequence Then
            '            blnOrdersMoved = True
            '            For Each o In items
            '                'if routing by sequence number we must make the order fit
            '                Do Until t.addOrder(o, trucks, RtCapPref)
            '                    If Not t.canClearSpace(LaneLocation, trucks, spaceLeft, spaceNeeded, RtCapPref, intSeq) Then
            '                        blnOrdersMoved = False
            '                        Exit For
            '                    End If
            '                Loop
            '                AddedOrders.Add(o)
            '            Next
            '            If blnOrdersMoved Then
            '                For Each r In AddedOrders
            '                    If Me.Orders.Contains(r) Then
            '                        Me.Orders.Remove(r)
            '                        Me.TrackingState = TrackingInfo.Updated
            '                    End If
            '                Next
            '                Return True
            '            Else
            '                For Each r In AddedOrders
            '                    If t.Orders.Contains(r) Then
            '                        t.Orders.Remove(r)
            '                        t.TrackingState = TrackingInfo.Updated
            '                    End If
            '                Next
            '            End If
            '        Else
            '            If spaceLeft >= spaceNeeded Then
            '                blnOrdersMoved = True
            '                For Each o In items
            '                    If t.addOrder(o, trucks, RtCapPref) Then
            '                        AddedOrders.Add(o)
            '                    Else
            '                        blnOrdersMoved = False
            '                        Exit For
            '                    End If
            '                Next
            '                If blnOrdersMoved Then
            '                    For Each r In AddedOrders
            '                        If Me.Orders.Contains(r) Then
            '                            Me.Orders.Remove(r)
            '                            Me.TrackingState = TrackingInfo.Updated
            '                        End If
            '                    Next
            '                    Return True
            '                Else
            '                    'If all the orders do not fit remove them from the selected truck t
            '                    For Each r In AddedOrders
            '                        If t.Orders.Contains(r) Then
            '                            t.Orders.Remove(r)
            '                            t.TrackingState = TrackingInfo.Updated
            '                        End If
            '                    Next
            '                End If
            '            Else
            '                If t.canClearSpace(LaneLocation, trucks, spaceLeft, spaceNeeded, RtCapPref, intSeq) Then
            '                    blnOrdersMoved = True
            '                    For Each o In items
            '                        If t.addOrder(o, trucks, RtCapPref) Then
            '                            AddedOrders.Add(o)
            '                        Else
            '                            blnOrdersMoved = False
            '                            Exit For
            '                        End If
            '                    Next
            '                    If blnOrdersMoved Then
            '                        For Each r In AddedOrders
            '                            If Me.Orders.Contains(r) Then
            '                                Me.Orders.Remove(r)
            '                                Me.TrackingState = TrackingInfo.Updated
            '                            End If
            '                        Next
            '                        Return True
            '                        Exit For
            '                    Else
            '                        For Each r In AddedOrders
            '                            If t.Orders.Contains(r) Then
            '                                t.Orders.Remove(r)
            '                                t.TrackingState = TrackingInfo.Updated
            '                            End If
            '                        Next
            '                    End If
            '                End If
            '            End If
            '        End If
            '    Next
            '    If Not blnOrdersMoved Then
            '        Return moveToNewTruck(items, trucks, RtCapPref)
            '    End If
            Return blnRet

        End Function

        Public ReadOnly Property nextTempOrderControlNbr As Integer
            Get
                gOrderControlSeed -= 1
                Return gOrderControlSeed
            End Get
        End Property

        Public ReadOnly Property nextTempItemDetailControlNbr As Integer
            Get
                gItemDetailControlSeed -= 1
                Return gItemDetailControlSeed
            End Get
        End Property

#End Region

#Region " private or Protected Methods "

        Private Sub AddToNotLoaded(ByRef o As tblSolutionDetail)
            If TruckDataObject Is Nothing Then Return
            TruckDataObject.AddToNotLoaded(o, TruckDataObject.OrdersNotLoaded)
        End Sub

        Private Sub AddToReadyToTender(ByRef t As tblSolutionTruck)
            If TruckDataObject Is Nothing Then Return
            TruckDataObject.AddToReadyToTender(t)
        End Sub



#End Region

    End Class

    Public Class clsOrderSummary
        Public TotalCases As Double = 0
        Public TotalWgt As Double = 0
        Public TotalPLTs As Double = 0
        Public TotalCubes As Double = 0
        Public Key As String
        Public MaxSeq As Integer = 0
        Public Count As Integer = 0
        Public Control As Integer = 0
        Public LaneLocation As String = ""

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal cases As Double, _
                       ByVal wgt As Double, _
                       ByVal plts As Double, _
                       ByVal cubes As Double, _
                       ByVal seq As Double, _
                       ByVal key As String, _
                       ByVal count As Integer)
            MyBase.New()
            Me.TotalCases = cases
            Me.TotalWgt = wgt
            Me.TotalPLTs = plts
            Me.TotalCubes = cubes
            Me.MaxSeq = seq
            Me.Key = key
            Me.Count = count
        End Sub

    End Class
End Namespace