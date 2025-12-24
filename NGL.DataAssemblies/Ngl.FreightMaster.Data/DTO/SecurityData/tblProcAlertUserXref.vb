Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblProcAlertUserXref
        Inherits DTOBaseClass

#Region " Data Members"

        Private _ProcAlertUserXrefControl As Integer = 0
        <DataMember()> _
        Public Property ProcAlertUserXrefControl() As Integer
            Get
                Return _ProcAlertUserXrefControl
            End Get
            Set(ByVal value As Integer)
                _ProcAlertUserXrefControl = value
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

        'Added By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes
        Private _ProcAlertUserXrefShowPopup As Integer = 0
        <DataMember()> _
        Public Property ProcAlertUserXrefShowPopup() As Integer
            Get
                Return _ProcAlertUserXrefShowPopup
            End Get
            Set(ByVal value As Integer)
                _ProcAlertUserXrefShowPopup = value
            End Set
        End Property

        'Added By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes
        Private _ProcAlertUserXrefSendEmail As Integer = 0
        <DataMember()> _
        Public Property ProcAlertUserXrefSendEmail() As Integer
            Get
                Return _ProcAlertUserXrefSendEmail
            End Get
            Set(ByVal value As Integer)
                _ProcAlertUserXrefSendEmail = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblProcAlertUserXref
            instance = DirectCast(MemberwiseClone(), tblProcAlertUserXref)
            instance.tblProcedureLists = Nothing
            For Each item In tblProcedureLists
                instance.tblProcedureLists.Add(DirectCast(item.Clone, tblProcedureList))
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
