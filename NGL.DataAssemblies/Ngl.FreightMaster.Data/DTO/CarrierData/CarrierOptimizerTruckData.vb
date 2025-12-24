Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierOptimizerTruckData
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _TruckControl As String = ""
        <DataMember()> _
        Public Property TruckControl() As String
            Get
                Return Left(_TruckControl, 25)
            End Get
            Set(ByVal value As String)
                _TruckControl = Left(value, 25)
            End Set
        End Property

        Private _SpecialCodes As String = ""
        <DataMember()> _
        Public Property SpecialCodes() As String
            Get
                Return Left(_SpecialCodes, 60)
            End Get
            Set(ByVal value As String)
                _SpecialCodes = Left(value, 60)
            End Set
        End Property

        Private _MileRate As Decimal = 0
        <DataMember()> _
        Public Property MileRate() As Decimal
            Get
                Return _MileRate
            End Get
            Set(ByVal value As Decimal)
                _MileRate = value
            End Set
        End Property

        Private _FlatRate As Decimal = 0
        <DataMember()> _
        Public Property FlatRate() As Decimal
            Get
                Return _FlatRate
            End Get
            Set(ByVal value As Decimal)
                _FlatRate = value
            End Set
        End Property

        Private _DropCost As Decimal = 0
        <DataMember()> _
        Public Property DropCost() As Decimal
            Get
                Return _DropCost
            End Get
            Set(ByVal value As Decimal)
                _DropCost = value
            End Set
        End Property

        Private _PerUnitCost As Decimal = 0
        <DataMember()> _
        Public Property PerUnitCost() As Decimal
            Get
                Return _PerUnitCost
            End Get
            Set(ByVal value As Decimal)
                _PerUnitCost = value
            End Set
        End Property

        Private _CasesAvailable As Double = 0
        <DataMember()> _
        Public Property CasesAvailable() As Double
            Get
                Return _CasesAvailable
            End Get
            Set(ByVal value As Double)
                _CasesAvailable = value
            End Set
        End Property

        Private _WgtAvailable As Double = 0
        <DataMember()> _
        Public Property WgtAvailable() As Double
            Get
                Return _WgtAvailable
            End Get
            Set(ByVal value As Double)
                _WgtAvailable = value
            End Set
        End Property

        Private _PltsAvailable As Integer = 0
        <DataMember()> _
        Public Property PltsAvailable() As Integer
            Get
                Return _PltsAvailable
            End Get
            Set(ByVal value As Integer)
                _PltsAvailable = value
            End Set
        End Property

        Private _CubesAvailable As Double = 0
        <DataMember()> _
        Public Property CubesAvailable() As Double
            Get
                Return _CubesAvailable
            End Get
            Set(ByVal value As Double)
                _CubesAvailable = value
            End Set
        End Property

        Private _ErrMsg As String = ""
        <DataMember()> _
        Public Property ErrMsg() As String
            Get
                Return Left(_ErrMsg, 255)
            End Get
            Set(ByVal value As String)
                _ErrMsg = Left(value, 255)
            End Set
        End Property




#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierOptimizerTruckData
            instance = DirectCast(MemberwiseClone(), CarrierOptimizerTruckData)
            Return instance
        End Function

#End Region

    End Class
End Namespace