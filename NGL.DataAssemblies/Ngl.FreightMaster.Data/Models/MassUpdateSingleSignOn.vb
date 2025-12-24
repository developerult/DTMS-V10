Namespace Models

    Public Class MassUpdateSingleSignOn

        Private _NEXTStop As Boolean
        Private _P44 As Boolean
        Private _SSOAXUN As String
        Private _SSOAXPass As String
        Private _NewPass As String
        Private _SSOAXRefID As String
        Private _UserControls As Integer()
        Private _CopyFromSSOAXCtrl As Integer

        Public Property NEXTStop() As Boolean
            Get
                Return _NEXTStop
            End Get
            Set(ByVal value As Boolean)
                _NEXTStop = value
            End Set
        End Property

        Public Property P44() As Boolean
            Get
                Return _P44
            End Get
            Set(ByVal value As Boolean)
                _P44 = value
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

        Public Property NewPass() As String
            Get
                Return _NewPass
            End Get
            Set(ByVal value As String)
                _NewPass = value
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

        Public Property UserControls() As Integer()
            Get
                Return _UserControls
            End Get
            Set(ByVal value As Integer())
                _UserControls = value
            End Set
        End Property

        Public Property CopyFromSSOAXCtrl() As Integer
            Get
                Return _CopyFromSSOAXCtrl
            End Get
            Set(ByVal value As Integer)
                _CopyFromSSOAXCtrl = value
            End Set
        End Property

    End Class


End Namespace

