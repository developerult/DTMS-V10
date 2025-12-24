Namespace Model

    <Serializable()> Public Class Map

        Private _MapProvider As IMapProvider

        Public Property MapProvider() As IMapProvider
            Get
                Return _MapProvider
            End Get
            Set(ByVal value As IMapProvider)
                _MapProvider = value
            End Set
        End Property

    End Class

End Namespace

