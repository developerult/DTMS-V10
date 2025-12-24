Namespace Models


    Public Class FilterDetails

        Private _filterName As String
        Public Property filterName() As String
            Get
                Return _filterName
            End Get
            Set(ByVal value As String)
                _filterName = value
            End Set
        End Property

        Private _filterValueFrom As String
        Public Property filterValueFrom() As String
            Get
                Return _filterValueFrom
            End Get
            Set(ByVal value As String)
                _filterValueFrom = value
            End Set
        End Property

        Private _filterValueTo As String
        Public Property filterValueTo() As String
            Get
                Return _filterValueTo
            End Get
            Set(ByVal value As String)
                _filterValueTo = value
            End Set
        End Property


        Private _filterFrom As Date?
        Public Property filterFrom() As Date?
            Get
                Return _filterFrom
            End Get
            Set
                _filterFrom = Value
            End Set
        End Property


        Private _filterTo As Date?
        Public Property filterTo() As Date?
            Get
                Return _filterTo
            End Get
            Set
                _filterTo = Value
            End Set
        End Property
        'added manoRama to read filter data for Load Planning page.
        Private _filterCaption As String
        Public Property filterCaption() As String
            Get
                Return _filterCaption
            End Get
            Set(ByVal value As String)
                _filterCaption = value
            End Set
        End Property
    End Class

End Namespace
