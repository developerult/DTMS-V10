Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblFormSecurityGroupXref
        Inherits DTOBaseClass

#Region " Data Members"

        Private _FormSecurityGroupXrefControl As Integer = 0
        <DataMember()> _
        Public Property FormSecurityGroupXrefControl() As Integer
            Get
                Return _FormSecurityGroupXrefControl
            End Get
            Set(ByVal value As Integer)
                _FormSecurityGroupXrefControl = value
            End Set
        End Property

        Private _UserGroupsControl As Integer = 0
        <DataMember()> _
        Public Property UserGroupsControl() As Integer
            Get
                Return _UserGroupsControl
            End Get
            Set(ByVal value As Integer)
                _UserGroupsControl = value
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

        Private _tblUserGroups As List(Of tblUserGroup)
        <DataMember()> _
        Public Property tblUserGroups() As List(Of tblUserGroup)
            Get
                Return _tblUserGroups
            End Get
            Set(ByVal value As List(Of tblUserGroup))
                _tblUserGroups = value
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

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblFormSecurityGroupXref
            instance = DirectCast(MemberwiseClone(), tblFormSecurityGroupXref)
            instance.tblUserGroups = Nothing
            For Each item In tblUserGroups
                instance.tblUserGroups.Add(DirectCast(item.Clone, tblUserGroup))
            Next
            instance.tblFormLists = Nothing
            For Each item In tblFormLists
                instance.tblFormLists.Add(DirectCast(item.Clone, tblFormList))
            Next
            Return instance
        End Function


#End Region
    End Class
End Namespace
