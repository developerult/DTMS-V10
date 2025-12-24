Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompCal
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompCalControl As Integer = 0
        <DataMember()> _
        Public Property CompCalControl() As Integer
            Get
                Return _CompCalControl
            End Get
            Set(ByVal value As Integer)
                _CompCalControl = value
            End Set
        End Property

        Private _CompCalCompControl As Integer = 0
        <DataMember()> _
        Public Property CompCalCompControl() As Integer
            Get
                Return _CompCalCompControl
            End Get
            Set(ByVal value As Integer)
                _CompCalCompControl = value
            End Set
        End Property

        Private _CompCalMonth As Integer = 0
        <DataMember()> _
        Public Property CompCalMonth() As Integer
            Get
                Return _CompCalMonth
            End Get
            Set(ByVal value As Integer)
                _CompCalDay = value
            End Set
        End Property

        Private _CompCalDay As Integer = 0
        <DataMember()> _
        Public Property CompCalDay() As Integer
            Get
                Return _CompCalDay
            End Get
            Set(ByVal value As Integer)
                _CompCalDay = value
            End Set
        End Property

        Private _CompCalOpen As Boolean = False
        <DataMember()> _
        Public Property CompCalOpen() As Boolean
            Get
                Return _CompCalOpen
            End Get
            Set(ByVal value As Boolean)
                _CompCalOpen = value
            End Set
        End Property

        Private _CompCalStartTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompCalStartTime() As System.Nullable(Of Date)
            Get
                Return _CompCalStartTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompCalStartTime = value
            End Set
        End Property

        Private _CompCalEndTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompCalEndTime() As System.Nullable(Of Date)
            Get
                Return _CompCalEndTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompCalEndTime = value
            End Set
        End Property

        Private _CompCalIsHoliday As Boolean = False
        <DataMember()> _
        Public Property CompCalIsHoliday() As Boolean
            Get
                Return _CompCalIsHoliday
            End Get
            Set(ByVal value As Boolean)
                _CompCalIsHoliday = value
            End Set
        End Property

        Private _CompCalUpdated As Byte()
        <DataMember()> _
        Public Property CompCalUpdated() As Byte()
            Get
                Return _CompCalUpdated
            End Get
            Set(ByVal value As Byte())
                _CompCalUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompCal
            instance = DirectCast(MemberwiseClone(), CompCal)
            Return instance
        End Function

#End Region

    End Class
End Namespace