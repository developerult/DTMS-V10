Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblProcedureList
        Inherits DTOBaseClass


#Region " Data Members"

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


        Private _ProcedureName As String = ""
        <DataMember()> _
        Public Property ProcedureName() As String
            Get
                Return Left(_ProcedureName, 50)
            End Get
            Set(ByVal value As String)
                _ProcedureName = Left(value, 50)
            End Set
        End Property

        Private _ProcedureDescription As String = ""
        <DataMember()> _
        Public Property ProcedureDescription() As String
            Get
                Return Left(_ProcedureDescription, 50)
            End Get
            Set(ByVal value As String)
                _ProcedureDescription = Left(value, 50)
            End Set
        End Property

        Private _ProcedureHasAlert As Boolean = False
        <DataMember()> _
        Public Property ProcedureHasAlert As Boolean
            Get
                Return _ProcedureHasAlert
            End Get
            Set(value As Boolean)
                _ProcedureHasAlert = value
            End Set
        End Property

        Private _ProcedureUpdated As Byte()
        <DataMember()> _
        Public Property ProcedureUpdated() As Byte()
            Get
                Return _ProcedureUpdated
            End Get
            Set(ByVal value As Byte())
                _ProcedureUpdated = value
            End Set
        End Property

        Private _ProcedureSecurityXrefControl As Integer = 0
        <DataMember()> _
        Public Property ProcedureSecurityXrefControl() As Integer
            Get
                Return _ProcedureSecurityXrefControl
            End Get
            Set(ByVal value As Integer)
                _ProcedureSecurityXrefControl = value
            End Set
        End Property

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


        Private _ProcedureUserOverrideGroup As Boolean = False
        <DataMember()> _
        Public Property ProcedureUserOverrideGroup() As Boolean
            Get
                Return _ProcedureUserOverrideGroup
            End Get
            Set(ByVal value As Boolean)
                _ProcedureUserOverrideGroup = value
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

        'Added By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes
        Private _strPopup As String = ""
        <DataMember()> _
        Public Property strPopup() As String
            Get
                Return _strPopup
            End Get
            Set(ByVal value As String)
                _strPopup = value
            End Set
        End Property

        'Added By LVV on 11/28/16 for v-7.0.5.110 Subscription Alert Changes
        Private _strEmail As String = ""
        <DataMember()> _
        Public Property strEmail() As String
            Get
                Return _strEmail
            End Get
            Set(ByVal value As String)
                _strEmail = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblProcedureList
            instance = DirectCast(MemberwiseClone(), tblProcedureList)
            Return instance
        End Function

#End Region

    End Class
End Namespace
