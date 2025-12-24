Namespace Models

    Public Enum RecurranceRuleFREQ
        SECONDLY
        MINUTELY
        HOURLY
        DAILY
        WEEKLY
        MONTHLY
        YEARLY
    End Enum

    Public Enum RecurranceWeekday
        SU
        MO
        TU
        WE
        TH
        FR
        SA
    End Enum
    Public Class RecurrenceRule

        Private _FREQ As RecurranceRuleFREQ = RecurranceRuleFREQ.WEEKLY
        ''' <summary>
        ''' frequency enum in v-8.2 value is read only set to WEEKLY
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' FREQ rule part identifies the type of recurrence rule.  This
        ''' rule part MUST be specified In the recurrence rule.  Valid values
        ''' include SECONDLY, to specify repeating events based On an interval
        ''' of a second Or more; MINUTELY, to specify repeating events based
        ''' On an interval of a minute Or more; HOURLY, to specify repeating
        ''' events based On an interval Of an hour Or more; DAILY, To specify
        ''' repeating events based On an interval Of a day Or more; WEEKLY, To
        ''' specify repeating events based On an interval Of a week Or more;
        ''' MONTHLY, to specify repeating events based on an interval of a
        ''' month Or more; And YEARLY, to specify repeating events based on an
        ''' interval of a year Or more.
        ''' </remarks>
        Public ReadOnly Property FREQ() As RecurranceRuleFREQ
            Get
                Return _FREQ
            End Get
            ' in 8.2 FREQ is read only 
            'Set(ByVal value As RecurranceRuleFREQ)

            '    _FREQ = value
            'End Set
        End Property

        Private _UNTIL As Date?
        ''' <summary>
        ''' UTC Date for end of Recurrance
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' only one of UNTIL for COUNT are alowed,  UNTIL is processed first
        ''' </remarks>
        Public Property UNTIL() As Date?
            Get
                Return _UNTIL
            End Get
            Set(ByVal value As Date?)
                _UNTIL = value
            End Set
        End Property

        Private _COUNT As Integer
        Public Property COUNT() As Integer
            Get
                Return _COUNT
            End Get
            Set(ByVal value As Integer)
                _COUNT = value
            End Set
        End Property

        Private _INTERVAL As Integer = 0
        ''' <summary>
        ''' in 8.2 interval should be zero or 1 because we only support every week
        ''' </summary>
        ''' <returns></returns>
        Public Property INTERVAL() As Integer
            Get
                Return _INTERVAL
            End Get
            Set(ByVal value As Integer)
                _INTERVAL = value
            End Set
        End Property

        'Private _BYSECOND As Integer
        '''' <summary>
        '''' Not supported in 8.2 An integer  value between 0 and 60; 0 indicates that the value will not be used
        '''' </summary>
        '''' <returns></returns>
        'Public Property BYSECOND() As Integer
        '    Get
        '        Return _BYSECOND
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < 0 Or value > 60 Then
        '            _BYSECOND = 0
        '            Throw New System.ArgumentOutOfRangeException("BYSECOND", value, "Value must be between 0 and 60;  the value will be set to zero as the default.")
        '        Else
        '            _BYSECOND = value
        '        End If
        '    End Set
        'End Property

        'Private _BYMINUTE As Integer
        '''' <summary>
        '''' Not supported in 8.2 An integer  value between 0 and 59; 0 indicates that the value will not be used
        '''' </summary>
        '''' <returns></returns>
        'Public Property BYMINUTE() As Integer
        '    Get
        '        Return _BYMINUTE
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < 0 Or value > 59 Then
        '            _BYMINUTE = 0
        '            Throw New System.ArgumentOutOfRangeException("BYMINUTE", value, "Value must be between 0 and 59;  the value will be set to zero as the default.")
        '        Else
        '            _BYMINUTE = value
        '        End If
        '    End Set
        'End Property

        'Private _BYHOUR As Integer
        '''' <summary>
        '''' Not supported in 8.2 An integer  value between 0 and 23; 0 indicates that the value will not be used
        '''' </summary>
        '''' <returns></returns>
        'Public Property BYHOUR() As Integer
        '    Get
        '        Return _BYHOUR
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < 0 Or value > 23 Then
        '            _BYHOUR = 0
        '            Throw New System.ArgumentOutOfRangeException("BYHOUR", value, "Value must be between 0 and 23;  the value will be set to zero as the default.")
        '        Else
        '            _BYHOUR = value
        '        End If
        '    End Set
        'End Property

        Private _BYDAY As List(Of RecurranceWeekday)
        Public Property BYDAY() As List(Of RecurranceWeekday)
            Get
                Return _BYDAY
            End Get
            Set(ByVal value As List(Of RecurranceWeekday))
                _BYDAY = value
            End Set
        End Property

        'Private _MONTHWEEK As Integer
        '''' <summary>
        '''' Not supported in 8.2 An integer  value between -5 and 5; 0 indicates that the value will not be used
        '''' </summary>
        '''' <returns></returns>
        'Public Property MONTHWEEK() As Integer
        '    Get
        '        Return _MONTHWEEK
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < -5 Or value > 5 Then
        '            _MONTHWEEK = 0
        '            Throw New System.ArgumentOutOfRangeException("MONTHWEEK", value, "Value must be between -5 and 5;  the value will be set to zero as the default.")
        '        Else
        '            _MONTHWEEK = value
        '        End If
        '    End Set
        'End Property

        'Private _YEARWEEK As Integer
        '''' <summary>
        '''' Not supported in 8.2 An integer  value between -53 and 53; 0 indicates that the value will not be used
        '''' </summary>
        '''' <returns></returns>
        'Public Property YEARWEEK() As Integer
        '    Get
        '        Return _YEARWEEK
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < -53 Or value > 53 Then
        '            _YEARWEEK = 0
        '            Throw New System.ArgumentOutOfRangeException("YEARWEEK", value, "Value must be between -53 and 53;  the value will be set to zero as the default.")
        '        Else
        '            _YEARWEEK = value
        '        End If
        '    End Set
        'End Property

        'Private _MONTHDAY As Integer
        '''' <summary>
        '''' Not supported in 8.2 An integer  value between -31 and 31; 0 indicates that the value will not be used
        '''' </summary>
        '''' <returns></returns>
        'Public Property MONTHDAY() As Integer
        '    Get
        '        Return _MONTHDAY
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < -31 Or value > 31 Then
        '            _MONTHDAY = 0
        '            Throw New System.ArgumentOutOfRangeException("MONTHDAY", value, "Value must be between -31 and 31;  the value will be set to zero as the default.")
        '        Else
        '            _MONTHDAY = value
        '        End If
        '    End Set
        'End Property

        'Private _YEARDAY As Integer
        '''' <summary>
        '''' Not supported in 8.2 An integer  value between -366 and 366; 0 indicates that the value will not be used
        '''' </summary>
        '''' <returns></returns>
        'Public Property YEARDAY() As Integer
        '    Get
        '        Return _YEARDAY
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < -366 Or value > 366 Then
        '            _YEARDAY = 0
        '            Throw New System.ArgumentOutOfRangeException("YEARDAY", value, "Value must be between -366 and 366;  the value will be set to zero as the default.")
        '        Else
        '            _YEARDAY = value
        '        End If
        '    End Set
        'End Property

        'Private _YEARMONTH As Integer
        '''' <summary>
        '''' Not supported in 8.2 An integer  value between -12 and 12; 0 indicates that the value will not be used
        '''' </summary>
        '''' <returns></returns>
        'Public Property YEARMONTH() As Integer
        '    Get
        '        Return _YEARMONTH
        '    End Get
        '    Set(ByVal value As Integer)
        '        If value < -12 Or value > 12 Then
        '            _YEARMONTH = 0
        '            Throw New System.ArgumentOutOfRangeException("YEARMONTH", value, "Value must be between -12 and 12;  the value will be set to zero as the default.")
        '        Else
        '            _YEARMONTH = value
        '        End If
        '    End Set
        'End Property

        Public Sub resetBYDAYs(ByVal Optional blnSU As Boolean = False,
                               ByVal Optional blnMO As Boolean = False,
                               ByVal Optional blnTU As Boolean = False,
                               ByVal Optional blnWE As Boolean = False,
                               ByVal Optional blnTH As Boolean = False,
                               ByVal Optional blnFR As Boolean = False,
                               ByVal Optional blnSA As Boolean = False)
            BYDAY = New List(Of RecurranceWeekday)
            If blnSU Then BYDAY.Add(RecurranceWeekday.SU)
            If blnMO Then BYDAY.Add(RecurranceWeekday.MO)
            If blnTU Then BYDAY.Add(RecurranceWeekday.TU)
            If blnWE Then BYDAY.Add(RecurranceWeekday.WE)
            If blnTH Then BYDAY.Add(RecurranceWeekday.TH)
            If blnFR Then BYDAY.Add(RecurranceWeekday.FR)
            If blnSA Then BYDAY.Add(RecurranceWeekday.SA)
        End Sub

        ''' <summary>
        ''' parse a bitwise string to identify which days are on 0 = off 1 = on first bit is sunday
        ''' </summary>
        ''' <param name="sBits"></param>
        Public Sub resetBYDAYs(ByVal Optional sBits As String = "0000000")
            BYDAY = New List(Of RecurranceWeekday)
            If sBits.Length < 1 Then Return
            Dim charArray() As Char = sBits.ToCharArray
            For i As Integer = 0 To charArray.Count - 1
                If i > 6 Then Exit For
                If charArray(i) = "1" Then BYDAY.Add(i)
            Next
        End Sub

        ''' <summary>
        ''' Converts the BYDAY list to a bitwise string value like "0100001" where 0 is false and 1 is true starting with Sunday, example shows Monday and Saturday checked
        ''' </summary>
        ''' <returns></returns>
        Public Function getBYDayBitString() As String
            Dim sRet As String = String.Format("{0}{1}{2}{3}{4}{5}{6}",
                                               If(BYDAY.Contains(RecurranceWeekday.SU), "1", "0"),
                                               If(BYDAY.Contains(RecurranceWeekday.MO), "1", "0"),
                                               If(BYDAY.Contains(RecurranceWeekday.TU), "1", "0"),
                                               If(BYDAY.Contains(RecurranceWeekday.WE), "1", "0"),
                                               If(BYDAY.Contains(RecurranceWeekday.TH), "1", "0"),
                                               If(BYDAY.Contains(RecurranceWeekday.FR), "1", "0"),
                                               If(BYDAY.Contains(RecurranceWeekday.SA), "1", "0"))
            Return sRet
        End Function

        ''' <summary>
        ''' True if the recurrence is scheduled for dtVal using Until or forever must be called by RecurrenceEvent to determine start date, times and counters
        ''' </summary>
        ''' <param name="dtVal"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' in v-8.2 we only check for weekly recurrence days of the week based on Until 
        ''' </remarks>
        Public Function isRecurrenceOnDate(ByVal dtVal As Date) As Boolean
            Dim blnRet As Boolean = False
            Dim dtTo As Date = dtVal.ToShortDateString()
            If UNTIL.HasValue AndAlso (dtTo > UNTIL.Value) Then Return False
            If FREQ = RecurranceRuleFREQ.WEEKLY Then
                If BYDAY Is Nothing OrElse BYDAY.Count < 1 Then
                    blnRet = True 'every day
                Else
                    If BYDAY.Contains(dtVal.DayOfWeek) Then blnRet = True
                    'add other logic here for future evaluation 
                End If
            End If

            Return blnRet
        End Function

        Public Overrides Function toString() As String
            Dim sRule As New System.Text.StringBuilder()
            sRule.AppendFormat("FREQ={0}", FREQ.ToString())
            If UNTIL.HasValue Then
                sRule.AppendFormat(";UNTIL={0}", UNTIL.Value.ToString("yyyy-MM-dd"))
            ElseIf COUNT > 0 Then
                sRule.AppendFormat(";COUNT={0}", COUNT)
            End If
            sRule.Append(";WKST=SU")
            If Not BYDAY Is Nothing AndAlso BYDAY.Count > 0 Then
                sRule.Append(";BYDAY=")
                Dim sDaySpacer = ""
                For Each d In BYDAY
                    sRule.AppendFormat("{0}{1}", sDaySpacer, d.ToString())
                    sDaySpacer = ","
                Next
            End If
            Return sRule.ToString()
        End Function

    End Class


    '    recur-rule-part = ( "FREQ" "=" freq )
    '                       / ( "UNTIL" "=" enddate )
    '                       / ( "COUNT" "=" 1*DIGIT )
    '                       / ( "INTERVAL" "=" 1*DIGIT )
    '                       / ( "BYSECOND" "=" byseclist )
    '                       / ( "BYMINUTE" "=" byminlist )
    '                       / ( "BYHOUR" "=" byhrlist )
    '                       / ( "BYDAY" "=" bywdaylist )
    '                       / ( "BYMONTHDAY" "=" bymodaylist )
    '                       / ( "BYYEARDAY" "=" byyrdaylist )
    '                       / ( "BYWEEKNO" "=" bywknolist )
    '                       / ( "BYMONTH" "=" bymolist )
    '                       / ( "BYSETPOS" "=" bysplist )
    '                       / ( "WKST" "=" weekday )



    '       byseclist   = ( seconds *("," seconds) )

    '       seconds     = 1*2DIGIT       ;0 to 60

    '       byminlist   = ( minutes *("," minutes) )

    '       minutes     = 1*2DIGIT       ;0 to 59

    '       byhrlist    = ( hour *("," hour) )

    '       hour        = 1*2DIGIT       ;0 to 23

    '       bywdaylist  = ( weekdaynum *("," weekdaynum) )


    '       weekdaynum  = [[plus / minus] ordwk] weekday
    'TODO: we need to create a dictionary or hash the identifies teh type of BDAY data so we can apply the integer validation rules below
    'Setting "BYDAY" the value is a COMMA-separated list of days of the week associated With one or more of the weekday enums
    'When the FREQ "=" WEEKLY the ordwk value is ignored
    'When the FREQ "=" "MONTHLY" the value is one or more weekday enums and must identify a positive or negative ord week of month value (-5 to 5)
    '       plus        = "+" positive number from first week to last week; for example: 1MO indicates first Monday of the Month
    '       minus       = "-" negagive number from the last week to the first week; -1MO indicates the last Monday of the Month
    'When the FREQ "=" "YEARLY" the value is one or more weekday enums and must identify a positive or negative ord week of the year value (-53 to 53
    '       plus        = "+" positive number from first week of the year to the last week of the year; 1MO indicates first Monday of the Year
    '       minus       = "-" negagive number from the last week or the year to the last week of the year; -1MO indicates the last Monday of the Year

    '       weekday  enum   = "SU" / "MO" / "TU" / "WE" / "TH" / "FR" / "SA"
    '       ;Corresponding to SUNDAY, MONDAY, TUESDAY, WEDNESDAY, THURSDAY,
    '       ;FRIDAY, And SATURDAY days of the week.
    'NOTE: the BYDAY rule for YEARWEEK numeric value is ignored when the FREQ "=" "YEARLY" and the BYWEEKNO is provided as a YEARWEEK?
    '       See The BYWEEKNO rule part specifies a COMMA-separated list of
    'ordinals specifying weeks Of the year.  Valid values are 1 To 53
    'Or -53 to -1.  This corresponds to weeks according to week
    'numbering as defined in [ISO.8601.2004].  A week Is defined as a
    'seven day period, starting On the day Of the week defined To be
    'the week start (see WKST).  Week number one Of the calendar year
    'Is the first week that contains at least four (4) days in that
    'calendar year.  This rule part MUST Not be used When the FREQ rule
    'part Is set to anything other than YEARLY.  For example, 3
    'represents the third week Of the year.


    ' bymodaylist = ( monthdaynum *("," monthdaynum) )


    'monthdaynum = [plus / minus] ordmoday

    'ordmoday    = 1*2DIGIT       ;1 to 31

    'byyrdaylist = ( yeardaynum *("," yeardaynum) )

    'yeardaynum  = [plus / minus] ordyrday

    'ordyrday    = 1*3DIGIT      ;1 to 366

    'bywknolist  = ( weeknum *("," weeknum) )

    'weeknum     = [plus / minus] ordwk

    'bymolist    = ( monthnum *("," monthnum) )

    'monthnum    = 1*2DIGIT       ;1 to 12

    'bysplist    = ( setposday *("," setposday) )

    'setposday   = yeardaynum


End Namespace
