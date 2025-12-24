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
Imports PCM = NGL.FreightMaster.PCMiler
Imports NGL.FM.ERE.Transform
Imports Microsoft.Office.Interop.Excel
Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.ApplicationInsights
Imports Newtonsoft.Json
Imports Serilog
Imports NGL.FM.CarTar
Imports System.Web.UI.WebControls
Imports Serilog.Context
Imports SerilogTracing

Public Class NGLBookRevenueBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()

        Me.Parameters = oParameters
        Me.SourceClass = "NGLBookRevenueBLL"
        Logger = Logger.ForContext(Of NGLBookRevenueBLL)

    End Sub

#End Region

#Region " Properties "



    Private _RecalcCosts As Boolean = False
    Public Property RecalcCosts() As Boolean
        Get
            Return _RecalcCosts
        End Get
        Set(ByVal value As Boolean)
            _RecalcCosts = value
        End Set
    End Property

#End Region

#Region "DAL Wrapper Methods"

    Public Function GetBookRevenuesWDetailsFiltered(ByVal Control As Integer) As DTO.BookRevenue()
        Using LogContext.PushProperty("BookControl", Control)
            Logger.Information("GetBookRevenuesWDetailsFiltered(Control) - Control: {BookControl}", Control)
            Return NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(Control)
        End Using

    End Function

    Public Function GetBookRevenuesWDetailsFiltered(ByVal Control As Integer, ByVal showOverridenFees As Boolean) As DTO.BookRevenue()

        Logger.Information("GetBookRevenuesWDetailsFiltered(Control, showOverriddenFees) - {BookControl},{showOverridenFees}", Control, showOverridenFees)
        Dim list As DTO.BookRevenue() = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(Control)
        If list IsNot Nothing AndAlso list.Length > 0 Then
            For Each item In list
                item.BookFees = item.BookFees.FindAll(Function(p) p.BookFeesOverRidden = showOverridenFees)
            Next
        End If
        Return list
    End Function

    Public Function GetBookRevenue(ByVal Control As Integer) As DTO.BookRevenue
        Return NGLBookRevenueData.GetBookRevenueFiltered(Control)
    End Function

    Public Function GetBookRevenueWDetails(ByVal orderNumber As String, ByVal sequenceNumber As Integer, ByVal customerControl As Integer) As DTO.BookRevenue
        Return NGLBookRevenueData.GetBookRevenueWDetailsFiltered(orderNumber, sequenceNumber, customerControl)
    End Function

    Public Function UpdateBookRevenue(ByVal oData As DTO.BookRevenue) As DTO.BookRevenue
        'NGLBookRevenueData.UpdateRecord(oData)
        NGLBookRevenueData.UpdateWithDependencies(oData)
        If Not NGLBookRevenueData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookRevenueData.BookDependencyResult.BookControl, 0),
                                   If(NGLBookRevenueData.BookDependencyResult.ErrNumber, 0),
                                   NGLBookRevenueData.BookDependencyResult.RetMsg,
                                   NGLBookRevenueData.LastProcedureName,
                                   If(NGLBookRevenueData.BookDependencyResult.MustRecalculate, False))
        Return NGLBookRevenueData.GetBookRevenueFiltered(NGLBookRevenueData.BookDependencyResult.BookControl)
    End Function

    Public Function UpdateBookRevenueWithDetails(ByVal oData As DTO.BookRevenue) As DTO.BookRevenue
        NGLBookRevenueData.UpdateRecordWithDetailsNoReturn(oData)
        'Allways recalculate when we save with details
        AssignCarrier(oData, FreightMaster.Data.Utilities.AssignCarrierCalculationType.Recalculate)
        'check for errors
        If Not NGLBookRevenueData.BookDependencyResult Is Nothing AndAlso If(NGLBookRevenueData.BookDependencyResult.ErrNumber, 0) <> 0 Then _
            throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {NGLBookRevenueData.LastProcedureName, If(NGLBookRevenueData.BookDependencyResult.ErrNumber, 0), NGLBookRevenueData.BookDependencyResult.RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)
        Return NGLBookRevenueData.GetBookRevenueFiltered(oData.BookControl)
    End Function

    Public Sub UpdateBookRevenueNoReturn(ByVal oData As DTO.BookRevenue)
        NGLBookRevenueData.UpdateRecordNoReturn(oData)
        If Not NGLBookRevenueData.BookDependencyResult Is Nothing Then _
            processRecalculateParameters(If(NGLBookRevenueData.BookDependencyResult.BookControl, 0),
                           If(NGLBookRevenueData.BookDependencyResult.ErrNumber, 0),
                           NGLBookRevenueData.BookDependencyResult.RetMsg,
                           NGLBookRevenueData.LastProcedureName,
                           If(NGLBookRevenueData.BookDependencyResult.MustRecalculate, False))
    End Sub


    Public Sub UpdateBookRevenueWithDetailsNoReturn(ByVal oData As DTO.BookRevenue)
        NGLBookRevenueData.UpdateRecordWithDetailsNoReturn(oData)
        'Allways recalculate when we save with details
        AssignCarrier(oData, FreightMaster.Data.Utilities.AssignCarrierCalculationType.Recalculate)
        'check for errors
        If Not NGLBookRevenueData.BookDependencyResult Is Nothing AndAlso If(NGLBookRevenueData.BookDependencyResult.ErrNumber, 0) <> 0 Then _
            throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {NGLBookRevenueData.LastProcedureName, If(NGLBookRevenueData.BookDependencyResult.ErrNumber, 0), NGLBookRevenueData.BookDependencyResult.RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)
    End Sub

    ''' <summary>
    ''' This only save the revenue array, does no recalculating.  Typically used for SpotRates since spot rates does "in memory" calculations before saving.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateBookRevenuesWithDetails(ByVal oData As DTO.BookRevenue()) As DTO.BookRevenue()
        Return NGLBookRevenueData.SaveRevenuesWDetails(oData)
    End Function

    ''' <summary>
    ''' This only save the revenue array, does no recalculating.  Typically used for SpotRates, adjusting BFC calculator since spot rates does "in memory" calculations before saving.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <remarks></remarks>
    Public Sub UpdateBookRevenuesWithDetailsNoReturnNoRecalc(ByVal oData As DTO.BookRevenue())
        NGLBookRevenueData.SaveRevenuesWDetails(oData)
    End Sub

    ''' <summary>
    ''' May no longer be used
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="AllocateBFC"></param>
    ''' <param name="UpdateBFC"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveAndCalcBookRevenue(ByVal oData As DTO.BookRevenue, ByVal AllocateBFC As Boolean, ByVal UpdateBFC As Boolean) As DTO.BookRevenue
        If Not oData Is Nothing AndAlso oData.BookControl <> 0 Then
            AssignCarrier(oData, FreightMaster.Data.Utilities.AssignCarrierCalculationType.Recalculate)
            Return GetBookRevenue(oData.BookControl)
        Else
            Return If(oData, New DTO.BookRevenue)
        End If
    End Function


    ''' <summary>
    ''' Typically called from a client component to recalculate costs using existing linehaul. 
    ''' It returns a CarrierCostResults object which contains errors, logs and message.  
    ''' An updated BookRevenue object is returned as the first item in the BookRevs list.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.100 5/16/2016
    ''' Should now be used to replace RecalculateBookRevenueFreightCosts method that does not return messages.
    ''' </remarks>
    Public Function RecalculateUsingLineHaul(ByVal BookControl As Integer) As DTO.CarrierCostResults
        Dim results As New DTO.CarrierCostResults()
        Logger.Information("RecalculateUsingLineHaul(BookControl) - BookControl: {BookControl}", BookControl)
        If BookControl = 0 Then
            results.AddMessage(FreightMaster.Data.DataTransferObjects.CarrierCostResults.MessageEnum.M_NoOrdersFound)
            Return results
        End If
        Logger.Information("RecalculateUsingLineHaul(BookControl) - AssignCarrier(BookControl, True)")
        results = AssignCarrier(BookControl, True)

        If results Is Nothing Then
            results = New DTO.CarrierCostResults
            results.AddMessage(FreightMaster.Data.DataTransferObjects.CarrierCostResults.MessageEnum.M_NoTariffsFound)
            Logger.Information("RecalculateUsingLineHaul(BookControl) - No Tariffs Found")
        End If
        results.BookRevs = New List(Of FreightMaster.Data.DataTransferObjects.BookRevenue)
        results.BookRevs.Add(GetBookRevenue(BookControl))
        Logger.Information("RecalculateUsingLineHaul(BookControl) - GetBookRevenue({BookControl}), results: {@results}", BookControl, results)
        Return results
    End Function

    ''' <summary>
    ''' This method is called by the client after the book record has been saved.  it returns an updated BookRevenue object
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecalculateBookRevenueFreightCosts(ByVal BookControl As Integer) As DTO.BookRevenue
        If BookControl = 0 Then Return New DTO.BookRevenue
        AssignCarrier(BookControl, True)
        Return GetBookRevenue(BookControl)
    End Function

    Public Sub RecalculateBookRevenueFreightCostsNoReturn(ByVal BookControl As Integer)
        AssignCarrier(BookControl, True)
    End Sub

    Public Function RecalculateBookRevenueFreightCostsNoBFC(ByVal BookControl As Integer) As DTO.BookRevenue
        If BookControl = 0 Then Return New DTO.BookRevenue
        AssignCarrier(GetBookRevenue(BookControl), DAL.Utilities.AssignCarrierCalculationType.RecalcuateNoBFC)
        Return GetBookRevenue(BookControl)
    End Function

    Public Sub RecalculateBookRevenueFreightCostsNoBFCNoReturn(ByVal BookControl As Integer)
        AssignCarrier(GetBookRevenue(BookControl), DAL.Utilities.AssignCarrierCalculationType.RecalcuateNoBFC)
    End Sub

    Public Function UpdateCarrier(ByVal BookControl As Integer, ByVal CarrierControl As Integer) As DTO.CarrierCostResults
        Return AssignCarrier(BookControl, CarrierControl, DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier)
    End Function

    Public Function UpdateCarrier(ByVal BookControl As Integer) As DTO.CarrierCostResults
        Return AssignCarrier(GetBookRevenue(BookControl), DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier)
    End Function

    Public Function UpdateAssignedCarrier(ByVal BookControl As Integer, ByVal CarrierControl As Integer) As DTO.CarrierCostResults
        Return AssignCarrier(BookControl, CarrierControl, DAL.Utilities.AssignCarrierCalculationType.UpdateAssignedCarrier)
    End Function

    ''' <summary>
    ''' updates the booking record for BookControl with the selected carrier or saves the book revenue objects if they exist (not null).
    ''' If BookRevs is provided the BookControl and SelectedCarrier are ignored.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="SelectedCarrier"></param>
    ''' <param name="CarrierCont"></param>
    ''' <param name="BookRevs"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.007 on 04/24/2020
    '''     added logic to automatically recalculate costs on SPOT RATE when we have zero line haul/total cost
    ''' </remarks>
    Public Function updateSelectedCarrier(ByVal BookControl As Integer, ByVal SelectedCarrier As DTO.CarriersByCost, ByVal CarrierCont As DTO.CarrierCont, ByVal BookRevs() As DTO.BookRevenue) As DTO.CarrierCostResults
        Dim results As New DTO.CarrierCostResults()
        Using operation = Logger.StartActivity("updateSelectedCarrier(BookControl: {BookControl}, SelectedCarrier: {SelectedCarrier}, CarrierCont: {CarrierCont}, BookRevs: {BookRevs})", BookControl, SelectedCarrier, CarrierCont, BookRevs)
            ''when BookRevs is available we just save it.  Typically used for the SpotRate process.
            If (Not BookRevs Is Nothing AndAlso BookRevs.Count > 0) Then
                Try
                    'set the carrier contact.
                    For Each rev In BookRevs
                        rev.BookCarrierControl = SelectedCarrier.CarrierControl
                        rev.BookCarrierContact = CarrierCont.CarrierContName
                        rev.BookCarrierContactPhone = CarrierCont.CarrierContactPhone
                        rev.BookCarrierContControl = CarrierCont.CarrierContControl
                        ' Modified by RHR for v-8.2.1.007 on 04/24/2020
                        If (rev.BookRevTotalCost <= 0 AndAlso rev.BookCarrTarName = "SPOT RATE" AndAlso NGLBookRevenueData.GetParValue("AllowZeroLineHaulOnSpotRate", rev.BookCustCompControl) = 1) Then
                            rev.BookLockAllCosts = False
                        End If
                    Next
                    NGLBookRevenueData.SaveRevenuesNoReturn(BookRevs, True, True)
                    ' Modified by RHR for v-8.2.1.007 on 04/24/2020
                    For Each rev In BookRevs
                        If (rev.BookRevTotalCost <= 0 AndAlso rev.BookCarrTarName = "SPOT RATE" AndAlso NGLBookRevenueData.GetParValue("AllowZeroLineHaulOnSpotRate", rev.BookCustCompControl) = 1) Then
                            Try
                                RecalculateUsingLineHaul(rev.BookControl)
                                'relock the booking record
                                NGLBookRevenueData.LockOrUnlockBooking(rev.BookControl, True)
                            Catch ex As Exception
                                'do nothing
                                operation.Complete(exception:=ex)
                            End Try
                        End If
                    Next

                    operation.Complete()
                    results.Success = True
                    Return results
                Catch ex As Exception
                    operation.Complete(exception:=ex)
                    Logger.Error(ex, "Error in updateSelectedCarrier")
                    Throw
                End Try
            Else
                'modified by RHR 9/30/15 v-7.0.4 we must update BookRevs 
                'BookCarrTarEquipMatControl and BookCarrTarEquipControl if they are provided or the 
                'CalculationType of UpdateCarrier will be changed to Normal and lowest cost carrier will be selected instead
                'If BookRevs Is Nothing OrElse BookRevs.Count() < 1 AndAlso (SelectedCarrier.BookCarrTarEquipMatControl <> 0 And SelectedCarrier.BookCarrTarEquipControl <> 0) Then
                '    BookRevs = GetBookRevenuesWDetailsFiltered(BookControl)
                'End If
                'If (Not BookRevs Is Nothing AndAlso BookRevs.Count > 0) Then
                Try
                    ''set the carrier information
                    'For Each rev In BookRevs
                    '    rev.BookCarrierControl = SelectedCarrier.CarrierControl
                    '    rev.BookCarrTarEquipMatControl = SelectedCarrier.BookCarrTarEquipMatControl
                    '    rev.BookCarrTarEquipControl = SelectedCarrier.BookCarrTarEquipControl
                    'Next
                    NGLBookRevenueData.SaveRevenuesNoReturn(BookControl, SelectedCarrier.CarrierControl, SelectedCarrier.BookCarrTarEquipMatControl, vbObjectError, True, True)
                Catch sqlEx As FaultException
                    operation.Complete(exception:=sqlEx)
                    Throw
                Catch ex As Exception
                    'do nothing
                    operation.Complete(exception:=ex)
                    Logger.Error(ex, "Error in updateSelectedCarrier")
                End Try
                'End If
            End If

            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier

            Dim carrierControl As Integer = SelectedCarrier.CarrierControl
            Dim CarrTarEquipMatControl As Integer = SelectedCarrier.BookCarrTarEquipMatControl
            Dim CarrTarEquipControl As Integer = SelectedCarrier.BookCarrTarEquipControl
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = False
            Dim validated As Boolean = False
            Dim optimizeByCapacity As Boolean = False
            Dim modeTypeControl As Integer = SelectedCarrier.BookModeTypeControl
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0

            results = TARBookRev.assignCarrier(BookControl,
                                            carrierControl,
                                            CalculationType,
                                            CarrTarEquipMatControl,
                                            CarrTarEquipControl,
                                            prefered,
                                            noLateDelivery,
                                            validated,
                                            optimizeByCapacity,
                                            modeTypeControl,
                                            tempType,
                                            tariffTypeControl,
                                            carrTarEquipMatClass,
                                            carrTarEquipMatClassTypeControl,
                                            carrTarEquipMatTarRateTypeControl,
                                            agentControl,
                                            1,
                                            1000,
                                            CarrierCont)
        End Using
        Return results

    End Function

    Public Function getEstimatedCarriersByCostPrefered(bookControl As Integer, page As Integer, pagesize As Integer) As DTO.CarrierCostResults

        Return getEstimatedCarriersByCost(bookControl:=bookControl, prefered:=True, page:=page, pagesize:=pagesize)

    End Function

    Public Function getEstimatedCarriersByCostFiltered(ByVal Filters As DTO.GetCarriersByCostParameters) As DTO.CarrierCostResults
        Dim results As DTO.CarrierCostResults

        Using Logger.StartActivity("getEstimatedCarriersByCostFiltered(Filters: {Filters})", Filters)

            With Filters
                results = getEstimatedCarriersByCost(.BookControl, .carrierControl, .prefered, .noLateDelivery, .validated, .optimizeByCapacity, .modeTypeControl, .tempType, .tariffTypeControl, .carrTarEquipMatClass, .carrTarEquipMatClassTypeControl, .carrTarEquipMatTarRateTypeControl, .agentControl, .Page, .PageSize)
            End With

        End Using

        Return results
    End Function

    Public Function getEstimatedCarriersByCostAll(bookControl As Integer, page As Integer, pagesize As Integer) As DTO.CarrierCostResults

        Return getEstimatedCarriersByCost(bookControl:=bookControl, prefered:=False, noLateDelivery:=False, validated:=False, page:=page, pagesize:=pagesize)

    End Function

    Public Function getEstimatedCarriersByCostForMode(bookControl As Integer,
                                               modeTypeControl As Integer,
                                               page As Integer,
                                               pagesize As Integer) As DTO.CarrierCostResults
        Dim results As DTO.CarrierCostResults

        Using Logger.StartActivity("getEstimatedCarriersByCostForMode(BookControl: {BookControl}, ModeTypeControl: {ModeTypeControl})", bookControl, modeTypeControl)
            results = TARBookRev.estimatedCarriersByCost(bookControl:=bookControl, modeTypeControl:=modeTypeControl, page:=page, pagesize:=pagesize)
        End Using

        Return results

    End Function

    Public Function UpdateBookFuelFee(ByVal BookControl As Integer) As Boolean
        Dim lLoads = UpdateBookFuelFeeForLoad(BookControl)
        If Not lLoads Is Nothing AndAlso lLoads.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' wrapper for UpdateBookFuelFeesBatch which returns true or false
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateBookFuelFees(ByVal CarrierControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet = UpdateBookFuelFeesBatch(CarrierControl)
        If Not oRet Is Nothing AndAlso oRet.Success = True Then
            If Not oRet.Log Is Nothing AndAlso oRet.Log.Count > 0 Then
                Dim strServerMessage As String = oRet.getLogAsSingleStr("|")
                'some record could not be processed so raise an error
                throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_BatchProcessError, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_ServerMsgDetails, New List(Of String) From {strServerMessage}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_ProcessProcedureFailure)
            Else
                blnRet = True
            End If
        End If
        Return blnRet
    End Function

    Public Function UpdateBookFuelFeesBatch(ByVal CarrierControl As Integer) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults With {.Key = CarrierControl, .Success = False}
        Using Logger.StartActivity("UpdateBookFuelFeesBatch({CarrierControl})", CarrierControl)
            Try
                Dim lBooksProcessed As New List(Of DTO.NGLListItem)
                Dim oBookRefData() As DTO.BookReferenceData = NGLBookData.GetFuelBookingReferenceForCarrier(CarrierControl)
                If oBookRefData Is Nothing OrElse oBookRefData.Count = 0 Then
                    'nothing to do so return true
                    oRet.Success = True
                    Return oRet
                End If
                For Each b In oBookRefData
                    Try 'if one error occurs in this loop we do not want to skip the remaining booking records.  Just log the error and return the message to the caller
                        If Not DTO.WCFResults.ContainsNGLItem(b.BookControl, lBooksProcessed) Then
                            Dim lLoads = UpdateBookFuelFeeForLoad(b.BookControl)
                            If Not lLoads Is Nothing AndAlso lLoads.Count > 0 Then
                                For Each i In lLoads
                                    lBooksProcessed.Add(New DTO.NGLListItem With {.Key = i, .intValue = i})
                                Next
                            End If
                        End If
                    Catch ex As FaultException(Of DAL.SqlFaultInfo)
                        If Not ex.Detail.Details = "E_CannotSaveProtectedDataDetails" Then
                            b.Results.AddFaultException(FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.E_SQLFaultCannotUpdateBookFuelFeeForLoad, ex.Reason.ToString(), ex.Detail.Message, ex.Detail.Details, ex.Detail.DetailsList)
                            oRet.Log.Add(New DTO.NGLMessage(ex.Message & "; " & ex.Detail.ToString()))
                        Else
                            oRet.Log.Add(New DTO.NGLMessage("Cannot update override fuel fees. BookProNumber " & b.BookProNumber & " " & ex.Message & "; " & ex.Detail.ToString()))
                        End If
                    Catch ex As Exception
                        b.Results.AddUnexpectedError(ex)
                        oRet.Log.Add(New DTO.NGLMessage(ex.Message))
                    End Try
                Next
                oRet.intValues = lBooksProcessed
                'PFM 8/22/2014 removed this code becasue BookReferencedata fails when returned through WCF
                'Need to find which properties cannot be serialized and fix them.
                'TODO - Ready for testing 8/25/2014 RHR
                'Modified by RHR 8/26/14 cannot save BookReferenceData int DTOData so we created a server
                'side only propert BLLOnlyData.  this must be used by any server components to read the results.
                oRet.BLLOnlyData = (From d In oBookRefData Where DTO.WCFResults.ContainsNGLItem(d.BookControl, lBooksProcessed) Select d).ToArray()
                oRet.Success = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("UpdateBookFuelFeesBatch"))
            End Try
        End Using
        Return oRet
    End Function

    ''' <summary>
    ''' This version of the method gets the parameter filters from the client
    ''' updates the booking record for BookControl with the selected carrier or saves the book revenue objects if they exist (not null).
    ''' If BookRevs is provided the BookControl and SelectedCarrier are ignored.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="params"></param>
    ''' <param name="SelectedCarrier"></param>
    ''' <param name="CarrierCont"></param>
    ''' <param name="BookRevs"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 10/4/16 for v-7.0.5.110 Mode/Temp Type Precedence
    ''' </remarks>
    Public Function updateSelectedCarrierFiltered(ByVal BookControl As Integer, ByVal params As DTO.GetCarriersByCostParameters, ByVal SelectedCarrier As DTO.CarriersByCost, ByVal CarrierCont As DTO.CarrierCont, ByVal BookRevs() As DTO.BookRevenue) As DTO.CarrierCostResults
        Dim results As New DTO.CarrierCostResults()

        ''when BookRevs is available we just save it.  Typically used for the SpotRate process.
        If (Not BookRevs Is Nothing AndAlso BookRevs.Count > 0) Then
            Try
                'set the carrier contact.
                For Each rev In BookRevs
                    rev.BookCarrierControl = SelectedCarrier.CarrierControl
                    rev.BookCarrierContact = CarrierCont.CarrierContName
                    rev.BookCarrierContactPhone = CarrierCont.CarrierContactPhone
                    rev.BookCarrierContControl = CarrierCont.CarrierContControl
                Next
                NGLBookRevenueData.SaveRevenuesNoReturn(BookRevs, True, True)
                results.Success = True
                Return results
            Catch sqlEx As FaultException
                Throw
            Catch ex As Exception
                Throw
            End Try
        Else
            'modified by RHR 9/30/15 v-7.0.4 we must update BookRevs 
            'BookCarrTarEquipMatControl and BookCarrTarEquipControl if they are provided or the 
            'CalculationType of UpdateCarrier will be changed to Normal and lowest cost carrier will be selected instead
            If BookRevs Is Nothing OrElse BookRevs.Count() < 1 AndAlso (SelectedCarrier.BookCarrTarEquipMatControl <> 0 And SelectedCarrier.BookCarrTarEquipControl <> 0) Then
                BookRevs = GetBookRevenuesWDetailsFiltered(BookControl)
            End If
            If (Not BookRevs Is Nothing AndAlso BookRevs.Count > 0) Then
                Try
                    'set the carrier information
                    For Each rev In BookRevs
                        rev.BookCarrierControl = SelectedCarrier.CarrierControl
                        rev.BookCarrTarEquipMatControl = SelectedCarrier.BookCarrTarEquipMatControl
                        rev.BookCarrTarEquipControl = SelectedCarrier.BookCarrTarEquipControl
                    Next
                    NGLBookRevenueData.SaveRevenuesNoReturn(BookRevs, True, True)
                Catch ex As Exception
                    'do nothing
                End Try
            End If
        End If

        Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier

        Dim carrierControl As Integer = SelectedCarrier.CarrierControl
        Dim CarrTarEquipMatControl As Integer = SelectedCarrier.BookCarrTarEquipMatControl
        Dim CarrTarEquipControl As Integer = SelectedCarrier.BookCarrTarEquipControl
        Dim prefered As Boolean = params.prefered
        Dim noLateDelivery As Boolean = params.noLateDelivery
        Dim validated As Boolean = params.validated
        Dim optimizeByCapacity As Boolean = params.optimizeByCapacity
        Dim modeTypeControl As Integer = params.modeTypeControl
        Dim tempType As Integer = params.tempType
        Dim tariffTypeControl As Integer = params.tariffTypeControl
        Dim carrTarEquipMatClass As String = params.carrTarEquipMatClass
        Dim carrTarEquipMatClassTypeControl As Integer = params.carrTarEquipMatClassTypeControl
        Dim carrTarEquipMatTarRateTypeControl As Integer = params.carrTarEquipMatTarRateTypeControl
        Dim agentControl As Integer = params.agentControl

        Return TARBookRev.assignCarrier(BookControl,
                                        carrierControl,
                                        CalculationType,
                                        CarrTarEquipMatControl,
                                        CarrTarEquipControl,
                                        prefered,
                                        noLateDelivery,
                                        validated,
                                        optimizeByCapacity,
                                        modeTypeControl,
                                        tempType,
                                        tariffTypeControl,
                                        carrTarEquipMatClass,
                                        carrTarEquipMatClassTypeControl,
                                        carrTarEquipMatTarRateTypeControl,
                                        agentControl,
                                        1,
                                        1000,
                                        CarrierCont)


    End Function


#End Region

#Region " Public Methods"

    Public Function updateCreditHold(ByRef revs As DTO.BookRevenue(), ByVal creditHold As Boolean) As Boolean
        If revs Is Nothing OrElse revs.Length < 1 Then Return False
        Dim needtoSave As Boolean = False
        Dim checkComplete As Boolean = False
        For Each item In revs.ToList()
            If Not item.BookCreditHold = creditHold Then
                If checkComplete = False Then
                    needtoSave = True
                End If
                checkComplete = True
                item.BookCreditHold = creditHold
            End If
        Next
        If needtoSave Then
            revs = NGLBookRevenueData.SaveRevenuesWDetails(revs.ToArray())
        End If

        Return True
    End Function



