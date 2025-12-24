Imports System
Imports System.Collections.Generic
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.Linq
Imports System.ServiceModel
Imports Microsoft.VisualBasic
Imports NGL.Core.Utility
Imports NGL.FreightMaster.Data.Utilities
Imports BidTypeEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum
Imports BSCEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidStatusCodeEnum
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports DTran = NGL.Core.Utility.DataTransformation
Imports LTS = NGL.FreightMaster.Data.LTS
Imports LTSCEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderStatusCodeEnum
Imports LTTypeEnum = NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.LoadTenderTypeEnum
Imports P44 = NGL.FM.P44
Imports Models = NGL.FreightMaster.Data.Models
Imports System.Windows.Forms
Imports System.Drawing.Printing
Imports NGL.FreightMaster.Core.Model
Imports NGL.FM.CarTar
Imports Serilog
Imports SerilogTracing


'Added by LVV 5/23/16 for v-7.0.5.110 DAT
Public Class NGLDATBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLDATBLL"
        Me.Logger = Me.Logger.ForContext(Of NGLDATBLL)
    End Sub

#End Region

#Region "Delegates"
    'Modified by RHR for v-7.0.5.103 on 02/19/2017
    Public Delegate Sub CreateNGLTariffBidDelegate(ByVal BookControl As Integer, ByVal LoadTenderControl As Integer, ByVal SHID As String, ByVal strMsg As String, ByVal tariffOptions As DTO.GetCarriersByCostParameters)
    Public Delegate Sub CreateNGLTariffBidNoBookDelegate(ByVal oP44Request As P44.RateRequest, ByVal LoadTenderControl As Integer, ByVal tariffOptions As DTO.GetCarriersByCostParameters)
    Public Delegate Sub CreateNGLTariffBidFromRateRequestOrderDelegate(ByVal order As DAL.Models.RateRequestOrder, ByVal LoadTenderControl As Integer, ByVal tariffOptions As DTO.GetCarriersByCostParameters)

#End Region

    Public Enum BookTrackLSCEnum
        LBPost = 500
        LBDelete = 501
        LBExpired = 502
        LBError = 503
    End Enum

