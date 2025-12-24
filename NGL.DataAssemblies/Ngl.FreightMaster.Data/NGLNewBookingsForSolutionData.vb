Imports System.ServiceModel
Imports Ngl.Core.Utility

Public Class NGLNewBookingsForSolutionData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.vNewBookingsForSolutions
        Me.LinqDB = db
        Me.SourceClass = "NGLNewBookingsForSolutionData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            Me.LinqTable = db.vNewBookingsForSolutions
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetNewBookingFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be providced.
    ''' The Solution Control Number is used to ignore new booking records that have already been assigned to the solution. 
    ''' The FKControl parameter is a reference to the Comp Control Number.
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="SolutionControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseLoadDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetFirstRecord(ByVal LowerControl As Long,
                                             ByVal FKControl As Long,
                                             ByVal SolutionControl As Long,
                                             ByVal FromDate As Date,
                                             ByVal ToDate As Date,
                                             ByVal UseLoadDate As Boolean,
                                             ByVal NatAccountNumber As Integer,
                                             ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass

        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                FromDate = DataTransformation.formatStartDateFilter(FromDate)
                ToDate = DataTransformation.formatEndDateFilter(ToDate)
                Dim NewBooking As New DataTransferObjects.tblSolutionDetail
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                If SolutionControl <> 0 Then
                    Dim SolutionTrucks = From st In db.tblSolutionTrucks Where st.SolutionTruckSolutionControl = SolutionControl Select st.SolutionTruckControl
                    Dim AssignedBookings = From t In db.tblSolutionDetails Where SolutionTrucks.Contains(t.SolutionDetailSolutionTruckControl) Select t.SolutionDetailBookControl

                    NewBooking = (
                        From d In db.vNewBookingsForSolutions
                            Where (LowerControl = 0 OrElse d.SolutionDetailBookControl >= LowerControl) _
                                  And
                                  (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                  And
                                  (If(UseLoadDate,
                                      (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                      (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                      )
                                      ) _
                                  And
                                  (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                  And
                                  (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode) _
                                  And
                                  (AssignedBookings Is Nothing _
                                   OrElse AssignedBookings.Count < 1 _
                                   OrElse Not AssignedBookings.Contains(d.SolutionDetailBookControl))
                            Order By d.SolutionDetailBookControl
                            Select selectDTOData(d)).FirstOrDefault()
                Else
                    'Zero indicates that we should get all the records no solution exists
                    NewBooking = (
                        From d In db.vNewBookingsForSolutions
                            Where (LowerControl = 0 OrElse d.SolutionDetailBookControl >= LowerControl) _
                                  And
                                  (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                  And
                                  (If(UseLoadDate,
                                      (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                      (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                      )
                                      ) _
                                  And
                                  (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                  And
                                  (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                            Order By d.SolutionDetailBookControl
                            Select selectDTOData(d)).FirstOrDefault()
                End If
                Return NewBooking
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetFirstRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The CurrentControl parameter is the seed for the previous record.
    ''' The Solution Control Number is used to ignore new booking records that have already been assigned to the solution. 
    ''' The FKControl parameter is a reference to the Comp Control Number.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="SolutionControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseLoadDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetPreviousRecord(ByVal CurrentControl As Long,
                                                ByVal FKControl As Long,
                                                ByVal SolutionControl As Long,
                                                ByVal FromDate As Date,
                                                ByVal ToDate As Date,
                                                ByVal UseLoadDate As Boolean,
                                                ByVal NatAccountNumber As Integer,
                                                ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                FromDate = DataTransformation.formatStartDateFilter(FromDate)
                ToDate = DataTransformation.formatEndDateFilter(ToDate)
                'For the test we use the book table and the bookload data is ignored.  
                'The final query will need to use a view that selects the book load data because
                'We need the temperature.
                Dim NewBooking As New DataTransferObjects.tblSolutionDetail
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                If SolutionControl <> 0 Then
                    Dim SolutionTrucks = From st In db.tblSolutionTrucks Where st.SolutionTruckSolutionControl = SolutionControl Select st.SolutionTruckControl
                    Dim AssignedBookings = From t In db.tblSolutionDetails Where SolutionTrucks.Contains(t.SolutionDetailSolutionTruckControl) Select t.SolutionDetailBookControl

                    'Get the first record that matches the provided criteria
                    NewBooking = (
                        From d In db.vNewBookingsForSolutions
                            Where d.SolutionDetailBookControl < CurrentControl _
                                  And
                                  (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                  And
                                  (If(UseLoadDate,
                                      (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                      (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                      )
                                      ) _
                                  And
                                  (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                  And
                                  (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode) _
                                  And
                                  (AssignedBookings Is Nothing _
                                   OrElse AssignedBookings.Count < 1 _
                                   OrElse Not AssignedBookings.Contains(d.SolutionDetailBookControl))
                            Order By d.SolutionDetailBookControl Descending
                            Select selectDTOData(d)).FirstOrDefault
                Else
                    NewBooking = (
                        From d In db.vNewBookingsForSolutions
                            Where d.SolutionDetailBookControl < CurrentControl _
                                  And
                                  (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                  And
                                  (If(UseLoadDate,
                                      (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                      (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                      )
                                      ) _
                                  And
                                  (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                  And
                                  (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                            Order By d.SolutionDetailBookControl Descending
                            Select selectDTOData(d)).FirstOrDefault()
                End If
                Return NewBooking
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetPreviousRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The LowerControl parameter allows for a starting control number to be providced.
    ''' The Solution Control Number is used to ignore new booking records that have already been assigned to the solution. 
    ''' The FKControl parameter is a reference to the Comp Control Number.
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="SolutionControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseLoadDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetNextRecord(ByVal CurrentControl As Long,
                                            ByVal FKControl As Long,
                                            ByVal SolutionControl As Long,
                                            ByVal FromDate As Date,
                                            ByVal ToDate As Date,
                                            ByVal UseLoadDate As Boolean,
                                            ByVal NatAccountNumber As Integer,
                                            ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                FromDate = DataTransformation.formatStartDateFilter(FromDate)
                ToDate = DataTransformation.formatEndDateFilter(ToDate)
                'For the test we use the book table and the bookload data is ignored.  
                'The final query will need to use a view that selects the book load data because
                'We need the temperature.
                Dim NewBooking As New DataTransferObjects.tblSolutionDetail
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber
                If SolutionControl <> 0 Then
                    Dim SolutionTrucks = From st In db.tblSolutionTrucks Where st.SolutionTruckSolutionControl = SolutionControl Select st.SolutionTruckControl
                    Dim AssignedBookings = From t In db.tblSolutionDetails Where SolutionTrucks.Contains(t.SolutionDetailSolutionTruckControl) Select t.SolutionDetailBookControl

                    'Get the first record that matches the provided criteria
                    NewBooking = (
                        From d In db.vNewBookingsForSolutions
                            Where d.SolutionDetailBookControl > CurrentControl _
                                  And
                                  (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                  And
                                  (If(UseLoadDate,
                                      (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                      (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                      )
                                      ) _
                                  And
                                  (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                  And
                                  (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode) _
                                  And
                                  (AssignedBookings Is Nothing _
                                   OrElse AssignedBookings.Count < 1 _
                                   OrElse Not AssignedBookings.Contains(d.SolutionDetailBookControl))
                            Order By d.SolutionDetailBookControl
                            Select selectDTOData(d)).FirstOrDefault()
                Else
                    NewBooking = (
                        From d In db.vNewBookingsForSolutions
                            Where d.SolutionDetailBookControl > CurrentControl _
                                  And
                                  (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                  And
                                  (If(UseLoadDate,
                                      (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                      (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                      )
                                      ) _
                                  And
                                  (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                  And
                                  (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                            Order By d.SolutionDetailBookControl
                            Select selectDTOData(d)).FirstOrDefault()
                End If
                Return NewBooking
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' The UpperControl parameter allows for a starting control number to be providced.
    ''' The Solution Control Number is used to ignore new booking records that have already been assigned to the solution. 
    ''' The FKControl parameter is a reference to the Comp Control Number.
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <param name="SolutionControl"></param>
    ''' <param name="FromDate"></param>
    ''' <param name="ToDate"></param>
    ''' <param name="UseLoadDate"></param>
    ''' <param name="NatAccountNumber"></param>
    ''' <param name="RouteTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function GetLastRecord(ByVal UpperControl As Long,
                                            ByVal FKControl As Long,
                                            ByVal SolutionControl As Long,
                                            ByVal FromDate As Date,
                                            ByVal ToDate As Date,
                                            ByVal UseLoadDate As Boolean,
                                            ByVal NatAccountNumber As Integer,
                                            ByVal RouteTypeCode As Integer) As DataTransferObjects.DTOBaseClass
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                FromDate = DataTransformation.formatStartDateFilter(FromDate)
                ToDate = DataTransformation.formatEndDateFilter(ToDate)
                Dim NewBooking As New DataTransferObjects.tblSolutionDetail
                Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber

                If UpperControl <> 0 Then
                    If SolutionControl <> 0 Then
                        Dim SolutionTrucks = From st In db.tblSolutionTrucks Where st.SolutionTruckSolutionControl = SolutionControl Select st.SolutionTruckControl
                        Dim AssignedBookings = From t In db.tblSolutionDetails Where SolutionTrucks.Contains(t.SolutionDetailSolutionTruckControl) Select t.SolutionDetailBookControl
                        NewBooking = (
                            From d In db.vNewBookingsForSolutions
                                Where d.SolutionDetailBookControl >= UpperControl _
                                      And
                                      (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                      And
                                      (If(UseLoadDate,
                                          (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                          (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                          )
                                          ) _
                                      And
                                      (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                      And
                                      (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode) _
                                      And
                                      (AssignedBookings Is Nothing _
                                       OrElse AssignedBookings.Count < 1 _
                                       OrElse Not AssignedBookings.Contains(d.SolutionDetailBookControl))
                                Order By d.SolutionDetailBookControl
                                Select selectDTOData(d)).FirstOrDefault()
                    Else
                        NewBooking = (
                            From d In db.vNewBookingsForSolutions
                                Where d.SolutionDetailBookControl >= UpperControl _
                                      And
                                      (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                      And
                                      (If(UseLoadDate,
                                          (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                          (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                          )
                                          ) _
                                      And
                                      (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                      And
                                      (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                                Order By d.SolutionDetailBookControl
                                Select selectDTOData(d)).FirstOrDefault()
                    End If
                Else
                    If SolutionControl <> 0 Then
                        Dim SolutionTrucks = From st In db.tblSolutionTrucks Where st.SolutionTruckSolutionControl = SolutionControl Select st.SolutionTruckControl
                        Dim AssignedBookings = From t In db.tblSolutionDetails Where SolutionTrucks.Contains(t.SolutionDetailSolutionTruckControl) Select t.SolutionDetailBookControl
                        NewBooking = (
                            From d In db.vNewBookingsForSolutions
                                Where (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                      And
                                      (If(UseLoadDate,
                                          (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                          (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                          )
                                          ) _
                                      And
                                      (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                      And
                                      (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode) _
                                      And
                                      (AssignedBookings Is Nothing _
                                       OrElse AssignedBookings.Count < 1 _
                                       OrElse Not AssignedBookings.Contains(d.SolutionDetailBookControl))
                                Order By d.SolutionDetailBookControl Descending
                                Select selectDTOData(d)).FirstOrDefault()
                    Else
                        NewBooking = (
                            From d In db.vNewBookingsForSolutions
                                Where (NatAccountNumber <> 0 OrElse (d.SolutionDetailCompControl = FKControl And (oSecureComp Is Nothing OrElse oSecureComp.Count = 0 OrElse oSecureComp.Contains(d.SolutionDetailCompControl)))) _
                                      And
                                      (If(UseLoadDate,
                                          (d.SolutionDetailDateLoad >= FromDate And d.SolutionDetailDateLoad <= ToDate),
                                          (d.SolutionDetailDateRequired >= FromDate And d.SolutionDetailDateRequired <= ToDate)
                                          )
                                          ) _
                                      And
                                      (NatAccountNumber = 0 OrElse (d.SolutionDetailCompNatNumber = NatAccountNumber And (oSecureNat Is Nothing OrElse oSecureNat.Count = 0 OrElse oSecureNat.Contains(d.SolutionDetailCompNatNumber)))) _
                                      And
                                      (RouteTypeCode = 0 OrElse d.SolutionDetailRouteTypeCode = RouteTypeCode)
                                Order By d.SolutionDetailBookControl Descending
                                Select selectDTOData(d)).FirstOrDefault()
                    End If
                End If
                Return NewBooking
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLastRecord"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetNewBookingFiltered(ByVal Control As Long) As DataTransferObjects.tblSolutionDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim NewBooking As DataTransferObjects.tblSolutionDetail = (From d In db.vNewBookingsForSolutions Where d.SolutionDetailBookControl = Control Select selectDTOData(d)).FirstOrDefault()
                Return NewBooking
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNewBookingFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    'Added to get Existing Loads based on truckkey by ManoRama'

    Public Function GetLoadBookingFiltered(ByVal Control As Long) As DataTransferObjects.tblSolutionDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim ExistingBooking As DataTransferObjects.tblSolutionDetail = (From d In db.vLoadBookingsForSolutions
                        Where d.SolutionDetailBookControl = Control Select selectDTOData(d)).FirstOrDefault()
                Return ExistingBooking
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLoadBookingFiltered"))
            End Try
            Return Nothing
        End Using
    End Function
    ''' <summary>
    ''' Return a list of filtered New Booking Records using the vNewBookingsForSolutions View
    ''' </summary>
    ''' <param name="Filter"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.6.101 on 01/31/2016
    ''' </remarks>
    Public Function GetNewBookingsFiltered(ByVal Filter As DataTransferObjects.LoadPlanningTruckDataFilter) As DataTransferObjects.tblSolutionDetail()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Filter.StartDateFilter = DataTransformation.formatStartDateFilter(Filter.StartDateFilter)
                Filter.StopDateFilter = DataTransformation.formatEndDateFilter(Filter.StopDateFilter)
                'For this release the comp control number is requried and must be provided by the caller so we do not need to apply company level restrictions
                'Dim oSecureComp = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName Select s.CompControl
                'Dim oSecureNat = From s In db.vUserAdminWithCompControlRefBooks Where s.UserAdminUserName = Me.Parameters.UserName And s.CompNatNumber <> 0 Select s.CompNatNumber
                Dim OrigStates As New List(Of String)
                Dim blnUseOrigStateFilter As Boolean = False
                Dim DestStates As New List(Of String)
                Dim blnUseDestStateFilter As Boolean = False
                populateStateListsFromFilter(Filter, OrigStates, blnUseOrigStateFilter, DestStates, blnUseDestStateFilter)
                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'count the record
                'db.Log = New DebugTextWriter
                Try
                    'Modified by RHR for v-6.0.6.101 on 01/31/2016
                    Dim qNewBookings = (From d In db.vNewBookingsForSolutions
                            Where
                            (Filter.CompControlFilter = 0 OrElse d.SolutionDetailCompControl = Filter.CompControlFilter) _
                            And If(Filter.UseLoadDateFilter = True,
                                   If(d.SolutionDetailDateLoad.HasValue, d.SolutionDetailDateLoad.Value >= Filter.StartDateFilter, False) And If(d.SolutionDetailDateLoad.HasValue, d.SolutionDetailDateLoad.Value <= Filter.StopDateFilter, False),
                                   If(d.SolutionDetailDateRequired.HasValue, d.SolutionDetailDateRequired.Value >= Filter.StartDateFilter, False) And If(d.SolutionDetailDateRequired.HasValue, d.SolutionDetailDateRequired.Value <= Filter.StopDateFilter, False)) _
                            And (d.SolutionDetailOrigZip.Trim >= If(Filter.OrigStartZipFilter = String.Empty, "0", Filter.OrigStartZipFilter)) _
                            And (d.SolutionDetailOrigZip.Trim <= If(Filter.OrigStopZipFilter = String.Empty, "ZZZZZZZZZZ", Filter.OrigStopZipFilter)) _
                            And (d.SolutionDetailDestZip.Trim >= If(Filter.DestStartZipFilter = String.Empty, "0", Filter.DestStartZipFilter)) _
                            And (d.SolutionDetailDestZip.Trim <= If(Filter.DestStopZipFilter = String.Empty, "ZZZZZZZZZZ", Filter.DestStopZipFilter)) _
                            And If(Filter.OrigCityFilter.Trim = String.Empty, True, (d.SolutionDetailOrigCity.Trim.ToUpper = Filter.OrigCityFilter.Trim.ToUpper)) _
                            And If(Filter.DestCityFilter.Trim = String.Empty, True, (d.SolutionDetailDestCity.Trim.ToUpper = Filter.DestCityFilter.Trim.ToUpper)) _
                            And (blnUseOrigStateFilter = False _
                                 OrElse (OrigStates.Contains(d.SolutionDetailOrigState))) _
                            And (blnUseDestStateFilter = False _
                                 OrElse (DestStates.Contains(d.SolutionDetailDestState))) _
                            And ((Filter.BookTransTypeFilter.Trim Is Nothing OrElse Filter.BookTransTypeFilter.Trim Is String.Empty) _
                                 OrElse d.SolutionDetailTransType.ToUpper = Filter.BookTransTypeFilter.ToUpper)
                            Select d.SolutionDetailPOHdrControl).ToArray()
                    If Not qNewBookings Is Nothing Then intRecordCount = qNewBookings.Count
                Catch ex As Exception
                    'ignore any record count errors when counting the available new bookings
                End Try
                If Filter.PageSize < 1 Then Filter.PageSize = 1
                If intRecordCount < 1 Then intRecordCount = 1
                If Filter.Page < 1 Then Filter.Page = 1
                If intRecordCount > Filter.PageSize Then intPageCount = ((intRecordCount - 1) \ Filter.PageSize) + 1
                Dim intSkip As Integer = (Filter.Page - 1) * Filter.PageSize
                'Return all the booking records that match the criteria sorted by name
                Dim NewBookings() As DataTransferObjects.tblSolutionDetail = (
                        From d In db.vNewBookingsForSolutions
                        Where
                        (Filter.CompControlFilter = 0 OrElse d.SolutionDetailCompControl = Filter.CompControlFilter) _
                        And If(Filter.UseLoadDateFilter = True,
                               If(d.SolutionDetailDateLoad.HasValue, d.SolutionDetailDateLoad.Value >= Filter.StartDateFilter, False) And If(d.SolutionDetailDateLoad.HasValue, d.SolutionDetailDateLoad.Value <= Filter.StopDateFilter, False),
                               If(d.SolutionDetailDateRequired.HasValue, d.SolutionDetailDateRequired.Value >= Filter.StartDateFilter, False) And If(d.SolutionDetailDateRequired.HasValue, d.SolutionDetailDateRequired.Value <= Filter.StopDateFilter, False)) _
                        And (d.SolutionDetailOrigZip.Trim >= If(Filter.OrigStartZipFilter = String.Empty, "0", Filter.OrigStartZipFilter)) _
                        And (d.SolutionDetailOrigZip.Trim <= If(Filter.OrigStopZipFilter = String.Empty, "ZZZZZZZZZZ", Filter.OrigStopZipFilter)) _
                        And (d.SolutionDetailDestZip.Trim >= If(Filter.DestStartZipFilter = String.Empty, "0", Filter.DestStartZipFilter)) _
                        And (d.SolutionDetailDestZip.Trim <= If(Filter.DestStopZipFilter = String.Empty, "ZZZZZZZZZZ", Filter.DestStopZipFilter)) _
                        And If(Filter.OrigCityFilter.Trim = String.Empty, True, (d.SolutionDetailOrigCity.Trim.ToUpper = Filter.OrigCityFilter.Trim.ToUpper)) _
                        And If(Filter.DestCityFilter.Trim = String.Empty, True, (d.SolutionDetailDestCity.Trim.ToUpper = Filter.DestCityFilter.Trim.ToUpper)) _
                        And (blnUseOrigStateFilter = False _
                             OrElse (OrigStates.Contains(d.SolutionDetailOrigState))) _
                        And (blnUseDestStateFilter = False _
                             OrElse (DestStates.Contains(d.SolutionDetailDestState))) _
                        And ((Filter.BookTransTypeFilter.Trim Is Nothing OrElse Filter.BookTransTypeFilter.Trim Is String.Empty) _
                             OrElse d.SolutionDetailTransType.ToUpper = Filter.BookTransTypeFilter.ToUpper)
                        Order By d.SolutionDetailBookControl Descending
                        Select selectDTOData(d, Filter.Page, intPageCount, intRecordCount, Filter.PageSize)).Skip(intSkip).Take(Filter.PageSize).ToArray()
                Return NewBookings
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNewBookingsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetNewBookingFilteredByPro(ByVal BookProNumber As String) As DataTransferObjects.tblSolutionDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Get the newest record that matches the provided criteria
                Dim NewBooking As DataTransferObjects.tblSolutionDetail = (From d In db.vNewBookingsForSolutions Where d.SolutionDetailProNumber = BookProNumber Select selectDTOData(d)).FirstOrDefault()
                Return NewBooking
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNewBookingFilteredByPro"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.110 9/27/16
    '''   new method to retrieve Solution data using the new BookWaitingToProcessNewBooking 
    ''' </remarks>
    Public Function GetNextNewBookWaitingToProcess() As DataTransferObjects.tblSolutionDetail
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Get the newest record that matches the provided criteria
                Dim NewBooking As DataTransferObjects.tblSolutionDetail = (From d In db.vNewBookingsWaitingForSolutions Order By d.SolutionDetailBookControl Select selectDTOData(d)).FirstOrDefault()
                If Not NewBooking Is Nothing AndAlso NewBooking.SolutionDetailBookControl <> 0 Then
                    'as soon as we have a booking record flagged as waiting to be processed we mark it
                    'as finished processing because we only want to process this record once, even if the 
                    'remaining procedures fail
                    DirectCast(Me.NDPBaseClassFactory("NGLBookData", False), NGLBookData).UpdateBookWaitingToProcessNewBooking(NewBooking.SolutionDetailBookControl)
                End If
                Return NewBooking
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNextNewBookWaitingToProcess"))
            End Try
            Return Nothing
        End Using
    End Function

    Friend Shared Function selectDTOData(ByVal d As LTS.vNewBookingsForSolution, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.tblSolutionDetail
        Dim oDTO As New DataTransferObjects.tblSolutionDetail
        Dim skipObjs As New List(Of String) From {"SolutionDetailUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionDetailUpdated = d.SolutionDetailUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

    ''Added to get Existing Loads based on truckkey changing DTOData parameter with new view vLoadBookingsForSolution by  ManoRama'
    Friend Shared Function selectDTOData(ByVal d As LTS.vLoadBookingsForSolution, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.tblSolutionDetail
        Dim oDTO As New DataTransferObjects.tblSolutionDetail
        Dim skipObjs As New List(Of String) From {"SolutionDetailUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionDetailUpdated = d.SolutionDetailUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function
    Friend Shared Function selectDTOData(ByVal d As LTS.vNewBookingsWaitingForSolution, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.tblSolutionDetail
        Dim oDTO As New DataTransferObjects.tblSolutionDetail
        Dim skipObjs As New List(Of String) From {"SolutionDetailUpdated", "Page", "Pages", "RecordCount", "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .SolutionDetailUpdated = d.SolutionDetailUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO
    End Function

#End Region


#Region "LTS & 365 Updates"


    ''' <summary>
    ''' Return a list of filtered New Booking Records using the vNewBookingsForSolutions View
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-6.0.6.101 on 01/31/2016
    ''' </remarks>
    Public Function GetNewBookingsFiltered365(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As DataTransferObjects.tblSolutionDetail()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As DataTransferObjects.tblSolutionDetail
        Dim oNewData() As LTS.vNewBookingsForSolution365
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim leComps As Integer() = db.vLEComp365RefBooks.Where(Function(t) t.LEAdminControl = Parameters.UserLEControl And t.UserSecurityControl = Parameters.UserControl).Select(Function(x) x.CompControl).ToArray()
                If leComps Is Nothing OrElse leComps.Count < 1 Then Return Nothing
                Dim OrigStates As New List(Of String)
                Dim blnUseOrigStateFilter As Boolean = False
                Dim DestStates As New List(Of String)
                Dim blnUseDestStateFilter As Boolean = False
                populateStateListsFromFilter(filters, OrigStates, blnUseOrigStateFilter, DestStates, blnUseDestStateFilter)
                Dim iQuery As IQueryable(Of LTS.vNewBookingsForSolution365)

                iQuery = (From t In db.vNewBookingsForSolution365s
                    Where (leComps.Contains(t.BookCustCompControl)) And (blnUseOrigStateFilter = False OrElse OrigStates.Contains(t.BookOrigState)) And (blnUseDestStateFilter = False OrElse DestStates.Contains(t.BookDestState))
                    Select t)
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oNewData = iQuery.Skip(filters.skip).Take(filters.take).ToArray()

                oRet = (From d In oNewData Select CopyvNewBookingsForSolution365SolutionDetailData(d)).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNewBookingsFiltered"))
            End Try
            Return Nothing
        End Using
    End Function

#End Region

#Region "Protected Functions"


    Protected Function CopyvNewBookingsForSolution365SolutionDetailData(ByVal oData As LTS.vNewBookingsForSolution365) As DataTransferObjects.tblSolutionDetail
        Dim oSolutionDetails As New DataTransferObjects.tblSolutionDetail()
        With oData
            oSolutionDetails.SolutionDetailControl = 0
            oSolutionDetails.SolutionDetailSolutionTruckControl = .BookCarrTruckControl
            oSolutionDetails.SolutionDetailBookControl = .BookControl
            oSolutionDetails.SolutionDetailPOHdrControl = .POHdrControl
            oSolutionDetails.SolutionDetailBookLoadControl = .BookLoadControl
            oSolutionDetails.SolutionDetailProNumber = .BookProNumber
            oSolutionDetails.SolutionDetailPONumber = .BookLoadPONumber
            oSolutionDetails.SolutionDetailOrderNumber = .BookCarrOrderNumber
            oSolutionDetails.SolutionDetailOrderSequence = .BookOrderSequence
            oSolutionDetails.SolutionDetailCom = .BookLoadCom
            oSolutionDetails.SolutionDetailConsPrefix = .BookConsPrefix
            oSolutionDetails.SolutionDetailCompControl = .BookCustCompControl
            oSolutionDetails.SolutionDetailCompNumber = If(.CompNumber, 0)
            oSolutionDetails.SolutionDetailCompName = .CompName
            oSolutionDetails.SolutionDetailCompNatNumber = If(.CompNatNumber, 0)
            oSolutionDetails.SolutionDetailCompNatName = .CompNatName
            oSolutionDetails.SolutionDetailODControl = .BookODControl
            oSolutionDetails.SolutionDetailCarrierControl = .BookCarrierControl
            oSolutionDetails.SolutionDetailCarrierNumber = .CarrierNumber
            oSolutionDetails.SolutionDetailCarrierName = .CarrierName
            oSolutionDetails.SolutionDetailOrigCompControl = If(.BookOrigCompControl, 0)
            oSolutionDetails.SolutionDetailOrigName = .BookOrigName
            oSolutionDetails.SolutionDetailOrigAddress1 = .BookOrigAddress1
            oSolutionDetails.SolutionDetailOrigAddress2 = .BookOrigAddress2
            oSolutionDetails.SolutionDetailOrigAddress3 = .BookOrigAddress3
            oSolutionDetails.SolutionDetailOrigCity = .BookOrigCity
            oSolutionDetails.SolutionDetailOrigState = .BookOrigState
            oSolutionDetails.SolutionDetailOrigCountry = .BookOrigCountry
            oSolutionDetails.SolutionDetailOrigZip = .BookOrigZip
            oSolutionDetails.SolutionDetailDestCompControl = If(.BookDestCompControl, 0)
            oSolutionDetails.SolutionDetailDestName = .BookDestName
            oSolutionDetails.SolutionDetailDestAddress1 = .BookDestAddress1
            oSolutionDetails.SolutionDetailDestAddress2 = .BookDestAddress2
            oSolutionDetails.SolutionDetailDestAddress3 = .BookDestAddress3
            oSolutionDetails.SolutionDetailDestCity = .BookDestCity
            oSolutionDetails.SolutionDetailDestState = .BookDestState
            oSolutionDetails.SolutionDetailDestCountry = .BookDestCountry
            oSolutionDetails.SolutionDetailDestZip = .BookDestZip
            oSolutionDetails.SolutionDetailDateOrdered = .BookDateOrdered
            oSolutionDetails.SolutionDetailDateLoad = .BookDateLoad
            oSolutionDetails.SolutionDetailDateRequired = .BookDateRequired
            oSolutionDetails.SolutionDetailTotalCases = If(.BookTotalCases, 0)
            oSolutionDetails.SolutionDetailTotalWgt = If(.BookTotalWgt, 0)
            oSolutionDetails.SolutionDetailTotalPL = If(.BookTotalPL, 0)
            oSolutionDetails.SolutionDetailTotalCube = If(.BookTotalCube, 0)
            oSolutionDetails.SolutionDetailTotalPX = .BookTotalPX
            oSolutionDetails.SolutionDetailTotalBFC = If(.BookRevBilledBFC, 0)
            oSolutionDetails.SolutionDetailTranCode = .BookTranCode
            oSolutionDetails.SolutionDetailPayCode = .BookPayCode
            oSolutionDetails.SolutionDetailTypeCode = .BookTypeCode
            oSolutionDetails.SolutionDetailStopNo = .BookStopNo
            oSolutionDetails.SolutionDetailModDate = .BookModDate
            oSolutionDetails.SolutionDetailModUser = .BookModUser
            oSolutionDetails.SolutionDetailUpdated = .BookUpdated.ToArray()
            oSolutionDetails.SolutionDetailMilesFrom = .LaneBenchMiles
            oSolutionDetails.SolutionDetailHoldLoad = .BookHoldLoad
            oSolutionDetails.SolutionDetailTransType = .BookTransType
            oSolutionDetails.SolutionDetailDateRequested = .BookDateRequested
            oSolutionDetails.SolutionDetailCarrierEquipmentCodes = .BookCarrierEquipmentCodes
            oSolutionDetails.SolutionDetailRouteTypeCode = .BookRouteTypeCode
            oSolutionDetails.SolutionDetailIsHazmat = .IsHazmat
            oSolutionDetails.SolutionDetailInbound = .Inbound
            oSolutionDetails.SolutionDetailRouteGuideNumber = .BookRouteGuideNumber
            oSolutionDetails.SolutionDetailLaneNumber = .LaneNumber
            oSolutionDetails.SolutionDetailLaneName = .LaneName
            oSolutionDetails.SolutionDetailBookNotes = .BookNotesVisable1
            oSolutionDetails.SolutionDetailBookCarrTarControl = .BookCarrTarControl
        End With
        Return oSolutionDetails

    End Function


    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Return Nothing
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return Nothing
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow records to be added 
        Utilities.SaveAppError("Cannot add data.  Records cannot be created using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow records to be updated 
        Utilities.SaveAppError("Cannot save data.  Records cannot be modified using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'We do not allow Route Type records to be deleted 
        Utilities.SaveAppError("Cannot delete data.  Records cannot be deleted using this interface!", Me.Parameters)
        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_AccessDenied"}, New FaultReason("E_DataValidationFailure"))

    End Sub

#End Region

End Class