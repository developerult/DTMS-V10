Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblDistanceQueueDetail
        Inherits DTOBaseClass


#Region " Data Members"

        Private _DistanceQueueDetailControl As Integer = 0
        <DataMember()> _
        Public Property DistanceQueueDetailControl() As Integer
            Get
                Return Me._DistanceQueueDetailControl
            End Get
            Set(value As Integer)
                If ((Me._DistanceQueueDetailControl = value) _
                   = False) Then
                    Me._DistanceQueueDetailControl = value
                    Me.SendPropertyChanged("DistanceQueueDetailControl")
                End If
            End Set
        End Property

        Private _DistanceQueueDetailDistanceQueueControl As Integer = 0
        <DataMember()> _
        Public Property DistanceQueueDetailDistanceQueueControl() As Integer
            Get
                Return Me._DistanceQueueDetailDistanceQueueControl
            End Get
            Set(value As Integer)
                If ((Me._DistanceQueueDetailDistanceQueueControl = value) _
                   = False) Then
                    Me._DistanceQueueDetailDistanceQueueControl = value
                    Me.SendPropertyChanged("DistanceQueueDetailDistanceQueueControl")
                End If
            End Set
        End Property

        Private _DistanceQueueDetailStopControl As Integer = 0
        <DataMember()> _
        Public Property DistanceQueueDetailStopControl() As Integer
            Get
                Return Me._DistanceQueueDetailStopControl
            End Get
            Set(value As Integer)
                If ((Me._DistanceQueueDetailStopControl = value) _
                   = False) Then
                    Me._DistanceQueueDetailStopControl = value
                    Me.SendPropertyChanged("DistanceQueueDetailStopControl")
                End If
            End Set
        End Property

        Private _DistanceQueueDetailModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DistanceQueueDetailModDate() As System.Nullable(Of Date)
            Get
                Return Me._DistanceQueueDetailModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._DistanceQueueDetailModDate.Equals(value) = False) Then
                    Me._DistanceQueueDetailModDate = value
                    Me.SendPropertyChanged("DistanceQueueDetailModDate")
                End If
            End Set
        End Property

        Private _DistanceQueueDetailModUser As String = ""
        <DataMember()> _
        Public Property DistanceQueueDetailModUser() As String
            Get
                Return Left(Me._DistanceQueueDetailModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._DistanceQueueDetailModUser, value) = False) Then
                    Me._DistanceQueueDetailModUser = Left(value, 100)
                    Me.SendPropertyChanged("DistanceQueueDetailModUser")
                End If
            End Set
        End Property

        Private _DistanceQueueDetailUpdated As Byte()
        <DataMember()> _
        Public Property DistanceQueueDetailUpdated() As Byte()
            Get
                Return Me._DistanceQueueDetailUpdated
            End Get
            Set(value As Byte())
                Me._DistanceQueueDetailUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblDistanceQueueDetail
            instance = DirectCast(MemberwiseClone(), tblDistanceQueueDetail)
            Return instance
        End Function

#End Region

    End Class
End Namespace
