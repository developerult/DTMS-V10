Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblReportSecurityGroupXref
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ReportSecurityGroupXrefControl As Integer = 0
        <DataMember()> _
        Public Property ReportSecurityGroupXrefControl() As Integer
            Get
                Return _ReportSecurityGroupXrefControl
            End Get
            Set(ByVal value As Integer)
                _ReportSecurityGroupXrefControl = value
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

        Private _ReportControl As Integer = 0
        <DataMember()> _
        Public Property ReportControl() As Integer
            Get
                Return _ReportControl
            End Get
            Set(ByVal value As Integer)
                _ReportControl = value
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

        Private _tblReportLists As List(Of tblReportList)
        <DataMember()> _
        Public Property tblReportLists() As List(Of tblReportList)
            Get
                Return _tblReportLists
            End Get
            Set(ByVal value As List(Of tblReportList))
                _tblReportLists = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblReportSecurityGroupXref
            instance = DirectCast(MemberwiseClone(), tblReportSecurityGroupXref)
            instance.tblUserGroups = Nothing
            For Each item In tblUserGroups
                instance.tblUserGroups.Add(DirectCast(item.Clone, tblUserGroup))
            Next
            instance.tblReportLists = Nothing
            For Each item In tblReportLists
                instance.tblReportLists.Add(DirectCast(item.Clone, tblReportList))
            Next
            Return instance
        End Function

#End Region
    End Class
End Namespace