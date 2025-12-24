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
Imports NGL.Core
Imports NGL.FM.DAT
Imports Serilog
Imports SerilogTracing

Public Class NGLOrderImportBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLOrderImportBLL"
        Me.Logger = Logger.ForContext(Of NGLOrderImportBLL)
    End Sub

#End Region

#Region " Properties "

#End Region

#Region "Delegates"

    Public Delegate Sub ProcessDataDelegate(ByRef lOrders As List(Of DTO.tblSolutionDetail), ByVal OptimizeCapacity As Boolean)
    Public Delegate Sub ProcessUnRoutedDataDelegate(ByRef lOrders As List(Of DTO.tblSolutionDetail))

#End Region

#Region "DAL Wrapper Methods"

    Public Function ImportAllPOHdrRecords(ByVal StartDate As Date,
                                      ByVal EndDate As Date,
                                      Optional ByVal CompNumber As Integer = 0,
                                      Optional ByVal FrtTyp As Byte = 0,
                                      Optional ByVal CreateUser As String = "",
                                      Optional ByVal NatAcctNbr As Integer = 0) As DTO.POHdr()
        Dim oPOHdrs() As DTO.POHdr
        Using operation = Logger.StartActivity("ImportAllPoHdrRecords(StartDate: {StartDate}, EndDate: {EndDate}, CompNumber: {CompNumber}, FreightType: {FreightType}, CreateUser: {CreateUser}, NatAcctNumber: {NatAcctNumber}", StartDate, EndDate, CompNumber, FrtTyp, CreateUser, NatAcctNbr)
            Try
                oPOHdrs = NGLPOHdrData.GetPOHdrsFiltered(StartDate, EndDate, CompNumber, FrtTyp, CreateUser, NatAcctNbr)
                For Each oData In oPOHdrs
                    If oData.POHDRHoldLoad Then throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_AccessDenied, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_RecordOnHold, Nothing, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_InvalidOperationException)
                    Dim strBookProNumber As String = oData.POHDRPRONumber
                    Dim strOrderNumber As String = oData.POHDROrderNumber
                    Dim intOrderSequence As Integer = oData.POHDROrderSequence
                    Dim intDefCompNumber As Integer = 0
                    If oData.POHDRDefaultCustomer.HasValue Then intDefCompNumber = oData.POHDRDefaultCustomer.Value
                    Dim strVendorNumber As String = oData.POHDRvendor
                    Dim strPOHDRModVerify As String = oData.POHDRModVerify.ToUpper
                    'Check if we are allowed to process this record
                    If strPOHDRModVerify = "NEW COMP" Then
                        Dim blnProcessNewComp As Boolean = True
                        Try
                            NGLPOHdrData.validateNewCompPro(strBookProNumber, intDefCompNumber)
                        Catch ex As FaultException
                            blnProcessNewComp = False
                        End Try
                        If blnProcessNewComp Then
                            'It is ok to import the data
                            ImportPOHdrRecord(strPOHDRModVerify,
                                                 strOrderNumber,
                                                 intOrderSequence,
                                                 intDefCompNumber,
                                                 strVendorNumber,
                                                 strBookProNumber)
                        End If
                    ElseIf strPOHDRModVerify <> "FINALIZED" _
                        And strPOHDRModVerify <> "DELETE-F" _
                        And strPOHDRModVerify <> "NO LANE" _
                        And strPOHDRModVerify <> "NEW TRAN-F" _
                        And strPOHDRModVerify <> "NEW TRAN" Then
                        'Import the data
                        ImportPOHdrRecord(strPOHDRModVerify,
                                             strOrderNumber,
                                             intOrderSequence,
                                             intDefCompNumber,
                                             strVendorNumber,
                                             strBookProNumber)
                    End If
                Next
                operation.Complete()
                'get any data left using the current filters
                Return NGLPOHdrData.GetPOHdrsFiltered(StartDate, EndDate, CompNumber, FrtTyp, CreateUser, NatAcctNbr)
            Catch ex As FaultException
                operation.Complete(exception:=ex)
                Throw
            Catch ex As Exception
                operation.Complete(exception:=ex)
                throwUnExpectedFaultException(ex, buildProcedureName("ImportAllPOHdrRecords"))
            End Try
        End Using

        Return oPOHdrs
    End Function

    Public Function ImportAllPOHdrRecordsByCompOrNatAcctNbr(Optional ByVal CompNumber As Integer = 0,
                                                            Optional ByVal NatAcctNbr As Integer = 0) As DTO.POHdr()
        Dim oPOHdrs() As DTO.POHdr
        Try
            oPOHdrs = NGLPOHdrData.GetAllPOHdrsFilteredByCompOrNatAcct(CompNumber, NatAcctNbr)
            For Each oData In oPOHdrs
                If Not oData.POHDRHoldLoad Then
                    Dim strBookProNumber As String = oData.POHDRPRONumber
                    Dim strOrderNumber As String = oData.POHDROrderNumber
                    Dim intOrderSequence As Integer = oData.POHDROrderSequence
                    Dim intDefCompNumber As Integer = 0
                    If oData.POHDRDefaultCustomer.HasValue Then intDefCompNumber = oData.POHDRDefaultCustomer.Value
                    Dim strVendorNumber As String = oData.POHDRvendor
                    Dim strPOHDRModVerify As String = oData.POHDRModVerify.ToUpper
                    'Check if we are allowed to process this record
                    If strPOHDRModVerify = "NEW COMP" Then
                        Dim blnProcessNewComp As Boolean = True
                        Try
                            NGLPOHdrData.validateNewCompPro(strBookProNumber, intDefCompNumber)
                        Catch ex As FaultException
                            blnProcessNewComp = False
                        End Try
                        If blnProcessNewComp Then
                            'It is ok to import the data
                            ImportPOHdrRecord(strPOHDRModVerify,
                                                 strOrderNumber,
                                                 intOrderSequence,
                                                 intDefCompNumber,
                                                 strVendorNumber,
                                                 strBookProNumber)
                        End If
                    ElseIf strPOHDRModVerify <> "FINALIZED" _
                        And strPOHDRModVerify <> "DELETE-F" _
                        And strPOHDRModVerify <> "NO LANE" _
                        And strPOHDRModVerify <> "NEW TRAN-F" _
                        And strPOHDRModVerify <> "NEW TRAN" Then
                        'Import the data
                        ImportPOHdrRecord(strPOHDRModVerify,
                                             strOrderNumber,
                                             intOrderSequence,
                                             intDefCompNumber,
                                             strVendorNumber,
                                             strBookProNumber)
                    End If
                End If
            Next
            'get any data left using the current filters
            Return NGLPOHdrData.GetAllPOHdrsFilteredByCompOrNatAcct(CompNumber, NatAcctNbr)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("ImportAllPOHdrRecordsByCompOrNatAcctNbr"))
        End Try
        Return oPOHdrs
    End Function

    Public Function ImportPOHdrRecord(ByVal oData As DTO.POHdr) As Boolean
        Try
            If oData.POHDRHoldLoad Then throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_AccessDenied, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_RecordOnHold, Nothing, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_InvalidOperationException)
            Dim strBookProNumber As String = oData.POHDRPRONumber
            Dim strOrderNumber As String = oData.POHDROrderNumber
            Dim intOrderSequence As Integer = oData.POHDROrderSequence
            Dim intDefCompNumber As Integer = 0
            If oData.POHDRDefaultCustomer.HasValue Then intDefCompNumber = oData.POHDRDefaultCustomer.Value
            Dim strVendorNumber As String = oData.POHDRvendor
            Dim strPOHDRModVerify As String = oData.POHDRModVerify
            Return ImportPOHdrRecord(strPOHDRModVerify,
                                       strOrderNumber,
                                       intOrderSequence,
                                       intDefCompNumber,
                                       strVendorNumber,
                                     strBookProNumber)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("ImportPOHdrRecord"))
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Uses the value in strPOHDRModVerify to determine what action to take when processing orders in the order preview screen
    ''' </summary>
    ''' <param name="strPOHDRModVerify"></param>
    ''' <param name="strOrderNumber"></param>
    ''' <param name="intOrderSequence"></param>
    ''' <param name="intDefCompNumber"></param>
    ''' <param name="strVendorNumber"></param>
    ''' <param name="strBookProNumber"></param>
    ''' <param name="blnAsync"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 11/9/2015 added optional blnAsync parameter; 
    ''' normally this is true but for testing and debugging this may be set to false
    ''' Modified by RHR 01/29/2016 v-7.0.4.1
    '''   added new logic to call UpdateCarrierCost instead of AssignCarrier when costs need to be recalculated
    '''   this fixes the issue where costs are only recalculated using the line haul.  the new method gets the updated
    '''   line haul if a tariff has been assigned and follow the new ship confirmed business rules.
    ''' Modified by RHR for v-7.0.6.101 on 1/26/2017 to be sure we update item details
    ''' </remarks>
    Public Function ImportPOHdrRecord(ByVal strPOHDRModVerify As String,
                                      ByVal strOrderNumber As String,
                                      ByVal intOrderSequence As Integer,
                                      ByVal intDefCompNumber As Integer,
                                      ByVal strVendorNumber As String,
                                      Optional ByVal strBookProNumber As String = "",
                                      Optional ByVal blnAsync As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim strNewPro As String = ""
        Using operation = Logger.StartActivity("ImportPOHdrRecord(strPOHDRModVerify: {strPOHDRModVerify}, strOrderNumber: {strOrderNumber}, intOrderSequence: {intOrderSequence}, intDefCompNumber: {intDefCompNumber}, strVendorNumber: {strVendorNumber}, strBookProNumber: {strBookProNumber}, blnAsync: {blnAsync}", strPOHDRModVerify, strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strBookProNumber, blnAsync)
            Try
                'Check Data
                If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then throwFieldRequiredException("Order Number")
                If intDefCompNumber = 0 Then throwFieldRequiredException("Default Company")
                If String.IsNullOrEmpty(strVendorNumber) OrElse strVendorNumber.Trim.Length < 1 Then throwFieldRequiredException("Lane Number")
                Select Case strPOHDRModVerify.ToUpper
                    Case "FINALIZED"
                        Dim oResults = NGLBookData.UpdateBookingRecord(strOrderNumber, intOrderSequence, intDefCompNumber)
                        If Not oResults Is Nothing Then
                            If oResults.BookControl.HasValue AndAlso oResults.BookControl.Value <> 0 Then
                                If oResults.MustRecalculate Then
                                    BookRevenueBLL.UpdateCarrierCost(oResults.BookControl.Value)
                                Else 'Modified by RHR for v-7.0.6.101 on 1/26/2017 be sure we update item details
                                    NGLBookData.UpdateBookDependencies(oResults.BookControl.Value, 0)
                                End If
                                If If(oResults.AddToPicklist, False) Then
                                    NGLBatchProcessData.generatePickListRecord(oResults.BookControl.Value, If(oResults.BookConsPrefix, ""))
                                End If
                            End If
                        End If
                    Case "DELETED"
                        NGLPOHdrData.runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber)
                    Case "DELETE-B"
                        If String.IsNullOrEmpty(strBookProNumber) OrElse strBookProNumber.Trim.Length < 1 Then throwFieldRequiredException("Pro Number")
                        NGLPOHdrData.runDeleteOrderWithData(strBookProNumber, strOrderNumber, intOrderSequence, intDefCompNumber)
                    Case "DELETE-F"
                        If String.IsNullOrEmpty(strBookProNumber) OrElse strBookProNumber.Trim.Length < 1 Then throwFieldRequiredException("Pro Number")
                        NGLPOHdrData.runDeleteOrderWithData(strBookProNumber, strOrderNumber, intOrderSequence, intDefCompNumber)
                    Case "NO LANE"
                        Dim oResult = NGLBookData.WriteNewBookingForBatch(strOrderNumber, intOrderSequence, intDefCompNumber, strNewPro)
                        If blnAsync Then
                            TryToRouteLoadAsync(strNewPro)
                        Else
                            TryToRouteLoad(strNewPro)
                        End If
                    Case "NEW LANE"
                        Dim oResult = NGLBookData.WriteNewBookingForBatch(strOrderNumber, intOrderSequence, intDefCompNumber, strNewPro)
                        If blnAsync Then
                            TryToRouteLoadAsync(strNewPro)
                        Else
                            TryToRouteLoad(strNewPro)
                        End If
                    Case "NEW COMP"
                        NGLPOHdrData.validateNewCompPro(strBookProNumber, intDefCompNumber)
                        Dim oResult = NGLBookData.WriteNewBookingForBatch(strOrderNumber, intOrderSequence, intDefCompNumber, strNewPro)
                        If blnAsync Then
                            TryToRouteLoadAsync(strNewPro)
                        Else
                            TryToRouteLoad(strNewPro)
                        End If
                    Case "NO PRO"
                        Dim oResult = NGLBookData.WriteNewBookingForBatch(strOrderNumber, intOrderSequence, intDefCompNumber, strNewPro)
                        If blnAsync Then
                            TryToRouteLoadAsync(strNewPro)
                        Else
                            TryToRouteLoad(strNewPro)
                        End If
                    Case "NEW TRAN"
                        If String.IsNullOrEmpty(strBookProNumber) OrElse strBookProNumber = "0" Then
                            Dim oResult = NGLBookData.WriteNewBookingForBatch(strOrderNumber, intOrderSequence, intDefCompNumber, strNewPro)
                            If blnAsync Then
                                TryToRouteLoadAsync(strNewPro)
                            Else
                                TryToRouteLoad(strNewPro)
                            End If
                        Else
                            Dim oResults = NGLBookData.UpdateBookingRecord(strOrderNumber, intOrderSequence, intDefCompNumber)
                            If Not oResults Is Nothing Then
                                If oResults.BookControl.HasValue AndAlso oResults.BookControl.Value <> 0 Then
                                    If oResults.MustRecalculate Then
                                        BookRevenueBLL.UpdateCarrierCost(oResults.BookControl.Value)
                                    Else 'Modified by RHR for v-7.0.6.101 on 1/26/2017 be sure we update item details
                                        NGLBookData.UpdateBookDependencies(oResults.BookControl.Value, 0)
                                    End If
                                    If If(oResults.AddToPicklist, False) Then
                                        NGLBatchProcessData.generatePickListRecord(oResults.BookControl.Value, If(oResults.BookConsPrefix, ""))
                                    End If
                                End If
                            End If
                        End If
                    Case Else
                        Dim oResults = NGLBookData.UpdateBookingRecord(strOrderNumber, intOrderSequence, intDefCompNumber)
                        If Not oResults Is Nothing Then
                            If oResults.BookControl.HasValue AndAlso oResults.BookControl.Value <> 0 Then
                                If oResults.MustRecalculate Then
                                    BookRevenueBLL.UpdateCarrierCost(oResults.BookControl.Value)
                                Else 'Modified by RHR for v-7.0.6.101 on 1/26/2017 be sure we update item details
                                    NGLBookData.UpdateBookDependencies(oResults.BookControl.Value, 0)
                                End If
                                If If(oResults.AddToPicklist, False) Then
                                    NGLBatchProcessData.generatePickListRecord(oResults.BookControl.Value, If(oResults.BookConsPrefix, ""))
                                End If
                            End If
                        End If
                End Select
                blnRet = True
            Catch ex As FaultException
                operation.Complete(exception:=ex)
                Throw
            Catch ex As Exception
                operation.Complete(exception:=ex)
                throwUnExpectedFaultException(ex, buildProcedureName("ImportPOHdrRecord"), FreightMaster.Data.sysErrorParameters.sysErrorState.ServerLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Updates the Load Planning Item using the CompControl and TruckKey information
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="TruckKey"></param>
    ''' <param name="droppedItem"></param>
    ''' <param name="droppedIndex"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' WriteNewBooking  now we always call spWriteNewBookingForBatch
    ''' --> The following DAL procedures must be replaced or modified:
    '''	--> NGLLoadPlanningTruckData.WriteNewBooking
    '''		--> called by NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier
    '''	--> NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier
    '''		--> called by: NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier30
    '''			--> Called by NGLLoadPlanningTruckData.reSequenceStopNumbers
    '''				--> Called by NGLLoadPlanningTruckData.UpdateLoadPlanningItem30
    '''					--> Called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateLoadPlanningItem30Old
    '''					--> Called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateLoadPlanningItem30
    '''		--> called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateNewPOWithLoadPlanningCarrier
    ''' </remarks>
    Public Function UpdateLoadPlanningItem30(ByVal CompControl As Integer, ByVal TruckKey As String, ByVal droppedItem As DTO.tblSolutionDetail, ByVal droppedIndex As Integer) As DTO.UpdateLoadPlanningResults
        Return UpdateLoadPlanningItem30(NGLLoadPlanningTruckData.GetLoadPlanningTruckFiltered(CompControl, TruckKey), droppedItem, droppedIndex)
    End Function

    ''' <summary>
    ''' Updates the Load Planning Item using a tblSolutionTruck object
    ''' </summary>
    ''' <param name="newTruck"></param>
    ''' <param name="droppedItem"></param>
    ''' <param name="droppedIndex"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' WriteNewBooking  now we always call spWriteNewBookingForBatch
    ''' --> The following DAL procedures must be replaced or modified:
    '''	--> NGLLoadPlanningTruckData.WriteNewBooking
    '''		--> called by NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier
    '''	--> NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier
    '''		--> called by: NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier30
    '''			--> Called by NGLLoadPlanningTruckData.reSequenceStopNumbers
    '''				--> Called by NGLLoadPlanningTruckData.UpdateLoadPlanningItem30
    '''					--> Called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateLoadPlanningItem30Old
    '''					--> Called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateLoadPlanningItem30
    '''		--> called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateNewPOWithLoadPlanningCarrier
    ''' </remarks>
    Public Function UpdateLoadPlanningItem30(ByVal newTruck As DTO.tblSolutionTruck, ByVal droppedItem As DTO.tblSolutionDetail, ByVal droppedIndex As Integer) As DTO.UpdateLoadPlanningResults
        Dim newStopNo As Integer = 1
        Dim curItem As DTO.tblSolutionDetail = Nothing
        Dim foundItem As Boolean = False
        'remove the item if can.
        For i As Integer = 0 To newTruck.SolutionDetails.Count - 1
            curItem = newTruck.SolutionDetails(i)
            If curItem.SolutionDetailBookControl = droppedItem.SolutionDetailBookControl Then
                newTruck.SolutionDetails.RemoveAt(i)
                foundItem = True
                Exit For
            End If
        Next

        'if the dropped index is last, add the dropped item to the end.
        If droppedIndex > newTruck.SolutionDetails.Count - 1 Then
            newTruck.SolutionDetails.Add(droppedItem)
        Else
            newTruck.SolutionDetails.Insert(droppedIndex, droppedItem)
        End If

        Dim result As DTO.UpdateLoadPlanningResults = reSequenceStopNumbers(newTruck, newTruck.SolutionDetails.IndexOf(droppedItem))

        'finally get the latest changes.
        Dim lpTruck As DTO.tblSolutionTruck = NGLLoadPlanningTruckData.GetLoadPlanningTruckFiltered(newTruck.SolutionTruckCompControl, newTruck.SolutionTruckKey)

        result.UpdatedLPTruck = lpTruck

        Return result
    End Function

    ''' <summary>
    ''' Replaces Calls to the NGL.FreightMaster.Data.NGLLoadPlanningTruckData.reSequenceStopNumbers
    ''' </summary>
    ''' <param name="truck"></param>
    ''' <param name="newDetailIndex"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' WriteNewBooking  now we always call spWriteNewBookingForBatch
    ''' --> The following DAL procedures must be replaced or modified:
    '''	--> NGLLoadPlanningTruckData.WriteNewBooking
    '''		--> called by NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier
    '''	--> NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier
    '''		--> called by: NGLLoadPlanningTruckData.UpdateNewPOWithLoadPlanningCarrier30
    '''			--> Called by NGLLoadPlanningTruckData.reSequenceStopNumbers
    '''				--> Called by NGLLoadPlanningTruckData.UpdateLoadPlanningItem30
    '''					--> Called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateLoadPlanningItem30Old
    '''					--> Called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateLoadPlanningItem30
    '''		--> called by NGL.Service.FreightMaster.Data.NGLSolutionData.UpdateNewPOWithLoadPlanningCarrier
    ''' </remarks>
    Friend Function reSequenceStopNumbers(ByVal truck As DTO.tblSolutionTruck, ByVal newDetailIndex As Integer) As DTO.UpdateLoadPlanningResults
        Dim success As Boolean = False
        Dim result As New DTO.UpdateLoadPlanningResults
        Try

            Dim Item As DTO.tblSolutionDetail
            Dim stopNumbCounter As Integer = 0
            Dim currAddr As String = ""
            Dim prevAddr As String = ""
            Dim blnUseStop1 As Boolean = False
            If truck.SolutionDetails(0).SolutionDetailRouteTypeCode = 4 Then blnUseStop1 = True

            For i As Integer = 0 To truck.SolutionDetails.Count - 1
                Item = truck.SolutionDetails(i)
                stopNumbCounter = stopNumbCounter + 1

                'they dropped it somewhere after the first spot
                If Item.SolutionDetailInbound Then
                    'compare the origins
                    currAddr = Item.SolutionDetailOrigAddress1.ToUpper & "-" & Item.SolutionDetailOrigCity.ToUpper & "-" & Item.SolutionDetailOrigState.ToUpper & "-" & Item.SolutionDetailOrigZip.ToUpper
                Else
                    'compare the destinations
                    currAddr = Item.SolutionDetailDestAddress1.ToUpper & "-" & Item.SolutionDetailDestCity.ToUpper & "-" & Item.SolutionDetailDestState.ToUpper & "-" & Item.SolutionDetailDestZip.ToUpper
                End If

                'If the address is the same, no need to increment the stop number.
                If prevAddr.Equals(currAddr) Then
                    'same stop no.
                    stopNumbCounter = stopNumbCounter - 1
                End If
                If blnUseStop1 Then
                    stopNumbCounter = 1
                End If
                'check is this is an import.  Imported records will not have a BookControl number.
                If Item.SolutionDetailBookControl = 0 Then
                    'if the newDetailIndex is the same as the index we are on, then we must call the updateFull method instead of the quick update stop method.
                    If i = newDetailIndex Then
                        success = UpdateNewPOWithLoadPlanningCarrier30(truck, Item, stopNumbCounter, False)
                    Else
                        success = UpdateLoadPlanningNewStopNo30(truck, Item, stopNumbCounter, False)
                    End If
                Else
                    If i = newDetailIndex Then
                        success = UpdateLoadPlanningCarrierWithTruck30(truck, Item, stopNumbCounter, False)
                    Else
                        success = UpdateLoadPlanningNewStopNo30(truck, Item, stopNumbCounter, False)
                    End If
                End If
                prevAddr = currAddr
                If success = False Then
                    Exit For
                End If
            Next

            result.Success = success
        Catch ex As FaultException
            result.Success = False
            result.Exception = ex
            Throw ex
            'Catch ex As InvalidOperationException
            '    result.Success = False
            '    Utilities.SaveAppError(ex.Message, Me.Parameters)
            '    Dim exFaultException As New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            '    result.Exception = exFaultException
            '    Throw exFaultException
            'Catch ex As System.Data.SqlClient.SqlException
            '    result.Success = False
            '    Utilities.SaveAppError(ex.Message, Me.Parameters)
            '    Dim exFaultException As New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            '    result.Exception = exFaultException
            '    Throw exFaultException
            'Catch ex As Exception
            '    result.Success = False
            '    Utilities.SaveAppError(ex.Message, Me.Parameters)
            '    Dim exFaultException As New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            '    result.Exception = exFaultException
            '    Throw exFaultException
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("reSequenceStopNumbers"), FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
        Return result
    End Function

    Private Function UpdateLoadPlanningCarrierWithTruck30(ByVal newTruck As DTO.tblSolutionTruck, ByVal detailItem As DTO.tblSolutionDetail, ByRef newStopNo As Integer, ByVal refreshTruck As Boolean) As Boolean
        Dim success As Boolean = False
        'Try
        NGLLoadPlanningTruckData.UpdateLoadPlanningCarrier(detailItem.SolutionDetailBookControl,
                                                                              newTruck.SolutionTruckConsPrefix,
                                                                              detailItem.SolutionDetailRouteConsFlag,
                                                                              newStopNo,
                                                                              newTruck.SolutionTruckCarrierControl,
                                                                              newTruck.SolutionTruckCarrierTruckControl)
        success = True

        Return success
    End Function

    Private Function UpdateNewPOWithLoadPlanningCarrier30(ByVal newTruck As DTO.tblSolutionTruck, ByVal detailItem As DTO.tblSolutionDetail, ByRef newStopNo As Integer, ByVal refreshTruck As Boolean) As Boolean
        Dim success As Boolean = False
        'Try
        'get consIntegrity flag from other load
        Dim consFlag As Boolean = True
        Dim holdLoad As Integer = 0

        If newTruck.SolutionDetails.Count = 0 Then
            consFlag = True
            holdLoad = 0
        Else
            consFlag = newTruck.SolutionDetails(0).SolutionDetailRouteConsFlag
            holdLoad = newTruck.SolutionDetails(0).SolutionDetailHoldLoad
        End If

        UpdateNewPOWithLoadPlanningCarrier(detailItem.SolutionDetailOrderNumber,
                                                                              detailItem.SolutionDetailOrderSequence,
                                                                              detailItem.SolutionDetailCompNumber,
                                                                              newTruck.SolutionTruckConsPrefix,
                                                                              consFlag,
                                                                              newStopNo,
                                                                              newTruck.SolutionTruckCarrierControl,
                                                                              newTruck.SolutionTruckCarrierTruckControl,
                                                                              holdLoad)
        success = True

        Return success
    End Function

    Private Function UpdateLoadPlanningNewStopNo30(ByVal newTruck As DTO.tblSolutionTruck, ByVal detailItem As DTO.tblSolutionDetail, ByRef newStopNo As Integer, ByVal refreshTruck As Boolean) As Boolean
        Dim success As Boolean = False
        NGLLoadPlanningTruckData.UpdateLoadPlanningStopNo(detailItem.SolutionDetailBookControl, newStopNo)
        success = True
        Return success
    End Function

    ''' <summary>
    ''' This method calls the .Net method AssignCarrier for all orders with the specified BookConsPrefix.
    ''' 1. get all books by consprefix
    ''' 2. select the first record with cons integrity checked
    ''' 3. calculate the costs by calling AssignCarrier
    ''' 4. Process New tran code 
    ''' 5. Loop thru the orders where cons integrity is unchecked
    ''' 6. calculate the costs by calling Assigned carrier
    ''' 7. Process new TransCode
    ''' 
    ''' Throws E_InvalidOperationException if BookConsPrefix is NULL or less than 2 characters long 
    ''' </summary>
    ''' <param name="BookConsPrefix"></param>
    ''' <remarks>
    ''' Calculates and Tenders the load.  This works for standard and ltl pool truck loads.
    ''' Returns an array of WCFRsults for validation and processing messages.
    ''' If validation Fails, the caller needs to call ProcessNewTranCode for that specific bookcontrol that failed validation.
    ''' </remarks>
    Public Function CalculateAndTenderLoad(ByVal BookConsPrefix As String) As DTO.WCFResults()
        If String.IsNullOrEmpty(BookConsPrefix) OrElse BookConsPrefix.Trim.Length < 2 Then
            'this is not a valid CNS Number so throw an error
            throwInvalidOperatonException("Invalid CNS Number", True, False)
        End If

        'next get a list of booking records for this cns number
        Dim oBook = NGLBookData.GetBooksFilteredNoChildren(BookConsPrefix)
        Dim blnSuccess As Boolean = False

        Dim results As New List(Of DTO.WCFResults)

        If Not oBook Is Nothing AndAlso oBook.Count > 0 Then
            'update the consolidated loads first
            For Each b In oBook.Where(Function(x) x.BookRouteConsFlag = True)
                Dim carCostResult As New DTO.CarrierCostResults
                carCostResult = BookRevenueBLL.AssignCarrier(b.BookControl, b.BookCarrierControl, b.BookCarrTarEquipMatControl, b.BookCarrTarEquipControl)
                Dim AssignCarrierResult As New DTO.WCFResults
                If carCostResult.Success = False Then
                    AssignCarrierResult.Success = False
                    AssignCarrierResult.Log = carCostResult.Log
                    AssignCarrierResult.Messages = carCostResult.Messages
                    results.Add(AssignCarrierResult)
                    Return results.ToArray()
                    Exit For
                End If
                'set the messages from the assign carrier results.
                AssignCarrierResult.Log = carCostResult.Log
                AssignCarrierResult.Messages = carCostResult.Messages
                AssignCarrierResult.Success = carCostResult.Success
                results.Add(AssignCarrierResult)
                Dim ProcessNewTransCodeResult As New DTO.WCFResults
                ProcessNewTransCodeResult = BookBLL.ProcessNewTransCode(b.BookControl, "PC", "P", 0)
                results.Add(ProcessNewTransCodeResult)
                'we only need to call AssignCarrier once for the consolidation because the method 
                'Updates all orders on the load
                Exit For
            Next
            'update any loads where consolidation integrity is off
            For Each b In oBook.Where(Function(x) x.BookRouteConsFlag = False)
                Dim CarrierCostResult As New DTO.CarrierCostResults
                CarrierCostResult = BookRevenueBLL.AssignCarrier(b.BookControl, b.BookCarrierControl, b.BookCarrTarEquipMatControl, b.BookCarrTarEquipControl)
                Dim AssignCarrierResult As New DTO.WCFResults
                If CarrierCostResult.Success = False Then
                    AssignCarrierResult.Success = False
                    AssignCarrierResult.Log = CarrierCostResult.Log
                    AssignCarrierResult.Messages = CarrierCostResult.Messages
                    results.Add(AssignCarrierResult)
                    Return results.ToArray()
                    Exit For
                End If
                'set the messages from the assign carrier results.
                AssignCarrierResult.Log = CarrierCostResult.Log
                AssignCarrierResult.Messages = CarrierCostResult.Messages
                AssignCarrierResult.Success = CarrierCostResult.Success
                results.Add(AssignCarrierResult)
                Dim ProcessNewTransCodeResult As New DTO.WCFResults
                ProcessNewTransCodeResult = BookBLL.ProcessNewTransCode(b.BookControl, "PC", "P", 0)
                results.Add(ProcessNewTransCodeResult)
            Next
        End If
        Return results.ToArray()
    End Function

    ''' <summary>
    ''' No longer used Deprecated  we now use ProcessNewBookWaiting called from the task service only
    ''' </summary>
    ''' <param name="BookProNumber"></param>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.110 9/27/16
    ''' No longer used Deprecated
    ''' </remarks>
    Public Sub TryToRouteLoadAsync(ByVal BookProNumber As String)
        Return
        'Dim newBooking = NGLNewBookingsForSolutionData.GetNewBookingFilteredByPro(BookProNumber)
        ''Modified by RHR 11/9/2015 added new method in lane data isTransLoadOn to test transload before we call processTransLoadFacility
        ''this improves performance on process all
        'If Not newBooking Is Nothing Then
        '    If newBooking.SolutionDetailODControl <> 0 Then
        '        If NGLLaneData.isTransLoadOn(newBooking.SolutionDetailODControl) Then
        '            'check if this is a transload facility and process the splits needed for the order
        '            'TODO:?  for 7.0 we may need to run each transload through the routing guide logic, but in 6.3 we do not do this?
        '            If LaneTransLoadXrefDetBLL.processTransLoadFacility(newBooking.SolutionDetailBookControl) Then Return
        '        End If
        '    End If
        'End If

        'If Not newBooking Is Nothing AndAlso newBooking.SolutionDetailRouteGuideControl <> 0 Then
        '    Dim fetcher As New ProcessDataDelegate(AddressOf Me.prepareToRouteLoads)
        '    ' Launch thread
        '    fetcher.BeginInvoke(New List(Of DTO.tblSolutionDetail) From {newBooking}, True, Nothing, Nothing)
        'Else
        '    Dim fetcher As New ProcessUnRoutedDataDelegate(AddressOf Me.processUnRoutedLoads)
        '    ' Launch thread
        '    fetcher.BeginInvoke(New List(Of DTO.tblSolutionDetail) From {newBooking}, Nothing, Nothing)
        'End If
    End Sub

    ''' <summary>
    ''' No longer used Deprecated  we now use ProcessNewBookWaiting called from the task service only
    ''' </summary>
    ''' <param name="BookProNumber"></param>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.110 9/27/16
    ''' No longer used Deprecated
    ''' </remarks>
    Public Sub TryToRouteLoad(ByVal BookProNumber As String)
        Return
        'Dim newBooking = NGLNewBookingsForSolutionData.GetNewBookingFilteredByPro(BookProNumber)
        ''Modified by RHR 11/9/2015 added new method in lane data isTransLoadOn to test transload before we call processTransLoadFacility
        ''this improves performance on process all
        'If Not newBooking Is Nothing Then
        '    If newBooking.SolutionDetailODControl <> 0 Then
        '        If NGLLaneData.isTransLoadOn(newBooking.SolutionDetailODControl) Then
        '            'check if this is a transload facility and process the splits needed for the order
        '            'TODO:?  for 7.0 we may need to run each transload through the routing guide logic, but in 6.3 we do not do this?
        '            If LaneTransLoadXrefDetBLL.processTransLoadFacility(newBooking.SolutionDetailBookControl) Then Return
        '        End If
        '    End If
        'End If
        'If Not newBooking Is Nothing AndAlso newBooking.SolutionDetailRouteGuideControl <> 0 Then
        '    prepareToRouteLoads(New List(Of DTO.tblSolutionDetail) From {newBooking}, True)
        'Else
        '    processUnRoutedLoads(New List(Of DTO.tblSolutionDetail) From {newBooking})
        'End If
    End Sub


    ''' <summary>
    ''' Copies the selected order data from the po staging table into 
    ''' the book table and updates the book record with the provided 
    ''' load information like carrier
    ''' </summary>
    ''' <param name="OrderNumber"></param>
    ''' <param name="OrderSequence"></param>
    ''' <param name="CompNumber"></param>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="BookRouteConsFlag"></param>
    ''' <param name="BookStopNo"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="BookCarrTruckControl"></param>
    ''' <param name="BookHoldLoad"></param>
    ''' <remarks></remarks>
    Public Sub UpdateNewPOWithLoadPlanningCarrier(ByVal OrderNumber As String,
                                                  ByVal OrderSequence As Integer,
                                                  ByVal CompNumber As Integer,
                                                  ByVal BookConsPrefix As String,
                                                  ByVal BookRouteConsFlag As Boolean,
                                                  ByVal BookStopNo As Integer,
                                                  ByVal CarrierControl As Integer,
                                                  ByVal BookCarrTruckControl As Integer,
                                                  ByVal BookHoldLoad As Integer)
        Dim strNewPro As String = ""
        Dim oReturn = NGLBookData.WriteNewBookingForBatch(OrderNumber, OrderSequence, CompNumber, strNewPro)
        NGLLoadPlanningTruckData.UpdateLoadPlanningCarrier(strNewPro, BookConsPrefix, BookRouteConsFlag, BookStopNo, CarrierControl, BookCarrTruckControl, BookHoldLoad)
    End Sub

    ''' <summary>
    ''' Selects Tariff and Calculates costs for unrouted orders then
    ''' attempts to auto tender as needed.
    ''' Replaces NGLLoadPlanningTruckData.UpdateDefaultCarrierForUnRouted 
    ''' we no longer use spUpdateDefaultCarrierForUnRouted 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <remarks>
    ''' Executed as part of the order download for new orders.  the following rules are applied:
    ''' 1. the write new booking stored procedure, spWriteNewBookingForBatch, should already determine the correct
    '''    value of BookCarrierControl so we do not need to check the Lane Default Carrier Settings
    ''' 2. Lane fees are automatically inserted by spWriteNewBookingForBatch calling spInsertMissingLaneFees
    ''' 3. Call AssignOrUpdateCarrier it will determine if we need to select the lowest cost carrier
    '''    or use the default carrier assigned
    ''' 4. If AssignOrUpdateCarrier returns false or if the CarrierControl = 0 
    '''    or the CarrierCost = 0  try again to select the lowest cost carrier
    ''' 5. Assign Carrier method should update the BFC as needed (check)
    ''' 6. Assign Carrier method should recalculate all costs and save changes to book table
    ''' 7. Check if Can Auto Tender
    ''' 8. Check for errors and send emails on error.
    ''' 9. on new orders we always set the send email flag to true
    ''' 10. If AutoFinalize = true for carrier call AutoFinalize else call AutoTender
    ''' 11. Check for errors warnings or messages and generate new subscription alerts as needed. 
    ''' 
    ''' Modified by RHR v-7.0.5.110 9/27/16
    '''   added advanced error handling for this method
    '''   the caller must parse the errors, messages and warnings in WCFResults 
    '''   to identify if any subscript alert messages are required
    ''' Modified by RHR for v-7.0.5.6.0 on 11/22/2016
    '''   added logic for new preferred carrier logic with multiple default carriers
    ''' Modified by RHR for v-7.0.6.101 on 2/9/2017 
    '''   fixes bug where blnIgnoreTariff was not being updated 
    ''' Modified by RHR for v-7.0.6.103 on 02/13/2017
    '''  changed to select all non expired tariffs using lane preferred carrier settings    
    '''  Modified by RHR for v-8.5.3.001 on 05/25/2022
    '''     Added new logic to call NGLBookRevenueBLL.GenerateQuote
    '''     and NGLBookRevenueBLL.DispatchBid 
    '''     this change is to support API integration
    '''     each API configuration should be used to do rate shopping
    '''     and dispatching.
    '''     Data is written to the tblBid child tables of tblLoadTender
    '''     The new logic will read the data in tblBid and select the lowest cost carrier for dispatching.
    '''     All logs and Errors are written to tables  
    '''     currently we capture Errors, Warnings, Messages, and Logs
    '''     All of these messges are stored in the tblBidSvcErrs table when a bid is not available the system must create a zero cost record in tblbid for the carrier
    '''     errors/messages/warnings are then inserted into  tblBidSvcErrs using the bidcontrol FK
	'''     use standard localization message formatting where we have Message, Details,  With an additional field For mapping field identification 
	'''     maps to these fields in tblBidSvcErrs
	'''     BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message)
	'''     BidSvcErrVendorErrorMessage = msg.Details
	'''     BidSvcErrFieldName = msg.FieldName
    '''     (review changes to InsertNGLTariffBid) 
    ''' </remarks>
    Public Function UpdateDefaultCarrierForUnRouted(ByVal BookControl As Integer) As DTO.WCFResults


        Dim results As New DTO.WCFResults With {.Key = BookControl, .Success = True}
        If BookControl = 0 Then Return results

        results.AddLog("Read the order data")
        Dim oBookRevs = NGLBookRevenueData.GetBookRevenues(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.Success = False
            results.AddLog("BookControl Not Found: " & BookControl.ToString())
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
            'Modified by RHR v-7.0.5.110 9/27/16
            'Removed system errors this is replaced by a subscripiton alert generated by the caller
            'SaveSysError(DTO.WCFResults.getMessageNotLocalizedString(DTO.WCFResults.MessageEnum.M_NoOrdersFound), "NGLOrderImportBLL.UpdateDefaultCarrierForUnRouted", "BookControl Not Found: " & BookControl.ToString(), 0, sysErrorSeverity.Warning, sysErrorState.SystemLevelFault, 601)
            Return results
        End If
        Dim oSBook As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
        If oSBook Is Nothing OrElse oSBook.BookControl = 0 Then
            oSBook = oBookRevs(0)
        End If
        Dim intDefaultCarrier As Integer = oSBook.BookCarrierControl
        Dim vData As New LTS.spGetAutoTenderDataResult
        Dim oCarrierCostResults As New DTO.CarrierCostResults()
        Dim blnIgnoreTariff As Boolean = False
        Dim strCarrierNumber As String = "[Not Available]"
        Dim strCarrierName As String = "[Not Available]"
        results.AddLog("Read the auto tender variables and parameters")
        vData = NGLBookData.GetAutoTenderData(BookControl)
        Dim blnUsingZeroCostCarrier As Boolean = False
        Dim oSelectedDefaultCarrier As clsPreferedDefaultCarrier
        'Modified by RHR for v-7.0.5.6.0 on 11/22/2016
        If intDefaultCarrier <> 0 Then
            'Auto Assign Carrier is off and we are using the parameter settings for default carrier
            'we ignore all preferred carriers and carrier restriction rules
            Dim oCarrierNameNbr = NGLCarrierData.getCarrierNameNumber(intDefaultCarrier)
            If Not oCarrierNameNbr Is Nothing AndAlso oCarrierNameNbr.Count > 0 AndAlso oCarrierNameNbr.ContainsKey("CarrierName") Then
                strCarrierName = oCarrierNameNbr("CarrierName")
            Else
                strCarrierName = " Carrier Control Number " & intDefaultCarrier.ToString
            End If
            If Not oCarrierNameNbr Is Nothing AndAlso oCarrierNameNbr.Count > 0 AndAlso oCarrierNameNbr.ContainsKey("CarrierNumber") Then
                strCarrierNumber = oCarrierNameNbr("CarrierNumber")
            Else
                strCarrierNumber = " Carrier Control Number " & intDefaultCarrier.ToString
            End If
            blnIgnoreTariff = vData.IgnoreTariff
            If Not blnIgnoreTariff Then
                results.AddLog("Assign Default Carrier")
                'AssignOrUpdateCarrier will select lowest cost carrier if carriercontrol is 0 
                'or will select the lowest cost tariff if the BookCarrTarEquipControl is 0 
                'or it will update the carrier cost using the currently assigned tariff
                oCarrierCostResults = BookRevenueBLL.AssignOrUpdateCarrier(oSBook)
            Else
                results.AddLog("Ignore Tariff for Default Carrier")
                'set all costs to zero and assign the carrier.
                For Each b In oBookRevs
                    Dim intCarrContact As Integer = b.BookCarrierContControl
                    Dim strCarrContName As String = b.BookCarrierContact
                    Dim strCarrContPhone As String = b.BookCarrierContactPhone
                    b.ResetToNStatus()
                    'keep the carrier info
                    b.BookCarrierControl = intDefaultCarrier
                    b.BookCarrTarName = "Ignore Tariff"
                    b.BookCarrierContControl = intCarrContact
                    b.BookCarrierContact = strCarrContName
                    b.BookCarrierContactPhone = strCarrContPhone
                Next
                results = BookBLL.ProcessNewTransCode(oBookRevs, BookControl, "P", "N", results)
                If Not results.Success Then
                    oCarrierCostResults.Success = False
                    'unrecoverable failure so create a subscription alert
                    With oSBook
                        Dim strSubject As String = String.Format("Alert - Cannot Auto Assign the system parameter default carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence, strCarrierName)
                        results.AddLog(strSubject)
                        results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAssignSystemDefaultCarrier)
                        BookBLL.createAutoAssignCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, intDefaultCarrier, strCarrierNumber, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence.ToString(), .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                    End With
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                    Return results
                End If
                oCarrierCostResults.Success = True
            End If
        Else
            'Modified by RHR for v-7.0.6.0 12/29/2016
            '  added logic for capacity on zero cost carriers.
            Dim TotalCases As Integer = oBookRevs.Sum(Function(x) x.BookTotalCases)
            Dim TotalWgt As Double = oBookRevs.Sum(Function(x) x.BookTotalWgt)
            Dim TotalPlts As Double = oBookRevs.Sum(Function(x) x.BookTotalPL)
            Dim TotalCubes As Integer = oBookRevs.Sum(Function(x) x.BookTotalCube)
            'new code to select the preferred or lowest cost carrier we get here
            'when LaneDefaultCarrierUse flag is true or when The COMPBACKHAULCARRIERNO or CARRIERIMPORTNUMBER  are zero 
            '(based on LaneTransType and determined in spUpdatePOHDRDefaults )
            Dim ModeType As Integer = oSBook.BookModeTypeControl
            Dim TempType As Integer = 0
            NGLBookData.GetModeTempTypesByPrecedence(BookControl, ModeType, TempType)
            'get a list of the preferred carriers for this booking records lane 'Modified by RHR for v-7.0.6.103 on 02/13/2017
            '  changed to select all non expired tariffs using lane preferred carrier settings
            Dim oDefault As DTO.LimitLaneToCarrier() = NGLLaneData.GetLanePreferredCarrTars(oSBook.BookODControl)
            'Modified by RHR for v-8.5.3.001 on 6/1/2022 we only check for zero cost here now

            If Not oDefault Is Nothing AndAlso oDefault.Count() > 0 Then
                'check if we have any ignore tariff carriers.
                Dim oZeroCost As DTO.LimitLaneToCarrier() = (From d In oDefault
                                                             Where
                                                                d.LLTCIgnoreTariff = True _
                                                                And
                                                                (d.LLTCMaxCases = 0 OrElse TotalCases <= d.LLTCMaxCases) _
                                                                And
                                                                (d.LLTCMaxWgt = 0 OrElse TotalWgt <= d.LLTCMaxWgt) _
                                                                And
                                                                (d.LLTCMaxCube = 0 OrElse TotalCubes <= d.LLTCMaxCube) _
                                                                And
                                                                (d.LLTCMaxPL = 0 OrElse TotalPlts <= d.LLTCMaxPL)
                                                             Order By
                                                                d.LLTCSequenceNumber, d.LLTCControl
                                                             Select d).ToArray()

                If Not oZeroCost Is Nothing AndAlso oZeroCost.Count() > 0 Then
                    'we have a zero cost carrier so assign it and return
                    results.AddLog("Ignore Tariff for Default Carrier")
                    'Add code here to get the carrier contact information
                    strCarrierNumber = oZeroCost(0).LLTCCarrierNumber
                    strCarrierName = oZeroCost(0).LLTCCarrierName
                    'set all costs to zero and assign the carrier.
                    For Each b In oBookRevs
                        b.ResetToNStatus()
                        'keep the carrier info
                        b.BookCarrierControl = oZeroCost(0).LLTCCarrierControl
                        b.BookCarrTarName = "Ignore Tariff"
                        b.BookCarrierContControl = oZeroCost(0).LLTCCarrierContControl
                        b.BookCarrierContact = oZeroCost(0).CarrierContName
                        b.BookCarrierContactPhone = If(String.IsNullOrWhiteSpace(oZeroCost(0).CarrierContact800), oZeroCost(0).CarrierContactPhone, oZeroCost(0).CarrierContact800)
                    Next
                    results = BookBLL.ProcessNewTransCode(oBookRevs, BookControl, "P", "N", results)
                    If Not results.Success Then
                        oCarrierCostResults.Success = False
                        'unrecoverable failure so create a subscription alert
                        With oSBook
                            Dim strSubject As String = String.Format("Alert - Cannot Auto Assign the default carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence, strCarrierName)
                            results.AddLog(strSubject)
                            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAssignDefaultCarrier)
                            BookBLL.createAutoAssignCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, oZeroCost(0).LLTCCarrierControl, strCarrierNumber, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence.ToString(), .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                        End With
                        results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                        Return results
                    End If
                    oCarrierCostResults.Success = True
                    blnUsingZeroCostCarrier = True
                End If
            End If
            If Not blnUsingZeroCostCarrier Then

                'get a list of the available carriers for this booking record
                'Modified by RHR for v-8.5.3.001 on 05/31/2022 added logic for API Carriers,  we now call NGLBookRevenueBll.GenerateQuote 
                'this writes quotes to the Load Tender Bid table
                'we then read the bids to select the lowest cost carrier
                'TODO: performance can be imporved by passing a list of preferred carriers to NGLBookRevenueBll.GenerateQuote to miminize calls 
                '       to get rates but this would require significant programming
                'configure the bid types
                Dim bidTypes As New List(Of DTO.tblLoadTender.BidTypeEnum)
                bidTypes.Add(DTO.tblLoadTender.BidTypeEnum.P44)
                bidTypes.Add(DTO.tblLoadTender.BidTypeEnum.NGLTariff)
                'configure tariff options
                Dim tariffOptions As New DTO.GetCarriersByCostParameters(True, True, True, ModeType, TempType)
                'configure Tender Types
                Dim TenderTypes As DTO.tblLoadTender.LoadTenderTypeEnum() = {DTO.tblLoadTender.LoadTenderTypeEnum.LoadBoard}
                Dim oRet = BookRevenueBLL.GenerateQuote(Nothing, TenderTypes, bidTypes.ToArray(), BookControl, tariffOptions)
                'Removed by RHR for v-8.5.3.001 on 05/31/2022 added logic for API Carriers
                'Dim oCostResults As DTO.CarrierCostResults = TARBookRev.estimatedCarriersByCost(BookControl, 0, True, False, True, True, ModeType, TempType)

                'Modified by RHR for v-8.5.3.001 on 05/31/2022 added logic for API Carriers,  Read Bid Results using load tender 
                '  add logic to select bid, via xxx, and dispatch, via getBidToDispatch and DispatchBid
                Dim iLoadTenderControl As Integer = 0
                oRet.TryParseKeyInt("LoadTenderControl", iLoadTenderControl)
                If iLoadTenderControl = 0 Then
                    'we cannot proceed
                    results.Success = False
                    results.AddLog("No carriers were found for bookcontrol: " & BookControl.ToString())
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.W_NoAutoTenderData)
                    'Modified by RHR v-7.0.5.110 9/27/16
                    'Removed system errors this is replaced by a subscripiton alert generated by the caller
                    'SaveSysError(DTO.WCFResults.getMessageNotLocalizedString(DTO.WCFResults.MessageEnum.M_NoOrdersFound), "NGLOrderImportBLL.UpdateDefaultCarrierForUnRouted", "BookControl Not Found: " & BookControl.ToString(), 0, sysErrorSeverity.Warning, sysErrorState.SystemLevelFault, 601)
                    Return results
                End If
                Dim RecordCount As Integer = 0
                Dim filters As New DAL.Models.AllFilters With {.filterName = "BidLoadTenderControl", .filterValue = iLoadTenderControl.ToString()}
                filters.addToSort("BidTotalCost", True)
                Dim oBids As LTS.tblBid() = NGLBidData.GetActiveBidsById(RecordCount, filters) 'returns active bids greater than zero cost
                If Not oBids Is Nothing AndAlso oBids.Count() > 0 Then
                    If Not oDefault Is Nothing AndAlso oDefault.Count() > 0 Then
                        'we have some carriers available so loop through each prefered default carrier and determine if we can assign one.
                        'create a list of carriers that match 
                        Dim oDefaultMatchsFound As New List(Of clsPreferedDefaultCarrier)
                        For Each oPref In oDefault
                            'add any estimated carriers that match the preferred default settings to a list of matched default carriers
                            'TODO: we need to add logic to the code that writes to the Bid table to identify the values for
                            '   (a) LLTCModeTypeControl -- BidBookModeTypeControl? BidMode?
                            '   (b) LLTCTempType -- may map to mode like LTL/TL/Refer or even equipment.  
                            '   (c) LLTCTariffEquip  -- BidServiceType should map to the values in the carrier equipment or Carrier truck data.
                            '       using the return data from the Quote.  Some carriers may be forced to use defaults as configured in the SSOA settings
                            For Each estimated In oBids
                                If estimated.BidCarrierControl = oPref.LLTCCarrierControl _
                                    And (oPref.LLTCMaxCases = 0 OrElse TotalCases <= oPref.LLTCMaxCases) _
                                    And (oPref.LLTCMaxWgt = 0 OrElse TotalWgt <= oPref.LLTCMaxWgt) _
                                    And (oPref.LLTCMaxCube = 0 OrElse TotalCubes <= oPref.LLTCMaxCube) _
                                    And (oPref.LLTCMaxPL = 0 OrElse TotalPlts <= oPref.LLTCMaxPL) Then
                                    oDefaultMatchsFound.Add(New clsPreferedDefaultCarrier(estimated, oPref))
                                End If
                            Next
                        Next
                        If Not oDefaultMatchsFound Is Nothing AndAlso oDefaultMatchsFound.Count() > 0 Then
                            'this is the code we need to test to be sure we are selecting the most precise carrier
                            'filter by is cost allowed property formula
                            oDefaultMatchsFound = oDefaultMatchsFound.Where(Function(d) d.IsCostAllowed = True).ToList()
                            If Not oDefaultMatchsFound Is Nothing AndAlso oDefaultMatchsFound.Count() > 1 Then
                                Dim dblGlobalUseLowestCostForDefaultCarrier As Double = NGLBookData.GetParValue("GlobalUseLowestCostForDefaultCarrier", 0)
                                'if we get here and we have multiple default carriers we sort and validate the list
                                'the previous queries above validate the mode, temp, and address data here we select the preferred or lowest cost
                                'based on the configuration settings.                               
                                If dblGlobalUseLowestCostForDefaultCarrier = 1 Then
                                    oSelectedDefaultCarrier = oDefaultMatchsFound.OrderBy(Function(x) x.CarrierCost).ThenBy(Function(x) x.SequenceNumber).ThenBy(Function(x) x.Control).FirstOrDefault()
                                Else
                                    oSelectedDefaultCarrier = oDefaultMatchsFound.OrderBy(Function(x) x.CarrierCost).ThenBy(Function(x) x.SequenceNumber).ThenBy(Function(x) x.Control).FirstOrDefault()
                                End If
                            Else
                                oSelectedDefaultCarrier = oDefaultMatchsFound(0)
                            End If
                        End If
                    Else
                        'we do not have a list of default carriers so just select the lowest cost carrier from the bids
                        oSelectedDefaultCarrier = New clsPreferedDefaultCarrier(oBids(0))
                    End If
                End If
                If oSelectedDefaultCarrier Is Nothing Then
                    'None of the default carriers have a valid rate 
                    oCarrierCostResults.Success = False
                    'unrecoverable failure so create a subscription alert
                    With oSBook
                        Dim strSubject As String = String.Format("Alert - Cannot Auto Assign the default carrier for OrderNumber - Sequence: {0}-{1}.", .BookCarrOrderNumber, .BookOrderSequence)
                        results.AddLog(strSubject)
                        results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotAutoAssignDefaultCarrier)
                        BookBLL.createAutoAssignCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, 0, 0, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence.ToString(), .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                    End With
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                    Return results
                Else
                    'we update the default carrier control and use it further down
                    intDefaultCarrier = oSelectedDefaultCarrier.SelectedBid.BidCarrierControl
                    strCarrierNumber = oSelectedDefaultCarrier.SelectedBid.BidCarrierNumber
                    strCarrierName = oSelectedDefaultCarrier.SelectedBid.BidCarrierName
                    'update the selected carrier data
                    ' note:  updateSelectedCarrier calls TARBookRev.assignCarrier this does not work for API
                    '           we need a new method, NSAPIAssign that works like NSAPITender
                    '           we need to add new logic to manual tender to carrier so it also call the correct version of NSAPITender
                    '           P44 users NSP44Tender and NSP44Accept
                    '           We need to add logic to determine if we are dispatching with 204 or direct API.
                    ' Removed for v-8.5.3.001  oCarrierCostResults = BookRevenueBLL.updateSelectedCarrier(BookControl, oSelectedDefaultCarrier.CarriersByCost, Nothing, Nothing)
                    'TODO: call NGLBookRevenueBLL.DispatchBid
                    '!!! here we just need to assign the carrier we dispatch it later

                    Select Case oSelectedDefaultCarrier.SelectedBid.BidBidTypeControl
                        Case DTO.tblLoadTender.BidTypeEnum.NGLTariff
                            Dim SelectedCarrier = New DTO.CarriersByCost() With {.CarrierControl = oSelectedDefaultCarrier.SelectedBid.BidCarrierControl, .BookCarrTarEquipMatControl = oSelectedDefaultCarrier.SelectedBid.BidBookCarrTarEquipMatControl, .BookCarrTarEquipControl = oSelectedDefaultCarrier.SelectedBid.BidBookCarrTarEquipControl, .BookModeTypeControl = oSelectedDefaultCarrier.SelectedBid.BidBookModeTypeControl}
                            oRet = NGLDATBLL.NSNGLTariffAssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, SelectedCarrier, 0)
                        Case DTO.tblLoadTender.BidTypeEnum.P44
                            oRet = NGLDATBLL.NSP44AssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, oSelectedDefaultCarrier, 0)
                        Case DTO.tblLoadTender.BidTypeEnum.CHRAPI
                            oRet = NGLDATBLL.NSAPIAssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, oSelectedDefaultCarrier, 0)
                        Case DTO.tblLoadTender.BidTypeEnum.JTSAPI
                            oRet = NGLDATBLL.NSAPIAssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, oSelectedDefaultCarrier, 0)
                        Case DTO.tblLoadTender.BidTypeEnum.UPSAPI
                            oRet = NGLDATBLL.NSAPIAssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, oSelectedDefaultCarrier, 0)
                        Case Else
                            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidParameterNameValue", New String() {"Load Tender Bid Type", oSelectedDefaultCarrier.SelectedBid.BidBidTypeControl})
                            SaveAppError(String.Concat("Invalid Parameter: No record exists in the database for  Load Tender Bid Type ", oSelectedDefaultCarrier.SelectedBid.BidBidTypeControl))
                            Return oRet
                    End Select
                    'Dim oDispatch As DAL.Models.Dispatch = NGLLoadTenderData.getBidToDispatch(oSelectedDefaultCarrier.SelectedBid.BidControl)
                    'Dim oRes As DTO.WCFResults = BookRevenueBLL.DispatchBid(oDispatch, DTO.tblLoadTender.LoadTenderTypeEnum.LoadBoard, Nothing, oSelectedDefaultCarrier.SelectedBid.BidControl)

                End If
            End If
        End If
        results.AddLog("Prepare to auto tender and auto accept the load")
        '  Modified by RHR for v-8.5.3.001 on 06/13/2022
        results.AddLog("Get the booking records back after assignment")
        oBookRevs = NGLBookRevenueData.GetBookRevenues(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.Success = False
            results.AddLog("BookControl Not Found: " & BookControl.ToString())
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
            'Modified by RHR v-7.0.5.110 9/27/16
            'we do not add system errors instead the caller will generate a subscription alert
            'SaveSysError(DTO.WCFResults.getMessageNotLocalizedString(DTO.WCFResults.MessageEnum.M_NoOrdersFound), "NGLOrderImportBLL.UpdateDefaultCarrierForUnRouted", "BookControl Not Found: " & BookControl.ToString, 0, sysErrorSeverity.Warning, sysErrorState.SystemLevelFault, 601)
            Return results
        End If
        oSBook = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
        If oSBook Is Nothing OrElse oSBook.BookControl = 0 Then
            oSBook = oBookRevs(0)
        End If
        results.AddLog("Get the updated carrier info")
        Dim intAssignedCarrier = oSBook.BookCarrierControl
        If intAssignedCarrier = 0 Then
            results.Success = False
            results.AddLog("A carrier has not been assigned for order number: " & oSBook.BookCarrOrderNumber)
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
            Return results
        End If
        strCarrierNumber = "[Not Available]"
        strCarrierName = "[Not Available]"
        Dim odic = NGLCarrierData.getCarrierNameNumber(intAssignedCarrier)
        If Not odic Is Nothing AndAlso odic.Count > 0 AndAlso odic.ContainsKey("CarrierName") Then
            strCarrierName = odic("CarrierName")
        Else
            strCarrierName = " Carrier Control Number " & intAssignedCarrier.ToString
        End If
        If Not odic Is Nothing AndAlso odic.Count > 0 AndAlso odic.ContainsKey("CarrierNumber") Then
            strCarrierNumber = odic("CarrierNumber")
        Else
            strCarrierNumber = " Carrier Control Number " & intAssignedCarrier.ToString
        End If
        vData = NGLBookData.GetAutoTenderData(BookControl)
        Dim blnCanAutoTender As Boolean = BookBLL.CanAutoTender(BookControl, oBookRevs, vData, results)
        If Not blnCanAutoTender Then
            With oSBook
                Dim strSubject As String = String.Format("Message - Cannot auto tender a load because the configuration does not allow auto tender with carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence, strCarrierName)
                results.AddLog(strSubject)
                BookBLL.createAutoTenderConfigurationNotAllowedSubscriptionAlert(strSubject, .BookCustCompControl, .BookCarrierControl, strCarrierNumber, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence, .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                If Not oSBook Is Nothing AndAlso oSBook.BookControl <> 0 AndAlso oSBook.BookCustCompControl <> 0 Then NGLBookRevenueData.LockBFCOnImport(oSBook.BookControl, oSBook.BookCustCompControl)
                Return results
            End With
        End If
        Dim blnCanAutoAccept As Boolean = False
        If Not vData Is Nothing AndAlso vData.AutoFinalize Then
            blnCanAutoAccept = BookBLL.AutoAccept(BookControl, oBookRevs, results, strCarrierName, True)            '
            If Not BookBLL.AutoAccept(BookControl, oBookRevs, results, strCarrierName, True) Then
                'we have a problem and cannot auto accept so generate the subscription alert
                With oSBook
                    Dim strSubject As String = String.Format("Alert - Cannot auto finalize a load with carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence, strCarrierName)
                    results.AddLog(strSubject)
                    BookBLL.createAutoFinalizeCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, .BookCarrierControl, strCarrierNumber, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence, .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                    If Not oSBook Is Nothing AndAlso oSBook.BookControl <> 0 AndAlso oSBook.BookCustCompControl <> 0 Then NGLBookRevenueData.LockBFCOnImport(oSBook.BookControl, oSBook.BookCustCompControl)
                End With
            End If
        End If
        If blnUsingZeroCostCarrier Or blnIgnoreTariff Then
            If Not vData.IgnoreTariff Then
                results.Success = False
                results.AddLog(String.Format("The carrier {0} is not compatible with the expected ignore tariff setting for order number {1} ", strCarrierName, oSBook.BookCarrOrderNumber))
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                Return results
            End If
            results.AddLog("The carrier is configured to ignore the tariff")
            If Not BookBLL.AutoTender(BookControl, oBookRevs, results, strCarrierName, True) Then
                'we have a problem and cannot auto tender so generate the subscription alert
                With oSBook
                    Dim strSubject As String = String.Format("Alert - Cannot auto tender a load with carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence, strCarrierName)
                    results.AddLog(strSubject)
                    BookBLL.createAutoTenderCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, .BookCarrierControl, strCarrierNumber, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence, .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                    If Not oSBook Is Nothing AndAlso oSBook.BookControl <> 0 AndAlso oSBook.BookCustCompControl <> 0 Then NGLBookRevenueData.LockBFCOnImport(oSBook.BookControl, oSBook.BookCustCompControl)
                    Return results
                End With
            End If
        Else
            If oBookRevs.Any(Function(x) x.BookRevCarrierCost <= 0) Then
                With oSBook
                    Dim strMsg As String = "The carrier cost is not valid "
                    If intAssignedCarrier <> 0 Then
                        strMsg &= " for the assigned carrier: " & strCarrierName
                    End If
                    Dim strSubject As String = String.Format("Alert - Cannot Auto Assign the lowest cost carrier for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence)
                    results.AddLog(strSubject)
                    results.AddLog(strMsg)
                    BookBLL.createAutoAssignLowestCostCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence.ToString(), .BookConsPrefix, "", "", strMsg)
                End With
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                results.Success = False
                If Not oSBook Is Nothing AndAlso oSBook.BookControl <> 0 AndAlso oSBook.BookCustCompControl <> 0 Then NGLBookRevenueData.LockBFCOnImport(oSBook.BookControl, oSBook.BookCustCompControl)
                Return results
            End If
            If Not oSelectedDefaultCarrier Is Nothing AndAlso Not oSelectedDefaultCarrier.SelectedBid Is Nothing AndAlso oSelectedDefaultCarrier.SelectedBid.BidControl <> 0 Then
                Dim oDispatch As DAL.Models.Dispatch = NGLLoadTenderData.getBidToDispatch(oSelectedDefaultCarrier.SelectedBid.BidControl)
                oDispatch.AutoAcceptOnDispatch = blnCanAutoAccept
                Dim oRes As DTO.WCFResults = BookRevenueBLL.DispatchBid(oDispatch, DTO.tblLoadTender.LoadTenderTypeEnum.LoadBoard, Nothing, oSelectedDefaultCarrier.SelectedBid.BidControl)

            Else
                If Not BookBLL.AutoTender(BookControl, oBookRevs, results, strCarrierName, True) Then
                    'we have a problem and cannot auto tender so generate the subscription alert
                    With oSBook
                        Dim strSubject As String = String.Format("Alert - Cannot auto tender a load with carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .BookCarrOrderNumber, .BookOrderSequence, strCarrierName)
                        results.AddLog(strSubject)
                        BookBLL.createAutoTenderCarrierFailedSubscriptionAlert(strSubject, .BookCustCompControl, .BookCarrierControl, strCarrierNumber, .BookProNumber, .BookSHID, .BookCarrOrderNumber, .BookOrderSequence, .BookConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                        If Not oSBook Is Nothing AndAlso oSBook.BookControl <> 0 AndAlso oSBook.BookCustCompControl <> 0 Then NGLBookRevenueData.LockBFCOnImport(oSBook.BookControl, oSBook.BookCustCompControl)
                        Return results
                    End With
                End If
            End If
        End If
        If Not oSBook Is Nothing AndAlso oSBook.BookControl <> 0 AndAlso oSBook.BookCustCompControl <> 0 Then NGLBookRevenueData.LockBFCOnImport(oSBook.BookControl, oSBook.BookCustCompControl)
        'TODO: Add logic to save logs and messages to database.

        Return results
    End Function

    Public Function UpdateDefaultCarrierForUnRoutedSpeedtest(ByRef oSolutionDetail As DTO.tblSolutionDetail) As DTO.WCFResults

        Dim results As New DTO.WCFResults With {.Key = oSolutionDetail.SolutionDetailBookControl, .Success = True}
        results.AddLog("Read the order data")
        'Dim oBookRevs = NGLBookRevenueData.GetBookRevenues(BookControl)
        'If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
        '    results.Success = False
        '    results.AddLog("BookControl Not Found: " & BookControl.ToString())
        '    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
        '    SaveSysError(DTO.WCFResults.getMessageNotLocalizedString(DTO.WCFResults.MessageEnum.M_NoOrdersFound), "NGLOrderImportBLL.UpdateDefaultCarrierForUnRouted", "BookControl Not Found: " & BookControl.ToString(), 0, sysErrorSeverity.Warning, sysErrorState.SystemLevelFault, 601)
        '    Return results
        'End If
        'Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
        'If oSelectedBooking Is Nothing OrElse oSelectedBooking.BookControl = 0 Then
        '    oSelectedBooking = oBookRevs(0)
        'End If
        Dim intDefaultCarrier As Integer = oSolutionDetail.SolutionDetailCarrierControl
        Dim vData As New LTS.spGetAutoTenderDataResult
        Dim oCarrierCostResults As New DTO.CarrierCostResults()
        Dim blnIgnoreTariff As Boolean = False
        Dim strCarrierNumber As String = "[Not Available]"
        Dim strCarrierName As String = "[Not Available]"
        If intDefaultCarrier <> 0 Then
            results.AddLog("Read the auto tender variables and parameters")
            vData = NGLBookData.GetAutoTenderData(oSolutionDetail.SolutionDetailBookControl)
            Dim odic = NGLCarrierData.getCarrierNameNumber(intDefaultCarrier)
            If Not odic Is Nothing AndAlso odic.Count > 0 AndAlso odic.ContainsKey("CarrierName") Then
                strCarrierName = odic("CarrierName")
            Else
                strCarrierName = " Carrier Control Number " & intDefaultCarrier.ToString
            End If
            If Not odic Is Nothing AndAlso odic.Count > 0 AndAlso odic.ContainsKey("CarrierNumber") Then
                strCarrierNumber = odic("CarrierNumber")
            Else
                strCarrierNumber = " Carrier Control Number " & intDefaultCarrier.ToString
            End If
            blnIgnoreTariff = vData.IgnoreTariff
            If Not blnIgnoreTariff Then
                results.AddLog("Assign Default Carrier")
                'AssignOrUpdateCarrier will select lowest cost carrier if carriercontrol is 0 
                'or will select the lowest cost tariff if the BookCarrTarEquipControl is 0 
                'or it will update the carrier cost using the currently assigned tariff
                oCarrierCostResults = BookRevenueBLL.AssignOrUpdateCarrier(oSolutionDetail.SolutionDetailBookControl, False)
            Else
                'this should not be needed for new records?
                results.AddLog("Ignore Tariff for Default Carrier")
                'set all costs to zero and assign the carrier.
                'For Each b In oBookRevs
                '    Dim intCarrContact As Integer = b.BookCarrierContControl
                '    Dim strCarrContName As String = b.BookCarrierContact
                '    Dim strCarrContPhone As String = b.BookCarrierContactPhone
                '    b.ResetToNStatus()
                '    'keep the carrier info
                '    b.BookCarrierControl = intDefaultCarrier
                '    b.BookCarrierContControl = intCarrContact
                '    b.BookCarrierContact = strCarrContName
                '    b.BookCarrierContactPhone = strCarrContPhone
                'Next
                results = BookBLL.ProcessNewTransCode(oSolutionDetail.SolutionDetailBookControl, "P", "N")
                If Not results.Success Then
                    oCarrierCostResults.Success = False
                    'unrecoverable failure so create a subscription alert
                    With oSolutionDetail
                        Dim strSubject As String = String.Format("Alert - Cannot Auto Assign the default carrier, {2}, for OrderNumber - Sequence: {0}-{1}", .SolutionDetailOrderNumber, .SolutionDetailOrderSequence, strCarrierName)
                        results.AddLog(strSubject)
                        BookBLL.createAutoAssignCarrierFailedSubscriptionAlert(strSubject, .SolutionDetailCompControl, intDefaultCarrier, strCarrierNumber, .SolutionDetailProNumber, .SolutionDetailBookSHID, .SolutionDetailOrderNumber, .SolutionDetailOrderSequence.ToString(), .SolutionDetailConsPrefix, results.concatErrors(), results.concatWarnings(), results.concatMessage())
                    End With
                    results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                    Return results
                End If
                oCarrierCostResults.Success = True
            End If
        Else
            'get the lowest cost carrier
            oCarrierCostResults = BookRevenueBLL.AssignOrUpdateCarrier(oSolutionDetail.SolutionDetailBookControl)
        End If
        'Test for errors from the default carrier assignment
        If oCarrierCostResults.Success = False Then
            Dim sbCarrierMessages As New System.Text.StringBuilder()
            If Not oCarrierCostResults.CarriersByCost Is Nothing AndAlso oCarrierCostResults.CarriersByCost.Count > 0 Then
                For Each c In oCarrierCostResults.CarriersByCost
                    If Not c.Messages Is Nothing AndAlso c.Messages.Count > 0 Then
                        c.concatMessage(sbCarrierMessages)
                    End If
                Next
            End If
            oCarrierCostResults.concatMessage(sbCarrierMessages)
            If intDefaultCarrier = 0 Then
                'the system could not assign the lowest cost carrier
                With oSolutionDetail
                    Dim strSubject As String = String.Format("Alert - Cannot Auto Assign the lowest cost carrier for OrderNumber - Sequence: {0}-{1}", .SolutionDetailOrderNumber, .SolutionDetailOrderSequence)
                    results.AddLog(strSubject)
                    BookBLL.createAutoAssignLowestCostCarrierFailedSubscriptionAlert(strSubject, .SolutionDetailCompControl, .SolutionDetailProNumber, .SolutionDetailBookSHID, .SolutionDetailOrderNumber, .SolutionDetailOrderSequence.ToString(), .SolutionDetailConsPrefix, "", "", sbCarrierMessages.ToString())
                End With
                results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                results.Success = False
                Return results
            Else
                'the system could not assign the default carrier cost
                'but if Cascading Dispatching is on we will try to get the next lowest cost carrier
                'but first trigger the alert for default carrier failed
                With oSolutionDetail
                    Dim strSubject As String = String.Format("Alert - Cannot Auto Assign the default carrier for OrderNumber - Sequence: {0}-{1}", .SolutionDetailOrderNumber, .SolutionDetailOrderSequence)
                    results.AddLog(strSubject)
                    BookBLL.createAutoAssignDefaultCarrierFailedSubscriptionAlert(strSubject, .SolutionDetailCompControl, intDefaultCarrier, strCarrierNumber, .SolutionDetailProNumber, .SolutionDetailBookSHID, .SolutionDetailOrderNumber, .SolutionDetailOrderSequence.ToString(), .SolutionDetailConsPrefix, "", "", sbCarrierMessages.ToString())
                End With
                'now try to get the next lowest cost carrier
                If vData.LaneCascadingDispatchingFlag And vData.AutoTenderCascadingDispatching And vData.AutoTenderCascadingDispatchingDepth > 0 Then
                    'Dim lbookControl As List(Of Integer) = oBookRevs.Select(Function(x) x.BookControl).Distinct().ToList()
                    Dim lbookControl As New List(Of Integer)
                    lbookControl.Add(oSolutionDetail.SolutionDetailBookControl)
                    Dim lRestrictedCarriers = NGLBookData.getPreviouslyTenderedCarriers(lbookControl)
                    oCarrierCostResults = BookRevenueBLL.AssignCarrier(oSolutionDetail.SolutionDetailBookControl, 0, lRestrictedCarriers)
                    If oCarrierCostResults.Success = False Then
                        'unrecoverable failure so create a subscription alert
                        sbCarrierMessages = New System.Text.StringBuilder()
                        If Not oCarrierCostResults.CarriersByCost Is Nothing AndAlso oCarrierCostResults.CarriersByCost.Count > 0 Then
                            For Each c In oCarrierCostResults.CarriersByCost
                                If Not c.Messages Is Nothing AndAlso c.Messages.Count > 0 Then
                                    c.concatMessage(sbCarrierMessages)
                                End If
                            Next
                        End If
                        oCarrierCostResults.concatMessage(sbCarrierMessages)
                        With oSolutionDetail
                            Dim strSubject As String = String.Format("Alert - Cannot Auto Assign the lowest cost carrier for OrderNumber - Sequence: {0}-{1}", .SolutionDetailOrderNumber, .SolutionDetailOrderSequence)
                            results.AddLog(strSubject)
                            BookBLL.createAutoAssignLowestCostCarrierFailedSubscriptionAlert(strSubject, .SolutionDetailCompControl, .SolutionDetailProNumber, .SolutionDetailBookSHID, .SolutionDetailOrderNumber, .SolutionDetailOrderSequence.ToString(), .SolutionDetailConsPrefix, "", "", sbCarrierMessages.ToString())
                        End With
                        results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Warnings, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts)
                        results.Success = False
                        Return results
                    End If
                End If
            End If
        End If
        results.AddLog("Get the book records back")
        Dim oBookRevs = NGLBookRevenueData.GetBookRevenues(oSolutionDetail.SolutionDetailBookControl)
      
        Return results
    End Function

    ''' <summary>
    ''' Do not use: Deprecated; We now use the FM Task Service to call ProcessNewBookWaiting 
    ''' </summary>
    ''' <param name="lOrders"></param>
    ''' <param name="OptimizeCapacity"></param>
    ''' <remarks>
    ''' Called by TryToRouteLoadAsync on seperate thread
    ''' Deprecated by RHR for v-7.0.5.110 9/27/16
    '''   We now use the FM Task Service to call ProcessNewBookWaiting
    ''' </remarks>
   



    ''' <summary>
    ''' This method is designed to be called directly by the FM Task Service v-7.0.6.0
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.110 9/27/16
    '''   new method to process new bookings waiting to be processed
    '''   should only be called by the task service
    ''' Modified by RHR for Cabot Hot fix 9/17/2020
    '''     added for loop to process up to 100 new orders in each batch
    ''' Modified by RHR for v-8.2.0.007 on 04/15/2021
    '''     added Verbos and Debug flags and new ability to add logs to tblLog
    '''     we now Call ProcessNewBookWaitingWithResults so the service can 
    '''     log results that are greater than zero
    ''' </remarks>
    Public Sub ProcessNewBookWaiting(Optional ByVal Verbos As Boolean = False, Optional ByVal Debug As Boolean = False)
        Dim iResults = ProcessNewBookWaitingWithResults(Verbos, Debug)
    End Sub

    ''' <summary>
    ''' This method is designed to be called directly by the FM Task Service v-7.0.6.0
    ''' </summary>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.110 9/27/16
    '''   new method to process new bookings waiting to be processed
    '''   should only be called by the task service
    ''' Modified by RHR for Cabot Hot fix 9/17/2020
    '''     added for loop to process up to 100 new orders in each batch
    ''' Modified by RHR for v-8.2.0.007 on 04/15/2021
    '''     added Verbos and Debug flags and new ability to add logs to tblLog     
    ''' </remarks>
    Public Function ProcessNewBookWaitingWithResults(Optional ByVal Verbos As Boolean = False, Optional ByVal Debug As Boolean = False) As Integer

        Dim iBookings As Integer = 0
        Dim iRouted As Integer = 0
        'note: the Process New Book Waiting procedure is designed as an un-attended execution procedure
        'all exceptions and errors must be processed and handled so as not to break the calling service
        Dim sAlertMsg As String = ""
        Dim CompControl As Integer = 0
        Dim BookProNumber As String = "N/A"
        Dim OrderNumber As String = "N/A"
        Dim OrderSequence As String = "0"
        Try
            For iBookings = 0 To 100  'Modified by RHR for Cabot Hot fix 9/17/2020
                Dim newBooking = NGLNewBookingsForSolutionData.GetNextNewBookWaitingToProcess()
                If Not newBooking Is Nothing Then
                    CompControl = newBooking.SolutionDetailCompControl
                    BookProNumber = newBooking.SolutionDetailProNumber
                    OrderNumber = newBooking.SolutionDetailOrderNumber
                    OrderSequence = newBooking.SolutionDetailOrderSequence
                    If newBooking.SolutionDetailODControl <> 0 Then
                        If NGLLaneData.isTransLoadOn(newBooking.SolutionDetailODControl) Then
                            'check if this is a transload facility and process the splits needed for the order
                            'TODO:?  for 7.0 we may need to run each transload through the routing guide logic, but in 6.3 we do not do this?
                            If LaneTransLoadXrefDetBLL.processTransLoadFacility(newBooking.SolutionDetailBookControl) Then Return iBookings
                        End If
                    End If
                Else
                    Exit For
                End If
                If Verbos Or Debug Then NGLBookData.Log("Processing New Booking number " & iBookings.ToString() & " of 100 for Order Number " & OrderNumber)
                Dim strNotLoadedErrors As New List(Of String)
                Dim strUnexpectedErrors As New List(Of String)
                Dim blnOrderRouted = False
                'Modified by RHR for v-8.5.3.007 on 06/20/2023
                Dim blnGetNewMiles As Boolean = False
                'If Not newBooking Is Nothing AndAlso newBooking.SolutionDetailRouteGuideControl <> 0 Then
                If Not newBooking Is Nothing AndAlso Not String.IsNullOrWhiteSpace(newBooking.SolutionDetailRouteGuideNumber) Then
                    blnOrderRouted = NGLLoadPlanningTruckData.RouteLoad(newBooking, strUnexpectedErrors, strNotLoadedErrors, True, blnGetNewMiles)
                End If


                If Not blnOrderRouted Then
                    If Verbos Or Debug Then NGLBookData.Log("Could not route Order Number " & OrderNumber & " using default carrier settings")
                    'if we cannot route the order call processUnRoutedLoads which calls UpdateDefaultCarrierForUnRouted
                    processUnRoutedLoads(newBooking, strUnexpectedErrors)
                Else
                    'Modified by RHR for v-8.5.3.007 on 06/20/2023
                    If blnGetNewMiles Then
                        Dim oBLL As NGLTMS365BLL = New NGLTMS365BLL(Parameters)
                        Dim oData As NGL.FreightMaster.Data.Models.ResultObject = oBLL.StopResequence(newBooking.SolutionDetailBookControl, True)
                    End If
                    If Debug Then NGLBookData.Log("Order Number " & OrderNumber & " has been routed")
                    iRouted = iRouted + 1
                End If

                If Not strNotLoadedErrors Is Nothing AndAlso strNotLoadedErrors.Count > 0 Then
                    sAlertMsg &= String.Join("<br />" & vbCrLf, strNotLoadedErrors.ToArray())
                    If Debug Then NGLBookData.Log("Order Number " & OrderNumber & " had some not loaded errors")
                End If
                If Not strUnexpectedErrors Is Nothing AndAlso strUnexpectedErrors.Count > 0 Then
                    sAlertMsg &= String.Join("<br />" & vbCrLf, strUnexpectedErrors.ToArray())
                    If Debug Then NGLBookData.Log("Order Number " & OrderNumber & " had some unexpected errors")

                End If

            Next
            Dim sProcess As String = iBookings.ToString()
            If iBookings > 0 Then
                sProcess = (iBookings - 1).ToString()
            End If
            NGLBookData.Log(sProcess & " New Bookings Were Processed and " & iRouted.ToString() & " were Routed")

        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            sAlertMsg = ex.Detail.ToString(ex.Reason.ToString())
        Catch ex As Exception
            sAlertMsg = ex.Message
        Finally
            Try
                If Not String.IsNullOrWhiteSpace(sAlertMsg) Then
                    createProcessNewBookingFailedSubscriptionAlert("Process New Book Waiting Procedure Failed", CompControl, "One or more of the required steps for imporing a new booking order could not be completed.  Please check the order and manually correct any problems like default carrier or routing configurations", BookProNumber, OrderNumber, OrderSequence, sAlertMsg, "", "")
                End If
            Catch ex As Exception
                'ignore any errors when generating alerts
                If Verbos Or Debug Then NGLBookData.Log("Create Process New Booking Failed Subscription Alert Error:  " & ex.Message)
            End Try

        End Try
        Return iBookings
    End Function


    ''' <summary>
    ''' Process loads that are not configured to use the routing guide and update the default carrier information
    ''' typically this will assign the default carrier and calculate costs
    ''' </summary>
    ''' <param name="lOrder"></param>
    ''' <param name="strUnexpectedErrors"></param>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.5.110 9/28/16
    '''   we no longer use TryToRouteLoadAsync
    '''   lOrders was changed from a list to a single record because we
    '''   process one record at a time 
    '''   we now require a reference to the strUnexpectedErrors list
    '''   this method in now used exclusively by the ProcessNewBookWaiting which 
    '''   uses the strUnexpectedErrors to generate subscription alerts on failure
    '''   we no-longer catch unexpcted error here the caller ProcessNewBookWaiting must 
    '''   catch and process any unexpected errors
    ''' </remarks>
    Public Sub processUnRoutedLoads(ByRef lOrder As DTO.tblSolutionDetail,
                              ByRef strUnexpectedErrors As List(Of String))
        If lOrder Is Nothing OrElse lOrder.SolutionDetailBookControl = 0 Then Return 'nothing to do

        Try
            Dim oResult = UpdateDefaultCarrierForUnRouted(lOrder.SolutionDetailBookControl)
            'we only check for errors or messages if the results are false
            If oResult.Success = False Then
                'add any errors, warnings or messages to the list for subscription alert messages
                If Not oResult.Errors Is Nothing AndAlso oResult.Errors.Count > 0 Then
                    strUnexpectedErrors.Add(oResult.concatErrors() & "<br />" & vbCrLf)
                End If
                If Not oResult.Warnings Is Nothing AndAlso oResult.Warnings.Count > 0 Then
                    strUnexpectedErrors.Add(oResult.concatWarnings() & "<br />" & vbCrLf)
                End If
                If Not oResult.Messages Is Nothing AndAlso oResult.Messages.Count > 0 Then
                    strUnexpectedErrors.Add(oResult.concatMessage() & "<br />" & vbCrLf)
                End If
            End If
            'UpdateDefaultCarrierForUnRoutedSpeedtest(o)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            strUnexpectedErrors.Add(ex.Detail.ToString(ex.Reason.ToString()) & "<br />" & vbCrLf)
        Catch ex As Exception
            strUnexpectedErrors.Add(ex.Message & "<br />" & vbCrLf)
        End Try

    End Sub

 
    Public Function getPOHDRWDetails(ByVal ordernumber As String, ByVal orderSequence As Integer, ByVal customerNumber As Integer) As DTO.POHdr
        Dim podhr As DTO.POHdr = NGLPOHdrData.GetPOHdrFiltered(ordernumber, orderSequence, customerNumber)
        If podhr Is Nothing Then Return Nothing
        Dim items As DTO.POItem() = NGLPOItemsData.GetPOItemsFiltered(ordernumber, orderSequence, customerNumber)
        If items Is Nothing Then Return Nothing
        podhr.POItems = items.ToList
        Return podhr
    End Function

    Public Function GetImportChanges(ByVal orderNumber As String, ByVal sequenceNumber As Integer, ByVal customerNumber As Integer) As DTO.BookChanged
        If String.IsNullOrEmpty(orderNumber) Then
            Return New DTO.BookChanged
        End If
        'get the customerControl from companyNumber.
        Dim comp As DTO.Comp = NGLCompData.GetCompFiltered(0, customerNumber, "")
        If comp Is Nothing Then Return New DTO.BookChanged
        'Dim bookrecord As DTO.BookRevenue = BookRevenueBLL.GetBookRevenueWDetails(orderNumber, sequenceNumber, comp.CompControl)
        'If bookrecord Is Nothing OrElse bookrecord.BookControl = 0 Then Return New DTO.BookChanged

        Dim bookrecord As DTO.Book = NGLBookData.GetBookFiltered(BookCarrOrderNumber:=orderNumber, BookOrderSequence:=sequenceNumber)
        If bookrecord Is Nothing OrElse bookrecord.BookControl = 0 Then Return New DTO.BookChanged

        Dim podrRecord As DTO.POHdr = OrderImportBLL.getPOHDRWDetails(orderNumber, sequenceNumber, customerNumber)
        If podrRecord Is Nothing Then Return New DTO.BookChanged

        Dim laneNumber As String = NGLLaneData.GetLaneNumber(bookrecord.BookODControl)
        Dim compNo As Integer = NGLCompData.GetCompNumber(bookrecord.BookCustCompControl)


        Dim poChanges As New DTO.BookChanges
        poChanges.BookTotalCases = If(podrRecord.POHDRQty.HasValue, podrRecord.POHDRQty, 0)
        ' poChanges.BookTotalItems = If(podrRecord.POItems Is Nothing, 0, podrRecord.POItems.Count)
        poChanges.BookTotalPL = If(podrRecord.POHDRPallets.HasValue, podrRecord.POHDRPallets, 0)
        poChanges.BookTotalWgt = If(podrRecord.POHDRWgt.HasValue, podrRecord.POHDRWgt, 0)
        poChanges.BookDateLoad = podrRecord.POHDRShipdate
        '**NEW FIELDS**
        poChanges.BookDateOrdered = podrRecord.POHDRPOdate
        poChanges.CompanyNumber = podrRecord.POHDRDefaultCustomer
        poChanges.BookDateRequired = podrRecord.POHDRReqDate
        poChanges.BookCarrOrderNumber = podrRecord.POHDROrderNumber
        poChanges.BookTransType = podrRecord.POHDRFrt
        poChanges.BookRevTotalCost = podrRecord.POHDRTotalCost
        If podrRecord.POHDRStatusFlag = 4 Then
            poChanges.BookOrigCity = podrRecord.POHDROrigCity
            poChanges.BookOrigState = podrRecord.POHDROrigState
            poChanges.BookOrigCountry = podrRecord.POHDROrigCountry
            poChanges.BookOrigZip = podrRecord.POHDROrigZip
            poChanges.BookOrigAddress1 = podrRecord.POHDROrigAddress1
            poChanges.BookDestCity = podrRecord.POHDRDestCity
            poChanges.BookDestState = podrRecord.POHDRDestState
            poChanges.BookDestCountry = podrRecord.POHDRDestCountry
            poChanges.BookDestZip = podrRecord.POHDRDestZip
            poChanges.BookDestAddress1 = podrRecord.POHDRDestAddress1
        Else
            'compare to lane values
        End If
        poChanges.BookMustLeaveByDateTime = podrRecord.POHDRMustLeaveByDateTime
        poChanges.BookProNumber = podrRecord.POHDRPRONumber
        poChanges.BookLoadCom = podrRecord.POHDRTemp
        poChanges.LaneNumber = podrRecord.POHDRvendor
        poChanges.BookModeTypeControl = podrRecord.POHDRModeTypeControl
        poChanges.BookUser1 = podrRecord.POHDRUser1
        poChanges.BookUser2 = podrRecord.POHDRUser2
        poChanges.BookUser3 = podrRecord.POHDRUser3
        poChanges.BookUser4 = podrRecord.POHDRUser4


        Dim boChanges As New DTO.BookChanges
        boChanges.BookTotalCases = bookrecord.BookTotalCases
        'boChanges.BookTotalItems = bookrecord.BookLoads(0).BookItems.Count
        boChanges.BookTotalPL = bookrecord.BookTotalPL
        boChanges.BookTotalWgt = bookrecord.BookTotalWgt
        boChanges.BookDateLoad = bookrecord.BookDateLoad
        '**NEW FIELDS**
        boChanges.BookDateOrdered = bookrecord.BookDateOrdered
        boChanges.CompanyNumber = compNo.ToString()
        boChanges.BookDateRequired = bookrecord.BookDateRequired
        boChanges.BookCarrOrderNumber = bookrecord.BookCarrOrderNumber
        boChanges.BookTransType = bookrecord.BookTransType
        boChanges.BookRevTotalCost = bookrecord.BookRevTotalCost
        If podrRecord.POHDRStatusFlag = 4 Then
            boChanges.BookOrigCity = bookrecord.BookOrigCity
            boChanges.BookOrigState = bookrecord.BookOrigState
            boChanges.BookOrigCountry = bookrecord.BookOrigCountry
            boChanges.BookOrigZip = bookrecord.BookOrigZip
            boChanges.BookOrigAddress1 = bookrecord.BookOrigAddress1
            boChanges.BookDestCity = bookrecord.BookDestCity
            boChanges.BookDestState = bookrecord.BookDestState
            boChanges.BookDestCountry = bookrecord.BookDestCountry
            boChanges.BookDestZip = bookrecord.BookDestZip
            boChanges.BookDestAddress1 = bookrecord.BookDestAddress1
        Else
            'compare to lane values
        End If
        boChanges.BookMustLeaveByDateTime = bookrecord.BookMustLeaveByDateTime
        boChanges.BookProNumber = bookrecord.BookProNumber
        boChanges.BookLoadCom = bookrecord.BookLoads(0).BookLoadCom
        boChanges.LaneNumber = laneNumber
        boChanges.BookModeTypeControl = bookrecord.BookModeTypeControl
        boChanges.BookUser1 = bookrecord.BookUser1
        boChanges.BookUser2 = bookrecord.BookUser2
        boChanges.BookUser3 = bookrecord.BookUser3
        boChanges.BookUser4 = bookrecord.BookUser4


        Dim changed As New DTO.BookChanged
        changed.BookChanges = boChanges
        changed.POChanges = poChanges
        If Not poChanges.BookTotalCases = boChanges.BookTotalCases Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookTotalCases = True
        End If
        If Not poChanges.BookDateLoad = boChanges.BookDateLoad Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookDateLoad = True
        End If
        'If Not poChanges.BookTotalItems = boChanges.BookTotalItems Then
        '    changed.AreThereChanges = True
        'End If
        If Not poChanges.BookTotalPL = boChanges.BookTotalPL Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookTotalPL = True
        End If
        If Not poChanges.BookTotalWgt = boChanges.BookTotalWgt Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookTotalWgt = True
        End If
        If Not poChanges.BookDateOrdered = boChanges.BookDateOrdered Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookDateOrdered = True
        End If
        If Not poChanges.CompanyNumber = boChanges.CompanyNumber Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedCompanyNumber = True
        End If
        If Not poChanges.BookDateRequired = boChanges.BookDateRequired Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookDateRequired = True
        End If
        If Not poChanges.BookCarrOrderNumber = boChanges.BookCarrOrderNumber Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookCarrOrderNumber = True
        End If
        If Not poChanges.BookTransType = boChanges.BookTransType Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookTransType = True
        End If
        If Not poChanges.BookRevTotalCost = boChanges.BookRevTotalCost Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookRevTotalCost = True
        End If
        If podrRecord.POHDRStatusFlag = 4 Then

            If Not poChanges.BookOrigCity = boChanges.BookOrigCity Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookOrigCity = True
            End If
            If Not poChanges.BookOrigState = boChanges.BookOrigState Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookOrigState = True
            End If
            If Not poChanges.BookOrigCountry = boChanges.BookOrigCountry Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookOrigCountry = True
            End If
            If Not poChanges.BookOrigZip = boChanges.BookOrigZip Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookOrigZip = True
            End If
            If Not poChanges.BookOrigAddress1 = boChanges.BookOrigAddress1 Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookOrigAddress1 = True
            End If
            If Not poChanges.BookDestCity = boChanges.BookDestCity Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookDestCity = True
            End If
            If Not poChanges.BookDestState = boChanges.BookDestState Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookDestState = True
            End If
            If Not poChanges.BookDestCountry = boChanges.BookDestCountry Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookDestCountry = True
            End If
            If Not poChanges.BookDestZip = boChanges.BookDestZip Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookDestZip = True
            End If
            If Not poChanges.BookDestAddress1 = boChanges.BookDestAddress1 Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedBookDestAddress1 = True
            End If
        Else
            If Not poChanges.LaneNumber = boChanges.LaneNumber Then
                changed.AreThereChanges = True
                changed.POChanges.HasChangedLaneNumber = True
            End If
        End If
        If Not poChanges.BookMustLeaveByDateTime = boChanges.BookMustLeaveByDateTime Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookMustLeaveByDateTime = True
        End If
        If Not poChanges.BookProNumber = boChanges.BookProNumber Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookProNumber = True
        End If
        If Not poChanges.BookLoadCom = boChanges.BookLoadCom Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookLoadCom = True
        End If
        If Not poChanges.BookModeTypeControl = boChanges.BookModeTypeControl Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookModeTypeControl = True
        End If
        If Not poChanges.BookUser1 = boChanges.BookUser1 Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookUser1 = True
        End If
        If Not poChanges.BookUser2 = boChanges.BookUser2 Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookUser2 = True
        End If
        If Not poChanges.BookUser3 = boChanges.BookUser3 Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookUser3 = True
        End If
        If Not poChanges.BookUser4 = boChanges.BookUser4 Then
            changed.AreThereChanges = True
            changed.POChanges.HasChangedBookUser4 = True
        End If

        Return changed
    End Function


    Public Function GetImportChanges365(ByVal orderNumber As String, ByVal sequenceNumber As Integer, ByVal customerNumber As Integer) As DAL.Models.CompareChanges()
        Dim retVal As New List(Of DAL.Models.CompareChanges)
        If String.IsNullOrEmpty(orderNumber) Then
            Return retVal.ToArray()
        End If
        'get the customerControl from companyNumber.
        Dim comp As DTO.Comp = NGLCompData.GetCompFiltered(0, customerNumber, "")
        If comp Is Nothing Then Return retVal.ToArray()
        'Get the existing record
        Dim bookrecord As DTO.Book = NGLBookData.GetBookFiltered(BookCarrOrderNumber:=orderNumber, BookOrderSequence:=sequenceNumber)
        If bookrecord Is Nothing OrElse bookrecord.BookControl = 0 Then Return retVal.ToArray()
        'Get the modified record
        Dim podrRecord As DTO.POHdr = OrderImportBLL.getPOHDRWDetails(orderNumber, sequenceNumber, customerNumber)
        If podrRecord Is Nothing Then Return retVal.ToArray()
        'Get other info
        Dim laneNumber As String = NGLLaneData.GetLaneNumber(bookrecord.BookODControl)
        Dim compNo As Integer = NGLCompData.GetCompNumber(bookrecord.BookCustCompControl)

        'Compare Changes
        Dim intID As Integer = 1

        'Order Number
        If Not podrRecord.POHDROrderNumber = bookrecord.BookCarrOrderNumber Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookCarrOrderNumber", .Caption = "Order Number", .OriginalValue = bookrecord.BookCarrOrderNumber.ToString(), .ModifiedValue = podrRecord.POHDROrderNumber.ToString(), .ValueType = "String"}
            retVal.Add(s)
            intID += 1
        End If
        'Pro Number
        If Not podrRecord.POHDRPRONumber = bookrecord.BookProNumber Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookProNumber", .Caption = "Pro Number", .OriginalValue = bookrecord.BookProNumber, .ModifiedValue = podrRecord.POHDRPRONumber, .ValueType = "String"}
            retVal.Add(s)
            intID += 1
        End If
        'CompanyNumber
        If Not podrRecord.POHDRDefaultCustomer = compNo.ToString() Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "CompanyNumber", .Caption = "Company Number", .OriginalValue = compNo.ToString(), .ModifiedValue = podrRecord.POHDRDefaultCustomer.ToString(), .ValueType = "Integer"}
            retVal.Add(s)
            intID += 1
        End If
        'Quantity
        Dim poQty = If(podrRecord.POHDRQty.HasValue, podrRecord.POHDRQty.Value, 0)
        If Not poQty = bookrecord.BookTotalCases Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookTotalCases", .Caption = "Quantity", .OriginalValue = bookrecord.BookTotalCases.ToString(), .ModifiedValue = poQty.ToString(), .ValueType = "Integer"}
            retVal.Add(s)
            intID += 1
        End If
        'Weight
        Dim poWgt = If(podrRecord.POHDRWgt.HasValue, podrRecord.POHDRWgt.Value, 0)
        If Not poWgt = bookrecord.BookTotalWgt Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookTotalWgt", .Caption = "Weight", .OriginalValue = bookrecord.BookTotalWgt.ToString(), .ModifiedValue = poWgt.ToString(), .ValueType = "Double"}
            retVal.Add(s)
            intID += 1
        End If
        'Pallets
        Dim poPallets = If(podrRecord.POHDRPallets.HasValue, podrRecord.POHDRPallets.Value, 0)
        If Not poPallets = bookrecord.BookTotalPL Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookTotalPL", .Caption = "Pallets", .OriginalValue = bookrecord.BookTotalPL.ToString(), .ModifiedValue = poPallets.ToString(), .ValueType = "Double"}
            retVal.Add(s)
            intID += 1
        End If
        'Load Date
        If Not podrRecord.POHDRShipdate = bookrecord.BookDateLoad Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookDateLoad", .Caption = "Load Date", .OriginalValue = bookrecord.BookDateLoad.ToString(), .ModifiedValue = podrRecord.POHDRShipdate.ToString(), .ValueType = "Date"}
            retVal.Add(s)
            intID += 1
        End If
        'Ordered Date
        If Not podrRecord.POHDRPOdate = bookrecord.BookDateOrdered Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookDateOrdered", .Caption = "Order Date", .OriginalValue = bookrecord.BookDateOrdered.ToString(), .ModifiedValue = podrRecord.POHDRPOdate.ToString(), .ValueType = "Date"}
            retVal.Add(s)
            intID += 1
        End If
        'Required
        If Not podrRecord.POHDRReqDate = bookrecord.BookDateRequired Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookDateRequired", .Caption = "Required", .OriginalValue = bookrecord.BookDateRequired.ToString(), .ModifiedValue = podrRecord.POHDRReqDate.ToString(), .ValueType = "Date"}
            retVal.Add(s)
            intID += 1
        End If
        'Total Cost
        If Not podrRecord.POHDRTotalCost = bookrecord.BookRevTotalCost Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookRevTotalCost", .Caption = "Total Cost", .OriginalValue = bookrecord.BookRevTotalCost.ToString(), .ModifiedValue = podrRecord.POHDRTotalCost.ToString(), .ValueType = "Decimal"}
            retVal.Add(s)
            intID += 1
        End If
        'Trans Type
        If Not podrRecord.POHDRFrt.ToString() = bookrecord.BookTransType Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookTransType", .Caption = "Trans Type", .OriginalValue = bookrecord.BookTransType, .ModifiedValue = podrRecord.POHDRFrt.ToString(), .ValueType = "String"}
            retVal.Add(s)
            intID += 1
        End If
        'Address Fields
        If podrRecord.POHDRStatusFlag = 4 Then
            'Origin
            If Not podrRecord.POHDROrigAddress1 = bookrecord.BookOrigAddress1 Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookOrigAddress1", .Caption = "Orig Addr 1", .OriginalValue = bookrecord.BookOrigAddress1, .ModifiedValue = podrRecord.POHDROrigAddress1, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            If Not podrRecord.POHDROrigCity = bookrecord.BookOrigCity Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookOrigCity", .Caption = "Orig City", .OriginalValue = bookrecord.BookOrigCity, .ModifiedValue = podrRecord.POHDROrigCity, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            If Not podrRecord.POHDROrigState = bookrecord.BookOrigState Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookOrigState", .Caption = "Orig State", .OriginalValue = bookrecord.BookOrigState, .ModifiedValue = podrRecord.POHDROrigState, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            If Not podrRecord.POHDROrigZip = bookrecord.BookOrigZip Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookOrigZip", .Caption = "Orig Zip", .OriginalValue = bookrecord.BookOrigZip, .ModifiedValue = podrRecord.POHDROrigZip, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            If Not podrRecord.POHDROrigCountry = bookrecord.BookOrigCountry Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookOrigCountry", .Caption = "Orig Country", .OriginalValue = bookrecord.BookOrigCountry, .ModifiedValue = podrRecord.POHDROrigCountry, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            'Destination
            If Not podrRecord.POHDRDestAddress1 = bookrecord.BookDestAddress1 Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookDestAddress1", .Caption = "Dest Addr 1", .OriginalValue = bookrecord.BookDestAddress1, .ModifiedValue = podrRecord.POHDRDestAddress1, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            If Not podrRecord.POHDRDestCity = bookrecord.BookDestCity Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookDestCity", .Caption = "Dest City", .OriginalValue = bookrecord.BookDestCity, .ModifiedValue = podrRecord.POHDRDestCity, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            If Not podrRecord.POHDRDestState = bookrecord.BookDestState Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookDestState", .Caption = "Dest State", .OriginalValue = bookrecord.BookDestState, .ModifiedValue = podrRecord.POHDRDestState, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            If Not podrRecord.POHDRDestZip = bookrecord.BookDestZip Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookDestZip", .Caption = "Dest Zip", .OriginalValue = bookrecord.BookDestZip, .ModifiedValue = podrRecord.POHDRDestZip, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
            If Not podrRecord.POHDRDestCountry = bookrecord.BookDestCountry Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookDestCountry", .Caption = "Dest Country", .OriginalValue = bookrecord.BookDestCountry, .ModifiedValue = podrRecord.POHDRDestCountry, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
        Else
            'compare to lane values
            'Lane Number
            If Not podrRecord.POHDRvendor = laneNumber Then
                Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "LaneNumber", .Caption = "Lane Number", .OriginalValue = laneNumber, .ModifiedValue = podrRecord.POHDRvendor, .ValueType = "String"}
                retVal.Add(s)
                intID += 1
            End If
        End If
        'Leave By Date/Time
        If Not podrRecord.POHDRMustLeaveByDateTime = bookrecord.BookMustLeaveByDateTime Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookMustLeaveByDateTime", .Caption = "Leave By Date/Time", .OriginalValue = bookrecord.BookMustLeaveByDateTime, .ModifiedValue = podrRecord.POHDRMustLeaveByDateTime, .ValueType = "Date"}
            retVal.Add(s)
            intID += 1
        End If
        'Temp
        If Not podrRecord.POHDRTemp = bookrecord.BookLoads(0).BookLoadCom Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookLoadCom", .Caption = "Temp", .OriginalValue = bookrecord.BookLoads(0).BookLoadCom, .ModifiedValue = podrRecord.POHDRTemp, .ValueType = "String"}
            retVal.Add(s)
            intID += 1
        End If
        'Mode
        If Not podrRecord.POHDRModeTypeControl = bookrecord.BookModeTypeControl Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookModeTypeControl", .Caption = "Mode", .OriginalValue = bookrecord.BookModeTypeControl.ToString(), .ModifiedValue = podrRecord.POHDRModeTypeControl.ToString(), .ValueType = "Integer"}
            retVal.Add(s)
            intID += 1
        End If
        'User 1
        If Not podrRecord.POHDRUser1 = bookrecord.BookUser1 Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookUser1", .Caption = "User 1", .OriginalValue = bookrecord.BookUser1, .ModifiedValue = podrRecord.POHDRUser1, .ValueType = "String"}
            retVal.Add(s)
            intID += 1
        End If
        'User 2
        If Not podrRecord.POHDRUser2 = bookrecord.BookUser2 Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookUser2", .Caption = "User 2", .OriginalValue = bookrecord.BookUser2, .ModifiedValue = podrRecord.POHDRUser2, .ValueType = "String"}
            retVal.Add(s)
            intID += 1
        End If
        'User 3
        If Not podrRecord.POHDRUser3 = bookrecord.BookUser3 Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookUser3", .Caption = "User 3", .OriginalValue = bookrecord.BookUser3, .ModifiedValue = podrRecord.POHDRUser3, .ValueType = "String"}
            retVal.Add(s)
            intID += 1
        End If
        'User 4
        If Not podrRecord.POHDRUser4 = bookrecord.BookUser4 Then
            Dim s As New DAL.Models.CompareChanges With {.ID = intID, .FieldName = "BookUser4", .Caption = "User 4", .OriginalValue = bookrecord.BookUser4, .ModifiedValue = podrRecord.POHDRUser4, .ValueType = "String"}
            retVal.Add(s)
            intID += 1
        End If
        'Items
        ' poChanges.BookTotalItems = If(podrRecord.POItems Is Nothing, 0, podrRecord.POItems.Count)
        'boChanges.BookTotalItems = bookrecord.BookLoads(0).BookItems.Count
        'If Not poChanges.BookTotalItems = boChanges.BookTotalItems Then
        '    changed.AreThereChanges = True
        'End If


        Return retVal.ToArray()
    End Function


#End Region

#Region "Protected or Private Functions"

    Friend Function createProcessNewBookingFailedSubscriptionAlert(ByVal Subject As String,
                                       ByVal CompControl As Integer,
                                       ByVal Msg As String,
                                       ByVal BookProNumber As String,
                                       ByVal OrderNumber As String,
                                       ByVal OrderSequence As String,
                                       ByVal Errors As String,
                                       ByVal Warnings As String,
                                       ByVal Messages As String) As Boolean
        Try
            Dim Note1 As String = String.Format(" Book Pro Number: {0} Order Number - Sequence: {1}-{2}", BookProNumber, OrderNumber, OrderSequence)
            Dim Note2 As String = " Errors: " & Errors
            Dim Note3 As String = " Warnings: " & Warnings
            Dim Note4 As String = " Messages: " & Messages
            Dim Body As String = String.Concat("Alert - there was a problem processing all the steps durring new order import for: ", vbCrLf, Msg, vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4)
            'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
            Return NGLtblAlertMessageData.InsertAlertMessage("ProcessNewBookingFailed", "Alert the system failed to complete all the steps durring order import", Subject, Body, CompControl, 0, 0, 0, Note1, Note2, Note3, Note4, "")
        Catch ex As Exception
            'ignore any errors when processing alerts 
        End Try
        'if we get here just return false
        Return False

    End Function
#End Region


End Class
