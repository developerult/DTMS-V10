Namespace Models

    ''' <summary>
    ''' Map generic drive days to standard class,  typically used for Drive Saturday and Sunday logic
    ''' but any day of week can be used
    ''' </summary>
    Public Class DriveDays

        Private _Control As Integer?
        Public Property Control() As Integer?
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer?)
                _Control = value
            End Set
        End Property


        Private _DriveMon As Boolean? = True
        Public Property DriveMon() As Boolean?
            Get
                Return _DriveMon
            End Get
            Set(ByVal value As Boolean?)
                _DriveMon = value
            End Set
        End Property

        Private _DriveTue As Boolean? = True
        Public Property DriveTue() As Boolean?
            Get
                Return _DriveTue
            End Get
            Set(ByVal value As Boolean?)
                _DriveTue = value
            End Set
        End Property

        Private _DriveWed As Boolean? = True
        Public Property DriveWed() As Boolean?
            Get
                Return _DriveWed
            End Get
            Set(ByVal value As Boolean?)
                _DriveWed = value
            End Set
        End Property

        Private _DriveThur As Boolean? = True
        Public Property DriveThur() As Boolean?
            Get
                Return _DriveThur
            End Get
            Set(ByVal value As Boolean?)
                _DriveThur = value
            End Set
        End Property

        Private _DriveFri As Boolean? = True
        Public Property DriveFri() As Boolean?
            Get
                Return _DriveFri
            End Get
            Set(ByVal value As Boolean?)
                _DriveFri = value
            End Set
        End Property

        Private _DriveSat As Boolean? = False
        Public Property DriveSat() As Boolean?
            Get
                Return _DriveSat
            End Get
            Set(ByVal value As Boolean?)
                _DriveSat = value
            End Set
        End Property

        Private _DriveSun As Boolean? = False
        Public Property DriveSun() As Boolean?
            Get
                Return _DriveSun
            End Get
            Set(ByVal value As Boolean?)
                _DriveSun = value
            End Set
        End Property

    End Class


End Namespace