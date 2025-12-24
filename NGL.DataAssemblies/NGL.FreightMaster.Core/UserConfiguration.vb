<Serializable()> _
Public Class UserConfiguration

#Region " Class Variables and Properties """

    Private _ConnectionString As String = ""
    ''' <summary>
    '''This property is used to set the database connection string at run time
    ''' It uses the Application Settings NGLMAS connection string value as the default     ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ConnectionString() As String
        Get
            If _ConnectionString.Trim.Length < 1 Then
                If Me.DBServer.Trim.Length > 0 AndAlso Me.Database.Trim.Length > 0 Then
                    _ConnectionString = String.Format("Server={0};Database={1};Integrated Security=SSPI", Me.DBServer.Trim, Me.Database.Trim)
                Else
                    _ConnectionString = My.Settings.NGLMAS
                End If
            End If
            Return _ConnectionString
        End Get

        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property
    
    Private _ShortTimeOut As Integer = 300
    ''' <summary>
    ''' This property determines the length when command operations time out
    ''' All data BLL classes have the ability to set the SetCommandTimeOut property 
    ''' for each  command  object in the command collection.  Some will use the
    ''' ShortTimeOut value some will use the LongTimeOut value and some will use the
    ''' BatchTimeOut value     ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>The minimum value for ShortTimeOut is 300</remarks>
    Public Property ShortTimeOut() As Integer
        Get
            If _ShortTimeOut < 300 Then
                _ShortTimeOut = 300
            End If
            Return _ShortTimeOut
        End Get

        Set(ByVal value As Integer)

            _ShortTimeOut = value
        End Set

    End Property

    
    Private _LongTimeOut As Integer = 600
    ''' <summary>
    ''' This property determines the length when command operations time out
    ''' All data BLL classes have the ability to set the SetCommandTimeOut property 
    ''' for each  command  object in the command collection.  Some will use the
    ''' ShortTimeOut value some will use the LongTimeOut value and some will use the
    ''' BatchTimeOut value 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>The minimum value for LongTimeOut is 600</remarks>
    Public Property LongTimeOut() As Integer
        Get
            If _LongTimeOut < 600 Then
                _LongTimeOut = 600
            End If
            Return _LongTimeOut
        End Get

        Set(ByVal value As Integer)

            _LongTimeOut = value
        End Set

    End Property

   
    Private _BatchTimeOut As Integer = 1200
    ''' <summary>
    ''' This property determines the length when command operations time out
    ''' All data BLL classes have the ability to set the SetCommandTimeOut property 
    ''' for each  command  object in the command collection.  Some will use the
    ''' ShortTimeOut value some will use the LongTimeOut value and some will use the
    ''' BatchTimeOut value     ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>The minimum value for BatchTimeOut is 1200</remarks>
    Public Property BatchTimeOut() As Integer
        Get
            If _BatchTimeOut < 1200 Then
                _BatchTimeOut = 1200
            End If
            Return _BatchTimeOut
        End Get

        Set(ByVal value As Integer)

            _BatchTimeOut = value
        End Set

    End Property
    
    Private _blnDebug As Boolean = False
    ''' <summary>
    ''' Property that determines if debuging on on or off
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Debug() As Boolean
        Get
            Return _blnDebug
        End Get
        Set(ByVal value As Boolean)
            _blnDebug = value
        End Set
    End Property
   
    Private _strResultsFile As String = ""
    ''' <summary>
    ''' Result file name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ResultsFile() As String
        Get
            Return _strResultsFile
        End Get
        Set(ByVal value As String)
            _strResultsFile = value
        End Set
    End Property

    Private _strLogFile As String = ""
    ''' <summary>
    ''' Log file name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LogFile() As String
        Get
            Return _strLogFile
        End Get
        Set(ByVal value As String)
            _strLogFile = value
        End Set
    End Property
    
    Private _strINIKey As String = "NGL"
    ''' <summary>
    ''' INI File Key Section For Configuration Settings; default = NGL
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property INIKey() As String
        Get
            Return _strINIKey
        End Get
        Set(ByVal value As String)
            _strINIKey = value
        End Set
    End Property
    
    Private _intAutoRetry As Integer = 3
    ''' <summary>
    ''' Number of auto retrys before throwing errors
    ''' Used on batch processing and unattended execution components    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoRetry() As Integer
        Get
            Return _intAutoRetry
        End Get
        Set(ByVal value As Integer)
            _intAutoRetry = value
        End Set
    End Property
    
    Private _strDatabase As String = ""
    ''' <summary>
    ''' Primary Database Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Database() As String
        Get
            Return _strDatabase
        End Get
        Set(ByVal value As String)
            _strDatabase = value
        End Set
    End Property
    Private _strDBServer As String = ""
    ''' <summary>
    ''' Primary Database Server
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DBServer() As String
        Get
            Return _strDBServer
        End Get
        Set(ByVal value As String)
            _strDBServer = value
        End Set
    End Property

    Private _strSource As String = ""
    ''' <summary>
    ''' Code source or file name used to help identify problems and execution paths
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Source() As String
        Get
            Return _strSource
        End Get
        Set(ByVal Value As String)
            _strSource = Value
        End Set
    End Property
   
    Private _strAdminEmail As String = ""
    ''' <summary>
    ''' Administrative Email Address 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AdminEmail() As String
        Get
            Return _strAdminEmail
        End Get
        Set(ByVal value As String)
            _strAdminEmail = value
        End Set
    End Property
    
    Private _strGroupEmail As String = ""
    ''' <summary>
    ''' User Community Group Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GroupEmail() As String
        Get
            Return _strGroupEmail
        End Get
        Set(ByVal value As String)
            _strGroupEmail = value
        End Set
    End Property
    Private _strFromEmail As String = "system@nextgeneration.com"
    ''' <summary>
    ''' System From Email Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FromEmail() As String
        Get
            Return _strFromEmail
        End Get
        Set(ByVal value As String)
            _strFromEmail = value
        End Set
    End Property

    Private _strSMTPServer As String = "mail.ngl.local"
    ''' <summary>
    ''' SMTP Server Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SMTPServer() As String
        Get
            Return _strSMTPServer
        End Get
        Set(ByVal value As String)
            _strSMTPServer = value
        End Set
    End Property

    Private _intKeepLogDays As Integer = 30
    ''' <summary>
    ''' Keep log file days; default = 30
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KeepLogDays() As Integer
        Get
            Return _intKeepLogDays
        End Get
        Set(ByVal value As Integer)
            _intKeepLogDays = value
        End Set
    End Property

    Private _blnSaveOldLog As Boolean = True
    ''' <summary>
    ''' Save old log file flag, default = true
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SaveOldLog() As Boolean
        Get
            Return _blnSaveOldLog
        End Get
        Set(ByVal value As Boolean)
            _blnSaveOldLog = value
        End Set
    End Property

    Private _strUserName As String = ""
    ''' <summary>
    ''' Current UserName normally used to filter data
    ''' or apply FreightMaster Security.  May also be
    ''' used by some procedures as part of the connection
    '''  string when a valid password is provided
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UserName() As String
        Get
            Return _strUserName
        End Get
        Set(ByVal value As String)
            _strUserName = value
        End Set
    End Property

    Private _intUserID As Integer = 0
    ''' <summary>
    ''' Current User ID value from user security table
    ''' if value = 0 the user is not restricted or it has not been populated
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UserID() As Integer
        Get
            Return _intUserID
        End Get
        Set(ByVal value As Integer)
            _intUserID = value
        End Set
    End Property

    Private _strPassword As String = ""
    ''' <summary>
    ''' Password may be used as part of the connection
    '''  string when a valid username is provided
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Password() As String
        Get
            Return _strPassword
        End Get
        Set(ByVal value As String)
            _strPassword = value
        End Set
    End Property

    Public ReadOnly Property DBInfo() As String
        Get
            Return "Server: " & DBServer & " | " & "Database: " & Database

        End Get
    End Property

    Private _WSAuthCode As String
    ''' <summary>
    ''' Web Service Authorization Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WSAuthCode() As String
        Get
            Return _WSAuthCode
        End Get
        Set(ByVal value As String)
            _WSAuthCode = value
        End Set
    End Property

    Private _WCFAuthCode As String
    ''' <summary>
    ''' WCF Authorization Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WCFAuthCode() As String
        Get
            Return _WCFAuthCode
        End Get
        Set(ByVal value As String)
            _WCFAuthCode = value
        End Set
    End Property

    Private _WCFURL As String
    ''' <summary>
    ''' URL to WCF Web Services
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WCFURL() As String
        Get
            Return _WCFURL
        End Get
        Set(ByVal value As String)
            _WCFURL = value
        End Set
    End Property

    Private _WCFTCPURL As String
    ''' <summary>
    ''' URL to TCP Port for WCF Processing
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WCFTCPURL() As String
        Get
            Return _WCFTCPURL
        End Get
        Set(ByVal value As String)
            _WCFTCPURL = value
        End Set
    End Property


#End Region

#Region " Constructors "




#End Region

#Region " Functions "




#End Region



End Class