#Region "DAT Methods"

    ''' <summary>
    ''' getAdditionalDATLTInfo
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="DATFeature"></param>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 9/29/20 for v-8.3.0.001 Task #20200930125350 - DAT Migration
    '''   Bug fix - incorrect null check on USATToken was causing missing information in the return value lt (USC in particular)
    '''   which caused problems with the Login
    ''' </remarks>
    Public Function getAdditionalDATLTInfo(ByVal LTControl As Integer, ByVal DATFeature As Integer, ByVal UserName As String) As DTO.tblLoadTender
        Dim lt As New DTO.tblLoadTender
        Dim oUSAT As New DAL.NGLtblUserSecurityAccessTokenData(Me.Parameters)

        'get back the lt record that was just created in the DAT sp
        lt = NGLLoadTenderData.GetLoadTenderFiltered(LTControl)

        'get the user security control for this user
        Dim uSec = NGLSecurityData.GettblUserSecurityByUserName(UserName)
        Dim USATToken As New DTO.tblUserSecurityAccessToken
        Dim UserSecurityControl As Integer = 0
        If Not uSec Is Nothing Then
            UserSecurityControl = uSec.UserSecurityControl
            'get the token from the database for this user and DAT
            USATToken = oUSAT.GettblUserSecurityAccessTokenFiltered(UserSecurityControl, SSOAAccount.DAT)
        End If

        'GettblSingleSignOnAccountControl

        If Not USATToken Is Nothing Then
            lt.TokenString = USATToken.USATToken
            lt.TokenExpiresDate = USATToken.USATExpires
        End If
        With lt
            .DATFeature = DATFeature
            .Database = Me.Parameters.Database
            .DBServer = Me.Parameters.DBServer
            .ConnectionString = Me.Parameters.ConnectionString
            .UserName = UserName
            .UserSecurityControl = UserSecurityControl
        End With
        getDATAccountInfo(UserSecurityControl, SSOAAccount.DAT, lt)
        Return lt
    End Function

    Public Sub getDATAccountInfo(ByVal UserSecurityControl As Integer, ByVal SSOAControl As DAL.Utilities.SSOAAccount, ByRef lt As DTO.tblLoadTender)

        Dim SSOA = NGLSSOAData.GetSingleSignOnAccountByUser(UserSecurityControl, SSOAControl)
        If SSOA Is Nothing Then
            'something went wrong -- send and error msg or whatever
        End If

        For Each s In SSOA
            If Not s.Warnings Is Nothing Then
                For Each k In s.Warnings.Keys
                    'If Not String.Equals(k, "E_NotFoundSSOAByUser") Then 'We don't need to show a popup if there are no SSOAs for this user
                    '    Dim nglMsgs = a.Warnings(k).ToList()
                    '    Dim FMMsgs = FMNGLMessage.convertNGLMessageListToFMNGLMessageList(nglMsgs)
                    '    If Not FMMsgs Is Nothing AndAlso FMMsgs.Count > 0 Then
                    '        strFormatted &= FMLocalizeNGLMessageData.LocalizeNGLMessage(k, FMMsgs) & vbCrLf & vbCrLf
                    '    End If
                    'End If
                Next
            End If
            'Process SSOA Accounts and convert to FMRollCenterSSOA objects
            ' Modified by RHR for v-8.2 12/29/2018 to simplify reading of WCFResults keys 
            Dim sVals = s.TryGetKeys({"Username", "Pass", "SSOALoginURL"}, {"", "", ""})
            With lt
                .SSOAUserName = sVals(0)
                .SSOAPassword = sVals(1)
                .SSOALoginURL = sVals(2)
            End With
        Next

    End Sub

    Public Function DATPost(ByRef results As DTO.WCFResults, ByRef oBookRevs As DTO.BookRevenue(), ByRef blnDATGetDataSuccess As Boolean, ByRef blnDATPostingSuccess As Boolean, ByVal oSelectedBooking As DTO.BookRevenue) As DTO.DATResults
        Dim datRes As New DTO.DATResults
        Dim LoadTenderControl As Integer = 0
        'Post
        results.AddLog("Execute sp to add DAT Post data to tblLoadTender")
        'Insert record into tblLoadTender for this Posting
        Dim wcfRes = NGLLoadTenderData.InsertLoadBoardRecords(oSelectedBooking.BookControl, oSelectedBooking.BookSHID, LTTypeEnum.DAT)
        If Not wcfRes.Success = True Then blnDATGetDataSuccess = False
        If blnDATGetDataSuccess Then
            If Not wcfRes.Warnings Is Nothing AndAlso wcfRes.Warnings.Count > 0 Then
                'The sp failed so nothing can be posted so skip changing tran code and return to the caller to display the message to the user
                blnDATPostingSuccess = False
                datRes.Success = False
                datRes.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcfRes.Warnings)
                If datRes.Warnings?.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
                Return datRes
            End If
            'get the record back using Control and post to DAT
            ' Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys   
            wcfRes.TryParseKeyInt("LoadTenderControl", LoadTenderControl)
            If LoadTenderControl = 0 Then
                datRes.Success = False
                datRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_LoadBoardSpFailNoLTControl)
                If datRes.Warnings?.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
                Return datRes
            End If
            'If the sp Insert worked then execute the Posting to the DAT Load Board
            Dim feature = DAT.Infrastructure.Feature.Post
            Dim lt = NGLDATBLL.getAdditionalDATLTInfo(LoadTenderControl, feature, Parameters.UserName)
            datRes = DAT.DAT.processData(lt, Parameters)
            If datRes.Warnings?.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
            blnDATPostingSuccess = datRes.Success
            If blnDATPostingSuccess Then
                For Each b In oBookRevs
                    b.BookRevLoadTenderTypeControl = UpdateBookRevLTTC(b.BookRevLoadTenderTypeControl, LTTypeEnum.DAT, True)
                    'b.BookRevLoadTenderStatusCode = NGLDATBLL.UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.DATPosted)
                    b.BookRevLoadTenderStatusCode = UpdateBookRevLTSCPost(b.BookRevLoadTenderStatusCode, LTTypeEnum.DAT)
                Next
            End If
        Else
            If Not wcfRes.Warnings Is Nothing AndAlso wcfRes.Warnings.Count > 0 Then
                'The sp failed so nothing can be posted so skip changing tran code and return to the caller to display the message to the user
                blnDATPostingSuccess = False
                datRes.Success = False
                datRes.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcfRes.Warnings)
                If datRes.Warnings?.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
                Return datRes
            End If
        End If
        Return datRes
    End Function

    Public Function DATUpdatePost(ByRef results As DTO.WCFResults, ByRef oBookRevs As DTO.BookRevenue(), ByRef blnDATIsUpdate As Boolean, ByRef blnDATPostingSuccess As Boolean, ByVal oSelectedBooking As DTO.BookRevenue) As DTO.DATResults
        Dim datRes As New DTO.DATResults
        Dim wgt As Integer = 0
        Dim cube As Double = 0
        Dim pallet As Integer = 0
        Dim cases As Double = 0
        Dim miles As Double = 0
        Dim TotalWgt As Integer? = Nothing
        Dim TotalCube As Double? = Nothing
        Dim TotalPL As Integer? = Nothing
        Dim TotalCases As Double? = Nothing
        Dim TotalMiles As Double? = Nothing
        Dim Comment1 As String = Nothing
        Dim Comment2 As String = Nothing
        Dim blnFieldChanged As Boolean = False
        '-------------------------------------
        blnDATIsUpdate = True

        Dim pLTControl = NGLLoadTenderData.GetLTControlFiltered(BookSHID:=oSelectedBooking.BookSHID, intLoadTenderType:=LTTypeEnum.DAT, intStatusCode:=LTSCEnum.DATPosted, Archived:=0)

        If pLTControl = Nothing OrElse pLTControl = 0 Then
            'Could not find Load Tender record with BookSHID: {0}, LoadTenderTypeControl: {1}, and Archived: 0.
            Dim p() As String = {oSelectedBooking.BookSHID, LTTypeEnum.DAT.ToString()}
            datRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_NoLoadTenderForBook)
            datRes.Success = False
            blnDATPostingSuccess = datRes.Success
            datRes.LTStatusCode = LTSCEnum.DATError
            datRes.LTMessage = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_NoLoadTenderForBook), p)
            If Not datRes.Warnings Is Nothing AndAlso datRes.Warnings.Count > 0 Then
                results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
            End If
            Return datRes
        End If

        Dim feature = NGL.FM.DAT.Infrastructure.Feature.UpdatePost
        Dim lt = NGLDATBLL.getAdditionalDATLTInfo(pLTControl, feature, Parameters.UserName)

        Dim wcf = NGLLoadTenderData.GetDATStopData(oSelectedBooking.BookControl)
        ' Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys  
        Dim iVals = wcf.TryGetKeyInts({"BookTotalWgt", "BookTotalPL"}, {wgt, pallet})
        wgt = iVals(0)
        pallet = iVals(1)

        ' Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys  
        Dim dVals = wcf.TryGetKeyDbls({"BookTotalCube", "BookTotalCube", "BookMilesFrom"}, {cube, cases, miles})
        cube = dVals(0)
        cases = dVals(1)
        miles = dVals(2)


        'Update any values that have changed
        If lt.LTBookTotalWgt <> wgt Then
            lt.LTBookTotalWgt = wgt
            TotalWgt = wgt
            blnFieldChanged = True
        End If
        If lt.LTBookTotalCube <> cube Then
            lt.LTBookTotalCube = cube
            TotalCube = cube
            blnFieldChanged = True
        End If
        If lt.LTBookTotalPL <> pallet Then
            lt.LTBookTotalPL = pallet
            TotalPL = pallet
            blnFieldChanged = True
        End If
        If lt.LTBookTotalCases <> cases Then
            lt.LTBookTotalCases = cases
            TotalCases = cases
            blnFieldChanged = True
        End If
        If lt.LTBookTotalMiles <> miles Then
            lt.LTBookTotalMiles = miles
            TotalMiles = miles
            blnFieldChanged = True
        End If
        ' Modified by RHR for v-8.2 01/01/2019 to improve and simplify reading of WCFResults keys
        Dim sDATComment1 = wcf.TryGetKeyValue("DATComment1")
        If lt.LTDATComment1 <> sDATComment1 Then
            lt.LTDATComment1 = sDATComment1
            Comment1 = sDATComment1
            blnFieldChanged = True
        End If
        ' Modified by RHR for v-8.2 01/01/2019 to improve and simplify reading of WCFResults keys
        Dim sDATComment2 = wcf.TryGetKeyValue("DATComment2")
        If lt.LTDATComment2 <> sDATComment2 Then
            lt.LTDATComment1 = sDATComment2
            Comment2 = sDATComment2
            blnFieldChanged = True
        End If

        datRes = DAT.DAT.processData(lt, Parameters)

        If datRes.Success AndAlso blnFieldChanged Then
            NGLLoadTenderData.updateLoadTender(pLTControl, TotalWgt:=TotalWgt, TotalCube:=TotalCube, DATComment1:=Comment1, DATComment2:=Comment2, TotalPL:=TotalPL, TotalCases:=TotalCases, TotalMiles:=TotalMiles)
        End If

        '------------------------------
        If Not datRes.Warnings Is Nothing AndAlso datRes.Warnings.Count > 0 Then
            results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
        End If
        blnDATPostingSuccess = datRes.Success
        If blnDATPostingSuccess Then
            For Each b In oBookRevs
                b.BookRevLoadTenderStatusCode = NGLDATBLL.UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.DATUpdated)
            Next
        End If

        Return datRes
    End Function

    Public Function DeleteDATPosting(ByRef results As DTO.WCFResults, ByRef oBookRevs As DTO.BookRevenue(), ByRef blnDATDeleteSuccess As Boolean, ByVal eAcceptReject As NGLBookBLL.AcceptRejectEnum) As DTO.DATResults
        Dim userName As String = Parameters.UserName
        Dim spEx = " "
        Dim source As String = "NGL.FM.BLL.NGLDATBLL.DeleteDATPosting"
        Dim lt As New DTO.tblLoadTender
        Dim datRes As New DTO.DATResults
        datRes.Success = True
        Dim ltsc = LTSCEnum.DATDeleted

        'Get the most recent record from the LoadTender table for BookSHID where Type is DAT, Status Code is Posted or Updated, and Archived is false
        'I filter by SHID instead of BookControl because a DAT LT record is one record for a load. I think there is ond SHID for a load
        'Dim pLT = NGLLoadTenderData.GetLoadTenderFiltered(BookSHID:=oBookRevs(0).BookSHID, intLoadTenderType:=LTTypeEnum.DAT, intStatusCode:=DTO.tblLoadTender.LoadTenderStatusCodeEnum.DATPosted, Archived:=0)
        Dim statusCodes = New Integer() {LTSCEnum.DATPosted, LTSCEnum.DATUpdated}
        Dim pLT = NGLLoadTenderData.GetLTFilteredByStatusCodes(statusCodes, BookSHID:=oBookRevs(0).BookSHID, intLoadTenderType:=LTTypeEnum.DAT, Archived:=0)

        If Not pLT Is Nothing Then
            If eAcceptReject = NGLBookBLL.AcceptRejectEnum.Expired Then
                userName = pLT.LTDATPoster
                spEx = " expired "
                ltsc = LTSCEnum.DATExpired
            End If
            'since there is no system account for DAT, use the credentials of the user who did the posting to do the delete
            'this is not the best way to do it but it will be fine for now. Could get 2 processes trying to use same credentials at same time
            Dim uSec = NGLSecurityData.GettblUserSecurityByUserName(userName)
            If Not uSec Is Nothing Then
                'make sure this is valid account
                If Not NGLSSOAData.DoesUserHaveSSOAAccount(uSec.UserSecurityControl, DAL.Utilities.SSOAAccount.DAT) Then
                    datRes.Success = False
                    Dim p() As String = {spEx, userName, uSec.UserSecurityControl.ToString(), "DAT"}
                    datRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_LBDeleteFailedNoUser, p)
                    Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_LBDeleteFailedNoUser), p)
                    NGLSystemLogData.AddApplicaitonLog(msg, source)
                Else
                    If pLT.LoadTenderControl <> 0 Then
                        Dim feature = NGL.FM.DAT.Infrastructure.Feature.Delete
                        lt = NGLDATBLL.getAdditionalDATLTInfo(pLT.LoadTenderControl, feature, userName)
                        'do the delete
                        datRes = DAT.DAT.processData(lt, Parameters)
                        'If there were any error messages then we need to log them
                        If datRes.Success = False Then
                            Dim msg = datRes.concatWarnings()
                            NGLSystemLogData.AddApplicaitonLog(msg, source)
                        Else
                            'If success is true but there were warnings save those to the log
                            If datRes.Warnings.Count > 0 Then
                                Dim msg = datRes.concatWarnings()
                                NGLSystemLogData.AddApplicaitonLog(msg, source)
                            End If
                        End If
                    End If
                End If
            Else
                datRes.Success = False
                Dim p() As String = {userName}
                datRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_NoUserSecurityForUser, p)
                Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_NoUserSecurityForUser), p)
                NGLSystemLogData.AddApplicaitonLog(msg, source)
            End If
        Else
            datRes.Success = False
            Dim p() As String = {oBookRevs(0).BookSHID.ToString(), DTO.tblLoadTender.LoadTenderTypeEnum.DAT.ToString()}
            datRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_NoLoadTenderForBook, p)
            Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_NoLoadTenderForBook), p)
            NGLSystemLogData.AddApplicaitonLog(msg, source)
        End If

        If Not datRes.Warnings Is Nothing AndAlso datRes.Warnings.Count > 0 Then
            results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, datRes.Warnings)
        End If

        blnDATDeleteSuccess = datRes.Success
        If blnDATDeleteSuccess Then
            For Each b In oBookRevs
                b.BookRevLoadTenderStatusCode = NGLDATBLL.UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, ltsc)
            Next
        End If

        Return datRes
    End Function

    'Public Function DATAccept(ByVal LTControl As Integer, ByVal BookControl As Integer, ByVal BookSHID As String) As DTO.WCFResults
    '    Dim oBookRevs As DTO.BookRevenue()
    '    Dim results As New DTO.WCFResults
    '    Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
    '    Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
    '    Dim oBrack As New DAL.NGLtblTarBracketTypeData(Parameters)

    '    Dim source = "NGL.FM.BLL.NGLDATBLL.DATAccept"

    '    oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)

    '    Dim bwBRLTSC As New BitwiseFlags32(LTSC)
    '    Dim bwBRLTType As New BitwiseFlags32(LTType)

    '    'Check for active NS Posting
    '    If HasActiveLBPosting(LTTypeEnum.NextStop, bwBRLTType, bwBRLTSC) Then
    '        'Delete the NS Posting
    '        Dim blnNSDeleteSuccess As Boolean = True
    '        Dim nsRes = NGLDATBLL.NSDeletePost(results, oBookRevs, blnNSDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected, True)
    '        NGLDATBLL.ProcessLoadBoardStatusUpdates(nsRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnNSDeleteSuccess, True, True, source, False)
    '    End If

    '    Dim oBR = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl, True)

    '    For Each b In oBR
    '        b.BookRevLoadTenderStatusCode = UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.DATAccepted)
    '    Next

    '    results = oBookBLL.ProcessNewTransCode(oBR, BookControl, "PB", "P", results)

    '    NGLLoadTenderData.updateLoadTender(LTControl, Parameters.UserName, "", LTSCEnum.DATAccepted, Nothing)

    '    Return results
    'End Function


#End Region

#Region "Next Stop Methods"

    Public Function NSPost(ByRef results As DTO.WCFResults, ByRef oBookRevs As DTO.BookRevenue(), ByRef blnNSGetDataSuccess As Boolean, ByRef blnNSPostingSuccess As Boolean, ByVal oSelectedBooking As DTO.BookRevenue, Optional ByVal tariffOptions As DTO.GetCarriersByCostParameters = Nothing) As DTO.DATResults
        Dim nsRes As New DTO.DATResults
        Dim LoadTenderControl As Integer = 0
        Dim RetMsg As String = ""
        If tariffOptions Is Nothing Then tariffOptions = New DTO.GetCarriersByCostParameters(False, False, True, 0, 0) 'use default for standard routing prefered = false, noLateDelivery = False, validated = True, optimizeByCapacity = True --  modeTypeControl and  tempType  are looked up later

        'These will always be valid
        nsRes.LTBookControl = oSelectedBooking.BookControl
        nsRes.UserName = Parameters.UserName
        nsRes.LTTypeControl = LTTypeEnum.NextStop
        'Post
        results.AddLog("Execute sp to add NEXTStop Post data to tblLoadTender")
        'Insert record into tblLoadTender for this Posting
        Dim wcfRes = NGLLoadTenderData.InsertLoadBoardRecords(oSelectedBooking.BookControl, oSelectedBooking.BookSHID, LTTypeEnum.NextStop)
        If Not wcfRes.Success = True Then blnNSGetDataSuccess = False
        If blnNSGetDataSuccess Then
            'Check for Warnings
            If wcfRes.Warnings?.Count > 0 Then
                'The sp failed so nothing can be posted so skip changing tran code and return to the caller to display the message to the user
                nsRes.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcfRes.Warnings)
                Try
                    RetMsg = wcfRes.TryGetKeyValue("RetMsg") 'Modified by RHR for v-8.2 01/01/2019 to improve and simplify reading of WCFResults keys
                Catch ex As Exception
                    'do nothing
                End Try
                If Not String.IsNullOrWhiteSpace(RetMsg) Then nsRes.LTMessage = RetMsg 'If the sp returned an error message we have to update the LTMessage field and also change the Status Code to NStopError
                nsRes.LTStatusCode = LTSCEnum.NStopError
                nsRes.Success = False
                blnNSPostingSuccess = False
                If nsRes.Warnings?.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, nsRes.Warnings)
                Return nsRes
            End If
            'In this case the Insert to tblLoadTender is the Post
            'So all we have to do is verify the Post was successful by getting back a
            'non-zero LoadTenderControl and then update the required fields in nsRes
            wcfRes.TryParseKeyInt("LoadTenderControl", LoadTenderControl) 'Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys 
            If LoadTenderControl = 0 Then
                nsRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_LoadBoardSpFailNoLTControl)
                Dim s = nsRes.LTMessage = DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_LoadBoardSpFailNoLTControl)
                Try
                    RetMsg = wcfRes.TryGetKeyValue("RetMsg") 'Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys
                Catch ex As Exception
                    'do nothing
                End Try
                'If the sp returned an error message we have to update the LTMessage field and also change the Status Code to NStopError
                If Not String.IsNullOrWhiteSpace(RetMsg) Then nsRes.LTMessage = RetMsg + ". " + s Else nsRes.LTMessage = s
                nsRes.LTStatusCode = LTSCEnum.NStopError
                nsRes.Success = False
                blnNSPostingSuccess = False
                If nsRes.Warnings?.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, nsRes.Warnings)
                Return nsRes
            End If
            'The Post Worked so we can update these fields
            nsRes.LTControl = LoadTenderControl
            nsRes.LTDATRefID = LoadTenderControl
            nsRes.LTStatusCode = LTSCEnum.NStopPosted
            nsRes.Success = True
            'Send Emails to Carriers
            NGLCarrierData.SendNEXTStopCarrierEmails(oSelectedBooking.BookControl)
            'nsRes.LTBookSHID = ""
            'nsRes.LTCarrierName = ""
            'nsRes.LTCarrierNumber = 0
            'Get the P44 bids for the load -Asych
            'TODO -- eventually we will have to pass in some login in params for P44
            NGLLoadTenderData.CreateP44BidAsync(oSelectedBooking.BookControl, LoadTenderControl, oSelectedBooking.BookSHID)
            CreateNGLTariffBidAsync(oSelectedBooking.BookControl, LoadTenderControl, oSelectedBooking.BookSHID, "", tariffOptions)
            If nsRes.Warnings?.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, nsRes.Warnings)
            blnNSPostingSuccess = nsRes.Success
            If blnNSPostingSuccess Then
                For Each b In oBookRevs
                    'b.BookRevLoadTenderStatusCode = NGLDATBLL.UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.NStopPosted)
                    b.BookRevLoadTenderTypeControl = UpdateBookRevLTTC(b.BookRevLoadTenderTypeControl, LTTypeEnum.NextStop, True)
                    b.BookRevLoadTenderStatusCode = UpdateBookRevLTSCPost(b.BookRevLoadTenderStatusCode, LTTypeEnum.NextStop)
                Next
            End If
        Else
            If wcfRes.Warnings?.Count > 0 Then
                'The sp failed so nothing can be posted so skip changing tran code and return to the caller to display the message to the user
                nsRes.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcfRes.Warnings)
                Try
                    RetMsg = wcfRes.TryGetKeyValue("RetMsg") 'Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys
                Catch ex As Exception
                    'do nothing
                End Try
                If Not String.IsNullOrWhiteSpace(RetMsg) Then nsRes.LTMessage = RetMsg 'If the sp returned an error message we have to update the LTMessage field and also change the Status Code to NStopError
                nsRes.LTStatusCode = LTSCEnum.NStopError
                nsRes.Success = False
                blnNSPostingSuccess = False
                If nsRes.Warnings?.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, nsRes.Warnings)
            End If
        End If
        Return nsRes
    End Function

    Public Function NSUpdatePost(ByRef results As DTO.WCFResults, ByRef oBookRevs As DTO.BookRevenue(), ByRef blnNSIsUpdate As Boolean, ByRef blnNSPostingSuccess As Boolean, ByVal oSelectedBooking As DTO.BookRevenue) As DTO.DATResults
        Dim nsRes As New DTO.DATResults
        Dim lt As New DTO.tblLoadTender
        Dim wgt As Integer = 0
        Dim cube As Double = 0
        Dim pallet As Integer = 0
        Dim cases As Double = 0
        Dim miles As Double = 0
        Dim TotalWgt As Integer? = Nothing
        Dim TotalCube As Double? = Nothing
        Dim TotalPL As Integer? = Nothing
        Dim TotalCases As Double? = Nothing
        Dim TotalMiles As Double? = Nothing
        Dim Comment1 As String = Nothing
        Dim Comment2 As String = Nothing
        Dim blnFieldChanged As Boolean = False
        '-------------------------------------
        'These will always be valid
        blnNSIsUpdate = True
        nsRes.LTBookControl = oSelectedBooking.BookControl
        nsRes.UserName = Parameters.UserName
        nsRes.LTTypeControl = LTTypeEnum.NextStop

        Dim pLTControl = NGLLoadTenderData.GetLTControlFiltered(BookSHID:=oSelectedBooking.BookSHID, intLoadTenderType:=LTTypeEnum.NextStop, intStatusCode:=LTSCEnum.NStopPosted, Archived:=0)

        If pLTControl = Nothing OrElse pLTControl = 0 Then
            'Could not find Load Tender record with BookSHID: {0}, LoadTenderTypeControl: {1}, and Archived: 0.
            Dim p() As String = {oSelectedBooking.BookSHID, LTTypeEnum.NextStop.ToString()}
            nsRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_NoLoadTenderForBook)
            nsRes.Success = False
            blnNSPostingSuccess = nsRes.Success
            nsRes.LTStatusCode = LTSCEnum.NStopError
            nsRes.LTMessage = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_NoLoadTenderForBook), p)
            If Not nsRes.Warnings Is Nothing AndAlso nsRes.Warnings.Count > 0 Then
                results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, nsRes.Warnings)
            End If
            Return nsRes
        End If

        lt = NGLLoadTenderData.GetLoadTenderFiltered(pLTControl)
        Dim wcf = NGLLoadTenderData.GetDATStopData(oSelectedBooking.BookControl)
        ' Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys  
        Dim iVals = wcf.TryGetKeyInts({"BookTotalWgt", "BookTotalPL"}, {wgt, pallet})
        wgt = iVals(0)
        pallet = iVals(1)

        ' Modified by RHR for v-8.2 01/01/2019 to simplify reading of WCFResults keys  
        Dim dVals = wcf.TryGetKeyDbls({"BookTotalCube", "BookTotalCube", "BookMilesFrom"}, {cube, cases, miles})
        cube = dVals(0)
        cases = dVals(1)
        miles = dVals(2)


        'Compare the original LT record to the newly created data from database
        'Update any values that have changed
        If lt.LTBookTotalWgt <> wgt Then
            lt.LTBookTotalWgt = wgt
            TotalWgt = wgt
            blnFieldChanged = True
        End If
        If lt.LTBookTotalCube <> cube Then
            lt.LTBookTotalCube = cube
            TotalCube = cube
            blnFieldChanged = True
        End If
        If lt.LTBookTotalPL <> pallet Then
            lt.LTBookTotalPL = pallet
            TotalPL = pallet
            blnFieldChanged = True
        End If
        If lt.LTBookTotalCases <> cases Then
            lt.LTBookTotalCases = cases
            TotalCases = cases
            blnFieldChanged = True
        End If
        If lt.LTBookTotalMiles <> miles Then
            lt.LTBookTotalMiles = miles
            TotalMiles = miles
            blnFieldChanged = True
        End If
        ' Modified by RHR for v-8.2 01/01/2019 to improve and simplify reading of WCFResults keys
        Dim sDATComment1 = wcf.TryGetKeyValue("DATComment1")
        If lt.LTDATComment1 <> sDATComment1 Then
            lt.LTDATComment1 = sDATComment1
            Comment1 = sDATComment1
            blnFieldChanged = True
        End If
        ' Modified by RHR for v-8.2 01/01/2019 to improve and simplify reading of WCFResults keys
        Dim sDATComment2 = wcf.TryGetKeyValue("DATComment2")
        If lt.LTDATComment2 <> sDATComment2 Then
            lt.LTDATComment1 = sDATComment2
            Comment2 = sDATComment2
            blnFieldChanged = True
        End If


        If blnFieldChanged Then
            NGLLoadTenderData.updateLoadTender(pLTControl, TotalWgt:=TotalWgt, TotalCube:=TotalCube, DATComment1:=Comment1, DATComment2:=Comment2, TotalPL:=TotalPL, TotalCases:=TotalCases, TotalMiles:=TotalMiles)
        End If

        'The Update Worked so we can update these fields
        nsRes.Success = True
        nsRes.LTControl = pLTControl
        nsRes.LTDATRefID = pLTControl
        nsRes.LTStatusCode = LTSCEnum.NStopUpdated

        'nsRes.LTBookSHID = ""
        'nsRes.LTCarrierName = ""
        'nsRes.LTCarrierNumber = 0

        If Not nsRes.Warnings Is Nothing AndAlso nsRes.Warnings.Count > 0 Then
            results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, nsRes.Warnings)
        End If
        blnNSPostingSuccess = nsRes.Success
        If blnNSPostingSuccess Then
            For Each b In oBookRevs
                b.BookRevLoadTenderStatusCode = NGLDATBLL.UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.NStopUpdated)
            Next
        End If

        Return nsRes
    End Function

    ''' <summary>
    ''' Deletes a NextStop record by Archiving the record and setting the Status Code to either NStopExpired Or NStopDeleted.
    ''' Also deletes any Active Bids associated with this Posting by setting BidArchived to True And the BidStatus to BidStatusCode Or Expired.
    ''' Note: This applies to Bids of all types and for all carriers.
    '''  This sp is only to be called from inside the AcceptReject method Or when Ops chooses to manually delete a posting from the TMS 365 NSOps screen
    ''' </summary>
    ''' <param name="results"></param>
    ''' <param name="oBookRevs"></param>
    ''' <param name="blnNSDeleteSuccess"></param>
    ''' <param name="eAcceptReject"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function NSDeletePost(ByRef results As DTO.WCFResults, ByRef oBookRevs As DTO.BookRevenue(), ByRef blnNSDeleteSuccess As Boolean, ByVal eAcceptReject As NGLBookBLL.AcceptRejectEnum, Optional ByVal BidStatusCode As Integer = BSCEnum.OpsDeletePost) As DTO.DATResults
        Dim source As String = "NGL.FM.BLL.NGLDATBLL.NSDeletePost"
        Dim userName As String = Parameters.UserName
        Dim usc As Integer = Parameters.UserControl
        Dim ltsc As Integer = LTSCEnum.NStopDeleted
        Dim spEx = " "

        Dim nsRes As New DTO.DATResults With {.Success = True, .LTTypeControl = LTTypeEnum.NextStop, .UserName = userName}

        'Get the most recent record from the LoadTender table for BookSHID where Type is DAT, Status Code is Posted or Updated, and Archived is false
        'I filter by SHID instead of BookControl because a LB LT record is one record for a load. I think there is ond SHID for a load        
        Dim statusCodes = New Integer() {LTSCEnum.NStopPosted, LTSCEnum.NStopUpdated}
        Dim pLT = NGLLoadTenderData.GetLTFilteredByStatusCodes(statusCodes, BookSHID:=oBookRevs(0).BookSHID, intLoadTenderType:=LTTypeEnum.NextStop, Archived:=0)

        If Not pLT Is Nothing Then
            With nsRes
                .LTControl = pLT.LoadTenderControl
                .LTBookControl = pLT.LTBookControl
                .LTDATRefID = pLT.LoadTenderControl
                .LTCarrierName = pLT.LTCarrierName
                .LTCarrierNumber = pLT.LTCarrierNumber
                .LTCarrierControl = pLT.LTCarrierControl
                .LTBookCustCompControl = pLT.LTBookCustCompControl
                .LTCompName = pLT.LTCompName
                .LTBookSHID = pLT.LTBookSHID
            End With

            If eAcceptReject = NGLBookBLL.AcceptRejectEnum.Expired Then
                spEx = " expired "
                ltsc = LTSCEnum.NStopExpired
                BidStatusCode = BSCEnum.Expired
            End If

            'Modified By LVV on 6/11/19 for BUG FIX (TEMPORARY?)
            'Anytime the carrier is changed on a load it should automatically remove the load from NEXTStop (archive). The New code changes for Rating may have caused a bug. This bug appears to be related to user sercurity And access to NEXTStop.  
            'For now remove sercurity To limit user permissions To NEXTStop (comment out). Except For Posting.

            ''Dim blnHasPermission = False
            ''If userName.ToLower() = "nglweb" Then
            ''    blnHasPermission = True 'LVV - I added this so that the ProcessExpiredLoads CMDLine process could work properly since it is system and not a user
            ''Else
            ''    'First use the Control we already have in Parameters - if this doesn't exist use the Parameters.UserName to look it up
            ''    If usc = 0 Then
            ''        Dim uSec = NGLSecurityData.GettblUserSecurityByUserName(userName)
            ''        If Not uSec Is Nothing Then usc = uSec.UserSecurityControl
            ''    End If
            ''    'verify user account exists
            ''    If usc <> 0 Then
            ''        If NGLSSOAData.DoesUserHaveSSOAAccount(usc, SSOAAccount.NextStop) Then 'make sure this is valid account
            ''            blnHasPermission = True
            ''        Else
            ''            nsRes.Success = False
            ''            Dim p() As String = {spEx, userName, usc.ToString(), "NEXTStop"}
            ''            nsRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_LBDeleteFailedNoUser, p)
            ''            Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_LBDeleteFailedNoUser), p)
            ''            NGLSystemLogData.AddApplicaitonLog(msg, source)
            ''        End If
            ''    Else
            ''        nsRes.Success = False
            ''        Dim p() As String = {userName}
            ''        nsRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_NoUserSecurityForUser, p)
            ''        Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_NoUserSecurityForUser), p)
            ''        NGLSystemLogData.AddApplicaitonLog(msg, source)
            ''        nsRes.LTStatusCode = LTSCEnum.NStopError
            ''    End If
            ''End If

            'If we have the correct permissions then do the delete
            ''If blnHasPermission Then
            If pLT.LoadTenderControl <> 0 Then
                'do the delete
                Dim wcf = NGLLoadTenderData.DeleteNextStopLoad(pLT.LoadTenderControl, ltsc, BidStatusCode)
                If wcf.Success = False Then nsRes.LTStatusCode = LTSCEnum.NStopError Else nsRes.LTStatusCode = ltsc
                nsRes.Success = wcf.Success
                If Not wcf.Warnings Is Nothing AndAlso wcf.Warnings.Count > 0 Then nsRes.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcf.Warnings)
            End If
            ''End If
        Else
            nsRes.Success = False
            Dim p() As String = {oBookRevs(0).BookSHID.ToString(), LTTypeEnum.DAT.ToString()}
            nsRes.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_NoLoadTenderForBook, p)
            Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_NoLoadTenderForBook), p)
            NGLSystemLogData.AddApplicaitonLog(msg, source)
            nsRes.LTStatusCode = LTSCEnum.NStopError
        End If

        'If there were any error messages then we need to log them
        If nsRes.Warnings.Count > 0 Then
            Dim msg = nsRes.concatWarnings()
            NGLSystemLogData.AddApplicaitonLog(msg, source)
            nsRes.LTMessage = msg
        End If

        If Not nsRes.Warnings Is Nothing AndAlso nsRes.Warnings.Count > 0 Then results.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, nsRes.Warnings)

        blnNSDeleteSuccess = nsRes.Success
        If blnNSDeleteSuccess Then
            For Each b In oBookRevs
                b.BookRevLoadTenderStatusCode = NGLDATBLL.UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, ltsc)
            Next
        End If

        Return nsRes
    End Function

    Public Function RunNEXTStopDeleteMethod(ByVal BookControl As Integer) As DTO.WCFResults
        Dim selectedLoadBoards As New BitwiseFlags32()
        selectedLoadBoards.turnBitFlagOn(LTTypeEnum.NextStop)
        Dim results = LoadBoardDeleteMethod(BookControl, selectedLoadBoards)
        Return results
    End Function


    ''' <summary>
    ''' Accept Bid Posted By Carrier to NEXTStop
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BidControl"></param>
    ''' <param name="BidCarrierControl"></param>
    ''' <param name="BidLineHaul"></param>
    ''' <param name="BidFuelUOM"></param>
    ''' <param name="BidFuelVariable"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Added parameter CarrierContControl (gets passed in from dispatch model)
    '''   Added logic to pass CarrierContControl into the method GetDefaultContactForCarrier() as an optional param
    ''' NOTE:
    '''  The parameter CarrierContControl will eventually be optional with a Default value of 0. 
    '''  However, in initial development do not make it optional to ensure proper testing
    '''  When unit testing is complete change the parameter to optional
    ''' Modified by RHR for v-8.2 on 7/16/2019 
    '''   added logic to use the Carrier Pro as the SHID if provided
    ''' Modified by RHR for v-8.5.2.003 on 08/14/2023 
    '''     added logic to include Rate It shipment id when needed
    '''     Added logic To Get the Next consolidation number When needed
    ''' </remarks>
    Public Function NSAccept(ByVal LTControl As Integer,
                             ByVal BookControl As Integer,
                             ByVal BidControl As Integer,
                             ByVal BidCarrierControl As Integer,
                             ByVal BidLineHaul As Decimal,
                             ByVal BidFuelUOM As String,
                             ByVal BidFuelVariable As Decimal,
                             ByVal CarrierContControl As Integer,
                             Optional ByVal oDispatch As DAL.Models.Dispatch = Nothing) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim bookfees As New List(Of DTO.BookFee)
        Dim wcf As New DTO.WCFResults
        Dim oBookBLL As New NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim oBrack As New DAL.NGLtblTarBracketTypeData(Parameters)

        Dim source = "NGL.FM.BLL.NGLDATBLL.NSAccept"
        'ProcessNewTransCode(BookControl, "N", "PC")

        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)

        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        'To determine active postings we must check both BookRevLoadTenderStatusCode and BookRevLoadTenderTypeControl flags
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'Delete the DAT Posting
            Dim blnDATDeleteSuccess As Boolean = True
            Dim datRes = NGLDATBLL.DeleteDATPosting(wcf, oBookRevs, blnDATDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected)
            'We have to at least save the 2 BookRevLT fields
            'results.AddLog("Save changes to booking records")
            'NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
            NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnDATDeleteSuccess, True, True, source, False)
        End If
        'Modified by RHR for v-8.2 on 7/16/2019 
        Dim blnUseCarrierProAsSHID As Boolean = If(oDispatch Is Nothing, False, If(String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber), False, If(oDispatch.CarrierProNumber.Trim.Length > 0, True, False)))
        For Each b In oBookRevs
            b.BookRevLoadTenderStatusCode = UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.NStopAccepted)
            b.BookCarrTarName = "SPOT RATE"
            'Modified by RHR for v-8.5.2.003 on 08/14/2023 added logic to include Rate It shipment id when needed
            'Added logic to get the next consolidation number when needed
            If blnUseCarrierProAsSHID AndAlso Not String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber) Then
                b.BookSHID = oDispatch.CarrierProNumber
            ElseIf String.IsNullOrWhiteSpace(b.BookSHID) Then
                If Not (String.IsNullOrWhiteSpace(oDispatch.SHID)) Then
                    b.BookSHID = oDispatch.SHID
                End If
            End If
            If String.IsNullOrWhiteSpace(b.BookConsPrefix) Then
                If Not (String.IsNullOrWhiteSpace(oDispatch.SHID)) Then
                    b.BookConsPrefix = oDispatch.SHID
                Else
                    b.BookConsPrefix = NGLBookData.GetNextCNSNumber(b.BookCustCompControl)
                End If
            End If
            If String.IsNullOrWhiteSpace(b.BookSHID) Then
                b.BookSHID = b.BookConsPrefix
            End If
        Next

        Dim allocationBFCFormula As New DTO.tblTarBracketType
        Dim allocationFormula = oBrack.GettblTarBracketTypeFiltered(4)

        If BidFuelVariable <> 0 Then
            Dim AccessorialCode As Integer = 0
            Dim bookFeesVar As Decimal = BidFuelVariable
            Dim bookfeesmin As Decimal = 0
            'TODO -- fix the case statment/UOM field to not use strings eventually
            'This will have to happen after we figure out how to do drop down lists in TMS 365
            Select Case BidFuelUOM
                Case "Flat Rate"
                    AccessorialCode = 15
                    bookfeesmin = BidFuelVariable
                    bookFeesVar = 0
                Case "Rate Per Mile"
                    AccessorialCode = 9
                Case "Percentage"
                    AccessorialCode = 2
                Case Else
                    AccessorialCode = 15
                    bookfeesmin = BidFuelVariable
                    bookFeesVar = 0
            End Select
            Dim acc = NGLtblAccessorialData.GettblAccessorialFiltered(AccessorialCode)

            Dim bookFee As New DTO.BookFee
            With bookFee
                .BookFeesBookControl = BookControl
                .BookFeesVariable = bookFeesVar
                .BookFeesMinimum = bookfeesmin
                .BookFeesAccessorialCode = AccessorialCode
                .BookFeesAccessorialFeeTypeControl = 3
                .BookFeesVariableCode = acc.AccessorialVariableCode
                .BookFeesVisible = 1
                .BookFeesCaption = acc.AccessorialCaption
                .BookFeesTaxable = 1
                .BookFeesAccessorialFeeAllocationTypeControl = 4
                .BookFeesTarBracketTypeControl = 4
                .BookFeesAccessorialFeeCalcTypeControl = 2
                .BookFeesModDate = Date.Now
                .BookFeesModUser = Parameters.UserName
            End With

            bookfees.Add(bookFee)
        End If

        Dim totalbfc As Decimal = oBookRevs(0).BookSpotRateTotalUnallocatedBFC
        If totalbfc = Nothing Then totalbfc = 0

        Dim params As New DTO.SpotRate
        With params
            .BookRevs = oBookRevs.ToList()
            .allocationFormula = allocationFormula
            .totalLineHaulCost = BidLineHaul
            .BookFees = bookfees
            .DeleteTariffFees = True
            .DeleteLaneFees = True
            .DeleteOrderFees = True
            .UseCarrierFuelAddendum = False
            .CarrierControl = BidCarrierControl
            '.State = Nothing
            '.EffectiveDate = Nothing
            '.AvgFuelPrice = Nothing
            .AutoCalculateBFC = True
            .TotalBFC = totalbfc
            .AllocationBFCFormula = allocationBFCFormula
            .BookRevNegRevenueValue = 1
        End With

        'get the carrier contact
        Dim CarrContact = NGLCarrierContData.GetDefaultContactForCarrier(BidCarrierControl, CarrierContControl) 'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch -- added CarrierContControl parameter to method and passed it in as optional param to GetDefaultContactForCarrier()

        oBookRevBLL.DoSpotRateSave(params, CarrContact)

        'Modified by RHR for v-8.2 on 6/30/2019 added logic to support AutoAcceptOnDispatch flag
        Dim sNewTranCode As String = "PC"
        Dim iOverrideSendLoadTenderEmail = 0
        If Not oDispatch Is Nothing Then
            If oDispatch.AutoAcceptOnDispatch = True Then sNewTranCode = "PB"
            If oDispatch.EmailLoadTenderSheet = True Then
                iOverrideSendLoadTenderEmail = 1
            Else
                iOverrideSendLoadTenderEmail = -1
            End If
        End If

        Dim res = oBookBLL.ProcessNewTransCode(BookControl, sNewTranCode, "P", 0, BSCEnum.OpsDeletePost, iOverrideSendLoadTenderEmail)

        If Not wcf.Warnings Is Nothing AndAlso wcf.Warnings.Count > 0 Then
            res.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcf.Warnings)
        End If

        'update bid records
        NGLLoadTenderData.AcceptNextStopBid(LTControl, BidControl)

        Return res
    End Function

