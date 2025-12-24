<Serializable()> _
Public Class clsCompanyCalendarObject
    Public CompNumber As String = ""
    Public Month As Integer = 0
    Public Day As Integer = 0
    Public Open As Boolean = True
    Public StartTime As String = ""
    Public EndTime As String = ""
    Public IsHoliday As Boolean = False
End Class

<Serializable()> _
Public Class clsCompanyCalendarObject70 : Inherits clsImportDataBase

    Private _CompLegalEntity As String = ""
    Public Property CompLegalEntity As String
        Get
            Return Left(_CompLegalEntity, 50)
        End Get
        Set(value As String)
            _CompLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _CompNumber As Integer = 0
    Public Property CompNumber() As Integer
        Get
            Return _CompNumber
        End Get
        Set(ByVal value As Integer)
            _CompNumber = value
        End Set
    End Property

    Private _CompAlphaCode As String = ""
    Public Property CompAlphaCode() As String
        Get
            Return Left(_CompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _CompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _Month As Integer = 0
    Public Property Month() As Integer
        Get
            Return _Month
        End Get
        Set(ByVal value As Integer)
            _Month = value
        End Set
    End Property

    Private _Day As Integer = 0
    Public Property Day() As Integer
        Get
            Return _Day
        End Get
        Set(ByVal value As Integer)
            _Day = value
        End Set
    End Property

    Private _Open As Boolean = True
    Public Property Open() As Boolean
        Get
            Return _Open
        End Get
        Set(ByVal value As Boolean)
            _Open = value
        End Set
    End Property

    Private _StartTime As String = ""
    Public Property StartTime() As String
        Get
            Return CleanTime(_StartTime)
        End Get
        Set(ByVal value As String)
            _StartTime = value
        End Set
    End Property

    Private _EndTime As String = ""
    Public Property EndTime() As String
        Get
            Return CleanTime(_EndTime)
        End Get
        Set(ByVal value As String)
            _EndTime = value
        End Set
    End Property

    Private _IsHoliday As Boolean = False
    Public Property IsHoliday() As Boolean
        Get
            Return _IsHoliday
        End Get
        Set(ByVal value As Boolean)
            _IsHoliday = value
        End Set
    End Property

End Class
