Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports Ngl.FreightMaster.Data.Utilities
Imports System.Linq.Dynamic
Imports BidTypeEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum
Imports BSCEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidStatusCodeEnum
Imports SerilogTracing

Public Class NGLBidData : Inherits NGLLinkDataBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        'Dim db As New NGLMASIntegrationDataContext(ConnectionString)
        'Me.LinqTable = db.tblBids
        'Me.LinqDB = db
        Me.SourceClass = "NGLBidData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASIntegrationDataContext(ConnectionString)
                _LinqTable = db.tblBids
                _LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

#Region "DEPRECIATED"

    ''' <summary>
    ''' Returns an array of LTS.tblBid data objects representing  
    ''' Next Stop Bids that are Archived and that do not have BidStatusCode Active
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 1/4/17 for v-8.0 Next Stop
    ''' DEPRECIATED By LVV on 10/18/2018
    ''' </remarks>
    ''Public Function GetNSHisoricalBids(ByRef RecordCount As Integer,
    ''                                 Optional ByVal filterWhere As String = "",
    ''                                  Optional ByVal sortExpression As String = "",
    ''                                  Optional ByVal page As Integer = 1,
    ''                                  Optional ByVal pagesize As Integer = 1000,
    ''                                  Optional ByVal skip As Integer = 0,
    ''                                  Optional ByVal take As Integer = 0) As LTS.tblBid()
    ''    throwDepreciatedException("This version of " & buildProcedureName("GetNSHisoricalBids") & " has been depreciated. Please use method GetNSCarHisoricalBids().")
    ''    Return Nothing
    ''End Function


#End Region

    Public Function GetRecord(control As Integer) As LTS.tblBid
        Dim oRet As New LTS.tblBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.tblBids.Where(Function(x) x.BidControl = control).FirstOrDefault()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetRecord"))
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetAll() As List(Of LTS.tblBid)
        Dim oRet As New List(Of LTS.tblBid)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.tblBids.ToList()

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetAll"))
            End Try

            Return oRet

        End Using
    End Function

    Public Function GetPage(ByVal skip As Integer, ByVal take As Integer, ByRef RecordCount As Integer) As List(Of LTS.tblBid)
        Dim oRet As New List(Of LTS.tblBid)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                RecordCount = 0
                Dim intPageCount As Integer = 1
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000
                'db.Log = New DebugTextWriter
                RecordCount = db.tblBids.Count()


                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRet = db.tblBids.Skip(skip).Take(pagesize).ToList()


            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetPage"))
            End Try

            Return oRet

        End Using

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere">'(String.Equals(cmLocalKey, "BookLoadPO", StringComparison.OrdinalIgnoreCase)) Or String.Equals(cmLocalKey, "BookProNumber", StringComparison.OrdinalIgnoreCase)) "</param>
    ''' <param name="sortExpression"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFiltered(ByRef RecordCount As Integer,
                                ByVal filterWhere As String,
                                Optional ByVal sortExpression As String = "",
                                Optional ByVal skip As Integer = 0,
                                Optional ByVal take As Integer = 1000) As List(Of LTS.tblBid)
        Dim oRet As New List(Of LTS.tblBid)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                RecordCount = 0
                Dim intPageCount As Integer = 1
                Dim page As Integer = 1
                Dim pagesize As Integer = 1000
                'db.Log = New DebugTextWriter
                'Dim oQuery = db.cmLocalizeKeyValuePairs.Where(Function(x) sKeys.Contains(x.cmLocalKey))
                Dim oQuery = db.tblBids
                If Not String.IsNullOrWhiteSpace(filterWhere) Then
                    '(String.Equals(cmLocalKey, "BookLoadPO", StringComparison.OrdinalIgnoreCase)) Or String.Equals(cmLocalKey, "BookProNumber", StringComparison.OrdinalIgnoreCase)) "
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If
                RecordCount = oQuery.Count()

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                If Not String.IsNullOrWhiteSpace(sortExpression) Then
                    oRet = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToList()
                Else
                    oRet = oQuery.Skip(skip).Take(pagesize).ToList()
                End If


            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetFiltered"))
            End Try

            Return oRet

        End Using


    End Function

    Public Function Save(oRecord As LTS.tblBid) As LTS.tblBid

        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oTable = db.tblBids
                oRecord.BidModDate = Date.Now()
                oRecord.BidModUser = Me.Parameters.UserName
                oTable.Attach(oRecord, True)
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Save"))
            End Try

            Return oRecord

        End Using
    End Function

    Public Function Insert(oRecord As LTS.tblBid) As LTS.tblBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Perform some validation
                'If String.IsNullOrWhiteSpace(oRecord.cmLocalKey) Then
                '    'E_FieldRequired = 'The '{0}' is required and cannot be empty.
                '    Dim oFaultDetails As New List(Of String) From {"Localization Key"}
                '    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_FieldRequired, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)

                'End If
                'If db.cmLocalizeKeyValuePairs.Any(Function(x) x.cmLocalKey = oRecord.cmLocalKey) Then
                '    'E_CannotSaveKeyValuesAlreadyExist = 'Cannot save changes to {0}. A record with one or more of the following key values already exist: Keys {1} Values {2}.
                '    Dim oFaultDetails As New List(Of String) From {"Localization Data", "Localization Key", oRecord.cmLocalKey}
                '    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyValuesAlreadyExist, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)

                'End If

                Dim oTable = db.tblBids
                oRecord.BidModDate = Date.Now()
                oRecord.BidModUser = Me.Parameters.UserName
                oTable.InsertOnSubmit(oRecord)
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("Insert"))
            End Try

            Return oRecord

        End Using
    End Function

    Public Function Delete(oRecord As LTS.tblBid) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim oTable = db.tblBids
            Try
                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                LinqDB.SubmitChanges()
                blnRet = True
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
        Return blnRet
    End Function

    Public Function Delete(Control As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Dim oTable = db.tblBids
            Try
                Dim oRecord As LTS.tblBid = db.tblBids.Where(Function(x) x.BidControl = Control).FirstOrDefault()
                If (oRecord Is Nothing OrElse oRecord.BidControl = 0) Then Return False
                oTable.Attach(oRecord, True)
                oTable.DeleteOnSubmit(oRecord)
                LinqDB.SubmitChanges()
                blnRet = True
            Catch ex As SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch conflictEx As ChangeConflictException
                Try
                    Dim conflictInfo As ConflictFaultInfo = (New SqlConflictFaultInfo(LinqDB))
                    conflictInfo.LogConflictDetails(conflictInfo.Message, Me.Parameters)
                    Throw New FaultException(Of ConflictFaultInfo)(conflictInfo, New FaultReason("E_DataConflict"))
                Catch ex As FaultException
                    Throw
                Catch ex As Exception
                    Utilities.SaveAppError(ex.Message, Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
                End Try
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Inserts a Bid from the TMS 365 NEXTStop Carrier page
    ''' </summary>
    ''' <param name="oRecord"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by LVV on 2/2/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function InsertNEXTStopBid(oRecord As LTS.tblBid) As LTS.tblBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim dtNow = Date.Now()
                Dim Car As New NGLCarrierData(Me.Parameters)

                Dim oCarrier = Car.GetCarrier(oRecord.BidCarrierControl)


                ''Perform some validation
                'If String.IsNullOrWhiteSpace(oRecord.cmLocalKey) Then
                '    'E_FieldRequired = 'The '{0}' is required and cannot be empty.
                '    Dim oFaultDetails As New List(Of String) From {"Localization Key"}
                '    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_FieldRequired, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)

                'End If
                'If db.cmLocalizeKeyValuePairs.Any(Function(x) x.cmLocalKey = oRecord.cmLocalKey) Then
                '    'E_CannotSaveKeyValuesAlreadyExist = 'Cannot save changes to {0}. A record with one or more of the following key values already exist: Keys {1} Values {2}.
                '    Dim oFaultDetails As New List(Of String) From {"Localization Data", "Localization Key", oRecord.cmLocalKey}
                '    throwFaultException(SqlFaultInfo.FaultInfoMsgs.E_InvalidKeyFilterMetaData, SqlFaultInfo.FaultDetailsKey.E_CannotSaveKeyValuesAlreadyExist, oFaultDetails, SqlFaultInfo.FaultReasons.E_DataValidationFailure, True)

                'End If

                'TODO -- fix the case statment/UOM field to not use strings eventually
                'This will have to happen after we figure out how to do drop down lists in TMS 365
                Dim TotalFuel As Decimal = 0
                Select Case oRecord.BidFuelUOM
                    Case "Flat Rate"
                        TotalFuel = oRecord.BidFuelVariable
                    Case "Rate Per Mile"
                        TotalFuel = Math.Round(oRecord.BidFuelVariable * oRecord.BidTotalMiles, 2)
                    Case "Percentage"
                        TotalFuel = Math.Round(oRecord.BidFuelVariable * oRecord.BidLineHaul, 2)
                End Select

                Dim TotalCost = oRecord.BidLineHaul + TotalFuel

                Dim oTable = db.tblBids
                oRecord.BidFuelTotal = TotalFuel
                oRecord.BidTotalCost = TotalCost
                oRecord.BidCarrierNumber = oCarrier.CarrierNumber
                oRecord.BidCarrierName = oCarrier.CarrierName
                oRecord.BidCarrierSCAC = oCarrier.CarrierSCAC
                oRecord.BidBidTypeControl = BidTypeEnum.NextStop 'Bid Type is NextStop
                oRecord.BidStatusCode = 1     'Bid Status is Active
                oRecord.BidArchived = False
                oRecord.BidPosted = dtNow
                oRecord.BidModDate = dtNow
                oRecord.BidModUser = Me.Parameters.UserName
                oTable.InsertOnSubmit(oRecord)
                db.SubmitChanges()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("InsertNEXTStopBid"))
            End Try

            Return oRecord

        End Using
    End Function


    ''' <summary>
    ''' Insert new bids, fees and charges using carriersByCost data for the provided Load Tender Record
    ''' </summary>
    ''' <param name="carriersByCost"></param>
    ''' <param name="BookRevs"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="SHID"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.3.002 on 10/28/2020
    '''     added logic to include miles in Bid table
    ''' Depricated in v-8.5.3.001 we now use 
    '''     InsertNGLTariffBid365
    ''' </remarks>
    Public Function InsertNGLTariffBid(ByVal carriersByCost As List(Of DTO.CarriersByCost),
                                       ByVal BookRevs As List(Of DTO.BookRevenue),
                                       ByVal LoadTenderControl As Integer,
                                       ByVal SHID As String,
                                       Optional ByRef strMsg As String = "") As Boolean
        throwDepreciatedException("InsertNGLTariffBid is not valid please use InsertNGLTariffBid365")
        Return False

    End Function

    ''' <summary>
    ''' Insert new bids, fees and charges using carriersByCost data for the provided Load Tender Record
    ''' </summary>
    ''' <param name="oResponse"></param>
    ''' <param name="BookRevs"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="SHID"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    '''  Created by RHR for v-8.5.3.001 on 05/25/2022
    '''     replaces InsertNGLTariffBid
    '''     added logic include API message tracking rules
    '''     All of these messges are stored in the tblBidSvcErrs table when a bid is not available the system must create a zero cost record in tblbid for the carrier
    '''     errors/messages/warnings are then inserted into  tblBidSvcErrs using the bidcontrol FK
	'''     use standard localization message formatting where we have Message, Details,  With an additional field For mapping field identification 
	'''     maps to these fields in tblBidSvcErrs
	'''     BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message)
	'''     BidSvcErrVendorErrorMessage = msg.Details
	'''     BidSvcErrFieldName = msg.FieldName
    ''' Modified by RHR for v-8.5.4.001 on 07/07/2023 
    '''     added new logic to assign the Cost Adjustment Type 
    '''     added new logic to calculate the customer upcharge precent
    ''' </remarks>
    Public Function InsertNGLTariffBid365(ByVal oResponse As DTO.CarrierCostResults,
                                       ByVal BookRevs As List(Of DTO.BookRevenue),
                                       ByVal LoadTenderControl As Integer,
                                       ByVal SHID As String,
                                       Optional ByRef strMsg As String = "") As Boolean

        Dim result As Boolean = False

        Using Logger.StartActivity("InsertNGLTariffBid365(LoadTenderControl: {LoadTenderControl}, SHID: {SHID})", LoadTenderControl, SHID)
            Dim carriersByCost As List(Of DTO.CarriersByCost) = oResponse.CarriersByCost
            If LoadTenderControl = 0 Then
                Logger.Warning("Load Tendered Reference cannot be found and is required")
                Dim lDetails As New List(Of String) From {"Load Tendered Reference", " cannot be found and "}
                throwInvalidKeyParentRequiredException(lDetails)
                Return False
            End If


            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Try
                    Dim oLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = LoadTenderControl).FirstOrDefault()
                    Dim oCarrier As New NGLCarrierData(Parameters)
                    Dim blnInvalidData As Boolean = False
                    Dim bStatusCode As BSCEnum = BSCEnum.Active
                    Dim dtNow = Date.Now
                    Dim origST As String = ""
                    Dim destST As String = ""
                    Dim totalPlt As Double = BookRevs.Sum(Function(x) x.BookTotalPL)
                    Dim TotalQty As Double = BookRevs.Sum(Function(x) x.BookTotalCases)
                    Dim totalWgt As Double = BookRevs.Sum(Function(x) x.BookTotalWgt)
                    Dim dblMiles As Double = 0 ' Modified by RHR for v-8.3.002 on 10/28/2020
                    ' Dim sLoadDetails = String.Format("Using Tariffs,  LoadTenderControl {0}, SHID {1}, Order No. {2}", LoadTenderControl, SHID, BookRevs(0).BookCarrOrderNumber)

                    If oResponse Is Nothing OrElse oResponse.CarriersByCost Is Nothing OrElse oResponse.CarriersByCost.Count() < 1 Then
                        oResponse.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoTariffsFound)
                        blnInvalidData = True
                        Logger.Warning("No Tariffs Found for LoadTenderControl {LoadTenderControl}, SHID {SHID}, Order No. {BookCarrOrderNumber}", LoadTenderControl, SHID, BookRevs(0).BookCarrOrderNumber)
                    End If
                    If Not BookRevs Is Nothing AndAlso BookRevs.Count > 0 Then
                        origST = BookRevs(0).BookOrigState
                        destST = BookRevs(0).BookDestState
                        dblMiles = BookRevs.Sum(Function(x) x.BookMilesFrom) ' Modified by RHR for v-8.3.002 on 10/28/2020
                        Logger.Information("LoadTenderControl {LoadTenderControl}, SHID {SHID}, Order No. {BookCarrOrderNumber} calculated dblMiles {Miles}", LoadTenderControl, SHID, BookRevs(0).BookCarrOrderNumber, dblMiles)
                        'Else
                        '    oResponse.AddMessage(DTO.CarrierCostResults.MessageEnum.M_NoOrdersFound)
                        '    blnInvalidData = True
                        Logger.Information("Checking to see if any book fees exist that are not associated with tblLECarrAccessorials")
                        For Each bookRev In BookRevs
                            For Each bookfee In bookRev.BookFees
                                If Not String.IsNullOrWhiteSpace(bookfee.BookFeesAccessorialCode) Then

                                    Logger.Information("Validating Accessorials for {AccessorialCode}", bookfee.BookFeesAccessorialCode)
                                    
                                    For Each carrier In carriersByCost

                                        Dim leCarrierAccessorials As New NGLLECarrierAccessorialData(Parameters)
                                        Dim foundAccessorials = leCarrierAccessorials.GetLECarrierAccessorialsByCarrierControl(carrier.CarrierControl)
                                        
                                        If foundAccessorials IsNot Nothing AndAlso foundAccessorials.Any() Then
                                            Dim found = foundAccessorials.Any(Function(x) x.LECAAccessorialCode = bookfee.BookFeesAccessorialCode)
                                        
                                            If Not found Then
                                                Logger.Information("Invalid book fee found for carrier {CarrierName}, Fee: {BookFee}", oResponse.CarriersByCost(0).CarrierName, bookfee)
                                            
                                                oResponse.AddMessage(DTO.CarrierCostResults.MessageEnum.None,
                                                                     New DTO.NGLMessage($"BookFee {bookfee.BookFeesAccessorialCode} is not supported by this carrier.  If this is a mistake, update the Carrier to include"))
                                                
                                                blnInvalidData = True
                                            End If
                                        End If
                                    Next
                                End If
                            Next
                        Next








                    End If
                    If Not blnInvalidData AndAlso Not oResponse.postMessagesOnly Then

                        Dim lCarrier As List(Of Integer) = New NGLSecurityDataProvider(Parameters).RestrictedCarriersForSalesReps()
                        For Each q In carriersByCost
                            Logger.Information("Valid Data - Iterating through carrier: {CarrierName}, {@Carrier}", q.CarrierName, q)
                            If ((Not lCarrier Is Nothing) AndAlso (lCarrier.Count() > 0) AndAlso (Not lCarrier.Contains(q.CarrierControl))) Then
                                'skip
                                Logger.Information("Carrier {CarrierName} is not in the list of restricted carriers for the user", q.CarrierName)
                                Continue For
                            End If
                            'get the carrier SCAC
                            Dim sSCAC As String = ""
                            Dim c = oCarrier.getCarrierNameNumberSCAC(q.CarrierControl)
                            If Not c Is Nothing AndAlso c.Count > 0 Then sSCAC = c("CarrierSCAC")

                            Dim strErrs = q.concatMessage()

                            bStatusCode = BSCEnum.Active
                            Dim bArchived = 0
                            Dim blnInsertErrMsg As Boolean = False
                            Dim strErrMsg = ""

                            'We don't want zero cost bids to clutter up the NEXTStop screen so if a quote had errors that result in a zero
                            'cost bid we set the Archived Flag to True and set the Status Code to BidError (NS screen only shows unarchived active bids)
                            'From CarriersByCost.vb definition -- show alert icon when cost is zero or when user cannot select the record
                            If q.HasInfo AndAlso q.HasMessages Then
                                bStatusCode = BSCEnum.BidError
                                bArchived = 1
                                strErrMsg = q.concatMessage()
                                If Not String.IsNullOrWhiteSpace(strErrMsg) Then blnInsertErrMsg = True
                            End If
                            Dim dt As DateTime = DateTime.Now
                            Dim TotalCost As Decimal = 0
                            TotalCost = q.CarrierCost
                            Dim iTotalAdjs As Integer = 0
                            Dim dAdjustmentAmount As Decimal = 0
                            Dim Adjs As New List(Of LTS.tblBidCostAdj)
                            Dim dBidCostAdjRate As Decimal = 0
                            Dim sBidCostAdjDescCode As String = ""

                            Logger.Information("Setting TotalCost to {TotalCost}", TotalCost)
                            'Begin Modified by RHR for v-8.5.4.001 on 07/07/2023
                            '   added new logic to assign the Cost Adjustment Type
                            '   added new logic to calculate the customer upcharge precent
                            Dim iCompControl As Integer = If(oLoadTender.LTBookCustCompControl, 0)
                            Dim dblCarrierCostUpcharge As Double = NGLLegalEntityCarrierObjData.GetCarrierUpliftValue(q.CarrierControl, iCompControl)
                            Logger.Information("Setting dblCarrierCostUpcharge to {dblCarrierCostUpcharge} for Carrier {CarrierName}", dblCarrierCostUpcharge, q.CarrierName)
                            Dim decBidCustLineHaul As Decimal = 0
                            Dim decBidCustTotalCost As Decimal = 0
                            If (Not q.postMessagesOnly) Then

                                If (q.CarrierPltRate > 0) Then
                                    dBidCostAdjRate = q.CarrierPltRate
                                    sBidCostAdjDescCode = "PerPlt"
                                ElseIf (q.CarrierCaseRate > 0) Then
                                    dBidCostAdjRate = q.CarrierCaseRate
                                    sBidCostAdjDescCode = "PerCase"
                                ElseIf (q.CarrierCubeRate > 0) Then
                                    dBidCostAdjRate = q.CarrierCubeRate
                                    sBidCostAdjDescCode = "PerCube"
                                ElseIf (q.CarrierLbsRate > 0) Then
                                    dBidCostAdjRate = q.CarrierLbsRate
                                    sBidCostAdjDescCode = "PerLbs"
                                ElseIf (q.CarrierMileRate > 0) Then
                                    dBidCostAdjRate = q.CarrierMileRate
                                    sBidCostAdjDescCode = "PerMile"
                                End If

                                Logger.Information("{CarrierName} Using {sBidCostAdjDescCode} Rate of {dBidCostAdjRate}", q.CarrierName, sBidCostAdjDescCode, dBidCostAdjRate)
                                Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = q.ClassTypeName,
                                                                               .BidCostAdjWeight = totalWgt,
                                                                               .BidCostAdjAmount = q.BookRevLineHaul,
                                                                               .BidCostAdjRate = dBidCostAdjRate,
                                                                               .BidCostAdjDescCode = sBidCostAdjDescCode,
                                                                               .BidCostAdjDesc = "Carrier Line Haul",
                                                                               .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CarrierLineHaul,
                                                                               .BidCostAdjModDate = dt,
                                                                               .BidCostAdjModUser = Me.Parameters.UserName})
                                iTotalAdjs += 1
                                decBidCustLineHaul = q.BookRevLineHaul + (q.BookRevLineHaul * dblCarrierCostUpcharge)
                                Logger.Information("Adding Carrier Line Haul Adjustment. Setting decBidCustLineHaul to {CustomerLineHaul} for Carrier {CarrierName}", decBidCustLineHaul, q.CarrierName)


                                'Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = q.ClassTypeName,
                                '                                               .BidCostAdjWeight = totalWgt,
                                '                                               .BidCostAdjAmount = decBidCustLineHaul,
                                '                                               .BidCostAdjRate = 1,
                                '                                               .BidCostAdjDescCode = "UPLF",
                                '                                               .BidCostAdjDesc = "Customer Line Haul",
                                '                                               .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.CustomerLineHaul,
                                '                                               .BidCostAdjModDate = dt,
                                '                                               .BidCostAdjModUser = Me.Parameters.UserName})
                                'iTotalAdjs += 1
                                'Logger.Information("Adding Customer Line Haul Adjustment. Setting decBidCustLineHaul to {CustomerLineHaul} for Carrier {CarrierName}", decBidCustLineHaul, q.CarrierName)


                                If (q.FuelCost > 0) Then
                                    Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = q.ClassTypeName,
                                                                    .BidCostAdjWeight = totalWgt,
                                                                    .BidCostAdjAmount = q.FuelCost,
                                                                    .BidCostAdjRate = 1,
                                                                    .BidCostAdjDescCode = "FSC",
                                                                    .BidCostAdjDesc = "Fuel",
                                                                    .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Fuel,
                                                                    .BidCostAdjModDate = dt,
                                                                    .BidCostAdjModUser = Me.Parameters.UserName})
                                    iTotalAdjs += 1
                                    dAdjustmentAmount += q.FuelCost
                                    Logger.Information("Adding Fuel Adjustment. Setting dAdjustmentAmount to {FuelCost} for Carrier {CarrierName}", q.FuelCost, q.CarrierName)
                                End If


                                If (q.StopCharges > 0) Then
                                    Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = q.ClassTypeName,
                                                                    .BidCostAdjWeight = totalWgt,
                                                                    .BidCostAdjAmount = q.StopCharges,
                                                                    .BidCostAdjRate = 1,
                                                                    .BidCostAdjDescCode = "Stop",
                                                                    .BidCostAdjDesc = "Stop Charges",
                                                                    .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Accessorial,
                                                                    .BidCostAdjModDate = dt,
                                                                    .BidCostAdjModUser = Me.Parameters.UserName})
                                    iTotalAdjs += 1
                                    dAdjustmentAmount += q.StopCharges
                                    Logger.Information("Adding Stop Charges Adjustment. Setting dAdjustmentAmount to {StopCharges} for Carrier {CarrierName}", q.StopCharges, q.CarrierName)

                                End If
                                If (q.PickCharges > 0) Then
                                    Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = q.ClassTypeName,
                                                                    .BidCostAdjWeight = totalWgt,
                                                                    .BidCostAdjAmount = q.PickCharges,
                                                                    .BidCostAdjRate = 1,
                                                                    .BidCostAdjDescCode = "Pick",
                                                                    .BidCostAdjDesc = "Pick Charges",
                                                                    .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Accessorial,
                                                                    .BidCostAdjModDate = dt,
                                                                    .BidCostAdjModUser = Me.Parameters.UserName})
                                    iTotalAdjs += 1
                                    dAdjustmentAmount += q.PickCharges
                                    Logger.Information("Adding Pick Charges Adjustment. Setting dAdjustmentAmount to {PickCharges} for Carrier {CarrierName}", q.PickCharges, q.CarrierName)

                                End If
                                If (q.OtherFees > 0) Then
                                    Adjs.Add(New LTS.tblBidCostAdj With {.BidCostAdjFreightClass = q.ClassTypeName,
                                                                    .BidCostAdjWeight = totalWgt,
                                                                    .BidCostAdjAmount = q.OtherFees,
                                                                    .BidCostAdjRate = 1,
                                                                    .BidCostAdjDescCode = "Other",
                                                                    .BidCostAdjDesc = "Other Fees",
                                                                    .BidCostAdjTypeControl = NGLLookupDataProvider.CostAdjType.Accessorial,
                                                                    .BidCostAdjModDate = dt,
                                                                    .BidCostAdjModUser = Me.Parameters.UserName})
                                    iTotalAdjs += 1
                                    dAdjustmentAmount += q.OtherFees

                                    Logger.Information("Adding Other Fees Adjustment. Setting dAdjustmentAmount to {OtherFees} for Carrier {CarrierName}", q.OtherFees, q.CarrierName)
                                End If

                                decBidCustTotalCost = (TotalCost - q.BookRevLineHaul) + decBidCustLineHaul
                                Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                            .BidBidTypeControl = BidTypeEnum.NGLTariff,
                                                            .BidCarrierControl = q.CarrierControl,
                                                            .BidCarrierNumber = q.CarrierNumber,
                                                            .BidCarrierName = q.CarrierName,
                                                            .BidCarrierSCAC = sSCAC,
                                                            .BidSHID = SHID,
                                                            .BidTotalCost = TotalCost,
                                                            .BidLineHaul = q.BookRevLineHaul,   'TODO: figure out how to get the line haul
                                                            .BidFuelTotal = q.FuelCost,
                                                            .BidFuelVariable = 0,
                                                            .BidFuelUOM = "Flat Rate",
                                                            .BidOrigState = origST,
                                                            .BidDestState = destST,
                                                            .BidPosted = dtNow,
                                                            .BidStatusCode = bStatusCode,
                                                            .BidArchived = bArchived,
                                                            .BidMode = q.ModeTypeName,
                                                            .BidErrorCount = q.Messages.Count(),
                                                            .BidErrors = Left(strErrs, 3999),
                                                            .BidWarnings = "", ' ADD CODE TO process warnings
                                                            .BidInfos = "", 'add code to process info messages
                                                            .BidInterLine = q.InterlinePoint, 'or is it q.BookAllowInterlinePoints ??
                                                            .BidQuoteNumber = Left(q.BookCarrTarName & dt.Month.ToString & dt.Day.ToString & dt.Year.ToString & dt.Hour.ToString & dt.Minute.ToString & dt.Second.ToString, 100), ' & Ngl.Core.Utility.DataTransformation,
                                                            .BidTransitTime = q.CarrTarEquipMatMaxDays,
                                                            .BidDeliveryDate = q.BookExpDelDateTime,
                                                            .BidQuoteDate = dt,
                                                            .BidTotalWeight = totalWgt,
                                                            .BidDetailTotal = 0,
                                                            .BidDetailTransitTime = q.CarrTarEquipMatMaxDays,
                                                            .BidAdjustments = dAdjustmentAmount, 'difference between line haul and total cost 
                                                            .BidAdjustmentCount = iTotalAdjs,
                                                            .BidVendor = sSCAC,
                                                            .BidContractID = Left(q.BookCarrTarName & "-" & q.BookCarrTarRevisionNumber.ToString(), 50),
                                                            .BidServiceType = q.ModeTypeName,
                                                            .BidTotalPlts = CInt(totalPlt),
                                                            .BidTotalQty = TotalQty,
                                                            .BidBookCarrTarEquipMatControl = q.BookCarrTarEquipMatControl,
                                                            .BidBookCarrTarEquipControl = q.BookCarrTarEquipControl,
                                                            .BidBookModeTypeControl = q.BookModeTypeControl,
                                                            .BidCustLineHaul = decBidCustLineHaul,
                                                            .BidCustTotalCost = decBidCustTotalCost,
                                                            .BidModDate = dtNow,
                                                            .BidModUser = Me.Parameters.UserName,
                                                            .BidTotalMiles = dblMiles} ' Modified by RHR for v-8.3.002 on 10/28/2020

                                'End Modified by RHR for v-8.5.4.001 on 07/07/2023
                                Dim oTable = db.tblBids
                                oTable.InsertOnSubmit(oBid)
                                db.SubmitChanges()
                                Dim bidCtrl = oBid.BidControl

                                If (Not Adjs Is Nothing AndAlso Adjs.Count() > 0) Then
                                    Dim oT = db.tblBidCostAdjs
                                    For Each adj In Adjs
                                        adj.BidCostAdjBidControl = bidCtrl
                                        oT.InsertOnSubmit(adj)
                                    Next
                                    db.SubmitChanges()
                                End If
                                'these messages one carrier or bid
                                'Modified by RHR for v-8.5.4.006 on 04/15/24 added logic for Booking Profile fees errors
                                If Not q.Messages Is Nothing AndAlso q.Messages.Count > 0 Then
                                    For Each m In q.Messages
                                        Dim sBidSvcErrErrorMessage As String = ""
                                        Dim sBidSvcErrMessage As String = ""
                                        Dim sBidSvcErrFieldName As String = ""
                                        Dim eMsg = q.getMessageEnumFromString(m.Key)
                                        Dim lMsgs As List(Of DTO.NGLMessage) = m.Value
                                        If eMsg <> DTO.CarriersByCost.MessageEnum.None Then
                                            sBidSvcErrErrorMessage = DTO.CarriersByCost.getMessageNotLocalizedString(eMsg, m.Key)
                                        End If
                                        If String.IsNullOrWhiteSpace(sBidSvcErrErrorMessage) Then sBidSvcErrErrorMessage = "Read Carrier Tariff Message"
                                        Dim oTbl = db.tblBidSvcErrs
                                        If Not lMsgs Is Nothing AndAlso lMsgs.Count() > 0 Then
                                            For Each oMsg As DTO.NGLMessage In lMsgs.Where(Function(x) x.bLogged = False)
                                                oMsg.bLogged = True
                                                Dim oBidErr As New LTS.tblBidSvcErr With {
                                                .BidSvcErrBidControl = bidCtrl,
                                                .BidSvcErrErrorMessage = sBidSvcErrErrorMessage,
                                                .BidSvcErrMessage = oMsg.Message,
                                                .BidSvcErrFieldName = oMsg.Details,
                                                .BidSvcErrModDate = dtNow,
                                                .BidSvcErrModUser = Me.Parameters.UserName}
                                                oTbl.InsertOnSubmit(oBidErr)
                                            Next
                                        Else
                                            Dim oBidErr As New LTS.tblBidSvcErr With {
                                                .BidSvcErrBidControl = bidCtrl,
                                                .BidSvcErrErrorMessage = sBidSvcErrErrorMessage,
                                                .BidSvcErrMessage = sBidSvcErrErrorMessage,
                                                .BidSvcErrFieldName = sBidSvcErrErrorMessage,
                                                .BidSvcErrModDate = dtNow,
                                                .BidSvcErrModUser = Me.Parameters.UserName}
                                            oTbl.InsertOnSubmit(oBidErr)
                                        End If
                                    Next
                                End If
                            Else
                                Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                                .BidBidTypeControl = BidTypeEnum.NGLTariff,
                                                                .BidCarrierControl = q.CarrierControl,
                                                                .BidCarrierNumber = q.CarrierNumber,
                                                                .BidCarrierName = q.CarrierName,
                                                                .BidCarrierSCAC = sSCAC,
                                                                .BidSHID = SHID,
                                                                .BidTotalCost = 0,
                                                                .BidLineHaul = 0,
                                                                .BidFuelTotal = 0,
                                                                .BidFuelVariable = 0,
                                                                .BidFuelUOM = "NA",
                                                                .BidOrigState = Left(origST, 2),
                                                                .BidDestState = Left(destST, 2),
                                                                .BidPosted = dtNow,
                                                                .BidStatusCode = bStatusCode,
                                                                .BidArchived = False,
                                                                .BidMode = "NA",
                                                                .BidErrorCount = q.Messages.Count(),
                                                                .BidErrors = Left(q.concatMessage(), 3999),
                                                                .BidVendor = "",
                                                                .BidTotalPlts = 0,
                                                                .BidTotalQty = 0,
                                                                .BidCustLineHaul = 0,
                                                                .BidCustTotalCost = 0,
                                                                .BidModDate = dtNow,
                                                                .BidModUser = Me.Parameters.UserName}

                                Dim oTable = db.tblBids
                                oTable.InsertOnSubmit(oBid)
                                db.SubmitChanges()
                                Dim bidCtrl = oBid.BidControl

                                'Modified by RHR for v-8.5.4.006 on 04/15/24 added logic for Booking Profile fees errors
                                If Not q.Messages Is Nothing AndAlso q.Messages.Count > 0 Then
                                    For Each m In q.Messages
                                        Dim sBidSvcErrErrorMessage As String = ""
                                        Dim sBidSvcErrMessage As String = ""
                                        Dim sBidSvcErrFieldName As String = ""
                                        Dim eMsg = q.getMessageEnumFromString(m.Key)
                                        Dim lMsgs As List(Of DTO.NGLMessage) = m.Value
                                        If eMsg <> DTO.CarriersByCost.MessageEnum.None Then
                                            sBidSvcErrErrorMessage = DTO.CarriersByCost.getMessageNotLocalizedString(eMsg, m.Key)
                                        End If
                                        If String.IsNullOrWhiteSpace(sBidSvcErrErrorMessage) Then sBidSvcErrErrorMessage = "Read Carrier Tariff Message"
                                        Dim oTbl = db.tblBidSvcErrs
                                        If Not lMsgs Is Nothing AndAlso lMsgs.Count() > 0 Then
                                            For Each oMsg As DTO.NGLMessage In lMsgs.Where(Function(x) x.bLogged = False)
                                                oMsg.bLogged = True
                                                Dim oBidErr As New LTS.tblBidSvcErr With {
                                                .BidSvcErrBidControl = bidCtrl,
                                                .BidSvcErrErrorMessage = sBidSvcErrErrorMessage,
                                                .BidSvcErrMessage = oMsg.Message,
                                                .BidSvcErrFieldName = oMsg.Details,
                                                .BidSvcErrModDate = dtNow,
                                                .BidSvcErrModUser = Me.Parameters.UserName}
                                                oTbl.InsertOnSubmit(oBidErr)
                                            Next
                                        Else
                                            Dim oBidErr As New LTS.tblBidSvcErr With {
                                                .BidSvcErrBidControl = bidCtrl,
                                                .BidSvcErrErrorMessage = sBidSvcErrErrorMessage,
                                                .BidSvcErrMessage = sBidSvcErrErrorMessage,
                                                .BidSvcErrFieldName = sBidSvcErrErrorMessage,
                                                .BidSvcErrModDate = dtNow,
                                                .BidSvcErrModUser = Me.Parameters.UserName}
                                            oTbl.InsertOnSubmit(oBidErr)
                                        End If

                                        db.SubmitChanges()
                                    Next
                                End If
                            End If
                        Next
                    Else
                        ' post messages only we need a place holder as the bid
                        'these messages are for the entire rate request not just one carrier or bid
                        Dim lMessages As List(Of DTO.NGLMessage) = oResponse.GetMessages().Where(Function(x) x.bLogged = False).ToList()
                        If Not lMessages Is Nothing AndAlso lMessages.Count() > 0 Then
                            Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                                .BidBidTypeControl = BidTypeEnum.JTSAPI,
                                                                .BidCarrierControl = oLoadTender.LTCarrierControl,
                                                                .BidCarrierNumber = oLoadTender.LTCarrierNumber,
                                                                .BidCarrierName = oLoadTender.LTCarrierName,
                                                                .BidCarrierSCAC = oLoadTender.LTCarrierSCAC,
                                                                .BidSHID = SHID,
                                                                .BidTotalCost = 0,
                                                                .BidLineHaul = 0,
                                                                .BidFuelTotal = 0,
                                                                .BidFuelVariable = 0,
                                                                .BidFuelUOM = "NA",
                                                                .BidOrigState = Left(origST, 2),
                                                                .BidDestState = Left(destST, 2),
                                                                .BidPosted = dtNow,
                                                                .BidStatusCode = bStatusCode,
                                                                .BidArchived = False,
                                                                .BidMode = "NA",
                                                                .BidErrorCount = lMessages.Count(),
                                                                .BidErrors = Left(oResponse.concateMessages, 3999),
                                                                .BidVendor = "",
                                                                .BidTotalPlts = oLoadTender.LTBookTotalPL,
                                                                .BidTotalQty = oLoadTender.LTBookTotalCases,
                                                                .BidCustLineHaul = 0,
                                                                .BidCustTotalCost = 0,
                                                                .BidModDate = dtNow,
                                                                .BidModUser = Me.Parameters.UserName}

                            Dim oTable = db.tblBids
                            oTable.InsertOnSubmit(oBid)
                            db.SubmitChanges()
                            Dim bidCtrl = oBid.BidControl

                            If (Not lMessages Is Nothing AndAlso lMessages.Count > 0) Then
                                Dim oTbl = db.tblBidSvcErrs
                                For Each msg In lMessages
                                    msg.bLogged = True
                                    'Modified by RHR for v-8.5.4.006 on 04/24/2024 added logic to truncte the message details in the BidSvcErrVendorErrorMessage field to 499 characters
                                    Dim oBidErr As New LTS.tblBidSvcErr With {
                                        .BidSvcErrBidControl = bidCtrl,
                                        .BidSvcErrErrorMessage = getLocalizedString(msg.MessageLocalCode, msg.Message),
                                        .BidSvcErrVendorErrorMessage = Left(msg.Details, 499),
                                        .BidSvcErrFieldName = msg.FieldName,
                                        .BidSvcErrModDate = dtNow,
                                        .BidSvcErrModUser = Me.Parameters.UserName}
                                    oTbl.InsertOnSubmit(oBidErr)
                                Next
                            End If

                            db.SubmitChanges()
                        End If

                    End If
                    Return True
                Catch ex As Exception
                    Logger.Error(ex, "Error in InsertNGLTariffBid365")
                    'ManageLinqDataExceptions(ex, buildProcedureName("InsertNGLTariffBid365"))
                Finally
                    Try
                        'save errors and logs
                        Logger.Information("Saving Load Tender Carrier Cost Messages for LoadTenderControl: {LoadTenderControl}, Type: NGLLoadTenderType.RateQuote", LoadTenderControl)
                        NGLLoadTenderObjData.saveLoadTenderCarrierCostMessages(LoadTenderControl, NGLLoadTenderTypes.RateQuote, oResponse)
                    Catch ex As Exception
                        'do nothing on error
                        Logger.Error(ex, "Error in InsertNGLTariffBid365 saving Load Tender Carrier Cost Messages")
                    End Try
                End Try
                Return False
            End Using
        End Using

    End Function




    ''' <summary>
    ''' Create a tblBid record using the CarriersByCost results.  Returns zero on failure or a valid bidControl number
    ''' </summary>
    ''' <param name="carriersByCost"></param>
    ''' <param name="BookRevs"></param>
    ''' <param name="LoadTenderControl"></param>
    ''' <param name="oSpotRate"></param>
    ''' <param name="strMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2 on 12/02/2018
    '''   creates a new bid that represents the users spot rate 
    '''   for v-8.2 the BidBidTypeControl = BidTypeEnum.Spot
    ''' Modified By LVV on 7/2/19 for v-8.2
    '''  Fixed it so it would allow assigning a Zero Cost Carrier via Spot Rate
    ''' </remarks>
    Public Function InsertNGLSpotRate(ByVal carriersByCost As List(Of DTO.CarriersByCost),
                                      ByVal BookRevs As List(Of DTO.BookRevenue),
                                      ByVal LoadTenderControl As Integer,
                                      ByVal oSpotRate As LTS.vBookSpotRateData,
                                      Optional ByRef strMsg As String = "") As Integer
        Dim iRet As Integer = 0
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oCarrier As New NGLCarrierData(Parameters)
                Dim dtNow = Date.Now
                Dim origST As String = ""
                Dim destST As String = ""
                Dim totalWgt As Double = 0
                Dim totalPlts As Double = 0
                Dim totalCubes As Integer = 0
                Dim totalCases As Integer = 0
                Dim totalMiles As Double? = 0
                Dim totalStops As Integer = 0
                Dim SHID As String = ""
                If Not BookRevs Is Nothing AndAlso BookRevs.Count > 0 Then
                    origST = BookRevs.OrderBy(Function(x) x.BookPickupStopNumber).Select(Function(x) x.BookOrigState).FirstOrDefault()
                    destST = BookRevs.OrderByDescending(Function(x) x.BookStopNo).Select(Function(x) x.BookOrigState).FirstOrDefault()
                    totalWgt = BookRevs.Sum(Function(x) x.BookTotalWgt)
                    totalPlts = BookRevs.Sum(Function(x) x.BookTotalPL)
                    totalCubes = BookRevs.Sum(Function(x) x.BookTotalCube)
                    totalCases = BookRevs.Sum(Function(x) x.BookTotalCases)
                    totalMiles = BookRevs.Sum(Function(x) x.BookMilesFrom)
                    'not sure how the group by count works so for now we just set the total stops to the number of orders
                    'totalStops = BookRevs.GroupBy(Function(x) x.BookDestName).Count()
                    totalStops = BookRevs.Count()
                    SHID = BookRevs(0).BookSHID
                End If

                'Modified By LVV on 7/2/19 for v-8.2
                Dim q As New DTO.CarriersByCost()
                Dim sSCAC As String = ""
                Dim sCarrierName As String = ""
                Dim sCarrierNo As String = ""
                Dim strErrs As String = ""
                Dim intCarrierNo As Integer = 0

                If carriersByCost Is Nothing OrElse carriersByCost.Count() < 1 Then
                    If BookRevs(0).BookCarrierControl < 1 Then
                        strMsg = "Insert Spot Rate Failure: No Rate or Carrier was Available"
                        Return iRet
                    End If
                    'Verify that the user actually entered in 0 for the LineHaul, otherwise return the error that says no carriers are available
                    If oSpotRate.BookSpotRateTotalLineHaulCost > 0 Then
                        strMsg = "Insert Spot Rate Failure: No Rate or Carrier was Available"
                        Return iRet
                    End If
                    'get the carrier SCAC, Name, and Number
                    Dim c = oCarrier.getCarrierNameNumberSCAC(BookRevs(0).BookCarrierControl)
                    If Not c Is Nothing AndAlso c.Count > 0 Then
                        sSCAC = c("CarrierSCAC")
                        sCarrierName = c("CarrierName")
                        sCarrierNo = c("CarrierNumber")
                        Integer.TryParse(sCarrierNo, intCarrierNo)
                    End If
                    'Create an object for the zero cost carrier
                    With q
                        .CarrierControl = BookRevs(0).BookCarrierControl
                        .BookRevLineHaul = 0
                        .FuelCost = 0
                        .ModeTypeName = "Road"
                        .Messages = Nothing
                        .InterlinePoint = False
                        .BookExpDelDateTime = Date.Now.AddDays(30)
                        .BookCarrTarEquipMatControl = 0
                        .BookCarrTarEquipControl = 0
                        .BookModeTypeControl = 3
                        .CarrierNumber = intCarrierNo
                        .CarrierName = sCarrierName
                    End With
                Else
                    q = carriersByCost(0) 'for spot rates we just process the first value returned
                    Dim c = oCarrier.getCarrierNameNumberSCAC(q.CarrierControl) 'get the carrier SCAC
                    If Not c Is Nothing AndAlso c.Count > 0 Then sSCAC = c("CarrierSCAC")
                    strErrs = q.concatMessage()
                End If

                Dim bStatusCode = BSCEnum.Active
                Dim bArchived = 0
                Dim blnInsertErrMsg As Boolean = False
                Dim strErrMsg = ""

                'Removed by RHR on for v-8.2 on 12/01/2018  
                ' this should not be needed for user entered spot rates
                'some testing may be needed
                '****************************************************************************
                ''We don't want zero cost bids to clutter up the NEXTStop screen so if a quote had errors that result in a zero
                ''cost bid we set the Archived Flag to True and set the Status Code to BidError (NS screen only shows unarchived active bids)
                ''From CarriersByCost.vb definition -- show alert icon when cost is zero or when user cannot select the record
                'If q.HasInfo AndAlso q.HasMessages Then
                '    bStatusCode = BSCEnum.BidError
                '    bArchived = 1
                '    strErrMsg = q.concatMessage()
                '    If Not String.IsNullOrWhiteSpace(strErrMsg) Then blnInsertErrMsg = True
                'End If
                '*******************************************************************************

                Dim TotalCost As Decimal = q.BookRevLineHaul + q.FuelCost

                Dim oBid As New LTS.tblBid With {.BidLoadTenderControl = LoadTenderControl,
                                                    .BidQuoteNumber = dtNow.Year.ToString() & dtNow.Month.ToString() & dtNow.Day.ToString() & dtNow.Hour.ToString() & dtNow.Minute.ToString() & dtNow.Second.ToString(),
                                                    .BidBidTypeControl = BidTypeEnum.Spot,
                                                    .BidCarrierControl = q.CarrierControl,
                                                    .BidCarrierNumber = q.CarrierNumber,
                                                    .BidCarrierName = q.CarrierName,
                                                    .BidVendor = sSCAC,
                                                    .BidCarrierSCAC = sSCAC,
                                                    .BidSHID = SHID,
                                                    .BidTotalCost = TotalCost,
                                                    .BidLineHaul = q.BookRevLineHaul,   'TODO: figure out how to get the line haul
                                                    .BidFuelTotal = q.FuelCost,
                                                    .BidFuelVariable = 0,
                                                    .BidFuelUOM = "Spot Rate",
                                                    .BidOrigState = origST,
                                                    .BidDestState = destST,
                                                    .BidPosted = dtNow,
                                                    .BidStatusCode = bStatusCode,
                                                    .BidArchived = bArchived,
                                                    .BidMode = q.ModeTypeName,
                                                    .BidErrorCount = If(q.Messages Is Nothing, 0, q.Messages.Count),
                                                    .BidErrors = Left(strErrs, 3999),
                                                    .BidInterLine = q.InterlinePoint, 'or is it q.BookAllowInterlinePoints ??
                                                    .BidDeliveryDate = q.BookExpDelDateTime,
                                                    .BidQuoteDate = dtNow,
                                                    .BidTotalWeight = totalWgt,
                                                    .BidTotalPlts = totalPlts,
                                                    .BidTotalQty = totalCases,
                                                    .BidTotalMiles = totalMiles,
                                                    .BidTotalStops = totalStops,
                                                    .BidBookCarrTarEquipMatControl = q.BookCarrTarEquipMatControl,
                                                    .BidBookCarrTarEquipControl = q.BookCarrTarEquipControl,
                                                    .BidBookModeTypeControl = q.BookModeTypeControl,
                                                    .BidModDate = dtNow,
                                                    .BidModUser = Me.Parameters.UserName}
                Dim oTable = db.tblBids
                oTable.InsertOnSubmit(oBid)
                db.SubmitChanges()
                Dim bidCtrl = oBid.BidControl
                If blnInsertErrMsg Then
                    Dim oTbl = db.tblBidSvcErrs
                    Dim oBidErr As New LTS.tblBidSvcErr With {
                        .BidSvcErrBidControl = bidCtrl,
                        .BidSvcErrErrorMessage = strErrMsg,
                        .BidSvcErrModDate = dtNow,
                        .BidSvcErrModUser = Me.Parameters.UserName}
                    oTbl.InsertOnSubmit(oBidErr)
                    db.SubmitChanges()
                End If
                Return bidCtrl
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertNGLSpotRate"))
            End Try
            Return iRet
        End Using
    End Function


    ''' <summary>
    ''' Returns tblBidCostAdj() filtered by filterWhere if provided and
    ''' sorted by sortExpression if provided
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 3/1/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function GetBidCostAdjustments(ByRef RecordCount As Integer,
                                          Optional ByVal filterWhere As String = "",
                                          Optional ByVal sortExpression As String = "",
                                          Optional ByVal page As Integer = 1,
                                          Optional ByVal pagesize As Integer = 1000,
                                          Optional ByVal skip As Integer = 0,
                                          Optional ByVal take As Integer = 0) As LTS.tblBidCostAdj()
        Dim oRetData As LTS.tblBidCostAdj()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim intPageCount As Integer = 1

                Dim oQuery = From t In db.tblBidCostAdjs
                             Select t
                '"(CarrTarDiscountMinValue < 75) AND (CarrTarDiscountWgtLimit > 50)"
                '"(BidStatus = 1) AND (BidCarrierControl = CarrierControl)"
                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If

                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()

                Return oRetData

            Catch ex As Exception
                Logger.Error(ex, "Error in GetBidCostAdjustments")
                ManageLinqDataExceptions(ex, buildProcedureName("GetBidCostAdjustments"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' This overload uses the AllFilters model data to get all Bid Cost Adj using BidCostAdjBidControl as the key filter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 02/20/2018 to support standard content management processing
    ''' Modified by RHr for v-8.5.4.001 on 07/18/2023 added logic for default take value of 50 so we get all records
    ''' </remarks>
    Public Function GetBidCostAdjustments(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblBidCostAdj()
        If filters Is Nothing Then Return Nothing

        Dim oRet() As LTS.tblBidCostAdj

        Dim BidCostAdjBidControl As Integer
        If filters.filterName <> "BidCostAdjBidControl" Then Return Nothing 'BidCostAdjBidControl is required
        Integer.TryParse(filters.filterValue, BidCostAdjBidControl)
        filters.take = 50
        Dim dblCarrierCostUpchargeLimitVisibility = GetParValue("CarrierCostUpchargeLimitVisibility", Me.Parameters.CompControl)
        Dim blnRestrict As Boolean = False
        If (New NGLSecurityDataProvider(Parameters).isUserRateShopRestricted(Me.Parameters.UserName, dblCarrierCostUpchargeLimitVisibility)) Then
            blnRestrict = True
        End If
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblBidCostAdj)
                Dim iCostAdjments() As Integer = {2, 4, 5, 6}

                If BidCostAdjBidControl > 0 Then
                    If blnRestrict Then
                        iQuery = db.tblBidCostAdjs.Where(Function(x) x.BidCostAdjBidControl = BidCostAdjBidControl AndAlso iCostAdjments.Contains(x.BidCostAdjTypeControl))
                    Else
                        iQuery = db.tblBidCostAdjs.Where(Function(x) x.BidCostAdjBidControl = BidCostAdjBidControl)
                    End If

                Else
                    Return Nothing
                End If
                If iQuery Is Nothing Then Return Nothing
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBidCostAdjustments"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Return the array of bid cost adjustment records by bid control
    ''' </summary>
    ''' <param name="iBidControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.2.0.117 on 9/4/2019
    '''   provides  quick access to the data using the parent key
    ''' </remarks>
    Public Function GetBidCostAdjustments(ByVal iBidControl As Integer) As LTS.tblBidCostAdj()

        Dim oRet() As LTS.tblBidCostAdj
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                oRet = db.tblBidCostAdjs.Where(Function(x) x.BidCostAdjBidControl = iBidControl).ToArray()

            Catch ex As Exception
                Logger.Error(ex, "Error in GetBidCostAdjustments")
                ManageLinqDataExceptions(ex, buildProcedureName("GetBidCostAdjustments"))
            End Try

            Return oRet

        End Using
    End Function


    ''' <summary>
    ''' Returns tblBidSvcErr() filtered by filterWhere if provided and
    ''' sorted by sortExpression if provided
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 3/1/17 for v-8.0 Next Stop
    ''' Do not use this method use GetBidServiceErrors(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) overload
    ''' this function is left for backward compatibility and may be removed int he future
    ''' </remarks>
    Public Function GetBidServiceErrors(ByRef RecordCount As Integer,
                                          Optional ByVal filterWhere As String = "",
                                          Optional ByVal sortExpression As String = "",
                                          Optional ByVal page As Integer = 1,
                                          Optional ByVal pagesize As Integer = 1000,
                                          Optional ByVal skip As Integer = 0,
                                          Optional ByVal take As Integer = 0) As LTS.tblBidSvcErr()
        Dim oRetData As LTS.tblBidSvcErr()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim intPageCount As Integer = 1

                Dim oQuery = From t In db.tblBidSvcErrs
                             Select t
                '"(CarrTarDiscountMinValue < 75) AND (CarrTarDiscountWgtLimit > 50)"
                '"(BidStatus = 1) AND (BidCarrierControl = CarrierControl)"
                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If

                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()

                Return oRetData

            Catch ex As Exception
                Logger.Error(ex, "Error in GetBidServiceErrors")
                ManageLinqDataExceptions(ex, buildProcedureName("GetBidServiceErrors"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' This overload uses the AllFilters model data to get all Bid Svc Errs using BidSvcErrBidControl as the key filter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 02/20/2018 to support standard content management processing
    '''  Modified by RHr for v-8.5.4.001 on 07/18/2023 added logic for default take value of 50 so we get all records
    ''' </remarks>
    Public Function GetBidServiceErrors(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblBidSvcErr()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblBidSvcErr
        Dim BidSvcErrBidControl As Integer
        If filters.filterName <> "BidSvcErrBidControl" Then Return Nothing
        Integer.TryParse(filters.filterValue, BidSvcErrBidControl)
        filters.take = 50
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.tblBidSvcErr)
                If BidSvcErrBidControl > 0 Then
                    iQuery = db.tblBidSvcErrs.Where(Function(x) x.BidSvcErrBidControl = BidSvcErrBidControl)
                Else
                    Return Nothing
                End If
                If iQuery Is Nothing Then Return Nothing

                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()

                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBidServiceErrors"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Sub CarrierDeleteBid(ByVal BidControl As Integer, Optional ByVal UserName As String = "")
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                Dim oLTS = (From d In db.tblBids Where d.BidControl = BidControl).FirstOrDefault()
                If Not oLTS Is Nothing AndAlso oLTS.BidControl > 0 Then
                    With oLTS
                        .BidArchived = True
                        .BidStatusCode = BSCEnum.CarDeleted
                        If Not String.IsNullOrWhiteSpace(UserName) Then
                            .BidModUser = UserName
                        End If
                        .BidModDate = Date.Now
                    End With
                    db.SubmitChanges()
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("CarrierDeleteBid"))
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Returns an array of LTS.tblBid data objects representing active/pending 
    ''' Next Stop Bids posted by the Carrier
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 1/4/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function GetBids(ByRef RecordCount As Integer,
                                     Optional ByVal filterWhere As String = "",
                                      Optional ByVal sortExpression As String = "",
                                      Optional ByVal page As Integer = 1,
                                      Optional ByVal pagesize As Integer = 1000,
                                      Optional ByVal skip As Integer = 0,
                                      Optional ByVal take As Integer = 0) As LTS.tblBid()
        Dim oRetData As LTS.tblBid()
        Using Logger.StartActivity("GetBids(RecordCount: {RecordCount}, filterWhere: {FilterWhere}, sortExpression: {SortExpression})", RecordCount, filterWhere, sortExpression)
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Try

                    Dim intPageCount As Integer = 1

                    Dim oQuery = From t In db.tblBids
                                 Select t
                    '"(CarrTarDiscountMinValue < 75) AND (CarrTarDiscountWgtLimit > 50)"
                    '"(BidStatus = 1) AND (BidCarrierControl = CarrierControl)"
                    If oQuery Is Nothing Then Return Nothing
                    If Not String.IsNullOrEmpty(filterWhere) Then
                        oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                    End If

                    RecordCount = oQuery.Count()
                    If RecordCount < 1 Then Return Nothing

                    If take <> 0 Then
                        pagesize = take
                    Else
                        'calculate based on page and pagesize
                        If pagesize < 1 Then pagesize = 1
                        If RecordCount < 1 Then RecordCount = 1
                        If page < 1 Then page = 1
                        skip = (page - 1) * pagesize
                    End If
                    If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                    oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()

                    Return oRetData

                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetBids"))
                End Try

                Return Nothing
            End Using

        End Using
    End Function

    ''' <summary>
    ''' This overload uses the AllFilters model data to get all bids using BidLoadTenderControl as the key filter
    ''' BidLoadTenderControl must be passed in as filters.ParentControl
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1 on 02/20/2018 to support standard content management processing
    ''' Modified By LVV on 3/18/19 - changed logic so BidLoadTenderControl is now passed in as filters.ParentControl
    ''' instead of as part of the array filters.FilterValues. This makes it easier to deal with the user applied filters in the client
    ''' because BidLoadTenderControl as a filter will always be hidden from the users. Keeps backend and frontend filters separate
    ''' </remarks>
    Public Function GetBids(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblBid()
        Using Logger.StartActivity("GetBids(RecordCount: {RecordCount}, filters: {filters})", RecordCount, filters)
            If filters Is Nothing Then Return Nothing
            If filters.ParentControl = 0 Then Return Nothing 'BidLoadTenderControl is required
            Dim oRet() As LTS.tblBid
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Try
                    Dim iQuery As IQueryable(Of LTS.tblBid)
                    iQuery = db.tblBids.Where(Function(x) x.BidLoadTenderControl = filters.ParentControl)
                    If String.IsNullOrWhiteSpace(filters.sortName) Then
                        filters.sortName = "BidTotalCost"
                        filters.sortDirection = "asc"
                    End If
                    Dim filterWhere = ""
                    ApplyAllFilters(iQuery, filters, filterWhere)
                    PrepareQuery(iQuery, filters, RecordCount)
                    db.Log = New DebugTextWriter
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                    Return oRet
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetBids"))
                End Try
                Return Nothing
            End Using
        End Using

    End Function

    ''' <summary>
    ''' This overload uses the AllFilters model data to get all bids using BidLoadTenderControl as the key filter where the total cost is greater than 0
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.1,1,1 on 06/06/2018 to support standard content management processing
    ''' </remarks>
    Public Function GetNonZeroBids(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblBid()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblBid
        Dim BidLoadTenderControl As Integer
        If filters.filterName <> "BidLoadTenderControl" Then Return Nothing 'BidLoadTenderControl is required
        Integer.TryParse(filters.filterValue, BidLoadTenderControl)
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblBid)
                Dim dShowNonZero As Double = 0
                dShowNonZero = Me.GetParValueByLegalEntity("RatingDefaultShowNonZeroHistBids", Me.Parameters.UserLEControl)
                If BidLoadTenderControl > 0 Then
                    If dShowNonZero = 1 Then
                        iQuery = db.tblBids.Where(Function(x) x.BidLoadTenderControl = BidLoadTenderControl)
                    Else
                        iQuery = db.tblBids.Where(Function(x) x.BidLoadTenderControl = BidLoadTenderControl And x.BidTotalCost > 0)
                    End If
                Else
                    Return Nothing
                End If
                If iQuery Is Nothing Then Return Nothing
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetBids"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' This overload uses the AllFilters model data to get all Active bids using BidLoadTenderControl as the key filter 
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 11/7/18
    '''  Created an overload for GetBids for use by content management
    '''  Plus this is better code anyway
    ''' Modified by LVV on 8/15/19
    '''  Added code to filter out bids where TotalCost is 0 (Approved by Ari - too many zero cost bids)
    ''' </remarks>
    Public Function GetActiveBidsById(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.tblBid()
        If filters Is Nothing Then Return Nothing
        If filters.filterName <> "BidLoadTenderControl" Then Return Nothing 'BidLoadTenderControl is required
        Dim iBidLoadTenderControl As Integer
        Integer.TryParse(filters.filterValue, iBidLoadTenderControl)
        If iBidLoadTenderControl < 1 Then Return Nothing
        Dim oRet() As LTS.tblBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblBid)
                iQuery = db.tblBids.Where(Function(x) x.BidStatusCode = BSCEnum.Active AndAlso x.BidLoadTenderControl = iBidLoadTenderControl AndAlso x.BidTotalCost > 0) 'Modified by LVV on 8/15/19 - Added code to filter out bids where TotalCost is 0 (Approved by Ari - too many zero cost bids)
                If iQuery Is Nothing Then Return Nothing
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                Logger.Error(ex, "Error in GetActiveBidsById")
                ManageLinqDataExceptions(ex, buildProcedureName("GetActiveBidsById"))
            End Try
            Return Nothing
        End Using
    End Function


    ''' <summary>
    ''' Returns an array of LTS.tblBid data objects representing  
    ''' Next Stop Bids that are Archived and that do not have BidStatusCode Active
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 1/4/17 for v-8.0 Next Stop
    ''' </remarks>
    Public Function GetNSHisoricalBids(ByRef RecordCount As Integer,
                                     Optional ByVal filterWhere As String = "",
                                      Optional ByVal sortExpression As String = "",
                                      Optional ByVal page As Integer = 1,
                                      Optional ByVal pagesize As Integer = 1000,
                                      Optional ByVal skip As Integer = 0,
                                      Optional ByVal take As Integer = 0) As LTS.tblBid()
        Dim oRetData As LTS.tblBid()
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try

                Dim intPageCount As Integer = 1

                Dim oQuery = From t In db.tblBids
                             Where (t.BidArchived = True AndAlso t.BidStatusCode <> BSCEnum.Active)
                             Select t
                '"(CarrTarDiscountMinValue < 75) AND (CarrTarDiscountWgtLimit > 50)"
                '"(BidStatus = 1) AND (BidCarrierControl = CarrierControl)"
                If oQuery Is Nothing Then Return Nothing
                If Not String.IsNullOrEmpty(filterWhere) Then
                    oQuery = DLinqUtil.filterWhere(oQuery, filterWhere)
                End If

                RecordCount = oQuery.Count()
                If RecordCount < 1 Then Return Nothing

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If RecordCount < 1 Then RecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If RecordCount > pagesize Then intPageCount = ((RecordCount - 1) \ pagesize) + 1
                oRetData = oQuery.OrderBy(sortExpression).Skip(skip).Take(pagesize).ToArray()

                Return oRetData

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNSHisoricalBids"))
            End Try

            Return Nothing

        End Using
    End Function

    ''' <summary>
    ''' Returns an array of LTS.tblBid data objects representing  
    ''' Next Stop Bids that are Archived and that do not have BidStatusCode Active
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added By LVV on 10/18/18
    '''  Created an replacement for GetNSHisoricalBids for use by content management
    '''  Plus this is better code anyway
    ''' </remarks>
    Public Function GetNSCarHisoricalBids(ByRef RecordCount As Integer, ByVal CarrierControl As Integer, ByVal filters As Models.AllFilters) As LTS.vBid()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.vBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.vBid)
                iQuery = From t In db.vBids
                         Where (t.BidArchived = True AndAlso t.BidStatusCode <> BSCEnum.Active AndAlso t.BidBidTypeControl = BidTypeEnum.NextStop AndAlso t.BidCarrierControl = CarrierControl)
                         Select t

                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNSCarHisoricalBids"), db)
            End Try
        End Using
        Return Nothing
    End Function

    Public Function GetNSCarrPendingBids(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.tblBid()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.tblBid
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'Get the data iqueryable
                Dim iQuery As IQueryable(Of LTS.tblBid)
                iQuery = From t In db.tblBids
                         Where (t.BidArchived = False AndAlso t.BidStatusCode = BSCEnum.Active AndAlso t.BidBidTypeControl = BidTypeEnum.NextStop AndAlso t.BidCarrierControl = Parameters.UserCarrierControl)
                         Select t

                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetNSCarrPendingBids"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' This overload uses the AllFilters model data to get all bids using BidLoadTenderControl as the key filter
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="RecordCount"></param>
    ''' <returns></returns>
    Public Function GetvBids(ByVal filters As Models.AllFilters, ByRef RecordCount As Integer) As LTS.vBid()
        Using operation = Logger.StartActivity("GetvBids(filters: {Filters}, RecordCount: {RecordCount})", filters, RecordCount)
            If filters Is Nothing Then Return Nothing
            Dim oRet() As LTS.vBid
            Dim BidLoadTenderControl As Integer
            If filters.filterName <> "BidLoadTenderControl" Then Return Nothing 'BidLoadTenderControl is required
            Integer.TryParse(filters.filterValue, BidLoadTenderControl)
            Using db As New NGLMASIntegrationDataContext(ConnectionString)
                Try
                    'Get the data iqueryable
                    Dim iQuery As IQueryable(Of LTS.vBid)
                    If BidLoadTenderControl > 0 Then
                        iQuery = db.vBids.Where(Function(x) x.BidLoadTenderControl = BidLoadTenderControl)
                    Else
                        Return Nothing
                    End If
                    If iQuery Is Nothing Then Return Nothing
                    Dim filterWhere = ""
                    ApplyAllFilters(iQuery, filters, filterWhere)
                    PrepareQuery(iQuery, filters, RecordCount)
                    oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                    Return oRet
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("GetvBids"))
                End Try

                Return Nothing
            End Using

        End Using
    End Function


    Public Function GetBidType(ByVal BidControl As Integer, Optional blnIgnoreErrors As Boolean = True) As Integer
        Dim iRet As Integer = 0
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                iRet = db.tblBids.Where(Function(x) x.BidControl = BidControl).Select(Function(x) x.BidBidTypeControl).FirstOrDefault()
            Catch ex As Exception
                'do nothing just return zero as default
                If Not blnIgnoreErrors Then
                    ManageLinqDataExceptions(ex, buildProcedureName("GetBidType"))
                End If
            End Try

            Return iRet

        End Using
    End Function

    ''' <summary>
    ''' Update the BidSelectedForExport flag 
    ''' </summary>
    ''' <param name="iBidControl"></param>
    ''' <param name="blnSelected"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.001 on 07/15/2023
    ''' </remarks>
    Public Function UpdateBidSelectedForExport(ByVal iBidControl As Integer, ByVal blnSelected As Boolean) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'verify the records exist
                Dim oData = db.tblBids.Where(Function(x) x.BidControl = iBidControl).FirstOrDefault()
                If oData Is Nothing OrElse oData.BidControl = 0 Then Return False 'nothing to do
                Dim iLoadTenderControl As Integer = oData.BidLoadTenderControl
                If (blnSelected) Then
                    'if the flag is true the parent load tender data must also be true
                    If db.tblLoadTenders.Any(Function(x) x.LoadTenderControl = iLoadTenderControl AndAlso (x.LTSelectedForExport Is Nothing OrElse x.LTSelectedForExport = False)) Then
                        Dim oLoadTender As LTS.tblLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = iLoadTenderControl).FirstOrDefault()
                        oLoadTender.LTSelectedForExport = True
                    End If
                Else
                    'check if this is the last true export being set to false.  if so the parent load tender must also be set to false
                    If db.tblBids.Any(Function(x) x.BidLoadTenderControl = iLoadTenderControl AndAlso (x.BidControl <> iBidControl) AndAlso (x.BidSelectedForExport = True)) Then
                        Dim oLoadTender As LTS.tblLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = iLoadTenderControl).FirstOrDefault()
                        oLoadTender.LTSelectedForExport = False
                    End If
                End If
                oData.BidSelectedForExport = blnSelected
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateBidSelectedForExport"), db)
            End Try
        End Using
        Return blnRet
    End Function


    ''' <summary>
    ''' Update all the BidSelectedForExport flag for the Load Tender data
    ''' </summary>
    ''' <param name="iLoadTenderControl"></param>
    ''' <param name="blnSelected"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.5.4.001 on 07/15/2023
    ''' </remarks>
    Public Function UpdateAllBidSelectedForExport(ByVal iLoadTenderControl As Integer, ByVal blnSelected As Boolean) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASIntegrationDataContext(ConnectionString)
            Try
                'verify the records exist
                Dim oData = db.tblBids.Where(Function(x) x.BidLoadTenderControl = iLoadTenderControl).ToArray()
                If oData Is Nothing OrElse oData.Count() < 1 Then Return False 'nothing to do
                For Each d In oData
                    d.BidSelectedForExport = blnSelected
                Next
                Dim oLoadTender As LTS.tblLoadTender = db.tblLoadTenders.Where(Function(x) x.LoadTenderControl = iLoadTenderControl).FirstOrDefault()
                oLoadTender.LTSelectedForExport = blnSelected
                db.SubmitChanges()
                blnRet = True
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("UpdateAllBidSelectedForExport"), db)
            End Try
        End Using
        Return blnRet
    End Function


#End Region

#Region "Protected Functions"

#End Region

End Class


