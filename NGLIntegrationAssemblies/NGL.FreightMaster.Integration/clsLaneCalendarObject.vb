<Serializable()> _
Public Class clsLaneCalendarObject
    Public LaneNumber As String = ""
    Public Month As Integer = 0
    Public Day As Integer = 0
    Public Open As Boolean = True
    Public StartTime As String = ""
    Public EndTime As String = ""
    Public IsHoliday As Boolean = False
    Public ApplyToOrigin As Boolean = False

End Class

<Serializable()> _
Public Class clsLaneCalendarObject60 : Inherits clsImportDataBase

    Private _LaneNumber As String = ""
    Public Property LaneNumber As String
        Get
            Return Left(_LaneNumber, 50)
        End Get
        Set(value As String)
            _LaneNumber = Left(value, 50)
        End Set
    End Property

    Private _Month As Integer = 0
    Public Property Month As Integer
        Get
            Return _Month
        End Get
        Set(value As Integer)
            _Month = value
        End Set
    End Property

    Private _Day As Integer = 0
    Public Property Day As Integer
        Get
            Return _Day
        End Get
        Set(value As Integer)
            _Day = value
        End Set
    End Property

    Private _Open As Boolean = True
    Public Property Open As Boolean
        Get
            Return _Open
        End Get
        Set(value As Boolean)
            _Open = value
        End Set
    End Property

    Private _StartTime As String = ""
    Public Property StartTime As String
        Get
            Return CleanTime(_StartTime)
        End Get
        Set(value As String)
            _StartTime = value
        End Set
    End Property

    Private _EndTime As String = ""
    Public Property EndTime As String
        Get
            Return CleanTime(_EndTime)
        End Get
        Set(value As String)
            _EndTime = value
        End Set
    End Property

    Private _IsHoliday As Boolean = False
    Public Property IsHoliday As Boolean
        Get
            Return _IsHoliday
        End Get
        Set(value As Boolean)
            _IsHoliday = value
        End Set
    End Property

    Private _ApplyToOrigin As Boolean = False
    Public Property ApplyToOrigin As Boolean
        Get
            Return _ApplyToOrigin
        End Get
        Set(value As Boolean)
            _ApplyToOrigin = value
        End Set
    End Property

End Class

<Serializable()>
Public Class clsLaneCalendarObject70 : Inherits clsLaneCalendarObject60
    Private _LaneLegalEntity As String = ""
    Public Property LaneLegalEntity As String
        Get
            Return Left(_LaneLegalEntity, 50)
        End Get
        Set(value As String)
            _LaneLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _LaneCompAlphaCode As String = ""
    Public Property LaneCompAlphaCode() As String
        Get
            Return Left(_LaneCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _LaneCompAlphaCode = Left(value, 50)
        End Set
    End Property

End Class


<Serializable()>
Public Class clsLaneCalendarObject80 : Inherits clsLaneCalendarObject70
    'currently no changes
End Class
