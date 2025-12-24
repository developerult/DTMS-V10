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
Imports Models = NGL.FreightMaster.Data.Models

Imports NGL.Core.Utility

Imports PCM = NGL.FreightMaster.PCMiler
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Serilog

Public Class NGLTMS365BLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLTMS365BLL"
        me.Logger = Logger.ForContext(Of BLLBaseClass)
    End Sub

#End Region

#Region "Delegates"
    'Added by LVV for v-8.0 on 04/12/2017 TMS 365
    Public Delegate Sub Save365USATokenDelegate(ByVal sso As Models.SSOResults)
    Public Delegate Sub CreateUserGroupsForLEDelegate(ByVal LE As String, ByVal CompControl As Integer)
    Public Delegate Sub CheckFreeTrialExpirationDelegate(ByVal UserControl As Integer)
#End Region

#Region "Enum"

    Public Enum BookingActions
        None
        RemoveOrder 'P-->N 
        UnassignCarrier 'P-->N  
        Reject 'PC-->N 
        DropLoad 'PB-->N 
        SelectCarrier
        SpotRate
        Modify 'P - Carrrier Assigned
        Tender 'PC
        AcceptFinalize 'PB/finalize
        Invoice 'I
        InvoiceComplete 'IC
        'Added By LVV 8/2/16 for v-7.0.5.110 #Invoice All
        InvoiceSingle
        InvoiceCompleteSingle
        AcceptAll 'Added By LVV 11/29/18 for TMS365
    End Enum

    '***    Note: ResultProcedures, ResultTitles, ResultPrefix, and ResultSuffix were copied to 
    '               DAL.Utilities.  these remain for backward  compatibility.  all new code changes 
    '               should use the new Enum References
    Public Enum ResultProcedures
        None
        freightbill
    End Enum

    Public Enum ResultTitles
        None
        TitleSaveHistLogFailure
        TitleSaveExpectedCost
        TitleDataValidationError
        TitlePendingFeeApprovalWarning
        TitlePendingFeeApprovalError
        TitleAuditFreightBillWarning
    End Enum

    Public Enum ResultPrefix
        None
        MsgDetails
        MsgCostComparisonNotAvailable
        MsgUnexpectedFeeValidationIssue
        MsgRecalculateCostForFeeFailed
    End Enum

    Public Enum ResultSuffix
        None
        MsgDoesNotEffectProcess
        MsgCheckAppErrorLogs
        MsgUpdatedTotalCostManually
    End Enum

#End Region

#Region "Format Result Messages"

    Private dicResultProcedures As Dictionary(Of ResultProcedures, String)
    Private dicResultTitles As Dictionary(Of ResultTitles, String)
    Private dicResultPrefix As Dictionary(Of ResultPrefix, String)
    Private dicSuffixType As Dictionary(Of ResultSuffix, String)

    Private Sub addWCFMessagesToResultObj(ByRef result As Models.ResultObject, ByRef wcfRes As DTO.WCFResults, ByVal sLogTitle As String)
        If wcfRes.Errors IsNot Nothing AndAlso wcfRes.Errors.Count() > 0 Then
            For Each oErr In wcfRes.Errors
                If oErr.Value IsNot Nothing Then
                    result.Err.AddRange(oErr.Value)
                End If
                If String.IsNullOrWhiteSpace(result.ErrTitle) Then
                    result.ErrTitle = getLocalizedValueByKey(oErr.Key.ToString(), oErr.Key)
                End If
            Next
        End If

        If wcfRes.Warnings IsNot Nothing AndAlso wcfRes.Warnings.Count() > 0 Then
            For Each oWarn In wcfRes.Warnings
                If oWarn.Value IsNot Nothing Then
                    result.Warn.AddRange(oWarn.Value)
                End If
                If String.IsNullOrWhiteSpace(result.WarningTitle) Then
                    result.WarningTitle = getLocalizedValueByKey(oWarn.Key.ToString(), oWarn.Key)
                End If
            Next
        End If

        If wcfRes.Messages IsNot Nothing AndAlso wcfRes.Messages.Count() > 0 Then
            For Each oMsg In wcfRes.Messages
                If oMsg.Value IsNot Nothing Then
                    result.Warn.AddRange(oMsg.Value)
                End If
                If String.IsNullOrWhiteSpace(result.MsgTitle) Then
                    result.MsgTitle = getLocalizedValueByKey(oMsg.Key.ToString(), oMsg.Key)
                End If
            Next
        End If

        If wcfRes.Log IsNot Nothing AndAlso wcfRes.Log.Count() > 0 Then
            result.Log.AddRange(wcfRes.Log)
            result.LogTitle = sLogTitle
        End If
    End Sub

    Private Function readResultProcedure(ByVal eValue As ResultProcedures) As String
        Dim strMsg As String = "Procedure"
        If eValue = ResultProcedures.None Then Return strMsg
        If Not dicResultProcedures Is Nothing AndAlso dicResultProcedures.ContainsKey(eValue) Then
            Return dicResultProcedures(eValue)
        End If

        Try
            If dicResultProcedures Is Nothing Then dicResultProcedures = New Dictionary(Of ResultProcedures, String)
            Select Case eValue
                Case ResultProcedures.freightbill
                    strMsg = getLocalizedValueByKey("freightbill", "freight bill")
                    dicResultProcedures.Add(eValue, strMsg)
                Case Else
                    strMsg = ""
            End Select
        Catch ex As Exception
            'ignore errors on reading strings just return an empty string
        End Try
        Return strMsg

    End Function

    ''' <summary>
    ''' Return the localized value of the title based on the enum value
    ''' </summary>
    ''' <param name="eValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.007 on 04/04/2023 we now call the
    '''     NGLcmLocalizeKeyValuePairData verison of readResultTitle the 
    '''     NGLTMS365BLL version has been deprecated
    ''' </remarks>
    Private Function readResultTitle(ByVal eValue As ResultTitles) As String
        Return NGLcmLocalizeKeyValuePairData.readResultTitle(eValue)
    End Function

    Private Function readResultPrefix(ByVal eValue As ResultPrefix) As String
        If eValue = ResultPrefix.None Then Return ""
        If Not dicResultPrefix Is Nothing AndAlso dicResultPrefix.ContainsKey(eValue) Then
            Return dicResultPrefix(eValue)
        End If
        Dim strMsg As String = ""
        Try
            If dicResultPrefix Is Nothing Then dicResultPrefix = New Dictionary(Of ResultPrefix, String)
            Select Case eValue
                Case ResultPrefix.MsgDetails
                    strMsg = getLocalizedValueByKey("MsgDetails", "Details: ")
                    dicResultPrefix.Add(eValue, strMsg)
                Case ResultPrefix.MsgCostComparisonNotAvailable
                    strMsg = getLocalizedValueByKey("MsgCostComparisonNotAvailable", " Cost comparison may not be available for ")
                    dicResultPrefix.Add(eValue, strMsg)
                Case ResultPrefix.MsgUnexpectedFeeValidationIssue
                    strMsg = getLocalizedValueByKey("MsgUnexpectedFeeValidationIssue", " Unexpeced fee validation issue for ")
                    dicResultPrefix.Add(eValue, strMsg)
                Case ResultPrefix.MsgRecalculateCostForFeeFailed
                    strMsg = getLocalizedValueByKey("MsgRecalculateCostForFeeFailed", " Recalculate Costs Failed for ")
                    dicResultPrefix.Add(eValue, strMsg)
                Case Else
                    strMsg = ""
            End Select
        Catch ex As Exception
            'ignore errors on reading strings just return an empty string
        End Try
        Return strMsg

    End Function

    Private Function readResultSuffix(ByVal eValue As ResultSuffix) As String
        If eValue = ResultSuffix.None Then Return ""
        If Not dicSuffixType Is Nothing AndAlso dicSuffixType.ContainsKey(eValue) Then
            Return dicSuffixType(eValue)
        End If
        Dim strMsg As String = ""
        Try
            If dicSuffixType Is Nothing Then dicSuffixType = New Dictionary(Of ResultSuffix, String)
            Select Case eValue
                Case ResultSuffix.MsgDoesNotEffectProcess
                    strMsg = getLocalizedValueByKey("MsgDoesNotEffectProcess", " This does Not affect the systems ability To process the")
                    dicSuffixType.Add(eValue, strMsg)
                Case ResultSuffix.MsgCheckAppErrorLogs
                    strMsg = getLocalizedValueByKey("MsgCheckAppErrorLogs", " Check the application error logs for more details.")
                    dicSuffixType.Add(eValue, strMsg)
                Case ResultSuffix.MsgUpdatedTotalCostManually
                    strMsg = getLocalizedValueByKey("MsgUpdatedTotalCostManually", " Total costs should be updated manually.")
                    dicSuffixType.Add(eValue, strMsg)
                Case Else
                    strMsg = ""
            End Select
        Catch ex As Exception
            'ignore errors on reading strings just return an empty string
        End Try
        Return strMsg

    End Function

    ''' <summary>
    ''' Adds the localized messages to the result object and returns a formatted string for logs and alerts
    ''' </summary>
    ''' <param name="oResults"></param>
    ''' <param name="sDetails"></param>
    ''' <param name="eResultType"></param>
    ''' <param name="eProcedureType"></param>
    ''' <param name="eTitleType"></param>
    ''' <param name="ePrefixType"></param>
    ''' <param name="eSuffixType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for 8.2.0.117 on 8/22/19
    '''   applies new localize formats and result type logic for generating errors, warnings and positive result messages
    ''' Modified by RHR for v-8.5.3.007 on 04/04/2023 we now call the
    '''     NGLcmLocalizeKeyValuePairData verison of readResultTitle the 
    '''     NGLTMS365BLL version has been deprecated
    ''' </remarks>
    Public Function appendToResultMessage(ByRef oResults As Models.ResultObject,
                                    ByVal sDetails As String,
                                     ByVal eResultType As Models.ResultObject.ResultMsgType,
                                     ByVal eProcedureType As ResultProcedures,
                                     ByVal eTitleType As ResultTitles,
                                     ByVal ePrefixType As ResultPrefix,
                                     ByVal eSuffixType As ResultSuffix) As String
        Dim strRet As String = ""
        Try
            ' Modified by RHR for v-8.5.3.007 on 04/04/2023 we now call the
            '   NGLcmLocalizeKeyValuePairData verison of readResultTitle the 
            '   NGLTMS365BLL version has been deprecated
            Dim sTitle As String = NGLcmLocalizeKeyValuePairData.readResultTitle(eTitleType)
            Dim sMsg = String.Format(" {0} {1} {2}", readResultPrefix(ePrefixType), sDetails, readResultSuffix(eSuffixType))
            strRet = sTitle & " " & sMsg
            oResults.appendToResultMessage(eResultType, sMsg, sTitle)
            oResults.SuccessTitle = ""
            oResults.SuccessMsg = "Pending"
        Catch ex As Exception
            'do nothing on error just return an empty string the caller can determine what to do if the results are empty
        End Try
        Return strRet
    End Function

#End Region

#Region "NEXTrack Migration Methods"

