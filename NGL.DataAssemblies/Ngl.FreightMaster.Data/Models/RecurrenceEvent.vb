Namespace Models

    Public Class RecurrenceEvent

        Private _Id As Integer
        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal value As Integer)
                _Id = value
            End Set
        End Property

        Private _Title As String
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set
        End Property

        Private _Description As String
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Private _StartDate As Date
        ''' <summary>
        ''' Maps to Start on Kendo Recurrence Event class
        ''' </summary>
        ''' <returns></returns>
        Public Property StartDate() As Date
            Get
                Return _StartDate
            End Get
            Set(ByVal value As Date)
                _StartDate = value
            End Set
        End Property

        Private _StartTimezone As String
        ''' <summary>
        ''' Start Time IANA time zone like "Etc/UTC" , Generally empty to use client time zone for recurrence so 9 am will be 9 on the schedule.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' see https://en.wikipedia.org/wiki/List_of_tz_database_time_zones for more details on time zones
        ''' see https://github.com/mj1856/TimeZoneConverter for a time zone converter
        ''' </remarks>
        Public Property StartTimezone() As String
            Get
                Return _StartTimezone
            End Get
            Set(ByVal value As String)
                _StartTimezone = value
            End Set
        End Property

        Private _EndDate As Date
        ''' <summary>
        ''' Maps to End on Kendo Recurrence Event class
        ''' </summary>
        ''' <returns></returns>
        Public Property EndDate() As Date
            Get
                Return _EndDate
            End Get
            Set(ByVal value As Date)
                _EndDate = value
            End Set
        End Property

        Private _EndTimezone As String
        ''' <summary>
        ''' End Time IANA time zone like "Etc/UTC", Generally empty to use client time zone for recurrence so 9 am will be 9 on the schedule.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' see https://en.wikipedia.org/wiki/List_of_tz_database_time_zones for more details on time zones
        ''' see https://github.com/mj1856/TimeZoneConverter for a time zone converter
        ''' </remarks>
        Public Property EndTimezone() As String
            Get
                Return _EndTimezone
            End Get
            Set(ByVal value As String)
                _EndTimezone = value
            End Set
        End Property

        Private _IsAllDay As Boolean = False
        Public Property IsAllDay() As Boolean
            Get
                Return _IsAllDay
            End Get
            Set(ByVal value As Boolean)
                _IsAllDay = value
            End Set
        End Property

        Private _recurrenceException As String
        Public Property recurrenceException() As String
            Get
                Return _recurrenceException
            End Get
            Set(ByVal value As String)
                _recurrenceException = value
            End Set
        End Property

        Private _recurrenceId As String
        Public Property recurrenceId() As String
            Get
                Return _recurrenceId
            End Get
            Set(ByVal value As String)
                _recurrenceId = value
            End Set
        End Property

        Private _recurrenceRule As String
        ''' <summary>
        ''' use the RecurrenceRule class to string method
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' save data as a Recurrence Rule and call the to string override method to populate 
        ''' NO need to store this value in the database 
        ''' </remarks>
        Public ReadOnly Property recurrenceRule() As String
            Get
                If Rule Is Nothing Then
                    Return ""
                Else
                    Return Rule.toString()
                End If
            End Get

        End Property

        Private _Rule As New RecurrenceRule()
        ''' <summary>
        ''' This is a child (table) of the the event
        ''' </summary>
        ''' <returns></returns>
        Public Property Rule() As RecurrenceRule
            Get
                Return _Rule
            End Get
            Set(ByVal value As RecurrenceRule)
                _Rule = value
            End Set
        End Property

        'Added By LVV On 7/23/18 For v-8.3 TMS365 Scheduler
        Private _DockControl As Integer
        Public Property DockControl() As Integer
            Get
                Return _DockControl
            End Get
            Set(ByVal value As Integer)
                _DockControl = value
            End Set
        End Property

        Private _DockBlockExpired As Boolean
        Public Property DockBlockExpired() As Boolean
            Get
                Return _DockBlockExpired
            End Get
            Set(ByVal value As Boolean)
                _DockBlockExpired = value
            End Set
        End Property

        Private _DockBlockOn As Boolean
        Public Property DockBlockOn() As Boolean
            Get
                Return _DockBlockOn
            End Get
            Set(ByVal value As Boolean)
                _DockBlockOn = value
            End Set
        End Property

        Private _RecTypeColorCode As String
        Public Property RecTypeColorCode() As String
            Get
                Return _RecTypeColorCode
            End Get
            Set(ByVal value As String)
                _RecTypeColorCode = value
            End Set
        End Property

        Private _DockDoorName As String
        Public Property DockDoorName() As String
            Get
                Return _DockDoorName
            End Get
            Set(ByVal value As String)
                _DockDoorName = value
            End Set
        End Property

        Private _DockDoorID As String
        Public Property DockDoorID() As String
            Get
                Return _DockDoorID
            End Get
            Set(ByVal value As String)
                _DockDoorID = value
            End Set
        End Property

        ''' <summary>
        ''' True if this event is scheduled on dtVal
        ''' </summary>
        ''' <param name="dtVal"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Used to determine if this event is available on a particular day
        ''' </remarks>
        Public Function isEventOnDate(ByVal dtVal As Date) As Boolean
            Dim dtTo As Date = dtVal.ToShortDateString()
            If Not Rule.isRecurrenceOnDate(dtVal) Then Return False
            Dim dtStart = StartDate.ToShortDateString()
            If dtStart > dtTo Then Return False
            Dim blnIsRecurrence = True
            If Rule.UNTIL.HasValue = False And Rule.COUNT > 0 Then
                Dim iInstances As Integer = 0
                Dim dtNew As Date = dtStart
                While iInstances < Rule.COUNT
                    If Rule.isRecurrenceOnDate(dtNew) Then iInstances += 1
                    dtNew = dtNew.AddDays(1)
                    If dtNew > dtTo Then Exit While 'no more dates to check
                End While
                If iInstances > Rule.COUNT Then blnIsRecurrence = False

            End If
            Return blnIsRecurrence

        End Function

        ''' <summary>
        ''' True if scheduled on dtVal, dtStart and dtEnd use dtVal with start and End times if True else null
        ''' </summary>
        ''' <param name="dtVal"></param>
        ''' <param name="dtStart"></param>
        ''' <param name="dtEnd"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' if true but blnallday is false and dtstart or dtend are null there is a problem with the recurrence configuration and the result is false
        ''' </remarks>
        Public Function getAppointmentByDate(ByVal dtVal As Date, ByRef dtStart As Date?, ByRef dtEnd As Date?, ByRef blnAllDay As Boolean) As Boolean
            dtStart = Nothing
            dtEnd = Nothing
            Dim dtTest As Date
            blnAllDay = IsAllDay
            If isEventOnDate(dtVal) Then
                If IsAllDay Then Return True
                Dim sStart = dtVal.ToShortDateString() & " " & StartDate.ToShortTimeString()
                Dim sEnd = dtVal.ToShortDateString() & " " & EndDate.ToShortTimeString()
                If Date.TryParse(sStart, dtTest) Then
                    dtStart = dtTest
                Else
                    Return False 'invalid recurrence configuration
                End If
                If Date.TryParse(sEnd, dtTest) Then
                    dtEnd = dtTest
                Else
                    Return False 'invalid recurrence configuration
                End If
                Return True
            Else
                Return False
            End If
        End Function


    End Class

    'java script example:
    'var Event = New kendo.data.SchedulerEvent({
    '    id: 1,
    '    start: New Date("2013/9/2 12:00"),
    '    End :  New Date("2013/9/2 12:30"),
    '    title: "Lunch",
    '    recurrenceRule: "FREQ=DAILY",
    '    recurrenceException: New Date("2013/9/3 12:00").toISOString()
    '});
    'var exception = New kendo.data.SchedulerEvent({
    '    id:  2,
    '    start: New Date("2013/9/3 12:30"),
    '    End :  New Date("2013/9/3 13:00"),
    '    title: "Lunch",
    '    recurrenceId: 1
    '});

End Namespace

