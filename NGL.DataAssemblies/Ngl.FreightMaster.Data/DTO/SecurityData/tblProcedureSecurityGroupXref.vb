Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblProcedureSecurityGroupXref
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ProcedureSecurityGroupXrefControl As Integer = 0
        <DataMember()> _
        Public Property ProcedureSecurityGroupXrefControl() As Integer
            Get
                Return _ProcedureSecurityGroupXrefControl
            End Get
            Set(ByVal value As Integer)
                _ProcedureSecurityGroupXrefControl = value
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

        Private _ProcedureControl As Integer = 0
        <DataMember()> _
        Public Property ProcedureControl() As Integer
            Get
                Return _ProcedureControl
            End Get
            Set(ByVal value As Integer)
                _ProcedureControl = value
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

        Private _tblProcedureLists As List(Of tblProcedureList)
        <DataMember()> _
        Public Property tblProcedureLists() As List(Of tblProcedureList)
            Get
                Return _tblProcedureLists
            End Get
            Set(ByVal value As List(Of tblProcedureList))
                _tblProcedureLists = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblProcedureSecurityGroupXref
            instance = DirectCast(MemberwiseClone(), tblProcedureSecurityGroupXref)
            instance.tblUserGroups = Nothing
            For Each item In tblUserGroups
                instance.tblUserGroups.Add(DirectCast(item.Clone, tblUserGroup))
            Next
            instance.tblProcedureLists = Nothing
            For Each item In tblProcedureLists
                instance.tblProcedureLists.Add(DirectCast(item.Clone, tblProcedureList))
            Next
            Return instance
        End Function

#End Region
    End Class
End Namespace