Namespace Models
    'Added By LVV On 6/19/18 For v-8.3 TMS365 Scheduler
    Public Class DockPTType

        Private _PTBitPos As Integer
        Private _PTCaption As String
        Private _PTOn As Boolean

        Public Property PTBitPos() As Integer
            Get
                Return _PTBitPos
            End Get
            Set(ByVal value As Integer)
                _PTBitPos = value
            End Set
        End Property

        Public Property PTCaption() As String
            Get
                Return _PTCaption
            End Get
            Set(ByVal value As String)
                _PTCaption = value
            End Set
        End Property

        Public Property PTOn() As Boolean
            Get
                Return _PTOn
            End Get
            Set(ByVal value As Boolean)
                _PTOn = value
            End Set
        End Property

    End Class


End Namespace

