<Serializable()> _
Public Class clsCompanyHeaderObject
    Public CompNumber As String = ""
    Public CompName As String = ""
    Public CompNatNumber As Integer = 0
    Public CompNatName As String = ""
    Public CompStreetAddress1 As String = ""
    Public CompStreetAddress2 As String = ""
    Public CompStreetAddress3 As String = ""
    Public CompStreetCity As String = ""
    Public CompStreetState As String = ""
    Public CompStreetCountry As String = ""
    Public CompStreetZip As String = ""
    Public CompMailAddress1 As String = ""
    Public CompMailAddress2 As String = ""
    Public CompMailAddress3 As String = ""
    Public CompMailCity As String = ""
    Public CompMailState As String = ""
    Public CompMailCountry As String = ""
    Public CompMailZip As String = ""
    Public CompWeb As String = ""
    Public CompEmail As String = ""
    Public CompDirections As String = ""
    Public CompAbrev As String = ""
    Public CompActive As Boolean = True
    Public CompNEXTrack As Boolean = False
    Public CompNEXTStopAcctNo As String = ""
    Public CompNEXTStopPsw As String = ""
    Public CompNextstopSubmitRFP As Boolean = False
    Public CompFAAShipID As String = ""
    Public CompFAAShipDate As String = ""
    Public CompFinDuns As String = ""
    Public CompFinTaxID As String = ""
    Public CompFinPaymentForm As Short = 0
    Public CompFinSIC As String = ""
    Public CompFinPaymentDiscount As Short = 0
    Public CompFinPaymentDays As Short = 0
    Public CompFinPaymentNetDays As Short = 0
    Public CompFinCommTerms As String = ""
    Public CompFinCommTermsPer As Double = 0
    Public CompFinCreditLimit As Integer = 0
    Public CompFinCreditUsed As Integer = 0
    Public CompFinInvPrnCode As Boolean = False
    Public CompFinInvEMailCode As Boolean = False
    Public CompFinCurType As Integer = 0
    Public CompFinFBToleranceHigh As Double = 0
    Public CompFinFBToleranceLow As Double = 0
    Public CompFinCustomerSince As String = ""
    Public CompFinCardType As String = ""
    Public CompFinCardName As String = ""
    Public CompFinCardExpires As String = ""
    Public CompFinCardAuthorizor As String = ""
    Public CompFinCardAuthPassword As String = ""
    Public CompFinUseImportFrtCost As Boolean = False
    Public CompFinBkhlFlatFee As Double = 0
    Public CompFinBkhlCostPerc As Double = 0
    Public CompLatitude As Double = 0
    Public CompLongitude As Double = 0
    Public CompMailTo As String = ""
End Class