#Region "   Assign Carrier Methods"

    Public Function AssignCarrier(ByVal BookControl As Integer, ByVal recalculateOnly As Boolean) As DTO.CarrierCostResults
        Using Logger.StartActivity("AssignCarrier(BookControl: {BookControl},recalculateOnly: {recalculateOnly} [NotUsed?]) - But DaL.Utilities.AssignCarrierCalculationType.Recalcuate is...", BookControl, recalculateOnly)
            Return AssignCarrier(GetBookRevenue(BookControl), DAL.Utilities.AssignCarrierCalculationType.Recalculate)
        End Using

    End Function

    Public Function AssignCarrier(ByVal BookControl As Integer, ByVal CalculationType As DAL.Utilities.AssignCarrierCalculationType) As DTO.CarrierCostResults
        Return AssignCarrier(GetBookRevenue(BookControl), CalculationType)
    End Function

    ''' <summary>
    ''' recalculates the load using the current tariff if assigned or just recalculates the using the linehaul if 
    ''' a carrier tariff is not assigned (like when using a spot rate)  
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateCarrierCost(ByVal BookControl As Integer) As DTO.CarrierCostResults

        Dim carrierCostResults As DTO.CarrierCostResults
        Using Logger.StartActivity("UpdateCarrierCost(BookControl: {BookControl})", BookControl)
            Dim oBookData = GetBookRevenue(BookControl)
            If oBookData.BookCarrTarEquipControl = 0 Then
                carrierCostResults = RecalculateUsingLineHaul(BookControl)
            Else
                carrierCostResults = AssignCarrier(BookControl, DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier)
            End If
        End Using
        Return carrierCostResults


    End Function

    ''' <summary>
    ''' We should not use this overload bacause it does not use the correct calculation type
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AssignCarrier(ByVal BookControl As Integer) As DTO.CarrierCostResults
        Return AssignCarrier(GetBookRevenue(BookControl))
    End Function

    ''' <summary>
    ''' will select lowest cost carrier if carriercontrol is 0 or will select the lowest cost tariff if the BookCarrTarEquipControl is 0 or it will update the carrier cost using the currently assigned tariff
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' the following rules are applied:
    ''' 1. Prefered = true: parameter for future use where some tariffs may be selected as prefered
    ''' 2. noLateDelivery = true: parameter for future logic to estimate the delivery date based on load date and transit times
    ''' 3. validated = true: parameter to determine if the carrier is qualified; checks CarrierQualQualified, CarrierQualInsuranceDate, CarrierQualContract and CarrierQualContractDate
    '''    when validated is true carriers that are not qualified are ignored and no warnings, errors or validation messages are returned.
    '''    To recieve validation warning alerts call a different method where validated can be set to false.
    ''' 4. optimizedByCapacity = true: Applies the capacity configuration settings for the carrier tariff equipment service settings
    '''    When optimizedByCapacity = true carriers are ignored if their tariff service level capacity sittings are do not meet or exceed the order capacity values.
    ''' 5. modeTypeControl will use the lane default mode type assigned to the order.
    ''' 6. tempType will be set to zero: in the future we may add temperature restrictions 
    ''' </remarks>
    Public Function AssignOrUpdateCarrier(ByVal BookControl As Integer, Optional ByVal OptmisticConcurrencyOn As Boolean = True) As DTO.CarrierCostResults


        Dim carrierCostResults As DTO.CarrierCostResults
        Using Logger.StartActivity("AssignOrUpdateCarrier(BookControl: {BookControl}, OptimisticConcurrency: {OptimisticConcurrency}", BookControl, OptmisticConcurrencyOn)

            'Dim oBookRevs = GetBookRevenuesWDetailsFiltered(BookControl)
            Dim oBookRevs = NGLBookRevenueData.GetBookRevenuesWDetailsFilteredFromView(BookControl)
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = True
            Dim validated As Boolean = True
            Dim optimizeByCapacity As Boolean = True
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0

            carrierCostResults = TARBookRev.assignCarrier(oBookRevs,
                                            prefered,
                                            noLateDelivery,
                                            validated,
                                            optimizeByCapacity,
                                            tempType,
                                            tariffTypeControl,
                                            carrTarEquipMatClass,
                                            carrTarEquipMatClassTypeControl,
                                            carrTarEquipMatTarRateTypeControl,
                                            agentControl,
                                            1,
                                            1000,
                                            Nothing,
                                            Nothing,
                                            OptmisticConcurrencyOn)
        End Using
        Return carrierCostResults
    End Function

    ''' <summary>
    ''' Will select lowest cost carrier if BookRevenue.Bookcarriercontrol is 0 
    ''' or will select the lowest cost tariff if the BookRevenue.BookCarrTarEquipControl is 0 
    ''' or it will update the carrier cost using the currently assigned tariff.
    ''' If selecting the lowest cost carrier any carrier controls provided in lRestrictedCarriers 
    ''' will be ignored.
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <param name="lRestrictedCarriers"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' the following rules are applied:
    ''' 1. Prefered = true: parameter for future use where some tariffs may be selected as prefered
    ''' 2. noLateDelivery = true: parameter for future logic to estimate the delivery date based on load date and transit times
    ''' 3. validated = true: parameter to determine if the carrier is qualified; checks CarrierQualQualified, CarrierQualInsuranceDate, CarrierQualContract and CarrierQualContractDate
    '''    when validated is true carriers that are not qualified are ignored and no warnings, errors or validation messages are returned.
    '''    To recieve validation warning alerts call a different method where validated can be set to false.
    ''' 4. optimizedByCapacity = true: Applies the capacity configuration settings for the carrier tariff equipment service settings
    '''    When optimizedByCapacity = true carriers are ignored if their tariff service level capacity sittings are do not meet or exceed the order capacity values.
    ''' 5. modeTypeControl will use the lane default mode type assigned to the order.
    ''' 6. tempType will be set to zero: in the future we may add temperature restrictions 
    ''' 7. If selecting the lowest cost carrier any carrier controls provided in lRestrictedCarriers will be ignored.
    ''' </remarks>
    Public Function AssignOrUpdateCarrier(ByVal oData As DTO.BookRevenue, Optional lRestrictedCarriers As List(Of Integer) = Nothing) As DTO.CarrierCostResults
        Dim results As New DTO.CarrierCostResults() With {.Success = False}
        Using operation = Logger.StartActivity("AssignOrUpdateCarrier(oData: {oData}, lRestrictedCarriers: {lRestrictedCarriers}", oData, lRestrictedCarriers)

            'Modified By LVV on 10/3/16 for v-

            If oData Is Nothing OrElse oData.BookControl = 0 Then
                results = New DTO.CarrierCostResults() With {.Success = False}
                results.AddMessage(FreightMaster.Data.DataTransferObjects.CarrierCostResults.MessageEnum.M_NoOrdersFound)
                operation.Complete()
                Return results
            End If

            Dim BookControl As Integer = oData.BookControl
            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal
            Dim modeTypeControl As Integer = oData.BookModeTypeControl
            Dim tempType As Integer = 0

            'Modified By LVV on 10/3/16 for v-7.0.5.110 Mode/Temp Type Precedence
            If oData.BookCarrierControl <> 0 And oData.BookCarrTarEquipControl <> 0 Then
                CalculationType = DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier
                NGLBookData.GetModeTempTypesByPrecedence(oData.BookControl, modeTypeControl, tempType)
            End If

            If oData Is Nothing Then Return New DTO.CarrierCostResults()
            Dim carrierControl As Integer = oData.BookCarrierControl
            Dim CarrTarEquipMatControl As Integer = oData.BookCarrTarEquipMatControl
            Dim CarrTarEquipControl As Integer = oData.BookCarrTarEquipControl
            'if BookCarrTarEquipControl is not zero we do not use the parameters below
            'when selecting the tariff
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = True
            Dim validated As Boolean = True
            Dim optimizeByCapacity As Boolean = True
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0

            results = TARBookRev.assignCarrier(BookControl,
                                            carrierControl,
                                            CalculationType,
                                            CarrTarEquipMatControl,
                                            CarrTarEquipControl,
                                            prefered,
                                            noLateDelivery,
                                            validated,
                                            optimizeByCapacity,
                                            modeTypeControl,
                                            tempType,
                                            tariffTypeControl,
                                            carrTarEquipMatClass,
                                            carrTarEquipMatClassTypeControl,
                                            carrTarEquipMatTarRateTypeControl,
                                            agentControl,
                                            1,
                                            1000,
                                            Nothing,
                                            lRestrictedCarriers)
        End Using

        Return results
    End Function

    ''' <summary>
    ''' This method can only be used to select the lowest cost carrier, 
    ''' when carriercontrol = 0 or to update the exitsting carrier costs.
    ''' If selecting the lowest cost carrier any carrier controls provided in lRestrictedCarriers 
    ''' will be ignored.
    ''' </summary>
    ''' <param name="bookControl"></param>
    ''' <param name="carrierControl"></param>
    ''' <param name="lRestrictedCarriers"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AssignCarrier(ByVal bookControl As Integer, ByVal carrierControl As Integer, lRestrictedCarriers As List(Of Integer)) As DTO.CarrierCostResults

        Dim results As DTO.CarrierCostResults
        Using Logger.StartActivity("AssignCarrier(bookControl: {bookControl}, carrierControl: {carrierControl}, lRestrictedCarriers: {lRestrictedCarriers}", bookControl, carrierControl, lRestrictedCarriers)


            Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = If(carrierControl = 0, DAL.Utilities.AssignCarrierCalculationType.Normal, DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier)
            'Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal
            Dim CarrTarEquipMatControl As Integer = 0
            Dim CarrTarEquipControl As Integer = 0
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = True
            Dim validated As Boolean = True
            Dim optimizeByCapacity As Boolean = True
            Dim modeTypeControl As Integer = 0
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0

            results = TARBookRev.assignCarrier(bookControl,
                                            carrierControl,
                                            CalculationType,
                                            CarrTarEquipMatControl,
                                            CarrTarEquipControl,
                                            prefered,
                                            noLateDelivery,
                                            validated,
                                            optimizeByCapacity,
                                            modeTypeControl,
                                            tempType,
                                            tariffTypeControl,
                                            carrTarEquipMatClass,
                                            carrTarEquipMatClassTypeControl,
                                            carrTarEquipMatTarRateTypeControl,
                                            agentControl,
                                            1,
                                            1000,
                                            Nothing,
                                            lRestrictedCarriers)
        End Using
        Return results
    End Function

    ''' <summary>
    ''' This method can only be used to select the lowest cost carrier, 
    ''' when carriercontrol = 0 or to update the exitsting carrier costs.
    ''' </summary>
    ''' <param name="bookControl"></param>
    ''' <param name="carrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AssignCarrier(ByVal bookControl As Integer, ByVal carrierControl As Integer, ByVal CarrTarEquipMatControl As Integer, ByVal CarrTarEquipControl As Integer) As DTO.CarrierCostResults
        'Update Carrier Calculation type cannot be used without the CarrTarEquipControl and CarrTarEquipMatControl
        Dim results As DTO.CarrierCostResults

        Using Logger.StartActivity("AssignCarrier(bookControl: {bookControl}, carrierControl: {carrierControl}, CarrTarEquipMatControl: {CarrTarEquipMatControl}, CarrTarEquipControl: {CarrTarEquipControl}", bookControl, carrierControl, CarrTarEquipMatControl, CarrTarEquipControl)
            'If CarrTarEquipControl = 0 Then
            '    CalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal
            'End If
            'Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = If(carrierControl = 0, DAL.Utilities.AssignCarrierCalculationType.Normal, DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier)
            'Dim CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = True
            Dim validated As Boolean = True
            Dim optimizeByCapacity As Boolean = True
            Dim modeTypeControl As Integer = 0
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0
            results = TARBookRev.assignCarrier(bookControl,
                                            carrierControl,
                                            DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier,
                                            CarrTarEquipMatControl,
                                            CarrTarEquipControl,
                                            prefered,
                                            noLateDelivery,
                                            validated,
                                            optimizeByCapacity,
                                            modeTypeControl,
                                            tempType,
                                            tariffTypeControl,
                                            carrTarEquipMatClass,
                                            carrTarEquipMatClassTypeControl,
                                            carrTarEquipMatTarRateTypeControl,
                                            agentControl)
        End Using
        '   we must use normal
        Return results

    End Function

    Public Function AssignCarrier(ByRef oData As DTO.BookRevenue, Optional ByVal CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal, Optional lRestrictedCarriers As List(Of Integer) = Nothing) As DTO.CarrierCostResults
        Dim results As New DTO.CarrierCostResults()

        Using operation = Logger.StartActivity("AssignCarrier(BookRevenue: {BookRevenue}, CalculationType: {CalculationType})", oData, CalculationType)
            Dim oRet As New DTO.WCFResults()
            Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)
            Dim iLoadTenderControl As Integer = 0
            Dim strMsg As String = ""
            Dim oLTLogData As New DAL.NGLLoadTenderLogData(Me.Parameters)
            oRet.Success = False
            oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
            If oData Is Nothing Then Return results
            Dim bookControl As Integer = oData.BookControl
            Dim carrierControl As Integer = oData.BookCarrierControl
            Dim CarrTarEquipMatControl As Integer = oData.BookCarrTarEquipMatControl
            Dim CarrTarEquipControl As Integer = oData.BookCarrTarEquipControl
            Dim prefered As Boolean = True
            Dim noLateDelivery As Boolean = True
            Dim validated As Boolean = True
            Dim optimizeByCapacity As Boolean = True
            Dim modeTypeControl As Integer = oData.BookModeTypeControl
            Dim tempType As Integer = 0
            Dim tariffTypeControl As Integer = 0
            Dim carrTarEquipMatClass As String = Nothing
            Dim carrTarEquipMatClassTypeControl As Integer = 0
            Dim carrTarEquipMatTarRateTypeControl As Integer = 0
            Dim agentControl As Integer = 0
            Dim SHID As String = oData.BookSHID
            Dim blnUseTariff As Boolean = True


            Logger.Information("AssignCarrier BookControl:{BookControl}, carrierControl: {carrierControl}", bookControl, carrierControl)
            If CalculationType = DAL.Utilities.AssignCarrierCalculationType.UpdateCarrier Or CalculationType = DAL.Utilities.AssignCarrierCalculationType.UpdateAssignedCarrier Then
                Dim eTypeCode As NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum.None
                [Enum].TryParse(oData.BookCarrActualService, eTypeCode)
                Dim oCarrier As DTO.Carrier = NGLCarrierData.GetCarrier(carrierControl)
                Logger.Information("AssignCarrier BookControl:{BookControl}, carrierControl: {carrierControl}, BookCarrActualService: {BookCarrActualService}", bookControl, carrierControl, oData?.BookCarrActualService)

                'check BidTypeEnum value stored in BookCarrActualService
                ' values are:
                'None = 0
                'NextStop = 1
                'NGLTariff = 2
                'P44 = 3
                'Spot = 4
                'CHRAPI = 5
                'UPSAPI = 6
                'YRCAPI = 7
                'JTSAPI = 8
                'FedXAPI = 9
                Logger.Information("AssignCarrier BookControl:{BookControl} check if eTypeCode ({eTypeCode}) is API carrier", bookControl, eTypeCode)
                If bookControl > 0 And (eTypeCode = DTO.tblLoadTender.BidTypeEnum.P44 Or eTypeCode = DTO.tblLoadTender.BidTypeEnum.CHRAPI Or eTypeCode = DTO.tblLoadTender.BidTypeEnum.FedXAPI Or eTypeCode = DTO.tblLoadTender.BidTypeEnum.JTSAPI Or eTypeCode = DTO.tblLoadTender.BidTypeEnum.UPSAPI Or eTypeCode = DTO.tblLoadTender.BidTypeEnum.YRCAPI) Then
                    blnUseTariff = False
                    'we have a book control and this is an API carrier so we need to create a LoadTender record for the API
                    'Insert the Booking Info in the the Load Board returns LoadBoardControl in WCFResults
                    'TODO: merge with spot rate insert load board and return LoadTenderControl, CNS number and SHID
                    oRet = NGLLoadTenderData.InsertLoadBoardRecords(bookControl, SHID, DTO.tblLoadTender.LoadTenderTypeEnum.LoadBoard) 'LoadTenderTypeEnum.p44
                    Logger.Information("AssignCarrier BookControl:{BookControl}, SHID: {SHID} InsertLoadBoardRecords returned {@oRet}", bookControl, SHID, oRet)
                    If Not oRet.Success Then
                        results.Success = False
                        results.AddMessage("Insert booking records into load tender table failed.")
                        Return results
                    End If
                    oRet.AddLog("Create load tender record for rate quote.")
                    Dim sVals = oRet.TryGetKeys({"LoadTenderControl", "BookConsPrefix", "BookSHID"}, {"0", "Undefined", "Undefined"})
                    Integer.TryParse(sVals(0), iLoadTenderControl)
                    Dim sCNS As String = sVals(1)
                    Dim sSHID As String = sVals(2)

                    If iLoadTenderControl = 0 Then
                        'we cannot continue
                        oRet.Success = False
                        results.Success = False
                        results.AddMessage("Generate quote failure a load tender record could not be created.")
                        Logger.Warning("Generate quote failure a load tender record could not be created")
                        Return results
                    End If
                    'Get the Order Number for the Log
                    Dim sOrderNumber As String = NGLLoadTenderData.GetOrderNumberFromLoadTender(iLoadTenderControl)
                    oRet.updateKeyFields("BookCarrOrderNumber", sOrderNumber)
                    Logger.Information("AssignCarrier BookControl:{BookControl}, SHID: {SHID} LoadTenderControl: {LoadTenderControl}, BookCarrOrderNumber: {OrderNumber}", bookControl, SHID, iLoadTenderControl, sOrderNumber)
                    strMsg = ""
                    Dim blnQuotesExist As Boolean = False
                    Select Case eTypeCode
                        Case DTO.tblLoadTender.BidTypeEnum.P44
                            blnQuotesExist = NGLLoadTenderData.CreateP44Bid(bookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                            Logger.Information("NGLBookRevenueBLL.AssignCarrier - CreateP44Bid returned blnQuotesExist: {blnQuotesExist}", blnQuotesExist)
                        Case DTO.tblLoadTender.BidTypeEnum.CHRAPI
                            Dim oCHRLEConfig As New LTS.tblSSOALEConfig()
                            Dim lCHRCompConfig As New List(Of LTS.tblSSOAConfig)
                            If NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.CHRAPI, oCHRLEConfig, lCHRCompConfig, bookControl) Then
                                blnQuotesExist = oLT.CreateCHRBid(bookControl, iLoadTenderControl, "", oCHRLEConfig, lCHRCompConfig, DAL.Utilities.SSOAAccount.CHRAPI, strMsg)
                            End If
                        Case DTO.tblLoadTender.BidTypeEnum.UPSAPI
                            Dim oUPSLEConfig As New LTS.tblSSOALEConfig()
                            Dim lUPSCompConfig As New List(Of LTS.tblSSOAConfig)
                            If NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.UPSAPI, oUPSLEConfig, lUPSCompConfig, bookControl) Then
                                blnQuotesExist = oLT.CreateUPSBid(bookControl, iLoadTenderControl, "", oUPSLEConfig, lUPSCompConfig, DAL.Utilities.SSOAAccount.UPSAPI, strMsg)
                            End If
                        Case DTO.tblLoadTender.BidTypeEnum.JTSAPI
                            Dim oJTSLEConfig As New LTS.tblSSOALEConfig()
                            Dim lJTSCompConfig As New List(Of LTS.tblSSOAConfig)
                            If NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.JTSAPI, oJTSLEConfig, lJTSCompConfig, bookControl) Then
                                blnQuotesExist = oLT.CreateJTSBid(bookControl, iLoadTenderControl, "", oJTSLEConfig, lJTSCompConfig, DAL.Utilities.SSOAAccount.JTSAPI, strMsg)
                            End If
                        Case Else
                            blnUseTariff = True
                    End Select
                    If blnQuotesExist Then
                        Dim oSelectedDefaultCarrier As clsPreferedDefaultCarrier
                        Logger.Information("Quotes Exist, getting bids for CarrierControl: {CarrierControl}", carrierControl)
                        'prepare to update the booking from bid
                        Dim RecordCount As Integer = 0
                        Dim filters As New DAL.Models.AllFilters With {.filterName = "BidLoadTenderControl", .filterValue = iLoadTenderControl.ToString()}
                        filters.addToSort("BidTotalCost", True)
                        Dim oBids As LTS.tblBid() = NGLBidData.GetActiveBidsById(RecordCount, filters).Where(Function(x) x.BidCarrierControl = carrierControl).ToArray()  'returns active bids greater than zero cost
                        If Not oBids Is Nothing AndAlso oBids.Count() > 0 Then
                            Dim newBid As LTS.tblBid = oBids(0)
                            If oBids.Count > 1 Then
                                For Each oBid As LTS.tblBid In oBids
                                    'check the name and SCAC code
                                    [Enum].TryParse(oBid.BidBidTypeControl, eTypeCode)
                                    Logger.Information("AssignCarrier BookControl:{BookControl}, SHID: {SHID} BidCarrierControl: {BidCarrierControl} - tblLoadTender.GetAPICarrierServiceReference({eTypeCode}, {oBid.BidServiceType})", bookControl, SHID, oBid.BidCarrierControl, eTypeCode)
                                    Dim eStatusCodeEnum = DTO.tblLoadTender.GetAPICarrierServiceReference(eTypeCode, oBid.BidServiceType)
                                    Dim blnCarrierAMatchs As Boolean = False
                                    'Compare Carrier SCAC first 
                                    If (oData.BookShipCarrierNumber = oBid.BidCarrierSCAC OrElse oCarrier.CarrierSCAC = oBid.BidCarrierSCAC) Then
                                        blnCarrierAMatchs = True
                                    ElseIf (oData.BookShipCarrierName = oBid.BidCarrierName OrElse oCarrier.CarrierName = oBid.BidCarrierName) Then
                                        'the carrier name matches
                                        blnCarrierAMatchs = True

                                    End If

                                    If (blnCarrierAMatchs AndAlso oData.BookTypeCode = eStatusCodeEnum.ToString()) Then
                                        Logger.Information("Assign Carrier - blnCarrierAMatch and BookTypeCode matches {eStatusCode}", eStatusCodeEnum)
                                        newBid = oBid
                                        Exit For
                                    End If
                                Next
                            End If
                            oSelectedDefaultCarrier = New clsPreferedDefaultCarrier(newBid)
                            Logger.Information("Calling NGLDATBLL.NSAPIUpdateLineHaul for {@SelectedCarrier}", oSelectedDefaultCarrier)
                            oRet = NGLDATBLL.NSAPIUpdateLineHaul(iLoadTenderControl, bookControl, oData.BookCarrOrderNumber, oSelectedDefaultCarrier, 0)
                        Else
                            blnUseTariff = True
                        End If
                    Else
                        blnUseTariff = True
                    End If
                End If
            End If

            If blnUseTariff Then
                Logger.Information("AssignCarrier - Call TarBookRev.AssignCarrier BookControl:{BookControl}, carrierControl: {carrierControl}, CalculationType: {CalculationType}", bookControl, carrierControl, CalculationType)
                results = TARBookRev.assignCarrier(bookControl,
                                           carrierControl,
                                           CalculationType,
                                           CarrTarEquipMatControl,
                                           CarrTarEquipControl,
                                           prefered,
                                           noLateDelivery,
                                           validated,
                                           optimizeByCapacity,
                                           modeTypeControl,
                                           tempType,
                                           tariffTypeControl,
                                           carrTarEquipMatClass,
                                           carrTarEquipMatClassTypeControl,
                                           carrTarEquipMatTarRateTypeControl,
                                           agentControl,
                                           1,
                                           1000,
                                           Nothing,
                                           lRestrictedCarriers)
            End If
        End Using

        Return results
    End Function

    Public Function AssignCarrier(bookControl As Integer,
                                  Optional carrierControl As Integer = 0,
                                  Optional CalculationType As DAL.Utilities.AssignCarrierCalculationType = DAL.Utilities.AssignCarrierCalculationType.Normal,
                                  Optional CarrTarEquipMatControl As Integer = 0,
                                  Optional CarrTarEquipControl As Integer = 0,
                                  Optional prefered As Boolean = True,
                                  Optional noLateDelivery As Boolean = False,
                                  Optional validated As Boolean = False,
                                  Optional optimizeByCapacity As Boolean = True,
                                  Optional modeTypeControl As Integer = 0,
                                  Optional tempType As Integer = 0,
                                  Optional tariffTypeControl As Integer = 0,
                                  Optional carrTarEquipMatClass As String = Nothing,
                                  Optional carrTarEquipMatClassTypeControl As Integer = 0,
                                  Optional carrTarEquipMatTarRateTypeControl As Integer = 0,
                                  Optional agentControl As Integer = 0,
                                  Optional page As Integer = 1,
                                  Optional pagesize As Integer = 1000,
                                  Optional CarrContact As DTO.CarrierCont = Nothing,
                                  Optional lRestrictedCarriers As List(Of Integer) = Nothing) As DTO.CarrierCostResults

        Return TARBookRev.assignCarrier(bookControl,
                                        carrierControl,
                                        CalculationType,
                                        CarrTarEquipMatControl,
                                        CarrTarEquipControl,
                                        prefered,
                                        noLateDelivery,
                                        validated,
                                        optimizeByCapacity,
                                        modeTypeControl,
                                        tempType,
                                        tariffTypeControl,
                                        carrTarEquipMatClass,
                                        carrTarEquipMatClassTypeControl,
                                        carrTarEquipMatTarRateTypeControl,
                                        agentControl,
                                        page,
                                        pagesize,
                                        CarrContact,
                                        lRestrictedCarriers)

    End Function

#End Region

#Region "   Estimate Costs"

    Public Function getEstimatedCarriersByCost(bookControl As Integer,
                                               Optional carrierControl As Integer = 0,
                                               Optional prefered As Boolean = True,
                                               Optional noLateDelivery As Boolean = False,
                                               Optional validated As Boolean = False,
                                               Optional optimizeByCapacity As Boolean = True,
                                               Optional modeTypeControl As Integer = 0,
                                               Optional tempType As Integer = 0,
                                               Optional tariffTypeControl As Integer = 0,
                                               Optional carrTarEquipMatClass As String = Nothing,
                                               Optional carrTarEquipMatClassTypeControl As Integer = 0,
                                               Optional carrTarEquipMatTarRateTypeControl As Integer = 0,
                                               Optional agentControl As Integer = 0,
                                               Optional page As Integer = 1,
                                               Optional pagesize As Integer = 1000) As DTO.CarrierCostResults

        Dim results As DTO.CarrierCostResults

        Using Logger.StartActivity("NGLBookRevenueBLL.getEstimatedCarriersByCost - bookControl: {bookControl}, carrierControl: {carrierControl}, prefered: {prefered}, noLateDelivery: {noLateDelivery}, validated: {validated}, optimizeByCapacity: {optimizeByCapacity}, modeTypeControl: {modeTypeControl}, tempType: {tempType}, tariffTypeControl: {tariffTypeControl}, carrTarEquipMatClass: {carrTarEquipMatClass}, carrTarEquipMatClassTypeControl: {carrTarEquipMatClassTypeControl}, carrTarEquipMatTarRateTypeControl: {carrTarEquipMatTarRateTypeControl}, agentControl: {agentControl}, page: {page}, pagesize: {pagesize}", bookControl, carrierControl, prefered, noLateDelivery, validated, optimizeByCapacity, modeTypeControl, tempType, tariffTypeControl, carrTarEquipMatClass, carrTarEquipMatClassTypeControl, carrTarEquipMatTarRateTypeControl, agentControl, page, pagesize)
            results = TARBookRev.estimatedCarriersByCost(bookControl,
                                        carrierControl,
                                        prefered,
                                        noLateDelivery,
                                        validated,
                                        optimizeByCapacity,
                                        modeTypeControl,
                                        tempType,
                                        tariffTypeControl,
                                        carrTarEquipMatClass,
                                        carrTarEquipMatClassTypeControl,
                                        carrTarEquipMatTarRateTypeControl,
                                        agentControl,
                                        page,
                                        pagesize)
        End Using



        Return results


    End Function


#End Region

#Region "   Cost Allocation"

    Public Function allocateLineHaulCost(ByVal bookrevs() As DTO.BookRevenue,
                                         ByVal allocationType As DTO.tblTarBracketType,
                                         ByVal userLineHaul As Decimal) As DTO.BookRevenue()
        If bookrevs Is Nothing Then Return Nothing
        Return TARBookRev.allocateLineHaulCost(bookrevs, allocationType, userLineHaul)

    End Function

    Public Function allocateBFCCostsByAllocationMode(ByVal bookrevs() As DTO.BookRevenue,
                                       ByVal allocationType As DTO.tblTarBracketType,
                                       ByVal userBFC As Decimal,
                                       ByVal autocalculateBFC As Boolean) As DTO.BookRevenue()
        If bookrevs Is Nothing Then Return Nothing
        Return TARBookRev.allocateBFCCostsByAllocationMode(bookrevs, allocationType, userBFC, autocalculateBFC)

    End Function

#End Region

#Region "   Rate Shop"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rateShop"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.002 on 10/28/2020
    '''   added logic to return the bookrev data from The rateShop object to assist
    '''   with accurate updates to the tblBid data including BookMilesFrom
    ''' </remarks>
    Public Function DoRateShop(ByVal rateShop As DTO.RateShop) As DTO.CarrierCostResults
        Dim results As DTO.CarrierCostResults = Nothing
        Using Logger.StartActivity("DoRateShop(rateShop: {RateShop})", rateShop)
            If rateShop IsNot Nothing AndAlso rateShop.BookRevs IsNot Nothing AndAlso rateShop.BookRevs.Count > 0 Then
                'setup the rate shopping defaults
                rateShop.BookRevs(0).BookProNumber = "RATESHOP" + "123456"
                rateShop.BookRevs(0).BookCarrOrderNumber = "RSOrder1234"
                rateShop.BookRevs(0).BookTranCode = "N"
                rateShop.BookRevs(0).BookAllowInterlinePoints = True
                'rateShop.BookRevs(0).BookFees = New List(Of DTO.BookFee) '-not used, fees are in the dto.rateshop.bookfees object.   'yes, it is used, for calcs
                rateShop.PageSize = 100
                rateShop.Page = 1
                rateShop.AgentControl = 0
                rateShop.OptimizeByCapacity = True
                rateShop.NoLateDelivery = False

                Dim oPCMAllStops As New DTO.PCMAllStops
                PCMilerBLL.loadPCMParameters()

                Logger.Information("Determing if we should use PCMiler based on PCMiler.BLL.NGLUsePCMiler ({0}) and rateShop.UsePCM ({1}) both being true. ", PCMilerBLL.NGLUsePCMiler, rateShop.UsePCM)

                If PCMilerBLL.NGLUsePCMiler And rateShop.UsePCM Then

                    Logger.Information("DoRateShop, Getting Practical miles based on rateShop.BookRevs(0)")

                    oPCMAllStops = PCMilerBLL.GetPracticalMiles(rateShop.BookRevs(0), -1, -1)

                    If oPCMAllStops Is Nothing Then
                        rateShop.BookRevs(0).BookMilesFrom = 0
                        Logger.Warning("DoRateShop, PCMiler failed to get miles, setting to 0")
                    Else
                        rateShop.BookRevs(0).BookMilesFrom = oPCMAllStops.TotalMiles
                        Logger.Information("DoRateShop, PCMiler got miles: {0}", oPCMAllStops.TotalMiles)
                    End If
                Else
                    'use the miles they provide
                    Logger.Information("DoRateShop, Using the miles provided in rateShop.BookRevs(0) which is {0}", rateShop.BookRevs(0).BookMilesFrom)
                End If

                'rate shop here.
                'results = TARBookRev.estimatedCarriersByCost(rateShop)
                'Logger.Information("Completed TARBookRev.estimatedCarriersByCost: ", results?.Log)
                'If results Is Nothing Then Return results
                'results.BookRevs = rateShop.BookRevs ' Modified by RHR for v-8.3.0.002 on 10/28/2020
                ''add any pcmiler errors here
                Logger.Information("DoRateShop, CheckBox If PCMilerIsActive ({0}) And rateShop.UsePCM ({1})", PCMilerBLL.IsPCMilerActive, rateShop.UsePCM)
                If PCMilerBLL.IsPCMilerActive And rateShop.UsePCM Then
                    If oPCMAllStops Is Nothing Then
                        Logger.Warning("DoRateShop, PCMiler failed to get miles, setting to 0, LastError: {0}", oPCMAllStops.LastError)
                        results.AddMessage(FreightMaster.Data.DataTransferObjects.CarrierCostResults.MessageEnum.MSGPCMGetMilesFailedWarning, oPCMAllStops.LastError)
                    Else
                        If Len(Trim(oPCMAllStops.LastError)) > 0 Then
                            Logger.Warning("DoRateShop, PCMiler failed to get miles, setting to 0, LastError: {0}", oPCMAllStops.LastError)
                            results.AddMessage(FreightMaster.Data.DataTransferObjects.CarrierCostResults.MessageEnum.MSGPCMGetMilesFailedWarning, oPCMAllStops.LastError)
                        End If
                        'If oPCMBadAddresses.COUNT > 0 Then logBadAddress(oPCMBadAddresses, oPCMAllStops, WCFProperties, PaneSettings)
                    End If
                Else
                    Logger.Information("DoRateShop, PCMiler is not active or rateShop.UsePCM is false, no PCMiler miles were calculated.")
                    '  no errors since they used thier own miles.
                End If

                'START TEST


                results = TARBookRev.estimatedCarriersByCost(rateShop)
                If results Is Nothing Then Return results

                results.BookRevs = rateShop.BookRevs    'TEST - put results AFTER PCMiler
                'END TEST

                'do any ere stuff.
                If (rateShop.UseERE) Then
                    Logger.Information("DoRateShop UseERE")
                    Dim transform As New TransformERE()
                    Dim ereResult As DTO.CarrierCostResults = transform.Rate(rateShop)
                    If ereResult IsNot Nothing Then
                        Logger.Information("DoRateShop UseERE, ereResult: {0}", ereResult)
                        If ereResult.Messages IsNot Nothing AndAlso ereResult.Messages.Count > 0 Then 'add messages
                            For Each item In ereResult.Messages
                                results.Messages.Add(item.Key, item.Value)
                            Next
                        End If
                        'For Each item In ereResult.CarriersByCost
                        '    'add the carrier controls here
                        'need to map the carrier name to freightmasters carrier number/control
                        'Next
                        For Each item In ereResult.CarriersByCost
                            results.CarriersByCost.Add(item)
                        Next
                        results.CarriersByCost = (From costs In results.CarriersByCost Order By costs.CarrierCost).ToList() 'sort cost assending.
                        Logger.Information("Completed DoRateShop UseERE", results)
                    End If
                End If

                'add any user level data restrictions.
                'A TMS user will want to see the upcharges, whereas the NEXTrack only user such as customer service will
                'not be able to see the upcharge.
                If (NGLSecurityData.IsNEXTrackOnly(Me.Parameters.UserName)) Then
                    results.IsNextrackOnly = True
                    'hide the markup. no one needs to know unless they are a TMS user.
                    'a smart IT person can look in the webbrowser variables to see what markup percent.
                    'Removed by RHR for v-8.5.4.001 on 07/06/2023 we now use this value differetly as part of the Bid data
                    'For Each item In results.CarriersByCost
                    '    If item.UpchargeCarrierCost > 0 Then
                    '        item.CarrierCost = item.UpchargeCarrierCost 'move the upcharge into the total carrier cost field.for easy UI settings
                    '    End If
                    '    item.UpchargePercent = 0 'clear out the upcharge percent.
                    '    item.UpchargeCarrierCost = 0 'clear out the upcharge $.
                    'Next
                Else
                    results.IsNextrackOnly = False
                    'this is a regulare TMS user. not need to clear out the percent.
                    'do nothing
                    'For Each item In results.CarriersByCost
                    '    'do nothiing
                    'Next
                End If

            End If
            Logger.Information("Completed NGLBookRevenueBLL.DoRateShop", results)
        End Using

        Return results
    End Function


