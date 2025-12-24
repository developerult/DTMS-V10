Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports TAR = NGL.FM.CarTar
Imports NGLCoreComm = NGL.Core.Communication
Imports DAT = NGL.FM.DAT

Imports LTTypeEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum
Imports LTSCEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum
Imports BSCEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidStatusCodeEnum
Imports NGL.Core.Utility
Imports Serilog
Imports Serilog.Events
Imports SerilogTracing
Public Class NGLBookBLL : Inherits BLLBaseClass

#Region " Enums "

    Public Enum AcceptRejectEnum As Integer
        Accepted = 0
        Rejected
        Expired
        Unfinalize
        Tendered
        Dropped
        Unassigned
        ModifyUnaccepted
    End Enum

    Public Enum AcceptRejectModeEnum As Integer
        MANUAL
        EDI
        WEB
        System
        Token
        DAT 'Added by LVV 5/17/16 for v-7.0.5.110 DAT
    End Enum

    Public Enum AMSDateTimeValidationResults As Integer
        ApptExistAndMatches             'the appointment exists in the calendar and both date and time values match
        ApptExistDateIsBeforeAppt       'the appointment exists but the date and time are before the scheduled appointment
        ApptExitsDateIsAfterAppt        'the appointment exists but the date and time are after the scheduled appointment
        ApptExitsDataAndTimeDoNotMatch  'the appointment exits but it is different (normally source is empty)
        ApptExitsButDateAndTimeAreEmpty 'the appointment exists but the associated calendar date and time values are empty
        ApptDoesNotExist                'the carrier appointment does not exist
    End Enum

    ''' <summary>
    ''' AMSApptStartDate: Appointment Date and Time;
    ''' AMSApptActualDateTime: Check in Date and Time;
    ''' AMSApptStartLoadingDateTime: Start (un)Loading Date and Time;
    ''' AMSApptFinishLoadingDateTime: Finished (un)Loading Date and Time;
    ''' AMSApptActLoadCompleteDateTime: Checkout Date and Time
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum AMSDateTimeValidationType As Integer
        AMSApptStartDate        'Appointment Date and Time
        AMSApptActualDateTime   'Check in Date and Time
        AMSApptStartLoadingDateTime 'Start (un)Loading Date and Time
        AMSApptFinishLoadingDateTime    'Finished (un)Loading Date and Time
        AMSApptActLoadCompleteDateTime 'Checkout Date and Time
    End Enum

#End Region

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLBookBLL"
        Me.Logger = Me.Logger.ForContext(Of NGLBookBLL)
    End Sub

#End Region

#Region " Properties "

#End Region

#Region "DAL Wrapper Methods"


    Public Function UpdateAssignedCarrier(ByVal bookControl As Integer,
                                 ByVal assignedCarrierNumber As String,
                                 ByVal assignedCarrierName As String) As Boolean
        Using operation = Me.Logger.StartActivity("UpdateAssignedCarrier(BookControl: {BookControl}, AssignedCarrierNumber: {AssignedCarrierNumber}, AssignedCarrierName: {AssignedCarrierName})", bookControl, assignedCarrierNumber, assignedCarrierName)
            Try
                Dim paramsCollection As New List(Of DTO.NGLStoredProcedureParameter)

                Dim item As New DTO.NGLStoredProcedureParameter

                item.ParName = "@BookShipCarrierName"
                item.ParValue = assignedCarrierName
                paramsCollection.Add(item)

                item = New DTO.NGLStoredProcedureParameter
                item.ParName = "@BookShipCarrierNumber"
                item.ParValue = assignedCarrierNumber
                paramsCollection.Add(item)

                item = New DTO.NGLStoredProcedureParameter
                item.ParName = "@BookControl"
                item.ParValue = bookControl.ToString
                paramsCollection.Add(item)

                Return NGLBatchProcessData.executeNGLStoredProcedure("spUpdateAssignedCarrier", "spUpdateAssignedCarrier", paramsCollection.ToArray, True, 1)

            Catch ex As FaultException
                operation.Complete(exception:=ex)
                Throw
            Catch ex As Exception
                operation.Complete(exception:=ex)
                throwUnExpectedFaultException(ex, buildProcedureName("UpdateAssignedCarrier"))
            End Try
        End Using

        Return False
    End Function


#Region "   NEXTrack sp conversion Methods"

    Public Function UpdateAssignedCarrierItems(ByVal bookControl As Integer,
                                 ByVal assignedProNumber As String,
                                 ByVal assignedCarrierNumber As String,
                                 ByVal assignedCarrierName As String) As Boolean
        Try
            Dim paramsCollection As New List(Of DTO.NGLStoredProcedureParameter)

            Dim item As New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookShipCarrierProNumber"
            item.ParValue = assignedProNumber
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookShipCarrierName"
            item.ParValue = assignedCarrierName
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookShipCarrierNumber"
            item.ParValue = assignedCarrierNumber
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookControl"
            item.ParValue = bookControl.ToString
            paramsCollection.Add(item)

            Return NGLBatchProcessData.executeNGLStoredProcedure("spUpdateAssignedCarrierItems", "spUpdateAssignedCarrierItems", paramsCollection.ToArray, True, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateAssignedCarrierItems"))
        End Try
        Return False
    End Function


    Public Function UpdateCarrDataPickupByCNS(ByVal bookControl As Integer,
                                 ByVal cnsNumber As String,
                                 ByVal pickupStopNumber As String,
                                 ByVal PickupScheduledAppointmentDate As Date,
                                 ByVal PickupScheduledAppointmentTime As Date,
                                 ByVal PickupActualArrivalDate As Date,
                                 ByVal PickupActualArrivalTime As Date,
                                 ByVal PickupStartLoadingDate As Date,
                                 ByVal PickupStartLoadingTime As Date,
                                 ByVal PickupFinishLoadingDate As Date,
                                 ByVal PickupFinishLoadingTime As Date,
                                 ByVal PickupActLoadCompleteDate As Date,
                                 ByVal PickupActLoadCompleteTime As Date,
                                 ByVal PickupDockPUAssignment As String,
                                 ByVal WhseAuthorizationNumber As String) As Boolean
        Try
            Dim paramsCollection As New List(Of DTO.NGLStoredProcedureParameter)

            Dim item As New DTO.NGLStoredProcedureParameter

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookControl"
            item.ParValue = bookControl.ToString()
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@CNS"
            item.ParValue = cnsNumber
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@PickupStopNumber"
            item.ParValue = pickupStopNumber
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrScheduleDate"
            item.ParValue = IIf(PickupScheduledAppointmentDate = DateTime.MinValue, Nothing, PickupScheduledAppointmentDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrScheduleTime"
            item.ParValue = IIf(PickupScheduledAppointmentTime = DateTime.MinValue, Nothing, PickupScheduledAppointmentTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrActualDate"
            item.ParValue = IIf(PickupActualArrivalDate = DateTime.MinValue, Nothing, PickupActualArrivalDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrActuaTime"
            item.ParValue = IIf(PickupActualArrivalTime = DateTime.MinValue, Nothing, PickupActualArrivalTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrStartLoadingDate"
            item.ParValue = IIf(PickupStartLoadingDate = DateTime.MinValue, Nothing, PickupStartLoadingDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrStartLoadingTime"
            item.ParValue = IIf(PickupStartLoadingTime = DateTime.MinValue, Nothing, PickupStartLoadingTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrFinishLoadingDate"
            item.ParValue = IIf(PickupFinishLoadingDate = DateTime.MinValue, Nothing, PickupFinishLoadingDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrFinishLoadingTime"
            item.ParValue = IIf(PickupFinishLoadingTime = DateTime.MinValue, Nothing, PickupFinishLoadingTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrActLoadComplete_Date"
            item.ParValue = IIf(PickupActLoadCompleteDate = DateTime.MinValue, Nothing, PickupActLoadCompleteDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrActLoadCompleteTime"
            item.ParValue = IIf(PickupActLoadCompleteTime = DateTime.MinValue, Nothing, PickupActLoadCompleteTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrDockPUAssigment"
            item.ParValue = PickupDockPUAssignment
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookWhseAuthorizationNo"
            item.ParValue = WhseAuthorizationNumber
            paramsCollection.Add(item)

            Return NGLBatchProcessData.executeNGLStoredProcedure("spUpdateCarrDataPickupByCNS", "spUpdateCarrDataPickupByCNS", paramsCollection.ToArray, True, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateCarrDataPickupByCNS"))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bookControl"></param>
    ''' <param name="Contact"></param>
    ''' <param name="Comments"></param>
    ''' <param name="Status"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' This method needs to be modified to call the Linq WCF 
    ''' service wrapper for spUpdateBookTracksForLoad, an overload needs
    ''' to be created to support then new localization data fields for 
    ''' system generated load status comments.  User entered load status 
    ''' comments will still work the same and will be displayed in the 
    ''' language they were entered.
    ''' </remarks>
    Public Function UpdateBookTracksByCNS(ByVal bookControl As Integer,
                                 ByVal Contact As String,
                                 ByVal Comments As String,
                                 ByVal Status As String) As Boolean
        Try
            Return UpdateBookTracksForLoad(bookControl, Comments, Status, Contact)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateBookTracksByCNS"))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="BookTrackComment"></param>
    ''' <param name="BookTrackStatus"></param>
    ''' <param name="BookTrackContact"></param>
    ''' <param name="BookTrackDate"></param>
    ''' <param name="BookTrackCommentLocalized"></param>
    ''' <param name="BookTrackCommentKeys"></param>
    ''' <remarks></remarks>
    Public Function UpdateBookTracksForLoad(ByVal BookControl As Integer,
                                       ByVal BookTrackComment As String,
                                       ByVal BookTrackStatus As Integer,
                                       ByVal BookTrackContact As String,
                                       Optional ByVal BookTrackDate As System.Nullable(Of Date) = Nothing,
                                       Optional ByVal BookTrackCommentLocalized As String = Nothing,
                                       Optional ByVal BookTrackCommentKeys As String = Nothing,
                                       Optional ByVal IgnoreErrors As Boolean = False) As Boolean
        Dim blnRet As Boolean = True
        Try
            NGLBookTrackData.UpdateBookTracksForLoad(BookControl, BookTrackComment, BookTrackStatus, BookTrackContact, BookTrackDate, BookTrackCommentLocalized, BookTrackCommentKeys)
        Catch ex As Exception
            blnRet = False
            If Not IgnoreErrors Then Throw
        End Try
        Return blnRet
    End Function

    Public Function UpdateCarrDataDeliveryByCNS(ByVal bookControl As Integer,
                                 ByVal cnsNumber As String,
                                 ByVal stopNumber As String,
                                 ByVal DeliveryScheduledAppointmentDate As Date,
                                 ByVal DeliveryScheduledAppointmentTime As Date,
                                 ByVal DeliveryActualArrivalDate As Date,
                                 ByVal DeliveryActualArrivalTime As Date,
                                 ByVal DeliveryStartUnloadingDate As Date,
                                 ByVal DeliveryStartUnloadingTime As Date,
                                 ByVal DeliveryFinishUnloadingDate As Date,
                                 ByVal DeliveryFinishUnloadingTime As Date,
                                 ByVal DeliveryActUnloadCompDate As Date,
                                 ByVal DeliveryActUnloadCompTime As Date,
                                 ByVal DeliveryDockDelAssignment As String,
                                 ByVal WhseAuthorizationNumber As String) As Boolean
        Try
            Dim paramsCollection As New List(Of DTO.NGLStoredProcedureParameter)

            Dim item As New DTO.NGLStoredProcedureParameter

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookControl"
            item.ParValue = bookControl.ToString()
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@CNS"
            item.ParValue = cnsNumber
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookStopNumber"
            item.ParValue = stopNumber
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrApptDate"
            item.ParValue = IIf(DeliveryScheduledAppointmentDate = DateTime.MinValue, Nothing, DeliveryScheduledAppointmentDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrApptTime"
            item.ParValue = IIf(DeliveryScheduledAppointmentTime = DateTime.MinValue, Nothing, DeliveryScheduledAppointmentTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrActDate"
            item.ParValue = IIf(DeliveryActualArrivalDate = DateTime.MinValue, Nothing, DeliveryActualArrivalDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrActTime"
            item.ParValue = IIf(DeliveryActualArrivalTime = DateTime.MinValue, Nothing, DeliveryActualArrivalTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrStartUnloadingDate"
            item.ParValue = IIf(DeliveryStartUnloadingDate = DateTime.MinValue, Nothing, DeliveryStartUnloadingDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrStartUnloadingTime"
            item.ParValue = IIf(DeliveryStartUnloadingTime = DateTime.MinValue, Nothing, DeliveryStartUnloadingTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrFinishUnloadingDate"
            item.ParValue = IIf(DeliveryFinishUnloadingDate = DateTime.MinValue, Nothing, DeliveryFinishUnloadingDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrFinishUnloadingTime"
            item.ParValue = IIf(DeliveryFinishUnloadingTime = DateTime.MinValue, Nothing, DeliveryFinishUnloadingTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrActUnloadCompDate"
            item.ParValue = IIf(DeliveryActUnloadCompDate = DateTime.MinValue, Nothing, DeliveryActUnloadCompDate.ToString("MM/dd/yyyy"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrActUnloadCompTime"
            item.ParValue = IIf(DeliveryActUnloadCompTime = DateTime.MinValue, Nothing, DeliveryActUnloadCompTime.ToString("HH:mm:ss tt"))
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookCarrDockDelAssignment"
            item.ParValue = DeliveryDockDelAssignment
            paramsCollection.Add(item)

            item = New DTO.NGLStoredProcedureParameter
            item.ParName = "@BookWhseAuthorizationNo"
            item.ParValue = WhseAuthorizationNumber
            paramsCollection.Add(item)


            Return NGLBatchProcessData.executeNGLStoredProcedure("spUpdateCarrDataDeliveryByCNS", "spUpdateCarrDataDeliveryByCNS", paramsCollection.ToArray, True, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("UpdateCarrDataDeliveryByCNS"))
        End Try
        Return False
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bookControl"></param>
    ''' <param name="invoiceNumber"></param>
    ''' <param name="invoiceAmount"></param>
    ''' <param name="carrierControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.004 on 01/02/2020
    '''   Added new logic to follow new SettlementSave logic needed to proces 
    '''   Quick Edit Freight Bills.  replaces calls to spInsertFreightBillWeb50
    '''   Create a new Settlement Save object
    ''' </remarks>
    Public Function InsertFreightBillWeb50(ByVal bookControl As Integer,
                                 ByVal invoiceNumber As String,
                                 ByVal invoiceAmount As Decimal,
                                 ByVal carrierControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Try

            Dim dtCurrent = Date.Now()
            'first we need to get the settlementSave Data from the book table
            Dim oNGLData = NGLBookData.GetRecordFiltered(bookControl)
            If oNGLData Is Nothing Then
                Return False 'no bookings
            End If
            Dim bData As DTO.Book = DirectCast(oNGLData, DTO.Book)
            If bData.BookControl = 0 Or bData.BookCarrierControl <> carrierControl Then
                Return False 'no bookings for carrier
            End If
            Dim oBookCarrierComp = NGLBookData.GetBookCarrierCompBySHID(bData.BookSHID)
            Dim s As New DAL.Models.SettlementSave()

            s.APBillDate = dtCurrent
            s.APControl = 0
            s.APCustomerID = oBookCarrierComp.CompNumber
            s.APReceivedDate = dtCurrent
            s.BookCarrBLNumber = bData.BookCarrBLNumber
            s.BookControl = bookControl
            s.BookFinAPActWgt = bData.BookTotalWgt
            s.BookSHID = bData.BookSHID
            s.CarrierControl = carrierControl
            s.CarrierNumber = oBookCarrierComp.CarrierNumber
            s.CompControl = bData.BookCustCompControl
            s.ID = 0
            'we use the Expected Fees to populate all the fees for quick edit
            'no fees should be missing 
            s.Fees = NGLEDIData.GetDataForEDIExpectedFees(bData.BookSHID, oBookCarrierComp.CarrierNumber, oBookCarrierComp.CompNumber)
            s.InvoiceAmt = invoiceAmount
            s.InvoiceNo = invoiceNumber
            'Modified by RHR for v-8.2.1.004 on 01/10/2020
            'we always set line haul to zero when performing quick edit on freight bills
            'this way the processBilledFees procedure can accurately update the correct line haul
            s.LineHaul = 0
            'If s.Fees Is Nothing OrElse s.Fees.Count() < 1 Then
            '    s.LineHaul = invoiceAmount
            '    's.TotalFuel = 0
            'Else
            '    'set the linehaul (carriercost) equal to the total minus fees
            '    s.LineHaul = invoiceAmount - s.Fees.Sum(Function(x) x.Cost)
            '    'set the total fuel equal to sum of all fees with EDI Code FUE
            '    's.TotalFuel = s.Fees.Where(Function(x) x.EDICode = "FUE").Sum(Function(x) x.Cost)
            'End If
            Dim oResults = NGLBookData.SettlementSave(s, False)
            'add logic to recalculate Costs
            If oResults.Success = True Then
                Dim bllBookRev As New BLL.NGLBookRevenueBLL(Parameters)
                Dim carrierCostResults = bllBookRev.RecalculateUsingLineHaul(oResults.BookControl)
                If ((carrierCostResults Is Nothing) OrElse (carrierCostResults.BookRevs Is Nothing OrElse carrierCostResults.BookRevs.Count < 1)) Then
                    Dim sDetails = String.Format(" SHID {0} and freight bill number {1}. ", s.BookSHID, s.InvoiceNo)
                    NGLBookData.addAuditMessage(sDetails, Nothing, oResults.Control, DAL.NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("InsertFreightBillWeb50"))
                    oResults.Success = False
                End If
            End If
            'logic to audit freight bill
            If oResults.Success = True Then
                oResults = NGLBookData.UpdateAndAuditAPMassEntry(oResults, oResults.Control, s.BookSHID, oResults.BookControl, s.InvoiceNo, False)
            End If

            blnRet = oResults.Success
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            NGLAPMassEntryData.throwUnExpectedFaultException(ex, buildProcedureName("InsertFreightBillWeb50"), DAL.sysErrorParameters.sysErrorState.SystemLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' This  function is called from NEXTrack.
    ''' ResponseCodes are:
    ''' 0 = Accepted
    ''' 1 = Rejected
    ''' 2 = Expired
    ''' </summary>
    ''' <param name="bookControl"></param>
    ''' <param name="responseCode"></param>
    ''' <param name="carrierContControl"></param>
    ''' <param name="carrierControl"></param>
    ''' <param name="sendEmail"></param>
    ''' <param name="trucComment"></param>
    ''' <param name="UserName"></param>
    ''' <param name="emailTo"></param>
    ''' <param name="emailCc"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' TODO need to update NEXTrack to handle the new WCFresults functionality.  For now we just return
    ''' true as false as before.
    ''' </remarks>
    Public Function CarrierAcceptOrRejectLoadWeb(ByVal bookControl As Integer,
                                ByVal responseCode As Integer,
                                ByVal carrierContControl As Integer,
                                ByVal carrierControl As Integer,
                                ByVal sendEmail As Boolean,
                                ByVal trucComment As String,
                                ByVal UserName As String,
                                ByVal emailTo As String,
                                ByVal emailCc As String) As Boolean
        Try
            'Code Change PFM 7/7/2014 - No longer using the stored procedure. Convert to use the new AcceptJect BLL method.
            Dim acceptReject As AcceptRejectEnum
            'convert the reponse code from the caller to a enum used in the bll.
            Select Case responseCode
                Case 0
                    acceptReject = AcceptRejectEnum.Accepted
                Case 1
                    acceptReject = AcceptRejectEnum.Rejected
                Case 2
                    acceptReject = AcceptRejectEnum.Expired
                Case Else
                    Return False ''neeed to add logic to add to log.
            End Select

            Dim result As New DTO.WCFResults()
            result = AcceptOrRejectLoad(bookControl,
                            carrierControl,
                            carrierContControl,
                            acceptReject,
                            sendEmail,
                            trucComment,
                            0,
                            emailTo,
                            emailCc,
                            AcceptRejectModeEnum.WEB,
                            UserName)
            If result Is Nothing OrElse result.Success = False Then
                Return False
            Else
                Return True
            End If




            'Dim paramsCollection As New List(Of DTO.NGLStoredProcedureParameter)

            'Dim item As New DTO.NGLStoredProcedureParameter
            'item.ParName = "@BookControl"
            'item.ParValue = bookControl.ToString
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@CarrierControl"
            'item.ParValue = carrierControl.ToString
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@CarrierContControl"
            'item.ParValue = carrierContControl.ToString
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@AcceptRejectCode"
            'item.ParValue = responseCode.ToString
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@sendEMail"
            'item.ParValue = IIf(sendEmail = True, 1, 0).ToString
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@BookTrackComment"
            'item.ParValue = trucComment
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@BookTrackStatus"
            'item.ParValue = "0"
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@WebUserName"
            'item.ParValue = UserName
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@NotificationEMailAddress"
            'item.ParValue = emailTo
            'paramsCollection.Add(item)

            'item = New DTO.NGLStoredProcedureParameter
            'item.ParName = "@NotificationEMailAddressCc"
            'item.ParValue = emailCc
            'paramsCollection.Add(item)

            'Return NGLBatchProcessData.executeNGLStoredProcedure("spCarrierAcceptOrRejectLoad", "spCarrierAcceptOrRejectLoad", paramsCollection.ToArray, True, 1)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("CarrierAcceptOrRejectLoadWeb"))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Update the Load Accept or Reject Status
    ''' </summary>
    ''' <param name="bookControl"></param>
    ''' <param name="acceptReject"></param>
    ''' <param name="carrierContControl"></param>
    ''' <param name="carrierControl"></param>
    ''' <param name="trucComment"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.0 TMS 365 on 12/14/2017
    '''   logic to accept or reject loads from the new Load Tender page
    ''' </remarks>
    Public Function CarrierAcceptOrRejectLoad365(ByVal bookControl As Integer,
                                ByVal acceptReject As AcceptRejectEnum,
                                ByVal carrierContControl As Integer,
                                ByVal carrierControl As Integer,
                                ByVal trucComment As String,
                                ByVal UserName As String) As DTO.WCFResults
        Dim oResult As New DTO.WCFResults
        oResult.Success = False
        Try
            'get the email information
            Dim emailTo As String = ""
            Dim emailCc As String = ""
            Dim dictEmails = NGLBookData.GetAcceptRejectEmailsFromComp(bookControl)
            If Not dictEmails Is Nothing Then
                Select Case acceptReject
                    Case AcceptRejectEnum.Accepted
                        If dictEmails.ContainsKey("CompAcceptedLoadsTo") Then emailTo = dictEmails("CompAcceptedLoadsTo")
                        If dictEmails.ContainsKey("CompAcceptedLoadsCc") Then emailCc = dictEmails("CompAcceptedLoadsCc")
                    Case AcceptRejectEnum.Rejected
                        If dictEmails.ContainsKey("CompRejectedLoadsTo") Then emailTo = dictEmails("CompRejectedLoadsTo")
                        If dictEmails.ContainsKey("CompRejectedLoadsCc") Then emailCc = dictEmails("CompRejectedLoadsCc")
                    Case AcceptRejectEnum.Expired
                        If dictEmails.ContainsKey("CompExpiredLoadsTo") Then emailTo = dictEmails("CompExpiredLoadsTo")
                        If dictEmails.ContainsKey("CompExpiredLoadsCc") Then emailCc = dictEmails("CompExpiredLoadsCc")
                End Select

            End If

            oResult = AcceptOrRejectLoad(bookControl,
                            carrierControl,
                            carrierContControl,
                            acceptReject,
                            True,
                            trucComment,
                            0,
                            emailTo,
                            emailCc,
                            AcceptRejectModeEnum.WEB,
                            UserName)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("CarrierAcceptOrRejectLoad365"))
        End Try
        Return oResult
    End Function


#End Region

#Region "   Standard CRUD Method Wrappers"

#Region "   Book Data"

    Public Function AssignTruckStopCarrier(ByVal StopName As String,
                                   ByVal ID1 As String,
                                   ByVal ID2 As String,
                                   ByVal TruckID As String,
                                   ByVal SeqNbr As Integer,
                                   ByVal DistToPrev As Double,
                                   ByVal TotalRouteCost As Double,
                                   ByVal ConsNumber As String) As DTO.CarrierCostResults
        Try
            Dim TSCarrierInfo = NGLBookData.AssignTruckStopCarrier(StopName, ID1, ID2, TruckID, SeqNbr, DistToPrev, TotalRouteCost, ConsNumber)
            Dim BookControl As Integer = If(TSCarrierInfo.BookControl, 0)
            Dim CarrierControl As Integer = If(TSCarrierInfo.CarrierControl, 0)
            Dim results As New DTO.CarrierCostResults
            If BookControl = 0 Then
                throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_InvalidParentKeyField, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_CannotSaveParentKeyRequired, New List(Of String) From {"BookLoadControl", ID1}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_DataValidationFailure, True, True)
                Return results
            End If
            results = BookRevenueBLL.AssignCarrier(BookControl, CarrierControl, 0, 0)
            'If a tariff exits the load should be assigned to the carrier 'P' Status, else it should stay at 'N' status.
            If results.CarriersByCost.Count > 0 Then
                ProcessNewTransCode(BookControl, "P", "N") 'Optional ByVal intValidationFlags As Int64 = 0
            End If

            Return results
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AssignTruckStopCarrier"))
        End Try
        Return Nothing
    End Function

    Public Function CreateBook(ByVal oData As DTO.Book) As DTO.Book
        NGLBookData.CreateRecord(oData)
        Return returnCalculatedBookingData(False)
    End Function

    Public Function CreateBookWithDetails(ByVal oData As DTO.Book) As DTO.Book
        NGLBookData.CreateRecordWithDetails(oData)
        Return returnCalculatedBookingData(True)
    End Function

    Public Sub DeleteBook(ByVal oData As DTO.Book)
        NGLBookData.DeleteRecord(oData)
    End Sub

    Public Function UpdateBook(ByVal oData As DTO.Book) As DTO.Book
        NGLBookData.UpdateRecord(oData)
        Return returnCalculatedBookingData(False)
    End Function

    Public Function UpdateBookQuick(ByVal oData As DTO.Book) As DTO.QuickSaveResults
        NGLBookData.UpdateRecordQuick(oData)
        Return returnCalculatedBookingDataQuick()
    End Function

    Public Sub UpdateBookNoReturn(ByVal oData As DTO.Book)
        NGLBookData.UpdateRecordNoReturn(oData)
        processRecalculateParameters(NGLBookData.BookDependencyResult, NGLBookData.LastProcedureName)
    End Sub

    Public Sub UpdateBookWithDetailsNoReturn(ByVal oData As DTO.Book)
        NGLBookData.UpdateRecordWithDetailsNoReturn(oData)
        processRecalculateParameters(NGLBookData.BookDependencyResult, NGLBookData.LastProcedureName)
    End Sub

    Public Function UpdateBookWithDetails(ByVal oData As DTO.Book) As DTO.Book
        NGLBookData.UpdateRecordWithDetails(oData)
        Return returnCalculatedBookingData(True)
    End Function

    Public Function SilentTenderFinalized(ByVal strOrderNumber As String,
                                                    ByVal intOrderSequence As Integer,
                                                    ByVal intDefCompNumber As Integer) As String
        Dim strRet As String = "Success!"
        Try
            Dim oResult = NGLBookData.SilentTenderFinalized(strOrderNumber, intOrderSequence, intDefCompNumber)
            If Not oResult Is Nothing Then
                strRet = oResult.RetMsg
                If oResult.ErrNumber <> 0 Then
                    If oResult.ErrNumber < 10 Then
                        createAutoUpdateChangesToFinalizedFailedSubscriptionAlert("", intDefCompNumber, strOrderNumber, intOrderSequence, oResult.RetMsg)
                    Else
                        throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {NGLBookData.LastProcedureName, oResult.ErrNumber, oResult.RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)
                    End If

                ElseIf oResult.MustRecalculate AndAlso oResult.BookControl.HasValue AndAlso oResult.BookControl.Value <> 0 Then
                    BookRevenueBLL.AssignCarrier(oResult.BookControl.Value, True)
                    NGLBatchProcessData.generatePickListRecord(oResult.BookControl.Value, If(oResult.BookConsPrefix, ""))
                End If
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("SilentTenderFinalized"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return strRet
    End Function


#End Region

#Region "   BookItem Data"

    Public Function CreateBookItem(ByVal oData As DTO.BookItem) As DTO.BookItem
        NGLBookItemData.CreateRecord(oData)
        Return returnUpdatedBookItemData(NGLBookItemData.LastBookItemControl)
    End Function

    Public Sub DeleteBookItem(ByVal oData As DTO.BookItem)
        NGLBookItemData.DeleteRecord(oData)
        If Not NGLBookItemData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookItemData.BookDependencyResult.BookControl, 0),
                                    If(NGLBookItemData.BookDependencyResult.ErrNumber, 0),
                                    NGLBookItemData.BookDependencyResult.RetMsg,
                                    NGLBookItemData.LastProcedureName,
                                    If(NGLBookItemData.BookDependencyResult.MustRecalculate, False))
    End Sub

    Public Function UpdateBookItem(ByVal oData As DTO.BookItem) As DTO.BookItem
        NGLBookItemData.UpdateRecord(oData)
        Return returnUpdatedBookItemData(oData.BookItemControl)
    End Function

    Public Function UpdateBookItemQuick(ByVal oData As DTO.BookItem) As DTO.QuickSaveResults
        NGLBookItemData.UpdateQuick(oData, NGLBookItemData.getLinqTable())
        'NGLBookItemData.UpdateRecordQuick(oData)
        Return returnUpdatedBookItemDataQuick(oData.BookItemControl)
    End Function

    Public Sub UpdateBookItemNoReturn(ByVal oData As DTO.BookItem)
        NGLBookItemData.UpdateRecordNoReturn(oData)
        If Not NGLBookItemData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookItemData.BookDependencyResult.BookControl, 0),
                                    If(NGLBookItemData.BookDependencyResult.ErrNumber, 0),
                                    NGLBookItemData.BookDependencyResult.RetMsg,
                                    NGLBookItemData.LastProcedureName,
                                    If(NGLBookItemData.BookDependencyResult.MustRecalculate, False))

    End Sub

    ''' <summary>
    ''' Save all changes in the Batch to the database and recalculate costs if needed, like when the pallet count changes.
    ''' </summary>
    ''' <param name="oRecords"></param>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.102 8/16/2016
    '''   Checks all records in the batch for optimistic conncurrance before saving and recalculating costs
    ''' </remarks>
    Public Sub UpdateBookItemBatch(ByVal oRecords As DTO.BookItem())
        NGLBookItemData.UpdateBookItemBatch(oRecords)
        If Not NGLBookItemData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookItemData.BookDependencyResult.BookControl, 0),
                                    If(NGLBookItemData.BookDependencyResult.ErrNumber, 0),
                                    NGLBookItemData.BookDependencyResult.RetMsg,
                                    NGLBookItemData.LastProcedureName,
                                    If(NGLBookItemData.BookDependencyResult.MustRecalculate, False))
    End Sub

#End Region

#Region "   BookLoad Data"

    Public Function CreateBookLoad(ByVal oData As DTO.BookLoad) As DTO.BookLoad
        NGLBookLoadData.CreateRecord(oData)
        Return returnUpdatedBookLoadData()
    End Function

    Public Sub DeleteBookLoad(ByVal oData As DTO.BookLoad)
        NGLBookLoadData.DeleteRecord(oData)
        If Not NGLBookLoadData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookLoadData.BookDependencyResult.BookControl, 0),
                                    If(NGLBookLoadData.BookDependencyResult.ErrNumber, 0),
                                    NGLBookLoadData.BookDependencyResult.RetMsg,
                                    NGLBookLoadData.LastProcedureName,
                                    If(NGLBookLoadData.BookDependencyResult.MustRecalculate, False))
    End Sub

    Public Function UpdateBookLoad(ByVal oData As DTO.BookLoad) As DTO.BookLoad
        NGLBookLoadData.UpdateRecord(oData)
        Return returnUpdatedBookLoadData()
    End Function

    Public Function UpdateBookLoadQuick(ByVal oData As DTO.BookLoad) As DTO.QuickSaveResults
        NGLBookLoadData.UpdateRecordQuick(oData)
        Return returnUpdatedBookLoadDataQuick()
    End Function

    Public Sub UpdateBookLoadNoReturn(ByVal oData As DTO.BookLoad)
        NGLBookLoadData.UpdateRecordNoReturn(oData)
        If Not NGLBookLoadData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookLoadData.BookDependencyResult.BookControl, 0),
                                    If(NGLBookLoadData.BookDependencyResult.ErrNumber, 0),
                                    NGLBookLoadData.BookDependencyResult.RetMsg,
                                    NGLBookLoadData.LastProcedureName,
                                    If(NGLBookLoadData.BookDependencyResult.MustRecalculate, False))
    End Sub

#End Region

#Region "   BookLoadDetail Data"

    Public Function CreateBookLoadDetail(ByVal oData As DTO.BookLoadDetail) As DTO.BookLoadDetail
        Return NGLBookLoadDetailData.CreateRecord(oData)
    End Function

    Public Sub DeleteBookLoadDetail(ByVal oData As DTO.BookLoadDetail)
        NGLBookLoadDetailData.DeleteRecord(oData)
    End Sub

    Public Function UpdateBookLoadDetail(ByVal oData As DTO.BookLoadDetail) As DTO.BookLoadDetail
        NGLBookLoadDetailData.UpdateRecord(oData)
        If Not NGLBookLoadDetailData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookLoadDetailData.BookDependencyResult.BookControl, 0),
                                     If(NGLBookLoadDetailData.BookDependencyResult.ErrNumber, 0),
                                     NGLBookLoadDetailData.BookDependencyResult.RetMsg,
                                     NGLBookLoadDetailData.LastProcedureName,
                                     If(NGLBookLoadDetailData.BookDependencyResult.MustRecalculate, False))
        Return NGLBookLoadDetailData.GetBookLoadDetailFiltered(oData.BookControl)
    End Function

    Public Sub UpdateBookLoadDetailNoReturn(ByVal oData As DTO.BookLoadDetail)
        NGLBookLoadDetailData.UpdateRecordNoReturn(oData)
        If Not NGLBookLoadDetailData.BookDependencyResult Is Nothing Then _
           processRecalculateParameters(If(NGLBookLoadDetailData.BookDependencyResult.BookControl, 0),
                                    If(NGLBookLoadDetailData.BookDependencyResult.ErrNumber, 0),
                                    NGLBookLoadDetailData.BookDependencyResult.RetMsg,
                                    NGLBookLoadDetailData.LastProcedureName,
                                    If(NGLBookLoadDetailData.BookDependencyResult.MustRecalculate, False))
    End Sub

#End Region

#Region "   NGLLoadStatusBoard Data"

    Public Function UpdatevLoadStatusBoardData(ByVal oData As DTO.vLoadStatusBoard) As DTO.vLoadStatusBoard
        With NGLLoadStatusBoardData
            .UpdateRecord(oData)
            If Not .BookDependencyResult Is Nothing Then _
                processRecalculateParameters(If(.BookDependencyResult.BookControl, 0),
                                         If(.BookDependencyResult.ErrNumber, 0),
                                         .BookDependencyResult.RetMsg,
                                         .LastProcedureName,
                                         If(.BookDependencyResult.MustRecalculate, False))
            Return .GetvLoadStatusBoardFiltered(oData.BookControl)
        End With
    End Function

    Public Function UpdatevLoadStatusBoardDataQuick(ByVal oData As DTO.vLoadStatusBoard) As DTO.QuickSaveResults
        With NGLLoadStatusBoardData
            .UpdateRecord(oData)
            If Not .BookDependencyResult Is Nothing Then _
                processRecalculateParameters(If(.BookDependencyResult.BookControl, 0),
                                         If(.BookDependencyResult.ErrNumber, 0),
                                         .BookDependencyResult.RetMsg,
                                         .LastProcedureName,
                                         If(.BookDependencyResult.MustRecalculate, False))
            Return .QuickSaveResults(oData.BookControl)
        End With
    End Function

    Public Sub UpdatevLoadStatusBoardDataNoReturn(ByVal oData As DTO.vLoadStatusBoard)
        With NGLLoadStatusBoardData
            .UpdateRecord(oData)
            If Not .BookDependencyResult Is Nothing Then _
                processRecalculateParameters(If(.BookDependencyResult.BookControl, 0),
                                         If(.BookDependencyResult.ErrNumber, 0),
                                         .BookDependencyResult.RetMsg,
                                         .LastProcedureName,
                                         If(.BookDependencyResult.MustRecalculate, False))
        End With
    End Sub

#End Region

#Region "   vBookConsData"

    Public Sub DeleteBookConsData(ByVal oData As DTO.vBookCons)
        NGLvBookConsData.DeleteRecord(oData)
    End Sub

    Public Function UpdatevBookConsData(ByVal oData As DTO.vBookCons) As DTO.vBookCons
        NGLvBookConsData.UpdateRecord(oData)
        Return returnCalculatedvBookConsData()
    End Function

    Public Function UpdatevBookConsDataQuick(ByVal oData As DTO.vBookCons) As DTO.QuickSaveResults
        NGLvBookConsData.UpdateRecordQuick(oData)
        Return returnCalculatedvBookConsDataQuick()
    End Function

    Public Sub UpdatevBookConsDataNoReturn(ByVal oData As DTO.vBookCons)
        NGLvBookConsData.UpdateRecordNoReturn(oData)
        processRecalculateParameters(NGLvBookConsData.BookDependencyResult, NGLvBookConsData.LastProcedureName)
    End Sub

#End Region

#Region "NGLSolutionData"

    ''' <summary>
    ''' Not used in production.
    ''' </summary>
    ''' <param name="TruckFilter"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLoadPlanningTrucksFiltered(ByVal TruckFilter As DTO.LoadPlanningTruckDataFilter) As DTO.tblSolutionTruck()
        Dim l As DTO.tblSolutionTruck() = NGLLoadPlanningTruckData.GetLoadPlanningTrucksFiltered(TruckFilter)

        If l IsNot Nothing And l.Length > 0 Then
            For Each item As DTO.tblSolutionTruck In l
                If item IsNot Nothing Then
                    Dim intParentID As Integer = 0
                    Dim intNextTreeID As Integer = 0
                    Dim newDetails As New List(Of DTO.tblSolutionDetail)(item.SolutionDetails)
                    newDetails.AddRange(newDetails)
                    'build tree
                    Dim oRoot As New DTO.NEXTrackTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = "NEXTrackTreeNode", .Name = item.CNS, .Text = item.CNS, .Description = item.CNS}
                    intParentID = oRoot.TreeID
                    oRoot.HasChildren = True
                    'get the orig stop numbers by 1
                    'get max orig stop number.
                    Dim maxStopNum = item.SolutionDetails.AsEnumerable().Max(Function(r) Max(r.SolutionDetailOrigStopNumber, r.SolutionDetailDestStopNumber))
                    For stops As Integer = 1 To maxStopNum
                        Dim stopRoots As New DTO.NEXTrackTreeNode With {.TreeID = incrementID(intNextTreeID), .ParentTreeID = intParentID, .ClassName = ""}
                        intParentID = stopRoots.TreeID
                        Dim currentStop As Integer = stops
                        stopRoots.Name = "S" & currentStop
                        stopRoots.Text = "S" & currentStop
                        stopRoots.Description = "S" & currentStop
                        Dim origStops = (From os In item.SolutionDetails Where os.SolutionDetailOrigStopNumber = currentStop Select selectNEXTrackNode(os, intParentID, intNextTreeID, True)).ToList
                        Dim destStops = (From os In item.SolutionDetails Where os.SolutionDetailDestStopNumber = currentStop Select selectNEXTrackNode(os, intParentID, intNextTreeID, False)).ToList
                        If (origStops IsNot Nothing AndAlso origStops.Count > 0) Or (destStops IsNot Nothing AndAlso destStops.Count > 0) Then
                            stopRoots.HasChildren = True
                            stopRoots.Items.AddRange(origStops)
                            stopRoots.Items.AddRange(destStops)
                        Else
                            stopRoots.HasChildren = False
                        End If
                        If stopRoots.HasChildren Then
                            oRoot.Items.Add(stopRoots)
                        End If
                    Next
                    item.Stops = oRoot
                End If
            Next
        End If
        Return l
    End Function
    Private Function Max(ByVal origStopNo As Integer, ByVal destStopNo As Integer)
        Return IIf(origStopNo > destStopNo, origStopNo, destStopNo)
    End Function

    Private Function selectNEXTrackNode(ByVal detail As DTO.tblSolutionDetail, ByVal parentTreeID As Integer, ByRef intNextTreeID As Integer, ByVal selectOrig_Dest As Boolean) As DTO.NEXTrackTreeNode
        Dim address As String
        Dim name As String
        Dim node = New DTO.NEXTrackTreeNode
        If selectOrig_Dest Then
            'origin selection
            address = detail.SolutionDetailOrigCity & " " & detail.SolutionDetailOrigState
            name = "ORIG|" & " " & detail.SolutionDetailOrigName
        Else
            'destination selection
            address = detail.SolutionDetailDestCity & " " & detail.SolutionDetailDestState
            name = "DEST|" & " " & detail.SolutionDetailDestName
        End If

        node.HasChildren = False
        node.ClassName = "NEXTrackTreeNode"
        node.Control = detail.SolutionDetailBookControl
        node.Description = address
        node.Name = name
        node.ParentTreeID = parentTreeID
        node.Text = name
        node.TreeID = intNextTreeID

        intNextTreeID = incrementID(intNextTreeID)
        Return node
    End Function