#Region "All Tab"

    ''' <summary>
    ''' Updates the Carrier's load details from the All Tab
    ''' </summary>
    ''' <param name="allItem"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.1 on 6/13/2018 
    '''   added logic to allow saving changes to the Carrier Pro even  when an assigned Carrier Name is not provided
    ''' </remarks>
    Public Function AllTabSave(ByVal allItem As DTO.AllItem) As DTO.WCFResults
        Dim result As New DTO.WCFResults
        Dim blnHasWarnings As Boolean = False
        Dim p() As String
        'if they entered an assigned carrier number check to make sure it exists  
        ''Modified by RHR for v-8.1 on 6/13/2018
        If (allItem.AssignedCarrierNumber.Length > 0 OrElse allItem.AssignedCarrierName.Length > 0 OrElse allItem.AssignedProNumber.Length > 0) Then
            Try

                'save in db
                BookBLL.UpdateAssignedCarrierItems(allItem.Control,
                                 allItem.AssignedProNumber,
                                 allItem.AssignedCarrierNumber,
                                 allItem.AssignedCarrierName)
            Catch ex As Exception
                'invalid carrier number
                p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave (UpdateAssignedCarrierItems)"}
                result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllUpdateAssignedInfoFail, p)
                result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                blnHasWarnings = True
                'move on
            End Try
        End If

        Dim dbAllItem As New DTO.AllItem
        Try
            'get current dates so we can compare with what was changed
            dbAllItem.Control = allItem.Control
            dbAllItem.CarrierData = NGLBookCarrierData.GetBookCarrierFiltered(allItem.Control)
        Catch ex As Exception
            p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave (GetBookCarrierFiltered)"}
            result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllGetBookCarrierFilteredFail, p)
            result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
            blnHasWarnings = True
            'move on
        End Try

        'if they changed delivery dates to later than requied make sure that they entered load status code
        If (Not allItem.CarrierData.BookCarrActDate = Date.MinValue AndAlso Not allItem.RequestedToArrive = Date.MinValue AndAlso
            (allItem.CarrierData.BookCarrActDate > allItem.RequestedToArrive) _
            AndAlso (allItem.CarrierData.BookCarrActDate > dbAllItem.CarrierData.BookCarrActDate) _
            AndAlso allItem.Status = 1) Then '/* No Issues Reported */
            Try
                'load status code Not specified
                p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave"}
                result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllLoadStatusCodeNotSpecified, p)
                result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                blnHasWarnings = True
                'move on
            Catch
                ''load status code Not specified
                p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave"}
                result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllUpdateAssignedInfoFail, p)
                result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                blnHasWarnings = True
                'move on
            End Try
        End If

        Try
            'update dates/times in db

            'set bookCarrier equal to the current record in the database and
            'update the fields with the allItem parameter passed from the client
            Dim tempAllItem As New DTO.AllItem
            'get current dates so we can compare with what was changed
            tempAllItem.Control = allItem.Control
            tempAllItem.CarrierData = NGLBookCarrierData.GetBookCarrierFiltered(allItem.Control)

            Dim bookCarrier = tempAllItem.CarrierData

            'bookCarrier.TrackingState = Core.ChangeTracker.TrackingInfo.Updated 'TODO -- DO I DO THIS? OR WILL UpdateRecordNoReturn DO IT FOR ME?
            'bookCarrier.BookControl = allItem.Control
            'bookCarrier.BookCarrOrderNumber = allItem.OrderNumber

            Dim n = allItem.CarrierData
            'only overwrite if the value was provided by the user aka is not Date.MinValue aka null

            'Pickup information
            bookCarrier.BookCarrActualDate = If(n.BookCarrActualDate = Date.MinValue, bookCarrier.BookCarrActualDate, n.BookCarrActualDate)
            bookCarrier.BookCarrActualTime = If(n.BookCarrActualTime = Date.MinValue, bookCarrier.BookCarrActualTime, n.BookCarrActualTime)
            bookCarrier.BookCarrScheduleDate = If(n.BookCarrScheduleDate = Date.MinValue, bookCarrier.BookCarrScheduleDate, n.BookCarrScheduleDate)
            bookCarrier.BookCarrScheduleTime = If(n.BookCarrScheduleTime = Date.MinValue, bookCarrier.BookCarrScheduleTime, n.BookCarrScheduleTime)
            bookCarrier.BookCarrStartLoadingDate = If(n.BookCarrStartLoadingDate = Date.MinValue, bookCarrier.BookCarrStartLoadingDate, n.BookCarrStartLoadingDate)
            bookCarrier.BookCarrStartLoadingTime = If(n.BookCarrStartLoadingTime = Date.MinValue, bookCarrier.BookCarrStartLoadingTime, n.BookCarrStartLoadingTime)
            bookCarrier.BookCarrFinishLoadingDate = If(n.BookCarrFinishLoadingDate = Date.MinValue, bookCarrier.BookCarrFinishLoadingDate, n.BookCarrFinishLoadingDate)
            bookCarrier.BookCarrFinishLoadingTime = If(n.BookCarrFinishLoadingTime = Date.MinValue, bookCarrier.BookCarrFinishLoadingTime, n.BookCarrFinishLoadingTime)
            bookCarrier.BookCarrActLoadComplete_Date = If(n.BookCarrActLoadComplete_Date = Date.MinValue, bookCarrier.BookCarrActLoadComplete_Date, n.BookCarrActLoadComplete_Date)
            bookCarrier.BookCarrActLoadCompleteTime = If(n.BookCarrActLoadCompleteTime = Date.MinValue, bookCarrier.BookCarrActLoadCompleteTime, n.BookCarrActLoadCompleteTime)
            bookCarrier.BookCarrDockPUAssigment = If(String.IsNullOrWhiteSpace(n.BookCarrDockPUAssigment), bookCarrier.BookCarrDockPUAssigment, n.BookCarrDockPUAssigment)
            'Delivery information
            bookCarrier.BookCarrActDate = If(n.BookCarrActDate = Date.MinValue, bookCarrier.BookCarrActDate, n.BookCarrActDate)
            bookCarrier.BookCarrActTime = If(n.BookCarrActTime = Date.MinValue, bookCarrier.BookCarrActTime, n.BookCarrActTime)
            bookCarrier.BookCarrApptDate = If(n.BookCarrApptDate = Date.MinValue, bookCarrier.BookCarrApptDate, n.BookCarrApptDate)
            bookCarrier.BookCarrApptTime = If(n.BookCarrApptTime = Date.MinValue, bookCarrier.BookCarrApptTime, n.BookCarrApptTime)
            bookCarrier.BookCarrStartUnloadingDate = If(n.BookCarrStartUnloadingDate = Date.MinValue, bookCarrier.BookCarrStartUnloadingDate, n.BookCarrStartUnloadingDate)
            bookCarrier.BookCarrStartUnloadingTime = If(n.BookCarrStartUnloadingTime = Date.MinValue, bookCarrier.BookCarrStartUnloadingTime, n.BookCarrStartUnloadingTime)
            bookCarrier.BookCarrFinishUnloadingDate = If(n.BookCarrFinishUnloadingDate = Date.MinValue, bookCarrier.BookCarrFinishUnloadingDate, n.BookCarrFinishUnloadingDate)
            bookCarrier.BookCarrFinishUnloadingTime = If(n.BookCarrFinishUnloadingTime = Date.MinValue, bookCarrier.BookCarrFinishUnloadingTime, n.BookCarrFinishUnloadingTime)
            bookCarrier.BookCarrActUnloadCompDate = If(n.BookCarrActUnloadCompDate = Date.MinValue, bookCarrier.BookCarrActUnloadCompDate, n.BookCarrActUnloadCompDate)
            bookCarrier.BookCarrActUnloadCompTime = If(n.BookCarrActUnloadCompTime = Date.MinValue, bookCarrier.BookCarrActUnloadCompTime, n.BookCarrActUnloadCompTime)
            bookCarrier.BookCarrDockDelAssignment = If(String.IsNullOrWhiteSpace(n.BookCarrDockDelAssignment), bookCarrier.BookCarrDockDelAssignment, n.BookCarrDockDelAssignment)
            'Trailer information
            bookCarrier.BookCarrTrailerNo = If(String.IsNullOrWhiteSpace(n.BookCarrTrailerNo), bookCarrier.BookCarrTrailerNo, n.BookCarrTrailerNo)
            bookCarrier.BookCarrSealNo = If(String.IsNullOrWhiteSpace(n.BookCarrSealNo), bookCarrier.BookCarrSealNo, n.BookCarrSealNo)
            bookCarrier.BookCarrDriverNo = If(String.IsNullOrWhiteSpace(n.BookCarrDriverNo), bookCarrier.BookCarrDriverNo, n.BookCarrDriverNo)
            bookCarrier.BookCarrDriverName = If(String.IsNullOrWhiteSpace(n.BookCarrDriverName), bookCarrier.BookCarrDriverName, n.BookCarrDriverName)
            'Load Information
            bookCarrier.BookFinAPActWgt = If(String.IsNullOrWhiteSpace(n.BookFinAPActWgt), bookCarrier.BookFinAPActWgt, n.BookFinAPActWgt)
            bookCarrier.BookCarrBLNumber = If(String.IsNullOrWhiteSpace(n.BookCarrBLNumber), bookCarrier.BookCarrBLNumber, n.BookCarrBLNumber)
            'Warehouse information
            bookCarrier.BookWhseAuthorizationNo = If(String.IsNullOrWhiteSpace(n.BookWhseAuthorizationNo), bookCarrier.BookWhseAuthorizationNo, n.BookWhseAuthorizationNo)

            NGLBookCarrierData.UpdateRecordNoReturn(bookCarrier)

        Catch ex As Exception
            p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave (NGLBookCarrierData.UpdateRecordNoReturn)"}
            result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllUpdateAssignedInfoFail, p)
            result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
            blnHasWarnings = True
            'move on
        End Try

        Try
            'add comment for any changed dates Or times
            SetCommentsForChangedDatesAndTimes(dbAllItem, allItem, True, True)
        Catch ex As Exception
            p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave (SetCommentsForChangedDatesAndTimes)"}
            result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllSetCommentsForChangedDatesAndTimesFail, p)
            result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
            blnHasWarnings = True
            'move on
        End Try

        'Add comments for changed dates/times to all items with the same CNS 
        If (allItem.ApplyToAllPickups OrElse allItem.ApplyToAllDestinations) Then
            Try
                AddDateTimeTrackChangesToRelatedItems(allItem, allItem.ApplyToAllPickups, allItem.ApplyToAllDestinations)
            Catch
                p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave (AddDateTimeTrackChangesToRelatedItems)"}
                result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllSetCommentsRelatedItemsFail, p)
                result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                blnHasWarnings = True
                'move on
            End Try
        End If

        'get comments
        Dim comments = If(String.IsNullOrWhiteSpace(allItem.Comments), "", allItem.Comments.Trim())

        'validate
        If (Not String.IsNullOrEmpty(comments)) Then
            Try
                'update comments in db
                SetCommentsForAllItem(allItem)
            Catch ex As Exception
                p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave (SetCommentsForAllItem)"}
                result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllSetCommentsRelatedItemsFail, p)
                result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                blnHasWarnings = True
                'move on
            End Try

            'NO MORE DIRECT EMAILS --> NOW WILL BE SUBSCRIPTION ALERT
            'send comments email alert
            Try
                SendCommentsAlert(allItem, comments)
            Catch ex As Exception
                'invalid carrier number
                p = {allItem.ProNumber, "NGLTMS365BLL.AllTabSave (SendCommentsEmail)"}
                result.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.W_NTAllSendCommentsEmailFail, p)
                result.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                blnHasWarnings = True
                'move on
            End Try
        End If

        SetPickupForAllItem(allItem)
        SetDeliveryForAllItem(allItem)

        'add status message
        Dim str1 = ""
        Dim str2 = ""
        If Not String.IsNullOrWhiteSpace(allItem.CnsNumber) Then
            str1 = " with CNS "
            str2 = allItem.CnsNumber
        End If
        p = {allItem.ProNumber, str1, str2}
        If blnHasWarnings Then
            result.AddLog(DTO.WCFResults.MessageEnum.M_NTAllTabSaveWarnings, p.ToList())
        Else
            result.AddLog(DTO.WCFResults.MessageEnum.M_NTAllTabSaveSuccess, p.ToList())
        End If

        Return result

    End Function


    ''' <summary>
    ''' Adds comments with track changes related to dates/times to all book records with the same CNS And Stop number
    ''' </summary>
    ''' <param name="currentAllItem"></param>
    ''' <param name="applyPickupTrackChangesToRelatedItems"></param>
    ''' <param name="applyDeliveryTrackChangesToRelatedItems"></param>
    Public Sub AddDateTimeTrackChangesToRelatedItems(ByVal currentAllItem As DTO.AllItem, ByVal applyPickupTrackChangesToRelatedItems As Boolean, ByVal applyDeliveryTrackChangesToRelatedItems As Boolean)

        If (Not applyPickupTrackChangesToRelatedItems AndAlso Not applyDeliveryTrackChangesToRelatedItems) Then Return

        Dim currentItemControlId = currentAllItem.Control

        Dim bookControlCollection As DTO.vBookControl()

        'get set of controls of the books with the same Cns And stopNo
        If (applyDeliveryTrackChangesToRelatedItems) Then
            bookControlCollection = NGLBookData.GetGetBookControlsWithSameStopNo(currentAllItem.CnsNumber, currentAllItem.StopNumber, currentItemControlId)
        Else
            bookControlCollection = NGLBookData.GetGetBookControlsWithSameCNS(currentAllItem.CnsNumber, currentItemControlId)
        End If

        For Each bc In bookControlCollection

            'create New item And get current dates so we can compare with what was changed
            Dim relatedAllItem As New DTO.AllItem
            'AllItem.GetDates(bc.BookControl);
            relatedAllItem.Control = bc.BookControl
            relatedAllItem.CarrierData = NGLBookCarrierData.GetBookCarrierFiltered(bc.BookControl)

            'set control
            currentAllItem.Control = bc.BookControl
            'add comment for any changed dates Or times
            SetCommentsForChangedDatesAndTimes(relatedAllItem, currentAllItem, applyPickupTrackChangesToRelatedItems, applyDeliveryTrackChangesToRelatedItems)
        Next

        'restore control identifier
        currentAllItem.Control = currentItemControlId
    End Sub

    Public Sub SetCommentsForChangedDatesAndTimes(ByVal oldAllItem As DTO.AllItem, ByVal newAllItem As DTO.AllItem, ByVal trackPickupChanges As Boolean, ByVal trackDeliveryChanges As Boolean)

        'create string to hold changeds
        Dim changed = ""

        Dim pickup = If(trackPickupChanges, "PICKUP Info: " + Environment.NewLine, "")
        Dim delivery = If(trackDeliveryChanges, "DELIVERY Info: " + Environment.NewLine, "")

        Dim aOld = oldAllItem.CarrierData
        Dim aNew = newAllItem.CarrierData

        If (trackPickupChanges) Then
            'compare this (current item) with changed item (passed in)
            If ((Not compareNullableShortDate(aOld.BookCarrScheduleDate, aNew.BookCarrScheduleDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrScheduleTime, aNew.BookCarrScheduleTime))) Then
                changed += pickup + " APPT [from " + formatDateString(aOld.BookCarrScheduleDate) + " " + formatTimeString(aOld.BookCarrScheduleTime) + " to " + formatDateString(aNew.BookCarrScheduleDate) + " " + formatTimeString(aNew.BookCarrScheduleTime) + "]" + Environment.NewLine
                pickup = ""
            End If

            If ((Not compareNullableShortDate(aOld.BookCarrActualDate, aNew.BookCarrActualDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrActualTime, aNew.BookCarrActualTime))) Then
                changed += pickup + " CHK IN [from " + formatDateString(aOld.BookCarrActualDate) + " " + formatTimeString(aOld.BookCarrActualTime) + " to " + formatDateString(aNew.BookCarrActualDate) + " " + formatTimeString(aNew.BookCarrActualTime) + "]" + Environment.NewLine
                pickup = ""
            End If

            If ((Not compareNullableShortDate(aOld.BookCarrStartLoadingDate, aNew.BookCarrStartLoadingDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrStartLoadingTime, aNew.BookCarrStartLoadingTime))) Then
                changed += pickup + " STRT [from " + formatDateString(aOld.BookCarrStartLoadingDate) + " " + formatTimeString(aOld.BookCarrStartLoadingTime) + " to " + formatDateString(aNew.BookCarrStartLoadingDate) + " " + formatTimeString(aNew.BookCarrStartLoadingTime) + "]" + Environment.NewLine
                pickup = ""
            End If

            If ((Not compareNullableShortDate(aOld.BookCarrFinishLoadingDate, aNew.BookCarrFinishLoadingDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrFinishLoadingTime, aNew.BookCarrFinishLoadingTime))) Then
                changed += pickup + " FNSH [from " + formatDateString(aOld.BookCarrFinishLoadingDate) + " " + formatTimeString(aOld.BookCarrFinishLoadingTime) + " to " + formatDateString(aNew.BookCarrFinishLoadingDate) + " " + formatTimeString(aNew.BookCarrFinishLoadingTime) + "]" + Environment.NewLine
                pickup = ""
            End If

            If ((Not compareNullableShortDate(aOld.BookCarrActLoadComplete_Date, aNew.BookCarrActLoadComplete_Date)) OrElse (Not compareNullableShortTime(aOld.BookCarrActLoadCompleteTime, aNew.BookCarrActLoadCompleteTime))) Then
                changed += pickup + " CHK Out [from " + formatDateString(aOld.BookCarrActLoadComplete_Date) + " " + formatTimeString(aOld.BookCarrActLoadCompleteTime) + " to " + formatDateString(aNew.BookCarrActLoadComplete_Date) + " " + formatTimeString(aNew.BookCarrActLoadCompleteTime) + "]" + Environment.NewLine
                pickup = ""
            End If
        End If

        If (trackDeliveryChanges) Then

            If ((Not compareNullableShortDate(aOld.BookCarrApptDate, aNew.BookCarrApptDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrApptTime, aNew.BookCarrApptTime))) Then
                changed += (If((changed.Length > 0), " ", "")) + delivery + " APPT [from " + formatDateString(aOld.BookCarrApptDate) + " " + formatTimeString(aOld.BookCarrApptTime) + " to " + formatDateString(aNew.BookCarrApptDate) + " " + formatTimeString(aNew.BookCarrApptTime) + "]" + Environment.NewLine
                delivery = ""
            End If

            If ((Not compareNullableShortDate(aOld.BookCarrActDate, aNew.BookCarrActDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrActTime, aNew.BookCarrActTime))) Then
                changed += (If((changed.Length > 0 AndAlso delivery.Length = 0), " ", "")) + delivery + " CHK IN [from " + formatDateString(aOld.BookCarrActDate) + " " + formatTimeString(aOld.BookCarrActTime) + " to " + formatDateString(aNew.BookCarrActDate) + " " + formatTimeString(aNew.BookCarrActTime) + "]" + Environment.NewLine
                delivery = ""
            End If

            If ((Not compareNullableShortDate(aOld.BookCarrStartUnloadingDate, aNew.BookCarrStartUnloadingDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrStartUnloadingTime, aNew.BookCarrStartUnloadingTime))) Then
                changed += (If((changed.Length > 0 AndAlso delivery.Length = 0), " ", "")) + delivery + " STRT [from " + formatDateString(aOld.BookCarrStartUnloadingDate) + " " + formatTimeString(aOld.BookCarrStartUnloadingTime) + " to " + formatDateString(aNew.BookCarrStartUnloadingDate) + " " + formatTimeString(aNew.BookCarrStartUnloadingTime) + "]" + Environment.NewLine
                delivery = String.Empty
            End If

            If ((Not compareNullableShortDate(aOld.BookCarrFinishUnloadingDate, aNew.BookCarrFinishUnloadingDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrFinishUnloadingTime, aNew.BookCarrFinishUnloadingTime))) Then
                changed += (If((changed.Length > 0 AndAlso delivery.Length = 0), " ", "")) + delivery + " FNSH [from " + formatDateString(aOld.BookCarrFinishUnloadingDate) + " " + formatTimeString(aOld.BookCarrFinishUnloadingTime) + " to " + formatDateString(aNew.BookCarrFinishUnloadingDate) + " " + formatTimeString(aNew.BookCarrFinishUnloadingTime) + "]" + Environment.NewLine
                delivery = ""
            End If

            If ((Not compareNullableShortDate(aOld.BookCarrActUnloadCompDate, aNew.BookCarrActUnloadCompDate)) OrElse (Not compareNullableShortTime(aOld.BookCarrActUnloadCompTime, aNew.BookCarrActUnloadCompTime))) Then
                changed += (If((changed.Length > 0 AndAlso delivery.Length = 0), " ", "")) + delivery + " CHK Out [from " + formatDateString(aOld.BookCarrActUnloadCompDate) + " " + formatTimeString(aOld.BookCarrActUnloadCompTime) + " to " + formatDateString(aNew.BookCarrActUnloadCompDate) + " " + formatTimeString(aNew.BookCarrActUnloadCompTime) + "]" + Environment.NewLine
            End If
        End If

        'check changed
        If (changed.Length > 0) Then

            'prepend title
            changed = "NEXTrack Changes " + changed

            SetChangedDateComments(newAllItem.Control, changed, newAllItem)
        End If
    End Sub

    Private Sub SetChangedDateComments(ByVal control As Integer, ByVal changed As String, ByVal allItem As DTO.AllItem)
        'if carrier then contact Is assigned carrier - otherwise if regular user set contact as user name
        Dim contact = ""

        If (Not Parameters.IsUserCarrier) Then
            'If the user is not a carrier
            contact = allItem.AssignedCarrier
        Else
            'if the user is a carrier
            contact = Parameters.UserName
        End If

        'create New instance of BookTrack object
        Dim bookTrack = New DTO.BookTrack
        With bookTrack
            .BookTrackBookControl = allItem.Control
            .BookTrackDate = Date.Now
            .BookTrackContact = CheckAndClear(contact)
            .BookTrackComment = CheckAndClear(changed)
            .BookTrackModUser = Parameters.UserName
            .BookTrackModDate = Date.Now
            .BookTrackStatus = allItem.Status
        End With

        Dim result = NGLBookTrackData.CreateRecord(bookTrack)  'we should use AddNewRecord instead Of SaveItem,When Try To save New record
    End Sub

    ''Public Sub SetCommentsForAllItem(ByVal allItem As DTO.AllItem)
    ''    Dim Contact = ""
    ''    'if carrier then contact Is assigned carrier - otherwise if regular user set contact as user name
    ''    If (Not Parameters.IsUserCarrier) Then
    ''        Contact = allItem.AssignedCarrier
    ''    Else
    ''        Contact = Parameters.UserName
    ''    End If
    ''    If (allItem.ApplyCommentsToCNS) Then
    ''        BookBLL.UpdateBookTracksByCNS(allItem.Control, CheckAndClear(Contact), CheckAndClear(allItem.Comments), allItem.Status.ToString())
    ''    Else
    ''        'Updatting single record
    ''        Dim bookTrack = New DTO.BookTrack
    ''        With bookTrack
    ''            .BookTrackBookControl = allItem.Control
    ''            .BookTrackDate = Date.Now  'In Case we a Not provide this parameter we will have a null walue In BookTrackDate field Of the [BookTrack] table
    ''            .BookTrackContact = CheckAndClear(Contact)
    ''            .BookTrackComment = CheckAndClear(allItem.Comments)  'Truncate(Helper.CheckAndClear(allItem.Comments), 255),
    ''            .BookTrackModUser = Truncate(Parameters.UserName, 25)
    ''            .BookTrackModDate = Date.Now  'it will be provided by wcf
    ''            .BookTrackStatus = allItem.Status
    ''        End With
    ''        Dim record = NGLBookTrackData.CreateRecord(bookTrack)  'we should use AddNewRecord instead Of SaveItem,When Try To save New record
    ''    End If
    ''End Sub

    Public Sub SetCommentsForAllItem(ByVal allItem As DTO.AllItem)
        Dim bookTrack = getBookTrackFromAllItem(allItem)
        If (allItem.ApplyCommentsToCNS) Then
            'UpdateBookTracksByCNS
            Dim oData As New List(Of DTO.BookTrack)
            Dim bookControls = NGLBookRevenueData.GetDependentBookControls(allItem.Control, True)
            For Each b In bookControls
                Dim bt As New DTO.BookTrack()
                bt = bookTrack.Clone()
                bt.BookTrackBookControl = b.BookControl
                oData.Add(bt)
            Next
            NGLBookTrackData.InsertBookTracksWithDetails(oData.ToArray())
        Else
            'Updatting single record           
            NGLBookTrackData.InsertBookTrackWithDetails(bookTrack)
        End If
    End Sub

    Public Function getBookTrackFromAllItem(ByVal allItem As DTO.AllItem) As DTO.BookTrack
        Dim bookTrack As New DTO.BookTrack()
        Dim strContact = ""
        If (Parameters.IsUserCarrier) Then strContact = CheckAndClear(allItem.AssignedCarrier) Else strContact = CheckAndClear(Parameters.UserName) 'if carrier then contact is assigned carrier - otherwise if regular user set contact as user name
        Dim strComment As String = Truncate(CheckAndClear(allItem.Comments), 255)
        With bookTrack
            .BookTrackBookControl = allItem.Control
            .BookTrackComment = strComment
            .BookTrackContact = strContact
            .BookTrackDate = Date.Now
            .BookTrackStatus = allItem.Status
        End With
        Dim stp = allItem.getCommentLocation()
        If Not stp Is Nothing AndAlso stp.HasData() Then
            Dim bookTrackDetail As New DTO.BookTrackDetail()
            bookTrackDetail.setStop(stp)
            bookTrackDetail.setLinkStopRecord(True)
            Dim commentDate = allItem.getCommentDate()
            Dim commentTime = allItem.getCommentTime()
            If commentDate.HasValue Then bookTrackDetail.BookTrackDetailAT705 = commentDate.Value.ToString("MM-dd-yyyy")
            If commentTime.HasValue Then bookTrackDetail.BookTrackDetailAT706 = commentTime.Value.ToString("HH:mm")
            If commentDate.HasValue OrElse commentTime.HasValue Then bookTrackDetail.BookTrackDetailAT707 = "LT" 'This is always LT according to 214 documentation
            bookTrackDetail.BookTrackDetailAT701 = "X6" 'NOTE: FOR NOW THIS IS ALWAYS X6 BUT PROBABLY IN FUTURE WILL ALLOW VARIOUS CODES (TODO)
            bookTrack.addDetail(bookTrackDetail)
        End If
        Return bookTrack
    End Function

    Public Sub SendCommentsAlert(ByVal allItem As DTO.AllItem, ByVal comments As String)
        'get subject
        Dim subject = If((allItem.ApplyCommentsToCNS), "Carrier Comments For All Stops Added To " + allItem.ProNumber, "Carrier Comments Added To " + allItem.ProNumber)
        Dim sInfo = ""
        Dim sSep = ""
        If Not String.IsNullOrWhiteSpace(allItem.AssignedCarrier) Then
            sInfo += sSep + "<strong>Carrier</strong>: " + allItem.AssignedCarrier
            sSep = ", "
        End If
        If Not String.IsNullOrWhiteSpace(allItem.SHID) Then
            sInfo += sSep + "<strong>SHID</strong>: " + allItem.SHID
            sSep = ", "
        End If
        If Not String.IsNullOrWhiteSpace(allItem.CnsNumber) Then
            sInfo += sSep + "<strong>CNS</strong>: " + allItem.CnsNumber
            sSep = ", "
        End If
        If Not String.IsNullOrWhiteSpace(allItem.OrderNumber) Then
            sInfo += sSep + "<strong>Order Number</strong>: " + allItem.OrderNumber
            sSep = ", "
        End If
        If Not String.IsNullOrWhiteSpace(allItem.PurchaseOrderNumber) Then
            sInfo += sSep + "<strong>PO</strong>: " + allItem.PurchaseOrderNumber
            sSep = ", "
        End If
        If Not String.IsNullOrWhiteSpace(allItem.DestinationName) Then
            sInfo += sSep + "<strong>Ship To</strong>: " + allItem.DestinationName
            sSep = ", "
        End If
        'build body
        Dim body = "<h4>" + subject + "</h4>" + "Comments:" + "<br/>" + comments + "<br/><br/>" + sInfo
        '** TODO ** Do we need the company/carrier controls for alerts? They are optional parameters but I forgot what they are for when populated
        NGLtblAlertMessageData.InsertAlertMessage("AlertCarrierLoadStatusComment", "Alert when carrier enters comments in NEXTrack or via EDI", subject, body)
    End Sub

    Private Sub SetPickupForAllItem(ByVal allItem As DTO.AllItem)

        If (Not allItem.ApplyToAllPickups) Then Return '//alg:>>I don't know why i have to do in such way,but this logic was programmed in SqlServerDataProvider.SetDeliveryForAllItem

        Dim a = allItem.CarrierData

        BookBLL.UpdateCarrDataPickupByCNS(allItem.Control,
                                          CheckAndClear(allItem.CnsNumber),
                                          CheckAndClear(allItem.BookPickupStopNumber),
                                          a.BookCarrScheduleDate,
                                          a.BookCarrScheduleTime,
                                          a.BookCarrActualDate,
                                          a.BookCarrActualTime,
                                          a.BookCarrStartLoadingDate,
                                          a.BookCarrStartLoadingTime,
                                          a.BookCarrFinishLoadingDate,
                                          a.BookCarrFinishLoadingTime,
                                          a.BookCarrActLoadComplete_Date,
                                          a.BookCarrActLoadCompleteTime,
                                          CheckAndClear(a.BookCarrDockPUAssigment),
                                          CheckAndClear(a.BookWhseAuthorizationNo))

    End Sub

    Private Sub SetDeliveryForAllItem(ByVal allItem As DTO.AllItem)
        If (Not allItem.ApplyToAllDestinations) Then Return 'I don't know why i have to do in such way,but this logic was programmed in SqlServerDataProvider.SetDeliveryForAllItem

        Dim a = allItem.CarrierData
        BookBLL.UpdateCarrDataDeliveryByCNS(allItem.Control,
                                            CheckAndClear(allItem.CnsNumber),
                                            CheckAndClear(allItem.StopNumber),
                                            a.BookCarrApptDate,
                                            a.BookCarrApptTime,
                                            a.BookCarrActDate,
                                            a.BookCarrActTime,
                                            a.BookCarrStartUnloadingDate,
                                            a.BookCarrStartUnloadingTime,
                                            a.BookCarrFinishUnloadingDate,
                                            a.BookCarrFinishUnloadingTime,
                                            a.BookCarrActUnloadCompDate,
                                            a.BookCarrActUnloadCompTime,
                                            CheckAndClear(a.BookCarrDockDelAssignment),
                                            CheckAndClear(a.BookWhseAuthorizationNo))

    End Sub

    Private Function Truncate(ByVal value As String, ByVal length As Integer) As String
        If (String.IsNullOrEmpty(value)) Then Return ""
        If (value.Length <= length) Then Return value
        Return value.Substring(0, length)
    End Function

    Friend Shared Function CheckAndClear(value As String) As String
        Dim result As String = If(String.IsNullOrEmpty(value), Nothing, value.Trim())
        Return result
    End Function

    ''' <summary>
    ''' Compares the Date part of a nullable DateTime
    ''' </summary>
    ''' <param name="dtA"></param>
    ''' <param name="dtB"></param>
    ''' <returns></returns>
    Private Function compareNullableShortDate(ByVal dtA As Date?, ByVal dtB As Date?) As Boolean
        Dim blnRet = False
        Dim strA = ""
        Dim strB = ""

        If dtA = Date.MinValue Then
            strA = ""
        Else
            strA = String.Format("{0:d}", dtA)
        End If
        If dtB = Date.MinValue Then
            strB = ""
        Else
            strB = String.Format("{0:d}", dtB)
        End If

        If strA = strB Then blnRet = True

        Return blnRet
    End Function

    ''' <summary>
    ''' Compares the Time part of a nullable DateTime
    ''' </summary>
    ''' <param name="dtA"></param>
    ''' <param name="dtB"></param>
    ''' <returns></returns>
    Private Function compareNullableShortTime(ByVal dtA As Date?, ByVal dtB As Date?) As Boolean
        Dim blnRet = False
        Dim strA = ""
        Dim strB = ""

        If dtA = Date.MinValue Then
            strA = ""
        Else
            strA = String.Format("{0:HH:mm}", dtA)
        End If
        If dtB = Date.MinValue Then
            strB = ""
        Else
            strB = String.Format("{0:HH:mm}", dtB)
        End If

        If strA = strB Then blnRet = True

        Return blnRet
    End Function

    Private Function isNullDateTime(value As String, defaultValue As String) As String
        If String.IsNullOrWhiteSpace(value) Then
            Return defaultValue
        Else
            If value = "1/1/0001" Then
                Return defaultValue
            Else
                Return value
            End If
        End If
    End Function

    Private Function formatDateString(ByVal dt As Date?) As String
        Return isNullDateTime(String.Format("{0:d}", dt), "-")
    End Function

    Private Function formatTimeString(ByVal dt As Date?) As String
        Return isNullDateTime(String.Format("{0:HH:mm:tt}", dt), "-")
    End Function

    Private Function BuildXmlParameter(ByVal value As String) As String
        Return "<row id=""" + value + """></row>"
    End Function

    Private Function getSortFieldKey(ByVal fieldName As String) As String
        Dim strRet = ""

        'Which ones are these??
        'LOADToLoadDate
        'LOADScheduleTime

        Select Case fieldName
            Case "CnsNumber"
                strRet = "LOADCONSPREFIX"
            Case "SHID"
                strRet = "BookSHID"
            Case "ProNumber"
                strRet = "LOADPRONUMBER"
            Case "PurchaseOrderNumber"
                strRet = "LOADPO"
            Case "OrderNumber"
                strRet = "LOADORDERNUMBER"
            Case "ScheduledToLoad"
                strRet = "LOADDATE" '?
            Case "RequestedToArrive"
                strRet = "SCHEDULEDATE" '?
            Case "BookCarrScheduleDate"
                strRet = ""
            Case "BookCarrScheduleTime"
                strRet = ""
            Case "BookCarrActualDate"
                strRet = ""
            Case "BookCarrActualTime"
                strRet = ""
            Case "AssignedCarrier"
                strRet = "LOADCARRIERNAME"
            Case "DestinationName"
                strRet = "LOADDESTNAME"
            Case "DestinationCity"
                strRet = "LOADDESTCITY"
            Case "DestinationState"
                strRet = "LOADDESTSTATE"
            Case Else
                strRet = ""
        End Select

        Return strRet
    End Function

    ''' <summary>
    ''' NOTE: I am pretty sure this is depreciated - I think we only call BookdataProvider.NGLAllItemData.GetAllItemsFiltered365()
    ''' Returns an array of all tab records filtered by page size
    ''' </summary>
    ''' <param name="f"></param>
    ''' <param name="UserControl"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.0 on 01/24/2018
    ''' </remarks>
    Public Function GetAllItemsFiltered365(ByVal f As Models.AllFilters, ByVal UserControl As Integer, ByVal page As Integer, ByVal pagesize As Integer) As LTS.spNGLLOAD_SORTEDWPages_New7052Result()
        Dim oAll = New DAL.NGLAllItemData(Parameters)
        Dim oUSC = New DAL.NGLUserSecurityCarrierData(Parameters)

        Dim proNumber As String = Nothing
        Dim CNS As String = Nothing
        Dim PO As String = Nothing
        Dim OrderNumber As String = Nothing
        Dim LoadDate As Date? = Nothing
        Dim LoadDateTo As Date? = Nothing
        Dim SCHEDULEDATE As Date? = Nothing
        Dim SCHEDULEDATETo As Date? = Nothing
        Dim LOADDelScheduleDate As Date? = Nothing
        Dim LOADDelScheduleDateTo As Date? = Nothing
        Dim LOADDelScheduleTime As String = Nothing
        Dim LOADActDelDate As Date? = Nothing
        Dim LOADActDelDateTo As Date? = Nothing
        Dim LOADCARRIERNAME As String = Nothing
        Dim LOADCARRIERNUMBER As Integer? = Nothing
        Dim LOADDESTNAME As String = Nothing
        Dim LOADDESTCITY As String = Nothing
        Dim LOADDESTSTATE As String = Nothing
        Dim LOADBROKERNUMBER As String = Nothing
        Dim LOADCARRIERCONTCONTROL As Integer? = Nothing
        Dim UseCarrierFilters As Boolean? = Nothing
        Dim DaysOut As Integer? = Nothing
        Dim DelDaysOut As Integer? = Nothing
        Dim xmlTransCode As String = Nothing
        Dim xmlLoadCompanyIDs As String = Nothing
        Dim xmlLoadLanes As String = Nothing
        Dim p_sortordinal As String = "LOADCONSPREFIX"
        Dim p_sortdirection As String = "DESCENDING"
        Dim p_datefilterfield As String = String.Empty
        Dim p_datefilterfrom As Date? = Nothing
        Dim p_datefilterto As Date? = Nothing
        Dim bookSHID As String = Nothing
        Dim bookShipCarrierProNumber As String = Nothing
        Dim fromDate As DateTime = DateTime.MinValue
        Dim toDate As DateTime = DateTime.MaxValue


        '******************************************************        

        'See if the user is a Carrier
        Dim carrier As LTS.tblUserSecurityCarrier = Nothing
        'Modified By LVV on 10/15/2018 -- we already have this information we do not need to look it up again
        ''Dim ucat = NGLSecurityData.GetCategoryForUser(UserControl)
        ''If ucat = 2 Then
        ''    carrier = oUSC.GetRecord(UserControl)
        ''End If
        If Parameters.CatControl = 2 Then
            carrier = oUSC.GetFirstRecordByUser(UserControl)
        End If

        'validate
        If (Not carrier Is Nothing) Then
            'set up xml variable
            Dim Xml = String.Empty

            'check if carrier allows web tender And build xml
            If (NGLCarrierData.GetCarrierAllowWebTender(carrier.USCCarrierControl)) Then
                Xml = "<Root>" + BuildXmlParameter("N") + BuildXmlParameter("P") + BuildXmlParameter("PC") + "</Root>"
            Else
                Xml = "<Root>" + BuildXmlParameter("N") + BuildXmlParameter("P") + "</Root>"
            End If

            'add parameter for xml
            xmlTransCode = Xml

            'add parameter to use carrier filters
            UseCarrierFilters = True

            'add parameter for carrier number
            LOADCARRIERNUMBER = carrier.USCCarrierNumber

            'check carrier contact
            If (carrier.USCCarrierContControl > 0) Then
                'add parameter
                LOADCARRIERCONTCONTROL = carrier.USCCarrierContControl
            End If

        End If



        'get company id's
        '** TODO ** Figure out what is a company id?? Is it the control, the name, the number??
        Dim companyIds = NGLSecurityData.GettblUserAdminsByUser(UserControl)

        'check company id's
        If (companyIds?.Length > 0) Then
            'set up xml variable
            Dim Xml = String.Empty

            'open segment
            Xml += "<Root>"

            'loop
            For Each c In companyIds
                Xml += BuildXmlParameter(c.UserAdminCompControl.ToString())
            Next

            'close segment
            Xml += "</Root>"

            'add parameter
            xmlLoadCompanyIDs = Xml
        End If


        Dim usettings = NGLSecurityData.GetUserSecuritySetting(UserControl)
        If Not usettings Is Nothing Then
            'get restricted lanes
            Dim restrictedLanes = NGLSecurityData.GetUserSecurityLanes(UserControl)
            'check restricted lanes
            If (usettings.USSRestrictLanes = "Y") Then
                '    {
                If (restrictedLanes?.Count > 0) Then
                    'set up xml variable
                    Dim Xml = String.Empty

                    'open segment
                    Xml += "<Root>"

                    'Loop Name
                    For Each restrictedLane In restrictedLanes
                        Xml += BuildXmlParameter(restrictedLane.LaneName)
                    Next

                    'close segment
                    Xml += "</Root>"

                    'add parameter
                    xmlLoadLanes = Xml
                End If
            End If

            'check broker number
            If (Not String.IsNullOrWhiteSpace(usettings.USSBrokerNumber)) Then
                LOADBROKERNUMBER = usettings.USSBrokerNumber.Trim()
            End If
        End If

        '******************************************************

        'add filtering if necessary
        If (Not f Is Nothing) Then

            'if they're filtering by one pro number or one cns number then pass parameter to make stored proc disregard the company
            'setting for days back to search
            'If (f.filterName = "LOADCONSPREFIX" OrElse f.filterName = "LOADPRONUMBER" OrElse f.filterName = "LOADORDERNUMBER" OrElse f.filterName = "LOADPO") Then
            'Modified by RHR for v-8.0 on 01/24/2018
            If (f.filterName = "CnsNumber" OrElse f.filterName = "SHID" OrElse f.filterName = "ProNumber" OrElse f.filterName = "OrderNumber" OrElse f.filterName = "PurchaseOrderNumber") Then
                'check length of item - only do this if a full cns or pro was typed in
                If (f.filterValue.Length >= 5) Then
                    DaysOut = 10000
                    DelDaysOut = 10000
                End If
            End If

            'Check if person filtered by carrier number in case the current user Is a carrier.  if that's the case then disregard the filter
            Dim skipFilter = False
            If (f.filterName = "LOADCARRIERNUMBER") Then
                If (LOADCARRIERNUMBER > 0) Then
                    skipFilter = True
                End If
            End If

            'check skip
            If (Not skipFilter) Then
                Select Case (f.filterName)
                    Case "CnsNumber" '"LOADCONSPREFIX" 'CNS Number
                        CNS = f.filterValue
                    Case "ProNumber" '"LOADPRONUMBER" 'Pro Number
                        proNumber = f.filterValue
                    Case "PurchaseOrderNumber" '"LOADPO" 'PO Number
                        PO = f.filterValue
                    Case "OrderNumber" '"LOADORDERNUMBER" 'Order Number
                        OrderNumber = f.filterValue
                    Case "AssignedCarrier" '"LOADCARRIERNAME" 'Assigned vCarrier
                        LOADCARRIERNAME = f.filterValue
                    Case "LOADCARRIERNUMBER" 'vCarrier Number
                        Dim intLOADCARRIERNUMBER As Integer = 0
                        Integer.TryParse(f.filterValue, intLOADCARRIERNUMBER)
                        LOADCARRIERNUMBER = intLOADCARRIERNUMBER
                    Case "DestinationName" '"LOADDESTNAME" 'Destination
                        LOADDESTNAME = f.filterValue
                    Case "DestinationCity" '"LOADDESTCITY" 'City
                        LOADDESTCITY = f.filterValue
                    Case "DestinationState" '"LOADDESTSTATE" 'State
                        LOADDESTSTATE = f.filterValue
                    Case "SHID" '"BookSHID" 'SHID
                        bookSHID = f.filterValue
                    Case "BookShipCarrierProNumber" 'CarrierPro
                        bookShipCarrierProNumber = f.filterValue
                    Case "LOADDATE"
                        fromDate = If(f.filterFrom.HasValue, f.filterFrom.Value, Date.MinValue)
                        toDate = If(f.filterTo.HasValue, f.filterTo.Value, Date.MaxValue)
                        p_datefilterfield = f.filterValue
                        p_datefilterfrom = fromDate
                        p_datefilterto = toDate
                        LoadDate = fromDate
                        LoadDateTo = toDate
                    Case "ScheduledToLoad"  'Modified by RHR for v-8.0 on 01/24/2018
                        fromDate = If(f.filterFrom.HasValue, f.filterFrom.Value, Date.MinValue)
                        toDate = If(f.filterTo.HasValue, f.filterTo.Value, Date.MaxValue)
                        p_datefilterfield = f.filterValue
                        p_datefilterfrom = fromDate
                        p_datefilterto = toDate
                        LoadDate = fromDate
                        LoadDateTo = toDate
                    Case "SCHEDULEDATE"
                        fromDate = If(f.filterFrom.HasValue, f.filterFrom.Value, Date.MinValue)
                        toDate = If(f.filterTo.HasValue, f.filterTo.Value, Date.MaxValue)
                        p_datefilterfield = f.filterValue
                        p_datefilterfrom = fromDate
                        p_datefilterto = toDate
                        SCHEDULEDATE = fromDate
                        SCHEDULEDATETo = toDate
                    Case Else
                        'Do Nothing
                End Select
            End If

            'add sorting if necessary
            If (Not String.IsNullOrWhiteSpace(f.sortName)) Then
                'create sort parameters
                Dim sdir = ""
                Select Case f.sortDirection
                    Case "asc"
                        sdir = "Ascending"
                    Case "desc"
                        sdir = "Descending"
                    Case Else
                        sdir = "Ascending"
                End Select

                p_sortordinal = getSortFieldKey(f.sortName)
                p_sortdirection = sdir
            End If

        End If


        '******************************************************

        Dim spRes = oAll.GetAllItems365(proNumber,
                                                    CNS,
                                                    PO,
                                                    OrderNumber,
                                                    bookSHID,
                                                    bookShipCarrierProNumber,
                                                    LoadDate,
                                                    LoadDateTo,
                                                    SCHEDULEDATE,
                                                    SCHEDULEDATETo,
                                                    LOADDelScheduleDate,
                                                    LOADDelScheduleDateTo,
                                                    LOADDelScheduleTime,
                                                    LOADActDelDate,
                                                    LOADActDelDateTo,
                                                    LOADCARRIERNAME,
                                                    LOADCARRIERNUMBER,
                                                    LOADDESTNAME,
                                                    LOADDESTCITY,
                                                    LOADDESTSTATE,
                                                    LOADBROKERNUMBER,
                                                    LOADCARRIERCONTCONTROL,
                                                    UseCarrierFilters,
                                                    DaysOut,
                                                    DelDaysOut,
                                                    xmlTransCode,
                                                    xmlLoadCompanyIDs,
                                                    xmlLoadLanes,
                                                    p_sortordinal,
                                                    p_sortdirection,
                                                    p_datefilterfield,
                                                    p_datefilterfrom,
                                                    p_datefilterto,
                                                    page,
                                                    pagesize)
        Return spRes
    End Function

#End Region

#Region "Settlement Tab"

    Public Function GetSettlementItems365(ByRef RecordCount As Integer, ByVal f As Models.AllFilters) As DTO.SettlementItem()
        Dim settlementItems As DTO.SettlementItem()

        Dim CarrierControl As Integer? = Nothing
        Dim CarrierContControl As Integer? = Nothing
        Dim proNumber As String = Nothing
        Dim cns As String = Nothing
        Dim bookCarrOrderNumber As String = Nothing
        Dim bookDateDelDate As Date? = Nothing
        Dim bookDateDelDateTo As Date? = Nothing
        Dim bookPayCode As String = Nothing
        Dim bookRevTotalCost As Decimal? = Nothing
        Dim bookFinAPBillNumber As String = Nothing
        Dim bookFinAPActCost As Decimal? = Nothing
        Dim sortOrdinal = "CNS Number"  'default value
        Dim sortDirection = "Descending"    'default value
        Dim datefilterfield = ""
        Dim dateFilterFrom As Date? = Nothing
        Dim dateFilterTo As Date? = Nothing
        Dim bookSHID As String = Nothing
        Dim bookShipCarrierProNumber As String = Nothing
        Dim carrierName As String = Nothing

        If Parameters.UserCarrierControl > 0 Then CarrierControl = Parameters.UserCarrierControl
        If Parameters.UserCarrierContControl > 0 Then CarrierContControl = Parameters.UserCarrierContControl

        'add filtering if necessary
        If (Not f Is Nothing) Then
            Select Case (f.filterName)
                Case "CnsNumber" ': //BookConsPrefix
                    cns = f.filterValue
                Case "ProNumber" ':   //BookProNumber 
                    proNumber = f.filterValue
                Case "OrderNumber" ':  //BookCarrOrderNumber
                    bookCarrOrderNumber = f.filterValue
                Case "Status" ':  //BookPayCode
                    Select Case (f.filterValue.ToUpper)
                        Case "N"
                            bookPayCode = "N"
                        Case "PENDING"
                            bookPayCode = "PA"
                        Case "FAILED"
                            bookPayCode = "M"
                    End Select
                Case "SHID" ':  //BookSHID
                    bookSHID = f.filterValue
                Case "InvoiceNumber" ':  //BookFinAPBillNumber
                    bookFinAPBillNumber = f.filterValue
                Case "CarrierPro" ':  //BookShipCarrierProNumber
                    bookShipCarrierProNumber = f.filterValue
                Case "DeliveredDate" 'BookDateDelivered
                    Dim fromDate = If(f.filterFrom.HasValue, f.filterFrom.Value, Date.MinValue)
                    Dim toDate = If(f.filterTo.HasValue, f.filterTo.Value, Date.MaxValue)
                    'datefilterfield = f.filterName
                    datefilterfield = "BookDateDelivered"
                    dateFilterFrom = fromDate
                    dateFilterTo = toDate
                Case "CarrierName"  'Carrier Name
                    carrierName = f.filterValue
            End Select

            'add sorting if necessary
            If (Not String.IsNullOrWhiteSpace(f.sortName)) Then
                'create sort parameters
                Dim sdir = ""
                Select Case f.sortDirection
                    Case "asc"
                        sdir = "Ascending"
                    Case "desc"
                        sdir = "Descending"
                    Case Else
                        sdir = "Ascending"
                End Select

                sortOrdinal = getSortFieldKeySettlement(f.sortName)
                sortDirection = sdir
            End If

        End If

        settlementItems = NGLBookData.GetSettlementRecords365(RecordCount,
                                                              CarrierControl,
                                                              CarrierContControl,
                                                              proNumber,
                                                              cns,
                                                              bookCarrOrderNumber,
                                                              bookDateDelDate,
                                                              bookDateDelDateTo,
                                                              bookPayCode,
                                                              bookRevTotalCost,
                                                              bookFinAPBillNumber,
                                                              bookFinAPActCost,
                                                              bookSHID,
                                                              bookShipCarrierProNumber,
                                                              sortOrdinal,
                                                              sortDirection,
                                                              datefilterfield,
                                                              dateFilterFrom,
                                                              dateFilterTo,
                                                              carrierName,
                                                              f)

        Return settlementItems
    End Function

    Private Function getSortFieldKeySettlement(ByVal fieldName As String) As String
        Dim strRet = ""

        Select Case fieldName
            Case "ProNumber"
                strRet = "BookProNumber"
            Case "CnsNumber"
                strRet = "BookConsPrefix"
            Case "OrderNumber"
                strRet = "BookCarrOrderNumber"
            Case "PickupName"
                strRet = "BookOrigName"
            Case "DestinationName"
                strRet = "BookDestName"
            Case "Status"
                strRet = "BookPayCode"
            Case "DeliveredDate"
                strRet = "BookDateDelivered"
            Case "ContractedCost"
                strRet = "BookRevTotalCost"
            Case "InvoiceNumber"
                strRet = "BookFinAPBillNumber"
            Case "InvoiceAmount"
                strRet = "BookFinAPActCost"
            Case "SHID"
                strRet = "BookSHID"
            Case "CarrierPro"
                strRet = "BookShipCarrierProNumber"
            Case "CarrierName"
                strRet = "CarrierName"
            Case Else
                strRet = fieldName
        End Select

        Return strRet
    End Function



#End Region

#Region "Settled Tab"

    Public Function GetSettledItems365(ByVal f As Models.AllFilters, ByRef recordCount As Integer) As DTO.SettledItem()
        Dim nglSettledItems As DTO.SettledItem()

        Dim CarrierControl As Integer? = Nothing
        Dim CarrierContControl As Integer? = Nothing
        Dim proNumber As String = Nothing
        Dim cns As String = Nothing
        Dim bookFinAPPayDate As Date? = Nothing
        Dim bookFinAPPayAmt As Decimal? = Nothing
        Dim bookFinAPPayCheck As String = Nothing
        Dim bookRevTotalCost As Decimal? = Nothing
        Dim bookFinAPBillNumber As String = Nothing
        Dim bookFinAPActCost As Decimal? = Nothing
        Dim sortOrdinal = "CNS Number"  'default value
        Dim sortDirection = "Descending"    'default value
        Dim datefilterfield = ""
        Dim dateFilterFrom As Date? = Nothing
        Dim dateFilterTo As Date? = Nothing
        Dim bookSHID As String = Nothing
        Dim bookShipCarrierProNumber As String = Nothing
        Dim carrierName As String = Nothing

        If Parameters.UserCarrierControl > 0 Then CarrierControl = Parameters.UserCarrierControl
        If Parameters.UserCarrierContControl > 0 Then CarrierContControl = Parameters.UserCarrierContControl

        'add filtering if necessary
        If (Not f Is Nothing) Then
            Select Case (f.filterName)
                Case "InvoiceNumber"  'Invoice Number
                    bookFinAPBillNumber = f.filterValue
                Case "CheckNumber"   'Check number
                    bookFinAPPayCheck = f.filterValue
                Case "ProNumber" 'Pro Number
                    proNumber = f.filterValue
                Case "CnsNumber"  'CNS Number
                    cns = f.filterValue
                Case "SHID"   'SHID
                    bookSHID = f.filterValue
                Case "CarrierPro"   'CarrierPro
                    bookShipCarrierProNumber = f.filterValue
                Case "PaidDate" 'Paid date
                    Dim fromDate = If(f.filterFrom.HasValue, f.filterFrom.Value, Date.MinValue)
                    Dim toDate = If(f.filterTo.HasValue, f.filterTo.Value, Date.MaxValue)
                    datefilterfield = "BookFinAPPayDate"
                    dateFilterFrom = fromDate
                    dateFilterTo = toDate
                Case "CarrierName"  'Carrier Name
                    carrierName = f.filterValue
            End Select

            'add sorting if necessary
            If (Not String.IsNullOrWhiteSpace(f.sortName)) Then
                'Modified by RHR for v-8.5.2.005 on 08/05/2022 convert sorting to PaidDate for txt version
                If f.sortName = "TxtPaidDate" Then f.sortName = "PaidDate"
                'create sort parameters
                Dim sdir = ""
                Select Case f.sortDirection
                    Case "asc"
                        sdir = "Ascending"
                    Case "desc"
                        sdir = "Descending"
                    Case Else
                        sdir = "Ascending"
                End Select

                sortOrdinal = getSortFieldKeySettled(f.sortName)
                sortDirection = sdir
            End If
        End If

        nglSettledItems = NGLBookData.GetSettledRecords365(recordCount,
                                                              CarrierControl,
                                                              CarrierContControl,
                                                              proNumber,
                                                              cns,
                                                              bookFinAPPayDate,
                                                              bookFinAPPayAmt,
                                                              bookFinAPPayCheck,
                                                              bookRevTotalCost,
                                                              bookFinAPBillNumber,
                                                              bookFinAPActCost,
                                                              bookSHID,
                                                              bookShipCarrierProNumber,
                                                              sortOrdinal,
                                                              sortDirection,
                                                              datefilterfield,
                                                              dateFilterFrom,
                                                              dateFilterTo,
                                                              carrierName,
                                                              f)

        Return nglSettledItems
    End Function

    Private Function getSortFieldKeySettled(ByVal fieldName As String) As String
        Dim strRet = ""

        Select Case fieldName
            Case "InvoiceAmount"
                strRet = "BookFinAPActCost"
            Case "ContractedCost"
                strRet = "BookRevTotalCost"
            Case "PaidCost"
                strRet = "BookFinAPPayAmt"
            Case "InvoiceNumber"
                strRet = "BookFinAPBillNumber"
            Case "PaidDate"
                strRet = "BookFinAPPayDate"
            Case "CheckNumber"
                strRet = "BookFinAPCheck"
            Case "ProNumber"
                strRet = "BookProNumber"
            Case "CnsNumber"
                strRet = "BookConsPrefix"
            Case "SHID"
                strRet = "BookSHID"
            Case "CarrierPro"
                strRet = "BookShipCarrierProNumber"
            Case "CarrierName"
                strRet = "CarrierName"
            Case Else
                strRet = fieldName
        End Select

        Return strRet
    End Function


#End Region

#End Region

#Region "Process Freight Bill Methods"

    'Private Function InsertFreightBillHistory(ByRef s As Models.SettlementSave, ByVal blnElectronicFlag As Boolean, ByRef sRetMsg As String, ByRef oResults As Models.ResultObject) As Boolean
    '    Dim blnRet As Boolean = False
    '    Dim sHistRecord As String = ""
    '    If String.IsNullOrWhiteSpace(s.InvoiceNo) Then
    '        'we cannot continue return an exception
    '        NGLAPMassEntryData.throwFieldRequiredException("Freight Bill Invoice Number")
    '    End If
    '    Try
    '        Dim dtNow As DateTime = Date.Now()
    '        If s.CarrierNumber = 0 AndAlso s.CarrierControl <> 0 Then
    '            Dim oCarrierData = NGLCarrierData.getCarrierNameNumber(s.CarrierControl)
    '            If Not oCarrierData Is Nothing AndAlso oCarrierData.ContainsKey("CarrierNumber") Then
    '                Integer.TryParse(oCarrierData("CarrierNumber"), s.CarrierNumber)
    '            End If
    '        End If
    '        If s.BookControl = 0 AndAlso Not String.IsNullOrWhiteSpace(s.BookSHID) Then
    '            s.BookControl = NGLBookData.GetFirstBookControlBySHID(s.BookSHID)
    '        End If
    '        If (String.IsNullOrWhiteSpace(s.APCustomerID)) And s.BookControl <> 0 Then
    '            Dim oCompData = NGLBookData.GetCompanyNameNumberByBookControl(s.BookControl)
    '            If Not oCompData Is Nothing AndAlso oCompData.ContainsKey("CompNumber") Then
    '                s.APCustomerID = oCompData("CompNumber")
    '            End If
    '        End If
    '        Dim dOtherCost As Decimal = (s.InvoiceAmt - s.LineHaul) - s.TotalFuel
    '        sHistRecord = String.Format("BillDate = {0}, BilledWeight = {1}, BillNumber = {2}, CarrierCost = {3}, CarrierNumber = {4}, CustomerID = {5}, ElectronicFlag = {6}, ReceivedDate = {7}, SHID = {8}, TotalCost = {9}, BLNumber = {10}, OtherCosts = {11} ", dtNow, s.BookFinAPActWgt, s.InvoiceNo, s.LineHaul, s.CarrierNumber, s.APCustomerID, blnElectronicFlag, dtNow, s.BookSHID, s.InvoiceAmt, s.BookCarrBLNumber, dOtherCost)
    '        Dim oHistory As New LTS.APMassEntryHistory() With {.APMHBillDate = dtNow, .APMHBilledWeight = s.BookFinAPActWgt, .APMHBillNumber = s.InvoiceNo, .APMHCarrierCost = s.LineHaul, .APMHCarrierNumber = s.CarrierNumber, .APMHCustomerID = s.APCustomerID, .APMHElectronicFlag = blnElectronicFlag, .APMHReceivedDate = dtNow, .APMHSHID = s.BookSHID, .APMHTotalCost = s.InvoiceAmt, .APMHBLNumber = s.BookCarrBLNumber, .APMHOtherCosts = dOtherCost}

    '        If NGLAPMassEntryHistoriesData.InsertOrUpdateAPMassEntryHistory(oHistory) Then
    '            Dim iAPMHControl = oHistory.APMHControl
    '            Dim iProcessFeesErrors As Integer = 0
    '            If iAPMHControl <> 0 AndAlso Not s.Fees Is Nothing AndAlso s.Fees.Count() > 0 Then
    '                Dim sFeeRecord As String = ""
    '                For Each f In s.Fees
    '                    Try
    '                        sFeeRecord = String.Format("AccessorialCode = {0}, APMHControl = {1}, BookControl = {2}, Caption = {3}, Cost = {4}, StopSequence = {5}, OrderNumber = {6}", f.AccessorialCode, iAPMHControl, f.BookControl, f.Caption, f.Cost, f.StopSequence, f.BookCarrOrderNumber)
    '                    Catch innerE As Exception
    '                        'do nothing
    '                    End Try
    '                    If (f.BookControl = 0) Then f.BookControl = s.BookControl
    '                    Try
    '                        Dim oFee As New LTS.APMassEntryHistoryFee() With {.APMHFeesAccessorialCode = f.AccessorialCode, .APMHFeesAPMHControl = iAPMHControl, .APMHFeesBookControl = f.BookControl, .APMHFeesCaption = f.Caption, .APMHFeesValue = f.Cost, .APMHFeesStopSequence = f.StopSequence, .APMHFeesOrderNumber = f.BookCarrOrderNumber}
    '                        NGLAPMassEntryHistoryFeesData.InsertOrUpdateAPMassEntryHistoryFee(oFee)
    '                    Catch ex As Exception
    '                        iProcessFeesErrors += 1
    '                        'just log any save historical fees data in system error log
    '                        logSystemError(ex, "NGLMS365BLL.InsertFreightBillHistory(Fees)", sFeeRecord)
    '                    End Try

    '                Next
    '                If iProcessFeesErrors > 0 Then
    '                    Dim sDetail = String.Format(" Error Count {0} freight bill number {1}, ", iProcessFeesErrors, s.InvoiceNo)
    '                    sRetMsg = appendToResultMessage(oResults, sDetail, Models.ResultObject.ResultMsgType.Warning, ResultProcedures.freightbill, ResultTitles.TitleSaveHistLogFailure, ResultPrefix.MsgDetails, ResultSuffix.MsgDoesNotEffectProcess)
    '                End If
    '            End If
    '            blnRet = True
    '        End If
    '    Catch ex As Exception
    '        Dim sDetail = String.Format(" freight bill number {0}, ", s.InvoiceNo)
    '        sRetMsg = appendToResultMessage(oResults, sDetail, Models.ResultObject.ResultMsgType.Err, ResultProcedures.freightbill, ResultTitles.TitleSaveHistLogFailure, ResultPrefix.MsgDetails, ResultSuffix.MsgDoesNotEffectProcess)
    '        'log any save historical fees data in system error log
    '        logSystemError(ex, "NGLMS365BLL.InsertFreightBillHistory", sHistRecord)
    '    End Try
    '    Return blnRet
    'End Function

    Private Sub createSubscriptionAlert(ByVal msg As String)

    End Sub
    ''' <summary>
    ''' logs non-empty messages to the APMassEntryMsg table
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <param name="sbRetMsgs"></param>
    ''' <param name="APControl"></param>
    ''' <param name="eReasonCode"></param>
    ''' <param name="blnMarkAsResolved"></param>
    ''' <param name="sSource"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.117 on 8/12/19
    '''     if msg is not empty the procedure creates an APMassEntryMsg record using the provided parameters
    '''     if sbRetMsg is not nothing the msg is added to the stringbuilder for use by the caller
    '''     Errors are logged as system errors 
    ''' </remarks>
    Private Sub addAuditMessage(ByVal msg As String, ByRef sbRetMsgs As System.Text.StringBuilder, ByVal APControl As Integer, ByVal eReasonCode As DAL.NGLLookupDataProvider.FBLoadStatusCodes, Optional ByVal blnMarkAsResolved As Boolean = False, Optional ByVal sSource As String = "NGLTMS365BLL.addAuditMessage")
        Dim sRecord = msg
        Try
            If Not String.IsNullOrWhiteSpace(msg) Then
                If Not sbRetMsgs Is Nothing Then
                    sbRetMsgs.Append(msg)
                End If
                If APControl > 0 Then
                    Dim iLoadStatusControl = 0
                    If Not tryGetLoadStatusControl(eReasonCode, iLoadStatusControl) Then iLoadStatusControl = storeLoadStatusControl(eReasonCode, NGLLoadStatusCodeData.GetLoadStatusControl(eReasonCode, DAL.NGLLookupDataProvider.GetFBReasonCodeDesc(eReasonCode), DAL.NGLLookupDataProvider.LoadStatusCodeTypes.FreightBill))

                    Dim oAPMessageData As New LTS.APMassEntryMsg() With {.APMMsgAPControl = APControl, .APMMsgCreateDate = Date.Now(), .APMMsgLoadStatusControl = iLoadStatusControl, .APMMsgMessage = msg, .APMMsgResolved = blnMarkAsResolved, .APMMsgSource = sSource, .APMMsgModDate = Date.Now(), .APMMsgModUser = Me.Parameters.UserName}
                    NGLAPMassEntryMsgData.InsertOrUpdateAPMassEntryMsg(oAPMessageData)
                End If

            End If
        Catch ex As Exception
            Try
                sRecord = String.Format("APControl:  {0}, Reason: {1}, Msg: {2}", APControl, eReasonCode.ToString(), msg)
            Catch innerE As Exception
                'do nothing
            End Try

            'just log the error 
            logSystemError(ex, "NGLMS365BLL.addAuditMessage", sRecord)
        End Try
    End Sub

    '''' <summary>
    '''' Copies the current booking financial data into Book Revenue History and marks it as The Expected Cost
    '''' </summary>
    '''' <param name="s"></param>
    '''' <param name="sRetMsg"></param>
    '''' <param name="oResults"></param>
    '''' <param name="blnContinueOnFaultException"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' Created by RHR for v-8.2.0.117 on 8/9/19
    ''''         wrapper to CreateBookRevenueHistory
    ''''         the optional blnContinueOnFaultException parameter allows a true response 
    ''''         even on a Fault exception but returns a message instead.
    ''''         This allows the caller to continue processing other tasks.  
    ''''         Unexpected Exceptions are still re=thrown to the caller.
    ''''         if blnContinueOnFaultException is false all exceptions are thrown back to the caller
    '''' </remarks>
    'Private Function createExpectedCost(ByRef s As Models.SettlementSave, ByRef sRetMsg As String, ByRef oResults As Models.ResultObject, Optional ByVal blnContinueOnFaultException As Boolean = False) As Boolean
    '    Try
    '        Return NGLBookRevenueData.CreateBookRevenueHistory(s.BookControl, True)

    '    Catch ex As FaultException
    '        If blnContinueOnFaultException Then
    '            Dim sDetail = String.Format(" freight bill number {0}. ", s.InvoiceNo)
    '            sRetMsg = appendToResultMessage(oResults, sDetail, Models.ResultObject.ResultMsgType.Err, ResultProcedures.freightbill, ResultTitles.TitleSaveExpectedCost, ResultPrefix.MsgCostComparisonNotAvailable, ResultSuffix.MsgCheckAppErrorLogs)
    '            'log any save historical fees data in system error log
    '            SaveAppError(String.Format("Unexpected Error in NGLTMS365BLL.createExpectedCost for freight bill {0}. Message: {1}  ", s.InvoiceNo, ex.Message))
    '            Return True
    '        Else
    '            Throw
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Function

    'Private Function createOrUpdateAPMassEntry(ByRef s As Models.SettlementSave, ByVal ElectronicFlag As Boolean, ByRef sRetMsg As String, ByRef oResults As Models.ResultObject) As Boolean
    '    Dim dalAPME As New DAL.NGLAPMassEntryData(Parameters)
    '    'Re-write InsertFreightBillWeb365 logic to only create the record and not run the audit.
    '    Dim r = dalAPME.InsertFreightBillWeb365(s.BookControl, s.InvoiceNo, s.APBillDate, s.APReceivedDate, s.InvoiceAmt, s.LineHaul, s.CarrierControl, s.BookFinAPActWgt, s.BookCarrBLNumber, ElectronicFlag)
    '    If (Not r Is Nothing) Then
    '        If (r.ErrNumber <> 0) Then
    '            sRetMsg = appendToResultMessage(oResults, r.RetMsg, Models.ResultObject.ResultMsgType.Warning, ResultProcedures.freightbill, ResultTitles.TitleSaveHistLogFailure, ResultPrefix.MsgDetails, ResultSuffix.None)
    '            oResults.Success = False
    '            Return False
    '        Else
    '            'update the settlementsave data
    '            s.BookSHID = r.BookSHID
    '            s.InvoiceNo = r.APBillNumber
    '            s.APControl = r.APControl
    '            Return True
    '        End If

    '    End If
    '    Return True
    'End Function


    'Private Sub logAPMessage(ByRef s As Models.SettlementSave, ByVal ElectronicFlag As Boolean, ByVal sMsg As String)
    '    Return
    'End Sub

    'Private Function processBilledFees(ByRef s As Models.SettlementSave, ByRef sRetMsg As String) As Boolean
    '    'steps for each fee
    '    Dim oStopFees As New List(Of Models.SettlementFee)
    '    Dim oBookFees As New List(Of Models.SettlementFee)
    '    ' Dim dTotalFuel As Decimal = s.TotalFuel
    '    Dim sFuelFeeCaption As String = ""
    '    Dim sFuelFeeEDICode As String = ""
    '    Dim sMsg As String = ""
    '    Dim iErrNumber As Integer
    '    'if one fails we log the issue and continue to the next fee
    '    '1. Identify if each fee is by stop number or order number
    '    '       loop through each fee and merge into one fee for each stop or order number as needed.
    '    '2. Lookup correct EDI Code and map to correct accessorial code this is done by the callers
    '    '3. Determine if we have Fuel as one of the accessorial fees (typically from EDI)
    '    '   process TotalFuel from SettlementSave
    '    '   Append any fuel fees to TotalFuel
    '    'Note:  
    '    '       4. 5. and 6. must work together becaue some functionality cannot be completed until all 
    '    '       fees have been processed atlease once.
    '    '       Step 1. Determine total cost by stop using allocation rules.
    '    '       Step 2. Save to APMassEntryFee for each bookcontrol
    '    '       Step 3. Update Pending Fees
    '    '       Step 4. Log and process any alerts or issues
    '    '4. If any fee total does not match total for order create a new pending fee by order.
    '    '       If order number is empty and stop number is zero this fee allcoates to the entire load.
    '    '           so we need to merge any fees with blank order numbers and stop number zero into one fee record
    '    '       If order number is provided we ignore the stop number logic and we ignore the allocation rules.
    '    '       If total fuel for the load is different than SettlementSave.TotalFuel we create a new pending fee as Spot Rate Fuel
    '    '           this is allocated accross all stops with Spot Rate Fuel Allocation rules in Accessorial Table (later we may make this configurable by each LE)
    '    '       Follow all other fee rules
    '    '5. Update the APMassEntryFee table with the correct allocated amount by bookcontrol
    '    '       Once all order numbers and stop numbers are process we need to determine if any expected fees are 
    '    '           are missing,  We use the APMassEntryFee table to compare costs. Costs must match or a pending
    '    '           must be waiting on approval with a matching cost.  Zeros are allowd in APMassEntryFee this indicates 
    '    '           that the fee is to be waved or removed.
    '    '6. Log AP Fee Messages and Alerts (typically return value from 5.)  5. will update Fee Table flags
    '    '       Special Issues and Alerts:
    '    '           Alert when an expected fee is missing in the bill
    '    '           Alert when a non-zero billed fee is not listed as expected 
    '    '           Alert when a billed fee is not configured for the L.E. Carrier
    '    If Not s.Fees Is Nothing AndAlso s.Fees.Count() > 0 Then

    '        For Each sFee In s.Fees
    '            If sFee.AccessorialCode = 2 Or sFee.AccessorialCode = 9 Or sFee.AccessorialCode = 15 Then
    '                'For step 3 we combine the total fuel
    '                s.TotalFuel += sFee.Cost
    '                sFuelFeeCaption = sFee.Caption
    '                sFuelFeeEDICode = sFee.EDICode
    '            Else
    '                Dim blnMatchFound As Boolean = False
    '                If String.IsNullOrWhiteSpace(sFee.BookCarrOrderNumber) Then
    '                    For Each sf In oStopFees
    '                        If sf.StopSequence = sFee.StopSequence And sf.AccessorialCode = sFee.AccessorialCode Then
    '                            sf.Cost += sFee.Cost 'add the fees together for this stop
    '                            blnMatchFound = True
    '                            Exit For
    '                        End If
    '                    Next
    '                    If Not blnMatchFound Then
    '                        oStopFees.Add(sFee)
    '                    End If
    '                Else
    '                    For Each sf In oStopFees
    '                        If sf.BookCarrOrderNumber = sFee.BookCarrOrderNumber And sf.AccessorialCode = sFee.AccessorialCode Then
    '                            sf.Cost += sFee.Cost 'in the case where the fee is listed twice for the same order we add them together
    '                            blnMatchFound = True
    '                            Exit For
    '                        End If
    '                    Next
    '                    If Not blnMatchFound Then
    '                        oStopFees.Add(sFee)
    '                    End If
    '                End If
    '            End If

    '        Next
    '        'now that we have the stop specific fees we need to get the booking control numbers and allocate for each fee that does not have an order number
    '        For Each sf In oStopFees.Where(Function(x) String.IsNullOrWhiteSpace(x.BookCarrOrderNumber) = True)
    '            Dim oAllocatedBookFees = NGLBookData.GetBookControlForStopFee(s.BookSHID, sf.StopSequence, sf.Cost, sf.AccessorialCode)
    '            If Not oAllocatedBookFees Is Nothing AndAlso oAllocatedBookFees.Count() > 0 Then
    '                For Each af In oAllocatedBookFees
    '                    oBookFees.Add(New Models.SettlementFee() With {
    '                        .BookControl = af.BookControl,
    '                        .Cost = af.AllocatedCost,
    '                        .BookCarrOrderNumber = af.BookCarrOrderNumber,
    '                        .AccessorialCode = sf.AccessorialCode,
    '                        .AllowCarrierUpdates = sf.AllowCarrierUpdates,
    '                        .AutoApprove = sf.AutoApprove,
    '                        .BFPControl = sf.BFPControl,
    '                        .Caption = sf.Caption,
    '                        .Control = sf.Control,
    '                        .FeeIndex = sf.FeeIndex,
    '                        .Minimum = sf.Minimum,
    '                        .Msg = sf.Msg,
    '                        .Pending = sf.Pending,
    '                        .StopSequence = sf.StopSequence})
    '                Next
    '            End If
    '        Next

    '        For Each sf In oStopFees.Where(Function(x) String.IsNullOrWhiteSpace(x.BookCarrOrderNumber) = False)
    '            If sf.BookControl = 0 Then sf.BookControl = NGLBookData.GetBookControlByOrderNumber(sf.BookCarrOrderNumber)
    '            oBookFees.Add(New Models.SettlementFee() With {
    '                        .BookControl = sf.BookControl,
    '                        .Cost = sf.Cost,
    '                        .BookCarrOrderNumber = sf.BookCarrOrderNumber,
    '                        .AccessorialCode = sf.AccessorialCode,
    '                        .AllowCarrierUpdates = sf.AllowCarrierUpdates,
    '                        .AutoApprove = sf.AutoApprove,
    '                        .BFPControl = sf.BFPControl,
    '                        .Caption = sf.Caption,
    '                        .Control = sf.Control,
    '                        .FeeIndex = sf.FeeIndex,
    '                        .Minimum = sf.Minimum,
    '                        .Msg = sf.Msg,
    '                        .Pending = sf.Pending,
    '                        .StopSequence = sf.StopSequence})

    '        Next
    '        'Now we consolidate fees that need to be allocated by Load, Origin, or Destination into one stop fee
    '        Dim oLoadSpecificFees = New List(Of Models.SettlementFee)
    '        Dim oLoadOrigFees = New List(Of Models.SettlementFee)
    '        Dim oLoadDestFees = New List(Of Models.SettlementFee)
    '        Dim oOrderFees = New List(Of Models.SettlementFee)
    '        'get all of the  accessorials
    '        Dim oAccessorials = NGLtblAccessorialData.GetAllAccessorials()
    '        'get the booking records so we can check the orig and dest address info
    '        Dim oBooks = NGLBookData.GetBooksBySHID(s.BookSHID)
    '        For Each bf In oBookFees
    '            Dim iAllocationType = oAccessorials.Where(Function(a) a.AccessorialCode = bf.AccessorialCode).Select(Function(i) i.AccessorialAccessorialFeeAllocationTypeControl).FirstOrDefault()
    '            '1   None
    '            '2   Origin
    '            '3   Destination
    '            '4   Load
    '            Select Case iAllocationType
    '                Case 4   'Load
    '                    If oLoadSpecificFees.Any(Function(o) o.AccessorialCode = bf.AccessorialCode) Then
    '                        Dim item = oLoadSpecificFees.Where(Function(o) o.AccessorialCode = bf.AccessorialCode).FirstOrDefault()
    '                        item.Cost += bf.Cost
    '                    Else
    '                        oLoadSpecificFees.Add(bf)
    '                    End If
    '                Case 2 'Origin
    '                    If Not oBooks Is Nothing AndAlso oBooks.Any(Function(b) b.BookControl = bf.BookControl) Then
    '                        If oLoadOrigFees.Any(Function(o) o.AccessorialCode = bf.AccessorialCode) Then
    '                            Dim items = oLoadOrigFees.Where(Function(o) o.AccessorialCode = bf.AccessorialCode).ToArray()
    '                            'for each i in items we check the orig informaiton
    '                            If Not items Is Nothing AndAlso items.Count() > 0 Then
    '                                Dim blnOrigFound As Boolean = False
    '                                For Each i In items
    '                                    'get the book data
    '                                    Dim iBook = oBooks.Where(Function(b) b.BookControl = i.BookControl).FirstOrDefault()
    '                                    Dim tBook = oBooks.Where(Function(b) b.BookControl = bf.BookControl).FirstOrDefault()
    '                                    If Not iBook Is Nothing AndAlso Not tBook Is Nothing Then
    '                                        If (iBook.BookOrigAddress1 = tBook.BookOrigAddress1 _
    '                                            And iBook.BookOrigCity = tBook.BookOrigCity _
    '                                            And iBook.BookOrigState = tBook.BookOrigState _
    '                                            And iBook.BookOrigZip = tBook.BookOrigZip) Then
    '                                            i.Cost += bf.Cost
    '                                            blnOrigFound = True
    '                                            Exit For
    '                                        End If
    '                                    End If
    '                                Next
    '                                If Not blnOrigFound Then
    '                                    'just add this to the orig fees it has a new address
    '                                    oLoadOrigFees.Add(bf)
    '                                End If
    '                            Else
    '                                'just add this to the orig fees
    '                                oLoadOrigFees.Add(bf)
    '                            End If

    '                        Else
    '                            oLoadOrigFees.Add(bf)
    '                        End If
    '                    Else
    '                        'just add this fee as an order specific fee, this should not happen but we dont want to miss any fees
    '                        oOrderFees.Add(bf)
    '                    End If

    '                Case 3 'Destination
    '                    If Not oBooks Is Nothing AndAlso oBooks.Any(Function(b) b.BookControl = bf.BookControl) Then
    '                        If oLoadDestFees.Any(Function(o) o.AccessorialCode = bf.AccessorialCode) Then
    '                            Dim items = oLoadDestFees.Where(Function(o) o.AccessorialCode = bf.AccessorialCode).ToArray()
    '                            'for each i in items we check the orig informaiton
    '                            If Not items Is Nothing AndAlso items.Count() > 0 Then
    '                                Dim blnDestFound As Boolean = False
    '                                For Each i In items
    '                                    'get the book data
    '                                    Dim iBook = oBooks.Where(Function(b) b.BookControl = i.BookControl).FirstOrDefault()
    '                                    Dim tBook = oBooks.Where(Function(b) b.BookControl = bf.BookControl).FirstOrDefault()
    '                                    If Not iBook Is Nothing AndAlso Not tBook Is Nothing Then
    '                                        If (iBook.BookDestAddress1 = tBook.BookDestAddress1 _
    '                                            And iBook.BookDestCity = tBook.BookDestCity _
    '                                            And iBook.BookDestState = tBook.BookDestState _
    '                                            And iBook.BookDestZip = tBook.BookDestZip) Then
    '                                            i.Cost += bf.Cost
    '                                            blnDestFound = True
    '                                            Exit For
    '                                        End If
    '                                    End If
    '                                Next
    '                                If Not blnDestFound Then
    '                                    'just add this to the orig fees it has a new address
    '                                    oLoadDestFees.Add(bf)
    '                                End If
    '                            Else
    '                                'just add this to the orig fees
    '                                oLoadDestFees.Add(bf)
    '                            End If

    '                        Else
    '                            oLoadDestFees.Add(bf)
    '                        End If
    '                    Else
    '                        'just add this fee as an order specific fee, this should not happen but we dont want to miss any fees
    '                        oOrderFees.Add(bf)
    '                    End If

    '                Case Else
    '                    oOrderFees.Add(bf)
    '            End Select
    '        Next
    '        'add all the fees together
    '        oStopFees = New List(Of Models.SettlementFee)
    '        oStopFees.AddRange(oOrderFees)
    '        oStopFees.AddRange(oLoadOrigFees)
    '        oStopFees.AddRange(oLoadDestFees)
    '        oStopFees.AddRange(oLoadSpecificFees)
    '        'Now oBookFees should have all the correct data and allocated costs so replae the original fee array
    '        s.Fees = oStopFees.ToArray()
    '        'If If(s.APControl, 0) > 0 Then ' we must have an apcontrol to process fuel,  this should always pass if the caller does it's job
    '        '    NGLAPMassEntryFeesData.UpdateAPMassEntryFees(s.APControl, s.Fees)
    '        'End If
    '        'Note: this code may need to be called after we process fuel and if we updae s.Fees with the fuel fees returned
    '        '      must test with multilple stops 

    '        If Not s.Fees Is Nothing AndAlso s.Fees.Count() > 0 Then
    '            For Each sFee In s.Fees
    '                Try
    '                    ' SaveSettlementFees will insert or update the pending fees and the APMassEntryFeesTable 
    '                    Dim spRes As LTS.spSaveSettlementFeesResult = NGLBookFeePendingData.SaveSettlementFees(sFee, s.CompControl, s.CarrierControl)
    '                    '8. Update the Fees object with the results
    '                    sFee.BFPControl = If(spRes.BFPControl, 0)
    '                    sFee.AutoApprove = If(spRes.BFPAutoApprove, False)
    '                    If If(spRes.ErrNumber, 0) <> 0 Then
    '                        'we have an error
    '                        sRetMsg &= " Process Billed Fee Error; " & sFee.Caption & " failed: " & spRes.RetMsg & " | "
    '                    End If
    '                Catch ex As Exception
    '                    'we may need to add special exceptions to handle fault exceptions
    '                    'for now just append to the sRetMsg; also add logic to determine if we should return false?
    '                    sRetMsg &= " Process Billed Fee Error; " & sFee.Caption & " failed: " & ex.Message & " | "
    '                End Try
    '            Next
    '        End If

    '    End If
    '    If If(s.APControl, 0) > 0 Then ' we must have an apcontrol to process fuel,  this should always pass if the caller does it's job
    '        'process the fuel charges
    '        Dim oFuelFees As Models.SettlementFee() = NGLAPMassEntryFeesData.UpdateFreightBillFuelCosts(s.APControl, s.BookSHID, s.TotalFuel, sFuelFeeCaption, sFuelFeeEDICode, sMsg, iErrNumber)
    '        If iErrNumber <> 0 And Not String.IsNullOrWhiteSpace(sMsg) Then
    '            sRetMsg &= " Process Billed Fuel Error: " & sMsg & " | "
    '        End If
    '        If Not oFuelFees Is Nothing AndAlso oFuelFees.Count() > 0 Then
    '            Dim lFees As New List(Of Models.SettlementFee)
    '            lFees = s.Fees.ToList()
    '            lFees.AddRange(oFuelFees)
    '            s.Fees = lFees.ToArray()
    '            's.Fees.append(oFuelFees)
    '        End If
    '    End If

    '    Return True
    'End Function


    'Private Function runAutoApprovePendingFees(ByVal s As Models.SettlementSave, ByRef sRetMsg As String, ByRef oResults As Models.ResultObject) As Boolean
    '    Dim bllBookRev As New BLL.NGLBookRevenueBLL(Parameters)
    '    ' all updates to fees must have been processed so we need to read the data from the table
    '    ' this will include any pending fuel costs
    '    Try
    '        Dim iBookControls As New List(Of Integer)
    '        If Not s.Fees Is Nothing AndAlso s.Fees.Count() > 0 Then
    '            iBookControls = s.Fees.Select(Function(x) x.BookControl).Distinct().ToList()
    '        End If
    '        If Not iBookControls.Contains(s.BookControl) Then iBookControls.Add(s.BookControl) 'be sure we have the primary book control
    '        'now process all the fees for each book control
    '        For Each iBookControl In iBookControls
    '            Dim oBFPs = NGLBookFeePendingData.GetBookFeePendingsFiltered(iBookControl)
    '            If Not oBFPs Is Nothing AndAlso oBFPs.Count() > 0 Then
    '                'steps for each fee
    '                For Each sFee In oBFPs
    '                    Try
    '                        'if one fails we log the issue and continue to the next fee
    '                        '1. Can Fee Be Auto Approved
    '                        If sFee.BookFeesPendingControl <> 0 Then
    '                            Dim blnWithinTolerance = NGLBookFeePendingData.CanFeeBeAutoApproved(sFee.BookFeesPendingBookControl, sFee.BookFeesPendingAccessorialCode, sFee.BookFeesPendingValue, sFee.BookFeesPendingControl)
    '                            If (blnWithinTolerance) Then
    '                                '2. If true then fee Is AutoApproved - update BFP as approved = true And user = "AutoApprove" And also create record in BF table
    '                                Dim spApproveBFPRes As LTS.spAcceptPendingBookFeeResult = NGLBookFeePendingData.ApproveBookFeePending(sFee.BookFeesPendingControl, "AutoApprove")
    '                                '3. process the approval messages
    '                                If (Not spApproveBFPRes Is Nothing) Then
    '                                    If ((If(spApproveBFPRes.ErrNumber, 0) <> 0) OrElse (If(spApproveBFPRes.BookFeesControl, 0) = 0)) Then
    '                                        Dim sDetails = spApproveBFPRes.RetMsg & " For " & sFee.BookFeesPendingCaption
    '                                        sRetMsg &= appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Warning, ResultProcedures.freightbill, ResultTitles.TitlePendingFeeApprovalWarning, ResultPrefix.MsgDetails, ResultSuffix.MsgDoesNotEffectProcess)
    '                                        Continue For
    '                                    End If
    '                                Else
    '                                    'spApproveBFPRes is nothing so the procedure failed
    '                                    Dim sDetails = sFee.BookFeesPendingCaption & ". "
    '                                    sRetMsg &= appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Warning, ResultProcedures.freightbill, ResultTitles.TitlePendingFeeApprovalWarning, ResultPrefix.MsgUnexpectedFeeValidationIssue, ResultSuffix.MsgDoesNotEffectProcess)
    '                                    Continue For
    '                                End If

    '                            End If
    '                        End If

    '                    Catch ex As Exception
    '                        Dim sDetails = String.Format(" SHID {0}, freight bill number{1}, fee {2}, error {3} . ", s.BookSHID, s.InvoiceNo, sFee.BookFeesPendingCaption, ex.Message)
    '                        sRetMsg = appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Err, ResultProcedures.freightbill, ResultTitles.TitlePendingFeeApprovalError, ResultPrefix.MsgDetails, ResultSuffix.None)
    '                        'add to system error log
    '                        Dim sRecord = String.Format(" For SHID: {0} , Freight Bill Number: {1}, and Fee: {2}", s.BookSHID, s.InvoiceNo, sFee.BookFeesPendingCaption)
    '                        logSystemError(ex, "NGLTMS365BLL.runAutoApprovePendingFees", sRecord)
    '                    End Try
    '                Next
    '            End If
    '        Next
    '        Try
    '            '4 Recalculate
    '            Dim carrierCostResults = bllBookRev.RecalculateUsingLineHaul(s.BookControl)
    '            If ((carrierCostResults Is Nothing) OrElse (carrierCostResults.BookRevs Is Nothing OrElse carrierCostResults.BookRevs.Count < 1)) Then
    '                Dim sDetails = String.Format(" SHID {0} and freight bill number {1}. ", s.BookSHID, s.InvoiceNo)
    '                sRetMsg &= appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Warning, ResultProcedures.freightbill, ResultTitles.TitlePendingFeeApprovalWarning, ResultPrefix.MsgRecalculateCostForFeeFailed, ResultSuffix.MsgUpdatedTotalCostManually)

    '            End If
    '        Catch ex As Exception
    '            'this should not happen so just create a system alert
    '            Dim sRecord = String.Format(" For SHID {0} and freight bill number {1}. ", s.BookSHID, s.InvoiceNo)
    '            logSystemError(ex, "NGLTMS365BLL.runAutoApprovePendingFees.RecalculateUsingLineHaul", sRecord)
    '        End Try


    '    Catch ex As Exception
    '        Dim sDetails = String.Format(" SHID {0}, freight bill number {1}, error {2}. ", s.BookSHID, s.InvoiceNo, ex.Message)
    '        sRetMsg = appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Err, ResultProcedures.freightbill, ResultTitles.TitlePendingFeeApprovalError, ResultPrefix.MsgDetails, ResultSuffix.None)
    '        'add to system error log
    '        Dim sRecord = String.Format(" For SHID: {0} , and Freight Bill Number: {1}", s.BookSHID, s.InvoiceNo)
    '        logSystemError(ex, "NGLTMS365BLL.runAutoApprovePendingFees", sRecord)
    '    End Try

    '    Return True
    'End Function


    Private Function runManualApprovePendingFee(ByVal BookFeesPending As LTS.BookFeesPending, ByVal blnUnlockBFC As Boolean, ByRef sRetMsg As String) As Boolean
        Dim bllBookRev As New BLL.NGLBookRevenueBLL(Parameters)

        Try
            'if one fails we log the issue and continue to the next fee
            '1. Can Fee Be Auto Approved
            If Not BookFeesPending Is Nothing AndAlso BookFeesPending.BookFeesPendingControl <> 0 Then

                Dim spApproveBFPRes As LTS.spAcceptPendingBookFeeResult = NGLBookFeePendingData.ApproveBookFeePending(BookFeesPending.BookFeesPendingControl, Parameters.UserName)
                '3. process the approval messages
                If (Not spApproveBFPRes Is Nothing) Then
                    If ((If(spApproveBFPRes.ErrNumber, 0) <> 0) OrElse (If(spApproveBFPRes.BookFeesControl, 0) = 0)) Then
                        sRetMsg &= " Pending Fee Approval Error; " & BookFeesPending.BookFeesPendingCaption & " failed: " & spApproveBFPRes.RetMsg & " | "
                    End If
                    If Not NGLBookFeePendingData.UnlockCostsForSHID365(BookFeesPending.BookFeesPendingBookControl, blnUnlockBFC) Then
                        sRetMsg &= " Unlock costs failed for  " & BookFeesPending.BookFeesPendingCaption & " using BookFeesPendingControl: " & BookFeesPending.BookFeesPendingControl.ToString() & ", costs will need to be unlocked manually. | "
                    End If
                Else
                    sRetMsg &= " Unexpected Pending Fee Approval Error; for " & BookFeesPending.BookFeesPendingCaption & " failed using BookFeesPendingControl: " & BookFeesPending.BookFeesPendingControl.ToString() & " | "
                End If
                '4 Recalculate
                Dim carrierCostResults = bllBookRev.RecalculateUsingLineHaul(BookFeesPending.BookFeesPendingBookControl)
                If ((carrierCostResults Is Nothing) OrElse (carrierCostResults.BookRevs Is Nothing OrElse carrierCostResults.BookRevs.Count < 1)) Then
                    sRetMsg &= " Recalculate Costs Failed for " & BookFeesPending.BookFeesPendingCaption & " Pending Fee Approval; total costs may need to be updated manually. | "
                End If


            End If

        Catch ex As Exception
            'we may need to add special exceptions to handle fault exceptions
            'for now just append to the sRetMsg; also add logic to determine if we should return false?
            sRetMsg &= " Pending Fee Approval Error; " & BookFeesPending.BookFeesPendingCaption & " failed: " & ex.Message & " | "
        End Try
        Return True
    End Function


    'Private Function canRunAudit(ByRef s As Models.SettlementSave, ByRef sRetMsg As String) As Boolean
    '    'Steps
    '    '1. check for any pending fees
    '    '2. check for any outstanding messages
    '    '3. Log pending fee message
    '    Return True
    'End Function

#End Region

    ''' <summary>
    ''' Code called from SettlementSave in 365 (Web Tender) and also EDI (210In ProcessData)
    ''' TODO: Insert note listing Business Rules here
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/16/2018 for v-8.1
    ''' Modified by LVV on 02/26/2018 for v-8.1 PQ EDI
    ''' Added parameter ElectronicFlag 
    ''' TODO: v-8.2 6/17/2019
    '''     a) find out where and how the Line Haul and Fuel is getting saved
    '''     b) We need a way to update the fuel and some fees (future) without 
    '''        calling InsertFreightBillWeb365 (create freight bill and run audit)
    ''' Modified by RHR for v-8.2 on 7/2/2019
    '''     added new work flow, the previous workflow was out of step with the updated business requirements
    '''     Requirements:
    '''        1.  Create a copy of the Expected cost in Book Rev History before updating and freight charges
    '''        2.  Save a copy of all freight bill accessorials in the APMassEntryFees table
    '''        3.  Allow updates to the APMassEntryFees (including fuel) if not in AA status, could also impact total cost in header, Line Haul cannot be modified once it is created
    '''        4.  Change the logic used to populate the AP Fee buckets with approved costs each time costs are recalculated
    '''        5.  Log carrier changes to fees that do not match currently approved cost in the pending fees table
    '''        6.  Automaticall update the the approved fees based on carrier fee profile settings and tolerances
    '''        7.  When an expected fee is not provided by the carrier the audit freight bull routine will go to M and a message will
    '''            inform the user that a missing expected fee for x with a value of y was not provided.  Users must approve this before the freight bill will pass audit.
    '''        8.  When a missing fee is approved at zero cost the approved fee will be modified with a zero cost order specific fee and the audit will pass
    '''        9.  When a missing fee is approved with a non-zero cost it will be added to the AP Fees data, the Pending Fees data, and the and the approved fees data
    '''        10. AP Fees and Pending Fees will associated with missing carrier fees will be flagged to allow future reporting
    '''        11. The audit cannot pass when pending or missing fees are identified
    '''        12. The line haul audit is associated with total cost we do not audit the line haul directly so if all pending fees are approved and the audit fails it
    '''            must be the result of an invalid line haul amount.  TMS Users must have the ability to edit the line haul in the pending fee approval table
    '''        13. Pending Fee Approval must use allocation rules to generate the correct total amount on the approval screen, so TMS users can edit the total
    '''            for example Fuel is allocated across the entier load so on fee should be listed for fuel on the pending fee approval screen
    '''    Data Storage:
    '''        1.  BookRevenue Data (fields included in the book table)
    '''        2.  BookFees 
    '''        3.  BookFeesPending
    '''        3.  BookRevHistory
    '''        4.  BookRevHistoryFees 
    '''        5.  APMassEntry
    '''        6.  APMassEntryFees
    '''        7.  APMassEntryHistory  (it is not clear how this data is used)
    '''        8.  APMassEntryHistoryFees (new, it is not clear how this data is used)
    '''        9.  APExport (header data, item details and fees are returned at run time from bookitem and bookfees tables)
    '''     Process Flow:
    '''        1.  Create an APMassEntry record or update the existing, add logic to prevent updates to AA status
    '''            Actual data will be written to the APMassEntryHistory table and the APMassEntryHistoryFees table.
    '''            The APMassEntryHistoryFees table holds the actual data provided, not the allocated amount, this will be a snapshot of the Freight Bill
    '''            before any allocation or other alterations have been made during the audit and approval process.
    '''        2.  if creating a new APMassEntry record create a snapshot of Expected Cost in BookRevenueHistory and BookRevHistoryFees
    '''        3.  insert the Billed Fees into the APMassEntryFees table costs are allocated by order number or stop as provided by carrier 
    '''            costs assigned to stop 0 (pickup) are allocated to all orders using TMS/Carrier specific allocation rules.
    '''            if Fuel does not match total fuel for expected cost a spot/flat rate fuel charge will be used and allocation
    '''            will follow the spot/flat rate fuel allocation rules.
    '''        4.  
    ''' Note:  new logic is needed to replace spSaveAndAllocateAPMassEntryFee from EDI
    '''         This function is called by the D365 settlement fees controller and by the
    '''         clsEDI210.ProcessData method.
    ''' Modified by RHR for v-8.2.0.119 on 09/27/19
    '''     we now use the new DAL.Utilities.NGLStoredProcError to identify the type of error
    '''     and perform the correct actions
    ''' Modified by RHR for v-8.2.1.004 on 12/23/2019
    '''   Moved all primary functionality and Data Access Code to the DAL so 
    '''   Legacy freight bill processing can use the same code base.  some functionality
    '''   like; runManualApprovePendingFee can only be performed in the BLL library.
    '''   ..
    '''   Calls to  bllBookRev.RecalculateUsingLineHaul(s.BookControl) must be called from the BLL so the general
    '''   logic to Save The Settlement data is split into two parts.
    '''   The legacy components mus be modify to execut this new 
    ''' Modified by RHR for v-8.2.1.004 on 12/31/2019
    '''   primary logic moved to DAL
    ''' </remarks>
    Public Function SettlementSave(ByVal s As Models.SettlementSave, ByVal ElectronicFlag As Boolean) As Models.ResultObject
        Dim oResults As New Models.ResultObject() With {.Success = True, .SuccessMsg = "Success!"}


        Try
            oResults = NGLBookData.SettlementSave(s, ElectronicFlag)
            'add logic to recalculate Costs
            If oResults.Success = True Then
                Dim bllBookRev As New BLL.NGLBookRevenueBLL(Parameters)
                Dim carrierCostResults = bllBookRev.RecalculateUsingLineHaul(oResults.BookControl)
                If ((carrierCostResults Is Nothing) OrElse (carrierCostResults.BookRevs Is Nothing OrElse carrierCostResults.BookRevs.Count < 1)) Then
                    Dim sDetails = String.Format(" SHID {0} and freight bill number {1}. ", s.BookSHID, s.InvoiceNo)
                    NGLBookData.addAuditMessage(sDetails, Nothing, oResults.Control, DAL.NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementSave"))
                    oResults.Success = False
                End If
            End If
            'logic to audit freight bill
            If oResults.Success = True Then
                oResults = NGLBookData.UpdateAndAuditAPMassEntry(oResults, oResults.Control, s.BookSHID, oResults.BookControl, s.InvoiceNo, ElectronicFlag)
            End If


        Catch ex As FaultException
            Throw
        Catch ex As Exception
            NGLAPMassEntryData.throwUnExpectedFaultException(ex, buildProcedureName("SettlementSave"), DAL.sysErrorParameters.sysErrorState.SystemLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try

        Return oResults

    End Function

  
    Public Function SettlementApproveFee(ByVal BookFeesPendingControl As Integer,
                                         ByVal BookFeesPendingBookControl As Integer,
                                         ByVal blnUnlockBFC As Boolean) As String

        'Process flow.
        '1. read AP and Pending fee data; validate status
        '2. call manualApprovePendingFee  (pass in unlock BFC parameter
        '3. call processBilledFee to update the APMassEntryFee record
        '4. call AuditFreightBill365 (need AP Control Number)
        Dim strErrMsg As String = ""
        Dim sbRetMsgs As New System.Text.StringBuilder()
        Dim iAPControl As Integer = 0
        Dim s As New Models.SettlementSave()
        Dim oFee As New Models.SettlementFee()
        Try
            Dim oPendingFee = NGLBookFeePendingData.GetLTSBookFeesPending(BookFeesPendingControl)
            If oPendingFee Is Nothing OrElse oPendingFee.BookFeesPendingControl = 0 Then
                NGLAPMassEntryData.throwNoDataFaultException("The selected pending fee record cannot be found.")
            End If

            If Not runManualApprovePendingFee(oPendingFee, blnUnlockBFC, strErrMsg) Then
                createSubscriptionAlert(strErrMsg) ' We need to identify the correct alert? 
            End If
            iAPControl = NGLAPMassEntryFeesData.UpdatePendingAPMassEntryFee(oPendingFee)
            addAuditMessage(strErrMsg, sbRetMsgs, iAPControl, DAL.NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementApproveFee--runManualApprovePendingFee"))
            strErrMsg = "" 'clear the string


            'Run the FreightBill Audit Routine
            Dim oRet = NGLAPMassEntryData.AuditFreightBill365(iAPControl, BookFeesPendingBookControl)
            If Not oRet Is Nothing AndAlso oRet.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                addAuditMessage(oRet(0).RetMsg, sbRetMsgs, iAPControl, DAL.NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("SettlementApproveFee--runManualApprovePendingFee"))
                strErrMsg = "" 'clear the string
            End If
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            NGLAPMassEntryData.throwUnExpectedFaultException(ex, buildProcedureName("SettlementApproveFee"), DAL.sysErrorParameters.sysErrorState.SystemLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try

        Return sbRetMsgs.ToString()
    End Function

  

    Public Function RunAudit365(ByVal dictAPs As Dictionary(Of Integer, String)) As Models.ResultObject
        Dim oResults As New Models.ResultObject() With {.Success = True, .SuccessMsg = "Success!"}
        Dim strErrMsg As String = ""
        Dim sbRetMsgs As New System.Text.StringBuilder()
        Try
            For Each key In dictAPs.Keys
                Dim iAPControl = key
                Dim sInvoiceNo = dictAPs(key)
                If iAPControl > 0 Then
                    Dim oRet = NGLAPMassEntryData.AuditFreightBill365(iAPControl) 'Run the FreightBill Audit Routine
                    If oRet?.Count() > 0 AndAlso oRet(0).ErrNumber <> 0 Then
                        Dim sDetails = String.Format(" {0}, freight bill number {1}. ", oRet(0).RetMsg, sInvoiceNo)
                        strErrMsg = appendToResultMessage(oResults, sDetails, Models.ResultObject.ResultMsgType.Warning, ResultProcedures.freightbill, ResultTitles.TitleAuditFreightBillWarning, ResultPrefix.MsgDetails, ResultSuffix.None)
                        addAuditMessage(strErrMsg, sbRetMsgs, iAPControl, DAL.NGLLookupDataProvider.FBLoadStatusCodes.FBUnexpected, True, buildProcedureName("RunAudit365"))
                        strErrMsg = "" 'clear the string
                    End If
                End If
            Next
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            NGLAPMassEntryData.throwUnExpectedFaultException(ex, buildProcedureName("RunAudit365"), DAL.sysErrorParameters.sysErrorState.SystemLevelFault, DAL.sysErrorParameters.sysErrorSeverity.Unexpected)
        End Try
        Return oResults
    End Function


#Region "TMS 365 Authentication Methods"

    ''' <summary>
    ''' Asychronous caller of Save365USAToken()
    ''' </summary>
    ''' <param name="sso"></param>
    ''' <remarks>
    ''' Added By LVV on 4/11/17 for v-8.0 TMS 365
    ''' </remarks>
    Public Sub Save365USATokenAsync(ByVal sso As Models.SSOResults)

        Dim fetcher As New Save365USATokenDelegate(AddressOf Me.Save365USAToken)
        ' Launch thread
        fetcher.BeginInvoke(sso, Nothing, Nothing)

    End Sub

    ''' <summary>
    ''' Calls InsertOrUpdatetblUserSecurityAccessToken and saves any errors to the AppErrors table
    ''' </summary>
    ''' <param name="sso"></param>
    ''' <remarks>
    ''' Added By LVV on 4/11/17 for v-8.0 TMS 365
    ''' </remarks>
    Public Sub Save365USAToken(ByVal sso As Models.SSOResults)
        Try
            Dim oUSAT As New DAL.NGLtblUserSecurityAccessTokenData(Parameters)
            Dim s = "Save365USATokenAsync Error: "
            Dim expires = sso.getTokenExpirationDate()

            Dim wcf = oUSAT.InsertOrUpdatetblUserSecurityAccessToken(sso.UserSecurityControl, sso.SSOAControl, sso.USATToken, expires, sso.USATUserID)

            If Not wcf Is Nothing Then
                If Not wcf.Warnings Is Nothing AndAlso wcf.Warnings.Count > 0 Then
                    Dim strMsg = wcf.concatWarnings()
                    SaveAppError(s & strMsg)
                End If
            End If

        Catch ex As Exception
            'log as system alert message
            SaveAppError("Save365USATokenAsync Error: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Creates records in the following tables:
    ''' Comp, CompContact, tblLegalEntityAdmin, and tblSubscriptionRequestPending
    ''' </summary>
    ''' <param name="ui"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by LVV on 04/11/2018 for v-8.1 VSTS Task #93 Ted Page
    ''' </remarks>
    Public Function CreateNewSignUpComp(ByVal ui As Models.FreeTrialComp) As Models.FreeTrialComp
        Dim retVal = New Models.FreeTrialComp()
        Dim CompNumber As Integer = 0
        Dim validationMsg As String = ""
        Dim nglWarningMsg As String = ""
        Try
            Dim oLEA As New DAL.NGLLegalEntityAdminData(Parameters)
            Dim oSUBPD As New DAL.NGLSubscriptionRequestPendingData(Parameters)

            '1. Check if Legal Entity already exists in tblLegalEntityAdmin
            If oLEA.DoesLegalEntityAdminExist(ui.CompLegalEntity) Then
                retVal.ValidationMsg = "The Legal Entity '" + ui.CompLegalEntity + "' already exists."
                Return retVal
            End If

            '2. Validate Comp Before Insert
            If Not NGLCompData.ValidateCompBeforeInsert(CompNumber, ui.CompName, ui.CompLegalEntity, ui.CompAlphaCode, ui.CompAbrev, validationMsg) Then
                retVal.ValidationMsg = validationMsg
                Return retVal
            End If

            Dim dtModDate = Date.Now
            Dim strModUser = Parameters.UserName

            '3. Create New Company
            Dim c As New LTS.Comp
            With c
                .CompNumber = CompNumber
                .CompName = ui.CompName
                .CompLegalEntity = ui.CompLegalEntity
                .CompAlphaCode = ui.CompAlphaCode
                .CompAbrev = ui.CompAbrev
                .CompStreetAddress1 = ui.ShipFromAddress1
                .CompStreetAddress2 = ui.ShipFromAddress2
                .CompStreetAddress3 = ui.ShipFromAddress3
                .CompStreetCity = ui.ShipFromCity
                .CompStreetState = ui.ShipFromState
                .CompStreetCountry = ui.ShipFromCountry
                .CompStreetZip = ui.ShipFromZip
                .CompEmail = ui.CompContEmail
                .CompFinCurType = 1 'Hardcoded for now
                .CompModDate = dtModDate
                .CompModUser = strModUser
            End With
            Dim dtoNewComp = NGLCompData.InsertComp(c)

            '4. Create New CompContact
            Try
                NGLCompContData.InsertOrUpdateCompContact70(dtoNewComp.CompLegalEntity, dtoNewComp.CompNumber, dtoNewComp.CompAlphaCode, ui.CompContName, ui.CompContTitle, True, ui.CompCont800, True, ui.CompContPhone, True, ui.CompContPhoneExt, True, ui.CompContFax, True, ui.CompContEmail, True)
            Catch ex As Exception
                'log as system alert message but continue processing
                SaveAppError("Source: NGLTMS365BLL.CreateNewSignUpComp, Msg: Problem creating CompCont, Error: " & ex.ToString)
            End Try

            '5. Create New LegalEntityAdmin
            ' Modified by RHR for v-8.1 on 05/10/2018 to make it easier to process default values
            Try
                oLEA.createNewLEAdmin(dtoNewComp.CompControl, dtoNewComp.CompLegalEntity)
            Catch ex As Exception
                'log as system alert message but continue processing
                SaveAppError("Source: NGLTMS365BLL.CreateNewSignUpComp, Msg: Problem creating LEAdmin for CompControl: " + dtoNewComp.CompControl + " and LegalEntity: " + dtoNewComp.CompLegalEntity + ", Error: " & ex.ToString)
                nglWarningMsg = "Problem creating LEAdmin for CompControl: " + dtoNewComp.CompControl + " and LegalEntity: " + dtoNewComp.CompLegalEntity + ". This LEAdmin must be created manually. Check AppErrors for more details." + vbCrLf
            End Try

            '6. Insert record into tblSubscriptionRequestPending for this CompControl and UserControl
            Try
                Dim SUBPD As New LTS.tblSubscriptionRequestPending
                With SUBPD
                    .SUBPDCompControl = dtoNewComp.CompControl
                    .SUBPDUserSecurityControl = Parameters.UserControl
                    .SUBPDModDate = dtModDate
                    .SUBPDModUser = strModUser
                End With
                oSUBPD.InsertSubscriptionRequestPending(SUBPD)
            Catch ex As Exception
                'log as system alert message but continue processing
                SaveAppError("Source: NGLTMS365BLL.CreateNewSignUpComp, Msg: Problem creating SubscriptionRequestPending for CompControl: " + dtoNewComp.CompControl + " and USC: " + Parameters.UserControl + ", Error: " & ex.ToString)
                nglWarningMsg += "Problem creating SubscriptionRequestPending for CompControl: " + dtoNewComp.CompControl + " and USC: " + Parameters.UserControl + ". This SubscriptionRequestPending must be created manually. Check AppErrors for more details."
            End Try

            '7. Insert the new UserSecuritySettings record
            Try
                Dim usSetting As New LTS.tblUserSecuritySetting
                With usSetting
                    .USSUserSecurityControl = Parameters.UserControl
                    .USSUserSecurityCompControl = dtoNewComp.CompControl
                    .USSCompanyName = dtoNewComp.CompName
                    .USSCompanyCity = dtoNewComp.CompStreetCity
                    .USSCompanyState = dtoNewComp.CompStreetState
                    .USSFMUserName = Parameters.UserName
                    .USSModDate = dtModDate
                    .USSModUser = strModUser
                End With
                NGLSecurityData.CreateUserSecuritySetting(usSetting)
            Catch ex As Exception
                'log as system alert message but continue processing
                SaveAppError("Source: NGLTMS365BLL.CreateNewSignUpComp, Msg: Problem creating tblUserSecuritySetting for CompControl: " + dtoNewComp.CompControl + " and USC: " + Parameters.UserControl + ", Error: " & ex.ToString)
                nglWarningMsg += "Problem creating tblUserSecuritySetting for CompControl: " + dtoNewComp.CompControl + " and USC: " + Parameters.UserControl + ". This tblUserSecuritySetting must be created manually. Check AppErrors for more details."
            End Try

            '8. Populate the return object
            With retVal
                .UserSecurityControl = Parameters.UserControl
                .CompControl = dtoNewComp.CompControl
                .CompLegalEntity = dtoNewComp.CompLegalEntity
                .CompName = dtoNewComp.CompName
                .CompNumber = dtoNewComp.CompNumber
                .ShipFromAddress1 = dtoNewComp.CompStreetAddress1
                .ShipFromAddress2 = dtoNewComp.CompStreetAddress2
                .ShipFromAddress3 = dtoNewComp.CompStreetAddress3
                .ShipFromCity = dtoNewComp.CompStreetCity
                .ShipFromState = dtoNewComp.CompStreetState
                .ShipFromZip = dtoNewComp.CompStreetZip
                .ShipFromCountry = dtoNewComp.CompStreetCountry
                .CompAbrev = dtoNewComp.CompAbrev
                .CompAlphaCode = dtoNewComp.CompAlphaCode
                .CompContName = ui.CompContName
                .CompContTitle = ui.CompContTitle
                .CompCont800 = ui.CompCont800
                .CompContPhone = ui.CompContPhone
                .CompContPhoneExt = ui.CompContPhoneExt
                .CompContFax = ui.CompContFax
                .CompContEmail = ui.CompContEmail
                .ValidationMsg = validationMsg
                .WarningMsg = nglWarningMsg
            End With

            '9. Asynchronously create default User Groups for this new LE
            CreateUserGroupsForLegalEntityAsync(dtoNewComp.CompLegalEntity, dtoNewComp.CompControl)

        Catch ex As Exception
            'log as system alert message 
            SaveAppError("Source: NGLTMS365BLL.CreateNewSignUpComp, Error: " & ex.ToString)
        End Try

        Return retVal

    End Function

    Public Sub CreateUserGroupsForLegalEntityAsync(ByVal LE As String, ByVal CompControl As Integer)

        Dim fetcher As New CreateUserGroupsForLEDelegate(AddressOf NGLSecurityData.CreateDefaultUserGroupsForLE)
        ' Launch thread
        fetcher.BeginInvoke(LE, CompControl, Nothing, Nothing)

    End Sub

    ''OLD CreateUserGroupsForLegalEntityAsync
    ''Public Sub CreateUserGroupsForLegalEntityAsync(ByVal LE As String, ByVal CompControl As Integer)

    ''    Dim fetcher As New CreateUserGroupsForLEDelegate(AddressOf Me.CreateUserGroupsForLegalEntity)
    ''    ' Launch thread
    ''    fetcher.BeginInvoke(LE, CompControl, Nothing, Nothing)

    ''End Sub

    ''''' <summary>
    ''''' Creates User Groups for LE for Admin, Standard, and Carrier
    ''''' </summary>
    ''''' <param name="LE"></param>
    ''''' <param name="CompControl"></param>
    ''''' <remarks>
    ''''' Modified by LVV on 04/11/2018 for v-8.1 VSTS Task #93 Ted Page
    '''''  Renamed some groups and also removed create groups CarrierAccountant and Accounting
    '''''  because I don't think those are necessary
    ''''' </remarks>
    ''Public Sub CreateUserGroupsForLegalEntity(ByVal LE As String, ByVal CompControl As Integer)

    ''    Dim admin As New LTS.tblUserGroup
    ''    With admin
    ''        .UserGroupsLegalEntityCompControl = CompControl
    ''        .UserGroupsName = LE + " Administrators"
    ''        .UserGroupsUGCControl = 3
    ''    End With
    ''    NGLSecurityData.CreatetblUserGroup(admin)

    ''    Dim ops As New LTS.tblUserGroup
    ''    With ops
    ''        .UserGroupsLegalEntityCompControl = CompControl
    ''        .UserGroupsName = LE + " Operations"
    ''        .UserGroupsUGCControl = 1
    ''    End With
    ''    NGLSecurityData.CreatetblUserGroup(ops)

    ''    Dim Accounting As New LTS.tblUserGroup
    ''    With Accounting
    ''        .UserGroupsLegalEntityCompControl = CompControl
    ''        .UserGroupsName = LE + " Accounting"
    ''        .UserGroupsUGCControl = 1
    ''    End With
    ''    NGLSecurityData.CreatetblUserGroup(Accounting)

    ''    Dim cariers As New LTS.tblUserGroup
    ''    With cariers
    ''        .UserGroupsLegalEntityCompControl = CompControl
    ''        .UserGroupsName = LE + " Carriers"
    ''        .UserGroupsUGCControl = 2
    ''    End With
    ''    NGLSecurityData.CreatetblUserGroup(cariers)

    ''    Dim carrAccountants As New LTS.tblUserGroup
    ''    With carrAccountants
    ''        .UserGroupsLegalEntityCompControl = CompControl
    ''        .UserGroupsName = LE + " Carrier Accountants"
    ''        .UserGroupsUGCControl = 2
    ''    End With
    ''    NGLSecurityData.CreatetblUserGroup(carrAccountants)

    ''End Sub



    Public Function GetPendingSignUpInfo(ByVal UserControl As Integer) As Models.FreeTrialComp
        Dim compInfo As New Models.FreeTrialComp
        Dim oSUBPD As New DAL.NGLSubscriptionRequestPendingData(Parameters)

        Dim vsub = oSUBPD.GetSubscriptionRequest(UserControl)


        If Not vsub Is Nothing AndAlso vsub.SUBPDCompControl <> 0 Then
            Dim c = NGLCompData.GetCompFiltered(Control:=vsub.SUBPDCompControl)
            Dim cont = NGLCompContData.GetFirstCompContFiltered(c.CompControl)

            With compInfo
                .UserSecurityControl = UserControl
                .CompControl = c.CompControl
                .CompLegalEntity = c.CompLegalEntity
                .CompName = c.CompName
                .CompNumber = c.CompNumber
                .ShipFromAddress1 = c.CompStreetAddress1
                .ShipFromAddress2 = c.CompStreetAddress2
                .ShipFromAddress3 = c.CompStreetAddress3
                .ShipFromCity = c.CompStreetCity
                .ShipFromState = c.CompStreetState
                .ShipFromZip = c.CompStreetZip
                .ShipFromCountry = c.CompStreetCountry
                .CompAbrev = c.CompAbrev
                .CompAlphaCode = c.CompAlphaCode
                .CompContName = cont.CompContName
                .CompContTitle = cont.CompContTitle
                .CompCont800 = cont.CompCont800
                .CompContPhone = cont.CompContPhone
                .CompContPhoneExt = cont.CompContPhoneExt
                .CompContFax = cont.CompContFax
                .CompContEmail = cont.CompContEmail
                .ValidationMsg = ""
            End With
        End If

        Return compInfo
    End Function

    ''' <summary>
    ''' Asychronous caller of CheckFreeTrialExpiration()
    ''' </summary>
    ''' <param name="UserControl"></param>
    ''' <remarks>
    ''' Added By LVV on 7/12/17 for v-8.0 TMS 365
    ''' </remarks>
    Public Sub CheckFreeTrialExpirationAsync(ByVal UserControl As Integer)

        Dim fetcher As New CheckFreeTrialExpirationDelegate(AddressOf Me.CheckFreeTrialExpiration)
        ' Launch thread
        fetcher.BeginInvoke(UserControl, Nothing, Nothing)

    End Sub

    ''' <summary>
    ''' Checks if the user is a Free Trial User and if so checks to see if the free trial is still active.
    ''' If the free trial is expired, restrict the user from accessing the Rate Shopping page.
    ''' </summary>
    ''' <param name="UserControl"></param>
    ''' <remarks>
    ''' Added By LVV on 7/12/17 for v-8.0 TMS 365
    ''' </remarks>
    Public Sub CheckFreeTrialExpiration(ByVal UserControl As Integer)
        Try
            Dim oSec As New DAL.NGLSecurityDataProvider(Parameters)
            Dim usec = oSec.GettblUserSecurity(UserControl)

            If usec.UserGroupsName.Equals("Free Trial") Then
                If (Not usec.UserFreeTrialActive) OrElse ((Not usec.UserEndFreeTrial.HasValue) OrElse (usec.UserEndFreeTrial.Value < Date.Now)) Then
                    'ft is expired so restrict user from seeing rate shop page
                    oSec.RestrictFormForUser365("365 Rate Shopping", UserControl)
                End If
            End If

        Catch ex As Exception
            'log as system alert message
            SaveAppError("CheckFreeTrialExpirationAsync Error: " & ex.Message)
        End Try
    End Sub


#End Region



#Region "Stop Resequence/Get Miles"

    Public Function StopResequence(ByVal iBookControl As Integer, ByVal blnKeepStopNumbers As Boolean) As Models.ResultObject
        Dim retVal As New Models.ResultObject
        retVal.Success = True
        Dim intPCMReturn As Integer
        Dim blnShowBadAddressWindow As Boolean = False
        Dim dblBatchID As Double = CDbl(Format(Now(), "MddyyyyHHmmss"))
        Try
            Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)

            Dim book = NGLBookData.GetBookFilteredNoChildren(iBookControl)

            If book Is Nothing Then
                retVal.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGBookNotSelectedWarning", "Warning! Invalid Book Control Number. You must select a booking record before performing this operation.")
                retVal.Success = False
                Return retVal
            End If
            If book.BookLockAllCosts = True Then
                If blnKeepStopNumbers Then
                    retVal.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGPCMGetMilesAllCostsLocked", "Stop distance cannot be recalculated while costs are locked.")
                Else
                    retVal.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGPCMStopReSyncAllCostsLocked", "Stops cannot be resequenced while costs are locked.")
                End If
                retVal.Success = False
                Return retVal
            End If
            If book.BookTranCode <> "N" AndAlso book.BookTranCode <> "P" Then
                If blnKeepStopNumbers Then
                    retVal.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGPCMGetMilesNotNPStatus", "Stop distance can only be recalculated while in N or P status.")
                Else
                    retVal.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGPCMStopReSyncNotNPStatus", "Stops can only be resequenced while in N or P status.")
                End If
                retVal.Success = False
                Return retVal
            End If
            If String.IsNullOrEmpty(book.BookConsPrefix) OrElse book.BookConsPrefix.Trim.Length < 1 Then

                If blnKeepStopNumbers Then
                    retVal = CalculateAndSaveBookMiles(book, iBookControl)
                    'retVal.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGPCMGetMilesConsRequired", "Consolidation Number Required for Getting Stop Distances! Please select a Route/Consolidation number before selecting this option.")
                Else
                    retVal.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGPCMStopReSyncConsRequired", "Consolidation Number Required for Stop Resequencing! Please select a Route/Consolidation number before selecting this option.")
                End If
                retVal.Success = False
                Return retVal
            End If

            'Call Resequence
            intPCMReturn = StopResequenceEx(retVal, book.BookConsPrefix, book.BookCarrierControl, dblBatchID, blnKeepStopNumbers, False, True)

            'Updates Status and Show Messages
            If intPCMReturn > 0 Then
                If blnKeepStopNumbers Then
                    retVal.SuccessMsg = oLocalize.GetLocalizedValueByKey("MSGPCMGetMilesSuccess", "Success! Your stops distance has been recalculated.")
                Else
                    retVal.SuccessMsg = oLocalize.GetLocalizedValueByKey("MSGPCMStopReSyncSuccess", "Success! Your stops have been resequenced.")
                End If
            ElseIf intPCMReturn < 0 Then
                If NGLBatchProcessData.GetParValue("PCMilerShowResynAddressWarnings", 0) > 0 Then
                    blnShowBadAddressWindow = True
                    Dim sSep = ""
                    If Not String.IsNullOrWhiteSpace(retVal.WarningMsg) Then sSep = " "
                    retVal.WarningMsg += String.Format(oLocalize.GetLocalizedValueByKey("MSGPCMBadAddressWarning", "Warning! TMS found {0} address problems. Your address may require special attention."), intPCMReturn * -1)
                Else
                    If blnKeepStopNumbers Then
                        retVal.SuccessMsg = oLocalize.GetLocalizedValueByKey("MSGPCMGetMilesSuccess", "Success! Your stops distance has been recalculated, but one or more of the street addresses is not valid using closest match.")
                    Else
                        retVal.SuccessMsg = oLocalize.GetLocalizedValueByKey("MSGPCMStopReSyncSuccess", "Success! Your stops have been resequenced, but one or more of the street addresses is not valid using closest match.")
                    End If
                End If
            Else
                ''UpdateStatus("Failed")
                If blnKeepStopNumbers Then
                    retVal.ErrMsg = String.Format(oLocalize.GetLocalizedValueByKey("MSGPCMGetMilesFailedWarning", "Get Practical Distance Failure! There was a problem with PC Miler: {0}"), "")
                    retVal.Success = False
                Else
                    retVal.ErrMsg = String.Format(oLocalize.GetLocalizedValueByKey("MSGPCMStopReSyncFailedWarning", "PC Miler Resequence Failure! There was a problem with PC Miler: {0}"), "")
                    retVal.Success = False
                End If
            End If
        Catch IOE As InvalidOperationException
            SaveAppError("Source: NGLTMS365BLL.StopResequence, Error: " & IOE.Message) ''PaneSettings.MainInterface.FMMessage.ShowFMError(PaneSettings.MainInterface.LocalizeString(IOE.Message))
        Catch ex As Exception
            SaveAppError("Source: NGLTMS365BLL.StopResequence, Error: " & ex.Message) ''PaneSettings.MainInterface.FMMessage.ShowFMError(ex.Message)
        Finally
            'display the bad address screen.
            'If blnShowBadAddressWindow Then
            '    Dim win As New AddressWarningReport(PaneSettings.MainInterface, My.Settings.Theme, dblBatchID)
            '    win.Show()
            'End If
        End Try
        Return retVal
    End Function

    ''' <summary>
    ''' Caller must catch the following exceptions
    ''' InvalidOperationException
    ''' Unhandled Exception
    ''' returns positive numbers represent the number of stops updated.  
    ''' negative numbers represent the number of bad addresses 
    ''' 0 indicates a failure
    ''' </summary>
    ''' <param name="BookConsPrefix"></param>
    ''' <param name="BookCarrierControl"></param>
    ''' <returns>positive numbers represent the number of stops updated.  
    ''' negative numbers represent the number of bad addresses 
    ''' 0 indicates a failure </returns>
    ''' <remarks></remarks>
    Public Function StopResequenceEx(ByRef retVal As Models.ResultObject,
                                    ByVal BookConsPrefix As String,
                                     ByVal BookCarrierControl As Integer,
                                     ByVal dblBatchID As Double,
                                    Optional ByVal blnKeepStopNumbers As Boolean = False,
                                    Optional ByVal Silent As Boolean = False,
                                    Optional ByVal SortByStopNumber As Boolean = False) As Integer
        Dim intRet As Integer = 0
        Dim sSep = ""
        If retVal Is Nothing Then retVal = New Models.ResultObject()
        Try
            Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
            Dim sPCMErrors As New List(Of String)
            retVal.addToLogList("Start processing Stop Resequence Logic", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
            retVal.addToLogList("Keep stop numbers = " & blnKeepStopNumbers.ToString(), 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
            retVal.addToLogList("Run silent  = " & Silent.ToString(), 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
            retVal.addToLogList("Sort by stop number  = " & SortByStopNumber.ToString(), 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
            Dim oBadAddresses As New PCM.clsPCMBadAddresses()
            Dim oPCMReturn As PCM.clsPCMReturnEx = PCMilerBLL.PCMReSyncMultiStop(BookConsPrefix, dblBatchID, sPCMErrors, blnKeepStopNumbers, Silent, SortByStopNumber, oBadAddresses)
            If Not sPCMErrors Is Nothing AndAlso sPCMErrors.Count() > 0 Then
                retVal.addToLogList("Stop resequence had errors; see error messages", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")

                For Each e In sPCMErrors
                    retVal.Err.Add(New DTO.NGLMessage(e))
                    'retVal.WarningMsg += sSep + e
                    'sSep = " "
                Next
            End If
            If oPCMReturn Is Nothing Then
                retVal.addToLogList("PCM Stop Resequence returned no data, the process cannot continue", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
                Return intRet
            End If

            If oPCMReturn.BadAddressCount <> 0 Then
                If Not oBadAddresses Is Nothing AndAlso oBadAddresses.COUNT > 0 Then
                    retVal.addToLogList("PCM Stop Resequence had bad address data, see Bad Address Messages", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
                    retVal.MsgTitle = "Bad Addresses"

                    Dim oBaddAddress As PCM.clsPCMBadAddress
                    For intBadAddressCT = 1 To oBadAddresses.COUNT
                        oBaddAddress = oBadAddresses.Item(intBadAddressCT)
                        '        Public Sub New(ByVal m As String, ByVal c As Int64, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef, ByVal eReason As String, ByVal eMessage As String, ByVal eDetails As String)
                        Dim sOrigOrDest = ""
                        Dim sAddressDetails As String = "Undefined"
                        If Not oBaddAddress.objOrig Is Nothing AndAlso Not String.IsNullOrWhiteSpace(oBaddAddress.objOrig.strAddress) Then
                            sOrigOrDest = "Pickup Location"
                            With oBaddAddress.objOrig
                                sAddressDetails = String.Concat(.strAddress, " ", .strCity, ", ", .strState, "  ", .strZip)
                            End With
                        ElseIf Not oBaddAddress.objDest Is Nothing Then
                            sOrigOrDest = "Delivery Location"
                            With oBaddAddress.objDest
                                sAddressDetails = String.Concat(.strAddress, " ", .strCity, ", ", .strState, "  ", .strZip)
                            End With
                        End If
                        retVal.Message.Add(New DTO.NGLMessage(sAddressDetails, 0, "", DAL.Utilities.NGLMessageKeyRef.None, oBaddAddress.Message, sOrigOrDest, ""))
                    Next

                Else
                    retVal.addToLogList("PCM Stop Resequence bad address count = " & oPCMReturn.BadAddressCount.ToString(), 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
                End If
            End If
            If Not String.IsNullOrWhiteSpace(oPCMReturn.Message) Then
                retVal.addToLogList("PCM Stop Resequence Messages, check the warning messages", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
                retVal.Warn.Add(New DTO.NGLMessage(oPCMReturn.Message))
            End If
            If Len(Trim(oPCMReturn.FailedAddressMessage)) > 0 Then
                retVal.addToLogList("PCM Stop Resequence returned a failed address warning, check the warning messages and the bad address list", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
                retVal.Warn.Add(New DTO.NGLMessage(oPCMReturn.FailedAddressMessage)) ''PaneSettings.MainInterface.FMMessage.ShowFMMessage(oPCMReturn.FailedAddressMessage)
            End If
            Dim oFMStops As List(Of PCM.clsFMStopData) = TryCast(oPCMReturn.Results, List(Of PCM.clsFMStopData))
            If oFMStops Is Nothing Then
                retVal.addToLogList("PCM Stop Resequence returned no stop data, the process cannot continue", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
                Return intRet
            End If

            If oFMStops.Count < 1 Then
                retVal.addToLogList("PCM Stop Resequence returned no stop data, the process cannot continue", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
                Return intRet
            End If


            retVal.addToLogList("PCM Stop Resequence total miles =  " & oPCMReturn.TotalMiles.ToString(), 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
            If Not String.IsNullOrWhiteSpace(oPCMReturn.LastError) Then
                retVal.addToLogList("PCM Stop Resequence last Error = " & oPCMReturn.LastError, 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")
            End If
            retVal.addToLogList("Stop Resequence updating results", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")

            If UpdateBookConsMultiPickPCMilerTwoWay(oFMStops) Then
                If Not blnKeepStopNumbers Then
                    'update the pick numbers
                    Me.Parameters.ProcedureName = "UpdateBookConsPickNumberTwoWay"
                    NGLBatchProcessData.UpdateBookConsPickNumber(BookConsPrefix, True)
                    Me.Parameters.ProcedureName = ""
                End If
                'Check for carrier and re-calculate costs
                If BookCarrierControl > 0 Then
                    If Not updateConsCarrier2Way(oFMStops(0).BookControl, BookCarrierControl) Then
                        retVal.WarningMsg += sSep + oLocalize.GetLocalizedValueByKey("MSGRecalcCostFailedWarning", "Warning! Carrier costs have Not been recalculated. You must recalculate costs manually.") ''Dim message As String = PaneSettings.MainInterface.LocalizeString("MSGRecalcCostFailedWarning")
                    End If
                End If
                If oPCMReturn.BadAddressCount > 0 Then
                    intRet = oPCMReturn.BadAddressCount * -1
                Else
                    intRet = oFMStops.Count
                End If
            End If
            retVal.addToLogList("Stop Resequence complete", 0, BookConsPrefix, DAL.Utilities.NGLMessageKeyRef.None, "Consolidation Number")

        Catch ex As Exception
            Throw
        End Try
        Return intRet
    End Function

    Private Function UpdateBookConsMultiPickPCMilerTwoWay(ByVal oFMStops As List(Of PCM.clsFMStopData)) As Boolean
        Dim blnRet As Boolean = True
        For Each oStop In oFMStops
            If Not blnRet Then Exit For
            With oStop
                Me.Parameters.ProcedureName = "UpdateBookConsMultiPickPCMiler2Way"
                blnRet = NGLBatchProcessData.UpdateBookConsMultiPickPCMiler(.BookControl, .LocationisOrigin, .StopNumber, .LegMiles, .LegCost, .LegTime, .LegTolls, .LegESTCHG, True)
            End With
        Next
        Me.Parameters.ProcedureName = ""
        Return blnRet
    End Function

    Private Function updateConsCarrier2Way(ByVal BookControl As Integer, ByVal BookCarrierControl As Integer) As Boolean
        Me.Parameters.ProcedureName = "updateConsCarrier2Way"
        Return (New BLL.NGLBookRevenueBLL(Parameters)).AssignOrUpdateCarrier(BookControl).Success
    End Function

#End Region

#Region "Booking Menu"


    ''' <summary>
    ''' ** CALLER NEEDS TO MAKE SURE TO KICK BACK Models.ResultObject.validationLong -- original set to 0 **
    ''' </summary>
    ''' <param name="bookControl"></param>
    ''' <param name="action"></param>
    ''' <returns></returns>
    Public Function ExecBookingMenu365(ByVal bookControl As Integer, ByVal action As BookingActions) As Models.ResultObject
        Try
            Dim result As New Models.ResultObject
            Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
            Dim oBookBLL As New BLL.NGLBookBLL(Parameters)

            Dim book = NGLBookData.GetBookFilteredNoChildren(bookControl)

            If book Is Nothing Then
                result.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGBookNotSelectedWarning", "Warning! Invalid Book Control Number. You must select a booking record before performing this operation.")
                result.Success = False
                Return result
            End If

            If Not CanSaveBook(result, True, book.BookControl, book.BookTranCode, book.BookDateOrdered, book.BookDateLoad, book.BookDateRequired, book.BookFinARInvoiceDate) Then
                result.ErrMsg = oLocalize.GetLocalizedValueByKey("MSGCannotChangeTranCode", "Cannot Change Transaction Code")
                result.Success = False
                Return result
            End If

            Dim actionsList = GetAvailableBookingActions(book.BookControl, book.BookTranCode, book.BookDateOrdered, book.BookDateRequired, book.BookDateLoad, book.BookFinARInvoiceDate, book.BookCarrierControl, book.BookLockAllCosts, book.BookSHID, book.BookDateDelivered, book.BookRouteConsFlag)

            If Not actionsList.Contains(action) Then
                result.ErrMsg = oLocalize.GetLocalizedValueByKey("BookingTenderOptionUnavailable", "Action is not available for this load.")
                result.Success = False
                Return result
            End If

            'Finish this function when we figure out how to send user selections back and forth to the widget
            ProcessBookingActionValidation(action, book.BookTranCode, book.BookLockBFCCost, book.BookLockAllCosts)

            Select Case action
                Case BookingActions.UnassignCarrier, BookingActions.Reject, BookingActions.DropLoad
                    UpdateBookTranCode365(result, bookControl, "N", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case BookingActions.RemoveOrder
                    oBookBLL.RemoveOrderFromLoad(bookControl)
                    result.Success = True
                    Return result
                Case BookingActions.Modify
                    UpdateBookTranCode365(result, bookControl, "P", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case BookingActions.Tender
                    UpdateBookTranCode365(result, bookControl, "PC", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case BookingActions.AcceptFinalize
                    UpdateBookTranCode365(result, bookControl, "PB", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case BookingActions.AcceptAll ' "ynAcceptAll" Modified by RHR for v-8.4.0.005 on 02/21/2022 ynAcceptAll is not valid
                    Console.WriteLine("ynAcceptAll")
                    UpdateBookTranCode365(result, bookControl, "PB", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case BookingActions.Invoice
                    UpdateBookTranCode365(result, bookControl, "I", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case BookingActions.InvoiceComplete
                    UpdateBookTranCode365(result, bookControl, "IC", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case BookingActions.InvoiceSingle
                    UpdateBookTranCodeSingle365(result, bookControl, "I", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case BookingActions.InvoiceCompleteSingle
                    UpdateBookTranCodeSingle365(result, bookControl, "IC", book.BookTranCode, book.BookCarrOrderNumber, book.BookOrderSequence)
                    Return result
                Case Else
                    'do nothing
            End Select

            Return result
        Catch ex As Exception
            SaveAppError("Source: NGLTMS365BLL.ExecBookingMenu365, Error: " & ex.Message) ''PaneSettings.MainInterface.FMMessage.ShowFMError(ex.Message)
            Throw
        End Try
        Return Nothing
    End Function

    Public Sub UpdateBookTranCode365(ByRef result As Models.ResultObject, ByVal bookControl As Integer, ByVal newTranCode As String, ByVal bookTranCode As String, ByVal orderNumber As String, ByVal orderSeq As Integer)
        Try
            Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
            Dim oBookBLL As New BLL.NGLBookBLL(Parameters)

            Dim wcfRes = oBookBLL.ProcessNewTransCode(bookControl, newTranCode, bookTranCode, result.validationLong)
            'add errors, warning, messages, annd logs to result
            addWCFMessagesToResultObj(result, wcfRes, "Update Tran Code Log")
            If Not wcfRes.Success Or (wcfRes.Action = DTO.WCFResults.ActionEnum.ShowValidationMsg) Then
                'show stuff here, then try again if they want to try again.
                If wcfRes.Action = DTO.WCFResults.ActionEnum.ShowValidationMsg Then
                    'show validation messages and question 
                    result.ynQuestion = oLocalize.GetLocalizedValueByKey("TranCodeChangeFailedValidationQuestion", "Unable to complete request due to validation rules. Would you like to try again without validating?")
                    result.Success = False
                    result.validationLong = wcfRes.acceptLastValidationFlag()
                    Return
                Else


                    'just show messages  
                    result.ErrMsg = oLocalize.GetLocalizedValueByKey("TranCodeChangeFailedValidation", "Unable to complete request, please review the messages.")
                    result.Success = False
                    Return
                End If
            Else
                If wcfRes.Errors?.Count > 0 Then
                    result.ErrMsg = wcfRes.concatErrors()
                    result.SuccessMsg = False
                    Return
                End If
                If wcfRes.Messages?.Count > 0 Then result.Msg = wcfRes.concatMessage()
                If wcfRes.Warnings?.Count > 0 Then result.WarningMsg = wcfRes.concatWarnings()
                result.SuccessMsg = oLocalize.GetLocalizedValueByKey("StatusMsgTransCodeSaved", "Changes to your transaction code have been saved!") & " - " & orderNumber & "-" & orderSeq
                result.Success = True
                Return
            End If
        Catch ex As Exception
            SaveAppError("Source: NGLTMS365BLL.UpdateBookTranCode365, Error: " & ex.Message) ''PaneSettings.MainInterface.FMMessage.ShowFMError(ex.Message)
            Throw
        End Try
    End Sub

    Private Function CanSaveBook(ByRef result As Models.ResultObject, ByVal displayMessage As Boolean, ByVal BookControl As Integer, ByVal BookTranCode As String, ByVal BookDateOrdered As Date?, ByVal BookDateLoad As Date?, ByVal BookDateRequired As Date?, ByVal BookFinARInvoiceDate As Date?) As Boolean
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim blnRet As Boolean = True
        If BookControl = 0 Then Return False
        If BookTranCode = "P" Or BookTranCode = "PC" Or BookTranCode = "I" Or BookTranCode = "IC" Then
            If BookDateOrdered.HasValue = True Then
                If BookDateLoad.HasValue = True Then
                    If BookDateRequired.HasValue = True Then
                        If BookTranCode = "I" Or BookTranCode = "IC" Then
                            If BookTranCode = "IC" Then
                                If BookFinARInvoiceDate.HasValue = False Then
                                    result.Success = False
                                    result.ErrMsg = oLocalize.GetLocalizedValueByKey("LoadDetailSaveInvoiceDateValid", "Invoice Date Required for IC type trancode")
                                    blnRet = False
                                End If
                            End If
                        End If
                    Else
                        result.Success = False
                        result.ErrMsg = oLocalize.GetLocalizedValueByKey("LoadDetailSaveDateRequiredValid", "Date Required is Required for any trancode beyond 'N'")
                        blnRet = False
                    End If
                Else
                    result.Success = False
                    result.ErrMsg = oLocalize.GetLocalizedValueByKey("LoadDetailSaveDateLoadValid", "Date to Load is required for any trancode beyond 'N'")
                    blnRet = False
                End If
            Else
                result.Success = False
                result.ErrMsg = oLocalize.GetLocalizedValueByKey("LoadDetailSaveDateOrderedValid", "Date Ordered is required for any trancode beyond 'N'")
                blnRet = False
            End If
        End If
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="BookTranCode"></param>
    ''' <param name="BookDateOrdered"></param>
    ''' <param name="BookDateRequired"></param>
    ''' <param name="BookDateLoad"></param>
    ''' <param name="BookFinARInvoiceDate"></param>
    ''' <param name="BookCarrierControl"></param>
    ''' <param name="BookLockAllCosts"></param>
    ''' <param name="BookSHID"></param>
    ''' <param name="BookDateDelivered"></param>
    ''' <param name="BookRouteConsFlag"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.003 on 10/23/2023
    '''     added logic to remove order from load if carrier is zero and order is in P
    '''     normally should not happen but some cases have been found where the carrier was removed
    '''     and the users cannot correct the load,  we still need to determine how the carrier was remove
    ''' </remarks>
    Public Function GetAvailableBookingActions(ByVal BookControl As Integer,
                                               ByVal BookTranCode As String,
                                               ByVal BookDateOrdered As Nullable(Of Date),
                                               ByVal BookDateRequired As Nullable(Of Date),
                                               ByVal BookDateLoad As Nullable(Of Date),
                                               ByVal BookFinARInvoiceDate As Nullable(Of Date),
                                               ByVal BookCarrierControl As Integer,
                                               ByVal BookLockAllCosts As Boolean,
                                               ByVal BookSHID As String,
                                               Optional ByVal BookDateDelivered As Nullable(Of Date) = Nothing,
                                               Optional ByVal BookRouteConsFlag As Integer? = Nothing) As List(Of BookingActions)
        Dim result As New List(Of BookingActions)
        'Added By LVV 8/2/16 for v-7.0.5.110 #Invoice All
        Dim blnCanSingle As Boolean = False
        If Not BookDateDelivered Is Nothing Then
            If BookDateDelivered < Date.Now AndAlso BookRouteConsFlag = 0 Then
                blnCanSingle = True
            End If
        End If

        If BookControl = 0 Then
            result.Add(BookingActions.None)
            Return result
        End If
        If String.IsNullOrEmpty(BookTranCode) Then
            result.Add(BookingActions.None)
            Return result
        End If
        If BookDateLoad.HasValue = False Then
            result.Add(BookingActions.None)
            Return result
        End If
        If BookDateOrdered.HasValue = False Then
            result.Add(BookingActions.None)
            Return result
        End If
        If BookDateRequired.HasValue = False Then
            result.Add(BookingActions.None)
            Return result
        End If
        Select Case BookTranCode
            Case "N"
                result.Add(BookingActions.SelectCarrier)
                result.Add(BookingActions.SpotRate)
                result.Add(BookingActions.RemoveOrder)
            Case "P"
                If BookCarrierControl > 0 Then
                    result.Add(BookingActions.RemoveOrder)
                    result.Add(BookingActions.UnassignCarrier)
                    If BookLockAllCosts = False Then
                        If String.IsNullOrEmpty(BookSHID) Then 'they have to reject the load if a shid is already assigned so the system will create a new shid for the new carrier.
                            result.Add(BookingActions.SelectCarrier)
                        Else
                            result.Add(BookingActions.Reject)
                        End If
                        'Modified by RHR 7.0.5.100 05/06/2016
                        result.Add(BookingActions.SpotRate)
                    End If
                    result.Add(BookingActions.Tender)
                    result.Add(BookingActions.AcceptFinalize)
                    result.Add(BookingActions.Invoice)
                    'Added By LVV 8/2/16 for v-7.0.5.110 #Invoice All
                    If blnCanSingle Then result.Add(BookingActions.InvoiceSingle)
                Else
                    'Added By LVV 11/10/16 for v-7.0.5.110 P Status Assign Carrier Fix
                    result.Add(BookingActions.SelectCarrier)
                    result.Add(BookingActions.SpotRate)
                    'result.Add(BookingActions.None)
                    result.Add(BookingActions.RemoveOrder) 'Modified by RHR for v-8.5.4.003 on 10/23/2023
                End If
            Case "PC"
                If BookCarrierControl > 0 Then
                    result.Add(BookingActions.Reject)
                    result.Add(BookingActions.Modify)
                    result.Add(BookingActions.AcceptFinalize)
                    result.Add(BookingActions.Invoice)
                    'Added By LVV 8/2/16 for v-7.0.5.110 #Invoice All
                    If blnCanSingle Then result.Add(BookingActions.InvoiceSingle)
                Else
                    result.Add(BookingActions.RemoveOrder) 'P-->N 
                    result.Add(BookingActions.UnassignCarrier) 'P-->N  
                    result.Add(BookingActions.Reject) 'PC-->N 
                End If
            Case "PB"
                If BookCarrierControl > 0 Then
                    result.Add(BookingActions.DropLoad)
                    result.Add(BookingActions.Modify)
                    result.Add(BookingActions.Invoice)
                    'Added By LVV 8/2/16 for v-7.0.5.110 #Invoice All
                    If blnCanSingle Then result.Add(BookingActions.InvoiceSingle)
                Else
                    result.Add(BookingActions.RemoveOrder) 'P-->N 
                    result.Add(BookingActions.UnassignCarrier) 'P-->N  
                    result.Add(BookingActions.Reject) 'PC-->N 
                    result.Add(BookingActions.DropLoad) 'PB-->N 
                End If
                'Modified by RHR 7.0.5.100 05/06/2016
                If BookLockAllCosts = False Then result.Add(BookingActions.SpotRate)
            Case "I"
                If BookCarrierControl > 0 Then
                    result.Add(BookingActions.Reject)
                    result.Add(BookingActions.Modify)
                    If BookFinARInvoiceDate.HasValue Then
                        result.Add(BookingActions.InvoiceComplete)
                        'Added By LVV 8/2/16 for v-7.0.5.110 #Invoice All
                        If blnCanSingle Then result.Add(BookingActions.InvoiceCompleteSingle)
                    End If
                Else
                    result.Add(BookingActions.None)
                End If
            Case "IC"
                If BookCarrierControl > 0 Then
                    result.Add(BookingActions.Reject)
                    result.Add(BookingActions.Modify)
                    result.Add(BookingActions.Invoice)
                    'Added By LVV 8/2/16 for v-7.0.5.110 #Invoice All
                    If blnCanSingle Then result.Add(BookingActions.InvoiceSingle)
                Else
                    result.Add(BookingActions.None)
                End If
            Case Else
                result.Add(BookingActions.None)
        End Select
        Return result
    End Function

    ''' <summary>
    ''' Gets the single BookRevenue object for the provided Book Control
    ''' and calls ProcessNewTranCode with that single BookRev
    ''' Does not get dependant BookControls
    ''' </summary>
    ''' <param name="result"></param>
    ''' <param name="bookControl"></param>
    ''' <param name="newTranCode"></param>
    ''' <param name="bookTranCode"></param>
    ''' <param name="orderNumber"></param>
    ''' <param name="orderSeq"></param>
    Public Sub UpdateBookTranCodeSingle365(ByRef result As Models.ResultObject, ByVal bookControl As Integer, ByVal newTranCode As String, ByVal bookTranCode As String, ByVal orderNumber As String, ByVal orderSeq As Integer)
        Try
            Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
            Dim oBookBLL As New BLL.NGLBookBLL(Parameters)

            Dim bRev = NGLBookRevenueData.GetBookRevenueFiltered(bookControl)
            Dim bookRevs = New DTO.BookRevenue() {bRev}

            Dim wcfRes As New DTO.WCFResults With {.Key = bookControl, .Success = True}
            wcfRes.setAction(FreightMaster.Data.DataTransferObjects.WCFResults.ActionEnum.DoNothing)
            wcfRes.AddLog("Get the Booking records.")

            wcfRes = oBookBLL.ProcessNewTransCode(bookRevs, bookControl, newTranCode, bookTranCode, wcfRes, result.validationLong)
            If Not wcfRes.Success Or (wcfRes.Action = DTO.WCFResults.ActionEnum.ShowValidationMsg) Then
                'show stuff here, then try again if they want to try again.
                If wcfRes.Action = DTO.WCFResults.ActionEnum.ShowValidationMsg Then
                    'show validation messages and question 
                    result.ynQuestion = oLocalize.GetLocalizedValueByKey("TranCodeChangeFailedValidationQuestion", "Unable to complete request due to validation rules. Would you like to try again without validating?")
                    result.Success = False
                    result.validationLong = wcfRes.acceptLastValidationFlag()
                    Return
                Else
                    'just show messages  
                    result.ErrMsg = oLocalize.GetLocalizedValueByKey("TranCodeChangeFailedValidation", "Unable to complete request, please review the messages.")
                    result.Success = False
                    Return
                End If
            Else
                If wcfRes.Errors?.Count > 0 Then
                    result.ErrMsg = wcfRes.concatErrors()
                    result.SuccessMsg = False
                    Return
                End If
                If wcfRes.Messages?.Count > 0 Then result.Msg = wcfRes.concatMessage()
                If wcfRes.Warnings?.Count > 0 Then result.WarningMsg = wcfRes.concatWarnings()
                result.SuccessMsg = oLocalize.GetLocalizedValueByKey("StatusMsgTransCodeSaved", "Changes to your transaction code have been saved!") & " - " & orderNumber & "-" & orderSeq
                result.Success = True
                Return
            End If
        Catch ex As Exception
            SaveAppError("Source: NGLTMS365BLL.UpdateBookTranCodeSingle365, Error: " & ex.Message) ''PaneSettings.MainInterface.FMMessage.ShowFMError(ex.Message)
            Throw
        End Try
    End Sub



    ''DO THIS ON 365 CLIENT SIDE
    Private Sub ProcessBookingActionValidation(ByVal action As BookingActions, ByVal bookTranCode As String, ByVal BookLockBFCCost As Boolean, ByVal BookLockAllCosts As Boolean)
        ''Dim result As New UpdateTrancodeValidation()
        ''result.ContinueProcess = True

        '''Going from IC to anything, show message
        ''If Not action = BookingActions.InvoiceComplete And
        ''  bookTranCode = "IC" Then
        ''    If (New FMMessageBoxPrompt).ShowFMMessageBox(Me,
        ''                                                                 FMMessageBox.DialogTypes.OkCancel,
        ''                                                                 FMMessageBox.ImportanceLevels.Question,
        ''                                                                 LocalizeString("MSGChangedFromICToOther"),
        ''                                                                  False) = DialogResults.No Then
        ''        result.ContinueProcess = False
        ''        result.ModifyCostFlags = False
        ''        Return result
        ''    End If
        ''End If

        ''If action = BookingActions.Modify And bookTranCode = "PB" Then
        ''    If (New FMMessageBoxPrompt).ShowFMMessageBox(Me,
        ''                                          FMMessageBox.DialogTypes.OkCancel,
        ''                                          FMMessageBox.ImportanceLevels.Question,
        ''                                          LocalizeString("MSGundoFinalizeBooking"),
        ''                                           False) = DialogResults.No Then
        ''        result.ContinueProcess = False
        ''        result.ModifyCostFlags = False
        ''        Return result
        ''    End If
        ''End If
        ''Return result
    End Sub

#End Region


#Region "Lane"

    Public Function CalculateAndSaveLaneMilesLatLong(ByVal LaneControl As Integer) As Models.ResultObject
        Dim res As New Models.ResultObject
        res.Success = True
        Try
            'Get the Lane data using the LaneControl
            Dim RecordCount = 0
            Dim filters As New Models.AllFilters
            filters.filterName = "LaneControl"
            filters.filterValue = LaneControl
            Dim oData = NGLLaneData.GetLELane365(RecordCount, filters)
            If oData?.Count < 1 OrElse oData(0) Is Nothing OrElse oData(0).LaneControl = 0 Then Return Nothing
            Dim objOrig As New PCM.clsAddress
            With objOrig
                .strZip = Trim(oData(0).LaneOrigZip)
                .strAddress = Trim(oData(0).LaneOrigAddress1)
                .strState = Trim(oData(0).LaneOrigState)
                .strCity = Trim(oData(0).LaneOrigCity)
            End With
            Dim objDest As New PCM.clsAddress
            With objDest
                .strZip = Trim(oData(0).LaneDestZip)
                .strAddress = Trim(oData(0).LaneDestAddress1)
                .strState = Trim(oData(0).LaneDestState)
                .strCity = Trim(oData(0).LaneDestCity)
            End With
            Dim laneOriginAddressUse = oData(0).LaneOriginAddressUse
            'get the parameter settings
            Dim gPCMiler As New PCM.PCMiles
            Dim blnLoggingOn As Boolean = False
            Dim strPCMilerLogFile As String = ""
            getPCMilerParameters(gPCMiler, blnLoggingOn, strPCMilerLogFile)
            'Get the Miles
            Dim objallstops As New PCM.clsAllStops
            Dim intBookControl As Integer = 0
            Dim strItemType As String = "Lane Number"""
            Dim dblBatchID As Double = CDbl(Format(Now(), "MddyyyyHHmmss"))
            objallstops = getPracticalMiles(res, gPCMiler, blnLoggingOn, strPCMilerLogFile, objOrig, objDest, oData(0).LaneCompControl, intBookControl, oData(0).LaneControl, oData(0).LaneNumber, strItemType, dblBatchID)
            If objallstops Is Nothing Then Exit Try
            If Len(Trim(objallstops.FailedAddressMessage)) > 0 Then
                res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, objallstops.FailedAddressMessage, "Warning!")
                Return res
            End If
            If objallstops.BadAddressCount > 0 Then
                Dim msg = String.Format(NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("MSGPCMBadAddressWarning", "Warning! TMS found {0} address problems. Your address may require special attention."), objallstops.BadAddressCount)
                res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, msg, "Warning!")
            End If
            Dim LaneBenchMiles = objallstops.TotalMiles
            Dim LaneOrigZip As String = Nothing
            Dim LaneDestZip As String = Nothing
            If objallstops.AutoCorrectBadLaneZipCodes = 1 Then
                If Len(Trim(objallstops.OriginZip)) > 0 Then LaneOrigZip = objallstops.OriginZip
                If Len(Trim(objallstops.DestZip)) > 0 Then LaneDestZip = objallstops.DestZip
            End If
            'Get the Lat/Long
            Dim dblLat As Double = 0
            Dim dblLong As Double = 0
            Dim origZip = If(String.IsNullOrWhiteSpace(LaneOrigZip), Trim(oData(0).LaneOrigZip), LaneOrigZip) 'if we updated (autocorrect) the zip use that value else use the value from the lane
            Dim destZip = If(String.IsNullOrWhiteSpace(LaneDestZip), Trim(oData(0).LaneDestZip), LaneDestZip) 'if we updated (autocorrect) the zip use that value else use the value from the lane
            Dim blnSaveLatLong = GetLatLongPCMiler(res, dblLat, dblLong, origZip, destZip, laneOriginAddressUse, gPCMiler, blnLoggingOn, strPCMilerLogFile)
            'Save the values
            NGLLaneData.SaveLaneMilesLatLong(LaneControl, LaneBenchMiles, dblLat, dblLong, blnSaveLatLong, LaneOrigZip, LaneDestZip)
        Catch ex As Exception
            res.appendToResultMessage(Models.ResultObject.ResultMsgType.Err, ex.Message, "Error")
        End Try
        Return res
    End Function


    Public Function CalculateAndSaveBookMiles(ByVal intBookControl As Integer) As Models.ResultObject
        Dim res As New Models.ResultObject

        res.Success = True
        Try
            'Get the Lane data using the LaneControl
            Dim RecordCount = 0
            Dim oData = NGLBookData.GetBookFilteredNoChildren(intBookControl)
            If oData Is Nothing OrElse oData.BookControl = 0 Then Return Nothing
            Return CalculateAndSaveBookMiles(oData, intBookControl)

        Catch ex As Exception
            res.appendToResultMessage(Models.ResultObject.ResultMsgType.Err, ex.Message, "Error")
        End Try
        Return res
    End Function


    Public Function CalculateAndSaveBookMiles(ByVal oData As DTO.Book, ByVal intBookControl As Integer) As Models.ResultObject
        Dim res As New Models.ResultObject
        res.Success = True
        Try
            'Get the Lane data using the LaneControl
            Dim RecordCount = 0
            If oData Is Nothing OrElse oData.BookControl = 0 Then Return Nothing
            Dim objOrig As New PCM.clsAddress
            With objOrig
                .strZip = Trim(oData.BookOrigZip)
                .strAddress = Trim(oData.BookOrigAddress1)
                .strState = Trim(oData.BookOrigState)
                .strCity = Trim(oData.BookOrigCity)
            End With
            Dim objDest As New PCM.clsAddress
            With objDest
                .strZip = Trim(oData.BookDestZip)
                .strAddress = Trim(oData.BookDestAddress1)
                .strState = Trim(oData.BookDestState)
                .strCity = Trim(oData.BookDestCity)
            End With
            'get the parameter settings
            Dim gPCMiler As New PCM.PCMiles
            Dim blnLoggingOn As Boolean = False
            Dim strPCMilerLogFile As String = ""
            'getPCMilerParameters(gPCMiler, blnLoggingOn, strPCMilerLogFile)
            'Get the Miles
            Dim objallstops As New PCM.clsAllStops
            'Dim intBookControl As Integer = oD
            Dim strItemType As String = ""
            Dim dblBatchID As Double = CDbl(Format(Now(), "MddyyyyHHmmss"))
            objallstops = getPracticalMiles(res, gPCMiler, blnLoggingOn, strPCMilerLogFile, objOrig, objDest, oData.BookCustCompControl, intBookControl, oData.BookODControl, "", strItemType, dblBatchID)
            If objallstops Is Nothing Then Exit Try
            If Len(Trim(objallstops.FailedAddressMessage)) > 0 Then
                res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, objallstops.FailedAddressMessage, "Warning!")
                Return res
            End If
            If objallstops.BadAddressCount > 0 Then
                Dim msg = String.Format(NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("MSGPCMBadAddressWarning", "Warning! TMS found {0} address problems. Your address may require special attention."), objallstops.BadAddressCount)
                res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, msg, "Warning!")
            End If
            'Save the values
            NGLBookData.SaveBookMiles(intBookControl, objallstops.TotalMiles)
        Catch ex As Exception
            res.appendToResultMessage(Models.ResultObject.ResultMsgType.Err, ex.Message, "Error")
        End Try
        Return res
    End Function


    'PCMiler Configuration
    Public Sub getPCMilerParameters(ByRef gPCMiler As PCM.PCMiles, ByRef blnLoggingOn As Boolean, ByRef strPCMilerLogFile As String)
        If NGLBatchProcessData.GetParValue("PCMilerLogging", 0) <> 0 Then
            strPCMilerLogFile = NGLBatchProcessData.GetParText("PCMilerLogging", 0)
            If Len(Trim(strPCMilerLogFile)) > 0 Then blnLoggingOn = True
        End If
        If NGLBatchProcessData.GetParValue("GlobalDebugMode", 0) = 0 Then gPCMiler.Debug = False Else gPCMiler.Debug = True
        If NGLBatchProcessData.GetParValue("GlobalKeepLogDays", 0) = 0 Then gPCMiler.KeepLogDays = False Else gPCMiler.KeepLogDays = True
        If NGLBatchProcessData.GetParValue("GlobalSaveOldLogs", 0) = 0 Then gPCMiler.SaveOldLog = False Else gPCMiler.SaveOldLog = True
        gPCMiler.WebServiceURL = NGLBatchProcessData.GetParText("PCMilerWebServiceURL", 0)
        If NGLBatchProcessData.GetParValue("PCMilerUseZipOnly", 0) = 0 Then gPCMiler.UseZipOnly = False Else gPCMiler.UseZipOnly = True
    End Sub

    'Miles
    Function getPracticalMiles(ByRef res As Models.ResultObject,
                               ByVal gPCMiler As PCM.PCMiles,
                               ByVal blnLoggingOn As Boolean,
                               ByVal strPCMilerLogFile As String,
                               ByVal oPCMOrig As PCM.clsAddress,
                               ByVal oPCMDest As PCM.clsAddress,
                               ByVal intCompControl As Integer,
                               ByVal intBookControl As Integer,
                               ByVal intLaneControl As Integer,
                               ByVal strItemNumber As String,
                               ByVal strItemType As String,
                               ByVal dblBatchID As Double) As PCM.clsAllStops
        Dim oPCMBadAddresses As New PCM.clsPCMBadAddresses
        Dim dblAutoCorrectBadLaneZipCodes As Double
        Dim oPCMAllStops As New PCM.clsAllStops
        With oPCMAllStops
            .BadAddressCount = 0
            .FailedAddressMessage = ""
            .BatchID = dblBatchID
        End With
        Dim Route_Type As Integer = NGLBatchProcessData.GetParValue("PCMilerRouteType", intCompControl) 'Route_Type = CInt(gParData.getParValue("PCMilerRouteType", PaneSettings.MainInterface.ParameterList, intCompControl))
        Dim Dist_Type As Integer = NGLBatchProcessData.GetParValue("PCMilerDistanceType", intCompControl) 'Dist_Type = CInt(gParData.getParValue("PCMilerDistanceType", PaneSettings.MainInterface.ParameterList, intCompControl))
        If intLaneControl > 0 Then
            dblAutoCorrectBadLaneZipCodes = NGLBatchProcessData.GetParValue("AutoCorrectBadLaneZipCodes", intCompControl) 'dblAutoCorrectBadLaneZipCodes = CInt(gParData.getParValue("AutoCorrectBadLaneZipCodes", PaneSettings.MainInterface.ParameterList, intCompControl))
        Else
            dblAutoCorrectBadLaneZipCodes = 0
        End If
        'call the PCMiler wrapper method
        Dim sLastError As String = ""
        oPCMAllStops = PCMilerBLL.GetPracticalMiles(oPCMOrig, oPCMDest, Route_Type, Dist_Type, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, False, oPCMBadAddresses, blnLoggingOn, strPCMilerLogFile, sLastError)
        If oPCMAllStops Is Nothing Then
            Dim msg = String.Format(NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("MSGPCMGetMilesFailedWarning", "Get Practical Distance Failure: {0}. Check the bad address details."), sLastError) 'PaneSettings.MainInterface.FMMessage.ShowFMWarning(String.Format(PaneSettings.MainInterface.LocalizeString("MSGPCMGetMilesFailedWarning"), gPCMiler.LastError))
            res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, msg, "Warning!")
            Return oPCMAllStops
        Else
            If Len(Trim(sLastError)) > 0 Then
                Dim msg = String.Format(NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("MSGPCMGetMilesFailedWarning", "Get Practical Distance Failure: {0}. Check the bad address details."), sLastError) 'PaneSettings.MainInterface.FMMessage.ShowFMWarning(String.Format(PaneSettings.MainInterface.LocalizeString("MSGPCMGetMilesFailedWarning"), gPCMiler.LastError))
                res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, msg, "Warning!")
                Return oPCMAllStops
            End If
            If oPCMBadAddresses.COUNT > 0 Then logBadAddress(oPCMBadAddresses, oPCMAllStops)
        End If
        Return oPCMAllStops
    End Function

    Public Sub logBadAddress(ByVal oPCMBadAddresses As PCM.clsPCMBadAddresses, ByVal oPCMAllStops As PCM.clsAllStops, Optional ByVal Silent As Boolean = False)
        Dim oPCMBadAddress As PCM.clsPCMBadAddress
        Dim intRet As Integer = 0
        For intBadAddressCT = 1 To oPCMBadAddresses.COUNT
            oPCMBadAddress = oPCMBadAddresses.Item(intBadAddressCT)
            Try
                intRet = NGLLaneData.AddBadAddress(oPCMBadAddress.BookControl,
                                   oPCMBadAddress.LaneControl,
                                   Left(oPCMBadAddress.objOrig.strAddress, 40),
                                   Left(oPCMBadAddress.objOrig.strCity, 25),
                                   Left(oPCMBadAddress.objOrig.strState, 8),
                                   Left(oPCMBadAddress.objOrig.strZip, 10),
                                   "",
                                   Left(oPCMBadAddress.objDest.strAddress, 40),
                                   Left(oPCMBadAddress.objDest.strCity, 25),
                                   Left(oPCMBadAddress.objDest.strState, 2),
                                   Left(oPCMBadAddress.objDest.strZip, 10),
                                   "",
                                   Left(oPCMBadAddress.objPCMOrig.strAddress, 40),
                                   Left(oPCMBadAddress.objPCMOrig.strCity, 25),
                                   Left(oPCMBadAddress.objPCMOrig.strState, 8),
                                   Left(oPCMBadAddress.objPCMOrig.strZip, 10),
                                   "",
                                   Left(oPCMBadAddress.objPCMDest.strAddress, 40),
                                   Left(oPCMBadAddress.objPCMDest.strCity, 25),
                                   Left(oPCMBadAddress.objPCMDest.strState, 2),
                                   Left(oPCMBadAddress.objPCMDest.strZip, 10),
                                   "",
                                   Left(oPCMBadAddress.Message, 1000),
                                   oPCMBadAddress.BatchID)
            Catch ex As Exception
                'do nothing
            End Try
        Next
    End Sub

    'Lat/Long
    Public Function GetLatLongPCMiler(ByRef res As Models.ResultObject,
                                 ByRef dblLat As Double,
                                 ByRef dblLong As Double,
                                 ByVal LaneOrigZip As String,
                                 ByVal LaneDestZip As String,
                                 ByVal LaneOriginAddressUse As Boolean,
                                 ByVal gPCMiler As PCM.PCMiles,
                                 ByVal blnLoggingOn As Boolean,
                                 ByVal strPCMilerLogFile As String) As Boolean
        Dim blnRet As Boolean = False
        Dim strlocation As String = ""
        Try
            If LaneOriginAddressUse = False Then
                If Not String.IsNullOrEmpty(LaneDestZip) AndAlso LaneDestZip.Trim.Length > 0 Then strlocation = strlocation & " " & zipClean(LaneDestZip)
            Else
                If Not String.IsNullOrEmpty(LaneOrigZip) AndAlso LaneOrigZip.Trim.Length > 0 Then strlocation = strlocation & " " & zipClean(LaneOrigZip)
            End If
            Dim sLastError As String = ""
            blnRet = PCMilerBLL.getGeoCode(strlocation, dblLat, dblLong, sLastError, blnLoggingOn, strPCMilerLogFile)
            If Not blnRet Then
                Dim msg = String.Format(NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("MSGPCMGeoCodeInvalidZip", "Cannot Lookup Latitude and Longitude; please check that the postal code, {0}, is valid and try again."), strlocation)
                res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, msg, "Warning!")
                If Len(Trim(sLastError)) > 0 Then
                    msg = String.Format(NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("MSGPCMGeoCodeFailedWarning", "Get GeoCode Failure. PC Miler returned the following information: {0}."), sLastError)
                    res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, msg, "Warning!")
                End If
            End If
        Catch ex As Exception
            res.appendToResultMessage(Models.ResultObject.ResultMsgType.Err, ex.Message, "Error")
        End Try
        Return blnRet
    End Function

    Private Function stripQuotes(ByVal strval As String) As String
        Dim strNewVal As String
        If strval.Substring(0, 1) = "'" Then strNewVal = Mid(strval, 2) Else strNewVal = strval
        If Right(strNewVal, 1) = "'" Then strNewVal = strNewVal.Substring(0, Len(strNewVal) - 1)
        stripQuotes = strNewVal
    End Function

    Private Function zipClean(ByVal strCode As String) As String
        Dim intPos As Integer
        strCode = Trim(stripQuotes(strCode))
        intPos = InStr(1, strCode, "-")
        If intPos Then
            strCode = strCode.Substring(0, intPos - 1)
            If Len(strCode) > 5 Then strCode = strCode.Substring(0, 5)
        End If
        zipClean = strCode
    End Function

