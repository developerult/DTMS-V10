Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports CoreUtility = Ngl.Core.Utility


Namespace DataTransferObjects
    ''' <summary>
    ''' Standard Message Object used between methods
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.002 on 12/17/2020
    '''  added new methods to read and write to three new 
    '''  private properties
    '''  1. AsyncMessagesPossible  -- boolean
    '''  2. AsyncMessageKey  -- long
    '''  3. AsyncTypeKey  -- integer
    '''  We use methods so we do not break interaction with Desktop
    '''  New functionality only availble in D365
    ''' </remarks>
    <DataContract()>
    Public Class WCFResults
        Inherits DTOBaseClass

#Region " Enums"

        Public Enum MessageType
            Errors
            Warnings
            Messages
        End Enum

        Public Enum MessageEnum
            None
            M_NoOrdersFound '"No BookRevenue data found." 
            W_BookingRecordMissingCompany '"A company has not been assigned to this order."
            E_CreditHold '"CREDIT HOLD"
            E_CreditNotSetMsg '"*WARNING* This Customer Is On Credit Hold Do Not Place Order!"
            E_CreditOverLimit '"*WARNING* This Customer Is Over Their Credit Limit Check With Accounting!"
            M_BookDeletedEDIMarkedAsExported '"EDI 204 marked as recieved by carrier for an order modification."
            M_BookDeletedEDIMarkedAsExportedForCanceled '"EDI 204 marked as recieved by carrier for a canceled order."
            Success '"Success"
            W_CarrContEmailNotifInfoNotAvailable '"*WARNING* The carrier contact information is not available."
            W_AcceptRejectExpire_InvalidCompContEmail '"Notifications about a loads accept, reject or expire status could not be transmitted to the company contact because of an invalid company contact email address; the system will attempt to use the default notification email settings."
            W_AcceptRejectExpire_InvalidNotificationEmail '"Notifications about a loads accept, reject or expire status could not be transmitted because of invalid default notification email address settings."
            E_CreateExpiredDropLoadFaultException '"There was a problem creating the Drop Load record when a booking record was expired: {0}."
            E_ProcessExpiredLoadsError '"Could not process expired loads for Companany Number: {2}; SQL Error: {0}; Message: {1}."
            E_ProcessExpiredLoadsErrorNoPro '"Could not process expired loads; the Book PRO number, {0} cannot be found."
            MSGdoFinalizeAllExp '"carrier {0} has a current value of: {1} with a maximum exposure of: {2}."
            MSGdoFinalizeBooking '"You are about to accept/finalize the selected load.  Are you sure you want continue?"
            MSGdoFinalizeContractExp '"carrier {0} has an expiration date of: {1}."
            MSGdoFinalizeInsuranceExp '"carrier {0} has an expiration date of: {1}."
            MSGdoFinalizePerShipExp '"carrier {0} has a current value of: {1} with a maximum exposure of: {2}."
            MSGundoFinalizeBooking '"You are about to unfinalize the selected load.  Are you sure you want continue?"
            W_AccessDeniedFinalizeOrdersBeforeShip '"Access Denied! {0} is not authorized to finalize orders."
            W_AccessDeniedFinalizeOrdersAfterShip '"Access Denied! {0} is not authorized to finalize orders after they have shipped."
            E_SQLFaultCannotFinalize '"Data Access Failure Cannot Finalize Load."
            E_ValidateCarrierQualBeforeFinalize_Error '"Cannot Finalize Load because the Carrier Validation Failed: {0}."
            W_FinalizePickListFailed '"Warning! The load was finalized but the pick list status update had errors: {0}."
            E_SQLFaultCannotProcessNewTranCode '"SQL Fault Exception! Cannot update tran code."
            W_AccessDeniedUnFinalizeOrdersBeforeShip '"Access Denied! {0} is not authorized to unfinalize orders."
            W_AccessDeniedUnFinalizeOrdersAfterShip '"Access Denied! {0} is not authorized to unfinalize orders after they have shipped."
            W_UnFinalizePickListFailed '"Warning! The load was unfinalized but the pick list status update had errors: {0}."
            W_UnFinalizeCancelEDIFailed '"Warning! The load was unfinalized but the process to send a cancel EDI message had errors: {0}."
            W_AccessDeniedMoveToPCFromPB '"Access Denied! Cannot go to PC from PB try unfinalizing to P status."
            W_CannotTenderLoadUpdateSHIDFailed '"Warning! Cannot tender load because the update SHID method failed, check your load consolidation settings and try again."
            M_CarrierCostsWillBeUnlocked 'One or more orders on this load has the carrier costs locked; your action will unlock the carrier costs for all orders on this load.  Are you sure you want to continue?"
            M_BFCCostsWillBeUnlocked 'One or more orders on this load has the BFC costs locked; your action will unlock the BFC costs for all orders on this load.  Are you sure you want to continue?"
            M_CarrierANDBFCCostsWillBeUnlocked 'One or more orders on this load has the carrier and BFC costs locked; your action will unlock both the carrier and BFC costs for all orders on this load.  Are you sure you want to continue?"
            M_CarrierCostWereNotUnlocked 'One or more orders on this load has the carrier costs locked.  Carrier costs will not be updated unless you unlock the costs.
            M_BFCCostsWereNotUnlocked 'One or more orders on this load has the BFC costs locked.  BFC costs will not be updated unless you unlock the BFC costs.
            M_CarrierAndBFCCostWereNotUnlocked 'One or more orders on this load has the BFC and carrier costs locked.  BFC and Carrier costs will not be updated unless you unlock the costs.
            MSGChangedFromICToOther 'You are about to remove the Invoice Complete status of this load, would you like to continue?
            M_RejectLoadValidation 'You are about to reject the selected load all carrier information will be reset. Are you sure you wish to continue?
            M_DropLoadValidation 'You are about to drop the carrier assigned to the selected load; all carrier information will be reset. Are you sure you wish to continue?
            W_ManualAutoAcceptNoTenderEmail '"Warning! The load has been tendered and auto accepted by moving from P to PB.  Load Tender Email are not created.  If a Load Tender is required it must be transmitted manually."  
            W_CarrierDoesNotMatch '"Warning! Cannot {0} load because the current carrier, {1}, no longer matches the previously assigned carrier: {2}."
            E_SQLFaultCannotUpdateLoadStatus '"Data Access Failure Cannot Update Load Status."
            E_SQLFaultCreateDropLoadRecordFailure '"The load was returned to N status but the system could not save the Drop Load Transaction Record. Please create this record manually if desired." 
            W_CannotAddAutoTenderLogData '"Warning! Tender not affected. A load has been auto tendered to carrier {0} but the system could not log the results.  If this load is rejected the maximum number of cascading dispatches may be exceeded."
            W_CannotAddAutoAcceptLogData '"Warning! Tender not affected. A load has been auto accepted for carrier {0} but the system could not log the results.  If this load is later rejected the maximum number of cascading dispatches may be exceeded."
            W_NoAutoTenderData '"Cannot Auto Tender the load because the Auto Tender Information is not available."
            W_CannotAutoTenderInvalidTranCode '"Cannot Auto Tender because one of the Booking Transaction Codes is not allowed for auto tendering."
            W_NoAutoAcceptData '"Cannot Auto Accept the load because the Auto Accept Information is not available."
            W_CannotAutoAcceptInvalidTranCode '"Cannot Auto Accept because one of the Booking Transaction Codes is not allowed for auto acceptance."
            W_CannotAutoTenderInvalidCarrier '"Cannot Auto Tender Load because one or more orders has not been assigned a carrier or the carrier cost is less than or equal to zero."
            W_CannotAutoAcceptInvalidCarrier '"Cannot Auto Accept Load because one or more orders has not been assigned a carrier or the carrier cost is less than or equal to zero."
            W_CannotAutoTenderNotATruckLoad '"Cannot Auto Tender Load because it does not meet the lane truck load capacity requirements."
            W_CannotAutoAcceptNotATruckLoad '"Cannot Auto Accept Load because it does not meet the lane truck load capacity requirements."
            W_CannotAutoTenderMultiPick '"Cannot Auto Tender Load because it is a multi-pick load."
            W_CannotAutoAcceptMultiPick '"Cannot Auto Accept Load because it is a multi-pick load."
            W_CannotAutoTenderMultiStop '"Cannot Auto Tender Load because it is a multi-stop load."
            W_CannotAutoAcceptMultiStop '"Cannot Auto Accept Load because it is a multi-stop load."
            W_CannotAutoTenderGlobalParameterIsOff '"Cannot Auto Tender Load because the global AutoTender parameter is turned off; value must be equal to 1."
            W_CannotAutoAcceptGlobalParameterIsOff '"Cannot Auto Accept Load because the global AutoTender parameter is turned off; value must be equal to 1."
            W_CannotAutoTenderLaneAutoTenderIsOff '"Cannot Auto Tender Load because one of the lanes has Auto Tender turned off."
            W_CannotAutoAcceptLaneAutoTenderIsOff '"Cannot Auto Accept Load because one of the lanes has Auto Tender turned off."
            W_CannotCascadeDispatchGlobalParameterIsOff '"Cannot Cascade Dispatch Load because the global AutoTenderCascadingDispatching parameter is turned off; value must be equal to 1."
            W_CannotCascadeDispatchLaneIsOff '"Cannot Cascade Dispatch Load because the AutoTenderCascadingDispatchingDepth parameter has been reached."
            W_CannotCascadeDispatchDepthReached '"Cannot Cascade Dispatch Load because one of the lanes has Cascading Dispatching turned off."
            W_CannotCascadeDispatchInvalidTranCode '"Cannot Cascade Dispatch Load because one of the Booking Transaction Codes is not N."
            W_CannotCascadeDispatchMultiPick '"Cannot Cascade Dispatch Load because it is a multi-pick load."
            W_CannotCascadeDispatchMultiStop '"Cannot Cascade Dispatch Load because it is a multi-stop load."
            W_CannotCascadeDispatchNotATruckLoad '"Cannot Cascade Dispatch Load because it does not meet the lane truck load capacity requirements."
            W_CannotUpdateDefaultCarrierCheckAlerts '"Cannot update the default carrier check alerts for more information."
            E_SQLFaultCannotUpdateBookFuelFeeForLoad '"Data Access Failure Cannot Update Fuel Fees For Load."
            E_UnExpected_Error '"An Unexpected Error Has Occurred: {0}.  You should manually refresh your data to be sure you have the latest changes."
            'Added by LVV on 3/18/16 for v-7.0.5.1 DAT
            E_NotFoundSSOAByUser '"No Single Sign On Accounts exist for selected user."
            M_Success '"Success!"
            E_InvalidSSOAName '"No record exists in the database with SSOAName: {0}."
            E_InvalidSSOAControl '"No record exists in the database with SSOAControl: {0}."
            'Added by LVV 5/18/16 for v-7.0.5.110 DAT
            E_InvalidParameterNameValue 'Invalid Parameter: No record exists in the database for {0}: {1}.
            E_RequiredFieldDATEquipType 'The DATEquipType field is required and cannot be null. Please check that a DATEquipType mapping exists for all Temp Type Codes in the Lane Code Maintenance Window and that the Company Level Parameter DATDefaultMixTempType is populated correctly.
            'Added by LVV 5/23/16 for v-7.0.5.110 DAT
            W_UserNoDATAccount 'User {0} does not have an SSOA account set up for DAT
            'Added by LVV 6/30/16 for v-7.0.5.110 DAT
            E_DATGeneralRetMsg '{0}
            E_DATInvalidFeature 'Failure - Could not execute DAT program. Invalid DAT Feature {0}
            E_DATDeletePostFailed 'DAT Delete Post Failed -- {0}
            E_DATPostFailed 'DAT Post Failed -- {0}
            W_CannotAutoAssignDefaultCarrier 'Cannot Auto Assign the default carrier because none of the configured preferred carriers pass the data validation rules.  Please check your Preferred Carrier Settings!"
            W_CannotAutoAssignSystemDefaultCarrier 'Cannot Auto Assign the system default carrier configured in the global parameter settings.  Lane Default Carrier is turned off and we are using the parameter settings for default carrier.  Check your parameter settings and be sure the carrier is properly configured.
            E_DATBookRevsNull 'DAT Post Failed because the BookRevenues cannot be null -- {0}
            E_DATCarrierNull 'DAT Post Failed because the Load Board Carrier could not be found -- {0}
            W_UserNoSSOAAccount 'User {0} does not have an SSOA account set up for {1}
            E_NoLoadTenderForBook 'Could not find Load Tender record with BookSHID: {0}, LoadTenderTypeControl: {1}, and Archived: 0.
            E_LoadBoardSpFailNoLTControl 'The stored procedure spInsertLoadBoardRecords did not return a valid LoadTenderControl.
            E_LBBookRevsNull '{0} {1} Failed because the BookRevenues cannot be null. Source: {2}  (Where 1 is LB Name and 2 is Action ex "DAT Post" or "Next Stop Delete")
            E_NSP44AcceptNoCarrier 'NEXTStop Error: Accept P44 Bid failed for LoadTenderControl {0} and BidControl {1}.{2}No CarrierControl was found using CarrierSCAC {3}.{2}Source: {4}.
            W_NTAllUpdateAssignedInfoFail 'There was a problem updating assigned information For PRO #{0}. The record was Not saved. Source: {1}.
            W_NTAllGetBookCarrierFilteredFail '"There was a problem getting dates information for PRO #{0}. Source: {1}"
            W_NTAllLoadStatusCodeNotSpecified '"Please enter a reason code for Pro #{0}, the Delivered Date did not meet the Required Date. The record was not saved. Source {1}."
            W_NTAllSetCommentsForChangedDatesAndTimesFail 'There was a problem setting comments for changed dates and times for PRO #{0}. Comments were not saved. Source{1}.
            W_NTAllSetCommentsRelatedItemsFail 'There was a problem setting comments for related items PRO #{0}. Comments were not saved. Source: {1}.
            W_NTAllSendCommentsEmailFail 'There was a problem sending comments email alert for PRO #{0}. Source: {1}.
            M_NTAllTabSaveSuccess 'PRO #{0}{1}{2} has been saved successfully.      'Note: (if cns blank {1}{2} = "" else {1} = " with CNS " {2} = cns )
            M_NTAllTabSaveWarnings 'PRO #{0}{1}{2} has been saved with Warnings.      'Note: (if cns blank {1}{2} = "" else {1} = " with CNS " {2} = cns )
            'Added by RHR for v-8.2 on 12/17/2018 for new Quote Generation logic
            E_SaveRateSpFailNoLTControl 'Save rate failure a load tender record could not be created.
            E_SaveRateFailure 'Save Rate Failure for LoadTenderControl = {0}: {1}.
            E_ReadQuoteSpFailNoLTControl 'Read quote failure a load tender record could not be created.
            E_ReadQuoteFailure 'Read Quote Failure for LoadTenderControl = {0}: {1}.
            E_NotFoundCarrierAPISSOAByUser 'The current user {0} does not have an account to access the Carrier API interface'
            E_SystemWarning '"Warning: {0}."
            E_SystemInfo '"Info: {0}."
            W_MissingLoadTenderDispatched 'Warning: Your load has been dispatched; however, we could not save the booking information.  Please create the booking information manually. {0}'
            E_ReadDispatchBidFailure 'The system was unable to read the selected bid using bid control number {0}.  The load may have already been dispatched. {1}'
            E_DispatchFailure 'The system was unable to dispatch the selected bid using bid control number {0}. {1}'
            E_SaveTariffRevisionFailed 'Unable to save new tariff revision changes: {0}

        End Enum

        Public Enum ActionEnum
            DoNothing
            PrintReport
            ShowValidationMsg
            ShowErrors
            ShowWarnings
        End Enum

        Public Enum ReportEnum
            CAR021
            CAR106
            NGL021sFax
            NGL106csFax
            CAR120b
            CAR120c
            CAR155b
            CAR155c
            CAR156
            CAR156c
            OPS152
            OPS153
            FIN014
            FIN017
            FIN018
            FIN017B
            FIN018C
            CAR100
        End Enum

        'Each validation bit represents unique validation flag
        'boolean value stored inside a 64 bit integer starting 
        'at bit 0 (zero) and ending at bit 63.  Use the NGL.Core.Utility.BitwiseFlags 
        'class to determine which flag is true and which flag is false.  Assign the
        'FlagSource of the BitwiseFlags class to the value of the 64 bit integer and
        'check if the flag is true or false using the isBitFlagOn method.  the position 
        'is determined by the ValidationBits enum.  The ValidationBits enum should be
        'implemented on the client to assist with message validation accept or reject.
        'the integer 64 value.  Changes made to this enum must be copied to the WCFResults
        'partial class in the NGLWCFClient project in the NGLBookDataObjects file.

        Public Enum ValidationBits
            None
            CarrierOrBFCCostsWillBeUnlocked
            CarrierANDBFCCostsWillBeUnlocked
            ReverseTheInvoiceCompleteStatus
            DoFinalize
            ValidateAlertsBeforeFinalize
            UndoFinalize
            RejectTheLoad
            DropTheLoad
        End Enum
