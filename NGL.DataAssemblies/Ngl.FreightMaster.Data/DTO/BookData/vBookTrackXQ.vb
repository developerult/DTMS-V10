Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vBookTrackXQ
        Inherits DTOBaseClass


#Region " Data Members"

        Private _BookTrackControl As Integer = 0
        <DataMember()> _
        Public Property BookTrackControl() As Integer
            Get
                Return _BookTrackControl
            End Get
            Set(ByVal value As Integer)
                _BookTrackControl = value
            End Set
        End Property

        Private _BookTrackBookControl As Integer = 0
        <DataMember()> _
        Public Property BookTrackBookControl() As Integer
            Get
                Return _BookTrackBookControl
            End Get
            Set(ByVal value As Integer)
                _BookTrackBookControl = value
            End Set
        End Property

        Private _BookTrackDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookTrackDate() As System.Nullable(Of Date)
            Get
                Return _BookTrackDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookTrackDate = value
            End Set
        End Property

        Private _BookTrackContact As String = ""
        <DataMember()> _
        Public Property BookTrackContact() As String
            Get
                Return Left(_BookTrackContact, 50)
            End Get
            Set(ByVal value As String)
                _BookTrackContact = Left(value, 50)
            End Set
        End Property

        Private _BookTrackComment As String = ""
        <DataMember()> _
        Public Property BookTrackComment() As String
            Get
                Return Left(_BookTrackComment, 255)
            End Get
            Set(ByVal value As String)
                _BookTrackComment = Left(value, 255)
            End Set
        End Property

        Private _BookTrackStatus As Integer = 0
        <DataMember()> _
        Public Property BookTrackStatus() As Integer
            Get
                Return _BookTrackStatus
            End Get
            Set(ByVal value As Integer)
                _BookTrackStatus = value
            End Set
        End Property

        Private _BookTrackModUser As String = ""
        <DataMember()> _
        Public Property BookTrackModUser() As String
            Get
                Return Left(_BookTrackModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookTrackModUser = Left(value, 100)
            End Set
        End Property

        Private _BookTrackModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookTrackModDate() As System.Nullable(Of Date)
            Get
                Return _BookTrackModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookTrackModDate = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New vBookTrackXQ
            instance = DirectCast(MemberwiseClone(), vBookTrackXQ)
            Return instance
        End Function

#End Region

    End Class
End Namespace