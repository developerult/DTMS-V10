Namespace Models

    Public Class EditorContent

        Private _PageControl As Integer = 0
        Private _USec As Integer = 0
        Private _EditorName As String = ""
        Private _Content As String = ""
        Private _PageDetControl As Integer = 0

        Public Property PageControl() As Integer
            Get
                Return _PageControl
            End Get
            Set
                _PageControl = Value
            End Set
        End Property

        Public Property USec() As Integer
            Get
                Return _USec
            End Get
            Set
                _USec = Value
            End Set
        End Property

        Public Property EditorName() As String
            Get
                Return _EditorName
            End Get
            Set(ByVal value As String)
                _EditorName = value
            End Set
        End Property

        Public Property Content() As String
            Get
                Return _Content
            End Get
            Set(ByVal value As String)
                _Content = value
            End Set
        End Property

        Public Property PageDetControl() As Integer
            Get
                Return _PageDetControl
            End Get
            Set
                _PageDetControl = Value
            End Set
        End Property

    End Class

End Namespace

