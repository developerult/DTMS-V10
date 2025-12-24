Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompTrack
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompTrackControl As Integer = 0
        <DataMember()> _
        Public Property CompTrackControl() As Integer
            Get
                Return _CompTrackControl
            End Get
            Set(ByVal value As Integer)
                _CompTrackControl = value
            End Set
        End Property

        Private _CompTrackCompControl As Integer = 0
        <DataMember()> _
        Public Property CompTrackCompControl() As Integer
            Get
                Return _CompTrackCompControl
            End Get
            Set(ByVal value As Integer)
                _CompTrackCompControl = value
            End Set
        End Property

        Private _CompTrackDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompTrackDate() As System.Nullable(Of Date)
            Get
                Return _CompTrackDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompTrackDate = value
            End Set
        End Property

        Private _CompTrackContact As String = ""
        <DataMember()> _
        Public Property CompTrackContact() As String
            Get
                Return Left(_CompTrackContact, 50)
            End Get
            Set(ByVal value As String)
                _CompTrackContact = Left(value, 50)
            End Set
        End Property

        Private _CompTrackBetween As String = ""
        <DataMember()> _
        Public Property CompTrackBetween() As String
            Get
                Return Left(_CompTrackBetween, 50)
            End Get
            Set(ByVal value As String)
                _CompTrackBetween = Left(value, 50)
            End Set
        End Property

        Private _CompTrackRegards As String = ""
        <DataMember()> _
        Public Property CompTrackRegards() As String
            Get
                Return Left(_CompTrackRegards, 50)
            End Get
            Set(ByVal value As String)
                _CompTrackRegards = Left(value, 50)
            End Set
        End Property

        Private _CompTrackFollowUpOn As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompTrackFollowUpOn() As System.Nullable(Of Date)
            Get
                Return _CompTrackFollowUpOn
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompTrackFollowUpOn = value
            End Set
        End Property

        Private _CompTrackComment As String = ""
        <DataMember()> _
        Public Property CompTrackComment() As String
            Get
                Return Left(_CompTrackComment, 255)
            End Get
            Set(ByVal value As String)
                _CompTrackComment = Left(value, 255)
            End Set
        End Property

        Private _CompTrackFollowUpOnComplete As Boolean = False
        <DataMember()> _
        Public Property CompTrackFollowUpOnComplete() As Boolean
            Get
                Return _CompTrackFollowUpOnComplete
            End Get
            Set(ByVal value As Boolean)
                _CompTrackFollowUpOnComplete = value
            End Set
        End Property

        Private _CompTrackCompletionDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompTrackCompletionDate() As System.Nullable(Of Date)
            Get
                Return _CompTrackCompletionDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompTrackCompletionDate = value
            End Set
        End Property

        Private _CompTrackModUser As String = ""
        <DataMember()> _
        Public Property CompTrackModUser() As String
            Get
                Return Left(_CompTrackModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompTrackModUser = Left(value, 100)
            End Set
        End Property

        Private _CompTrackModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompTrackModDate() As System.Nullable(Of Date)
            Get
                Return _CompTrackModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompTrackModDate = value
            End Set
        End Property

        Private _CompTrackUpdated As Byte()
        <DataMember()> _
        Public Property CompTrackUpdated() As Byte()
            Get
                Return _CompTrackUpdated
            End Get
            Set(ByVal value As Byte())
                _CompTrackUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompTrack
            instance = DirectCast(MemberwiseClone(), CompTrack)
            Return instance
        End Function

#End Region

    End Class
End Namespace
