Namespace Models

    Public Class EquipIDValidation

        Private _BookControl As Integer
        Private _ApptControl As Integer
        Private _EquipID As String
        Private _IsPickup As Boolean
        Private _Success As Boolean
        Private _IsAdd As Boolean
        Private _ErrMsg As String

        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Public Property ApptControl() As Integer
            Get
                Return _ApptControl
            End Get
            Set(ByVal value As Integer)
                _ApptControl = value
            End Set
        End Property

        Public Property EquipID() As String
            Get
                Return _EquipID
            End Get
            Set(ByVal value As String)
                _EquipID = value
            End Set
        End Property

        Public Property IsPickup() As Boolean
            Get
                Return _IsPickup
            End Get
            Set(ByVal value As Boolean)
                _IsPickup = value
            End Set
        End Property

        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Public Property IsAdd() As Boolean
            Get
                Return _IsAdd
            End Get
            Set(ByVal value As Boolean)
                _IsAdd = value
            End Set
        End Property

        Public Property ErrMsg() As String
            Get
                Return _ErrMsg
            End Get
            Set(ByVal value As String)
                _ErrMsg = value
            End Set
        End Property


    End Class


End Namespace

