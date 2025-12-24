Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblUserAdmin
        Inherits DTOBaseClass

#Region " Data Members"

        Private _UserAdminControl As Integer = 0
        <DataMember()> _
        Public Property UserAdminControl() As Integer
            Get
                Return _UserAdminControl
            End Get
            Set(ByVal value As Integer)
                _UserAdminControl = value
            End Set
        End Property

        Private _UserSecurityControl As Integer = 0
        <DataMember()> _
        Public Property UserSecurityControl() As Integer
            Get
                Return _UserSecurityControl
            End Get
            Set(ByVal value As Integer)
                _UserSecurityControl = value
            End Set
        End Property

        Private _UserAdminCompControl As Integer = 0
        <DataMember()> _
        Public Property UserAdminCompControl() As Integer
            Get
                Return _UserAdminCompControl
            End Get
            Set(ByVal value As Integer)
                _UserAdminCompControl = value
            End Set
        End Property

        Private _UserName As String = ""
        <DataMember()> _
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return _CompName
            End Get
            Set(ByVal value As String)
                _CompName = value
            End Set
        End Property


        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _UserAdminUpdated As Byte()
        <DataMember()> _
        Public Property UserAdminUpdated() As Byte()
            Get
                Return _UserAdminUpdated
            End Get
            Set(ByVal value As Byte())
                _UserAdminUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblUserAdmin
            instance = DirectCast(MemberwiseClone(), tblUserAdmin)
            Return instance
        End Function

#End Region
    End Class
End Namespace