#End Region

#Region "Shared Load Board Methods"

    Public Sub ProcessLoadBoardStatusUpdates(ByRef lbRes As DTO.DATResults,
                                        ByVal AcceptRejectCode As NGLBookBLL.AcceptRejectEnum,
                                        ByVal blnDeleteSuccess As Boolean,
                                        ByVal blnPostingSuccess As Boolean,
                                        ByVal blnBookRevSaveComplete As Boolean,
                                        ByVal source As String,
                                        Optional ByVal blnIsUpdate As Boolean = False)

        Dim btcomm As String = ""
        'I did this like this here because it was the quickest and easiest way to get the info without changing
        'a bunch of other methods/tables/etc. So far we are only using this field datRes.LTTypeName here
        Dim strName = NGLLoadTenderData.GetLoadTenderTypeName(lbRes.LTTypeControl)
        lbRes.LTTypeName = strName


        Select Case AcceptRejectCode
            Case NGLBookBLL.AcceptRejectEnum.Tendered
                If blnIsUpdate Then
                    If blnPostingSuccess Then
                        'update the fields in LT table and post success book track msg
                        btcomm = "Post with Reference ID " + lbRes.LTDATRefID + " Updated on " + lbRes.LTTypeName + "."
                        NGLBookTrackData.UpdateBookTracksForLoad(lbRes.LTBookControl, btcomm, BookTrackLSCEnum.LBPost, lbRes.LTTypeName, DateTime.Now, "", "")
                        NGLLoadTenderData.updateLoadTender(lbRes.LTControl, lbRes.UserName, lbRes.LTMessage)
                    Else
                        'failed ls msg ?
                        btcomm = "Update Failed for " + lbRes.LTTypeName + " Post with Reference ID " + lbRes.LTDATRefID
                        NGLBookTrackData.UpdateBookTracksForLoad(lbRes.LTBookControl, btcomm, BookTrackLSCEnum.LBError, lbRes.LTTypeName, DateTime.Now, "", "")
                        NGLLoadTenderData.updateLoadTender(lbRes.LTControl, lbRes.UserName, lbRes.LTMessage)
                    End If
                Else
                    If blnPostingSuccess AndAlso blnBookRevSaveComplete Then
                        btcomm = "Load Posted to " + lbRes.LTTypeName + ". Reference ID " + lbRes.LTDATRefID
                        NGLLoadTenderData.updatePostResults(lbRes.LTControl, lbRes.LTDATRefID, lbRes.UserName, lbRes.LTStatusCode)
                        NGLBookTrackData.UpdateBookTracksForLoad(lbRes.LTBookControl, btcomm, BookTrackLSCEnum.LBPost, lbRes.LTTypeName, DateTime.Now, "", "")
                    End If
                    If Not blnPostingSuccess Then
                        NGLLoadTenderData.updateLoadTender(lbRes.LTControl, UserName:=lbRes.UserName, Message:=lbRes.LTMessage, StatusCode:=lbRes.LTStatusCode)
                    End If
                    If blnPostingSuccess AndAlso Not blnBookRevSaveComplete Then
                        Dim p() As String = {lbRes.LTDATRefID, lbRes.LTTypeName}
                        btcomm = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_DATDPostFail), p)
                        NGLBookTrackData.UpdateBookTracksForLoad(lbRes.LTBookControl, btcomm, BookTrackLSCEnum.LBError, lbRes.LTTypeName, DateTime.Now, "", "")
                    End If
                End If

            Case NGLBookBLL.AcceptRejectEnum.Expired
                If blnBookRevSaveComplete Then
                    'It doesn't really matter if the Delete worked or not, but if the Save worked then we act as though the Delete worked regardless
                    btcomm = lbRes.LTTypeName + ": Post Expired Ref ID: " + lbRes.LTDATRefID + ". " + lbRes.BookTrackComment
                    NGLBookTrackData.UpdateBookTracksForLoad(lbRes.LTBookControl, btcomm, BookTrackLSCEnum.LBExpired, lbRes.LTTypeName, DateTime.Now, "", "")
                    NGLLoadTenderData.updateLoadTender(lbRes.LTControl, Parameters.UserName, lbRes.LTMessage, StatusCode:=LTSCEnum.DATExpired, Archived:=True, Expired:=True)
                End If
                If blnDeleteSuccess AndAlso Not blnBookRevSaveComplete Then
                    'We still expire the load in tblLoadTender
                    NGLLoadTenderData.updateLoadTender(lbRes.LTControl, Parameters.UserName, lbRes.LTMessage, StatusCode:=LTSCEnum.DATExpired, Archived:=True, Expired:=True)
                    'Create a message saying the load is still tendered to DATCarrier even though post expired
                    Dim p() As String = {lbRes.LTBookSHID, lbRes.LTCarrierNumber.ToString(), lbRes.LTCarrierName, lbRes.LTTypeName}
                    btcomm = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_DATDeleteStillTenderedToCarrier), p)
                    'This is a background process so add the msg to the tblLog, update the BookTrack, and send an Alert
                    NGLSystemLogData.AddApplicaitonLog(btcomm, source)
                    NGLBookTrackData.UpdateBookTracksForLoad(lbRes.LTBookControl, btcomm, BookTrackLSCEnum.LBError, lbRes.LTTypeName, DateTime.Now, "", "")
                    NGLDATBLL.createDATErrorSubscriptionAlert(lbRes, "Alert: System failed to properly process expired " + lbRes.LTTypeName + " load", btcomm, source)
                End If
                If Not blnDeleteSuccess AndAlso Not blnBookRevSaveComplete Then
                    'If the Save failed and the DAT delete also failed nothing needs to happen because both things are still true
                    'The load is still tendered to DAT Carrier and the Posting for that load is still on the DAT Load Board
                    'Still create an alert that says this load is expired but there was a problem deleting and resetting to N Status
                    Dim p() As String = {lbRes.LTBookSHID, lbRes.LTDATRefID, lbRes.LTTypeName}
                    btcomm = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_DATExpireFail), p)
                    NGLSystemLogData.AddApplicaitonLog(btcomm, source)
                    NGLDATBLL.createDATErrorSubscriptionAlert(lbRes, "Alert: System failed to properly process expired " + lbRes.LTTypeName + " load", btcomm, source)
                End If
            Case NGLBookBLL.AcceptRejectEnum.Rejected, NGLBookBLL.AcceptRejectEnum.Dropped
                'It doesn't really matter if the Delete worked or not, but if the Save worked then we act as though the Delete worked regardless
                If blnBookRevSaveComplete Then
                    'update BookTrack and update the fields in the tblLoadTender
                    btcomm = "Post with Reference ID " + lbRes.LTDATRefID + " Deleted from " + lbRes.LTTypeName + "."
                    NGLLoadTenderData.updateLoadTender(lbRes.LTControl, lbRes.UserName, lbRes.LTMessage, lbRes.LTStatusCode, Archived:=True)
                    NGLBookTrackData.UpdateBookTracksForLoad(lbRes.LTBookControl, btcomm, BookTrackLSCEnum.LBDelete, lbRes.LTTypeName, DateTime.Now, "", "")
                End If
                'If the Save failed but the Delete worked, we have to alert the user that the load is still tendered to DAT even though the Post has been deleted
                If Not blnBookRevSaveComplete AndAlso blnDeleteSuccess Then
                    btcomm = "Post with Reference ID " + lbRes.LTDATRefID + " Deleted from " + lbRes.LTTypeName + "."
                    NGLLoadTenderData.updateLoadTender(lbRes.LTControl, lbRes.UserName, lbRes.LTMessage, lbRes.LTStatusCode, Archived:=True)
                    NGLBookTrackData.UpdateBookTracksForLoad(lbRes.LTBookControl, btcomm, BookTrackLSCEnum.LBDelete, lbRes.LTTypeName, DateTime.Now, "", "")
                    Dim p() As String = {lbRes.LTBookSHID, lbRes.LTDATRefID, lbRes.LTTypeName}
                    Dim msg = String.Format(DTO.DATResults.getMessageNotLocalizedString(DTO.DATResults.MessageEnum.E_DATDeleteFail), p)
                    NGLSystemLogData.AddApplicaitonLog(msg, source)
                    NGLDATBLL.createDATErrorSubscriptionAlert(lbRes, "Alert: System failed to properly process deleted " + lbRes.LTTypeName + " posting", msg, source)
                End If

        End Select

    End Sub

    'Added by LVV on 1/13/17 for v-8.0 Next Stop
    Public Function LoadBoardPostMethod(ByVal pbookRevs As DTO.BookRevenue(),
                                        ByVal srBookControl As Integer,
                                        ByVal selectedLoadBoardsInt As Integer) As DTO.WCFResults

        Dim selectedLoadBoards As New BitwiseFlags32(selectedLoadBoardsInt)

        Return LoadBoardPostMethod(pbookRevs, srBookControl, selectedLoadBoards)

    End Function

    'Added by LVV on 1/13/17 for v-8.0 Next Stop
    Public Function LoadBoardPostMethod(ByVal pbookRevs As DTO.BookRevenue(),
                                        ByVal srBookControl As Integer,
                                        ByVal selectedLoadBoards As BitwiseFlags32) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim results As New DTO.WCFResults
        results.Success = True
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim LoadStatusCodeActions As New BitwiseFlags32()

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

        Dim source = "NGL.FM.BLL.NGLDATBLL.LoadBoardPostMethod"

        If pbookRevs Is Nothing OrElse pbookRevs.Count < 1 Then Return results

        'Get the LoadStatusCode and Type for this booking
        'This is removed temporarily until we can understand the impact on optimistic concurrency
        'Don't get this from the database use the bookrevs object
        'Dim sc = NGLBookRevenueData.GetLTStatusCode(pbookRevs(0).BookControl)
        'Dim LTTypeFlag = NGLBookRevenueData.GetBookLTTypeControl(pbookRevs(0).BookControl)
        Dim sc = pbookRevs(0).BookRevLoadTenderStatusCode
        Dim LTTypeFlag = pbookRevs(0).BookRevLoadTenderTypeControl

        Dim bwBRLTSC As New BitwiseFlags32(sc)
        Dim bwBRLTType As New BitwiseFlags32(LTTypeFlag)
        Dim blnHasDATPosting As Boolean = False
        Dim blnHasNSPosting As Boolean = False

        'Check for active postings
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            blnHasDATPosting = True
        End If
        If HasActiveLBPosting(LTTypeEnum.NextStop, bwBRLTType, bwBRLTSC) Then
            blnHasNSPosting = True
        End If

        'The user has selected the checkbox to Post to DAT
        If selectedLoadBoards.isBitFlagOn(LTTypeEnum.DAT) Then
            If blnHasDATPosting Then
                LoadStatusCodeActions.turnBitFlagOn(LTSCEnum.DATUpdated)
            Else
                LoadStatusCodeActions.turnBitFlagOn(LTSCEnum.DATPosted)
            End If
        End If

        'The user has selected the checkbox to Post to NEXTStop
        If selectedLoadBoards.isBitFlagOn(LTTypeEnum.NextStop) Then
            If blnHasNSPosting Then
                LoadStatusCodeActions.turnBitFlagOn(LTSCEnum.NStopUpdated)
            Else
                LoadStatusCodeActions.turnBitFlagOn(LTSCEnum.NStopPosted)
            End If
        End If


        If blnHasDATPosting OrElse blnHasNSPosting Then
            'If a posting for one already exists then we don't have to go through
            'the whole AcceptReject logic -- all we have to do is post or update

            'NOTE: This might have a performace impact and we can evaluate its necessity later
            'Save the BookRevs from the screen in case they made changes
            oBookRevs = NGLBookRevenueData.SaveRevenuesWDetails(pbookRevs, True)
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                Dim p As String() = {"Load Board", "Post/Update", "NGLDATBLL.LoadBoardPostMethod (SaveRevenuesWDetails)"}
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_LBBookRevsNull, p)
                results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                results.Success = False
                Return results
            End If

            Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = srBookControl).FirstOrDefault()

            If selectedLoadBoards.isBitFlagOn(LTTypeEnum.DAT) Then
                If LoadStatusCodeActions.isBitFlagOn(LTSCEnum.DATPosted) Then
                    datRes = NGLDATBLL.DATPost(results, oBookRevs, blnDATGetDataSuccess, blnDATPostingSuccess, oSelectedBooking)
                End If
                If LoadStatusCodeActions.isBitFlagOn(LTSCEnum.DATUpdated) Then
                    'datRes = NGLDATBLL.DATUpdatePost(results, oBookRevs, blnDATIsUpdate, blnDATPostingSuccess, oSelectedBooking)
                End If
            End If
            If selectedLoadBoards.isBitFlagOn(LTTypeEnum.NextStop) Then
                If LoadStatusCodeActions.isBitFlagOn(LTSCEnum.NStopPosted) Then
                    nsRes = NGLDATBLL.NSPost(results, oBookRevs, blnNSGetDataSuccess, blnNSPostingSuccess, oSelectedBooking)
                End If
                If LoadStatusCodeActions.isBitFlagOn(LTSCEnum.NStopUpdated) Then
                    'nsRes = NGLDATBLL.NSUpdatePost(results, oBookRevs, blnNSIsUpdate, blnNSPostingSuccess, oSelectedBooking)
                End If
            End If

            'Should we save the entire thing here?
            'We have to at least save the 2 BookRevLT fields
            results.AddLog("Save changes to booking records")
            NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)

            If selectedLoadBoards.isBitFlagOn(LTTypeEnum.DAT) Then
                NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, NGLBookBLL.AcceptRejectEnum.Tendered, blnDATDeleteSuccess, blnDATPostingSuccess, True, source, blnDATIsUpdate)
            End If
            If selectedLoadBoards.isBitFlagOn(LTTypeEnum.NextStop) Then
                NGLDATBLL.ProcessLoadBoardStatusUpdates(nsRes, NGLBookBLL.AcceptRejectEnum.Tendered, blnNSDeleteSuccess, blnNSPostingSuccess, True, source, blnNSIsUpdate)
            End If

        Else
            'If this is a fresh posting we have to go through all the AcceptReject logic

            'reset the param collection to N Status, Save, and get the results back
            'NOTE: This might have a performace impact and we can evaluate its necessity later
            oBookRevs = NGLBookRevenueData.SaveRevenuesWDetails(pbookRevs, True)
            If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
                Dim p As String() = {"Source: NGLDATBLL.LoadBoardPostMethod (SaveRevenuesWDetails)"}
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_DATBookRevsNull, p)
                results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                results.Success = False
                Return results
            End If

            'Get the LoadBoard Carrier data and the next CNS if one does not already exist
            Dim carrierTender = NGLCarrierData.GetDATCarrierTenderData(oBookRevs(0).BookCustCompControl)
            If carrierTender Is Nothing Then
                Dim p As String() = {"Source: NGLDATBLL.LoadBoardPostMethod (GetDATCarrierTenderData)"}
                results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_DATCarrierNull, p)
                results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
                results.Success = False
                Return results
            End If

            'Note: We don't update the value of BookRevLoadTenderTypeControl until later in AcceptReject
            'This is because the value we update it (selectedLoadBoards) with can potentially change between here and later
            'in the ProcessNewTransCode method (ex if the user checked Post to DAT but doesn't have an account)
            For Each b In oBookRevs
                With b
                    .BookCarrierControl = carrierTender.CarrierControl
                    .BookCarrierContControl = carrierTender.CarrierContControl
                    .BookCarrierContact = carrierTender.CarrierContName
                    .BookCarrierContactPhone = carrierTender.CarrierContactPhone
                    If String.IsNullOrEmpty(.BookConsPrefix) Then
                        .BookConsPrefix = carrierTender.BookConsPrefix
                    End If
                    .BookRevNegRevenue = 9
                    .TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                End With
            Next

            results = oBookBLL.ProcessNewTransCode(oBookRevs, srBookControl, "PC", "N", results, selectedLoadBoards, LoadStatusCodeActions, 0, -1)

        End If

        Return results
    End Function

    Public Function LoadBoardDeleteMethod(ByVal BookControl As Integer,
                                        ByVal selectedLoadBoardsInt As Integer) As DTO.WCFResults

        Dim selectedLoadBoards As New BitwiseFlags32(selectedLoadBoardsInt)

        Return LoadBoardDeleteMethod(BookControl, selectedLoadBoards)

    End Function

    Public Function LoadBoardDeleteMethod(ByVal BookControl As Integer,
                                          ByVal selectedLoadBoards As BitwiseFlags32,
                                          Optional ByVal blnIsCarDelete As Boolean = False) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim results As New DTO.WCFResults
        results.Success = True
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim LoadStatusCodeActions As New BitwiseFlags32()

        Dim blnDATDeleteSuccess As Boolean = True
        Dim datRes As New DTO.DATResults
        Dim blnNSDeleteSuccess As Boolean = True
        Dim nsRes As New DTO.DATResults
        Dim source = "NGL.FM.BLL.NGLDATBLL.LoadBoardDeleteMethod"

        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)

        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            Dim p As String() = {"Load Board", "Delete", "NGLDATBLL.LoadBoardDeleteMethod (GetBookRevenuesWDetailsFiltered)"}
            results.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_LBBookRevsNull, p)
            results.setAction(DTO.WCFResults.ActionEnum.ShowWarnings)
            results.Success = False
            Return results
        End If

        Dim sc = oBookRevs(0).BookRevLoadTenderStatusCode
        Dim LTTypeFlag = oBookRevs(0).BookRevLoadTenderTypeControl

        Dim bwBRLTSC As New BitwiseFlags32(sc)
        Dim bwBRLTType As New BitwiseFlags32(LTTypeFlag)
        Dim blnHasDATPosting As Boolean = False
        Dim blnHasNSPosting As Boolean = False

        'Check for active Posting
        'Active --> Post or Update Flags are True AndAlso Delete, Expired, Error, Accepted are all False
        If bwBRLTType.isBitFlagOn(LTTypeEnum.DAT) _
                    AndAlso (bwBRLTSC.isBitFlagOn(LTSCEnum.DATPosted) OrElse bwBRLTSC.isBitFlagOn(LTSCEnum.DATUpdated)) _
                    AndAlso (Not bwBRLTSC.isBitFlagOn(LTSCEnum.DATDeleted) AndAlso Not bwBRLTSC.isBitFlagOn(LTSCEnum.DATExpired)) _
                    AndAlso (Not bwBRLTSC.isBitFlagOn(LTSCEnum.DATAccepted) AndAlso Not bwBRLTSC.isBitFlagOn(LTSCEnum.DATError)) Then
            blnHasDATPosting = True
        End If

        If bwBRLTType.isBitFlagOn(LTTypeEnum.NextStop) _
                    AndAlso (bwBRLTSC.isBitFlagOn(LTSCEnum.NStopPosted) OrElse bwBRLTSC.isBitFlagOn(LTSCEnum.NStopUpdated)) _
                    AndAlso (Not bwBRLTSC.isBitFlagOn(LTSCEnum.NStopDeleted) AndAlso Not bwBRLTSC.isBitFlagOn(LTSCEnum.NStopExpired)) _
                    AndAlso (Not bwBRLTSC.isBitFlagOn(LTSCEnum.NStopAccepted) AndAlso Not bwBRLTSC.isBitFlagOn(LTSCEnum.NStopError)) Then
            blnHasNSPosting = True
        End If


        'The user wants to Delete all LB Posts so run through ProcessNewTransCode to reset to N
        If selectedLoadBoards.isBitFlagOn(LTTypeEnum.DAT) AndAlso selectedLoadBoards.isBitFlagOn(LTTypeEnum.NextStop) Then
            results = oBookBLL.ProcessNewTransCode(oBookRevs, BookControl, "N", "PC", results, selectedLoadBoards, LoadStatusCodeActions, 0)
        End If
        'The user wants to Delete only the DAT Post
        If selectedLoadBoards.isBitFlagOn(LTTypeEnum.DAT) AndAlso Not selectedLoadBoards.isBitFlagOn(LTTypeEnum.NextStop) AndAlso blnHasDATPosting Then
            If blnHasNSPosting Then
                'There is currently another LB Post as well so delete the DAT Post but do not reset to N
                datRes = NGLDATBLL.DeleteDATPosting(results, oBookRevs, blnDATDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected)
                'We have to at least save the 2 BookRevLT fields
                results.AddLog("Save changes to booking records")
                NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
                NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnDATDeleteSuccess, True, True, source, False)
            Else
                'DAT is the only current active posting so reset to N
                results = oBookBLL.ProcessNewTransCode(oBookRevs, BookControl, "N", "PC", results, selectedLoadBoards, LoadStatusCodeActions, 0)
            End If
        End If
        'The user wants to Delete only the NEXTStop Post
        If selectedLoadBoards.isBitFlagOn(LTTypeEnum.NextStop) AndAlso Not selectedLoadBoards.isBitFlagOn(LTTypeEnum.DAT) AndAlso blnHasNSPosting Then
            If blnHasDATPosting Then
                'There is currently another LB Post as well so delete the NEXTStop Post but do not reset to N
                nsRes = NGLDATBLL.NSDeletePost(results, oBookRevs, blnNSDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected)
                'We have to at least save the 2 BookRevLT fields
                results.AddLog("Save changes to booking records")
                NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
                NGLDATBLL.ProcessLoadBoardStatusUpdates(nsRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnNSDeleteSuccess, True, True, source, False)
            Else
                'NEXTStop is the only current active posting so reset to N
                results = oBookBLL.ProcessNewTransCode(oBookRevs, BookControl, "N", "PC", results, selectedLoadBoards, LoadStatusCodeActions, 0)
            End If
        End If

        Return results
    End Function


#End Region

