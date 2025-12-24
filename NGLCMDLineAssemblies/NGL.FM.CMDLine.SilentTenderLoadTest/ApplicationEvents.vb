Imports System
Imports System.IO
Imports System.Threading
Imports System.ComponentModel

Public Delegate Sub NGLAppExceptionEventHandler(ByVal sender As Object, ByVal e As NGLAppExceptionEventArgs)
Public Delegate Sub NGLAppTimeOutExceptionEventHandler(ByVal sender As Object, ByVal e As NGLAppExceptionEventArgs)
Public Delegate Sub NGLAppCompletedEventHandler(ByVal sender As Object, ByVal e As NGLAppCompletedEventArgs)
Public Delegate Sub NGLAppStatusUpdateEventHandler(ByVal sender As Object, ByVal e As NGLAppStatusUpdateEventArgs)
Public Delegate Sub NGLAppNotLoadedEventHandler(ByVal sender As Object, ByVal e As NGLAppMessageEventArgs)
Public Delegate Sub NGLAppBadCarriersEventHandler(ByVal sender As Object, ByVal e As NGLAppMessageEventArgs)


Public Class NGLAppExceptionEventArgs
    Inherits AsyncCompletedEventArgs
    Public Sub New(ByVal [error] As Exception, ByVal canceled As Boolean, ByVal userState As Object, ByVal Reason As String, ByVal Details As String)
        MyBase.New([error], canceled, userState)
        _Reason = Reason
        _Detail = Detail
    End Sub

    Private _Detail As String
    Public ReadOnly Property Detail() As String
        Get
            Return _Detail
        End Get
    End Property

    Private _Reason As String
    Public ReadOnly Property Reason() As String
        Get
            Return _Reason
        End Get
    End Property

End Class

Public Class NGLAppCompletedEventArgs
    Inherits AsyncCompletedEventArgs
    Public Sub New(ByVal [error] As Exception, ByVal canceled As Boolean, ByVal userState As Object)
        MyBase.New([error], canceled, userState)
    End Sub
End Class

Public Class NGLAppStatusUpdateEventArgs
    Inherits AsyncCompletedEventArgs

    Public Sub New(ByVal [error] As Exception, ByVal canceled As Boolean, ByVal userState As Object, ByVal message As String, ByVal curstep As Integer, ByVal steps As Integer)
        MyBase.New([error], canceled, userState)
        _Msg = message
        _CurStep = curstep
        _Steps = steps
    End Sub

    Private _Msg As String
    Public ReadOnly Property Message() As String
        Get
            Return _Msg
        End Get
    End Property

    Private _CurStep As Integer
    Public ReadOnly Property CurrentStep() As Integer
        Get
            Return _CurStep
        End Get
    End Property

    Private _Steps As Integer
    Public ReadOnly Property Steps() As Integer
        Get
            Return _Steps
        End Get
    End Property

End Class

Public Class NGLAppMessageEventArgs
    Inherits AsyncCompletedEventArgs

    Public Sub New(ByVal [error] As Exception, ByVal canceled As Boolean, ByVal userState As Object, ByVal message As String)
        MyBase.New([error], canceled, userState)
        _Msg = message
    End Sub

    Private _Msg As String
    Public ReadOnly Property Message() As String
        Get
            Return _Msg
        End Get
    End Property

End Class