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

Public Class NGLLaneTransLoadXrefDetBLL : Inherits BLLBaseClass


#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLLaneTransLoadXrefDetBLL"
    End Sub

#End Region

#Region " Properties "

    Private _OriginalLaneControl As Integer = 0
    Public Property OriginalLaneControl() As Integer
        Get
            Return _OriginalLaneControl
        End Get
        Set(ByVal value As Integer)
            _OriginalLaneControl = value
        End Set
    End Property

    Private _CompControl As Integer = 0
    Public Property CompControl() As Integer
        Get
            Return _CompControl
        End Get
        Set(ByVal value As Integer)
            _CompControl = value
        End Set
    End Property

    Private _ThisOrderSequence As Integer = 0
    Public Property ThisOrderSequence() As Integer
        Get
            Return _ThisOrderSequence
        End Get
        Set(ByVal value As Integer)
            _ThisOrderSequence = value
        End Set
    End Property

    Private _RoutingIndex As Integer
    Public Property RoutingIndex() As Integer
        Get
            Return _RoutingIndex
        End Get
        Set(ByVal value As Integer)
            _RoutingIndex = value
        End Set
    End Property

    Private _CurrentCNS As String
    Public Property CurrentCNS() As String
        Get
            Return _CurrentCNS
        End Get
        Set(ByVal value As String)
            _CurrentCNS = value
        End Set
    End Property

    Private _BookItemsRouted As New List(Of NGLTransLoadEquipDataBLL)
    Public Property BookItemsRouted() As List(Of NGLTransLoadEquipDataBLL)
        Get
            If _BookItemsRouted Is Nothing Then _BookItemsRouted = New List(Of NGLTransLoadEquipDataBLL)
            Return _BookItemsRouted
        End Get
        Set(ByVal value As List(Of NGLTransLoadEquipDataBLL))
            _BookItemsRouted = value
        End Set
    End Property

    Private _MovementSplitSequenceNumber As Integer = 0
    Public Property MovementSplitSequenceNumber() As Integer
        Get
            Return _MovementSplitSequenceNumber
        End Get
        Set(ByVal value As Integer)
            _MovementSplitSequenceNumber = value
        End Set
    End Property


    Private _FirstJump As Boolean
    Public Property FirstJump() As Boolean
        Get
            Return _FirstJump
        End Get
        Set(ByVal value As Boolean)
            _FirstJump = value
        End Set
    End Property

    Private _NeedNewLaneTranXControl As Boolean = False
    ''' <summary>
    ''' Flag to identify when we need the Next LaneTransLoadXref record
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 10/18/2023 v-8.5.4.003
    ''' </remarks>
    Public Property NeedNewLaneTranXControl() As Boolean
        Get
            Return _NeedNewLaneTranXControl
        End Get
        Set(ByVal value As Boolean)
            _NeedNewLaneTranXControl = value
        End Set
    End Property

    Private _ActiveLaneTranXControl As Integer = 0
    ''' <summary>
    ''' The active Lane Tran Xref header control
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 10/18/2023 v-8.5.4.003
    ''' </remarks>
    Public Property ActiveLaneTranXControl() As Integer
        Get
            Return _ActiveLaneTranXControl
        End Get
        Set(ByVal value As Integer)
            _ActiveLaneTranXControl = value
        End Set
    End Property

    Private _ActiveLaneTranXSequence As Integer = 0
    ''' <summary>
    ''' The active Lane Tran Xref header sequence number
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR 10/18/2023 v-8.5.4.003
    ''' </remarks>
    Public Property ActiveLaneTranXSequence() As Integer
        Get
            Return _ActiveLaneTranXSequence
        End Get
        Set(ByVal value As Integer)
            _ActiveLaneTranXSequence = value
        End Set
    End Property

#End Region

#Region "DAL Wrapper Methods"

#End Region

#Region " Public Methods"

    ''' <summary>
    ''' Send specific trans load header record then process each child jump
    ''' To allow the system to auto select the first available trans load 
    ''' call processTransLoadFacility with just the bookcontrol number
    ''' </summary>
    ''' <param name="iLaneTranXControl"></param>
    ''' <param name="intBookControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.003 on 10/26/2023
    '''  New method to be called from UI when users selects a 
    '''  specific trans load header LaneTranXControl
    '''  supports options for different jumps for the same lane
    '''  Primary purpose is to have a different configuration determined by 
    '''  Carrier Selection.  In this version this opcion can only be performed manually
    ''' </remarks>
    Public Function processSelectedTransLoadConfiguration(ByVal iLaneTranXControl As Integer, ByVal intBookControl As Integer) As DTO.WCFResults
        Dim oRet As New DTO.WCFResults()
        oRet.Success = False
        Dim blnRet As Boolean = False
        If intBookControl = 0 Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
            Return oRet
        End If
        Dim oBook As DTO.Book = NGLBookData.GetRecordFiltered(intBookControl)
        If oBook Is Nothing OrElse oBook.BookControl = 0 Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
            Return oRet
        End If
        If Not String.IsNullOrEmpty(oBook.BookSHID) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_SystemWarning, New String() {"Cannot process Trans Load after an SHID has been assigned,  remove order from carrier and try again."})
            Return oRet
        End If
        Me.OriginalLaneControl = oBook.BookODControl
        CurrentCNS = oBook.BookConsPrefix
        CompControl = oBook.BookCustCompControl
        If String.IsNullOrEmpty(CurrentCNS) OrElse CurrentCNS.Trim.Length < 1 Then
            CurrentCNS = NGLBatchProcessData.GetNextConsNumber(CompControl)
            oBook.BookConsPrefix = CurrentCNS
            oBook.BookRouteConsFlag = True
            oBook.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
        End If

        'Dim iModeType As Integer = NGLLaneData.getLaneModeTypeControlforActiveTransLoad(oBook.BookODControl)
        'If (iModeType = 0) Then
        '    oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_SystemWarning, New String() {"Cannot process selected Trans Load configuration.  The feature is not active for Lane,  update the Lane settings and try again."})
        '    Return oRet
        'End If

        If (Not NGLLaneData.isTransLoadOn(oBook.BookODControl)) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_SystemWarning, New String() {"Cannot process selected Trans Load configuration.  The feature is not active for Lane,  update the Lane settings and try again."})
            Return oRet
        End If
        Dim oTransLoad As DTO.LaneTransLoadXref = NGLLaneTransLoadXrefData.GetLaneTransLoadXrefFilteredWDetails(iLaneTranXControl)
        If oTransLoad Is Nothing Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_SystemWarning, New String() {"Cannot process Trans Load, the seleted record is no longer availale,  refresh your page and try again."})
            Return oRet
        End If

        If (Not exceedsMinimum(oBook, oTransLoad) OrElse Not inferiorToMaximum(oBook, oTransLoad)) Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_SystemWarning, New String() {"Cannot process Trans Load, the booking is outside of capacity settings, update your Transload capacity or select a new configuration."})
            Return oRet
        End If

        Me.ActiveLaneTranXSequence = 0
        FirstJump = True
        Me.NeedNewLaneTranXControl = False
        Me.ActiveLaneTranXControl = oTransLoad.LaneTranXControl
        If processNextTransLoad(oBook, oTransLoad, 1, oRet) Then
            saveJumps(oBook)
            oRet.Success = True
        Else
            oRet.Success = False
        End If

        Return oRet
    End Function

    ''' <summary>
    ''' Attempts to process transload facilities.  returns false if no trans load is available or possible
    ''' </summary>
    ''' <param name="intBookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function processTransLoadFacility(ByVal intBookControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Dim oRet As New DTO.WCFResults()
        oRet.Success = False
        If intBookControl = 0 Then Return False
        Dim oBook As DTO.Book = NGLBookData.GetRecordFiltered(intBookControl)
        If oBook Is Nothing OrElse oBook.BookControl = 0 Then Return False
        'Code Change PFM 5/27/2015' if SHID is blank, continue with transload routing., if SHId is not blank, return false because the load has already been configured and previoulsy finalized.
        If Not String.IsNullOrEmpty(oBook.BookSHID) Then Return False
        Me.OriginalLaneControl = oBook.BookODControl
        CurrentCNS = oBook.BookConsPrefix
        CompControl = oBook.BookCustCompControl
        If String.IsNullOrEmpty(CurrentCNS) OrElse CurrentCNS.Trim.Length < 1 Then
            CurrentCNS = NGLBatchProcessData.GetNextConsNumber(CompControl)
            oBook.BookConsPrefix = CurrentCNS
            oBook.BookRouteConsFlag = True
            oBook.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
        End If
        Me.ActiveLaneTranXSequence = 0
        Dim oTransLoad As DTO.LaneTransLoadXref = getTransloadConfiguration(oBook, OriginalLaneControl)
        If oTransLoad Is Nothing OrElse oTransLoad.LaneTranXControl = 0 Then Return False
        FirstJump = True
        Me.NeedNewLaneTranXControl = False
        Me.ActiveLaneTranXControl = oTransLoad.LaneTranXControl
        If processNextTransLoad(oBook, oTransLoad, 1) Then
            saveJumps(oBook)
            Return True
        Else
            Return False
        End If
    End Function



    Public Sub Print()

        For Each r In BookItemsRouted
            Console.WriteLine("*******************************************************************************************************")
            Console.WriteLine("Capacity | Cases: {0} | Weight {1} | Pallets: {2} | Cubes: {3} |", r.CarrTarEquipCasesMaximum, r.CarrTarEquipWgtMaximum, r.CarrTarEquipPltsMaximum, r.CarrTarEquipCubesMaximum)
            Console.WriteLine("Used | Cases: {0} | Weight {1} | Pallets: {2} | Cubes: {3} |", r.TotalCases, r.TotalWgt, r.TotalPlts, r.TotalCubes)
            Console.WriteLine("Avail | Cases: {0} | Weight {1} | Pallets: {2} | Cubes: {3} |", r.AvailCases, r.AvailWgt, r.AvailPlts, r.AvailCubes)
            For Each i In r.BookItemsBLL.BookItems
                Console.WriteLine("       Item {4} | Cases: {0} | Weight {1} | Pallets: {2} | Cubes: {3} |", i.BookItemQtyOrdered, i.BookItemWeight, i.BookItemPallets, i.BookItemCube, i.BookItem.BookItemItemNumber)
            Next
            Console.WriteLine("*******************************************************************************************************")
            Console.WriteLine("")
        Next
    End Sub

