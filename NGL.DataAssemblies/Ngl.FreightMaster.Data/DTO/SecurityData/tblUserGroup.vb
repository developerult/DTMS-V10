Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblUserGroup
        Inherits DTOBaseClass

#Region " Data Members"

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

        Private _UserGroupsAltControl As Integer = 0
        <DataMember()> _
        Public Property UserGroupsAltControl() As Integer
            Get
                Return _UserGroupsAltControl
            End Get
            Set(ByVal value As Integer)
                _UserGroupsAltControl = value
            End Set
        End Property


        Private _UserGroupsName As String = ""
        <DataMember()> _
        Public Property UserGroupsName() As String
            Get
                Return Left(_UserGroupsName, 100)
            End Get
            Set(ByVal value As String)
                _UserGroupsName = Left(value, 50)
            End Set
        End Property

        Private _UserGroupsDescription As String = ""
        <DataMember()> _
        Public Property UserGroupsDescription() As String
            Get
                Return Left(_UserGroupsDescription, 255)
            End Get
            Set(ByVal value As String)
                _UserGroupsDescription = Left(value, 255)
            End Set
        End Property

        Private _UserGroupsIcon As String = ""
        <DataMember()> _
        Public Property UserGroupsIcon() As String
            Get
                Return Left(_UserGroupsIcon, 100)
            End Get
            Set(ByVal value As String)
                _UserGroupsIcon = Left(value, 100)
            End Set
        End Property

        Private _UserGroupsUpdated As Byte()
        <DataMember()> _
        Public Property UserGroupsUpdated() As Byte()
            Get
                Return _UserGroupsUpdated
            End Get
            Set(ByVal value As Byte())
                _UserGroupsUpdated = value
            End Set
        End Property

        'Private _tblFormSecurityGroupXrefs As List(Of tblFormSecurityGroupXref)
        '<DataMember()> _
        'Public Property tblFormSecurityGroupXrefs() As List(Of tblFormSecurityGroupXref)
        '    Get
        '        Return _tblFormSecurityGroupXrefs
        '    End Get
        '    Set(ByVal value As List(Of tblFormSecurityGroupXref))
        '        _tblFormSecurityGroupXrefs = value
        '    End Set
        'End Property

        'Private _tblProcedureSecurityGroupXrefs As List(Of tblProcedureSecurityGroupXref)
        '<DataMember()> _
        'Public Property tblProcedureSecurityGroupXrefs() As List(Of tblProcedureSecurityGroupXref)
        '    Get
        '        Return _tblProcedureSecurityGroupXrefs
        '    End Get
        '    Set(ByVal value As List(Of tblProcedureSecurityGroupXref))
        '        _tblProcedureSecurityGroupXrefs = value
        '    End Set
        'End Property

        'Private _tblReportSecurityGroupXrefs As List(Of tblReportSecurityGroupXref)
        '<DataMember()> _
        'Public Property tblReportSecurityGroupXrefs() As List(Of tblReportSecurityGroupXref)
        '    Get
        '        Return _tblReportSecurityGroupXrefs
        '    End Get
        '    Set(ByVal value As List(Of tblReportSecurityGroupXref))
        '        _tblReportSecurityGroupXrefs = value
        '    End Set
        'End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblUserGroup
            instance = DirectCast(MemberwiseClone(), tblUserGroup)
            'instance.tblFormSecurityGroupXrefs = Nothing
            'For Each item In tblFormSecurityGroupXrefs
            '    instance.tblFormSecurityGroupXrefs.Add(DirectCast(item.Clone, tblFormSecurityGroupXref))
            'Next
            'instance.tblProcedureSecurityGroupXrefs = Nothing
            'For Each item In tblProcedureSecurityGroupXrefs
            '    instance.tblProcedureSecurityGroupXrefs.Add(DirectCast(item.Clone, tblProcedureSecurityGroupXref))
            'Next
            'instance.tblReportSecurityGroupXrefs = Nothing
            'For Each item In tblReportSecurityGroupXrefs
            '    instance.tblReportSecurityGroupXrefs.Add(DirectCast(item.Clone, tblReportSecurityGroupXref))
            'Next
            Return instance
        End Function

#End Region
    End Class
End Namespace
