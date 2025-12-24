Public Class DebugTextWriter : Inherits System.IO.TextWriter

    Public Overrides Sub Write(ByVal buffer As Char(), ByVal index As Integer, ByVal count As Integer)
        System.Diagnostics.Debug.Write(New [String](buffer, index, count))
    End Sub
    Public Overrides Sub Write(ByVal value As String)
        System.Diagnostics.Debug.Write(value)
    End Sub

    Public Overrides ReadOnly Property Encoding As System.Text.Encoding
        Get
            Return System.Text.Encoding.[Default]
        End Get
    End Property

End Class