#End Region

#Region "   Spot Rate"

    ''' <summary>
    ''' Creates a new Load Tender Record, run the spot rate calculator, creates a new tblbid record and returns the bidcontrol for dispatching
    ''' </summary>
    ''' <param name="wcfRes"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v- 8.2 on 11/27/2018
    '''
    ''' Return BidControl for dispatching as 'BidControl' key in wcfRes object
    ''' </remarks>
    Public Function InsertLoadTenderSpotRate(ByRef wcfRes As DTO.WCFResults) As Boolean
        Dim blnRet As Boolean = False
        Dim iVals = wcfRes.TryGetKeyInts({"LoadTenderControl", "BookControl"}, {0, 0})
        Dim iBidControl As Integer = 0
        Dim loadTenderControl As Integer = iVals(0)
        Dim BookControl As Integer = iVals(1)

        Logger.Information("InsertLoadTenderSpotRate - 1249, loadTenderControl: {0}, BookControl: {1}", loadTenderControl, BookControl)

        Dim DalSpotRate As DAL.NGLBookSpotRateData = New DAL.NGLBookSpotRateData(Me.Parameters)
        loadTenderControl = DalSpotRate.CreateLoadTenderFromBook(BookControl) 'NOTE: This line might be a duplicate call, depending on the method that calls this (?)
        Logger.Warning("InsertLoadTenderSpotRate - CreateLoadTenderFromBook - Potential Duplicate call, loadTenderControl: {0}", loadTenderControl)


        'get the spot rate data
        Dim oFilter = New DAL.Models.AllFilters()
        oFilter.ParentControl = BookControl
        oFilter.page = 1
        oFilter.pageSize = 1
        Logger.Information("Executing DalSpotRate.GetBookSpotRateData")

        Dim oSpotRates = DalSpotRate.GetBookSpotRateData(oFilter, 0)

        Logger.Information("Completed DalSpotRate.GetBookSpotRateData", oSpotRates)

        If oSpotRates Is Nothing OrElse oSpotRates.Count() < 1 Then Return 0 'cannot continue (do we throw an error?)

        Dim DalBookRevs As DAL.NGLBookRevenueData = New DAL.NGLBookRevenueData(Me.Parameters)
        Dim DalBookAcc As DAL.NGLBookAccessorial = New DAL.NGLBookAccessorial(Me.Parameters)
        Dim oSRate = New DTO.SpotRate
        With oSRate
            '.BookRevs = DalBookRevs.GetBookRevenues(BookControl).ToList()
            .BookRevs = GetBookRevenuesWDetailsFiltered(BookControl).ToList()
            .BookFees = DalSpotRate.GetSpotRateBookFees(BookControl)

            .CarrierControl = oSpotRates(0).BookSpotRateCarrierControl
            Dim allocationFormula As New DTO.tblTarBracketType
            'TODO: the AllocationFormula is not user configurable in the UI via drop down list 
            '   more work is needed here
            Select Case Trim(oSpotRates(0).AllocationFormula.ToLower)
                Case "pallets"
                    allocationFormula.TarBracketTypeControl = 1
                    allocationFormula.TarBracketTypeName = "Pallets"
                Case "volume"
                    allocationFormula.TarBracketTypeControl = 2
                    allocationFormula.TarBracketTypeName = "Volume"
                Case "quantity"
                    allocationFormula.TarBracketTypeControl = 3
                    allocationFormula.TarBracketTypeName = "Quantity"
                Case "cwt"
                    allocationFormula.TarBracketTypeControl = 5
                    allocationFormula.TarBracketTypeName = "Cwt"
                Case "distance"
                    allocationFormula.TarBracketTypeControl = 6
                    allocationFormula.TarBracketTypeName = "Distance"
                Case "even"
                    allocationFormula.TarBracketTypeControl = 7
                    allocationFormula.TarBracketTypeName = "Even"
                Case Else
                    allocationFormula.TarBracketTypeControl = 1
                    allocationFormula.TarBracketTypeName = "Pallets"
            End Select
            .allocationFormula = allocationFormula 'default to Pallets
            .AvgFuelPrice = oSpotRates(0).BookSpotRateAvgFuelPrice
            .totalLineHaulCost = oSpotRates(0).BookSpotRateTotalLineHaulCost
            .DeleteLaneFees = oSpotRates(0).BookSpotRateDeleteLaneFees
            .DeleteOrderFees = oSpotRates(0).BookSpotRateDeleteOrderFees
            .DeleteTariffFees = oSpotRates(0).BookSpotRateDeleteTariffFees
            .UseCarrierFuelAddendum = oSpotRates(0).BookSpotRateUserCarrierFuelAddendum
            .AutoCalculateBFC = oSpotRates(0).BookSpotRateAutoCalculateBFC
            Dim BFCBracketType As New DTO.tblTarBracketType
            'TODO: the BFCAllocationFormula is not user configurable in the UI via drop down list 
            '   more work is needed here
            Select Case Trim(oSpotRates(0).BFCAllocationFormula.ToLower)
                Case "pallets"
                    BFCBracketType.TarBracketTypeControl = 1
                    BFCBracketType.TarBracketTypeName = "Pallets"
                Case "volume"
                    BFCBracketType.TarBracketTypeControl = 2
                    BFCBracketType.TarBracketTypeName = "Volume"
                Case "quantity"
                    BFCBracketType.TarBracketTypeControl = 3
                    BFCBracketType.TarBracketTypeName = "Quantity"
                Case "cwt"
                    BFCBracketType.TarBracketTypeControl = 5
                    BFCBracketType.TarBracketTypeName = "Cwt"
                Case "distance"
                    BFCBracketType.TarBracketTypeControl = 6
                    BFCBracketType.TarBracketTypeName = "Distance"
                Case "even"
                    BFCBracketType.TarBracketTypeControl = 7
                    BFCBracketType.TarBracketTypeName = "Even"
                Case Else
                    BFCBracketType.TarBracketTypeControl = 1
                    BFCBracketType.TarBracketTypeName = "Pallets"
                    'BFCBracketType.TarBracketTypeControl = 4
                    'BFCBracketType.TarBracketTypeName = "Weight"
            End Select
            .AllocationBFCFormula = BFCBracketType
            .TotalBFC = oSpotRates(0).BookSpotRateTotalBFC
            .BookRevNegRevenueValue = 0
        End With
        'Calculate the cost but do not save to the book table
        'Save will happen on dispatching
        Dim oCostResults As DTO.CarrierCostResults = DoSpotRateNoSave(oSRate)
        If oCostResults Is Nothing Then
            'this should never happen in production
            wcfRes.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateFailure", {loadTenderControl.ToString(), "Cannot Create Spot Rate, please check your order and try again."})
            SaveAppError(String.Format("Save Rate Failure for LoadTenderControl = {0}: {1}.", loadTenderControl.ToString(), "Cannot Create Spot Rate, please check your order and try again."))
            wcfRes.Success = False
            Return False
        End If
        If Not oCostResults.Log Is Nothing AndAlso oCostResults.Log.Count > 0 Then wcfRes.Log.AddRange(oCostResults.Log)
        If Not oCostResults.Messages Is Nothing AndAlso oCostResults.Messages.Count > 0 Then wcfRes.AddRangeToDictionary(DTO.WCFResults.MessageType.Messages, oCostResults.Messages)
        ''LVV NOTE - Commented out 7/2/19 zero cost carrier bug
        ''If oCostResults.CarriersByCost Is Nothing OrElse oCostResults.CarriersByCost.Count < 1 Then
        ''    wcfRes.Success = False
        ''    Return False
        ''End If
        Dim strMsg As String = ""

        Dim DalNGLBidData As DAL.NGLBidData = New DAL.NGLBidData(Me.Parameters)
        iBidControl = DalNGLBidData.InsertNGLSpotRate(oCostResults.CarriersByCost, oCostResults.BookRevs, loadTenderControl, oSpotRates(0), strMsg)
        If iBidControl = 0 Then
            'note: we can throw an exception or update wcfResulst with a message
            'Not sure which is best?  we need to drill down into InsertNGLSpotRate
            'to determine why iBidControl might be zero.
            'if we throw an exception all previous messages are lost.
            throwFaultException(DAL.SqlFaultInfo.FaultInfoMsgs.E_FailedToExecute, DAL.SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails, New List(Of String) From {strMsg}, DAL.SqlFaultInfo.FaultReasons.E_CreateRecordFailure)
        End If
        blnRet = True
        wcfRes.updateKeyFields("BidControl", iBidControl.ToString())


        'Dim s = "CreateNGLTariffBidNoBookAsync Warning: "
        'Dim strMsg As String = ""

        'Dim success = CreateNGLTariffBidNoBook(oP44Request, LoadTenderControl, strMsg)

        'If String.IsNullOrWhiteSpace(strMsg) Then
        '    Dim oLT As New DAL.NGLLoadTenderData(Parameters)
        '    oLT.updateLoadTender(LoadTenderControl, Message:=strMsg)
        '    If Not success Then s = "CreateNGLTariffBidNoBookAsync Error: "
        '    SaveAppError(s & strMsg)
        'End If

        Return blnRet
    End Function

    Public Function DoSpotRateNoSave(ByVal parms As DTO.SpotRate) As DTO.CarrierCostResults
        Dim results As DTO.CarrierCostResults
        Using Logger.StartActivity("DoSpotRateNoSave(parms: {parms})", parms)
            If parms Is Nothing Then Return Nothing
            If parms.BookRevs Is Nothing Then Return Nothing
            results = TARBookRev.DoSpotRateNoSave(parms)
        End Using

        Return results
    End Function

    'Added by LVV On 2/17/17 For v-8.0 Next Stop
    Public Function DoSpotRateSave(ByVal parms As DTO.SpotRate, ByVal CarrContact As DTO.CarrierCont) As DTO.CarrierCostResults
        Dim results As DTO.CarrierCostResults
        Using Logger.StartActivity("DoSpotRateNoSave(parms: {parms}, CarrContact: {CarrContact})", parms, CarrContact)
            If parms Is Nothing Then Return Nothing
            If parms.BookRevs Is Nothing Then Return Nothing
            results = TARBookRev.DoSpotRateSave(parms, CarrContact)
        End Using
        Return results
    End Function

    ''' <summary>
    ''' Primarily uesed for unit and cycle testing.  Does not return exceptions or error details.  
    ''' Returns true on success or false for other reason  or errors.
    ''' </summary>
    ''' <param name="orderNumber"></param>
    ''' <param name="orderNumberSequenceNumber"></param>
    ''' <param name="carrierControl"></param>
    ''' <param name="totalLineHaulCost"></param>
    ''' <param name="Finalize"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DoAutoSpotRateWithSave(ByVal orderNumber As String, ByVal orderNumberSequenceNumber As String, ByVal carrierControl As Integer, Optional totalLineHaulCost As Double = 1000, Optional ByVal Finalize As Boolean = False) As Boolean
        Try

            If String.IsNullOrEmpty(orderNumber) Then Return False
            If String.IsNullOrEmpty(orderNumberSequenceNumber) Then Return False
            If carrierControl = 0 Then Return False
            'GetBookRevenueWDetailsFiltered
            'get bookrevenue.
            Dim book As DTO.Book = NGLBookData.GetBookFiltered(BookCarrOrderNumber:=orderNumber, BookOrderSequence:=orderNumberSequenceNumber)
            If book Is Nothing OrElse book.BookControl = 0 Then Return False
            'Dim bookrevs() As DTO.BookRevenue = NGLBookRevenueData.GetBookRevenues(book.BookControl)
            Dim bookrevs() As DTO.BookRevenue = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(book.BookControl)
            If bookrevs Is Nothing Then Return False

            Dim parms As New DTO.SpotRate()
            parms.BookRevs = bookrevs.ToList
            parms.CarrierControl = carrierControl
            Dim weightBracketType As New DTO.tblTarBracketType
            weightBracketType.TarBracketTypeControl = 4
            weightBracketType.TarBracketTypeName = "Weight"
            parms.allocationFormula = weightBracketType 'default to Weight
            parms.totalLineHaulCost = totalLineHaulCost
            parms.BookFees = New List(Of DTO.BookFee)
            parms.DeleteLaneFees = True
            parms.DeleteOrderFees = True
            parms.DeleteTariffFees = True
            parms.UseCarrierFuelAddendum = False
            parms.AutoCalculateBFC = True
            parms.AllocationBFCFormula = New DTO.tblTarBracketType With {.TarBracketTypeControl = 0}
            'AllocationBFCFormula.TarBracketTypeControl
            Dim results As DTO.CarrierCostResults = DoSpotRateNoSave(parms)
            If results Is Nothing Then Return False
            If results.Success = False Then Return False
            If results.BookRevs Is Nothing Then Return False
            bookrevs = NGLBookRevenueData.SaveRevenuesWDetails(results.BookRevs.ToArray(), False)
            If Finalize Then
                Dim oResults As New DTO.WCFResults With {.Success = True}
                BookBLL.ProcessNewTransCode(bookrevs, book.BookControl, "PB", book.BookTranCode, oResults)
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function



#End Region

#Region "   Adjust BFC"

    Public Function AdjustBFCNoSave(ByVal parms As DTO.AdjustBFC) As DTO.CarrierCostResults
        If parms Is Nothing Then Return Nothing
        If parms.BookRevs Is Nothing Then Return Nothing
        Return TARBookRev.AdjustBFCNoSave(parms)

    End Function

#End Region

#Region "   Label Generation"

    Public Function getShippingLabels(ByVal BookConsPrefix As String, ByVal CarrierControl As Integer) As List(Of DTO.BookShipLabel)
        Dim oRet As New List(Of DTO.BookShipLabel)
        Try
            Dim blnDirty As Boolean
            Dim oBookRevs = NGLBookRevenueData.GetBookRevenues(BookConsPrefix, CarrierControl)
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then Return oRet

            Dim oBookRevsSorted = (From d In oBookRevs Order By d.BookOrigCountry, d.BookOrigState, d.BookOrigCity, d.BookOrigAddress1, d.BookSHID, d.BookDestCountry, d.BookDestState, d.BookDestCity, d.BookDestAddress1 Select d).ToList()
            If oBookRevsSorted Is Nothing OrElse oBookRevsSorted.Count < 1 Then Return oRet 'Return an empty Ship Label
            If Not updateBookSHID(oBookRevsSorted, blnDirty, True) Then Return oRet 'Return an empty Ship Label
            Dim LabelPrintMaxDefault = NGLBookRevenueData.GetParValue("LabelPrintMaxDefault", oBookRevs(0).BookCustCompControl)
            'get the carrier name and number
            Dim dicCarNameNbr = NGLCarrierData.getCarrierNameNumber(CarrierControl)
            If dicCarNameNbr Is Nothing OrElse dicCarNameNbr.Count < 1 OrElse Not dicCarNameNbr.ContainsKey("CarrierName") OrElse Not dicCarNameNbr.ContainsKey("CarrierNumber") Then Return oRet 'not a valid carrier
            Dim strCarName As String = dicCarNameNbr("CarrierName")
            Dim intCarNumber As Integer
            Integer.TryParse(dicCarNameNbr("CarrierNumber"), intCarNumber)
            Dim BookOrigCountry As String = ""
            Dim BookOrigState As String = ""
            Dim BookOrigCity As String = ""
            Dim BookOrigAddress1 As String = ""
            Dim BookSHID As String = ""
            Dim BookDestCountry As String = ""
            Dim BookDestState As String = ""
            Dim BookDestCity As String = ""
            Dim BookDestAddress1 As String = ""
            Dim oLabel As DTO.BookShipLabel
            Dim intLabelCt As Integer = 1
            For Each r In oBookRevsSorted
                If r.BookOrigCountry <> BookOrigCountry _
                    Or r.BookOrigState <> BookOrigState _
                    Or r.BookOrigCity <> BookOrigCity _
                    Or r.BookOrigAddress1 <> BookOrigAddress1 _
                    Or r.BookSHID <> BookSHID _
                    Or r.BookDestCountry <> BookDestCountry _
                    Or r.BookDestState <> BookDestState _
                    Or r.BookDestCity <> BookDestCity _
                    Or r.BookDestAddress1 <> BookDestAddress1 Then

                    BookOrigCountry = r.BookOrigCountry
                    BookOrigState = r.BookOrigState
                    BookOrigCity = r.BookOrigCity
                    BookOrigAddress1 = r.BookOrigAddress1
                    BookSHID = r.BookSHID
                    BookDestCountry = r.BookDestCountry
                    BookDestState = r.BookDestState
                    BookDestCity = r.BookDestCity
                    BookDestAddress1 = r.BookDestAddress1
                    If Not oLabel Is Nothing Then oRet.Add(oLabel) 'on first pass oLabel with be nothing
                    oLabel = New DTO.BookShipLabel With {.BookShipLabelControl = intLabelCt,
                                                         .BookConsPrefix = r.BookConsPrefix,
                                                         .BookDestAddress1 = r.BookDestAddress1,
                                                         .BookDestAddress2 = r.BookDestAddress2,
                                                         .BookDestAddress3 = r.BookDestAddress3,
                                                         .BookDestCity = r.BookDestCity,
                                                         .BookDestCompControl = r.BookDestCompControl,
                                                         .BookDestCountry = r.BookDestCountry,
                                                         .BookDestName = r.BookDestName,
                                                         .BookDestState = r.BookDestState,
                                                         .BookDestZip = r.BookDestZip,
                                                         .BookOrigAddress1 = r.BookOrigAddress1,
                                                         .BookOrigAddress2 = r.BookOrigAddress2,
                                                         .BookOrigAddress3 = r.BookOrigAddress3,
                                                         .BookOrigCity = r.BookOrigCity,
                                                         .BookOrigCompControl = r.BookOrigCompControl,
                                                         .BookOrigCountry = r.BookOrigCountry,
                                                         .BookOrigName = r.BookOrigName,
                                                         .BookOrigState = r.BookOrigState,
                                                         .BookOrigZip = r.BookOrigZip,
                                                         .BookSHID = r.BookSHID,
                                                         .CarrierControl = r.BookCarrierControl,
                                                         .CarrierName = strCarName,
                                                         .CarrierNumber = intCarNumber,
                                                         .ShipCompControl = r.BookCustCompControl,
                                                         .ShipCompName = r.CompanyName,
                                                         .ShipCompNumber = r.CompanyNumber}

                    intLabelCt += 1
                End If
                oLabel.BookShipLabelDetails.Add(New DTO.BookShipLabelDetail With {.BookControl = r.BookControl,
                                                                                   .BookCarrOrderNumber = r.BookCarrOrderNumber,
                                                                                   .BookDateLoad = r.BookDateLoad,
                                                                                   .BookDateRequired = r.BookDateRequired,
                                                                                   .BookOrderSequence = r.BookOrderSequence,
                                                                                   .BookProNumber = r.BookProNumber,
                                                                                   .BookShipCarrierProControl = r.BookShipCarrierProControl,
                                                                                   .BookShipCarrierProNumber = r.BookShipCarrierProNumber,
                                                                                   .BookShipCarrierProNumberRaw = r.BookShipCarrierProNumberRaw,
                                                                                   .BookTotalCases = r.BookTotalCases,
                                                                                   .BookTotalCube = r.BookTotalCube,
                                                                                   .BookTotalPL = r.BookTotalPL,
                                                                                   .BookTotalPX = r.BookTotalPX,
                                                                                   .BookTotalWgt = r.BookTotalWgt})

                oLabel.populatePrintQty(LabelPrintMaxDefault)
            Next
            If Not oLabel Is Nothing Then oRet.Add(oLabel)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("getShippingLabels"))
        End Try
        Return oRet
    End Function

    Public Function getShippingLabel(ByVal BookControl As Integer, ByVal CarrierControl As Integer) As DTO.BookShipLabel
        Dim oRet As New DTO.BookShipLabel
        Try
            Dim blnDirty As Boolean
            Dim oData As New List(Of DTO.BookRevenue) From {NGLBookRevenueData.GetBookRevenueFiltered(BookControl)}
            If oData Is Nothing OrElse oData.Count < 1 OrElse oData(0).BookCarrierControl <> CarrierControl Then Return oRet 'return an empty ship label
            If Not updateBookSHID(oData, blnDirty, True) Then Return oRet 'return an empty ship label
            Dim LabelPrintMaxDefault = NGLBookRevenueData.GetParValue("LabelPrintMaxDefault", oData(0).BookCustCompControl)
            'get the carrier name and number
            Dim dicCarNameNbr = NGLCarrierData.getCarrierNameNumber(CarrierControl)
            If dicCarNameNbr Is Nothing OrElse dicCarNameNbr.Count < 1 OrElse Not dicCarNameNbr.ContainsKey("CarrierName") OrElse Not dicCarNameNbr.ContainsKey("CarrierNumber") Then Return oRet 'not a valid carrier
            Dim strCarName As String = dicCarNameNbr("CarrierName")
            Dim intCarNumber As Integer
            Integer.TryParse(dicCarNameNbr("CarrierNumber"), intCarNumber)
            Dim intLabelCt As Integer = 1
            Dim r = oData(0) 'we only need the first records this is a single order
            oRet = New DTO.BookShipLabel With {.BookShipLabelControl = intLabelCt,
                                                         .BookConsPrefix = r.BookConsPrefix,
                                                         .BookDestAddress1 = r.BookDestAddress1,
                                                         .BookDestAddress2 = r.BookDestAddress2,
                                                         .BookDestAddress3 = r.BookDestAddress3,
                                                         .BookDestCity = r.BookDestCity,
                                                         .BookDestCompControl = r.BookDestCompControl,
                                                         .BookDestCountry = r.BookDestCountry,
                                                         .BookDestName = r.BookDestName,
                                                         .BookDestState = r.BookDestState,
                                                         .BookDestZip = r.BookDestZip,
                                                         .BookOrigAddress1 = r.BookOrigAddress1,
                                                         .BookOrigAddress2 = r.BookOrigAddress2,
                                                         .BookOrigAddress3 = r.BookOrigAddress3,
                                                         .BookOrigCity = r.BookOrigCity,
                                                         .BookOrigCompControl = r.BookOrigCompControl,
                                                         .BookOrigCountry = r.BookOrigCountry,
                                                         .BookOrigName = r.BookOrigName,
                                                         .BookOrigState = r.BookOrigState,
                                                         .BookOrigZip = r.BookOrigZip,
                                                         .BookSHID = r.BookSHID,
                                                         .CarrierControl = r.BookCarrierControl,
                                                         .CarrierName = strCarName,
                                                         .CarrierNumber = intCarNumber,
                                                         .ShipCompControl = r.BookCustCompControl,
                                                         .ShipCompName = r.CompanyName,
                                                         .ShipCompNumber = r.CompanyNumber}


            oRet.BookShipLabelDetails.Add(New DTO.BookShipLabelDetail With {.BookControl = r.BookControl,
                                                                               .BookCarrOrderNumber = r.BookCarrOrderNumber,
                                                                               .BookDateLoad = r.BookDateLoad,
                                                                               .BookDateRequired = r.BookDateRequired,
                                                                               .BookOrderSequence = r.BookOrderSequence,
                                                                               .BookProNumber = r.BookProNumber,
                                                                               .BookShipCarrierProControl = r.BookShipCarrierProControl,
                                                                               .BookShipCarrierProNumber = r.BookShipCarrierProNumber,
                                                                               .BookShipCarrierProNumberRaw = r.BookShipCarrierProNumberRaw,
                                                                               .BookTotalCases = r.BookTotalCases,
                                                                               .BookTotalCube = r.BookTotalCube,
                                                                               .BookTotalPL = r.BookTotalPL,
                                                                               .BookTotalPX = r.BookTotalPX,
                                                                               .BookTotalWgt = r.BookTotalWgt})

            oRet.populatePrintQty(LabelPrintMaxDefault)

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("getShippingLabel"))
        End Try
        Return oRet
    End Function

#End Region

