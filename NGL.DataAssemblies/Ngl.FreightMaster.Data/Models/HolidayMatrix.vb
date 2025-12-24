Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS

Namespace Models

    Public Class HolidayMatrix

        Private _iHolidayYear As Integer = 0
        Public Property iHolidayYear() As Integer
            Get
                Return _iHolidayYear
            End Get
            Set(ByVal value As Integer)
                _iHolidayYear = value
            End Set
        End Property

        Private _USHolidayDates As List(Of Date)
        Public Property USHolidayDates() As List(Of Date)
            Get
                If _USHolidayDates Is Nothing Then
                    _USHolidayDates = New List(Of Date)
                End If
                Return _USHolidayDates
            End Get
            Set(ByVal value As List(Of Date))
                _USHolidayDates = value
            End Set
        End Property

        'CarrTarNDDNoDrivingDate determines which days are holidays for a tariff
        Private _CarrierHolidayDates As List(Of Date)
        Public Property CarrierHolidayDates() As List(Of Date)
            Get
                If _CarrierHolidayDates Is Nothing Then
                    _CarrierHolidayDates = New List(Of Date)
                End If
                Return _CarrierHolidayDates
            End Get
            Set(ByVal value As List(Of Date))
                _CarrierHolidayDates = value
            End Set
        End Property

        Private _iCarrTarControl As Integer = 0
        Public Property iCarrTarControl() As Integer
            Get
                Return _iCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _iCarrTarControl = value
            End Set
        End Property

        Private _iCarrierControl As Integer = 0
        Public Property iCarrierControl() As Integer
            Get
                Return _iCarrierControl
            End Get
            Set(ByVal value As Integer)
                _iCarrierControl = value
            End Set
        End Property

        Private _CompHolidayDates As List(Of Date)
        Public Property CompHolidayDates() As List(Of Date)
            Get
                If _CompHolidayDates Is Nothing Then
                    _CompHolidayDates = New List(Of Date)
                End If
                Return _CompHolidayDates
            End Get
            Set(ByVal value As List(Of Date))
                _CompHolidayDates = value
            End Set
        End Property

        Private _iCompControl As Integer = 0
        Public Property iCompControl() As Integer
            Get
                Return _iCompControl
            End Get
            Set(ByVal value As Integer)
                _iCompControl = value
            End Set
        End Property

        Private _DriveSaturday As Boolean = False
        Public Property DriveSaturday() As Boolean
            Get
                Return _DriveSaturday
            End Get
            Set(ByVal value As Boolean)
                _DriveSaturday = value
            End Set
        End Property

        Private _DriveSunday As Boolean = False
        Public Property DriveSunday() As Boolean
            Get
                Return _DriveSunday
            End Get
            Set(ByVal value As Boolean)
                _DriveSunday = value
            End Set
        End Property

        Private _CompClosedDates As List(Of Date)
        Public Property CompClosedDates() As List(Of Date)
            Get
                If _CompClosedDates Is Nothing Then
                    _CompClosedDates = New List(Of Date)
                End If
                Return _CompClosedDates
            End Get
            Set(ByVal value As List(Of Date))
                _CompClosedDates = value
            End Set
        End Property



        Public Function isHoliday(ByVal day As Date?, Optional blnCheckCarrier As Boolean = True, Optional blnCheckComp As Boolean = False) As Boolean
            Dim blnRet As Boolean = False
            If (day Is Nothing) Then Return False
            If blnCheckCarrier Then
                blnRet = CarrierHolidayDates.Contains(CDate(day.Value.ToShortDateString()))
            End If
            If Not blnRet AndAlso blnCheckComp Then
                blnRet = CompHolidayDates.Contains(CDate(day.Value.ToShortDateString()))
            End If
            Return blnRet
        End Function

        Public Function isCompClosed(ByVal day As Date?) As Boolean
            Dim blnRet As Boolean = False

            blnRet = CompClosedDates.Contains(CDate(day.Value.ToShortDateString()))

            Return blnRet
        End Function

        Public Sub LoadCompDatesList(ByVal intCompControl As Integer, ByVal lCompCal As List(Of DTO.CompCal), Optional ByVal iYear As Integer = 0, Optional ByVal blnUseUSHolidaysAsDefault As Boolean = True)
            CompHolidayDates = New List(Of Date)
            CompClosedDates = New List(Of Date)
            iCompControl = intCompControl
            If iYear = 0 Then iYear = Date.Now.Year

            If Not lCompCal Is Nothing Then
                For Each oCal In lCompCal
                    If iCompControl = 0 Then iCompControl = oCal.CompCalCompControl
                    If oCal.CompCalIsHoliday Then
                        CompHolidayDates.Add(New Date(iYear, oCal.CompCalMonth, oCal.CompCalDay))
                    End If
                    If oCal.CompCalOpen = False Then
                        CompClosedDates.Add(New Date(iYear, oCal.CompCalMonth, oCal.CompCalDay))
                    End If

                Next
            Else
                If blnUseUSHolidaysAsDefault Then CompHolidayDates = getUSHolidayList(iYear)
            End If

        End Sub

        Public Sub CompClosedOnSaturday(ByVal iMonth As Integer, ByVal iYear As Integer)
            'add the saturdays from the previous month
            If iMonth = 1 Then
                AddSaturdayToCompClosedByMonth(12, iYear) ' last december
            Else
                AddSaturdayToCompClosedByMonth((iMonth - 1), iYear) ' prevous Month
            End If
            'add the saturdays from the delivery month
            AddSaturdayToCompClosedByMonth(iMonth, iYear)
            'add the saturdays from the next month
            If iMonth = 12 Then
                AddSaturdayToCompClosedByMonth(1, iYear + 1) ' next January
            Else
                AddSaturdayToCompClosedByMonth((iMonth + 1), iYear) ' next month
            End If
        End Sub

        Public Sub AddSaturdayToCompClosedByMonth(ByVal iMonth As Integer, ByVal iYear As Integer)
            For iDay As Integer = 1 To Date.DaysInMonth(iYear, iMonth)
                Try
                    Dim dt = New Date(iYear, iMonth, iDay)
                    If dt.DayOfWeek = DayOfWeek.Saturday Then
                        CompClosedDates.Add(dt)
                    End If
                Catch ex As Exception
                    ' do nothing
                End Try
            Next

        End Sub

        Public Sub CompClosedOnSunday(ByVal iMonth As Integer, ByVal iYear As Integer)
            'add the Sundays from the previous month
            If iMonth = 1 Then
                AddSundayToCompClosedByMonth(12, iYear) ' last december
            Else
                AddSundayToCompClosedByMonth((iMonth - 1), iYear) ' prevous Month
            End If
            'add the Sundays from the current month
            AddSundayToCompClosedByMonth(iMonth, iYear)
            'add the Sundays from the next month
            If iMonth = 12 Then
                AddSundayToCompClosedByMonth(1, iYear + 1) ' next January
            Else
                AddSundayToCompClosedByMonth((iMonth + 1), iYear) ' next month
            End If
        End Sub

        Public Sub AddSundayToCompClosedByMonth(ByVal iMonth As Integer, ByVal iYear As Integer)
            For iDay As Integer = 1 To Date.DaysInMonth(iYear, iMonth)
                Try
                    Dim dt = New Date(iYear, iMonth, iDay)
                    If dt.DayOfWeek = DayOfWeek.Sunday Then
                        CompClosedDates.Add(dt)
                    End If
                Catch ex As Exception
                    ' do nothing
                End Try

            Next

        End Sub

        Public Sub LoadCarrierHolidayList(ByVal intCarrierControl As Integer, ByVal lCarrierDates As List(Of Date), Optional ByVal iYear As Integer = 0, Optional ByVal blnUseUSHolidaysAsDefault As Boolean = True)
            CarrierHolidayDates = New List(Of Date)
            For Each iDate As Date In lCarrierDates
                CarrierHolidayDates.Add(CDate(iDate.ToShortDateString())) ' remove any time components
            Next

            iCarrierControl = intCarrierControl
            If iYear = 0 Then iYear = Date.Now.Year

            If CarrierHolidayDates Is Nothing AndAlso blnUseUSHolidaysAsDefault Then
                CarrierHolidayDates = getUSHolidayList(iYear)
            End If
        End Sub


        Public Sub LoadCarrierNoDriveDays(ByVal intCarrierControl As Integer, ByVal lNoDriveDays As List(Of DTO.CarrTarNoDriveDays), Optional ByVal iYear As Integer = 0, Optional ByVal blnUseUSHolidaysAsDefault As Boolean = True)
            CarrierHolidayDates = New List(Of Date)
            For Each iDate As DTO.CarrTarNoDriveDays In lNoDriveDays
                CarrierHolidayDates.Add(CDate(iDate.CarrTarNDDNoDrivingDate.ToShortDateString()))
            Next

            iCarrierControl = intCarrierControl
            If iYear = 0 Then iYear = Date.Now.Year

            If CarrierHolidayDates Is Nothing AndAlso blnUseUSHolidaysAsDefault Then
                CarrierHolidayDates = getUSHolidayList(iYear)
            End If

        End Sub



        Public Sub LoadCarrierNoDriveDays(ByVal intCarrierControl As Integer, ByVal lNoDriveDays As LTS.CarrierNoDriveDay(), Optional ByVal iYear As Integer = 0, Optional ByVal blnUseUSHolidaysAsDefault As Boolean = True)
            CarrierHolidayDates = New List(Of Date)
            For Each iDate As LTS.CarrierNoDriveDay In lNoDriveDays
                CarrierHolidayDates.Add(CDate(iDate.CarrierNDDNoDrivingDate.ToShortDateString()))
            Next

            iCarrierControl = intCarrierControl
            If iYear = 0 Then iYear = Date.Now.Year

            If CarrierHolidayDates Is Nothing AndAlso blnUseUSHolidaysAsDefault Then
                CarrierHolidayDates = getUSHolidayList(iYear)
            End If

        End Sub

        Public Function getUSHolidayList(ByVal iYear As Integer) As List(Of Date)

            If (USHolidayDates Is Nothing OrElse USHolidayDates.Count() < 1) AndAlso iHolidayYear <> iYear Then
                iHolidayYear = iYear
                USHolidayDates = New List(Of Date)

                Dim FirstWeek As Integer = 1
                Dim SecondWeek As Integer = 2
                Dim ThirdWeek As Integer = 3
                Dim FourthWeek As Integer = 4
                Dim LastWeek As Integer = 5
                '   http://www.usa.gov/citizens/holidays.shtml      
                '   http://archive.opm.gov/operating_status_schedules/fedhol/2013.asp

                ' New Year's Day            Jan 1
                USHolidayDates.Add(DateSerial(iYear, 1, 1))

                ' Martin Luther King, Jr. third Mon in Jan
                USHolidayDates.Add(GetNthDayOfNthWeek(DateSerial(iYear, 1, 1), DayOfWeek.Monday, ThirdWeek))

                ' Washington's Birthday third Mon in Feb
                USHolidayDates.Add(GetNthDayOfNthWeek(DateSerial(iYear, 2, 1), DayOfWeek.Monday, ThirdWeek))

                ' Memorial Day          last Mon in May
                USHolidayDates.Add(GetNthDayOfNthWeek(DateSerial(iYear, 5, 1), DayOfWeek.Monday, LastWeek))

                ' Independence Day      July 4
                USHolidayDates.Add(DateSerial(iYear, 7, 4))

                ' Labor Day             first Mon in Sept
                USHolidayDates.Add(GetNthDayOfNthWeek(DateSerial(iYear, 9, 1), DayOfWeek.Monday, FirstWeek))

                ' Columbus Day          second Mon in Oct
                USHolidayDates.Add(GetNthDayOfNthWeek(DateSerial(iYear, 10, 1), DayOfWeek.Monday, SecondWeek))

                ' Veterans Day          Nov 11
                USHolidayDates.Add(DateSerial(iYear, 11, 11))

                ' Thanksgiving Day      fourth Thur in Nov
                USHolidayDates.Add(GetNthDayOfNthWeek(DateSerial(iYear, 11, 1), DayOfWeek.Thursday, FourthWeek))

                ' Christmas Day         Dec 25
                USHolidayDates.Add(DateSerial(iYear, 12, 25))

                'saturday holidays are moved to Fri; Sun to Mon
                For i As Integer = 0 To USHolidayDates.Count - 1
                    Dim dt As Date = USHolidayDates(i)
                    If dt.DayOfWeek = DayOfWeek.Saturday Then
                        USHolidayDates(i) = dt.AddDays(-1)
                    End If
                    If dt.DayOfWeek = DayOfWeek.Sunday Then
                        USHolidayDates(i) = dt.AddDays(1)
                    End If
                Next

            End If

            'return
            Return USHolidayDates

        End Function

        Private Function GetNthDayOfNthWeek(ByVal dt As Date, ByVal DayofWeek As Integer, ByVal WhichWeek As Integer) As Date
            'specify which day of which week of a month and this function will get the date
            'this function uses the month and year of the date provided

            'get first day of the given date
            Dim dtFirst As Date = DateSerial(dt.Year, dt.Month, 1)

            'get first DayOfWeek of the month
            Dim dtRet As Date = dtFirst.AddDays(6 - dtFirst.AddDays(-(DayofWeek + 1)).DayOfWeek)

            'get which week
            dtRet = dtRet.AddDays((WhichWeek - 1) * 7)

            'if day is past end of month then adjust backwards a week
            If dtRet >= dtFirst.AddMonths(1) Then
                dtRet = dtRet.AddDays(-7)
            End If

            'return
            Return dtRet

        End Function

    End Class


End Namespace

