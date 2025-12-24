Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Linq.Dynamic
Imports LTSCEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum
'Public Class NGLtblPickListItemData : Inherits NGLLinkDataBaseClass

'#Region " Constructors "

'    Public Sub New(ByVal oParameters As WCFParameters)
'        MyBase.New()
'        processParameters(oParameters)
'        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
'        Me.LinqTable = db.tblPickListItems
'        Me.LinqDB = db
'        Me.SourceClass = "NGLtblPickListItemData"
'    End Sub

'#End Region

'#Region " Properties "

'    Protected Overrides Property LinqTable() As Object
'        Get
'            Dim db As New NGLMASIntegrationDataContext(ConnectionString)
'            _LinqTable = db.tblPickListItems
'            Me.LinqDB = db
'            Return _LinqTable
'        End Get
'        Set(ByVal value As Object)
'            _LinqTable = value
'        End Set
'    End Property

'#End Region

'#Region "Public Methods"

'    ''' <summary>
'    ''' Get a filtered array of Task Data
'    ''' </summary>
'    ''' <param name="filters"></param>
'    ''' <param name="RecordCount"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function GettblPickListItems(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblPickListItem()
'        If filters Is Nothing Then Return Nothing
'        Dim oRet() As LTS.tblPickListItem
'        Using db As New NGLMASIntegrationDataContext(ConnectionString)
'            Try
'                Dim iQuery As IQueryable(Of LTS.tblPickListItem)
'                iQuery = db.tblPickListItems
'                Dim filterWhere = ""
'                ApplyAllFilters(iQuery, filters, filterWhere)
'                PrepareQuery(iQuery, filters, RecordCount)
'                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
'                Return oRet
'            Catch ex As Exception
'                ManageLinqDataExceptions(ex, buildProcedureName("GettblPickListItems"), db)
'            End Try
'        End Using
'        Return Nothing
'    End Function

'    ''' <summary>
'    ''' Insert or Update Task Data
'    ''' </summary>
'    ''' <param name="oData"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function SavetblPickListItem(ByVal oData As LTS.tblPickListItem) As Boolean
'        Dim blnRet As Boolean = False
'        If oData Is Nothing Then Return False 'nothing to do

'        Using db As New NGLMASIntegrationDataContext(ConnectionString)
'            Try
'                Dim lPickListItemControl As Long = oData.PickListItemControl
'                'If String.IsNullOrWhiteSpace(oData.RunTaskName) Then
'                '    Dim lDetails As New List(Of String) From {"Task Name", " was not provided and "}
'                '    throwInvalidKeyParentRequiredException(lDetails)
'                '    Return False
'                'End If
'                oData.PickListItemModDate = Date.Now
'                oData.PickListItemModUser = Me.Parameters.UserName

'                If (lPickListItemControl = 0) Then
'                    'this is an insert test if the name already exists
'                    'If db.tblPickListItems.Any(Function(x) String.Compare(x.RunTaskName, oData.RunTaskName, True)) Then
'                    '    Dim lDetails As New List(Of String) From {"Task Name", " " & oData.RunTaskName & " already exists, provide a unique value and "}
'                    '    throwInvalidKeyParentRequiredException(lDetails)
'                    '    Return False
'                    'End If
'                    db.tblPickListItems.InsertOnSubmit(oData)
'                Else
'                    'This is an update so get the current record,  if it does not exist throw error
'                    Dim oTask As LTS.tblPickListItem = db.tblPickListItems.Where(Function(x) x.RunTaskControl = lPickListItemControl).FirstOrDefault()
'                    If oTask Is Nothing OrElse oTask.RunTaskControl = 0 Then
'                        Dim lDetails As New List(Of String) From {"Task Control", " was not found or has been deleted and "}
'                        throwInvalidKeyParentRequiredException(lDetails)
'                        Return False
'                    End If
'                    If db.tblPickListItems.Any(Function(x) String.Compare(x.RunTaskName, oData.RunTaskName, True) And x.RunTaskControl <> oData.RunTaskControl) Then
'                        Dim lDetails As New List(Of String) From {"Task Name", " " & oData.RunTaskName & " a duplicate exists, provide a unique value and "}
'                        throwInvalidKeyParentRequiredException(lDetails)
'                        Return False
'                    End If
'                    db.tblPickListItems.Attach(oData, True)
'                End If
'                db.SubmitChanges()
'                blnRet = True
'            Catch ex As Exception
'                ManageLinqDataExceptions(ex, buildProcedureName("SavetblPickListItem"), db)
'            End Try
'        End Using
'        Return blnRet
'    End Function

'    ''' <summary>
'    ''' Delete a specific Task Record
'    ''' </summary>
'    ''' <param name="iRunTaskControl"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function DeletetblPickListItem(ByVal iRunTaskControl As Integer) As Boolean
'        Dim blnRet As Boolean = False
'        If iRunTaskControl = 0 Then Return False 'nothing to do
'        Using db As New NGLMASIntegrationDataContext(ConnectionString)
'            Try
'                'verify the record
'                Dim oExisting = db.tblPickListItems.Where(Function(x) x.RunTaskControl = iRunTaskControl).FirstOrDefault()
'                If oExisting Is Nothing OrElse oExisting.RunTaskControl = 0 Then Return True
'                db.tblPickListItems.DeleteOnSubmit(oExisting)
'                db.SubmitChanges()
'                blnRet = True
'            Catch ex As FaultException
'                Throw
'            Catch ex As Exception
'                ManageLinqDataExceptions(ex, buildProcedureName("DeletetblPickListItem"), db)
'            End Try
'        End Using
'        Return blnRet
'    End Function

'    ''' <summary>
'    ''' Format the Task Run Date Message using selected settngs
'    ''' </summary>
'    ''' <param name="oData"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function getTaskDateMessage(ByRef oData As LTS.vtblPickListItem) As String

'        Dim intMin As Integer = oData.RunTaskMinutes
'        Dim intHour As Integer = oData.RunTaskHours
'        Dim intDay As Integer = oData.RunTaskDays
'        Dim intMonth As Integer = oData.RunTaskMonths
'        Dim intWeekDay As Integer = oData.RunTaskWeekDays

'        Dim d1 As DateTime = DateTime.Now
'        Dim intToRunMinute As Integer
'        Dim intLastRunMinute As Integer
'        Dim intThisMinute As Integer = d1.Minute
'        Dim intToRunHour As Integer
'        Dim intLastRunHour As Integer
'        Dim intThisHour As Integer = d1.Hour
'        Dim intToRunDay As Integer
'        Dim intLastRunDay As Integer
'        Dim intThisDay As Integer = d1.Day
'        Dim intToRunMonth As Integer
'        Dim intLastRunMonth As Integer
'        Dim intThisMonth As Integer = d1.Month
'        Dim intThisWeekDay As Integer = d1.DayOfWeek
'        Dim intToRunYear As Integer
'        Dim intLastRunYear As Integer
'        Dim intThisYear As Integer = d1.Year
'        Dim intThisDayOfYear As Integer = d1.DayOfYear
'        Dim blnDateFound As Boolean = False
'        Dim arrDates As New ArrayList

'        'set all defaults to NOW
'        intToRunYear = intThisYear
'        intToRunMonth = intThisMonth
'        intToRunDay = intThisDay
'        intToRunHour = intThisHour
'        intToRunMinute = intThisMinute

'        intLastRunYear = intThisYear
'        intLastRunMonth = intThisMonth
'        intLastRunDay = intThisDay
'        intLastRunHour = intThisHour
'        intLastRunMinute = intThisMinute

'        Select Case intMin
'            Case 62 'Every 15 Minutes
'                blnDateFound = True
'                Return "Every 15 Minutes"
'            Case 61 'Every 10 Minutes
'                blnDateFound = True
'                Return "Every 10 Minutes"
'            Case 60 'Every 5 Minutes
'                blnDateFound = True
'                Return "Every 5 Minutes"
'            Case Else
'                intToRunMinute = intMin
'        End Select
'        'Now we test the hour
'        If Not blnDateFound Then
'            Select Case intHour
'                Case 28 'Every 12 hours
'                    blnDateFound = True
'                    Return "Every 12 Hours at " & intMin & " minutes past the hour."
'                Case 27 'Every 6 hours
'                    blnDateFound = True
'                    Return "Every 6 Hours at " & intMin & " minutes past the hour."
'                Case 26 'Every 4 hours
'                    blnDateFound = True
'                    Return "Every 4 Hours at " & intMin & " minutes past the hour."
'                Case 25 'Every 2 hours
'                    blnDateFound = True
'                    Return "Every 2 Hours at " & intMin & " minutes past the hour."
'                Case 24 'Every Hour
'                    blnDateFound = True
'                    Return "Every Hour at " & intMin & " minutes past the hour."
'                Case Else
'                    intToRunHour = intHour
'            End Select
'        End If
'        'Now we test the day
'        If Not blnDateFound Then
'            Select Case intDay
'                Case 0 'Every Day
'                    'Test for Weekday
'                    If intWeekDay < 7 Then
'                        Select Case intWeekDay
'                            Case 0
'                                blnDateFound = True
'                                Return "Every Sunday at " & formatTime(intHour, intMin) & "."
'                            Case 1
'                                blnDateFound = True
'                                Return "Every Monday at " & formatTime(intHour, intMin) & "."
'                            Case 2
'                                blnDateFound = True
'                                Return "Every Tuesday at " & formatTime(intHour, intMin) & "."
'                            Case 3
'                                blnDateFound = True
'                                Return "Every Wednesday at " & formatTime(intHour, intMin) & "."
'                            Case 4
'                                blnDateFound = True
'                                Return "Every Thursday at " & formatTime(intHour, intMin) & "."
'                            Case 5
'                                blnDateFound = True
'                                Return "Every Friday at " & formatTime(intHour, intMin) & "."
'                            Case Else
'                                blnDateFound = True
'                                Return "Every Saturday at " & formatTime(intHour, intMin) & "."
'                        End Select
'                    Else
'                        blnDateFound = True
'                        Return "Every Day at " & formatTime(intHour, intMin) & "."
'                    End If
'                Case Else
'                    intToRunDay = intDay
'            End Select
'        End If
'        'Now we test the month
'        If Not blnDateFound Then
'            If intMonth > 0 Then
'                If intThisMonth <= intMonth Then
'                    intToRunYear = intThisYear
'                Else
'                    intToRunYear = intThisYear + 1
'                End If
'                blnDateFound = True
'                Return intMonth & "-" & intDay & "-" & intToRunYear & " at " & formatTime(intHour, intMin) & "."
'            Else  'Run Every Month
'                blnDateFound = True
'                Return "Every Month on the " & formatDay(intDay) & " day at " & formatTime(intHour, intMin) & "."
'            End If
'        End If

'        Return " N/A "
'    End Function

'    ''' <summary>
'    ''' Format the Task Time string for seleted task settings
'    ''' </summary>
'    ''' <param name="intHour"></param>
'    ''' <param name="intMin"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function formatTime(ByVal intHour As Integer, ByVal intMin As Integer) As String
'        Dim strMin As String

'        If intMin < 10 Then
'            strMin = "0" & intMin
'        Else
'            strMin = intMin
'        End If
'        If intHour > 12 Then
'            intHour -= 12
'            Return intHour & ":" & strMin & " pm"
'        Else
'            Return intHour & ":" & strMin & " am"
'        End If
'    End Function

'    ''' <summary>
'    ''' Format the Task Day string using selected configuration
'    ''' </summary>
'    ''' <param name="intDay"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function formatDay(ByVal intDay As Integer) As String
'        Select Case intDay
'            Case 1, 21, 31
'                Return intDay & "st"
'            Case 2, 22
'                Return intDay & "nd"
'            Case 3, 23
'                Return intDay & "rd"
'            Case Else
'                Return intDay & "th"
'        End Select
'    End Function

'    ''' <summary>
'    ''' Get an array of filtered task log records
'    ''' </summary>
'    ''' <param name="filters"></param>
'    ''' <param name="RecordCount"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function getTaskLog(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblTaskLog()
'        If filters Is Nothing Then Return Nothing
'        Dim oRet() As LTS.tblTaskLog
'        Using db As New NGLMASIntegrationDataContext(ConnectionString)
'            Try
'                Dim iQuery As IQueryable(Of LTS.tblTaskLog)
'                iQuery = db.tblTaskLogs
'                Dim filterWhere = ""
'                If String.IsNullOrWhiteSpace(filters.sortName) Then
'                    filters.sortName = "TaskControl"
'                    filters.sortDirection = "desc"
'                End If
'                ApplyAllFilters(iQuery, filters, filterWhere)
'                PrepareQuery(iQuery, filters, RecordCount)
'                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
'                Return oRet
'            Catch ex As Exception
'                ManageLinqDataExceptions(ex, buildProcedureName("tblTaskLog"), db)
'            End Try
'        End Using
'        Return Nothing
'    End Function

'    ''' <summary>
'    ''' format task settings into readable message
'    ''' </summary>
'    ''' <param name="oData"></param>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Sub formatTaskMessages(ByRef oData As LTS.vtblPickListItem)
'        oData.RunTaskTaskDateMessage = getTaskDateMessage(oData)
'        'Case "TaskMinutes" 'Created by RHR for v-8.5.0.001 on 09/22/21 
'        Select Case oData.RunTaskMinutes
'            Case 60
'                oData.RunTaskMinutesMessage = "Every 5 minutes all day"
'            Case 61
'                oData.RunTaskMinutesMessage = "Every 10 minutes all day"
'            Case 62
'                oData.RunTaskMinutesMessage = "Every 15 minutes all day"
'            Case Else
'                oData.RunTaskMinutesMessage = oData.RunTaskMinutes.ToString() & " Minutes past the hour"
'        End Select

'        'Case "TaskHours" 'Created by RHR for v-8.5.0.001 on 09/22/21 
'        Select Case oData.RunTaskHours
'            Case 24
'                oData.RunTaskHoursMessage = "Every hour all day"
'            Case 25
'                oData.RunTaskHoursMessage = "Every 2 hours all day"
'            Case 26
'                oData.RunTaskHoursMessage = "Every 4 hours all day"
'            Case 27
'                oData.RunTaskHoursMessage = "Every 6 hours all day"
'            Case 28
'                oData.RunTaskHoursMessage = "Every 12 hours all day"
'            Case Else
'                If oData.RunTaskHours < 12 Then
'                    oData.RunTaskHoursMessage = oData.RunTaskHours.ToString() & " am"
'                Else
'                    oData.RunTaskHoursMessage = (oData.RunTaskHours - 12).ToString() & " pm"
'                End If
'        End Select
'        'Case "TaskDays" 'Created by RHR for v-8.5.0.001 on 09/22/21 
'        If (oData.RunTaskDays = 0) Then
'            oData.RunTaskDaysMessage = "Every day"
'        Else
'            oData.RunTaskDaysMessage = "On day " & oData.RunTaskDays.ToString() & " of the month"
'        End If
'        'Case "TaskWeekDays" 'Created by RHR for v-8.5.0.001 on 09/22/21 
'        Select Case oData.RunTaskWeekDays
'            Case 0
'                oData.RunTaskWeekDaysMessage = "Every Sunday"
'            Case 1
'                oData.RunTaskWeekDaysMessage = "Every Monday"
'            Case 2
'                oData.RunTaskWeekDaysMessage = "Every Tuesday"
'            Case 3
'                oData.RunTaskWeekDaysMessage = "Every Wednesday"
'            Case 4
'                oData.RunTaskWeekDaysMessage = "Every Thursday"
'            Case 5
'                oData.RunTaskWeekDaysMessage = "Every Friday"
'            Case 6
'                oData.RunTaskWeekDaysMessage = "Every Saturday"
'            Case Else
'                oData.RunTaskWeekDaysMessage = "Use Days Only"
'        End Select

'        'Case "TaskMonths" 'Created by RHR for v-8.5.0.001 on 09/22/21 
'        If (oData.RunTaskMonths = 0) Then
'            oData.RunTaskMonthsMessage = "Every Month"
'        Else
'            oData.RunTaskMonthsMessage = oData.RunTaskMonths.ToString()
'        End If

'    End Sub

'    ''' <summary>
'    ''' Copy table data to view
'    ''' </summary>
'    ''' <param name="oData"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function selectLTSViewFromData(ByVal oData As LTS.tblPickListItem) As LTS.vtblPickListItem
'        Dim ltsView As New LTS.vtblPickListItem()
'        If oData Is Nothing Then Return ltsView
'        Dim skipObjs As New List(Of String) From {""}
'        Dim sMsg As String = ""
'        ltsView = DTran.CopyMatchingFields(ltsView, oData, skipObjs, sMsg)
'        formatTaskMessages(ltsView)
'        Return ltsView
'    End Function

'    ''' <summary>
'    ''' Copy View data to  Table
'    ''' </summary>
'    ''' <param name="oView"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' Created by RHR for v-8.5 on 09/25/2021 for New Task Manager Logic
'    ''' </remarks>
'    Public Function selectLTSDataFromView(ByVal oView As LTS.vtblPickListItem) As LTS.tblPickListItem
'        Dim ltsTable As New LTS.tblPickListItem()
'        If oView Is Nothing Then Return ltsTable
'        Dim skipObjs As New List(Of String) From {"RunTaskTaskDateMessage", "RunTaskMinutesMessage", "RunTaskHoursMessage", "RunTaskDaysMessage", "RunTaskMonthsMessage", "RunTaskWeekDaysMessage"}
'        Dim sMsg As String = ""
'        ltsTable = DTran.CopyMatchingFields(ltsTable, oView, skipObjs, sMsg)
'        Return ltsTable
'    End Function


'#End Region

'#Region "Protected Functions"


'#End Region

'End Class

'Class Added By LVV 2/29/16 for v-7.0.5.1 EDI Migration
Public Class NGLEDIData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.Parameters = oParameters
        Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        Me.LinqTable = db.tbl210EDIs
        Me.LinqDB = db
    End Sub

#End Region

#Region "Public Methods"
    'tbl210EDI
    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetEDI210OutFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetEDI210OutsFiltered()
    End Function

    Public Function GetEDI210OutFiltered(ByVal Control As Long) As DTO.tbl210EDI
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim EDI210 As DTO.tbl210EDI = (
                From d In db.tbl210EDIs
                Where
                    d._210EDIControl = Control
                Select selectDTOData(d, db)).FirstOrDefault()


                Return EDI210

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI210OutFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetEDI210OutsFiltered(Optional ByVal archived As Boolean = False, Optional ByVal EDI997Received As Boolean = False) As DTO.tbl210EDI()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim EDI210s() As DTO.tbl210EDI = (
                From d In db.tbl210EDIs
                Where
                    (d.Archived = archived) _
                    And
                    (d._997Received = EDI997Received)
                Order By d._210EDIControl
                Select selectDTOData(d, db)).ToArray()
                Return EDI210s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI210OutsFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Delegate Sub ProcessEDI210DataDelegate()

    Public Sub InsertEDI210OutboundDataAsync()

        Dim fetcher As New ProcessEDI210DataDelegate(AddressOf Me.InsertEDI210OutboundData)
        ' Launch thread
        fetcher.BeginInvoke(Nothing, Nothing)

    End Sub

    Public Sub InsertEDI210OutboundData()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                db.spGetEDI210OutboundData()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertEDI210OutboundData"))
            End Try

        End Using
    End Sub

    Public Function GetEDI210OutboundData() As DTO.tbl210EDI()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim EDI210s() = (
                From d In db.tbl210EDIs
                Where
                    (d._997Received = False) _
                    And
                    (d.Archived = False) _
                    And
                    (d._210Retry <= 3)
                Select selectDTOData(d, db)).ToArray()

                For Each e In EDI210s
                    Dim edi = e
                    Try
                        Dim oLTS = (From d In db.tbl210EDIs Where d._210EDIControl = edi.EDI210Control).FirstOrDefault()
                        If Not oLTS Is Nothing AndAlso oLTS._210EDIControl > 0 Then
                            If oLTS.FirstDateSent.HasValue Then
                                oLTS.LastDateSent = Date.Now
                                oLTS._210Retry += 1
                            Else
                                oLTS.FirstDateSent = Date.Now
                            End If

                            db.SubmitChanges()
                        End If

                    Catch ex As Exception
                        'Ignore errors when updating
                    End Try
                Next

                Return EDI210s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI210OutboundData"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetEDI210OutboundFeeData(ByVal EDIControl As Integer) As DTO.tbl210EDIFees()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim EDI210Fees() = (
                From d In db.tbl210EDIFees
                Where
                    (d._210EDIFees210EDIControl = EDIControl)
                Select selectDTOData(d, db)).ToArray()


                Return EDI210Fees

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI210OutboundFeeData"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function EDI210InvoiceRejectedVia997(ByVal EDI210Control As Integer) As LTS.sp210RejectedBy997Result
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Return (From d In db.sp210RejectedBy997(EDI210Control) Select d).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("EDI210InvoiceRejectedVia997"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function InvoiceUpdateOn997(ByVal EDI210Control As Integer) As LTS.spInvoiceUpdateOn997Result

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim res As New LTS.spInvoiceUpdateOn997Result

                res = (From d In db.spInvoiceUpdateOn997(EDI210Control) Select d).FirstOrDefault()
                Return res

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InvoiceUpdateOn997"))
            End Try

        End Using
        Return Nothing
    End Function

    Public Function GetEDI210OutboundDataScreen(ByVal StartDate As Date, ByVal EndDate As Date, ByVal ProNumber As String) As DTO.tbl210EDI()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim EDI210s As DTO.tbl210EDI()
            If ProNumber = Nothing Then ProNumber = ""
            Try
                StartDate = DTran.formatStartDateFilter(StartDate)
                EndDate = DTran.formatEndDateFilter(EndDate)
                'Gets all active records that matches the provided criteria
                If ProNumber.Trim.Length > 0 Then
                    EDI210s = (
                                    From d In db.tbl210EDIs
                                    Where
                                        (d.BookProNumber = ProNumber) _
                                        And
                                        ((d.Archived = False) Or (d.Archived = True And d._210EDIStatusCode = LTSCEnum.DataValidationFail)) _
                                        And
                                        ((d.FirstDateSent Is Nothing) _
                                        Or (d.LastDateSent Is Nothing And (d.FirstDateSent >= StartDate And d.FirstDateSent <= EndDate)) _
                                        Or (d.LastDateSent >= StartDate AndAlso d.LastDateSent <= EndDate))
                                    Order By d._210EDIControl Descending
                                    Select selectDTOData(d, db)).ToArray()
                Else
                    EDI210s = (From d In db.tbl210EDIs
                               Where
                                   (
                                        (d.Archived = False) _
                                        Or
                                        (d.Archived = True And d._210EDIStatusCode = LTSCEnum.DataValidationFail)
                                   ) _
                                   And
                                   (
                                        (d.FirstDateSent Is Nothing) _
                                        Or
                                        (d.LastDateSent Is Nothing _
                                            And
                                            (d.FirstDateSent >= StartDate And d.FirstDateSent <= EndDate)
                                        ) _
                                        Or
                                        (d.LastDateSent >= StartDate And d.LastDateSent <= EndDate)
                                   )
                               Order By d._210EDIControl Descending
                               Select selectDTOData(d, db)).ToArray()
                End If

                Return EDI210s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI210OutboundDataScreen"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function SendUpdateOrCancelEDI210Outbound(ByVal BookControl As Integer, ByVal CorrectionsIndicator As String) As Boolean
        Dim success As Boolean = False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                db.spSendUpdateOrCancelEDI210Outbound(BookControl, CorrectionsIndicator)
                success = True
                Return success

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SendUpdateOrCancelEDI210Outbound"))
            End Try

            Return success
        End Using

    End Function

    Public Function ResendEDI210Out(ByVal EDI210Control As Integer) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim retval As Boolean = False
            Try
                'Get the record with the specified control number
                Dim EDI210 = (
                From d In db.tbl210EDIs
                Where
                    d._210EDIControl = EDI210Control).FirstOrDefault()

                If Not EDI210 Is Nothing Then
                    Try
                        EDI210.LastDateSent = Nothing
                        EDI210._210Retry = 0
                        EDI210._210EDIStatusCode = LTSCEnum.ManualRetransmit

                        db.SubmitChanges()
                        retval = True

                    Catch ex As Exception
                        'Ignore errors when updating
                        retval = False
                    End Try
                End If
                Return retval
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ResendEDI210Out"))
            End Try
            Return retval
        End Using

    End Function

    Public Function StopEDI210Out(ByVal EDI210Control As Integer) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim retval As Boolean = False
            Try
                'Get the record with the specified control number
                Dim EDI210 = (
                From d In db.tbl210EDIs
                Where
                    d._210EDIControl = EDI210Control).FirstOrDefault()

                If Not EDI210 Is Nothing Then
                    Try
                        EDI210._210Retry = 4
                        EDI210._210EDIStatusCode = DTO.tblLoadTender.LoadTenderStatusCode.TransmissionStopped

                        db.SubmitChanges()
                        retval = True

                    Catch ex As Exception
                        'Ignore errors when updating
                        retval = False
                    End Try
                End If
                Return retval
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("StopEDI210Out"))
            End Try
            Return retval
        End Using

    End Function

    ''' <summary>
    ''' Sets the Archived Flag to true and the 210EDIStatusCode to StatusCode for the specified 210EDIControl
    ''' </summary>
    ''' <param name="EDI210Control"></param>
    ''' <param name="StatusCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ArchiveEDI210Out(ByVal EDI210Control As Integer, ByVal StatusCode As Integer, ByVal StatusMessage As String) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim retval As Boolean = False
            Try
                'Get the record with the specified control number
                Dim EDI210 = (
                From d In db.tbl210EDIs
                Where
                    d._210EDIControl = EDI210Control).FirstOrDefault()

                If Not EDI210 Is Nothing Then
                    Try
                        EDI210.Archived = 1
                        EDI210._210EDIStatusCode = StatusCode
                        EDI210._210EDIMessage = StatusMessage
                        EDI210._210EDIModDate = Date.Now
                        EDI210._210EDIModUser = Me.Parameters.UserName
                        EDI210.FirstDateSent = Nothing
                        EDI210.LastDateSent = Nothing
                        EDI210._210Retry = 0

                        db.SubmitChanges()
                        retval = True

                    Catch ex As Exception
                        'Ignore errors when updating
                        retval = False
                    End Try
                End If
                Return retval
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ArchiveEDI210Out"))
            End Try
            Return retval
        End Using

    End Function

    Public Sub InsertFileNameTo210Table(ByVal EDI210Control As Integer, Optional ByVal FileName210 As String = "", Optional ByVal FileName997 As String = "", Optional ByVal FileName820 As String = "")
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim blnChanged As Boolean = False
                'Get the record with the specified control number
                Dim EDI210 = (
                From d In db.tbl210EDIs
                Where
                    d._210EDIControl = EDI210Control).FirstOrDefault()

                If Not EDI210 Is Nothing Then
                    Try
                        If FileName210.Trim.Length > 0 Then
                            EDI210._210EDIFileName210 = FileName210
                            blnChanged = True
                        End If
                        If FileName997.Trim.Length > 0 Then
                            EDI210._210EDIFileName997 = FileName997
                            blnChanged = True
                        End If
                        If FileName820.Trim.Length > 0 Then
                            EDI210._210EDIFileName820 = FileName820
                            blnChanged = True
                        End If
                        If blnChanged Then
                            EDI210._210EDIModDate = Date.Now
                            EDI210._210EDIModUser = Me.Parameters.UserName
                        End If

                        db.SubmitChanges()

                    Catch ex As Exception
                        'Ignore errors when updating
                    End Try
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertFileNameTo210Table"))
            End Try

        End Using
    End Sub

    'tblEDI820Log
    Public Function Process820(ByVal BookProNumber As String, ByVal PaidDate820 As Date?, ByVal PaidAmt820 As Decimal, ByVal ACH820 As String, ByVal logMsg820 As String, ByVal fileName As String) As Boolean

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try

                Dim res = db.spProcess820(BookProNumber, PaidDate820, PaidAmt820, ACH820, logMsg820, fileName).FirstOrDefault()
                If res.ErrNumber = 0 Then
                    blnRet = True
                End If

                Return blnRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Process820"))
            End Try
            Return blnRet
        End Using
    End Function

    Public Function GetEDI820DataScreen(ByVal StartDate As Date, ByVal EndDate As Date, ByVal ProNumber As String) As DTO.tblEDI820Log()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim EDI820s As DTO.tblEDI820Log()
            If ProNumber = Nothing Then ProNumber = ""
            Try
                StartDate = DTran.formatStartDateFilter(StartDate)
                EndDate = DTran.formatEndDateFilter(EndDate)
                'Gets all active records that matches the provided criteria
                If ProNumber.Trim.Length > 0 Then
                    EDI820s = (
                                    From d In db.tblEDI820Logs
                                    Where
                                        (d.EDI820InvoiceNumber = ProNumber) _
                                        And
                                        (d.BookFinARPayDate >= StartDate And d.BookFinARPayDate <= EndDate) _
                                        Or (d.BookFinARPayDate Is Nothing And (d.EDI820LogModDate >= StartDate And d.EDI820LogModDate <= EndDate))
                                    Select selectDTOData(d, db)).ToArray()
                Else
                    EDI820s = (
                                    From d In db.tblEDI820Logs
                                    Where
                                        (d.BookFinARPayDate >= StartDate And d.BookFinARPayDate <= EndDate) _
                                        Or (d.BookFinARPayDate Is Nothing And (d.EDI820LogModDate >= StartDate And d.EDI820LogModDate <= EndDate))
                                    Select selectDTOData(d, db)).ToArray()
                End If

                Return EDI820s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI820DataScreen"))
            End Try

            Return Nothing

        End Using
    End Function

    'tblEDI204
    Public Function GetEDI204DataScreen(ByVal StartDate As Date, ByVal EndDate As Date) As DTO.tblEDI204()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                StartDate = DTran.formatStartDateFilter(StartDate)
                EndDate = DTran.formatEndDateFilter(EndDate)
                'Gets all active records that matches the provided criteria
                Dim EDI204s = (
                                From d In db.tblEDI204s
                                Where
                                    ((d.Archived = False) Or (d.Archived = True And d.EDI204StatusCode = LTSCEnum.DataValidationFail)) _
                                    And
                                    ((d.FirstDateSent Is Nothing) _
                                    Or (d.LastDateSent Is Nothing AndAlso (d.FirstDateSent >= StartDate And d.FirstDateSent <= EndDate)) _
                                    Or (d.LastDateSent >= StartDate And d.LastDateSent <= EndDate))
                                Order By d.EDI204Control Descending
                                Select selectDTOData(d, db)).ToArray()

                Return EDI204s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI204DataScreen"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function ResendEDI204(ByVal EDI204Control As Integer) As LTS.spFlagToResend204Result
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim res As New LTS.spFlagToResend204Result
            Try
                'Get the record with the specified control number
                Dim EDI204 = (
                From d In db.tblEDI204s
                Where
                    d.EDI204Control = EDI204Control).FirstOrDefault()

                If Not EDI204 Is Nothing Then

                    res = (From d In db.spFlagToResend204(EDI204.BookControl) Select d).FirstOrDefault()

                End If

                Return res

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ResendEDI204"))
            End Try

            Return Nothing
        End Using
    End Function

    Public Sub InsertFileInfoTo204Table(ByVal BookConsPrefix As String,
                                        ByVal FileName204 As String,
                                        ByVal DateSent As Date,
                                        ByVal EDI204GS06 As Integer)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim blnChanged As Boolean = False
                'Get the record with the specified control number
                Dim EDI204 = (
                From d In db.tblEDI204s
                Where
                    d.Archived = 0 _
                    And
                    d.BookConsPrefix = BookConsPrefix _
                    And
                    d.EDI204FileName204 Is Nothing _
                    And
                    d.EDI204GS06 = 0
                Order By d.EDI204Control Descending).ToArray()

                For Each edi In EDI204
                    If Not edi Is Nothing Then
                        Try
                            edi.EDI204FileName204 = FileName204
                            edi.EDI204StatusCode = LTSCEnum.Pending
                            edi.EDI204GS06 = EDI204GS06

                            If edi.FirstDateSent.HasValue Then
                                edi.LastDateSent = DateSent
                            Else
                                edi.FirstDateSent = DateSent
                            End If

                            edi.EDI204ModDate = DateSent
                            edi.EDI204ModUser = Me.Parameters.UserName

                            db.SubmitChanges()

                        Catch ex As Exception
                            'Ignore errors when updating
                        End Try
                    End If
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertFileInfoTo204Table"))
            End Try

        End Using
    End Sub

    Public Sub ArchiveEDI204(ByVal BookControl As Integer, ByVal StatusCode As Integer, ByVal StatusMessage As String, ByVal DateSent As Date)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the record with the specified control number
                Dim EDI204 = (
                From d In db.tblEDI204s
                Where
                    d.BookControl = BookControl
                Order By d.EDI204Control Descending).FirstOrDefault()

                If Not EDI204 Is Nothing Then
                    Try
                        EDI204.Archived = 1
                        EDI204.EDI204StatusCode = StatusCode
                        EDI204.EDI204Message = StatusMessage
                        EDI204.EDI204ModDate = DateSent
                        EDI204.EDI204ModUser = Me.Parameters.UserName
                        EDI204.FirstDateSent = Nothing
                        EDI204.LastDateSent = Nothing

                        db.SubmitChanges()

                    Catch ex As Exception
                        'Ignore errors when updating
                    End Try
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ArchiveEDI204"))
            End Try
        End Using

    End Sub

    Public Sub InsertFileNamesTo204Table(ByVal EDI204GS06 As Integer,
                                        ByVal DateReceived As Date,
                                        ByVal StatusCode As Integer,
                                        Optional ByVal FileName997 As String = "",
                                        Optional ByVal FileName990 As String = "")
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                'Get the record with the specified control number
                Dim EDI204s = (
                From d In db.tblEDI204s
                Where
                    d.Archived = 0 _
                    And
                    d.EDI204GS06 = EDI204GS06
                Order By d.EDI204Control Descending).ToArray()

                For Each edi In EDI204s
                    If Not edi Is Nothing Then
                        Try
                            If FileName997.Trim.Length > 0 Then
                                edi.EDI204FileName997 = FileName997
                                edi.EDI204997Received = 1
                                edi.EDI204997ReceivedDate = DateReceived
                                edi.EDI204StatusCode = StatusCode
                            End If
                            If FileName990.Trim.Length > 0 Then
                                edi.EDI204FileName990 = FileName990
                                edi._990Received = 1
                                edi._990ReceivedDate = DateReceived
                                edi.EDI204StatusCode = StatusCode
                            End If

                            edi.EDI204ModDate = DateReceived
                            edi.EDI204ModUser = Me.Parameters.UserName

                            db.SubmitChanges()

                        Catch ex As Exception
                            'Ignore errors when updating
                        End Try
                    End If
                Next

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertFileNamesTo204Table"))
            End Try

        End Using
    End Sub

    Public Function Update204990Received(ByVal CNS As String, ByVal StatusCode As String, ByVal DateReceived As Date) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim blnRet As Boolean = True
            Try
                'Get the record with the specified control number
                Dim EDI204s = (
                From d In db.tblEDI204s
                Where
                    d.Archived = 0 _
                    And
                    d.BookConsPrefix = CNS
                Order By d.EDI204Control Descending).ToArray()

                For Each edi In EDI204s
                    If Not edi Is Nothing Then
                        Try
                            edi._990Received = 1
                            edi._990ReceivedDate = DateReceived
                            edi.EDI204StatusCode = StatusCode
                            edi.EDI204ModDate = DateReceived
                            edi.EDI204ModUser = Me.Parameters.UserName

                            db.SubmitChanges()

                        Catch ex As Exception
                            'Ignore errors when updating
                            blnRet = False
                        End Try
                    End If
                Next
                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("Update204990Received"))
            End Try

            Return blnRet
        End Using
    End Function

    'tblEDI990
    Public Function InsertIntoEDI990(ByVal BookConsPrefix As String, ByVal SCAC As String, ByVal StatusCode As Integer, ByVal DateProcessed As Date, ByVal FileName990 As String) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                Dim o990 As New LTS.tblEDI990
                With o990
                    .BookConsPrefix = BookConsPrefix
                    .CarrierSCAC = SCAC
                    .EDI990StatusCode = StatusCode
                    .EDI990Received = 1
                    .EDI990ReceivedDate = DateProcessed
                    .EDI990FileName = FileName990
                    .EDI990ModDate = DateProcessed
                    .EDI990ModUser = Me.Parameters.UserName
                End With

                db.tblEDI990s.InsertOnSubmit(o990)
                db.SubmitChanges()
                blnRet = True

                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertIntoEDI990"))
            End Try

            Return blnRet
        End Using
    End Function

    Public Function GetEDI990DataScreen(ByVal StartDate As Date, ByVal EndDate As Date) As DTO.tblEDI990()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim EDI990s As DTO.tblEDI990()
            Try
                StartDate = DTran.formatStartDateFilter(StartDate)
                EndDate = DTran.formatEndDateFilter(EndDate)

                'Gets all active records that matches the provided criteria
                EDI990s = (
                                    From d In db.tblEDI990s
                                    Where
                                        (d.EDI990ReceivedDate >= StartDate And d.EDI990ReceivedDate <= EndDate)
                                    Select selectDTOData(d, db)).ToArray()

                Return EDI990s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI990DataScreen"))
            End Try

            Return Nothing

        End Using
    End Function

    'tblEDI210In
    Public Function InsertIntoEDI210In(ByVal dto210In As DTO.tblEDI210In, ByVal DateProcessed As Date) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                Dim o210In As New LTS.tblEDI210In
                With o210In
                    .APPONumber = dto210In.APPONumber
                    .APPRONumber = dto210In.APPRONumber
                    .APCNSNumber = dto210In.APCNSNumber
                    .APCarrierNumber = dto210In.APCarrierNumber
                    .APBillNumber = dto210In.APBillNumber
                    .APBillDate = dto210In.APBillDate
                    .APCustomerID = dto210In.APCustomerID
                    .APCostCenterNumber = dto210In.APCostCenterNumber
                    .APTotalCost = dto210In.APTotalCost
                    .APBLNumber = dto210In.APBLNumber
                    .APBilledWeight = dto210In.APBilledWeight
                    .APTotalTax = dto210In.APTotalTax
                    .APFee1 = dto210In.APFee1
                    .APFee2 = dto210In.APFee2
                    .APFee3 = dto210In.APFee3
                    .APFee4 = dto210In.APFee4
                    .APFee5 = dto210In.APFee5
                    .APFee6 = dto210In.APFee6
                    .APOtherCosts = dto210In.APOtherCosts
                    .APCarrierCost = dto210In.APCarrierCost
                    .APOrderSequence = dto210In.APOrderSequence
                    .EDI210InReceived = 1
                    .EDI210InReceivedDate = DateProcessed
                    .EDI210InStatusCode = LTSCEnum.FreightBillReceived
                    .EDI210InMessage = dto210In.EDI210InMessage
                    .EDI210InFileName = dto210In.EDI210InFileName
                    .CarrierName = dto210In.CarrierName
                    .CompName = dto210In.CompName
                    .EDI210InModDate = DateProcessed
                    .EDI210InModUser = Me.Parameters.UserName
                End With

                db.tblEDI210Ins.InsertOnSubmit(o210In)
                db.SubmitChanges()
                blnRet = True

                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertIntoEDI210In"))
            End Try

            Return blnRet
        End Using
    End Function

    Public Function GetEDI210InDataScreen(ByVal StartDate As Date, ByVal EndDate As Date, ByVal blnUseInvoiceDate As Boolean) As DTO.tblEDI210In()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim EDIInbound210s As DTO.tblEDI210In()
            Try
                StartDate = DTran.formatStartDateFilter(StartDate)
                EndDate = DTran.formatEndDateFilter(EndDate)

                If blnUseInvoiceDate Then
                    'Gets all active records that matches the provided criteria
                    EDIInbound210s = (
                                        From d In db.tblEDI210Ins
                                        Where
                                            (d.APBillDate >= StartDate And d.APBillDate <= EndDate)
                                        Select selectDTOData(d, db)).ToArray()
                Else
                    'Gets all active records that matches the provided criteria
                    EDIInbound210s = (
                                        From d In db.tblEDI210Ins
                                        Where
                                            (d.EDI210InReceivedDate >= StartDate And d.EDI210InReceivedDate <= EndDate)
                                        Select selectDTOData(d, db)).ToArray()
                End If

                Return EDIInbound210s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI210InDataScreen"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Sub UpdateFeeLabels210In()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                db.spUpdateFeeLabels210In()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateFeeLabels210In"))
            End Try

        End Using
    End Sub

    'tblEDI214
    Public Function InsertIntoEDI214(ByVal dto214 As DTO.tblEDI214, ByVal DateProcessed As Date) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                Dim o214 As New LTS.tblEDI214
                With o214
                    .EDI214Control = dto214.EDI214Control
                    .BookCarrOrderNumber = dto214.BookCarrOrderNumber
                    .BookOrderSequence = dto214.BookOrderSequence
                    .BookConsPrefix = dto214.BookConsPrefix
                    .CarrierPartnerCode = dto214.CarrierPartnerCode
                    .CompPartnerCode = dto214.CompPartnerCode
                    .EventCode = dto214.EventCode
                    .EventDate = dto214.EventDate
                    .EventTime = dto214.EventTime
                    .BookShipCarrierProNumber = dto214.BookShipCarrierProNumber
                    .BookShipCarrierNumber = dto214.BookShipCarrierNumber
                    .BookShipCarrierName = dto214.BookShipCarrierName
                    .EventComments = dto214.EventComments
                    .CarrierName = dto214.CarrierName
                    .CompName = dto214.CompName
                    .EDI214Received = 1
                    .EDI214ReceivedDate = DateProcessed
                    .EDI214StatusCode = dto214.EDI214StatusCode
                    .EDI214Message = dto214.EDI214Message
                    .EDI214FileName = dto214.EDI214FileName
                    .EDI214ModDate = DateProcessed
                    .EDI214ModUser = Me.Parameters.UserName
                    .SHID = dto214.SHID
                End With

                db.tblEDI214s.InsertOnSubmit(o214)
                db.SubmitChanges()
                blnRet = True

                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertIntoEDI214"))
            End Try

            Return blnRet
        End Using
    End Function

    Public Function GetEDI214DataScreen(ByVal StartDate As Date, ByVal EndDate As Date) As DTO.tblEDI214()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim EDI214s As DTO.tblEDI214()
            Try
                StartDate = DTran.formatStartDateFilter(StartDate)
                EndDate = DTran.formatEndDateFilter(EndDate)

                'Gets all active records that matches the provided criteria
                EDI214s = (From d In db.tblEDI214s
                           Where
                                (d.EDI214ReceivedDate >= StartDate And d.EDI214ReceivedDate <= EndDate)
                           Select selectDTOData(d, db)).ToArray()

                Return EDI214s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI214DataScreen"))
            End Try

            Return Nothing

        End Using
    End Function

    'tblEDI204In

    ''' <summary>
    ''' InsertIntoEDI204In
    ''' </summary>
    ''' <param name="lts204In"></param>
    ''' <param name="DateProcessed"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 5/2/17 for v-7.0.6.105 EDI 204In
    ''' </remarks>
    Public Function InsertIntoEDI204In(ByVal lts204In As LTS.tblEDI204In, ByVal DateProcessed As Date) As Boolean
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim blnRet As Boolean = False
            Try
                lts204In.EDI204InModDate = DateProcessed
                lts204In.EDI204InModUser = Parameters.UserName
                lts204In.EDI204InReceivedDate = DateProcessed

                db.tblEDI204Ins.InsertOnSubmit(lts204In)
                db.SubmitChanges()
                blnRet = True

                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertIntoEDI204In"))
            End Try

            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' GetEDI204InDataScreen
    ''' </summary>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="blnUseShipDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 5/2/17 for v-7.0.6.105 EDI 204In
    ''' </remarks>
    Public Function GetEDI204InDataScreen(ByVal StartDate As Date, ByVal EndDate As Date, ByVal blnUseShipDate As Boolean) As DTO.tblEDI204In()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim EDIInbound204s As DTO.tblEDI204In()
            Try
                StartDate = DTran.formatStartDateFilter(StartDate)
                EndDate = DTran.formatEndDateFilter(EndDate)

                If blnUseShipDate Then
                    'Gets all active records that matches the provided criteria
                    EDIInbound204s = (
                                        From d In db.tblEDI204Ins
                                        Where
                                            (d.ShipDate >= StartDate And d.ShipDate <= EndDate)
                                        Select selectDTOData(d, db)).ToArray()
                Else
                    'Gets all active records that matches the provided criteria
                    EDIInbound204s = (
                                        From d In db.tblEDI204Ins
                                        Where
                                            (d.EDI204InReceivedDate >= StartDate And d.EDI204InReceivedDate <= EndDate)
                                        Select selectDTOData(d, db)).ToArray()
                End If

                Return EDIInbound204s

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI204InDataScreen"))
            End Try

            Return Nothing

        End Using
    End Function

    'selectDTOData
    Friend Function selectDTOData(ByVal d As LTS.tbl210EDI, ByRef db As NGLMASIntegrationDataContext) As DTO.tbl210EDI

        Return New DTO.tbl210EDI With {.EDI210Control = d._210EDIControl _
                                     , .BookControl = d.BookControl _
                                     , .BookProNumber = d.BookProNumber _
                                     , .BookConsPrefix = d.BookConsPrefix _
                                     , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                                     , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
                                     , .BookFinARInvoiceAmt = d.BookFinARInvoiceAmt _
                                     , .Currency = d.CurrencyType _
                                     , .BookDateLoad = d.BookDateLoad _
                                     , .BookCarrActDate = d.BookCarrActDate _
                                     , .BookTypeCode = d.BookTypeCode _
                                     , .BookLoadPONumber = d.BookLoadPONumber _
                                     , .BookLoadPONumber2 = d.BookLoadPONumber2 _
                                     , .BookLoadPONumber3 = d.BookLoadPONumber3 _
                                     , .BookLoadPONumber4 = d.BookLoadPONumber4 _
                                     , .BookLoadPONumber5 = d.BookLoadPONumber5 _
                                     , .BookLoadPONumber6 = d.BookLoadPONumber6 _
                                     , .BookLoadPONumber7 = d.BookLoadPONumber7 _
                                     , .BookLoadPONumber8 = d.BookLoadPONumber8 _
                                     , .BookLoadPONumber9 = d.BookLoadPONumber9 _
                                     , .BookLoadPONumber10 = d.BookLoadPONumber10 _
                                     , .BookOrigName = d.BookOrigName _
                                     , .BookOrigAddress1 = d.BookOrigAddress1 _
                                     , .BookOrigAddress2 = d.BookOrigAddress2 _
                                     , .BookOrigCity = d.BookOrigCity _
                                     , .BookOrigState = d.BookOrigState _
                                     , .BookOrigCountry = d.BookOrigCountry _
                                     , .BookOrigZip = d.BookOrigZip _
                                     , .BookDestName = d.BookDestName _
                                     , .BookDestAddress1 = d.BookDestAddress1 _
                                     , .BookDestAddress2 = d.BookDestAddress2 _
                                     , .BookDestCity = d.BookDestCity _
                                     , .BookDestState = d.BookDestState _
                                     , .BookDestCountry = d.BookDestCountry _
                                     , .BookDestZip = d.BookDestZip _
                                     , .BookTotalWgt = d.BookTotalWgt _
                                     , .BookRevBilledBFC = d.BookRevBilledBFC _
                                     , .CarrierSCAC = d.CarrierSCAC _
                                     , .CarrierName = d.CarrierName _
                                     , .CarrierControl = d.CarrierControl _
                                     , .CarrierNumber = d.CarrierNumber _
                                     , .CompName = d.CompName _
                                     , .CompNumber = d.CompNumber _
                                     , .CompControl = d.CompControl _
                                     , .LaneNumber = d.LaneNumber _
                                     , .LaneOriginAddressUse = d.LaneOriginAddressUse _
                                     , .CorrectionIndicator = d.CorrectionIndicator _
                                     , .EDI400LoopFeesProcessed = d._400LoopFeesProcessed _
                                     , .FirstDateSent = d.FirstDateSent _
                                     , .LastDateSent = d.LastDateSent _
                                     , .EDI997Received = d._997Received _
                                     , .EDI997ReceivedDate = d._997ReceivedDate _
                                     , .EDI210Retry = d._210Retry _
                                     , .CompEDISecurityQual = d.CompEDISecurityQual _
                                     , .CompEDISecurityCode = d.CompEDISecurityCode _
                                     , .CompEDIPartnerQual = d.CompEDIPartnerQual _
                                     , .CompEDIPartnerCode = d.CompEDIPartnerCode _
                                     , .ParamCompEDIPartnerQual = d.ParamCompEDIPartnerQual _
                                     , .ParamCompEDIPartnerCode = d.ParamCompEDIPartnerCode _
                                     , .BookTotalPL = d.BookTotalPL _
                                     , .EDI210StatusCode = d._210EDIStatusCode _
                                     , .EDI210Message = d._210EDIMessage _
                                     , .Archived = d.Archived _
                                     , .EDI210ModDate = d._210EDIModDate _
                                     , .EDI210ModUser = d._210EDIModUser _
                                     , .EDI210FileName210 = d._210EDIFileName210 _
                                     , .EDI210FileName997 = d._210EDIFileName997 _
                                     , .EDI210FileName820 = d._210EDIFileName820 _
                                     , .EDI210Updated = d._210EDIUpdated.ToArray()}
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tbl210EDIFee, ByRef db As NGLMASIntegrationDataContext) As DTO.tbl210EDIFees

        Return New DTO.tbl210EDIFees With {.EDI210FeesControl = d._210EDIFeesControl _
                                     , .EDI210Fees210EDIControl = d._210EDIFees210EDIControl _
                                     , .FeeName = d.FeeName _
                                     , .FeeCost = d.FeeCost _
                                     , .EDICode = d.EDICode _
                                     , .EDI210FeesModDate = d._210EDIFeesModDate _
                                     , .EDI210FeesModUser = d._210EDIFeesModUser _
                                     , .EDI210FeesUpdated = d._210EDIFeesUpdated.ToArray()}
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblEDI820Log, ByRef db As NGLMASIntegrationDataContext) As DTO.tblEDI820Log

        Return New DTO.tblEDI820Log With {.EDI820LogControl = d.EDI820LogControl _
                                     , .BookFinARPayDate = d.BookFinARPayDate _
                                     , .BookFinARPayAmt = d.BookFinARPayAmt _
                                     , .BookFinARCheck = d.BookFinARCheck _
                                     , .BookFinARBalance = d.BookFinARBalance _
                                     , .EDI820LogMessage = d.EDI820LogMessage _
                                     , .EDI820InvoiceNumber = d.EDI820InvoiceNumber _
                                     , .EDI820BookControl = d.EDI820BookControl _
                                     , .EDI820CompControl = d.EDI820CompControl _
                                     , .EDI820CompNumber = d.EDI820CompNumber _
                                     , .EDI820CompName = d.EDI820CompName _
                                     , .EDI820CarrierControl = d.EDI820CarrierControl _
                                     , .EDI820CarrierNumber = d.EDI820CarrierNumber _
                                     , .EDI820LogFileName820 = d.EDI820LogFileName820 _
                                     , .EDI820CarrierName = d.EDI820CarrierName _
                                     , .EDI820LogModDate = d.EDI820LogModDate _
                                     , .EDI820LogModUser = d.EDI820LogModUser _
                                     , .EDI820LogUpdated = d.EDI820LogUpdated.ToArray()}
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblEDI204, ByRef db As NGLMASIntegrationDataContext) As DTO.tblEDI204

        Return New DTO.tblEDI204 With {.EDI204Control = d.EDI204Control _
                                  , .CarrierSCAC = d.CarrierSCAC _
                                  , .CarrierNumber = d.CarrierNumber _
                                  , .CarrierName = d.CarrierName _
                                  , .BookControl = d.BookControl _
                                  , .BookProNumber = d.BookProNumber _
                                  , .BookConsPrefix = d.BookConsPrefix _
                                  , .BookStopNo = d.BookStopNo _
                                  , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                                  , .BookOrderSequence = d.BookOrderSequence _
                                  , .BookRouteFinalCode = d.BookRouteFinalCode _
                                  , .BookTransactionPurpose = d.BookTransactionPurpose _
                                  , .BookTotalCases = d.BookTotalCases _
                                  , .BookTotalWgt = d.BookTotalWgt _
                                  , .BookTotalPL = d.BookTotalPL _
                                  , .BookTotalCube = d.BookTotalCube _
                                  , .BookDateLoad = d.BookDateLoad _
                                  , .BookDateRequired = d.BookDateRequired _
                                  , .BookCarrScheduleDate = d.BookCarrScheduleDate _
                                  , .BookCarrScheduleTime = d.BookCarrScheduleTime _
                                  , .BookCarrApptDate = d.BookCarrApptDate _
                                  , .BookCarrApptTime = d.BookCarrApptTime _
                                  , .BookOrigName = d.BookOrigName _
                                  , .BookOrigAddress1 = d.BookOrigAddress1 _
                                  , .BookOrigAddress2 = d.BookOrigAddress2 _
                                  , .BookOrigAddress3 = d.BookOrigAddress3 _
                                  , .BookOrigCity = d.BookOrigCity _
                                  , .BookOrigState = d.BookOrigState _
                                  , .BookOrigCountry = d.BookOrigCountry _
                                  , .BookOrigZip = d.BookOrigZip _
                                  , .BookOrigPhone = d.BookOrigPhone _
                                  , .BookOrigIDENTIFICATIONCODEQUALIFIER = d.BookOrigIDENTIFICATIONCODEQUALIFIER _
                                  , .BookOrigCompanyNumber = d.BookOrigCompanyNumber _
                                  , .BookDestName = d.BookDestName _
                                  , .BookDestAddress1 = d.BookDestAddress1 _
                                  , .BookDestAddress2 = d.BookDestAddress2 _
                                  , .BookDestAddress3 = d.BookDestAddress3 _
                                  , .BookDestCity = d.BookDestCity _
                                  , .BookDestState = d.BookDestState _
                                  , .BookDestCountry = d.BookDestCountry _
                                  , .BookDestZip = d.BookDestZip _
                                  , .BookDestPhone = d.BookDestPhone _
                                  , .BookDestIDENTIFICATIONCODEQUALIFIER = d.BookDestIDENTIFICATIONCODEQUALIFIER _
                                  , .BookDestCompanyNumber = d.BookDestCompanyNumber _
                                  , .BookLoadPONumber = d.BookLoadPONumber _
                                  , .BookLoadCom = d.BookLoadCom _
                                  , .CommCodeDescription = d.CommCodeDescription _
                                  , .LaneComments = d.LaneComments _
                                  , .LaneOriginAddressUse = d.LaneOriginAddressUse _
                                  , .BookTrackDate = d.BookTrackDate _
                                  , .CompEDISecurityQual = d.CompEDISecurityQual _
                                  , .CompEDISecurityCode = d.CompEDISecurityCode _
                                  , .CompEDIPartnerQual = d.CompEDIPartnerQual _
                                  , .CompEDIPartnerCode = d.CompEDIPartnerCode _
                                  , .CompEDIEmailNotificationOn = d.CompEDIEmailNotificationOn _
                                  , .CompEDIEmailAddress = d.CompEDIEmailAddress _
                                  , .CompEDIAcknowledgementRequested = d.CompEDIAcknowledgementRequested _
                                  , .CompEDIMethodOfPayment = d.CompEDIMethodOfPayment _
                                  , .BookRouteConsFlag = d.BookRouteConsFlag _
                                  , .BookRevTotalCost = d.BookRevTotalCost _
                                  , .BillToCompName = d.BillToCompName _
                                  , .BillToCompNumber = d.BillToCompNumber _
                                  , .BillToCompAddress1 = d.BillToCompAddress1 _
                                  , .BillToCompAddress2 = d.BillToCompAddress2 _
                                  , .BillToCompCity = d.BillToCompCity _
                                  , .BillToCompState = d.BillToCompState _
                                  , .BillToCompZip = d.BillToCompZip _
                                  , .BillToCompCountry = d.BillToCompCountry _
                                  , .EDICombineOrdersForStops = d.EDICombineOrdersForStops _
                                  , .BookCustCompControl = d.BookCustCompControl _
                                  , .CompName = d.CompName _
                                  , .FirstDateSent = d.FirstDateSent _
                                  , .LastDateSent = d.LastDateSent _
                                  , .EDI204997Received = d.EDI204997Received _
                                  , .EDI204997ReceivedDate = d.EDI204997ReceivedDate _
                                  , .EDI990Received = d._990Received _
                                  , .EDI990ReceivedDate = d._990ReceivedDate _
                                  , .EDI204Retry = d.EDI204Retry _
                                  , .EDI204StatusCode = d.EDI204StatusCode _
                                  , .EDI204Message = d.EDI204Message _
                                  , .Archived = d.Archived _
                                  , .EDI204ModDate = d.EDI204ModDate _
                                  , .EDI204ModUser = d.EDI204ModUser _
                                  , .EDI204Updated = d.EDI204Updated.ToArray() _
                                  , .EDI204FileName204 = d.EDI204FileName204 _
                                  , .EDI204FileName997 = d.EDI204FileName997 _
                                  , .EDI204FileName990 = d.EDI204FileName990 _
                                  , .EDI204GS06 = d.EDI204GS06 _
                                  , .BookSHID = d.BookSHID}
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblEDI990, ByRef db As NGLMASIntegrationDataContext) As DTO.tblEDI990

        Return New DTO.tblEDI990 With {.EDI990Control = d.EDI990Control _
                                     , .CarrierSCAC = d.CarrierSCAC _
                                     , .BookConsPrefix = d.BookConsPrefix _
                                     , .EDI990Received = d.EDI990Received _
                                     , .EDI990ReceivedDate = d.EDI990ReceivedDate _
                                     , .EDI990StatusCode = d.EDI990StatusCode _
                                     , .EDI990FileName = d.EDI990FileName _
                                     , .EDI990ModDate = d.EDI990ModDate _
                                     , .EDI990ModUser = d.EDI990ModUser _
                                     , .EDI990Updated = d.EDI990Updated.ToArray()}
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblEDI210In, ByRef db As NGLMASIntegrationDataContext) As DTO.tblEDI210In

        Return New DTO.tblEDI210In With {.EDI210InControl = d.EDI210InControl _
                                     , .APPONumber = d.APPONumber _
                                     , .APPRONumber = d.APPRONumber _
                                     , .APCNSNumber = d.APCNSNumber _
                                     , .APCarrierNumber = d.APCarrierNumber _
                                     , .APBillNumber = d.APBillNumber _
                                     , .APBillDate = d.APBillDate _
                                     , .APCustomerID = d.APCustomerID _
                                     , .APCostCenterNumber = d.APCostCenterNumber _
                                     , .APTotalCost = d.APTotalCost _
                                     , .APBLNumber = d.APBLNumber _
                                     , .APBilledWeight = d.APBilledWeight _
                                     , .APTotalTax = d.APTotalTax _
                                     , .APFee1 = d.APFee1 _
                                     , .APFee2 = d.APFee2 _
                                     , .APFee3 = d.APFee3 _
                                     , .APFee4 = d.APFee4 _
                                     , .APFee5 = d.APFee5 _
                                     , .APFee6 = d.APFee6 _
                                     , .APOtherCosts = d.APOtherCosts _
                                     , .APCarrierCost = d.APCarrierCost _
                                     , .APOrderSequence = d.APOrderSequence _
                                     , .EDI210InReceived = d.EDI210InReceived _
                                     , .EDI210InReceivedDate = d.EDI210InReceivedDate _
                                     , .EDI210InStatusCode = d.EDI210InStatusCode _
                                     , .EDI210InMessage = d.EDI210InMessage _
                                     , .EDI210InFileName = d.EDI210InFileName _
                                     , .CarrierName = d.CarrierName _
                                     , .CompName = d.CompName _
                                     , .APFeeDesc1 = d.APFeeDesc1 _
                                     , .APFeeDesc2 = d.APFeeDesc2 _
                                     , .APFeeDesc3 = d.APFeeDesc3 _
                                     , .APFeeDesc4 = d.APFeeDesc4 _
                                     , .APFeeDesc5 = d.APFeeDesc5 _
                                     , .APFeeDesc6 = d.APFeeDesc6 _
                                     , .EDI210InModDate = d.EDI210InModDate _
                                     , .EDI210InModUser = d.EDI210InModUser _
                                     , .EDI210InUpdated = d.EDI210InUpdated.ToArray()}
    End Function

    Friend Function selectDTOData(ByVal d As LTS.tblEDI214, ByRef db As NGLMASIntegrationDataContext) As DTO.tblEDI214

        Return New DTO.tblEDI214 With {.EDI214Control = d.EDI214Control _
                                     , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                                     , .BookOrderSequence = d.BookOrderSequence _
                                     , .BookConsPrefix = d.BookConsPrefix _
                                     , .CarrierPartnerCode = d.CarrierPartnerCode _
                                     , .CompPartnerCode = d.CompPartnerCode _
                                     , .EventCode = d.EventCode _
                                     , .EventDate = d.EventDate _
                                     , .EventTime = d.EventTime _
                                     , .BookShipCarrierProNumber = d.BookShipCarrierProNumber _
                                     , .BookShipCarrierNumber = d.BookShipCarrierNumber _
                                     , .BookShipCarrierName = d.BookShipCarrierName _
                                     , .EventComments = d.EventComments _
                                     , .CarrierName = d.CarrierName _
                                     , .CompName = d.CompName _
                                     , .EDI214Received = d.EDI214Received _
                                     , .EDI214ReceivedDate = d.EDI214ReceivedDate _
                                     , .EDI214StatusCode = d.EDI214StatusCode _
                                     , .EDI214Message = d.EDI214Message _
                                     , .EDI214FileName = d.EDI214FileName _
                                     , .SHID = d.SHID _
                                     , .EDI214ModDate = d.EDI214ModDate _
                                     , .EDI214ModUser = d.EDI214ModUser _
                                     , .EDI214Updated = d.EDI214Updated.ToArray()}
    End Function

    'Added by LVV On 5/2/17 For v-7.0.6.105 EDI 204In
    Friend Shared Function selectDTOData(ByVal d As LTS.tblEDI204In, ByRef db As NGLMASIntegrationDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.tblEDI204In
        Dim oDTO As New DTO.tblEDI204In
        Dim skipObjs As New List(Of String) From {"EDI204InUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .EDI204InUpdated = d.EDI204InUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function


#Region "TMS 365"

    ''' <summary>
    ''' Uses the data from the EDI doc (210In) to get the missing fields for the fee in order to then call the SettlementSave
    ''' logic in the BLL (this code base Is shared by both Web Tender And EDI to process Freight Bills in 365).
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="EDICode"></param>
    ''' <param name="StopNo"></param>
    ''' <param name="EDICodeDesc"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 2/19/18 for v-8.1 TMS 365 PQ EDI
    ''' </remarks>
    Public Function GetDataForEDIFee365(ByVal SHID As String,
                                        ByVal CarrierNumber As Integer,
                                        ByVal CompNumber As Integer,
                                        ByVal OrderNumber As String,
                                        ByVal EDICode As String,
                                        ByVal StopNo As Integer,
                                        ByVal EDICodeDesc As String) As LTS.spGetDataForEDIFee365Result
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Return (From d In db.spGetDataForEDIFee365(SHID, CarrierNumber, CompNumber, OrderNumber, EDICode, StopNo, EDICodeDesc) Select d).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDataForEDIFee365"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Read All Expected and Unapproved Pending Fee Data
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="CompNumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By RHR for v-8.2.1.004 on  12/31/2019
    '''     Returns all expected and unapproved pending 
    '''     fee data For the provided SHID
    '''     The caller, typically EDI processing logic,
    '''     should compare billed fees With missing fees
    '''     And include these with the AP data.
    '''     New logic in v-8.2.1.004 requires user
    '''     approval for all missing fees before the audit will pass
    '''     NOTE:  the procedure For saving billed fees must
    '''            have matching modifications To only save
    '''            the fees provided that are Not missing.
    '''            If no fees are provided billed fees are 
    '''            Not included in the historical record
    ''' </remarks>
    Public Function GetDataForEDIExpectedFees(ByVal SHID As String,
                                        ByVal CarrierNumber As Integer,
                                        ByVal CompNumber As Integer) As Models.SettlementFee()
        Dim oRet As Models.SettlementFee()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                oRet = (From d In db.spGetDataForEDIExpectedFees(SHID, CarrierNumber, CompNumber)
                        Select New Models.SettlementFee _
                        With {.AccessorialCode = d.AccessorialCode,
                            .BilledFee = d.FeeBilled,
                            .BookCarrOrderNumber = d.BookCarrOrderNumber,
                            .BookControl = d.BookControl,
                            .BookOrderSequence = d.BookOrderSequence,
                            .Caption = d.Caption,
                            .Control = d.FeeControl,
                            .Cost = d.Cost,
                            .EDICode = d.EDICode,
                            .Minimum = d.Minimum,
                            .MissingFee = d.MissingFee,
                            .Pending = d.Pending,
                            .StopSequence = d.StopSequence
                            }).ToArray

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDataForEDIExpectedFees"))
            End Try
            Return oRet
        End Using
    End Function

    Public Function GetEDI214(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDI214()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblEDI214
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDI214)
                iQuery = db.tblEDI214s
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI214"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetEDI210In(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblEDI210In()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblEDI210In
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblEDI210In)
                iQuery = db.tblEDI210Ins
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI210In"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetEDI204TruckLoadData(ByRef CarrierControl As Integer) As LTS.spGetEDI204TruckLoadDataResult()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Return (From d In db.spGetEDI204TruckLoadData(CarrierControl) Select d).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI204TruckLoadData"))
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetEDI204ItemDetails(ByRef BookControl As Integer) As LTS.spgetEDI204ItemDetailsResult()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Return (From d In db.spgetEDI204ItemDetails(BookControl) Select d).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetEDI204ItemDetails"))
            End Try
        End Using
        Return Nothing
    End Function

