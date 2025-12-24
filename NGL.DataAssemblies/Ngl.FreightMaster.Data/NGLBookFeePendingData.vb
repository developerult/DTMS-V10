Imports System.Linq.Dynamic
Imports System.ServiceModel
Imports Ngl.Core.Utility

Public Class NGLBookFeePendingData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMasBookDataContext(ConnectionString)
        Me.LinqTable = db.BookFeesPendings
        Me.LinqDB = db
        Me.SourceClass = "NGLBookFeePendingData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            Dim db As New NGLMasBookDataContext(ConnectionString)
            _LinqTable = db.BookFeesPendings
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
        Return GetBookFeePendingFiltered(Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetBookFeePendingsFiltered()
    End Function

    Public Function GetBookFeePendingFiltered(ByVal Control As Integer) As DataTransferObjects.BookFeePending
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                'Get the newest record that matches the provided criteria
                Dim BookFee As DataTransferObjects.BookFeePending = (
                        From d In db.BookFeesPendings
                        Where
                        d.BookFeesPendingControl = Control
                        Select selectDTOData(d)).First()

                Return BookFee

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookFeesPendingFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' ** IMPORTANT NOTE **
    ''' Modified By LVV on 9/28/2017 for v-8.0 TMS365
    ''' Since we added new field BookFeesPendingApproved to the table I added a filter
    ''' to the Where clause to only select records where BookFeesPendingApproved = 0
    ''' This is so the function does not return any historical records and thus continues to
    ''' function as it did before the change to the BookFeesPending table.
    ''' (Previously when a BookFeesPending record was approved it was removed from the table
    ''' but now it stays and has the BookFeesPendingApproved Flag set to 1)
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    Public Function GetBookFeePendingsFiltered(Optional ByVal BookControl As Integer = 0) As DataTransferObjects.BookFeePending()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Return all the fees that match the criteria sorted by caption
                Dim BookFeesPending() As DataTransferObjects.BookFeePending = (
                        From d In db.BookFeesPendings
                        Where
                        (d.BookFeesPendingBookControl = BookControl) _
                        And
                        d.BookFeesPendingApproved = 0
                        Order By d.BookFeesPendingCaption
                        Select selectDTOData(d)).ToArray()

                Return BookFeesPending

            Catch ex As System.Data.SqlClient.SqlException
                throwSQLFaultException(ex.Message)
            Catch ex As InvalidOperationException
                throwNoDataFaultException(ex.Message)
            Catch ex As Exception
                throwUnExpectedFaultException(ex, buildProcedureName("GetBookFeesPendingFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function DeleteBookFeesPending(ByVal BFPControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Dim oTable = db.BookFeesPendings
            Try
                Dim oRecord As LTS.BookFeesPending = db.BookFeesPendings.Where(Function(x) x.BookFeesPendingControl = BFPControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.BookFeesPendingControl = 0) Then Return False

                oTable.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteBookFeesPending"))
            End Try
        End Using
        Return blnRet
    End Function

#Region "TMS 365"

    ''' <summary>
    ''' Returns BookFeesPending records that are NOT APPROVED
    ''' Used to get the records for the grid on the 
    ''' Carrier Accessorial Approval Screen in 365
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 9/27/2017 for v-8.0 TMS 365
    ''' 
    ''' </remarks>
    Public Function GetvBookFeesPending(ByRef RecordCount As Integer,
                                        ByVal filters As Models.AllFilters) As LTS.vBookFeesPending()
        Dim oRetData As LTS.vBookFeesPending()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim oQuery As IQueryable(Of LTS.vBookFeesPending)

                Dim filterWhere = ""
                Dim sSep = ""
                If (Not String.IsNullOrWhiteSpace(filters.filterName)) Then
                    If (Not String.IsNullOrWhiteSpace(filters.filterValue)) Then
                        Dim dblVal = 0
                        If Double.TryParse(filters.filterValue, dblVal) Then
                            filterWhere = "(" + filters.filterName + "=""" + filters.filterValue + """)"
                        Else
                            filterWhere = "(" + filters.filterName + ".Contains(""" + filters.filterValue + """))"
                        End If

                        sSep = " And "
                    End If

                    If ((Not filters.filterTo Is Nothing) OrElse (Not filters.filterFrom Is Nothing)) Then
                        Dim StartDate = DataTransformation.formatStartDateFilter(filters.filterFrom)
                        Dim EndDate = DataTransformation.formatEndDateFilter(filters.filterTo)
                        '(t.BFPModDate Is Nothing) Or (t.BFPModDate >= StartDate And t.BFPModDate <= EndDate))
                        filterWhere += sSep + "((" + filters.filterName + " = NULL) Or (" + filters.filterName + " >= DateTime.Parse(""" + StartDate + """) And " + filters.filterName + " <= DateTime.Parse(""" + EndDate + """)))"
                        sSep = " And "
                    End If

                    If (filters.CarrierControlFrom > 0) Then
                        filterWhere += sSep + "(CarrierControl = " + filters.CarrierControlFrom.ToString() + ")"
                        sSep = " And "
                    End If

                End If

                oQuery = (From t In db.vBookFeesPendings
                    Where t.BFPApproved = 0
                    Select t)

                db.Log = New DebugTextWriter

                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If

                If oQuery Is Nothing Then Return Nothing
                RecordCount = oQuery.Count()
                If RecordCount < 1 Then RecordCount = 1
                If filters.take < 1 Then filters.take = 1
                'adjust for last page if skip beyond last page
                If filters.skip >= RecordCount Then filters.skip = (CInt(((RecordCount - 1) / filters.take)) * filters.take)
                'adjust for first page if skip beyond or below first page
                If filters.skip >= RecordCount Or filters.skip < 0 Then filters.skip = 0

                'use the extension methods to sort the query
                If Not String.IsNullOrWhiteSpace(filters.sortName) Then
                    If Left(filters.sortDirection.ToLower(), 3) = "des" Then
                        oQuery = oQuery.OrderBy(filters.sortName, True)
                    Else
                        oQuery = oQuery.OrderBy(filters.sortName, False)
                    End If
                Else
                    'default sort by BookSHID Desc
                    oQuery = oQuery.OrderBy("BookSHID", True)
                End If

                oRetData = oQuery.Skip(filters.skip).Take(filters.take).ToArray()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetvBookFeesPending"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetLTSBookFeesPending(ByVal BookFeesPendingControl As Integer) As LTS.BookFeesPending

        Dim oRetData As LTS.BookFeesPending
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                oRetData = db.BookFeesPendings.Where(Function(x) x.BookFeesPendingControl = BookFeesPendingControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLTSBookFeesPending"))
            End Try
            Return oRetData
        End Using
    End Function

    ''' <summary>
    ''' Called from the Carrier Accessorial Approval Screen in 365
    ''' Saves update to BookFeesPendingValue = BFPValue AND BookFeesPendingMinimum = BFPValue
    ''' for the record with BookFeesPendingControl = BFPControl
    ''' </summary>
    ''' <param name="BFPControl"></param>
    ''' <param name="BFPValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 9/27/2017 for v-8.0 TMS 365
    ''' </remarks>
    Public Function SaveBookFeesPendingValue(ByVal BFPControl As Integer, ByVal BFPValue As Decimal) As Boolean
        Using db As New NGLMasBookDataContext(ConnectionString)
            Dim retval As Boolean = False
            Try
                'Get the record with the specified control number
                Dim BPF = (From d In db.BookFeesPendings Where d.BookFeesPendingControl = BFPControl).FirstOrDefault()

                If Not BPF Is Nothing Then
                    BPF.BookFeesPendingValue = BFPValue
                    BPF.BookFeesPendingMinimum = BFPValue
                    BPF.BookFeesPendingModDate = Date.Now
                    BPF.BookFeesPendingModUser = Parameters.UserName

                    db.SubmitChanges()
                    retval = True
                End If
                Return retval
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveBookFeesPendingValue"))
            End Try
            Return retval
        End Using
    End Function

    ''' <summary>
    ''' Called from the Carrier Accessorial Approval Screen in 365
    ''' Also called from the Settlement Page 365 when the user saves
    ''' changes in the FBDE window if AutoApprove is true for an entered fee.
    ''' Approves a BookFeePending record by adding it to the BookFees table and
    ''' setting BookFeePendingApproved Flag to true. 
    ''' Returns an object with RetMsg, ErrNumber, and BookFeeControl
    ''' </summary>
    ''' <param name="BFPControl"></param>
    ''' <param name="ApprovedBy"></param>
    ''' <remarks>
    ''' Added By LVV on 9/28/2017 for v-8.0 TMS 365
    ''' </remarks>
    ''' <returns></returns>
    Public Function ApproveBookFeePending(ByVal BFPControl As Integer, ByVal ApprovedBy As String) As LTS.spAcceptPendingBookFeeResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Return db.spAcceptPendingBookFee(BFPControl, ApprovedBy).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ApproveBookFeePending"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Called from Settlement Screen 365
    ''' Used to get any existing fees in the database for the Booking
    ''' records associated with the SHID of the Settlement record
    ''' Populates the details grids of the FBSHIDGRID
    ''' </summary>
    ''' <param name="BookSHID"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 6/27/2019
    '''     added parameter to ignore fuel fees
    '''     caller must now call GetSettlementFuelForSHID to read fuel values
    ''' Modified by RHR for v-8.2.1.004 on 01/03/2020
    '''     added logic to identify missing fees
    '''     Note: requires update to spGetSettlementFeesForSHID
    ''' </remarks>
    Public Function GetSettlementFeesForSHID(ByVal BookSHID As String, Optional ByVal ReturnVisibleFeesOnly As Boolean = False, Optional blnIgnoreFuel As Boolean = True) As Models.SettlementFee()
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim Fees As New List(Of Models.SettlementFee)
                If String.IsNullOrWhiteSpace(BookSHID) Then Return Nothing
                Dim spRes = db.spGetSettlementFeesForSHID(BookSHID, ReturnVisibleFeesOnly, blnIgnoreFuel).ToArray()
                Dim fIndex = 1
                For Each oItem In spRes
                    Dim f As New Models.SettlementFee
                    With f
                        .Control = If(oItem.Ctrl, 0)
                        .BookControl = If(oItem.BookControl, 0)
                        .Cost = If(oItem.Cost, 0)
                        .Minimum = If(oItem.Minimum, If(oItem.Cost, 0))
                        .AccessorialCode = If(oItem.AccessorialCode, 0)
                        .Caption = oItem.Caption
                        .AutoApprove = If(oItem.AutoApprove, False)
                        .AllowCarrierUpdates = If(oItem.AllowCarrierUpdates, False)
                        .Pending = If(oItem.Pending, False)
                        .Msg = oItem.Msg
                        .FeeIndex = fIndex
                        .StopSequence = If(oItem.StopSequence, 0)
                        .BookCarrOrderNumber = oItem.BookCarrOrderNumber
                        .BookOrderSequence = If(oItem.BookOrderSequence, 0)
                        .EDICode = oItem.EDICode
                        .MissingFee = If(oItem.MissingFee, False)
                    End With
                    Fees.Add(f)
                    fIndex += 1
                Next
                Return Fees.ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSettlementFeesForSHID"), db)
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' BFP - Creates or updates record in BFP
    ''' BF - Creates a New record in BFP table for this update (leave original BF record alone)
    ''' </summary>
    ''' <param name="sFee"></param>
    ''' <remarks>
    ''' Added By LVV on 10/12/2017 for v-8.0 TMS 365
    ''' </remarks>
    ''' <returns></returns>
    Public Function SaveSettlementFees(ByVal sFee As Models.SettlementFee, ByVal CompControl As Integer, ByVal CarrierControl As Integer) As LTS.spSaveSettlementFeesResult
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try

                Return db.spSaveSettlementFees(sFee.Control, sFee.BookControl, sFee.Cost, sFee.AccessorialCode, sFee.Caption, CompControl, CarrierControl, sFee.Pending, sFee.MissingFee).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SaveSettlementFees"))
            End Try
            Return Nothing
        End Using
    End Function

    Public Function GetSettlementFBDEData(ByVal BookSHID As String, Optional ByVal ReturnVisibleFeesOnly As Boolean = False, Optional blnIgnoreFuel As Boolean = True) As Models.SettlementFBDEData
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                If String.IsNullOrWhiteSpace(BookSHID) Then Return Nothing
                Dim oRet As New Models.SettlementFBDEData
                'Get the Last Stop info
                Dim lastStop = db.vFBSHIDGrid365s.Where(Function(x) x.BookSHID = BookSHID).OrderByDescending(Function(y) y.BookStopNo).FirstOrDefault()
                oRet.LastStopCompControl = lastStop.BookCustCompControl
                oRet.LastStopCompName = lastStop.CompName
                oRet.LastStopCompLE = lastStop.CompLegalEntity
                oRet.LastStopCarrierControl = lastStop.BookCarrierControl
                oRet.LastStopCarrierName = lastStop.CarrierName
                oRet.LastStopCompLEControl = lastStop.CompLEControl
                'Get the Audit Message Visibility Info
                Dim oLook As New NGLLookupDataProvider(Parameters)
                Dim spMsg = oLook.CheckSettlementAuditMessageVisibility(lastStop.BookCustCompControl, lastStop.BookCarrierControl, BookSHID)
                If Not spMsg Is Nothing Then
                    oRet.ShowAuditFailReason = If(spMsg.ShowAuditFailReason.HasValue, spMsg.ShowAuditFailReason.Value, False)
                    oRet.ShowPendingFeeFailReason = If(spMsg.ShowPendingFeeFailReason.HasValue, spMsg.ShowPendingFeeFailReason.Value, False)
                    oRet.APMessage = spMsg.APMessage
                End If
                'Get the Fees
                oRet.LoadFees = db.spGetLoadSettlementFeesForSHID(BookSHID, ReturnVisibleFeesOnly, blnIgnoreFuel) _
                    .Select(Function(x, ixc) New Models.SettlementFee _
                               With {
                               .Control = If(x.Ctrl, 0),
                               .BookControl = If(x.BookControl, 0),
                               .Cost = If(x.Cost, 0),
                               .Minimum = If(x.Minimum, If(x.Cost, 0)),
                               .AccessorialCode = If(x.AccessorialCode, 0),
                               .Caption = x.Caption,
                               .AutoApprove = If(x.AutoApprove, False),
                               .AllowCarrierUpdates = If(x.AllowCarrierUpdates, False),
                               .Pending = If(x.Pending, False),
                               .Msg = x.Msg,
                               .FeeIndex = (ixc + 1), 'We add 1 because we want the FeeIndex to start at 1 not 0
                               .StopSequence = If(x.StopSequence, 0),
                               .BookCarrOrderNumber = x.BookCarrOrderNumber,
                               .BookOrderSequence = If(x.BookOrderSequence, 0),
                               .EDICode = x.EDICode,
                               .MissingFee = If(x.MissingFee, False),
                               .OrigName = x.BookOrigName, 'Added By LVV on 3/15/20
                               .DestName = x.BookDestName, 'Added By LVV on 3/15/20
                               .OrigZip = x.BookOrigZip, 'Added By LVV on 4/1/20
                               .DestZip = x.BookDestZip, 'Added By LVV on 4/1/20
                               .CNS = x.BookConsPrefix, 'Added By LVV on 3/15/20
                               .SHID = x.BookSHID, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeControl = x.AccessorialFeeAllocationTypeControl, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeName = x.AccessorialFeeAllocationTypeName, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeDesc = x.AccessorialFeeAllocationTypeDesc 'Added By LVV on 3/15/20                                                              
                               }).ToArray()

                oRet.OrderFees = db.spGetOrderSettlementFeesForSHID(BookSHID, ReturnVisibleFeesOnly, blnIgnoreFuel) _
                    .Select(Function(x, ixc) New Models.SettlementFee _
                               With {
                               .Control = If(x.Ctrl, 0),
                               .BookControl = If(x.BookControl, 0),
                               .Cost = If(x.Cost, 0),
                               .Minimum = If(x.Minimum, If(x.Cost, 0)),
                               .AccessorialCode = If(x.AccessorialCode, 0),
                               .Caption = x.Caption,
                               .AutoApprove = If(x.AutoApprove, False),
                               .AllowCarrierUpdates = If(x.AllowCarrierUpdates, False),
                               .Pending = If(x.Pending, False),
                               .Msg = x.Msg,
                               .FeeIndex = (ixc + 1), 'We add 1 because we want the FeeIndex to start at 1 not 0
                               .StopSequence = If(x.StopSequence, 0),
                               .BookCarrOrderNumber = x.BookCarrOrderNumber,
                               .BookOrderSequence = If(x.BookOrderSequence, 0),
                               .EDICode = x.EDICode,
                               .MissingFee = If(x.MissingFee, False),
                               .OrigName = x.BookOrigName, 'Added By LVV on 3/15/20
                               .DestName = x.BookDestName, 'Added By LVV on 3/15/20
                               .OrigZip = x.BookOrigZip, 'Added By LVV on 4/1/20
                               .DestZip = x.BookDestZip, 'Added By LVV on 4/1/20
                               .CNS = x.BookConsPrefix, 'Added By LVV on 3/15/20
                               .SHID = x.BookSHID, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeControl = x.AccessorialFeeAllocationTypeControl, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeName = x.AccessorialFeeAllocationTypeName, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeDesc = x.AccessorialFeeAllocationTypeDesc 'Added By LVV on 3/15/20                                                              
                               }).ToArray()

                oRet.OrigFees = db.spGetOrigSettlementFeesForSHID(BookSHID, ReturnVisibleFeesOnly, blnIgnoreFuel) _
                    .Select(Function(x, ixc) New Models.SettlementFee _
                               With {
                               .Control = If(x.Ctrl, 0),
                               .BookControl = If(x.BookControl, 0),
                               .Cost = If(x.Cost, 0),
                               .Minimum = If(x.Minimum, If(x.Cost, 0)),
                               .AccessorialCode = If(x.AccessorialCode, 0),
                               .Caption = x.Caption,
                               .AutoApprove = If(x.AutoApprove, False),
                               .AllowCarrierUpdates = If(x.AllowCarrierUpdates, False),
                               .Pending = If(x.Pending, False),
                               .Msg = x.Msg,
                               .FeeIndex = (ixc + 1), 'We add 1 because we want the FeeIndex to start at 1 not 0
                               .StopSequence = If(x.StopSequence, 0),
                               .BookCarrOrderNumber = x.BookCarrOrderNumber,
                               .BookOrderSequence = If(x.BookOrderSequence, 0),
                               .EDICode = x.EDICode,
                               .MissingFee = If(x.MissingFee, False),
                               .OrigName = x.BookOrigName, 'Added By LVV on 3/15/20
                               .DestName = x.BookDestName, 'Added By LVV on 3/15/20
                               .OrigZip = x.BookOrigZip, 'Added By LVV on 4/1/20
                               .DestZip = x.BookDestZip, 'Added By LVV on 4/1/20
                               .CNS = x.BookConsPrefix, 'Added By LVV on 3/15/20
                               .SHID = x.BookSHID, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeControl = x.AccessorialFeeAllocationTypeControl, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeName = x.AccessorialFeeAllocationTypeName, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeDesc = x.AccessorialFeeAllocationTypeDesc 'Added By LVV on 3/15/20                                                              
                               }).ToArray()

                oRet.DestFees = db.spGetDestSettlementFeesForSHID(BookSHID, ReturnVisibleFeesOnly, blnIgnoreFuel) _
                    .Select(Function(x, ixc) New Models.SettlementFee _
                               With {
                               .Control = If(x.Ctrl, 0),
                               .BookControl = If(x.BookControl, 0),
                               .Cost = If(x.Cost, 0),
                               .Minimum = If(x.Minimum, If(x.Cost, 0)),
                               .AccessorialCode = If(x.AccessorialCode, 0),
                               .Caption = x.Caption,
                               .AutoApprove = If(x.AutoApprove, False),
                               .AllowCarrierUpdates = If(x.AllowCarrierUpdates, False),
                               .Pending = If(x.Pending, False),
                               .Msg = x.Msg,
                               .FeeIndex = (ixc + 1), 'We add 1 because we want the FeeIndex to start at 1 not 0
                               .StopSequence = If(x.StopSequence, 0),
                               .BookCarrOrderNumber = x.BookCarrOrderNumber,
                               .BookOrderSequence = If(x.BookOrderSequence, 0),
                               .EDICode = x.EDICode,
                               .MissingFee = If(x.MissingFee, False),
                               .OrigName = x.BookOrigName, 'Added By LVV on 3/15/20
                               .DestName = x.BookDestName, 'Added By LVV on 3/15/20
                               .OrigZip = x.BookOrigZip, 'Added By LVV on 4/1/20
                               .DestZip = x.BookDestZip, 'Added By LVV on 4/1/20
                               .CNS = x.BookConsPrefix, 'Added By LVV on 3/15/20
                               .SHID = x.BookSHID, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeControl = x.AccessorialFeeAllocationTypeControl, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeName = x.AccessorialFeeAllocationTypeName, 'Added By LVV on 3/15/20
                               .FeeAllocationTypeDesc = x.AccessorialFeeAllocationTypeDesc 'Added By LVV on 3/15/20                                                              
                               }).ToArray()

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetSettlementFBDEData"), db)
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' ** DEPRECIATED **
    ''' Uses BookControl to look up the CarrierControl and LEAdminControl.
    ''' Looks up CLAX Tolerances and tests if Cost is within tolerances. 
    ''' If CLAX Tolerances are not set up then returns true by default
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="AccessorialCode"></param>
    ''' <param name="Cost"></param>
    ''' <remarks>
    ''' Added By LVV on 10/12/2017 for v-8.0 TMS 365
    ''' Depreciated By LVV on 3/15/20 - I don't think this is being used anywhere
    ''' </remarks>
    ''' <returns></returns>
    Public Function IsFeeWithinCLAXTolerances(ByVal BookControl As Integer, ByVal AccessorialCode As Integer, ByVal Cost As Decimal) As Boolean
        throwDepreciatedException("IsFeeWithinCLAXTolerances - Depreciated By LVV on 3/15/20 because it is not being called by anything")
        Return Nothing
        ''Dim blnRet = False
        ''Using db As New NGLMasBookDataContext(ConnectionString)
        ''    Try
        ''        Dim res = db.spIsFeeWithinCLAXTolerances(BookControl, AccessorialCode, Cost).FirstOrDefault()
        ''        Return res.blnWithinTolerace
        ''    Catch ex As Exception
        ''        ManageLinqDataExceptions(ex, buildProcedureName("IsFeeWithinCLAXTolerances"))
        ''    End Try
        ''    Return blnRet
        ''End Using
    End Function

    ''' <summary>
    ''' Called by the Delete button in the FBSHIDGRID
    ''' on the Settlement Screen 365
    ''' </summary>
    ''' <param name="BFPControl"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    Public Function DeleteSettlementBFP(ByVal BFPControl As Integer, ByRef strMsg As String) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Dim oTable = db.BookFeesPendings
            Try
                Dim oRecord As LTS.BookFeesPending = db.BookFeesPendings.Where(Function(x) x.BookFeesPendingControl = BFPControl).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.BookFeesPendingControl = 0) Then Return False

                'RULE: If signed in user is Carrier the CarrierControl must match the booking record
                If Parameters.IsUserCarrier Then
                    Dim carrierControl As Integer = db.Books.Where(Function(x) x.BookControl = oRecord.BookFeesPendingBookControl).Select(Function(y) y.BookCarrierControl).FirstOrDefault()
                    If Parameters.UserCarrierControl <> carrierControl Then
                        strMsg = "User account does not match the Carrier for the booking And does Not have permissions to delete this fee"
                        Return False
                    End If
                End If

                oTable.DeleteOnSubmit(oRecord)
                db.SubmitChanges()
                blnRet = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DeleteSettlementBFP"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Unlocks the costs from all orders on the shipment id without any further validation for the entire SHID. 
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="blnUnlockBFC"></param>
    ''' <returns></returns>
    Public Function UnlockCostsForSHID365(ByVal BookControl As Integer, ByVal blnUnlockBFC As Boolean) As Boolean
        Dim blnRet = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim res = db.spUnlockCostsForSHID365(BookControl, blnUnlockBFC)
                blnRet = True
                Return blnRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UnlockCostsForSHID365"))
            End Try
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Uses BookControl to look up the CarrierControl and LEAdminControl.
    ''' Looks up CLAX Tolerances and tests if Cost is within tolerances. 
    ''' Updates the BookFeesPendingMessage with the reason for failure or
    ''' blank if success.
    ''' Returns true if Fee can be auto approved or False if it can't
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="AccessorialCode"></param>
    ''' <param name="Cost"></param>
    ''' <param name="BFPControl"></param>
    ''' <remarks>
    ''' Added By LVV on 1/9/2018 for v-8.0 TMS 365
    ''' Modified by LVV on 02/26/2018 for v-8.1 PQ EDI 
    ''' Now we call udfGetAutoApproveFailReason everytime --> removed parameter AutoApprove.
    ''' This is because I added additional messaging rules to the udf which I do not want to overwrite in code in this method
    ''' Modified by RHR for v-8.2.1.004 on 01/03/2020
    '''     added logic to support missing fee logic
    ''' </remarks>
    ''' <returns></returns>
    Public Function CanFeeBeAutoApproved(ByVal BookControl As Integer, ByVal AccessorialCode As Integer, ByVal Cost As Decimal, ByVal BFPControl As Integer) As Boolean
        Dim blnRet = False
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Save the BFPMessage
                Dim BFPMessage As String = ""
                Dim BPF = (From d In db.BookFeesPendings Where d.BookFeesPendingControl = BFPControl).FirstOrDefault()
                If Not BPF Is Nothing _
                   AndAlso BPF.BookFeesPendingValue > 0 _
                   AndAlso BPF.BookFeesPendingMissingFee _
                   AndAlso BPF.BookFeesPendingApproved = 0 _
                   AndAlso BPF.BookFeesPendingOverRidden = 0 Then
                    BFPMessage = "This fee was missing from the billed cost, manual approval is required."
                Else
                    'check if the fee can be auto approved and/or is within tolerance
                    Dim res = db.udfGetAutoApproveFailReason(BookControl, AccessorialCode, Cost)
                    If String.IsNullOrWhiteSpace(res) Then blnRet = True
                    BFPMessage = res.ToString()
                End If

                Try
                    'Save the BFPMessage
                    If Not BPF Is Nothing Then
                        BPF.BookFeesPendingMessage = BFPMessage
                        BPF.BookFeesPendingModDate = Date.Now
                        BPF.BookFeesPendingModUser = Parameters.UserName

                        db.SubmitChanges()
                    End If
                Catch ex As Exception
                    'Do nothing because we want to return true/false based on can fee be Auto Approved not based on update success/fail
                End Try

                Return blnRet

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CanFeeBeAutoApproved"))
            End Try
            Return blnRet
        End Using
    End Function

    ''' <summary>
    ''' Unit test method for auto approve fee message
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <param name="AccessorialCode"></param>
    ''' <param name="Cost"></param>
    ''' <param name="BFPControl"></param>
    ''' <returns></returns>
    Public Function GetFeeAutoApprovedMessage(ByVal BookControl As Integer, ByVal AccessorialCode As Integer, ByVal Cost As Decimal, ByVal BFPControl As Integer) As String

        Dim BFPMessage As String = ""
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                'Save the BFPMessage

                Dim BPF = (From d In db.BookFeesPendings Where d.BookFeesPendingControl = BFPControl).FirstOrDefault()
                If Not BPF Is Nothing _
                   AndAlso BPF.BookFeesPendingValue > 0 _
                   AndAlso BPF.BookFeesPendingMissingFee _
                   AndAlso BPF.BookFeesPendingApproved = 0 _
                   AndAlso BPF.BookFeesPendingOverRidden = 0 Then
                    BFPMessage = "This fee was missing from the billed cost, manual approval is required."
                Else
                    'check if the fee can be auto approved and/or is within tolerance
                    Dim res = db.udfGetAutoApproveFailReason(BookControl, AccessorialCode, Cost)

                    BFPMessage = res.ToString()
                End If

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CanFeeBeAutoApproved"))
            End Try
            Return BFPMessage
        End Using
    End Function


#End Region


#End Region

#Region "Protected Functions"

    ''' <summary>
    ''' Modified By LVV on 9/28/2017 for v-8.0 TMS365
    ''' Added fields: BookFeesPendingApproved, BookFeesPendingApprovedDate, and BookFeesPendingApprovedBy
    ''' Modified By LVV on 1/9/2018 for v-8.0 TMS365
    '''  Added field: BookFeesPendingMessage
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="t"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 7/3/2019
    ''' Added BookFeesPendingMissingFee  
    ''' </remarks>
    Friend Sub UpdateLTSWithDTO(ByRef d As DataTransferObjects.BookFeePending, ByRef t As LTS.BookFeesPending)

        With t
            .BookFeesPendingBookControl = d.BookFeesPendingBookControl
            .BookFeesPendingMinimum = d.BookFeesPendingMinimum
            .BookFeesPendingValue = d.BookFeesPendingValue
            .BookFeesPendingVariable = d.BookFeesPendingVariable
            .BookFeesPendingAccessorialCode = d.BookFeesPendingAccessorialCode
            .BookFeesPendingAccessorialFeeTypeControl = d.BookFeesPendingAccessorialFeeTypeControl
            .BookFeesPendingOverRidden = d.BookFeesPendingOverRidden
            .BookFeesPendingVariableCode = d.BookFeesPendingVariableCode
            .BookFeesPendingVisible = d.BookFeesPendingVisible
            .BookFeesPendingAutoApprove = d.BookFeesPendingAutoApprove
            .BookFeesPendingAllowCarrierUpdates = d.BookFeesPendingAllowCarrierUpdates
            .BookFeesPendingCaption = d.BookFeesPendingCaption
            .BookFeesPendingEDICode = d.BookFeesPendingEDICode
            .BookFeesPendingTaxable = d.BookFeesPendingTaxable
            .BookFeesPendingIsTax = d.BookFeesPendingIsTax
            .BookFeesPendingTaxSortOrder = d.BookFeesPendingTaxSortOrder
            .BookFeesPendingBOLText = d.BookFeesPendingBOLText
            .BookFeesPendingBOLPlacement = d.BookFeesPendingBOLPlacement
            .BookFeesPendingAccessorialFeeAllocationTypeControl = d.BookFeesPendingAccessorialFeeAllocationTypeControl
            .BookFeesPendingTarBracketTypeControl = d.BookFeesPendingTarBracketTypeControl
            .BookFeesPendingAccessorialFeeCalcTypeControl = d.BookFeesPendingAccessorialFeeCalcTypeControl
            .BookFeesPendingModDate = Date.Now
            .BookFeesPendingModUser = Parameters.UserName
            .BookFeesPendingApproved = d.BookFeesPendingApproved
            .BookFeesPendingApprovedDate = d.BookFeesPendingApprovedDate
            .BookFeesPendingApprovedBy = d.BookFeesPendingApprovedBy
            .BookFeesPendingMessage = d.BookFeesPendingMessage
            .BookFeesPendingMissingFee = d.BookFeesPendingMissingFee
        End With

    End Sub

    Friend Overloads Function GetLTSFromDTO(ByVal oData As DataTransferObjects.BookFee) As LTS.BookFee
        Return CopyDTOToLinq(oData)
    End Function

    ''' <summary>
    ''' Modified By LVV on 9/28/2017 for v-8.0 TMS365
    ''' Added fields: BookFeesPendingApproved, BookFeesPendingApprovedDate, and BookFeesPendingApprovedBy
    ''' Modified By LVV on 1/9/2018 for v-8.0 TMS365
    '''  Added field: BookFeesPendingMessage
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 7/3/2019
    ''' Added BookFeesPendingMissingFee  
    ''' </remarks>
    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim d = CType(oData, DataTransferObjects.BookFeePending)
        'Create New Record
        Return New LTS.BookFeesPending With {.BookFeesPendingControl = d.BookFeesPendingControl _
            , .BookFeesPendingBookControl = d.BookFeesPendingBookControl _
            , .BookFeesPendingMinimum = d.BookFeesPendingMinimum _
            , .BookFeesPendingValue = d.BookFeesPendingValue _
            , .BookFeesPendingVariable = d.BookFeesPendingVariable _
            , .BookFeesPendingAccessorialCode = d.BookFeesPendingAccessorialCode _
            , .BookFeesPendingAccessorialFeeTypeControl = d.BookFeesPendingAccessorialFeeTypeControl _
            , .BookFeesPendingOverRidden = d.BookFeesPendingOverRidden _
            , .BookFeesPendingVariableCode = d.BookFeesPendingVariableCode _
            , .BookFeesPendingVisible = d.BookFeesPendingVisible _
            , .BookFeesPendingAutoApprove = d.BookFeesPendingAutoApprove _
            , .BookFeesPendingAllowCarrierUpdates = d.BookFeesPendingAllowCarrierUpdates _
            , .BookFeesPendingCaption = d.BookFeesPendingCaption _
            , .BookFeesPendingEDICode = d.BookFeesPendingEDICode _
            , .BookFeesPendingTaxable = d.BookFeesPendingTaxable _
            , .BookFeesPendingIsTax = d.BookFeesPendingIsTax _
            , .BookFeesPendingTaxSortOrder = d.BookFeesPendingTaxSortOrder _
            , .BookFeesPendingBOLText = d.BookFeesPendingBOLText _
            , .BookFeesPendingBOLPlacement = d.BookFeesPendingBOLPlacement _
            , .BookFeesPendingAccessorialFeeAllocationTypeControl = d.BookFeesPendingAccessorialFeeAllocationTypeControl _
            , .BookFeesPendingTarBracketTypeControl = d.BookFeesPendingTarBracketTypeControl _
            , .BookFeesPendingAccessorialFeeCalcTypeControl = d.BookFeesPendingAccessorialFeeCalcTypeControl _
            , .BookFeesPendingModDate = Date.Now _
            , .BookFeesPendingModUser = Parameters.UserName _
            , .BookFeesPendingUpdated = If(d.BookFeesPendingUpdated Is Nothing, New Byte() {}, d.BookFeesPendingUpdated) _
            , .BookFeesPendingApproved = d.BookFeesPendingApproved _
            , .BookFeesPendingApprovedDate = d.BookFeesPendingApprovedDate _
            , .BookFeesPendingApprovedBy = d.BookFeesPendingApprovedBy _
            , .BookFeesPendingMessage = d.BookFeesPendingMessage _
            , .BookFeesPendingMissingFee = d.BookFeesPendingMissingFee}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetBookFeePendingFiltered(Control:=CType(LinqTable, LTS.BookFeesPending).BookFeesPendingControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMasBookDataContext(ConnectionString)
            Try
                Dim source As LTS.BookFeesPending = TryCast(LinqTable, LTS.BookFeesPending)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.BookFeesPendings
                    Where d.BookFeesPendingControl = source.BookFeesPendingControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.BookFeesPendingControl _
                        , .ModDate = d.BookFeesPendingModDate _
                        , .ModUser = d.BookFeesPendingModUser _
                        , .Updated = d.BookFeesPendingUpdated.ToArray}).First

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
    ''' Modified By LVV on 9/28/2017 for v-8.0 TMS365
    '''  Added fields: BookFeesPendingApproved, BookFeesPendingApprovedDate, and BookFeesPendingApprovedBy
    ''' Modified By LVV on 1/9/2018 for v-8.0 TMS365
    '''  Added field: BookFeesPendingMessage
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Modified by RHR for v-8.2 on 7/3/2019
    '''  Added BookFeesPendingMissingFee
    ''' </remarks>
    Friend Function selectDTOData(ByVal d As LTS.BookFeesPending, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.BookFeePending

        Return New DataTransferObjects.BookFeePending With {.BookFeesPendingControl = d.BookFeesPendingControl _
            , .BookFeesPendingBookControl = If(d.BookFeesPendingBookControl.HasValue, d.BookFeesPendingBookControl.Value, 0) _
            , .BookFeesPendingMinimum = If(d.BookFeesPendingMinimum.HasValue, d.BookFeesPendingMinimum.Value, 0) _
            , .BookFeesPendingValue = d.BookFeesPendingValue _
            , .BookFeesPendingVariable = If(d.BookFeesPendingVariable.HasValue, d.BookFeesPendingVariable.Value, 0) _
            , .BookFeesPendingAccessorialCode = If(d.BookFeesPendingAccessorialCode.HasValue, d.BookFeesPendingAccessorialCode.Value, 0) _
            , .BookFeesPendingAccessorialFeeTypeControl = d.BookFeesPendingAccessorialFeeTypeControl _
            , .BookFeesPendingOverRidden = d.BookFeesPendingOverRidden _
            , .BookFeesPendingVariableCode = If(d.BookFeesPendingVariableCode.HasValue, d.BookFeesPendingVariableCode.Value, 0) _
            , .BookFeesPendingVisible = d.BookFeesPendingVisible _
            , .BookFeesPendingAutoApprove = d.BookFeesPendingAutoApprove _
            , .BookFeesPendingAllowCarrierUpdates = d.BookFeesPendingAllowCarrierUpdates _
            , .BookFeesPendingCaption = d.BookFeesPendingCaption _
            , .BookFeesPendingEDICode = d.BookFeesPendingEDICode _
            , .BookFeesPendingTaxable = d.BookFeesPendingTaxable _
            , .BookFeesPendingIsTax = d.BookFeesPendingIsTax _
            , .BookFeesPendingTaxSortOrder = d.BookFeesPendingTaxSortOrder _
            , .BookFeesPendingBOLText = d.BookFeesPendingBOLText _
            , .BookFeesPendingBOLPlacement = d.BookFeesPendingBOLPlacement _
            , .BookFeesPendingAccessorialFeeAllocationTypeControl = d.BookFeesPendingAccessorialFeeAllocationTypeControl _
            , .BookFeesPendingTarBracketTypeControl = d.BookFeesPendingTarBracketTypeControl _
            , .BookFeesPendingAccessorialFeeCalcTypeControl = d.BookFeesPendingAccessorialFeeCalcTypeControl _
            , .BookFeesPendingModDate = d.BookFeesPendingModDate _
            , .BookFeesPendingModUser = d.BookFeesPendingModUser _
            , .BookFeesPendingUpdated = d.BookFeesPendingUpdated.ToArray() _
            , .BookFeesPendingApproved = d.BookFeesPendingApproved _
            , .BookFeesPendingApprovedDate = d.BookFeesPendingApprovedDate _
            , .BookFeesPendingApprovedBy = d.BookFeesPendingApprovedBy _
            , .BookFeesPendingMessage = d.BookFeesPendingMessage _
            , .BookFeesPendingMissingFee = d.BookFeesPendingMissingFee _
            , .Page = page _
            , .Pages = pagecount _
            , .RecordCount = recordcount _
            , .PageSize = pagesize}

    End Function


#End Region

End Class