Public Class clsErrorMessages

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(strSubject As String, logMessage As String)
        MyBase.New()
        Subject = strSubject
        Message = logMessage
    End Sub

    Private _Subject As String
    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property

    Private _Message As String
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property



End Class
