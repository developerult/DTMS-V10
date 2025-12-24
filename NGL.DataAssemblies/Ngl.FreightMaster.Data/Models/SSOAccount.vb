Namespace Models

    ''' <summary>
    ''' Single Sign On Account Information Model From Database For the Current User
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
    ''' Note: this property is only used by the DTMS Web Authentication when the SSOAName = "NGL"
    ''' Modified by RHR for v-8.5.4.005 on 03/28/2024
    ''' </remarks>
    Public Class SSOAccount

        Private _SSOAControl As Integer
        ''' <summary>
        ''' zero for New users; system will look up control with SSOAName on New users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAControl() As Integer
            Get
                Return _SSOAControl
            End Get
            Set(ByVal value As Integer)
                _SSOAControl = value
            End Set
        End Property


        Private _SSOAName As String
        ''' <summary>
        ''' Use Web config SSOADefaultName for New users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAName() As String
            Get
                Return _SSOAName
            End Get
            Set(ByVal value As String)
                _SSOAName = value
            End Set
        End Property

        Private _SSOADesc As String
        ''' <summary>
        ''' Not required
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOADesc() As String
            Get
                Return _SSOADesc
            End Get
            Set(ByVal value As String)
                _SSOADesc = value
            End Set
        End Property


        Private _AllowNonPrimaryComputers As Boolean
        ''' <summary>
        ''' on new users this is True until the primary computer is configured.
        ''' on Trial Users only one computer is allowed per user
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property AllowNonPrimaryComputers() As Boolean
            Get
                Return _AllowNonPrimaryComputers
            End Get
            Set(ByVal value As Boolean)
                _AllowNonPrimaryComputers = value
            End Set
        End Property


        Private _AllowPublicComputer As Boolean = False
        ''' <summary>
        ''' on new users we do not allow Public Computers
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property AllowPublicComputer() As Boolean
            Get
                Return _AllowPublicComputer
            End Get
            Set(ByVal value As Boolean)
                _AllowPublicComputer = value
            End Set
        End Property

        Private _SSOAClientID As String
        ''' <summary>
        ''' use Web config idaClientId for New users; empty when using NGL Authentication
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAClientID() As String
            Get
                Return _SSOAClientID
            End Get
            Set(ByVal value As String)
                _SSOAClientID = value
            End Set
        End Property

        Private _SSOALoginURL As String
        ''' <summary>
        ''' use Web config idaInstance for New users; empty when using NGL Authentication
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOALoginURL() As String
            Get
                Return _SSOALoginURL
            End Get
            Set(ByVal value As String)
                _SSOALoginURL = value
            End Set
        End Property


        Private _SSOADataURL As String
        ''' <summary>
        ''' Not required
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOADataURL() As String
            Get
                Return _SSOADataURL
            End Get
            Set(ByVal value As String)
                _SSOADataURL = value
            End Set
        End Property


        Private _SSOARedirectURL As String
        ''' <summary>
        ''' use Web config WebBaseURI for New users; empty when using NGL Authentication
        ''' Provide a default value but typically generated dynamically at run time
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOARedirectURL() As String
            Get
                Return _SSOARedirectURL
            End Get
            Set(ByVal value As String)
                _SSOARedirectURL = value
            End Set
        End Property

        Private _SSSOAClientSecret As String
        ''' <summary>
        ''' Not Required empty when using NGL Authentication
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAClientSecret() As String
            Get
                Return _SSSOAClientSecret
            End Get
            Set(ByVal value As String)
                _SSSOAClientSecret = value
            End Set
        End Property

        Private _SSOAAuthCode As String
        ''' <summary>
        ''' maps to tenant use Web config idaTenant for New users;  default = common
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAAuthCode() As String
            Get
                Return _SSOAAuthCode
            End Get
            Set(ByVal value As String)
                _SSOAAuthCode = value
            End Set
        End Property

        Private _SSOAAuthenticationRequired As Boolean = False
        ''' <summary>
        ''' always true for New users.  Generally false when using NGL Authentication
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAAuthenticationRequired() As Boolean
            Get
                Return _SSOAAuthenticationRequired
            End Get
            Set(ByVal value As Boolean)
                _SSOAAuthenticationRequired = value
            End Set
        End Property

        Private _SSOAUserSecurityControl As Integer
        ''' <summary>
        ''' zero for New users.  Maps to tblUserSecurity.UserSecurityControl 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAUserSecurityControl() As Integer
            Get
                Return _SSOAUserSecurityControl
            End Get
            Set(ByVal value As Integer)
                _SSOAUserSecurityControl = value
            End Set
        End Property

        Private _SSOAUserName As String
        ''' <summary>
        ''' for New users this Is the HttpContext.Current.User.Identity
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAUserName() As String
            Get
                Return _SSOAUserName
            End Get
            Set(ByVal value As String)
                _SSOAUserName = value
            End Set
        End Property

        Private _SSOAUserEmail As String
        ''' <summary>
        ''' empty for New users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAUserEmail() As String
            Get
                Return _SSOAUserEmail
            End Get
            Set(ByVal value As String)
                _SSOAUserEmail = value
            End Set
        End Property

        Private _UserFriendlyName As String
        ''' <summary>
        ''' empty for new users maps to tblUserSecurity.UserFriendlyName
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property UserFriendlyName() As String
            Get
                Return _UserFriendlyName
            End Get
            Set(ByVal value As String)
                _UserFriendlyName = value
            End Set
        End Property

        Private _UserFirstName As String
        ''' <summary>
        ''' empty for new users maps to tblUserSecurity.UserFirstName
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property UserFirstName() As String
            Get
                Return _UserFirstName
            End Get
            Set(ByVal value As String)
                _UserFirstName = value
            End Set
        End Property

        Private _UserLastName As String
        ''' <summary>
        ''' empty for new users maps to tblUserSecurity.UserLastName
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property UserLastName() As String
            Get
                Return _UserLastName
            End Get
            Set(ByVal value As String)
                _UserLastName = value
            End Set
        End Property

        Private _AuthenticationErrorCode As Integer
        ''' <summary>
        ''' if Authentication Fails return an Error Code (typically bound to an enumerator)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property AuthenticationErrorCode() As Integer
            Get
                Return _AuthenticationErrorCode
            End Get
            Set(ByVal value As Integer)
                _AuthenticationErrorCode = value
            End Set
        End Property

        Private _AuthenticationErrorMessage As String
        ''' <summary>
        ''' Default Error Message returned in English if authentication fails.  Error Code may be used to lookup language specific message
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property AuthenticationErrorMessage() As String
            Get
                Return _AuthenticationErrorMessage
            End Get
            Set(ByVal value As String)
                _AuthenticationErrorMessage = value
            End Set
        End Property

        'Added By LVV 7/31/17 for v-8.0 TMS365
        Private _IsUserCarrier As Boolean = False
        Public Property IsUserCarrier() As Boolean
            Get
                Return _IsUserCarrier
            End Get
            Set(ByVal value As Boolean)
                _IsUserCarrier = value
            End Set
        End Property

        Private _UserCarrierControl As Integer
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

        Private _UserLEControl As Integer
        Public Property UserLEControl() As Integer
            Get
                Return _UserLEControl
            End Get
            Set(ByVal value As Integer)
                _UserLEControl = value
            End Set
        End Property

        Private _UserTheme365 As String
        Public Property UserTheme365() As String
            Get
                Return _UserTheme365
            End Get
            Set(ByVal value As String)
                _UserTheme365 = value
            End Set
        End Property

        Private _UserWorkPhone As String
        Public Property UserWorkPhone() As String
            Get
                Return _UserWorkPhone
            End Get
            Set(ByVal value As String)
                _UserWorkPhone = value
            End Set
        End Property

        Private _UserWorkPhoneExt As String
        Public Property UserWorkPhoneExt() As String
            Get
                Return _UserWorkPhoneExt
            End Get
            Set(ByVal value As String)
                _UserWorkPhoneExt = value
            End Set
        End Property

        Private _CatControl As Integer
        Public Property CatControl() As Integer
            Get
                Return _CatControl
            End Get
            Set(ByVal value As Integer)
                _CatControl = value
            End Set
        End Property

        Private _UserMustChangePassword As Boolean
        ''' <summary>
        ''' Administrators can select an option to force user to change their password
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR on 2023-01-25 new property populated from tblUserSecurity
        ''' </remarks>
        Public Property UserMustChangePassword() As Boolean
            Get
                Return _UserMustChangePassword
            End Get
            Set(ByVal value As Boolean)
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
                Return _UserCultureInfo
            End Get
            Set(ByVal value As String)
                _UserCultureInfo = Left(value, 100)
            End Set
        End Property



    End Class


End Namespace

