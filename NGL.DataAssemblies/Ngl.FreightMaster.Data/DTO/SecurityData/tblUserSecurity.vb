Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    ''' <summary>
    ''' DTO object for tblUserSecurity information used primarily by WCF services
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Modified by RHR for v-8.5.4.005 on 03/28/2024 added timezone and culture
    ''' </remarks>
    <DataContract()>
    Public Class tblUserSecurity
        Inherits DTOBaseClass

#Region " Data Members"

        Private _UserSecurityControl As Integer = 0
        <DataMember()>
        Public Property UserSecurityControl() As Integer
            Get
                Return _UserSecurityControl
            End Get
            Set(ByVal value As Integer)
                _UserSecurityControl = value
            End Set
        End Property

        Private _UserName As String = ""
        <DataMember()>
        Public Property UserName() As String
            Get
                Return Left(_UserName, 100)
            End Get
            Set(ByVal value As String)
                _UserName = Left(value, 100)
            End Set
        End Property

        Private _UserEmail As String = ""
        <DataMember()>
        Public Property UserEmail() As String
            Get
                Return Left(_UserEmail, 255)
            End Get
            Set(ByVal value As String)
                _UserEmail = Left(value, 255)
            End Set
        End Property

        Private _UserFriendlyName As String = ""
        <DataMember()>
        Public Property UserFriendlyName() As String
            Get
                Return Left(_UserFriendlyName, 100)
            End Get
            Set(ByVal value As String)
                _UserFriendlyName = Left(value, 100)
            End Set
        End Property

        Private _UserFirstName As String = ""
        <DataMember()>
        Public Property UserFirstName() As String
            Get
                Return Left(_UserFirstName, 100)
            End Get
            Set(ByVal value As String)
                _UserFirstName = Left(value, 100)
            End Set
        End Property

        Private _UserMiddleIn As String = ""
        <DataMember()>
        Public Property UserMiddleIn() As String
            Get
                Return Left(_UserMiddleIn, 2)
            End Get
            Set(ByVal value As String)
                _UserMiddleIn = Left(value, 2)
            End Set
        End Property

        Private _UserLastName As String = ""
        <DataMember()>
        Public Property UserLastName() As String
            Get
                Return Left(_UserLastName, 100)
            End Get
            Set(ByVal value As String)
                _UserLastName = Left(value, 100)
            End Set
        End Property

        Private _UserTitle As String = ""
        <DataMember()>
        Public Property UserTitle() As String
            Get
                Return Left(_UserTitle, 100)
            End Get
            Set(ByVal value As String)
                _UserTitle = Left(value, 100)
            End Set
        End Property

        Private _UserDepartment As String = ""
        <DataMember()>
        Public Property UserDepartment() As String
            Get
                Return Left(_UserDepartment, 255)
            End Get
            Set(ByVal value As String)
                _UserDepartment = Left(value, 255)
            End Set
        End Property

        Private _UserPhoneWork As String = ""
        <DataMember()>
        Public Property UserPhoneWork() As String
            Get
                Return Left(_UserPhoneWork, 20)
            End Get
            Set(ByVal value As String)
                _UserPhoneWork = Left(value, 20)
            End Set
        End Property

        Private _UserPhoneCell As String = ""
        <DataMember()>
        Public Property UserPhoneCell() As String
            Get
                Return Left(_UserPhoneCell, 20)
            End Get
            Set(ByVal value As String)
                _UserPhoneCell = Left(value, 20)
            End Set
        End Property

        Private _UserPhoneHome As String = ""
        <DataMember()>
        Public Property UserPhoneHome() As String
            Get
                Return Left(_UserPhoneHome, 20)
            End Get
            Set(ByVal value As String)
                _UserPhoneHome = Left(value, 20)
            End Set
        End Property

        Private _UserWorkExt As String = ""
        <DataMember()>
        Public Property UserWorkExt() As String
            Get
                Return Left(_UserWorkExt, 20)
            End Get
            Set(ByVal value As String)
                _UserWorkExt = Left(value, 20)
            End Set
        End Property

        Private _UseAuthCode As String = ""
        <DataMember()>
        Public Property UseAuthCode() As String
            Get
                Return Left(_UseAuthCode, 500)
            End Get
            Set(ByVal value As String)
                _UseAuthCode = Left(value, 500)
            End Set
        End Property

        Private _UserRemotePassword As String = ""
        <DataMember()>
        Public Property UserRemotePassword() As String
            Get
                Return Left(_UserRemotePassword, 100)
            End Get
            Set(ByVal value As String)
                _UserRemotePassword = Left(value, 100)
            End Set
        End Property

        Private _UserUpdated As Byte()
        <DataMember()>
        Public Property UserUpdated() As Byte()
            Get
                Return _UserUpdated
            End Get
            Set(ByVal value As Byte())
                _UserUpdated = value
            End Set
        End Property


        Private _UserGroupsName As String = ""
        <DataMember()>
        Public Property UserGroupsName() As String
            Get
                Return _UserGroupsName
            End Get
            Set(ByVal value As String)
                _UserGroupsName = value
            End Set
        End Property

        Private _UserUserGroupsControl As Integer = 0
        <DataMember()>
        Public Property UserUserGroupsControl() As Integer
            Get
                Return _UserUserGroupsControl
            End Get
            Set(ByVal value As Integer)
                _UserUserGroupsControl = value
            End Set
        End Property

        'Private _tblFormSecurityXrefs As List(Of tblFormSecurityXref)
        '<DataMember()> _
        'Public Property tblFormSecurityXrefs() As List(Of tblFormSecurityXref)
        '    Get
        '        Return _tblFormSecurityXrefs
        '    End Get
        '    Set(ByVal value As List(Of tblFormSecurityXref))
        '        _tblFormSecurityXrefs = value
        '    End Set
        'End Property

        'Private _tblProcedureSecurityXrefs As List(Of tblProcedureSecurityXref)
        '<DataMember()> _
        'Public Property tblProcedureSecurityXrefs() As List(Of tblProcedureSecurityXref)
        '    Get
        '        Return _tblProcedureSecurityXrefs
        '    End Get
        '    Set(ByVal value As List(Of tblProcedureSecurityXref))
        '        _tblProcedureSecurityXrefs = value
        '    End Set
        'End Property

        'Private _tblReportSecurityXrefs As List(Of tblReportSecurityXref)
        '<DataMember()> _
        'Public Property tblReportSecurityXrefs() As List(Of tblReportSecurityXref)
        '    Get
        '        Return _tblReportSecurityXrefs
        '    End Get
        '    Set(ByVal value As List(Of tblReportSecurityXref))
        '        _tblReportSecurityXrefs = value
        '    End Set
        'End Property

        'Private _tblUserAdmins As List(Of tblUserAdmin)
        '<DataMember()> _
        'Public Property tblUserAdmins() As List(Of tblUserAdmin)
        '    Get
        '        Return _tblUserAdmins
        '    End Get
        '    Set(ByVal value As List(Of tblUserAdmin))
        '        _tblUserAdmins = value
        '    End Set
        'End Property

        Private _NEXTrackOnly As Boolean = False
        <DataMember()>
        Public Property NEXTrackOnly() As Boolean
            Get
                Return _NEXTrackOnly
            End Get
            Set(ByVal value As Boolean)
                _NEXTrackOnly = value
            End Set
        End Property


        'Added by LVV for v-8.0 on 04/12/2017 TMS 365

        Private _UserSSOAControl As Integer = 0
        <DataMember()>
        Public Property UserSSOAControl() As Integer
            Get
                Return _UserSSOAControl
            End Get
            Set(ByVal value As Integer)
                _UserSSOAControl = value
            End Set
        End Property

        Private _UserStartFreeTrial As System.Nullable(Of Date)
        <DataMember()>
        Public Property UserStartFreeTrial() As System.Nullable(Of Date)
            Get
                Return _UserStartFreeTrial
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _UserStartFreeTrial = value
            End Set
        End Property

        Private _UserEndFreeTrial As System.Nullable(Of Date)
        <DataMember()>
        Public Property UserEndFreeTrial() As System.Nullable(Of Date)
            Get
                Return _UserEndFreeTrial
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _UserEndFreeTrial = value
            End Set
        End Property

        Private _UserFreeTrialActive As Boolean = False
        <DataMember()>
        Public Property UserFreeTrialActive() As Boolean
            Get
                Return _UserFreeTrialActive
            End Get
            Set(ByVal value As Boolean)
                _UserFreeTrialActive = value
            End Set
        End Property

        Private _UserTheme365 As String = ""
        <DataMember()>
        Public Property UserTheme365() As String
            Get
                Return Left(_UserTheme365, 50)
            End Get
            Set(ByVal value As String)
                _UserTheme365 = Left(value, 50)
            End Set
        End Property


        Private _UserMustChangePassword As Boolean?
        ''' <summary>
        ''' Administrators can select an option to force user to change their password
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR on 2023-01-25 new property populated from tblUserSecurity
        ''' </remarks>
        <DataMember()>
        Public Property UserMustChangePassword() As Boolean?
            Get
                Return _UserMustChangePassword
            End Get
            Set(ByVal value As Boolean?)
                _UserMustChangePassword = value
            End Set
        End Property



        Private _UserCultureInfo As String
        ''' <summary>
        ''' User Culture Information like en-us
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.4.005 on 03/28/2024
        ''' </remarks>
        Public Property UserCultureInfo() As String
            Get
                Return _UserCultureInfo
            End Get
            Set(ByVal value As String)
                _UserCultureInfo = Left(value, 100)
            End Set
        End Property

        Private _UserTimeZone As String
        ''' <summary>
        ''' User Time Zone Information like Central Time
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.5.4.005 on 03/28/2024
        ''' </remarks>
        Public Property UserTimeZone() As String
            Get
                Return _UserTimeZone
            End Get
            Set(ByVal value As String)
                _UserTimeZone = Left(value, 100)
            End Set
        End Property




#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblUserSecurity
            instance = DirectCast(MemberwiseClone(), tblUserSecurity)
            Return instance
        End Function

#End Region
    End Class
End Namespace