#Region "Shared Helper Methods"

    Public Function HasActiveLBPosting(ByVal loadBoard As LTTypeEnum, ByVal bwTenderType As BitwiseFlags32, ByVal bwStatusCode As BitwiseFlags32) As Boolean
        Dim blnReturn As Boolean = False

        Select Case loadBoard
            Case LTTypeEnum.DAT
                If bwTenderType.isBitFlagOn(LTTypeEnum.DAT) _
                    AndAlso (bwStatusCode.isBitFlagOn(LTSCEnum.DATPosted) OrElse bwStatusCode.isBitFlagOn(LTSCEnum.DATUpdated)) _
                    AndAlso (Not bwStatusCode.isBitFlagOn(LTSCEnum.DATDeleted) AndAlso Not bwStatusCode.isBitFlagOn(LTSCEnum.DATExpired)) _
                    AndAlso (Not bwStatusCode.isBitFlagOn(LTSCEnum.DATAccepted) AndAlso Not bwStatusCode.isBitFlagOn(LTSCEnum.DATError)) Then
                    blnReturn = True
                End If
            Case LTTypeEnum.NextStop
                If bwTenderType.isBitFlagOn(LTTypeEnum.NextStop) _
                    AndAlso (bwStatusCode.isBitFlagOn(LTSCEnum.NStopPosted) OrElse bwStatusCode.isBitFlagOn(LTSCEnum.NStopUpdated)) _
                    AndAlso (Not bwStatusCode.isBitFlagOn(LTSCEnum.NStopDeleted) AndAlso Not bwStatusCode.isBitFlagOn(LTSCEnum.NStopExpired)) _
                    AndAlso (Not bwStatusCode.isBitFlagOn(LTSCEnum.NStopAccepted) AndAlso Not bwStatusCode.isBitFlagOn(LTSCEnum.NStopError)) Then
                    blnReturn = True
                End If
        End Select

        Return blnReturn
    End Function

    ''' <summary>
    ''' Returns the correct AcceptRejectModeEnum based on the LoadTenderTypeMode bitwise flag
    ''' Note: I did this function for backwards compatibility for now because I do not want to figure
    ''' out how to change all this code at the moment
    ''' </summary>
    ''' <param name="LoadTenderTypeMode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function getAcceptRejectModeEnum(ByVal LoadTenderTypeMode As BitwiseFlags32) As NGLBookBLL.AcceptRejectModeEnum
        'The default is MANUAL
        Dim returnVal As NGLBookBLL.AcceptRejectModeEnum = NGLBookBLL.AcceptRejectModeEnum.MANUAL

        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.EDI204) Then
            returnVal = NGLBookBLL.AcceptRejectModeEnum.EDI
            Return returnVal
        End If
        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.Web) Then
            returnVal = NGLBookBLL.AcceptRejectModeEnum.WEB
            Return returnVal
        End If
        'We don't return here because multiple load boards can be selected
        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then
            returnVal = NGLBookBLL.AcceptRejectModeEnum.DAT
        End If
        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then
            returnVal = NGLBookBLL.AcceptRejectModeEnum.DAT
        End If

        Return returnVal

    End Function


    ''' <summary>
    ''' Returns the correct BookRevLoadTenderTypeControl based on the LoadTenderTypeMode bitwise flag
    ''' LoadTenderTypeMode - has the values of the actions we want to perform
    ''' BookRevLoadTenderTypeControl - has the history/state of the actions for this BookRev object -- we want to update this
    ''' to reflect what we are about to do
    ''' </summary>
    ''' <param name="BookRevLoadTenderTypeControl"></param>
    ''' <param name="LoadTenderTypeMode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' </remarks>
    'Public Function UpdateBookRevLoadTenderTypeControl(ByVal BookRevLoadTenderTypeControl As Integer, ByVal LoadTenderTypeMode As BitwiseFlags32) As Integer
    '    Dim bw As New BitwiseFlags32(BookRevLoadTenderTypeControl)
    '    'I do this here for the LoadBoards because this is after we check if the user has valid SSOA accounts so this could have changed
    '    'since the original cal on the client
    '    Dim blnBRUpdated As Boolean = False

    '    If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.Manual) Then
    '        bw.turnBitFlagOn(LTTypeEnum.Manual)
    '        blnBRUpdated = True
    '        Return bw.FlagSource
    '    End If
    '    If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.EDI204) Then
    '        bw.turnBitFlagOn(LTTypeEnum.EDI204)
    '        blnBRUpdated = True
    '        Return bw.FlagSource
    '    End If
    '    If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.Web) Then
    '        bw.turnBitFlagOn(LTTypeEnum.Web)
    '        blnBRUpdated = True
    '        Return bw.FlagSource
    '    End If
    '    'We don't return here because multiple load boards can be selected
    '    If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) Then
    '        bw.turnBitFlagOn(LTTypeEnum.DAT)
    '        blnBRUpdated = True
    '    End If
    '    If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then
    '        bw.turnBitFlagOn(LTTypeEnum.NextStop)
    '        blnBRUpdated = True
    '    End If

    '    If Not blnBRUpdated Then
    '        'If none of the If statements above were executed set the default to Manual
    '        bw.turnBitFlagOn(LTTypeEnum.Manual)
    '    End If

    '    Return bw.FlagSource

    'End Function

    ''' <summary>
    ''' When LB Post is successful we set Posted bit to True and
    ''' reset Deleted, Expired, Error, Accepted, and Updated bits to False
    ''' </summary>
    ''' <param name="BookRevLoadTenderStatusCode"></param>
    ''' <param name="LoadTenderType"></param>
    ''' <returns></returns>
    Public Function UpdateBookRevLTSCPost(ByVal BookRevLoadTenderStatusCode As Integer, ByVal LoadTenderType As LTTypeEnum) As Integer
        Dim bw As New BitwiseFlags32(BookRevLoadTenderStatusCode)
        'DAT
        If LoadTenderType = LTTypeEnum.DAT Then
            'Turn on Post
            bw.turnBitFlagOn(LTSCEnum.DATPosted)
            'Make sure Deleted, Expired, and Error are off
            bw.turnBitFlagOff(LTSCEnum.DATDeleted)
            bw.turnBitFlagOff(LTSCEnum.DATExpired)
            bw.turnBitFlagOff(LTSCEnum.DATError)
            bw.turnBitFlagOff(LTSCEnum.DATAccepted)
            bw.turnBitFlagOff(LTSCEnum.DATUpdated)
        End If
        'NEXTStop
        If LoadTenderType = LTTypeEnum.NextStop Then
            'Turn on Post
            bw.turnBitFlagOn(LTSCEnum.NStopPosted)
            'Make sure Deleted, Expired, and Error are off
            bw.turnBitFlagOff(LTSCEnum.NStopDeleted)
            bw.turnBitFlagOff(LTSCEnum.NStopExpired)
            bw.turnBitFlagOff(LTSCEnum.NStopError)
            bw.turnBitFlagOff(LTSCEnum.NStopAccepted)
            bw.turnBitFlagOff(LTSCEnum.NStopUpdated)
        End If

        Return bw.FlagSource
    End Function

    ''' <summary>
    ''' Sets the BookRevLoadTenderTypeControl to the provided Load Tender Type
    ''' If blnSuccess is true it turns on the Flag for the LTType
    ''' If blnSuccess is false it turns the Flag off for the LTType
    ''' </summary>
    ''' <param name="BookRevLoadTenderTypeControl"></param>
    ''' <param name="LoadTenderType"></param>
    ''' <param name="blnSuccess"></param>
    ''' <returns></returns>
    Public Function UpdateBookRevLTTC(ByVal BookRevLoadTenderTypeControl As Integer, ByVal LoadTenderType As LTTypeEnum, ByVal blnSuccess As Boolean) As Integer
        Dim bw As New BitwiseFlags32(BookRevLoadTenderTypeControl)

        If blnSuccess Then
            bw.turnBitFlagOn(LoadTenderType)
        Else
            bw.turnBitFlagOff(LoadTenderType)
        End If

        Return bw.FlagSource
    End Function


    ''' <summary>
    ''' Returns the correct BookRevLoadTenderStatusCode based on the LoadTenderSC enum
    ''' </summary>
    ''' <param name="BookRevLoadTenderStatusCode"></param>
    ''' <param name="LoadTenderSC"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/19/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function UpdateBookRevLoadTenderStatusCode(ByVal BookRevLoadTenderStatusCode As Integer, ByVal LoadTenderSC As LTSCEnum) As Integer
        Dim bw As New BitwiseFlags32(BookRevLoadTenderStatusCode)
        bw.turnBitFlagOn(LoadTenderSC)
        Return bw.FlagSource
    End Function

    ''' <summary>
    ''' Creates a AlertProcessDATError subscription alert using the 
    ''' information provided. If the DATResults item has any Messages,
    ''' Warnings, or Errors they will be added in the Notes
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="Subject"></param>
    ''' <param name="strBody"></param>
    ''' <param name="source">Optional</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV 7/5/16 for v-7.0.5.110 DAT
    ''' </remarks>
    Friend Function createDATErrorSubscriptionAlert(ByVal d As DTO.DATResults,
                                      ByVal Subject As String,
                                      ByVal strBody As String,
                                      Optional ByVal source As String = "") As Boolean

        Dim strErrs = ""
        Dim strWarnings = ""
        Dim strMsgs = ""

        If Not d.Errors Is Nothing AndAlso d.Errors.Count > 0 Then
            strErrs = d.concatErrors()
        End If
        If Not d.Warnings Is Nothing AndAlso d.Warnings.Count > 0 Then
            strWarnings = d.concatWarnings()
        End If
        If Not d.Messages Is Nothing AndAlso d.Messages.Count > 0 Then
            strMsgs = d.concatMessage()
        End If

        Dim Note1 As String = String.Format(" Book SHID: {0} ", d.LTBookSHID)
        Dim Note2 As String = " Errors: " & strErrs
        Dim Note3 As String = " Warnings: " & strWarnings
        Dim Note4 As String = " Messages: " & strMsgs
        Dim Note5 As String = ""
        If Not String.IsNullOrWhiteSpace(source) Then
            Note5 = "Source: " + source
        End If

        Dim Body As String = String.Concat(strBody, vbCrLf, Note1, vbCrLf, vbCrLf, Note2, vbCrLf, vbCrLf, Note3, vbCrLf, vbCrLf, Note4, vbCrLf, vbCrLf, Note5)
        'NOTE:  the alert description is limited to 50 characters and will be truncated to fit; also the descriptiong will be displayed in the alert subscription selection screen.
        Return NGLtblAlertMessageData.InsertAlertMessage("AlertProcessDATError", "Alert Load Board Error", Subject, Body, d.LTBookCustCompControl, 0, d.LTCarrierControl, d.LTCarrierNumber, Note1, Note2, Note3, Note4, Note5)

    End Function


#End Region


#Region "Called From AcceptReject Method"

    ''' <summary>
    ''' Determines if we should save the BookRevenues in the Tendered case of Accept Reject
    ''' Rules:
    '''   If we are only posting one and one is fail then we don't save
    '''   If we are posting 2 and at least one passes we save
    '''   If this isn't a Load Board Posting action we save
    ''' (See in line code comments for more detailed explanation)
    ''' </summary>
    ''' <param name="blnDATPostingSuccess"></param>
    ''' <param name="blnNSPostingSuccess"></param>
    ''' <param name="LoadTenderTypeMode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/31/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function GetTenderWillSaveBookRev(ByVal blnDATPostingSuccess As Boolean, ByVal blnNSPostingSuccess As Boolean, ByVal LoadTenderTypeMode As BitwiseFlags32) As Boolean
        'NOTE:
        'The default value for now will be true because if it isn't a Load Board Posting then always save
        'This seems to be how it used to work in 705110 because in the previous code it would save if blnDATPostingSuccess was 
        'true and since it is default set to true the only time it wouldn't save is if it attempted to post to DAT and failed
        Dim blnSaveBookRev As Boolean = True

        'If we are posting to only one Load Board
        If (LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) AndAlso Not LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop)) OrElse
            (Not LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) AndAlso LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop)) Then
            'Since the default of blnXXXPostingSucess is true -
            'If one is fail then we don't save
            'because we are only trying to post to one Load Board and it failed
            If blnDATPostingSuccess AndAlso blnNSPostingSuccess Then
                blnSaveBookRev = True
            Else
                blnSaveBookRev = False
            End If
        End If

        'If we are posting to both Load Boards
        If LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.DAT) AndAlso LoadTenderTypeMode.isBitFlagOn(LTTypeEnum.NextStop) Then
            'The only reason we would not save here is if both posts failed
            If Not blnDATPostingSuccess AndAlso Not blnNSPostingSuccess Then
                blnSaveBookRev = False
            Else
                blnSaveBookRev = True
            End If
        End If

        Return blnSaveBookRev
    End Function

    ''' <summary>
    ''' Used to convert AcceptRejectModeEnum to Bitwise flags
    ''' Note: I didn't put in translations for AcceptRejectModeEnum.System or
    ''' AcceptRejectModeEnum.Token because I do not know where those get
    ''' used or handled. Also, there are no options for those in 
    ''' tblLoadTenderType. Are we supposed to add those??
    ''' TODO Ask Rob what to do about this
    ''' </summary>
    ''' <param name="ARModeEnum"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 1/13/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function ConvertARModeEnumToBitwiseFlag(ByVal ARModeEnum As NGLBookBLL.AcceptRejectModeEnum) As BitwiseFlags32
        Dim bwRet As New BitwiseFlags32
        Select Case ARModeEnum
            Case NGLBookBLL.AcceptRejectModeEnum.MANUAL
                bwRet.turnBitFlagOn(LTTypeEnum.Manual)
            Case NGLBookBLL.AcceptRejectModeEnum.EDI
                bwRet.turnBitFlagOn(LTTypeEnum.EDI204)
            Case NGLBookBLL.AcceptRejectModeEnum.WEB
                bwRet.turnBitFlagOn(LTTypeEnum.Web)
            Case NGLBookBLL.AcceptRejectModeEnum.DAT
                bwRet.turnBitFlagOn(LTTypeEnum.DAT)
            Case Else
                'I guess just default to Manual ??
                bwRet.turnBitFlagOn(LTTypeEnum.Manual)
        End Select
        Return bwRet
    End Function

#End Region

