Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblFormSecurityXref
        Inherits DTOBaseClass


#Region " Data Members"

        Private _FormSecurityXrefControl As Integer = 0
        <DataMember()> _
        Public Property FormSecurityXrefControl() As Integer
            Get
                Return _FormSecurityXrefControl
            End Get
            Set(ByVal value As Integer)
                _FormSecurityXrefControl = value
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

        Private _FormControl As Integer = 0
        <DataMember()> _
        Public Property FormControl() As Integer
            Get
                Return _FormControl
            End Get
            Set(ByVal value As Integer)
                _FormControl = value
            End Set
        End Property

        Private _tblFormLists As List(Of tblFormList)
        <DataMember()> _
        Public Property tblFormLists() As List(Of tblFormList)
            Get
                Return _tblFormLists
            End Get
            Set(ByVal value As List(Of tblFormList))
                _tblFormLists = value
            End Set
        End Property

        Private _tblUserSecuritys As List(Of tblUserSecurity)
        <DataMember()> _
        Public Property tblUserSecuritys() As List(Of tblUserSecurity)
            Get
                Return _tblUserSecuritys
            End Get
            Set(ByVal value As List(Of tblUserSecurity))
                _tblUserSecuritys = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblFormSecurityXref
            instance = DirectCast(MemberwiseClone(), tblFormSecurityXref)
            instance.tblFormLists = Nothing
            For Each item In tblFormLists
                instance.tblFormLists.Add(DirectCast(item.Clone, tblFormList))
            Next
            instance.tblUserSecuritys = Nothing
            For Each item In tblUserSecuritys
                instance.tblUserSecuritys.Add(DirectCast(item.Clone, tblUserSecurity))
            Next
            Return instance
        End Function

#End Region
    End Class
End Namespace
