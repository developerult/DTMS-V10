Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblStopDistance
        Inherits DTOBaseClass


#Region " Data Members"


        Private _StopDistanceControl As Integer = 0
        <DataMember()> _
        Public Property StopDistanceControl() As Integer
            Get
                Return Me._StopDistanceControl
            End Get
            Set(value As Integer)
                If ((Me._StopDistanceControl = value) _
                   = False) Then
                    Me._StopDistanceControl = value
                    Me.SendPropertyChanged("StopDistanceControl")
                End If
            End Set
        End Property

        Private _StopDistanceFromStopControl As Integer = 0
        <DataMember()> _
        Public Property StopDistanceFromStopControl() As Integer
            Get
                Return Me._StopDistanceFromStopControl
            End Get
            Set(value As Integer)
                If ((Me._StopDistanceFromStopControl = value) _
                   = False) Then
                    Me._StopDistanceFromStopControl = value
                    Me.SendPropertyChanged("StopDistanceFromStopControl")
                End If
            End Set
        End Property

        Private _StopDistanceToStopControl As Integer = 0
        <DataMember()> _
        Public Property StopDistanceToStopControl() As Integer
            Get
                Return Me._StopDistanceToStopControl
            End Get
            Set(value As Integer)
                If ((Me._StopDistanceToStopControl = value) _
                   = False) Then
                    Me._StopDistanceToStopControl = value
                    Me.SendPropertyChanged("StopDistanceToStopControl")
                End If
            End Set
        End Property

        Private _StopDistanceRoadMiles As Double = 0
        <DataMember()> _
        Public Property StopDistanceRoadMiles() As Double
            Get
                Return Me._StopDistanceRoadMiles
            End Get
            Set(value As Double)
                If ((Me._StopDistanceRoadMiles = value) _
                   = False) Then
                    Me._StopDistanceRoadMiles = value
                    Me.SendPropertyChanged("StopDistanceRoadMiles")
                End If
            End Set
        End Property

        Private _StopDistanceDirectMiles As Double = 0
        <DataMember()> _
        Public Property StopDistanceDirectMiles() As Double
            Get
                Return Me._StopDistanceDirectMiles
            End Get
            Set(value As Double)
                If ((Me._StopDistanceDirectMiles = value) _
                   = False) Then
                    Me._StopDistanceDirectMiles = value
                    Me.SendPropertyChanged("StopDistanceDirectMiles")
                End If
            End Set
        End Property

        Private _StopDistanceModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property StopDistanceModDate() As System.Nullable(Of Date)
            Get
                Return Me._StopDistanceModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._StopDistanceModDate.Equals(value) = False) Then
                    Me._StopDistanceModDate = value
                    Me.SendPropertyChanged("StopDistanceModDate")
                End If
            End Set
        End Property

        Private _StopDistanceModUser As String = ""
        <DataMember()> _
        Public Property StopDistanceModUser() As String
            Get
                Return Left(Me._StopDistanceModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._StopDistanceModUser, value) = False) Then
                    Me._StopDistanceModUser = Left(value, 100)
                    Me.SendPropertyChanged("StopDistanceModUser")
                End If
            End Set
        End Property

        Private _StopDistanceUpdated As Byte()
        <DataMember()> _
        Public Property StopDistanceUpdated() As Byte()
            Get
                Return Me._StopDistanceUpdated
            End Get
            Set(value As Byte())
                Me._StopDistanceUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblStopDistance
            instance = DirectCast(MemberwiseClone(), tblStopDistance)
            Return instance
        End Function

#End Region

    End Class
End Namespace