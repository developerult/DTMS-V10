Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrTrack
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrTrackControl As Integer = 0
        <DataMember()> _
        Public Property CarrTrackControl() As Integer
            Get
                Return _CarrTrackControl
            End Get
            Set(ByVal value As Integer)
                _CarrTrackControl = value
            End Set
        End Property

        Private _CarrTrackCompControl As Integer = 0
        <DataMember()> _
        Public Property CarrTrackCompControl() As Integer
            Get
                Return _CarrTrackCompControl
            End Get
            Set(ByVal value As Integer)
                _CarrTrackCompControl = value
            End Set
        End Property

        Private _CarrTrackDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTrackDate() As System.Nullable(Of Date)
            Get
                Return _CarrTrackDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrTrackDate = value
            End Set
        End Property


        Private _CarrTrackContact As String = ""
        <DataMember()> _
        Public Property CarrTrackContact() As String
            Get
                Return Left(_CarrTrackContact, 50)
            End Get
            Set(ByVal value As String)
                _CarrTrackContact = Left(value, 50)
            End Set
        End Property

        Private _CarrTrackComment As String = ""
        <DataMember()> _
        Public Property CarrTrackComment() As String
            Get
                Return Left(_CarrTrackComment, 255)
            End Get
            Set(ByVal value As String)
                _CarrTrackComment = Left(value, 255)
            End Set
        End Property

        Private _CarrTrackFollowUpOn As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTrackFollowUpOn() As System.Nullable(Of Date)
            Get
                Return _CarrTrackFollowUpOn
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrTrackFollowUpOn = value
            End Set
        End Property


        Private _carrTrackFollowUpOnComplete As Boolean = False
        <DataMember()> _
        Public Property carrTrackFollowUpOnComplete() As Boolean
            Get
                Return _carrTrackFollowUpOnComplete
            End Get
            Set(ByVal value As Boolean)
                _carrTrackFollowUpOnComplete = value
            End Set
        End Property

        Private _CarrTrackModUser As String = ""
        <DataMember()> _
        Public Property CarrTrackModUser() As String
            Get
                Return Left(_CarrTrackModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrTrackModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrTrackModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrTrackModDate() As System.Nullable(Of Date)
            Get
                Return _CarrTrackModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrTrackModDate = value
            End Set
        End Property


        Private _CarrTrackUpdated As Byte()
        <DataMember()> _
        Public Property CarrTrackUpdated() As Byte()
            Get
                Return _CarrTrackUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrTrackUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrTrack
            instance = DirectCast(MemberwiseClone(), CarrTrack)
            Return instance
        End Function

#End Region

    End Class
End Namespace