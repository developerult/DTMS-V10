
Imports System.Runtime.Serialization

<DataContract()> _
Public Class WCFParameters
    Private _Database As String = ""
    <DataMember()> _
    Public Property Database() As String
        Get
            If String.IsNullOrEmpty(_Database) Then _Database = ""
            Return _Database
        End Get
        Set(ByVal value As String)
            _Database = value
        End Set
    End Property

    Private _DBServer As String = ""
    <DataMember()> _
    Public Property DBServer() As String
        Get
            If String.IsNullOrEmpty(_DBServer) Then _DBServer = ""
            Return _DBServer
        End Get
        Set(ByVal value As String)
            _DBServer = value
        End Set
    End Property

    Private _ConnectionString As String = ""
    <DataMember()> _
    Public Property ConnectionString() As String
        Get
            If String.IsNullOrEmpty(_ConnectionString) Then _ConnectionString = ""
            Return _ConnectionString
        End Get
        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property

    Private _UserName As String = ""
    <DataMember()>
    Public Property UserName() As String
        Get
            If String.IsNullOrEmpty(_UserName) Then _UserName = ""
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property

    Private _UserControl As Integer = 0
    <DataMember()>
    Public Property UserControl() As Integer
        Get
            Return _UserControl
        End Get
        Set(ByVal value As Integer)
            _UserControl = value
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

    Private _UserRemotePassword As String = ""
    <DataMember()> _
    Public Property UserRemotePassword() As String
        Get
            If String.IsNullOrEmpty(_UserRemotePassword) Then _UserRemotePassword = ""
            Return _UserRemotePassword
        End Get
        Set(ByVal value As String)
            _UserRemotePassword = value
        End Set
    End Property

    Private _WCFAuthCode As String = ""
    <DataMember()> _
    Public Property WCFAuthCode() As String
        Get
            If String.IsNullOrEmpty(_WCFAuthCode) Then _WCFAuthCode = ""
            Return _WCFAuthCode
        End Get
        Set(ByVal value As String)
            _WCFAuthCode = value
        End Set
    End Property

    Private _WCFServiceURL As String = ""
    <DataMember()> _
    Public Property WCFServiceURL() As String
        Get
            Return _WCFServiceURL
        End Get
        Set(ByVal value As String)
            _WCFServiceURL = value

        End Set
    End Property

    Private _CompControl As Integer = 0
    <DataMember()> _
    Public Property CompControl() As Integer
        Get
            Return _CompControl
        End Get
        Set(ByVal value As Integer)
            _CompControl = value
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

    Private _FormName As String = ""
    <DataMember()> _
    Public Property FormName() As String
        Get
            If String.IsNullOrEmpty(_FormName) Then _FormName = ""
            Return _FormName
        End Get
        Set(ByVal value As String)
            _FormName = value
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

    Private _ProcedureName As String = ""
    <DataMember()> _
    Public Property ProcedureName() As String
        Get
            If String.IsNullOrEmpty(_ProcedureName) Then _ProcedureName = ""
            Return _ProcedureName
        End Get
        Set(ByVal value As String)
            _ProcedureName = value
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

    Private _ReportName As String = ""
    <DataMember()> _
    Public Property ReportName() As String
        Get
            If String.IsNullOrEmpty(_ReportName) Then _ReportName = ""
            Return _ReportName
        End Get
        Set(ByVal value As String)
            _ReportName = value
        End Set
    End Property

    Private _ValidateAccess As Boolean = False
    <DataMember()>
    Public Property ValidateAccess() As Boolean
        Get
            Return _ValidateAccess
        End Get
        Set(ByVal value As Boolean)
            _ValidateAccess = value
        End Set
    End Property

    Private _AuthenticationRequired As Boolean = True
    ''' <summary>
    ''' Boolean flag to identify if the user must log into the Acitive Directory for the SSOAName
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.0 on 3/22/2017
    '''     added to identify if SSO Azure Acitive Directory Authentication Update is required
    ''' </remarks>
    <DataMember()>
    Public Property AuthenticationRequired() As Boolean
        Get
            Return _AuthenticationRequired
        End Get
        Set(ByVal value As Boolean)
            _AuthenticationRequired = value
        End Set
    End Property

    Private _UseToken As Boolean = False
    <DataMember()>
    Public Property UseToken() As Boolean
        Get
            Return _UseToken
        End Get
        Set(ByVal value As Boolean)
            _UseToken = value
        End Set
    End Property

    Private _SSOAControl As Integer = 0
    <DataMember()>
    Public Property SSOAControl() As Integer
        Get
            Return _SSOAControl
        End Get
        Set(ByVal value As Integer)
            _SSOAControl = value
        End Set
    End Property

    Private _SSOAName As String = ""
    <DataMember()>
    Public Property SSOAName() As String
        Get
            If String.IsNullOrEmpty(_SSOAName) Then _SSOAName = ""
            Return Left(_SSOAName, 500)
        End Get
        Set(ByVal value As String)
            _SSOAName = Left(value, 500)
        End Set
    End Property

    Private _SSOAClientID As String = ""
    <DataMember()>
    Public Property SSOAClientID() As String
        Get
            If String.IsNullOrEmpty(_SSOAClientID) Then _SSOAClientID = ""
            Return Left(_SSOAClientID, 500)
        End Get
        Set(ByVal value As String)
            _SSOAClientID = Left(value, 500)
        End Set
    End Property

    Private _SSOALoginURL As String = ""
    <DataMember()>
    Public Property SSOALoginURL() As String
        Get
            If String.IsNullOrEmpty(_SSOALoginURL) Then _SSOALoginURL = ""
            Return Left(_SSOALoginURL, 1000)
        End Get
        Set(ByVal value As String)
            _SSOALoginURL = Left(value, 1000)
        End Set
    End Property

    Private _SSOARedirectURL As String = ""
    <DataMember()>
    Public Property SSOARedirectURL() As String
        Get
            If String.IsNullOrEmpty(_SSOARedirectURL) Then _SSOARedirectURL = ""
            Return Left(_SSOARedirectURL, 1000)
        End Get
        Set(ByVal value As String)
            _SSOARedirectURL = Left(value, 1000)
        End Set
    End Property

    Private _SSOAClientSecret As String = ""
    <DataMember()> _
    Public Property SSOAClientSecret() As String
        Get
            If String.IsNullOrEmpty(_SSOAClientSecret) Then _SSOAClientSecret = ""
            Return Left(_SSOAClientSecret, 500)
        End Get
        Set(ByVal value As String)
            _SSOAClientSecret = Left(value, 500)
        End Set
    End Property

    Private _USATToken As String = ""
    <DataMember()> _
    Public Property USATToken() As String
        Get
            If String.IsNullOrEmpty(_USATToken) Then _USATToken = ""
            Return _USATToken
        End Get
        Set(ByVal value As String)
            _USATToken = value
        End Set
    End Property

    Private _USATUserID As String
    <DataMember()>
    Public Property USATUserID() As String
        Get
            Return Left(Me._USATUserID, 255)
        End Get
        Set(value As String)
            If (String.Equals(Me._USATUserID, value) = False) Then
                Me._USATUserID = Left(value, 255)
            End If
        End Set
    End Property

    Private _USATExpires As Date
    Public Property USATExpires() As Date
        Get
            Return _USATExpires
        End Get
        Set(ByVal value As Date)
            _USATExpires = value
        End Set
    End Property

    Private _UseExceptionEvents As Boolean = False
    <DataMember()>
    Public Property UseExceptionEvents() As Boolean
        Get
            Return _UseExceptionEvents
        End Get
        Set(ByVal value As Boolean)
            _UseExceptionEvents = value
        End Set
    End Property

    'Added By LVV 7/31/17 for v-8.0 TMS365
    Private _IsUserCarrier As Boolean = False
    <DataMember()>
    Public Property IsUserCarrier() As Boolean
        Get
            Return _IsUserCarrier
        End Get
        Set(ByVal value As Boolean)
            _IsUserCarrier = value
        End Set
    End Property

    Private _UserCarrierControl As Integer = 0
    <DataMember()>
    Public Property UserCarrierControl() As Integer
        Get
            Return _UserCarrierControl
        End Get
        Set(ByVal value As Integer)
            _UserCarrierControl = value
        End Set
    End Property

    Private _UserCarrierContControl As Integer
    Public Property UserCarrierContControl() As Integer
        Get
            Return _UserCarrierContControl
        End Get
        Set(ByVal value As Integer)
            _UserCarrierContControl = value
        End Set
    End Property

    Private _UserLEControl As Integer = 0
    <DataMember()>
    Public Property UserLEControl() As Integer
        Get
            Return _UserLEControl
        End Get
        Set(ByVal value As Integer)
            _UserLEControl = value
        End Set
    End Property

    Private _UserEmail As String = ""
    <DataMember()>
    Public Property UserEmail() As String
        Get
            If String.IsNullOrEmpty(_UserEmail) Then _UserEmail = ""
            Return _UserEmail
        End Get
        Set(ByVal value As String)
            _UserEmail = value
        End Set
    End Property

    Private _CatControl As Integer = 0
    <DataMember()>
    Public Property CatControl() As Integer
        Get
            Return _CatControl
        End Get
        Set(ByVal value As Integer)
            _CatControl = value
        End Set
    End Property



    Public Function DBInfo() As String
        Return "Server: " & Me.DBServer & vbCrLf & "Database: " & Me.Database
    End Function

    'TODO - Add rest of properties
    Public Function CloneParameters() As WCFParameters
        Dim WCFProperty As New WCFParameters
        CloneParameters(WCFProperty)
        Return WCFProperty
    End Function

    Public Sub CloneParameters(ByRef WCFProperty As WCFParameters)
        WCFProperty.Database = Database
        WCFProperty.DBServer = DBServer
        WCFProperty.ConnectionString = Me.ConnectionString
        WCFProperty.UserName = UserName
        WCFProperty.UserControl = Me.UserControl
        WCFProperty.UserRemotePassword = UserRemotePassword
        WCFProperty.WCFAuthCode = WCFAuthCode
        WCFProperty.WCFServiceURL = Me.WCFServiceURL
        WCFProperty.CompControl = CompControl
        WCFProperty.FormControl = FormControl
        WCFProperty.FormName = FormName
        WCFProperty.ProcedureControl = ProcedureControl
        WCFProperty.ProcedureName = ProcedureName
        WCFProperty.ReportControl = ReportControl
        WCFProperty.ReportName = ReportName
        WCFProperty.ValidateAccess = ValidateAccess
        WCFProperty.AuthenticationRequired = Me.AuthenticationRequired
        WCFProperty.UseToken = Me.UseToken
        WCFProperty.SSOAControl = Me.SSOAControl
        WCFProperty.SSOAName = Me.SSOAName
        WCFProperty.SSOAClientID = Me.SSOAClientID
        WCFProperty.SSOALoginURL = Me.SSOALoginURL
        WCFProperty.SSOARedirectURL = Me.SSOARedirectURL
        WCFProperty.SSOAClientSecret = Me.SSOAClientSecret
        WCFProperty.USATToken = Me.USATToken
        WCFProperty.USATUserID = Me.USATUserID
        WCFProperty.USATExpires = Me.USATExpires
        WCFProperty.UseExceptionEvents = Me.UseExceptionEvents
        WCFProperty.IsUserCarrier = Me.IsUserCarrier
        WCFProperty.UserCarrierControl = Me.UserCarrierControl
        WCFProperty.UserLEControl = Me.UserLEControl
        WCFProperty.UserEmail = Me.UserEmail
        WCFProperty.CatControl = Me.CatControl
        WCFProperty.UserUserGroupsControl = Me.UserUserGroupsControl
    End Sub

End Class