#End Region

#End Region

#End Region

#Region " Public Methods"

    ''' <summary>
    ''' Wrapper method to InsertNewBookingDuplicate, returns the load board data, does not insert a new order number
    ''' </summary>
    ''' <param name="iBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.3.0.003 on 01/30/2021
    '''     intended to be call from D365 to duplicate the current pro
    '''     does not insert an order number    '''     
    ''' </remarks>
    Public Function DuplicatePro(ByVal iBookControl As Integer) As LTS.vBookLoadBoard
        Dim sNewPRONumber As String = ""
        Dim oBook = NGLBookData.GetvBookLoadBoard(iBookControl)
        If Not oBook Is Nothing AndAlso oBook.BookControl = iBookControl Then
            sNewPRONumber = NGLBatchProcessData.InsertNewBookingDuplicate(oBook.BookCustCompControl, oBook.BookProNumber)
        End If

        Return NGLBookData.GetvBookLoadBoard(sNewPRONumber)
    End Function

    ''' <summary>
    ''' Wrapper method to CreateBookingOrderSequence, returns the load board data
    ''' </summary>
    ''' <param name="iBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.3.0.003 on 01/30/2021
    '''     intended to be call from D365 to insert a new booking sequence for the current pro
    '''     typiclly used for Cross dock or Trans load logic         
    ''' </remarks>
    Public Function NewSequence(ByVal iBookControl As Integer) As LTS.vBookLoadBoard
        Dim sNewPRONumber As String = ""
        Dim oBook = NGLBookData.GetvBookLoadBoard(iBookControl)
        If Not oBook Is Nothing AndAlso oBook.BookControl = iBookControl Then
            sNewPRONumber = NGLBatchProcessData.CreateBookingOrderSequence(oBook.BookCustCompControl, oBook.BookProNumber)
        End If

        Return NGLBookData.GetvBookLoadBoard(sNewPRONumber)
    End Function

    ''' <summary>
    ''' checks for an existing appointment and compares the sourceDate with the associated date in the calendar based on Validation Type
    ''' </summary>
    ''' <param name="sourceDate"></param>
    ''' <param name="ApptControl"></param>
    ''' <param name="ValidationType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function validateAMSDateTimes(ByVal sourceDate As System.Nullable(Of Date), ByVal ApptControl As Integer, ByVal ValidationType As AMSDateTimeValidationType, ByRef strApptDateTime As String) As AMSDateTimeValidationResults

        Dim enmRet As AMSDateTimeValidationResults = AMSDateTimeValidationResults.ApptDoesNotExist
        'GetAMSAppointmentFiltered manages exceptions and throws a fault exception on error
        'get the AMS Appointment data
        Dim oAMS = NGLAMSAppointmentData.GetAMSAppointmentFiltered(ApptControl)
        If Not oAMS Is Nothing AndAlso oAMS.AMSApptControl <> 0 Then
            Dim targetDate As Date?
            Select Case ValidationType
                Case AMSDateTimeValidationType.AMSApptStartDate
                    targetDate = oAMS.AMSApptStartDate
                Case AMSDateTimeValidationType.AMSApptActualDateTime
                    targetDate = oAMS.AMSApptActualDateTime
                Case AMSDateTimeValidationType.AMSApptStartLoadingDateTime
                    targetDate = oAMS.AMSApptStartLoadingDateTime
                Case AMSDateTimeValidationType.AMSApptFinishLoadingDateTime
                    targetDate = oAMS.AMSApptFinishLoadingDateTime
                Case AMSDateTimeValidationType.AMSApptActLoadCompleteDateTime
                    targetDate = oAMS.AMSApptActLoadCompleteDateTime
            End Select
            If targetDate.HasValue Then strApptDateTime = targetDate.Value.ToString("g") 'General date/time pattern (short time).  example: 6/15/2009 1:45 PM 
            'when source and target dates are nothing then return match 
            'when 
            If sourceDate Is Nothing Then
                If targetDate Is Nothing Then
                    enmRet = AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                Else
                    enmRet = AMSDateTimeValidationResults.ApptExitsDataAndTimeDoNotMatch
                End If
            Else
                If targetDate Is Nothing Then
                    enmRet = AMSDateTimeValidationResults.ApptExitsButDateAndTimeAreEmpty
                Else
                    Select Case Date.Compare(sourceDate.Value, targetDate.Value)
                        Case Is < 0 'earlier
                            enmRet = AMSDateTimeValidationResults.ApptExistDateIsBeforeAppt
                        Case Is = 0 'same
                            enmRet = AMSDateTimeValidationResults.ApptExistAndMatches
                        Case Is > 0 'greater
                            enmRet = AMSDateTimeValidationResults.ApptExitsDateIsAfterAppt
                    End Select
                End If
            End If
        End If


        Return enmRet

    End Function

    Public Function ResetToNStatusSP(ByVal Filters As String, ByVal CompControlFilter As Integer) As DTO.vOptimizationStop()

        Dim retVals = NGLBookData.ResetToNStatusSP(Parameters.UserName, Filters, CompControlFilter)

        Return retVals

    End Function

    Public Function ResetToNStatusSPOptHasSHID(ByVal Filters As String, ByVal CompControlFilter As Integer) As DTO.vOptimizationStop()

        Dim retVals = NGLBookData.ResetToNStatusSPOptHasSHID(Parameters.UserName, Filters, CompControlFilter)

        Return retVals

    End Function


#End Region

#Region " Trans Code Changes"

    Friend Function createAutoTenderCarrierFailedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal CarrierNumber As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal Errors As String,
                                       ByVal Warnings As String,
                                       ByVal Messages As String) As Boolean

        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS Number: {3} ", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert - Cannot Auto Tender order to carrier number, ", CarrierNumber.ToString(), ", for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertAutoTenderFailed", "Alert system failed to auto tender order", Subject, Body, CompControl, 0, CarrierControl, CarrierNumber, Note1, Note2, Note3, Note4, "")
    End Function

    ''' <summary>
    ''' An over load to createAutoAssignDefaultCarrierFailedSubscriptionAlert which does not include the message cascading dispatching.  
    ''' Use this method when cascading dispatching cannot be performed after the default carrier assigment fails. 
    ''' </summary>
    ''' <param name="Subject"></param>
    ''' <param name="CompControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="BookProNumber"></param>
    ''' <param name="BookSHID"></param>
    ''' <param name="OrderNumber"></param>
    ''' <param name="OrderSequence"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="Errors"></param>
    ''' <param name="Warnings"></param>
    ''' <param name="Messages"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function createAutoAssignCarrierFailedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal CarrierNumber As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal Errors As String,
                                       ByVal Warnings As String,
                                       ByVal Messages As String) As Boolean

        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS Number: {3}", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Note5 As String = " The system was not able to use cascading dispatching and attempt to select the next lowest cost carrier."
        Dim Body As String = String.Concat("Alert - Cannot Auto Assign carrier number, ", CarrierNumber.ToString(), ", for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertAutoAssignDefaultCarrierFailed", "Alert system failed to assign default carrier", Subject, Body, CompControl, 0, CarrierControl, CarrierNumber, Note1, Note2, Note3, Note4, Note5)

    End Function

    Friend Function createAutoAssignLowestCostCarrierFailedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompControl As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal Errors As String,
                                       ByVal Warnings As String,
                                       ByVal Messages As String) As Boolean

        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS Number: {3}", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert - Cannot Auto Assign lowest cost carrier for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertAutoAssignLowestCostCarrierFailed", "Alert system failed to assign lowest cost carrier", Subject, Body, CompControl, 0, 0, 0, Note1, Note2, Note3, Note4, "")

    End Function

    Friend Function createAutoAssignDefaultCarrierFailedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal CarrierNumber As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal Errors As String,
                                       ByVal Warnings As String,
                                       ByVal Messages As String) As Boolean

        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS Number: {3}", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Note5 As String = " If Cascading Dispatching is turned on the system will attempt to select the next lowest cost carrier."
        Dim Body As String = String.Concat("Alert - Cannot Auto Assign the default carrier number, ", CarrierNumber.ToString(), ", for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4, vbCrLf, vbCrLf, Note5)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertAutoAssignDefaultCarrierFailed", "Alert system failed to assign default carrier", Subject, Body, CompControl, 0, CarrierControl, CarrierNumber, Note1, Note2, Note3, Note4, Note5)

    End Function

    Friend Function createAutoFinalizeCarrierFailedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal CarrierNumber As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal Errors As String,
                                       ByVal Warnings As String,
                                       ByVal Messages As String) As Boolean

        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS Number: {3}", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert - Cannot Auto Finalize order to carrier number, ", CarrierNumber.ToString(), ", for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertAutoFinalizeFailed", "Alert system failed to auto finalize order", Subject, Body, CompControl, 0, CarrierControl, CarrierNumber, Note1, Note2, Note3, Note4, "")

    End Function

    Friend Function createAutoTenderConfigurationNotAllowedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal CarrierNumber As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal Errors As String,
                                       ByVal Warnings As String,
                                       ByVal Messages As String) As Boolean

        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS Number: {3}", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert - The current configuration does not allow auto tender with carrier number, ", CarrierNumber.ToString(), ", for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        'Alert system failed to assign lowest cost carrier
        'Alert configuration does not allow auto tender
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertAutoTenderConfigurationNotAllowed", "Alert configuration does not allow auto tender", Subject, Body, CompControl, 0, CarrierControl, CarrierNumber, Note1, Note2, Note3, Note4, "")

    End Function


    Friend Function createAutoUpdateChangesToFinalizedFailedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompNumber As Integer,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal Messages As String) As Boolean

        Dim Note1 As String = String.Format(" Order Number - Sequence: {0}-{1} ", OrderNumber, OrderSequence)

        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert - Auto update failed on the finialized order shipping from company # ", CompNumber.ToString(), ", for: ", vbCrLf, Note1, vbCrLf, vbCrLf, "because: ", vbCrLf, vbCrLf, Note4)
        Return NGLtblAlertMessageData.InsertAlertMessage("AutoUpdateChangesToFinalizedFailed", "Alert when auto update fails on changes to finalized orders", Subject, Body, 0, CompNumber, 0, 0, Note1, "", "", Note4, "")

    End Function


    Friend Function createCascadeDispatchFailedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompControl As Integer,
                                       ByVal BookProNumber As String,
                                       ByVal BookSHID As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal BookConsPrefix As String,
                                       ByVal Errors As String,
                                       ByVal Warnings As String,
                                       ByVal Messages As String) As Boolean

        Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2} CNS Number: {3}", BookProNumber, OrderNumber, OrderSequence, BookConsPrefix)
        Dim Note2 As String = " Errors: " & Errors
        Dim Note3 As String = " Warnings: " & Warnings
        Dim Note4 As String = " Messages: " & Messages
        Dim Body As String = String.Concat("Alert - the system failed to complete the cascade dispatch procedure for: ", vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertCascadeDispatchFailed", "Alert system failed to complete cascade dispatch", Subject, Body, CompControl, 0, 0, 0, Note1, Note2, Note3, Note4, "")

    End Function

    ''' <summary>
    ''' Removes a Single order from Load.
    ''' Business Rule, this feature is only allowed in N or P status.
    ''' It assigned a new CNS and resets the order to N status.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes
    '''  If there is a single BookRev record then reset the stem miles to the lane miles
    ''' </remarks>
    Public Function RemoveOrderFromLoad(ByVal BookControl As Integer) As DTO.BookRevenue
        Dim oBookRev = NGLBookRevenueData.GetBookRevenueWDetailsFiltered(BookControl)
        Dim revResult As DTO.BookRevenue
        If oBookRev IsNot Nothing AndAlso Not oBookRev.BookControl = 0 Then
            Dim cns As String = NGLBatchProcessData.GetNextConsNumber(oBookRev.BookCustCompControl)
            oBookRev.ResetToNStatus()
            oBookRev.BookConsPrefix = cns
            oBookRev.BookRouteConsFlag = True
            oBookRev.BookPickupStopNumber = 1
            oBookRev.BookStopNo = 1

            'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes
            'If there is a single BookRev record then reset the stem miles to the lane miles
            If oBookRev.BookRevLaneBenchMiles Is Nothing OrElse oBookRev.BookRevLaneBenchMiles = 0 Then
                Dim laneBenchMiles = NGLBookRevenueData.GetLaneBenchMilesByBookODControl(oBookRev.BookODControl)
                oBookRev.BookMilesFrom = laneBenchMiles
            Else
                oBookRev.BookMilesFrom = oBookRev.BookRevLaneBenchMiles
            End If

            revResult = NGLBookRevenueData.SaveRevenueWDetails(oBookRev)
        Else
            Return Nothing
        End If
        Return revResult
    End Function


    ''' <summary>
    ''' Will return true or false for success.  If success is false check for warnings and messages.  
    ''' If this is a manual process ask the user to confirm and retry based on the intValidationFlags.
    ''' When some intValidationFlags are false the associated validation rules are ignored so the procedure will succeed.  
    ''' Some work needs to be completed to determine which messages and warnings can be ignored if the user so desires.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="NewTranCode"></param>
    ''' <param name="OldTranCode"></param>
    ''' <param name="intValidationFlags"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR for v-8.2 on 6/24/2019
    '''   added optional parameter OverrideSendLoadTenderEmail
    ''' </remarks>
    Public Function ProcessNewTransCode(ByVal BookControl As Integer,
                                        ByVal NewTranCode As String,
                                        ByVal OldTranCode As String,
                                        Optional ByVal intValidationFlags As Int64 = 0,
                                        Optional ByVal BidStatusCode As Integer = BSCEnum.OpsDeletePost,
                                        Optional ByVal OverrideSendLoadTenderEmail As Integer = 0) As DTO.WCFResults
        Dim results As New DTO.WCFResults With {.Key = BookControl, .Success = True}
        results.setAction(DTO.WCFResults.ActionEnum.DoNothing)
        results.AddLog("Get the Booking records.")
        'When changing the trans code we always include the LTL Pool records
        Dim oBookRevs = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(BookControl, True)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.Success = False
            results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
            Return results
        End If
        Return ProcessNewTransCode(oBookRevs, BookControl, NewTranCode, OldTranCode, results, intValidationFlags, BidStatusCode, OverrideSendLoadTenderEmail)
    End Function


    ''' <summary>
    ''' THIS IS THE OVERLOAD I AM WRITING FOR THE LOAD BOARDS
    ''' Will return true or false for success.  If success is false check for warnings and messages.  
    ''' If this is a manual process ask the user to confirm and retry based on the intValidationFlags.
    ''' When some intValidationFlags are false the associated validation rules are ignored so the procedure will succeed.  
    ''' Some work needs to be completed to determine which messages and warnings can be ignored if the user so desires.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="NewTranCode"></param>
    ''' <param name="OldTranCode"></param>
    ''' <param name="intValidationFlags"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' This should use BookRevenueWithDetails
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    '''  Modified by RHR for v-8.2 on 6/24/2019
    '''   added optional parameter OverrideSendLoadTenderEmail
    ''' </remarks>
    Public Function ProcessNewTransCode(ByRef oBookRevs As DTO.BookRevenue(),
                                        ByVal BookControl As Integer,
                                        ByVal NewTranCode As String,
                                        ByVal OldTranCode As String,
                                        ByRef results As DTO.WCFResults,
                                        Optional ByVal intValidationFlags As Int64 = 0,
                                        Optional ByVal BidStatusCode As Integer = BSCEnum.OpsDeletePost,
                                        Optional ByVal OverrideSendLoadTenderEmail As Integer = 0) As DTO.WCFResults
        Dim LoadTenderTypeMode As New BitwiseFlags32
        Dim LoadStatusCodeActions As New BitwiseFlags32
        'Set the default for this to Manual because in the old ProcessNewTransCode Method Manual was passed as the default AcceptRejectModeEnum to AcceptOrRejectLoad
        LoadTenderTypeMode.turnBitFlagOn(LTTypeEnum.Manual)
        Return ProcessNewTransCode(oBookRevs, BookControl, NewTranCode, OldTranCode, results, LoadTenderTypeMode, LoadStatusCodeActions, intValidationFlags, BidStatusCode, OverrideSendLoadTenderEmail)

    End Function

    Public Function ProcessNewTransCode(ByVal BookControl As Integer,
                                        ByVal NewTranCode As String,
                                        ByVal OldTranCode As String,
                                        ByVal enumLoadTenderType As LTTypeEnum,
                                        ByVal OverrideSendLoadTenderEmail As Integer) As DTO.WCFResults
        Dim results As New DTO.WCFResults With {.Key = BookControl, .Success = True}
        results.setAction(DTO.WCFResults.ActionEnum.DoNothing)
        results.AddLog("Get the Booking records.")
        'When changing the trans code we always include the LTL Pool records
        Dim oBookRevs = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(BookControl, True)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.Success = False
            results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
            Return results
        End If
        Dim LoadTenderTypeMode As New BitwiseFlags32
        Dim LoadStatusCodeActions As New BitwiseFlags32
        Dim intValidationFlags As Int64 = 0
        Dim BidStatusCode As Integer = BSCEnum.OpsDeletePost
        LoadTenderTypeMode.turnBitFlagOn(enumLoadTenderType)
        Return ProcessNewTransCode(oBookRevs, BookControl, NewTranCode, OldTranCode, results, LoadTenderTypeMode, LoadStatusCodeActions, intValidationFlags, BidStatusCode, OverrideSendLoadTenderEmail)

    End Function

    ''' <summary>
    ''' THIS IS THE OVERLOAD I AM WRITING FOR THE LOAD BOARDS
    ''' </summary>
    ''' <param name="oBookRevs"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="NewTranCode"></param>
    ''' <param name="OldTranCode"></param>
    ''' <param name="results"></param>
    ''' <param name="intValidationFlags"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' Modified by RHR for v-8.2 on 6/24/2019
    '''   added optional parameter OverrideSendLoadTenderEmail
    '''   this parameter only impacts loads dispatched from new D365 pages
    '''   this parameter only impacts loads with an OldTranCode of N,P,PC, or PB and a  NewTranCode of  PC or PB
    '''   0 is the default do not override use standard processing logic for blnSendEmail
    '''   -1 override And never send load tender email when NewTranCode Is PC Or PB (set blnSendEmail = false)
    '''   1 override And always send load tender email when NewTranCode Is PC Or PB (set blnSendEmail = true)
    '''   Modified by RHR for v-8.4.0.003 on 08/25/2021 we now do send emails for NEXTstop tender in stored procedure
    ''' </remarks>
    Public Function ProcessNewTransCode(ByRef oBookRevs As DTO.BookRevenue(),
                                        ByVal BookControl As Integer,
                                        ByVal NewTranCode As String,
                                        ByVal OldTranCode As String,
                                        ByRef results As DTO.WCFResults,
                                        ByVal LoadTenderTypeMode As BitwiseFlags32,
                                        ByVal LoadStatusCodeActions As BitwiseFlags32,
                                        Optional ByVal intValidationFlags As Int64 = 0,
                                        Optional ByVal BidStatusCode As Integer = BSCEnum.OpsDeletePost,
                                        Optional ByVal OverrideSendLoadTenderEmail As Integer = 0) As DTO.WCFResults
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}
        results.setAction(DTO.WCFResults.ActionEnum.DoNothing)
        Dim UserName As String = Me.Parameters.UserName
        Dim blnShowValidationMessage As Boolean = False
        Dim blnSendEmail As Boolean = True
        Try
            'TranCodes
            'N = New Load
            'P = Carrier Assigned
            'PC = Tendered to Carrier
            'PB = Accepted by Carrier/Finalized
            'I = Ready to Invoice
            'IC = Invoice Complete
            results.AddLog("Test the Booking records.")
            Dim sSHID As String = ""
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                results.Success = False
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
                Return results
            Else
                sSHID = oBookRevs(0).BookSHID
                If String.IsNullOrWhiteSpace(sSHID) Then sSHID = oBookRevs(0).BookConsPrefix
            End If

            'Modified by LVV on 1/13/17 for v-8.0 Next Stop
            'TODO -- Optimize code : I know there has to be a more efficient way to do this but I am in a hurry and this is it for now
            Dim source = "NGL.FM.BLL.NGLBookBLL.ProcessNewTransCode"
            Dim blnHasDATAccount As Boolean
            Dim blnHasNSAccount As Boolean
            Dim userSecurityControl As Integer = 0
            Dim blnNoUsec As Boolean = False

            Dim uSec = NGLSecurityData.GettblUserSecurityByUserName(Parameters.UserName)
            If Not uSec Is Nothing Then
                If uSec.UserSecurityControl <> 0 Then userSecurityControl = uSec.UserSecurityControl Else blnNoUsec = True
            Else
                blnNoUsec = True
            End If
            If blnNoUsec Then
                'If we can't get the account credentials then we can't post so return false
                Dim msg = "Could not get the User Security Control for User " + Parameters.UserName + ". Load Board Post Failed. " + source
                NGLSystemLogData.AddApplicaitonLog(msg, source)
                Dim p As String() = {msg}
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_DATGeneralRetMsg, p)
                results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                results.Success = False
                Return results
            End If

            Dim msgDAT = "No DAT Account found for user " + Parameters.UserName + " using UserSecurityControl" + userSecurityControl.ToString() + "."
            Dim msgNS = "No NEXTStop Account found for user " + Parameters.UserName + " using UserSecurityControl" + userSecurityControl.ToString() + "."


            'If we want to attempt to Post to DAT or NEXTStop we need to have an account
            If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then blnHasDATAccount = NGLSSOAData.DoesUserHaveSSOAAccount(userSecurityControl, DAL.Utilities.SSOAAccount.DAT)
            If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then blnHasNSAccount = NGLSSOAData.DoesUserHaveSSOAAccount(userSecurityControl, DAL.Utilities.SSOAAccount.NextStop)

            'DAT Only
            If Not LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) AndAlso LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then
                blnSendEmail = False
                'HERE IS WHERE WE CHECK IF THE USER HAS A DAT ACCOUNT -- IF NOT SHOW A WARNING MESSAGE AND RETURN FALSE
                If blnHasDATAccount = False Then
                    NGLSystemLogData.AddApplicaitonLog(msgDAT, source)
                    Dim p As String() = {Parameters.UserName, "DAT"}
                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_UserNoSSOAAccount, p)
                    results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                    results.Success = False
                    Return results
                End If
            End If
            'NEXTStop Only
            If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) AndAlso Not LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then
                'Modified by RHR for v-8.4.0.003 on 08/25/2021 we do send emails to Carriers but with a diferent message and no pdf.
                'We need to modify the send load tender sheet stored procedure to a) check if the carrier is the next stop carrier
                'and (b) to check which carriers have a nextstop account then send a message with a link to all carriers in the list.
                'blnSendEmail = False
                'HERE IS WHERE WE CHECK IF THE USER HAS A NEXTStop ACCOUNT -- IF NOT SHOW A WARNING MESSAGE AND RETURN FALSE
                If blnHasNSAccount = False Then
                    NGLSystemLogData.AddApplicaitonLog(msgNS, source)
                    Dim p As String() = {Parameters.UserName, "NextStop"}
                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_UserNoSSOAAccount, p)
                    results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                    results.Success = False
                    Return results
                End If
            End If
            'Both
            If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) AndAlso LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then
                'Modified by RHR for v-8.4.0.003 on 08/25/2021 we do send emails to Carriers but with a diferent message and no pdf.
                'We need to modify the send load tender sheet stored procedure to a) check if the carrier is the next stop carrier
                'and (b) to check which carriers have a nextstop account then send a message with a link to all carriers in the list.
                'blnSendEmail = False
                If blnHasDATAccount = False Then
                    'Change the selected flag to false for DAT since we can't post because we don't have a valid login
                    LoadTenderTypeMode.turnBitFlagOff(LTTypeEnum.DAT)

                    NGLSystemLogData.AddApplicaitonLog(msgDAT, source)
                    Dim p As String() = {Parameters.UserName, "DAT"}
                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_UserNoSSOAAccount, p)
                    results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                    results.Success = False
                    If blnHasNSAccount = False Then
                        NGLSystemLogData.AddApplicaitonLog(msgNS, source)
                        Dim p2 As String() = {Parameters.UserName, "NextStop"}
                        results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_UserNoSSOAAccount, p2)
                        Return results
                    End If
                Else
                    If blnHasNSAccount = False Then
                        'Change the selected flag to false for NextStop since we can't post because we don't have a valid login
                        LoadTenderTypeMode.turnBitFlagOff(LTTypeEnum.NextStop)
                        NGLSystemLogData.AddApplicaitonLog(msgNS, source)
                        Dim p As String() = {Parameters.UserName, "NextStop"}
                        results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_UserNoSSOAAccount, p)
                        results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                        results.Success = False
                    End If
                End If
            End If


            results.Key = BookControl
            'run UI Validation 
            If OldTranCode <> "N" And NewTranCode = "N" Then
                If results.isValidationOn(DTO.WCFResults.ValidationBits.CarrierANDBFCCostsWillBeUnlocked, intValidationFlags) Then
                    results.AddLog("Validate if carrier and BFC Costs will be unlocked")
                    If oBookRevs.Any(Function(x) x.BookLockAllCosts = True And (x.BookLockBFCCost = True And x.CompFinUseImportFrtCost = False)) Then
                        results.setValidationFailed(DTO.WCFResults.ValidationBits.CarrierANDBFCCostsWillBeUnlocked)
                        blnShowValidationMessage = True
                    End If
                Else
                    If results.isValidationOn(DTO.WCFResults.ValidationBits.CarrierOrBFCCostsWillBeUnlocked, intValidationFlags) Then
                        results.AddLog("Validate if carrier or BFC Costs will be unlocked")
                        If oBookRevs.Any(Function(x) x.BookLockAllCosts = True) Then
                            results.setValidationFailed(DTO.WCFResults.ValidationBits.CarrierOrBFCCostsWillBeUnlocked)
                            blnShowValidationMessage = True
                        End If
                        If oBookRevs.Any(Function(x) x.BookLockBFCCost = True And x.CompFinUseImportFrtCost = False) Then
                            results.ValidationBitFailed = DTO.WCFResults.ValidationBits.CarrierOrBFCCostsWillBeUnlocked
                            results.AddMessage(DTO.WCFResults.MessageType.Messages, DTO.WCFResults.MessageEnum.M_BFCCostsWillBeUnlocked)
                            blnShowValidationMessage = True
                        End If
                    End If
                End If
                If blnShowValidationMessage Then
                    results.AddLog("One or more Costs will be unlocked so show validation message to continue.")
                    results.Success = False
                    results.setAction(DTO.WCFResults.ActionEnum.ShowValidationMsg)
                    Return results
                End If
            End If
            If OldTranCode <> "N" And OldTranCode <> "P" And NewTranCode = "P" Then
                results.AddLog("Add costs are locked warnings.")
                If oBookRevs.Any(Function(x) x.BookLockAllCosts = True And x.BookLockBFCCost = True) Then
                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_CarrierAndBFCCostWereNotUnlocked)
                    results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                Else
                    If oBookRevs.Any(Function(x) x.BookLockAllCosts = True) Then
                        results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_CarrierCostWereNotUnlocked)
                        results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                    End If
                    If oBookRevs.Any(Function(x) x.BookLockBFCCost = True And x.CompFinUseImportFrtCost = False) Then
                        results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_BFCCostsWereNotUnlocked)
                        results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                    End If
                End If
            End If
            If OldTranCode = "IC" And (NewTranCode = "N" Or NewTranCode = "PB" Or NewTranCode = "PC" Or NewTranCode = "P") Then
                If results.isValidationOn(DTO.WCFResults.ValidationBits.ReverseTheInvoiceCompleteStatus, intValidationFlags) Then
                    results.AddLog("IC reversal validation is required.")
                    results.setValidationFailed(DTO.WCFResults.ValidationBits.ReverseTheInvoiceCompleteStatus)
                    results.Success = False
                    results.setAction(DTO.WCFResults.ActionEnum.ShowValidationMsg)
                    Return results
                End If
            End If
            If ((OldTranCode = "P" Or OldTranCode = "N") And (NewTranCode <> "N" And NewTranCode <> "P")) Then
                results.AddLog("Validate Company Credit if we are moving From N or P to any value that is not N or P")
                'get a list of companies for this load
                Dim oComps As List(Of Integer) = oBookRevs.Select(Function(x) x.BookCustCompControl).Distinct().ToList()
                If oComps Is Nothing OrElse oComps.Count < 1 Then
                    results.Success = False
                    results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_BookingRecordMissingCompany)
                    Return results
                End If
                For Each c In oComps
                    Dim BFC = oBookRevs.Where(Function(x) x.BookCustCompControl = c).Sum(Function(x) x.BookRevBilledBFC)
                    Dim oCreditData = NGLCompData.GetCompValidateCredit(c, BFC)
                    If Not oCreditData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(oCreditData.Message) Then
                        'Note: oCreditData returns localization keys not text messages 
                        results.Success = False
                        results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                        results.AddMessage(DTO.WCFResults.MessageType.Messages, oCreditData.Message)
                        results.AddMessage(DTO.WCFResults.MessageType.Warnings, oCreditData.Details)
                    End If
                Next
                If Not results.Success Then
                    BookRevenueBLL.updateCreditHold(oBookRevs, True) 'on credit hold = true.
                    Return results
                Else
                    BookRevenueBLL.updateCreditHold(oBookRevs, False)
                End If
            End If


            'Check if we are tendering the load.  if so we need to update the Carrier Pro and SHID
            Dim blnDataDirty As Boolean = False
            Dim blnTenderLoad As Boolean = False
            'Turn of email notificaiton for Auto Accept movements when moving from P to PB directly.
            'Modified by RHR for v-8.2 on 6/24/2019
            '  add override to default rule using OverrideSendLoadTenderEmail
            If NewTranCode = "PB" And OldTranCode = "P" Then
                If OverrideSendLoadTenderEmail = 1 Then
                    blnSendEmail = True
                Else
                    blnSendEmail = False
                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_ManualAutoAcceptNoTenderEmail)
                    results.Action = DTO.WCFResults.ActionEnum.ShowWarnings
                End If
            End If
            'Modified by RHR for v-8.2 on 6/24/2019
            '  add override to default rule using OverrideSendLoadTenderEmail
            If (NewTranCode = "PC" Or NewTranCode = "PB") And (OldTranCode = "P" Or OldTranCode = "N") Then
                If OverrideSendLoadTenderEmail = -1 Then
                    blnSendEmail = False '  user has selected to not send email

                    results.AddLog("The user has selected to not send an email to the carrier for " & sSHID)
                End If
                results.AddLog("Tender the load and update the SHID")
                'Tender the load to the carrier
                'TODO: add logic for unattended execution Auto Tender
                If Not BookRevenueBLL.updateBookSHID(oBookRevs.ToList(), blnDataDirty, False) Then
                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_CannotTenderLoadUpdateSHIDFailed)
                    results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                    results.Success = False
                    Return results
                End If
                blnTenderLoad = True
            End If
            'now that we have updated the book shids we can get the selected booking record needed below for accept/Reject logic
            Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            results.AlphaCode = oSelectedBooking.BookProNumber
            results.updateKeyFields("BookConsPrefix", oSelectedBooking.BookConsPrefix)
            results.updateKeyFields("BookSHID", oSelectedBooking.BookSHID)
            Dim strBookTrackComment As String = " Trans Code changed From " & OldTranCode & " to " & NewTranCode

            If NewTranCode = "N" And OldTranCode = "PC" Then
                'manually reject load
                If results.isValidationOn(DTO.WCFResults.ValidationBits.RejectTheLoad, intValidationFlags) Then
                    results.AddLog("Reject Load validation is required.")
                    results.setValidationFailed(DTO.WCFResults.ValidationBits.RejectTheLoad)
                    results.Success = False
                    results.setAction(DTO.WCFResults.ActionEnum.ShowValidationMsg)
                    Return results
                End If
                'Modified by LVV on 1/13/17 for v-8.0 Next Stop  
                results.AddLog("Reject the load.")
                AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Rejected, True, strBookTrackComment & " Load Rejected", "0", "", "", LoadTenderTypeMode, LoadStatusCodeActions, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)
            ElseIf NewTranCode = "N" And OldTranCode = "P" Then
                ' the carrier will be manually unassigned from the load we do not send email 
                blnSendEmail = False
                results.AddLog("Carrier unassigned from the load.")
                'Modified by LVV on 1/13/17 for v-8.0 Next Stop
                AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Unassigned, blnSendEmail, strBookTrackComment & " Load Unassigned", "0", "", "", LoadTenderTypeMode, LoadStatusCodeActions, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)
            ElseIf NewTranCode = "N" And OldTranCode <> "N" Then
                If results.isValidationOn(DTO.WCFResults.ValidationBits.DropTheLoad, intValidationFlags) Then
                    results.AddLog("Drop the Load validation is required.")
                    results.setValidationFailed(DTO.WCFResults.ValidationBits.DropTheLoad)
                    results.Success = False
                    results.setAction(DTO.WCFResults.ActionEnum.ShowValidationMsg)
                    Return results
                End If
                results.AddLog("Drop the load.")
                'Modified by LVV on 1/13/17 for v-8.0 Next Stop
                AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Dropped, True, strBookTrackComment & " Load Dropped", "0", "", "", LoadTenderTypeMode, LoadStatusCodeActions, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)
            ElseIf NewTranCode = "P" And OldTranCode = "PB" Then
                If results.isValidationOn(DTO.WCFResults.ValidationBits.UndoFinalize, intValidationFlags) Then
                    results.AddLog("Undo Finalize validation is required.")
                    results.setValidationFailed(DTO.WCFResults.ValidationBits.UndoFinalize)
                    results.Success = False
                    results.setAction(DTO.WCFResults.ActionEnum.ShowValidationMsg)
                    Return results
                End If
                results.AddLog("Unfinalize the load.")
                'Modified by LVV on 1/13/17 for v-8.0 Next Stop
                AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Unfinalize, True, strBookTrackComment & " Load Unfinalized", "0", "", "", LoadTenderTypeMode, LoadStatusCodeActions, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)
            ElseIf NewTranCode = "P" And OldTranCode = "PC" Then
                results.AddLog("Modify the load after it was tendered but before it was accepted.")
                'Modified by LVV on 1/13/17 for v-8.0 Next Stop
                AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.ModifyUnaccepted, True, strBookTrackComment & " Load Unfinalized", "0", "", "", LoadTenderTypeMode, LoadStatusCodeActions, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)
            ElseIf NewTranCode = "PC" And OldTranCode = "PB" Then
                'return Access Denied Message
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_AccessDeniedMoveToPCFromPB)
                results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                Return results
            ElseIf NewTranCode = "PB" And (OldTranCode = "PC" Or OldTranCode = "P" Or OldTranCode = "N") Then
                If results.isValidationOn(DTO.WCFResults.ValidationBits.DoFinalize, intValidationFlags) Then
                    results.AddLog("Finalizaton validation is required.")
                    results.setValidationFailed(DTO.WCFResults.ValidationBits.DoFinalize)
                    results.Success = False
                    results.setAction(DTO.WCFResults.ActionEnum.ShowValidationMsg)
                    Return results
                End If
                results.AddLog("Finalize the load.")
                'Modified by LVV on 1/13/17 for v-8.0 Next Stop
                AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Accepted, blnSendEmail, strBookTrackComment & " Load Finalize", "0", "", "", LoadTenderTypeMode, LoadStatusCodeActions, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)
            ElseIf blnTenderLoad Then
                results.AddLog("Tender the load.")
                For Each b In oBookRevs
                    b.BookTranCode = NewTranCode
                    b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                Next
                'Modified by LVV on 1/13/17 for v-8.0 Next Stop
                AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Tendered, blnSendEmail, strBookTrackComment & " Load Tendered", "0", "", "", LoadTenderTypeMode, LoadStatusCodeActions, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)
            Else
                'Just update the trancode on all cons records
                For Each b In oBookRevs
                    b.BookTranCode = NewTranCode
                    b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                Next
                results.AddLog("Saving changes.")
                NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, False)
                results.AddLog("Success!")
                results.Success = True
                'Modified By LVV on 10/24/16 for v-7.0.5.110 Spot Save Action
                If NewTranCode = "IC" And OldTranCode = "P" Then NGLBookData.CloseOutSpecificLoads(BookControl) 'call the sp
            End If
        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            'sqlEx.Reason.ToString(, sqlEx.Detail.Message, sqlEx.Detail.Details)
            results.AddFaultException(DTO.WCFResults.MessageEnum.E_SQLFaultCannotProcessNewTranCode, sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.Detail.DetailsList)
            results.Success = False
            results.setAction(DTO.WCFResults.ActionEnum.ShowErrors)
        Catch ex As Exception
            results.AddUnexpectedError(ex)
            results.Success = False
            results.setAction(DTO.WCFResults.ActionEnum.ShowErrors)
        End Try
        Return results
    End Function

    ''' <summary>
    ''' overload used to lookup the bookrevenue and selected booking data using the book control 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="AcceptRejectCode"></param>
    ''' <param name="SendEmail"></param>
    ''' <param name="BookTrackComment"></param>
    ''' <param name="BookTrackStatus"></param>
    ''' <param name="NotificationEMailAddress"></param>
    ''' <param name="NotificationEMailAddressCc"></param>
    ''' <param name="AcceptRejectMode"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AcceptOrRejectLoad(ByVal BookControl As Integer,
                                       ByVal CarrierControl As Integer,
                                       ByVal CarrierContControl As Integer,
                                       ByVal AcceptRejectCode As AcceptRejectEnum,
                                       ByVal SendEmail As Boolean,
                                       ByVal BookTrackComment As String,
                                       ByVal BookTrackStatus As Integer,
                                       ByVal NotificationEMailAddress As String,
                                       ByVal NotificationEMailAddressCc As String,
                                       ByVal AcceptRejectMode As AcceptRejectModeEnum,
                                       ByVal UserName As String,
                                       Optional ByRef results As DTO.WCFResults = Nothing,
                                       Optional ByVal intValidationFlags As Int64 = 0,
                                       Optional ByVal BidStatusCode As Integer = BSCEnum.OpsDeletePost) As DTO.WCFResults
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}
        UserName = If(String.IsNullOrEmpty(UserName) OrElse UserName.Trim.Length < 1, Me.Parameters.UserName, UserName)
        Try
            'Get the Booking Records For this Load
            Dim oBookRevs = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(BookControl)
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                'Check for deleted booking records
                If AcceptRejectMode = AcceptRejectModeEnum.EDI Then
                    results.AddLog("Check the BookDeleted transaction table for EDI 204 updates that have been accepted or rejected.")
                    If NGLBookData.UpdateBookDeletedEDIExported(BookControl, CarrierControl) Then
                        results.AddMessage(DTO.WCFResults.MessageType.Messages, DTO.WCFResults.MessageEnum.M_BookDeletedEDIMarkedAsExportedForCanceled)
                        results.setAction(DTO.WCFResults.ActionEnum.DoNothing)
                        results.Success = True
                        Return results
                    End If
                End If
                results.Success = False
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
                Return results
            End If
            Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            Return AcceptOrRejectLoad(oBookRevs, oSelectedBooking, CarrierControl, CarrierContControl, AcceptRejectCode, SendEmail, BookTrackComment, BookTrackStatus, NotificationEMailAddress, NotificationEMailAddressCc, AcceptRejectMode, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AcceptOrRejectLoad"), DAL.sysErrorParameters.sysErrorState.SystemLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return results
    End Function

    Public Function AcceptOrRejectLoadEmailToken(ByVal token As String) As DTO.WCFResults

        Dim BookControl As Integer
        Dim CarrierControl As Integer
        Dim CarrierContControl As Integer
        Dim AcceptRejectCode As AcceptRejectEnum = AcceptRejectEnum.Accepted
        Dim SendEmail As Boolean = True
        Dim BookTrackComment As String = "Accepted via Email Token"
        Dim BookTrackStatus As Integer
        Dim NotificationEMailAddress As String
        Dim NotificationEMailAddressCc As String
        Dim AcceptRejectMode As AcceptRejectModeEnum = AcceptRejectModeEnum.Token
        Dim UserName As String = Me.Parameters.UserName
        Dim results As New DTO.WCFResults With {.Key = BookControl, .Success = True}
        Dim intValidationFlags As Int64 = 0
        'read data from db using Token
        'manage Token Validation Results
        'Return Accept or Reject results
        Return AcceptOrRejectLoad(BookControl, CarrierControl, CarrierContControl, AcceptRejectCode, SendEmail, BookTrackComment, BookTrackStatus, NotificationEMailAddress, NotificationEMailAddressCc, AcceptRejectMode, UserName, results, intValidationFlags)
        Try

            'Get the Booking Records For this Load
            Dim oBookRevs = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(BookControl)
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                'Check for deleted booking records
                If AcceptRejectMode = AcceptRejectModeEnum.EDI Then
                    results.AddLog("Check the BookDeleted transaction table for EDI 204 updates that have been accepted or rejected.")
                    If NGLBookData.UpdateBookDeletedEDIExported(BookControl, CarrierControl) Then
                        results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_BookDeletedEDIMarkedAsExportedForCanceled)
                        results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.DoNothing)
                        results.Success = True
                        Return results
                    End If
                End If
                results.Success = False
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_NoOrdersFound)
                Return results
            End If
            Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            Return AcceptOrRejectLoad(oBookRevs, oSelectedBooking, CarrierControl, CarrierContControl, AcceptRejectCode, SendEmail, BookTrackComment, BookTrackStatus, NotificationEMailAddress, NotificationEMailAddressCc, AcceptRejectMode, UserName, results, intValidationFlags)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AcceptOrRejectLoad"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try

        Return results
    End Function


    ''' <summary>
    ''' If there is a single BookRev record then reset the stem miles to the lane miles.
    ''' Called from AcceptOrRejectLoad() only - written to make that code easier to maintain
    ''' We were calling the exact same code multiple times in AcceptOrRejectLoad, 
    ''' so now if we make a change we don't have to remember to fix it in multiple places
    ''' </summary>
    ''' <param name="oBookRevs"></param>
    ''' <remarks>
    ''' Created By LVV on 6/13/19
    ''' </remarks>
    Private Sub ResetStemMilesToLaneMiles(ByRef oBookRevs As DTO.BookRevenue())
        'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes
        'If there is a single BookRev record then reset the stem miles to the lane miles
        If oBookRevs.Count = 1 Then
            If oBookRevs(0).BookRevLaneBenchMiles Is Nothing OrElse oBookRevs(0).BookRevLaneBenchMiles = 0 Then
                Dim laneBenchMiles = NGLBookRevenueData.GetLaneBenchMilesByBookODControl(oBookRevs(0).BookODControl)
                oBookRevs(0).BookMilesFrom = laneBenchMiles
            Else
                oBookRevs(0).BookMilesFrom = oBookRevs(0).BookRevLaneBenchMiles
            End If
        End If
    End Sub

    ''' <summary>
    ''' THIS IS THE OVERLOAD I AM WRITING FOR THE LOAD BOARDS
    ''' </summary>
    ''' <param name="oBookRevs"></param>
    ''' <param name="oSelectedBooking"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="AcceptRejectCode"></param>
    ''' <param name="SendEmail"></param>
    ''' <param name="BookTrackComment"></param>
    ''' <param name="BookTrackStatus"></param>
    ''' <param name="NotificationEMailAddress"></param>
    ''' <param name="NotificationEMailAddressCc"></param>
    ''' <param name="AcceptRejectMode"></param>
    ''' <param name="UserName"></param>
    ''' <param name="results"></param>
    ''' <param name="intValidationFlags"></param>
    ''' <param name="NewTranCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function AcceptOrRejectLoad(ByRef oBookRevs As DTO.BookRevenue(),
                                       ByRef oSelectedBooking As DTO.BookRevenue,
                                       ByVal CarrierControl As Integer,
                                       ByVal CarrierContControl As Integer,
                                       ByVal AcceptRejectCode As AcceptRejectEnum,
                                       ByVal SendEmail As Boolean,
                                       ByVal BookTrackComment As String,
                                       ByVal BookTrackStatus As Integer,
                                       ByVal NotificationEMailAddress As String,
                                       ByVal NotificationEMailAddressCc As String,
                                       ByVal AcceptRejectMode As AcceptRejectModeEnum,
                                       ByVal UserName As String,
                                       Optional ByRef results As DTO.WCFResults = Nothing,
                                       Optional ByVal intValidationFlags As Int64 = 0,
                                       Optional ByVal NewTranCode As String = "",
                                       Optional ByVal BidStatusCode As Integer = BSCEnum.OpsDeletePost) As DTO.WCFResults
        If results Is Nothing Then results = New DTO.WCFResults With {.Success = True}
        UserName = If(String.IsNullOrEmpty(UserName) OrElse UserName.Trim.Length < 1, Me.Parameters.UserName, UserName)
        Try
            Dim LoadTenderTypeMode As New BitwiseFlags32
            Dim LoadStatusCodeActions As New BitwiseFlags32

            'Convert the AcceptRejectModeEnum to Bitwise Flag to use in our new version of AcceptReject 
            LoadTenderTypeMode = NGLDATBLL.ConvertARModeEnumToBitwiseFlag(AcceptRejectMode)

            'I don't think we necessarily need to turn on any flags as default for LoadStatusCodeActions because right now we are only using that for Load Board actions

            'Modified by LVV on 1/13/17 for v-8.0 Next Stop
            Return AcceptOrRejectLoad(oBookRevs, oSelectedBooking, CarrierControl, CarrierContControl, AcceptRejectCode, SendEmail, BookTrackComment, BookTrackStatus, NotificationEMailAddress, NotificationEMailAddressCc, LoadTenderTypeMode, LoadStatusCodeActions, UserName, results, intValidationFlags, BidStatusCode:=BidStatusCode)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AcceptOrRejectLoad"), DAL.sysErrorParameters.sysErrorState.SystemLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return results
    End Function

    ''' <summary>
    ''' THIS IS THE OVERLOAD I MADE FOR THE LOAD BOARDS
    ''' </summary>
    ''' <param name="oBookRevs"></param>
    ''' <param name="oSelectedBooking"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="AcceptRejectCode"></param>
    ''' <param name="SendEmail"></param>
    ''' <param name="BookTrackComment"></param>
    ''' <param name="BookTrackStatus"></param>
    ''' <param name="NotificationEMailAddress"></param>
    ''' <param name="NotificationEMailAddressCc"></param>
    ''' <param name="LoadTenderTypeMode"></param>
    ''' <param name="LoadStatusCodeActions"></param>
    ''' <param name="UserName"></param>
    ''' <param name="results"></param>
    ''' <param name="intValidationFlags"></param>
    ''' <param name="NewTranCode"></param>
    ''' <returns>DTO.WCFResults</returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' 
    ''' NOTE: This method requires a valid booking record; when processing EDI Accept or Reject messages 
    ''' the order may have been canceled and may no longer exist.
    ''' The EDI calling procedure will need to check for Book records and if none exist it will need to call
    ''' NGLBookData.UpdateBookDeletedEDIExported(oSelectedBooking.BookControl, CarrierControl) to be sure the EDI 
    ''' canceled 204 status gets updated!!
    ''' Modified by RHR v-7.0.5.100 05/17/2016
    '''   we now include SuppressEmailWhenLoadsManuallyAccepted and SuppressEmailChangesAfterLoadShipped 
    '''   company level parameters
    ''' Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes
    '''  For Dropped, Rejected, Expired, Removed: If there is a single BookRev record then reset the stem miles to the lane miles
    ''' Modified by RHR for v-8.4 on 05/03/2021 
    '''     added logic to generate the Load Tender Token information
    ''' Modified by RHR for v-8.4.0.003 on 08/27/2021 we allow email for  NextStop when AcceptRejectEnum.Tendered is selected 
    ''' </remarks>
    Public Function AcceptOrRejectLoad(ByRef oBookRevs As DTO.BookRevenue(),
                                       ByRef oSelectedBooking As DTO.BookRevenue,
                                       ByVal CarrierControl As Integer,
                                       ByVal CarrierContControl As Integer,
                                       ByVal AcceptRejectCode As AcceptRejectEnum,
                                       ByVal SendEmail As Boolean,
                                       ByVal BookTrackComment As String,
                                       ByVal BookTrackStatus As Integer,
                                       ByVal NotificationEMailAddress As String,
                                       ByVal NotificationEMailAddressCc As String,
                                       ByVal LoadTenderTypeMode As BitwiseFlags32,
                                       ByVal LoadStatusCodeActions As BitwiseFlags32,
                                       ByVal UserName As String,
                                       Optional ByRef results As DTO.WCFResults = Nothing,
                                       Optional ByVal intValidationFlags As Int64 = 0,
                                       Optional ByVal NewTranCode As String = "",
                                       Optional ByVal BidStatusCode As Integer = BSCEnum.OpsDeletePost) As DTO.WCFResults
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = oSelectedBooking.BookControl, .Success = True}
        'TODO:  Add logic to retender using cascading dispatching when rejected or expired
        '       Find out what the difference is between reject and expire
        '       Test that expire works on all loads tendered not just Web tender
        '       Add logic to handle AcceptRejectCode of Unfinalize
        '       Hold:  we are not sure we need to use the new optional parameter NewTranCode: 
        '           this allows the user to unfinalize to P  not just go to N like when a load is rejected.
        '       Add logic to handle the AcceptRejectEnum.Unfinalize vs AcceptRejectEnum.Reject logic for manual processing
        If oSelectedBooking Is Nothing OrElse oSelectedBooking.BookControl = 0 Then
            results.Success = False
            results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound) 'Warning!  The booking record cannot be found.
            Return results
        End If
        Dim intSelectedCarrierControl As Integer = oSelectedBooking.BookCarrierControl 'store the current carrier control for use in cascading dispatching
        Try
            'Added By LVV on 6/13/19
            'Test the global parameter DATCarrierNumber and compare the value with that of the assigned carrier to the selected load. 
            'If it matches then any changes made to the TransCode except Expired will result in an automatic Reject of the load, removing the carrier and changing the status back To N. 
            'This change will guarantee that positing to NEXTStop/DAT will be deleted.
            ' Modified by RHR for v-8.4.0.003 on 08/27/2021 we allow email for  NextStop when AcceptRejectEnum.Tendered is selected we set blnIsDATCarrier to true
            Dim blnIsDATCarrier = False
            Dim oCarDict = NGLCarrierData.getCarrierNameNumber(CarrierControl)
            If oCarDict?.Count > 0 AndAlso oCarDict.ContainsKey("CarrierNumber") Then
                Dim dblCarNo As Double = 0
                Dim strCarNum = oCarDict("CarrierNumber")
                Double.TryParse(strCarNum, dblCarNo)
                Dim loadBoardCarNo = NGLParameterData.GetParValue("DATCarrierNumber")
                If loadBoardCarNo = dblCarNo Then blnIsDATCarrier = True
                If blnIsDATCarrier AndAlso AcceptRejectCode <> AcceptRejectEnum.Expired AndAlso AcceptRejectCode <> AcceptRejectEnum.Tendered Then AcceptRejectCode = AcceptRejectEnum.Rejected
            End If

            '1. Process EDI Canceled Orders  See notes in <remarks> above about canceled orders 
            ' NOTE: The UpdateBookDeletedEDIExported method should be called by the EDI program when seperately when the accept on 997 
            '       is turned off because we should mark the 204 as revieved when a 997 is recieved but we do not call AcceptOrReject until the 990 is recieved
            If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.EDI204) Then
                results.AddLog("Check the BookDeleted transaction table for EDI 204 updates that have been accepted or rejected.")
                If NGLBookData.UpdateBookDeletedEDIExported(oSelectedBooking.BookControl, CarrierControl) Then
                    results.AddMessage(DTO.WCFResults.MessageType.Messages, DTO.WCFResults.MessageEnum.M_BookDeletedEDIMarkedAsExported)
                End If
            End If
            '2.	Validate the Booking Records For this Load
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                results.Success = False
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound) 'Warning!  The booking record cannot be found.
                Return results
            End If

            'Added by LVV 7/5/16 for v-7.0.5.110 DAT
            'Modified by RHR v-7.0.5.110 7/11/2016 we now use getLoadTenderTypeEnum
            'Check the AcceptRejectMode value and update the LoadTenderTypeControl flag in the Book table
            Dim source = "NGL.FM.BLL.NGLBookBLL.AcceptOrRejectLoad"
            'Dim oEnum As DTO.tblLoadTender.LoadTenderTypeEnum = getLoadTenderTypeEnum(LoadTenderTypeMode)
            'Modfied by LVV on 1/13/17 for v-8.0 Next Stop
            'For Each b In oBookRevs
            '    b.BookRevLoadTenderTypeControl = NGLDATBLL.UpdateBookRevLoadTenderTypeControl(b.BookRevLoadTenderTypeControl, LoadTenderTypeMode)
            'Next

            'We never send load tender emails if we are posting to a Load Board
            ' Modified by RHR for v-8.4.0.003 on 08/27/2021 we allow email for  NextStop when AcceptRejectEnum.Tendered is selected
            'If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) OrElse LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then SendEmail = False
            If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) OrElse LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Or blnIsDATCarrier Then
                If Not LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then
                    SendEmail = False
                Else
                    If AcceptRejectCode <> AcceptRejectEnum.Tendered Then SendEmail = False
                End If
            End If
            results.Key = oSelectedBooking.BookControl
            results.AlphaCode = oSelectedBooking.BookProNumber
            results.updateKeyFields("BookConsPrefix", oSelectedBooking.BookConsPrefix)
            '3.	Validate the Carrier Information
            If oBookRevs.Any(Function(x) x.BookCarrierControl <> CarrierControl) Then
                results.AddLog("The carrier is not valid or has been changed.")
                results.Success = False
                Dim intUnMatchedCarrier As Integer = (From d In oBookRevs Where d.BookCarrierControl <> CarrierControl Select d.BookCarrierControl).FirstOrDefault()
                Dim oPar(3) As String
                oPar(0) = [Enum].GetName(GetType(AcceptRejectEnum), AcceptRejectCode)
                'get the carrier name for the first assigned carrier that does not match
                Dim oUnmatchedCarDict = NGLCarrierData.getCarrierNameNumber(intUnMatchedCarrier)
                If oUnmatchedCarDict?.Count > 0 AndAlso oUnmatchedCarDict.ContainsKey("CarrierName") Then oPar(1) = oUnmatchedCarDict("CarrierName") Else oPar(1) = " Carrier Control Number " & intUnMatchedCarrier.ToString
                'Get the Carrier name for the previous carrier
                'Modified By LVV on 6/13/19 - we added a call to getCarrierNameNumber(CarrierControl) above so we don't need to call it again
                'odic = NGLCarrierData.getCarrierNameNumber(CarrierControl)
                If oCarDict?.Count > 0 AndAlso oCarDict.ContainsKey("CarrierName") Then oPar(2) = oCarDict("CarrierName") Else oPar(2) = " Carrier Control Number " & CarrierControl.ToString
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_CarrierDoesNotMatch, oPar)
                results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                Return results
            End If
            '4.	Get the Carrier and Carrier Contact Email Notification information for the selected order
            results.AddLog("Get the Carrier and Carrier Contact Email Notification information for the selected order")
            Dim carCont = NGLCarrierData.GetCarrierContactInformation(oSelectedBooking.BookControl)
            If carCont Is Nothing Then
                carCont = New LTS.spGetCarrierContactInfoResult With {.CarrierName = "Unknown", .CarrierNumber = 0, .CarrierEmail = ""}
                'Carrier contact information is not required when posting to NEXStop or to DAT
                If Not LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) AndAlso Not LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then
                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_CarrContEmailNotifInfoNotAvailable)
                    results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                End If
            End If
            results.updateKeyFields("CarrierName", carCont.CarrierName)
            results.updateKeyFields("CarrierNumber", carCont.CarrierNumber)
            results.updateKeyFields("CarrierEmail", carCont.CarrierEmail)
            '5.	Check Accept Reject Codes, Validate Load Status Code, and Select LoadStatusControl 
            results.AddLog("Check Accept Reject Codes, Validate Load Status Code, and Select LoadStatusControl")
            'Modified by LVV on 1/13/17 for v-8.0 Next Stop
            Dim eAcceptRejectMode = NGLDATBLL.getAcceptRejectModeEnum(LoadTenderTypeMode)
            Dim oARejInfo As New clsAcceptRejectCodeInfo(Me.Parameters, eAcceptRejectMode, AcceptRejectCode, oSelectedBooking.BookRouteFinalCode)
            'TODO: verify that the updateAcceptRejectCodeInfo reads the correct tran code value.
            'performs old logic in one method for getting the Load Status Code, Accept Msg, Email Subject & Body.  Uses BookRouteFinalCode
            'and AcceptRejectMode.  It updates the BookTranCode property of the clsAcceptRejectCodeInfo object to match the current conditions?
            oARejInfo.updateAcceptRejectCodeInfo()
            'TODO:  add logic for email messages to check for Accept by System or Tender by system
            '   Use correct parameter stttings for emails and messages.
            Dim oEmailLocalization As DTO.EmailLocalizationData = prepareEmailLoacalization(oSelectedBooking.BookSHID, oSelectedBooking.BookConsPrefix, BookTrackComment, carCont.CarrierNumber, carCont.CarrierName, oARejInfo)
            '6.	Read the GetAutoTenderData Detail Data
            results.AddLog("Read the GetAutoTenderData Detail Data")
            'the spGetAutoTenderDataResult contains special info like:
            'LaneAutoTenderFlag
            'LaneCascadingDispatchingFlag
            'TenderCompControl
            'CompContEmails
            'dbo.getParText('GlobalFromEmail') as MailFrom,	
            'dbo.getParText('GlobalGroupEmail') as GlobalGroupEmail,
            'dbo.getParValue ('AutoTender')  as AutoTender,
            'dbo.getParValue ('AutoTenderCascadingDispatchingDepth') as AutoTenderCascadingDispatchingDepth,
            'dbo.getParValue ('AutoTenderCascadingDispatching') as AutoTenderCascadingDispatching,
            'dbo.getParValue('AutoTenderCascadingDispatchingEmailAlerts') as AutoTenderCascadingDispatchingEmailAlerts,
            'and NbrOfTenders (a count of the records int he BookCarrTend table used for cascading dispatching
            'TODO:  The spGetAutoTenderDataResult is not optimized and is causing performance issues.  It should only be 
            '       called when needed and the view needs to be optimized.  the calls to udfGetTenderCompControl 
            '       and udfGetCompContNotifyEmails are slow and may be redundant.
            '      vData is used by prepareCompanyContactEmail and CanAutoTender
            Dim vData As LTS.spGetAutoTenderDataResult = NGLBookData.GetAutoTenderData(oSelectedBooking.BookControl)
            '7.	Determine which company contact emails should be used 
            'TODO  we use vData.CompContEmails in prepareCompanyContactEmail.  this is generated via udfGetCompContNotifyEmails
            '       check if we can optimize this functionality
            results.AddLog("Determine which company contact emails should be used")
            Dim CompContEmail As String = prepareCompanyContactEmail(NotificationEMailAddress, NotificationEMailAddressCc, vData, results)
            '8.	Update Drop Load table data
            'Modified by RHR v-7.0.5.110 7/11/2016 - added flags to test for DAT success or failure
            Dim blnDATDeleteSuccess As Boolean = True
            Dim blnDATPostingSuccess As Boolean = True
            Dim blnDATGetDataSuccess As Boolean = True
            Dim blnDATIsUpdate As Boolean = False
            Dim datRes As New DTO.DATResults
            Dim blnNSDeleteSuccess As Boolean = True
            Dim blnNSPostingSuccess As Boolean = True
            Dim blnNSGetDataSuccess As Boolean = True
            Dim blnNSIsUpdate As Boolean = False
            Dim nsRes As New DTO.DATResults

            Dim blnBookRevSaveComplete As Boolean = True
            'Modified by LVV on 9/11/19
            If AcceptRejectCode = AcceptRejectEnum.Expired Or AcceptRejectCode = AcceptRejectEnum.Rejected Or AcceptRejectCode = AcceptRejectEnum.Dropped Then
                results.AddLog("Update Drop Load table data")
                updateDropLoad(AcceptRejectCode, carCont.CarrierNumber, UserName, BookTrackComment, oEmailLocalization, oBookRevs, results)
            End If
            'Modified by LVV on 9/11/19 - Added call to delete load boards if Unassign was selected
            If AcceptRejectCode = AcceptRejectEnum.Expired Or AcceptRejectCode = AcceptRejectEnum.Rejected Or AcceptRejectCode = AcceptRejectEnum.Dropped Or AcceptRejectCode = AcceptRejectEnum.Unassigned Then
                'Added by LVV 7/1/16 for v-7.0.5.110 DAT
                'Modified by RHR v-7.0.5.110 7/11/2016
                'Modified by LVV on 1/13/17 for v-8.0 Next Stop

                'TODO Putting this in a loop like this would be better for clean code and easier to add more loadboards without expanding the AcceptReject method
                'for each type in the lttype table -- get the records form db
                'Dim loadtendertype As New List(Of LTS.tblLoadTenderType)
                'For Each Type In loadtendertype
                '    If LoadTenderTypeMode.isBitFlagOn(Type.LoadTenderTypeBitPosition) AndAlso (LoadStatusCodeActions.isBitFlagOn(LTSCEnum.DATDeleted) OrElse LoadStatusCodeActions.isBitFlagOn(LTSCEnum.DATExpired)) Then
                '        datRes = NGLDATBLL.DeleteDATPosting(oBookRevs, AcceptRejectCode)
                '        If Not datRes.Warnings Is Nothing AndAlso datRes.Warnings.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
                '        blnDATDeleteSuccess = datRes.Success
                '    End If
                'Next

                Dim bwBookRevSC As New BitwiseFlags32(oSelectedBooking.BookRevLoadTenderStatusCode)
                Dim bwBookRevLTTC As New BitwiseFlags32(oSelectedBooking.BookRevLoadTenderTypeControl)

                'Check for active DAT Posting
                'Active --> Post or Update Flags are True AndAlso Delete, Expired, Error, Accepted are all False
                If NGLDATBLL.HasActiveLBPosting(LTTypeEnum.DAT, bwBookRevLTTC, bwBookRevSC) Then
                    'Delete DAT
                    datRes = NGLDATBLL.DeleteDATPosting(results, oBookRevs, blnDATDeleteSuccess, AcceptRejectCode)
                    LoadTenderTypeMode.turnBitFlagOn(LTTypeEnum.DAT)
                    LoadTenderTypeMode.turnBitFlagOff(LTTypeEnum.Manual)
                End If
                If NGLDATBLL.HasActiveLBPosting(LTTypeEnum.NextStop, bwBookRevLTTC, bwBookRevSC) Then
                    'Delete Next Stop
                    nsRes = NGLDATBLL.NSDeletePost(results, oBookRevs, blnNSDeleteSuccess, AcceptRejectCode, BidStatusCode)
                    LoadTenderTypeMode.turnBitFlagOn(LTTypeEnum.NextStop)
                    LoadTenderTypeMode.turnBitFlagOff(LTTypeEnum.Manual)
                End If
            End If
            '9.	Update Book Data (We may need to get an updated copy of the data depending on how long the previous task took to execute because the data may have been modified by another user or process)
            ' If BookRouteFinalCode = 'NS' and this load is accepted via EDI This load is already finalized and this is the acceptance (via EDI) of an update changes.
            'Modified by RHR new Try Catch partially completed
            Try
                Select Case AcceptRejectCode
                    Case AcceptRejectEnum.Accepted
                        'test if there was a problem with the AcceptRejectCode
                        If oARejInfo.InvalidAcceptRejectCode Then oARejInfo.BookTranCode = "PB" 'finalize is always PB
                        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.EDI204) And oSelectedBooking.BookRouteFinalCode = "NS" Then
                            results.AddLog("Changes transmitted via EDI were accepted by carrier.")
                            'this load is already finalized and this is an acceptance via EDI of changes when
                            'the load was not unfinalized but a new 204 was generated
                            'so just update the Final Code and Trans Code
                            For Each b In oBookRevs
                                b.BookTranCode = oARejInfo.BookTranCode
                                b.BookRouteFinalCode = "SH"
                                b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                            Next
                            results.AddLog("Save changes to booking records")
                            NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
                        Else
                            FinalizeBookings(oBookRevs, oARejInfo.BookTranCode, results, intValidationFlags)
                        End If
                        'Send a load tender email if we are accepting a NEXTStop Bid
                        'Modified by RHR for v-8.4 on 05/03/2021 
                        '   added logic to generate the Book Appt Token information
                        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then
                            SendEmail = True
                        Else
                            'add logic here to generate the Book Appointment by Token on Accept information
                            Dim iExpMin As Integer = 0
                            Dim sEmailBody As String = ""
                            'Note: all lanes must be the same or this Token logic is not allowed.
                            Dim iLaneControl = oSelectedBooking.BookODControl
                            If Not oBookRevs.Any(Function(x) x.BookODControl <> iLaneControl) Then
                                '   ServiceTokenBookControl
                                '   ServiceTokenLaneControl
                                '   ServiceTokenExpirationDate from iExpMin  or AutoExpireApptTenderTokenMin
                                '   ServiceTokenServiceTypeControl = DAL.NGLtblServiceTokenData.TokenServiceType.CarrierBookApptWithToken
                                'Call NGLtblServiceTokenData.InsertOrUpdatetblServiceToken() With And New tblServiceToken record
                                Dim bUseCarrieContEmail As Boolean
                                Dim sBookApptEmail As String
                                If NGLLaneData.AllowLaneBookApptTokenByEmail(iLaneControl, oSelectedBooking.BookCustCompControl, iExpMin, bUseCarrieContEmail, sBookApptEmail) Then
                                    Dim oTokenData As New LTS.tblServiceToken
                                    With oTokenData
                                        .ServiceTokenBookControl = oSelectedBooking.BookControl
                                        .ServiceTokenCarrierControl = CarrierControl
                                        .ServiceTokenCompControl = oSelectedBooking.BookCustCompControl
                                        .ServiceTokenLaneControl = iLaneControl
                                        .ServiceTokenExpirationDate = Date.Now.AddMinutes(iExpMin)
                                        .ServiceTokenServiceTypeControl = DAL.NGLtblServiceTokenData.TokenServiceType.CarrierBookApptWithToken
                                    End With
                                    oTokenData = NGLtblServiceTokenData.InsertOrUpdatetblServiceToken(oTokenData)

                                    If oTokenData IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(oTokenData.ServiceToken) Then
                                        Dim sEmailTo As String = carCont.CarrierEmail
                                        If Not bUseCarrieContEmail Then
                                            sEmailTo = sBookApptEmail
                                        End If
                                        If Not String.IsNullOrWhiteSpace(sEmailTo) Then
                                            Dim sSubject As String = "Book Appointment For SHID " & oSelectedBooking.BookSHID
                                            Dim sAcceptUrl As String = NGLtblServiceTokenData.GetParText("NEXTrackURL", oSelectedBooking.BookCustCompControl) & "/CarrierBookAppt?token=" & oTokenData.ServiceToken
                                            'add the link and message to the Tender Email
                                            sEmailBody &= String.Format("{0}{1}A load with SHID Number {3} is ready to book an appointment.{0}{1}Details:{0}{1}Consolidation Number: {4}{0}{1}Order Number: {5}{0}{1}Pickup Date: {6} {0}{1}Click or copy the link below into your browser to Book the appointment.{0}{1}{2}", vbCrLf, "<br><br>", sAcceptUrl, oSelectedBooking.BookSHID, oSelectedBooking.BookConsPrefix, oSelectedBooking.BookCarrOrderNumber, If(oSelectedBooking.BookDateLoad.HasValue, oSelectedBooking.BookDateLoad.Value.ToShortDateString(), "Pending"))
                                            NGLBatchProcessData.executeGenerateTenderEmailProcedure(oSelectedBooking.BookControl, intSelectedCarrierControl, oSelectedBooking.BookProNumber, oSelectedBooking.BookConsPrefix, sEmailTo, CompContEmail, sSubject, sEmailBody)
                                        End If

                                    End If
                                End If

                            End If
                        End If
                    Case AcceptRejectEnum.Rejected
                        For Each b In oBookRevs
                            b.ResetToNStatus()
                        Next
                        'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes - If there is a single BookRev record then reset the stem miles to the lane miles
                        'Modified By LVV on 6/13/19 - put code in method to make it easier to maintain - was repeated in multiple places
                        ResetStemMilesToLaneMiles(oBookRevs)
                        results.AddLog("Save changes to booking records")
                        NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, True, True)
                    Case AcceptRejectEnum.Expired
                        For Each b In oBookRevs
                            b.ResetToNStatus()
                        Next
                        'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes - If there is a single BookRev record then reset the stem miles to the lane miles
                        'Modified By LVV on 6/13/19 - put code in method to make it easier to maintain - was repeated in multiple places
                        ResetStemMilesToLaneMiles(oBookRevs)
                        results.AddLog("Save changes to booking records")
                        NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, True, True)
                    Case AcceptRejectEnum.Unfinalize
                        'test if there was a problem with the AcceptRejectCode
                        If oARejInfo.InvalidAcceptRejectCode Then oARejInfo.BookTranCode = "P" 'unfinalize is always P
                        UnFinalizeBookings(oBookRevs, oARejInfo.BookTranCode, results, intValidationFlags)
                    Case AcceptRejectEnum.Tendered
                        If oARejInfo.InvalidAcceptRejectCode Then oARejInfo.BookTranCode = "PC" 'tender is always PC
                        For Each b In oBookRevs
                            b.BookTranCode = oARejInfo.BookTranCode
                            b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                        Next
                        'NOTE: I don't think this Update Code is necessary because if a load already has ANY posting it doesn't
                        'need to go through Accept Reject -- updates should be called directly from LoadBoardPostMethod
                        'You can't Post through the Booking Menu -- only through the Post button
                        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then
                            If LoadStatusCodeActions.isBitFlagOn(LTSCEnum.DATPosted) Then datRes = NGLDATBLL.DATPost(results, oBookRevs, blnDATGetDataSuccess, blnDATPostingSuccess, oSelectedBooking)
                        End If
                        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then
                            If LoadStatusCodeActions.isBitFlagOn(LTSCEnum.NStopPosted) Then nsRes = NGLDATBLL.NSPost(results, oBookRevs, blnNSGetDataSuccess, blnNSPostingSuccess, oSelectedBooking)
                        End If
                        'This is a method to determine if we should call SaveRevenuesNoReturn
                        'See the in line comments in the method GetTenderWillSaveBookRev for more details
                        Dim blnSaveRev = NGLDATBLL.GetTenderWillSaveBookRev(blnDATPostingSuccess, blnNSPostingSuccess, LoadTenderTypeMode)
                        If blnSaveRev Then
                            results.AddLog("Save changes to booking records")
                            NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
                        Else
                            blnBookRevSaveComplete = False
                        End If
                    Case AcceptRejectEnum.Dropped
                        For Each b In oBookRevs
                            b.ResetToNStatus()
                        Next
                        'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes - If there is a single BookRev record then reset the stem miles to the lane miles
                        'Modified By LVV on 6/13/19 - put code in method to make it easier to maintain - was repeated in multiple places
                        ResetStemMilesToLaneMiles(oBookRevs)
                        results.AddLog("Save changes to booking records")
                        UnFinalizeBookings(oBookRevs, oARejInfo.BookTranCode, results, intValidationFlags)
                        'NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, True, True)
                    Case AcceptRejectEnum.Unassigned
                        'Return to N status 
                        For Each b In oBookRevs
                            b.ResetToNStatus()
                        Next
                        'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes - If there is a single BookRev record then reset the stem miles to the lane miles
                        'Modified By LVV on 6/13/19 - put code in method to make it easier to maintain - was repeated in multiple places
                        ResetStemMilesToLaneMiles(oBookRevs)
                        results.AddLog("Save changes to booking records")
                        NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, True, True)
                    Case AcceptRejectEnum.ModifyUnaccepted
                        'reset to P
                        For Each b In oBookRevs
                            b.BookTranCode = "P"
                            b.BookRouteFinalCode = "" 'be sure the BookRouteFinalCode is clear so it can be re-tendered
                            b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                        Next
                        results.AddLog("Save changes to booking records")
                        NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
                    Case Else
                        SendEmail = False
                        If oARejInfo.InvalidAcceptRejectCode Then oARejInfo.BookTranCode = oSelectedBooking.BookTranCode
                        'just update the trancode data
                        For Each b In oBookRevs
                            b.BookTranCode = oARejInfo.BookTranCode
                            b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                        Next
                        results.AddLog("Save changes to booking records")
                        NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
                End Select
            Catch ex As FaultException
                blnBookRevSaveComplete = False
                If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, AcceptRejectCode, blnDATDeleteSuccess, blnDATPostingSuccess, blnBookRevSaveComplete, source, blnDATIsUpdate)
                If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then NGLDATBLL.ProcessLoadBoardStatusUpdates(nsRes, AcceptRejectCode, blnNSDeleteSuccess, blnNSPostingSuccess, blnBookRevSaveComplete, source, blnNSIsUpdate)
                Throw
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("AcceptOrRejectLoad"), DAL.sysErrorParameters.sysErrorState.SystemLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Critical)
            End Try
            'partially modified by RHR
            If results.Success AndAlso blnBookRevSaveComplete Then
                'If results.Success Then


                '10. Insert Book Tracking Record For Load Status 
                results.AddLog("Update the Load Status")
                Try
                    'check if we are processing a DAT message and call the ProcessLoadBoardStatusUpdates (put success msg there too)
                    If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) OrElse LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then
                        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, AcceptRejectCode, blnDATDeleteSuccess, blnDATPostingSuccess, blnBookRevSaveComplete, source, blnDATIsUpdate)
                        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then NGLDATBLL.ProcessLoadBoardStatusUpdates(nsRes, AcceptRejectCode, blnNSDeleteSuccess, blnNSPostingSuccess, blnBookRevSaveComplete, source, blnNSIsUpdate)
                    Else
                        'else call what is already there
                        If Not oARejInfo.InvalidAcceptRejectCode AndAlso Not oEmailLocalization Is Nothing Then
                            UpdateBookTracksForLoad(oSelectedBooking.BookControl, oEmailLocalization.getFormattedBodyNoHTML(), oARejInfo.LoadStatusControl, UserName, Date.Now(), oEmailLocalization.BodyLocalized, oEmailLocalization.BodyKeys)
                        End If
                    End If
                Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
                    'sqlEx.Reason.ToString(, sqlEx.Detail.Message, sqlEx.Detail.Details)
                    results.AddFaultException(DTO.WCFResults.MessageEnum.E_SQLFaultCannotUpdateLoadStatus, sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.Detail.DetailsList)
                    results.setAction(DTO.WCFResults.ActionEnum.ShowErrors)
                Catch ex As Exception
                    'TODO: the caller must capture and process any error messages. 
                    '   If the caller is an unattended execution procedure it should send emails to the administrator.  This can be accomplished via the system alert logic.
                    results.AddUnexpectedError(ex)
                End Try

                '11. Insert Book Tracking Record For Comments
                results.AddLog("Update any Load Status Comments")
                Try
                    If Not String.IsNullOrWhiteSpace(BookTrackComment) AndAlso Not oARejInfo.InvalidAcceptRejectCode AndAlso Not oEmailLocalization Is Nothing Then _
                        UpdateBookTracksForLoad(oSelectedBooking.BookControl, BookTrackComment, oARejInfo.LoadStatusControl, UserName, Date.Now(), oEmailLocalization.BodyLocalized, oEmailLocalization.BodyKeys)
                Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
                    'sqlEx.Reason.ToString(, sqlEx.Detail.Message, sqlEx.Detail.Details)
                    results.AddFaultException(DTO.WCFResults.MessageEnum.E_SQLFaultCannotUpdateLoadStatus, sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.Detail.DetailsList)
                    results.setAction(DTO.WCFResults.ActionEnum.ShowErrors)
                Catch ex As Exception
                    'TODO: the caller must capture and process any error messages. 
                    '   If the caller is an unattended execution procedure it should send emails to the administrator.  This can be accomplished via the system alert logic.
                    results.AddUnexpectedError(ex)
                End Try
                '12. Send the accept/expired/reject email
                'Modified by RHR v-7.0.5.100 05/17/2016
                'We now check the SuppressEmailChangesAfterLoadShipped company level parameter to determine if SendEMail should be false.  
                'This parameter has no effect if SendEMail is already false
                If SendEmail = True _
                    AndAlso oBookRevs.Any(Function(x) If(x.BookDateLoad, Date.Now()) < CDate(Date.Now.ToShortDateString())) _
                    AndAlso Not vData Is Nothing _
                    AndAlso vData.SuppressEmailChangesAfterLoadShipped <> 0 Then
                    SendEmail = False
                End If
                'Modified by RHR v-7.0.5.100 05/17/2016
                'We now check the SuppressEmailWhenLoadsManuallyAccepted company level parameter to determine if SendEMail should be false.
                'This parameter has no effect if SendEMail is already false
                If SendEmail = True _
                    AndAlso Not oEmailLocalization Is Nothing _
                    AndAlso oEmailLocalization.Type = DAL.Utilities.EmailLocalizationTypesEnum.LoadManuallyAccepted _
                    AndAlso Not vData Is Nothing _
                    AndAlso vData.SuppressEmailWhenLoadsManuallyAccepted <> 0 Then
                    If Not (LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.SpotRate) OrElse LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.LoadBoard) OrElse LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.RateShopping)) Then SendEmail = False
                End If

                'Modified by RHR for v-8.4 on 05/03/2021 
                '   added logic to generate the Load Tender Token information
                If SendEmail And Not oEmailLocalization Is Nothing Then
                    Dim iExpMin As Integer = 0
                    Dim sEmailBody As String = oEmailLocalization.getFormattedBody

                    If AcceptRejectCode = AcceptRejectEnum.Tendered Then
                        'add logic here to generate the Load Tender Token information
                        'key fields required: 
                        '   ServiceTokenBookControl
                        '   ServiceTokenExpirationDate from  iExpMin or AutoExpireTenderTokenMin
                        '   ServiceTokenServiceTypeControl = DAL.NGLtblServiceTokenData.TokenServiceType.CarrierAcceptLoadWithToken
                        'Call NGLtblServiceTokenData.InsertOrUpdatetblServiceToken() With And New tblServiceToken record
                        If NGLLegalEntityCarrierData.AllowLECarAcceptRejectTokenByEmail(CarrierControl, oSelectedBooking.BookCustCompControl, iExpMin) Then
                            Dim oTokenData As New LTS.tblServiceToken
                            With oTokenData
                                .ServiceTokenBookControl = oSelectedBooking.BookControl
                                .ServiceTokenCarrierControl = CarrierControl
                                .ServiceTokenCompControl = oSelectedBooking.BookCustCompControl
                                .ServiceTokenExpirationDate = Date.Now.AddMinutes(iExpMin)
                                .ServiceTokenServiceTypeControl = DAL.NGLtblServiceTokenData.TokenServiceType.CarrierAcceptLoadWithToken
                            End With
                            oTokenData = NGLtblServiceTokenData.InsertOrUpdatetblServiceToken(oTokenData)

                            If oTokenData IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(oTokenData.ServiceToken) Then
                                Dim sAcceptUrl As String = NGLtblServiceTokenData.GetParText("NEXTrackURL", oSelectedBooking.BookCustCompControl) & "/CarrierAcceptRejectLoad?token=" & oTokenData.ServiceToken
                                'add the link and message to the Tender Email
                                sEmailBody &= String.Format("{0}{1}Click or copy the link below into your browser to Accept or Reject this Load {0}{1}{2}", vbCrLf, "<br><br>", sAcceptUrl)
                            End If
                        End If
                    End If

                    NGLBatchProcessData.executeGenerateTenderEmailProcedure(oSelectedBooking.BookControl, intSelectedCarrierControl, oSelectedBooking.BookProNumber, oSelectedBooking.BookConsPrefix, carCont.CarrierEmail, CompContEmail, oEmailLocalization.getFormattedSubject, sEmailBody)
                End If

                '13. if the load is rejected or expired and this is not a manual process 
                'Call the Cascade Dispatching and Auto Tender Load Procedures 
                'so that a new carrier will be selected
                'If this process fails we do not return false because the Reject or Expire has 
                'already been process.  We do add messages to the results object and we generate
                'a subscription alert when needed.  If the caller needs to process failed cascade 
                'dispatching or auto tender message they will need to parse the Error, Warning, Message
                'Lists returned (generally this is not desired).
                If (AcceptRejectCode = AcceptRejectEnum.Dropped _
                    Or AcceptRejectCode = AcceptRejectEnum.Expired _
                    Or AcceptRejectCode = AcceptRejectEnum.Rejected) _
                    And
                    Not LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.Manual) Then
                    'AcceptRejectMode <> AcceptRejectModeEnum.MANUAL Then
                    'First get a list of the previous carriers this load was dispatched to
                    Dim lbookControls As List(Of Integer) = oBookRevs.Select(Function(x) x.BookControl).Distinct().ToList()
                    Dim lRestrictedCarriers = NGLBookData.getPreviouslyTenderedCarriers(lbookControls)
                    If lRestrictedCarriers Is Nothing Then lRestrictedCarriers = New List(Of Integer)
                    Dim NbrOfTenders As Integer = 0
                    If Not lRestrictedCarriers Is Nothing Then NbrOfTenders = lRestrictedCarriers.Count()
                    If intSelectedCarrierControl <> 0 AndAlso (Not lRestrictedCarriers.Contains(intSelectedCarrierControl)) Then
                        'add the previous carrier to the list so we do not re-select the dropped, expired, or rejected carrier
                        'this must be performed after the NbrOfTenders count is calculated because 
                        'manually tendered orders are not considered in the number of cascading dispatching 
                        'attempts.
                        lRestrictedCarriers.Add(intSelectedCarrierControl)
                    End If
                    'check if we can cascade dispatch
                    If CanCascadeDispatch(oSelectedBooking.BookControl, oBookRevs, NbrOfTenders, vData, results) Then
                        Dim oCarrierCostResults = BookRevenueBLL.AssignCarrier(oSelectedBooking.BookControl, 0, lRestrictedCarriers)
                        If oCarrierCostResults.Success = False Then
                            'unrecoverable failure so create a subscription alert
                            Dim sbCarrierMessages = New Text.StringBuilder()
                            If oCarrierCostResults.CarriersByCost?.Count > 0 Then
                                For Each c In oCarrierCostResults.CarriersByCost
                                    If c.Messages?.Count > 0 Then c.concatMessage(sbCarrierMessages)
                                Next
                            End If
                            oCarrierCostResults.concatMessage(sbCarrierMessages)
                            With oSelectedBooking
                                Dim strSubject As String = String.Format("Alert - Cannot complete cascading dispatch for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence)
                                results.AddLog(strSubject)
                                createCascadeDispatchFailedSubscriptionAlert(strSubject, .BookCustCompControl, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence.ToString(), .BookConsPrefix, "", "", sbCarrierMessages.ToString())
                            End With
                            results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                            Return results
                        End If
                        'read booking records back.
                        Dim strCarrierNumber As String = "[Not Available]"
                        Dim strCarrierName As String = "[Not Available]"
                        results.AddLog("Get the book records back")
                        Dim inBookcontrol As Integer = oSelectedBooking.BookControl
                        oBookRevs = NGLBookRevenueData.GetBookRevenues(oSelectedBooking.BookControl)
                        'Validate the Booking Records For this Load (this should only happen if someone deletes the order while we are still processing)
                        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                            results.AddLog("BookControl Not Found: " & oSelectedBooking.BookControl.ToString())
                            results.AddMessage(DTO.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
                            SaveSysError(DTO.WCFResults.getMessageNotLocalizedString(DTO.WCFResults.MessageEnum.M_NoOrdersFound), "NGLOrderImportBLL.UpdateDefaultCarrierForUnRouted", "BookControl Not Found: " & oSelectedBooking.BookControl.ToString, 0, sysErrorSeverity.Warning, sysErrorState.SystemLevelFault, 601)
                            Return results
                        End If
                        oSelectedBooking = oBookRevs.Where(Function(x) x.BookControl = inBookcontrol).FirstOrDefault()
                        If oSelectedBooking Is Nothing OrElse oSelectedBooking.BookControl = 0 Then
                            oSelectedBooking = oBookRevs(0)
                        End If
                        'look up the carrier information
                        results.AddLog("Get the updated carrier info")
                        Dim intAssignedCarrier = oSelectedBooking.BookCarrierControl
                        strCarrierNumber = "[Not Available]"
                        strCarrierName = "[Not Available]"
                        If intAssignedCarrier <> 0 Then
                            'Modified By LVV on 6/13/19 - simplified code to make it more readable
                            Dim oAssignedCarDict = NGLCarrierData.getCarrierNameNumber(intAssignedCarrier)
                            If oAssignedCarDict?.Count > 0 Then
                                If oAssignedCarDict.ContainsKey("CarrierName") Then strCarrierName = oAssignedCarDict("CarrierName") Else strCarrierName = " Carrier Control Number " & intAssignedCarrier.ToString 'same as strCarrierID in AutoTender method
                                If oAssignedCarDict.ContainsKey("CarrierNumber") Then strCarrierNumber = oAssignedCarDict("CarrierNumber") Else strCarrierNumber = " Carrier Control Number " & intAssignedCarrier.ToString
                            End If
                        End If
                        'auto tender
                        'send alert if failure.
                        If CanAutoTender(oSelectedBooking.BookControl, oBookRevs, vData, results) Then
                            With oSelectedBooking
                                If Not AutoTender(.BookControl, oBookRevs, results, strCarrierName, True) Then
                                    Dim strSubject As String = String.Format("Alert - Cannot auto tender a load with carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence, strCarrierName)
                                    results.AddLog(strSubject)
                                    BookBLL.createAutoTenderCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, .BookCarrierControl, strCarrierNumber, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence, .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                                    Return results
                                End If
                            End With
                        Else
                            With oSelectedBooking
                                Dim strSubject As String = String.Format("Message - Cannot auto tender a load because the configuration does not allow auto tender with carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence, strCarrierName)
                                results.AddLog(strSubject)
                                BookBLL.createAutoTenderCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, .BookCarrierControl, strCarrierNumber, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence, .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                            End With
                        End If
                    Else
                        With oSelectedBooking
                            Dim strSubject As String = String.Format("Message - Cannot cascade dispatch a load because the configuration does not allow cascade dispatching for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence)
                            results.AddLog(strSubject)
                            createCascadeDispatchFailedSubscriptionAlert(strSubject, .BookCustCompControl, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence.ToString(), .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                        End With
                    End If
                End If
            Else
                'this should call the ProcessLoadBoardStatusUpdates method because the save failed
                If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, AcceptRejectCode, blnDATDeleteSuccess, blnDATPostingSuccess, blnBookRevSaveComplete, source, blnDATIsUpdate)
                If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then NGLDATBLL.ProcessLoadBoardStatusUpdates(nsRes, AcceptRejectCode, blnNSDeleteSuccess, blnNSPostingSuccess, blnBookRevSaveComplete, source, blnNSIsUpdate)
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AcceptOrRejectLoad"), DAL.sysErrorParameters.sysErrorState.SystemLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return results
    End Function

    Public Function ProcessExpiredLoads(ByVal WebDefaults As Dictionary(Of Integer, Tuple(Of Integer, String, String)),
                                        Optional ByVal SendEmail As Boolean = True,
                                        Optional ByVal intValidationFlags As Int64 = 0) As DTO.WCFResults
        Dim AllowedMinutes As Integer = 0
        Dim ExpiredLoadsTo As String = ""
        Dim ExpiredLoadsCc As String = ""
        Dim CompControl As Integer = 0
        Dim ProcessedOrders As New List(Of Integer)
        Dim results As New DTO.WCFResults With {.Success = True}
        'CarrierLoadAcceptanceAllowedMinutes, ExpiredLoadsTo, ExpiredLoadsCc
        Try
            For Each d In WebDefaults.OrderByDescending(Function(x) x.Key)
                CompControl = d.Key
                AllowedMinutes = d.Value.Item1
                ExpiredLoadsTo = d.Value.Item2
                ExpiredLoadsCc = d.Value.Item3
                Dim oExpiredLoads = NGLBookData.GetPendingExpiredOrders(AllowedMinutes, CompControl)
                If Not oExpiredLoads Is Nothing AndAlso oExpiredLoads.Count > 0 Then
                    If oExpiredLoads(0).ErrNumber.HasValue AndAlso oExpiredLoads(0).ErrNumber <> 0 Then
                        results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_ProcessExpiredLoadsError, oExpiredLoads(0).ErrNumber.ToString(), oExpiredLoads(0).RetMsg, CompControl.ToString())
                        results.Success = False
                    Else
                        For Each l In oExpiredLoads
                            If Not ProcessedOrders.Contains(l.BookControl) Then
                                '1. get the Booking records
                                results.AddLog("Read the booking records")
                                Dim oBookRevs = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(l.BookControl)

                                'Added by LVV 7/20/16 for v-7.0.5.110 DAT
                                Dim eAcceptRejectMode = AcceptRejectModeEnum.System
                                'If oBookRevs(0).BookRevLoadTenderTypeControl = LTTypeEnum.DAT Then
                                '    eAcceptRejectMode = AcceptRejectModeEnum.DAT
                                'End If

                                If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                                    results.Success = False
                                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_ProcessExpiredLoadsErrorNoPro, l.BookProNumber)
                                Else
                                    Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = l.BookControl).FirstOrDefault()
                                    results.Key = l.BookControl
                                    results.AlphaCode = oSelectedBooking.BookProNumber
                                    results.updateKeyFields("BookConsPrefix", oSelectedBooking.BookConsPrefix)
                                    results.AddLog("Reset load to N Status")
                                    'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes
                                    'Removed call to b.ResetToNStatus() in For Each because this wipes out the Carrier info so no emails get sent in AcceptOrRejectLoad
                                    'Also, AcceptOrRejectLoad calls ResetToNStatus for Expired loads anyway
                                    For Each b In oBookRevs
                                        If Not ProcessedOrders.Contains(b.BookControl) Then ProcessedOrders.Add(b.BookControl)
                                    Next

                                    AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Expired, SendEmail, "Tendered Load Has Expired", 0, ExpiredLoadsTo, ExpiredLoadsCc, eAcceptRejectMode, "ProcessExpiredLoads", results, intValidationFlags)
                                End If
                            End If
                        Next
                    End If
                End If

            Next
        Catch ex As Exception

        End Try
        Return results
    End Function

    ''' <summary>
    ''' Moved some logic from CMDLine code to here and into the new sp
    ''' </summary>
    ''' <param name="SendEmail"></param>
    ''' <param name="intValidationFlags"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 1/31/2019
    ''' </remarks>
    Public Function ProcessExpiredLoads365(Optional ByVal SendEmail As Boolean = True, Optional ByVal intValidationFlags As Int64 = 0) As DTO.WCFResults
        Dim ProcessedOrders As New List(Of Integer)
        Dim results As New DTO.WCFResults With {.Success = True}
        Try
            Dim comps() As DTO.vLookupList = NGLLookupData.GetViewLookupList("CompActive", 2)
            For Each c In comps
                Dim oExpiredLoads = NGLBookData.GetPendingExpiredOrders365(c.Control)
                If Not oExpiredLoads Is Nothing AndAlso oExpiredLoads.Count > 0 Then
                    If oExpiredLoads(0).ErrNumber.HasValue AndAlso oExpiredLoads(0).ErrNumber <> 0 Then
                        results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_ProcessExpiredLoadsError, oExpiredLoads(0).ErrNumber.ToString(), oExpiredLoads(0).RetMsg, c.Control.ToString())
                        results.Success = False
                    Else
                        For Each l In oExpiredLoads
                            If Not ProcessedOrders.Contains(l.BookControl) Then
                                '1. Get the Booking records
                                results.AddLog("Read the booking records")
                                Dim oBookRevs = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(l.BookControl)
                                'Added by LVV 7/20/16 for v-7.0.5.110 DAT
                                Dim eAcceptRejectMode = AcceptRejectModeEnum.System
                                'If oBookRevs(0).BookRevLoadTenderTypeControl = LTTypeEnum.DAT Then
                                '    eAcceptRejectMode = AcceptRejectModeEnum.DAT
                                'End If
                                If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                                    results.Success = False
                                    results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_ProcessExpiredLoadsErrorNoPro, l.BookProNumber)
                                Else
                                    Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = l.BookControl).FirstOrDefault()
                                    results.Key = l.BookControl
                                    results.AlphaCode = oSelectedBooking.BookProNumber
                                    results.updateKeyFields("BookConsPrefix", oSelectedBooking.BookConsPrefix)
                                    results.AddLog("Reset load to N Status")
                                    'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes
                                    'Removed call to b.ResetToNStatus() in For Each because this wipes out the Carrier info so no emails get sent in AcceptOrRejectLoad
                                    'Also, AcceptOrRejectLoad calls ResetToNStatus for Expired loads anyway
                                    For Each b In oBookRevs
                                        If Not ProcessedOrders.Contains(b.BookControl) Then ProcessedOrders.Add(b.BookControl)
                                    Next
                                    AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Expired, SendEmail, "Tendered Load Has Expired", 0, l.ExpiredLoadsTo, l.ExpiredLoadsCc, eAcceptRejectMode, "ProcessExpiredLoads", results, intValidationFlags)
                                End If
                            End If
                        Next
                    End If
                End If
            Next
        Catch ex As Exception
            'do nothing apparently
        End Try
        Return results
    End Function



    ''' <summary>
    ''' updates the drop load table with information about Expired or Rejected loads for each order on the load
    ''' </summary>
    ''' <param name="AcceptRejectCode"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="UserName"></param>
    ''' <param name="BookTrackComment"></param>
    ''' <param name="oEmailLocalization"></param>
    ''' <param name="oBookRevs"></param>
    ''' <param name="oRet"></param>
    ''' <remarks>
    ''' Drop Load Reasons:
    ''' The reason code will use the BookTrackComment if provided or the email subject 
    ''' the caller should format the messages appropriately:
    ''' PC to N Manually Rejected or System Expired
    ''' PB to N Drop Load 
    ''' </remarks>
    Private Sub updateDropLoad(ByVal AcceptRejectCode As AcceptRejectEnum,
                               ByVal CarrierNumber As Integer,
                               ByVal UserName As String,
                               ByVal BookTrackComment As String,
                               ByRef oEmailLocalization As DTO.EmailLocalizationData,
                               ByRef oBookRevs As DTO.BookRevenue(),
                               ByRef oRet As DTO.WCFResults)
        If oRet Is Nothing Then oRet = New DTO.WCFResults()
        Dim strDropReason = Left(If(String.IsNullOrWhiteSpace(BookTrackComment), oEmailLocalization.getFormattedSubject(), BookTrackComment), 255)
        Dim strDropReasonLocalized = ""
        Dim strDropReasonKeys = ""
        'oEmailLocalization may not have been created so we test for null
        If Not oEmailLocalization Is Nothing Then
            strDropReasonLocalized = oEmailLocalization.SubjectLocalized
            strDropReasonKeys = oEmailLocalization.SubjectKeys
        End If
        If AcceptRejectCode = AcceptRejectEnum.Expired Or AcceptRejectCode = AcceptRejectEnum.Rejected Or AcceptRejectCode = AcceptRejectEnum.Dropped Then
            For Each b In oBookRevs
                If Not b Is Nothing AndAlso b.BookControl <> 0 Then
                    Dim oDropLoad As New DTO.CarrierDropLoad With {.CarrierDropNumber = CarrierNumber,
                                                                   .CarrierDropContact = UserName,
                                                                   .CarrierDropProNumber = b.BookProNumber,
                                                                   .CarrierDropReason = strDropReason,
                                                                   .CarrierDropDate = Date.Now,
                                                                   .CarrierDropTime = Date.Now,
                                                                   .CarrierDropReasonLocalized = strDropReasonLocalized,
                                                                   .CarrierDropReasonKeys = strDropReasonKeys,
                                                                   .TrackingState = Core.ChangeTracker.TrackingInfo.Created}
                    Try
                        NGLCarrierDropLoadData.CreateRecord(oDropLoad)
                    Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
                        oRet.AddFaultException(FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_SQLFaultCreateDropLoadRecordFailure, sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details, sqlEx.Detail.DetailsList)
                    Catch ex As Exception
                        Throw 'throw any unhandeled exception
                    End Try
                End If
            Next
        End If
    End Sub

    Private Function prepareCompanyContactEmail(ByVal NotificationEMailAddress As String,
                                ByVal NotificationEMailAddressCc As String,
                                ByRef vData As LTS.spGetAutoTenderDataResult,
                                ByRef oRet As DTO.WCFResults) As String

        If oRet Is Nothing Then oRet = New DTO.WCFResults()
        Dim CompContEmail As String = ""
        If Not vData Is Nothing Then CompContEmail = vData.CompContEmails
        oRet.updateKeyFields("CompContEmail", CompContEmail)
        oRet.updateKeyFields("NotificationEMailAddress", NotificationEMailAddress)
        oRet.updateKeyFields("NotificationEMailAddressCc", NotificationEMailAddressCc)
        If Not NGLCoreComm.Email.canBuildEmailAddressCollection(CompContEmail) Then
            oRet.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_AcceptRejectExpire_InvalidCompContEmail)
            CompContEmail = ""
            Dim strSep As String = ""
            Dim strEmail As String = ""
            If Not String.IsNullOrEmpty(NotificationEMailAddress) AndAlso NotificationEMailAddress.Trim.Length > 3 Then
                strEmail = NotificationEMailAddress
                strSep = "; "
            End If
            If Not String.IsNullOrEmpty(NotificationEMailAddressCc) AndAlso NotificationEMailAddressCc.Trim.Length > 3 Then
                strEmail = String.Concat(strEmail, strSep, NotificationEMailAddressCc)
            End If
            If NGLCoreComm.Email.canBuildEmailAddressCollection(strEmail) Then
                CompContEmail = strEmail
            Else
                oRet.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_AcceptRejectExpire_InvalidNotificationEmail)
            End If
        End If


        Return CompContEmail
    End Function

    ''' <summary>
    ''' Creates a new EmailLocalizationData object and prepares the subject and body text for Localized and non-Localized formats.
    ''' If the clsAcceptRejectCodeInfo has an InvalidAcceptRejectCode the method returns nothing.  Caller must test for a null object returned.
    ''' </summary>
    ''' <param name="sBookSHID"></param>
    ''' <param name="sBookConsPrefix"></param>
    ''' <param name="BookTrackComment"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="CarrierName"></param>
    ''' <param name="oARejInfo"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Localization is possible by storeing the key fields in the database as  a csv string. 
    ''' To implement this logic the Mail Server program needs to be modified to support localization logic
    ''' </remarks>
    Private Function prepareEmailLoacalization(ByVal sBookSHID As String,
                                               ByVal sBookConsPrefix As String,
                                               ByVal BookTrackComment As String,
                                               ByVal CarrierNumber As Integer?,
                                               ByVal CarrierName As String,
                                               ByRef oARejInfo As clsAcceptRejectCodeInfo) As DTO.EmailLocalizationData
        If oARejInfo.InvalidAcceptRejectCode Then Return Nothing

        Dim oEmailLocalization As New DTO.EmailLocalizationData(oARejInfo.EmailLocalizationType)
        '* Sample Code for using BodyKeys *'
        '*  Dim strTest As String = "A load with Pro Number {0} was accepted by Carrier {1} -- {2} <br><br>Carrier Comments:<br>{3}"
        '*  Dim lKeys As New List(Of String) From {"PRO12345", "1224", "Test Carrier", "My Comments Are ..."}
        '*  Dim strFormated = String.Format(strTest, lKeys.ToArray())
        '*  List of Keys are stored in the database as CSV data 
        '*  Dim strCSV = String.Join(",", lKeys)
        '*  Dim strCSVFormated = String.Format(strTest, strCSV.Split(",").ToArray())

        '* NOTE: List items must be added in the correct order for the formatted string data 
        Dim shid As String = If(String.IsNullOrWhiteSpace(sBookSHID), "No SHID", sBookSHID)
        Dim cns As String = If(String.IsNullOrWhiteSpace(sBookConsPrefix), "No CNS", sBookConsPrefix)
        Dim loadNumberStr As String = shid & " " & cns
        oEmailLocalization.SubjectKeyList.Add(shid)
        oEmailLocalization.SubjectKeyList.Add(cns)
        oEmailLocalization.SubjectKeyList.Add(If(CarrierNumber, CarrierNumber.ToString(), "[Invalid Carrier Number]"))
        oEmailLocalization.BodyKeyList.Add(shid)
        oEmailLocalization.BodyKeyList.Add(cns)
        oEmailLocalization.BodyKeyList.Add(If(CarrierNumber, CarrierNumber.ToString(), "[Invalid Carrier Number]"))
        oEmailLocalization.BodyKeyList.Add(If(String.IsNullOrWhiteSpace(CarrierName), "No Carrier Name", CarrierName))
        oEmailLocalization.BodyKeyList.Add(If(String.IsNullOrWhiteSpace(BookTrackComment), "No Comments", BookTrackComment))
        'Create the CSV data from the lists
        oEmailLocalization.fillKeysFromLists()
        Return oEmailLocalization
    End Function

