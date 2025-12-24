Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ClaimTrack
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ClaimTrackControl As Integer = 0

        Private _ClaimTrackClaimControl As Integer = 0

        Private _ClaimTrackDate As System.Nullable(Of Date)

        Private _ClaimTrackContact As String = ""

        Private _ClaimTrackComment As String = ""

        Private _ClaimTrackModUser As String = ""

        Private _ClaimTrackModDate As System.Nullable(Of Date)

        Private _ClaimTrackUpdated As Byte()

        <DataMember()> _
        Public Property ClaimTrackControl() As Integer
            Get
                Return Me._ClaimTrackControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimTrackControl = value) = False) Then
                    Me._ClaimTrackControl = value
                    Me.SendPropertyChanged("ClaimTrackControl")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimTrackClaimControl() As Integer
            Get
                Return Me._ClaimTrackClaimControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._ClaimTrackClaimControl = value) = False) Then
                    Me._ClaimTrackClaimControl = value
                    Me.SendPropertyChanged("ClaimTrackClaimControl")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimTrackDate() As System.Nullable(Of Date)
            Get
                Return Me._ClaimTrackDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ClaimTrackDate.Equals(value) = False) Then
                    Me._ClaimTrackDate = value
                    Me.SendPropertyChanged("ClaimTrackDate")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimTrackContact() As String
            Get
                Return Left(Me._ClaimTrackContact, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimTrackContact, value) = False) Then
                    Me._ClaimTrackContact = Left(value, 50)
                    Me.SendPropertyChanged("ClaimTrackContact")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimTrackComment() As String
            Get
                Return Left(Me._ClaimTrackComment, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimTrackComment, value) = False) Then
                    Me._ClaimTrackComment = Left(value, 255)
                    Me.SendPropertyChanged("ClaimTrackComment")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimTrackModUser() As String
            Get
                Return Left(Me._ClaimTrackModUser, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ClaimTrackModUser, value) = False) Then
                    Me._ClaimTrackModUser = Left(value, 100)
                    Me.SendPropertyChanged("ClaimTrackModUser")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimTrackModDate() As System.Nullable(Of Date)
            Get
                Return Me._ClaimTrackModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ClaimTrackModDate.Equals(value) = False) Then
                    Me._ClaimTrackModDate = value
                    Me.SendPropertyChanged("ClaimTrackModDate")
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property ClaimTrackUpdated() As Byte()
            Get
                Return Me._ClaimTrackUpdated
            End Get
            Set(ByVal value As Byte())
                _ClaimTrackUpdated = value
                Me.SendPropertyChanged("ClaimTrackUpdated")
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ClaimTrack
            instance = DirectCast(MemberwiseClone(), ClaimTrack)
            Return instance
        End Function

#End Region

    End Class

End Namespace