#End Region


#End Region

#Region "Protected Functions"
    'tbl210EDI
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.tbl210EDI)
        'Create New Record
        Return New LTS.tbl210EDI With {._210EDIControl = d.EDI210Control _
                                     , .BookControl = d.BookControl _
                                     , .BookProNumber = d.BookProNumber _
                                     , .BookConsPrefix = d.BookConsPrefix _
                                     , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                                     , .BookFinARInvoiceDate = d.BookFinARInvoiceDate _
                                     , .BookFinARInvoiceAmt = d.BookFinARInvoiceAmt _
                                     , .CurrencyType = d.Currency _
                                     , .BookDateLoad = d.BookDateLoad _
                                     , .BookCarrActDate = d.BookCarrActDate _
                                     , .BookTypeCode = d.BookTypeCode _
                                     , .BookLoadPONumber = d.BookLoadPONumber _
                                     , .BookLoadPONumber2 = d.BookLoadPONumber2 _
                                     , .BookLoadPONumber3 = d.BookLoadPONumber3 _
                                     , .BookLoadPONumber4 = d.BookLoadPONumber4 _
                                     , .BookLoadPONumber5 = d.BookLoadPONumber5 _
                                     , .BookLoadPONumber6 = d.BookLoadPONumber6 _
                                     , .BookLoadPONumber7 = d.BookLoadPONumber7 _
                                     , .BookLoadPONumber8 = d.BookLoadPONumber8 _
                                     , .BookLoadPONumber9 = d.BookLoadPONumber9 _
                                     , .BookLoadPONumber10 = d.BookLoadPONumber10 _
                                     , .BookOrigName = d.BookOrigName _
                                     , .BookOrigAddress1 = d.BookOrigAddress1 _
                                     , .BookOrigAddress2 = d.BookOrigAddress2 _
                                     , .BookOrigCity = d.BookOrigCity _
                                     , .BookOrigState = d.BookOrigState _
                                     , .BookOrigCountry = d.BookOrigCountry _
                                     , .BookOrigZip = d.BookOrigZip _
                                     , .BookDestName = d.BookDestName _
                                     , .BookDestAddress1 = d.BookDestAddress1 _
                                     , .BookDestAddress2 = d.BookDestAddress2 _
                                     , .BookDestCity = d.BookDestCity _
                                     , .BookDestState = d.BookDestState _
                                     , .BookDestCountry = d.BookDestCountry _
                                     , .BookDestZip = d.BookDestZip _
                                     , .BookTotalWgt = d.BookTotalWgt _
                                     , .BookRevBilledBFC = d.BookRevBilledBFC _
                                     , .CarrierSCAC = d.CarrierSCAC _
                                     , .CarrierName = d.CarrierName _
                                     , .CarrierControl = d.CarrierControl _
                                     , .CarrierNumber = d.CarrierNumber _
                                     , .CompName = d.CompName _
                                     , .CompNumber = d.CompNumber _
                                     , .CompControl = d.CompControl _
                                     , .LaneNumber = d.LaneNumber _
                                     , .LaneOriginAddressUse = d.LaneOriginAddressUse _
                                     , .CorrectionIndicator = d.CorrectionIndicator _
                                     , ._400LoopFeesProcessed = d.EDI400LoopFeesProcessed _
                                     , .FirstDateSent = d.FirstDateSent _
                                     , .LastDateSent = d.LastDateSent _
                                     , ._997Received = d.EDI997Received _
                                     , ._997ReceivedDate = d.EDI997ReceivedDate _
                                     , ._210Retry = d.EDI210Retry _
                                     , .CompEDISecurityQual = d.CompEDISecurityQual _
                                     , .CompEDISecurityCode = d.CompEDISecurityCode _
                                     , .CompEDIPartnerQual = d.CompEDIPartnerQual _
                                     , .CompEDIPartnerCode = d.CompEDIPartnerCode _
                                     , .ParamCompEDIPartnerQual = d.ParamCompEDIPartnerQual _
                                     , .ParamCompEDIPartnerCode = d.ParamCompEDIPartnerCode _
                                     , .BookTotalPL = d.BookTotalPL _
                                     , ._210EDIStatusCode = d.EDI210StatusCode _
                                     , ._210EDIMessage = d.EDI210Message _
                                     , .Archived = d.Archived _
                                     , ._210EDIModDate = d.EDI210ModDate _
                                     , ._210EDIModUser = d.EDI210ModUser _
                                     , ._210EDIFileName210 = d.EDI210FileName210 _
                                     , ._210EDIFileName997 = d.EDI210FileName997 _
                                     , ._210EDIFileName820 = d.EDI210FileName820 _
                                     , ._210EDIUpdated = If(d.EDI210Updated Is Nothing, New Byte() {}, d.EDI210Updated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetEDI210OutFiltered(Control:=CType(LinqTable, LTS.tbl210EDI)._210EDIControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim source As LTS.tbl210EDI = TryCast(LinqTable, LTS.tbl210EDI)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.tbl210EDIs
                       Where d._210EDIControl = source._210EDIControl
                       Select New DTO.QuickSaveResults With {.Control = d._210EDIControl _
                                                     , .ModDate = Date.Now _
                                                     , .ModUser = Parameters.UserName _
                                                     , .Updated = d._210EDIUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

#End Region

End Class