#End Region


#Region "Company"

    ''' <summary>
    ''' Calculates the Lattitude and Longitude using the provided Zip Code use PCMilerBLL.concateAddress to build the address string
    ''' </summary>
    ''' <param name="res"></param>
    ''' <param name="dblLat"></param>
    ''' <param name="dblLong"></param>
    ''' <param name="sAddress"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created By LVV on 6/15/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes
    ''' Modified by RHR for v-8.4.0.003 on 07/19/2021 added full address support
    ''' </remarks>
    Public Function GetLatLongPCMiler(ByRef res As Models.ResultObject, ByRef dblLat As Double, ByRef dblLong As Double, ByVal sAddress As String) As Boolean
        Dim blnRet As Boolean = False
        Dim strLastError As String = ""

        'get the parameter settings
        Dim gPCMiler As New PCM.PCMiles()
        Dim blnLoggingOn As Boolean = False
        Dim strPCMilerLogFile As String = ""
        getPCMilerParameters(gPCMiler, blnLoggingOn, strPCMilerLogFile)


        Dim strlocation As String = ""
        Try
            blnRet = PCMilerBLL.getGeoCode(sAddress, dblLat, dblLong, strLastError, blnLoggingOn, strPCMilerLogFile)

            If Not blnRet Then
                res.Success = False
                Dim msg = String.Format(NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("MSGPCMGeoCodeInvalidZip", "Cannot Lookup Latitude and Longitude; please check the Address Information, {0}, and try again."), strlocation)
                res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, msg, "Warning!")
                If Len(Trim(strLastError)) > 0 Then
                    msg = String.Format(NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("MSGPCMGeoCodeFailedWarning", "Get GeoCode Failure. PC Miler returned the following information: {0}."), strLastError)
                    res.appendToResultMessage(Models.ResultObject.ResultMsgType.Warning, msg, "Warning!")
                End If
            End If
        Catch ex As Exception
            res.appendToResultMessage(Models.ResultObject.ResultMsgType.Err, ex.Message, "Error")
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Calculates and saves the Lattitude and Longitude of the provided Company
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns></returns>
    ''' <remarks>Created By LVV on 6/15/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes</remarks>
    Public Function RecalculateCompLatLong(ByVal CompControl As Integer) As Models.ResultObject
        Dim res As New Models.ResultObject
        res.Success = True
        Try
            'Get the Company data using the CompControl
            Dim oData = NGLCompData.GetComp(CompControl)
            If oData Is Nothing OrElse oData.CompControl = 0 Then Return Nothing
            'Get the Lat/Long
            Dim dblLat As Double = 0
            Dim dblLong As Double = 0
            Dim strAddress = PCMilerBLL.concateAddress(oData.CompStreetAddress1, oData.CompStreetCity, oData.CompStreetState, oData.CompStreetZip, oData.CompStreetCountry)
            Dim blnSaveLatLong = GetLatLongPCMiler(res, dblLat, dblLong, strAddress)
            'Save the values
            If blnSaveLatLong Then
                NGLCompData.SaveCompLatLong(CompControl, dblLat, dblLong)
            End If
            If res.Success Then
                Dim title = NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("M_Success", "Success!")
                Dim msg = NGLcmLocalizeKeyValuePairData.GetLocalizedValueByKey("M_CalcLatLongSuccess", "Calculate Lattitude and Longitude Success")
                res.appendToResultMessage(Models.ResultObject.ResultMsgType.Success, msg, title)
            End If
        Catch ex As Exception
            res.appendToResultMessage(Models.ResultObject.ResultMsgType.Err, ex.Message, "Error")
        End Try
        Return res
    End Function

#End Region


  

End Class



