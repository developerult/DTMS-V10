Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core.ChangeTracker
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports System.Linq.Dynamic

Public Class NGLAMSAppointmentData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASAMSDataContext(ConnectionString)
        Me.LinqTable = db.AMSAppointments
        Me.LinqDB = db
        Me.SourceClass = "NGLAMSAppointmentData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASAMSDataContext(ConnectionString)
            _LinqTable = db.AMSAppointments
            Me.LinqDB = db
            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

#Region "DEPRICIATED"

    ''' <summary>
    ''' DEPRECIATED By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' Enter zero for LowerControl to return the record with the lowest control PK
    ''' the FKControl parameter is used to filter by compcontrol
    ''' </summary>
    ''' <param name="LowerControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFirstRecord(ByVal LowerControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        throwDepreciatedException(buildProcedureName("GetFirstRecord"))
        Return Nothing
        ''Using db As New NGLMASAMSDataContext(ConnectionString)
        ''    Try
        ''        Dim AMSAppointment As DTO.AMSAppointment
        ''        If LowerControl <> 0 Then
        ''            AMSAppointment = ( _
        ''           From d In db.AMSAppointments _
        ''           Where d.AMSApptControl >= LowerControl _
        ''           And _
        ''           d.AMSApptCompControl = FKControl _
        ''           Order By d.AMSApptControl _
        ''           Select selectDTOData(d, db)).FirstOrDefault
        ''        Else
        ''            'Zero indicates that we should get the record with the lowest control number even if it is below zero
        ''            AMSAppointment = ( _
        ''           From d In db.AMSAppointments _
        ''           Where _
        ''           d.AMSApptCompControl = FKControl _
        ''           Order By d.AMSApptControl _
        ''           Select selectDTOData(d, db)).FirstOrDefault
        ''        End If
        ''        If Not AMSAppointment Is Nothing Then
        ''            Dim osum = GetAMSAppointmentSummaryByControl(AMSAppointment.AMSApptControl)
        ''            AMSAppointment.AMSApptLabel = osum.AMSApptLabel
        ''            AMSAppointment.AMSApptHover = osum.AMSApptHover
        ''        End If
        ''        Return AMSAppointment
        ''    Catch ex As System.Data.SqlClient.SqlException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        ''    Catch ex As InvalidOperationException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        ''    End Try
        ''    Return Nothing
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' Enter the CurrentControl number to use as the reference to the previous record
    ''' the FKControl parameter is used to filter the records by compcontrol
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetPreviousRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        throwDepreciatedException(buildProcedureName("GetPreviousRecord"))
        Return Nothing
        ''Using db As New NGLMASAMSDataContext(ConnectionString)
        ''    Try
        ''        'Get the first record that matches the provided criteria
        ''        Dim AMSAppointment As DTO.AMSAppointment = (
        ''        From d In db.AMSAppointments
        ''        Where d.AMSApptControl < CurrentControl _
        ''        And
        ''        d.AMSApptCompControl = FKControl
        ''        Order By d.AMSApptControl Descending
        ''        Select selectDTOData(d, db)).FirstOrDefault
        ''        If Not AMSAppointment Is Nothing Then
        ''            Dim osum = GetAMSAppointmentSummaryByControl(AMSAppointment.AMSApptControl)
        ''            AMSAppointment.AMSApptLabel = osum.AMSApptLabel
        ''            AMSAppointment.AMSApptHover = osum.AMSApptHover
        ''        End If
        ''        Return AMSAppointment
        ''    Catch ex As System.Data.SqlClient.SqlException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        ''    Catch ex As InvalidOperationException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        ''    End Try
        ''    Return Nothing
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' Enter the CurrentControl number to use as the reference to the next record
    ''' the FKControl parameter is used to filter records by compcontrol
    ''' </summary>
    ''' <param name="CurrentControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetNextRecord(ByVal CurrentControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        throwDepreciatedException(buildProcedureName("GetNextRecord"))
        Return Nothing
        ''Using db As New NGLMASAMSDataContext(ConnectionString)
        ''    Try
        ''        'Get the first record that matches the provided criteria
        ''        Dim AMSAppointment As DTO.AMSAppointment = (
        ''        From d In db.AMSAppointments
        ''        Where d.AMSApptControl > CurrentControl _
        ''        And
        ''        d.AMSApptCompControl = FKControl
        ''        Order By d.AMSApptControl
        ''        Select selectDTOData(d, db)).FirstOrDefault
        ''        If Not AMSAppointment Is Nothing Then
        ''            Dim osum = GetAMSAppointmentSummaryByControl(AMSAppointment.AMSApptControl)
        ''            AMSAppointment.AMSApptLabel = osum.AMSApptLabel
        ''            AMSAppointment.AMSApptHover = osum.AMSApptHover
        ''        End If
        ''        Return AMSAppointment
        ''    Catch ex As System.Data.SqlClient.SqlException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        ''    Catch ex As InvalidOperationException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        ''    End Try
        ''    Return Nothing
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' Enter zero as the UpperControl number return the record with the highest control PK
    ''' the FKControl parameter is used to filter records by compcontrol
    ''' </summary>
    ''' <param name="UpperControl"></param>
    ''' <param name="FKControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLastRecord(ByVal UpperControl As Long, ByVal FKControl As Long) As DataTransferObjects.DTOBaseClass
        throwDepreciatedException(buildProcedureName("GetLastRecord"))
        Return Nothing
        ''Using db As New NGLMASAMSDataContext(ConnectionString)
        ''    Try
        ''        Dim AMSAppointment As DTO.AMSAppointment
        ''        If UpperControl <> 0 Then
        ''            AMSAppointment = (
        ''            From d In db.AMSAppointments
        ''            Where d.AMSApptControl >= UpperControl _
        ''            And
        ''            d.AMSApptCompControl = FKControl
        ''            Order By d.AMSApptControl
        ''            Select selectDTOData(d, db)).FirstOrDefault
        ''        Else
        ''            'Zero indicates that we should get the hightest AMSApptControl record
        ''            AMSAppointment = (
        ''            From d In db.AMSAppointments
        ''            Where
        ''            d.AMSApptCompControl = FKControl
        ''            Order By d.AMSApptControl Descending
        ''            Select selectDTOData(d, db)).FirstOrDefault
        ''        End If
        ''        If Not AMSAppointment Is Nothing Then
        ''            Dim osum = GetAMSAppointmentSummaryByControl(AMSAppointment.AMSApptControl)
        ''            AMSAppointment.AMSApptLabel = osum.AMSApptLabel
        ''            AMSAppointment.AMSApptHover = osum.AMSApptHover
        ''        End If
        ''        Return AMSAppointment
        ''    Catch ex As System.Data.SqlClient.SqlException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        ''    Catch ex As InvalidOperationException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        ''    End Try
        ''    Return Nothing
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' CALLED BY GetAMSAppointmentSummaryForMonth() IN AMS.cs
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <returns></returns>
    Public Function GetAMSAppointmentSummaryForMonth(ByVal CompControl As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As DTO.AMSAppointment()
        throwDepreciatedException(buildProcedureName("GetAMSAppointmentSummaryForMonth"))
        Return Nothing
        ''Using db As New NGLMASAMSDataContext(ConnectionString)
        ''    Try
        ''        Dim AMSAppointments() As DTO.AMSAppointment = (
        ''        From d In db.spGetApptSummaryForMonth(CompControl, StartDate, EndDate)
        ''        Select New DTO.AMSAppointment With {.AMSApptControl = 0 _
        ''                                              , .AMSApptCompControl = CompControl _
        ''                                              , .AMSApptStartDate = If(d.AMSApptDate.HasValue, d.AMSApptDate.Value.AddMinutes(1), d.AMSApptDate) _
        ''                                              , .AMSApptEndDate = If(d.AMSApptDate.HasValue, d.AMSApptDate.Value.AddMinutes(1439), d.AMSApptDate) _
        ''                                              , .AMSApptLabel = d.AMSApptLabel _
        ''                                              , .AMSApptHover = d.AMSApptHover}).ToArray()
        ''        Return AMSAppointments
        ''    Catch ex As System.Data.SqlClient.SqlException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        ''    Catch ex As InvalidOperationException
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        ''    Catch ex As Exception
        ''        Utilities.SaveAppError(ex.Message, Me.Parameters)
        ''        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        ''    End Try
        ''    Return Nothing
        ''End Using
    End Function

    ''' <summary>
    ''' DEPRECIATED - Old interface via WCF no longer supported throws 
    ''' </summary>
    ''' <param name="Orders"></param>
    ''' <param name="Appointment"></param>
    ''' <param name="createOrUpdate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by RHR on 09/19/2018 to allow WCF service to build.
    ''' </remarks>
    Public Function CreateOrUpdateAppointment(ByVal Orders As DTO.AMSOrderList(), ByVal Appointment As DTO.AMSAppointment, ByVal createOrUpdate As Boolean) As DTO.AMSAppointmentResult
        throwDepreciatedException("This inteface to the Scheduler is no longer supported,  an AMSValidation  object is required.")
        Return Nothing
    End Function

    ''' <summary>
    ''' DEPRECIATED -  CALLED BY AddOrdersToAppointment() IN AMS.cs
    ''' </summary>
    ''' <param name="Orders"></param>
    ''' <param name="Appointment"></param>
    ''' <returns></returns>
    Public Function AddOrdersToAppointment(ByVal Orders As DTO.AMSOrderList(), ByVal Appointment As DTO.AMSAppointment) As DTO.AMSOrderList()
        throwDepreciatedException("This inteface to the Scheduler is no longer supported,  an AMSValidation  object is required.")
        Return Nothing
    End Function

#End Region

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetAMSAppointmentFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    ''' <summary>
    ''' CALLED BY GetAMSAppoinment() IN AMS.cs
    ''' Note: We could make this faster by putting this all in a sp
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV On 6/12/18 For v-8.3 TMS365 Scheduler
    '''  Added AMSAppointment.DockDoorName to return object
    ''' </remarks>
    Public Function GetAMSAppointmentFiltered(ByVal Control As Integer) As DTO.AMSAppointment
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim AMSAppointment As DTO.AMSAppointment = (
                From d In db.AMSAppointments
                Where
                    d.AMSApptControl = Control
                Select selectDTOData(d, db)).FirstOrDefault()

                If AMSAppointment Is Nothing Then Return Nothing

                Dim dockName = (From t In db.AMSRefCompDockDoors Where t.CompDockCompControl = AMSAppointment.AMSApptCompControl And t.DockDoorID = AMSAppointment.AMSApptDockdoorID Select t.DockDoorName).FirstOrDefault()
                If String.IsNullOrWhiteSpace(dockName) Then dockName = AMSAppointment.AMSApptDockdoorID
                AMSAppointment.DockDoorName = dockName

                If Not AMSAppointment Is Nothing Then
                    Dim osum = GetAMSAppointmentSummaryByControl(db, AMSAppointment.AMSApptControl)
                    AMSAppointment.AMSApptLabel = osum.AMSApptLabel
                    AMSAppointment.AMSApptHover = osum.AMSApptHover
                End If
                Return AMSAppointment
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' CALLED BY GetScheduledAppointments() IN AMS.cs 
    ''' Alternately, you can call GetAMSApptsFilteredOptimized() below because it does the same thing as this method but 
    ''' without the DTO overhead because it returns the LTS object directly
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    Public Function GetAMSAppointmentsFiltered(ByVal CompControl As Integer,
                                               ByVal StartDate As Date,
                                               ByVal EndDate As Date,
                                               Optional ByVal page As Integer = 1,
                                               Optional ByVal pagesize As Integer = 1000) As DTO.AMSAppointment()

        Return GetAMSAppointmentsFilteredOptimized(CompControl,
                                                StartDate,
                                                EndDate)

    End Function

    ''' <summary>
    ''' OLD - GetAMSAppointmentsFilteredOptimized
    ''' This method now calls the new method GetAMSApptsFilteredOptimized. Old code that calls this method should still work
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 5/9/18 for v-8.3 TMS365 Scheduler
    '''  This method now calls the new method GetAMSApptsFilteredOptimized. Old code that calls this method should still work
    ''' Modified By LVV on 5/30/18 for v-8.3 TMS365 Scheduler
    '''  Added field CompAMSColorCodeSettingColorCode 
    ''' </remarks>
    Public Function GetAMSAppointmentsFilteredOptimized(ByVal CompControl As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As DTO.AMSAppointment()
        Dim res = GetAMSApptsFilteredOptimized(CompControl, StartDate, EndDate)
        Dim AMSAppointments As New List(Of DTO.AMSAppointment)
        For Each d In res
            Dim appt As New DTO.AMSAppointment
            With appt
                .AMSApptControl = d.AMSApptControl
                .AMSApptCompControl = d.AMSApptCompControl
                .AMSApptCarrierControl = d.AMSApptCarrierControl
                .AMSApptCarrierSCAC = d.AMSApptCarrierSCAC
                .AMSApptCarrierName = d.AMSApptCarrierName
                .AMSApptDescription = d.AMSApptDescription
                .AMSApptStartDate = d.AMSApptStartDate
                .AMSApptEndDate = d.AMSApptEndDate
                .AMSApptTimeZone = d.AMSApptTimeZone
                .AMSApptRecurrenceParentControl = d.AMSApptRecurrenceParentControl
                .AMSApptRecurrence = d.AMSApptRecurrence
                .AMSApptActualDateTime = d.AMSApptActualDateTime
                .AMSApptStartLoadingDateTime = d.AMSApptStartLoadingDateTime
                .AMSApptFinishLoadingDateTime = d.AMSApptFinishLoadingDateTime
                .AMSApptActLoadCompleteDateTime = d.AMSApptActLoadCompleteDateTime
                .AMSApptModDate = d.AMSApptModDate
                .AMSApptModUser = d.AMSApptModUser
                .AMSApptNotes = d.AMSApptNotes
                .AMSApptDockdoorID = d.AMSApptDockdoorID
                .AMSApptStatusCode = d.AMSApptStatusCode
                .AMSApptLabel = d.AMSApptLabel
                .AMSApptHover = d.AMSApptHover
                .AMSApptOrderCount = d.AMSApptOrderCount
                .CompAMSColorCodeSettingColorCode = d.CompAMSColorCodeSettingColorCode.Trim() 'Added By LVV on 5/30/18 for v-8.3 TMS365 Scheduler
                .Page = 1
                .Pages = 1
                .AMSApptUpdated = d.AMSApptUpdated.ToArray()
            End With
            AMSAppointments.Add(appt)
        Next
        Return AMSAppointments.ToArray()
    End Function

    ''' <summary>
    ''' ** UPDATED METHODS (called by old code - backwards compatible) **
    ''' Does the same thing as GetAMSAppointmentsFilteredOptimized (and by extension GetAMSAppointmentsFiltered which calls it)
    ''' but without the DTO overhead. Returns the sp results lts object directly
    ''' Note: Remember if you are going to use the Updated field you have to create a C# Model in the client to handle it
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 5/9/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 5/30/18 for v-8.3 TMS365 Scheduler
    '''  Added field CompAMSColorCodeSettingColorCode to return object
    '''  CompAMSColorCodeSettingColorCode can be NULL and a null check must be
    '''  handled on the client side (as far as any defaults may be concerned)
    ''' Modified By LVV on 7/10/18 for v-8.3 TMS365 Scheduler
    '''  Added ability to turn on/off visibility of fields in hover over And label
    ''' </remarks>
    Public Function GetAMSApptsFilteredOptimized(ByVal CompControl As Integer, ByVal StartDate As Date, ByVal EndDate As Date) As LTS.spGetAMSAppointmentWithDetailsResult()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim FormatedStartDate = DTran.formatStartDateFilter(StartDate)
                Dim FormatedEndDate = DTran.formatEndDateFilter(EndDate)

                Dim ApptNumberOn = True
                Dim CarrierOn = True
                Dim DockNameOn = True
                Dim CNSNumbersOn = True
                Dim OrderNumbersOn = True
                Dim ProNumbersOn = True
                Dim PONumbersOn = True
                Dim ApptNotesOn = True
                Dim oComp As New NGLCompData(Parameters)
                oComp.GetCompAMSApptDetailFieldsVisibility(CompControl, ApptNumberOn, CarrierOn, DockNameOn, CNSNumbersOn, OrderNumbersOn, ProNumbersOn, PONumbersOn, ApptNotesOn)

                Return (From d In db.spGetAMSAppointmentWithDetails(CompControl, FormatedStartDate, FormatedEndDate, ApptNumberOn, CarrierOn, DockNameOn, CNSNumbersOn, OrderNumbersOn, ProNumbersOn, PONumbersOn, ApptNotesOn) Select d).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSApptsFilteredOptimized"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' OLD - GetAMSAppointmentSummaryByControl
    ''' This method now calls the new method GetAMSApptSummaryByControl. Old code that calls this method should still work
    ''' </summary>
    ''' <param name="AMSApptControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 5/9/18 for v-8.3 TMS365 Scheduler
    '''  This method now calls the new method GetAMSApptSummaryByControl. Old code that calls this method should still work
    ''' </remarks>
    Public Function GetAMSAppointmentSummaryByControl(ByRef db As NGLMASAMSDataContext, ByVal AMSApptControl As Integer) As DTO.AMSApptSummary
        Dim spRes = GetAMSApptSummaryByControl(db, AMSApptControl)
        Dim ApptSummary As New DTO.AMSApptSummary()
        With ApptSummary
            .AMSApptHover = spRes.AMSApptHover
            .AMSApptLabel = spRes.AMSApptLabel
            .AMSApptOrderCount = spRes.AMSApptOrderCount
        End With
        Return ApptSummary
    End Function

    ''' <summary>
    ''' ** UPDATED METHODS (called by old code - backwards compatible) **
    ''' Does the same thing as GetAMSAppointmentSummaryByControl but without the DTO overhead
    ''' Returns the sp results lts object directly
    ''' </summary>
    ''' <param name="AMSApptControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 5/9/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 7/10/18 for v-8.3 TMS365 Scheduler
    '''  Added ability to turn on/off visibility of fields in hover over and label
    ''' </remarks>
    Public Function GetAMSApptSummaryByControl(ByRef db As NGLMASAMSDataContext, ByVal AMSApptControl As Integer) As LTS.spGetApptSummaryByControlResult
        Try
            Dim compControl = db.AMSAppointments.Where(Function(x) x.AMSApptControl = AMSApptControl).Select(Function(y) y.AMSApptCompControl).FirstOrDefault()

            Dim ApptNumberOn = True
            Dim CarrierOn = True
            Dim DockNameOn = True
            Dim CNSNumbersOn = True
            Dim OrderNumbersOn = True
            Dim ProNumbersOn = True
            Dim PONumbersOn = True
            Dim ApptNotesOn = True

            Dim oComp As New NGLCompData(Parameters)
            oComp.GetCompAMSApptDetailFieldsVisibility(compControl, ApptNumberOn, CarrierOn, DockNameOn, CNSNumbersOn, OrderNumbersOn, ProNumbersOn, PONumbersOn, ApptNotesOn)

            Return (From d In db.spGetApptSummaryByControl(AMSApptControl, ApptNumberOn, CarrierOn, DockNameOn, CNSNumbersOn, OrderNumbersOn, ProNumbersOn, PONumbersOn, ApptNotesOn) Select d).FirstOrDefault()

        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetAMSApptSummaryByControl"), db)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' CALLED BY GetAMSOrderList(), GetAMSOrdersByCarrier(), GetAMSOrdersByOrderNumber(), GetAMSOrdersByPO(), GetAMSOrdersByCNS(), GetAMSOrdersByPRO() IN AMS.cs
    ''' This method returns orders that can be scheduled for appointments based on the provided filters
    ''' CompControl is required. The view vAMSOrderLists pre-filters the data 
    ''' where
    ''' BookTranCode not in ('N','P','PC')
    ''' And
    ''' (BookAMSPickupApptControl = 0 
    ''' OR 
    ''' BookAMSDeliveryApptControl = 0)
    ''' If the BookConsPrefix is NULL or EMPTY then we set it to 9999
    ''' the maximum nunber or records returned is 1000
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="ConsPrefix"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="PRONumber"></param>
    ''' <param name="PONumber"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v- 8.2.0.118 on 9/9/2019
    '''   changed defautl carriercontrol filter to allow for a carrier filter as a string we now 
    '''   support looking up the carrier name or the carrier number and find 
    '''   the first match
    '''Modified by RHR for v-8.4.0.003 on 07/22/2021 added new IgnoreDateOnSchedulerFilters parameter option
    ''' </remarks>
    Public Function GetAMSOrders(ByVal CompControl As Integer,
                                 ByVal StartDate As System.Nullable(Of Date),
                                 ByVal EndDate As System.Nullable(Of Date),
                                 Optional ByVal ConsPrefix As String = Nothing,
                                 Optional ByVal OrderNumber As String = Nothing,
                                 Optional ByVal PRONumber As String = Nothing,
                                 Optional ByVal PONumber As String = Nothing,
                                 Optional ByVal sCarrier As String = "",
                                 Optional ByVal SHID As String = Nothing,
                                 Optional ByVal CarrierControl As Integer = 0) As DTO.AMSOrderList()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                db.Log = New DebugTextWriter
                Dim dblIgnoreDateFilterPar = GetParValue("IgnoreDateOnSchedulerFilters", CompControl)
                Dim blnIgnoreDateFilter As Boolean = False
                If dblIgnoreDateFilterPar = 1 _
                    AndAlso (Not String.IsNullOrWhiteSpace(ConsPrefix) _
                    Or Not String.IsNullOrWhiteSpace(OrderNumber) _
                    Or Not String.IsNullOrWhiteSpace(PRONumber) _
                    Or Not String.IsNullOrWhiteSpace(PONumber) _
                    Or Not String.IsNullOrWhiteSpace(SHID)) Then
                    blnIgnoreDateFilter = True
                End If
                If EndDate.HasValue Then EndDate = DTran.formatEndDateFilter(EndDate)
                If StartDate.HasValue Then StartDate = DTran.formatStartDateFilter(StartDate)


                Dim iCarrierControl As Integer = 0
                If CarrierControl <> 0 Then
                    iCarrierControl = CarrierControl
                Else
                    'check if we have a carrier filter
                    If Not String.IsNullOrWhiteSpace(sCarrier) Then
                        If db.CarrierRefAMS.Any(Function(x) x.CarrierName.Contains(sCarrier)) Then
                            iCarrierControl = db.CarrierRefAMS.Where(Function(x) x.CarrierName.Contains(sCarrier)).OrderBy(Function(o) o.CarrierName).Select(Function(y) y.CarrierControl).FirstOrDefault()
                        Else
                            Dim CarrierNumber As Integer = 0
                            'check if we have a valid carrier number
                            Integer.TryParse(sCarrier, CarrierNumber)
                            If CarrierNumber > 0 Then
                                iCarrierControl = db.CarrierRefAMS.Where(Function(x) x.CarrierNumber = CarrierNumber).Select(Function(y) y.CarrierControl).FirstOrDefault()
                            End If
                        End If
                    End If
                End If
                Dim AMSOrders As List(Of DTO.AMSOrderList)
                If blnIgnoreDateFilter Then
                    AMSOrders = (
                       From d In db.vAMSOrderLists
                       Where
                       (
                           (
                               d.BookOrigCompControl = CompControl _
                               And
                               d.BookAMSPickupApptControl = 0
                              ) Or (
                               d.BookDestCompControl = CompControl _
                               And
                               d.BookAMSDeliveryApptControl = 0
                           )
                       ) _
                       And
                       (String.IsNullOrEmpty(ConsPrefix) OrElse d.BookConsPrefix = ConsPrefix) _
                       And
                       (String.IsNullOrEmpty(OrderNumber) OrElse d.BookCarrOrderNumber = OrderNumber) _
                       And
                       (String.IsNullOrEmpty(PRONumber) OrElse d.BookProNumber = PRONumber) _
                       And
                       (String.IsNullOrEmpty(PONumber) OrElse d.BookLoadPONumber = PONumber) _
                       And
                       (iCarrierControl = 0 OrElse d.BookCarrierControl = iCarrierControl) _
                       And
                       (String.IsNullOrEmpty(SHID) OrElse d.BookSHID = SHID)
                       Select selectAMSOrderListDTOData(d, db, CompControl)).Take(1000).ToList
                Else
                    AMSOrders = (
                   From d In db.vAMSOrderLists
                   Where
                   (
                       (
                           d.BookOrigCompControl = CompControl _
                           And
                           d.BookAMSPickupApptControl = 0 _
                           And
                           (
                               (
                                   Not StartDate.HasValue And Not EndDate.HasValue
                               ) Or (
                                   StartDate.HasValue _
                                   And
                                   EndDate.HasValue _
                                   And
                                   (If(d.BookDateLoad.HasValue, d.BookDateLoad >= StartDate And d.BookDateLoad <= EndDate, False))
                               )
                           )
                       ) Or (
                           d.BookDestCompControl = CompControl _
                           And
                           d.BookAMSDeliveryApptControl = 0 _
                           And
                           (
                               (
                                   Not StartDate.HasValue And Not EndDate.HasValue
                               ) Or (
                                   StartDate.HasValue _
                                   And
                                   EndDate.HasValue _
                                   And
                                   (If(d.BookDateRequired.HasValue, d.BookDateRequired >= StartDate And d.BookDateRequired <= EndDate, False))
                               )
                           )
                       )
                   ) _
                   And
                   (String.IsNullOrEmpty(ConsPrefix) OrElse d.BookConsPrefix = ConsPrefix) _
                   And
                   (String.IsNullOrEmpty(OrderNumber) OrElse d.BookCarrOrderNumber = OrderNumber) _
                   And
                   (String.IsNullOrEmpty(PRONumber) OrElse d.BookProNumber = PRONumber) _
                   And
                   (String.IsNullOrEmpty(PONumber) OrElse d.BookLoadPONumber = PONumber) _
                   And
                   (iCarrierControl = 0 OrElse d.BookCarrierControl = iCarrierControl) _
                   And
                   (String.IsNullOrEmpty(SHID) OrElse d.BookSHID = SHID)
                   Select selectAMSOrderListDTOData(d, db, CompControl)).Take(1000).ToList
                End If


                If Not AMSOrders Is Nothing AndAlso AMSOrders.Count > 0 Then
                    'if we are filtering by date we need to get any consolidated orders that may have a different pickup or delivery date by mistake
                    If EndDate.HasValue And StartDate.HasValue Then
                        Dim oBookControls = (From a In AMSOrders Where a.BookConsPrefix <> "9999" Select a.BookControl).ToList
                        'we create a seperate list AMSOrdersToAdd because the for each enumerator will fail if we add order to the list inside the loop
                        Dim AMSOrdersToAdd As New List(Of DTO.AMSOrderList)
                        'loop through each order where a consolidation number has been assigned and consolidation integrity is on
                        'the view converts Null or empty bookconsprefix values to 9999

                        Dim oConsOrders = (From a In AMSOrders Where a.BookConsPrefix <> "9999" AndAlso a.BookRouteConsFlag = True Select a.BookConsPrefix).ToList
                        'Get the booking records
                        Dim ConsolidatedAMSOrders As List(Of DTO.AMSOrderList) = (
                              From d In db.vAMSOrderLists
                              Where
                              (
                                  (d.BookOrigCompControl = CompControl AndAlso d.BookAMSPickupApptControl = 0) _
                                  Or
                                  (d.BookDestCompControl = CompControl AndAlso d.BookAMSDeliveryApptControl = 0)
                              ) _
                              And
                              (oConsOrders.Contains(d.BookConsPrefix) AndAlso d.BookRouteConsFlag = True AndAlso Not oBookControls.Contains(d.BookControl))
                              Select selectAMSOrderListDTOData(d, db, CompControl)).Take(1000).ToList
                        If Not ConsolidatedAMSOrders Is Nothing AndAlso ConsolidatedAMSOrders.Count > 0 Then
                            For Each a In ConsolidatedAMSOrders
                                AMSOrders.Add(a)
                            Next
                        End If

                        'For Each o In AMSOrders.Where(Function(x) (x.BookConsPrefix <> "9999" AndAlso x.BookRouteConsFlag = True)).ToList
                        '    'we only get orders that do not have appointments assigned that match the current cns and that have not already been selected
                        '    Dim oConsPrefix = o.BookConsPrefix
                        '    Dim ConsolidatedAMSOrders As List(Of DTO.AMSOrderList) = ( _
                        '       From d In db.vAMSOrderLists _
                        '       Where _
                        '       ( _
                        '           (d.BookOrigCompControl = CompControl AndAlso d.BookAMSPickupApptControl = 0) _
                        '           Or _
                        '           (d.BookDestCompControl = CompControl AndAlso d.BookAMSDeliveryApptControl = 0) _
                        '       ) _
                        '       And _
                        '       (d.BookConsPrefix = oConsPrefix AndAlso d.BookRouteConsFlag = True AndAlso Not oBookControls.Contains(d.BookControl)) _
                        '       Select selectAMSOrderListDTOData(d, db, CompControl)).Take(1000).ToList
                        '    'if any records are found we add them to the temporary list(s)
                        '    If Not ConsolidatedAMSOrders Is Nothing AndAlso ConsolidatedAMSOrders.Count > 0 Then
                        '        For Each c In ConsolidatedAMSOrders
                        '            If Not oBookControls.Contains(c.BookControl) Then
                        '                oBookControls.Add(c.BookControl)
                        '                AMSOrdersToAdd.Add(c)
                        '            End If
                        '        Next
                        '    End If
                        'Next
                        'finally we can add the new orders to the main list
                        'If Not AMSOrdersToAdd Is Nothing AndAlso AMSOrdersToAdd.Count > 0 Then
                        '    For Each a In AMSOrdersToAdd
                        '        AMSOrders.Add(a)
                        '    Next
                        'End If
                    End If
                    Return AMSOrders.OrderBy(Function(x) x.OrderType).ThenBy(Function(x) x.BookConsPrefix).ThenByDescending(Function(x) x.BookRouteConsFlag).ToArray
                End If

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' code change PFM 8/6/2013 - This is a new method, a rewrite of the save method in NEXTrack.
    ''' Moved the save method to here, wcf for perfomance enhancements.
    ''' This combines the AddOrdersToAppointment and the NEXTrack save logic.
    ''' Called By REST AMSAppointmentController.SaveUpdateAppointmentOrders() - Warehouse Scheduler
    ''' </summary>
    ''' <param name="Orders"></param>
    ''' <param name="Appointment"></param>
    ''' <param name="createOrUpdate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 12/6/16 for v-7.0.5.110 NEXTrack Cabot Enhancement
    ''' Added call to sp to create Notes if param GlobalAddVisibleNotesToAppointment turned on
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  fixed bug where query to get dock is not valid with only the DockDoorID
    '''  because DockDoorID is not unique, requires CompControl
    '''  to correct this issue the functions, OverrideDockSettings and ValidateDockSettingsForAppt, 
    '''  were modified to accept a DTO.AMSAppointment object as a parmeter instead of some of the data
    '''  We now throw an InvalidKeyParentRequiredException if the Appointment, 
    '''  the Dock, the start date, or the end dates are not valid
    '''  Added new logic to call ManageLinqDataExceptions on exceptions
    ''' Modified by RHR for v-8.3.0.001 on 09/27/2020
    '''     added logic to read dock door and test for validation on or off before calling ValidateDockSettingsForAppt
    '''     used to assist with new logic to auto stack appointments on the same time slot based on 
    '''     new parameter AppointmenstStackWhenValidationIsOff
    ''' Modified by RHR for v-8.3.0.001 on 10/08/2020
    '''   fixed bug where updates were not being saved correctly when order details were missing (the update always reads dependent bookings)
    ''' </remarks>
    Public Function CreateOrUpdateAppointment(ByRef oValidation As Models.AMSValidation, ByVal Orders As DTO.AMSOrderList(), ByVal Appointment As DTO.AMSAppointment, ByVal createOrUpdate As Boolean) As DTO.AMSAppointmentResult
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'Validation here
                Dim refDockControl As Integer = 0
                Dim blnInsertLog As Boolean = False
                Dim aptControl As Integer = Appointment.AMSApptControl
                Dim compControl As Integer = 0
                Dim newStart As Date
                Dim newEnd As Date
                Dim newDockID As String = ""
                Dim ogStart As Date
                Dim ogEnd As Date
                Dim ogDockID As String = ""
                If Appointment Is Nothing OrElse Appointment.AMSApptCompControl = 0 Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "Record"})
                End If
                If Not Appointment.AMSApptStartDate.HasValue Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "Start Date"})
                End If
                If Not Appointment.AMSApptEndDate.HasValue Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "End Date"})
                End If

                ' Begin  Modified by RHR for v-8.3.0.001 on 09/27/2020
                Dim dock = NGLCompDockDoorObjData.GetCompDockDoorForAppt(Appointment)
                If dock Is Nothing OrElse dock.CompDockContol = 0 Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "Dock"})
                End If
                If Not String.IsNullOrWhiteSpace(oValidation.BitString) Then
                    'Attempting to perform an Override
                    'Modified by RHR for v-8.2.1 on 10/16/2019 
                    OverrideDockSettings(db, oValidation, Appointment, blnInsertLog, refDockControl)
                Else
                    If dock.CompDockValidation Then
                        'We need to run the Validation
                        Dim blnRunAll As Boolean = False
                        Dim blnDateChanged As Boolean = False
                        Dim blnTimeChanged As Boolean = False
                        'If this is an Insert, or if this is an Update and the Dock has changed - we need to run all validation
                        If createOrUpdate Then
                            blnRunAll = True
                        Else
                            Dim dbAppt = GetAppointment(db, Appointment.AMSApptControl)
                            'Modified by RHR for v-8.2.1 on 10/16/2019 
                            ' Added test for Null Reference Exception,  The appointment does not exist
                            ' This should not happen but we handle the error in case someone else 
                            If dbAppt Is Nothing OrElse dbAppt.AMSApptControl = 0 Then
                                throwNoDataFaultMessage("the selected appointment cannot be found", False)
                            End If
                            If dbAppt.AMSApptDockdoorID <> Appointment.AMSApptDockdoorID Then
                                blnRunAll = True 'The Dock has changed so we have to run all validation
                            Else
                                'Check if the day changed
                                If dbAppt.AMSApptStartDate.Value.Date <> Appointment.AMSApptStartDate.Value.Date Then blnDateChanged = True
                                'Check if the time changed
                                If (dbAppt.AMSApptStartDate.Value.TimeOfDay <> Appointment.AMSApptStartDate.Value.TimeOfDay) OrElse (dbAppt.AMSApptEndDate.Value.TimeOfDay <> Appointment.AMSApptEndDate.Value.TimeOfDay) Then blnTimeChanged = True
                            End If
                        End If
                        'Modified by RHR for v-8.2.1 on 10/16/2019 
                        'Modified by RHR for v-8.3.0.001 on 09/27/2020
                        oValidation = NGLDockSettingObjData.ValidateDockSettingsForAppt(False, blnRunAll, blnDateChanged, blnTimeChanged, Orders.Select(Function(x) x.BookControl).ToArray(), Appointment, dock)
                    Else
                        ' set the default values for validation
                        Dim strValidation = "000000000"
                        oValidation = NGLDockSettingObjData.parseValidationString(strValidation)
                    End If
                End If
                ' End  Modified by RHR for v-8.3.0.001 on 09/27/2020
                'We only run the save if validation passes -- If the validation is false then return
                If oValidation.Success Then
                    'first create the appointment if does not exist.
                    If createOrUpdate Then
                        ' when createOrUpdate is true we are inserting a new appointment.
                        'new logic created by RHR for v-8.3.0.001 on 09/27/2020 
                        '1. check if validation is on or off
                        '2. check is AppointmenstStackWhenValidationIsOff is on or off
                        '3. check if we have dropped this on an existing appointment.
                        '4. if not try to look up a match
                        '5. if no match create a new appointment
                        Dim AppointmenstStackWhenValidationIsOff As Double = GetParValue("AppointmenstStackWhenValidationIsOff", Appointment.AMSApptCompControl)
                        Dim blnCreateNew As Boolean = True
                        If (Not dock.CompDockValidation) And AppointmenstStackWhenValidationIsOff = 1 Then
                            'If Appointment.AMSApptControl <> 0 Then
                            '    'we stack when dropped on another appointment
                            '    blnCreateNew = False
                            'End If
                            If Appointment.AMSApptControl = 0 Then
                                Dim oExisting = db.AMSAppointments.Where(Function(x) x.AMSApptDockdoorID = Appointment.AMSApptDockdoorID And x.AMSApptCompControl = Appointment.AMSApptCompControl And x.AMSApptStartDate = Appointment.AMSApptStartDate).ToArray()
                                If Not oExisting Is Nothing AndAlso oExisting.Count > 0 Then
                                    For Each e In oExisting
                                        If (e.AMSApptCarrierControl <> 0 AndAlso e.AMSApptCarrierControl = Appointment.AMSApptCarrierControl) _
                                            OrElse (e.AMSApptCarrierSCAC = Appointment.AMSApptCarrierSCAC And e.AMSApptCarrierName = Appointment.AMSApptCarrierName) Then
                                            Appointment.AMSApptControl = e.AMSApptControl
                                            blnCreateNew = False
                                            Exit For
                                        End If
                                    Next
                                End If

                            Else
                                blnCreateNew = False
                            End If
                        End If
                        If blnCreateNew Then
                            Appointment = CreateRecord(Appointment)
                        End If
                        'Get the times from the appointment
                        ogStart = Appointment.AMSApptStartDate.Value
                        ogEnd = Appointment.AMSApptEndDate.Value
                        ogDockID = Appointment.AMSApptDockdoorID
                    Else
                        'Get the new times from the method parameter
                        newStart = Appointment.AMSApptStartDate.Value
                        newEnd = Appointment.AMSApptEndDate.Value
                        newDockID = Appointment.AMSApptDockdoorID
                        'Get the original times for the notifications below
                        Dim ogAppt = db.AMSAppointments.Where(Function(x) x.AMSApptControl = aptControl).FirstOrDefault()
                        ogStart = ogAppt.AMSApptStartDate.Value
                        ogEnd = ogAppt.AMSApptEndDate.Value
                        ogDockID = ogAppt.AMSApptDockdoorID
                    End If
                    If Appointment Is Nothing Then Return Nothing
                    aptControl = Appointment.AMSApptControl
                    compControl = Appointment.AMSApptCompControl

                    'Insert the override log records after we get an appointment control
                    If blnInsertLog = True Then
                        InsertAMSApptOverrideLogAsync(oValidation.BitString, Appointment.AMSApptControl, refDockControl, oValidation.ReasonCode)
                        'InsertAMSApptOverrideLog(oValidation.BitString, Appointment.AMSApptControl, refDockControl, oValidation.ReasonCode)
                    End If

                    Dim lUpdated As New List(Of DTO.AMSOrderList)
                    If Orders Is Nothing OrElse Orders.Count > 0 Then
                        For Each o In Orders
                            If Not o Is Nothing Then
                                Dim oUpdated As DTO.AMSOrderList = UpdateAMSBooking(db, o, Appointment)
                                If Not oUpdated Is Nothing Then lUpdated.Add(oUpdated)
                            End If
                        Next
                    End If


                    ' Modified by RHR for v-8.3.0.001 on 10/08/2020
                    'If createOrUpdate = False Or (Orders Is Nothing OrElse Orders.Count > 0) Then 'we must re-save the appointment data to 
                    ' If createOrUpdate = False AndAlso (Orders Is Nothing OrElse Orders.Count > 0) Then 'we must re-save the appointment data to 
                    If createOrUpdate = False Then 'we must re-save the appointment data to 
                        ''force an update to all booking orders with correct dates and times etc...
                        Appointment.TrackingState = TrackingInfo.Updated
                        Appointment = UpdateRecord(Appointment)
                    Else
                        UpdateAMSBookings(Appointment.AMSApptControl)
                    End If
                    Dim result As New DTO.AMSAppointmentResult
                    result.Appointment = Appointment
                    result.AppointmentOrders = lUpdated.ToArray

                    'Modified By LVV on 12/6/16 for v-7.0.5.110 NEXTrack Cabot Enhancement
                    'createOrUpdate is true on insert it is just named weird
                    Dim sParams As New Models.SchedMessagingParams With {.ApptControl = aptControl, .CompControl = compControl, .ogDockID = ogDockID, .ogApptStart = ogStart, .ogApptEnd = ogEnd}
                    If createOrUpdate Then
                        'call new sp here and pass in apptcontrol
                        GetApptNotesByControl(db, Appointment.AMSApptControl)
                        'call getappt with apptcontrol to return the updated appt info and set result.appt = to this
                        result.Appointment = GetAMSAppointmentFiltered(Appointment.AMSApptControl)

                        'Send Carrier Booked Notification                    
                        NGLDockSettingObjData.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.WHBooked, sParams)
                    Else
                        If ogStart.Date <> newStart.Date OrElse ogStart.TimeOfDay <> newStart.TimeOfDay OrElse ogEnd.TimeOfDay <> newEnd.TimeOfDay OrElse ogDockID <> newDockID Then
                            'Send Carrier Modified Notification                   
                            NGLDockSettingObjData.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.WHModify, sParams)
                        End If
                    End If

                    Return result
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CreateOrUpdateAppointment"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Called By REST AMSOrderController.AddOrdersToAppointment() - Warehouse Scheduler
    ''' </summary>
    ''' <param name="oValidation"></param>
    ''' <param name="Orders"></param>
    ''' <param name="Appointment"></param>
    ''' <returns></returns>
    Public Function AddOrdersToAppointment(ByRef oValidation As Models.AMSValidation, ByVal Orders As DTO.AMSOrderList(), ByVal Appointment As DTO.AMSAppointment) As DTO.AMSOrderList()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Return AddOrdersToAppointment(db, oValidation, Orders, Appointment)
        End Using
    End Function

    ''' <summary>
    ''' Add additional order to an existing appointment
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="oValidation"></param>
    ''' <param name="Orders"></param>
    ''' <param name="Appointment"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  fixed bug where query to get dock is not valid with only the DockDoorID
    '''  because DockDoorID is not unique, requires CompControl
    '''  to correct this issue the functions, OverrideDockSettings and ValidateDockSettingsForAppt, 
    '''  were modified to accept a DTO.AMSAppointment object as a parmeter instead of some of the data
    '''  We now throw an InvalidKeyParentRequiredException if the Appointment, 
    '''  the Dock, the start date, or the end dates are not valid
    '''  Added new logic to call ManageLinqDataExceptions on exceptions
    ''' </remarks>
    Public Function AddOrdersToAppointment(ByRef db As NGLMASAMSDataContext, ByRef oValidation As Models.AMSValidation, ByVal Orders As DTO.AMSOrderList(), ByVal Appointment As DTO.AMSAppointment) As DTO.AMSOrderList()
        Try
            Dim lUpdated As New List(Of DTO.AMSOrderList)
            If Orders Is Nothing OrElse Orders.Count < 1 Then Return Nothing

            'Validation
            Dim oDockSet As New NGLDockSettingData(Parameters)
            Dim refDockControl As Integer = 0
            If Not String.IsNullOrWhiteSpace(oValidation.BitString) Then
                'Attempting to perform an Override
                Dim blnInsertLog As Boolean = False
                'Modified by RHR for v-8.2.1 on 10/16/2019
                OverrideDockSettings(db, oValidation, Appointment, blnInsertLog, refDockControl)
                If blnInsertLog = True Then InsertAMSApptOverrideLogAsync(oValidation.BitString, Appointment.AMSApptControl, refDockControl, oValidation.ReasonCode)
            Else
                'When calling from this method blnEditOrdersOnly must be True, the other boolean flags won't get checked so just set them to false
                Dim arrOrders = Orders.Select(Function(x) x.BookControl).ToArray()
                If Appointment.AMSApptStartDate.HasValue AndAlso Appointment.AMSApptEndDate.HasValue Then
                    'Modified by RHR for v-8.2.1 on 10/16/2019 
                    oValidation = oDockSet.ValidateDockSettingsForAppt(True, False, False, False, arrOrders, Appointment)
                Else
                    oValidation.Success = True
                End If
            End If

            If oValidation.Success Then
                'Bug fix v-8.5.3 07/07/2022 variable scope
                Dim oUpdated As DTO.AMSOrderList = Nothing
                For Each o In Orders
                    If Not o Is Nothing Then
                        oUpdated = Nothing
                        If o.BookControl = 0 Then
                            oUpdated = CreateAddHockOrder(o, Appointment)
                        ElseIf o.BookControl < 0 Then
                            oUpdated = UpdateAddHockOrder(o, Appointment)
                        Else
                            oUpdated = UpdateBook(o, Appointment)
                        End If
                        If Not oUpdated Is Nothing Then lUpdated.Add(oUpdated)
                    End If
                Next
                Return lUpdated.ToArray
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("AddOrdersToAppointment"))
        End Try
        Return Nothing
    End Function


    '***** CALLED BY GetAMSOrdersByAppointment() IN AMS.cs *****
    Public Function GetAMSOrdersByAppointment(ByVal AMSApptControl As Integer) As DTO.AMSOrderList()
        Try
            Dim lOrders As New List(Of DTO.AMSOrderList)
            Dim lSubOrders As List(Of DTO.AMSOrderList)
            'get any booking records
            lSubOrders = GetAMSBookRecordsByAppointment(AMSApptControl)
            If Not lSubOrders Is Nothing AndAlso lSubOrders.Count > 0 Then
                For Each b In lSubOrders
                    lOrders.Add(b)
                Next
            End If
            'get any adhoc records
            lSubOrders = GetAMSBookAdhocRecordsByAppointment(AMSApptControl)
            If Not lSubOrders Is Nothing AndAlso lSubOrders.Count > 0 Then
                For Each b In lSubOrders
                    lOrders.Add(b)
                Next
            End If
            Return lOrders.ToArray
        Catch ex As FaultException(Of SqlFaultInfo)
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
    End Function

    '***** CALLED BY RemoveBookingByAppointment() IN AMS.cs *****
    Public Function RemoveAMSBooking(ByVal AMSApptControl As Integer, ByVal BookControl As Integer) As Boolean
        'spRemoveAMSBooking
        Dim strBatchName As String = "RemoveAMSBooking"
        Dim strProcName As String = "dbo.spRemoveAMSBooking"
        'Validate the parameter data
        If AMSApptControl = 0 Then Return False
        If BookControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@AMSApptControl", AMSApptControl)
        oCmd.Parameters.AddWithValue("@BookControl", BookControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)

        Return True
    End Function

    ''' <summary>
    ''' Creates the notes for the appt if the param GlobalAddVisibleNotesToAppointment is turned on
    ''' </summary>
    ''' <param name="ApptControl"></param>
    ''' <remarks>
    ''' Added By LVV on 12/6/16 for v-7.0.5.110 NEXTrack Cabot Enhancement
    ''' </remarks>
    Public Sub GetApptNotesByControl(ByVal ApptControl As Integer)
        Using db As New NGLMASAMSDataContext(ConnectionString)
            GetApptNotesByControl(db, ApptControl)
        End Using
    End Sub

    ''' <summary>
    ''' Creates the notes for the appt if the param GlobalAddVisibleNotesToAppointment is turned on
    ''' </summary>
    ''' <param name="ApptControl"></param>
    ''' <remarks>
    ''' Added By LVV on 12/6/16 for v-7.0.5.110 NEXTrack Cabot Enhancement
    ''' </remarks>
    Public Sub GetApptNotesByControl(ByRef db As NGLMASAMSDataContext, ByVal ApptControl As Integer)
        Try
            'db.Log = New DebugTextWriter
            db.spGetApptNotesByControl(ApptControl)

        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
    End Sub


    ''' <summary>
    ''' TMS 365 - Called By AMSAppointmentController.GetRecordsByAppointmentFilter() -- Warehouse Scheduler Search Appointment Grid, also Add/Edit window for adding orders
    ''' NEXTrackCode - Called By FindAMSApptByOrderNumber(), FindAMSApptByOrderPro(), FindAMSApptByOrderPO() in AMS.cs
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="Inbound"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="PRONumber"></param>
    ''' <param name="PONumber"></param>
    ''' <param name="CNSNumber"></param>
    ''' <param name="SHID"></param>
    ''' <returns></returns>
    Public Function FindAMSApptByOrder(ByVal CompControl As Integer,
                                       ByVal Inbound As Boolean,
                                       Optional ByVal OrderNumber As String = Nothing,
                                       Optional ByVal PRONumber As String = Nothing,
                                       Optional ByVal PONumber As String = Nothing,
                                       Optional ByVal CNSNumber As String = Nothing,
                                       Optional ByVal SHID As String = Nothing) As DTO.AMSAppointment

        Dim intAMSApptControl As Integer = 0
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                Dim AMSResults = db.spFindAMSApptControlByOrder(CompControl, Inbound, OrderNumber, PRONumber, PONumber, CNSNumber, SHID)

                If Not AMSResults Is Nothing Then
                    For Each a In AMSResults
                        If a.AMSApptControl <> 0 Then
                            intAMSApptControl = a.AMSApptControl
                            Exit For
                        End If
                    Next
                End If

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
        If intAMSApptControl <> 0 Then
            Return GetAMSAppointmentFiltered(intAMSApptControl)
        Else
            Return Nothing
        End If
    End Function

    'Modified By LVV On 7/19/18 For v-8.3 TMS365 Scheduler -- Added fields OrderType and OrderColorCode and BookCarrTrailerNo
    Friend Function selectAMSOrderListDTOData(ByVal d As LTS.vAMSOrderList, ByRef db As NGLMASAMSDataContext, ByVal CompControl As Integer, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.AMSOrderList
        Return New DTO.AMSOrderList With {.BookControl = d.BookControl _
                                                     , .BookCustCompControl = d.BookCustCompControl _
                                                     , .BookProNumber = d.BookProNumber _
                                                    , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                                                    , .BookConsPrefix = d.BookConsPrefix _
                                                    , .AMSCompControl = CompControl _
                                                    , .BookCarrierControl = d.BookCarrierControl _
                                                    , .BookCarrierContact = d.BookCarrierContact _
                                                    , .BookCarrierContactPhone = d.BookCarrierContactPhone _
                                                    , .BookOrigCompControl = d.BookOrigCompControl _
                                                    , .BookOrigName = d.BookOrigName _
                                                    , .BookOrigAddress1 = d.BookOrigAddress1 _
                                                    , .BookOrigCity = d.BookOrigCity _
                                                    , .BookOrigState = d.BookOrigState _
                                                    , .BookOrigCountry = d.BookOrigCountry _
                                                    , .BookOrigZip = d.BookOrigZip _
                                                    , .BookOrigPhone = d.BookOrigPhone _
                                                    , .BookDestCompControl = d.BookDestCompControl _
                                                    , .BookDestName = d.BookDestName _
                                                    , .BookDestAddress1 = d.BookDestAddress1 _
                                                    , .BookDestCity = d.BookDestCity _
                                                    , .BookDestState = d.BookDestState _
                                                    , .BookDestCountry = d.BookDestCountry _
                                                    , .BookDestZip = d.BookDestZip _
                                                    , .BookDestPhone = d.BookDestPhone _
                                                    , .BookDateOrdered = d.BookDateOrdered _
                                                    , .BookDateLoad = d.BookDateLoad _
                                                    , .BookDateRequired = d.BookDateRequired _
                                                    , .BookTotalCases = d.BookTotalCases _
                                                    , .BookTotalWgt = d.BookTotalWgt _
                                                    , .BookTotalPL = d.BookTotalPL _
                                                    , .BookTotalCube = d.BookTotalCube _
                                                    , .BookTotalPX = d.BookTotalPX _
                                                    , .BookStopNo = d.BookStopNo _
                                                    , .BookRouteConsFlag = d.BookRouteConsFlag _
                                                    , .BookOrderSequence = d.BookOrderSequence _
                                                    , .BookShipCarrierProNumber = d.BookShipCarrierProNumber _
                                                    , .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw _
                                                    , .BookShipCarrierProControl = d.BookShipCarrierProControl _
                                                    , .BookShipCarrierName = d.BookShipCarrierName _
                                                    , .BookShipCarrierNumber = d.BookShipCarrierNumber _
                                                    , .BookODControl = d.BookODControl _
                                                    , .CarrierNumber = If(d.CarrierNumber.HasValue, d.CarrierNumber.Value, 0) _
                                                    , .CarrierName = d.CarrierName _
                                                    , .CarrierSCAC = d.CarrierSCAC _
                                                    , .LaneNumber = d.LaneNumber _
                                                    , .LaneOriginAddressUse = d.LaneOriginAddressUse _
                                                    , .BookLoadPONumber = d.BookLoadPONumber _
                                                    , .BookLoadControl = If(d.BookLoadControl.HasValue, d.BookLoadControl.Value, 0) _
                                                    , .BookItemDetailDescription = d.BookItemDetailDescription _
                                                    , .BookAMSPickupApptControl = d.BookAMSPickupApptControl _
                                                    , .BookAMSDeliveryApptControl = d.BookAMSDeliveryApptControl _
                                                    , .OrderType = d.OrderType _
                                                    , .OrderColorCode = d.OrderColorCode _
                                                    , .EquipmentID = d.BookCarrTrailerNo _
                                                    , .BookSHID = d.BookSHID _
                                                    , .Page = page _
                                                    , .Pages = pagecount _
                                                    , .RecordCount = recordcount _
                                                    , .PageSize = pagesize}
    End Function


#Region "TMS 365"

    ''***************************** NEW METHODS *****************************

    ''' <summary>
    ''' Deletes a record in AMSAppointment.
    ''' Then calls DeleteAMSBookings()
    ''' </summary>
    ''' <param name="AMSApptControl"></param>
    ''' <remarks>
    ''' Added By LVV on 5/7/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub DeleteAMSAppointment(ByVal AMSApptControl As Integer, ByVal blnCarrierDeleted As Boolean)
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim oDockSet As New NGLDockSettingData(Parameters)

                Dim r As LTS.AMSAppointment = db.AMSAppointments.Where(Function(x) x.AMSApptControl = AMSApptControl).FirstOrDefault()
                If (r Is Nothing OrElse r.AMSApptControl = 0) Then Return

                'Get the messaging info using the AppointmentControl before it is deleted
                Dim spDets = oDockSet.AMSGetDetailsForMessaging(AMSApptControl)
                Dim sParams As New Models.SchedMessagingParams With {.ApptControl = AMSApptControl, .CompControl = r.AMSApptCompControl, .spDets = spDets}

                db.AMSAppointments.DeleteOnSubmit(r)
                db.SubmitChanges()

                'Delete CleanUp
                If Not r Is Nothing Then DeleteAMSBookings(AMSApptControl)

                'Send Alerts/CarrierConfirmation/CarrierNotification
                If blnCarrierDeleted Then
                    'Send Subscription Alert                    
                    oDockSet.SendSchedulerAlertAsync(NGLDockSettingData.AMSMsg.CarrierDelete, sParams)
                    'Send Carrier Delete Confirmation
                    oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.DeleteConfirm, sParams)
                Else
                    'Send Carrier Delete Notification
                    oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.WHDelete, sParams)
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteAMSAppointment"), db)
            End Try
        End Using
    End Sub

    Public Function GetAppointment(ByVal AMSApptControl As Integer) As LTS.AMSAppointment
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Return GetAppointment(db, AMSApptControl)
        End Using
    End Function

    Public Function GetAppointment(ByRef db As NGLMASAMSDataContext, ByVal AMSApptControl As Integer) As LTS.AMSAppointment
        Try
            Return db.AMSAppointments.Where(Function(x) x.AMSApptControl = AMSApptControl).FirstOrDefault()
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("GetAppointment"), db)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Method called when the user clicks the "Override" button from the Scheduler.
    ''' Checks to see if a valid Password/Reason Code were entered if required
    ''' Note: If there are 3 records for invalid password in the log table for this
    ''' user on this date then the user is locked out for 24 hours 
    ''' Note: This means they cannot override any rules that require a password for 24 hours.
    ''' This is to prevent brute force password attacks. The user is not prevented
    ''' from overriding rules via Reason Code or prevented from scheduling appointments
    ''' that pass validation
    ''' </summary>
    ''' <param name="oValidation"></param>
    ''' <param name="Appointment"></param>
    ''' <param name="blnInsertLog"></param>
    ''' <param name="DockControl"></param>
    ''' <remarks>
    ''' Added By LVV on 8/3/18 for v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  fixed bug where query to get dock is not valid with only the DockDoorID
    '''  because DockDoorID is not unique, requires CompControl
    '''  to correct this issue the function was modified to accept 
    '''  a DTO.AMSAppointment object as a parmeter instead of some of the data
    '''  We now throw an InvalidKeyParentRequiredException if the Appointment,  
    '''  the Dock or start and end dates are not valid
    ''' </remarks>
    Public Sub OverrideDockSettings(ByRef db As NGLMASAMSDataContext, ByRef oValidation As Models.AMSValidation, ByRef Appointment As DTO.AMSAppointment, ByRef blnInsertLog As Boolean, ByRef DockControl As Integer)
        Try
            Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
            If Appointment Is Nothing OrElse Appointment.AMSApptCompControl = 0 Then
                throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "Record"})
            End If
            If Not Appointment.AMSApptStartDate.HasValue Then
                throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "Start Date"})
            End If
            If Not Appointment.AMSApptEndDate.HasValue Then
                throwInvalidKeyParentRequiredException(New List(Of String) From {"Appointment", "End Date"})
            End If
            Dim ApptControl As Integer = Appointment.AMSApptControl
            Dim DockDoorID As String = Appointment.AMSApptDockdoorID
            Dim StartDate As Date = Appointment.AMSApptStartDate.Value()
            Dim EndDate As Date = Appointment.AMSApptEndDate.Value()
            Dim CompControl As Integer = Appointment.AMSApptCompControl

            If oValidation.NoOverride = False Then 'Double check
                'Modified by RHR for v-8.2.1 on 10/16/2019 added CompControl filter
                Dim dock = db.AMSRefCompDockDoors.Where(Function(x) x.DockDoorID = DockDoorID And x.CompDockCompControl = CompControl).FirstOrDefault()
                If dock Is Nothing OrElse dock.CompDockContol = 0 Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "Dock"})
                End If
                DockControl = dock.CompDockContol
                blnInsertLog = False

                'Process Password
                If oValidation.SPRequired = True AndAlso Not String.IsNullOrWhiteSpace(oValidation.Input) Then
                    'check the logs to see if there are 3 records that say password failed
                    If db.AMSApptOverrideLogs.Where(Function(x) x.AMSOLInvalidPwd = True AndAlso x.AMSOLUserControl = Parameters.UserControl AndAlso x.AMSOLCompDockContol = dock.CompDockContol AndAlso x.AMSOLModDate.Value.Date = Date.Now.Date).Count() >= 3 Then
                        'Too many attempts - locked out for 24 hours
                        oValidation.Success = False
                        oValidation.NoOverride = True
                        oValidation.InvalidSP = True
                        oValidation.FailedMsg = oLocalize.GetLocalizedValueByKey("PwdFailLockout24Hr", "Account temporarily locked out because of too many failed password attempts. Try again tomorrow.")
                        oValidation.Input = "" 'clear the password
                        Return
                    Else
                        'Check password
                        If Not String.Equals(oValidation.Input, DTran.Decrypt(dock.CompDockOverridePwd, "NGL")) Then
                            'Invalid password so create a record in the log and return the fail result
                            Dim strMsgPassFail = oLocalize.GetLocalizedValueByKey("InvalidPassword", "Invalid Password")
                            oValidation.Success = False
                            oValidation.NoOverride = False
                            oValidation.InvalidSP = True
                            oValidation.FailedMsg = strMsgPassFail
                            oValidation.Input = "" 'clear the password

                            Dim log As New LTS.AMSApptOverrideLog
                            With log
                                .AMSOLApptControl = ApptControl
                                .AMSOLUserControl = Parameters.UserControl
                                .AMSOLCompDockContol = DockControl
                                .AMSOLInvalidPwd = True
                                .AMSOLRequiredPwd = True
                                .AMSOLMessage = strMsgPassFail
                                .AMSOLModDate = Date.Now
                                .AMSOLModUser = Parameters.UserName
                            End With
                            db.AMSApptOverrideLogs.InsertOnSubmit(log)
                            db.SubmitChanges()
                            Return
                        Else
                            'Password matched
                            oValidation.Success = True
                            oValidation.Input = "" 'clear the password
                            blnInsertLog = True
                        End If
                    End If
                End If

                'Process Reason Code
                If oValidation.RCRequired = True Then
                    If String.IsNullOrWhiteSpace(oValidation.ReasonCode) Then
                        oValidation.InvalidRC = True
                        oValidation.Success = False
                        oValidation.FailedMsg = oLocalize.GetLocalizedValueByKey("ReasonCodeRequired", "Reason Code Required")
                        oValidation.NoOverride = False
                    Else
                        oValidation.Success = True
                        blnInsertLog = True
                    End If
                End If

            Else
                'Make sure Success Flag is false (NoOverride = True)
                oValidation.Success = False
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("OverrideDockSettings"), db)
        End Try
    End Sub

    'Added By LVV on 7/31/18 for v-8.3 TMS365 Scheduler
    Public Delegate Sub InsertAMSApptOverrideLogDelegate(ByVal strValidation As String, ByVal AMSApptControl As Integer, ByVal DockControl As Integer, ByVal ReasonCode As String)

    ''' <summary>
    ''' Asynchronous call to the method that inserts a record into the AMSApptOverrideLog log table
    ''' </summary>
    ''' <param name="strValidation"></param>
    ''' <param name="AMSApptControl"></param>
    ''' <param name="DockControl"></param>
    ''' <param name="ReasonCode"></param>
    ''' <remarks>
    ''' Added By LVV on 7/31/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub InsertAMSApptOverrideLogAsync(ByVal strValidation As String, ByVal AMSApptControl As Integer, ByVal DockControl As Integer, ByVal ReasonCode As String)
        Dim fetcher As New InsertAMSApptOverrideLogDelegate(AddressOf Me.InsertAMSApptOverrideLog)
        ' Launch thread
        fetcher.BeginInvoke(strValidation, AMSApptControl, DockControl, ReasonCode, Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Inserts a record into the AMSApptOverrideLog log table
    ''' </summary>
    ''' <param name="strValidation"></param>
    ''' <param name="AMSApptControl"></param>
    ''' <param name="DockControl"></param>
    ''' <param name="ReasonCode"></param>
    ''' <remarks>
    ''' Added By LVV on 7/31/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Private Sub InsertAMSApptOverrideLog(ByVal strValidation As String, ByVal AMSApptControl As Integer, ByVal DockControl As Integer, ByVal ReasonCode As String)
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                Dim strMsg As String = ""

                For i = 0 To strValidation.Length - 1
                    If strValidation(i) <> "0" AndAlso strValidation(i) <> "1" Then
                        Select Case i
                            Case 0 'DockHours
                                strMsg = oLocalize.GetLocalizedValueByKey("SchedDockHourValFail", "The appointment is outside of the Resource Hours")
                                db.spInsertAMSApptOverrideLog(AMSApptControl, Parameters.UserControl, DockControl, strMsg, strValidation(i), ReasonCode, Parameters.UserName)
                            Case 1 'MaxAppts
                                strMsg = oLocalize.GetLocalizedValueByKey("SchedMaxApptsValFail", "The maximum allowed appointments per day have been met")
                                db.spInsertAMSApptOverrideLog(AMSApptControl, Parameters.UserControl, DockControl, strMsg, strValidation(i), ReasonCode, Parameters.UserName)
                            Case 2 'TempType
                                strMsg = oLocalize.GetLocalizedValueByKey("SchedTempTypeValFail", "An order contains one or more unsupported Temperature Codes")
                                db.spInsertAMSApptOverrideLog(AMSApptControl, Parameters.UserControl, DockControl, strMsg, strValidation(i), ReasonCode, Parameters.UserName)
                            Case 3 'PackageType
                                strMsg = oLocalize.GetLocalizedValueByKey("SchedPackTypeValFail", "An order contains one or more unsupported Package Types")
                                db.spInsertAMSApptOverrideLog(AMSApptControl, Parameters.UserControl, DockControl, strMsg, strValidation(i), ReasonCode, Parameters.UserName)
                            Case 4 'DoubleBooking
                                strMsg = oLocalize.GetLocalizedValueByKey("SchedDoubleBookValFail", "Double Booking is not allowed")
                                db.spInsertAMSApptOverrideLog(AMSApptControl, Parameters.UserControl, DockControl, strMsg, strValidation(i), ReasonCode, Parameters.UserName)
                            Case 5 'BlockOutPeriod
                                strMsg = oLocalize.GetLocalizedValueByKey("SchedBlockOutValFail", "The Appointment overlaps a Block Out Period")
                                db.spInsertAMSApptOverrideLog(AMSApptControl, Parameters.UserControl, DockControl, strMsg, strValidation(i), ReasonCode, Parameters.UserName)
                            Case 6 'MinTime
                                strMsg = oLocalize.GetLocalizedValueByKey("SchedMinLengthValFail", "The Appointment is less than the minimum required minutes")
                                db.spInsertAMSApptOverrideLog(AMSApptControl, Parameters.UserControl, DockControl, strMsg, strValidation(i), ReasonCode, Parameters.UserName)
                            Case 7 'MaxTime
                                strMsg = oLocalize.GetLocalizedValueByKey("SchedMaxLengthValFail", "The Appointment is greater than the maximum allowed minutes")
                                db.spInsertAMSApptOverrideLog(AMSApptControl, Parameters.UserControl, DockControl, strMsg, strValidation(i), ReasonCode, Parameters.UserName)
                            Case Else
                                'do nothing
                        End Select
                    End If
                    strMsg = ""
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertAMSApptOverrideLog"), db)
            End Try
        End Using
    End Sub

    Public Function GetvAMSApptOverrideLogFailedAttempts(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vAMSApptOverrideLogFailedAttempt()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vAMSApptOverrideLogFailedAttempt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSApptOverrideLogFailedAttempt)
                iQuery = db.vAMSApptOverrideLogFailedAttempts
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvAMSApptOverrideLogFailedAttempts"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function ResetAMSOverrideLogFailedAttempts(ByVal oData As LTS.vAMSApptOverrideLogFailedAttempt) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim oRecords As LTS.AMSApptOverrideLog() = db.AMSApptOverrideLogs.Where(Function(x) x.AMSOLInvalidPwd = True AndAlso x.AMSOLUserControl = oData.USC AndAlso x.AMSOLCompDockContol = oData.DockControl AndAlso x.AMSOLModDate.Value.Date = Date.Now.Date).ToArray()
                For Each r In oRecords
                    If (r Is Nothing OrElse r.AMSOLControl = 0) Then Continue For
                    db.AMSApptOverrideLogs.DeleteOnSubmit(r)
                Next
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ResetAMSOverrideLogFailedAttempts"), db)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Gets all pick up orders for the Carrier that still need to be scheduled
    ''' If there are no user filters, only show orders where LoadDate is after yesterday (aka today or later)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/31/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 10/2/18 for v-8.3 TMS365 Scheduler
    '''  Added logic to only show orders where LoadDate is after yesterday (aka today or later)
    ''' </remarks>
    Public Function GetAMSCarrierPickNeedAppt(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vAMSCarrierPickNeedAppt()
        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.vAMSCarrierPickNeedAppt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSCarrierPickNeedAppt)
                iQuery = db.vAMSCarrierPickNeedAppts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    filterWhere = " (BookTranCode = ""PB"") And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                    If Parameters.UserCarrierContControl <> 0 Then
                        filterWhere &= " And (BookCarrierContControl = " & Parameters.UserCarrierContControl & ") "
                    End If
                Else
                    Return oRet 'we only show records for carriers when Allow Web Tender is true
                End If

                sFilterSpacer = " And "

                'If there Then are no user filters, only show orders where LoadDate is after yesterday (aka today or later)
                If filters.FilterValues?.Count < 1 OrElse Not filters.FilterValues.Any(Function(x) x.filterName.Trim().Length > 0) Then
                    Dim y = Date.Now
                    Dim DaysOutDate = New Date(y.Year, y.Month, y.Day, 0, 0, 0)
                    filterWhere &= sFilterSpacer & " (BookDateLoad >= DateTime.Parse(""" + DaysOutDate + """)) "
                    sFilterSpacer = " And "

                End If

                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookDateLoad"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierPickNeedAppt"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Gets all delivery orders for the Carrier that still need to be scheduled
    ''' If there are no user filters, only show orders where Required date is after 5 days before today (go back 5 days)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/31/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 10/2/18 for v-8.3 TMS365 Scheduler
    '''  Added logic to only show orders where Required date is after 5 days before today (go back 5 days)
    ''' </remarks>
    Public Function GetAMSCarrierDelNeedAppt(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vAMSCarrierDelNeedAppt()
        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.vAMSCarrierDelNeedAppt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSCarrierDelNeedAppt)
                iQuery = db.vAMSCarrierDelNeedAppts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    filterWhere = " (BookTranCode = ""PB"") And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                    If Parameters.UserCarrierContControl <> 0 Then
                        filterWhere &= " And (BookCarrierContControl = " & Parameters.UserCarrierContControl & ") "
                    End If
                Else
                    Return oRet 'we only show records for carriers when Allow Web Tender is true
                End If

                sFilterSpacer = " And "

                'If there Then are no user filters, only show orders where Required Date Is after 5 days before today (go back 5 days)
                If filters.FilterValues?.Count < 1 OrElse Not filters.FilterValues.Any(Function(x) x.filterName.Trim().Length > 0) Then
                    Dim y = Date.Now.AddDays(-5)
                    Dim DaysOutDate = New Date(y.Year, y.Month, y.Day, 0, 0, 0)
                    filterWhere &= sFilterSpacer & " (BookDateRequired >= DateTime.Parse(""" + DaysOutDate + """)) "
                    'sFilterSpacer = " And "

                End If
                If String.IsNullOrWhiteSpace(filters.sortName) Then
                    filters.sortName = "BookDateRequired"
                    filters.sortDirection = "asc"
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierDelNeedAppt"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Gets all pick up orders for the Carrier that have appointments scheduled
    ''' If there are no user filters, only show orders where sched date is after yesterday (aka today or later)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/31/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 10/2/18 for v-8.3 TMS365 Scheduler
    '''  Added logic to only show orders where sched date is after yesterday (aka today or later)
    ''' </remarks>
    Public Function GetAMSCarrierPickBookedAppt(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vAMSCarrierPickBookedAppt()
        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.vAMSCarrierPickBookedAppt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSCarrierPickBookedAppt)
                iQuery = db.vAMSCarrierPickBookedAppts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    filterWhere = " (BookTranCode = ""PB"") And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                    If Parameters.UserCarrierContControl <> 0 Then
                        filterWhere &= " And (BookCarrierContControl = " & Parameters.UserCarrierContControl & ") "
                    End If
                Else
                    Return oRet 'we only show records for carriers when Allow Web Tender is true
                End If

                sFilterSpacer = " And "

                'If there Then are no user filters, only show orders where sched Date Is after yesterday (aka today Or later)
                If filters.FilterValues?.Count < 1 OrElse Not filters.FilterValues.Any(Function(x) x.filterName.Trim().Length > 0) Then
                    Dim y = Date.Now
                    Dim DaysOutDate = New Date(y.Year, y.Month, y.Day, 0, 0, 0)
                    filterWhere &= sFilterSpacer & " (ScheduledDate >= DateTime.Parse(""" + DaysOutDate + """)) "

                End If
                If filters.SortValues?.Count < 1 OrElse Not filters.SortValues.Any(Function(x) x.sortName.Trim().Length > 0) Then
                    Dim a As New Models.SortDetails With {.sortName = "ScheduledDate", .sortDirection = "asc"}
                    Dim b As New Models.SortDetails With {.sortName = "ScheduledTime", .sortDirection = "asc"}
                    Dim sortArray As Models.SortDetails() = {a, b}
                    filters.SortValues = sortArray
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierPickBookedAppt"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Gets all delivery orders for the Carrier that have appointments scheduled
    ''' If there are no user filters, only show orders where sched date is after yesterday (aka today or later)
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 7/31/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 10/2/18 for v-8.3 TMS365 Scheduler
    '''  Added logic to only show orders where sched date is after yesterday (aka today or later)
    ''' </remarks>
    Public Function GetAMSCarrierDelBookedAppt(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vAMSCarrierDelBookedAppt()
        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.vAMSCarrierDelBookedAppt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSCarrierDelBookedAppt)
                iQuery = db.vAMSCarrierDelBookedAppts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    filterWhere = " (BookTranCode = ""PB"") And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                    If Parameters.UserCarrierContControl <> 0 Then
                        filterWhere &= " And (BookCarrierContControl = " & Parameters.UserCarrierContControl & ") "
                    End If
                Else
                    Return oRet 'we only show records for carriers when Allow Web Tender is true
                End If

                sFilterSpacer = " And "

                'If there Then are no user filters, only show orders where sched Date Is after yesterday (aka today Or later)
                If filters.FilterValues?.Count < 1 OrElse Not filters.FilterValues.Any(Function(x) x.filterName.Trim().Length > 0) Then
                    Dim y = Date.Now
                    Dim DaysOutDate = New Date(y.Year, y.Month, y.Day, 0, 0, 0)
                    filterWhere &= sFilterSpacer & " (ScheduledDate >= DateTime.Parse(""" + DaysOutDate + """)) "

                End If
                If filters.SortValues?.Count < 1 OrElse Not filters.SortValues.Any(Function(x) x.sortName.Trim().Length > 0) Then
                    Dim a As New Models.SortDetails With {.sortName = "ScheduledDate", .sortDirection = "asc"}
                    Dim b As New Models.SortDetails With {.sortName = "ScheduledTime", .sortDirection = "asc"}
                    Dim sortArray As Models.SortDetails() = {a, b}
                    filters.SortValues = sortArray
                End If
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                db.Log = New DebugTextWriter
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierDelBookedAppt"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' Do not use, Depreciated,  Instead call overloaded GetApptTimesOnDay that uses DockControl Number.
    ''' </summary>
    ''' <param name="DockDoorID"></param>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/10/18 for v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  fixed bug where query to get dock is not valid with only the DockDoorID
    ''' </remarks>
    Public Function GetApptTimesOnDay(ByVal DockDoorID As String, ByVal dt As Date) As List(Of Models.TimeSlot)
        throwDepreciatedException("Do not Use GetApptTimesOnDay with DockDoorID: Instead call overloaded GetApptTimesOnDay that uses DockControl Number.")
        'Using db As New NGLMASAMSDataContext(ConnectionString)
        '    Try
        '        Dim oRet As New List(Of Models.TimeSlot)
        '        oRet = (From d In db.AMSAppointments
        '                Where d.AMSApptDockdoorID = DockDoorID And d.AMSApptStartDate.Value.Date = dt.Date
        '                Select New Models.TimeSlot With {.Start = d.AMSApptStartDate, .End = d.AMSApptEndDate}).ToList()
        '        Return oRet
        '    Catch ex As Exception
        '        ManageLinqDataExceptions(ex, buildProcedureName("GetApptTimesOnDay"))
        '    End Try
        '    Return Nothing
        'End Using
    End Function

    ''' <summary>
    ''' Returns a list of timeslots in the appt table for the specified dock 
    ''' </summary>
    ''' <param name="DockControl"></param>
    ''' <param name="dt"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.1 on 10/16/2019
    ''' </remarks>
    Public Function GetApptTimesOnDay(ByVal DockControl As Integer, ByVal dt As Date) As List(Of Models.TimeSlot)
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim oRet As New List(Of Models.TimeSlot)
                Dim dock = db.AMSRefCompDockDoors.Where(Function(x) x.CompDockContol = DockControl).FirstOrDefault()
                If dock Is Nothing OrElse dock.CompDockCompControl = 0 Then
                    Return oRet
                End If
                Dim DockDoorID = dock.DockDoorID
                Dim CompControl = dock.CompDockCompControl
                oRet = (From d In db.AMSAppointments
                        Where d.AMSApptDockdoorID = DockDoorID And d.AMSApptCompControl = CompControl And (d.AMSApptStartDate.HasValue = True And d.AMSApptStartDate.Value.Date = dt.Date)
                        Select New Models.TimeSlot With {.Start = d.AMSApptStartDate, .End = d.AMSApptEndDate}).ToList()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetApptTimesOnDay"))
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' * DEPRECIATED *
    ''' Attempts to update the appointment dock/times at the available resources listed in Docks (Docks is listed in order of precedence for bookings)
    ''' On Success returns an empty string
    ''' On Fail returns an error message (or an exception if one occured)
    ''' Notes:
    ''' Docks is a comma separated string listing the DockDoorIDs in order of precedence for bookings
    ''' All of these parameters are returned by the method GetCarrierAvailableAppointments() and
    ''' passed back to this method in order to change the booking
    ''' </summary>
    ''' <param name="ts"></param>
    ''' <param name="blnCarrierAlg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/19/18 for v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  fixed bug where query to Check if the time is still available at this dock fails
    '''  because DockDoorID is not unique, requires CompControl
    '''  to correct this issue the function was modified to read the CompControl
    '''  from the Models.AMSCarrierAvailableSlots object 
    '''  We now throw an InvalidKeyParentRequiredException if the Models.AMSCarrierAvailableSlots is nothing
    '''  or if the Models.AMSCarrierAvailableSlots.CompControl is zero
    ''' Depreciated By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  Calls to this method are now replaced by calls to either CarrierAutomationUpdateExistingAppointment() or WarehouseUpdateExistingAppointmentWithSuggestion()
    '''  This is is to fix concurrency issues as well as improve code readability and maintainability
    '''  (The process flow on the scheduler Is a mess And I am slowly trying to fix it)
    ''' </remarks>
    Public Function UpdateCarrierBookedAppointment(ByVal ts As Models.AMSCarrierAvailableSlots, Optional ByVal blnCarrierAlg As Boolean = True) As String
        throwDepreciatedException("Calls to this method are now replaced by calls to either CarrierAutomationUpdateExistingAppointment() or WarehouseUpdateExistingAppointmentWithSuggestion()")
        Return Nothing
        'Using db As New NGLMASAMSDataContext(ConnectionString)
        '    Try
        '        Dim strRet As String = ""
        '        Dim blnApptBooked As Boolean = False
        '        Dim ogDockID As String = ""
        '        Dim ogStart As Date
        '        Dim ogEnd As Date
        '        Dim modDockID As String = ""
        '        Dim modStart As Date = ts.StartTime
        '        Dim modEnd As Date
        '        Dim dtEndTime As Date
        '        If ts Is Nothing OrElse ts.CompControl = 0 Then
        '            throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "control # (compcontrol)"})
        '        End If
        '        Dim iCompControl As Integer = ts.CompControl
        '        Dim dockIDs = ts.Docks.Split(", ")
        '        Dim arEndTimes = ts.EndTime.Split(",")
        '        For i As Integer = 0 To dockIDs.Length - 1
        '            modDockID = dockIDs(i)
        '            dtEndTime = Nothing 'clear any old values
        '            If i > (arEndTimes.Length - 1) Then Exit For
        '            If Not Date.TryParse(arEndTimes(i), dtEndTime) Then Continue For
        '            modEnd = dtEndTime
        '            'Check if the time is still available at this dock
        '            If Not db.AMSAppointments.Any(Function(x) x.AMSApptCompControl = iCompControl AndAlso x.AMSApptDockdoorID = modDockID AndAlso x.AMSApptStartDate < dtEndTime AndAlso ts.StartTime < x.AMSApptEndDate) Then
        '                'The time is still available so book it
        '                Dim record = db.AMSAppointments.Where(Function(x) x.AMSApptControl = ts.ApptControl).FirstOrDefault()
        '                If record Is Nothing Then Return Nothing
        '                'Get the original date and times and dock door
        '                ogDockID = record.AMSApptDockdoorID
        '                ogStart = record.AMSApptStartDate.Value
        '                ogEnd = record.AMSApptEndDate.Value
        '                With record
        '                    .AMSApptStartDate = ts.StartTime
        '                    .AMSApptEndDate = dtEndTime
        '                    .AMSApptDockdoorID = modDockID
        '                    .AMSApptModDate = Date.Now
        '                    .AMSApptModUser = "Carrier Appointment Generator"
        '                End With

        '                db.SubmitChanges()

        '                UpdateAMSBookings(ts.ApptControl)

        '                blnApptBooked = True
        '                Exit For
        '            Else
        '                'This time is no longer available so check the next dock on the list
        '                Continue For
        '            End If
        '        Next

        '        If Not blnApptBooked Then
        '            Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
        '            strRet = oLocalize.GetLocalizedValueByKey("SchedTimeNotAvailable", "The appointment could Not be booked because the time slot is no longer available. Please refresh your data And try again.")
        '        Else
        '            'Send Alerts/CarrierConfirmation/CarrierNotification
        '            Dim oDockSet As New NGLDockSettingData(Parameters)
        '            'If action is by Carrier then trigger Subscription Alerts and Carrier Confirmation, if by Warehouse then send Carrier Notification
        '            Dim sParams As New Models.SchedMessagingParams With {.ApptControl = ts.ApptControl, .CompControl = ts.CompControl, .ogDockID = ogDockID, .ogApptStart = ogStart, .ogApptEnd = ogEnd}
        '            If blnCarrierAlg Then
        '                'Generate Subscription Alert (Carrier modified appt via Self-Service Portal) This also sends notification emails to applicable Resources
        '                oDockSet.SendSchedulerAlertAsync(NGLDockSettingData.AMSMsg.CarrierModify, sParams)
        '                'Send Email Confirmation to Carrier that they modified the appt
        '                oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.ModifyConfirm, sParams)
        '            Else
        '                'Send Email Notification to Carrier that appt was modified by Warehouse
        '                If ogStart.Date <> modStart.Date OrElse ogStart.TimeOfDay <> modStart.TimeOfDay OrElse ogEnd.TimeOfDay <> modEnd.TimeOfDay OrElse ogDockID <> modDockID Then
        '                    oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.WHModify, sParams)
        '                End If
        '            End If
        '        End If

        '        Return strRet

        '    Catch ex As Exception
        '        ManageLinqDataExceptions(ex, buildProcedureName("UpdateCarrierBookedAppointment"))
        '    End Try
        '    Return Nothing
        'End Using
    End Function

    ''' <summary>
    ''' Called from the Carrier Scheduler Grouped Page when the user modifies an existing booked appointment from the "Booked Appointments" tab.
    ''' Attempts to book an appointment with the selected start time at the available dock with the lowest sequence number.
    ''' On Success returns an empty string.
    ''' On Fail returns an error message (or an exception if one occured).
    ''' Notes:
    ''' The Docks property of ts is ignored.
    ''' All of these parameters are returned by the method GetCarrierAvailableAppointments() and
    ''' passed back to this method in order to change the booking
    ''' </summary>
    ''' <param name="ts"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  Replaces UpdateCarrierBookedAppointment()
    '''  This is is to fix concurrency issues as well as improve code readability and maintainability
    '''  (The process flow on the scheduler is a mess and I am slowly trying to fix it)
    ''' </remarks>
    Public Function CarrierAutomationUpdateExistingAppointment(ByVal ts As Models.AMSCarrierAvailableSlots) As String
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim oDock As New NGLDockSettingData(Parameters)
                'Make sure ts is not null
                If ts Is Nothing OrElse ts.CompControl = 0 Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "control # (compcontrol)"})
                End If
                'Values passed from UI
                Dim iCompControl As Integer = ts.CompControl
                Dim iCarrierControl As Integer = Parameters.UserCarrierControl
                Dim iCarrierNumber As Integer = ts.CarrierNumber
                Dim sCarrierName As String = ts.CarrierName
                Dim sWarehouse As String = ts.Warehouse
                Dim newStartTime As Date = ts.StartTime
                Dim aptControl As Integer = ts.ApptControl
                'Variables to populate
                Dim strRet As String = ""
                Dim blnApptBooked As Boolean = False
                Dim newDockID As String = ""
                Dim newEndTime As Date
                'For messaging purposes
                Dim oldDockID As String = ""
                Dim oldStart As Date
                Dim oldEnd As Date

                'Get the BookControl
                Dim iBookControl As Integer = 0
                Dim bookControls = ts.Books.Split(", ")
                Integer.TryParse(bookControls(0), iBookControl)

                'Validate that the selected time slot is still available at a dock. Returns the DockDoorID and Appointment End Time for the Dock with the lowest Sequence Number that is still available for the selected Appointment Start Time
                If oDock.ValidateCarrierAppointmentAvailability(newDockID, newEndTime, iCarrierControl, iCompControl, iBookControl, sWarehouse, sCarrierName, iCarrierNumber, newStartTime) Then
                    'The time is still available so book it
                    Dim record = db.AMSAppointments.Where(Function(x) x.AMSApptControl = aptControl).FirstOrDefault()
                    If record Is Nothing Then Return Nothing
                    'Get the original date and times and dock door
                    oldDockID = record.AMSApptDockdoorID
                    oldStart = record.AMSApptStartDate.Value
                    oldEnd = record.AMSApptEndDate.Value
                    With record
                        .AMSApptStartDate = newStartTime
                        .AMSApptEndDate = newEndTime
                        .AMSApptDockdoorID = newDockID
                        .AMSApptModDate = Date.Now
                        .AMSApptModUser = "Carrier Appointment Generator"
                    End With
                    db.SubmitChanges()
                    UpdateAMSBookings(aptControl)
                    blnApptBooked = True
                End If

                If Not blnApptBooked Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    strRet = oLocalize.GetLocalizedValueByKey("SchedTimeNotAvailable", "The appointment could Not be booked because the time slot is no longer available. Please refresh your data And try again.")
                Else
                    'The action is done by the Carrier so trigger Subscription Alerts and Carrier Confirmation
                    Dim oDockSet As New NGLDockSettingData(Parameters)
                    Dim sParams As New Models.SchedMessagingParams With {.ApptControl = aptControl, .CompControl = ts.CompControl, .ogDockID = oldDockID, .ogApptStart = oldStart, .ogApptEnd = oldEnd}
                    'Generate Subscription Alert (Carrier modified appt via Self-Service Portal) This also sends notification emails to applicable Resources
                    oDockSet.SendSchedulerAlertAsync(NGLDockSettingData.AMSMsg.CarrierModify, sParams)
                    'Send Email Confirmation to Carrier that they modified the appt
                    oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.ModifyConfirm, sParams)
                End If
                Return strRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CarrierAutomationUpdateExistingAppointment"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Called from the Warehouse Scheduler Page when the user selects a new timeslot for an existing booked appointment using the "View Availability" wizard.
    ''' Attempts to book an appointment with the selected start time at the available dock with the lowest sequence number.
    ''' On Success returns an empty string.
    ''' On Fail returns an error message (or an exception if one occured).
    ''' Notes:
    ''' The Docks property of ts is ignored.
    ''' All of these parameters are returned by the method GetCarrierAvailableAppointments() and
    ''' passed back to this method in order to change the booking
    ''' </summary>
    ''' <param name="ts"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  Replaces UpdateCarrierBookedAppointment()
    '''  This is is to fix concurrency issues as well as improve code readability and maintainability
    '''  (The process flow on the scheduler is a mess and I am slowly trying to fix it)
    ''' </remarks>
    Public Function WarehouseUpdateExistingAppointmentWithSuggestion(ByVal ts As Models.AMSCarrierAvailableSlots) As String
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Dim strRet As String = ""
            Try
                Dim oDock As New NGLDockSettingData(Parameters)
                'Make sure ts is not null
                If ts Is Nothing OrElse ts.CompControl = 0 Then throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "control # (compcontrol)"})
                'Values passed from UI
                Dim iCompControl As Integer = ts.CompControl
                Dim iCarrierControl As Integer = ts.CarrierControl
                Dim iCarrierNumber As Integer = ts.CarrierNumber
                Dim sCarrierName As String = ts.CarrierName
                Dim sWarehouse As String = ts.Warehouse
                Dim newStartTime As Date = ts.StartTime
                Dim aptControl As Integer = ts.ApptControl
                'Default variables
                Dim blnApptBooked As Boolean = False
                'Reference variables to be populated
                Dim newDockID As String = ""
                Dim newEndTime As Date
                'For messaging purposes
                Dim oldDockID As String = ""
                Dim oldStart As Date
                Dim oldEnd As Date

                'Get the BookControl
                Dim iBookControl As Integer = 0
                Dim bookControls = ts.Books.Split(", ")
                Integer.TryParse(bookControls(0), iBookControl)

                'Validate that the selected time slot is still available at a dock. Returns the DockDoorID and Appointment End Time for the Dock with the lowest Sequence Number that is still available for the selected Appointment Start Time
                If oDock.ValidateWarehouseSuggestedAppointmentAvailability(newDockID, newEndTime, iCarrierControl, sCarrierName, iCarrierNumber, iCompControl, sWarehouse, newStartTime, iBookControl, aptControl) Then
                    'The time is still available so book it
                    Dim record = db.AMSAppointments.Where(Function(x) x.AMSApptControl = aptControl).FirstOrDefault()
                    If record Is Nothing Then Return Nothing
                    'Get the original date and times and dock door
                    oldDockID = record.AMSApptDockdoorID
                    oldStart = record.AMSApptStartDate.Value
                    oldEnd = record.AMSApptEndDate.Value
                    With record
                        .AMSApptStartDate = newStartTime
                        .AMSApptEndDate = newEndTime
                        .AMSApptDockdoorID = newDockID
                        .AMSApptModDate = Date.Now
                        .AMSApptModUser = "Carrier Appointment Generator"
                    End With
                    db.SubmitChanges()
                    UpdateAMSBookings(aptControl)
                    blnApptBooked = True
                End If

                If Not blnApptBooked Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    strRet = oLocalize.GetLocalizedValueByKey("SchedTimeNotAvailable", "The appointment could Not be booked because the time slot is no longer available. Please refresh your data And try again.")
                Else
                    'The action is by the Warehouse so send Carrier Notification
                    Dim oDockSet As New NGLDockSettingData(Parameters)
                    Dim sParams As New Models.SchedMessagingParams With {.ApptControl = aptControl, .CompControl = iCompControl, .ogDockID = oldDockID, .ogApptStart = oldStart, .ogApptEnd = oldEnd}
                    'Send Email Notification to Carrier that appt was modified by Warehouse
                    If oldStart.Date <> newStartTime.Date OrElse oldStart.TimeOfDay <> newStartTime.TimeOfDay OrElse oldEnd.TimeOfDay <> newEndTime.TimeOfDay OrElse oldDockID <> newDockID Then
                        oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.WHModify, sParams)
                    End If
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("WarehouseUpdateExistingAppointmentWithSuggestion"))
            End Try
            Return strRet
        End Using
    End Function




    ''' <summary>
    ''' * DEPRECIATED *
    ''' Called by either BookCarrierAppointment or BookWarehouseAppointment. I think this is probably so we can track where 
    ''' the algorithms are being used to create appointments for reporting purposes.
    ''' Attempts to book the appointment at the available resources listed in Docks (Docks is listed in order of precedence for bookings)
    ''' On Success returns an empty string
    ''' On Fail returns an error message (or an exception if one occured)
    ''' Notes:
    ''' Docks is a comma separated string listing the DockDoorIDs in order of precedence for bookings
    ''' Books is a comma separated string of all the BookControls included in the appointment
    ''' All of these parameters are returned by the method GetCarrierAvailableAppointments() and
    ''' passed back to this method in order to schedule the booking
    ''' </summary>
    ''' <param name="ts">The time slot to book</param>
    ''' <param name="blnCarrierAlg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/14/18 for v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.1 on 10/16/2019 
    '''  fixed bug where query to Check if the time is still available at this dock fails
    '''  because DockDoorID is not unique, requires CompControl
    '''  to correct this issue the function was modified to read the CompControl
    '''  from the Models.AMSCarrierAvailableSlots object 
    '''  We now throw an InvalidKeyParentRequiredException if the Models.AMSCarrierAvailableSlots is nothing
    '''  or if the Models.AMSCarrierAvailableSlots.CompControl is zero
    ''' Depreciated By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  Depreciated because I wrote a new method to validate appointment availability in case of concurrency issue
    '''  This method is now replaced by WarehouseCreateNewAppointmentWithSuggestion()
    '''  This is is to fix concurrency issues as well as improve code readability and maintainability
    '''  (The process flow on the scheduler Is a mess And I am slowly trying to fix it)
    ''' </remarks>
    Private Function BookSuggestedAppointment(ByVal ts As Models.AMSCarrierAvailableSlots, Optional ByVal blnCarrierAlg As Boolean = True) As String
        throwDepreciatedException("This method is now replaced by WarehouseCreateNewAppointmentWithSuggestion()")
        Return Nothing
        'Using db As New NGLMASAMSDataContext(ConnectionString)
        '    Dim strRet As String = ""
        '    Dim blnApptBooked As Boolean = False
        '    Dim modUser As String = ""
        '    If blnCarrierAlg Then modUser = "Carrier Appointment Generator" Else modUser = "Warehouse Appointment Generator"
        '    Dim dockID As String = ""
        '    Dim dtEndTime As Date
        '    Dim aptControl As Integer = 0
        '    Dim bookControl As Integer = 0
        '    If ts Is Nothing OrElse ts.CompControl = 0 Then
        '        throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "control # (compcontrol)"})
        '    End If
        '    Dim iCompControl As Integer = ts.CompControl
        '    Dim dockIDs = ts.Docks.Split(", ")
        '    Dim arEndTimes = ts.EndTime.Split(", ")
        '    For i As Integer = 0 To dockIDs.Length - 1
        '        dockID = dockIDs(i)
        '        dtEndTime = Nothing 'clear any old values
        '        If Not Date.TryParse(arEndTimes(i), dtEndTime) Then Continue For
        '        'Check if the time is still available at this dock
        '        If Not db.AMSAppointments.Any(Function(x) x.AMSApptCompControl = iCompControl AndAlso x.AMSApptDockdoorID = dockID AndAlso x.AMSApptStartDate < dtEndTime AndAlso ts.StartTime < x.AMSApptEndDate) Then
        '            'The time is still available so book it
        '            Dim Appointment As New DTO.AMSAppointment
        '            With Appointment
        '                .AMSApptCompControl = ts.CompControl
        '                .AMSApptCarrierControl = ts.CarrierControl
        '                .AMSApptCarrierSCAC = ts.CarrierNumber 'I don't know why we do this but it was like this in the old NEXTrack so I didn't change it
        '                .AMSApptCarrierName = ts.CarrierName
        '                .AMSApptStartDate = ts.StartTime
        '                .AMSApptEndDate = dtEndTime
        '                .AMSApptDockdoorID = dockID
        '                .AMSApptModDate = Date.Now
        '                .AMSApptModUser = modUser
        '            End With

        '            'Create the appointment record
        '            Appointment = CreateRecord(Appointment)
        '            If Appointment Is Nothing Then Return Nothing
        '            aptControl = Appointment.AMSApptControl

        '            'Update the Pickup/Delivery appointment controls for all the books in the appointment
        '            Dim bookControls = ts.Books.Split(", ")
        '            Integer.TryParse(bookControls(0), bookControl)
        '            For Each b In bookControls
        '                db.spUpdateAMSBookingCarrierAutomation(b, aptControl, Appointment.AMSApptCompControl)
        '            Next
        '            'Modified by RHR for v-8.2 on 09/19/2018
        '            'we do not need to save the appointment we just need to update the booking data like Scheduled Date Dock door etc...
        '            UpdateAMSBookings(aptControl)
        '            'Why is this neccessary? What is this doing exactly?
        '            'If (bookControls.Count > 1) Then 'we must re-save the appointment data to 
        '            ''force an update to all booking orders with correct dates and times etc...
        '            'Appointment.TrackingState = TrackingInfo.Updated
        '            'Appointment = UpdateRecord(Appointment)
        '            'End If

        '            'Call new sp here and pass in ApptControl
        '            db.spGetApptNotesByControl(aptControl) 'NEXTrack Cabot Enhancement

        '            blnApptBooked = True
        '            Exit For
        '        Else
        '            'This time is no longer available so check the next dock on the list
        '            Continue For
        '        End If
        '    Next

        '    If Not blnApptBooked Then
        '        Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
        '        strRet = oLocalize.GetLocalizedValueByKey("SchedTimeNotAvailable", "The appointment could Not be booked because the time slot Is no longer available. Please refresh your data And Try again.")
        '    Else
        '        'Send Alerts/CarrierConfirmation/CarrierNotification
        '        Dim oDockSet As New NGLDockSettingData(Parameters)
        '        Dim dtStart = ts.Date.Date.Add(ts.StartTime.TimeOfDay)
        '        Dim sParams As New Models.SchedMessagingParams With {.ApptControl = aptControl, .CompControl = ts.CompControl, .Warehouse = ts.Warehouse, .CarrierControl = ts.CarrierControl, .CarrierNumber = ts.CarrierNumber}
        '        'If action is by Carrier then trigger Subscription Alerts and Carrier Confirmation, if by Warehouse then send Carrier Notification
        '        If blnCarrierAlg Then
        '            'Send Subscription Alert
        '            oDockSet.SendSchedulerAlertAsync(NGLDockSettingData.AMSMsg.CarrierBooked, sParams)
        '            'Send Carrier Booked Confirmation
        '            If bookControl > 0 Then oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.BookedConfirm, sParams)
        '        Else
        '            'Send Carrier Booked Notification
        '            oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.WHBooked, sParams)
        '        End If
        '    End If
        '    Return strRet
        'End Using
    End Function

    ''' <summary>
    ''' Called from the Warehouse Scheduler Page when the user selects a timeslot for a new appointment using the "View Availability" wizard.
    ''' Attempts to book an appointment with the selected start time at the available dock with the lowest sequence number.
    ''' On Success returns an empty string
    ''' On Fail returns an error message (or an exception if one occured)
    ''' Notes:
    ''' The Docks property of ts is ignored
    ''' Books is a comma separated string of all the BookControls included in the appointment
    ''' All of these parameters are returned by the method GetCarrierAvailableAppointments() and
    ''' passed back to this method in order to schedule the booking
    ''' </summary>
    ''' <param name="ts"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  Replaces BookSuggestedAppointment()
    '''  This is is to fix concurrency issues as well as improve code readability and maintainability
    '''  (The process flow on the scheduler is a mess and I am slowly trying to fix it)
    ''' </remarks>
    Private Function WarehouseCreateNewAppointmentWithSuggestion(ByVal ts As Models.AMSCarrierAvailableSlots) As String
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Dim strRet As String = ""
            Try
                Dim oDock As New NGLDockSettingData(Parameters)
                'Make sure ts is not null
                If ts Is Nothing OrElse ts.CompControl = 0 Then throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "control # (compcontrol)"})
                'Values passed from UI
                Dim iCompControl As Integer = ts.CompControl
                Dim iCarrierControl As Integer = ts.CarrierControl
                Dim iCarrierNumber As Integer = ts.CarrierNumber
                Dim sCarrierName As String = ts.CarrierName
                Dim sWarehouse As String = ts.Warehouse
                Dim ApptReqStartTime As Date = ts.StartTime
                'Variables to populate
                Dim blnApptBooked As Boolean = False
                Dim aptControl As Integer = 0
                'Reference variables to be populated
                Dim newDockID As String = ""
                Dim dtEndTime As Date

                'Get the BookControl
                Dim iBookControl As Integer = 0
                Dim bookControls = ts.Books.Split(", ")
                Integer.TryParse(bookControls(0), iBookControl)

                'Validate that the selected time slot is still available at a dock. Returns the DockDoorID and Appointment End Time for the Dock with the lowest Sequence Number that is still available for the selected Appointment Start Time
                If oDock.ValidateWarehouseSuggestedAppointmentAvailability(newDockID, dtEndTime, iCarrierControl, sCarrierName, iCarrierNumber, iCompControl, sWarehouse, ApptReqStartTime, iBookControl, aptControl) Then
                    'The time is still available so book it
                    Dim Appointment As New DTO.AMSAppointment
                    With Appointment
                        .AMSApptCompControl = iCompControl
                        .AMSApptCarrierControl = iCarrierControl
                        .AMSApptCarrierSCAC = iCarrierNumber 'I don't know why we do this but it was like this in the old NEXTrack so I didn't change it
                        .AMSApptCarrierName = sCarrierName
                        .AMSApptStartDate = ApptReqStartTime
                        .AMSApptEndDate = dtEndTime
                        .AMSApptDockdoorID = newDockID
                        .AMSApptModDate = Date.Now
                        .AMSApptModUser = "Warehouse Appointment Generator"
                    End With

                    'Create the appointment record
                    Appointment = CreateRecord(Appointment)
                    If Appointment Is Nothing Then Return Nothing
                    aptControl = Appointment.AMSApptControl

                    'Update the Pickup/Delivery appointment controls for all the books in the appointment
                    For Each b In bookControls
                        db.spUpdateAMSBookingCarrierAutomation(b, aptControl, Appointment.AMSApptCompControl)
                    Next

                    UpdateAMSBookings(aptControl) 'Modified by RHR for v-8.2 on 09/19/2018 - we do not need to save the appointment we just need to update the booking data like Scheduled Date Dock door etc...

                    'Call new sp here and pass in ApptControl
                    db.spGetApptNotesByControl(aptControl) 'NEXTrack Cabot Enhancement

                    blnApptBooked = True
                End If

                If Not blnApptBooked Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    strRet = oLocalize.GetLocalizedValueByKey("SchedTimeNotAvailable", "The appointment could Not be booked because the time slot Is no longer available. Please refresh your data And Try again.")
                Else
                    'The action is by the Warehouse so send Carrier Notification
                    Dim oDockSet As New NGLDockSettingData(Parameters)
                    Dim dtStart = ts.Date.Date.Add(ApptReqStartTime.TimeOfDay)
                    Dim sParams As New Models.SchedMessagingParams With {.ApptControl = aptControl, .CompControl = iCompControl, .Warehouse = sWarehouse, .CarrierControl = iCarrierControl, .CarrierNumber = iCarrierNumber}
                    'Send Carrier Booked Notification
                    oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.WHBooked, sParams)
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("WarehouseCreateNewAppointmentWithSuggestion"))
            End Try
            Return strRet
        End Using
    End Function



    ''' <summary>
    ''' * DEPRECIATED *
    ''' Attempts to book the appointment at the available resources listed in Docks (Docks is listed in order of precedence for bookings)
    ''' On Success returns an empty string
    ''' On Fail returns an error message (or an exception if one occured)
    ''' Notes:
    ''' Docks is a comma separated string listing the DockDoorIDs in order of precedence for bookings
    ''' Books is a comma separated string of all the BookControls included in the appointment
    ''' All of these parameters are returned by the method GetCarrierAvailableAppointments() and
    ''' passed back to this method in order to schedule the booking
    ''' </summary>
    ''' <param name="ts">The time slot to book</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/31/18 for v-8.3 TMS365 Scheduler
    ''' Depreciated By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  Depreciated because I wrote a new method to validate appointment availability in case of concurrency issue
    '''  This method is now replaced by CarrierAutomationCreateNewAppointment()
    '''  This is is to fix concurrency issues as well as improve code readability and maintainability
    '''  (The process flow on the scheduler Is a mess And I am slowly trying to fix it)
    ''' </remarks>
    Public Function BookCarrierAppointment(ByVal ts As Models.AMSCarrierAvailableSlots) As String
        throwDepreciatedException("This method is now replaced by CarrierAutomationCreateNewAppointment() which validates appointment availability in case of concurrency issue")
        Return Nothing
        'Try
        '    ts.CarrierControl = Parameters.UserCarrierControl
        '    Return BookSuggestedAppointment(ts)
        'Catch ex As Exception
        '    ManageLinqDataExceptions(ex, buildProcedureName("BookCarrierAppointment"))
        'End Try
        'Return Nothing
    End Function

    ''' <summary>
    ''' Called from the Carrier Scheduler Grouped Page when the user books a new appointment from the "Need Appointments" tab.
    ''' Called By CarrierSchedulerGroupedController.CarrierScheduleApptForUOGrouped()
    ''' Attempts to book an appointment with the selected start time at the available dock with the lowest sequence number
    ''' On Success returns an empty string
    ''' On Fail returns an error message (or an exception if one occured)
    ''' Notes:
    ''' The Docks property of ts is ignored
    ''' Books is a comma separated string of all the BookControls included in the appointment
    ''' All of these parameters are returned by the method GetCarrierAvailableAppointments() and
    ''' passed back to this method in order to schedule the booking
    ''' </summary>
    ''' <param name="ts">The time slot to book</param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency</remarks>
    Public Function CarrierAutomationCreateNewAppointment(ByVal ts As Models.AMSCarrierAvailableSlots) As String
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim oDock As New NGLDockSettingData(Parameters)
                'Make sure ts is not null
                If ts Is Nothing OrElse ts.CompControl = 0 Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "control # (compcontrol)"})
                End If
                'Values passed from UI
                Dim iCompControl As Integer = ts.CompControl
                Dim iCarrierControl As Integer = Parameters.UserCarrierControl
                Dim iCarrierNumber As Integer = ts.CarrierNumber
                Dim sCarrierName As String = ts.CarrierName
                Dim sWarehouse As String = ts.Warehouse
                Dim ApptReqStartTime As Date = ts.StartTime
                'Variables to populate
                Dim strRet As String = ""
                Dim blnApptBooked As Boolean = False
                Dim dockID As String = ""
                Dim dtEndTime As Date
                Dim aptControl As Integer = 0

                'Get the BookControl
                Dim iBookControl As Integer = 0
                Dim bookControls = ts.Books.Split(", ")
                Integer.TryParse(bookControls(0), iBookControl)

                'Validate that the selected time slot is still available at a dock. Returns the DockDoorID and Appointment End Time for the Dock with the lowest Sequence Number that is still available for the selected Appointment Start Time
                If oDock.ValidateCarrierAppointmentAvailability(dockID, dtEndTime, iCarrierControl, iCompControl, iBookControl, sWarehouse, sCarrierName, iCarrierNumber, ApptReqStartTime) Then

                    'The time is still available so book it
                    Dim Appointment As New DTO.AMSAppointment
                    With Appointment
                        .AMSApptCompControl = iCompControl
                        .AMSApptCarrierControl = iCarrierControl
                        .AMSApptCarrierSCAC = iCarrierNumber 'I don't know why we do this but it was like this in the old NEXTrack so I didn't change it
                        .AMSApptCarrierName = sCarrierName
                        .AMSApptStartDate = ApptReqStartTime
                        .AMSApptEndDate = dtEndTime
                        .AMSApptDockdoorID = dockID
                        .AMSApptModDate = Date.Now
                        .AMSApptModUser = "Carrier Appointment Generator"
                    End With

                    'Create the appointment record
                    Appointment = CreateRecord(Appointment)
                    If Appointment Is Nothing Then Return Nothing
                    aptControl = Appointment.AMSApptControl

                    'Update the Pickup/Delivery appointment controls for all the books in the appointment           
                    For Each b In bookControls
                        db.spUpdateAMSBookingCarrierAutomation(b, aptControl, Appointment.AMSApptCompControl)
                    Next
                    'Modified by RHR for v-8.2 on 09/19/2018 - we do not need to save the appointment we just need to update the booking data like Scheduled Date Dock door etc...
                    UpdateAMSBookings(aptControl)

                    'Call new sp here and pass in ApptControl
                    db.spGetApptNotesByControl(aptControl) 'NEXTrack Cabot Enhancement

                    blnApptBooked = True
                End If

                If Not blnApptBooked Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    strRet = oLocalize.GetLocalizedValueByKey("SchedTimeNotAvailable", "The appointment could not be booked because the time slot is no longer available. Please refresh your data and try again.")
                Else
                    'Send Alerts/CarrierConfirmation/CarrierNotification
                    Dim oDockSet As New NGLDockSettingData(Parameters)
                    Dim sParams As New Models.SchedMessagingParams With {.ApptControl = aptControl, .CompControl = iCompControl, .Warehouse = sWarehouse, .CarrierControl = iCarrierControl, .CarrierNumber = iCarrierNumber}
                    'The booking action is done by the Carrier so trigger Subscription Alerts and Carrier Confirmation
                    oDockSet.SendSchedulerAlertAsync(NGLDockSettingData.AMSMsg.CarrierBooked, sParams) 'Send Subscription Alert
                    If iBookControl > 0 Then oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.BookedConfirm, sParams) 'Send the Carrier the Booked Confirmation
                End If
                Return strRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CarrierAutomationCreateNewAppointment"))
            End Try
            Return Nothing
        End Using
    End Function


    Public Function CarrierAutomationCreateNewAppointmentWReport(ByVal ts As Models.AMSCarrierAvailableSlots) As Models.ResultObject
        Dim oRet As New Models.ResultObject()
        oRet.Success = False
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim oDock As New NGLDockSettingData(Parameters)
                'Make sure ts is not null
                If ts Is Nothing OrElse ts.CompControl = 0 Then
                    throwInvalidKeyParentRequiredException(New List(Of String) From {"Warehouse", "control # (compcontrol)"})
                End If
                'Values passed from UI
                Dim iCompControl As Integer = ts.CompControl
                Dim iCarrierControl As Integer = Parameters.UserCarrierControl
                Dim iCarrierNumber As Integer = ts.CarrierNumber
                Dim sCarrierName As String = ts.CarrierName
                Dim sWarehouse As String = ts.Warehouse
                Dim ApptReqStartTime As Date = ts.StartTime
                'Variables to populate
                Dim strRet As String = ""
                Dim blnApptBooked As Boolean = False
                Dim dockID As String = ""
                Dim dtEndTime As Date
                Dim aptControl As Integer = 0

                'Get the BookControl
                Dim iBookControl As Integer = 0
                Dim bookControls = ts.Books.Split(", ")
                Integer.TryParse(bookControls(0), iBookControl)

                'Validate that the selected time slot is still available at a dock. Returns the DockDoorID and Appointment End Time for the Dock with the lowest Sequence Number that is still available for the selected Appointment Start Time
                If oDock.ValidateCarrierAppointmentAvailability(dockID, dtEndTime, iCarrierControl, iCompControl, iBookControl, sWarehouse, sCarrierName, iCarrierNumber, ApptReqStartTime) Then

                    'The time is still available so book it
                    Dim Appointment As New DTO.AMSAppointment
                    With Appointment
                        .AMSApptCompControl = iCompControl
                        .AMSApptCarrierControl = iCarrierControl
                        .AMSApptCarrierSCAC = iCarrierNumber 'I don't know why we do this but it was like this in the old NEXTrack so I didn't change it
                        .AMSApptCarrierName = sCarrierName
                        .AMSApptStartDate = ApptReqStartTime
                        .AMSApptEndDate = dtEndTime
                        .AMSApptDockdoorID = dockID
                        .AMSApptModDate = Date.Now
                        .AMSApptModUser = "Carrier Appointment Generator"
                    End With

                    'Create the appointment record
                    Appointment = CreateRecord(Appointment)
                    If Appointment Is Nothing Then Return Nothing
                    aptControl = Appointment.AMSApptControl
                    Dim sReport As String = String.Format("Confirmation Appt Number: {2}{0}{1} Warehouse: {3}{0}{1} Must Start: {4}{0}{1} Expected End: {4}{0}{1}", vbCrLf, "<br><br>", aptControl, sWarehouse, If(Appointment.AMSApptStartDate.HasValue, Appointment.AMSApptStartDate.Value.ToString("yyyy-MM-dd HH:mm"), "Call"), If(Appointment.AMSApptEndDate.HasValue, Appointment.AMSApptEndDate.Value.ToString("yyyy-MM-dd HH:mm"), "Call"), Appointment.AMSApptDescription, Appointment.AMSApptNotes)
                    oRet.SuccessMsg = sReport
                    'Update the Pickup/Delivery appointment controls for all the books in the appointment           
                    For Each b In bookControls
                        db.spUpdateAMSBookingCarrierAutomation(b, aptControl, Appointment.AMSApptCompControl)
                    Next
                    'Modified by RHR for v-8.2 on 09/19/2018 - we do not need to save the appointment we just need to update the booking data like Scheduled Date Dock door etc...
                    UpdateAMSBookings(aptControl)

                    'Call new sp here and pass in ApptControl
                    db.spGetApptNotesByControl(aptControl) 'NEXTrack Cabot Enhancement

                    blnApptBooked = True
                End If

                If Not blnApptBooked Then
                    Dim oLocalize As New NGLcmLocalizeKeyValuePairData(Parameters)
                    oRet.ErrMsg = oLocalize.GetLocalizedValueByKey("SchedTimeNotAvailable", "The appointment could not be booked because the time slot is no longer available. Please refresh your data and try again.")
                Else
                    oRet.Success = True

                    'Send Alerts/CarrierConfirmation/CarrierNotification
                    Dim oDockSet As New NGLDockSettingData(Parameters)
                    Dim sParams As New Models.SchedMessagingParams With {.ApptControl = aptControl, .CompControl = iCompControl, .Warehouse = sWarehouse, .CarrierControl = iCarrierControl, .CarrierNumber = iCarrierNumber}
                    'The booking action is done by the Carrier so trigger Subscription Alerts and Carrier Confirmation
                    oDockSet.SendSchedulerAlertAsync(NGLDockSettingData.AMSMsg.CarrierBooked, sParams) 'Send Subscription Alert
                    If iBookControl > 0 Then oDockSet.SchedulerSendCarrierEmailAsync(NGLDockSettingData.AMSMsg.BookedConfirm, sParams) 'Send the Carrier the Booked Confirmation
                End If
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CarrierAutomationCreateNewAppointment"))
            End Try
            Return oRet
        End Using
    End Function



    ''' <summary>
    ''' Attempts to book the appointment at the available resources listed in Docks (Docks is listed in order of precedence for bookings)
    ''' On Success returns an empty string
    ''' On Fail returns an error message (or an exception if one occured)
    ''' Notes:
    ''' Docks is a comma separated string listing the DockDoorIDs in order of precedence for bookings
    ''' Books is a comma separated string of all the BookControls included in the appointment
    ''' All of these parameters are returned by the method GetCarrierAvailableAppointments() and
    ''' passed back to this method in order to schedule the booking
    ''' </summary>
    ''' <param name="ts">The time slot to book</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/31/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  We now call WarehouseUpdateExistingAppointmentWithSuggestion() instead of UpdateCarrierBookedAppointment()
    '''  and WarehouseCreateNewAppointmentWithSuggestion() instead of BookSuggestedAppointment()
    '''  This is is to fix concurrency issues as well as improve code readability and maintainability
    '''  (The process flow on the scheduler is a mess and I am slowly trying to fix it)
    ''' </remarks>
    Public Function BookWarehouseAppointment(ByVal ts As Models.AMSCarrierAvailableSlots) As String
        Try
            If ts.ApptControl <> 0 Then
                'Return UpdateCarrierBookedAppointment(ts, False)
                Return WarehouseUpdateExistingAppointmentWithSuggestion(ts) 'Modified By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
            Else
                Return WarehouseCreateNewAppointmentWithSuggestion(ts) 'Modified By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
                'Return BookSuggestedAppointment(ts, False)
            End If
        Catch ex As Exception
            ManageLinqDataExceptions(ex, buildProcedureName("BookWarehouseAppointment"))
        End Try
        Return Nothing
    End Function


    ''' <summary>
    ''' Gets any records that are linked to the Booking via SHID or EquipID and returns them all as DTO.AMSOrderList()
    ''' The view vAMSOrderLists pre-filters the data 
    ''' Where BookTranCode not in ('N','P','PC') And (BookAMSPickupApptControl = 0 OR BookAMSDeliveryApptControl = 0)
    ''' If the BookConsPrefix is NULL or EMPTY then we set it to 9999
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="EquipID"></param>
    ''' <param name="BookDateLoad"></param>
    ''' <param name="BookDateRequired"></param>
    ''' <param name="LaneOriginAddressUse"></param>
    ''' <param name="BookAMSPickupApptControl"></param>
    ''' <param name="BookAMSDeliveryApptControl"></param>
    ''' <param name="BookOrigCompControl"></param>
    ''' <param name="BookDestCompControl"></param>
    ''' <param name="IncludeLTLPool"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/31/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetAMSOrdersGrouped(ByVal BookControl As Integer,
                                        ByVal CompControl As Integer,
                                        ByVal EquipID As String,
                                        ByVal BookDateLoad As Date?,
                                        ByVal BookDateRequired As Date?,
                                        ByVal LaneOriginAddressUse As Boolean,
                                        ByVal BookAMSPickupApptControl As Integer,
                                        ByVal BookAMSDeliveryApptControl As Integer,
                                        ByVal BookOrigCompControl As Integer,
                                        ByVal BookDestCompControl As Integer,
                                        Optional ByVal IncludeLTLPool As Boolean = True) As DTO.AMSOrderList()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'Determine if this order is a Pickup or Delivery and whether to match for LoadDate or Required date
                Dim IsPickup As Boolean = LaneOriginAddressUse
                Dim dt As Date
                If BookOrigCompControl = CompControl And BookAMSPickupApptControl = 0 Then IsPickup = True
                If BookDestCompControl = CompControl And BookAMSDeliveryApptControl = 0 Then IsPickup = False
                If IsPickup Then dt = BookDateLoad Else dt = BookDateRequired

                'Get all dependent BookControls by SHID and EquipID
                Dim oBook As New NGLBookData(Parameters)
                Dim bookControls = oBook.GetDependentBookControlsByEquipID(BookControl, EquipID, CompControl, dt, IsPickup, LaneOriginAddressUse, IncludeLTLPool) 'This method now calls udfGetAMSDependentBookControls instead of udfGetDependentBookControlsByEquipID 2/13/20 

                'Get the AMSOrders for the above BookControls
                Dim AMSOrders = (
                    From d In db.vAMSOrderLists
                    Where
                        bookControls.Contains(d.BookControl)
                    Order By d.OrderType Ascending, d.BookConsPrefix Ascending, d.BookRouteConsFlag Descending
                    Select selectAMSOrderListDTOData(d, db, CompControl)).ToArray()

                Return AMSOrders
                'Return AMSOrders.OrderBy(Function(x) x.OrderType).ThenBy(Function(x) x.BookConsPrefix).ThenByDescending(Function(x) x.BookRouteConsFlag).ToArray()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSOrdersGrouped"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Do not use, Depreciated,  Instead call overloaded IsTimeSlotOpen that uses iCompControl Number. 
    ''' </summary>
    ''' <param name="DockDoorID"></param>
    ''' <param name="slotStart"></param>
    ''' <param name="slotEnd"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/31/18 for v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.1 on 10/16/2019
    '''     Depreciated,  Instead call overloaded IsTimeSlotOpen that uses iCompControl Number.
    '''     fixed bug where DockDoorID is not unique with warehouse
    ''' </remarks>
    Public Function IsTimeSlotOpen(ByVal DockDoorID As String, ByVal slotStart As Date, ByVal slotEnd As Date) As Boolean
        throwDepreciatedException("Do not Use IsTimeSlotOpen with just DockDoorID: Instead call overloaded IsTimeSlotOpen that uses DockDoorID and Comp Control Number.")
        'Using db As New NGLMASAMSDataContext(ConnectionString)
        '    Try
        '        'If Any() returns true then that means there is an exisitng appointment that overlaps the time slot so the function should return false
        '        Return Not db.AMSAppointments.Where(Function(x) x.AMSApptDockdoorID = DockDoorID AndAlso slotStart < x.AMSApptEndDate AndAlso x.AMSApptStartDate < slotEnd).Any()

        '    Catch ex As Exception
        '        ManageLinqDataExceptions(ex, buildProcedureName("IsTimeSlotOpen"))
        '    End Try
        '    Return False
        'End Using
    End Function


    ''' <summary>
    ''' Returns true if the time slot provided is not overlapped by any existing appointments at this dock
    ''' </summary>
    ''' <param name="DockDoorID"></param>
    ''' <param name="slotStart"></param>
    ''' <param name="slotEnd"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/31/18 for v-8.3 TMS365 Scheduler
    ''' Modified by RHR for v-8.2.1 on 10/16/2019
    '''     fixed bug where DockDoorID is not unique with warehouse
    ''' </remarks>
    Public Function IsTimeSlotOpen(ByVal iCompControl As Integer, ByVal DockDoorID As String, ByVal slotStart As Date, ByVal slotEnd As Date) As Boolean
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'If Any() returns true then that means there is an exisitng appointment that overlaps the time slot so the function should return false
                Return Not db.AMSAppointments.Where(Function(x) x.AMSApptCompControl = iCompControl AndAlso x.AMSApptDockdoorID = DockDoorID AndAlso slotStart < x.AMSApptEndDate AndAlso x.AMSApptStartDate < slotEnd).Any()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("IsTimeSlotOpen"))
            End Try
            Return False
        End Using
    End Function

    ''' <summary>
    ''' Checks if the appointment exists and if it does returns the Appointment Control
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="IsPickup"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="ExpectedDockID"></param>
    ''' <param name="ExpectedStartDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 8/5/20 for v-8.3.0.001 - Task #202007231830 - Scheduler Concurrency
    '''  I needed this for the unit testing
    ''' </remarks>
    Public Function VerifyAppointmentExists(ByVal BookControl As Integer, ByVal IsPickup As Boolean, ByVal CompControl As Integer, ByVal ExpectedDockID As String, ByVal ExpectedStartDate As Date) As Integer
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Dim apptControl As Integer = 0
            Try
                apptControl = db.udfVerifyAppointmentExists(BookControl, IsPickup, CompControl, ExpectedStartDate, ExpectedDockID)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("VerifyAppointmentExists"))
            End Try
            Return apptControl
        End Using
    End Function

#Region "Carrier Scheduler Grouped"

    ''' <summary>
    ''' Gets header records for the Pickup Unscheduled Orders grid
    ''' on the Carrier Scheduler Grouped page 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/7/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetAMSCarrierPickNeedApptGrouped(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.spGetAMSCarrierPickNeedApptGroupedResult()
        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.spGetAMSCarrierPickNeedApptGroupedResult 'we could create a view that only returns the SHID and it may run faster
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim FilterPRO As String = ""
                Dim FilterORDER As String = ""
                Dim FilterSHID As String = ""
                Dim FilterCNS As String = ""
                Dim FilterSchedDtFrom As Date? = Nothing
                Dim FilterSchedDtTo As Date? = Nothing
                Dim FilterLoadDtFrom As Date? = Nothing
                Dim FilterLoadDtTo As Date? = Nothing
                Dim FilterWarehouse As String = ""
                Dim sortordinal As String = ""
                Dim sortdirection As String = ""

                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    'Get the sort
                    If Not String.IsNullOrWhiteSpace(filters.sortName) Then
                        sortordinal = filters.sortName
                        If Left(filters.sortDirection.ToLower(), 3) = "des" Then
                            sortdirection = "DESCENDING"
                        Else
                            sortdirection = "ASCENDING"
                        End If
                    End If

                    'Get the filters
                    Select Case filters.filterName
                        Case "BookProNumber"
                            FilterPRO = filters.filterValue
                        Case "BookCarrOrderNumber"
                            FilterORDER = filters.filterValue
                        Case "BookSHID"
                            FilterSHID = filters.filterValue
                        Case "BookConsPrefix"
                            FilterCNS = filters.filterValue
                        Case "ScheduledDate"
                            FilterSchedDtFrom = DTran.formatStartDateFilter(filters.filterFrom)
                            FilterSchedDtTo = DTran.formatEndDateFilter(filters.filterTo)
                        Case "BookDateLoad"
                            FilterLoadDtFrom = DTran.formatStartDateFilter(filters.filterFrom)
                            FilterLoadDtTo = DTran.formatEndDateFilter(filters.filterTo)
                        Case "Warehouse"
                            FilterWarehouse = filters.filterValue
                    End Select

                    Dim spRes = (From d In db.spGetAMSCarrierPickNeedApptGrouped(Parameters.UserCarrierControl, Parameters.UserCarrierContControl, FilterPRO, FilterORDER, FilterSHID, FilterCNS, FilterSchedDtFrom, FilterSchedDtTo, FilterLoadDtFrom, FilterLoadDtTo, FilterWarehouse, sortordinal, sortdirection) Select d).ToArray()

                    'Do the paging (implement custom paging)
                    RecordCount = spRes.Length
                    If RecordCount < 1 Then RecordCount = 1
                    If filters.take < 1 Then filters.take = If(RecordCount > 500, 500, RecordCount)
                    'adjust for last page if skip beyound last page
                    If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take)
                    'adjust for first page if skip beyound or below first page
                    If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0
                    oRet = spRes.Skip(filters.skip).Take(filters.take).ToArray()
                    Return oRet
                Else
                    Return Nothing 'we only show records for carriers when Allow Web Tender is true
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierPickNeedApptGrouped"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the entire record from vAMSCarrierPickNeedAppt based on SHID
    ''' Used by Controller methods for Carrier Scheduler Grouped page
    ''' because the records no longer have BookControl etc because we only
    ''' show one record in the grid for the entire SHID
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <param name="BookOrigCompControl"></param>
    ''' <param name="LoadDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/7/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 6/5/19
    '''  Added the optional filter by LoadDate and BookOrigCompControl for the detail grids get records - you only want to see the
    '''  orders that are being picked up from the same warehouse as listed in the header grid record
    '''  Same thing with when it is called from in the REST method GetCarAvailableApptsUOGrouped()
    '''  However, when this function is called in the REST method SaveEquipmentIDForOrderGrouped(), we don't
    '''  want to filter by BookOrigCompControl because we want all the records on the truck to have the same EquipID
    ''' </remarks>
    Public Function GetAMSCarrierPickNeedApptBySHID(ByVal SHID As String, Optional ByVal BookOrigCompControl As Integer = 0, Optional ByVal LoadDate As Date? = Nothing) As LTS.vAMSCarrierPickNeedAppt()
        If String.IsNullOrWhiteSpace(SHID) Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.vAMSCarrierPickNeedAppt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSCarrierPickNeedAppt)
                iQuery = db.vAMSCarrierPickNeedAppts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                filterWhere = " (BookTranCode = ""PB"") And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                If Parameters.UserCarrierContControl <> 0 Then
                    filterWhere &= " And (BookCarrierContControl = " & Parameters.UserCarrierContControl & ") "
                End If
                filterWhere &= " And (BookSHID = """ & SHID & """) "
                If BookOrigCompControl <> 0 Then
                    filterWhere &= " And (BookOrigCompControl = " & BookOrigCompControl & ") "
                End If
                If LoadDate.HasValue Then
                    Dim dtS As Date? = New Date(LoadDate.Value.Year, LoadDate.Value.Month, LoadDate.Value.Day, 0, 0, 0)
                    Dim dtE As Date? = New Date(LoadDate.Value.Year, LoadDate.Value.Month, LoadDate.Value.Day, 23, 59, 0)
                    filterWhere &= " And (BookDateLoad >= DateTime.Parse(""" + dtS + """) And BookDateLoad <= DateTime.Parse(""" + dtE + """)) "
                End If

                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                End If

                oRet = iQuery.OrderBy("BookDateLoad", False).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierPickNeedApptBySHID"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets header records for the Delivery Unscheduled Orders grid
    ''' on the Carrier Scheduler Grouped page 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/7/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetAMSCarrierDelNeedApptGrouped(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.spGetAMSCarrierDelNeedApptGroupedResult()
        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.spGetAMSCarrierDelNeedApptGroupedResult 'we could create a view that only returns the SHID and it may run faster
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim FilterPRO As String = ""
                Dim FilterORDER As String = ""
                Dim FilterSHID As String = ""
                Dim FilterCNS As String = ""
                Dim FilterSchedDtFrom As Date? = Nothing
                Dim FilterSchedDtTo As Date? = Nothing
                Dim FilterReqDtFrom As Date? = Nothing
                Dim FilterReqDtTo As Date? = Nothing
                Dim FilterWarehouse As String = ""
                Dim sortordinal As String = ""
                Dim sortdirection As String = ""

                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    'Get the sort
                    If Not String.IsNullOrWhiteSpace(filters.sortName) Then
                        sortordinal = filters.sortName
                        If Left(filters.sortDirection.ToLower(), 3) = "des" Then
                            sortdirection = "DESCENDING"
                        Else
                            sortdirection = "ASCENDING"
                        End If
                    End If

                    'Get the filters
                    Select Case filters.filterName
                        Case "BookProNumber"
                            FilterPRO = filters.filterValue
                        Case "BookCarrOrderNumber"
                            FilterORDER = filters.filterValue
                        Case "BookSHID"
                            FilterSHID = filters.filterValue
                        Case "BookConsPrefix"
                            FilterCNS = filters.filterValue
                        Case "ScheduledDate"
                            FilterSchedDtFrom = DTran.formatStartDateFilter(filters.filterFrom)
                            FilterSchedDtTo = DTran.formatEndDateFilter(filters.filterTo)
                        Case "BookDateRequired"
                            FilterReqDtFrom = DTran.formatStartDateFilter(filters.filterFrom)
                            FilterReqDtTo = DTran.formatEndDateFilter(filters.filterTo)
                        Case "Warehouse"
                            FilterWarehouse = filters.filterValue
                    End Select

                    Dim spRes = (From d In db.spGetAMSCarrierDelNeedApptGrouped(Parameters.UserCarrierControl, Parameters.UserCarrierContControl, FilterPRO, FilterORDER, FilterSHID, FilterCNS, FilterSchedDtFrom, FilterSchedDtTo, FilterReqDtFrom, FilterReqDtTo, FilterWarehouse, sortordinal, sortdirection) Select d).ToArray()

                    'Do the paging (implement custom paging)
                    RecordCount = spRes.Length
                    If RecordCount < 1 Then RecordCount = 1
                    If filters.take < 1 Then filters.take = If(RecordCount > 500, 500, RecordCount)
                    'adjust for last page if skip beyound last page
                    If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take)
                    'adjust for first page if skip beyound or below first page
                    If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0
                    oRet = spRes.Skip(filters.skip).Take(filters.take).ToArray()
                    Return oRet
                Else
                    Return Nothing 'we only show records for carriers when Allow Web Tender is true
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierDelNeedApptGrouped"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the entire record from vAMSCarrierDelNeedAppt based on SHID
    ''' Used by Controller methods for Carrier Scheduler Grouped page
    ''' because the records no longer have BookControl etc because we only
    ''' show one record in the grid for the entire SHID
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <param name="BookDestCompControl"></param>
    ''' <param name="RequiredDate"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/7/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 6/5/19
    '''  Added the optional filter by RequiredDate and BookDestCompControl for the detail grids get records - you only want to see the
    '''  orders that are being picked up from the same warehouse as listed in the header grid record
    '''  Same thing with when it is called from in the REST method GetCarAvailableApptsUOGrouped()
    '''  However, when this function is called in the REST method SaveEquipmentIDForOrderGrouped(), we don't
    '''  want to filter by BookDestCompControl because we want all the records on the truck to have the same EquipID
    ''' </remarks>
    Public Function GetAMSCarrierDelNeedApptBySHID(ByVal SHID As String, Optional ByVal BookDestCompControl As Integer = 0, Optional ByVal RequiredDate As Date? = Nothing) As LTS.vAMSCarrierDelNeedAppt()
        If String.IsNullOrWhiteSpace(SHID) Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.vAMSCarrierDelNeedAppt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSCarrierDelNeedAppt)
                iQuery = db.vAMSCarrierDelNeedAppts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                filterWhere = " (BookTranCode = ""PB"") And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                If Parameters.UserCarrierContControl <> 0 Then
                    filterWhere &= " And (BookCarrierContControl = " & Parameters.UserCarrierContControl & ") "
                End If
                filterWhere &= " And (BookSHID = """ & SHID & """) "
                If BookDestCompControl <> 0 Then
                    filterWhere &= " And (BookDestCompControl = " & BookDestCompControl & ") "
                End If
                If RequiredDate.HasValue Then
                    Dim dtS As Date? = New Date(RequiredDate.Value.Year, RequiredDate.Value.Month, RequiredDate.Value.Day, 0, 0, 0)
                    Dim dtE As Date? = New Date(RequiredDate.Value.Year, RequiredDate.Value.Month, RequiredDate.Value.Day, 23, 59, 0)
                    filterWhere &= " And (BookDateRequired >= DateTime.Parse(""" + dtS + """) And BookDateRequired <= DateTime.Parse(""" + dtE + """)) "
                End If

                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                End If

                oRet = iQuery.OrderBy("BookDateRequired", False).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierDelNeedApptBySHID"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets header records for the Pickup Booked Appts grid
    ''' on the Carrier Scheduler Grouped page 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/10/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetAMSCarrierPickBookedApptGrouped(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.spGetAMSCarrierPickBookedApptGroupedResult()
        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.spGetAMSCarrierPickBookedApptGroupedResult 'we could create a view that only returns the SHID and it may run faster
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim FilterPRO As String = ""
                Dim FilterORDER As String = ""
                Dim FilterSHID As String = ""
                Dim FilterCNS As String = ""
                Dim FilterSchedDtFrom As Date? = Nothing
                Dim FilterSchedDtTo As Date? = Nothing
                Dim FilterLoadDtFrom As Date? = Nothing
                Dim FilterLoadDtTo As Date? = Nothing
                Dim FilterWarehouse As String = ""
                Dim sortordinal As String = ""
                Dim sortdirection As String = ""

                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    'Get the sort
                    If Not String.IsNullOrWhiteSpace(filters.sortName) Then
                        sortordinal = filters.sortName
                        If Left(filters.sortDirection.ToLower(), 3) = "des" Then
                            sortdirection = "DESCENDING"
                        Else
                            sortdirection = "ASCENDING"
                        End If
                    End If

                    'Get the filters
                    Select Case filters.filterName
                        Case "BookProNumber"
                            FilterPRO = filters.filterValue
                        Case "BookCarrOrderNumber"
                            FilterORDER = filters.filterValue
                        Case "BookSHID"
                            FilterSHID = filters.filterValue
                        Case "BookConsPrefix"
                            FilterCNS = filters.filterValue
                        Case "ScheduledDate"
                            FilterSchedDtFrom = DTran.formatStartDateFilter(filters.filterFrom)
                            FilterSchedDtTo = DTran.formatEndDateFilter(filters.filterTo)
                        Case "BookDateLoad"
                            FilterLoadDtFrom = DTran.formatStartDateFilter(filters.filterFrom)
                            FilterLoadDtTo = DTran.formatEndDateFilter(filters.filterTo)
                        Case "Warehouse"
                            FilterWarehouse = filters.filterValue
                    End Select

                    Dim spRes = (From d In db.spGetAMSCarrierPickBookedApptGrouped(Parameters.UserCarrierControl, Parameters.UserCarrierContControl, FilterPRO, FilterORDER, FilterSHID, FilterCNS, FilterSchedDtFrom, FilterSchedDtTo, FilterLoadDtFrom, FilterLoadDtTo, FilterWarehouse, sortordinal, sortdirection) Select d).ToArray()

                    'Do the paging (implement custom paging)
                    RecordCount = spRes.Length
                    If RecordCount < 1 Then RecordCount = 1
                    If filters.take < 1 Then filters.take = If(RecordCount > 500, 500, RecordCount)
                    'adjust for last page if skip beyound last page
                    If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take)
                    'adjust for first page if skip beyound or below first page
                    If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0
                    oRet = spRes.Skip(filters.skip).Take(filters.take).ToArray()
                    Return oRet
                Else
                    Return Nothing 'we only show records for carriers when Allow Web Tender is true
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierPickBookedApptGrouped"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the entire record from vAMSCarrierPickBookedAppt based on SHID
    ''' Used by Controller methods for Carrier Scheduler Grouped page
    ''' because the records no longer have BookControl etc because we only
    ''' show one record in the grid for the entire SHID
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/10/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 6/7/19
    '''  Added the optional filter by LoadDate and BookOrigCompControl for the detail grids get records - you only want to see the
    '''  orders that are being picked up from the same warehouse as listed in the header grid record
    '''  Same thing with when it is called from in the REST method GetModifyOptionCarrierBAGrouped()
    ''' </remarks>
    Public Function GetAMSCarrierPickBookedApptBySHID(ByVal SHID As String, Optional ByVal BookOrigCompControl As Integer = 0, Optional ByVal LoadDate As Date? = Nothing) As LTS.vAMSCarrierPickBookedAppt()
        If String.IsNullOrWhiteSpace(SHID) Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.vAMSCarrierPickBookedAppt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSCarrierPickBookedAppt)
                iQuery = db.vAMSCarrierPickBookedAppts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                filterWhere = " (BookTranCode = ""PB"") And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                If Parameters.UserCarrierContControl <> 0 Then
                    filterWhere &= " And (BookCarrierContControl = " & Parameters.UserCarrierContControl & ") "
                End If
                filterWhere &= " And (BookSHID = """ & SHID & """) "
                If BookOrigCompControl <> 0 Then
                    filterWhere &= " And (BookOrigCompControl = " & BookOrigCompControl & ") "
                End If
                If LoadDate.HasValue Then
                    Dim dtS As Date? = New Date(LoadDate.Value.Year, LoadDate.Value.Month, LoadDate.Value.Day, 0, 0, 0)
                    Dim dtE As Date? = New Date(LoadDate.Value.Year, LoadDate.Value.Month, LoadDate.Value.Day, 23, 59, 0)
                    filterWhere &= " And (BookDateLoad >= DateTime.Parse(""" + dtS + """) And BookDateLoad <= DateTime.Parse(""" + dtE + """)) "
                End If

                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                End If

                oRet = iQuery.OrderBy("BookDateLoad", False).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierPickBookedApptBySHID"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets header records for the Delivery Booked Appt grid
    ''' on the Carrier Scheduler Grouped page 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/10/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Function GetAMSCarrierDelBookedApptGrouped(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.spGetAMSCarrierDelBookedApptGroupedResult()
        If filters Is Nothing Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.spGetAMSCarrierDelBookedApptGroupedResult 'we could create a view that only returns the SHID and it may run faster
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim FilterPRO As String = ""
                Dim FilterORDER As String = ""
                Dim FilterSHID As String = ""
                Dim FilterCNS As String = ""
                Dim FilterSchedDtFrom As Date? = Nothing
                Dim FilterSchedDtTo As Date? = Nothing
                Dim FilterReqDtFrom As Date? = Nothing
                Dim FilterReqDtTo As Date? = Nothing
                Dim FilterWarehouse As String = ""
                Dim sortordinal As String = ""
                Dim sortdirection As String = ""

                If (DirectCast(Me.NDPBaseClassFactory("NGLCarrierData"), NGLCarrierData).GetCarrierAllowWebTender(Parameters.UserCarrierControl)) Then
                    'Get the sort
                    If Not String.IsNullOrWhiteSpace(filters.sortName) Then
                        sortordinal = filters.sortName
                        If Left(filters.sortDirection.ToLower(), 3) = "des" Then
                            sortdirection = "DESCENDING"
                        Else
                            sortdirection = "ASCENDING"
                        End If
                    End If

                    'Get the filters
                    Select Case filters.filterName
                        Case "BookProNumber"
                            FilterPRO = filters.filterValue
                        Case "BookCarrOrderNumber"
                            FilterORDER = filters.filterValue
                        Case "BookSHID"
                            FilterSHID = filters.filterValue
                        Case "BookConsPrefix"
                            FilterCNS = filters.filterValue
                        Case "ScheduledDate"
                            FilterSchedDtFrom = DTran.formatStartDateFilter(filters.filterFrom)
                            FilterSchedDtTo = DTran.formatEndDateFilter(filters.filterTo)
                        Case "BookDateRequired"
                            FilterReqDtFrom = DTran.formatStartDateFilter(filters.filterFrom)
                            FilterReqDtTo = DTran.formatEndDateFilter(filters.filterTo)
                        Case "Warehouse"
                            FilterWarehouse = filters.filterValue
                    End Select

                    Dim spRes = (From d In db.spGetAMSCarrierDelBookedApptGrouped(Parameters.UserCarrierControl, Parameters.UserCarrierContControl, FilterPRO, FilterORDER, FilterSHID, FilterCNS, FilterSchedDtFrom, FilterSchedDtTo, FilterReqDtFrom, FilterReqDtTo, FilterWarehouse, sortordinal, sortdirection) Select d).ToArray()

                    'Do the paging (implement custom paging)
                    RecordCount = spRes.Length
                    If RecordCount < 1 Then RecordCount = 1
                    If filters.take < 1 Then filters.take = If(RecordCount > 500, 500, RecordCount)
                    'adjust for last page if skip beyound last page
                    If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take)
                    'adjust for first page if skip beyound or below first page
                    If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0
                    oRet = spRes.Skip(filters.skip).Take(filters.take).ToArray()
                    Return oRet
                Else
                    Return Nothing 'we only show records for carriers when Allow Web Tender is true
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierDelBookedApptGrouped"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the entire record from vAMSCarrierDelBookedAppt based on SHID
    ''' Used by Controller methods for Carrier Scheduler Grouped page
    ''' because the records no longer have BookControl etc because we only
    ''' show one record in the grid for the entire SHID
    ''' </summary>
    ''' <param name="SHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 12/10/18 for v-8.3 TMS365 Scheduler
    ''' Modified By LVV on 6/7/19
    '''  Added the optional filter by RequiredDate and BookDestCompControl for the detail grids get records - you only want to see the
    '''  orders that are being picked up from the same warehouse as listed in the header grid record
    '''  Same thing with when it is called from in the REST method GetModifyOptionCarrierBAGrouped()
    ''' </remarks>
    Public Function GetAMSCarrierDelBookedApptBySHID(ByVal SHID As String, Optional ByVal BookDestCompControl As Integer = 0, Optional ByVal RequiredDate As Date? = Nothing) As LTS.vAMSCarrierDelBookedAppt()
        If String.IsNullOrWhiteSpace(SHID) Then Return Nothing
        If Parameters.UserCarrierControl = 0 Then Return Nothing
        Dim oRet() As LTS.vAMSCarrierDelBookedAppt
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.vAMSCarrierDelBookedAppt)
                iQuery = db.vAMSCarrierDelBookedAppts
                Dim filterWhere As String = ""
                Dim sFilterSpacer As String = ""
                filterWhere = " (BookTranCode = ""PB"") And (BookCarrierControl = " & Parameters.UserCarrierControl & ")"
                If Parameters.UserCarrierContControl <> 0 Then
                    filterWhere &= " And (BookCarrierContControl = " & Parameters.UserCarrierContControl & ") "
                End If
                filterWhere &= " And (BookSHID = """ & SHID & """) "
                If BookDestCompControl <> 0 Then
                    filterWhere &= " And (BookDestCompControl = " & BookDestCompControl & ") "
                End If
                If RequiredDate.HasValue Then
                    Dim dtS As Date? = New Date(RequiredDate.Value.Year, RequiredDate.Value.Month, RequiredDate.Value.Day, 0, 0, 0)
                    Dim dtE As Date? = New Date(RequiredDate.Value.Year, RequiredDate.Value.Month, RequiredDate.Value.Day, 23, 59, 0)
                    filterWhere &= " And (BookDateRequired >= DateTime.Parse(""" + dtS + """) And BookDateRequired <= DateTime.Parse(""" + dtE + """)) "
                End If

                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    iQuery = DLinqUtil.filterWhere(iQuery, filterWhere)
                End If

                oRet = iQuery.OrderBy("BookDateRequired", False).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetAMSCarrierDelBookedApptBySHID"))
            End Try
        End Using
        Return Nothing
    End Function

#End Region

#End Region

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.AMSAppointment)
        'Create New Record
        Return New LTS.AMSAppointment With {.AMSApptControl = d.AMSApptControl _
                                            , .AMSApptCompControl = d.AMSApptCompControl _
                                            , .AMSApptCarrierControl = d.AMSApptCarrierControl _
                                            , .AMSApptCarrierSCAC = d.AMSApptCarrierSCAC _
                                            , .AMSApptCarrierName = d.AMSApptCarrierName _
                                            , .AMSApptDescription = d.AMSApptDescription _
                                            , .AMSApptStartDate = d.AMSApptStartDate _
                                            , .AMSApptEndDate = d.AMSApptEndDate _
                                            , .AMSApptTimeZone = d.AMSApptTimeZone _
                                            , .AMSApptRecurrenceParentControl = d.AMSApptRecurrenceParentControl _
                                            , .AMSApptRecurrence = d.AMSApptRecurrence _
                                            , .AMSApptActualDateTime = d.AMSApptActualDateTime _
                                            , .AMSApptStartLoadingDateTime = d.AMSApptStartLoadingDateTime _
                                            , .AMSApptFinishLoadingDateTime = d.AMSApptFinishLoadingDateTime _
                                            , .AMSApptActLoadCompleteDateTime = d.AMSApptActLoadCompleteDateTime _
                                            , .AMSApptModDate = Date.Now _
                                            , .AMSApptModUser = Parameters.UserName _
                                            , .AMSApptNotes = d.AMSApptNotes _
                                            , .AMSApptDockdoorID = d.AMSApptDockdoorID _
                                            , .AMSApptStatusCode = d.AMSApptStatusCode _
                                            , .AMSApptUpdated = If(d.AMSApptUpdated Is Nothing, New Byte() {}, d.AMSApptUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.AMSAppointment = TryCast(LinqTable, LTS.AMSAppointment)
        If oData Is Nothing Then Return Nothing
        UpdateAMSBookings(oData.AMSApptControl)
        Return GetAMSAppointmentFiltered(oData.AMSApptControl)
    End Function

    Protected Overrides Sub NoReturnCleanUp(ByVal LinqTable As Object)
        Dim oData As LTS.AMSAppointment = TryCast(LinqTable, LTS.AMSAppointment)
        If Not oData Is Nothing Then UpdateAMSBookings(oData.AMSApptControl)
    End Sub

    Protected Overrides Sub DeleteCleanUp(ByVal LinqTable As Object)
        Dim oData As LTS.AMSAppointment = TryCast(LinqTable, LTS.AMSAppointment)
        If Not oData Is Nothing Then DeleteAMSBookings(oData.AMSApptControl)
    End Sub

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim source As LTS.AMSAppointment = TryCast(LinqTable, LTS.AMSAppointment)
                If source Is Nothing Then Return Nothing
                UpdateAMSBookings(source.AMSApptControl)
                ret = (From d In db.AMSAppointments
                       Where d.AMSApptControl = source.AMSApptControl
                       Select New DTO.QuickSaveResults With {.Control = d.AMSApptControl _
                                                            , .ModDate = d.AMSApptModDate _
                                                            , .ModUser = d.AMSApptModUser _
                                                            , .Updated = d.AMSApptUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    ''' <summary>
    ''' Creates a new BookAdhoc record and returns the updated control number in AMSOrderList object
    ''' the caller must capture all errors
    ''' </summary>
    ''' <param name="o"></param>
    ''' <param name="a"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 7/19/18 for v-8.3 TMS365 Scheduler
    '''  Added field EquipmentID maps to BookAdhocCarrTrailerNo
    ''' </remarks>
    Protected Function CreateAddHockOrder(ByRef o As DTO.AMSOrderList, ByRef a As DTO.AMSAppointment) As DTO.AMSOrderList
        Dim oRet As New DTO.AMSOrderList
        Dim oAddHock As New DTO.BookAdhoc With {.BookAdhocProNumber = o.BookProNumber,
                                                .BookAdhocAMSDeliveryApptControl = o.BookAMSDeliveryApptControl,
                                                .BookAdhocAMSPickupApptControl = o.BookAMSPickupApptControl,
                                                .BookAdhocCustCompControl = o.AMSCompControl,
                                                .BookAdhocCarrierControl = o.BookCarrierControl,
                                                .BookAdhocCarrierContact = o.BookCarrierContact,
                                                .BookAdhocCarrierContactPhone = o.BookCarrierContactPhone,
                                                .BookAdhocConsPrefix = o.BookConsPrefix,
                                                .BookAdhocCarrOrderNumber = o.BookCarrOrderNumber,
                                                .BookAdhocOrderSequence = o.BookOrderSequence,
                                                .BookAdhocOrigName = o.BookOrigName,
                                                .BookAdhocOrigAddress1 = o.BookOrigAddress1,
                                                .BookAdhocOrigCity = o.BookOrigCity,
                                                .BookAdhocOrigState = o.BookOrigState,
                                                .BookAdhocOrigCountry = o.BookOrigCountry,
                                                .BookAdhocOrigZip = o.BookOrigZip,
                                                .BookAdhocOrigPhone = o.BookOrigPhone,
                                                .BookAdhocDestName = o.BookDestName,
                                                .BookAdhocDestAddress1 = o.BookDestAddress1,
                                                .BookAdhocDestCity = o.BookDestCity,
                                                .BookAdhocDestState = o.BookDestState,
                                                .BookAdhocDestCountry = o.BookDestCountry,
                                                .BookAdhocDestZip = o.BookDestZip,
                                                .BookAdhocDestPhone = o.BookDestPhone,
                                                .BookAdhocDateOrdered = o.BookDateOrdered,
                                                .BookAdhocDateLoad = o.BookDateLoad,
                                                .BookAdhocDateRequired = o.BookDateRequired,
                                                .BookAdhocTotalCases = If(o.BookTotalCases.HasValue, o.BookTotalCases.Value, 0),
                                                .BookAdhocTotalWgt = If(o.BookTotalWgt.HasValue, o.BookTotalWgt.Value, 0),
                                                .BookAdhocTotalPL = If(o.BookTotalPL.HasValue, o.BookTotalPL.Value, 0),
                                                .BookAdhocTotalCube = If(o.BookTotalCube.HasValue, o.BookTotalCube.Value, 0),
                                                .BookAdhocTotalPX = If(o.BookTotalPX.HasValue, o.BookTotalPX.Value, 0),
                                                .BookAdhocStopNo = If(o.BookStopNo.HasValue, o.BookStopNo.Value, 0),
                                                .BookAdhocRouteConsFlag = o.BookRouteConsFlag,
                                                .BookAdhocShipCarrierName = o.BookShipCarrierName,
                                                .BookAdhocShipCarrierNumber = o.BookShipCarrierNumber,
                                                .BookAdhocShipCarrierProNumber = o.BookShipCarrierProNumber,
                                                .BookAdhocItemDetailDescription = o.BookItemDetailDescription,
                                                .BookAdhocCarrTrailerNo = o.EquipmentID}

        Dim oAdHocProvider As New NGLBookAdhocData(Me.Parameters)
        Dim newAdHocData As DTO.BookAdhoc = oAdHocProvider.CreateRecord(oAddHock)
        If Not newAdHocData Is Nothing Then
            'create the po record
            Dim oAdHockLoad As New DTO.BookAdhocLoad With {.BookAdhocLoadBookAdhocControl = newAdHocData.BookAdhocControl,
                                                           .BookAdhocLoadCaseQty = If(o.BookTotalCases.HasValue, o.BookTotalCases.Value, 0),
                                                           .BookAdhocLoadCube = If(o.BookTotalCube.HasValue, o.BookTotalCube.Value, 0),
                                                           .BookAdhocLoadPL = If(o.BookTotalCube.HasValue, o.BookTotalPL.Value, 0),
                                                           .BookAdhocLoadPONumber = o.BookLoadPONumber,
                                                           .BookAdhocLoadPX = If(o.BookTotalPX.HasValue, o.BookTotalPX.Value, 0),
                                                           .BookAdhocLoadWgt = If(o.BookTotalWgt.HasValue, o.BookTotalWgt.Value, 0)}
            Dim oAdHocLoadProvider = New NGLBookAdhocLoadData(Me.Parameters)
            Dim newAdhocLoad As DTO.BookAdhocLoad = oAdHocLoadProvider.CreateRecord(oAdHockLoad)
            'update the control numbers
            o.BookControl = newAdHocData.BookAdhocControl
            If Not newAdhocLoad Is Nothing Then o.BookLoadControl = newAdhocLoad.BookAdhocLoadControl

            oRet = o
        End If

        Return oRet

    End Function

    ''' <summary>
    ''' Updates the BookAdHoc data with changes
    ''' the caller must handle all errors
    ''' </summary>
    ''' <param name="o"></param>
    ''' <param name="a"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified for v-5.2 by RHR 7/5/2012 
    '''   added new logic to handle bookadhocloadcontrol;  we also now can create the bookadhocload record if it does not exist.
    ''' Modified By LVV on 7/19/18 for v-8.3 TMS365 Scheduler
    '''  Added field EquipmentID maps to BookAdhocCarrTrailerNo
    ''' </remarks>
    Public Function UpdateAddHockOrder(ByRef o As DTO.AMSOrderList, ByRef a As DTO.AMSAppointment) As DTO.AMSOrderList

        Dim oAdHocProvider As New NGLBookAdhocData(Me.Parameters)
        'get the current data
        Dim oAddHock As DTO.BookAdhoc = oAdHocProvider.GetBookAdhocFiltered(o.BookControl)

        With oAddHock
            .BookAdhocProNumber = o.BookProNumber
            .BookAdhocCustCompControl = o.AMSCompControl
            .BookAdhocCarrierControl = o.BookCarrierControl
            .BookAdhocCarrierContact = o.BookCarrierContact
            .BookAdhocCarrierContactPhone = o.BookCarrierContactPhone
            .BookAdhocConsPrefix = o.BookConsPrefix
            .BookAdhocCarrOrderNumber = o.BookCarrOrderNumber
            .BookAdhocOrderSequence = o.BookOrderSequence
            .BookAdhocOrigName = o.BookOrigName
            .BookAdhocOrigAddress1 = o.BookOrigAddress1
            .BookAdhocOrigCity = o.BookOrigCity
            .BookAdhocOrigState = o.BookOrigState
            .BookAdhocOrigCountry = o.BookOrigCountry
            .BookAdhocOrigZip = o.BookOrigZip
            .BookAdhocOrigPhone = o.BookOrigPhone
            .BookAdhocDestName = o.BookDestName
            .BookAdhocDestAddress1 = o.BookDestAddress1
            .BookAdhocDestCity = o.BookDestCity
            .BookAdhocDestState = o.BookDestState
            .BookAdhocDestCountry = o.BookDestCountry
            .BookAdhocDestZip = o.BookDestZip
            .BookAdhocDestPhone = o.BookDestPhone
            .BookAdhocDateOrdered = o.BookDateOrdered
            .BookAdhocDateLoad = o.BookDateLoad
            .BookAdhocDateRequired = o.BookDateRequired
            .BookAdhocTotalCases = If(o.BookTotalCases.HasValue, o.BookTotalCases.Value, 0)
            .BookAdhocTotalWgt = If(o.BookTotalWgt.HasValue, o.BookTotalWgt.Value, 0)
            .BookAdhocTotalPL = If(o.BookTotalPL.HasValue, o.BookTotalPL.Value, 0)
            .BookAdhocTotalCube = If(o.BookTotalCube.HasValue, o.BookTotalCube.Value, 0)
            .BookAdhocTotalPX = If(o.BookTotalPX.HasValue, o.BookTotalPX.Value, 0)
            .BookAdhocStopNo = If(o.BookStopNo.HasValue, o.BookStopNo.Value, 0)
            .BookAdhocRouteConsFlag = o.BookRouteConsFlag
            .BookAdhocShipCarrierName = o.BookShipCarrierName
            .BookAdhocShipCarrierNumber = o.BookShipCarrierNumber
            .BookAdhocShipCarrierProNumber = o.BookShipCarrierProNumber
            .BookAdhocItemDetailDescription = o.BookItemDetailDescription
            .BookAdhocAMSDeliveryApptControl = o.BookAMSDeliveryApptControl
            .BookAdhocAMSPickupApptControl = o.BookAMSPickupApptControl
            .BookAdhocCarrTrailerNo = o.EquipmentID
        End With

        oAdHocProvider.UpdateRecordNoReturn(oAddHock)
        Dim oAdHocLoadProvider = New NGLBookAdhocLoadData(Me.Parameters)

        'get the po record
        If o.BookLoadControl > 0 Then
            Dim oAdHockLoad As DTO.BookAdhocLoad = oAdHocLoadProvider.GetBookAdhocLoadFiltered(o.BookLoadControl)
            If Not oAdHockLoad Is Nothing Then
                'update the record
                With oAdHockLoad
                    .BookAdhocLoadCaseQty = If(o.BookTotalCases.HasValue, o.BookTotalCases.Value, 0)
                    .BookAdhocLoadCube = If(o.BookTotalCube.HasValue, o.BookTotalCube.Value, 0)
                    .BookAdhocLoadPL = If(o.BookTotalPL.HasValue, o.BookTotalPL, 0)
                    .BookAdhocLoadPONumber = o.BookLoadPONumber
                    .BookAdhocLoadPX = If(o.BookTotalPX.HasValue, o.BookTotalPX.Value, 0)
                    .BookAdhocLoadWgt = If(o.BookTotalWgt.HasValue, o.BookTotalWgt.Value, 0)
                End With
                oAdHocLoadProvider.UpdateRecordNoReturn(oAdHockLoad)
            End If
        Else
            Dim oAdHockLoads As DTO.BookAdhocLoad() = oAdHocLoadProvider.GetBookAdhocLoadsFiltered(o.BookControl)
            Dim blnMatchFound As Boolean = False
            If Not oAdHockLoads Is Nothing AndAlso oAdHockLoads.Count > 0 Then
                For Each l In oAdHockLoads
                    If l.BookAdhocLoadPONumber = o.BookLoadPONumber Then
                        blnMatchFound = True
                        With l
                            .BookAdhocLoadCaseQty = If(o.BookTotalCases.HasValue, o.BookTotalCases.Value, 0)
                            .BookAdhocLoadCube = If(o.BookTotalCube.HasValue, o.BookTotalCube.Value, 0)
                            .BookAdhocLoadPL = If(o.BookTotalPL.HasValue, o.BookTotalPL, 0)
                            .BookAdhocLoadPONumber = o.BookLoadPONumber
                            .BookAdhocLoadPX = If(o.BookTotalPX.HasValue, o.BookTotalPX.Value, 0)
                            .BookAdhocLoadWgt = If(o.BookTotalWgt.HasValue, o.BookTotalWgt.Value, 0)
                        End With
                        Dim currentAdhocLoad As DTO.BookAdhocLoad = oAdHocLoadProvider.UpdateRecord(l)
                        If Not currentAdhocLoad Is Nothing Then o.BookLoadControl = currentAdhocLoad.BookAdhocLoadControl
                        Exit For
                    End If
                Next
                If Not blnMatchFound Then
                    'just update the first record
                    With oAdHockLoads(0)
                        .BookAdhocLoadCaseQty = If(o.BookTotalCases.HasValue, o.BookTotalCases.Value, 0)
                        .BookAdhocLoadCube = If(o.BookTotalCube.HasValue, o.BookTotalCube.Value, 0)
                        .BookAdhocLoadPL = If(o.BookTotalPL.HasValue, o.BookTotalPL, 0)
                        .BookAdhocLoadPONumber = o.BookLoadPONumber
                        .BookAdhocLoadPX = If(o.BookTotalPX.HasValue, o.BookTotalPX.Value, 0)
                        .BookAdhocLoadWgt = If(o.BookTotalWgt.HasValue, o.BookTotalWgt.Value, 0)
                    End With
                    Dim currentAdhocLoad As DTO.BookAdhocLoad = oAdHocLoadProvider.UpdateRecord(oAdHockLoads(0))
                    If Not currentAdhocLoad Is Nothing Then o.BookLoadControl = currentAdhocLoad.BookAdhocLoadControl
                End If
            Else
                'we need to create the record
                Dim oAdHockLoad As New DTO.BookAdhocLoad With {.BookAdhocLoadBookAdhocControl = o.BookControl,
                                                           .BookAdhocLoadCaseQty = If(o.BookTotalCases.HasValue, o.BookTotalCases.Value, 0),
                                                           .BookAdhocLoadCube = If(o.BookTotalCube.HasValue, o.BookTotalCube.Value, 0),
                                                           .BookAdhocLoadPL = If(o.BookTotalPL.HasValue, o.BookTotalPL, 0),
                                                           .BookAdhocLoadPONumber = o.BookLoadPONumber,
                                                           .BookAdhocLoadPX = If(o.BookTotalPX.HasValue, o.BookTotalPX.Value, 0),
                                                           .BookAdhocLoadWgt = If(o.BookTotalWgt.HasValue, o.BookTotalWgt.Value, 0)}
                Dim newAdhocLoad As DTO.BookAdhocLoad = oAdHocLoadProvider.CreateRecord(oAdHockLoad)
                'update the control number
                If Not newAdhocLoad Is Nothing Then o.BookLoadControl = newAdhocLoad.BookAdhocLoadControl
            End If

        End If

        Return o

    End Function


    ''' <summary>
    ''' save or creates book or adhocbook data using the current Appointment information.
    ''' </summary>
    ''' <param name="o"></param>
    ''' <param name="a"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 7/19/18 for v-8.3 TMS365 Scheduler
    '''  Added field EquipmentID maps to BookCarrTrailerNo/BookAdhocCarrTrailerNo
    ''' </remarks>
    Public Function UpdateAMSBooking(ByRef o As DTO.AMSOrderList, ByRef a As DTO.AMSAppointment) As DTO.AMSOrderList
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Return UpdateAMSBooking(db, o, a)
        End Using
    End Function

    ''' <summary>
    ''' save or creates book or adhocbook data using the current Appointment information.
    ''' </summary>
    ''' <param name="o"></param>
    ''' <param name="a"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 7/19/18 for v-8.3 TMS365 Scheduler
    '''  Added field EquipmentID maps to BookCarrTrailerNo/BookAdhocCarrTrailerNo
    ''' </remarks>
    Public Function UpdateAMSBooking(ByRef db As NGLMASAMSDataContext, ByRef o As DTO.AMSOrderList, ByRef a As DTO.AMSAppointment) As DTO.AMSOrderList
        Dim intAMSApptControl As Integer = 0
        Try
            Dim AMSApptCompControl = a.AMSApptCompControl
            'db.Log = New DebugTextWriter
            Dim AMSResults = (From d In db.spUpdateAMSBooking(a.AMSApptControl,
                                                   a.AMSApptCompControl,
                                                   o.BookControl,
                                                   o.BookLoadControl,
                                                   o.BookProNumber,
                                                   o.BookAMSPickupApptControl,
                                                   o.BookAMSDeliveryApptControl,
                                                   o.BookConsPrefix,
                                                   o.BookCustCompControl,
                                                   o.BookCarrierControl,
                                                   o.BookCarrierContact,
                                                   o.BookCarrierContactPhone,
                                                   o.BookCarrOrderNumber,
                                                   o.BookOrderSequence,
                                                   o.BookOrigCompControl,
                                                   o.BookOrigName,
                                                   o.BookOrigAddress1,
                                                   o.BookOrigCity,
                                                   o.BookOrigState,
                                                   o.BookOrigCountry,
                                                   o.BookOrigZip,
                                                   o.BookOrigPhone,
                                                   o.BookDestCompControl,
                                                   o.BookDestName,
                                                   o.BookDestAddress1,
                                                   o.BookDestCity,
                                                   o.BookDestState,
                                                   o.BookDestCountry,
                                                   o.BookDestZip,
                                                   o.BookDestPhone,
                                                   o.BookDateOrdered,
                                                   o.BookDateLoad,
                                                   o.BookDateRequired,
                                                   o.BookTotalCases,
                                                   o.BookTotalWgt,
                                                   o.BookTotalPL,
                                                   o.BookTotalCube,
                                                   o.BookTotalPX,
                                                   o.BookStopNo,
                                                   o.BookRouteConsFlag,
                                                   o.BookShipCarrierProNumber,
                                                   o.BookShipCarrierName,
                                                   o.BookShipCarrierNumber,
                                                   o.BookItemDetailDescription,
                                                   o.BookLoadPONumber,
                                                   o.EquipmentID,
                                                   Parameters.UserName)
                              Select New DTO.AMSOrderList With {.BookControl = d.BookControl _
                                                 , .BookCustCompControl = d.BookCustCompControl _
                                                 , .BookProNumber = d.BookProNumber _
                                                , .BookCarrOrderNumber = d.BookCarrOrderNumber _
                                                , .BookConsPrefix = d.BookConsPrefix _
                                                , .AMSCompControl = AMSApptCompControl _
                                                , .BookCarrierControl = d.BookCarrierControl _
                                                , .BookCarrierContact = d.BookCarrierContact _
                                                , .BookCarrierContactPhone = d.BookCarrierContactPhone _
                                                , .BookOrigCompControl = d.BookOrigCompControl _
                                                , .BookOrigName = d.BookOrigName _
                                                , .BookOrigAddress1 = d.BookOrigAddress1 _
                                                , .BookOrigCity = d.BookOrigCity _
                                                , .BookOrigState = d.BookOrigState _
                                                , .BookOrigCountry = d.BookOrigCountry _
                                                , .BookOrigZip = d.BookOrigZip _
                                                , .BookOrigPhone = d.BookOrigPhone _
                                                , .BookDestCompControl = d.BookDestCompControl _
                                                , .BookDestName = d.BookDestName _
                                                , .BookDestAddress1 = d.BookDestAddress1 _
                                                , .BookDestCity = d.BookDestCity _
                                                , .BookDestState = d.BookDestState _
                                                , .BookDestCountry = d.BookDestCountry _
                                                , .BookDestZip = d.BookDestZip _
                                                , .BookDestPhone = d.BookDestPhone _
                                                , .BookDateOrdered = d.BookDateOrdered _
                                                , .BookDateLoad = d.BookDateLoad _
                                                , .BookDateRequired = d.BookDateRequired _
                                                , .BookTotalCases = d.BookTotalCases _
                                                , .BookTotalWgt = d.BookTotalWgt _
                                                , .BookTotalPL = d.BookTotalPL _
                                                , .BookTotalCube = d.BookTotalCube _
                                                , .BookTotalPX = d.BookTotalPX _
                                                , .BookStopNo = d.BookStopNo _
                                                , .BookRouteConsFlag = d.BookRouteConsFlag _
                                                , .BookOrderSequence = d.BookOrderSequence _
                                                , .BookShipCarrierProNumber = d.BookShipCarrierProNumber _
                                                , .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw _
                                                , .BookShipCarrierProControl = d.BookShipCarrierProControl _
                                                , .BookShipCarrierName = d.BookShipCarrierName _
                                                , .BookShipCarrierNumber = d.BookShipCarrierNumber _
                                                , .BookODControl = d.BookODControl _
                                                , .CarrierNumber = If(d.CarrierNumber.HasValue, d.CarrierNumber.Value, 0) _
                                                , .CarrierName = d.CarrierName _
                                                , .CarrierSCAC = d.CarrierSCAC _
                                                , .LaneNumber = d.LaneNumber _
                                                , .LaneOriginAddressUse = d.LaneOriginAddressUse _
                                                , .BookLoadPONumber = d.BookLoadPONumber _
                                                , .BookLoadControl = If(d.BookLoadControl.HasValue, d.BookLoadControl.Value, 0) _
                                                , .BookItemDetailDescription = d.BookItemDetailDescription _
                                                , .BookAMSPickupApptControl = d.BookAMSPickupApptControl _
                                                , .BookAMSDeliveryApptControl = d.BookAMSDeliveryApptControl _
                                                , .EquipmentID = d.BookCarrTrailerNo}).FirstOrDefault()

            Return AMSResults

        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
    End Function

    'Modified By LVV On 7/19/18 For v-8.3 TMS365 Scheduler -- Added field EquipmentID maps to BookCarrTrailerNo
    Protected Function UpdateBook(ByRef o As DTO.AMSOrderList, ByRef a As DTO.AMSAppointment) As DTO.AMSOrderList
        Dim oBookProvider As New NGLBookData(Me.Parameters)
        'get the current data
        Dim oBook As DTO.Book = oBookProvider.GetBookFilteredNoChildren(o.BookControl)
        With oBook
            .BookAMSPickupApptControl = o.BookAMSPickupApptControl
            .BookAMSDeliveryApptControl = o.BookAMSDeliveryApptControl
            .BookCarrTrailerNo = o.EquipmentID
        End With

        oBookProvider.UpdateRecordNoReturn(oBook)

        Return o

    End Function

    'Modified By LVV On 7/19/18 For v-8.3 TMS365 Scheduler -- Added field EquipmentID maps to BookAdhocCarrTrailerNo
    Protected Function GetAMSBookAdhocRecordsByAppointment(ByVal AMSApptControl As Integer) As List(Of DTO.AMSOrderList)
        Dim lAMSOrders As New List(Of DTO.AMSOrderList)
        Dim oAdHocDataProvider As New NGLBookAdhocData(Me.Parameters)
        'get the current data
        Dim oData As DTO.BookAdhoc() = oAdHocDataProvider.GetBookAdhocsByAppointment(AMSApptControl)
        If Not oData Is Nothing AndAlso oData.Count > 0 Then
            For Each d In oData
                Dim oListItem As New DTO.AMSOrderList
                With oListItem
                    .BookControl = d.BookAdhocControl
                    .BookProNumber = d.BookAdhocProNumber
                    .BookCustCompControl = d.BookAdhocCustCompControl
                    .BookCarrierControl = d.BookAdhocCarrierControl
                    .BookCarrierContact = d.BookAdhocCarrierContact
                    .BookCarrierContactPhone = d.BookAdhocCarrierContactPhone
                    .BookConsPrefix = d.BookAdhocConsPrefix
                    .BookCarrOrderNumber = d.BookAdhocCarrOrderNumber
                    .BookOrderSequence = d.BookAdhocOrderSequence
                    .BookOrigCompControl = d.BookAdhocOrigCompControl
                    .BookOrigName = d.BookAdhocOrigName
                    .BookOrigAddress1 = d.BookAdhocOrigAddress1
                    .BookOrigCity = d.BookAdhocOrigCity
                    .BookOrigState = d.BookAdhocOrigState
                    .BookOrigCountry = d.BookAdhocOrigCountry
                    .BookOrigZip = d.BookAdhocOrigZip
                    .BookOrigPhone = d.BookAdhocOrigPhone
                    .BookDestCompControl = d.BookAdhocDestCompControl
                    .BookDestName = d.BookAdhocDestName
                    .BookDestAddress1 = d.BookAdhocDestAddress1
                    .BookDestCity = d.BookAdhocDestCity
                    .BookDestState = d.BookAdhocDestState
                    .BookDestCountry = d.BookAdhocDestCountry
                    .BookDestZip = d.BookAdhocDestZip
                    .BookDestPhone = d.BookAdhocDestPhone
                    .BookDateOrdered = d.BookAdhocDateOrdered
                    .BookDateLoad = d.BookAdhocDateLoad
                    .BookDateRequired = d.BookAdhocDateRequired
                    .BookTotalCases = d.BookAdhocTotalCases
                    .BookTotalWgt = d.BookAdhocTotalWgt
                    .BookTotalPL = d.BookAdhocTotalPL
                    .BookTotalCube = d.BookAdhocTotalCube
                    .BookTotalPX = d.BookAdhocTotalPX
                    .BookStopNo = d.BookAdhocStopNo
                    .BookRouteConsFlag = d.BookAdhocRouteConsFlag
                    .BookShipCarrierName = d.BookAdhocShipCarrierName
                    .BookShipCarrierNumber = d.BookAdhocShipCarrierNumber
                    .BookShipCarrierProNumber = d.BookAdhocShipCarrierProNumber
                    .BookItemDetailDescription = d.BookAdhocItemDetailDescription
                    If Not d.BookAdhocLoads Is Nothing AndAlso d.BookAdhocLoads.Count > 0 Then
                        .BookLoadPONumber = d.BookAdhocLoads(0).BookAdhocLoadPONumber
                        .BookLoadControl = d.BookAdhocLoads(0).BookAdhocLoadControl
                    End If
                    .BookAMSPickupApptControl = d.BookAdhocAMSPickupApptControl
                    .BookAMSDeliveryApptControl = d.BookAdhocAMSDeliveryApptControl
                    .EquipmentID = d.BookAdhocCarrTrailerNo
                End With
                lAMSOrders.Add(oListItem)
            Next
        End If
        Return lAMSOrders

    End Function

    'Modified By LVV On 7/19/18 For v-8.3 TMS365 Scheduler -- Added field EquipmentID maps to BookCarrTrailerNo
    Protected Function GetAMSBookRecordsByAppointment(ByVal AMSApptControl As Integer) As List(Of DTO.AMSOrderList)
        Dim lAMSOrders As New List(Of DTO.AMSOrderList)
        Dim oBookDataProvider As New NGLBookData(Me.Parameters)
        'get the current data
        Dim oData As DTO.Book() = oBookDataProvider.GetBooksByAppointment(AMSApptControl)
        If Not oData Is Nothing AndAlso oData.Count > 0 Then
            For Each d In oData
                Dim oListItem As New DTO.AMSOrderList
                With oListItem
                    .BookControl = d.BookControl
                    .BookProNumber = d.BookProNumber
                    .BookCustCompControl = d.BookCustCompControl
                    .BookCarrierControl = d.BookCarrierControl
                    .BookCarrierContact = d.BookCarrierContact
                    .BookCarrierContactPhone = d.BookCarrierContactPhone
                    .BookConsPrefix = d.BookConsPrefix
                    .BookCarrOrderNumber = d.BookCarrOrderNumber
                    .BookOrderSequence = d.BookOrderSequence
                    .BookOrigCompControl = d.BookOrigCompControl
                    .BookOrigName = d.BookOrigName
                    .BookOrigAddress1 = d.BookOrigAddress1
                    .BookOrigCity = d.BookOrigCity
                    .BookOrigState = d.BookOrigState
                    .BookOrigCountry = d.BookOrigCountry
                    .BookOrigZip = d.BookOrigZip
                    .BookOrigPhone = d.BookOrigPhone
                    .BookDestCompControl = d.BookDestCompControl
                    .BookDestName = d.BookDestName
                    .BookDestAddress1 = d.BookDestAddress1
                    .BookDestCity = d.BookDestCity
                    .BookDestState = d.BookDestState
                    .BookDestCountry = d.BookDestCountry
                    .BookDestZip = d.BookDestZip
                    .BookDestPhone = d.BookDestPhone
                    .BookDateOrdered = d.BookDateOrdered
                    .BookDateLoad = d.BookDateLoad
                    .BookDateRequired = d.BookDateRequired
                    .BookTotalCases = d.BookTotalCases
                    .BookTotalWgt = d.BookTotalWgt
                    .BookTotalPL = d.BookTotalPL
                    .BookTotalCube = d.BookTotalCube
                    .BookTotalPX = d.BookTotalPX
                    .BookStopNo = d.BookStopNo
                    .BookRouteConsFlag = d.BookRouteConsFlag
                    .BookShipCarrierName = d.BookShipCarrierName
                    .BookShipCarrierNumber = d.BookShipCarrierNumber
                    .BookShipCarrierProNumber = d.BookShipCarrierProNumber
                    .BookShipCarrierProNumberRaw = d.BookShipCarrierProNumberRaw
                    .BookShipCarrierProControl = d.BookShipCarrierProControl
                    .BookItemDetailDescription = d.BookItemDetailDescription
                    If Not d.BookLoads Is Nothing AndAlso d.BookLoads.Count > 0 Then
                        .BookLoadPONumber = d.BookLoads(0).BookLoadPONumber
                        .BookLoadControl = d.BookLoads(0).BookLoadControl
                    End If
                    .BookAMSPickupApptControl = d.BookAMSPickupApptControl
                    .BookAMSDeliveryApptControl = d.BookAMSDeliveryApptControl
                    .EquipmentID = d.BookCarrTrailerNo
                End With
                lAMSOrders.Add(oListItem)
            Next
        End If
        Return lAMSOrders

    End Function

    Protected Function UpdateAMSBookings(ByVal AMSApptControl As Integer) As Boolean
        Dim strBatchName As String = "UpdateAMSBookings"
        Dim strProcName As String = "dbo.spUpdateAMSBookings"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If AMSApptControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@AMSApptControl", AMSApptControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function

    Protected Function DeleteAMSBookings(ByVal AMSApptControl As Integer) As Boolean
        Dim strBatchName As String = "DeleteAMSBookings"
        Dim strProcName As String = "dbo.spDeleteAMSBookings"
        Dim blnRet As Boolean = False
        'Validate the parameter data
        If AMSApptControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@AMSApptControl", AMSApptControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        blnRet = True
        Return blnRet
    End Function


    Friend Function selectDTOData(ByVal d As LTS.AMSAppointment, ByRef db As NGLMASAMSDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DTO.AMSAppointment
        Return New DTO.AMSAppointment With {.AMSApptControl = d.AMSApptControl _
                                                      , .AMSApptCompControl = d.AMSApptCompControl _
                                                      , .AMSApptCarrierControl = d.AMSApptCarrierControl _
                                                      , .AMSApptCarrierSCAC = d.AMSApptCarrierSCAC _
                                                      , .AMSApptCarrierName = d.AMSApptCarrierName _
                                                      , .AMSApptDescription = d.AMSApptDescription _
                                                      , .AMSApptStartDate = d.AMSApptStartDate _
                                                      , .AMSApptEndDate = d.AMSApptEndDate _
                                                      , .AMSApptTimeZone = d.AMSApptTimeZone _
                                                      , .AMSApptRecurrenceParentControl = d.AMSApptRecurrenceParentControl _
                                                      , .AMSApptRecurrence = d.AMSApptRecurrence _
                                                      , .AMSApptActualDateTime = d.AMSApptActualDateTime _
                                                      , .AMSApptStartLoadingDateTime = d.AMSApptStartLoadingDateTime _
                                                      , .AMSApptFinishLoadingDateTime = d.AMSApptFinishLoadingDateTime _
                                                      , .AMSApptActLoadCompleteDateTime = d.AMSApptActLoadCompleteDateTime _
                                                      , .AMSApptModDate = d.AMSApptModDate _
                                                      , .AMSApptModUser = d.AMSApptModUser _
                                                      , .AMSApptNotes = d.AMSApptNotes _
                                                      , .AMSApptDockdoorID = d.AMSApptDockdoorID _
                                                      , .AMSApptStatusCode = d.AMSApptStatusCode,
                                                       .Page = page,
                                                       .Pages = pagecount,
                                                       .RecordCount = recordcount,
                                                       .PageSize = pagesize _
                                                      , .AMSApptUpdated = d.AMSApptUpdated.ToArray()}
    End Function

#End Region

End Class

Public Class NGLBookAdhocData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASAMSDataContext(ConnectionString)
        Me.LinqTable = db.BookAdhocs
        Me.LinqDB = db
        Me.SourceClass = "NGLBookAdhocData"
    End Sub

#End Region

#Region "Properties"
    Private _RecalcTotals As Boolean = False


    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASAMSDataContext(ConnectionString)
            Me.LinqTable = db.BookAdhocs
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
        Return GetBookAdhocFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookAdhocsFiltered()
    End Function

    Public Function GetBookAdhocFiltered(Optional ByVal Control As Integer = 0,
                                    Optional ByVal BookAdhocProNumber As String = "",
                                    Optional ByVal BookAdhocConsPrefix As String = "",
                                    Optional ByVal BookAdhocCarrOrderNumber As String = "",
                                    Optional ByVal BookAdhocOrderSequence As Integer = 0) As DTO.BookAdhoc
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria                
                Dim BookAdhoc As DTO.BookAdhoc = (
                From d In db.BookAdhocs
                Where
                    (d.BookAdhocControl = If(Control = 0, d.BookAdhocControl, Control)) _
                    And
                    (BookAdhocProNumber Is Nothing OrElse String.IsNullOrEmpty(BookAdhocProNumber) OrElse d.BookAdhocProNumber = BookAdhocProNumber) _
                    And
                    (BookAdhocConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookAdhocConsPrefix) OrElse d.BookAdhocConsPrefix = BookAdhocConsPrefix) _
                    And
                    (BookAdhocCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookAdhocCarrOrderNumber) OrElse d.BookAdhocCarrOrderNumber = BookAdhocCarrOrderNumber) _
                    And
                    (BookAdhocOrderSequence = 0 OrElse d.BookAdhocOrderSequence = BookAdhocOrderSequence)
                Order By d.BookAdhocStopNo Ascending
                Select New DTO.BookAdhoc With {.BookAdhocControl = d.BookAdhocControl _
                                            , .BookAdhocProNumber = d.BookAdhocProNumber _
                                            , .BookAdhocProBase = d.BookAdhocProBase _
                                            , .BookAdhocConsPrefix = d.BookAdhocConsPrefix _
                                            , .BookAdhocCustCompControl = d.BookAdhocCustCompControl _
                                            , .BookAdhocCommCompControl = d.BookAdhocCommCompControl _
                                            , .BookAdhocODControl = d.BookAdhocODControl _
                                            , .BookAdhocCarrierControl = d.BookAdhocCarrierControl _
                                            , .BookAdhocCarrierContact = d.BookAdhocCarrierContact _
                                            , .BookAdhocCarrierContactPhone = d.BookAdhocCarrierContactPhone _
                                            , .BookAdhocOrigCompControl = If(d.BookAdhocOrigCompControl.HasValue, d.BookAdhocOrigCompControl, 0) _
                                            , .BookAdhocOrigName = d.BookAdhocOrigName _
                                            , .BookAdhocOrigAddress1 = d.BookAdhocOrigAddress1 _
                                            , .BookAdhocOrigAddress2 = d.BookAdhocOrigAddress2 _
                                            , .BookAdhocOrigAddress3 = d.BookAdhocOrigAddress3 _
                                            , .BookAdhocOrigCity = d.BookAdhocOrigCity _
                                            , .BookAdhocOrigState = d.BookAdhocOrigState _
                                            , .BookAdhocOrigCountry = d.BookAdhocOrigCountry _
                                            , .BookAdhocOrigZip = d.BookAdhocOrigZip _
                                            , .BookAdhocOrigPhone = d.BookAdhocOrigPhone _
                                            , .BookAdhocOrigFax = d.BookAdhocOrigFax _
                                            , .BookAdhocOriginStartHrs = d.BookAdhocOriginStartHrs _
                                            , .BookAdhocOriginStopHrs = d.BookAdhocOriginStopHrs _
                                            , .BookAdhocOriginApptReq = If(d.BookAdhocOriginApptReq.HasValue, d.BookAdhocOriginApptReq, False) _
                                            , .BookAdhocDestCompControl = If(d.BookAdhocDestCompControl.HasValue, d.BookAdhocDestCompControl, 0) _
                                            , .BookAdhocDestName = d.BookAdhocDestName _
                                            , .BookAdhocDestAddress1 = d.BookAdhocDestAddress1 _
                                            , .BookAdhocDestAddress2 = d.BookAdhocDestAddress2 _
                                            , .BookAdhocDestAddress3 = d.BookAdhocDestAddress3 _
                                            , .BookAdhocDestCity = d.BookAdhocDestCity _
                                            , .BookAdhocDestState = d.BookAdhocDestState _
                                            , .BookAdhocDestCountry = d.BookAdhocDestCountry _
                                            , .BookAdhocDestZip = d.BookAdhocDestZip _
                                            , .BookAdhocDestPhone = d.BookAdhocDestPhone _
                                            , .BookAdhocDestFax = d.BookAdhocDestFax _
                                            , .BookAdhocDestStartHrs = d.BookAdhocDestStartHrs _
                                            , .BookAdhocDestStopHrs = d.BookAdhocDestStopHrs _
                                            , .BookAdhocDestApptReq = If(d.BookAdhocDestApptReq.HasValue, d.BookAdhocDestApptReq, False) _
                                            , .BookAdhocDateOrdered = d.BookAdhocDateOrdered _
                                            , .BookAdhocDateLoad = d.BookAdhocDateLoad _
                                            , .BookAdhocDateInvoice = d.BookAdhocDateInvoice _
                                            , .BookAdhocDateRequired = d.BookAdhocDateRequired _
                                            , .BookAdhocDateDelivered = d.BookAdhocDateDelivered _
                                            , .BookAdhocTotalCases = If(d.BookAdhocTotalCases.HasValue, d.BookAdhocTotalCases, 0) _
                                            , .BookAdhocTotalWgt = If(d.BookAdhocTotalWgt.HasValue, d.BookAdhocTotalWgt, 0) _
                                            , .BookAdhocTotalPL = If(d.BookAdhocTotalPL.HasValue, d.BookAdhocTotalPL, 0) _
                                            , .BookAdhocTotalCube = If(d.BookAdhocTotalCube.HasValue, d.BookAdhocTotalCube, 0) _
                                            , .BookAdhocTotalPX = If(d.BookAdhocTotalPX.HasValue, d.BookAdhocTotalPX, 0) _
                                            , .BookAdhocTotalBFC = If(d.BookAdhocTotalBFC.HasValue, d.BookAdhocTotalBFC, 0) _
                                            , .BookAdhocTranCode = d.BookAdhocTranCode _
                                            , .BookAdhocPayCode = d.BookAdhocPayCode _
                                            , .BookAdhocTypeCode = d.BookAdhocTypeCode _
                                            , .BookAdhocBOLCode = d.BookAdhocBOLCode _
                                            , .BookAdhocStopNo = If(d.BookAdhocStopNo.HasValue, d.BookAdhocStopNo, 0) _
                                            , .BookAdhocCarrFBNumber = d.BookAdhocCarrFBNumber _
                                            , .BookAdhocCarrOrderNumber = d.BookAdhocCarrOrderNumber _
                                            , .BookAdhocCarrBLNumber = d.BookAdhocCarrBLNumber _
                                            , .BookAdhocCarrBookAdhocDate = d.BookAdhocCarrBookAdhocDate _
                                            , .BookAdhocCarrBookAdhocTime = d.BookAdhocCarrBookAdhocTime _
                                            , .BookAdhocCarrBookAdhocContact = d.BookAdhocCarrBookAdhocContact _
                                            , .BookAdhocCarrScheduleDate = d.BookAdhocCarrScheduleDate _
                                            , .BookAdhocCarrScheduleTime = d.BookAdhocCarrScheduleTime _
                                            , .BookAdhocCarrActualDate = d.BookAdhocCarrActualDate _
                                            , .BookAdhocCarrActualTime = d.BookAdhocCarrActualTime _
                                            , .BookAdhocCarrActLoadComplete_Date = d.BookAdhocCarrActLoadComplete_Date _
                                            , .BookAdhocCarrActLoadCompleteTime = d.BookAdhocCarrActLoadCompleteTime _
                                            , .BookAdhocCarrDockPUAssigment = d.BookAdhocCarrDockPUAssigment _
                                            , .BookAdhocCarrPODate = d.BookAdhocCarrPODate _
                                            , .BookAdhocCarrPOTime = d.BookAdhocCarrPOTime _
                                            , .BookAdhocCarrApptDate = d.BookAdhocCarrApptDate _
                                            , .BookAdhocCarrApptTime = d.BookAdhocCarrApptTime _
                                            , .BookAdhocCarrActDate = d.BookAdhocCarrActDate _
                                            , .BookAdhocCarrActTime = d.BookAdhocCarrActTime _
                                            , .BookAdhocCarrActUnloadCompDate = d.BookAdhocCarrActUnloadCompDate _
                                            , .BookAdhocCarrActUnloadCompTime = d.BookAdhocCarrActUnloadCompTime _
                                            , .BookAdhocCarrDockDelAssignment = d.BookAdhocCarrDockDelAssignment _
                                            , .BookAdhocCarrVarDay = If(d.BookAdhocCarrVarDay.HasValue, d.BookAdhocCarrVarDay, 0) _
                                            , .BookAdhocCarrVarHrs = If(d.BookAdhocCarrVarHrs.HasValue, d.BookAdhocCarrVarHrs, 0) _
                                            , .BookAdhocCarrTrailerNo = d.BookAdhocCarrTrailerNo _
                                            , .BookAdhocCarrSealNo = d.BookAdhocCarrSealNo _
                                            , .BookAdhocCarrDriverNo = d.BookAdhocCarrDriverNo _
                                            , .BookAdhocCarrDriverName = d.BookAdhocCarrDriverName _
                                            , .BookAdhocCarrRouteNo = d.BookAdhocCarrRouteNo _
                                            , .BookAdhocCarrTripNo = d.BookAdhocCarrTripNo _
                                            , .BookAdhocFinARBookAdhocFrt = If(d.BookAdhocFinARBookAdhocFrt.HasValue, d.BookAdhocFinARBookAdhocFrt, 0) _
                                            , .BookAdhocFinARInvoiceDate = d.BookAdhocFinARInvoiceDate _
                                            , .BookAdhocFinARInvoiceAmt = If(d.BookAdhocFinARInvoiceAmt.HasValue, d.BookAdhocFinARInvoiceAmt, 0) _
                                            , .BookAdhocFinARPayDate = d.BookAdhocFinARPayDate _
                                            , .BookAdhocFinARPayAmt = If(d.BookAdhocFinARPayAmt.HasValue, d.BookAdhocFinARPayAmt, 0) _
                                            , .BookAdhocFinARCheck = d.BookAdhocFinARCheck _
                                            , .BookAdhocFinARGLNumber = d.BookAdhocFinARGLNumber _
                                            , .BookAdhocFinARBalance = If(d.BookAdhocFinARBalance.HasValue, d.BookAdhocFinARBalance, 0) _
                                            , .BookAdhocFinARCurType = If(d.BookAdhocFinARCurType.HasValue, d.BookAdhocFinARCurType, 0) _
                                            , .BookAdhocFinAPBillNumber = d.BookAdhocFinAPBillNumber _
                                            , .BookAdhocFinAPBillNoDate = d.BookAdhocFinAPBillNoDate _
                                            , .BookAdhocFinAPBillInvDate = d.BookAdhocFinAPBillInvDate _
                                            , .BookAdhocFinAPActWgt = If(d.BookAdhocFinAPActWgt.HasValue, d.BookAdhocFinAPActWgt, 0) _
                                            , .BookAdhocFinAPStdCost = If(d.BookAdhocFinAPStdCost.HasValue, d.BookAdhocFinAPStdCost, 0) _
                                            , .BookAdhocFinAPActCost = If(d.BookAdhocFinAPActCost.HasValue, d.BookAdhocFinAPActCost, 0) _
                                            , .BookAdhocFinAPPayDate = d.BookAdhocFinAPPayDate _
                                            , .BookAdhocFinAPPayAmt = If(d.BookAdhocFinAPPayAmt.HasValue, d.BookAdhocFinAPPayAmt, 0) _
                                            , .BookAdhocFinAPCheck = d.BookAdhocFinAPCheck _
                                            , .BookAdhocFinAPGLNumber = d.BookAdhocFinAPGLNumber _
                                            , .BookAdhocFinAPLastViewed = d.BookAdhocFinAPLastViewed _
                                            , .BookAdhocFinAPCurType = If(d.BookAdhocFinAPCurType.HasValue, d.BookAdhocFinAPCurType, 0) _
                                            , .BookAdhocFinCommStd = If(d.BookAdhocFinCommStd.HasValue, d.BookAdhocFinCommStd, 0) _
                                            , .BookAdhocFinCommAct = If(d.BookAdhocFinCommAct.HasValue, d.BookAdhocFinCommAct, 0) _
                                            , .BookAdhocFinCommPayDate = d.BookAdhocFinCommPayDate _
                                            , .BookAdhocFinCommPayAmt = If(d.BookAdhocFinCommPayAmt.HasValue, d.BookAdhocFinCommPayAmt, 0) _
                                            , .BookAdhocFinCommtCheck = d.BookAdhocFinCommtCheck _
                                            , .BookAdhocFinCommCreditAmt = If(d.BookAdhocFinCommCreditAmt.HasValue, d.BookAdhocFinCommCreditAmt, 0) _
                                            , .BookAdhocFinCommCreditPayDate = d.BookAdhocFinCommCreditPayDate _
                                            , .BookAdhocFinCommLoadCount = If(d.BookAdhocFinCommLoadCount.HasValue, d.BookAdhocFinCommLoadCount, 0) _
                                            , .BookAdhocFinCommGLNumber = d.BookAdhocFinCommGLNumber _
                                            , .BookAdhocFinCheckClearedDate = d.BookAdhocFinCheckClearedDate _
                                            , .BookAdhocFinCheckClearedNumber = d.BookAdhocFinCheckClearedNumber _
                                            , .BookAdhocFinCheckClearedAmt = If(d.BookAdhocFinCheckClearedAmt.HasValue, d.BookAdhocFinCheckClearedAmt, 0) _
                                            , .BookAdhocFinCheckClearedDesc = d.BookAdhocFinCheckClearedDesc _
                                            , .BookAdhocFinCheckClearedAcct = d.BookAdhocFinCheckClearedAcct _
                                            , .BookAdhocRevBilledBFC = If(d.BookAdhocRevBilledBFC.HasValue, d.BookAdhocRevBilledBFC, 0) _
                                            , .BookAdhocRevCarrierCost = If(d.BookAdhocRevCarrierCost.HasValue, d.BookAdhocRevCarrierCost, 0) _
                                            , .BookAdhocRevStopQty = If(d.BookAdhocRevStopQty.HasValue, d.BookAdhocRevStopQty, 0) _
                                            , .BookAdhocRevStopCost = If(d.BookAdhocRevStopCost.HasValue, d.BookAdhocRevStopCost, 0) _
                                            , .BookAdhocRevOtherCost = If(d.BookAdhocRevOtherCost.HasValue, d.BookAdhocRevOtherCost, 0) _
                                            , .BookAdhocRevTotalCost = If(d.BookAdhocRevTotalCost.HasValue, d.BookAdhocRevTotalCost, 0) _
                                            , .BookAdhocRevLoadSavings = If(d.BookAdhocRevLoadSavings.HasValue, d.BookAdhocRevLoadSavings, 0) _
                                            , .BookAdhocRevCommPercent = If(d.BookAdhocRevCommPercent.HasValue, d.BookAdhocRevCommPercent, 0) _
                                            , .BookAdhocRevCommCost = If(d.BookAdhocRevCommCost.HasValue, d.BookAdhocRevCommCost, 0) _
                                            , .BookAdhocRevGrossRevenue = If(d.BookAdhocRevGrossRevenue.HasValue, d.BookAdhocRevGrossRevenue, 0) _
                                            , .BookAdhocRevNegRevenue = If(d.BookAdhocRevNegRevenue.HasValue, d.BookAdhocRevNegRevenue, 0) _
                                            , .BookAdhocMilesFrom = If(d.BookAdhocMilesFrom.HasValue, d.BookAdhocMilesFrom, 0) _
                                            , .BookAdhocLaneCarrControl = If(d.BookAdhocLaneCarrControl.HasValue, d.BookAdhocLaneCarrControl, 0) _
                                            , .BookAdhocHoldLoad = If(d.BookAdhocHoldLoad.HasValue, d.BookAdhocHoldLoad, 0) _
                                            , .BookAdhocRouteFinalDate = d.BookAdhocRouteFinalDate _
                                            , .BookAdhocRouteFinalCode = d.BookAdhocRouteFinalCode _
                                            , .BookAdhocRouteFinalFlag = d.BookAdhocRouteFinalFlag _
                                            , .BookAdhocWarehouseNumber = d.BookAdhocWarehouseNumber _
                                            , .BookAdhocComCode = d.BookAdhocComCode _
                                            , .BookAdhocTransType = d.BookAdhocTransType _
                                            , .BookAdhocRouteConsFlag = d.BookAdhocRouteConsFlag _
                                            , .BookAdhocWhseAuthorizationNo = d.BookAdhocWhseAuthorizationNo _
                                            , .BookAdhocHotLoad = If(d.BookAdhocHotLoad.HasValue, d.BookAdhocHotLoad, False) _
                                            , .BookAdhocFinAPActTax = If(d.BookAdhocFinAPActTax.HasValue, d.BookAdhocFinAPActTax, 0) _
                                            , .BookAdhocFinAPExportFlag = d.BookAdhocFinAPExportFlag _
                                            , .BookAdhocFinARFreightTax = d.BookAdhocFinARFreightTax _
                                            , .BookAdhocRevFreightTax = d.BookAdhocRevFreightTax _
                                            , .BookAdhocRevNetCost = d.BookAdhocRevNetCost _
                                            , .BookAdhocFinServiceFee = If(d.BookAdhocFinServiceFee.HasValue, d.BookAdhocFinServiceFee, 0) _
                                            , .BookAdhocFinAPExportDate = d.BookAdhocFinAPExportDate _
                                            , .BookAdhocFinAPExportRetry = If(d.BookAdhocFinAPExportRetry.HasValue, d.BookAdhocFinAPExportRetry, 0) _
                                            , .BookAdhocCarrierContControl = If(d.BookAdhocCarrierContControl.HasValue, d.BookAdhocCarrierContControl, 0) _
                                            , .BookAdhocHotLoadSent = If(d.BookAdhocHotLoadSent.HasValue, d.BookAdhocHotLoadSent, False) _
                                            , .BookAdhocExportDocCreated = If(d.BookAdhocExportDocCreated.HasValue, d.BookAdhocExportDocCreated, False) _
                                            , .BookAdhocDoNotInvoice = d.BookAdhocDoNotInvoice _
                                            , .BookAdhocCarrStartLoadingDate = d.BookAdhocCarrStartLoadingDate _
                                            , .BookAdhocCarrStartLoadingTime = d.BookAdhocCarrStartLoadingTime _
                                            , .BookAdhocCarrFinishLoadingDate = d.BookAdhocCarrFinishLoadingDate _
                                            , .BookAdhocCarrFinishLoadingTime = d.BookAdhocCarrFinishLoadingTime _
                                            , .BookAdhocCarrStartUnloadingDate = d.BookAdhocCarrStartUnloadingDate _
                                            , .BookAdhocCarrStartUnloadingTime = d.BookAdhocCarrStartUnloadingTime _
                                            , .BookAdhocCarrFinishUnloadingDate = d.BookAdhocCarrFinishUnloadingDate _
                                            , .BookAdhocCarrFinishUnloadingTime = d.BookAdhocCarrFinishUnloadingTime _
                                            , .BookAdhocOrderSequence = d.BookAdhocOrderSequence _
                                            , .BookAdhocChepGLID = d.BookAdhocChepGLID _
                                            , .BookAdhocCarrierTypeCode = d.BookAdhocCarrierTypeCode _
                                            , .BookAdhocPalletPositions = d.BookAdhocPalletPositions _
                                            , .BookAdhocShipCarrierProNumber = d.BookAdhocShipCarrierProNumber _
                                            , .BookAdhocShipCarrierName = d.BookAdhocShipCarrierName _
                                            , .BookAdhocShipCarrierNumber = d.BookAdhocShipCarrierNumber _
                                            , .BookAdhocAPAdjReasonControl = d.BookAdhocAPAdjReasonControl _
                                            , .BookAdhocDateRequested = d.BookAdhocDateRequested _
                                            , .BookAdhocCarrierEquipmentCodes = d.BookAdhocCarrierEquipmentCodes _
                                            , .BookAdhocLockAllCosts = d.BookAdhocLockAllCosts _
                                            , .BookAdhocLockBFCCost = d.BookAdhocLockBFCCost _
                                            , .BookAdhocDestStopNumber = d.BookAdhocDestStopNumber _
                                            , .BookAdhocOrigStopNumber = d.BookAdhocOrigStopNumber _
                                            , .BookAdhocDestStopControl = d.BookAdhocDestStopControl _
                                            , .BookAdhocOrigStopControl = d.BookAdhocOrigStopControl _
                                            , .BookAdhocRouteTypeCode = d.BookAdhocRouteTypeCode _
                                            , .BookAdhocAlternateAddressLaneControl = d.BookAdhocAlternateAddressLaneControl _
                                            , .BookAdhocAlternateAddressLaneNumber = d.BookAdhocAlternateAddressLaneNumber _
                                            , .BookAdhocDefaultRouteSequence = d.BookAdhocDefaultRouteSequence _
                                            , .BookAdhocRouteGuideControl = d.BookAdhocRouteGuideControl _
                                            , .BookAdhocRouteGuideNumber = d.BookAdhocRouteGuideNumber _
                                            , .BookAdhocCustomerApprovalRecieved = d.BookAdhocCustomerApprovalRecieved _
                                            , .BookAdhocCustomerApprovalTransmitted = d.BookAdhocCustomerApprovalTransmitted _
                                            , .BookAdhocAMSPickupApptControl = d.BookAdhocAMSPickupApptControl _
                                            , .BookAdhocAMSDeliveryApptControl = d.BookAdhocAMSDeliveryApptControl _
                                            , .BookAdhocItemDetailDescription = d.BookAdhocItemDetailDescription _
                                            , .BookAdhocModDate = d.BookAdhocModDate _
                                            , .BookAdhocModUser = d.BookAdhocModUser _
                                            , .BookAdhocUpdated = d.BookAdhocUpdated.ToArray}).First

                Return BookAdhoc

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookAdhocsFiltered(Optional ByVal BookAdhocConsPrefix As String = "",
                                          Optional ByVal BookAdhocCarrOrderNumber As String = "") As DTO.BookAdhoc()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try


                'Get the newest record that matches the provided criteria
                Dim BookAdhocs() As DTO.BookAdhoc = (
                From d In db.BookAdhocs
                Where
                    (BookAdhocConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookAdhocConsPrefix) OrElse d.BookAdhocConsPrefix = BookAdhocConsPrefix) _
                    And
                    (BookAdhocCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookAdhocCarrOrderNumber) OrElse d.BookAdhocCarrOrderNumber = BookAdhocCarrOrderNumber)
                Order By d.BookAdhocStopNo Ascending
                Select New DTO.BookAdhoc With {.BookAdhocControl = d.BookAdhocControl _
                                            , .BookAdhocProNumber = d.BookAdhocProNumber _
                                            , .BookAdhocProBase = d.BookAdhocProBase _
                                            , .BookAdhocConsPrefix = d.BookAdhocConsPrefix _
                                            , .BookAdhocCustCompControl = d.BookAdhocCustCompControl _
                                            , .BookAdhocCommCompControl = d.BookAdhocCommCompControl _
                                            , .BookAdhocODControl = d.BookAdhocODControl _
                                            , .BookAdhocCarrierControl = d.BookAdhocCarrierControl _
                                            , .BookAdhocCarrierContact = d.BookAdhocCarrierContact _
                                            , .BookAdhocCarrierContactPhone = d.BookAdhocCarrierContactPhone _
                                            , .BookAdhocOrigCompControl = If(d.BookAdhocOrigCompControl.HasValue, d.BookAdhocOrigCompControl, 0) _
                                            , .BookAdhocOrigName = d.BookAdhocOrigName _
                                            , .BookAdhocOrigAddress1 = d.BookAdhocOrigAddress1 _
                                            , .BookAdhocOrigAddress2 = d.BookAdhocOrigAddress2 _
                                            , .BookAdhocOrigAddress3 = d.BookAdhocOrigAddress3 _
                                            , .BookAdhocOrigCity = d.BookAdhocOrigCity _
                                            , .BookAdhocOrigState = d.BookAdhocOrigState _
                                            , .BookAdhocOrigCountry = d.BookAdhocOrigCountry _
                                            , .BookAdhocOrigZip = d.BookAdhocOrigZip _
                                            , .BookAdhocOrigPhone = d.BookAdhocOrigPhone _
                                            , .BookAdhocOrigFax = d.BookAdhocOrigFax _
                                            , .BookAdhocOriginStartHrs = d.BookAdhocOriginStartHrs _
                                            , .BookAdhocOriginStopHrs = d.BookAdhocOriginStopHrs _
                                            , .BookAdhocOriginApptReq = If(d.BookAdhocOriginApptReq.HasValue, d.BookAdhocOriginApptReq, False) _
                                            , .BookAdhocDestCompControl = If(d.BookAdhocDestCompControl.HasValue, d.BookAdhocDestCompControl, 0) _
                                            , .BookAdhocDestName = d.BookAdhocDestName _
                                            , .BookAdhocDestAddress1 = d.BookAdhocDestAddress1 _
                                            , .BookAdhocDestAddress2 = d.BookAdhocDestAddress2 _
                                            , .BookAdhocDestAddress3 = d.BookAdhocDestAddress3 _
                                            , .BookAdhocDestCity = d.BookAdhocDestCity _
                                            , .BookAdhocDestState = d.BookAdhocDestState _
                                            , .BookAdhocDestCountry = d.BookAdhocDestCountry _
                                            , .BookAdhocDestZip = d.BookAdhocDestZip _
                                            , .BookAdhocDestPhone = d.BookAdhocDestPhone _
                                            , .BookAdhocDestFax = d.BookAdhocDestFax _
                                            , .BookAdhocDestStartHrs = d.BookAdhocDestStartHrs _
                                            , .BookAdhocDestStopHrs = d.BookAdhocDestStopHrs _
                                            , .BookAdhocDestApptReq = If(d.BookAdhocDestApptReq.HasValue, d.BookAdhocDestApptReq, False) _
                                            , .BookAdhocDateOrdered = d.BookAdhocDateOrdered _
                                            , .BookAdhocDateLoad = d.BookAdhocDateLoad _
                                            , .BookAdhocDateInvoice = d.BookAdhocDateInvoice _
                                            , .BookAdhocDateRequired = d.BookAdhocDateRequired _
                                            , .BookAdhocDateDelivered = d.BookAdhocDateDelivered _
                                            , .BookAdhocTotalCases = If(d.BookAdhocTotalCases.HasValue, d.BookAdhocTotalCases, 0) _
                                            , .BookAdhocTotalWgt = If(d.BookAdhocTotalWgt.HasValue, d.BookAdhocTotalWgt, 0) _
                                            , .BookAdhocTotalPL = If(d.BookAdhocTotalPL.HasValue, d.BookAdhocTotalPL, 0) _
                                            , .BookAdhocTotalCube = If(d.BookAdhocTotalCube.HasValue, d.BookAdhocTotalCube, 0) _
                                            , .BookAdhocTotalPX = If(d.BookAdhocTotalPX.HasValue, d.BookAdhocTotalPX, 0) _
                                            , .BookAdhocTotalBFC = If(d.BookAdhocTotalBFC.HasValue, d.BookAdhocTotalBFC, 0) _
                                            , .BookAdhocTranCode = d.BookAdhocTranCode _
                                            , .BookAdhocPayCode = d.BookAdhocPayCode _
                                            , .BookAdhocTypeCode = d.BookAdhocTypeCode _
                                            , .BookAdhocBOLCode = d.BookAdhocBOLCode _
                                            , .BookAdhocStopNo = If(d.BookAdhocStopNo.HasValue, d.BookAdhocStopNo, 0) _
                                            , .BookAdhocCarrFBNumber = d.BookAdhocCarrFBNumber _
                                            , .BookAdhocCarrOrderNumber = d.BookAdhocCarrOrderNumber _
                                            , .BookAdhocCarrBLNumber = d.BookAdhocCarrBLNumber _
                                            , .BookAdhocCarrBookAdhocDate = d.BookAdhocCarrBookAdhocDate _
                                            , .BookAdhocCarrBookAdhocTime = d.BookAdhocCarrBookAdhocTime _
                                            , .BookAdhocCarrBookAdhocContact = d.BookAdhocCarrBookAdhocContact _
                                            , .BookAdhocCarrScheduleDate = d.BookAdhocCarrScheduleDate _
                                            , .BookAdhocCarrScheduleTime = d.BookAdhocCarrScheduleTime _
                                            , .BookAdhocCarrActualDate = d.BookAdhocCarrActualDate _
                                            , .BookAdhocCarrActualTime = d.BookAdhocCarrActualTime _
                                            , .BookAdhocCarrActLoadComplete_Date = d.BookAdhocCarrActLoadComplete_Date _
                                            , .BookAdhocCarrActLoadCompleteTime = d.BookAdhocCarrActLoadCompleteTime _
                                            , .BookAdhocCarrDockPUAssigment = d.BookAdhocCarrDockPUAssigment _
                                            , .BookAdhocCarrPODate = d.BookAdhocCarrPODate _
                                            , .BookAdhocCarrPOTime = d.BookAdhocCarrPOTime _
                                            , .BookAdhocCarrApptDate = d.BookAdhocCarrApptDate _
                                            , .BookAdhocCarrApptTime = d.BookAdhocCarrApptTime _
                                            , .BookAdhocCarrActDate = d.BookAdhocCarrActDate _
                                            , .BookAdhocCarrActTime = d.BookAdhocCarrActTime _
                                            , .BookAdhocCarrActUnloadCompDate = d.BookAdhocCarrActUnloadCompDate _
                                            , .BookAdhocCarrActUnloadCompTime = d.BookAdhocCarrActUnloadCompTime _
                                            , .BookAdhocCarrDockDelAssignment = d.BookAdhocCarrDockDelAssignment _
                                            , .BookAdhocCarrVarDay = If(d.BookAdhocCarrVarDay.HasValue, d.BookAdhocCarrVarDay, 0) _
                                            , .BookAdhocCarrVarHrs = If(d.BookAdhocCarrVarHrs.HasValue, d.BookAdhocCarrVarHrs, 0) _
                                            , .BookAdhocCarrTrailerNo = d.BookAdhocCarrTrailerNo _
                                            , .BookAdhocCarrSealNo = d.BookAdhocCarrSealNo _
                                            , .BookAdhocCarrDriverNo = d.BookAdhocCarrDriverNo _
                                            , .BookAdhocCarrDriverName = d.BookAdhocCarrDriverName _
                                            , .BookAdhocCarrRouteNo = d.BookAdhocCarrRouteNo _
                                            , .BookAdhocCarrTripNo = d.BookAdhocCarrTripNo _
                                            , .BookAdhocFinARBookAdhocFrt = If(d.BookAdhocFinARBookAdhocFrt.HasValue, d.BookAdhocFinARBookAdhocFrt, 0) _
                                            , .BookAdhocFinARInvoiceDate = d.BookAdhocFinARInvoiceDate _
                                            , .BookAdhocFinARInvoiceAmt = If(d.BookAdhocFinARInvoiceAmt.HasValue, d.BookAdhocFinARInvoiceAmt, 0) _
                                            , .BookAdhocFinARPayDate = d.BookAdhocFinARPayDate _
                                            , .BookAdhocFinARPayAmt = If(d.BookAdhocFinARPayAmt.HasValue, d.BookAdhocFinARPayAmt, 0) _
                                            , .BookAdhocFinARCheck = d.BookAdhocFinARCheck _
                                            , .BookAdhocFinARGLNumber = d.BookAdhocFinARGLNumber _
                                            , .BookAdhocFinARBalance = If(d.BookAdhocFinARBalance.HasValue, d.BookAdhocFinARBalance, 0) _
                                            , .BookAdhocFinARCurType = If(d.BookAdhocFinARCurType.HasValue, d.BookAdhocFinARCurType, 0) _
                                            , .BookAdhocFinAPBillNumber = d.BookAdhocFinAPBillNumber _
                                            , .BookAdhocFinAPBillNoDate = d.BookAdhocFinAPBillNoDate _
                                            , .BookAdhocFinAPBillInvDate = d.BookAdhocFinAPBillInvDate _
                                            , .BookAdhocFinAPActWgt = If(d.BookAdhocFinAPActWgt.HasValue, d.BookAdhocFinAPActWgt, 0) _
                                            , .BookAdhocFinAPStdCost = If(d.BookAdhocFinAPStdCost.HasValue, d.BookAdhocFinAPStdCost, 0) _
                                            , .BookAdhocFinAPActCost = If(d.BookAdhocFinAPActCost.HasValue, d.BookAdhocFinAPActCost, 0) _
                                            , .BookAdhocFinAPPayDate = d.BookAdhocFinAPPayDate _
                                            , .BookAdhocFinAPPayAmt = If(d.BookAdhocFinAPPayAmt.HasValue, d.BookAdhocFinAPPayAmt, 0) _
                                            , .BookAdhocFinAPCheck = d.BookAdhocFinAPCheck _
                                            , .BookAdhocFinAPGLNumber = d.BookAdhocFinAPGLNumber _
                                            , .BookAdhocFinAPLastViewed = d.BookAdhocFinAPLastViewed _
                                            , .BookAdhocFinAPCurType = If(d.BookAdhocFinAPCurType.HasValue, d.BookAdhocFinAPCurType, 0) _
                                            , .BookAdhocFinCommStd = If(d.BookAdhocFinCommStd.HasValue, d.BookAdhocFinCommStd, 0) _
                                            , .BookAdhocFinCommAct = If(d.BookAdhocFinCommAct.HasValue, d.BookAdhocFinCommAct, 0) _
                                            , .BookAdhocFinCommPayDate = d.BookAdhocFinCommPayDate _
                                            , .BookAdhocFinCommPayAmt = If(d.BookAdhocFinCommPayAmt.HasValue, d.BookAdhocFinCommPayAmt, 0) _
                                            , .BookAdhocFinCommtCheck = d.BookAdhocFinCommtCheck _
                                            , .BookAdhocFinCommCreditAmt = If(d.BookAdhocFinCommCreditAmt.HasValue, d.BookAdhocFinCommCreditAmt, 0) _
                                            , .BookAdhocFinCommCreditPayDate = d.BookAdhocFinCommCreditPayDate _
                                            , .BookAdhocFinCommLoadCount = If(d.BookAdhocFinCommLoadCount.HasValue, d.BookAdhocFinCommLoadCount, 0) _
                                            , .BookAdhocFinCommGLNumber = d.BookAdhocFinCommGLNumber _
                                            , .BookAdhocFinCheckClearedDate = d.BookAdhocFinCheckClearedDate _
                                            , .BookAdhocFinCheckClearedNumber = d.BookAdhocFinCheckClearedNumber _
                                            , .BookAdhocFinCheckClearedAmt = If(d.BookAdhocFinCheckClearedAmt.HasValue, d.BookAdhocFinCheckClearedAmt, 0) _
                                            , .BookAdhocFinCheckClearedDesc = d.BookAdhocFinCheckClearedDesc _
                                            , .BookAdhocFinCheckClearedAcct = d.BookAdhocFinCheckClearedAcct _
                                            , .BookAdhocRevBilledBFC = If(d.BookAdhocRevBilledBFC.HasValue, d.BookAdhocRevBilledBFC, 0) _
                                            , .BookAdhocRevCarrierCost = If(d.BookAdhocRevCarrierCost.HasValue, d.BookAdhocRevCarrierCost, 0) _
                                            , .BookAdhocRevStopQty = If(d.BookAdhocRevStopQty.HasValue, d.BookAdhocRevStopQty, 0) _
                                            , .BookAdhocRevStopCost = If(d.BookAdhocRevStopCost.HasValue, d.BookAdhocRevStopCost, 0) _
                                            , .BookAdhocRevOtherCost = If(d.BookAdhocRevOtherCost.HasValue, d.BookAdhocRevOtherCost, 0) _
                                            , .BookAdhocRevTotalCost = If(d.BookAdhocRevTotalCost.HasValue, d.BookAdhocRevTotalCost, 0) _
                                            , .BookAdhocRevLoadSavings = If(d.BookAdhocRevLoadSavings.HasValue, d.BookAdhocRevLoadSavings, 0) _
                                            , .BookAdhocRevCommPercent = If(d.BookAdhocRevCommPercent.HasValue, d.BookAdhocRevCommPercent, 0) _
                                            , .BookAdhocRevCommCost = If(d.BookAdhocRevCommCost.HasValue, d.BookAdhocRevCommCost, 0) _
                                            , .BookAdhocRevGrossRevenue = If(d.BookAdhocRevGrossRevenue.HasValue, d.BookAdhocRevGrossRevenue, 0) _
                                            , .BookAdhocRevNegRevenue = If(d.BookAdhocRevNegRevenue.HasValue, d.BookAdhocRevNegRevenue, 0) _
                                            , .BookAdhocMilesFrom = If(d.BookAdhocMilesFrom.HasValue, d.BookAdhocMilesFrom, 0) _
                                            , .BookAdhocLaneCarrControl = If(d.BookAdhocLaneCarrControl.HasValue, d.BookAdhocLaneCarrControl, 0) _
                                            , .BookAdhocHoldLoad = If(d.BookAdhocHoldLoad.HasValue, d.BookAdhocHoldLoad, 0) _
                                            , .BookAdhocRouteFinalDate = d.BookAdhocRouteFinalDate _
                                            , .BookAdhocRouteFinalCode = d.BookAdhocRouteFinalCode _
                                            , .BookAdhocRouteFinalFlag = d.BookAdhocRouteFinalFlag _
                                            , .BookAdhocWarehouseNumber = d.BookAdhocWarehouseNumber _
                                            , .BookAdhocComCode = d.BookAdhocComCode _
                                            , .BookAdhocTransType = d.BookAdhocTransType _
                                            , .BookAdhocRouteConsFlag = d.BookAdhocRouteConsFlag _
                                            , .BookAdhocWhseAuthorizationNo = d.BookAdhocWhseAuthorizationNo _
                                            , .BookAdhocHotLoad = If(d.BookAdhocHotLoad.HasValue, d.BookAdhocHotLoad, False) _
                                            , .BookAdhocFinAPActTax = If(d.BookAdhocFinAPActTax.HasValue, d.BookAdhocFinAPActTax, 0) _
                                            , .BookAdhocFinAPExportFlag = d.BookAdhocFinAPExportFlag _
                                            , .BookAdhocFinARFreightTax = d.BookAdhocFinARFreightTax _
                                            , .BookAdhocRevFreightTax = d.BookAdhocRevFreightTax _
                                            , .BookAdhocRevNetCost = d.BookAdhocRevNetCost _
                                            , .BookAdhocFinServiceFee = If(d.BookAdhocFinServiceFee.HasValue, d.BookAdhocFinServiceFee, 0) _
                                            , .BookAdhocFinAPExportDate = d.BookAdhocFinAPExportDate _
                                            , .BookAdhocFinAPExportRetry = If(d.BookAdhocFinAPExportRetry.HasValue, d.BookAdhocFinAPExportRetry, 0) _
                                            , .BookAdhocCarrierContControl = If(d.BookAdhocCarrierContControl.HasValue, d.BookAdhocCarrierContControl, 0) _
                                            , .BookAdhocHotLoadSent = If(d.BookAdhocHotLoadSent.HasValue, d.BookAdhocHotLoadSent, False) _
                                            , .BookAdhocExportDocCreated = If(d.BookAdhocExportDocCreated.HasValue, d.BookAdhocExportDocCreated, False) _
                                            , .BookAdhocDoNotInvoice = d.BookAdhocDoNotInvoice _
                                            , .BookAdhocCarrStartLoadingDate = d.BookAdhocCarrStartLoadingDate _
                                            , .BookAdhocCarrStartLoadingTime = d.BookAdhocCarrStartLoadingTime _
                                            , .BookAdhocCarrFinishLoadingDate = d.BookAdhocCarrFinishLoadingDate _
                                            , .BookAdhocCarrFinishLoadingTime = d.BookAdhocCarrFinishLoadingTime _
                                            , .BookAdhocCarrStartUnloadingDate = d.BookAdhocCarrStartUnloadingDate _
                                            , .BookAdhocCarrStartUnloadingTime = d.BookAdhocCarrStartUnloadingTime _
                                            , .BookAdhocCarrFinishUnloadingDate = d.BookAdhocCarrFinishUnloadingDate _
                                            , .BookAdhocCarrFinishUnloadingTime = d.BookAdhocCarrFinishUnloadingTime _
                                            , .BookAdhocOrderSequence = d.BookAdhocOrderSequence _
                                            , .BookAdhocChepGLID = d.BookAdhocChepGLID _
                                            , .BookAdhocCarrierTypeCode = d.BookAdhocCarrierTypeCode _
                                            , .BookAdhocPalletPositions = d.BookAdhocPalletPositions _
                                            , .BookAdhocShipCarrierProNumber = d.BookAdhocShipCarrierProNumber _
                                            , .BookAdhocShipCarrierName = d.BookAdhocShipCarrierName _
                                            , .BookAdhocShipCarrierNumber = d.BookAdhocShipCarrierNumber _
                                            , .BookAdhocAPAdjReasonControl = d.BookAdhocAPAdjReasonControl _
                                            , .BookAdhocDateRequested = d.BookAdhocDateRequested _
                                            , .BookAdhocCarrierEquipmentCodes = d.BookAdhocCarrierEquipmentCodes _
                                            , .BookAdhocLockAllCosts = d.BookAdhocLockAllCosts _
                                            , .BookAdhocLockBFCCost = d.BookAdhocLockBFCCost _
                                            , .BookAdhocDestStopNumber = d.BookAdhocDestStopNumber _
                                            , .BookAdhocOrigStopNumber = d.BookAdhocOrigStopNumber _
                                            , .BookAdhocDestStopControl = d.BookAdhocDestStopControl _
                                            , .BookAdhocOrigStopControl = d.BookAdhocOrigStopControl _
                                            , .BookAdhocRouteTypeCode = d.BookAdhocRouteTypeCode _
                                            , .BookAdhocAlternateAddressLaneControl = d.BookAdhocAlternateAddressLaneControl _
                                            , .BookAdhocAlternateAddressLaneNumber = d.BookAdhocAlternateAddressLaneNumber _
                                            , .BookAdhocDefaultRouteSequence = d.BookAdhocDefaultRouteSequence _
                                            , .BookAdhocRouteGuideControl = d.BookAdhocRouteGuideControl _
                                            , .BookAdhocRouteGuideNumber = d.BookAdhocRouteGuideNumber _
                                            , .BookAdhocCustomerApprovalRecieved = d.BookAdhocCustomerApprovalRecieved _
                                            , .BookAdhocCustomerApprovalTransmitted = d.BookAdhocCustomerApprovalTransmitted _
                                            , .BookAdhocAMSPickupApptControl = d.BookAdhocAMSPickupApptControl _
                                            , .BookAdhocAMSDeliveryApptControl = d.BookAdhocAMSDeliveryApptControl _
                                            , .BookAdhocItemDetailDescription = d.BookAdhocItemDetailDescription _
                                            , .BookAdhocModDate = d.BookAdhocModDate _
                                            , .BookAdhocModUser = d.BookAdhocModUser _
                                            , .BookAdhocUpdated = d.BookAdhocUpdated.ToArray}).Take(20).ToArray()

                Return BookAdhocs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookAdhocsByPONumber(ByVal BookAdhocLoadPONumber As String) As DTO.BookAdhoc()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try

                If BookAdhocLoadPONumber Is Nothing OrElse String.IsNullOrEmpty(BookAdhocLoadPONumber) Then Return Nothing
                Dim oBLs = From bl In db.BookAdhocLoads Where bl.BookAdhocLoadPONumber = BookAdhocLoadPONumber Select bl.BookAdhocLoadBookAdhocControl

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.BookAdhoc)(Function(t As LTS.BookAdhoc) t.BookAdhocLoads)
                oDLO.LoadWith(Of LTS.BookAdhocLoad)(Function(t As LTS.BookAdhocLoad) t.BookAdhocItems)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim BookAdhocs() As DTO.BookAdhoc = (
                From d In db.BookAdhocs
                Where oBLs.Contains(d.BookAdhocControl)
                Order By d.BookAdhocStopNo Ascending
                Select New DTO.BookAdhoc With {.BookAdhocControl = d.BookAdhocControl _
                                            , .BookAdhocProNumber = d.BookAdhocProNumber _
                                            , .BookAdhocProBase = d.BookAdhocProBase _
                                            , .BookAdhocConsPrefix = d.BookAdhocConsPrefix _
                                            , .BookAdhocCustCompControl = d.BookAdhocCustCompControl _
                                            , .BookAdhocCommCompControl = d.BookAdhocCommCompControl _
                                            , .BookAdhocODControl = d.BookAdhocODControl _
                                            , .BookAdhocCarrierControl = d.BookAdhocCarrierControl _
                                            , .BookAdhocCarrierContact = d.BookAdhocCarrierContact _
                                            , .BookAdhocCarrierContactPhone = d.BookAdhocCarrierContactPhone _
                                            , .BookAdhocOrigCompControl = If(d.BookAdhocOrigCompControl.HasValue, d.BookAdhocOrigCompControl, 0) _
                                            , .BookAdhocOrigName = d.BookAdhocOrigName _
                                            , .BookAdhocOrigAddress1 = d.BookAdhocOrigAddress1 _
                                            , .BookAdhocOrigAddress2 = d.BookAdhocOrigAddress2 _
                                            , .BookAdhocOrigAddress3 = d.BookAdhocOrigAddress3 _
                                            , .BookAdhocOrigCity = d.BookAdhocOrigCity _
                                            , .BookAdhocOrigState = d.BookAdhocOrigState _
                                            , .BookAdhocOrigCountry = d.BookAdhocOrigCountry _
                                            , .BookAdhocOrigZip = d.BookAdhocOrigZip _
                                            , .BookAdhocOrigPhone = d.BookAdhocOrigPhone _
                                            , .BookAdhocOrigFax = d.BookAdhocOrigFax _
                                            , .BookAdhocOriginStartHrs = d.BookAdhocOriginStartHrs _
                                            , .BookAdhocOriginStopHrs = d.BookAdhocOriginStopHrs _
                                            , .BookAdhocOriginApptReq = If(d.BookAdhocOriginApptReq.HasValue, d.BookAdhocOriginApptReq, False) _
                                            , .BookAdhocDestCompControl = If(d.BookAdhocDestCompControl.HasValue, d.BookAdhocDestCompControl, 0) _
                                            , .BookAdhocDestName = d.BookAdhocDestName _
                                            , .BookAdhocDestAddress1 = d.BookAdhocDestAddress1 _
                                            , .BookAdhocDestAddress2 = d.BookAdhocDestAddress2 _
                                            , .BookAdhocDestAddress3 = d.BookAdhocDestAddress3 _
                                            , .BookAdhocDestCity = d.BookAdhocDestCity _
                                            , .BookAdhocDestState = d.BookAdhocDestState _
                                            , .BookAdhocDestCountry = d.BookAdhocDestCountry _
                                            , .BookAdhocDestZip = d.BookAdhocDestZip _
                                            , .BookAdhocDestPhone = d.BookAdhocDestPhone _
                                            , .BookAdhocDestFax = d.BookAdhocDestFax _
                                            , .BookAdhocDestStartHrs = d.BookAdhocDestStartHrs _
                                            , .BookAdhocDestStopHrs = d.BookAdhocDestStopHrs _
                                            , .BookAdhocDestApptReq = If(d.BookAdhocDestApptReq.HasValue, d.BookAdhocDestApptReq, False) _
                                            , .BookAdhocDateOrdered = d.BookAdhocDateOrdered _
                                            , .BookAdhocDateLoad = d.BookAdhocDateLoad _
                                            , .BookAdhocDateInvoice = d.BookAdhocDateInvoice _
                                            , .BookAdhocDateRequired = d.BookAdhocDateRequired _
                                            , .BookAdhocDateDelivered = d.BookAdhocDateDelivered _
                                            , .BookAdhocTotalCases = If(d.BookAdhocTotalCases.HasValue, d.BookAdhocTotalCases, 0) _
                                            , .BookAdhocTotalWgt = If(d.BookAdhocTotalWgt.HasValue, d.BookAdhocTotalWgt, 0) _
                                            , .BookAdhocTotalPL = If(d.BookAdhocTotalPL.HasValue, d.BookAdhocTotalPL, 0) _
                                            , .BookAdhocTotalCube = If(d.BookAdhocTotalCube.HasValue, d.BookAdhocTotalCube, 0) _
                                            , .BookAdhocTotalPX = If(d.BookAdhocTotalPX.HasValue, d.BookAdhocTotalPX, 0) _
                                            , .BookAdhocTotalBFC = If(d.BookAdhocTotalBFC.HasValue, d.BookAdhocTotalBFC, 0) _
                                            , .BookAdhocTranCode = d.BookAdhocTranCode _
                                            , .BookAdhocPayCode = d.BookAdhocPayCode _
                                            , .BookAdhocTypeCode = d.BookAdhocTypeCode _
                                            , .BookAdhocBOLCode = d.BookAdhocBOLCode _
                                            , .BookAdhocStopNo = If(d.BookAdhocStopNo.HasValue, d.BookAdhocStopNo, 0) _
                                            , .BookAdhocCarrFBNumber = d.BookAdhocCarrFBNumber _
                                            , .BookAdhocCarrOrderNumber = d.BookAdhocCarrOrderNumber _
                                            , .BookAdhocCarrBLNumber = d.BookAdhocCarrBLNumber _
                                            , .BookAdhocCarrBookAdhocDate = d.BookAdhocCarrBookAdhocDate _
                                            , .BookAdhocCarrBookAdhocTime = d.BookAdhocCarrBookAdhocTime _
                                            , .BookAdhocCarrBookAdhocContact = d.BookAdhocCarrBookAdhocContact _
                                            , .BookAdhocCarrScheduleDate = d.BookAdhocCarrScheduleDate _
                                            , .BookAdhocCarrScheduleTime = d.BookAdhocCarrScheduleTime _
                                            , .BookAdhocCarrActualDate = d.BookAdhocCarrActualDate _
                                            , .BookAdhocCarrActualTime = d.BookAdhocCarrActualTime _
                                            , .BookAdhocCarrActLoadComplete_Date = d.BookAdhocCarrActLoadComplete_Date _
                                            , .BookAdhocCarrActLoadCompleteTime = d.BookAdhocCarrActLoadCompleteTime _
                                            , .BookAdhocCarrDockPUAssigment = d.BookAdhocCarrDockPUAssigment _
                                            , .BookAdhocCarrPODate = d.BookAdhocCarrPODate _
                                            , .BookAdhocCarrPOTime = d.BookAdhocCarrPOTime _
                                            , .BookAdhocCarrApptDate = d.BookAdhocCarrApptDate _
                                            , .BookAdhocCarrApptTime = d.BookAdhocCarrApptTime _
                                            , .BookAdhocCarrActDate = d.BookAdhocCarrActDate _
                                            , .BookAdhocCarrActTime = d.BookAdhocCarrActTime _
                                            , .BookAdhocCarrActUnloadCompDate = d.BookAdhocCarrActUnloadCompDate _
                                            , .BookAdhocCarrActUnloadCompTime = d.BookAdhocCarrActUnloadCompTime _
                                            , .BookAdhocCarrDockDelAssignment = d.BookAdhocCarrDockDelAssignment _
                                            , .BookAdhocCarrVarDay = If(d.BookAdhocCarrVarDay.HasValue, d.BookAdhocCarrVarDay, 0) _
                                            , .BookAdhocCarrVarHrs = If(d.BookAdhocCarrVarHrs.HasValue, d.BookAdhocCarrVarHrs, 0) _
                                            , .BookAdhocCarrTrailerNo = d.BookAdhocCarrTrailerNo _
                                            , .BookAdhocCarrSealNo = d.BookAdhocCarrSealNo _
                                            , .BookAdhocCarrDriverNo = d.BookAdhocCarrDriverNo _
                                            , .BookAdhocCarrDriverName = d.BookAdhocCarrDriverName _
                                            , .BookAdhocCarrRouteNo = d.BookAdhocCarrRouteNo _
                                            , .BookAdhocCarrTripNo = d.BookAdhocCarrTripNo _
                                            , .BookAdhocFinARBookAdhocFrt = If(d.BookAdhocFinARBookAdhocFrt.HasValue, d.BookAdhocFinARBookAdhocFrt, 0) _
                                            , .BookAdhocFinARInvoiceDate = d.BookAdhocFinARInvoiceDate _
                                            , .BookAdhocFinARInvoiceAmt = If(d.BookAdhocFinARInvoiceAmt.HasValue, d.BookAdhocFinARInvoiceAmt, 0) _
                                            , .BookAdhocFinARPayDate = d.BookAdhocFinARPayDate _
                                            , .BookAdhocFinARPayAmt = If(d.BookAdhocFinARPayAmt.HasValue, d.BookAdhocFinARPayAmt, 0) _
                                            , .BookAdhocFinARCheck = d.BookAdhocFinARCheck _
                                            , .BookAdhocFinARGLNumber = d.BookAdhocFinARGLNumber _
                                            , .BookAdhocFinARBalance = If(d.BookAdhocFinARBalance.HasValue, d.BookAdhocFinARBalance, 0) _
                                            , .BookAdhocFinARCurType = If(d.BookAdhocFinARCurType.HasValue, d.BookAdhocFinARCurType, 0) _
                                            , .BookAdhocFinAPBillNumber = d.BookAdhocFinAPBillNumber _
                                            , .BookAdhocFinAPBillNoDate = d.BookAdhocFinAPBillNoDate _
                                            , .BookAdhocFinAPBillInvDate = d.BookAdhocFinAPBillInvDate _
                                            , .BookAdhocFinAPActWgt = If(d.BookAdhocFinAPActWgt.HasValue, d.BookAdhocFinAPActWgt, 0) _
                                            , .BookAdhocFinAPStdCost = If(d.BookAdhocFinAPStdCost.HasValue, d.BookAdhocFinAPStdCost, 0) _
                                            , .BookAdhocFinAPActCost = If(d.BookAdhocFinAPActCost.HasValue, d.BookAdhocFinAPActCost, 0) _
                                            , .BookAdhocFinAPPayDate = d.BookAdhocFinAPPayDate _
                                            , .BookAdhocFinAPPayAmt = If(d.BookAdhocFinAPPayAmt.HasValue, d.BookAdhocFinAPPayAmt, 0) _
                                            , .BookAdhocFinAPCheck = d.BookAdhocFinAPCheck _
                                            , .BookAdhocFinAPGLNumber = d.BookAdhocFinAPGLNumber _
                                            , .BookAdhocFinAPLastViewed = d.BookAdhocFinAPLastViewed _
                                            , .BookAdhocFinAPCurType = If(d.BookAdhocFinAPCurType.HasValue, d.BookAdhocFinAPCurType, 0) _
                                            , .BookAdhocFinCommStd = If(d.BookAdhocFinCommStd.HasValue, d.BookAdhocFinCommStd, 0) _
                                            , .BookAdhocFinCommAct = If(d.BookAdhocFinCommAct.HasValue, d.BookAdhocFinCommAct, 0) _
                                            , .BookAdhocFinCommPayDate = d.BookAdhocFinCommPayDate _
                                            , .BookAdhocFinCommPayAmt = If(d.BookAdhocFinCommPayAmt.HasValue, d.BookAdhocFinCommPayAmt, 0) _
                                            , .BookAdhocFinCommtCheck = d.BookAdhocFinCommtCheck _
                                            , .BookAdhocFinCommCreditAmt = If(d.BookAdhocFinCommCreditAmt.HasValue, d.BookAdhocFinCommCreditAmt, 0) _
                                            , .BookAdhocFinCommCreditPayDate = d.BookAdhocFinCommCreditPayDate _
                                            , .BookAdhocFinCommLoadCount = If(d.BookAdhocFinCommLoadCount.HasValue, d.BookAdhocFinCommLoadCount, 0) _
                                            , .BookAdhocFinCommGLNumber = d.BookAdhocFinCommGLNumber _
                                            , .BookAdhocFinCheckClearedDate = d.BookAdhocFinCheckClearedDate _
                                            , .BookAdhocFinCheckClearedNumber = d.BookAdhocFinCheckClearedNumber _
                                            , .BookAdhocFinCheckClearedAmt = If(d.BookAdhocFinCheckClearedAmt.HasValue, d.BookAdhocFinCheckClearedAmt, 0) _
                                            , .BookAdhocFinCheckClearedDesc = d.BookAdhocFinCheckClearedDesc _
                                            , .BookAdhocFinCheckClearedAcct = d.BookAdhocFinCheckClearedAcct _
                                            , .BookAdhocRevBilledBFC = If(d.BookAdhocRevBilledBFC.HasValue, d.BookAdhocRevBilledBFC, 0) _
                                            , .BookAdhocRevCarrierCost = If(d.BookAdhocRevCarrierCost.HasValue, d.BookAdhocRevCarrierCost, 0) _
                                            , .BookAdhocRevStopQty = If(d.BookAdhocRevStopQty.HasValue, d.BookAdhocRevStopQty, 0) _
                                            , .BookAdhocRevStopCost = If(d.BookAdhocRevStopCost.HasValue, d.BookAdhocRevStopCost, 0) _
                                            , .BookAdhocRevOtherCost = If(d.BookAdhocRevOtherCost.HasValue, d.BookAdhocRevOtherCost, 0) _
                                            , .BookAdhocRevTotalCost = If(d.BookAdhocRevTotalCost.HasValue, d.BookAdhocRevTotalCost, 0) _
                                            , .BookAdhocRevLoadSavings = If(d.BookAdhocRevLoadSavings.HasValue, d.BookAdhocRevLoadSavings, 0) _
                                            , .BookAdhocRevCommPercent = If(d.BookAdhocRevCommPercent.HasValue, d.BookAdhocRevCommPercent, 0) _
                                            , .BookAdhocRevCommCost = If(d.BookAdhocRevCommCost.HasValue, d.BookAdhocRevCommCost, 0) _
                                            , .BookAdhocRevGrossRevenue = If(d.BookAdhocRevGrossRevenue.HasValue, d.BookAdhocRevGrossRevenue, 0) _
                                            , .BookAdhocRevNegRevenue = If(d.BookAdhocRevNegRevenue.HasValue, d.BookAdhocRevNegRevenue, 0) _
                                            , .BookAdhocMilesFrom = If(d.BookAdhocMilesFrom.HasValue, d.BookAdhocMilesFrom, 0) _
                                            , .BookAdhocLaneCarrControl = If(d.BookAdhocLaneCarrControl.HasValue, d.BookAdhocLaneCarrControl, 0) _
                                            , .BookAdhocHoldLoad = If(d.BookAdhocHoldLoad.HasValue, d.BookAdhocHoldLoad, 0) _
                                            , .BookAdhocRouteFinalDate = d.BookAdhocRouteFinalDate _
                                            , .BookAdhocRouteFinalCode = d.BookAdhocRouteFinalCode _
                                            , .BookAdhocRouteFinalFlag = d.BookAdhocRouteFinalFlag _
                                            , .BookAdhocWarehouseNumber = d.BookAdhocWarehouseNumber _
                                            , .BookAdhocComCode = d.BookAdhocComCode _
                                            , .BookAdhocTransType = d.BookAdhocTransType _
                                            , .BookAdhocRouteConsFlag = d.BookAdhocRouteConsFlag _
                                            , .BookAdhocWhseAuthorizationNo = d.BookAdhocWhseAuthorizationNo _
                                            , .BookAdhocHotLoad = If(d.BookAdhocHotLoad.HasValue, d.BookAdhocHotLoad, False) _
                                            , .BookAdhocFinAPActTax = If(d.BookAdhocFinAPActTax.HasValue, d.BookAdhocFinAPActTax, 0) _
                                            , .BookAdhocFinAPExportFlag = d.BookAdhocFinAPExportFlag _
                                            , .BookAdhocFinARFreightTax = d.BookAdhocFinARFreightTax _
                                            , .BookAdhocRevFreightTax = d.BookAdhocRevFreightTax _
                                            , .BookAdhocRevNetCost = d.BookAdhocRevNetCost _
                                            , .BookAdhocFinServiceFee = If(d.BookAdhocFinServiceFee.HasValue, d.BookAdhocFinServiceFee, 0) _
                                            , .BookAdhocFinAPExportDate = d.BookAdhocFinAPExportDate _
                                            , .BookAdhocFinAPExportRetry = If(d.BookAdhocFinAPExportRetry.HasValue, d.BookAdhocFinAPExportRetry, 0) _
                                            , .BookAdhocCarrierContControl = If(d.BookAdhocCarrierContControl.HasValue, d.BookAdhocCarrierContControl, 0) _
                                            , .BookAdhocHotLoadSent = If(d.BookAdhocHotLoadSent.HasValue, d.BookAdhocHotLoadSent, False) _
                                            , .BookAdhocExportDocCreated = If(d.BookAdhocExportDocCreated.HasValue, d.BookAdhocExportDocCreated, False) _
                                            , .BookAdhocDoNotInvoice = d.BookAdhocDoNotInvoice _
                                            , .BookAdhocCarrStartLoadingDate = d.BookAdhocCarrStartLoadingDate _
                                            , .BookAdhocCarrStartLoadingTime = d.BookAdhocCarrStartLoadingTime _
                                            , .BookAdhocCarrFinishLoadingDate = d.BookAdhocCarrFinishLoadingDate _
                                            , .BookAdhocCarrFinishLoadingTime = d.BookAdhocCarrFinishLoadingTime _
                                            , .BookAdhocCarrStartUnloadingDate = d.BookAdhocCarrStartUnloadingDate _
                                            , .BookAdhocCarrStartUnloadingTime = d.BookAdhocCarrStartUnloadingTime _
                                            , .BookAdhocCarrFinishUnloadingDate = d.BookAdhocCarrFinishUnloadingDate _
                                            , .BookAdhocCarrFinishUnloadingTime = d.BookAdhocCarrFinishUnloadingTime _
                                            , .BookAdhocOrderSequence = d.BookAdhocOrderSequence _
                                            , .BookAdhocChepGLID = d.BookAdhocChepGLID _
                                            , .BookAdhocCarrierTypeCode = d.BookAdhocCarrierTypeCode _
                                            , .BookAdhocPalletPositions = d.BookAdhocPalletPositions _
                                            , .BookAdhocShipCarrierProNumber = d.BookAdhocShipCarrierProNumber _
                                            , .BookAdhocShipCarrierName = d.BookAdhocShipCarrierName _
                                            , .BookAdhocShipCarrierNumber = d.BookAdhocShipCarrierNumber _
                                            , .BookAdhocAPAdjReasonControl = d.BookAdhocAPAdjReasonControl _
                                            , .BookAdhocDateRequested = d.BookAdhocDateRequested _
                                            , .BookAdhocCarrierEquipmentCodes = d.BookAdhocCarrierEquipmentCodes _
                                            , .BookAdhocLockAllCosts = d.BookAdhocLockAllCosts _
                                            , .BookAdhocLockBFCCost = d.BookAdhocLockBFCCost _
                                            , .BookAdhocDestStopNumber = d.BookAdhocDestStopNumber _
                                            , .BookAdhocOrigStopNumber = d.BookAdhocOrigStopNumber _
                                            , .BookAdhocDestStopControl = d.BookAdhocDestStopControl _
                                            , .BookAdhocOrigStopControl = d.BookAdhocOrigStopControl _
                                            , .BookAdhocRouteTypeCode = d.BookAdhocRouteTypeCode _
                                            , .BookAdhocAlternateAddressLaneControl = d.BookAdhocAlternateAddressLaneControl _
                                            , .BookAdhocAlternateAddressLaneNumber = d.BookAdhocAlternateAddressLaneNumber _
                                            , .BookAdhocDefaultRouteSequence = d.BookAdhocDefaultRouteSequence _
                                            , .BookAdhocRouteGuideControl = d.BookAdhocRouteGuideControl _
                                            , .BookAdhocRouteGuideNumber = d.BookAdhocRouteGuideNumber _
                                            , .BookAdhocCustomerApprovalRecieved = d.BookAdhocCustomerApprovalRecieved _
                                            , .BookAdhocCustomerApprovalTransmitted = d.BookAdhocCustomerApprovalTransmitted _
                                            , .BookAdhocAMSPickupApptControl = d.BookAdhocAMSPickupApptControl _
                                            , .BookAdhocAMSDeliveryApptControl = d.BookAdhocAMSDeliveryApptControl _
                                            , .BookAdhocItemDetailDescription = d.BookAdhocItemDetailDescription _
                                            , .BookAdhocModDate = d.BookAdhocModDate _
                                            , .BookAdhocModUser = d.BookAdhocModUser _
                                            , .BookAdhocUpdated = d.BookAdhocUpdated.ToArray}).Take(20).ToArray()

                Return BookAdhocs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookAdhocsFilteredContains(Optional ByVal BookAdhocProNumber As String = "",
                                          Optional ByVal BookAdhocConsPrefix As String = "",
                                          Optional ByVal BookAdhocCarrOrderNumber As String = "") As DTO.BookAdhoc()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim BookAdhocs() As DTO.BookAdhoc = (
                From d In db.BookAdhocs
                Where
                    (BookAdhocProNumber Is Nothing OrElse String.IsNullOrEmpty(BookAdhocProNumber) OrElse d.BookAdhocProNumber.Contains(BookAdhocProNumber)) _
                    And
                    (BookAdhocConsPrefix Is Nothing OrElse String.IsNullOrEmpty(BookAdhocConsPrefix) OrElse d.BookAdhocConsPrefix.Contains(BookAdhocConsPrefix)) _
                    And
                    (BookAdhocCarrOrderNumber Is Nothing OrElse String.IsNullOrEmpty(BookAdhocCarrOrderNumber) OrElse d.BookAdhocCarrOrderNumber.Contains(BookAdhocCarrOrderNumber))
                Order By d.BookAdhocStopNo Ascending
                Select New DTO.BookAdhoc With {.BookAdhocControl = d.BookAdhocControl _
                                            , .BookAdhocProNumber = d.BookAdhocProNumber _
                                            , .BookAdhocProBase = d.BookAdhocProBase _
                                            , .BookAdhocConsPrefix = d.BookAdhocConsPrefix _
                                            , .BookAdhocCustCompControl = d.BookAdhocCustCompControl _
                                            , .BookAdhocCommCompControl = d.BookAdhocCommCompControl _
                                            , .BookAdhocODControl = d.BookAdhocODControl _
                                            , .BookAdhocCarrierControl = d.BookAdhocCarrierControl _
                                            , .BookAdhocCarrierContact = d.BookAdhocCarrierContact _
                                            , .BookAdhocCarrierContactPhone = d.BookAdhocCarrierContactPhone _
                                            , .BookAdhocOrigCompControl = If(d.BookAdhocOrigCompControl.HasValue, d.BookAdhocOrigCompControl, 0) _
                                            , .BookAdhocOrigName = d.BookAdhocOrigName _
                                            , .BookAdhocOrigAddress1 = d.BookAdhocOrigAddress1 _
                                            , .BookAdhocOrigAddress2 = d.BookAdhocOrigAddress2 _
                                            , .BookAdhocOrigAddress3 = d.BookAdhocOrigAddress3 _
                                            , .BookAdhocOrigCity = d.BookAdhocOrigCity _
                                            , .BookAdhocOrigState = d.BookAdhocOrigState _
                                            , .BookAdhocOrigCountry = d.BookAdhocOrigCountry _
                                            , .BookAdhocOrigZip = d.BookAdhocOrigZip _
                                            , .BookAdhocOrigPhone = d.BookAdhocOrigPhone _
                                            , .BookAdhocOrigFax = d.BookAdhocOrigFax _
                                            , .BookAdhocOriginStartHrs = d.BookAdhocOriginStartHrs _
                                            , .BookAdhocOriginStopHrs = d.BookAdhocOriginStopHrs _
                                            , .BookAdhocOriginApptReq = If(d.BookAdhocOriginApptReq.HasValue, d.BookAdhocOriginApptReq, False) _
                                            , .BookAdhocDestCompControl = If(d.BookAdhocDestCompControl.HasValue, d.BookAdhocDestCompControl, 0) _
                                            , .BookAdhocDestName = d.BookAdhocDestName _
                                            , .BookAdhocDestAddress1 = d.BookAdhocDestAddress1 _
                                            , .BookAdhocDestAddress2 = d.BookAdhocDestAddress2 _
                                            , .BookAdhocDestAddress3 = d.BookAdhocDestAddress3 _
                                            , .BookAdhocDestCity = d.BookAdhocDestCity _
                                            , .BookAdhocDestState = d.BookAdhocDestState _
                                            , .BookAdhocDestCountry = d.BookAdhocDestCountry _
                                            , .BookAdhocDestZip = d.BookAdhocDestZip _
                                            , .BookAdhocDestPhone = d.BookAdhocDestPhone _
                                            , .BookAdhocDestFax = d.BookAdhocDestFax _
                                            , .BookAdhocDestStartHrs = d.BookAdhocDestStartHrs _
                                            , .BookAdhocDestStopHrs = d.BookAdhocDestStopHrs _
                                            , .BookAdhocDestApptReq = If(d.BookAdhocDestApptReq.HasValue, d.BookAdhocDestApptReq, False) _
                                            , .BookAdhocDateOrdered = d.BookAdhocDateOrdered _
                                            , .BookAdhocDateLoad = d.BookAdhocDateLoad _
                                            , .BookAdhocDateInvoice = d.BookAdhocDateInvoice _
                                            , .BookAdhocDateRequired = d.BookAdhocDateRequired _
                                            , .BookAdhocDateDelivered = d.BookAdhocDateDelivered _
                                            , .BookAdhocTotalCases = If(d.BookAdhocTotalCases.HasValue, d.BookAdhocTotalCases, 0) _
                                            , .BookAdhocTotalWgt = If(d.BookAdhocTotalWgt.HasValue, d.BookAdhocTotalWgt, 0) _
                                            , .BookAdhocTotalPL = If(d.BookAdhocTotalPL.HasValue, d.BookAdhocTotalPL, 0) _
                                            , .BookAdhocTotalCube = If(d.BookAdhocTotalCube.HasValue, d.BookAdhocTotalCube, 0) _
                                            , .BookAdhocTotalPX = If(d.BookAdhocTotalPX.HasValue, d.BookAdhocTotalPX, 0) _
                                            , .BookAdhocTotalBFC = If(d.BookAdhocTotalBFC.HasValue, d.BookAdhocTotalBFC, 0) _
                                            , .BookAdhocTranCode = d.BookAdhocTranCode _
                                            , .BookAdhocPayCode = d.BookAdhocPayCode _
                                            , .BookAdhocTypeCode = d.BookAdhocTypeCode _
                                            , .BookAdhocBOLCode = d.BookAdhocBOLCode _
                                            , .BookAdhocStopNo = If(d.BookAdhocStopNo.HasValue, d.BookAdhocStopNo, 0) _
                                            , .BookAdhocCarrFBNumber = d.BookAdhocCarrFBNumber _
                                            , .BookAdhocCarrOrderNumber = d.BookAdhocCarrOrderNumber _
                                            , .BookAdhocCarrBLNumber = d.BookAdhocCarrBLNumber _
                                            , .BookAdhocCarrBookAdhocDate = d.BookAdhocCarrBookAdhocDate _
                                            , .BookAdhocCarrBookAdhocTime = d.BookAdhocCarrBookAdhocTime _
                                            , .BookAdhocCarrBookAdhocContact = d.BookAdhocCarrBookAdhocContact _
                                            , .BookAdhocCarrScheduleDate = d.BookAdhocCarrScheduleDate _
                                            , .BookAdhocCarrScheduleTime = d.BookAdhocCarrScheduleTime _
                                            , .BookAdhocCarrActualDate = d.BookAdhocCarrActualDate _
                                            , .BookAdhocCarrActualTime = d.BookAdhocCarrActualTime _
                                            , .BookAdhocCarrActLoadComplete_Date = d.BookAdhocCarrActLoadComplete_Date _
                                            , .BookAdhocCarrActLoadCompleteTime = d.BookAdhocCarrActLoadCompleteTime _
                                            , .BookAdhocCarrDockPUAssigment = d.BookAdhocCarrDockPUAssigment _
                                            , .BookAdhocCarrPODate = d.BookAdhocCarrPODate _
                                            , .BookAdhocCarrPOTime = d.BookAdhocCarrPOTime _
                                            , .BookAdhocCarrApptDate = d.BookAdhocCarrApptDate _
                                            , .BookAdhocCarrApptTime = d.BookAdhocCarrApptTime _
                                            , .BookAdhocCarrActDate = d.BookAdhocCarrActDate _
                                            , .BookAdhocCarrActTime = d.BookAdhocCarrActTime _
                                            , .BookAdhocCarrActUnloadCompDate = d.BookAdhocCarrActUnloadCompDate _
                                            , .BookAdhocCarrActUnloadCompTime = d.BookAdhocCarrActUnloadCompTime _
                                            , .BookAdhocCarrDockDelAssignment = d.BookAdhocCarrDockDelAssignment _
                                            , .BookAdhocCarrVarDay = If(d.BookAdhocCarrVarDay.HasValue, d.BookAdhocCarrVarDay, 0) _
                                            , .BookAdhocCarrVarHrs = If(d.BookAdhocCarrVarHrs.HasValue, d.BookAdhocCarrVarHrs, 0) _
                                            , .BookAdhocCarrTrailerNo = d.BookAdhocCarrTrailerNo _
                                            , .BookAdhocCarrSealNo = d.BookAdhocCarrSealNo _
                                            , .BookAdhocCarrDriverNo = d.BookAdhocCarrDriverNo _
                                            , .BookAdhocCarrDriverName = d.BookAdhocCarrDriverName _
                                            , .BookAdhocCarrRouteNo = d.BookAdhocCarrRouteNo _
                                            , .BookAdhocCarrTripNo = d.BookAdhocCarrTripNo _
                                            , .BookAdhocFinARBookAdhocFrt = If(d.BookAdhocFinARBookAdhocFrt.HasValue, d.BookAdhocFinARBookAdhocFrt, 0) _
                                            , .BookAdhocFinARInvoiceDate = d.BookAdhocFinARInvoiceDate _
                                            , .BookAdhocFinARInvoiceAmt = If(d.BookAdhocFinARInvoiceAmt.HasValue, d.BookAdhocFinARInvoiceAmt, 0) _
                                            , .BookAdhocFinARPayDate = d.BookAdhocFinARPayDate _
                                            , .BookAdhocFinARPayAmt = If(d.BookAdhocFinARPayAmt.HasValue, d.BookAdhocFinARPayAmt, 0) _
                                            , .BookAdhocFinARCheck = d.BookAdhocFinARCheck _
                                            , .BookAdhocFinARGLNumber = d.BookAdhocFinARGLNumber _
                                            , .BookAdhocFinARBalance = If(d.BookAdhocFinARBalance.HasValue, d.BookAdhocFinARBalance, 0) _
                                            , .BookAdhocFinARCurType = If(d.BookAdhocFinARCurType.HasValue, d.BookAdhocFinARCurType, 0) _
                                            , .BookAdhocFinAPBillNumber = d.BookAdhocFinAPBillNumber _
                                            , .BookAdhocFinAPBillNoDate = d.BookAdhocFinAPBillNoDate _
                                            , .BookAdhocFinAPBillInvDate = d.BookAdhocFinAPBillInvDate _
                                            , .BookAdhocFinAPActWgt = If(d.BookAdhocFinAPActWgt.HasValue, d.BookAdhocFinAPActWgt, 0) _
                                            , .BookAdhocFinAPStdCost = If(d.BookAdhocFinAPStdCost.HasValue, d.BookAdhocFinAPStdCost, 0) _
                                            , .BookAdhocFinAPActCost = If(d.BookAdhocFinAPActCost.HasValue, d.BookAdhocFinAPActCost, 0) _
                                            , .BookAdhocFinAPPayDate = d.BookAdhocFinAPPayDate _
                                            , .BookAdhocFinAPPayAmt = If(d.BookAdhocFinAPPayAmt.HasValue, d.BookAdhocFinAPPayAmt, 0) _
                                            , .BookAdhocFinAPCheck = d.BookAdhocFinAPCheck _
                                            , .BookAdhocFinAPGLNumber = d.BookAdhocFinAPGLNumber _
                                            , .BookAdhocFinAPLastViewed = d.BookAdhocFinAPLastViewed _
                                            , .BookAdhocFinAPCurType = If(d.BookAdhocFinAPCurType.HasValue, d.BookAdhocFinAPCurType, 0) _
                                            , .BookAdhocFinCommStd = If(d.BookAdhocFinCommStd.HasValue, d.BookAdhocFinCommStd, 0) _
                                            , .BookAdhocFinCommAct = If(d.BookAdhocFinCommAct.HasValue, d.BookAdhocFinCommAct, 0) _
                                            , .BookAdhocFinCommPayDate = d.BookAdhocFinCommPayDate _
                                            , .BookAdhocFinCommPayAmt = If(d.BookAdhocFinCommPayAmt.HasValue, d.BookAdhocFinCommPayAmt, 0) _
                                            , .BookAdhocFinCommtCheck = d.BookAdhocFinCommtCheck _
                                            , .BookAdhocFinCommCreditAmt = If(d.BookAdhocFinCommCreditAmt.HasValue, d.BookAdhocFinCommCreditAmt, 0) _
                                            , .BookAdhocFinCommCreditPayDate = d.BookAdhocFinCommCreditPayDate _
                                            , .BookAdhocFinCommLoadCount = If(d.BookAdhocFinCommLoadCount.HasValue, d.BookAdhocFinCommLoadCount, 0) _
                                            , .BookAdhocFinCommGLNumber = d.BookAdhocFinCommGLNumber _
                                            , .BookAdhocFinCheckClearedDate = d.BookAdhocFinCheckClearedDate _
                                            , .BookAdhocFinCheckClearedNumber = d.BookAdhocFinCheckClearedNumber _
                                            , .BookAdhocFinCheckClearedAmt = If(d.BookAdhocFinCheckClearedAmt.HasValue, d.BookAdhocFinCheckClearedAmt, 0) _
                                            , .BookAdhocFinCheckClearedDesc = d.BookAdhocFinCheckClearedDesc _
                                            , .BookAdhocFinCheckClearedAcct = d.BookAdhocFinCheckClearedAcct _
                                            , .BookAdhocRevBilledBFC = If(d.BookAdhocRevBilledBFC.HasValue, d.BookAdhocRevBilledBFC, 0) _
                                            , .BookAdhocRevCarrierCost = If(d.BookAdhocRevCarrierCost.HasValue, d.BookAdhocRevCarrierCost, 0) _
                                            , .BookAdhocRevStopQty = If(d.BookAdhocRevStopQty.HasValue, d.BookAdhocRevStopQty, 0) _
                                            , .BookAdhocRevStopCost = If(d.BookAdhocRevStopCost.HasValue, d.BookAdhocRevStopCost, 0) _
                                            , .BookAdhocRevOtherCost = If(d.BookAdhocRevOtherCost.HasValue, d.BookAdhocRevOtherCost, 0) _
                                            , .BookAdhocRevTotalCost = If(d.BookAdhocRevTotalCost.HasValue, d.BookAdhocRevTotalCost, 0) _
                                            , .BookAdhocRevLoadSavings = If(d.BookAdhocRevLoadSavings.HasValue, d.BookAdhocRevLoadSavings, 0) _
                                            , .BookAdhocRevCommPercent = If(d.BookAdhocRevCommPercent.HasValue, d.BookAdhocRevCommPercent, 0) _
                                            , .BookAdhocRevCommCost = If(d.BookAdhocRevCommCost.HasValue, d.BookAdhocRevCommCost, 0) _
                                            , .BookAdhocRevGrossRevenue = If(d.BookAdhocRevGrossRevenue.HasValue, d.BookAdhocRevGrossRevenue, 0) _
                                            , .BookAdhocRevNegRevenue = If(d.BookAdhocRevNegRevenue.HasValue, d.BookAdhocRevNegRevenue, 0) _
                                            , .BookAdhocMilesFrom = If(d.BookAdhocMilesFrom.HasValue, d.BookAdhocMilesFrom, 0) _
                                            , .BookAdhocLaneCarrControl = If(d.BookAdhocLaneCarrControl.HasValue, d.BookAdhocLaneCarrControl, 0) _
                                            , .BookAdhocHoldLoad = If(d.BookAdhocHoldLoad.HasValue, d.BookAdhocHoldLoad, 0) _
                                            , .BookAdhocRouteFinalDate = d.BookAdhocRouteFinalDate _
                                            , .BookAdhocRouteFinalCode = d.BookAdhocRouteFinalCode _
                                            , .BookAdhocRouteFinalFlag = d.BookAdhocRouteFinalFlag _
                                            , .BookAdhocWarehouseNumber = d.BookAdhocWarehouseNumber _
                                            , .BookAdhocComCode = d.BookAdhocComCode _
                                            , .BookAdhocTransType = d.BookAdhocTransType _
                                            , .BookAdhocRouteConsFlag = d.BookAdhocRouteConsFlag _
                                            , .BookAdhocWhseAuthorizationNo = d.BookAdhocWhseAuthorizationNo _
                                            , .BookAdhocHotLoad = If(d.BookAdhocHotLoad.HasValue, d.BookAdhocHotLoad, False) _
                                            , .BookAdhocFinAPActTax = If(d.BookAdhocFinAPActTax.HasValue, d.BookAdhocFinAPActTax, 0) _
                                            , .BookAdhocFinAPExportFlag = d.BookAdhocFinAPExportFlag _
                                            , .BookAdhocFinARFreightTax = d.BookAdhocFinARFreightTax _
                                            , .BookAdhocRevFreightTax = d.BookAdhocRevFreightTax _
                                            , .BookAdhocRevNetCost = d.BookAdhocRevNetCost _
                                            , .BookAdhocFinServiceFee = If(d.BookAdhocFinServiceFee.HasValue, d.BookAdhocFinServiceFee, 0) _
                                            , .BookAdhocFinAPExportDate = d.BookAdhocFinAPExportDate _
                                            , .BookAdhocFinAPExportRetry = If(d.BookAdhocFinAPExportRetry.HasValue, d.BookAdhocFinAPExportRetry, 0) _
                                            , .BookAdhocCarrierContControl = If(d.BookAdhocCarrierContControl.HasValue, d.BookAdhocCarrierContControl, 0) _
                                            , .BookAdhocHotLoadSent = If(d.BookAdhocHotLoadSent.HasValue, d.BookAdhocHotLoadSent, False) _
                                            , .BookAdhocExportDocCreated = If(d.BookAdhocExportDocCreated.HasValue, d.BookAdhocExportDocCreated, False) _
                                            , .BookAdhocDoNotInvoice = d.BookAdhocDoNotInvoice _
                                            , .BookAdhocCarrStartLoadingDate = d.BookAdhocCarrStartLoadingDate _
                                            , .BookAdhocCarrStartLoadingTime = d.BookAdhocCarrStartLoadingTime _
                                            , .BookAdhocCarrFinishLoadingDate = d.BookAdhocCarrFinishLoadingDate _
                                            , .BookAdhocCarrFinishLoadingTime = d.BookAdhocCarrFinishLoadingTime _
                                            , .BookAdhocCarrStartUnloadingDate = d.BookAdhocCarrStartUnloadingDate _
                                            , .BookAdhocCarrStartUnloadingTime = d.BookAdhocCarrStartUnloadingTime _
                                            , .BookAdhocCarrFinishUnloadingDate = d.BookAdhocCarrFinishUnloadingDate _
                                            , .BookAdhocCarrFinishUnloadingTime = d.BookAdhocCarrFinishUnloadingTime _
                                            , .BookAdhocOrderSequence = d.BookAdhocOrderSequence _
                                            , .BookAdhocChepGLID = d.BookAdhocChepGLID _
                                            , .BookAdhocCarrierTypeCode = d.BookAdhocCarrierTypeCode _
                                            , .BookAdhocPalletPositions = d.BookAdhocPalletPositions _
                                            , .BookAdhocShipCarrierProNumber = d.BookAdhocShipCarrierProNumber _
                                            , .BookAdhocShipCarrierName = d.BookAdhocShipCarrierName _
                                            , .BookAdhocShipCarrierNumber = d.BookAdhocShipCarrierNumber _
                                            , .BookAdhocAPAdjReasonControl = d.BookAdhocAPAdjReasonControl _
                                            , .BookAdhocDateRequested = d.BookAdhocDateRequested _
                                            , .BookAdhocCarrierEquipmentCodes = d.BookAdhocCarrierEquipmentCodes _
                                            , .BookAdhocLockAllCosts = d.BookAdhocLockAllCosts _
                                            , .BookAdhocLockBFCCost = d.BookAdhocLockBFCCost _
                                            , .BookAdhocDestStopNumber = d.BookAdhocDestStopNumber _
                                            , .BookAdhocOrigStopNumber = d.BookAdhocOrigStopNumber _
                                            , .BookAdhocDestStopControl = d.BookAdhocDestStopControl _
                                            , .BookAdhocOrigStopControl = d.BookAdhocOrigStopControl _
                                            , .BookAdhocRouteTypeCode = d.BookAdhocRouteTypeCode _
                                            , .BookAdhocAlternateAddressLaneControl = d.BookAdhocAlternateAddressLaneControl _
                                            , .BookAdhocAlternateAddressLaneNumber = d.BookAdhocAlternateAddressLaneNumber _
                                            , .BookAdhocDefaultRouteSequence = d.BookAdhocDefaultRouteSequence _
                                            , .BookAdhocRouteGuideControl = d.BookAdhocRouteGuideControl _
                                            , .BookAdhocRouteGuideNumber = d.BookAdhocRouteGuideNumber _
                                            , .BookAdhocCustomerApprovalRecieved = d.BookAdhocCustomerApprovalRecieved _
                                            , .BookAdhocCustomerApprovalTransmitted = d.BookAdhocCustomerApprovalTransmitted _
                                            , .BookAdhocAMSPickupApptControl = d.BookAdhocAMSPickupApptControl _
                                            , .BookAdhocAMSDeliveryApptControl = d.BookAdhocAMSDeliveryApptControl _
                                            , .BookAdhocItemDetailDescription = d.BookAdhocItemDetailDescription _
                                            , .BookAdhocModDate = d.BookAdhocModDate _
                                            , .BookAdhocModUser = d.BookAdhocModUser _
                                            , .BookAdhocUpdated = d.BookAdhocUpdated.ToArray}).Take(20).ToArray()

                Return BookAdhocs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetBookAdhocsByAppointment(ByVal AMSApptControl As Integer) As DTO.BookAdhoc()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try

                If AMSApptControl = 0 Then Return Nothing

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.BookAdhoc)(Function(t As LTS.BookAdhoc) t.BookAdhocLoads)
                db.LoadOptions = oDLO

                Dim BookAdhocs() As DTO.BookAdhoc = (
                From d In db.BookAdhocs
                Where
                (d.BookAdhocAMSPickupApptControl = AMSApptControl Or d.BookAdhocAMSDeliveryApptControl = AMSApptControl)
                Select New DTO.BookAdhoc With {.BookAdhocControl = d.BookAdhocControl _
                                            , .BookAdhocProNumber = d.BookAdhocProNumber _
                                            , .BookAdhocProBase = d.BookAdhocProBase _
                                            , .BookAdhocConsPrefix = d.BookAdhocConsPrefix _
                                            , .BookAdhocCustCompControl = d.BookAdhocCustCompControl _
                                            , .BookAdhocCommCompControl = d.BookAdhocCommCompControl _
                                            , .BookAdhocODControl = d.BookAdhocODControl _
                                            , .BookAdhocCarrierControl = d.BookAdhocCarrierControl _
                                            , .BookAdhocCarrierContact = d.BookAdhocCarrierContact _
                                            , .BookAdhocCarrierContactPhone = d.BookAdhocCarrierContactPhone _
                                            , .BookAdhocOrigCompControl = If(d.BookAdhocOrigCompControl.HasValue, d.BookAdhocOrigCompControl, 0) _
                                            , .BookAdhocOrigName = d.BookAdhocOrigName _
                                            , .BookAdhocOrigAddress1 = d.BookAdhocOrigAddress1 _
                                            , .BookAdhocOrigAddress2 = d.BookAdhocOrigAddress2 _
                                            , .BookAdhocOrigAddress3 = d.BookAdhocOrigAddress3 _
                                            , .BookAdhocOrigCity = d.BookAdhocOrigCity _
                                            , .BookAdhocOrigState = d.BookAdhocOrigState _
                                            , .BookAdhocOrigCountry = d.BookAdhocOrigCountry _
                                            , .BookAdhocOrigZip = d.BookAdhocOrigZip _
                                            , .BookAdhocOrigPhone = d.BookAdhocOrigPhone _
                                            , .BookAdhocOrigFax = d.BookAdhocOrigFax _
                                            , .BookAdhocOriginStartHrs = d.BookAdhocOriginStartHrs _
                                            , .BookAdhocOriginStopHrs = d.BookAdhocOriginStopHrs _
                                            , .BookAdhocOriginApptReq = If(d.BookAdhocOriginApptReq.HasValue, d.BookAdhocOriginApptReq, False) _
                                            , .BookAdhocDestCompControl = If(d.BookAdhocDestCompControl.HasValue, d.BookAdhocDestCompControl, 0) _
                                            , .BookAdhocDestName = d.BookAdhocDestName _
                                            , .BookAdhocDestAddress1 = d.BookAdhocDestAddress1 _
                                            , .BookAdhocDestAddress2 = d.BookAdhocDestAddress2 _
                                            , .BookAdhocDestAddress3 = d.BookAdhocDestAddress3 _
                                            , .BookAdhocDestCity = d.BookAdhocDestCity _
                                            , .BookAdhocDestState = d.BookAdhocDestState _
                                            , .BookAdhocDestCountry = d.BookAdhocDestCountry _
                                            , .BookAdhocDestZip = d.BookAdhocDestZip _
                                            , .BookAdhocDestPhone = d.BookAdhocDestPhone _
                                            , .BookAdhocDestFax = d.BookAdhocDestFax _
                                            , .BookAdhocDestStartHrs = d.BookAdhocDestStartHrs _
                                            , .BookAdhocDestStopHrs = d.BookAdhocDestStopHrs _
                                            , .BookAdhocDestApptReq = If(d.BookAdhocDestApptReq.HasValue, d.BookAdhocDestApptReq, False) _
                                            , .BookAdhocDateOrdered = d.BookAdhocDateOrdered _
                                            , .BookAdhocDateLoad = d.BookAdhocDateLoad _
                                            , .BookAdhocDateInvoice = d.BookAdhocDateInvoice _
                                            , .BookAdhocDateRequired = d.BookAdhocDateRequired _
                                            , .BookAdhocDateDelivered = d.BookAdhocDateDelivered _
                                            , .BookAdhocTotalCases = If(d.BookAdhocTotalCases.HasValue, d.BookAdhocTotalCases, 0) _
                                            , .BookAdhocTotalWgt = If(d.BookAdhocTotalWgt.HasValue, d.BookAdhocTotalWgt, 0) _
                                            , .BookAdhocTotalPL = If(d.BookAdhocTotalPL.HasValue, d.BookAdhocTotalPL, 0) _
                                            , .BookAdhocTotalCube = If(d.BookAdhocTotalCube.HasValue, d.BookAdhocTotalCube, 0) _
                                            , .BookAdhocTotalPX = If(d.BookAdhocTotalPX.HasValue, d.BookAdhocTotalPX, 0) _
                                            , .BookAdhocTotalBFC = If(d.BookAdhocTotalBFC.HasValue, d.BookAdhocTotalBFC, 0) _
                                            , .BookAdhocTranCode = d.BookAdhocTranCode _
                                            , .BookAdhocPayCode = d.BookAdhocPayCode _
                                            , .BookAdhocTypeCode = d.BookAdhocTypeCode _
                                            , .BookAdhocBOLCode = d.BookAdhocBOLCode _
                                            , .BookAdhocStopNo = If(d.BookAdhocStopNo.HasValue, d.BookAdhocStopNo, 0) _
                                            , .BookAdhocCarrFBNumber = d.BookAdhocCarrFBNumber _
                                            , .BookAdhocCarrOrderNumber = d.BookAdhocCarrOrderNumber _
                                            , .BookAdhocCarrBLNumber = d.BookAdhocCarrBLNumber _
                                            , .BookAdhocCarrBookAdhocDate = d.BookAdhocCarrBookAdhocDate _
                                            , .BookAdhocCarrBookAdhocTime = d.BookAdhocCarrBookAdhocTime _
                                            , .BookAdhocCarrBookAdhocContact = d.BookAdhocCarrBookAdhocContact _
                                            , .BookAdhocCarrScheduleDate = d.BookAdhocCarrScheduleDate _
                                            , .BookAdhocCarrScheduleTime = d.BookAdhocCarrScheduleTime _
                                            , .BookAdhocCarrActualDate = d.BookAdhocCarrActualDate _
                                            , .BookAdhocCarrActualTime = d.BookAdhocCarrActualTime _
                                            , .BookAdhocCarrActLoadComplete_Date = d.BookAdhocCarrActLoadComplete_Date _
                                            , .BookAdhocCarrActLoadCompleteTime = d.BookAdhocCarrActLoadCompleteTime _
                                            , .BookAdhocCarrDockPUAssigment = d.BookAdhocCarrDockPUAssigment _
                                            , .BookAdhocCarrPODate = d.BookAdhocCarrPODate _
                                            , .BookAdhocCarrPOTime = d.BookAdhocCarrPOTime _
                                            , .BookAdhocCarrApptDate = d.BookAdhocCarrApptDate _
                                            , .BookAdhocCarrApptTime = d.BookAdhocCarrApptTime _
                                            , .BookAdhocCarrActDate = d.BookAdhocCarrActDate _
                                            , .BookAdhocCarrActTime = d.BookAdhocCarrActTime _
                                            , .BookAdhocCarrActUnloadCompDate = d.BookAdhocCarrActUnloadCompDate _
                                            , .BookAdhocCarrActUnloadCompTime = d.BookAdhocCarrActUnloadCompTime _
                                            , .BookAdhocCarrDockDelAssignment = d.BookAdhocCarrDockDelAssignment _
                                            , .BookAdhocCarrVarDay = If(d.BookAdhocCarrVarDay.HasValue, d.BookAdhocCarrVarDay, 0) _
                                            , .BookAdhocCarrVarHrs = If(d.BookAdhocCarrVarHrs.HasValue, d.BookAdhocCarrVarHrs, 0) _
                                            , .BookAdhocCarrTrailerNo = d.BookAdhocCarrTrailerNo _
                                            , .BookAdhocCarrSealNo = d.BookAdhocCarrSealNo _
                                            , .BookAdhocCarrDriverNo = d.BookAdhocCarrDriverNo _
                                            , .BookAdhocCarrDriverName = d.BookAdhocCarrDriverName _
                                            , .BookAdhocCarrRouteNo = d.BookAdhocCarrRouteNo _
                                            , .BookAdhocCarrTripNo = d.BookAdhocCarrTripNo _
                                            , .BookAdhocFinARBookAdhocFrt = If(d.BookAdhocFinARBookAdhocFrt.HasValue, d.BookAdhocFinARBookAdhocFrt, 0) _
                                            , .BookAdhocFinARInvoiceDate = d.BookAdhocFinARInvoiceDate _
                                            , .BookAdhocFinARInvoiceAmt = If(d.BookAdhocFinARInvoiceAmt.HasValue, d.BookAdhocFinARInvoiceAmt, 0) _
                                            , .BookAdhocFinARPayDate = d.BookAdhocFinARPayDate _
                                            , .BookAdhocFinARPayAmt = If(d.BookAdhocFinARPayAmt.HasValue, d.BookAdhocFinARPayAmt, 0) _
                                            , .BookAdhocFinARCheck = d.BookAdhocFinARCheck _
                                            , .BookAdhocFinARGLNumber = d.BookAdhocFinARGLNumber _
                                            , .BookAdhocFinARBalance = If(d.BookAdhocFinARBalance.HasValue, d.BookAdhocFinARBalance, 0) _
                                            , .BookAdhocFinARCurType = If(d.BookAdhocFinARCurType.HasValue, d.BookAdhocFinARCurType, 0) _
                                            , .BookAdhocFinAPBillNumber = d.BookAdhocFinAPBillNumber _
                                            , .BookAdhocFinAPBillNoDate = d.BookAdhocFinAPBillNoDate _
                                            , .BookAdhocFinAPBillInvDate = d.BookAdhocFinAPBillInvDate _
                                            , .BookAdhocFinAPActWgt = If(d.BookAdhocFinAPActWgt.HasValue, d.BookAdhocFinAPActWgt, 0) _
                                            , .BookAdhocFinAPStdCost = If(d.BookAdhocFinAPStdCost.HasValue, d.BookAdhocFinAPStdCost, 0) _
                                            , .BookAdhocFinAPActCost = If(d.BookAdhocFinAPActCost.HasValue, d.BookAdhocFinAPActCost, 0) _
                                            , .BookAdhocFinAPPayDate = d.BookAdhocFinAPPayDate _
                                            , .BookAdhocFinAPPayAmt = If(d.BookAdhocFinAPPayAmt.HasValue, d.BookAdhocFinAPPayAmt, 0) _
                                            , .BookAdhocFinAPCheck = d.BookAdhocFinAPCheck _
                                            , .BookAdhocFinAPGLNumber = d.BookAdhocFinAPGLNumber _
                                            , .BookAdhocFinAPLastViewed = d.BookAdhocFinAPLastViewed _
                                            , .BookAdhocFinAPCurType = If(d.BookAdhocFinAPCurType.HasValue, d.BookAdhocFinAPCurType, 0) _
                                            , .BookAdhocFinCommStd = If(d.BookAdhocFinCommStd.HasValue, d.BookAdhocFinCommStd, 0) _
                                            , .BookAdhocFinCommAct = If(d.BookAdhocFinCommAct.HasValue, d.BookAdhocFinCommAct, 0) _
                                            , .BookAdhocFinCommPayDate = d.BookAdhocFinCommPayDate _
                                            , .BookAdhocFinCommPayAmt = If(d.BookAdhocFinCommPayAmt.HasValue, d.BookAdhocFinCommPayAmt, 0) _
                                            , .BookAdhocFinCommtCheck = d.BookAdhocFinCommtCheck _
                                            , .BookAdhocFinCommCreditAmt = If(d.BookAdhocFinCommCreditAmt.HasValue, d.BookAdhocFinCommCreditAmt, 0) _
                                            , .BookAdhocFinCommCreditPayDate = d.BookAdhocFinCommCreditPayDate _
                                            , .BookAdhocFinCommLoadCount = If(d.BookAdhocFinCommLoadCount.HasValue, d.BookAdhocFinCommLoadCount, 0) _
                                            , .BookAdhocFinCommGLNumber = d.BookAdhocFinCommGLNumber _
                                            , .BookAdhocFinCheckClearedDate = d.BookAdhocFinCheckClearedDate _
                                            , .BookAdhocFinCheckClearedNumber = d.BookAdhocFinCheckClearedNumber _
                                            , .BookAdhocFinCheckClearedAmt = If(d.BookAdhocFinCheckClearedAmt.HasValue, d.BookAdhocFinCheckClearedAmt, 0) _
                                            , .BookAdhocFinCheckClearedDesc = d.BookAdhocFinCheckClearedDesc _
                                            , .BookAdhocFinCheckClearedAcct = d.BookAdhocFinCheckClearedAcct _
                                            , .BookAdhocRevBilledBFC = If(d.BookAdhocRevBilledBFC.HasValue, d.BookAdhocRevBilledBFC, 0) _
                                            , .BookAdhocRevCarrierCost = If(d.BookAdhocRevCarrierCost.HasValue, d.BookAdhocRevCarrierCost, 0) _
                                            , .BookAdhocRevStopQty = If(d.BookAdhocRevStopQty.HasValue, d.BookAdhocRevStopQty, 0) _
                                            , .BookAdhocRevStopCost = If(d.BookAdhocRevStopCost.HasValue, d.BookAdhocRevStopCost, 0) _
                                            , .BookAdhocRevOtherCost = If(d.BookAdhocRevOtherCost.HasValue, d.BookAdhocRevOtherCost, 0) _
                                            , .BookAdhocRevTotalCost = If(d.BookAdhocRevTotalCost.HasValue, d.BookAdhocRevTotalCost, 0) _
                                            , .BookAdhocRevLoadSavings = If(d.BookAdhocRevLoadSavings.HasValue, d.BookAdhocRevLoadSavings, 0) _
                                            , .BookAdhocRevCommPercent = If(d.BookAdhocRevCommPercent.HasValue, d.BookAdhocRevCommPercent, 0) _
                                            , .BookAdhocRevCommCost = If(d.BookAdhocRevCommCost.HasValue, d.BookAdhocRevCommCost, 0) _
                                            , .BookAdhocRevGrossRevenue = If(d.BookAdhocRevGrossRevenue.HasValue, d.BookAdhocRevGrossRevenue, 0) _
                                            , .BookAdhocRevNegRevenue = If(d.BookAdhocRevNegRevenue.HasValue, d.BookAdhocRevNegRevenue, 0) _
                                            , .BookAdhocMilesFrom = If(d.BookAdhocMilesFrom.HasValue, d.BookAdhocMilesFrom, 0) _
                                            , .BookAdhocLaneCarrControl = If(d.BookAdhocLaneCarrControl.HasValue, d.BookAdhocLaneCarrControl, 0) _
                                            , .BookAdhocHoldLoad = If(d.BookAdhocHoldLoad.HasValue, d.BookAdhocHoldLoad, 0) _
                                            , .BookAdhocRouteFinalDate = d.BookAdhocRouteFinalDate _
                                            , .BookAdhocRouteFinalCode = d.BookAdhocRouteFinalCode _
                                            , .BookAdhocRouteFinalFlag = d.BookAdhocRouteFinalFlag _
                                            , .BookAdhocWarehouseNumber = d.BookAdhocWarehouseNumber _
                                            , .BookAdhocComCode = d.BookAdhocComCode _
                                            , .BookAdhocTransType = d.BookAdhocTransType _
                                            , .BookAdhocRouteConsFlag = d.BookAdhocRouteConsFlag _
                                            , .BookAdhocWhseAuthorizationNo = d.BookAdhocWhseAuthorizationNo _
                                            , .BookAdhocHotLoad = If(d.BookAdhocHotLoad.HasValue, d.BookAdhocHotLoad, False) _
                                            , .BookAdhocFinAPActTax = If(d.BookAdhocFinAPActTax.HasValue, d.BookAdhocFinAPActTax, 0) _
                                            , .BookAdhocFinAPExportFlag = d.BookAdhocFinAPExportFlag _
                                            , .BookAdhocFinARFreightTax = d.BookAdhocFinARFreightTax _
                                            , .BookAdhocRevFreightTax = d.BookAdhocRevFreightTax _
                                            , .BookAdhocRevNetCost = d.BookAdhocRevNetCost _
                                            , .BookAdhocFinServiceFee = If(d.BookAdhocFinServiceFee.HasValue, d.BookAdhocFinServiceFee, 0) _
                                            , .BookAdhocFinAPExportDate = d.BookAdhocFinAPExportDate _
                                            , .BookAdhocFinAPExportRetry = If(d.BookAdhocFinAPExportRetry.HasValue, d.BookAdhocFinAPExportRetry, 0) _
                                            , .BookAdhocCarrierContControl = If(d.BookAdhocCarrierContControl.HasValue, d.BookAdhocCarrierContControl, 0) _
                                            , .BookAdhocHotLoadSent = If(d.BookAdhocHotLoadSent.HasValue, d.BookAdhocHotLoadSent, False) _
                                            , .BookAdhocExportDocCreated = If(d.BookAdhocExportDocCreated.HasValue, d.BookAdhocExportDocCreated, False) _
                                            , .BookAdhocDoNotInvoice = d.BookAdhocDoNotInvoice _
                                            , .BookAdhocCarrStartLoadingDate = d.BookAdhocCarrStartLoadingDate _
                                            , .BookAdhocCarrStartLoadingTime = d.BookAdhocCarrStartLoadingTime _
                                            , .BookAdhocCarrFinishLoadingDate = d.BookAdhocCarrFinishLoadingDate _
                                            , .BookAdhocCarrFinishLoadingTime = d.BookAdhocCarrFinishLoadingTime _
                                            , .BookAdhocCarrStartUnloadingDate = d.BookAdhocCarrStartUnloadingDate _
                                            , .BookAdhocCarrStartUnloadingTime = d.BookAdhocCarrStartUnloadingTime _
                                            , .BookAdhocCarrFinishUnloadingDate = d.BookAdhocCarrFinishUnloadingDate _
                                            , .BookAdhocCarrFinishUnloadingTime = d.BookAdhocCarrFinishUnloadingTime _
                                            , .BookAdhocOrderSequence = d.BookAdhocOrderSequence _
                                            , .BookAdhocChepGLID = d.BookAdhocChepGLID _
                                            , .BookAdhocCarrierTypeCode = d.BookAdhocCarrierTypeCode _
                                            , .BookAdhocPalletPositions = d.BookAdhocPalletPositions _
                                            , .BookAdhocShipCarrierProNumber = d.BookAdhocShipCarrierProNumber _
                                            , .BookAdhocShipCarrierName = d.BookAdhocShipCarrierName _
                                            , .BookAdhocShipCarrierNumber = d.BookAdhocShipCarrierNumber _
                                            , .BookAdhocAPAdjReasonControl = d.BookAdhocAPAdjReasonControl _
                                            , .BookAdhocDateRequested = d.BookAdhocDateRequested _
                                            , .BookAdhocCarrierEquipmentCodes = d.BookAdhocCarrierEquipmentCodes _
                                            , .BookAdhocLockAllCosts = d.BookAdhocLockAllCosts _
                                            , .BookAdhocLockBFCCost = d.BookAdhocLockBFCCost _
                                            , .BookAdhocDestStopNumber = d.BookAdhocDestStopNumber _
                                            , .BookAdhocOrigStopNumber = d.BookAdhocOrigStopNumber _
                                            , .BookAdhocDestStopControl = d.BookAdhocDestStopControl _
                                            , .BookAdhocOrigStopControl = d.BookAdhocOrigStopControl _
                                            , .BookAdhocRouteTypeCode = d.BookAdhocRouteTypeCode _
                                            , .BookAdhocAlternateAddressLaneControl = d.BookAdhocAlternateAddressLaneControl _
                                            , .BookAdhocAlternateAddressLaneNumber = d.BookAdhocAlternateAddressLaneNumber _
                                            , .BookAdhocDefaultRouteSequence = d.BookAdhocDefaultRouteSequence _
                                            , .BookAdhocRouteGuideControl = d.BookAdhocRouteGuideControl _
                                            , .BookAdhocRouteGuideNumber = d.BookAdhocRouteGuideNumber _
                                            , .BookAdhocCustomerApprovalRecieved = d.BookAdhocCustomerApprovalRecieved _
                                            , .BookAdhocCustomerApprovalTransmitted = d.BookAdhocCustomerApprovalTransmitted _
                                            , .BookAdhocAMSPickupApptControl = d.BookAdhocAMSPickupApptControl _
                                            , .BookAdhocAMSDeliveryApptControl = d.BookAdhocAMSDeliveryApptControl _
                                            , .BookAdhocItemDetailDescription = d.BookAdhocItemDetailDescription _
                                            , .BookAdhocModDate = d.BookAdhocModDate _
                                            , .BookAdhocModUser = d.BookAdhocModUser _
                                            , .BookAdhocUpdated = d.BookAdhocUpdated.ToArray _
                                            , .BookAdhocLoads = (
                                                    From c In d.BookAdhocLoads
                                                    Select New DTO.BookAdhocLoad With {.BookAdhocLoadControl = c.BookAdhocLoadControl _
                                                    , .BookAdhocLoadBookAdhocControl = c.BookAdhocLoadBookAdhocControl _
                                                    , .BookAdhocLoadBuy = c.BookAdhocLoadBuy _
                                                    , .BookAdhocLoadPONumber = c.BookAdhocLoadPONumber _
                                                    , .BookAdhocLoadVendor = c.BookAdhocLoadVendor _
                                                    , .BookAdhocLoadCaseQty = If(c.BookAdhocLoadCaseQty.HasValue, c.BookAdhocLoadCaseQty, 0) _
                                                    , .BookAdhocLoadWgt = If(c.BookAdhocLoadWgt.HasValue, c.BookAdhocLoadWgt, 0) _
                                                    , .BookAdhocLoadCube = If(c.BookAdhocLoadCube.HasValue, c.BookAdhocLoadCube, 0) _
                                                    , .BookAdhocLoadPL = If(c.BookAdhocLoadPL.HasValue, c.BookAdhocLoadPL, 0) _
                                                    , .BookAdhocLoadPX = If(c.BookAdhocLoadPX.HasValue, c.BookAdhocLoadPX, 0) _
                                                    , .BookAdhocLoadPType = c.BookAdhocLoadPType _
                                                    , .BookAdhocLoadCom = c.BookAdhocLoadCom _
                                                    , .BookAdhocLoadPUOrigin = c.BookAdhocLoadPUOrigin _
                                                    , .BookAdhocLoadBFC = If(c.BookAdhocLoadBFC.HasValue, c.BookAdhocLoadBFC, 0) _
                                                    , .BookAdhocLoadTotCost = If(c.BookAdhocLoadTotCost.HasValue, c.BookAdhocLoadTotCost, 0) _
                                                    , .BookAdhocLoadComments = c.BookAdhocLoadComments _
                                                    , .BookAdhocLoadStopSeq = If(c.BookAdhocLoadStopSeq.HasValue, c.BookAdhocLoadStopSeq, 0) _
                                                    , .BookAdhocLoadModDate = c.BookAdhocLoadModDate _
                                                    , .BookAdhocLoadModUser = c.BookAdhocLoadModUser _
                                                    , .BookAdhocLoadUpdated = c.BookAdhocLoadUpdated.ToArray()}).ToList()}).ToArray()

                Return BookAdhocs

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function


#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.BookAdhoc)
        'Dim strBookAdhocPro As String = "Adhoc" & d.BookAdhocControl.ToString

        'Create New Record
        Return New LTS.BookAdhoc With {.BookAdhocControl = d.BookAdhocControl _
                                          , .BookAdhocProNumber = d.BookAdhocProNumber _
                                          , .BookAdhocProBase = d.BookAdhocProBase _
                                          , .BookAdhocConsPrefix = d.BookAdhocConsPrefix _
                                          , .BookAdhocCustCompControl = d.BookAdhocCustCompControl _
                                          , .BookAdhocCommCompControl = d.BookAdhocCommCompControl _
                                          , .BookAdhocODControl = d.BookAdhocODControl _
                                          , .BookAdhocCarrierControl = d.BookAdhocCarrierControl _
                                          , .BookAdhocCarrierContact = d.BookAdhocCarrierContact _
                                          , .BookAdhocCarrierContactPhone = d.BookAdhocCarrierContactPhone _
                                          , .BookAdhocOrigCompControl = d.BookAdhocOrigCompControl _
                                          , .BookAdhocOrigName = d.BookAdhocOrigName _
                                          , .BookAdhocOrigAddress1 = d.BookAdhocOrigAddress1 _
                                          , .BookAdhocOrigAddress2 = d.BookAdhocOrigAddress2 _
                                          , .BookAdhocOrigAddress3 = d.BookAdhocOrigAddress3 _
                                          , .BookAdhocOrigCity = d.BookAdhocOrigCity _
                                          , .BookAdhocOrigState = d.BookAdhocOrigState _
                                          , .BookAdhocOrigCountry = d.BookAdhocOrigCountry _
                                          , .BookAdhocOrigZip = d.BookAdhocOrigZip _
                                          , .BookAdhocOrigPhone = d.BookAdhocOrigPhone _
                                          , .BookAdhocOrigFax = d.BookAdhocOrigFax _
                                          , .BookAdhocOriginStartHrs = d.BookAdhocOriginStartHrs _
                                          , .BookAdhocOriginStopHrs = d.BookAdhocOriginStopHrs _
                                          , .BookAdhocOriginApptReq = d.BookAdhocOriginApptReq _
                                          , .BookAdhocDestCompControl = d.BookAdhocDestCompControl _
                                          , .BookAdhocDestName = d.BookAdhocDestName _
                                          , .BookAdhocDestAddress1 = d.BookAdhocDestAddress1 _
                                          , .BookAdhocDestAddress2 = d.BookAdhocDestAddress2 _
                                          , .BookAdhocDestAddress3 = d.BookAdhocDestAddress3 _
                                          , .BookAdhocDestCity = d.BookAdhocDestCity _
                                          , .BookAdhocDestState = d.BookAdhocDestState _
                                          , .BookAdhocDestCountry = d.BookAdhocDestCountry _
                                          , .BookAdhocDestZip = d.BookAdhocDestZip _
                                          , .BookAdhocDestPhone = d.BookAdhocDestPhone _
                                          , .BookAdhocDestFax = d.BookAdhocDestFax _
                                          , .BookAdhocDestStartHrs = d.BookAdhocDestStartHrs _
                                          , .BookAdhocDestStopHrs = d.BookAdhocDestStopHrs _
                                          , .BookAdhocDestApptReq = d.BookAdhocDestApptReq _
                                          , .BookAdhocDateOrdered = d.BookAdhocDateOrdered _
                                          , .BookAdhocDateLoad = d.BookAdhocDateLoad _
                                          , .BookAdhocDateInvoice = d.BookAdhocDateInvoice _
                                          , .BookAdhocDateRequired = d.BookAdhocDateRequired _
                                          , .BookAdhocDateDelivered = d.BookAdhocDateDelivered _
                                          , .BookAdhocTotalCases = d.BookAdhocTotalCases _
                                          , .BookAdhocTotalWgt = d.BookAdhocTotalWgt _
                                          , .BookAdhocTotalPL = d.BookAdhocTotalPL _
                                          , .BookAdhocTotalCube = d.BookAdhocTotalCube _
                                          , .BookAdhocTotalPX = d.BookAdhocTotalPX _
                                          , .BookAdhocTotalBFC = d.BookAdhocTotalBFC _
                                          , .BookAdhocTranCode = d.BookAdhocTranCode _
                                          , .BookAdhocPayCode = d.BookAdhocPayCode _
                                          , .BookAdhocTypeCode = d.BookAdhocTypeCode _
                                          , .BookAdhocBOLCode = d.BookAdhocBOLCode _
                                          , .BookAdhocStopNo = d.BookAdhocStopNo _
                                          , .BookAdhocCarrFBNumber = d.BookAdhocCarrFBNumber _
                                          , .BookAdhocCarrOrderNumber = d.BookAdhocCarrOrderNumber _
                                          , .BookAdhocCarrBLNumber = d.BookAdhocCarrBLNumber _
                                          , .BookAdhocCarrBookAdhocDate = d.BookAdhocCarrBookAdhocDate _
                                          , .BookAdhocCarrBookAdhocTime = d.BookAdhocCarrBookAdhocTime _
                                          , .BookAdhocCarrBookAdhocContact = d.BookAdhocCarrBookAdhocContact _
                                          , .BookAdhocCarrScheduleDate = d.BookAdhocCarrScheduleDate _
                                          , .BookAdhocCarrScheduleTime = d.BookAdhocCarrScheduleTime _
                                          , .BookAdhocCarrActualDate = d.BookAdhocCarrActualDate _
                                          , .BookAdhocCarrActualTime = d.BookAdhocCarrActualTime _
                                          , .BookAdhocCarrActLoadComplete_Date = d.BookAdhocCarrActLoadComplete_Date _
                                          , .BookAdhocCarrActLoadCompleteTime = d.BookAdhocCarrActLoadCompleteTime _
                                          , .BookAdhocCarrDockPUAssigment = d.BookAdhocCarrDockPUAssigment _
                                          , .BookAdhocCarrPODate = d.BookAdhocCarrPODate _
                                          , .BookAdhocCarrPOTime = d.BookAdhocCarrPOTime _
                                          , .BookAdhocCarrApptDate = d.BookAdhocCarrApptDate _
                                          , .BookAdhocCarrApptTime = d.BookAdhocCarrApptTime _
                                          , .BookAdhocCarrActDate = d.BookAdhocCarrActDate _
                                          , .BookAdhocCarrActTime = d.BookAdhocCarrActTime _
                                          , .BookAdhocCarrActUnloadCompDate = d.BookAdhocCarrActUnloadCompDate _
                                          , .BookAdhocCarrActUnloadCompTime = d.BookAdhocCarrActUnloadCompTime _
                                          , .BookAdhocCarrDockDelAssignment = d.BookAdhocCarrDockDelAssignment _
                                          , .BookAdhocCarrVarDay = d.BookAdhocCarrVarDay _
                                          , .BookAdhocCarrVarHrs = d.BookAdhocCarrVarHrs _
                                          , .BookAdhocCarrTrailerNo = d.BookAdhocCarrTrailerNo _
                                          , .BookAdhocCarrSealNo = d.BookAdhocCarrSealNo _
                                          , .BookAdhocCarrDriverNo = d.BookAdhocCarrDriverNo _
                                          , .BookAdhocCarrDriverName = d.BookAdhocCarrDriverName _
                                          , .BookAdhocCarrRouteNo = d.BookAdhocCarrRouteNo _
                                          , .BookAdhocCarrTripNo = d.BookAdhocCarrTripNo _
                                          , .BookAdhocFinARBookAdhocFrt = d.BookAdhocFinARBookAdhocFrt _
                                          , .BookAdhocFinARInvoiceDate = d.BookAdhocFinARInvoiceDate _
                                          , .BookAdhocFinARInvoiceAmt = d.BookAdhocFinARInvoiceAmt _
                                          , .BookAdhocFinARPayDate = d.BookAdhocFinARPayDate _
                                          , .BookAdhocFinARPayAmt = d.BookAdhocFinARPayAmt _
                                          , .BookAdhocFinARCheck = d.BookAdhocFinARCheck _
                                          , .BookAdhocFinARGLNumber = d.BookAdhocFinARGLNumber _
                                          , .BookAdhocFinARBalance = d.BookAdhocFinARBalance _
                                          , .BookAdhocFinARCurType = d.BookAdhocFinARCurType _
                                          , .BookAdhocFinAPBillNumber = d.BookAdhocFinAPBillNumber _
                                          , .BookAdhocFinAPBillNoDate = d.BookAdhocFinAPBillNoDate _
                                          , .BookAdhocFinAPBillInvDate = d.BookAdhocFinAPBillInvDate _
                                          , .BookAdhocFinAPActWgt = d.BookAdhocFinAPActWgt _
                                          , .BookAdhocFinAPStdCost = d.BookAdhocFinAPStdCost _
                                          , .BookAdhocFinAPActCost = d.BookAdhocFinAPActCost _
                                          , .BookAdhocFinAPPayDate = d.BookAdhocFinAPPayDate _
                                          , .BookAdhocFinAPPayAmt = d.BookAdhocFinAPPayAmt _
                                          , .BookAdhocFinAPCheck = d.BookAdhocFinAPCheck _
                                          , .BookAdhocFinAPGLNumber = d.BookAdhocFinAPGLNumber _
                                          , .BookAdhocFinAPLastViewed = d.BookAdhocFinAPLastViewed _
                                          , .BookAdhocFinAPCurType = d.BookAdhocFinAPCurType _
                                          , .BookAdhocFinCommStd = d.BookAdhocFinCommStd _
                                          , .BookAdhocFinCommAct = d.BookAdhocFinCommAct _
                                          , .BookAdhocFinCommPayDate = d.BookAdhocFinCommPayDate _
                                          , .BookAdhocFinCommPayAmt = d.BookAdhocFinCommPayAmt _
                                          , .BookAdhocFinCommtCheck = d.BookAdhocFinCommtCheck _
                                          , .BookAdhocFinCommCreditAmt = d.BookAdhocFinCommCreditAmt _
                                          , .BookAdhocFinCommCreditPayDate = d.BookAdhocFinCommCreditPayDate _
                                          , .BookAdhocFinCommLoadCount = d.BookAdhocFinCommLoadCount _
                                          , .BookAdhocFinCommGLNumber = d.BookAdhocFinCommGLNumber _
                                          , .BookAdhocFinCheckClearedDate = d.BookAdhocFinCheckClearedDate _
                                          , .BookAdhocFinCheckClearedNumber = d.BookAdhocFinCheckClearedNumber _
                                          , .BookAdhocFinCheckClearedAmt = d.BookAdhocFinCheckClearedAmt _
                                          , .BookAdhocFinCheckClearedDesc = d.BookAdhocFinCheckClearedDesc _
                                          , .BookAdhocFinCheckClearedAcct = d.BookAdhocFinCheckClearedAcct _
                                          , .BookAdhocRevBilledBFC = d.BookAdhocRevBilledBFC _
                                          , .BookAdhocRevCarrierCost = d.BookAdhocRevCarrierCost _
                                          , .BookAdhocRevStopQty = d.BookAdhocRevStopQty _
                                          , .BookAdhocRevStopCost = d.BookAdhocRevStopCost _
                                          , .BookAdhocRevOtherCost = d.BookAdhocRevOtherCost _
                                          , .BookAdhocRevTotalCost = d.BookAdhocRevTotalCost _
                                          , .BookAdhocRevLoadSavings = d.BookAdhocRevLoadSavings _
                                          , .BookAdhocRevCommPercent = d.BookAdhocRevCommPercent _
                                          , .BookAdhocRevCommCost = d.BookAdhocRevCommCost _
                                          , .BookAdhocRevGrossRevenue = d.BookAdhocRevGrossRevenue _
                                          , .BookAdhocRevNegRevenue = d.BookAdhocRevNegRevenue _
                                          , .BookAdhocMilesFrom = d.BookAdhocMilesFrom _
                                          , .BookAdhocLaneCarrControl = d.BookAdhocLaneCarrControl _
                                          , .BookAdhocHoldLoad = d.BookAdhocHoldLoad _
                                          , .BookAdhocRouteFinalDate = d.BookAdhocRouteFinalDate _
                                          , .BookAdhocRouteFinalCode = d.BookAdhocRouteFinalCode _
                                          , .BookAdhocRouteFinalFlag = d.BookAdhocRouteFinalFlag _
                                          , .BookAdhocWarehouseNumber = d.BookAdhocWarehouseNumber _
                                          , .BookAdhocComCode = d.BookAdhocComCode _
                                          , .BookAdhocTransType = d.BookAdhocTransType _
                                          , .BookAdhocRouteConsFlag = d.BookAdhocRouteConsFlag _
                                          , .BookAdhocWhseAuthorizationNo = d.BookAdhocWhseAuthorizationNo _
                                          , .BookAdhocHotLoad = d.BookAdhocHotLoad _
                                          , .BookAdhocFinAPActTax = d.BookAdhocFinAPActTax _
                                          , .BookAdhocFinAPExportFlag = d.BookAdhocFinAPExportFlag _
                                          , .BookAdhocFinARFreightTax = d.BookAdhocFinARFreightTax _
                                          , .BookAdhocRevFreightTax = d.BookAdhocRevFreightTax _
                                          , .BookAdhocRevNetCost = d.BookAdhocRevNetCost _
                                          , .BookAdhocFinServiceFee = d.BookAdhocFinServiceFee _
                                          , .BookAdhocFinAPExportDate = d.BookAdhocFinAPExportDate _
                                          , .BookAdhocFinAPExportRetry = d.BookAdhocFinAPExportRetry _
                                          , .BookAdhocCarrierContControl = d.BookAdhocCarrierContControl _
                                          , .BookAdhocHotLoadSent = d.BookAdhocHotLoadSent _
                                          , .BookAdhocExportDocCreated = d.BookAdhocExportDocCreated _
                                          , .BookAdhocDoNotInvoice = d.BookAdhocDoNotInvoice _
                                          , .BookAdhocCarrStartLoadingDate = d.BookAdhocCarrStartLoadingDate _
                                          , .BookAdhocCarrStartLoadingTime = d.BookAdhocCarrStartLoadingTime _
                                          , .BookAdhocCarrFinishLoadingDate = d.BookAdhocCarrFinishLoadingDate _
                                          , .BookAdhocCarrFinishLoadingTime = d.BookAdhocCarrFinishLoadingTime _
                                          , .BookAdhocCarrStartUnloadingDate = d.BookAdhocCarrStartUnloadingDate _
                                          , .BookAdhocCarrStartUnloadingTime = d.BookAdhocCarrStartUnloadingTime _
                                          , .BookAdhocCarrFinishUnloadingDate = d.BookAdhocCarrFinishUnloadingDate _
                                          , .BookAdhocCarrFinishUnloadingTime = d.BookAdhocCarrFinishUnloadingTime _
                                          , .BookAdhocOrderSequence = d.BookAdhocOrderSequence _
                                          , .BookAdhocChepGLID = d.BookAdhocChepGLID _
                                          , .BookAdhocCarrierTypeCode = d.BookAdhocCarrierTypeCode _
                                          , .BookAdhocPalletPositions = d.BookAdhocPalletPositions _
                                          , .BookAdhocShipCarrierProNumber = d.BookAdhocShipCarrierProNumber _
                                          , .BookAdhocShipCarrierName = d.BookAdhocShipCarrierName _
                                          , .BookAdhocShipCarrierNumber = d.BookAdhocShipCarrierNumber _
                                          , .BookAdhocAPAdjReasonControl = d.BookAdhocAPAdjReasonControl _
                                          , .BookAdhocDateRequested = d.BookAdhocDateRequested _
                                          , .BookAdhocCarrierEquipmentCodes = d.BookAdhocCarrierEquipmentCodes _
                                          , .BookAdhocLockAllCosts = d.BookAdhocLockAllCosts _
                                          , .BookAdhocLockBFCCost = d.BookAdhocLockBFCCost _
                                            , .BookAdhocDestStopNumber = d.BookAdhocDestStopNumber _
                                            , .BookAdhocOrigStopNumber = d.BookAdhocOrigStopNumber _
                                            , .BookAdhocDestStopControl = d.BookAdhocDestStopControl _
                                            , .BookAdhocOrigStopControl = d.BookAdhocOrigStopControl _
                                            , .BookAdhocRouteTypeCode = d.BookAdhocRouteTypeCode _
                                            , .BookAdhocAlternateAddressLaneControl = d.BookAdhocAlternateAddressLaneControl _
                                            , .BookAdhocAlternateAddressLaneNumber = d.BookAdhocAlternateAddressLaneNumber _
                                            , .BookAdhocDefaultRouteSequence = d.BookAdhocDefaultRouteSequence _
                                            , .BookAdhocRouteGuideControl = d.BookAdhocRouteGuideControl _
                                            , .BookAdhocRouteGuideNumber = d.BookAdhocRouteGuideNumber _
                                            , .BookAdhocCustomerApprovalRecieved = d.BookAdhocCustomerApprovalRecieved _
                                            , .BookAdhocCustomerApprovalTransmitted = d.BookAdhocCustomerApprovalTransmitted _
                                            , .BookAdhocAMSPickupApptControl = d.BookAdhocAMSPickupApptControl _
                                            , .BookAdhocAMSDeliveryApptControl = d.BookAdhocAMSDeliveryApptControl _
                                            , .BookAdhocItemDetailDescription = d.BookAdhocItemDetailDescription _
                                          , .BookAdhocModUser = Me.Parameters.UserName _
                                          , .BookAdhocUpdated = If(d.BookAdhocUpdated Is Nothing, New Byte() {}, d.BookAdhocUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim source As LTS.BookAdhoc = TryCast(LinqTable, LTS.BookAdhoc)
        If source Is Nothing Then Return Nothing

        Return GetBookAdhocFiltered(Control:=source.BookAdhocControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim source As LTS.BookAdhoc = TryCast(LinqTable, LTS.BookAdhoc)
                If source Is Nothing Then Return Nothing
                ret = (From d In db.BookAdhocs
                       Where d.BookAdhocControl = source.BookAdhocControl
                       Select New DTO.QuickSaveResults With {.Control = d.BookAdhocControl _
                                                            , .ModDate = d.BookAdhocModDate _
                                                            , .ModUser = d.BookAdhocModUser _
                                                            , .Updated = d.BookAdhocUpdated.ToArray}).First
            Catch ex As FaultException(Of SqlFaultInfo)
                Throw
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed      
        With CType(oData, DTO.BookAdhoc)
            Try

                If String.IsNullOrEmpty(.BookAdhocProNumber) OrElse .BookAdhocProNumber.Trim.Length < 1 Then
                    'We need to add the pro number
                    Dim AdhocProSettings As String = getScalarString("Select top 1 cast(parvalue As nvarchar(20)) + ',' + partext as parsettings from Parameter where ParKey = 'GlobalAdhocPRONUMBER'")
                    Dim AdHocProSetting() As String = AdhocProSettings.Split(",")
                    Dim PROBase As String = ""
                    If Not AdHocProSetting Is Nothing AndAlso AdHocProSetting.Count > 0 Then
                        PROBase = AdHocProSetting(0)
                    End If
                    Dim intNextPro As Integer = 0
                    Dim intSaveProSeed As Integer = 1
                    If PROBase.Trim.Length < 1 Then
                        Dim dtVal = Date.Now

                        PROBase = dtVal.Month & dtVal.Day & dtVal.Year & dtVal.Hour & dtVal.Minute & dtVal.Second

                        Integer.TryParse(PROBase, intNextPro)
                        intSaveProSeed = 1
                    Else

                        Integer.TryParse(PROBase, intNextPro)
                        intNextPro += 1
                        intSaveProSeed = intNextPro
                    End If

                    Dim PROAbbrev As String = ""

                    If Not AdHocProSetting Is Nothing AndAlso AdHocProSetting.Count > 0 Then
                        PROAbbrev = AdHocProSetting(1)
                    End If
                    If PROAbbrev.Trim.Length < 1 Then PROAbbrev = "ADH"
                    executeSQL("Update dbo.Parameter Set ParValue = " & intSaveProSeed & " Where ParKey = 'GlobalAdhocPRONUMBER'")
                    Dim NewPRONumber = PROAbbrev & intNextPro.ToString
                    .BookAdhocProBase = Left(intNextPro.ToString, 50)
                    .BookAdhocProNumber = NewPRONumber
                    .TrackingState = TrackingInfo.Updated
                Else
                    'verify that the BookAdhocpronumber is not in use
                    Dim oBookAdhoc As DTO.BookAdhoc = (
                        From t In CType(oDB, NGLMASAMSDataContext).BookAdhocs
                        Where
                             t.BookAdhocProNumber = .BookAdhocProNumber
                        Select New DTO.BookAdhoc With {.BookAdhocControl = t.BookAdhocControl}).First
                    If Not oBookAdhoc Is Nothing Then
                        Dim strDetails As String = "Cannot save new BookAdhoc data.  The BookAdhoc Pro number, " & .BookAdhocProNumber & " ,  already exist."
                        Utilities.SaveAppError(strDetails, Me.Parameters)
                        Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField", .Details = strDetails}, New FaultReason("E_DataValidationFailure"))
                    End If
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DTO.BookAdhoc)
            Try
                'Get the newest record that matches the provided criteria
                Dim BookAdhoc As DTO.BookAdhoc = (
                From t In CType(oDB, NGLMASAMSDataContext).BookAdhocs
                Where
                     (t.BookAdhocControl <> .BookAdhocControl) _
                     And
                     (t.BookAdhocProNumber = .BookAdhocProNumber)
                Select New DTO.BookAdhoc With {.BookAdhocControl = t.BookAdhocControl}).First

                If Not BookAdhoc Is Nothing Then
                    Utilities.SaveAppError("Cannot save BookAdhoc changes.  The BookAdhoc PRO Number, " & .BookAdhocProNumber & " already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
        'Check if the BookAdhoc is being used by the BookAdhoc data or the lane data
        With CType(oData, DTO.BookAdhoc)
            Try
                ''Add code here to call the BookAdhoc and Lane data providers when they are created
                'Dim dpBookAdhoc As New NGLBookAdhocData(Me.Parameters)
                'Dim dpLane As New NGLLaneData(Me.Parameters)
                'Dim oBookAdhocs() As DTO.BookAdhoc
                'Dim oLanes() As DTO.Lane
                'Try
                '    oBookAdhocs = dpBookAdhoc.GetBookAdhocsByBookAdhoc(.BookAdhocControl)
                'Catch ex As FaultException
                '    If ex.Message <> "E_NoData" Then
                '        Throw
                '    End If
                'End Try
                'Try
                '    oLanes = dpLane.GetLanesByBookAdhoc(.BookAdhocControl)
                'Catch ex As FaultException
                '    If ex.Message <> "E_NoData" Then
                '        Throw
                '    End If
                'End Try
                'If (Not oBookAdhocs Is Nothing AndAlso oBookAdhocs.Length > 0) OrElse (Not oLanes Is Nothing AndAlso oLanes.Length > 0) Then
                '    Utilities.SaveAppError("Cannot delete BookAdhoc data.  The BookAdhoc number, " & .BookAdhocNumber & " is being used and cannot be deleted. check the BookAdhoc or lane information.", Me.Parameters)
                '    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                'End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub


#End Region

End Class

Public Class NGLBookAdhocLoadData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASAMSDataContext(ConnectionString)
        Me.LinqTable = db.BookAdhocLoads
        Me.LinqDB = db
        Me.SourceClass = "NGLBookAdhocLoadData"
    End Sub

#End Region

#Region "Properties"

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASAMSDataContext(ConnectionString)
            Me.LinqTable = db.BookAdhocLoads
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
        Return GetBookAdhocLoadFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookAdhocLoadsFiltered()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 
    ''' </remarks>
    Public Function GetBookAdhocLoadFiltered(Optional ByVal Control As Integer = 0) As DTO.BookAdhocLoad
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try

                Dim BookAdhocLoad As DTO.BookAdhocLoad = (
                From d In db.BookAdhocLoads
                Where
                    (d.BookAdhocLoadControl = If(Control = 0, d.BookAdhocLoadControl, Control))
                Order By d.BookAdhocLoadControl Descending
                Select New DTO.BookAdhocLoad With {.BookAdhocLoadControl = d.BookAdhocLoadControl _
                                          , .BookAdhocLoadBookAdhocControl = d.BookAdhocLoadBookAdhocControl _
                                          , .BookAdhocLoadBuy = d.BookAdhocLoadBuy _
                                          , .BookAdhocLoadPONumber = d.BookAdhocLoadPONumber _
                                          , .BookAdhocLoadVendor = d.BookAdhocLoadVendor _
                                          , .BookAdhocLoadCaseQty = If(d.BookAdhocLoadCaseQty.HasValue, d.BookAdhocLoadCaseQty, 0) _
                                          , .BookAdhocLoadWgt = If(d.BookAdhocLoadWgt.HasValue, d.BookAdhocLoadWgt, 0) _
                                          , .BookAdhocLoadCube = If(d.BookAdhocLoadCube.HasValue, d.BookAdhocLoadCube, 0) _
                                          , .BookAdhocLoadPL = If(d.BookAdhocLoadPL.HasValue, d.BookAdhocLoadPL, 0) _
                                          , .BookAdhocLoadPX = If(d.BookAdhocLoadPX.HasValue, d.BookAdhocLoadPX, 0) _
                                          , .BookAdhocLoadPType = d.BookAdhocLoadPType _
                                          , .BookAdhocLoadCom = d.BookAdhocLoadCom _
                                          , .BookAdhocLoadPUOrigin = d.BookAdhocLoadPUOrigin _
                                          , .BookAdhocLoadBFC = If(d.BookAdhocLoadBFC.HasValue, d.BookAdhocLoadBFC, 0) _
                                          , .BookAdhocLoadTotCost = If(d.BookAdhocLoadTotCost.HasValue, d.BookAdhocLoadTotCost, 0) _
                                          , .BookAdhocLoadComments = d.BookAdhocLoadComments _
                                          , .BookAdhocLoadStopSeq = If(d.BookAdhocLoadStopSeq.HasValue, d.BookAdhocLoadStopSeq, 0) _
                                          , .BookAdhocLoadModDate = d.BookAdhocLoadModDate _
                                          , .BookAdhocLoadModUser = d.BookAdhocLoadModUser _
                                          , .BookAdhocLoadUpdated = d.BookAdhocLoadUpdated.ToArray()}).First

                Return BookAdhocLoad

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>    '''
    ''' </remarks>
    Public Function GetBookAdhocLoadsFiltered(Optional ByVal BookControl As Integer = 0) As DTO.BookAdhocLoad()
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try

                Dim BookAdhocLoads() As DTO.BookAdhocLoad = (
                From d In db.BookAdhocLoads
                Where
                   (d.BookAdhocLoadBookAdhocControl = If(BookControl = 0, d.BookAdhocLoadBookAdhocControl, BookControl))
                Order By d.BookAdhocLoadControl
                Select New DTO.BookAdhocLoad With {.BookAdhocLoadControl = d.BookAdhocLoadControl _
                                         , .BookAdhocLoadBookAdhocControl = d.BookAdhocLoadBookAdhocControl _
                                         , .BookAdhocLoadBuy = d.BookAdhocLoadBuy _
                                         , .BookAdhocLoadPONumber = d.BookAdhocLoadPONumber _
                                         , .BookAdhocLoadVendor = d.BookAdhocLoadVendor _
                                         , .BookAdhocLoadCaseQty = If(d.BookAdhocLoadCaseQty.HasValue, d.BookAdhocLoadCaseQty, 0) _
                                         , .BookAdhocLoadWgt = If(d.BookAdhocLoadWgt.HasValue, d.BookAdhocLoadWgt, 0) _
                                         , .BookAdhocLoadCube = If(d.BookAdhocLoadCube.HasValue, d.BookAdhocLoadCube, 0) _
                                         , .BookAdhocLoadPL = If(d.BookAdhocLoadPL.HasValue, d.BookAdhocLoadPL, 0) _
                                         , .BookAdhocLoadPX = If(d.BookAdhocLoadPX.HasValue, d.BookAdhocLoadPX, 0) _
                                         , .BookAdhocLoadPType = d.BookAdhocLoadPType _
                                         , .BookAdhocLoadCom = d.BookAdhocLoadCom _
                                         , .BookAdhocLoadPUOrigin = d.BookAdhocLoadPUOrigin _
                                         , .BookAdhocLoadBFC = If(d.BookAdhocLoadBFC.HasValue, d.BookAdhocLoadBFC, 0) _
                                         , .BookAdhocLoadTotCost = If(d.BookAdhocLoadTotCost.HasValue, d.BookAdhocLoadTotCost, 0) _
                                         , .BookAdhocLoadComments = d.BookAdhocLoadComments _
                                         , .BookAdhocLoadStopSeq = If(d.BookAdhocLoadStopSeq.HasValue, d.BookAdhocLoadStopSeq, 0) _
                                         , .BookAdhocLoadModDate = d.BookAdhocLoadModDate _
                                         , .BookAdhocLoadModUser = d.BookAdhocLoadModUser _
                                         , .BookAdhocLoadUpdated = d.BookAdhocLoadUpdated.ToArray()}).ToArray()


                Return BookAdhocLoads

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DTO.BookAdhocLoad)
        'Create New Record
        Return New LTS.BookAdhocLoad With {.BookAdhocLoadControl = d.BookAdhocLoadControl _
                                          , .BookAdhocLoadBookAdhocControl = d.BookAdhocLoadBookAdhocControl _
                                          , .BookAdhocLoadBuy = d.BookAdhocLoadBuy _
                                          , .BookAdhocLoadPONumber = d.BookAdhocLoadPONumber _
                                          , .BookAdhocLoadVendor = d.BookAdhocLoadVendor _
                                          , .BookAdhocLoadCaseQty = d.BookAdhocLoadCaseQty _
                                          , .BookAdhocLoadWgt = d.BookAdhocLoadWgt _
                                          , .BookAdhocLoadCube = d.BookAdhocLoadCube _
                                          , .BookAdhocLoadPL = d.BookAdhocLoadPL _
                                          , .BookAdhocLoadPX = d.BookAdhocLoadPX _
                                          , .BookAdhocLoadPType = d.BookAdhocLoadPType _
                                          , .BookAdhocLoadCom = d.BookAdhocLoadCom _
                                          , .BookAdhocLoadPUOrigin = d.BookAdhocLoadPUOrigin _
                                          , .BookAdhocLoadBFC = d.BookAdhocLoadBFC _
                                          , .BookAdhocLoadTotCost = d.BookAdhocLoadTotCost _
                                          , .BookAdhocLoadComments = d.BookAdhocLoadComments _
                                          , .BookAdhocLoadStopSeq = d.BookAdhocLoadStopSeq _
                                          , .BookAdhocLoadModDate = Date.Now _
                                          , .BookAdhocLoadModUser = Parameters.UserName _
                                          , .BookAdhocLoadUpdated = If(d.BookAdhocLoadUpdated Is Nothing, New Byte() {}, d.BookAdhocLoadUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Dim oData As LTS.BookAdhocLoad = TryCast(LinqTable, LTS.BookAdhocLoad)
        If oData Is Nothing Then
            Return Nothing
        End If
        Return GetBookAdhocLoadFiltered(Control:=oData.BookAdhocLoadControl)
    End Function


    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim source As LTS.BookAdhocLoad = TryCast(LinqTable, LTS.BookAdhocLoad)
                If source Is Nothing Then Return Nothing
                ret = (From d In db.BookAdhocLoads
                       Where d.BookAdhocLoadControl = source.BookAdhocLoadControl
                       Select New DTO.QuickSaveResults With {.Control = d.BookAdhocLoadControl _
                                                            , .ModDate = d.BookAdhocLoadModDate _
                                                            , .ModUser = d.BookAdhocLoadModUser _
                                                            , .Updated = d.BookAdhocLoadUpdated.ToArray}).First
            Catch ex As FaultException(Of SqlFaultInfo)
                Throw
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function


#End Region

End Class

Public Class NGLAMSAppointmentTrackingData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASAMSDataContext(ConnectionString)
        Me.LinqTable = db.AMSAppointmentTrackings
        Me.LinqDB = db
        Me.SourceClass = "NGLAMSAppointmentTrackingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASAMSDataContext(ConnectionString)
            Me.LinqTable = db.AMSAppointmentTrackings
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
        Return GetAMSAppointmentTrackingFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    '***** CALLED BY GetAMSCustomTracking() IN AMS.cs *****
    Public Function GetAMSAppointmentTrackingFiltered(ByVal Control As Integer) As DTO.AMSAppointmentTracking
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim AMSAppointmentTracking As DTO.AMSAppointmentTracking = (
                From d In db.AMSAppointmentTrackings
                Where
                    (d.AMSApptTrackingControl = Control)
                Order By d.AMSApptTrackingControl Descending
                Select selectDTOData(d, db)).First
                Return AMSAppointmentTracking

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    '***** CALLED BY GetAMSCustomTrackingsByAppointment() IN AMS.cs *****
    Public Function GetAMSAppointmentTrackingsFiltered(ByVal AMSControl As Integer) As DTO.AMSAppointmentTracking()

        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'ConfigureApptTrackingData updates or inserts the data from the company configuration table as needed
                If Not ConfigureApptTrackingData(AMSControl) Then Return Nothing
                'Return all the records that match the criteria sorted by name
                Dim AMSAppointmentTrackings() As DTO.AMSAppointmentTracking = (
                From d In db.AMSAppointmentTrackings
                Where
                    (d.AMSApptTrackingApptControl = AMSControl)
                Order By d.AMSApptTrackingName
                Select selectDTOData(d, db)).ToArray()
                Return AMSAppointmentTrackings

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#Region "TMS 365"

    ''***************************** NEW METHODS *****************************

    ''' <summary>
    ''' Inserts or Updates a record in AMSAppointmentTracking
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Added By LVV on 5/24/18 for v-8.3 TMS365 Scheduler
    ''' </remarks>
    Public Sub InsertOrUpdateAMSAppointmentTracking(ByVal oRecord As LTS.AMSAppointmentTracking)
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try

                oRecord.AMSApptTrackingModDate = Date.Now
                oRecord.AMSApptTrackingModUser = Parameters.UserName

                If oRecord.AMSApptTrackingControl = 0 Then
                    'Insert
                    db.AMSAppointmentTrackings.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.AMSAppointmentTrackings.Attach(oRecord, True)
                End If

                db.SubmitChanges()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateAMSAppointmentTracking"), db)
            End Try
        End Using
    End Sub

#End Region

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.AMSAppointmentTracking)
        'Create New Record
        Return New LTS.AMSAppointmentTracking With {.AMSApptTrackingControl = d.AMSApptTrackingControl _
                                                       , .AMSApptTrackingApptControl = d.AMSApptTrackingApptControl _
                                                       , .AMSApptTrackingCompAMSApptTrackingSettingControl = d.AMSApptTrackingCompAMSApptTrackingSettingControl _
                                                       , .AMSApptTrackingDateTime = d.AMSApptTrackingDateTime _
                                                       , .AMSApptTrackingName = d.AMSApptTrackingName _
                                                       , .AMSApptTrackingDesc = d.AMSApptTrackingDesc _
                                                       , .AMSApptTrackingModDate = Date.Now _
                                                       , .AMSApptTrackingModUser = Parameters.UserName _
                                                       , .AMSApptTrackingUpdated = If(d.AMSApptTrackingUpdated Is Nothing, New Byte() {}, d.AMSApptTrackingUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetAMSAppointmentTrackingFiltered(Control:=CType(LinqTable, LTS.AMSAppointmentTracking).AMSApptTrackingControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim source As LTS.AMSAppointmentTracking = TryCast(LinqTable, LTS.AMSAppointmentTracking)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.AMSAppointmentTrackings
                       Where d.AMSApptTrackingControl = source.AMSApptTrackingControl
                       Select New DTO.QuickSaveResults With {.Control = d.AMSApptTrackingControl _
                                                            , .ModDate = d.AMSApptTrackingModDate _
                                                            , .ModUser = d.AMSApptTrackingModUser _
                                                            , .Updated = d.AMSApptTrackingUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Friend Function selectDTOData(ByVal d As LTS.AMSAppointmentTracking, ByRef db As NGLMASAMSDataContext) As DTO.AMSAppointmentTracking
        Return New DTO.AMSAppointmentTracking With {.AMSApptTrackingControl = d.AMSApptTrackingControl _
                                                       , .AMSApptTrackingApptControl = d.AMSApptTrackingApptControl _
                                                       , .AMSApptTrackingCompAMSApptTrackingSettingControl = d.AMSApptTrackingCompAMSApptTrackingSettingControl _
                                                       , .AMSApptTrackingDateTime = d.AMSApptTrackingDateTime _
                                                       , .AMSApptTrackingName = d.AMSApptTrackingName _
                                                       , .AMSApptTrackingDesc = d.AMSApptTrackingDesc _
                                                       , .AMSApptTrackingModDate = d.AMSApptTrackingModDate _
                                                       , .AMSApptTrackingModUser = d.AMSApptTrackingModUser _
                                                       , .AMSApptTrackingUpdated = d.AMSApptTrackingUpdated.ToArray()}
    End Function

    Protected Function ConfigureApptTrackingData(ByVal AMSApptControl As Integer) As Boolean

        Dim strBatchName As String = "ConfigureApptTrackingData"
        Dim strProcName As String = "dbo.spConfigureApptTrackingData"
        'Validate the parameter data
        If AMSApptControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@AMSApptControl", AMSApptControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        Return True
    End Function

#End Region

End Class

Public Class NGLAMSAppointmentUserFieldDataData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        processParameters(oParameters)
        Dim db As New NGLMASAMSDataContext(ConnectionString)
        Me.LinqTable = db.AMSAppointmentUserFieldDatas
        Me.LinqDB = db
        Me.SourceClass = "NGLAMSAppointmentUserFieldDataData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMASAMSDataContext(ConnectionString)
            Me.LinqTable = db.AMSAppointmentUserFieldDatas
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
        Return GetAMSAppointmentUserFieldDataFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return Nothing
    End Function

    '***** CALLED BY GetAMSUserField() IN AMS.cs *****
    Public Function GetAMSAppointmentUserFieldDataFiltered(ByVal Control As Integer) As DTO.AMSAppointmentUserFieldData
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim AMSAppointmentUserFieldData As DTO.AMSAppointmentUserFieldData = (
                From d In db.AMSAppointmentUserFieldDatas
                Where
                    (d.AMSApptUFDControl = Control)
                Order By d.AMSApptUFDControl Descending
                Select selectDTOData(d, db)).First
                Return AMSAppointmentUserFieldData

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    '***** CALLED BY GetAMSUserFieldsByAppointment() IN AMS.cs *****
    Public Function GetAMSAppointmentUserFieldDatasFiltered(ByVal AMSControl As Integer) As DTO.AMSAppointmentUserFieldData()

        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                'Configure updates or inserts to the data from the company configuration table as needed
                If Not ConfigureApptUserFieldData(AMSControl) Then Return Nothing
                'Return all the records that match the criteria sorted by name
                Dim AMSAppointmentUserFieldDatas() As DTO.AMSAppointmentUserFieldData = (
                From d In db.AMSAppointmentUserFieldDatas
                Where
                    (d.AMSApptUFDApptControl = AMSControl)
                Order By d.AMSApptUFDName
                Select selectDTOData(d, db)).ToArray()
                Return AMSAppointmentUserFieldDatas

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

#Region "TMS 365"

    ''***************************** NEW METHODS *****************************

    ''' <summary>
    ''' Inserts or Updates a record in AMSAppointmentUserFieldData
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <remarks>
    ''' Added By LVV on 5/24/18 for v-8.3 TMS365 Scheduler
    ''' Modified By RHR for v-8.3.0.002 on 10/26/2020
    '''     added logic to save user data to the book table via spUpdateBookAMSUserFieldData
    ''' </remarks>
    Public Sub InsertOrUpdateAMSAppointmentUserFieldData(ByVal oRecord As LTS.AMSAppointmentUserFieldData)
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try

                oRecord.AMSApptUFDModDate = Date.Now
                oRecord.AMSApptUFDModUser = Parameters.UserName

                If oRecord.AMSApptUFDControl = 0 Then
                    'Insert
                    db.AMSAppointmentUserFieldDatas.InsertOnSubmit(oRecord)
                Else
                    'Update
                    db.AMSAppointmentUserFieldDatas.Attach(oRecord, True)
                End If

                db.SubmitChanges()
                ' Modified By RHR for v-8.3.0.002 on 10/26/2020
                Try
                    Dim iAMSApptUFDControl As Integer = oRecord.AMSApptUFDControl
                    If iAMSApptUFDControl <> 0 Then
                        db.spUpdateBookAMSUserFieldData(iAMSApptUFDControl)
                    End If
                Catch ex As Exception
                    Utilities.SaveAppError("Unexpected Update Book AMS User Field Data Error: " & ex.Message, Me.Parameters)
                End Try

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateAMSAppointmentUserFieldData"), db)
            End Try
        End Using
    End Sub

#End Region

#End Region

#Region "Protected Methods"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = TryCast(oData, DTO.AMSAppointmentUserFieldData)
        'Create New Record
        Return New LTS.AMSAppointmentUserFieldData With {.AMSApptUFDControl = d.AMSApptUFDControl _
                                                       , .AMSApptUFDApptControl = d.AMSApptUFDApptControl _
                                                       , .AMSApptUFDCompAMSUserFieldSettingControl = d.AMSApptUFDCompAMSUserFieldSettingControl _
                                                       , .AMSApptUFDData = d.AMSApptUFDData _
                                                       , .AMSApptUFDName = d.AMSApptUFDName _
                                                       , .AMSApptUFDDesc = d.AMSApptUFDDesc _
                                                       , .AMSApptUFDModDate = Date.Now _
                                                       , .AMSApptUFDModUser = Parameters.UserName _
                                                       , .AMSApptUFDUpdated = If(d.AMSApptUFDUpdated Is Nothing, New Byte() {}, d.AMSApptUFDUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetAMSAppointmentUserFieldDataFiltered(Control:=CType(LinqTable, LTS.AMSAppointmentUserFieldData).AMSApptUFDControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DTO.QuickSaveResults
        Using db As New NGLMASAMSDataContext(ConnectionString)
            Try
                Dim source As LTS.AMSAppointmentUserFieldData = TryCast(LinqTable, LTS.AMSAppointmentUserFieldData)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.AMSAppointmentUserFieldDatas
                       Where d.AMSApptUFDControl = source.AMSApptUFDControl
                       Select New DTO.QuickSaveResults With {.Control = d.AMSApptUFDControl _
                                                            , .ModDate = d.AMSApptUFDModDate _
                                                            , .ModUser = d.AMSApptUFDModUser _
                                                            , .Updated = d.AMSApptUFDUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    Friend Function selectDTOData(ByVal d As LTS.AMSAppointmentUserFieldData, ByRef db As NGLMASAMSDataContext) As DTO.AMSAppointmentUserFieldData
        Return New DTO.AMSAppointmentUserFieldData With {.AMSApptUFDControl = d.AMSApptUFDControl _
                                                       , .AMSApptUFDApptControl = d.AMSApptUFDApptControl _
                                                       , .AMSApptUFDCompAMSUserFieldSettingControl = d.AMSApptUFDCompAMSUserFieldSettingControl _
                                                       , .AMSApptUFDData = d.AMSApptUFDData _
                                                       , .AMSApptUFDName = d.AMSApptUFDName _
                                                       , .AMSApptUFDDesc = d.AMSApptUFDDesc _
                                                       , .AMSApptUFDModDate = d.AMSApptUFDModDate _
                                                       , .AMSApptUFDModUser = d.AMSApptUFDModUser _
                                                       , .AMSApptUFDUpdated = d.AMSApptUFDUpdated.ToArray()}
    End Function

    Protected Function ConfigureApptUserFieldData(ByVal AMSApptControl As Integer) As Boolean

        Dim strBatchName As String = "ConfigureApptUserFieldData"
        Dim strProcName As String = "dbo.spConfigureApptUserFieldData"
        'Validate the parameter data
        If AMSApptControl = 0 Then Return False

        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@AMSApptControl", AMSApptControl)
        oCmd.Parameters.AddWithValue("@UserName", Left(Me.Parameters.UserName, 100))
        runNGLStoredProcedure(oCmd, strProcName, 0)
        Return True
    End Function

#End Region

End Class

