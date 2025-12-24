Public Class clsDistanceRef

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal s1 As Integer, s2 As Integer)
        MyBase.New()
        Stop1 = s1
        Stop2 = s2
    End Sub

    Public Stop1 As Integer = 0
    Public Stop2 As Integer = 0

End Class
