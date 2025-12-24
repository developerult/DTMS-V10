Public Class Route


    Private _OrigCity As String
    Public Property OrigCity() As String
        Get
            Return _OrigCity
        End Get
        Set(ByVal value As String)
            _OrigCity = value
        End Set
    End Property

    Private _OrigState As String
    Public Property OrigState() As String
        Get
            Return _OrigState
        End Get
        Set(ByVal value As String)
            _OrigState = value
        End Set
    End Property

    Private _OrigZip As String
    Public Property OrigZip() As String
        Get
            Return _OrigZip
        End Get
        Set(ByVal value As String)
            _OrigZip = value
        End Set
    End Property

    Private _DestCity As String
    Public Property DestCity() As String
        Get
            Return _DestCity
        End Get
        Set(ByVal value As String)
            _DestCity = value
        End Set
    End Property

    Private _DestState As String
    Public Property DestState() As String
        Get
            Return _DestState
        End Get
        Set(ByVal value As String)
            _DestState = value
        End Set
    End Property

    Private _DestZip As String
    Public Property DestZip() As String
        Get
            Return _DestZip
        End Get
        Set(ByVal value As String)
            _DestZip = value
        End Set
    End Property

    Private _FlatRate As Double
    Public Property FlatRate() As Double
        Get
            Return _FlatRate
        End Get
        Set(ByVal value As Double)
            _FlatRate = value
        End Set
    End Property

    Private _Miles As Double
    Public Property Miles() As Double
        Get
            Return _Miles
        End Get
        Set(ByVal value As Double)
            _Miles = value
        End Set
    End Property

    Private _MileRate As Double
    Public Property MileRate() As Double
        Get
            Return _MileRate
        End Get
        Set(ByVal value As Double)
            _MileRate = value
        End Set
    End Property

    Private _Message As String
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property




    Public Function getPCMOrigAddress() As String

        Return String.Concat(OrigZip, " ", OrigCity, ",  ", OrigState)

    End Function

    Public Function getPCMDestAddress() As String
        Return String.Concat(DestZip, " ", DestCity, ", ", DestState)
    End Function

    Public Sub calculateMileRate()
        If FlatRate > 0 AndAlso Miles > 0 Then
            MileRate = Math.Round(FlatRate / Miles, 2)
        End If
    End Sub

    Public Function parseInPutCSV(ByVal data As String) As Boolean

        Dim strArray = data.Split(",")
        If strArray.Count() <> 7 Then
            Message = "Invalid Record Format: " & data.Replace(",", "")
            Return False
        End If
        OrigCity = strArray(0)
        OrigState = strArray(1)
        OrigZip = strArray(2)

        DestCity = strArray(3)
        DestState = strArray(4)
        DestZip = strArray(5)
        Double.TryParse(strArray(6), FlatRate)
        Return True
    End Function

    Public Function getOutPutCSV() As String
        Return String.Concat(OrigCity, ",", OrigState, ",", OrigZip, ",", DestCity, ",", DestState, ",", DestZip, ",", FlatRate.ToString(), ",", Miles.ToString(), ",", MileRate.ToString(), ",", Message)
    End Function

End Class