#End Region

#Region "Private Properties"
        ''' Modified by RHR for v-8.3.0.002 on 12/17/2020
        '''  added new methods to read and write to three new 
        '''  private properties
        '''  1. AsyncMessagesPossible  -- boolean
        '''  2. AsyncMessageKey  -- integer
        '''  3. AsyncTypeKey  -- integer
        '''  
        Private _AsyncMessagesPossible As Boolean = False
        Private _AsyncMessageKey As Long = 0
        Private _AsyncTypeKey As Integer = 0


#End Region

#Region " Data Members"

        Private _Key As Integer = 0
        <DataMember()>
        Public Property Key() As Integer
            Get
                Return _Key
            End Get
            Set(ByVal value As Integer)
                _Key = value
            End Set
        End Property

        Private _KeyFields As Dictionary(Of String, String)
        <DataMember()>
        Public Property KeyFields() As Dictionary(Of String, String)
            Get
                If _KeyFields Is Nothing Then _KeyFields = New Dictionary(Of String, String)
                Return _KeyFields
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                _KeyFields = value
            End Set
        End Property

        Private _AlphaCode As String
        <DataMember()>
        Public Property AlphaCode() As String
            Get
                Return _AlphaCode
            End Get
            Set(ByVal value As String)
                _AlphaCode = value
            End Set
        End Property

        Private _Success As Boolean = False
        <DataMember()>
        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Private _intValues As New List(Of NGLListItem)
        ''' <summary>
        ''' intValues is a list of integers stored in an NGLListItem object. 
        ''' Built for use with change tracking collections
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()>
        Public Property intValues() As List(Of NGLListItem)
            Get
                Return _intValues
            End Get
            Set(ByVal value As List(Of NGLListItem))
                _intValues = value
            End Set
        End Property

        Private _dblValues As New List(Of NGLListItem)
        ''' <summary>
        ''' dblValues is a list of doubles stored in an NGLListItem object. 
        ''' Built for use with change tracking collections
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()>
        Public Property dblValues() As List(Of NGLListItem)
            Get
                Return _dblValues
            End Get
            Set(ByVal value As List(Of NGLListItem))
                _dblValues = value
            End Set
        End Property

        Private _dtValues As New List(Of NGLListItem)
        ''' <summary>
        ''' dtValues is a list of dates stored in an NGLListItem object. 
        ''' Built for use with change tracking collections
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()>
        Public Property dtValues() As List(Of NGLListItem)
            Get
                Return _dtValues
            End Get
            Set(ByVal value As List(Of NGLListItem))
                _dtValues = value
            End Set
        End Property

        Private _strValues As New List(Of NGLListItem)
        ''' <summary>
        ''' strValues is a list of dates stored in an NGLListItem object. 
        ''' Built for use with change tracking collections
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()>
        Public Property strValues() As List(Of NGLListItem)
            Get
                Return _strValues
            End Get
            Set(ByVal value As List(Of NGLListItem))
                _strValues = value
            End Set
        End Property


        Private _Errors As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()>
        Public Property Errors() As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Errors
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Errors = value
            End Set
        End Property

        Private _Warnings As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()>
        Public Property Warnings() As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Warnings
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Warnings = value
            End Set
        End Property

        Private _Messages As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()>
        Public Property Messages() As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Messages
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Messages = value
            End Set
        End Property

        Private _DTOData As DTOBaseClass()
        <DataMember()>
        Public Property DTOData() As DTOBaseClass()
            Get
                Return _DTOData
            End Get
            Set(ByVal value As DTOBaseClass())
                _DTOData = value
            End Set
        End Property

        Private _BLLOnlyData As DTOBaseClass()
        Public Property BLLOnlyData() As DTOBaseClass()
            Get
                Return _BLLOnlyData
            End Get
            Set(ByVal value As DTOBaseClass())
                _BLLOnlyData = value
            End Set
        End Property

        Private _Data As Object
        Public Property Data() As Object
            Get
                Return _Data
            End Get
            Set(ByVal value As Object)
                _Data = value
            End Set
        End Property

        Private _Log As New List(Of NGLMessage)
        <DataMember()>
        Public Property Log() As List(Of NGLMessage)
            Get
                Return _Log
            End Get
            Set(ByVal value As List(Of NGLMessage))
                _Log = value
            End Set
        End Property

        Private _ActionName As String
        <DataMember()>
        Public Property ActionName() As String
            Get
                Return _ActionName
            End Get
            Set(ByVal value As String)
                _ActionName = value
            End Set
        End Property

        Private _Action As ActionEnum
        <DataMember()>
        Public Property Action() As ActionEnum
            Get
                Return _Action
            End Get
            Set(ByVal value As ActionEnum)
                _Action = value
            End Set
        End Property

        Private _Report As String
        <DataMember()>
        Public Property Report() As String
            Get
                Return _Report
            End Get
            Set(ByVal value As String)
                _Report = value
            End Set
        End Property


        Private _ValidationFlags As Int64
        <DataMember()>
        Public Property ValidationFlags() As Int64
            Get
                If _ValidationFlags = 0 Then
                    'set all validation flags to true.  
                    'Each validation method will be executed if the bitwise  ValidationFlag is true.
                    _ValidationFlags = New CoreUtility.BitwiseFlags(True).FlagSource
                End If
                Return _ValidationFlags
            End Get
            Set(ByVal value As Int64)
                _ValidationFlags = value
            End Set
        End Property

        Private _ValidationBitFailed As ValidationBits = ValidationBits.None
        <DataMember()>
        Public Property ValidationBitFailed As ValidationBits
            Get
                Return _ValidationBitFailed
            End Get
            Set(ByVal value As ValidationBits)
                _ValidationBitFailed = value
            End Set
        End Property

#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New WCFResults
            instance = DirectCast(MemberwiseClone(), DTOBaseClass)
            instance.DTOData = Nothing
            If Not DTOData Is Nothing Then
                instance.DTOData = DTOData.Clone()
            End If
            If Not intValues Is Nothing AndAlso intValues.Count > 0 Then
                instance.intValues = (From d In intValues Select d).ToList() 'creates a copy (clone)
            End If
            If Not strValues Is Nothing AndAlso strValues.Count > 0 Then
                instance.strValues = (From d In strValues Select d).ToList() 'creates a copy (clone)
            End If
            If Not dblValues Is Nothing AndAlso dblValues.Count > 0 Then
                instance.dblValues = (From d In dblValues Select d).ToList() 'creates a copy (clone)
            End If
            If Not dtValues Is Nothing AndAlso dtValues.Count > 0 Then
                instance.dtValues = (From d In dtValues Select d).ToList() 'creates a copy (clone)
            End If
            If Not Log Is Nothing AndAlso Log.Count > 0 Then
                instance.Log = (From d In Log Select d).ToList() 'creates a copy (clone)
            End If
            If Not Errors Is Nothing AndAlso Errors.Count > 0 Then
                instance.Errors = (From x In Errors Select x).ToDictionary(Function(x) x.Key, Function(y) y.Value) 'creates a copy (clone)
            End If
            If Not Warnings Is Nothing AndAlso Warnings.Count > 0 Then
                instance.Warnings = (From x In Warnings Select x).ToDictionary(Function(x) x.Key, Function(y) y.Value) 'creates a copy (clone)
            End If
            If Not Messages Is Nothing AndAlso Messages.Count > 0 Then
                instance.Messages = (From x In Messages Select x).ToDictionary(Function(x) x.Key, Function(y) y.Value) 'creates a copy (clone)
            End If
            If Not KeyFields Is Nothing AndAlso KeyFields.Count > 0 Then
                instance.KeyFields = (From x In KeyFields Select x).ToDictionary(Function(x) x.Key, Function(y) y.Value) 'creates a copy (clone)
            End If
            Return instance
        End Function

        Public Function getErrorsAsSingleStr(ByVal delimiter As String, Optional ByVal blnUseNewLine As Boolean = True) As String
            Return getMessageAsSingleStr(MessageType.Errors, delimiter, blnUseNewLine)
        End Function

        ''' <summary>
        ''' Loops through each message defined by type and returns a single non localized string seperated by delimiter. 
        ''' A new line is added after each delimiter if the blnUseNewLine flag is true (default)
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="delimiter"></param>
        ''' <param name="blnUseNewLine"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getMessageAsSingleStr(ByVal type As MessageType, ByVal delimiter As String, Optional ByVal blnUseNewLine As Boolean = True) As String
            Dim result As New StringBuilder
            Dim blnFirst As Boolean = True
            Dim strLineFeed As String = ""
            If blnUseNewLine Then strLineFeed = vbCrLf
            Dim oDict As New Dictionary(Of String, List(Of NGLMessage))
            Select Case type
                Case MessageType.Errors
                    oDict = Errors
                Case MessageType.Messages
                    oDict = Messages
                Case MessageType.Warnings
                    oDict = Warnings
            End Select
            If Not oDict Is Nothing AndAlso oDict.Count > 0 Then
                For Each e In oDict
                    Dim strMsg = formatMsgNotLocalized(e.Key, e.Value)
                    If blnFirst Then
                        result.Append(strMsg)
                    Else
                        result.AppendFormat(" {0} {1} {2}", delimiter, strLineFeed, strMsg)
                    End If
                    blnFirst = False
                Next
            Else
                Return ""
            End If

            Return result.ToString()
        End Function

        Public Function getLogAsSingleStr(ByVal delimiter As String, Optional ByVal blnUseNewLine As Boolean = True) As String
            Dim result As New StringBuilder
            Dim blnFirst As Boolean = True
            Dim strLineFeed As String = ""
            If blnUseNewLine Then strLineFeed = vbCrLf
            Dim append As String = delimiter & "{0}" & " {1}"
            If Not Log Is Nothing AndAlso Log.Count > 0 Then
                For Each e In Log
                    If blnFirst Then
                        result.Append(e.Message)
                    Else
                        result.AppendFormat(append, strLineFeed, e.Message)
                    End If
                    blnFirst = False
                Next
            Else
                Return ""
            End If

            Return result.ToString()
        End Function

        Public Shared Function ContainsNGLItem(ByVal intvalue As Integer, ByVal list As List(Of NGLListItem)) As Boolean
            Dim val = (From d In list Where d.intValue = intvalue Select d).FirstOrDefault
            If (val IsNot Nothing AndAlso val.intValue = intvalue) Then Return True
            Return False
        End Function

        ''' <summary>
        ''' set the bit at ValidationBits to true in intFlags using bitwise operation
        ''' </summary>
        ''' <param name="enumBit"></param>
        ''' <param name="intFlags"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function turnValidationOn(ByVal enumBit As ValidationBits, ByVal intFlags As Int64) As Int64
            Dim oBitWise As New CoreUtility.BitwiseFlags(intFlags)
            oBitWise.turnBitFlagOn(CInt(enumBit))
            Return oBitWise.FlagSource
        End Function

        ''' <summary>
        ''' set the bit at ValidationBits to false in intFlags using bitwise operation
        ''' </summary>
        ''' <param name="enumBit"></param>
        ''' <param name="intFlags"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function turnValidationOff(ByVal enumBit As ValidationBits, ByVal intFlags As Int64) As Int64
            Dim oBitWise As New CoreUtility.BitwiseFlags(intFlags)
            oBitWise.turnBitFlagOff(CInt(enumBit))
            Return oBitWise.FlagSource
        End Function

        ''' <summary>
        ''' returns the boolean value of the bit at ValidationBits in intFlags
        ''' </summary>
        ''' <param name="enumBit"></param>
        ''' <param name="intFlags"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function isValidationOn(ByVal enumBit As ValidationBits, ByVal intFlags As Int64) As Boolean
            Dim oBitWise As New CoreUtility.BitwiseFlags(intFlags)
            Return oBitWise.isBitFlagOn(enumBit)
        End Function

        ''' <summary>
        ''' Turns validation off for the last validation flag that failed and returns the updated ValidationFlags value.
        ''' Used to resubmit a request to the server after a user has accepted a validation message.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function acceptLastValidationFlag() As Int64
            Return turnValidationOff(ValidationBitFailed, ValidationFlags)
        End Function

        ''' <summary>
        ''' Will instanciate the validation flag variable and turn validation off for 
        ''' any flag in the list provided.  All other validation flags will be turned on. 
        ''' Returns the updated validation flag variable.  Pass these results to the WCF
        ''' method that requires validation.  Allows for pre validation to be performed 
        ''' on the client; like doFinalize.
        ''' </summary>
        ''' <param name="vFlags"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getPreSetValidationFlags(ByVal vFlags As List(Of ValidationBits)) As Int64
            Dim oBitWise As New CoreUtility.BitwiseFlags(True) 'sets all validation flags to true

            If Not vFlags Is Nothing AndAlso vFlags.Count > 0 Then
                For Each f In vFlags
                    Dim i As Integer = f
                    oBitWise.turnBitFlagOff(i) 'sets this validation flag to off
                    'Dim blnOn = oBitWise.isBitFlagOn(i)
                    'System.Diagnostics.Debug.WriteLine(" {3} is On {0} Flag {1} flags {2}", blnOn, oBitWise.FlagSource, oBitWise.toString(), i)
                Next
            End If

            Return oBitWise.FlagSource 'returns the updated validation flags with the desired flags turned off
        End Function

        ''' <summary>
        ''' Example of how to use the getPreSetValidationFlags function.
        ''' Do not attempt to execute this code as nothing will happen.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub preSetValidationExample()
            Dim oFlags As New List(Of ValidationBits)
            oFlags.Add(ValidationBits.DoFinalize)
            oFlags.Add(ValidationBits.ValidateAlertsBeforeFinalize)
            Dim intValidationFlags As Int64 = getPreSetValidationFlags(oFlags)
            'Pass intValidationFlags to WCF method like:
            'ProcessNewTransCode(BookControl, NewTranCode, OldTranCode, intValidationFlags)
        End Sub

        ''' Modified by RHR for v-8.3.0.002 on 12/17/2020
        '''  added new methods to read and write to three new 
        '''  private properties
        '''  1. AsyncMessagesPossible  -- boolean
        '''  2. AsyncMessageKey  -- integer
        '''  3. AsyncTypeKey  -- integer
        '''  

        Public Sub setAsyncMsgPossible(ByVal blnYN As Boolean)
            _AsyncMessagesPossible = blnYN
        End Sub

        Public Function isAsyncMsgPossible() As Boolean
            Return _AsyncMessagesPossible
        End Function

        Public Sub setAsyncMessageKey(ByVal lngVal As Long)
            _AsyncMessageKey = lngVal
        End Sub

        Public Function getAsyncMessageKey() As Long
            Return _AsyncMessageKey
        End Function

        Public Sub setAsyncTypeKey(ByVal iVal As Integer)
            _AsyncTypeKey = iVal
        End Sub

        Public Function getAsyncTypeKey() As Integer
            Return _AsyncTypeKey
        End Function

        Public Sub configureForAsyncMessages(ByVal lngAsyncMessageKey As Long, Optional ByVal eType As Utilities.NGLLoadTenderTypes = Utilities.NGLLoadTenderTypes.None)
            setAsyncMsgPossible(True)
            setAsyncMessageKey(lngAsyncMessageKey)
            setAsyncTypeKey(CInt(eType))
        End Sub

        Public Function updateMessagesAndLogsWithCarrierCostResults(ByRef Res As CarrierCostResults, ByVal sOrderNumber As String) As Boolean
            Dim blnRet As Boolean = False
            If Not Res.Log Is Nothing AndAlso Res.Log.Count > 0 Then Log.AddRange(Res.Log)
            If Not Res.Messages Is Nothing AndAlso Res.Messages.Count > 0 Then AddRangeToDictionary(WCFResults.MessageType.Messages, Res.Messages)

            If Res Is Nothing Then
                ' the caller must set Success to true or false if this fails
                'E_NoTariffAvailable -- A tariff is not available for the select route CNS: {0}.
                AddMessage(WCFResults.MessageType.Warnings, "E_NoTariffAvailable", {sOrderNumber})
                AddLog("No Tariff is available for Order Number: " & sOrderNumber)
                Return blnRet
            End If
            blnRet = True
            Return blnRet

        End Function


#End Region


#Region "       Message Enum Processing"


        Public Sub AddFaultException(ByVal item As MessageEnum, ByVal r As String, ByVal m As String, ByVal d As String, ByVal ePars As List(Of String))
            Try
                Dim oMsg As New NGLMessage
                Dim par As New List(Of NGLMessage)
                oMsg.populateErrorInformationFromList(r, m, d, ePars)
                par.Add(oMsg)
                If Errors Is Nothing Then Errors = New Dictionary(Of String, List(Of NGLMessage))
                Dim sKey As String = getMessageLocalizedString(item)
                If Errors.ContainsKey(sKey) Then
                    If Not Errors(sKey) Is Nothing Then
                        Errors(sKey).AddRange(par)
                    Else
                        Errors(sKey) = par
                    End If
                Else
                    Errors.Add(getMessageLocalizedString(item), par)
                End If
                AddLog(getMessageNotLocalizedString(item))

            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & Key & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub AddUnexpectedError(ByVal ex As Exception)
            Dim Message = SqlFaultInfo.getFaultMessage(SqlFaultInfo.FaultInfoMsgs.E_UnExpectedMSG)
            Dim Reason = SqlFaultInfo.getFaultReason(SqlFaultInfo.FaultReasons.E_UnExpected)
            Dim Details = SqlFaultInfo.getFaultDetailsKey(SqlFaultInfo.FaultDetailsKey.E_ExceptionMsgDetails)
            AddFaultException(MessageEnum.E_UnExpected_Error, Reason, Message, Details, New List(Of String) From {ex.Message})
        End Sub


        Public Sub AddMessage(ByVal type As MessageType, ByVal key As String, ByVal MsgParameter As NGLMessage)
            Try

                Dim par As New List(Of NGLMessage)
                If Not MsgParameter Is Nothing Then
                    par.Add(MsgParameter)
                End If
                Select Case type
                    Case MessageType.Errors
                        If Errors Is Nothing Then Errors = New Dictionary(Of String, List(Of NGLMessage))
                        If Errors.ContainsKey(key) Then
                            If Not Errors(key) Is Nothing Then
                                Errors(key).AddRange(par)
                            Else
                                Errors(key) = par
                            End If
                        Else
                            Errors.Add(key, par)
                        End If
                    Case MessageType.Warnings
                        If Warnings Is Nothing Then Warnings = New Dictionary(Of String, List(Of NGLMessage))
                        If Warnings.ContainsKey(key) Then
                            If Not Warnings(key) Is Nothing Then
                                Warnings(key).AddRange(par)
                            Else
                                Warnings(key) = par
                            End If
                        Else
                            Warnings.Add(key, par)
                        End If
                    Case MessageType.Messages
                        If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                        If Messages.ContainsKey(key) Then
                            If Not Messages(key) Is Nothing Then
                                Messages(key).AddRange(par)
                            Else
                                Messages(key) = par
                            End If
                        Else
                            Messages.Add(key, par)
                        End If
                End Select

            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal key As String, ByVal blnLog As Boolean, ByVal sLog As String, ByVal ParamArray p() As String)
            Try

                Dim par As New List(Of NGLMessage)
                If Not p Is Nothing AndAlso p.Length > 0 Then
                    For Each s In p
                        par.Add(New NGLMessage(s))
                    Next
                End If
                Select Case type
                    Case MessageType.Errors
                        If Errors Is Nothing Then Errors = New Dictionary(Of String, List(Of NGLMessage))
                        If Errors.ContainsKey(key) Then
                            If Not Errors(key) Is Nothing Then
                                Errors(key).AddRange(par)
                            Else
                                Errors(key) = par
                            End If
                        Else
                            Errors.Add(key, par)
                        End If
                    Case MessageType.Warnings
                        If Warnings Is Nothing Then Warnings = New Dictionary(Of String, List(Of NGLMessage))
                        If Warnings.ContainsKey(key) Then
                            If Not Warnings(key) Is Nothing Then
                                Warnings(key).AddRange(par)
                            Else
                                Warnings(key) = par
                            End If
                        Else
                            Warnings.Add(key, par)
                        End If
                    Case MessageType.Messages
                        If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                        If Messages.ContainsKey(key) Then
                            If Not Messages(key) Is Nothing Then
                                Messages(key).AddRange(par)
                            Else
                                Messages(key) = par
                            End If
                        Else
                            Messages.Add(key, par)
                        End If
                End Select

                If blnLog Then
                    If p Is Nothing OrElse p.Length < 1 Then
                        AddLog(sLog)
                    Else
                        AddLog(String.Format(sLog, p.ToArray()))
                    End If
                End If
            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & key & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal key As String, ByVal ParamArray p() As String)
            AddMessage(type, key, True, "", p)
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal key As String)
            If (Not String.IsNullOrWhiteSpace(key)) Then
                Dim enmVal As MessageEnum
                If ([Enum].TryParse(key, enmVal)) Then
                    Dim sLog As String = getMessageNotLocalizedString(enmVal, key)
                    AddMessage(type, key, True, sLog, Nothing)
                Else
                    AddMessage(type, key, False, "", Nothing)
                End If
            End If
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal item As MessageEnum, ByVal blnLog As Boolean, ByVal ParamArray p() As String)
            Dim key As String = getMessageLocalizedString(item)
            Dim sLog As String = ""
            If blnLog Then sLog = getMessageNotLocalizedString(item)
            AddMessage(type, key, blnLog, sLog, p)
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal item As MessageEnum, ByVal ParamArray p() As String)
            AddMessage(type, item, True, p)
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal item As MessageEnum)
            AddMessage(type, item, True, Nothing)
        End Sub

        Public Sub AddMessage(ByVal type As MessageType, ByVal item As MessageEnum, ByVal s As List(Of String), Optional ByVal blnLog As Boolean = True)
            AddMessage(type, item, blnLog, s.ToArray())
        End Sub

        Public Sub AddLog(ByVal s As String)
            If Me.Log Is Nothing Then Me.Log = New List(Of NGLMessage)
            Me.Log.Add(New NGLMessage(s))
        End Sub

        ''' <summary>
        ''' Adds a new log message with reference link data to the logs
        ''' </summary>
        ''' <param name="m"></param>
        ''' <param name="c"></param>
        ''' <param name="n"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub AddLog(ByVal m As String, ByVal c As Integer, ByVal n As String, ByVal e As Utilities.NGLMessageKeyRef)
            If Me.Log Is Nothing Then Me.Log = New List(Of NGLMessage)
            Me.Log.Add(New NGLMessage(m, c, n, e))
        End Sub

        Public Sub AddLog(ByVal item As MessageEnum, ByVal p As List(Of String))
            Dim key As String = " Undefined "
            Try
                key = getMessageLocalizedString(item)
                Dim sLog As String = getMessageNotLocalizedString(item)
                If p Is Nothing OrElse p.Count() < 1 Then
                    AddLog(sLog)
                Else
                    AddLog(String.Format(sLog, p.ToArray()))
                End If
            Catch ex As System.FormatException
                AddLog("Invalid Message Format; the message [ " & key & " ] may require missing parameters")
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Function formatMsgNotLocalized(ByVal m As String, ByVal p As List(Of NGLMessage)) As String
            If String.IsNullOrWhiteSpace(m) Then Return m
            Dim strRet As String = m
            Try
                If Not p Is Nothing AndAlso p.Count() > 0 Then
                    Dim sList = (From d In p Select d.Message).ToList()
                    strRet = String.Format(m, sList.ToArray())
                End If
            Catch ex As System.FormatException
                strRet = "Invalid Message Format; the message [ " & m & " ] may require missing parameters"
            Catch ex As Exception
                Throw
            End Try
            Return strRet
        End Function

        Public Shared Function getMessageNotLocalizedString(ByVal item As MessageEnum, Optional ByVal sdefault As String = "N/A") As String
            Dim strReturn = sdefault
            Try
                Select Case item
                    Case MessageEnum.M_NoOrdersFound
                        strReturn = "No BookRevenue data found."
                    Case MessageEnum.W_BookingRecordMissingCompany
                        strReturn = "A company has not been assigned to this order."
                    Case MessageEnum.E_CreditHold
                        strReturn = "CREDIT HOLD"
                    Case MessageEnum.E_CreditNotSetMsg
                        strReturn = "*WARNING* This Customer Is On Credit Hold Do Not Place Order!"
                    Case MessageEnum.E_CreditOverLimit
                        strReturn = "*WARNING* This Customer Is Over Their Credit Limit Check With Accounting!"
                    Case MessageEnum.M_BookDeletedEDIMarkedAsExported
                        strReturn = "EDI 204 marked as recieved by carrier for an order modification."
                    Case MessageEnum.M_BookDeletedEDIMarkedAsExportedForCanceled
                        strReturn = "EDI 204 marked as recieved by carrier for a canceled order."
                    Case MessageEnum.Success
                        strReturn = "Success"
                    Case MessageEnum.W_CarrContEmailNotifInfoNotAvailable
                        strReturn = "*WARNING* The carrier contact information is not available."
                    Case MessageEnum.W_AcceptRejectExpire_InvalidCompContEmail
                        strReturn = "Notifications about a loads accept, reject or expire status could not be transmitted to the company contact because of an invalid company contact email address; the system will attempt to use the default notification email settings."
                    Case MessageEnum.W_AcceptRejectExpire_InvalidNotificationEmail
                        strReturn = "Notifications about a loads accept, reject or expire status could not be transmitted because of invalid default notification email address settings."
                    Case MessageEnum.E_CreateExpiredDropLoadFaultException
                        strReturn = "There was a problem creating the Drop Load record when a booking record was expired: {0}."
                    Case MessageEnum.E_ProcessExpiredLoadsError
                        strReturn = "Could not process expired loads for Companany Number: {2}; SQL Error: {0}; Message: {1}."
                    Case MessageEnum.E_ProcessExpiredLoadsErrorNoPro
                        strReturn = "Could not process expired loads; the Book PRO number, {0} cannot be found."
                    Case MessageEnum.MSGdoFinalizeAllExp
                        strReturn = "carrier {0} has a current value of: {1} with a maximum exposure of: {2}."
                    Case MessageEnum.MSGdoFinalizeBooking
                        strReturn = "You are about to accept/finalize the selected load.  Are you sure you want continue?"
                    Case MessageEnum.MSGdoFinalizeContractExp
                        strReturn = "carrier {0} has an expiration date of: {1}."
                    Case MessageEnum.MSGdoFinalizeInsuranceExp
                        strReturn = "carrier {0} has an expiration date of: {1}."
                    Case MessageEnum.MSGdoFinalizePerShipExp
                        strReturn = "carrier {0} has a current value of: {1} with a maximum exposure of: {2}."
                    Case MessageEnum.MSGundoFinalizeBooking
                        strReturn = "You are about to unfinalize the selected load.  Are you sure you want continue?"
                    Case MessageEnum.W_AccessDeniedFinalizeOrdersBeforeShip
                        strReturn = "Access Denied! {0} is not authorized to finalize orders."
                    Case MessageEnum.W_AccessDeniedFinalizeOrdersAfterShip
                        strReturn = "Access Denied! {0} is not authorized to finalize orders after they have shipped."
                    Case MessageEnum.E_SQLFaultCannotFinalize
                        strReturn = "Data Access Failure Cannot Finalize Load."
                    Case MessageEnum.E_ValidateCarrierQualBeforeFinalize_Error
                        strReturn = "Cannot Finalize Load because the Carrier Validation Failed: {0}."
                    Case MessageEnum.W_FinalizePickListFailed
                        strReturn = "Warning! The load was finalized but the pick list status update had errors: {0}."
                    Case MessageEnum.E_SQLFaultCannotProcessNewTranCode
                        strReturn = "SQL Fault Exception! Cannot update tran code."
                    Case MessageEnum.W_AccessDeniedUnFinalizeOrdersBeforeShip
                        strReturn = "Access Denied! {0} is not authorized to unfinalize orders."
                    Case MessageEnum.W_AccessDeniedUnFinalizeOrdersAfterShip
                        strReturn = "Access Denied! {0} is not authorized to unfinalize orders after they have shipped."
                    Case MessageEnum.W_UnFinalizePickListFailed
                        strReturn = "Warning! The load was unfinalized but the pick list status update had errors: {0}."
                    Case MessageEnum.W_UnFinalizeCancelEDIFailed
                        strReturn = "Warning! The load was unfinalized but the process to send a cancel EDI message had errors: {0}."
                    Case MessageEnum.W_AccessDeniedMoveToPCFromPB
                        strReturn = "Access Denied! Cannot go to PC from PB try unfinalizing to P status."
                    Case MessageEnum.W_CannotTenderLoadUpdateSHIDFailed
                        strReturn = "Warning! Cannot tender load because the update SHID method failed, check your load consolidation settings and try again."
                    Case MessageEnum.M_CarrierCostsWillBeUnlocked
                        strReturn = "One or more orders on this load has the carrier costs locked; your action will unlock the carrier costs for all orders on this load.  Are you sure you want to continue?"
                    Case MessageEnum.M_BFCCostsWillBeUnlocked
                        strReturn = "One or more orders on this load has the BFC costs locked; your action will unlock the BFC costs for all orders on this load.  Are you sure you want to continue?"
                    Case MessageEnum.M_CarrierANDBFCCostsWillBeUnlocked
                        strReturn = "One or more orders on this load has the carrier and BFC costs locked; your action will unlock both the carrier and BFC costs for all orders on this load.  Are you sure you want to continue?"
                    Case MessageEnum.M_CarrierCostWereNotUnlocked
                        strReturn = "One or more orders on this load has the carrier costs locked.  Carrier costs will not be updated unless you unlock the costs."
                    Case MessageEnum.M_BFCCostsWereNotUnlocked
                        strReturn = "One or more orders on this load has the BFC costs locked.  BFC costs will not be updated unless you unlock the BFC costs."
                    Case MessageEnum.M_CarrierAndBFCCostWereNotUnlocked
                        strReturn = "One or more orders on this load has the BFC and carrier costs locked.  BFC and Carrier costs will not be updated unless you unlock the costs."
                    Case MessageEnum.MSGChangedFromICToOther
                        strReturn = "You are about to remove the Invoice Complete status of this load, would you like to continue?"
                    Case MessageEnum.M_RejectLoadValidation
                        strReturn = "You are about to reject the selected load all carrier information will be reset. Are you sure you wish to continue?"
                    Case MessageEnum.M_DropLoadValidation
                        strReturn = "You are about to drop the carrier assigned to the selected load; all carrier information will be reset. Are you sure you wish to continue?"
                    Case MessageEnum.W_ManualAutoAcceptNoTenderEmail
                        strReturn = "Warning! The load has been tendered and auto accepted by moving from P to PB.  Load Tender Email are not created.  If a Load Tender is required it must be transmitted manually."
                    Case MessageEnum.W_CarrierDoesNotMatch
                        strReturn = "Warning! Cannot {0} load because the current carrier, {1}, no longer matches the previously assigned carrier: {2}."
                    Case MessageEnum.E_SQLFaultCannotUpdateLoadStatus
                        strReturn = "Data Access Failure Cannot Update Load Status."
                    Case MessageEnum.E_SQLFaultCreateDropLoadRecordFailure
                        strReturn = "Data Access Failure Cannot Save the Drop Load Transaction Record."
                    Case MessageEnum.W_CannotAddAutoTenderLogData
                        strReturn = "Warning! Tender not affected. A load has been auto tendered to carrier {0} but the system could not log the results.  If this load is rejected the maximum number of cascading dispatches may be exceeded."
                    Case MessageEnum.W_CannotAddAutoAcceptLogData
                        strReturn = "Warning! Tender not affected. A load has been auto accepted for carrier {0} but the system could not log the results.  If this load is later rejected the maximum number of cascading dispatches may be exceeded."
                    Case MessageEnum.W_NoAutoTenderData
                        strReturn = "Cannot Auto Tender the load because the Auto Tender Information is not available."
                    Case MessageEnum.W_CannotAutoTenderInvalidTranCode
                        strReturn = "Cannot Auto Tender because one of the Booking Transaction Codes is not allowed for auto tendering."
                    Case MessageEnum.W_NoAutoAcceptData
                        strReturn = "Cannot Auto Accept the load because the Auto Accept Information is not available."
                    Case MessageEnum.W_CannotAutoAcceptInvalidTranCode
                        strReturn = "Cannot Auto Accept because one of the Booking Transaction Codes is not allowed for auto acceptance."
                    Case MessageEnum.W_CannotAutoTenderInvalidCarrier
                        strReturn = "Cannot Auto Tender Load because one or more orders has not been assigned a carrier or the carrier cost is less than or equal to zero."
                    Case MessageEnum.W_CannotAutoAcceptInvalidCarrier
                        strReturn = "Cannot Auto Accept Load because one or more orders has not been assigned a carrier or the carrier cost is less than or equal to zero."
                    Case MessageEnum.W_CannotAutoTenderNotATruckLoad
                        strReturn = "Cannot Auto Tender Load because it does not meet the lane truck load capacity requirements."
                    Case MessageEnum.W_CannotAutoAcceptNotATruckLoad
                        strReturn = "Cannot Auto Accept Load because it does not meet the lane truck load capacity requirements."
                    Case MessageEnum.W_CannotAutoTenderMultiPick
                        strReturn = "Cannot Auto Tender Load because it is a multi-pick load."
                    Case MessageEnum.W_CannotAutoAcceptMultiPick
                        strReturn = "Cannot Auto Accept Load because it is a multi-pick load."
                    Case MessageEnum.W_CannotAutoTenderMultiStop
                        strReturn = "Cannot Auto Tender Load because it is a multi-stop load."
                    Case MessageEnum.W_CannotAutoAcceptMultiStop
                        strReturn = "Cannot Auto Accept Load because it is a multi-stop load."
                    Case MessageEnum.W_CannotAutoTenderGlobalParameterIsOff
                        strReturn = "Cannot Auto Tender Load because the global AutoTender parameter is turned off; value must be equal to 1."
                    Case MessageEnum.W_CannotAutoAcceptGlobalParameterIsOff
                        strReturn = "Cannot Auto Accept Load because the global AutoTender parameter is turned off; value must be equal to 1."
                    Case MessageEnum.W_CannotAutoTenderLaneAutoTenderIsOff
                        strReturn = "Cannot Auto Tender Load because one of the lanes has Auto Tender turned off."
                    Case MessageEnum.W_CannotAutoAcceptLaneAutoTenderIsOff
                        strReturn = "Cannot Auto Accept Load because one of the lanes has Auto Tender turned off."
                    Case MessageEnum.W_CannotCascadeDispatchGlobalParameterIsOff
                        strReturn = "Cannot Cascade Dispatch Load because the global AutoTenderCascadingDispatching parameter is turned off; value must be equal to 1."
                    Case MessageEnum.W_CannotCascadeDispatchLaneIsOff
                        strReturn = "Cannot Cascade Dispatch Load because one of the lanes has Cascading Dispatching turned off."
                    Case MessageEnum.W_CannotCascadeDispatchDepthReached
                        strReturn = "Cannot Cascade Dispatch Load because one of the lanes has Cascading Dispatching turned off."
                    Case MessageEnum.W_CannotCascadeDispatchInvalidTranCode
                        strReturn = "Cannot Cascade Dispatch Load because one of the Booking Transaction Codes is not N."
                    Case MessageEnum.W_CannotCascadeDispatchMultiPick
                        strReturn = "Cannot Cascade Dispatch Load because it is a multi-pick load."
                    Case MessageEnum.W_CannotCascadeDispatchMultiStop
                        strReturn = "Cannot Cascade Dispatch Load because it is a multi-stop load."
                    Case MessageEnum.W_CannotCascadeDispatchNotATruckLoad
                        strReturn = "Cannot Cascade Dispatch Load because it does not meet the lane truck load capacity requirements."
                    Case MessageEnum.W_CannotUpdateDefaultCarrierCheckAlerts
                        strReturn = "Cannot update the default carrier check alerts for more information."
                    Case MessageEnum.E_SQLFaultCannotUpdateBookFuelFeeForLoad
                        strReturn = "Data Access Failure Cannot Update Fuel Fees For Load."
                    Case MessageEnum.E_UnExpected_Error
                        strReturn = "An Unexpected Error Has Occurred: {0}.  You should manually refresh your data to be sure you have the latest changes."
                    Case MessageEnum.E_NotFoundSSOAByUser   'Added by LVV on 3/18/16 for v-7.0.5.1 DAT
                        strReturn = "No Single Sign On Accounts exist for selected user."
                    Case MessageEnum.M_Success              'Added by LVV on 3/18/16 for v-7.0.5.1 DAT
                        strReturn = "Success!"
                    Case MessageEnum.E_InvalidSSOAName      'Added by LVV on 3/18/16 for v-7.0.5.1 DAT
                        strReturn = "No record exists in the database with SSOAName: {0}."
                    Case MessageEnum.E_InvalidSSOAControl      'Added by LVV on 3/18/16 for v-7.0.5.1 DAT
                        strReturn = "No record exists in the database with SSOAControl: {0}."
                    Case MessageEnum.E_InvalidParameterNameValue    'Added by LVV on 5/18/16 for v-7.0.5.110 DAT
                        strReturn = "Invalid Parameter: No record exists in the database for {0}: {1}."
                    Case MessageEnum.E_RequiredFieldDATEquipType    'Added by LVV on 5/18/16 for v-7.0.5.110 DAT
                        strReturn = "The DATEquipType field is required and cannot be null. Please check that a DATEquipType mapping exists for all Temp Type Codes in the Lane Code Maintenance Window and that the Company Level Parameter DATDefaultMixTempType is populated correctly."
                    Case MessageEnum.W_UserNoDATAccount 'Added by LVV 5/23/16 for v-7.0.5.110 DAT
                        strReturn = "User {0} does not have an SSOA account set up for DAT"
                    Case MessageEnum.E_DATGeneralRetMsg 'Added by LVV 6/30/16 for v-7.0.5.110 DAT
                        strReturn = "{0}"
                    Case MessageEnum.E_DATInvalidFeature 'Added by LVV 6/30/16 for v-7.0.5.110 DAT
                        strReturn = "Failure - Could not execute DAT program. Invalid DAT Feature {0}"
                    Case MessageEnum.E_DATDeletePostFailed 'Added by LVV 6/30/16 for v-7.0.5.110 DAT
                        strReturn = "DAT Delete Post Failed -- {0}"
                    Case MessageEnum.E_DATPostFailed 'Added by LVV 6/30/16 for v-7.0.5.110 DAT
                        strReturn = "DAT Post Failed -- {0}"
                    Case MessageEnum.W_CannotAutoAssignDefaultCarrier 'Added by RHR 11/30/16 for v-7.0.6.0 
                        strReturn = "Cannot Auto Assign the default carrier because none of the configured preferred carriers pass the data validation rules.  Please check your Preferred Carrier Settings!"
                    Case MessageEnum.W_CannotAutoAssignSystemDefaultCarrier 'Added by RHR 11/30/16 for v-7.0.6.0 
                        strReturn = "Cannot Auto Assign the system default carrier configured in the global parameter settings.  Lane Default Carrier is turned off and we are using the parameter settings for default carrier.  Check your parameter settings and be sure the carrier is properly configured."
                    Case MessageEnum.E_DATBookRevsNull
                        strReturn = "DAT Post Failed because the BookRevenues cannot be null -- {0}"
                    Case MessageEnum.E_DATCarrierNull
                        strReturn = "DAT Post Failed because the Load Board Carrier could not be found -- {0}"
                    Case MessageEnum.E_NoLoadTenderForBook
                        strReturn = "Could not find Load Tender record with BookSHID: {0}, LoadTenderTypeControl: {1}, and Archived: 0."
                    Case MessageEnum.E_LoadBoardSpFailNoLTControl
                        strReturn = "The stored procedure spInsertLoadBoardRecords did not return a valid LoadTenderControl."
                    Case MessageEnum.E_LBBookRevsNull
                        strReturn = "{0} {1} Failed because the BookRevenues cannot be null. Source: {2}"
                    Case MessageEnum.E_NSP44AcceptNoCarrier
                        strReturn = "NEXTStop Error: Accept P44 Bid failed for LoadTenderControl {0} and BidControl {1}.{2}No CarrierControl was found using CarrierSCAC {3}.{2}Source: {4}."
                    Case MessageEnum.W_NTAllUpdateAssignedInfoFail
                        strReturn = "There was a problem updating assigned information For PRO #{0}. The record was not saved. Source: {1}."
                    Case MessageEnum.W_NTAllGetBookCarrierFilteredFail
                        strReturn = "There was a problem getting dates information for PRO #{0}. Source: {1}"
                    Case MessageEnum.W_NTAllLoadStatusCodeNotSpecified
                        strReturn = "Please enter a reason code for Pro #{0}, the Delivered Date did not meet the Required Date. The record was not saved. Source {1}."
                    Case MessageEnum.W_NTAllSetCommentsForChangedDatesAndTimesFail
                        strReturn = "There was a problem setting comments for changed dates and times for PRO #{0}. Comments were not saved. Source {1}."
                    Case MessageEnum.W_NTAllSetCommentsRelatedItemsFail
                        strReturn = "There was a problem setting comments for related items PRO #{0}. Comments were not saved. Source: {1}."
                    Case MessageEnum.W_NTAllSendCommentsEmailFail
                        strReturn = "There was a problem sending comments email for PRO #{0}. Source: {1}."
                    Case MessageEnum.M_NTAllTabSaveSuccess
                        strReturn = "PRO #{0}{1}{2} has been saved successfully."
                    Case MessageEnum.M_NTAllTabSaveWarnings
                        strReturn = "PRO #{0}{1}{2} has been saved with Warnings."

                    Case MessageEnum.E_SaveRateSpFailNoLTControl 'Added by RHR for v-8.2 on 12/17/2018 for new Quote Generation logic
                        strReturn = "Save rate failure a load tender record could not be created."
                    Case MessageEnum.E_SaveRateFailure 'Added by RHR for v-8.2 on 12/17/2018 for new Quote Generation logic
                        strReturn = "Save Rate Failure for LoadTenderControl = {0}: {1}."
                    Case MessageEnum.E_ReadQuoteSpFailNoLTControl 'Added by RHR for v-8.2 on 12/17/2018 for new Quote Generation logic
                        strReturn = "Read quote failure a load tender record could not be created."
                    Case MessageEnum.E_ReadQuoteFailure 'Added by RHR for v-8.2 on 12/17/2018 for new Quote Generation logic
                        strReturn = "Read Quote Failure for LoadTenderControl = {0}: {1}."
                    Case MessageEnum.E_NotFoundCarrierAPISSOAByUser 'The current user {0} does not have an account to access the Carrier API interface'
                        strReturn = "The current user {0} does not have an account to access the Carrier API interface"
                    Case MessageEnum.E_SystemWarning 'Warning: {0}.'
                        strReturn = "Warning: {0}."
                    Case MessageEnum.E_SystemInfo 'Info: {0}.'
                        strReturn = "Info: {0}."
                    Case MessageEnum.W_MissingLoadTenderDispatched 'Warning: Your load has been dispatched; however, we could not save the booking information.  Please create the booking information manually. {0}'
                        strReturn = "Warning: Your load has been dispatched; however, we could not save the booking information.  Please create the booking information manually. {0}"
                    Case MessageEnum.E_ReadDispatchBidFailure 'The system was unable to read the selected bid using bid control number {0}.  The load may have already been dispatched. {1}'
                        strReturn = "The system was unable to read the selected bid using bid control number {0}.  The load may have already been dispatched. {1}"
                    Case MessageEnum.E_DispatchFailure 'The system was unable to dispatch the selected bid using bid control number {0}. {1}'
                        strReturn = "The system was unable to dispatch the selected bid using bid control number {0}. {1}"
                    Case MessageEnum.E_SaveTariffRevisionFailed 'Unable to save new tariff revision changes: {0}
                        strReturn = "Unable to save new tariff revision changes: {0}"
                End Select

            Catch ex As System.ArgumentNullException
                'enum type or value is nothing so return default
                Return strReturn
            Catch ex As System.ArgumentException
                'the item is not valid so return default
                Return strReturn
            Catch ex As Exception
                Throw
            End Try

            Return strReturn
        End Function

        Public Shared Function getMessageLocalizedString(ByVal item As MessageEnum, Optional ByVal sdefault As String = "N/A") As String
            Dim strReturn = sdefault
            Try
                Dim Enumerator As Type = GetType(MessageEnum)
                strReturn = [Enum].GetName(Enumerator, item)
            Catch ex As System.ArgumentNullException
                'enum type or value is nothing so return default
                Return strReturn
            Catch ex As System.ArgumentException
                'the item is not valid so return default
                Return strReturn
            Catch ex As Exception
                Throw
            End Try

            Return strReturn
        End Function

        ''' <summary>
        ''' Parses the MessageEnum using strEnum and returns the actual Enum if strEnum is not valid 
        ''' retuns the default MessageEnum.None
        ''' </summary>
        ''' <param name="strEnum"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Caller should test for MessageEnum.None and process the messages accordingly
        ''' </remarks>
        Public Function getMessageEnumFromString(ByVal strEnum As String) As MessageEnum
            Dim enmVal As MessageEnum = MessageEnum.None
            [Enum].TryParse(strEnum, enmVal)
            Return enmVal
        End Function

        ''' <summary>
        ''' checks if the key exists and adds or updates the value for key
        ''' </summary>
        ''' <param name="k">Key</param>
        ''' <param name="v">Value</param>
        ''' <remarks></remarks>
        Public Sub updateKeyFields(ByVal k As String, ByVal v As String)
            If Not KeyFields.ContainsKey(k) Then
                KeyFields.Add(k, v)
            ElseIf KeyFields(k) <> v Then
                KeyFields(k) = v
            End If
        End Sub

        ''' <summary>
        ''' sets the action property to type and the ActionName to the type name
        ''' </summary>
        ''' <param name="type"></param>
        ''' <remarks></remarks>
        Public Sub setAction(ByVal type As ActionEnum)
            Me.Action = type
            Me.ActionName = [Enum].GetName(GetType(ActionEnum), type)
        End Sub

        ''' <summary>
        ''' sets the ValidationBitFailed property and optionally adds a default message for the type.  
        ''' NOTE: Some validaiton types like ValidateAlertsBeforeFinalize do not have a default message.
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="blnAddMessage"></param>
        ''' <remarks></remarks>
        Public Sub setValidationFailed(ByVal type As ValidationBits, Optional ByVal blnAddMessage As Boolean = True)
            ValidationBitFailed = type
            If blnAddMessage Then
                Select Case type
                    Case ValidationBits.CarrierANDBFCCostsWillBeUnlocked
                        AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_CarrierANDBFCCostsWillBeUnlocked)
                    Case ValidationBits.CarrierOrBFCCostsWillBeUnlocked
                        AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_CarrierCostsWillBeUnlocked)
                    Case ValidationBits.DoFinalize
                        AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.MSGdoFinalizeBooking)
                    Case ValidationBits.ReverseTheInvoiceCompleteStatus
                        AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.MSGChangedFromICToOther)
                    Case ValidationBits.UndoFinalize
                        AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.MSGundoFinalizeBooking)
                    Case ValidationBits.RejectTheLoad
                        AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_RejectLoadValidation)
                    Case ValidationBits.DropTheLoad
                        AddMessage(FreightMaster.Data.DataTransferObjects.WCFResults.MessageType.Messages, FreightMaster.Data.DataTransferObjects.WCFResults.MessageEnum.M_DropLoadValidation)
                End Select

            End If



        End Sub

        Public Sub setReport(ByVal type As ReportEnum)
            Me.Report = [Enum].GetName(GetType(ReportEnum), type)
        End Sub

        Public Function getAllMessagesNotLocalized() As String
            Dim sb As New System.Text.StringBuilder()
            Return getAllMessagesNotLocalized(sb).ToString()
        End Function

        Public Function getAllMessagesNotLocalized(ByRef sb As System.Text.StringBuilder) As System.Text.StringBuilder
            concatMessage(sb, Messages)
            concatMessage(sb, Warnings)
            concatErrors(sb, Errors)
            Return sb
        End Function

        Public Function concatWarnings() As String
            If Warnings Is Nothing OrElse Warnings.Count < 1 Then Return ""
            Dim sb As New System.Text.StringBuilder()
            For Each m In Warnings
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                            Try
                                sb.AppendFormat(sMsg, m.Value.Select(Function(x) x.Message).ToArray())
                            Catch ex As System.FormatException
                                sb.Append(sMsg)
                            End Try
                        Else
                            sb.Append(sMsg)
                        End If
                        sb.AppendLine()
                    End If
                End If
            Next
            Return sb.ToString()
        End Function

        Public Sub concatMessage(ByRef sb As System.Text.StringBuilder, ByVal dMsgs As Dictionary(Of String, List(Of NGLMessage)))
            If dMsgs Is Nothing OrElse dMsgs.Count < 1 Then Return
            If sb Is Nothing Then sb = New System.Text.StringBuilder()
            For Each m In dMsgs
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                            Try
                                sb.AppendFormat(sMsg, m.Value.Select(Function(x) x.Message).ToArray())
                            Catch ex As System.FormatException
                                sb.Append(sMsg)
                            End Try
                        Else
                            sb.Append(sMsg)
                        End If
                        sb.AppendLine()
                    End If
                End If
            Next
        End Sub

        Public Function concatMessage() As String
            If Messages Is Nothing OrElse Messages.Count < 1 Then Return ""
            Dim sb As New System.Text.StringBuilder()
            For Each m In Messages
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                            Try
                                sb.AppendFormat(sMsg, m.Value.Select(Function(x) x.Message).ToArray())
                            Catch ex As System.FormatException
                                sb.Append(sMsg)
                            End Try
                        Else
                            sb.Append(sMsg)
                        End If
                        sb.AppendLine()
                    End If
                End If
            Next
            Return sb.ToString()
        End Function

        Public Sub concatErrors(ByRef sb As System.Text.StringBuilder, ByVal dErrs As Dictionary(Of String, List(Of NGLMessage)))
            If dErrs Is Nothing OrElse dErrs.Count < 1 Then Return
            If sb Is Nothing Then sb = New System.Text.StringBuilder()
            Dim blnAppendDefault As Boolean = False
            For Each m In dErrs
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        sb.Append(sMsg)
                    Else
                        sb.Append(m.Key)
                    End If
                    sb.AppendLine()
                    If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                        'append the error Reason 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorReason) Then
                            blnAppendDefault = False
                            Dim eReason = SqlFaultInfo.getFaultReasonsEnumFromString(m.Value(0).ErrorReason)
                            Try
                                If eReason = SqlFaultInfo.FaultReasons.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim sReason = SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(eReason, "")
                                    If Not String.IsNullOrWhiteSpace(sReason) Then
                                        sb.AppendFormat("{0}Error Reason: {1}", vbTab, sReason)
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Reason: {1}", vbTab, m.Value(0).ErrorReason)
                            End If
                            sb.AppendLine()
                        End If
                        'append the error message info 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorMessage) Then
                            blnAppendDefault = False
                            Dim eInfo = SqlFaultInfo.getFaultInfoMsgsEnumFromString(m.Value(0).ErrorMessage)
                            Try
                                If eInfo = SqlFaultInfo.FaultInfoMsgs.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim sInfo = SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(eInfo, "")
                                    If Not String.IsNullOrWhiteSpace(sInfo) Then
                                        sb.AppendFormat("{0}Error Info: {1}", vbTab, sInfo)
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Info: {1}", vbTab, m.Value(0).ErrorMessage)
                            End If
                            sb.AppendLine()
                        End If
                        'append the error details 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorDetails) Then
                            blnAppendDefault = False
                            Dim eDet = SqlFaultInfo.getFaultDetailsKeyEnumFromString(m.Value(0).ErrorDetails)
                            Try
                                If eDet = SqlFaultInfo.FaultDetailsKey.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim strDet = SqlFaultInfo.getFaultDetailsKeyNotLocalizedString(eDet, "")
                                    If Not String.IsNullOrWhiteSpace(strDet) Then
                                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorCSVDetailsParameters) Then
                                            sb.AppendFormat("{0}Error Details: ", vbTab)
                                            sb.AppendFormat(strDet, m.Value(0).ErrorCSVDetailsParameters.Split(","))
                                        Else
                                            sb.AppendFormat("{0}Error Details: {1}", vbTab, strDet)
                                        End If
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Details: {1}", vbTab, m.Value(0).ErrorDetails)
                            End If
                            sb.AppendLine()
                        End If
                    End If
                End If
            Next
        End Sub

        Public Function concatErrors() As String
            If Errors Is Nothing OrElse Errors.Count < 1 Then Return ""
            Dim sb As New System.Text.StringBuilder()
            Dim blnAppendDefault As Boolean = False
            For Each m In Errors
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        sb.Append(sMsg)
                    Else
                        sb.Append(m.Key)
                    End If
                    sb.AppendLine()
                    If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                        'append the error Reason 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorReason) Then
                            blnAppendDefault = False
                            Dim eReason = SqlFaultInfo.getFaultReasonsEnumFromString(m.Value(0).ErrorReason)
                            Try
                                If eReason = SqlFaultInfo.FaultReasons.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim sReason = SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(eReason, "")
                                    If Not String.IsNullOrWhiteSpace(sReason) Then
                                        sb.AppendFormat("{0}Error Reason: {1}", vbTab, sReason)
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Reason: {1}", vbTab, m.Value(0).ErrorReason)
                            End If
                            sb.AppendLine()
                        End If
                        'append the error message info 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorMessage) Then
                            blnAppendDefault = False
                            Dim eInfo = SqlFaultInfo.getFaultInfoMsgsEnumFromString(m.Value(0).ErrorMessage)
                            Try
                                If eInfo = SqlFaultInfo.FaultInfoMsgs.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim sInfo = SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(eInfo, "")
                                    If Not String.IsNullOrWhiteSpace(sInfo) Then
                                        sb.AppendFormat("{0}Error Info: {1}", vbTab, sInfo)
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Info: {1}", vbTab, m.Value(0).ErrorMessage)
                            End If
                            sb.AppendLine()
                        End If
                        'append the error details 
                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorDetails) Then
                            blnAppendDefault = False
                            Dim eDet = SqlFaultInfo.getFaultDetailsKeyEnumFromString(m.Value(0).ErrorDetails)
                            Try
                                If eDet = SqlFaultInfo.FaultDetailsKey.None Then
                                    blnAppendDefault = True
                                Else
                                    Dim strDet = SqlFaultInfo.getFaultDetailsKeyNotLocalizedString(eDet, "")
                                    If Not String.IsNullOrWhiteSpace(strDet) Then
                                        If Not String.IsNullOrWhiteSpace(m.Value(0).ErrorCSVDetailsParameters) Then
                                            sb.AppendFormat("{0}Error Details: ", vbTab)
                                            sb.AppendFormat(strDet, m.Value(0).ErrorCSVDetailsParameters.Split(","))
                                        Else
                                            sb.AppendFormat("{0}Error Details: {1}", vbTab, strDet)
                                        End If
                                    Else
                                        blnAppendDefault = True
                                    End If
                                End If
                            Catch ex As System.FormatException
                                blnAppendDefault = True
                            End Try
                            If blnAppendDefault Then
                                sb.AppendFormat("{0}Error Details: {1}", vbTab, m.Value(0).ErrorDetails)
                            End If
                            sb.AppendLine()
                        End If
                    End If
                End If
            Next
            Return sb.ToString()
        End Function

        ''' <summary>
        ''' Adds the entire range of the parameter dictionary d to the
        ''' WCFResults object property specified by the parameter type
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="d"></param>
        ''' <remarks>
        ''' Added by LVV 7/6/16 for v-7.0.5.110 DAT
        ''' The reason I chose to make the parameter a dictionary instead of
        ''' another WCFResults object is so that I can add the dictionaries of
        ''' other objects as well such as DATResults etc.
        ''' </remarks>
        Public Sub AddRangeToDictionary(ByVal type As MessageType, ByVal d As Dictionary(Of String, List(Of NGLMessage)))

            If Not d Is Nothing Then

                Select Case type
                    Case MessageType.Errors
                        If Errors Is Nothing Then Errors = New Dictionary(Of String, List(Of NGLMessage))
                        For Each kv As KeyValuePair(Of String, List(Of NGLMessage)) In d
                            If Not Errors.ContainsKey(kv.Key) Then
                                Errors.Add(kv.Key, kv.Value)
                            Else
                                Errors(kv.Key).AddRange(kv.Value)
                            End If
                        Next
                    Case MessageType.Warnings
                        If Warnings Is Nothing Then Warnings = New Dictionary(Of String, List(Of NGLMessage))
                        For Each kv As KeyValuePair(Of String, List(Of NGLMessage)) In d
                            If Not Warnings.ContainsKey(kv.Key) Then
                                Warnings.Add(kv.Key, kv.Value)
                            Else
                                Warnings(kv.Key).AddRange(kv.Value)
                            End If
                        Next
                    Case MessageType.Messages
                        If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                        For Each kv As KeyValuePair(Of String, List(Of NGLMessage)) In d
                            If Not Messages.ContainsKey(kv.Key) Then
                                Messages.Add(kv.Key, kv.Value)
                            Else
                                Messages(kv.Key).AddRange(kv.Value)
                            End If
                        Next
                End Select
            End If
        End Sub

#End Region


#Region "       Key Field Processing"

        ''' <summary>
        ''' Returns the current string value for key or default if key does not exist
        ''' </summary>
        ''' <param name="sKey"></param>
        ''' <param name="sDefault"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 12/29/2018 to simplify reading of keys 
        ''' 
        ''' </remarks>
        Public Function TryGetKeyValue(ByVal sKey As String, Optional ByVal sDefault As String = "") As String
            Dim sRet As String = sDefault
            If Not Me.KeyFields Is Nothing _
                AndAlso Me.KeyFields.Count > 0 _
                AndAlso Me.KeyFields.ContainsKey(sKey) Then sRet = Me.KeyFields(sKey)
            Return sRet
        End Function

        ''' <summary>
        ''' Returns as array of values with the same index as sKeys using tryParse to convert
        ''' strings to integers, iDefaults is used when nothing exists or if the string in key
        ''' cannot be converted to an integer
        ''' </summary>
        ''' <param name="sKeys"></param>
        ''' <param name="iDefaults"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 12/29/2018 to simplify reading of keys 
        ''' 
        ''' </remarks>
        Public Function TryGetKeyInts(ByVal sKeys() As String, ByVal iDefaults() As Integer) As Integer()
            If sKeys Is Nothing OrElse sKeys.Count < 1 Then
                Return {0} 'just return one item set to zero
            End If
            If iDefaults Is Nothing OrElse iDefaults.Count < 1 Then
                iDefaults = {0}  'just create one default and  set it to zero
            End If
            Dim iRet(sKeys.Count) As Integer
            'assign the defaults
            For i As Integer = 0 To iRet.Count() - 1
                If iDefaults.Count() > i Then iRet(i) = iDefaults(i)
            Next
            If Not Me.KeyFields Is Nothing AndAlso Me.KeyFields.Count > 0 Then
                For i As Integer = 0 To sKeys.Count() - 1
                    Dim iVal As Integer = 0
                    If iDefaults.Count() > i Then iVal = iDefaults(i)
                    If Me.KeyFields.ContainsKey(sKeys(i)) Then Integer.TryParse(Me.KeyFields(sKeys(i)), iVal)
                    iRet(i) = iVal
                Next
            End If

            Return iRet
        End Function

        ''' <summary>
        ''' Returns as array of string values with the same index as sKeys using index in sDefaults if the key does not exist
        ''' </summary>
        ''' <param name="sKeys"></param>
        ''' <param name="sDefaults"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 12/29/2018 to simplify reading of keys 
        ''' 
        ''' </remarks>
        Public Function TryGetKeys(ByVal sKeys() As String, ByVal sDefaults() As String) As String()
            If sKeys Is Nothing OrElse sKeys.Count < 1 Then
                Return {""} 'just return one item set to an empty string
            End If
            If sDefaults Is Nothing OrElse sDefaults.Count < 1 Then
                sDefaults = {""}  'just create one default and set an empty string
            End If
            Dim sRet(sKeys.Count) As String
            'assign the defaults
            For i As Integer = 0 To sRet.Count() - 1
                If sDefaults.Count() > i Then sRet(i) = sDefaults(i)
            Next
            If Not Me.KeyFields Is Nothing AndAlso Me.KeyFields.Count > 0 Then
                For i As Integer = 0 To sKeys.Count() - 1
                    Dim sVal As String = "0"
                    If sDefaults.Count() > i Then sVal = sDefaults(i)
                    If Me.KeyFields.ContainsKey(sKeys(i)) Then sVal = Me.KeyFields(sKeys(i))
                    sRet(i) = sVal
                Next
            End If

            Return sRet
        End Function

        ''' <summary>
        ''' Attempts to parse the key into an integer if it exists uses iVal as the default and updates iVal where found with the key value.
        ''' Returns true if the key value exists and is converted to an integer
        ''' </summary>
        ''' <param name="sKey"></param>
        ''' <param name="iVal"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 12/29/2018 to simplify reading of keys 
        ''' 
        ''' </remarks>
        Public Function TryParseKeyInt(ByVal sKey As String, ByRef iVal As Integer) As Boolean
            Dim blnRet As Boolean = False

            If Not Me.KeyFields Is Nothing AndAlso Me.KeyFields.Count > 0 Then
                If Me.KeyFields.ContainsKey(sKey) Then blnRet = Integer.TryParse(Me.KeyFields(sKey), iVal)
            End If

            Return blnRet
        End Function

        ''' <summary>
        ''' Attempts to parse the key into a Double if it exists uses dVal as the default and updates dVal where found with the key value.
        ''' Returns true if the key value exists and is converted to a Double
        ''' </summary>
        ''' <param name="sKey"></param>
        ''' <param name="dVal"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 01/01/2019 to simplify reading of keys 
        ''' 
        ''' </remarks>
        Public Function TryParseKeyDbl(ByVal sKey As String, ByRef dVal As Double) As Boolean
            Dim blnRet As Boolean = False

            If Not Me.KeyFields Is Nothing AndAlso Me.KeyFields.Count > 0 Then
                If Me.KeyFields.ContainsKey(sKey) Then blnRet = Double.TryParse(Me.KeyFields(sKey), dVal)
            End If

            Return blnRet
        End Function

        ''' <summary>
        ''' Returns as array of values with the same index as sKeys using tryParse to convert
        ''' strings to Doubles, dDefaults is used when nothing exists or if the string in key
        ''' cannot be converted to a Double
        ''' </summary>
        ''' <param name="sKeys"></param>
        ''' <param name="dDefaults"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-8.2 01/01/2019 to simplify reading of keys 
        ''' 
        ''' </remarks>
        Public Function TryGetKeyDbls(ByVal sKeys() As String, ByVal dDefaults() As Double) As Double()
            If sKeys Is Nothing OrElse sKeys.Count < 1 Then
                Return {0} 'just return one item set to zero
            End If
            If dDefaults Is Nothing OrElse dDefaults.Count < 1 Then
                dDefaults = {0}  'just create one default and  set it to zero
            End If
            Dim dRet(sKeys.Count) As Double
            'assign the defaults
            For i As Integer = 0 To dRet.Count() - 1
                If dDefaults.Count() > i Then dRet(i) = dDefaults(i)
            Next
            If Not Me.KeyFields Is Nothing AndAlso Me.KeyFields.Count > 0 Then
                For i As Integer = 0 To sKeys.Count() - 1
                    Dim dVal As Double = 0
                    If dDefaults.Count() > i Then dVal = dDefaults(i)
                    If Me.KeyFields.ContainsKey(sKeys(i)) Then Double.TryParse(Me.KeyFields(sKeys(i)), dVal)
                    dRet(i) = dVal
                Next
            End If

            Return dRet
        End Function




#End Region


    End Class

End Namespace
