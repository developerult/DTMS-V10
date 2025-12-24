<Serializable()> _
Public Class clsCarrierHeaderObject

    Public CarrierNumber As Integer = 0
    Public CarrierName As String = ""
    Public CarrierStreetAddress1 As String = ""
    Public CarrierStreetAddress2 As String = ""
    Public CarrierStreetAddress3 As String = ""
    Public CarrierStreetCity As String = ""
    Public CarrierStreetState As String = ""
    Public CarrierStreetCountry As String = ""
    Public CarrierStreetZip As String = ""
    Public CarrierMailAddress1 As String = ""
    Public CarrierMailAddress2 As String = ""
    Public CarrierMailAddress3 As String = ""
    Public CarrierMailCity As String = ""
    Public CarrierMailState As String = ""
    Public CarrierMailCountry As String = ""
    Public CarrierMailZip As String = ""
    Public CarrierTypeCode As String = ""
    Public CarrierSCAC As String = ""
    Public CarrierWebSite As String = ""
    Public CarrierEmail As String = ""
    Public CarrierQualInsuranceDate As String = Date.Now().AddYears(1).ToShortDateString()
    Public CarrierQualQualified As Boolean = True
    Public CarrierQualAuthority As String = ""
    Public CarrierQualContract As Boolean
    Public CarrierQualSignedDate As String = ""
    Public CarrierQualContractExpiresDate As String = ""

End Class

