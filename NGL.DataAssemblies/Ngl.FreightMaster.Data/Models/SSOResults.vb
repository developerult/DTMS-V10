Namespace Models

    ''' <summary>
    ''' Single Sign On Results Model From OAuth2 Data
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.005 on 03/28/2024
    ''' </remarks>
    Public Class SSOResults
        ''' <summary>
        ''' on new users we ask if this is a public/private computer and if this is a private or public computer
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        '''Modified by RHR for v-8.5.2.006 added tblUserSecurity.UserMustChangePassword property
        ''' Note: this property is only used by the DTMS Web Authentication when the SSOAName = "NGL"
        ''' </remarks>
        Public Property PrimaryComputer() As Boolean
            Get
                Return m_PrimaryComputer
            End Get
            Set
                m_PrimaryComputer = Value
            End Set
        End Property

        Private m_PrimaryComputer As Boolean

        ''' <summary>
        ''' on new users we ask if this is a public/private computer and if this is a private or public computer
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property PublicComputer() As Boolean
            Get
                Return m_PublicComputer
            End Get
            Set
                m_PublicComputer = Value
            End Set
        End Property
        Private m_PublicComputer As Boolean

        ''' <summary>
        ''' zero for new users; system will look up control with SSOAName on new users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAControl() As Integer
            Get
                Return m_SSOAControl
            End Get
            Set
                m_SSOAControl = Value
            End Set
        End Property
        Private m_SSOAControl As Integer

        ''' <summary>
        ''' use Web config SSOADefaultName for new users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAName() As String
            Get
                Return m_SSOAName
            End Get
            Set
                m_SSOAName = Value
            End Set
        End Property
        Private m_SSOAName As String

        ''' <summary>
        ''' use Web config idaClientId for new users; empty when using NGL Authentication
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAClientID() As String
            Get
                Return m_SSOAClientID
            End Get
            Set
                m_SSOAClientID = Value
            End Set
        End Property
        Private m_SSOAClientID As String

        ''' <summary>
        ''' use Web config idaInstance for new users; empty when using NGL Authentication
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOALoginURL() As String
            Get
                Return m_SSOALoginURL
            End Get
            Set
                m_SSOALoginURL = Value
            End Set
        End Property
        Private m_SSOALoginURL As String

        ''' <summary>
        ''' use Web config WebBaseURI for new users; empty when using NGL Authentication
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOARedirectURL() As String
            Get
                Return m_SSOARedirectURL
            End Get
            Set
                m_SSOARedirectURL = Value
            End Set
        End Property
        Private m_SSOARedirectURL As String

        ''' <summary>
        ''' empty when using NGL Authentication
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAClientSecret() As String
            Get
                Return m_SSOAClientSecret
            End Get
            Set
                m_SSOAClientSecret = Value
            End Set
        End Property
        Private m_SSOAClientSecret As String

        ''' <summary>
        ''' use tenant use Web config idaTenant for new users;  default = common
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAAuthCode() As String
            Get
                Return m_SSOAAuthCode
            End Get
            Set
                m_SSOAAuthCode = Value
            End Set
        End Property
        Private m_SSOAAuthCode As String

        ''' <summary>
        ''' maps to tblUserSecurity.UserSecurityControl if zero this is a new user and a new record needs to be created 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property UserSecurityControl() As Integer
            Get
                Return m_UserSecurityControl
            End Get
            Set
                m_UserSecurityControl = Value
            End Set
        End Property
        Private m_UserSecurityControl As Integer

        ''' <summary>
        ''' maps to tblUserSecurity.UserName for new users this maps to HttpContext.Current.User.Identity if this is the primary computer and it is private not public 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property UserName() As String
            Get
                Return m_UserName
            End Get
            Set
                m_UserName = Value
            End Set
        End Property
        Private m_UserName As String

        ''' <summary>
        ''' maps to tblUserSecurity.UserLastName from user.profile.family_name on new users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property UserLastName() As String
            Get
                Return m_UserLastName
            End Get
            Set
                m_UserLastName = Value
            End Set
        End Property
        Private m_UserLastName As String

        ''' <summary>
        ''' maps to tblUserSecurity.UserFirstName from user.profile.given_name on new users and also maps to and tblUserSecurity.UserFriendlyName on new users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property UserFirstName() As String
            Get
                Return m_UserFirstName
            End Get
            Set
                m_UserFirstName = Value
            End Set
        End Property
        Private m_UserFirstName As String

        ''' <summary>
        ''' maps to tblUserSecurityAccessToken.USATUserID user.profile.unique_name on new users if PirmaryComputer = false or PublicComputer = true maps to tblUserSecurity.UserName
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property USATUserID() As String
            Get
                Return m_USATUserID
            End Get
            Set
                m_USATUserID = Value
            End Set
        End Property
        Private m_USATUserID As String

        ''' <summary>
        ''' maps to tblUserSecurityAccessToken.USATToken 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by LVV for v-8.0 on 4/12/2017 TMS 365
        ''' </remarks>
        Public Property USATToken() As String
            Get
                Return m_USATToken
            End Get
            Set
                m_USATToken = Value
            End Set
        End Property
        Private m_USATToken As String

        ''' <summary>
        ''' maps to tblUserSecurity.UserEmail from user.profile.email on new users
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAUserEmail() As String
            Get
                Return m_SSOAUserEmail
            End Get
            Set
                m_SSOAUserEmail = Value
            End Set
        End Property
        Private m_SSOAUserEmail As String


        ''' <summary>
        ''' maps to user.profile.exp: number of seconds after January 1, 1970 (1970-01-01T0:0:0Z) UTC  when the token expires
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAExpires() As Integer
            Get
                Return m_SSOAExpires
            End Get
            Set
                m_SSOAExpires = Value
            End Set
        End Property
        Private m_SSOAExpires As Integer

        ''' <summary>
        ''' Javascript milliseconds expiration timestamp
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' created by RHR for v-8.2 on 09/01/2018
        ''' </remarks>
        Public Property SSOAExpiresMilli() As Int64
            Get
                Return m_SSOAExpiresMilli
            End Get
            Set
                m_SSOAExpiresMilli = Value
            End Set
        End Property
        Private m_SSOAExpiresMilli As Int64

        ''' <summary>
        ''' maps to user.profile.iat: number of seconds after January 1, 1970 (1970-01-01T0:0:0Z) UTC  when the token was issued
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Property SSOAIssuedAtTime() As Integer
            Get
                Return m_SSOAIssuedAtTime
            End Get
            Set
                m_SSOAIssuedAtTime = Value
            End Set
        End Property
        Private m_SSOAIssuedAtTime As Integer
        '
        'Note: tblUserSecurityAccessToken.USATExpires maps to Expiration time formula: current datetime + (SSOAExpires - SSOAIssuedAtTime) - 5 minutes for processing 

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

        Private m_CatControl As Integer
        Public Property CatControl() As Integer
            Get
                Return m_CatControl
            End Get
            Set
                m_CatControl = Value
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






        ''' <summary>
        ''' test if the token will expire in intSeconds
        ''' </summary>
        ''' <param name="intSeconds"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Function willTokenExpire(ByVal intSeconds As Integer) As Boolean
            If getTokenExpirationDate() < Date.Now().AddSeconds(intSeconds) Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' test if the token has expired
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Function hasTokenExpired() As Boolean
            If getTokenExpirationDate() < Date.Now() Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Gets the expiration date of the token in local time
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Function getTokenExpirationDate() As DateTime
            Dim expOrigin = New Date(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            Dim expDate As Date = expOrigin.AddSeconds(SSOAExpires)
            Return expDate.ToLocalTime()
        End Function

        ''' <summary>
        ''' Gets the issue date of the token in local time
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.0 on 4/6/2017
        ''' </remarks>
        Public Function getTokenIssueDate() As DateTime
            Dim issOrigin = New Date(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            Dim issDate As Date = issOrigin.AddSeconds(SSOAIssuedAtTime)
            Return issDate.ToLocalTime()
        End Function

        Public Function willTokenExpire(ByVal intSeconds As Integer, ByVal token As String) As Boolean
            If Me.USATToken <> token Then
                Return True
            End If
            If getTokenExpirationDate() < Date.Now().AddSeconds(intSeconds) Then
                Return True
            Else
                Return False
            End If
        End Function


    End Class



End Namespace
