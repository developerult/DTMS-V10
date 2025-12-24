Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblDistanceQueue
        Inherits DTOBaseClass


#Region " Data Members"

        Private _DistanceQueueControl As Integer = 0
        <DataMember()> _
        Public Property DistanceQueueControl() As Integer
            Get
                Return Me._DistanceQueueControl
            End Get
            Set(value As Integer)
                If ((Me._DistanceQueueControl = value) _
                   = False) Then
                    Me._DistanceQueueControl = value
                    Me.SendPropertyChanged("DistanceQueueControl")
                End If
            End Set
        End Property


        Private _DistanceQueueStartDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DistanceQueueStartDate() As System.Nullable(Of Date)
            Get
                Return Me._DistanceQueueStartDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._DistanceQueueStartDate.Equals(value) = False) Then
                    Me._DistanceQueueStartDate = value
                    Me.SendPropertyChanged("DistanceQueueStartDate")
                End If
            End Set
        End Property

        Private _DistanceQueueEndDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DistanceQueueEndDate() As System.Nullable(Of Date)
            Get
                Return Me._DistanceQueueEndDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._DistanceQueueEndDate.Equals(value) = False) Then
                    Me._DistanceQueueEndDate = value
                    Me.SendPropertyChanged("DistanceQueueEndDate")
                End If
            End Set
        End Property

        Private _DistanceQueueComplete As Boolean = False
        <DataMember()> _
        Public Property DistanceQueueComplete() As Boolean
            Get
                Return Me._DistanceQueueComplete
            End Get
            Set(value As Boolean)
                If ((Me._DistanceQueueComplete = value) _
                   = False) Then
                    Me._DistanceQueueComplete = value
                    Me.SendPropertyChanged("DistanceQueueComplete")
                End If
            End Set
        End Property


        Private _DistanceQueueMessage As String = ""
        <DataMember()> _
        Public Property DistanceQueueMessage() As String
            Get
                Return Left(Me._DistanceQueueMessage, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._DistanceQueueMessage, value) = False) Then
                    Me._DistanceQueueMessage = Left(value, 4000)
                    Me.SendPropertyChanged("DistanceQueueMessage")
                End If
            End Set
        End Property


        Private _DistanceQueueRunAll As Boolean = False
        <DataMember()> _
        Public Property DistanceQueueRunAll() As Boolean
            Get
                Return Me._DistanceQueueRunAll
            End Get
            Set(value As Boolean)
                If ((Me._DistanceQueueRunAll = value) _
                   = False) Then
                    Me._DistanceQueueRunAll = value
                    Me.SendPropertyChanged("DistanceQueueRunAll")
                End If
            End Set
        End Property

        Private _DistanceQueueUseLatLong As Boolean = False
        <DataMember()> _
        Public Property DistanceQueueUseLatLong() As Boolean
            Get
                Return Me._DistanceQueueUseLatLong
            End Get
            Set(value As Boolean)
                If ((Me._DistanceQueueUseLatLong = value) _
                   = False) Then
                    Me._DistanceQueueUseLatLong = value
                    Me.SendPropertyChanged("DistanceQueueUseLatLong")
                End If
            End Set
        End Property

        Private _DistanceQueueModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DistanceQueueModDate() As System.Nullable(Of Date)
            Get
                Return Me._DistanceQueueModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._DistanceQueueModDate.Equals(value) = False) Then
                    Me._DistanceQueueModDate = value
                    Me.SendPropertyChanged("DistanceQueueModDate")
                End If
            End Set
        End Property

        Private _DistanceQueueModUser As String = ""
        <DataMember()> _
        Public Property DistanceQueueModUser() As String
            Get
                Return Left(Me._DistanceQueueModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._DistanceQueueModUser, value) = False) Then
                    Me._DistanceQueueModUser = Left(value, 100)
                    Me.SendPropertyChanged("DistanceQueueModUser")
                End If
            End Set
        End Property


        Private _DistanceQueueUpdated As Byte()
        <DataMember()> _
        Public Property DistanceQueueUpdated() As Byte()
            Get
                Return Me._DistanceQueueUpdated
            End Get
            Set(value As Byte())
                Me._DistanceQueueUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblDistanceQueue
            instance = DirectCast(MemberwiseClone(), tblDistanceQueue)
            Return instance
        End Function

#End Region

    End Class
End Namespace