#Region "NGL Tariff Methods"

    Public Function CreateNGLTariffBidAsync(ByVal BookControl As Integer, ByVal LoadTenderControl As Integer, ByVal SHID As String, ByVal strMsg As String, ByVal tariffOptions As DTO.GetCarriersByCostParameters) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults With {.Key = 0, .Success = True}
        Using Logger.StartActivity("CreateNGLTariffBidAsync(BookControl: {BookControl}, LoadTenderControl: {LoadTenderControl}, SHID: {SHID}, TariffOptions: {TariffOptions}", BookControl, LoadTenderControl, SHID, tariffOptions)
            If oRet.Log Is Nothing Then oRet.Log = New List(Of DTO.NGLMessage)

            Dim msg As New DTO.NGLMessage("Background Process Running On Server.  No further notificaiton is expected")
            oRet.Log.Add(msg)


            ExecCreateNGLTariffBid(BookControl, LoadTenderControl, SHID, strMsg, tariffOptions)
        End Using


        Return oRet

    End Function

    Private Sub ExecCreateNGLTariffBid(ByVal BookControl As Integer, ByVal LoadTenderControl As Integer, ByVal SHID As String, ByVal strMsg As String, ByVal tariffOptions As DTO.GetCarriersByCostParameters)
        Try
            Dim success = CreateNGLTariffBid(BookControl, LoadTenderControl, SHID, strMsg, tariffOptions)

            Logger.Information("CreateNGLTariffBidAsync - BookControl: {BookControl}, LoadTenderControl: {LoadTenderControl}, SHID: {SHID}, Success: {success}", BookControl, LoadTenderControl, SHID, success)

            If String.IsNullOrWhiteSpace(strMsg) Then
                Logger.Information("CreateNGLTariffBidAsync - No Message Returned, updating oLT.updateLoadTender")
                Dim oLT As New DAL.NGLLoadTenderData(Parameters)
                oLT.updateLoadTender(LoadTenderControl, Message:=strMsg)
                If Not success Then
                Logger.Warning("Something went wrong updating load tender ")
                    SaveAppError("CreateNGLTariffBidAsync Error: " & strMsg)
            End If
            End If
        Catch ex As Exception
            Logger.Error(ex, "CreateNGLTariffBidAsync Error: {0}", ex.Message)
            'ignore all errors for async processing at this time
            'we could log as system alert message
            'SaveAppError("CreateNGLTariffBidAsync Error: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="SHID"></param>
    ''' <param name="strMsg"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.001 on 05/25/2022 we now use InsertNGLTariffBid365
    '''     add new TariffOptions so users can configure how tariff rates are calculated
    ''' </remarks>
    Public Function CreateNGLTariffBid(ByVal BookControl As Integer,
                                       ByVal LoadTenderControl As Integer,
                                       ByVal SHID As String,
                                       ByRef strMsg As String,
                                       ByVal tariffOptions As DTO.GetCarriersByCostParameters) As Boolean
        Dim blnRet As Boolean = False

        Using Logger.StartActivity("CreateNGLTariffBid - BookControl: {BookControl}, LoadTenderControl: {LoadTenderControl}, SHID: {SHID}", BookControl, LoadTenderControl, SHID)

            Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
            Dim oBid As New DAL.NGLBidData(Parameters)



            Dim res = oBookRevBLL.getEstimatedCarriersByCost(BookControl,
                                                             0,
                                                             tariffOptions.prefered,
                                                             tariffOptions.noLateDelivery,
                                                             tariffOptions.validated,
                                                             tariffOptions.optimizeByCapacity,
                                                             tariffOptions.modeTypeControl,
                                                             tariffOptions.tempType,
                                                             tariffOptions.tariffTypeControl,
                                                             tariffOptions.carrTarEquipMatClass,
                                                             tariffOptions.carrTarEquipMatClassTypeControl,
                                                             tariffOptions.carrTarEquipMatTarRateTypeControl,
                                                             tariffOptions.agentControl,
                                                             tariffOptions.Page,
                                                             tariffOptions.PageSize)

            Logger.Information("oBookRevBLL.getEstimatedCarriesByCost returned {0} results", res?.BookRevs.Count)

            If res Is Nothing Then Return True 'Nothing to do
            If res.CarriersByCost Is Nothing OrElse res.CarriersByCost.Count < 1 Then Return True 'Nothing to do

            blnRet = oBid.InsertNGLTariffBid365(res, res.BookRevs, LoadTenderControl, SHID, strMsg)


        End Using

        Return blnRet
    End Function


    ''' <summary>
    ''' Accepts a Tariff Bid
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BidControl"></param>
    ''' <param name="SelectedCarrier"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="oDispatch"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Added parameter CarrierContControl (gets passed in from dispatch model)
    '''   Added logic to pass CarrierContControl into the method GetDefaultContactForCarrier() as an optional param
    ''' NOTE:
    '''  The parameter CarrierContControl will eventually be optional with a Default value of 0. 
    '''  However, in initial development do not make it optional to ensure proper testing
    '''  When unit testing is complete change the parameter to optional
    '''  Modified by RHR for v-8.2 on 3/5/19
    '''    added logic to save some of the data entry information on dispatch like origin contact information.
    '''    updates only occurr when the optional oDispatch parameter is provided
    '''  Modified by RHR for v-8.2 on 6/30/2019 
    '''    added logic to support DAL.Models.Dispatch which contains
    '''    the new AutoAcceptOnDispatch flag
    '''    and the new EmailLoadTenderSheet flag
    ''' Modified by RHR for v-8.2 on 7/16/2019 
    '''    added logic to use carrier pro as shid if provided
    ''' Modified by RHR for v-8.5.2.003 on 08/14/2023 
    '''     added logic to include Rate It shipment id when needed
    '''     Added logic to get the next consolidation number when needed
    ''' </remarks>
    Public Function NSNGLTariffAccept(ByVal LTControl As Integer,
                                      ByVal BookControl As Integer,
                                      ByVal BidControl As Integer,
                                      ByVal SelectedCarrier As DTO.CarriersByCost,
                                      ByVal CarrierContControl As Integer,
                                      Optional ByVal oDispatch As DAL.Models.Dispatch = Nothing,
                                      Optional eLoadTenderType As LTTypeEnum = LTTypeEnum.LoadBoard) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim wcf As New DTO.WCFResults
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim oBrack As New DAL.NGLtblTarBracketTypeData(Parameters)

        Dim source = "NGL.FM.BLL.NGLDATBLL.NSNGLTariffAccept"
        'ProcessNewTransCode(BookControl, "N", "PC")

        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)

        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'Delete the DAT Posting
            Dim blnDATDeleteSuccess As Boolean = True
            Dim datRes = NGLDATBLL.DeleteDATPosting(wcf, oBookRevs, blnDATDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected)
            'We have to at least save the 2 BookRevLT fields
            'results.AddLog("Save changes to booking records")
            'NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
            NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnDATDeleteSuccess, True, True, source, False)
        End If

        'Modified by RHR for v-8.2 on 7/16/2019 
        Dim blnUseCarrierProAsSHID As Boolean = If(oDispatch Is Nothing, False, If(String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber), False, If(oDispatch.CarrierProNumber.Trim.Length > 0, True, False)))
        For Each b In oBookRevs
            b.BookRevLoadTenderStatusCode = UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.NStopAccepted)
            'Modified by RHR for v-8.5.2.003 on 08/14/2023 added logic to include Rate It shipment id when needed
            'Added logic to get the next consolidation number when needed
            If blnUseCarrierProAsSHID AndAlso Not String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber) Then
                b.BookSHID = oDispatch.CarrierProNumber
            ElseIf String.IsNullOrWhiteSpace(b.BookSHID) Then
                If Not (String.IsNullOrWhiteSpace(oDispatch.SHID)) Then
                    b.BookSHID = oDispatch.SHID
                End If
            End If
            If String.IsNullOrWhiteSpace(b.BookConsPrefix) Then
                If Not (String.IsNullOrWhiteSpace(oDispatch.SHID)) Then
                    b.BookConsPrefix = oDispatch.SHID
                Else
                    b.BookConsPrefix = NGLBookData.GetNextCNSNumber(b.BookCustCompControl)
                End If
            End If
            If String.IsNullOrWhiteSpace(b.BookSHID) Then
                b.BookSHID = b.BookConsPrefix
            End If
        Next

        NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)

        'get the carrier contact
        Dim CarrContact = NGLCarrierContData.GetDefaultContactForCarrier(SelectedCarrier.CarrierControl, CarrierContControl) 'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch -- added CarrierContControl parameter to method and passed it in as optional param to GetDefaultContactForCarrier()

        oBookRevBLL.updateSelectedCarrier(BookControl, SelectedCarrier, CarrContact, Nothing)

        'Modified by RHR for v-8.2 on 6/30/2019 added logic to support AutoAcceptOnDispatch flag
        Dim sNewTranCode As String = "PC"
        Dim iOverrideSendLoadTenderEmail = 0
        If Not oDispatch Is Nothing Then
            If oDispatch.AutoAcceptOnDispatch = True Then sNewTranCode = "PB"
            If oDispatch.EmailLoadTenderSheet = True Then
                iOverrideSendLoadTenderEmail = 1
            Else
                iOverrideSendLoadTenderEmail = -1
            End If
        End If

        Dim res = oBookBLL.ProcessNewTransCode(BookControl, sNewTranCode, "P", LTTypeEnum.LoadBoard, iOverrideSendLoadTenderEmail)

        If Not wcf.Warnings Is Nothing AndAlso wcf.Warnings.Count > 0 Then
            res.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcf.Warnings)
        End If
        If Not oDispatch Is Nothing Then
            'create or update booking, company and lane data associated with this shipment
            Dim strMsg As String = ""
            Dim iBookControl = NGLLoadTenderData.dispatchLoadTender(LTControl, oDispatch, strMsg, eLoadTenderType)
        End If


        'update bid records
        NGLLoadTenderData.AcceptNextStopBid(LTControl, BidControl)

        Return res
    End Function




    Public Sub CreateNGLTariffBidNoBookAsync(ByVal order As DAL.Models.RateRequestOrder, ByVal LoadTenderControl As Integer, ByVal tariffOptions As DTO.GetCarriersByCostParameters)
        'Dim fetcher As New CreateNGLTariffBidFromRateRequestOrderDelegate(AddressOf Me.ExecCreateNGLTariffBidNoBook)
        ' Launch thread
        'fetcher.BeginInvoke(order, LoadTenderControl, tariffOptions, Nothing, Nothing)

        'make sync passthrough
        Using Logger.StartActivity("CreateNGLTariffBidNoBookAsync - order: {@order}, LoadTenderControl: {LoadTenderControl}, tariffOptions, {tariffOptions}", order, LoadTenderControl, tariffOptions)
            ExecCreateNGLTariffBidNoBook(order, LoadTenderControl, tariffOptions)
        End Using


    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.002 on 12/17/2020
    '''     added logic to process messages async
    '''Modified by RHR for v-8.5.3.001 on 05/27/2022 added logic for tblLoadTenderLog records and tariff options
    ''' </remarks>
    Private Sub ExecCreateNGLTariffBidNoBook(ByVal order As DAL.Models.RateRequestOrder, ByVal LoadTenderControl As Integer, ByVal tariffOptions As DTO.GetCarriersByCostParameters)
        Dim oLTLogData As New DAL.NGLLoadTenderLogData(Me.Parameters)
        Try
            Logger.Information("CreateNGLTariffBidNoBookAsync - order: {@order}, LoadTenderControl: {LoadTenderControl}, tariffOptions, {@tariffOptions}", order, LoadTenderControl, tariffOptions)
            Dim s = "CreateNGLTariffBidNoBookAsync Warning "
            Dim strMsg As String = ""
            ' Modified by RHR for v-8.3.0.002 on 12/17/2020 new parameter set to true
            Dim success = CreateNGLTariffBidNoBook(order, LoadTenderControl, strMsg, True, tariffOptions)

            If success Then
                Dim oLT As New DAL.NGLLoadTenderData(Parameters)
                oLT.updateLoadTender(LoadTenderControl, Message:=strMsg)
                Logger.Information("CreateNGLTariffBidNoBookAsync - No Message Returned, updating oLT.updateLoadTender")
            End If

            If Not String.IsNullOrWhiteSpace(strMsg) Then
                'on failure we change s to Error instead of Warning
                If Not success Then s = "CreateNGLTariffBidNoBookAsync Error "
                Logger.Warning("CreateNGLTariffBidNoBookAsync - strMsg: {strMsg}", strMsg)
                Dim sAppError = String.Concat(s, " for LoadTenderControl = ", LoadTenderControl.ToString(), ": ", strMsg, ". No user message displayed.")
                SaveAppError(sAppError)
            End If
        Catch ex As Exception
            Logger.Error(ex, "CreateNGLTariffBidNoBookAsync Error: {@ex}")
            'ignore all errors for async processing at this time
            'just log as App Error
            'SaveAppError(String.Concat("CreateNGLTariffBidNoBookAsync Error for LoadTenderControl = ", LoadTenderControl.ToString(), ": ", ex.Message, ". No user message displayed."))
        End Try
    End Sub

    ''' <summary>
    ''' Method to write NGL Tariff Bids to the Load Tender Bids table
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.0.002 on 12/17/2020
    '''     added support for Async Messages
    ''' Modified by RHR for v-8.5.3.001 on 05/25/2022 we now use InsertNGLTariffBid365
    '''Modified by RHR for v-8.5.4.001 on 06/28/2023
    ''' added logic to support temperature selection when Rate Shopping 
    ''' </remarks>
    Public Function CreateNGLTariffBidNoBook(ByVal order As DAL.Models.RateRequestOrder,
                                       ByVal LoadTenderControl As Integer,
                                       ByRef strMsg As String,
                                       Optional ByVal blnAsyncMessagesPossible As Boolean = False,
                                       Optional ByVal tariffOptions As DTO.GetCarriersByCostParameters = Nothing) As Boolean

        Dim results As Boolean
        Using Logger.StartActivity("CreateNGLTariffBidNoBook - order: {@order}, LoadTenderControl: {LoadTenderControl}, tariffOptions: {tariffOptions}", order, LoadTenderControl, tariffOptions)
            If tariffOptions Is Nothing Then tariffOptions = New DTO.GetCarriersByCostParameters(False, False, True, 0, 0) 'use default for rate shop prefered = false, noLateDelivery = False, validated = false, optimizeByCapacity = True,  modeTypeControl = 0,   tempType = 0

            Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
            Dim oLT As New DAL.NGLLoadTenderData(Parameters)
            Dim oBid As New DAL.NGLBidData(Parameters)
            Dim rateShop As New DTO.RateShop
            Dim lRevs As New List(Of DTO.BookRevenue)

            'get the accessorials and compcontrols
            Dim lAccessorial As New List(Of DTO.BookFee)
            Dim origCompControl As Integer = 0
            Dim destCompControl As Integer = 0

            Logger.Information("CreateNGLTariffBidNoBook - GetInfoForLTRateQuoteTariffBids - LoadTenderControl: {LoadTenderControl}, origCompControl: {origCompControl}, destCompControl: {destCompControl}, lAccessorial: {@lAccessorial}, {@order.AccessorialValues} ", LoadTenderControl, origCompControl, destCompControl, lAccessorial, order.AccessorialValues)
            Dim blnContinue = oLT.GetInfoForLTRateQuoteTariffBids(LoadTenderControl, order.Accessorials, origCompControl, destCompControl, lAccessorial, order.AccessorialValues)
            Logger.Information("CreateNGLTariffBidNoBoodk - blnContinue: {blnContinue}", blnContinue)
            If Not blnContinue Then Return True 'Nothing to do

            ''If (Not oP44Request.origin Is Nothing) Then
            ''If (Not oP44Request.destination Is Nothing) Then

            If order Is Nothing OrElse order.Stops Is Nothing OrElse order.Stops.Count() < 1 Then
                'this should never happen unless there is a design bug.
                Logger.Warning("CreateNGLTariffBidNoBook - Shipping information is missing stop data")
                ' throwInvalidOperatonException("Shipping information is missing stop data")
                Return False
            End If
            Dim TotalCases = 1
            Dim TotalWgt = 1
            Dim TotalPL = 1
            Dim TotalCube = 0
            For Each s In order.Stops

                Dim oItems = New List(Of DAL.Models.RateRequestItem)
                oLT.fillRateRequestItems(s, oItems)
                Logger.Information("fillRateRequestItems - s: {@s}, oItems: {@oItems}", s, oItems)

                Dim lBookItems As New List(Of DTO.BookItem)
                If Not oItems Is Nothing AndAlso oItems.Count() > 0 Then
                    TotalCases = oItems.Sum(Function(x) x.NumPieces)
                    TotalWgt = oItems.Sum(Function(x) x.Weight)
                    TotalPL = oItems.Sum(Function(x) x.PalletCount)
                    Logger.Information("TotalCases: {TotalCases}, TotalWgt: {TotalWgt}, TotalPL: {TotalPL}", TotalCases, TotalWgt, TotalPL)
                    For Each itm In oItems
                        Dim oBookItem = New DTO.BookItem()
                        With oBookItem
                            .BookItemCube = itm.Length * itm.Width * itm.Height
                            .BookItemFAKClass = itm.FreightClass
                            .BookItemItemNumber = itm.ItemNumber
                            .BookItemControl = itm.ItemControl
                            .BookItemNMFCClass = itm.NMFCItem
                            '.BookItemHazmatTypeCode = itm.HazmatClass 'need lookup for hazmat type code using hazmatclass
                            .BookItemPallets = itm.PalletCount
                            '.BookItemPalletTypeID = itm.PackageType 'need to identify if a lookup for BookItemPalletTypeID is needed
                            .BookItemQtyHeight = itm.Height
                            .BookItemQtyLength = itm.Length
                            .BookItemQtyOrdered = itm.NumPieces
                            .BookItemQtyWidth = itm.Width
                            .BookItemStackable = itm.Stackable
                            .BookItemWeight = itm.Weight
                            .BookItemBookLoadControl = s.BookControl
                        End With
                        Logger.Information("CreateNGLTariffBidNoBook - oBookItem: {@oBookItem}", oBookItem)
                        lBookItems.Add(oBookItem)

                        TotalCube += itm.Length * itm.Width * itm.Height
                    Next

                    Logger.Information("TotalCube: {TotalCube}", TotalCube)
                End If

                Dim lBookLoads As New List(Of DTO.BookLoad)

                Dim oBookLoad As New DTO.BookLoad

                order.prepareTemperatureSettings() ' Modified by RHR for v-8.5.4.001 on 06/28/2023
                Logger.Information("CreateNGLTariffBidNoBook - order.TariffTempType: {TariffTempType}", order.TariffTempType)
                With oBookLoad
                    .BookLoadControl = s.BookControl
                    .BookLoadBookControl = s.BookControl
                    .BookLoadPONumber = s.BookCarrOrderNumber
                    .BookLoadWgt = TotalWgt
                    .BookLoadCaseQty = TotalCases
                    .BookLoadPL = TotalPL
                    .BookLoadCube = Conversion.Int(TotalCube)
                    .BookLoadPONumber = "RateShop"
                    .BookLoadCom = order.CommCodeType ' Modified by RHR for v-8.5.4.001 on 06/28/2023
                    .BookItems = lBookItems
                End With
                Logger.Information("CreateNGLTariffBidNoBook - Create oBookLoad: {@oBookLoad}", oBookLoad)
                lBookLoads.Add(oBookLoad)
                Dim bRev As New DTO.BookRevenue

                With bRev
                    .BookOrigCompControl = origCompControl
                    .BookOrigName = order.Pickup.CompName
                    .BookOrigAddress1 = order.Pickup.CompAddress1
                    .BookOrigAddress2 = order.Pickup.CompAddress2
                    .BookOrigAddress3 = order.Pickup.CompAddress3
                    .BookOrigCity = order.Pickup.CompCity
                    .BookOrigState = order.Pickup.CompState
                    .BookOrigZip = order.Pickup.CompPostalCode
                    .BookOrigCountry = order.Pickup.CompCountry
                    .BookDestCompControl = destCompControl
                    .BookDestName = s.CompName
                    .BookDestAddress1 = s.CompAddress1
                    .BookDestAddress2 = s.CompAddress2
                    .BookDestAddress3 = s.CompAddress3
                    .BookDestCity = s.CompCity
                    .BookDestState = s.CompState
                    .BookDestCountry = s.CompCountry
                    .BookDestZip = s.CompPostalCode
                    .BookTotalWgt = TotalWgt
                    .BookTotalCases = TotalCases
                    .BookTotalPL = TotalPL
                    .BookTotalCube = Conversion.Int(TotalCube)
                    .BookTotalPX = TotalPL
                    '                        BookModeTypeControl: ddlMode,
                    .BookMilesFrom = 0 'note:  oBookRevBLL.DoRateShop will get the miles
                    .BookDateLoad = returnDateFromString(order.ShipDate)
                    .BookDateRequired = returnDateFromString(order.DeliveryDate, If(.BookDateLoad.HasValue, .BookDateLoad.Value.AddDays(3), Date.Now.AddDays(3)))
                    .BookLoads = lBookLoads
                    .BookCarrOrderNumber = s.BookCarrOrderNumber
                    .BookConsPrefix = s.SHID
                    .BookSHID = s.SHID
                    .BookStopNo = s.StopNumber
                    .BookControl = s.BookControl
                    .BookProNumber = s.BookProNumber
                End With
                Logger.Information("Create Book Revision : {@bRev}", bRev)
                lRevs.Add(bRev)
            Next
            Logger.Information("Setting lRevs(0).BookFees = {@lAccessorial}", lAccessorial)
            lRevs(0).BookFees = lAccessorial    'TIM TOTAL HACK

            With rateShop
                .BookRevs = lRevs
                .BookFees = lAccessorial
                .CarrierControl = .Prefered = tariffOptions.prefered
                .NoLateDelivery = tariffOptions.noLateDelivery
                .Validated = tariffOptions.validated
                .OptimizeByCapacity = tariffOptions.optimizeByCapacity
                .ModeTypeControl = tariffOptions.modeTypeControl
                .TempType = If(order.TariffTempType, 1) 'Modified by RHR for v-8.5.4.001 on 06/28/2023
                .TariffTypeControl = 0
                .CarrTarEquipMatClass = Nothing
                .CarrTarEquipMatClassTypeControl = 0
                .CarrTarEquipMatTarRateTypeControl = 0
                .AgentControl = 0
                .Outbound = If(origCompControl <> 0, True, If(destCompControl = 0, True, False)) ' Modified by RHR for v-8.5.4.002 on 07/20/2023 we now set Outboud to true when both orig and dest compcontrols are 0, supports new Orig zip code tariffs -- Old code If If(origCompControl <> 0, True, False)
                .UsePCM = 1
                .UseERE = 0
            End With

            Logger.Information("Created rateShop Request Object: {@rateShop}", rateShop)

            Dim res = oBookRevBLL.DoRateShop(rateShop)

            Logger.Information("DoRateShop returned: {@res}", res)

            ' Modified by RHR for v-8.3.0.002 on 12/17/2020
            'Modified by RHR for v-8.5.3.001 on 05/27/2022 added logic for tblLoadTenderLog records And New Tariff Options
            If res Is Nothing Then
                'If blnAsyncMessagesPossible Then ' as of v-8.5.3.001 we always save cost messages and logs when no tariff data is found
                res = New DTO.CarrierCostResults()
                res.AddLog("No Tariff data found.")
                Logger.Warning("CreateNGLTariffBidNoBook - No Tariff data found.", res)
                NGLLoadTenderData.saveLoadTenderCarrierCostMessages(LoadTenderControl, NGLLoadTenderTypes.RateQuote, res)
                'End If
                Return True 'Nothing to do
            End If
            ' Modified by RHR for v-8.3.0.002 on 12/17/2020
            'Removed by RHR for v-8.5.3.001 on 05/27/2022 added logic for tblLoadTenderLog records And New Tariff Options
            'If blnAsyncMessagesPossible Then
            '    NGLLoadTenderData.saveLoadTenderCarrierCostMessages(LoadTenderControl, NGLLoadTenderTypes.RateQuote, res)
            'End If
            'Modified by RHR for v-8.5.3.001 on 05/27/2022 added logic for tblLoadTenderLog records And New Tariff Options
            If res.CarriersByCost Is Nothing OrElse res.CarriersByCost.Count < 1 Then
                res.AddLog("No Tariff data found.")
                Logger.Warning("CreateNGLTariffBidNoBook - No Tariff data found.", res)
                NGLLoadTenderData.saveLoadTenderCarrierCostMessages(LoadTenderControl, NGLLoadTenderTypes.RateQuote, res)
                Return True 'Nothing to do
            End If

            Logger.Information("CreateNGLTariffBidNoBook - res.CarriersByCost: {@res.CarriersByCost}", res.CarriersByCost)
            'Modified by RHR for v-8.5.3.001 on 05/25/2022 we now use InsertNGLTariffBid365


            results = oBid.InsertNGLTariffBid365(res, res.BookRevs, LoadTenderControl, "")

            Logger.Information("CreateNGLTariffBidNoBook - results: {results}", results)
        End Using
        Return results

    End Function



#End Region

