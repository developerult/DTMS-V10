Public Class clsUnitTestKeys

    Private _Source As String = "NAV Unit Test"
    Public Property Source() As String
        Get
            Return _Source
        End Get
        Set(ByVal value As String)
            _Source = value
        End Set
    End Property

    Private _DBName As String = "NGLMASPROD"
    Public Property DBName() As String
        Get
            Return _DBName
        End Get
        Set(ByVal value As String)
            _DBName = value
        End Set
    End Property

    Private _DBServer As String = "NGLRDP06D"
    Public Property DBServer() As String
        Get
            Return _DBServer
        End Get
        Set(ByVal value As String)
            _DBServer = value
        End Set
    End Property

    Private _ConnectionSting As String
    Public Property ConnectionSting() As String
        Get
            Return _ConnectionSting
        End Get
        Set(ByVal value As String)
            _ConnectionSting = value
        End Set
    End Property

    Private _DBUser As String
    Public Property DBUser() As String
        Get
            Return _DBUser
        End Get
        Set(ByVal value As String)
            _DBUser = value
        End Set
    End Property

    Private _DBPass As String
    Public Property DBPass() As String
        Get
            Return _DBPass
        End Get
        Set(ByVal value As String)
            _DBPass = value
        End Set
    End Property

    Private _Debug As Boolean = True
    Public Property Debug() As Boolean
        Get
            Return _Debug
        End Get
        Set(ByVal value As Boolean)
            _Debug = value
        End Set
    End Property

    Private _Verbos As Boolean = True
    Public Property Verbos() As Boolean
        Get
            Return _Verbos
        End Get
        Set(ByVal value As Boolean)
            _Verbos = value
        End Set
    End Property

    Private _LegalEntity As String
    Public Property LegalEntity() As String
        Get
            Return _LegalEntity
        End Get
        Set(ByVal value As String)
            _LegalEntity = value
        End Set
    End Property

    Private _CompName As String
    Public Property CompName() As String
        Get
            Return _CompName
        End Get
        Set(ByVal value As String)
            _CompName = value
        End Set
    End Property

    Private _CompAbrev As String
    Public Property CompAbrev() As String
        Get
            Return _CompAbrev
        End Get
        Set(ByVal value As String)
            _CompAbrev = value
        End Set
    End Property

    Private _CompAlphaCode As String
    Public Property CompAlphaCode() As String
        Get
            Return _CompAlphaCode
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = value
        End Set
    End Property

    Private _CompNumber As Integer
    Public Property CompNumber() As Integer
        Get
            Return _CompNumber
        End Get
        Set(ByVal value As Integer)
            _CompNumber = value
        End Set
    End Property

    Private _CarrierName As String
    Public Property CarrierName() As String
        Get
            Return _CarrierName
        End Get
        Set(ByVal value As String)
            _CarrierName = value
        End Set
    End Property

    Private _CarrierNumber As Integer
    Public Property CarrierNumber() As Integer
        Get
            Return _CarrierNumber
        End Get
        Set(ByVal value As Integer)
            _CarrierNumber = value
        End Set
    End Property

    Private _CarrierAlphaCode As String
    Public Property CarrierAlphaCode() As String
        Get
            Return _CarrierAlphaCode
        End Get
        Set(ByVal value As String)
            _CarrierAlphaCode = value
        End Set
    End Property


    Private _LaneName As String
    Public Property LaneName() As String
        Get
            Return _LaneName
        End Get
        Set(ByVal value As String)
            _LaneName = value
        End Set
    End Property

    Private _LaneNumber As String
    Public Property LaneNumber() As String
        Get
            Return _LaneNumber
        End Get
        Set(ByVal value As String)
            _LaneNumber = value
        End Set
    End Property

    Private _OrderNumber As String
    Public Property OrderNumber() As String
        Get
            Return _OrderNumber
        End Get
        Set(ByVal value As String)
            _OrderNumber = value
        End Set
    End Property

    Private _PicklistMaxRetry As Integer = 3

    Public Property PicklistMaxRetry() As Integer
        Get
            Return _PicklistMaxRetry
        End Get
        Set(ByVal value As Integer)
            _PicklistMaxRetry = value
        End Set
    End Property

    Private _PicklistRetryMinutes As Integer = 15

    Public Property PicklistRetryMinutes() As Integer
        Get
            Return _PicklistRetryMinutes
        End Get
        Set(ByVal value As Integer)
            _PicklistRetryMinutes = value
        End Set
    End Property

    Private _PicklistMaxRowsReturned As Integer = 100

    Public Property PicklistMaxRowsReturned() As Integer
        Get
            Return _PicklistMaxRowsReturned
        End Get
        Set(ByVal value As Integer)
            _PicklistMaxRowsReturned = value
        End Set
    End Property

    Private _PicklistAutoConfirmation As Boolean = False

    Public Property PicklistAutoConfirmation() As Boolean
        Get
            Return _PicklistAutoConfirmation
        End Get
        Set(ByVal value As Boolean)
            _PicklistAutoConfirmation = value
        End Set
    End Property

    Private _APExportMaxRetry As Integer = 3

    Public Property APExportMaxRetry() As Integer
        Get
            Return _APExportMaxRetry
        End Get
        Set(ByVal value As Integer)
            _APExportMaxRetry = value
        End Set
    End Property

    Private _APExportRetryMinutes As Integer = 15

    Public Property APExportRetryMinutes() As Integer
        Get
            Return _APExportRetryMinutes
        End Get
        Set(ByVal value As Integer)
            _APExportRetryMinutes = value
        End Set
    End Property

    Private _APExportMaxRowsReturned As Integer = 100

    Public Property APExportMaxRowsReturned() As Integer
        Get
            Return _APExportMaxRowsReturned
        End Get
        Set(ByVal value As Integer)
            _APExportMaxRowsReturned = value
        End Set
    End Property

    Private _APExportAutoConfirmation As Boolean = False

    Public Property APExportAutoConfirmation() As Boolean
        Get
            Return _APExportAutoConfirmation
        End Get
        Set(ByVal value As Boolean)
            _APExportAutoConfirmation = value
        End Set
    End Property

    Private _WSAuthCode As String = "WSPROD"

    Public Property WSAuthCode() As String
        Get
            Return Left(_WSAuthCode, 20)
        End Get
        Set(ByVal value As String)
            _WSAuthCode = Left(value, 20)
        End Set
    End Property

    Private _WSURL As String = ""

    Public Property WSURL() As String
        Get
            Return Left(_WSURL, 1000)
        End Get
        Set(ByVal value As String)
            _WSURL = Left(value, 1000)
        End Set
    End Property

    Private _WCFAuthCode As String = "WCFPROD"

    Public Property WCFAuthCode() As String
        Get
            Return Left(_WCFAuthCode, 20)
        End Get
        Set(ByVal value As String)
            _WCFAuthCode = Left(value, 20)
        End Set
    End Property

    Private _WCFURL As String = ""

    Public Property WCFURL() As String
        Get
            Return Left(_WCFURL, 1000)
        End Get
        Set(ByVal value As String)
            _WCFURL = Left(value, 1000)
        End Set
    End Property

    Private _WCFTCPURL As String = ""

    Public Property WCFTCPURL() As String
        Get
            Return Left(_WCFTCPURL, 1000)
        End Get
        Set(ByVal value As String)
            _WCFTCPURL = Left(value, 1000)
        End Set
    End Property

    Private _FreightCost As Double = 1000.0
    Public Property FreightCost() As Double
        Get
            Return _FreightCost
        End Get
        Set(ByVal value As Double)
            _FreightCost = value
        End Set
    End Property

    Private _FreightBillNumber As String = "FB Unit Test"
    Public Property FreightBillNumber() As String
        Get
            Return _FreightBillNumber
        End Get
        Set(ByVal value As String)
            _FreightBillNumber = value
        End Set
    End Property





End Class
