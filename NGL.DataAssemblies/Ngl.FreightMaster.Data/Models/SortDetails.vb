Namespace Models

    Public Class SortDetails
        Private _sortName As String
        Public Property sortName() As String
            Get
                Return _sortName
            End Get
            Set(ByVal value As String)
                _sortName = value
            End Set
        End Property

        Private _sortDirection As String
        Public Property sortDirection() As String
            Get
                Return _sortDirection
            End Get
            Set(ByVal value As String)
                _sortDirection = value
            End Set
        End Property

    End Class

End Namespace