<Serializable()> _
Public Class clsCarrierContactObject
    Public CarrierNumber As Integer = 0
    Public CarrierContName As String = ""
    Public CarrierContTitle As String = ""
    Public CarrierContactPhone As String = ""
    Public CarrierContactFax As String = ""
    Public CarrierContact800 As String = ""
    Public CarrierContactEMail As String = ""
End Class

<Serializable()> _
Public Class clsCarrierContactObject70 : Inherits clsImportDataBase

    Private _CarrierLegalEntity As String = ""
    Public Property CarrierLegalEntity As String
        Get
            Return Left(_CarrierLegalEntity, 50)
        End Get
        Set(value As String)
            _CarrierLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _CarrierNumber As Integer = 0
    Public Property CarrierNumber() As Integer
        Get
            Return _CarrierNumber
        End Get
        Set(ByVal value As Integer)
            _CarrierNumber = value
        End Set
    End Property

    Private _CarrierAlphaCode As String = ""
    Public Property CarrierAlphaCode() As String
        Get
            Return Left(_CarrierAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CarrierAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _CarrierContName As String = ""
    Public Property CarrierContName() As String
        Get
            Return Left(_CarrierContName, 25)
        End Get
        Set(ByVal value As String)
            _CarrierContName = Left(value, 25)
        End Set
    End Property

    Private _CarrierContTitle As String = ""
    Public Property CarrierContTitle() As String
        Get
            Return Left(_CarrierContTitle, 25)
        End Get
        Set(ByVal value As String)
            _CarrierContTitle = Left(value, 25)
        End Set
    End Property

    Private _CarrierContactPhone As String = ""
    Public Property CarrierContactPhone() As String
        Get
            Return Left(_CarrierContactPhone, 15)
        End Get
        Set(ByVal value As String)
            _CarrierContactPhone = Left(value, 15)
        End Set
    End Property

    Private _CarrierContPhoneExt As String = ""
    Public Property CarrierContPhoneExt() As String
        Get
            Return Left(_CarrierContPhoneExt, 5)
        End Get
        Set(ByVal value As String)
            _CarrierContPhoneExt = Left(value, 5)
        End Set
    End Property

    Private _CarrierContactFax As String = ""
    Public Property CarrierContactFax() As String
        Get
            Return Left(_CarrierContactFax, 15)
        End Get
        Set(ByVal value As String)
            _CarrierContactFax = Left(value, 15)
        End Set
    End Property

    Private _CarrierContact800 As String = ""
    Public Property CarrierContact800() As String
        Get
            Return Left(_CarrierContact800, 15)
        End Get
        Set(ByVal value As String)
            _CarrierContact800 = Left(value, 15)
        End Set
    End Property

    Private _CarrierContactEMail As String = ""
    Public Property CarrierContactEMail() As String
        Get
            Return Left(_CarrierContactEMail, 255)
        End Get
        Set(ByVal value As String)
            _CarrierContactEMail = Left(value, 255)
        End Set
    End Property

    Public Shared Function GenerateSampleObject(ByVal CarrierNumber As Integer, ByVal CarrierAlphaCode As String, ByVal CarrierLegalEntity As String) As clsCarrierContactObject70

        Return New clsCarrierContactObject70 With { _
                 .CarrierAlphaCode = CarrierAlphaCode,
               .CarrierNumber = CarrierNumber,
               .CarrierLegalEntity = CarrierLegalEntity,
               .CarrierContName = "NGL",
               .CarrierContactEMail = "NGL@SomeDomain.com",
               .CarrierContactPhone = "1-800-555-1212"}

    End Function

End Class