<Serializable()> _
Public Class clsCompanyHeaderObject70 : Inherits clsImportDataBase

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

    Private _CompName As String = ""
    Public Property CompName() As String
        Get
            Return Left(_CompName, 40)
        End Get
        Set(ByVal value As String)
            _CompName = Left(value, 40)
        End Set
    End Property

    Private _CompNatNumber As Integer = 0
    Public Property CompNatNumber() As Integer
        Get
            Return _CompNatNumber
        End Get
        Set(ByVal value As Integer)
            _CompNatNumber = value
        End Set
    End Property

    Private _CompNatName As String = ""
    Public Property CompNatName() As String
        Get
            Return Left(_CompNatName, 40)
        End Get
        Set(ByVal value As String)
            _CompNatName = Left(value, 40)
        End Set
    End Property

    Private _CompStreetAddress1 As String = ""
    Public Property CompStreetAddress1() As String
        Get
            Return Left(_CompStreetAddress1, 40)
        End Get
        Set(ByVal value As String)
            _CompStreetAddress1 = Left(value, 40)
        End Set
    End Property

    Private _CompStreetAddress2 As String = ""
    Public Property CompStreetAddress2() As String
        Get
            Return Left(_CompStreetAddress2, 40)
        End Get
        Set(ByVal value As String)
            _CompStreetAddress2 = Left(value, 40)
        End Set
    End Property

    Private _CompStreetAddress3 As String = ""
    Public Property CompStreetAddress3() As String
        Get
            Return Left(_CompStreetAddress3, 40)
        End Get
        Set(ByVal value As String)
            _CompStreetAddress3 = Left(value, 40)
        End Set
    End Property

    Private _CompStreetCity As String = ""
    Public Property CompStreetCity() As String
        Get
            Return Left(_CompStreetCity, 25)
        End Get
        Set(ByVal value As String)
            _CompStreetCity = Left(value, 25)
        End Set
    End Property

    Private _CompStreetState As String = ""
    Public Property CompStreetState() As String
        Get
            Return Left(_CompStreetState, 2)
        End Get
        Set(ByVal value As String)
            _CompStreetState = Left(value, 2)
        End Set
    End Property

    Private _CompStreetCountry As String = ""
    Public Property CompStreetCountry() As String
        Get
            Return Left(_CompStreetCountry, 30)
        End Get
        Set(ByVal value As String)
            _CompStreetCountry = Left(value, 30)
        End Set
    End Property

    Private _CompStreetZip As String = ""
    Public Property CompStreetZip() As String
        Get
            Return Left(_CompStreetZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _CompStreetZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property


    Private _CompMailAddress1 As String = ""
    Public Property CompMailAddress1() As String
        Get
            Return Left(_CompMailAddress1, 40)
        End Get
        Set(ByVal value As String)
            _CompMailAddress1 = Left(value, 40)
        End Set
    End Property

    Private _CompMailAddress2 As String = ""
    Public Property CompMailAddress2() As String
        Get
            Return Left(_CompMailAddress2, 40)
        End Get
        Set(ByVal value As String)
            _CompMailAddress2 = Left(value, 40)
        End Set
    End Property

    Private _CompMailAddress3 As String = ""
    Public Property CompMailAddress3() As String
        Get
            Return Left(_CompMailAddress3, 40)
        End Get
        Set(ByVal value As String)
            _CompMailAddress3 = Left(value, 40)
        End Set
    End Property

    Private _CompMailCity As String = ""
    Public Property CompMailCity() As String
        Get
            Return Left(_CompMailCity, 25)
        End Get
        Set(ByVal value As String)
            _CompMailCity = Left(value, 25)
        End Set
    End Property

    Private _CompMailState As String = ""
    Public Property CompMailState() As String
        Get
            Return Left(_CompMailState, 2)
        End Get
        Set(ByVal value As String)
            _CompMailState = Left(value, 2)
        End Set
    End Property

    Private _CompMailCountry As String = ""
    Public Property CompMailCountry() As String
        Get
            Return Left(_CompMailCountry, 30)
        End Get
        Set(ByVal value As String)
            _CompMailCountry = Left(value, 30)
        End Set
    End Property

    Private _CompMailZip As String = ""
    Public Property CompMailZip() As String
        Get
            Return Left(_CompMailZip, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(ByVal value As String)
            _CompMailZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _CompWeb As String = ""
    Public Property CompWeb() As String
        Get
            Return Left(_CompWeb, 255)
        End Get
        Set(ByVal value As String)
            _CompWeb = Left(value, 255)
        End Set
    End Property

    Private _CompEmail As String = ""
    Public Property CompEmail() As String
        Get
            Return Left(_CompEmail, 50)
        End Get
        Set(ByVal value As String)
            _CompEmail = Left(value, 50)
        End Set
    End Property

    Private _CompDirections As String = ""
    Public Property CompDirections() As String
        Get
            Return _CompDirections
        End Get
        Set(ByVal value As String)
            _CompDirections = value
        End Set
    End Property

    Private _CompAbrev As String = ""
    Public Property CompAbrev() As String
        Get
            Return Left(_CompAbrev, 3)
        End Get
        Set(ByVal value As String)
            _CompAbrev = Left(value, 3)
        End Set
    End Property

    Private _CompActive As Boolean = False
    Public Property CompActive() As Boolean
        Get
            Return _CompActive
        End Get
        Set(ByVal value As Boolean)
            _CompActive = value
        End Set
    End Property

    Private _CompNEXTrack As Boolean = False
    Public Property CompNEXTrack() As Boolean
        Get
            Return _CompNEXTrack
        End Get
        Set(ByVal value As Boolean)
            _CompNEXTrack = value
        End Set
    End Property

    Private _CompNEXTStopAcctNo As String = ""
    Public Property CompNEXTStopAcctNo() As String
        Get
            Return Left(_CompNEXTStopAcctNo, 50)
        End Get
        Set(ByVal value As String)
            _CompNEXTStopAcctNo = Left(value, 50)
        End Set
    End Property

    Private _CompNEXTStopPsw As String = ""
    Public Property CompNEXTStopPsw() As String
        Get
            Return Left(_CompNEXTStopPsw, 50)
        End Get
        Set(ByVal value As String)
            _CompNEXTStopPsw = Left(value, 50)
        End Set
    End Property

    Private _CompNextstopSubmitRFP As Boolean = False
    Public Property CompNextstopSubmitRFP() As Boolean
        Get
            Return _CompNextstopSubmitRFP
        End Get
        Set(ByVal value As Boolean)
            _CompNextstopSubmitRFP = value
        End Set
    End Property

    Private _CompFAAShipID As String = ""
    Public Property CompFAAShipID() As String
        Get
            Return Left(_CompFAAShipID, 50)
        End Get
        Set(ByVal value As String)
            _CompFAAShipID = Left(value, 50)
        End Set
    End Property

    Private _CompFAAShipDate As String = ""
    Public Property CompFAAShipDate() As String
        Get
            Return cleanDate(_CompFAAShipDate)
        End Get
        Set(ByVal value As String)
            _CompFAAShipDate = value
        End Set
    End Property

    Private _CompFinDuns As String = ""
    Public Property CompFinDuns() As String
        Get
            Return Left(_CompFinDuns, 11)
        End Get
        Set(ByVal value As String)
            _CompFinDuns = Left(value, 11)
        End Set
    End Property

    Private _CompFinTaxID As String = ""
    Public Property CompFinTaxID() As String
        Get
            Return Left(_CompFinTaxID, 20)
        End Get
        Set(ByVal value As String)
            _CompFinTaxID = Left(value, 20)
        End Set
    End Property

    Private _CompFinPaymentForm As String = ""
    Public Property CompFinPaymentForm() As String
        Get
            Return Left(_CompFinPaymentForm, 50)
        End Get
        Set(ByVal value As String)
            _CompFinPaymentForm = Left(value, 50)
        End Set
    End Property

    Private _CompFinSIC As String = ""
    Public Property CompFinSIC() As String
        Get
            Return Left(_CompFinSIC, 8)
        End Get
        Set(ByVal value As String)
            _CompFinSIC = Left(value, 8)
        End Set
    End Property

    Private _CompFinPaymentDiscount As Short = 0
    Public Property CompFinPaymentDiscount() As Short
        Get
            Return _CompFinPaymentDiscount
        End Get
        Set(ByVal value As Short)
            _CompFinPaymentDiscount = value
        End Set
    End Property

    Private _CompFinPaymentDays As Short = 0
    Public Property CompFinPaymentDays() As Short
        Get
            Return _CompFinPaymentDays
        End Get
        Set(ByVal value As Short)
            _CompFinPaymentDays = value
        End Set
    End Property

    Private _CompFinPaymentNetDays As Short = 0
    Public Property CompFinPaymentNetDays() As Short
        Get
            Return _CompFinPaymentNetDays
        End Get
        Set(ByVal value As Short)
            _CompFinPaymentNetDays = value
        End Set
    End Property

    Private _CompFinCommTerms As String = ""
    Public Property CompFinCommTerms() As String
        Get
            Return Left(_CompFinCommTerms, 15)
        End Get
        Set(ByVal value As String)
            _CompFinCommTerms = Left(value, 15)
        End Set
    End Property

    Private _CompFinCommTermsPer As Double = 0
    Public Property CompFinCommTermsPer() As Double
        Get
            Return _CompFinCommTermsPer
        End Get
        Set(ByVal value As Double)
            _CompFinCommTermsPer = value
        End Set
    End Property

    Private _CompFinCreditLimit As Integer = 0
    Public Property CompFinCreditLimit() As Integer
        Get
            Return _CompFinCreditLimit
        End Get
        Set(ByVal value As Integer)
            _CompFinCreditLimit = value
        End Set
    End Property

    Private _CompFinCreditUsed As Integer = 0
    Public Property CompFinCreditUsed() As Integer
        Get
            Return _CompFinCreditUsed
        End Get
        Set(ByVal value As Integer)
            _CompFinCreditUsed = value
        End Set
    End Property

    Private _CompFinInvPrnCode As Boolean = False
    Public Property CompFinInvPrnCode() As Boolean
        Get
            Return _CompFinInvPrnCode
        End Get
        Set(ByVal value As Boolean)
            _CompFinInvPrnCode = value
        End Set
    End Property

    Private _CompFinInvEMailCode As Boolean = False
    Public Property CompFinInvEMailCode() As Boolean
        Get
            Return _CompFinInvEMailCode
        End Get
        Set(ByVal value As Boolean)
            _CompFinInvEMailCode = value
        End Set
    End Property

    Private _CompFinCurType As Integer = 0
    Public Property CompFinCurType() As Integer
        Get
            Return _CompFinCurType
        End Get
        Set(ByVal value As Integer)
            _CompFinCurType = value
        End Set
    End Property

    ''' <summary>
    ''' The CompCurrencyType is an alpha code reference to [dbo].[Currency].[CurrencyType]
    ''' If CompCurrencyType is an empty string we use CompFinCurType else we lookup the CompFinCurType
    ''' ID using the CompCurrencyType and the [dbo].[Currency].[CurrencyType]
    ''' </summary>
    ''' <remarks></remarks>
    Private _CompCurrencyType As String = ""
    Public Property CompCurrencyType() As String
        Get
            Return _CompCurrencyType
        End Get
        Set(ByVal value As String)
            _CompCurrencyType = value
        End Set
    End Property
    Private _CompFinFBToleranceHigh As Double = 0
    Public Property CompFinFBToleranceHigh() As Double
        Get
            Return _CompFinFBToleranceHigh
        End Get
        Set(ByVal value As Double)
            _CompFinFBToleranceHigh = value
        End Set
    End Property

    Private _CompFinFBToleranceLow As Double = 0
    Public Property CompFinFBToleranceLow() As Double
        Get
            Return _CompFinFBToleranceLow
        End Get
        Set(ByVal value As Double)
            _CompFinFBToleranceLow = value
        End Set
    End Property

    Private _CompFinCustomerSince As String = ""
    Public Property CompFinCustomerSince() As String
        Get
            Return cleanDate(_CompFinCustomerSince)
        End Get
        Set(ByVal value As String)
            _CompFinCustomerSince = value
        End Set
    End Property

    Private _CompFinCardType As String = ""
    Public Property CompFinCardType() As String
        Get
            Return Left(_CompFinCardType, 50)
        End Get
        Set(ByVal value As String)
            _CompFinCardType = Left(value, 50)
        End Set
    End Property

    Private _CompFinCardName As String = ""
    Public Property CompFinCardName() As String
        Get
            Return Left(_CompFinCardName, 50)
        End Get
        Set(ByVal value As String)
            _CompFinCardName = Left(value, 50)
        End Set
    End Property

    Private _CompFinCardExpires As String = ""
    Public Property CompFinCardExpires() As String
        Get
            Return Left(_CompFinCardExpires, 50)
        End Get
        Set(ByVal value As String)
            _CompFinCardExpires = Left(value, 50)
        End Set
    End Property

    Private _CompFinCardAuthorizor As String = ""
    Public Property CompFinCardAuthorizor() As String
        Get
            Return Left(_CompFinCardAuthorizor, 50)
        End Get
        Set(ByVal value As String)
            _CompFinCardAuthorizor = Left(value, 50)
        End Set
    End Property

    Private _CompFinCardAuthPassword As String = ""
    Public Property CompFinCardAuthPassword() As String
        Get
            Return Left(_CompFinCardAuthPassword, 50)
        End Get
        Set(ByVal value As String)
            _CompFinCardAuthPassword = Left(value, 50)
        End Set
    End Property

    Private _CompFinUseImportFrtCost As Boolean = False
    Public Property CompFinUseImportFrtCost() As Boolean
        Get
            Return _CompFinUseImportFrtCost
        End Get
        Set(ByVal value As Boolean)
            _CompFinUseImportFrtCost = value
        End Set
    End Property

    Private _CompFinBkhlFlatFee As Double = 0
    Public Property CompFinBkhlFlatFee() As Double
        Get
            Return _CompFinBkhlFlatFee
        End Get
        Set(ByVal value As Double)
            _CompFinBkhlFlatFee = value
        End Set
    End Property

    Private _CompFinBkhlCostPerc As Double = 0
    Public Property CompFinBkhlCostPerc() As Double
        Get
            Return _CompFinBkhlCostPerc
        End Get
        Set(ByVal value As Double)
            _CompFinBkhlCostPerc = value
        End Set
    End Property

    Private _CompLatitude As Double = 0
    Public Property CompLatitude() As Double
        Get
            Return _CompLatitude
        End Get
        Set(ByVal value As Double)
            _CompLatitude = value
        End Set
    End Property

    Private _CompLongitude As Double = 0
    Public Property CompLongitude() As Double
        Get
            Return _CompLongitude
        End Get
        Set(ByVal value As Double)
            _CompLongitude = value
        End Set
    End Property

    Private _CompMailTo As String = ""
    Public Property CompMailTo() As String
        Get
            Return Left(_CompMailTo, 500)
        End Get
        Set(ByVal value As String)
            _CompMailTo = Left(value, 500)
        End Set
    End Property

    Private _CompTimeZone As String = ""
    Public Property CompTimeZone() As String
        Get
            Return Left(_CompTimeZone, 100)
        End Get
        Set(ByVal value As String)
            _CompTimeZone = Left(value, 100)
        End Set
    End Property

    Private _CompRailStationName As String = ""
    Public Property CompRailStationName() As String
        Get
            Return Left(_CompRailStationName, 50)
        End Get
        Set(ByVal value As String)
            _CompRailStationName = Left(value, 50)
        End Set
    End Property

    Private _CompRailSPLC As String = ""
    Public Property CompRailSPLC() As String
        Get
            Return Left(_CompRailSPLC, 50)
        End Get
        Set(ByVal value As String)
            _CompRailSPLC = Left(value, 50)
        End Set
    End Property

    Private _CompRailFSAC As String = ""
    Public Property CompRailFSAC() As String
        Get
            Return Left(_CompRailFSAC, 50)
        End Get
        Set(ByVal value As String)
            _CompRailFSAC = Left(value, 50)
        End Set
    End Property

    Private _CompRail333 As String = ""
    Public Property CompRail333() As String
        Get
            Return Left(_CompRail333, 50)
        End Get
        Set(ByVal value As String)
            _CompRail333 = Left(value, 50)
        End Set
    End Property

    Private _CompRailR260 As String = ""
    Public Property CompRailR260() As String
        Get
            Return Left(_CompRailR260, 50)
        End Get
        Set(ByVal value As String)
            _CompRailR260 = Left(value, 50)
        End Set
    End Property

    Private _CompIsTransLoad As Boolean = False
    Public Property CompIsTransLoad() As Boolean
        Get
            Return _CompIsTransLoad
        End Get
        Set(ByVal value As Boolean)
            _CompIsTransLoad = value
        End Set
    End Property

    Private _CompUser1 As String = ""
    Public Property CompUser1 As String
        Get
            Return Left(_CompUser1, 4000)
        End Get
        Set(value As String)
            _CompUser1 = Left(value, 4000)
        End Set
    End Property

    Private _CompUser2 As String = ""
    Public Property CompUser2 As String
        Get
            Return Left(_CompUser2, 4000)
        End Get
        Set(value As String)
            _CompUser2 = Left(value, 4000)
        End Set
    End Property

    Private _CompUser3 As String = ""
    Public Property CompUser3 As String
        Get
            Return Left(_CompUser3, 4000)
        End Get
        Set(value As String)
            _CompUser3 = Left(value, 4000)
        End Set
    End Property

    Private _CompUser4 As String = ""
    Public Property CompUser4 As String
        Get
            Return Left(_CompUser4, 4000)
        End Get
        Set(value As String)
            _CompUser4 = Left(value, 4000)
        End Set
    End Property

    Public Shared Function GenerateSampleObject(ByVal CompName As String, ByVal CompNumber As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String, ByVal CompAbrev As String) As clsCompanyHeaderObject70

        Return New clsCompanyHeaderObject70 With { _
                .CompName = CompName,
                .CompNumber = CompNumber,
                .CompAlphaCode = CompAlphaCode,
                .CompLegalEntity = CompLegalEntity,
                .CompAbrev = CompAbrev,
                .CompStreetAddress1 = "123 Some Street",
                .CompStreetCity = "Some Town",
                .CompStreetState = "IL",
                .CompStreetCountry = "US",
                .CompStreetZip = "60611",
                .CompMailAddress1 = "123 Some Street",
                .CompMailCity = "Some Town",
                .CompMailState = "IL",
                .CompMailCountry = "US",
                .CompMailZip = "60611"}

    End Function


End Class
