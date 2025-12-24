Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV on 10/31/16 for v-7.0.5.110 Lane Default Carrier Enhancements 

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LimitLaneToCarrierDetails
        Inherits DTOBaseClass


#Region " Data Members"

        Private _LLTCDControl As Integer = 0
        <DataMember()> _
        Public Property LLTCDControl() As Integer
            Get
                Return _LLTCDControl
            End Get
            Set(ByVal value As Integer)
                _LLTCDControl = value
            End Set
        End Property

        Private _ModeTypeControl As Integer = 0
        <DataMember()> _
        Public Property ModeTypeControl() As Integer
            Get
                Return _ModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _ModeTypeControl = value
            End Set
        End Property

        Private _TempType As Integer = 0
        <DataMember()> _
        Public Property TempType() As Integer
            Get
                Return _TempType
            End Get
            Set(ByVal value As Integer)
                _TempType = value
            End Set
        End Property

        Private _MaxCases As Integer = 0
        <DataMember()> _
        Public Property MaxCases() As Integer
            Get
                Return _MaxCases
            End Get
            Set(ByVal value As Integer)
                _MaxCases = value
            End Set
        End Property

        Private _MaxWgt As Double = 0
        <DataMember()> _
        Public Property MaxWgt() As Double
            Get
                Return _MaxWgt
            End Get
            Set(ByVal value As Double)
                _MaxWgt = value
            End Set
        End Property

        Private _MaxCube As Integer = 0
        <DataMember()> _
        Public Property MaxCube() As Integer
            Get
                Return _MaxCube
            End Get
            Set(ByVal value As Integer)
                _MaxCube = value
            End Set
        End Property

        Private _MaxPL As Double = 0
        <DataMember()> _
        Public Property MaxPL() As Double
            Get
                Return _MaxPL
            End Get
            Set(ByVal value As Double)
                _MaxPL = value
            End Set
        End Property

        Private _MinAllowedCost As Decimal = 0
        <DataMember()> _
        Public Property MinAllowedCost() As Decimal
            Get
                Return _MinAllowedCost
            End Get
            Set(ByVal value As Decimal)
                _MinAllowedCost = value
            End Set
        End Property

        Private _MaxAllowedCost As Decimal = 0
        <DataMember()> _
        Public Property MaxAllowedCost() As Decimal
            Get
                Return _MaxAllowedCost
            End Get
            Set(ByVal value As Decimal)
                _MaxAllowedCost = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LimitLaneToCarrierDetails
            instance = DirectCast(MemberwiseClone(), LimitLaneToCarrierDetails)
            Return instance
        End Function

#End Region

    End Class
End Namespace