#Region "API Methods"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BidControl"></param>
    ''' <param name="BidCarrierControl"></param>
    ''' <param name="BidLineHaul"></param>
    ''' <param name="BidFuelUOM"></param>
    ''' <param name="BidFuelVariable"></param>
    ''' <param name="BidFuelTotal"></param>
    ''' <param name="BidTotalCost"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Added parameter CarrierContControl (gets passed in from dispatch model)
    '''   Added logic to pass CarrierContControl to call to NSP44AcceptTemp()
    ''' NOTE:
    '''  The parameter CarrierContControl will eventually be optional with a Default value of 0. 
    '''  However, in initial development do not make it optional to ensure proper testing
    '''  When unit testing is complete change the parameter to optional
    ''' Modified by RHR for v-8.2 on 6/30/2019 
    '''  added logic to support DAL.Models.Dispatch which contains
    '''  the new AutoAcceptOnDispatch flag
    '''  and the new EmailLoadTenderSheet flag
    ''' </remarks>
    Public Function NSP44Accept(ByVal LTControl As Integer,
                                ByVal BookControl As Integer,
                                ByVal BidControl As Integer,
                                ByVal BidCarrierControl As Integer,
                                ByVal BidLineHaul As Decimal,
                                ByVal BidFuelUOM As String,
                                ByVal BidFuelVariable As Decimal,
                                ByVal BidFuelTotal As Decimal,
                                ByVal BidTotalCost As Decimal,
                                ByVal CarrierContControl As Integer,
                                Optional ByVal oDispatch As DAL.Models.Dispatch = Nothing) As DTO.WCFResults
        'Dim oCarrier As New DAL.NGLCarrierData(Parameters)
        'Dim CarrierControl As Integer = 0
        'CarrierControl = oCarrier.getCarrierControlBySCAC(BidCarrierSCAC)
        'If CarrierControl = 0 Then
        '    Dim wcfRet As New DTO.WCFResults
        '    wcfRet.Success = False
        '    'NEXTStop Error: Accept P44 Bid failed for LoadTenderControl {0} and BidControl {1}.{2}No CarrierControl was found using CarrierSCAC {3}.{2}Source: {4}.
        '    Dim source = "NGL.FM.BLL.NGLDATBLL.NSP44Accept (getCarrierControlBySCAC)"
        '    Dim p() As String = {LTControl, BidControl, vbCrLf, BidCarrierSCAC, source}
        '    wcfRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_NSP44AcceptNoCarrier, p)
        '    Dim msg = String.Format(DTO.WCFResults.getMessageNotLocalizedString(DTO.WCFResults.MessageEnum.E_NSP44AcceptNoCarrier), p)
        '    NGLSystemLogData.AddApplicaitonLog(msg, source)
        'End If

        'Modified By LVV On 2/19/2019 For bug fix add Carrier Contact selection To Dispatch - Added parameter CarrierContControl so it could be passed to call to NSP44AcceptTemp()
        'Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
        Return NSP44AcceptTemp(LTControl, BookControl, BidControl, BidCarrierControl, BidLineHaul, BidFuelUOM, BidFuelVariable, BidFuelTotal, BidTotalCost, CarrierContControl, oDispatch)

    End Function

    ''' <summary>
    ''' NOTE TODO -- I DON"T KNOW IF THIS IS BEING USED ANYWHERE
    ''' PROBABLY WE CAN DEPRECIATE THIS METHOD AT SOME POINT
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="BidControl"></param>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Added parameter CarrierContControl (gets passed in from dispatch model)
    '''   Added logic to pass CarrierContControl to call to NSP44AcceptTemp()
    ''' NOTE:
    '''  The parameter CarrierContControl will eventually be optional with a Default value of 0. 
    '''  However, in initial development do not make it optional to ensure proper testing
    '''  When unit testing is complete change the parameter to optional
    ''' Modified by RHR for v-8.2 on 6/30/2019 
    '''  added logic to support DAL.Models.Dispatch which contains
    '''  the new AutoAcceptOnDispatch flag
    '''  and the new EmailLoadTenderSheet flag
    ''' </remarks>
    Public Function NSP44Accept(ByVal LoadTenderControl As Integer,
                                ByVal BidControl As Integer,
                                ByVal BookControl As Integer,
                                ByVal CarrierContControl As Integer,
                                Optional ByVal oDispatch As DAL.Models.Dispatch = Nothing) As DTO.WCFResults
        Dim oBidData As New DAL.NGLBidData(Parameters)
        Dim oCarrier As New DAL.NGLCarrierData(Parameters)

        If LoadTenderControl = 0 Then oBidData.throwFieldRequiredException("Load Tender Control") 'The system could not generate a new {0}
        If BidControl = 0 Then oBidData.throwFieldRequiredException("Bid Control") 'The system could not generate a new {0}
        If BookControl = 0 Then oBidData.throwFieldRequiredException("Book Control") 'The system could not generate a new {0}

        Dim BidCarrierControl As Integer
        Dim BidLineHaul As Decimal
        Dim BidFuelUOM As String
        Dim BidFuelVariable As Decimal
        Dim BidFuelTotal As Decimal
        Dim BidTotalCost As Decimal
        'CannotAcceptFinalizeRecord
        Dim oBidRecord = oBidData.GetRecord(BidControl)
        If oBidRecord Is Nothing Then oBidData.throwFieldRequiredException("Bid Control")
        BidCarrierControl = oBidRecord.BidCarrierControl
        BidLineHaul = oBidRecord.BidLineHaul
        BidFuelUOM = oBidRecord.BidFuelUOM
        BidFuelVariable = oBidRecord.BidFuelVariable
        BidFuelTotal = oBidRecord.BidFuelTotal
        BidTotalCost = oBidRecord.BidTotalCost
        'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
        'Modified by RHR for v-8.2 on 6/30/2019 added logic to support DAL.Models.Dispatch data
        Return NSP44AcceptTemp(LoadTenderControl, BookControl, BidControl, BidCarrierControl, BidLineHaul, BidFuelUOM, BidFuelVariable, BidFuelTotal, BidTotalCost, CarrierContControl, oDispatch)

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BidControl"></param>
    ''' <param name="BidCarrierControl"></param>
    ''' <param name="BidLineHaul"></param>
    ''' <param name="BidFuelUOM"></param>
    ''' <param name="BidFuelVariable"></param>
    ''' <param name="BidFuelTotal"></param>
    ''' <param name="BidTotalCost"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="oDispatch"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' This is a copy of NSAPITender so we can make changes that do not impact the 
    ''' dispatch to EDI 204 from API logic.
    ''' </remarks>
    Public Function NSAPIAccept(ByVal LTControl As Integer,
                                ByVal BookControl As Integer,
                                ByVal BidControl As Integer,
                                ByVal BidCarrierControl As Integer,
                                ByVal BidLineHaul As Decimal,
                                ByVal BidFuelUOM As String,
                                ByVal BidFuelVariable As Decimal,
                                ByVal BidFuelTotal As Decimal,
                                ByVal BidTotalCost As Decimal,
                                ByVal CarrierContControl As Integer,
                                Optional ByVal oDispatch As DAL.Models.Dispatch = Nothing) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim oRet As New DTO.WCFResults
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim sOldTrancode As String = "N"  'N is default

        Dim source = "NGL.FM.BLL.NGLDATBLL.NSAPIAccept"

        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            oBookRevBLL.throwFieldRequiredException("Book Control")
        End If

        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'Delete the DAT Posting
            Dim blnDATDeleteSuccess As Boolean = True
            Dim datRes = NGLDATBLL.DeleteDATPosting(oRet, oBookRevs, blnDATDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected)
            'We have to at least save the 2 BookRevLT fields
            'results.AddLog("Save changes to booking records")
            'NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
            NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnDATDeleteSuccess, True, True, source, False)
        End If

        Dim oBid = NGLBidData.GetRecord(BidControl)
        If oDispatch Is Nothing Then
            oDispatch = NGLLoadTenderData.getBidToDispatch(BidControl)
        End If
        updateAPIBookFromBid(oBookRevs, oBid, oDispatch, True)

        Dim bookFeesVar As Decimal = BidFuelVariable
        Dim bookfeesmin As Decimal = 0
        Dim bookFee As DTO.BookFee = CreateAPIFuelFee(BookControl, bookFeesVar, bookfeesmin, BidFuelUOM, BidFuelTotal)

        Dim totalbfc = oBookRevs(0).BookSpotRateTotalUnallocatedBFC
        If totalbfc = Nothing Then totalbfc = 0
        Dim bookfees As New List(Of DTO.BookFee)
        If Not bookFee Is Nothing Then bookfees.Add(bookFee)
        Dim dTotalOtherCosts As Decimal = ProcessAPIOtherCosts(BookControl, BidControl, oDispatch.Accessorials, bookfees)

        Dim lh = BidTotalCost - BidFuelTotal - dTotalOtherCosts
        oDispatch.LineHaul = lh
        Dim oResult As DTO.CarrierCostResults = saveAPIBidasSpotRate(oBookRevBLL, oBookRevs, bookfees, lh, totalbfc, BidCarrierControl, CarrierContControl)

        Dim sNewTranCode As String = "PC"
        Dim iOverrideSendLoadTenderEmail = 0
        If Not oDispatch Is Nothing Then
            If oDispatch.AutoAcceptOnDispatch = True Then sNewTranCode = "PB"
            If oDispatch.EmailLoadTenderSheet = True Then
                iOverrideSendLoadTenderEmail = 1
            Else
                iOverrideSendLoadTenderEmail = -1
            End If
        End If

        Dim res = oBookBLL.ProcessNewTransCode(BookControl, sNewTranCode, sOldTrancode, LTTypeEnum.LoadBoard, iOverrideSendLoadTenderEmail)

        If Not oRet.Warnings Is Nothing AndAlso oRet.Warnings.Count > 0 Then
            res.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, oRet.Warnings)
        End If
        If Not oDispatch Is Nothing Then
            'create or update booking, company and lane data associated with this shipment
            Dim strMsg As String = ""
            Dim iBookControl = NGLLoadTenderData.dispatchLoadTender(LTControl, oDispatch, strMsg)
        End If

        'update bid records
        NGLLoadTenderData.AcceptNextStopBid(LTControl, BidControl)

        Return res
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BidControl"></param>
    ''' <param name="BidCarrierControl"></param>
    ''' <param name="BidLineHaul"></param>
    ''' <param name="BidFuelUOM"></param>
    ''' <param name="BidFuelVariable"></param>
    ''' <param name="BidFuelTotal"></param>
    ''' <param name="BidTotalCost"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <param name="oDispatch"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.004 on 08/11/2022 fixed default Fuel Cost issues
    ''' Modified by RHR for v-8.5.3.004 on 08/11/2022 fixed bid type enum mapping issue
    ''' </remarks>
    Public Function NSAPITender(ByVal LTControl As Integer,
                                ByVal BookControl As Integer,
                                ByVal BidControl As Integer,
                                ByVal BidCarrierControl As Integer,
                                ByVal BidLineHaul As Decimal,
                                ByVal BidFuelUOM As String,
                                ByVal BidFuelVariable As Decimal,
                                ByVal BidFuelTotal As Decimal,
                                ByVal BidTotalCost As Decimal,
                                ByVal CarrierContControl As Integer,
                                Optional ByVal oDispatch As DAL.Models.Dispatch = Nothing) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim wcf As New DTO.WCFResults
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)

        Dim sOldTrancode As String = "N"  'N is default

        Dim source = "NGL.FM.BLL.NGLDATBLL.NSAPITender"

        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            oBookRevBLL.throwFieldRequiredException("Book Control")
        End If

        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'Delete the DAT Posting
            Dim blnDATDeleteSuccess As Boolean = True
            Dim datRes = NGLDATBLL.DeleteDATPosting(wcf, oBookRevs, blnDATDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected)
            'We have to at least save the 2 BookRevLT fields
            'results.AddLog("Save changes to booking records")
            'NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
            NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnDATDeleteSuccess, True, True, source, False)
        End If


        Dim oBid = NGLBidData.GetRecord(BidControl)
        updateAPIBookFromBid(oBookRevs, oBid, oDispatch, True)

        ' Begin Modified by RHR for v-8.5.3.005 on 08//230022 fixed default Fuel Cost issues
        Dim bookFeesVar As Decimal = BidFuelVariable
        Dim bookfeesmin As Decimal = 0
        Dim bookFee As DTO.BookFee = CreateAPIFuelFee(BookControl, bookFeesVar, bookfeesmin, BidFuelUOM, BidFuelTotal)
        'End Modified by RHR for v-8.5.3.004 on 08/11/2022 fixed default Fuel Cost issues

        Dim totalbfc = oBookRevs(0).BookSpotRateTotalUnallocatedBFC
        If totalbfc = Nothing Then totalbfc = 0
        Dim bookfees As New List(Of DTO.BookFee)
        If Not bookFee Is Nothing Then bookfees.Add(bookFee)
        Dim dTotalOtherCosts As Decimal = ProcessAPIOtherCosts(BookControl, BidControl, oDispatch.Accessorials, bookfees)

        Dim lh = BidTotalCost - BidFuelTotal - dTotalOtherCosts
        oDispatch.LineHaul = lh
        Dim oResult As DTO.CarrierCostResults = saveAPIBidasSpotRate(oBookRevBLL, oBookRevs, bookfees, lh, totalbfc, BidCarrierControl, CarrierContControl)
        'Modified by RHR for v-8.2 on 6/30/2019 added logic to support AutoAcceptOnDispatch flag
        Dim sNewTranCode As String = "PC"
        Dim iOverrideSendLoadTenderEmail = 0
        If Not oDispatch Is Nothing Then
            If oDispatch.AutoAcceptOnDispatch = True Then sNewTranCode = "PB"
            If oDispatch.EmailLoadTenderSheet = True Then
                iOverrideSendLoadTenderEmail = 1
            Else
                iOverrideSendLoadTenderEmail = -1
            End If
        End If

        Dim res = oBookBLL.ProcessNewTransCode(BookControl, sNewTranCode, sOldTrancode, LTTypeEnum.LoadBoard, iOverrideSendLoadTenderEmail)

        If Not wcf.Warnings Is Nothing AndAlso wcf.Warnings.Count > 0 Then
            res.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcf.Warnings)
        End If
        If Not oDispatch Is Nothing Then
            'create or update booking, company and lane data associated with this shipment
            Dim strMsg As String = ""
            Dim iBookControl = NGLLoadTenderData.dispatchLoadTender(LTControl, oDispatch, strMsg)
        End If

        'update bid records
        NGLLoadTenderData.AcceptNextStopBid(LTControl, BidControl)

        Return res
    End Function



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LTControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="BidControl"></param>
    ''' <param name="BidCarrierControl"></param>
    ''' <param name="BidLineHaul"></param>
    ''' <param name="BidFuelUOM"></param>
    ''' <param name="BidFuelVariable"></param>
    ''' <param name="BidFuelTotal"></param>
    ''' <param name="BidTotalCost"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Added parameter CarrierContControl (gets passed in from dispatch model)
    '''   Added logic to pass CarrierContControl into the method GetDefaultContactForCarrier() as an optional param
    ''' NOTE:
    '''  The parameter CarrierContControl will eventually be optional with a Default value of 0. 
    '''  However, in initial development do not make it optional to ensure proper testing
    '''  When unit testing is complete change the parameter to optional
    ''' Modified by RHR for v-8.2 on 6/30/2019 
    '''  added logic to support DAL.Models.Dispatch which contains
    '''  the new AutoAcceptOnDispatch flag
    '''  and the new EmailLoadTenderSheet flag
    ''' Modified by RHR for v-8.2 on 7/16/2019 
    '''   added logic to use the Carrier Pro as the SHID if provided
    ''' Modified by RHR for v-8.2.0.117 on 9/4/2019 
    '''     fixed issue with carrier pro number
    '''     fixed issue with missing accessorial costing
    '''     fixed issue with invalid line haul and discount
    ''' Modified by RHR for v-8.2.1 on 11/07/2019
    '''     We now ignore zero cost accessorials when dispatching loads via API
    '''     Previously all fees were set to a default of $1.00 so they would show 
    '''     on the BOL.  This causes issues with the Freight Bill Audit.
    '''     User must use the pickup or delivery notes for information about appointments or other special instructions.
    '''     Approved by John R. 
    '''     Additionally we modified how Captions are identified
    '''     Also we now update the EDI code with the tblAccessorial EDI code
    ''' Modified by RHR for v-8.2.1 on 11/14/2019
    '''     we now reset book data to N status before generating a spot rate when using API Dispatching.
    ''' </remarks>
    Public Function NSP44AcceptTemp(ByVal LTControl As Integer,
                                    ByVal BookControl As Integer,
                                    ByVal BidControl As Integer,
                                    ByVal BidCarrierControl As Integer,
                                    ByVal BidLineHaul As Decimal,
                                    ByVal BidFuelUOM As String,
                                    ByVal BidFuelVariable As Decimal,
                                    ByVal BidFuelTotal As Decimal,
                                    ByVal BidTotalCost As Decimal,
                                    ByVal CarrierContControl As Integer,
                                    Optional ByVal oDispatch As DAL.Models.Dispatch = Nothing) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim wcf As New DTO.WCFResults
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim sOldTrancode As String = "N"  'N is default

        Dim source = "NGL.FM.BLL.NGLDATBLL.NSP44AcceptTemp"
        'ProcessNewTransCode(BookControl, "N", "PC")

        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            oBookRevBLL.throwFieldRequiredException("Book Control")
        End If

        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'Delete the DAT Posting
            Dim blnDATDeleteSuccess As Boolean = True
            Dim datRes = NGLDATBLL.DeleteDATPosting(wcf, oBookRevs, blnDATDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected)
            'We have to at least save the 2 BookRevLT fields
            'results.AddLog("Save changes to booking records")
            'NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
            NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnDATDeleteSuccess, True, True, source, False)
        End If
        Dim oBid = NGLBidData.GetRecord(BidControl)
        If oDispatch Is Nothing Then
            oDispatch = NGLLoadTenderData.getBidToDispatch(BidControl)
        End If
        updateAPIBookFromBid(oBookRevs, oBid, oDispatch, True)

        Dim bookFeesVar As Decimal = BidFuelVariable
        Dim bookfeesmin As Decimal = 0
        Dim bookFee As DTO.BookFee = CreateAPIFuelFee(BookControl, bookFeesVar, bookfeesmin, BidFuelUOM, BidFuelTotal)

        Dim totalbfc = oBookRevs(0).BookSpotRateTotalUnallocatedBFC
        If totalbfc = Nothing Then totalbfc = 0
        Dim bookfees As New List(Of DTO.BookFee)
        If Not bookFee Is Nothing Then bookfees.Add(bookFee)
        Dim dTotalOtherCosts As Decimal = ProcessAPIOtherCosts(BookControl, BidControl, oDispatch.Accessorials, bookfees)

        Dim lh = BidTotalCost - BidFuelTotal - dTotalOtherCosts
        oDispatch.LineHaul = lh
        Dim oResult As DTO.CarrierCostResults = saveAPIBidasSpotRate(oBookRevBLL, oBookRevs, bookfees, lh, totalbfc, BidCarrierControl, CarrierContControl)

        Dim sNewTranCode As String = "PC"
        Dim iOverrideSendLoadTenderEmail = 0
        If Not oDispatch Is Nothing Then
            If oDispatch.AutoAcceptOnDispatch = True Then sNewTranCode = "PB"
            If oDispatch.EmailLoadTenderSheet = True Then
                iOverrideSendLoadTenderEmail = 1
            Else
                iOverrideSendLoadTenderEmail = -1
            End If
        End If

        Dim res = oBookBLL.ProcessNewTransCode(BookControl, sNewTranCode, sOldTrancode, LTTypeEnum.LoadBoard, iOverrideSendLoadTenderEmail)

        If Not wcf.Warnings Is Nothing AndAlso wcf.Warnings.Count > 0 Then
            res.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, wcf.Warnings)
        End If
        If Not oDispatch Is Nothing Then
            'create or update booking, company and lane data associated with this shipment
            Dim strMsg As String = ""
            Dim iBookControl = NGLLoadTenderData.dispatchLoadTender(LTControl, oDispatch, strMsg)
        End If

        'update bid records
        NGLLoadTenderData.AcceptNextStopBid(LTControl, BidControl)

        Return res
    End Function


    Public Function NSP44AssignSilent(ByVal iLoadTenderControl As Integer,
                                    ByVal BookControl As Integer,
                                    ByVal sOrderNumber As String,
                                    ByVal oPrefBid As clsPreferedDefaultCarrier,
                                    ByVal CarrierContControl As Integer) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim oRet As New DTO.WCFResults
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim sOldTrancode As String = "N"  'N is default
        Dim sBidReference = String.Format("Order Number: {0}", sOrderNumber)
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        oRet.updateKeyFields("BookControl", BookControl.ToString())
        If oPrefBid Is Nothing OrElse oPrefBid.SelectedBid Is Nothing OrElse oPrefBid.SelectedBid.BidControl = 0 Then
            oRet.Success = False
            oRet.AddLog("P44 API assign carrier silent failed because no quote for " & sBidReference)
            Return oRet
        End If
        sBidReference = String.Format("Order Number: {0}, Carrier Name: {1}", sOrderNumber, oPrefBid.SelectedBid.BidCarrierName)
        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            oRet.Success = False
            oRet.AddLog("P44 API assign carrier silent failed because no booking orders found for " & sBidReference)
            Return oRet
        End If
        oRet.updateKeyFields("BidControl", oPrefBid.SelectedBid.BidControl.ToString())
        Dim BidControl As Integer
        Dim BidCarrierControl As Integer
        Dim BidLineHaul As Decimal
        Dim BidFuelUOM As String
        Dim BidFuelVariable As Decimal
        Dim BidFuelTotal As Decimal
        Dim BidTotalCost As Decimal
        BidControl = oPrefBid.SelectedBid.BidControl
        BidCarrierControl = oPrefBid.SelectedBid.BidCarrierControl
        BidLineHaul = oPrefBid.SelectedBid.BidLineHaul
        BidFuelUOM = oPrefBid.SelectedBid.BidFuelUOM
        BidFuelVariable = oPrefBid.SelectedBid.BidFuelVariable
        BidFuelTotal = oPrefBid.SelectedBid.BidFuelTotal
        BidTotalCost = oPrefBid.SelectedBid.BidTotalCost
        Dim source = "NGL.FM.BLL.NGLDATBLL.NSP44AssignSilent"

        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'For Silent processing we cannot move forward this load has been posted to a Load Board for bids
            'this should not happen on New Orders
            oRet.Success = False
            oRet.AddLog("API assign carrier silent failed because the booking has been posted to DAT or NEXTStop.")
            oRet.AddLog("Manual assignment to the carrier is required for " & sBidReference)
            Return oRet
        End If
        Dim oBid = oPrefBid.SelectedBid
        Dim oDispatch = NGLLoadTenderData.getBidToDispatch(oPrefBid.SelectedBid.BidControl)
        updateAPIBookFromBid(oBookRevs, oBid, oDispatch)

        Dim bookFeesVar As Decimal = BidFuelVariable
        Dim bookfeesmin As Decimal = 0
        Dim bookFee As DTO.BookFee = CreateAPIFuelFee(BookControl, bookFeesVar, bookfeesmin, BidFuelUOM, BidFuelTotal)

        Dim totalbfc = oBookRevs(0).BookSpotRateTotalUnallocatedBFC
        If totalbfc = Nothing Then totalbfc = 0
        Dim bookfees As New List(Of DTO.BookFee)
        If Not bookFee Is Nothing Then bookfees.Add(bookFee)
        Dim dTotalOtherCosts As Decimal = ProcessAPIOtherCosts(BookControl, BidControl, oDispatch.Accessorials, bookfees)
        'Note:  ProcessAPIOtherCosts uses Dispatch.Accessorials so it does not apply filters like:
        'If (
        '                oFee.BidCostAdjDescCode <> "FUE" _
        '                And
        '                oFee.BidCostAdjDescCode <> "FSC" _
        '                And
        '                oFee.BidCostAdjDescCode <> "AFSC" _
        '                And
        '                oFee.BidCostAdjDescCode <> "OFSC" _
        '                And
        '                oFee.BidCostAdjDescCode <> "GFC" _
        '                And
        '                oFee.BidCostAdjDescCode <> "DSC") _
        '                AndAlso
        '                (oFee.BidCostAdjAmount > 0) Then

        Dim lh = BidTotalCost - BidFuelTotal - dTotalOtherCosts
        oDispatch.LineHaul = lh
        Dim oResult As DTO.CarrierCostResults = saveAPIBidasSpotRate(oBookRevBLL, oBookRevs, bookfees, lh, totalbfc, BidCarrierControl, CarrierContControl)

        Dim sNewTranCode As String = "P"
        Dim iOverrideSendLoadTenderEmail = 0
        Dim res = oBookBLL.ProcessNewTransCode(BookControl, sNewTranCode, sOldTrancode, LTTypeEnum.LoadBoard, iOverrideSendLoadTenderEmail)

        If Not oRet.Warnings Is Nothing AndAlso oRet.Warnings.Count > 0 Then
            res.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, oRet.Warnings)
        End If
        'update bid records
        NGLLoadTenderData.AcceptNextStopBid(iLoadTenderControl, BidControl)

        Return res
    End Function


    ''' <summary>
    ''' Temporary Place Holder
    ''' </summary>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="BidControl"></param>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 unfinished code based off of the P44 logic for accepting loads
    '''     Note:  this method does not use the dispatch data.  We should make sure any changes
    '''     made durring dispatching are updated here.
    '''     we need to check that costs are calculated correctly
    '''     Fuel needs work
    ''' Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    '''   Added parameter CarrierContControl (gets passed in from dispatch model)
    '''   Added logic to pass CarrierContControl into the method GetDefaultContactForCarrier() as an optional param
    ''' NOTE:
    '''  The parameter CarrierContControl will eventually be optional with a Default value of 0. 
    '''  However, in initial development do not make it optional to ensure proper testing
    '''  When unit testing is complete change the parameter to optional
    '''  
    ''' Modified By LVV on 7/2/19 for v-8.2
    '''  Fixed it so it would allow assigning a Zero Cost Carrier via Spot 
    ''' Modified by RHR for v-8.2 on 7/16/2019 
    '''   added logic to use the Carrier Pro as the SHID if provided
    ''' Modified by RHR for v-8.5.2.003 on 08/14/2023 
    '''     added logic to include Rate It shipment id when needed
    '''     Added logic to get the next consolidation number when needed
    ''' </remarks>
    Public Function NSAcceptSpotRate(ByVal LoadTenderControl As Integer,
                                     ByVal BidControl As Integer,
                                     ByVal BookControl As Integer,
                                     ByVal CarrierContControl As Integer,
                                     Optional ByVal oDispatch As DAL.Models.Dispatch = Nothing) As DTO.WCFResults
        Dim oBidData As New DAL.NGLBidData(Parameters)
        Dim oBookBLL As New NGLBookBLL(Parameters)
        Dim oBookRevBLL As New NGLBookRevenueBLL(Parameters)
        Dim oBrack As New DAL.NGLtblTarBracketTypeData(Parameters)

        If LoadTenderControl = 0 Then oBidData.throwFieldRequiredException("Load Tender Control") 'The system could not generate a new {0}
        If BidControl = 0 Then oBidData.throwFieldRequiredException("Bid Control") 'The system could not generate a new {0}
        If BookControl = 0 Then oBidData.throwFieldRequiredException("Book Control") 'The system could not generate a new {0}

        Dim BidCarrierControl As Integer
        Dim BidLineHaul As Decimal
        Dim BidFuelUOM As String
        Dim BidFuelVariable As Decimal
        Dim BidFuelTotal As Decimal
        Dim BidTotalCost As Decimal
        Dim oBidRecord = oBidData.GetRecord(BidControl)
        If oBidRecord Is Nothing Then oBidData.throwFieldRequiredException("Bid Control")
        BidCarrierControl = oBidRecord.BidCarrierControl
        BidLineHaul = oBidRecord.BidLineHaul
        BidFuelUOM = oBidRecord.BidFuelUOM
        BidFuelVariable = oBidRecord.BidFuelVariable
        BidFuelTotal = oBidRecord.BidFuelTotal
        BidTotalCost = oBidRecord.BidTotalCost

        Dim oBookRevs As DTO.BookRevenue()
        Dim oRet As New DTO.WCFResults
        Dim owcfRes As New DTO.WCFResults
        Dim sOldTrancode As String = "P"  'P is default

        Dim source = "NGL.FM.BLL.NGLDATBLL.NSAcceptSpotRate"
        'ProcessNewTransCode(BookControl, "N", "PC")

        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            oBookRevBLL.throwFieldRequiredException("Book Control")
        End If

        Dim oFilter = New DAL.Models.AllFilters()
        oFilter.ParentControl = BookControl
        oFilter.page = 1
        oFilter.pageSize = 1
        Dim DalSpotRate As DAL.NGLBookSpotRateData = New DAL.NGLBookSpotRateData(Me.Parameters)
        Dim oSpotRates = DalSpotRate.GetBookSpotRateData(oFilter, 0)
        If oSpotRates Is Nothing OrElse oSpotRates.Count() < 1 Then
            oBookRevBLL.throwFieldRequiredException("Book Spot Rate") 'cannot continue 
        End If
        Dim oBookFees = DalSpotRate.GetSpotRateBookFees(BookControl)
        sOldTrancode = oBookRevs(0).BookTranCode
        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        'TODO: the wcf variable is passed by ref and we also get a datRes returned.
        'We have too many versions.  For now we just ignore both when updating the DAT Posting logic
        'further down we replace wcf variable with the results from saveing the spot rate
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'Delete the DAT Posting
            Dim blnDATDeleteSuccess As Boolean = True
            Dim datRes = NGLDATBLL.DeleteDATPosting(owcfRes, oBookRevs, blnDATDeleteSuccess, NGLBookBLL.AcceptRejectEnum.Rejected)
            'We have to at least save the 2 BookRevLT fields
            'results.AddLog("Save changes to booking records")
            'NGLBookRevenueData.SaveRevenuesNoReturn(oBookRevs, False, True)
            NGLDATBLL.ProcessLoadBoardStatusUpdates(datRes, NGLBookBLL.AcceptRejectEnum.Rejected, blnDATDeleteSuccess, True, True, source, False)
        End If
        'Modified by RHR for v-8.2 on 7/16/2019 
        Dim blnUseCarrierProAsSHID As Boolean = If(oDispatch Is Nothing, False, If(String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber), False, If(oDispatch.CarrierProNumber.Trim.Length > 0, True, False)))
        For Each b In oBookRevs
            b.BookRevLoadTenderStatusCode = UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.NStopAccepted)
            b.BookCarrTarName = "SPOT RATE"
            'Modified by RHR for v-8.5.2.003 on 08/14/2023 added logic to include Rate It shipment id when needed
            'Added logic to get the next consolidation number when needed
            If blnUseCarrierProAsSHID AndAlso Not String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber) Then
                b.BookSHID = oDispatch.CarrierProNumber
            ElseIf String.IsNullOrWhiteSpace(b.BookSHID) Then
                If Not (String.IsNullOrWhiteSpace(oDispatch.SHID)) Then
                    b.BookSHID = oDispatch.SHID
                End If
            End If
            If String.IsNullOrWhiteSpace(b.BookConsPrefix) Then
                If Not (String.IsNullOrWhiteSpace(oDispatch.SHID)) Then
                    b.BookConsPrefix = oDispatch.SHID
                Else
                    b.BookConsPrefix = NGLBookData.GetNextCNSNumber(b.BookCustCompControl)
                End If
            End If
            If String.IsNullOrWhiteSpace(b.BookSHID) Then
                b.BookSHID = b.BookConsPrefix
            End If
        Next

        Dim allocationBFCFormula As New DTO.tblTarBracketType
        Dim allocationFormula = oBrack.GettblTarBracketTypeFiltered(1)
        Dim palletBracket = oBrack.GettblTarBracketTypeFiltered(1)

        Dim AccessorialCode As Integer = 0
        Dim bookFeesVar As Decimal = BidFuelVariable
        Dim bookfeesmin As Decimal = 0
        'TODO -- fix how fuel is calculated check how old spot rate fuel work
        '        the case statment/UOM field to not use strings eventually
        '        This will have to happen after we figure out how to do drop down lists in TMS 365
        If oSpotRates(0).BookSpotRateUserCarrierFuelAddendum AndAlso BidFuelUOM = "Rate Per Mile" Or BidFuelUOM = "Percentage" Or BidFuelUOM = "Flat Rate" Then
            Select Case BidFuelUOM
                Case "Flat Rate"
                    AccessorialCode = 15
                    bookfeesmin = BidFuelTotal
                    bookFeesVar = 0
                Case "Rate Per Mile"
                    AccessorialCode = 9
                Case "Percentage"
                    AccessorialCode = 2
                Case Else
                    AccessorialCode = 15
                    bookfeesmin = BidFuelVariable
                    bookFeesVar = 0
            End Select
            Dim acc = NGLtblAccessorialData.GettblAccessorialFiltered(AccessorialCode)

            Dim bookFee As New DTO.BookFee
            With bookFee
                .BookFeesBookControl = BookControl
                .BookFeesVariable = bookFeesVar
                .BookFeesMinimum = bookfeesmin
                .BookFeesAccessorialCode = AccessorialCode
                .BookFeesAccessorialFeeTypeControl = 3
                .BookFeesVariableCode = acc.AccessorialVariableCode
                .BookFeesVisible = 1
                .BookFeesCaption = acc.AccessorialCaption
                .BookFeesTaxable = 1
                .BookFeesAccessorialFeeAllocationTypeControl = 4
                .BookFeesTarBracketTypeControl = 4
                .BookFeesAccessorialFeeCalcTypeControl = 2
                .BookFeesModDate = Date.Now
                .BookFeesModUser = Parameters.UserName
            End With
            oBookFees.Add(bookFee)
        End If

        Dim lh = BidTotalCost - BidFuelTotal

        Dim params As New DTO.SpotRate
        With params
            .BookRevs = oBookRevs.ToList()
            .allocationFormula = allocationFormula
            .totalLineHaulCost = lh
            .BookFees = oBookFees
            .DeleteTariffFees = oSpotRates(0).BookSpotRateDeleteTariffFees
            .DeleteLaneFees = oSpotRates(0).BookSpotRateDeleteLaneFees
            .DeleteOrderFees = oSpotRates(0).BookSpotRateDeleteOrderFees
            .UseCarrierFuelAddendum = oSpotRates(0).BookSpotRateUserCarrierFuelAddendum
            .CarrierControl = BidCarrierControl
            '.State = Nothing
            '.EffectiveDate = Nothing
            '.AvgFuelPrice = Nothing
            .AutoCalculateBFC = oSpotRates(0).BookSpotRateAutoCalculateBFC
            .TotalBFC = oSpotRates(0).BookSpotRateTotalBFC
            .AllocationBFCFormula = palletBracket
            .BookRevNegRevenueValue = 1
        End With

        'get the carrier contact
        Dim CarrContact = NGLCarrierContData.GetDefaultContactForCarrier(BidCarrierControl, CarrierContControl) 'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch -- added CarrierContControl parameter to method and passed it in as optional param to GetDefaultContactForCarrier()

        Dim res = oBookRevBLL.DoSpotRateSave(params, CarrContact)
        If res Is Nothing Then
            'this should never happen in production
            owcfRes.AddMessage(DTO.WCFResults.MessageType.Warnings, "E_SaveRateFailure", {LoadTenderControl.ToString(), "Cannot Dispatch Spot Rate, please check your order and try again."})
            SaveAppError(String.Format("Save Rate Failure for LoadTenderControl = {0}: {1}.", LoadTenderControl.ToString(), "Cannot Dispatch Spot Rate, please check your order and try again."))
            owcfRes.Success = False
            Return owcfRes
        End If
        If Not res.Log Is Nothing AndAlso res.Log.Count > 0 Then owcfRes.Log.AddRange(res.Log)
        If Not res.Messages Is Nothing AndAlso res.Messages.Count > 0 Then owcfRes.AddRangeToDictionary(DTO.WCFResults.MessageType.Messages, res.Messages)
        If res.CarriersByCost Is Nothing OrElse res.CarriersByCost.Count < 1 Then
            'Modified By LVV on 7/2/19 for v-8.2 - Fixed it so it would allow assigning a Zero Cost Carrier via Spot Rate
            'Verify that the user actually entered in 0 for the LineHaul, otherwise return the error that says no carriers are available
            If oSpotRates(0).BookSpotRateTotalLineHaulCost > 0 Then
                owcfRes.Success = False
                Return owcfRes
            End If
            'owcfRes.Success = False
            'Return owcfRes
        End If
        If res.Success = False Then
            owcfRes.Success = False
            Return owcfRes
        End If

        Dim sNewTranCode As String = "PC"
        Dim iOverrideSendLoadTenderEmail = 0
        If Not oDispatch Is Nothing Then
            If oDispatch.AutoAcceptOnDispatch = True Then sNewTranCode = "PB"
            If oDispatch.EmailLoadTenderSheet = True Then
                iOverrideSendLoadTenderEmail = 1
            Else
                iOverrideSendLoadTenderEmail = -1
            End If
        End If

        oRet = oBookBLL.ProcessNewTransCode(BookControl, sNewTranCode, sOldTrancode, LTTypeEnum.SpotRate, iOverrideSendLoadTenderEmail)
        If Not owcfRes.Errors Is Nothing AndAlso owcfRes.Errors.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Errors, owcfRes.Errors)
        If Not owcfRes.Messages Is Nothing AndAlso owcfRes.Errors.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Messages, owcfRes.Messages)
        If Not owcfRes.Warnings Is Nothing AndAlso owcfRes.Warnings.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, owcfRes.Warnings)
        If Not owcfRes.Log Is Nothing AndAlso owcfRes.Log.Count > 0 Then oRet.Log.AddRange(owcfRes.Log)
        'update bid records
        owcfRes = NGLLoadTenderData.AcceptNextStopBid(LoadTenderControl, BidControl)
        If Not owcfRes.Errors Is Nothing AndAlso owcfRes.Errors.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Errors, owcfRes.Errors)
        If Not owcfRes.Messages Is Nothing AndAlso owcfRes.Errors.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Messages, owcfRes.Messages)
        If Not owcfRes.Warnings Is Nothing AndAlso owcfRes.Warnings.Count > 0 Then oRet.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, owcfRes.Warnings)
        If Not owcfRes.Log Is Nothing AndAlso owcfRes.Log.Count > 0 Then oRet.Log.AddRange(owcfRes.Log)

        Return oRet
    End Function

