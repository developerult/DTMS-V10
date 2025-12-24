Namespace Models

    'Added by LVV For v-8.1 On 03/28/2018

    Public Class SingleSignOn

        Private _SSOAXControl As Integer
        Private _SSOAXUN As String
        Private _SSOAXPass As String
        Private _SSOAXRefID As String
        Private _USC As Integer
        Private _UserName As String
        Private _NewPass As String
        Private _SSOAControl As Integer
        Private _SSOAName As String
        Private _SSOADesc As String
        Private _UpdateP As Boolean

        Public Property SSOAXControl() As Integer
            Get
                Return _SSOAXControl
            End Get
            Set(ByVal value As Integer)
                _SSOAXControl = value
            End Set
        End Property

        Public Property SSOAXUN() As String
            Get
                Return _SSOAXUN
            End Get
            Set(ByVal value As String)
                _SSOAXUN = value
            End Set
        End Property

        Public Property SSOAXPass() As String
            Get
                Return _SSOAXPass
            End Get
            Set(ByVal value As String)
                _SSOAXPass = value
            End Set
        End Property

        Public Property SSOAXRefID() As String
            Get
                Return _SSOAXRefID
            End Get
            Set(ByVal value As String)
                _SSOAXRefID = value
            End Set
        End Property

        Public Property USC() As Integer
            Get
                Return _USC
            End Get
            Set(ByVal value As Integer)
                _USC = value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property

        Public Property NewPass() As String
            Get
                Return _NewPass
            End Get
            Set(ByVal value As String)
                _NewPass = value
            End Set
        End Property

        Public Property SSOAControl() As Integer
            Get
                Return _SSOAControl
            End Get
            Set(ByVal value As Integer)
                _SSOAControl = value
            End Set
        End Property

        Public Property SSOAName() As String
            Get
                Return _SSOAName
            End Get
            Set(ByVal value As String)
                _SSOAName = value
            End Set
        End Property

        Public Property SSOADesc() As String
            Get
                Return _SSOADesc
            End Get
            Set(ByVal value As String)
                _SSOADesc = value
            End Set
        End Property

        Public Property UpdateP() As Boolean
            Get
                Return _UpdateP
            End Get
            Set(ByVal value As Boolean)
                _UpdateP = value
            End Set
        End Property




    End Class


End Namespace