#Region "   Assign BookSHID"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oRevs"></param>
    ''' <param name="blnDataDirty">reference parameter used to determine if a save is still required</param>
    ''' <param name="blnAutoSave">allows the updateBookSHID to save changes if needed. oRevs will be refreshed with the latest changes</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function updateBookSHID(ByRef oRevs As List(Of DTO.BookRevenue), ByRef blnDataDirty As Boolean, Optional ByVal blnAutoSave As Boolean = True) As Boolean
        Dim blnRet As Boolean = False
        Dim strSHID As String
        Dim blnSaveRequired As Boolean = False
        blnDataDirty = False
        Try
            If oRevs Is Nothing OrElse oRevs.Count < 1 Then Return False
            If oRevs.Count = 1 Then
                Dim oBookRev = oRevs(0)
                'TODO: add logic to check for existing orders with the same CNS number that were not selected
                '   in oRevs.  typically where we have mixed consolidation integrity.
                If String.IsNullOrWhiteSpace(oBookRev.BookSHID) Then
                    If Not TryUpdateBookSHIDUsingCarrierAssignedPro(oBookRev) Then
                        'set the default to CNS or BookPro
                        If String.IsNullOrWhiteSpace(oBookRev.BookConsPrefix) Then
                            oBookRev.BookSHID = oBookRev.BookProNumber 'We assume that the book pro number is always unique
                        Else
                            strSHID = oBookRev.BookConsPrefix
                            Dim strSuffix As String = String.Empty
                            Do While NGLBookRevenueData.DoesBookSHIDExist(strSHID) 'The SHID must be unique for each freight bill
                                strSHID = String.Concat(oBookRev.BookConsPrefix, "-", updateAlphaCodeSuffix(strSuffix))
                            Loop
                            oBookRev.BookSHID = strSHID
                        End If
                    End If
                    oBookRev.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                    blnSaveRequired = True
                End If
            Else
                Dim blnDataChanged As Boolean = False

                strSHID = TryGetConsIntegritySHID(oRevs, blnDataChanged)
                If blnDataChanged Then blnSaveRequired = True
                blnDataChanged = False
                Dim blnLTLSHIDAssigned = TryGetLTLSHID(oRevs, blnDataChanged)
                If blnDataChanged Then blnSaveRequired = True

            End If

            'save the changes to the DB
            If blnSaveRequired Then
                If blnAutoSave Then
                    oRevs = NGLBookRevenueData.SaveRevenuesWDetails(oRevs.ToArray()).ToList()
                    blnDataDirty = False
                Else
                    blnDataDirty = True
                End If
            End If
            blnRet = True
        Catch ex As FaultException
            Logger.Error(ex, "Fault Exception - 1700")
            Throw
        Catch ex As Exception
            Logger.Error(ex, "updateBookSHID Error")
            throwUnExpectedFaultException(ex, buildProcedureName("updateBookSHID"))
        End Try
        Return blnRet
    End Function

    Private Function TryUpdateBookSHIDUsingCarrierAssignedPro(ByRef oBookRev As DTO.BookRevenue) As Boolean
        Dim blnRet As Boolean = False
        Try
            'Check For a Carrier Assigned Pro
            If String.IsNullOrWhiteSpace(oBookRev.BookShipCarrierProNumber) Then
                If oBookRev.BookCarrTarEquipControl <> 0 Then
                    'try to get the next carrier assigned pro number
                    Try
                        'Update the shid to the Carrier Pro Number Raw if it is available
                        Dim oCarrProResults = CarrierBLL.GetCarrierProNumberByEquip(oBookRev.BookCarrTarEquipControl)
                        If Not oCarrProResults Is Nothing AndAlso oCarrProResults.CarrProControl <> 0 Then
                            oBookRev.BookSHID = oCarrProResults.CarrierProNumberRaw
                            oBookRev.BookShipCarrierProNumber = oCarrProResults.CarrierProNumber
                            oBookRev.BookShipCarrierProNumberRaw = oCarrProResults.CarrierProNumberRaw
                            oBookRev.BookShipCarrierProControl = oCarrProResults.CarrProControl
                            blnRet = True
                        End If
                    Catch ex As Exception
                        DAL.Utilities.SaveAppError("Unexpected GetCarrierProNumberByEquip Error: " & ex.Message, Me.Parameters)
                        Return False
                    End Try
                End If
            Else
                'The Carrier Pro has already been assigned so  
                'Update the shid to the Carrier Pro Number Raw if it is available
                oBookRev.BookSHID = If(String.IsNullOrWhiteSpace(oBookRev.BookShipCarrierProNumberRaw), oBookRev.BookShipCarrierProNumber, oBookRev.BookShipCarrierProNumberRaw)
                blnRet = True
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            DAL.Utilities.SaveAppError("Unexpected TryUpdateBookSHIDUsingCarrierAssignedPro Error: " & ex.Message, Me.Parameters)
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Updates the CNS, CarrierProNumber and SHID for all orders in the list that have the same CNS number and consolidation integrity is on.
    ''' The caller should check for a single order and call TryUpdateBookSHIDUsingCarrierAssignedPro instead of TryGetConsIntegritySHID.
    ''' all other orders must be processed as LTL by the caller. 
    ''' The procedure sets blnSaveRequired to true if changes 
    ''' have been made; the caller is responsible for saving changes to the database.
    ''' </summary>
    ''' <param name="oRevs"></param>
    ''' <param name="blnSaveRequired"></param>
    ''' <returns>
    ''' Returns the selected SHID or an empty string if the SHID cannot be identified
    ''' </returns>
    ''' <remarks>
    ''' Business Rules for SHID, CNS and Carrier Assigned Pro Number when Consolidation Integrity is on; applied when a load is tendered:
    ''' 1. All CNS #s where consolidation integrity is on must be the same or blank.  Throws Invalid Operation if CNS numbers are not unique or blank
    ''' 2. If all CNS #s are blank we get the next available CNS #.  Throws Invalid Operation if a CNS numbers cannot be generated.
    ''' 3. All blank CNS #s are updated with the selected CNS.
    ''' 4. All Carrier Assigned Pro #s must be the same or blank.  Throws Invalid Operation if PRO numbers are not unique or blank.
    ''' 5. carrier pro uses the CarrTarEquipmentControl they all must be the same. Throws Invalid Operation if the CarrTarEquipmentControl are not the same.
    ''' 6. If all Carrier Assigned Pro #s are blank we try to get the next available value from the carrier/tariff. 
    ''' 7. If a Carrier Assigned Pro is available all blank values are updated with the selected value.
    ''' 8. All SHIDs will be updated using the selected carrier pro number or the selected CNS number if the carrier pro number is blank.
    ''' 9. Each SHID is validated for exists. if the value already exists a new SHID is created with the next available alpha code and rechecked this continue while the value already exists
    ''' 10. Master Billing rules only apply to LTL Pool where consolidation integrity is off
    ''' 
    ''' Modified by LVV on 10/21/16 for v-7.0.5.110 SHID -A Bug
    '''  Write For Each on oIntegirtyOnPros and check for any non-empty SHIDs that do not match strConIntDefaultSHID
    '''  If at least one matches and none are different excluding blanks we can use the default
    '''  All SHIDs must match or be blank otherwise we have to test for existing
    ''' </remarks>
    Private Function TryGetConsIntegritySHID(ByRef oRevs As List(Of DTO.BookRevenue), ByRef blnSaveRequired As Boolean) As String
        Dim strConIntDefaultSHID As String = ""
        blnSaveRequired = False
        Try

            Dim strCarrierPro As String
            Dim strCarrierProRaw As String
            Dim strCNS As String
            Dim intCompControl As Integer

            Dim intCarrierProControl As Integer?
            Dim oIntegirtyOnPros = (From d In oRevs Where d.BookRouteConsFlag = True Select d).ToList()
            If oIntegirtyOnPros Is Nothing OrElse oIntegirtyOnPros.Count < 1 Then Return Nothing 'no orders have consolidation integrity turned on
            'We skip any orders where Consolidatin integrity is uncheked, these must be treated as LTL by caller
            'get the CNS number
            strCNS = (From d In oIntegirtyOnPros Where Not String.IsNullOrWhiteSpace(d.BookConsPrefix) Select d.BookConsPrefix).FirstOrDefault()
            If String.IsNullOrWhiteSpace(strCNS) Then
                'Rule 2. get the next available CNS number
                'we need a company 
                intCompControl = (From d In oIntegirtyOnPros Order By d.BookPickupStopNumber, d.BookStopNo Select d.BookCustCompControl).FirstOrDefault()
                strCNS = NGLBatchProcessData.GetNextConsNumber(intCompControl)
                'we cannot get a CNS number
                throwInvalidOperatonException("Cannot generate a Consolidation Number.")
                Return ""
            Else
                'Rule 1. validate the CNS number
                If (From d In oIntegirtyOnPros
                    Where Not String.IsNullOrWhiteSpace(d.BookConsPrefix) _
                        AndAlso
                        String.Compare(d.BookConsPrefix.Trim, strCNS.Trim) <> 0).Any() Then
                    'we have more than one CNS number so throw the invalid operation exception
                    throwInvalidOperatonException("Too many Consolidation Numbers; values must be unique when consolidation integrity is checked.")
                    Return ""
                End If
            End If
            'Update Carrier Assigned Pro #s.
            Dim BookRevProAssigned = (From d In oIntegirtyOnPros Where Not String.IsNullOrWhiteSpace(d.BookShipCarrierProNumber) Select d).FirstOrDefault()
            If Not BookRevProAssigned Is Nothing AndAlso BookRevProAssigned.BookControl <> 0 Then
                strCarrierPro = BookRevProAssigned.BookShipCarrierProNumber
                strCarrierProRaw = BookRevProAssigned.BookShipCarrierProNumberRaw
                intCarrierProControl = BookRevProAssigned.BookShipCarrierProControl
                'Rule 4: All Carrier Assigned Pro #s must be the same or blank.
                If (From d In oIntegirtyOnPros
                    Where Not String.IsNullOrWhiteSpace(d.BookShipCarrierProNumber) _
                       AndAlso
                       String.Compare(d.BookShipCarrierProNumber.Trim, strCarrierPro.Trim) <> 0).Any() Then
                    'we have more than one Carrier Pro Number so throw the invalid operation exception
                    throwInvalidOperatonException("Too many Carrier Assigned Pro Numbers; values must be unique when consolidation integrity is checked.")
                    Return ""
                End If
            Else
                'A carrier pro has not been assigned so try to get one
                Dim oAssignedEquip = (From d In oIntegirtyOnPros Where d.BookCarrTarEquipControl <> 0 Select d).ToList()
                If Not oAssignedEquip Is Nothing AndAlso oAssignedEquip.Count > 0 Then
                    'we have a match so check for unique values
                    Dim intEquipControl As Integer
                    For Each e In oAssignedEquip
                        If intEquipControl = 0 Then
                            intEquipControl = e.BookCarrTarEquipControl
                        ElseIf intEquipControl <> e.BookCarrTarEquipControl Then
                            'throw an exception the equipment does not match
                            'Rule 6. carrier pro uses the CarrTarEquipmentControl they all must be the same. Throws Invalid Operation if the CarrTarEquipmentControl are not the same.
                            throwInvalidOperatonException("Invalid Tariff Equipment Assignment; each order on a Consolidation where Consolidation Integrity is checked must have the same Tariff Equipment Rate assigned.  Please re-assign the transportation provider and try again.")
                            Return ""
                        End If
                    Next
                    'Rule 6. If all Carrier Assigned Pro #s are blank we try to get the next available value from the carrier/tariff.
                    If intEquipControl <> 0 Then
                        Dim oCarrProResults = CarrierBLL.GetCarrierProNumberByEquip(intEquipControl)
                        If Not oCarrProResults Is Nothing AndAlso oCarrProResults.CarrProControl <> 0 Then
                            strCarrierProRaw = oCarrProResults.CarrierProNumberRaw
                            strCarrierPro = oCarrProResults.CarrierProNumber
                            intCarrierProControl = oCarrProResults.CarrProControl
                        End If
                    End If
                End If
            End If

            If Not String.IsNullOrWhiteSpace(strCarrierPro) Then
                If Not String.IsNullOrWhiteSpace(strCarrierProRaw) Then
                    strConIntDefaultSHID = strCarrierProRaw
                Else
                    strConIntDefaultSHID = strCarrierPro
                End If
            Else
                'use the CNS number
                strConIntDefaultSHID = strCNS
                Dim strSuffix As String = String.Empty
                'Modified by LVV on 10/21/16 for v-7.0.5.110 SHID -A Bug
                'Write For Each on oIntegirtyOnPros and check for any non-empty SHIDs that do not match strConIntDefaultSHID
                'If at least one matches and none are different excluding blanks we can use the default
                'All SHIDs must match or be blank otherwise we have to test for existing
                Dim blnMatchFound As Boolean = False
                Dim blnUnMatchedFound As Boolean = False
                For Each i In oIntegirtyOnPros
                    If i.BookSHID.ToUpper().Trim() = strConIntDefaultSHID.ToUpper().ToUpper() Then
                        blnMatchFound = True
                        Continue For
                    End If
                    If Not String.IsNullOrWhiteSpace(i.BookSHID) Then blnUnMatchedFound = True
                Next
                If Not blnMatchFound Or blnUnMatchedFound Then
                    Do While NGLBookRevenueData.DoesBookSHIDExist(strConIntDefaultSHID) 'The SHID must be unique for each freight bill
                        strConIntDefaultSHID = String.Concat(strCNS, "-", updateAlphaCodeSuffix(strSuffix))
                    Loop
                End If

            End If
            For Each c In oIntegirtyOnPros
                If c.BookConsPrefix <> strCNS Then
                    'Rule 3. update all cns numbers
                    blnSaveRequired = True
                    c.BookConsPrefix = strCNS
                End If
                If c.BookShipCarrierProNumber <> strCarrierPro Then
                    c.BookShipCarrierProNumber = strCarrierPro
                    blnSaveRequired = True
                End If
                If c.BookShipCarrierProControl <> intCarrierProControl Then
                    c.BookShipCarrierProControl = intCarrierProControl
                    blnSaveRequired = True
                End If
                If c.BookShipCarrierProNumberRaw <> strCarrierProRaw Then
                    c.BookShipCarrierProNumberRaw = strCarrierProRaw
                    blnSaveRequired = True
                End If
                If c.BookSHID <> strConIntDefaultSHID Then
                    c.BookSHID = strConIntDefaultSHID
                    blnSaveRequired = True
                End If
                c.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
            Next

        Catch ex As FaultException
            Throw
        Catch ex As Exception
            DAL.Utilities.SaveAppError("Unexpected TryGetConsIntegritySHID error; data processing was not affected message saved to app error log: " & ex.Message, Me.Parameters)
        End Try
        Return strConIntDefaultSHID
    End Function


    ''' <summary>
    ''' Returns a list of orders that can be master billed. 
    ''' </summary>
    ''' <param name="b"></param>
    ''' <param name="l"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Rules for master billing:
    '''    a) each order needs to be assigned the same origin based on BookOrigCompControl or BookCustCompControl if  BookOrigCompControl is empty or zero
    '''    b) Mastr Billing is only applied to outbound LTL Pool loads 
    '''    c) each order needs to be assigned the same carrier.
    '''    d) consolidation integrity must be off
    '''    e) each order must be delivered at the same country, city, state, and street address.
    ''' </remarks>
    Private Function TryGetMasterBilling(ByVal b As DTO.BookRevenue, ByVal l As List(Of DTO.BookRevenue)) As List(Of DTO.BookRevenue)
        Dim r As New List(Of DTO.BookRevenue)
        Try

            Dim m = (From d In l
                     Where
                d.BookCarrierControl = b.BookCarrierControl _
                And
                d.BookOrigAddress1 = d.BookOrigAddress1 _
                And
                d.BookDestAddress1 = b.BookDestAddress1 _
                And
                (
                        (
                            b.LaneOriginAddressUse = False _
                            AndAlso
                            (If(b.BookOrigCompControl = 0, b.BookCustCompControl, b.BookOrigCompControl) = If(d.BookOrigCompControl = 0, d.BookCustCompControl, d.BookOrigCompControl))
                        ) _
                        OrElse
                        (
                            b.LaneOriginAddressUse = True _
                            AndAlso
                            (If(b.BookDestCompControl = 0, b.BookCustCompControl, b.BookDestCompControl) = If(d.BookDestCompControl = 0, d.BookCustCompControl, d.BookDestCompControl))
                        )
                )
                     Select d).ToList()
            If Not m Is Nothing AndAlso m.Count > 0 Then Return m
        Catch ex As Exception
            'ignore any errors in TryGetMasterBilling just return an empty list
        End Try

        Return r

    End Function

    Private Function canMasterBill(ByVal p As DTO.BookRevenue, ByVal c As DTO.BookRevenue) As Boolean
        Dim blnRet As Boolean = False
        If (p Is Nothing OrElse p.BookControl = 0) OrElse (c Is Nothing OrElse c.BookControl = 0) Then Return False

        Dim pOrigCompControl As Integer = If(p.BookOrigCompControl = 0, p.BookCustCompControl, p.BookOrigCompControl)
        Dim cOrigCompControl As Integer = If(c.BookOrigCompControl = 0, c.BookCustCompControl, c.BookOrigCompControl)

        If p.BookCarrierControl = c.BookCarrierControl _
            And pOrigCompControl = cOrigCompControl _
            And p.BookDestCountry = c.BookDestCountry _
            And p.BookDestState = c.BookDestState _
            And p.BookDestCity = c.BookDestCity _
            And p.BookDestAddress1 = c.BookDestAddress1 Then
            blnRet = True
        End If

        Return blnRet

    End Function

    ''' <summary>
    ''' Updates the SHID for LTL orders Groupeds by Company, CNS, Carrier Pro/SHID and Master Billing.
    ''' The caller should check for a single order and call TryUpdateBookSHIDUsingCarrierAssignedPro before calling this method.
    ''' The caller must process orders with consolidation integrity on by using TryGetConsIntegritySHID before calling this method. 
    ''' The procedure sets blnSaveRequired to true if changes 
    ''' have been made; the caller is responsible for saving changes to the database.
    ''' </summary>
    ''' <param name="oRevs"></param>
    ''' <param name="blnSaveRequired"></param>
    ''' <returns>
    ''' Returns true on success or false on failure
    ''' </returns>
    ''' <remarks>
    ''' Business Rules for SHID, CNS, Carrier Assigned Pro Number for LTL Pool truck orders when consolidation integrity is off; applied when a load is tendered:
    ''' 1. Only orders where Consolidaton Integrity will be processed.
    ''' 2. All orders require a CNS number.  If the CNS number is blank a new one will be assigned.
    ''' 3. Orders are grouped by CNS number and Carrier Assigned Pro Number.
    ''' 4. If the Carrier Pro Number is empty the system will attempt to select on using the carrier/tariff configuration.
    ''' 5. All orders will be checked for Master Billing.  
    ''' 6. Orders will be assigned a Carrier Pro Number where possible sequentially.  
    ''' 7. The system will check for master billing is possible before moving on to the next order;
    '''    the CNS number, Carrier Assigned Pro Number and BookSHID of the first record found will be used 
    '''    for all master billed orders unless the first record's Carrier Assigned Pro Number is empty;  
    '''    In that case each master billed orders will be tested for a previously assigned carrier pro number; 
    '''    the first one found will be used.
    ''' 8. If a Carrier Pro Nunmber is available it will be used for the SHID.  If not the CNS-Alpha Code will be used as the SHID
    ''' TODO:  we need to find out what to do if the BookCarrTarEquipControl does not match when master billing?
    '''        for now we throw an Invalid Operation exception
    ''' </remarks>
    Private Function TryGetLTLSHID(ByRef oRevs As List(Of DTO.BookRevenue), ByRef blnSaveRequired As Boolean) As Boolean
        Dim strLTLSHID As String = ""
        Dim strSuffix As String = ""
        blnSaveRequired = False
        Dim blnSuccess As Boolean = False
        Try

            Dim strCarrierPro As String
            Dim strCarrierProRaw As String
            Dim intCarrierProControl As Integer?
            Dim strCNS As String = ""
            Dim intBookCarrTarEquipControl As Integer

            Dim oPreviousBooking As DTO.BookRevenue = Nothing
            Dim strUsedSHIDs As New List(Of String)
            'Rule 3. Orders are sorted by carrier, cns, origin, destination, bookshid, then BookShipCarrierProNumber.
            Dim oIntegirtyOffPros = (From d In oRevs Where d.BookRouteConsFlag = False Order By d.BookCarrierControl, d.BookConsPrefix Descending, d.BookOrigCompControl, d.BookCustCompControl, d.BookDestCountry, d.BookDestState, d.BookDestCity, d.BookDestAddress1, d.BookSHID, d.BookShipCarrierProNumber Descending Select d).ToList()
            If oIntegirtyOffPros Is Nothing OrElse oIntegirtyOffPros.Count < 1 Then Return True 'Rule 1: no orders have consolidation integrity turned off
            'when assigning the SHID the caller must include the IncludeLTLPool flag so that all records with the same 
            'CNS number are returned based on the selected booking record.  we will have all orders with consolidaiton integrity off 
            'or all orders with consolidaiton integrity on.  if consolidation integrity is off we will have 
            'one single order or an LTL pool truck. for the LTL Pool truck the CNS numbers will already have been 
            'assigned and will already be the same.  for a single order we could have an empty CNS number so we need to 
            'get a new CNS if needed.
            'get the first CNS number available
            strCNS = (From d In oIntegirtyOffPros Where Not String.IsNullOrWhiteSpace(d.BookConsPrefix) Select d.BookConsPrefix).FirstOrDefault()
            If String.IsNullOrWhiteSpace(strCNS) Then strCNS = NGLBatchProcessData.GetNextConsNumber(oIntegirtyOffPros(0).BookCustCompControl) 'get the next available cns number


            Dim lProcessedBooks As New List(Of Integer)
            Dim blnSHIDUsed As Boolean = False
            For Each r In oIntegirtyOffPros
                'check lProcessedBooks for current record
                If lProcessedBooks Is Nothing Then lProcessedBooks = New List(Of Integer)
                If lProcessedBooks.Count < 1 OrElse Not lProcessedBooks.Contains(r.BookControl) Then
                    'add the current bookcontrol to the processed list
                    lProcessedBooks.Add(r.BookControl)
                    'Rule 2: All orders require a CNS number and if oIntegirtyOffPros.count is > 1 then this is an LTL Pool load so all orders 
                    ' will have the same CNS number.
                    Dim blnCanMasterBill As Boolean = False
                    blnCanMasterBill = canMasterBill(oPreviousBooking, r)

                    If oPreviousBooking Is Nothing _
                        OrElse oPreviousBooking.BookControl = 0 _
                        OrElse Not blnCanMasterBill Then
                        'we need to get a new SHID
                        '4. If the Carrier Pro Number is empty the system will attempt to select one using the carrier/tariff configuration.
                        strCarrierPro = r.BookShipCarrierProNumber
                        strCarrierProRaw = r.BookShipCarrierProNumberRaw
                        intCarrierProControl = r.BookShipCarrierProControl
                        intBookCarrTarEquipControl = r.BookCarrTarEquipControl
                        If Not String.IsNullOrWhiteSpace(r.BookSHID) Then
                            strLTLSHID = r.BookSHID 'use the previously assigned shid number
                        Else
                            If String.IsNullOrWhiteSpace(r.BookShipCarrierProNumber) And r.BookCarrTarEquipControl <> 0 Then
                                Dim oCarrProResults = CarrierBLL.GetCarrierProNumberByEquip(r.BookCarrTarEquipControl)
                                If Not oCarrProResults Is Nothing AndAlso oCarrProResults.CarrProControl <> 0 Then
                                    strCarrierProRaw = oCarrProResults.CarrierProNumberRaw
                                    strCarrierPro = oCarrProResults.CarrierProNumber
                                    intCarrierProControl = oCarrProResults.CarrProControl
                                End If
                            End If
                            If Not String.IsNullOrWhiteSpace(strCarrierPro) Then
                                If Not String.IsNullOrWhiteSpace(strCarrierProRaw) Then
                                    strLTLSHID = strCarrierProRaw
                                Else
                                    strLTLSHID = strCarrierPro
                                End If
                            Else
                                strLTLSHID = strCNS
                                'test for unique shid any time we use the CNS as the source
                                blnSHIDUsed = Not strUsedSHIDs Is Nothing AndAlso strUsedSHIDs.Count > 0 AndAlso strUsedSHIDs.Contains(strLTLSHID)

                                Do While blnSHIDUsed OrElse NGLBookRevenueData.DoesBookSHIDExist(strLTLSHID)
                                    strLTLSHID = String.Concat(strCNS, "-", updateAlphaCodeSuffix(strSuffix))
                                    blnSHIDUsed = Not strUsedSHIDs Is Nothing AndAlso strUsedSHIDs.Count > 0 AndAlso strUsedSHIDs.Contains(strLTLSHID)
                                Loop
                                'add the SHID to the list
                                strUsedSHIDs.Add(strLTLSHID)
                            End If
                        End If
                    End If
                    If blnCanMasterBill Then
                        'validate the master billed BookCarrTarEquipControl data
                        If r.BookCarrTarEquipControl <> intBookCarrTarEquipControl Then
                            throwInvalidOperatonException("Invalid Tariff Equipment Assignment; some LTL orders can be master billed but they do not have the same Tariff Equipment Rate assigned.  Please re-assign the transportation provider and try again.")
                            Return False
                        End If
                    End If
                    'Rule 2: All orders require a CNS number and if oIntegirtyOffPros.count is > 1 then this is an LTL Pool load so all orders 
                    ' will have the same CNS number.
                    If r.BookConsPrefix <> strCNS Then
                        r.BookConsPrefix = strCNS
                        blnSaveRequired = True
                    End If
                    If r.BookShipCarrierProNumber <> strCarrierPro Then
                        r.BookShipCarrierProNumber = strCarrierPro
                        blnSaveRequired = True
                    End If
                    If r.BookShipCarrierProControl <> intCarrierProControl Then
                        r.BookShipCarrierProControl = intCarrierProControl
                        blnSaveRequired = True
                    End If
                    If r.BookShipCarrierProNumberRaw <> strCarrierProRaw Then
                        r.BookShipCarrierProNumberRaw = strCarrierProRaw
                        blnSaveRequired = True
                    End If
                    If r.BookSHID <> strLTLSHID Then
                        r.BookSHID = strLTLSHID
                        blnSaveRequired = True
                    End If
                    r.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                End If
                'save the currently selected values
                oPreviousBooking = r
            Next
            blnSuccess = True
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            DAL.Utilities.SaveAppError("Unexpected TryGetLTLSHID Error: " & ex.Message, Me.Parameters)
            Throw
        End Try
        Return blnSuccess
    End Function

    Private Function updateAlphaCodeSuffix(ByRef strSuffix As String) As String

        If String.IsNullOrWhiteSpace(strSuffix) Then
            strSuffix = "A"
        Else
            Dim iLen = strSuffix.Trim.Length
            Dim iAsc = Asc(strSuffix.Substring(0, 1))
            If iAsc > 89 Then
                iAsc = 64
                iLen += 1
            End If
            'Console.WriteLine("Ascii = {0} Char = {1} Loop = {2}", iAsc, Chr(iAsc), iLen)
            Dim sb As New System.Text.StringBuilder()
            For i As Integer = 1 To iLen
                sb.Append(Chr(iAsc + 1))
                ' Console.WriteLine("Loop = {0}", i)
            Next
            strSuffix = sb.ToString()
        End If


        Return strSuffix

    End Function

#End Region

