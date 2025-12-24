Public Class clsBookTrailer : Inherits clsImportDataBase

    Private _POCompLegalEntity As String = ""
    Public Property POCompLegalEntity As String
        Get
            Return Left(_POCompLegalEntity, 50)
        End Get
        Set(value As String)
            _POCompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _PODefaultCustomer As String = ""
    Public Property PODefaultCustomer As String
        Get
            Return Left(_PODefaultCustomer, 50)
        End Get
        Set(value As String)
            _PODefaultCustomer = Left(value, 50)
        End Set
    End Property

    Private _POCompAlphaCode As String = ""
    Public Property POCompAlphaCode() As String
        Get
            Return Left(_POCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _POCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _PONumber As String = ""
    Public Property PONumber As String
        Get
            Return Left(_PONumber, 20)
        End Get
        Set(value As String)
            _PONumber = Left(value, 20)
        End Set
    End Property


    Private _POOrderSequence As Integer = 0
    Public Property POOrderSequence As Integer
        Get
            Return _POOrderSequence
        End Get
        Set(value As Integer)
            _POOrderSequence = value
        End Set
    End Property

    Private _TrailerNumber As String = ""
    Public Property TrailerNumber As String
        Get
            Return Left(_TrailerNumber, 100)
        End Get
        Set(value As String)
            _TrailerNumber = Left(value, 100)
        End Set
    End Property

    Private _User1 As String = ""
    Public Property User1 As String
        Get
            Return Left(_User1, 4000)
        End Get
        Set(value As String)
            _User1 = Left(value, 4000)
        End Set
    End Property

    Private _User2 As String = ""
    Public Property User2 As String
        Get
            Return Left(_User2, 4000)
        End Get
        Set(value As String)
            _User2 = Left(value, 4000)
        End Set
    End Property

    Private _User3 As String = ""
    Public Property User3 As String
        Get
            Return Left(_User3, 4000)
        End Get
        Set(value As String)
            _User3 = Left(value, 4000)
        End Set
    End Property

    Private _User4 As String = ""
    Public Property User4 As String
        Get
            Return Left(_User4, 4000)
        End Get
        Set(value As String)
            _User4 = Left(value, 4000)
        End Set
    End Property

End Class
