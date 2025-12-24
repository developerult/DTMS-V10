Imports System

Public Class DatabaseInvalidException : Inherits ApplicationException

    Public Sub New()

    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub


End Class 'DatabaseInvalidException
Public Class DatabaseLogInException : Inherits ApplicationException

    Public Sub New()

    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub


End Class 'DatabaseLogInException

Public Class DatabaseRetryExceededException : Inherits ApplicationException

    Public Sub New()

    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub


End Class 'DatabaseRetryExceededException

Public Class DatabaseReadDataException : Inherits ApplicationException

    Public Sub New()

    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub


End Class 'DatabaseReadDataException