#Region "changes added for Auto Assign Carrier for v-8.5.3.001 on 06/03/2022"

    ''' <summary>
    ''' Assign the selected API carrier from the bid to the shipment during silent tendering.
    ''' Use NSNGLTariffAssignSilent for Tariffs
    ''' Use NSP44AssignSilent for P44 API
    ''' </summary>
    ''' <param name="iLoadTenderControl"></param>
    ''' <param name="BookControl"></param>
    ''' <param name="oPrefBid"></param>
    ''' <param name="CarrierContControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created for v-8.5.3.001 on 6/10/2022
    '''     new logic to assign API carrier and fees (set to P) on new orders
    '''     the caller must check if the load should auto tender and/or auto accept the shipment
    '''     The API must map valid EDI Codes to the tblBidCostAdj.BidCostAdjDescCode 
    '''     Fuel is extracted seperately for each API so the EDI Code for Fuel is ignored and not required
    '''     API rates are inserted exactly like a TMS spot rate except we use the Carrier Name and "API Rate" 
    '''     as the tariff name.
    '''     TODO: check how the Service Name is mapped     
    ''' </remarks>
    Public Function NSAPIAssignSilent(ByVal iLoadTenderControl As Integer,
                                ByVal BookControl As Integer,
                                ByVal sOrderNumber As String,
                                ByVal oPrefBid As clsPreferedDefaultCarrier,
                                ByVal CarrierContControl As Integer) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim oRet As New DTO.WCFResults()
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim sOldTrancode As String = "N"  'N is default
        Dim sBidReference = String.Format("Order Number: {0}", sOrderNumber)
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        oRet.updateKeyFields("BookControl", BookControl.ToString())
        If oPrefBid Is Nothing OrElse oPrefBid.SelectedBid Is Nothing OrElse oPrefBid.SelectedBid.BidControl = 0 Then
            oRet.Success = False
            oRet.AddLog("API assign carrier silent failed because no quote for " & sBidReference)
            Return oRet
        End If
        sBidReference = String.Format("Order Number: {0}, Carrier Name: {1}", sOrderNumber, oPrefBid.SelectedBid.BidCarrierName)
        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            oRet.Success = False
            oRet.AddLog("API assign carrier silent failed because no booking orders found for " & sBidReference)
            Return oRet
        End If
        oRet.updateKeyFields("BidControl", oPrefBid.SelectedBid.BidControl.ToString())
        Dim BidControl As Integer
        Dim BidCarrierControl As Integer
        Dim BidLineHaul As Decimal
        Dim BidFuelUOM As String
        Dim BidFuelVariable As Decimal
        Dim BidFuelTotal As Decimal
        Dim BidTotalCost As Decimal
        BidControl = oPrefBid.SelectedBid.BidControl
        BidCarrierControl = oPrefBid.SelectedBid.BidCarrierControl
        BidLineHaul = oPrefBid.SelectedBid.BidLineHaul
        BidFuelUOM = oPrefBid.SelectedBid.BidFuelUOM
        BidFuelVariable = oPrefBid.SelectedBid.BidFuelVariable
        BidFuelTotal = oPrefBid.SelectedBid.BidFuelTotal
        BidTotalCost = oPrefBid.SelectedBid.BidTotalCost
        Dim source = "NGL.FM.BLL.NGLDATBLL.NSAPIAssignSilent"

        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'For Silent processing we cannot move forward this load has been posted to a Load Board for bids
            'this should not happen on New Orders
            oRet.Success = False
            oRet.AddLog("API assign carrier silent failed because the booking has been posted to DAT or NEXTStop.")
            oRet.AddLog("Manual assignment to the carrier is required for " & sBidReference)
            Return oRet
        End If
        Dim oBid = oPrefBid.SelectedBid
        Dim oDispatch = NGLLoadTenderData.getBidToDispatch(oPrefBid.SelectedBid.BidControl)
        updateAPIBookFromBid(oBookRevs, oBid, oDispatch)

        Dim bookFeesVar As Decimal = BidFuelVariable
        Dim bookfeesmin As Decimal = 0
        Dim bookFee As DTO.BookFee = CreateAPIFuelFee(BookControl, bookFeesVar, bookfeesmin, BidFuelUOM, BidFuelTotal)

        Dim totalbfc = oBookRevs(0).BookSpotRateTotalUnallocatedBFC
        If totalbfc = Nothing Then totalbfc = 0
        Dim bookfees As New List(Of DTO.BookFee)
        If Not bookFee Is Nothing Then bookfees.Add(bookFee)
        Dim dTotalOtherCosts As Decimal = ProcessAPIOtherCosts(BookControl, BidControl, oDispatch.Accessorials, bookfees)

        Dim lh = BidTotalCost - BidFuelTotal - dTotalOtherCosts
        oDispatch.LineHaul = lh
        Dim oResult As DTO.CarrierCostResults = saveAPIBidasSpotRate(oBookRevBLL, oBookRevs, bookfees, lh, totalbfc, BidCarrierControl, CarrierContControl)

        Dim sNewTranCode As String = "P"
        Dim res = oBookBLL.ProcessNewTransCode(BookControl, sNewTranCode, sOldTrancode, LTTypeEnum.LoadBoard, -1)
        If Not oRet.Warnings Is Nothing AndAlso oRet.Warnings.Count > 0 Then
            res.AddRangeToDictionary(DTO.WCFResults.MessageType.Warnings, oRet.Warnings)
        End If
        'update bid records
        NGLLoadTenderData.AcceptNextStopBid(iLoadTenderControl, BidControl)

        Return res
    End Function

    Public Function NSAPIUpdateLineHaul(ByVal iLoadTenderControl As Integer,
                                ByVal BookControl As Integer,
                                ByVal sOrderNumber As String,
                                ByVal oPrefBid As clsPreferedDefaultCarrier,
                                ByVal CarrierContControl As Integer) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim oRet As New DTO.WCFResults()
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim sOldTrancode As String = "N"  'N is default
        Dim sBidReference = String.Format("Order Number: {0}", sOrderNumber)
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        oRet.updateKeyFields("BookControl", BookControl.ToString())
        If oPrefBid Is Nothing OrElse oPrefBid.SelectedBid Is Nothing OrElse oPrefBid.SelectedBid.BidControl = 0 Then
            oRet.Success = False
            oRet.AddLog("API update carrier line haul failed because no quote for " & sBidReference)
            Return oRet
        End If
        sBidReference = String.Format("Order Number: {0}, Carrier Name: {1}", sOrderNumber, oPrefBid.SelectedBid.BidCarrierName)
        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            oRet.Success = False
            oRet.AddLog("API update carrier line haul failed because no booking orders found for " & sBidReference)
            Return oRet
        End If
        oRet.updateKeyFields("BidControl", oPrefBid.SelectedBid.BidControl.ToString())
        Dim BidControl As Integer
        Dim BidCarrierControl As Integer
        Dim BidLineHaul As Decimal
        Dim BidFuelUOM As String
        Dim BidFuelVariable As Decimal
        Dim BidFuelTotal As Decimal
        Dim BidTotalCost As Decimal
        BidControl = oPrefBid.SelectedBid.BidControl
        BidCarrierControl = oPrefBid.SelectedBid.BidCarrierControl
        BidLineHaul = oPrefBid.SelectedBid.BidLineHaul
        BidFuelUOM = oPrefBid.SelectedBid.BidFuelUOM
        BidFuelVariable = oPrefBid.SelectedBid.BidFuelVariable
        BidFuelTotal = oPrefBid.SelectedBid.BidFuelTotal
        BidTotalCost = oPrefBid.SelectedBid.BidTotalCost
        Dim source = "NGL.FM.BLL.NGLDATBLL.NSAPIUpdateLineHaul"

        Dim oBid = oPrefBid.SelectedBid
        Dim oDispatch = NGLLoadTenderData.getBidToDispatch(oPrefBid.SelectedBid.BidControl)
        updateAPIBookFromBid(oBookRevs, oBid, oDispatch)

        Dim bookFeesVar As Decimal = BidFuelVariable
        Dim bookfeesmin As Decimal = 0
        Dim bookFee As DTO.BookFee = CreateAPIFuelFee(BookControl, bookFeesVar, bookfeesmin, BidFuelUOM, BidFuelTotal)

        Dim totalbfc = oBookRevs(0).BookSpotRateTotalUnallocatedBFC
        If totalbfc = Nothing Then totalbfc = 0
        Dim bookfees As New List(Of DTO.BookFee)
        If Not bookFee Is Nothing Then bookfees.Add(bookFee)
        Dim dTotalOtherCosts As Decimal = ProcessAPIOtherCosts(BookControl, BidControl, oDispatch.Accessorials, bookfees)

        Dim lh = BidTotalCost - BidFuelTotal - dTotalOtherCosts
        oDispatch.LineHaul = lh
        Dim oResult As DTO.CarrierCostResults = saveAPIBidasSpotRate(oBookRevBLL, oBookRevs, bookfees, lh, totalbfc, BidCarrierControl, CarrierContControl)
        If Not oResult Is Nothing OrElse oResult.Success = False Then
            oRet.Success = False
            oRet.Messages = oResult.Messages
        End If

        Return oRet
    End Function


    Public Function NSNGLTariffAssignSilent(ByVal iLoadTenderControl As Integer,
                                      ByVal BookControl As Integer,
                                      ByVal sOrderNumber As String,
                                      ByVal SelectedCarrier As DTO.CarriersByCost,
                                      ByVal CarrierContControl As Integer) As DTO.WCFResults
        Dim oBookRevs As DTO.BookRevenue()
        Dim oRet As New DTO.WCFResults
        Dim oBookBLL As New BLL.NGLBookBLL(Parameters)
        Dim oBookRevBLL As New BLL.NGLBookRevenueBLL(Parameters)
        Dim oBrack As New DAL.NGLtblTarBracketTypeData(Parameters)
        Dim sBidReference = String.Format("Order Number: {0}, Carrier Name: {1}", sOrderNumber, SelectedCarrier.CarrierName)
        Dim source = "NGL.FM.BLL.NGLDATBLL.NSNGLTariffAssign"
        oRet.updateKeyFields("LoadTenderControl", iLoadTenderControl.ToString())
        oRet.updateKeyFields("BookControl", BookControl.ToString())
        oBookRevs = oBookRevBLL.GetBookRevenuesWDetailsFiltered(BookControl)
        If oBookRevs Is Nothing OrElse oBookRevs.Count < 1 Then
            oRet.Success = False
            oRet.AddLog("Tariff assign carrier silent failed because no booking orders found for " & sBidReference)
            Return oRet
        End If
        Dim bwBRLTSC As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderStatusCode)
        Dim bwBRLTType As New BitwiseFlags32(oBookRevs(0).BookRevLoadTenderTypeControl)

        'Check for active DAT Posting
        If HasActiveLBPosting(LTTypeEnum.DAT, bwBRLTType, bwBRLTSC) Then
            'For Silent processing we cannot move forward this load has been posted to a Load Board for bids
            'this should not happen on New Orders
            oRet.Success = False
            oRet.AddLog("Tariff assign carrier silent failed because the booking has been posted to DAT or NEXTStop.")
            oRet.AddLog("Manual assignment to the carrier is required for " & sBidReference)
            Return oRet
        End If
        'get the carrier contact
        Dim CarrContact = NGLCarrierContData.GetDefaultContactForCarrier(SelectedCarrier.CarrierControl, CarrierContControl) 'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch -- added CarrierContControl parameter to method and passed it in as optional param to GetDefaultContactForCarrier()
        'use the tariff engine to cost out the load
        Dim oCarrierCostResults As DTO.CarrierCostResults = oBookRevBLL.updateSelectedCarrier(BookControl, SelectedCarrier, CarrContact, Nothing)
        If Not oRet.updateMessagesAndLogsWithCarrierCostResults(oCarrierCostResults, sOrderNumber) Then
            oRet.Success = False
        End If
        Return oRet
    End Function


    Public Function CreateAPIFuelFee(ByVal BookControl As Integer, ByVal bookFeesVar As Decimal, ByVal bookfeesmin As Decimal, ByVal BidFuelUOM As String, ByVal BidFuelTotal As Decimal) As DTO.BookFee
        Dim bookFee As New DTO.BookFee()
        Try
            Dim AccessorialCode As Integer
            Select Case BidFuelUOM
                Case "Rate Per Mile"
                    AccessorialCode = 9
                Case "Percentage"
                    AccessorialCode = 2
                Case Else
                    AccessorialCode = 15
                    bookfeesmin = BidFuelTotal
                    bookFeesVar = 0
            End Select
            Dim acc = NGLtblAccessorialData.GettblAccessorialFiltered(AccessorialCode)

            With bookFee
                .BookFeesBookControl = BookControl
                .BookFeesVariable = bookFeesVar
                .BookFeesMinimum = bookfeesmin
                .BookFeesAccessorialCode = AccessorialCode
                .BookFeesAccessorialFeeTypeControl = 3 'Order specific fee
                .BookFeesVariableCode = acc.AccessorialVariableCode
                .BookFeesVisible = 1
                .BookFeesCaption = acc.AccessorialCaption
                .BookFeesTaxable = acc.AccessorialTaxable
                .BookFeesIsTax = acc.AccessorialIsTax
                .BookFeesAccessorialFeeAllocationTypeControl = acc.AccessorialAccessorialFeeAllocationTypeControl
                .BookFeesTarBracketTypeControl = acc.AccessorialTarBracketTypeControl
                .BookFeesAccessorialFeeCalcTypeControl = acc.AccessorialAccessorialFeeCalcTypeControl
                .BookFeesEDICode = acc.AccessorialEDICode 'Modified by RHR for v-8.2.1 on 11/07/2019 we now make sure the EDI Code is inserted
                .BookFeesModDate = Date.Now
                .BookFeesModUser = Parameters.UserName
            End With

        Catch ex As Exception
            'do nothing
        End Try
        Return bookFee

    End Function

    ''' <summary>
    ''' Update the BookRev data with selected bid
    ''' </summary>
    ''' <param name="oBookRevs"></param>
    ''' <param name="oBid"></param>
    ''' <param name="oDispatch"></param>
    ''' <param name="blnUseDispatchDatesIfDifferent"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.3.006 on 11/29/2022 new logic to update the booking transit time in hours
    '''   In this version we are not expecting multi-pick or multi-stop loads for LTL so transit time is 
    '''   always the same for all orders on the load.  When calculating Must Leave By date TMS will use the 
    '''   Transit time from the order with the highest miles.
    '''   The new CarrTarEstimatedDates.CalculateMustLeaveByDate is used to get the Must Leave By date
    '''   The Must Leave By date will then be used to adjust the actual ship date or reqired date based
    '''   on Lane Settings -- After the do spot rate method is called.
    ''' Modified by RHR for v-8.5.2.003 on 08/14/2023 
    '''     added logic to include Rate It shipment id when needed
    '''     Added logic to get the next consolidation number when needed
    ''' </remarks>
    Public Sub updateAPIBookFromBid(ByRef oBookRevs As DTO.BookRevenue(), ByVal oBid As LTS.tblBid, ByVal oDispatch As DAL.Models.Dispatch, Optional ByVal blnUseDispatchDatesIfDifferent As Boolean = False)
        Dim blnUseCarrierProAsSHID As Boolean = If(oDispatch Is Nothing, False, If(String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber), False, If(oDispatch.CarrierProNumber.Trim.Length > 0, True, False)))
        '        Dim LaneTLTBenchmark As Integer? = getScalarInteger("Select LaneTLTBenchmark From dbo.Lane Where LaneControl = " & LaneControl.ToString()) 'Is used As a Default transit time For this lane based On statistical analysis
        'CalculateAPITenderTransitTimes
        ' calculate the Tender Transit time for the load
        Dim BookTotalWgt As Double? = oBookRevs.Sum(Function(x) x.BookTotalWgt)
        Dim NbrMiles As Double? = oBookRevs.Sum(Function(x) x.BookMilesFrom)
        Dim CompControl As Integer = oBookRevs(0).BookCustCompControl
        Dim LaneControl As Integer = oBookRevs(0).BookODControl
        ' in version 8.5.3.006 we assume one Lane so use the first lane number to get the lane transit time calculated from statistical analysis
        ' Modified by RHR for v-8.5.4.006 on 04/18/2024 added isnull to sql to prevent sql exception
        Dim LaneTLTBenchmark As Integer? = NGLBookRevenueData.getScalarInteger("Select  isnull(LaneTLTBenchmark,0) From dbo.Lane Where LaneControl = " & LaneControl.ToString()) 'Is used As a Default transit time For this lane based On statistical analysis
        If LaneTLTBenchmark.HasValue = False Then LaneTLTBenchmark = 0
        Dim BookLeadTimeAutomationDaysByMile As Integer? = oBookRevs(0).BookLeadTimeAutomationDaysByMile
        Dim BookLeadTimeLTLMinimum As Integer? = oBookRevs(0).BookLeadTimeLTLMinimum
        'BookLeadTimeAutomationDaysByMile and BookLeadTimeLTLMinimum are passed by ref
        'if null or zero values will be updated from parameter table
        Dim iTranHours As Integer = NGLBookRevenueData.CalculateTenderTransitTimes(BookLeadTimeAutomationDaysByMile,
                                                   BookLeadTimeLTLMinimum,
                                                   BookTotalWgt,
                                                   NbrMiles,
                                                   CompControl,
                                                   LaneControl,
                                                   LaneTLTBenchmark,
                                                   oBid.BidTransitTime)
        For Each b In oBookRevs
            'we must reset book data to N status before generating a spot rate using API Data.
            Dim tmpSHID = b.BookSHID 'save the shid as this was used on the dispatch to the carrier
            b.ResetToNStatus()
            b.BookSHID = tmpSHID 'put it back
            'save any changes to the parameter data
            b.BookLeadTimeAutomationDaysByMile = BookLeadTimeAutomationDaysByMile
            b.BookLeadTimeLTLMinimum = BookLeadTimeLTLMinimum
            'the code below only works when users make changes in the dispatch sheet UI not in AssignSilent
            If blnUseDispatchDatesIfDifferent Then
                If Not oDispatch Is Nothing Then
                    If b.BookDateLoad <> oDispatch.PickupDate Then b.BookDateLoad = oDispatch.PickupDate
                    If b.BookDateRequired <> oDispatch.DeliveryDate Then b.BookDateRequired = oDispatch.DeliveryDate
                End If
            End If

            b.BookRevLoadTenderStatusCode = UpdateBookRevLoadTenderStatusCode(b.BookRevLoadTenderStatusCode, LTSCEnum.Pending)
            'Modified by RHR for v-8.5.2.003 on 08/14/2023 added logic to include Rate It shipment id when needed
            'Added logic to get the next consolidation number when needed
            If Not String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber) Then
                b.BookShipCarrierProNumber = oDispatch.CarrierProNumber
                b.BookShipCarrierProNumberRaw = oDispatch.CarrierProNumber
            End If
            If blnUseCarrierProAsSHID AndAlso Not String.IsNullOrWhiteSpace(oDispatch.CarrierProNumber) Then
                b.BookSHID = oDispatch.CarrierProNumber
            ElseIf String.IsNullOrWhiteSpace(b.BookSHID) Then
                If Not (String.IsNullOrWhiteSpace(oDispatch.SHID)) Then
                    b.BookSHID = oDispatch.SHID
                End If
            End If
            If String.IsNullOrWhiteSpace(b.BookConsPrefix) Then
                If Not (String.IsNullOrWhiteSpace(oDispatch.SHID)) Then
                    b.BookConsPrefix = oDispatch.SHID
                Else
                    b.BookConsPrefix = NGLBookData.GetNextCNSNumber(b.BookCustCompControl)
                End If
            End If
            If String.IsNullOrWhiteSpace(b.BookSHID) Then
                b.BookSHID = b.BookConsPrefix
            End If
            b.BookCarrTarName = "API RATE"
            b.BookCarrTarEquipName = "DRY53"
            If Not oBid Is Nothing AndAlso oBid.BidControl <> 0 Then
                b.BookShipCarrierName = oBid.BidCarrierName
                b.BookShipCarrierNumber = oBid.BidCarrierSCAC
                ' Begin Modified by RHR for v-8.5.3.005 on 08/30/2022 fixed bid type enum mapping issue
                Dim eTypeCode As NGL.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum = BidTypeEnum.None
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
                [Enum].TryParse(oBid.BidBidTypeControl, eTypeCode)
                b.BookCarrActualService = oBid.BidBidTypeControl.ToString()
                Dim eStatusCodeEnum = DTO.tblLoadTender.GetAPICarrierServiceReference(eTypeCode, oBid.BidServiceType)
                b.BookTypeCode = eStatusCodeEnum.ToString()
                b.BookCarrTarEquipName = DTO.tblLoadTender.GetAPIStatusCodeName(eStatusCodeEnum)
                ' Begin Modified by RHR for v-8.5.3.006 on 11/29/2022 added lead time
                ' Note: Logic is missing to perform Lead Time adjustments based on stops, hours or service, 
                'here we update the transit time using the bid transit time vs the time calculation
                'NGLBookRevenueData.CalculateAPITenderTransitTimes(b,oBid.BidTransitTime)
                b.BookCarrTransitTime = iTranHours 'note we need to ensure the transit time is populated
                'later, after we save the spot rate/API rate we get the must leave by date then we update the
                'ship date / required date based on the must leave by logic.
                'This will give us the correct dates for the tender to the carrier.
                ' End Modified by RHR for v-8.5.3.006 on 11/29/2022 added lead time
            End If
            ' End Modified by RHR for v-8.5.3.005 on 08/30/2022 fixed bid type enum mapping issue
        Next
    End Sub

    Public Function ProcessAPIOtherCosts(ByVal iBookControl As Integer, ByVal iBidControl As Integer, ByVal lAccessorials As String(), ByRef bookfees As List(Of DTO.BookFee)) As Decimal
        Dim dTotalOtherCosts As Decimal = 0
        If Not lAccessorials Is Nothing AndAlso lAccessorials.Count() > 0 Then
            'get the bid cost adjustments data 
            Dim oBidAdjs() As LTS.tblBidCostAdj
            Try
                oBidAdjs = NGLBidData.GetBidCostAdjustments(iBidControl)
            Catch ex As Exception
                Logger.Error(ex, "Exception getting NGLBidData.GetBidCostAdjustments")
                'ignore any errors
            End Try

            For Each sFee In lAccessorials
                'Modified by RHR for v-8.2.1 on 11/07/2019
                ' we now ignore zero cost accessorials when dispatching loads via API
                ' Previously all fees were set to a default of $1.00 so they would show 
                ' on the BOL.  This causes issues with the Freight Bill Audit.
                ' User must use the pickup or delivery notes for information about appointments or other special instructions.
                '  Approved by John R. 
                ' removed RHR -- Dim dFee As Decimal = 1
                Dim dFee As Decimal = 0
                If Not oBidAdjs Is Nothing AndAlso oBidAdjs.Any(Function(a) a.BidCostAdjDescCode = sFee) Then
                    Logger.Information("oBidAdj nothing and Any Code = {sFee}")
                    dFee = oBidAdjs.Where(Function(a) a.BidCostAdjDescCode = sFee).Select(Function(b) b.BidCostAdjAmount).FirstOrDefault()
                    ' removed RHR -- If dFee <= 0 Then dFee = 1
                End If
                ' Added by RHR 11/07/2019: we now ignore zero costs 
                If dFee > 0 Then
                    'add to total costs
                    dTotalOtherCosts += dFee
                    'get the accessorial data
                    Dim oAccData = NGLtblAccessorialData.GetAccessorialForNACCode(sFee)
                    'add to book fees
                    Logger.Information("Adding Accessorial Fee: " & sFee & " with cost of " & dFee)
                    Logger.Information("oAccData Found: {oAccData}", oAccData)
                    bookfees.Add(New DTO.BookFee() With {
                        .BookFeesBookControl = iBookControl,
                        .BookFeesVariable = 0,
                        .BookFeesMinimum = dFee,
                        .BookFeesAccessorialCode = If(oAccData Is Nothing, 42, oAccData.AccessorialCode),
                        .BookFeesAccessorialFeeTypeControl = 3,
                        .BookFeesVariableCode = If(oAccData Is Nothing, 1, If(oAccData.AccessorialVariableCode, 1)),
                        .BookFeesVisible = If(oAccData Is Nothing, True, oAccData.AccessorialVisible),
                        .BookFeesCaption = If(oAccData Is Nothing, If(String.IsNullOrWhiteSpace(oAccData.AccessorialCaption), "MSC", oAccData.AccessorialCaption), If(String.IsNullOrWhiteSpace(oAccData.AccessorialCaption), If(String.IsNullOrWhiteSpace(oAccData.AccessorialCaption), "MSC", oAccData.AccessorialCaption), oAccData.AccessorialCaption)), 'Modified by RHR for v-8.2.1 on 11/07/2019 we now use the default caption for the accessorial code
                        .BookFeesTaxable = If(oAccData Is Nothing, True, oAccData.AccessorialTaxable),
                        .BookFeesAccessorialFeeAllocationTypeControl = If(oAccData Is Nothing, 4, oAccData.AccessorialAccessorialFeeAllocationTypeControl),
                        .BookFeesTarBracketTypeControl = If(oAccData Is Nothing, 4, oAccData.AccessorialTarBracketTypeControl),
                        .BookFeesAccessorialFeeCalcTypeControl = If(oAccData Is Nothing, 2, oAccData.AccessorialAccessorialFeeCalcTypeControl),
                        .BookFeesEDICode = oAccData.AccessorialEDICode, 'Modified by RHR for v-8.2.1 on 11/07/2019 we now make sure the EDI Code is inserted
                        .BookFeesModDate = Date.Now,
                        .BookFeesModUser = Parameters.UserName
                    })
                End If
            Next
        End If

        Return dTotalOtherCosts
    End Function

    Public Function saveAPIBidasSpotRate(ByRef oBookRevBLL As BLL.NGLBookRevenueBLL, ByRef oBookRevs As DTO.BookRevenue(), ByRef bookfees As List(Of DTO.BookFee), ByVal lh As Decimal, ByVal totalbfc As Decimal, ByVal BidCarrierControl As Integer, ByVal CarrierContControl As Integer) As DTO.CarrierCostResults
        Dim oBrack As New DAL.NGLtblTarBracketTypeData(Parameters)
        Dim allocationBFCFormula As New DTO.tblTarBracketType
        Dim allocationFormula = oBrack.GettblTarBracketTypeFiltered(4)

        Dim params As New DTO.SpotRate
        With params
            .BookRevs = oBookRevs.ToList()
            .allocationFormula = allocationFormula
            .totalLineHaulCost = lh
            .BookFees = bookfees
            .DeleteTariffFees = True
            .DeleteLaneFees = True
            .DeleteOrderFees = True
            .UseCarrierFuelAddendum = False
            .CarrierControl = BidCarrierControl
            '.State = Nothing
            '.EffectiveDate = Nothing
            '.AvgFuelPrice = Nothing
            .AutoCalculateBFC = True
            .TotalBFC = totalbfc
            .AllocationBFCFormula = allocationBFCFormula
            .BookRevNegRevenueValue = 1
            .LockAllCost = False
            .LockBFCCost = True
            .FromAPI = True
        End With

        'get the carrier contact
        Dim CarrContact = NGLCarrierContData.GetDefaultContactForCarrier(BidCarrierControl, CarrierContControl) 'Modified By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch -- added CarrierContControl parameter to method and passed it in as optional param to GetDefaultContactForCarrier()
        Dim oResults As DTO.CarrierCostResults = oBookRevBLL.DoSpotRateSave(params, CarrContact)
        If oResults.Success Then
            ' In v-8.5.3.006 the first lane value for LaneTransLeadTimeCalcType are used for all the orders on the load
            '   primarily because we are not supporting multi-stop APIs in this version. 
            '   in the future this may be modified to include multi-stop APIs
            'First get the Lane control from the booking
            Dim iLaneControl = oBookRevs(0).BookODControl
            'next get the LaneTransLeadTimeCalcType  this is used to modify the ship date or required date based on transit hours/days
            Dim iGetLaneTransLeadTimeCalcType = NGLLaneData.GetLaneTransLeadTimeCalcType(iLaneControl)
            ' now calculate the must leave by date using the HolidayMatrix model data
            Dim oHolidayM As New Models.HolidayMatrix()
            'load HolidayMatrix defaults
            Dim iComp As Integer = oBookRevs(0).BookCustCompControl
            Dim iCarrier As Integer = oBookRevs(0).BookCarrierControl
            Dim iTranHours = If(oBookRevs(0).BookCarrTransitTime, 24) ' set default to 1 day, previous methods must caclulate the transit time.
            Dim iTranDays = If(iTranHours < 24, 1, iTranHours / 24)
            Dim RequiredDate As Date? = oBookRevs.Max(Function(x) x.BookDateRequired)
            Dim ShipDate As Date? = oBookRevs.Min(Function(x) x.BookDateLoad)
            If Not ShipDate.HasValue Then ShipDate = Date.Now
            If Not RequiredDate.HasValue Then RequiredDate = ShipDate.Value.AddDays(5)
            Dim iYear As Integer = RequiredDate.Value.Year
            Dim iMonth As Integer = RequiredDate.Value.Month
            Dim oCompCal As New DAL.NGLCompCalData(Parameters)
            oHolidayM.LoadCompDatesList(iComp, oCompCal.GetCompCalsFiltered(iComp).ToList(), iYear)
            Dim oCompLoadWeekends As Models.DriveDays = NGLCompData.GetCompWeekendLoadSettings(iComp)
            If If(oCompLoadWeekends.DriveSat, False) = False Then oHolidayM.CompClosedOnSaturday(iMonth, iYear)
            If If(oCompLoadWeekends.DriveSun, False) = False Then oHolidayM.CompClosedOnSunday(iMonth, iYear)
            Dim oCarCal As New DAL.NGLCarrierNoDriveDaysData(Parameters)
            oHolidayM.LoadCarrierNoDriveDays(iCarrier, oCarCal.GetCarrierNoDriveDays(iCarrier), iYear)
            Dim oCarDriveWeekends As Models.DriveDays = NGLLegalEntityCarrierData.GetLECarrierWeekendDriveSettings(iCarrier, iComp)
            oHolidayM.DriveSaturday = If(oCarDriveWeekends.DriveSat, False)
            oHolidayM.DriveSunday = If(oCarDriveWeekends.DriveSun, False)
            'TODO: use LaneSDFEarlyTM6 and LaneSDFEarlyTM7 to determine if we can pick up or deliver to lane on sat or sunday,  zero indicates closed
            Dim MustLeaveByDateTime As Date = NGL.FM.CarTar.CarrTarEstimatedDates.CalculateMustLeaveByDate(ShipDate, iTranDays, RequiredDate, oHolidayM)
            For Each b In oBookRevs
                b.BookMustLeaveByDateTime = MustLeaveByDateTime
                'Ari Requirement for TransitDay calculations. 8/19/2014.
                'If BookMustLeaveByDateTime date Is null set them to the BookDateLoad.
                If (b.BookMustLeaveByDateTime.HasValue = False) Then
                    b.BookMustLeaveByDateTime = b.BookDateLoad
                Else
                    ' If we Then are Not adjusting load Date because Of production lead time issues                            
                    'And the must leave by date Is before the current ship date
                    'And BookMustLeaveByDateTime Is greater than today
                    'And the lane Is configured to update the ship date based on transit time
                    'We change the ship date to the Must Leave by Date And Time
                    ' rules
                    '1. if BookProductionLeadTimeApplied Is null update BookDateLoad
                    '2. if BookProductionLeadTimeApplied Is Not null And Is false update BookDateLoad
                    '3. if BookProductionLeadTimeApplied Is true do Not update BookDateLoad
                    ' result:  If Nullable BookProductionLeadTimeApplied Not true Then  update BookDateLoad
                    If (Not (If(b.BookProductionLeadTimeApplied, False))) Then
                        If (b.BookMustLeaveByDateTime < b.BookDateLoad AndAlso b.BookMustLeaveByDateTime > Date.Now) Then
                            If (iGetLaneTransLeadTimeCalcType = 1) Then
                                b.BookDateLoad = b.BookMustLeaveByDateTime
                            End If
                        End If
                    End If
                End If
            Next
        End If

        '       @BookDateLoad = 
        'Case 
        '	When isnull(l.LaneTransLeadTimeCalcType,0) = 1 And isnull(l.LaneTLTBenchmark,0) > 0 And poh.POHDRReqDate Is Not null then
        '					Case 
        '						When cast(isnull(poh.POHDRWgt,0)as float) < 10001 And isnull(@BookLeadTimeLTLMinimum,0) > 0 And @BookLeadTimeLTLMinimum > isnull(LaneTLTBenchmark,0) then DateAdd(d,(@BookLeadTimeLTLMinimum * -1),poh.POHDRReqDate)
        '						Else DateAdd(d,(l.LaneTLTBenchmark * -1),poh.POHDRReqDate)
        '					End
        '       Else poh.POHDRShipdate	
        'End , 		--31  CalculateShipDate Using Lane If Configured Modified Ship Date By RHR For v-8.5.3.001 On 06/11/2022 updated logic For Transportation Lead Time adjustments
        '--@BookDateLoad = Case When isnull(l.LaneTransLeadTimeCalcType,0) = 1 And isnull(l.LaneTLTBenchmark,0) > 0 And poh.POHDRReqDate Is Not null then  DateAdd(d,(l.LaneTLTBenchmark * -1),poh.POHDRReqDate)
        '--	Else poh.POHDRShipdate	End , 		--31  CalculateShipDate using Lane if Configured Modified by RHR for v-8.4.0.003
        '@BookDateRequired = Case When isnull(l.LaneTransLeadTimeCalcType,0) = 2 And isnull(l.LaneTLTBenchmark,0) > 0 And poh.POHDRShipdate Is Not null then  DateAdd(d,l.LaneTLTBenchmark,poh.POHDRShipdate)
        '	Else poh.POHDRReqDate	End , 		--32  CalculateRequiredDate Using Lane If Configured Modified by RHR For v-8.4.0.003








        Return oResults
    End Function
#End Region

#End Region




End Class



