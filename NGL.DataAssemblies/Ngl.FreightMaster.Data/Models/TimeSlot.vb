Namespace Models

    Public Class TimeSlot

        Private _Start As Date
        Private _End As Date

        Public Property Start() As Date
            Get
                Return _Start
            End Get
            Set(ByVal value As Date)
                _Start = value
            End Set
        End Property

        Public Property [End]() As Date
            Get
                Return _End
            End Get
            Set(ByVal value As Date)
                _End = value
            End Set
        End Property




    End Class


End Namespace

