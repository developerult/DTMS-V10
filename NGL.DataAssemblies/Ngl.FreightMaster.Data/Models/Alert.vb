Namespace Models

    Public Class Alert

        Private _Title As String
        Private _Msg As String

        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set
        End Property

        Public Property Msg() As String
            Get
                Return _Msg
            End Get
            Set(ByVal value As String)
                _Msg = value
            End Set
        End Property


    End Class


End Namespace


