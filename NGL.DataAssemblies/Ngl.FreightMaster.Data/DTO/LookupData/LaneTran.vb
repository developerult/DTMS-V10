Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneTran
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ID As Integer = 0
        <DataMember()> _
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Private _LaneTransNumber As Integer = 0
        <DataMember()> _
        Public Property LaneTransNumber() As Integer
            Get
                Return _LaneTransNumber
            End Get
            Set(ByVal value As Integer)
                _LaneTransNumber = value
            End Set
        End Property

        Private _LaneTransTypeDesc As String = ""
        <DataMember()> _
        Public Property LaneTransTypeDesc() As String
            Get
                Return Left(_LaneTransTypeDesc, 50)
            End Get
            Set(ByVal value As String)
                _LaneTransTypeDesc = Left(value, 50)
            End Set
        End Property

        Private _LaneTransServiceFeeMin As Decimal = 0
        <DataMember()> _
        Public Property LaneTransServiceFeeMin() As Decimal
            Get
                Return _LaneTransServiceFeeMin
            End Get
            Set(ByVal value As Decimal)
                _LaneTransServiceFeeMin = value
            End Set
        End Property

        Private _LaneTransServiceFeeMax As Decimal = 0
        <DataMember()> _
        Public Property LaneTransServiceFeeMax() As Decimal
            Get
                Return _LaneTransServiceFeeMax
            End Get
            Set(ByVal value As Decimal)
                _LaneTransServiceFeeMax = value
            End Set
        End Property

        Private _LaneTransServiceFeeFlat As Decimal = 0
        <DataMember()> _
        Public Property LaneTransServiceFeeFlat() As Decimal
            Get
                Return _LaneTransServiceFeeFlat
            End Get
            Set(ByVal value As Decimal)
                _LaneTransServiceFeeFlat = value
            End Set
        End Property

        Private _LaneTransServiceFeePerc As Double = 0
        <DataMember()> _
        Public Property LaneTransServiceFeePerc() As Double
            Get
                Return _LaneTransServiceFeePerc
            End Get
            Set(ByVal value As Double)
                _LaneTransServiceFeePerc = value
            End Set
        End Property

        Private _LaneTransUpdated As Byte()
        <DataMember()> _
        Public Property LaneTransUpdated() As Byte()
            Get
                Return _LaneTransUpdated
            End Get
            Set(ByVal value As Byte())
                _LaneTransUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneTran
            instance = DirectCast(MemberwiseClone(), LaneTran)
            Return instance
        End Function

#End Region

    End Class
End Namespace