Public Class clsStandard
    Protected mstrKey As String
    Protected mobjLastException As Exception
    Protected mstrAppName As String
    Protected mstrAppKey As String

    Public Sub New(Optional ByVal strAppName As String = "FreightMaster" _
        , Optional ByVal strAppKey As String = "FreightMaster" _
        , Optional ByVal Key As String = "Standard1")
        MyBase.new()


        mstrKey = Key
        mstrAppName = strAppName
        mstrAppKey = strAppKey

    End Sub



    'public property readonly Key()as String

    Public Overrides Function ToString() As String
        Return mstrKey & " for " & mstrAppName & " with key " & mstrAppKey

    End Function

    Public ReadOnly Property Key() As String
        Get
            Return mstrKey
        End Get
    End Property

    Public ReadOnly Property AppName() As String
        Get
            Return mstrAppName
        End Get
    End Property

    Public ReadOnly Property AppKey() As String
        Get
            Return mstrAppKey
        End Get
    End Property

    Public ReadOnly Property lastException() As Exception
        Get
            Return mobjLastException

        End Get
    End Property
End Class
