Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblReportSecurityXref
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ReportSecurityXrefControl As Integer = 0
        <DataMember()> _
        Public Property ReportSecurityXrefControl() As Integer
            Get
                Return _ReportSecurityXrefControl
            End Get
            Set(ByVal value As Integer)
                _ReportSecurityXrefControl = value
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
            Dim instance As New tblReportSecurityXref
            instance = DirectCast(MemberwiseClone(), tblReportSecurityXref)
            instance.tblReportLists = Nothing
            For Each item In tblReportLists
                instance.tblReportLists.Add(DirectCast(item.Clone, tblReportList))
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