#Region "  Rate IT"

    'Overview
    'Step 1: User manually enters order details or select an existing order
    '        Includes Dimensional Packages; User Entered Fees and Dispatch Type
    '        If using API amount/value of fees is determined via API results
    '         however, users should enter a default flat rate value for all fees that are expected 
    '       Carrier API or Tariff values will replace default flat rate value where available
    'Step 2: Insert Load Board Records From Selected Book Control or From Manual Order Data Entry into Load Tender Table
    '        map fees, dimensions and types to child tables of Load Tender table where available
    'Step 3: Request Quotes from API or Tariff as requested
    '        Note: spot rate quote is included in step 2
    '        Write Quotes to tblBid table associated with Load Tender Table
    '        NEXTStop bids are used to compare API and Tariff rates with carrier bids
    'Step 4: Return Bids to user, user selects bid to dispatch
    'Step 5: Dispatch seleted bid to carrier:
    '           API: gets sent to P44 for accept reject on accept load is finalized (user cannot auto accept)
    '           Tariff: gets sent via EDI or NEXTrack and are set to Tendered unless user select the AutoAccept option
    '           NEXTStop bids are marked as Accepted (note: the dispatch dialog may or may not be used for NEXStop)
    '           Update BookRev data with selected carrier.
    '           Note: SpotRate and NGL Tariff loads are recalculated upon dispatching.  If the costs have changes we need to notify the user that the bid does not match current rates.
    'Step 6: If everything is successful archive all previous bids associated with this load.



    Public Function GenerateQuote(ByVal order As DAL.Models.RateRequestOrder, ByVal tenderTypes() As DTO.tblLoadTender.LoadTenderTypeEnum, bidTypes() As DTO.tblLoadTender.BidTypeEnum, Optional ByVal BookControl As Integer = 0, Optional ByVal tariffOptions As DTO.GetCarriersByCostParameters = Nothing) As DTO.WCFResults
        Try
            Try
                Dim telemetry As TelemetryClient = New TelemetryClient()

                telemetry.TrackTrace("GenerateQuote_Log", DataContracts.SeverityLevel.Information, New Dictionary(Of String, String) From {
                        {"order", JsonConvert.SerializeObject(order)},
                        {"tenderTypes", JsonConvert.SerializeObject(tenderTypes)},
                        {"bidTypes", JsonConvert.SerializeObject(bidTypes)},
                        {"BookControl", JsonConvert.SerializeObject(BookControl)},
                        {"tariffOptions", JsonConvert.SerializeObject(tariffOptions)},
                        {"paramUserName", Me.Parameters.UserName}
                    })
                Serilog.Log.Information("GenerateQuote_Log", New Dictionary(Of String, String) From {
                        {"order", JsonConvert.SerializeObject(order)},
                        {"tenderTypes", JsonConvert.SerializeObject(tenderTypes)},
                        {"bidTypes", JsonConvert.SerializeObject(bidTypes)},
                        {"BookControl", JsonConvert.SerializeObject(BookControl)},
                        {"tariffOptions", JsonConvert.SerializeObject(tariffOptions)},
                        {"paramUserName", Me.Parameters.UserName}
                    })
            Catch ex As Exception
                'Swallow any exception trying to log. Don't crash over this.
            End Try

            'Wrapper to call the real work
            Using Logger.StartActivity("GenerateQuote(Order: {Order}, tenderTypes: {TenderTypes}, BidTypes: {BidTypes}, BookControl: {BookControl}, TariffOptions: {TariffOptions}", order, tenderTypes, bidTypes, BookControl, tariffOptions)
                Return Inner_GenerateQuote(order, tenderTypes, bidTypes, BookControl, tariffOptions)
            End Using
        Catch ex As Exception
            Logger.Error(ex, "Exception in GenerateQuote")
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Generate a rate quote using the ratetype provided and returns the LoadTenderControl of the associated load tender record
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="tenderTypes"></param
    ''' <param name="bidTypes"></param>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 12/12/2018
    '''     returns a DTO.WCFResults with 
    '''     Success = true or fals
    '''     KeyField "LoadTenderControl" 
    '''     KeyField "RetMsg" as English message not localized
    '''     Errors Dictionary with Localiztion key  and list of strings for formating
    ''' Modified by RHR for v-8.2 on 12/22/2018
    '''     Load Tender Types (P44 and RateQuote) were mdified as RateShopping and LoadBoard
    '''     to determine how the rates were generated.  
    '''     So we now use bidTypes to identify which rates to get
    '''     this way we can get different bid types based on user configuration for each Tender Type.
    ''' Modified by RHR for v-8.2.1 on 11/12/2019
    '''     Added logic to use prefered temperature and mode.
    '''     we now pass in filters and call   
    ''' Modified by RHR for v-8.2.1.006 on 04/15/2020
    '''     Added logic to log p44 errors and continue to run NGL quotes when
    '''     the API fails (new Try Catch Logic)
    ''' Modified by RHR for v-8.3.0.002 on 10/20/2020
    '''     added logic to only call CreateNGLTariffBidNoBookAsync when we are not using APIs
    '''     modified CreateRateRequestOrderQuote to pass blnUseP44API by reference so we 
    '''     can get the modified value back
    ''' Modified by RHR for v-8.3.0.002 on 12/17/2020
    '''     Added logic to process messages async
    ''' Modified by RHR for v-8.5.3.001 on 05/25/2022 we now use InsertNGLTariffBid365
    '''     added optional NGLTariffOptions
    ''' Modified by RHR for v-8.5.3.005 on 08/24/2022 added logic to use bookcontrol to look up Legal Entity because system users are not assigned to a legal entity
    '''       typically used for auto carrier selection
    ''' Modified by RHR for v-8.5.4.004 on 11/29/2023 added logic to read SSOA configurations for all API carriers
    '''     we now let the API Integraion Library determine how to communicate with 
    '''     carriers/partners.  P44 is still stand alone.
    ''' </remarks>
    Private Function Inner_GenerateQuote(ByVal order As DAL.Models.RateRequestOrder, ByVal tenderTypes() As DTO.tblLoadTender.LoadTenderTypeEnum, bidTypes() As DTO.tblLoadTender.BidTypeEnum, Optional ByVal BookControl As Integer = 0, Optional ByVal tariffOptions As DTO.GetCarriersByCostParameters = Nothing) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()

        Using operation = Logger.StartActivity("Inner_GenerateQuote(Order: {Order}, tenderTypes: {TenderTypes}, BidTypes: {BidTypes}, BookControl: {BookControl}, TariffOptions: {TariffOptions}", order, tenderTypes, bidTypes, BookControl, tariffOptions)
            Dim iLoadTenderControl As Integer = 0
            Dim strMsg As String = ""
            Dim oLTLogData As New DAL.NGLLoadTenderLogData(Me.Parameters)
            oRet.Success = False
            oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())

            Dim oldLogger = Logger



            'TODO: determine if we need to show a message to the user,  these options should not happen in production.
            Logger.Information("GenerateQuote called with order: {@0}\n tenderTypes: {@1}\n bidTypes: {@2}\n BookControl: {@3}\n tariffOptions: {@4}", order, tenderTypes, bidTypes, BookControl, tariffOptions)


            'Hack override to always add FSC
            'If (order?.Accessorials.Contains("FSC") = False) Then
            '    order.Accessorials = order.Accessorials.Append("FSC").ToArray()
            'End If

            If tenderTypes Is Nothing OrElse tenderTypes.Count < 1 Then Return oRet
            If bidTypes Is Nothing OrElse bidTypes.Count < 1 Then Return oRet

            If tariffOptions Is Nothing Then
                tariffOptions = New DTO.GetCarriersByCostParameters(False, True, True, 0, 0) 'use default for standard routing prefered = false, noLateDelivery = False, validated = True, optimizeByCapacity = True --  modeTypeControl and  tempType  are looked up later
                Logger.Information("Update tarriffOptions:\n {@0}", tariffOptions)
            End If
            Try
                Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)
                Dim oDATBLL As BLL.NGLDATBLL = New BLL.NGLDATBLL(Parameters)
                Dim oBookDAL As New DAL.NGLBookData(Parameters)
                Dim oBookRev As New DAL.NGLBookRevenueData(Parameters)
                Dim oAPIBLL As New NGLAPIBLL(Me.Parameters)
                Dim oItems As New List(Of DAL.Models.RateRequestItem)
                'the roles may be reversed but the results should be the same.
                Dim oP44Proxy As P44.P44Proxy
                Dim oP44Data As P44.RateRequest = New P44.RateRequest()
                Dim blnUseP44API As Boolean = False
                Dim blnUseNGLTariff As Boolean = False
                Dim blnUseCHRAPI As Boolean = False
                Dim blnUseUPSAPI As Boolean = False
                Dim blnUseJTSAPI As Boolean = False
                Dim blnUseHMBAPI As Boolean = False
                Dim blnUseFFEAPI As Boolean = False

                Dim oCHRLEConfig As New LTS.tblSSOALEConfig()
                Dim lCHRCompConfig As New List(Of LTS.tblSSOAConfig)

                Dim oJTSLEConfig As New LTS.tblSSOALEConfig()
                Dim lJTSCompConfig As New List(Of LTS.tblSSOAConfig)

                Dim oUPSLEConfig As New LTS.tblSSOALEConfig()
                Dim lUPSCompConfig As New List(Of LTS.tblSSOAConfig)

                Dim oHMBLEConfig As New LTS.tblSSOALEConfig()
                Dim lHMBCompConfig As New List(Of LTS.tblSSOAConfig)

                Dim oFFELEConfig As New LTS.tblSSOALEConfig()
                Dim lFFECompConfig As New List(Of LTS.tblSSOAConfig)

                Dim iCompControl As Integer = 0
                oLTLogData.AddToCollection("Read quote and bid types")
                Dim lAPICarrierConfigs As New List(Of LTS.tblSSOALEConfig)
                For Each bt In bidTypes
                    Logger.Information("Check for bid type: {0}", bt)
                    If bt = DTO.tblLoadTender.BidTypeEnum.P44 Then
                        'P44 = 3  the Rate It option only allows 3 for API or 2 for Tariff so all 3s are API
                        'Modified by RHR for v-8.5.4.1 on 06/30/2023 new rate shopping temperature logic
                        Logger.Information("Check for P44 API, order: {@0}", order)
                        If (order Is Nothing) Then
                            blnUseP44API = True ' ToDo: add logic below to test for temperature on booking record data.
                        ElseIf (order.TariffTempType <= 1) Then '0 (any) and 1 (dry) are ok for p44
                            blnUseP44API = True
                        End If
                        'Modified by RHR for v-8.5.3.005 on 08/24/2022 added logic to use bookcontrol to get legal entity
                        ' get the SSOA settings if the bookcontrol does not exist GetSSOAConfig will use the users legal entity
                        'Modified by RHR for v-8.5.4.004 on 11/29/2023 get all configs for any API configured for Rate Shopping RateRequestOut = 1
                        ' removed blnUseCHRAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.CHRAPI, oCHRLEConfig, lCHRCompConfig, BookControl)
                        ' removed blnUseJTSAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.JTSAPI, oJTSLEConfig, lJTSCompConfig, BookControl)
                        'removed blnUseUPSAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.UPSAPI, oUPSLEConfig, lUPSCompConfig, BookControl)
                        'the API module must call GetSSOAConfigs to get the key/value mapping details
                        Try
                            lAPICarrierConfigs = NGLtblSingleSignOnAccountData.GetSSOALEConfigsBySSOAType(DAL.Utilities.SSOAType.RateRequestOut, BookControl)
                        Catch ex As Exception
                            Logger.Error(ex, "Error getting SSOA Configs for RateRequestOut")

                        End Try
                    ElseIf bt = DTO.tblLoadTender.BidTypeEnum.NGLTariff Then
                        blnUseNGLTariff = True
                        Logger.Information("Set blnUseNGLTariff = True - 2343")
                        'ElseIf bt = DTO.tblLoadTender.BidTypeEnum.d Then
                    End If
                Next

                Dim blnAPIRatesPossible As Boolean = False
                If Not lAPICarrierConfigs Is Nothing AndAlso lAPICarrierConfigs.Count() > 0 Then
                    blnAPIRatesPossible = True
                End If
                Dim blnP44QuotesExist As Boolean = False
                Select Case tenderTypes(0)
                    Case DTO.tblLoadTender.LoadTenderTypeEnum.LoadBoard
                        Using (LogContext.PushProperty("TenderType", "LoadBoard"))
                            oLTLogData.AddToCollection("Generate quotes from Load Board")
                            Logger.Information("Generate quotes from Load Board - 2356")
                            'TODO: determine if we need to raise an error or show a message to the user 
                            'that this option is not allowed?  This should not happen in production.
                            If Not (blnAPIRatesPossible Or blnUseP44API Or blnUseNGLTariff) Then
                                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "Cannot generate quotes an API or Tariff quote Type is required")
                                Return oRet
                            End If

                            If Not order Is Nothing Then
                                Using LogContext.PushProperty("Order", order)
                                    Logger.Information("Insert Load Board Records for BookControl: {BookControl}, tenderTypes: {tenderType}, which is theoretically impossible according to inputs...", BookControl, tenderTypes(0))
                                    oLTLogData.AddToCollection("Create a rate request order quote and new Load Tender Record")
                                    Logger.Information("Creating Rate Request with order: {@order}", order)
                                    If oLT.CreateRateRequestOrderQuote(order, iLoadTenderControl, oP44Proxy, oP44Data, DAL.Utilities.SSOAAccount.P44, strMsg, blnUseP44API, oLTLogData) Then
                                        If iLoadTenderControl <> 0 Then
                                            oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
                                            'REM RHR 10/20/2020  If blnUseNGLTariff Then oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl)
                                            ' Modified by RHR for v-8.3.0.002 on 10/20/2020
                                            If blnUseNGLTariff Then
                                                If (blnAPIRatesPossible Or blnUseP44API) And tariffOptions.AllowAsync Then
                                                    ' Modified by RHR for v-8.3.0.002 on 12/17/2020
                                                    oRet.configureForAsyncMessages(iLoadTenderControl)
                                                    'oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl, tariffOptions)
                                                    oDATBLL.CreateNGLTariffBidNoBook(order, iLoadTenderControl, strMsg)
                                                Else
                                                    oDATBLL.CreateNGLTariffBidNoBook(order, iLoadTenderControl, strMsg)
                                                End If
                                            End If
                                            'While reading any tariffs we also can request the rates from P44
                                            'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
                                            If blnUseP44API Then
                                                '    'Modified by RHR for v-8.5.4.006 on 05/24/2024 to use the AllowP44Async property
                                                If blnAPIRatesPossible And tariffOptions.AllowP44Async Then
                                                    oRet.configureForAsyncMessages(iLoadTenderControl)
                                                    '        oLT.ProcessP44RateRequestAsync(oP44Proxy, oP44Data, iLoadTenderControl, strMsg)
                                                Else
                                                    '        blnP44QuotesExist = oLT.ProcessP44RateRequest(oP44Proxy, oP44Data, iLoadTenderControl, strMsg)
                                                    blnP44QuotesExist = False
                                                End If
                                            End If
                                            Logger.Information("Pre oAPIBLL.ProcessAPIRateRequest - TenderType: {tenderType}", tenderTypes(0))
                                            Dim blnAPIQuoted = oAPIBLL.ProcessAPIRateRequest(order, iLoadTenderControl, lAPICarrierConfigs, oRet, strMsg)
                                            Logger.Information("Post oAPIBLL.ProcessAPIRateRequest - blnApiQuoted: {0}, tenderType: {tenderType}, result: {oRet}", blnAPIQuoted, tenderTypes(0), oRet)

                                            If iLoadTenderControl <> 0 Then
                                                oRet.Success = True
                                            Else
                                                oRet.Success = False
                                            End If

                                        End If
                                    Else
                                        'E_ReadQuoteSpFailNoLTControl
                                        Logger.Warning("Read quote failure a load tender record could not be created.")
                                        oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteSpFailNoLTControl")

                                        Return oRet
                                    End If
                                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                                        oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteFailure", New String() {iLoadTenderControl.ToString(), strMsg})

                                        Logger.Warning("Read Quote Failure for LoadTenderControl = {0}: {1}", iLoadTenderControl, strMsg)
                                    End If
                                End Using
                            ElseIf BookControl > 0 Then
                                'we have a book control so process data from the book table
                                'Insert the Booking Info in the the Load Board returns LoadBoardControl in WCFResults
                                'TODO: merge with spot rate insert load board and return LoadTenderControl, CNS number and SHID
                                Using LogContext.PushProperty("BookControl", BookControl)
                                    Logger.Information("Insert Load Board Records for BookControl: {BookControl}, tenderTypes: {tenderType}", BookControl, tenderTypes(0))

                                    oRet = NGLLoadTenderData.InsertLoadBoardRecords(BookControl, "", tenderTypes(0))

                                    If Not oRet.Success Then
                                        oRet.AddLog("Insert booking records into load tender table failed.")
                                        Logger.Warning("Insert booking records into load tender table failed, result: {@oRet}", oRet)
                                        'we have some errors so just return
                                        Return oRet
                                    End If
                                    oRet.AddLog("Create load tender record for rate quote.")
                                    Dim sVals = oRet.TryGetKeys({"LoadTenderControl", "BookConsPrefix", "BookSHID"}, {"0", "Undefined", "Undefined"})
                                    Integer.TryParse(sVals(0), iLoadTenderControl)
                                    Dim sCNS As String = sVals(1)
                                    Dim sSHID As String = sVals(2)

                                    Logger.Information("LoadTenderControl: {LoadTenderControl}, BookConsPrefix: {BookConsPrefix}, BookSHID: {BookSHID}", iLoadTenderControl, sCNS, sSHID)

                                    If iLoadTenderControl = 0 Then
                                        'we cannot continue
                                        oRet.Success = False
                                        'E_SaveRateSpFailNoLTControl
                                        oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateSpFailNoLTControl")
                                        oRet.AddLog("Generate quote failure a load tender record could not be created.")

                                        Logger.Warning("Generate quote failure a load tender record could not be created because iLoadTenderControl =0.")
                                        Return oRet
                                    End If
                                    'Get the Order Number for the Log
                                    Logger.Information("Get Order Number from Load Tender: {0}", iLoadTenderControl)
                                    Dim sOrderNumber As String = NGLLoadTenderData.GetOrderNumberFromLoadTender(iLoadTenderControl)
                                    oRet.updateKeyFields("BookCarrOrderNumber", sOrderNumber)
                                    Logger.Information("BookCarrOrderNumber: {0}", sOrderNumber)
                                    strMsg = ""

                                    If blnUseNGLTariff Then
                                        'oDATBLL.CreateNGLTariffBid(BookControl, iLoadTenderControl, "", strMsg)
                                        'Modified by RHR for v-8.2.1 on 11/12/2019
                                        'Added logic to use prefered temperature and mode.
                                        ' we now pass in filters and call 
                                        Dim ModeType As Integer = 0 'default is zero -- any
                                        Dim TempType As Integer = 0

                                        NGLBookData.GetModeTempTypesByPrecedence(BookControl, ModeType, TempType)

                                        With tariffOptions
                                            .BookControl = BookControl
                                            .modeTypeControl = ModeType
                                            .tempType = TempType
                                        End With

                                        Logger.Information("blnUseNgltariff Create NGL Tariff Bid for BookControl: {BookControl}, iLoadTenderControl: {1}, tariffOptions.TempType: {TempType}, tariffOptions.modeType: {3}", BookControl, iLoadTenderControl, tariffOptions.tempType, tariffOptions.modeTypeControl)

                                        If (blnAPIRatesPossible Or blnUseP44API) And tariffOptions.AllowAsync Then
                                            ' Modified by RHR for v-8.5.4.005 on 03/21/2024
                                            Logger.Information("Either blnAPIRatesPossible ({0}) or blnUseP44API ({1}) is true while tariffOptions.AllowAsync is also true.  So running oDATBLL.CreateNGLTariffBidAsync which is probably causing a thread issue")
                                            oRet.configureForAsyncMessages(iLoadTenderControl)
                                            oDATBLL.CreateNGLTariffBidAsync(BookControl, iLoadTenderControl, "Quote", strMsg, tariffOptions)
                                        Else
                                            Logger.Information("Create NGL Tariff Bid for BookControl: {BookControl}, iLoadTenderControl: {1}, tariffOptions.TempType: {TempType}, tariffOptions.modeType: {3}", BookControl, iLoadTenderControl, tariffOptions.tempType, tariffOptions.modeTypeControl)
                                            oDATBLL.CreateNGLTariffBid(BookControl, iLoadTenderControl, "Quote", strMsg, tariffOptions)
                                        End If
                                    End If
                                    'create the P44 bid 
                                    'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
                                    'If blnUseP44API Then NGLLoadTenderData.CreateP44BidAsync(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                                    ' If blnUseP44API Then
                                    '     'Modified by RHR for v-8.5.4.006 on 05/24/2024 to use the AllowP44Async property
                                    '     If blnAPIRatesPossible And tariffOptions.AllowP44Async Then
                                    '         NGLLoadTenderData.CreateP44BidAsync(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                                    '     Else
                                    '         NGLLoadTenderData.CreateP44Bid(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                                    '     End If
                                    ' End If

                                    Logger.Information("Calling oAPIBLL.ProcessAPIRateRequest for BookControl: {BookControl}, iLoadTenderControl: {LoadTenderControl}, oRet: {@oRet}, strMsg: {strMsg} Order: {@order}", BookControl, iLoadTenderControl, oRet, strMsg, order)
                                    Dim blnAPIQuoted = oAPIBLL.ProcessAPIRateRequest(order, iLoadTenderControl, lAPICarrierConfigs, oRet, strMsg, BookControl)
                                    Logger.Information("Post oAPIBLL.ProcessAPIRateRequest - blnApiQuoted: {0}, results: {@1}", blnAPIQuoted, oRet)
                                    oRet.Success = True

                                    'update the type and status codes (may not be needed but this allows the system to validate and reset type codes 
                                    'and status codes as needed
                                    'The BookData overloads do not require a Book Revenue object, just a book control,  they
                                    'call a stored procedure which updates all the dependent booking reocrds direclty in the db.
                                    'The methods in the NGLDATBLL are used when we have a book revenue object that save to the db later
                                    'used when posting to NEXTStop or DAT UpdateBookRevLTTC and UpdateBookRevLTSCPost respectively
                                    Logger.Information("Update LoadTenderType for {BookControl} to {0}", BookControl, tenderTypes(0))
                                    oBookDAL.updateBookRevLoadTenderTypeControl(BookControl, tenderTypes(0))
                                    Logger.Information("Update LoadTenderStatusCode for {BookControl} to {0}", BookControl, tenderTypes(0))
                                    oBookDAL.updateBookRevLoadTenderStatusCodePost(BookControl, tenderTypes(0))
                                End Using

                            End If
                        End Using
                    Case DTO.tblLoadTender.LoadTenderTypeEnum.RateShopping
                        'TODO: determine if we need to raise an error or show a message to the user 
                        'that this option is not allowed?  This should not happen in production.
                        Using LogContext.PushProperty("TenderType", "RateShop")
                            If Not (blnAPIRatesPossible Or blnUseP44API Or blnUseNGLTariff) Then
                                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "Cannot generate quotes an API or Tariff quote Type is required")
                                Return oRet
                            End If
                            Logger.Information("Generate quotes from DTO.tblLoadTender.LoadTenderTypeEnum.RateShopping")
                            If Not order Is Nothing Then
                                'this is a rate shopping order with no booking record
                                Using LogContext.PushProperty("Order", order)
                                    Logger.Information("Create a rate request order quote and new Load Tender Record for {iLoadTenderControl} with oP44Data: {oP44Data}", iLoadTenderControl, oP44Data)

                                    If oLT.CreateRateRequestOrderQuote(order, iLoadTenderControl, oP44Proxy, oP44Data, DAL.Utilities.SSOAAccount.P44, strMsg, blnUseP44API, oLTLogData) Then
                                        Logger.Information("Create Rate Request Order Quote - 2532")
                                        If iLoadTenderControl <> 0 Then
                                            oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
                                            'REM RHR 10/20/2020 If blnUseNGLTariff Then oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl)
                                            ' Modified by RHR for v-8.3.0.002 on 10/20/2020
                                            If blnUseNGLTariff Then
                                                If (blnAPIRatesPossible Or blnUseP44API) And tariffOptions.AllowAsync Then
                                                    ' Modified by RHR for v-8.3.0.002 on 12/17/2020
                                                    Logger.Information("Create NGL Tariff Bid No Book Async - 2542")
                                                    oRet.configureForAsyncMessages(iLoadTenderControl)
                                                    oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl, tariffOptions)
                                                Else
                                                    Logger.Information("Create NGL Tariff Bid No Book - 2547")
                                                    oDATBLL.CreateNGLTariffBidNoBook(order, iLoadTenderControl, strMsg)
                                                End If
                                            End If
                                            'While reading any tariffs we also can request the rates from P44
                                            'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
                                            If blnUseP44API Then
                                                'Try
                                                'Modified by RHR for v-8.5.4.006 on 05/24/2024 to use the AllowP44Async property
                                                Logger.Information("Create P44 Bid Async - 2558")
                                                If blnAPIRatesPossible And tariffOptions.AllowP44Async Then
                                                    Logger.Information("Configure {iLoadTenderControl} for AsyncMessages...", iLoadTenderControl)
                                                    oRet.configureForAsyncMessages(iLoadTenderControl)
                                                    '    oLT.ProcessP44RateRequestAsync(oP44Proxy, oP44Data, iLoadTenderControl, strMsg)
                                                Else
                                                    '      blnP44QuotesExist = oLT.ProcessP44RateRequest(oP44Proxy, oP44Data, iLoadTenderControl, strMsg)
                                                    Logger.Information("Process P44 Rate Request - 2567")
                                                    blnP44QuotesExist = False
                                                End If
                                                'Catch ex As Exception
                                                '    Dim sMsgs = {iLoadTenderControl.ToString(), ex.Message}
                                                '    oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_ReadQuoteFailure, sMsgs)

                                                'End Try
                                            End If

                                            Logger.Information("Pre oAPIBLL.ProcessAPIRateRequest - 2576")
                                            Dim blnAPIQuoted = oAPIBLL.ProcessAPIRateRequest(order, iLoadTenderControl, lAPICarrierConfigs, oRet, strMsg)
                                            Logger.Information("Post oAPIBLL.ProcessAPIRateRequest - 2578, blnApiQuoted: {0}", blnAPIQuoted)
                                            'If blnUseCHRAPI Then
                                            '    Dim blnQuotesExist = oLT.ProcessCHRRateRequest(order, iLoadTenderControl, oCHRLEConfig, lCHRCompConfig, strMsg)
                                            'End If
                                            'If blnUseUPSAPI Then
                                            '    Dim blnQuotesExist = oLT.ProcessUPSRateRequest(order, iLoadTenderControl, oUPSLEConfig, lUPSCompConfig, strMsg)
                                            'End If
                                            'If blnUseJTSAPI Then
                                            '    Dim blnQuotesExist = oLT.ProcessJTSRateRequest(order, iLoadTenderControl, oJTSLEConfig, lJTSCompConfig, strMsg)
                                            'End If

                                            If iLoadTenderControl <> 0 Then
                                                oRet.Success = True
                                            Else
                                                oRet.Success = False
                                            End If
                                        End If
                                    Else
                                        'E_ReadQuoteSpFailNoLTControl
                                        oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteSpFailNoLTControl")
                                        SaveAppError("Read quote failure a load tender record could not be created.")

                                        Return oRet
                                    End If
                                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                                        Logger.Warning("Read Quote Failure for LoadTenderControl = {0}: {1}", iLoadTenderControl, strMsg)
                                        oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteFailure", New String() {iLoadTenderControl.ToString(), strMsg})
                                        SaveAppError(String.Concat("Read Quote Failure for LoadTenderControl = ", iLoadTenderControl.ToString(), ": ", strMsg))
                                    End If
                                End Using
                            ElseIf BookControl > 0 Then
                                'we have a book control so process data from the book table
                                'Insert the Booking Info in the the Load Board returns LoadBoardControl in WCFResults
                                'TODO: merge with spot rate insert load board and return LoadTenderControl, CNS number and SHID
                                Using LogContext.PushProperty("BookControl", BookControl)
                                    Logger.Information("Pre - Insert Load Board Records for BookControl: {BookControl}, tenderTypes: {tenderType}", BookControl, tenderTypes(0))
                                    oRet = NGLLoadTenderData.InsertLoadBoardRecords(BookControl, "", tenderTypes(0))
                                    Logger.Information("Post Insert Load Board Records for BookControl: {BookControl}, tenderTypes: {tendeyType}, oRet: {oRet}", BookControl, tenderTypes(0), oRet)
                                    If Not oRet.Success Then
                                        'we have some errors so just return 
                                        Logger.Warning("Insert booking records into load tender table failed for BookControl {BookControl} - {tenderType}, result: {@oRet}", BookControl, tenderTypes(0), oRet)
                                        Return oRet
                                    End If
                                    Dim sVals = oRet.TryGetKeys({"LoadTenderControl", "BookConsPrefix", "BookSHID"}, {"0", "Undefined", "Undefined"})
                                    Integer.TryParse(sVals(0), iLoadTenderControl)
                                    Dim sCNS As String = sVals(1)
                                    Dim sSHID As String = sVals(2)

                                    If iLoadTenderControl = 0 Then
                                        'we cannot continue
                                        oRet.Success = False

                                        'E_SaveRateSpFailNoLTControl
                                        oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateSpFailNoLTControl")
                                        Logger.Warning("Save rate failure a load tender record could not be created.  BookControl {BookControl}", BookControl)
                                        SaveAppError("Save rate failure a load tender record could not be created.")
                                        Return oRet
                                    End If
                                    If Not String.IsNullOrWhiteSpace(strMsg) Then
                                        oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateFailure", New String() {iLoadTenderControl.ToString(), strMsg})
                                        SaveAppError(String.Concat("Save Rate Failure for LoadTenderControl = ", iLoadTenderControl.ToString(), ": ", strMsg))
                                    End If
                                    strMsg = ""
                                    If blnUseNGLTariff Then
                                        Dim ModeType As Integer = 0 'default is zero -- any
                                        Dim TempType As Integer = 0
                                        NGLBookData.GetModeTempTypesByPrecedence(BookControl, ModeType, TempType)
                                        With tariffOptions
                                            .BookControl = BookControl
                                            .modeTypeControl = ModeType
                                            .tempType = TempType
                                        End With
                                        Logger.Information("Create NGL Tariff Bid for BookControl: {BookControl}, iLoadTenderControl: {LoadTenderControl}, tariffOptions - ModeType: {ModeType}, TempType:{TempType}, LaneControl: {LaneControl}, TariffType: {TariffTypeControl}", BookControl, iLoadTenderControl, tariffOptions.modeTypeControl, tariffOptions.tempType, tariffOptions.LaneControl, tariffOptions.tariffTypeControl)
                                        'Modified by RHR for v-8.5.4.006 on 05/24/2024 to use the AllowP44Async property
                                        If (blnAPIRatesPossible Or blnUseP44API) And tariffOptions.AllowAsync Then
                                            ' Modified by RHR for v-8.5.4.005 on 03/21/2024
                                            Logger.Information("Create NGL Tariff Bid Async - 2620")
                                            oRet.configureForAsyncMessages(iLoadTenderControl)
                                            oDATBLL.CreateNGLTariffBidAsync(BookControl, iLoadTenderControl, "Quote", strMsg, tariffOptions)
                                        Else
                                            Logger.Information("Create NGL Tariff Bid - 2625")
                                            oDATBLL.CreateNGLTariffBid(BookControl, iLoadTenderControl, "Quote", strMsg, tariffOptions)
                                        End If
                                    End If
                                    'create the P44 bid 
                                    'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
                                    ' Modified by RHR for v-8.3.0.002 on 12/17/2020
                                    'Modified by RHR for v-8.5.4.006 on 05/24/2024 to use the AllowP44Async property
                                    If blnUseP44API Then
                                        If blnAPIRatesPossible And tariffOptions.AllowP44Async Then
                                            oRet.configureForAsyncMessages(iLoadTenderControl)
                                            '     NGLLoadTenderData.CreateP44BidAsync(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                                        Else
                                            '   NGLLoadTenderData.CreateP44Bid(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                                        End If
                                    End If
                                    Logger.Information("Pre oAPIBLL.ProcessAPIRateRequest - 2644")
                                    Dim blnAPIQuoted = oAPIBLL.ProcessAPIRateRequest(order, iLoadTenderControl, lAPICarrierConfigs, oRet, strMsg, BookControl)
                                    Logger.Information("Post oAPIBLL.ProcessAPIRateRequest - 2646, blnApiQuoted: {0}", blnAPIQuoted)
                                    'If blnUseJTSAPI Then
                                    '            Dim blnQuotesExist = oLT.CreateJTSBid(BookControl, iLoadTenderControl, "", oJTSLEConfig, lJTSCompConfig, DAL.Utilities.SSOAAccount.JTSAPI, strMsg)
                                    '        End If

                                    oRet.Success = True
                                    'update the type and status codes (may not be needed but this allows the system to validate and reset type codes 
                                    'and status codes as needed
                                    'The BookData overloads do not require a Book Revenue object, just a book control,  they
                                    'call a stored procedure which updates all the dependent booking reocrds direclty in the db.
                                    'The methods in the NGLDATBLL are used when we have a book revenue object that save to the db later
                                    'used when posting to NEXTStop or DAT UpdateBookRevLTTC and UpdateBookRevLTSCPost respectively
                                    Logger.Information("Update LoadTenderType for {BookControl} to {TenderType}", BookControl, tenderTypes(0))
                                    oBookDAL.updateBookRevLoadTenderTypeControl(BookControl, tenderTypes(0))
                                    Logger.Information("Update LoadTenderStatusCode for {BookControl} to {TenderType}", BookControl, tenderTypes(0))
                                    oBookDAL.updateBookRevLoadTenderStatusCodePost(BookControl, tenderTypes(0))
                                End Using

                            End If
                        End Using

                    Case DTO.tblLoadTender.LoadTenderTypeEnum.SpotRate
                        Using LogContext.PushProperty("TenderType", "SpotRate")
                            If BookControl > 0 Then
                                'we have a book control so process data from the book table
                                'Insert the Booking Info in the the Load Board returns LoadBoardControl in WCFResults
                                'Note: CreateLoadTenderFromBook should be replaces with NGLLoadTenderData.InsertLoadBoardRecords 
                                '   as soon as possible so we have one process flow.  
                                '   first we need to merge difference in spCreateLoadTenderFromBook with spInsertLoadBoardRecords
                                Logger.Information("Insert Load Board Records for BookControl: {0}, tenderTypes: {1}", BookControl, tenderTypes(0))
                                Dim DalSpotRate As DAL.NGLBookSpotRateData = New DAL.NGLBookSpotRateData(Me.Parameters)
                                iLoadTenderControl = DalSpotRate.CreateLoadTenderFromBook(BookControl)
                                If iLoadTenderControl = 0 Then
                                    'we have some errors so just return 
                                    Return oRet
                                End If
                                oRet.Success = True
                                oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
                                oRet.updateKeyFields("BookControl", BookControl.ToString())
                                If Not InsertLoadTenderSpotRate(oRet) Then
                                    Return oRet
                                End If
                                'update the type and status codes (may not be needed but this allows the system to validate and reset type codes 
                                'and status codes as needed
                                'The BookData overloads do not require a Book Revenue object, just a book control,  they
                                'call a stored procedure which updates all the dependent booking reocrds direclty in the db.
                                'The methods in the NGLDATBLL are used when we have a book revenue object that save to the db later
                                'used when posting to NEXTStop or DAT UpdateBookRevLTTC and UpdateBookRevLTSCPost respectively
                                oBookDAL.updateBookRevLoadTenderTypeControl(BookControl, tenderTypes(0))
                                oBookDAL.updateBookRevLoadTenderStatusCodePost(BookControl, tenderTypes(0))
                            Else
                                'TODO: add exception message here for no book?
                            End If
                        End Using

                    Case DTO.tblLoadTender.LoadTenderTypeEnum.NextStop

                        Dim source = 0
                        Dim bwLB As New NGL.Core.Utility.BitwiseFlags32(source)
                        'turn on the flag for NEXTStop
                        bwLB.turnBitFlagOn(DTO.tblLoadTender.LoadTenderTypeEnum.NextStop)
                        If tenderTypes.Count > 1 AndAlso tenderTypes(1) = DTO.tblLoadTender.LoadTenderTypeEnum.DAT Then
                            bwLB.turnBitFlagOn(DTO.tblLoadTender.LoadTenderTypeEnum.DAT)
                        End If
                        'get the booking revenue data
                        Dim oBookRevs = oBookRev.GetBookRevenuesWDetailsFiltered(BookControl)
                        oRet = oDATBLL.LoadBoardPostMethod(oBookRevs, BookControl, bwLB)
                    Case DTO.tblLoadTender.LoadTenderTypeEnum.DAT
                        Dim source = 0
                        Dim bwLB As New NGL.Core.Utility.BitwiseFlags32(source)
                        'turn on the flag for NEXTStop
                        bwLB.turnBitFlagOn(DTO.tblLoadTender.LoadTenderTypeEnum.DAT)
                        If tenderTypes.Count > 1 AndAlso tenderTypes(1) = DTO.tblLoadTender.LoadTenderTypeEnum.NextStop Then
                            bwLB.turnBitFlagOn(DTO.tblLoadTender.LoadTenderTypeEnum.NextStop)
                        End If
                        'get the booking revenue data
                        Dim oBookRevs = oBookRev.GetBookRevenuesWDetailsFiltered(BookControl)

                        oRet = oDATBLL.LoadBoardPostMethod(oBookRevs, BookControl, bwLB)

                    Case Else
                        'Invalid Parameter: No record exists in the database for {0}: {1}.
                        oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidParameterNameValue", New String() {"Load Tender Bid Type", tenderTypes(0).ToString()})
                        SaveAppError(String.Concat("Invalid Parameter: No record exists in the database for  Load Tender Bid Type ", tenderTypes(0).ToString()))
                        Return oRet
                End Select


            Catch ex As FaultException
                Logger.Error(ex, "Fault Exception in Inner_GenerateQuote")
                Throw
            Catch ex As System.ApplicationException
                Log.Error(ex, "Application Exception in Inner_GenerateQuote")
                oLTLogData.AddToCollection("Application Error: " & ex.Message)
                throwInvalidOperatonException(ex.Message, True, False)
            Catch ex As Exception
                Logger.Error(ex, "Unexpected Error in Inner_GenerateQuote")
                oLTLogData.AddToCollection("Unexpected Error: " & ex.Message)
                throwUnExpectedFaultException(ex, buildProcedureName("GenerateQuote"), FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
            Finally
                Logger.Information("Save Load Tender Log Data")
                If Not oLTLogData Is Nothing AndAlso oLTLogData.Logs.Count() > 0 AndAlso iLoadTenderControl <> 0 Then
                    oLTLogData.SaveCollectionToDB(iLoadTenderControl, Me.Parameters.UserName)
                    Logger.Information("SaveCollectionToDB - 2715")
                End If
            End Try
            If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
                'if we do not have any meesages set one to  Invalid Request 
                oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest")
            End If
        End Using

        Return oRet
    End Function

    Public Function CreateNewLoadTender(ByVal order As DAL.Models.RateRequestOrder, ByVal tenderTypes() As DTO.tblLoadTender.LoadTenderTypeEnum, bidTypes() As DTO.tblLoadTender.BidTypeEnum, Optional ByVal BookControl As Integer = 0, Optional ByVal tariffOptions As DTO.GetCarriersByCostParameters = Nothing) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        Dim iLoadTenderControl As Integer = 0
        Dim strMsg As String = ""
        Dim oLTLogData As New DAL.NGLLoadTenderLogData(Me.Parameters)
        oRet.Success = False
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())

        Logger.Warning("Visual Studio says this has zero references...  So technically it shoudn't be called...")

        Logger.Information("CreateNewLoadTender\nOrder: {0}\nTenderTypes: {1}\nBidTypes: {2}\nBookControl: {3}\nTariffOptions: {4}", order, tenderTypes, bidTypes, BookControl, tariffOptions)
        'TODO: determine if we need to show a message to the user,  these options should not happen in production.
        If tenderTypes Is Nothing OrElse tenderTypes.Count < 1 Then Return oRet
        If bidTypes Is Nothing OrElse bidTypes.Count < 1 Then Return oRet
        If tariffOptions Is Nothing Then tariffOptions = New DTO.GetCarriersByCostParameters(False, True, True, 0, 0) 'use default for standard routing prefered = false, noLateDelivery = False, validated = True, optimizeByCapacity = True --  modeTypeControl and  tempType  are looked up later
        Try
            Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)
            Dim oDATBLL As BLL.NGLDATBLL = New BLL.NGLDATBLL(Parameters)
            Dim oBookDAL As New DAL.NGLBookData(Parameters)
            Dim oBookRev As New DAL.NGLBookRevenueData(Parameters)
            Dim oItems As New List(Of DAL.Models.RateRequestItem)
            'the roles may be reversed but the results should be the same.
            Dim oP44Proxy As P44.P44Proxy
            Dim oP44Data As P44.RateRequest
            Dim blnUseP44API As Boolean = False
            Dim blnUseNGLTariff As Boolean = False
            Dim blnUseCHRAPI As Boolean = False
            Dim blnUseUPSAPI As Boolean = False
            Dim blnUseJTSAPI As Boolean = False

            Dim oCHRLEConfig As New LTS.tblSSOALEConfig()
            Dim lCHRCompConfig As New List(Of LTS.tblSSOAConfig)

            Dim oJTSLEConfig As New LTS.tblSSOALEConfig()
            Dim lJTSCompConfig As New List(Of LTS.tblSSOAConfig)

            Dim oUPSLEConfig As New LTS.tblSSOALEConfig()
            Dim lUPSCompConfig As New List(Of LTS.tblSSOAConfig)
            Dim iCompControl As Integer = 0
            oLTLogData.AddToCollection("Read quote and bid types")
            For Each bt In bidTypes
                If bt = DTO.tblLoadTender.BidTypeEnum.P44 Then
                    'Modified by RHR for v-8.5.4.1 on 06/30/2023 new rate shopping temperature logic
                    If (order.TariffTempType <= 1) Then '0 (any) and 1 (dry) are ok for p44
                        blnUseP44API = True
                    End If
                    'Modified by RHR for v-8.5.3.005 on 08/24/2022 added logic to use bookcontrol to get legal entity
                    ' get the SSOA settings if the bookcontrol does not exist GetSSOAConfig will use the users legal entity
                    blnUseCHRAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.CHRAPI, oCHRLEConfig, lCHRCompConfig, BookControl)
                    blnUseJTSAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.JTSAPI, oJTSLEConfig, lJTSCompConfig, BookControl)
                    blnUseUPSAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.UPSAPI, oUPSLEConfig, lUPSCompConfig, BookControl)
                ElseIf bt = DTO.tblLoadTender.BidTypeEnum.NGLTariff Then
                    blnUseNGLTariff = True
                    'ElseIf bt = DTO.tblLoadTender.BidTypeEnum.d Then
                End If
            Next

            Select Case tenderTypes(0)
                Case DTO.tblLoadTender.LoadTenderTypeEnum.LoadBoard
                    oLTLogData.AddToCollection("Generate quotes from Load Board")

                    'TODO: determine if we need to raise an error or show a message to the user 
                    'that this option is not allowed?  This should not happen in production.
                    If Not (blnUseP44API Or blnUseNGLTariff) Then
                        oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "Cannot generate quotes an API or Tariff quote Type is required")
                        Return oRet
                    End If

                    If Not order Is Nothing Then
                        Logger.Warning("Can this situation even happen where order isn't null on a LoadBoard?")
                        oLTLogData.AddToCollection("Create a rate request order quote and new Load Tender Record")
                        If oLT.CreateRateRequestOrderQuote(order, iLoadTenderControl, oP44Proxy, oP44Data, DAL.Utilities.SSOAAccount.P44, strMsg, blnUseP44API, oLTLogData) Then
                            If iLoadTenderControl <> 0 Then
                                oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
                                'REM RHR 10/20/2020  If blnUseNGLTariff Then oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl)
                                ' Modified by RHR for v-8.3.0.002 on 10/20/2020
                                If blnUseNGLTariff Then
                                    If blnUseP44API And tariffOptions.AllowAsync Then
                                        ' Modified by RHR for v-8.3.0.002 on 12/17/2020
                                        oRet.configureForAsyncMessages(iLoadTenderControl)
                                        oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl, tariffOptions)
                                    Else
                                        oDATBLL.CreateNGLTariffBidNoBook(order, iLoadTenderControl, strMsg)
                                    End If
                                End If
                                'While reading any tariffs we also can request the rates from P44
                                'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
                                If blnUseP44API Then
                                    Dim blnQuotesExist = oLT.ProcessP44RateRequest(oP44Proxy, oP44Data, iLoadTenderControl, strMsg)
                                End If
                                If blnUseCHRAPI Then
                                    Dim blnQuotesExist = oLT.ProcessCHRRateRequest(order, iLoadTenderControl, oCHRLEConfig, lCHRCompConfig, strMsg)
                                End If

                                If blnUseUPSAPI Then
                                    Dim blnQuotesExist = oLT.ProcessUPSRateRequest(order, iLoadTenderControl, oUPSLEConfig, lUPSCompConfig, strMsg)
                                End If

                                If blnUseJTSAPI Then
                                    Dim blnQuotesExist = oLT.ProcessJTSRateRequest(order, iLoadTenderControl, oJTSLEConfig, lJTSCompConfig, strMsg)
                                End If

                                If iLoadTenderControl <> 0 Then
                                    oRet.Success = True
                                Else
                                    oRet.Success = False
                                End If

                            End If
                        Else
                            'E_ReadQuoteSpFailNoLTControl
                            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteSpFailNoLTControl")
                            SaveAppError("Read quote failure a load tender record could not be created.")
                            Return oRet
                        End If
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteFailure", New String() {iLoadTenderControl.ToString(), strMsg})
                            SaveAppError(String.Concat("Read Quote Failure for LoadTenderControl = ", iLoadTenderControl.ToString(), ": ", strMsg))
                        End If
                    ElseIf BookControl > 0 Then
                        'we have a book control so process data from the book table
                        'Insert the Booking Info in the the Load Board returns LoadBoardControl in WCFResults
                        'TODO: merge with spot rate insert load board and return LoadTenderControl, CNS number and SHID
                        oRet = NGLLoadTenderData.InsertLoadBoardRecords(BookControl, "", tenderTypes(0))

                        If Not oRet.Success Then
                            oRet.AddLog("Insert booking records into load tender table failed.")
                            'we have some errors so just return
                            Return oRet
                        End If
                        oRet.AddLog("Create load tender record for rate quote.")
                        Dim sVals = oRet.TryGetKeys({"LoadTenderControl", "BookConsPrefix", "BookSHID"}, {"0", "Undefined", "Undefined"})
                        Integer.TryParse(sVals(0), iLoadTenderControl)
                        Dim sCNS As String = sVals(1)
                        Dim sSHID As String = sVals(2)

                        If iLoadTenderControl = 0 Then
                            'we cannot continue
                            oRet.Success = False
                            'E_SaveRateSpFailNoLTControl
                            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateSpFailNoLTControl")
                            oRet.AddLog("Generate quote failure a load tender record could not be created.")
                            SaveAppError("Generate quote failure a load tender record could not be created.")
                            Return oRet
                        End If
                        'Get the Order Number for the Log
                        Dim sOrderNumber As String = NGLLoadTenderData.GetOrderNumberFromLoadTender(iLoadTenderControl)
                        oRet.updateKeyFields("BookCarrOrderNumber", sOrderNumber)
                        strMsg = ""
                        'create the P44 bid 
                        'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
                        'If blnUseP44API Then NGLLoadTenderData.CreateP44BidAsync(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                        If blnUseP44API Then NGLLoadTenderData.CreateP44Bid(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                        If blnUseCHRAPI Then
                            Dim blnQuotesExist = oLT.CreateCHRBid(BookControl, iLoadTenderControl, "", oCHRLEConfig, lCHRCompConfig, DAL.Utilities.SSOAAccount.CHRAPI, strMsg)
                        End If
                        If blnUseUPSAPI Then
                            Dim blnQuotesExist = oLT.CreateUPSBid(BookControl, iLoadTenderControl, "", oUPSLEConfig, lUPSCompConfig, DAL.Utilities.SSOAAccount.UPSAPI, strMsg)
                        End If
                        If blnUseJTSAPI Then
                            Dim blnQuotesExist = oLT.CreateJTSBid(BookControl, iLoadTenderControl, "", oJTSLEConfig, lJTSCompConfig, DAL.Utilities.SSOAAccount.JTSAPI, strMsg)
                        End If
                        If blnUseNGLTariff Then
                            'oDATBLL.CreateNGLTariffBid(BookControl, iLoadTenderControl, "", strMsg)
                            'Modified by RHR for v-8.2.1 on 11/12/2019
                            'Added logic to use prefered temperature and mode.
                            ' we now pass in filters and call 
                            Dim ModeType As Integer = 0 'default is zero -- any
                            Dim TempType As Integer = 0
                            NGLBookData.GetModeTempTypesByPrecedence(BookControl, ModeType, TempType)
                            With tariffOptions
                                .BookControl = BookControl
                                .modeTypeControl = ModeType
                                .tempType = TempType
                            End With
                            Dim res = getEstimatedCarriersByCostFiltered(tariffOptions)
                            If Not oRet.updateMessagesAndLogsWithCarrierCostResults(res, sOrderNumber) Then
                                If Not blnUseP44API Then
                                    'no need to continue 
                                    oRet.Success = False
                                    Return oRet
                                End If
                            End If

                            If res.CarriersByCost Is Nothing OrElse res.CarriersByCost.Count < 1 Then
                                If Not blnUseP44API Then
                                    'no need to continue 
                                    oRet.Success = False
                                    Return oRet
                                End If
                            Else
                                Dim oBid As New DAL.NGLBidData(Parameters)
                                'Modified by RHR for v-8.5.3.001 on 05/25/2022 we now use InsertNGLTariffBid365
                                'we have the rates in res, res has the messages and logs,
                                'add any res.CarriersByCost Messages for each carrier
                                'to the bid errors  tblBidSvcErrs like we do for the API carriers.
                                oBid.InsertNGLTariffBid365(res, res.BookRevs, iLoadTenderControl, sSHID, strMsg)
                            End If
                        End If
                        oRet.Success = True
                        'update the type and status codes (may not be needed but this allows the system to validate and reset type codes 
                        'and status codes as needed
                        'The BookData overloads do not require a Book Revenue object, just a book control,  they
                        'call a stored procedure which updates all the dependent booking reocrds direclty in the db.
                        'The methods in the NGLDATBLL are used when we have a book revenue object that save to the db later
                        'used when posting to NEXTStop or DAT UpdateBookRevLTTC and UpdateBookRevLTSCPost respectively
                        oBookDAL.updateBookRevLoadTenderTypeControl(BookControl, tenderTypes(0))
                        oBookDAL.updateBookRevLoadTenderStatusCodePost(BookControl, tenderTypes(0))
                    End If
                Case DTO.tblLoadTender.LoadTenderTypeEnum.RateShopping
                    'TODO: determine if we need to raise an error or show a message to the user 
                    'that this option is not allowed?  This should not happen in production.
                    Logger.Warning("Apparently this isn't supposed to happen for RateShopping")
                    If Not (blnUseP44API Or blnUseNGLTariff) Then
                        oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "Cannot generate quotes an API or Tariff quote Type is required")
                        Return oRet
                    End If

                    If Not order Is Nothing Then
                        'this is a rate shopping order with no booking record
                        If oLT.CreateRateRequestOrderQuote(order, iLoadTenderControl, oP44Proxy, oP44Data, DAL.Utilities.SSOAAccount.P44, strMsg, blnUseP44API, oLTLogData) Then
                            If iLoadTenderControl <> 0 Then
                                oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
                                'REM RHR 10/20/2020 If blnUseNGLTariff Then oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl)
                                ' Modified by RHR for v-8.3.0.002 on 10/20/2020
                                If blnUseNGLTariff Then
                                    If blnUseP44API Then
                                        ' Modified by RHR for v-8.3.0.002 on 12/17/2020
                                        oRet.configureForAsyncMessages(iLoadTenderControl)
                                        oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl, tariffOptions)
                                    Else
                                        oDATBLL.CreateNGLTariffBidNoBook(order, iLoadTenderControl, strMsg)
                                    End If
                                End If
                                'While reading any tariffs we also can request the rates from P44
                                'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
                                If blnUseP44API Then
                                    'Try
                                    Dim blnQuotesExist = oLT.ProcessP44RateRequest(oP44Proxy, oP44Data, iLoadTenderControl, strMsg)
                                    'Catch ex As Exception
                                    '    Dim sMsgs = {iLoadTenderControl.ToString(), ex.Message}
                                    '    oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_ReadQuoteFailure, sMsgs)

                                    'End Try
                                End If
                                If blnUseCHRAPI Then
                                    Dim blnQuotesExist = oLT.ProcessCHRRateRequest(order, iLoadTenderControl, oCHRLEConfig, lCHRCompConfig, strMsg)
                                End If
                                If blnUseUPSAPI Then
                                    Dim blnQuotesExist = oLT.ProcessUPSRateRequest(order, iLoadTenderControl, oUPSLEConfig, lUPSCompConfig, strMsg)
                                End If
                                If blnUseJTSAPI Then
                                    Dim blnQuotesExist = oLT.ProcessJTSRateRequest(order, iLoadTenderControl, oJTSLEConfig, lJTSCompConfig, strMsg)
                                End If

                                If iLoadTenderControl <> 0 Then
                                    oRet.Success = True
                                Else
                                    oRet.Success = False
                                End If
                            End If
                        Else
                            'E_ReadQuoteSpFailNoLTControl
                            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteSpFailNoLTControl")
                            SaveAppError("Read quote failure a load tender record could not be created.")
                            Return oRet
                        End If
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteFailure", New String() {iLoadTenderControl.ToString(), strMsg})
                            SaveAppError(String.Concat("Read Quote Failure for LoadTenderControl = ", iLoadTenderControl.ToString(), ": ", strMsg))
                        End If
                    ElseIf BookControl > 0 Then
                        'we have a book control so process data from the book table
                        'Insert the Booking Info in the the Load Board returns LoadBoardControl in WCFResults
                        Logger.Warning("Also hoping this didn't happen...")
                        'TODO: merge with spot rate insert load board and return LoadTenderControl, CNS number and SHID
                        oRet = NGLLoadTenderData.InsertLoadBoardRecords(BookControl, "", tenderTypes(0))
                        If Not oRet.Success Then
                            'we have some errors so just return 
                            Return oRet
                        End If
                        Dim sVals = oRet.TryGetKeys({"LoadTenderControl", "BookConsPrefix", "BookSHID"}, {"0", "Undefined", "Undefined"})
                        Integer.TryParse(sVals(0), iLoadTenderControl)
                        Dim sCNS As String = sVals(1)
                        Dim sSHID As String = sVals(2)

                        If iLoadTenderControl = 0 Then
                            'we cannot continue
                            oRet.Success = False
                            'E_SaveRateSpFailNoLTControl
                            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateSpFailNoLTControl")
                            SaveAppError("Save rate failure a load tender record could not be created.")
                            Return oRet
                        End If
                        If Not String.IsNullOrWhiteSpace(strMsg) Then
                            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateFailure", New String() {iLoadTenderControl.ToString(), strMsg})
                            SaveAppError(String.Concat("Save Rate Failure for LoadTenderControl = ", iLoadTenderControl.ToString(), ": ", strMsg))
                        End If
                        strMsg = ""
                        'create the P44 bid 
                        'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
                        ' Modified by RHR for v-8.3.0.002 on 12/17/2020
                        oRet.configureForAsyncMessages(iLoadTenderControl)
                        If blnUseP44API Then NGLLoadTenderData.CreateP44BidAsync(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
                        If blnUseCHRAPI Then
                            Dim blnQuotesExist = oLT.CreateCHRBid(BookControl, iLoadTenderControl, "", oCHRLEConfig, lCHRCompConfig, DAL.Utilities.SSOAAccount.CHRAPI, strMsg)
                        End If
                        If blnUseUPSAPI Then
                            Dim blnQuotesExist = oLT.CreateUPSBid(BookControl, iLoadTenderControl, "", oUPSLEConfig, lUPSCompConfig, DAL.Utilities.SSOAAccount.UPSAPI, strMsg)
                        End If
                        If blnUseJTSAPI Then
                            Dim blnQuotesExist = oLT.CreateJTSBid(BookControl, iLoadTenderControl, "", oJTSLEConfig, lJTSCompConfig, DAL.Utilities.SSOAAccount.JTSAPI, strMsg)
                        End If
                        If blnUseNGLTariff Then
                            'oDATBLL.CreateNGLTariffBid(BookControl, iLoadTenderControl, "", strMsg)
                            'Modified by RHR for v-8.2.1 on 11/12/2019
                            'Added logic to use prefered temperature and mode.
                            ' we now pass in filters and call 
                            Dim ModeType As Integer = 0 'default is zero -- any
                            Dim TempType As Integer = 0
                            NGLBookData.GetModeTempTypesByPrecedence(BookControl, ModeType, TempType)
                            With tariffOptions
                                .BookControl = BookControl
                                .modeTypeControl = ModeType
                                .tempType = TempType
                            End With
                            Dim res = getEstimatedCarriersByCostFiltered(tariffOptions)
                            'Removed by RHR for v-8.2.1 on 11/12/2019
                            'Dim res = getEstimatedCarriersByCostAll(BookControl, 1, 1000)
                            If res Is Nothing Then
                                'E_NoTariffAvailable -- A tariff is not available for the select route CNS: {0}.
                                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_NoTariffAvailable", {sCNS})
                                SaveAppError(String.Format("A tariff is not available for the select route CNS: {0}", sCNS))
                                If Not blnUseP44API Then
                                    'no need to continue 
                                    oRet.Success = False
                                    Return oRet
                                End If
                            End If
                            If Not res.Log Is Nothing AndAlso res.Log.Count > 0 Then oRet.Log.AddRange(res.Log)
                            If Not res.Messages Is Nothing AndAlso res.Messages.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Messages, res.Messages)
                            If res.CarriersByCost Is Nothing OrElse res.CarriersByCost.Count < 1 Then
                                If Not blnUseP44API Then
                                    'no need to continue 
                                    oRet.Success = False
                                    Return oRet
                                End If
                            Else
                                Dim oBid As New DAL.NGLBidData(Parameters)
                                'Modified by RHR for v-8.5.3.001 on 05/25/2022 we now use InsertNGLTariffBid365
                                oBid.InsertNGLTariffBid365(res, res.BookRevs, iLoadTenderControl, sSHID, strMsg)
                            End If
                        End If
                        oRet.Success = True
                        'update the type and status codes (may not be needed but this allows the system to validate and reset type codes 
                        'and status codes as needed
                        'The BookData overloads do not require a Book Revenue object, just a book control,  they
                        'call a stored procedure which updates all the dependent booking reocrds direclty in the db.
                        'The methods in the NGLDATBLL are used when we have a book revenue object that save to the db later
                        'used when posting to NEXTStop or DAT UpdateBookRevLTTC and UpdateBookRevLTSCPost respectively
                        oBookDAL.updateBookRevLoadTenderTypeControl(BookControl, tenderTypes(0))
                        oBookDAL.updateBookRevLoadTenderStatusCodePost(BookControl, tenderTypes(0))
                    End If
                Case DTO.tblLoadTender.LoadTenderTypeEnum.SpotRate
                    If BookControl > 0 Then
                        'we have a book control so process data from the book table
                        'Insert the Booking Info in the the Load Board returns LoadBoardControl in WCFResults
                        'Note: CreateLoadTenderFromBook should be replaces with NGLLoadTenderData.InsertLoadBoardRecords 
                        '   as soon as possible so we have one process flow.  
                        '   first we need to merge difference in spCreateLoadTenderFromBook with spInsertLoadBoardRecords
                        Dim DalSpotRate As DAL.NGLBookSpotRateData = New DAL.NGLBookSpotRateData(Me.Parameters)
                        iLoadTenderControl = DalSpotRate.CreateLoadTenderFromBook(BookControl)
                        If iLoadTenderControl = 0 Then
                            'we have some errors so just return 
                            Return oRet
                        End If
                        oRet.Success = True
                        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
                        oRet.updateKeyFields("BookControl", BookControl.ToString())
                        If Not InsertLoadTenderSpotRate(oRet) Then
                            Return oRet
                        End If
                        'update the type and status codes (may not be needed but this allows the system to validate and reset type codes 
                        'and status codes as needed
                        'The BookData overloads do not require a Book Revenue object, just a book control,  they
                        'call a stored procedure which updates all the dependent booking reocrds direclty in the db.
                        'The methods in the NGLDATBLL are used when we have a book revenue object that save to the db later
                        'used when posting to NEXTStop or DAT UpdateBookRevLTTC and UpdateBookRevLTSCPost respectively
                        oBookDAL.updateBookRevLoadTenderTypeControl(BookControl, tenderTypes(0))
                        oBookDAL.updateBookRevLoadTenderStatusCodePost(BookControl, tenderTypes(0))
                    Else
                        'TODO: add exception message here for no book?
                    End If
                Case DTO.tblLoadTender.LoadTenderTypeEnum.NextStop

                    Dim source = 0
                    Dim bwLB As New NGL.Core.Utility.BitwiseFlags32(source)
                    'turn on the flag for NEXTStop
                    bwLB.turnBitFlagOn(DTO.tblLoadTender.LoadTenderTypeEnum.NextStop)
                    If tenderTypes.Count > 1 AndAlso tenderTypes(1) = DTO.tblLoadTender.LoadTenderTypeEnum.DAT Then
                        bwLB.turnBitFlagOn(DTO.tblLoadTender.LoadTenderTypeEnum.DAT)
                    End If
                    'get the booking revenue data
                    Dim oBookRevs = oBookRev.GetBookRevenuesWDetailsFiltered(BookControl)
                    oRet = oDATBLL.LoadBoardPostMethod(oBookRevs, BookControl, bwLB)
                Case DTO.tblLoadTender.LoadTenderTypeEnum.DAT
                    Dim source = 0
                    Dim bwLB As New NGL.Core.Utility.BitwiseFlags32(source)
                    'turn on the flag for NEXTStop
                    bwLB.turnBitFlagOn(DTO.tblLoadTender.LoadTenderTypeEnum.DAT)
                    If tenderTypes.Count > 1 AndAlso tenderTypes(1) = DTO.tblLoadTender.LoadTenderTypeEnum.NextStop Then
                        bwLB.turnBitFlagOn(DTO.tblLoadTender.LoadTenderTypeEnum.NextStop)
                    End If
                    'get the booking revenue data
                    Dim oBookRevs = oBookRev.GetBookRevenuesWDetailsFiltered(BookControl)

                    oRet = oDATBLL.LoadBoardPostMethod(oBookRevs, BookControl, bwLB)

                Case Else
                    'Invalid Parameter: No record exists in the database for {0}: {1}.
                    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidParameterNameValue", New String() {"Load Tender Bid Type", tenderTypes(0).ToString()})
                    SaveAppError(String.Concat("Invalid Parameter: No record exists in the database for  Load Tender Bid Type ", tenderTypes(0).ToString()))
                    Return oRet
            End Select


        Catch ex As FaultException
            Throw
        Catch ex As System.ApplicationException
            oLTLogData.AddToCollection("Application Error: " & ex.Message)
            throwInvalidOperatonException(ex.Message, True, False)
        Catch ex As Exception
            oLTLogData.AddToCollection("Unexpected Error: " & ex.Message)
            throwUnExpectedFaultException(ex, buildProcedureName("GenerateQuote"), FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
        Finally
            If Not oLTLogData Is Nothing AndAlso oLTLogData.Logs.Count() > 0 AndAlso iLoadTenderControl <> 0 Then
                oLTLogData.SaveCollectionToDB(iLoadTenderControl, Me.Parameters.UserName)
            End If
        End Try
        If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
            'if we do not have any meesages set one to  Invalid Request 
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest")
        End If
        Return oRet
    End Function

    'Public Function UpdateRateShopQuotes(ByVal iLoadTenderControl As Integer, Optional ByVal tariffOptions As DTO.GetCarriersByCostParameters = Nothing) As DTO.WCFResults
    '    Dim oRet As New DTO.WCFResults()
    '    Dim strMsg As String = ""
    '    Dim oLTLogData As New DAL.NGLLoadTenderLogData(Me.Parameters)
    '    oRet.Success = False
    '    oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
    '    'TODO: determine if we need to show a message to the user,  these options should not happen in production.
    '    If tariffOptions Is Nothing Then tariffOptions = New DTO.GetCarriersByCostParameters(False, True, True, 0, 0) 'use default for standard routing prefered = false, noLateDelivery = False, validated = True, optimizeByCapacity = True --  modeTypeControl and  tempType  are looked up later
    '    Try
    '        Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)
    '        Dim oDATBLL As BLL.NGLDATBLL = New BLL.NGLDATBLL(Parameters)
    '        Dim oBookDAL As New DAL.NGLBookData(Parameters)
    '        Dim oBookRev As New DAL.NGLBookRevenueData(Parameters)
    '        Dim oItems As New List(Of DAL.Models.RateRequestItem)
    '        'the roles may be reversed but the results should be the same.
    '        Dim oP44Proxy As P44.P44Proxy
    '        Dim oP44Data As P44.RateRequest
    '        Dim blnUseP44API As Boolean = False
    '        Dim blnUseNGLTariff As Boolean = False
    '        Dim blnUseCHRAPI As Boolean = False
    '        Dim blnUseUPSAPI As Boolean = False
    '        Dim blnUseJTSAPI As Boolean = False

    '        Dim oCHRLEConfig As New LTS.tblSSOALEConfig()
    '        Dim lCHRCompConfig As New List(Of LTS.tblSSOAConfig)

    '        Dim oJTSLEConfig As New LTS.tblSSOALEConfig()
    '        Dim lJTSCompConfig As New List(Of LTS.tblSSOAConfig)

    '        Dim oUPSLEConfig As New LTS.tblSSOALEConfig()
    '        Dim lUPSCompConfig As New List(Of LTS.tblSSOAConfig)
    '        Dim iCompControl As Integer = 0
    '        oLTLogData.AddToCollection("Read quote and bid types")

    '        blnUseP44API = True
    '        'Modified by RHR for v-8.5.3.005 on 08/24/2022 added logic to use bookcontrol to get legal entity
    '        ' get the SSOA settings if the bookcontrol does not exist GetSSOAConfig will use the users legal entity
    '        blnUseCHRAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.CHRAPI, oCHRLEConfig, lCHRCompConfig, 0)
    '        blnUseJTSAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.JTSAPI, oJTSLEConfig, lJTSCompConfig, 0)
    '        blnUseUPSAPI = NGLtblSingleSignOnAccountData.GetSSOAConfig(DAL.Utilities.SSOAAccount.UPSAPI, oUPSLEConfig, lUPSCompConfig, 0)
    '        blnUseNGLTariff = True

    '        If Not (blnUseP44API Or blnUseNGLTariff) Then
    '            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "Cannot generate quotes an API or Tariff quote Type is required")
    '            Return oRet
    '        End If
    '        Dim order As New DAL.Models.RateRequestOrder()
    '        Dim lTdata = oLT.getLoadTenderData(iLoadTenderControl)
    '        order.ID = oLT.orderi $("#txtOrderID").val(); //** Reference To BookControl If availavle Else does Not Get used **
    '        order.ShipKey = $("#txtSHID").val(); //** Reference to BookSHID if availavle else does Not get used **
    '        order.BookConsPrefix =   $("#txtBookConsPrefix").val(); //** Reference to BookConsPrefix if availavle else does Not get used :  Previously this was mapped to  $("#txtSHID").data("kendoMaskedTextBox").value();
    '        order.ShipDate = $("#txtShipDate").data("kendoDatePicker").value();  //"02-25-2017";
    '        order.DeliveryDate = $("#txtDeliveryDate").data("kendoDatePicker").value();  //"03-06-2017";
    '        order.BookCustCompControl = $("#txtBookCustCompControl").val();  //** NEVER GETS USED **
    '        order.CompName = $("#txtorigCompName").data("kendoMaskedTextBox").value();
    '        order.CompNumber = $("#txtCompNumber").val(); //** NEVER GETS USED **
    '        order.CompAlphaCode = $("#txtCompAlphaCode").val(); //** NEVER GETS USED **
    '        order.BookCarrierControl = $("#txtBookCarrierControl").val(); //** NEVER GETS USED **
    '        order.CarrierName = $("#txtCarrierName").val(); //** NEVER GETS USED **
    '        order.CarrierNumber = $("#txtCarrierNumber").val(); //** NEVER GETS USED **
    '        order.CarrierAlphaCode = $("#txtCarrierAlphaCode").val(); //** NEVER GETS USED **
    '        order.TotalCases = $("#txtTotalCases").data("kendoNumericTextBox").value(); 
    '        order.TotalWgt = $("#txtTotalWgt").data("kendoNumericTextBox").value();
    '        order.TotalPL = $("#txtTotalPlts").data("kendoNumericTextBox").value();
    '        //Begin Modified by RHR for v-8.5.4.001 add temperature
    '        var tempCode = 'Dry';
    '        var diTmp = $("#ddlLoadTemp").data("kendoDropDownList").dataItem();
    '        If (diTmp!= null) Then { tempCode = diTmp.Name; } 
    '        order.CommCodeDescription = tempCode;
    '        var tempType = 1;
    '        If (diTmp!= null) Then { tempType = diTmp.Control; }
    '        order.TariffTempType = tempType;
    '        //End Modified by RHR for v-8.5.4.001 add temperature
    '        //TotalCube
    '        var labelText = $('#txtTotalStops').text();
    '        order.TotalStops = parseInt(labelText); 
    '        order.Pickup = fnGetPickup();
    '        order.Stops = fnGetStops();
    '        order.Accessorials = fnGetAccessorials();   //LVV ADD
    '        order.WeightUnit = $("#txtWeightUnit").data("kendoComboBox").value();
    '        order.LengthUnit = $("#txtLengthUnit").data("kendoComboBox").value(); 
    '        If (order.TariffTempType <= 1) Then '0 (any) and 1 (dry) are ok for p44
    '            blnUseP44API = True
    '        End If
    '        If Not order Is Nothing Then
    '            'this is a rate shopping order with no booking record
    '            If oLT.CreateRateRequestOrderQuote(order, iLoadTenderControl, oP44Proxy, oP44Data, DAL.Utilities.SSOAAccount.P44, strMsg, blnUseP44API, oLTLogData) Then
    '                If iLoadTenderControl <> 0 Then
    '                    oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
    '                    'REM RHR 10/20/2020 If blnUseNGLTariff Then oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl)
    '                    ' Modified by RHR for v-8.3.0.002 on 10/20/2020
    '                    If blnUseNGLTariff Then
    '                        If blnUseP44API Then
    '                            ' Modified by RHR for v-8.3.0.002 on 12/17/2020
    '                            oRet.configureForAsyncMessages(iLoadTenderControl)
    '                            oDATBLL.CreateNGLTariffBidNoBookAsync(order, iLoadTenderControl, tariffOptions)
    '                        Else
    '                            oDATBLL.CreateNGLTariffBidNoBook(order, iLoadTenderControl, strMsg)
    '                        End If
    '                    End If
    '                    'While reading any tariffs we also can request the rates from P44
    '                    'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
    '                    If blnUseP44API Then
    '                        'Try
    '                        Dim blnQuotesExist = oLT.ProcessP44RateRequest(oP44Proxy, oP44Data, iLoadTenderControl, strMsg)
    '                        'Catch ex As Exception
    '                        '    Dim sMsgs = {iLoadTenderControl.ToString(), ex.Message}
    '                        '    oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_ReadQuoteFailure, sMsgs)

    '                        'End Try
    '                    End If
    '                    If blnUseCHRAPI Then
    '                        Dim blnQuotesExist = oLT.ProcessCHRRateRequest(order, iLoadTenderControl, oCHRLEConfig, lCHRCompConfig, strMsg)
    '                    End If
    '                    If blnUseUPSAPI Then
    '                        Dim blnQuotesExist = oLT.ProcessUPSRateRequest(order, iLoadTenderControl, oUPSLEConfig, lUPSCompConfig, strMsg)
    '                    End If
    '                    If blnUseJTSAPI Then
    '                        Dim blnQuotesExist = oLT.ProcessJTSRateRequest(order, iLoadTenderControl, oJTSLEConfig, lJTSCompConfig, strMsg)
    '                    End If

    '                    If iLoadTenderControl <> 0 Then
    '                        oRet.Success = True
    '                    Else
    '                        oRet.Success = False
    '                    End If
    '                End If
    '            Else
    '                'E_ReadQuoteSpFailNoLTControl
    '                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteSpFailNoLTControl")
    '                SaveAppError("Read quote failure a load tender record could not be created.")
    '                Return oRet
    '            End If
    '            If Not String.IsNullOrWhiteSpace(strMsg) Then
    '                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteFailure", New String() {iLoadTenderControl.ToString(), strMsg})
    '                SaveAppError(String.Concat("Read Quote Failure for LoadTenderControl = ", iLoadTenderControl.ToString(), ": ", strMsg))
    '            End If
    '        ElseIf BookControl > 0 Then
    '            'we have a book control so process data from the book table
    '            'Insert the Booking Info in the the Load Board returns LoadBoardControl in WCFResults
    '            'TODO: merge with spot rate insert load board and return LoadTenderControl, CNS number and SHID
    '            oRet = NGLLoadTenderData.InsertLoadBoardRecords(BookControl, "", tenderTypes(0))
    '            If Not oRet.Success Then
    '                'we have some errors so just return 
    '                Return oRet
    '            End If
    '            Dim sVals = oRet.TryGetKeys({"LoadTenderControl", "BookConsPrefix", "BookSHID"}, {"0", "Undefined", "Undefined"})
    '            Integer.TryParse(sVals(0), iLoadTenderControl)
    '            Dim sCNS As String = sVals(1)
    '            Dim sSHID As String = sVals(2)

    '            If iLoadTenderControl = 0 Then
    '                'we cannot continue
    '                oRet.Success = False
    '                'E_SaveRateSpFailNoLTControl
    '                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateSpFailNoLTControl")
    '                SaveAppError("Save rate failure a load tender record could not be created.")
    '                Return oRet
    '            End If
    '            If Not String.IsNullOrWhiteSpace(strMsg) Then
    '                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateFailure", New String() {iLoadTenderControl.ToString(), strMsg})
    '                SaveAppError(String.Concat("Save Rate Failure for LoadTenderControl = ", iLoadTenderControl.ToString(), ": ", strMsg))
    '            End If
    '            strMsg = ""
    '            'create the P44 bid 
    '            'TODO: verify that CreateP44BidAsync and ProcessP44RateRequest generate the same results
    '            ' Modified by RHR for v-8.3.0.002 on 12/17/2020
    '            oRet.configureForAsyncMessages(iLoadTenderControl)
    '            If blnUseP44API Then NGLLoadTenderData.CreateP44BidAsync(BookControl, iLoadTenderControl, "", DAL.Utilities.SSOAAccount.P44, strMsg)
    '            If blnUseCHRAPI Then
    '                Dim blnQuotesExist = oLT.CreateCHRBid(BookControl, iLoadTenderControl, "", oCHRLEConfig, lCHRCompConfig, DAL.Utilities.SSOAAccount.CHRAPI, strMsg)
    '            End If
    '            If blnUseUPSAPI Then
    '                Dim blnQuotesExist = oLT.CreateUPSBid(BookControl, iLoadTenderControl, "", oUPSLEConfig, lUPSCompConfig, DAL.Utilities.SSOAAccount.UPSAPI, strMsg)
    '            End If
    '            If blnUseJTSAPI Then
    '                Dim blnQuotesExist = oLT.CreateJTSBid(BookControl, iLoadTenderControl, "", oJTSLEConfig, lJTSCompConfig, DAL.Utilities.SSOAAccount.JTSAPI, strMsg)
    '            End If
    '            If blnUseNGLTariff Then
    '                'oDATBLL.CreateNGLTariffBid(BookControl, iLoadTenderControl, "", strMsg)
    '                'Modified by RHR for v-8.2.1 on 11/12/2019
    '                'Added logic to use prefered temperature and mode.
    '                ' we now pass in filters and call 
    '                Dim ModeType As Integer = 0 'default is zero -- any
    '                Dim TempType As Integer = 0
    '                NGLBookData.GetModeTempTypesByPrecedence(BookControl, ModeType, TempType)
    '                With tariffOptions
    '                    .BookControl = BookControl
    '                    .modeTypeControl = ModeType
    '                    .tempType = TempType
    '                End With
    '                Dim res = getEstimatedCarriersByCostFiltered(tariffOptions)
    '                'Removed by RHR for v-8.2.1 on 11/12/2019
    '                'Dim res = getEstimatedCarriersByCostAll(BookControl, 1, 1000)
    '                If res Is Nothing Then
    '                    'E_NoTariffAvailable -- A tariff is not available for the select route CNS: {0}.
    '                    oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_NoTariffAvailable", {sCNS})
    '                    SaveAppError(String.Format("A tariff is not available for the select route CNS: {0}", sCNS))
    '                    If Not blnUseP44API Then
    '                        'no need to continue 
    '                        oRet.Success = False
    '                        Return oRet
    '                    End If
    '                End If
    '                If Not res.Log Is Nothing AndAlso res.Log.Count > 0 Then oRet.Log.AddRange(res.Log)
    '                If Not res.Messages Is Nothing AndAlso res.Messages.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Messages, res.Messages)
    '                If res.CarriersByCost Is Nothing OrElse res.CarriersByCost.Count < 1 Then
    '                    If Not blnUseP44API Then
    '                        'no need to continue 
    '                        oRet.Success = False
    '                        Return oRet
    '                    End If
    '                Else
    '                    Dim oBid As New DAL.NGLBidData(Parameters)
    '                    'Modified by RHR for v-8.5.3.001 on 05/25/2022 we now use InsertNGLTariffBid365
    '                    oBid.InsertNGLTariffBid365(res, res.BookRevs, iLoadTenderControl, sSHID, strMsg)
    '                End If
    '            End If
    '            oRet.Success = True
    '            'update the type and status codes (may not be needed but this allows the system to validate and reset type codes 
    '            'and status codes as needed
    '            'The BookData overloads do not require a Book Revenue object, just a book control,  they
    '            'call a stored procedure which updates all the dependent booking reocrds direclty in the db.
    '            'The methods in the NGLDATBLL are used when we have a book revenue object that save to the db later
    '            'used when posting to NEXTStop or DAT UpdateBookRevLTTC and UpdateBookRevLTSCPost respectively
    '            oBookDAL.updateBookRevLoadTenderTypeControl(BookControl, tenderTypes(0))
    '            oBookDAL.updateBookRevLoadTenderStatusCodePost(BookControl, tenderTypes(0))
    '        End If



    '    Catch ex As FaultException
    '        Throw
    '    Catch ex As System.ApplicationException
    '        oLTLogData.AddToCollection("Application Error: " & ex.Message)
    '        throwInvalidOperatonException(ex.Message, True, False)
    '    Catch ex As Exception
    '        oLTLogData.AddToCollection("Unexpected Error: " & ex.Message)
    '        throwUnExpectedFaultException(ex, buildProcedureName("GenerateQuote"), FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
    '    Finally
    '        If Not oLTLogData Is Nothing AndAlso oLTLogData.Logs.Count() > 0 AndAlso iLoadTenderControl <> 0 Then
    '            oLTLogData.SaveCollectionToDB(iLoadTenderControl, Me.Parameters.UserName)
    '        End If
    '    End Try
    '    If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
    '        'if we do not have any meesages set one to  Invalid Request 
    '        oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest")
    '    End If
    '    Return oRet
    'End Function



    ''' <summary>
    ''' Beta version of Wrapper method for processing all dispatching from D365
    ''' Needs a little work and lots of testing.  This method must be used to dispatch
    ''' loads to P44
    ''' </summary>
    ''' <param name="oDispatch"></param>
    ''' <param name="tenderType"></param>
    ''' <param name="bidType"></param>
    ''' <param name="BidControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Created by RHR for v-8.2
    '''     much of the logic needed to properly process the DAL.Models.Dispatch data is missing
    '''     this may impact the printing of the BOL with accurate information
    '''     the DAL.Models.Dispatch data may not get saved to the booking record as expected
    '''     Dispatching to P44 should work correclty for Rate Shop, Load Board, and NEXSTop
    '''     note: load tender type may not be able to identify the difference between Rate Shopping and order dispatching.
    '''     here are the load tender types available.
    '''     NextStop = 6
    '''     P44 = 7 (we need to determine if this is rate shop or order dispatching?)
    '''     RateQuote = 8 (we need to determine if this is rate shop or order dispatching?)
    '''     SpotRate = 9
    ''' </remarks>
    Public Function DispatchBid(ByRef oDispatch As DAL.Models.Dispatch, ByVal tenderType As DTO.tblLoadTender.LoadTenderTypeEnum, ByVal bidType As DTO.tblLoadTender.BidTypeEnum, Optional ByVal BidControl As Integer = 0) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        Dim iLoadTenderControl As Integer = 0
        Dim strMsg As String = ""
        oRet.Success = False
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        If oDispatch Is Nothing And BidControl = 0 Then Return oRet

        Try
            Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)
            Dim oDATBLL As NGLDATBLL = New NGLDATBLL(Parameters)
            Dim oBookDAL As New DAL.NGLBookData(Parameters)
            Dim oBookRev As New DAL.NGLBookRevenueData(Parameters)
            Dim oItems As New List(Of DAL.Models.RateRequestItem)
            Dim oBidData As DAL.NGLBidData = New DAL.NGLBidData(Parameters)
            Dim iCarrierContactControl As Integer = 0
            If Not oDispatch.CarrierContact Is Nothing AndAlso oDispatch.CarrierContact.ContactControl <> 0 Then
                iCarrierContactControl = oDispatch.CarrierContact.ContactControl
            End If
            Dim iBidTypeControl As Integer = 0
            Select Case tenderType
                Case DTO.tblLoadTender.LoadTenderTypeEnum.LoadBoard
                    'check the bid type and use the bid type associated with the bid control if possible
                    If (oDispatch.BidControl <> 0) Then
                        iBidTypeControl = oBidData.GetBidType(oDispatch.BidControl)
                        If iBidTypeControl <> 0 Then
                            bidType = CType(iBidTypeControl, DTO.tblLoadTender.BidTypeEnum)
                        End If
                    End If
                    Select Case bidType
                        Case DTO.tblLoadTender.BidTypeEnum.NGLTariff
                            Dim SelectedCarrier = New DTO.CarriersByCost() With {.CarrierControl = oDispatch.CarrierControl, .BookCarrTarEquipMatControl = oDispatch.CarrTarEquipMatControl, .BookCarrTarEquipControl = oDispatch.CarrTarEquipControl, .BookModeTypeControl = oDispatch.ModeTypeControl}
                            oRet = oDATBLL.NSNGLTariffAccept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, SelectedCarrier, iCarrierContactControl, oDispatch, tenderType)
                        Case DTO.tblLoadTender.BidTypeEnum.P44
                            oRet = dispatchLoadBoardToP44(oDispatch)
                        Case DTO.tblLoadTender.BidTypeEnum.CHRAPI ' Move to Case Else below once Dispatching is set up for API
                            'see if we have a dispatch API SSOA account set up for CHR
                            Dim oSSOALEConfig As LTS.tblSSOALEConfig = NGLtblSingleSignOnAccountData.GetSSOALEConfigsByBidType(iBidTypeControl, oDispatch.BookControl, DAL.Utilities.SSOAType.LoadTenderOut).FirstOrDefault()
                            If Not oSSOALEConfig Is Nothing AndAlso oSSOALEConfig.SSOALESSOATypeControl = DAL.Utilities.SSOAType.LoadTenderOut Then
                                oRet = dispatchLoadBoardToAPI(oDispatch, oSSOALEConfig)
                            Else
                                oRet = dispatchLoadBoardToAPI204(oDispatch)
                            End If
                        Case DTO.tblLoadTender.BidTypeEnum.JTSAPI ' Move to Case Else below if Dispatching is set up for API
                            oRet = dispatchLoadBoardToAPI204(oDispatch)
                        Case Else
                            'The default, exec 204 dispatching 
                            oRet = dispatchLoadBoardToAPI204(oDispatch)
                    End Select
                Case DTO.tblLoadTender.LoadTenderTypeEnum.RateShopping
                    'check the bid type and use the bid type associated with the bid control if possible
                    If (oDispatch.BidControl <> 0) Then
                        iBidTypeControl = oBidData.GetBidType(oDispatch.BidControl)
                        If iBidTypeControl <> 0 Then
                            bidType = CType(iBidTypeControl, DTO.tblLoadTender.BidTypeEnum)
                        End If
                    End If
                    Select Case bidType
                        Case DTO.tblLoadTender.BidTypeEnum.NGLTariff
                            Dim SelectedCarrier = New DTO.CarriersByCost() With {.CarrierControl = oDispatch.CarrierControl, .BookCarrTarEquipMatControl = oDispatch.CarrTarEquipMatControl, .BookCarrTarEquipControl = oDispatch.CarrTarEquipControl, .BookModeTypeControl = oDispatch.ModeTypeControl}
                            oRet = oDATBLL.NSNGLTariffAccept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, SelectedCarrier, iCarrierContactControl, oDispatch, tenderType)
                        Case DTO.tblLoadTender.BidTypeEnum.P44
                            oRet = dispatchRateShopToP44(oDispatch)
                        Case DTO.tblLoadTender.BidTypeEnum.CHRAPI
                            oRet = dispatchRateShopToAPI204(oDispatch)
                        Case DTO.tblLoadTender.BidTypeEnum.JTSAPI
                            oRet = dispatchRateShopToAPI204(oDispatch)
                        Case Else
                            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidParameterNameValue", New String() {"Load Tender Bid Type", bidType.ToString()})
                            SaveAppError(String.Concat("Invalid Parameter: No record exists in the database for  Load Tender Bid Type ", bidType.ToString()))
                            Return oRet
                    End Select
                    'If Not String.IsNullOrWhiteSpace(strMsg) Then
                    'oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_ReadQuoteFailure", New String() {iLoadTenderControl.ToString(), strMsg})
                    'SaveAppError(String.Concat("Read Quote Failure for LoadTenderControl = ", iLoadTenderControl.ToString(), ": ", strMsg))
                    'End If
                Case DTO.tblLoadTender.LoadTenderTypeEnum.SpotRate
                    'needs more work.
                    oRet = oDATBLL.NSAcceptSpotRate(oDispatch.LoadTenderControl, oDispatch.BidControl, oDispatch.BookControl, iCarrierContactControl, oDispatch)
                Case DTO.tblLoadTender.LoadTenderTypeEnum.NextStop
                    Select Case bidType
                        Case DTO.tblLoadTender.BidTypeEnum.NextStop
                            oRet = oDATBLL.NSAccept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, oDispatch.CarrierControl, oDispatch.LineHaul, oDispatch.FuelUOM, oDispatch.FuelVariable, iCarrierContactControl)
                        Case DTO.tblLoadTender.BidTypeEnum.NGLTariff
                            Dim SelectedCarrier = New DTO.CarriersByCost() With {.CarrierControl = oDispatch.CarrierControl, .BookCarrTarEquipMatControl = oDispatch.CarrTarEquipMatControl, .BookCarrTarEquipControl = oDispatch.CarrTarEquipControl, .BookModeTypeControl = oDispatch.ModeTypeControl}
                            oRet = oDATBLL.NSNGLTariffAccept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, SelectedCarrier, iCarrierContactControl, oDispatch, tenderType)
                        Case DTO.tblLoadTender.BidTypeEnum.P44
                            oRet = dispatchNEXTStopToP44(oDispatch)
                        Case DTO.tblLoadTender.BidTypeEnum.CHRAPI
                            Dim SelectedCarrier = New DTO.CarriersByCost() With {.CarrierControl = oDispatch.CarrierControl, .BookCarrTarEquipMatControl = oDispatch.CarrTarEquipMatControl, .BookCarrTarEquipControl = oDispatch.CarrTarEquipControl, .BookModeTypeControl = oDispatch.ModeTypeControl}
                            oRet = oDATBLL.NSNGLTariffAccept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, SelectedCarrier, iCarrierContactControl, oDispatch, tenderType)
                        Case DTO.tblLoadTender.BidTypeEnum.JTSAPI
                            Dim SelectedCarrier = New DTO.CarriersByCost() With {.CarrierControl = oDispatch.CarrierControl, .BookCarrTarEquipMatControl = oDispatch.CarrTarEquipMatControl, .BookCarrTarEquipControl = oDispatch.CarrTarEquipControl, .BookModeTypeControl = oDispatch.ModeTypeControl}
                            oRet = oDATBLL.NSNGLTariffAccept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, SelectedCarrier, iCarrierContactControl, oDispatch, tenderType)

                        Case Else
                            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidParameterNameValue", New String() {"Load Tender Bid Type", bidType.ToString()})
                            SaveAppError(String.Concat("Invalid Parameter: No record exists in the database for  Load Tender Bid Type ", bidType.ToString()))
                            Return oRet
                    End Select
                Case DTO.tblLoadTender.LoadTenderTypeEnum.DAT


                Case Else
                    'Invalid Parameter: No record exists in the database for {0}: {1}.
                    oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidParameterNameValue", New String() {"Load Tender Bid Type", bidType.ToString()})
                    SaveAppError(String.Concat("Invalid Parameter: No record exists in the database for  Load Tender Bid Type ", bidType.ToString()))
                    Return oRet
            End Select
            'get the text for the BOL
            Try
                Dim oLEDAL As DAL.NGLLegalEntityAdminData = New DAL.NGLLegalEntityAdminData(Parameters)
                oDispatch.DispatchLegalText = oLEDAL.getDispatchLegalText()
            Catch
            End Try
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("DispatchBid"), DAL.sysErrorParameters.sysErrorState.UserLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest") 'if we do not have any messages set one to Invalid Request 
        End If
        Return oRet
    End Function


    Public Function AssignBid(ByRef oDispatch As DAL.Models.Dispatch) As DTO.WCFResults

        Dim results As New DTO.WCFResults()

        If oDispatch Is Nothing OrElse oDispatch.BookControl = 0 Then
            results.AddLog("Cannot read the order data")
            results.Success = False
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
            Return results
        End If

        Dim BookControl As Integer = oDispatch.BookControl
        results.AddLog("Read the order data")
        Dim oBookRevs = NGLBookRevenueData.GetBookRevenues(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            results.Success = False
            results.AddLog("BookControl Not Found: " & BookControl.ToString())
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
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
        results.AddLog("Read the bid data")
        Dim oBid As LTS.tblBid = NGLBidData.GetRecord(oDispatch.BidControl)
        If oBid Is Nothing OrElse oBid.BidControl = 0 Then
            results.Success = False
            results.AddLog("Bid Not Found For: " & oDispatch.CarrierName)
            Dim p As String() = {oDispatch.LoadTenderControl.ToString(), " The bid control is missing."}
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_ReadQuoteFailure, p)
            Return results
        End If
        results.AddLog("Read Selected Carrier Data")
        Dim oSelectedCarrier As clsPreferedDefaultCarrier = New clsPreferedDefaultCarrier(oBid)

        If oSelectedCarrier Is Nothing Then
            results.Success = False
            results.AddLog("Bid Not Found For: " & oDispatch.CarrierName)
            Dim p As String() = {oDispatch.LoadTenderControl.ToString(), " Cannot read carrier data from bid."}
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_ReadQuoteFailure, p)
            Return results
        End If

        'we update the default carrier control and use it further down
        intDefaultCarrier = oSelectedCarrier.SelectedBid.BidCarrierControl
        strCarrierNumber = oSelectedCarrier.SelectedBid.BidCarrierNumber
        strCarrierName = oSelectedCarrier.SelectedBid.BidCarrierName
        Dim iLoadTenderControl = oDispatch.LoadTenderControl
        Dim oRet As New DTO.WCFResults()
        Select Case oSelectedCarrier.SelectedBid.BidBidTypeControl
            Case DTO.tblLoadTender.BidTypeEnum.NGLTariff
                Dim SelectedCarrier = New DTO.CarriersByCost() With {.CarrierControl = oSelectedCarrier.SelectedBid.BidCarrierControl, .BookCarrTarEquipMatControl = oSelectedCarrier.SelectedBid.BidBookCarrTarEquipMatControl, .BookCarrTarEquipControl = oSelectedCarrier.SelectedBid.BidBookCarrTarEquipControl, .BookModeTypeControl = oSelectedCarrier.SelectedBid.BidBookModeTypeControl}
                oRet = NGLDATBLL.NSNGLTariffAssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, SelectedCarrier, 0)
            Case DTO.tblLoadTender.BidTypeEnum.P44
                oRet = NGLDATBLL.NSP44AssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, oSelectedCarrier, 0)
            Case DTO.tblLoadTender.BidTypeEnum.CHRAPI
                oRet = NGLDATBLL.NSAPIAssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, oSelectedCarrier, 0)
            Case DTO.tblLoadTender.BidTypeEnum.JTSAPI
                oRet = NGLDATBLL.NSAPIAssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, oSelectedCarrier, 0)
            Case DTO.tblLoadTender.BidTypeEnum.UPSAPI
                oRet = NGLDATBLL.NSAPIAssignSilent(iLoadTenderControl, BookControl, oBookRevs(0).BookCarrOrderNumber, oSelectedCarrier, 0)
            Case Else
                results.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidParameterNameValue", New String() {"Load Tender Bid Type", oSelectedCarrier.SelectedBid.BidBidTypeControl})
                SaveAppError(String.Concat("Invalid Parameter: No record exists in the database for  Load Tender Bid Type ", oSelectedCarrier.SelectedBid.BidBidTypeControl))
                Return results
        End Select
        If Not oRet Is Nothing Then
            If oRet.Log.Count() > 0 Then
                For Each lg In oRet.Log
                    results.Log.Add(lg)
                Next
            End If

            If oRet.Messages.Count() > 0 Then
                For Each msg In oRet.Messages
                    results.Messages.Add(msg.Key, msg.Value)
                Next
            End If

            If oRet.Warnings.Count() > 0 Then
                For Each warn In oRet.Warnings
                    results.Warnings.Add(warn.Key, warn.Value)
                Next
            End If

            If oRet.Errors.Count() > 0 Then
                For Each ers In oRet.Errors
                    results.Errors.Add(ers.Key, ers.Value)
                Next
            End If
            results.Success = oRet.Success
        Else
            results.Success = False
            Dim p As String() = {" Cannot assign carrier to load."}
            results.AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Errors, DTO.WCFResults.MessageEnum.E_UnExpected_Error, p)
        End If
        Return results

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDispatch"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Modified call to NSP44Accept() by passing in the field oDispatch.CarrierContact.ContactControl
    '''   This is ok because dispatchRateShopToP44() is called by NGLBookRevenueBLL.DispatchBid()
    '''   and that method is called by the UI, so oDispatch.CarrierContact.ContactControl will have been populated
    '''   because this method call is still within the same process flow
    ''' Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
    ''' </remarks>
    Private Function dispatchRateShopToP44(ByRef oDispatch As DAL.Models.Dispatch) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        Dim iLoadTenderControl As Integer = 0
        Dim strMsg As String = ""
        oRet.Success = False
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        If oDispatch Is Nothing Then Return oRet
        Try
            Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)
            Dim oDATBLL As BLL.NGLDATBLL = New BLL.NGLDATBLL(Parameters)
            oRet = oLT.DispatchToP44(oDispatch, DAL.Utilities.SSOAAccount.P44, strMsg)
            If Not oRet.Success Then Return oRet
            If oDispatch.LoadTenderControl <> 0 Then
                Dim iBookControl As Integer = 0
                'create or update booking, company and lane data associated with this shipment
                iBookControl = oLT.dispatchLoadTender(oDispatch.LoadTenderControl, oDispatch, strMsg)

                If Not String.IsNullOrWhiteSpace(strMsg) Then
                    'Warning: Your load has been dispatched; however, we could not save the booking information.  Please create the booking information manually. {0}'
                    oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_MissingLoadTenderDispatched, New String() {strMsg})
                    oRet.Success = False
                    SaveAppError(oRet.getAllMessagesNotLocalized())
                    Return oRet
                End If
                If iBookControl <> 0 Then
                    Try
                        Dim oLEDAL As DAL.NGLLegalEntityAdminData = New DAL.NGLLegalEntityAdminData(Parameters)
                        oDispatch.DispatchLegalText = oLEDAL.getDispatchLegalText()
                    Catch
                    End Try
                    oDispatch.BookControl = iBookControl
                    'mark the load as accepted
                    Dim dTotalCost As Decimal = 0
                    Decimal.TryParse(oDispatch.TotalCost, dTotalCost)
                    'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
                    'Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
                    oRet = oDATBLL.NSP44Accept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, oDispatch.CarrierControl, oDispatch.LineHaul, oDispatch.FuelUOM, oDispatch.FuelVariable, oDispatch.Fuel, dTotalCost, oDispatch.CarrierContact.ContactControl, oDispatch)

                    If oRet IsNot Nothing AndAlso oRet.Warnings.Count > 0 Then
                        'we need to find out why we add then remove this warning?
                        If oRet.Warnings.ContainsKey("W_ManualAutoAcceptNoTenderEmail") Then
                            oRet.Warnings.Remove("W_ManualAutoAcceptNoTenderEmail")
                        End If
                    End If
                End If
            Else
                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_MissingLoadTenderDispatched, New String() {"The Load Tender Control is not valid."})
                oRet.Success = False
                SaveAppError(oRet.getAllMessagesNotLocalized())
                Return oRet
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("dispatchToP44"), DAL.sysErrorParameters.sysErrorState.UserLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest") 'if we do not have any meesages set one to Invalid Request 
        End If
        Return oRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDispatch"></param>
    ''' <param name="blnReadDispatchDataFromLoadTender"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Modified call to NSP44Accept() by passing in the field oDispatch.CarrierContact.ContactControl
    '''   This is ok because dispatchNEXTStopToP44() is called by NGLBookRevenueBLL.DispatchBid()
    '''   and that method is called by the UI, so oDispatch.CarrierContact.ContactControl will have been populated
    '''   because this method call is still within the same process flow
    ''' Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
    ''' </remarks>
    Private Function dispatchNEXTStopToP44(ByRef oDispatch As DAL.Models.Dispatch, Optional ByVal blnReadDispatchDataFromLoadTender As Boolean = True) As DTO.WCFResults
        ''in this method we populate the missing data needed by P44 to dispatch a load
        'normally the client would call NGLLoadTenderData.getBidToDispatch and use the results to populate
        'all of the required fields for the DAL.Models.Dispatch data.  This functions checks that all fields
        'are populated (in the future the client should use the dispatch dialog to do this)
        Dim oRet As New DTO.WCFResults()
        Dim iLoadTenderControl As Integer = 0
        Dim strMsg As String = ""
        oRet.Success = False
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        If oDispatch Is Nothing Then Return oRet
        Try
            Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)
            If oDispatch.LoadTenderControl = 0 Then oLT.throwFieldRequiredException("Load Tender Control") 'The system could not generate a new {0}
            If oDispatch.BidControl = 0 Then oLT.throwFieldRequiredException("Bid Control") 'The system could not generate a new {0}
            If oDispatch.BookControl = 0 Then oLT.throwFieldRequiredException("Book Control") 'The system could not generate a new {0}

            If blnReadDispatchDataFromLoadTender Then
                'get the bid data 
                Dim oBidToDispatch = oLT.getBidToDispatch(oDispatch.BidControl)
                If oBidToDispatch Is Nothing Then
                    oLT.throwFieldRequiredException("Bid Control")
                End If
                oRet = oLT.DispatchToP44(oBidToDispatch, DAL.Utilities.SSOAAccount.P44, strMsg)
                If Not oRet.Success Then Return oRet
                'copy NEXTStop fields to new dispatch object
                oBidToDispatch.DispatchBidType = oDispatch.DispatchBidType
                oBidToDispatch.CarrierControl = oDispatch.CarrierControl
                oBidToDispatch.CarrTarEquipMatControl = oDispatch.CarrTarEquipMatControl
                oBidToDispatch.CarrTarEquipControl = oDispatch.CarrTarEquipControl
                oBidToDispatch.ModeTypeControl = oDispatch.ModeTypeControl
                'save oBidToDispatch  as oDispatch
                oDispatch = oBidToDispatch
            Else
                oRet = oLT.DispatchToP44(oDispatch, DAL.Utilities.SSOAAccount.P44, strMsg)
                If Not oRet.Success Then Return oRet
            End If

            Dim oDATBLL As NGLDATBLL = New NGLDATBLL(Parameters)
            Dim dTotalCost As Decimal = 0
            Decimal.TryParse(oDispatch.TotalCost, dTotalCost)
            'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
            'Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
            oRet = oDATBLL.NSP44Accept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, oDispatch.CarrierControl, oDispatch.LineHaul, oDispatch.FuelUOM, oDispatch.FuelVariable, oDispatch.Fuel, dTotalCost, oDispatch.CarrierContact.ContactControl, oDispatch)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("dispatchNEXTStopToP44"), DAL.sysErrorParameters.sysErrorState.UserLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest") 'if we do not have any meesages set one to Invalid Request 
        End If
        Return oRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDispatch"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Modified call to NSP44Accept() by passing in the field oDispatch.CarrierContact.ContactControl
    '''   This is ok because dispatchLoadBoardToP44() is called by NGLBookRevenueBLL.DispatchBid()
    '''   and that method is called by the UI, so oDispatch.CarrierContact.ContactControl will have been populated
    '''   because this method call is still within the same process flow
    ''' Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
    ''' </remarks>
    Private Function dispatchLoadBoardToP44(ByRef oDispatch As DAL.Models.Dispatch) As DTO.WCFResults
        ''in this method we populate the missing data needed by P44 to dispatch a load
        'normally the client would call NGLLoadTenderData.getBidToDispatch and use the results to populate
        'all of the required fields for the DAL.Models.Dispatch data.  This functions checks that all fields
        'are populated (in the future the client should use the dispatch dialog to do this)
        Dim oRet As New DTO.WCFResults()
        Dim iLoadTenderControl As Integer = 0
        Dim strMsg As String = ""
        oRet.Success = False
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        If oDispatch Is Nothing Then Return oRet
        Try
            Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)

            If oDispatch.LoadTenderControl = 0 Then oLT.throwFieldRequiredException("Load Tender Control") 'The system could not generate a new {0}
            If oDispatch.BidControl = 0 Then oLT.throwFieldRequiredException("Bid Control") 'The system could not generate a new {0}
            If oDispatch.BookControl = 0 Then oLT.throwFieldRequiredException("Book Control") 'The system could not generate a new {0}

            oRet = oLT.DispatchToP44(oDispatch, DAL.Utilities.SSOAAccount.P44, strMsg)
            If Not oRet.Success Then Return oRet

            Dim oDATBLL As NGLDATBLL = New NGLDATBLL(Parameters)
            Dim dTotalCost As Decimal = 0
            Decimal.TryParse(oDispatch.TotalCost, dTotalCost)
            'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
            'Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
            oRet = oDATBLL.NSP44Accept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, oDispatch.CarrierControl, oDispatch.LineHaul, oDispatch.FuelUOM, oDispatch.FuelVariable, oDispatch.Fuel, dTotalCost, oDispatch.CarrierContact.ContactControl, oDispatch)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("dispatchLoadBoardToP44"), DAL.sysErrorParameters.sysErrorState.UserLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest") 'if we do not have any meesages set one to Invalid Request 
        End If
        Return oRet
    End Function

    Private Function dispatchLoadBoardToAPI(ByRef oDispatch As DAL.Models.Dispatch, ByVal oSSOALEConfig As LTS.tblSSOALEConfig) As DTO.WCFResults
        ''in this method we populate the missing data needed by APIs to dispatch a load
        'normally the client would call NGLLoadTenderData.getBidToDispatch and use the results to populate
        'all of the required fields for the DAL.Models.Dispatch data.  This functions checks that all fields
        'are populated (in the future the client should use the dispatch dialog to do this?)
        Dim oRet As New DTO.WCFResults()
        Dim iLoadTenderControl As Integer = 0
        Dim strMsg As String = ""
        oRet.Success = False
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        If oDispatch Is Nothing Then Return oRet
        Try
            Dim oAPIBLL As New NGLAPIBLL(Me.Parameters)

            If oDispatch.LoadTenderControl = 0 Then oAPIBLL.throwFieldRequiredException("Load Tender Control") 'The system could not generate a new {0}
            If oDispatch.BidControl = 0 Then oAPIBLL.throwFieldRequiredException("Bid Control") 'The system could not generate a new {0}
            If oDispatch.BookControl = 0 Then oAPIBLL.throwFieldRequiredException("Book Control") 'The system could not generate a new {0}
            'Get the SSOA Account data.
            oRet = oAPIBLL.DispatchToAPI(oDispatch, oSSOALEConfig, strMsg)
            If Not oRet.Success Then Return oRet

            Dim oDATBLL As NGLDATBLL = New NGLDATBLL(Parameters)
            Dim dTotalCost As Decimal = 0
            Decimal.TryParse(oDispatch.TotalCost, dTotalCost)
            'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
            'Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
            oRet = oDATBLL.NSP44Accept(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, oDispatch.CarrierControl, oDispatch.LineHaul, oDispatch.FuelUOM, oDispatch.FuelVariable, oDispatch.Fuel, dTotalCost, oDispatch.CarrierContact.ContactControl, oDispatch)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("dispatchLoadBoardToP44"), DAL.sysErrorParameters.sysErrorState.UserLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest") 'if we do not have any meesages set one to Invalid Request 
        End If
        Return oRet
    End Function

    Private Function dispatchLoadBoardToAPI204(ByRef oDispatch As DAL.Models.Dispatch) As DTO.WCFResults

        Dim oRet As New DTO.WCFResults()
        Dim iLoadTenderControl As Integer = 0
        Dim strMsg As String = ""
        oRet.Success = False
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        If oDispatch Is Nothing Then Return oRet
        Try
            Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)

            If oDispatch.LoadTenderControl = 0 Then oLT.throwFieldRequiredException("Load Tender Control") 'The system could not generate a new {0}
            If oDispatch.BidControl = 0 Then oLT.throwFieldRequiredException("Bid Control") 'The system could not generate a new {0}
            If oDispatch.BookControl = 0 Then oLT.throwFieldRequiredException("Book Control") 'The system could not generate a new {0}


            Dim oDATBLL As NGLDATBLL = New NGLDATBLL(Parameters)
            Dim dTotalCost As Decimal = 0
            Decimal.TryParse(oDispatch.TotalCost, dTotalCost)
            oRet = oDATBLL.NSAPITender(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, oDispatch.CarrierControl, oDispatch.LineHaul, oDispatch.FuelUOM, oDispatch.FuelVariable, oDispatch.Fuel, dTotalCost, oDispatch.CarrierContact.ContactControl, oDispatch)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("dispatchLoadBoardToAPI204"), DAL.sysErrorParameters.sysErrorState.UserLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest") 'if we do not have any meesages set one to Invalid Request 
        End If
        Return oRet
    End Function


    Private Function dispatchRateShopToAPI204(ByRef oDispatch As DAL.Models.Dispatch) As DTO.WCFResults

        Dim oRet As New DTO.WCFResults()
        Dim iLoadTenderControl As Integer = 0
        Dim strMsg As String = ""
        oRet.Success = False
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        If oDispatch Is Nothing Then Return oRet
        Try
            Dim oLT As DAL.NGLLoadTenderData = New DAL.NGLLoadTenderData(Parameters)

            If oDispatch.LoadTenderControl = 0 Then oLT.throwFieldRequiredException("Load Tender Control") 'The system could not generate a new {0}
            'not available from rate shop
            'If oDispatch.BidControl = 0 Then oLT.throwFieldRequiredException("Bid Control") 'The system could not generate a new {0}
            'If oDispatch.BookControl = 0 Then oLT.throwFieldRequiredException("Book Control") 'The system could not generate a new {0}
            Dim oDATBLL As NGLDATBLL = New NGLDATBLL(Parameters)
            Dim iBookControl As Integer = 0
            'create or update booking, company and lane data associated with this shipment
            iBookControl = oLT.dispatchLoadTender(oDispatch.LoadTenderControl, oDispatch, strMsg)
            If Not String.IsNullOrWhiteSpace(strMsg) Then
                'Warning: Your load has been dispatched; however, we could not save the booking information.  Please create the booking information manually. {0}'
                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_MissingLoadTenderDispatched, New String() {strMsg})
                oRet.Success = False
                SaveAppError(oRet.getAllMessagesNotLocalized())
                Return oRet
            End If
            If iBookControl = 0 Then
                oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_MissingLoadTenderDispatched, New String() {"The Load Tender Control is not valid."})
                oRet.Success = False
                SaveAppError(oRet.getAllMessagesNotLocalized())
                Return oRet
            End If
            Try
                Dim oLEDAL As DAL.NGLLegalEntityAdminData = New DAL.NGLLegalEntityAdminData(Parameters)
                oDispatch.DispatchLegalText = oLEDAL.getDispatchLegalText()
            Catch
            End Try
            oDispatch.BookControl = iBookControl
            Dim dTotalCost As Decimal = 0
            Decimal.TryParse(oDispatch.TotalCost, dTotalCost)
            oRet = oDATBLL.NSAPITender(oDispatch.LoadTenderControl, oDispatch.BookControl, oDispatch.BidControl, oDispatch.CarrierControl, oDispatch.LineHaul, oDispatch.FuelUOM, oDispatch.FuelVariable, oDispatch.Fuel, dTotalCost, oDispatch.CarrierContact.ContactControl, oDispatch)
            If oRet IsNot Nothing AndAlso oRet.Warnings.Count > 0 Then
                'we need to find out why we add then remove this warning?
                If oRet.Warnings.ContainsKey("W_ManualAutoAcceptNoTenderEmail") Then
                    oRet.Warnings.Remove("W_ManualAutoAcceptNoTenderEmail")
                End If
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            throwUnExpectedFaultException(ex, buildProcedureName("dispatchRateShopToAPI204"), DAL.sysErrorParameters.sysErrorState.UserLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        If oRet.Success = False AndAlso (oRet.Errors.Count < 1 And oRet.Messages.Count < 1 And oRet.Warnings.Count < 1) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Errors, "E_InvalidRequest") 'if we do not have any meesages set one to Invalid Request 
        End If
        Return oRet
    End Function

#End Region

#Region "  Bid IT"

#End Region

#Region "  Dispatch IT"

#End Region

#End Region

#Region "Private Methods"

    Private Sub processRecalculateParameters(ByVal BookControl As Integer, ByVal ErrNumber As Integer, ByVal RetMsg As String, ByVal Procedure As String, ByVal MustRecalculate As Boolean)
        If ErrNumber <> 0 Then
            throwFaultException(FreightMaster.Data.SqlFaultInfo.FaultInfoMsgs.E_SQLExceptionMSG, FreightMaster.Data.SqlFaultInfo.FaultDetailsKey.E_SQLWarningDetails, New List(Of String) From {Procedure, ErrNumber, RetMsg}, FreightMaster.Data.SqlFaultInfo.FaultReasons.E_SQLException)
        ElseIf MustRecalculate Then
            AssignCarrier(BookControl, True)
        End If
    End Sub

    ''' <summary>
    ''' this method is private because it returns a generic list of integer.  this type of object cannot be passed 
    ''' back to the client via NGL's WCF service because it does not support the change tracking interface.  A wrapper method
    ''' with a different type of return object is requred by NGL's WCF service
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 9/19/14 we now only change the fees for oLastBookFees and call saveRevenue (this method will update all dependend fees)
    ''' </remarks>
    Private Function UpdateBookFuelFeeForLoad(ByVal BookControl As Integer) As List(Of Integer)
        Dim lRet As New List(Of Integer)
        Using Logger.StartActivity("UpdateBookFuelFeeForLoad(BookControl: {BookControl})", BookControl)
            Try
                'Get a list of BookRevenue objects
                Dim lBookRevs As DTO.BookRevenue() = NGLBookRevenueData.GetBookRevenuesWDetailsFiltered(BookControl)
                If lBookRevs Is Nothing OrElse lBookRevs.Count < 1 Then Return lRet
                'check if any order on the load has locked costs. if so skip this whole thing.
                Dim lBookRevsLocked = lBookRevs.Where(Function(x) x.BookLockAllCosts = True).ToList()
                If lBookRevsLocked.Count > 0 Then Return lRet
                'check if any order on the load was scheduled to ship before today
                If lBookRevs.Any(Function(x) x.BookDateLoad <= Date.Now.ToShortDateString()) Then
                    'if any book record has a load date before the date when the fuel surcharge update is executed 
                    '(generally calculated as 12:00 am on the current date); we skip the update for the entire load.
                    Return lRet
                End If
                'Dim lBookRevsShipped = lBookRevs.Where(Function(x) x.BookDateLoad < Date.Now.ToShortDateString()).ToList()
                ''if any book record has a load date before the date when the fuel surcharge update is executed 
                ''(generally calculated as 12:00 am on the current date); we skip the update for the entire load.
                'If lBookRevsShipped Is Nothing OrElse lBookRevsShipped.Count < 1 Then Return lRet
                'Update the return value list with all the control numbers.  This can be used by the calling procedure
                'to prevent duplicate calls to bookcontrol numbers associated with consolidated loads
                lRet = (From d In lBookRevs Order By d.BookControl Select control = d.BookControl).ToList()
                'Select Fields From Last Stop 
                Dim oLast As LTS.udfGetTariffSelectionKeysResult = NGLBookData.GetLastStopData(lBookRevs(0).BookControl)
                If oLast Is Nothing Then
                    Logger.Information("NGLBookData.GetLastStopData for BookControl: {BookControl} returned nothing, returning {@lRet}", lBookRevs(0)?.BookControl, lRet)
                    Return lRet
                End If
                Dim LastStopControl As Integer = oLast.RatedBookControl
                'Get the FuelSurCharge
                Dim oRet = NGLCarrierFuelAddendumData.GetFuelSurChargeForBook(LastStopControl, Date.Now.ToShortDateString())
                'check if we have a fuel surcharge to update 
                If oRet Is Nothing OrElse oRet.FuelSurcharge <= 0 Then
                    Logger.Information("NGLCarrierFuelAddendumData.GetFuelSurChargeForBook for {LastStopControl} returned nothing or Fuel Surcharge < 0", LastStopControl)
                    Return lRet 'nothing to update for this load
                End If
                'Modified by RHR 9/19/14 we now only change the fees for oLastBookFees and call saveRevenue (this method will update all dependend fees)
                'Modified by RHR 2/2/2016 v-7.0.4.1 'put back the logic to update each fee seperately.
                Dim oLastBook As DTO.BookRevenue = lBookRevs.Where(Function(x) x.BookControl = LastStopControl).FirstOrDefault()
                If oLastBook Is Nothing OrElse oLastBook.BookControl = 0 Then
                    Logger.Information("lBookRevs.Where for BookControl: {LastStopControl} returned nothing, returning {@lRet}", LastStopControl, lRet)
                    Return lRet 'nothing to do
                End If
                Dim blnRecalculate As Boolean = False
                For Each oBook In lBookRevs
                    Dim blnFeeFound As Boolean = False
                    Logger.Information("Checking bookFees where AccessorialFeeTypeControl = DAL.Utilities.AccessorialFeeType.Tariff (1) and BookFeesAccessorialCode = 2 or 9")
                    Dim tarFuelFee = (From d In oBook.BookFees Where d.BookFeesAccessorialFeeTypeControl = DAL.Utilities.AccessorialFeeType.Tariff And (d.BookFeesAccessorialCode = 2 Or d.BookFeesAccessorialCode = 9) Order By d.BookFeesAccessorialCode Select d).FirstOrDefault()
                    If Not tarFuelFee Is Nothing AndAlso tarFuelFee.BookFeesControl <> 0 Then

                        'we have a fee so update the data to match the retun value
                        tarFuelFee.BookFeesVariable = oRet.FuelSurcharge
                        tarFuelFee.BookFeesAccessorialCode = If(oRet.UseRatePerMile, 9, 2)
                        tarFuelFee.BookFeesVariableCode = If(oRet.UseRatePerMile, 4, 3)
                        tarFuelFee.BookFeesCaption = If(oRet.UseRatePerMile, "Fuel SurCharge Per Mile", "Fuel SurCharge Percent")
                        tarFuelFee.AllowOverwrite = True
                        Logger.Information("Fuel Fee Found, Set to use RatePerMile {RatePerMile} with AvgFuelPrice {AvgFuelPrice}, on TariffControl {CarrTarControl} and equipment {CarrTarEquipControl}, effective Date: {EffectiveDate}", oRet.UseRatePerMile, oRet.AvgFuelPrice, oRet.CarrTarControl, oRet.CarrTarEquipControl, oRet.EffectiveDate)
                        oBook.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                        Logger.Information("Call NGLBookRevenueData.SaveRevenue(oBook, UpdateDependencyResult: False, saveDetails: False, replaceFees: false, Return Changes: True, UpdateDependencies: True ")
                        NGLBookRevenueData.SaveRevenue(oBook, False, True, False, False, True)

                        blnRecalculate = True
                    End If
                Next
                'Dim blnFeeFound As Boolean = False
                'Dim tarFuelFee = (From d In oLastBook.BookFees Where d.BookFeesAccessorialFeeTypeControl = DAL.Utilities.AccessorialFeeType.Tariff And (d.BookFeesAccessorialCode = 2 Or d.BookFeesAccessorialCode = 9) Order By d.BookFeesAccessorialCode Select d).FirstOrDefault()
                'If Not tarFuelFee Is Nothing AndAlso tarFuelFee.BookFeesControl <> 0 Then
                '    'we have a fee so update the data to match the retun value
                '    tarFuelFee.BookFeesVariable = oRet.FuelSurcharge
                '    tarFuelFee.BookFeesAccessorialCode = If(oRet.UseRatePerMile, 9, 2)
                '    tarFuelFee.BookFeesVariableCode = If(oRet.UseRatePerMile, 4, 3)
                '    tarFuelFee.BookFeesCaption = If(oRet.UseRatePerMile, "Fuel SurCharge Per Mile", "Fuel SurCharge Percent")
                '    tarFuelFee.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                '    'Modified by RHR 2/2/2016 v-7.0.4.1 
                '    tarFuelFee.AllowOverwrite = True
                '    oLastBook.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                '    blnFeeFound = True
                'End If
                If blnRecalculate Then
                    Logger.Information("blnCalcluate True, Calling Assign Carrier with {LastStopControl}", LastStopControl)
                    AssignCarrier(LastStopControl, True)
                End If


                ''Update Tariff Fees for each order if it exists.  
                ''We do not add missing fees via this process; users must re-assign the carrier to update changes to fuel accessorial fees.  
                ''This solves the issue for spot rate transportation
                'For Each b In lBookRevs
                '    'modified by RHR 8/27/14 removed where condition And d.BookFeesOverRidden = False because we still 
                '    'need to update the BookFeesVariable and the BookFeesVariableCode if the fuel addendum changes even
                '    'if this fee has been overridden.
                '    Dim tarFuelFee = (From d In b.BookFees Where d.BookFeesAccessorialFeeTypeControl = DAL.Utilities.AccessorialFeeType.Tariff And (d.BookFeesAccessorialCode = 2 Or d.BookFeesAccessorialCode = 9) Order By d.BookFeesAccessorialCode Select d).FirstOrDefault()
                '    If Not tarFuelFee Is Nothing AndAlso tarFuelFee.BookFeesControl <> 0 Then
                '        'we have a fee so update the data to match the retun value
                '        tarFuelFee.BookFeesVariable = oRet.FuelSurcharge
                '        tarFuelFee.BookFeesAccessorialCode = If(oRet.UseRatePerMile, 9, 2)
                '        tarFuelFee.BookFeesVariableCode = If(oRet.UseRatePerMile, 4, 3)
                '        tarFuelFee.BookFeesCaption = If(oRet.UseRatePerMile, "Fuel SurCharge Per Mile", "Fuel SurCharge Percent")
                '        tarFuelFee.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                '        b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                '    End If
                'Next
                ''Save BookRevenue Changes (TODO: note in future we may be able to pass the BookRevenue object to the Recalculate method below and only save once)
                'NGLBookRevenueData.SaveRevenuesNoReturn(lBookRevs)
                'Recalculate Carrier Costs, we use the last book control just to be sure we get the latest changes from the db
                'AssignCarrier(LastStopControl, True)
            Catch ex As FaultException
                Logger.Error(ex, "UpdateBookFuelFeeForLoad")
                Throw
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("UpdateBookFuelFee"), FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
            End Try
        End Using

        'Return the list of BookControl records affected
        Return lRet
    End Function

#End Region

End Class