#End Region

#Region " Private Methods"

    ''' <summary>
    ''' Selects Tariff if not assigned and Calculates costs then
    ''' attempts to auto tender as configured.
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <remarks>
    ''' Call AssignOrUpdateCarrier with current carrier assigned to book
    ''' then executes spAutoTenderLoadCurrentCarrier
    ''' Note: in the future we may want to find a way to process the messages returned from AssignOrUpdateCarrier
    ''' </remarks>
    Public Sub UpdateCarrier(ByVal BookControl As Integer)

        Try
            'AssignOrUpdateCarrier will select lowest cost carrier if carriercontrol is 0 
            'or will select the lowest cost tariff if the BookCarrTarEquipControl is 0 
            'or it will update the carrier cost using the currently assigned tariff
            If Not BookRevenueBLL.AssignOrUpdateCarrier(BookControl).Success Then Return
            'now run the auto tender routine for the current carrier selected
            Try
                NGLBookData.AutoTenderLoadCurrentCarrier(BookControl)
            Catch ex As FaultException
                'do nothing if stored procedure fails 
            Catch ex As Exception
                Throw
            End Try
        Catch ex As Exception
            logSystemError(ex, buildProcedureName("UpdateCarrier"), "Auto Tender Load Current Carrier For Transload Failure", FreightMaster.Data.sysErrorParameters.sysErrorState.SystemLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Critical)
        End Try
    End Sub

    ''' <summary>
    ''' The caller must capture the exceptions.  for testing we do not 
    ''' worry about transactions but before going to production we need
    ''' to implement a soluton see NOTE in remarks
    ''' </summary>
    ''' <param name="oBook"></param>
    ''' <remarks> 
    ''' NOTE: logic must be identified for how to handle a roll back 
    ''' If one update fails the update for the entire list should fail
    ''' We have 2 ways to do this  One is using TransactionScope() in linq to sql
    ''' the other is to wrap all saves into one method where we call 
    ''' SubmitChanges only once after all the other insertonsubmit is applied
    ''' Both require programming changes in the data class the second option
    ''' may be the best option.  We should only need to use TransactionScope
    ''' if data must be saved between database reads or if we want to prevent 
    ''' other users from seeing changes until the transaction is complete.
    ''' We must call transaction.Complete when using TransactionScope
    ''' </remarks>
    Private Function saveJumps(ByRef oBook As DTO.Book) As Boolean
        'TODO:  add error handler and transacton scope logic to this method to roll back on error.
        Dim blnRet As Boolean = True
        Dim strPrevCNS As String = ""
        For Each r In Me.BookItemsRouted.OrderBy(Function(x) x.MovementSplitSequenceNumber)
            If r.MovementSplitSequenceNumber = 0 Then
                If Not updateOriginalBookData(oBook, r) Then Return False
                If Not updateBookItems(oBook, r) Then Return False
                BookBLL.UpdateBookWithDetailsNoReturn(oBook)
                UpdateCarrier(oBook.BookControl)
            Else
                'TODO:? add key fields to equipment object: BookLaneTransXDetControl, BookConsPrefix and BookRouteConsFlag (not sure if this is done.  we need to test!!)
                'create a new split
                Dim oKeyResults = NGLBookData.SplitOrderForTransload(oBook.BookControl, r.TransLoadXrefDet.LaneTranXDetControl, r.BookConsPrefix, r.BookRouteConsFlag)
                'TODO:  process key results and errors
                'get the updated book data back
                Dim oSplitBook As DTO.Book = NGLBookData.GetRecordFiltered(oKeyResults.BookControl)
                If oSplitBook.BookConsPrefix = strPrevCNS Then
                    'this is a consolidated split so set miles to zero on subsequent splits (if we add logic to uncheck BookRouteConsFlag then we need to change this logic) 
                    oSplitBook.BookMilesFrom = 0

                End If
                If oSplitBook Is Nothing OrElse oSplitBook.BookControl = 0 Then Return False
                If Not updateBookItems(oSplitBook, r) Then Return False
                oSplitBook.TrackingState = Core.ChangeTracker.TrackingInfo.Updated 'always mark this as updated to force a save
                BookBLL.UpdateBookWithDetailsNoReturn(oSplitBook)
                UpdateCarrier(oSplitBook.BookControl)
            End If
            strPrevCNS = r.BookConsPrefix
        Next
        Return blnRet

    End Function

    ''' <summary>
    ''' adds items to one or more similar pieces of equipment.  Full pieces of
    ''' equipment are added to the routed collection but the original piece of 
    ''' equipment is not.  The caller must add the original piece of equipment 
    ''' to the routed collection if it contains items.
    ''' </summary>
    ''' <param name="oEquipmentBLL"></param>
    ''' <param name="oBookItemsBLL"></param>
    ''' <remarks></remarks>
    Public Sub routeOrders(ByRef oEquipmentBLL As NGLTransLoadEquipDataBLL, ByRef oBookItemsBLL As NGLTransLoadBookItemsBLL)
        If Not oEquipmentBLL.CanFit(oBookItemsBLL.TotalCases, oBookItemsBLL.TotalWgt, oBookItemsBLL.TotalPlts, oBookItemsBLL.TotalCubes) Then
            For Each i In oBookItemsBLL.BookItems.OrderByDescending(Function(x) x.BookItemWeight)
                fillEquipment(oEquipmentBLL, i)
            Next
        Else
            oEquipmentBLL.BookItemsBLL = oBookItemsBLL
        End If
    End Sub

    ''' <summary>
    ''' Adds item cases (Qty) to the configured equipment until the equipment is full or no Qty remain 
    ''' </summary>
    ''' <param name="oEquipToRoute"></param>
    ''' <param name="oItem"></param>
    ''' <remarks></remarks>
    Public Sub fillEquipment(ByRef oEquipToRoute As NGLTransLoadEquipDataBLL, ByRef oItem As NGLTransLoadBookItemDataBLL)
        Do While oItem.BookItemQtyOrdered > 0
            oItem = oEquipToRoute.addWithSplit(oItem.Clone())
            If Not oItem Is Nothing AndAlso oItem.BookItemQtyOrdered > 0 Then
                Dim newEquipment As NGLTransLoadEquipDataBLL = oEquipToRoute.Clone(False) 'create a full clone to remove any object reference to the routed equipment
                If newEquipment.TransLoadXrefDet.LaneTranXDetConsolidateSplits = False Then
                    'we need to get a new CNS number for each movement
                    newEquipment.BookRouteConsFlag = NGLBatchProcessData.GetNextConsNumber(CompControl)
                    CurrentCNS = newEquipment.BookRouteConsFlag
                End If
                AddEquipmentToRouted(newEquipment)
                oEquipToRoute.BookItemsBLL = Nothing 'remove all items because they were added in the previous equipment
            End If
        Loop
    End Sub

    ''' <summary>
    ''' updates the equipment's MovementSplitSequenceNumber
    ''' increases the MovementSplitSequenceNumber counter and
    ''' adds the equipment to the BookItemsRouted list.
    ''' </summary>
    ''' <param name="oEquipmentBLL"></param>
    ''' <remarks></remarks>
    Private Sub AddEquipmentToRouted(ByVal oEquipmentBLL As NGLTransLoadEquipDataBLL)
        'the equipment is full so add it to the routed collection and create a new piece of equipment
        'Note: the MovementSplitSequenceNumber is not implemented until we add the equipment to the 
        'routed collection
        oEquipmentBLL.MovementSplitSequenceNumber = MovementSplitSequenceNumber
        MovementSplitSequenceNumber += 1
        Me.BookItemsRouted.Add(oEquipmentBLL)
    End Sub

    ''' <summary>
    ''' Called recursively to process each transload movement
    ''' </summary>
    ''' <param name="oBook"></param>
    ''' <param name="oTransLoad"></param>
    ''' <param name="intDetailSequence"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Notes added by RHR 10/13/2023
    '''     BookControl is required
    '''     The DTO.LaneTransLoadXref contains a list of LaneTransLoadXrefDet populated by the caller
    '''     The intDetailSequence starts at 1 and is updated to the next value until
    '''     the max value for this list is reached
    '''     each time the value is increased the system calls processNextTransLoad(oBook, oTransLoad, intDetailSequence) recursively
    '''     only after all LaneTransLoadXrefDets are returned does the processing stop, returning true or false
    '''     the caller must call saveJumps on success
    ''' Transload changes by RHR 10/18/2023 v-8.5.4.003
    '''     Each Transload header has a sequence number And a list of jumps as details
    '''     Automation (see auto tender For Transload lanes) 
    '''         Select the first sequence number at the header level (1) And attempt To process the jumps
    '''         Add logic To the getTransloadConfiguration And/Or the processNextTransLoad To apply sequence number filters To the header record
    '''         If the jumps fail due To mode Or capacity we move To the Next header If available.
    '''         On success save the movements
    '''         If all headers fail we log the errors/messages To the App Error Table 
	'''         Two new identification columns are needed: 
    '''             Type with values like Warning, Message, Error 
    '''             And Source Like Lane Automation etc.
    '''     Manual 
    '''         selection will Not move To Next header if the processing fails 
	'''         we will log errors And messages
	'''         we will show popup errors And messages To the user via the client UI
	'''         Add logic To skip Get Next transload configuration For Manual selection.
    '''    Additional changes
    '''     Added WCFResults class to manage errors and messages
    '''     Added Flag to identify Manual Selection
    '''     Added a new property to the class for the active header control, ActiveLaneTranXControl
    '''     Added a new property to the class to identify when we need the Next LaneTransLoadXref record, NeedNewLaneTranXControl
    '''     Added a new property to the class for the active header sequence number, ActiveLaneTranXSequence
    '''     The caller of processNextTransLoad must set the correct values for the new properties
    '''         processNextTransLoad can also be the caller
    ''' </remarks>
    Private Function processNextTransLoad(ByVal oBook As DTO.Book, ByRef oTransLoad As DTO.LaneTransLoadXref, ByVal intDetailSequence As Integer, Optional ByRef oRet As DTO.WCFResults = Nothing) As Boolean

        If oRet Is Nothing Then
            oRet = New DTO.WCFResults()
            oRet.Success = False
        End If
        Dim blnRet As Boolean = True
        If oBook Is Nothing OrElse oBook.BookControl = 0 Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.M_NoOrdersFound)
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_SystemWarning, New String() {"Cannot process Trans Load details for " & oTransLoad.LaneTranXName & ".  The assigned booking does not exist,  please review your data and try again."})
            Return False
        End If
        'Note: it is expected that LaneTranXDetSequence will be unique and sequential 
        Dim oTransLoadXrefDet As DTO.LaneTransLoadXrefDet = oTransLoad.LaneTransLoadXrefDets.Where(Function(x) x.LaneTranXDetSequence = intDetailSequence).FirstOrDefault()
        If oTransLoadXrefDet Is Nothing OrElse oTransLoadXrefDet.LaneTranXDetControl = 0 Then
            oRet.AddMessage(DTO.WCFResults.MessageType.Warnings, DTO.WCFResults.MessageEnum.E_SystemWarning, New String() {"Cannot process Trans Load details for " & oTransLoad.LaneTranXName & ".  The jump at sequence # " & intDetailSequence.ToString() & " is not available,  please review your data and try again."})
            Return False
        End If

        Dim childTransLoads As DTO.LaneTransLoadXref = Nothing
        'check if this jump has child transload configurations and nested jumps
        ' but only if the detail record is for a different lane
        If (oTransLoad.LaneTranXLaneControl <> oTransLoadXrefDet.LaneTranXDetLaneControl) Then
            childTransLoads = getTransloadConfiguration(oBook, oTransLoadXrefDet.LaneTranXDetLaneControl)
        End If
        'Each lane associated with a transload movement can require additional movements 
        If Not childTransLoads Is Nothing AndAlso childTransLoads.LaneTranXControl <> 0 Then
            'process next trans load will loop through all the nested jumps and additional children
            'calling AddEquipmentToRouted or each jump.  if any jump fails to be added
            'to the equipment the entire process is canceled and  rolled back
            If Not processNextTransLoad(oBook, childTransLoads, 1, oRet) Then Return False
        Else
            'Note: we only process this jump when the detail does not have any child jumps
            'example: Green Bay to NY may stop in Chicago as the first Jump but the 
            '           however the trip to chicago can be one jump (processed here) or
            '           it could have child jumps (above) Greenbay to Kenosha, then Kenosha to Chicago
            '           We cannot go directly to Chicago when a Kenosha jump us requred
            '           so this part of the code is never processed when we have nested jumps
            If Not FirstJump OrElse String.IsNullOrEmpty(CurrentCNS) OrElse CurrentCNS.Trim.Length < 1 Then
                CurrentCNS = NGLBatchProcessData.GetNextConsNumber(CompControl)
            Else
                FirstJump = False
            End If
            '****************** this comment is out of date, pfm 5/27/2015 - skip routing if SHID has been assgined because it was previously finalized. ****************************************************************************************************
            'this code is outdated,  we now use SHID not CNS  logic must be replaces to reset the SHID
            'Each new piece of equipment created by processNextTransLoad will have a new CNS number 
            'the fillequipment called by RouteLoad will check the LaneTranXDetConsolidateSplits flag and determine 
            'if a new CNS is required for splits
            'Change code to update the SHID when we are ready
            'If intDetailSequence > 0 AndAlso Not canBillTogether(oTransLoad, oTransLoadXrefDet, intDetailSequence) Then CurrentCNS = ""
            '*****************************************************************************************************************************
            'We verify that a piece of equipment has been selected
            ' if no equipment we create a default piece of equipment and add all items to the equipment
            ' if equipment has been assigned we route orders for each bookload item; this will add a new 
            ' piece of equipment to the BookItemsRouted collection.  Each item in the BookItemsRouted collection
            ' may be sorted by a LaneTranXDetSequence or MovementSplitSequenceNumber, each MovementSplitSequenceNumber is unique 
            ' and assigned sequentially to each piece of equipment.  BookOrderSequence numbers 
            ' are assigned by the spSplitOrderForTransload procedure.  If the MovementSplitSequenceNumber is zero this is the first 
            ' movement so we do not call spSplitOrderForTransload; we simply update the item details to match the 
            ' routed amount and save the changes.  
            Dim oCarrEquip = NGLCarrTarEquipData.GetCarrTarEquipFiltered(oTransLoadXrefDet.LaneTranXDetCarrTarEquipControl)
            Dim oEquipmentBLL As New NGLTransLoadEquipDataBLL
            If oCarrEquip Is Nothing OrElse oCarrEquip.CarrTarEquipControl = 0 Then
                'equipment has not been assigned so create a default piece of equipment and add all items
                oEquipmentBLL.BookConsPrefix = CurrentCNS
                'TODO:  add logic to determine when BookRouteConsFlag should be false.  For now it is always true
                oEquipmentBLL.BookRouteConsFlag = True
                oEquipmentBLL.TransLoadXrefDet = oTransLoadXrefDet
                For Each bl In oBook.BookLoads
                    'add all of the item details
                    oEquipmentBLL.BookItemsBLL.populateItems(bl.BookItems)
                Next
                AddEquipmentToRouted(oEquipmentBLL)
            Else
                oEquipmentBLL = populateEquip(oCarrEquip, oTransLoadXrefDet)
                oEquipmentBLL.BookConsPrefix = CurrentCNS
                'TODO:  add logic to determine when BookRouteConsFlag should be false.  For now it is always true
                oEquipmentBLL.BookRouteConsFlag = True
                For Each bl In oBook.BookLoads
                    Dim oItemBLL = createBookItemsBLL(bl.BookItems)
                    'this will split items accross multiple pieces of identical equipment if needed based on capacity.
                    routeOrders(oEquipmentBLL, oItemBLL)
                Next
                If oEquipmentBLL.BookItemsBLL.TotalCases > 0 Then
                    AddEquipmentToRouted(oEquipmentBLL)
                End If
            End If
        End If
        'Note: the system must make sure that the detail sequence numbers are sequential stating at 1
        If intDetailSequence < (oTransLoad.LaneTransLoadXrefDets.Max(Function(x) x.LaneTranXDetSequence)) Then
            'get the next detail record
            'Bug fixed by RHR for v-8.5.4.003 on 10/29/2023 gaps may exist in sequence numbers 
            '   values are numeric fields editable by users
            'intDetailSequence += 1
            'We now get the next value in the data
            intDetailSequence = oTransLoad.LaneTransLoadXrefDets.Where(Function(x) x.LaneTranXDetSequence > intDetailSequence).OrderBy(Function(z) z.LaneTranXDetSequence).Select(Function(y) y.LaneTranXDetSequence).FirstOrDefault()
            Return processNextTransLoad(oBook, oTransLoad, intDetailSequence, oRet)
        End If
        Return blnRet
    End Function

    ''' <summary>
    ''' Is Transload: the new LaneIsTransLoad  flag must be checked (true) 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function isTransLoadOn(ByRef oLane As DTO.Lane) As Boolean
        Dim blnRet As Boolean = False
        If oLane Is Nothing OrElse oLane.LaneControl = 0 Then Return False
        If oLane.LaneIsTransLoad Then Return True
        Return blnRet
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oTransLoad"></param>
    ''' <param name="intDetailSequence"></param>
    ''' <param name="intCompControl"></param>
    ''' <param name="strCurrentCNS"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Business Rules:
    ''' 1 - If can bill together and LaneTranXDetConsolidateSplits = true the SHID number from the previous LaneTransLoadXrefDet is assigned to this LaneTransLoadXrefDet
    ''' 2 - ElseIf LaneTranXDetConsolidateSplits = true and the intDetailSequence > 1 use the previous SHID number
    ''' 3 - Else We Get the Next available SHID number
    ''' </remarks>
    Private Function getBookingSHID(ByRef oTransLoad As DTO.LaneTransLoadXref, ByRef oTransLoadXrefDetCurrent As DTO.LaneTransLoadXrefDet, ByVal intDetailSequence As Integer, ByVal intCompControl As Integer, ByVal strCurrentCNS As String) As Integer
        ''this code is not ready we do not have a way to assign the SHID,  this code was copied from the old code where we used the CNS as the SHID
        'If String.IsNullOrEmpty(strCurrentCNS) OrElse strCurrentCNS.Trim.Length < 3 Then Return NGLBatchProcessData.GetNextConsNumber(intCompControl)
        'Dim blnCanBillTogether As Boolean = canBillTogether(oTransLoad, oTransLoadXrefDetCurrent, intDetailSequence)
        'If blnCanBillTogether OrElse oTransLoadXrefDetCurrent.LaneTranXDetConsolidateSplits = True Then
        '    Return strCurrentCNS
        'Else
        '    Return NGLBatchProcessData.GetNextConsNumber(intCompControl)
        'End If
        Return 0
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oTransLoad"></param>
    ''' <param name="intDetailSequence"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Can bill together is only true when the previous LaneTransLoadXrefDet and the current LaneTransLoadXrefDet both have LaneTranXDetBilledSeperately set to false 
    ''' </remarks>
    Private Function canBillTogether(ByRef oTransLoad As DTO.LaneTransLoadXref, ByRef oTransLoadXrefDetCurrent As DTO.LaneTransLoadXrefDet, ByVal intDetailSequence As Integer) As Boolean
        Dim intPrevious As Integer = 0
        If intDetailSequence > 1 Then intPrevious = intDetailSequence - 1
        If intPrevious > 0 AndAlso oTransLoadXrefDetCurrent.LaneTranXDetBilledSeperately = False Then
            Dim oTransLoadXrefDetPrevious As DTO.LaneTransLoadXrefDet = oTransLoad.LaneTransLoadXrefDets.Where(Function(x) x.LaneTranXDetSequence = intPrevious).FirstOrDefault()
            If oTransLoadXrefDetPrevious.LaneTranXDetBilledSeperately = False Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Get first matching LaneTransLoadXref record for booking and Lane
    ''' </summary>
    ''' <param name="oBook"></param>
    ''' <param name="intLaneControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Old Notes added on 10/13/2023
    '''  reads all LaneTransLoadXref records for the provided intLaneControl
    '''     calls GetLaneTransLoadXrefsFilteredWDetailsAndText
    '''  next the procedure gets the first LaneTransLoadXref that matches the the Lane's Mode Type
    '''  if no lane mode matches get the first match by sea, rail, road, and air in that order
    '''  using orderMatchesTransLoadSettings which checks the matching capacity settings
    ''' </remarks>
    Private Function getTransloadConfiguration(ByRef oBook As DTO.Book, ByVal intLaneControl As Integer) As DTO.LaneTransLoadXref
        Dim oTransLoad As DTO.LaneTransLoadXref
        If oBook Is Nothing OrElse oBook.BookControl = 0 Then Return Nothing
        If intLaneControl = 0 Then Return Nothing
        Dim oLane As DTO.Lane = NGLLaneData.GetLaneFiltered(intLaneControl)
        If Not isTransLoadOn(oLane) Then Return Nothing
        Dim oTransLoads As DTO.LaneTransLoadXref() = NGLLaneTransLoadXrefData.GetLaneTransLoadXrefsFilteredWDetailsAndText(intLaneControl)
        If oTransLoads Is Nothing OrElse oTransLoads.Count < 1 Then Return Nothing
        Dim blnMatchFound As Boolean = False
        'filter by prefered mode as configured in the lane
        If oLane.LaneModeTypeControl <> 0 OrElse Not orderMatchesTransLoadSettings(oBook, oTransLoads, oTransLoad, oLane.LaneModeTypeControl) Then
            'if the lane mode does not match get the first one that matches search each mode type order by  (Sea, Rail, Road, Air)
            If oLane.LaneModeTypeControl = FreightMaster.Data.Utilities.TariffModeType.Sea AndAlso orderMatchesTransLoadSettings(oBook, oTransLoads, oTransLoad, FreightMaster.Data.Utilities.TariffModeType.Sea) Then Return oTransLoad
            If oLane.LaneModeTypeControl = FreightMaster.Data.Utilities.TariffModeType.Rail AndAlso orderMatchesTransLoadSettings(oBook, oTransLoads, oTransLoad, FreightMaster.Data.Utilities.TariffModeType.Rail) Then Return oTransLoad
            If oLane.LaneModeTypeControl = FreightMaster.Data.Utilities.TariffModeType.Road AndAlso orderMatchesTransLoadSettings(oBook, oTransLoads, oTransLoad, FreightMaster.Data.Utilities.TariffModeType.Road) Then Return oTransLoad
            If oLane.LaneModeTypeControl = FreightMaster.Data.Utilities.TariffModeType.Air AndAlso orderMatchesTransLoadSettings(oBook, oTransLoads, oTransLoad, FreightMaster.Data.Utilities.TariffModeType.Air) Then Return oTransLoad
        End If
        Return oTransLoad
    End Function

    ''' <summary>
    ''' Get the first available LaneTransLoadXref record that meets requirements
    ''' </summary>
    ''' <param name="oBook"></param>
    ''' <param name="oTransLoads"></param>
    ''' <param name="oTransLoad"></param>
    ''' <param name="intModeType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Notes added on 10/13/2023
    '''     Loops through each LaneTransLoadXref list
    '''     and returns the first match sorted by Sequence number
    '''     Where intModeType matches the LaneTranXModeTypeControl
    '''     and the booking data exceeds the minimum capacity requirements
    '''     and the booking data is less than the maximum capacity requirements
    ''' </remarks>
    Private Function orderMatchesTransLoadSettings(ByRef oBook As DTO.Book, ByRef oTransLoads As DTO.LaneTransLoadXref(), ByRef oTransLoad As DTO.LaneTransLoadXref, ByVal intModeType As DAL.Utilities.TariffModeType) As Boolean
        Dim blnRet As Boolean = False
        For Each t In oTransLoads.Where(Function(x) x.LaneTranXModeTypeControl = intModeType).OrderBy(Function(x) x.LaneTranXSequence)
            'For Each b In oBooks
            '    'this code does not work as expected also we do not have the child table bookloads at this time
            '    'how do we handle muliple temperature and does bookloadcom reference the same id field as lanetranxtemptype?
            '    'If b.BookLoads(0).BookLoadCom = t.LaneTranXTempType Then
            '    '    Continue For
            '    'End If
            'Next
            If exceedsMinimum(oBook, t) AndAlso inferiorToMaximum(oBook, t) Then
                blnRet = True
                oTransLoad = t
                Exit For
            End If
        Next
        Return blnRet
    End Function

    Private Function exceedsMinimum(ByRef oBook As DTO.Book, ByRef oTransLoad As DTO.LaneTransLoadXref) As Boolean
        Dim blnRet As Boolean = True
        If oTransLoad.LaneTranXMinCases > 0 AndAlso oBook.BookTotalCases < oTransLoad.LaneTranXMinCases Then Return False
        If oTransLoad.LaneTranXMinCube > 0 AndAlso oBook.BookTotalCube < oTransLoad.LaneTranXMinCube Then Return False
        If oTransLoad.LaneTranXMinPL > 0 AndAlso oBook.BookTotalPL < oTransLoad.LaneTranXMinPL Then Return False
        If oTransLoad.LaneTranXMinWgt > 0 AndAlso oBook.BookTotalWgt < oTransLoad.LaneTranXMinWgt Then Return False
        Return blnRet
    End Function

    Private Function inferiorToMaximum(ByRef oBook As DTO.Book, ByRef oTransLoad As DTO.LaneTransLoadXref) As Boolean
        Dim blnRet As Boolean = True
        If oTransLoad.LaneTranXMaxCases > 0 AndAlso oBook.BookTotalCases > oTransLoad.LaneTranXMaxCases Then Return False
        If oTransLoad.LaneTranXMaxCube > 0 AndAlso oBook.BookTotalCube > oTransLoad.LaneTranXMaxCube Then Return False
        If oTransLoad.LaneTranXMaxPL > 0 AndAlso oBook.BookTotalPL > oTransLoad.LaneTranXMaxPL Then Return False
        If oTransLoad.LaneTranXMaxWgt > 0 AndAlso oBook.BookTotalWgt > oTransLoad.LaneTranXMaxWgt Then Return False
        Return blnRet
    End Function

    Private Function populateEquip(ByRef oCarrEquip As DTO.CarrTarEquip, ByRef oTransLoadDet As DTO.LaneTransLoadXrefDet) As NGLTransLoadEquipDataBLL
        Dim oEquipmentBLL As New NGLTransLoadEquipDataBLL With {.CarrTarEquipControl = oCarrEquip.CarrTarEquipControl _
                                                           , .CarrTarEquipCasesConsiderFull = oCarrEquip.CarrTarEquipCasesConsiderFull _
                                                           , .CarrTarEquipCasesMaximum = oCarrEquip.CarrTarEquipCasesMaximum _
                                                           , .CarrTarEquipCasesMinimum = oCarrEquip.CarrTarEquipCasesMinimum _
                                                           , .CarrTarEquipCubesConsiderFull = oCarrEquip.CarrTarEquipCubesConsiderFull _
                                                           , .CarrTarEquipCubesMaximum = oCarrEquip.CarrTarEquipCubesMaximum _
                                                           , .CarrTarEquipCubesMinimum = oCarrEquip.CarrTarEquipCubesMinimum _
                                                           , .CarrTarEquipPltsConsiderFull = oCarrEquip.CarrTarEquipPltsConsiderFull _
                                                           , .CarrTarEquipPltsMaximum = oCarrEquip.CarrTarEquipPltsMaximum _
                                                           , .CarrTarEquipPltsMinimum = oCarrEquip.CarrTarEquipPltsMinimum _
                                                           , .CarrTarEquipWgtConsiderFull = oCarrEquip.CarrTarEquipWgtConsiderFull _
                                                           , .CarrTarEquipWgtMaximum = oCarrEquip.CarrTarEquipWgtMaximum _
                                                           , .CarrTarEquipWgtMinimum = oCarrEquip.CarrTarEquipWgtMinimum _
                                                           , .TransLoadXrefDet = oTransLoadDet}

        Return oEquipmentBLL
    End Function

    Private Function createBookItemsBLL(ByRef oBookItems As List(Of DTO.BookItem)) As NGLTransLoadBookItemsBLL
        Dim oBookItemsBLL As New NGLTransLoadBookItemsBLL
        oBookItemsBLL.populateItems(oBookItems)
        Return oBookItemsBLL
    End Function

    Private Function updateOriginalBookData(ByRef oBook As DTO.Book, ByRef oEquipmentBLL As NGLTransLoadEquipDataBLL) As Boolean
        Dim blnRet As Boolean = True
        If oBook Is Nothing OrElse oBook.BookControl = 1 Then Return False
        With oBook
            If Not updateTransloadLaneAddressInfo(oBook, oEquipmentBLL.TransLoadXrefDet) Then Return False
            .BookOriginalLaneControl = Me.OriginalLaneControl
            .BookMultiMode = True
            .BookLaneTranXControl = oEquipmentBLL.TransLoadXrefDet.LaneTranXDetLaneTranXControl
            .BookLaneTranXDetControl = oEquipmentBLL.TransLoadXrefDet.LaneTranXDetControl
            .BookCarrierControl = oEquipmentBLL.TransLoadXrefDet.LaneTranXDetCarrierControl
            .BookCarrierContControl = oEquipmentBLL.TransLoadXrefDet.LaneTranXDetCarrierContControl
            .BookCarrierContact = oEquipmentBLL.TransLoadXrefDet.LaneTranXDetContName
            .BookCarrierContactPhone = If(If(oEquipmentBLL.TransLoadXrefDet.LaneTranXDetCont800, "").Trim.Length < 1, _
                                          If(oEquipmentBLL.TransLoadXrefDet.LaneTranXDetContPhone, "") & " " & _
                                          If(oEquipmentBLL.TransLoadXrefDet.LaneTranXDetContExt, ""), _
                                          If(oEquipmentBLL.TransLoadXrefDet.LaneTranXDetCont800, ""))
            .BookCarrTarControl = oEquipmentBLL.TransLoadXrefDet.LaneTranXDetCarrTarControl
            .BookCarrTarEquipControl = oEquipmentBLL.TransLoadXrefDet.LaneTranXDetCarrTarEquipControl
            .BookTransType = If(oEquipmentBLL.TransLoadXrefDet.LaneTranXDetTransType, 0)
            If oEquipmentBLL.TransLoadXrefDet.LaneTranXDetCarrierControl = 0 Then
                .BookTranCode = "N"
            Else
                .BookTranCode = "P"
            End If
            'BookMilesFrom is set to oLane.LaneBenchMiles in updateTransloadLaneAddressInfo above
            .BookMilesFrom = If(oEquipmentBLL.TransLoadXrefDet.LaneTranXDetBenchMiles < 1, .BookMilesFrom, oEquipmentBLL.TransLoadXrefDet.LaneTranXDetBenchMiles)
            If oEquipmentBLL.TransLoadXrefDet.LaneTranXDetRule11Required Then .BookAllowInterlinePoints = True
            .BookConsPrefix = oEquipmentBLL.BookConsPrefix
            .BookRouteConsFlag = oEquipmentBLL.BookRouteConsFlag
            .TrackingState = Core.ChangeTracker.TrackingInfo.Updated
        End With
        Return blnRet
    End Function

    Private Function updateTransloadLaneAddressInfo(ByRef oBook As DTO.Book, ByRef oTranLoadDet As DTO.LaneTransLoadXrefDet) As Boolean
        Dim blnRet As Boolean = True
        'get the lane data
        Dim oLane As DTO.Lane = NGLLaneData.GetLaneFiltered(oTranLoadDet.LaneTranXDetLaneControl)
        With oBook
            .BookODControl = oLane.LaneControl
            .BookOrigCompControl = oLane.LaneOrigCompControl
            .BookOrigName = oLane.LaneOrigName
            .BookOrigAddress1 = oLane.LaneOrigAddress1
            .BookOrigAddress2 = oLane.LaneOrigAddress2
            .BookOrigAddress3 = oLane.LaneOrigAddress3
            .BookOrigCity = oLane.LaneOrigCity
            .BookOrigState = oLane.LaneOrigState
            .BookOrigZip = oLane.LaneOrigZip
            .BookOrigCountry = oLane.LaneOrigCountry
            .BookOriginApptReq = oLane.LaneAppt
            .BookOrigPhone = oLane.LaneOrigContactPhone
            .BookOrigFax = oLane.LaneOrigContactFax
            .BookOriginStartHrs = oLane.LaneRecHourStart
            .BookOriginStopHrs = oLane.LaneRecHourStop
            .BookDestCompControl = oLane.LaneDestCompControl
            .BookDestName = oLane.LaneDestName
            .BookDestAddress1 = oLane.LaneDestAddress1
            .BookDestAddress2 = oLane.LaneDestAddress2
            .BookDestAddress3 = oLane.LaneDestAddress3
            .BookDestCity = oLane.LaneDestCity
            .BookDestState = oLane.LaneDestState
            .BookDestZip = oLane.LaneDestZip
            .BookDestCountry = oLane.LaneDestCountry
            .BookDestPhone = oLane.LaneDestContactPhone
            .BookDestFax = oLane.LaneDestContactFax
            .BookDestStartHrs = oLane.LaneDestHourStart
            .BookDestStopHrs = oLane.LaneDestHourStop
            .BookDestApptReq = oLane.LaneAptDelivery
            .BookDefaultRouteSequence = oLane.LaneDefaultRouteSequence
            .BookRouteGuideNumber = oLane.LaneRouteGuideNumber
            .BookRouteGuideControl = oLane.LaneRouteGuideControl
            .BookRouteTypeCode = oLane.LaneRouteTypeCode
            .BookAllowInterlinePoints = oLane.LaneAllowInterline
            .BookMilesFrom = oLane.LaneBenchMiles
        End With

        Return blnRet
    End Function

    Private Function updateBookItems(ByRef oBook As DTO.Book, ByRef oEquipmentBLL As NGLTransLoadEquipDataBLL) As Boolean
        Dim blnRet As Boolean = True
        If oEquipmentBLL Is Nothing OrElse oEquipmentBLL.BookItemsBLL Is Nothing OrElse oEquipmentBLL.BookItemsBLL.BookItems Is Nothing OrElse oEquipmentBLL.BookItemsBLL.BookItems.Count < 1 Then Return False
        With oBook
            If .BookLoads Is Nothing OrElse .BookLoads.Count < 1 Then Return False
            oBook.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
            If oBook.BookOrderSequence = 0 Then
                For Each bl In .BookLoads
                    For Each item In bl.BookItems
                        Dim oIUpdated = oEquipmentBLL.BookItemsBLL.BookItems.Where(Function(x) x.BookItem.BookItemControl = item.BookItemControl).FirstOrDefault()
                        If oIUpdated Is Nothing OrElse oIUpdated.BookItem.BookItemControl = 0 Then
                            item.TrackingState = Core.ChangeTracker.TrackingInfo.Deleted
                        Else
                            item.BookItemQtyOrdered = oIUpdated.BookItemQtyOrdered
                            item.BookItemWeight = oIUpdated.BookItemWeight
                            item.BookItemPallets = oIUpdated.BookItemPallets
                            item.BookItemCube = oIUpdated.BookItemCube
                            item.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                        End If
                    Next
                Next
            Else
                'this is a split order so just add the items associated with this order to the first bookload record
                Dim oBookLoad As DTO.BookLoad = .BookLoads(0)
                oBookLoad.TrackingState = Core.ChangeTracker.TrackingInfo.Updated
                For Each bi In oEquipmentBLL.BookItemsBLL.BookItems
                    Dim oBookItem As DTO.BookItem = createNewBookItem(bi)
                    oBookItem.BookItemBookLoadControl = oBookLoad.BookLoadControl
                    oBookLoad.BookItems.Add(oBookItem)
                Next
            End If
        End With
        Return blnRet
    End Function

    Private Function createNewBookItem(ByVal bi As NGLTransLoadBookItemDataBLL) As DTO.BookItem

        Dim oBookItem As DTO.BookItem = bi.BookItem.Clone()
        With oBookItem
            .BookItemQtyOrdered = bi.BookItemQtyOrdered
            .BookItemWeight = bi.BookItemWeight
            .BookItemPallets = bi.BookItemPallets
            .BookItemCube = bi.BookItemCube
            .BookItemBookLoadControl = 0
            .BookItemControl = 0
            .BookItemUpdated = New Byte() {}
            .TrackingState = Core.ChangeTracker.TrackingInfo.Created
        End With
        Return oBookItem

    End Function

