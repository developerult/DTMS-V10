Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompGoal
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompGoalControl As Integer = 0
        <DataMember()> _
        Public Property CompGoalControl() As Integer
            Get
                Return _CompGoalControl
            End Get
            Set(ByVal value As Integer)
                _CompGoalControl = value
            End Set
        End Property

        Private _CompGoalCompControl As Integer = 0
        <DataMember()> _
        Public Property CompGoalCompControl() As Integer
            Get
                Return _CompGoalCompControl
            End Get
            Set(ByVal value As Integer)
                _CompGoalCompControl = value
            End Set
        End Property

        Private _CompGoalYear As Short = 0
        <DataMember()> _
        Public Property CompGoalYear() As Short
            Get
                Return _CompGoalYear
            End Get
            Set(ByVal value As Short)
                _CompGoalYear = value
            End Set
        End Property

        Private _CompGoalGrossSales As Integer = 0
        <DataMember()> _
        Public Property CompGoalGrossSales() As Integer
            Get
                Return _CompGoalGrossSales
            End Get
            Set(ByVal value As Integer)
                _CompGoalGrossSales = value
            End Set
        End Property

        Private _CompGoalCOGS As Integer = 0
        <DataMember()> _
        Public Property CompGoalCOGS() As Integer
            Get
                Return _CompGoalCOGS
            End Get
            Set(ByVal value As Integer)
                _CompGoalCOGS = value
            End Set
        End Property

        Private _CompGoalCOGSPer As Double = 0
        <DataMember()> _
        Public Property CompGoalCOGSPer() As Double
            Get
                Return _CompGoalCOGSPer
            End Get
            Set(ByVal value As Double)
                _CompGoalCOGSPer = value
            End Set
        End Property

        Private _CompGoalEstFreight As Integer = 0
        <DataMember()> _
        Public Property CompGoalEstFreight() As Integer
            Get
                Return _CompGoalEstFreight
            End Get
            Set(ByVal value As Integer)
                _CompGoalEstFreight = value
            End Set
        End Property

        Private _CompGoalEstFrtPer As Double = 0
        <DataMember()> _
        Public Property CompGoalEstFrtPer() As Double
            Get
                Return _CompGoalEstFrtPer
            End Get
            Set(ByVal value As Double)
                _CompGoalEstFrtPer = value
            End Set
        End Property

        Private _CompGoalEstControlledFrt As Integer = 0
        <DataMember()> _
        Public Property CompGoalEstControlledFrt() As Integer
            Get
                Return _CompGoalEstControlledFrt
            End Get
            Set(ByVal value As Integer)
                _CompGoalEstControlledFrt = value
            End Set
        End Property

        Private _CompGoalEstControlledPer As Double = 0
        <DataMember()> _
        Public Property CompGoalEstControlledPer() As Double
            Get
                Return _CompGoalEstControlledPer
            End Get
            Set(ByVal value As Double)
                _CompGoalEstControlledPer = value
            End Set
        End Property

        Private _CompGoalEstSavings As Integer = 0
        <DataMember()> _
        Public Property CompGoalEstSavings() As Integer
            Get
                Return _CompGoalEstSavings
            End Get
            Set(ByVal value As Integer)
                _CompGoalEstSavings = value
            End Set
        End Property

        Private _CompGoalEstSavPer As Double = 0
        <DataMember()> _
        Public Property CompGoalEstSavPer() As Double
            Get
                Return _CompGoalEstSavPer
            End Get
            Set(ByVal value As Double)
                _CompGoalEstSavPer = value
            End Set
        End Property

        Private _CompGoalModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompGoalModDate() As System.Nullable(Of Date)
            Get
                Return _CompGoalModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompGoalModDate = value
            End Set
        End Property

        Private _CompGoalModUser As String = ""
        <DataMember()> _
        Public Property CompGoalModUser() As String
            Get
                Return Left(_CompGoalModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompGoalModUser = Left(value, 100)
            End Set
        End Property

        Private _CompGoalUpdated As Byte()
        <DataMember()> _
        Public Property CompGoalUpdated() As Byte()
            Get
                Return _CompGoalUpdated
            End Get
            Set(ByVal value As Byte())
                _CompGoalUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompGoal
            instance = DirectCast(MemberwiseClone(), CompGoal)
            Return instance
        End Function

#End Region

    End Class
End Namespace