Namespace Models

    Public Class AMSCarrierResults

        Private _AvailableSlots As List(Of AMSCarrierAvailableSlots)
        Private _blnMustRequestAppt As Boolean
        Private _RequestSendToEmail As String
        Private _Subject As String
        Private _Body As String
        Private _Message As String

        Public Property AvailableSlots() As List(Of AMSCarrierAvailableSlots)
            Get
                Return _AvailableSlots
            End Get
            Set(ByVal value As List(Of AMSCarrierAvailableSlots))
                _AvailableSlots = value
            End Set
        End Property

        Public Property blnMustRequestAppt() As Boolean
            Get
                Return _blnMustRequestAppt
            End Get
            Set(ByVal value As Boolean)
                _blnMustRequestAppt = value
            End Set
        End Property

        Public Property RequestSendToEmail() As String
            Get
                Return _RequestSendToEmail
            End Get
            Set(ByVal value As String)
                _RequestSendToEmail = value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return _Subject
            End Get
            Set(ByVal value As String)
                _Subject = value
            End Set
        End Property

        Public Property Body() As String
            Get
                Return _Body
            End Get
            Set(ByVal value As String)
                _Body = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal value As String)
                _Message = value
            End Set
        End Property



    End Class


End Namespace

