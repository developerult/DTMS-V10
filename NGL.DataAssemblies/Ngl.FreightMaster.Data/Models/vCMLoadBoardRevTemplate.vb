Namespace Models
    Public Class vCMLoadBoardRevTemplate

        Private _BookControl As Integer
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _Name As String
        Public Property Name() As String
            Get
                Return Left(_Name, 100)
            End Get
            Set(ByVal value As String)
                _Name = Left(value, 100)
            End Set
        End Property

        Private _Description As String
        Public Property Description() As String
            Get
                Return Left(_Description, 100)
            End Get
            Set(ByVal value As String)
                _Description = Left(value, 100)
            End Set
        End Property

        Private _Code As String
        Public Property Code() As String
            Get
                Return Left(_Code, 100)
            End Get
            Set(ByVal value As String)
                _Code = Left(value, 100)
            End Set
        End Property

        Private _Quoted As Decimal
        Public Property Quoted() As Decimal
            Get
                Return _Quoted
            End Get
            Set(ByVal value As Decimal)
                _Quoted = value
            End Set
        End Property

        Private _QvE_Variance As Decimal
        Public Property QvE_Variance() As Decimal
            Get
                Return _QvE_Variance
            End Get
            Set(ByVal value As Decimal)
                _QvE_Variance = value
            End Set
        End Property

        Private _Expected As Decimal
        Public Property Expected() As Decimal
            Get
                Return _Expected
            End Get
            Set(ByVal value As Decimal)
                _Expected = value
            End Set
        End Property

        Private _QvB_Variance As Decimal
        Public Property QvB_Variance() As Decimal
            Get
                Return _QvB_Variance
            End Get
            Set(ByVal value As Decimal)
                _QvB_Variance = value
            End Set
        End Property

        Private _EvB_Variance As Decimal
        Public Property EvB_Variance() As Decimal
            Get
                Return _EvB_Variance
            End Get
            Set(ByVal value As Decimal)
                _EvB_Variance = value
            End Set
        End Property

        Private _Billed As Decimal
        Public Property Billed() As Decimal
            Get
                Return _Billed
            End Get
            Set(ByVal value As Decimal)
                _Billed = value
            End Set
        End Property

        Private _QvP_Variance As Decimal
        Public Property QvP_Variance() As Decimal
            Get
                Return _QvP_Variance
            End Get
            Set(ByVal value As Decimal)
                _QvP_Variance = value
            End Set
        End Property

        Private _EvP_Variance As Decimal
        Public Property EvP_Variance() As Decimal
            Get
                Return _EvP_Variance
            End Get
            Set(ByVal value As Decimal)
                _EvP_Variance = value
            End Set
        End Property

        Private _BvP_Variance As Decimal
        Public Property BvP_Variance() As Decimal
            Get
                Return _BvP_Variance
            End Get
            Set(ByVal value As Decimal)
                _BvP_Variance = value
            End Set
        End Property

        Private _Pending As Decimal
        Public Property Pending() As Decimal
            Get
                Return _Pending
            End Get
            Set(ByVal value As Decimal)
                _Pending = value
            End Set
        End Property


        '******************************************

        Private _QvAj_Variance As Decimal
        Public Property QvAj_Variance() As Decimal
            Get
                Return _QvAj_Variance
            End Get
            Set(ByVal value As Decimal)
                _QvAj_Variance = value
            End Set
        End Property

        Private _EvAj_Variance As Decimal
        Public Property EvAj_Variance() As Decimal
            Get
                Return _EvAj_Variance
            End Get
            Set(ByVal value As Decimal)
                _EvAj_Variance = value
            End Set
        End Property

        Private _BvAj_Variance As Decimal
        Public Property BvAj_Variance() As Decimal
            Get
                Return _BvAj_Variance
            End Get
            Set(ByVal value As Decimal)
                _BvAj_Variance = value
            End Set
        End Property

        Private _PvAj_Variance As Decimal
        Public Property PvAj_Variance() As Decimal
            Get
                Return _PvAj_Variance
            End Get
            Set(ByVal value As Decimal)
                _PvAj_Variance = value
            End Set
        End Property

        Private _Ajusted As Decimal
        Public Property Ajusted() As Decimal
            Get
                Return _Ajusted
            End Get
            Set(ByVal value As Decimal)
                _Ajusted = value
            End Set
        End Property

        '*********************************

        Private _QvA_Variance As Decimal
        Public Property QvA_Variance() As Decimal
            Get
                Return _QvA_Variance
            End Get
            Set(ByVal value As Decimal)
                _QvA_Variance = value
            End Set
        End Property

        Private _EvA_Variance As Decimal
        Public Property EvA_Variance() As Decimal
            Get
                Return _EvA_Variance
            End Get
            Set(ByVal value As Decimal)
                _EvA_Variance = value
            End Set
        End Property

        Private _BvA_Variance As Decimal
        Public Property BvA_Variance() As Decimal
            Get
                Return _BvA_Variance
            End Get
            Set(ByVal value As Decimal)
                _BvA_Variance = value
            End Set
        End Property

        Private _PvA_Variance As Decimal
        Public Property PvA_Variance() As Decimal
            Get
                Return _PvA_Variance
            End Get
            Set(ByVal value As Decimal)
                _PvA_Variance = value
            End Set
        End Property

        Private _AjvA_Variance As Decimal
        Public Property AjvA_Variance() As Decimal
            Get
                Return _AjvA_Variance
            End Get
            Set(ByVal value As Decimal)
                _AjvA_Variance = value
            End Set
        End Property

        Private _Appoved As Decimal
        Public Property Appoved() As Decimal
            Get
                Return _Appoved
            End Get
            Set(ByVal value As Decimal)
                _Appoved = value
            End Set
        End Property

        Private _QvPd_Variance As Decimal
        Public Property QvPd_Variance() As Decimal
            Get
                Return _QvPd_Variance
            End Get
            Set(ByVal value As Decimal)
                _QvPd_Variance = value
            End Set
        End Property

        Private _EvPd_Variance As Decimal
        Public Property EvPd_Variance() As Decimal
            Get
                Return _EvPd_Variance
            End Get
            Set(ByVal value As Decimal)
                _EvPd_Variance = value
            End Set
        End Property

        Private _BvPd_Variance As Decimal
        Public Property BvPd_Variance() As Decimal
            Get
                Return _BvPd_Variance
            End Get
            Set(ByVal value As Decimal)
                _BvPd_Variance = value
            End Set
        End Property

        Private _PvPd_Variance As Decimal
        Public Property PvPd_Variance() As Decimal
            Get
                Return _PvPd_Variance
            End Get
            Set(ByVal value As Decimal)
                _PvPd_Variance = value
            End Set
        End Property

        Private _AjvPd_Variance As Decimal
        Public Property AjvPd_Variance() As Decimal
            Get
                Return _AjvPd_Variance
            End Get
            Set(ByVal value As Decimal)
                _AjvPd_Variance = value
            End Set
        End Property

        Private _AvPd_Variance As Decimal
        Public Property AvPd_Variance() As Decimal
            Get
                Return _AvPd_Variance
            End Get
            Set(ByVal value As Decimal)
                _AvPd_Variance = value
            End Set
        End Property

        Private _Paid As Decimal
        Public Property Paid() As Decimal
            Get
                Return _Paid
            End Get
            Set(ByVal value As Decimal)
                _Paid = value
            End Set
        End Property

    End Class


End Namespace


