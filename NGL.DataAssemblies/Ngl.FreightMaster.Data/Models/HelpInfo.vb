Namespace Models

    Public Class HelpInfo

        Private _PHControlL1 As Integer = 0
        Private _PHControlL2 As Integer = 0
        Private _PHControlL3 As Integer = 0
        Private _PHControlL4 As Integer = 0
        Private _HelpWindowTitle As String = ""
        Private _CompTitle As String = ""
        Private _UserTitle As String = ""
        Private _DefaultTitle As String = ""
        Private _NotesL1 As String = ""
        Private _NotesL2 As String = ""
        Private _NotesL3 As String = ""
        Private _NotesL4 As String = ""
        Private _NotesLocalL1 As String = ""
        Private _NotesLocalL2 As String = ""
        Private _NotesLocalL3 As String = ""
        Private _NotesLocalL4 As String = ""
        Private _ALevel As Integer = 3
        Private _USec As Integer = 0
        Private _Page As Integer = 0


        Public Property PHControlL1() As Integer
            Get
                Return _PHControlL1
            End Get
            Set
                _PHControlL1 = Value
            End Set
        End Property

        Public Property PHControlL2() As Integer
            Get
                Return _PHControlL2
            End Get
            Set
                _PHControlL2 = Value
            End Set
        End Property

        Public Property PHControlL3() As Integer
            Get
                Return _PHControlL3
            End Get
            Set
                _PHControlL3 = Value
            End Set
        End Property
        Public Property PHControlL4() As Integer
            Get
                Return _PHControlL4
            End Get
            Set
                _PHControlL4 = Value
            End Set
        End Property

        Public Property HelpWindowTitle() As String
            Get
                Return _HelpWindowTitle
            End Get
            Set(ByVal value As String)
                _HelpWindowTitle = value
            End Set
        End Property

        Public Property CompTitle() As String
            Get
                Return _CompTitle
            End Get
            Set(ByVal value As String)
                _CompTitle = value
            End Set
        End Property

        Public Property UserTitle() As String
            Get
                Return _UserTitle
            End Get
            Set(ByVal value As String)
                _UserTitle = value
            End Set
        End Property

        Public Property DefaultTitle() As String
            Get
                Return _DefaultTitle
            End Get
            Set(ByVal value As String)
                _DefaultTitle = value
            End Set
        End Property

        Public Property NotesL1() As String
            Get
                Return _NotesL1
            End Get
            Set(ByVal value As String)
                _NotesL1 = value
            End Set
        End Property

        Public Property NotesL2() As String
            Get
                Return _NotesL2
            End Get
            Set(ByVal value As String)
                _NotesL2 = value
            End Set
        End Property

        Public Property NotesL3() As String
            Get
                Return _NotesL3
            End Get
            Set(ByVal value As String)
                _NotesL3 = value
            End Set
        End Property

        Public Property NotesL4() As String
            Get
                Return _NotesL4
            End Get
            Set(ByVal value As String)
                _NotesL4 = value
            End Set
        End Property

        Public Property NotesLocalL1() As String
            Get
                Return _NotesLocalL1
            End Get
            Set(ByVal value As String)
                _NotesLocalL1 = value
            End Set
        End Property

        Public Property NotesLocalL2() As String
            Get
                Return _NotesLocalL2
            End Get
            Set(ByVal value As String)
                _NotesLocalL2 = value
            End Set
        End Property

        Public Property NotesLocalL3() As String
            Get
                Return _NotesLocalL3
            End Get
            Set(ByVal value As String)
                _NotesLocalL3 = value
            End Set
        End Property

        Public Property NotesLocalL4() As String
            Get
                Return _NotesLocalL4
            End Get
            Set(ByVal value As String)
                _NotesLocalL4 = value
            End Set
        End Property

        Public Property ALevel() As Integer
            Get
                Return _ALevel
            End Get
            Set
                _ALevel = Value
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

        Public Property Page() As Integer
            Get
                Return _Page
            End Get
            Set
                _Page = Value
            End Set
        End Property

    End Class

End Namespace

