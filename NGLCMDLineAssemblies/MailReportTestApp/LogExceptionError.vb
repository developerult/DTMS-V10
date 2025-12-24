Public Class LogExceptionError


    Sub New(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String, ByVal ex As Exception, Optional ByVal strHeader As String = "")
        Me.Subject = strSubject
        Me.LogMessage = logMessage
        Me.MailTo = strMailTo
        Me.EXception = ex
        Me.Header = strHeader
    End Sub

    Private _Subject As String = ""
    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property

    Private _LogMessage As String = ""
    Public Property LogMessage() As String
        Get
            Return _LogMessage
        End Get
        Set(ByVal value As String)
            _LogMessage = value
        End Set
    End Property


    Private _MailTo As String = ""
    Public Property MailTo() As String
        Get
            Return _MailTo
        End Get
        Set(ByVal value As String)
            _MailTo = value
        End Set
    End Property


    Private _EXception As New Exception
    Public Property EXception() As Exception
        Get
            Return _EXception
        End Get
        Set(ByVal value As Exception)
            _EXception = value
        End Set
    End Property


    Private _Header As String = ""
    Public Property Header() As String
        Get
            Return _Header
        End Get
        Set(ByVal value As String)
            _Header = value
        End Set
    End Property

End Class
