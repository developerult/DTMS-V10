Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookNote
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookNotesControl As Integer = 0
        <DataMember()> _
        Public Property BookNotesControl() As Integer
            Get
                Return _BookNotesControl
            End Get
            Set(ByVal value As Integer)
                _BookNotesControl = value
            End Set
        End Property

        Private _BookNotesBookControl As Integer = 0
        <DataMember()> _
        Public Property BookNotesBookControl() As Integer
            Get
                Return _BookNotesBookControl
            End Get
            Set(ByVal value As Integer)
                _BookNotesBookControl = value
            End Set
        End Property

        Private _BookNotesVisable1 As String = ""
        <DataMember()> _
        Public Property BookNotesVisable1() As String
            Get
                Return Left(_BookNotesVisable1, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesVisable1 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesVisable2 As String = ""
        <DataMember()> _
        Public Property BookNotesVisable2() As String
            Get
                Return Left(_BookNotesVisable2, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesVisable2 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesVisable3 As String = ""
        <DataMember()> _
        Public Property BookNotesVisable3() As String
            Get
                Return Left(_BookNotesVisable3, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesVisable3 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesVisable4 As String = ""
        <DataMember()> _
        Public Property BookNotesVisable4() As String
            Get
                Return Left(_BookNotesVisable4, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesVisable4 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesVisable5 As String = ""
        <DataMember()> _
        Public Property BookNotesVisable5() As String
            Get
                Return Left(_BookNotesVisable5, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesVisable5 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesConfidential1 As String = ""
        <DataMember()> _
        Public Property BookNotesConfidential1() As String
            Get
                Return Left(_BookNotesConfidential1, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesConfidential1 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesConfidential2 As String = ""
        <DataMember()> _
        Public Property BookNotesConfidential2() As String
            Get
                Return Left(_BookNotesConfidential2, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesConfidential2 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesConfidential3 As String = ""
        <DataMember()> _
        Public Property BookNotesConfidential3() As String
            Get
                Return Left(_BookNotesConfidential3, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesConfidential3 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesConfidential4 As String = ""
        <DataMember()> _
        Public Property BookNotesConfidential4() As String
            Get
                Return Left(_BookNotesConfidential4, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesConfidential4 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesConfidential5 As String = ""
        <DataMember()> _
        Public Property BookNotesConfidential5() As String
            Get
                Return Left(_BookNotesConfidential5, 250)
            End Get
            Set(ByVal value As String)
                _BookNotesConfidential5 = Left(value, 250)
            End Set
        End Property

        Private _BookNotesBookUser1 As String = ""
        <DataMember()> _
        Public Property BookNotesBookUser1() As String
            Get
                Return Left(_BookNotesBookUser1, 4000)
            End Get
            Set(ByVal value As String)
                _BookNotesBookUser1 = Left(value, 4000)
            End Set
        End Property

        Private _BookNotesBookUser2 As String = ""
        <DataMember()> _
        Public Property BookNotesBookUser2() As String
            Get
                Return Left(_BookNotesBookUser2, 4000)
            End Get
            Set(ByVal value As String)
                _BookNotesBookUser2 = Left(value, 4000)
            End Set
        End Property

        Private _BookNotesBookUser3 As String = ""
        <DataMember()> _
        Public Property BookNotesBookUser3() As String
            Get
                Return Left(_BookNotesBookUser3, 4000)
            End Get
            Set(ByVal value As String)
                _BookNotesBookUser3 = Left(value, 4000)
            End Set
        End Property

        Private _BookNotesBookUser4 As String = ""
        <DataMember()> _
        Public Property BookNotesBookUser4() As String
            Get
                Return Left(_BookNotesBookUser4, 4000)
            End Get
            Set(ByVal value As String)
                _BookNotesBookUser4 = Left(value, 4000)
            End Set
        End Property

        Private _BookNotesModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookNotesModDate() As System.Nullable(Of Date)
            Get
                Return _BookNotesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookNotesModDate = value
            End Set
        End Property

        Private _BookNotesModUser As String = ""
        <DataMember()> _
        Public Property BookNotesModUser() As String
            Get
                Return Left(_BookNotesModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookNotesModUser = Left(value, 100)
            End Set
        End Property

        Private _BookNotesUpdated As Byte()
        <DataMember()> _
        Public Property BookNotesUpdated() As Byte()
            Get
                Return _BookNotesUpdated
            End Get
            Set(ByVal value As Byte())
                _BookNotesUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookNote
            instance = DirectCast(MemberwiseClone(), BookNote)
            Return instance
        End Function

#End Region

    End Class
End Namespace