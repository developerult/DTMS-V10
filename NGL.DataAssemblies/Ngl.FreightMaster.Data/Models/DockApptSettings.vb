Namespace Models
    'Added By LVV On 6/19/18 For v-8.3 TMS365 Scheduler

    Public Class DockApptSettings

        Private _DockControl As Integer
        Private _DockSettingControl As Integer
        Private _MonStart As Date?
        Private _MonEnd As Date?
        Private _MonMaxAppt As Integer
        Private _TueStart As Date?
        Private _TueEnd As Date?
        Private _TueMaxAppt As Integer
        Private _WedStart As Date?
        Private _WedEnd As Date?
        Private _WedMaxAppt As Integer
        Private _ThuStart As Date?
        Private _ThuEnd As Date?
        Private _ThuMaxAppt As Integer
        Private _FriStart As Date?
        Private _FridEnd As Date?
        Private _FriMaxAppt As Integer
        Private _SatStart As Date?
        Private _SatEnd As Date?
        Private _SatMaxAppt As Integer
        Private _SunStart As Date?
        Private _SunEnd As Date?
        Private _SunMaxAppt As Integer
        Private _ApptMinsMin As Integer
        Private _ApptMinsAvg As Integer
        Private _ApptMinsMax As Integer
        'Private _ApptMinsSetup As Integer
        'Private _ApptMinsBreakdown As Integer
        Private _DockSettingDescription As String
        Private _DockSettingOn As Boolean
        Private _DockSettingRequireReasonCode As Boolean
        Private _DockSettingRequireSupervisorPwd As Boolean

        Public Property DockControl() As Integer
            Get
                Return _DockControl
            End Get
            Set(ByVal value As Integer)
                _DockControl = value
            End Set
        End Property

        Public Property DockSettingControl() As Integer
            Get
                Return _DockSettingControl
            End Get
            Set(ByVal value As Integer)
                _DockSettingControl = value
            End Set
        End Property

        Public Property MonStart() As Date?
            Get
                Return _MonStart
            End Get
            Set
                _MonStart = Value
            End Set
        End Property

        Public Property MonEnd() As Date?
            Get
                Return _MonEnd
            End Get
            Set
                _MonEnd = Value
            End Set
        End Property

        Public Property MonMaxAppt() As Integer
            Get
                Return _MonMaxAppt
            End Get
            Set(ByVal value As Integer)
                _MonMaxAppt = value
            End Set
        End Property

        Public Property TueStart() As Date?
            Get
                Return _TueStart
            End Get
            Set
                _TueStart = Value
            End Set
        End Property

        Public Property TueEnd() As Date?
            Get
                Return _TueEnd
            End Get
            Set
                _TueEnd = Value
            End Set
        End Property

        Public Property TueMaxAppt() As Integer
            Get
                Return _TueMaxAppt
            End Get
            Set(ByVal value As Integer)
                _TueMaxAppt = value
            End Set
        End Property

        Public Property WedStart() As Date?
            Get
                Return _WedStart
            End Get
            Set
                _WedStart = Value
            End Set
        End Property

        Public Property WedEnd() As Date?
            Get
                Return _WedEnd
            End Get
            Set
                _WedEnd = Value
            End Set
        End Property

        Public Property WedMaxAppt() As Integer
            Get
                Return _WedMaxAppt
            End Get
            Set(ByVal value As Integer)
                _WedMaxAppt = value
            End Set
        End Property

        Public Property ThuStart() As Date?
            Get
                Return _ThuStart
            End Get
            Set
                _ThuStart = Value
            End Set
        End Property

        Public Property ThuEnd() As Date?
            Get
                Return _ThuEnd
            End Get
            Set
                _ThuEnd = Value
            End Set
        End Property

        Public Property ThuMaxAppt() As Integer
            Get
                Return _ThuMaxAppt
            End Get
            Set(ByVal value As Integer)
                _ThuMaxAppt = value
            End Set
        End Property

        Public Property FriStart() As Date?
            Get
                Return _FriStart
            End Get
            Set
                _FriStart = Value
            End Set
        End Property

        Public Property FridEnd() As Date?
            Get
                Return _FridEnd
            End Get
            Set
                _FridEnd = Value
            End Set
        End Property

        Public Property FriMaxAppt() As Integer
            Get
                Return _FriMaxAppt
            End Get
            Set(ByVal value As Integer)
                _FriMaxAppt = value
            End Set
        End Property

        Public Property SatStart() As Date?
            Get
                Return _SatStart
            End Get
            Set
                _SatStart = Value
            End Set
        End Property

        Public Property SatEnd() As Date?
            Get
                Return _SatEnd
            End Get
            Set
                _SatEnd = Value
            End Set
        End Property

        Public Property SatMaxAppt() As Integer
            Get
                Return _SatMaxAppt
            End Get
            Set(ByVal value As Integer)
                _SatMaxAppt = value
            End Set
        End Property

        Public Property SunStart() As Date?
            Get
                Return _SunStart
            End Get
            Set
                _SunStart = Value
            End Set
        End Property

        Public Property SunEnd() As Date?
            Get
                Return _SunEnd
            End Get
            Set
                _SunEnd = Value
            End Set
        End Property

        Public Property SunMaxAppt() As Integer
            Get
                Return _SunMaxAppt
            End Get
            Set(ByVal value As Integer)
                _SunMaxAppt = value
            End Set
        End Property

        Public Property ApptMinsMin() As Integer
            Get
                Return _ApptMinsMin
            End Get
            Set(ByVal value As Integer)
                _ApptMinsMin = value
            End Set
        End Property

        Public Property ApptMinsAvg() As Integer
            Get
                Return _ApptMinsAvg
            End Get
            Set(ByVal value As Integer)
                _ApptMinsAvg = value
            End Set
        End Property

        Public Property ApptMinsMax() As Integer
            Get
                Return _ApptMinsMax
            End Get
            Set(ByVal value As Integer)
                _ApptMinsMax = value
            End Set
        End Property

        'Public Property ApptMinsSetup() As Integer
        '    Get
        '        Return _ApptMinsSetup
        '    End Get
        '    Set(ByVal value As Integer)
        '        _ApptMinsSetup = value
        '    End Set
        'End Property

        'Public Property ApptMinsBreakdown() As Integer
        '    Get
        '        Return _ApptMinsBreakdown
        '    End Get
        '    Set(ByVal value As Integer)
        '        _ApptMinsBreakdown = value
        '    End Set
        'End Property

        Public Property DockSettingDescription() As String
            Get
                Return _DockSettingDescription
            End Get
            Set(ByVal value As String)
                _DockSettingDescription = value
            End Set
        End Property

        Public Property DockSettingOn() As Boolean
            Get
                Return _DockSettingOn
            End Get
            Set(ByVal value As Boolean)
                _DockSettingOn = value
            End Set
        End Property

        Public Property DockSettingRequireReasonCode() As Boolean
            Get
                Return _DockSettingRequireReasonCode
            End Get
            Set(ByVal value As Boolean)
                _DockSettingRequireReasonCode = value
            End Set
        End Property

        Public Property DockSettingRequireSupervisorPwd() As Boolean
            Get
                Return _DockSettingRequireSupervisorPwd
            End Get
            Set(ByVal value As Boolean)
                _DockSettingRequireSupervisorPwd = value
            End Set
        End Property

    End Class


End Namespace

