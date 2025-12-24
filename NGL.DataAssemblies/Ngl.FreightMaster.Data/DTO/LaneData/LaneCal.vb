Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneCal
        Inherits DTOBaseClass


#Region " Data Members"

        Private _LaneCalControl As Integer = 0
        <DataMember()> _
        Public Property LaneCalControl() As Integer
            Get
                Return _LaneCalControl
            End Get
            Set(ByVal value As Integer)
                _LaneCalControl = value
            End Set
        End Property

        Private _LaneCalLaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneCalLaneControl() As Integer
            Get
                Return _LaneCalLaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneCalLaneControl = value
            End Set
        End Property

        Private _LaneCalMonth As Integer = 0
        <DataMember()> _
        Public Property LaneCalMonth() As Integer
            Get
                Return _LaneCalMonth
            End Get
            Set(ByVal value As Integer)
                _LaneCalMonth = value
            End Set
        End Property

        Private _LaneCalDay As Integer = 0
        <DataMember()> _
        Public Property LaneCalDay() As Integer
            Get
                Return _LaneCalDay
            End Get
            Set(ByVal value As Integer)
                _LaneCalDay = value
            End Set
        End Property

        Private _LaneCalOpen As Boolean = False
        <DataMember()> _
        Public Property LaneCalOpen() As Boolean
            Get
                Return _LaneCalOpen
            End Get
            Set(ByVal value As Boolean)
                _LaneCalOpen = value
            End Set
        End Property


        Private _LaneCalStartTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneCalStartTime() As System.Nullable(Of Date)
            Get
                Return _LaneCalStartTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneCalStartTime = value
            End Set
        End Property

        Private _LaneCalEndTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LaneCalEndTime() As System.Nullable(Of Date)
            Get
                Return _LaneCalEndTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LaneCalEndTime = value
            End Set
        End Property

        Private _LaneCalIsHoliday As Boolean = False
        <DataMember()> _
        Public Property LaneCalIsHoliday() As Boolean
            Get
                Return _LaneCalIsHoliday
            End Get
            Set(ByVal value As Boolean)
                _LaneCalIsHoliday = value
            End Set
        End Property

        Private _LaneCalApplyToOrigin As Boolean = False
        <DataMember()> _
        Public Property LaneCalApplyToOrigin() As Boolean
            Get
                Return _LaneCalApplyToOrigin
            End Get
            Set(ByVal value As Boolean)
                _LaneCalApplyToOrigin = value
            End Set
        End Property


        Private _LaneCalUpdated As Byte()
        <DataMember()> _
        Public Property LaneCalUpdated() As Byte()
            Get
                Return _LaneCalUpdated
            End Get
            Set(ByVal value As Byte())
                _LaneCalUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneCal
            instance = DirectCast(MemberwiseClone(), LaneCal)
            Return instance
        End Function

#End Region

    End Class
End Namespace