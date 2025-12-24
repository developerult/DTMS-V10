Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports TAR = Ngl.FM.CarTar
Imports PCM = Ngl.FreightMaster.PCMiler
Imports Ngl.FM.ERE.Transform

Public Class NGLSolutionDetailBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLSolutionDetailBLL"
    End Sub

#End Region

#Region " Properties "


#End Region


#Region " Public Methods"

    Public Sub LoadPlanningItemDropped(ByVal truck As DTO.tblSolutionTruck, ByVal detail As DTO.tblSolutionDetail, ByVal droppedIndex As Integer)
        UpdateSolutionTruckDetailIndexAfterDrop(truck, detail, droppedIndex)
        OrderImportBLL.reSequenceStopNumbers(truck, truck.SolutionDetails.IndexOf(detail))
    End Sub


    Public Sub clearTruckMessages(ByVal compControl As Integer, ByVal truckKey As String)
        NGLLoadPlanningTruckData.deleteLoadPlanningTruckMessagesAsync(compControl, truckKey)
    End Sub

    Public Sub LoadPlanningReSequenceStopNumbers(ByVal truck As DTO.tblSolutionTruck)
        OrderImportBLL.reSequenceStopNumbers(truck, 0)
    End Sub

    ''' <summary>
    ''' Updates Miles and then recalculates costs for the entire truck.  a tblSolutionTruck object is required. 
    ''' If calcMiles is false all miles are set to zero 
    ''' If calcCost is false all costs will be set to zero
    ''' calcUsingTariff must be true to update the linehaul else it just recalculates fees and other costs using the current line haul
    ''' like when using a spot rate.
    ''' </summary>
    ''' <param name="truck"></param>
    ''' <param name="calcCost"></param>
    ''' <param name="calcUsingTariff"></param>
    ''' <param name="calcMiles"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadPlanningRecalculateTruck(ByVal truck As DTO.tblSolutionTruck, ByVal calcCost As Boolean, ByVal calcUsingTariff As Boolean, ByVal calcMiles As Boolean) As DTO.UpdateLoadPlanningResults
        'Make Call to DB. Return success boolean. Then update UI based on Success.
        Dim result As New DTO.UpdateLoadPlanningResults With {.Success = False}
        'we use a new UpdateLoadPlanningResults object to hold messages so we can use the message tools but also so we can leave the results messages blank
        'to improve performance; we do not need messages at the result and at the record level.
        'this may changes in the future?
        Dim ResultMessages As New DTO.UpdateLoadPlanningResults
        If truck Is Nothing Then
            result.AddMessage(FreightMaster.Data.DataTransferObjects.UpdateLoadPlanningResults.MessageEnum.MSGNoTruck)
            Return result
        End If
        Try
            If Not calcCost Or Not calcMiles Then
                'clear the costs or miles as needed
                If calcCost Or calcMiles Then
                    'we can run this async to improve performance
                    NGLLoadPlanningTruckData.ClearLoadPlanningTruckCostOrMilesAsync(truck.SolutionTruckCompControl, truck.SolutionTruckKey, Not calcCost, Not calcMiles)
                Else
                    'we must run this synchronous
                    NGLLoadPlanningTruckData.ClearLoadPlanningTruckCostOrMiles(truck.SolutionTruckCompControl, truck.SolutionTruckKey, Not calcCost, Not calcMiles)
                End If

            End If
            'delete any old message records
            NGLLoadPlanningTruckData.deleteLoadPlanningTruckMessagesAsync(truck.SolutionTruckCompControl, truck.SolutionTruckKey)
            Dim BookControl As Integer = 0
            'set the default bookcontrol value 
            If Not truck.SolutionDetails Is Nothing AndAlso truck.SolutionDetails.Count() <> 0 Then
                BookControl = truck.SolutionDetails(0).SolutionDetailBookControl
            End If
            Dim dblBatchID As Double = CDbl(Format(Now(), "MddyyyyHHmmss"))
            Dim BookConsPrefix As String = truck.SolutionTruckConsPrefix
            Dim BookCarrierControl As Integer = truck.SolutionTruckCarrierControl
            Dim sPCMErrors As New List(Of String)
            If calcMiles Then
                Dim oPCMReturn As PCM.clsPCMReturnEx = PCMilerBLL.PCMReSyncMultiStop(BookConsPrefix, dblBatchID, sPCMErrors, True, True, True)
                If Not oPCMReturn Is Nothing AndAlso Not oPCMReturn.Results Is Nothing Then
                    Dim oFMStops As List(Of PCM.clsFMStopData) = TryCast(oPCMReturn.Results, List(Of PCM.clsFMStopData))
                    If Not oFMStops Is Nothing AndAlso oFMStops.Count() > 0 Then
                        'use the pcmiler stop bookcontrol if provided
                        BookControl = oFMStops(0).BookControl
                        For Each oStop In oFMStops
                            With oStop
                                NGLBatchProcessData.UpdateBookConsMultiPickPCMiler(.BookControl, _
                                                                                    .LocationisOrigin, _
                                                                                    .StopNumber, _
                                                                                    .LegMiles, _
                                                                                    .LegCost, _
                                                                                    .LegTime, _
                                                                                    .LegTolls, _
                                                                                    .LegESTCHG, _
                                                                                    True)

                            End With
                        Next
                        If oPCMReturn.BadAddressCount > 0 Then
                            ResultMessages.AddMessage(FreightMaster.Data.DataTransferObjects.UpdateLoadPlanningResults.MessageEnum.MSGBadAddressCount, oPCMReturn.BadAddressCount.ToString())
                        End If
                        'Because we always keep the stop numbers we always call UpdateBookConsPickNumber
                        NGLBatchProcessData.UpdateBookConsPickNumber(BookConsPrefix, True)
                    End If
                Else
                    'clear the miles before we can calculate costs
                    NGLLoadPlanningTruckData.ClearLoadPlanningTruckCostOrMiles(truck.SolutionTruckCompControl, truck.SolutionTruckKey, False, True)
                    If Not sPCMErrors Is Nothing AndAlso sPCMErrors.Count() > 0 Then
                        For Each e In sPCMErrors
                            ResultMessages.AddMessage(e)
                        Next
                    End If
                End If
            End If
            If calcCost Then
                If calcUsingTariff Then
                    'Check for carrier and re-calculate costs
                    If BookCarrierControl > 0 Then
                        Dim oCCResults = BookRevenueBLL.AssignOrUpdateCarrier(BookControl, False)
                        Dim blnMessagesAdded As Boolean = False
                        'add any tariff messages
                        If Not oCCResults.Messages Is Nothing AndAlso oCCResults.Messages.Count() > 0 Then
                            For Each m In oCCResults.Messages
                                If Not String.IsNullOrWhiteSpace(m.Key) Then
                                    ResultMessages.AddMessage(m.Key, m.Value)
                                    blnMessagesAdded = True
                                End If
                            Next
                            If Not oCCResults.CarriersByCost Is Nothing AndAlso oCCResults.CarriersByCost.Count() > 0 Then
                                For Each c In oCCResults.CarriersByCost
                                    If Not c.Messages Is Nothing AndAlso c.Messages.Count() > 0 Then
                                        For Each m In c.Messages
                                            If Not String.IsNullOrWhiteSpace(m.Key) Then
                                                ResultMessages.AddMessage(m.Key, m.Value)
                                                blnMessagesAdded = True
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        End If
                        If Not oCCResults.Success Then
                            If Not blnMessagesAdded Then
                                'Modified by RHR v-7.0.5.100 5/17/2016
                                'Added new message MSGRecalcCostNoValidTariffWarning if the carrier has already been assigned
                                If truck.SolutionTruckCarrierControl <> 0 Then
                                    ResultMessages.AddMessage(FreightMaster.Data.DataTransferObjects.UpdateLoadPlanningResults.MessageEnum.MSGRecalcCostNoValidTariffWarning)
                                Else
                                    ResultMessages.AddMessage(FreightMaster.Data.DataTransferObjects.UpdateLoadPlanningResults.MessageEnum.MSGRecalcCostNoTariffWarning)
                                End If

                            End If
                            'clear the costs
                            NGLLoadPlanningTruckData.ClearLoadPlanningTruckCostOrMiles(truck.SolutionTruckCompControl, truck.SolutionTruckKey, True)
                        End If
                    End If
                Else
                    'recalc using existing linehaul such as spot rate.
                    BookRevenueBLL.RecalculateBookRevenueFreightCostsNoReturn(BookControl)
                End If
            End If

            'Add a sample Message
            'ResultMessages.AddMessage("MSGTest", " Sample Details ")
            'Get the data back?
            Dim lpTruck As DTO.tblSolutionTruck = NGLLoadPlanningTruckData.GetLoadPlanningTruckFiltered(truck.SolutionTruckCompControl, truck.SolutionTruckKey, False)
            If Not lpTruck Is Nothing AndAlso Not lpTruck.SolutionDetails Is Nothing AndAlso lpTruck.SolutionDetails.Count() > 0 Then
                result.Success = True
                'add the messages to the truck
                lpTruck.Messages = ResultMessages.Messages
                result.UpdatedLPTruck = lpTruck
                'save the message to the database  async
                NGLLoadPlanningTruckData.saveLoadPlanningTruckMessagesAsync(lpTruck.SolutionTruckCompControl, lpTruck.SolutionTruckKey, ResultMessages.Messages)

            Else
                result.Success = False
            End If
            Return result
        Catch ex As Exception
            Throw

        End Try
        Return result
    End Function

#End Region

#Region " Private Methods"


    Private Sub UpdateSolutionTruckDetailIndexAfterDrop(ByRef newTruck As DTO.tblSolutionTruck, ByRef droppedItem As DTO.tblSolutionDetail, ByVal droppedIndex As Integer)
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
    End Sub

#End Region

End Class
