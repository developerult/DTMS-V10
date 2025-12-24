Namespace Models
    'Added By RHR for v-8.2 on o8/31/2018
    'no need for a view but we do need the model for JSON
    'using this model follows other design patterns for Tariff Exception data processing
    Public Class mCarrierTariffInterline


        Private _CarrTarInterlineControl As Integer
        Public Property CarrTarInterlineControl() As Integer
            Get
                Return _CarrTarInterlineControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarInterlineControl = value
            End Set
        End Property

        Private _CarrTarInterlineCarrTarControl As Integer
        Public Property CarrTarInterlineCarrTarControl() As Integer
            Get
                Return _CarrTarInterlineCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrTarInterlineCarrTarControl = value
            End Set
        End Property

        Private _CarrTarInterlineName As String
        Public Property CarrTarInterlineName() As String
            Get
                Return _CarrTarInterlineName
            End Get
            Set(ByVal value As String)
                _CarrTarInterlineName = value
            End Set
        End Property

        Private _CarrTarInterlineEffDateFrom As Date?
        Public Property CarrTarInterlineEffDateFrom() As Date?
            Get
                Return _CarrTarInterlineEffDateFrom
            End Get
            Set(ByVal value As Date?)
                _CarrTarInterlineEffDateFrom = value
            End Set
        End Property

        Private _CarrTarInterlineEffDateTo As Date?
        Public Property CarrTarInterlineEffDateTo() As Date?
            Get
                Return _CarrTarInterlineEffDateTo
            End Get
            Set(ByVal value As Date?)
                _CarrTarInterlineEffDateTo = value
            End Set
        End Property

        Private _CarrTarInterlineCountry As String
        Public Property CarrTarInterlineCountry() As String
            Get
                Return _CarrTarInterlineCountry
            End Get
            Set(ByVal value As String)
                _CarrTarInterlineCountry = value
            End Set
        End Property

        Private _CarrTarInterlineState As String
        Public Property CarrTarInterlineState() As String
            Get
                Return _CarrTarInterlineState
            End Get
            Set(ByVal value As String)
                _CarrTarInterlineState = value
            End Set
        End Property

        Private _CarrTarInterlineCity As String
        Public Property CarrTarInterlineCity() As String
            Get
                Return _CarrTarInterlineCity
            End Get
            Set(ByVal value As String)
                _CarrTarInterlineCity = value
            End Set
        End Property

        Private _CarrTarInterlineZip As String
        Public Property CarrTarInterlineZip() As String
            Get
                Return _CarrTarInterlineZip
            End Get
            Set(ByVal value As String)
                _CarrTarInterlineZip = value
            End Set
        End Property

        Private _CarrTarInterlineModUser As String
        Public Property CarrTarInterlineModUser() As String
            Get
                Return _CarrTarInterlineModUser
            End Get
            Set(ByVal value As String)
                _CarrTarInterlineModUser = value
            End Set
        End Property

        Private _CarrTarInterlineModDate As Date
        Public Property CarrTarInterlineModDate() As Date
            Get
                Return _CarrTarInterlineModDate
            End Get
            Set(ByVal value As Date)
                _CarrTarInterlineModDate = value
            End Set
        End Property

    End Class

End Namespace


