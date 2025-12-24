Namespace Models

    Public Class AMSValidation

        Private _SPRequired As Boolean
        Private _InvalidSP As Boolean
        Private _RCRequired As Boolean
        Private _InvalidRC As Boolean
        Private _Success As Boolean
        Private _NoOverride As Boolean
        Private _BitString As String
        Private _Input As String
        Private _ReasonCode As String
        Private _FailedMsg As String '(non-exception message typically associated with a validation failure)
        Private _FailedMsgDetails As List(Of String)


        Public Property SPRequired() As Boolean
            Get
                Return _SPRequired
            End Get
            Set(ByVal value As Boolean)
                _SPRequired = value
            End Set
        End Property

        Public Property InvalidSP() As Boolean
            Get
                Return _InvalidSP
            End Get
            Set(ByVal value As Boolean)
                _InvalidSP = value
            End Set
        End Property

        Public Property RCRequired() As Boolean
            Get
                Return _RCRequired
            End Get
            Set(ByVal value As Boolean)
                _RCRequired = value
            End Set
        End Property

        Public Property InvalidRC() As Boolean
            Get
                Return _InvalidRC
            End Get
            Set(ByVal value As Boolean)
                _InvalidRC = value
            End Set
        End Property

        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Public Property NoOverride() As Boolean
            Get
                Return _NoOverride
            End Get
            Set(ByVal value As Boolean)
                _NoOverride = value
            End Set
        End Property

        Public Property BitString() As String
            Get
                Return _BitString
            End Get
            Set(ByVal value As String)
                _BitString = value
            End Set
        End Property

        Public Property Input() As String
            Get
                Return _Input
            End Get
            Set(ByVal value As String)
                _Input = value
            End Set
        End Property

        Public Property ReasonCode() As String
            Get
                Return _ReasonCode
            End Get
            Set(ByVal value As String)
                _ReasonCode = value
            End Set
        End Property

        Public Property FailedMsg() As String
            Get
                Return _FailedMsg
            End Get
            Set(ByVal value As String)
                _FailedMsg = value
            End Set
        End Property

        Public Property FailedMsgDetails() As List(Of String)
            Get
                Return _FailedMsgDetails
            End Get
            Set(ByVal value As List(Of String))
                _FailedMsgDetails = value
            End Set
        End Property


    End Class


End Namespace

