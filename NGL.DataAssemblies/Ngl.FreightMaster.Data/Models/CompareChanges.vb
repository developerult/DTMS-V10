Namespace Models

    Public Class CompareChanges

        Private _ID As Integer
        Private _FieldName As String
        Private _Caption As String
        Private _OriginalValue As String
        Private _ModifiedValue As String
        Private _ValueType As String

        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Public Property FieldName() As String
            Get
                Return _FieldName
            End Get
            Set(ByVal value As String)
                _FieldName = value
            End Set
        End Property

        Public Property Caption() As String
            Get
                Return _Caption
            End Get
            Set(ByVal value As String)
                _Caption = value
            End Set
        End Property

        Public Property OriginalValue() As String
            Get
                Return _OriginalValue
            End Get
            Set(ByVal value As String)
                _OriginalValue = value
            End Set
        End Property

        Public Property ModifiedValue() As String
            Get
                Return _ModifiedValue
            End Get
            Set(ByVal value As String)
                _ModifiedValue = value
            End Set
        End Property

        Public Property ValueType() As String
            Get
                Return _ValueType
            End Get
            Set(ByVal value As String)
                _ValueType = value
            End Set
        End Property

    End Class

End Namespace