#End Region

#Region "Depreciated Methods"

    'Private Function getNextOrderSequence() As Integer
    '    Me.ThisOrderSequence += 1
    '    Return ThisOrderSequence
    'End Function

    'Private Function updateNextBooks(ByRef oNextBooks As List(Of DTO.Book), ByRef oTransLoad As DTO.LaneTransLoadXref, ByVal intDetailSequence As Integer, ByVal strCurrentCNS As String, ByRef oLoaded As List(Of DTO.Book)) As Boolean
    '    Dim blnRet As Boolean = False
    '    If oNextBooks Is Nothing OrElse oNextBooks.Count < 1 Then Return False
    '    For Each b In oNextBooks
    '        If Not updateTransloadLaneAddressInfo(b, oTransLoad.LaneTransLoadXrefDets(intDetailSequence)) Then Return False
    '        b.BookOriginalLaneControl = Me.OriginalLaneControl
    '        b.BookOrderSequence = getNextOrderSequence()
    '        b.BookMultiMode = True
    '        b.BookLaneTranXControl = oTransLoad.LaneTranXControl
    '        With oTransLoad.LaneTransLoadXrefDets(intDetailSequence)
    '            b.BookLaneTranXDetControl = .LaneTranXDetControl
    '            b.BookCarrierControl = .LaneTranXDetCarrierControl
    '            b.BookCarrierContControl = .LaneTranXDetCarrierContControl
    '            b.BookCarrierContact = .LaneTranXDetContName
    '            b.BookCarrierContactPhone = If(.LaneTranXDetCont800.Trim.Length < 1, .LaneTranXDetContPhone & " " & .LaneTranXDetContExt, .LaneTranXDetCont800)
    '            b.BookCarrTarControl = .LaneTranXDetCarrTarControl
    '            b.BookCarrTarEquipControl = .LaneTranXDetCarrTarEquipControl
    '            b.BookTransType = .LaneTranXDetTransType
    '            b.BookMilesFrom = .LaneTranXDetBenchMiles
    '            If .LaneTranXDetRule11Required Then b.BookAllowInterlinePoints = True
    '        End With
    '        b.BookConsPrefix = getBookingCNS(oTransLoad, intDetailSequence, b.BookCustCompControl, strCurrentCNS)
    '        b.BookRouteConsFlag = True
    '        b.TrackingState = Core.ChangeTracker.TrackingInfo.Updated


    '    Next
    '    Return blnRet
    'End Function



    'Private Sub updateBookTotals(ByRef oBook As DTO.Book)
    '    With oBook
    '        For Each bl In .BookLoads
    '            bl.BookLoadCaseQty = bl.BookItems.Sum(Function(x) x.BookItemQtyOrdered)
    '            bl.BookLoadWgt = bl.BookItems.Sum(Function(x) x.BookItemWeight)
    '            bl.BookLoadPL = bl.BookItems.Sum(Function(x) x.BookItemPallets)
    '            bl.BookLoadCube = bl.BookItems.Sum(Function(x) x.BookItemCube)
    '        Next
    '        .BookTotalCases = .BookLoads.Sum(Function(x) x.BookLoadCaseQty)
    '        .BookTotalWgt = .BookLoads.Sum(Function(x) x.BookLoadWgt)
    '        .BookTotalPL = .BookLoads.Sum(Function(x) x.BookLoadPL)
    '        .BookTotalCube = .BookLoads.Sum(Function(x) x.BookLoadCube)
    '    End With
    'End Sub


    'Private Function cloneBooking(ByRef oBook As DTO.Book, Optional blnNoItems As Boolean = True) As DTO.Book
    '    Dim oNewBook As DTO.Book = oBook.Clone()
    '    With oNewBook
    '        .BookProNumber = "" 'we will get this before we save
    '        .BookControl = -1 'we will insert the booking record then add the bookloads back in on save
    '        .BookConsPrefix = "" 'we will get this before we sabe
    '        For Each bl In .BookLoads
    '            bl.BookLoadControl = -1 ' we will get this before we save
    '            bl.BookLoadBookControl = -1 'we will get this before we save
    '            bl.TrackingState = Core.ChangeTracker.TrackingInfo.Created
    '            If blnNoItems Then
    '                bl.BookItems = New List(Of DTO.BookItem)
    '            Else
    '                For Each bi In bl.BookItems
    '                    bi.BookItemControl = -1
    '                    bi.BookItemBookLoadControl = -1
    '                    bi.TrackingState = Core.ChangeTracker.TrackingInfo.Created
    '                Next

    '            End If
    '        Next
    '        .TrackingState = Core.ChangeTracker.TrackingInfo.Created
    '    End With
    '    Return oNewBook
    'End Function

#End Region

End Class
