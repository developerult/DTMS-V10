Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports TAR = Ngl.FM.CarTar

''' <summary>
''' Prefered Default Carrier class
''' </summary>
''' <remarks>
''' Created by RHR for v-7.0.6.0 on 11/30/2016
'''   used to sort and select the best default carrier
''' </remarks>
Public Class clsPreferedDefaultCarrier

    Private _Control As Integer
    Public Property Control() As Integer
        Get
            Return _Control
        End Get
        Set(ByVal value As Integer)
            _Control = value
        End Set
    End Property

    Private _SequenceNumber As Integer
    Public Property SequenceNumber() As Integer
        Get
            Return _SequenceNumber
        End Get
        Set(ByVal value As Integer)
            _SequenceNumber = value
        End Set
    End Property

    Private _CarrierCost As Decimal
    Public Property CarrierCost() As Decimal
        Get
            Return _CarrierCost
        End Get
        Set(ByVal value As Decimal)
            _CarrierCost = value
        End Set
    End Property

    Private _MinCostAllowed As Decimal
    Public Property MinCostAllowed() As Decimal
        Get
            Return _MinCostAllowed
        End Get
        Set(ByVal value As Decimal)
            _MinCostAllowed = value
        End Set
    End Property

    Private _MaxCostAllowed As Decimal
    Public Property MaxCostAllowed() As Decimal
        Get
            Return _MaxCostAllowed
        End Get
        Set(ByVal value As Decimal)
            _MaxCostAllowed = value
        End Set
    End Property

    Private _CarriersByCost As DTO.CarriersByCost
    Public Property CarriersByCost() As DTO.CarriersByCost
        Get
            Return _CarriersByCost
        End Get
        Set(ByVal value As DTO.CarriersByCost)
            _CarriersByCost = value
        End Set
    End Property

    Private _SelectedBid As LTS.tblBid
    Public Property SelectedBid() As LTS.tblBid
        Get
            Return _SelectedBid
        End Get
        Set(ByVal value As LTS.tblBid)
            _SelectedBid = value
        End Set
    End Property

    Private _PreferredCarrier As DTO.LimitLaneToCarrier
    Public Property PreferredCarrier() As DTO.LimitLaneToCarrier
        Get
            Return _PreferredCarrier
        End Get
        Set(ByVal value As DTO.LimitLaneToCarrier)
            _PreferredCarrier = value
        End Set
    End Property


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.0 on 12/16/2016
    '''   added logic to allow zero for minimun and maximum cost validation
    ''' </remarks>
    Public ReadOnly Property IsCostAllowed() As Boolean
        Get
            Dim blnReturn As Boolean = True
            If Me.MinCostAllowed > 0 Then
                If Me.CarrierCost < Me.MinCostAllowed Then blnReturn = False
            End If
            If Me.MaxCostAllowed > 0 Then
                If Me.CarrierCost > Me.MaxCostAllowed Then
                    blnReturn = False
                End If
            End If

            Return blnReturn

        End Get

    End Property

    'LLTCMaxCases = 0 OrElse TotalCases <= d.LLTCMaxCases) _
    '                                                            And _
    '                                                            (d.LLTCMaxWgt = 0 OrElse TotalWgt <= d.LLTCMaxWgt) _
    '                                                            And _
    '                                                            (d.LLTCMaxCube = 0 OrElse TotalCubes <= d.LLTCMaxCube) _
    '                                                            And _
    '                                                            (d.LLTCMaxPL = 0 OrElse TotalPlts <= d.LLTCMaxPL) _


    Private _LLTCMaxCases As Integer
    Public Property LLTCMaxCases() As Integer
        Get
            Return _LLTCMaxCases
        End Get
        Set(ByVal value As Integer)
            _LLTCMaxCases = value
        End Set
    End Property

    Private _LLTCMaxWgt As Double
    Public Property LLTCMaxWgt() As Double
        Get
            Return _LLTCMaxWgt
        End Get
        Set(ByVal value As Double)
            _LLTCMaxWgt = value
        End Set
    End Property

    Private _LLTCMaxCube As Integer
    Public Property LLTCMaxCube() As Integer
        Get
            Return _LLTCMaxCube
        End Get
        Set(ByVal value As Integer)
            _LLTCMaxCube = value
        End Set
    End Property

    Private _LLTCMaxPL As Double
    Public Property LLTCMaxPL() As Double
        Get
            Return _LLTCMaxPL
        End Get
        Set(ByVal value As Double)
            _LLTCMaxPL = value
        End Set
    End Property



    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal c As DTO.CarriersByCost, ByVal p As DTO.LimitLaneToCarrier)
        MyBase.New()
        Me.CarriersByCost = c
        Me.PreferredCarrier = p
        Me.Control = p.LLTCControl
        Me.CarrierCost = c.CarrierCost
        Me.SequenceNumber = p.LLTCSequenceNumber
        Me.MinCostAllowed = p.LLTCMinAllowedCost
        Me.MaxCostAllowed = p.LLTCMaxAllowedCost
        Me.LLTCMaxPL = p.LLTCMaxPL
        Me.LLTCMaxCube = p.LLTCMaxCube
        Me.LLTCMaxWgt = p.LLTCMaxWgt
        Me.LLTCMaxCases = p.LLTCMaxCases
    End Sub

    Public Sub New(ByVal b As LTS.tblBid, ByVal p As DTO.LimitLaneToCarrier)
        MyBase.New()
        Me.SelectedBid = b
        Me.PreferredCarrier = p
        Me.Control = p.LLTCControl
        Me.CarrierCost = b.BidTotalCost
        Me.SequenceNumber = p.LLTCSequenceNumber
        Me.MinCostAllowed = p.LLTCMinAllowedCost
        Me.MaxCostAllowed = p.LLTCMaxAllowedCost
        Me.LLTCMaxPL = p.LLTCMaxPL
        Me.LLTCMaxCube = p.LLTCMaxCube
        Me.LLTCMaxWgt = p.LLTCMaxWgt
        Me.LLTCMaxCases = p.LLTCMaxCases
    End Sub
    Public Sub New(ByVal b As LTS.tblBid)
        MyBase.New()
        Me.SelectedBid = b
        Me.PreferredCarrier = Nothing
        Me.Control = 0
        Me.CarrierCost = b.BidTotalCost
        Me.SequenceNumber = 0
        Me.MinCostAllowed = 0
        Me.MaxCostAllowed = 0
        Me.LLTCMaxPL = 0
        Me.LLTCMaxCube = 0
        Me.LLTCMaxWgt = 0
        Me.LLTCMaxCases = 0
    End Sub

    Public Function IsCostAllowedc() As Boolean
        If Me.CarrierCost >= Me.MinCostAllowed And Me.CarrierCost <= Me.MaxCostAllowed Then
            Return True
        Else
            Return False
        End If
    End Function









End Class
