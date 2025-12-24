Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookTrack
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

        Private _BookTrackCommentLocalized As String = ""
        <DataMember()> _
        Public Property BookTrackCommentLocalized() As String
            Get
                Return Left(_BookTrackCommentLocalized, 255)
            End Get
            Set(ByVal value As String)
                _BookTrackCommentLocalized = Left(value, 255)
            End Set
        End Property

        Private _BookTrackCommentKeys As String = ""
        <DataMember()> _
        Public Property BookTrackCommentKeys() As String
            Get
                Return Left(_BookTrackCommentKeys, 4000)
            End Get
            Set(ByVal value As String)
                _BookTrackCommentKeys = Left(value, 4000)
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

        Private _BookTrackUpdated As Byte()
        <DataMember()>
        Public Property BookTrackUpdated() As Byte()
            Get
                Return _BookTrackUpdated
            End Get
            Set(ByVal value As Byte())
                _BookTrackUpdated = value
            End Set
        End Property


        'Added By LVV on 9/17/19 for Bing Maps
        Private _Details As New List(Of BookTrackDetail)



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookTrack
            instance = DirectCast(MemberwiseClone(), BookTrack)
            Return instance
        End Function

        ''' <summary>
        ''' Gets the associated list of BookTrackDetails.
        ''' If list is nothing create and return a new list
        ''' (will never return null)
        ''' </summary>
        ''' <returns>List(Of BookTrackDetail)</returns>
        ''' <remarks>
        ''' Added By LVV on 9/17/19 for Bing Maps
        ''' </remarks>
        Public Function getDetails() As List(Of BookTrackDetail)
            If _Details Is Nothing Then _Details = New List(Of BookTrackDetail)
            Return _Details
        End Function

        ''' <summary>
        ''' Sets the associated List of BookTrackDetails to the provided parameter object.
        ''' </summary>
        ''' <param name="list">The object with which to set the current list of BookTrackDetails. If the value is null a new list will be created (will never set property to null)</param>
        ''' <remarks>
        ''' Added By LVV on 9/17/19 for Bing Maps
        ''' </remarks>
        Public Sub setDetails(ByVal list As List(Of BookTrackDetail))
            If list Is Nothing Then list = New List(Of BookTrackDetail)
            _Details = list
        End Sub

        ''' <summary>
        ''' Adds the parameter detail to the associated List of BookTrackDetails
        ''' </summary>
        ''' <param name="detail">The object to be added to the end of the current list of BookTrackDetails</param>
        ''' <remarks>
        ''' Added By LVV on 9/17/19 for Bing Maps
        ''' </remarks>
        Public Sub addDetail(ByVal detail As BookTrackDetail)
            If _Details Is Nothing Then _Details = New List(Of BookTrackDetail)
            _Details.Add(detail)
        End Sub

#End Region

    End Class
End Namespace