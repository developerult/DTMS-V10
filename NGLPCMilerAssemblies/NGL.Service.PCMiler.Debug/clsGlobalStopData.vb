<Serializable()> _
Public Class clsGlobalStopData
#Region "Properties"
    Private mintBadAddressControls() As Integer
    Public Property BadAddressControls(ByVal Index As Integer) As Integer
        Get
            If mintBadAddressControls Is Nothing Then Return 0
            If mintBadAddressControls.Length < (Index + 1) Then Return 0
            Return mintBadAddressControls(Index)
        End Get
        Set(ByVal value As Integer)
            If mintBadAddressControls Is Nothing Then
                ReDim mintBadAddressControls(Index)
            ElseIf Index = 0 And mintBadAddressControls.Length < 1 Then
                ReDim mintBadAddressControls(Index)

            ElseIf mintBadAddressControls.Length < (Index + 1) Then
                ReDim Preserve mintBadAddressControls(Index)
            End If
            mintBadAddressControls(Index) = value
        End Set
    End Property

    Private mstrFailedAddressMessage As String = ""
    Public Property FailedAddressMessage() As String
        Get
            Return mstrFailedAddressMessage
        End Get
        Set(ByVal value As String)
            mstrFailedAddressMessage = value
        End Set
    End Property

    Private mintBadAddressCount As Integer = 0
    Public Property BadAddressCount() As Integer
        Get
            Return mintBadAddressCount
        End Get
        Set(ByVal value As Integer)
            mintBadAddressCount = value
        End Set
    End Property

    Private mdblTotalMiles As Double = 0
    Public Property TotalMiles() As Double
        Get
            Return mdblTotalMiles
        End Get
        Set(ByVal value As Double)
            mdblTotalMiles = value
        End Set
    End Property

    Private mstrOriginZip As String = ""
    Public Property OriginZip() As String
        Get
            Return mstrOriginZip
        End Get
        Set(ByVal value As String)
            mstrOriginZip = value
        End Set
    End Property

    Private mstrDestZip As String = ""
    Public Property DestZip() As String
        Get
            Return mstrDestZip
        End Get
        Set(ByVal value As String)
            mstrDestZip = value
        End Set
    End Property

    Private mdblAutoCorrectBadLaneZipCodes As Double = 0
    Public Property AutoCorrectBadLaneZipCodes() As Double
        Get
            Return mdblAutoCorrectBadLaneZipCodes
        End Get
        Set(ByVal value As Double)
            mdblAutoCorrectBadLaneZipCodes = value
        End Set
    End Property

    Private mdblBatchID As Double = 0
    Public Property BatchID() As Double
        Get
            Return mdblBatchID
        End Get
        Set(ByVal value As Double)
            mdblBatchID = value
        End Set
    End Property

    Private mstrLastError As String = ""
    Public ReadOnly Property LastError() As String
        Get
            Return mstrLastError
        End Get
    End Property

#End Region
End Class