#End Region

#Region " Private Methods"



    Private Function returnUpdatedBookLoadData() As DTO.BookLoad
        If Not NGLBookLoadData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookLoadData.BookDependencyResult.BookControl, 0),
                                     If(NGLBookLoadData.BookDependencyResult.ErrNumber, 0),
                                     NGLBookLoadData.BookDependencyResult.RetMsg,
                                     NGLBookLoadData.LastProcedureName,
                                     If(NGLBookLoadData.BookDependencyResult.MustRecalculate, False))

        Return NGLBookLoadData.GetBookLoadFiltered(NGLBookLoadData.BookDependencyResult.BookLoadControl)

    End Function

    Private Function returnUpdatedBookLoadDataQuick() As DTO.QuickSaveResults
        If Not NGLBookLoadData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookLoadData.BookDependencyResult.BookControl, 0),
                                      If(NGLBookLoadData.BookDependencyResult.ErrNumber, 0),
                                      NGLBookLoadData.BookDependencyResult.RetMsg,
                                      NGLBookLoadData.LastProcedureName,
                                      If(NGLBookLoadData.BookDependencyResult.MustRecalculate, False))

        Return NGLBookLoadData.QuickSaveResults(NGLBookLoadData.BookDependencyResult.BookLoadControl)
    End Function

    Private Function returnUpdatedBookItemData(ByVal BookItemControl As Integer) As DTO.BookItem
        If Not NGLBookItemData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookItemData.BookDependencyResult.BookControl, 0),
                                     If(NGLBookItemData.BookDependencyResult.ErrNumber, 0),
                                     NGLBookItemData.BookDependencyResult.RetMsg,
                                     NGLBookItemData.LastProcedureName,
                                     If(NGLBookItemData.BookDependencyResult.MustRecalculate, False))

        Return NGLBookItemData.GetBookItemFiltered(BookItemControl)

    End Function

    Private Function returnUpdatedBookItemDataQuick(ByVal BookItemControl As Integer) As DTO.QuickSaveResults
        processRecalculateParameters(If(NGLBookItemData.BookDependencyResult.BookControl, 0),
                                     If(NGLBookItemData.BookDependencyResult.ErrNumber, 0),
                                     NGLBookItemData.BookDependencyResult.RetMsg,
                                     NGLBookItemData.LastProcedureName,
                                     If(NGLBookItemData.BookDependencyResult.MustRecalculate, False))

        Return NGLBookItemData.QuickSaveResults(BookItemControl)
    End Function

    Private Function returnCalculatedBookingData(Optional ByVal blnWithDetails As Boolean = False) As DTO.Book

        Dim resultBook As DTO.Book = Nothing
        Using Logger.StartActivity("returnCalclatdBookingData(blnWithDetails: {blnWithDetails}", blnWithDetails)
            If Not NGLBookData.BookDependencyResult Is Nothing Then
                processRecalculateParameters(
                    If(NGLBookData.BookDependencyResult.BookControl, 0),
                              If(NGLBookData.BookDependencyResult.ErrNumber, 0),
                                         NGLBookData.BookDependencyResult.RetMsg,
                                         NGLBookData.LastProcedureName,
                                         If(NGLBookData.BookDependencyResult.MustRecalculate, False))
            End If

            If blnWithDetails Then
                resultBook = NGLBookData.GetBookFiltered(If(NGLBookData.BookDependencyResult.BookControl, 0))
            Else
                resultBook = NGLBookData.GetBookFilteredNoChildren(NGLBookData.BookDependencyResult.BookControl)
            End If
        End Using
        Return resultBook
    End Function

    Private Function returnCalculatedBookingDataQuick() As DTO.QuickSaveResults
        processRecalculateParameters(If(NGLBookData.BookDependencyResult.BookControl, 0),
                                     If(NGLBookData.BookDependencyResult.ErrNumber, 0),
                                     NGLBookData.BookDependencyResult.RetMsg,
                                     NGLBookData.LastProcedureName,
                                     If(NGLBookData.BookDependencyResult.MustRecalculate, False))
        Return NGLBookData.QuickSaveResults(NGLBookData.BookDependencyResult.BookControl)
    End Function

    Private Function returnCalculatedvBookConsData() As DTO.vBookCons
        If Not NGLvBookConsData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLvBookConsData.BookDependencyResult.BookControl, 0),
                                     If(NGLvBookConsData.BookDependencyResult.ErrNumber, 0),
                                     NGLvBookConsData.BookDependencyResult.RetMsg,
                                     NGLvBookConsData.LastProcedureName,
                                     If(NGLvBookConsData.BookDependencyResult.MustRecalculate, False))

        Return NGLvBookConsData.GetvBookConsRecord(If(NGLvBookConsData.BookDependencyResult.BookControl, 0))

    End Function

    Private Function returnCalculatedvBookConsDataQuick() As DTO.QuickSaveResults
        processRecalculateParameters(If(NGLvBookConsData.BookDependencyResult.BookControl, 0),
                                     If(NGLvBookConsData.BookDependencyResult.ErrNumber, 0),
                                     NGLvBookConsData.BookDependencyResult.RetMsg,
                                     NGLvBookConsData.LastProcedureName,
                                     If(NGLvBookConsData.BookDependencyResult.MustRecalculate, False))
        Return NGLBookData.QuickSaveResults(If(NGLvBookConsData.BookDependencyResult.BookControl, 0))
    End Function

    Private Sub processRecalculateParameters(ByVal BookControl As Integer, ByVal ErrNumber As Integer, ByVal RetMsg As String, ByVal Procedure As String, ByVal MustRecalculate As Boolean)
        Using operation = Logger.StartActivity("processRecalculateParameters(BookControl: {BookControl}, ErrNumber: {ErrNumber}, RetMsg: {RetMsg}, Procedure: {Procedure}, MustRecalculate: {MustRecalculate}", BookControl, ErrNumber, RetMsg, Procedure, MustRecalculate)
            If ErrNumber <> 0 Then
                operation.Complete()
                throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {Procedure, ErrNumber, RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)

            ElseIf MustRecalculate Then
                
                BookRevenueBLL.AssignCarrier(BookControl, True)
            End If
        End Using
    End Sub

    Private Sub processRecalculateParameters(ByRef BookDependencyResult As LTS.spUpdateBookDependenciesResult, ByVal Procedure As String)
        If Not BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(BookDependencyResult.BookControl, 0),
                                     If(BookDependencyResult.ErrNumber, 0),
                                     BookDependencyResult.RetMsg,
                                     Procedure,
                                     If(BookDependencyResult.MustRecalculate, False))
    End Sub

    ''' <summary>
    ''' Returns the correct Load Tender Type Enum based on the Accept Reject Mode
    ''' </summary>
    ''' <param name="AcceptRejectMode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 7/11/2016
    ''' </remarks>
    Private Function getLoadTenderTypeEnum(ByVal AcceptRejectMode As AcceptRejectModeEnum) As DTO.tblLoadTender.LoadTenderTypeEnum

        Select Case AcceptRejectMode
            Case AcceptRejectModeEnum.EDI
                Return DTO.tblLoadTender.LoadTenderTypeEnum.EDI204
            Case AcceptRejectModeEnum.WEB
                Return DTO.tblLoadTender.LoadTenderTypeEnum.Web
            Case AcceptRejectModeEnum.DAT
                Return DTO.tblLoadTender.LoadTenderTypeEnum.DAT
                'Todo add logic to support the following if needed
                'Case AcceptRejectModeEnum.System
                'Case AcceptRejectModeEnum.Token
            Case Else
                Return DTO.tblLoadTender.LoadTenderTypeEnum.Manual
        End Select


    End Function

#Region " New DAT Methods Added by RHR:  may want to move to NGLDATBLL library?"

    'Private Sub ProcessDATStatusUpdates(ByVal AcceptRejectMode As AcceptRejectModeEnum, _
    '                                    ByRef results As DTO.WCFResults, _
    '                                    ByRef datRes As DTO.DATResults(),
    '                                    ByVal AcceptRejectCode As AcceptRejectEnum, _
    '                                    ByRef wcfRes As DTO.WCFResults, _
    '                                    ByVal blnDATDeleteSuccess As Boolean, _
    '                                    ByVal blnDATPostingSuccess As Boolean, _
    '                                    ByVal blnBookRevSaveComplete As Boolean)
    '    Select Case AcceptRejectMode
    '        Case AcceptRejectModeEnum.DAT
    '            Select Case AcceptRejectCode
    '                Case AcceptRejectEnum.Tendered

    '                Case AcceptRejectEnum.Expired
    '                Case AcceptRejectEnum.Rejected, AcceptRejectEnum.Dropped
    '            End Select
    '            'status updates and alert for fail
    '            If Not datRes Is Nothing AndAlso datRes.Count > 0 Then
    '                For Each d In datRes
    '                    If d.Success Then
    '                        'The Delete succeeded so we still have to update the ls messages etc.
    '                        Dim uname As String = d.UserName
    '                        If AcceptRejectCode = AcceptRejectEnum.Expired Then uname = "System"

    '                        Dim p() As String = {d.LTBookSHID, d.LTCarrierNumber.ToString(), d.LTCarrierName}
    '                        d.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATDeleteStillTenderedToCarrier, p)

    '                        Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_DATDeleteStillTenderedToCarrier), p)
    '                        NGLSystemLogData.AddApplicaitonLog(msg, source)

    '                        If AcceptRejectCode = AcceptRejectEnum.Expired Then
    '                            NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, "DAT: Post Expired Ref ID: " + d.LTDATRefID, 502, "DAT", DateTime.Now, "", "")
    '                        End If
    '                        NGLLoadTenderData.updateLoadTender(d.LTControl, uname, d.LTMessage, d.LTStatusCode)
    '                        NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, d.BookTrackComment, d.BookTrackStatus, "DAT", DateTime.Now, "", "")
    '                        If AcceptRejectCode = AcceptRejectEnum.Expired Then
    '                            NGLLoadTenderData.updateLoadTender(d.LTControl, uname, d.LTMessage, StatusCode:=DTO.tblLoadTender.LoadTenderStatusCodeEnum.PostExpired, Archived:=True, Expired:=True)
    '                            NGLDATBLL.createDATErrorSubscriptionAlert(d, "Alert: System failed to properly process expired DAT load", msg, source)
    '                        End If
    '                        'Error Load Status Note
    '                        NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, msg, 503, "DAT", DateTime.Now, "", "")
    '                        If Not d.Warnings Is Nothing AndAlso d.Warnings.Count > 0 Then
    '                            results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, d.Warnings)
    '                        End If
    '                    Else
    '                        'If the Save failed and the DAT delete also failed nothing needs to happen because both things are still true
    '                        'The load is still tendered to DAT Carrier and the Posting for that load is still on the DAT Load Board
    '                        'Still create an alert that says this load is expired but there was a problem deleting and resetting to N Status
    '                        Dim eMsg As DTO.DATResults.MessageEnum
    '                        If AcceptRejectCode = AcceptRejectEnum.Expired Then
    '                            eMsg = DTO.DATResults.MessageEnum.E_DATExpireFail
    '                        Else
    '                            eMsg = DTO.DATResults.MessageEnum.E_DATDeleteFail
    '                        End If
    '                        Dim p() As String = {d.LTBookSHID, d.LTDATRefID}
    '                        d.AddMessage(DTO.DATResults.MessageType.Warnings, eMsg, p)

    '                        Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(eMsg), p)
    '                        NGLSystemLogData.AddApplicaitonLog(msg, source)
    '                        If AcceptRejectCode = AcceptRejectEnum.Expired Then NGLDATBLL.createDATErrorSubscriptionAlert(d, "Alert: System failed to properly process expired DAT load", msg, source)
    '                        If Not d.Warnings Is Nothing AndAlso d.Warnings.Count > 0 Then
    '                            results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, d.Warnings)
    '                        End If
    '                    End If
    '                Next
    '            End If
    '            'this is where we do the updates to lt table andbook track
    '            For Each d In datRes
    '                If d.Success Then
    '                    Dim uname As String = d.UserName
    '                    If AcceptRejectCode = AcceptRejectEnum.Expired Then uname = "System"
    '                    If AcceptRejectCode = AcceptRejectEnum.Expired Then
    '                        NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, "DAT: Post Expired Ref ID: " + d.LTDATRefID, 502, "DAT", DateTime.Now, "", "")
    '                    End If
    '                    NGLLoadTenderData.updateLoadTender(d.LTControl, uname, d.LTMessage, d.LTStatusCode)
    '                    NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, d.BookTrackComment, d.BookTrackStatus, "DAT", DateTime.Now, "", "")
    '                    If AcceptRejectCode = AcceptRejectEnum.Expired Then
    '                        NGLLoadTenderData.updateLoadTender(d.LTControl, uname, d.LTMessage, StatusCode:=DTO.tblLoadTender.LoadTenderStatusCodeEnum.PostExpired, Archived:=True, Expired:=True)
    '                    End If

    '                End If
    '            Next
    '    End Select
    'End Sub

    Private Sub DATRejectSaveFailedCode()
        'Catch ex As FaultException
        '    'status updates and alert for fail
        '    If Not datRes Is Nothing AndAlso datRes.Count > 0 Then
        '        For Each d In datRes
        '            If d.Success Then
        '                'The Delete succeeded so we still have to update the ls messages etc.
        '                NGLLoadTenderData.updateLoadTender(d.LTControl, "System", d.LTMessage, d.LTStatusCode)
        '                NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, d.BookTrackComment, d.BookTrackStatus, "DAT", DateTime.Now, "", "")
        '                'Error Load Status Note
        '                Dim msg = "DAT: Warning - The Load with SHID " + d.LTBookSHID + " is still tendered to Carrier " + d.LTCarrierNumber.ToString() + " " + d.LTCarrierName + " in TMS but is no longer Posted on the DAT Load Board."
        '                NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, msg, 503, "DAT", DateTime.Now, "", "")
        '                'Subscription Alert
        '                'NGLDATBLL.createDATErrorSubscriptionAlert(d, "Alert: System failed to properly delete DAT posting", msg, "NGL.FM.BLL.NGLBookBLL.AcceptOrRejectLoad")
        '                Dim p As String() = {msg}
        '                results.AddMessage(DTO.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_DATGeneralRetMsg, p)
        '                If Not d.Warnings Is Nothing AndAlso d.Warnings.Count > 0 Then
        '                    results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, d.Warnings)
        '                End If
        '            Else
        '                'If the Save failed and the DAT delete also failed nothing needs to happen because both things are still true
        '                'The load is still tendered to DAT Carrier and the Posting for that load is still on the DAT Load Board
        '                Dim msg = "DAT: Warning - The Load with SHID " + d.LTBookSHID + " could not be reset to N Status and the DAT Posting with Reference ID " + d.LTDATRefID + " could not be deleted from the DAT Load Board."
        '                'NGLDATBLL.createDATErrorSubscriptionAlert(d, "Alert: System failed to properly delete DAT posting", msg, "NGL.FM.BLL.NGLBookBLL.AcceptOrRejectLoad")
        '                Dim p As String() = {msg}
        '                results.AddMessage(DTO.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_DATGeneralRetMsg, p)
        '                If Not d.Warnings Is Nothing AndAlso d.Warnings.Count > 0 Then
        '                    results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, d.Warnings)
        '                End If
        '            End If
        '        Next
        '    End If
        '    'Throw
        'End Try
        ''this is where we do the updates to lt table andbook track
        'For Each d In datRes
        '    If d.Success Then
        '        'the first time is for the delete
        '        NGLLoadTenderData.updateLoadTender(d.LTControl, "System", d.LTMessage, d.LTStatusCode)
        '        NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, d.BookTrackComment, d.BookTrackStatus, "DAT", DateTime.Now, "", "")
        '    Else
        '        If Not d.Warnings Is Nothing AndAlso d.Warnings.Count > 0 Then
        '            results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, d.Warnings)
        '        End If
        '    End If
        'Next
        '            Else
        'For Each b In oBookRevs
        '    b.ResetToNStatus()
        'Next
        'results.AddLog("Save changes to booking records")
        'NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, True, True)
        '            End If
    End Sub

    Private Sub DATExpiredSaveFailedCode()
        'Select Case AcceptRejectMode
        '    Case AcceptRejectModeEnum.DAT
        '        'Added by LVV 7/1/16 for v-7.0.5.110 DAT
        '        Dim datRes = NGLDATBLL.DeleteDATPosting(oBookRevs, AcceptRejectEnum.Expired)
        '        Try
        '            results.AddLog("Save changes to booking records")
        '            NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, True, True)
        '        Catch ex As FaultException
        '            'status updates and alert for fail
        '            If Not datRes Is Nothing AndAlso datRes.Count > 0 Then
        '                For Each d In datRes
        '                    If d.Success Then
        '                        'The Delete succeeded so we still have to update the ls messages etc.
        '                        'the first time is for the delete
        '                        NGLLoadTenderData.updateLoadTender(d.LTControl, "System", d.LTMessage, d.LTStatusCode)
        '                        NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, d.BookTrackComment, d.BookTrackStatus, "DAT", DateTime.Now, "", "")
        '                        'the second time is for the expiration
        '                        NGLLoadTenderData.updateLoadTender(d.LTControl, "System", d.LTMessage, StatusCode:=DTO.tblLoadTender.LoadTenderStatusCodeEnum.PostExpired, Archived:=True, Expired:=True)
        '                        NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, "DAT: Post Expired Ref ID: " + d.LTDATRefID, 502, "DAT", DateTime.Now, "", "")
        '                        'Error Load Status Note
        '                        Dim msg = "DAT: Warning - The Load with SHID " + d.LTBookSHID + " is still tendered to Carrier " + d.LTCarrierNumber.ToString() + " " + d.LTCarrierName + " in TMS but is no longer Posted on the DAT Load Board."
        '                        NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, msg, 503, "DAT", DateTime.Now, "", "")
        '                        'Subscription Alert
        '                        NGLDATBLL.createDATErrorSubscriptionAlert(d, "Alert: System failed to properly process expired DAT load", msg, "NGL.FM.BLL.NGLBookBLL.AcceptOrRejectLoad")
        '                    Else
        '                        'If the Save failed and the DAT delete also failed nothing needs to happen because both things are still true
        '                        'The load is still tendered to DAT Carrier and the Posting for that load is still on the DAT Load Board
        '                        'Still create an alert that says this load is expired but there was a problem deleting and resetting to N Status
        '                        Dim msg = "DAT: Warning - The Load with SHID " + d.LTBookSHID + " is expired but could not be reset to N Status as there was a problem with the Save. The DAT Posting with Reference ID " + d.LTDATRefID + " could not be deleted from the DAT Load Board."
        '                        NGLDATBLL.createDATErrorSubscriptionAlert(d, "Alert: System failed to properly process expired DAT load", msg, "NGL.FM.BLL.NGLBookBLL.AcceptOrRejectLoad")
        '                    End If
        '                Next
        '            End If
        '            Throw
        '        End Try
        '        'this is where we do the updates to lt table andbook track
        '        For Each d In datRes
        '            If d.Success Then
        '                'the first time is for the delete
        '                NGLLoadTenderData.updateLoadTender(d.LTControl, "System", d.LTMessage, d.LTStatusCode)
        '                NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, d.BookTrackComment, d.BookTrackStatus, "DAT", DateTime.Now, "", "")
        '                'the second time is for the expiration
        '                NGLLoadTenderData.updateLoadTender(d.LTControl, "System", d.LTMessage, StatusCode:=DTO.tblLoadTender.LoadTenderStatusCodeEnum.PostExpired, Archived:=True, Expired:=True)
        '                NGLBookTrackData.UpdateBookTracksForLoad(d.LTBookControl, "DAT: Post Expired Ref ID: " + d.LTDATRefID, 502, "DAT", DateTime.Now, "", "")
        '            Else
        '                'the post is not actually deleted. do we care?
        '            End If
        '        Next
        '    Case Else
        '        results.AddLog("Save changes to booking records")
        '        NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, True, True)
        'End Select
    End Sub

    Private Sub OldDATTenderedCode()
        'At this point we want to always add a record to the new loadTender table with the correct loadTenderType
        'based on the acceptRejectModeType enum
        'Added by LVV on 5/18/16 for v-7.0.5.110 DAT
        'Select Case AcceptRejectMode
        '    Case AcceptRejectModeEnum.DAT
        '        results.AddLog("Execute sp to add DAT Post data to tblLoadTender")
        '        Dim wcfRes = NGLBookRevenueData.GetDATData(oSelectedBooking.BookControl)
        '        Dim LoadTenderControl As Integer = 0
        '        If Not wcfRes Is Nothing Then
        '            If Not wcfRes.Warnings Is Nothing AndAlso wcfRes.Warnings.Count > 0 Then
        '                'The sp failed so nothing can be posted so skip changing tran code and return to the caller to display the message to the user
        '                results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcfRes.Warnings)
        '                Return results
        '            End If
        '            'get the record back using Control and post to DAT
        '            If Not wcfRes.KeyFields Is Nothing AndAlso wcfRes.KeyFields.Count > 0 Then
        '                Integer.TryParse(wcfRes.KeyFields("LoadTenderControl"), LoadTenderControl)
        '            End If
        '        End If
        '        If LoadTenderControl = 0 Then Return results 'maybe add a message here. although if this fails it should be caught in the above message check
        '        Dim feature = Ngl.FM.DAT.Infrastructure.Feature.Post
        '        Dim lt = NGLDATBLL.getAdditionalDATLTInfo(LoadTenderControl, feature, UserName)
        '        Dim datRes = DAT.DAT.processData(lt)

        '        If datRes.Success Then
        '            'If the post worked then we can attempt to make the save
        '            results.AddLog("Save changes to booking records")
        '            Try
        '                NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
        '            Catch ex As FaultException
        '                'TODO If the save failed but the post was successful then idk figure out what to do and messages
        '                If datRes.Success Then

        '                End If
        '                Throw
        '            End Try
        '            NGLLoadTenderData.updateDATPostResults(lt.LoadTenderControl, datRes.LTDATRefID, datRes.UserName)
        '            NGLBookTrackData.UpdateBookTracksForLoad(lt.LTBookControl, "Load Posted to DAT. Reference ID " + datRes.LTDATRefID, 500, "DAT", DateTime.Now, "", "")
        '        Else
        '            'TODO probably add a message that the post and update both failed or something
        '            'NGLLoadTenderData.updateDATPostResultsError(lt.LoadTenderControl, datRes.LTMessage, datRes.UserName)
        '            NGLLoadTenderData.updateLoadTender(lt.LoadTenderControl, UserName:=datRes.UserName, Message:=datRes.LTMessage, StatusCode:=datRes.LTStatusCode)
        '            If Not datRes.Warnings Is Nothing AndAlso datRes.Warnings.Count > 0 Then
        '                results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
        '                Return results
        '            End If

        '        End If

        '    Case AcceptRejectModeEnum.MANUAL
        '        results.AddLog("Save changes to booking records")
        '        NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
        'End Select
    End Sub
#End Region

#Region "      AutoTender Methods"

    ''' <summary>
    ''' Caller must validate that this procedure is allowed (Can auto tender).
    ''' Caller must assign the correct carrier to the selected bookcontrol record.
    ''' Caller must validate any cascading dispatching rules.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="results"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function AutoTender(ByVal BookControl As Integer, ByRef results As DTO.WCFResults, Optional ByVal blnSendEmail As Boolean = True) As Boolean
        Dim blnRet As Boolean = True
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}
        Try
            'Get the Booking Records For this Load
            Dim oBookRevs = NGLBookRevenueData.GetBookRevenues(BookControl)
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 OrElse oBookRevs(0).BookControl = 0 Then
                results.Success = False
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_NoOrdersFound)
                Return False
            End If
            Dim strCarrierID As String = "[Carrier Not Assigned]"
            If oBookRevs(0).BookCarrierControl <> 0 Then
                Dim odic = NGLCarrierData.getCarrierNameNumber(oBookRevs(0).BookCarrierControl)
                If Not odic Is Nothing AndAlso odic.Count > 0 AndAlso odic.ContainsKey("CarrierName") Then
                    strCarrierID = odic("CarrierName")
                Else
                    strCarrierID = " Carrier Control Number " & oBookRevs(0).BookCarrierControl.ToString
                End If
            End If
            Return AutoTender(BookControl, oBookRevs, results, strCarrierID, blnSendEmail)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AutoTender"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' Caller must validate that this procedure is allowed (Can auto tender).
    ''' Caller must assign the correct carrier to the selected bookcontrol record.
    ''' Caller must validate any cascading dispatching rules.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="oBookRevs"></param>
    ''' <param name="results"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function AutoTender(ByVal BookControl As Integer, ByRef oBookRevs As DTO.BookRevenue(), ByRef results As DTO.WCFResults, ByVal strCarrierID As String, Optional ByVal blnSendEmail As Boolean = True) As Boolean
        Dim blnRet As Boolean = True
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.Success = False
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_NoOrdersFound)
            Return False
        End If
        Try
            Dim blnDataDirty As Boolean = False
            If Not BookRevenueBLL.updateBookSHID(oBookRevs.ToList(), blnDataDirty, False) Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotTenderLoadUpdateSHIDFailed)
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowWarnings)
                results.Success = False
                Return False
            End If
            Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            results.AlphaCode = oSelectedBooking.BookProNumber

            results.updateKeyFields("BookConsPrefix", oSelectedBooking.BookConsPrefix)
            results.updateKeyFields("BookSHID", oSelectedBooking.BookSHID)
            Dim strBookTrackComment As String = " System Auto Tender Load"

            'call accept reject            
            results = AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Tendered, blnSendEmail, strBookTrackComment, 0, "", "", AcceptRejectModeEnum.System, "System", results)
            If results.Success Then
                Dim oBookCarrTends As New List(Of LTS.BookCarrTend)
                For Each b In oBookRevs
                    oBookCarrTends.Add(New LTS.BookCarrTend() With {.BookCarrTendBookControl = b.BookControl, .BookCarrTendCarrierControl = b.BookCarrierControl, .BookCarrTendDate = Date.Now()})
                Next

                If Not NGLBookData.AddBookCarrTends(oBookCarrTends) Then
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAddAutoTenderLogData, strCarrierID)
                End If
            Else
                Return False
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AutoTender"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' Caller must validate that this procedure is allowed (Can auto accept (finalize)).
    ''' Caller must assign the correct carrier to the selected bookcontrol record.
    ''' Caller must validate any cascading dispatching rules or carrier acceptance rules.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="results"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function AutoAccept(ByVal BookControl As Integer, ByRef results As DTO.WCFResults, Optional ByVal blnSendEmail As Boolean = True) As Boolean
        Dim blnRet As Boolean = True
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}
        Try
            'Get the Booking Records For this Load
            Dim oBookRevs = NGLBookRevenueData.GetBookRevenues(BookControl)
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 OrElse oBookRevs(0).BookControl = 0 Then
                results.Success = False
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_NoOrdersFound)
                Return False
            End If
            Dim strCarrierID As String = "[Carrier Not Assigned]"
            If oBookRevs(0).BookCarrierControl <> 0 Then
                Dim odic = NGLCarrierData.getCarrierNameNumber(oBookRevs(0).BookCarrierControl)
                If Not odic Is Nothing AndAlso odic.Count > 0 AndAlso odic.ContainsKey("CarrierName") Then
                    strCarrierID = odic("CarrierName")
                Else
                    strCarrierID = " Carrier Control Number " & oBookRevs(0).BookCarrierControl.ToString
                End If
            End If
            Return AutoAccept(BookControl, oBookRevs, results, strCarrierID, blnSendEmail)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AutoTender"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' Caller must validate that this procedure is allowed (Can auto accept (finalize)).
    ''' Caller must assign the correct carrier to the selected bookcontrol record.
    ''' Caller must validate any cascading dispatching rules or carrier acceptance rules.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="oBookRevs"></param>
    ''' <param name="results"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function AutoAccept(ByVal BookControl As Integer, ByRef oBookRevs As DTO.BookRevenue(), ByRef results As DTO.WCFResults, ByVal strCarrierID As String, Optional ByVal blnSendEmail As Boolean = True) As Boolean
        Dim blnRet As Boolean = True
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.Success = False
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_NoOrdersFound)
            Return False
        End If
        Try
            Dim blnDataDirty As Boolean = False
            If Not BookRevenueBLL.updateBookSHID(oBookRevs.ToList(), blnDataDirty, False) Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotTenderLoadUpdateSHIDFailed)
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowWarnings)
                results.Success = False
                Return False
            End If
            Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
            results.AlphaCode = oSelectedBooking.BookProNumber
            results.updateKeyFields("BookConsPrefix", oSelectedBooking.BookConsPrefix)
            results.updateKeyFields("BookSHID", oSelectedBooking.BookSHID)
            Dim strBookTrackComment As String = " System Auto Accept Load"
            'TODO: find out how to check for System Auto Accept messages?
            'call accept reject

            Dim oFlags As New List(Of DTO.WCFResults.ValidationBits) From {DTO.WCFResults.ValidationBits.DoFinalize}
            Dim intValidationFlags As Int64 = results.getPreSetValidationFlags(oFlags)
            'TODO: update the validation flags so we do not get prompted to finalize the load.
            results = AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, AcceptRejectEnum.Accepted, blnSendEmail, strBookTrackComment, 0, "", "", AcceptRejectModeEnum.System, "System", results, intValidationFlags, "PB")
            'TODO: deal with finalization validation rules
            If results.Success Then
                Dim oBookCarrTends As New List(Of LTS.BookCarrTend)
                For Each b In oBookRevs
                    oBookCarrTends.Add(New LTS.BookCarrTend() With {.BookCarrTendBookControl = b.BookControl, .BookCarrTendCarrierControl = b.BookCarrierControl, .BookCarrTendDate = Date.Now()})
                Next
                If Not NGLBookData.AddBookCarrTends(oBookCarrTends) Then
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAddAutoAcceptLogData, strCarrierID)
                End If
            Else
                Return False
            End If

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("AutoAccept"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try

        Return blnRet
    End Function

    ''' <summary>
    ''' Determines if a load can be auto tendered. The caller is responsible for logging problems, sending emails and processing messages added to the results object.
    ''' The procedure updates the results object success flag but also returns true or false. 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="oBookRevs"></param>
    ''' <param name="vData"></param>
    ''' <param name="results"></param>
    ''' <returns>
    ''' returns true or false but results are updated as needed
    ''' DTO.WCFResults Success must be managed by the caller
    ''' Additional information is added to the Warnings, Errors, or Messages lists as needed 
    ''' </returns>
    ''' <remarks>
    ''' Rules:
    ''' 1. oBookRevs cannot be empty
    ''' 2. vData.AutoTender must be 1 (global AutoTender parameter)
    ''' 3. vData.LaneAutoTenderFlag must be true (all lanes for SHID must have LaneAutoTenderFlag checked)
    ''' 4. verify that the bookcontrol exists in oBookRevs
    ''' 5. Test BookTranCode must be N, P or PC
    ''' 6. Check that a carrier has been assigned and that the costs are not zero or that ignore tariff is checked for carrier
    ''' 7. Validate single Pickup location (New to 7.0 we allow for auto tender of consolidated loads if same pickup and stop location.)
    ''' 8. Validate single stop location (New to 7.0 we allow for auto tender of consolidated loads if same pickup and stop location.)
    ''' 9. Validate full truck load (New to 7.0 we allow for auto tender of consolidated loads if same pickup and stop location.)
    ''' </remarks>
    Friend Function CanAutoTender(ByVal BookControl As Integer, ByRef oBookRevs As DTO.BookRevenue(), ByRef vData As LTS.spGetAutoTenderDataResult, ByRef results As DTO.WCFResults) As Boolean

        Dim blnRet As Boolean = True
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}

        results.AddLog("Validate Auto Tender Business Rules.")
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_NoOrdersFound)
            Return False
        End If
        If vData Is Nothing Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_NoAutoTenderData)
            Return False
        End If
        results.AddLog("Validate Auto Tender Parameter Settings.")
        If vData.AutoTender <> 1 Then
            If vData.AutoFinalize Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAcceptGlobalParameterIsOff)
            Else
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoTenderGlobalParameterIsOff)
            End If
            Return False
        End If
        results.AddLog("Validate Lane Auto Tender Settings.")
        If Not vData.LaneAutoTenderFlag Then
            If vData.AutoFinalize Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAcceptLaneAutoTenderIsOff)
            Else
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoTenderLaneAutoTenderIsOff)
            End If
            Return False
        End If
        Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
        If oSelectedBooking Is Nothing OrElse oSelectedBooking.BookControl = 0 Then
            'there is a problem with the selected booking control number so just use the first record in obookRevs
            oSelectedBooking = oBookRevs(0)
        End If
        Dim sGoodTransCodes As New List(Of String) From {"N", "P", "PC"}
        results.AddLog("Validate Auto Tender Trans Codes.")
        If oBookRevs.Any(Function(x) Not sGoodTransCodes.Contains(x.BookTranCode)) Then
            If vData.AutoFinalize Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAcceptInvalidTranCode)
            Else
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoTenderInvalidTranCode)
            End If
            Return False
        End If
        results.AddLog("Double check that a carrier is assigned and that the costs are not zero.")
        If Not vData.IgnoreTariff AndAlso (oBookRevs.Any(Function(x) x.BookCarrierControl = 0) OrElse oBookRevs.Any(Function(x) x.BookRevCarrierCost <= 0)) Then
            If vData.AutoFinalize Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAcceptInvalidCarrier)
            Else
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoTenderInvalidCarrier)
            End If
            Return False
        End If
        results.AddLog("Validate Single Pickup Location.")
        If oBookRevs.Any(Function(x) x.BookOrigAddress1 <> oSelectedBooking.BookOrigAddress1) Then
            If vData.AutoFinalize Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAcceptMultiPick)
            Else
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoTenderMultiPick)
            End If
            Return False
        End If
        results.AddLog("Validate Single Stop Location.")
        If oBookRevs.Any(Function(x) x.BookDestAddress1 <> oSelectedBooking.BookDestAddress1) Then
            If vData.AutoFinalize Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAcceptMultiStop)
            Else
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoTenderMultiStop)
            End If
            Return False
        End If
        results.AddLog("Validate Full Truck Load.")
        If Not ((vData.LaneTLCases = 0 OrElse oBookRevs.Sum(Function(x) x.BookTotalCases) >= vData.LaneTLCases) _
            And
            (vData.LaneTLWgt = 0 OrElse oBookRevs.Sum(Function(x) x.BookTotalWgt) >= vData.LaneTLWgt) _
            And
            (vData.LaneTLCube = 0 OrElse oBookRevs.Sum(Function(x) x.BookTotalCube) >= vData.LaneTLCube) _
            And
            (vData.LaneTLPL = 0 OrElse oBookRevs.Sum(Function(x) x.BookTotalPL) >= vData.LaneTLPL)) Then
            'this is not a truck load so we cannot auto tender
            If vData.AutoFinalize Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAcceptNotATruckLoad)
            Else
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoTenderNotATruckLoad)
            End If
            Return False
        End If
        results.AddLog("Can Auto Tender")
        Return blnRet
    End Function


    ''' <summary>
    ''' Determines if a load can be dispatched to the next lowest cost carrier.  
    ''' The caller is responsible for logging problems, sending emails, and processing messages added to the result object
    ''' The procedure updates the results object success flag but also returns true or false
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="oBookRevs"></param>
    ''' <param name="vData"></param>
    ''' <param name="results"></param>
    ''' <returns>
    ''' returns true or false but results are updated as needed
    ''' DTO.WCFResults Success is true or false
    ''' Additional information is added to the Warnings, Errors, or Messages lists as needed 
    ''' </returns>
    ''' <remarks>
    ''' Rules:
    ''' 1. oBookRevs cannot be empty
    ''' 2. AutoTenderCascadingDispatching must be 1 (Global Parameter)
    ''' 3. LaneCascadingDispatchingFlag must be checked for all Lanes
    ''' 4. AutoTenderCascadingDispatchingDepth must not have been reached (Global Parameter)
    ''' 5. verify that the bookcontrol exists in oBookRevs
    ''' 6. The caller must verify that the load has been rejected or expired by any method except manually from the FreightMaster Client (AcceptRejectModeEnum.MANUAL)
    ''' 6. Test BookTranCode must be N
    ''' 7. Validate single Pickup location (New to 7.0 we allow for auto tender of consolidated loads if same pickup and stop location.)
    ''' 8. Validate single stop location (New to 7.0 we allow for auto tender of consolidated loads if same pickup and stop location.)
    ''' 9. Validate full truck load (New to 7.0 we allow for auto tender of consolidated loads if same pickup and stop location.)
    ''' </remarks>
    Friend Function CanCascadeDispatch(ByVal BookControl As Integer, ByRef oBookRevs As DTO.BookRevenue(), ByVal NbrOfTenders As Integer, ByRef vData As LTS.spGetAutoTenderDataResult, ByRef results As DTO.WCFResults) As Boolean
        Dim blnRet As Boolean = True
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}

        results.AddLog("Validate Cascade Dispatching Business Rules.")
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_NoOrdersFound)
            Return False
        End If
        If vData Is Nothing Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_NoAutoTenderData)
            Return False
        End If
        results.AddLog("Validate Auto Tender Cascading Dispatching Parameter Settings.")
        If vData.AutoTenderCascadingDispatching <> 1 Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotCascadeDispatchGlobalParameterIsOff)
            Return False
        End If
        results.AddLog("Validate Lane Cascading Dispatching Settings.")
        If Not vData.LaneCascadingDispatchingFlag Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotCascadeDispatchLaneIsOff)
            Return False
        End If
        results.AddLog("Validate Cascading Dispatching Depth.")
        If NbrOfTenders >= vData.AutoTenderCascadingDispatchingDepth Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotCascadeDispatchDepthReached)
            Return False
        End If
        Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
        If oSelectedBooking Is Nothing OrElse oSelectedBooking.BookControl = 0 Then
            'there is a problem with the selected booking control number so just use the first record in obookRevs
            oSelectedBooking = oBookRevs(0)
        End If
        results.AddLog("Validate Cascading Dispatching Tran Code.")
        If oBookRevs.Any(Function(x) x.BookTranCode <> "N") Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotCascadeDispatchInvalidTranCode)
            Return False
        End If
        results.AddLog("Validate Single Pickup Location.")
        If oBookRevs.Any(Function(x) x.BookOrigAddress1 <> oSelectedBooking.BookOrigAddress1) Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotCascadeDispatchMultiPick)
            Return False
        End If
        results.AddLog("Validate Single Stop Location.")
        If oBookRevs.Any(Function(x) x.BookDestAddress1 <> oSelectedBooking.BookDestAddress1) Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotCascadeDispatchMultiStop)
            Return False
        End If
        results.AddLog("Validate Full Truck Load.")
        If Not ((vData.LaneTLCases = 0 OrElse oBookRevs.Sum(Function(x) x.BookTotalCases) >= vData.LaneTLCases) _
            And
            (vData.LaneTLWgt = 0 OrElse oBookRevs.Sum(Function(x) x.BookTotalWgt) >= vData.LaneTLWgt) _
            And
            (vData.LaneTLCube = 0 OrElse oBookRevs.Sum(Function(x) x.BookTotalCube) >= vData.LaneTLCube) _
            And
            (vData.LaneTLPL = 0 OrElse oBookRevs.Sum(Function(x) x.BookTotalPL) >= vData.LaneTLPL)) Then
            'this is not a truck load so we cannot cascade dispatch
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotCascadeDispatchNotATruckLoad)
            Return False
        End If
        results.AddLog("Can Cascade Dispatch")
        Return blnRet
    End Function

    Private Function ValidateCarrierBeforeFinalize(ByVal BookControl As Integer) As Boolean
        Dim oItem As DTO.DoFinalizeAlerts = NGLBatchProcessData.ValidateAlertsBeforeFinalize(BookControl)
        If Not oItem Is Nothing Then
            ''process the messages
            'Dim strMsg As String = ""
            'Dim strMsgSpacer As String = ""
            'With oItem
            '    If Not String.IsNullOrEmpty(.ContractExpiresMessage) AndAlso .ContractExpiresDate.HasValue Then
            '        strMsg &= PaneSettings.MainInterface.LocalizeString(.ContractExpiresMessage) & String.Format(PaneSettings.MainInterface.LocalizeString("MSGdoFinalizeContractExp"), .CarrierNumber.ToString, .ContractExpiresDate.Value.ToShortDateString)
            '        strMsgSpacer = vbCrLf & vbCrLf
            '    End If
            '    If Not String.IsNullOrEmpty(.InsuranceMessage) AndAlso .InsuranceDate.HasValue Then
            '        strMsg &= strMsgSpacer & PaneSettings.MainInterface.LocalizeString(.InsuranceMessage) & String.Format(PaneSettings.MainInterface.LocalizeString("MSGdoFinalizeInsuranceExp"), .CarrierNumber.ToString, .InsuranceDate.Value.ToShortDateString)
            '        strMsgSpacer = vbCrLf & vbCrLf
            '    End If
            '    If Not String.IsNullOrEmpty(.ExposuerPerShipmentMessage) Then
            '        strMsg &= strMsgSpacer & PaneSettings.MainInterface.LocalizeString(.ExposuerPerShipmentMessage) & String.Format(PaneSettings.MainInterface.LocalizeString("MSGdoFinalizePerShipExp"), .CarrierNumber.ToString, Format(.PerShipmentValue, "C"), Format(.PerShipmentExposure, "C"))
            '        strMsgSpacer = vbCrLf & vbCrLf
            '    End If
            '    If Not String.IsNullOrEmpty(.ExposureAllMessage) Then
            '        strMsg &= strMsgSpacer & PaneSettings.MainInterface.LocalizeString(.ExposureAllMessage) & String.Format(PaneSettings.MainInterface.LocalizeString("MSGdoFinalizeAllExp"), .CarrierNumber.ToString, Format(.TotalExposueValue, "C"), Format(.AllShipmentExposure, "C"))
            '    End If
            'End With
            'If Not String.IsNullOrEmpty(strMsg) AndAlso strMsg.Trim.Length > 3 Then
            '    If (New FMMessageBoxPrompt).ShowFMMessageBox(PaneSettings.MainInterface, _
            '                                               FMMessageBox.DialogTypes.OkCancel, _
            '                                               FMMessageBox.ImportanceLevels.Question, _
            '                                               PaneSettings.MainInterface.LocalizeString("MSGdoFinalizeAlertsFound") _
            '                                               & vbCrLf & vbCrLf _
            '                                               & strMsg, _
            '                                                False) = DialogResults.No Then Return False
            'End If
            Return True
        End If
        Return False
    End Function

    Private Function LogCarrierValidationMessages(ByRef messages As DTO.DoFinalizeAlerts) As Boolean
        Dim blnRet As Boolean = False
        Dim Results As Double = 0
        Dim ThisMsg As String = ""
        Dim ErrNumber As Integer = 0
        Dim EmailMsg As String = ""
        Dim BookNotesVisible5 As String = ""
        'ToDo: add additional language reference logic to these messages
        If (Not String.IsNullOrEmpty(messages.ExposuerPerShipmentMessage)) Then
            Results = Math.Round((messages.PerShipmentValue - messages.PerShipmentExposure), 2)
            'ExposuerPerShipmentMessage is a parameter and should not normally need locaization
            ThisMsg = messages.ExposuerPerShipmentMessage & " " & FormatCurrency(Results) _
                & " for carrier: " & messages.CarrierNumber
            EmailMsg = ThisMsg
            BookNotesVisible5 = "Warning! Per Shipment Exposure Exceeded"
            ErrNumber = 1
        End If


        If (Not String.IsNullOrEmpty(messages.ExposureAllMessage)) Then
            Results = Math.Round((messages.TotalExposueValue - messages.AllShipmentExposure), 2)
            'ExposureAllMessage is a parameter and should not normally need locaization
            ThisMsg = messages.ExposureAllMessage & " " & FormatCurrency(Results) _
                & " for carrier: " & messages.CarrierNumber
            If ErrNumber > 0 Then
                EmailMsg &= " also " & ThisMsg
                BookNotesVisible5 = " -- AND -- All Shipment Exposure Exceeded"
            Else
                EmailMsg = ThisMsg
                BookNotesVisible5 = "Warning! All Shipments Exposure Exceeded"
                ErrNumber = 1
            End If
        End If

        If (Not String.IsNullOrEmpty(messages.ContractExpiresMessage)) Then
            Results = Math.Round((messages.TotalExposueValue - messages.AllShipmentExposure), 2)
            'ContractExpiresMessage is a parameter and should not normally need locaization
            ThisMsg = messages.ContractExpiresMessage & " " & If(messages.ContractExpiresDate.HasValue, FormatDateTime(messages.ContractExpiresDate), "") _
                & " for carrier: " & messages.CarrierNumber
            If ErrNumber > 0 Then
                EmailMsg &= " also " & ThisMsg
                BookNotesVisible5 = " -- AND -- Contract Expired"
            Else
                EmailMsg = ThisMsg
                BookNotesVisible5 = "Warning! Contract Expired"
                ErrNumber = 1
            End If
        End If

        If (Not String.IsNullOrEmpty(messages.InsuranceMessage)) Then
            Results = Math.Round((messages.TotalExposueValue - messages.AllShipmentExposure), 2)
            'ContractExpiresMessage is a parameter and should not normally need locaization
            ThisMsg = messages.InsuranceMessage & " " & If(messages.InsuranceDate.HasValue, FormatDateTime(messages.InsuranceDate), "") _
                & " for carrier: " & messages.CarrierNumber
            If ErrNumber > 0 Then
                EmailMsg &= " also " & ThisMsg
                BookNotesVisible5 = " -- AND -- Insurance Expired"
            Else
                EmailMsg = ThisMsg
                BookNotesVisible5 = "Warning! Insurance Expired"
                ErrNumber = 1
            End If
        End If

        Return blnRet

    End Function

    ''' <summary>
    ''' Replaces the doFinalize procedure
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="BookTranCode"></param>
    ''' <param name="intValidationFlag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FinalizeBookings(ByRef oBooks As DTO.BookRevenue(),
                                      ByVal BookTranCode As String,
                                      Optional ByRef results As DTO.WCFResults = Nothing,
                                      Optional ByVal intValidationFlag As Int64 = 0) As Boolean

        If results Is Nothing Then results = New DTO.WCFResults()
        Dim dtNow = Date.Now()

        If oBooks Is Nothing OrElse oBooks.Count < 1 Then Return False
        Dim blnRet As Boolean = False
        Try
            'Check the BookDateLoad for procedure level security
            Dim dtLoad As Date? = (From d In oBooks Order By d.BookDateLoad Select d.BookDateLoad).FirstOrDefault()
            Dim strProcName As String = ""
            If dtLoad > Now Then
                strProcName = "FinalizeOrdersBeforeShip"
            Else
                strProcName = "FinalizeOrdersAfterShip"
            End If
            'We always validate the procedure role center security even if the intValidationFlag flag is false
            results.AddLog("Validate user security for finalize operation.")
            If NGLSecurityData.CanUserRunProcedure(0, strProcName) Then
                If results.isValidationOn(FreightMaster.Data.DataTransferObjects.WCFResults.ValidationBits.ValidateAlertsBeforeFinalize, intValidationFlag) Then
                    results.AddLog("Validate alerts before finalize.")
                    Dim oItem As DTO.DoFinalizeAlerts = NGLBatchProcessData.ValidateAlertsBeforeFinalize(oBooks(0).BookControl)
                    If Not oItem Is Nothing Then
                        'process the messages
                        Dim strMsg As String = ""
                        Dim strMsgSpacer As String = ""
                        Dim blnHasWarnings As Boolean = False
                        'The messages below are managed in the company parameter data with a global default in the parameter table.
                        'these messages do not use the message enum but the second message does use localization so we need to create
                        'two messages in the message collection object the first one  is a warning and the second is a message with details
                        With oItem
                            If Not String.IsNullOrEmpty(.ContractExpiresMessage) AndAlso .ContractExpiresDate.HasValue Then
                                blnHasWarnings = True
                                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, .ContractExpiresMessage, True, .ContractExpiresMessage, Nothing)
                                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.MSGdoFinalizeContractExp, .CarrierNumber.ToString(), If(.ContractExpiresDate.HasValue(), .ContractExpiresDate.Value.ToShortDateString, "*Unknown*"))
                            End If
                            If Not String.IsNullOrEmpty(.InsuranceMessage) AndAlso .InsuranceDate.HasValue Then
                                blnHasWarnings = True
                                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, .InsuranceMessage, True, .InsuranceMessage, Nothing)
                                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.MSGdoFinalizeInsuranceExp, .CarrierNumber.ToString(), If(.InsuranceDate.HasValue(), .InsuranceDate.Value.ToShortDateString, "*Unknown*"))
                            End If
                            If Not String.IsNullOrEmpty(.ExposuerPerShipmentMessage) Then
                                blnHasWarnings = True
                                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, .ExposuerPerShipmentMessage, True, .ExposuerPerShipmentMessage, Nothing)
                                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.MSGdoFinalizePerShipExp, .CarrierNumber.ToString(), Format(.PerShipmentValue, "C"), Format(.PerShipmentExposure, "C"))
                            End If
                            If Not String.IsNullOrEmpty(.ExposureAllMessage) Then
                                blnHasWarnings = True
                                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, .ExposureAllMessage, True, .ExposureAllMessage, Nothing)
                                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.MSGdoFinalizeAllExp, .CarrierNumber.ToString(), Format(.TotalExposueValue, "C"), Format(.AllShipmentExposure, "C"))
                            End If
                        End With
                        If blnHasWarnings Then
                            results.Success = False
                            results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowValidationMsg)
                            results.ValidationBitFailed = FreightMaster.Data.DataTransferObjects.WCFResults.ValidationBits.ValidateAlertsBeforeFinalize
                            Return False
                        End If
                    End If
                End If
            Else
                'show not authorized message and return false
                results.AddLog("Access Denied.")
                If dtLoad > Now Then
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_AccessDeniedFinalizeOrdersBeforeShip, Me.Parameters.UserName)
                Else
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_AccessDeniedFinalizeOrdersAfterShip, Me.Parameters.UserName)
                End If
                results.Success = False
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowErrors)
                Return False
            End If
            ' code copied from spFinalizeBooking50 here...
            '1. We validate up above but in spFinalizeBooking50 we validate again then add the messages to an email in a particular format
            '   based on which validation failed.  We also update BookNotesVisible5 and messagers in the BookTrack table for SYSTEM: Carrier Validation Failure
            '   It is possible to improve performance in this logic but for now we need to call the old stored procedure
            results.AddLog("Validate carrier qualifications before finalize.")
            Dim oSQLResults = NGLBatchProcessData.ValidateCarrierQualBeforeFinalize(oBooks(0).BookControl)
            If Not oSQLResults Is Nothing AndAlso oSQLResults.ErrNumber <> 0 Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_ValidateCarrierQualBeforeFinalize_Error, oSQLResults.RetMsg)
                results.Success = False
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowErrors)
                Return False
            End If
            results.AddLog("Validate carrier assignment for load.")
            'Check if there are more than one carrier assigned to the load
            Dim intCarrierControl As Integer = oBooks(0).BookCarrierControl
            If oBooks.Any(Function(x) x.BookCarrierControl <> intCarrierControl) Then
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_ValidateCarrierQualBeforeFinalize_Error, "Multiple Carriers; only one carrier allowed for each movement.")
                results.Success = False
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowErrors)
                Return False
            End If
            results.AddLog("Update finalization codes and flags.")
            'update the fields for each item in the bookrevenue list
            For Each b In oBooks
                If Not String.IsNullOrEmpty(b.BookRouteFinalCode) Then
                    Select Case b.BookRouteFinalCode
                        Case "CM"
                            b.BookRouteFinalCode = "NS"
                        Case "UF"
                            b.BookRouteFinalCode = "RF"
                        Case Else
                            b.BookRouteFinalCode = "SH"
                    End Select
                Else
                    b.BookRouteFinalCode = "SH"
                End If
                b.BookTranCode = BookTranCode
                b.BookRouteFinalFlag = True
                b.BookRouteFinalDate = dtNow
                b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
            Next
            results.AddLog("Save changes to the database.")
            'Here the data is saved to the database
            NGLBookRevenueData.SaveRevenuesNoReturn(oBooks, False, True)
            'Write the data to the pick list table
            'TODO: validate that the BookConsPrefix is needed when we generate the picklist,  we should be able to use the SHID?
            Try
                results.AddLog("Generate a pick list status update record.")
                NGLBatchProcessData.generatePickListRecord2Way(oBooks(0).BookControl, oBooks(0).BookConsPrefix)
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.DoNothing)
            Catch ex As Exception
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_FinalizePickListFailed, ex.Message)
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowWarnings)
            End Try
            results.AddLog("Success, load is finalized.")
            results.Success = True
            Return True


        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            'sqlEx.Reason.ToString(, sqlEx.Detail.Message, sqlEx.Detail.Details)
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_SQLFaultCannotFinalize, sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details)
            results.Success = False
            results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowErrors)
            Return False
        Catch conflictEx As System.ServiceModel.FaultException(Of NGL.FreightMaster.Data.ConflictFaultInfo)
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_SQLFaultCannotFinalize, conflictEx.Reason.ToString(), conflictEx.Detail.Message)
            results.Success = False
            results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowErrors)
            Return False
        Catch ex As Exception
            results.AddUnexpectedError(ex)
            results.Success = False
            results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowErrors)
            Return False
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Replaces UndoFinalize
    ''' </summary>
    ''' <param name="oBooks"></param>
    ''' <param name="BookTranCode"></param>
    ''' <param name="results"></param>
    ''' <param name="intValidationFlag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UnFinalizeBookings(ByRef oBooks As DTO.BookRevenue(),
                                      ByVal BookTranCode As String,
                                      Optional ByRef results As DTO.WCFResults = Nothing,
                                      Optional ByVal intValidationFlag As Int64 = 0) As Boolean

        If results Is Nothing Then results = New DTO.WCFResults()
        Dim dtNow = Date.Now()
        If oBooks Is Nothing OrElse oBooks.Count < 1 Then Return False
        Dim blnRet As Boolean = False
        Try
            'Check the BookDateLoad for procedure level security
            Dim dtLoad As Date? = (From d In oBooks Order By d.BookDateLoad Select d.BookDateLoad).FirstOrDefault()
            Dim strProcName As String = ""
            If dtLoad > Now Then
                strProcName = "UnFinalizeOrdersBeforeShip"
            Else
                strProcName = "UnFinalizeOrdersAfterShip"
            End If
            'We always validate the procedure role center security even if the Validate flag is false
            results.AddLog("Validate user security for unfinalize operation.")
            If Not NGLSecurityData.CanUserRunProcedure(0, strProcName) Then
                'show not authorized message and return false
                results.AddLog("Access Denied.")
                If dtLoad > Now Then
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_AccessDeniedUnFinalizeOrdersBeforeShip, Me.Parameters.UserName)
                Else
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_AccessDeniedUnFinalizeOrdersAfterShip, Me.Parameters.UserName)
                End If
                results.Success = False
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowWarnings)
                Return False
            End If
            ' code copied from spUnFinalizeBooking50 here...            
            results.AddLog("Update unfinalization codes and flags.")
            'update the fields for each item in the bookrevenue list
            Dim blnInsertBookDeletedRecord As Boolean = False
            For Each b In oBooks
                If Not String.IsNullOrEmpty(b.BookRouteFinalCode) Then
                    Select Case b.BookRouteFinalCode
                        Case "SC"
                            b.BookRouteFinalCode = "UF"
                        Case "RF"
                            b.BookRouteFinalCode = "UF"
                        Case "MS"
                            If (DateDiff(DateInterval.Day, dtNow, If(b.BookDateLoad.HasValue, b.BookDateLoad.Value.AddDays(1), dtNow.AddDays(1))) > 0) Then
                                'Not Shipped Yet
                                b.BookRouteFinalCode = "CM"
                                blnInsertBookDeletedRecord = True 'We must send an EDI cancel if any BookRouteFinalCode = "CM"
                            Else
                                'Already Shipped
                                b.BookRouteFinalCode = "UF"
                            End If
                        Case "US"
                            b.BookRouteFinalCode = "UF"
                        Case "UL"
                            b.BookRouteFinalCode = "CM"
                            blnInsertBookDeletedRecord = True 'We must send an EDI cancel if any BookRouteFinalCode = "CM"
                        Case Else
                            b.BookRouteFinalCode = "CM"
                            blnInsertBookDeletedRecord = True 'We must send an EDI cancel if any BookRouteFinalCode = "CM"
                    End Select
                Else
                    b.BookRouteFinalCode = "CM"
                    blnInsertBookDeletedRecord = True 'We must send an EDI cancel if any BookRouteFinalCode = "CM"
                End If
                b.BookRouteFinalFlag = False
                b.BookTranCode = BookTranCode
                b.BookRouteFinalDate = Nothing
                b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated

            Next
            results.AddLog("Save changes to the database.")
            'Here the data is saved to the database
            'Modified By LVV on 10/20/16 for v-7.0.5.110 Task #35 Accept/Reject Fixes
            'Changed second param below from False to True so the Details get saved
            'This is so that the changes made locally during ResetToNStatus() get saved to the database
            NGLBookRevenueData.SaveRevenuesNoReturn(oBooks, True, True)
            'Write the data to the pick list table
            'TODO: validate that the BookConsPrefix is needed when we generate the picklist,  we should be able to use the SHID?
            Try
                results.AddLog("Generate a pick list status update record.")
                NGLBatchProcessData.generatePickListRecord2Way(oBooks(0).BookControl, oBooks(0).BookConsPrefix)
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.DoNothing)
            Catch ex As Exception
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_FinalizePickListFailed, ex.Message)
                results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowWarnings)
            End Try
            If blnInsertBookDeletedRecord Then
                Dim oRet = NGLBookData.InsertBookDeletedForUnfinalized(oBooks(0).BookControl)
                If Not oRet Is Nothing AndAlso oRet.ErrNumber <> 0 Then
                    'log the error
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_UnFinalizeCancelEDIFailed, oRet.RetMsg)
                    results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowWarnings)
                End If
            End If
            results.AddLog("Success, load is finalized.")
            results.Success = True
            Return True

        Catch sqlEx As FaultException(Of DAL.SqlFaultInfo)
            'sqlEx.Reason.ToString(, sqlEx.Detail.Message, sqlEx.Detail.Details)
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_SQLFaultCannotFinalize, sqlEx.Reason.ToString(), sqlEx.Detail.Message, sqlEx.Detail.Details)
            results.Success = False
            results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowErrors)
            Return False
        Catch ex As Exception
            results.AddUnexpectedError(ex)
            results.Success = False
            results.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.ShowErrors)
            Return False
        End Try
        Return blnRet
    End Function



#End Region

#End Region

End Class
