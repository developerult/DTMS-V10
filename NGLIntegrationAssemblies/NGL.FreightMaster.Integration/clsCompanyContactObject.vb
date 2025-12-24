<Serializable()> _
Public Class clsCompanyContactObject
    Public CompNumber As String = ""
    Public CompContName As String = ""
    Public CompContTitle As String = ""
    Public CompCont800 As String = ""
    Public CompContPhone As String = ""
    Public CompContFax As String = ""
    Public CompContEmail As String = ""

End Class

<Serializable()> _
Public Class clsCompanyContactObject70 : Inherits clsImportDataBase

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return Left(_CompLegalEntity, 50)
        End Get
        Set(value As String)
            _CompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _CompNumber As Integer = 0
    Public Property CompNumber() As Integer
        Get
            Return _CompNumber
        End Get
        Set(ByVal value As Integer)
            _CompNumber = value
        End Set
    End Property

    Private _CompAlphaCode As String = ""
    Public Property CompAlphaCode() As String
        Get
            Return Left(_CompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _CompContName As String = ""
    Public Property CompContName() As String
        Get
            Return Left(_CompContName, 25)
        End Get
        Set(ByVal value As String)
            _CompContName = Left(value, 25)
        End Set
    End Property

    Private _CompContTitle As String = ""
    Public Property CompContTitle() As String
        Get
            Return Left(_CompContTitle, 25)
        End Get
        Set(ByVal value As String)
            _CompContTitle = Left(value, 25)
        End Set
    End Property

    Private _CompCont800 As String = ""
    Public Property CompCont800() As String
        Get
            Return Left(_CompCont800, 50)
        End Get
        Set(ByVal value As String)
            _CompCont800 = Left(value, 50)
        End Set
    End Property

    Private _CompContPhone As String = ""
    Public Property CompContPhone() As String
        Get
            Return Left(_CompContPhone, 15)
        End Get
        Set(ByVal value As String)
            _CompContPhone = Left(value, 15)
        End Set
    End Property

    Private _CompContPhoneExt As String = ""
    Public Property CompContPhoneExt() As String
        Get
            Return Left(_CompContPhoneExt, 5)
        End Get
        Set(ByVal value As String)
            _CompContPhoneExt = Left(value, 5)
        End Set
    End Property

    Private _CompContFax As String = ""
    Public Property CompContFax() As String
        Get
            Return Left(_CompContFax, 15)
        End Get
        Set(ByVal value As String)
            _CompContFax = Left(value, 15)
        End Set
    End Property

    Private _CompContEmail As String = ""
    Public Property CompContEmail() As String
        Get
            Return Left(_CompContEmail, 50)
        End Get
        Set(ByVal value As String)
            _CompContEmail = Left(value, 50)
        End Set
    End Property

    Public Shared Function GenerateSampleObject(ByVal CompNumber As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String) As clsCompanyContactObject70

        Return New clsCompanyContactObject70 With { _
                 .CompAlphaCode = CompAlphaCode,
                .CompNumber = CompNumber,
                .CompLegalEntity = CompLegalEntity,
                .CompContName = "NGL",
                .CompContEmail = "NGL@SomeDomain.com",
                .CompContPhone = "1-800-555-1212"}

    End Function

End Class