<Serializable()> _
Public Class clsCarrierHeaderObject70 : Inherits clsImportDataBase

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

    Private _CarrierName As String = ""
    Public Property CarrierName() As String
        Get
            Return Left(_CarrierName, 40)
        End Get
        Set(ByVal value As String)
            _CarrierName = Left(value, 40)
        End Set
    End Property

    Private _CarrierStreetAddress1 As String = ""
    Public Property CarrierStreetAddress1() As String
        Get
            Return Left(_CarrierStreetAddress1, 40)
        End Get
        Set(ByVal value As String)
            _CarrierStreetAddress1 = Left(value, 40)
        End Set
    End Property

    Private _CarrierStreetAddress2 As String = ""
    Public Property CarrierStreetAddress2() As String
        Get
            Return Left(_CarrierStreetAddress2, 40)
        End Get
        Set(ByVal value As String)
            _CarrierStreetAddress2 = Left(value, 40)
        End Set
    End Property

    Private _CarrierStreetAddress3 As String = ""
    Public Property CarrierStreetAddress3() As String
        Get
            Return Left(_CarrierStreetAddress3, 40)
        End Get
        Set(ByVal value As String)
            _CarrierStreetAddress3 = Left(value, 40)
        End Set
    End Property

    Private _CarrierStreetCity As String = ""
    Public Property CarrierStreetCity() As String
        Get
            Return Left(_CarrierStreetCity, 25)
        End Get
        Set(ByVal value As String)
            _CarrierStreetCity = Left(value, 25)
        End Set
    End Property

    Private _CarrierStreetState As String = ""
    Public Property CarrierStreetState() As String
        Get
            Return Left(_CarrierStreetState, 2)
        End Get
        Set(ByVal value As String)
            _CarrierStreetState = Left(value, 2)
        End Set
    End Property

    Private _CarrierStreetCountry As String = ""
    Public Property CarrierStreetCountry() As String
        Get
            Return Left(_CarrierStreetCountry, 30)
        End Get
        Set(ByVal value As String)
            _CarrierStreetCountry = Left(value, 30)
        End Set
    End Property

    Private _CarrierStreetZip As String = ""
    Public Property CarrierStreetZip() As String
        Get
            Return Left(_CarrierStreetZip, 10)
        End Get
        Set(ByVal value As String)
            _CarrierStreetZip = Left(value, 10)
        End Set
    End Property

    Private _CarrierMailAddress1 As String = ""
    Public Property CarrierMailAddress1() As String
        Get
            Return Left(_CarrierMailAddress1, 40)
        End Get
        Set(ByVal value As String)
            _CarrierMailAddress1 = Left(value, 40)
        End Set
    End Property

    Private _CarrierMailAddress2 As String = ""
    Public Property CarrierMailAddress2() As String
        Get
            Return Left(_CarrierMailAddress2, 40)
        End Get
        Set(ByVal value As String)
            _CarrierMailAddress2 = Left(value, 40)
        End Set
    End Property

    Private _CarrierMailAddress3 As String = ""
    Public Property CarrierMailAddress3() As String
        Get
            Return Left(_CarrierMailAddress3, 40)
        End Get
        Set(ByVal value As String)
            _CarrierMailAddress3 = Left(value, 40)
        End Set
    End Property

    Private _CarrierMailCity As String = ""
    Public Property CarrierMailCity() As String
        Get
            Return Left(_CarrierMailCity, 25)
        End Get
        Set(ByVal value As String)
            _CarrierMailCity = Left(value, 25)
        End Set
    End Property

    Private _CarrierMailState As String = ""
    Public Property CarrierMailState() As String
        Get
            Return Left(_CarrierMailState, 2)
        End Get
        Set(ByVal value As String)
            _CarrierMailState = Left(value, 2)
        End Set
    End Property

    Private _CarrierMailCountry As String = ""
    Public Property CarrierMailCountry() As String
        Get
            Return Left(_CarrierMailCountry, 30)
        End Get
        Set(ByVal value As String)
            _CarrierMailCountry = Left(value, 30)
        End Set
    End Property

    Private _CarrierMailZip As String = ""
    Public Property CarrierMailZip() As String
        Get
            Return Left(_CarrierMailZip, 10)
        End Get
        Set(ByVal value As String)
            _CarrierMailZip = Left(value, 10)
        End Set
    End Property

    Private _CarrierTypeCode As String = ""
    Public Property CarrierTypeCode() As String
        Get
            Return Left(_CarrierTypeCode, 1)
        End Get
        Set(ByVal value As String)
            _CarrierTypeCode = Left(value, 1)
        End Set
    End Property

    Private _CarrierSCAC As String = ""
    Public Property CarrierSCAC() As String
        Get
            Return Left(_CarrierSCAC, 4)
        End Get
        Set(ByVal value As String)
            _CarrierSCAC = Left(value, 4)
        End Set
    End Property

    Private _CarrierWebSite As String = ""
    Public Property CarrierWebSite() As String
        Get
            Return Left(_CarrierWebSite, 255)
        End Get
        Set(ByVal value As String)
            _CarrierWebSite = Left(value, 255)
        End Set
    End Property

    Private _CarrierEmail As String = ""
    Public Property CarrierEmail() As String
        Get
            Return Left(_CarrierEmail, 50)
        End Get
        Set(ByVal value As String)
            _CarrierEmail = Left(value, 50)
        End Set
    End Property

    Private _CarrierQualInsuranceDate As String = Date.Now().AddYears(1).ToShortDateString()
    Public Property CarrierQualInsuranceDate() As String
        Get
            If String.IsNullOrEmpty(_CarrierQualInsuranceDate) Then _CarrierQualInsuranceDate = Date.Now().AddYears(1).ToShortDateString()
            Return cleanDate(_CarrierQualInsuranceDate)
        End Get
        Set(ByVal value As String)
            _CarrierQualInsuranceDate = value
        End Set
    End Property

    Private _CarrierQualQualified As Boolean = True
    Public Property CarrierQualQualified() As Boolean
        Get
            Return _CarrierQualQualified
        End Get
        Set(ByVal value As Boolean)
            _CarrierQualQualified = value
        End Set
    End Property

    Private _CarrierQualAuthority As String = ""
    Public Property CarrierQualAuthority() As String
        Get
            Return Left(_CarrierQualAuthority, 15)
        End Get
        Set(ByVal value As String)
            _CarrierQualAuthority = Left(value, 15)
        End Set
    End Property

    Private _CarrierQualContract As Boolean = False
    Public Property CarrierQualContract() As Boolean
        Get
            Return _CarrierQualContract
        End Get
        Set(ByVal value As Boolean)
            _CarrierQualContract = value
        End Set
    End Property

    Private _CarrierQualSignedDate As String = ""
    Public Property CarrierQualSignedDate() As String
        Get
            Return cleanDate(_CarrierQualSignedDate)
        End Get
        Set(ByVal value As String)
            _CarrierQualSignedDate = value
        End Set
    End Property

    Private _CarrierQualContractExpiresDate As String = ""
    Public Property CarrierQualContractExpiresDate() As String
        Get
            Return cleanDate(_CarrierQualContractExpiresDate)
        End Get
        Set(ByVal value As String)
            _CarrierQualContractExpiresDate = value
        End Set
    End Property

    ''' <summary>
    ''' used to look up the currency.ID value to populate the CarrierCurType Key
    ''' </summary>
    ''' <remarks></remarks>
    Private _CarrierCurrencyType As String = ""
    Public Property CarrierCurrencyType() As String
        Get
            Return _CarrierCurrencyType
        End Get
        Set(ByVal value As String)
            _CarrierCurrencyType = value
        End Set
    End Property


    Private _CarrierUser1 As String = ""
    Public Property CarrierUser1 As String
        Get
            Return Left(_CarrierUser1, 4000)
        End Get
        Set(value As String)
            _CarrierUser1 = Left(value, 4000)
        End Set
    End Property

    Private _CarrierUser2 As String = ""
    Public Property CarrierUser2 As String
        Get
            Return Left(_CarrierUser2, 4000)
        End Get
        Set(value As String)
            _CarrierUser2 = Left(value, 4000)
        End Set
    End Property

    Private _CarrierUser3 As String = ""
    Public Property CarrierUser3 As String
        Get
            Return Left(_CarrierUser3, 4000)
        End Get
        Set(value As String)
            _CarrierUser3 = Left(value, 4000)
        End Set
    End Property

    Private _CarrierUser4 As String = ""
    Public Property CarrierUser4 As String
        Get
            Return Left(_CarrierUser4, 4000)
        End Get
        Set(value As String)
            _CarrierUser4 = Left(value, 4000)
        End Set
    End Property

    Public Shared Function GenerateSampleObject(ByVal CarrierName As String, ByVal CarrierNumber As Integer, ByVal CarrierAlphaCode As String, ByVal CarrierLegalEntity As String) As clsCarrierHeaderObject70

        Return New clsCarrierHeaderObject70 With { _
                 .CarrierName = CarrierName,
               .CarrierNumber = CarrierNumber,
               .CarrierAlphaCode = CarrierAlphaCode,
               .CarrierLegalEntity = CarrierLegalEntity,
               .CarrierStreetAddress1 = "123 Some Street",
               .CarrierStreetCity = "Some Town",
               .CarrierStreetState = "IL",
               .CarrierStreetCountry = "US",
               .CarrierStreetZip = "60611",
               .CarrierMailAddress1 = "123 Some Street",
               .CarrierMailCity = "Some Town",
               .CarrierMailState = "IL",
               .CarrierMailCountry = "US",
               .CarrierMailZip = "60611"}

    End Function

End Class
