
<Serializable()> _
Public Class clsPalletTypeObject : Inherits clsImportDataBase


    Private _PalletType As String = ""
    Public Property PalletType As String
        Get
            Return Left(_PalletType, 50)
        End Get
        Set(value As String)
            _PalletType = Left(value, 50)
        End Set
    End Property

    Private _PalletTypeDescription As String = ""
    Public Property PalletTypeDescription As String
        Get
            Return Left(_PalletTypeDescription, 50)
        End Get
        Set(value As String)
            _PalletTypeDescription = Left(value, 50)
        End Set
    End Property


    Private _PalletTypeWeight As Double = 0
    Public Property PalletTypeWeight As Double
        Get
            Return _PalletTypeWeight
        End Get
        Set(value As Double)
            _PalletTypeWeight = value
        End Set
    End Property

    Private _PalletTypeHeight As Double = 0
    Public Property PalletTypeHeight As Double
        Get
            Return _PalletTypeHeight
        End Get
        Set(value As Double)
            _PalletTypeHeight = value
        End Set
    End Property

    Private _PalletTypeWidth As Double = 0
    Public Property PalletTypeWidth As Double
        Get
            Return _PalletTypeWidth
        End Get
        Set(value As Double)
            _PalletTypeWidth = value
        End Set
    End Property

    Private _PalletTypeDepth As Double = 0
    Public Property PalletTypeDepth As Double
        Get
            Return _PalletTypeDepth
        End Get
        Set(value As Double)
            _PalletTypeDepth = value
        End Set
    End Property

    Private _PalletTypeVolume As Double = 0
    Public Property PalletTypeVolume As Double
        Get
            Return _PalletTypeVolume
        End Get
        Set(value As Double)
            _PalletTypeVolume = value
        End Set
    End Property

    Private _PalletTypeContainer As Boolean = False
    Public Property PalletTypeContainer As Boolean
        Get
            Return _PalletTypeContainer
        End Get
        Set(value As Boolean)
            _PalletTypeContainer = value
        End Set
    End Property


    Public Shared Function GenerateSampleObject() As clsPalletTypeObject

        Return New clsPalletTypeObject With { _
            .PalletType = "T",
            .PalletTypeDescription = "Test",
            .PalletTypeWeight = 20,
            .PalletTypeHeight = 48,
            .PalletTypeWidth = 48,
            .PalletTypeDepth = 48,
            .PalletTypeVolume = 0,
            .PalletTypeContainer = 0}

    End Function



End Class
