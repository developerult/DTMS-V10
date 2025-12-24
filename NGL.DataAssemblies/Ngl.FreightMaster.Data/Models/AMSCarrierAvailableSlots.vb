Namespace Models

    Public Class AMSCarrierAvailableSlots

        Private _ApptControl As Integer
        Private _Date As Date
        Private _StartTime As Date
        ''Private _EndTime As Date
        Private _EndTime As String
        Private _Docks As String
        Private _Warehouse As String
        Private _Books As String
        Private _CarrierNumber As Integer
        Private _CarrierName As String
        Private _CompControl As Integer
        Private _CarrierControl As Integer
        Public Property ApptControl() As Integer
            Get
                Return _ApptControl
            End Get
            Set(ByVal value As Integer)
                _ApptControl = value
            End Set
        End Property

        Public Property [Date]() As Date
            Get
                Return _Date
            End Get
            Set(ByVal value As Date)
                _Date = value
            End Set
        End Property

        Public Property StartTime() As Date
            Get
                Return _StartTime
            End Get
            Set(ByVal value As Date)
                _StartTime = value
            End Set
        End Property

        ''Public Property EndTime() As Date
        ''    Get
        ''        Return _EndTime
        ''    End Get
        ''    Set(ByVal value As Date)
        ''        _EndTime = value
        ''    End Set
        ''End Property
        Public Property EndTime() As String
            Get
                Return _EndTime
            End Get
            Set(ByVal value As String)
                _EndTime = value
            End Set
        End Property

        Public Property Docks() As String
            Get
                Return _Docks
            End Get
            Set(ByVal value As String)
                _Docks = value
            End Set
        End Property

        Public Property Warehouse() As String
            Get
                Return _Warehouse
            End Get
            Set(ByVal value As String)
                _Warehouse = value
            End Set
        End Property

        Public Property Books() As String
            Get
                Return _Books
            End Get
            Set(ByVal value As String)
                _Books = value
            End Set
        End Property

        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property

        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property

        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

    End Class


End Namespace

