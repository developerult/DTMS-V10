Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompCont
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompContControl As Integer = 0
        <DataMember()> _
        Public Property CompContControl() As Integer
            Get
                Return _CompContControl
            End Get
            Set(ByVal value As Integer)
                _CompContControl = value
            End Set
        End Property

        Private _CompContCompControl As Integer = 0
        <DataMember()> _
        Public Property CompContCompControl() As Integer
            Get
                Return _CompContCompControl
            End Get
            Set(ByVal value As Integer)
                _CompContCompControl = value
            End Set
        End Property

        Private _CompContName As String = ""
        <DataMember()> _
        Public Property CompContName() As String
            Get
                Return Left(_CompContName, 25)
            End Get
            Set(ByVal value As String)
                _CompContName = Left(value, 25)
            End Set
        End Property

        Private _CompContTitle As String = ""
        <DataMember()> _
        Public Property CompContTitle() As String
            Get
                Return Left(_CompContTitle, 25)
            End Get
            Set(ByVal value As String)
                _CompContTitle = Left(value, 25)
            End Set
        End Property

        Private _CompCont800 As String = ""
        <DataMember()> _
        Public Property CompCont800() As String
            Get
                Return Left(_CompCont800, 50)
            End Get
            Set(ByVal value As String)
                _CompCont800 = Left(value, 50)
            End Set
        End Property

        Private _CompContPhone As String = ""
        <DataMember()> _
        Public Property CompContPhone() As String
            Get
                Return Left(_CompContPhone, 15)
            End Get
            Set(ByVal value As String)
                _CompContPhone = Left(value, 15)
            End Set
        End Property

        Private _CompContPhoneExt As String = ""
        <DataMember()> _
        Public Property CompContPhoneExt() As String
            Get
                Return Left(_CompContPhoneExt, 5)
            End Get
            Set(ByVal value As String)
                _CompContPhoneExt = Left(value, 5)
            End Set
        End Property

        Private _CompContFax As String = ""
        <DataMember()> _
        Public Property CompContFax() As String
            Get
                Return Left(_CompContFax, 15)
            End Get
            Set(ByVal value As String)
                _CompContFax = Left(value, 15)
            End Set
        End Property

        Private _CompContEmail As String = ""
        <DataMember()> _
        Public Property CompContEmail() As String
            Get
                Return Left(_CompContEmail, 50)
            End Get
            Set(ByVal value As String)
                _CompContEmail = Left(value, 50)
            End Set
        End Property

        Private _CompContTender As Boolean = False
        <DataMember()> _
        Public Property CompContTender() As Boolean
            Get
                Return _CompContTender
            End Get
            Set(ByVal value As Boolean)
                _CompContTender = value
            End Set
        End Property

        Private _CompContUpdated As Byte()
        <DataMember()> _
        Public Property CompContUpdated() As Byte()
            Get
                Return _CompContUpdated
            End Get
            Set(ByVal value As Byte())
                _CompContUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompCont
            instance = DirectCast(MemberwiseClone(), CompCont)
            Return instance
        End Function

#End Region

    End Class
End Namespace