Namespace Models
    'Created by LVV on 5/27/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues
    'Intended to work with a selection widget in the 365 UI

    Public Class SelectableGridItem

        Private _SGItemBitPos As Integer
        Private _SGItemCaption As String
        Private _SGItemOn As Boolean

        ''' <summary>The bit position of the item</summary>
        Public Property SGItemBitPos() As Integer
            Get
                Return _SGItemBitPos
            End Get
            Set(ByVal value As Integer)
                _SGItemBitPos = value
            End Set
        End Property

        ''' <summary>The caption to show in the UI selection grid</summary>
        Public Property SGItemCaption() As String
            Get
                Return _SGItemCaption
            End Get
            Set(ByVal value As String)
                _SGItemCaption = value
            End Set
        End Property

        ''' <summary>Stores if the bit position of the item is on or off</summary>
        Public Property SGItemOn() As Boolean
            Get
                Return _SGItemOn
            End Get
            Set(ByVal value As Boolean)
                _SGItemOn = value
            End Set
        End Property

    End Class

    Public Class SelectableGridSave

        Private _Control As Integer
        Private _BitPositionsOn() As Integer

        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                _Control = value
            End Set
        End Property

        ''' <summary>The bit positions of all the items the use selected to be on via the UI</summary>
        Public Property BitPositionsOn() As Integer()
            Get
                Return _BitPositionsOn
            End Get
            Set(ByVal value As Integer())
                _BitPositionsOn = value
            End Set
        End Property

    End Class

    Public Class MultiSelectBatchObjects

        Private _Controls() As Integer
        Private _SelectedBits() As Integer
        Private _ConfigType As Integer
        Private _Action As Integer

        Public Property Controls() As Integer()
            Get
                Return _Controls
            End Get
            Set(ByVal value As Integer())
                _Controls = value
            End Set
        End Property

        Public Property SelectedBits() As Integer()
            Get
                Return _SelectedBits
            End Get
            Set(ByVal value As Integer())
                _SelectedBits = value
            End Set
        End Property

        Public Property ConfigType() As Integer
            Get
                Return _ConfigType
            End Get
            Set(ByVal value As Integer)
                _ConfigType = value
            End Set
        End Property

        Public Property Action() As Integer
            Get
                Return _Action
            End Get
            Set(ByVal value As Integer)
                _Action = value
            End Set
        End Property

    End Class

End Namespace