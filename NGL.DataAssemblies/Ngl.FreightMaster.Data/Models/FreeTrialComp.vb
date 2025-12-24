Namespace Models

    Public Class FreeTrialComp

        Private _UserSecurityControl As Integer
        Private _CompControl As Integer
        Private _CompLegalEntity As String
        Private _CompName As String
        Private _CompNumber As Integer
        Private _ShipFromAddress1 As String
        Private _ShipFromAddress2 As String
        Private _ShipFromAddress3 As String
        Private _ShipFromCity As String
        Private _ShipFromState As String
        Private _ShipFromZip As String
        Private _ShipFromCountry As String
        Private _CompAbrev As String
        Private _CompAlphaCode As String
        Private _CompContName As String
        Private _CompContTitle As String
        Private _CompCont800 As String
        Private _CompContPhone As String
        Private _CompContPhoneExt As String
        Private _CompContFax As String
        Private _CompContEmail As String
        Private _ValidationMsg As String
        Private _WarningMsg As String

        Public Property UserSecurityControl() As Integer
            Get
                Return _UserSecurityControl
            End Get
            Set
                _UserSecurityControl = Value
            End Set
        End Property

        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set
                _CompControl = Value
            End Set
        End Property

        Public Property CompLegalEntity() As String
            Get
                Return _CompLegalEntity
            End Get
            Set(ByVal value As String)
                _CompLegalEntity = value
            End Set
        End Property

        Public Property CompName() As String
            Get
                Return _CompName
            End Get
            Set(ByVal value As String)
                _CompName = value
            End Set
        End Property

        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set
                _CompNumber = Value
            End Set
        End Property

        Public Property ShipFromAddress1() As String
            Get
                Return _ShipFromAddress1
            End Get
            Set(ByVal value As String)
                _ShipFromAddress1 = value
            End Set
        End Property

        Public Property ShipFromAddress2() As String
            Get
                Return _ShipFromAddress2
            End Get
            Set(ByVal value As String)
                _ShipFromAddress2 = value
            End Set
        End Property

        Public Property ShipFromAddress3() As String
            Get
                Return _ShipFromAddress3
            End Get
            Set(ByVal value As String)
                _ShipFromAddress3 = value
            End Set
        End Property

        Public Property ShipFromCity() As String
            Get
                Return _ShipFromCity
            End Get
            Set(ByVal value As String)
                _ShipFromCity = value
            End Set
        End Property

        Public Property ShipFromState() As String
            Get
                Return _ShipFromState
            End Get
            Set(ByVal value As String)
                _ShipFromState = value
            End Set
        End Property

        Public Property ShipFromZip() As String
            Get
                Return _ShipFromZip
            End Get
            Set(ByVal value As String)
                _ShipFromZip = value
            End Set
        End Property

        Public Property ShipFromCountry() As String
            Get
                Return _ShipFromCountry
            End Get
            Set(ByVal value As String)
                _ShipFromCountry = value
            End Set
        End Property

        Public Property CompAbrev() As String
            Get
                Return _CompAbrev
            End Get
            Set(ByVal value As String)
                _CompAbrev = value
            End Set
        End Property

        Public Property CompAlphaCode() As String
            Get
                Return _CompAlphaCode
            End Get
            Set(ByVal value As String)
                _CompAlphaCode = value
            End Set
        End Property

        Public Property CompContName() As String
            Get
                Return _CompContName
            End Get
            Set(ByVal value As String)
                _CompContName = value
            End Set
        End Property

        Public Property CompContTitle() As String
            Get
                Return _CompContTitle
            End Get
            Set(ByVal value As String)
                _CompContTitle = value
            End Set
        End Property

        Public Property CompCont800() As String
            Get
                Return _CompCont800
            End Get
            Set(ByVal value As String)
                _CompCont800 = value
            End Set
        End Property

        Public Property CompContPhone() As String
            Get
                Return _CompContPhone
            End Get
            Set(ByVal value As String)
                _CompContPhone = value
            End Set
        End Property

        Public Property CompContPhoneExt() As String
            Get
                Return _CompContPhoneExt
            End Get
            Set(ByVal value As String)
                _CompContPhoneExt = value
            End Set
        End Property

        Public Property CompContFax() As String
            Get
                Return _CompContFax
            End Get
            Set(ByVal value As String)
                _CompContFax = value
            End Set
        End Property

        Public Property CompContEmail() As String
            Get
                Return _CompContEmail
            End Get
            Set(ByVal value As String)
                _CompContEmail = value
            End Set
        End Property

        Public Property ValidationMsg() As String
            Get
                Return _ValidationMsg
            End Get
            Set(ByVal value As String)
                _ValidationMsg = value
            End Set
        End Property

        Public Property WarningMsg() As String
            Get
                Return _WarningMsg
            End Get
            Set(ByVal value As String)
                _WarningMsg = value
            End Set
        End Property

    End Class


End Namespace

